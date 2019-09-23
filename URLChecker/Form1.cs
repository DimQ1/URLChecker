using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Net.Http;

namespace URLChecker
{
    public partial class Form1 : Form
    {

        private static CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();

        public Form1()
        {
            InitializeComponent();
            //для NLog
            Logger.Configure("error.txt", "ok.txt", "uploadFile.txt");                        //для сетевого алгоритма
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
                for (int i = 0; i < arStr.Length; i++) { if (arStr[i] != "" && arStr[i].Length >= 10) arStr[i] = arStr[i].Substring(0, 10); }

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

        //кнопка для альтернативного алгоритма с объектом
        private async void SmartCheckAllHashes(object sender, EventArgs e)
        {
            ((Button)sender).Enabled = false;

            string filePath_settings = Directory.GetCurrentDirectory() + "/settings.txt";
            string startHash = StartHash.Text;
            string widthOfRangeBruteforce = WidthOfRangeBruteforce.Text;
            var mutHash = new GenerateMutationHash(filePath_settings, startHash, Convert.ToInt32(widthOfRangeBruteforce));

            HttpBruteForce httpBruteForce = new HttpBruteForce(1000);

            Stack<string> stack = mutHash.Next1000Hashs();

            while (stack.Count > 0)
            {
                await httpBruteForce.StartBruteForce(stack, CancellationTokenSource.Token);
                textBox7.Text = Convert.ToString(Convert.ToInt32(textBox7.Text) + 1000);

                if (((Button)sender).Enabled)
                {
                    break;
                }

                if (checkTrueHash)
                {
                    mutHash.optimizeBruteHash(); checkTrueHash = false;
                }   

                CancellationTokenSource = new CancellationTokenSource();

                stack = mutHash.Next1000Hashs();
            }

            ((Button)sender).Enabled = true;
        }

        //это все для события
        public static bool checkTrueHash = false;
        private void Show_Message(string message)
        {
            var text = $"{message}";
            ShowSuccesUrls.Text = ShowSuccesUrls.Text + text + " - ";

            checkTrueHash = true;
            CancellationTokenSource.Cancel();
        }

        private void StopCheckHashes_Click(object sender, EventArgs e)
        {
            CancellationTokenSource.Cancel();
            fastCheckButton.Enabled = true;
        }

        
        //загрузка файла на сервер
        public async Task<string> SendRequestAnonf(string filePath, CancellationToken ct)
        {
            if (File.Exists(filePath))
            {
                byte[] file_bytes = File.ReadAllBytes(filePath);
                HttpClient httpClient = new HttpClient();
                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new StringContent("name"), "file");
                form.Add(new StringContent("filename"), filePath);
                form.Add(new ByteArrayContent(file_bytes, 0, file_bytes.Length), "file", Path.GetFileName(filePath));
                HttpResponseMessage response = await httpClient.PostAsync("https://anonfile.com/api/upload", form);
                response.EnsureSuccessStatusCode();
                httpClient.Dispose();
                var sd = response.Content.ReadAsStringAsync().Result;

                //распарсить json

                return sd.Substring(sd.IndexOf(@"https://anonfile.com/"), 31); ;
            }
            else { return ""; }
        }


        //стартуем отправку файла на сервер
        private void Button6_Click(object sender, EventArgs e)
        {
            if (button6.Text == "Start")
            {
                if (((textBox6.Text != "") && (Convert.ToInt32(textBox6.Text) != 0)))
                {
                    textBox1.Enabled = false;
                    textBox5.Enabled = false;
                    textBox6.Enabled = false;

                    timer1.Interval = Convert.ToInt32(textBox6.Text) * 1000;
                    timer1.Start(); //.Enabled = true;

                    button6.Text = "Stop";
                }
            }
            else
            {
                timer1.Stop();
                button6.Text = "Start";

                textBox1.Enabled = true;
                textBox5.Enabled = true;
                textBox6.Enabled = true;
            }



            
        }

        private static NLog.Logger loggerUpload = NLog.LogManager.GetCurrentClassLogger();
        //периодический опрос папки на наличие новых файлов
        private async void Timer1_Tick(object sender, EventArgs e)
        {
            if (File.Exists(textBox1.Text) && (Directory.Exists(textBox5.Text)))
            {
                string[] filesInDir = Directory.GetFiles(textBox5.Text, "*.txt", SearchOption.TopDirectoryOnly);

                foreach (string pathF in filesInDir)
                {
                    //string fileName = Path.GetFileName(pathF);

                    string s = await SendRequestAnonf(textBox1.Text, new CancellationToken());
                    loggerUpload.Info($"Uploading - | {Path.GetFileName(pathF)}| - {s}");
                    File.Delete(pathF);
                }
            }
        }


        //выбрали директорию для мониторинга
        private void Button5_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox5.Clear();
                textBox5.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        //выбрали файл для загрузки на сайт
        private void Button1_Click_1(object sender, EventArgs e)
        {
            openFileDialog3.FilterIndex = 1;
            openFileDialog3.RestoreDirectory = true;

            if (openFileDialog3.ShowDialog() == DialogResult.OK)
            {
                textBox1.Clear();
                textBox1.Text = openFileDialog3.FileName;
            }
        }
               

    }
}