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
        static EquationSolver m_solver;
        public static EquationSolver Solver
        {
            get
            {
                return m_solver;
            }
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            m_solver = new EquationSolver();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form1 _mainForm = new Form1();
            _mainForm.StartPosition = FormStartPosition.CenterScreen;
            Application.Run(_mainForm);
            
        }
    }
}
