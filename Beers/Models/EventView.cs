﻿using System;
namespace Beers.Models
{
    public class EventView
    {
		public int Id { get; set; }
        public string Name { get; set; }
		private Uri imageUrl { get; set; }
        public Uri ImageUrl
        {
			get { return imageUrl; }
			set { imageUrl = Util.GetUriFromImageUrl(value); }
        }
        public string Detail { get; set; }
        public string Place { get; set; }
        public string Schedule { get; set; }
        public string AimTotalPrize { get; set; }
    }
}
