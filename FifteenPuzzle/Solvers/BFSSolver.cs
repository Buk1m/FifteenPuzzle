using System.Collections.Generic;
using FifteenPuzzle.Model;

namespace FifteenPuzzle.Solvers
{
    public class BFSSolver : SolverBase
    {
        #region Constructor

        public BFSSolver( Node startingNode, string strategy ) : base( startingNode )
        {
            _order = Converters.StrategyToOperatorsConverter( strategy );
            Information.StatesVisited++;
        }

        #endregion

        #region Public

        public override Node Solve()
        {
            Stopwatch.Start();
            while ( !CurrentNode.IsSolution() )
            {
                Explored.Add( string.Join( ",", CurrentNode.Board.Values ) );

                AddChildNodes( CurrentNode );
                if ( CurrentNode.IsSolution() )
                {
                    break;
                }

                CurrentNode = _queue.Dequeue();
            }

            Stopwatch.Stop();

            Information.DeepestLevelReached = CurrentNode.CurrentPathCost;
            Information.ProcessingTime = Stopwatch.Elapsed.TotalMilliseconds;
            Information.StatesProcessed = Explored.Count;

            return CurrentNode;
        }

        #endregion

        #region Private

        private readonly List<Operator> _order;
        private readonly Queue<Node> _queue = new Queue<Node>();

        private void AddChildNodes( Node node )
        {
            foreach ( Operator availableMove in node.GetMoves( _order ) )
            {
                Node nextNode = MoveTo( node, availableMove );
                if ( nextNode != null )
                {
                    Information.StatesVisited++;
                    if ( nextNode.IsSolution() )
                    {
                        CurrentNode = nextNode;
                        return;
                    }

                    _queue.Enqueue( nextNode );
                }
            }
        }

        #endregion
    }
}