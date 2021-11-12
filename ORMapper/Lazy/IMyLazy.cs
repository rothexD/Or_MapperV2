namespace ORMapper.Lazy
{
    public abstract class IMyLazy
    {
        public bool Isloaded { get; protected set; } = false;
    }
}