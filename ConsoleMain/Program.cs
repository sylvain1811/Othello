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
            board.GetBlackScore();
            board.GetWhiteScore();
            Console.WriteLine(board.IsPlayable(3, 2, false));
            Console.ReadKey();
            
        }
    }
}
