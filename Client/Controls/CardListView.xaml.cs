using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Client.Models;
using Client.Service;

namespace Client.Controls
{
    /// <summary>
    /// Interaction logic for CardListView.xaml
    /// </summary>
    public partial class CardListView : UserControl
    {
        private IGameManager _gameManager;
        private readonly List<CardIndex> _cardValues = new List<CardIndex>()
        {
            new CardIndex() {Name = "1", Value = 1},
            new CardIndex() {Name = "2", Value = 2},
            new CardIndex() {Name = "3", Value = 3},
            new CardIndex() {Name = "4", Value = 4},
            new CardIndex() {Name = "5", Value = 5},
            new CardIndex() {Name = "8", Value = 8},
            new CardIndex() {Name = "10", Value = 10},
            new CardIndex() {Name = "13", Value = 13},
            new CardIndex() {Name = "20", Value = 20},
            new CardIndex() {Name = "40", Value = 40},
            new CardIndex() {Name = "100", Value = 100},
            new CardIndex() {Name = "?", Value = -1},
        };

        public CardListView()
        {
            InitializeComponent();
            Loop();
            _gameManager = Services.GetService<IGameManager>();
        }

        public void Loop()
        {
            int i = 0;
            foreach (var x in Cards.Children.OfType<CardView>())
            {
                x.CardValue.Text = _cardValues[i].Name;
                x.Card = _cardValues[i];
                i++;
            }

        }

        public void CardClicked(object sender, MouseButtonEventArgs e)
        {
            _gameManager.Submit((sender as CardView).Card.Value);
            // MessageBox.Show((sender as CardView).Card.Name );
        }
    }
}