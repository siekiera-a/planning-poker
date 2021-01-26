using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Client.Service;
using Server.Models.Server;

namespace Client.ViewModels.Game
{
    public class UserGameViewModel : ViewModelBase
    {
        private IGameManager _gameManager;
        private int _meetingId;

        public string QuestionText
        {
            get;
            set;
        }
        
        public UserGameViewModel(int meetingId)
        {
            _meetingId = meetingId;
            _gameManager = Services.GetService<IGameManager>();
        }

        

    }
}