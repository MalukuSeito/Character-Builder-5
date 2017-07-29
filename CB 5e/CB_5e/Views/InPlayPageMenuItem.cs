using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB_5e.Views
{

    public class InPlayPageMenuItem
    {
        public InPlayPageMenuItem()
        {
            TargetType = typeof(InPlayPageDetail);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}