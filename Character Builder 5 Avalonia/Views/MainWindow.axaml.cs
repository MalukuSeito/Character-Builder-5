using System;
using System.IO;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using TheArtOfDev.HtmlRenderer.Avalonia;
using TheArtOfDev.HtmlRenderer.Core.Entities;

namespace CharacterBuilder5.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        HtmlPanel.ImageLoad += HtmlPanel_ImageLoad;
        Console.Out.WriteLine("Init");
    }
    
    private void HtmlPanel_ImageLoad(object? sender, HtmlRendererRoutedEventArgs<HtmlImageLoadEventArgs> e)
    {

        if (e.Event.Src.TrimStart().StartsWith("data:image"))
        {
            Console.Out.WriteLine(e.Event.Src);
            var base64 = e.Event.Src[(e.Event.Src.IndexOf(',') + 1)..];
            var bytes = Convert.FromBase64String(base64);

            using var ms = new MemoryStream(bytes);
            var bitmap = new Bitmap(ms);

            e.Event.Callback(bitmap);
        }
    }
}