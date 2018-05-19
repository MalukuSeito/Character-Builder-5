using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CB_5e.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InPlayPageMaster : ContentPage
    {
        public ListView ListView;

        public InPlayPageMaster()
        {
            InitializeComponent();

            BindingContext = new InPlayPageMasterViewModel();
            ListView = MenuItemsListView;
        }

        class InPlayPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<InPlayPageMenuItem> MenuItems { get; set; }

            public InPlayPageMasterViewModel()
            {
                MenuItems = new ObservableCollection<InPlayPageMenuItem>(new[]
                {
                    new InPlayPageMenuItem { Id = 0, Title = "Page 1" },
                    new InPlayPageMenuItem { Id = 1, Title = "Page 2" },
                    new InPlayPageMenuItem { Id = 2, Title = "Page 3" },
                    new InPlayPageMenuItem { Id = 3, Title = "Page 4" },
                    new InPlayPageMenuItem { Id = 4, Title = "Page 5" },
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}