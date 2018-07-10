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

namespace Beers.Droid.Managers
{
	[Service]
	public class StepService : Service, ISensorEventListener
    {
        public event EventHandler<ServiceConnectedEventArgs> LocationServiceConnected = delegate { };
        protected static LocationServiceConnection locationServiceConnection;

		public LocationManager_Droid locationManager
        {
            get
            {
                if (locationServiceConnection.Binder == null)
                    return null;
                // note that we use the ServiceConnection to get the Binder, and the Binder to get the Service here
                return locationServiceConnection.Binder.Service;
            }
        }

		private SensorManager mSensorManager;
        private Sensor mStepDetectorSensor;
        private Sensor mStepConterSensor;

        private int stepCounts;
		private int totalStepCounts;
		private int beforeStepCounts;
      
        private double stepPointRate;

		private bool isCountTargetArea;
		//private bool hasCountTagerArea;

        private List<TargetArea> TargetAreas;

        readonly string logTag = "StepManager";
        IBinder binder;

        public override void OnCreate()
        {
            base.OnCreate();
            Log.Debug(logTag, "OnCreate called in the Location Service");
        }

        // This gets called when StartService is called in our App class
        [Obsolete("deprecated in base class")]
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            Log.Debug(logTag, "LocationService started");
            return StartCommandResult.Sticky;
        }

        public override IBinder OnBind(Intent intent)
        {
            Log.Debug(logTag, "Client now bound to service");

			binder = new StepServiceBinder(this);
            return binder;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            Log.Debug(logTag, "Service has been terminated");

			StopStepCounts();
        }

        public void OnSensorChanged(SensorEvent sensorEvent)
        {
            Sensor sensor = sensorEvent.Sensor;
            IList<float> values = sensorEvent.Values;
            long timestamp = sensorEvent.Timestamp;

            if (sensor.Type == SensorType.StepDetector)
            {
				totalStepCounts++;

				UpdateStepCountsWithTargetArea();

                /*
				if (hasCountTagerArea)
                {
					UpdateStepCountsWithTargetArea();
                }
                */

            }
        }

        public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy)
        {
            // accuracy に変更があった時の処理
        }

		public StepService()
        {
			// create a new service connection so we can get a binder to the service
            locationServiceConnection = new LocationServiceConnection(null);
            // this event will fire when the Service connectin in the OnServiceConnected call 
            locationServiceConnection.ServiceConnected += (object sender, ServiceConnectedEventArgs e) => {
                // we will use this event to notify MainActivity when to start updating the UI
                this.LocationServiceConnected(this, e);
            };

            //センサーマネージャを取得  
			mSensorManager = Android.App.Application.Context.GetSystemService("sensor") as SensorManager;           
            //センサマネージャから TYPE_STEP_DETECTOR についての情報を取得する
            mStepDetectorSensor = mSensorManager.GetDefaultSensor(SensorType.StepDetector);         
            //センサマネージャから TYPE_STEP_COUNTER についての情報を取得する
            mStepConterSensor = mSensorManager.GetDefaultSensor(SensorType.StepCounter);
            stepCounts = 0;
            beforeStepCounts = 0;
			totalStepCounts = 0;
            isCountTargetArea = false;
        }

        public void StartStepCounts()
        {
			new Task(() => {
                // Start our main service
                Log.Debug("App", "Calling StartService");
                Android.App.Application.Context.StartService(new Intent(Android.App.Application.Context, typeof(LocationManager_Droid)));

                // bind our service (Android goes and finds the running service by type, and puts a reference
                // on the binder to that service)
                // The Intent tells the OS where to find our Service (the Context) and the Type of Service
                // we're looking for (LocationService)
                Intent locationServiceIntent = new Intent(Android.App.Application.Context, typeof(LocationManager_Droid));
                Log.Debug("App", "Calling service binding");

                // Finally, we can bind to the Service using our Intent and the ServiceConnection we
                // created in a previous step.
                Android.App.Application.Context.BindService(locationServiceIntent, locationServiceConnection, Bind.AutoCreate);
            }).Start();

            mSensorManager.RegisterListener(this, mStepConterSensor, SensorDelay.Normal);
            mSensorManager.RegisterListener(this, mStepDetectorSensor, SensorDelay.Normal);

			SetStepPointProperty();

        }

		private void SetStepPointProperty()
        {
            List<TargetArea> targetAreas = GetTargetAreas();
            SetTargetAreas(targetAreas);
            this.stepPointRate = 0.1;
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
            mSensorManager.UnregisterListener(this, mStepConterSensor);
            mSensorManager.UnregisterListener(this, mStepDetectorSensor);
			// Unbind from the LocationService; otherwise, StopSelf (below) will not work:
            if (locationServiceConnection != null)
            {
                Log.Debug("App", "Unbinding from LocationService");
                Android.App.Application.Context.UnbindService(locationServiceConnection);
            }

            // Stop the LocationService:
            if (this.locationManager != null)
            {
                Log.Debug("App", "Stopping the LocationService");
                this.locationManager.StopSelf();
            }
            
            //Update StepCount
            stepCounts = 0;
			beforeStepCounts = 0;
			totalStepCounts = 0;
        }

        public int GetStepCounts()
		{
			return stepCounts;
		}

		public int GetStepPoints()
        {
            return (int)Math.Round(this.stepCounts * this.stepPointRate, 0);
        }

		private void UpdateStepCountsWithTargetArea()
        {
            if (IsTargetArea())
            {
                isCountTargetArea = true;
				stepCounts += totalStepCounts - beforeStepCounts;
				beforeStepCounts = totalStepCounts;
            }
            else
            {
                isCountTargetArea = false;
				beforeStepCounts = totalStepCounts;
            }

        }

        private bool IsTargetArea()
        {
            bool isTargetArea = false;
            
            foreach (TargetArea ta in TargetAreas)
            {
                if (locationManager.GetDistance(ta.TargetLoacation) <= ta.TargetScope) isTargetArea = true;
            }

            return isTargetArea;
        }

		public void SetTargetAreas(List<TargetArea> targetAreas)
        {
            this.TargetAreas = targetAreas;
            //this.hasCountTagerArea = true;
        }

		public bool IsCountTargetArea()
        {
            return isCountTargetArea;
        }

		public Location GetCurrentLocation()
        {
			if (locationManager == null) return new Location(0.0, 0.0);
            return locationManager.CurrentLocation;
        }

    }
}
