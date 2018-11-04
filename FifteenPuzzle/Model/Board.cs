using System;
using System.Collections.Generic;

namespace FifteenPuzzle.Model
{
    public class Board : ICloneable
    {
        public byte[] Values { get; }
        public byte X { get; }
        public byte Y { get; }
        public int EmptyPuzzleIndex { get; private set; }
        public List<Operator> AvailableMoves { get; set; }

        public Board( byte[] values, byte x, byte y )
        {
            Values = values;
            X = x;
            Y = y;
            EmptyPuzzleIndex = FindEmptyPuzzle();
            AvailableMoves = FindAvailableMoves();
        }

        public void MoveEmptyPuzzle( Operator direction )
        {
            if ( direction == Operator.L )
            {
                SwapBytes( EmptyPuzzleIndex - 1 );
                return;
            }

            if ( direction == Operator.R )
            {
                SwapBytes( EmptyPuzzleIndex + 1 );
                return;
            }

            if ( direction == Operator.U )
            {
                SwapBytes( EmptyPuzzleIndex - X );
                return;
            }

            if ( direction == Operator.D )
            {
                SwapBytes( EmptyPuzzleIndex + X );
            }
        }

        public object Clone()
        {
            return new Board( (byte[]) Values.Clone(), X, Y );
        }

        #region Private

        private void SwapBytes( int index )
        {
            byte buffer = Values[EmptyPuzzleIndex];
            Values[EmptyPuzzleIndex] = Values[index];
            Values[index] = buffer;
            EmptyPuzzleIndex = index;
            AvailableMoves = FindAvailableMoves();
        }

        public override string ToString()
        {
            return string.Join( ",", Values );
        }

        private int FindEmptyPuzzle()
        {
            return Array.FindIndex( Values, value => value == 0 );
        }

        private List<Operator> FindAvailableMoves()
        {
            List<Operator> availableMoves = new List<Operator>();
            int posX = EmptyPuzzleIndex % X;
            int posY = EmptyPuzzleIndex / Y;

            if ( HasLeftNeighbour( posX ) )
            {
                availableMoves.Add( Operator.L );
            }

            if ( HasRightNeighbour( posX ) )
            {
                availableMoves.Add( Operator.R );
            }

            if ( HasUpperNeighbour( posY ) )
            {
                availableMoves.Add( Operator.U );
            }

            if ( HasBottomNeighbour( posY ) )
            {
                availableMoves.Add( Operator.D );
            }

            return availableMoves;
        }

        private bool HasBottomNeighbour( int posY )
        {
            return posY < Y - 1;
        }

        private static bool HasUpperNeighbour( int posY )
        {
            return posY > 0;
        }

        private bool HasRightNeighbour( int posX )
        {
            return posX < X - 1;
        }

        private bool HasLeftNeighbour( int posX )
        {
            return posX > 0;
        }

        #endregion
    }
}