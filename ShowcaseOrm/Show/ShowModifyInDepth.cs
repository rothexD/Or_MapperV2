using System;
using ORMapper;
using ShowcaseOrm.Models;

namespace ShowcaseOrm.Show
{
    public static class ShowModifyInDepth
    {
        public static void Show()
        {
            ShowHelper.Begin("Showing modify in depth");

            var i = Orm.Get<Teacher>("t.2");

            i.Courses[0].Students[0].Grade = 3;
            var safe = i.Courses[0].Students[0].ID;
            
            Orm.Save(i);
            
            Console.WriteLine(Orm.Get<Student>(safe).Grade);
            ShowHelper.End();
        }
    }
}