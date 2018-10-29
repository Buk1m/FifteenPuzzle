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
        public List<Operator> AvailableMoves { get; }

        public Board(byte[] values, byte x, byte y)
        {
            Values = values;
            X = x;
            Y = y;
            AvailableMoves = new List<Operator>();
            FindEmptyPuzzle();
            FindAvailableMoves();
        }

        public void MoveEmptyPuzzle(Operator direction)
        {
            if ( direction == Operator.L )
            {
                SwapBytes(EmptyPuzzleIndex - 1);
            }

            if ( direction == Operator.R )
            {
                SwapBytes(EmptyPuzzleIndex + 1);
            }

            if ( direction == Operator.U )
            {
                SwapBytes(EmptyPuzzleIndex - X);
            }

            if ( direction == Operator.D )
            {
                SwapBytes(EmptyPuzzleIndex + X);
            }
        }

        private void SwapBytes(int index)
        {
            byte value = Values[EmptyPuzzleIndex];
            Values[EmptyPuzzleIndex] = Values[index];
            Values[index] = value;
            EmptyPuzzleIndex = index;
            FindAvailableMoves();
        }

        #region Private

        private void FindEmptyPuzzle()
        {
            EmptyPuzzleIndex = Array.FindIndex(Values, value => value == 0);
        }

        private void FindAvailableMoves()
        {
            AvailableMoves.Clear();
            int posX = EmptyPuzzleIndex % X;
            int posY = EmptyPuzzleIndex / Y;
            if ( posX > 0 )
            {
                AvailableMoves.Add(Operator.L);
            }

            if ( posX < X - 1 )
            {
                AvailableMoves.Add(Operator.R);
            }

            if ( posY > 0 )
            {
                AvailableMoves.Add(Operator.U);
            }

            if ( posY < Y - 1 )
            {
                AvailableMoves.Add(Operator.D);
            }
        }

        #endregion

        public object Clone()
        {
            return new Board((byte[]) Values.Clone(), X, Y);
        }
    }
}