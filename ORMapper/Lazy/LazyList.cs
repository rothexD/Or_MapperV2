using System.Collections;
using System.Collections.Generic;

namespace ORMapper.Lazy
{
    public class MyLazyList<T> : IMyLazy, IList<T>
    {
        public MyLazyList(object pk = null)
        {
            _pk = pk;
        }

        private List<T> storage { get; set; }

        private object _pk { get; }
        
        protected List<T> Get
        {
            get
            {
                if (!Isloaded)
                {
                    storage = Orm.GetAll<T>(_pk._GetTable().PrimaryKey.GetValue(_pk));
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
        // [interface] IList<T>                                                                                             //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Gets or sets an item by its index.</summary>
        /// <param name="index">Index.</param>
        /// <returns>Item.</returns>
        public T this[int index]
        {
            get { return storage[index]; }
            set { storage[index] = value; }
        }


        /// <summary>Gets the number of items in this list.</summary>
        public int Count
        {
            get { return storage.Count; }
        }


        /// <summary>Gets if the list is read-only.</summary>
        bool ICollection<T>.IsReadOnly
        {
            get { return ((IList<T>) storage).IsReadOnly; }
        }


        /// <summary>Adds an item to the list.</summary>
        /// <param name="item">Item.</param>
        public void Add(T item)
        {
            storage.Add(item);
        }


        /// <summary>Clears the list.</summary>
        public void Clear()
        {
            storage.Clear();
        }


        /// <summary>Returns if the list contains an item.</summary>
        /// <param name="item">Item.</param>
        /// <returns>Returns TRUE if the list contains the item, otherwise returns FALSE.</returns>
        public bool Contains(T item)
        {
            return storage.Contains(item);
        }


        /// <summary>Copies the list to an array.</summary>
        /// <param name="array">Array.</param>
        /// <param name="arrayIndex">Starting index.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            storage.CopyTo(array, arrayIndex);
        }


        /// <summary>Returns an enumerator for this list.</summary>
        /// <returns>Enumerator.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return storage.GetEnumerator();
        }


        /// <summary>Returns the index of an item.</summary>
        /// <param name="item">Item.</param>
        /// <returns>Index.</returns>
        public int IndexOf(T item)
        {
            return storage.IndexOf(item);
        }


        /// <summary>Inserts an item into the list.</summary>
        /// <param name="index">Index.</param>
        /// <param name="item">Item.</param>
        public void Insert(int index, T item)
        {
            storage.Insert(index, item);
        }


        /// <summary>Removes an item from the list.</summary>
        /// <param name="item">Item.</param>
        /// <returns>Returns TRUE if successful, otherwise returns FALSE.</returns>
        public bool Remove(T item)
        {
            return storage.Remove(item);
        }


        /// <summary>Removes an item with a specific index from the list.</summary>
        /// <param name="index">Index.</param>
        public void RemoveAt(int index)
        {
            storage.RemoveAt(index);
        }


        /// <summary>Returns an enumerator for this list.</summary>
        /// <returns>Enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return storage.GetEnumerator();
        }
    }
}