using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

using Beers.Models;
using Beers.Interfaces;
using Beers.Controllers;

using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Beers.ViewModels
{
    public class StepCounterViewModel : BaseViewModel
    {
        private Geocoder geocoder;
        public Command ControlStepStatusCommand { get; set; }

        private string currentPlace;
        public string CurrentPlace
        {
            get { return currentPlace; }
            set { SetProperty(ref currentPlace, value); }
        }

        private string isTargetArea;
        public string IsTargetArea
        {
            get { return isTargetArea; }
            set { SetProperty(ref isTargetArea, value); }
        }

        private int stepCounts;
        public int StepCounts
        {
            get { return stepCounts; }
            set { SetProperty(ref stepCounts, value); }
        }

        private int stepPoints;
        public int StepPoints
        {
            get { return stepPoints; }
            set { SetProperty(ref stepPoints, value); }
        }

		private ImageSource walkImg;
		public ImageSource WalkImg
        {
			get { return walkImg; }
			set { SetProperty(ref walkImg, value); }
        }
        
		private ImageSource runImg;
		public ImageSource RunImg
        {
			get { return runImg; }
			set { SetProperty(ref runImg, value); }
        }
        
		private ImageSource carImg;
		public ImageSource CarImg
        {
			get { return carImg; }
			set { SetProperty(ref carImg, value); }
        }

        private string buttonName;
        public string ButtonName
        {
            get { return buttonName; }
            set { SetProperty(ref buttonName, value); }
        }

        private string stepStatus;
        public string StepStatus
        {
            get { return stepStatus; }
            set { SetProperty(ref stepStatus, value); }
        }

        public StepCounterViewModel()
        {
            if(UserController.stepManger == null) {
                UserController.stepManger = DependencyService.Get<IStepManager>();
            }
            geocoder = new Geocoder();
            SetStepStatus(UserController.stepManger.IsStarted());
            ControlStepStatusCommand = new Command(() => ExecuteControlStepStatusCommand());

			Device.StartTimer(
				TimeSpan.FromSeconds(10),
				() =>
				{
					Task.Run(async () =>
					{
						await UpdateStepCountStatus();
					});
					return true;
				});
        }
            
        public async Task UpdateStepCountStatus()
        {
            IsTargetArea = GetIsTargetAreaString();
            StepCounts = UserController.stepManger.GetStepCounts();
            StepPoints = UserController.stepManger.GetStepPoints();
            Location loc = UserController.stepManger.GetCurrentLocation();
            Position position = new Position(loc.Latitude, loc.Longitude);
            var addresses = await geocoder.GetAddressesForPositionAsync(position);
            CurrentPlace = "";
            foreach (var str in addresses) CurrentPlace = str.Replace("\r", "").Replace("\n", "");
			LocationType.ActivityType activityType  = loc.GetActivityType();
			SetActivityImage(activityType);
        }

		private void SetActivityImage(LocationType.ActivityType activityType)
		{
			switch (activityType)
            {
                case LocationType.ActivityType.Walk:
                    WalkImg = "WalkOn.png";
                    RunImg = "Run.png";
                    CarImg = "Car.png";
                    break;
                case LocationType.ActivityType.Run:
                    WalkImg = "Walk.png";
                    RunImg = "RunOn.png";
                    CarImg = "Car.png";
                    break;
                case LocationType.ActivityType.Car:
                    WalkImg = "Walk.png";
                    RunImg = "Run.png";
                    CarImg = "CarOn.png";
                    break;
                default:
                    break;
            }
		}

        private string GetIsTargetAreaString()
        {
            if (UserController.stepManger.IsCountTargetArea()) return "対象エリア内";
            else return "対象エリア外";
        }

        async void ExecuteControlStepStatusCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {

                if (UserController.stepManger.IsStarted())
                {
                    await UserController.GetStepPoint();
                    UserController.stepManger.StopStepCounts();
                }
                else
                {
                    UserController.stepManger.StartStepCounts();               
                }
				SetStepStatus(UserController.stepManger.IsStarted());
                await UpdateStepCountStatus();
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

        private void SetStepStatus(bool setStatus)
        {
            if (setStatus)
            {
                ButtonName = "計測終了";
                StepStatus = "計測中です";
            }
            else
            {
                ButtonName = "計測開始";
                StepStatus = "計測してません";
            }
        }
    }
}
