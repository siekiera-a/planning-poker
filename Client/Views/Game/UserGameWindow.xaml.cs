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
    public partial class GameWindow : Window
    {
        private readonly IGameManager _manager;
        public GameWindow()
        {
            InitializeComponent();
            _manager = Services.GetService<IGameManager>();
            Loaded += TaskDescription;
            _manager.CloseEvent += WindowClose;
            _manager.TaskChangedEvent += HandleTask;
        }

        public async void TaskDescription(object sender, RoutedEventArgs e)
        {
            if (DataContext is UserGameViewModel context)
            {
                var clientResponse = await _manager.Connection.InvokeAsync<ClientResponse>("currentTask", _manager.MeetingId);
                Question.Text = clientResponse.Description;
            }
        }


        public void WindowClose(object sender, EventArgs e)
        {
            Window.GetWindow(this)?.Close();
        }

        public void HandleTask(object sender, ClientResponse e)
        {
            if (DataContext is UserGameViewModel context)
            {
                context.QuestionText = e.Description;
                Question.Text = e.Description;

            }
        }

    }
}
