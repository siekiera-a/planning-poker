using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Win32;

namespace Client.Views.Teams
{
    /// <summary>
    /// Interaction logic for CreateView.xaml
    /// </summary>
    public partial class CreateView : UserControl
    {
        public CreateView()
        {
            InitializeComponent();
        }

        private void AddTaskButtonClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is CreateViewModel context)
            {
                context.AddTask();
            }
        }

        private void DeleteTaskButtonClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is CreateViewModel context &&
                TaskListBox.SelectedItem != null)
            {
                context.DeleteTask(TaskListBox.SelectedItem.ToString());
            }
        }
    }
}