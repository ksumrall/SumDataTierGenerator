using System.Collections;
using System.IO;
using System.Text;

namespace PriorityIt.DataTierGenerator
{
	internal class CsGenerator
	{
		private CsGenerator() {}

		/// <summary>
		/// Creates a C# data access class for all of the table's stored procedures.
		/// </summary>
		/// <param name="table">Instance of the Table class that represents the table this class will be created for.</param>
		/// <param name="storedProcedurePrefix">Prefix to be appended to the name of the stored procedure.</param>
		/// <param name="path">Path where the class should be created.</param>
		internal static void CreateDataAccessClass(Table table, string targetNamespace, string storedProcedurePrefix, string path)
		{
			string className = Utility.CleanWhitespace(Utility.FormatPascal(table.ProgrammaticAlias));

			// Create the header for the class
			StreamWriter streamWriter = new StreamWriter(path + className + ".cs");
			streamWriter.WriteLine("using System;");
			streamWriter.WriteLine("using System.Data;");
			streamWriter.WriteLine("using Microsoft.Practices.EnterpriseLibrary.Data;");
			streamWriter.WriteLine();
			streamWriter.WriteLine("namespace " + targetNamespace + "\n{");

			streamWriter.WriteLine("\tpublic sealed class " + className + "\n\t{");
			streamWriter.WriteLine("\t\tprivate " + className + "() {}");

			// Append the access methods
			CreateInsertMethod(table, storedProcedurePrefix, streamWriter);
			CreateUpdateMethod(table, storedProcedurePrefix, streamWriter);
			CreateDeleteMethod(table, storedProcedurePrefix, streamWriter);
			CreateDeleteByMethods(table, storedProcedurePrefix, streamWriter);
			CreateSelectMethod(table, storedProcedurePrefix, streamWriter);
			CreateSelectAllMethod(table, storedProcedurePrefix, streamWriter);
			CreateSelectByMethods(table, storedProcedurePrefix, streamWriter);

			// Close out the class and namespace
			streamWriter.WriteLine("\t}");
			streamWriter.WriteLine("}");

			// Flush and close the stream
			streamWriter.Flush();
			streamWriter.Close();
		}

