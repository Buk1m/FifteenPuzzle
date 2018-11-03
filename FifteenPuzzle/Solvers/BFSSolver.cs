using System.Collections.Generic;
using FifteenPuzzle.Model;

namespace FifteenPuzzle.Solvers
{
    public class BFSSolver : SolverBase
    {
        private readonly List<Operator> _order;
        private readonly Queue<Node> _queue = new Queue<Node>();

        public BFSSolver( Node startingNode, string strategy ) : base( startingNode )
        {
            _order = Converters.StrategyToOperatorsConverter( strategy );
            Information.StatesVisited++;
        }

        public override Node Solve()
        {
            Stopwatch.Start();
            while (!CurrentNode.IsSolution())
            {
                Explored.Add( CurrentNode.ToString() );

                AddChildNodes( CurrentNode );
                if (CurrentNode.IsSolution())
                {
                    break;
                }

                CurrentNode = _queue.Dequeue();
            }

            Information.DeepestLevelReached = CurrentNode.CurrentPathCost;

            Stopwatch.Stop();
            Information.ProcessingTime = Stopwatch.Elapsed.TotalMilliseconds;
            Information.StatesProcessed = Explored.Count;

            return CurrentNode;
        }

        private void AddChildNodes( Node node )
        {
            foreach (Node adjacent in node.GetAdjacents( _order ))
            {
                if (ExploredNotContainsNode( adjacent ) )
                {
                    Information.StatesVisited++;
                    if ( adjacent.IsSolution())
                    {
                        CurrentNode = adjacent;
                        return;
                    }

                    _queue.Enqueue( adjacent );
                }
            }
        }

        private bool ExploredNotContainsNode( Node nextNode )
        {
            return !Explored.Contains( nextNode.ToString() );
        }
    }
}