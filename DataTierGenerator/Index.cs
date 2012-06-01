using System;
using System.Collections.Generic;
using System.Text;

namespace TSHOU.DataTierGenerator {
    public class Index {

        #region private and protected member variables

        private string m_Name;
        private Table m_ParentTable;
        private List<Column> m_ColumnList;

        #endregion

        #region constructors / desturctors

        public Index() {

            m_Name = "";
            m_ParentTable = null;
            m_ColumnList = new List<Column>();

        }

        #endregion

        #region public properties

        public string Name {
            get {
                return m_Name;
            }
            set {
                m_Name = value;
            }
        }

        public Table ParentTable {
            get {
                return m_ParentTable;
            }
            set {
                m_ParentTable = value;
            }
        }

        public List<Column> Columns {
            get {
                return m_ColumnList;
            }
            set {
                m_ColumnList = value;
            }
        }

        #endregion

        #region public methods

        #endregion

        #region private implementation

        #endregion
    }
}
