using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace _2._0_deneme
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        List<string> a = new List<string>();
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                listBox1.Items.AddRange(System.IO.File.ReadAllLines("liste.txt"));
                MessageBox.Show("Toplam " + listBox1.Items.Count + " adet isim eklendi.");
            }
            catch { MessageBox.Show("Liste yüklemede hata oluştu. Listenin bulunduğu liste.txt dosyasını programla aynı yere koyunuz."); }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count > 0)
            {
                kontrol();
            }
            else
                MessageBox.Show("TXT dosyasından yükleme yapın.");
        }
        private void kontrol()
        {
            toolStripStatusLabel2.Text = "Kontrol İşlemi Başladı..";
            for (int i = 0; i < listBox1.Items.Count - 1;i++)
            {
                listBox1.SetSelected(i, true);
                string[] bl = listBox1.Items[i].ToString().Split('@');
                linkBekle(bl[1]);
                webBrowser1.Document.GetElementById("imembernamelive").InnerText = bl[0];
                webBrowser1.Document.GetElementById("idomain").SetAttribute("value", bl[1]);
                webBrowser1.Document.GetElementById("imembernamelive").Focus();
                webBrowser1.Document.GetElementById("iPwd").Focus();
                wait();
                if (webBrowser1.Document.GetElementById("iLiveMessage").InnerText != null && webBrowser1.Document.GetElementById("iLiveMessage").InnerText.Contains(bl[0]))
                {
                    listBox3.Items.Add(webBrowser1.Document.GetElementById("iLiveMessage").InnerText);
                    listBox2.Items.Add(listBox1.Items[i].ToString());
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            sec--;
        }
        int sec = 4;
        private void wait()
        {
            timer1.Enabled = true;
            do
            {
                this.Text = "Mail Kontrol "+sec.ToString();
                Application.DoEvents();
            }
            while (sec > 1);
            sec = 4;
            timer1.Enabled = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> son = new List<string>();
                foreach (string s in listBox2.Items)
                    son.Add(s);
                System.IO.File.WriteAllLines("uygun_liste.txt", son);
                MessageBox.Show("Programla aynı klasörde, uygun_liste.txt şeklinde uygun olanlar kayıt edildi.");
            }
            catch
            {
                MessageBox.Show("Kayıt aşamasında sorun oluştu.");
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            do
            {
                Application.DoEvents();
            }
            while (webBrowser1.ReadyState != WebBrowserReadyState.Complete);
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            toolStripStatusLabel2.Text = "Program Yüklendi. Başlamaya Hazır";
            timer2.Enabled = false;
        }
        private void linkBekle(string extension)
        {
            switch (extension)
            {
                case "hotmail.com":
                case "hotmail.com.tr":
                case "outlook.com":
                case "outlook.com.tr":
                case "windowslive.com":
                    if(webBrowser1.Url.ToString()!="https://signup.live.com/signup.aspx?mkt=tr-tr&ns=1&h__&lic=1")
                        webBrowser1.Navigate("https://signup.live.com/signup.aspx?mkt=tr-tr&ns=1&h__&lic=1");
                    break;
                case "outlook.de":
                case "hotmail.de":
                case "live.de":
                    if(webBrowser1.Url.ToString()!="https://signup.live.com/signup.aspx?mkt=de-de&ns=1&h__&lic=1")
                        webBrowser1.Navigate("https://signup.live.com/signup.aspx?mkt=de-de&ns=1&h__&lic=1");
                    break;
                case "outlook.fr":
                case "hotmail.fr":
                case "live.fr":
                    if (webBrowser1.Url.ToString() != "https://signup.live.com/signup.aspx?mkt=fr-fr&ns=1&h__&lic=1")
                        webBrowser1.Navigate("https://signup.live.com/signup.aspx?mkt=fr-fr&ns=1&h__&lic=1");
                    break;
                case "hotmail.se":
                case "live.se":
                case "live.com":
                    if (webBrowser1.Url.ToString() != "https://signup.live.com/signup.aspx?mkt=sv-sv&ns=1&h__&lic=1")
                        webBrowser1.Navigate("https://signup.live.com/signup.aspx?mkt=sv-sv&ns=1&h__&lic=1");
                    break;
                case "outlook.it":
                case "hotmail.it":
                case "live.it":
                    if (webBrowser1.Url.ToString() != "https://signup.live.com/signup.aspx?mkt=it-it&ns=1&h__&lic=1")
                        webBrowser1.Navigate("https://signup.live.com/signup.aspx?mkt=it-it&ns=1&h__&lic=1");
                    break;
                case "hotmail.ca":
                case "live.ca":
                    if (webBrowser1.Url.ToString() != "https://signup.live.com/signup.aspx?mkt=en-ca&ns=1&h__&lic=1")
                        webBrowser1.Navigate("https://signup.live.com/signup.aspx?mkt=en-ca&ns=1&h__&lic=1");
                    break;
                case "hotmail.es":
                case "outlook.es":
                    if (webBrowser1.Url.ToString() != "https://signup.live.com/signup.aspx?mkt=es-es&ns=1&h__&lic=1")
                        webBrowser1.Navigate("https://signup.live.com/signup.aspx?mkt=es-es&ns=1&h__&lic=1");
                    break;
                case "hotmail.co.uk":
                case "outlook.co.uk":
                    if (webBrowser1.Url.ToString() != "https://signup.live.com/signup.aspx?mkt=co-uk&ns=1&h__&lic=1")
                        webBrowser1.Navigate("https://signup.live.com/signup.aspx?mkt=co-uk&ns=1&h__&lic=1");
                    break;
                default:
                    break;
            }
            do
            {
                Application.DoEvents();
            }
            while (webBrowser1.ReadyState != WebBrowserReadyState.Complete);
            webBrowser1.Document.GetElementById("iliveswitch").InvokeMember("click");
            wait();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            if (MessageBox.Show("Çıkmak İstediğinizden Emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Environment.Exit(0);
            }
        }
    }
}
