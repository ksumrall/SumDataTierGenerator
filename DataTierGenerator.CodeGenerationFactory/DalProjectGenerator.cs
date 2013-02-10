using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Xml;
using SumDataTierGenerator.Common;

namespace SumDataTierGenerator.CodeGenerationFactory
{

    public class DalProjectGenerator
    {

        #region private and protected member variables

        private bool m_GenProjectFile;
        private string m_DalNamespace;
        private string m_DalProjectDirectory;
        private List<Schema> m_SchemaList;
        private List<Table> m_TableList;
        private List<View> m_ViewList;
        private List<Function> m_FunctionList;
        private List<Procedure> m_ProcedureList;
        private XmlDocument m_DataMappingXml;
        private string m_ProviderType;

        #endregion

        #region constructors / desturctors

        public DalProjectGenerator()
        {
            m_SchemaList = new List<Schema>();
            m_TableList = new List<Table>();
            m_ViewList = new List<View>();
            m_FunctionList = new List<Function>();
            m_ProcedureList = new List<Procedure>();
            m_GenProjectFile = false;
            m_ProviderType = "";
        }

        public DalProjectGenerator(Project project)
            : this()
        {
            m_DalNamespace = project.Configuration.CodeGenerationDetails.Namespace;
            m_DalProjectDirectory = project.Configuration.CodeGenerationDetails.OutputPath;
            m_ProviderType = project.Configuration.DbConnectionDetails.DbProviderType;
            if (!m_DalProjectDirectory.EndsWith("\\"))
            {
                m_DalProjectDirectory += "\\";
            }
            m_SchemaList.AddRange(project.Schemas);
        }

        #endregion

        #region public properties

        public string DalNamespace
        {
            get { return m_DalNamespace; }
            set { m_DalNamespace = value; }
        }

        public string DalProjectDirectory
        {
            get { return m_DalProjectDirectory; }
            set { m_DalProjectDirectory = value; }
        }

        public List<Schema> SchemaList
        {
            get { return m_SchemaList; }
        }

        public List<Table> TableList
        {
            get { return m_TableList; }
        }

        public List<View> ViewList
        {
            get { return m_ViewList; }
        }

        public List<Function> FunctionList
        {
            get { return m_FunctionList; }
        }

        public List<Procedure> ProcedureList
        {
            get { return m_ProcedureList; }
        }

        #endregion

        #region public methods

        public void GenerateDalProject()
        {
            StringBuilder itemGroup = new StringBuilder();

            CreateDirectoryStructure(m_DalProjectDirectory);

            CreateCommonDataLayerFiles();

            foreach (Schema schema in SchemaList)
            {
                itemGroup.AppendLine(GenerateTables(m_DalNamespace, m_DalProjectDirectory, m_ProviderType, new List<Table>(schema.Tables)));

                itemGroup.AppendLine(GenerateViews(m_DalNamespace, m_DalProjectDirectory, m_ProviderType, new List<View>(schema.Views)));

                //itemGroup.AppendLine(GenerateFunctions(m_DalNamespace, m_DalProjectDirectory, m_ProviderType, new List<Function>(schema.Function)));

                //itemGroup.AppendLine(GenerateProcedures(m_DalNamespace, m_DalProjectDirectory, m_ProviderType, new List<Procedure>(schema.Procedure)));
            }

            if (m_GenProjectFile)
            {
                // create Project File
                string projectFile;
                projectFile = Utility.GetResource(Assembly.GetExecutingAssembly(), "SumDataTierGenerator.CodeGenerationFactory.EmbeddedResources.ProjectTemplate.csproj");
                projectFile = projectFile.Replace("$guid1$", Guid.NewGuid().ToString());
                projectFile = projectFile.Replace("$safeprojectname$", m_DalNamespace);
                projectFile = projectFile.Replace("$CompileItem$", itemGroup.ToString());
                File.WriteAllText(m_DalProjectDirectory + m_DalNamespace + ".csproj", projectFile);
            }
        }

