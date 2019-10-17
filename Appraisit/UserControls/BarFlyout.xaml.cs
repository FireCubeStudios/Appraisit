using Appraisit.Helpers;
using Appraisit.Services;
using Appraisit.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Shell;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Appraisit.UserControls
{
    public sealed partial class BarFlyout : UserControl
    {
        public BarFlyout()
        {
            this.InitializeComponent();
            Initialize();
        }
        public event EventHandler BackdropTapped;
        private ElementTheme _elementTheme = ThemeSelectorService.Theme;
        private void Backdrop_Tapped(object sender, TappedRoutedEventArgs e) => BackdropTapped?.Invoke(this, new EventArgs());
        public void MyFancyPanel_BackdrpClicked(object sender, RoutedEventArgs e)
        {
            // MainPage m = new MainPage();
            // m.MyFancyPanel_BackdropClicked();
            UnloadObject(UserSettings);
        }
        public ElementTheme ElementTheme
        {
            get { return _elementTheme; }

            set { Set(ref _elementTheme, value); }
        }
        private void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        private string _versionDescription;

        public string VersionDescription
        {
            get { return _versionDescription; }

            set { Set(ref _versionDescription, value); }
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
                        await new MessageDialog("AlreadyPinnedDesc".GetLocalized(), "AlreadyPinnedTitle".GetLocalized()).ShowAsync();
                    }
                    else
                    {
                        bool IsPinned = await TaskbarManager.GetDefault().RequestPinCurrentAppAsync();
                    }
                }

                else
                {
                    await new MessageDialog("UpdateToPin".GetLocalized(), "UpdateDevice".GetLocalized()).ShowAsync();
                }
            }



            else
            {
                var t = Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily;
                switch (t)
                {
                    case "Windows.Desktop":
                        await new MessageDialog("GPODisabledPin".GetLocalized(), "TaskbarPinFailed".GetLocalized()).ShowAsync();
                        break;
                    case "Windows.Mobile":
                        await new MessageDialog("WoADisabledPin".GetLocalized(), "TaskbarPinFailed".GetLocalized()).ShowAsync();
                        break;
                    case "Windows.IoT":
                        await new MessageDialog("IoTDisabledPin".GetLocalized(), "TaskbarPinFailed".GetLocalized()).ShowAsync();
                        break;
                    case "Windows.Team":
                        break;
                    case "Windows.Holographic":
                        await new MessageDialog("HololensDisabledPin".GetLocalized(), "TaskbarPinFailed".GetLocalized()).ShowAsync();
                        break;
                    case "Windows.Xbox":
                        await new MessageDialog("XboxDisabledPin".GetLocalized(), "TaskbarPinFailed".GetLocalized()).ShowAsync();
                        break;
                    default:
                        await new MessageDialog(string.Format("OtherDisabledPin".GetLocalized(), t), "TaskbarPinFailed".GetLocalized()).ShowAsync();
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
                        await new MessageDialog("AlreadyTileDesc".GetLocalized(), "AlreadyTile".GetLocalized()).ShowAsync();
                    }
                    else
                    {
                        bool IsPinned = await StartScreenManager.GetDefault().RequestAddAppListEntryAsync(entry);
                    }
                }
                else
                {
                    await new MessageDialog("UpdateToTile".GetLocalized(), "UpdateDevice".GetLocalized()).ShowAsync();
                }
            }
            else
            {
                var t = Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily;
                switch (t)
                {
                    case "Windows.IoT":
                        await new MessageDialog("IoTTileFail".GetLocalized(), "LiveTileFailed".GetLocalized()).ShowAsync();
                        break;
                    case "Windows.Team":
                        break;
                    case "Windows.Holographic":
                        await new MessageDialog("HololensTileFail".GetLocalized(), "LiveTileFailed".GetLocalized()).ShowAsync();
                        break;
                    case "Windows.Xbox":
                        await new MessageDialog("XboxTileFail".GetLocalized(), "LiveTileFailed".GetLocalized()).ShowAsync();
                        break;
                    default:
                        await new MessageDialog(string.Format("OtherTileFail".GetLocalized(), t), "LiveTileFailed".GetLocalized()).ShowAsync();
                        break;
                }
            }
        }



    }
}
