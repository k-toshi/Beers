using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

using Beers.Models;
using Beers.Controllers;

namespace Beers.Views
{
    public class QrReaderPage : ZXingScannerPage
    {
        public QrReaderPage()
        {
            DefaultOverlayTopText = "バーコードを読み取ります";
            DefaultOverlayBottomText = "";

            OnScanResult += async (result) =>
            {
                // スキャン停止
                this.IsScanning = false;

                await Execute(result.Text);

                SetDisplayAlertManager("支払完了", "支払が完了しました", "OK");

                // PopAsyncで元のページに戻り、結果をダイアログで表示
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopAsync(true);
                });

            };
        }

        private async Task Execute(string qrString)
        {
            string[] qrParams = qrString.Split(',');
            CommonDef.QrCodeType qrCodeType = (CommonDef.QrCodeType)Convert.ToInt32(qrParams[0]);

            switch (qrCodeType)
            {
                case CommonDef.QrCodeType.payment:
                    await PayToPub(qrParams);
                    break;
            }
        }

        private async Task PayToPub(string[] payParams)
        {
            int targetPubId = Convert.ToInt32(payParams[1]);
            int targetItemId = Convert.ToInt32(payParams[2]);
            long targetPrice = Convert.ToInt64(payParams[3]);

            string result = await UserController.PayToPub(targetPubId, targetPrice);

            if(String.IsNullOrEmpty(result)) SetDisplayAlertManager("支払完了", "支払が完了しました", "OK");
            else SetDisplayAlertManager("エラー", "支払が失敗しました", "OK");

        }

        private void SetDisplayAlertManager(string title,string message,string button)
        {
            DisplayAlertManager.IsAlert = true;
            DisplayAlertManager.Title = title;
            DisplayAlertManager.Message = message;
            DisplayAlertManager.Button = button;
        }
    }
}

