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
        /// Evaluation du board.
        /// </summary>
        /// <returns></returns>
        public double Eval()
        {
            int maxPlayerCoin, minPlayerCoin, maxPlayerMove, minPlayerMove, coinParity, mobility, corners;
            maxPlayerCoin = minPlayerCoin = maxPlayerMove = minPlayerMove = coinParity = mobility = corners = 0;

            int[,] mat_eval = new int[,] {{20, -10, 1, 1, 1, 1, -10, 20},
                                          {-10, -7, 1, 1, 1, 1, -7, -10},
                                              {1, 1, 1, 1, 1, 1, 1, 1},
                                              {1, 1, 1, 1, 1, 1, 1, 1},
                                              {1, 1, 1, 1, 1, 1, 1, 1},
                                              {1, 1, 1, 1, 1, 1, 1, 1},
                                          {-10, -7, 1, 1, 1, 1, -7, -10},
                                          {20, -10, 1, 1, 1, 1, -10, 20}};

            maxPlayerCoin = isWhite ? board.GetWhiteScore() : board.GetBlackScore();
            minPlayerCoin = isWhite ? board.GetBlackScore() : board.GetWhiteScore();

            if (maxPlayerCoin > minPlayerCoin)
            {
                coinParity = (100 * maxPlayerCoin) / (maxPlayerCoin + minPlayerCoin);
            }
            else
            {
                coinParity = -(100 * maxPlayerCoin) / (maxPlayerCoin + minPlayerCoin);
            }

            int nbrOfPlayableMoveWhite = Ops(isWhite).Count;
            int nbrOfPlayableMoveBlack = Ops(isWhite).Count;

            maxPlayerMove = isWhite ? nbrOfPlayableMoveWhite : nbrOfPlayableMoveBlack;
            minPlayerMove = isWhite ? nbrOfPlayableMoveBlack : nbrOfPlayableMoveWhite;

            if (maxPlayerMove > minPlayerMove)
            {
                mobility = (100 * maxPlayerMove) / (maxPlayerMove + minPlayerMove);
            }
            else if (maxPlayerMove < minPlayerMove)
            {
                mobility = -(100 * maxPlayerMove) / (maxPlayerMove + minPlayerMove);
            }

            corners = GetCornerValue(mat_eval);

            return 2 * coinParity + 5 * mobility + 10 * corners;
        }

        private int GetCornerValue(int[,] mat_eval)
        {
            int typeCoin = isWhite ? 0 : 1;
            int cornerValue = 0;

            for (int c = 0; c < 8; c++)
            {
                for (int l = 0; l < 8; l++)
                {
                    if (board.GetBoard()[c, l] == typeCoin)
                    {
                        cornerValue += board.GetBoard()[c, l];
                    }
                }
            }

            return cornerValue;
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
