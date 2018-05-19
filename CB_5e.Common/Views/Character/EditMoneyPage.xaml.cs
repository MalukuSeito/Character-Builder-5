using CB_5e.Helpers;
using CB_5e.ViewModels;
using CB_5e.ViewModels.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CB_5e.Views.Character
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditMoneyPage : ContentPage
	{
        public EditMoneyPage (PlayerInfoViewModel model)
		{
            BindingContext = model;
			InitializeComponent ();
		}
	}
}