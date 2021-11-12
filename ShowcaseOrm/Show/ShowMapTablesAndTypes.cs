using System;
using System.Collections.Generic;
using ORMapper;
using ShowcaseOrm.Models;

namespace ShowcaseOrm.Show
{
    public class ShowMapTablesAndTypes
    {
        public static void Show(bool PrintToConsole = true)
        {
            ShowHelper.Begin("Showing OrMapping");
            
            List<Type> tables = new();
            tables.Add(typeof(Gender));
            tables.Add(typeof(Course));
            tables.Add(typeof(Student));
            tables.Add(typeof(STUDENT_COURSES));
            tables.Add(typeof(Class));
            tables.Add(typeof(Teacher));
            
            OrmMapping.Map(tables.ToArray(),PrintToConsole);
            
            ShowHelper.End();
        }
    }
}