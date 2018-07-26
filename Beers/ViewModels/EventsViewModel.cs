using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using Beers.Models;
using Beers.Controllers;

namespace Beers.ViewModels
{
	public class EventsViewModel : BaseViewModel
    {
		public ObservableCollection<EventView> EventViewList { get; set; }
        public Command LoadEventsCommand { get; set; }
		public CommonDef.EventsViewMode eventsViewMode { get; private set; }
		private PubView targetPub;

		public EventsViewModel(CommonDef.EventsViewMode evm,PubView pub)
        {
			eventsViewMode = evm;

			targetPub = pub;

			EventViewList = new ObservableCollection<EventView>();
			LoadEventsCommand = new Command(async () => await ExecuteLoadEventsCommand());
        }

		async Task ExecuteLoadEventsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
				EventViewList.Clear();
				await SetEvents();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        
        private async Task SetEvents()
        {
            var events = new ObservableCollection<EventView>();

            switch (eventsViewMode)
            {
                case CommonDef.EventsViewMode.PubParticipatedEvents:
                    Title = "イベント一覧";
					events = await EventController.GetParticipatedEvents(targetPub.Id);
                    break;
				case CommonDef.EventsViewMode.AllNotStartedEvents:
                    Title = "イベント一覧";
					events = await EventController.GetAllNotStartedEvents();
                    break;
                default:
                    break;
            }

			foreach (var e in events) EventViewList.Add(e);

        }
    }
}
