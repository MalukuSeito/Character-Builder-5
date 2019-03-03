using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB_5e
{
    public class ObservableListGroupCollection<TKey, TItem> : ObservableListCollection<ObservableListGroup<TKey, TItem>>
    {
        private Func<TItem, TKey> _groupKeySelector;

        private IEqualityComparer<TItem> _equivalenceComparer;

        private Func<TItem, TItem, bool> _updateFunction;

        public ObservableListGroupCollection(
            IEnumerable<TItem> collection,
            Func<TItem, TKey> keySelector,
            IEqualityComparer<TItem> equivalence = null,
            Func<TItem, TItem, bool> updateCallback = null)
        {
            _groupKeySelector = keySelector;
            _equivalenceComparer = equivalence;
            _updateFunction = updateCallback;

            UpdateItems(collection);
        }

        public void UpdateItems(IEnumerable<TItem> items)
        {
            var grouppedItems = items.GroupBy(_groupKeySelector);

            var startIndex = Count;

            var newItems = new List<ObservableListGroup<TKey, TItem>>();

            foreach (var group in grouppedItems)
            {
                var section = Items.FirstOrDefault(item => item.Key.Equals(group.Key));

                if (section == null)
                {
                    var newGroup = new ObservableListGroup<TKey, TItem>(group.Key, group, _equivalenceComparer, _updateFunction);
                    newItems.Add(newGroup);
                    Items.Add(newGroup);
                }
                else
                {
                    section.UpdateRange(group);
                }
            }

            if (newItems.Any())
            {
                OnPropertyChanged(new PropertyChangedEventArgs("Count"));
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItems, startIndex));
            }
        }
    }

    /// <summary>
    /// Grouping of items
    /// </summary>
    public class ObservableListGroup<TKey, TItem> : ObservableListCollection<TItem>
    {
        public TKey Key { get; }

        public ObservableListGroup(TKey key, IEnumerable<TItem> items, IEqualityComparer<TItem> equivalence = null, Func<TItem, TItem, bool> updateCallback = null)
            : base(Enumerable.Empty<TItem>(), equivalence, updateCallback)
        {
            Key = key;
            UpdateRange(items);
        }
    }

    /// <summary>
    /// Used by bound collections to expose ordering methods/events
    /// </summary>
    public interface IOrderable
    {
        /// <summary>
        /// Event fired when the items in the collection are re-ordered.
        /// </summary>
        event EventHandler OrderChanged;

        /// <summary>
        /// Used to change the item orders in an enumerable
        /// </summary>
        /// <param name="oldIndex">
        /// The old index of the item.
        /// </param>
        /// <param name="newIndex">
        /// The new index of the item.
        /// </param>
        void ChangeOrdinal(int oldIndex, int newIndex);
    }

    /// <summary>
    /// ObservableCollection implementation for updating and adding items
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public class ObservableListCollection<T> : ObservableCollection<T>, IOrderable
    {
        private IEqualityComparer<T> _equivalenceComparer;

        private Func<T, T, bool> _updateFunction;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableListCollection{T}"/> class.
        /// </summary>
        public ObservableListCollection() : base()
        {
        }
        public event EventHandler OrderChanged;

        public void ChangeOrdinal(int oldIndex, int newIndex)
        {
            var priorIndex = oldIndex;
            var latterIndex = newIndex;

            var changedItem = Items[oldIndex];
            if (newIndex < oldIndex)
            {
                // add one to where we delete, because we're increasing the index by inserting
                priorIndex += 1;
            }
            else
            {
                // add one to where we insert, because we haven't deleted the original yet
                latterIndex += 1;
            }

            Items.Insert(latterIndex, changedItem);
            Items.RemoveAt(priorIndex);

            OrderChanged?.Invoke(this, EventArgs.Empty);

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, changedItem, newIndex, oldIndex));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableListCollection{T}"/> class.
        /// </summary>
        /// <param name="collection">collection: The collection from which the elements are copied.</param>
        /// <exception cref="System.ArgumentNullException">The collection parameter cannot be null.</exception>
        public ObservableListCollection(IEnumerable<T> collection, IEqualityComparer<T> equivalence = null, Func<T, T, bool> updateCallback = null)
            : base(collection)
        {
            _equivalenceComparer = equivalence ?? EqualityComparer<T>.Default;
            _updateFunction = updateCallback;
        }

        /// <summary>
        /// Updates or adds the elements of the specified collection to the end of the ObservableCollection(Of T).
        /// </summary>
        /// <param name="collection">
        /// The collection to be added.
        /// </param>
        public void UpdateRange(IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            CheckReentrancy();

            int startIndex = Count;

            var updatedItems = collection.Where(item => Items.Any(contact => _equivalenceComparer.Equals(contact, item))).ToList();

            bool anyItemUpdated = false;

            foreach (var updateItem in updatedItems)
            {
                var existingItem = Items.FirstOrDefault(contact => _equivalenceComparer.Equals(contact, updateItem));

                // TODO: We can fire NotifyCollectionChanged.Update if needed depending on anyItemUpdated.
                anyItemUpdated = anyItemUpdated | _updateFunction?.Invoke(existingItem, updateItem) ?? false;
            }

            var newItems = collection.Where(item => !Items.Any(contact => _equivalenceComparer.Equals(contact, item))).ToList();

            if (!newItems.Any())
            {
                return;
            }

            foreach (var item in newItems)
            {
                Items.Add(item);
            }

            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItems, startIndex));
        }
    }
}
