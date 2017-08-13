using CB_5e.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CB_5e.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayerPersonalPage : ContentPage
    {
        public PlayerPersonalPage(PlayerPersonalViewModel model)
        {
            InitializeComponent();
            model.Navigation = Navigation;
            BindingContext = model;
            MakeBinding(XP, new Binding("XP"));
        }

        private static void MakeBinding(Entry entry, Binding b)
        {
            entry.SetBinding(Entry.TextProperty, b);
            entry.Focused += (s, e) => { entry.RemoveBinding(Entry.TextProperty); };
            entry.Unfocused += (s, e) => {
                string tmp = entry.Text;
                entry.SetBinding(Entry.TextProperty, b);
                entry.SetValue(Entry.TextProperty, tmp);
            };
        }
    }
}