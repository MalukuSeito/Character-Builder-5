using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace CB_5e.Views
{
	public class ImageViewer : ContentPage
	{
        public ImageViewer(ImageSource image, byte[] data, string name = null)
        {
            Content = new Grid
            {
                Padding = new Thickness(20),
                Children = {
                    new ZoomImage() {
                        Source = image,
                        Data = data,
                        Name = name
                    }
                }
            };
        }
    }
}