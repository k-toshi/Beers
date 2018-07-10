using System;
using System.Collections.Generic;

using System.Threading;

using Xamarin.Forms;
using Beers.ViewModels;

namespace Beers.Views
{
    public partial class MyPage : ContentPage
    {
        MyViewModel viewModel;

        public MyPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new MyViewModel();
            //sl.IsVisible = false;
        }

        void ReferHistory_Clicked(object sender, EventArgs e)
        {
            //this.IsEnabled = false;
            //sl.IsVisible = true;
            Thread.Sleep(5000);
            /*
            Device.BeginInvokeOnMainThread(() => {
                sl.IsVisible = true;
                Thread.Sleep(5000);
                //sl.IsVisible = false;
            });
            */
        }

    }
}
