using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using RowReductionAlgorithm;

namespace RowReductionView
{
    static class Program
    {
        static EquationSolver _mSolver;
        public static EquationSolver Solver
        {
            get
            {
                return _mSolver;
            }
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            _mSolver = new EquationSolver();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var _mainForm = new Form1();
            _mainForm.StartPosition = FormStartPosition.CenterScreen;
            Application.Run(_mainForm);
            
        }
    }
}
