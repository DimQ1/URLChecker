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

            LowLevelHttpRequest.SuccessUrl += Show_Message;
            HashChecker.Status += HashChecker_Status;
        }

        private void HashChecker_Status(string message)
        {
            var infoText = $"{message}\r\n{Info.Text}";
            Info.Text = infoText;
        }

        private async void Button1_Click(object sender, EventArgs e)
        {
            await HashChecker.CheckUrlsWithHash(textBox1.Text);
        }

        private void Show_Message(string message)
        {

            var text = $"{message}\r\n{ShowSuccesUrls.Text}";
            ShowSuccesUrls.Text = text;
        }

    }







}
