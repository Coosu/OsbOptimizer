﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;

namespace LibOSB
{
    public partial class frmMain : Form
    {
        Stopwatch wow;
        Thread t1;
        private static string before;
        private static string after;

        public frmMain()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;

            y = yy;
            x = xx;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GetSettings();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            root = txtRoot.Text;
            //bool error;
            @start_rdy(out bool error); 
            if (error) return;

            t1 = new Thread(RunOptimizer);
            t1.Start();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Optimizer.sb.Append(after);
            Reporter.OptimizedSpriteNumber++;
            @continue_rdy();
            //status1 = false;
            //status2 = true;
            //timerWindow.Enabled = true;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if ((Optimizer.ifPause || Optimizer.ifPause2 || Optimizer.ifCheck) && Optimizer.Before != null
                && before != Optimizer.Before && Optimizer.Display == true)
            {
                @pause();
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            WriteSettings();
        }
        private void timerProgress_Tick(object sender, EventArgs e)
        {
            progressbar.Value = (int)(Optimizer.Progress * progressbar.Maximum);
            progress.Text = Math.Round(Optimizer.Progress * 100).ToString() + "%";
        }
        private void detectStatus_Tick(object sender, EventArgs e)
        {
            //root = txtRoot.Text;

            if (Optimizer.Finsh == true)
            {
                //MessageBox.Show("Optimization finished. Time: " + wow.Elapsed.ToString(), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (System.IO.Path.GetExtension(root).ToLower() == ".osb")
                    objectroot = root;
                else
                    objectroot = System.IO.Path.GetDirectoryName(root) + @"\" + System.IO.Path.GetFileNameWithoutExtension(root) + ".osb";
                sControls.WriteFile(Optimizer.sb.ToString(), objectroot);
                Optimizer.Finsh = false;
                @finish_rdy(false);
                return;
            }
        }
        private void chkConfirm_CheckedChanged(object sender, EventArgs e)
        {
            Optimizer.ifPause = chkConfirm.Checked;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Optimizer.sb.Append(before);
            @continue_rdy();
            //status1 = false;
            //status2 = true;
            //timerWindow.Enabled = true;
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void button4_Click(object sender, EventArgs e)
        {
            var FileDialog = new OpenFileDialog();
            FileDialog.Filter = "osb file|*.osb|osu file|*.osu|All types|*.*";
            FileDialog.FileName = "";
            FileDialog.CheckFileExists = true;
            FileDialog.ValidateNames = true;
            if (FileDialog.ShowDialog() == DialogResult.OK)
            {
                txtRoot.Text = FileDialog.FileName;
                button1_Click(sender, e);
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox1_LostFocus(object sender, EventArgs e)
        {
            int value;
            try
            {
                if (textBox1.Text.Trim() == "") return;
                value = int.Parse(textBox1.Text);
                Optimizer.Decimal = value;
                WriteSettings();
            }
            catch
            {
                MessageBox.Show("Please enter a valid number", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                textBox1.Focus();
            }
        }

        private void chkConfirm_Click(object sender, EventArgs e)
        {
            Optimizer.ifPause = chkConfirm.Checked;
            WriteSettings();
        }

        private void chkConfirm2_Click(object sender, EventArgs e)
        {
            Optimizer.ifPause2 = chkConfirm2.Checked;
            WriteSettings();
        }

        private void chkConfirm2_CheckedChanged(object sender, EventArgs e)
        {
            Optimizer.ifPause2 = chkConfirm2.Checked;
        }


        [DllImport("kernel32", CharSet = CharSet.Ansi, EntryPoint = "GetPrivateProfileStringA", ExactSpelling = true, SetLastError = true)]
        private static extern int GetPrivateProfileString([MarshalAs(UnmanagedType.VBByRefStr)] ref string lpApplicationName, [MarshalAs(UnmanagedType.VBByRefStr)] ref string lpKeyName, [MarshalAs(UnmanagedType.VBByRefStr)] ref string lpDefault, [MarshalAs(UnmanagedType.VBByRefStr)] ref string lpReturnedString, int nSize, [MarshalAs(UnmanagedType.VBByRefStr)] ref string lpFileName);
        [DllImport("kernel32", CharSet = CharSet.Ansi, EntryPoint = "WritePrivateProfileStringA", ExactSpelling = true, SetLastError = true)]
        private static extern int WritePrivateProfileString([MarshalAs(UnmanagedType.VBByRefStr)] ref string lpApplicationName, [MarshalAs(UnmanagedType.VBByRefStr)] ref string lpKeyName, [MarshalAs(UnmanagedType.VBByRefStr)] ref string lpString, [MarshalAs(UnmanagedType.VBByRefStr)] ref string lpFileName);


        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;

        private static string GetINI(string Section, string AppName, string lpDefault, string FileName)
        {
            string str = null;
            str = Strings.LSet(str, 256);
            GetPrivateProfileString(ref Section, ref AppName, ref lpDefault, ref str, Strings.Len(str), ref FileName);
            return Strings.Left(str, checked(Strings.InStr(str, "\0", CompareMethod.Binary) - 1));
        }
        private static long WriteINI(string Section, string AppName, string lpDefault, string FileName)
        {
            return WritePrivateProfileString(ref Section, ref AppName, ref lpDefault, ref FileName);
        }

        string root = "";
        string objectroot;
        string iniroot = Application.StartupPath + @"\config.ini";
        private void WriteSettings()
        {
            WriteINI("RegularSettings", "ChangeConfirm", chkConfirm.Checked.ToString(), iniroot);
            WriteINI("RegularSettings", "DeleteConfirm", chkConfirm2.Checked.ToString(), iniroot);

            WriteINI("OptimizationSettings", "Decimal", textBox1.Text, iniroot);
            WriteINI("OptimizationSettings", "CheckBeforeOptimize", chkchk.Checked.ToString(), iniroot);

            WriteINI("RootSetting", "BackupFile", sControls.BackupRoot, iniroot);

            WriteINI("InfoSetting", "CollectResult", InfoCollector.IfCollectInfo.ToString(), iniroot);
            WriteINI("InfoSetting", "CollectException", InfoCollector.IfCollectEx.ToString(), iniroot);
        }
        private void GetSettings()
        {
            chkConfirm.Checked = bool.Parse(GetINI("RegularSettings", "ChangeConfirm", "False", iniroot));
            chkConfirm2.Checked = bool.Parse(GetINI("RegularSettings", "DeleteConfirm", "True", iniroot));
            Optimizer.ifPause = chkConfirm.Checked;
            Optimizer.ifPause2 = chkConfirm2.Checked;

            textBox1.Text = GetINI("OptimizationSettings", "Decimal", "3", iniroot);
            chkchk.Checked = bool.Parse(GetINI("OptimizationSettings", "CheckBeforeOptimize", "True", iniroot));
            Optimizer.ifCheck = chkchk.Checked;

            chkinfo.Checked = bool.Parse(GetINI("InfoSetting", "CollectResult", "True", iniroot));
            chkex.Checked = bool.Parse(GetINI("InfoSetting", "CollectException", "True", iniroot));
            InfoCollector.IfCollectInfo = chkinfo.Checked;
            InfoCollector.IfCollectEx = chkex.Checked;


            sControls.BackupRoot = GetINI("RootSetting", "BackupFile", "", iniroot);
            textBox1_LostFocus(null, null);
        }

        private void RunOptimizer()
        {

            wow = new Stopwatch();
            wow.Start();
            try
            {
                lbl_Line1.Text = "Preparing for optimization, please wait...";
                long lines = sControls.GetFileLine(root);
                lbl_Line1.Text = "";
                sControls.Backup(root);
                Optimizer.ReadFile(root, lines);
            }
            catch (ThreadAbortException) { }
            catch (System.IO.FileNotFoundException)
            {

                lbl_Line1.Text = "No such file.";
                @finish_rdy(true);
                t1.Abort();
            }
            catch (Exception ex)
            {
                lbl_Line1.Text = ex.Message;
                if (InfoCollector.IfCollectEx)
                {
                    try { InfoCollector.UploadEx(ex); } catch { }
                }
                //MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //sControls.WriteFile(ex.Message, "Exception.log");

                @finish_rdy(true);
                progressbar.Value = 0;
                progress.Text = "0%";
                t1.Abort();
                GC.Collect();
            }
            wow.Stop();
            //Console.WriteLine(wow.Elapsed);
        }
        private void @start_rdy(out bool error)
        {
            Reporter.ExecTime = DateTime.Now;
            try
            {
                Reporter.SizeBefore = new System.IO.FileInfo(root).Length;
                Reporter.SourceFileRoot = root;
            }
            catch (System.IO.FileNotFoundException)
            {
                lbl_Line1.Text = "No such file.";
                error = true;
                return;
            }
            richTextBox1.Clear();
            richTextBox2.Clear();
            richTextBox1.ScrollBars = ScrollBars.None;
            richTextBox2.ScrollBars = ScrollBars.None;

            txtRoot.Enabled = false;
            button1.Enabled = false;
            button4.Enabled = false;
            timer1.Enabled = true;
            detectStatus.Enabled = true;
            timerProgress.Enabled = true;
            if (status1) timerWindow.Enabled = true;
            error = false;
        }
        private void @finish_rdy(bool error)
        {
            txtRoot.Enabled = true;
            button1.Enabled = true;
            button4.Enabled = true;
            timer1.Enabled = false;
            detectStatus.Enabled = false;
            timerProgress.Enabled = false;
            richTextBox1.ScrollBars = ScrollBars.Vertical;
            richTextBox2.ScrollBars = ScrollBars.Vertical;

            if (!error)
            {
                progressbar.Value = progressbar.Maximum;
                progress.Text = "100%";

                Reporter.RealtotalTime = wow.Elapsed;
                Reporter.EndTime = DateTime.Now;
                Reporter.SizeAfter = new System.IO.FileInfo(objectroot).Length;
                Reporter.ObjectFileRoot = objectroot;
                richTextBox1.Text = Reporter.ToString(false);
                richTextBox2.Text = Reporter.ToString(true);
                lbl_Line1.Text = "Optimization finished.";
                if (InfoCollector.IfCollectInfo)
                {
                    try { InfoCollector.UploadInfo(); } catch { }
                }
            }

            Reporter.Clear();

            if (status2) timerWindow.Enabled = true;
        }
        private void @continue_rdy()
        {
            richTextBox1.Clear();
            richTextBox2.Clear();

            richTextBox1.ScrollBars = ScrollBars.None;
            richTextBox2.ScrollBars = ScrollBars.None;


            button2.Enabled = false;
            button3.Enabled = false;
            lbl_Line1.Text = "";

            before = "";after = "";

            wow.Start();
            Optimizer.Pause = false;
        }
        private void @pause()
        {
            wow.Stop();
            before = Optimizer.Before;
            after = Optimizer.After;
            lbl_Line1.Text = "Found new sprite on line " + Optimizer.CurrentObjLine.ToString();

            richTextBox1.ScrollBars = ScrollBars.Vertical;
            richTextBox2.ScrollBars = ScrollBars.Vertical;

            if (before.Length > 10000) richTextBox1.Text = @"/* Too Long to display. */";
            else richTextBox1.Text = before;
            if (after.Length > 10000) richTextBox2.Text = @"/* Too Long to display. */";
            else if (after == "") richTextBox2.Text = @"/* Unuseful object. Deleted. */";
            else if (after == "IsError") richTextBox2.Text = "/* Exist illogical, conflicting or obsolete commands. */\r\n/* Skip this object. */";
            else
            {
                richTextBox2.Text = after;
                int gg = richTextBox1.Lines.Length - richTextBox2.Lines.Length;
                for (int i = 1; i <= gg; i++) richTextBox2.Text += "{" + i + "}\r\n";
            }
            button2.Enabled = true;
            button3.Enabled = true;
            button2.Focus();

            richTextBox1.ScrollBars = ScrollBars.Vertical;
            richTextBox2.ScrollBars = ScrollBars.Vertical;
        }

        double yy = 9;
        double y;
        double xx = 26;
        double x;

        bool status1 = true, status2 = false;

        private void timerWindow_Tick(object sender, EventArgs e)
        {
            if (status1 && y != 0)
            {
                ToStatus2();
            }
            else if (status1 && y == 0)
            {
                status1 = false;
                status2 = true;
                timerWindow.Enabled = false;
            }
            else if (status2 && y != yy)
            {
                ToStatus1();
            }
            else if (status2 && y == yy)
            {
                status2 = false;
                status1 = true;
                timerWindow.Enabled = false;
            }
        }
        private void ToStatus2()
        {
            this.Height += (int)y;
            y -= 0.5;
        }
        private void ToStatus1()
        {
            this.Height -= (int)(yy - y);
            y += 0.5;
        }

        bool status3 = true, status4 = false;

        private void timerWindowX_Tick(object sender, EventArgs e)
        {
            if (status3 && x != 0)
            {
                button5.Text = "Settings <";
                ToStatus4();
            }
            else if (status3 && x == 0)
            {
                status3 = false;
                status4 = true;
                timerWindowX.Enabled = false;
            }
            else if (status4 && x != xx)
            {
                button5.Text = "Settings >";
                ToStatus3();
            }
            else if (status4 && x == xx)
            {
                status4 = false;
                status3 = true;
                timerWindowX.Enabled = false;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            timerWindowX.Enabled = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (t1 != null && t1.IsAlive == true)
            {
                //e.Cancel = true;
                DialogResult ok = MessageBox.Show("Optimization is running, quit?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (ok == DialogResult.No) return;
                t1.Abort();
                //timerclose.Enabled = true;
                //return;
            }
            //e.Cancel = false;
            timerclose.Enabled = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void frmMain_MouseDown(object sender, MouseEventArgs e)
        {
            if (timerWindowX.Enabled == true) return;
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        private void lblTitle_MouseDown(object sender, MouseEventArgs e)
        {
            if (timerWindowX.Enabled == true) return;
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            if (timerWindowX.Enabled == true) return;
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        private void panelCode_MouseDown(object sender, MouseEventArgs e)
        {
            if (timerWindowX.Enabled == true) return;
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            var folder = new FolderBrowserDialog();
            if (sControls.BackupRoot == "")
                folder.Description = "Current root: <source osb file root>\n\nSelect a new directory...";
            else folder.Description = "Current root: " + sControls.BackupRoot + "\n\nSelect a new directory...";
            //folder.ShowDialog();
            if (folder.ShowDialog() == DialogResult.OK)
            {
                sControls.BackupRoot = folder.SelectedPath;
            }
            else sControls.BackupRoot = "";
            WriteSettings();
        }

        private void button8_MouseHover(object sender, EventArgs e)
        {
            string tmproot = sControls.BackupRoot;
            if (tmproot == "")
            {
                label1.Text = "<source osb file root>";
                return;
            }
            if (tmproot.Length > 20)
                label1.Text = ".." + tmproot.Substring(tmproot.Length - 20, 20);
            else
                label1.Text = tmproot;
        }

        private void button8_MouseLeave(object sender, EventArgs e)
        {
            label1.Text = "(Cancel to back to default)";
        }

        private void timeropen_Tick(object sender, EventArgs e)
        {
            Opacity += 0.15;
            if (Opacity >= 1)
            {
                Opacity = 1;
                timeropen.Enabled = false;
            }
        }

        private void frmMain_Shown(object sender, EventArgs e)
        {
            timeropen.Enabled = true;
        }

        private void timerclose_Tick(object sender, EventArgs e)
        {
            Opacity -= 0.15;
            if (Opacity <= 0)
            {
                Opacity = 0;
                timeropen.Enabled = false;
                this.Close();
            }
        }

        private void chkinfo_Click(object sender, EventArgs e)
        {
            InfoCollector.IfCollectInfo = chkinfo.Checked;
            WriteSettings();
        }

        private void chkex_Click(object sender, EventArgs e)
        {
            InfoCollector.IfCollectEx = chkex.Checked;
            WriteSettings();
        }

        private void chkinfo_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkchk_Click(object sender, EventArgs e)
        {
            Optimizer.ifCheck = chkchk.Checked;
            WriteSettings();
        }

        private void chkchk_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ToStatus4()
        {
            this.Left -= (int)(x / 2);
            this.Width += (int)x;
            button6.Left += (int)x;
            button7.Left += (int)x;
            button9.Left += (int)x;
            //this.Width = button6.Left + button6.Width + button6.Top;
            x -= 1;
        }
        private void ToStatus3()
        {
            this.Left += (int)(xx - x) / 2;
            this.Width -= (int)(xx - x);
            button6.Left -= (int)(xx - x);
            button7.Left -= (int)(xx - x);
            button9.Left -= (int)(xx - x);
            //this.Width = button6.Left + button6.Width + button6.Top;

            x += 1;
        }
    }
}