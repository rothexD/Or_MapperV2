using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using ORMapper.FluentQuery;
using ORMapper.Models;

namespace ORMapper
{
    public static class Orm
    {
        private static readonly Dictionary<Type, Table> _mappedEntities = new();
        public static string Connectionstring = "";

        public static IDbConnection Connection()
        {
            return new NpgsqlConnection(Connectionstring);
            ;
        }

        internal static Table _GetEntity(this object o)
        {
            var t = o is Type ? (Type) o : o.GetType();

            if (!_mappedEntities.ContainsKey(t)) _mappedEntities.Add(t, new Table(t));
            return _mappedEntities[t];
        }

        internal static object Searchcache(Type t, object pk, ICollection<object> localcache)
        {
            if (localcache == null) return null;
            foreach (var i in localcache)
            {
                if (i.GetType() != t) continue;

                if (t._GetEntity().PrimaryKey.GetValue(i).Equals(pk)) return i;
            }

            return null;
        }


        internal static object _CreateObject(Type t, IDataReader reader, ICollection<object> localcache)
        {
            var returnObject = Searchcache(t,
                t._GetEntity().PrimaryKey
                    .ToFieldType(reader.GetValue(reader.GetOrdinal(t._GetEntity().PrimaryKey.ColumnName)), localcache),
                localcache);

            if (returnObject == null)
            {
                if (localcache == null) localcache = new List<object>();
                localcache.Add(returnObject = Activator.CreateInstance(t));
            }

            foreach (var i in t._GetEntity().Internals)
                i.SetValue(returnObject, i.ToFieldType(reader.GetValue(reader.GetOrdinal(i.ColumnName)), localcache));

            foreach (var i in t._GetEntity().Externals)
            {
                var list = Activator.CreateInstance(i.Type) as IList;
                if (i.IsExternal)
                {
                    var value = _CreateObjectAll(i.Type.GetGenericArguments()[0], localcache, i.Table.PrimaryKey.GetValue(returnObject),i.ColumnName) as IList;
                    i.SetValue(returnObject, value);
                }
            }

            return returnObject;
        }

        internal static object _CreateObject(Type t, object pk, ICollection<object> localcache)
        {
            var returnObject = Searchcache(t, pk, localcache);
            if (returnObject != null) return returnObject;

            var con = Connection();
            con.CustomOpen();
            var command = con.CreateCommand();

            command.CommandText = t._GetEntity().GetSelectSQL(null) + " Where " + t._GetEntity().PrimaryKey.ColumnName +
                                  " = :pk";

            Parameterhelper.ParaHelp(":pk", pk, command);

            var reader = command.ExecuteReader();

            if (reader.Read()) returnObject = _CreateObject(t, reader, localcache);
            reader.Close();
            reader.Dispose();
            command.Dispose();

            if (returnObject == null) throw new Exception("no data.");

            con.CloseCustom();
            return returnObject;
        }

        private static object _CreateObjectAll(Type t, ICollection<object> localcache, object pk,string foreignkeytablename)
        {
            var con = Connection();
            con.CustomOpen();
            var command = con.CreateCommand();

            Console.WriteLine(pk);
            command.CommandText = pk == null
                ? t._GetEntity().GetSelectSQL(null)
                : t._GetEntity().GetSelectSQL(null) + " Where " + foreignkeytablename + " = :pk";
            
            Console.WriteLine(command.CommandText);

            Parameterhelper.ParaHelp(":pk", pk, command);
            var reader = command.ExecuteReader();

            var listType = typeof(List<>);
            var constructedListType = listType.MakeGenericType(t);

            var objectlist = Activator.CreateInstance(constructedListType) as IList;


            while (reader.Read()) objectlist.Add(_CreateObject(t, reader, localcache));
            
            
            reader.Close();
            reader.Dispose();
            command.Dispose();
            con.CloseCustom();
            return objectlist;
        }

        public static void Save(object o)
        {
            if (o is IEnumerable)
            {
                foreach (var x in o as IEnumerable)
                {
                    SaveInternal(x,new List<object>());
                }
            }
            else
            {
                SaveInternal(o, new List<object>());
            }
        }

        internal static void SaveInternal(object o, List<object> localcache)
        {
            var ent = o._GetEntity();
            var first = true;
            if (o == null) return;

            if (Searchcache(o.GetType(), o._GetEntity().PrimaryKey.GetValue(o), localcache) != null) return;
            localcache.Add(o);

            var con = Connection();

            var command = con.CreateCommand();
            command.CommandText = $"Insert into {ent.TableName} (";
            var update = " ON CONFLICT (" + ent.PrimaryKey.ColumnName + ") DO UPDATE SET ";
            var insert = "";


            for (var i = 0; i < ent.Internals.Length; i++)
            {
                if (i > 0)
                {
                    command.CommandText += ", ";
                    insert += ", ";
                }

                if (ent.Internals[i].IsForeignKey)
                {
                    SaveInternal(ent.Internals[i].GetValue(o), localcache);
                    localcache.Add(ent.Internals[i].GetValue(o));
                }


                command.CommandText += ent.Internals[i].ColumnName;
                insert += ":v" + i;

                Parameterhelper.ParaHelp(":v" + i,
                    ent.Internals?[i].ToColumnType(ent.Internals?[i].GetValue(o), localcache), command);

                if (!ent.Internals[i].IsPrimaryKey)
                {
                    if (first)
                        first = false;
                    else
                        update += ", ";

                    update += ent.Internals[i].ColumnName + " = :w" + i;

                    Parameterhelper.ParaHelp(":w" + i,
                        ent.Internals[i].ToColumnType(ent.Internals[i].GetValue(o), localcache), command);
                }
            }

            command.CommandText += ") Values (" + insert + ")" + update;
            con.CustomOpen();
            command.ExecuteNonQuery();
            command.Dispose();
            con.CloseCustom();

            for (var i = 0; i < ent.Externals.Length; i++)
                foreach (var x in ent.Externals[i].GetValue(o) as IEnumerable)
                {
                    SaveInternal(x, localcache);
                    localcache.Add(x);
                }
        }


        public static T Get<T>(object pk)
        {
            return (T) _CreateObject(typeof(T), pk, new List<object>());
        }

        public static List<T> GetAll<T>(object pk = null)
        {
            return (List<T>) _CreateObjectAll(typeof(T), new List<object>(), pk,typeof(T)._GetEntity().PrimaryKey.ColumnName);
        }

        public static void Delete<T>(object pk)
        {
            var con = Connection();
            con.CustomOpen();
            var command = con.CreateCommand();


            command.CommandText =
                $"Delete from {typeof(T)._GetEntity().TableName} where {typeof(T)._GetEntity().PrimaryKey.ColumnName} = :pk";

            Parameterhelper.ParaHelp(":pk", pk, command);

            command.ExecuteNonQuery();
            command.Dispose();
            con.CloseCustom();
        }
    }
}