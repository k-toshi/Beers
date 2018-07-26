using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using Beers.Models;
using Beers.Controllers;

using System.Linq;

namespace Beers.ViewModels 
{
	public class EventHeaderViewModel : BaseViewModel
    {
		private int id;
        public int Id
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

		private ImageSource eventImageUrl;
		public ImageSource EventImageUrl
        {
			get { return eventImageUrl; }
			set { SetProperty(ref eventImageUrl, value); }
        }

		private string detail;
		public string Detail
        {
			get { return detail; }
			set { SetProperty(ref detail, value); }
        }

		private string place;
		public string Place
        {
			get { return place; }
			set { SetProperty(ref place, value); }
        }

		private string schedule;
		public string Schedule
        {
			get { return schedule; }
			set { SetProperty(ref schedule, value); }
        }

		private string aimTotalPrize;
		public string AimTotalPrize
        {
			get { return aimTotalPrize; }
			set { SetProperty(ref aimTotalPrize, value); }
        }
        
		private int EventId;      
		public Command LoadEventHeaderCommand { get; set; }

		public EventHeaderViewModel(int eventId)
        {
			LoadEventHeaderCommand = new Command(async () => await ExecuteLoadEventHeaderCommand());
			EventId = eventId;       
        }

		public async Task ExecuteLoadEventHeaderCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            
            try
            {
				var eventHeaderView = new ObservableCollection<EventHeaderView>();            
				eventHeaderView = await EventController.GetEventHeader(EventId);

				Id = eventHeaderView[0].Id;
				Name = eventHeaderView[0].Name;
				EventImageUrl = eventHeaderView[0].EventImageUrl;
				Detail = eventHeaderView[0].Detail;
				Place = eventHeaderView[0].Place;
				Schedule = eventHeaderView[0].Schedule;
				AimTotalPrize = eventHeaderView[0].AimTotalPrize;

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
    }
}
