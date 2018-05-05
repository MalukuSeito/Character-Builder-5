using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using CB_5e.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(ScrollableTabbedPageRenderer))]
namespace CB_5e.Droid
{
    public class ScrollableTabbedPageRenderer : TabbedPageRenderer
    {
        public ScrollableTabbedPageRenderer()
        {
        }
        public ScrollableTabbedPageRenderer(Context context) : base(context)
        {
        }
        public override void OnViewAdded(Android.Views.View child)
        {
            base.OnViewAdded(child);
            if (child is TabLayout tabLayout) tabLayout.TabMode = TabLayout.ModeScrollable;
        }

    }
}