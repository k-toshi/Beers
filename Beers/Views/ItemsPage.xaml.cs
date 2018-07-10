using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xamarin.Forms;

using Beers.Controllers;
using Beers.Models;
using Beers.ViewModels;

namespace Beers.Views
{
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel viewModel;
        private int targetPubId;
        private int targetCategoryId;

        public ItemsPage(int pubId,int categoryId)
        {
            InitializeComponent();
            targetPubId = pubId;
            targetCategoryId = categoryId;

            BindingContext = viewModel = new ItemsViewModel(targetPubId,targetCategoryId);
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Item;
            if (item == null)
                return;

            string qrCodeString =
                (int)CommonDef.QrCodeType.payment + "," +
                         targetPubId.ToString() + "," +
                         item.Id.ToString() + "," +
                         item.Price.ToString();

            Object[] itemInfo = new object[3];
            itemInfo[0] = "商品名:" + item.ItemName;
            itemInfo[1] = "詳細:" + item.Detail;
            itemInfo[2] = "必要ポイント:" + item.Price;

            QrCodePrintPage page = new QrCodePrintPage(
                qrCodeString,
                "商品のお支払い",
                "内容をお確かめの上、QRコードをお読込み下さい。",
                "商品情報",
                itemInfo
            );

            await Navigation.PushAsync(page);

            // Manually deselect item
            ItemsListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
       
    }
}
