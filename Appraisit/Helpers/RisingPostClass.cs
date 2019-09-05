using Microsoft.Toolkit.Collections;
using Reddit;
using Reddit.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Appraisit.Helpers
{
    public class RisingPostsClass : IIncrementalSource<Posts>
    {
        public Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
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
                // Gets items from the collection according to pageIndex and pageSize parameters.
                PostCollection = new List<Posts>();
                var reddit = new RedditAPI(appId, refreshToken, secret);
                var subreddit = reddit.Subreddit("Appraisit");
                var posts = subreddit.Posts.GetRising(limit: limit).Skip(skipInt);
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
                            PostAuthor = "by: " + post.Author,
                            PostDate = "Created: " + post.Created,
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
          

            return PostCollection;


        }
    }
}
