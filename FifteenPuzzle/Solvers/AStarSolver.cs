using System;
using System.Collections.Generic;
using System.Linq;
using FifteenPuzzle.Model;


namespace FifteenPuzzle.Solvers
{
    public class AStarSolver : SolverBase
    {
        public SortedList<Tuple<string, int>, Node> queue;

        public override Node Solve()
        {
            Stopwatch.Start();
            while ( !CurrentNode.IsSolution() )
            {
                Explored.Add(string.Join(",", CurrentNode.Board.Values));

                AddChildNodes(CurrentNode);
                if ( CurrentNode.IsSolution() )
                {
                    break;
                }

                KeyValuePair<Tuple<string, int>, Node> cheapestNode = queue.First();
                CurrentNode = cheapestNode.Value;
                queue.Remove(cheapestNode.Key);
                if ( CurrentNode.CurrentPathCost > Information.DeepestLevelReached )
                {
                    Information.DeepestLevelReached = CurrentNode.CurrentPathCost;
                }
            }


            Stopwatch.Stop();
            Information.ProcessingTime = Stopwatch.Elapsed.TotalMilliseconds;
            Information.StatesProcessed = Explored.Count;
            return CurrentNode;
        }

        public void AddChildNodes(Node node)
        {
            foreach ( Operator availableMove in node.Board.AvailableMoves )
            {
                Node nextNode = MoveTo(node, availableMove);
                if ( !Explored.Contains(string.Join(",", nextNode.Board.Values)) && !queue.ContainsKey(
                         new Tuple<string, int>(string.Join(",", nextNode.Board.Values), HeuristicFunction(nextNode))) )
                {
                    Information.StatesVisited++;
                    if ( nextNode.IsSolution() )
                    {
                        CurrentNode = nextNode;
                        return;
                    }

                    queue.Add(
                        new Tuple<string, int>(string.Join(",", nextNode.Board.Values), HeuristicFunction(nextNode)),
                        nextNode);
                }
            }
        }


        public Node MoveTo(Node node, Operator direction)
        {
            Board newBoard = node.Board.Clone() as Board;
            newBoard.MoveEmptyPuzzle(direction);

            return new Node(newBoard, CurrentNode, direction);
        }


        public AStarSolver(Node startingNode) : base(startingNode)
        {
            queue = new SortedList<Tuple<string, int>, Node>(
                Comparer<Tuple<string, int>>.Create((t1, t2) => t1.Item1 == t2.Item1 ? 0 : t1.Item2 > t2.Item2 ? 1 : t1.Item2 < t2.Item2 ? -1 : 1));
        }

        private int HeuristicFunction(Node node)
        {
            byte[] board = node.Board.Values;
            int dimensionX = node.Board.X;
            int dimensionY = node.Board.Y;
            int distance = node.CurrentPathCost;

            for ( int i = 0; i < dimensionY; i++ )
            {
                for ( int j = 0; j < dimensionX; j++ )
                {
                    if ( i == dimensionY - 1 && j == dimensionX - 1 )
                    {
                        if ( board[j + i * dimensionX] != 0 )
                            distance++;
                    }
                    else
                    {
                        if ( board[j + i * dimensionX] != j + i * dimensionX + 1 )
                            distance++;
                    }
                }
            }

            return distance;
        }
    }
}