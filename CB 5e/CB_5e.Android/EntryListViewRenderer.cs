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
using CB_5e.Droid;
using CB_5e.Views;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Android.Views.InputMethods;

[assembly: ExportRenderer(typeof(EntryListView), typeof(EntryListViewRenderer))]
namespace CB_5e.Droid
{
    public class EntryListViewRenderer: ListViewRenderer
    {

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            var service = (InputMethodManager)Forms.Context.GetSystemService(Context.InputMethodService);
            if (changed || !(service?.IsAcceptingText ?? false))
                base.OnLayout(changed, l, t, r, b);
        }
    }

}