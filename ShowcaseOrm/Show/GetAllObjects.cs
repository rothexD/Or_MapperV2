using System;
using System.Linq;
using Newtonsoft.Json;
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
            Console.WriteLine(JsonConvert.SerializeObject(Orm.GetAll<Teacher>(),
                Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }));
            var i = Orm.GetAll<Teacher>("t.0");
            
            
            
            //i[0].Courses[0].Name = "changedAgain";
            //i[0].Classes[0].Name = "changedAgain";
            Orm.Save(i[0]);
        }
    }
}