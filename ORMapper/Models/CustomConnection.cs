using System;
using System.Data;
using Npgsql;

namespace ORMapper.Models
{
    /// <summary>
    /// defines a static global counter
    /// </summary>
    public static class counter
    {
        public static int counterI;
    }
    /// <summary>
    /// defines a customconnection that can be used with using(){} scoping
    /// </summary>
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
    
    /// <summary>
    /// defines an IDbConnection extentions
    /// </summary>
    public static class DbExtention
    {
        /// <summary>
        /// Decrements the global counter.counterI by 1 and closes the connection
        /// </summary>
        /// <param name="a">IDbConnection</param>
        public static void CloseCustom(this IDbConnection a)
        {
            counter.counterI--;
            a.Close();
        }
        
        /// <summary>
        /// increments the global counter.counterI by 1 and opens the connection
        /// </summary>
        /// <param name="a">IDbConnection</param>
        public static void CustomOpen(this IDbConnection a)
        {
            counter.counterI++;
            a.Open();
        }
    }
}