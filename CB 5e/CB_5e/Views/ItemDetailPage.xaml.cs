
using CB_5e.ViewModels;

using Xamarin.Forms;

namespace CB_5e.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        NewFolderViewModel viewModel;

        // Note - The Xamarin.Forms Previewer requires a default, parameterless constructor to render a page.
        public ItemDetailPage()
        {
            InitializeComponent();
        }

        public ItemDetailPage(NewFolderViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }
    }
}
