using Character_Builder;
using OGL;
using System;
using System.Collections.Generic;
using System.IO;
using CharacterBuilder5.Common;

namespace CharacterBuilder5.ViewModels;

public static class Config
{
    public static PDF? PDFExporter = null;
    public static void LoadConfig(BuilderContext context, string path)
    {
        try
        {
            if (!File.Exists(Path.Combine(path, "Config.xml")))
            {
                ConfigManager cm = new ConfigManager()
                {
                    PDF = new List<string>() { "DefaultPDF.xml", "AlternatePDF.xml" }
                };
                cm.Save(Path.Combine(path, "Config.xml"));
            }
            ConfigManager loaded = context.LoadConfig(path);
            PDFExporter = PlayerExtensions.Load(ImportExtensions.Fullpath(path, loaded.PDF[0]));
            context.LoadAbilityScores(ImportExtensions.Fullpath(path, loaded.AbilityScores));
        }
        catch (Exception e)
        {
            // For Avalonia, we could use a dialog, but for now just log or rethrow
            Console.WriteLine($"Error while Loading Configuration: {e.Message}\n{e.StackTrace}");
        }
    }
}
