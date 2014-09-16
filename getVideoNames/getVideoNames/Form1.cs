using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Net;
using System.Threading;

namespace getVideoNames
{
    public partial class Form1 : Form
    {
        Thread a;
        List<string> nm = new List<string>();
        public Form1()
        {
            InitializeComponent();
        }
        private void Ekle(string l)
        {
            using (OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=names.mdb"))
            {
                conn.Open();
                OleDbCommand sorgu = new OleDbCommand("INSERT INTO tablom(isim) values('" + l + "')", conn);
                sorgu.ExecuteNonQuery();
            }
        }
        private bool Kontrol(string l)
        {
            using (OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=names.mdb"))
            {
                conn.Open();
                OleDbCommand sorgu = new OleDbCommand("select Count(isim) from tablom where isim ='" + l + "'", conn);
                if (Convert.ToInt16(sorgu.ExecuteScalar()) > 0)
                    return true;
                else
                    return false;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length < 1)
            {
                MessageBox.Show("Lütfen Link Girin."); return;
            }
            /*a = new Thread(new ThreadStart(getNames));
            a.Start();*/
            backgroundWorker1.RunWorkerAsync();
        }
        int kacTaneAyni = 0;
        private string getSource(string site)
        {
            if (!site.Contains("http://"))
                site = "http://" + site;
            string src="";
            using (WebClient wb = new WebClient())
            {
                wb.Encoding = Encoding.UTF8;
                src = wb.DownloadString(site);
            }
            return src;
        }
        private void getNames()
        {
            listBox1.Items.Clear();
            nm.Clear();
            if (textBox1.Text.Substring(textBox1.Text.Length - 1, 1)=="/")
            {
                string rm = textBox1.Text;
                rm = rm.Remove(rm.Length - 1);
                textBox1.Text = rm;
            }

            string src = getSource(textBox1.Text);
            if (!src.Contains("/templates/z5"))
            {
                MessageBox.Show("Template Uyuşmazlığı. Z5 Olması Gerekir.");
                return;
            }
            string[] a = src.Split(new[] { "<a class=\"link\" href=\"" }, StringSplitOptions.None);
            string[] b = a[1].Split('"');
            int baslangic = b[0].LastIndexOf('-')+1;
            int bitis = b[0].LastIndexOf('.') - b[0].LastIndexOf('-')-1;
            for(int k = Convert.ToInt32(numericUpDown1.Value);k<=Convert.ToInt32(numericUpDown2.Value);k++)
            {
                string c = getSource(textBox1.Text+b[0].Replace(b[0].Substring(baslangic, bitis), k.ToString()));
                if (!c.Contains("<div class=\"scene\">"))
                {
                    this.Text = "Bitti. Sayfa: "+k.ToString();
                    break;
                }
                this.Text = k.ToString() + ". Sayfa";
                string[] i1 = c.Split(new string[] {"<img class=\"thumb\" src=\""},StringSplitOptions.None);
                for (int i = 1; i < i1.Length; i++)
                {
                    string[] w = i1[i].Split(new string[] { "title=\"" }, StringSplitOptions.None);
                    string[] w1 = w[1].Split(new string[] { "\">" }, StringSplitOptions.None);
                    if (w1[0].Contains("'"))
                        w1[0] = w1[0].Replace('\'', '"');

                    if (!Kontrol(w1[0]))
                    {
                        Ekle(w1[0]);
                        listBox1.Items.Add(w1[0]);
                        nm.Add(w1[0]);
                        int visibleItems = listBox1.ClientSize.Height / listBox1.ItemHeight;
                        listBox1.TopIndex = Math.Max(listBox1.Items.Count - visibleItems + 1, 0);
                    }
                    else
                        kacTaneAyni++;
                }

                /*if (kacTaneAyni > 200)
                {
                    this.Text = "Bitti.";
                    MessageBox.Show("Ard Arda 200 Aynı Video Bulundu. Yeni Video Yok.");
                    break;
                }*/

            }
            if (nm.Count > 0)
            {
                string dt = ("sonuc-" + DateTime.Now.ToString() + ".txt").Replace(':','.');
                System.IO.File.WriteAllLines(dt, nm.ToArray());
                MessageBox.Show("Sonuçlar, "+dt+" Dosyasına Yazıldı.");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult sonuc;
            sonuc = MessageBox.Show("Veritabanındaki Tüm Bilgileri Silmek İstediğinden Emin Misin? ", "Temizleme", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (sonuc == DialogResult.Yes)
            {
                using (OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=names.mdb"))
                {
                    conn.Open();
                    OleDbCommand sorgu = new OleDbCommand("delete * from tablom", conn);
                    sorgu.ExecuteNonQuery();
                }
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult sonuc;
            sonuc = MessageBox.Show("Durdurmak İstediğinizden Emin Misiniz? ", "Durdurma", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (sonuc == DialogResult.Yes)
            {
                backgroundWorker1.CancelAsync();
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            listBox1.Items.Clear();
            nm.Clear();
            if (textBox1.Text.Substring(textBox1.Text.Length - 1, 1) == "/")
            {
                string rm = textBox1.Text;
                rm = rm.Remove(rm.Length - 1);
                textBox1.Text = rm;
            }

            string src = getSource(textBox1.Text);
            if (!src.Contains("/templates/z5"))
            {
                MessageBox.Show("Template Uyuşmazlığı. Z5 Olması Gerekir.");
                return;
            }
            string[] a = src.Split(new[] { "<a class=\"link\" href=\"" }, StringSplitOptions.None);
            string[] b = a[1].Split('"');
            int baslangic = b[0].LastIndexOf('-') + 1;
            int bitis = b[0].LastIndexOf('.') - b[0].LastIndexOf('-') - 1;
            for (int k = Convert.ToInt32(numericUpDown1.Value); k <= Convert.ToInt32(numericUpDown2.Value); k++)
            {
                if (this.backgroundWorker1.CancellationPending)
                {
                    e.Cancel = true;
                    DialogResult sonuc;
                    sonuc = MessageBox.Show("Durduruldu. Listeyi Kaydetmek İster Misiniz? ", "Kayıt", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (sonuc == DialogResult.Yes)
                    {
                        if (nm.Count > 0)
                        {
                            string dt = ("sonuc-" + DateTime.Now.ToString() + ".txt").Replace(':', '.');
                            System.IO.File.WriteAllLines(dt, nm.ToArray());
                            sonuc = MessageBox.Show("Sonuçlar, " + dt + " Dosyasına Yazıldı. Açmak İster Misiniz?", "Aç", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (sonuc == DialogResult.Yes)
                            {
                                //MessageBox.Show(Application.StartupPath + "\\" + dt + ".txt");
                                System.Diagnostics.Process.Start(Application.StartupPath + "\\" + dt);
                            }
                        }
                    }
                    return;
                }
                string c = getSource(textBox1.Text + b[0].Replace(b[0].Substring(baslangic, bitis), k.ToString()));
                if (!c.Contains("<div class=\"scene\">"))
                {
                    this.Text = "Bitti. Sayfa: " + k.ToString();
                    break;
                }
                this.Text = k.ToString() + ". Sayfa";
                string[] i1 = c.Split(new string[] { "<img class=\"thumb\" src=\"" }, StringSplitOptions.None);
                for (int i = 1; i < i1.Length; i++)
                {
                    string[] w = i1[i].Split(new string[] { "title=\"" }, StringSplitOptions.None);
                    string[] w1 = w[1].Split(new string[] { "\">" }, StringSplitOptions.None);
                    if (w1[0].Contains("'"))
                        w1[0] = w1[0].Replace('\'', '"');

                    if (!Kontrol(w1[0]))
                    {
                        Ekle(w1[0]);
                        listBox1.Items.Add(w1[0]);
                        nm.Add(w1[0]);
                        int visibleItems = listBox1.ClientSize.Height / listBox1.ItemHeight;
                        listBox1.TopIndex = Math.Max(listBox1.Items.Count - visibleItems + 1, 0);
                    }
                    else
                        kacTaneAyni++;
                }

                /*if (kacTaneAyni > 200)
                {
                    this.Text = "Bitti.";
                    MessageBox.Show("Ard Arda 200 Aynı Video Bulundu. Yeni Video Yok.");
                    break;
                }*/

            }
            if (nm.Count > 0)
            {
                string dt = ("sonuc-" + DateTime.Now.ToString() + ".txt").Replace(':', '.');
                System.IO.File.WriteAllLines(dt, nm.ToArray());
                DialogResult sonuc;
                sonuc = MessageBox.Show("Sonuçlar, " + dt + " Dosyasına Yazıldı. Açmak İster Misiniz?", "Aç", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (sonuc == DialogResult.Yes)
                {
                    //MessageBox.Show(Application.StartupPath + "\\" + dt + ".txt");
                    System.Diagnostics.Process.Start(Application.StartupPath+"\\"+dt);
                }
            }
        }
        int tut = 0;
        string[] str = System.IO.File.ReadAllLines("deneme.txt");
        private void button4_Click(object sender, EventArgs e)
        {
            int say = 0;
            while (say < 50)
            {
                dice();
                say++;
            }
        }

        Random rnd = new Random();
        private void dice()
        {
            int tut2 = 0;
            tut2 = rnd.Next(0, str.Length);
            while (tut == tut2)
            {
                tut2 = rnd.Next(0, str.Length);
            }
            tut = tut2;
            listBox1.Items.Add(str[tut2]);
        }
    }
}
