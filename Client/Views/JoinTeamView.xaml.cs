using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using Client.Models;
using Notifications.Wpf.Core;

namespace Client.Views
{
    public class Joincsos
    {
        public int TeamId { get; set; }
    }

    /// <summary>
    /// Interaction logic for CreateView.xaml
    /// </summary>
    public partial class JoinTeamView : UserControl
    {
        private readonly IApiClient _apiClient;

        public JoinTeamView()
        {
            InitializeComponent();
            _apiClient = Services.GetService<IApiClient>();
        }

        private async void JoinTeamButtonClick(object sender, RoutedEventArgs e)
        {

            var response = await _apiClient.PostAsyncAuth<Joincsos>("/team/join-code", new {Code = TeamName.Text});

            string message = "";

            if (response.IsOk)
            {
                message = "You've joined the team";

                

            }
            else
            {
                if (response.HttpStatusCode == HttpStatusCode.BadRequest || response.HttpStatusCode == HttpStatusCode.NotFound)
                {
                    message = response.Error.Message;
                }
            }


            TeamName.Clear();

            var notificationManager = new NotificationManager();

            await notificationManager.ShowAsync(
                new NotificationContent
                {
                    Title = message,

                },
                areaName: "WindowArea");


        }
    }
}