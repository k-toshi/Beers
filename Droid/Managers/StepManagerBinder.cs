using System;
using Android.OS;

namespace Beers.Droid.Managers
{
	public class StepServiceBinder : Binder
    {
		public StepService Service
        {
            get { return this.service; }
        }
		protected StepService service;

        public bool IsBound { get; set; }

        // constructor
		public StepServiceBinder(StepService service)
        {
            this.service = service;
        }
    }
}
