using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
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

        private void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            Window window = new MainWindow();
            window.DataContext = new MainViewModel();
            window.Show();

            Window.GetWindow(this)?.Close();
        }

        private void RegisterButtonClick(object sender, RoutedEventArgs e)
        {
            Window window = new RegisterWindow();
            window.Show();

            Window.GetWindow(this)?.Close();
        }
    }
}