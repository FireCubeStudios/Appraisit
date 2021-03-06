﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Reddit;
using Reddit.Controllers;
using Reddit.Inputs.Search;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Toolkit.Uwp;
using System.Linq;
using System.Web;
using Appraisit.Helpers;
using Windows.ApplicationModel.Core;
using MUXC = Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Composition;
using Windows.UI.Xaml.Hosting;
using System.Numerics;
using Microsoft.Graphics.Canvas.Effects;
using Windows.Graphics.Effects;
using Windows.UI.Xaml.Input;
using System.Text.RegularExpressions;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.Networking.Connectivity;
using Windows.System.Profile;
using Windows.UI.Xaml.Media;
using Windows.System;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Text;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Popups;

namespace Appraisit.Views
{
    public sealed partial class MainPage : Page, INotifyPropertyChanged

    {
        //string BackupappId = "fNIq4XWgq6T33w";
        int view = 1;
        private SpriteVisual _destinationSprite;
        ColorBloomTransitionHelper transition;
        private Compositor _compositor;
        OpenPosts ReferencePost;
        public string appId = "-bL9o_t7kgNNmA";
        // string backupRefreshToken = "209908787246-qLtBfs46Ci9dWAcesPmmCZt-lz0";
        public Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        public string refreshToken;
        public string BackuprefreshToken = "387967562864-jQyY5b8B4Ln2Cm5mNiyX65NteNo";
        public string accessToken;
        public string backupbackuprefreshToken = "";
        // string backupAccesToken = "209908787246-EHnGFWXgWZDrmpEv3iYmkXLB-ew";
        public string secret = "SESshAirmwAuAvBFHbq_JUkAMmk";
        public string FlairTemplate;
        List<Posts> PostCollection;
        List<Comments> CommentCollection;
        List<Comments> NewCommentCollection;
        List<Comments> TopCommentCollection;
        List<Comments> LiveCommentCollection;
        List<Comments> OldCommentCollection;
        List<Comments> ControversialCommentCollection;
        List<Comments> RandomCommentCollection;
        List<Comments> QACommentCollection;
        List<Comments> ReplyCollection;
        /* MainPage TopGridViewControl;
         MainPage HotGridViewControl;
         MainPage NewGridViewControl;
         MainPage RisingGridViewControl;
         MainPage SearchGridViewControl;
         readonly MainPage QACommentTreeViewControl;
         MainPage OldCommentTreeViewControl;
         MainPage LiveCommentTreeViewControl;
         MainPage NewCommentTreeViewControl;
         MainPage ControversialGridViewControl;
         MUXC.TreeView ControversialCommentTreeViewControl;
         MUXC.TreeView RandomCommentTreeViewControl;
         MUXC.TreeView UniversalCommentTreeViewControl;
         MUXC.TreeView TopCommentTreeViewControl;
         MUXC.TreeView RepliesCommentTreeViewControl;*/
        /* AdaptiveGridView TopGridViewControl;
         AdaptiveGridView HotGridViewControl;
         AdaptiveGridView NewGridViewControl;
         AdaptiveGridView RisingGridViewControl;
         AdaptiveGridView SearchGridViewControl;
         MUXC.TreeView QACommentTreeViewControl;
     MUXC.TreeView OldCommentTreeViewControl;
     MUXC.TreeView LiveCommentTreeViewControl;
     MUXC.TreeView NewCommentTreeViewControl;
     AdaptiveGridView ControversialGridViewControl;
     MUXC.TreeView ControversialCommentTreeViewControl;
     MUXC.TreeView RandomCommentTreeViewControl;
     MUXC.TreeView UniversalCommentTreeViewControl;
     MUXC.TreeView TopCommentTreeViewControl;
     MUXC.TreeView RepliesCommentTreeViewControl;*/
        public static InAppNotification UniversalPageNotificationPost { get; set; }
        public static MUXC.TeachingTip UniversalTipPost{ get; set; }
        public MainPage()
        {
            InitializeComponent();
            UniversalPageNotificationPost = UniversalPageNotification;
            UniversalTipPost = UniversalPageTip;
            // CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            // Window.Current.SetTitleBar(TitleGrid);
            /* if (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
             {
                 if ((string)localSettings.Values["refresh_token"] == BackuprefreshToken)
                 {
                     refreshToken = BackuprefreshToken;
                     MobileSignInBar.Visibility = Visibility.Visible;
                     UniversalPageTip.IsOpen = true;
                     UniversalPageTip.Title = "SignInTip".GetLocalized();
                     PivotBar.Visibility = Visibility.Collapsed;
                     //FindName("SB");
                     FindName("MobileBar");
                 }
                 else
                 {

                     if ((string)localSettings.Values["refresh_token"] == null)
                     {
                         refreshToken = BackuprefreshToken;
                         localSettings.Values["refresh_token"] = BackuprefreshToken;
                         MobileSignInBar.Visibility = Visibility.Visible;
                         UnloadObject(MobileBar);
                         UniversalPageTip.IsOpen = true;
                         UniversalPageTip.Title = "Sign in to access more features such as upvoting!";
                         PivotBar.Visibility = Visibility.Collapsed;
                         //FindName("SB");
                         FindName("MobileBar");
                     }
                     else
                     {
                         refreshToken = localSettings.Values["refresh_token"].ToString();
                         MobileSignInBar.Visibility = Visibility.Collapsed;
                         PivotBar.Visibility = Visibility.Collapsed;
                         FindName("SB");
                         FindName("MobileBar");
                     }
                 }
             }
             else
             {*/
            if ((string)localSettings.Values["refresh_token"] == BackuprefreshToken)
                {
                    refreshToken = BackuprefreshToken;
                    SignInBar.Visibility = Visibility.Visible;
                    UniversalPageTip.IsOpen = true;
                    UniversalPageTip.Title = "Sign in to access more features such as Upvoting";
                }
                else
                {

                    if ((string)localSettings.Values["refresh_token"] == null)
                    {
                        refreshToken = BackuprefreshToken;
                        localSettings.Values["refresh_token"] = BackuprefreshToken;
                        SignInBar.Visibility = Visibility.Visible;

                        UniversalPageTip.IsOpen = true;
                        UniversalPageTip.Title = "Sign in to access more features such as upvote, downvote and more!";
                    }
                    else
                    {
                        refreshToken = localSettings.Values["refresh_token"].ToString();
                        SignInBar.Visibility = Visibility.Collapsed;

                    }
                }
          
            /*ProgressRing.IsActive = true;
            var scopes = Constants.Constants.scopeList.Aggregate("", (acc, x) => acc + " " + x);
            var urlParams = "client_id=" + appId + "&response_type=code&state=uyagsjgfhjs&duration=permanent&redirect_uri=" + HttpUtility.UrlEncode("http://127.0.0.1:3000/reddit_callback") + "&scope=" + HttpUtility.UrlEncode(scopes);
            Uri targetUri = new Uri(Constants.Constants.redditApiBaseUrl + "authorize?" + urlParams);
            ContentGrid.Visibility = Visibility.Collapsed;
            FindName("loginView");
            FindName("WebBlockBar");
            loginView.Navigate(targetUri);
            ProgressRing.IsActive = false;*/
            SortBox.SelectedItem = "New";  
        }

        public class OpenPosts
        {
            public Post PostSelf { get; set; }
        }

