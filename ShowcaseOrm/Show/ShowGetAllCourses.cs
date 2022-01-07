using System;
using System.Diagnostics.CodeAnalysis;
using ORMapper;
using ShowcaseOrm.Models;

namespace ShowcaseOrm.Show
{
    [ExcludeFromCodeCoverage]
    public static class ShowGetAllCourses
    {
        public static void Show()
        {
            ShowHelper.Begin("Get all Students");
            
            var i = Orm.GetAll<Course>();
            ShowHelper.printNewtonsoftJson(i);
        }
    }
}