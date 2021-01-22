using System;
using System.Collections.Generic;
using System.IO.Compression;
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
using Client.State.Navigators;
using Client.ViewModels;
using Client.Views.Login;
using Client.Views.Game;
using Newtonsoft.Json;

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        class User
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public LoginView()
        {
            InitializeComponent();
        }

        private async void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            User userData = new User {Email = Email.Text, Password = Password.Password};

            var json = JsonConvert.SerializeObject(userData);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var url = "https://localhost:5001/api/User/login";
            using var client = new HttpClient();

            var response = await client.PostAsync(url, data);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();

                Window window = new MainWindow
                {
                    DataContext = new MainViewModel()
                };
                window.Show();

                Window.GetWindow(this)?.Close();

                GameWindow gameWindow = new GameWindow();
                gameWindow.ShowDialog();
            }
        }

        private async void RegisterButtonClick(object sender, RoutedEventArgs e)
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