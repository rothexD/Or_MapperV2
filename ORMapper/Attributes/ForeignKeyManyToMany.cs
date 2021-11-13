using System;

namespace ORMapper.Attributes
{
    public class ForeignKeyManyToMany : ForeignKeyAttribute
    {
        public string TheirReferenceToThisColumnName = null;
        public Type RemoteTableName = null;
    }
}