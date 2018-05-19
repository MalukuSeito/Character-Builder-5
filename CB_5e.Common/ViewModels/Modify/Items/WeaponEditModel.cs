using OGL;
using OGL.Common;
using OGL.Items;
using OGL.Keywords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB_5e.ViewModels.Modify.Items
{
    public class WeaponEditModel : ItemEditModel<Weapon>
    {
        public WeaponEditModel(Weapon obj, OGLContext context): base(obj, context)
        {

        }
        public string Damage { get => Model.Damage; set { if (value == Damage) return; MakeHistory("Damage"); Model.Damage = value; OnPropertyChanged("Damage"); } }
        public string DamageType {
            get => Model.DamageType;
            set
            {
                if (value == null) return;
                if (value == DamageType) return;
                MakeHistory("DamageType");
                Model.DamageType = value;
                OnPropertyChanged("DamageType");
                if (DamageTypes.Contains(value))
                {
                    Keyword kw = new Keyword(value);
                    if (!Model.Keywords.Contains(kw))
                    {
                        Model.Keywords.Add(kw);
                        foreach (string s in DamageTypes.Where(s => s != value))
                        {
                            Model.Keywords.Remove(new Keyword(s));
                        }
                        OnPropertyChanged("Keywords");
                    }
                }
                else
                {
                    foreach (string s in DamageTypes)
                    {
                        Model.Keywords.Remove(new Keyword(s));
                    }
                    OnPropertyChanged("Keywords");
                }
            }
        }
        public List<string> DamageTypes { get => new List<string>() {
                "bludgeoning",
                "piercing",
                "slashing"
            }; }
    }
}
