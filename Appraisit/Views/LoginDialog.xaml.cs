using Appraisit.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Web;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Profile;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Appraisit.Views
{
    public sealed partial class LoginDialog : ContentDialog
    {
        public string appId = "-bL9o_t7kgNNmA";
        // string backupRefreshToken = "209908787246-qLtBfs46Ci9dWAcesPmmCZt-lz0";
        public Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        public string refreshToken;
        public string BackuprefreshToken = "387967562864-jQyY5b8B4Ln2Cm5mNiyX65NteNo";
        public string accessToken;
        public string backupbackuprefreshToken = "";
        // string backupAccesToken = "209908787246-EHnGFWXgWZDrmpEv3iYmkXLB-ew";
        public string secret = "SESshAirmwAuAvBFHbq_JUkAMmk";
        public LoginDialog()
        {
            this.InitializeComponent();
            StartUp();
        }
        private async void StartUp()
        {
           
            FindName("Fail");
            FindName("Block");
            UnloadObject(Wblock);
            var scopes = Constants.Constants.scopeList.Aggregate("", (acc, x) => acc + " " + x);
            var urlParams = "client_id=" + appId + "&response_type=code&state=uyagsjgfhjs&duration=permanent&redirect_uri=" + HttpUtility.UrlEncode("http://127.0.0.1:3000/reddit_callback") + "&scope=" + HttpUtility.UrlEncode(scopes);
            Uri targetUri = new Uri(Constants.Constants.redditApiBaseUrl + "authorize?" + urlParams);
            loginView.Navigate(targetUri);
            if (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
            {
                var messageDialog = new MessageDialog("Mobile login might not work in new login experience");
                await messageDialog.ShowAsync();
            }
            else
            {
                FindName("Wblock");
            }
            UnloadObject(loginView);
        }
     
        private async void LoginView_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {

            LoginHelper loginHelper = new LoginHelper(appId, secret);
            if (args.Uri.AbsoluteUri.Contains("http://127.0.0.1:3000/reddit_callback"))
            {
                var result = await loginHelper.Login_Stage2(args.Uri);
                accessToken = result.AccessToken;
                refreshToken = result.RefreshToken;
                // NewPivotItem.Header = result.RefreshToken;
                localSettings.Values["refresh_token"] = result.RefreshToken;
                FindName("Success");
                //NewPivotItem.Header = refreshToken;
                if (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
                {
                    FindName("MobileBar");
                    FindName("SB");
                   // PivotBar.Visibility = Visibility.Collapsed;
                }
                FindName("Block");

            }
            else if (args.Uri.ToString() == "https://www.reddit.com/coins/" && AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
            {
                UnloadObject(loginView);
                FindName("Fail");
                FindName("Block");
                UnloadObject(Wblock);
            }
            else if (args.Uri.AbsoluteUri.Contains("https://play.google.com/store/apps/details?id=com.reddit.frontpage") && AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
            {
                UnloadObject(loginView);
                FindName("Fail");
                FindName("Block");
                UnloadObject(Wblock);
            }
            else if (args.Uri.ToString() == "https://www.reddit.com/premium/" && AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
            {
                UnloadObject(loginView);
                FindName("Fail");
                FindName("Block");
                UnloadObject(Wblock);
            }
            else if (args.Uri.ToString() == "https://www.redditgifts.com/" && AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
            {
                UnloadObject(loginView);
                FindName("Fail");
                FindName("Block");
                UnloadObject(Wblock);
            }
            else if (args.Uri.ToString() == "https://apps.apple.com/us/app/reddit-the-official-app/id1064216828" && AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
            {
                UnloadObject(loginView);
                FindName("Fail");
                FindName("Block");
                UnloadObject(Wblock);
            }
            else
            {
                return;
            }



        }
       

    }
}
