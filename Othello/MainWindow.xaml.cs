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
using OthelloIA_G3;

namespace Othello
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string BLANC = "blanc";
        private static string NOIR = "noir";

        Board board;
        PawnBtn[,] tabPawnBtn;
        bool whiteTurn = false;
        public MainWindow()
        {
            InitializeComponent();
            board = new Board();
            tabPawnBtn = new PawnBtn[8, 8];
            Style resource = (Style)FindResource("pawnStyle");
            for (int c = 0; c < 8; c++)
            {
                for (int l = 0; l < 8; l++)
                {
                    var val = board.GetBoard()[c, l];
                    PawnBtn pawnBtn = new PawnBtn(c, l, val)
                    {
                        Style = resource
                    };

                    pawnBtn.Click += Pawn_Click;
                    Grid.SetColumn(pawnBtn, c + 1);
                    Grid.SetRow(pawnBtn, l + 1);
                    gridBoard.Children.Add(pawnBtn);
                    tabPawnBtn[c, l] = pawnBtn;
                }
            }
        }

        private void Pawn_Click(object sender, RoutedEventArgs e)
        {
            PawnBtn pawn = (PawnBtn)sender;
            int c = pawn.C;
            int l = pawn.L;

            if (board.PlayMove(c, l, whiteTurn))
            {
                pawn.Val = board.GetBoard()[c, l];
                whiteTurn = !whiteTurn;
                UpdateUIBoard();
            }
            else
            {
                var whosPlaying = whiteTurn ? BLANC : NOIR;
                MessageBox.Show($"Coup impossible de {whosPlaying}");
            }
        }

        private void UpdateUIBoard()
        {
            var tabBoard = board.GetBoard();
            for (int c = 0; c < 8; c++)
            {
                for (int l = 0; l < 8; l++)
                {
                    tabPawnBtn[c, l].Val = tabBoard[c, l];
                }
            }
            var whosPlaying = whiteTurn ? BLANC : NOIR;
            turn.Text = $"Tour de {whosPlaying}";
            //throw new NotImplementedException();
        }
    }
}
