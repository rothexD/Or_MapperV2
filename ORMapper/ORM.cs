using System;
using System.Collections.Generic;
using System.Data;
using ORMapper.Models;

namespace ORMapper
{
    public static class Orm
    {
        private static Dictionary<Type, Table> _mappedEntities = new();
        public static IDbConnection Connection;
        internal static Table _GetEntity(this object o)
        {
            Type t = (o is Type) ? (Type) o : o.GetType();

            if (!_mappedEntities.ContainsKey(t))
            {
                _mappedEntities.Add(t, new Table(t));
            }
            return _mappedEntities[t];
        }
        internal static object _CreateObject(Type t, IDataReader reader)
        {
            object returnObject = Activator.CreateInstance(t);

            foreach (Column i in t._GetEntity().Columns)
            {
                i.SetValue(returnObject, i.ToFieldType(reader.GetValue(reader.GetOrdinal(i.ColumnName))));
            }
            return returnObject;
        }

        internal static object _CreateObject(Type t, object pk)
        {
            IDbCommand command = Connection.CreateCommand();

            command.CommandText = t._GetEntity().GetSelectSQL(null) + " Where " + t._GetEntity().PrimaryKey.ColumnName + " = :pk";

            IDataParameter p = command.CreateParameter();
            p.ParameterName = ":pk";
            p.Value = pk;
            command.Parameters.Add(p);

            object returnObject = null;
            IDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                returnObject = _CreateObject(t, reader);
            }
            reader.Close();
            reader.Dispose();
            command.Dispose();

            if (returnObject == null) { throw new Exception("no data."); }

            return returnObject;
        }

        private static object _CreateObjectAll<T>()
        {
            IDbCommand command = Connection.CreateCommand();

            command.CommandText = typeof(T)._GetEntity().GetSelectSQL(null);

            IDataReader reader = command.ExecuteReader();

            var objectlist = new List<T>();
            while (reader.Read())
            {
                objectlist.Add((T)_CreateObject(typeof(T), reader));
            }
            reader.Close();
            reader.Dispose();
            command.Dispose();

            return objectlist;
        }
        

        public static void Save(object o)
        {
            Table ent = o._GetEntity();

            IDbCommand command = Connection.CreateCommand();
            command.CommandText = $"Insert into {ent.TableName} (";
            string update = " ON CONFLICT (" + ent.PrimaryKey.ColumnName + ") DO UPDATE SET ";
            string insert = "";

            IDataParameter p;
            bool first = true;
            for(int i = 0; i < ent.Columns.Length; i++)
            {
                if (i > 0) { command.CommandText += ", "; insert += ", "; }
                command.CommandText += ent.Columns[i].ColumnName;
                insert += (":v" + i.ToString());

                p = command.CreateParameter();
                p.ParameterName = ":v" + i.ToString();
                p.Value = ent.Columns[i].ToColumnType(ent.Columns[i].GetValue(o));
                command.Parameters.Add(p);

                if (!ent.Columns[i].isPrimaryKey)
                {
                    if (first)
                    { 
                        first = false;
                    }
                    else
                    {
                        update += ", ";
                    }
                    update += (ent.Columns[i].ColumnName + " = :w" + i.ToString());

                    p = command.CreateParameter();
                    p.ParameterName = ":w" + i.ToString();
                    p.Value = ent.Columns[i].ToColumnType(ent.Columns[i].GetValue(o));
                    command.Parameters.Add(p);
                }             
            }
            command.CommandText += (") Values (" + insert + ")" + update);
            command.ExecuteNonQuery();
            command.Dispose();
        }
        
        public static T Get<T>(object pk)
        {
            return (T) _CreateObject(typeof(T), pk);
        }
        public static List<T> GetAll<T>()
        {
            return (List<T>) _CreateObjectAll<T>();
        }
        public static void Delete<T>(object pk)
        {
            IDbCommand command = Connection.CreateCommand();

            command.CommandText = $"Delete from {typeof(T)._GetEntity().TableName} where {typeof(T)._GetEntity().PrimaryKey.ColumnName} = :pk";

            IDbDataParameter primaryKey = command.CreateParameter();
            primaryKey.ParameterName = ":pk";
            primaryKey.Value = pk;

            command.Parameters.Add(primaryKey);

            command.ExecuteNonQuery();
            command.Dispose();         
        }
        public static void DbInit()
        {

        }
        
    }
}