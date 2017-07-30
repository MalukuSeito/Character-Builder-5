using Character_Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CB_5e.ViewModels
{
    public abstract class PlayerModel: BaseViewModel
    {
        public event EventHandler PlayerChanged;
        public void FirePlayerChanged() => PlayerChanged?.Invoke(this, EventArgs.Empty);
        public BuilderContext Context { get; private set; }

        public PlayerModel(BuilderContext context)
        {
            Context = context;
        }

        public Command Undo { get; set; }
        public Command Redo { get; set; }

        public abstract void MakeHistory(string h = null);
        public abstract void Save();
        public abstract void DoSave();
    }
}
