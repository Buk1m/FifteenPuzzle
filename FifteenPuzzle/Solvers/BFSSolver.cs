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
//            Explored.Add( string.Join(",",CurrentNode.Board.Values ));
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
            foreach (Operator availableMove in node.GetMoves( _order ))
            {
                Node nextNode = MoveTo( node, availableMove );
                if (ExploredNotContainsNode( nextNode ))
                {
                    Information.StatesVisited++;
                    if (nextNode.IsSolution())
                    {
                        CurrentNode = nextNode;
                        return;
                    }

                    _queue.Enqueue( nextNode );
                }
            }
        }

        private bool ExploredNotContainsNode( Node nextNode )
        {
            return !Explored.Contains( CurrentNode.ToString() );
        }

        private Node MoveTo( Node node, Operator direction )
        {
            Board newBoard = node.Board.Clone() as Board;
            newBoard.MoveEmptyPuzzle( direction );

            return new Node( newBoard, CurrentNode, direction );
        }
    }
}