        #endregion

        #region private implementation

        private void CreateDirectoryStructure(string rootPath)
        {

            string[] objectDirectories = new string[] { 
                "UserDataObjects\\"
                , "UserGateway\\"
                , "GeneratedClasses\\DataObject\\"
                , "GeneratedClasses\\Gateway\\"
            };

            //If the directory already exists, CreateDirectory method does nothing.
            // create the directory if it does not exist
            Directory.CreateDirectory(rootPath);

            // create the directory if it does not exist
            Directory.CreateDirectory(rootPath + "bin\\");

            // create the directory if it does not exist
            Directory.CreateDirectory(rootPath + "Common\\");

            // create the directory if it does not exist
            Directory.CreateDirectory(rootPath + "GeneratedClasses\\");

            foreach (string path in objectDirectories)
            {
                // create the directory if it does not exist
                Directory.CreateDirectory(rootPath + path);

                // create the directory if it does not exist
                Directory.CreateDirectory(rootPath + path + "Tables\\");

                // create the directory if it does not exist
                Directory.CreateDirectory(rootPath + path + "Views\\");

                // create the directory if it does not exist
                Directory.CreateDirectory(rootPath + path + "Functions\\");

                // create the directory if it does not exist
                Directory.CreateDirectory(rootPath + path + "Procedures\\");
            }

        }

        private void CreateCommonDataLayerFiles()
        {
            string fileContents;

            // create FieldDefinition
            fileContents = Utility.GetResource(Assembly.GetExecutingAssembly(), "SumDataTierGenerator.CodeGenerationFactory.EmbeddedResources.FieldDefinition.cs");
            fileContents = fileContents.Replace("#ROOT_NAMESPACE#", m_DalNamespace);
            File.WriteAllText(m_DalProjectDirectory + "Common\\FieldDefinition.cs", fileContents);

            // create the TypeDefaultValue
            fileContents = Utility.GetResource(Assembly.GetExecutingAssembly(), "SumDataTierGenerator.CodeGenerationFactory.EmbeddedResources.TypeDefaultValue.cs");
            fileContents = fileContents.Replace("#ROOT_NAMESPACE#", m_DalNamespace);
            File.WriteAllText(m_DalProjectDirectory + "Common\\TypeDefaultValue.cs", fileContents);

            // create the GatewayHelper
            switch (m_ProviderType)
            {
                case "Microsoft SQL Server (SqlClient)":
                    fileContents = Utility.GetResource(Assembly.GetExecutingAssembly(), "SumDataTierGenerator.CodeGenerationFactory.EmbeddedResources.GatewayHelper.cs");
                    break;

                case "Microsoft SQL Server Compact 3.5 (SqlCeClient)":
                    fileContents = Utility.GetResource(Assembly.GetExecutingAssembly(), "SumDataTierGenerator.CodeGenerationFactory.EmbeddedResources.GatewayHelperSqlCe.cs");
                    break;
            }
            fileContents = fileContents.Replace("#ROOT_NAMESPACE#", m_DalNamespace);
            File.WriteAllText(m_DalProjectDirectory + "Common\\GatewayHelper.cs", fileContents);

            // create the IGateway
            fileContents = Utility.GetResource(Assembly.GetExecutingAssembly(), "SumDataTierGenerator.CodeGenerationFactory.EmbeddedResources.IGateway.cs");
            fileContents = fileContents.Replace("#ROOT_NAMESPACE#", m_DalNamespace);
            File.WriteAllText(m_DalProjectDirectory + "Common\\IGateway.cs", fileContents);

            // create the IFieldValues
            fileContents = Utility.GetResource(Assembly.GetExecutingAssembly(), "SumDataTierGenerator.CodeGenerationFactory.EmbeddedResources.IFieldValues.cs");
            fileContents = fileContents.Replace("#ROOT_NAMESPACE#", m_DalNamespace);
            File.WriteAllText(m_DalProjectDirectory + "Common\\IFieldValues.cs", fileContents);

            // create the IDataObject
            fileContents = Utility.GetResource(Assembly.GetExecutingAssembly(), "SumDataTierGenerator.CodeGenerationFactory.EmbeddedResources.IDataObject.cs");
            fileContents = fileContents.Replace("#ROOT_NAMESPACE#", m_DalNamespace);
            File.WriteAllText(m_DalProjectDirectory + "Common\\IDataObject.cs", fileContents);

            // create the Microsoft.Practices.EnterpriseLibrary.Common.dll
            Utility.SaveResourceFile(Assembly.GetExecutingAssembly(), "SumDataTierGenerator.CodeGenerationFactory.EmbeddedResources.Microsoft.Practices.EnterpriseLibrary.Common.dll"
                , m_DalProjectDirectory + "bin\\Microsoft.Practices.EnterpriseLibrary.Common.dll");

            // create the Microsoft.Practices.EnterpriseLibrary.Data.dll
            switch (m_ProviderType)
            {
                case "Microsoft SQL Server (SqlClient)":
                    Utility.SaveResourceFile(Assembly.GetExecutingAssembly(), "SumDataTierGenerator.CodeGenerationFactory.EmbeddedResources.Microsoft.Practices.EnterpriseLibrary.Data.dll"
                        , m_DalProjectDirectory + "bin\\Microsoft.Practices.EnterpriseLibrary.Data.dll");
                    break;

                case "Microsoft SQL Server Compact 3.5 (SqlCeClient)":
                    Utility.SaveResourceFile(Assembly.GetExecutingAssembly(), "SumDataTierGenerator.CodeGenerationFactory.EmbeddedResources.Microsoft.Practices.EnterpriseLibrary.Data.SqlCe.dll"
                , m_DalProjectDirectory + "bin\\Microsoft.Practices.EnterpriseLibrary.Data.SqlCe.dll");
                    break;
            }

            // create the Microsoft.Practices.EnterpriseLibrary.ObjectBuilder.dll
            Utility.SaveResourceFile(Assembly.GetExecutingAssembly(), "SumDataTierGenerator.CodeGenerationFactory.EmbeddedResources.Microsoft.Practices.ObjectBuilder.dll"
                , m_DalProjectDirectory + "bin\\Microsoft.Practices.ObjectBuilder.dll");

        }

