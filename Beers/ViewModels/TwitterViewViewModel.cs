using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using Beers.Models;
using Beers.Controllers;

namespace Beers.ViewModels
{
	public class TwitterViewViewModel : BaseViewModel
    {
		public ObservableCollection<Tweet> Tweets { get; set; }
		public Command LoadTweetsCommand { get; set; }
        
		public TwitterViewViewModel()
        {
            Title = "関連ツイート";
			Tweets = new ObservableCollection<Tweet>();
			LoadTweetsCommand = new Command(async () => await ExecuteLoadTweetsCommand());
        }

		async Task ExecuteLoadTweetsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
				Tweets.Clear();
				await TwitterController.GetTweets("熱中症");
				var tweets = TwitterController.GetTweetStatuses();
				foreach (var t in tweets){
					Tweet tweet = new Tweet();
					tweet.Id = t["id"];
                    tweet.Name = t["user"]["name"];
					tweet.Created = t["created_at"];
					tweet.Text = t["text"];
					tweet.Profile_image_url = t["user"]["profile_image_url"];
					Tweets.Add(tweet);
				} 
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
