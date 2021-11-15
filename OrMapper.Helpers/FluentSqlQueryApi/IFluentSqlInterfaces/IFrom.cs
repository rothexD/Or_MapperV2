namespace OrMapper.Helpers.FluentSqlQueryApi.IFluentSqlInterfaces
{
    public interface IFrom
    {
        public IJoinAndWhere From(string tableName);
    }
}