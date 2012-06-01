using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Microsoft.Data.ConnectionUI;
using TotalSafety.DataTierGenerator.MVP;

namespace TotalSafety.DataTierGenerator
{
    public partial class MiscSettings : Form
    {

        #region private and protected member variables

        private string m_LastSelectedOutputdirectory = "";
        private string m_DbConnectionType;

        #endregion

        #region internal structured members
        #endregion

        #region constructors / desturctors

        public MiscSettings()
        {
            InitializeComponent();
        }

        #endregion

        #region public properties

        public MiscSettingsModel Model
        {
            set
            {
                miscSettingsModelBindingSource.Add(value);
            }
        }

        #endregion

        #region event handlers / overrides

        private void m_GuiConnectionStringDialogButton_Click(object sender, EventArgs e)
        {
            GetConnectionDetails();
        }

        private void m_GuiDataLayerOutputDirectoryButton_Click(object sender, EventArgs e)
        {
            GetOutputDirectory();
        }

        private void m_GuiCancelButton_Click(object sender, EventArgs e)
        {

            miscSettingsModelBindingSource.CancelEdit();

        }

        private void m_GuiOkButton_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region public methods

        #endregion

        #region private implementation

        private void GetConnectionDetails()
        {
            DataConnectionDialog dcd = new DataConnectionDialog();
            DataConnectionConfiguration dcs = new DataConnectionConfiguration(null);
            dcs.LoadConfiguration(dcd);
            DataConnectionDialog.Show(dcd);

            m_GuiDbProviderTextBox.Text = dcd.SelectedDataProvider.Name;
            m_GuiConnectionString.Text = dcd.ConnectionString;

            //m_DbConnectionType = dcd.SelectedDataProvider.TargetConnectionType.ToString();

            miscSettingsModelBindingSource.EndEdit();

        }

        private void GetOutputDirectory()
        {

            System.Windows.Forms.FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Select an output folder for the created classes.";
            fbd.ShowNewFolderButton = true;

            if (m_GuiDataLayerOutputDirectory.Text.Length == 0)
            {
                if (m_LastSelectedOutputdirectory.Length > 0)
                {
                    fbd.SelectedPath = m_LastSelectedOutputdirectory;
                }
            }
            else
            {
                fbd.SelectedPath = m_GuiDataLayerOutputDirectory.Text;
            }

            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                m_LastSelectedOutputdirectory = fbd.SelectedPath;
                m_GuiDataLayerOutputDirectory.Text = m_LastSelectedOutputdirectory;
            }

            fbd.Dispose();

        }

        #endregion

    }
}