using System;
using System.Data;
using System.Threading;
using Npgsql;
using ORMapper;
using ORMapper.Models;
using ShowcaseOrm.Models;
using ShowcaseOrm.Show;

namespace ShowcaseOrm
{
    class Program
    {
        static void Main(string[] args)
        {
            Orm.Connectionstring = "Server=127.0.0.1;Port=5438;Database=school;User Id=postgres;Password=postgres;";
           
            Console.WriteLine(counter.counterI);
            InsertModifyShouldsave.Show();
            Thread.Sleep(1000);
            Console.WriteLine(counter.counterI);
            InsertObject.Show();
            Thread.Sleep(1000);
            Console.WriteLine(counter.counterI);
            ModifyObject.Show();
            Thread.Sleep(1000);
            Console.WriteLine(counter.counterI);
            GetAllObjets.Show();
            Thread.Sleep(1000);
            Console.WriteLine(counter.counterI);
            TryCourse.Show();
            Thread.Sleep(1000);
            Console.WriteLine(counter.counterI);
            insertClass.Show();
            
        }
    }
}