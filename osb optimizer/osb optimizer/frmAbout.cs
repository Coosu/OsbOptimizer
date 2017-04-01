using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;
namespace LibOSB
{
    partial class frmAbout : Form
    {
        public frmAbout()
        {
            InitializeComponent();
        }

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;

        private void okButton_Click(object sender, EventArgs e)
        {
            //"86,156,214";
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://osu.ppy.sh/forum/t/532042");
        }

        private void labelProductName_Click(object sender, EventArgs e)
        {

        }

        private void frmAbout_Load(object sender, EventArgs e)
        {

        }

        private void frmAbout_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        private void timeropen_Tick(object sender, EventArgs e)
        {
            Opacity += 0.1;
            if (Opacity >= 0.9)
            {
                Opacity = 0.9;
                timeropen.Enabled = false;
            }
        }

        private void timerclose_Tick(object sender, EventArgs e)
        {
            Opacity -= 0.1;
            if (Opacity <= 0)
            {
                Opacity = 0;
                timeropen.Enabled = false;
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timerclose.Enabled = true;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://osu.ppy.sh/u/1243669");
        }
    }
}
