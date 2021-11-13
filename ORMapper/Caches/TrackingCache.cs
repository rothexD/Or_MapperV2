using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text;
using ORMapper.Models;

namespace ORMapper.Caches
{
    public class TrackingCache : Cache
    {
        public Dictionary<Type, Dictionary<object, string>> _hashdictionary = new();

        private Dictionary<object, string> GetHashDictionary(Type t)
        {
            if(_hashdictionary.TryGetValue(t,out var value)) { return value; }
            
            return _hashdictionary[t] = new();
        }

        public override void Add(object obj)
        {
            var innnerDictionary = GetHashDictionary(obj.GetType());
            innnerDictionary[obj._GetTable().PrimaryKey.GetValue(obj)] = calculateHash(obj);
            base.Add(obj);
        }

        public override void Remove(object obj)
        {
            var innnerDictionary = GetHashDictionary(obj.GetType());
            innnerDictionary.Remove(obj._GetTable().PrimaryKey.GetValue(obj));
            base.Add(obj);
        }
        public override void Remove(Type t,object pk)
        {
            GetHashDictionary(t).Remove(pk);
            base.Remove(t,pk);
        }

        private string GetHashFromStorageObject(object obj)
        {
            var innnerDictionary = GetHashDictionary(obj.GetType());
            if(innnerDictionary.TryGetValue(obj._GetTable().PrimaryKey.GetValue(obj),out var result))
            {
                return result;
            }

            return null;
        }
        public static string calculateHash(object obj,ICollection<object> localcache = null)
        {
            if (obj is null)
            {
                return null;
            }
            if (localcache is null)
            {
                localcache = new Collection<object>();
            }
            string rval = "";
            foreach(Column i in obj._GetTable().Internals) 
            {
                if(i.IsForeignKey)
                {
                    object m = i.GetValue(obj);
                    if (m != null)
                    {
                        if (Orm.SearchInCache(m.GetType(), m._GetTable().PrimaryKey.GetValue(m), localcache) != null)
                        {
                           continue;
                        }
                        localcache.Add(m);
                        rval += calculateHash(m,localcache);
                        
                    }
                }
                else { rval += (i.ColumnName + "=" + i.GetValue(obj).ToString() + ";"); }
            }
            
            foreach(Column i in obj._GetTable().Externals)
            {
                IEnumerable m = (IEnumerable) i.GetValue(obj);

                if(m != null)
                {
                    rval += (i.ColumnName + "=");
                    foreach(object k in m)
                    {
                        if (Orm.SearchInCache(k.GetType(), k._GetTable().PrimaryKey.GetValue(k), localcache) != null)
                        {
                            continue;
                        }
                        localcache.Add(k);
                        rval += calculateHash(k,localcache);
                    }
                }
            }
            //Console.WriteLine(rval);
            return Encoding.UTF8.GetString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(rval)));
        }

        public override bool HasChanged(object obj)
        {
            if (obj is null)
            {
                throw new Exception("null given to has changed");
            }
            var parameterObjectHash = calculateHash(obj,new Collection<object>());
           
            var storedHash = GetHashFromStorageObject(obj);
            if (storedHash is null)
            {
                return true;
            }
            
            return parameterObjectHash != storedHash;
        }
    }
}