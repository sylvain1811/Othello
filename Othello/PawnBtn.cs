using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Othello
{
    class PawnBtn : Button
    {
        public int C { get; set; }
        public int L { get; set; }
        public int Val
        {
            get { return Val; }
            set
            {
                Content = value;
            }
        }

        public PawnBtn(int c, int l, int val)
        {
            C = c;
            L = l;
            Val = val;
        }
    }
}
