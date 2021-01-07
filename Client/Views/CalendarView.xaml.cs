using System;
using System.Collections.Generic;
using System.Windows.Controls;

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

        private void MyCalendarSelectedDatesChanged(object? sender, SelectionChangedEventArgs e)
        {
            List<string> meetingsList = new List<string>();
            MyDate.Text = MyCalendar.SelectedDate.Value.ToShortDateString();
            meetingsList.Add(MyCalendar.SelectedDate.ToString());
            DayMeetings.ItemsSource = meetingsList;
        }
    }
}