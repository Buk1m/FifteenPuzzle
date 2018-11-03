using System;
using System.Collections.Generic;
using System.Linq;
using FifteenPuzzle.Model;

namespace FifteenPuzzle.Solvers
{
    public class AStarSolver : SolverBase
    {
        #region Constructor

        public AStarSolver( Node startingNode, string strategy ) : base( startingNode )
        {
            if ( strategy == "hamm" )
            {
                _heuristicFunction = HammingFunction;
            }
            else
            {
                _heuristicFunction = ManhattanFunction;
            }

            _queue = new SortedList<Tuple<string, int>, Node>(
                Comparer<Tuple<string, int>>.Create( ( t1, t2 ) =>
                    t1.Item1 == t2.Item1 ? 0 : t1.Item2 > t2.Item2 ? 1 : t1.Item2 < t2.Item2 ? -1 : 1 ) );
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

                if ( CurrentNode.CurrentPathCost > Information.DeepestLevelReached )
                {
                    Information.DeepestLevelReached = CurrentNode.CurrentPathCost;
                }

                if ( CurrentNode.IsSolution() )
                {
                    break;
                }

                KeyValuePair<Tuple<string, int>, Node> cheapestNode = _queue.First();
                CurrentNode = cheapestNode.Value;
                _queue.Remove( cheapestNode.Key );
            }

            Stopwatch.Stop();
            Information.ProcessingTime = Stopwatch.Elapsed.TotalMilliseconds;
            Information.StatesProcessed = Explored.Count;

            return CurrentNode;
        }

        #endregion

        #region Private

        private delegate int HeuristicFunction( Node node );

        private readonly SortedList<Tuple<string, int>, Node> _queue;
        private readonly HeuristicFunction _heuristicFunction;

        private void AddChildNodes( Node node )
        {
            foreach ( Operator availableMove in node.Board.AvailableMoves )
            {
                Node nextNode = MoveTo( node, availableMove );
                if ( nextNode != null && !_queue.ContainsKey( new Tuple<string, int>(
                         string.Join( ",", nextNode.Board.Values ),
                         _heuristicFunction( nextNode ) ) ) )
                {
                    Information.StatesVisited++;
                    if ( nextNode.IsSolution() )
                    {
                        CurrentNode = nextNode;
                        return;
                    }

                    _queue.Add(
                        new Tuple<string, int>( string.Join( ",", nextNode.Board.Values ),
                            _heuristicFunction( nextNode ) ),
                        nextNode );
                }
            }
        }

        private static int HammingFunction( Node node )
        {
            byte[] board = node.Board.Values;
            int dimensionX = node.Board.X;
            int dimensionY = node.Board.Y;
            int stepCost = node.CurrentPathCost;

            for ( int i = 0; i < dimensionY; i++ )
            {
                for ( int j = 0; j < dimensionX; j++ )
                {
                    bool isLastTile = i == dimensionY - 1 && j == dimensionX - 1;
                    if ( !isLastTile )
                    {
                        bool isTileAtWrongPosition = board[j + i * dimensionX] != j + i * dimensionX + 1;
                        if ( isTileAtWrongPosition )
                            stepCost++;
                    }
                }
            }

            return stepCost;
        }

        private static int ManhattanFunction( Node node )
        {
            byte[] board = node.Board.Values;
            int dimensionX = node.Board.X;
            int dimensionY = node.Board.Y;
            int stepCost = node.CurrentPathCost;

            for ( int i = 0; i < dimensionX; i++ )
            {
                for ( int j = 0; j < dimensionY; j++ )
                {
                    int value = board[j + i * dimensionX];
                    if ( value != 0 )
                    {
                        int x = ( value - 1 ) % dimensionX;
                        int y = ( value - 1 ) / dimensionX;
                        x = Math.Abs( j - x );
                        y = Math.Abs( i - y );
                        stepCost += 4 * ( x + y );
                    }
                }
            }

            return stepCost;
        }

        #endregion
    }
}