using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using Npgsql;
using ORMapper.Caches;
using OrMapper.Helpers.extentions;
using ORMapper.Models;

namespace ORMapper
{
    public static class Orm
    {
        private static readonly Dictionary<Type, Table> _mappedEntities = new();
        public static string ConnectionString = "";

        private static readonly TrackingCache _internalCache = new();

        /// <summary>
        ///     creates a new connection
        /// </summary>
        /// <returns>returns a new connection</returns>
        public static NpgsqlConnection Connection()
        {
            return new NpgsqlConnection(ConnectionString);
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
                    //if object is of type IEnumeable, call save for each object
                    if (o is IEnumerable)
                        foreach (var x in o as IEnumerable)
                            _SaveInternal(x, new List<object>());
                    else
                        _SaveInternal(o, new List<object>());

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
                    var returnVal = (T) _CreateSingleObject(typeof(T), pk, new List<object>());
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
                    _Delete(typeof(T), pk);
                    scope.Complete();
                }
            }
            catch (TransactionAbortedException e)
            {
                Console.WriteLine("Transaction aborted in Delete: " + e.Message);
                throw;
            }
        }
        
        /// <summary>
        ///     searches in a localcache for a object that matches it
        /// </summary>
        /// <param name="findObjectOfType">tries to find a object of this type</param>
        /// <param name="pk">the primary key that identifies the object</param>
        /// <param name="localCache">a localcache to track objects</param>
        /// <returns>null if not in cache, object if in cache</returns>
        public static object SearchInCache(Type findObjectOfType, object pk, ICollection<object> localCache)
        {
            if (localCache != null)
                foreach (var i in localCache)
                {
                    if (i.GetType() != findObjectOfType) continue;

                    if (findObjectOfType._GetTable().PrimaryKey.GetValue(i).Equals(pk))
                    {
                        return i;
                    }
                }
            return null;
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
        ///     Creates a table object that represents an entry
        /// </summary>
        /// <param name="t">create a table object which should be created</param>
        /// <param name="reader">A Datareader with data to fill the objet</param>
        /// <param name="localCache"></param>
        /// <returns>a column object</returns>
        internal static object _CreateObject(Type t, IDataReader reader, ICollection<object> localCache)
        {
            // look for object in localcache
            var returnObject = SearchInCache(t,
                t._GetTable().PrimaryKey
                    .ToFieldType(reader.GetValue(reader.GetOrdinal(t._GetTable().PrimaryKey.ColumnName)),
                        localCache),
                localCache);
            //look for object in internalCache
            if(_internalCache.Contains(t, t._GetTable().PrimaryKey.ToFieldType(reader.GetValue(reader.GetOrdinal(t._GetTable().PrimaryKey.ColumnName)),
                        localCache)))
            {
                returnObject = _internalCache.Get(t, t._GetTable().PrimaryKey
                    .ToFieldType(reader.GetValue(reader.GetOrdinal(t._GetTable().PrimaryKey.ColumnName)), localCache));
                return returnObject;
            }
            if (returnObject == null)
            {
                // create new instance of object
                if (localCache == null) localCache = new List<object>();
                localCache.Add(returnObject = Activator.CreateInstance(t));
                
                //fill internal columns/parameters with values from reader
                //map reader.column to ordinal of column name
                foreach (var i in t._GetTable().Internals)
                {
                    i.SetValue(returnObject,
                        i.ToFieldType(reader.GetValue(reader.GetOrdinal(i.ColumnName)), localCache));
                    localCache.Add(returnObject);
                }

                //fill external fields
                foreach (var i in t._GetTable().Externals)
                {
                    // fills 1:n relations
                    if (i.IsExternal && !i.IsManyToMany)
                    {
                        //get all objects of type and pk
                        var value = (IList) _CreateObjectAll(i.Type.GetGenericArguments()[0], localCache,
                            i.Table.PrimaryKey.GetValue(returnObject), i.ColumnName);
                        i.SetValue(returnObject, value);
                    }
                    //fill n:m relations
                    if (i.IsExternal && i.IsManyToMany)
                    {
                        // get n:m reference table objects
                        var referencesFromNtoMTable = (IList) _CreateObjectAll(i.RemoteTable, localCache,
                            i.Table.PrimaryKey.GetValue(returnObject), i.ColumnName);

                        var returnList = (IList) Activator.CreateInstance(i.ColumnType);
                        
                        //foreach reference object generate the corresponding object 
                        foreach (var o in referencesFromNtoMTable)
                        {
                            var key = o._GetTable().Columns.First(x =>
                                x.ColumnName.ToLower() == i.TheirReferenceToThisColumnName.ToLower());
                            var pk = key.GetValue(o);

                            var obj = _CreateSingleObject(i.ColumnType.GetGenericArguments()[0], pk, localCache);
                            localCache.Add(obj);
                            _internalCache.Add(obj);
                            returnList.Add(obj);
                        }
                        //internalCache.Add(returnList);
                        i.SetValue(returnObject, returnList);
                    }
                }
            }
            _internalCache.Add(returnObject);
            return returnObject;
        }

        /// <summary>
        ///     creates a table entry object for a given primary key
        /// </summary>
        /// <param name="t">creates table of type</param>
        /// <param name="pk"></param>
        /// <param name="localCache"></param>
        /// <returns>returns a table entry</returns>
        internal static object _CreateSingleObject(Type t, object pk, ICollection<object> localCache)
        {

            //check if in cache if so return that object
            var returnObject = SearchInCache(t, pk, localCache);
            if (_internalCache.Contains(t, pk)) returnObject = _internalCache.Get(t, pk);
            if (returnObject != null)
            {
                return returnObject;
            }

            var con = Connection();
            DbExtentions.Open(con);
            var command = con.CreateCommand();

            command.CommandText = t._GetTable().GetSelectSql(null) + " Where " + t._GetTable().PrimaryKey.ColumnName +
                                  " = :pk";

            command.HelpWithParameter(":pk", pk);

            var reader = DbExtentions.ExecuteReader(command);
           
            // create object
            if (reader.Read()) returnObject = _CreateObject(t, reader, localCache);
            
            reader.Close();
            reader.Dispose();
            command.Dispose();

            //if (returnObject == null) throw new Exception("no data.");

            DbExtentions.Close(con);
            return returnObject;
        }

        /// <summary>
        ///     gets all objects that match a primary key
        /// </summary>
        /// <param name="t">which type of table should be created</param>
        /// <param name="localCache"></param>
        /// <param name="pk">primary key</param>
        /// <param name="foreignKeyTablename"></param>
        /// <returns>All table entries that match the pk</returns>
        internal static object _CreateObjectAll(Type t, ICollection<object> localCache, object pk,
            string foreignKeyTablename)
        {
            var con = Connection();
            DbExtentions.Open(con);
            var command = con.CreateCommand();

            command.CommandText = pk == null
                ? t._GetTable().GetSelectSql(null)
                : t._GetTable().GetSelectSql(null) + " Where " + foreignKeyTablename + " = :pk";

            

            
            
            command.HelpWithParameter(":pk", pk);
            var reader = DbExtentions.ExecuteReader(command);

            //create list for Type
            var listType = typeof(List<>);
            var constructedListType = listType.MakeGenericType(t);
            var objectlist = (IList) Activator.CreateInstance(constructedListType);
            
            // fill list with objects
            while (reader.Read())
            {
                var createdObj = _CreateObject(t, reader, localCache);
                _internalCache.Add(createdObj);
                objectlist.Add(createdObj);
            }


            reader.Close();
            reader.Dispose();
            command.Dispose();
            DbExtentions.Close(con);
            return objectlist;
        }

        internal static object _CreateObjectAll(Type t, ICollection<object> localCache, (string,List<(string, object)>) first)
        {
            var con = Connection();
            DbExtentions.Open(con);
            var command = con.CreateCommand();

            command.CommandText = t._GetTable().GetSelectSql(null) + first.Item1;

            foreach (var item in first.Item2)
            {
                command.HelpWithParameter(item.Item1,item.Item2);
            }
            
            var reader = DbExtentions.ExecuteReader(command);

            //create list for Type
            var listType = typeof(List<>);
            var constructedListType = listType.MakeGenericType(t);
            var objectlist = (IList) Activator.CreateInstance(constructedListType);
            
            // fill list with objects
            while (reader.Read())
            {
                var createdObj = _CreateObject(t, reader, localCache);
                _internalCache.Add(createdObj);
                objectlist.Add(createdObj);
            }


            reader.Close();
            reader.Dispose();
            command.Dispose();
            DbExtentions.Close(con);
            return objectlist;
        }

        /// <summary>
        ///     saves or updates an object to datebase
        /// </summary>
        /// <param name="o">which object(table entry)</param>
        /// <param name="localCache"></param>
        internal static void _SaveInternal(object o, List<object> localCache)
        {
            //Todo: make easier to understand, refactor 
            
            var ent = o._GetTable();
            var first = true;
            if (o == null) return;
            
            //try to find in if in localcache, if in cache already saved
            if (SearchInCache(o.GetType(), o._GetTable().PrimaryKey.GetValue(o), localCache) != null)
            {
                return;
            }
            localCache.Add(o);
            //if object has changed, save internals
            if (_internalCache.HasChanged(o))
            {
                _internalCache.Add(o);
                //save object internals
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
                    
                    //if internal but foreign call save with object and then continue
                    if (ent.Internals[i].IsForeignKey)
                    {
                        _SaveInternal(ent.Internals[i].GetValue(o), localCache);
                        localCache.Add(ent.Internals[i].GetValue(o));
                    }

                    
                    command.CommandText += ent.Internals[i].ColumnName;
                    insert += ":v" + i;

                    command.HelpWithParameter(":v" + i,
                        ent.Internals?[i].ToColumnType(ent.Internals?[i].GetValue(o), localCache));

                    if (!ent.Internals[i].IsPrimaryKey)
                    {
                        if (first)
                            first = false;
                        else
                            update += ", ";

                        update += ent.Internals[i].ColumnName + " = :w" + i;

                        command.HelpWithParameter(":w" + i, ent.Internals[i].ToColumnType(ent.Internals[i].GetValue(o), localCache));
                    }
                }

                command.CommandText += ") Values (" + insert + ")" + update;
                DbExtentions.Open(con);
                DbExtentions.ExecuteNonQuery(command);
                DbExtentions.Close(con);
                command.Dispose();
            }
            for (var i = 0; i < ent.Externals.Length; i++)
                // save n:m relations
                if (ent.Externals[i].IsManyToMany)
                    foreach (var x in ent.Externals[i].GetValue(o) as IEnumerable)
                    {
                        //if (!internalCache.HasChanged(x)) continue;
                        // save n:m object
                        _SaveInternal(x, localCache);
                        
                        // if already in cache ignore already saved
                        if (SearchInCache(x.GetType(), x._GetTable().PrimaryKey.GetValue(x), localCache) != null)
                            continue;

                        // update n:m reference in middleman table
                        var con2 = Connection();
                        var command = con2.CreateCommand();
                        command.CommandText = "insert into "
                                              + ent.Externals[i].RemoteTable._GetTable().TableName
                                              + "(" + ent.Externals[i].TheirReferenceToThisColumnName + "," +
                                              ent.Externals[i].ColumnName + ") values ("
                                              + ":para1,:para2) ON CONFLICT (" +
                                              ent.Externals[i].TheirReferenceToThisColumnName + "," +
                                              ent.Externals[i].ColumnName + ") do nothing;";
                        
                        command.HelpWithParameter(":para1", x._GetTable().PrimaryKey.GetValue(x));
                        var y = ent.Externals[i].Table;
                        var y2 = y.PrimaryKey;
                        var y3 = y2.GetValue(o);

                        command.HelpWithParameter(":para2", y3);
                        con2.Open();
                        command.ExecuteNonQuery();
                        con2.Close();
                        con2.Dispose();
                    }
                else
                    // update 1:n relations, recurisvly save
                    foreach (var x in ent.Externals[i].GetValue(o) as IEnumerable)
                    {
                        //if (!internalCache.HasChanged(x)) continue;
                        _SaveInternal(x, localCache);
                    }
        }


        /// <summary>
        ///     delete a single table entry from db
        /// </summary>
        /// <param name="pk">primary key</param>
        /// <param name="t">table</param>
        internal static void _Delete(Type t, object pk)
        {
            var con = Connection();
            DbExtentions.Open(con);
            var command = con.CreateCommand();
            _internalCache.Remove(t,pk);

            command.CommandText =
                $"Delete from {t._GetTable().TableName} where {t._GetTable().PrimaryKey.ColumnName} = :pk";

            command.HelpWithParameter(":pk", pk);

            DbExtentions.ExecuteNonQuery(command);
            command.Dispose();
            DbExtentions.Close(con);
        }
    }
}