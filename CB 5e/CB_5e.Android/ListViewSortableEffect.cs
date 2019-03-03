using System;
using System.Collections;
using System.Collections.Generic;
using Android.Animation;
using Android.Content;
using Android.Database;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Java.Lang;
using CB_5e.Droid;
using CB_5e;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using AViews = Android.Views;
using AWidget = Android.Widget;

[assembly: ResolutionGroupName("CubiSoft")]
[assembly: ExportEffect(typeof(ListViewSortableEffect), "ListViewSortableEffect")]
namespace CB_5e.Droid
{
    
    class ListViewSortableEffect : PlatformEffect
    {
        private DragListAdapter _dragListAdapter = null;

        protected override void OnAttached()
        {
            var element = Element as ListView;

            if (Control is AWidget.ListView listView)
            {
                _dragListAdapter = new DragListAdapter(listView, element);
                listView.Adapter = _dragListAdapter;
                listView.SetOnDragListener(_dragListAdapter);
                listView.OnItemLongClickListener = _dragListAdapter;
            }
        }

        protected override void OnDetached()
        {
            if (Control is AWidget.ListView listView)
            {
                listView.Adapter = _dragListAdapter.WrappedAdapter;

                // TODO: Remove the attached listeners
            }
        }

        protected override void OnElementPropertyChanged(System.ComponentModel.PropertyChangedEventArgs args)
        {
            if (args.PropertyName == Sorting.IsSortableProperty.PropertyName)
            {
                _dragListAdapter.DragDropEnabled = Sorting.GetIsSortable(Element);
            }
        }
    }

    public class DragItem : Java.Lang.Object
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DragItem"/> class.
        /// </summary>
        /// <param name="index">
        /// The initial index for the data item.
        /// </param>
        /// <param name="view">
        /// The view element that is being dragged.
        /// </param>
        /// <param name="dataItem">
        /// The data item that is bound to the view.
        /// </param>
        public DragItem(int index, AViews.View view, object dataItem)
        {
            OriginalIndex = Index = index;
            View = view;
            Item = dataItem;
        }

        /// <summary>
        /// Gets or sets the current index for the data item.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Gets the original index for the data item
        /// </summary>
        public int OriginalIndex { get; }

        /// <summary>
        /// Gets the data item that is being dragged
        /// </summary>
        public object Item { get; }

