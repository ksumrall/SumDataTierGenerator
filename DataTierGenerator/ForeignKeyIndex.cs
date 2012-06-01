using System;
using System.Collections.Generic;
using System.Text;

namespace TSHOU.DataTierGenerator {

    public class ForeignKeyIndexList : List<ForeignKeyIndex> {

        public ForeignKeyIndex this[string foreignKeyIndexName] {
            get {
                return FindForeignKeyIndex(foreignKeyIndexName);
            }
        }

        public ForeignKeyIndex FindForeignKeyIndex(string foreignKeyIndexName) {

            foreach (ForeignKeyIndex foreignKeyIndex in this) {
                if (foreignKeyIndex.Name == foreignKeyIndexName) {
                    return foreignKeyIndex;
                }
            }

            return null;

        }

    }

    public class ForeignKeyIndex : Index {

        #region private and protected member variables

        private Table m_ReferencedTable;
        private Index m_ReferencedPrimaryKey;

        #endregion

        #region constructors / desturctors

        public ForeignKeyIndex() : base() {

            m_ReferencedTable = null;
            m_ReferencedPrimaryKey = null;

        }

        #endregion

        #region public properties

        public Table ReferencedTable {
            get {
                return m_ReferencedTable;
            }
            set {
                m_ReferencedTable = value;
            }
        }

        public Index ReferencedPrimaryKey {
            get {
                return m_ReferencedPrimaryKey;
            }
            set {
                m_ReferencedPrimaryKey = value;
            }
        }

        #endregion

        #region public methods

        #endregion

        #region private implementation

        #endregion
    }
}
