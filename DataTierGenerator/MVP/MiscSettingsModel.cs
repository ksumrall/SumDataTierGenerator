using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TSHOU.DataTierGenerator.MVP {

    public class MiscSettingsModel : IEditableObject {

        #region private and protected member variables

        private string m_ProjectPathname = "";
        private string m_DbConnectionType = "";
        private string m_ConnectionString = "";
        private string m_Namespace = "";
        private string m_GeneratedDataProjectPath = "";

        private bool IsEditMode = false;

        private string m_ProjectPathname_PreEdit = "";
        private string m_DbConnectionType_PreEdit = "";
        private string m_ConnectionString_PreEdit = "";
        private string m_Namespace_PreEdit = "";
        private string m_GeneratedDataProjectPath_PreEdit = "";

        #endregion

        #region internal structured members
        #endregion

        #region constructors / desturctors

        #endregion

        #region public properties

        public string ProjectPathname {
            get {
                return m_ProjectPathname;
            }
            set {
                m_ProjectPathname = value;
            }
        }

        public string DbConnectionType
        {
            get
            {
                return m_DbConnectionType;
            }
            set
            {
                m_DbConnectionType = value;
            }
        }

        public string ConnectionString
        {
            get
            {
                return m_ConnectionString;
            }
            set
            {
                m_ConnectionString = value;
            }
        }

        public string Namespace
        {
            get
            {
                return m_Namespace;
            }
            set
            {
                m_Namespace = value;
            }
        }

        public string GeneratedDataProjectPath
        {
            get {
                return m_GeneratedDataProjectPath;
            }
            set {
                m_GeneratedDataProjectPath = value;
            }
        }

        #endregion

        #region event handlers / overrides

        #endregion

        #region public methods

        #endregion

        #region private implementation

        #endregion

        #region IEditableObject Members

        public void BeginEdit() {

            m_ProjectPathname_PreEdit = m_ProjectPathname;
            m_DbConnectionType_PreEdit = m_DbConnectionType;
            m_ConnectionString_PreEdit = m_ConnectionString;
            m_Namespace_PreEdit = m_Namespace;
            m_GeneratedDataProjectPath_PreEdit = m_GeneratedDataProjectPath;

            IsEditMode = true;

        }

        public void CancelEdit() {

            m_ProjectPathname = m_ProjectPathname_PreEdit;
            m_DbConnectionType = m_DbConnectionType_PreEdit;
            m_ConnectionString = m_ConnectionString_PreEdit;
            m_Namespace = m_Namespace_PreEdit;
            m_GeneratedDataProjectPath = m_GeneratedDataProjectPath_PreEdit;

            IsEditMode = false;

        }

        public void EndEdit() {

            if (IsEditMode) {
                IsEditMode = false;
            }
        
        }

        #endregion
    }

}
