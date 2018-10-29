using System.Collections.Generic;
using FifteenPuzzle.Model;

namespace FifteenPuzzle.Solvers
{
    public abstract class SolverBase
    {
        protected readonly HashSet<Node> Explored = new HashSet<Node>();
        protected Node CurrentNode;

        protected SolverBase(Node startingNode)
        {
            CurrentNode = startingNode;
        }

        public abstract Node Solve();
    }
}