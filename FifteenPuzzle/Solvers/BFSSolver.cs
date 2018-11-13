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
            Statistics.StatesVisited++;
        }

        #endregion

        #region Public

        public override Node Solve()
        {
            Stopwatch.Start();
            while ( !CurrentNode.IsSolution() )
            {
                Explored.Add( CurrentNode.ToString() );

                AddChildNodes( CurrentNode );
                if ( CurrentNode.IsSolution() )
                {
                    break;
                }

                CurrentNode = _frontier.Dequeue();
            }

            Stopwatch.Stop();

            Statistics.DeepestLevelReached = CurrentNode.CurrentPathCost;
            Statistics.ProcessingTime = Stopwatch.Elapsed.TotalMilliseconds;
            Statistics.StatesProcessed = Explored.Count;

            return CurrentNode;
        }

        #endregion

        #region Private

        private readonly List<Operator> _order;
        private readonly Queue<Node> _frontier = new Queue<Node>();

        private void AddChildNodes( Node node )
        {
            foreach ( Operator availableMove in node.GetMoves(_order) )
            {

                Node adjacent = CurrentNode.MoveTo( node, availableMove, Explored );
                if ( adjacent != null )
                {
                    Statistics.StatesVisited++;
                    if ( adjacent.IsSolution() )
                    {
                        CurrentNode = adjacent;
                        return;
                    }

                    _frontier.Enqueue( adjacent );

                }

            }
        }

        #endregion
    }
}