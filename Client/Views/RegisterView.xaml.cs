using System;
using System.Collections.Generic;
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
using Client.ViewModels;
using Client.Views.Login;
using Newtonsoft.Json;

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for RegisterView.xaml
    /// </summary>
    public partial class RegisterView : UserControl
    {
        class User
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string Username { get; set; }
        }

        public RegisterView()
        {
            InitializeComponent();
        }

        private async void CreateAccountButtonClick(object sender, RoutedEventArgs e)
        {

            User userData = new User { Email = Email.Text, Password = Password.Text, Username = Username.Text};

            var json = JsonConvert.SerializeObject(userData);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            
            var url = "https://localhost:5001/api/User/register";
            using var client = new HttpClient();
            
            var response = await client.PostAsync(url, data);

            string content = await response.Content.ReadAsStringAsync();

            Window window = new MainWindow
            {
                DataContext = new MainViewModel()
            };
            window.Show();

            Window.GetWindow(this)?.Close();
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
    }
}