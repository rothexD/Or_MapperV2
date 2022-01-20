using System.Diagnostics.CodeAnalysis;
using ORMapper;
using OrMapper.Helpers.extentions;
using ShowcaseOrm.Show;

namespace ShowcaseOrm
{
    [ExcludeFromCodeCoverage]
    internal class Program
    {
        private static void Main(string[] args)
        {
            Orm.ConnectionString =
                "Server=127.0.0.1;Port=5432;Database=remote;User Id=remote_user;Password=remote_password;";

            ShowMapTablesAndTypes.Show(true);

            ShowInsertStudent.Show();
            ShowModifyStudent.Show();


            Show1To1Save.Show();
            Show1ToNSave.Show();
            ShowNtoMSave.Show();

            ShowGetAllCourses.Show();

            ShowModifyInDepth.Show();


            Showcaching.Show();
            ShowCachingSaving.Show();

            ShowCustomFluentQueryImproved.Show();

            ShowHelper.BeginNewShowcase($"Used a total of {Counter.LongTermCounter} connections!");
            ShowHelper.EndNewShowcase();

            ShowDeleteFromDb.Show();

            //transactions are supported with transaction scope
            ShowTransaction.Show();
        }
    }
}