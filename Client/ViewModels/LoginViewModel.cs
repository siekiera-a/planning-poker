using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Windows.Input;
using Client.Commands;
using Client.State.Navigators;

namespace Client.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private string _mail;

        public string Mail
        {
            get
            {
                return _mail;
            }
            set
            {
                _mail = value;
                OnPropertyChanged(nameof(Mail));
            }
        }

        private string _password;

        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new LoginCommand(this);
        }

    }
}
