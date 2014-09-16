using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mshtml;
using System.IO;
using DeathByCaptcha;
using System.Threading;

namespace _4._0_deneme
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            siraliCheck();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (webBrowser1.DocumentText.Contains("kısa mesajla bir kod"))
                MessageBox.Show("var");
            else
                MessageBox.Show("yk");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            HtmlElementCollection a = webBrowser1.Document.GetElementsByTagName("button");
            foreach (HtmlElement b in a)
            {
                if (b.InnerText.Contains("Gönder"))
                {
                    b.InvokeMember("click");
                    break;
                }
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog f = new OpenFileDialog();
            f.Title="Dosya Seçin..";
            f.Filter = "(*.txt)|*.txt";
            DialogResult d = f.ShowDialog();
            if (d == DialogResult.OK)
            {
                listBox1.Items.Clear();
                listBox2.Items.Clear();
                listBox3.Items.Clear();
                listBox1.Items.AddRange(File.ReadAllLines(f.FileName));
                MessageBox.Show(listBox1.Items.Count + " Adet Mail Eklendi..");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog f = new FolderBrowserDialog();
            if (f.ShowDialog() == DialogResult.OK)
            {
                List<string> ls = new List<string>();
                foreach (string s in listBox2.Items)
                    ls.Add(s);
                List<string> lst = new List<string>();
                foreach (string s in listBox3.Items)
                    lst.Add(s);
                File.WriteAllLines(f.SelectedPath + "\\kodlu.txt", ls);
                File.WriteAllLines(f.SelectedPath + "\\kodsuz.txt", lst);
            }
        }
        int sec = 4;
        private void W8()
        {
            timer1.Enabled = true;
            do
            {
                Application.DoEvents();
            }
            while (sec > 0);
            sec = 4;
            timer1.Enabled = false;
        }
        private void siraliCheck()
        {
            lbldurum.Text = "Başladı..";
            lbladet.Text = "0";
            int ad = 1;
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                listBox1.SetSelected(i, true);
                lbladet.Text = ad+++". Hesap Taranıyor..";
                if (listBox1.Items[i].ToString().Equals(""))
                    continue;
                webBrowser1.Navigate("https://www.facebook.com/login/identify?ctx=recover");
                do { Application.DoEvents(); }
                while (webBrowser1.ReadyState != WebBrowserReadyState.Complete);
                webBrowser1.Document.GetElementById("identify_email").InnerText = listBox1.Items[i].ToString();
                webBrowser1.Document.GetElementById("u_0_0").InvokeMember("click");
                W8();
                if (webBrowser1.DocumentText.Contains("Güvenlik Kontrolü"))
                {
                    bool bas = false;
                    do
                    {
                        //captcha
                        IHTMLDocument2 doc = (IHTMLDocument2)webBrowser1.Document.DomDocument;
                        IHTMLControlRange imgRange = (IHTMLControlRange)((HTMLBody)doc.body).createControlRange();

                        foreach (IHTMLImgElement img in doc.images)
                        {
                            if (img.nameProp.Contains("tfbimage.php"))
                            {
                                imgRange.add((IHTMLControlElement)img);

                                imgRange.execCommand("Copy", false, null);
                                Image img2 = Clipboard.GetImage();
                                if (File.Exists(Application.StartupPath + "\\image.jpg"))
                                {
                                    File.Delete(Application.StartupPath + "\\image.jpg");
                                }
                                img2.Save(Application.StartupPath + "\\image.jpg");
                                break;
                            }

                        }

                        W8();
                        bas = decaptIMG();
                    } while (bas == false);

                    HtmlElementCollection asq = webBrowser1.Document.GetElementsByTagName("button");

                    foreach (HtmlElement bs in asq)
                    {
                        if (bs.InnerText.Contains("Gönder"))
                        {
                            bs.InvokeMember("click");
                            break;
                        }
                    }
                    W8();
                    if (webBrowser1.DocumentText.Contains("kısa mesajla bir kod"))
                    {
                        listBox2.Items.Add(listBox1.Items[i].ToString());
                        lblmbl.Text = (Convert.ToInt32(lblmbl.Text) + 1).ToString();
                    }
                    else
                        listBox3.Items.Add(listBox1.Items[i].ToString());
                }
                else
                {

                    if (webBrowser1.DocumentText.Contains("kısa mesajla bir kod"))
                    {
                        listBox2.Items.Add(listBox1.Items[i].ToString());
                        lblmbl.Text = (Convert.ToInt32(lblmbl.Text) + 1).ToString();
                    }
                    else
                        listBox3.Items.Add(listBox1.Items[i].ToString());
                }
            }
            lbldurum.Text = "Tamamlandı..";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            sec--;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
        }
        private bool decaptIMG()
        {
            bool f = false;
            try
            {
                Client client = (Client)new SocketClient("zagot", "626925lanet");
                Captcha captcha = client.Decode(Application.StartupPath + "\\image.jpg", 10000);
                if (captcha.Solved && captcha.Correct)
                {
                    webBrowser1.Document.GetElementById("captcha_response").InnerText = captcha.Text;
                    if (!captcha.Solved)
                    {
                        client.Report(captcha);
                    }
                    f= true;
                }
            }
            catch
            {
                f = false;
            }
            return f;
        }

        private void hakkımdaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Çağdaş ARSLAN - Ertuğ İLMEZ tarafından yaptırılmıştır.");
        }
    }
}
