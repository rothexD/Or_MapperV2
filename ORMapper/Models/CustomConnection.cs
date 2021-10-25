using System;
using System.Data;
using Npgsql;

namespace ORMapper.Models
{
    public static class counter
    {
        public static int counterI;
    }

    public class CustomConnection : IDisposable
    {
        private IDbConnection connection;
        private readonly string connectionstring;


        public CustomConnection(string connectionstring)
        {
            this.connectionstring = connectionstring;
        }

        public void Dispose()
        {
            counter.counterI--;
            connection.Close();
        }

        public IDbConnection Open()
        {
            connection = new NpgsqlConnection(connectionstring);
            connection.Open();
            counter.counterI++;
            return connection;
        }
    }

    public static class DbExtention
    {
        public static void CloseCustom(this IDbConnection a)
        {
            counter.counterI--;
            a.Close();
        }

        public static void CustomOpen(this IDbConnection a)
        {
            counter.counterI++;
            a.Open();
        }
    }
}