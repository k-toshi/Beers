using System.Collections.Generic;

using Xamarin.Forms;

using Beers.Models;
using Beers.ViewModels;


namespace Beers.Views
{
    public partial class CategoriesPage : ContentPage
    {
        CategoriesViewModel viewModel;
        private int targetPubId;

        public CategoriesPage(PubView pub)
        {
            InitializeComponent();
            targetPubId = pub.Id;
            BindingContext = viewModel = new CategoriesViewModel(targetPubId);

        }

        async void OnCategorySelected(object sender, SelectedItemChangedEventArgs args)
        {
            var category = args.SelectedItem as Category;
            if (category == null)
                return;

            await Navigation.PushAsync(new ItemsPage(targetPubId,category.Id));

            // Manually deselect item
            CategoriesListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Categories.Count == 0)
                viewModel.LoadCategoriesCommand.Execute(null);
        }
    }
}
