using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace RowReductionAlgorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            double[,] _matrix = { {-3, 4, 1, 4, -1},
                                {0, 1, 3, 2, -1},
                                {4, 0, -2, -3, 4},
                                {1000, 3, 1, -5, -2}};

            EquationSolver _solver = new EquationSolver();
            _solver.Init(_matrix);

            Console.WriteLine(_solver.Solve());

            for (int i = 0; i < _solver.History.Count; i++)
            {
                EquationSolver.PrintToConsole(_solver.History[i].Item1, _solver.History[i].Item2);
            }

            List<double> _XVector = _solver.XVector;

            Console.WriteLine("XVector:");
            foreach (double _x in _XVector)
            {
                Console.WriteLine(_x);
            }

            List<double> _ErrorVector = _solver.ErrorVector;

            Console.WriteLine("ErrorVector:");
            foreach (double _err in _ErrorVector)
            {
                Console.WriteLine(_err);
            }

        }
        
    }
}
