using System;
using System.Data;
using Npgsql;

namespace ORMapper.extentions
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
    
    /// <summary>
    /// defines an IDbConnection extentions
    /// </summary>
    public static class DbExtentions
    {
        /// <summary>
        /// Decrements the global counter.counterI by 1 and closes the connection
        /// </summary>
        /// <param name="a">IDbConnection</param>
        public static void Close(this IDbConnection a)
        {
            Counter.CounterI--;
            a.Close();
        }
        
        /// <summary>
        /// increments the global counter.counterI by 1 and opens the connection
        /// </summary>
        /// <param name="a">IDbConnection</param>
        public static void Open(this IDbConnection a)
        {
            Counter.CounterI++;
            Counter.LongTermCounter++;
            a.Open();
        }
        
        public static int ExecuteNonQuery(this IDbCommand a)
        {
            Console.WriteLine(a.CommandText);
            return a.ExecuteNonQuery();
        }
        public static IDataReader ExecuteReader(this IDbCommand a)
        {
            Console.WriteLine(a.CommandText);
            return a.ExecuteReader();
        }
    }
}