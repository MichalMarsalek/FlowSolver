using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowSolver
{
    public partial class Solver
    {
        public class Branch : List<Point>
        {
            public bool Solved { get; set; }        //Tahle větev je vyřešena?
            public readonly int Color;              //Barva větve
            public Branch Other { get; set; }       //Druhá větev stejné barvy

            public Branch(int color)
            {
                Color = color;
            }

            public Branch(Branch old) : base(old)
            {
                Color = old.Color;
                Solved = old.Solved;
            }
        }
    }
}
