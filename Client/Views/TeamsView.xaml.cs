using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Client.Models;
using Client.Service;
using Client.ViewModels;
using Notifications.Wpf.Core;
using Server.Dtos.Outgoing;

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for TeamsView.xaml
    /// </summary>
    public partial class TeamsView : UserControl
    {
        private readonly IApiClient _apiClient;

        public TeamsView()
        {
            InitializeComponent();
            _apiClient = Services.GetService<IApiClient>();
            Loaded += FetchTeams;
        }

        private async void AddMemberByEmail(object sender, RoutedEventArgs e)
        {
            if (DataContext is TeamsViewModel context)
            {
                await context.AddByMail();
                ShowNotification(context);
            }
        }

        private async void GenerateCodeButtonClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is TeamsViewModel context)
            {
                await context.GenerateCode();
            }
        }

        private async void FetchTeams(object sender, RoutedEventArgs e)
        {
            if (DataContext is TeamsViewModel context)
            {
                await context.FetchTeams();
            }
        }

        private async void ComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is TeamsViewModel context)
            {
                if (e.AddedItems[0] is TeamResponse selectedItem)
                {
                    await context.FetchMembers(selectedItem.Id);
                }
            }
        }

        private async void ShowNotification(TeamsViewModel context)
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