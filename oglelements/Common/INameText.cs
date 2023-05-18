using OGL.Items;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace OGL.Common
{
    public interface IInfoText
    {
        string InfoTitle { get; }
        string InfoText { get; }
        string ToInfo(bool includeDescription = false);
        string Name { get; }

        bool Matches(string text, bool nameOnly)
        {
			CultureInfo Culture = CultureInfo.InvariantCulture;
			if (nameOnly) return Culture.CompareInfo.IndexOf(Name ?? "", text, CompareOptions.IgnoreCase) >= 0;
            return Culture.CompareInfo.IndexOf(Name ?? "", text, CompareOptions.IgnoreCase) >= 0
                || Culture.CompareInfo.IndexOf(InfoText ?? "", text, CompareOptions.IgnoreCase) >= 0;
		}


	}
}
