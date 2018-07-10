using System;
using System.IO;

namespace Beers.Interfaces
{
    public interface IQRCodeManager
    {
        Stream ConvertImageStream(string text, int width = 300, int height = 300);
    }
}
