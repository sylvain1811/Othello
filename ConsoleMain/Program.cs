using System;
using OthelloIA_G3;

namespace OthelloIA_G3
{
    class Program
    {
        /// <summary>
        /// Test de l'IA en mode console.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Board boardWhite = new Board();
            Board boardBlack = new Board();

            PrintBoard(boardWhite.GetBoard(), false);

            while (true)
            {
                var moveBlack = boardBlack.GetNextMove(boardBlack.GetBoard(), 4, false);
                if (boardBlack.IsPlayable(moveBlack.Item1, moveBlack.Item2, false))
                {
                    boardBlack.PlayMove(moveBlack.Item1, moveBlack.Item2, false);
                    boardWhite.PlayMove(moveBlack.Item1, moveBlack.Item2, false);
                }
                else
                {
                    PrintError(moveBlack, false);
                    break;
                }

                PrintBoard(boardBlack.GetBoard(), false);

                Console.ReadKey();

                var moveWhite = boardWhite.GetNextMove(boardWhite.GetBoard(), 4, true);
                if (boardWhite.IsPlayable(moveWhite.Item1, moveWhite.Item2, true))
                {
                    boardWhite.PlayMove(moveWhite.Item1, moveWhite.Item2, true);
                    boardBlack.PlayMove(moveWhite.Item1, moveWhite.Item2, true);
                }
                else
                {
                    PrintError(moveWhite, true);
                    break;
                }
                PrintBoard(boardWhite.GetBoard(), true);

                Console.ReadKey();
            }
            Console.ReadKey();
        }

        static void PrintError(Tuple<int, int> tuple, bool isWhite)
        {
            string turn = isWhite ? WHITE : BLACK;
            Console.WriteLine("===============================================================================");
            Console.WriteLine($"ARBITRE - Coup invalide {turn} ({tuple.Item1};{tuple.Item2})");
            Console.WriteLine("===============================================================================");
        }

        static string WHITE = "WHITE";
        static string BLACK = "BLACK";
        public static void PrintBoard(int[,] board, bool whiteTurn)
        {
            string turn = whiteTurn ? WHITE : BLACK;
            Console.WriteLine("===============================================================================");
            Console.WriteLine($"NEW BOARD : {turn}");
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
