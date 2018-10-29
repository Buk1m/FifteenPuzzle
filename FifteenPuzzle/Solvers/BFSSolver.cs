using System.Collections.Generic;
using FifteenPuzzle.Model;

namespace FifteenPuzzle.Solvers
{
    public class BFSSolver : SolverBase
    {
        protected readonly HashSet<Node> Explored = new HashSet<Node>();
        private readonly List<Operator> _order;
        private readonly Queue<Node> _queue = new Queue<Node>();

        public BFSSolver(Node startingNode, string strategy) : base(startingNode)
        {
            _order = Converters.StrategyToOperatorsConverter(strategy);
            Explored.Add(startingNode);
        }

        public override Node Solve()
        {
            while ( !CurrentNode.IsSolution() )
            {
                AddChildNodes(CurrentNode);
                CurrentNode = _queue.Dequeue();
            }

            return CurrentNode;
        }

        public void AddChildNodes(Node node)
        {
            foreach ( Operator availableMove in node.GetMoves(_order) )
            {
                Node nextNode = MoveTo(node, availableMove);
                if ( !Explored.Contains(nextNode) )
                {
                    _queue.Enqueue(nextNode);
                    if ( nextNode.IsSolution() )
                    {
                        return;
                    }

                    Explored.Add(nextNode);
                }
            }
        }

        public Node MoveTo(Node node, Operator direction)
        {
            Board newBoard = node.Board.Clone() as Board;
            newBoard.MoveEmptyPuzzle(direction);

            return new Node(newBoard, CurrentNode, direction);
        }
    }
}