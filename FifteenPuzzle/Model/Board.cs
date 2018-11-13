using System;
using System.Collections.Generic;

namespace FifteenPuzzle.Model
{
    public class Board : ICloneable
    {
        public byte[] Values { get; }
        public byte X { get; }
        public byte Y { get; }
        public List<Operator> AvailableMoves { get; private set; }

        public Board( byte[] values, byte x, byte y )
        {
            Values = values;
            X = x;
            Y = y;
            _emptyPuzzleIndex = FindEmptyPuzzle();
            AvailableMoves = FindAvailableMoves();
        }

        public void MoveEmptyPuzzle( Operator direction )
        {
            if ( direction == Operator.L )
            {
                SwapBytes( _emptyPuzzleIndex - 1 );
                return;
            }

            if ( direction == Operator.R )
            {
                SwapBytes( _emptyPuzzleIndex + 1 );
                return;
            }

            if ( direction == Operator.U )
            {
                SwapBytes( _emptyPuzzleIndex - X );
                return;
            }

            if ( direction == Operator.D )
            {
                SwapBytes( _emptyPuzzleIndex + X );
            }
        }

        public object Clone()
        {
            return new Board( (byte[]) Values.Clone(), X, Y );
        }

        public override string ToString()
        {
            return string.Join( ",", Values );
        }

        #region Private

        private int _emptyPuzzleIndex;
        private void SwapBytes( int index )
        {
            byte buffer = Values[_emptyPuzzleIndex];
            Values[_emptyPuzzleIndex] = Values[index];
            Values[index] = buffer;
            _emptyPuzzleIndex = index;
            AvailableMoves = FindAvailableMoves();
        }

        private int FindEmptyPuzzle()
        {
            return Array.FindIndex( Values, value => value == 0 );
        }

        private List<Operator> FindAvailableMoves()
        {
            List<Operator> availableMoves = new List<Operator>();
            int posX = _emptyPuzzleIndex % X;
            int posY = _emptyPuzzleIndex / Y;

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

        private bool HasUpperNeighbour( int posY )
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