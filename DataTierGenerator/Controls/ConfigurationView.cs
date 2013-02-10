using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Microsoft.Data.ConnectionUI;
using SumDataTierGenerator.Common;
using SumDataTierGenerator.MVP;

namespace SumDataTierGenerator.Controls
{
    public partial class ConfigurationView : UserControl
    {
        public class ChangedEventArgs : EventArgs
        {
            public ChangedEventArgs(string dbProvider, string connectionString, string _namespace, string outputPath)
            {
                m_DbProvider = dbProvider;
                m_ConnectionString = connectionString;
                m_Namespace = _namespace;
                m_OutputPath = outputPath;
            }

            private string m_DbProvider;
            public string DbProvider
            {
                get { return m_DbProvider; }
            }

            private string m_ConnectionString;
            public string ConnectionString
            {
                get { return m_ConnectionString; }
            }

            private string m_Namespace;
            public string Namespace
            {
                get { return m_Namespace; }
            }

            private string m_OutputPath;
            public string OutputPath
            {
                get { return m_OutputPath; }
            }

        }

        public event EventHandler<ChangedEventArgs> ConfigurationChanged;

        private string m_Original_DbProvider;
        private string m_Original_ConnectionString;
        private string m_Original_Namespace;
        private string m_Original_OutputPath;

        public ConfigurationView(Project project)
        {
            InitializeComponent();

            m_Original_DbProvider = m_GuiDbProviderTextBox.Text = project.Configuration.DbConnectionDetails.DbProviderType;
            m_Original_ConnectionString = m_GuiConnectionStringTextBox.Text = project.Configuration.DbConnectionDetails.ConnectionString;
            m_Original_Namespace = m_GuiNamespaceTextBox.Text = project.Configuration.CodeGenerationDetails.Namespace;
            m_Original_OutputPath = m_GuiOutputPathTextBox.Text = project.Configuration.CodeGenerationDetails.OutputPath;
        }

        #region events

        private void m_GuiConnectionDetailsButton_Click(object sender, EventArgs e)
        {
            GetConnectionSettings();
        }

        private void m_GuiOutputPathButton_Click(object sender, EventArgs e)
        {
            GetOutputDirectory();
        }

        private void m_GuiOkButton_Click(object sender, EventArgs e)
        {
            if (ConfigurationChanged != null)
            {
                ConfigurationChanged(
                    this
                    , new ChangedEventArgs(
                        m_GuiDbProviderTextBox.Text
                        , m_GuiConnectionStringTextBox.Text
                        , m_GuiNamespaceTextBox.Text
                        , m_GuiOutputPathTextBox.Text
                    )
                );

                m_Original_DbProvider = m_GuiDbProviderTextBox.Text;
                m_Original_ConnectionString = m_GuiConnectionStringTextBox.Text;
                m_Original_Namespace = m_GuiNamespaceTextBox.Text;
                m_Original_OutputPath = m_GuiOutputPathTextBox.Text;

                ChangeStateToClean();

            }
        }

        private void m_GuiCancelButton_Click(object sender, EventArgs e)
        {
            m_GuiDbProviderTextBox.Text = m_Original_DbProvider;
            m_GuiConnectionStringTextBox.Text = m_Original_ConnectionString;
            m_GuiNamespaceTextBox.Text = m_Original_Namespace;
            m_GuiOutputPathTextBox.Text = m_Original_OutputPath;

            if (Parent is Form)
            {
                ((Form)Parent).Close();
            }
            else
            {
                ChangeStateToClean();
            }
        }

        private void m_GuiNamespaceTextBox_TextChanged(object sender, EventArgs e)
        {
            ChangeStateToDirty();
        }

        #endregion

        #region private implementation

        private void GetConnectionSettings()
        {
            DataConnectionDialog dcd = new DataConnectionDialog();
            DataConnectionConfiguration dcs = new DataConnectionConfiguration(null);
            dcs.LoadConfiguration(dcd);

            if (DataConnectionDialog.Show(dcd) == DialogResult.OK)
            {
                #region set the text boxes according to the connection settings
                if (dcd.SelectedDataSource != null)
                {
                    if (dcd.SelectedDataSource == dcd.UnspecifiedDataSource)
                    {
                        if (dcd.SelectedDataProvider != null)
                        {
                            m_GuiDbProviderTextBox.Text = dcd.SelectedDataProvider.DisplayName;
                        }
                        else
                        {
                            m_GuiDbProviderTextBox.Text = null;
                        }
                    }
                    else
                    {
                        m_GuiDbProviderTextBox.Text = dcd.SelectedDataSource.DisplayName;
                        if (dcd.SelectedDataProvider != null)
                        {
                            if (dcd.SelectedDataProvider.ShortDisplayName != null)
                            {
                                m_GuiDbProviderTextBox.Text = String.Format("{0} ({1})", m_GuiDbProviderTextBox.Text, dcd.SelectedDataProvider.ShortDisplayName);
                            }
                        }
                    }
                }
                else
                {
                    m_GuiDbProviderTextBox.Text = null;
                }

                m_GuiConnectionStringTextBox.Text = dcd.ConnectionString;

                ChangeStateToDirty();
                #endregion
            }
        }

        private void GetOutputDirectory()
        {

            System.Windows.Forms.FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Select an output folder for the created classes.";
            fbd.ShowNewFolderButton = true;

            if (m_GuiOutputPathTextBox.Text.Length > 0)
            {
                fbd.SelectedPath = m_GuiOutputPathTextBox.Text;
            }

            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                m_GuiOutputPathTextBox.Text = fbd.SelectedPath;
            }

            fbd.Dispose();

        }

        private void ChangeStateToDirty()
        {
            m_GuiOkButton.Enabled = true;
            m_GuiCancelButton.Text = "&Cancel";
        }

        private void ChangeStateToClean()
        {
            m_GuiOkButton.Enabled = false;
            m_GuiCancelButton.Text = "&Close";
        }

        #endregion
    }
}
