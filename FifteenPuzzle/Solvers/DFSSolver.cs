using System;
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
            while ( !CurrentNode.IsSolution() )
            {
                CurrentNode = _frontier.Pop();
                Explored.Add( CurrentNode.ToString() + CurrentNode.CurrentPathCost );
                AddChildNodes( CurrentNode );

                if ( Information.DeepestLevelReached < CurrentNode.CurrentPathCost )
                {
                    Information.DeepestLevelReached = CurrentNode.CurrentPathCost;
                }
            }

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

            foreach ( Node adjacent in node.GetAdjacentNodes( _order ) )
            {
                if ( ExploredNotContainsNode( adjacent ) )
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

        private bool IsDepthGreaterThanMax( Node node )
        {
            return node.CurrentPathCost >= MaxDepth;
        }

        private bool ExploredNotContainsNode( Node nextNode )
        {
            return !Explored.Contains( nextNode.ToString() + ( CurrentNode.CurrentPathCost - 1 ) );
        }
        #endregion
    }
}
