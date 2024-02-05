using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using TwitterApi.Models;
using Tweetinvi;
using Newtonsoft.Json;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;

namespace TwitterApi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITwitterClient _twitterClient;

        public HomeController(ILogger<HomeController> logger, ITwitterClient twitterClient)
        {
            _logger = logger;
            _twitterClient = twitterClient;
        }

        public async Task<IActionResult> Index()
        {
            var poster = new ManageTweetPosts(_twitterClient);
            //create tweet
            //ITwitterResult result = await poster.PostTweet(
            //    new TweetPostModel
            //    {
            //        Text = "Hey, this is my new post from api request"
            //    }
            //);

            //delete tweet
            //ITwitterResult result = await poster.DeleteTweet("xxxxxxxxxxxxxxx");
            //if (result.Response.IsSuccessStatusCode == false)
            //{
            //    throw new Exception(
            //        "Error when posting tweet: " + Environment.NewLine + result.Content
            //    );
            //}

            //tweet lookup
            //var data = await poster.LookupTweet("xxxxxxxxxxxxxxxxxxx");

            //like tweet
            //LikeTweetModel model = new LikeTweetModel
            //{
            //    Tweet_Id = "xxxxxxxxxxxxxxxxxxx"
            //};
            //var liketweet = await poster.LikeTweet(model);

            //unlike tweet
            //var unliketweet = await poster.UnlikeTweet("xxxxxxxxxxxxxxxxxxx");

            //follow user
            //FollowUserModel model = new FollowUserModel
            //{
            //    Target_User_Id = "target_user_id"
            //};
            //var followuser = await poster.FollowUser(model);

            //unfollow user
            //var unfollowuser = await poster.UnfollowUser("target_user_id");

            //blocked users list
            //var blockuseelist = await poster.GetBlockedUsersList();

            //muted users list
            //var muteuseelist = await poster.GetMutedUsersList();

            //retweet tweet
            LikeTweetModel model = new LikeTweetModel
            {
                Tweet_Id = "1373888327535919110"
            };
            var retweet = await poster.Retweet(model);

            //undo retweet
            var undoretweet = await poster.UndoRetweet("1373888327535919110");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public class ManageTweetPosts
    {
        private readonly ITwitterClient client;
        public ManageTweetPosts(ITwitterClient client)
        {
            this.client = client;
        }

        public Task<ITwitterResult> PostTweet(TweetPostModel tweetParams)
        {
            return this.client.Execute.AdvanceRequestAsync(
                (ITwitterRequest request) =>
                {
                    var jsonBody = this.client.Json.Serialize(tweetParams);
                    var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                    request.Query.Url = "https://api.twitter.com/2/tweets";
                    request.Query.HttpMethod = Tweetinvi.Models.HttpMethod.POST;
                    request.Query.HttpContent = content;
                }
            );
        }

        public Task<ITwitterResult> DeleteTweet(string id)
        {
            return this.client.Execute.AdvanceRequestAsync(
                (ITwitterRequest request) =>
                {
                    request.Query.Url = "https://api.twitter.com/2/tweets/:id/".Replace(":id/", id);
                    request.Query.HttpMethod = Tweetinvi.Models.HttpMethod.DELETE;
                }
            );
        }
        
        public Task<ITwitterResult> LookupTweet(string id)
        {
            return this.client.Execute.AdvanceRequestAsync(
                (ITwitterRequest request) =>
                {
                    request.Query.Url = "https://api.twitter.com/2/tweets/:id".Replace(":id", id);
                    request.Query.HttpMethod = Tweetinvi.Models.HttpMethod.GET;
                }
            );
        }
        
        public Task<ITwitterResult> LikeTweet(LikeTweetModel likeTweet)
        {
            return this.client.Execute.AdvanceRequestAsync(
                (ITwitterRequest request) =>
                {
                    var jsonBody = this.client.Json.Serialize(likeTweet);
                    var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                    request.Query.Url = "https://api.twitter.com/2/users/:id/likes".Replace(":id", "user_numeric_id");
                    request.Query.HttpMethod = Tweetinvi.Models.HttpMethod.POST;
                    request.Query.HttpContent = content;
                }
            );
        }

        public Task<ITwitterResult> UnlikeTweet(string id)
        {
            return this.client.Execute.AdvanceRequestAsync(
                (ITwitterRequest request) =>
                {
                    request.Query.Url = "https://api.twitter.com/2/users/:id/likes/:tweet_id".Replace(":id", "user_numeric_id").Replace(":tweet_id", "xxxxxxxxxxxxxxxxxxx");
                    request.Query.HttpMethod = Tweetinvi.Models.HttpMethod.DELETE;
                }
            );
        }
        
        public Task<ITwitterResult> FollowUser(FollowUserModel followUser)
        {
            return this.client.Execute.AdvanceRequestAsync(
                (ITwitterRequest request) =>
                {
                    var jsonBody = this.client.Json.Serialize(followUser);
                    var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                    request.Query.Url = "https://api.twitter.com/2/users/:id/following".Replace(":id", "user_numeric_id");
                    request.Query.HttpMethod = Tweetinvi.Models.HttpMethod.POST;
                    request.Query.HttpContent = content;
                }
            );
        }
        
        public Task<ITwitterResult> UnfollowUser(string target_user_id)
        {
            return this.client.Execute.AdvanceRequestAsync(
                (ITwitterRequest request) =>
                {
                    request.Query.Url = "https://api.twitter.com/2/users/:id/following/:target_user_id".Replace(":id", "user_numeric_id").Replace(":target_user_id", target_user_id);
                    request.Query.HttpMethod = Tweetinvi.Models.HttpMethod.DELETE;
                }
            );
        }

        public Task<ITwitterResult> GetBlockedUsersList()
        {
            return this.client.Execute.AdvanceRequestAsync(
                (ITwitterRequest request) =>
                {
                    request.Query.Url = "https://api.twitter.com/2/users/:id/blocking".Replace(":id", "user_numeric_id");
                    request.Query.HttpMethod = Tweetinvi.Models.HttpMethod.GET;
                }
            );
        }
        
        public Task<ITwitterResult> GetMutedUsersList()
        {
            return this.client.Execute.AdvanceRequestAsync(
                (ITwitterRequest request) =>
                {
                    request.Query.Url = "https://api.twitter.com/2/users/:id/muting".Replace(":id", "user_numeric_id");
                    request.Query.HttpMethod = Tweetinvi.Models.HttpMethod.GET;
                }
            );
        }

        public Task<ITwitterResult> Retweet(LikeTweetModel likeTweet)
        {
            return this.client.Execute.AdvanceRequestAsync(
                (ITwitterRequest request) =>
                {
                    var jsonBody = this.client.Json.Serialize(likeTweet);
                    var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                    request.Query.Url = "https://api.twitter.com/2/users/:id/retweets".Replace(":id", "xxxxxxxxxxxxxxxxxx");
                    request.Query.HttpMethod = Tweetinvi.Models.HttpMethod.POST;
                    request.Query.HttpContent = content;
                }
            );
        }

        public Task<ITwitterResult> UndoRetweet(string source_tweet_id)
        {
            return this.client.Execute.AdvanceRequestAsync(
                (ITwitterRequest request) =>
                {
                    request.Query.Url = "https://api.twitter.com/2/users/:id/retweets/:source_tweet_id".Replace(":id", "xxxxxxxxxxxxxxxxxx").Replace(":source_tweet_id", source_tweet_id);
                    request.Query.HttpMethod = Tweetinvi.Models.HttpMethod.DELETE;
                }
            );
        }
    }

    public class TweetPostModel
    {
        [JsonProperty("text")]
        public string Text { get; set; } = string.Empty;
    }
    
    public class LikeTweetModel
    {
        [JsonProperty("tweet_id")]
        public string Tweet_Id { get; set; } = string.Empty;
    }
    
    public class FollowUserModel
    {
        [JsonProperty("target_user_id")]
        public string Target_User_Id { get; set; } = string.Empty;
    }
}