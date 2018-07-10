using System;
using System.Collections.Generic;

using Xamarin.Forms;

using Beers.ViewModels;
using Beers.Controllers;
using Beers.Models;

namespace Beers.Views
{
    public partial class LoginPage : ContentPage
    {
        LoginViewModel viewModel;
        MainPage source;

        public LoginPage(MainPage source)
        {
            InitializeComponent();
            this.source = source;
            BindingContext = viewModel = new LoginViewModel();
        }

        private async void Login_Clicked(object sender, EventArgs e)
        {
            await viewModel.ExecuteLoginCommand();
            source.SetLoginPage();
        }

        private async void Add_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CuUserPage(CommonDef.CuUserPageType.New));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            source.SetLoginPage();
        }

    }
}
