using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CB_5e.Helpers
{
    public class ExtendedFlexLayout : FlexLayout
    {
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(ExtendedFlexLayout), propertyChanged: OnItemsSourceChanged);
        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(ExtendedFlexLayout));

        private INotifyCollectionChanged Col;

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        static void OnItemsSourceChanged(BindableObject bindable, object oldVal, object newVal)
        {
            var layout = (ExtendedFlexLayout)bindable;
            layout.BindingChanged(newVal);
        }
        private void BindingChanged(object newVal)
        {
            if (Col != null)
            {
                Col.CollectionChanged -= Col_CollectionChanged;
            }
            Children.Clear();
            if (newVal is IEnumerable newValue)
            {
                foreach (var item in newValue)
                {
                    Children.Add(CreateChildView(item));
                }
            }
            if (newVal is INotifyCollectionChanged col)
            {
                Col = col;
                col.CollectionChanged += Col_CollectionChanged;
            }
        }

        private void Col_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (Col is IEnumerable newValue)
            {
                foreach (var item in newValue)
                {
                    Children.Add(CreateChildView(item));
                }
            }
        }

        View CreateChildView(object item)
        {
            ItemTemplate.SetValue(BindableObject.BindingContextProperty, item);
            return (View)ItemTemplate.CreateContent();
        }
    }
}
