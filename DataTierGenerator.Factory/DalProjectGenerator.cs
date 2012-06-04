using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Xml;
using TotalSafety.DataTierGenerator.Common;

namespace TotalSafety.DataTierGenerator.Factory
{

    public class DalProjectGenerator
    {

        #region private and protected member variables

        private string m_DalNamespace;
        private List<Table> m_TableList;
        private string m_DalProjectDirectory;
        private XmlDocument m_DataMappingXml;
        private bool m_GenProjectFile;

        #endregion

        #region constructors / desturctors

        public DalProjectGenerator()
        {
            m_TableList = new List<Table>();
            m_GenProjectFile = false;
        }

        public DalProjectGenerator(Project project)
        {
            m_DalNamespace = project.Namespace;
            m_DalProjectDirectory = project.OutputPath;

            // add tables
            foreach (Table table in project.TableList)
            {
                if (table.BuildClass)
                {
                    m_TableList.Add(table);
                }
            }

            // add views

            // add procedures

        }

        public DalProjectGenerator(string filePath)
        {
            // load the file and create a project object
        }

        #endregion

        #region public properties

        public string DalNamespace
        {
            get { return m_DalNamespace; }
            set { m_DalNamespace = value; }
        }

        public List<Table> TableList
        {
            get { return m_TableList; }
        }

        public string DalProjectDirectory
        {
            get { return m_DalProjectDirectory; }
            set { m_DalProjectDirectory = value; }
        }

        #endregion

        #region public methods

        public void GenerateDalProject()
        {

            string projectFile;
            string dataMapping;

            if (m_TableList.Count > 0)
            {
                StringBuilder itemGroup;

                dataMapping = Utility.GetResource(Assembly.GetExecutingAssembly(), "TotalSafety.DataTierGenerator.Factory.EmbeddedResources.DataMapping.xml");

                m_DataMappingXml = new XmlDocument();
                m_DataMappingXml.LoadXml(dataMapping);

                CreateDirectoryStructure(m_DalProjectDirectory);

                CreateCommonDataLayerFiles();

                itemGroup = GenerateTableClassFiles();

                if (m_GenProjectFile)
                {
                    // create Project File
                    projectFile = Utility.GetResource(Assembly.GetExecutingAssembly(), "TotalSafety.DataTierGenerator.Factory.EmbeddedResources.ProjectTemplate.csproj");
                    projectFile = projectFile.Replace("$guid1$", Guid.NewGuid().ToString());
                    projectFile = projectFile.Replace("$safeprojectname$", m_DalNamespace);
                    projectFile = projectFile.Replace("$CompileItem$", itemGroup.ToString());
                    File.WriteAllText(m_DalProjectDirectory + m_DalNamespace + ".csproj", projectFile);
                }

            }


        }

        #endregion

        #region private implementation

        private void CreateDirectoryStructure(string rootPath)
        {

            //If the directory already exists, CreateDirectory method does nothing.
            // create the directory if it does not exist
            Directory.CreateDirectory(rootPath);

            // create the directory if it does not exist
            Directory.CreateDirectory(rootPath + "bin\\");

            // create the directory if it does not exist
            Directory.CreateDirectory(rootPath + "Common\\");

            // create the directory if it does not exist
            Directory.CreateDirectory(rootPath + "GeneratedClasses\\");

            // create the directory if it does not exist
            Directory.CreateDirectory(rootPath + "GeneratedClasses\\DataObject\\");

            // create the directory if it does not exist
            Directory.CreateDirectory(rootPath + "GeneratedClasses\\Gateway\\");

        }

