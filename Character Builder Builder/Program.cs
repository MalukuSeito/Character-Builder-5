using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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
