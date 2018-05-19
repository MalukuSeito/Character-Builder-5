using CB_5e.Services;
using CB_5e.ViewModels;
using CB_5e.ViewModels.Modify.Collections;
using OGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CB_5e.Views.Modify.Collections
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MonsterSkillEditPage : ContentPage
    {
        public MonsterSkillViewModel Model { get; private set; }
        private OGLContext context;
        public MonsterSkillEditPage(MonsterSkillViewModel model, OGLContext context)
        {
            BindingContext = Model = model;
            InitializeComponent();
            this.context = context;
        }
        protected override void OnDisappearing()
        {
            Model?.Refresh();
            base.OnDisappearing();

        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (context.SkillsSimple.Count == 0)
            {
                await context.ImportSkillsAsync();
            }
            Model.Skills = context.SkillsSimple.Keys.OrderBy(s => s).ToList();
        }

    }
}