using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace Beers.ViewModels
{
	public class PrizeCheckViewModel :BaseViewModel
    {
		private int prize;
        public int Prize
        {
            get { return prize; }
			set { SetProperty(ref prize, value); }
        }

        public PrizeCheckViewModel(int getPrize)
        {
			Title = "当選結果";
			Prize = getPrize;

			OpenWebCommand = new Command(() => Device.OpenUri(new Uri("http://13.112.224.68/event1/")));

        }

		public ICommand OpenWebCommand { get; }

    }
}
