using OGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Character_Builder_Forms;
using System.IO;
using System.Reflection;
using System.Linq.Expressions;

namespace Data_Browser
{
    static class Program
    {

        public static OGLContext Context = new OGLContext();
        public static ErrorLog Errorlog = null;
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ConfigManager.LogEvents += (sender, text, e) => Console.WriteLine((text != null ? text + ": " : "") + e?.StackTrace);
            string[] args = Environment.GetCommandLineArgs();
            Errorlog = new ErrorLog();
            LoadConfig(Application.StartupPath);
            LoadData();
            Application.Run(new Drowser());
        }

        private static void LoadConfig(string path)
        {
            try
            {
                ConfigManager loaded = Context.LoadConfig(path);
                SourceManager.Init(Context, Application.StartupPath, true);
                ConfigManager.AlwaysShowSource = true;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message + "\n" + e.InnerException + "\n" + e.StackTrace, "Error while Loading Configuration");
                Application.Exit();
            }
        }

        public static void ReloadData()
        {
            LoadConfig(Application.StartupPath);
            SourceManager.Init(Program.Context, Application.StartupPath, true);
            LoadData();
        }

        public static void LoadData()
        {
            Context.ImportSkills();
            Context.ImportLanguages();
            Context.ImportSpells();
            Context.ImportItems();
            Context.ImportBackgrounds();
            Context.ImportRaces();
            Context.ImportSubRaces();
            Context.ImportStandaloneFeatures();
            Context.ImportConditions();
            Context.ImportMagic();
            Context.ImportClasses(true);
            Context.ImportSubClasses(true);
        }
    }

    public static class PropertyHelper<T>
    {
        public static PropertyInfo GetProperty<TValue>(
            Expression<Func<T, TValue>> selector)
        {
            Expression body = selector;
            if (body is LambdaExpression)
            {
                body = ((LambdaExpression)body).Body;
            }
            switch (body.NodeType)
            {
                case ExpressionType.MemberAccess:
                    return (PropertyInfo)((MemberExpression)body).Member;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
