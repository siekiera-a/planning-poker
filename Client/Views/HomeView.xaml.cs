using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Client.Models;
using Client.ViewModels;

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {

        public HomeView()
        {
            InitializeComponent();
            Loaded += FetchTasks;
        }

        private async void FetchTasks(object sender, RoutedEventArgs e)
        {
            if (DataContext is HomeViewModel context)
            {
                await context.FetchTasks();
            }
        }

    }
}