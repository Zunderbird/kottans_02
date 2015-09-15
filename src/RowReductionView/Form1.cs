using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RowReductionAlgorithm;

namespace RowReductionView
{
    public partial class Form1 : Form
    {
        public const int MAX_EQUATIONS_COUNT = 9;

        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            button_solve.Enabled = false;
            numericUpDown1.Maximum = MAX_EQUATIONS_COUNT;
            toolStripStatusLabel2.Text = "Enter the matrix ;)";
            statusStrip1.Refresh();
        }

        private void button_solve_Click(object sender, EventArgs e)
        {
            Program.Solver.Init(Maintainer.ReadMatrix(dataGridView1));
            Program.Solver.SortZeroOnTop = (comboBox1.SelectedIndex == 0);
            Program.Solver.ReduceByMainElement = (comboBox2.SelectedIndex == 0);

            toolStripStatusLabel2.Text = Program.Solver.Solve();
            statusStrip1.Refresh();

            Maintainer.DisplayVector(Program.Solver.XVector, dataGridView4);
            Maintainer.DisplayVector(Program.Solver.ErrorVector, dataGridView5);
            Maintainer.DisplayMatrix(Program.Solver.History[Program.Solver.History.Count - 1].Item1, dataGridView2);
            Maintainer.DisplayHistory(dataGridView3);
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress += new KeyPressEventHandler(Cell_KeyPress);
        }

        private void Cell_KeyPress(object Sender, KeyPressEventArgs pressE)
        {
            pressE.KeyChar = Maintainer.ConvertForDouble(pressE.KeyChar);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void hELPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Made by Ruslana Lagutina");
        }

        private void defaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Maintainer.DisplayMatrix(Maintainer.DefaultMatrix, dataGridView1);
            button_solve.PerformClick();
        }

        private void dataGridView1_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            if (dataGridView1.ColumnCount > 0 && dataGridView1.RowCount > 0) button_solve.Enabled = true;
            if (button_solve.Enabled)setNumericValue(dataGridView1.ColumnCount - 1,"70");
         //   else setNumericValue(0, "71");
            
        }

        private void setNumericValue(int value, string caller=null)
        {
            Console.WriteLine("Setting numeric by "+caller+" new=" + value + " old=" + numericUpDown1.Value);
            numericUpDown1.Value = value;
        }
        private void dataGridView1_ColumnRemoved(object sender, DataGridViewColumnEventArgs e)
        {
            if (dataGridView1.ColumnCount == 0 || dataGridView1.RowCount == 0) button_solve.Enabled = false;
            if (button_solve.Enabled) setNumericValue(dataGridView1.ColumnCount - 1, "83");
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Maintainer.ResizeTable((int)numericUpDown1.Value, dataGridView1);
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (dataGridView1.ColumnCount > 0 && dataGridView1.RowCount > 0) button_solve.Enabled = true;
        }

        private void dataGridView1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (dataGridView1.ColumnCount == 0 || dataGridView1.RowCount == 0) button_solve.Enabled = false;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            defaultToolStripMenuItem.PerformClick();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Maintainer.DisplayMatrix(Maintainer.GenerateRandMatrix(dataGridView1, true), dataGridView1);
            button_solve.PerformClick();
        }

        private void dataGridView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) button_solve.PerformClick();
        }

        private void randomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RandomSettings _formRandom = new RandomSettings();
            _formRandom.StartPosition = FormStartPosition.CenterParent;
            _formRandom.ShowDialog();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            randomToolStripMenuItem.PerformClick();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Maintainer.DisplayMatrix(Maintainer.GenerateRandMatrix(dataGridView1, false), dataGridView1);
            button_solve.PerformClick();
        }

    }
}
