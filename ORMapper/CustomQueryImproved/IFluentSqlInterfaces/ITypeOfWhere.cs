namespace ORMapper.CustomQueryImproved.IFluentSqlInterfaces
{
    public interface ITypeOfWhere
    {
        public IConjunction Equals<T1, T2>(T1 first, T2 second);
        public IConjunction NotEquals<T1, T2>(T1 first, T2 second);
        public IConjunction Like<T1, T2>(T1 first, T2 second);
        public IConjunction Smaller<T1, T2>(T1 first, T2 second);
        public IConjunction Greater<T1, T2>(T1 first, T2 second);
        public IConjunction SmallerEquals<T1, T2>(T1 first, T2 second);
        public IConjunction GreaterEquals<T1, T2>(T1 first, T2 second);
        public IConjunction NotLike<T1, T2>(T1 first, T2 second);
        public ITypeOfWhere BracketClose_();
        public ITypeOfWhere BracketOpen_();

    }
}