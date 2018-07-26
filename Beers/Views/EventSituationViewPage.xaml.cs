using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Xamarin.Forms;

using Beers.Models;
using Beers.Controllers;
using Beers.ViewModels;

namespace Beers.Views
{
    public partial class EventSituationViewPage : ContentPage
    {
		EventSituationViewViewModel viewModel;

        public EventSituationViewPage()
        {
            InitializeComponent();

			BindingContext = viewModel = new EventSituationViewViewModel();
        }

		async void OnTwitterIcon(object sender, EventArgs args)
		{
			await Navigation.PushAsync(new TwitterViewPage());
		}
        
		private async void Check_Clicked(object sender, EventArgs e)
        {
			ObservableCollection<WinPubView> winPubViewList = await viewModel.GetWinPubs();

            if(winPubViewList.Count<=0)
			{
				await DisplayAlert("未確定", "当選店が未確定です。しばらくお待ちください。", "OK");
			}
			else
			{
				int unitPrize = WinPubPrize(winPubViewList);

				if (unitPrize > 0) await Navigation.PushAsync(new PrizeCheckPage(new object[] { unitPrize }));
				else await Navigation.PushAsync(new LostPage());
			}
        }

		private int WinPubPrize(ObservableCollection<WinPubView> winPubViewList)
		{
			foreach(var wpv in winPubViewList)if (wpv.WinPubId == UserController.LoginUser.PubId) return wpv.UnitPrize;

			return -1;

		}

		protected override void OnAppearing()
        {
            base.OnAppearing();
            
			viewModel.LoadEventsCommand.Execute(null);
        }
    }
}
