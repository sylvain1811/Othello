using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
            StartGame();
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
                /*var whosPlaying = whiteTurn ? BLANC : NOIR;
                MessageBox.Show($"Coup impossible de {whosPlaying}");*/
            }
            if (board.IsFinished())
            {
                var scoreBlack = board.GetBlackScore();
                var scoreWhite = board.GetWhiteScore();
                string winner;
                if (scoreBlack > scoreWhite)
                    winner = $"Gagnant : {NOIR}";
                else if (scoreWhite > scoreBlack)
                    winner = $"Gagnant : {BLANC}";
                else
                    winner = "Match nul";
                var m = MessageBox.Show($"Score joueur noir : {board.GetBlackScore()}\nScore joueur blanc : {board.GetWhiteScore()}\n{winner}", "Partie terminée !", MessageBoxButton.YesNo);
                if (m == MessageBoxResult.Yes)
                    StartGame();
                else
                    Close();
            }
        }

        private void UpdateUIBoard()
        {
            var tabBoard = board.GetBoard();
            for (int c = 0; c < 8; c++)
            {
                for (int l = 0; l < 8; l++)
                {
                    if (board.IsPlayable(c, l, whiteTurn))
                        tabPawnBtn[c, l].IsPlayable = whiteTurn ? 0 : 1;
                    else
                        tabPawnBtn[c, l].IsPlayable = -1;

                    tabPawnBtn[c, l].Val = tabBoard[c, l];
                }
            }
            var whosPlaying = whiteTurn ? BLANC : NOIR;
            turn.Text = $"Tour du joueur {whosPlaying}";
            blackScoreText.Text = $"{board.GetBlackScore()}";
            whiteScoreText.Text = $"{board.GetWhiteScore()}";
        }

        private void StartGame()
        {
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
                        Style = resource,
                        Background = Brushes.White
                    };

                    pawnBtn.Click += Pawn_Click;
                    tabPawnBtn[c, l] = pawnBtn;

                    Border border = new Border
                    {
                        BorderThickness = new Thickness(0.7),
                        BorderBrush = Brushes.White
                    };

                    Grid grid = new Grid
                    {
                        Background = Brushes.Green
                    };
                    Viewbox viewbox = new Viewbox
                    {
                        Child = pawnBtn.Ellipse,
                        Margin = new Thickness(4)
                    };
                    grid.Children.Add(viewbox);
                    grid.Children.Add(pawnBtn);

                    border.Child = grid;
                    Grid.SetColumn(border, c + 1);
                    Grid.SetRow(border, l + 1);
                    gridBoard.Children.Add(border);
                }
            }
            AddHeader();
            UpdateUIBoard();
        }
        private void AddHeader()
        {
            var columns = "ABCDEFGH";
            var style = (Style)FindResource("headerStyle");

            for (int c = 1; c < 9; c++)
            {
                AddTextBlock(c, 0, columns, style, false);
                AddTextBlock(c, 9, columns, style, false);
            }
            for (int l = 1; l < 9; l++)
            {
                AddTextBlock(0, l, columns, style, true);
                AddTextBlock(9, l, columns, style, true);
            }
        }

        private void AddTextBlock(int c, int l, string columns, Style style, bool isLine)
        {
            TextBlock textBlock = new TextBlock
            {
                Style = style,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            if (isLine)
            {
                textBlock.Text = "" + l;
            }
            else
            {
                textBlock.Text = "" + columns[c - 1];
            }
            Grid.SetRow(textBlock, l);
            Grid.SetColumn(textBlock, c);

            gridBoard.Children.Add(textBlock);
        }

    }
}
