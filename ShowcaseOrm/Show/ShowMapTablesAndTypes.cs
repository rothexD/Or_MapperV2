using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ORMapper;
using ShowcaseOrm.Models;

namespace ShowcaseOrm.Show
{
    [ExcludeFromCodeCoverage]
    public class ShowMapTablesAndTypes
    {
        public static void Show(bool automaticInsertIntoDb = false)
        {
            ShowHelper.Begin("Showing OrMapping");
            
            List<Type> tables = new();
            tables.Add(typeof(Gender));
            tables.Add(typeof(Course));
            tables.Add(typeof(Student));
            tables.Add(typeof(STUDENT_COURSES));
            tables.Add(typeof(Class));
            tables.Add(typeof(Teacher));
            
            var x =OrmMapping.Map(tables.ToArray(),automaticInsertIntoDb);
            OrmMapping.ReloadTypes();
            //if set to false:
            //OrmMapping._Print(x);
            //
            
            
            ShowHelper.End();
        }
    }
}