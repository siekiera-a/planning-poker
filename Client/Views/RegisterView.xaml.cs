using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
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
using Client.Service;
using Client.ViewModels;
using Client.Views.Login;
using Newtonsoft.Json;
using Server.Dtos.Incoming;
using Server.Dtos.Outgoing;

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for RegisterView.xaml
    /// </summary>
    public partial class RegisterView : UserControl
    {
        private readonly IApiClient _apiClient;
        private readonly ITokenManager _tokenManager;
        private readonly IUserDataProvider _userData;

        public RegisterView()
        {
            InitializeComponent();
            _apiClient = Services.GetService<IApiClient>();
            _tokenManager = Services.GetService<ITokenManager>();
            _userData = Services.GetService<IUserDataProvider>();
        }

        private async void CreateAccountButtonClick(object sender, RoutedEventArgs e)
        {
            var response = await _apiClient.PostAsync<LoginResponse>("/user/register",
                new RegisterRequest {Email = Email.Text, Password = Password.Password, Username = Username.Text});

            if (response.IsOk)
            {
                _tokenManager.Token = response.Value.Token;
                _userData.Username = response.Value.Username;
                _userData.Email = response.Value.Email;

                Window window = new MainWindow
                {
                    DataContext = new MainViewModel()
                };
                window.Show();

                Window.GetWindow(this)?.Close();
            }
            else
            {
                if (response.HttpStatusCode == HttpStatusCode.Conflict)
                {
                    ErrorMessage.Text = response.Error.Message;
                    ErrorMessage.Visibility = Visibility.Visible;
                }
            }
        }

        private void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            Window window = new LoginWindow
            {
                DataContext = new LoginViewModel()
            };
            window.Show();

            Window.GetWindow(this)?.Close();
        }

        private void Password_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is RegisterViewModel context)
            {
                context.Password = Password.Password;
            }
        }

        private void ConfirmPassword_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is RegisterViewModel context)
            {
                context.ConfirmPassword = ConfirmPassword.Password;
            }
        }
    }
}