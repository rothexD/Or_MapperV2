using System;
using System.Collections.Generic;
using ORMapper.CustomQueryImproved.IFluentSqlInterfaces;
using OrMapper.ExternalModels;
namespace ORMapper.CustomQueryImproved
{
    /// <summary>
    ///     provides a custom sql query filter fluent api
    /// </summary>
    public class CustomGet<T> : ITypeOfWhere<T>, IConjunction<T>, IJoinAndWhere<T> where T : class
    {
        private readonly List<(string, object)> _parameterList = new();
        private readonly string _parameterPrefix;
        private int _bracketCount;
        private int _counter;
        private string _selectBlock = "";

        private CustomGet(string parameterPrefix)
        {
            _parameterPrefix = parameterPrefix;
        }

        public ITypeOfWhere<T> And()
        {
            _selectBlock += " AND ";
            return this;
        }

        public ITypeOfWhere<T> Or()
        {
            _selectBlock += " OR ";
            return this;
        }

        public IConjunction<T> BracketOpen()
        {
            _selectBlock += "(";
            _bracketCount++;
            return this;
        }

        public IConjunction<T> BracketClose()
        {
            _selectBlock += ")";
            _bracketCount--;
            return this;
        }

        /// <summary>
        ///     creates object list for requested sql
        /// </summary>
        /// <typeparam name="T">requested type</typeparam>
        /// <returns>IList of requested Type</returns>
        /// <exception cref="Exception">mismatch in bracketcount</exception>
        public IList<T> Execute()
        {
            if (_bracketCount != 0) throw new Exception("mismatching open and close in query");
            return (List<T>) Orm._CreateObjectAll(typeof(T), new List<object>(), (_selectBlock, _parameterList));
        }

        public (string, List<(string, object)>) OutputResult()
        {
            return (_selectBlock, _parameterList);
        }


        public ITypeOfWhere<T> Where()
        {
            _selectBlock += " WHERE ";
            return this;
        }

        public IConjunction<T> Equals<T1, T2>(T1 first, T2 second)
        {
            TypeOfWhereHelper(first, second, "=");

            return this;
        }

        public IConjunction<T> NotEquals<T1, T2>(T1 first, T2 second)
        {
            TypeOfWhereHelper(first, second, "!=");
            return this;
        }

        public IConjunction<T> Like<T1, T2>(T1 first, T2 second)
        {
            TypeOfWhereHelper(first, second, "LIKE");
            return this;
        }

        public IConjunction<T> NotLike<T1, T2>(T1 first, T2 second)
        {
            TypeOfWhereHelper(first, second, "NOT LIKE");
            return this;
        }

        public IConjunction<T> Smaller<T1, T2>(T1 first, T2 second)
        {
            TypeOfWhereHelper(first, second, "<");
            return this;
        }

        public IConjunction<T> Greater<T1, T2>(T1 first, T2 second)
        {
            TypeOfWhereHelper(first, second, ">");

            return this;
        }

        public IConjunction<T> SmallerEquals<T1, T2>(T1 first, T2 second)
        {
            TypeOfWhereHelper(first, second, "<=");
            return this;
        }

        public IConjunction<T> GreaterEquals<T1, T2>(T1 first, T2 second)
        {
            TypeOfWhereHelper(first, second, ">=");

            return this;
        }

        public ITypeOfWhere<T> BracketOpen_()
        {
            _selectBlock += "(";
            _bracketCount++;
            return this;
        }

        public ITypeOfWhere<T> BracketClose_()
        {
            _selectBlock += ")";
            _bracketCount--;
            return this;
        }

        public static IJoinAndWhere<T> Create(string parameterPrefix = ":")
        {
            return new CustomGet<T>(parameterPrefix);
        }

        private void TypeOfWhereHelper<T1, T2>(T1 first, T2 second, string insert)
        {
            TypeOfWhereParameterHelper(first);
            _selectBlock += insert + " ";
            TypeOfWhereParameterHelper(second);
        }

        private void TypeOfWhereParameterHelper<T1>(T1 para)
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
                _selectBlock += " " + _parameterPrefix + "param" + _counter + " ";
                _parameterList.Add((_parameterPrefix + "param" + _counter++, secureParameter1.Parameter));
            }
            else
            {
                _selectBlock += " " + para + " ";
            }
        }
    }
}