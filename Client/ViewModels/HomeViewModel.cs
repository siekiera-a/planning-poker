using Client.Models;

namespace Client.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly IUserDataProvider _userData;

        private string _username;

        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
            }
        }

        public HomeViewModel()
        {
            _userData = Services.GetService<IUserDataProvider>();
            _username = _userData.Username;
        }
    }
}
