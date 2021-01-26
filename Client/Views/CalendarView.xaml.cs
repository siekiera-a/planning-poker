using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Client.ViewModels;
using Server.Dtos.Outgoing;

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for CalendarView.xaml
    /// </summary>
    public partial class CalendarView : UserControl
    {
        public CalendarView()
        {
            InitializeComponent();
        }

        private async void MyCalendarSelectedDatesChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (DataContext is CalendarViewModel context)
            {
                await context.FetchMeetings();
            }
        }

        private void JoinMeetingButtonClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is CalendarViewModel context)
            {
                context.JoinMeeting(MeetingList.SelectedItem as MeetingDetailsResponse);
            }
        }
    }
}