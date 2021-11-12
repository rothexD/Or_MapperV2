using System;

namespace ShowcaseOrm.Show
{
    public static class ShowTransaction
    {
        public static void Show()
        {
            ShowHelper.Begin("Showing transactions");
            throw new NotImplementedException();
            ShowHelper.End();  
        }
    }
}