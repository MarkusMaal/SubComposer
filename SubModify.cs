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
    public partial class SubModify : Form
    {
        public long current1 = 0;
        public SubModify()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            subContent.SelectedText = "<b>" + subContent.SelectedText + "</b>";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            subContent.SelectedText = "<i>" + subContent.SelectedText + "</i>";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            subContent.SelectedText = "<u>" + subContent.SelectedText + "</u>";
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton8.Checked == true)
            {
                if (subContent.Text.StartsWith("{\\an")) { 
                    for (int i = 1; i < 10; i++)
                    {
                        subContent.Text = subContent.Text.Replace("{\\an" + i.ToString() + "}", "");
                    }
                }
                subContent.Text = "{\\an2}" + subContent.Text;
            }
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton7.Checked == true)
            {
                if (subContent.Text.StartsWith("{\\an"))
                {
                    for (int i = 1; i < 10; i++)
                    {
                        subContent.Text = subContent.Text.Replace("{\\an" + i.ToString() + "}", "");
                    }
                }
                subContent.Text = "{\\an1}" + subContent.Text;
            }
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton9.Checked == true)
            {
                if (subContent.Text.StartsWith("{\\an"))
                {
                    for (int i = 1; i < 10; i++)
                    {
                        subContent.Text = subContent.Text.Replace("{\\an" + i.ToString() + "}", "");
                    }
                }
                subContent.Text = "{\\an3}" + subContent.Text;
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked == true)
            {
                if (subContent.Text.StartsWith("{\\an"))
                {
                    for (int i = 1; i < 10; i++)
                    {
                        subContent.Text = subContent.Text.Replace("{\\an" + i.ToString() + "}", "");
                    }
                }
                subContent.Text = "{\\an4}" + subContent.Text;
            }
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton5.Checked == true)
            {
                if (subContent.Text.StartsWith("{\\an"))
                {
                    for (int i = 1; i < 10; i++)
                    {
                        subContent.Text = subContent.Text.Replace("{\\an" + i.ToString() + "}", "");
                    }
                }
                subContent.Text = "{\\an5}" + subContent.Text;
            }
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton6.Checked == true)
            {
                if (subContent.Text.StartsWith("{\\an"))
                {
                    for (int i = 1; i < 10; i++)
                    {
                        subContent.Text = subContent.Text.Replace("{\\an" + i.ToString() + "}", "");
                    }
                }
                subContent.Text = "{\\an6}" + subContent.Text;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                if (subContent.Text.StartsWith("{\\an"))
                {
                    for (int i = 1; i < 10; i++)
                    {
                        subContent.Text = subContent.Text.Replace("{\\an" + i.ToString() + "}", "");
                    }
                }
                subContent.Text = "{\\an7}" + subContent.Text;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                if (subContent.Text.StartsWith("{\\an"))
                {
                    for (int i = 1; i < 10; i++)
                    {
                        subContent.Text = subContent.Text.Replace("{\\an" + i.ToString() + "}", "");
                    }
                }
                subContent.Text = "{\\an8}" + subContent.Text;
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked == true)
            {
                if (subContent.Text.StartsWith("{\\an"))
                {
                    for (int i = 1; i < 10; i++)
                    {
                        subContent.Text = subContent.Text.Replace("{\\an" + i.ToString() + "}", "");
                    }
                }
                subContent.Text = "{\\an9}" + subContent.Text;
            }
        }

        private void SubModify_Load(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void framecode1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && (e.KeyCode == Keys.P))
            {
                framecode1.Text = current1.ToString();
            }
        }

        private void framecode2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && (e.KeyCode == Keys.P))
            {
                framecode2.Text = current1.ToString();
            }
        }

        private void subContent_TextChanged(object sender, EventArgs e)
        {
            if(subContent.Text != "")
            {
                if (framecode1.Text != "")
                {
                    if (framecode2.Text != "")
                    {
                        button4.Enabled = true;
                    } else
                    {
                        button4.Enabled = false;
                    }
                }
                else
                {
                    button4.Enabled = false;
                }
            }
            else
            {
                button4.Enabled = false;
            }
        }

        private void subContent_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && (e.KeyCode == Keys.B))
            {
                button1.PerformClick();
            }
            else if (e.Control && (e.KeyCode == Keys.I))
            {
                button2.PerformClick();
            }
            else if (e.Control && (e.KeyCode == Keys.U))
            {
                button3.PerformClick();
            }
        }
    }
}
