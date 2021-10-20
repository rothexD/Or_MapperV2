namespace ORMapper.FluentQuery.IFluentSqlInterfaces
{
    public interface IConjunction
    {
        public ITypeOfWhere And();
        public ITypeOfWhere Or();
    }
}