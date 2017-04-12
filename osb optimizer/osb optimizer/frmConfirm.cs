using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LibOSB
{
    public partial class frmConfirm : Form
    {
        public frmConfirm()
        {
            InitializeComponent();
        }

        public frmConfirm(int type)
        {
            InitializeComponent();
            isNoOnly = type;
        }

        private int isNoOnly = 0;
        private bool isClicked = false;

        public int IsNoOnly { get => isNoOnly; set => isNoOnly = value; }
        public bool IsClicked { get => isClicked; set => isClicked = value; }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void timeropen_Tick(object sender, EventArgs e)
        {
            Opacity += 0.2;
            if (Opacity >= 1)
            {
                Opacity = 1;
                timeropen.Enabled = false;
            }
        }

        private void timerclose_Tick(object sender, EventArgs e)
        {
            Opacity -= 0.2;
            if (Opacity <= 0)
            {
                Opacity = 0;
                timeropen.Enabled = false;
                IsClicked = true;
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmMain.iii = 0;
            timerclose.Enabled = true;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frmConfirm_Load(object sender, EventArgs e)
        {
            if (IsNoOnly == 1)
            {
                button1.Visible = false;
                button2.Text = "OK";
                label1.Text = "Please enter a number between 0 and 15.";
                button2.Left = Width / 2 - button2.Width / 2;
            }
            if (IsNoOnly == 2)
            {
                button1.Visible = false;
                button2.Text = "OK";
                label1.Text = "Please enter a valid number.";
                button2.Left = Width / 2 - button2.Width / 2;
            }
        }
    }
}
