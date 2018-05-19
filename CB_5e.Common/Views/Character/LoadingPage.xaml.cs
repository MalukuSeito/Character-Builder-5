﻿using CB_5e.Models;
using CB_5e.ViewModels.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CB_5e.Views.Character
{
    
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingPage : ContentPage
    {

        public CancellationTokenSource Cancel = new CancellationTokenSource();

        public LoadingPage(LoadingProgress loader, bool canCancel = true)
        {
            InitializeComponent();
            BindingContext = loader;
            CanCancel = canCancel;
        }

        public bool CanCancel { get; private set; }

        protected override bool OnBackButtonPressed()
        {
            if (!CanCancel) return true;
            Cancel.Cancel();
            return base.OnBackButtonPressed();
        }
    }
}