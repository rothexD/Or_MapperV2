using System;
using System.Data;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;
using Npgsql;

namespace ORMapper.Models
{
    public static class counter
    {
        public static int counterI= 0;
    }
    public class CustomConnection : IDisposable
    {
        private string connectionstring;
        private IDbConnection connection;
        

        public CustomConnection(string connectionstring)
        {
            this.connectionstring = connectionstring;
        }

        public IDbConnection Open()
        {
            connection = new NpgsqlConnection(connectionstring);
            connection.Open();
            counter.counterI++;
            return connection;
        }
        public void Dispose()
        {
            counter.counterI--;
            connection.Close();
        }
       
    }
    public static class DbExtention
    {
        public static void CloseCustom(this IDbConnection a){
            counter.counterI--;
            a.Close();
        }
        public static void CustomOpen(this IDbConnection a){
            counter.counterI++;
            a.Open();
        }
    }
}