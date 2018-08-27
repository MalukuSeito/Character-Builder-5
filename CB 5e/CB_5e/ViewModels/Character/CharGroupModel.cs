using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB_5e.ViewModels.Character
{
    public class CharGroupModel : List<Models.Character>
    {
        public string Folder { get; set; }
        public string SFolder { get => Folder.Substring(0, 1); }
        public CharGroupModel(string folder, IEnumerable<Models.Character> c) : base(c)
        {
            Folder = folder;
        }
    }
}
