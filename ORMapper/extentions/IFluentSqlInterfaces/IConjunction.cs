using System.Collections.Generic;

namespace ORMapper.extentions.IFluentSqlInterfaces
{
    public interface IConjunction
    {
        public ITypeOfWhere And();
        public ITypeOfWhere Or();
        public (List<(string, object)>, string) Build();
    }
}