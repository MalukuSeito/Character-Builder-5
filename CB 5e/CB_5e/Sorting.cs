using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CB_5e
{
    public static class Sorting
    {
        public static readonly BindableProperty IsSortableProperty =
            BindableProperty.CreateAttached("IsSortable", typeof(bool), typeof(ListViewSortableEffect), false,
                propertyChanged: OnIsSortableChanged);

        public static bool GetIsSortable(BindableObject view)
        {
            return (bool)view.GetValue(IsSortableProperty);
        }

        public static void SetIsSortable(BindableObject view, bool value)
        {
            view.SetValue(IsSortableProperty, value);
        }

        static void OnIsSortableChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as ListView;
            if (view == null)
            {
                return;
            }

            if (!view.Effects.Any(item => item is ListViewSortableEffect))
            {
                view.Effects.Add(new ListViewSortableEffect());
            }
        }

        class ListViewSortableEffect : RoutingEffect
        {
            public ListViewSortableEffect() : base("CubiSoft.ListViewSortableEffect")
            {

            }
        }
    }
}
