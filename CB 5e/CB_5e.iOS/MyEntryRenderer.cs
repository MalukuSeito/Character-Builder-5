
using CB_5e.iOS;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Entry), typeof(MyEntryRenderer))]
namespace CB_5e.iOS
{
    class MyEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);


            if (e?.NewElement?.Keyboard == Keyboard.Telephone)
            {
                Control.KeyboardType = UIKeyboardType.DecimalPad;
                var toolbar = new UIToolbar(new CGRect(0.0f, 0.0f, Control.Frame.Size.Width, 44.0f))
                {
                    Items = new[]
                    {
                        new UIBarButtonItem("-", UIBarButtonItemStyle.Plain, (o, ee) => { Control.Text += "-"; }),
                        new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
                        new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate { Control.ResignFirstResponder(); })
                    }
                };

                this.Control.InputAccessoryView = toolbar;
            } else if (e?.NewElement?.Keyboard == Keyboard.Numeric)
            {
                var toolbar = new UIToolbar(new CGRect(0.0f, 0.0f, Control.Frame.Size.Width, 44.0f))
                {
                    Items = new[]
                    {
                        new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
                        new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate { Control.ResignFirstResponder(); })
                    }
                };

                this.Control.InputAccessoryView = toolbar;
            }
        }
    }
}