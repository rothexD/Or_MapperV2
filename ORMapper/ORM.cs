using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using Npgsql;
using ORMapper.extentions;
using ORMapper.Models;

namespace ORMapper
{
    public static class Orm
    {
        private static readonly Dictionary<Type, Table> _mappedEntities = new();
        public static string Connectionstring = "";

        /// <summary>
        ///     creates a new connection
        /// </summary>
        /// <returns>returns a new connection</returns>
        public static NpgsqlConnection Connection()
        {
            return new NpgsqlConnection(Connectionstring);
        }

        /// <summary>
        ///     gets the table Entity for a given object by referencing its Type
        ///     (Class)._GetTable = Enitiy of class
        ///     Internal Storage in a Dictionary (Type,Table)
        ///     if Enitity does not exist in dictionary, create a new one and add to dictionary
        /// </summary>
        /// <param name="o">object whos Enitity should be returned</param>
        /// <returns>a mapped Entity</returns>
        internal static Table _GetTable(this object o)
        {
            var t = o is Type ? (Type) o : o.GetType();

            if (!_mappedEntities.ContainsKey(t)) _mappedEntities.Add(t, new Table(t));
            return _mappedEntities[t];
        }

        /// <summary>
        ///     searches in a localcache for a object that matches it
        /// </summary>
        /// <param name="findObjectOfType">tries to find a object of this type</param>
        /// <param name="pk">the primary key that identifies the object</param>
        /// <param name="localcache">a localcache</param>
        /// <returns>null if not in cache, object if in cache</returns>
        public static object SearchInCache(Type findObjectOfType, object pk, ICollection<object> localcache)
        {
            if (localcache == null) return null;
            foreach (var i in localcache)
            {
                if (i.GetType() != findObjectOfType) continue;

                if (findObjectOfType._GetTable().PrimaryKey.GetValue(i).Equals(pk)) return i;
            }

            return null;
        }

        /// <summary>
        ///     Creates a table object that represents an entry
        /// </summary>
        /// <param name="t">create a table object which should be created</param>
        /// <param name="reader">A Datareader with data to fill the objet</param>
        /// <param name="localcache"></param>
        /// <param name="shouldMethodUseCaching">should this method use caching, by default true</param>
        /// <returns>a column object</returns>
        internal static object _CreateObject(Type t, IDataReader reader, ICollection<object> localcache,
            bool shouldMethodUseCaching = true)
        {
            var returnObject = shouldMethodUseCaching
                ? SearchInCache(t,
                    t._GetTable().PrimaryKey
                        .ToFieldType(reader.GetValue(reader.GetOrdinal(t._GetTable().PrimaryKey.ColumnName)),
                            localcache),
                    localcache)
                : null;

            if (returnObject == null)
            {
                if (localcache == null) localcache = new List<object>();
                localcache.Add(returnObject = Activator.CreateInstance(t));
            }

            foreach (var i in t._GetTable().Internals)
                i.SetValue(returnObject, i.ToFieldType(reader.GetValue(reader.GetOrdinal(i.ColumnName)), localcache));


            foreach (var i in t._GetTable().Externals)
            {
                var list = Activator.CreateInstance(i.Type) as IList;
                if (i.IsExternal && !i.IsManyToMany)
                {
                    var value = _CreateObjectAll(i.Type.GetGenericArguments()[0], localcache,
                        i.Table.PrimaryKey.GetValue(returnObject), i.ColumnName) as IList;
                    i.SetValue(returnObject, value);
                }

                if (i.IsExternal && i.IsManyToMany)
                {
                    var referencesFromNtoMTable = _CreateObjectAll(i.RemoteTable, localcache,
                        i.Table.PrimaryKey.GetValue(returnObject), i.ColumnName, false);

                    var returnList = Activator.CreateInstance(i.ColumnType) as IList;

                    foreach (var o in referencesFromNtoMTable as IList)
                    {
                        var key = o._GetTable().Columns.First(x =>
                            x.ColumnName.ToLower() == i.TheirReferenceToThisColumnName.ToLower());
                        var pk = key.GetValue(o);
                        returnList.Add(_CreateObject(i.ColumnType.GetGenericArguments()[0], pk, localcache));
                    }

                    i.SetValue(returnObject, returnList);
                }
            }

            return returnObject;
        }

