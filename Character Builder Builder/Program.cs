using Character_Builder_Forms;
using OGL;
using System;
using System.Windows.Forms;

namespace Character_Builder_Builder
{
    static class Program
    {
        public static ErrorLog Errorlog = null;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //String s = "Name = \"hallo'welt\" or name = \"Foobar\"";
            //String Name = "Hallo'Welt";
            //var e = new Expression(ConfigManager.fixQuotes(s));
            //e.EvaluateParameter += (name, args) => args.Result = (name.ToLowerInvariant() == "name") ? Name.ToLowerInvariant() : "";
            //Console.WriteLine(s + " = " + e.Evaluate());
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ConfigManager.LogEvents += (sender, text, e) => Console.WriteLine((text != null ? text + ": " : "") + e?.StackTrace);
            ConfigManager.LicenseProvider = new LicenseProvider();
            Errorlog = new ErrorLog();
            if (SourceManager.Init(Application.StartupPath, true))
            {
                ConfigManager.AlwaysShowSource = true;
                ConfigManager.LoadConfig(Application.StartupPath);
                Application.Run(new MainTab());
            } else
            {
                Application.Exit();
            }
        }
    }
}
