using Reddit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Appraisit.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Sidebar : Page
    {
        public string appId = "-bL9o_t7kgNNmA";
        // string backupRefreshToken = "209908787246-qLtBfs46Ci9dWAcesPmmCZt-lz0";
        public Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        public string refreshToken;
        public string BackuprefreshToken = "344019503430-vIqovAQ5eVORrnpqlnyI3ScgOkE";
        public string accessToken;
        public string backupaccessToken = "344019503430-BUX_uSW3ZfNr9wqbcWU5GMp8qfU";
        // string backupAccesToken = "209908787246-EHnGFWXgWZDrmpEv3iYmkXLB-ew";
        public string secret = "SESshAirmwAuAvBFHbq_JUkAMmk";
        public Sidebar()
        {
            this.InitializeComponent();
            refreshToken = localSettings.Values["refresh_token"].ToString();
            var reddit = new RedditAPI(appId, localSettings.Values["refresh_token"].ToString(), secret);
            var subreddit = reddit.Subreddit("Appraisit");
            Joined.Text = subreddit.Subscribers.ToString() + "160+ Reddit Subscribers";
            Online.Text = subreddit.ActiveUserCount.ToString() + "error Online";
            About.Text = " Discover new apps and games. Learn about improvements to your favorites and help them get discovered. You decide who rises to the top!";
        }
    }
}
