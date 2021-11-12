using System;

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
    }
}