using System;
using System.Collections.Generic;

using Xamarin.Forms;

using Beers.Models;
using Beers.ViewModels;

namespace Beers.Views
{
    public partial class LoginUserInfoPage : ContentPage
    {

        LoginUserInfoViewModel viewModel;
        
        public LoginUserInfoPage()
        {
            InitializeComponent();
            viewModel = new LoginUserInfoViewModel();
        }

        async void Update_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CuUserPage(CommonDef.CuUserPageType.Change));
        }

        async void UpdatePassword_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CuUserPage(CommonDef.CuUserPageType.ChangePassword));
        }

        async void ReferHistory_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TransactionHistoryPage());
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.GetUserInfo();
            tsLoginUserInfo.BindingContext = tsLoginUserAccountInfo.BindingContext = null;
            tsLoginUserInfo.BindingContext = tsLoginUserAccountInfo.BindingContext = viewModel;
            viewModel.IsBusy = false;
        }
    }
}
