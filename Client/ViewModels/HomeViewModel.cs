using Client.Models;
using Client.Service;

namespace Client.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly IUserDataProvider _userData;

        private string _username;

        public string Username
        {
            get => _username;
            set => _username = value;
        }

        public HomeViewModel()
        {
            _userData = Services.GetService<IUserDataProvider>();
            _username = _userData.Username;
        }
    }
}