        /// <summary>
        ///     creates a table entry object for a given primary key
        /// </summary>
        /// <param name="t">creates table of type</param>
        /// <param name="pk"></param>
        /// <param name="localcache"></param>
        /// <param name="shouldMethodUseCaching">should this method use caching</param>
        /// <returns>returns a table entry</returns>
        internal static object _CreateObject(Type t, object pk, ICollection<object> localcache,
            bool shouldMethodUseCaching = true)
        {
            var returnObject = shouldMethodUseCaching ? SearchInCache(t, pk, localcache) : null;
            if (returnObject != null) return returnObject;

            var con = Connection();
            con.CustomOpen();
            var command = con.CreateCommand();

            command.CommandText = t._GetTable().GetSelectSql(null) + " Where " + t._GetTable().PrimaryKey.ColumnName +
                                  " = :pk";

            command.Help(":pk", pk);

            var reader = command.ExecuteReader();

            if (reader.Read()) returnObject = _CreateObject(t, reader, localcache);
            reader.Close();
            reader.Dispose();
            command.Dispose();

            //if (returnObject == null) throw new Exception("no data.");

            con.CloseCustom();
            return returnObject;
        }

        /// <summary>
        ///     gets all objects that match a primary key
        /// </summary>
        /// <param name="t">which type of table should be created</param>
        /// <param name="localcache"></param>
        /// <param name="pk">primary key</param>
        /// <param name="foreignKeyTablename"></param>
        /// <param name="shouldMethodUseCaching"></param>
        /// <returns>All table entries that match the pk</returns>
        private static object _CreateObjectAll(Type t, ICollection<object> localcache, object pk,
            string foreignKeyTablename, bool shouldMethodUseCaching = true)
        {
            var con = Connection();
            con.CustomOpen();
            var command = con.CreateCommand();

            command.CommandText = pk == null
                ? t._GetTable().GetSelectSql(null)
                : t._GetTable().GetSelectSql(null) + " Where " + foreignKeyTablename + " = :pk";


            command.Help(":pk", pk);
            var reader = command.ExecuteReader();

            var listType = typeof(List<>);
            var constructedListType = listType.MakeGenericType(t);
            var objectlist = Activator.CreateInstance(constructedListType) as IList;


            while (reader.Read()) objectlist.Add(_CreateObject(t, reader, localcache, shouldMethodUseCaching));


            reader.Close();
            reader.Dispose();
            command.Dispose();
            con.CloseCustom();
            return objectlist;
        }

        /// <summary>
        ///     public interface for save to db function
        /// </summary>
        /// <param name="o">object which should be stored to db</param>
        public static void Save(object o)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    if (o is IEnumerable)
                        foreach (var x in o as IEnumerable)
                            SaveInternal(x, new List<object>());
                    else
                        SaveInternal(o, new List<object>());

