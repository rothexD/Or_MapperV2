using System;
using System.Collections;
using System.Collections.Generic;

namespace ORMapper.Caches
{
    public class Cache
    {
        //Todo: add logging 
        //Todo: add code comments
        public Dictionary<Type,Dictionary<object,object>> storage = new();

        protected virtual Dictionary<object, object> GetCache(Type t)
        {
            if(storage.TryGetValue(t,out var value)) { return value; }
            
            return storage[t] = new();
        }
        
        public virtual object Get(Type t,object pk)
        {
            return GetCache(t)[pk] ?? null;
        }
        public virtual bool Contains(Type t,object pk)
        {
            return GetCache(t).ContainsKey(pk);
        }
        public virtual void Add(object obj)
        {
            if (obj != null)
            {
                var innerCache = GetCache(obj.GetType());
                var key = obj._GetTable().PrimaryKey.GetValue(obj);
                innerCache[key] = obj;
            }
        }
        public virtual void Remove(object obj)
        {
            GetCache(obj.GetType()).Remove(obj._GetTable().PrimaryKey.GetValue(obj));
        }
        public virtual void Remove(Type t,object pk)
        {
            GetCache(t).Remove(pk);
        }
        public virtual bool HasChanged(object obj)
        {
            return true;
        }
    }
}