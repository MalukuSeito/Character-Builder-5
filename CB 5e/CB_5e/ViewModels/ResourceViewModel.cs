using CB_5e.Helpers;
using Character_Builder;
using OGL.Base;
using OGL.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CB_5e.ViewModels
{
    public class ResourceViewModel : ObservableObject
    {
        public static string last;

        public Object Value;

        public ResourceViewModel(object value, PlayerViewModel model)
        {
            Value = value;
            Reduce = new Command(() =>
            {
                if (IsChangeable && last != null && last.Equals(ResourceID))
                {
                    Used++;
                    model.MakeHistory("Resource" + ResourceID);
                    if (Max > 0 && Used > Max) Used = Max;
                    model.Context.Player.SetUsedResources(ResourceID, Used);
                    model.UpdateUsed();

                    model.Save();
                }
                last = ResourceID;
            });
        }

        public int Max
        {
            get
            {
                if (Value is ResourceInfo r) return r.Max;
                else if (Value is ModifiedSpell ms) return ms.count;
                return 0;
            }
        }

        public int Used
        {
            get
            {
                if (Value is ResourceInfo r) return r.Used;
                else if (Value is ModifiedSpell ms) return ms.used;
                return 0;
            }
            set
            {
                if (Value is ResourceInfo r) r.Used = value;
                else if (Value is ModifiedSpell ms) ms.used = value;
                OnPropertyChanged("Desc");
            }
        }

        public String ResourceID
        {
            get
            {
                if (Value is ResourceInfo r) return r.ResourceID;
                else if (Value is ModifiedSpell ms) return ms.getResourceID();
                return null;
            }
        }

        public string Name
        {
            get
            {
                if (Value is ResourceInfo r) return r.Name;
                else if (Value is ModifiedSpell ms) return ms.Name;
                return null;
            }
        }

        public string Desc
        {
            get
            {
                if (Value is ResourceInfo r) return r.Desc;
                else if (Value is ModifiedSpell ms) return ms.Desc;
                return null;
            }
        }
        public string Text
        {
            get
            {
                if (Value is ResourceInfo r) return r.Text;
                else if (Value is ModifiedSpell ms) return ms.Text;
                return null;
            }
        }

        public Command Reduce { get; private set; }
        public bool IsChangeable
        {
            get
            {
                return Value is ResourceInfo || Value is ModifiedSpell ms && ((ms.Level > 0 && ms.RechargeModifier < RechargeModifier.AtWill) || (ms.Level == 0 && ms.RechargeModifier != RechargeModifier.Unmodified && ms.RechargeModifier < RechargeModifier.AtWill));
            }
        }
    }
}
