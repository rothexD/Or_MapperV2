namespace ORMapper.Lazy
{
    public class MyLazyObject<T> : IMyLazy
    {
        public MyLazyObject(object pk = null)
        {
            _pk = pk;
        }

        private T storage { get; set; }

        private object _pk { get; }
        public T Get
        {
            get
            {
                if (!Isloaded)
                {
                    storage = Orm.Get<T>(_pk);
                    Isloaded = true;
                    return storage;
                }

                return storage;
            }
            set
            {
                Isloaded = true;
                storage = value;
            }
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // operators                                                                                                        //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Implements an implicit operator for the Lazy class.</summary>
        /// <param name="lazy">Lazy object.</param>
        public static implicit operator T(MyLazyObject<T> lazy)
        {
            return lazy.Get;
        }


        /// <summary>Implements an implicit operator for the Lazy class.</summary>
        /// <param name="lazy">Lazy object.</param>
        public static implicit operator MyLazyObject<T>(T obj)
        {
            MyLazyObject<T> rval = new MyLazyObject<T>();
            rval.Get = obj;

            return rval;
        }
    }
}