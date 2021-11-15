using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text;

namespace ORMapper.Caches
{
    public class TrackingCache : Cache
    {
        //Todo: add logging 
        //Todo: add code comments

        public Dictionary<Type, Dictionary<object, string>> _hashdictionary = new();
        
        /// <summary>
        /// gets or creates a new Dictionary for this hash type
        /// </summary>
        /// <param name="t">type index key</param>
        /// <returns>hash dictionary for specific type</returns>
        private Dictionary<object, string> GetHashDictionary(Type t)
        {
            if (_hashdictionary.TryGetValue(t, out var value)) return value;

            return _hashdictionary[t] = new Dictionary<object, string>();
        }
        /// <summary>
        /// adds or updates a hash to the dictionary and calls base method Add with the object
        /// </summary>
        /// <param name="obj">object which should be added</param>
        public override void Add(object obj)
        {
            var innnerDictionary = GetHashDictionary(obj.GetType());
            innnerDictionary[obj._GetTable().PrimaryKey.GetValue(obj)] = CalculateHash(obj);
            base.Add(obj);
        }
        /// <summary>
        /// removes a hash from the dictionary and calls base method Remove with object
        /// </summary>
        /// <param name="obj">object which should be removed</param>
        public override void Remove(object obj)
        {
            var innnerDictionary = GetHashDictionary(obj.GetType());
            innnerDictionary.Remove(obj._GetTable().PrimaryKey.GetValue(obj));
            base.Add(obj);
        }
        /// <summary>
        /// removes an hash of type and primary key, calls base method remove wit htype and pk
        /// </summary>
        /// <param name="t">Type of object</param>
        /// <param name="pk">Primary key of object</param>
        public override void Remove(Type t, object pk)
        {
            GetHashDictionary(t).Remove(pk);
            base.Remove(t, pk);
        }

        /// <summary>
        /// recursive calls internally
        ///
        /// calculates the hash for an object, using an localcache to track already hashed objects.
        /// </summary>
        /// <param name="obj">object that should be hashed</param>
        /// <param name="localcache">stores already hashed objects</param>
        /// <returns>hash for an object</returns>
        public static string CalculateHash(object obj, ICollection<object> localcache = null)
        {
            if (obj is null) return null;
            if (localcache is null) localcache = new Collection<object>();
            var rval = "";
            
            //hash internal values
            foreach (var i in obj._GetTable().Internals)
                // hash object types
                if (i.IsForeignKey)
                {
                    var m = i.GetValue(obj);
                    if (m != null)
                    {
                        // if not already hashed and in localcache, recursivly call hash function again but this time as object parameter,
                        // turning external object to internal object for function
                        if (Orm.SearchInCache(m.GetType(), m._GetTable().PrimaryKey.GetValue(m), localcache) !=
                            null) continue;
                        localcache.Add(m);
                        rval += CalculateHash(m, localcache);
                    }
                }
                //hash default c# types
                else
                {
                    rval += i.ColumnName + "=" + i.GetValue(obj) + ";";
                }
            //hash externals
            foreach (var i in obj._GetTable().Externals)
            {
                var m = (IEnumerable) i.GetValue(obj);

                if (m != null)
                {
                    rval += i.ColumnName + "=";
                    // For each external object in the list
                    // if not already hashed and in localcache, recursivly call hash function again but this time as object parameter,
                    // turning external object to internal object for function
                    foreach (var k in m)
                    {
                        if (Orm.SearchInCache(k.GetType(), k._GetTable().PrimaryKey.GetValue(k), localcache) !=
                            null) continue;
                        localcache.Add(k);
                        rval += CalculateHash(k, localcache);
                    }
                }
            }

            //Console.WriteLine(rval);
            return Encoding.UTF8.GetString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(rval)));
        }
        /// <summary>
        /// Compares an object to a stored object, if hash is the same, no changes, if hash does not exist create hash and is new
        /// If object changed, return true
        /// </summary>
        /// <param name="obj">object which hash should be compared to stored hash</param>
        /// <returns>true if object is different to stored hash, false otherwise</returns>
        /// <exception cref="Exception">null object given to function</exception>
        public override bool HasChanged(object obj)
        {
            if (obj is null) throw new Exception("null given to has changed");
            var parameterObjectHash = CalculateHash(obj, new Collection<object>());

            var storedHash = GetHashFromStorageObject(obj);
            if (storedHash is null) return true;

            return parameterObjectHash != storedHash;
        }
        /// <summary>
        /// gets an hash for from storage
        /// </summary>
        /// <param name="obj">which hash should be gotten</param>
        /// <returns>hash if in storage, null if not in storage</returns>
        private string GetHashFromStorageObject(object obj)
        {
            var innnerDictionary = GetHashDictionary(obj.GetType());
            if (innnerDictionary.TryGetValue(obj._GetTable().PrimaryKey.GetValue(obj), out var result)) return result;

            return null;
        }
    }
}