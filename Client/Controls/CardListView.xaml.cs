using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Client.Models;

namespace Client.Controls
{
    /// <summary>
    /// Interaction logic for CardListView.xaml
    /// </summary>
    public partial class CardListView : UserControl
    {
        private readonly List<CardIndex> _cardValues = new List<CardIndex>()
        {
            new CardIndex() {Name = "0", Value = 0},
            new CardIndex() {Name = "1/2", Value = 0.5},
            new CardIndex() {Name = "1", Value = 1},
            new CardIndex() {Name = "2", Value = 2},
            new CardIndex() {Name = "3", Value = 3},
            new CardIndex() {Name = "5", Value = 5},
            new CardIndex() {Name = "8", Value = 8},
            new CardIndex() {Name = "13", Value = 13},
            new CardIndex() {Name = "20", Value = 20},
            new CardIndex() {Name = "40", Value = 40},
            new CardIndex() {Name = "100", Value = 100},
            new CardIndex() {Name = "?", Value = double.NaN},
        };

        public CardListView()
        {
            InitializeComponent();
            Loop();
            
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

        private void CardClicked(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show((sender as CardView).Card.Name );
        }
    }
}