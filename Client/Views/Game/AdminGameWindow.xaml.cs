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
        private readonly IGameManager _manager;

        public AdminGameWindow()
        {
            InitializeComponent();
            _manager = Services.GetService<IGameManager>();
            Loaded += FetchMembers;
            _manager.SubmittedEvent += HandleSubmitted;
            _manager.CloseEvent += CloseWindow;
        }

        private async void FetchMembers(object sender, RoutedEventArgs e)
        {
            if (DataContext is AdminGameViewModel context)
            {

                var response = await _manager._connection.InvokeAsync<OrganizerResponse>("currentTask", _manager.MeetingId);

                if (response.Clients != null)
                {
                    context.Users.Clear();
                    foreach (var c in response.Clients)
                    {
                        context.Users.Add(c);
                    }
                }


                Task.Text = response.Description;
            }
        }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is AdminGameViewModel context)
            {
                if (ListView.SelectedItem is Server.Models.Server.Client client)
                {
                    await _manager._connection.InvokeAsync<bool>("assignUser", _manager.MeetingId, client.Id);
                    await _manager._connection.InvokeAsync("next", _manager.MeetingId);
                }
            }
        }

        private void HandleSubmitted(object o, OrganizerResponse e)
        {
            if (DataContext is AdminGameViewModel context)
            {
                if (e.Clients != null)
                {
                    context.Users.Clear();
                    foreach (var c in e.Clients)
                    {
                        context.Users.Add(c);
                    }
                }
                Task.Text = e.Description;
            }
        }

        private void CloseWindow(object sender, EventArgs e)
        {
            Window.GetWindow(this)?.Close();
        }

        private async void Rewind_Button(object sender, RoutedEventArgs e)
        {
            await _manager._connection.InvokeAsync("rewind", _manager.MeetingId);
        }
    }

}