        private void CreateCommonDataLayerFiles()
        {
            string fileContents;

            // create FieldDefinition
            fileContents = Utility.GetResource(Assembly.GetExecutingAssembly(), "TotalSafety.DataTierGenerator.Factory.EmbeddedResources.FieldDefinition.cs");
            fileContents = fileContents.Replace("#ROOT_NAMESPACE#", m_DalNamespace);
            File.WriteAllText(m_DalProjectDirectory + "Common\\FieldDefinition.cs", fileContents);

            // create the TypeDefaultValue
            fileContents = Utility.GetResource(Assembly.GetExecutingAssembly(), "TotalSafety.DataTierGenerator.Factory.EmbeddedResources.TypeDefaultValue.cs");
            fileContents = fileContents.Replace("#ROOT_NAMESPACE#", m_DalNamespace);
            File.WriteAllText(m_DalProjectDirectory + "Common\\TypeDefaultValue.cs", fileContents);

            // create the GatewayHelper
            fileContents = Utility.GetResource(Assembly.GetExecutingAssembly(), "TotalSafety.DataTierGenerator.Factory.EmbeddedResources.GatewayHelper.cs");
            fileContents = fileContents.Replace("#ROOT_NAMESPACE#", m_DalNamespace);
            File.WriteAllText(m_DalProjectDirectory + "Common\\GatewayHelper.cs", fileContents);

            // create the IGateway
            fileContents = Utility.GetResource(Assembly.GetExecutingAssembly(), "TotalSafety.DataTierGenerator.Factory.EmbeddedResources.IGateway.cs");
            fileContents = fileContents.Replace("#ROOT_NAMESPACE#", m_DalNamespace);
            File.WriteAllText(m_DalProjectDirectory + "Common\\IGateway.cs", fileContents);

            // create the IDataObject
            fileContents = Utility.GetResource(Assembly.GetExecutingAssembly(), "TotalSafety.DataTierGenerator.Factory.EmbeddedResources.IFieldValues.cs");
            fileContents = fileContents.Replace("#ROOT_NAMESPACE#", m_DalNamespace);
            File.WriteAllText(m_DalProjectDirectory + "Common\\IFieldValues.cs", fileContents);

            // create the IDataObject
            fileContents = Utility.GetResource(Assembly.GetExecutingAssembly(), "TotalSafety.DataTierGenerator.Factory.EmbeddedResources.IDataObject.cs");
            fileContents = fileContents.Replace("#ROOT_NAMESPACE#", m_DalNamespace);
            File.WriteAllText(m_DalProjectDirectory + "Common\\IDataObject.cs", fileContents);

            // create the Microsoft.Practices.EnterpriseLibrary.Common.dll
            Utility.SaveResourceFile(Assembly.GetExecutingAssembly(), "TotalSafety.DataTierGenerator.Factory.EmbeddedResources.Microsoft.Practices.EnterpriseLibrary.Common.dll"
                , m_DalProjectDirectory + "bin\\Microsoft.Practices.EnterpriseLibrary.Common.dll");

            // create the Microsoft.Practices.EnterpriseLibrary.Data.dll
            Utility.SaveResourceFile(Assembly.GetExecutingAssembly(), "TotalSafety.DataTierGenerator.Factory.EmbeddedResources.Microsoft.Practices.EnterpriseLibrary.Data.dll"
                , m_DalProjectDirectory + "bin\\Microsoft.Practices.EnterpriseLibrary.Data.dll");

            // create the Microsoft.Practices.EnterpriseLibrary.ObjectBuilder.dll
            Utility.SaveResourceFile(Assembly.GetExecutingAssembly(), "TotalSafety.DataTierGenerator.Factory.EmbeddedResources.Microsoft.Practices.ObjectBuilder.dll"
                , m_DalProjectDirectory + "bin\\Microsoft.Practices.ObjectBuilder.dll");

        }

        private StringBuilder GenerateTableClassFiles()
        {
            string fullFileName;
            string fileContents;
            string errorMessage;
            StringBuilder itemGroup = new StringBuilder();

            // Create everything we need
            foreach (Table table in m_TableList)
            {

                #region create the Generated Gateway for the table

                try
                {

                    GeneratedGateway generatedGateway =
                        new GeneratedGateway(m_DalNamespace, table);

                    fullFileName = m_DalProjectDirectory + "GeneratedClasses\\Gateway\\" + generatedGateway.CLASS_NAME + "_Generated.cs";

                    fileContents = generatedGateway.ToString();
                    File.WriteAllText(fullFileName, fileContents);

                    itemGroup.Append("\t\t<Compile Include=\"GeneratedClasses\\Gateway\\" + generatedGateway.CLASS_NAME + "_Generated.cs" + "\" />\n");

                }
                catch (Exception exp)
                {
                    errorMessage = "Error while creating the generated file for table - " + table.Name + Environment.NewLine;
                    errorMessage += "Internal error message:\n";
                    errorMessage += exp.Message;

                    throw new Exception(errorMessage, exp);
                }

                #endregion

                #region create the User Gateway for the table

                try
                {

                    UserGateway userGateway =
                        new UserGateway(m_DalNamespace, table);

                    fullFileName = m_DalProjectDirectory + userGateway.CLASS_NAME + ".cs";

                    if (!File.Exists(fullFileName))
                    {

                        fileContents = userGateway.ToString();

                        File.WriteAllText(fullFileName, fileContents);

                    }

                    itemGroup.Append("\t\t<Compile Include=\"" + userGateway.CLASS_NAME + ".cs" + "\" />\n");

                }
                catch (Exception exp)
                {
                    errorMessage = "Error while creating the user file for table - " + table.Name + Environment.NewLine;
                    errorMessage += "Internal error message:\n";
                    errorMessage += exp.Message;

                    throw new Exception(errorMessage, exp);
                }

                #endregion

                #region create the Generated Data Entity for the table

                try
                {

                    GeneratedDataObject dataObjectGenerator = new GeneratedDataObject(m_DalNamespace, table);

                    fullFileName = m_DalProjectDirectory + "GeneratedClasses\\DataObject\\" + dataObjectGenerator.CLASS_NAME + "_Generated.cs";

                    fileContents = dataObjectGenerator.ToString();
                    File.WriteAllText(fullFileName, fileContents);

                    //CsEntityGenerator entityGenerator = new CsEntityGenerator();
                    //entityGenerator.GenerateEntity( table, m_DalNamespace, m_DataMappingXml, m_DalProjectDirectory + "DataObjects\\" );

                    itemGroup.Append("\t\t<Compile Include=\"GeneratedClasses\\DataObject\\" + dataObjectGenerator.CLASS_NAME + "_Generated.cs" + "\" />\n");

                }
                catch (Exception exp)
                {
                    errorMessage = "Error while creating the data object file for table - " + table.Name + Environment.NewLine;
                    errorMessage += "Internal error message:\n";
                    errorMessage += exp.Message;

                    throw new Exception(errorMessage, exp);
                }

                #endregion

            }

            return itemGroup;
        }

        #endregion
    }

}
