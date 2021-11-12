using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Threading;
using ORMapper;
using ORMapper.extentions;
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

            string[] test = {"a","b"};
            (var para,string sqlstring) = CustomQuery.Create("@").Select(test).From("student").Join("hans","hans.ID","student.Id").Where()
                .Equals(new SecureParameter(test), "test").And().Greater(2,1).Build();
            Console.WriteLine("sql: "+  Environment.NewLine+sqlstring);
            Console.WriteLine("paras");
            para.ForEach(x => Console.WriteLine("key: " + x.Item1 + " ,Value: " + x.Item2));

            /*List<Type> tables = new();
            tables.Add(typeof(Gender));
            tables.Add(typeof(Course));
            tables.Add(typeof(Student));
            tables.Add(typeof(STUDENT_COURSES));
            tables.Add(typeof(Class));
            tables.Add(typeof(Teacher));*/

            //OrmMapping.Map(tables.ToArray(),false);
            
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