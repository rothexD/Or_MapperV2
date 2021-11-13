using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using ORMapper.FluentSqlQueryApi.IFluentSqlInterfaces;
using ORMapper.Logging;

namespace ORMapper.FluentSqlQueryApi
{
    public class CustomQuery : ISelect, IFrom, ITypeOfWhere, IConjunction, IJoinAndWhere
    {
        private ILogger logger = CustomLogger.GetLogger<CustomQuery>();
        private string _selectBlock = "";
        private readonly List<(string, object)> _parameterList = new ();
        private int _counter = 0;
        private readonly string _parameterPrefix;
        
        public static ISelect Create(string parameterPrefix = ":")
        {
            return new CustomQuery(parameterPrefix);
        }

        private CustomQuery(string parameterPrefix)
        {
            _parameterPrefix = parameterPrefix;
        }
        public ITypeOfWhere And()
        {
            _selectBlock += " and ";
            return this;
        }

        public ITypeOfWhere Or()
        {
            _selectBlock += " or ";
            return this;
        }

        public IJoinAndWhere From(string tableName)
        {
            _selectBlock += " from " + tableName + " ";
            return this;
        }

        public IFrom Select(string[] param)
        {
            this._selectBlock += "Select ";
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
            TypeOfWhereHelper(first,second, "like");
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
            _selectBlock += " Where ";
            return this;
        }
        

        private void TypeOfWhereHelper<T, C>(T first, C second, string insert)
        {
            if (first is SecureParameter secureParameter1)
            {
                _selectBlock += " "+_parameterPrefix+"param" + _counter + " ";
                _parameterList.Add((_parameterPrefix+"param" + _counter++,secureParameter1.Parameter));
            }
            else
            {
                _selectBlock += " " + first + " ";
            }

            _selectBlock += insert + " ";
            
            if (second is SecureParameter secureParameter2)
            {
                _selectBlock += " "+_parameterPrefix+"param" + _counter + " "; 
                _parameterList.Add((_parameterPrefix+"param" +_counter++,secureParameter2.Parameter));
            }
            else
            {
                _selectBlock += " " + second+ " ";
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