		/// <summary>
		/// Creates a string that represents the insert functionality of the data access class.
		/// </summary>
		/// <param name="table">The Table instance that this method will be created for.</param>
		/// <param name="storedProcedurePrefix">The prefix that is used on the stored procedure that this method will call.</param>
		/// <param name="streamWriter">The StreamWriter instance that will be used to create the method.</param>
		private static void CreateInsertMethod(Table table, string storedProcedurePrefix, StreamWriter streamWriter)
		{
			// Append the method header
			streamWriter.WriteLine();
			streamWriter.WriteLine("\t\t/// <summary>");
			streamWriter.WriteLine("\t\t/// Inserts a record into the " + table.Name + " table.");
			streamWriter.WriteLine("\t\t/// </summary>");

			// Determine the return type of the insert function
			bool returnVoid = true;
			foreach (Column column in table.Columns) {
				if (column.IsIdentity) {
					streamWriter.Write("\t\tpublic static int Insert(");
					returnVoid = false;
					break;
				} else if (column.IsRowGuidCol) {
					streamWriter.Write("\t\tpublic static Guid Insert(");
					returnVoid = false;
					break;
				}
			}

			// If the return type hasn't been set, return void
			if (returnVoid) {
				streamWriter.Write("\t\tpublic static void Insert(");
			}

			// Append the method call parameters
			int remainingColumns = table.Columns.Count;
			foreach (Column column in table.Columns) {
				remainingColumns--;

				if (column.IsIdentity == false && column.IsRowGuidCol == false) {
					if (remainingColumns > 0) {
						streamWriter.Write(Utility.CreateMethodParameter(column) + ", ");
					} else {
						streamWriter.Write(Utility.CreateMethodParameter(column));
					}
				}
			}

			streamWriter.WriteLine(")\n\t\t{");

			bool executeScalar = false;
			// Append the parameter value extraction
			foreach (Column column in table.Columns) {
				if (column.IsIdentity || column.IsRowGuidCol) {
					if (column.IsIdentity) {
						streamWriter.WriteLine("\t\t\tDatabase myDatabase = DatabaseFactory.CreateDatabase();");
						streamWriter.WriteLine("\t\t\tDBCommandWrapper myCommand = myDatabase.GetStoredProcCommandWrapper(\"" + storedProcedurePrefix + table.Name + "Insert\");\n");
					} else {
						streamWriter.WriteLine("\t\t\tDatabase myDatabase = DatabaseFactory.CreateDatabase();");
						streamWriter.WriteLine("\t\t\tDBCommandWrapper myCommand = myDatabase.GetStoredProcCommandWrapper(\"" + storedProcedurePrefix + table.Name + "Insert\");\n");
					}

					executeScalar = true;
				}
			}

			if (executeScalar == false) {
				streamWriter.WriteLine("\t\t\tDatabase myDatabase = DatabaseFactory.CreateDatabase();");
				streamWriter.WriteLine("\t\t\tDBCommandWrapper myCommand = myDatabase.GetStoredProcCommandWrapper(\"" + storedProcedurePrefix + table.Name + "Insert\");\n");
			}

			// Append the parameters
			StringBuilder builder = new StringBuilder();
			foreach (Column column in table.Columns) {
				if (column.IsIdentity == false && column.IsRowGuidCol == false) {
					builder.Append("\t\t\t" + Utility.CreateSqlParameter(column) + ";\n");
				}
			}
			streamWriter.WriteLine(builder.ToString());

			// Append the execution query
			foreach (Column column in table.Columns) {
				if (column.IsIdentity || column.IsRowGuidCol) {
					if (column.IsIdentity) {
						streamWriter.WriteLine("\t\t\t// Execute the query and return the new identity value");
						streamWriter.WriteLine("\t\t\treturn Convert.ToInt32(myDatabase.ExecuteScalar(myCommand));");
					} else {
						streamWriter.WriteLine("\t\t\t// Execute the query and return the new GUID");
						streamWriter.WriteLine("\t\t\tGuid myGuid = new Guid(Convert.ToString(myDatabase.ExecuteScalar(myCommand)));\n");
						streamWriter.WriteLine("\t\t\treturn myGuid;");
					}
				}
			}

			if (executeScalar == false) {
				streamWriter.WriteLine("\t\t\tmyDatabase.ExecuteNonQuery(myCommand);");
			}

			// Append the method footer
			streamWriter.WriteLine("\t\t}");
		}

		/// <summary>
		/// Creates a string that represents the update functionality of the data access class.
		/// </summary>
		/// <param name="table">The Table instance that this method will be created for.</param>
		/// <param name="storedProcedurePrefix">The prefix that is used on the stored procedure that this method will call.</param>
		/// <param name="streamWriter">The StreamWriter instance that will be used to create the method.</param>
		private static void CreateUpdateMethod(Table table, string storedProcedurePrefix, StreamWriter streamWriter)
		{
			if (table.PrimaryKeys.Count > 0 && table.Columns.Count != table.PrimaryKeys.Count && table.Columns.Count != table.ForeignKeys.Count) {
				// Append the method header
				streamWriter.WriteLine();
				streamWriter.WriteLine("\t\t/// <summary>");
				streamWriter.WriteLine("\t\t/// Updates a record in the " + table.Name + " table.");
				streamWriter.WriteLine("\t\t/// </summary>");

				streamWriter.Write("\t\tpublic static void Update(");
				int remainingColumns = table.Columns.Count;
				foreach (Column column in table.Columns) {
					remainingColumns--;

					if (remainingColumns > 0) {
						streamWriter.Write(Utility.CreateMethodParameter(column) + ", ");
					} else {
						streamWriter.Write(Utility.CreateMethodParameter(column));
					}
				}
				streamWriter.WriteLine(")\n\t\t{");

				// Append the stored procedure execution
				streamWriter.WriteLine("\t\t\tDatabase myDatabase = DatabaseFactory.CreateDatabase();");
				streamWriter.WriteLine("\t\t\tDBCommandWrapper myCommand = myDatabase.GetStoredProcCommandWrapper(\"" + storedProcedurePrefix + table.Name + "Update\");\n");

				// Append the parameters
				StringBuilder builder = new StringBuilder();
				foreach (Column column in table.Columns) {
					builder.Append("\t\t\t" + Utility.CreateSqlParameter(column) + ";\n");
				}
				streamWriter.WriteLine(builder.ToString());
				streamWriter.WriteLine("\t\t\tmyDatabase.ExecuteNonQuery(myCommand);");

				// Append the method footer
				streamWriter.WriteLine("\t\t}");
			}
		}

