using CB_5e.Helpers;
using Character_Builder;
using OGL.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CB_5e.ViewModels
{
    public class SpellbookViewModel: SubModel
    {
        public SpellcastingFeature SpellcastingFeature { get; protected set; }
        public string SpellcastingID { get => SpellcastingFeature.SpellcastingID; }
        public PlayerModel Model { get => Parent; }
        
        public virtual void Refresh(SpellcastingFeature feature)
        {
            SpellcastingFeature = feature;
            OnPropertyChanged(null);
        }

        public SpellbookViewModel(PlayerModel model, SpellcastingFeature spellcasting, string title): base(model, title)
        {
            SpellcastingFeature = spellcasting;
        }
    }
}
