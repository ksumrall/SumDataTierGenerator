using System;
using System.Collections.Generic;
using System.Text;

namespace #ROOT_NAMESPACE# {

    public class FieldValue {
    
        #region private and protected member variables

        internal bool m_Dirty;
        internal object m_Value;
        private FieldDefinition m_FieldDefinition;

        #endregion

        #region internal structured members
        #endregion

        #region constructors / desturctors

        public FieldValue(FieldDefinition fieldDefinition) {
            m_Dirty = false;
            m_Value = null;

            m_FieldDefinition = fieldDefinition;
        }

        public FieldValue(FieldDefinition fieldDefinition, object value) : this(fieldDefinition) {
            m_Value = value;
        }

        public FieldDefinition FieldDefinition {
            get { return m_FieldDefinition; }
        }

        #endregion

        #region public properties

        public bool IsDirty {
            get { return m_Dirty; }
            internal set { m_Dirty = value; }
        }

        public object Value {
            get { return m_Value; }
            set {
                m_Value = value;
                m_Dirty = true;
            }
        }

        #endregion

        #region public methods
        #endregion

        #region private implementation
        #endregion
    
    }

    public class FieldDefinition {

        #region private and protected member variables

        private string m_Name;
        private Type m_ValueType;
        public string m_DataTypeName;       // SqlDbType enum
        private string m_SourceSchemaName;
        private string m_SourceObjectName;  // Table or View
        private string m_SourceColumnName;
        private string m_SourceColumnDataType;
        private int m_SourceColumnMaxLength;
        private byte m_SourceColumnScale;
        private byte m_SourceColumnPrecision;
        private bool m_IsIdentity;
        private bool m_IsNullable;
        private bool m_IsPrimaryKey;
        private bool m_IsForeignKey;
        private bool m_IsReadOnly;
        private bool m_HasDefault;
        private int m_FieldIndex;

        #endregion

        #region internal structured members
        #endregion

        #region constructors / destructors

        public FieldDefinition( string name, string dataTypeName ) {
            m_Name = name;
            m_DataTypeName = dataTypeName;
            m_SourceColumnName = string.Empty;
            m_SourceColumnMaxLength = 0;
            m_SourceColumnPrecision = 0;
            m_SourceColumnScale = 0;
            m_IsNullable = true;
            m_HasDefault = false;
        }

        public FieldDefinition( string name, string dataTypeName, Type valueType
            , string sourceSchemaName, string sourceObjectName, string sourceColumnName
            , string sourceColumnDataType, int sourceColumnMaxLength
            , byte sourceColumnScale, byte sourceColumnPrecesion, bool isPrimaryKey
            , bool isIdentity, bool isNullable, bool isReadOnly, bool hasDefault, int fieldIndex) {
        
            m_Name = name;
            m_DataTypeName = dataTypeName;
            m_ValueType = valueType;
            m_SourceSchemaName = sourceSchemaName;
            m_SourceObjectName = sourceObjectName;
            m_SourceColumnName = sourceColumnName;
            m_SourceColumnDataType = sourceColumnDataType;
            m_SourceColumnMaxLength = sourceColumnMaxLength;
            m_SourceColumnScale = sourceColumnScale;
            m_SourceColumnPrecision = sourceColumnPrecesion;
            m_IsPrimaryKey = isPrimaryKey;
            m_IsIdentity = isIdentity;
            m_IsNullable = isNullable;
            m_IsReadOnly = isReadOnly;
            m_HasDefault = hasDefault;
            m_FieldIndex = fieldIndex;
        }

        #endregion

        #region public properties

        public string Name {
            get { return m_Name; }
            set { m_Name = value; }
        }

        public string DataTypeName {
            get { return m_DataTypeName; }
            set { m_DataTypeName = value; }
        }

        public Type ValueType {
            get { return m_ValueType; }
            set { m_ValueType = value; }
        }

        public string SourceSchemaName {
            get { return m_SourceSchemaName; }
            set { m_SourceSchemaName = value; }
        }

        public string SourceObjectName {
            get { return m_SourceObjectName; }
            set { m_SourceObjectName = value; }
        }

        public string SourceColumnName {
            get { return m_SourceColumnName; }
            set { m_SourceColumnName = value; }
        }

        public string SourceColumnDbType {
            get { return m_SourceColumnDataType; }
            set { m_SourceColumnDataType = value; }
        }

        public int SourceColumnMaxLength {
            get { return m_SourceColumnMaxLength; }
            set { m_SourceColumnMaxLength = value; }
        }

        public byte SourceColumnScale {
            get { return m_SourceColumnScale; }
            set { m_SourceColumnScale = value; }
        }

        public byte SourceColumnPrecision {
            get { return m_SourceColumnPrecision; }
            set { m_SourceColumnPrecision = value; }
        }

        public bool IsIdentity {
            get { return m_IsIdentity; }
            set { m_IsIdentity = value; }
        }

        public bool IsNullable {
            get { return m_IsNullable; }
            set { m_IsNullable = value; }
        }

        public bool IsPrimaryKey {
            get { return m_IsPrimaryKey; }
            set { m_IsPrimaryKey = value; }
        }

        public bool IsForeignKey {
            get { return m_IsForeignKey; }
            set { m_IsForeignKey = value; }
        }

        public bool IsReadOnly {
            get { return m_IsReadOnly; }
            set { m_IsReadOnly = value; }
        }

        public bool HasDefault {
            get { return m_HasDefault; }
            set { m_HasDefault = value; }
        }

        public int FieldIndex {
            get { return m_FieldIndex; }
            set { m_FieldIndex = value; }
        }

        #endregion

        #region public methods
        #endregion

        #region private implementation
        #endregion

    }
}
