using System;
using System.Collections.Generic;

using Xamarin.Forms;

using Beers.Models;
using Beers.ViewModels;

namespace Beers.Views
{
    public partial class PubsPage : ContentPage
    {
        PubsViewModel viewModel;

        public PubsPage(object[] viewparams)
        {
            InitializeComponent();

            int pvmInteger = Convert.ToInt32(viewparams[0]);
            BindingContext = viewModel = new PubsViewModel((CommonDef.PubsViewMode)pvmInteger);
        }

        async void OnPubSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var pub = args.SelectedItem as PubView;
            if (pub == null)
                return;

            switch (viewModel.pubsViewMode)
            {
                case CommonDef.PubsViewMode.UserPub:
                    await Navigation.PushAsync(new CategoriesPage(pub));
                    break;
                default:
                    break;
            }

            // Manually deselect item
            PubsListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Pubs.Count == 0)
                viewModel.LoadPubsCommand.Execute(null);
        }
    }
}