        #region table creation implementation

        private string GenerateTables(string projectNamespace, string projectDirectory, string providerType, List<Table> tableList)
        {
            StringBuilder itemGroup = new StringBuilder();

            if (tableList.Count > 0)
            {
                foreach (Table table in tableList)
                {
                    if (1 == 1 || table.Build)
                    {
                        itemGroup.AppendLine(GenerateTableDataGatewayGeneratedClass(projectNamespace, projectDirectory, providerType, table));

                        itemGroup.AppendLine(GenerateTableDataGatewayUserClass(projectNamespace, projectDirectory, providerType, table));

                        itemGroup.AppendLine(GenerateTableDataEntityGeneratedClass(projectNamespace, projectDirectory, providerType, table));
                    }
                }
            }

            return itemGroup.ToString();
        }

        private string GenerateTableDataGatewayGeneratedClass(string projectNamespace, string projectDirectory, string providerType, Table table)
        {
            string fullFileName;
            string fileContents;
            string errorMessage;
            string includeFile;

            try
            {

                GeneratedTableGateway generatedGateway =
                    new GeneratedTableGateway(projectNamespace, providerType, table);

                fullFileName = projectDirectory + "GeneratedClasses\\Gateway\\Tables\\" + generatedGateway.FILE_NAME;

                fileContents = generatedGateway.ToString();
                File.WriteAllText(fullFileName, fileContents);

                includeFile = "\t\t<Compile Include=\"GeneratedClasses\\Gateway\\Tables\\" + generatedGateway.FILE_NAME + "\" />";

            }
            catch (Exception exp)
            {
                errorMessage = "Error while creating the generated file for table - " + table.Name + Environment.NewLine;
                errorMessage += "Internal error message:\n";
                errorMessage += exp.Message;

                throw new Exception(errorMessage, exp);
            }

            return includeFile;
        }

