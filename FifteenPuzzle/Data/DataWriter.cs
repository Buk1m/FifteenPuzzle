using System.IO;
using FifteenPuzzle.Model;

namespace FifteenPuzzle.Data
{
    public static class DataWriter
    {
        public static void WriteSolution(string fileName, int solutionLength, char[] steps)
        {
            using ( StreamWriter streamWriter = new StreamWriter(fileName) )
            {
                streamWriter.WriteLine(solutionLength);

                if ( solutionLength < 0 )
                {
                    return;
                }

                for ( int i = 0; i < solutionLength; i++ )
                {
                    streamWriter.Write(steps[i]);
                    streamWriter.Write(" ");
                }
            }
        }

        public static void WriteInformation(string filename, Information info)
        {
            using ( StreamWriter streamWriter = new StreamWriter(filename) )
            {
                streamWriter.WriteLine(info.SolutionLength);
                streamWriter.WriteLine(info.StatesVisited);
                streamWriter.WriteLine(info.StatesProcessed);
                streamWriter.WriteLine(info.DeepestLevelReached);
                streamWriter.WriteLine(info.ProcessingTime);
            }
        }
    }
}