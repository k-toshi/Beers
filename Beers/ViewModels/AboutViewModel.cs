using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace Beers
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("http://13.112.224.68/event1/")));
        }

        public ICommand OpenWebCommand { get; }
    }
}
