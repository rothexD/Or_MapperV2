using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using ORMapper;
using ShowcaseOrm.Models;
using Newtonsoft.Json;

namespace ShowcaseOrm.Show
{
    public class insertClass
    {
        public static void Show()
        {
            Console.WriteLine("(5) Get Class with all teachers");
            Console.WriteLine("--------------------------");
            
            Teacher t = new Teacher();

            t.ID = "t.1ton";
            t.FirstName = "Pery";
            t.Name = "Mouse";
            t.Gender = Gender.male;
            t.BirthDate = new DateTime(1970, 8, 18);
            t.HireDate = new DateTime(2015, 6, 20);
            t.Salary = 600;
            Orm.Save(t);
            Orm.Save(new Class()
            {
                ID = "elektronik",
                Name = "Elektronik",
                Teacher = t,
            });
            
            Console.WriteLine(JsonConvert.SerializeObject(Orm.Get<Class>("elektronik"), 
                Formatting.None, 
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore 
                }));
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(Orm.Get<Teacher>("t.1ton"), 
                Formatting.None, 
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore 
                }));
            Console.WriteLine("\n");
        }
    }
}