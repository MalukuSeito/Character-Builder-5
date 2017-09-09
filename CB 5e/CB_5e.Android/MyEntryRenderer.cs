using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using CB_5e.Droid;
using Android.Widget;

[assembly: ExportRenderer(typeof(Entry), typeof(MyEntryRenderer))]
namespace CB_5e.Droid
{
    class MyEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            var native = Control as EditText;
            if (e.NewElement.Keyboard == Keyboard.Telephone)
                native.InputType = Android.Text.InputTypes.ClassNumber | Android.Text.InputTypes.NumberFlagSigned | Android.Text.InputTypes.NumberFlagDecimal;
        }
    }
}