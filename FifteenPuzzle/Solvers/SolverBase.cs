using System.Collections.Generic;
using System.Diagnostics;
using FifteenPuzzle.Model;

namespace FifteenPuzzle.Solvers
{
    public abstract class SolverBase
    {
        #region Constructor

        protected SolverBase( Node startingNode )
        {
            CurrentNode = startingNode;
        }

        #endregion

        #region Protected

        protected readonly HashSet<string> Explored = new HashSet<string>();
        protected Node CurrentNode;
        protected readonly Stopwatch Stopwatch = new Stopwatch();

        /*protected SolverBase(Node startingNode)
        {
            Board newBoard = node.Board.Clone() as Board;
            newBoard.MoveEmptyPuzzle( direction );
            if ( Explored.Contains( string.Join( ",", newBoard.Values ) ) )
                return null;

            return new Node( newBoard, CurrentNode, direction );
        }*/

        #endregion

        #region Abstract

        public abstract Node Solve();

        #endregion
    }
}