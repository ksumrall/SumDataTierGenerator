using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

using Microsoft.Win32;

using SumDataTierGenerator.Common;
using SumDataTierGenerator.CodeGenerationFactory;
using SumDataTierGenerator.MVP;
using SumDataTierGenerator.SchemaExtractor;

namespace SumDataTierGenerator
{
    /// <summary>
    /// Form used to collect the connection information for the code we're going to generate.
    /// </summary>
    public partial class MainForm : System.Windows.Forms.Form
    {

        #region private / protected member variables

        private SqlConnectionSettingsModel m_SqlConnectionSettingsModel;
        private MiscSettingsModel m_MiscSettingsModel;
        private Project m_Project = null;
        private bool m_IsProjectLoaded = false;

        private string m_LastSelectedOutputdirectory = "";
        private string m_ProjectPathName = "";
        XmlDocument m_ProjectXmlDoc;
        private SchemaExtractorWrapper m_SchemaExtractor = null;

        private const string BaseTitle = "Data Tier Generator";

        private Controls.ConfigurationView m_ConfigurationViewControl;

        #endregion

        #region constructors destructors

        public MainForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }

        #endregion

        #region

        private Controls.ConfigurationView ConfigurationViewControl
        {
            get
            {
                if (m_ConfigurationViewControl == null)
                {
                    m_ConfigurationViewControl = new Controls.ConfigurationView(m_Project);
                    m_ConfigurationViewControl.ConfigurationChanged += new EventHandler<Controls.ConfigurationView.ChangedEventArgs>(m_ConfigurationViewControl_ConfigurationChanged);
                }
                return m_ConfigurationViewControl;
            }
        }

        #endregion

        #region event handlers

        private void MainForm_Load(object sender, EventArgs e)
        {
            m_SqlConnectionSettingsModel = new SqlConnectionSettingsModel();
            m_MiscSettingsModel = new MiscSettingsModel();

            LoadPreviousValues();

            LoadProject(new Project());

            UpdateTitle();

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //StoreApplicationValues();
        }

        #region menu file events

