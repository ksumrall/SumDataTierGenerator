using System.Collections;
using System.IO;
using System.Text;

namespace PriorityIt.DataTierGenerator
{
	internal sealed class SqlGenerator
	{
		private SqlGenerator() {}

		/// <summary>
		/// Creates the SQL script that is responsible for granting the specified login access to the specified database.
		/// </summary>
		/// <param name="databaseName">The name of the database that the login will be created for.</param>
		/// <param name="grantLoginName">Name of the SQL Server user that should have execute rights on the stored procedure.</param>
		/// <param name="path">Path where the script should be created.</param>
		/// <param name="createMultipleFiles">Indicates the script should be created in its own file.</param>
		internal static void CreateUserQueries(string databaseName, string grantLoginName, string path, bool createMultipleFiles)
		{
			if (grantLoginName.Length > 0) {
				string fileName;

				// Determine the file name to be used
				if (createMultipleFiles) {
					fileName = path + "GrantUserPermissions.sql";
				} else {
					fileName = path + "StoredProcedures.sql";
				}

				using (StreamWriter writer = new StreamWriter(fileName, true)) {
					writer.Write(Utility.GetUserQueries(databaseName, grantLoginName));
				}
			}
		}

		/// <summary>
		/// Creates an insert stored procedure SQL script for the specified table
		/// </summary>
		/// <param name="table">Instance of the Table class that represents the table this stored procedure will be created for.</param>
		/// <param name="grantLoginName">Name of the SQL Server user that should have execute rights on the stored procedure.</param>
		/// <param name="storedProcedurePrefix">Prefix to be appended to the name of the stored procedure.</param>
		/// <param name="path">Path where the stored procedure script should be created.</param>
		/// <param name="createMultipleFiles">Indicates the procedure(s) generated should be created in its own file.</param>
		internal static void CreateInsertStoredProcedure(Table table, string grantLoginName, string storedProcedurePrefix, string path, bool createMultipleFiles)
		{
			// Create the stored procedure name
			string procedureName = storedProcedurePrefix + table.Name + "Insert";
			string fileName;

			// Determine the file name to be used
			if (createMultipleFiles) {
				fileName = path + procedureName + ".sql";
			} else {
				fileName = path + "StoredProcedures.sql";
			}

			using (StreamWriter writer = new StreamWriter(fileName, true)) {
				// Create the seperator
				if (createMultipleFiles == false) {
					writer.WriteLine();
					writer.WriteLine("/*******************************************************************************");
					writer.WriteLine("*******************************************************************************/");
				}

				// Create the drop statment
				writer.WriteLine("if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" + procedureName + "]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)");
				writer.WriteLine("\tdrop procedure [dbo].[" + procedureName + "]");
				writer.WriteLine("GO");
				writer.WriteLine();

				// Create the SQL for the stored procedure
				writer.WriteLine("CREATE PROCEDURE [dbo].[" + procedureName + "] (");

				// Create the parameter list
				for (int i = 0; i < table.Columns.Count; i++) {
					Column column = (Column) table.Columns[i];
					if (column.IsRowGuidCol == false && column.IsIdentity == false) {
						if (i < (table.Columns.Count - 1)) {
							writer.WriteLine("\t" + Utility.CreateParameterString(column) + ",");
						} else {
							writer.WriteLine("\t" + Utility.CreateParameterString(column));
						}
					}
				}
				writer.WriteLine(")");

				writer.WriteLine();
				writer.WriteLine("AS");
				writer.WriteLine();
				writer.WriteLine("SET NOCOUNT ON");
				writer.WriteLine();

				// Initialize all RowGuidCol columns
				foreach (Column column in table.Columns) {
					if (column.IsRowGuidCol) {
						writer.WriteLine("DECLARE @" + column.ProgrammaticAlias + " uniqueidentifier");
						writer.WriteLine("SET @" + column.ProgrammaticAlias + " = NEWID()");
						writer.WriteLine();
						break;
					}
				}

				writer.WriteLine("INSERT INTO [" + table.Name + "] (");

				// Create the parameter list
				for (int i = 0; i < table.Columns.Count; i++) {
					Column column = (Column) table.Columns[i];

					// Ignore any identity columns
					if (column.IsIdentity == false) {
						// Append the column name as a parameter of the insert statement
						if (i < (table.Columns.Count - 1)) {
							writer.WriteLine("\t[" + column.Name + "],");
						} else {
							writer.WriteLine("\t[" + column.Name + "]");
						}
					}
				}

				writer.WriteLine(") VALUES (");

				// Create the values list
				for (int i = 0; i < table.Columns.Count; i++) {
					Column column = (Column) table.Columns[i];

					// Is the current column an identity column?
					if (column.IsIdentity == false) {
						// Append the necessary line breaks and commas
						if (i < (table.Columns.Count - 1)) {
							writer.WriteLine("\t@" + column.ProgrammaticAlias + ",");
						} else {
							writer.WriteLine("\t@" + column.ProgrammaticAlias);
						}
					}
				}

				writer.WriteLine(")");

				// Should we include a line for returning the identity?
				foreach (Column column in table.Columns) {
					// Is the current column an identity column?
					if (column.IsIdentity) {
						writer.WriteLine();
						writer.WriteLine("SELECT SCOPE_IDENTITY() AS " + column.Name);
						break;
					} else if (column.IsRowGuidCol) {
						writer.WriteLine();
						writer.WriteLine("SELECT @" + column.Name + " AS " + column.Name);
						break;
					}
				}

				writer.WriteLine("GO");

				// Create the grant statement, if a user was specified
				if (grantLoginName.Length > 0) {
					writer.WriteLine();
					writer.WriteLine("GRANT EXECUTE ON [dbo].[" + procedureName + "] TO [" + grantLoginName + "]");
					writer.WriteLine("GO");
				}
			}
		}

