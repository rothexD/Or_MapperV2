using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using ORMapper.Models;

namespace ORMapper.Caches
{
    public class TrackingCache : Cache
    {
        private string calculateHash(object obj)
        {
            string rval = "";
            foreach(Column i in obj._GetTable().Internals) 
            {
                if(i.IsForeignKey)
                {
                    object m = i.GetValue(obj);
                    if(m != null) { rval += m._GetTable().PrimaryKey.GetValue(m).ToString(); }
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
                        rval += k._GetTable().PrimaryKey.GetValue(k).ToString() + ",";
                    }
                }
            }

            return Encoding.UTF8.GetString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(rval)));
        }

        public override bool HasChanged(object obj)
        {
            var hash1 = calculateHash(obj);
            var hash2 = calculateHash(Get(obj.GetType(), obj));
            
            return hash1 == hash2;
        }
    }
}