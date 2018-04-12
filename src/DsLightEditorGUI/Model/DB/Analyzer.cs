/*
 * DsLight
 * 
 * Copyright (c) 2014..2018 by Simon Baer
 * 
 * This program is free software; you can redistribute it and/or modify it under the terms
 * of the GNU General Public License as published by the Free Software Foundation; either
 * version 3 of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License along with this program;
 * If not, see http://www.gnu.org/licenses/.
 *
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace deceed.DsLight.EditorGUI.DB
{
    /// <summary>
    /// Helper class for querying metadata from an SQL database.
    /// </summary>
    internal class Analyzer
    {
        private string connectionString;

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="connectionString">connection string</param>
        public Analyzer(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Returns a list of all stored procedure names in the database.
        /// </summary>
        /// <returns>list of strings</returns>
        public List<string> GetSPNames()
        {
            var result = new List<string>();
            DataTable sprocs = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                sprocs = conn.GetSchema("Procedures");
                conn.Close();
            }

            foreach (DataRow row in sprocs.Rows)
            {
                string spType = row["ROUTINE_TYPE"].ToString();
                string spName = row["ROUTINE_NAME"].ToString();
                if (spType == "PROCEDURE" && !spName.StartsWith("sp_"))
                {
                    result.Add(spName);
                }
            }

            result.Sort();
            return result;
        }

        /// <summary>
        /// Returns the meta-data of the given stored procedure or SQL query.
        /// </summary>
        /// <param name="commandText">name of stored procedure or SQL query</param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public Metadata GetQueryMetadata(string commandText, CommandType commandType)
        {
            Metadata md = new Metadata();
            if (commandType == CommandType.StoredProcedure)
            {
                md.Parameters = GetSPParams(commandText);
                md.Columns = GetSPColumns(commandText, md.Parameters);
            }
            else if (commandType == CommandType.Text)
            {
                md.Columns = GetSQLColumns(commandText, md.Parameters);
            }
            return md;
        }

        /// <summary>
        /// Returns a list of parameters of teh given stored procedure.
        /// </summary>
        /// <param name="spName">name of stored procedure</param>
        /// <returns>list of parameters</returns>
        private List<SPParam> GetSPParams(string spName)
        {
            var result = new List<SPParam>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string[] restrictions = new string[4] { conn.Database, null, spName, null };
                
                conn.Open();
                var sprocs = conn.GetSchema("ProcedureParameters", restrictions);
                conn.Close();
                foreach (DataRow row in sprocs.Select("", "ORDINAL_POSITION"))
                {
                    SPParam p = new SPParam();
                    p.Name = row["PARAMETER_NAME"].ToString().Replace("@", "");
                    p.SysType = GetSysType(row["DATA_TYPE"].ToString(), false);
                    p.DbType = GetDbType(row["DATA_TYPE"].ToString());
                    p.IsOutput = row["PARAMETER_MODE"].ToString() == "INOUT";
                    result.Add(p);
                }
            }
            return result;
        }

        /// <summary>
        /// Returns a list of columns that are returned by the given stored procedure.
        /// </summary>
        /// <param name="spName">name of stored procedure</param>
        /// <param name="spParams">parameters</param>
        /// <returns>list of columns</returns>
        private List<Column> GetSPColumns(string spName, List<SPParam> spParams)
        {
            var result = new List<Column>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandText = spName;
                command.CommandType = CommandType.StoredProcedure;

                foreach (var spp in spParams)
                {
                    SqlParameter p = new SqlParameter();
                    p.ParameterName = spp.Name;
                    p.DbType = spp.DbType;
                    command.Parameters.Add(p);
                }

                GetSchemaFromReader(command, result);
                conn.Close();
            }
            return result;
        }

        /// <summary>
        /// Returns a list of columns that are returned by the given SQL query.
        /// </summary>
        /// <param name="sql">SQL query</param>
        /// <param name="spParams">query parameters are added to this list</param>
        /// <returns>list of columns</returns>
        private List<Column> GetSQLColumns(string sql, List<SPParam> spParams)
        {
            var result = new List<Column>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                string rxPattern = @"((?<=\= |\=)@\w*)(\s*/\*\s*dsl:(\w*\??))?";
                foreach (Match match in Regex.Matches(sql, rxPattern))
                {
                    string paramName = match.Groups[1].Value;
                    string saveParamName = paramName.TrimStart('@');
                    if (GetDbTypeFromSysType(saveParamName.ToLower()) != null)
                    {
                        saveParamName = "@" + saveParamName;
                    }
                    DbType? dbType = GetDbTypeFromSysType(match.Groups[3].Value);
                    string paramType = dbType.HasValue ? match.Groups[3].Value : "object";
                    
                    spParams.Add(new SPParam
                    {
                        Name = saveParamName,
                        DbType = dbType ?? DbType.String,
                        SysType = paramType
                    });
                    command.Parameters.Add(new SqlParameter(paramName, null));
                }

                /*string rxPattern = @"(?<=\= |\=)@\w*";
                foreach (System.Text.RegularExpressions.Match item in System.Text.RegularExpressions.Regex.Matches(sql, rxPattern))
                {
                    spParams.Add(new SPParam
                    {
                        Name = item.Value.TrimStart('@'),
                        DbType = DbType.String,
                        SysType = "object"
                    });
                    command.Parameters.Add(new SqlParameter(item.Value, null));
                }*/
    
                GetSchemaFromReader(command, result);

                conn.Close();
            }
            return result;
        }

        /// <summary>
        /// Extract the schema definition from an SqlCommand.
        /// </summary>
        /// <param name="command">SqlCommand</param>
        /// <param name="result">list of columns</param>
        private void GetSchemaFromReader(SqlCommand command, List<Column> result)
        {
            using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.SchemaOnly))
            {
                DataTable schema = reader.GetSchemaTable();
                if (schema != null)
                {
                    Microsoft.CSharp.CSharpCodeProvider codeProvider = new Microsoft.CSharp.CSharpCodeProvider();
                    foreach (DataRow row in schema.Rows)
                    {
                        Column col = new Column()
                        {
                            Name = codeProvider.CreateValidIdentifier((string)row["ColumnName"]),
                            SysType = GetSysType((string)row["DataTypeName"], (bool)row["AllowDBNull"]),
                            DbType = GetDbType((string)row["DataTypeName"]).ToString(),
                            IsNullable = (bool)row["AllowDBNull"]
                        };
                        result.Add(col);
                    }

                    // add unique names for columns without a name
                    // ensure that column names are unique and add a sequence number if neccessary
                    string nameTemplate = "Column{0}";
                    int columnNr = 1;
                    foreach (Column col in result)
                    {
                        if (String.IsNullOrEmpty(col.Name))
                        {
                            string newName;
                            do
                            {
                                newName = String.Format(nameTemplate, columnNr++);
                            } while (result.Any(x => x.Name == newName));
                            col.Name = newName;
                        }
                        else
                        {
                            string colName = col.Name;
                            int seq = 1;
                            while (result.Any(x => x != col && x.Name == col.Name))
                            {
                                col.Name = colName + (seq++).ToString();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Convert an SQL data-type into a C# data-type.
        /// </summary>
        /// <param name="sqlType">name of SQL data-type</param>
        /// <param name="isNullable">whether the SQL data-type is nullable</param>
        /// <returns>C# data-type</returns>
        private string GetSysType(string sqlType, bool isNullable)
        {
            bool allowNullable = true;
            string sysType;
            switch (sqlType)
            {
                case "bigint":
                    sysType = "long";
                    break;
                case "smallint":
                    sysType = "short";
                    break;
                case "int":
                    sysType = "int";
                    break;
                case "uniqueidentifier":
                    sysType = "Guid";
                    break;
                case "smalldatetime":
                case "datetime":
                case "datetime2":
                case "date":
                    sysType = "DateTime";
                    break;
                case "time":
                    sysType = "TimeSpan";
                    break;
                case "datetimeoffset":
                    sysType = "DateTimeOffset";
                    break;
                case "float":
                case "real":
                    sysType = "double";
                    break;
                case "numeric":
                case "smallmoney":
                case "decimal":
                case "money":
                    sysType = "decimal";
                    break;
                case "tinyint":
                    sysType = "byte";
                    break;
                case "bit":
                    sysType = "bool";
                    break;
                case "image":
                case "binary":
                case "varbinary":
                case "timestamp":
                    sysType = "byte[]";
                    allowNullable = false;
                    break;
                default:
                    sysType = "string";
                    allowNullable = false;
                    break;
            }

            if (isNullable && allowNullable)
            {
                sysType += "?";
            }

            return sysType;
        }

        /// <summary>
        /// Returns the DbType that corresponds to the SQL data-type with the given name.
        /// </summary>
        /// <param name="sqlType">SQL data-type</param>
        /// <returns>DbType</returns>
        private DbType GetDbType(string sqlType)
        {
            switch (sqlType)
            {
                case "varchar":
                    return DbType.AnsiString;
                case "nvarchar":
                    return DbType.String;
                case "int":
                    return DbType.Int32;
                case "uniqueidentifier":
                    return DbType.Guid;
                case "datetime":
                case "datetime2":
                case "smalldatetime":
                case "date":
                    return DbType.DateTime;
                case "time":
                    return DbType.Time;
                case "datetimeoffset":
                    return DbType.DateTimeOffset;
                case "bigint":
                    return DbType.Int64;
                case "binary":
                    return DbType.Binary;
                case "bit":
                    return DbType.Boolean;
                case "char":
                    return DbType.AnsiStringFixedLength;
                case "decimal":
                    return DbType.Decimal;
                case "float":
                    return DbType.Double;
                case "image":
                    return DbType.Binary;
                case "money":
                    return DbType.Currency;
                case "nchar":
                    return DbType.String;
                case "ntext":
                    return DbType.String;
                case "numeric":
                    return DbType.Decimal;
                case "real":
                    return DbType.Single;
                case "smallint":
                    return DbType.Int16;
                case "smallmoney":
                    return DbType.Currency;
                case "sql_variant":
                    return DbType.String;
                case "sysname":
                    return DbType.String;
                case "text":
                    return DbType.AnsiString;
                case "timestamp":
                    return DbType.Binary;
                case "tinyint":
                    return DbType.Byte;
                case "varbinary":
                    return DbType.Binary;
                case "xml":
                    return DbType.Xml;
                default:
                    return DbType.AnsiString;
            }
        }

        /// <summary>
        /// Returns the DbType that matches the given .NET data type.
        /// </summary>
        /// <param name="sysType">name of .NET data type</param>
        /// <returns>DbType or null</returns>
        private DbType? GetDbTypeFromSysType(string sysType)
        {
            switch (sysType.TrimEnd('?'))
            {
                case "byte":
                case "Byte":
                    return DbType.Byte;
                case "sbyte":
                case "SByte":
                    return DbType.SByte;
                 case "short":
                 case "Int16":   
                    return DbType.Int16;
                case "ushort":
                case "UInt16":
                    return DbType.UInt16;
                case "int":
                case "Int32":
                    return DbType.Int32;
                case "uint":
                case "Unt32":
                    return DbType.UInt32;
                case "long":
                case "Int64":
                    return DbType.Int64;
                case "ulong":
                case "UInt64":
                    return DbType.UInt64;
                case "float":
                    return DbType.Single;
                case "double":
                case "Double":
                    return DbType.Double;
                case "decimal":
                case "Decimal":
                    return DbType.Decimal;
                case "bool":
                case "Boolean":
                    return DbType.Boolean;
                case "string":
                case "String":
                    return DbType.String;
                case "char":
                case "Char":    
                    return DbType.StringFixedLength;
                case "Guid":
                    return DbType.Guid;
                case "DateTime":
                    return DbType.DateTime;
                case "DateTimeOffset":
                    return DbType.DateTimeOffset;
                case "byte[]":
                    return DbType.Binary;
                default:
                    return null;
            }
        }
    }
}
