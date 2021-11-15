using System;
using System.Collections.Generic;
using System.Reflection;

namespace ORMapper.Models
{
    public class Column
    {
        //Todo: add logging 
        //Todo: add code comments
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entity">Set columntable to the table that created it in parsing</param>
        public Column(Table entity)
        {
            Table = entity;
        }
        /// <summary>
        /// in what table does the column exist
        /// </summary>
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
        /// <summary>
        /// name of the column in the database
        /// </summary>
        public string ColumnName { get; internal set; }
        /// <summary>
        /// Type of the columnvalue
        /// </summary>
        public Type ColumnType { get; internal set; }

        public bool IsPrimaryKey { get; internal set; }
        public bool IsForeignKey { get; internal set; }
        public bool IsNullable { get; internal set; }

        public bool IsExternal { get; internal set; } = false;

        public Type RemoteTable { get; internal set; } = null;

        /// <summary>
        /// Columname that is referenced by them for n:m
        /// </summary>
        public string TheirReferenceToThisColumnName { get; internal set; } = null;

        public bool IsManyToMany { get; internal set; } = false;

        /// <summary>
        /// Gets the value for an object
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public object GetValue(object obj)
        {
            if (Member is PropertyInfo)
            {
               var z= ((PropertyInfo) Member).GetValue(obj);
               return z;
            }
            throw new NotSupportedException("Member type not supported");
        }
        /// <summary>
        /// Sets the value for an object
        /// </summary>
        /// <param name="obj">object which value should be set</param>
        /// <param name="value">value which should be set</param>
        /// <exception cref="NotSupportedException"></exception>
        public void SetValue(object obj, object value)
        {
            if (Member is PropertyInfo)
                ((PropertyInfo) Member).SetValue(obj, value);
            else
                throw new NotSupportedException("Member type not supported");
        }
        /// <summary>
        /// transfers a object to the columntype, uses caching by default (orm->db)
        /// </summary>
        /// <param name="value">value to be trasnformed</param>
        /// <param name="localchache">cache in which objects can be stored and searched for</param>
        /// <returns>transformed value </returns>
        public object ToColumnType(object value, ICollection<object> localchache)
        {
            if (IsForeignKey)
            {
                if (value == null) return null;
                var i = value._GetTable().PrimaryKey
                    .ToColumnType(value._GetTable().PrimaryKey.GetValue(value), localchache);
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
        /// <summary>
        /// transfers a object to a different type (db->orm)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="localcache"></param>
        /// <returns>value in new type</returns>
        public object ToFieldType(object value, ICollection<object> localcache) //,ICollection<object> localcache
        {
            if (IsForeignKey) return Orm._CreateSingleObject(Type, value, localcache);

            if (Type == typeof(bool))
            {
                if (value is int) return (int) value != 0;
                if (value is short) return (int) value != 0;
                if (value is long) return (int) value != 0;
            }

            if (Type == typeof(int)) return Convert.ToInt32(value);
            if (Type == typeof(short)) return Convert.ToInt16(value);
            if (Type == typeof(long)) return Convert.ToInt64(value);

            if (Type.IsEnum) return Enum.Parse(ColumnType, value.ToString());
            return value;
        }
    }
}