		/// <summary>
		/// Creates an update stored procedure SQL script for the specified table
		/// </summary>
		/// <param name="table">Instance of the Table class that represents the table this stored procedure will be created for.</param>
		/// <param name="grantLoginName">Name of the SQL Server user that should have execute rights on the stored procedure.</param>
		/// <param name="storedProcedurePrefix">Prefix to be appended to the name of the stored procedure.</param>
		/// <param name="path">Path where the stored procedure script should be created.</param>
		/// <param name="createMultipleFiles">Indicates the procedure(s) generated should be created in its own file.</param>
		internal static void CreateUpdateStoredProcedure(Table table, string grantLoginName, string storedProcedurePrefix, string path, bool createMultipleFiles)
		{
			if (table.PrimaryKeys.Count > 0 && table.Columns.Count != table.PrimaryKeys.Count && table.Columns.Count != table.ForeignKeys.Count) {
				// Create the stored procedure name
				string procedureName = storedProcedurePrefix + table.Name + "Update";
				string fileName;

				// Determine the file name to be used
				if (createMultipleFiles) {
					fileName = path + procedureName + ".sql";
				} else {
					fileName = path + "StoredProcedures.sql";
				}

				using (StreamWriter writer = new StreamWriter(fileName, true)) {
					// Create the seperator
					if (createMultipleFiles == false) {
						writer.WriteLine();
						writer.WriteLine("/*******************************************************************************");
						writer.WriteLine("*******************************************************************************/");
					}

					// Create the drop statment
					writer.WriteLine("if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" + procedureName + "]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)");
					writer.WriteLine("\tdrop procedure [dbo].[" + procedureName + "]");
					writer.WriteLine("GO");
					writer.WriteLine();

					// Create the SQL for the stored procedure
					writer.WriteLine("CREATE PROCEDURE [dbo].[" + procedureName + "] (");

					// Create the parameter list
					for (int i = 0; i < table.Columns.Count; i++) {
						Column column = (Column) table.Columns[i];

						if (i < (table.Columns.Count - 1)) {
							writer.WriteLine("\t" + Utility.CreateParameterString(column) + ",");
						} else {
							writer.WriteLine("\t" + Utility.CreateParameterString(column));
						}
					}
					writer.WriteLine(")");

					writer.WriteLine();
					writer.WriteLine("AS");
					writer.WriteLine();
					writer.WriteLine("SET NOCOUNT ON");
					writer.WriteLine();
					writer.WriteLine("UPDATE");
					writer.WriteLine("\t[" + table.Name + "]");
					writer.WriteLine("SET");

					// Create the set statement
					for (int i = 0; i < table.Columns.Count; i++) {
						Column column = (Column) table.Columns[i];

						// Ignore Identity and RowGuidCol columns
						if (table.PrimaryKeys.Contains(column) == false) {
							if (i < (table.Columns.Count - 1)) {
								writer.WriteLine("\t[" + column.Name + "] = @" + column.ProgrammaticAlias + ",");
							} else {
								writer.WriteLine("\t[" + column.Name + "] = @" + column.ProgrammaticAlias);
							}
						}
					}

					writer.WriteLine("WHERE");

					// Create the where clause
					for (int i = 0; i < table.PrimaryKeys.Count; i++) {
						Column column = (Column) table.PrimaryKeys[i];

						if (i > 0) {
							writer.Write("\tAND [" + column.Name + "] = @" + column.ProgrammaticAlias);
						} else {
							writer.Write("\t [" + column.Name + "] = @" + column.ProgrammaticAlias);
						}
					}
					writer.WriteLine();

					writer.WriteLine("GO");

					// Create the grant statement, if a user was specified
					if (grantLoginName.Length > 0) {
						writer.WriteLine();
						writer.WriteLine("GRANT EXECUTE ON [dbo].[" + procedureName + "] TO [" + grantLoginName + "]");
						writer.WriteLine("GO");
					}
				}
			}
		}

