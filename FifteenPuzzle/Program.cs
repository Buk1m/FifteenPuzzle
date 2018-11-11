using System.Linq;
using FifteenPuzzle.Data;
using FifteenPuzzle.Model;
using FifteenPuzzle.Solvers;

namespace FifteenPuzzle
{
    internal class Program
    {
        private static void Main( string[] args )
        {
            ReadExecutionParams( args );
            Board board = DataReader.ReadBoard( _boardFileName );
            SolverBase solver = InitSolver( board );
            Node solution = solver.Solve();
            SaveSolutionData( solution );
        }

        #region Private

        private static string _algorithm;
        private static string _strategy;
        private static string _boardFileName;
        private static string _solutionFileName;
        private static string _statisticsFileName;

        private static SolverBase InitSolver( Board board )
        {
            Node startingNode = new Node( board );
            return AlgorithmFactory.Algorithm[_algorithm].Invoke( startingNode, _strategy );
        }

        private static void SaveSolutionData( Node solution )
        {
            string path = GetSolutionPath( solution );
            Information.SolutionLength = path.Length;
            DataWriter.WriteSolution( _solutionFileName, path );
            DataWriter.WriteInformation( _statisticsFileName );
        }

        private static void ReadExecutionParams( string[] args )
        {
            if ( args.Length != 5 )
                args = null;

            _algorithm = args?[0] ?? "astr";
            _strategy = args?[1] ?? "manh";
            _boardFileName = args?[2] ?? "4x4_07_00009.txt";
            _solutionFileName = args?[3] ?? "..txt";
            _statisticsFileName = args?[4] ?? "...txt";
        }

        public static string GetSolutionPath( Node solution )
        {

            string path = "";
            while ( solution != null )
            {
                if ( solution.LastOperator != Operator.N )
                {
                    path += solution.LastOperator.ToString();
                }

                solution = solution.Parent;
            }

            return string.Join( "", path.Reverse() );
        }

        #endregion
    }
}