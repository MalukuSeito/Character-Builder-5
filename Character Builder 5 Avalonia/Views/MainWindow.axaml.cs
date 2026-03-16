using System;
using System.IO;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using TheArtOfDev.HtmlRenderer.Avalonia;
using TheArtOfDev.HtmlRenderer.Core.Entities;

using Avalonia.Input;
using Character_Builder_IO;
using CharacterBuilder5.ViewModels;
using OGL;
using OGL.Common;

namespace CharacterBuilder5.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        HtmlPanel.ImageLoad += HtmlPanel_ImageLoad;
    }
    
    private void Preview(object? sender, TappedEventArgs e)
    {
        if (sender is ListBox listBox)
        {
            HtmlPanel.Text = (listBox.SelectedItem as IXML).ToHTML();
        }
    }

    private void Race_DoubleTapped(object? sender, TappedEventArgs e)
    {
        Console.Out.WriteLine(sender);
        Console.Out.WriteLine(DataContext);
        if (sender is ListBox listBox && DataContext is MainWindowViewModel viewModel)
        {
            Console.Out.WriteLine(listBox.SelectedItem);
            viewModel.Context.MakeHistory("");
            viewModel.Context.Player.Race = listBox.SelectedItem as Race;
            viewModel.UpdateRace();
        }
    }

    private void SubRace_DoubleTapped(object? sender, TappedEventArgs e)
    {
        (DataContext as MainWindowViewModel)?.SelectSubRace();
    }

    private void Class_DoubleTapped(object? sender, TappedEventArgs e)
    {
        (DataContext as MainWindowViewModel)?.AddLevel();
    }

    private void Background_DoubleTapped(object? sender, TappedEventArgs e)
    {
        (DataContext as MainWindowViewModel)?.SelectBackground();
    }
    
    private void HtmlPanel_ImageLoad(object? sender, HtmlRendererRoutedEventArgs<HtmlImageLoadEventArgs> e)
    {
        if (e.Event.Src.TrimStart().StartsWith("data:image"))
        {
            var base64 = e.Event.Src[(e.Event.Src.IndexOf(',') + 1)..];
            var bytes = Convert.FromBase64String(base64);

            using var ms = new MemoryStream(bytes);
            var bitmap = new Bitmap(ms);

            e.Event.Callback(bitmap);
        }
    }
}