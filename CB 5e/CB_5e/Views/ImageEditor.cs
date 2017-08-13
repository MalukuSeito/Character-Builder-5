using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace CB_5e.Views
{
    public class ImageEditor : ContentPage
	{
        public ImageEditor(ImageSource image, byte[] data, Command Save, string name = null)
        {
            Grid g = new Grid
            {
                Padding = new Thickness(20),
            };
            g.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            g.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            g.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            g.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            g.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            g.Children.Add(new ZoomImage()
            {
                Source = image,
                Data = data,
                Name = name
            }, 0, 3, 0, 1);
            Button s = new Button()
            {
                Text = "Select"
            };
            s.Clicked += async (o, e) =>
            {
                byte[] b = await DependencyService.Get<IPhotoService>().GetImageDataAsync();
                if (b != null) Save.Execute(b);
            };
            g.Children.Add(s, 0, 1);
            Button d = new Button()
            {
                Text = "Clear"
            };
            d.Clicked += (o, e) => Save.Execute(null);
            g.Children.Add(d, 1, 1);
            Button p = new Button()
            {
                Text = "Paste"
            };
            p.Clicked += (o, e) =>
            {
                byte[] b = DependencyService.Get<IClipboardService>().GetImageData();
                if (b != null) Save.Execute(b);
            };
            g.Children.Add(p, 2, 1);
            Content = g;
        }
    }
}