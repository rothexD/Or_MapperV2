using System.Data;

namespace ORMapper.FluentQuery.IFluentSqlInterfaces
{
    public interface ITypeOfWhere
    {
        public IDbCommand getCommand { get; }
        public IConjunction EqualsDb<T, C>((T first, C second) tupel);
        public IConjunction NotEquals<T, C>((T first, C second) tupel);
        public IConjunction Like<T, C>((T first, C second) tupel);
        public IConjunction Smaller<T, C>((T first, C second) tupel);
        public IConjunction Greater<T, C>((T first, C second) tupel);
        public IConjunction SmallerEquals<T, C>((T first, C second) tupel);
        public IConjunction GreaterEquals<T, C>((T first, C second) tupel);
    }
}