using System;
using System.Collections.Generic;

using Xamarin.Forms;

using Beers.Interfaces;

namespace Beers.Views
{
    public partial class QrCodePrintPage : ContentPage
    {
        private string qrCodeString;
        private string tsTitle;
        private Object[] paramsForTv;

        public QrCodePrintPage(string qrCodeString,string title,string titleMessage, string tsTitle, Object[] paramsForTv)
        {
            InitializeComponent();
            this.Title = title;
            this.qrCodeString = qrCodeString;
            lTitleMessage.Text = titleMessage;
            this.tsTitle = tsTitle;
            this.paramsForTv = paramsForTv;

            CreateTableView();

            imageView.Source = ImageSource.FromStream(() =>
            {
                return DependencyService.Get<IQRCodeManager>().ConvertImageStream(qrCodeString);
            });

        }

        private void CreateTableView()
        {
            TableSection ts = new TableSection();
            ts.Title = tsTitle;

            foreach (string param in paramsForTv)
            {
                TextCell tc = new TextCell();
                tc.Text = param;
				tc.TextColor = Color.Black;
                ts.Add(tc);
            }
            trMenu.Add(ts);
        }
    }
}
