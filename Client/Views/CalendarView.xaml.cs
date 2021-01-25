using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Client.ViewModels;

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
            MyCalendar.SelectedDate = DateTime.Today;
        }

        private async void MyCalendarSelectedDatesChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (DataContext is CalendarViewModel context)
            {
                await context.FetchMeetings();
            }
        }
    }
}