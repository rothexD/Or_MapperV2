using System.Collections.Generic;

namespace ORMapper.CustomQueryImproved.IFluentSqlInterfaces
{
    public interface IJoinAndWhere<T>
    {
        public IList<T> Execute();
        public (string, List<(string, object)>) OutputResult();
        public ITypeOfWhere<T> Where();
    }
}