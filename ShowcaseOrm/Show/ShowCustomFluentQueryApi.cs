using System;
using OrMapper.Helpers.FluentSqlQueryApi;

namespace ShowcaseOrm.Show
{
    public static class ShowCustomFluentQueryApi
    {
        public static void Show()
        {
            ShowHelper.Begin("building a custom fluent query");
            
            string[] test = {"a", "b"};
            (var para, string sqlstring) = CustomQuery.Create("@").Select(test).From("student")
                .Join("hans", "hans.ID", "student.Id").Where()
                .Equals(SecureParameter.Create(test), "test").And().Greater(2, 1).Build();
            Console.WriteLine("sql: " + Environment.NewLine + sqlstring);
            Console.WriteLine("paras");
            para.ForEach(x => Console.WriteLine("key: " + x.Item1 + " ,Value: " + x.Item2));
            
            
            
            
            string[] test2 = {"grade"};
            (var para2, string sqlstring2) = CustomQuery.Create("@").Select(test2).From("students")
                .Where()
                .Equals(SecureParameter.Create(null), "name").Build();
            Console.WriteLine("sql: " + Environment.NewLine + sqlstring2);
            Console.WriteLine("paras");
            para2.ForEach(x => Console.WriteLine("key: " + x.Item1 + " ,Value: " + x.Item2));
            
            ShowHelper.End();
        }
    }
}