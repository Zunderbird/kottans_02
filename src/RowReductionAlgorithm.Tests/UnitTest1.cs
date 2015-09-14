using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RowReductionAlgorithm;

namespace RowReductionAlgorithm.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Init_EquationSolver_should_return_message_if_matrix_is_empty()
        {
            double[,] _matrix = { { } };

            EquationSolver _solver = new EquationSolver();
            _solver.Init(_matrix);

            Assert.AreEqual("Entering matrix is empty!", _solver.Solve());
        }

        [TestMethod]
        public void Init_EquationSolver_should_return_message_about_unique_solution()
        {
            double[,] _matrix = { {-3, 4, 1, 4, -1},
                                {0, 1, 3, 2, -1},
                                {4, 0, -2, -3, 4},
                                {1000, 3, 1, -5, -2}};

            EquationSolver _solver = new EquationSolver();
            _solver.Init(_matrix);

            Assert.AreEqual("There is unique solution!", _solver.Solve());
        }

        [TestMethod]
        public void Init_EquationSolver_should_return_message_about_many_solutions()
        {
            double[,] _matrix = { {-3, 4, 1, 4, -1},
                                {0, 1, 3, 2, -1},
                                {4, 0, -2, -3, 4},
                                {0, 0, 0, 0, 0}};

            EquationSolver _solver = new EquationSolver();
            _solver.Init(_matrix);

            Assert.AreEqual("There are infinitely many solutions!", _solver.Solve());
        }

        [TestMethod]
        public void Init_EquationSolver_should_return_message_about_infinitely_many_solutions()
        {
            double[,] _matrix = { {-3, 4, 1, 4, -1, 6},
                                {0, 1, 3, 2, -1, 1},
                                {4, 0, -2, -3, 4, -3},
                                {5, 0, -4, 1, 1, 9}};

            EquationSolver _solver = new EquationSolver();
            _solver.Init(_matrix);

            Assert.AreEqual("There are infinitely many solutions!", _solver.Solve());
        }

        [TestMethod]
        public void Init_EquationSolver_should_return_message_about_no_solutions()
        {
            double[,] _matrix = { {-3, 4, 1, 4, -1},
                                {0, 1, 3, 2, -1},
                                {4, 0, -2, -3, 4},
                                {0, 0, 0, 0, -2}};

            EquationSolver _solver = new EquationSolver();
            _solver.Init(_matrix);

            Assert.AreEqual("There are no solutions!", _solver.Solve());
        }

        [TestMethod]
        public void Init_EquationSolver_should_return_message_about_no_solutions2()
        {
            double[,] _matrix = { {-3, 4, 1, 4, -1},
                                {0, 1, 3, 2, -1},
                                {4, 0, -2, -3, 4},
                                {0, 0, 0, 0, 0},
                                {0, 0, 0, 0, 0},
                                {0, 0, 0, 0, -2}};

            EquationSolver _solver = new EquationSolver();
            _solver.Init(_matrix);

            Assert.AreEqual("There are no solutions!", _solver.Solve());
        }

        [TestMethod]
        public void Init_EquationSolver_should_return_message_about_no_solutions3()
        {
            double[,] _matrix = { {-3, 4, 1, 4, -1},
                                {0, 1, 3, 2, -1},
                                {4, 0, -2, -3, 4},
                                {1000, 3, 1, -5, -2},
                                {2, 3, 4, -2, 0}};

            EquationSolver _solver = new EquationSolver();
            _solver.Init(_matrix);

            Assert.AreEqual("There are no solutions!", _solver.Solve());
        }

        [TestMethod]
        public void Init_EquationSolver_should_return_message_about_unique_solution2()
        {
            double[,] _matrix = { {78, 53, 97, 43, 69, 86},
                                {73, 94, 3, 90, 4, 77},
                                {51, 88, 31, 94, 14, 36},
                                {91, 60, 96, 38, 74, 56},
                                {64, 34, 1, 28, 83, 15}};

            EquationSolver _solver = new EquationSolver();
            _solver.Init(_matrix);

            Assert.AreEqual("There is unique solution!", _solver.Solve());
        }

        [TestMethod]
        public void Init_EquationSolver_should_return_message_about_unique_solution3()
        {
            double[,] _matrix = { {8432, 4825, 4305, 6171},
                                {643, 4399, 7976, 0},
                                {8822, 7372, 9169, 0}};

            EquationSolver _solver = new EquationSolver();
            _solver.Init(_matrix);

            Assert.AreEqual("There is unique solution!", _solver.Solve());
        }
    }
}
