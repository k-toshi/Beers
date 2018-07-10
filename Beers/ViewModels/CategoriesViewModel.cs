using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using Beers.Models;
using Beers.Controllers;

namespace Beers.ViewModels
{
    public class CategoriesViewModel : BaseViewModel
    {
        public ObservableCollection<Category> Categories { get; set; }
        public Command LoadCategoriesCommand { get; set; }
        private int targetPubId;

        public CategoriesViewModel(int pubId)
        {
            Categories = new ObservableCollection<Category>();
            targetPubId = pubId;
            Title = "商品カテゴリ";
            LoadCategoriesCommand = new Command(async () => await ExecuteLoadCategoriesCommand());
        }

        async Task ExecuteLoadCategoriesCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Categories.Clear();
                var categories = await PubController.GetCategories(targetPubId);
                foreach (var category in categories) Categories.Add(category);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
