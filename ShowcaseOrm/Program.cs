using System;
using System.Data;
using Npgsql;
using ORMapper;
using ShowcaseOrm.Models;
using ShowcaseOrm.Show;

namespace ShowcaseOrm
{
    class Program
    {
        static void Main(string[] args)
        {
            Orm.Connection =
                new NpgsqlConnection(
                    "Server=127.0.0.1;Port=5438;Database=postgres;User Id=postgres;Password=postgres;");
            Orm.Connection.Open();
         
            InsertObject.Show();
            ModifyObject.Show();
            GetAllObjets.Show();

        }
    }
    
}