        private string GenerateTableDataGatewayUserClass(string projectNamespace, string projectDirectory, string providerType, Table table)
        {
            string fullFileName;
            string fileContents;
            string errorMessage;
            string includeFile;

            try
            {

                UserTableGateway userGateway =
                    new UserTableGateway(projectNamespace, providerType, table);

                fullFileName = projectDirectory + "UserGateway\\Tables\\" + userGateway.CLASS_NAME + ".cs";

                if (!File.Exists(fullFileName))
                {

                    fileContents = userGateway.ToString();

                    File.WriteAllText(fullFileName, fileContents);

                }

                includeFile = "\t\t<Compile Include=\"UserGateway\\Tables\\" + userGateway.CLASS_NAME + ".cs" + "\" />";

            }
            catch (Exception exp)
            {
                errorMessage = "Error while creating the user file for table - " + table.Name + Environment.NewLine;
                errorMessage += "Internal error message:\n";
                errorMessage += exp.Message;

                throw new Exception(errorMessage, exp);
            }

            return includeFile;
        }

        private string GenerateTableDataEntityGeneratedClass(string projectNamespace, string projectDirectory, string providerType, Table table)
        {
            string fullFileName;
            string fileContents;
            string errorMessage;
            string includeFile;

            try
            {

                GeneratedTableDataObject dataObjectGenerator = new GeneratedTableDataObject(projectNamespace, providerType, table);

                fullFileName = projectDirectory + "GeneratedClasses\\DataObject\\Tables\\" + dataObjectGenerator.CLASS_NAME + "_Generated.cs";

                fileContents = dataObjectGenerator.ToString();
                File.WriteAllText(fullFileName, fileContents);

                includeFile = "\t\t<Compile Include=\"GeneratedClasses\\DataObject\\Tables\\" + dataObjectGenerator.CLASS_NAME + "_Generated.cs" + "\" />";

            }
            catch (Exception exp)
            {
                errorMessage = "Error while creating the data object file for table - " + table.Name + Environment.NewLine;
                errorMessage += "Internal error message:\n";
                errorMessage += exp.Message;

                throw new Exception(errorMessage, exp);
            }

            return includeFile;
        }

        #endregion

        #region view creation implementation

        private string GenerateViews(string projectNamespace, string projectDirectory, string providerType, List<View> viewList)
        {
            StringBuilder itemGroup = new StringBuilder();

            if (viewList.Count > 0)
            {
                foreach (View view in viewList)
                {
                    if (1 == 1 || view.Build)
                    {
                        itemGroup.AppendLine(GenerateViewDataGatewayGeneratedClass(projectNamespace, projectDirectory, providerType, view));

                        itemGroup.AppendLine(GenerateViewDataGatewayUserClass(projectNamespace, projectDirectory, providerType, view));

                        itemGroup.AppendLine(GenerateViewDataEntityGeneratedClass(projectNamespace, projectDirectory, providerType, view));
                    }
                }
            }

            return itemGroup.ToString();
        }

        private string GenerateViewDataGatewayGeneratedClass(string projectNamespace, string projectDirectory, string providerType, View view)
        {
            string fullFileName;
            string fileContents;
            string errorMessage;
            string includeFile;

            try
            {

                GeneratedViewGateway generatedGateway =
                    new GeneratedViewGateway(projectNamespace, providerType, view);

                fullFileName = projectDirectory + "GeneratedClasses\\Gateway\\Views\\" + generatedGateway.CLASS_NAME + "_Generated.cs";

                fileContents = generatedGateway.ToString();
                File.WriteAllText(fullFileName, fileContents);

                includeFile = "\t\t<Compile Include=\"GeneratedClasses\\Gateway\\Views\\" + generatedGateway.CLASS_NAME + "_Generated.cs" + "\" />";

            }
            catch (Exception exp)
            {
                errorMessage = "Error while creating the generated file for table - " + view.Name + Environment.NewLine;
                errorMessage += "Internal error message:\n";
                errorMessage += exp.Message;

                throw new Exception(errorMessage, exp);
            }

            return includeFile;
        }

