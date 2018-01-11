using System;
using System.Collections.Generic;

namespace OthelloIA_G3
{
    public class Board : IPlayable.IPlayable
    {

        // ATTRIBUTS
        private int[,] board;
        private static string NAME = "Costa_Renaud";

        // CONSTRUCTEUR
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
            board[3, 3] = board[4, 4] = 0; // White
            board[4, 3] = board[3, 4] = 1; // Black
        }

        // IMPLEMENTS
        public int GetBlackScore()
        {
            // throw new NotImplementedException();
            int score = 0;
            foreach (int i in board)
            {
                if (i == 1)
                {
                    score++;
                }
            }
            Console.WriteLine(score);
            return score;
        }

        public int[,] GetBoard()
        {
            return board;
        }

        public string GetName()
        {
            return NAME;
        }

        public Tuple<int, int> GetNextMove(int[,] game, int level, bool whiteTurn)
        {
            // TODO Choisir le meilleur move avec un algo
            throw new NotImplementedException();
        }

        public int GetWhiteScore()
        {
            int score = 0;
            foreach (int i in board)
            {
                if (i == 0)
                {
                    score++;
                }
            }
            Console.WriteLine(score);
            return score;
        }

        public bool IsPlayable(int column, int line, bool isWhite)
        {
            return CheckOrPlay(column, line, isWhite, true);
        }

        public bool PlayMove(int column, int line, bool isWhite)
        {
            return CheckOrPlay(column, line, isWhite, false);
        }

        // PRIVATE
        private bool CheckOrPlay(int column, int line, bool isWhite, bool checkOnly)
        {
            // Check case is empty
            if (board[column, line] > -1)
            {
                return false;
            }

            bool playable = false;

            // Check voisin d'autre couleurs
            int colorOpponent;
            int myColor;
            if (isWhite == true)
            {
                colorOpponent = 1;
                myColor = 0;
            }
            else
            {
                colorOpponent = 0;
                myColor = 1;
            }

            //  (c; l) 
            //  -----------------------
            //  (-1;-1)  (0;-1)  (1;-1)
            //  (-1; 0)  (0; 0)  (1; 0)
            //  (-1; 1)  (0; 1)  (1; 1)

            // Init start/end for line and column
            int cstart;
            if (column <= 0)
                cstart = 0;
            else
                cstart = -1;

            int cend;
            if (column >= 7)
                cend = 0;
            else
                cend = 1;

            int lstart;
            if (line <= 0)
                lstart = 0;
            else
                lstart = -1;

            int lend;
            if (line >= 7)
                lend = 0;
            else
                lend = 1;

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

                            List<Tuple<int, int>> opponentPion = new List<Tuple<int, int>>
                            {
                                new Tuple<int, int>(column + c, line + l)
                            };

                            // Console.WriteLine($"Color ({column + c};{line + l}): " + board[column + c, line + l]);

                            int copyC = c + c; // c dans [-1;1]
                            int copyL = l + l; // l dans [-1;1]

                            while (column + copyC <= 7 && line + copyL <= 7 && column + copyC >= 0 && line + copyL >= 0)
                            {
                                int currentCell = board[column + copyC, line + copyL];
                                if (currentCell == myColor)
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
                                            board[tuple.Item1, tuple.Item2] = myColor;
                                        }
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
                                    opponentPion.Add(new Tuple<int, int>(column + copyC, line + copyL));
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
                board[column, line] = myColor;
                return true;
            }
            return false;
        }
    }
}
