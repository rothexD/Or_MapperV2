using System;
using System.Data;

namespace OrMapper.Helpers.extentions
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
    /// defines an IDbConnection extentions
    /// </summary>
    public static class DbExtentions
    {
        /// <summary>
        /// Decrements the global Counter.CounterI by 1 and closes the connection
        /// </summary>
        /// <param name="a">IDbConnection</param>
        public static void Close(this IDbConnection a)
        {
            Counter.CounterI--;
            a.Close();
        }
        
        /// <summary>
        /// increments the global Counter.CounterI by 1 and opens the connection
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