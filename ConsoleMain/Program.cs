using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Othello_G3;

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

            Console.ReadKey();
        }
    }
}
