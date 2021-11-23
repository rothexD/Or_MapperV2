using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OrMapper.Attributes;

namespace ORMapper.Models
{
    public class Table
    {   
        //Todo: add logging
        //Todo: add code comments
        
        /// <summary>
        /// Constructor, parses a Table into Columns
        /// </summary>
        /// <param name="type">what Type to parse</param>
        public Table(Type type)
        {
            var tattr = (TableAttribute) type.GetCustomAttribute(typeof(TableAttribute));
            Member = type;
            var columnList = new List<Column>();

            //set table name
            if (tattr == null || string.IsNullOrWhiteSpace(tattr.TableName))
                TableName = type.Name.ToUpper();
            else
                TableName = tattr.TableName;
            
            //is as many to many table defined ?
            if (tattr != null)
                IsManyToManyTable = tattr.IsManyToManyTable;
            
            foreach (var info in type.GetProperties(
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                //if ingored continue and dont consider
                if ((IgnoreAttribute) info.GetCustomAttribute(typeof(IgnoreAttribute)) is not null) continue;

                Column column = new(this);
                //get attributes and parse for property, if no columnattribute is set default functinonallity
                var fattr = (ColumnAttribute) info.GetCustomAttribute(typeof(ColumnAttribute));
                if (fattr is not null)
                {
                    // is a primary key?
                    if (fattr is PrimaryKeyAttribute)
                    {
                        PrimaryKey = column;
                        column.IsPrimaryKey = true;
                    }
                    //Set columname, table and nullable if set in columnattribute
                    column.ColumnName = fattr?.ColumnName ?? info.Name;
                    column.ColumnType = fattr?.ColumnType ?? info.PropertyType;
                    column.IsNullable = fattr.Nullable;
                    
                    // if it is a foreign key
                    if (column.IsForeignKey = fattr is ForeignKeyAttribute)
                    {
                        //is external if it is IEnumerable
                        column.IsExternal = typeof(IEnumerable).IsAssignableFrom(info.PropertyType);
                        
                        // configures a many to many external
                        if (fattr is ForeignKeyManyToMany)
                        {
                            column.RemoteTable = ((ForeignKeyManyToMany) fattr).RemoteTableName;
                            column.TheirReferenceToThisColumnName = ((ForeignKeyManyToMany) fattr).TheirReferenceToThisColumnName;
                        }
                        column.IsManyToMany = !string.IsNullOrEmpty(column.TheirReferenceToThisColumnName);
                    }
                }
                else
                {
                    //if has a public get method we consider it saveable
                    if (info.GetGetMethod() == null || !info.GetGetMethod().IsPublic) continue;
                    column.ColumnType = info.PropertyType;
                    column.ColumnName = info.Name;
                    column.IsNullable = false;
                }

                column.Member = info;
                columnList.Add(column);
            }
            Columns = columnList.ToArray();
            // split all columns in internals and externals
            Internals = Columns.Where(x => !x.IsExternal).ToArray();
            Externals = Columns.Where(x => x.IsExternal).ToArray();
        }

        public Type Member { get; }
        public string TableName { get; }
        public Column[] Columns { get; }
        public Column PrimaryKey { get; set; }

        public Column[] Externals { get; set; }
        public Column[] Internals { get; set; }

        public bool IsManyToManyTable { get; set; } = false;
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