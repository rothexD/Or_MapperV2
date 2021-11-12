using System;
using System.Collections;
using System.Collections.Generic;

namespace ORMapper.Caches
{
    public class Cache
    {
        protected Dictionary<Type,Dictionary<object,object>> storage = new();

        protected virtual Dictionary<object, object> GetCache(Type t)
        {
            if(storage.TryGetValue(t,out var value)) { return value; }
            
            return storage[t] = new();
        }
        
        public virtual object Get(Type t,object pk)
        {
            var innerCache = GetCache(t);

            return innerCache[pk] ?? null;
        }
        public virtual object Contains(Type t,object pk)
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
        public virtual bool HasChanged(object obj)
        {
            return true;
        }
    }
}