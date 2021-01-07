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
            if (EmailTextBox.Text.Length == 0)
            {
                EmailTextBox.Focus();
            }
            else if (IsValid(EmailTextBox.Text))
            {
                EmailTextBox.Select(0, EmailTextBox.Text.Length);
                EmailTextBox.Focus();
            }
            else
            {
                string userName = UserNameTextBox.Text;
                string email = EmailTextBox.Text;
                string password = PasswordBoxPassword.Password;

            }
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
    }
}
