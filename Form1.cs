using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using WMPLib;

namespace SubComposer
{
    public partial class Form1 : Form
    {
        List<string[]> subs = new List<string[]>();
        int framerate = 0;
        double posmemory = 0.0;
        public string filename = "";
        bool seek = false;
        bool special = false;
        //positsioonimaatriks
        /* 7 8 9
         * 4 5 6
         * 1 2 3
         */
        public Form1()
        {
            InitializeComponent();
        }

        private void suleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            viigeKursorSoovitudVäljaleEtSaadaLisainfotToolStripMenuItem.Text = "Programm sulgub...";
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (filename != "")
            {
                //laadi subtiiter
                this.Text = "Sub composer - " + filename;
            }
        }

        string GetSubRipString()
        {
            string s = "";
            Int64 i = 1;
            foreach (string[] el in subs)
            {
                if (el.Length < 3)
                {
                    continue;
                }
                s += i.ToString() + "\n";
                double hourcode;
                double minutecode;
                double secondcode;
                double mscode;
                string onecode = "";
                int i1 = Convert.ToInt32(el[0]) / framerate;
                hourcode = i1 / 60 / 60;
                minutecode = (i1 / 60) - (hourcode * 60);
                secondcode = i1 - (hourcode * 60 * 60) - (minutecode * 60);
                mscode = Convert.ToInt32(el[0]) % framerate;
                onecode += FixFormat(Math.Round(hourcode, 0).ToString()) + ":" + FixFormat(Math.Round(minutecode, 0).ToString()) + ":" + FixFormat(Math.Round(secondcode, 0).ToString()) + "," + FixMSFormat(Math.Round(mscode, 2).ToString()) + " --> ";
                i1 = Convert.ToInt32(el[1]) / framerate;
                hourcode = i1 / 60 / 60;
                minutecode = (i1 / 60) - (hourcode * 60);
                secondcode = i1 - (hourcode * 60 * 60) - (minutecode * 60);
                mscode = Convert.ToInt32(el[0]) % framerate;
                onecode += FixFormat(Math.Round(hourcode, 0).ToString()) + ":" + FixFormat(Math.Round(minutecode, 0).ToString()) + ":" + FixFormat(Math.Round(secondcode, 0).ToString()) + "," + FixMSFormat(Math.Round(mscode, 2).ToString()) + "\n";
                i++;
                s += onecode;
                s += el[2].ToString() + "\n\n";
            }
            return s;
        }

        string FixFormat(string fixable)
        {
            string s = "";
            int checkable = Convert.ToInt32(fixable);
            if (checkable < 10)
            {
                s = "0" + fixable;
            } else
            {
                s = fixable;
            }
            return s;
        }

        string FixMSFormat(string fixable)
        {
            string s = "";
            int checkable = Convert.ToInt32(fixable);
            if (checkable < 10)
            {
                s = fixable + "00";
            }
            else if ((checkable < 100) && (checkable > 9))
            {
                s = fixable + "0";
            } else
            {
                s = fixable;
            }
            return s;
        }

        void AddSub(int startframe, int endframe, string content)
        {
            if (!this.Text.EndsWith("*"))
            {
                this.Text = this.Text + "*";
            }
            string[] frms = { startframe.ToString(), endframe.ToString(), content };
            subs.Add(frms);
            vScrollBar1.Maximum += 1;
            vScrollBar1.LargeChange = 4;
            UpdateScroll();
        }

