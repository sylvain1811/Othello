using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello_G3
{
    public class Board : IPlayable
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
                if(i == 1)
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
            // throw new NotImplementedException();
            if (board[column, line] > -1)
            {
                return false;
            }
            return true;
        }

        public bool PlayMove(int column, int line, bool isWhite)
        {
            throw new NotImplementedException();
        }
    }
}
