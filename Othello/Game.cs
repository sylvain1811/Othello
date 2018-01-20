using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello
{
    [Serializable]
    // Classe sérialisable contenant les données essentielles d'une partie en cours.
    class Game
    {
        public int ElapsedWhite { get; set; }
        public int ElapsedBlack { get; set; }
        public int[,] Board { get; set; }
        public bool WhiteTurn { get; set; }

        public Game(int[,] board, bool whiteTurn, int elapsedWhite, int elapsedBlack)
        {
            Board = board;
            WhiteTurn = whiteTurn;
            ElapsedBlack = elapsedBlack;
            ElapsedWhite = elapsedWhite;
        }
    }
}