		/// <summary>
		/// Creates an delete stored procedure SQL script for the specified table
		/// </summary>
		/// <param name="table">Instance of the Table class that represents the table this stored procedure will be created for.</param>
		/// <param name="grantLoginName">Name of the SQL Server user that should have execute rights on the stored procedure.</param>
		/// <param name="storedProcedurePrefix">Prefix to be appended to the name of the stored procedure.</param>
		/// <param name="path">Path where the stored procedure script should be created.</param>
		/// <param name="createMultipleFiles">Indicates the procedure(s) generated should be created in its own file.</param>
		internal static void CreateDeleteStoredProcedure(Table table, string grantLoginName, string storedProcedurePrefix, string path, bool createMultipleFiles)
		{
			if (table.PrimaryKeys.Count > 0) {
				// Create the stored procedure name
				string procedureName = storedProcedurePrefix + table.Name + "Delete";
				string fileName;

				// Determine the file name to be used
				if (createMultipleFiles) {
					fileName = path + procedureName + ".sql";
				} else {
					fileName = path + "StoredProcedures.sql";
				}

				using (StreamWriter writer = new StreamWriter(fileName, true)) {
					// Create the seperator
					if (createMultipleFiles == false) {
						writer.WriteLine();
						writer.WriteLine("/*******************************************************************************");
						writer.WriteLine("*******************************************************************************/");
					}

					// Create the drop statment
					writer.WriteLine("if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" + procedureName + "]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)");
					writer.WriteLine("\tdrop procedure [dbo].[" + procedureName + "]");
					writer.WriteLine("GO");
					writer.WriteLine();

					// Create the SQL for the stored procedure
					writer.WriteLine("CREATE PROCEDURE [dbo].[" + procedureName + "] (");

					// Create the parameter list
					for (int i = 0; i < table.PrimaryKeys.Count; i++) {
						Column column = (Column) table.PrimaryKeys[i];

						if (i < (table.PrimaryKeys.Count - 1)) {
							writer.WriteLine("\t" + Utility.CreateParameterString(column) + ",");
						} else {
							writer.WriteLine("\t" + Utility.CreateParameterString(column));
						}
					}
					writer.WriteLine(")");

					writer.WriteLine();
					writer.WriteLine("AS");
					writer.WriteLine();
					writer.WriteLine("SET NOCOUNT ON");
					writer.WriteLine();
					writer.WriteLine("DELETE FROM");
					writer.WriteLine("\t[" + table.Name + "]");
					writer.WriteLine("WHERE");

					// Create the where clause
					for (int i = 0; i < table.PrimaryKeys.Count; i++) {
						Column column = (Column) table.PrimaryKeys[i];

						if (i > 0) {
							writer.WriteLine("\tAND [" + column.Name + "] = @" + column.ProgrammaticAlias);
						} else {
							writer.WriteLine("\t[" + column.Name + "] = @" + column.ProgrammaticAlias);
						}
					}

					writer.WriteLine("GO");

					// Create the grant statement, if a user was specified
					if (grantLoginName.Length > 0) {
						writer.WriteLine();
						writer.WriteLine("GRANT EXECUTE ON [dbo].[" + procedureName + "] TO [" + grantLoginName + "]");
						writer.WriteLine("GO");
					}
				}
			}
		}

