using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tfo.Utils.Io.Collections
{
    /// <summary>
    /// Allows a class to provide a key value for the <i>KeyedBindingList</i>
    /// </summary>
    public interface IKeyedItem<out T>
    {
        /// <summary>
        /// The unique key to use
        /// </summary>
        T Key { get; }
    }

    public interface IView
    {
        /// <summary>
        /// Copies all relevant properties from the specified target
        /// </summary>        
        void CopyFrom(IView view);
    }

    public interface IKeyedCollection<TKey, T> : IList<T> where T : class, IKeyedItem<TKey>, IView
    {
        T Find(TKey key);

        void ApplyChanges(IKeyedCollection<TKey, T> updatedSource);

        void ApplyChanges(IKeyedCollection<TKey, T> updatedSource, bool deleteNonMatching);

        void Commit();
    }

    /// <summary>
    /// A binding list supporting finding items by a unique key.  Note that a dynamic key (i.e. that changes after being added to the list) isn't supported.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class KeyedBindingList<TKey, T> : BindingList<T>, IKeyedCollection<TKey, T> where T : class, IKeyedItem<TKey>, IView
    {
        private readonly Dictionary<TKey, T> keyMap = new Dictionary<TKey, T>();

        public KeyedBindingList()
            : base()
        {

        }

        public KeyedBindingList(IList<T> list)
            : base(list)
        {
            foreach (T item in list)
                keyMap[item.Key] = item;
        }

        void IKeyedCollection<TKey, T>.Commit()
        {
        }

        protected override void OnAddingNew(AddingNewEventArgs e)
        {
            base.OnAddingNew(e);

            T newObj = e.NewObject as T;
            if (newObj != null)
            {
                keyMap[newObj.Key] = newObj;
            }
        }

        protected override void ClearItems()
        {
            base.ClearItems();
            keyMap.Clear();
        }

        /// <summary>
        /// Locates the item in the list whose key matches the specified value
        /// </summary>
        public T Find(TKey key)
        {
            T item;
            keyMap.TryGetValue(key, out item);
            return item;
        }

        protected override void InsertItem(int index, T item)
        {
            base.InsertItem(index, item);
            keyMap[item.Key] = item;
        }

        protected override void RemoveItem(int index)
        {
            if (index >= 0 && index < Count)
            {
                T item = this[index];
                keyMap.Remove(item.Key);
            }
            base.RemoveItem(index);
        }

        protected override void SetItem(int index, T item)
        {
            if (index >= 0 && index < Count)
            {
                T existingItem = this[index];
                keyMap.Remove(existingItem.Key);
                keyMap[item.Key] = item;
            }
            base.SetItem(index, item);
        }

        public void Upsert(T item)
        {
            Upsert(item, null);
        }

        public void Upsert(T item, Func<T, T, bool> skipExistingItem)
        {
            T existingItem = Find(item.Key);
            if (existingItem == null)
                Add(item);
            else
            {
                if (skipExistingItem == null || !skipExistingItem(item, existingItem))
                    existingItem.CopyFrom(item);
            }
        }

        public bool RemoveByKey(T item)
        {
            T existingItem = Find(item.Key);
            if (existingItem == null)
                return false;
            else
            {
                return Remove(existingItem);
            }
        }

        /// <summary>
        /// Applies all changes from the specified updated source to the existing source
        /// </summary>   
        public void ApplyChanges(IKeyedCollection<TKey, T> updatedSource)
        {
            ApplyChanges(updatedSource, true);
        }

        /// <summary>
        /// Applies all changes from the specified updated source to the existing source
        /// </summary>        
        /// <param name="deleteNonMatching">If true, will delete anything in the existing source that isn't found in the updated source.</param>
        /// <param name="updatedSource"></param>
        public void ApplyChanges(IKeyedCollection<TKey, T> updatedSource, bool deleteNonMatching)
        {
            // For efficiency, if there are no changes to apply and we're not deleting non-matching items, skip the loop/find checks
            if (!deleteNonMatching && updatedSource.Count == 0)
                return;

            // Loop through the existing data structure and attempt to find the matching ticker so
            // as to update the row instead of rebinding the grid each time
            for (int i = Count - 1; i >= 0; i--)
            {
                T existingItem = this[i];
                T updatedItem = updatedSource.Find(existingItem.Key);
                if (updatedItem != null)
                    existingItem.CopyFrom(updatedItem);
                else if (deleteNonMatching)
                    RemoveAt(i);
            }

            // Now loop through the updated source and see what needs to be added to the existing source
            foreach (T updatedItem in updatedSource.Where(updatedItem => Find(updatedItem.Key) == null))
            {
                Add(updatedItem);
            }
        }
    }
}

