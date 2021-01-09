using System.Windows;
using Client.ViewModels;
using Client.Views.Login;

namespace Client
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
        protected override void OnStartup(StartupEventArgs e)
        {
            Window loginWindow = new LoginWindow();
            loginWindow.Show();
            

            base.OnStartup(e);
        }
	}
}
