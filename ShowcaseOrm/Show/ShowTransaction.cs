using System;

namespace ShowcaseOrm.Show
{
    public static class ShowTransaction
    {
        public static void Show()
        {
            ShowHelper.Begin("Showing transactions");
            
            Console.WriteLine("using c# transactions manager, not sure how to fail");
            throw new NotImplementedException();
            ShowHelper.End();  
        }
    }
}