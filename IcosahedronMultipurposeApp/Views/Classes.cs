using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Devices.Bluetooth.Advertisement;

namespace IcosahedronMultipurposeApp.Views;
public class User
{
    public User(string username, string name, string profilePicture)
    {
        Name = name;
        Username = username;
        ProfilePicture = profilePicture;
    }
    public string Name
    {
        get; set;
    }
    public string Username
    {
        get; set;
    }
    public string ProfilePicture
    {
        get; set;
    }
}

public class Message
{
    public Message(string content, User author)
    {
        Content = content;
        Author = author;
    }

    public string Content
    {
        get; set;
    }
    public User Author
    {
        get; set;
    }
}
public static class Data {
    public static string Ellipsis(this string s, int maxlength)
    {
        return s.Length > maxlength ? s[..(maxlength - 3)] + "..." : s;
    }
    public const int version = 1;
}