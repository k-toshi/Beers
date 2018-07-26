using System;
using System.Collections.Generic;

using Xamarin.Forms;

using Beers.Models;
using Beers.ViewModels;

namespace Beers.Views
{
    public partial class EventsPage : ContentPage
    {
		EventsViewModel viewModel;
		PubView pub;
        
		public EventsPage(object[] viewparams)
        {
            InitializeComponent();
            
			int evmInteger = Convert.ToInt32(viewparams[0]);
			if(viewparams.Length >1) pub = (PubView)viewparams[1];
			BindingContext = viewModel = new EventsViewModel((CommonDef.EventsViewMode)evmInteger,pub);
        }

		async void OnEventSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var e = args.SelectedItem as EventView;
            if (e == null)
                return;

            switch (viewModel.eventsViewMode)
            {
                case CommonDef.EventsViewMode.PubParticipatedEvents:
					//バーコード
					await Navigation.PushAsync(CreateQrCodePrintPage(e));
                    break;
				case CommonDef.EventsViewMode.AllNotStartedEvents:
                    //バーコード
					await Navigation.PushAsync(new EventHeaderPage(new object[]{e.Id}));
                    break;
                default:
                    break;
            }

            // Manually deselect item
			EventsListView.SelectedItem = null;
        }

        private QrCodePrintPage CreateQrCodePrintPage(EventView ev)
		{
			string qrCodeString =
                (int)CommonDef.QrCodeType.ApplyEvent + "," +
                         ev.Id.ToString() + "," +
                         pub.Id.ToString();
            
            Object[] itemInfo = new object[3];
            itemInfo[0] = "イベント名:" + ev.Name;
            itemInfo[1] = "参加店舗名:" + pub.Name;

            QrCodePrintPage page = new QrCodePrintPage(
                qrCodeString,
                "イベント参加",
                "内容をお確かめの上、QRコードをお読込み下さい。",
                "参加情報",
                itemInfo
            );

			return page;
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();

			if (viewModel.EventViewList.Count == 0)
                viewModel.LoadEventsCommand.Execute(null);
        }
    }
}
