﻿using System;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using 酷安_UWP.CoolApk;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace 酷安_UWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MyPage : Page
    {
        User user;
        public MyPage()
        {
            this.InitializeComponent();

            MainPage._User_Name = User_Name;
            MainPage._User_Face = User_Face;

            //加载用户信息
            LoadUserProfile();
        }

        public async void LoadUserProfile()
        {
            //本地信息
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            try
            {
                if (localSettings.Values["login"].ToString().Contains("1"))
                {
                    User_Name.Text = localSettings.Values["name"].ToString();
                    User_Face.ImageSource = new BitmapImage(new Uri(localSettings.Values["face"].ToString(), UriKind.RelativeOrAbsolute));
                }
            }
            catch (Exception)
            {
                localSettings.Values["login"] = "0";
                levelGrid.Visibility = Visibility.Collapsed;
            }

            try
            {
                user = await CoolApkSDK.GetUserProfileByName(localSettings.Values["name"].ToString());

                if (user != null)
                {
                    dt.Text = user.feed.ToString();
                    gz.Text = user.follow.ToString();
                    fs.Text = user.fans.ToString();
                    level.Text = "Lv." + user.level.ToString();
                    levelGrid.Visibility = Visibility.Visible;
                }

            }
            catch (Exception)
            {
                localSettings.Values["name"] = "null";
            }

        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            // OpenAppPage("https://www.coolapk.com/apk/" + ttt.Text);
            MainPage._ViewFrame.Navigate(typeof(LoginPage));
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            MainPage._ViewFrame.Visibility = Visibility.Visible;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (user == null) return;
            MainPage._ViewFrame.Navigate(typeof(FeedPage),user.uid);
            MainPage._ViewFrame.Visibility = Visibility.Visible;
        }
    }
}
