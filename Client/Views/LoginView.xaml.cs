using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Client.Models;
using Client.State.Navigators;
using Client.ViewModels;
using Client.Views.Login;
using Client.Views.Game;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Server.Dtos.Outgoing;

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        private readonly IApiClient _apiClient;
        private readonly ITokenManager _tokenManager;

        public LoginView()
        {
            InitializeComponent();
            _apiClient = Services.GetService<IApiClient>();
            _tokenManager = Services.GetService<ITokenManager>();
        }

        private async void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            var response = await _apiClient.PostAsync<LoginResponse>("/User/login",
                new {Email = Email.Text, Password = Password.Password});

            if (response.IsOk)
            {
                _tokenManager.Token = response.Value.Token;
                Window window = new MainWindow
                {
                    DataContext = new MainViewModel()
                };
                window.Show();
                Window.GetWindow(this)?.Close();
            }
            else
            {
                if (response.HttpStatusCode == HttpStatusCode.Unauthorized)
                {
                    ErrorMessage.Text = response.Error.Message;
                    ErrorMessage.Visibility = Visibility.Visible;
                }
            }
        }

        private void RegisterButtonClick(object sender, RoutedEventArgs e)
        {
            Window window = new RegisterWindow
            {
                DataContext = new RegisterViewModel()
            };
            window.Show();

            Window.GetWindow(this)?.Close();
        }

        private void Password_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel context)
            {
                context.Password = Password.Password;
            }
        }
    }
}