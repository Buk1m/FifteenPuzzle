using System.Collections.Generic;
using FifteenPuzzle.Model;

namespace FifteenPuzzle.Solvers
{
    public class DFSSolver : SolverBase
    {
        private readonly List<Operator> _order;
        private readonly Stack<Node> _stack = new Stack<Node>();
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
            _stack.Push( CurrentNode );

            while (!CurrentNode.IsSolution())
            {
                CurrentNode = _stack.Pop();
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


        private void AddChildNodes( Node node )
        {
            foreach (Operator availableMove in node.GetMoves( _order ))
            {
                if ( IsDepthGreaterThanMax( node ) )
                {
                    return;
                }
                Node nextNode = MoveTo( node, availableMove );
                if (ExploredNotContainsNode( nextNode ) || !_stack.Contains( nextNode ))
                {
                    Information.StatesVisited++;
                    if (nextNode.IsSolution())
                    {
                        CurrentNode = nextNode;
                        return;
                    }

                    _stack.Push( nextNode );
                }
            }
        }

        private bool IsDepthGreaterThanMax( Node node )
        {
            return node.CurrentPathCost > MaxDepth;
        }

        private bool ExploredNotContainsNode( Node nextNode )
        {
            return !Explored.Contains( nextNode.ToString() );
        }

        private Node MoveTo( Node node, Operator direction )
        {
            Board newBoard = node.Board.Clone() as Board;
            newBoard.MoveEmptyPuzzle( direction );

            return new Node( newBoard, CurrentNode, direction );
        }
    }
}