using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Transactions;
using Npgsql;
using OrMapper.Attributes;
using ORMapper.Models;
using OrMapper.Helpers.extentions;

namespace ORMapper
{
    public static class OrmMapping
    {
        /// <summary>
        ///     prints Sql Statements to console in order
        /// </summary>
        /// <param name="typesToMap">Array of Types that represent Tables that should be parsed</param>
        /// <param name="automaticInsertIntoDb">print to console or try to insert into db</param>
        public static List<string> Map(Type[] typesToMap, bool automaticInsertIntoDb = false)
        {
            List<string> sqlCommands = new();

            ParseEnums(sqlCommands, typesToMap.Where(x => x.IsEnum).ToArray());
            ParseTables(sqlCommands, typesToMap.Where(x => x.IsDefined(typeof(TableAttribute))).ToArray());


            if (automaticInsertIntoDb)
                _InsertIntoDb(sqlCommands);
            
            return sqlCommands;
        }


        /// <summary>
        ///     parses enumns into a list of create strings
        /// </summary>
        /// <param name="sqlCommands">list of sqlstrings which the new commands are added too</param>
        /// <param name="enums">list of enums that should be parsed</param>
        public static void ParseEnums(List<string> sqlCommands, Type[] enums)
        {
            foreach (var i in enums)
            {
                if (!i.IsEnum) continue;
                
                var values = Enum.GetValues(i);
                string insert = "";
                foreach (var x in values) insert += "'" + x + "',";
                insert = insert.Trim(',');
                if (string.IsNullOrWhiteSpace(insert)) continue;
                string sql = "DO $$ BEGIN ";
                sql += " CREATE TYPE "+ i.Name +" AS Enum (" + insert +"); ";
                    
                sql += " EXCEPTION ";
                sql += " WHEN duplicate_object THEN null; ";
                sql += " END $$;";
                sqlCommands.Add(sql);
            }
        }

