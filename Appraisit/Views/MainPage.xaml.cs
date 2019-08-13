using System;
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
using Windows.ApplicationModel;
using Appraisit.Services;
using Windows.UI.Popups;
using Windows.UI.StartScreen;
using Windows.Foundation.Metadata;
using Windows.UI.Shell;
using Windows.System;
using MUXC = Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Composition;
using Windows.UI.Xaml.Hosting;
using System.Numerics;
using Microsoft.Graphics.Canvas.Effects;
using Windows.Graphics.Effects;
using Windows.UI.Xaml.Input;
using Microsoft.Toolkit.Uwp.UI.Animations.Expressions;
using System.Text.RegularExpressions;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.Networking.Connectivity;
using Windows.System.Profile;

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
        public string BackuprefreshToken = "209908787246-4p2wKVe0RaB_coetdNPtatNe45c";
        public string accessToken;
        // string backupAccesToken = "209908787246-EHnGFWXgWZDrmpEv3iYmkXLB-ew";
        public string secret = "SESshAirmwAuAvBFHbq_JUkAMmk";
        public string PostFlair = "Update";
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
        private ElementTheme _elementTheme = ThemeSelectorService.Theme;
        public MainPage()
        {
            InitializeComponent();

            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            Window.Current.SetTitleBar(TitleGrid);                     
                    if ((string)localSettings.Values["refresh_token"] == null)
                    {
                        ProgressRing.IsActive = true;
                        var scopes = Constants.Constants.scopeList.Aggregate("", (acc, x) => acc + " " + x);
                        var urlParams = "client_id=" + appId + "&response_type=code&state=uyagsjgfhjs&duration=permanent&redirect_uri=" + HttpUtility.UrlEncode("http://127.0.0.1:3000/reddit_callback") + "&scope=" + HttpUtility.UrlEncode(scopes);
                        Uri targetUri = new Uri(Constants.Constants.redditApiBaseUrl + "authorize?" + urlParams);
                        ContentGrid.Visibility = Visibility.Collapsed;
                        FindName("loginView");
                        loginView.Navigate(targetUri);
                        ProgressRing.IsActive = false;
                    }
                    else
                    {


                        refreshToken = localSettings.Values["refresh_token"].ToString();

                    }

        }
        public ElementTheme ElementTheme
        {
            get { return _elementTheme; }

            set { Set(ref _elementTheme, value); }
        }

        private string _versionDescription;

        public string VersionDescription
        {
            get { return _versionDescription; }

            set { Set(ref _versionDescription, value); }
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
        private async void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
            var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
            try
            {
             if (connectionProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess)
             {
                ProgressRing.IsActive = true;
                        transition = new ColorBloomTransitionHelper(hostForVisual);
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
                                                   finalBounds);
                        switch (PivotNavigator.SelectedIndex.ToString())
                {
                    case "0":
                        try
                        {
                            var Newcollection = new IncrementalLoadingCollection<NewPostsClass, Posts>();
                            NewGridViewControl.ItemsSource = Newcollection;
                        }
                        catch
                        {
                            RefreshAccess();
                        }
                        ProgressRing.IsActive = false;
                        break;
                    case "1":
                        FindName("HotGridViewControl");
                        var Hotcollection = new IncrementalLoadingCollection<TopPostsClass, Posts>();
                        HotGridViewControl.ItemsSource = Hotcollection;
                        ProgressRing.IsActive = false;
                        break;
                    case "2":
                        FindName("TopGridViewControl");
                        var Topcollection = new IncrementalLoadingCollection<HotPostsClass, Posts>();
                        TopGridViewControl.ItemsSource = Topcollection;
                        ProgressRing.IsActive = false;
                        break;
                    case "3":
                        FindName("ExtraNavigationView");
                        var Risingcollection = new IncrementalLoadingCollection<HotPostsClass, Posts>();
                        RisingGridViewControl.ItemsSource = Risingcollection;
                        var Controversialcollection = new IncrementalLoadingCollection<HotPostsClass, Posts>();
                        ControversialGridViewControl.ItemsSource = Controversialcollection;
                        ProgressRing.IsActive = false;
                        ProgressRing.IsActive = false;
                        break;

                }
                ProgressRing.IsActive = false;
                            // the area to fill over the animation duration
                 }
              }
                catch
                {
                    UniversalPageNotification.Show("No internet connection");
                }
               if (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
                 {
                UnloadObject(PivotBar);
                FindName("SB");
                FindName("MobileBar");
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

        private void OnGoBack(object sender, RoutedEventArgs e)
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
        }

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
            Post upvote = ReferencePost.PostSelf;
            UniversalPostNotification.Show("Upvoted");
            await upvote.UpvoteAsync();
        }

        private async void Downvotes_Click(object sender, RoutedEventArgs e)
        {
            Post downvote = ReferencePost.PostSelf;
            UniversalPostNotification.Show("Downvoted");
            await downvote.UpvoteAsync();
        }

        private void OpenSearchDialog_Click(object sender, RoutedEventArgs e)
        {
            FindName("SearchTip");
            SearchTip.IsOpen = true;
        }


        private async void SearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                PostCollection = new List<Posts>();
                var reddit = new RedditAPI(appId, refreshToken, secret);
                var subreddit = reddit.Subreddit("Appraisit");
                List<Post> posts = reddit.Subreddit(subreddit).Search(new SearchGetSearchInput(SearchBox.Text));
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
                            PostAuthor = "by: " + post.Author,
                            PostDate = "Created: " + post.Created,
                            PostUpvotes = post.UpVotes.ToString(),
                            PostDownvotes = post.DownVotes.ToString(),
                            PostCommentCount = post.Comments.GetComments("new").Count.ToString()
                        });

                        SearchGridViewControl.ItemsSource = PostCollection;

                    }
                }
            });
        }

        private async void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async() =>
            {
                Random number = new Random();
                if (number.Next(1, 2) == 1)
                {
                    await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                    {
                        PostCollection = new List<Posts>();
                        var reddit = new RedditAPI(appId, refreshToken, secret);
                        var subreddit = reddit.Subreddit("Appraisit");
                        List<Post> posts = reddit.Subreddit(subreddit).Search(new SearchGetSearchInput(SearchBox.Text));  // Search r/MySub
                        if (posts.Count == 0)
                        {
                            return;
                        }
                        else
                        {
                            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                            {
                                foreach (Post post in posts)
                                {
                                    await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                                    {
                                        var p = post as SelfPost;
                                        PostCollection.Add(new Posts()
                                        {
                                            TitleText = post.Title,
                                            PostSelf = post,
                                            PostAuthor = "by: " + post.Author,
                                            PostDate = "Created: " + post.Created,
                                            PostUpvotes = post.UpVotes.ToString(),
                                            PostDownvotes = post.DownVotes.ToString(),
                                            PostCommentCount = post.Comments.GetComments("new").Count.ToString()
                                        });

                                        SearchGridViewControl.ItemsSource = PostCollection;
                                    });
                                }
                            });
                        }
                    });

                }
                else
                {
                    return;
                }
            });
        }

        private void LoginDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var scopes = Constants.Constants.scopeList.Aggregate("", (acc, x) => acc + " " + x);
            var urlParams = "client_id=" + appId + "&response_type=code&state=uyagsjgfhjs&duration=permanent&redirect_uri=" + HttpUtility.UrlEncode("http://127.0.0.1:3000/reddit_callback") + "&scope=" + HttpUtility.UrlEncode(scopes);
            Uri targetUri = new Uri(Constants.Constants.redditApiBaseUrl + "authorize?" + urlParams);
            ContentGrid.Visibility = Visibility.Collapsed;
            loginView.Visibility = Visibility.Visible;
            loginView.Navigate(targetUri);

        }
        private async void LoginView_NavigationStarting(WebView _, WebViewNavigationStartingEventArgs args)
        {
            LoginHelper loginHelper = new LoginHelper(appId, secret);
            if (args.Uri.AbsoluteUri.Contains("http://127.0.0.1:3000/reddit_callback"))
            {
                var result = await loginHelper.Login_Stage2(args.Uri);
                accessToken = result.AccessToken;
                refreshToken = result.RefreshToken;

                localSettings.Values["refresh_token"] = result.RefreshToken;
                ContentGrid.Visibility = Windows.UI.Xaml.Visibility.Visible;
                UnloadObject(loginView);
            }
        }

        private async void RefreshToken()
        {
            LoginHelper loginHelper = new LoginHelper(appId, secret);
            var result = await loginHelper.Login_Refresh((string)localSettings.Values["refresh_token"]);
            accessToken = result.AccessToken;
            refreshToken = result.RefreshToken;
            ContentGrid.Visibility = Windows.UI.Xaml.Visibility.Visible;
            ProgressRing.IsActive = false;
        }

        private void SettingOpenButton_Click(object sender, RoutedEventArgs e)
        {
            FindName("SettingsPanel");
            Initialize();
        }

        public void MyFancyPanel_BackdropTapped(object sender, EventArgs e)
        {

            UnloadObject(SettingsPanel);

        }
        public void MyFancyPanel_BackdropClicked(object sender, RoutedEventArgs e)
        {

            UnloadObject(SettingsPanel);

        }
        private void Initialize()
        {
            VersionDescription = GetVersionDescription();

            if (Microsoft.Services.Store.Engagement.StoreServicesFeedbackLauncher.IsSupported())
            {
                this.Feedbackbutton.IsEnabled = true;
            }
        }

        private string GetVersionDescription()
        {
            var appName = "AppDisplayName".GetLocalized();
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return $"{appName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }

        private async void ThemeChanged_CheckedAsync(object sender, RoutedEventArgs e)
        {
            var param = (sender as RadioButton)?.CommandParameter;

            if (param != null)
            {
                await ThemeSelectorService.SetThemeAsync((ElementTheme)param);

            }
        }
        private async void Whatsnew_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new WhatsNewDialog();
            await dialog.ShowAsync();
        }
        private async void Welcome_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FirstRunDialog();
            await dialog.ShowAsync();
        }
        private async void FeedbackLink_Click(object sender, RoutedEventArgs e)
        {
            var launcher = Microsoft.Services.Store.Engagement.StoreServicesFeedbackLauncher.GetDefault();
            await launcher.LaunchAsync();
        }
        private async void Rate_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(
    new Uri($"ms-windows-store://review/?PFN={Package.Current.Id.FamilyName}"));
        }
        private async void PinAppToTaskbar_Click(object sender, RoutedEventArgs e)
        {
            bool isPinningAllowed = TaskbarManager.GetDefault().IsPinningAllowed;
            if (isPinningAllowed)
            {
                if (ApiInformation.IsTypePresent("Windows.UI.Shell.TaskbarManager"))
                {
                    bool isPinned = await TaskbarManager.GetDefault().IsCurrentAppPinnedAsync();

                    if (isPinned)
                    {
                        await new MessageDialog("If not you can manually pin the app to the taskbar", "You already have the app pinned in your taskbar").ShowAsync();
                    }
                    else
                    {
                        bool IsPinned = await TaskbarManager.GetDefault().RequestPinCurrentAppAsync();
                    }
                }

                else
                {
                    await new MessageDialog("Update your device to the Fall creators update or higher to pin this app", "Update your device").ShowAsync();
                }
            }



            else
            {
                var t = Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily;
                switch (t)
                {
                    case "Windows.Desktop":
                        await new MessageDialog("It seems you are using a computer. Group policy disabled pinning of app in taskbar", "Taskbar pin failed").ShowAsync();
                        break;
                    case "Windows.Mobile":
                        await new MessageDialog("It seems you are using a Windows 10 on ARM device or mobile device. Group policy disabled pinning of the app", "Taskbar pin failed").ShowAsync();
                        break;
                    case "Windows.IoT":
                        await new MessageDialog("It seems you are using a IoT device which doesn't support taskbar pin API", "Taskbar pin failed").ShowAsync();
                        break;
                    case "Windows.Team":
                        break;
                    case "Windows.Holographic":
                        await new MessageDialog("It seems you are using hololens. Hololens doesn't have a taskbar", "Taskbar pin failed").ShowAsync();
                        break;
                    case "Windows.Xbox":
                        await new MessageDialog("It seems you are using a xbox. Xbox doesn't have a taskbar", "Taskbar pin failed").ShowAsync();
                        break;
                    default:
                        await new MessageDialog("It seems you are using a " + t + " device. This device does not support taskbar API or Group policy disabled pinning of the app", "Taskbar pin failed").ShowAsync();
                        break;
                }
            }
        }
        private async void LivePin(object sender, RoutedEventArgs e)
        {
            // Get your own app list entry
            // (which is always the first app list entry assuming you are not a multi-app package)
            AppListEntry entry = (await Package.Current.GetAppListEntriesAsync())[0];

            // Check if Start supports your app
            bool isSupported = StartScreenManager.GetDefault().SupportsAppListEntry(entry);
            if (isSupported)
            {
                if (ApiInformation.IsTypePresent("Windows.UI.StartScreen.StartScreenManager"))
                {
                    // Primary tile API's supported!
                    bool isPinned = await StartScreenManager.GetDefault().ContainsAppListEntryAsync(entry);
                    if (isPinned)
                    {
                        await new MessageDialog("If not you can manually put the live tile on to the StartScreen", "You already have the live tile in your StartScreen").ShowAsync();
                    }
                    else
                    {
                        bool IsPinned = await StartScreenManager.GetDefault().RequestAddAppListEntryAsync(entry);
                    }
                }
                else
                {
                    await new MessageDialog("You need to update your device to enable automatic pinning", "Update your device").ShowAsync();
                }
            }
            else
            {
                var t = Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily;
                switch (t)
                {
                    case "Windows.IoT":
                        await new MessageDialog("It seems you are using a IoT device which doesn't support Primary tile API", "live tile failed").ShowAsync();
                        break;
                    case "Windows.Team":
                        break;
                    case "Windows.Holographic":
                        await new MessageDialog("It seems you are using hololens. Hololens doesn't support live tile", "live tile failed").ShowAsync();
                        break;
                    case "Windows.Xbox":
                        await new MessageDialog("It seems you are using a xbox. Xbox doesn't support live tile", "live tile failed").ShowAsync();
                        break;
                    default:
                        await new MessageDialog("It seems you are using a " + t + " device. This device does not support Primary tile API", "live tile failed").ShowAsync();
                        break;
                }
            }
        }
        public async void Refresh(object sender, RoutedEventArgs e)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                ProgressRing.IsActive = true;
                switch (PivotNavigator.SelectedIndex.ToString())
                {
                    case "0":
                        try
                        {
                            var Newcollection = new IncrementalLoadingCollection<NewPostsClass, Posts>();
                            NewGridViewControl.ItemsSource = Newcollection;
                        }
                        catch
                        {
                            RefreshAccess();
                        }
                        ProgressRing.IsActive = false;
                        break;
                    case "1":
                        var Hotcollection = new IncrementalLoadingCollection<HotPostsClass, Posts>();
                        HotGridViewControl.ItemsSource = Hotcollection;
                        ProgressRing.IsActive = false;
                        break;
                    case "2":
                        var Topcollection = new IncrementalLoadingCollection<TopPostsClass, Posts>();
                        TopGridViewControl.ItemsSource = Topcollection;
                        ProgressRing.IsActive = false;
                        break;
                    case "3":
                        var Risingcollection = new IncrementalLoadingCollection<HotPostsClass, Posts>();
                        RisingGridViewControl.ItemsSource = Risingcollection;
                        var Controversialcollection = new IncrementalLoadingCollection<HotPostsClass, Posts>();
                        ControversialGridViewControl.ItemsSource = Controversialcollection;
                        ProgressRing.IsActive = false;
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
                switch (PivotNavigator.SelectedIndex.ToString())
                {
                    case "0":
                        try
                        {
                            var Newcollection = new IncrementalLoadingCollection<NewPostsClass, Posts>();
                            NewGridViewControl.ItemsSource = Newcollection;
                        }
                        catch
                        {
                            RefreshAccess();
                        }
                        ProgressRing.IsActive = false;
                        break;
                    case "1":
                        var Hotcollection = new IncrementalLoadingCollection<HotPostsClass, Posts>();
                        HotGridViewControl.ItemsSource = Hotcollection;
                        ProgressRing.IsActive = false;
                        break;
                    case "2":
                        var Topcollection = new IncrementalLoadingCollection<TopPostsClass, Posts>();
                        TopGridViewControl.ItemsSource = Topcollection;
                        ProgressRing.IsActive = false;
                        break;
                    case "3":
                        var Risingcollection = new IncrementalLoadingCollection<HotPostsClass, Posts>();
                        RisingGridViewControl.ItemsSource = Risingcollection;
                        var Controversialcollection = new IncrementalLoadingCollection<HotPostsClass, Posts>();
                        ControversialGridViewControl.ItemsSource = Controversialcollection;
                        ProgressRing.IsActive = false;
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

        private void UniversalRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            PostFlair = rb.Content.ToString();
        }
        private async void OpenCreatePostDialog(object sender, RoutedEventArgs e)
        {
            FindName("SearchTip");
            SearchTip.IsOpen = false;
            FindName("CreatePostDialog");
            await CreatePostDialog.ShowAsync();
        }
        private async void CreatePostDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async() =>
            {
                if (NewPostPivot.SelectedIndex == 0)
                {
                    try
                    {
                        await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                        {
                            if (String.IsNullOrEmpty(TitlePostText.Text.ToString()))
                            {
                                return;
                            }
                            else
                            {
                                string TITLETEST = TitlePostText.Text.ToString();
                                var reddit = new RedditAPI(appId, refreshToken, secret);
                                var subreddit = reddit.Subreddit("Appraisit");
                                subreddit.SelfPost(title: TitlePostText.Text.ToString(), selfText: PostText.Text.ToString()).Submit().SetFlair(PostFlair.ToString());
                            }
                        });
                    }
                    catch
                    {

                    }
                }
                else
                {
                    try
                    {
                        await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                        {
                            if (String.IsNullOrEmpty(TitlePostText.Text.ToString()))
                            {
                                return;
                            }
                            else
                            {
                                Uri Link = new Uri(NewPostLink.Text.ToString());
                                if (Uri.IsWellFormedUriString(Link.ToString(), UriKind.Absolute) == true)
                                {
                                    string TITLETEST = TitlePostText.Text.ToString();
                                    var reddit = new RedditAPI(appId, refreshToken, secret);
                                    var subreddit = reddit.Subreddit("Appraisit");
                                    subreddit.LinkPost(title: TitlePostText.Text.ToString(), url: Link.ToString()).Submit().SetFlair(PostFlair.ToString());
                                }
                            }
                        });
                    }
                    catch
                    {
                        return;
                    }
                }
                UniversalPageNotification.Show("Post created (refresh to view)");
            });
        }

        private async void OpenInBrowserButton_Click(object sender, RoutedEventArgs e)
        {
            var uriBing = new Uri(webView.Source.ToString());

            // Launch the URI
            await Windows.System.Launcher.LaunchUriAsync(uriBing);
        }

        private async void TopComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                var Topcollection = new IncrementalLoadingCollection<TopPostsClass, Posts>();
                TopPostsClass TopGenerator = new TopPostsClass();
                string newOrder = TopOrder.SelectedItem.ToString();
                switch (newOrder)
                {
                    case "all":
                        TopGenerator.Order = newOrder;
                        TopGridViewControl.ItemsSource = Topcollection;
                        break;
                    case "year":
                        TopGenerator.Order = newOrder;
                        TopGridViewControl.ItemsSource = Topcollection;
                        break;
                    case "month":
                        TopGenerator.Order = newOrder;
                        TopGridViewControl.ItemsSource = Topcollection;
                        break;
                    case "week":
                        TopGenerator.Order = newOrder;
                        TopGridViewControl.ItemsSource = Topcollection;
                        break;
                    case "day":
                        TopGenerator.Order = newOrder;
                        TopGridViewControl.ItemsSource = Topcollection;
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
        }

        private async void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async() =>
            {
                var reddit = new RedditAPI(appId, refreshToken, secret);
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    UserName.Text = "Username: " + reddit.Account.Me.Name;
                    Cakeday.Text = "Created/Cake Day: " + reddit.Account.Me.Created.ToString();
                    LinkKarma.Text = "Karma: " + reddit.Account.Me.LinkKarma;
                    CommentKarma.Text = "Comment Karma: " + reddit.Account.Me.CommentKarma;
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
            await Task.Run(async() =>
            {
                FindName("SearchTip");
                SearchTip.IsOpen = false;
                FindName("CreatePostDialog");
                await CreatePostDialog.ShowAsync();
            });
        }

        private async void SwipeItem_Invoked_2(MUXC.SwipeItem sender, MUXC.SwipeItemInvokedEventArgs args)
        {
            await Task.Run(() =>
            {
                FindName("SearchTip");
                SearchTip.IsOpen = true;
            });
        }

        private async void SwipeItem_Invoked_3(MUXC.SwipeItem sender, MUXC.SwipeItemInvokedEventArgs args)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                ProgressRing.IsActive = true;
                switch (PivotNavigator.SelectedIndex.ToString())
                {
                    case "0":
                        try
                        {
                            var Newcollection = new IncrementalLoadingCollection<NewPostsClass, Posts>();
                            NewGridViewControl.ItemsSource = Newcollection;
                        }
                        catch
                        {
                            RefreshAccess();
                        }
                        ProgressRing.IsActive = false;
                        break;
                    case "1":
                        var Hotcollection = new IncrementalLoadingCollection<HotPostsClass, Posts>();
                        HotGridViewControl.ItemsSource = Hotcollection;
                        ProgressRing.IsActive = false;
                        break;
                    case "2":
                        var Topcollection = new IncrementalLoadingCollection<TopPostsClass, Posts>();
                        TopGridViewControl.ItemsSource = Topcollection;
                        ProgressRing.IsActive = false;
                        break;
                    case "3":
                        var Risingcollection = new IncrementalLoadingCollection<HotPostsClass, Posts>();
                        RisingGridViewControl.ItemsSource = Risingcollection;
                        var Controversialcollection = new IncrementalLoadingCollection<HotPostsClass, Posts>();
                        ControversialGridViewControl.ItemsSource = Controversialcollection;
                        ProgressRing.IsActive = false;
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
                    switch (PivotNavigator.SelectedIndex.ToString())
                    {
                        case "0":
                            NewGridViewControl.OneRowModeEnabled = true;
                            view = view + 1;
                            break;
                        case "1":
                            HotGridViewControl.OneRowModeEnabled = true;
                            view = view + 1;
                            break;
                        case "2":
                            TopGridViewControl.OneRowModeEnabled = true;
                            view = view + 1;
                            break;
                        case "3":
                            RisingGridViewControl.OneRowModeEnabled = true;
                            ControversialGridViewControl.OneRowModeEnabled = true;
                            view = view + 1;
                            break;
                    }
                }
                else
                {
                    switch (PivotNavigator.SelectedIndex.ToString())
                    {
                        case "0":
                            NewGridViewControl.OneRowModeEnabled = false;
                            view = view - 1;
                            break;
                        case "1":
                            HotGridViewControl.OneRowModeEnabled = false;
                            view = view - 1;
                            break;
                        case "2":
                            TopGridViewControl.OneRowModeEnabled = false;
                            view = view - 1;
                            break;
                        case "3":
                            RisingGridViewControl.OneRowModeEnabled = false;
                            ControversialGridViewControl.OneRowModeEnabled = false;
                            view = view - 1;
                            break;
                    }
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
                        UniversalPostNotification.Show("Reply sent (viewing replies isnt supported, go to appraisit subreddit on web to view)");
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
                    UniversalPostNotification.Show("Comment sent (refresh to view)");
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
            var s = (FrameworkElement)sender;
            var D = s.DataContext;
            var dse = D as Comments;
            UniversalPostNotification.Show("Upvoted");
            await dse.CommentSelf.UpvoteAsync();
        }

        private async void DownvoteComment_Click(object sender, RoutedEventArgs e)
        {
            var s = (FrameworkElement)sender;
            var D = s.DataContext;
            var dse = D as Comments;
            UniversalPostNotification.Show("Downvoted");
            await dse.CommentSelf.DownvoteAsync();
        }
    }
}

