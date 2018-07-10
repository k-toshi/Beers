using System;
using System.Collections.Generic;

using Xamarin.Forms;

using Beers.ViewModels;
using Beers.Models;

namespace Beers.Views
{
    public partial class CuUserPage : ContentPage
    {
        CuUserViewModel viewModel;
        private CommonDef.CuUserPageType cuUserPageType { get; set; }

        public CuUserPage(CommonDef.CuUserPageType cuUserPageType)
        {
            InitializeComponent();
            this.cuUserPageType = cuUserPageType;
            SetView();
            BindingContext = viewModel = new CuUserViewModel(cuUserPageType);
        }

        private void SetView()
        {
            switch (this.cuUserPageType)
            {
                case CommonDef.CuUserPageType.New:
                    tiCuUser.Text = "新規登録";
                    trCuUser.Remove(tsOldPassword);
                    break;
                case CommonDef.CuUserPageType.Change:
                    tiCuUser.Text = "更新";
                    tsUserInfo.Remove(ecEmial);
                    trCuUser.Remove(tsOldPassword);
                    trCuUser.Remove(tsPassword);
                    trCuUser.Remove(tsConfirmationPassword);
                    break;
                case CommonDef.CuUserPageType.ChangePassword:
                    tiCuUser.Text = "更新";
                    trCuUser.Remove(tsUserInfo);
                    break;
                default:
                    break;
            }
        }

        private async void Update_Clicked(object sender, EventArgs e)
        {
            await viewModel.ExecuteCuUserCommand();
            if(!viewModel.IsError) await Navigation.PopAsync();
        }
    }
}