        private void m_GuiOpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenProjectFromOpenDialog();
        }

        private void m_GuiSaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_Project.Save();
            m_GuiSaveToolStripMenuItem.Enabled = false;
            UpdateTitle();
        }

        private void m_GuiSaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAs();
        }

        private void m_GuiExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region menu project events

        private void m_GuiConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Shell dlg = new Shell();
            dlg.ClientSize = ConfigurationViewControl.Size;
            dlg.Controls.Add(ConfigurationViewControl);
            ConfigurationViewControl.Dock = DockStyle.Fill;

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                LoadTree(m_Project);
            }

        }

        private void m_GuiRefreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_Project != null && m_IsProjectLoaded)
            {
                m_SchemaExtractor = new SchemaExtractorWrapper();
                m_SchemaExtractor.ProviderType = m_Project.Configuration.DbConnectionDetails.DbProviderType;
                m_SchemaExtractor.ConnectionString = m_Project.Configuration.DbConnectionDetails.ConnectionString;
                XmlDocument xDoc = m_SchemaExtractor.GetSchemaDefinition();

                m_Project.LoadSchemasFromXml(xDoc.SelectSingleNode("schemas"));
            }

            LoadTree(m_Project);
        }

        private void m_GuiGenerateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GenerateProject();
        }

        #endregion

        #region tree node events

        private void m_GuiProjectTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (m_IsProjectLoaded)
            {
                switch ((TreeNodeTypes)e.Node.Tag)
                {
                    case TreeNodeTypes.Undefined:
                        //m_GuiSplitContainer.Panel2.Controls.Clear();
                        break;
                    case TreeNodeTypes.Project:
                        //m_GuiSplitContainer.Panel2.Controls.Clear();
                        break;
                    case TreeNodeTypes.Configuration:
                        if (!m_GuiDetailPaneTabCollection.TabPages.ContainsKey("Configuration"))
                        {
                            m_GuiDetailPaneTabCollection.TabPages.Add("Configuration", "Configuration");
                            m_GuiDetailPaneTabCollection.TabPages["Configuration"].Controls.Add(ConfigurationViewControl);
                            ConfigurationViewControl.Dock = DockStyle.Fill;
                        }
                        break;
                    case TreeNodeTypes.Schema:
                        //m_GuiSplitContainer.Panel2.Controls.Clear();
                        break;
                    case TreeNodeTypes.Tables:
                        //m_GuiSplitContainer.Panel2.Controls.Clear();
                        break;
                    case TreeNodeTypes.Views:
                        //m_GuiSplitContainer.Panel2.Controls.Clear();
                        break;
                    case TreeNodeTypes.Procedures:
                        //m_GuiSplitContainer.Panel2.Controls.Clear();
                        break;
                    case TreeNodeTypes.Columns:
                        //m_GuiSplitContainer.Panel2.Controls.Clear();
                        break;
                    case TreeNodeTypes.Table:
                        //m_GuiSplitContainer.Panel2.Controls.Clear();
                        break;
                    case TreeNodeTypes.View:
                        //m_GuiSplitContainer.Panel2.Controls.Clear();
                        break;
                    case TreeNodeTypes.Procedure:
                        //m_GuiSplitContainer.Panel2.Controls.Clear();
                        break;
                    case TreeNodeTypes.Column:
                        //m_GuiSplitContainer.Panel2.Controls.Clear();
                        break;
                }
            }
        }

        #endregion

        void m_ConfigurationViewControl_ConfigurationChanged(object sender, Controls.ConfigurationView.ChangedEventArgs e)
        {
            m_Project.Configuration.DbConnectionDetails.DbProviderType = e.DbProvider;
            m_Project.Configuration.DbConnectionDetails.ConnectionString = e.ConnectionString;
            m_Project.Configuration.CodeGenerationDetails.Namespace = e.Namespace;
            m_Project.Configuration.CodeGenerationDetails.OutputPath = e.OutputPath;

            if (m_ConfigurationViewControl.Parent is Form)
            {
                //((Form)m_ConfigurationViewControl.Parent).Controls.Remove(m_ConfigurationViewControl);
                //((Form)m_ConfigurationViewControl.Parent).Dispose();
            }

            if (m_Project != null && m_IsProjectLoaded)
            {
                m_SchemaExtractor = new SchemaExtractorWrapper();
                m_SchemaExtractor.ProviderType = m_Project.Configuration.DbConnectionDetails.DbProviderType;
                m_SchemaExtractor.ConnectionString = m_Project.Configuration.DbConnectionDetails.ConnectionString;
                XmlDocument xDoc = m_SchemaExtractor.GetSchemaDefinition();

                m_Project.LoadSchemasFromXml(xDoc.SelectSingleNode("schemas"));
            }
            LoadTree(m_Project);

            UpdateApplicationFeaturesBasedOnRecentProjectChanges();
        }

        #endregion

        #region private implementation

        private void GenerateProject()
        {

            try
            {
                this.Cursor = Cursors.WaitCursor;

                m_GuiGenerateToolStripMenuItem.Enabled = false;

                DalProjectGenerator dpg = new DalProjectGenerator(m_Project);

                dpg.GenerateDalProject();

                this.Cursor = DefaultCursor;

                MessageBox.Show("C# classes generated successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
            }
            finally
            {
                m_GuiGenerateToolStripMenuItem.Enabled = true;
            }

        }

        private void LoadTree(Project project)
        {
            TreeNode schemaCollectionTreeNode;

            // Begin
            m_GuiProjectTree.Sorted = false;
            m_GuiProjectTree.BeginUpdate();
            m_GuiProjectTree.Nodes.Clear();

            // Add Root
            m_GuiProjectTree.Nodes.Add("Root", project.ProjectName).Tag = TreeNodeTypes.Project;

            if (project.Schemas != null)
            {
                // Add Schemas
                schemaCollectionTreeNode = m_GuiProjectTree.Nodes["Root"].Nodes.Add("Schemas", "Schemas");
                schemaCollectionTreeNode.Tag = TreeNodeTypes.Schema;
                foreach (Common.Schema schema in project.Schemas)
                {
                    AddSchemaNode(schemaCollectionTreeNode, schema);
                }
            }

            // End
            m_GuiProjectTree.EndUpdate();

            m_IsProjectLoaded = true;

        }

        private void AddSchemaNode(TreeNode schemaCollectionNode, Common.Schema schema)
        {
            TreeNode node;
            TreeNode collectionNode;

            node = new TreeNode(schema.Name);
            node.Tag = TreeNodeTypes.Schema;

            #region add tables

            collectionNode = new TreeNode("Tables");
            collectionNode.Tag = TreeNodeTypes.Tables;
            node.Nodes.Add(collectionNode);

            //schema.Tables.Sort(new Comparison<Table>(Table.CompareByProgrammaticAlias));
            foreach (Common.Table t in schema.Tables)
            {
                AddTableNode(collectionNode, t);
            }

            #endregion

            #region add views

            collectionNode = new TreeNode("Views");
            collectionNode.Tag = TreeNodeTypes.Views;
            node.Nodes.Add(collectionNode);

            //schema.Views.Sort(new Comparison<Common.View>(Common.View.CompareByProgrammaticAlias));
            foreach (Common.View v in schema.Views)
            {
                AddViewNode(collectionNode, v);
            }

            #endregion

            #region add functions

            collectionNode = new TreeNode("Functions");
            collectionNode.Tag = TreeNodeTypes.Functions;
            node.Nodes.Add(collectionNode);

            //schema.Functions.Sort(new Comparison<Function>(Function.CompareByProgrammaticAlias));
            foreach (Common.Function f in schema.Functions)
            {
                AddFunctionNode(collectionNode, f);
            }

            #endregion

            #region add procedures

            collectionNode = new TreeNode("Procedures");
            collectionNode.Tag = TreeNodeTypes.Procedures;
            node.Nodes.Add(collectionNode);

            //schema.Procedures.Sort(new Comparison<Procedure>(Procedure.CompareByProgrammaticAlias));
            foreach (Common.Procedure p in schema.Procedures)
            {
                AddProcedureNode(collectionNode, p);
            }

            #endregion

            schemaCollectionNode.Nodes.Add(node);
        }

        private void AddTableNode(TreeNode treeNnode, Common.Table table)
        {
            TreeNode node;

            node = new TreeNode(table.Name);
            node.Tag = TreeNodeTypes.Table;
            //table.Columns.Sort (new Comparison<Column> (Column.CompareByProgrammaticAlias));

            foreach (Column column in table.Columns)
            {
                AddColumnNode(node, column);
            }

            treeNnode.Nodes.Add(node);
        }

        private void AddViewNode(TreeNode treeNode, Common.View view)
        {
            TreeNode node;

            node = new TreeNode(view.Name);
            node.Tag = TreeNodeTypes.View;

            foreach (Column column in view.Columns)
            {
                AddColumnNode(node, column);
            }

            treeNode.Nodes.Add(node);
        }

        private void AddFunctionNode(TreeNode treeNode, Common.Function function)
        {
            TreeNode node;

            node = new TreeNode(function.Name);
            node.Tag = TreeNodeTypes.Function;

            foreach (Common.Parameter parameter in function.Parameters)
            {
                AddParameterNode(node, parameter);
            }

            treeNode.Nodes.Add(node);
        }

        private void AddProcedureNode(TreeNode treeNode, Common.Procedure procedure)
        {
            TreeNode node;

            node = new TreeNode(procedure.Name);
            node.Tag = TreeNodeTypes.Function;

            foreach (Common.Parameter parameter in procedure.Parameters)
            {
                AddParameterNode(node, parameter);
            }

            treeNode.Nodes.Add(node);
        }

        private void AddColumnNode(TreeNode treeNode, Column column)
        {

            TreeNode node;

            node = new TreeNode(column.PropertyName);
            node.Tag = TreeNodeTypes.Column;

            treeNode.Nodes.Add(node);
        }

        private void AddParameterNode(TreeNode treeNode, Common.Parameter parameter)
        {

            TreeNode node;

            node = new TreeNode(parameter.PropertyName);
            node.Tag = TreeNodeTypes.Parameter;

            treeNode.Nodes.Add(node);
        }

        private void GetOutputDirectory(TextBox txt)
        {

            System.Windows.Forms.FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Select an output folder for the created classes.";
            fbd.ShowNewFolderButton = true;

            if (txt.Text.Length == 0)
            {
                if (m_LastSelectedOutputdirectory.Length > 0)
                {
                    fbd.SelectedPath = m_LastSelectedOutputdirectory;
                }
            }
            else
            {
                fbd.SelectedPath = txt.Text;
            }

            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                m_LastSelectedOutputdirectory = fbd.SelectedPath;
                txt.Text = m_LastSelectedOutputdirectory;
            }

            fbd.Dispose();

        }

        private void StoreApplicationValues(string projectFileName)
        {
            RegistryKey rkey = Registry.CurrentUser.OpenSubKey("Software\\TotalSafety\\DataTierGenerator", true);

            rkey.SetValue("Xpos", this.Left);
            rkey.SetValue("Ypos", this.Top);
            rkey.SetValue("FileName", projectFileName);

        }

        private void LoadPreviousValues()
        {
            RegistryKey rkey = Registry.CurrentUser.OpenSubKey("Software\\TotalSafety\\DataTierGenerator");

            if (rkey == null)
            {
                rkey = Registry.CurrentUser.CreateSubKey("Software\\TotalSafety\\DataTierGenerator");
            }

            if (rkey.GetValue("Xpos") != null)
                this.Left = (int)rkey.GetValue("Xpos");

            if (rkey.GetValue("Ypos") != null)
                this.Top = (int)rkey.GetValue("Ypos");

            if (rkey.GetValue("FileName") != null)
            {
                //string filePathName = (string)rkey.GetValue("FileName");
                //LoadProject(Project.Load(filePathName));

                //m_ProjectPathName = System.IO.Path.GetFullPath(filePathName);
            }

        }

        private void UpdateApplicationFeaturesBasedOnRecentProjectChanges()
        {
            if (!m_Project.IsNew)
            {
                m_GuiSaveToolStripMenuItem.Enabled = true;
            }

            m_GuiGenerateToolStripMenuItem.Enabled = false;

            if (m_Project.Schemas != null)
            {
                foreach (Schema schema in m_Project.Schemas)
                {
                    if (schema.Tables.Length > 0 || schema.Views.Length > 0 || schema.Functions.Length > 0 || schema.Procedures.Length > 0)
                    {
                        m_GuiGenerateToolStripMenuItem.Enabled = true;
                        break;
                    }
                }
            }

            UpdateTitle();
        }

        private void UpdateTitle()
        {
            if (m_Project == null)
            {
                this.Text = BaseTitle;
            }
            else
            {
                if (m_Project.IsNew)
                {
                    this.Text = String.Format("New Project - {0}", BaseTitle);
                }
                else
                {
                    this.Text = String.Format("{0} - {1}", Path.GetFileName(m_Project.FilePath), BaseTitle);
                }

                if (m_Project.IsDirty)
                {
                    this.Text += " *";
                }
            }
        }

        #endregion

        #region load project implementation

        private void OpenProjectFromOpenDialog()
        {

            if (m_ProjectPathName == "")
            {
                openFileDialog1.InitialDirectory = Environment.CurrentDirectory;
            }
            else
            {
                openFileDialog1.InitialDirectory =
                    System.IO.Path.GetFullPath(m_ProjectPathName);
            }

            openFileDialog1.FileName = "";
            openFileDialog1.DefaultExt = "dtgproj";
            openFileDialog1.Filter = "DTG Project Files (*.dtgproj)|*.dtgproj|All files (*.*)|*.*";

            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                LoadProject(Project.Load(openFileDialog1.FileName));

                m_ProjectPathName = System.IO.Path.GetFullPath(openFileDialog1.FileName);
            }

        }

        private void LoadProject(Project project)
        {
            m_Project = project;
            m_GuiGenerateToolStripMenuItem.Enabled = false;

            LoadTree(m_Project);

            if (project.Schemas != null)
            {
                foreach (Schema schema in project.Schemas)
                {
                    if (schema.Tables.Length > 0 || schema.Views.Length > 0 || schema.Functions.Length > 0 || schema.Procedures.Length > 0)
                    {
                        m_GuiGenerateToolStripMenuItem.Enabled = true;
                        break;
                    }
                }
            }
        }

        #endregion

        #region save project to file implementation

        private void SaveAs()
        {

            saveFileDialog1.OverwritePrompt = true;
            saveFileDialog1.DefaultExt = "dtgproj";
            saveFileDialog1.Filter = "DTG Project Files (*.dtgproj)|*.dtgproj|All files (*.*)|*.*";

            if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                m_ProjectPathName = saveFileDialog1.FileName;
                SaveProjectFile(saveFileDialog1.FileName);
            }
        }

        private void SaveProjectFile(string filePathName)
        {
            m_Project.Save(filePathName);
        }

        #endregion

    }
}
