using System;

namespace OrMapper.Attributes
{
    public class ForeignKeyManyToMany : ForeignKeyAttribute
    {
        public string TheirReferenceToThisColumnName = null;
        public Type RemoteTableName = null;
    }
}