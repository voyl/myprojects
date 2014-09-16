using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Reflection;
using SpreadsheetLight;
using System.IO;
using System.Data.OleDb;
using System.Xml;

namespace emlak_bot
{
    public partial class Form1 : Form
    {
        const string HTML_TAG_PATTERN = "<.*?>";
        int data_say = 0;
        List<string> a = new List<string>();
        List<char> harf = new List<char>() {'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','V','Z'};
        Thread maint;
        OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=sahibinden.mdb");
        public Form1()
        {
            InitializeComponent();
        }
        private void Ekle(string ilan_id, string kisi_adi, string istel, string ceptel, string adres,string kelime)
        {
            OleDbCommand sorgu = new OleDbCommand("insert into sh(ilan_id,kisi_adi,istel,ceptel,adres,kelime) values('" + ilan_id + "','" + kisi_adi + "','" + istel + "','" + ceptel + "','" + adres + "','"+kelime+"')", conn);
            conn.Open();
            sorgu.ExecuteNonQuery();
            conn.Close();
        }
        private bool Kontrol(string ceptel, string istel)
        {
            bool b = true;
            OleDbCommand sorgu = new OleDbCommand("SELECT * from sh where ceptel ='" + ceptel + "' or istel = '" + istel + "'", conn);
            conn.Open();
            if (Convert.ToInt16(sorgu.ExecuteScalar()) > 0)
                b = false;
            conn.Close();
            return b;
        }
        private int getAdet()
        {
            OleDbCommand sorgu = new OleDbCommand("SELECT Count(*) from sh", conn);
            conn.Open();
            int rd = int.Parse(sorgu.ExecuteScalar().ToString());
            conn.Close();
            return rd;
        }
        private void ilanIDEkle(string ilan_id)
        {
            OleDbCommand sorgu = new OleDbCommand("insert into ilanlar(ilan_id) values ('" + ilan_id + "')", conn);
            conn.Open();
            sorgu.ExecuteNonQuery();
            conn.Close();
        }
        private bool KontrolID(string ilan_id)
        {
            bool b = true;
            OleDbCommand sorgu = new OleDbCommand("SELECT * from ilanlar where ilan_id ='" + ilan_id + "'", conn);
            conn.Open();
            if (sorgu.ExecuteScalar()!=null)
                b = false;
            conn.Close();
            return b;
        }
        void AppendText(RichTextBox box, Color color, string text)
        {
            int start = box.TextLength;
            box.AppendText(text + Environment.NewLine);
            int end = box.TextLength;
            box.Select(start, end - start);
            {
                box.SelectionColor = color;
            }
            box.SelectionLength = 0;
        }
        static string StripHTML(string inputString)
        {
            return Regex.Replace
              (inputString, HTML_TAG_PATTERN, string.Empty);
        }
        private string getHTML2(string link)
        {
            W8();
            string result = "yok";
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers.Add("user-agent", "Mozilla/5.0 (Windows; Windows NT 5.1; rv:1.9.2.4) Gecko/20100611 Firefox/3.6.4");
                    client.Encoding = Encoding.UTF8;
                    result = client.DownloadString(link);
                }
            }
            catch
            {
                result = "yok";
            }
            return result;
        }
        private string getHTML(string link)
        {
            string result = "yok";
            do
            {
                W8();
                try
                {
                    HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(link);
                    myRequest.Method = "GET";
                    myRequest.Timeout = 5000;
                    WebResponse myResponse = myRequest.GetResponse();
                    StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
                    result = sr.ReadToEnd();
                    sr.Close();
                    myResponse.Close();
                }
                catch
                {
                    result = "yok";
                }
            }
            while (result.Equals("yok"));
            return result;
        }
        private void emergencySituation(string dadi)
        {
            AppendText(richTextBox1, Color.DarkSlateBlue, "Toplam " + data_say.ToString() + " adet data işlemden geçti.");
            ts_adet.Text = getAdet().ToString() + " Adet Kayıt Bulunmaktadır.";
            if (checkBox1.Checked && listView1.Items.Count > 0)
            {
                bool tarih = false, id = false, badi = false, bistel = false, bceptel = false, badres = false;
                SLDocument exc = new SLDocument();
                SLStyle st = exc.CreateStyle();
                st.SetFontBold(true);
                if (checkBox7.Checked)
                {
                    tarih = true;
                    exc.SetCellValue("A1", "İlan Tarihi");
                    exc.SetCellStyle("A1", st);
                }
                if (checkBox2.Checked)
                {
                    id = true;
                    exc.SetCellValue("B1", "İlan ID");
                    exc.SetCellStyle("B1", st);
                }
                if (checkBox3.Checked)
                {
                    badi = true;
                    exc.SetCellValue("C1", "Adı");
                    exc.SetCellStyle("C1", st);
                }
                if (checkBox4.Checked)
                {
                    bistel = true;
                    exc.SetCellValue("D1", "İş Telefonu");
                    exc.SetCellStyle("D1", st);
                }
                if (checkBox5.Checked)
                {
                    bceptel = true;
                    exc.SetCellValue("E1", "Cep Telefonu");
                    exc.SetCellStyle("E1", st);
                }
                if (checkBox6.Checked)
                {
                    badres = true;
                    exc.SetCellValue("F1", "Adresi");
                    exc.SetCellStyle("F1", st);
                }
                int ex_sira = 2;
                foreach (ListViewItem itemRow in listView1.Items)
                {
                    string i_no = itemRow.SubItems[0].Text;
                    string adi = itemRow.SubItems[1].Text;
                    string istel = itemRow.SubItems[2].Text;
                    string ceptel = itemRow.SubItems[3].Text;
                    string adres = itemRow.SubItems[4].Text;
                    if (tarih)
                        exc.SetCellValue("A" + ex_sira, DateTime.Now.ToShortDateString());
                    if (id)
                        exc.SetCellValue("B" + ex_sira, i_no);
                    if (badi)
                        exc.SetCellValue("C" + ex_sira, adi);
                    if (bistel)
                        exc.SetCellValue("D" + ex_sira, istel);
                    if (bceptel)
                        exc.SetCellValue("E" + ex_sira, ceptel);
                    if (badres)
                        exc.SetCellValue("F" + ex_sira, adres);
                    ex_sira++;
                }
                if (exc.GetCellValueAsString("A1") == "")
                    exc.HideColumn("A");
                if (exc.GetCellValueAsString("B1") == "")
                    exc.HideColumn("B");
                if (exc.GetCellValueAsString("C1") == "")
                    exc.HideColumn("C");
                if (exc.GetCellValueAsString("D1") == "")
                    exc.HideColumn("D");
                if (exc.GetCellValueAsString("E1") == "")
                    exc.HideColumn("E");
                string exc_isim = dadi + ".xlsx";
                exc.SaveAs(exc_isim);
            }

        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count > 0)
            {
                tabControl1.SelectedIndex=1;
                maint = new Thread(new ThreadStart(asdasda1));
                maint.Start();
            }
            else
                MessageBox.Show("Aramak İstediğiniz Kelimeleri Girin..");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
                if (textBox1.Text.Length <= 3)
                    MessageBox.Show("3 Karakter veya Daha Kısa Olamaz!");
                else
                {
                    listBox1.Items.Add(textBox1.Text);
                    textBox1.Clear();
                }
            else
                MessageBox.Show("Lütfen Kelime Girin..");
        }
        private void reLoad()
        {
            button1.Enabled = true;
            button2.Enabled = true;
            textBox1.Enabled = true;
            stlbl.Text = "İşlemler Tamamlandı. Beklemede..";
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            ts_adet.Text = getAdet().ToString()+" Adet Kayıt Bulunmaktadır.";
            loadSettings();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            if (MessageBox.Show("Çıkmak İstediğinizden Emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                saveSettings();
                Environment.Exit(0);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show(listBox1.Items.Count.ToString());
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                checkBox2.Enabled = true;
                checkBox3.Enabled = true;
                checkBox4.Enabled = true;
                checkBox5.Enabled = true;
                checkBox6.Enabled = true;
            }
            else
            {
                checkBox2.Checked = false;
                checkBox2.Enabled = false;
                checkBox3.Checked = false;
                checkBox3.Enabled = false;
                checkBox4.Checked = false;
                checkBox4.Enabled = false;
                checkBox5.Checked = false;
                checkBox5.Enabled = false;
                checkBox6.Checked = false;
                checkBox6.Enabled = false;
            }
        }
        int geriSayanZamanSaniye = 10;
        private void timer1_Tick(object sender, EventArgs e)
        {
            
            string saat = (geriSayanZamanSaniye / 3600).ToString("00");
            string dakika = ((geriSayanZamanSaniye % 3600) / 60).ToString("00");
            string saniye = (geriSayanZamanSaniye % 60).ToString("00");
            this.Text="Sahibinden Bot  "+saat+":"+dakika+":"+saniye+" Süre Sonra Başlayacak..";
            geriSayanZamanSaniye--;
            if (geriSayanZamanSaniye < 0)
            {
                maint = new Thread(new ThreadStart(asdasda1));
                maint.Start();
                timer1.Enabled = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(maint==null)
            {
                int saat = 3600;
                switch (Convert.ToInt16(numericUpDown1.Value.ToString()))
                {
                    case 1:
                        geriSayanZamanSaniye = saat;
                        break;
                    case 2:
                        geriSayanZamanSaniye = saat * 2;
                        break;
                    case 3:
                        geriSayanZamanSaniye = saat * 3;
                        break;
                    case 4:
                        geriSayanZamanSaniye = saat * 4;
                        break;
                    case 5:
                        geriSayanZamanSaniye = saat * 5;
                        break;
                    case 6:
                        geriSayanZamanSaniye = saat * 6;
                        break;
                    case 7:
                        geriSayanZamanSaniye = saat * 7;
                        break;
                    case 8:
                        geriSayanZamanSaniye = saat * 8;
                        break;
                    case 9:
                        geriSayanZamanSaniye = saat * 9;
                        break;
                    case 10:
                        geriSayanZamanSaniye = saat * 10;
                        break;
                    case 11:
                        geriSayanZamanSaniye = saat * 11;
                        break;
                    case 12:
                        geriSayanZamanSaniye = saat * 12;
                        break;
                    case 13:
                        geriSayanZamanSaniye = saat * 13;
                        break;
                    case 14:
                        geriSayanZamanSaniye = saat * 14;
                        break;
                    case 15:
                        geriSayanZamanSaniye = saat * 15;
                        break;
                    case 16:
                        geriSayanZamanSaniye = saat * 16;
                        break;
                    case 17:
                        geriSayanZamanSaniye = saat * 17;
                        break;
                    case 18:
                        geriSayanZamanSaniye = saat * 18;
                        break;
                    case 19:
                        geriSayanZamanSaniye = saat * 19;
                        break;
                    case 20:
                        geriSayanZamanSaniye = saat * 20;
                        break;
                    case 21:
                        geriSayanZamanSaniye = saat * 21;
                        break;
                    case 22:
                        geriSayanZamanSaniye = saat * 22;
                        break;
                    case 23:
                        geriSayanZamanSaniye = saat * 23;
                        break;
                    case 24:
                        geriSayanZamanSaniye = saat * 24;
                        break;
                    default:
                        geriSayanZamanSaniye = 1;
                        break;
                }
                timer1.Enabled = true;
            }
            else if (maint.IsAlive)
            {
                MessageBox.Show("Ana İşlem Hala Devam Ediyor.. Lütfen Bitmesini Bekleyin..");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OleDbCommand a = new OleDbCommand("select * from sh where ceptel='0 (543) 571 35 45'", conn);
            conn.Open();
            int rd = 0;
            rd = Convert.ToInt16(a.ExecuteScalar());
            MessageBox.Show(rd.ToString());
            conn.Close();
        }

        int giris_say = 0;
        DateTime sTime;
        DateTime fTime;
        private string calcTimeDiff(DateTime start,DateTime finish)
        {
            TimeSpan a = finish.Subtract(start);
            return "İşlemler "+a.Hours+" Saat "+a.Minutes+" Dakika "+a.Seconds+" Saniye Sürdü.";
        }
        private void button6_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();
            using (OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=sahibinden.mdb"))
            {
                conn.Open();
                //OleDbCommand sorgu = new OleDbCommand("SELECT * FROM sh WHERE ilan_tarihi >  #01/01/2014# and ilan_tarihi < #04/01/2014#", conn);
                OleDbCommand sorgu = new OleDbCommand("Select * From sh Where Format(ilan_tarihi,'Short Date') Between '" + monthCalendar1.SelectionStart.ToShortDateString() + "' And '" + monthCalendar2.SelectionStart.ToShortDateString() + "'", conn);
                OleDbDataReader rd = sorgu.ExecuteReader();
                int i = 0;
                while (rd.Read())
                {
                    i++;
                    listView2.Items.Add(new ListViewItem(new string[] { rd.GetValue(2).ToString(), rd.GetValue(3).ToString(), rd.GetValue(4).ToString(), rd.GetValue(5).ToString(), rd.GetValue(6).ToString(), rd.GetValue(7).ToString() }));
                }
                if (i == 0)
                    MessageBox.Show("Veri Yok.");
                conn.Close();
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            sec--;
            stlbl.Text = sec + " Saniye Sonra Siteye Sorgu Atılacak..";
        }
        int sec = 12;
        private void W8()
        {
            sec = Convert.ToInt32(numericUpDown2.Value);
            timer2.Enabled = true;
            do
            {
                Application.DoEvents();
            }
            while (sec > 1);
            sec = Convert.ToInt32(numericUpDown2.Value);
            timer2.Enabled = false;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (listView2.Items.Count > 0)
            {
                SLDocument exc = new SLDocument();
                SLStyle st = exc.CreateStyle();
                st.SetFontBold(true);
                exc.SetCellValue("A1", "İlan ID");
                exc.SetCellStyle("A1", st);
                exc.SetCellValue("B1", "Adı");
                exc.SetCellStyle("B1", st);
                exc.SetCellValue("C1", "İş Telefonu");
                exc.SetCellStyle("C1", st);
                exc.SetCellValue("D1", "Cep Telefonu");
                exc.SetCellStyle("D1", st);
                exc.SetCellValue("E1", "Adresi");
                exc.SetCellStyle("E1", st);
                exc.SetCellValue("F1", "Tarihi");
                exc.SetCellStyle("F1", st);
                int ex_sira = 2;
                foreach (ListViewItem itemRow in listView2.Items)
                {
                    string i_no = itemRow.SubItems[0].Text;
                    string adi = itemRow.SubItems[1].Text;
                    string istel = itemRow.SubItems[2].Text;
                    string ceptel = itemRow.SubItems[3].Text;
                    string adres = itemRow.SubItems[4].Text;
                    string tarih = itemRow.SubItems[5].Text;
                    exc.SetCellValue("A" + ex_sira, i_no);
                    exc.SetCellValue("B" + ex_sira, adi);
                    exc.SetCellValue("C" + ex_sira, istel);
                    exc.SetCellValue("D" + ex_sira, ceptel);
                    exc.SetCellValue("E" + ex_sira, adres);
                    exc.SetCellValue("F" + ex_sira, tarih);
                    ex_sira++;
                }
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                DialogResult result = fbd.ShowDialog();
                string a = fbd.SelectedPath + "\\excel_çıktısı.xlsx";
                exc.SaveAs(a);
                MessageBox.Show("Dosyanız " + a + " olarak kayıt edildi.");
            }
            else
            {
                MessageBox.Show("Çıktı Alabilecek Bilgiler Görüntülenmemiş!..");
            }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            label8.Text = numericUpDown2.Value + " Saniye";
            sec = Convert.ToInt32(numericUpDown2.Value);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            loadSettings();
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show(listBox1.SelectedIndex.ToString());
        }
        private void loadSettings()
        {
            string s = File.ReadAllText("ayarlar.sahibinden");
            string[] a = s.Split('|');
            for (int i = 0; i < a.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        if (a[i] != "yok")
                        {
                            string[] q = a[i].Split(',');
                            foreach (string qq in q)
                                if (!qq.Equals(""))
                                    listBox1.Items.Add(qq);
                        }
                        break;
                    case 1:
                        numericUpDown1.Value = Convert.ToDecimal(a[i]);
                        break;
                    case 2:
                        string[] cc = a[i].Split(',');
                        for (int j = 0; j < cc.Length; j++)
                        {
                            if(!cc[j].Equals(""))
                                switch (j)
                                {
                                    case 0:
                                        if(cc[j].Equals("Checked"))
                                            checkBox1.Checked=true;
                                        else
                                            checkBox1.Checked = false;
                                        break;
                                    case 1:
                                        if (cc[j].Equals("Checked"))
                                            checkBox2.Checked = true;
                                        else
                                            checkBox2.Checked = false;
                                        break;
                                    case 2:
                                        if (cc[j].Equals("Checked"))
                                            checkBox3.Checked = true;
                                        else
                                            checkBox3.Checked = false;
                                        break;
                                    case 3:
                                        if (cc[j].Equals("Checked"))
                                            checkBox4.Checked = true;
                                        else
                                            checkBox4.Checked = false;
                                        break;
                                    case 4:
                                        if (cc[j].Equals("Checked"))
                                            checkBox5.Checked = true;
                                        else
                                            checkBox5.Checked = false;
                                        break;
                                    case 5:
                                        if (cc[j].Equals("Checked"))
                                            checkBox6.Checked = true;
                                        else
                                            checkBox6.Checked = false;
                                        break;
                                    case 6:
                                        if (cc[j].Equals("Checked"))
                                            checkBox7.Checked = true;
                                        else
                                            checkBox7.Checked = false;
                                        break;
                                }
                        }
                        break;
                    case 3:
                        numericUpDown2.Value = Convert.ToDecimal(a[i]);
                        break;

                }
            }
        }
        private void saveSettings()
        {
            string s="";
            if (listBox1.Items.Count == 0)
                s += "yok";
            else
                foreach (string ss in listBox1.Items)
                    s+=ss+",";
            s += "|" + numericUpDown1.Value.ToString() + "|" + checkBox1.CheckState.ToString() + "," + checkBox2.CheckState.ToString() + ","
                + checkBox3.CheckState.ToString() + "," + checkBox4.CheckState.ToString() + "," 
                + checkBox5.CheckState.ToString() + "," + checkBox6.CheckState.ToString()+","+checkBox7.CheckState.ToString()
                + "|" + numericUpDown2.Value.ToString();
            File.WriteAllText("ayarlar.sahibinden", s);
        }

        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            listBox1.SelectedIndex = listBox1.IndexFromPoint(e.X, e.Y);
            int index = this.listBox1.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches && e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                cms.Show(listBox1,e.Location);
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DialogResult a = MessageBox.Show("Silmek İstediğinizden Emin Misiniz?", "Silme Uyarısı", MessageBoxButtons.OKCancel);
            if (a == DialogResult.OK)
            {
                listBox1.Items.Remove(listBox1.SelectedItem);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Equals(""))
                MessageBox.Show("Kelime Girin.");
            else
            {
                listView2.Items.Clear();
                using (OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=sahibinden.mdb"))
                {
                    conn.Open();
                    //OleDbCommand sorgu = new OleDbCommand("SELECT * FROM sh WHERE ilan_tarihi >  #01/01/2014# and ilan_tarihi < #04/01/2014#", conn);
                    OleDbCommand sorgu = new OleDbCommand("Select * from sh where kelime like '%"+textBox2.Text+"%'", conn);
                    OleDbDataReader rd = sorgu.ExecuteReader();
                    int i = 0;
                    while (rd.Read())
                    {
                        i++;
                        listView2.Items.Add(new ListViewItem(new string[] { rd.GetValue(2).ToString(), rd.GetValue(3).ToString(), rd.GetValue(4).ToString(), rd.GetValue(5).ToString(), rd.GetValue(6).ToString(),rd.GetValue(7).ToString() }));
                    }
                    if (i == 0)
                        MessageBox.Show("Veri Yok.");
                    conn.Close();
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            DialogResult a = MessageBox.Show("Durdurmak istediğinizden emin misiniz?", "Durdurma", MessageBoxButtons.OKCancel);
            if (a == DialogResult.OK)
            {
                maint.Abort();
                MessageBox.Show("Arama Durduruldu..");
                button1.Enabled = true;
                button2.Enabled = true;
                textBox1.Enabled = true;
                AppendText(richTextBox1, Color.Red, "Arama Durduruldu." + DateTime.Now.ToString());
                stlbl.Text = "Arama Durduruldu. Beklemede..";
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            DialogResult a = MessageBox.Show("Tümünü Silmek İstediğinizden Emin Misiniz?", "Silme", MessageBoxButtons.OKCancel);
            if (a == DialogResult.OK)
            {
                listBox1.Items.Clear();
            }
        }
        private void asdasda1()
        {
            //ilanlar tablosu ilan_id sütunu önceki işlem görmüş ilanlar eklenecek.
            if (listBox1.Items.Count == 0)
            {
                MessageBox.Show("Ana Menüden Aranacak Kelime Ekleyin..");
                return;
            }
            sTime = DateTime.Now;
            AppendText(richTextBox1, Color.DarkViolet, "İşlemler Başladı.. " + sTime.ToString());
            a.Clear();
            button1.Enabled = false;
            button2.Enabled = false;
            textBox1.Enabled = false;
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                bool varmı = false;
                int var_adet = 0;
                listView1.Items.Clear();
                listView3.Items.Clear();
                bool adt = false;
                if (listBox1.Items[i].ToString().Contains("http://www.sahibinden.com/"))
                {
                    stlbl.Text = "Başladı.. Aranan Sayfa: " + listBox1.Items[i].ToString();
                    listBox1.SetSelected(i, true);
                    for (int ss = 0; ss <= 980; ss += 20)
                    {
                        string html = getHTML(listBox1.Items[i].ToString() + "?pagingOffset=" + ss);
                        if (html.Equals("yok"))
                        {
                            MessageBox.Show("Bağlantıda Bir Sorun Oluştu. Alınan Tüm Bilgiler, Database'ye ve ayarları yaptıysanız Excel'e yazıldı. Tekrar Çalıştırın. Hatalı Bölüm: 1 - Giriş Seviyesi");
                            string exc_isim = "";
                            if (listBox1.Items[i].ToString().Contains("http://www.sahibinden.com/"))
                                exc_isim = DateTime.Now.ToShortDateString() + "_" + listBox1.Items[i].ToString().Substring(listBox1.Items[i].ToString().LastIndexOf('/'), listBox1.Items[i].ToString().Length - 1) + ".xlsx";
                            else
                                exc_isim = DateTime.Now.ToString().Replace(' ', '_').Replace(':', '-').Replace('/', '-') + "_" + listBox1.Items[i].ToString() + ".xlsx";
                            emergencySituation(exc_isim);
                            reLoad();
                            return;
                        }
                        if (!html.Contains("<td class=\"searchResultsFirstColumn\">"))
                            break;
                        if (!adt)
                        {
                            string[] adet = html.Split(new string[] { "<div class=\"infoSearchResults\"> <div>" }, StringSplitOptions.None);
                            string[] adet1 = adet[1].Split(new string[] { "</div>" }, StringSplitOptions.None);
                            AppendText(richTextBox1, Color.Red, StripHTML(adet1[0]) + " " + DateTime.Now.ToString());
                            adt = true;
                        }
                        int syf = (ss / 20);
                        syf++;
                        AppendText(richTextBox1, Color.Red, "\"" + listBox1.Items[i].ToString() + "\" adlı sayfanın " + syf + ". sayfası işlemek için sıraya alınıyor.." + DateTime.Now.ToString());

                        string[] link = html.Split(new string[] { "<a class=\"classifiedTitle\" href=\"" }, StringSplitOptions.None);
                        for (int j = 1; j < link.Length; j++)
                        {

                            string[] lst = link[j].Split('"');
                            string[] iid = lst[0].Split('-');
                            string[] iid1 = iid[iid.Length - 1].Split('/');
                            if (KontrolID(iid1[0]))
                            {
                                a.Add(iid1[0]);
                            }
                        }
                    }
                }
                else
                {
                    stlbl.Text = "Başladı.. Aranan Kelime: " + listBox1.Items[i].ToString();
                    listBox1.SetSelected(i, true);
                    if (listBox1.Items[i].ToString().Contains(' '))
                        listBox1.Items[i].ToString().Replace(' ', '+');
                    for (int ss = 0; ss <= 980; ss += 20)
                    {
                        string html = getHTML("http://www.sahibinden.com/arama?query_text=" + listBox1.Items[i].ToString() + "&pagingOffset=" + ss);
                        if (html.Equals("yok"))
                        {
                            MessageBox.Show("Bağlantıda Bir Sorun Oluştu. Alınan Tüm Bilgiler, Database'ye ve ayarları yaptıysanız Excel'e yazıldı. Tekrar Çalıştırın. Hatalı Bölüm: 1 - Giriş Seviyesi");
                            string exc_isim = "";
                            if (listBox1.Items[i].ToString().Contains("http://www.sahibinden.com/"))
                                exc_isim = DateTime.Now.ToShortDateString() + "_" + listBox1.Items[i].ToString().Substring(listBox1.Items[i].ToString().LastIndexOf('/'), listBox1.Items[i].ToString().Length - 1) + ".xlsx";
                            else
                                exc_isim = DateTime.Now.ToString().Replace(' ', '_').Replace(':', '-').Replace('/', '-') + "_" + listBox1.Items[i].ToString() + ".xlsx";
                            emergencySituation(exc_isim);
                            reLoad();
                            return;
                        }
                        if (!html.Contains("<td class=\"searchResultsFirstColumn\">"))
                            break;
                        if (!adt)
                        {
                            string[] adet = html.Split(new string[] { "<div><h1>" }, StringSplitOptions.None);
                            string[] adet1 = adet[1].Split(new string[] { "</div>" }, StringSplitOptions.None);
                            AppendText(richTextBox1, Color.Red, StripHTML(adet1[0]) + " " + DateTime.Now.ToString());
                            adt = true;
                        }
                        int syf = (ss / 20);
                        syf++;
                        AppendText(richTextBox1, Color.Red, "\"" + listBox1.Items[i].ToString() + "\" adlı kelimenin " + syf + ". sayfası işlemek için sıraya alınıyor.." + DateTime.Now.ToString());

                        string[] link = html.Split(new string[] { "<a class=\"classifiedTitle\" href=\"" }, StringSplitOptions.None);
                        for (int j = 1; j < link.Length; j++)
                        {

                            string[] lst = link[j].Split('"');
                            string[] iid = lst[0].Split('-');
                            string[] iid1 = iid[iid.Length - 1].Split('/');
                            if (KontrolID(iid1[0]))
                            {
                                a.Add(iid1[0]);
                            }
                        }
                    }
                }
                MessageBox.Show(a[5].ToString());
                foreach (string aa in a)
                {
                    string ad = "Yok", ist = "Yok", cept = "Yok", adres = "Yok";
                    string info = getHTML("http://www.sahibinden.com/search.php?b%5Bsearch_text%5D=" + aa);
                    if (info.Equals("yok"))
                    {
                        MessageBox.Show("Bağlantıda Bir Sorun Oluştu. Alınan Tüm Bilgiler, Database'ye ve ayarları yaptıysanız Excel'e yazıldı. Tekrar Çalıştırın. Hatalı Bölüm: 2 - ID Aramaları");
                        string exc_isim = "";
                        if (listBox1.Items[i].ToString().Contains("http://www.sahibinden.com/"))
                            exc_isim = DateTime.Now.ToShortDateString() + "_" + listBox1.Items[i].ToString().Substring(listBox1.Items[i].ToString().LastIndexOf('/'), listBox1.Items[i].ToString().Length - 1) + ".xlsx";
                        else
                            exc_isim = DateTime.Now.ToString().Replace(' ', '_').Replace(':', '-').Replace('/', '-') + "_" + listBox1.Items[i].ToString() + ".xlsx";
                        emergencySituation(exc_isim);
                        reLoad();
                        return;
                    }
                    giris_say++;
                    listBox2.Items.Add(giris_say.ToString());
                    if (info.Contains("<td class=\"searchResultsSmallThumbnail\">") || info.Contains("18 yaş altı kişilerin girmesi yasaktır."))
                    {
                        AppendText(richTextBox1, Color.DarkBlue, "hatalı ilan");
                        continue;
                    }
                    if (info.Contains("Güvenlik Kontrolü"))
                    {
                        /*AppendText(richTextBox1, Color.Red, "Güvenlik Protokolü. Modeminizi Yeniden Başlatın. Captcha Kontrolü Başladı.");
                        string exc_isim = "";
                        if (listBox1.Items[i].ToString().Contains("http://www.sahibinden.com/"))
                            exc_isim = DateTime.Now.ToShortDateString() + "_" + listBox1.Items[i].ToString().Substring(listBox1.Items[i].ToString().LastIndexOf('/'), listBox1.Items[i].ToString().Length - 1) + ".xlsx";
                        else
                            exc_isim = DateTime.Now.ToString().Replace(' ', '_').Replace(':', '-').Replace('/', '-') + "_" + listBox1.Items[i].ToString() + ".xlsx";
                        emergencySituation(exc_isim);
                        return;*/
                        Process.Start("IExplore.exe", "http://www.sahibinden.com/ilan/emlak-konut-satilik-sahibinden-katta-tek-deniz-manzarali-3-plus1-ici-sifir-130-m2-161857851/detay/");
                        MessageBox.Show("Güvenlik Kontrolü Çıktı. Explorer'dan Giriş Yaptıktan Sonra Tamam'a Basın.");
                    }
                    if (info.Contains("<ul class=\"userContactInfo\">"))
                    {
                        string[] d1 = info.Split(new string[] { "</ul>" }, StringSplitOptions.None);
                        string[] dd = info.Split(new string[] { "<span>" }, StringSplitOptions.None);
                        for (int t = 1; t < dd.Length; t++)
                        {
                            string[] dd1 = dd[t].Split('<');
                            if (dd1[0].Contains("0 (5"))
                                cept = dd1[0];
                            else
                                ist = dd1[0];
                        }
                    }
                    if (info.Contains("classifiedUserContent"))
                    {
                        string[] dd = info.Split(new string[] { "<h5>" }, StringSplitOptions.None);
                        string[] dd1 = dd[1].Split('<');
                        ad = dd1[0];
                    }
                    string[] adr = info.Split(new string[] { "<div class=\"classifiedInfo\">" }, StringSplitOptions.None);
                    string[] adr1 = adr[1].Split(new string[] { "<ul class=\"classifiedInfoList\">" }, StringSplitOptions.None);
                    string[] adr2 = adr1[0].Split(new string[] { "\">" }, StringSplitOptions.None);
                    string adres_tut = "";
                    for (int s = 1; s < adr2.Length; s++)
                    {
                        string[] cc = adr2[s].Split('<');
                        cc[0] = cc[0].Trim();
                        if (s != adr2.Length - 1)
                            adres_tut += cc[0] + " / ";
                        else
                            adres_tut += cc[0];
                    }
                    adres = adres_tut;
                    bool yeni_kisi = false;
                    MessageBox.Show(ist + " - " + cept + " - " + adres_tut + " - " + ad);
                    if (!ist.Equals("Yok") && !cept.Equals("Yok"))
                    {
                        OleDbCommand sorgu = new OleDbCommand("SELECT * from sh where ceptel ='" + cept + "' or istel='" + ist + "'", conn);
                        conn.Open();
                        if (sorgu.ExecuteScalar() == null)
                            yeni_kisi = true;
                        conn.Close();

                    }
                    if (ist.Equals("Yok") && !cept.Equals("Yok"))
                    {
                        OleDbCommand sorgu = new OleDbCommand("SELECT * from sh where ceptel ='" + cept + "'", conn);
                        conn.Open();
                        if (sorgu.ExecuteScalar() == null)
                            yeni_kisi = true;
                        conn.Close();
                    }
                    if (!ist.Equals("Yok") && cept.Equals("Yok"))
                    {
                        OleDbCommand sorgu = new OleDbCommand("SELECT * from sh where istel ='" + ist + "'", conn);
                        conn.Open();
                        if (sorgu.ExecuteScalar() == null)
                            yeni_kisi = true;
                        conn.Close();
                    }
                    if (yeni_kisi)
                    {
                        varmı = true;
                        var_adet++;
                        listView1.Items.Add(new ListViewItem(new string[] { aa, ad, ist, cept, adres }));
                        listView3.Items.Add(new ListViewItem(new string[] { ad, cept }));
                        AppendText(richTextBox1, Color.DarkBlue, StripHTML(aa) + " ID'li ilandaki " + ad.ToUpper() + " isimli kişi veritabanına eklendi.");
                        ilanIDEkle(aa);
                        Ekle(aa, ad, ist, cept, adres, listBox1.Items[i].ToString());
                        data_say++;
                        toolStripStatusLabel5.Text = "Toplam " + data_say.ToString() + " adet yeni data işlendi.";
                    }
                }

                if (!varmı)
                    AppendText(richTextBox1, Color.DarkGreen, "\"" + listBox1.Items[i].ToString() + "\" adlı kelimede yeni ilan yok.");
                else
                    AppendText(richTextBox1, Color.DarkGreen, "\"" + listBox1.Items[i].ToString() + "\" adlı kelimede yeni " + var_adet + " ilan bulundu.");

                //tamamen bittiğinde bu satırdayız.
                //excele kaydet vsvs
                bool tarih = false, id = false, badi = false, bistel = false, bceptel = false, badres = false;

                if (varmı)//yeni ilan varsa işle
                    if (checkBox1.Checked)
                    {
                        SLDocument exc = new SLDocument();
                        SLStyle st = exc.CreateStyle();
                        st.SetFontBold(true);
                        if (checkBox7.Checked)
                        {
                            tarih = true;
                            exc.SetCellValue("A1", "İlan Tarihi");
                            exc.SetCellStyle("A1", st);
                        }
                        if (checkBox2.Checked)
                        {
                            id = true;
                            exc.SetCellValue("B1", "İlan ID");
                            exc.SetCellStyle("B1", st);
                        }
                        if (checkBox3.Checked)
                        {
                            badi = true;
                            exc.SetCellValue("C1", "Adı");
                            exc.SetCellStyle("C1", st);
                        }
                        if (checkBox4.Checked)
                        {
                            bistel = true;
                            exc.SetCellValue("D1", "İş Telefonu");
                            exc.SetCellStyle("D1", st);
                        }
                        if (checkBox5.Checked)
                        {
                            bceptel = true;
                            exc.SetCellValue("E1", "Cep Telefonu");
                            exc.SetCellStyle("E1", st);
                        }
                        if (checkBox6.Checked)
                        {
                            badres = true;
                            exc.SetCellValue("F1", "Adresi");
                            exc.SetCellStyle("F1", st);
                        }
                        int ex_sira = 2;
                        foreach (ListViewItem itemRow in listView1.Items)
                        {
                            string i_no = itemRow.SubItems[0].Text;
                            string adi = itemRow.SubItems[1].Text;
                            string istel = itemRow.SubItems[2].Text;
                            string ceptel = itemRow.SubItems[3].Text;
                            string adres = itemRow.SubItems[4].Text;
                            if (tarih)
                                exc.SetCellValue("A" + ex_sira, DateTime.Now.ToShortDateString());
                            if (id)
                                exc.SetCellValue("B" + ex_sira, i_no);
                            if (badi)
                                exc.SetCellValue("C" + ex_sira, adi);
                            if (bistel)
                                exc.SetCellValue("D" + ex_sira, istel);
                            if (bceptel)
                                exc.SetCellValue("E" + ex_sira, ceptel);
                            if (badres)
                                exc.SetCellValue("F" + ex_sira, adres);
                            ex_sira++;
                        }
                        if (exc.GetCellValueAsString("A1") == "")
                            exc.HideColumn("A");
                        if (exc.GetCellValueAsString("B1") == "")
                            exc.HideColumn("B");
                        if (exc.GetCellValueAsString("C1") == "")
                            exc.HideColumn("C");
                        if (exc.GetCellValueAsString("D1") == "")
                            exc.HideColumn("D");
                        if (exc.GetCellValueAsString("E1") == "")
                            exc.HideColumn("E");

                        string exc_isim = "";
                        if (listBox1.Items[i].ToString().Contains("http://www.sahibinden.com/"))
                            exc_isim = DateTime.Now.ToShortDateString() + "_" + listBox1.Items[i].ToString().Substring(listBox1.Items[i].ToString().LastIndexOf('/'), listBox1.Items[i].ToString().Length - 1) + ".xlsx";
                        else
                            exc_isim = DateTime.Now.ToString().Replace(' ', '_').Replace(':', '-').Replace('/', '-') + "_" + listBox1.Items[i].ToString() + ".xlsx";
                        exc.SaveAs(exc_isim);
                        AppendText(richTextBox1, Color.DarkOrange, "\"" + listBox1.Items[i].ToString() + "\" adlı kelimenin sonuçları " + exc_isim + " adlı excel dosyasına yazıldı.");
                    }


            }
            button1.Enabled = true;
            button2.Enabled = true;
            textBox1.Enabled = true;
            stlbl.Text = "İşlemler Tamamlandı. Beklemede..";
            AppendText(richTextBox1, Color.DarkSlateBlue, "Toplam " + data_say.ToString() + " adet data işlemden geçti.");
            AppendText(richTextBox1, Color.DarkViolet, calcTimeDiff(sTime, DateTime.Now).ToString());
            ts_adet.Text = getAdet().ToString() + " Adet Kayıt Bulunmaktadır.";
            listBox2.Items.Add(giris_say.ToString());

        }
        private void button12_Click(object sender, EventArgs e)
        {
            saveSettings();
        }
    }
}
