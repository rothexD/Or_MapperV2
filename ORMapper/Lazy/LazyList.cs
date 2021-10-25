using System.Collections.Generic;

namespace ORMapper.Lazy
{
    public class MyLazyList<T> : IMyLazy
    {
        public MyLazyList(object pk)
        {
            _pk = pk;
        }

        private List<T> storage { get; set; }

        private object _pk { get; }


        public List<T> Get
        {
            get
            {
                if (storage is null)
                {
                    storage = Orm.GetAll<T>(_pk);
                    return storage;
                }

                return storage;
            }
            set => storage = value;
        }
    }
}