        void UpdateScroll()
        {
            int idx = 0;
            for (int i = vScrollBar1.Value; i < vScrollBar1.Value + 4; i++)
            {
                idx++;
                if (i > vScrollBar1.Maximum)
                {
                    foreach(Control c in flowLayoutPanel1.Controls)
                    {
                        if (c is Panel)
                        {
                            foreach (Control ctrl in c.Controls)
                            {
                                if (ctrl.Name == "SubTime" + idx.ToString())
                                {
                                    ctrl.Text = "<lisa subtiitreid>";
                                }
                                if (ctrl.Name == "SubContent" + idx.ToString())
                                {
                                    ctrl.Text = "";
                                }
                            }
                        }
                    }
                    continue;
                }


                foreach (Control c in flowLayoutPanel1.Controls)
                {
                    if (c is Panel) { 
                        foreach (Control ctrl in c.Controls)
                        {
                            if (ctrl.Name == "SubTime" + idx.ToString())
                            {
                                if (subs.Count > i)
                                {
                                    ctrl.Text = subs[i][0].ToString() + " - " + subs[i][1].ToString();
                                }
                                else
                                {
                                    ctrl.Text = "<lisa subtiitreid>";
                                }
                            }
                            if (ctrl.Name == "SubContent" + idx.ToString())
                            {
                                if (subs.Count > i)
                                {
                                    ctrl.Text = subs[i][2];
                                    ctrl.Enabled = true;
                                }
                                else
                                {
                                    ctrl.Text = "";
                                    ctrl.Enabled = false;
                                }
                        }
                        }
                    }
                }
                continue;

            }
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            UpdateScroll();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "")
            {
                label2.Visible = true;
            }
            else
            {
                label2.Visible = false;
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Enter)
            {
                if (kuvaAjakoodiAsemelKaadridToolStripMenuItem.Checked == true)
                { 
                    AddSub(Convert.ToInt32(textBox2.Text), Convert.ToInt32(textBox3.Text), textBox1.Text);
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    UpdateScroll();
                }
                else
                {
                    AddSub(Convert.ToInt32(textBox2.Text.Split(':')[0]) * 60 * framerate + Convert.ToInt32(textBox2.Text.Split(':')[1]) * framerate, Convert.ToInt32(textBox3.Text.Split(':')[0]) * 60 * framerate + Convert.ToInt32(textBox3.Text.Split(':')[1]) * framerate, textBox1.Text);
                }
                toolStripMenuItem1.Text = "Praegune asukoht: All";
            }
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateStats();
        }

        void UpdateStats()
        {
            if (axWindowsMediaPlayer1.currentMedia.getItemInfo("Framerate") != "")
            {
                framerate = Convert.ToInt32(Math.Round((Convert.ToDouble(axWindowsMediaPlayer1.currentMedia.getItemInfo("Framerate")) / 1000), 0));
            }
            trackBar1.Maximum = Convert.ToInt32(axWindowsMediaPlayer1.currentMedia.duration);
            //label3.Text = "Kaader: " + GetCurrentFrame(axWindowsMediaPlayer1.Ctlcontrols.currentPositionString, framerate).ToString() + "/" + GetCurrentFrame(axWindowsMediaPlayer1.currentMedia.durationString, framerate).ToString() + "\nKaadrisagedus: " + framerate.ToString() + " k/s";
            label3.Text = "Kaader: " + GetCurrentFrameFromDouble(axWindowsMediaPlayer1.Ctlcontrols.currentPosition, framerate).ToString() + "/" + GetCurrentFrameFromDouble(axWindowsMediaPlayer1.currentMedia.duration, framerate).ToString() + "\nKaadrisagedus: " + framerate.ToString() + " k/s";
            TimecodeLabel.Text = axWindowsMediaPlayer1.Ctlcontrols.currentPositionString + "/" + axWindowsMediaPlayer1.currentMedia.durationString;
            trackBar1.Value = Convert.ToInt32(axWindowsMediaPlayer1.Ctlcontrols.currentPosition);
        }

        int GetCurrentFrame(string currentPosString, int fps)
        {
            if (currentPosString == "") { return 0; }
            int o = 0;
            currentPosString = currentPosString.Replace(":0", ":");
            string[] ps = currentPosString.Split(':');
            int multiple = 1;
            for (int i = ps.Length - 1; i >= 0; i--)
            {
                o += multiple * (Convert.ToInt32(ps[i].ToString()) * fps);
                multiple *= 60;
            }
            return o;
        }

        int GetCurrentFrameFromDouble(double currentPos, int fps)
        {
            int o = 0;
            o = Convert.ToInt32(currentPos * Convert.ToDouble(fps));
            return o;
        }

        private void trackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.currentPosition = trackBar1.Value;
            axWindowsMediaPlayer1.Ctlcontrols.play();
            axWindowsMediaPlayer1.Ctlcontrols.pause();
            UpdateStats();
            seek = false;
        }

        private void avaVideofailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            viigeKursorSoovitudVäljaleEtSaadaLisainfotToolStripMenuItem.Text = "Videofaili laadimine...";
            if (openVidDialog.ShowDialog() == DialogResult.OK)
            {
                axWindowsMediaPlayer1.URL = openVidDialog.FileName;
                axWindowsMediaPlayer1.PlayStateChange += new AxWMPLib._WMPOCXEvents_PlayStateChangeEventHandler(awpsc);
                timer1.Enabled = true;
                button1.Enabled = true;
                button2.Enabled = true;
                button3.Enabled = true;
                button4.Enabled = true;
                button5.Enabled = true;
                label3.Visible = true;
                trackBar1.Enabled = true;
                TimecodeLabel.Visible = true;
                salvestaSubtiitrifailToolStripMenuItem.Enabled = false;
                avaSubtiitrifailToolStripMenuItem.Enabled = true;
            }
            viigeKursorSoovitudVäljaleEtSaadaLisainfotToolStripMenuItem.Text = "Viige kursor soovitud objektile, et saada lisainfot";
        }

        private void awpsc(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            switch (e.newState)
            {
                case 3:
                    if (axWindowsMediaPlayer1.network.frameRate != 0)
                    {
                        framerate = Convert.ToInt32(Math.Round((Convert.ToDouble(axWindowsMediaPlayer1.currentMedia.getItemInfo("Framerate")) / 1000), 0));
                    }
                    break;
                default:
                    if (axWindowsMediaPlayer1.network.frameRate != 0)
                    {
                        framerate = Convert.ToInt32(Math.Round((Convert.ToDouble(axWindowsMediaPlayer1.currentMedia.getItemInfo("Framerate")) / 1000), 0));
                    }
                    break;
            }
        }

        private void trackBar1_MouseDown(object sender, MouseEventArgs e)
        {
            seek = true;
        }


        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "")
            {
                label2.Visible = true;
            }
            else
            {
                label2.Visible = false;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "")
            {
                label2.Visible = true;
            }
            else
            {
                label2.Visible = false;
            }
        }

        private void SubContent1_TextChanged(object sender, EventArgs e)
        {
            int add = 0;
            if (subs.Count > vScrollBar1.Value + add)
            { 
                string[] ss = subs[vScrollBar1.Value + add];
                string time1 = ss[0];
                string time2 = ss[1];
                string text = SubContent1.Text;
                string[] ns = { time1, time2, text};
                subs[vScrollBar1.Value + add] = ns;
            }
        }

        private void SubContent2_TextChanged(object sender, EventArgs e)
        {
            int add = 1;
            if (subs.Count > vScrollBar1.Value + add)
            {
                string[] ss = subs[vScrollBar1.Value + add];
                string time1 = ss[0];
                string time2 = ss[1];
                string text = SubContent2.Text;
                string[] ns = { time1, time2, text };
                subs[vScrollBar1.Value + add] = ns;
            }
        }

        private void SubContent3_TextChanged(object sender, EventArgs e)
        {
            int add = 2;
            if (subs.Count > vScrollBar1.Value + add)
            {
                string[] ss = subs[vScrollBar1.Value + add];
                string time1 = ss[0];
                string time2 = ss[1];
                string text = SubContent3.Text;
                string[] ns = { time1, time2, text };
                subs[vScrollBar1.Value + add] = ns;
            }
        }

        private void SubContent4_TextChanged(object sender, EventArgs e)
        {
            int add = 3;
            if (subs.Count > vScrollBar1.Value + add)
            {
                string[] ss = subs[vScrollBar1.Value + add];
                string time1 = ss[0];
                string time2 = ss[1];
                string text = SubContent4.Text;
                string[] ns = { time1, time2, text };
                subs[vScrollBar1.Value + add] = ns;
            }
        }

        private void vormindaKursiiviksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.SelectedText = "<i>" + textBox1.SelectedText + "</i>";
        }

        private void vormindaPaksuksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.SelectedText = "<b>" + textBox1.SelectedText + "</b>";
        }

        private void vormindaLäbikriipsutatuksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.SelectedText = "<u>" + textBox1.SelectedText + "</u>";
        }

        private void liigutaÜlesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int c = GetPos();
            int og = c;
            if (c == 0) { c = 8; }
            c += 3;
            if (c > 9) { c -= 3; }
            if (textBox1.Text.Contains("{\\an" + og.ToString() + "}"))
            {
                textBox1.Text = textBox1.Text.Replace("{\\an" + og.ToString() + "}", "{\\an" + c.ToString() + "}");
            } else
            {
                textBox1.Text = "{\\an" + c.ToString() + "}" + textBox1.Text;
            }
            DisPos(c);
        }

        void DisPos(int x)
        {
            string s = "";
            if (x == 1) { s = "All vasakus nurgas"; }
            if (x == 2) { s = "All"; }
            if (x == 3) { s = "All paremas nurgas"; }
            if (x == 4) { s = "Keskel vasakus ääres"; }
            if (x == 5) { s = "Keskel"; }
            if (x == 6) { s = "Keskel paremas ääres"; }
            if (x == 7) { s = "Üleval vasakus nurgas"; }
            if (x == 8) { s = "Üleval"; }
            if (x == 9) { s = "Üleval paremas nurgas"; }
            toolStripMenuItem1.Text = "Praegune asukoht: " + s;
        }

        int GetPos()
        {

            int pos = 0;
            if (textBox1.Text.Contains("{\\an"))
            { 
                pos = Convert.ToInt32(textBox1.Text.Split('}')[0].Replace("{\\an", ""));
            }
            return pos;
        }

        private void liigutaAllaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int c = GetPos();
            int og = c;
            if (c == 0) { c = 5; }
            c -= 3;
            if (c < 1) { c += 3; }
            if (textBox1.Text.Contains("{\\an" + og.ToString() + "}"))
            {
                textBox1.Text = textBox1.Text.Replace("{\\an" + og.ToString() + "}", "{\\an" + c.ToString() + "}");
            }
            else
            {
                textBox1.Text = "{\\an" + c.ToString() + "}" + textBox1.Text;
            }
            DisPos(c);
        }

        private void liigutaVasakuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int c = GetPos();
            int og = c;
            if (c == 0) { c = 8; }
            c -= 1;
            if (c < 1) { c += 1; }
            //lisatingimused, mis kindlustavad selle, et subtiiter ei oleks järsku vales asukohas
            if (c == 3) { c += 1; }
            if (c == 6) { c += 1; }
            if (textBox1.Text.Contains("{\\an" + og.ToString() + "}"))
            {
                textBox1.Text = textBox1.Text.Replace("{\\an" + og.ToString() + "}", "{\\an" + c.ToString() + "}");
            }
            else
            {
                textBox1.Text = "{\\an" + c.ToString() + "}" + textBox1.Text;
            }
            DisPos(c);
        }

        private void liigutaParemaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int c = GetPos();
            int og = c;
            if (c == 0) { c = 8; }
            c += 1;
            if (c > 9) { c -= 1; }
            //lisatingimused, mis kindlustavad selle, et subtiiter ei oleks järsku vales asukohas
            if (c == 4) { c -= 1; }
            if (c == 7) { c -= 1; }
            if (textBox1.Text.Contains("{\\an" + og.ToString() + "}"))
            {
                textBox1.Text = textBox1.Text.Replace("{\\an" + og.ToString() + "}", "{\\an" + c.ToString() + "}");
            }
            else
            {
                textBox1.Text = "{\\an" + c.ToString() + "}" + textBox1.Text;
            }
            DisPos(c);
        }

        private void juhendToolStripMenuItem_MouseMove(object sender, MouseEventArgs e)
        {
            viigeKursorSoovitudVäljaleEtSaadaLisainfotToolStripMenuItem.Text = juhendToolStripMenuItem.Tag.ToString();
        }

        private void spikkerToolStripMenuItem_DropDownClosed(object sender, EventArgs e)
        {
            viigeKursorSoovitudVäljaleEtSaadaLisainfotToolStripMenuItem.Text = "Viige kursor soovitud objekti juurde, et saada lisainfot";
        }

        private void muudaToolStripMenuItem_DropDownClosed(object sender, EventArgs e)
        {
            viigeKursorSoovitudVäljaleEtSaadaLisainfotToolStripMenuItem.Text = "Viige kursor soovitud objekti juurde, et saada lisainfot";
        }

        private void failToolStripMenuItem_DropDownClosed(object sender, EventArgs e)
        {
            viigeKursorSoovitudVäljaleEtSaadaLisainfotToolStripMenuItem.Text = "Viige kursor soovitud objekti juurde, et saada lisainfot";
        }

        void ShowInfo(ToolStripMenuItem tsmi)
        {
            viigeKursorSoovitudVäljaleEtSaadaLisainfotToolStripMenuItem.Text = tsmi.Tag.ToString() ;
        }

        private void teaveToolStripMenuItem_MouseMove(object sender, MouseEventArgs e)
        {
            ShowInfo(teaveToolStripMenuItem);
        }

        private void kuvaAjakoodiAsemelKaadridToolStripMenuItem_MouseMove(object sender, MouseEventArgs e)
        {
            ShowInfo(kuvaAjakoodiAsemelKaadridToolStripMenuItem);
        }

        private void vormindaKursiiviksToolStripMenuItem_MouseMove(object sender, MouseEventArgs e)
        {
            ShowInfo(vormindaKursiiviksToolStripMenuItem);
        }

        private void vormindaPaksuksToolStripMenuItem_MouseMove(object sender, MouseEventArgs e)
        {
            ShowInfo(vormindaPaksuksToolStripMenuItem);
        }

        private void vormindaLäbikriipsutatuksToolStripMenuItem_MouseMove(object sender, MouseEventArgs e)
        {
            ShowInfo(vormindaLäbikriipsutatuksToolStripMenuItem);
        }

        private void liigutaÜlesToolStripMenuItem_MouseMove(object sender, MouseEventArgs e)
        {
            ShowInfo(liigutaÜlesToolStripMenuItem);
        }

        private void liigutaAllaToolStripMenuItem_MouseMove(object sender, MouseEventArgs e)
        {
            ShowInfo(liigutaAllaToolStripMenuItem);
        }

        private void liigutaVasakuleToolStripMenuItem_MouseMove(object sender, MouseEventArgs e)
        {
            ShowInfo(liigutaVasakuleToolStripMenuItem);
        }

        private void liigutaParemaleToolStripMenuItem_MouseMove(object sender, MouseEventArgs e)
        {
            ShowInfo(liigutaParemaleToolStripMenuItem);
        }

        private void ajastamineToolStripMenuItem_MouseMove(object sender, MouseEventArgs e)
        {
            ShowInfo(ajastamineToolStripMenuItem);
        }

        private void avaVideofailToolStripMenuItem_MouseMove(object sender, MouseEventArgs e)
        {
            ShowInfo(avaVideofailToolStripMenuItem);
        }

        private void avaSubtiitrifailToolStripMenuItem_MouseMove(object sender, MouseEventArgs e)
        {
            ShowInfo(avaSubtiitrifailToolStripMenuItem);
        }

        private void salvestaSubtiitrifailKuiToolStripMenuItem_MouseMove(object sender, MouseEventArgs e)
        {
            ShowInfo(salvestaSubtiitrifailKuiToolStripMenuItem);
        }

        private void salvestaSubtiitrifailToolStripMenuItem_MouseMove(object sender, MouseEventArgs e)
        {
            ShowInfo(salvestaSubtiitrifailToolStripMenuItem);
        }

        private void suleToolStripMenuItem_MouseMove(object sender, MouseEventArgs e)
        {
            ShowInfo(suleToolStripMenuItem);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            seek = true;
            trackBar1.Value = Convert.ToInt32(posmemory);
            axWindowsMediaPlayer1.Ctlcontrols.currentPosition = trackBar1.Value;
            axWindowsMediaPlayer1.Ctlcontrols.play();
            timer1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            posmemory = trackBar1.Value;
            axWindowsMediaPlayer1.Ctlcontrols.pause();
            timer1.Enabled = false;
        }
        
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (seek)
            {
                axWindowsMediaPlayer1.Ctlcontrols.currentPosition = trackBar1.Value;
                axWindowsMediaPlayer1.Ctlcontrols.play();
                axWindowsMediaPlayer1.Ctlcontrols.pause();
                posmemory = trackBar1.Value;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            trackBar1.Value = Convert.ToInt32(posmemory);
            axWindowsMediaPlayer1.Ctlcontrols.currentPosition = trackBar1.Value;
            axWindowsMediaPlayer1.Ctlcontrols.pause();
            timer1.Enabled = false;
            UpdateStats();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            special = true;
            if (trackBar1.Value != trackBar1.Minimum + 1)
            { 
                trackBar1.Value -= 1;
            }
            special = false;
            posmemory = axWindowsMediaPlayer1.Ctlcontrols.currentPosition;
            UpdateStats();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            ((IWMPControls2)axWindowsMediaPlayer1.Ctlcontrols).step(1);
            posmemory = axWindowsMediaPlayer1.Ctlcontrols.currentPosition;
            UpdateStats();
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            if (special)
            {
                axWindowsMediaPlayer1.Ctlcontrols.currentPosition = trackBar1.Value;
                axWindowsMediaPlayer1.Ctlcontrols.play();
                axWindowsMediaPlayer1.Ctlcontrols.pause();
                posmemory = trackBar1.Value;
            }
        }

        private void button1_MouseMove(object sender, MouseEventArgs e)
        {
            viigeKursorSoovitudVäljaleEtSaadaLisainfotToolStripMenuItem.Text = "Esita (kui video juba mängib, taasesitatakse alustatud klipp)";
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            viigeKursorSoovitudVäljaleEtSaadaLisainfotToolStripMenuItem.Text = "Viige kursor soovitud objekti juurde, et saada lisainfot";
        }

        private void button2_MouseMove(object sender, MouseEventArgs e)
        {
            viigeKursorSoovitudVäljaleEtSaadaLisainfotToolStripMenuItem.Text = "Peata video praeguses kaadris";
        }

        private void button3_MouseMove(object sender, MouseEventArgs e)
        {
            viigeKursorSoovitudVäljaleEtSaadaLisainfotToolStripMenuItem.Text = "Peata video ning keri tagasi klipi algusesse";
        }

        private void button4_MouseMove(object sender, MouseEventArgs e)
        {
            viigeKursorSoovitudVäljaleEtSaadaLisainfotToolStripMenuItem.Text = "Keri video tagasi 1 s";
        }

        private void button5_MouseMove(object sender, MouseEventArgs e)
        {
            viigeKursorSoovitudVäljaleEtSaadaLisainfotToolStripMenuItem.Text = "Liigu edasi üks kaader korraga";
        }

        private void trackBar1_MouseMove(object sender, MouseEventArgs e)
        {
            viigeKursorSoovitudVäljaleEtSaadaLisainfotToolStripMenuItem.Text = "Liigu soovitud asukohale videos";
        }

        private void textBox2_MouseMove(object sender, MouseEventArgs e)
        {
            viigeKursorSoovitudVäljaleEtSaadaLisainfotToolStripMenuItem.Text = "Subtiitri algusaeg (vajutage Ctrl + P, et sisestada praegune ajakood/kaader)";
        }

        private void textBox3_MouseMove(object sender, MouseEventArgs e)
        {
            viigeKursorSoovitudVäljaleEtSaadaLisainfotToolStripMenuItem.Text = "Subtiitri lõpuaeg (vajutage Ctrl + P, et sisestada praegune ajakood/kaader)";
        }

        private void textBox1_MouseMove(object sender, MouseEventArgs e)
        {
            viigeKursorSoovitudVäljaleEtSaadaLisainfotToolStripMenuItem.Text = "Subtiiter";
        }

        private void SubContent1_MouseMove(object sender, MouseEventArgs e)
        {
            viigeKursorSoovitudVäljaleEtSaadaLisainfotToolStripMenuItem.Text = "See on lisatud subtiiter, mida on võimalik muuta.";
        }

        private void SubContent1_MouseLeave(object sender, EventArgs e)
        {
            viigeKursorSoovitudVäljaleEtSaadaLisainfotToolStripMenuItem.Text = "Viige kursor soovitud objekti juurde, et saada lisainfot";
        }

        private void trackBar1_MouseLeave(object sender, EventArgs e)
        {
            viigeKursorSoovitudVäljaleEtSaadaLisainfotToolStripMenuItem.Text = "Viige kursor soovitud objekti juurde, et saada lisainfot";
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Control && e.KeyCode == Keys.P)
            {
                if (kuvaAjakoodiAsemelKaadridToolStripMenuItem.Checked == true)
                {
                    textBox2.Text = Math.Round(framerate * axWindowsMediaPlayer1.Ctlcontrols.currentPosition, 0).ToString();
                }
                else
                {
                    textBox2.Text = TimecodeLabel.Text.Split('/')[0].ToString();
                }
            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.P)
            {
                if (kuvaAjakoodiAsemelKaadridToolStripMenuItem.Checked == true)
                {
                    textBox3.Text = Math.Round(framerate * axWindowsMediaPlayer1.Ctlcontrols.currentPosition, 0).ToString();
                }
                else
                {
                    textBox3.Text = TimecodeLabel.Text.Split('/')[0].ToString();
                }
            }
        }

        private void salvestaSubtiitrifailKuiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            viigeKursorSoovitudVäljaleEtSaadaLisainfotToolStripMenuItem.Text = "Salvestan...";
            if (subSaveDialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(subSaveDialog.FileName, GetSubRipString(), Encoding.UTF8);
                string bck = subOpenDialog.FileName;
                subOpenDialog.FileName = subSaveDialog.FileName;
                this.Text = "Sub composer - " + subOpenDialog.SafeFileName;
                subOpenDialog.FileName = bck;
                salvestaSubtiitrifailToolStripMenuItem.Enabled = true;
            }
            viigeKursorSoovitudVäljaleEtSaadaLisainfotToolStripMenuItem.Text = "Viige kursor soovitud objekti juurde, et saada lisainfot";
        }

        private void salvestaSubtiitrifailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            viigeKursorSoovitudVäljaleEtSaadaLisainfotToolStripMenuItem.Text = "Salvestan...";
            File.WriteAllText(subSaveDialog.FileName, GetSubRipString(), Encoding.UTF8);
            string bck = subOpenDialog.FileName;
            subOpenDialog.FileName = subSaveDialog.FileName;
            this.Text = "Sub composer - " + subOpenDialog.SafeFileName;
            subOpenDialog.FileName = bck;
            viigeKursorSoovitudVäljaleEtSaadaLisainfotToolStripMenuItem.Text = "Viige kursor soovitud objekti juurde, et saada lisainfot";
        }

        private void avaSubtiitrifailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            viigeKursorSoovitudVäljaleEtSaadaLisainfotToolStripMenuItem.Text = "Subtiitri laadimine...";
            if (subOpenDialog.ShowDialog() == DialogResult.OK)
            {
                string[] text = File.ReadAllLines(subOpenDialog.FileName, Encoding.UTF8);
                string subcontent = "";
                string substart = "";
                string subend = "";
                foreach (string line in text)
                {
                    if (line.Contains(" --> "))
                    {
                        string[] timings = line.Replace(" --> ", "\t").Split('\t');
                        substart = GetFrameFromCode(timings[0].ToString());
                        subend = GetFrameFromCode(timings[1].ToString());
                        continue;
                    }
                    // kui rida on tühi, siis lisatakse uus subtiiter
                    if (line == "")
                    {
                        AddSub(Convert.ToInt32(substart), Convert.ToInt32(subend), subcontent);
                        subcontent = "";
                        substart = "";
                        subend = "";
                    } else
                    {
                        // see if lause kontrollib, kas tegu on numbriga
                        // kui tegu on numbriga, siis ignoreeritakse seda rida ja liigutakse edasi.
                        if (!int.TryParse(line, out int x))
                        { 
                            if (subcontent == "")
                            {
                                subcontent = line;
                            } else
                            {
                                subcontent += "\n" + line;
                            }
                        }
                    }
                }
                UpdateScroll();
                salvestaSubtiitrifailToolStripMenuItem.Enabled = true;
                subSaveDialog.FileName = subOpenDialog.FileName;
                this.Text = "Sub composer - " + subOpenDialog.SafeFileName;
            }
            viigeKursorSoovitudVäljaleEtSaadaLisainfotToolStripMenuItem.Text = "Viige kursor soovitud objekti juurde, et saada lisainfot";
        }

        private void vormindaVärviksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                string hexcolor = "#" + FormatHex(colorDialog1.Color.R.ToString("X")) + FormatHex(colorDialog1.Color.G.ToString("X")) + FormatHex(colorDialog1.Color.B.ToString("X"));
                textBox1.SelectedText = "<font color=\"" + hexcolor + "\">" + textBox1.SelectedText + "</font>";
            }
        }

        string GetFrameFromCode(string code)
        {
            string s = "";
            long total = 0;
            string[] timeandms = code.Split(',');
            int ms = Convert.ToInt32(FixStringFormat(timeandms[1]));
            total += ms;
            List<string> realcodes = new List<string>();
            realcodes.AddRange(timeandms[0].ToString().Split(':'));
            for (int x = 0; x < realcodes.Count; x++)
            {
                realcodes[x] = FixStringFormat(realcodes[x]);
            }
            string temp;
            temp = realcodes[0].ToString();
            int hours = Convert.ToInt32(temp);
            temp = realcodes[1].ToString();
            int mins = Convert.ToInt32(temp);
            temp = realcodes[2].ToString();
            int secs = Convert.ToInt32(temp);
            total += secs * framerate;
            total += mins * framerate * 60;
            total += hours * framerate * 60 * 60;
            s = total.ToString();
            return s;
        }
        string FixStringFormat(string origin)
        {
            string s = origin;
            if (s == "00")
            {
                s = "0";
                return s;
            }
            if (s.StartsWith("00"))
            {
                s = s.Replace("00", "");
                return s;
            } else if (s.StartsWith("0"))
            {
                s = s.Substring(1, s.Length - 1);
            }
            return s;
        }
        string FormatHex(string origin)
        {
            string s = "";
            if (origin.Length < 2)
            {
                s = "0" + origin.ToLower();
            }
            else
            {
                s = origin.ToLower();
            }
            return s;
        }

        private void SubContent1_TextChanged_1(object sender, EventArgs e)
        {
            if (!Text.EndsWith("*"))
            {
                Text = Text + "*";
            }
        }

        private void ajastamineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TimeAdjustment ta = new TimeAdjustment();
            ta.framerate = this.framerate;
            if (ta.ShowDialog() == DialogResult.OK)
            {
                ta.label3.Text = "1 sek = " + ta.framerate + " kaadrit";
                int shift = ta.shift;
                for (int i = 0; i < subs.Count; i++)
                {
                    long long1 = Convert.ToInt64(subs[i][0]);
                    long long2 = Convert.ToInt64(subs[i][1]);
                    string contents = subs[i][2];
                    long1 += shift;
                    long2 += shift;
                    string[] modsub = { long1.ToString(), long2.ToString(), contents };
                    subs[i] = modsub;
                }
                UpdateScroll();
            }
            ta.Dispose();
        }

        private void muudaToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            if (subs.Count > 0)
            {
                ajastamineToolStripMenuItem.Enabled = true;
            } else
            {
                ajastamineToolStripMenuItem.Enabled = false;
            }
        }

        private void SubTime1_DoubleClick(object sender, EventArgs e)
        {
            if (SubContent1.Enabled == true)
            {
                int idx = vScrollBar1.Value;
                ModifySub(idx);
            }
        }

        private void SubTime2_DoubleClick(object sender, EventArgs e)
        {
            if (SubContent2.Enabled == true)
            {
                int idx = vScrollBar1.Value + 1;
                ModifySub(idx);
            }
        }

        void ModifySub(int idx)
        {
            string[] currentsub = subs[idx];
            SubModify sm = new SubModify();
            sm.framecode1.Text = currentsub[0];
            sm.framecode2.Text = currentsub[1];
            sm.subContent.Text = currentsub[2];
            sm.current1 = Convert.ToInt64(axWindowsMediaPlayer1.Ctlcontrols.currentPosition * framerate);
            if (sm.ShowDialog() == DialogResult.OK)
            {
                string[] newsub = { sm.framecode1.Text, sm.framecode2.Text, sm.subContent.Text };
                subs[idx] = newsub;
                UpdateScroll();
            }
            sm.Dispose();
        }

        private void SubTime3_DoubleClick(object sender, EventArgs e)
        {
            if (SubContent2.Enabled == true)
            {
                int idx = vScrollBar1.Value + 2;
                ModifySub(idx);
            }
        }

        private void SubTime4_DoubleClick(object sender, EventArgs e)
        {
            if (SubContent2.Enabled == true)
            {
                int idx = vScrollBar1.Value + 3;
                ModifySub(idx);
            }
        }

        private void teaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 a = new AboutBox1();
            a.ShowDialog();
            a.Dispose();
        }

        private void juhendToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Pole veel saadaval");
        }
    }
}