		/// <summary>
		/// Creates one or more delete stored procedures SQL script for the specified table and its foreign keys
		/// </summary>
		/// <param name="table">Instance of the Table class that represents the table this stored procedure will be created for.</param>
		/// <param name="grantLoginName">Name of the SQL Server user that should have execute rights on the stored procedure.</param>
		/// <param name="storedProcedurePrefix">Prefix to be appended to the name of the stored procedure.</param>
		/// <param name="path">Path where the stored procedure script should be created.</param>
		/// <param name="createMultipleFiles">Indicates the procedure(s) generated should be created in its own file.</param>
		internal static void CreateDeleteByStoredProcedures(Table table, string grantLoginName, string storedProcedurePrefix, string path, bool createMultipleFiles)
		{
			// Create a stored procedure for each foreign key
			foreach (ArrayList compositeKeyList in table.ForeignKeys.Values) {
				// Create the stored procedure name
				StringBuilder stringBuilder = new StringBuilder(255);
				stringBuilder.Append(storedProcedurePrefix + table.Name + "DeleteBy");

				// Create the parameter list
				for (int i = 0; i < compositeKeyList.Count; i++) {
					Column column = (Column) compositeKeyList[i];
					if (i > 0) {
						stringBuilder.Append("_" + Utility.FormatPascal(column.ProgrammaticAlias));
					} else {
						stringBuilder.Append(Utility.FormatPascal(column.ProgrammaticAlias));
					}
				}

				string procedureName = stringBuilder.ToString();
				string fileName;

				// Determine the file name to be used
				if (createMultipleFiles) {
					fileName = path + procedureName + ".sql";
				} else {
					fileName = path + "StoredProcedures.sql";
				}

				using (StreamWriter writer = new StreamWriter(fileName, true)) {
					// Create the seperator
					if (createMultipleFiles == false) {
						writer.WriteLine();
						writer.WriteLine("/*******************************************************************************");
						writer.WriteLine("*******************************************************************************/");
					}

					// Create the drop statment
					writer.WriteLine("if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" + procedureName + "]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)");
					writer.WriteLine("\tdrop procedure [dbo].[" + procedureName + "]");
					writer.WriteLine("GO");
					writer.WriteLine();

					// Create the SQL for the stored procedure
					writer.WriteLine("CREATE PROCEDURE [dbo].[" + procedureName + "] (");

					// Create the parameter list
					for (int i = 0; i < compositeKeyList.Count; i++) {
						Column column = (Column) compositeKeyList[i];

						if (i < (compositeKeyList.Count - 1)) {
							writer.WriteLine("\t" + Utility.CreateParameterString(column) + ",");
						} else {
							writer.WriteLine("\t" + Utility.CreateParameterString(column));
						}
					}
					writer.WriteLine(")");

					writer.WriteLine();
					writer.WriteLine("AS");
					writer.WriteLine();
					writer.WriteLine("SET NOCOUNT ON");
					writer.WriteLine();
					writer.WriteLine("DELETE FROM");
					writer.WriteLine("\t[" + table.Name + "]");
					writer.WriteLine("WHERE");

					// Create the where clause
					for (int i = 0; i < compositeKeyList.Count; i++) {
						Column column = (Column) compositeKeyList[i];

						if (i > 0) {
							writer.WriteLine("\tAND [" + column.Name + "] = @" + column.ProgrammaticAlias);
						} else {
							writer.WriteLine("\t[" + column.Name + "] = @" + column.ProgrammaticAlias);
						}
					}

					writer.WriteLine("GO");

					// Create the grant statement, if a user was specified
					if (grantLoginName.Length > 0) {
						writer.WriteLine();
						writer.WriteLine("GRANT EXECUTE ON [dbo].[" + procedureName + "] TO [" + grantLoginName + "]");
						writer.WriteLine("GO");
					}
				}
			}
		}

