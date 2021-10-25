using System;
using System.Data;
using ORMapper.FluentQuery.IFluentSqlInterfaces;

namespace ORMapper.FluentQuery
{
    public class CustomQuery : ISelectParam, IFrom, ITable, IWhere, ITypeOfWhere, IConjunction
    {
        private string _selectBlock = "";
        private IDbConnection con;
        private int counter;

        private CustomQuery()
        {
        }

        private string Query { get; set; }

        public ITypeOfWhere And()
        {
            getCommand.CommandText += " and ";
            return this;
        }

        public ITypeOfWhere Or()
        {
            getCommand.CommandText += " or ";
            return this;
        }

        public ITable From()
        {
            var selectblock = _selectBlock.Trim().Trim(',');

            getCommand.CommandText += selectblock;

            return this;
        }

        public IFrom SelectParam(string param)
        {
            IDataParameter par = getCommand.CreateParameter();
            par.ParameterName = " :w" + counter;
            par.Value = param;
            _selectBlock += " :w" + counter + ", ";
            counter++;
            return this;
        }

        public IWhere Table(Type table)
        {
            var tablename = table._GetTable().TableName;
            getCommand.CommandText += " " + tablename + "";
            return this;
        }

        public IDbCommand getCommand { get; private set; }

        public IConjunction EqualsDb<T, C>((T first, C second) tupel)
        {
            TypeOfWhereHelper(tupel, "=");

            return this;
        }

        public IConjunction NotEquals<T, C>((T first, C second) tupel)
        {
            TypeOfWhereHelper(tupel, "!=");
            return this;
        }

        public IConjunction Like<T, C>((T first, C second) tupel)
        {
            TypeOfWhereHelper(tupel, "like");
            return this;
        }

        public IConjunction Smaller<T, C>((T first, C second) tupel)
        {
            TypeOfWhereHelper(tupel, "<");
            return this;
        }

        public IConjunction Greater<T, C>((T first, C second) tupel)
        {
            TypeOfWhereHelper(tupel, ">");

            return this;
        }

        public IConjunction SmallerEquals<T, C>((T first, C second) tupel)
        {
            TypeOfWhereHelper(tupel, "<=");
            return this;
        }

        public IConjunction GreaterEquals<T, C>((T first, C second) tupel)
        {
            TypeOfWhereHelper(tupel, ">");

            return this;
        }

        public ITypeOfWhere Where()
        {
            getCommand.CommandText += " Where ";
            return this;
        }

        public static ISelectParam Select()
        {
            var i = new CustomQuery();
            i.con = Orm.Connection();
            i.getCommand = i.con.CreateCommand();

            i.getCommand.CommandText += "SELECT ";
            return i;
        }

        private void TypeOfWhereHelper<T, C>((T first, C second) tupel, string insert)
        {
            var par1name = ":w" + counter;
            counter++;
            var par2name = ":w" + counter;
            counter++;

            IDataParameter par1 = getCommand.CreateParameter();
            par1.ParameterName = par1name;
            par1.Value = tupel.first;

            IDataParameter par2 = getCommand.CreateParameter();
            par2.ParameterName = par2name;
            par2.Value = tupel.second;

            getCommand.Parameters.Add(par1);
            getCommand.Parameters.Add(par2);

            getCommand.CommandText += " " + par1name + " " + insert + " " + " :w" + par2name + " ";
        }
    }
}