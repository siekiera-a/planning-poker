using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using Client.Models;
using Client.Service;
using Client.ViewModels;
using Notifications.Wpf.Core;
using Server.Dtos.Outgoing;

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for CreateView.xaml
    /// </summary>
    public partial class JoinTeamView : UserControl
    {
        private string _message = "";

        public JoinTeamView()
        {
            InitializeComponent();
        }

        private async void JoinTeamButtonClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is JoinTeamModel context)
            {
                await context.FetchDataJoinTeam();
                ShowNotification(context);
            }
        }

        private async void CreateTeamButtonClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is JoinTeamModel context)
            {
                await context.FetchDataCreateTeam();
                ShowNotification(context);
            }
        }

        private async void ShowNotification(JoinTeamModel context)
        {
            var notificationManager = new NotificationManager();
            await notificationManager.ShowAsync(
                new NotificationContent
                {
                    Title = context.NotificationText,
                },
                areaName: "WindowArea");
        }
    }
}