		/// <summary>
		/// Creates an select stored procedure SQL script for the specified table
		/// </summary>
		/// <param name="table">Instance of the Table class that represents the table this stored procedure will be created for.</param>
		/// <param name="grantLoginName">Name of the SQL Server user that should have execute rights on the stored procedure.</param>
		/// <param name="storedProcedurePrefix">Prefix to be appended to the name of the stored procedure.</param>
		/// <param name="path">Path where the stored procedure script should be created.</param>
		/// <param name="createMultipleFiles">Indicates the procedure(s) generated should be created in its own file.</param>
		internal static void CreateSelectStoredProcedure(Table table, string grantLoginName, string storedProcedurePrefix, string path, bool createMultipleFiles)
		{
			if (table.PrimaryKeys.Count > 0 && table.ForeignKeys.Count != table.Columns.Count) {
				// Create the stored procedure name
				string procedureName = storedProcedurePrefix + table.Name + "Select";
				string fileName;

				// Determine the file name to be used
				if (createMultipleFiles) {
					fileName = path + procedureName + ".sql";
				} else {
					fileName = path + "StoredProcedures.sql";
				}

				using (StreamWriter writer = new StreamWriter(fileName, true)) {
					// Create the seperator
					if (createMultipleFiles == false) {
						writer.WriteLine();
						writer.WriteLine("/*******************************************************************************");
						writer.WriteLine("*******************************************************************************/");
					}

					// Create the drop statment
					writer.WriteLine("if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" + procedureName + "]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)");
					writer.WriteLine("\tdrop procedure [dbo].[" + procedureName + "]");
					writer.WriteLine("GO");
					writer.WriteLine();

					// Create the SQL for the stored procedure
					writer.WriteLine("CREATE PROCEDURE [dbo].[" + procedureName + "] (");

					// Create the parameter list
					for (int i = 0; i < table.PrimaryKeys.Count; i++) {
						Column column = (Column) table.PrimaryKeys[i];

						if (i == (table.PrimaryKeys.Count - 1)) {
							writer.WriteLine("\t" + Utility.CreateParameterString(column));
						} else {
							writer.WriteLine("\t" + Utility.CreateParameterString(column) + ",");
						}
					}

					writer.WriteLine(")");

					writer.WriteLine();
					writer.WriteLine("AS");
					writer.WriteLine();
					writer.WriteLine("SET NOCOUNT ON");
					writer.WriteLine();
					writer.WriteLine("SELECT");

					// Create the list of columns
					for (int i = 0; i < table.Columns.Count; i++) {
						Column column = (Column) table.Columns[i];

						if (i < (table.Columns.Count - 1)) {
							writer.WriteLine("\t[" + column.Name + "],");
						} else {
							writer.WriteLine("\t[" + column.Name + "]");
						}
					}

					writer.WriteLine("FROM");
					writer.WriteLine("\t[" + table.Name + "]");
					writer.WriteLine("WHERE");

					// Create the where clause
					for (int i = 0; i < table.PrimaryKeys.Count; i++) {
						Column column = (Column) table.PrimaryKeys[i];

						if (i > 0) {
							writer.WriteLine("\tAND [" + column.Name + "] = @" + column.ProgrammaticAlias);
						} else {
							writer.WriteLine("\t[" + column.Name + "] = @" + column.ProgrammaticAlias);
						}
					}

					writer.WriteLine("GO");

					// Create the grant statement, if a user was specified
					if (grantLoginName.Length > 0) {
						writer.WriteLine();
						writer.WriteLine("GRANT EXECUTE ON [dbo].[" + procedureName + "] TO [" + grantLoginName + "]");
						writer.WriteLine("GO");
					}
				}
			}
		}

