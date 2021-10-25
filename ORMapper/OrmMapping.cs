using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using ORMapper.Models;

namespace ORMapper
{
    public static class OrmMapping
    {
        /// <summary>
        /// prints Sql Statements to console in order
        /// </summary>
        /// <param name="Tables">Array of Types that represent Tables that should be parsed</param>
        /// <param name="enums">Array of Types that represent Enums that should be parsed</param>
        public static void Map(Type[] Tables, Type[] enums)
        {
            List<string> sqlCommands = new();
            List<Table> tableList = new();
            foreach (var table in Tables) tableList.Add(new Table(table));

            parseEnums(sqlCommands, enums);
            parseTables(sqlCommands, Tables);


            print(sqlCommands);
        }
        /// <summary>
        /// prints a list of strings to console
        /// </summary>
        /// <param name="sqlCommands">a list of strings in order of which they should be executed</param>
        private static void print(List<string> sqlCommands)
        {
            foreach (var line in sqlCommands) Console.WriteLine(line);
        }
        /// <summary>
        /// parses enumns into a list of create strings
        /// </summary>
        /// <param name="sqlcommands">list of sqlstrings which the new commands are added too</param>
        /// <param name="enums">list of enums that should be parsed</param>
        private static void parseEnums(List<string> sqlcommands, Type[] enums)
        {
            foreach (var i in enums)
            {
                var sql = "Create Type " + i.Name + " as enum (";
                var values = Enum.GetValues(i);
                foreach (var x in values)
                {
                    sql += "'" + x + "',";
                }

                sql = sql.Trim(',');
                sql += ");";
                sqlcommands.Add(sql);
            }
        }
        /// <summary>
        /// parses tables into a list of create strings
        /// </summary>
        /// <param name="sqlcommands">list of sqlstrings which the new commands are added too</param>
        /// <param name="Tables">list of tables that should be parsed</param>
        private static void parseTables(List<string> sqlcommands, Type[] Tables)
        {
            List<string> foreingKeySqlList = new();
            foreach (var i in Tables)
            {
                var createSqlScript = "Create Table " + i._GetTable().TableName + "(";
                foreach (var column in i._GetTable().Columns)
                {
                    if (!column.IsForeignKey)
                    {
                        createSqlScript += column.ColumnName + " " + toDatabaseType(column.Type) + " ";
                        if (!column.IsNullable) createSqlScript += " not null ";
                        if (column.IsPrimaryKey) createSqlScript += " primary key ";

                        createSqlScript += ", ";
                    }
                    else
                    {
                        if(column.ColumnType.GetInterface(nameof(ICollection)) == null)
                        {
                            createSqlScript += column.ColumnName + " " + toDatabaseType(column.Type._GetTable().PrimaryKey.Type) + " not null ,";
                        }
                        if (!column.IsManyToMany)
                        {

                            Column insert = (column.ColumnType.GetInterface(nameof(ICollection)) != null)
                                ? column.Type.GetGenericArguments()[0]._GetTable().PrimaryKey
                                : column.Type._GetTable().PrimaryKey;
                            
                            string a;
                            string b;
                            string c;
                            string d;

                            if (column.ColumnType.GetInterface(nameof(ICollection)) == null)
                            {
                                a = column.Table.TableName;
                                b = column.ColumnName;
                                c = insert.Table.TableName;
                                d = insert.Table.PrimaryKey.ColumnName;
                            }
                            else
                            {
                                a = insert.Table.TableName; 
                                b = column.ColumnName;
                                c = column.Table.TableName;
                                d = insert.Table.PrimaryKey.ColumnName;
                            }
                            foreingKeySqlList.Add("Alter table " + a + " ADD Constraint \"" +
                                            Math.Abs(Guid.NewGuid().GetHashCode() + Guid.NewGuid().GetHashCode() *100000) +
                                            "\" Foreign Key (" + b +
                                            ") references " + c + "(" +
                                            d +") on update cascade;");
                        }
                        else
                        {
                            string a = column.Table.TableName;
                            string b = column.MyReferenceToThisColumnName;
                            string c = column.Table.TableName ;
                            string d = column.Table.PrimaryKey.Type.Name;

                            if (column.MyReferenceToThisColumnName != null)
                            {
                                a = column._GetTable().TableName;
                            }
                            
                            foreingKeySqlList.Add("Alter table " + column.RemoteTable._GetTable().TableName + " ADD Constraint \"" +
                                            Math.Abs(Guid.NewGuid().GetHashCode() + Guid.NewGuid().GetHashCode() *100000) +
                                            "\" Foreign Key (" + column.MyReferenceToThisColumnName  +
                                            ") references " + column.Table.TableName + "(" +
                                            column.Table.PrimaryKey.ColumnName +") on update cascade;");
                        }
                    }
                }
                createSqlScript = createSqlScript.Trim().Trim(',');
                createSqlScript += ");";
                sqlcommands.Add(createSqlScript);
            }
            foreach (var y in foreingKeySqlList) sqlcommands.Add(y);
        }

        private static string toDatabaseType(Type e)
        {
            if (e.IsEnum)
            {
                return e.Name;
            }
            if (e == typeof(string))
            {
                return "text";
            }

            if (e == typeof(int))
            {
                return "integer";
            }
            if (e == typeof(short))
            {
                return "smallint";
            }
            if (e == typeof(long))
            {
                return "bigint";
            }

            if (e == typeof(float) || e == typeof(double))
            {
                return "numeric";
            }

            if (e == typeof(DateTime))
            {
                return "Timestamp";
            }
            throw new Exception("type not recognized");
        }
    }
}