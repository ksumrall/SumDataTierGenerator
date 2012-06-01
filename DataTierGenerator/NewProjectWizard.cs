using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using TSHOU.DataTierGenerator.MVP;

namespace TSHOU.DataTierGenerator {
    public partial class NewProjectWizard : Form {
        private SqlConnectionSettingsModel m_Model = null;
        private Project m_NewProject = new Project ();
        private SchemaGenerator m_Generator = null;

        public NewProjectWizard () {
            InitializeComponent ();
        }
        
        public Project Project {
            get {
                return m_NewProject;
            }
        }

        protected override void OnLoad (EventArgs e) {
            base.OnLoad (e);
            m_Model = new SqlConnectionSettingsModel ();
            m_GuiSqlLogin.Model = m_Model;
        }

        private void m_GuiWizardForm_Next (object sender, UtilityLibrary.Wizards.WizardForm.EventNextArgs e) {
            if (e.CurrentPage.Equals (m_GuiWelcomePage) || e.CurrentPage.Equals (m_GuiInfoPage)) {
                m_GuiWizardForm.WizardButtons ^= UtilityLibrary.Wizards.WizardForm.TWizardsButtons.Next;
            }
            else if (e.CurrentPage.Equals (m_GuiDbLoginPage)) {
                m_Generator = new SchemaGenerator (m_GuiSqlLogin.ConnectionString);
                m_Generator.ExtractSchema ();

                m_GuiTableList.Items.Clear ();
                foreach (Table table in m_Generator.TableList) {
                    m_GuiTableList.Items.Add (table, (!table.Name.StartsWith ("sys")));
                }
                m_GuiTableList.DisplayMember = "Name";
            }
        }

        private void CheckInfoNextState () {
            if ((m_GuiWizardForm.WizardButtons & UtilityLibrary.Wizards.WizardForm.TWizardsButtons.Next) != 0) {
                m_GuiWizardForm.WizardButtons ^= UtilityLibrary.Wizards.WizardForm.TWizardsButtons.Next;
            }

            if ((m_GuiOutputPath.Text.Trim ().Length > 0) && (m_GuiClassNamespace.Text.Trim ().Length > 0)) {
                if ((m_GuiWizardForm.WizardButtons & UtilityLibrary.Wizards.WizardForm.TWizardsButtons.Next) == 0) {
                    m_GuiWizardForm.WizardButtons |= UtilityLibrary.Wizards.WizardForm.TWizardsButtons.Next;
                }
            }
        }

        private void m_GuiSqlLogin_DatabindComplete (object sender, EventArgs e) {
            m_GuiWizardForm.WizardButtons |= UtilityLibrary.Wizards.WizardForm.TWizardsButtons.Next;
        }

        private void m_GuiWizardForm_Cancel (object sender, UtilityLibrary.Wizards.WizardForm.EventWizardCancelArgs e) {
            e.Title = "Cancel";
            e.Message = "Are you sure you want to cancel project creation?";
        }

        private void m_GuiWizardForm_WizardClosed (object sender, EventArgs e) {
            this.DialogResult = DialogResult.Cancel;
            this.Close ();
        }

        private void m_GuiBrowseButton_Click (object sender, EventArgs e) {
            DialogResult dr = m_GuiBrowseFolder.ShowDialog (this);
            if(dr == DialogResult.OK) {
                m_GuiOutputPath.Text = m_GuiBrowseFolder.SelectedPath;
            }
        }

        private void m_GuiOutputPath_TextChanged (object sender, EventArgs e) {
            CheckInfoNextState ();
        }

        private void m_GuiClassNamespace_TextChanged (object sender, EventArgs e) {
            CheckInfoNextState ();
        }

        private void m_GuiWizardForm_Finish (object sender, UtilityLibrary.Wizards.WizardForm.EventNextArgs e) {
            m_NewProject.OutputPath = m_GuiOutputPath.Text;
            m_NewProject.Namespace = m_GuiClassNamespace.Text;
            m_NewProject.ConnectionString = m_GuiSqlLogin.ConnectionString;
            m_NewProject.TableList.AddRange (m_Generator.TableList);
            // this does not work because the primary and foreign keys have references back to the containing table
            // this causes a circular reference which the xml serializer does not like.
            //m_NewProject.Save (@"c:\test.dtgproj");
            this.DialogResult = DialogResult.OK;
            this.Close ();
        }

        private void m_GuiSelectAllLabel_Click( object sender, EventArgs e ) {
            for ( int index = 0; index < m_GuiTableList.Items.Count; index++ ) {
                m_GuiTableList.SetItemChecked( index, true );
            }
        }

        private void m_GuiDeSelectAllLabel_Click( object sender, EventArgs e ) {
            for ( int index = 0; index < m_GuiTableList.Items.Count; index++ ) {
                m_GuiTableList.SetItemChecked( index, false );
            }
        }

        private void m_GuiTableList_ItemCheck( object sender, ItemCheckEventArgs e ) {
            ( (Table)m_GuiTableList.Items[e.Index] ).BuildClass = ( e.NewValue == CheckState.Checked );
        }
    }
}