		/// <summary>
		/// Creates a string that represents the delete functionality of the data access class.
		/// </summary>
		/// <param name="table">The Table instance that this method will be created for.</param>
		/// <param name="storedProcedurePrefix">The prefix that is used on the stored procedure that this method will call.</param>
		/// <param name="streamWriter">The StreamWriter instance that will be used to create the method.</param>
		private static void CreateDeleteMethod(Table table, string storedProcedurePrefix, StreamWriter streamWriter)
		{
			if (table.PrimaryKeys.Count > 0) {
				// Create the delete function based on keys
				// Append the method header
				streamWriter.WriteLine();
				streamWriter.WriteLine("\t\t/// <summary>");
				streamWriter.WriteLine("\t\t/// Deletes a record from the " + table.Name + " table by a composite primary key.");
				streamWriter.WriteLine("\t\t/// </summary>");

				streamWriter.Write("\t\tpublic static void Delete(");
				int remainingColumns = table.PrimaryKeys.Count;
				foreach (Column column in table.PrimaryKeys) {
					remainingColumns--;

					if (remainingColumns > 0) {
						streamWriter.Write(Utility.CreateMethodParameter(column) + ", ");
					} else {
						streamWriter.Write(Utility.CreateMethodParameter(column));
					}
				}
				streamWriter.WriteLine(")\n\t\t{");

				// Append the stored procedure execution
				streamWriter.WriteLine("\t\t\tDatabase myDatabase = DatabaseFactory.CreateDatabase();");
				streamWriter.WriteLine("\t\t\tDBCommandWrapper myCommand = myDatabase.GetStoredProcCommandWrapper(\"" + storedProcedurePrefix + table.Name + "Delete\");\n");

				// Append the parameters
				StringBuilder builder = new StringBuilder();
				foreach (Column column in table.PrimaryKeys) {
					builder.Append("\t\t\t" + Utility.CreateSqlParameter(column) + ";\n");
				}
				streamWriter.WriteLine(builder.ToString());
				streamWriter.WriteLine("\t\t\tmyDatabase.ExecuteNonQuery(myCommand);");

				// Append the method footer
				streamWriter.WriteLine("\t\t}");
			}
		}

		/// <summary>
		/// Creates a string that represents the "delete by" functionality of the data access class.
		/// </summary>
		/// <param name="table">The Table instance that this method will be created for.</param>
		/// <param name="storedProcedurePrefix">The prefix that is used on the stored procedure that this method will call.</param>
		/// <param name="streamWriter">The StreamWriter instance that will be used to create the method.</param>
		private static void CreateDeleteByMethods(Table table, string storedProcedurePrefix, StreamWriter streamWriter)
		{
			// Create a stored procedure for each foreign key
			foreach (ArrayList compositeKeyList in table.ForeignKeys.Values) {
				// Create the stored procedure name
				StringBuilder stringBuilder = new StringBuilder(255);
				stringBuilder.Append(storedProcedurePrefix + table.Name + "DeleteBy");
				for (int i = 0; i < compositeKeyList.Count; i++) {
					Column column = (Column) compositeKeyList[i];

					if (i > 0) {
						stringBuilder.Append("_" + Utility.FormatPascal(column.Name));
					} else {
						stringBuilder.Append(Utility.FormatPascal(column.Name));
					}
				}
				string procedureName = stringBuilder.ToString();

				// Create the method name
				stringBuilder = new StringBuilder(255);
				stringBuilder.Append("DeleteBy");
				for (int i = 0; i < compositeKeyList.Count; i++) {
					Column column = (Column) compositeKeyList[i];

					if (i > 0) {
						stringBuilder.Append("_" + Utility.FormatPascal(column.ProgrammaticAlias));
					} else {
						stringBuilder.Append(Utility.FormatPascal(column.ProgrammaticAlias));
					}
				}
				string methodName = stringBuilder.ToString();

				// Create the delete function based on keys
				// Append the method header
				streamWriter.WriteLine();
				streamWriter.WriteLine("\t\t/// <summary>");
				streamWriter.WriteLine("\t\t/// Deletes a record from the " + table.Name + " table by a foreign key.");
				streamWriter.WriteLine("\t\t/// </summary>");

				streamWriter.Write("\t\tpublic static void " + methodName + "(");
				int remainingColumns = compositeKeyList.Count;
				foreach (Column column in compositeKeyList) {
					remainingColumns--;

					if (remainingColumns > 0) {
						streamWriter.Write(Utility.CreateMethodParameter(column) + ", ");
					} else {
						streamWriter.Write(Utility.CreateMethodParameter(column));
					}
				}
				streamWriter.WriteLine(")\n\t\t{");

				// Append the stored procedure execution
				streamWriter.WriteLine("\t\t\tDatabase myDatabase = DatabaseFactory.CreateDatabase();");
				streamWriter.WriteLine("\t\t\tDBCommandWrapper myCommand = myDatabase.GetStoredProcCommandWrapper(\"" + procedureName + "\");\n");

				// Append the parameters
				StringBuilder builder = new StringBuilder();
				foreach (Column column in compositeKeyList) {
					builder.Append("\t\t\t" + Utility.CreateSqlParameter(column) + ";\n");
				}
				streamWriter.WriteLine(builder.ToString());
				streamWriter.WriteLine("\t\t\tmyDatabase.ExecuteNonQuery(myCommand);");

				// Append the method footer
				streamWriter.WriteLine("\t\t}");
			}
		}

