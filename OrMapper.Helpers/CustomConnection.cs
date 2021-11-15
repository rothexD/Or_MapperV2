using System;
using System.Data;
using Npgsql;

namespace OrMapper.Helpers
{
    /// <summary>
    /// defines a static global counter
    /// </summary>
    public static class Counter
    {
        public static int CounterI;
        public static int LongTermCounter;
    }
    /// <summary>
    /// defines a customconnection that can be used with using(){} scoping
    /// </summary>
    public class CustomConnection : IDisposable
    {
        private IDbConnection _connection;
        private readonly string _connectionstring;

        
        public CustomConnection(string connectionstring)
        {
            this._connectionstring = connectionstring;
        }

        public void Dispose()
        {
            Counter.CounterI--;
            _connection.Close();
        }

        public IDbConnection Open()
        {
            _connection = new NpgsqlConnection(_connectionstring);
            _connection.Open();
            Counter.CounterI++;
            
            return _connection;
        }
    }
    
    
}