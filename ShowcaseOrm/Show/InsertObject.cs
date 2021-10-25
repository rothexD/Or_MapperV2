using System;
using ORMapper;
using ShowcaseOrm.Models;

namespace ShowcaseOrm.Show
{
    /// <summary>This show case creates an object and saves it to database.</summary>
    public static class InsertObject
    {
        /// <summary>Implements the show case.</summary>
        public static void Show()
        {
            Console.WriteLine("(1) Insert object");
            Console.WriteLine("-----------------");

            var t = new Teacher();

            t.ID = "t.0";
            t.FirstName = "Pery";
            t.Name = "Mouse";
            t.Gender = Gender.male;
            t.BirthDate = new DateTime(1970, 8, 18);
            t.HireDate = new DateTime(2015, 6, 20);
            t.Salary = 600;


            Orm.Save(t);

            Console.WriteLine("\n");
        }
    }
}