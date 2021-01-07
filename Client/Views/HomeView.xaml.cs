using System.Collections.Generic;
using System.Windows.Controls;

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
            showRequest();
        }

        public void showRequest()
        {
            List<string> requests = new List<string>() {"zadanie1", "zadanie2", "zadanie3"};
            lista.ItemsSource = requests;
        }
    }
}