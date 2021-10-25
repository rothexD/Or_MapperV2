using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using ORMapper;
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

            List<Type> tables = new();
            List<Type> enums = new();
            enums.Add(typeof(Gender));
            tables.Add(typeof(Course));
            tables.Add(typeof(Student));
            tables.Add(typeof(STUDENT_COURSES));
            tables.Add(typeof(Class));
            tables.Add(typeof(Teacher));

            OrmMapping.Map(tables.ToArray(),enums.ToArray());
            
            
            //GetStudent.Show();
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