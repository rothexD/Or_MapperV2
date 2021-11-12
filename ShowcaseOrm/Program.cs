using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Threading;
using ORMapper;
using ORMapper.extentions;
using ORMapper.FluentSqlQueryApi;
using ORMapper.Models;
using ShowcaseOrm.Models;
using ShowcaseOrm.Show;

namespace ShowcaseOrm
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Orm.Connectionstring = "Server=127.0.0.1;Port=5438;Database=school;User Id=postgres;Password=postgres;";

            ShowCustomFluentQueryApi.Show();

            ShowMapTablesAndTypes.Show(false);

            ShowInsertStudent.Show();
            ShowModifyStudent.Show();

            
            Show1To1Save.Show();
            Show1ToNSave.Show();
            ShowNtoMSave.Show();

            
            ShowModifyInDepth.Show();
            ShowTransaction.Show();
            // InsertModifyShouldsave.Show();
            //GetStudent.Show();
            /*Console.WriteLine(counter.counterI);
            Console.WriteLine(counter.counterI);
            InsertObject.Show();
            Console.WriteLine(counter.counterI);
            ModifyObject.Show();
            Console.WriteLine(counter.counterI);
            GetAllObjets.Show();
            Console.WriteLine(counter.counterI);
            TryCourse.Show();
            Console.WriteLine(counter.counterI);
            insertClass.Show();*/
        }
    }
}