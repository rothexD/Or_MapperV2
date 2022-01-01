using System;
using System.Collections.Generic;
using ORMapper.CustomQueryImproved.IFluentSqlInterfaces;
using OrMapper.ExternalModels;

namespace ORMapper.CustomQueryImproved
{
    /// <summary>
    ///     provides a custom sql query filter fluent api
    /// </summary>
    public class CustomQueryImproved : ITypeOfWhere, IConjunction, IJoinAndWhere
    {
        private readonly List<(string, object)> _parameterList = new();
        private readonly string _parameterPrefix;
        private int _bracketCount;
        private int _counter;
        private string _selectBlock = "";

        private CustomQueryImproved(string parameterPrefix)
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

        public IConjunction BracketOpen()
        {
            _selectBlock += "(";
            _bracketCount++;
            return this;
        }

        public IConjunction BracketClose()
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
        public IList<T> GetAllMatches<T>()
        {
            if (_bracketCount != 0) throw new Exception("mismatching open and close in query");
            return (List<T>) Orm._CreateObjectAll(typeof(T), new List<object>(), (_selectBlock, _parameterList));
        }

        public (string, List<(string, object)>) OutputResult()
        {
            return (_selectBlock, _parameterList);
        }


        public ITypeOfWhere Where()
        {
            _selectBlock += " WHERE ";
            return this;
        }

        public IConjunction Equals<T1, T2>(T1 first, T2 second)
        {
            TypeOfWhereHelper(first, second, "=");

            return this;
        }

        public IConjunction NotEquals<T1, T2>(T1 first, T2 second)
        {
            TypeOfWhereHelper(first, second, "!=");
            return this;
        }

        public IConjunction Like<T1, T2>(T1 first, T2 second)
        {
            TypeOfWhereHelper(first, second, "LIKE");
            return this;
        }

        public IConjunction NotLike<T1, T2>(T1 first, T2 second)
        {
            TypeOfWhereHelper(first, second, "NOT LIKE");
            return this;
        }

        public IConjunction Smaller<T1, T2>(T1 first, T2 second)
        {
            TypeOfWhereHelper(first, second, "<");
            return this;
        }

        public IConjunction Greater<T1, T2>(T1 first, T2 second)
        {
            TypeOfWhereHelper(first, second, ">");

            return this;
        }

        public IConjunction SmallerEquals<T1, T2>(T1 first, T2 second)
        {
            TypeOfWhereHelper(first, second, "<=");
            return this;
        }

        public IConjunction GreaterEquals<T1, T2>(T1 first, T2 second)
        {
            TypeOfWhereHelper(first, second, ">=");

            return this;
        }

        public ITypeOfWhere BracketOpen_()
        {
            _selectBlock += "(";
            _bracketCount++;
            return this;
        }

        public ITypeOfWhere BracketClose_()
        {
            _selectBlock += ")";
            _bracketCount--;
            return this;
        }

        public static IJoinAndWhere Create(string parameterPrefix = ":")
        {
            return new CustomQueryImproved(parameterPrefix);
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