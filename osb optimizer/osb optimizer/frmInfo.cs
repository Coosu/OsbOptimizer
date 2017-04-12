using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace LibOSB
{
    public partial class frmUpdate : Form
    {
        public frmUpdate()
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


        private static UpdateInfo updateinfo;

        internal static UpdateInfo Updateinfo { get => updateinfo; set => updateinfo = value; }

        private void frmInfo_Load(object sender, EventArgs e)
        {
            LblTime.Text = string.Format("Found new version {0} ({1})", updateinfo.Version, updateinfo.Datetime.ToLongDateString());
            TextInfo.Text = updateinfo.Infomation;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(updateinfo.Link);
            }
            catch { }
            timerclose.Enabled = true;
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

        private void button6_Click(object sender, EventArgs e)
        {
            timerclose.Enabled = true;
        }

        private void frmUpdate_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        private void LblTime_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }
    }
}