		/// <summary>
		/// Creates an select all stored procedure SQL script for the specified table
		/// </summary>
		/// <param name="table">Instance of the Table class that represents the table this stored procedure will be created for.</param>
		/// <param name="grantLoginName">Name of the SQL Server user that should have execute rights on the stored procedure.</param>
		/// <param name="storedProcedurePrefix">Prefix to be appended to the name of the stored procedure.</param>
		/// <param name="path">Path where the stored procedure script should be created.</param>
		/// <param name="createMultipleFiles">Indicates the procedure(s) generated should be created in its own file.</param>
		internal static void CreateSelectAllStoredProcedure(Table table, string grantLoginName, string storedProcedurePrefix, string path, bool createMultipleFiles)
		{
			if (table.PrimaryKeys.Count > 0 && table.ForeignKeys.Count != table.Columns.Count) {
				// Create the stored procedure name
				string procedureName = storedProcedurePrefix + table.Name + "SelectAll";
				string fileName;

				// Determine the file name to be used
				if (createMultipleFiles) {
					fileName = path + procedureName + ".sql";
				} else {
					fileName = path + "StoredProcedures.sql";
				}

				using (StreamWriter writer = new StreamWriter(fileName, true)) {
					// Create the seperator
					if (createMultipleFiles == false) {
						writer.WriteLine();
						writer.WriteLine("/*******************************************************************************");
						writer.WriteLine("*******************************************************************************/");
					}

					// Create the drop statment
					writer.WriteLine("if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" + procedureName + "]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)");
					writer.WriteLine("\tdrop procedure [dbo].[" + procedureName + "]");
					writer.WriteLine("GO");
					writer.WriteLine();

					// Create the SQL for the stored procedure
					writer.WriteLine("CREATE PROCEDURE [dbo].[" + procedureName + "]");
					writer.WriteLine();
					writer.WriteLine("AS");
					writer.WriteLine();
					writer.WriteLine("SET NOCOUNT ON");
					writer.WriteLine();
					writer.WriteLine("SELECT");

					// Create the list of columns
					for (int i = 0; i < table.Columns.Count; i++) {
						Column column = (Column) table.Columns[i];

						if (i < (table.Columns.Count - 1)) {
							writer.WriteLine("\t[" + column.Name + "],");
						} else {
							writer.WriteLine("\t[" + column.Name + "]");
						}
					}

					writer.WriteLine("FROM");
					writer.WriteLine("\t[" + table.Name + "]");

					writer.WriteLine("GO");

					// Create the grant statement, if a user was specified
					if (grantLoginName.Length > 0) {
						writer.WriteLine();
						writer.WriteLine("GRANT EXECUTE ON [dbo].[" + procedureName + "] TO [" + grantLoginName + "]");
						writer.WriteLine("GO");
					}
				}
			}
		}

