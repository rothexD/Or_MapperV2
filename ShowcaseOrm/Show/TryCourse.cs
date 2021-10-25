using System;
using Newtonsoft.Json;
using ORMapper;
using ShowcaseOrm.Models;

public static class TryCourse
{
    /// <summary>Implements the show case.</summary>
    public static void Show()
    {
        Console.WriteLine("(4) Insert object");
        Console.WriteLine("-----------------");

        Teacher t = new Teacher();

        t.ID = "t.Course";
        t.FirstName = "Pery";
        t.Name = "Mouse";
        t.Gender = Gender.male;
        t.BirthDate = new DateTime(1970, 8, 18);
        t.HireDate = new DateTime(2015, 6, 20);
        t.Salary = 600;

        var course = new Course()
        {
            ID = "course2",
            Name = "course",
            Teacher = t
        };
        Orm.Save(course);
        
        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(Orm.Get<Course>("course"), 
            Formatting.None, 
            new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore 
            }));
        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(Orm.Get<Teacher>("t.Course"), 
            Formatting.None, 
            new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore 
            }));

        foreach (var VARIABLE in Orm.Get<Teacher>("t.Course").Courses)
        {
            Console.WriteLine("one course");
            Console.WriteLine(VARIABLE.Name);
            Console.WriteLine(VARIABLE.ID);
            
        }
        Console.WriteLine("\n");
    }
}