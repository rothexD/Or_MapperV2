using System;
using System.Diagnostics.CodeAnalysis;

namespace ShowcaseOrm.Show
{
    [ExcludeFromCodeCoverage]
    public static class ShowTransaction
    {
        /// <summary>
        /// uses transaction scope not self implemented
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public static void Show()
        {
            ShowHelper.BeginNewShowcase("Showing transactions");
            
            Console.WriteLine("using c# transactions manager, not sure how to fail");
            throw new NotImplementedException();
            ShowHelper.EndNewShowcase();  
        }
    }
}