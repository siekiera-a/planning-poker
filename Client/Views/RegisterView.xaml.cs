using System;
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

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for RegisterView.xaml
    /// </summary>
    public partial class RegisterView : UserControl
    {
        public RegisterView()
        {
            InitializeComponent();
        }

        // private void SignInButtonClick(object sender, RoutedEventArgs e)
        // {
        //     this.Navigate(new LoginView());
        // }
        //
        // private void Navigate(LoginView loginView)
        // {
        //     throw new NotImplementedException();
        // }

        private void CreateAccountButtonClick(object sender, RoutedEventArgs e)
        {
            // if (EmailTextBox.Text.Length == 0)
            // {
            //     EmailTextBox.Focus();
            // }
            // else if (IsValid(EmailTextBox.Text))
            // {
            //     EmailTextBox.Select(0, EmailTextBox.Text.Length);
            //     EmailTextBox.Focus();
            // }
            // else
            // {
            //     string userName = UserNameTextBox.Text;
            //     string email = EmailTextBox.Text;
            //     string password = PasswordBoxPassword.Password;
            //
            // }

            Window window = new MainWindow();
            window.DataContext = new MainViewModel();
            window.Show();

            Window.GetWindow(this)?.Close();

        }

        private bool IsValid(string emailAdress)
        {
            try
            {
                MailAddress mail = new MailAddress(emailAdress);
                return true;
            }
            catch (FormatException e)
            {
                return false;
            }
        }

        private void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            Window window = new LoginWindow();
            window.Show();

            Window.GetWindow(this)?.Close();
        }
    }
}
