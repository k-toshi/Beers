using System;
using Beers.Services;

namespace Beers.Models
{
    public static class Util
    {
        public static Uri GetUriFromImageUrl(Uri imageUrl) { return new Uri(HttpService.GetBaseUrl() + imageUrl); }
    }
}
