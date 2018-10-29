using System;
using System.Collections.Generic;
using FifteenPuzzle.Model;

namespace FifteenPuzzle
{
    public static class Converters
    {
        public static List<Operator> StrategyToOperatorsConverter(string strategy)
        {
            List<Operator> operators = new List<Operator>();
            foreach ( char move in strategy )
            {
                Enum.TryParse(move.ToString(), true, out Operator @operator);
                operators.Add(@operator);
            }

            return operators;
        }
    }
}