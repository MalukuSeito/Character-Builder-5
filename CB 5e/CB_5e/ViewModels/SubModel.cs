using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Character_Builder;
using Xamarin.Forms;

namespace CB_5e.ViewModels
{
    public abstract class SubModel : PlayerModel
    {
        public PlayerModel Parent { get; private set; }
        public SubModel(PlayerModel parent, string title) : base(parent.Context)
        {
            Parent = parent;
            Undo = parent.Undo;
            Redo = parent.Redo;
            ChildModel = true;
            Title = title;
            parent.PlayerChanged += Parent_PlayerChanged;
        }

        private void Parent_PlayerChanged(object sender, EventArgs e)
        {
            OnPropertyChanged(null);
        }

        public override void UpdateSpellcasting()
        {
            Parent.UpdateSpellcasting();
        }




        public override void MakeHistory(string h = null)
        {
            Parent.MakeHistory(h);
        }

        public override void Save()
        {
            Parent.Save();
        }
        public override void DoSave()
        {
            Parent.DoSave();
        }
        public override void FirePropertyChanged(string prop)
        {
            Parent.FirePropertyChanged(prop);
            base.FirePropertyChanged(prop);
        }
        public override void FirePlayerChanged()
        {
            Parent.FirePlayerChanged();
        }
        public override Command RefreshInventory => Parent.RefreshInventory;
        public override SubModel FirstPage => Parent.FirstPage;
        public override void MoneyChanged()
        {
            Parent.MoneyChanged();
        }
        public override void ChangedSelectedSpells(string spellcastingID)
        {
            Parent.ChangedSelectedSpells(spellcastingID);
        }
    }
}
