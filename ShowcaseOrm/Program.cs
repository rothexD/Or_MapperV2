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
            Orm.ConnectionString = "Server=127.0.0.1;Port=5432;Database=remote;User Id=remote_user;Password=remote_password;";

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
            
            ShowHelper.Begin($"Used a total of {Counter.LongTermCounter} connections!");
            ShowHelper.End();
            
            //transactions are supported with transaction scope.. not self implemented
            ShowTransaction.Show();
        }
    }
}