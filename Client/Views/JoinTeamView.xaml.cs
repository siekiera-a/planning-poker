using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using Client.Models;
using Notifications.Wpf.Core;
using Server.Dtos.Outgoing;

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for CreateView.xaml
    /// </summary>
    public partial class JoinTeamView : UserControl
    {
        private readonly IApiClient _apiClient;
        private string message = "";

        public JoinTeamView()
        {
            InitializeComponent();
            _apiClient = Services.GetService<IApiClient>();
        }

        private async void JoinTeamButtonClick(object sender, RoutedEventArgs e)
        {
            var response = await _apiClient.PostAsyncAuth<TeamResponse>("/team/join-code", new {Code = TeamName.Text});

            if (response.IsOk)
            {
                message = "You've joined the team";
            }
            else
            {
                if (response.HttpStatusCode == HttpStatusCode.BadRequest ||
                    response.HttpStatusCode == HttpStatusCode.NotFound)
                {
                    message = response.Error.Message;
                }
            }

            TeamName.Clear();
            ShowNotification(message);
        }

        private async void CreateTeamButtonClick(object sender, RoutedEventArgs e)
        {
            var response = await _apiClient.PostAsyncAuth<TeamResponse>("/team", new {Name = NewTeamName.Text});

            if (response.IsOk)
            {
                message = "New team created successfully";
            }
            else
            {
                message = "Unable to create team";
            }

            NewTeamName.Clear();
            ShowNotification(message);
        }

        private async void ShowNotification(string messageText)
        {
            var notificationManager = new NotificationManager();

            await notificationManager.ShowAsync(
                new NotificationContent
                {
                    Title = messageText,
                },
                areaName: "WindowArea");
        }
    }
}