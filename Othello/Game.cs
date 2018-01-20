using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello
{
    [Serializable]
    class Game
    {
        //public ContextPlayers ContextPlayers { get; set; }
        public int ElapsedWhite { get; set; }
        public int ElapsedBlack { get; set; }
        public int[,] Board { get; set; }
        public bool WhiteTurn { get; set; }

        public Game(/*ContextPlayers contextPlayers, */int[,] board, bool whiteTurn, int elapsedWhite, int elapsedBlack)
        {
            // ContextPlayers = contextPlayers;
            Board = board;
            WhiteTurn = whiteTurn;
            ElapsedBlack = elapsedBlack;
            ElapsedWhite = elapsedWhite;
        }
    }
}
