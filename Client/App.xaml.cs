using System;
using System.Runtime.Serialization;
using System.Windows;
using Client.Models;
using Client.Service;
using Client.ViewModels;
using Client.Views.Login;
using Microsoft.Extensions.DependencyInjection;

namespace Client
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Window loginWindow = new LoginWindow
            {
                DataContext = new LoginViewModel()
            };
            loginWindow.Show();

            base.OnStartup(e);
        }

        public App()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

           var sp = serviceCollection.BuildServiceProvider();
           Services.Init(sp);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpClientFactory, HttpClientFactory>();
            services.AddSingleton<IApiClient, ApiClient>();
            services.AddSingleton<ITokenManager, TokenManager>();
            services.AddSingleton<IUserDataProvider, UserDataProvider>();
            services.AddSingleton<IGameManager, GameManager>();
        }
        
	}
}
