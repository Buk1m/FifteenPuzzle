using System.Linq;
using FifteenPuzzle.Data;
using FifteenPuzzle.Model;
using FifteenPuzzle.Solvers;

namespace FifteenPuzzle
{
    class Program
    {
        static void Main( string[] args )
        {
//            args = null;
            string algorithm = args?[0] ?? "bfs";
            string strategy = args?[1] ?? "LUDR";
            string inputFileName = args?[2] ?? "4x4_01_00001.txt";
            string solutionFileName = args?[3] ?? "4x4_01_00001_sol.txt";
            string infoFileName = args?[4] ?? "4x4_01_00001_stats.txt";


            Board board = DataReader.ReadBoard( inputFileName );
            Node startingNode = new Node( board );
            SolverBase solver = AlgorithmFactory.Algorithm[algorithm].Invoke( startingNode, strategy );
            Node solution = solver.Solve();
            string path = "";
            int steps = 0;
            while (solution != null)
            {
                if (solution.Operator != Operator.N)
                {
                    path += solution.Operator.ToString();
                    steps++;
                }

                solution = solution.Parent;
            }

            Information.SolutionLength = steps;
            path = string.Join( "", path.Reverse() );
            //TODO: add values
            DataWriter.WriteSolution( solutionFileName, steps, path );
            DataWriter.WriteInformation( infoFileName );
        }
    }
}