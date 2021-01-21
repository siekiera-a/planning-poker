using System.Windows;
using Client.ViewModels;
using Client.Views.Login;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LogoutButtonClick(object sender, RoutedEventArgs e)
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