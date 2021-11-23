using System.Collections.Generic;

namespace ORMapper.CustomQueryImproved.IFluentSqlInterfaces
{
    public interface IJoinAndWhere
    {
        public IList<T> GetAllMatches<T>();
        
        public ITypeOfWhere Where();
    }
}