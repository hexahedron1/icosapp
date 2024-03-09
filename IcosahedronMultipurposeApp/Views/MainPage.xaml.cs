using System.Net.NetworkInformation;
using IcosahedronMultipurposeApp.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Dispatching;
using Windows.Networking.Connectivity;
using Windows.UI.Core;
using Windows.ApplicationModel.Core;
using Windows.Media.Protection.PlayReady;

namespace IcosahedronMultipurposeApp.Views;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel
    {
        get;
    }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();
    }
    Exception latestError;
    bool checkedForUpdates = false;
    private async void GetLatestVersion_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {/*
        try
        {
            HttpClient client = new HttpClient();
            string result = await client.GetStringAsync("https://raw.githubusercontent.com/hexahedron1/files/main/icosapp/data.txt");
            await new ContentDialog()
            {
                XamlRoot = this.XamlRoot,
                Title = "Latest version",
                Content = result,
                CloseButtonText = "Ok",
                DefaultButton = ContentDialogButton.Close
            }.ShowAsync();
        } catch (Exception exc)
        {
            latestError = exc;
            ErrorInfoBar.Title = exc.GetType().Name;
            ErrorInfoBar.Message = "An error occured when getting newest version.";
            ErrorInfoBar.IsOpen = true;
        }*/
        Page_Loaded(sender, e);
    }

    private async void Page_Loaded(object sender, RoutedEventArgs e)
    {
        Progressbar.ShowPaused = false;
        Progressbar.ShowError = false;
        Progressbar.Visibility = Visibility.Visible;
        NetworkInfoBar.Message = "Checking connection...";
        NetworkInfoBar.Severity = InfoBarSeverity.Informational;
        NetworkInfoBar.IsOpen = true;
        NetworkInfoBar.IsClosable = false;
        bool internet = NetworkInterface.GetIsNetworkAvailable();
        NetworkChange.NetworkAvailabilityChanged += NetworkAvailabilityChanged;
        if (!internet)
        {
            NetworkInfoBar.Message = "No internet connection";
            NetworkInfoBar.Severity = InfoBarSeverity.Warning;
            NetworkInfoBar.IsOpen = true;
            NetworkInfoBar.IsClosable = false;
            ShellPage.mailNavItem.IsEnabled = false;
            Progressbar.Visibility = Visibility.Collapsed;
        }
        else if (!checkedForUpdates)
        {
            checkedForUpdates = true;
            NetworkInfoBar.IsOpen = false;
            UpdateInfoBar.IsOpen = true;
            UpdateInfoBar.Message = "Checking for updates...";
            UpdateInfoBar.Severity = InfoBarSeverity.Informational;
            using HttpClient client = new HttpClient();
            try
            {
                var result = await client.GetStringAsync("https://raw.githubusercontent.com/hexahedron1/files/main/icosapp/data.txt");
                var version = int.Parse(result);
                if (version > Data.version)
                {
                    UpdateInfoBar.Message = "An update is available. Check the settings page to update.";
                    UpdateInfoBar.Severity = InfoBarSeverity.Warning;
                    UpdateInfoBar.ActionButton = new Button()
                    {
                        Content = "Changelog",
                    };
                    UpdateInfoBar.ActionButton.Click += GetChangelogButton_Click;
                    Progressbar.Visibility = Visibility.Collapsed;
                }
                else
                {
                    UpdateInfoBar.IsOpen = false;
                    Progressbar.Visibility = Visibility.Collapsed;
                }
            } catch (Exception exc)
            {
                latestError = exc;
                ErrorInfoBar.Title = exc.GetType().Name;
                ErrorInfoBar.Message = "An error occured when getting newest version.";
                ErrorInfoBar.IsOpen = true;
                Progressbar.Visibility = Visibility.Collapsed;
            }
        }
        else
        {
            UpdateInfoBar.IsOpen = false;
            Progressbar.Visibility = Visibility.Collapsed;
        } 
    }

    private void NetworkAvailabilityChanged(object? sender, NetworkAvailabilityEventArgs e)
    {
        DispatcherQueue.TryEnqueue(() => {
            ShellPage.mailNavItem.IsEnabled = e.IsAvailable;
            GetLatestVersion.IsEnabled = e.IsAvailable;
            if (!e.IsAvailable)
            {
                NetworkInfoBar.Message = "No internet connection";
                NetworkInfoBar.Severity = InfoBarSeverity.Warning;
                NetworkInfoBar.IsOpen = true;
                NetworkInfoBar.IsClosable = false;
            }
            else
            {
                NetworkInfoBar.Message = "Reconnected";
                NetworkInfoBar.Severity = InfoBarSeverity.Success;
                NetworkInfoBar.IsOpen = true;
                NetworkInfoBar.IsClosable = true;
            }
        });
    }

    private async void ShowErrorInfoButton_Click(object sender, RoutedEventArgs e)
    {
        await new ContentDialog()
        {
            XamlRoot = this.XamlRoot,
            Title = "Error info",
            Content = latestError.ToString(),
            CloseButtonText = "Ok",
            DefaultButton = ContentDialogButton.Close
        }.ShowAsync();
    }

    private async void GetChangelogButton_Click(object sender, RoutedEventArgs e)
    {
        using HttpClient client = new HttpClient();
        try
        {
            var result = await client.GetStringAsync("https://raw.githubusercontent.com/hexahedron1/files/main/icosapp/data.txt");
            var newVersion = int.Parse(result);
            var result2 = await client.GetStringAsync("https://raw.githubusercontent.com/hexahedron1/files/main/icosapp/changelog.txt");
            ContentDialog dialog = new ContentDialog()
            {
                Title = $"v{newVersion} Changelog",
                XamlRoot = this.XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                Content = result2,
                CloseButtonText = "Close"
            };
            await dialog.ShowAsync();
        } catch (Exception err) {
            ErrorInfoBar.Message = $"{err.GetType().Name}: {err.Message}";
            ErrorInfoBar.IsOpen = true;
            latestError = err;
        }
    }
}
