using OGL;
using OGL.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using OGL.Descriptions;
using OGL.Common;

namespace CB_5e.ViewModels
{

    public class DescriptionChoice : ChoiceViewModel<Feature>
    {

        public int Level { get; set; }
        public DescriptionChoice(PlayerModel model, TableDescription d, INavigation navigation) : base(model, d.UniqueID, d.Amount, null, false, true)
        {
            Desc = d;
            Name = d.Name;
            Navigation = navigation;
            UpdateOptions();
        }

        
        public override void Refresh(Feature feature)
        {
        }

        public TableDescription Desc { get; private set; }

        protected override IEnumerable<IXML> GetOptions()
        {
            return (from s in Desc.Entries select new TableValue { Entry = s }).ToList();
        }

        public override IXML GetValue(string nameWithSource)
        {
            TableEntry c = Desc.Entries.FirstOrDefault(s => s.ToString() == nameWithSource);
            if (c != null) return new TableValue { Entry = c };
            return null;
        }

        protected override void UpdateOptionsImmediately()
        {
        }
    }
}
