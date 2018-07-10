using System;
using System.IO;
using ZXing.Mobile;
using Beers.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(Beers.iOS.Managers.QRCodeManager_iOS))]
namespace Beers.iOS.Managers
{
    public class QRCodeManager_iOS :IQRCodeManager
    {
        public QRCodeManager_iOS()
        {

        }

        public Stream ConvertImageStream(string text, int width = 300, int height = 300)
        {
            var barcodeWriter = new BarcodeWriter
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = width,
                    Height = height,
                    Margin = 1
                }
            };
            barcodeWriter.Renderer = new BitmapRenderer();
            var bitmap = barcodeWriter.Write(text);
            var stream = bitmap.AsPNG().AsStream();
            stream.Position = 0;

            return stream;
        }
    }
}
