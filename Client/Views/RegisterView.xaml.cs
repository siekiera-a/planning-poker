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
using Client.ViewModels;
using Client.Views.Login;
using Newtonsoft.Json;
using Server.Dtos.Outgoing;

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for RegisterView.xaml
    /// </summary>
    public partial class RegisterView : UserControl
    {
        private readonly IApiClient _apiClient;

        public RegisterView()
        {
            InitializeComponent();
            _apiClient = Services.GetService<IApiClient>();
        }

        private async void CreateAccountButtonClick(object sender, RoutedEventArgs e)
        {
            // User userData = new User
            //     {Email = Email.Text, Password = Password.Password, Username = Username.Text};
            //
            // var response = await _apiClient.PostAsync<RegisterResponse>("/User/register", userData);
            //
            // if (response.IsOk)
            // {
            //     Window window = new MainWindow
            //     {
            //         DataContext = new MainViewModel()
            //     };
            //     window.Show();
            //
            //     Window.GetWindow(this)?.Close();
            // }
            // else
            // {
            //     if (response.HttpStatusCode == HttpStatusCode.Conflict)
            //     {
            //         ErrorMessage.Text = response.Error.Message;
            //         ErrorMessage.Visibility = Visibility.Visible;
            //     }
            // }
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