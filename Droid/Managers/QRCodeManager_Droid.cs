using System;
using System.IO;
using ZXing.Mobile;
using Beers.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(Beers.Droid.Managers.QRCodeManager_Droid))]
namespace Beers.Droid.Managers
{
    public class QRCodeManager_Droid : IQRCodeManager
    {
        public QRCodeManager_Droid()
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

            MemoryStream stream = new MemoryStream();
            bitmap.Compress(Android.Graphics.Bitmap.CompressFormat.Png, 100, stream);
            stream.Position = 0;

            return stream;
        }
    }
}
