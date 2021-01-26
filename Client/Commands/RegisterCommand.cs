using System;
using System.ComponentModel;
using System.Windows.Input;
using Client.ViewModels;

namespace Client.Commands
{
    class RegisterCommand : ICommand
    {
        private readonly RegisterViewModel _viewModel;

        public RegisterCommand(RegisterViewModel viewModel)
        {
            _viewModel = viewModel;
            _viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(_viewModel.Username) &&
                   !string.IsNullOrEmpty(_viewModel.Mail) &&
                   !string.IsNullOrEmpty(_viewModel.Password) &&
                   !string.IsNullOrEmpty(_viewModel.ConfirmPassword) &&
                   _viewModel.Password == _viewModel.ConfirmPassword;
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