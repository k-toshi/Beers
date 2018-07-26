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
	public class EventSituationViewViewModel : BaseViewModel
    {  
		private int eventId;
        public int EventId
        {
			get { return eventId; }
			set { SetProperty(ref eventId, value); }
        }

		private string eventName;
		public string EventName
        {
			get { return eventName; }
			set { SetProperty(ref eventName, value); }
        }

		private string pubName;
        public string PubName
        {
			get { return pubName; }
			set { SetProperty(ref pubName, value); }
        }
        
		private int userCount;
		public int UserCount
        {
			get { return userCount; }
			set { SetProperty(ref userCount, value); }
        }

		private int totalPrize;
		public int TotalPrize
        {
			get { return totalPrize; }
			set { SetProperty(ref totalPrize, value); }
        }

		private string endDateTime;
		public string EndDateTime
        {
			get { return endDateTime; }
			set { SetProperty(ref endDateTime, value); }
        }

		private int eventUnitPrice;
		public int EventUnitPrice
        {
			get { return eventUnitPrice; }
			set { SetProperty(ref eventUnitPrice, value); }
        }

		private int maxUnitPrize;
		public int MaxUnitPrize
        {
			get { return maxUnitPrize; }
			set { SetProperty(ref maxUnitPrize, value); }
        }

		private int myPrize;
		public int MyPrize
        {
			get { return myPrize; }
			set { SetProperty(ref myPrize, value); }
        }
        
		private ImageSource updownImg;
		public ImageSource UpdownImg
        {
			get { return updownImg; }
			set { SetProperty(ref updownImg, value); }
        }

		private ImageSource upImg;
        public ImageSource UpImg
        {
			get { return upImg; }
			set { SetProperty(ref upImg, value); }
        }
        
		private Color updownColor;
		public Color UpdownColor
        {
			get { return updownColor; }
			set { SetProperty(ref updownColor, value); }
        }

		private Color upColor;
        public Color UpColor
        {
			get { return upColor; }
			set { SetProperty(ref upColor, value); }
        }

		private int beforeMyPrize = 0;
		private int beforeTotalPrize = 0;
        public Command LoadEventsCommand { get; set; }

        public EventSituationViewViewModel()
        {         
			LoadEventsCommand = new Command(async () => await ExecuteLoadEventsCommand());
			UpColor = Color.White;
			UpdownColor = Color.White;
        }      

		public async Task ExecuteLoadEventsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
				var eventSituationView = new ObservableCollection<EventSituationView>();
				var eventUserCountViewList = new ObservableCollection<EventUserCountView>();
                
				eventSituationView = await EventController.GetEventSituation(UserController.LoginUser.EventId);

				EventId = eventSituationView[0].EventId;
				EventName = eventSituationView[0].EventName;
				TotalPrize = eventSituationView[0].TotalPrize;
				EndDateTime = eventSituationView[0].EndDateTime;
				EventUnitPrice = eventSituationView[0].EventUnitPrice;
				MaxUnitPrize = eventSituationView[0].MaxUnitPrize;
            
                eventUserCountViewList = await EventController.GetEventUserCount(UserController.LoginUser.EventId);            
				EventUserCountView targetPub = (EventUserCountView)eventUserCountViewList.Where((arg) => arg.PubId == UserController.LoginUser.PubId).FirstOrDefault();

				PubName = targetPub.PubName;
				UserCount = targetPub.UserCount;

				MyPrize = GetTargetPrizeUnderRow(System.Convert.ToInt32(Math.Floor((double)TotalPrize / (double)targetPub.UserCount)),MaxUnitPrize);

				if (beforeMyPrize > MyPrize)
				{
					UpdownImg = "down.png";
					UpdownColor = Color.FromHex("#FC0107");
				}
				else if (beforeMyPrize < MyPrize)
                {
                    UpdownImg = "up.png";
					UpdownColor = Color.FromHex("#50BAA9");
                }
				else
				{
					UpdownImg = "";
					UpdownColor = Color.White;
				}

				if (beforeTotalPrize < TotalPrize)
                {
                    UpImg = "up.png";
                    UpColor = Color.FromHex("#50BAA9");
                }
                else
                {
                    UpImg = "";
                    UpColor = Color.White;
                }

				beforeMyPrize = MyPrize;
				beforeTotalPrize = TotalPrize;

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
        
		/// <summary>
		/// todo:景品表示法の元にターゲット金額を設定　→APIで作り直す
        /// </summary>
        /// <param name="targetPrize"></param>
        /// <returns></returns>
        private int GetTargetPrizeUnderRow(int targetPrize,int maxunitprize)
        {
			int maxPrizeUnderRow = maxunitprize;

            if (targetPrize <= maxPrizeUnderRow)
            {
                return targetPrize;
            }
            else
            {
                return maxPrizeUnderRow;
            }
        }

		public async Task<ObservableCollection<WinPubView>> GetWinPubs()
        {
            return await EventController.GetWinPubs();
        }
    }
}
