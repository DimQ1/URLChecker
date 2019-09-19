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
using System.Collections;

namespace URLChecker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //для NLog
            Logger.Configure("error.txt", "ok.txt");                        //для сетевого алгоритма
                                                                            //Logger.Configure("error_local.txt", "ok_local.txt");            //для локального алгоритма

            //HashChecker.Status += HashChecker_Status;

            //сообщение найденом совпадении
            LowLevelHttpRequest.SuccessUrl += Show_Message;

        }

        #region TODO Need to check. not used yet

        private async void Button1_Click(object sender, EventArgs e)
        {
            //await Hash.CheckUrlWithHash(textBox1.Text, Convert.ToInt32(textBox5.Text));
        }


        static string[] arStr;              //зделали статическим массив чтобы передавать по button1
        private void Button2_Click(object sender, EventArgs e)
        {
            //openFileDialog1.Filter = "pcap files (*.pcap)|*.pcap|Cap files (*.cap)|*.cap";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.Clear();

                textBox2.Text = openFileDialog1.FileName;

                string[] arStr = File.ReadAllLines(openFileDialog1.FileName);

                Dictionary<string, Int32> dictSymbols = new Dictionary<string, int>();
                foreach (string s in arStr)
                {
                    string subS = s.Substring(9, 1);
                    if (dictSymbols.ContainsKey(subS)) { dictSymbols[subS] = dictSymbols[subS] + 1; }
                    else { dictSymbols.Add(subS, 1); }
                }


                var sortedDict = new SortedDictionary<string, int>(dictSymbols);
                foreach (KeyValuePair<String, Int32> pair in sortedDict)
                {
                    richTextBox1.Text = richTextBox1.Text + pair.Key + " - " + pair.Value + Environment.NewLine;
                }

            }


        }


        //загрузка данных для локального перебора
        private void Button3_Click(object sender, EventArgs e)
        {
            openFileDialog2.FilterIndex = 1;
            openFileDialog2.RestoreDirectory = true;

            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                richTextBox2.Clear();

                textBox3.Text = openFileDialog2.FileName;

                arStr = File.ReadAllLines(openFileDialog2.FileName);
                for (int i=0; i<arStr.Length; i++)       {  if (arStr[i] != "" && arStr[i].Length >= 10) arStr[i] = arStr[i].Substring(0, 10);    }

                foreach (string s in arStr)
                {
                    richTextBox2.Text = richTextBox2.Text + s + Environment.NewLine;
                }
                textBox4.Text = arStr[arStr.Length / 2];
            }
        }


        //старт локального перебора
        private async void Button4_Click(object sender, EventArgs e)
        {
            await LocalHash.CheckUrlWithHash(textBox4.Text, arStr);
        }
        #endregion

        private async void StartCheckAllHashes(object sender, EventArgs e)
        {
            await Hash.CheckUrlWithHash(textBox1.Text, Convert.ToInt32(textBox5.Text));
        }


        //кнопка для альтернативного алгоритма с объектом
        private async void SmartCheckAllHashes(object sender, EventArgs e)
        {
            string filePath_settings = Directory.GetCurrentDirectory() + "/settings.txt";
            GenerateMutationHash mutHash;

            if ((textBox1.Text != "") && (textBox1.Text.Length == 10) && (textBox5.Text != ""))
            {
                mutHash = new GenerateMutationHash(textBox1.Text, Convert.ToInt32(textBox5.Text));
            }
            else
            {
               mutHash = new GenerateMutationHash(filePath_settings);
            }

            mutHash.SaveProgress(filePath_settings);
            HttpBruteForce httpBruteForce = new HttpBruteForce(1000);

            Stack<string> stack;
            while ((stack = mutHash.Next1000Hashs()).Count > 0)
            {
                await httpBruteForce.StartBruteForce(stack);

                if (checkTrueHash) { mutHash.optimizeBruteHash(); checkTrueHash = false; }      //оптимизация

                mutHash.SaveProgress(filePath_settings);
            }
        }



        //это все для события
        public static bool checkTrueHash = false;
        static private void Show_Message(string message)
        {
            var text = $"{message}";
            checkTrueHash = true;
        }



    }
}