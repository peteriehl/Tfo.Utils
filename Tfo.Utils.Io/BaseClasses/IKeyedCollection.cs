using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tfo.Utils.Io.BaseClasses
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
}