		/// <summary>
		/// Creates one or more select stored procedures SQL script for the specified table and its foreign keys
		/// </summary>
		/// <param name="table">Instance of the Table class that represents the table this stored procedure will be created for.</param>
		/// <param name="grantLoginName">Name of the SQL Server user that should have execute rights on the stored procedure.</param>
		/// <param name="storedProcedurePrefix">Prefix to be appended to the name of the stored procedure.</param>
		/// <param name="path">Path where the stored procedure script should be created.</param>
		/// <param name="createMultipleFiles">Indicates the procedure(s) generated should be created in its own file.</param>
		internal static void CreateSelectByStoredProcedures(Table table, string grantLoginName, string storedProcedurePrefix, string path, bool createMultipleFiles)
		{
			// Create a stored procedure for each foreign key
			foreach (ArrayList compositeKeyList in table.ForeignKeys.Values) {
				// Create the stored procedure name
				StringBuilder stringBuilder = new StringBuilder(255);
				stringBuilder.Append(storedProcedurePrefix + table.Name + "SelectBy");

				// Create the parameter list
				for (int i = 0; i < compositeKeyList.Count; i++) {
					Column column = (Column) compositeKeyList[i];
					if (i > 0) {
						stringBuilder.Append("_" + Utility.FormatPascal(column.ProgrammaticAlias));
					} else {
						stringBuilder.Append(Utility.FormatPascal(column.ProgrammaticAlias));
					}
				}

				string procedureName = stringBuilder.ToString();
				string fileName;

				// Determine the file name to be used
				if (createMultipleFiles) {
					fileName = path + procedureName + ".sql";
				} else {
					fileName = path + "StoredProcedures.sql";
				}

				using (StreamWriter writer = new StreamWriter(fileName, true)) {
					// Create the seperator
					if (createMultipleFiles == false) {
						writer.WriteLine();
						writer.WriteLine("/*******************************************************************************");
						writer.WriteLine("*******************************************************************************/");
					}

					// Create the drop statment
					writer.WriteLine("if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" + procedureName + "]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)");
					writer.WriteLine("\tdrop procedure [dbo].[" + procedureName + "]");
					writer.WriteLine("GO");
					writer.WriteLine();

					// Create the SQL for the stored procedure
					writer.WriteLine("CREATE PROCEDURE [dbo].[" + procedureName + "] (");

					// Create the parameter list
					for (int i = 0; i < compositeKeyList.Count; i++) {
						Column column = (Column) compositeKeyList[i];

						if (i < (compositeKeyList.Count - 1)) {
							writer.WriteLine("\t" + Utility.CreateParameterString(column) + ",");
						} else {
							writer.WriteLine("\t" + Utility.CreateParameterString(column));
						}
					}
					writer.WriteLine(")");

					writer.WriteLine();
					writer.WriteLine("AS");
					writer.WriteLine();
					writer.WriteLine("SET NOCOUNT ON");
					writer.WriteLine();
					writer.WriteLine("SELECT");

					// Create the list of columns
					for (int i = 0; i < table.Columns.Count; i++) {
						Column column = (Column) table.Columns[i];

						if (i < (table.Columns.Count - 1)) {
							writer.WriteLine("\t[" + column.Name + "],");
						} else {
							writer.WriteLine("\t[" + column.Name + "]");
						}
					}

					writer.WriteLine("FROM");
					writer.WriteLine("\t[" + table.Name + "]");
					writer.WriteLine("WHERE");

					// Create the where clause
					for (int i = 0; i < compositeKeyList.Count; i++) {
						Column column = (Column) compositeKeyList[i];

						if (i > 0) {
							writer.WriteLine("\tAND [" + column.Name + "] = @" + column.ProgrammaticAlias);
						} else {
							writer.WriteLine("\t[" + column.Name + "] = @" + column.ProgrammaticAlias);
						}
					}

					writer.WriteLine("GO");

					// Create the grant statement, if a user was specified
					if (grantLoginName.Length > 0) {
						writer.WriteLine();
						writer.WriteLine("GRANT EXECUTE ON [dbo].[" + procedureName + "] TO [" + grantLoginName + "]");
						writer.WriteLine("GO");
					}
				}
			}
		}
	}
}