using System.Collections.Generic;
using System.Linq;

namespace FifteenPuzzle.Model
{
    public class Node
    {
        #region Public

        public Board Board { get; }
        public Node Parent { get; }
        public Operator LastOperator { get; }
        public int CurrentPathCost { get; }

        #endregion

        #region Constructors

        public Node( Board board )
        {
            Board = board;
        }

        public Node( Board board, Node parent, Operator direction )
        {
            Board = board;
            Parent = parent;
            LastOperator = direction;
            CurrentPathCost = parent.CurrentPathCost + 1;
        }

        #endregion

        public IEnumerable<Node> GetNotExploredAdjacentNodes( List<Operator> order, ICollection<string> explored )
        {
            IEnumerable<Operator> moves = GetMoves( order );
            return moves.Select( move => MoveTo( this, move, explored ) ).Where( move => move != null );
        }
        public IEnumerable<Node> GetAdjacentNodes( List<Operator> order )
        {
            IEnumerable<Operator> moves = GetMoves( order );
            return moves.Select( move => MoveTo( this, move ) );
        }

        public bool IsSolution()
        {
            if ( IsZeroAtWrongPosition() )
            {
                return false;
            }

            for ( int i = 0; i < Board.X; i++ )
            {
                for ( int j = 0; j < Board.Y; j++ )
                {
                    if ( IsNotLastElement( i, j ) )
                    {
                        if ( IsTileAtWrongPosition( i, j ) )
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        public override string ToString()
        {
            return string.Join( ",", Board.Values );
        }

        #region Privates

        private bool IsZeroAtWrongPosition()
        {
            return Board.Values[Board.Values.Length - 1] != 0;
        }

        private bool IsNotLastElement( int i, int j )
        {
            return j + i * Board.X != Board.Values.Length - 1;
        }

        private bool IsTileAtWrongPosition( int i, int j )
        {
            return Board.Values[j + i * Board.X] != j + i * Board.X + 1;
        }

        private List<Operator> GetMoves( List<Operator> order )
        {
            return order.Intersect( Board.AvailableMoves ).ToList();
        }

        private Node MoveTo( Node node, Operator direction, ICollection<string> explored )
        {
            Board newBoard = node.Board.Clone() as Board;
            newBoard.MoveEmptyPuzzle( direction );

            if ( explored.Contains( newBoard.ToString() ) )
            {
                return null;
            }

            return new Node( newBoard, this, direction );
        }

        private Node MoveTo( Node node, Operator direction )
        {
            Board newBoard = node.Board.Clone() as Board;
            newBoard.MoveEmptyPuzzle( direction );

            return new Node( newBoard, this, direction );
        }

        #endregion
    }
}