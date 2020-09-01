using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using CB_5e.iOS;
using Xamarin.Forms.Xaml;
using UIKit;
using CB_5e.Views.Character;

[assembly: ExportRenderer(typeof(FlowPage), typeof(ModalPageRenderer))]
namespace CB_5e.iOS
{
    public class ModalPageRenderer : PageRenderer
    {
        public override void WillMoveToParentViewController(UIViewController parent)
        {
            if (parent != null) parent.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
            base.WillMoveToParentViewController(parent);
        }
    }
}
