using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace TSHOU.DataTierGenerator.MVP {

    public class SqlConnectionSettingsModel : IEditableObject {

        #region private and protected member variables

        string m_ServerName = "";
        string m_DatabaseName = "";
        SqlServerAuthenticationTypeEnumeration m_SqlServerAuthenticationTypeEnumeration =
            SqlServerAuthenticationTypeEnumeration.Windows;
        string m_UserId = "";
        string m_Password = "";

        bool IsEditMode = false;
        string m_ServerName_PreEdit;
        string m_DatabaseName_PreEdit;
        SqlServerAuthenticationTypeEnumeration m_SqlServerAuthenticationTypeEnumeration_PreEdit;
        string m_UserId_PreEdit;
        string m_Password_PreEdit;

        #endregion

        #region internal structured members
        #endregion

        #region constructors / desturctors

        public SqlConnectionSettingsModel() {
        }

        #endregion

        #region public properties

        public string ServerName {
            get {
                return m_ServerName;
            }
            set {
                m_ServerName = value;
            }
        }

        public string DatabaseName {
            get {
                return m_DatabaseName;
            }
            set {
                m_DatabaseName = value;
            }
        }

        public SqlServerAuthenticationTypeEnumeration SqlServerAuthenticationType {
            get {
                return m_SqlServerAuthenticationTypeEnumeration;
            }
            set {
                m_SqlServerAuthenticationTypeEnumeration = value;
            }
        }

        public string UserId {
            get {
                return m_UserId;
            }
            set {
                m_UserId = value;
            }
        }

        public string Password {
            get {
                return m_Password;
            }
            set {
                m_Password = value;
            }
        }

        public string ConnectionString {
            get {
                return GetConnectionString ();
            }
        }

        #endregion

        #region event handlers / overrides

        #endregion

        #region public methods

        public bool CanEditPropertyByName(string propertyName) {

            bool rtnValue = false;

            switch (propertyName) {

                case "ServerName":
                    rtnValue = true;
                    break;

                case "DatabaseName":
                    rtnValue = true;
                    break;

                case "SqlServerAuthenticationType":
                    rtnValue = true;
                    break;

                case "UserId":
                    if (m_SqlServerAuthenticationTypeEnumeration == SqlServerAuthenticationTypeEnumeration.SqlServer) {
                        rtnValue = false;
                    }
                    break;

                case "Password":
                    if (m_SqlServerAuthenticationTypeEnumeration == SqlServerAuthenticationTypeEnumeration.SqlServer) {
                        rtnValue = false;
                    }
                    break;

            }

            return rtnValue;

        }

        public bool CanRefreshDatabaseList() {

            bool rtnValue = false;

            if (m_SqlServerAuthenticationTypeEnumeration == SqlServerAuthenticationTypeEnumeration.SqlServer) {
                if (m_UserId != "" && m_ServerName != "") {
                    rtnValue = true;
                }
            } else {
                if (m_ServerName != "") {
                    rtnValue = true;
                }
            }

            return rtnValue;
        }

        public string[] GetDatabaseList() {

            string connectionString = GetConnectionString();
            SqlCommand cmd;
            SqlDataReader dr;
            List<string> databaseArray;

            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();

            cmd = connection.CreateCommand();
            cmd.CommandText = "Select * from master..sysdatabases";
            cmd.CommandType = CommandType.Text;

            dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            databaseArray = new List<string>();

            while (dr.Read()) {
                if ( dr["name"].ToString() != "master"
                    && dr["name"].ToString() != "tempdb"
                    && dr["name"].ToString() != "model"
                    && dr["name"].ToString() != "msdb" ) {
                    databaseArray.Add((string)dr["name"]);
                }
            }

            databaseArray.Sort();
            dr.Close();

            return databaseArray.ToArray();

        }

        #endregion

        #region private implementation

        string GetConnectionString () {
            SqlConnectionStringBuilder sb = new SqlConnectionStringBuilder ();
            string databaseName;

            if (m_ServerName == "") {
                throw new ApplicationException ("Server Name cannot be blank.");
            }

            if (m_DatabaseName == "") {
                databaseName = "master";
            }
            else {
                databaseName = m_DatabaseName;
            }

            sb.DataSource = m_ServerName;
            sb.InitialCatalog = databaseName;

            if (m_SqlServerAuthenticationTypeEnumeration == SqlServerAuthenticationTypeEnumeration.Windows) {
                sb.IntegratedSecurity = true;
            }
            else {

                if (m_UserId == "") {
                    throw new ApplicationException ("User ID cannot be blank when using SQL authentication.");
                }

                if (m_Password == "") {
                    throw new ApplicationException ("Password cannot be blank when using SQL authentication.");
                }

                sb.UserID = m_UserId;
                sb.Password = m_Password;
            }

            return sb.ToString ();
        }

        #endregion

        #region IEditableObject Members

        void IEditableObject.BeginEdit() {

            m_ServerName_PreEdit = m_ServerName;
            m_DatabaseName_PreEdit = m_DatabaseName;
            m_SqlServerAuthenticationTypeEnumeration_PreEdit = m_SqlServerAuthenticationTypeEnumeration;
            m_UserId_PreEdit = m_UserId;
            m_Password_PreEdit = m_Password;

            IsEditMode = true;

        }

        void IEditableObject.CancelEdit() {

            IsEditMode = false;
        
            m_ServerName = m_ServerName_PreEdit;
            m_DatabaseName = m_DatabaseName_PreEdit;
            m_SqlServerAuthenticationTypeEnumeration = m_SqlServerAuthenticationTypeEnumeration_PreEdit;
            m_UserId = m_UserId_PreEdit;
            m_Password = m_Password_PreEdit;

        }

        void IEditableObject.EndEdit() {

            if (IsEditMode) {
                IsEditMode = false;
            }

        }

        #endregion
 
    }

}
