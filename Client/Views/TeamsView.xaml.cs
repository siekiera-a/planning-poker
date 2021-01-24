using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Client.Models;
using Server.Dtos.Outgoing;

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for TeamsView.xaml
    /// </summary>
    public partial class TeamsView : UserControl
    {
        private readonly IApiClient _apiClient;
        private int _teamId;

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
            var response = await _apiClient.GetAsyncAuth<CodeResponse>($"/team/{_teamId}/join-code");

            if (response.IsOk)
            {
                JoinCode.Text = response.Value.Code;
            }
            else
            {
                if (response.HttpStatusCode == HttpStatusCode.Forbidden)
                {
                    MessageBox.Show(response.Error.Message);
                }
            }
        }

        private async void FetchTeams(object sender, RoutedEventArgs e)
        {
            var response = await _apiClient.GetAsyncAuth<TeamResponse[]>("/team");

            if (response.IsOk)
            {
                ComboBox.ItemsSource = response.Value;
                ComboBox.Items.Refresh();
                ComboBox.DisplayMemberPath = "Name";
                ComboBox.SelectedValuePath = "Id";
            }
        }

        private async void ComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems[0] is TeamResponse selectedItem)
            {
                _teamId = selectedItem.Id;
                var members = await _apiClient.GetAsyncAuth<GetMembersResponse>($"/team/{_teamId}/members");

                if (members.IsOk)
                {
                    Members.ItemsSource = members.Value.Members;
                    Members.DisplayMemberPath = "Name";
                    Members.SelectedValuePath = "Id";
                    Members.Items.Refresh();
                }
            }
        }
    }
}