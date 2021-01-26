using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Client.Service;
using Client.ViewModels.Game;
using Microsoft.AspNetCore.SignalR.Client;
using Server.Models.Server;

namespace Client.Views.Game
{
    /// <summary>
    /// Interaction logic for AdminGameWindow.xaml
    /// </summary>
    public partial class AdminGameWindow : Window
    {
        private readonly IGameManager _connection;
        public AdminGameWindow()
        {
            InitializeComponent();
            _connection = Services.GetService<IGameManager>();
            Loaded += FetchMembers;
        }

        private async void FetchMembers(object sender, RoutedEventArgs e)
        {
            if (DataContext is AdminGameViewModel context)
            {

                var users = await _connection._connection.InvokeAsync<OrganizerResponse>("currentTask", _connection.MeetingId);

                if (users.Clients != null)
                {
                    context.Users.Clear();
                    foreach (var c in users.Clients)
                    {
                        context.Users.Add(c);
                    }
                }

                
                var clientResponse = await _connection._connection.InvokeAsync<ClientResponse>("currentTask", _connection.MeetingId);
                Task.Text = clientResponse.Description;
            }
        }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is AdminGameViewModel context)
            {
                var x = ListView.SelectedItem;
                await _connection._connection.InvokeAsync<bool>("currentTask", _connection.MeetingId);
            }
        }
    }
}
