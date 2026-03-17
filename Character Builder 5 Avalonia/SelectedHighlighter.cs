using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace CharacterBuilder5;

public class SelectedHighlighter : AvaloniaObject, IValueConverter
{
    private bool _swapped;
    public static readonly StyledProperty<IBrush> AccentBrushProperty =
        AvaloniaProperty.Register<SelectedHighlighter, IBrush>(nameof(AccentBrush));

    public IBrush AccentBrush
    {
        get => GetValue(AccentBrushProperty);
        set => SetValue(AccentBrushProperty, value);
    }
    
    public static readonly StyledProperty<IBrush> DefaultBrushProperty =
        AvaloniaProperty.Register<SelectedHighlighter, IBrush>(nameof(DefaultBrush));
    public IBrush DefaultBrush
    {
        get => GetValue(DefaultBrushProperty);
        set => SetValue(DefaultBrushProperty, value);
    }


    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is true)
        {
            _swapped = true;
            return AccentBrush;
        } 
        return _swapped ? DefaultBrush : BindingOperations.DoNothing;
    }
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>  throw new NotSupportedException();
}