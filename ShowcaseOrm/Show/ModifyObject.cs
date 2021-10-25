using System;
using ORMapper;
using ShowcaseOrm.Models;

namespace ShowcaseOrm.Show
{
    /// <summary>This show case loads and modifies an object and saves it to database.</summary>
    public static class ModifyObject
    {
        /// <summary>Implements the show case.</summary>
        public static void Show()
        {
            Console.WriteLine("(2) Load and modify object");
            Console.WriteLine("--------------------------");

            var t = Orm.Get<Teacher>("t.0");

            Console.WriteLine();
            Console.WriteLine("Salary for " + t.FirstName + " " + t.Name + " is " + t.Salary + " Pesos.");

            Console.WriteLine("Give rise of 12000.");
            t.Salary += 12000;

            Console.WriteLine("Salary for " + t.FirstName + " " + t.Name + " is now " + t.Salary + " Pesos.");

            Orm.Save(t);

            Console.WriteLine("\n");
        }
    }
}