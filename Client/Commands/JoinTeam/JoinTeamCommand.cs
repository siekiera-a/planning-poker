using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Text;
using System.Windows.Input;
using Client.Service;
using Client.ViewModels;
using Server.Dtos.Outgoing;

namespace Client.Commands.JoinTeam
{
    public class JoinTeamCommand : ICommand
    {
        private readonly JoinTeamModel _viewModel;


        public JoinTeamCommand(JoinTeamModel viewModel)
        {
            _viewModel = viewModel;
            _viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(_viewModel.TeamName);
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