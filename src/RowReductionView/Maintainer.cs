using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RowReductionAlgorithm;

namespace RowReductionView
{
    static class Maintainer
    {
        private static double[,] m_defaultMatrix = { { -3, 4, 1, 4, -1},
                                                     { 0, 1, 3, 2, -1},
                                                     { 4, 0, -2, -3, 4},
                                                     { 1000, 3, 1, -5, -2}};
        public static double[,] DefaultMatrix
        {
            get
            {
                return (double [,]) m_defaultMatrix.Clone();
            }
        }

        public static DataGridViewColumn NextColumn(string i_headerName)
        {
            DataGridViewTextBoxColumn _column = new DataGridViewTextBoxColumn();
            _column.HeaderText = i_headerName;
            _column.CellTemplate = new DataGridViewTextBoxCell();
            _column.MaxInputLength = 4;

            return _column;

        }

        public static void AddUnknowns(DataGridView i_table)
        {
            if (i_table.Columns.Count == 0)
            {
                i_table.Columns.Add(NextColumn("Sum"));
            }
            else if (i_table.Columns.Count == 1)
                i_table.Columns[0].HeaderText = "Sum";

            i_table.Columns.Insert(i_table.Columns.Count - 1, NextColumn("X" + i_table.Columns.Count));
        }

        public static void RemoveUnknowns(DataGridView i_table)
        {
            if (i_table.Columns.Count == 2)
            {
                i_table.Columns.Clear();
            }
            else
            {
                i_table.Columns.RemoveAt(i_table.Columns.Count - 2);
            }
        }

        public static void ResizeTable(int i_count, DataGridView i_table)
        {
            while (i_count + 1 > i_table.ColumnCount)
                AddUnknowns(i_table);
           
            while (i_count + 1 < i_table.ColumnCount)
                RemoveUnknowns(i_table);

            i_table.RowCount = i_count;
        }

        public static void DisplayVector(List<double> i_vector, DataGridView i_table)
        {
            i_table.Columns.Clear();

            foreach (double _x in i_vector)
            {
                i_table.Columns.Add(NextColumn("X" + (i_table.ColumnCount + 1)));

                if (i_table.Columns.Count == 1)
                {
                    i_table.Rows.Add();
                }
                i_table[i_table.Columns.Count - 1, 0].Value = _x;
            }
        }

        public static void DisplayMatrix(double[,] i_matrix, DataGridView i_table)
        {
            i_table.Columns.Clear();

            i_table.RowCount = i_matrix.GetUpperBound(0) + 1;
            i_table.ColumnCount = i_matrix.GetUpperBound(1) + 1;

            for (int i = 0; i < i_table.ColumnCount; i++)
            {
                if (i < i_table.ColumnCount - 1)
                    i_table.Columns[i].HeaderText = "X" + (i + 1);
                else i_table.Columns[i].HeaderText = "Sum";

                for (int j = 0; j < i_table.RowCount; j++)
                {
                    i_table[i, j].Value = Math.Round(i_matrix[j, i], 2);
                }
            }
        }

        public static void DisplayMatrix(List<LinearEquation> i_equations, DataGridView i_table)
        {
            i_table.Columns.Clear();

            i_table.RowCount = i_equations.Count;
            i_table.ColumnCount = EquationSolver.MaxCoeffCount(i_equations) + 1;

            for (int i = 0; i < i_table.ColumnCount; i++)
            {
                if (i < i_table.ColumnCount - 1)
                    i_table.Columns[i].HeaderText = "X" + (i + 1);
                else i_table.Columns[i].HeaderText = "Sum";

                for (int j = 0; j < i_table.RowCount; j++)
                {
                    if (i < i_table.ColumnCount - 1)
                        i_table[i, j].Value = Math.Round(i_equations[j].GetCoeff(i), 2);
                    else i_table[i, j].Value = Math.Round(i_equations[j].Sum, 2);
                }
            }
        }

