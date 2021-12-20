using System;
using ORMapper.CustomQueryImproved;
using OrMapper.ExternalModels;
using OrMapper.Helpers.FluentSqlQueryApi;
using ShowcaseOrm.Models;

namespace ShowcaseOrm.Show
{
    public class ShowCustomFluentQueryImproved
    {
        public static void Show()
        {
            ShowHelper.Begin("Showing with integer");
            var x = CustomQueryImproved.Create().Where()
                .Equals("Salary",SecureParameter.Create(50000)).GetAllMatches<Teacher>();
            ShowHelper.printNewtonsoftJson(x);    
            
            
            ShowHelper.Begin("Showing with caseinsentive name");
            var y = CustomQueryImproved.Create().Where()
                .Equals("Name","ZAPPELFISCH".MakeSecure().MakeCaseIns()).GetAllMatches<Teacher>();
            ShowHelper.printNewtonsoftJson(y);
            //throw new NotImplementedException("custom fluent query not implemented");
            ShowHelper.End();
        }
    }
}