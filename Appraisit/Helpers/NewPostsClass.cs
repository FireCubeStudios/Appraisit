using Microsoft.Toolkit.Collections;
using Reddit;
using Reddit.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace Appraisit.Helpers
{
    public class Posts
    {
        public string TitleText { get; set; }
        public string PostAuthor { get; set; }
        public string PostDate { get; set; }
        public Post PostSelf { get; set; }
        public string PostUpvotes { get; set; }
        public string PostDownvotes { get; set; }
        public string PostCommentCount { get; set; }
        public string PostFlair { get; set; }
        public string PostFlairColor { get; set; }
    }
    public class NewPostsClass : IIncrementalSource<Posts>
    {
        public Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        public string secret = "SESshAirmwAuAvBFHbq_JUkAMmk";
        public string appId = "-bL9o_t7kgNNmA";
        public int limit = 10;
        public int skipInt = 0;
        List<Posts> PostCollection;
        public async Task<IEnumerable<Posts>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default(CancellationToken))
        {
           
                await Task.Run(async () =>
            {
                string refreshToken = localSettings.Values["refresh_token"].ToString();
          try
          {
              PostCollection = new List<Posts>();
              var reddit = new RedditAPI(appId, refreshToken, secret);
              var subreddit = reddit.Subreddit("Appraisit");
              var posts = subreddit.Posts.GetNew(limit: limit).Skip(skipInt);
              limit = limit + 10;
              //if ()
              // {
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
                      PostCommentCount = (post.Comments.GetComments("new").Count.ToString()),
                      PostFlair = post.Listing.LinkFlairText,
                      PostFlairColor = post.Listing.LinkFlairBackgroundColor
                  });


                    }
              // }
              skipInt = skipInt + 10;


          }
          catch
          {
              try
              {
                  LoginHelper loginHelper = new LoginHelper(appId, secret);
                  var result = await loginHelper.Login_Refresh((string)localSettings.Values["refresh_token"]);
                  refreshToken = result.RefreshToken;
                  PostCollection = new List<Posts>();
                  var reddit = new RedditAPI(appId, refreshToken, secret);
                  var subreddit = reddit.Subreddit("Appraisit");
                  var posts = subreddit.Posts.GetNew(limit: limit);
                  limit = limit + 10;
                  if (posts.Count > 0)
                  {
                      foreach (Post post in posts.Skip(skipInt))
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
                              PostCommentCount = (post.Comments.GetComments("new").Count.ToString()),
                              PostFlair = post.Listing.LinkFlairText,
                              PostFlairColor = post.Listing.LinkFlairBackgroundColor
                          });



                      }
                  }
                  skipInt = skipInt + 10;
              }
              catch
              {
                  ContentDialog noWifiDialog = new ContentDialog()
                  {
                      Title = "Login denied",
                      Content = "Login denied, if you are unsure of how this app uses your reddit account then you can check the source code of the app here: https://github.com/FireCubeStudios/Appraisit",
                      CloseButtonText = "App might crash",
                      IsPrimaryButtonEnabled = false
                  };
              }

          }

         
      });
                
            return PostCollection;
            // Gets items from the collection according to pageIndex and pageSize parameters.
            //  var result = (from Posts in PostCollection
            // select Posts).Skip(pageIndex*pageSize).Take(pageSize);




        }
    }
}
