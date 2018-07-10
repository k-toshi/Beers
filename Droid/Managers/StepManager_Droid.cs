
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Beers.Interfaces;
using Beers.Models;
using Android.Hardware;
using Android.App;
using Android.Util;
using Android.Content;
using Android.OS;

[assembly: Xamarin.Forms.Dependency(typeof(Beers.Droid.Managers.StepManager_Droid))]
namespace Beers.Droid.Managers
{
	public class StepManager_Droid : IStepManager
	{
		// events
        public event EventHandler<ServiceConnectedEventArgs> StepServiceConnected = delegate { };      
		protected static StepServiceConnection stepServiceConnection;

		public StepService stepService
        {
            get
            {
                if (stepServiceConnection.Binder == null)
                    return null;
                // note that we use the ServiceConnection to get the Binder, and the Binder to get the Service here
				return stepServiceConnection.Binder.Service;
            }
        }

        private bool isStarted = false;
        
        public StepManager_Droid()
        {
			// create a new service connection so we can get a binder to the service
            stepServiceConnection = new StepServiceConnection(null);
            // this event will fire when the Service connectin in the OnServiceConnected call 
			stepServiceConnection.ServiceConnected += (object sender, ServiceConnectedEventArgs e) => {
                // we will use this event to notify MainActivity when to start updating the UI
                this.StepServiceConnected(this, e);
            };
        }
              
        public void StartStepCounts()
		{         
            isStarted = true;         
			new Task(() => {
                // Start our main service
                Log.Debug("App", "Calling StartService");
                Android.App.Application.Context.StartService(new Intent(Android.App.Application.Context, typeof(StepService)));

                // bind our service (Android goes and finds the running service by type, and puts a reference
                // on the binder to that service)
                // The Intent tells the OS where to find our Service (the Context) and the Type of Service
                // we're looking for (LocationService)
                Intent stepServiceIntent = new Intent(Android.App.Application.Context, typeof(StepService));
                Log.Debug("App", "Calling service binding");

                // Finally, we can bind to the Service using our Intent and the ServiceConnection we
                // created in a previous step.
                Android.App.Application.Context.BindService(stepServiceIntent, stepServiceConnection, Bind.AutoCreate);
            }).Start();
           
        }   

        public void StopStepCounts()
        {
            isStarted = false;
			// Unbind from the LocationService; otherwise, StopSelf (below) will not work:
            if (stepServiceConnection != null)
            {
                Log.Debug("App", "Unbinding from LocationService");
                Android.App.Application.Context.UnbindService(stepServiceConnection);
            }

            // Stop the LocationService:
            if (this.stepService != null)
            {
                Log.Debug("App", "Stopping the LocationService");
				this.stepService.StopStepCounts();
				//this.stepService.StopSelf();
            }
        }

		public void SetTargetAreas(List<TargetArea> targetAreas)
        {
			this.stepService.SetTargetAreas(targetAreas);
        }
      
        public int GetStepCounts()
        {
			if(this.stepService != null) return this.stepService.GetStepCounts();
			return 0;

        }

        public int GetStepPoints()
        {
			if (this.stepService != null) return this.stepService.GetStepPoints();
			else return 0;
        }

        public bool IsCountTargetArea()
        {
			if (this.stepService != null) return this.stepService.IsCountTargetArea();
			return false;
        }

        public Location GetCurrentLocation()
        {
			if (this.stepService != null) return this.stepService.GetCurrentLocation();
			else return new Location(0.0, 0.0);
        }

        public bool IsStarted() { return isStarted; }
    }
}
