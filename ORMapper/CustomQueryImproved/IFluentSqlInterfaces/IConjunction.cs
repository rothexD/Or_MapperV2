using System.Collections.Generic;

namespace ORMapper.CustomQueryImproved.IFluentSqlInterfaces
{
    public interface IConjunction
    {
        public ITypeOfWhere And();
        public ITypeOfWhere Or();
        public IList<T> GetAllMatches<T>();
        public IConjunction BracketClose();
        public IConjunction BracketOpen();
    }
}