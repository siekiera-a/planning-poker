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
using Server.Dtos.Outgoing;
using Server.Models.Dapper;

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
            Loaded += FetchTeams;
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

        private async void FetchTeams(object sender, RoutedEventArgs e)
        {
            if (DataContext is CreateViewModel context)
            {
                await context.FetchTeams();
            }
        }

        private async void Teams_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is CreateViewModel context)
            {
                if (e.AddedItems.Count > 0)
                {
                    if (e.AddedItems[0] is TeamResponse selectedItem)
                    {
                        await context.FetchMembers(selectedItem.Id);
                    }
                }
            }
        }

        private void Members_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is CreateViewModel context)
            {
                if (e.AddedItems.Count > 0)
                {
                    if (e.AddedItems[0] is User user)
                    {
                        context.FetchMembersToMeeting(user.Id);
                    }
                }
            }
        }

        private void DeleteSelectedMembers(object sender, RoutedEventArgs e)
        {
            if (DataContext is CreateViewModel context)
            {
                context.DeleteSelectedMembers(MemberList.SelectedItems);
            }
        }

        private void CancelCreatingMeeting(object sender, RoutedEventArgs e)
        {
            if (DataContext is CreateViewModel context)
            {
                context.CancelCreating();
            }
        }
    }
}