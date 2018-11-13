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
            _heuristicFunction = _strategies[ strategy ];

            _frontier = new SortedList<Tuple<string, int>, Node>(
                Comparer<Tuple<string, int>>.Create(
                    ( firstNode, secondNode ) =>
                        firstNode.Item1 == secondNode.Item1 ? 0 :
                        firstNode.Item2 > secondNode.Item2 ? 1 :
                        firstNode.Item2 < secondNode.Item2 ? -1 : 1 )
            );
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

                KeyValuePair<Tuple<string, int>, Node> cheapestNode = _frontier.First();
                CurrentNode = cheapestNode.Value;
                _frontier.Remove( cheapestNode.Key );
            }

            Stopwatch.Stop();
            Statistics.DeepestLevelReached = CurrentNode.CurrentPathCost;
            Statistics.ProcessingTime = Stopwatch.Elapsed.TotalMilliseconds;
            Statistics.StatesProcessed = Explored.Count;

            return CurrentNode;
        }

        #endregion

        #region Private

        private delegate int HeuristicFunction( Node node );

        private readonly SortedList<Tuple<string, int>, Node> _frontier;
        private readonly HeuristicFunction _heuristicFunction;

        private readonly Dictionary<string, HeuristicFunction> _strategies = new Dictionary<string, HeuristicFunction>()
        {
            {"hamm", HammingFunction},
            {"manh", ManhattanFunction}
        };

        private void AddChildNodes( Node node )
        {
            foreach ( Operator availableMove in node.Board.AvailableMoves )
            {
                Node adjacent = CurrentNode.MoveTo( node, availableMove, Explored );
                if ( adjacent != null && !_frontier.ContainsKey( new Tuple<string, int>(
                         string.Join( ",", adjacent.Board.Values ),
                         _heuristicFunction( adjacent ) ) ) )
                {
                    Statistics.StatesVisited++;
                    if ( adjacent.IsSolution() )
                    {
                        CurrentNode = adjacent;
                        return;
                    }

                    _frontier.Add(
                        new Tuple<string, int>( string.Join( ",", adjacent.Board.Values ),
                            _heuristicFunction( adjacent ) ),
                        adjacent );
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
                    if ( IsNotLastTile( i, j, dimensionX, dimensionY ) )
                    {
                        if ( IsTileAtWrongPosition( board, i, j, dimensionX ) )
                        {
                            stepCost++;
                        }
                    }
                }
            }

            return stepCost;
        }
        private static bool IsNotLastTile( int i, int j, int dimensionX, int dimensionY )
        {
            return !(( i == dimensionY - 1 ) && ( j == dimensionX - 1 ));
        }

        private static bool IsTileAtWrongPosition( byte[] board, int i, int j, int dimensionX )
        {
            return board[ j + i * dimensionX ] != ( j + i * dimensionX + 1 );
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
                    int value = board[ j + i * dimensionX ];
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