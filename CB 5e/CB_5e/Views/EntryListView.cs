using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CB_5e.Views
{
    public class EntryListView:ListView
    {
        public EntryListView():base(ListViewCachingStrategy.RecycleElement)
        {
        }
    }
}
