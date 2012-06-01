using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

using TSHOU.DataTierGenerator.MVP;

namespace TSHOU.DataTierGenerator.Controls {

    public partial class SqlServerLogin : UserControl {

        public event EventHandler DatabindComplete;

        public SqlServerLogin() {
            InitializeComponent();
        }

        public SqlConnectionSettingsModel Model {
            set {
                sqlConnectionSettingsModelBindingSource.Add(value);

                if ( CanRefreshDatabase() ) {
                    GetDatabindings();
                }
            }
        }

        public string ConnectionString {
            get {
                SqlConnectionSettingsModel model = sqlConnectionSettingsModelBindingSource[0] as SqlConnectionSettingsModel;
                return model.ConnectionString;
            }
            set {
                SqlConnectionStringBuilder sb = new SqlConnectionStringBuilder();
                sb.ConnectionString = value;

                m_GuiServerNameTextbox.Text = sb.DataSource;
                m_GuiDatabaseCombo.Text = sb.InitialCatalog;

                if ( sb.IntegratedSecurity ) {

                    m_GuiWindowsAuthenticationRadioButton.Checked = true;

                    m_GuiUserIdTextBox.Text = "";
                    m_GuiUserIdTextBox.Enabled = false;

                    m_GuiPasswordTextBox.Text = "";
                    m_GuiPasswordTextBox.Enabled = false;

                } else {

                    m_GuiSqlServerAuthenticationRadioButton.Checked = true;

                    m_GuiUserIdTextBox.Text = sb.UserID;
                    m_GuiUserIdTextBox.Enabled = true;

                    m_GuiPasswordTextBox.Text = sb.Password;
                    m_GuiPasswordTextBox.Enabled = true;

                }
            }
        }

        public void Commit() {
            sqlConnectionSettingsModelBindingSource.EndEdit();
        }

        public void Rollback() {
            sqlConnectionSettingsModelBindingSource.CancelEdit();
        }

        private void m_GuiGetDatabasesButton_Click(object sender, EventArgs e) {
            GetDatabindings();
        }

        private void m_GuiWindowsAuthenticationRadioButton_CheckedChanged(object sender, EventArgs e) {
            AuthenticationTypeChanged(SqlServerAuthenticationTypeEnumeration.Windows);
        }

        private void m_GuiSqlServerAuthenticationRadioButton_CheckedChanged(object sender, EventArgs e) {
            AuthenticationTypeChanged(SqlServerAuthenticationTypeEnumeration.SqlServer);
        }

        private void ConnectionDetails_DataChanged(object sender, EventArgs e) {
            //m_GuiGetDatabasesButton.Enabled = CanRefreshDatabase();
            m_GuiDatabaseCombo.Enabled = CanRefreshDatabase ();
        }

        private void OnDatabindComplete() {
            if ( DatabindComplete != null ) {
                DatabindComplete(this, new EventArgs());
            }
        }

        private void AuthenticationTypeChanged(SqlServerAuthenticationTypeEnumeration sqlServerAuthenticationType) {

            switch ( sqlServerAuthenticationType ) {
                case SqlServerAuthenticationTypeEnumeration.SqlServer:
                    m_GuiUserIdTextBox.Enabled = true;
                    m_GuiPasswordTextBox.Enabled = true;
                    m_GuiUserIdLabel.Enabled = true;
                    m_GuiPasswordLabel.Enabled = true;
                    break;

                case SqlServerAuthenticationTypeEnumeration.Windows:
                    m_GuiUserIdTextBox.Enabled = false;
                    m_GuiPasswordTextBox.Enabled = false;
                    m_GuiUserIdLabel.Enabled = false;
                    m_GuiPasswordLabel.Enabled = false;
                    break;
            }

            if ( sqlConnectionSettingsModelBindingSource.Count > 0 ) {
                ( (SqlConnectionSettingsModel)sqlConnectionSettingsModelBindingSource[0] ).SqlServerAuthenticationType = sqlServerAuthenticationType;
            }

            //m_GuiGetDatabasesButton.Enabled = CanRefreshDatabase();
            m_GuiDatabaseCombo.Enabled = CanRefreshDatabase ();
        }

        private bool CanRefreshDatabase() {
            if ( m_GuiServerNameTextbox.Text == "" ) {
                return false;
            }

            if ( m_GuiSqlServerAuthenticationRadioButton.Checked ) {
                if ( m_GuiUserIdTextBox.Text == "" ) {
                    return false;
                }
            }

            return true;
        }

        private void GetDatabindings() {
            SqlConnectionSettingsModel model;

            model =
                (SqlConnectionSettingsModel)sqlConnectionSettingsModelBindingSource[0];

            try {
                m_GuiDatabaseCombo.DataSource =
                    model.GetDatabaseList();

                OnDatabindComplete();
            } catch ( Exception exp ) {
                MessageBox.Show(this.FindForm(), exp.Message, "Error");
            }
        }

        private void m_GuiDatabaseCombo_DropDown (object sender, EventArgs e) {
            GetDatabindings ();
        }
    }
}
