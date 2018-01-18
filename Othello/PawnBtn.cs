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
    class PawnBtn : Button
    {
        private static Dictionary<int, Brush> fillBrushesDict = new Dictionary<int, Brush>(){
                    { -1, Brushes.Transparent},
                    { 0, Brushes.White },
                    { 1, Brushes.Black }
        };

        private static Dictionary<int, Brush> fillTransparentDict = new Dictionary<int, Brush>(){
                    { -1, Brushes.Transparent},
                    { 0, (Brush)(new BrushConverter().ConvertFrom("#82FFFFFF"))},
                    { 1, (Brush)(new BrushConverter().ConvertFrom("#82000000")) }
        };

        private static Dictionary<int, Brush> strokeBrushesDict = new Dictionary<int, Brush>(){
                    { -1, Brushes.Transparent},
                    { 0, Brushes.Black },
                    { 1, Brushes.Black }
        };

        public int C { get; set; }
        public int L { get; set; }
        public int IsPlayable { get; set; } // -1 -> personne; 0 -> blanc; 1 -> Noir
        public Ellipse Ellipse { get; set; }
        private int val;
        public int Val
        {
            get { return val; }
            set
            {
                Ellipse.Fill = fillBrushesDict[value];
                Ellipse.Stroke = strokeBrushesDict[value];
                val = value;
            }
        }

        public PawnBtn(int c, int l, int val) : base()
        {
            C = c;
            L = l;
            IsPlayable = -1;
            Ellipse = new Ellipse
            {
                Height = 100,
                Width = 100,
                Fill = Brushes.White,
            };
            Val = val;
        }
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            if (Val < 0)
                Ellipse.Fill = fillTransparentDict[IsPlayable];
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            if (Val < 0)
                Ellipse.Fill = fillTransparentDict[-1];
        }
    }
}
