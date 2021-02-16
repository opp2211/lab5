using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace lab1
{
    public partial class Form1 : Form
    {
        PassAnalysis pa;
        Brute brute;

        int countPasses = 0;
        public Form1()
        {
            InitializeComponent();
            pa = new PassAnalysis();
            brute = new Brute();

            Refresher();

            brute.OnNewPassWasFound += Brute_NewPassWasFound;
        }

        #region PassAnalysis
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            pa.Password = textBox1.Text;
            Refresher();
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            pa.S = textBox2.Text == "" ? 0 : Convert.ToInt32(textBox2.Text);
            Refresher();
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            pa.M = textBox3.Text == "" ? 0 : Convert.ToInt32(textBox3.Text);
            Refresher();
        }
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            pa.V = textBox4.Text == "" ? 0 : Convert.ToInt32(textBox4.Text);
            Refresher();
        }
        void Refresher()
        {
            label_alphPower.Text = Convert.ToString(pa.AlphPower);
            label_numOp.Text = Convert.ToString(pa.NumOp);
            label_timeOp.Text = pa.TimeOp;
        }
        #endregion

        #region BruteResults


        void Brute_NewPassWasFound(string pass, double speed, TimeSpan time)
        {
            Invoke((MethodInvoker)delegate()
            {
                label13.Text = time.ToString();
                label17.Text = pass;
                label11.Text = (Math.Round(speed*100)/100).ToString() + " p/sec";
            });
            countPasses++;
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            brute.FullBrute_Start(textBox1.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            brute.Stop();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            brute.MaxLength = (int)numericUpDown1.Value;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            brute.Numbers = checkBox1.Checked;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            brute.Symb = checkBox4.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            brute.Lower = checkBox2.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            brute.Upper = checkBox3.Checked;
        }

        private void button_file_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = openFileDialog1.FileName;
            brute.Filename = filename;
            label8.Text = filename.Substring(filename.LastIndexOf("\\"));
        }

        private void button_Start_bfD_Click(object sender, EventArgs e)
        {
            brute.DicBrute_Start(textBox1.Text);
        }
    }
}
