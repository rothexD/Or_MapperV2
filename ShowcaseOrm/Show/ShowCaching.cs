using System;
using ORMapper;
using ShowcaseOrm.Models;


namespace ShowcaseOrm.Show
{
    public static class Showcaching
    {
        public static void Show()
        {
            ShowHelper.Begin("Showing Caching load");

            
            var a = Orm.Get<Teacher>("t.0");
            var b = Orm.Get<Teacher>("t.0");
            
            Console.WriteLine("instance number of teacher t.0 a: "+a.InstanceNumber);
            Console.WriteLine("instance number of teacher t.0 b: "+b.InstanceNumber);
            
            ShowHelper.End();
        }
    }
}