using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Client.Service;
using Server.Models.Server;

namespace Client.ViewModels.Game
{
    public class UserGameViewModel : ViewModelBase
    {
        private IGameManager _gameManager;
        private int _meetingId;

        private string _questionText;

        public string QuestionText
        {
            get => _questionText;
            set
            {
                value = _questionText;
                OnPropertyChanged(nameof(QuestionText));
            }
        }

        public void Description(ClientResponse value)
        {
            _questionText = value.Description;
        }

        

        public UserGameViewModel(int meetingId)
        {
            _meetingId = meetingId;
            _gameManager = Services.GetService<IGameManager>();
        }
    }
}