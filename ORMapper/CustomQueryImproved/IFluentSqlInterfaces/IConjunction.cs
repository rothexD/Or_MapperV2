using System.Collections.Generic;

namespace ORMapper.CustomQueryImproved.IFluentSqlInterfaces
{
    public interface IConjunction<T>
    {
        public ITypeOfWhere<T> And();
        public ITypeOfWhere<T> Or();
        public IList<T> Execute();
        public IConjunction<T> BracketClose();
        public IConjunction<T> BracketOpen();
        public (string, List<(string, object)>) OutputResult();
    }
}