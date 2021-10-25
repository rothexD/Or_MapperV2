using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ORMapper.Attributes;

namespace ORMapper.Models
{
    public class Table
    {   
        /// <summary>
        /// Constructor, parses a Table into Columns
        /// </summary>
        /// <param name="type">what Type to parse</param>
        public Table(Type type)
        {
            var tattr = (TableAttribute) type.GetCustomAttribute(typeof(TableAttribute));
            Member = type;
            var column = new List<Column>();


            if (tattr == null || string.IsNullOrWhiteSpace(tattr.TableName))
                TableName = type.Name.ToUpper();
            else
                TableName = tattr.TableName;

            foreach (var info in type.GetProperties(
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if ((IgnoreAttribute) info.GetCustomAttribute(typeof(IgnoreAttribute)) is not null) continue;

                Column field = new(this);

                var fattr = (ColumnAttribute) info.GetCustomAttribute(typeof(ColumnAttribute));
                if (fattr is not null)
                {
                    if (fattr is PrimaryKeyAttribute)
                    {
                        PrimaryKey = field;
                        field.IsPrimaryKey = true;
                    }

                    field.ColumnName = fattr?.ColumnName ?? info.Name;
                    field.ColumnType = fattr?.ColumnType ?? info.PropertyType;
                    field.IsNullable = fattr.Nullable;

                    if (field.IsForeignKey = fattr is ForeignKeyAttribute)
                    {
                        field.IsExternal = typeof(IEnumerable).IsAssignableFrom(info.PropertyType);

                        field.RemoteTable = ((ForeignKeyAttribute) fattr).RemoteTableName ?? null;
                        field.MyReferenceToThisColumnName = ((ForeignKeyAttribute) fattr).MyReferenceToThisColumnName;
                        field.TheirReferenceToThisColumnName = ((ForeignKeyAttribute) fattr).TheirReferenceToThisColumnName;
                        field.IsManyToMany = ((ForeignKeyAttribute) fattr).isManyToMany;
                    }
                }
                else
                {
                    if (info.GetGetMethod() == null || !info.GetGetMethod().IsPublic) continue;
                    field.ColumnType = info.PropertyType;
                    field.ColumnName = info.Name;
                    field.IsNullable = false;
                }

                field.Member = info;
                column.Add(field);
            }

            Columns = column.ToArray();

            Internals = Columns.Where(x => !x.IsExternal).ToArray();
            Externals = Columns.Where(x => x.IsExternal).ToArray();
        }

        public Type Member { get; }
        public string TableName { get; }
        public Column[] Columns { get; }
        public Column PrimaryKey { get; set; }

        public Column[] Externals { get; set; }
        public Column[] Internals { get; set; }

        /// <summary>
        /// gets a basic select * from table statement
        /// </summary>
        /// <param name="prefix">defines a prefix that can be attached</param>
        /// <returns>a sql query string</returns>
        public string GetSelectSql(string prefix = "")
        {
            if (prefix is null) prefix = "";

            var sqlString = "Select ";
            for (var i = 0; i < Internals.Length; i++)
            {
                if (i > 0) sqlString += ", ";
                sqlString += prefix.Trim() + Internals[i].ColumnName;
            }

            sqlString += " From " + TableName;
            return sqlString;
        }
    }
}