using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace URLChecker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //для NLog
            Logger.Configure();
        }

        private async void Button1_Click(object sender, EventArgs e)
        {
            await Hash.CheckUrlWithHash(textBox1.Text);
        }

    }







}
