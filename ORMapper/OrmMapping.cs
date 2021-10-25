using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using ORMapper.Models;

namespace ORMapper
{
    public static class OrmMapping
    {
        public static void Map(Type[] Tables, Type[] enums)
        {
            List<string> sqlCommands = new();
            List<Table> tableList = new();
            foreach (var table in Tables) tableList.Add(new Table(table));

            parseEnums(sqlCommands, enums);
            parseTables(sqlCommands, Tables);


            print(sqlCommands);
        }

        private static void print(List<string> sqlCommands)
        {
            foreach (var line in sqlCommands) Console.WriteLine(line);
        }

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

        private static void parseTables(List<string> sqlcommands, Type[] Tables)
        {
            List<string> foreignkeys = new();
            foreach (var i in Tables)
            {
                var sql = "Create Table " + i._GetEntity().TableName + "(";
                foreach (var column in i._GetEntity().Columns)
                {
                    if (!column.IsForeignKey)
                    {
                        sql += column.ColumnName + " " + toDatabaseType(column.Type) + " ";
                        if (!column.IsNullable) sql += " not null ";
                        if (column.IsPrimaryKey) sql += " primary key ";

                        sql += ", ";
                    }
                    else
                    {
                        if(column.ColumnType.GetInterface(nameof(ICollection)) == null)
                        {
                            sql += column.ColumnName + " " + toDatabaseType(column.Type._GetEntity().PrimaryKey.Type) + " not null ,";
                        }
                        if (!column.IsManyToMany)
                        {

                            Column insert = (column.ColumnType.GetInterface(nameof(ICollection)) != null)
                                ? column.Type.GetGenericArguments()[0]._GetEntity().PrimaryKey
                                : column.Type._GetEntity().PrimaryKey;
                            
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
                            foreignkeys.Add("Alter table " + a + " ADD Constraint \"" +
                                            Math.Abs(Guid.NewGuid().GetHashCode() + Guid.NewGuid().GetHashCode() *100000) +
                                            "\" Foreign Key (" + b +
                                            ") references " + c + "(" +
                                            d +");");
                        }
                        else
                        {
                            string a = column.Table.TableName;
                            string b = column.MyReferenceToThisColumnName;
                            string c = column.Table.TableName ;
                            string d = column.Table.PrimaryKey.Type.Name;

                            if (column.MyReferenceToThisColumnName != null)
                            {
                                a = column._GetEntity().TableName;
                            }
                            
                            foreignkeys.Add("Alter table " + column.RemoteTable._GetEntity().TableName + " ADD Constraint \"" +
                                            Math.Abs(Guid.NewGuid().GetHashCode() + Guid.NewGuid().GetHashCode() *100000) +
                                            "\" Foreign Key (" + column.MyReferenceToThisColumnName  +
                                            ") references " + column.Table.TableName + "(" +
                                            column.Table.PrimaryKey.ColumnName +");");
                        }
                    }
                }
                sql = sql.Trim().Trim(',');
                sql += ");";
                sqlcommands.Add(sql);
            }
            foreach (var y in foreignkeys) sqlcommands.Add(y);
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