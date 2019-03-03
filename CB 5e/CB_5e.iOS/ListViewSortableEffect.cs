using System;
using System.Linq;
using CoreGraphics;
using Foundation;
using CB_5e.iOS;
using CB_5e;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ResolutionGroupName("CubiSoft")]
[assembly: ExportEffect(typeof(ListViewSortableEffect), "ListViewSortableEffect")]
namespace CB_5e.iOS
{
    public class ListViewSortableEffect : PlatformEffect
    {
        internal UITableView TableView
        {
            get
            {
                return Control as UITableView;
            }
        }

        protected override void OnAttached()
        {
            if (TableView == null)
            {
                return;
            }
            var isSortable = Sorting.GetIsSortable(Element);

            TableView.Source = new ListSortableTableSource(TableView.Source, Element as ListView);
            TableView.SetEditing(isSortable, true);
        }

        protected override void OnDetached()
        {
            if (TableView != null)
            {
                TableView.Source = (TableView.Source as ListSortableTableSource)?.OriginalSource;
                TableView.SetEditing(false, true);
            }
        }

        protected override void OnElementPropertyChanged(System.ComponentModel.PropertyChangedEventArgs args)
        {
            if (args.PropertyName == Sorting.IsSortableProperty.PropertyName)
            {
                TableView.SetEditing(Sorting.GetIsSortable(Element), true);
            }
        }
    }

    public class ListSortableTableSource : UITableViewSource
    {
        private UITableViewSource _originalSource;

        private ListView _formsElement;

        public ListSortableTableSource(UITableViewSource source, ListView element)
        {
            _originalSource = source;
            _formsElement = element;
        }

        public UITableViewSource OriginalSource
        {
            get
            {
                return _originalSource;
            }
        }

        public override UITableViewCellEditingStyle EditingStyleForRow(UITableView tableView, NSIndexPath indexPath)
        {
            // We do not want the "-" icon near each row (or the "+" icon)
            return UITableViewCellEditingStyle.None;
        }

        public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
        {
            // We still want the row to be editable
            return true;
        }

        public override bool CanMoveRow(UITableView tableView, NSIndexPath indexPath)
        {
            // We do want each row to be movable
            return true;
        }

        public override bool ShouldIndentWhileEditing(UITableView tableView, NSIndexPath indexPath)
        {
            // We do not want the "weird" indent for the rows when they are in editable mode.
            return false;
        }

        public override void MoveRow(UITableView tableView, NSIndexPath sourceIndexPath, NSIndexPath destinationIndexPath)
        {
            if (_formsElement.ItemsSource is IOrderable orderableList)
            {
                var deleteAt = sourceIndexPath.Row;
                var insertAt = destinationIndexPath.Row;

                orderableList.ChangeOrdinal(deleteAt, insertAt);
            }
        }
        //public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, Foundation.NSIndexPath indexPath)
        //{
        //    switch (editingStyle)
        //    {
        //        case UITableViewCellEditingStyle.Delete:
        //            break;
        //        case UITableViewCellEditingStyle.None:
        //            break;
        //    }
        //}

        //public override NSIndexPath CustomizeMoveTarget(UITableView tableView, NSIndexPath sourceIndexPath, NSIndexPath proposedIndexPath)
        //{
        //    var numRows = tableView.NumberOfRowsInSection(0);
        //    if (proposedIndexPath.Row >= numRows)
        //        return NSIndexPath.FromRowSection(numRows, 0);
        //    else
        //        return proposedIndexPath;
        //}

        //public override void DraggingEnded(UIScrollView scrollView, bool willDecelerate)
        //{
        //    _originalSource.DraggingEnded(scrollView, willDecelerate);
        //}

        //public override void DraggingStarted(UIScrollView scrollView)
        //{
        //    _originalSource.DraggingStarted(scrollView);
        //}

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            return OriginalSource.GetCell(tableView, indexPath);
        }

        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            return OriginalSource.GetHeightForHeader(tableView, section);
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return 44;
            //return OriginalSource.GetHeightForRow(tableView, indexPath);
        }

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            return OriginalSource.GetViewForHeader(tableView, section);
        }

        public override void HeaderViewDisplayingEnded(UITableView tableView, UIView headerView, nint section)
        {
            OriginalSource.HeaderViewDisplayingEnded(tableView, headerView, section);
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return OriginalSource.NumberOfSections(tableView);
        }

        public override void RowDeselected(UITableView tableView, NSIndexPath indexPath)
        {
            OriginalSource.RowDeselected(tableView, indexPath);
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            OriginalSource.RowSelected(tableView, indexPath);
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return OriginalSource.RowsInSection(tableview, section);
        }

        public override void Scrolled(UIScrollView scrollView)
        {
            OriginalSource.Scrolled(scrollView);
        }

        public override string[] SectionIndexTitles(UITableView tableView)
        {
            return OriginalSource.SectionIndexTitles(tableView);
        }
    }

}