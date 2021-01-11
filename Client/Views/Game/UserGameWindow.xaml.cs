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
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace Client.Views.Game
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow :Window
    {
        public GameWindow()
        {
            InitializeComponent();
        }

        private async void SendAnswerButtonClickAsync(object sender, RoutedEventArgs e)
        {
            // MahApps.Metro.Controls.Dialogs.MessageDialogResult result =
            //     await this.ShowMessageAsync("Information", "This is simple dialog");
            
        }
    }
}
