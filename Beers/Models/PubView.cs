using System;

namespace Beers.Models
{
    public class PubView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        private Uri imageUrl { get; set; }
        public Uri ImageUrl
        {
            get { return imageUrl; }
            set { imageUrl = Util.GetUriFromImageUrl(value); }
        }
        public string HpUrl { get; set; }
        public string Place { get; set; }
    }

}
