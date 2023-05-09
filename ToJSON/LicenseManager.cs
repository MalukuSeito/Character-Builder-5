using OGL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToJSON
{
    public class LicenseManager : ILicense
    {
        public bool ShowLicense(string title, string[] lines)
        {
            Console.Error.WriteLine(title);
            foreach (string line in lines) Console.Error.WriteLine(line);
            Console.Error.Write("Accept? [y/N]");
            var input = Console.ReadLine();
            return input?.StartsWith("y") ?? false;
        }
    }
}
