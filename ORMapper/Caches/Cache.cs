using System;
using System.Collections;
using System.Collections.Generic;

namespace ORMapper.Caches
{
    public class Cache
    {
        public Dictionary<Type,Dictionary<object,object>> storage = new();

        /// <summary>
        /// Gets the specific subdictionary for type
        /// </summary>
        /// <param name="t"></param>
        /// <returns>Dictionary for type</returns>
        protected virtual Dictionary<object, object> GetCache(Type t)
        {
            if(storage.TryGetValue(t,out var value)) { return value; }
            
            return storage[t] = new();
        }
        /// <summary>
        /// gets an object from the cache
        /// </summary>
        /// <param name="t">type of object</param>
        /// <param name="pk">primary key of object</param>
        /// <returns>object or null if not saved</returns>
        public virtual object Get(Type t,object pk)
        {
            return GetCache(t)[pk] ?? null;
        }
        /// <summary>
        /// asks if an object of type and pk is stored
        /// </summary>
        /// <param name="t">type of object</param>
        /// <param name="pk">primary key of object</param>
        /// <returns>true if stored,false if not</returns>
        public virtual bool Contains(Type t,object pk)
        {
            return GetCache(t).ContainsKey(pk);
        }
        /// <summary>
        /// adds or updates a stored object
        /// </summary>
        /// <param name="obj">object which should be stored</param>
        public virtual void Add(object obj)
        {
            if (obj != null)
            {
                var innerCache = GetCache(obj.GetType());
                var key = obj._GetTable().PrimaryKey.GetValue(obj);
                innerCache[key] = obj;
            }
        }
        /// <summary>
        /// removes an object from storage
        /// </summary>
        /// <param name="obj"></param>
        public virtual void Remove(object obj)
        {
            GetCache(obj.GetType()).Remove(obj._GetTable().PrimaryKey.GetValue(obj));
        }
        /// <summary>
        /// removes an object from storage by primary key and type
        /// </summary>
        /// <param name="t">type of object</param>
        /// <param name="pk">primary key of object</param>
        public virtual void Remove(Type t,object pk)
        {
            GetCache(t).Remove(pk);
            
        }
        /// <summary>
        /// asks if an object has changed, base cache cant track changes and always returns true
        /// </summary>
        /// <param name="obj">object which might have changed since storage</param>
        /// <returns>true</returns>
        public virtual bool HasChanged(object obj)
        {
            return true;
        }
    }
}