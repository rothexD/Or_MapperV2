using System;

namespace ShowcaseOrm.Show
{
    public static class ShowTransaction
    {
        public static void Show()
        {
            ShowHelper.Begin("Showing modify in depth");
            throw new NotImplementedException();
            ShowHelper.End();  
        }
    }
}