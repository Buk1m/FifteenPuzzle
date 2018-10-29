using System.Collections.Generic;
using System.Linq;

namespace FifteenPuzzle.Model
{
    public class Node
    {
        #region Public

        public Board Board { get; }
        public Node Parent { get; }
        public Operator Operator { get; }
        public int CurrentPathCost { get; }

        public override int GetHashCode()
        {
            return Board.Values.GetHashCode();
        }

        #endregion

        #region Constructors

        public Node(Board board)
        {
            Board = board;
        }

        public Node(Board board, Node parent, Operator direction)
        {
            Board = board;
            Parent = parent;
            Operator = direction;
            CurrentPathCost = parent.CurrentPathCost + 1;
        }

        #endregion

        public List<Operator> GetMoves(List<Operator> order)
        {
            return order.Intersect(Board.AvailableMoves).ToList();
        }

        public bool IsSolution()
        {
            if ( Board.Values[Board.Values.Length - 1] != 0 )
                return false;
            for ( int i = 0; i < Board.X; i++ )
            {
                for ( int j = 0; j < Board.Y; j++ )
                {
                    if ( j + i * Board.X != Board.Values.Length - 1 )
                        if ( Board.Values[j + i * Board.X] != j + i * Board.X + 1 )
                            return false;
                }
            }

            return true;
        }
    }
}