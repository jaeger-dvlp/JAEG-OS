using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace grnew20
{
    public partial class Form2 : Form
    {
        public int a = 0;
        public Form2()
        {
            InitializeComponent();
        }

        public void Form2_Load(object sender, EventArgs e)
        {

            this.Cursor = new Cursor("asd.ico");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (a==1)
            {
                pictureBox1.Visible = false;
                pictureBox1.Left = (this.ClientSize.Width - pictureBox1.Width) / 2;
                pictureBox1.Top = (this.ClientSize.Height - pictureBox1.Height) / 2;
                label1.Top = (pictureBox1.Top + 400);
                label1.Left = (pictureBox1.Left+215);
                label1.Text = "";
                label1.Visible = true;
                
                pictureBox1.Visible = true;
                timer1.Enabled = false;
                backgroundWorker1.RunWorkerAsync();
            }
            else
            {

            }
        }

        int b = 0;
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            a:
            if (b == 1)
            {
                label1.ForeColor = Color.Red;
            }
            else if (b == 2)
            {
                label1.ForeColor = Color.DodgerBlue;
            }
            else if (b == 3)
            {
                label1.ForeColor = Color.Fuchsia;
            }
            else if (b == 4)
            {
                label1.ForeColor = Color.Aqua;
            }
            else if (b == 5)
            {
                label1.ForeColor = Color.Orange;
                b = 0;
            }

            b = b + 1;
            label1.Text = "|";
            System.Threading.Thread.Sleep(200);
            label1.Text = "/";
            System.Threading.Thread.Sleep(200);
            label1.Text = "--";
            System.Threading.Thread.Sleep(200);
            label1.Text = "\\";
            System.Threading.Thread.Sleep(200);
            label1.Text = "|";
            System.Threading.Thread.Sleep(200);
            label1.Text = "/";
            System.Threading.Thread.Sleep(200);
            label1.Text = "--";
            System.Threading.Thread.Sleep(200);
            label1.Text = "\\";
            System.Threading.Thread.Sleep(200);
            label1.Text = "|";
            System.Threading.Thread.Sleep(200);

            goto a;
            
        }
        }
    }

