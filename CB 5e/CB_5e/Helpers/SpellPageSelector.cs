using CB_5e.ViewModels;
using CB_5e.ViewModels.Character;
using CB_5e.ViewModels.Character.Play;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CB_5e.Helpers
{
    class SpellPageSelector : DataTemplateSelector
    {
        public DataTemplate Spellcasting { get; set; }

        public DataTemplate Preparation { get; set; }

        public DataTemplate NoSpells { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is SpellbookSpellsViewModel)
                return Spellcasting;

            if (item is SpellPrepareViewModel)
                return Preparation;

            return NoSpells;
        }
    }
}
