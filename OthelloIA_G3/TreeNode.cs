using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OthelloIA_G3
{
    class TreeNode
    {
        public enum EType { MIN, MAX, LEAF };

        // Attributs

        public List<TreeNode> Children { get; set; }
        public EType Type { get; set; }

        private Board board;
        private int myColor;

        public TreeNode(Board board, EType type, int myColor)
        {
            this.board = board;
            Type = type;
            this.myColor = myColor;
        }

        public int Eval()
        {
            if (myColor == 0)
            {
                if (Type == EType.MAX)
                    // On veut connaitre le meilleur coup des blancs si on est blanc
                    return board.GetWhiteScore();
                else
                    return board.GetBlackScore();
            }
            else
            {
                if (Type == EType.MAX)
                    return board.GetBlackScore();
                else
                    return board.GetWhiteScore();
            }
        }

        public bool Final()
        {
            int cptPossibilite = 64;

            for (int c = 0; c < 8; c++)
            {
                for (int l = 0; l < 8; l++)
                {
                    if (board.GetBoard()[c, l] > -1)
                        // Cellule prise
                        cptPossibilite--;
                    else
                    {
                        if (board.IsPlayable(c, l, true) || board.IsPlayable(c, l, false))
                            // Un des deux joueurs peut encore jouer
                            return false;
                        else
                            // Personne peut jouer à cette case
                            cptPossibilite--;
                    }
                }
            }
            // Aucune possibilité
            return true;
        }

        public List<Tuple<int, int>> Ops(bool isWhite)
        {
            var ops = new List<Tuple<int, int>>();
            for (int c = 0; c < 8; c++)
            {
                for (int l = 0; l < 8; l++)
                {
                    if (board.IsPlayable(c, l, isWhite))
                    {
                        ops.Add(new Tuple<int, int>(c, l));
                    }
                }
            }
            return ops;
        }

        public TreeNode Apply(int col, int line, bool isWhite)
        {
            Board newBoard = new Board(board);
            newBoard.PlayMove(col, line, isWhite);

            EType newType;
            if (Type == EType.MAX)
                newType = EType.MIN;
            else
                newType = EType.MAX;

            return new TreeNode(newBoard, newType, myColor); ;
        }
    }
}
