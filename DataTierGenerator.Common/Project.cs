using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SumDataTierGenerator.Common
{

    public partial class Project
    {

        #region events

        public event EventHandler<ChangedEventArgs<Project>> Changed;

        #endregion

        #region private and protected member variables

        private string m_FilePath = String.Empty;
        private string m_ProjectName = String.Empty;
        private bool m_Dirty = false;
        private bool m_New = true;
        private bool m_SupressEvents = false;

        #endregion

        #region constructors / desturctors

        public Project()
        {
            m_ProjectName = "New Project";
            Configuration = new Configuration();
            Configuration.CodeGenerationDetails = new CodeGenerationDetails();
            Configuration.DbConnectionDetails = new DbConnectionDetails();
            Configuration.CodeGenerationDetails.Namespace = "Namespace";
            Configuration.CodeGenerationDetails.OutputPath = Environment.CurrentDirectory;
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

        #endregion

        #region public methods

        public static Project Load(string path)
        {

            StreamReader sr = null;

            try
            {
                sr = new StreamReader(path);
                XmlSerializer serializer = new XmlSerializer(typeof(Project));

                Project proj = serializer.Deserialize(sr) as Project;
                if (proj == null)
                {
                    throw new Exception();
                }

                return proj;
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
            List<Schema> schemaList = new List<Schema>();

            SupressEvents();

            Schema schema;
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
                //Project proj = new Project();

                //proj.Configuration = new Configuration();
                //proj.Configuration.DbConnectionDetails = new DbConnectionDetails();
                //proj.Configuration.CodeGenerationDetails = new CodeGenerationDetails();

                //proj.Configuration.DbConnectionDetails.DbProviderType = this.Configuration.DbConnectionDetails.DbProviderType;
                //proj.Configuration.DbConnectionDetails.ConnectionString = this.Configuration.DbConnectionDetails.ConnectionString;
                //proj.Configuration.CodeGenerationDetails.Namespace = this.Configuration.CodeGenerationDetails.Namespace;
                //proj.Configuration.CodeGenerationDetails.OutputPath = this.Configuration.CodeGenerationDetails.OutputPath;

                //proj.Schemas = new Schema[0];

                sw = new StreamWriter(path, false);
                XmlSerializer serializer = new XmlSerializer(typeof(Project));
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

        public Schema BuildSchema(XmlNode xNode)
        {
            Schema schema = new Schema();

            schema.Name = xNode.Attributes["name"].Value;

            #region add tables

            List<Table> tableList = new List<Table>();
            XmlNodeList xlist = xNode.SelectNodes(".//tables//table");
            foreach (XmlNode node in xlist)
            {
                tableList.Add(new Table(node));
            }
            schema.Tables = tableList.ToArray();

            #endregion

            #region add views

            List<View> viewList = new List<View>();
            xlist = xNode.SelectNodes(".//views//view");
            foreach (XmlNode node in xlist)
            {
                viewList.Add(new View(node));
            }
            schema.Views = viewList.ToArray();

            #endregion

            #region add functions

            List<Function> functionList = new List<Function>();
            xlist = xNode.SelectNodes(".//functions//function");
            foreach (XmlNode node in xlist)
            {
                functionList.Add(new Function(node));
            }
            schema.Functions = functionList.ToArray();

            #endregion

            #region add procedures

            List<Procedure> procedureList = new List<Procedure>();
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
