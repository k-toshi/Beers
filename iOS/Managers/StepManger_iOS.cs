using System;
using Foundation;
using System.Collections.Generic;
using CoreMotion;
using CoreFoundation;
using UIKit;
using System.Threading.Tasks;
using System.Linq;
using Beers.Interfaces;
using Beers.Models;

[assembly: Xamarin.Forms.Dependency(typeof(Beers.iOS.Managers.StepManager_iOS))]
namespace Beers.iOS.Managers
{
    public class StepManager_iOS : IStepManager
    {
        private CMPedometer pedometer;
        private LocationManager_iOS locationManager;

        private int stepCounts;
        private double stepPointRate;
        private bool isStarted;

        private bool hasCountTagerArea;
		private List<TargetArea> TargetAreas;
        /*
        private Location countTargetLocation;
        private double countTargetScope;
        */
        private int beforeStepCounts { get; set; }
        private bool isCountTargetArea;

        public StepManager_iOS()
        {
            pedometer = new CMPedometer();
            locationManager = new LocationManager_iOS();
            stepCounts = 0;
            beforeStepCounts = 0;
            hasCountTagerArea = false;
            isCountTargetArea = false;
            isStarted = false;
        }

        /*
        public void SetCountTargetArea(Location countTargetLocation,double countTargetScope)
        {
            this.countTargetLocation = countTargetLocation;
            this.countTargetScope = countTargetScope;
            this.hasCountTagerArea = true;
        }
        */

		private void SetTargetAreas(List<TargetArea> targetAreas)
        {
			this.TargetAreas = targetAreas;
            this.hasCountTagerArea = true;
        }

        public void StartStepCounts()
        {
            pedometer.StartPedometerUpdates(NSDate.Now, UpdateStepCounts);
			SetStepPointProperty();
            locationManager.StartLocationUpdates();
            isStarted = true;
        }

		private void SetStepPointProperty()
        {
            List<TargetArea> targetAreas = GetTargetAreas();
            SetTargetAreas(targetAreas);
            SetStepPointRate(0.1);
        }

        private List<TargetArea> GetTargetAreas()
        {
            List<TargetArea> targetAreas = new List<TargetArea>
            {
                new TargetArea { TargetLoacation=new Location(), TargetScope = 200},
                new TargetArea { TargetLoacation=new Location(35.615511,139.381886), TargetScope = 100},
                new TargetArea { TargetLoacation=new Location(35.689592,139.700413), TargetScope = 100},
                new TargetArea { TargetLoacation=new Location(35.688468,139.7645), TargetScope = 100},
            };

            return targetAreas;
        }
            
        public void StopStepCounts()
        {
            pedometer.StopPedometerUpdates();
            locationManager.StopLocationUpdates();
            //Update StepCount
            stepCounts = 0;
            beforeStepCounts = 0;
            isStarted = false;
        }

        private void UpdateStepCounts( CMPedometerData pedometerData, NSError error)
        {
            if(error == null) 
            {
                if (hasCountTagerArea)
                {
                    UpdateStepCountsWithTargetArea(pedometerData.NumberOfSteps.Int32Value);
                }
                else
                {
                    stepCounts = pedometerData.NumberOfSteps.Int32Value;
                }
            }
        }

        private void UpdateStepCountsWithTargetArea(int pedometerSteps)
        {
			if (IsTargetArea())
            {
                isCountTargetArea = true;
                stepCounts += pedometerSteps - beforeStepCounts;
                beforeStepCounts = pedometerSteps;
            }
            else
            {
                isCountTargetArea = false;
                beforeStepCounts = pedometerSteps;
            }
        }

        private bool IsTargetArea()
		{
			bool isTargetArea = false;

			foreach(TargetArea ta in TargetAreas)
			{
				if(locationManager.GetDistance(ta.TargetLoacation) <= ta.TargetScope) isTargetArea = true;
			}

			return isTargetArea;
		}

        public int GetStepCounts()
        {
            return stepCounts;
        }

        public void SetStepPointRate(double rate)
        {
            this.stepPointRate = rate;
        }

        public int GetStepPoints()
        {
            return (int)Math.Round(stepCounts * this.stepPointRate,0);
        }

        public bool IsCountTargetArea()
        {
            return isCountTargetArea;
        }

        public Location GetCurrentLocation()
        {
            return locationManager.CurrentLocation;
        }

        public bool IsStarted() { return isStarted; }
    }
}