                    scope.Complete();
                }
            }
            catch (TransactionAbortedException e)
            {
                Console.WriteLine("Transaction aborted in Get: " + e.Message);
                throw;
            }
        }

        /// <summary>
        ///     saves to object todaterbase
        /// </summary>
        /// <param name="o">which object(table entry)</param>
        /// <param name="localcache"></param>
        /// <param name="caching">should caching be used by default true (unused)</param>
        internal static void SaveInternal(object o, List<object> localcache, bool caching = true)
        {
            var ent = o._GetTable();
            var first = true;
            if (o == null) return;

            if (caching && SearchInCache(o.GetType(), o._GetTable().PrimaryKey.GetValue(o), localcache) != null) return;
            if (caching) localcache.Add(o);

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
                    if (caching) localcache.Add(ent.Internals[i].GetValue(o));
                }


                command.CommandText += ent.Internals[i].ColumnName;
                insert += ":v" + i;

                command.Help(":v" + i,
                    ent.Internals?[i].ToColumnType(ent.Internals?[i].GetValue(o), localcache));

                if (!ent.Internals[i].IsPrimaryKey)
                {
                    if (first)
                        first = false;
                    else
                        update += ", ";

                    update += ent.Internals[i].ColumnName + " = :w" + i;

                    command.Help(":w" + i, ent.Internals[i].ToColumnType(ent.Internals[i].GetValue(o), localcache));
                }
            }

            command.CommandText += ") Values (" + insert + ")" + update;
            con.CustomOpen();
            command.ExecuteNonQuery();
            con.CloseCustom();
            command.Dispose();
            

            for (var i = 0; i < ent.Externals.Length; i++)
            {
                if (ent.Externals[i].IsManyToMany)
                {
                    foreach (var x in ent.Externals[i].GetValue(o) as IEnumerable)
                    {
                        SaveInternal(x, localcache);
                        /*if (SearchInCache(x.GetType(), x._GetTable().PrimaryKey.GetValue(x), localcache) != null)
                        {
                            continue;
                        }*/
                        
                        var con2 = Connection();
                        command = con2.CreateCommand();
                        command.CommandText = "insert into "
                                              + ent.Externals[i].RemoteTable._GetTable().TableName
                                              + "("+ent.Externals[i].TheirReferenceToThisColumnName + "," +
                                              ent.Externals[i].ColumnName + ") values ("
                                              + ":para1,:para2) ON CONFLICT ("+ent.Externals[i].TheirReferenceToThisColumnName+","+ent.Externals[i].ColumnName +") do nothing;";
                        var z = x._GetTable().PrimaryKey.GetValue(x);
                        command.Help(":para1",x._GetTable().PrimaryKey.GetValue(x));
                        var y = ent.Externals[i].Table;
                        var y2 = y.PrimaryKey;
                        var y3 = y2.GetValue(o);
                        
                        command.Help(":para2",y3);
                        con2.Open();
                        command.ExecuteNonQuery();
                        con2.Close();
                        con2.Dispose();
                        
                    }
                }
                else
                {
                    foreach (var x in ent.Externals[i].GetValue(o) as IEnumerable)
                    {
                        SaveInternal(x, localcache);
                        localcache.Add(x);
                    }
                }
            }
        }

        /// <summary>
        ///     public interface for get (gets a single table entry where pk == pk)
        /// </summary>
        /// <param name="pk">primary key</param>
        /// <typeparam name="T">tabl</typeparam>
        /// <returns></returns>
        public static T Get<T>(object pk)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    var returnVal = (T) _CreateObject(typeof(T), pk, new List<object>());
                    scope.Complete();
                    return returnVal;
                }
            }
            catch (TransactionAbortedException e)
            {
                Console.WriteLine("Transaction aborted in Get: " + e.Message);
                throw;
            }
        }

        /// <summary>
        ///     public interface for GetAll (get all table entries where pk == pk)
        /// </summary>
        /// <param name="pk"></param>
        /// <typeparam name="T">table</typeparam>
        /// <returns></returns>
        public static List<T> GetAll<T>(object pk = null)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    var returnVal = (List<T>) _CreateObjectAll(typeof(T), new List<object>(), pk,
                        typeof(T)._GetTable().PrimaryKey.ColumnName);
                    scope.Complete();
                    return returnVal;
                }
            }
            catch (TransactionAbortedException e)
            {
                Console.WriteLine("Transaction aborted in GetAll: " + e.Message);
                throw;
            }
        }


        /// <summary>
        ///     delete a single table entry from db
        /// </summary>
        /// <param name="pk">primary key</param>
        /// <typeparam name="t">table</typeparam>
        internal static void Delete(Type t, object pk)
        {
            var con = Connection();
            con.CustomOpen();
            var command = con.CreateCommand();


            command.CommandText =
                $"Delete from {t._GetTable().TableName} where {t._GetTable().PrimaryKey.ColumnName} = :pk";

            command.Help(":pk", pk);

            command.ExecuteNonQuery();
            command.Dispose();
            con.CloseCustom();
        }


        /// <summary>
        ///     calls internal delete and defines transaction
        /// </summary>
        /// <param name="pk">primary key</param>
        /// <typeparam name="T">table</typeparam>
        public static void Delete<T>(object pk)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    Delete(typeof(T), pk);
                    scope.Complete();
                }
            }
            catch (TransactionAbortedException e)
            {
                Console.WriteLine("Transaction aborted in Delete: " + e.Message);
                throw;
            }
        }
    }
}