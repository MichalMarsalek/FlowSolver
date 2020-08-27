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
        public Grid Grid { get; private set; }          //Zadání
        private Stack<GridState> stack;                 //Stavy, které je třeba projít
        public int CountScenarios;                      //Počet projitých stavů - testovascí účely
        private HashSet<GridState> seenStates;          //V současnosti se nevyužívá

        //Inicializace řešiče
        public Solver(Grid grid)
        {
            Grid = grid;
            GridState s = new GridState(grid);            
            s.MakeForcedMoves();
            stack = new Stack<GridState>();
            stack.Push(s);
            seenStates = new HashSet<GridState>();
        }

        //this is for testing purposes
        public GridState NextState()
        {
            while (stack.Any())
            {
                GridState s = stack.Pop();
                CountScenarios++;
                foreach (GridState ns in s.NextStates().Reverse())
                {
                    stack.Push(ns);
                }
                return s;
            }
            return new GridState(Grid);
        }

        //Prohledávání do hloubky pomocí zásobníku
        public GridState SolveConnections(System.ComponentModel.BackgroundWorker bw)
        {
            while (stack.Any())
            {
                if (bw != null && bw.CancellationPending)
                {
                    break;
                }
                GridState s = stack.Pop();
                CountScenarios++;
                if (s.Solved)
                {
                    return s;
                }
                foreach (GridState ns in s.NextStates().Reverse())
                {
                    stack.Push(ns);
                }
            }
            return new GridState(Grid);
        }


    }
}
