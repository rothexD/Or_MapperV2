using System;
using Newtonsoft.Json;
using ORMapper;
using ShowcaseOrm.Models;

namespace ShowcaseOrm.Show
{
    public static class GetStudent
    {
        public static void Show()
        {

            Console.WriteLine(JsonConvert.SerializeObject(Orm.Get<Student>("s.01"),
                Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }));
            var x = Orm.Get<Student>("s.01");
            foreach (var i in (x.Course))
            {
                
                Console.WriteLine(i.ID);
            }

            x.Course[0].Name = "changed";
            Orm.Save(x);
        }
        
    }
}