		/// <summary>
		/// Creates a string that represents the select by primary key functionality of the data access class.
		/// </summary>
		/// <param name="table">The Table instance that this method will be created for.</param>
		/// <param name="storedProcedurePrefix">The prefix that is used on the stored procedure that this method will call.</param>
		/// <param name="streamWriter">The StreamWriter instance that will be used to create the method.</param>
		private static void CreateSelectMethod(Table table, string storedProcedurePrefix, StreamWriter streamWriter)
		{
			if (table.PrimaryKeys.Count > 0 && table.Columns.Count != table.ForeignKeys.Count) {
				// Append the method header
				streamWriter.WriteLine();
				streamWriter.WriteLine("\t\t/// <summary>");
				streamWriter.WriteLine("\t\t/// Selects a single record from the " + table.Name + " table.");
				streamWriter.WriteLine("\t\t/// </summary>");

				streamWriter.Write("\t\tpublic static IDataReader Select(");
				int remainingColumns = table.PrimaryKeys.Count;
				foreach (Column column in table.PrimaryKeys) {
					remainingColumns--;

					if (remainingColumns > 0) {
						streamWriter.Write(Utility.CreateMethodParameter(column) + ", ");
					} else {
						streamWriter.Write(Utility.CreateMethodParameter(column));
					}
				}
				streamWriter.WriteLine(")\n\t\t{");

				// Append the stored procedure execution
				streamWriter.WriteLine("\t\t\tDatabase myDatabase = DatabaseFactory.CreateDatabase();");
				streamWriter.WriteLine("\t\t\tDBCommandWrapper myCommand = myDatabase.GetStoredProcCommandWrapper(\"" + storedProcedurePrefix + table.Name + "Select\");\n");

				// Append the parameters
				StringBuilder builder = new StringBuilder();
				foreach (Column column in table.PrimaryKeys) {
					builder.Append("\t\t\t" + Utility.CreateSqlParameter(column) + ";\n");
				}
				streamWriter.WriteLine(builder.ToString());
				streamWriter.WriteLine("\t\t\treturn myDatabase.ExecuteReader(myCommand);");

				// Append the method footer
				streamWriter.WriteLine("\t\t}");
			}
		}

