using System;
using System.Collections.Generic;

using Xamarin.Forms;

using Beers.ViewModels;

namespace Beers.Views
{
    public partial class TransactionHistoryPage : ContentPage
    {
         TransactionHistoryViewModel viewModel;

        public TransactionHistoryPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new TransactionHistoryViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.TransactionHistory.Count == 0)
                viewModel.LoadTransactionHistoryCommand.Execute(null);
        }
    }
}
