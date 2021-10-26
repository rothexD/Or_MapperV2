using System;
using ORMapper;
using ShowcaseOrm.Models;

namespace ShowcaseOrm.Show
{
    public static class InsertModifyShouldsave
    {
        public static void Show()
        {
            Console.WriteLine("(6)---------------------------------------------------");
            Console.WriteLine("Try modifying in depth");
            var t = new Teacher();

            t.ID = "mechaniker";
            t.FirstName = "Mechy";
            t.Name = "test";
            t.Gender = Gender.male;
            t.BirthDate = new DateTime(1970, 8, 18);
            t.HireDate = new DateTime(2015, 6, 20);
            t.Salary = 600;
            Orm.Save(t);
            
            var t2 = new Teacher();

            t2.ID = "mechaniker2";
            t2.FirstName = "Mechy";
            t2.Name = "test";
            t2.Gender = Gender.male;
            t2.BirthDate = new DateTime(1970, 8, 18);
            t2.HireDate = new DateTime(2015, 6, 20);
            t2.Salary = 600;

            Orm.Save(new Course
            {
                ID = "autobau",
                Name = "autobau",
                Teacher = t
            });
            t.Name = "changed";
            Orm.Save(new Course
            {
                ID = "mechanik",
                Name = "mechanik",
                Teacher = t2
            });

            Console.WriteLine("before get");
            Orm.Get<Teacher>("mechaniker");

            Console.WriteLine("before get");
            var i = Orm.Get<Teacher>("mechaniker");
            i.Courses[0].Name = "roboter";
            Console.WriteLine(i.Name);
            Console.WriteLine("before save");

            Console.WriteLine(i.Courses[0].Name);
            Orm.Save(i);
            var item = Orm.Get<Teacher>("mechaniker");

            Console.WriteLine(item.Courses[0].Name);
        }
    }
}