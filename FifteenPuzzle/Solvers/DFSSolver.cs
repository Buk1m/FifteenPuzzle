using System.Collections.Generic;
using FifteenPuzzle.Model;

namespace FifteenPuzzle.Solvers
{
    public class DFSSolver : SolverBase
    {
        private new readonly Dictionary<string, int> Explored = new Dictionary<string, int>();
        private readonly List<Operator> _order;
        private readonly Stack<Node> _frontier = new Stack<Node>();
        private const int MaxDepth = 20;

        public DFSSolver( Node startingNode, string strategy ) : base( startingNode )
        {
            _order = Converters.StrategyToOperatorsConverter( strategy );
            _order.Reverse();
            Statistics.StatesVisited++;
        }

        public override Node Solve()
        {
            Stopwatch.Start();
            while ( !CurrentNode.IsSolution() )
            {
                if ( !Explored.ContainsKey( CurrentNode.ToString() ) )
                {
                    Explored.Add( CurrentNode.ToString(), CurrentNode.CurrentPathCost );
                }
                else 
                {
                    Explored[CurrentNode.ToString()] = CurrentNode.CurrentPathCost;
                }

                AddChildNodes( CurrentNode );

                if ( CurrentNode.CurrentPathCost > Statistics.DeepestLevelReached )
                {
                    Statistics.DeepestLevelReached = CurrentNode.CurrentPathCost;
                }

                if ( CurrentNode.IsSolution() )
                {
                    break;
                }

                CurrentNode = _frontier.Pop();
            }

            Stopwatch.Stop();
            Statistics.ProcessingTime = Stopwatch.Elapsed.TotalMilliseconds;
            Statistics.StatesProcessed = Explored.Count;

            return CurrentNode;
        }

        #region Privates

        private void AddChildNodes( Node node )
        {
            if ( IsDepthGreaterThanMax( node ) )
            {
                return;
            }

            foreach ( Operator availableMove in node.GetMoves( _order ) )
            {

                Node adjacent = CurrentNode.MoveTo( node, availableMove, Explored );
                if (  adjacent != null )
                {
                    Statistics.StatesVisited++;
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
            if ( !Explored.ContainsKey( nextNode.ToString() ) )
            {
                return true;
            }

            return Explored[nextNode.ToString()] > nextNode.CurrentPathCost;
        }

        #endregion
    }
}