        /// <summary>
        /// Gets the view that is being dragged
        /// </summary>
        public AViews.View View { get; }
    }

    public class DragListAdapter :
        AWidget.BaseAdapter,
        AWidget.IWrapperListAdapter,
        AViews.View.IOnDragListener,
        AWidget.AdapterView.IOnItemLongClickListener
    {
        private AWidget.IListAdapter _listAdapter;

        private AWidget.ListView _listView;

        private ListView _element;

        private List<AViews.View> _translatedItems = new List<AViews.View>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DragListAdapter"/> class.
        /// </summary>
        /// <param name="listAdapter">
        /// The list adapter.
        /// </param>
        /// <param name="listView">
        /// The list view.
        /// </param>
        /// <param name="element">
        /// The element.
        /// </param>
        public DragListAdapter(AWidget.ListView listView, ListView element)
        {
            _listView = listView;
            _listAdapter = ((AWidget.IWrapperListAdapter)_listView.Adapter).WrappedAdapter;
            _element = element;
        }

        public bool DragDropEnabled { get; set; } = false;

        #region IWrapperListAdapter Members

        public AWidget.IListAdapter WrappedAdapter => _listAdapter;

        public override int Count => WrappedAdapter.Count;

        public override bool HasStableIds => WrappedAdapter.HasStableIds;

        public override bool IsEmpty => WrappedAdapter.IsEmpty;

        public override int ViewTypeCount => WrappedAdapter.ViewTypeCount;

        public override bool AreAllItemsEnabled() => WrappedAdapter.AreAllItemsEnabled();

        public override Java.Lang.Object GetItem(int position)
        {
            return WrappedAdapter.GetItem(position);
        }

        public override long GetItemId(int position)
        {
            return WrappedAdapter.GetItemId(position);
        }

        public override int GetItemViewType(int position)
        {
            return WrappedAdapter.GetItemViewType(position);
        }

        public override Android.Views.View GetView(int position, Android.Views.View convertView, ViewGroup parent)
        {
            var view = WrappedAdapter.GetView(position, convertView, parent);
            view.SetOnDragListener(this);
            return view;
        }

        public override bool IsEnabled(int position)
        {
            return WrappedAdapter.IsEnabled(position);
        }

        private DataSetObserver _observer;

        public override void RegisterDataSetObserver(DataSetObserver observer)
        {
            _observer = observer;
            base.RegisterDataSetObserver(observer);
            WrappedAdapter.RegisterDataSetObserver(observer);
        }

        public override void UnregisterDataSetObserver(DataSetObserver observer)
        {
            base.UnregisterDataSetObserver(observer);
            WrappedAdapter.UnregisterDataSetObserver(observer);
        }

        #endregion

        public bool OnDrag(AViews.View v, DragEvent e)
        {
            switch (e.Action)
            {
                case DragAction.Started:
                    break;
                case DragAction.Entered:
                    System.Diagnostics.Debug.WriteLine($"DragAction.Entered from {v.GetType()}");

                    if (!(v is AWidget.ListView))
                    {
                        var dragItem = (DragItem)e.LocalState;

                        var targetPosition = InsertOntoView(v, dragItem);

                        dragItem.Index = targetPosition;

                        // Keep a list of items that has translation so we can reset
                        // them once the drag'n'drop is finished.
                        _translatedItems.Add(v);
                        _listView.Invalidate();
                    }
                    break;
                case DragAction.Location:
                    //_currentPosition = (int)e.GetY();
                    //System.Diagnostics.Debug.WriteLine($"DragAction.Location from {v.GetType()} => {_currentPosition}");
                    break;
                case DragAction.Exited:
                    System.Diagnostics.Debug.WriteLine($"DragAction.Entered from {v.GetType()}");

                    if (!(v is AWidget.ListView))
                    {
                        var positionEntered = GetListPositionForView(v);
                        var item1 = _listAdapter.GetItem(positionEntered);

                        System.Diagnostics.Debug.WriteLine($"DragAction.Exited index {positionEntered}");
                    }
                    break;
                case DragAction.Drop:


                    System.Diagnostics.Debug.WriteLine($"DragAction.Drop from {v.GetType()}");

                    //}

                    break;
                case DragAction.Ended:
                    if (!(v is AWidget.ListView))
                    {
                        return false;
                    }

                    System.Diagnostics.Debug.WriteLine($"DragAction.Ended from {v.GetType()}");

                    var mobileItem = (DragItem)e.LocalState;

                    mobileItem.View.Visibility = ViewStates.Visible;

                    foreach (var view in _translatedItems)
                    {
                        view.TranslationY = 0;
                    }

                    _translatedItems.Clear();

                    if (_element.ItemsSource is IOrderable orderable)
                    {
                        orderable.ChangeOrdinal(mobileItem.OriginalIndex, mobileItem.Index);
                    }

                    break;
            }

            return true;
        }

        /// <summary>
        /// Handler for Long Click event from <paramref name="view"/>
        /// </summary>
        /// <param name="parent">
        /// The parent list view .
        /// </param>
        /// <param name="view">
        /// The view that triggered the long click event
        /// </param>
        /// <param name="position">
        /// The position of the view in the list (has to be normalized, includes headers).
        /// </param>
        /// <param name="id">
        /// The id of the item that triggered the event (must be bigger than 0 under normal circumstances).
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> flag that identifies whether the event is handled.
        /// </returns>
        public bool OnItemLongClick(AWidget.AdapterView parent, AViews.View view, int position, long id)
        {
            var selectedItem = ((IList)_element.ItemsSource)[(int)id];

            DragItem dragItem = new DragItem(NormalizeListPosition(position), view, selectedItem);

            var data = ClipData.NewPlainText(string.Empty, string.Empty);

            AViews.View.DragShadowBuilder shadowBuilder = new AViews.View.DragShadowBuilder(view);

            view.Visibility = ViewStates.Invisible;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
            {
                view.StartDragAndDrop(data, shadowBuilder, dragItem, 0);
            }
            else
            {
                view.StartDrag(data, shadowBuilder, id, 0);
            }

            return true;
        }

        private int InsertOntoView(AViews.View view, DragItem item)
        {
            var positionEntered = GetListPositionForView(view);
            var correctedPosition = positionEntered;

            // If the view already has a translation, we need to adjust the position
            // If the view has a positive translation, that means that the current position
            // is actually one index down then where it started.
            // If the view has a negative translation, that means it actually moved
            // up previous now we will need to move it down.
            if (view.TranslationY > 0)
            {
                correctedPosition += 1;
            }
            else if (view.TranslationY < 0)
            {
                correctedPosition -= 1;
            }

            // If the current index of the dragging item is bigger than the target
            // That means the dragging item is moving up, and the target view should
            // move down, and vice-versa
            var translationCoef = item.Index > correctedPosition ? 1 : -1;

            // We translate the item as much as the height of the drag item (up or down)
            var translationTarget = view.TranslationY + (translationCoef * item.View.Height);

            ObjectAnimator anim = ObjectAnimator.OfFloat(view, "TranslationY", view.TranslationY, translationTarget);
            anim.SetDuration(100);
            anim.Start();

            return correctedPosition;
        }

        private int GetListPositionForView(AViews.View view)
        {
            return NormalizeListPosition(_listView.GetPositionForView(view));
        }

        private int NormalizeListPosition(int position)
        {
            return position - _listView.HeaderViewsCount;
        }

    }
}