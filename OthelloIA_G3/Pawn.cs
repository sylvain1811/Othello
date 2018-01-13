using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OthelloIA_G3
{
    public class Pawn : Cell
    {
        public int Color { get; set; }

        public Pawn(int c, int l, int color) : base(c, l)
        {
            Color = color;
        }

        public int Flip()
        {
            C = C == 0 ? 1 : 0;
            return C;
        }
    }
}
