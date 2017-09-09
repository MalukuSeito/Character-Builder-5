using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OGL;
using Xamarin.Forms;

namespace CB_5e.ViewModels
{
    public class ProxyModel : IEditModel
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

        public event PropertyChangedEventHandler PropertyChanged;

        public void MakeHistory(string id = "")
        {
            Changed = true;
        }

        public Task<bool> SaveAsync(bool overwrite)
        {
            Parent?.MakeHistory();
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
