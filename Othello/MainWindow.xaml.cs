using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;
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
                UpdateUI(true);
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
            whiteTurn = false;
            contextPlayers = new ContextPlayers(0, 0);
            DataContext = contextPlayers;
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

        private void MenuItemAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Jeu de l'Othello. \nProjet .NET\nAuteurs: \nRenaud Sylvain, Costa Pedro");
        }

        private void MenuItemNewGame_Click(object sender, RoutedEventArgs e)
        {
            StartGame();
        }

        private void MenuItemRules_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.ffothello.org/othello/regles-du-jeu/");
        }

        private void MenuItemSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                RestoreDirectory = true,
                Filter = "All files(*.*)|*.*"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                Game game = new Game(board.GetBoard(), whiteTurn, contextPlayers.ElapsedWhite, contextPlayers.ElapsedBlack);
                BinaryFormatter formatter = new BinaryFormatter();
                Stream stream = null;

                try
                {
                    //stream = new FileStream(, FileMode.Create, FileAccess.Write);
                    stream = saveFileDialog.OpenFile();
                    formatter.Serialize(stream, game);
                    stream.Flush();
                }
                catch { }
                finally
                {
                    if (stream != null)
                        stream.Close();
                }
            }


            // string path = "data.bin";

        }

        private void MenuItemLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                RestoreDirectory = true,
                Filter = "All files(*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                //string path = "data.bin";
                BinaryFormatter formatter = new BinaryFormatter();
                Stream stream = null;

                if ((stream = openFileDialog.OpenFile()) != null)
                {
                    try
                    {
                        //stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                        Game game = (Game)formatter.Deserialize(stream);
                        //contextPlayers = game.ContextPlayers;
                        board = new Board(game.Board);
                        whiteTurn = game.WhiteTurn;
                        UpdateUI(false);
                        contextPlayers.ElapsedBlack = game.ElapsedBlack;
                        contextPlayers.ElapsedWhite = game.ElapsedWhite;
                    }
                    catch
                    {
                        MessageBox.Show(
                            "Un problème est survenu lors du chargement de la partie, aucun chargement n'a été effectué.",
                            "Erreur de chargement", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    finally
                    {
                        if (stream != null)
                            stream.Close();
                    }
                }
            }

        }
    }
}
