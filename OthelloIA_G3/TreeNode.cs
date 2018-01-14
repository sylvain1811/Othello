using System;
using System.Collections.Generic;

namespace OthelloIA_G3
{
    class TreeNode
    {
        public enum EType { MIN = -1, MAX = 1 };

        // Attributs

        //public List<TreeNode> Children { get; set; }
        public EType Type { get; set; }

        private Board board;
        private bool isWhite;

        public TreeNode(Board board, EType type, bool isWhite)
        {
            this.board = new Board(board);
            Type = type;
            this.isWhite = isWhite;
        }

        /// <summary>
        /// V1 : La valeur retournée indique la différence de coups possible entre les joueurs. Si positif : noir à l'avantage.
        /// V2 : Division Noir/Blanc : si val > 1 alors black a l'avantage.
        /// </summary>
        /// <returns></returns>
        public double Eval()
        {
            int possibilityWhite = Ops(true).Count;
            int possibilityBlack = Ops(false).Count;
            return possibilityBlack - possibilityWhite;
        }

        public static bool Final(Board board)
        {
            for (int c = 0; c < 8; c++)
            {
                for (int l = 0; l < 8; l++)
                {
                    if (board.GetBoard()[c, l] > -1)
                        // Cellule prise
                        continue;
                    else
                    {
                        if (board.IsPlayable(c, l, true) || board.IsPlayable(c, l, false))
                            // Un des deux joueurs peut encore jouer
                            return false;
                    }
                }
            }
            // Aucune possibilité
            return true;
        }

        public bool Final()
        {
            return Final(board);
        }

        public List<Cell> Ops(bool isWhite)
        {
            var ops = new List<Cell>();
            for (int c = 0; c < 8; c++)
            {
                for (int l = 0; l < 8; l++)
                {
                    if (board.IsPlayable(c, l, isWhite))
                    {
                        ops.Add(new Cell(c, l));
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

            return new TreeNode(newBoard, newType, this.isWhite);
        }

        public override string ToString()
        {
            string output = "";
            for (int l = 0; l < 8; l++)
            {
                for (int c = 0; c < 8; c++)
                {
                    output += board.GetBoard()[c, l] + " ";
                }
                output += "\n";
            }
            return output;
        }
    }
}
