using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xamarin.Forms;

using Beers.Controllers;
using Beers.Models;
using Beers.ViewModels;

namespace Beers.Views
{
    public partial class TwitterViewPage : ContentPage
    {
		TwitterViewViewModel viewModel;

        public TwitterViewPage()
        {
            InitializeComponent();
			BindingContext = viewModel = new TwitterViewViewModel();
        }
        
        /*
		async void OnTweetSelected(object sender, SelectedItemChangedEventArgs args)
        {
        }
        */

		protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Tweets.Count == 0)
				viewModel.LoadTweetsCommand.Execute(null);
        }
    }
}
