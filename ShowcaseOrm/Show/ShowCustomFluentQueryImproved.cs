using System.Diagnostics.CodeAnalysis;
using ORMapper.CustomQueryImproved;
using OrMapper.ExternalModels;
using OrMapper.Helpers.extentions;
using ShowcaseOrm.Models;

namespace ShowcaseOrm.Show
{
    [ExcludeFromCodeCoverage]
    public class ShowCustomFluentQueryImproved
    {
        public static void Show()
        {
            ShowHelper.BeginNewShowcase("Showing customfluentquery with integer");
            var x = CustomGet<Teacher>.Create().Where()
                .Equals("Salary", SecureParameter.Create(50000)).Execute();
            ShowHelper.printNewtonsoftJson(x);


            ShowHelper.BeginNewShowcase("Showing customfluentquery with caseinsentive name");
            var y = CustomGet<Teacher>.Create().Where()
                .Equals("Name", "ZAPPELFISCH".MakeSecure().MakeCaseIns()).Execute();
            ShowHelper.printNewtonsoftJson(y);
            //throw new NotImplementedException("custom fluent query not implemented");
            ShowHelper.EndNewShowcase();
        }
    }
}