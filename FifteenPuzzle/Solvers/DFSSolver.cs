using System.Collections.Generic;
using FifteenPuzzle.Model;

namespace FifteenPuzzle.Solvers
{
    public class DFSSolver : SolverBase
    {
        private readonly List<Operator> _order;
        private readonly Stack<Node> _frontier = new Stack<Node>();
        private const int MaxDepth = 20;

        public DFSSolver( Node startingNode, string strategy ) : base( startingNode )
        {
            _order = Converters.StrategyToOperatorsConverter( strategy );
            _order.Reverse();
            Information.StatesVisited++;
        }

        public override Node Solve()
        {
            Stopwatch.Start();
            _frontier.Push( CurrentNode );

            while (!CurrentNode.IsSolution())
            {
                CurrentNode = _frontier.Pop();
                Explored.Add( CurrentNode.ToString() );
                if (CurrentNode.IsSolution())
                {
                    break;
                }

                AddChildNodes( CurrentNode );
            }

            Information.DeepestLevelReached = CurrentNode.CurrentPathCost;

            Stopwatch.Stop();
            Information.ProcessingTime = Stopwatch.Elapsed.TotalMilliseconds;
            Information.StatesProcessed = Explored.Count;

            return CurrentNode;
        }

        #region Privates

        private void AddChildNodes( Node node )
        {
            if ( IsDepthGreaterThanMax( node ) )
            {
                return;
            }

            foreach ( Node adjacent in node.GetNotExploredAdjacentNodes( _order, Explored ) )
            {
                if ( FrontierNotContainsNode( adjacent ) )
                {
                    Information.StatesVisited++;
                    if ( adjacent.IsSolution() )
                    {
                        CurrentNode = adjacent;
                        return;
                    }

                    _frontier.Push( adjacent );
                }
            }
        }

        private bool FrontierNotContainsNode( Node nextNode )
        {
            return !_frontier.Contains( nextNode );
        }

        private static bool IsDepthGreaterThanMax( Node node )
        {
            return node.CurrentPathCost > MaxDepth;
        }

        #endregion
    }
}