        private string GenerateViewDataGatewayUserClass(string projectNamespace, string projectDirectory, string providerType, View view)
        {
            string fullFileName;
            string fileContents;
            string errorMessage;
            string includeFile;

            try
            {

                UserViewGateway userGateway =
                    new UserViewGateway(projectNamespace, providerType, view);

                fullFileName = projectDirectory + "UserGateway\\Views\\" + userGateway.CLASS_NAME + ".cs";

                if (!File.Exists(fullFileName))
                {

                    fileContents = userGateway.ToString();

                    File.WriteAllText(fullFileName, fileContents);

                }

                includeFile = "\t\t<Compile Include=\"UserGateway\\Views\\" + userGateway.CLASS_NAME + ".cs" + "\" />";

            }
            catch (Exception exp)
            {
                errorMessage = "Error while creating the user file for table - " + view.Name + Environment.NewLine;
                errorMessage += "Internal error message:\n";
                errorMessage += exp.Message;

                throw new Exception(errorMessage, exp);
            }

            return includeFile;
        }

        private string GenerateViewDataEntityGeneratedClass(string projectNamespace, string projectDirectory, string providerType, View view)
        {
            string fullFileName;
            string fileContents;
            string errorMessage;
            string includeFile;

            try
            {

                GeneratedViewDataObject dataObjectGenerator = new GeneratedViewDataObject(projectNamespace, providerType, view);

                fullFileName = projectDirectory + "GeneratedClasses\\DataObject\\Views\\" + dataObjectGenerator.CLASS_NAME + "_Generated.cs";

                fileContents = dataObjectGenerator.ToString();
                File.WriteAllText(fullFileName, fileContents);

                includeFile = "\t\t<Compile Include=\"GeneratedClasses\\DataObject\\Views\\" + dataObjectGenerator.CLASS_NAME + "_Generated.cs" + "\" />";

            }
            catch (Exception exp)
            {
                errorMessage = "Error while creating the data object file for table - " + view.Name + Environment.NewLine;
                errorMessage += "Internal error message:\n";
                errorMessage += exp.Message;

                throw new Exception(errorMessage, exp);
            }

            return includeFile;
        }

        #endregion

        #region function creation implementation

        private string GenerateFunctions(string projectNamespace, string projectDirectory, List<Function> functionList)
        {
            StringBuilder itemGroup = new StringBuilder();

            if (functionList.Count > 0)
            {
                foreach (Function function in functionList)
                {
                    //itemGroup.AppendLine(GenerateViewDataGatewayGeneratedClass(projectNamespace, projectDirectory, view));

                    //itemGroup.AppendLine(GenerateViewDataGatewayUserClass(projectNamespace, projectDirectory, view));

                    //itemGroup.AppendLine(GenerateViewDataEntityGeneratedClass(projectNamespace, projectDirectory, view));
                }
            }

            return itemGroup.ToString();
        }

        #endregion

        #region procedure creation implementation

        private string GenerateProcedures(string projectNamespace, string projectDirectory, List<Procedure> procedureList)
        {
            StringBuilder itemGroup = new StringBuilder();

            if (procedureList.Count > 0)
            {
                foreach (Procedure procedure in procedureList)
                {
                    //itemGroup.AppendLine(GenerateViewDataGatewayGeneratedClass(projectNamespace, projectDirectory, view));

                    //itemGroup.AppendLine(GenerateViewDataGatewayUserClass(projectNamespace, projectDirectory, view));

                    //itemGroup.AppendLine(GenerateViewDataEntityGeneratedClass(projectNamespace, projectDirectory, view));
                }
            }

            return itemGroup.ToString();
        }

        #endregion

        #endregion
    }

}
