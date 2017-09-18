using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OGL;
using Xamarin.Forms;
using CB_5e.Helpers;

namespace CB_5e.ViewModels.Modify
{
    public class ProxyModel : ObservableObject, IEditModel
    {
        public ProxyModel(IEditModel parent = null)
        {
            Parent = parent;
            Context = parent?.Context;
            Navigation = parent?.Navigation;
        }

        public Command Undo => new Command(() => throw new NotImplementedException(), () => false);

        public Command Redo => new Command(() => throw new NotImplementedException(), () => false);

        public bool TrackChanges { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IEditModel Parent { get; private set; }
        public OGLContext Context { get; set; }

        public bool Changed { get; set; } = false;

        public int UnsavedChanges => throw new NotImplementedException();

        public INavigation Navigation { get; set; }

        public virtual void MakeHistory(string id = "")
        {
            if (!Changed)
            {
                Parent?.MakeHistory();
            }

            Changed = true;
        }

        public virtual Task<bool> SaveAsync(bool overwrite)
        {
            Changed = false;
            
            return Task.FromResult(true);
        }
    }

    public class ProxyModel<T>: ProxyModel {
        public ProxyModel(List<T> values, IEditModel parent = null) : base (parent)
        {
            Values = values;
        }
        List<T> Values { get; set; }
    }
}
