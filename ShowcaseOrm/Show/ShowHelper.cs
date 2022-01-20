using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace ShowcaseOrm.Show
{
    [ExcludeFromCodeCoverage]
    public static class ShowHelper
    {
        private static int counter = 0;

        public static void BeginNewShowcase(string showingWhat)
        {
            Console.WriteLine("----------------------------------------------------------------------------------------------");
            Console.WriteLine($"Showing ({counter++}) ---  {showingWhat}");
            Console.WriteLine("----------------------------------------------------------------------------------------------");
            Console.WriteLine();
        }
        public static void EndNewShowcase()
        {
            Console.WriteLine();
        }

        public static void printNewtonsoftJson(object o)
        {
            if (o is null)
            {
                Console.WriteLine("Print input was null");
                return;
            }
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(o, Formatting.Indented, 
                new JsonSerializerSettings 
                { 
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }));
        }
    }
}