using System;

namespace Beers.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string Detail { get; set; }
        public int Price { get; set; }
        private Uri imageUrl { get; set; }
        public Uri ImageUrl
        {
            get { return imageUrl; }
            set { imageUrl = Util.GetUriFromImageUrl(value); }
        }
    }
}
