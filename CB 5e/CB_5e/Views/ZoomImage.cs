using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CB_5e.Views
{
    public class ZoomImage : Image
    {
        private const double MIN_SCALE = 1;
        private const double MAX_SCALE = 4;
        private const double OVERSHOOT = 0.15;
        private double StartScale;
        private double LastX, LastY;

        public byte[] Data { get; set; }
        public string Name { get; set; }

        public ZoomImage()
        {
            var pinch = new PinchGestureRecognizer();
            pinch.PinchUpdated += OnPinchUpdated;
            GestureRecognizers.Add(pinch);

            var pan = new PanGestureRecognizer();
            pan.PanUpdated += OnPanUpdated;
            GestureRecognizers.Add(pan);

            var tap = new TapGestureRecognizer { NumberOfTapsRequired = 2 };
            tap.Tapped += OnTapped;
            GestureRecognizers.Add(tap);

            var tap2 = new TapGestureRecognizer { NumberOfTapsRequired = 1 };
            tap2.Tapped += OnShare;
            GestureRecognizers.Add(tap2);

            Scale = MIN_SCALE;
            TranslationX = TranslationY = 0;
            AnchorX = AnchorY = 0;
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            Scale = MIN_SCALE;
            TranslationX = TranslationY = 0;
            AnchorX = AnchorY = 0;
            return base.OnMeasure(widthConstraint, heightConstraint);
        }

        private void OnTapped(object sender, EventArgs e)
        {
            if (Scale > MIN_SCALE)
            {
                this.ScaleTo(MIN_SCALE, 250, Easing.CubicInOut);
                this.TranslateTo(0, 0, 250, Easing.CubicInOut);
            }
            else
            {
                AnchorX = AnchorY = 0.5; //TODO tapped position
                this.ScaleTo(MAX_SCALE, 250, Easing.CubicInOut);
            }
        }

        private void OnShare(object sender, EventArgs e)
        {
            if (Data != null)
            {
                DependencyService.Get<IDocumentViewer>().ShowDocumentFile(Name != null ? Name : "Image", new MemoryStream(Data), "image/jpeg");
            }
        }

        private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            if (Scale > MIN_SCALE)
                switch (e.StatusType)
                {
                    case GestureStatus.Started:
                        LastX = TranslationX;
                        LastY = TranslationY;
                        break;
                    case GestureStatus.Running:
                        TranslationX = Clamp(LastX + e.TotalX * Scale, -Width / 2, Width / 2);
                        TranslationY = Clamp(LastY + e.TotalY * Scale, -Height / 2, Height / 2);
                        break;
                }
        }

        private void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            switch (e.Status)
            {
                case GestureStatus.Started:
                    StartScale = Scale;
                    AnchorX = e.ScaleOrigin.X;
                    AnchorY = e.ScaleOrigin.Y;
                    break;
                case GestureStatus.Running:
                    double current = Scale + (e.Scale - 1) * StartScale;
                    Scale = Clamp(current, MIN_SCALE * (1 - OVERSHOOT), MAX_SCALE * (1 + OVERSHOOT));
                    break;
                case GestureStatus.Completed:
                    if (Scale > MAX_SCALE)
                        this.ScaleTo(MAX_SCALE, 250, Easing.SpringOut);
                    else if (Scale < MIN_SCALE)
                        this.ScaleTo(MIN_SCALE, 250, Easing.SpringOut);
                    break;
            }
        }

        private T Clamp<T>(T value, T minimum, T maximum) where T : IComparable
        {
            if (value.CompareTo(minimum) < 0)
                return minimum;
            else if (value.CompareTo(maximum) > 0)
                return maximum;
            else
                return value;
        }
    }
}
