using System;
using System.Windows;
using System.Windows.Controls;
using Notifications.Wpf.Core;

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for CreateView.xaml
    /// </summary>
    public partial class JoinTeamView : UserControl
    {
        public JoinTeamView()
        {
            InitializeComponent();
        }

        private async void JoinTeamButtonClick(object sender, RoutedEventArgs e)
        {
            if (TeamName.Text.Length > 0)
            {
                string messageText = TeamName.Text;
                TeamName.Clear();

                var notificationManager = new NotificationManager();

                await notificationManager.ShowAsync(
                    new NotificationContent
                    {
                        Title = "You've joined the team", Message = messageText
                    },
                    areaName: "WindowArea");
            }
        }
    }
}