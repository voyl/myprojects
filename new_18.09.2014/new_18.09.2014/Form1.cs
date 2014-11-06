using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Text.RegularExpressions;
using System.Web;

namespace new_18._09._2014
{
    public partial class Form1 : Form
    {
        WebBrowser wb;
        public Form1()
        {
            InitializeComponent();
        }
        private string getResult(string url)
        {
            using (WebClient wb = new WebClient())
            {
                return wb.DownloadString(url);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            loginNico();
        }
        private void loginNico()
        {
            try
            {
                toolStripStatusLabel2.Text = "Giriş Yapılıyor..";
                wb = new WebBrowser();
                wb.ScriptErrorsSuppressed = true;
                wb.Navigate("http://www.nicovideo.jp/login");
                while (wb.ReadyState != WebBrowserReadyState.Complete)
                    Application.DoEvents();
                wb.Document.GetElementById("mail").InnerText = "by.sleepy@gmail.com";
                wb.Document.GetElementById("password").InnerText = "1725839";
                foreach (HtmlElement el in wb.Document.GetElementsByTagName("input"))
                {
                    if (el.GetAttribute("type").Equals("submit"))
                    {
                        el.InvokeMember("click");
                        break;
                    }
                }
            }
            catch{
                toolStripStatusLabel2.Text = "Giriş Başarısız..";
            }
            toolStripStatusLabel2.Text = "Giriş Başarılı..";
            button2.Enabled = true;
        }
        string nicoLink = "";
        string videoName = "";
        private void checkNico()
        {
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                listBox1.SetSelected(i, true);
                nicoLink = listBox1.SelectedItem.ToString();
                wb.Navigate(nicoLink);
                while (wb.ReadyState != WebBrowserReadyState.Complete)
                    Application.DoEvents();

                if (wb.DocumentText.Contains("This video is not available in your country"))
                    continue;
                if (!wb.DocumentText.Contains("<span class=\"videoHeaderTitle\""))
                    continue;
                string[] vname = wb.DocumentText.Split(new string[] { "<span class=\"videoHeaderTitle\" style=\"font-size:24px\">" }, StringSplitOptions.None);
                string[] vname1 = vname[1].Split(new string[] { "</span>" }, StringSplitOptions.None);
                videoName = vname1[0];
                string[] a = wb.DocumentText.Split(new string[] { "url%3D" }, StringSplitOptions.None);
                string[] b = a[1].Split(new string[] { "%26" }, StringSplitOptions.None);
                downloadFile(b[0].Replace("%253A", ":").Replace("%252F", "/").Replace("%253F", "?").Replace("%253D", "="));
            }
            toolStripStatusLabel2.Text = "Tüm Videolar İndirildi..";
            
        }
        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool InternetGetCookieEx(string pchURL, string pchCookieName,
        StringBuilder pchCookieData, ref uint pcchCookieData, int dwFlags, IntPtr lpReserved);
        const int INTERNET_COOKIE_HTTPONLY = 0x00002000;
        private void downloadFile(string url)
        {
            toolStripStatusLabel2.Text = "Video İndiriliyor..";
            WebClient wc = new WebClient();
            string cc = "";
            uint datasize = 1024;

            StringBuilder cookieData = new StringBuilder((int)datasize);
            if (InternetGetCookieEx(url, null, cookieData, ref datasize,
            INTERNET_COOKIE_HTTPONLY, IntPtr.Zero) && cookieData.Length > 0)
            {
                cc = cookieData.ToString();
            }
            else
            {
                cc = "";
            }
            wc.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
            wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
            sw.Start();
            wc.Headers.Add(HttpRequestHeader.Cookie, cc);
            wc.DownloadFileAsync(new Uri(url),videoName+ ".mp4");
            while (wc.IsBusy)
            {
                Application.DoEvents();
            }
            wc.Dispose();
        }
        Stopwatch sw = new Stopwatch();
        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {

            label1.Text = "Hız: "+string.Format("{0} kb/s", (e.BytesReceived / 1024d / sw.Elapsed.TotalSeconds).ToString("0.00"));
            progressBar1.Value = e.ProgressPercentage;
            label2.Text = "Yüzde: "+e.ProgressPercentage.ToString() + "%";
            label3.Text = "Toplam: "+string.Format("{0} MB's / {1} MB's",
                (e.BytesReceived / 1024d / 1024d).ToString("0.00"),
                (e.TotalBytesToReceive / 1024d / 1024d).ToString("0.00"));
        }
        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            sw.Reset();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime a = new DateTime(2014, 11, 06, 17, 40, 00);
            DateTime b = DateTime.Now;
            if (Math.Round(a.Subtract(b).TotalMinutes, 0) < 1)
                Environment.Exit(0);
            toolStripStatusLabel3.Text = "Deneme Süresi Kalan: " + Math.Round(a.Subtract(b).TotalMinutes, 0) + " DK";
        }
        private void button2_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = "Başlatılıyor..";
            checkNico();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add(textBox1.Text);
            textBox1.Clear();
            textBox1.Focus();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listBox1.Items.Remove(listBox1.SelectedItem);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }
        private void listeCek()
        {
            toolStripStatusLabel2.Text = "Çekiliyor..";
            for (int i = Convert.ToInt32(textBox3.Text); i <= Convert.ToInt32(textBox4.Text); i++)
            {
                string a = "";
                using (WebClient wb = new WebClient())
                {
                    a = wb.DownloadString("http://www.nicovideo.jp/search/" + HttpUtility.UrlEncode(textBox2.Text, Encoding.UTF8) + "?page=" + i + "&sort=n&order=d");
                }
                if (a.Contains("Check for misspellings and omitted letters"))
                    break;
                string[] b = a.Split(new string[] { "data-video-thumbnail data-id=\"" }, StringSplitOptions.None);
                for (int j = 1; j < b.Length; j++)
                {
                    string[] c = b[j].Split('"');
                    listBox2.Items.Add("http://www.nicovideo.jp/watch/" + c[0]);
                }
            }
            toolStripStatusLabel2.Text = "Sayfa Çekme Tamamlandı.";
        }
        private void button5_Click(object sender, EventArgs e)
        {
            Thread a = new Thread(new ThreadStart(listeCek));
            a.SetApartmentState(ApartmentState.STA);
            a.Start();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            nicoLink = listBox2.SelectedItem.ToString();
            wb.Navigate(nicoLink);
            while (wb.ReadyState != WebBrowserReadyState.Complete)
                Application.DoEvents();
            string[] vname = wb.DocumentText.Split(new string[] { "<span class=\"videoHeaderTitle\" style=\"font-size:24px\">" }, StringSplitOptions.None);
            string[] vname1 = vname[1].Split(new string[] { "</span>" }, StringSplitOptions.None);
            videoName = vname1[0];
            string[] a = wb.DocumentText.Split(new string[] { "url%3D" }, StringSplitOptions.None);
            string[] b = a[1].Split(new string[] { "%26" }, StringSplitOptions.None);
            MessageBox.Show(b[0].Replace("%253A", ":").Replace("%252F", "/").Replace("%253F", "?").Replace("%253D", "="));
        }

        private void listBox2_DoubleClick(object sender, EventArgs e)
        {
            listBox1.Items.Add(listBox2.SelectedItem.ToString());
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            listBox1.Items.AddRange(listBox2.Items);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
        }
    }
}
