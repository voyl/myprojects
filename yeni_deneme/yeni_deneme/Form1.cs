using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Web;

namespace yeni_deneme
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void wazzap()
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {
            string lp = "";
            try
            {
                for (int j = 0; j <= Convert.ToInt16(textBox2.Text); j++)
                {
                    string src = "";
                    string a = "http://www.google.com.tr/search?q=" + textBox1.Text + "&start=" + (j * 10);
                    a = a.Replace(' ', '+');
                    webBrowser1.Navigate(a);
                    while (webBrowser1.ReadyState != WebBrowserReadyState.Complete)
                    {
                        Application.DoEvents();
                    }
                    src = webBrowser1.DocumentText;

                    if (src.Contains("Sistemimiz, bilgisayar ağınızdan gelen sıra dışı bir trafik algıladı. Bu sayfa, gelen isteği gönderen kişinin bir robot değil, gerçekten siz olduğunuzu denetlemek içindir."))
                    {
                        MessageBox.Show("CAPTCHA");
                        wait();
                    }
                    src = webBrowser1.DocumentText;
                    if (src.Contains("<div id=\"resultStats\">"))
                    {
                        string[] sn = src.Split(new string[] { "<div id=\"resultStats\">" }, StringSplitOptions.None);
                        string[] sn1 = sn[1].Split('/');
                        if (lp == sn1[0])
                        {
                            MessageBox.Show("Bitti.");
                            break;
                        }
                        else
                        {
                            lp = sn1[0];
                        }
                    }
                    if (src.Contains("<h3 class=\"r\"><a href=\"http://www.google.com.tr/url?url="))
                    {
                        string[] p1 = src.Split(new string[] { "<h3 class=\"r\"><a href=\"http://www.google.com.tr/url?url=" }, StringSplitOptions.None);
                        for (int i = 1; i < p1.Length; i++)
                        {
                            string[] p2 = p1[i].Split('&');
                            listBox1.Items.Add(HttpUtility.UrlDecode(p2[0]));
                        }
                    }
                    else if (src.Contains("ile ilgili hiçbir arama sonucu mevcut değil"))
                    {
                        MessageBox.Show("Daha Fazla Sayfa Yok.");
                        break;
                    }
                    else
                    {
                        Clipboard.SetText(src);
                        break;
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            MessageBox.Show("Bitti.");
        }
        private void Go()
        {
            
        }
        int dur = 5000;
        private void button2_Click(object sender, EventArgs e)
        {
            sec = Convert.ToInt16(textBox3.Text);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            sec--;
        }
        int sec = 10;
        private void wait()
        {
            timer1.Enabled = true;
            do
            {
                Application.DoEvents();
            }
            while (sec > 1);
            sec = 10;
            timer1.Enabled = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            List<string> l = new List<string>();
            foreach (string s in listBox1.Items)
                l.Add(s);
            System.IO.File.WriteAllLines("linkler.txt", l);
            MessageBox.Show("Liste, linkler.txt adıyla kaydedildi.");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }
    }
}
