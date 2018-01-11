using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IPlayable;

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
            this.board = new int[8, 8];
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
            // throw new NotImplementedException();
            return board;
        }

        public string GetName()
        {
            // throw new NotImplementedException();
            return NAME;
        }

        public Tuple<int, int> GetNextMove(int[,] game, int level, bool whiteTurn)
        {
            throw new NotImplementedException();
        }

        public int GetWhiteScore()
        {
            // throw new NotImplementedException();
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
            // Check case is empty
            if (board[column, line] > -1)
            {
                return false;
            }

            // Check voisin d'autre couleurs
            int colorVoisin;
            int myColor;
            if (isWhite == true)
            {
                colorVoisin = 1;
                myColor = 0;
            }
            else
            {
                myColor = 1;
                colorVoisin = 0;
            }

            //  (c; l) 
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

            // Parcours des voisins
            for (int c = cstart; c == cend; c++)
            {
                for (int l = lstart; l == lend; l++)
                {
                    // Check seulement si la case n'est pas nous-même
                    if (!(c == 0 && l == 0))
                    {
                        if (column <= 0)
                        {
                            if (board[column + c, line + l] == colorVoisin)
                            {
                                // Check si il y a un pion de notre couleur dans cette ligne/col/diagonale
                                int copyC = c; // [-1;1]
                                int copyL = l; // [-1;1]

                                while (copyC <= 7 && copyL <= 7 && copyC >= 0 && copyL >= 0)
                                {
                                    copyC += copyC;
                                    copyL += copyL;

                                    if (board[column + copyC, line + copyL] == myColor)
                                        return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        public bool PlayMove(int column, int line, bool isWhite)
        {
            throw new NotImplementedException();
        }
    }
}
