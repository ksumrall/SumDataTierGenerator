using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace TotalSafety.DataTierGenerator.Common
{

    [Serializable]
    public class Project : ProjectSchema.Project
    {

        #region events

        public event EventHandler<ChangedEventArgs<Project>> Changed;

        #endregion

        #region private and protected member variables

        private string m_FilePath = String.Empty;
        private string m_ProjectName = String.Empty;
        private string m_DbProviderType = String.Empty;
        private string m_ConnectionString = String.Empty;
        private string m_Namespace = String.Empty;
        private string m_OutputPath = String.Empty;
        private MonitoredList<Schema> m_SchemaList = new MonitoredList<Schema>();
        private MonitoredList<Table> m_TableList = new MonitoredList<Table>();
        private MonitoredList<Common.View> m_ViewList = new MonitoredList<Common.View>();
        private MonitoredList<Function> m_FunctionList = new MonitoredList<Function>();
        private MonitoredList<Procedure> m_ProcedureList = new MonitoredList<Procedure>();
        private bool m_Dirty = false;
        private bool m_New = true;
        private bool m_SupressEvents = false;

        #endregion

        #region constructors / desturctors

        public Project()
        {
            m_ProjectName = "New Project";
            Schemas = new ProjectSchema.Schema[0];
            m_TableList.Changed += new EventHandler<ChangedEventArgs<Table>>(m_TableList_Changed);
            m_TableList.Cleared += new EventHandler<ClearedEventArgs<Table>>(m_TableList_Cleared);
            m_ViewList.Changed += new EventHandler<ChangedEventArgs<Common.View>>(m_ViewList_Changed);
            m_ViewList.Cleared += new EventHandler<ClearedEventArgs<Common.View>>(m_ViewList_Cleared);
            m_ProcedureList.Changed += new EventHandler<ChangedEventArgs<Procedure>>(m_ProcedureList_Changed);
            m_ProcedureList.Cleared += new EventHandler<ClearedEventArgs<Procedure>>(m_ProcedureList_Cleared);
        }

        #endregion

        #region public properties

        [XmlIgnore]
        public string FilePath
        {
            get
            {
                return m_FilePath;
            }
            set
            {
                m_FilePath = value;
                OnChanged();
            }
        }

        [XmlIgnore]
        public bool IsDirty
        {
            get
            {
                return m_Dirty;
            }
            private set
            {
                m_Dirty = value;
            }
        }

        [XmlIgnore]
        public bool IsNew
        {
            get
            {
                return m_New;
            }
            private set
            {
                m_New = value;
            }
        }

        public string ProjectName
        {
            get { return m_ProjectName; }
            set
            {
                m_ProjectName = value;
                OnChanged();
            }
        }

        [XmlIgnore]
        public string DbProviderType
        {
            get
            {
                return m_DbProviderType;
            }
            set
            {
                m_DbProviderType = value;
                OnChanged();
            }
        }

        [XmlIgnore]
        public string ConnectionString
        {
            get
            {
                return m_ConnectionString;
            }
            set
            {
                m_ConnectionString = value;
                OnChanged();
            }
        }

        [XmlIgnore]
        public string Namespace
        {
            get
            {
                return m_Namespace;
            }
            set
            {
                m_Namespace = value;
                OnChanged();
            }
        }

        [XmlIgnore]
        public string OutputPath
        {
            get
            {
                return m_OutputPath;
            }
            set
            {
                m_OutputPath = value;
                if (!m_OutputPath.EndsWith("\\"))
                {
                    m_OutputPath += "\\";
                }
                OnChanged();
            }
        }

        [XmlIgnore]
        public MonitoredList<Schema> SchemaList
        {
            get
            {
                return m_SchemaList;
            }
            set
            {
                m_SchemaList = value;
                OnChanged();
            }
        }

        [XmlIgnore]
        public MonitoredList<Table> TableList
        {
            get
            {
                return m_TableList;
            }
            set
            {
                m_TableList = value;
                OnChanged();
            }
        }

        [XmlIgnore]
        public MonitoredList<Common.View> ViewList
        {
            get
            {
                return m_ViewList;
            }
            set
            {
                m_ViewList = value;
                OnChanged();
            }
        }

        [XmlIgnore]
        public MonitoredList<Function> FunctionList
        {
            get
            {
                return m_FunctionList;
            }
            set
            {
                m_FunctionList = value;
                OnChanged();
            }
        }

        [XmlIgnore]
        public MonitoredList<Procedure> ProcedureList
        {
            get
            {
                return m_ProcedureList;
            }
            set
            {
                m_ProcedureList = value;
                OnChanged();
            }
        }

        #endregion

        #region event handlers / overrides

        void m_TableList_Cleared(object sender, ClearedEventArgs<Table> e)
        {
            OnChanged();
        }

        private void m_TableList_Changed(object sender, ChangedEventArgs<Table> e)
        {
            OnChanged();
        }

        void m_ViewList_Cleared(object sender, ClearedEventArgs<Common.View> e)
        {
            OnChanged();
        }

        private void m_ViewList_Changed(object sender, ChangedEventArgs<Common.View> e)
        {
            OnChanged();
        }

        void m_ProcedureList_Cleared(object sender, ClearedEventArgs<Procedure> e)
        {
            OnChanged();
        }

        private void m_ProcedureList_Changed(object sender, ChangedEventArgs<Procedure> e)
        {
            OnChanged();
        }

        #endregion

        #region public methods

        public static Project Load(string path)
        {

            StreamReader sr = null;

            try
            {
                sr = new StreamReader(path);
                XmlSerializer serializer = new XmlSerializer(typeof(ProjectSchema.Project));
                ProjectSchema.Project proj = serializer.Deserialize(sr) as ProjectSchema.Project;
                if (proj == null)
                {
                    throw new Exception();
                }

                Project newProj = new Project();
                newProj.SupressEvents();

                newProj.FilePath = Path.GetFullPath(path);
                newProj.IsDirty = false;
                newProj.IsNew = false;

                newProj.DbProviderType = proj.Configuration.DbConnectionDetails.dbProviderType;
                newProj.ConnectionString = proj.Configuration.DbConnectionDetails.connectionString;
                newProj.Namespace = proj.Configuration.CodeGenerationDetails.projectNamespace;
                newProj.OutputPath = proj.Configuration.CodeGenerationDetails.outputPath;

                if (proj.Schemas != null && proj.Schemas.Length > 0)
                {
                    foreach (ProjectSchema.Schema s in proj.Schemas) { }
                    foreach (ProjectSchema.Schema s in proj.Schemas)
                    {
                        //#region
                        //Table newTable = new Table();
                        //newTable.Name = t.name;
                        //newTable.DatabaseName = t.dbName;
                        ////newTable.ClassName = t.className;
                        //newTable.Description = t.description;
                        //newTable.Schema = t.schema;
                        //newTable.BuildClass = t.build;

                        //if (t.Columns != null && t.Columns.Length > 0)
                        //{
                        //    foreach (Schema.Column c in t.Columns)
                        //    {
                        //        Column newCol = new Column();
                        //        newCol.Name = c.name;
                        //        newCol.ClrType = c.clrType;
                        //        newCol.Type = c.dbType;
                        //        newCol.LanguageType = c.languageType;
                        //        newCol.IsNullable = c.isNullable;
                        //        newCol.IsIdentity = c.isIdentity;
                        //        newCol.IsComputed = c.isComputed;
                        //        newCol.IsRowGuidCol = c.isRowGuid;
                        //        newCol.EnumeratedTypeName = c.enumeratedTypeName;
                        //        newCol.Length = c.length;
                        //        newCol.Precision = c.precision;
                        //        newCol.PropertyName = c.propertyName;
                        //        newCol.DefaultValue = c.defaultValue;
                        //        newCol.Scale = c.scale;

                        //        newTable.Columns.Add(newCol);
                        //    }
                        //}

                        //// Fix up the PK refs
                        //if (t.PrimaryKey != null && t.PrimaryKey.ColumnRef != null && t.PrimaryKey.ColumnRef.Length > 0)
                        //{
                        //    newTable.PrimaryKey = new Index();
                        //    foreach (Schema.TablePrimaryKeyColumnRef @ref in t.PrimaryKey.ColumnRef)
                        //    {
                        //        foreach (Column c in newTable.Columns)
                        //        {
                        //            if (c.Name == @ref.@ref)
                        //            {
                        //                newTable.PrimaryKey.Columns.Add(c);
                        //                break;
                        //            }
                        //        }
                        //    }
                        //}

                        //newProj.TableList.Add(newTable);
                        //#endregion
                    }
                }

                newProj.ResumeEvents();

                return newProj;
            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }
            }
        }

        public void LoadSchemasFromXml(XmlNode xnode)
        {
            XmlNodeList xlist = xnode.SelectNodes("schema");
            List<ProjectSchema.Schema> schemaList = new List<ProjectSchema.Schema>();

            SupressEvents();

            ProjectSchema.Schema schema;
            foreach (XmlNode schemNode in xlist)
            {
                schema = BuildSchema(schemNode);
                schemaList.Add(schema);
            }
            Schemas = schemaList.ToArray();

            ResumeEvents();
        }

        public void Save()
        {
            if (!String.IsNullOrEmpty(m_FilePath))
            {
                Save(m_FilePath);
            }
        }

        public void Save(string path)
        {
            StreamWriter sw = null;
            try
            {
                ProjectSchema.Project proj = new ProjectSchema.Project();

                proj.Configuration = new ProjectSchema.Configuration();
                proj.Configuration.DbConnectionDetails = new ProjectSchema.DbConnectionDetails();
                proj.Configuration.CodeGenerationDetails = new ProjectSchema.CodeGenerationDetails();

                proj.Configuration.DbConnectionDetails.dbProviderType = this.DbProviderType;
                proj.Configuration.DbConnectionDetails.connectionString = this.ConnectionString;
                proj.Configuration.CodeGenerationDetails.projectNamespace = this.Namespace;
                proj.Configuration.CodeGenerationDetails.outputPath = this.OutputPath;

                proj.Schemas = new ProjectSchema.Schema[0];

                //proj.Tables = new ProjectSchema.Table[this.TableList.Count];
                //for (int i = 0; i < this.TableList.Count; i++)
                //{
                //    #region
                //    Table t = this.TableList[i];
                //    Schema.Table pt = new Schema.Table();
                //    proj.Tables[i] = pt;

                //    pt.dbName = t.DatabaseName;
                //    pt.schema = t.Schema;
                //    pt.name = t.Name;
                //    pt.description = t.Description;
                //    pt.build = t.BuildClass;

                //    if (t.PrimaryKey != null)
                //    {
                //        pt.PrimaryKey = new Schema.TablePrimaryKey();
                //        pt.PrimaryKey.name = t.PrimaryKey.Name;
                //        pt.PrimaryKey.ColumnRef = new Schema.TablePrimaryKeyColumnRef[t.PrimaryKey.Columns.Count];
                //        for (int j = 0; j < t.PrimaryKey.Columns.Count; j++)
                //        {
                //            pt.PrimaryKey.ColumnRef[j] = new Schema.TablePrimaryKeyColumnRef();
                //            pt.PrimaryKey.ColumnRef[j].@ref = t.PrimaryKey.Columns[j].Name;
                //        }
                //    }

                //    pt.Columns = new Schema.Column[t.Columns.Count];
                //    for (int j = 0; j < t.Columns.Count; j++)
                //    {
                //        Column c = t.Columns[j];
                //        Schema.Column pc = new Schema.Column();
                //        pt.Columns[j] = pc;

                //        pc.name = c.Name;
                //        pc.clrType = c.ClrType;
                //        pc.dbType = c.Type;
                //        pc.languageType = c.LanguageType;
                //        pc.isNullable = c.IsNullable;
                //        pc.isIdentity = c.IsIdentity;
                //        pc.isComputed = c.IsComputed;
                //        pc.isRowGuid = c.IsRowGuidCol;
                //        pc.enumeratedTypeName = c.EnumeratedTypeName;
                //        pc.length = c.Length;
                //        pc.precision = c.Precision;
                //        pc.propertyName = c.PropertyName;
                //        pc.defaultValue = c.DefaultValue;
                //        pc.scale = c.Scale;
                //    }
                //    #endregion
                //}

                sw = new StreamWriter(path, false);
                XmlSerializer serializer = new XmlSerializer(typeof(Project), new Type[] { 
                    typeof(Common.Table), typeof(Common.View), typeof(Common.Column), typeof(Common.Function), typeof(Common.Procedure), typeof(Common.Parameter) });
                serializer.Serialize(sw, this);
                m_FilePath = path;
            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                IsDirty = false;
                IsNew = false;
                if (sw != null)
                {
                    sw.Flush();
                    sw.Close();
                }
            }
        }

        public void SupressEvents()
        {
            m_SupressEvents = true;
        }

        public void ResumeEvents()
        {
            m_SupressEvents = false;
        }

        public ProjectSchema.Schema BuildSchema(XmlNode xNode)
        {
            ProjectSchema.Schema schema = new ProjectSchema.Schema();

            schema.Name = xNode.Attributes["name"].Value;

            #region add tables

            List<ProjectSchema.Table> tableList = new List<ProjectSchema.Table>();
            XmlNodeList xlist = xNode.SelectNodes(".//tables//table");
            foreach (XmlNode node in xlist)
            {
                tableList.Add(new Table(node));
            }
            schema.Tables = tableList.ToArray();

            #endregion

            #region add views

            List<ProjectSchema.View> viewList = new List<ProjectSchema.View>();
            xlist = xNode.SelectNodes(".//views//view");
            foreach (XmlNode node in xlist)
            {
                viewList.Add(new View(node));
            }
            schema.Views = viewList.ToArray();

            #endregion

            #region add functions

            List<ProjectSchema.Function> functionList = new List<ProjectSchema.Function>();
            xlist = xNode.SelectNodes(".//functions//function");
            foreach (XmlNode node in xlist)
            {
                functionList.Add(new Function(node));
            }
            schema.Functions = functionList.ToArray();

            #endregion

            #region add procedures

            List<ProjectSchema.Procedure> procedureList = new List<ProjectSchema.Procedure>();
            xlist = xNode.SelectNodes(".//procedures//procedure");
            foreach (XmlNode node in xlist)
            {
                procedureList.Add(new Procedure(node));
            }
            schema.Procedures = procedureList.ToArray();

            #endregion

            //SchemaList.Add(schema);

            return schema;
        }

        public void BuildView(string schema, XmlElement table)
        {

        }

        public void BuildFunction(string schema, XmlElement table)
        {

        }

        public void BuildProcedure(string schema, XmlElement table)
        {

        }

        #endregion

        #region private implementation

        private void OnChanged()
        {
            m_Dirty = true;

            if (!m_SupressEvents)
            {
                if (Changed != null)
                {
                    Changed(this, new ChangedEventArgs<Project>(this, ChangeType.Added));
                }
            }
        }

        #endregion

    }
}
