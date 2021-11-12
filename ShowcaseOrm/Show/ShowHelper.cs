using System;
using Newtonsoft.Json;

namespace ShowcaseOrm.Show
{
    public static class ShowHelper
    {
        private static int counter = 0;

        public static void Begin(string showingWhat)
        {
            Console.WriteLine("----------------------------------------------------------------------------------------------");
            Console.WriteLine($"Showing ({counter++}) ---  {showingWhat}");
            Console.WriteLine("----------------------------------------------------------------------------------------------");
            Console.WriteLine();
        }
        public static void End()
        {
            Console.WriteLine();
            return;
            Console.WriteLine("----------------------------------------------------------------------------------------------");
            Console.WriteLine("----------------------------------------------------------------------------------------------");
            Console.WriteLine();
        }

        public static void printNewtonsoftJson(object o)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(o, Formatting.Indented, 
                new JsonSerializerSettings 
                { 
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }));
        }
    }
}