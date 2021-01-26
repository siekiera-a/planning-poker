using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Client.ViewModels.Teams;

namespace Client.Views.Teams
{
    /// <summary>
    /// Interaction logic for ResultsView.xaml
    /// </summary>
    public partial class ResultsView : UserControl
    {
        public ResultsView()
        {
            InitializeComponent();
            // Loaded += FetchTasks;
        }

        // private async void FetchTasks(object sender, RoutedEventArgs e)
        // {
        //     if (DataContext is ResultsViewModel context)
        //     {
        //         await context.FetchTasks();
        //     }
        // }
    }
}