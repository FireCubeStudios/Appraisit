using Microsoft.Toolkit.Collections;
using Reddit;
using Reddit.Controllers;
using Reddit.Inputs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Appraisit.Helpers
{
    public enum TopOrder
    {
        All,
        Year,
        Month,
        Week,
        Day
    }

    public class TopPostsClass : IIncrementalSource<Posts>
    {
        public Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        public TopOrder Order = TopOrder.All;
        public string secret = "SESshAirmwAuAvBFHbq_JUkAMmk";
        public string appId = "-bL9o_t7kgNNmA";
        List<Posts> PostCollection;
        public int limit = 10;
        public int skipInt = 0;
        public async Task<IEnumerable<Posts>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default(CancellationToken))
        {
            await Task.Run(() =>
            {
                string refreshToken = localSettings.Values["refresh_token"].ToString();
                PostCollection = new List<Posts>();
                var reddit = new RedditAPI(appId, refreshToken, secret);
                var subreddit = reddit.Subreddit("Appraisit");
                var posts = subreddit.Posts.GetTop(new TimedCatSrListingInput(t: TopOrderToString(Order), limit: limit)).Skip(skipInt);
                limit = limit + 10;
                
                    foreach (Post post in posts)
                    {

                        // pageContent += Environment.NewLine + "### [" + post.Title + "](" + post.Permalink + ")" + Environment.NewLine;

                        var p = post as SelfPost;
                        // Console.WriteLine("New Post by " + post.Author + ": " + post.Title);
                        PostCollection.Add(new Posts()
                        {
                            TitleText = post.Title,
                            PostSelf = post,
                            PostAuthor = string.Format("PostBy".GetLocalized(), post.Author),
                            PostDate = string.Format("PostDate".GetLocalized(), post.Created),
                            PostUpvotes = post.UpVotes.ToString(),
                            PostDownvotes = post.DownVotes.ToString(),
                            PostCommentCount = post.Comments.GetComments("new").Count.ToString(),
                            PostFlair = post.Listing.LinkFlairText,
                            PostFlairColor = post.Listing.LinkFlairBackgroundColor
                        });
                    }
                
                // Simulates a longer request...
                skipInt = skipInt + 10;

            });
            // Gets items from the collection according to pageIndex and pageSize parameters.
           
            return PostCollection;


        }

        private string TopOrderToString(TopOrder to)
        {
            switch (to)
            {
                case TopOrder.All: return "all";
                case TopOrder.Day: return "day";
                case TopOrder.Month: return "month";
                case TopOrder.Week: return "week";
                case TopOrder.Year: return "year";
                default: return "all";
            }
        }
    }
}
