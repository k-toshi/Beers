using System;
using Xamarin.Forms;
namespace Beers.Models
{
    public class BeersMenu
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public int Order { get; set; }
        public int UserAuthLevel { get; set; }
        public string PageName { get; set; }
        public FileImageSource Icon { get; set; }
    }
}
