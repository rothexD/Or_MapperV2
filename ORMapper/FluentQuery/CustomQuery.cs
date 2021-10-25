using System;
using System.Data;
using System.Reflection.Metadata;
using ORMapper.FluentQuery.IFluentSqlInterfaces;
using ORMapper.Models;

namespace ORMapper.FluentQuery 
{
    public class CustomQuery : ISelectParam, IFrom, ITable, IWhere, ITypeOfWhere,IConjunction
    {
        private IDbConnection con;
        private IDbCommand command;
        private string Query { get; set; }
        private int counter = 0;

        private string _selectBlock = "";

        private CustomQuery()
        {
            
        }
        public static ISelectParam Select()
        {
            var i =new CustomQuery();
            i.con = Orm.Connection();
            i.command = i.con.CreateCommand();
            
            i.command.CommandText += "SELECT ";
            return i;
        }

        public IFrom SelectParam(string param)
        {
            IDataParameter par = command.CreateParameter();
            par.ParameterName = " :w" + counter.ToString();
            par.Value = param;
            _selectBlock += " :w" + counter.ToString()+", ";
            counter++;
            return this;
        }

        public ITable From()
        {
            string selectblock = _selectBlock.Trim().Trim(',');
            
            command.CommandText += selectblock;

            return this;
        }

        public IWhere Table(Type table)
        {
            string tablename = table._GetEntity().TableName;
            command.CommandText += " " + tablename + "" ;
            return this;
        }
        public ITypeOfWhere Where()
        {
            command.CommandText += " Where " ;
            return this;
        }
        
        public IConjunction EqualsDb<T,C>((T first,C second) tupel)
        {
            TypeOfWhereHelper(tupel, "=");
            
            return this;
        }
        public IConjunction NotEquals<T,C>((T first,C second) tupel)
        {
            TypeOfWhereHelper(tupel, "!=");
            return this;
        }
        public IConjunction Like<T,C>((T first,C second) tupel)
        {
            TypeOfWhereHelper(tupel, "like");
            return this;
        }
        public IConjunction Smaller<T,C>((T first,C second) tupel)
        {
            TypeOfWhereHelper(tupel, "<");
            return this;
        }
        public IConjunction Greater<T,C>((T first,C second) tupel)
        {
            TypeOfWhereHelper(tupel, ">");
            
            return this;
        }
        public IConjunction SmallerEquals<T,C>((T first,C second) tupel)
        {
            TypeOfWhereHelper(tupel, "<=");
            return this;
        }
        public IConjunction GreaterEquals<T,C>((T first,C second) tupel)
        {
            TypeOfWhereHelper(tupel, ">");
            
            return this;
        }

        public ITypeOfWhere And()
        {
            command.CommandText += " and ";
            return this;
        }
        public ITypeOfWhere Or()
        {
            command.CommandText += " or ";
            return this;
        }
        private void TypeOfWhereHelper<T, C>((T first, C second) tupel, string insert)
        {
            string par1name = ":w" + counter.ToString();
            counter++;
            string par2name = ":w" + counter.ToString();
            counter++;
            
            IDataParameter par1 = command.CreateParameter();
            par1.ParameterName = par1name;
            par1.Value = tupel.first;
            
            IDataParameter par2 = command.CreateParameter();
            par2.ParameterName = par2name;
            par2.Value = tupel.second;

            command.Parameters.Add(par1);
            command.Parameters.Add(par2);

            command.CommandText += " "+ par1name + " " +insert+" " + " :w" + par2name + " ";
        }

        public IDbCommand getCommand
        {
            get
            {
                return command;
            }
        }
    }
}