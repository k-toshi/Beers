using System;
using System.Threading.Tasks;
using Beers.Services;
using Beers.Models;
using Beers.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;

namespace Beers.Controllers
{
    public static class TwitterController
    {
		private static string token { get; set; }
		private static string baseUrl = "https://api.twitter.com/";
		private static string consumerKey = "xaY3me4EVDVO6q34JhfoQrtTi";
		private static string consumerSecret = "sxLBknoaxhVi6dtvimxIisZlA0J1T5GffBhnOUuANywi1FFYd6";
		private static dynamic tweetStatuses;
		//private static string maxid = "";

		public static async Task<string> SetToken()
        {
			string authUrl = baseUrl + "oauth2/token";

            Dictionary<string, string> contents = new Dictionary<string, string>
            {
				{"grant_type","client_credentials"}
            };
            
			byte[] bytes = System.Text.Encoding.UTF8.GetBytes(consumerKey + ":" + consumerSecret);
			string base64string = System.Convert.ToBase64String(bytes);

			HttpServiceResult<dynamic> hsr = await HttpService.PostDataFromService<dynamic>(authUrl, contents,base64string);

            if (!hsr.IsError)
            {
                token = hsr.ResultData["access_token"];
                return "";
            }
            else return hsr.ErrorMessage;

        }

        public static string GetToken()
		{
			return token;
		}

        public static dynamic GetTweetStatuses()
		{
			return tweetStatuses;
		}
        
        /// <summary>
		/// Gets the event tweet. todo:since_id = "+max_id+"& 
        /// </summary>
        /// <returns>The event tweet.</returns>
        public async static Task<string> GetTweets(string keyword)
		{
			/*
			HttpServiceResult<dynamic> hsr;
			if(String.IsNullOrEmpty(maxid)) hsr = await HttpService.GetTwitterDataFromServiceWithToken<dynamic>(baseUrl + "1.1/search/tweets.json?q=kirin&result_type=recent&include_entities=true");
			else hsr = await HttpService.GetTwitterDataFromServiceWithToken<dynamic>(baseUrl + "1.1/search/tweets.json?q=kirin&result_type=recent&include_entities=true&since_id=" + maxid);
			*/
			var encodedKeyword = WebUtility.UrlEncode("#" + keyword);
			HttpServiceResult<dynamic> hsr = await HttpService.GetTwitterDataFromServiceWithToken<dynamic>(baseUrl + "1.1/search/tweets.json?q=" + encodedKeyword + "&result_type=recent&include_entities=true");
            if (!hsr.IsError)
            {
                tweetStatuses = hsr.ResultData["statuses"];
				//maxid = tweetStatuses[0]["id"];
                return "";
            }
            else return hsr.ErrorMessage;
		}
    }
}
