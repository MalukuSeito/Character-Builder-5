using Character_Builder_5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using XCalc;

namespace Character_Builder_Builder
{
    static class Program
    {
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
            if (Character_Builder_5.SourceManager.init(Application.StartupPath))
            {
                Character_Builder_5.ConfigManager.AlwaysShowSource = true;
                Character_Builder_5.ConfigManager.LoadConfig(Application.StartupPath);
                Application.Run(new MainTab());
            } else
            {
                Application.Exit();
            }
        }
    }
}
