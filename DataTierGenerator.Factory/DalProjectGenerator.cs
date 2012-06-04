using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.IO;
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
            StringBuilder itemGroup = new StringBuilder();
            string fileContents;
            string fullFileName;
            string dataMapping;
            string errorMessage;

            if (m_TableList.Count > 0)
            {

                dataMapping = Utility.GetResource("TotalSafety.DataTierGenerator.Resource.DataMapping.xml");

                m_DataMappingXml = new XmlDocument();
                m_DataMappingXml.LoadXml(dataMapping);

                #region ensure all the directories are created before creating files

                // create the directory if it does not exist
                if (!Directory.Exists(m_DalProjectDirectory))
                {
                    Directory.CreateDirectory(m_DalProjectDirectory);
                }

                // create the directory if it does not exist
                if (!Directory.Exists(m_DalProjectDirectory + "bin\\"))
                {
                    Directory.CreateDirectory(m_DalProjectDirectory + "bin\\");
                }

                // create the directory if it does not exist
                if (!Directory.Exists(m_DalProjectDirectory + "Common\\"))
                {
                    Directory.CreateDirectory(m_DalProjectDirectory + "Common\\");
                }

                // create the directory if it does not exist
                if (!Directory.Exists(m_DalProjectDirectory + "GeneratedClasses\\"))
                {
                    Directory.CreateDirectory(m_DalProjectDirectory + "GeneratedClasses\\");
                }

                // create the directory if it does not exist
                if (!Directory.Exists(m_DalProjectDirectory + "GeneratedClasses\\DataObject\\"))
                {
                    Directory.CreateDirectory(m_DalProjectDirectory + "GeneratedClasses\\DataObject\\");
                }

                // create the directory if it does not exist
                if (!Directory.Exists(m_DalProjectDirectory + "GeneratedClasses\\Gateway\\"))
                {
                    Directory.CreateDirectory(m_DalProjectDirectory + "GeneratedClasses\\Gateway\\");
                }

                #endregion

                #region create the common data layer files

                // create FieldDefinition
                fileContents = Utility.GetResource("TotalSafety.DataTierGenerator.Resource.FieldDefinition.cs");
                fileContents = fileContents.Replace("#ROOT_NAMESPACE#", m_DalNamespace);
                File.WriteAllText(m_DalProjectDirectory + "Common\\FieldDefinition.cs", fileContents);

                // create the TypeDefaultValue
                fileContents = Utility.GetResource("TotalSafety.DataTierGenerator.Resource.TypeDefaultValue.cs");
                fileContents = fileContents.Replace("#ROOT_NAMESPACE#", m_DalNamespace);
                File.WriteAllText(m_DalProjectDirectory + "Common\\TypeDefaultValue.cs", fileContents);

                // create the GatewayHelper
                fileContents = Utility.GetResource("TotalSafety.DataTierGenerator.Resource.GatewayHelper.cs");
                fileContents = fileContents.Replace("#ROOT_NAMESPACE#", m_DalNamespace);
                File.WriteAllText(m_DalProjectDirectory + "Common\\GatewayHelper.cs", fileContents);

                // create the IGateway
                fileContents = Utility.GetResource("TotalSafety.DataTierGenerator.Resource.IGateway.cs");
                fileContents = fileContents.Replace("#ROOT_NAMESPACE#", m_DalNamespace);
                File.WriteAllText(m_DalProjectDirectory + "Common\\IGateway.cs", fileContents);

                // create the IDataObject
                fileContents = Utility.GetResource("TotalSafety.DataTierGenerator.Resource.IFieldValues.cs");
                fileContents = fileContents.Replace("#ROOT_NAMESPACE#", m_DalNamespace);
                File.WriteAllText(m_DalProjectDirectory + "Common\\IFieldValues.cs", fileContents);

                // create the IDataObject
                fileContents = Utility.GetResource("TotalSafety.DataTierGenerator.Resource.IDataObject.cs");
                fileContents = fileContents.Replace("#ROOT_NAMESPACE#", m_DalNamespace);
                File.WriteAllText(m_DalProjectDirectory + "Common\\IDataObject.cs", fileContents);

                // create the Microsoft.Practices.EnterpriseLibrary.Common.dll
                Utility.SaveResourceFile("Microsoft.Practices.EnterpriseLibrary.Common.dll"
                    , m_DalProjectDirectory + "bin\\Microsoft.Practices.EnterpriseLibrary.Common.dll");

                // create the Microsoft.Practices.EnterpriseLibrary.Data.dll
                Utility.SaveResourceFile("Microsoft.Practices.EnterpriseLibrary.Data.dll"
                    , m_DalProjectDirectory + "bin\\Microsoft.Practices.EnterpriseLibrary.Data.dll");

                // create the Microsoft.Practices.EnterpriseLibrary.ObjectBuilder.dll
                Utility.SaveResourceFile("Microsoft.Practices.ObjectBuilder.dll"
                    , m_DalProjectDirectory + "bin\\Microsoft.Practices.ObjectBuilder.dll");

                #endregion

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

                if (m_GenProjectFile)
                {
                    // create Project File
                    projectFile = Utility.GetResource("TotalSafety.DataTierGenerator.Resource.ProjectTemplate.csproj");
                    projectFile = projectFile.Replace("$guid1$", Guid.NewGuid().ToString());
                    projectFile = projectFile.Replace("$safeprojectname$", m_DalNamespace);
                    projectFile = projectFile.Replace("$CompileItem$", itemGroup.ToString());
                    File.WriteAllText(m_DalProjectDirectory + m_DalNamespace + ".csproj", projectFile);
                }

            }


        }

        #endregion

    }

}
