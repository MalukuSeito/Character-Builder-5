using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Character_Builder_Builder
{
    class NewlineFormatter
    {
        private static Regex newline = new Regex("\r?\n");
        public static void Binding_Format(object sender, ConvertEventArgs e)
        {
            if (e.Value != null)
            {
                e.Value = newline.Replace(e.Value.ToString(), Environment.NewLine);
            }
        }
        public static void Add(ControlBindingsCollection dataBindings, string sP, object tO, string tP, bool formattingEnabled = true, DataSourceUpdateMode change = DataSourceUpdateMode.OnPropertyChanged)
        {
            Binding b = new Binding(sP, tO, tP, formattingEnabled, change);
            b.Format += Binding_Format;
            dataBindings.Add(b);
        }
    }
}
