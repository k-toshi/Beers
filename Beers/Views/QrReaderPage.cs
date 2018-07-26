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
                case CommonDef.QrCodeType.Payment:
                    await PayToPub(qrParams);
					break;
				case CommonDef.QrCodeType.ApplyEvent:
					await ApplyEvent(qrParams);
					break;
            }
        }

        private async Task PayToPub(string[] payParams)
		{
            int targetPubId = Convert.ToInt32(payParams[1]);
            int targetItemId = Convert.ToInt32(payParams[2]);
            long targetPrice = Convert.ToInt64(payParams[3]);

            string result = await UserController.PayToPub(targetPubId, targetPrice);

			if(String.IsNullOrEmpty(result)) SetDisplayAlertManager("支払", "支払が正常に完了しました", "OK");
            else SetDisplayAlertManager("エラー", "支払が失敗しました", "OK");
            
        }
        
		private async Task ApplyEvent(string[] applyEventParams)
        {
			if (UserController.LoginUser.EventId > 0)
            {
                SetDisplayAlertManager("イベント参加", "イベントに既に参加しています", "OK");
                return;
            }

			int targetEventId = Convert.ToInt32(applyEventParams[1]);
			int targetPubId = Convert.ToInt32(applyEventParams[2]);

			EventUserView result = await EventController.ApplyEvent(targetEventId, targetPubId,"1");

			await UserController.GetUserInfos();

			SetDisplayAlertManager("イベント参加", "イベントに参加しました", "OK");

            /*
            if (String.IsNullOrEmpty(result)) SetDisplayAlertManager("イベント参加", "イベントに参加しました", "OK");
            else SetDisplayAlertManager("エラー", "イベントに参加できませんでした", "OK");
            */

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

