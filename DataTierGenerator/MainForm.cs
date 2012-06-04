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

using TotalSafety.DataTierGenerator.Common;
using TotalSafety.DataTierGenerator.Factory;
using TotalSafety.DataTierGenerator.MVP;

namespace TotalSafety.DataTierGenerator
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
        private SchemaGenerator m_SchemaGenerator = null;

        List<Table> m_TableList;

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

        #region exceptions
        static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if ((e.ExceptionObject is Exception))
            {
                MessageBox.Show(((Exception)e.ExceptionObject).Message);
            }
        }

        static void ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message);
        }
        #endregion

        #region main
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(ThreadException);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.Run(new MainForm());
        }
        #endregion

        #region event handlers

        private void MainForm_Load(object sender, EventArgs e)
        {
            m_SqlConnectionSettingsModel = new SqlConnectionSettingsModel();
            m_MiscSettingsModel = new MiscSettingsModel();

            LoadProject(new Project());

            m_TableList = new List<Table>();

            LoadPreviousValues();

            if (m_SqlConnectionSettingsModel.DatabaseName != "")
            {
                GenerateProject();
            }

            UpdateTitle();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //StoreApplicationValues();
        }

        #region menu events

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAs();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void m_GuiConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Shell dlg = new Shell();
            dlg.ClientSize = ConfigurationViewControl.Size;
            dlg.Controls.Add(ConfigurationViewControl);
            ConfigurationViewControl.Dock = DockStyle.Fill;
            dlg.ShowDialog();

            //MiscSettings miscSettingsDlg = new MiscSettings();
            //miscSettingsDlg.Model = m_MiscSettingsModel;

            //miscSettingsDlg.ShowDialog(this);
            //UpdateTitle();
        }

        private void generateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GenerateProject();
        }

        private void m_GuiOpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenProjectFromOpenDialog();
        }

        private void projectToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            NewProjectWizard wizard = new NewProjectWizard();

            if (wizard.ShowDialog(this) == DialogResult.OK)
            {
                Project project = wizard.Project;
                if (project != null)
                {
                    LoadProject(project);
                }
            }
        }

        private void extractToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_Project != null && m_IsProjectLoaded)
            {
                m_SchemaGenerator = new SchemaGenerator(m_Project.ConnectionString);
                m_SchemaGenerator.ExtractSchema();

                List<string> noBuildTables = new List<string>();
                foreach (Table t in m_Project.TableList)
                {
                    if (!t.BuildClass)
                    {
                        noBuildTables.Add(t.Name);
                    }
                }

                m_Project.TableList.Clear();
                m_Project.TableList.AddRange(m_SchemaGenerator.TableList);
                foreach (Table t in m_Project.TableList)
                {
                    if (noBuildTables.Contains(t.Name))
                    {
                        t.BuildClass = false;
                    }
                    else
                    {
                        t.BuildClass = true;
                    }
                }

                LoadTree();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_Project.Save();
            saveToolStripMenuItem.Enabled = false;
            UpdateTitle();
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

        void m_Project_Changed(object sender, ChangedEventArgs<Project> e)
        {
            UpdateApplicationFeaturesBasedOnRecentProjectChanges();
        }

        void m_ConfigurationViewControl_ConfigurationChanged(object sender, Controls.ConfigurationView.ChangedEventArgs e)
        {
            m_Project.DbProviderType = e.DbProvider;
            m_Project.ConnectionString = e.ConnectionString;
            m_Project.Namespace = e.Namespace;
            m_Project.OutputPath = e.OutputPath;

            if (m_ConfigurationViewControl.Parent is Form)
            {
                //((Form)m_ConfigurationViewControl.Parent).Controls.Remove(m_ConfigurationViewControl);
                //((Form)m_ConfigurationViewControl.Parent).Dispose();
            }

            UpdateApplicationFeaturesBasedOnRecentProjectChanges();
        }

        #endregion

        #region private implementation

        private void GenerateProject()
        {

            try
            {
                m_GuiGenerateToolStripMenuItem.Enabled = false;

                DalProjectGenerator dpg = new DalProjectGenerator();

                dpg.DalNamespace = m_Project.Namespace;
                dpg.DalProjectDirectory = m_Project.OutputPath;

                foreach (Table table in m_Project.TableList)
                {
                    if (table.BuildClass)
                    {
                        dpg.TableList.Add(table);
                    }
                }

                dpg.GenerateDalProject();

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

        private void LoadTree()
        {
            // Begin
            m_GuiProjectTree.Sorted = false;
            m_GuiProjectTree.BeginUpdate();
            m_GuiProjectTree.Nodes.Clear();

            // Add Root
            m_GuiProjectTree.Nodes.Add("Root", m_Project.ProjectName).Tag = TreeNodeTypes.Project;

            // Add Configuration
            //m_GuiProjectTree.Nodes["Root"].Nodes.Add("Configuration", "Configuration").Tag = TreeNodeTypes.Configuration;

            // Add Schema
            //m_GuiProjectTree.Nodes["Root"].Nodes.Add("Schema", "Schema").Tag = TreeNodeTypes.Schema;

            // Add Schema Tables
            //m_GuiProjectTree.Nodes["Root"].Nodes["Schema"].Nodes.Add("Tables", "Tables").Tag = TreeNodeTypes.Tables;
            m_GuiProjectTree.Nodes["Root"].Nodes.Add("Tables", "Tables").Tag = TreeNodeTypes.Tables;
            foreach (Table table in m_Project.TableList)
            {
                //AddTableNode(m_GuiProjectTree.Nodes["Root"].Nodes["Schema"].Nodes["Tables"], table);
                AddTableNode(m_GuiProjectTree.Nodes["Root"].Nodes["Tables"], table);
            }

            // Add Schema Views
            //m_GuiProjectTree.Nodes["Root"].Nodes["Schema"].Nodes.Add("Views", "Views").Tag = TreeNodeTypes.Views;
            m_GuiProjectTree.Nodes["Root"].Nodes.Add("Views", "Views").Tag = TreeNodeTypes.Views;
            foreach (Common.View view in m_Project.ViewList)
            {
                //AddViewNode(m_GuiProjectTree.Nodes["Root"].Nodes["Schema"].Nodes["Views"], view);
                AddViewNode(m_GuiProjectTree.Nodes["Root"].Nodes["Views"], view);
            }

            // Add Schema Stored Procedures
            //m_GuiProjectTree.Nodes["Root"].Nodes["Schema"].Nodes.Add("Procedures", "Procedures").Tag = TreeNodeTypes.Procedures;
            m_GuiProjectTree.Nodes["Root"].Nodes.Add("Procedures", "Procedures").Tag = TreeNodeTypes.Procedures;
            foreach (Procedure procedure in m_Project.ProcedureList)
            {
                //AddProcedureNode(m_GuiProjectTree.Nodes["Root"].Nodes["Schema"].Nodes["rocedures"], procedure);
                AddProcedureNode(m_GuiProjectTree.Nodes["Root"].Nodes["rocedures"], procedure);
            }

            // End
            m_GuiProjectTree.EndUpdate();

            m_IsProjectLoaded = true;

        }

        private void AddTableNode(TreeNode treeNnode, Table table)
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

        private void AddProcedureNode(TreeNode node, Procedure procedure)
        {
        }

        private void AddColumnNode(TreeNode treeNode, Column column)
        {

            TreeNode node;

            node = new TreeNode(column.PropertyName);
            node.Tag = TreeNodeTypes.Column;

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

            //rkey.SetValue("Server", m_SqlConnectionSettingsModel.ServerName);
            //rkey.SetValue("Database", m_SqlConnectionSettingsModel.DatabaseName);

            //if (m_SqlConnectionSettingsModel.SqlServerAuthenticationType == SqlServerAuthenticationTypeEnumeration.Windows)
            //    rkey.SetValue("AuthenticationType", "Windows");
            //else
            //    rkey.SetValue("AuthenticationType", "SQL");

            //rkey.SetValue("AuthenticationUsername", m_SqlConnectionSettingsModel.UserId);
            //rkey.SetValue("AuthenticationPassword", m_SqlConnectionSettingsModel.Password);

            //rkey.SetValue("DataLayerNamespaceTextBox", m_MiscSettingsModel.Namespace);
            //rkey.SetValue("DataLayerOutputDirectory", m_MiscSettingsModel.ProjectPathname);
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
                string filePathName = (string)rkey.GetValue("FileName");
                LoadProjectFile(filePathName);

                m_ProjectPathName = System.IO.Path.GetFullPath(filePathName);
            }

            //if (rkey.GetValue("Server") != null)
            //    m_SqlConnectionSettingsModel.ServerName = (string)rkey.GetValue("Server");

            //if (rkey.GetValue("Database") != null)
            //    m_SqlConnectionSettingsModel.DatabaseName = (string)rkey.GetValue("Database");

            //if (rkey.GetValue("AuthenticationType") != null)
            //{
            //    if ((string)rkey.GetValue("AuthenticationType") == "Windows")
            //        m_SqlConnectionSettingsModel.SqlServerAuthenticationType =
            //            SqlServerAuthenticationTypeEnumeration.Windows;
            //    else
            //        m_SqlConnectionSettingsModel.SqlServerAuthenticationType =
            //            SqlServerAuthenticationTypeEnumeration.SqlServer;
            //}

            //if (rkey.GetValue("AuthenticationUsername") != null)
            //    m_SqlConnectionSettingsModel.UserId = (string)rkey.GetValue("AuthenticationUsername");

            //if (rkey.GetValue("AuthenticationPassword") != null)
            //    m_SqlConnectionSettingsModel.Password = (string)rkey.GetValue("AuthenticationPassword");

            //if (rkey.GetValue("DataLayerNamespaceTextBox") != null)
            //    m_MiscSettingsModel.Namespace = (string)rkey.GetValue("GuiDataLayerNamespaceTextBox");

            //if ( rkey.GetValue( "DataLayerOutputDirectory" ) != null )
            //    m_MiscSettingsModel.ProjectPathname = (string)rkey.GetValue("DataLayerOutputDirectory");

        }

        private XmlDocument GenerateProjectXmlDocument()
        {

            // create the xml file
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.AppendChild(xmlDoc.CreateElement("Root"));

            SaveConnectionProperties(xmlDoc.DocumentElement);
            SaveMiscProperties(xmlDoc.DocumentElement);

            return xmlDoc;

        }

        private void UpdateApplicationFeaturesBasedOnRecentProjectChanges()
        {
            if (!m_Project.IsNew)
            {
                saveToolStripMenuItem.Enabled = true;
            }

            if (m_Project.TableList.Count > 0 || m_Project.ViewList.Count > 0 || m_Project.ProcedureList.Count > 0)
            {
                m_GuiGenerateToolStripMenuItem.Enabled = true;
            }
            else
            {
                m_GuiGenerateToolStripMenuItem.Enabled = false;
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

                LoadProjectFile(openFileDialog1.FileName);

                m_ProjectPathName = System.IO.Path.GetFullPath(openFileDialog1.FileName);

            }

        }

        private void LoadProjectFile(string filePathName)
        {
            LoadProject(Project.Load(filePathName));
        }

        private void LoadProject(Project project)
        {
            m_Project = project;
            m_Project.Changed += new EventHandler<ChangedEventArgs<Project>>(m_Project_Changed);
            LoadTree();

            if (m_Project.TableList.Count > 0 || m_Project.ViewList.Count > 0 || m_Project.ProcedureList.Count > 0)
            {
                m_GuiGenerateToolStripMenuItem.Enabled = true;
            }
            else
            {
                m_GuiGenerateToolStripMenuItem.Enabled = false;
            }
        }

        private void LoadTableFromNode(XmlNode xmlNode)
        {

            Table table;

            table = new Table(xmlNode);

            m_TableList.Add(table);

        }

        private void LoadView()
        {
        }

        private void LoadStoredProcedure()
        {
        }

        #endregion

        #region save project to file implementation

        private void SaveAs()
        {

            //if (m_ProjectXmlDoc == null){
            //    MessageBox.Show("Project must be generated before saving.");
            //}

            /*m_ProjectXmlDoc = GenerateProjectXmlDocument ();

            if (m_ProjectPathName == "") {
                saveFileDialog1.InitialDirectory = Environment.CurrentDirectory;
            }
            else {
                saveFileDialog1.InitialDirectory =
                    System.IO.Path.GetFullPath (m_ProjectPathName);
            }*/

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

        private void SaveConnectionProperties(XmlElement xmlElement)
        {

            XmlDocument xmlDoc;
            XmlElement connectionElement;
            XmlElement propertyElement;

            XmlSerializer serializer = new XmlSerializer(typeof(SqlConnectionSettingsModel));

            //serializer.Serialize(writer, m_SqlConnectionSettingsModel);
            //xmlDoc = xmlElement.OwnerDocument;

            //connectionElement = xmlDoc.CreateElement("ConnectionProperties");

            //propertyElement = xmlDoc.CreateElement("Server");
            //propertyElement.InnerText = m_SqlConnectionSettingsModel.ServerName;
            //connectionElement.AppendChild(propertyElement);

            //propertyElement = xmlDoc.CreateElement("Database");
            //propertyElement.InnerText = m_SqlConnectionSettingsModel.DatabaseName;
            //connectionElement.AppendChild(propertyElement);

            //propertyElement = xmlDoc.CreateElement("AuthenticationType");
            //propertyElement.InnerText = m_SqlConnectionSettingsModel.SqlServerAuthenticationType.ToString();
            //connectionElement.AppendChild(propertyElement);

            //if (m_SqlConnectionSettingsModel.SqlServerAuthenticationType == SqlServerAuthenticationTypeEnumeration.SqlServer) {

            //    propertyElement = xmlDoc.CreateElement("UserId");
            //    propertyElement.InnerText = m_SqlConnectionSettingsModel.UserId;
            //    connectionElement.AppendChild(propertyElement);

            //    propertyElement = xmlDoc.CreateElement("Password");
            //    propertyElement.InnerText = m_SqlConnectionSettingsModel.Password;
            //    connectionElement.AppendChild(propertyElement);

            //}

            //xmlElement.AppendChild(connectionElement);

        }

        private void SaveMiscProperties(XmlElement xmlParentElement)
        {

            XmlDocument xmlDoc;
            XmlElement xmlElement;

            xmlDoc = xmlParentElement.OwnerDocument;

            xmlElement = xmlDoc.CreateElement("Namespace");
            xmlElement.InnerText = m_MiscSettingsModel.Namespace;
            xmlDoc.DocumentElement.AppendChild(xmlElement);

            xmlElement = xmlDoc.CreateElement("GeneratedOutputPath");
            xmlElement.InnerText = m_MiscSettingsModel.GeneratedDataProjectPath;

            xmlParentElement.AppendChild(xmlElement);

        }

        private void SaveTables(XmlElement xmlParentElement)
        {

            XmlDocument xmlDoc;
            XmlElement tableGroupElement;
            XmlElement xmlElement;

            xmlDoc = xmlParentElement.OwnerDocument;

            tableGroupElement = xmlDoc.CreateElement("Tables");

            foreach (Table table in m_TableList)
            {
                xmlElement = xmlDoc.CreateElement(table.Name);

                SaveColumns(xmlElement, table);

                tableGroupElement.AppendChild(xmlElement);
            }

            xmlParentElement.AppendChild(tableGroupElement);

        }

        private void SaveColumns(XmlElement xmlParentElement, Table table)
        {

            XmlDocument xmlDoc;
            XmlElement columnGroupElement;
            XmlElement xmlElement;

            xmlDoc = xmlParentElement.OwnerDocument;

            columnGroupElement = xmlDoc.CreateElement("Columns");

            foreach (Column column in table.Columns)
            {
                xmlElement = xmlDoc.CreateElement(column.PropertyName);

                columnGroupElement.AppendChild(xmlElement);
            }

            xmlParentElement.AppendChild(columnGroupElement);

        }

        private void SaveViews(XmlElement xmlParentElement)
        {

            XmlDocument xmlDoc;
            XmlElement xmlElement;

            xmlDoc = xmlParentElement.OwnerDocument;

        }

        private void SaveStoredProcedures(XmlElement xmlParentElement)
        {

            XmlDocument xmlDoc;
            XmlElement xmlElement;

            xmlDoc = xmlParentElement.OwnerDocument;

        }

        #endregion

    }
}
