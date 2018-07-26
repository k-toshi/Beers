using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Xamarin.Forms;

using Beers.Models;
using Beers.Controllers;
using Beers.ViewModels;

namespace Beers.Views
{
    public partial class EventHeaderPage : ContentPage
    {
		EventHeaderViewModel viewModel;

		public EventHeaderPage(object[] viewparams)
        {
            InitializeComponent();

			int eventId = Convert.ToInt32(viewparams[0]);         
			BindingContext = viewModel = new EventHeaderViewModel(eventId);

        }

		async void OnDetailPage(object sender, EventArgs args)
        {
            //await Navigation.PushAsync(new TwitterViewPage());
        }

		protected override void OnAppearing()
        {
            base.OnAppearing();         
            viewModel.LoadEventHeaderCommand.Execute(null);
        }

    }
}
