using OGL;
using System;
using System.Collections.Generic;
using System.IO;

namespace Character_Builder_5
{
    class Config
    {
        public static PDF PDFExporter = null;
        public static System.Drawing.Color SelectColor = System.Drawing.Color.DarkGray;
        public static void LoadConfig(string path)
        {
            try
            {
                if (!File.Exists(Path.Combine(path, "Config.xml")))
                {
                    ConfigManager cm = new ConfigManager();
                    cm.PDF = new List<string>() { "DefaultPDF.xml", "AlternatePDF.xml" };
                    cm.Save(Path.Combine(path, "Config.xml"));
                }
                ConfigManager loaded = ConfigManager.LoadConfig(path);
                PDFExporter = PDF.Load(ConfigManager.Fullpath(path, loaded.PDF[0]));
                AbilityScores.Load(ConfigManager.Fullpath(path, loaded.AbilityScores));
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message + "\n" + e.InnerException + "\n" + e.StackTrace, "Error while Loading Configuration");
                Program.Exit();
            }
        }
    }
}
