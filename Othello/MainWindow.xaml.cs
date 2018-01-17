using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Othello
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Button[,] tabButtun = new Button[8, 8];
            for (int c = 0; c < 8; c++)
            {
                for (int l = 0; l < 8; l++)
                {
                    Button btn = new Button
                    {
                        Content = $"btn ({c};{l})",
                        Background = Brushes.Transparent,
                        BorderBrush = Brushes.White
                    };
                    tabButtun[c, l] = btn;
                    Grid.SetColumn(btn, c);
                    Grid.SetRow(btn, l);
                    gridBoard.Children.Add(btn);
                }
            }
        }
    }
}
