using System;
using System.Collections.Generic;

using Xamarin.Forms;

using Beers.Models;
using Beers.ViewModels;
namespace Beers.Views
{
    public partial class PrizeCheckPage : ContentPage
    {
		PrizeCheckViewModel viewModel;

		public PrizeCheckPage(object[] viewparams)
        {
            InitializeComponent();

			int prize = Convert.ToInt32(viewparams[0]);
			BindingContext = viewModel = new PrizeCheckViewModel(prize);
        }
    }
}
