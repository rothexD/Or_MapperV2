namespace ORMapper.CustomQueryImproved.IFluentSqlInterfaces
{
    public interface ITypeOfWhere<T>
    {
        public IConjunction<T> Equals<T1, T2>(T1 first, T2 second);
        public IConjunction<T> NotEquals<T1, T2>(T1 first, T2 second);
        public IConjunction<T> Like<T1, T2>(T1 first, T2 second);
        public IConjunction<T> Smaller<T1, T2>(T1 first, T2 second);
        public IConjunction<T> Greater<T1, T2>(T1 first, T2 second);
        public IConjunction<T> SmallerEquals<T1, T2>(T1 first, T2 second);
        public IConjunction<T> GreaterEquals<T1, T2>(T1 first, T2 second);
        public IConjunction<T> NotLike<T1, T2>(T1 first, T2 second);
        public ITypeOfWhere<T> BracketClose_();
        public ITypeOfWhere<T> BracketOpen_();

    }
}