		/// <summary>
		/// Creates a string that represents the select functionality of the data access class.
		/// </summary>
		/// <param name="table">The Table instance that this method will be created for.</param>
		/// <param name="storedProcedurePrefix">The prefix that is used on the stored procedure that this method will call.</param>
		/// <param name="streamWriter">The StreamWriter instance that will be used to create the method.</param>
		private static void CreateSelectAllMethod(Table table, string storedProcedurePrefix, StreamWriter streamWriter)
		{
			if (table.Columns.Count != table.PrimaryKeys.Count && table.Columns.Count != table.ForeignKeys.Count) {
				// Append the method header
				streamWriter.WriteLine();
				streamWriter.WriteLine("\t\t/// <summary>");
				streamWriter.WriteLine("\t\t/// Selects all records from the " + table.Name + " table.");
				streamWriter.WriteLine("\t\t/// </summary>");
				streamWriter.WriteLine("\t\tpublic static IDataReader SelectAll()\n\t\t{");

				// Append the stored procedure execution
				streamWriter.WriteLine("\t\t\tDatabase myDatabase = DatabaseFactory.CreateDatabase();");
				streamWriter.WriteLine("\t\t\tDBCommandWrapper myCommand = myDatabase.GetStoredProcCommandWrapper(\"" + storedProcedurePrefix + table.Name + "SelectAll\");\n");
				streamWriter.WriteLine("\t\t\treturn myDatabase.ExecuteReader(myCommand);");

				// Append the method footer
				streamWriter.WriteLine("\t\t}");
			}
		}

		/// <summary>
		/// Creates a string that represents the "select by" functionality of the data access class.
		/// </summary>
		/// <param name="table">The Table instance that this method will be created for.</param>
		/// <param name="storedProcedurePrefix">The prefix that is used on the stored procedure that this method will call.</param>
		/// <param name="streamWriter">The StreamWriter instance that will be used to create the method.</param>
		private static void CreateSelectByMethods(Table table, string storedProcedurePrefix, StreamWriter streamWriter)
		{
			// Create a stored procedure for each foreign key
			foreach (ArrayList compositeKeyList in table.ForeignKeys.Values) {
				// Create the stored procedure name
				StringBuilder stringBuilder = new StringBuilder(255);
				stringBuilder.Append(storedProcedurePrefix + table.Name + "SelectBy");
				for (int i = 0; i < compositeKeyList.Count; i++) {
					Column column = (Column) compositeKeyList[i];

					if (i > 0) {
						stringBuilder.Append("_" + Utility.FormatPascal(column.Name));
					} else {
						stringBuilder.Append(Utility.FormatPascal(column.Name));
					}
				}
				string procedureName = stringBuilder.ToString();

				// Create the method name
				stringBuilder = new StringBuilder(255);
				stringBuilder.Append("SelectBy");
				for (int i = 0; i < compositeKeyList.Count; i++) {
					Column column = (Column) compositeKeyList[i];

					if (i > 0) {
						stringBuilder.Append("_" + Utility.FormatPascal(column.ProgrammaticAlias));
					} else {
						stringBuilder.Append(Utility.FormatPascal(column.ProgrammaticAlias));
					}
				}
				string methodName = stringBuilder.ToString();

				// Create the select function based on keys
				// Append the method header
				streamWriter.WriteLine();
				streamWriter.WriteLine("\t\t/// <summary>");
				streamWriter.WriteLine("\t\t/// Selects all records from the " + table.Name + " table by a foreign key.");
				streamWriter.WriteLine("\t\t/// </summary>");

				streamWriter.Write("\t\tpublic static IDataReader " + methodName + "(");
				for (int i = 0; i < compositeKeyList.Count; i++) {
					Column column = (Column) compositeKeyList[i];

					if (i < (compositeKeyList.Count - 1)) {
						streamWriter.Write(Utility.CreateMethodParameter(column) + ", ");
					} else {
						streamWriter.Write(Utility.CreateMethodParameter(column));
					}
				}
				streamWriter.WriteLine(")\n\t\t{");

				// Append the stored procedure execution
				streamWriter.WriteLine("\t\t\tDatabase myDatabase = DatabaseFactory.CreateDatabase();");
				streamWriter.WriteLine("\t\t\tDBCommandWrapper myCommand = myDatabase.GetStoredProcCommandWrapper(\"" + procedureName + "\");\n");

				// Append the parameters
				StringBuilder builder = new StringBuilder();
				foreach (Column column in compositeKeyList) {
					builder.Append("\t\t\t" + Utility.CreateSqlParameter(column) + ";\n");
				}
				streamWriter.WriteLine(builder.ToString());
				streamWriter.WriteLine("\t\t\treturn myDatabase.ExecuteReader(myCommand);");

				// Append the method footer
				streamWriter.WriteLine("\t\t}");
			}
		}
	}
}