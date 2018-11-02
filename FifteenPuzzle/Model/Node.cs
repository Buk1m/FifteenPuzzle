﻿using System.Collections.Generic;
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
            Operator = direction;
            CurrentPathCost = parent.CurrentPathCost + 1;
        }

        #endregion

        public List<Operator> GetMoves( List<Operator> order )
        {
            return order.Intersect( Board.AvailableMoves ).ToList();
        }

        public bool IsSolution()
        {
            if (IsZeroAtWrongPosition())
                return false;
            for (int i = 0; i < Board.X; i++)
            {
                for (int j = 0; j < Board.Y; j++)
                {
                    if (IsNotLastElement( i, j ))
                        if (IsTileAtWrongPosition( i, j ))
                            return false;
                }
            }

            return true;
        }

        public override string ToString()
        {
            /*if (Parent != null)
            {
                return string.Join( ",", Board.Values) + string.Join( ",", Parent.Board.Values );
            }
            else
            {
                return string.Join( ",", Board.Values) + "root";
            }*/
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

        #endregion
    }
}