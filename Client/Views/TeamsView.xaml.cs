using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Client.Models;
using Client.Service;
using Client.ViewModels;
using Server.Dtos.Outgoing;

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for TeamsView.xaml
    /// </summary>
    public partial class TeamsView : UserControl
    {
        private readonly IApiClient _apiClient;

        public TeamsView()
        {
            InitializeComponent();
            _apiClient = Services.GetService<IApiClient>();
            Loaded += FetchTeams;
        }

        private void AddMemberByEmail(object sender, RoutedEventArgs e)
        {
        }

        private async void GenerateCodeButtonClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is TeamsViewModel context)
            {
                await context.GenerateCode();
                JoinCode.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
            }
        }

        private async void FetchTeams(object sender, RoutedEventArgs e)
        {
            if (DataContext is TeamsViewModel context)
            {
                await context.FetchTeams();
            }
        }

        private async void ComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is TeamsViewModel context)
            {
                if (e.AddedItems[0] is TeamResponse selectedItem)
                {
                    await context.FetchMembers(selectedItem.Id);
                }
            }
        }
    }
}