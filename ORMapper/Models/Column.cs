using System;
using System.Collections.Generic;
using System.Reflection;

namespace ORMapper.Models
{
    public class Column
    {
        public Column(Table entity)
        {
            Table = entity;
        }

        public Table Table { get; internal set; }

        public MemberInfo Member { get; internal set; }

        public Type Type
        {
            get
            {
                if (Member is PropertyInfo) return ((PropertyInfo) Member).PropertyType;
                throw new NotSupportedException();
            }
        }

        public string ColumnName { get; internal set; }
        public Type ColumnType { get; internal set; }

        public bool IsPrimaryKey { get; internal set; }
        public bool IsForeignKey { get; internal set; }
        public bool IsNullable { get; internal set; }

        public bool IsExternal { get; internal set; } = false;
        
        public Type RemoteTable { get; internal set; }
        
        public string MyReferenceToThisColumnName { get; internal set; }
        public string TheirReferenceToThisColumnName { get; internal set; }
        public bool IsManyToMany { get; internal set; }

        public object GetValue(object obj)
        {
            if (Member is PropertyInfo)
                return ((PropertyInfo) Member).GetValue(obj);
            throw new NotSupportedException("Member type not supported");
        }

        public void SetValue(object obj, object value)
        {
            if (Member is PropertyInfo)
                ((PropertyInfo) Member).SetValue(obj, value);
            else
                throw new NotSupportedException("Member type not supported");
        }

        public object ToColumnType(object value, ICollection<object> localchache)
        {
            if (IsForeignKey)
            {
                if (value == null) return null;
                var i = value._GetEntity().PrimaryKey
                    .ToColumnType(value._GetEntity().PrimaryKey.GetValue(value), localchache);
                //localchache.Add(i);
                return i;
            }

            if (Type == ColumnType)
            {
                return value;
            }

            if (value is bool)
            {
                if (ColumnType == typeof(int)) return (bool) value ? 1 : 0;
                if (ColumnType == typeof(short)) return (bool) value ? 1 : 0;
                if (ColumnType == typeof(float)) return (bool) value ? 1 : 0;
            }

            return value;
        }

        public object ToFieldType(object value, ICollection<object> localcache) //,ICollection<object> localcache
        {
            if (IsForeignKey) return Orm._CreateObject(Type, value, localcache);

            if (Type == typeof(bool))
            {
                if (value is int) return (int) value != 0;
                if (value is short) return (int) value != 0;
                if (value is long) return (int) value != 0;
            }

            if (Type == typeof(int)) return Convert.ToInt16(value);
            if (Type == typeof(short)) return Convert.ToInt16(value);
            if (Type == typeof(long)) return Convert.ToInt16(value);

            if (Type.IsEnum) return Enum.Parse(ColumnType, value.ToString());
            return value;
        }
    }
}