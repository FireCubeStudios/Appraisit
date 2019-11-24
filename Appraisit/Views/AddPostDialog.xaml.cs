using Appraisit.Helpers;
using Reddit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Profile;
using Windows.UI.Core;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Toolkit.Uwp;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using MUXC = Microsoft.UI.Xaml.Controls;
using Microsoft.Toolkit.Uwp.UI.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Appraisit.Views
{
    public sealed partial class AddPostDialog : ContentDialog
    {
        public string appId = "-bL9o_t7kgNNmA";
        // string backupRefreshToken = "209908787246-qLtBfs46Ci9dWAcesPmmCZt-lz0";
        public Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        public string refreshToken;
        public string BackuprefreshToken = "387967562864-jQyY5b8B4Ln2Cm5mNiyX65NteNo";
        public string accessToken;
        public string FlairTemplate;
        public string backupbackuprefreshToken = "";
        // string backupAccesToken = "209908787246-EHnGFWXgWZDrmpEv3iYmkXLB-ew";
        public string secret = "SESshAirmwAuAvBFHbq_JUkAMmk";
        public AddPostDialog()
        {
            this.InitializeComponent();
            refreshToken = localSettings.Values["refresh_token"].ToString();
        }
        private void UniversalRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            switch (rb.Content.ToString())
            {
                case "Update":
                    FlairTemplate = "89cc3e84-8ba2-11e9-b1b0-0ecce5ed8ed0";
                    break;
                case "Price Drop":
                    FlairTemplate = "5944e7c0-8ba2-11e9-940b-0e44c05763c0";
                    break;
                case "Non ms store app":
                    FlairTemplate = "7284e7d0-b889-11e9-aeb1-0e11aefa7bba";
                    break;
                case "New Release":
                    FlairTemplate = "0beabbbe-6765-11e9-a21c-0e06ff2b8078";
                    break;
                case "Discover":
                    FlairTemplate = "29f854ac-8ba2-11e9-aaa1-0e0ea830c7ec";
                    break;
            }

        }
        private async void CreatePostDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                try
                {
                    await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, ()=>
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
                            subreddit.SelfPost(title: TitlePostText.Text.ToString(), selfText: PostText.Document.Selection.Text).Submit().SetFlair(flairTemplateId: FlairTemplate);
                            TitlePostText.Text = "";
                            PostText.Document.SetText(TextSetOptions.None, String.Empty);
                        }
                    });
                }
                catch
                {
                    if (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
                    {
                        MainPage.UniversalTipPost.IsOpen = true;
                        MainPage.UniversalTipPost.Title = "Reddit api limit. try again in 10 min. unfortunately i cant stop this however this limit will go away once you get some karma on appraisit.";
                    }
                    else
                    {
                        MainPage.UniversalPageNotificationPost.Show("Reddit api limit. try again in 10 min. unfortunately i cant stop this however this limit will go away once you get some karma on appraisit", 3000);
                    }
                }

                try
                {
                    await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,() =>
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
                                subreddit.LinkPost(title: TitlePostText.Text.ToString(), url: Link.ToString()).Submit().SetFlair(flairTemplateId: FlairTemplate);
                                TitlePostText.Text = "";
                                PostText.Document.SetText(TextSetOptions.None, String.Empty);
                            }
                        }
                    });
                }
                catch
                {
                    if (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
                    {
                        MainPage.UniversalTipPost.IsOpen = true;
                        MainPage.UniversalTipPost.Title = "Reddit api limit message: You are posting too much try again in x minutes";
                    }
                    else
                    {
                        MainPage.UniversalPageNotificationPost.Show("Reddit posting limit. Try again in 7 minutes or use web", 3000);
                    }
                }

                if ((string)localSettings.Values["refresh_token"] == BackuprefreshToken)
                {
                    if (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
                    {
                        MainPage.UniversalTipPost.IsOpen = true;
                        MainPage.UniversalTipPost.Title = "Post created. Sign in to use custom username";
                    }
                    else
                    {
                       MainPage.UniversalPageNotificationPost.Show("Post created. Sign in to use custom username", 3000);
                    }
                }
                else
                {
                    if (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
                    {
                        MainPage.UniversalTipPost.IsOpen = true;
                        MainPage.UniversalTipPost.Title = "PostCreated".GetLocalized();
                    }
                    else
                    {
                        MainPage.UniversalPageNotificationPost.Show("PostCreated".GetLocalized(), 3000);
                    }
                }
            });
        }
    }
}
