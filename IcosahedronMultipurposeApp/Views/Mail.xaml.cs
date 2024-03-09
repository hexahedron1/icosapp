using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using IcosahedronMultipurposeApp.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using static System.Net.Mime.MediaTypeNames;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace IcosahedronMultipurposeApp.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MailPage : Page
{
    public MailViewModel ViewModel
    {
        get;
    }
    public MailPage()
    {
        ViewModel = App.GetService<MailViewModel>();
        this.InitializeComponent();
        BreadcrumbBar1.ItemsSource = new string[] { "IMS", "Contacts" };
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        ContactList.ItemsSource = new List<User>()
        {
            new User("Amigger", "ammeter", "https://cdn.discordapp.com/avatars/1205193851652931596/281561d63dd94a6471c6462efcf7d0ce.png?size=1024"),
            new User("Qwertyuiop[]", "qwertyuiop", "https://cdn.discordapp.com/avatars/989194786206515240/775ea51f19132b83f0acdcaac5ec023c.png?size=1024")
        };
    }

    private void MessageTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == Windows.System.VirtualKey.Enter)
            SendMessage(MessageTextBox.Text);
    }
    private void SendMessage(string content)
    {
        NoticeTip.IsOpen = true;
        User author = new User("Ammiger", "ammeter", "https://cdn.discordapp.com/avatars/1205193851652931596/281561d63dd94a6471c6462efcf7d0ce.png?size=1024");
        List<Message> messages = ((List<Message>)MessageListView.ItemsSource);
        messages.Add(new Message(content, new User("You", "you", "nuhh")));
        messages.Add(new Message("the heavy is dead", author));
    }

    private void SendMessageButton_Click(object sender, RoutedEventArgs e)
    {
        SendMessage(MessageTextBox.Text);
    }

    private void ContactList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        User user = (User)ContactList.SelectedItem;
        UsernameBlock.Text = user.Username;
        NameBlock.Text = $"@{user.Name}";
        ContactPicture.ProfilePicture = new BitmapImage(new Uri(user.ProfilePicture));
        ContactPicture.DisplayName = user.Username;
        StatusBlock.Text = "Online";
        MessageListView.ItemsSource = new List<Message>()
        {
            new Message($"This chat is showing placeholder messages from {user.Username} (placeholder user)", user),
            new Message("When the app will be finished, this chat will contain actual messages from a real user", user),
        };
        MessageTextBox.PlaceholderText = $"Message @{user.Name}";
        MessageTextBox.IsEnabled = true;
        BreadcrumbBar1.ItemsSource = new string[] { "IMS", "Contacts", user.Name };
        SendMessageButton.IsEnabled = true;
    }
}
