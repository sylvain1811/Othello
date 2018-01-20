using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Othello
{
    class CellButton : Button
    {
        // Couleur de remplissage
        private static Dictionary<int, Brush> fillBrushesDict = new Dictionary<int, Brush>(){
                    { -1, Brushes.Transparent},
                    { 0, Brushes.White },
                    { 1, Brushes.Black },
                    { 2, (Brush)(new BrushConverter().ConvertFrom("#82FFFFFF"))}, // Blanc avec transparence
                    { 3, (Brush)(new BrushConverter().ConvertFrom("#82000000")) } // Noir avec transparence
                    
        };

        // Couleur de bordure
        private static Dictionary<int, Brush> strokeBrushesDict = new Dictionary<int, Brush>(){
                    { -1, Brushes.Transparent},
                    { 0, Brushes.Black },
                    { 1, Brushes.Black }
        };
        /// <summary>
        /// Colonne dans le plateau de jeu
        /// </summary>
        public int C { get; set; }

        /// <summary>
        /// Ligne dans le plateau de jeu
        /// </summary>
        public int L { get; set; }

        /// <summary>
        /// Case jouable ? 
        /// -1 -> personne
        ///  0 -> joueur blanc
        ///  1 -> joueur noir
        /// </summary>
        public int IsPlayable { get; set; } // -1 -> personne; 0 -> blanc; 1 -> Noir

        /// <summary>
        /// Ellipse à afficher sur le bouton
        /// </summary>
        public Ellipse Ellipse { get; set; }
        private int val;
        /// <summary>
        /// Etat de la case
        /// -1 -> vide
        ///  0 -> pion blanc
        ///  1 -> pion noir
        /// </summary>
        public int Val
        {
            get { return val; }
            set
            {
                // Mise à jour de l'ellipse à afficher
                Ellipse.Fill = fillBrushesDict[value]; // Couleur de remplissage
                Ellipse.Stroke = strokeBrushesDict[value]; // Couleur de bordure
                val = value;
            }
        }

        public CellButton(int c, int l, int val) : base()
        {
            C = c;
            L = l;
            IsPlayable = -1;
            Ellipse = new Ellipse
            {
                Height = 100,
                Width = 100,
            };
            Val = val;
        }

        /// <summary>
        /// Affiche une ellipse de la couleur du joueur actuel en légère transparance dans les cases jouables si il passe la souris dessus.
        /// </summary>
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            if (Val < 0)
            {
                if (IsPlayable >= 0)
                    Ellipse.Fill = fillBrushesDict[IsPlayable + 2]; // couleur du joueur avec transparence
                else
                    Ellipse.Fill = Brushes.Transparent;
            }
        }

        // Masque l'ellipse en légère transparance quand la souris quitte la case.
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            if (Val < 0)
                Ellipse.Fill = Brushes.Transparent;
        }
    }
}