        public static void DisplayHistory(DataGridView i_table)
        {
            i_table.Columns.Clear();
            i_table.ColumnCount = Program.Solver.CoeffCount + 1;
            i_table.RowCount = Program.Solver.History.Count * (Program.Solver.CoeffCount + 1);

            for (int i = 0; i < i_table.ColumnCount; i++)
            {
                i_table.Columns[i].Width = i_table.Width / i_table.ColumnCount - 2;

                if (i == i_table.ColumnCount - 1)
                {
                    i_table.Columns[i].HeaderText = "Sum";
                }
                else
                    i_table.Columns[i].HeaderText = "X" + i;

                for (int j = 0; j < i_table.RowCount; j++)
                {
                    if (j % i_table.ColumnCount == 0)
                    {
                        if (i == 0)
                            i_table[i, j].Value = Program.Solver.History[j / i_table.ColumnCount].Item2;
                    }
                    else
                    {
                        if (i != i_table.ColumnCount - 1)
                            i_table[i, j].Value =  Math.Round(Program.Solver.History[j / (i_table.ColumnCount)].Item1[(j - 1) % (i_table.ColumnCount)].GetCoeff(i), 2);
                        else
                            i_table[i, j].Value = Math.Round(Program.Solver.History[j / (i_table.ColumnCount)].Item1[(j - 1) % (i_table.ColumnCount)].Sum, 2);
                    }
                }
            }
        }

        public static char ConvertForInt(char i_character)
        {
            char _character = i_character;
            if (!Char.IsDigit(_character) && _character != 8 &&  _character != '-')
                _character = Convert.ToChar((char)0);

            return _character;
        }

        public static char ConvertForDouble(char i_character)
        {
            char _character = i_character;
            if (_character == '.')
                _character = Convert.ToChar(',');
            else if (!Char.IsDigit(_character) && _character != 8 && _character != ',' && _character != '-')
            {
                _character = Convert.ToChar((char)0);
            }
            return _character;
        }

        public static double[,] ReadMatrix(DataGridView i_table)
        {
            double[,] _inputData = new double[i_table.RowCount, i_table.ColumnCount];

            for (int i = 0; i < i_table.RowCount; i++)
                for (int j = 0; j < i_table.ColumnCount; j++)
                {
                    if (i_table[j, i].Value == null)
                        _inputData[i, j] = 0;
                    else
                        _inputData[i, j] = Convert.ToDouble(EditValue(i_table[j, i].Value.ToString()));

                    i_table[j, i].Value = _inputData[i, j];

                }
            return _inputData;
        }

        public static string EditValue(string i_text)
        {
            while (i_text.IndexOf(',') != i_text.LastIndexOf(','))
                i_text = i_text.Remove(i_text.LastIndexOf(','), 1);

            if (i_text.Length == 1 && i_text[0] == ',') i_text = "0";

            while (i_text.LastIndexOf('-') > 0)
                i_text = i_text.Remove(i_text.LastIndexOf('-'), 1);
           
            return i_text;
        }

        public static double[,] GenerateRandMatrix(DataGridView i_table, bool i_isRandSize)
        {
            Random rnd = new Random();

            if (i_isRandSize)
            {
                i_table.RowCount = rnd.Next() % (RandSettStor.MaxDimension - RandSettStor.MinDimension + 1) + RandSettStor.MinDimension;
                i_table.ColumnCount = i_table.RowCount + 1;
            }

            double[,] _randMatrix = new double[i_table.RowCount, i_table.ColumnCount];

            for (int i = 0; i < i_table.RowCount; i++)
                for (int j = 0; j < i_table.ColumnCount; j++)
                {
                    if (rnd.Next() % 100 < RandSettStor.RatioOfZero) 
                        _randMatrix[i, j] = 0;

                    else if (RandSettStor.IsDoubleValue)
                        _randMatrix[i, j] = Math.Round(rnd.NextDouble() * 100 % (RandSettStor.MaxValue - RandSettStor.MinValue + 1) + RandSettStor.MinValue, 2);

                    else _randMatrix[i, j] = rnd.Next() % (RandSettStor.MaxValue - RandSettStor.MinValue + 1) + RandSettStor.MinValue;

                }
            return _randMatrix;
        }

    }
}
