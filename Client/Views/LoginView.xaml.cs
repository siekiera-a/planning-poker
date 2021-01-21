using System;
using System.Collections.Generic;
using System.IO.Compression;
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

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private async void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            Window window = new MainWindow
            {
                DataContext = new MainViewModel()
            };
            window.Show();

            Window.GetWindow(this)?.Close();

            GameWindow gameWindow = new GameWindow();
            gameWindow.ShowDialog();

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
    }
}