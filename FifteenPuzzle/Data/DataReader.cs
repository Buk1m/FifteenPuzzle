using System.IO;
using FifteenPuzzle.Model;

namespace FifteenPuzzle.Data
{
    public static class DataReader
    {
        public static Board ReadBoard(string filename)
        {
            using ( StreamReader streamReader = new StreamReader(filename) )
            {
                string[] dimensions = streamReader.ReadLine()?.Split(' ');
                byte x = byte.Parse(dimensions[0]);
                byte y = byte.Parse(dimensions[1]);

                byte[] board = new byte[x * y];

                for ( int i = 0; i < y; i++ )
                {
                    string[] line = streamReader.ReadLine()?.Split(' ');
                    for ( int j = 0; j < x; j++ )
                    {
                        board[i * x + j] = byte.Parse(line[j]);
                    }
                }

                return new Board(board,x,y);
            }
        }
    }
}