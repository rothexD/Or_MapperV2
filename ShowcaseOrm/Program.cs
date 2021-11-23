using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Threading;
using ORMapper;
using ORMapper.Caches;
using ORMapper.CustomQueryImproved;
using OrMapper.Helpers;
using OrMapper.Helpers.extentions;
using ORMapper.Models;
using ShowcaseOrm.Models;
using ShowcaseOrm.Show;

namespace ShowcaseOrm
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Orm.ConnectionString = "Server=127.0.0.1;Port=5438;Database=school;User Id=postgres;Password=postgres;";
            
            ShowCustomFluentQueryApi.Show();
            return;
            ShowMapTablesAndTypes.Show(false);
            ShowInsertStudent.Show();
            ShowModifyStudent.Show();

            
            Show1To1Save.Show();
            Show1ToNSave.Show();
            ShowNtoMSave.Show();

            ShowGetAllCourses.Show();

            ShowModifyInDepth.Show();
            
            
            Showcaching.Show();
            ShowCachingSaving.Show();
            
            ShowHelper.Begin($"Used a total of {Counter.LongTermCounter} connections!");
            ShowHelper.End();
            
            ShowTransaction.Show();
        }
    }
}