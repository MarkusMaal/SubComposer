using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SubComposer
{
    public partial class TimeAdjustment : Form
    {
        public int shift = 0;
        public int framerate = 0;
        public TimeAdjustment()
        {
            InitializeComponent();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Adjust(1);
        }

        void Adjust(int i)
        {
            shift += i;
            label2.Text = shift.ToString() + " kaadrit";
            label3.Text = "1 sek = " + framerate.ToString() + " kaadrit";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Adjust(-1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Adjust(framerate);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Adjust(-framerate);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Adjust(framerate * 60);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Adjust(-(framerate * 60));
        }
    }
}