        public class Comments
        {
            public string CommentAuthor { get; set; }
            public string CommentText { get; set; }
            public string CommentDate { get; set; }
            public string CommentUpvotes { get; set; }
            public string CommentDownvotes { get; set; }
            public Comment CommentSelf { get; set; }
        }
        private async void Combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {

                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();

                try
                {
                    if (connectionProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess)
                    {
                        ProgressRing.IsActive = true;
                        /* transition = new ColorBloomTransitionHelper(hostForVisual);
                         var header = sender as Pivot;
                         var headerPosition = header.TransformToVisual(PivotNavigator).TransformPoint(new Windows.Foundation.Point(0d, 0d));
                         var initialBounds = new Windows.Foundation.Rect()  // maps to a rectangle the size of the header
                         {
                             Width = header.RenderSize.Width,

                             Height = header.RenderSize.Height,

                             X = headerPosition.X,

                             Y = headerPosition.Y
                         };
                         var finalBounds = Window.Current.Bounds;  // maps to the bounds of the current window
                         transition.Start(Windows.UI.Colors.Purple,  // the color for the circlular bloom
                                          initialBounds,                                  // the initial size and position
                                                    finalBounds);*/
                        switch (SortBox.SelectedItem.ToString())
                        {
                            case "New":
                                try
                                {
                                    var Newcollection = new IncrementalLoadingCollection<NewPostsClass, Posts>();
                                    UniversalGridViewControl.ItemsSource = Newcollection;
                                    UnloadObject(TopSort);
                                }
                                catch
                                {
                                    RefreshAccess();
                                }
                                ProgressRing.IsActive = false;
                                break;
                            case "Hot":
                                var Hotcollection = new IncrementalLoadingCollection<HotPostsClass, Posts>();
                                UniversalGridViewControl.ItemsSource = Hotcollection;
                                UnloadObject(TopSort);
                                ProgressRing.IsActive = false;
                                break;
                            case "Top":
                                var Topcollection = new IncrementalLoadingCollection<TopPostsClass, Posts>();
                                UniversalGridViewControl.ItemsSource = Topcollection;
                                FindName("TopSort");
                                TopSort.ItemsSource = "TopOrderItems".GetLocalized().Split('|');
                                TopSort.SelectedItem = "Day";
                                ProgressRing.IsActive = false;
                                break;
                            case "Rising":
                                var Risingcollection = new IncrementalLoadingCollection<HotPostsClass, Posts>();
                                UniversalGridViewControl.ItemsSource = Risingcollection;
                                UnloadObject(TopSort);
                                ProgressRing.IsActive = false;
                                break;
                            case "Controversial":
                                var Controversialcollection = new IncrementalLoadingCollection<HotPostsClass, Posts>();
                                UniversalGridViewControl.ItemsSource = Controversialcollection;
                                UnloadObject(TopSort);
                                ProgressRing.IsActive = false;
                                break;
                        }
                        ProgressRing.IsActive = false;
                        // the area to fill over the animation duration
                    }
                }
                catch
                {
                    if (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
                    {
                        UniversalPostTip.IsOpen = true;
                        UniversalPostTip.Title = "NoInternetConnection".GetLocalized();
                    }
                    else
                    {
                        UniversalPageNotification.Show("NoInternetConnection".GetLocalized());
                    }
                }


            });
        }
        public async void RefreshAccess()
        {

            LoginHelper loginHelper = new LoginHelper(appId, secret);
            var result = await loginHelper.Login_Refresh((string)localSettings.Values["refresh_token"]);
            refreshToken = result.RefreshToken;

        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        /* private void OnGoBack(object sender, RoutedEventArgs e)
         {
             if (webView.CanGoBack == true)
             {
                 webView.GoBack();
             }
         }

         private void OnGoForward(object sender, RoutedEventArgs e)
         {
             if (webView.CanGoForward == true)
             {
                 webView.GoForward();
             }
         }

         private void OnRefresh(object sender, RoutedEventArgs e)
         {
             webView.Refresh();
         }
         private void HomeWeb_Click(object sender, RoutedEventArgs e)
         {
             Uri Home = new Uri("https://www.reddit.com/r/appraisit/");
             webView.Navigate(Home);
         }

         private void WebviewPanel_NewWindowRequested(WebView sender, WebViewNewWindowRequestedEventArgs args)
         {
             webView.NavigateToString(webView.Source.ToString());
         }*/

        private async void UniversalGridViewControl_ItemClick(object sender, ItemClickEventArgs e)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                FindName("PostDialog");
                CommentCollection = new List<Comments>();
                NewCommentCollection = new List<Comments>();
                TopCommentCollection = new List<Comments>();
                ControversialCommentCollection = new List<Comments>();
                QACommentCollection = new List<Comments>();
                RandomCommentCollection = new List<Comments>();
                OldCommentCollection = new List<Comments>();
                LiveCommentCollection = new List<Comments>();
                ReplyCollection = new List<Comments>();
                FindName("SearchTip");
                SearchTip.IsOpen = false;
                UnloadObject(SearchTip);
                _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
                _destinationSprite = _compositor.CreateSpriteVisual();

                _destinationSprite.Size = new Vector2((float)ContentGrid.ActualWidth, (float)ContentGrid.ActualHeight);

                ElementCompositionPreview.SetElementChildVisual(ContentGrid, _destinationSprite);

                _destinationSprite.IsVisible = true;
                IGraphicsEffect graphicsEffect = null;
                string[] animatableProperties = null;
                graphicsEffect = new GaussianBlurEffect()

                {
                    Name = "Blur",
                    BlurAmount = 10,

                    Source = new CompositionEffectSourceParameter("ContentGrid"),

                    Optimization = EffectOptimization.Balanced,

                    BorderMode = EffectBorderMode.Hard,

                };
                CompositionEffectFactory _effectFactory = _compositor.CreateEffectFactory(graphicsEffect, animatableProperties);
                CompositionEffectBrush brush = _effectFactory.CreateBrush();
                brush.SetSourceParameter("ContentGrid", _compositor.CreateBackdropBrush());
                _destinationSprite.Brush = brush;
                ScalarKeyFrameAnimation showAnimation = _compositor.CreateScalarKeyFrameAnimation();

                showAnimation.InsertKeyFrame(0f, 0f);

                showAnimation.InsertKeyFrame(1f, 1f);

                showAnimation.Duration = TimeSpan.FromMilliseconds(1500);

                _destinationSprite.StartAnimation("Opacity", showAnimation);
                /* var Enfo = e.ClickedItem as Posts;
                 ConnectedAnimation ConnectedAnimation = NewGridViewControl.PrepareConnectedAnimation("forwardAnimation", Enfo, "Flair");
                 ConnectedAnimation.Configuration = new DirectConnectedAnimationConfiguration();
                 ConnectedAnimation.TryStart(destinationElement);*/
                if (e.ClickedItem != null)
                {
                    try
                    {
                        var info = e.ClickedItem as Posts;
                        var S = info.PostSelf as SelfPost;
                        String Title = info.TitleText;
                        String Author = info.PostAuthor;
                        string Date = info.PostDate;
                        Post postComment = info.PostSelf;
                        PostDialog.Title = Title;
                        AuthorText.Text = Author;
                        CreatedText.Text = Date;
                        PostContentText.Visibility = Visibility.Visible;
                        LinkPostLink.Visibility = Visibility.Collapsed;
                        PostContentText.Text = S.SelfText;
                        FlairText.Text = string.Format("Flair".GetLocalized(), S.Listing.LinkFlairText);
                        LinkNavigator.IsEnabled = false;
                        ReferencePost = new OpenPosts()
                        {
                            PostSelf = info.PostSelf
                        };
                        await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                        {
                            var CommentsList = postComment.Comments.GetConfidence();
                            if (CommentsList.Count > 0)
                            {
                                foreach (Comment commentObject in CommentsList)
                                {
                                    CommentCollection.Add(new Comments()
                                    {
                                        CommentAuthor = commentObject.Author,
                                        CommentDate = commentObject.Created.ToString(),
                                        CommentText = commentObject.Body,
                                        CommentDownvotes = commentObject.DownVotes.ToString(),
                                        CommentUpvotes = commentObject.UpVotes.ToString(),
                                        CommentSelf = commentObject
                                    });
                                    /* var CommentReplies = commentObject.Comments.GetComments();
                                     if (CommentReplies.Count > 0)
                                     {
                                         foreach (Comment ReplyObject in CommentReplies)
                                         {
                                             ReplyCollection.Add(new Comments()
                                             {
                                                 CommentAuthor = commentObject.Author,
                                                 CommentDate = commentObject.Created.ToString(),
                                                 CommentText = commentObject.Body,
                                             });
                                             ReplyCommentTreeViewControl.ItemsSource = ReplyCollection;
                                            /* MUXC.TreeViewNode replyNode = new MUXC.TreeViewNode();
                                             replyNode.Content = CommentCollection;
                                             replyNode.Children.Add(replyNode);
                                             MUXC.TreeViewNode rootNode = new MUXC.TreeViewNode();
                                             rootNode.Content = CommentCollection;
                                             UniversalCommentTreeViewControl.RootNodes.Add(rootNode);
                                         }
                                     }

                                    // else
                                     //{*/
                                    // MUXC.TreeViewNode rootNode = new MUXC.TreeViewNode();
                                    //   rootNode.Content = CommentCollection;
                                    //   UniversalCommentTreeViewControl.RootNodes.Add(rootNode);
                                    // }

                                }
                                UniversalCommentTreeViewControl.ItemsSource = CommentCollection;
                            }
                            var NewCommentsList = postComment.Comments.GetNew();
                            if (NewCommentsList.Count > 0)
                            {
                                foreach (Comment commentObject in NewCommentsList)
                                {
                                    NewCommentCollection.Add(new Comments()
                                    {
                                        CommentAuthor = commentObject.Author,
                                        CommentDate = commentObject.Created.ToString(),
                                        CommentText = commentObject.Body,
                                        CommentDownvotes = commentObject.DownVotes.ToString(),
                                        CommentUpvotes = commentObject.UpVotes.ToString(),
                                        CommentSelf = commentObject
                                    });
                                    try
                                    {
                                        var CommentReplies = commentObject.Replies;
                                        if (CommentReplies.Count > 0)
                                        {
                                            foreach (Comment CommentObject in CommentReplies)
                                            {
                                                ReplyCollection.Add(new Comments()
                                                {
                                                    CommentAuthor = CommentObject.Author,
                                                    CommentDate = CommentObject.Created.ToString(),
                                                    CommentText = CommentObject.Body,
                                                    CommentDownvotes = CommentObject.DownVotes.ToString(),
                                                    CommentUpvotes = CommentObject.UpVotes.ToString(),
                                                    CommentSelf = CommentObject,
                                                });

                                            }
                                            RepliesCommentTreeViewControl.ItemsSource = ReplyCollection;

                                        }


                                    }
                                    catch
                                    {

                                    }
                                }
                                NewCommentTreeViewControl.ItemsSource = NewCommentCollection;
                            }
                            var TopCommentsList = postComment.Comments.GetTop();
                            if (TopCommentsList.Count > 0)
                            {
                                foreach (Comment commentObject in TopCommentsList)
                                {
                                    TopCommentCollection.Add(new Comments()
                                    {
                                        CommentAuthor = commentObject.Author,
                                        CommentDate = commentObject.Created.ToString(),
                                        CommentText = commentObject.Body,
                                        CommentDownvotes = commentObject.DownVotes.ToString(),
                                        CommentUpvotes = commentObject.UpVotes.ToString(),
                                        CommentSelf = commentObject
                                    });
                                    try
                                    {
                                        var CommentReplies = commentObject.Replies;
                                        if (CommentReplies.Count > 0)
                                        {
                                            foreach (Comment CommentObject in CommentReplies)
                                            {
                                                ReplyCollection.Add(new Comments()
                                                {
                                                    CommentAuthor = CommentObject.Author,
                                                    CommentDate = CommentObject.Created.ToString(),
                                                    CommentText = CommentObject.Body,
                                                    CommentDownvotes = CommentObject.DownVotes.ToString(),
                                                    CommentUpvotes = CommentObject.UpVotes.ToString(),
                                                    CommentSelf = CommentObject,
                                                });

                                            }
                                            RepliesCommentTreeViewControl.ItemsSource = ReplyCollection;

                                        }


                                    }
                                    catch
                                    {

                                    }

                                }
                                TopCommentTreeViewControl.ItemsSource = TopCommentCollection;
                            }
                            var OldCommentsList = postComment.Comments.GetOld();
                            if (OldCommentsList.Count > 0)
                            {
                                foreach (Comment commentObject in OldCommentsList)
                                {
                                    OldCommentCollection.Add(new Comments()
                                    {
                                        CommentAuthor = commentObject.Author,
                                        CommentDate = commentObject.Created.ToString(),
                                        CommentText = commentObject.Body,
                                        CommentDownvotes = commentObject.DownVotes.ToString(),
                                        CommentUpvotes = commentObject.UpVotes.ToString(),
                                        CommentSelf = commentObject
                                    });
                                    /*var CommentReplies = commentObject.Replies;
                                    if (CommentReplies.Count > 0)
                                    {
                                        foreach (UIElement ReplyObject in CommentReplies)
                                        {
                                            ReplyCollection.Add(new Comments()
                                            {
                                                CommentAuthor = commentObject.Author,
                                                CommentDate = commentObject.Created.ToString(),
                                                CommentText = commentObject.Body,
                                            });
                                            MUXC.TreeViewNode replyNode = new MUXC.TreeViewNode();
                                            replyNode.Content = CommentCollection;
                                            replyNode.Children.Add(replyNode);
                                            MUXC.TreeViewNode rootNode = new MUXC.TreeViewNode();
                                            rootNode.Content = CommentCollection;
                                            UniversalCommentTreeViewControl.RootNodes.Add(rootNode);
                                        }
                                    }

                                    else
                                    {*/
                                    // MUXC.TreeViewNode rootNode = new MUXC.TreeViewNode();
                                    //   rootNode.Content = CommentCollection;
                                    //   UniversalCommentTreeViewControl.RootNodes.Add(rootNode);
                                    // }

                                }
                                OldCommentTreeViewControl.ItemsSource = OldCommentCollection;
                            }
                            var ControversialCommentsList = postComment.Comments.GetControversial();
                            if (ControversialCommentsList.Count > 0)
                            {
                                foreach (Comment commentObject in ControversialCommentsList)
                                {
                                    ControversialCommentCollection.Add(new Comments()
                                    {
                                        CommentAuthor = commentObject.Author,
                                        CommentDate = commentObject.Created.ToString(),
                                        CommentText = commentObject.Body,
                                        CommentDownvotes = commentObject.DownVotes.ToString(),
                                        CommentUpvotes = commentObject.UpVotes.ToString(),
                                        CommentSelf = commentObject
                                    });
                                    /*var CommentReplies = commentObject.Replies;
                                    if (CommentReplies.Count > 0)
                                    {
                                        foreach (UIElement ReplyObject in CommentReplies)
                                        {
                                            ReplyCollection.Add(new Comments()
                                            {
                                                CommentAuthor = commentObject.Author,
                                                CommentDate = commentObject.Created.ToString(),
                                                CommentText = commentObject.Body,
                                            });
                                            MUXC.TreeViewNode replyNode = new MUXC.TreeViewNode();
                                            replyNode.Content = CommentCollection;
                                            replyNode.Children.Add(replyNode);
                                            MUXC.TreeViewNode rootNode = new MUXC.TreeViewNode();
                                            rootNode.Content = CommentCollection;
                                            UniversalCommentTreeViewControl.RootNodes.Add(rootNode);
                                        }
                                    }

                                    else
                                    {*/
                                    // MUXC.TreeViewNode rootNode = new MUXC.TreeViewNode();
                                    //   rootNode.Content = CommentCollection;
                                    //   UniversalCommentTreeViewControl.RootNodes.Add(rootNode);
                                    // }

                                }
                                ControversialCommentTreeViewControl.ItemsSource = ControversialCommentCollection;
                            }
                            var RandomCommentsList = postComment.Comments.GetRandom();
                            if (RandomCommentsList.Count > 0)
                            {
                                foreach (Comment commentObject in RandomCommentsList)
                                {
                                    RandomCommentCollection.Add(new Comments()
                                    {
                                        CommentAuthor = commentObject.Author,
                                        CommentDate = commentObject.Created.ToString(),
                                        CommentText = commentObject.Body,
                                        CommentDownvotes = commentObject.DownVotes.ToString(),
                                        CommentUpvotes = commentObject.UpVotes.ToString(),
                                        CommentSelf = commentObject
                                    });
                                    /*var CommentReplies = commentObject.Replies;
                                    if (CommentReplies.Count > 0)
                                    {
                                        foreach (UIElement ReplyObject in CommentReplies)
                                        {
                                            ReplyCollection.Add(new Comments()
                                            {
                                                CommentAuthor = commentObject.Author,
                                                CommentDate = commentObject.Created.ToString(),
                                                CommentText = commentObject.Body,
                                            });
                                            MUXC.TreeViewNode replyNode = new MUXC.TreeViewNode();
                                            replyNode.Content = CommentCollection;
                                            replyNode.Children.Add(replyNode);
                                            MUXC.TreeViewNode rootNode = new MUXC.TreeViewNode();
                                            rootNode.Content = CommentCollection;
                                            UniversalCommentTreeViewControl.RootNodes.Add(rootNode);
                                        }
                                    }

                                    else
                                    {*/
                                    // MUXC.TreeViewNode rootNode = new MUXC.TreeViewNode();
                                    //   rootNode.Content = CommentCollection;
                                    //   UniversalCommentTreeViewControl.RootNodes.Add(rootNode);
                                    // }

                                }

                            }
                            var liveCommentsList = postComment.Comments.GetLive();
                            if (liveCommentsList.Count > 0)
                            {
                                foreach (Comment commentObject in liveCommentsList)
                                {
                                    LiveCommentCollection.Add(new Comments()
                                    {
                                        CommentAuthor = commentObject.Author,
                                        CommentDate = commentObject.Created.ToString(),
                                        CommentText = commentObject.Body,
                                        CommentDownvotes = commentObject.DownVotes.ToString(),
                                        CommentUpvotes = commentObject.UpVotes.ToString(),
                                        CommentSelf = commentObject
                                    });
                                    /*var CommentReplies = commentObject.Replies;
                                    if (CommentReplies.Count > 0)
                                    {
                                        foreach (UIElement ReplyObject in CommentReplies)
                                        {
                                            ReplyCollection.Add(new Comments()
                                            {
                                                CommentAuthor = commentObject.Author,
                                                CommentDate = commentObject.Created.ToString(),
                                                CommentText = commentObject.Body,
                                            });
                                            MUXC.TreeViewNode replyNode = new MUXC.TreeViewNode();
                                            replyNode.Content = CommentCollection;
                                            replyNode.Children.Add(replyNode);
                                            MUXC.TreeViewNode rootNode = new MUXC.TreeViewNode();
                                            rootNode.Content = CommentCollection;
                                            UniversalCommentTreeViewControl.RootNodes.Add(rootNode);
                                        }
                                    }

                                    else
                                    {*/
                                    // MUXC.TreeViewNode rootNode = new MUXC.TreeViewNode();
                                    //   rootNode.Content = CommentCollection;
                                    //   UniversalCommentTreeViewControl.RootNodes.Add(rootNode);
                                    // }

                                }
                                LiveCommentTreeViewControl.ItemsSource = LiveCommentCollection;
                            }
                            var QACommentsList = postComment.Comments.GetQA();
                            if (QACommentsList.Count > 0)
                            {
                                foreach (Comment commentObject in QACommentsList)
                                {
                                    QACommentCollection.Add(new Comments()
                                    {
                                        CommentAuthor = commentObject.Author,
                                        CommentDate = commentObject.Created.ToString(),
                                        CommentText = commentObject.Body,
                                        CommentDownvotes = commentObject.DownVotes.ToString(),
                                        CommentUpvotes = commentObject.UpVotes.ToString(),
                                        CommentSelf = commentObject
                                    });
                                    /*var CommentReplies = commentObject.Replies;
                                    if (CommentReplies.Count > 0)
                                    {
                                        foreach (UIElement ReplyObject in CommentReplies)
                                        {
                                            ReplyCollection.Add(new Comments()
                                            {
                                                CommentAuthor = commentObject.Author,
                                                CommentDate = commentObject.Created.ToString(),
                                                CommentText = commentObject.Body,
                                            });
                                            MUXC.TreeViewNode replyNode = new MUXC.TreeViewNode();
                                            replyNode.Content = CommentCollection;
                                            replyNode.Children.Add(replyNode);
                                            MUXC.TreeViewNode rootNode = new MUXC.TreeViewNode();
                                            rootNode.Content = CommentCollection;
                                            UniversalCommentTreeViewControl.RootNodes.Add(rootNode);
                                        }
                                    }

                                    else
                                    {*/
                                    // MUXC.TreeViewNode rootNode = new MUXC.TreeViewNode();
                                    //   rootNode.Content = CommentCollection;
                                    //   UniversalCommentTreeViewControl.RootNodes.Add(rootNode);
                                    // }

                                }
                                QACommentTreeViewControl.ItemsSource = QACommentCollection;
                            }

                            else
                            {
                                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                                {
                                    TopCommentTreeViewControl.ItemsSource = TopCommentCollection;
                                    QACommentTreeViewControl.ItemsSource = QACommentCollection;
                                    LiveCommentTreeViewControl.ItemsSource = LiveCommentCollection;
                                    RandomCommentTreeViewControl.ItemsSource = RandomCommentCollection;
                                    ControversialCommentTreeViewControl.ItemsSource = ControversialCommentCollection;
                                    NewCommentTreeViewControl.ItemsSource = NewCommentCollection;
                                    TopCommentTreeViewControl.ItemsSource = TopCommentCollection;
                                    OldCommentTreeViewControl.ItemsSource = OldCommentCollection;
                                    UniversalCommentTreeViewControl.ItemsSource = CommentCollection;
                                });
                            }
                            await PostDialog.ShowAsync();
                        });
                    }
                    catch
                    {
                        var info = e.ClickedItem as Posts;
                        var S = info.PostSelf as SelfPost;
                        var L = info.PostSelf as LinkPost;
                        LinkNavigator.IsEnabled = true;
                        String Title = info.TitleText;
                        String Author = info.PostAuthor;
                        string Date = info.PostDate;
                        Post postComment = info.PostSelf;
                        PostDialog.Title = Title;
                        AuthorText.Text = Author;
                        CreatedText.Text = Date;
                        Uri Link = new Uri(L.URL.ToString());
                        LinkPostLink.Visibility = Visibility.Visible;
                        PostContentText.Visibility = Visibility.Collapsed;
                        FlairText.Text = string.Format("Flair".GetLocalized(), L.Listing.LinkFlairText);
                        LinkPostLink.NavigateUri = Link;
                        ReferencePost = new OpenPosts()
                        {
                            PostSelf = info.PostSelf
                        };
                        await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                        {
                            var CommentsList = postComment.Comments.GetConfidence();
                            if (CommentsList.Count > 0)
                            {
                                foreach (Comment commentObject in CommentsList)
                                {
                                    CommentCollection.Add(new Comments()
                                    {
                                        CommentAuthor = commentObject.Author,
                                        CommentDate = commentObject.Created.ToString(),
                                        CommentText = commentObject.Body,
                                        CommentDownvotes = commentObject.DownVotes.ToString(),
                                        CommentUpvotes = commentObject.UpVotes.ToString(),
                                        CommentSelf = commentObject
                                    });
                                    /* var CommentReplies = commentObject.Comments.GetComments();
                                     if (CommentReplies.Count > 0)
                                     {
                                         foreach (Comment ReplyObject in CommentReplies)
                                         {
                                             ReplyCollection.Add(new Comments()
                                             {
                                                 CommentAuthor = commentObject.Author,
                                                 CommentDate = commentObject.Created.ToString(),
                                                 CommentText = commentObject.Body,
                                             });
                                             ReplyCommentTreeViewControl.ItemsSource = ReplyCollection;
                                            /* MUXC.TreeViewNode replyNode = new MUXC.TreeViewNode();
                                             replyNode.Content = CommentCollection;
                                             replyNode.Children.Add(replyNode);
                                             MUXC.TreeViewNode rootNode = new MUXC.TreeViewNode();
                                             rootNode.Content = CommentCollection;
                                             UniversalCommentTreeViewControl.RootNodes.Add(rootNode);
                                         }
                                     }

                                    // else
                                     //{*/
                                    // MUXC.TreeViewNode rootNode = new MUXC.TreeViewNode();
                                    //   rootNode.Content = CommentCollection;
                                    //   UniversalCommentTreeViewControl.RootNodes.Add(rootNode);
                                    // }

                                }
                                UniversalCommentTreeViewControl.ItemsSource = CommentCollection;
                            }
                            var NewCommentsList = postComment.Comments.GetNew();
                            if (NewCommentsList.Count > 0)
                            {
                                foreach (Comment commentObject in NewCommentsList)
                                {
                                    NewCommentCollection.Add(new Comments()
                                    {
                                        CommentAuthor = commentObject.Author,
                                        CommentDate = commentObject.Created.ToString(),
                                        CommentText = commentObject.Body,
                                        CommentDownvotes = commentObject.DownVotes.ToString(),
                                        CommentUpvotes = commentObject.UpVotes.ToString(),
                                        CommentSelf = commentObject
                                    });
                                    /*var CommentReplies = commentObject.Replies;
                                    if (CommentReplies.Count > 0)
                                    {
                                        foreach (UIElement ReplyObject in CommentReplies)
                                        {
                                            ReplyCollection.Add(new Comments()
                                            {
                                                CommentAuthor = commentObject.Author,
                                                CommentDate = commentObject.Created.ToString(),
                                                CommentText = commentObject.Body,
                                            });
                                            MUXC.TreeViewNode replyNode = new MUXC.TreeViewNode();
                                            replyNode.Content = CommentCollection;
                                            replyNode.Children.Add(replyNode);
                                            MUXC.TreeViewNode rootNode = new MUXC.TreeViewNode();
                                            rootNode.Content = CommentCollection;
                                            UniversalCommentTreeViewControl.RootNodes.Add(rootNode);
                                        }
                                    }

                                    else
                                    {*/
                                    // MUXC.TreeViewNode rootNode = new MUXC.TreeViewNode();
                                    //   rootNode.Content = CommentCollection;
                                    //   UniversalCommentTreeViewControl.RootNodes.Add(rootNode);
                                    // }

                                }
                                NewCommentTreeViewControl.ItemsSource = NewCommentCollection;
                            }
                            var TopCommentsList = postComment.Comments.GetTop();
                            if (TopCommentsList.Count > 0)
                            {
                                foreach (Comment commentObject in TopCommentsList)
                                {
                                    TopCommentCollection.Add(new Comments()
                                    {
                                        CommentAuthor = commentObject.Author,
                                        CommentDate = commentObject.Created.ToString(),
                                        CommentText = commentObject.Body,
                                        CommentDownvotes = commentObject.DownVotes.ToString(),
                                        CommentUpvotes = commentObject.UpVotes.ToString(),
                                        CommentSelf = commentObject
                                    });
                                    /*var CommentReplies = commentObject.Replies;
                                    if (CommentReplies.Count > 0)
                                    {
                                        foreach (UIElement ReplyObject in CommentReplies)
                                        {
                                            ReplyCollection.Add(new Comments()
                                            {
                                                CommentAuthor = commentObject.Author,
                                                CommentDate = commentObject.Created.ToString(),
                                                CommentText = commentObject.Body,
                                            });
                                            MUXC.TreeViewNode replyNode = new MUXC.TreeViewNode();
                                            replyNode.Content = CommentCollection;
                                            replyNode.Children.Add(replyNode);
                                            MUXC.TreeViewNode rootNode = new MUXC.TreeViewNode();
                                            rootNode.Content = CommentCollection;
                                            UniversalCommentTreeViewControl.RootNodes.Add(rootNode);
                                        }
                                    }

                                    else
                                    {*/
                                    // MUXC.TreeViewNode rootNode = new MUXC.TreeViewNode();
                                    //   rootNode.Content = CommentCollection;
                                    //   UniversalCommentTreeViewControl.RootNodes.Add(rootNode);
                                    // }

                                }
                                TopCommentTreeViewControl.ItemsSource = TopCommentCollection;
                            }
                            var OldCommentsList = postComment.Comments.GetOld();
                            if (OldCommentsList.Count > 0)
                            {
                                foreach (Comment commentObject in OldCommentsList)
                                {
                                    OldCommentCollection.Add(new Comments()
                                    {
                                        CommentAuthor = commentObject.Author,
                                        CommentDate = commentObject.Created.ToString(),
                                        CommentText = commentObject.Body,
                                        CommentDownvotes = commentObject.DownVotes.ToString(),
                                        CommentUpvotes = commentObject.UpVotes.ToString(),
                                        CommentSelf = commentObject
                                    });
                                    /*var CommentReplies = commentObject.Replies;
                                    if (CommentReplies.Count > 0)
                                    {
                                        foreach (UIElement ReplyObject in CommentReplies)
                                        {
                                            ReplyCollection.Add(new Comments()
                                            {
                                                CommentAuthor = commentObject.Author,
                                                CommentDate = commentObject.Created.ToString(),
                                                CommentText = commentObject.Body,
                                            });
                                            MUXC.TreeViewNode replyNode = new MUXC.TreeViewNode();
                                            replyNode.Content = CommentCollection;
                                            replyNode.Children.Add(replyNode);
                                            MUXC.TreeViewNode rootNode = new MUXC.TreeViewNode();
                                            rootNode.Content = CommentCollection;
                                            UniversalCommentTreeViewControl.RootNodes.Add(rootNode);
                                        }
                                    }

                                    else
                                    {*/
                                    // MUXC.TreeViewNode rootNode = new MUXC.TreeViewNode();
                                    //   rootNode.Content = CommentCollection;
                                    //   UniversalCommentTreeViewControl.RootNodes.Add(rootNode);
                                    // }

                                }
                                OldCommentTreeViewControl.ItemsSource = OldCommentCollection;
                            }
                            var ControversialCommentsList = postComment.Comments.GetControversial();
                            if (ControversialCommentsList.Count > 0)
                            {
                                foreach (Comment commentObject in ControversialCommentsList)
                                {
                                    ControversialCommentCollection.Add(new Comments()
                                    {
                                        CommentAuthor = commentObject.Author,
                                        CommentDate = commentObject.Created.ToString(),
                                        CommentText = commentObject.Body,
                                        CommentDownvotes = commentObject.DownVotes.ToString(),
                                        CommentUpvotes = commentObject.UpVotes.ToString(),
                                        CommentSelf = commentObject
                                    });
                                    /*var CommentReplies = commentObject.Replies;
                                    if (CommentReplies.Count > 0)
                                    {
                                        foreach (UIElement ReplyObject in CommentReplies)
                                        {
                                            ReplyCollection.Add(new Comments()
                                            {
                                                CommentAuthor = commentObject.Author,
                                                CommentDate = commentObject.Created.ToString(),
                                                CommentText = commentObject.Body,
                                            });
                                            MUXC.TreeViewNode replyNode = new MUXC.TreeViewNode();
                                            replyNode.Content = CommentCollection;
                                            replyNode.Children.Add(replyNode);
                                            MUXC.TreeViewNode rootNode = new MUXC.TreeViewNode();
                                            rootNode.Content = CommentCollection;
                                            UniversalCommentTreeViewControl.RootNodes.Add(rootNode);
                                        }
                                    }

                                    else
                                    {*/
                                    // MUXC.TreeViewNode rootNode = new MUXC.TreeViewNode();
                                    //   rootNode.Content = CommentCollection;
                                    //   UniversalCommentTreeViewControl.RootNodes.Add(rootNode);
                                    // }

                                }
                                ControversialCommentTreeViewControl.ItemsSource = ControversialCommentCollection;
                            }
                            var RandomCommentsList = postComment.Comments.GetRandom();
                            if (RandomCommentsList.Count > 0)
                            {
                                foreach (Comment commentObject in RandomCommentsList)
                                {
                                    RandomCommentCollection.Add(new Comments()
                                    {
                                        CommentAuthor = commentObject.Author,
                                        CommentDate = commentObject.Created.ToString(),
                                        CommentText = commentObject.Body,
                                        CommentDownvotes = commentObject.DownVotes.ToString(),
                                        CommentUpvotes = commentObject.UpVotes.ToString(),
                                        CommentSelf = commentObject
                                    });
                                    /*var CommentReplies = commentObject.Replies;
                                    if (CommentReplies.Count > 0)
                                    {
                                        foreach (UIElement ReplyObject in CommentReplies)
                                        {
                                            ReplyCollection.Add(new Comments()
                                            {
                                                CommentAuthor = commentObject.Author,
                                                CommentDate = commentObject.Created.ToString(),
                                                CommentText = commentObject.Body,
                                            });
                                            MUXC.TreeViewNode replyNode = new MUXC.TreeViewNode();
                                            replyNode.Content = CommentCollection;
                                            replyNode.Children.Add(replyNode);
                                            MUXC.TreeViewNode rootNode = new MUXC.TreeViewNode();
                                            rootNode.Content = CommentCollection;
                                            UniversalCommentTreeViewControl.RootNodes.Add(rootNode);
                                        }
                                    }

                                    else
                                    {*/
                                    // MUXC.TreeViewNode rootNode = new MUXC.TreeViewNode();
                                    //   rootNode.Content = CommentCollection;
                                    //   UniversalCommentTreeViewControl.RootNodes.Add(rootNode);
                                    // }

                                }

                            }
                            var liveCommentsList = postComment.Comments.GetLive();
                            if (liveCommentsList.Count > 0)
                            {
                                foreach (Comment commentObject in liveCommentsList)
                                {
                                    LiveCommentCollection.Add(new Comments()
                                    {
                                        CommentAuthor = commentObject.Author,
                                        CommentDate = commentObject.Created.ToString(),
                                        CommentText = commentObject.Body,
                                        CommentDownvotes = commentObject.DownVotes.ToString(),
                                        CommentUpvotes = commentObject.UpVotes.ToString(),
                                        CommentSelf = commentObject
                                    });
                                    /*var CommentReplies = commentObject.Replies;
                                    if (CommentReplies.Count > 0)
                                    {
                                        foreach (UIElement ReplyObject in CommentReplies)
                                        {
                                            ReplyCollection.Add(new Comments()
                                            {
                                                CommentAuthor = commentObject.Author,
                                                CommentDate = commentObject.Created.ToString(),
                                                CommentText = commentObject.Body,
                                            });
                                            MUXC.TreeViewNode replyNode = new MUXC.TreeViewNode();
                                            replyNode.Content = CommentCollection;
                                            replyNode.Children.Add(replyNode);
                                            MUXC.TreeViewNode rootNode = new MUXC.TreeViewNode();
                                            rootNode.Content = CommentCollection;
                                            UniversalCommentTreeViewControl.RootNodes.Add(rootNode);
                                        }
                                    }

                                    else
                                    {*/
                                    // MUXC.TreeViewNode rootNode = new MUXC.TreeViewNode();
                                    //   rootNode.Content = CommentCollection;
                                    //   UniversalCommentTreeViewControl.RootNodes.Add(rootNode);
                                    // }

                                }
                                LiveCommentTreeViewControl.ItemsSource = LiveCommentCollection;
                            }
                            var QACommentsList = postComment.Comments.GetQA();
                            if (QACommentsList.Count > 0)
                            {
                                foreach (Comment commentObject in QACommentsList)
                                {
                                    QACommentCollection.Add(new Comments()
                                    {
                                        CommentAuthor = commentObject.Author,
                                        CommentDate = commentObject.Created.ToString(),
                                        CommentText = commentObject.Body,
                                        CommentDownvotes = commentObject.DownVotes.ToString(),
                                        CommentUpvotes = commentObject.UpVotes.ToString(),
                                        CommentSelf = commentObject
                                    });
                                    /*var CommentReplies = commentObject.Replies;
                                    if (CommentReplies.Count > 0)
                                    {
                                        foreach (UIElement ReplyObject in CommentReplies)
                                        {
                                            ReplyCollection.Add(new Comments()
                                            {
                                                CommentAuthor = commentObject.Author,
                                                CommentDate = commentObject.Created.ToString(),
                                                CommentText = commentObject.Body,
                                            });
                                            MUXC.TreeViewNode replyNode = new MUXC.TreeViewNode();
                                            replyNode.Content = CommentCollection;
                                            replyNode.Children.Add(replyNode);
                                            MUXC.TreeViewNode rootNode = new MUXC.TreeViewNode();
                                            rootNode.Content = CommentCollection;
                                            UniversalCommentTreeViewControl.RootNodes.Add(rootNode);
                                        }
                                    }

                                    else
                                    {*/
                                    // MUXC.TreeViewNode rootNode = new MUXC.TreeViewNode();
                                    //   rootNode.Content = CommentCollection;
                                    //   UniversalCommentTreeViewControl.RootNodes.Add(rootNode);
                                    // }

                                }
                                QACommentTreeViewControl.ItemsSource = QACommentCollection;
                            }

                            else
                            {
                                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                                {
                                    TopCommentTreeViewControl.ItemsSource = TopCommentCollection;
                                    QACommentTreeViewControl.ItemsSource = QACommentCollection;
                                    LiveCommentTreeViewControl.ItemsSource = LiveCommentCollection;
                                    RandomCommentTreeViewControl.ItemsSource = RandomCommentCollection;
                                    ControversialCommentTreeViewControl.ItemsSource = ControversialCommentCollection;
                                    NewCommentTreeViewControl.ItemsSource = NewCommentCollection;
                                    TopCommentTreeViewControl.ItemsSource = TopCommentCollection;
                                    OldCommentTreeViewControl.ItemsSource = OldCommentCollection;
                                    UniversalCommentTreeViewControl.ItemsSource = CommentCollection;
                                });
                            }
                        });
                        await PostDialog.ShowAsync();
                    }
                }
            });
        }

        private async void Upvotes_Click(object sender, RoutedEventArgs e)
        {
            if ((string)localSettings.Values["refresh_token"] == BackuprefreshToken)
            {
                UniversalPostTip.IsOpen = true;
                UniversalPostTip.Title = "CantUpvoteWithoutSignin".GetLocalized();
            }
            else
            {
                Post upvote = ReferencePost.PostSelf;
                if (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
                {
                    UniversalPostTip.IsOpen = true;
                    UniversalPostTip.Title = "Upvoted".GetLocalized();
                }
                else
                {
                    UniversalPostNotification.Show("Upvoted".GetLocalized(), 3000);
                }
                await upvote.UpvoteAsync();
            }
        }

        private async void Downvotes_Click(object sender, RoutedEventArgs e)
        {
            if ((string)localSettings.Values["refresh_token"] == BackuprefreshToken)
            {
                UniversalPostTip.IsOpen = true;
                UniversalPostTip.Title = "CantDownvoteWithoutSignin".GetLocalized();
            }
            else
            {
                Post downvote = ReferencePost.PostSelf;
                if (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
                {
                    UniversalPostTip.IsOpen = true;
                    UniversalPostTip.Title = "Downvoted".GetLocalized();
                }
                else
                {
                    UniversalPostNotification.Show("Downvoted".GetLocalized(), 3000);
                }
                await downvote.UpvoteAsync();
            }
        }

        private void OpenSearchDialog_Click(object sender, RoutedEventArgs e)
        {
            FindName("SearchTip");
            SearchTip.IsOpen = true;
        }


        private async void SearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            /*  await Task.Run(async () =>
              {*/
            PostCollection = new List<Posts>();
            var reddit = new RedditAPI(appId, refreshToken, secret);
            var subreddit = reddit.Subreddit("Appraisit");


            var posts = reddit.Subreddit(subreddit).Search(new SearchGetSearchInput(SearchBox.Text));  // Search r/MySub
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {

                if (posts.Count == 0)
                {
                    return;
                }
                else
                {

                    foreach (Post post in posts)
                    {

                        var p = post as SelfPost;
                        PostCollection.Add(new Posts()
                        {
                            TitleText = post.Title,
                            PostSelf = post,
                            PostAuthor = string.Format("PostBy".GetLocalized(), post.Author),
                            PostDate = string.Format("PostCreated", post.Created),
                            PostUpvotes = post.UpVotes.ToString(),
                            PostDownvotes = post.DownVotes.ToString(),
                            PostCommentCount = post.Comments.GetComments("new").Count.ToString()
                        });

                        SearchGridViewControl.ItemsSource = PostCollection;

                    }

                }
            });

            // });

        }
        //removed auto suggest due to performance
        private async void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            PostCollection = new List<Posts>();
            var reddit = new RedditAPI(appId, refreshToken, secret);
            var subreddit = reddit.Subreddit("Appraisit");
            // await Task.Run(async () =>
            //{

            var posts = reddit.Subreddit(subreddit).Search(new SearchGetSearchInput(SearchBox.Text));  // Search r/MySub
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {

                if (posts.Count == 0)
                {
                    return;
                }
                else
                {

                    foreach (Post post in posts)
                    {

                        var p = post as SelfPost;
                        PostCollection.Add(new Posts()
                        {
                            TitleText = post.Title,
                            PostSelf = post,
                            PostAuthor = string.Format("PostBy".GetLocalized(), post.Author),
                            PostDate = string.Format("PostCreated", post.Created),
                            PostUpvotes = post.UpVotes.ToString(),
                            PostDownvotes = post.DownVotes.ToString(),
                            PostCommentCount = post.Comments.GetComments("new").Count.ToString()
                        });

                        SearchGridViewControl.ItemsSource = PostCollection;

                    }

                }
            });
            // });
        }
        /* await Task.Run(async () =>
         {
             await Task.Delay(1000);
             PostCollection = new List<Posts>();
                     var reddit = new RedditAPI(appId, refreshToken, secret);
                     var subreddit = reddit.Subreddit("Appraisit");
                 await Task.Run(async () =>
                 {
                     await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, ()=>
                     {
                         List<Post> posts = reddit.Subreddit(subreddit).Search(new SearchGetSearchInput(SearchBox.Text));  // Search r/MySub

                     if (posts.Count == 0)
                     {
                         return;
                     }
                     else
                     {

                         foreach (Post post in posts)
                             {

                                     var p = post as SelfPost;
                                     PostCollection.Add(new Posts()
                                     {
                                         TitleText = post.Title,
                                         PostSelf = post,
                                         PostAuthor = string.Format("PostBy".GetLocalized(), post.Author),
                                         PostDate = string.Format("PostCreated", post.Created),
                                         PostUpvotes = post.UpVotes.ToString(),
                                         PostDownvotes = post.DownVotes.ToString(),
                                         PostCommentCount = post.Comments.GetComments("new").Count.ToString()
                                     });

                                         SearchGridViewControl.ItemsSource = PostCollection;

                                 }

                 }
                 });
                 });
         });
     }*/

       /* private void LoginDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var scopes = Constants.Constants.scopeList.Aggregate("", (acc, x) => acc + " " + x);
            var urlParams = "client_id=" + appId + "&response_type=code&state=uyagsjgfhjs&duration=permanent&redirect_uri=" + HttpUtility.UrlEncode("http://127.0.0.1:3000/reddit_callback") + "&scope=" + HttpUtility.UrlEncode(scopes);
            Uri targetUri = new Uri(Constants.Constants.redditApiBaseUrl + "authorize?" + urlParams);
            ContentGrid.Visibility = Visibility.Collapsed;
           // loginView.Visibility = Visibility.Visible;

           // loginView.Navigate(targetUri);

        }
   */
        private async void RefreshToken()
        {
            LoginHelper loginHelper = new LoginHelper(appId, secret);
            var result = await loginHelper.Login_Refresh((string)localSettings.Values["refresh_token"]);
            accessToken = result.AccessToken;
            refreshToken = result.RefreshToken;
            FindName("ContentGrid");
            ProgressRing.IsActive = false;
        }

        private void SettingOpenButton_Click(object sender, RoutedEventArgs e)
        {
            FindName("SettingsPanel");
            // Initialize();
        }

        public void MyFancyPanel_BackdropTapped(object sender, EventArgs e)
        {

            UnloadObject(SettingsPanel);

        }
        public void MyFancyPanel_BackdropClicked()
        {

            UnloadObject(SettingsPanel);

        }

        public async void Refresh(object sender, RoutedEventArgs e)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                ProgressRing.IsActive = true;
                switch (SortBox.SelectedItem.ToString())
                {
                    case "New":
                        try
                        {
                            var Newcollection = new IncrementalLoadingCollection<NewPostsClass, Posts>();
                            UniversalGridViewControl.ItemsSource = Newcollection;
                            UnloadObject(TopSort);
                        }
                        catch
                        {
                            RefreshAccess();
                        }
                        ProgressRing.IsActive = false;
                        break;
                    case "Hot":
                        var Hotcollection = new IncrementalLoadingCollection<HotPostsClass, Posts>();
                        UniversalGridViewControl.ItemsSource = Hotcollection;
                        UnloadObject(TopSort);
                        ProgressRing.IsActive = false;
                        break;
                    case "Top":
                        var Topcollection = new IncrementalLoadingCollection<TopPostsClass, Posts>();
                        UniversalGridViewControl.ItemsSource = Topcollection;
                        FindName("TopSort");
                        TopSort.SelectedItem = "Day";
                        ProgressRing.IsActive = false;
                        break;
                    case "Rising":
                        var Risingcollection = new IncrementalLoadingCollection<HotPostsClass, Posts>();
                        UniversalGridViewControl.ItemsSource = Risingcollection;
                        UnloadObject(TopSort);
                        ProgressRing.IsActive = false;
                        break;
                    case "Controversial":
                        var Controversialcollection = new IncrementalLoadingCollection<HotPostsClass, Posts>();
                        UniversalGridViewControl.ItemsSource = Controversialcollection;
                        UnloadObject(TopSort);
                        ProgressRing.IsActive = false;
                        break;
                }
                ProgressRing.IsActive = false;
            });
        }
        private async void RefreshContainer_RefreshRequested(MUXC.RefreshContainer sender, MUXC.RefreshRequestedEventArgs args)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                ProgressRing.IsActive = true;
                switch (SortBox.SelectedItem.ToString())
                {
                    case "0":
                        try
                        {
                            var Newcollection = new IncrementalLoadingCollection<NewPostsClass, Posts>();
                            UniversalGridViewControl.ItemsSource = Newcollection;
                        }
                        catch
                        {
                            RefreshAccess();
                        }
                        ProgressRing.IsActive = false;
                        break;
                    case "1":
                        var Hotcollection = new IncrementalLoadingCollection<HotPostsClass, Posts>();
                        UniversalGridViewControl.ItemsSource = Hotcollection;
                        ProgressRing.IsActive = false;
                        break;
                    case "2":
                        var Topcollection = new IncrementalLoadingCollection<TopPostsClass, Posts>();
                        UniversalGridViewControl.ItemsSource = Topcollection;
                        ProgressRing.IsActive = false;
                        break;
                    case "3":
                        var Risingcollection = new IncrementalLoadingCollection<HotPostsClass, Posts>();
                        UniversalGridViewControl.ItemsSource = Risingcollection;
                        ProgressRing.IsActive = false;
                        break;

                }
                ProgressRing.IsActive = false;
            });
        }

        private async void WebBarButton_Click(object sender, RoutedEventArgs e)
        {
            var uriBing = new Uri(@"https://www.reddit.com/r/appraisit/");
            // Launch the URI
            await Windows.System.Launcher.LaunchUriAsync(uriBing);
        }


        private async void OpenCreatePostDialog(object sender, RoutedEventArgs e)
        {
            FindName("SearchTip");
            SearchTip.IsOpen = false;
            FindName("CreatePostDialog");
            var PostDialog = new AddPostDialog();
            await PostDialog.ShowAsync();
        }
      

        /* private async void OpenInBrowserButton_Click(object sender, RoutedEventArgs e)
         {
             var uriBing = new Uri(webView.Source.ToString());

             // Launch the URI
             await Windows.System.Launcher.LaunchUriAsync(uriBing);
         }*/

        private async void TopComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                var Topcollection = new IncrementalLoadingCollection<TopPostsClass, Posts>();
                TopPostsClass TopGenerator = new TopPostsClass();
                int newOrder = TopSort.SelectedIndex;
                switch (newOrder)
                {
                    case 0:
                        TopGenerator.Order = Helpers.TopOrder.All;
                        UniversalGridViewControl.ItemsSource = Topcollection;
                        break;
                    case 1:
                        TopGenerator.Order = Helpers.TopOrder.Year;
                        UniversalGridViewControl.ItemsSource = Topcollection;
                        break;
                    case 2:
                        TopGenerator.Order = Helpers.TopOrder.Month;
                        UniversalGridViewControl.ItemsSource = Topcollection;
                        break;
                    case 3:
                        TopGenerator.Order = Helpers.TopOrder.Week;
                        UniversalGridViewControl.ItemsSource = Topcollection;
                        break;
                    case 4:
                        TopGenerator.Order = Helpers.TopOrder.Day;
                        UniversalGridViewControl.ItemsSource = Topcollection;
                        break;
                }
            });
        }

        private void PostDialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            ScalarKeyFrameAnimation hideAnimation = _compositor.CreateScalarKeyFrameAnimation();

            hideAnimation.InsertKeyFrame(0f, 1f);

            hideAnimation.InsertKeyFrame(1.0f, 0f);

            hideAnimation.Duration = TimeSpan.FromMilliseconds(1000);

            _destinationSprite.StartAnimation("Opacity", hideAnimation);
            ElementCompositionPreview.SetElementChildVisual(ContentGrid, null);
            if (_destinationSprite != null)

            {

                _destinationSprite.Dispose();

                _destinationSprite = null;

            }
            RepliesCommentTreeViewControl.ItemsSource = null;
        }

        private async void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                var reddit = new RedditAPI(appId, refreshToken, secret);
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    UserName.Text = string.Format("MyUsername".GetLocalized(), reddit.Account.Me.Name);
                    Cakeday.Text = string.Format("MyCreated".GetLocalized(), reddit.Account.Me.Created.ToString());
                    LinkKarma.Text = string.Format("MyKarma".GetLocalized(), reddit.Account.Me.LinkKarma);
                    CommentKarma.Text = string.Format("MyCommentKarma".GetLocalized(), reddit.Account.Me.CommentKarma);
                });
            });
        }

        private async void SearchMSButton_Click(object sender, RoutedEventArgs e)
        {
            string output = Regex.Replace(SearchBox.Text.Split()[0], @"[^0-9a-zA-Z\ ]+", "");
            await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store://search/?query=" + output));
        }



        private void StackPanel_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var s = (FrameworkElement)sender;
            var D = s.DataContext;
            var dse = D as Posts;
            ReferencePost = new OpenPosts()
            {
                PostSelf = dse.PostSelf
            };
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

        private async void StackPanel_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var s = (FrameworkElement)sender;
            var D = s.DataContext;
            var dse = D as Posts;
            var output = dse.PostSelf.Title;
            await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store://search/?query=" + output));
        }

        private void StackPanel_Holding(object sender, HoldingRoutedEventArgs e)
        {
            var s = (FrameworkElement)sender;
            var D = s.DataContext;
            var dse = D as Posts;
            ReferencePost = new OpenPosts()
            {
                PostSelf = dse.PostSelf
            };
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

        private async void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            var s = (FrameworkElement)sender;
            var D = s.DataContext;
            var dse = D as Posts;
            string output = Regex.Replace(dse.PostSelf.Title.Split()[0], @"[^0-9a-zA-Z\ ]+", "");
            await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store://search/?query=" + output));
        }

        private void MenuFlyoutItem_Click_1(object sender, RoutedEventArgs e)
        {
            var s = (FrameworkElement)sender;
            var D = s.DataContext;
            var dse = D as Posts;
            var output = dse.PostSelf.Title;
            FindName("SearchTip");
            SearchTip.IsOpen = true;
            SearchBox.Text = output;
        }

        private async void MenuFlyoutItem_Click_2(object sender, RoutedEventArgs e)
        {
            var s = (FrameworkElement)sender;
            var D = s.DataContext;
            var dse = D as Posts;
            var output = dse.PostSelf.Title;
            await Windows.System.Launcher.LaunchUriAsync(new Uri("https://www.reddit.com/r/appraisit/search/?q=" + output));
        }
        private async void PostInWebClick(object sender, RoutedEventArgs e)
        {
            string oweb = ReferencePost.PostSelf.Id;
            await Windows.System.Launcher.LaunchUriAsync(new Uri("https://www.reddit.com/r/appraisit/comments/" + oweb));
        }
        private async void PostInWebClickLocal(object sender, RoutedEventArgs e)
        {
            var s = (FrameworkElement)sender;
            var D = s.DataContext;
            var dse = D as Posts;
            string oweb = dse.PostSelf.Id;
            await Windows.System.Launcher.LaunchUriAsync(new Uri("https://www.reddit.com/r/appraisit/comments/" + oweb));
        }
        private async void bARenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            string output = Regex.Replace(ReferencePost.PostSelf.Title.Split()[0], @"[^0-9a-zA-Z\ ]+", "");
            await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store://search/?query=" + output));
        }

        private void bARFlyoutItem_Click_1(object sender, RoutedEventArgs e)
        {
            var output = ReferencePost.PostSelf.Title;
            FindName("SearchTip");
            SearchTip.IsOpen = true;
            SearchBox.Text = output;
        }

        private async void bARFlyoutItem_Click_2(object sender, RoutedEventArgs e)
        {
            var output = ReferencePost.PostSelf.Title;
            await Windows.System.Launcher.LaunchUriAsync(new Uri("https://www.reddit.com/r/appraisit/search/?q=" + output));
        }

        private async void LinkNavigator_Click(object sender, RoutedEventArgs e)
        {
            var Linkpost = ReferencePost.PostSelf as LinkPost;
            await Windows.System.Launcher.LaunchUriAsync(new Uri(Linkpost.URL));
        }
        private async void RedditSearch_Click(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("https://www.reddit.com/r/appraisit/search/?q=" + SearchBox.Text));
        }

        private void MenuFlyoutItem_Click_3(object sender, RoutedEventArgs e)
        {
            var CommentsList = ReferencePost.PostSelf.Comments.GetComments("new");
            if (CommentsList.Count > 0)
            {
                foreach (Comment commentObject in CommentsList)
                {
                    CommentCollection.Add(new Comments()
                    {
                        CommentAuthor = commentObject.Author,
                        CommentDate = commentObject.Created.ToString(),
                        CommentText = commentObject.Body,
                    });

                }
                UniversalCommentTreeViewControl.ItemsSource = CommentCollection;
            }
        }

        private async void SwipeItem_Invoked_1(MUXC.SwipeItem sender, MUXC.SwipeItemInvokedEventArgs args)
        {
            FindName("SearchTip");
            SearchTip.IsOpen = false;
            FindName("CreatePostDialog");
            var PostDialog = new AddPostDialog();
            await PostDialog.ShowAsync();
        }

        private void SwipeItem_Invoked_2(MUXC.SwipeItem sender, MUXC.SwipeItemInvokedEventArgs args)
        {

            FindName("SearchTip");
            SearchTip.IsOpen = true;

        }

        private async void SwipeItem_Invoked_3(MUXC.SwipeItem sender, MUXC.SwipeItemInvokedEventArgs args)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                ProgressRing.IsActive = true;
                switch (SortBox.SelectedItem.ToString())
                {
                    case "0":
                        try
                        {
                            var Newcollection = new IncrementalLoadingCollection<NewPostsClass, Posts>();
                            UniversalGridViewControl.ItemsSource = Newcollection;
                        }
                        catch
                        {
                            RefreshAccess();
                        }
                        ProgressRing.IsActive = false;
                        break;
                    case "1":
                        var Hotcollection = new IncrementalLoadingCollection<HotPostsClass, Posts>();
                        UniversalGridViewControl.ItemsSource = Hotcollection;
                        ProgressRing.IsActive = false;
                        break;
                    case "2":
                        var Topcollection = new IncrementalLoadingCollection<TopPostsClass, Posts>();
                        UniversalGridViewControl.ItemsSource = Topcollection;
                        ProgressRing.IsActive = false;
                        break;
                    case "3":
                        var Risingcollection = new IncrementalLoadingCollection<HotPostsClass, Posts>();
                        UniversalGridViewControl.ItemsSource = Risingcollection;
                        ProgressRing.IsActive = false;
                        break;

                }
                ProgressRing.IsActive = false;
            });
        }

        private async void UniversalZoomButton_Click(object sender, RoutedEventArgs e)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (view == 1)
                {
                    UniversalGridViewControl.OneRowModeEnabled = true;
                    view = view + 1;
                }
                else
                {
                    UniversalGridViewControl.OneRowModeEnabled = false;
                    view = view - 1;
                }
            });
        }

        private async void ReplyText_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            var s = (FrameworkElement)sender;
            var D = s.DataContext;
            var dse = D as Comments;
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                try
                {
                    await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                    {
                        if ((string)localSettings.Values["refresh_token"] == BackuprefreshToken)
                        {
                            UniversalPostTip.IsOpen = true;
                            UniversalPostTip.Title = "Reply sent. Sign in to use custom username";
                        }
                        else
                        {
                            UniversalPostTip.IsOpen = true;
                            UniversalPostTip.Title = "ReplySent".GetLocalized();
                        }
                        var reddit = new RedditAPI(appId, refreshToken, secret);
                        var subreddit = reddit.Subreddit("Appraisit");
                        await dse.CommentSelf.ReplyAsync(sender.Text);
                        sender.Text = "";

                    });
                }
                catch
                {
                    return;
                }
            });

        }

        private async void CommentTextMessage_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                try
                {
                    if ((string)localSettings.Values["refresh_token"] == BackuprefreshToken)
                    {
                        UniversalPostTip.IsOpen = true;
                        UniversalPostTip.Title = "Comment sent. Sign in to use custom username";
                    }
                    else
                    {
                        UniversalPostTip.IsOpen = true;
                        UniversalPostTip.Title = "CommentSent".GetLocalized();
                    }
                    var reddit = new RedditAPI(appId, refreshToken, secret);
                    var subreddit = reddit.Subreddit("Appraisit");
                    Post NewComment = ReferencePost.PostSelf;
                    await NewComment.ReplyAsync(body: sender.Text.ToString());
                    sender.Text = "";
                    await Task.Delay(6000);
                    var CommentsList = NewComment.Comments.GetComments("new");
                    if (CommentsList.Count > 0)
                    {
                        foreach (Comment commentObject in CommentsList)
                        {
                            CommentCollection.Add(new Comments()
                            {
                                CommentAuthor = commentObject.Author,
                                CommentDate = commentObject.Created.ToString(),
                                CommentText = commentObject.Body,
                            });

                        }
                        UniversalCommentTreeViewControl.ItemsSource = CommentCollection;
                    }
                }
                catch
                {
                    return;
                }
            });

        }
        private async void UpvoteComment_Click(object sender, RoutedEventArgs e)
        {
            if ((string)localSettings.Values["refresh_token"] == BackuprefreshToken)
            {
                UniversalPostTip.IsOpen = true;
                UniversalPostTip.Title = "CantUpvoteWithoutSignin".GetLocalized();
            }
            else
            {
                var s = (FrameworkElement)sender;
                var D = s.DataContext;
                var dse = D as Comments;
                if (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
                {
                    UniversalPostTip.IsOpen = true;
                    UniversalPostTip.Title = "Upvoted".GetLocalized();
                    AppBarButton see = sender as AppBarButton;
                    var ll = see.Label;
                    ll = ll + 1;
                    see.Label = ll;
                }
                else
                {
                    UniversalPostNotification.Show("Upvoted".GetLocalized(), 3000);
                }
                await dse.CommentSelf.UpvoteAsync();
            }
        }

        private async void DownvoteComment_Click(object sender, RoutedEventArgs e)
        {
            if ((string)localSettings.Values["refresh_token"] == BackuprefreshToken)
            {
                UniversalPostTip.IsOpen = true;
                UniversalPostTip.Title = "CantDownvoteWithoutSignin".GetLocalized();
            }
            else
            {
                var s = (FrameworkElement)sender;
                var D = s.DataContext;
                var dse = D as Comments;
                if (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
                {
                    UniversalPostTip.IsOpen = true;
                    UniversalPostTip.Title = "Downvoted".GetLocalized();
                    AppBarButton see = sender as AppBarButton;
                    see.Label = "-1";
                }
                else
                {
                    UniversalPostNotification.Show("Downvoted".GetLocalized(), 3000);
                }
                await dse.CommentSelf.DownvoteAsync();
            }
        }

        private async void SignOutButton_Click(object sender, RoutedEventArgs e)
        {
            if ((string)localSettings.Values["refresh_token"] == BackuprefreshToken)
            {
                UniversalPageTip.IsOpen = true;
                UniversalPageTip.Title = "User not signed in error message: Cant sign out because no";
            }
            else
            {
                UniversalPageTip.IsOpen = true;
                UniversalPageTip.Title = "Signed out";

                refreshToken = BackuprefreshToken;
                localSettings.Values["refresh_token"] = BackuprefreshToken;
                /*LoginHelper loginHelper = new LoginHelper(appId, secret);
                var result = await loginHelper.Login_Refresh((string)localSettings.Values["refresh_token"]);
                accessToken = result.AccessToken;
                refreshToken = result.RefreshToken;*/
                FindName("ContentGrid");
                ProgressRing.IsActive = false;
                SignInBar.Visibility = Visibility.Visible;
                await WebView.ClearTemporaryWebDataAsync();
            }
        }

        private void Grid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            SolidColorBrush eee = new SolidColorBrush();
            Grid ee = sender as Grid;
            ee.BorderBrush = eee;

        }

        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            FindName("LoginFrame");
            FindName("CloseLogin");
            LoginFrame.Navigate(typeof(LoginPage));

        }
        private async void PostContentText_LinkClicked(object sender, Microsoft.Toolkit.Uwp.UI.Controls.LinkClickedEventArgs e)
        {
            if (Uri.TryCreate(e.Link, UriKind.Absolute, out Uri link))
            {
                await Launcher.LaunchUriAsync(link);
            }
        }

        private async void LikeButton_Click(object sender, RoutedEventArgs e)
        {
            if ((string)localSettings.Values["refresh_token"] == BackuprefreshToken)
            {
                UniversalPageTip.IsOpen = true;
                UniversalPageTip.Title = "CantUpvoteWithoutSignin".GetLocalized();
            }
            else
            {
                var FrameW = (FrameworkElement)sender;
                var DataC = FrameW.DataContext;
                var Like = DataC as Posts;
                await Like.PostSelf.UpvoteAsync();
                AppBarButton LikeButton = sender as AppBarButton;
                LikeButton.Label = Like.PostUpvotes;
                if (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
                {
                    UniversalPageTip.IsOpen = true;
                    UniversalPageTip.Title = "Upvoted".GetLocalized();
                }
                else
                {
                    UniversalPageNotification.Show("Upvoted".GetLocalized(), 3000);
                }
            }
        }
        private async void DisLikeButton_Click(object sender, RoutedEventArgs e)
        {
            if ((string)localSettings.Values["refresh_token"] == BackuprefreshToken)
            {
                UniversalPageTip.IsOpen = true;
                UniversalPageTip.Title = "CantDownVoteWithoutSignin".GetLocalized();
            }
            else
            {
                var FrameW = (FrameworkElement)sender;
                var DataC = FrameW.DataContext;
                var Like = DataC as Posts;
                await Like.PostSelf.DownvoteAsync();
                AppBarButton DisLikeButton = sender as AppBarButton;
                DisLikeButton.Label = "-1";
                if (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
                {
                    UniversalPageTip.IsOpen = true;
                    UniversalPageTip.Title = "Downvoted".GetLocalized();
                }
                else
                {
                    UniversalPageNotification.Show("Downvoted".GetLocalized(), 3000);
                }
            }
        }

        private void Sidebarbutton_Click(object sender, RoutedEventArgs e)
        {
            Sidebar.Navigate(typeof(Sidebar));
        }
        private void MarkdownText_OnImageResolving(object sender, ImageResolvingEventArgs e)
        {
            // This is basically the default implementation
            e.Image = new BitmapImage(new Uri(e.Url));
            e.Handled = true;
        }

        private void CloseLogin_Click(object sender, RoutedEventArgs e)
        {
            FindName("LoginFrame");
            UnloadObject(LoginFrame);
            UnloadObject(CloseLogin);
        }
    }
}
