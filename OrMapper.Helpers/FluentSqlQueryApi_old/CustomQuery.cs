using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using OrMapper.ExternalModels;
using OrMapper.Helpers.FluentSqlQueryApi.IFluentSqlInterfaces;
using OrMapper.Logging;

namespace OrMapper.Helpers.FluentSqlQueryApi
{
    public class CustomQuery : ISelect, IFrom, ITypeOfWhere, IConjunction, IJoinAndWhere
    {
        private ILogger logger = CustomLoggerDependencyContainer.GetLogger<CustomQuery>();
        private string _selectBlock = "";
        private readonly List<(string, object)> _parameterList = new ();
        private int _counter = 0;
        private readonly string _parameterPrefix;
        
        public static ISelect Create(string parameterPrefix = "@")
        {
            return new CustomQuery(parameterPrefix);
        }

        private CustomQuery(string parameterPrefix)
        {
            _parameterPrefix = parameterPrefix;
        }
        public ITypeOfWhere And()
        {
            _selectBlock += " AND ";
            return this;
        }

        public ITypeOfWhere Or()
        {
            _selectBlock += " OR ";
            return this;
        }

        public IJoinAndWhere From(string tableName)
        {
            _selectBlock += " FROM " + tableName + " ";
            return this;
        }

        public IFrom Select(string[] param)
        {
            this._selectBlock += "SELECT ";
            foreach (var i in param)
            {
                _selectBlock += i + ", ";
            }

            this._selectBlock = _selectBlock.Trim().Trim(',');
            return this;
        }

        public IConjunction Equals<T, T2>(T first, T2 second)
        {
            TypeOfWhereHelper(first,second, "=");

            return this;
        }

        public IConjunction NotEquals<T, T2>(T first, T2 second)
        {
            TypeOfWhereHelper(first,second, "!=");
            return this;
        }

        public IConjunction Like<T1, T2>(T1 first, T2 second)
        {
            TypeOfWhereHelper(first,second, "LIKE");
            return this;
        }
        
        public IConjunction NotLike<T1, T2>(T1 first, T2 second)
        {
            TypeOfWhereHelper(first,second, "NOT LIKE");
            return this;
        }
        public IConjunction Smaller<T1, T2>(T1 first, T2 second)
        {
            TypeOfWhereHelper(first,second, "<");
            return this;
        }

        public IConjunction Greater<T1, T2>(T1 first, T2 second)
        {
            TypeOfWhereHelper(first,second, ">");

            return this;
        }

        public IConjunction SmallerEquals<T1, T2>(T1 first, T2 second)
        {
            TypeOfWhereHelper(first,second, "<=");
            return this;
        }

        public IConjunction GreaterEquals<T1, T2>(T1 first, T2 second)
        {
            TypeOfWhereHelper(first,second, ">");

            return this;
        }

        public ITypeOfWhere Where()
        {
            _selectBlock += " WHERE ";
            return this;
        }
        

        private void TypeOfWhereHelper<T, C>(T first, C second, string insert)
        {
            TypeOfWhereParameterHelper(first);
            _selectBlock += insert + " ";
            TypeOfWhereParameterHelper(second);
        }

        private void TypeOfWhereParameterHelper<T>(T para)
        {
            if (para is CaseInsensitive external)
            {
                _selectBlock += " LOWER(";
                TypeOfWhereParameterHelper(external.Parameter);
                _selectBlock += ")";
                return;
            }
            if (para is SecureParameter secureParameter1)
            {
                _selectBlock += " "+_parameterPrefix+"param" + _counter + " ";
                _parameterList.Add((_parameterPrefix+"param" + _counter++,secureParameter1.Parameter));
            }
            else
            {
                _selectBlock += " " + para + " ";
            }
        }
        

        private void JoinHelper(string joinStatement, string tableName, string compareLeft, string compareRight)
        {
            _selectBlock += joinStatement + " " + tableName + " on " + compareLeft + " = " + compareRight;
        }
        public IJoinAndWhere InnerJoin(string tableName, string compareLeft, string compareRight)
        {
            JoinHelper("inner join", tableName, compareLeft, compareRight);
            return this;
        }
        public IJoinAndWhere RightJoin(string tableName, string compareLeft, string compareRight)
        {
            JoinHelper("right join", tableName, compareLeft, compareRight);
            return this;
        }
        public IJoinAndWhere LeftJoin(string tableName, string compareLeft, string compareRight)
        {
            JoinHelper("left join", tableName, compareLeft, compareRight);
            return this;
        }
        public IJoinAndWhere Join(string tableName, string compareLeft, string compareRight)
        {
            JoinHelper("inner join", tableName, compareLeft, compareRight);
            return this;
        }
        public (List<(string,object)>, string) Build()
        {
            return (this._parameterList, this._selectBlock);
        }
    }
}