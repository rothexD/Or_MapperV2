using System.Collections.Generic;

namespace ORMapper.CustomQueryImproved.IFluentSqlInterfaces
{
    public interface IJoinAndWhere
    {
        public IList<T> GetAllMatches<T>();
        public (string, List<(string, object)>) OutputResult();
        public ITypeOfWhere Where();
    }
}