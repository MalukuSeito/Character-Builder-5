using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CB_5e.Helpers
{
    public class IntToStringConverter : IValueConverter
    {
        // from Int32 to String
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }
        // String to Int
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (int.TryParse(value.ToString(), NumberStyles.AllowLeadingSign, null, out int parsedInt))
            {
                return parsedInt;
            }
            return value;
        }
    }
}
