using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Transactions;
using Npgsql;
using ORMapper.Attributes;

namespace ORMapper
{
    public static class OrmMapping
    {
        /// <summary>
        ///     prints Sql Statements to console in order
        /// </summary>
        /// <param name="Tables">Array of Types that represent Tables that should be parsed</param>
        /// <param name="enums">Array of Types that represent Enums that should be parsed</param>
        public static void Map(Type[] TypesToMap, bool SwitchConsoleOrDb = true)
        {
            List<string> sqlCommands = new();

            parseEnums(sqlCommands, TypesToMap.Where(x => x.IsEnum).ToArray());
            parseTables(sqlCommands, TypesToMap.Where(x => x.IsDefined(typeof(TableAttribute))).ToArray());


            if (SwitchConsoleOrDb)
                print(sqlCommands);
            else
                InsertIntoDb(sqlCommands);
        }

        private static void InsertIntoDb(List<string> sqlCommand)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                var con = Orm.Connection();
                con.Open();
                foreach (var x in sqlCommand)
                {
                    var command = con.CreateCommand();

                    command.CommandText = x;
                    command.ExecuteNonQuery();
                    command.Dispose();
                }
                con.Close();

                var connection = Orm.Connection();
                connection.Open();
                connection.ReloadTypes();
                connection.Close();
                scope.Complete();
            }
            
        }


        /// <summary>
        ///     prints a list of strings to console
        /// </summary>
        /// <param name="sqlCommands">a list of strings in order of which they should be executed</param>
        private static void print(List<string> sqlCommands)
        {
            foreach (var line in sqlCommands) Console.WriteLine(line);
        }

        /// <summary>
        ///     parses enumns into a list of create strings
        /// </summary>
        /// <param name="sqlcommands">list of sqlstrings which the new commands are added too</param>
        /// <param name="enums">list of enums that should be parsed</param>
        public static void parseEnums(List<string> sqlcommands, Type[] enums)
        {
            foreach (var i in enums)
            {
                if (!i.IsEnum) continue;
                
                var values = Enum.GetValues(i);
                string insert = "";
                foreach (var x in values) insert += "'" + x + "',";
                insert = insert.Trim(',');
                if (string.IsNullOrWhiteSpace(insert)) continue;
                string sql = "DO $$ BEGIN";
                sql += "CREATE TYPE my_type AS (" + insert +")";
                    
                sql += "EXCEPTION";
                sql += "WHEN duplicate_object THEN null";
                sql += "END $$";

                sqlcommands.Add(sql);
            }
        }

        /// <summary>
        ///     parses tables into a list of create strings
        /// </summary>
        /// <param name="sqlcommands">list of sqlstrings which the new commands are added too</param>
        /// <param name="Tables">list of tables that should be parsed</param>
        public static void parseTables(List<string> sqlcommands, Type[] Tables)
        {
            List<string> foreingKeySqlList = new();
            foreach (var i in Tables)
            {
                var createSqlScript = "Create Table IF NOT EXISTS " + i._GetTable().TableName + " (";
                foreach (var column in i._GetTable().Columns)
                    if (!column.IsForeignKey)
                    {
                        createSqlScript += column.ColumnName + " " + toDatabaseType(column.Type) + " ";
                        if (!column.IsNullable) createSqlScript += " not null ";
                        if (column.IsPrimaryKey) createSqlScript += " primary key ";

                        createSqlScript += ", ";
                    }
                    else
                    {
                        if (column.ColumnType.GetInterface(nameof(ICollection)) == null)
                            createSqlScript += column.ColumnName + " " +
                                               toDatabaseType(column.Type._GetTable().PrimaryKey.Type) + " not null ,";
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

                            foreingKeySqlList.Add("ALTER TABLE " + whichTableToALter + " DROP CONSTRAINT IF EXISTS " +
                                                  "\"fk_" + column.Table.TableName + "_" + column.ColumnName + "_" +
                                                  whichTableToALter + "_" + whichForeignKeyIsReferenced + "\";");
                            foreingKeySqlList.Add("Alter table " + whichTableToALter + " ADD Constraint \"" +
                                                  "fk_" + column.Table.TableName + "_" + column.ColumnName + "_" +
                                                  whichTableToALter + "_" + whichForeignKeyIsReferenced +
                                                  "\" Foreign Key (" + whichForeignkeyToConstraint +
                                                  ") references " + whichForeignTableIsReferenced + "(" +
                                                  whichForeignKeyIsReferenced +
                                                  ") on update cascade on delete cascade;");
                        }
                        else
                        {
                            foreingKeySqlList.Add("ALTER TABLE " + column.RemoteTable._GetTable().TableName +
                                                  " DROP CONSTRAINT IF EXISTS " + "\"fk_" + column.Table.TableName +
                                                  "_" + column.ColumnName + "_" +
                                                  column.RemoteTable._GetTable().TableName + "_" +
                                                  column.Table.PrimaryKey.ColumnName + "\";");
                            foreingKeySqlList.Add("Alter table " + column.RemoteTable._GetTable().TableName +
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
                sqlcommands.Add(createSqlScript);
            }

            foreach (var y in foreingKeySqlList) sqlcommands.Add(y);
        }

        public static string toDatabaseType(Type e)
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
    }
}