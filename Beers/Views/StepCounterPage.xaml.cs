using System;
using System.Collections.Generic;

using Xamarin.Forms;

using Beers.ViewModels;

namespace Beers.Views
{
    public partial class StepCounterPage : ContentPage
    {
        StepCounterViewModel viewModel;

        public StepCounterPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new StepCounterViewModel();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.UpdateStepCountStatus();
        }
    }
}
