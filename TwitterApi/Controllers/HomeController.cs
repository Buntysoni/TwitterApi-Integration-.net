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
            //ITwitterResult result = await poster.PostTweet(
            //    new TweetPostModel
            //    {
            //        Text = "Hey, this is my new post from api request"
            //    }
            //);
            ITwitterResult result = await poster.DeleteTweet("1750112290836488268");
            if (result.Response.IsSuccessStatusCode == false)
            {
                throw new Exception(
                    "Error when posting tweet: " + Environment.NewLine + result.Content
                );
            }
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
    }

    public class TweetPostModel
    {
        [JsonProperty("text")]
        public string Text { get; set; } = string.Empty;
    }
}