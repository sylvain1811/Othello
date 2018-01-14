using System;

namespace OthelloIA_G3
{
    class Program
    {
        // ATTRIBUTS
        static string WHITE = "WHITE";
        static string BLACK = "BLACK";
        static string TITLE = "===============================================================================";
        static string BORDER = "----------------------------------------------------------------";

        /// <summary>
        /// Test de l'IA en mode console.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Board boardWhite = new Board();
            Board boardBlack = new Board();
            Board boardArbitre = new Board();

            bool finished = false;
            bool whiteTurn = false;
            while (!finished)
            {
                Board turnBoard = whiteTurn ? boardWhite : boardBlack;

                var move = turnBoard.GetNextMove(boardArbitre.GetBoard(), 4, whiteTurn);

                if (move.Item1 == -1 && move.Item2 == -1)
                {
                    PrintPass(whiteTurn);
                }
                else
                {
                    if (boardArbitre.IsPlayable(move.Item1, move.Item2, whiteTurn))
                    {
                        boardArbitre.PlayMove(move.Item1, move.Item2, whiteTurn);
                        boardBlack.PlayMove(move.Item1, move.Item2, whiteTurn);
                        boardWhite.PlayMove(move.Item1, move.Item2, whiteTurn);
                    }
                    else
                    {
                        PrintError(move, whiteTurn);
                        break;
                    }
                    PrintBoard(boardArbitre.GetBoard(), whiteTurn, move.Item1, move.Item2);
                }
                whiteTurn = !whiteTurn;

                finished = boardArbitre.IsFinished();

                //Console.ReadKey();
            }
            PrintFinish(boardArbitre);
            Console.ReadKey();
        }

        static void PrintError(Tuple<int, int> tuple, bool isWhite)
        {
            string turn = isWhite ? WHITE : BLACK;
            Console.WriteLine(TITLE);
            Console.WriteLine($"ARBITRE - Coup invalide {turn} ({tuple.Item1};{tuple.Item2})");
            Console.WriteLine(TITLE);
        }

        static void PrintPass(bool isWhite)
        {
            string turn = isWhite ? WHITE : BLACK;
            Console.WriteLine(TITLE);
            Console.WriteLine($"{turn} PASS");
            Console.WriteLine(TITLE);
        }
        static void PrintFinish(Board board)
        {
            Console.WriteLine(TITLE);
            Console.WriteLine($"FINI ! Score : {BLACK} : {board.GetBlackScore()}; {WHITE}: {board.GetWhiteScore()}");
            Console.WriteLine(TITLE);
        }

        static void PrintBoard(int[,] board, bool whiteTurn, int c, int l)
        {
            string turn = whiteTurn ? WHITE : BLACK;
            //Console.Clear();
            Console.WriteLine(TITLE);
            Console.WriteLine($"NEW BOARD : {turn} ({c};{l})");
            Console.WriteLine(TITLE);
            Console.WriteLine(BORDER);
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (board[j, i] == -1)

                        Console.Write("       |");
                    else

                        Console.Write("   " + board[j, i] + "   |");
                }
                Console.WriteLine("\n" + BORDER);
            }
        }
    }
}
