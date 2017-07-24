using CB_5e.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CB_5e.Views
{
    
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingPage : ContentPage
    {

        public CancellationTokenSource Cancel = new CancellationTokenSource();
        public LoadingPage()
        {
            InitializeComponent();
            BindingContext = LoadingProgress.Instance;
            //Task.Run(async () =>
            //{
            //    await LoadingProgress.Instance.Load(worker.Token).ConfigureAwait(false);
            //    Device.BeginInvokeOnMainThread(async () => {
            //        await Navigation.PopModalAsync(false);
            //    });
            //}).Forget();
        }
        protected override bool OnBackButtonPressed()
        {
            Cancel.Cancel();
            return base.OnBackButtonPressed();
        }
    }
}