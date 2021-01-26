using System;
using System.ComponentModel;
using System.Windows.Input;
using Client.ViewModels;

namespace Client.Commands
{
    class LoginCommand : ICommand
    {
        private readonly LoginViewModel _viewModel;

        public LoginCommand(LoginViewModel viewModel)
        {
            _viewModel = viewModel;
            _viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(_viewModel.Mail) && !string.IsNullOrEmpty(_viewModel.Password);
        }

        public void Execute(object parameter)
        {
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}