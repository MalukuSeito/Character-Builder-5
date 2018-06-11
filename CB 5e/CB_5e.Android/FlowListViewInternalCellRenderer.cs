using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DLToolkit.Forms.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using CB_5e.Droid;

[assembly: ExportRenderer(typeof(FlowListViewInternalCell), typeof(FlowListViewInternalCellRenderer))]
namespace CB_5e.Droid
{
    public class FlowListViewInternalCellRenderer : ViewCellRenderer
    {
        protected override Android.Views.View GetCellCore(Cell item, Android.Views.View convertView, Android.Views.ViewGroup parent, Android.Content.Context context)
        {
            var cell = base.GetCellCore(item, convertView, parent, context);

            var listView = parent as Android.Widget.ListView;

            if (listView != null)
            {
                listView.SetSelector(Android.Resource.Color.Transparent);
                listView.CacheColorHint = Android.Graphics.Color.Transparent;
            }

            return cell;
        }
    }
}