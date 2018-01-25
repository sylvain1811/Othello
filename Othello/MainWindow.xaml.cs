using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
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
        CellButton[,] cells;
        bool whiteTurn = false;
        bool playAgainstIA;

        public MainWindow()
        {
            InitializeComponent();
            StartGame();
        }

        /// <summary>
        /// Un joueur tente de jouer un coup. Joue le coup et change de tour si possible.
        /// Si l'autre joueur doit passer, reviens au premier joueur.
        /// Si la partie est terminée, affichage des score et du gagnant.
        /// </summary>
        private void Pawn_Click(object sender, RoutedEventArgs e)
        {
            contextPlayers.Timer.Stop();
            CellButton cellButton = (CellButton)sender;
            int c = cellButton.C;
            int l = cellButton.L;


            if (board.PlayMove(c, l, whiteTurn))
            {
                // Coup jouable et joué
                cellButton.Val = board.GetBoard()[c, l];
                whiteTurn = !whiteTurn;
                var newWhiteTurn = whiteTurn;

                CheckFinished();

                // L'IA joue que si elle n'a pas passé son tour (donc whiteTurn n'a pas été modifié dans CheckFinished().)
                if (playAgainstIA && newWhiteTurn == whiteTurn)
                {
                    PlayIAMove();
                    CheckFinished();
                }
            }
            else
            {
                // Impossible de jouer ce coup
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void CheckFinished()
        {
            if (board.IsFinished())
            {
                // Partie terminée
                var scoreBlack = board.GetBlackScore();
                var scoreWhite = board.GetWhiteScore();
                UpdateUI(true);

                // Affichage du gagnant
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
                // Partie pas terminée
                UpdateUI(false);
                contextPlayers.WhiteTurn = whiteTurn;
                contextPlayers.Timer.Start();
            }
        }

        /// <summary>
        /// Met à jour les scores et dit qui doit joueur. Indique si un joueur a passer son tour.
        /// </summary>
        /// <param name="final">
        /// True si la partie est terminée. Evite d'indiquer qu'un joueur a passer son tour alors que la partie est terminée.
        /// </param>
        private void UpdateUI(bool final)
        {
            bool shouldPass = UpdateUIBoard();

            string whoPass = "";
            if (shouldPass && !final)
            {
                // Un joueur a passé, on doit l'indiquer
                whoPass = whiteTurn ? BLANC : NOIR;
                // C'est donc plus son tour, on inverse le tour.
                whiteTurn = !whiteTurn;
                whoPass = $"\nJoueur {whoPass} a passé son tour";
                // Recalcule les coups jouables par l'autre joueur
                UpdateUIBoard();
            }

            // Affichage du tour (avec indication si un joueur a passé)
            var whosPlaying = whiteTurn ? BLANC : NOIR;
            turn.Text = $"Tour du joueur {whosPlaying}{whoPass}";
            if (playAgainstIA && whiteTurn)
                turn.Text += $" (Ordinateur)";

            // Binding
            contextPlayers.BlackScore = $"{board.GetBlackScore()}";
            contextPlayers.WhiteScore = $"{board.GetWhiteScore()}";
        }

        /// <summary>
        /// Met à jour le plateau et check si un joueur doit passer son tour.
        /// </summary>
        /// <returns>True si le joueur doit passer son tour.</returns>
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
                        cells[c, l].IsPlayable = whiteTurn ? 0 : 1;
                        // Au moins un coup est jouable, pas besoin de passer son tour
                        shouldPass = false;
                    }
                    else
                        cells[c, l].IsPlayable = -1;

                    cells[c, l].Val = tabBoard[c, l];
                }
            }
            return shouldPass;
        }

        /// <summary>
        /// Commence ou recommence une partie.
        /// </summary>
        private void StartGame()
        {
            UpdateTitle();

            // Le joueur noir commence toujours
            whiteTurn = false;
            // Création du contexte pour les infos des joueurs
            contextPlayers = new ContextPlayers(2, 2);
            // On bind les infos au DataContext de la fenêtre.
            DataContext = contextPlayers;
            // Nouveau plateau de jeu
            board = new Board();
            cells = new CellButton[8, 8];
            // Style des boutons (pions)
            Style resource = (Style)FindResource("PawnStyle");
            var tabBoard = board.GetBoard();
            for (int c = 0; c < 8; c++)
            {
                for (int l = 0; l < 8; l++)
                {
                    // Création et ajout du pion dans le plateau de jeu
                    var val = tabBoard[c, l];
                    CellButton pawnBtn = new CellButton(c, l, val)
                    {
                        Style = resource
                    };
                    pawnBtn.Click += Pawn_Click;
                    cells[c, l] = pawnBtn;

                    // Ajout graphique du pion : Border(Grid(bouton transparent, ellipse visible))
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
            // Ajout des en-têtes au plateau (A-H dans les colonnes, 1-8 pour les lignes)
            AddHeader();
            // Refresh de la GUI
            UpdateUI(false);

        }

        /// <summary>
        /// Met à jour le titre de la fenêtre.
        /// </summary>
        private void UpdateTitle()
        {
            if (playAgainstIA)
                Title = "Othello - Partie contre l'ordinateur";
            else
                Title = "Othello - Partie joueur contre joueur";
        }

        /// <summary>
        /// Ajout des en-têtes au plateau (A-H dans les colonnes, 1-8 pour les lignes).
        /// </summary>
        private void AddHeader()
        {
            var columns = "ABCDEFGH";
            var style = (Style)FindResource("HeaderStyle");

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

        // Ajout d'une en-tête dans le plateau
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

        /// <summary>
        /// Ouvre le navigateur par défaut avec les règles du jeu.
        /// </summary>
        private void MenuItemRules_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.ffothello.org/othello/regles-du-jeu/");
        }

        /// <summary>
        /// Affiche un message "About" de l'application avec auteurs, etc.
        /// </summary>
        private void MenuItemAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Jeu de l'Othello. \nProjet .NET\n25 janvier 2018\nAuteurs: \nRenaud Sylvain, Costa Pedro");
        }

        /// <summary>
        /// Relance une nouvelle partie
        /// </summary>
        private void MenuItemNewGame_Click(object sender, RoutedEventArgs e)
        {
            playAgainstIA = false;
            StartGame();
        }

        /// <summary>
        /// Sauvegarde de la partie dans un fichier. 
        /// Sauvegarde de l'état du plateau et les temps de réfléxion des joueurs.
        /// </summary>
        private void MenuItemSave_Click(object sender, RoutedEventArgs e)
        {
            // Ouverture d'une boite de dialogue pour demander où sauver le fichier.
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                RestoreDirectory = true,
                Filter = "Game files(*.game)|*.game"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                // L'objet game est sérializable. Il contient le plateau de jeu (int[8,8]) et les temps de réfléxion des joueurs.
                Game game = new Game(board.GetBoard(), whiteTurn, contextPlayers.ElapsedWhite, contextPlayers.ElapsedBlack, playAgainstIA);
                BinaryFormatter formatter = new BinaryFormatter();
                Stream stream = null;

                try
                {
                    stream = saveFileDialog.OpenFile();

                    // Sérialisation de l'objet game dans le ficher
                    formatter.Serialize(stream, game);
                    stream.Flush();
                }
                finally
                {
                    if (stream != null)
                        stream.Close();
                }
            }
        }

        /// <summary>
        /// Charge une partie précédemment sauvegardées.
        /// </summary>
        private void MenuItemLoad_Click(object sender, RoutedEventArgs e)
        {
            // Ouverture d'une boite de dialogue pour demander quelle partie charger.
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                RestoreDirectory = true,
                Filter = "Game files(*.game)|*.game"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                BinaryFormatter formatter = new BinaryFormatter();
                Stream stream = null;

                if ((stream = openFileDialog.OpenFile()) != null)
                {
                    try
                    {
                        // Recréaction d'un objet game à partir du fichier de sauvegarde
                        Game game = (Game)formatter.Deserialize(stream);

                        // Charger les données de game dans la partie actuelle
                        board = new Board(game.Board);
                        whiteTurn = game.WhiteTurn;
                        playAgainstIA = game.PlayAgainstIA;
                        contextPlayers.ElapsedBlack = game.ElapsedBlack;
                        contextPlayers.ElapsedWhite = game.ElapsedWhite;
                        UpdateUI(false);
                        UpdateTitle();
                    }
                    catch
                    {
                        // Impossible de charger la partie
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

        /// <summary>
        /// Démarre une partie contre l'IA. l'IA joue les blancs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemPlayAgainstIA_Click(object sender, RoutedEventArgs e)
        {
            playAgainstIA = true;
            UpdateTitle();
            StartGame();
        }

        /// <summary>
        /// Demande à l'IA de jouer un coup.
        /// Méthode asynchrone pour permettre l'affichage du coup du joueur avant que l'IA joue.
        /// </summary>
        private void PlayIAMove()
        {
            int difficulty = 2;

            // Le joueur humain est bloqué
            foreach (var cell in cells)
                cell.IsEnabled = false;

            // Copie des variables utilisées dans un BackgroundWorker pour éviter la concurrence
            Board copyBoard = new Board(board);
            bool copyWhiteTurn = whiteTurn;
            int copyDifficulty = difficulty;
            Tuple<int, int> move = null;

            BackgroundWorker bgWorker = new BackgroundWorker();

            // On fait réfléchir l'IA dans un BackgroudWorker
            bgWorker.DoWork += (sender, ev) =>
            {
                Thread.Sleep(1000);
                move = copyBoard.GetNextMove(copyBoard.GetBoard(), copyDifficulty, copyWhiteTurn);
            };

            // Quand elle trouve un coup, on met à jour le board
            bgWorker.RunWorkerCompleted += (sender, ev) =>
            {
                int c = move.Item1;
                int l = move.Item2;

                // Vérifie que l'IA n'a pas passé son tour
                if (c >= 0 && l >= 0)
                {
                    if (board.PlayMove(c, l, whiteTurn))
                    {
                        // Coup jouable et joué
                        whiteTurn = !whiteTurn;
                    }
                    else
                    {
                        // Impossible de jouer ce coup
                    }
                }

                var newWhiteTurn = whiteTurn;

                CheckFinished();

                if (newWhiteTurn != whiteTurn)
                {
                    // Le joueur a passé son tour (whiteTurn a été modifié dans CheckFinished())
                    PlayIAMove();
                }

                // Débloque le joueur humain
                foreach (var cell in cells)
                    cell.IsEnabled = true;
            };

            // On lance le background worker
            bgWorker.RunWorkerAsync();
        }
    }
}
