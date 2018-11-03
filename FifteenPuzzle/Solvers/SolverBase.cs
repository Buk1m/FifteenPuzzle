using System.Collections.Generic;
using System.Diagnostics;
using FifteenPuzzle.Model;

namespace FifteenPuzzle.Solvers
{
    public abstract class SolverBase
    {
        protected readonly HashSet<string> Explored = new HashSet<string>();
        protected Node CurrentNode;
        protected readonly Stopwatch Stopwatch = new Stopwatch();

        protected SolverBase(Node startingNode)
        {
            CurrentNode = startingNode;
        }

        public abstract Node Solve();
    }
}