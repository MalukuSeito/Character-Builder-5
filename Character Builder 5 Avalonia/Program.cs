using Avalonia;
using System;
using Character_Builder_IO;
using Character_Builder;
using OGL;

namespace CharacterBuilder5;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        ConfigManager.LogEvents += (sender, text, e) => Console.WriteLine((text != null ? text + ": " : "") + e?.StackTrace);
        HTMLExtensions.LoadTransform += (t, o) => { if (o is DisplayPossession) t.Load(HTMLExtensions.Transform_Possession.FullName); };
        
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    } 

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
