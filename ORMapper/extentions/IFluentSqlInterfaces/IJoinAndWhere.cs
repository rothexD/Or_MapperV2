﻿using System.Collections.Generic;

namespace ORMapper.extentions.IFluentSqlInterfaces
{
    public interface IJoinAndWhere
    {
        public IJoinAndWhere InnerJoin(string tableName, string compareLeft, string compareRight);
        public IJoinAndWhere RightJoin(string tableName, string compareLeft, string compareRight);
        public IJoinAndWhere LeftJoin(string tableName, string compareLeft, string compareRight);
        public IJoinAndWhere Join(string tableName, string compareLeft, string compareRight);
        public (List<(string, object)>, string) Build();
        
        public ITypeOfWhere Where();
    }
}