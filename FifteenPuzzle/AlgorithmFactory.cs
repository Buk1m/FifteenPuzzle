﻿using System;
using System.Collections.Generic;
using FifteenPuzzle.Model;
using FifteenPuzzle.Solvers;

namespace FifteenPuzzle
{
    public static class AlgorithmFactory
    {
        public static Dictionary<string, Func<Node, string, SolverBase>> Algorithm =
            new Dictionary<string, Func<Node, string, SolverBase>>()
            {
                {"bfs", ( node, strategy ) => new BFSSolver( node, strategy )},
                {"astr", ( node, strategy ) => new AStarSolver( node, strategy )},
                {"dfs", ( node, strategy ) => new DFSSolver( node, strategy )},
            };
    }
}