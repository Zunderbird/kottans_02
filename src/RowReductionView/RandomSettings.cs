using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RowReductionView
{
    public partial class RandomSettings : Form
    {
        public RandomSettings()
        {
            InitializeComponent();
            numericUpDown1.Minimum = 1;
            numericUpDown2.Minimum = 1;
            numericUpDown1.Maximum = Form1.MAX_EQUATIONS_COUNT;
            numericUpDown2.Maximum = Form1.MAX_EQUATIONS_COUNT;
            textBox1.Text = RandSettStor.MinValue.ToString();
            textBox2.Text = RandSettStor.MaxValue.ToString();
            textBox3.Text = RandSettStor.RatioOfZero.ToString();
            numericUpDown1.Value = RandSettStor.MinDimension;
            numericUpDown2.Value = RandSettStor.MaxDimension;
            checkBox1.Checked = RandSettStor.IsDoubleValue;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Maintainer.ConvertForInt(e.KeyChar);
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Maintainer.ConvertForInt(e.KeyChar);
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Maintainer.ConvertForInt(e.KeyChar);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool _isInputCorrect = true;

            if (Convert.ToInt32(textBox1.Text) > Convert.ToInt32(textBox2.Text))
            {
                MessageBox.Show("Min value can't be larger then Max value!");
                _isInputCorrect = false;
            }

            if ((int)numericUpDown1.Value > (int)numericUpDown2.Value)
            {
                MessageBox.Show("Min dimension number can't be larger then Max dimension number!");
                _isInputCorrect = false;
            }

            if (_isInputCorrect == true)
            {
                RandSettStor.MinValue = Convert.ToInt32(textBox1.Text);
                RandSettStor.MaxValue = Convert.ToInt32(textBox2.Text);
                RandSettStor.IsDoubleValue = checkBox1.Checked;
                RandSettStor.RatioOfZero = Convert.ToInt32(textBox3.Text);
                RandSettStor.MinDimension = (int)numericUpDown1.Value;
                RandSettStor.MaxDimension = (int)numericUpDown2.Value;
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
