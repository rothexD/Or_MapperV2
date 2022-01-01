namespace OrMapper.Helpers.extentions
{
    /// <summary>
    /// defines a static global counter
    /// </summary>
    public static class Counter
    {
        public static int CounterI;
        public static int LongTermCounter { get; private set; }

        public static void IncrementLongTermCounter()
        {
            LongTermCounter++;
        }
    }
}