using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB_5e.ViewModels
{
    public class ShopGroupModel : List<ShopViewModel>
    {
        public string Type { get; set; }
        public string SType { get => Type.Substring(0, 1); }
        public ShopGroupModel(string t, IEnumerable<ShopViewModel> c) : base(c)
        {
            Type = t;
        }
    }
}
