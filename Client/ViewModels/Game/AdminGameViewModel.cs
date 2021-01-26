using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Client.Service;

namespace Client.ViewModels.Game
{
    public class AdminGameViewModel : ViewModelBase
    {
        private IGameManager _gameManager;
        public ObservableCollection<Server.Models.Server.Client> Users { get; }
        public string QuestionText
        {
            get;
            set;
        }

        public async Task FetchMembers()
        {
            
        }

        public AdminGameViewModel()
        {
            _gameManager = Services.GetService<IGameManager>();
            _gameManager.TaskChangedEvent += (sender, e) => QuestionText = e.Description;
            Users = new ObservableCollection<Server.Models.Server.Client>();
        }
    }
}
