using System.IO;
using FifteenPuzzle.Model;

namespace FifteenPuzzle.Data
{
    public static class DataWriter
    {
        public static void WriteSolution( string fileName, string steps )
        {
            int solutionLength = Statistics.SolutionLength;
            using ( StreamWriter streamWriter = new StreamWriter( fileName ) )
            {
                streamWriter.WriteLine( solutionLength );

                if ( solutionLength < 0 )
                {
                    return;
                }

                for ( int i = 0; i < solutionLength; i++ )
                {
                    streamWriter.Write( steps[i] );
                }
            }
        }

        public static void WriteInformation( string filename )
        {
            using ( StreamWriter streamWriter = new StreamWriter( filename ) )
            {
                streamWriter.WriteLine( Statistics.SolutionLength );
                streamWriter.WriteLine( Statistics.StatesVisited );
                streamWriter.WriteLine( Statistics.StatesProcessed );
                streamWriter.WriteLine( Statistics.DeepestLevelReached );
                streamWriter.WriteLine( Statistics.ProcessingTime );
            }
        }
    }
}