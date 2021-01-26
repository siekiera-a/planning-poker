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
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.AspNetCore.SignalR.Client;
using Server.Models.Server;

namespace Client.Views.Game
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow :Window
    {
        private readonly IGameManager _connection;
        public GameWindow()
        {
            InitializeComponent();
            _connection = Services.GetService<IGameManager>();
            Loaded += TaskDescription;
        }

        public async void TaskDescription(object sender, RoutedEventArgs e)
        {
            if (DataContext is UserGameViewModel context)
            {
                var clientResponse =  await _connection._connection.InvokeAsync<ClientResponse>("currentTask", _connection.MeetingId);
                Task.Text = clientResponse.Description;
                
                context.Description(clientResponse);
            }
        }
    }
}
