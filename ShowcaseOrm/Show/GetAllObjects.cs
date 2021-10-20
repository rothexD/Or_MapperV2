using System;
using ORMapper;
using ShowcaseOrm.Models;

namespace ShowcaseOrm.Show
{
    /// <summary>This show case loads and modifies an object and saves it to database.</summary>
    public static class GetAllObjets
    {
        /// <summary>Implements the show case.</summary>
        public static void Show()
        {
            Console.WriteLine("(3) Get All Objects of type");
            Console.WriteLine("--------------------------");
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(Orm.GetAll<Teacher>()));
            Console.WriteLine("\n");
        }
    }
}