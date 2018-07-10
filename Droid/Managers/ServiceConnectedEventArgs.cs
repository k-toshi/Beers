using System;
using Android.OS;

namespace Beers.Droid.Managers
{
	public class ServiceConnectedEventArgs : EventArgs
    {
        public IBinder Binder { get; set; }
    }
}
