using System;
namespace Beers.Models
{
    public class Tweet
    {
		public string Id { get; set; }
        public string Name { get; set; }
		public string Created { get; set; }
		public string Text { get; set; }
		private Uri profile_image_url { get; set; }
		public Uri Profile_image_url
        {
			get { return profile_image_url; }
			set { profile_image_url = value; }
        }
    }
}
