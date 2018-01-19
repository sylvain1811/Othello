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

        private ContextPlayers contextPlayers;
        Board board;
        PawnBtn[,] tabPawnBtn;
        bool whiteTurn = false;
        public MainWindow()
        {
            InitializeComponent();
            contextPlayers = new ContextPlayers(0, 0);
            DataContext = contextPlayers;
            StartGame();
        }

        private void Pawn_Click(object sender, RoutedEventArgs e)
        {
            contextPlayers.TimerBlack.Stop();
            contextPlayers.TimerWhite.Stop();
            PawnBtn pawn = (PawnBtn)sender;
            int c = pawn.C;
            int l = pawn.L;

            if (board.PlayMove(c, l, whiteTurn))
            {
                // Coup joué
                pawn.Val = board.GetBoard()[c, l];
                whiteTurn = !whiteTurn;
            }
            else
            {
                // Impossible de jouer ce coup
                /*var whosPlaying = whiteTurn ? BLANC : NOIR;
                MessageBox.Show($"Coup impossible de {whosPlaying}");*/
            }
            if (board.IsFinished())
            {
                // Partie terminée
                contextPlayers.TimerBlack.Stop();
                contextPlayers.TimerWhite.Stop();
                var scoreBlack = board.GetBlackScore();
                var scoreWhite = board.GetWhiteScore();
                string winner;
                if (scoreBlack > scoreWhite)
                    winner = $"Gagnant : {NOIR}";
                else if (scoreWhite > scoreBlack)
                    winner = $"Gagnant : {BLANC}";
                else
                    winner = "Match nul";
                var m = MessageBox.Show($"Score joueur noir : {board.GetBlackScore()}\nScore joueur blanc : {board.GetWhiteScore()}\n{winner}\n\nSouhaitez-vous relancer une partie ?", "Partie terminée !", MessageBoxButton.YesNo);
                if (m == MessageBoxResult.Yes)
                    // Recommence une partie
                    StartGame();
                else
                    // Ferme l'application
                    Close();
                UpdateUI(true);
            }
            else
            {
                UpdateUI(false);
                if (whiteTurn)
                    contextPlayers.TimerWhite.Start();
                else
                    contextPlayers.TimerBlack.Start();
            }
        }

        private void UpdateUI(bool final)
        {
            bool shouldPass = UpdateUIBoard();

            string whoPass = null;
            if (shouldPass && !final)
            {
                whoPass = whiteTurn ? BLANC : NOIR;
                whiteTurn = !whiteTurn;
            }
            var whosPlaying = whiteTurn ? BLANC : NOIR;
            turn.Text = $"Tour du joueur {whosPlaying}";
            if (shouldPass && !final)
            {
                turn.Text += $"\nJoueur {whoPass} a passé son tour";
                // Recalcule les coups jouables par l'autre joueur
                UpdateUIBoard();
            }
            //blackScoreText.Text = $"{board.GetBlackScore()}";
            //whiteScoreText.Text = $"{board.GetWhiteScore()}";

            // Binding
            contextPlayers.BlackScore = $"{board.GetBlackScore()}";
            contextPlayers.WhiteScore = $"{board.GetWhiteScore()}";
        }

        private bool UpdateUIBoard()
        {
            var tabBoard = board.GetBoard();
            bool shouldPass = true;
            for (int c = 0; c < 8; c++)
            {
                for (int l = 0; l < 8; l++)
                {
                    if (board.IsPlayable(c, l, whiteTurn))
                    {
                        tabPawnBtn[c, l].IsPlayable = whiteTurn ? 0 : 1;
                        shouldPass = false;
                    }
                    else
                        tabPawnBtn[c, l].IsPlayable = -1;

                    tabPawnBtn[c, l].Val = tabBoard[c, l];
                }
            }
            return shouldPass;
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
                        BorderBrush = Brushes.White,
                        BorderThickness = new Thickness(1, 1, 0, 0)
                    };
                    if (c == 0)
                    {
                        border.BorderThickness = new Thickness(0, 1, 0, 0);
                        if (l == 0)
                        {
                            border.BorderThickness = new Thickness(0, 0, 0, 0);
                        }
                    }
                    else if (l == 0)
                        border.BorderThickness = new Thickness(1, 0, 0, 0);


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
            UpdateUI(false);
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
