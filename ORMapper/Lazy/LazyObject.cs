namespace ORMapper.Lazy
{
    public class MyLazyObject<T> : IMyLazy
    {
        public MyLazyObject(object pk)
        {
            _pk = pk;
        }

        private T storage { get; set; }

        private object _pk { get; }

        public T Get
        {
            get
            {
                if (storage is null)
                {
                    storage = Orm.Get<T>(_pk);
                    return storage;
                }

                return storage;
            }
            set => storage = value;
        }
    }
}