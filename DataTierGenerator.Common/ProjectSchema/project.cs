﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.261
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=4.0.30319.1.
// 
namespace TotalSafety.DataTierGenerator.Common.ProjectSchema {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:dtg-project")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="urn:dtg-project", IsNullable=false)]
    public partial class Project {
        
        private Configuration configurationField;
        
        private Schemas[] schemasField;
        
        private string versionField;
        
        /// <remarks/>
        public Configuration Configuration {
            get {
                return this.configurationField;
            }
            set {
                this.configurationField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Schemas")]
        public Schemas[] Schemas {
            get {
                return this.schemasField;
            }
            set {
                this.schemasField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string version {
            get {
                return this.versionField;
            }
            set {
                this.versionField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:dtg-project")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="urn:dtg-project", IsNullable=false)]
    public partial class Configuration {
        
        private CodeGenerationDetails codeGenerationDetailsField;
        
        private DbConnectionDetails dbConnectionDetailsField;
        
        /// <remarks/>
        public CodeGenerationDetails CodeGenerationDetails {
            get {
                return this.codeGenerationDetailsField;
            }
            set {
                this.codeGenerationDetailsField = value;
            }
        }
        
        /// <remarks/>
        public DbConnectionDetails DbConnectionDetails {
            get {
                return this.dbConnectionDetailsField;
            }
            set {
                this.dbConnectionDetailsField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:dtg-project")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="urn:dtg-project", IsNullable=false)]
    public partial class CodeGenerationDetails {
        
        private string projectNamespaceField;
        
        private string outputPathField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string projectNamespace {
            get {
                return this.projectNamespaceField;
            }
            set {
                this.projectNamespaceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string outputPath {
            get {
                return this.outputPathField;
            }
            set {
                this.outputPathField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:dtg-project")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="urn:dtg-project", IsNullable=false)]
    public partial class DbConnectionDetails {
        
        private string dbProviderTypeField;
        
        private string connectionStringField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dbProviderType {
            get {
                return this.dbProviderTypeField;
            }
            set {
                this.dbProviderTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string connectionString {
            get {
                return this.connectionStringField;
            }
            set {
                this.connectionStringField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:dtg-project")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="urn:dtg-project", IsNullable=false)]
    public partial class Schemas {
        
        private Schema[] schemaField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Schema")]
        public Schema[] Schema {
            get {
                return this.schemaField;
            }
            set {
                this.schemaField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:dtg-project")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="urn:dtg-project", IsNullable=false)]
    public partial class Schema {
        
        private Table[] tablesField;
        
        private View[] viewsField;
        
        private Function[] functionsField;
        
        private Procedure[] proceduresField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Table", IsNullable=false)]
        public Table[] Tables {
            get {
                return this.tablesField;
            }
            set {
                this.tablesField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("View", IsNullable=false)]
        public View[] Views {
            get {
                return this.viewsField;
            }
            set {
                this.viewsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Function", IsNullable=false)]
        public Function[] Functions {
            get {
                return this.functionsField;
            }
            set {
                this.functionsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Procedure", IsNullable=false)]
        public Procedure[] Procedures {
            get {
                return this.proceduresField;
            }
            set {
                this.proceduresField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:dtg-project")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="urn:dtg-project", IsNullable=false)]
    public partial class Table {
        
        private TablePrimaryKey primaryKeyField;
        
        private Column[] columnsField;
        
        private string nameField;
        
        private string dbNameField;
        
        private string descriptionField;
        
        private string schemaField;
        
        private bool buildField;
        
        /// <remarks/>
        public TablePrimaryKey PrimaryKey {
            get {
                return this.primaryKeyField;
            }
            set {
                this.primaryKeyField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Column", IsNullable=false)]
        public Column[] Columns {
            get {
                return this.columnsField;
            }
            set {
                this.columnsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dbName {
            get {
                return this.dbNameField;
            }
            set {
                this.dbNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string description {
            get {
                return this.descriptionField;
            }
            set {
                this.descriptionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string schema {
            get {
                return this.schemaField;
            }
            set {
                this.schemaField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool build {
            get {
                return this.buildField;
            }
            set {
                this.buildField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:dtg-project")]
    public partial class TablePrimaryKey {
        
        private TablePrimaryKeyColumnRef[] columnRefField;
        
        private string nameField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ColumnRef")]
        public TablePrimaryKeyColumnRef[] ColumnRef {
            get {
                return this.columnRefField;
            }
            set {
                this.columnRefField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:dtg-project")]
    public partial class TablePrimaryKeyColumnRef {
        
        private string refField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string @ref {
            get {
                return this.refField;
            }
            set {
                this.refField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:dtg-project")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="urn:dtg-project", IsNullable=false)]
    public partial class Column {
        
        private string nameField;
        
        private string clrTypeField;
        
        private string dbTypeField;
        
        private string languageTypeField;
        
        private string lengthField;
        
        private string precisionField;
        
        private string propertyNameField;
        
        private string defaultValueField;
        
        private string enumeratedTypeNameField;
        
        private bool isNullableField;
        
        private bool isIdentityField;
        
        private bool isRowGuidField;
        
        private bool isComputedField;
        
        private string scaleField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string clrType {
            get {
                return this.clrTypeField;
            }
            set {
                this.clrTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dbType {
            get {
                return this.dbTypeField;
            }
            set {
                this.dbTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string languageType {
            get {
                return this.languageTypeField;
            }
            set {
                this.languageTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string length {
            get {
                return this.lengthField;
            }
            set {
                this.lengthField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string precision {
            get {
                return this.precisionField;
            }
            set {
                this.precisionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string propertyName {
            get {
                return this.propertyNameField;
            }
            set {
                this.propertyNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string defaultValue {
            get {
                return this.defaultValueField;
            }
            set {
                this.defaultValueField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string enumeratedTypeName {
            get {
                return this.enumeratedTypeNameField;
            }
            set {
                this.enumeratedTypeNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isNullable {
            get {
                return this.isNullableField;
            }
            set {
                this.isNullableField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isIdentity {
            get {
                return this.isIdentityField;
            }
            set {
                this.isIdentityField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isRowGuid {
            get {
                return this.isRowGuidField;
            }
            set {
                this.isRowGuidField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isComputed {
            get {
                return this.isComputedField;
            }
            set {
                this.isComputedField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string scale {
            get {
                return this.scaleField;
            }
            set {
                this.scaleField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:dtg-project")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="urn:dtg-project", IsNullable=false)]
    public partial class View {
        
        private Column[] columnsField;
        
        private string nameField;
        
        private string dbNameField;
        
        private string descriptionField;
        
        private string schemaField;
        
        private bool buildField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Column", IsNullable=false)]
        public Column[] Columns {
            get {
                return this.columnsField;
            }
            set {
                this.columnsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dbName {
            get {
                return this.dbNameField;
            }
            set {
                this.dbNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string description {
            get {
                return this.descriptionField;
            }
            set {
                this.descriptionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string schema {
            get {
                return this.schemaField;
            }
            set {
                this.schemaField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool build {
            get {
                return this.buildField;
            }
            set {
                this.buildField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:dtg-project")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="urn:dtg-project", IsNullable=false)]
    public partial class Function {
        
        private Parameter[] parametersField;
        
        private string nameField;
        
        private string dbNameField;
        
        private string descriptionField;
        
        private string schemaField;
        
        private bool buildField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Parameter", IsNullable=false)]
        public Parameter[] Parameters {
            get {
                return this.parametersField;
            }
            set {
                this.parametersField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dbName {
            get {
                return this.dbNameField;
            }
            set {
                this.dbNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string description {
            get {
                return this.descriptionField;
            }
            set {
                this.descriptionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string schema {
            get {
                return this.schemaField;
            }
            set {
                this.schemaField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool build {
            get {
                return this.buildField;
            }
            set {
                this.buildField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:dtg-project")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="urn:dtg-project", IsNullable=false)]
    public partial class Parameter {
        
        private string nameField;
        
        private string clrTypeField;
        
        private string dbTypeField;
        
        private string propertyNameField;
        
        private string defaultValueField;
        
        private string enumeratedTypeNameField;
        
        private bool isNullableField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string clrType {
            get {
                return this.clrTypeField;
            }
            set {
                this.clrTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dbType {
            get {
                return this.dbTypeField;
            }
            set {
                this.dbTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string propertyName {
            get {
                return this.propertyNameField;
            }
            set {
                this.propertyNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string defaultValue {
            get {
                return this.defaultValueField;
            }
            set {
                this.defaultValueField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string enumeratedTypeName {
            get {
                return this.enumeratedTypeNameField;
            }
            set {
                this.enumeratedTypeNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isNullable {
            get {
                return this.isNullableField;
            }
            set {
                this.isNullableField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:dtg-project")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="urn:dtg-project", IsNullable=false)]
    public partial class Procedure {
        
        private Parameter[] parametersField;
        
        private string nameField;
        
        private string dbNameField;
        
        private string descriptionField;
        
        private string schemaField;
        
        private bool buildField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Parameter", IsNullable=false)]
        public Parameter[] Parameters {
            get {
                return this.parametersField;
            }
            set {
                this.parametersField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dbName {
            get {
                return this.dbNameField;
            }
            set {
                this.dbNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string description {
            get {
                return this.descriptionField;
            }
            set {
                this.descriptionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string schema {
            get {
                return this.schemaField;
            }
            set {
                this.schemaField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool build {
            get {
                return this.buildField;
            }
            set {
                this.buildField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:dtg-project")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="urn:dtg-project", IsNullable=false)]
    public partial class Tables {
        
        private Table[] tableField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Table")]
        public Table[] Table {
            get {
                return this.tableField;
            }
            set {
                this.tableField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:dtg-project")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="urn:dtg-project", IsNullable=false)]
    public partial class Views {
        
        private View[] viewField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("View")]
        public View[] View {
            get {
                return this.viewField;
            }
            set {
                this.viewField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:dtg-project")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="urn:dtg-project", IsNullable=false)]
    public partial class Functions {
        
        private Function[] functionField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Function")]
        public Function[] Function {
            get {
                return this.functionField;
            }
            set {
                this.functionField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:dtg-project")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="urn:dtg-project", IsNullable=false)]
    public partial class Procedures {
        
        private Procedure[] procedureField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Procedure")]
        public Procedure[] Procedure {
            get {
                return this.procedureField;
            }
            set {
                this.procedureField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:dtg-project")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="urn:dtg-project", IsNullable=false)]
    public partial class Columns {
        
        private Column[] columnField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Column")]
        public Column[] Column {
            get {
                return this.columnField;
            }
            set {
                this.columnField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:dtg-project")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="urn:dtg-project", IsNullable=false)]
    public partial class Parameters {
        
        private Parameter[] parameterField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Parameter")]
        public Parameter[] Parameter {
            get {
                return this.parameterField;
            }
            set {
                this.parameterField = value;
            }
        }
    }
}
