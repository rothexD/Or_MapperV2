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
        public Type Member {  get;  private set;}
        public string TableName { get; private set; }
        public Column[] Columns { get; private set; }
        public Column PrimaryKey { get; set; }
        
        public Column[] Externals { get; set; }
        public Column[] Internals { get; set; }



        public Table(Type type)
        {
            TableAttribute tattr = (TableAttribute) type.GetCustomAttribute(typeof(TableAttribute));
            Member = type;
            List<Column> column = new List<Column>();



            if (tattr == null || string.IsNullOrWhiteSpace(tattr.TableName))
            {
                TableName = type.Name.ToUpper();
            }
            else
            {
                TableName = tattr.TableName;
            }

            foreach(PropertyInfo info in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if ((IgnoreAttribute)info.GetCustomAttribute(typeof(IgnoreAttribute)) is not null)
                {
                    continue;
                }

                Column field = new(this);

                ColumnAttribute fattr = (ColumnAttribute) info.GetCustomAttribute(typeof(ColumnAttribute));
                if(fattr is not null)
                {
                    if(fattr is PrimaryKeyAttribute)
                    {
                        PrimaryKey = field;
                        field.IsPrimaryKey = true;
                    }
                    field.ColumnName = (fattr?.ColumnName ?? info.Name);
                    field.ColumnType = (fattr?.ColumnType ?? info.PropertyType);
                    field.IsNullable = fattr.Nullable;

                    if(field.IsForeignKey = (fattr is ForeignKeyAttribute))
                    {
                        field.IsExternal = typeof(IEnumerable).IsAssignableFrom(info.PropertyType);

                        field.RemoteTable = ((ForeignKeyAttribute)fattr).RemoteTableName;
                        field.RemoteColumnName = ((ForeignKeyAttribute)fattr).RemoteColumnName;
                        field.IsManyToMany = (!string.IsNullOrWhiteSpace(field.RemoteTable));
                    }
                }
                else
                {
                    if ((info.GetGetMethod() == null) || (!info.GetGetMethod().IsPublic)) { continue; }
                    field.ColumnType = info.PropertyType;
                    field.ColumnName = info.Name;
                    field.IsNullable = false;
                }
                field.Member = info;
                column.Add(field);
            }
            Columns = column.ToArray();

            Internals = Columns.Where(x => (!x.IsExternal)).ToArray();
            Externals = Columns.Where(x => x.IsExternal).ToArray();
        }
        
        public string GetSelectSQL(string prefix ="")
        {
            if(prefix is null) { prefix = ""; }

            string sqlString = "Select ";
            for(int i = 0; i< Internals.Length; i++)
            {
                if( i > 0) { sqlString += ", "; }
                sqlString += prefix.Trim() + Internals[i].ColumnName;
            }
            sqlString += " From " + TableName;
            return sqlString;
        }
        public Column GetFieldForColumn(string columnName)
        {
            columnName = columnName.ToUpper();
            foreach (Column i in Internals)
            {
                if(i.ColumnName.ToUpper() == columnName)
                {
                    return i;
                }
            }
            return null;
        }
    }
}