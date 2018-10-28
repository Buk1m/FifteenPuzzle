namespace FifteenPuzzle.Data
{
    public class Board
    {
        public byte[] Values { get; set; }
        public byte X { get; }
        public byte Y { get; }

        public Board( byte[] values, byte x, byte y )
        {
            Values = values;
            X = x;
            Y = y;
        }
    }
}