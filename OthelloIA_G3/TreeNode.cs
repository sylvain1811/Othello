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

        /// <summary>
        /// Constructeur de TreeNode
        /// </summary>
        /// <param name="board">Board.</param>
        /// <param name="type">Type.</param>
        /// <param name="isWhite">If set to <c>true</c> is white.</param>
        public TreeNode(Board board, EType type, bool isWhite)
        {
            this.board = new Board(board);
            Type = type;
            this.isWhite = isWhite;
        }

        /// <summary>
        /// Evaluation du board avec plusieurs coefficients heuristiques.
        /// Ces coefficients sont déterminés selon des techniques permettant
        /// d'indiquer quels facteurs de jeu garanti un meilleur 
        /// pourcentage de réussite.
        /// </summary>
        /// <returns></returns>
        public double Eval()
        {
            //Déclaration des variables
            int maxPlayerCoin, minPlayerCoin, maxPlayerMove, minPlayerMove, coinParity,
            mobility, whiteCorners, blackCorners, whiteStability, blackStability,
            maxCorners, minCorners, corners, maxStability, minStability, stability;

            //Initialisation des variables
            maxPlayerCoin = minPlayerCoin = maxPlayerMove = minPlayerMove = coinParity = 
            mobility = whiteCorners = blackCorners = whiteStability = blackStability =
            maxCorners = minCorners = corners = maxStability = minStability = stability = 0;

            //Initialisation de la matrice de valeurs pour valoriser les coins. 
            int[,] mat_corner_weight = new int[,] {{20, -10, 1, 1, 1, 1, -10, 20},
                                                 {-10, -7, 1, 1, 1, 1, -7, -10},
                                                 {1, 1, 1, 1, 1, 1, 1, 1},
                                                 {1, 1, 1, 1, 1, 1, 1, 1},
                                                 {1, 1, 1, 1, 1, 1, 1, 1},
                                                 {1, 1, 1, 1, 1, 1, 1, 1},
                                                 {-10, -7, 1, 1, 1, 1, -7, -10},
                                                 {20, -10, 1, 1, 1, 1, -10, 20}};

            //Initialiation de la matrice de stabilité
            //La stabilité est définie en fonction du fait qu'un pion pourra être
            //retourné dans le tour même, dans un des tours à venir ou qu'il soit
            //impossible à être retourné. 
            int[,] mat_stability_weight = new int[,] { {4, -3, 2, 2, 2, 2, -3, 4},
                                                     {-3, -4, -1, -1, -1, -1, -4, -3}, 
                                                     {2, -1, 1, 0, 0, 1, -1, 2},
                                                     {2,  -1,  0,  1,  1,  0, -1,  2}, 
                                                     {2,  -1,  0,  1,  1,  0, -1,  2 }, 
                                                     {2,  -1,  1,  0,  0,  1, -1,  2}, 
                                                     {-3, -4, -1, -1, -1, -1, -4, -3}, 
                                                     {4,  -3,  2,  2,  2,  2, -3,  4}};


            //Récupère le nombre de pions des joueurs à maximiser et à minimiser
            maxPlayerCoin = isWhite ? board.GetWhiteScore() : board.GetBlackScore();
            minPlayerCoin = isWhite ? board.GetBlackScore() : board.GetWhiteScore();

            //Retourne une valeur entre -100 et 100 du rapport du nombre de pion à maximiser sur le total des deux joueurs
            if (maxPlayerCoin > minPlayerCoin){ 
                coinParity = (100 * maxPlayerCoin)/(maxPlayerCoin + minPlayerCoin);
            }else{
                coinParity = -(100 * maxPlayerCoin)/(maxPlayerCoin + minPlayerCoin);
            }

            //Nombre de coups possibles des blancs et des noirs
            int nbrOfPlayableMoveWhite = Ops(isWhite).Count;
            int nbrOfPlayableMoveBlack = Ops(isWhite).Count;

            //Assigne le nombre de coups en fonction de qui maximiser/minimiser
            maxPlayerMove = isWhite ? nbrOfPlayableMoveWhite : nbrOfPlayableMoveBlack;
            minPlayerMove = isWhite ? nbrOfPlayableMoveBlack : nbrOfPlayableMoveWhite;

            //Retourne une valeur entre -100 et 100 du rapport du nombre de coups à maximiser sur le total des deux joueurs
            if (maxPlayerMove > minPlayerMove){
                mobility = (100 * maxPlayerMove) / (maxPlayerMove + minPlayerMove);
            }else if (maxPlayerMove < minPlayerMove){
                mobility = -(100 * maxPlayerMove) / (maxPlayerMove + minPlayerMove);
            }

            //Retourne une valeur en fonction des pions posés sur le damier
            whiteCorners = EvaluateMatriceValue(mat_corner_weight, true);
            blackCorners = EvaluateMatriceValue(mat_corner_weight, false);

            //Détermine quel poids de case est adressé au joueur à maximiser
            maxCorners = isWhite ? whiteCorners : blackCorners;
            minCorners = isWhite ? blackCorners : whiteCorners;

            //Retourne une valeur entre -100 et 100 du rapport du poids des cases à maximiser sur le total des deux joueurs
            if (maxCorners > minCorners){
                corners = (100 * maxCorners) / (maxCorners + minCorners);
            }else if (maxCorners < minCorners){
                corners = -(100 * maxCorners) / (maxCorners + minCorners);
            }

            //Retourne une valeur en fonction de la stabilité des pions placés sur le damier
            whiteStability = EvaluateMatriceValue(mat_stability_weight, true);
            blackStability = EvaluateMatriceValue(mat_stability_weight, false);

            //Détermine quelle stabilité est adressé au joueur à maximiser
            maxStability = isWhite ? whiteStability : blackStability;
            minStability = isWhite ? blackStability : whiteStability;

            //Retourne une valeur entre -100 et 100 du rapport de stabilité à maximiser sur le total des deux joueurs
            if(maxStability > minStability){
                stability = (100 * maxStability) / (maxStability + minStability);
            }else if (maxStability < minStability){
                stability = -(100 * maxStability) / (maxStability + minStability);
            }

            //Retourne ces dernières valeurs avec des coéfficients selon leur importance
            return 2*coinParity + 5*mobility + 10 *corners + 15 * stability;
        }

        /// <summary>
        /// Calcul le poids des cases de la matrice passée en argument
        /// en fonction de la présence d'un pion noir ou blanc. 
        /// Le paramètre white permet de définir ce paramètre
        /// </summary>
        /// <returns>The matrice value.</returns>
        /// <param name="mat_eval">Mat eval.</param>
        /// <param name="white">If set to <c>true</c> white.</param>
        private int EvaluateMatriceValue(int[,] mat_eval, bool white)
        {
            int typeCoin = white ? 0 : 1;
            int value = 0;

            for (int c = 0; c < 8; c++){
                for (int l = 0; l < 8; l++){
                    if (board.GetBoard()[c,l] == typeCoin){
                        value += mat_eval[c, l];
                    }
                }
            }

            return value;
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
