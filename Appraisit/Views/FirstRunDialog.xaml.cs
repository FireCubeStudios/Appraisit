using Appraisit.Helpers;
using System;
using Windows.System.Profile;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Appraisit.Views
{
    public sealed partial class FirstRunDialog : ContentDialog
    {
        public FirstRunDialog()
        {
            // TODO WTS: Update the contents of this dialog with any important information you want to show when the app is used for the first time.
            RequestedTheme = (Window.Current.Content as FrameworkElement).RequestedTheme;
            InitializeComponent();
           /* if (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
            {
                FirstText.Text = "FirstTextOnMobile".GetLocalized();
            }*/
        }
    }
}
