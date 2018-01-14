using System;
using System.Collections.Generic;

namespace OthelloIA_G3
{
    public class Board : IPlayable.IPlayable
    {
        // ATTRIBUTS
        private int[,] board;
        private static string NAME = "Costa_Renaud";

        // CONSTRUCTEURS

        /// <summary>
        /// Construcuteur sans paramètres.
        /// </summary>
        public Board()
        {
            // Init board
            board = new int[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    board[i, j] = -1;
                }
            }

            // Place first pions
            board[3, 3] = board[4, 4] = 0; // White
            board[4, 3] = board[3, 4] = 1; // Black
        }

        public bool IsFinished()
        {
            return TreeNode.Final(this);
        }

        public Board(int[,] game)
        {
            // Init board
            board = new int[8, 8];

            // Copy source values            
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    board[i, j] = game[i, j];
                }
            }
        }

        /// <summary>
        /// Constructeur de copie.
        /// </summary>
        /// <param name="source"></param>
        public Board(Board source) : this(source.GetBoard()) { }

        // IMPLEMENTS

        /// <summary>
        /// Retourne le nom de l'IA.
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return NAME;
        }

        /// <summary>
        /// Retourne le tableau de int représentant le board.
        /// </summary>
        /// <returns></returns>
        public int[,] GetBoard()
        {
            return board;
        }

        /// <summary>
        /// Calcul le score des pions noir.
        /// </summary>
        /// <returns></returns>
        public int GetBlackScore()
        {
            int score = 0;
            foreach (int i in board)
            {
                if (i == 1)
                    score++;
            }
            // Console.WriteLine(score);
            return score;
        }

        /// <summary>
        /// Calcul le score des pions blancs.
        /// </summary>
        /// <returns></returns>
        public int GetWhiteScore()
        {
            int score = 0;
            foreach (int i in board)
            {
                if (i == 0)
                    score++;
            }
            // Console.WriteLine(score);
            return score;
        }

        /// <summary>
        /// Retourne le prochain coup que l'IA va jouer.
        /// </summary>
        /// <param name="game">game board</param>
        /// <param name="level">niveau de profondeur</param>
        /// <param name="whiteTurn">qui joue ?</param>
        /// <returns></returns>
        public Tuple<int, int> GetNextMove(int[,] game, int level, bool whiteTurn)
        {
            int myScore = whiteTurn ? GetWhiteScore() : GetBlackScore();

            TreeNode root = new TreeNode(this, TreeNode.EType.MAX, whiteTurn);

            //Console.WriteLine(root.ToString());

            var bestOp = AlphaBeta(root, level, whiteTurn, (int)TreeNode.EType.MAX, -int.MaxValue).Item2;

            if (bestOp == null)
            {
                bestOp = new Cell(-1, -1);

            }
            return new Tuple<int, int>(bestOp.C, bestOp.L);
        }

        /// <summary>
        /// Retourne si oui ou non un coup est jouable par un joueur d'une certaine couleur.
        /// </summary>
        /// <param name="column"></param>
        /// <param name="line"></param>
        /// <param name="isWhite">couleur du pion</param>
        /// <returns></returns>
        public bool IsPlayable(int column, int line, bool isWhite)
        {
            return CheckOrPlay(column, line, isWhite, true);
        }

        /// <summary>
        /// Joue un coup et met à jour le plateau de jeu.
        /// </summary>
        /// <param name="column"></param>
        /// <param name="line"></param>
        /// <param name="isWhite"></param>
        /// <returns></returns>
        public bool PlayMove(int column, int line, bool isWhite)
        {
            return CheckOrPlay(column, line, isWhite, false);
        }

        // PRIVATE

        /// <summary>
        /// Méthode générique appelée par IsPlayable et PlayMove.
        /// </summary>
        /// <param name="column"></param>
        /// <param name="line"></param>
        /// <param name="isWhite"></param>
        /// <param name="checkOnly">True si l'on veut juste vérifier qu'un coup est jouable. 
        /// False si on veut jouer le coup et mettre à jour le plateau.</param>
        /// <returns></returns>
        private bool CheckOrPlay(int column, int line, bool isWhite, bool checkOnly)
        {
            List<Pawn> toFlip = new List<Pawn>();
            // Check case is empty
            if (board[column, line] > -1)
            {
                return false;
            }

            bool playable = false;

            // Check voisin d'autre couleurs
            int colorOpponent = isWhite ? 1 : 0;
            int colorPlayer = isWhite ? 0 : 1;

            //  (c; l) 
            //  -----------------------
            //  (-1;-1)  (0;-1)  (1;-1)
            //  (-1; 0)  (0; 0)  (1; 0)
            //  (-1; 1)  (0; 1)  (1; 1)

            // Init start/end for line and column
            int cstart = column <= 0 ? 0 : -1;
            int cend = column >= 7 ? 0 : 1;
            int lstart = line <= 0 ? 0 : -1;
            int lend = line >= 7 ? 0 : 1;

            //Console.WriteLine($"lstart : {lstart}\nlend : {lend}\ncstart : {cstart}\ncend : {cend}");

            // Parcours des voisins
            for (int c = cstart; c <= cend; c++)
            {
                for (int l = lstart; l <= lend; l++)
                {
                    // Check seulement si la case n'est pas nous-même
                    if (!(c == 0 && l == 0))
                    {
                        if (board[column + c, line + l] == colorOpponent)
                        {
                            // Check si il y a un pion de notre couleur dans cette ligne/colonne/diagonale

                            List<Pawn> opponentPion = new List<Pawn>
                            {
                                new Pawn(column + c, line + l, colorOpponent)
                            };

                            // Console.WriteLine($"Color ({column + c};{line + l}): " + board[column + c, line + l]);

                            int copyC = c + c; // c dans [-1;1]
                            int copyL = l + l; // l dans [-1;1]

                            //while (!playable && (column + copyC < 8 && line + copyL < 8 && column + copyC >= 0 && line + copyL >= 0))
                            while (column + copyC < 8 && line + copyL < 8 && column + copyC >= 0 && line + copyL >= 0)
                            {
                                int currentCell = board[column + copyC, line + copyL];
                                if (currentCell == colorPlayer)
                                {
                                    // Un pion a moi
                                    if (checkOnly)
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        playable = true;
                                        foreach (var tuple in opponentPion)
                                        {
                                            //board[tuple.Item1, tuple.Item2] = colorPlayer;
                                            toFlip.Add(tuple);
                                        }
                                        break;
                                    }
                                }
                                else if (currentCell < 0)
                                {
                                    // Cellule vide
                                    break;
                                }
                                else if (currentCell == colorOpponent)
                                {
                                    // Pion adversaire
                                    opponentPion.Add(new Pawn(column + copyC, line + copyL, colorOpponent));
                                }

                                // Déplacement dans la continuité
                                copyC += c;
                                copyL += l;
                            }
                        }
                    }
                }
            }
            if (!checkOnly && playable)
            {
                toFlip.ForEach(pawn => board[pawn.C, pawn.L] = colorPlayer);
                board[column, line] = colorPlayer;
                return true;
            }
            return false;
        }
        private Tuple<double, Cell> AlphaBeta(TreeNode root, int level, bool isWhite, int minOrMax, double parentValue)
        {
            if (level == 0 || root.Final())
            {
                return Tuple.Create<double, Cell>(root.Eval(), null);
            }

            double optVal = minOrMax * (-int.MaxValue + 1);
            Cell optOp = null;
            var ops = root.Ops(isWhite);

            foreach (var op in ops)
            {
                TreeNode newTree = root.Apply(op.C, op.L, isWhite);
                double val = AlphaBeta(newTree, level - 1, !isWhite, -minOrMax, optVal).Item1;
                if (val * minOrMax >= optVal * minOrMax)
                {
                    optVal = val;
                    optOp = op;

                    if (optVal * minOrMax > parentValue * minOrMax)
                        break;
                }
            }
            return Tuple.Create(optVal, optOp);

            // return Tuple.Create(0, new Cell(-1, -1));
        }
    }
}