        /// <summary>
        ///     parses tables into a list of create strings
        /// </summary>
        /// <param name="sqlCommands">list of sqlstrings which the new commands are added too</param>
        /// <param name="Tables">list of tables that should be parsed</param>
        public static void ParseTables(List<string> sqlCommands, Type[] Tables)
        {
            List<string> foreginKeySqlList = new();
            foreach (var i in Tables)
            {
                if (i._GetTable().IsManyToManyTable)
                {
                    var createsqlscript = "Create Table IF NOT EXISTS " + i._GetTable().TableName + " (";
                    Column col1 = i._GetTable().Columns[0];
                    Column col2 = i._GetTable().Columns[1];

                    createsqlscript += col1.ColumnName + " " + ToDatabaseType(col1.Type);
                    createsqlscript += ", ";
                    createsqlscript += col2.ColumnName + " " + ToDatabaseType(col2.Type);
                    createsqlscript += ", ";
                    createsqlscript += "primary key("+col1.ColumnName+","+col2.ColumnName+")";
                    createsqlscript += ");";
                    sqlCommands.Add(createsqlscript);
                    continue;
                }
                var createSqlScript = "Create Table IF NOT EXISTS " + i._GetTable().TableName + " (";
                foreach (var column in i._GetTable().Columns)
                    if (!column.IsForeignKey)
                    {
                        createSqlScript += column.ColumnName + " " + ToDatabaseType(column.Type) + " ";
                        if (!column.IsNullable) createSqlScript += " not null ";
                        if (column.IsPrimaryKey) createSqlScript += " primary key ";

                        createSqlScript += ", ";
                    }
                    else
                    {
                        if (column.ColumnType.GetInterface(nameof(ICollection)) == null)
                        {
                            createSqlScript += column.ColumnName + " " +
                                               ToDatabaseType(column.Type._GetTable().PrimaryKey.Type) +
                                               " not null ,";
                        }
                            
                            
                        if (!column.IsManyToMany)
                        {
                            var insert = column.ColumnType.GetInterface(nameof(ICollection)) != null
                                ? column.Type.GetGenericArguments()[0]._GetTable().PrimaryKey
                                : column.Type._GetTable().PrimaryKey;

                            string whichTableToALter;
                            string whichForeignkeyToConstraint;
                            string whichForeignTableIsReferenced;
                            string whichForeignKeyIsReferenced;

                            if (column.ColumnType.GetInterface(nameof(ICollection)) == null)
                            {
                                whichTableToALter = column.Table.TableName;
                                whichForeignkeyToConstraint = column.ColumnName;
                                whichForeignTableIsReferenced = insert.Table.TableName;
                                whichForeignKeyIsReferenced = insert.Table.PrimaryKey.ColumnName;
                            }
                            else
                            {
                                whichTableToALter = insert.Table.TableName;
                                whichForeignkeyToConstraint = column.ColumnName;
                                whichForeignTableIsReferenced = column.Table.TableName;
                                whichForeignKeyIsReferenced = insert.Table.PrimaryKey.ColumnName;
                            }

                            foreginKeySqlList.Add("ALTER TABLE " + whichTableToALter + " DROP CONSTRAINT IF EXISTS " +
                                                  "\"fk_" + column.Table.TableName + "_" + column.ColumnName + "_" +
                                                  whichTableToALter + "_" + whichForeignKeyIsReferenced + "\";");
                            foreginKeySqlList.Add("Alter table " + whichTableToALter + " ADD Constraint \"" +
                                                  "fk_" + column.Table.TableName + "_" + column.ColumnName + "_" +
                                                  whichTableToALter + "_" + whichForeignKeyIsReferenced +
                                                  "\" Foreign Key (" + whichForeignkeyToConstraint +
                                                  ") references " + whichForeignTableIsReferenced + "(" +
                                                  whichForeignKeyIsReferenced +
                                                  ") on update cascade on delete cascade;");
                        }
                        else
                        {
                            foreginKeySqlList.Add("ALTER TABLE " + column.RemoteTable._GetTable().TableName +
                                                  " DROP CONSTRAINT IF EXISTS " + "\"fk_" + column.Table.TableName +
                                                  "_" + column.ColumnName + "_" +
                                                  column.RemoteTable._GetTable().TableName + "_" +
                                                  column.Table.PrimaryKey.ColumnName + "\";");
                            foreginKeySqlList.Add("Alter table " + column.RemoteTable._GetTable().TableName +
                                                  " ADD Constraint \"" +
                                                  "fk_" + column.Table.TableName + "_" + column.ColumnName + "_" +
                                                  column.RemoteTable._GetTable().TableName + "_" +
                                                  column.Table.PrimaryKey.ColumnName +
                                                  "\" Foreign Key (" + column.ColumnName +
                                                  ") references " + column.Table.TableName + "(" +
                                                  column.Table.PrimaryKey.ColumnName +
                                                  ") on update cascade on delete cascade;");
                        }
                    }

                createSqlScript = createSqlScript.Trim().Trim(',');
                createSqlScript += ");";
                sqlCommands.Add(createSqlScript);
            }

            foreach (var y in foreginKeySqlList) sqlCommands.Add(y);
        }
        
        public static string ToDatabaseType(Type e)
        {
            if (e.IsEnum) return e.Name;
            if (e == typeof(string)) return "text";
            if (e == typeof(char)) return "text";

            if (e == typeof(int)) return "integer";
            if (e == typeof(short)) return "smallint";
            if (e == typeof(long)) return "bigint";

            if (e == typeof(float) || e == typeof(double)) return "numeric";

            if (e == typeof(DateTime)) return "Timestamp";
            throw new Exception("type not recognized");
        }
        public static void _InsertIntoDb(List<string> sqlCommand,IDbConnection con = null)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                if (con is null)
                {
                    con = Orm.Connection();
                }
                con.Open();
                foreach (var x in sqlCommand)
                {
                    var command = con.CreateCommand();

                    command.CommandText = x;
                    command.ExecuteNonQuery();
                    command.Dispose();
                }
                con.Close();
                scope.Complete();
            }
        }
        public static void ReloadTypes()
        {
            var connection = Orm.Connection();
            connection.Open();
            connection.ReloadTypes();
            connection.Close();
        }
        /// <summary>
        ///     prints a list of strings to console
        /// </summary>
        /// <param name="sqlCommands">a list of strings in order of which they should be executed</param>
        public static void _Print(List<string> sqlCommands)
        {
            foreach (var line in sqlCommands) Console.WriteLine(line);
        }
    }
}