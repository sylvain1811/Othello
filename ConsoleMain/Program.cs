using System;
using OthelloIA_G3;

namespace IPlayable
{
    class Program
    {
        /// <summary>
        /// Test de l'IA en mode console.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Board board = new Board();
            //board.GetBlackScore();
            //board.GetWhiteScore();
            Console.WriteLine(board.IsPlayable(4, 5, false));
            PrintBoard(board.GetBoard());
            board.PlayMove(3, 2, false);
            PrintBoard(board.GetBoard());

            board.PlayMove(2, 2, true);
            PrintBoard(board.GetBoard());

            Console.ReadKey();
        }

        public static void PrintBoard(int[,] board)
        {
            Console.WriteLine("===============================================================================");
            Console.WriteLine("NEW BOARD");
            Console.WriteLine("===============================================================================");
            Console.WriteLine("----------------------------------------------------------------");
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (board[j, i] == -1)

                        Console.Write("       |");
                    else

                        Console.Write("   " + board[j, i] + "   |");
                }
                Console.WriteLine("\n----------------------------------------------------------------");
            }
        }
    }
}
