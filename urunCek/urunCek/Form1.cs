using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using OfficeOpenXml;
using System.Threading;
using System.Text.RegularExpressions;
using System.Web;
using System.Linq;

namespace urunCek
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private string getSource(string link)
        {
            label4.Text= link;
            string q = "";
            using (WebClient a = new WebClient())
            {
                a.Encoding = Encoding.GetEncoding("iso-8859-9");
                a.Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/34.0.1847.131 Safari/537.36";
                q = a.DownloadString(link);
            }
            return q;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
                MessageBox.Show("Linki Girin.");
            else
            {
                Thread a = new Thread(new ThreadStart(getNeccessary));
                a.Start();
            }
        }

        private void getNeccessary(){
            try
            {
                string a = getSource(textBox1.Text);
                List<string> lst = new List<string>();
                string[] b = a.Split(new string[] { "url: '" }, StringSplitOptions.None);
                string[] c = b[1].Split('\'');
                int i = 1;
                while (true)
                {
                    string q = getSource("http://esatis.tedarikmerkezi.org/" + c[0] + i);
                    string[] q1 = q.Split(new string[] { "<div class=\"defaultBlockContent\"><div class=\"urunAjax\">" }, StringSplitOptions.None);
                    string[] q2 = q1[1].Split(new string[] { "</div>" }, StringSplitOptions.None);
                    if (!q2[0].Contains("<div class=\"product\">"))
                        break;
                    i++;

                    string[] q3 = q1[1].Split(new string[] { "<script>" }, StringSplitOptions.None);
                    string[] q4 = q3[0].Split(new string[] { "<div class=\"product-image\"><a href=\"" }, StringSplitOptions.None);
                    for (int j = 1; j < q4.Length; j++)
                    {
                        string[] q5 = q4[j].Split('"');
                        lst.Add("http://esatis.tedarikmerkezi.org/" + q5[0]);
                    }
                }


                var fileName = "cikti__" + DateTime.Now.ToString("HH-mm-ss__dd-MM-yyyy") + ".xlsx";
                var file = new FileInfo(Application.StartupPath + "\\" + fileName);
                int satir = 1, sutun = 1;
                using (var package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sayfa1");
                    worksheet.Cells[1, 1].Value = "Ürün Adı";
                    worksheet.Cells[1, 2].Value = "Fiyatı";
                    worksheet.Cells[1, 3].Value = "Detay";
                    worksheet.Cells[1, 4].Value = "Resim1";
                    worksheet.Cells[1, 5].Value = "Resim2";
                    worksheet.Cells[1, 6].Value = "Resim3";
                    worksheet.Cells[1, 7].Value = "Resim4";
                    worksheet.Cells[1, 8].Value = "Resim5";
                    worksheet.Cells[1, 9].Value = "Resim6";
                    int topSay = 1;
                    
                    label3.Text = "Toplam: "+topSay.ToString()+"/" + lst.Count;
                    foreach (string s in lst)
                    {
                        label3.Text = topSay.ToString()+"/" + lst.Count;
                        topSay++;
                        satir++;
                        sutun = 1;
                        string q = getSource(s);
                        string[] bs = q.Split(new string[] { "<div class=\"baslik\">" }, StringSplitOptions.None);
                        string[] bs1 = bs[1].Split(new string[] { "</div>" }, StringSplitOptions.None);

                        worksheet.Cells[satir, sutun].Value = bs1[0];
                        sutun++;

                        string[] tl = q.Split(new string[] { "<div class=\"fiyat\"><span class=\"para\">" }, StringSplitOptions.None);
                        string[] tl1 = tl[1].Split(new string[] { " </span>" }, StringSplitOptions.None);

                        worksheet.Cells[satir, sutun].Value = tl1[0];
                        sutun++;

                        string[] ic = q.Split(new string[] { "<div class=\"tab-icerik\">" }, StringSplitOptions.None);
                        string[] ic1 = ic[1].Split(new string[] { "</div>" }, StringSplitOptions.None);

                        worksheet.Cells[satir, sutun].Value = ic[1];
                        sutun++;

                        string[] rs = q.Split(new string[] { "<ul class=\"pro_thumbers\">" }, StringSplitOptions.None);
                        string[] rs1 = rs[1].Split(new string[] { "src = \"templates/wstore/resizer.php?src=" }, StringSplitOptions.None);
                        for (int k = 1; k < rs1.Length; k++)
                        {
                            string[] rs2 = rs1[k].Split('&');

                            worksheet.Cells[satir, sutun].Value = "http://esatis.tedarikmerkezi.org/" + rs2[0];
                            sutun++;
                        }



                    }


                    package.Save();
                }
                MessageBox.Show("Tamamlandı.");
                label3.Text = "0/0";
                label4.Text = "Yok";
            }
            catch (Exception ex) { MessageBox.Show("Hata Oluştu."); richTextBox1.Text = ex.ToString(); };
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
        }
        private void button2_Click(object sender, EventArgs e)
        {

        }
        private void getYellowPages()
        {
            int say = 0;
            List<string> ls = new List<string>();
            string[] a = richTextBox2.Text.Split(new string[] { "<li><a href=\"" }, StringSplitOptions.None);
            for (int i = 1; i < a.Length; i++)
            {
                string[] b = a[i].Split(new string[] { "\">" }, StringSplitOptions.None);
                string[] c = b[1].Split('<');
                richTextBox1.Text+="<option value=\""+c[0]+"\">"+c[0]+"</option>"+Environment.NewLine;
            }
            /*foreach (string s in ls)
            {
                string q = getSource(s+"/1/50");
                if (!q.Contains("<a title=\"Son\" href=\""))
                {
                    listBox1.Items.Add(s.Substring(0, 60) + " Sayfa: 1");
                    continue;
                }
                string[] q1 = q.Split(new string[] { "<a title=\"Son\" href=\"" }, StringSplitOptions.None);
                string[] q2 = q1[1].Split(new string[] { "\">" }, StringSplitOptions.None);
                string[] q3 = q2[0].Split('/');
                listBox1.Items.Add(s.Substring(0,60) + " Sayfa: " + q3[q3.Length-2]);
                say += Convert.ToInt32(q3[q3.Length - 2]);
            }
            MessageBox.Show(say + " Adet Sayfa");*/
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            /*List<string> link = new List<string>();
            List<string> link2 = new List<string>();

            string[] a = webBrowser1.DocumentText.Split(new string[] { "<strong>Kategoriler</strong>" }, StringSplitOptions.None);
            string[] b = a[a.Length - 1].Split(new string[] { "<li class=\"categorymenu\"" }, StringSplitOptions.None);
            for (int i = 1; i < b.Length; i++)
            {
                string[] c = b[i].Split(new string[] { "<a href=\"" }, StringSplitOptions.None);
                string[] d = c[1].Split('"');
                link.Add("http://www.on-netb2b.com" + d[0]);
            }
            int pn = 1;
            foreach (string l in link)
            {
                pn = 1;
                while (true)
                {
                    webBrowser1.Navigate(l + "?pagenumber=" + pn);
                    while (webBrowser1.ReadyState != WebBrowserReadyState.Complete)
                        Application.DoEvents();
                    pn++;
                    if (!webBrowser1.DocumentText.Contains("<div class=\"product-viewmode\">"))
                        break;

                    string[] w1 = webBrowser1.DocumentText.Split(new string[] { "<div class=\"picture\">" }, StringSplitOptions.None);
                    for (int j = 1; j < w1.Length; j++)
                    {
                        string[] w2 = w1[j].Split(new string[] { "<a href=\"" }, StringSplitOptions.None);
                        string[] w3 = w2[1].Split('"');
                        link2.Add("http://www.on-netb2b.com" + w3[0]);
                    }
                }
            }
            System.IO.File.WriteAllLines("linkler.txt", link2.ToArray());
            return;*/

            string[] link = System.IO.File.ReadAllLines("linkler.txt");
            List<string> link2 = link.Skip(Convert.ToInt32(numericUpDown1.Value)).Take(Convert.ToInt32(numericUpDown2.Value)).ToList();
            var fileName = "netb2b__"+numericUpDown1.Value.ToString()+"-"+numericUpDown2.Value.ToString()+ ".xlsx";
            var file = new FileInfo(Application.StartupPath + "\\" + fileName);
            int satir = 1, sutun = 1;
            using (var package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sayfa1");
                worksheet.Cells[1, 1].Value = "Ürün Adı";
                worksheet.Cells[1, 2].Value = "Fiyatı";
                worksheet.Cells[1, 3].Value = "Kategori";
                worksheet.Cells[1, 4].Value = "Detay";
                worksheet.Cells[1, 5].Value = "Türkiye Stok";
                worksheet.Cells[1, 6].Value = "Avrupa Stok";
                worksheet.Cells[1, 7].Value = "Resim1";
                worksheet.Cells[1, 8].Value = "Resim2";
                worksheet.Cells[1, 9].Value = "Resim3";
                worksheet.Cells[1, 10].Value = "Resim4";
                worksheet.Cells[1, 11].Value = "Resim5";
                worksheet.Cells[1, 12].Value = "Resim6";
                int goster = 1;
                foreach (string s in link2)
                {
                    label9.Text=goster.ToString()+"/"+link2.Count.ToString();
                    goster++;
                    webBrowser1.Navigate(s);
                    while (webBrowser1.ReadyState != WebBrowserReadyState.Complete)
                        Application.DoEvents();
                    W8();

                    if (webBrowser1.DocumentText.Contains("The page you requested was not found")|| webBrowser1.DocumentText.Contains("Bad Request - Invalid URL") || webBrowser1.DocumentText.Contains("Server Error"))
                        continue;
                    sutun++; satir = 1;
                    string[] adi = webBrowser1.DocumentText.Split(new string[] { "<strong class=\"current-item\">" }, StringSplitOptions.None);
                    string[] adi1 = adi[1].Split(new string[] { "</strong>" }, StringSplitOptions.None);

                    worksheet.Cells[sutun, satir].Value = HttpUtility.HtmlDecode(adi1[0]);
                    satir++;

                    string[] fiyati = webBrowser1.DocumentText.Split(new string[] { "<span  itemprop=\"price\"  >" }, StringSplitOptions.None);
                    string[] fiyati1 = fiyati[1].Split(new string[] { "</span>" }, StringSplitOptions.None);

                    fiyati1[0] = fiyati1[0].Trim();

                    worksheet.Cells[sutun, satir].Value = HttpUtility.HtmlDecode(fiyati1[0]);
                    satir++;

                    string[] kat = webBrowser1.DocumentText.Split(new string[] { "<span itemprop=\"title\">" }, StringSplitOptions.None);
                    string katt = "";
                    for (int j = 2; j < kat.Length; j++)
                    {
                        string[] kat1 = kat[j].Split(new string[] { "</span>" }, StringSplitOptions.None);
                        katt += kat1[0] + " / ";
                    }

                    worksheet.Cells[sutun, satir].Value = HttpUtility.HtmlDecode(katt);
                    satir++;
                    if (webBrowser1.DocumentText.Contains("full-description"))
                    {
                        string[] desc = webBrowser1.DocumentText.Split(new string[] { "<div class=\"full-description\" style=\"overflow-x:scroll;\" itemprop=\"description\">" }, StringSplitOptions.None);
                        string[] desc1 = desc[1].Split(new string[] { "</div>" }, StringSplitOptions.None);

                        //desc1[0] = StripHTML(desc1[0]).Trim();

                        worksheet.Cells[sutun, satir].Value = HttpUtility.HtmlDecode(desc1[0]);
                        satir++;
                    }
                    else {
                        worksheet.Cells[sutun, satir].Value = "Yok";
                        satir++;
                    }

                    string[] trS = webBrowser1.DocumentText.Split(new string[]{"Türkiye Stok: </span><span class=\"value\">"},StringSplitOptions.None);
                    string[] trS1 = trS[1].Split(new string[] { "</span>" }, StringSplitOptions.None);

                    worksheet.Cells[sutun, satir].Value = HttpUtility.HtmlDecode(trS1[0]);
                    satir++;

                    string[] fS = webBrowser1.DocumentText.Split(new string[] { "Avrupa Stok: </span><span class=\"value\">" }, StringSplitOptions.None);
                    string[] fS1 = fS[1].Split(new string[] { "</span>" }, StringSplitOptions.None);

                    worksheet.Cells[sutun, satir].Value = HttpUtility.HtmlDecode(fS1[0]);
                    satir++;

                    string[] p1 = webBrowser1.DocumentText.Split(new string[] { "<div class=\"picture\">" }, StringSplitOptions.None);
                    string[] p11 = p1[1].Split(new string[] { "<div class=\"overview\">" }, StringSplitOptions.None);
                    string[] pics = p11[0].Split(new string[] { "http://resim.on-net.com.tr/thumbnailer.aspx?image=" }, StringSplitOptions.None);
                    List<string> picLast = new List<string>();
                    for (int j = 1; j < pics.Length; j++)
                    {
                        string[] p2 = pics[j].Split('&');
                        if (!picLast.Contains(HttpUtility.HtmlDecode("http://resim.on-net.com.tr/" + p2[0])))
                        {
                            worksheet.Cells[sutun, satir].Value = HttpUtility.HtmlDecode("http://resim.on-net.com.tr/" + p2[0]);
                            picLast.Add(HttpUtility.HtmlDecode("http://resim.on-net.com.tr/" + p2[0]));
                        }
                        satir++;
                    }
                }
                MessageBox.Show("Bitti.");
                package.Save();
            }
            label7.Text = "Yok";
            label9.Text = "0/0";

        }
        const string HTML_TAG_PATTERN = "<.*?>";
        static string StripHTML(string inputString)
        {
            return Regex.Replace
              (inputString, HTML_TAG_PATTERN, string.Empty);
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
                timer1.Enabled = false;
        }
        private void getB2B()
        {
            while (!loaded)
            {
                Application.DoEvents();
            }
            Thread.Sleep(5000);
            button4.PerformClick();


            //http://www.on-netb2b.com/logout
        }
        bool loaded = false;
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if(webBrowser1.Url.ToString()=="https://www.on-netb2b.com/login")
                button4.Enabled = true;
        }

        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            label7.Text = webBrowser1.Url.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            webBrowser1.Document.GetElementById("Username").InnerText = "yonetim@aarti.com.tr";
            webBrowser1.Document.GetElementById("Password").InnerText = "55514145";
            foreach (HtmlElement el in webBrowser1.Document.GetElementsByTagName("input"))
            {
                if (el.GetAttribute("value").Equals("Giriş"))
                {
                    el.InvokeMember("click");
                    break;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            sec--;
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            List<string> link = new List<string>();
            List<string> link2 = new List<string>();
            string[] a = webBrowser1.DocumentText.Split(new string[] { "<strong>Kategoriler</strong>" }, StringSplitOptions.None);
            string[] b = a[a.Length - 1].Split(new string[] { "<li class=\"categorymenu\"" }, StringSplitOptions.None);
            for (int i = 1; i < b.Length; i++)
            {
                string[] c = b[i].Split(new string[] { "<a href=\"" }, StringSplitOptions.None);
                string[] d = c[1].Split('"');
                link.Add("http://www.on-netb2b.com" + d[0]);
            }
            int pn = 1;
            var fileName = "netb2b__" + DateTime.Now.ToString("HH-mm-ss__dd-MM-yyyy") + ".xlsx";
            var file = new FileInfo(Application.StartupPath + "\\" + fileName);
            int satir = 1, sutun = 1;
            using (var package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sayfa1");
                worksheet.Cells[1, 1].Value = "Ürün Adı";
                worksheet.Cells[1, 2].Value = "Fiyatı";
                worksheet.Cells[1, 3].Value = "Kategori";
                worksheet.Cells[1, 4].Value = "Detay";
                worksheet.Cells[1, 5].Value = "Türkiye Stok";
                worksheet.Cells[1, 6].Value = "Avrupa Stok";
                worksheet.Cells[1, 7].Value = "Resim1";
                worksheet.Cells[1, 8].Value = "Resim2";
                worksheet.Cells[1, 9].Value = "Resim3";
            }
            foreach (string l in link)
            {
                while (true)
                {
                    webBrowser1.Navigate(l + "?pagenumber=" + pn);
                    W8();
                    pn++;
                    if (!webBrowser1.DocumentText.Contains("<div class=\"product-viewmode\">"))
                        break;

                    string[] w1 = webBrowser1.DocumentText.Split(new string[] { "<div class=\"picture\">" }, StringSplitOptions.None);
                    for (int j = 1; j < w1.Length; j++)
                    {
                        string[] w2 = w1[j].Split(new string[] { "<a href=\"" }, StringSplitOptions.None);
                        string[] w3 = w2[1].Split('"');
                        link2.Add("http://www.on-netb2b.com" + w3[0]);
                    }
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            webBrowser2.Document.GetElementById("l_username").InnerText = "yonetim@cnmevim.com";
            webBrowser2.Document.GetElementById("l_password").InnerText = "123456";
            foreach (HtmlElement el in webBrowser2.Document.GetElementsByTagName("img"))
            {
                if (el.GetAttribute("alt").Equals("login"))
                {
                    el.InvokeMember("click");
                    break;
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if(comboBox1.Text=="--Duvar Resmi")
            {
                var fileName = "jasmin2020_duvar_resimleri-" + DateTime.Now.ToString("HH-mm-ss__dd-MM-yyyy") + ".xlsx";
                var file = new FileInfo(Application.StartupPath + "\\" + fileName);
                int satir = 1, sutun = 1;
                using (var package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sayfa1");
                    worksheet.Cells[1, 1].Value = "Ürün Adı";
                    worksheet.Cells[1, 2].Value = "Fiyatı";
                    worksheet.Cells[1, 3].Value = "Resmi";
                    worksheet.Cells[1, 4].Value = "Açıklaması";
                    worksheet.Cells[1, 5].Value = "Opsiyon1";
                    worksheet.Cells[1, 6].Value = "Opsiyon2";
                    worksheet.Cells[1, 7].Value = "Opsiyon3";

                    for (int i = 1; i <= 7; i++)
                    {
                        webBrowser2.Navigate("http://www.jasmin2020.com/page.php?page="+i.ToString()+"&act=kategoriGoster&catID=91&markaID=spkomut_HEPSI&name=duvar-resmi&orderBy=&op=");
                        while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                            Application.DoEvents();
                        //W8();
                        string[] a = webBrowser2.DocumentText.Split(new string[] { "<div class=\"product-gallery\">" }, StringSplitOptions.None);
                        string[] a1 = a[1].Split(new string[] { "<div class=\"pagination\">" }, StringSplitOptions.None);
                        string[] a2 = a1[0].Split(new string[] { "<a href=\"" }, StringSplitOptions.None);
                        for (int j = 1; j < a2.Length; j++)
                        {
                            satir++; sutun = 1;
                            string[] a3 = a2[j].Split('"');
                            webBrowser2.Navigate("http://www.jasmin2020.com/" + a3[0]);
                            while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                                Application.DoEvents();
                            //W8();

                            StreamReader sr = new StreamReader(this.webBrowser2.DocumentStream, Encoding.GetEncoding("iso-8859-9"));
                            string source = sr.ReadToEnd();


                            string[] a4 = source.Split(new string[]{"<meta property=\"og:image\" content=\""},StringSplitOptions.None);
                            string[] a5 = a4[1].Split('"');

                            string[] a6 = source.Split(new string[] { "<h1>" }, StringSplitOptions.None);
                            string[] a7 = a6[1].Split(new string[] { "</h1>" }, StringSplitOptions.None);

                            string[] a13 = source.Split(new string[] { "<dt>Satış fiyatı :</dt>" }, StringSplitOptions.None);
                            string[] a14 = a13[1].Split(new string[] { "<dd>" }, StringSplitOptions.None);
                            string[] a15 = a14[1].Split(new string[] { "</dd>" }, StringSplitOptions.None);

                            string[] a16 = source.Split(new string[]{"<div id=\"dstoretab1\" class=\"dstoretab\">"},StringSplitOptions.None);
                            string[] a17 = a16[1].Split(new string[]{"<div id=\"dstoretab2\" class=\"dstoretab\">"},StringSplitOptions.None);

                            worksheet.Cells[satir, sutun].Value = a7[0];
                            sutun++;
                            worksheet.Cells[satir, sutun].Value = a15[0];
                            sutun++;
                            worksheet.Cells[satir, sutun].Value = a5[0];
                            sutun++;
                            worksheet.Cells[satir, sutun].Value = a17[0];
                            sutun++;
                            
                            string[] a8 = source.Split(new string[] { "<li value='" }, StringSplitOptions.None);
                            for (int k = 1; k < a8.Length; k++)
                            {
                                string[] a9 = a8[k].Split('\'');
                                string[] a11 = a8[k].Split(new string[] { "<b>" }, StringSplitOptions.None);
                                string[] a12 = a11[1].Split('<');
                                worksheet.Cells[satir, sutun].Value = a9[0] + " (" + a12[0] + ")";
                                sutun++;
                            }

                            
                        }
                    }
                    package.Save();
                }
                MessageBox.Show("Tamamlandı.");
            }
            else if (comboBox1.Text == "--Dik Duvar Resimleri")
            {
                var fileName = "jasmin2020_dik_duvar_resimleri-" + DateTime.Now.ToString("HH-mm-ss__dd-MM-yyyy") + ".xlsx";
                var file = new FileInfo(Application.StartupPath + "\\" + fileName);
                int satir = 1, sutun = 1;
                using (var package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sayfa1");
                    worksheet.Cells[1, 1].Value = "Ürün Adı";
                    worksheet.Cells[1, 2].Value = "Fiyatı";
                    worksheet.Cells[1, 3].Value = "Resmi";
                    worksheet.Cells[1, 4].Value = "Açıklaması";
                    worksheet.Cells[1, 5].Value = "Opsiyon1";
                    worksheet.Cells[1, 6].Value = "Opsiyon2";
                    worksheet.Cells[1, 7].Value = "Opsiyon3";

                    for (int i = 1; i <= 2; i++)
                    {
                        webBrowser2.Navigate("http://www.jasmin2020.com/page.php?page=" + i.ToString() + "&act=kategoriGoster&catID=86&markaID=spkomut_HEPSI&name=dik-duvar-resimleri&orderBy=&op=");
                        while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                            Application.DoEvents();
                        //W8();
                        string[] a = webBrowser2.DocumentText.Split(new string[] { "<div class=\"product-gallery\">" }, StringSplitOptions.None);
                        string[] a1 = a[1].Split(new string[] { "<div class=\"pagination\">" }, StringSplitOptions.None);
                        string[] a2 = a1[0].Split(new string[] { "<a href=\"" }, StringSplitOptions.None);
                        for (int j = 1; j < a2.Length; j++)
                        {
                            satir++; sutun = 1;
                            string[] a3 = a2[j].Split('"');
                            webBrowser2.Navigate("http://www.jasmin2020.com/" + a3[0]);
                            while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                                Application.DoEvents();
                            //W8();

                            StreamReader sr = new StreamReader(this.webBrowser2.DocumentStream, Encoding.GetEncoding("iso-8859-9"));
                            string source = sr.ReadToEnd();


                            string[] a4 = source.Split(new string[] { "<meta property=\"og:image\" content=\"" }, StringSplitOptions.None);
                            string[] a5 = a4[1].Split('"');

                            string[] a6 = source.Split(new string[] { "<h1>" }, StringSplitOptions.None);
                            string[] a7 = a6[1].Split(new string[] { "</h1>" }, StringSplitOptions.None);

                            string[] a13 = source.Split(new string[] { "<dt>Satış fiyatı :</dt>" }, StringSplitOptions.None);
                            string[] a14 = a13[1].Split(new string[] { "<dd>" }, StringSplitOptions.None);
                            string[] a15 = a14[1].Split(new string[] { "</dd>" }, StringSplitOptions.None);

                            string[] a16 = source.Split(new string[] { "<div id=\"dstoretab1\" class=\"dstoretab\">" }, StringSplitOptions.None);
                            string[] a17 = a16[1].Split(new string[] { "<div id=\"dstoretab2\" class=\"dstoretab\">" }, StringSplitOptions.None);

                            worksheet.Cells[satir, sutun].Value = a7[0];
                            sutun++;
                            worksheet.Cells[satir, sutun].Value = a15[0];
                            sutun++;
                            worksheet.Cells[satir, sutun].Value = a5[0];
                            sutun++;
                            worksheet.Cells[satir, sutun].Value = a17[0];
                            sutun++;

                            string[] a8 = source.Split(new string[] { "<li value='" }, StringSplitOptions.None);
                            for (int k = 1; k < a8.Length; k++)
                            {
                                string[] a9 = a8[k].Split('\'');
                                string[] a11 = a8[k].Split(new string[] { "<b>" }, StringSplitOptions.None);
                                string[] a12 = a11[1].Split('<');
                                worksheet.Cells[satir, sutun].Value = a9[0] + " (" + a12[0] + ")";
                                sutun++;
                            }


                        }
                    }
                    package.Save();
                }
                MessageBox.Show("Tamamlandı.");
            }
            else if (comboBox1.Text == "--Çocuk Odası Duvar Resimleri")
            {
                var fileName = "jasmin2020_cocuk_odasi_duvar_resimleri-" + DateTime.Now.ToString("HH-mm-ss__dd-MM-yyyy") + ".xlsx";
                var file = new FileInfo(Application.StartupPath + "\\" + fileName);
                int satir = 1, sutun = 1;
                using (var package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sayfa1");
                    worksheet.Cells[1, 1].Value = "Ürün Adı";
                    worksheet.Cells[1, 2].Value = "Fiyatı";
                    worksheet.Cells[1, 3].Value = "Resmi";
                    worksheet.Cells[1, 4].Value = "Açıklaması";
                    worksheet.Cells[1, 5].Value = "Opsiyon1";
                    worksheet.Cells[1, 6].Value = "Opsiyon2";
                    worksheet.Cells[1, 7].Value = "Opsiyon3";

                    for (int i = 1; i <= 4; i++)
                    {
                        webBrowser2.Navigate("http://www.jasmin2020.com/page.php?page=" + i.ToString() + "&act=kategoriGoster&catID=60&markaID=spkomut_HEPSI&name=cocuk-odasi-duvar-resimleri&orderBy=&op=");
                        while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                            Application.DoEvents();
                        //W8();
                        string[] a = webBrowser2.DocumentText.Split(new string[] { "<div class=\"product-gallery\">" }, StringSplitOptions.None);
                        string[] a1 = a[1].Split(new string[] { "<div class=\"pagination\">" }, StringSplitOptions.None);
                        string[] a2 = a1[0].Split(new string[] { "<a href=\"" }, StringSplitOptions.None);
                        for (int j = 1; j < a2.Length; j++)
                        {
                            satir++; sutun = 1;
                            string[] a3 = a2[j].Split('"');
                            webBrowser2.Navigate("http://www.jasmin2020.com/" + a3[0]);
                            while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                                Application.DoEvents();
                            //W8();

                            StreamReader sr = new StreamReader(this.webBrowser2.DocumentStream, Encoding.GetEncoding("iso-8859-9"));
                            string source = sr.ReadToEnd();


                            string[] a4 = source.Split(new string[] { "<meta property=\"og:image\" content=\"" }, StringSplitOptions.None);
                            string[] a5 = a4[1].Split('"');

                            string[] a6 = source.Split(new string[] { "<h1>" }, StringSplitOptions.None);
                            string[] a7 = a6[1].Split(new string[] { "</h1>" }, StringSplitOptions.None);

                            string[] a13 = source.Split(new string[] { "<dt>Satış fiyatı :</dt>" }, StringSplitOptions.None);
                            string[] a14 = a13[1].Split(new string[] { "<dd>" }, StringSplitOptions.None);
                            string[] a15 = a14[1].Split(new string[] { "</dd>" }, StringSplitOptions.None);

                            string[] a16 = source.Split(new string[] { "<div id=\"dstoretab1\" class=\"dstoretab\">" }, StringSplitOptions.None);
                            string[] a17 = a16[1].Split(new string[] { "<div id=\"dstoretab2\" class=\"dstoretab\">" }, StringSplitOptions.None);

                            worksheet.Cells[satir, sutun].Value = a7[0];
                            sutun++;
                            worksheet.Cells[satir, sutun].Value = a15[0];
                            sutun++;
                            worksheet.Cells[satir, sutun].Value = a5[0];
                            sutun++;
                            worksheet.Cells[satir, sutun].Value = a17[0];
                            sutun++;

                            string[] a8 = source.Split(new string[] { "<li value='" }, StringSplitOptions.None);
                            for (int k = 1; k < a8.Length; k++)
                            {
                                string[] a9 = a8[k].Split('\'');
                                string[] a11 = a8[k].Split(new string[] { "<b>" }, StringSplitOptions.None);
                                string[] a12 = a11[1].Split('<');
                                worksheet.Cells[satir, sutun].Value = a9[0] + " (" + a12[0] + ")";
                                sutun++;
                            }


                        }
                    }
                    package.Save();
                }
                MessageBox.Show("Tamamlandı.");
            }
            else if (comboBox1.Text == "--Pencereden Duvar Resimleri")
            {
                var fileName = "jasmin2020_pencereden_duvar_resimleri-" + DateTime.Now.ToString("HH-mm-ss__dd-MM-yyyy") + ".xlsx";
                var file = new FileInfo(Application.StartupPath + "\\" + fileName);
                int satir = 1, sutun = 1;
                using (var package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sayfa1");
                    worksheet.Cells[1, 1].Value = "Ürün Adı";
                    worksheet.Cells[1, 2].Value = "Fiyatı";
                    worksheet.Cells[1, 3].Value = "Resmi";
                    worksheet.Cells[1, 4].Value = "Açıklaması";
                    worksheet.Cells[1, 5].Value = "Opsiyon1";
                    worksheet.Cells[1, 6].Value = "Opsiyon2";
                    worksheet.Cells[1, 7].Value = "Opsiyon3";
                    webBrowser2.Navigate("http://www.jasmin2020.com/pencereden-duvar-resimleri-kat59.html");
                        while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                            Application.DoEvents();
                        //W8();
                        string[] a = webBrowser2.DocumentText.Split(new string[] { "<div class=\"product-gallery\">" }, StringSplitOptions.None);
                        string[] a1 = a[1].Split(new string[] { "<div class=\"pagination\">" }, StringSplitOptions.None);
                        string[] a2 = a1[0].Split(new string[] { "<a href=\"" }, StringSplitOptions.None);
                        for (int j = 1; j < a2.Length; j++)
                        {
                            satir++; sutun = 1;
                            string[] a3 = a2[j].Split('"');
                            webBrowser2.Navigate("http://www.jasmin2020.com/" + a3[0]);
                            while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                                Application.DoEvents();
                            //W8();

                            StreamReader sr = new StreamReader(this.webBrowser2.DocumentStream, Encoding.GetEncoding("iso-8859-9"));
                            string source = sr.ReadToEnd();


                            string[] a4 = source.Split(new string[] { "<meta property=\"og:image\" content=\"" }, StringSplitOptions.None);
                            string[] a5 = a4[1].Split('"');

                            string[] a6 = source.Split(new string[] { "<h1>" }, StringSplitOptions.None);
                            string[] a7 = a6[1].Split(new string[] { "</h1>" }, StringSplitOptions.None);

                            string[] a13 = source.Split(new string[] { "<dt>Satış fiyatı :</dt>" }, StringSplitOptions.None);
                            string[] a14 = a13[1].Split(new string[] { "<dd>" }, StringSplitOptions.None);
                            string[] a15 = a14[1].Split(new string[] { "</dd>" }, StringSplitOptions.None);

                            string[] a16 = source.Split(new string[] { "<div id=\"dstoretab1\" class=\"dstoretab\">" }, StringSplitOptions.None);
                            string[] a17 = a16[1].Split(new string[] { "<div id=\"dstoretab2\" class=\"dstoretab\">" }, StringSplitOptions.None);

                            worksheet.Cells[satir, sutun].Value = a7[0];
                            sutun++;
                            worksheet.Cells[satir, sutun].Value = a15[0];
                            sutun++;
                            worksheet.Cells[satir, sutun].Value = a5[0];
                            sutun++;
                            worksheet.Cells[satir, sutun].Value = a17[0];
                            sutun++;

                            string[] a8 = source.Split(new string[] { "<li value='" }, StringSplitOptions.None);
                            for (int k = 1; k < a8.Length; k++)
                            {
                                string[] a9 = a8[k].Split('\'');
                                string[] a11 = a8[k].Split(new string[] { "<b>" }, StringSplitOptions.None);
                                string[] a12 = a11[1].Split('<');
                                worksheet.Cells[satir, sutun].Value = a9[0] + " (" + a12[0] + ")";
                                sutun++;
                            }


                        }
                    package.Save();
                }
                MessageBox.Show("Tamamlandı.");
            }
            else if (comboBox1.Text == "--İthal Duvar Kağıtları")
            {
                var fileName = "jasmin2020_ithal_duvar_kagitlari-" + DateTime.Now.ToString("HH-mm-ss__dd-MM-yyyy") + ".xlsx";
                var file = new FileInfo(Application.StartupPath + "\\" + fileName);
                int satir = 1, sutun = 1;
                using (var package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sayfa1");
                    worksheet.Cells[1, 1].Value = "Ürün Adı";
                    worksheet.Cells[1, 2].Value = "Fiyatı";
                    worksheet.Cells[1, 3].Value = "Resmi";
                    worksheet.Cells[1, 4].Value = "Açıklaması";
                    for (int i = 1; i <= 2; i++)
                    {
                        webBrowser2.Navigate("http://www.jasmin2020.com/page.php?page=" + i.ToString() + "&act=kategoriGoster&catID=98&markaID=spkomut_HEPSI&name=duvar-kagitlari&orderBy=&op=");
                        while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                            Application.DoEvents();
                        //W8();
                        string[] a = webBrowser2.DocumentText.Split(new string[] { "<div class=\"product-gallery\">" }, StringSplitOptions.None);
                        string[] a1 = a[1].Split(new string[] { "<div class=\"pagination\">" }, StringSplitOptions.None);
                        string[] a2 = a1[0].Split(new string[] { "<a href=\"" }, StringSplitOptions.None);
                        for (int j = 1; j < a2.Length; j++)
                        {
                            satir++; sutun = 1;
                            string[] a3 = a2[j].Split('"');
                            webBrowser2.Navigate("http://www.jasmin2020.com/" + a3[0]);
                            while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                                Application.DoEvents();
                            //W8();

                            StreamReader sr = new StreamReader(this.webBrowser2.DocumentStream, Encoding.GetEncoding("iso-8859-9"));
                            string source = sr.ReadToEnd();


                            string[] a4 = source.Split(new string[] { "<meta property=\"og:image\" content=\"" }, StringSplitOptions.None);
                            string[] a5 = a4[1].Split('"');

                            string[] a6 = source.Split(new string[] { "<h1>" }, StringSplitOptions.None);
                            string[] a7 = a6[1].Split(new string[] { "</h1>" }, StringSplitOptions.None);

                            string[] a13 = source.Split(new string[] { "<dt>Satış fiyatı :</dt>" }, StringSplitOptions.None);
                            string[] a14 = a13[1].Split(new string[] { "<dd>" }, StringSplitOptions.None);
                            string[] a15 = a14[1].Split(new string[] { "</dd>" }, StringSplitOptions.None);

                            string[] a16 = source.Split(new string[] { "<div id=\"dstoretab1\" class=\"dstoretab\">" }, StringSplitOptions.None);
                            string[] a17 = a16[1].Split(new string[] { "<div id=\"dstoretab2\" class=\"dstoretab\">" }, StringSplitOptions.None);

                            worksheet.Cells[satir, sutun].Value = a7[0];
                            sutun++;
                            worksheet.Cells[satir, sutun].Value = a15[0];
                            sutun++;
                            worksheet.Cells[satir, sutun].Value = a5[0];
                            sutun++;
                            worksheet.Cells[satir, sutun].Value = a17[0];
                            sutun++;

                        }
                    }
                    package.Save();
                }
                MessageBox.Show("Tamamlandı.");
            }
            else if (comboBox1.Text == "ÇOCUK ODASI BORDÜR")
            {
                var fileName = "jasmin2020_duvar_kagitlari-" + DateTime.Now.ToString("HH-mm-ss__dd-MM-yyyy") + ".xlsx";
                var file = new FileInfo(Application.StartupPath + "\\" + fileName);
                int satir = 1, sutun = 1;
                using (var package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sayfa1");
                    worksheet.Cells[1, 1].Value = "Ürün Adı";
                    worksheet.Cells[1, 2].Value = "Fiyatı";
                    worksheet.Cells[1, 3].Value = "Resmi";
                    worksheet.Cells[1, 4].Value = "Açıklaması";
                    webBrowser2.Navigate("http://www.jasmin2020.com/cocuk-odasi-bordurler-kat93.html");
                    while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                        Application.DoEvents();
                    //W8();
                    string[] a = webBrowser2.DocumentText.Split(new string[] { "<div class=\"product-gallery\">" }, StringSplitOptions.None);
                    string[] a1 = a[1].Split(new string[] { "<div class=\"pagination\">" }, StringSplitOptions.None);
                    string[] a2 = a1[0].Split(new string[] { "<a href=\"" }, StringSplitOptions.None);
                    for (int j = 1; j < a2.Length; j++)
                    {
                        satir++; sutun = 1;
                        string[] a3 = a2[j].Split('"');
                        webBrowser2.Navigate("http://www.jasmin2020.com/" + a3[0]);
                        while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                            Application.DoEvents();
                        //W8();

                        StreamReader sr = new StreamReader(this.webBrowser2.DocumentStream, Encoding.GetEncoding("iso-8859-9"));
                        string source = sr.ReadToEnd();


                        string[] a4 = source.Split(new string[] { "<meta property=\"og:image\" content=\"" }, StringSplitOptions.None);
                        string[] a5 = a4[1].Split('"');

                        string[] a6 = source.Split(new string[] { "<h1>" }, StringSplitOptions.None);
                        string[] a7 = a6[1].Split(new string[] { "</h1>" }, StringSplitOptions.None);

                        string[] a13 = source.Split(new string[] { "<dt>Satış fiyatı :</dt>" }, StringSplitOptions.None);
                        string[] a14 = a13[1].Split(new string[] { "<dd>" }, StringSplitOptions.None);
                        string[] a15 = a14[1].Split(new string[] { "</dd>" }, StringSplitOptions.None);

                        string[] a16 = source.Split(new string[] { "<div id=\"dstoretab1\" class=\"dstoretab\">" }, StringSplitOptions.None);
                        string[] a17 = a16[1].Split(new string[] { "<div id=\"dstoretab2\" class=\"dstoretab\">" }, StringSplitOptions.None);

                        worksheet.Cells[satir, sutun].Value = a7[0];
                        sutun++;
                        worksheet.Cells[satir, sutun].Value = a15[0];
                        sutun++;
                        worksheet.Cells[satir, sutun].Value = a5[0];
                        sutun++;
                        worksheet.Cells[satir, sutun].Value = a17[0];
                        sutun++;

                    }
                    package.Save();
                }
                MessageBox.Show("Tamamlandı.");
            }
            else if (comboBox1.Text == "--2 Parçalı Tablolar")
            {
                var fileName = "jasmin2020_2_parcali_tablolar-" + DateTime.Now.ToString("HH-mm-ss__dd-MM-yyyy") + ".xlsx";
                var file = new FileInfo(Application.StartupPath + "\\" + fileName);
                int satir = 1, sutun = 1;
                using (var package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sayfa1");
                    worksheet.Cells[1, 1].Value = "Ürün Adı";
                    worksheet.Cells[1, 2].Value = "Fiyatı";
                    worksheet.Cells[1, 3].Value = "Resmi";
                    worksheet.Cells[1, 4].Value = "Açıklaması";

                    for (int i = 1; i <= 15; i++)
                    {
                        webBrowser2.Navigate("http://www.jasmin2020.com/page.php?page=" + i.ToString() + "&act=kategoriGoster&catID=51&markaID=spkomut_HEPSI&name=duvar-resimleri&orderBy=&op=");
                        while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                            Application.DoEvents();
                        //W8();
                        string[] a = webBrowser2.DocumentText.Split(new string[] { "<div class=\"product-gallery\">" }, StringSplitOptions.None);
                        string[] a1 = a[1].Split(new string[] { "<div class=\"pagination\">" }, StringSplitOptions.None);
                        string[] a2 = a1[0].Split(new string[] { "<a href=\"" }, StringSplitOptions.None);
                        for (int j = 1; j < a2.Length; j++)
                        {
                            satir++; sutun = 1;
                            string[] a3 = a2[j].Split('"');
                            webBrowser2.Navigate("http://www.jasmin2020.com/" + a3[0]);
                            while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                                Application.DoEvents();
                            //W8();

                            StreamReader sr = new StreamReader(this.webBrowser2.DocumentStream, Encoding.GetEncoding("iso-8859-9"));
                            string source = sr.ReadToEnd();


                            string[] a4 = source.Split(new string[] { "<meta property=\"og:image\" content=\"" }, StringSplitOptions.None);
                            string[] a5 = a4[1].Split('"');

                            string[] a6 = source.Split(new string[] { "<h1>" }, StringSplitOptions.None);
                            string[] a7 = a6[1].Split(new string[] { "</h1>" }, StringSplitOptions.None);

                            string[] a13 = source.Split(new string[] { "<dt>Satış fiyatı :</dt>" }, StringSplitOptions.None);
                            string[] a14 = a13[1].Split(new string[] { "<dd>" }, StringSplitOptions.None);
                            string[] a15 = a14[1].Split(new string[] { "</dd>" }, StringSplitOptions.None);

                            string[] a16 = source.Split(new string[] { "<div id=\"dstoretab1\" class=\"dstoretab\">" }, StringSplitOptions.None);
                            string[] a17 = a16[1].Split(new string[] { "<div id=\"dstoretab2\" class=\"dstoretab\">" }, StringSplitOptions.None);

                            worksheet.Cells[satir, sutun].Value = a7[0];
                            sutun++;
                            worksheet.Cells[satir, sutun].Value = a15[0];
                            sutun++;
                            worksheet.Cells[satir, sutun].Value = a5[0];
                            sutun++;
                            worksheet.Cells[satir, sutun].Value = a17[0];
                            sutun++;
                        }
                    }
                    package.Save();
                }
                MessageBox.Show("Tamamlandı.");
            }
            else if (comboBox1.Text == "--3 Parçalı Tablolar")
            {
                var fileName = "jasmin2020_3_parcali_tablolar-" + DateTime.Now.ToString("HH-mm-ss__dd-MM-yyyy") + ".xlsx";
                var file = new FileInfo(Application.StartupPath + "\\" + fileName);
                int satir = 1, sutun = 1;
                using (var package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sayfa1");
                    worksheet.Cells[1, 1].Value = "Ürün Adı";
                    worksheet.Cells[1, 2].Value = "Fiyatı";
                    worksheet.Cells[1, 3].Value = "Resmi";
                    worksheet.Cells[1, 4].Value = "Açıklaması";

                    for (int i = 1; i <= 8; i++)
                    {
                        webBrowser2.Navigate("http://www.jasmin2020.com/page.php?page=" + i.ToString() + "&act=kategoriGoster&catID=52&markaID=spkomut_HEPSI&name=3-parcali-tablolar&orderBy=&op=");
                        while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                            Application.DoEvents();
                        //W8();
                        string[] a = webBrowser2.DocumentText.Split(new string[] { "<div class=\"product-gallery\">" }, StringSplitOptions.None);
                        string[] a1 = a[1].Split(new string[] { "<div class=\"pagination\">" }, StringSplitOptions.None);
                        string[] a2 = a1[0].Split(new string[] { "<a href=\"" }, StringSplitOptions.None);
                        for (int j = 1; j < a2.Length; j++)
                        {
                            satir++; sutun = 1;
                            string[] a3 = a2[j].Split('"');
                            webBrowser2.Navigate("http://www.jasmin2020.com/" + a3[0]);
                            while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                                Application.DoEvents();
                            //W8();

                            StreamReader sr = new StreamReader(this.webBrowser2.DocumentStream, Encoding.GetEncoding("iso-8859-9"));
                            string source = sr.ReadToEnd();


                            string[] a4 = source.Split(new string[] { "<meta property=\"og:image\" content=\"" }, StringSplitOptions.None);
                            string[] a5 = a4[1].Split('"');

                            string[] a6 = source.Split(new string[] { "<h1>" }, StringSplitOptions.None);
                            string[] a7 = a6[1].Split(new string[] { "</h1>" }, StringSplitOptions.None);

                            string[] a13 = source.Split(new string[] { "<dt>Satış fiyatı :</dt>" }, StringSplitOptions.None);
                            string[] a14 = a13[1].Split(new string[] { "<dd>" }, StringSplitOptions.None);
                            string[] a15 = a14[1].Split(new string[] { "</dd>" }, StringSplitOptions.None);

                            string[] a16 = source.Split(new string[] { "<div id=\"dstoretab1\" class=\"dstoretab\">" }, StringSplitOptions.None);
                            string[] a17 = a16[1].Split(new string[] { "<div id=\"dstoretab2\" class=\"dstoretab\">" }, StringSplitOptions.None);

                            worksheet.Cells[satir, sutun].Value = a7[0];
                            sutun++;
                            worksheet.Cells[satir, sutun].Value = a15[0];
                            sutun++;
                            worksheet.Cells[satir, sutun].Value = a5[0];
                            sutun++;
                            worksheet.Cells[satir, sutun].Value = a17[0];
                            sutun++;
                        }
                    }
                    package.Save();
                }
                MessageBox.Show("Tamamlandı.");
            }
            else if (comboBox1.Text == "--4 Parçalı Tablolar")
            {
                var fileName = "jasmin2020_4_parcali_tablolar-" + DateTime.Now.ToString("HH-mm-ss__dd-MM-yyyy") + ".xlsx";
                var file = new FileInfo(Application.StartupPath + "\\" + fileName);
                int satir = 1, sutun = 1;
                using (var package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sayfa1");
                    worksheet.Cells[1, 1].Value = "Ürün Adı";
                    worksheet.Cells[1, 2].Value = "Fiyatı";
                    worksheet.Cells[1, 3].Value = "Resmi";
                    worksheet.Cells[1, 4].Value = "Açıklaması";

                    webBrowser2.Navigate("http://www.jasmin2020.com/4-parcali-tablolar-kat65.html");
                        while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                            Application.DoEvents();
                        //W8();
                        string[] a = webBrowser2.DocumentText.Split(new string[] { "<div class=\"product-gallery\">" }, StringSplitOptions.None);
                        string[] a1 = a[1].Split(new string[] { "<div class=\"pagination\">" }, StringSplitOptions.None);
                        string[] a2 = a1[0].Split(new string[] { "<a href=\"" }, StringSplitOptions.None);
                        for (int j = 1; j < a2.Length; j++)
                        {
                            satir++; sutun = 1;
                            string[] a3 = a2[j].Split('"');
                            webBrowser2.Navigate("http://www.jasmin2020.com/" + a3[0]);
                            while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                                Application.DoEvents();
                            //W8();

                            StreamReader sr = new StreamReader(this.webBrowser2.DocumentStream, Encoding.GetEncoding("iso-8859-9"));
                            string source = sr.ReadToEnd();


                            string[] a4 = source.Split(new string[] { "<meta property=\"og:image\" content=\"" }, StringSplitOptions.None);
                            string[] a5 = a4[1].Split('"');

                            string[] a6 = source.Split(new string[] { "<h1>" }, StringSplitOptions.None);
                            string[] a7 = a6[1].Split(new string[] { "</h1>" }, StringSplitOptions.None);

                            string[] a13 = source.Split(new string[] { "<dt>Satış fiyatı :</dt>" }, StringSplitOptions.None);
                            string[] a14 = a13[1].Split(new string[] { "<dd>" }, StringSplitOptions.None);
                            string[] a15 = a14[1].Split(new string[] { "</dd>" }, StringSplitOptions.None);

                            string[] a16 = source.Split(new string[] { "<div id=\"dstoretab1\" class=\"dstoretab\">" }, StringSplitOptions.None);
                            string[] a17 = a16[1].Split(new string[] { "<div id=\"dstoretab2\" class=\"dstoretab\">" }, StringSplitOptions.None);

                            worksheet.Cells[satir, sutun].Value = a7[0];
                            sutun++;
                            worksheet.Cells[satir, sutun].Value = a15[0];
                            sutun++;
                            worksheet.Cells[satir, sutun].Value = a5[0];
                            sutun++;
                            worksheet.Cells[satir, sutun].Value = a17[0];
                            sutun++;
                        }
                    package.Save();
                }
                MessageBox.Show("Tamamlandı.");
            }
            else if (comboBox1.Text == "--5 Parçalı Tablolar")
            {
                var fileName = "jasmin2020_5_parcali_tablolar-" + DateTime.Now.ToString("HH-mm-ss__dd-MM-yyyy") + ".xlsx";
                var file = new FileInfo(Application.StartupPath + "\\" + fileName);
                int satir = 1, sutun = 1;
                using (var package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sayfa1");
                    worksheet.Cells[1, 1].Value = "Ürün Adı";
                    worksheet.Cells[1, 2].Value = "Fiyatı";
                    worksheet.Cells[1, 3].Value = "Resmi";
                    worksheet.Cells[1, 4].Value = "Açıklaması";

                    for (int i = 1; i <= 2; i++)
                    {
                        webBrowser2.Navigate("http://www.jasmin2020.com/page.php?page=" + i.ToString() + "&act=kategoriGoster&catID=53&markaID=spkomut_HEPSI&name=5-parcali-tablolar&orderBy=&op=");
                        while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                            Application.DoEvents();
                        //W8();
                        string[] a = webBrowser2.DocumentText.Split(new string[] { "<div class=\"product-gallery\">" }, StringSplitOptions.None);
                        string[] a1 = a[1].Split(new string[] { "<div class=\"pagination\">" }, StringSplitOptions.None);
                        string[] a2 = a1[0].Split(new string[] { "<a href=\"" }, StringSplitOptions.None);
                        for (int j = 1; j < a2.Length; j++)
                        {
                            satir++; sutun = 1;
                            string[] a3 = a2[j].Split('"');
                            webBrowser2.Navigate("http://www.jasmin2020.com/" + a3[0]);
                            while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                                Application.DoEvents();
                            //W8();

                            StreamReader sr = new StreamReader(this.webBrowser2.DocumentStream, Encoding.GetEncoding("iso-8859-9"));
                            string source = sr.ReadToEnd();


                            string[] a4 = source.Split(new string[] { "<meta property=\"og:image\" content=\"" }, StringSplitOptions.None);
                            string[] a5 = a4[1].Split('"');

                            string[] a6 = source.Split(new string[] { "<h1>" }, StringSplitOptions.None);
                            string[] a7 = a6[1].Split(new string[] { "</h1>" }, StringSplitOptions.None);

                            string[] a13 = source.Split(new string[] { "<dt>Satış fiyatı :</dt>" }, StringSplitOptions.None);
                            string[] a14 = a13[1].Split(new string[] { "<dd>" }, StringSplitOptions.None);
                            string[] a15 = a14[1].Split(new string[] { "</dd>" }, StringSplitOptions.None);

                            string[] a16 = source.Split(new string[] { "<div id=\"dstoretab1\" class=\"dstoretab\">" }, StringSplitOptions.None);
                            string[] a17 = a16[1].Split(new string[] { "<div id=\"dstoretab2\" class=\"dstoretab\">" }, StringSplitOptions.None);

                            worksheet.Cells[satir, sutun].Value = a7[0];
                            sutun++;
                            worksheet.Cells[satir, sutun].Value = a15[0];
                            sutun++;
                            worksheet.Cells[satir, sutun].Value = a5[0];
                            sutun++;
                            worksheet.Cells[satir, sutun].Value = a17[0];
                            sutun++;
                        }
                    }
                    package.Save();
                }
                MessageBox.Show("Tamamlandı.");
            }
            else if (comboBox1.Text == "--Tek Parçalı Tablolar")
            {
                var fileName = "jasmin2020_tek_parcali_tablolar-" + DateTime.Now.ToString("HH-mm-ss__dd-MM-yyyy") + ".xlsx";
                var file = new FileInfo(Application.StartupPath + "\\" + fileName);
                int satir = 1, sutun = 1;
                using (var package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sayfa1");
                    worksheet.Cells[1, 1].Value = "Ürün Adı";
                    worksheet.Cells[1, 2].Value = "Fiyatı";
                    worksheet.Cells[1, 3].Value = "Resmi";
                    worksheet.Cells[1, 4].Value = "Açıklaması";

                    for (int i = 1; i <= 4; i++)
                    {
                        webBrowser2.Navigate("http://www.jasmin2020.com/page.php?page=" + i.ToString() + "&act=kategoriGoster&catID=67&markaID=spkomut_HEPSI&name=tek-parcali-tablolar&orderBy=&op=");
                        while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                            Application.DoEvents();
                        //W8();
                        string[] a = webBrowser2.DocumentText.Split(new string[] { "<div class=\"product-gallery\">" }, StringSplitOptions.None);
                        string[] a1 = a[1].Split(new string[] { "<div class=\"pagination\">" }, StringSplitOptions.None);
                        string[] a2 = a1[0].Split(new string[] { "<a href=\"" }, StringSplitOptions.None);
                        for (int j = 1; j < a2.Length; j++)
                        {
                            satir++; sutun = 1;
                            string[] a3 = a2[j].Split('"');
                            webBrowser2.Navigate("http://www.jasmin2020.com/" + a3[0]);
                            while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                                Application.DoEvents();
                            //W8();

                            StreamReader sr = new StreamReader(this.webBrowser2.DocumentStream, Encoding.GetEncoding("iso-8859-9"));
                            string source = sr.ReadToEnd();


                            string[] a4 = source.Split(new string[] { "<meta property=\"og:image\" content=\"" }, StringSplitOptions.None);
                            string[] a5 = a4[1].Split('"');

                            string[] a6 = source.Split(new string[] { "<h1>" }, StringSplitOptions.None);
                            string[] a7 = a6[1].Split(new string[] { "</h1>" }, StringSplitOptions.None);

                            string[] a13 = source.Split(new string[] { "<dt>Satış fiyatı :</dt>" }, StringSplitOptions.None);
                            string[] a14 = a13[1].Split(new string[] { "<dd>" }, StringSplitOptions.None);
                            string[] a15 = a14[1].Split(new string[] { "</dd>" }, StringSplitOptions.None);

                            string[] a16 = source.Split(new string[] { "<div id=\"dstoretab1\" class=\"dstoretab\">" }, StringSplitOptions.None);
                            string[] a17 = a16[1].Split(new string[] { "<div id=\"dstoretab2\" class=\"dstoretab\">" }, StringSplitOptions.None);

                            worksheet.Cells[satir, sutun].Value = a7[0];
                            sutun++;
                            worksheet.Cells[satir, sutun].Value = a15[0];
                            sutun++;
                            worksheet.Cells[satir, sutun].Value = a5[0];
                            sutun++;
                            worksheet.Cells[satir, sutun].Value = a17[0];
                            sutun++;
                        }
                    }
                    package.Save();
                }
                MessageBox.Show("Tamamlandı.");
            }
            else if (comboBox1.Text == "--Mini Poster")
            {
                var fileName = "jasmin2020_mini_poster-" + DateTime.Now.ToString("HH-mm-ss__dd-MM-yyyy") + ".xlsx";
                var file = new FileInfo(Application.StartupPath + "\\" + fileName);
                int satir = 1, sutun = 1;
                using (var package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sayfa1");
                    worksheet.Cells[1, 1].Value = "Ürün Adı";
                    worksheet.Cells[1, 2].Value = "Fiyatı";
                    worksheet.Cells[1, 3].Value = "Resmi";
                    worksheet.Cells[1, 4].Value = "Açıklaması";

                    for (int i = 1; i <= 2; i++)
                    {
                        webBrowser2.Navigate("http://www.jasmin2020.com/page.php?page=" + i.ToString() + "&act=kategoriGoster&catID=87&markaID=spkomut_HEPSI&name=mini-poster&orderBy=&op=");
                        while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                            Application.DoEvents();
                        //W8();
                        string[] a = webBrowser2.DocumentText.Split(new string[] { "<div class=\"product-gallery\">" }, StringSplitOptions.None);
                        string[] a1 = a[1].Split(new string[] { "<div class=\"pagination\">" }, StringSplitOptions.None);
                        string[] a2 = a1[0].Split(new string[] { "<a href=\"" }, StringSplitOptions.None);
                        for (int j = 1; j < a2.Length; j++)
                        {
                            satir++; sutun = 1;
                            string[] a3 = a2[j].Split('"');
                            webBrowser2.Navigate("http://www.jasmin2020.com/" + a3[0]);
                            while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                                Application.DoEvents();
                            //W8();

                            StreamReader sr = new StreamReader(this.webBrowser2.DocumentStream, Encoding.GetEncoding("iso-8859-9"));
                            string source = sr.ReadToEnd();


                            string[] a4 = source.Split(new string[] { "<meta property=\"og:image\" content=\"" }, StringSplitOptions.None);
                            string[] a5 = a4[1].Split('"');

                            string[] a6 = source.Split(new string[] { "<h1>" }, StringSplitOptions.None);
                            string[] a7 = a6[1].Split(new string[] { "</h1>" }, StringSplitOptions.None);

                            string[] a13 = source.Split(new string[] { "<dt>Satış fiyatı :</dt>" }, StringSplitOptions.None);
                            string[] a14 = a13[1].Split(new string[] { "<dd>" }, StringSplitOptions.None);
                            string[] a15 = a14[1].Split(new string[] { "</dd>" }, StringSplitOptions.None);

                            string[] a16 = source.Split(new string[] { "<div id=\"dstoretab1\" class=\"dstoretab\">" }, StringSplitOptions.None);
                            string[] a17 = a16[1].Split(new string[] { "<div id=\"dstoretab2\" class=\"dstoretab\">" }, StringSplitOptions.None);

                            worksheet.Cells[satir, sutun].Value = a7[0];
                            sutun++;
                            worksheet.Cells[satir, sutun].Value = a15[0];
                            sutun++;
                            worksheet.Cells[satir, sutun].Value = a5[0];
                            sutun++;
                            worksheet.Cells[satir, sutun].Value = a17[0];
                            sutun++;
                        }
                    }
                    package.Save();
                }
                MessageBox.Show("Tamamlandı.");
            }
            else if (comboBox1.Text == "--Mobilya Sticker")
            {
                var fileName = "jasmin2020_mobilya_sticker-" + DateTime.Now.ToString("HH-mm-ss__dd-MM-yyyy") + ".xlsx";
                var file = new FileInfo(Application.StartupPath + "\\" + fileName);
                int satir = 1, sutun = 1;
                using (var package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sayfa1");
                    worksheet.Cells[1, 1].Value = "Ürün Adı";
                    worksheet.Cells[1, 2].Value = "Fiyatı";
                    worksheet.Cells[1, 3].Value = "Resmi";
                    worksheet.Cells[1, 4].Value = "Açıklaması";
                    webBrowser2.Navigate("http://www.jasmin2020.com/mobilya-sticker-kat84.html");
                    while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                        Application.DoEvents();
                    //W8();
                    string[] a = webBrowser2.DocumentText.Split(new string[] { "<div class=\"product-gallery\">" }, StringSplitOptions.None);
                    string[] a1 = a[1].Split(new string[] { "<div class=\"pagination\">" }, StringSplitOptions.None);
                    string[] a2 = a1[0].Split(new string[] { "<a href=\"" }, StringSplitOptions.None);
                    for (int j = 1; j < a2.Length; j++)
                    {
                        satir++; sutun = 1;
                        string[] a3 = a2[j].Split('"');
                        webBrowser2.Navigate("http://www.jasmin2020.com/" + a3[0]);
                        while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                            Application.DoEvents();
                        //W8();

                        StreamReader sr = new StreamReader(this.webBrowser2.DocumentStream, Encoding.GetEncoding("iso-8859-9"));
                        string source = sr.ReadToEnd();


                        string[] a4 = source.Split(new string[] { "<meta property=\"og:image\" content=\"" }, StringSplitOptions.None);
                        string[] a5 = a4[1].Split('"');

                        string[] a6 = source.Split(new string[] { "<h1>" }, StringSplitOptions.None);
                        string[] a7 = a6[1].Split(new string[] { "</h1>" }, StringSplitOptions.None);

                        string[] a13 = source.Split(new string[] { "<dt>Satış fiyatı :</dt>" }, StringSplitOptions.None);
                        string[] a14 = a13[1].Split(new string[] { "<dd>" }, StringSplitOptions.None);
                        string[] a15 = a14[1].Split(new string[] { "</dd>" }, StringSplitOptions.None);

                        string[] a16 = source.Split(new string[] { "<div id=\"dstoretab1\" class=\"dstoretab\">" }, StringSplitOptions.None);
                        string[] a17 = a16[1].Split(new string[] { "<div id=\"dstoretab2\" class=\"dstoretab\">" }, StringSplitOptions.None);

                        worksheet.Cells[satir, sutun].Value = a7[0];
                        sutun++;
                        worksheet.Cells[satir, sutun].Value = a15[0];
                        sutun++;
                        worksheet.Cells[satir, sutun].Value = a5[0];
                        sutun++;
                        worksheet.Cells[satir, sutun].Value = a17[0];
                        sutun++;
                    }
                    package.Save();
                }
                MessageBox.Show("Tamamlandı.");
            }
            else if (comboBox1.Text == "----iphone 5 sticker")
            {
                var fileName = "jasmin2020_iphone5_sticker-" + DateTime.Now.ToString("HH-mm-ss__dd-MM-yyyy") + ".xlsx";
                var file = new FileInfo(Application.StartupPath + "\\" + fileName);
                int satir = 1, sutun = 1;
                using (var package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sayfa1");
                    worksheet.Cells[1, 1].Value = "Ürün Adı";
                    worksheet.Cells[1, 2].Value = "Fiyatı";
                    worksheet.Cells[1, 3].Value = "Resmi";
                    worksheet.Cells[1, 4].Value = "Açıklaması";
                    webBrowser2.Navigate("http://www.jasmin2020.com/iphone-5-sticker-kat81.html");
                    while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                        Application.DoEvents();
                    //W8();
                    string[] a = webBrowser2.DocumentText.Split(new string[] { "<div class=\"product-gallery\">" }, StringSplitOptions.None);
                    string[] a1 = a[1].Split(new string[] { "<div class=\"pagination\">" }, StringSplitOptions.None);
                    string[] a2 = a1[0].Split(new string[] { "<a href=\"" }, StringSplitOptions.None);
                    for (int j = 1; j < a2.Length; j++)
                    {
                        satir++; sutun = 1;
                        string[] a3 = a2[j].Split('"');
                        webBrowser2.Navigate("http://www.jasmin2020.com/" + a3[0]);
                        while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                            Application.DoEvents();
                        //W8();

                        StreamReader sr = new StreamReader(this.webBrowser2.DocumentStream, Encoding.GetEncoding("iso-8859-9"));
                        string source = sr.ReadToEnd();


                        string[] a4 = source.Split(new string[] { "<meta property=\"og:image\" content=\"" }, StringSplitOptions.None);
                        string[] a5 = a4[1].Split('"');

                        string[] a6 = source.Split(new string[] { "<h1>" }, StringSplitOptions.None);
                        string[] a7 = a6[1].Split(new string[] { "</h1>" }, StringSplitOptions.None);

                        string[] a13 = source.Split(new string[] { "<dt>Satış fiyatı :</dt>" }, StringSplitOptions.None);
                        string[] a14 = a13[1].Split(new string[] { "<dd>" }, StringSplitOptions.None);
                        string[] a15 = a14[1].Split(new string[] { "</dd>" }, StringSplitOptions.None);

                        string[] a16 = source.Split(new string[] { "<div id=\"dstoretab1\" class=\"dstoretab\">" }, StringSplitOptions.None);
                        string[] a17 = a16[1].Split(new string[] { "<div id=\"dstoretab2\" class=\"dstoretab\">" }, StringSplitOptions.None);

                        worksheet.Cells[satir, sutun].Value = a7[0];
                        sutun++;
                        worksheet.Cells[satir, sutun].Value = a15[0];
                        sutun++;
                        worksheet.Cells[satir, sutun].Value = a5[0];
                        sutun++;
                        worksheet.Cells[satir, sutun].Value = a17[0];
                        sutun++;
                    }
                    package.Save();
                }
                MessageBox.Show("Tamamlandı.");
            }
            else if (comboBox1.Text == "----iphone 4 sticker")
            {
                var fileName = "jasmin2020_iphone4_sticker-" + DateTime.Now.ToString("HH-mm-ss__dd-MM-yyyy") + ".xlsx";
                var file = new FileInfo(Application.StartupPath + "\\" + fileName);
                int satir = 1, sutun = 1;
                using (var package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sayfa1");
                    worksheet.Cells[1, 1].Value = "Ürün Adı";
                    worksheet.Cells[1, 2].Value = "Fiyatı";
                    worksheet.Cells[1, 3].Value = "Resmi";
                    worksheet.Cells[1, 4].Value = "Açıklaması";
                    webBrowser2.Navigate("http://www.jasmin2020.com/iphone-4-sticker-kat80.html");
                    while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                        Application.DoEvents();
                    //W8();
                    string[] a = webBrowser2.DocumentText.Split(new string[] { "<div class=\"product-gallery\">" }, StringSplitOptions.None);
                    string[] a1 = a[1].Split(new string[] { "<div class=\"pagination\">" }, StringSplitOptions.None);
                    string[] a2 = a1[0].Split(new string[] { "<a href=\"" }, StringSplitOptions.None);
                    for (int j = 1; j < a2.Length; j++)
                    {
                        satir++; sutun = 1;
                        string[] a3 = a2[j].Split('"');
                        webBrowser2.Navigate("http://www.jasmin2020.com/" + a3[0]);
                        while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                            Application.DoEvents();
                        //W8();

                        StreamReader sr = new StreamReader(this.webBrowser2.DocumentStream, Encoding.GetEncoding("iso-8859-9"));
                        string source = sr.ReadToEnd();


                        string[] a4 = source.Split(new string[] { "<meta property=\"og:image\" content=\"" }, StringSplitOptions.None);
                        string[] a5 = a4[1].Split('"');

                        string[] a6 = source.Split(new string[] { "<h1>" }, StringSplitOptions.None);
                        string[] a7 = a6[1].Split(new string[] { "</h1>" }, StringSplitOptions.None);

                        string[] a13 = source.Split(new string[] { "<dt>Satış fiyatı :</dt>" }, StringSplitOptions.None);
                        string[] a14 = a13[1].Split(new string[] { "<dd>" }, StringSplitOptions.None);
                        string[] a15 = a14[1].Split(new string[] { "</dd>" }, StringSplitOptions.None);

                        string[] a16 = source.Split(new string[] { "<div id=\"dstoretab1\" class=\"dstoretab\">" }, StringSplitOptions.None);
                        string[] a17 = a16[1].Split(new string[] { "<div id=\"dstoretab2\" class=\"dstoretab\">" }, StringSplitOptions.None);

                        worksheet.Cells[satir, sutun].Value = a7[0];
                        sutun++;
                        worksheet.Cells[satir, sutun].Value = a15[0];
                        sutun++;
                        worksheet.Cells[satir, sutun].Value = a5[0];
                        sutun++;
                        worksheet.Cells[satir, sutun].Value = a17[0];
                        sutun++;
                    }
                    package.Save();
                }
                MessageBox.Show("Tamamlandı.");
            }
            else if (comboBox1.Text == "----Samsung Note")
            {
                var fileName = "jasmin2020_Samsung_Note-" + DateTime.Now.ToString("HH-mm-ss__dd-MM-yyyy") + ".xlsx";
                var file = new FileInfo(Application.StartupPath + "\\" + fileName);
                int satir = 1, sutun = 1;
                using (var package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sayfa1");
                    worksheet.Cells[1, 1].Value = "Ürün Adı";
                    worksheet.Cells[1, 2].Value = "Fiyatı";
                    worksheet.Cells[1, 3].Value = "Resmi";
                    worksheet.Cells[1, 4].Value = "Açıklaması";
                    webBrowser2.Navigate("http://www.jasmin2020.com/samsung-note-kat82.html");
                    while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                        Application.DoEvents();
                    //W8();
                    string[] a = webBrowser2.DocumentText.Split(new string[] { "<div class=\"product-gallery\">" }, StringSplitOptions.None);
                    string[] a1 = a[1].Split(new string[] { "<div class=\"pagination\">" }, StringSplitOptions.None);
                    string[] a2 = a1[0].Split(new string[] { "<a href=\"" }, StringSplitOptions.None);
                    for (int j = 1; j < a2.Length; j++)
                    {
                        satir++; sutun = 1;
                        string[] a3 = a2[j].Split('"');
                        webBrowser2.Navigate("http://www.jasmin2020.com/" + a3[0]);
                        while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                            Application.DoEvents();
                        //W8();

                        StreamReader sr = new StreamReader(this.webBrowser2.DocumentStream, Encoding.GetEncoding("iso-8859-9"));
                        string source = sr.ReadToEnd();


                        string[] a4 = source.Split(new string[] { "<meta property=\"og:image\" content=\"" }, StringSplitOptions.None);
                        string[] a5 = a4[1].Split('"');

                        string[] a6 = source.Split(new string[] { "<h1>" }, StringSplitOptions.None);
                        string[] a7 = a6[1].Split(new string[] { "</h1>" }, StringSplitOptions.None);

                        string[] a13 = source.Split(new string[] { "<dt>Satış fiyatı :</dt>" }, StringSplitOptions.None);
                        string[] a14 = a13[1].Split(new string[] { "<dd>" }, StringSplitOptions.None);
                        string[] a15 = a14[1].Split(new string[] { "</dd>" }, StringSplitOptions.None);

                        string[] a16 = source.Split(new string[] { "<div id=\"dstoretab1\" class=\"dstoretab\">" }, StringSplitOptions.None);
                        string[] a17 = a16[1].Split(new string[] { "<div id=\"dstoretab2\" class=\"dstoretab\">" }, StringSplitOptions.None);

                        worksheet.Cells[satir, sutun].Value = a7[0];
                        sutun++;
                        worksheet.Cells[satir, sutun].Value = a15[0];
                        sutun++;
                        worksheet.Cells[satir, sutun].Value = a5[0];
                        sutun++;
                        worksheet.Cells[satir, sutun].Value = a17[0];
                        sutun++;
                    }
                    package.Save();
                }
                MessageBox.Show("Tamamlandı.");
            }
            else if (comboBox1.Text == "----Bulaşık Makinesi Sticker")
            {
                var fileName = "jasmin2020_bulasik_makinesi_sticker-" + DateTime.Now.ToString("HH-mm-ss__dd-MM-yyyy") + ".xlsx";
                var file = new FileInfo(Application.StartupPath + "\\" + fileName);
                int satir = 1, sutun = 1;
                using (var package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sayfa1");
                    worksheet.Cells[1, 1].Value = "Ürün Adı";
                    worksheet.Cells[1, 2].Value = "Fiyatı";
                    worksheet.Cells[1, 3].Value = "Resmi";
                    worksheet.Cells[1, 4].Value = "Açıklaması";
                    webBrowser2.Navigate("http://www.jasmin2020.com/bulasik-makinesi-sticker-kat69.html");
                    while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                        Application.DoEvents();
                    //W8();
                    string[] a = webBrowser2.DocumentText.Split(new string[] { "<div class=\"product-gallery\">" }, StringSplitOptions.None);
                    string[] a1 = a[1].Split(new string[] { "<div class=\"pagination\">" }, StringSplitOptions.None);
                    string[] a2 = a1[0].Split(new string[] { "<a href=\"" }, StringSplitOptions.None);
                    for (int j = 1; j < a2.Length; j++)
                    {
                        satir++; sutun = 1;
                        string[] a3 = a2[j].Split('"');
                        webBrowser2.Navigate("http://www.jasmin2020.com/" + a3[0]);
                        while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                            Application.DoEvents();
                        //W8();

                        StreamReader sr = new StreamReader(this.webBrowser2.DocumentStream, Encoding.GetEncoding("iso-8859-9"));
                        string source = sr.ReadToEnd();


                        string[] a4 = source.Split(new string[] { "<meta property=\"og:image\" content=\"" }, StringSplitOptions.None);
                        string[] a5 = a4[1].Split('"');

                        string[] a6 = source.Split(new string[] { "<h1>" }, StringSplitOptions.None);
                        string[] a7 = a6[1].Split(new string[] { "</h1>" }, StringSplitOptions.None);

                        string[] a13 = source.Split(new string[] { "<dt>Satış fiyatı :</dt>" }, StringSplitOptions.None);
                        string[] a14 = a13[1].Split(new string[] { "<dd>" }, StringSplitOptions.None);
                        string[] a15 = a14[1].Split(new string[] { "</dd>" }, StringSplitOptions.None);

                        string[] a16 = source.Split(new string[] { "<div id=\"dstoretab1\" class=\"dstoretab\">" }, StringSplitOptions.None);
                        string[] a17 = a16[1].Split(new string[] { "<div id=\"dstoretab2\" class=\"dstoretab\">" }, StringSplitOptions.None);

                        worksheet.Cells[satir, sutun].Value = a7[0];
                        sutun++;
                        worksheet.Cells[satir, sutun].Value = a15[0];
                        sutun++;
                        worksheet.Cells[satir, sutun].Value = a5[0];
                        sutun++;
                        worksheet.Cells[satir, sutun].Value = a17[0];
                        sutun++;
                    }
                    package.Save();
                }
                MessageBox.Show("Tamamlandı.");
            }
            else if (comboBox1.Text == "--Çocuk Odası Duvar Sticker")
            {
                var fileName = "jasmin2020_cocuk_odasi_duvar_sticker-" + DateTime.Now.ToString("HH-mm-ss__dd-MM-yyyy") + ".xlsx";
                var file = new FileInfo(Application.StartupPath + "\\" + fileName);
                int satir = 1, sutun = 1;
                using (var package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sayfa1");
                    worksheet.Cells[1, 1].Value = "Ürün Adı";
                    worksheet.Cells[1, 2].Value = "Fiyatı";
                    worksheet.Cells[1, 3].Value = "Resmi";
                    worksheet.Cells[1, 4].Value = "Açıklaması";
                    webBrowser2.Navigate("http://www.jasmin2020.com/cocuk-odasi-duvar-sticker-kat85.html");
                    while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                        Application.DoEvents();
                    //W8();
                    string[] a = webBrowser2.DocumentText.Split(new string[] { "<div class=\"product-gallery\">" }, StringSplitOptions.None);
                    string[] a1 = a[1].Split(new string[] { "<div class=\"pagination\">" }, StringSplitOptions.None);
                    string[] a2 = a1[0].Split(new string[] { "<a href=\"" }, StringSplitOptions.None);
                    for (int j = 1; j < a2.Length; j++)
                    {
                        satir++; sutun = 1;
                        string[] a3 = a2[j].Split('"');
                        webBrowser2.Navigate("http://www.jasmin2020.com/" + a3[0]);
                        while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                            Application.DoEvents();
                        //W8();

                        StreamReader sr = new StreamReader(this.webBrowser2.DocumentStream, Encoding.GetEncoding("iso-8859-9"));
                        string source = sr.ReadToEnd();


                        string[] a4 = source.Split(new string[] { "<meta property=\"og:image\" content=\"" }, StringSplitOptions.None);
                        string[] a5 = a4[1].Split('"');

                        string[] a6 = source.Split(new string[] { "<h1>" }, StringSplitOptions.None);
                        string[] a7 = a6[1].Split(new string[] { "</h1>" }, StringSplitOptions.None);

                        string[] a13 = source.Split(new string[] { "<dt>Satış fiyatı :</dt>" }, StringSplitOptions.None);
                        string[] a14 = a13[1].Split(new string[] { "<dd>" }, StringSplitOptions.None);
                        string[] a15 = a14[1].Split(new string[] { "</dd>" }, StringSplitOptions.None);

                        string[] a16 = source.Split(new string[] { "<div id=\"dstoretab1\" class=\"dstoretab\">" }, StringSplitOptions.None);
                        string[] a17 = a16[1].Split(new string[] { "<div id=\"dstoretab2\" class=\"dstoretab\">" }, StringSplitOptions.None);

                        worksheet.Cells[satir, sutun].Value = a7[0];
                        sutun++;
                        worksheet.Cells[satir, sutun].Value = a15[0];
                        sutun++;
                        worksheet.Cells[satir, sutun].Value = a5[0];
                        sutun++;
                        worksheet.Cells[satir, sutun].Value = a17[0];
                        sutun++;
                    }
                    package.Save();
                }
                MessageBox.Show("Tamamlandı.");
            }
            else if (comboBox1.Text == "--Fantastik Laptop Sticker")
            {
                var fileName = "jasmin2020_fantastik_laptop_sticker-" + DateTime.Now.ToString("HH-mm-ss__dd-MM-yyyy") + ".xlsx";
                var file = new FileInfo(Application.StartupPath + "\\" + fileName);
                int satir = 1, sutun = 1;
                using (var package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sayfa1");
                    worksheet.Cells[1, 1].Value = "Ürün Adı";
                    worksheet.Cells[1, 2].Value = "Fiyatı";
                    worksheet.Cells[1, 3].Value = "Resmi";
                    worksheet.Cells[1, 4].Value = "Açıklaması";
                    webBrowser2.Navigate("http://www.jasmin2020.com/fantastik-laptop-sticker-kat56.html");
                    while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                        Application.DoEvents();
                    //W8();
                    string[] a = webBrowser2.DocumentText.Split(new string[] { "<div class=\"product-gallery\">" }, StringSplitOptions.None);
                    string[] a1 = a[1].Split(new string[] { "<div class=\"pagination\">" }, StringSplitOptions.None);
                    string[] a2 = a1[0].Split(new string[] { "<a href=\"" }, StringSplitOptions.None);
                    for (int j = 1; j < a2.Length; j++)
                    {
                        satir++; sutun = 1;
                        string[] a3 = a2[j].Split('"');
                        webBrowser2.Navigate("http://www.jasmin2020.com/" + a3[0]);
                        while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                            Application.DoEvents();
                        //W8();

                        StreamReader sr = new StreamReader(this.webBrowser2.DocumentStream, Encoding.GetEncoding("iso-8859-9"));
                        string source = sr.ReadToEnd();


                        string[] a4 = source.Split(new string[] { "<meta property=\"og:image\" content=\"" }, StringSplitOptions.None);
                        string[] a5 = a4[1].Split('"');

                        string[] a6 = source.Split(new string[] { "<h1>" }, StringSplitOptions.None);
                        string[] a7 = a6[1].Split(new string[] { "</h1>" }, StringSplitOptions.None);

                        string[] a13 = source.Split(new string[] { "<dt>Satış fiyatı :</dt>" }, StringSplitOptions.None);
                        string[] a14 = a13[1].Split(new string[] { "<dd>" }, StringSplitOptions.None);
                        string[] a15 = a14[1].Split(new string[] { "</dd>" }, StringSplitOptions.None);

                        string[] a16 = source.Split(new string[] { "<div id=\"dstoretab1\" class=\"dstoretab\">" }, StringSplitOptions.None);
                        string[] a17 = a16[1].Split(new string[] { "<div id=\"dstoretab2\" class=\"dstoretab\">" }, StringSplitOptions.None);

                        worksheet.Cells[satir, sutun].Value = a7[0];
                        sutun++;
                        worksheet.Cells[satir, sutun].Value = a15[0];
                        sutun++;
                        worksheet.Cells[satir, sutun].Value = a5[0];
                        sutun++;
                        worksheet.Cells[satir, sutun].Value = a17[0];
                        sutun++;
                    }
                    package.Save();
                }
                MessageBox.Show("Tamamlandı.");
            }
            else if (comboBox1.Text == "--Konsept Laptop Sticker")
            {
                var fileName = "jasmin2020_konsept_laptop_sticker-" + DateTime.Now.ToString("HH-mm-ss__dd-MM-yyyy") + ".xlsx";
                var file = new FileInfo(Application.StartupPath + "\\" + fileName);
                int satir = 1, sutun = 1;
                using (var package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sayfa1");
                    worksheet.Cells[1, 1].Value = "Ürün Adı";
                    worksheet.Cells[1, 2].Value = "Fiyatı";
                    worksheet.Cells[1, 3].Value = "Resmi";
                    worksheet.Cells[1, 4].Value = "Açıklaması";
                    webBrowser2.Navigate("http://www.jasmin2020.com/konsept-laptop-sticker-kat57.html");
                    while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                        Application.DoEvents();
                    //W8();
                    string[] a = webBrowser2.DocumentText.Split(new string[] { "<div class=\"product-gallery\">" }, StringSplitOptions.None);
                    string[] a1 = a[1].Split(new string[] { "<div class=\"pagination\">" }, StringSplitOptions.None);
                    string[] a2 = a1[0].Split(new string[] { "<a href=\"" }, StringSplitOptions.None);
                    for (int j = 1; j < a2.Length; j++)
                    {
                        satir++; sutun = 1;
                        string[] a3 = a2[j].Split('"');
                        webBrowser2.Navigate("http://www.jasmin2020.com/" + a3[0]);
                        while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                            Application.DoEvents();
                        //W8();

                        StreamReader sr = new StreamReader(this.webBrowser2.DocumentStream, Encoding.GetEncoding("iso-8859-9"));
                        string source = sr.ReadToEnd();


                        string[] a4 = source.Split(new string[] { "<meta property=\"og:image\" content=\"" }, StringSplitOptions.None);
                        string[] a5 = a4[1].Split('"');

                        string[] a6 = source.Split(new string[] { "<h1>" }, StringSplitOptions.None);
                        string[] a7 = a6[1].Split(new string[] { "</h1>" }, StringSplitOptions.None);

                        string[] a13 = source.Split(new string[] { "<dt>Satış fiyatı :</dt>" }, StringSplitOptions.None);
                        string[] a14 = a13[1].Split(new string[] { "<dd>" }, StringSplitOptions.None);
                        string[] a15 = a14[1].Split(new string[] { "</dd>" }, StringSplitOptions.None);

                        string[] a16 = source.Split(new string[] { "<div id=\"dstoretab1\" class=\"dstoretab\">" }, StringSplitOptions.None);
                        string[] a17 = a16[1].Split(new string[] { "<div id=\"dstoretab2\" class=\"dstoretab\">" }, StringSplitOptions.None);

                        worksheet.Cells[satir, sutun].Value = a7[0];
                        sutun++;
                        worksheet.Cells[satir, sutun].Value = a15[0];
                        sutun++;
                        worksheet.Cells[satir, sutun].Value = a5[0];
                        sutun++;
                        worksheet.Cells[satir, sutun].Value = a17[0];
                        sutun++;
                    }
                    package.Save();
                }
                MessageBox.Show("Tamamlandı.");
            }
            else if (comboBox1.Text == "--Resim Leptop Sticker")
            {
                var fileName = "jasmin2020_resim_laptop_sticker-" + DateTime.Now.ToString("HH-mm-ss__dd-MM-yyyy") + ".xlsx";
                var file = new FileInfo(Application.StartupPath + "\\" + fileName);
                int satir = 1, sutun = 1;
                using (var package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sayfa1");
                    worksheet.Cells[1, 1].Value = "Ürün Adı";
                    worksheet.Cells[1, 2].Value = "Fiyatı";
                    worksheet.Cells[1, 3].Value = "Resmi";
                    worksheet.Cells[1, 4].Value = "Açıklaması";
                    webBrowser2.Navigate("http://www.jasmin2020.com/resim-leptop-sticker-kat58.html");
                    while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                        Application.DoEvents();
                    //W8();
                    string[] a = webBrowser2.DocumentText.Split(new string[] { "<div class=\"product-gallery\">" }, StringSplitOptions.None);
                    string[] a1 = a[1].Split(new string[] { "<div class=\"pagination\">" }, StringSplitOptions.None);
                    string[] a2 = a1[0].Split(new string[] { "<a href=\"" }, StringSplitOptions.None);
                    for (int j = 1; j < a2.Length; j++)
                    {
                        satir++; sutun = 1;
                        string[] a3 = a2[j].Split('"');
                        webBrowser2.Navigate("http://www.jasmin2020.com/" + a3[0]);
                        while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                            Application.DoEvents();
                        //W8();

                        StreamReader sr = new StreamReader(this.webBrowser2.DocumentStream, Encoding.GetEncoding("iso-8859-9"));
                        string source = sr.ReadToEnd();


                        string[] a4 = source.Split(new string[] { "<meta property=\"og:image\" content=\"" }, StringSplitOptions.None);
                        string[] a5 = a4[1].Split('"');

                        string[] a6 = source.Split(new string[] { "<h1>" }, StringSplitOptions.None);
                        string[] a7 = a6[1].Split(new string[] { "</h1>" }, StringSplitOptions.None);

                        string[] a13 = source.Split(new string[] { "<dt>Satış fiyatı :</dt>" }, StringSplitOptions.None);
                        string[] a14 = a13[1].Split(new string[] { "<dd>" }, StringSplitOptions.None);
                        string[] a15 = a14[1].Split(new string[] { "</dd>" }, StringSplitOptions.None);

                        string[] a16 = source.Split(new string[] { "<div id=\"dstoretab1\" class=\"dstoretab\">" }, StringSplitOptions.None);
                        string[] a17 = a16[1].Split(new string[] { "<div id=\"dstoretab2\" class=\"dstoretab\">" }, StringSplitOptions.None);

                        worksheet.Cells[satir, sutun].Value = a7[0];
                        sutun++;
                        worksheet.Cells[satir, sutun].Value = a15[0];
                        sutun++;
                        worksheet.Cells[satir, sutun].Value = a5[0];
                        sutun++;
                        worksheet.Cells[satir, sutun].Value = a17[0];
                        sutun++;
                    }
                    package.Save();
                }
                MessageBox.Show("Tamamlandı.");
            }
            else if (comboBox1.Text == "--Retro Laptop Sticker")
            {
                var fileName = "jasmin2020_retro_laptop_sticker-" + DateTime.Now.ToString("HH-mm-ss__dd-MM-yyyy") + ".xlsx";
                var file = new FileInfo(Application.StartupPath + "\\" + fileName);
                int satir = 1, sutun = 1;
                using (var package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sayfa1");
                    worksheet.Cells[1, 1].Value = "Ürün Adı";
                    worksheet.Cells[1, 2].Value = "Fiyatı";
                    worksheet.Cells[1, 3].Value = "Resmi";
                    worksheet.Cells[1, 4].Value = "Açıklaması";

                    for (int i = 1; i <= 2; i++)
                    {
                        webBrowser2.Navigate("http://www.jasmin2020.com/page.php?page=" + i.ToString() + "&act=kategoriGoster&catID=55&markaID=spkomut_HEPSI&name=retro-laptop-sticker&orderBy=&op=");
                        while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                            Application.DoEvents();
                        //W8();
                        string[] a = webBrowser2.DocumentText.Split(new string[] { "<div class=\"product-gallery\">" }, StringSplitOptions.None);
                        string[] a1 = a[1].Split(new string[] { "<div class=\"pagination\">" }, StringSplitOptions.None);
                        string[] a2 = a1[0].Split(new string[] { "<a href=\"" }, StringSplitOptions.None);
                        for (int j = 1; j < a2.Length; j++)
                        {
                            satir++; sutun = 1;
                            string[] a3 = a2[j].Split('"');
                            webBrowser2.Navigate("http://www.jasmin2020.com/" + a3[0]);
                            while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                                Application.DoEvents();
                            //W8();

                            StreamReader sr = new StreamReader(this.webBrowser2.DocumentStream, Encoding.GetEncoding("iso-8859-9"));
                            string source = sr.ReadToEnd();


                            string[] a4 = source.Split(new string[] { "<meta property=\"og:image\" content=\"" }, StringSplitOptions.None);
                            string[] a5 = a4[1].Split('"');

                            string[] a6 = source.Split(new string[] { "<h1>" }, StringSplitOptions.None);
                            string[] a7 = a6[1].Split(new string[] { "</h1>" }, StringSplitOptions.None);

                            string[] a13 = source.Split(new string[] { "<dt>Satış fiyatı :</dt>" }, StringSplitOptions.None);
                            string[] a14 = a13[1].Split(new string[] { "<dd>" }, StringSplitOptions.None);
                            string[] a15 = a14[1].Split(new string[] { "</dd>" }, StringSplitOptions.None);

                            string[] a16 = source.Split(new string[] { "<div id=\"dstoretab1\" class=\"dstoretab\">" }, StringSplitOptions.None);
                            string[] a17 = a16[1].Split(new string[] { "<div id=\"dstoretab2\" class=\"dstoretab\">" }, StringSplitOptions.None);

                            worksheet.Cells[satir, sutun].Value = a7[0];
                            sutun++;
                            worksheet.Cells[satir, sutun].Value = a15[0];
                            sutun++;
                            worksheet.Cells[satir, sutun].Value = a5[0];
                            sutun++;
                            worksheet.Cells[satir, sutun].Value = a17[0];
                            sutun++;
                        }
                    }
                    package.Save();
                }
                MessageBox.Show("Tamamlandı.");
            }
            else if (comboBox1.Text == "--Taş Desenli Duvar Paneli")
            {
                var fileName = "jasmin2020_tas_desenli_duvar_paneli-" + DateTime.Now.ToString("HH-mm-ss__dd-MM-yyyy") + ".xlsx";
                var file = new FileInfo(Application.StartupPath + "\\" + fileName);
                int satir = 1, sutun = 1;
                using (var package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sayfa1");
                    worksheet.Cells[1, 1].Value = "Ürün Adı";
                    worksheet.Cells[1, 2].Value = "Fiyatı";
                    worksheet.Cells[1, 3].Value = "Resmi";
                    worksheet.Cells[1, 4].Value = "Açıklaması";
                    webBrowser2.Navigate("http://www.jasmin2020.com/tas-desenli-duvar-paneli-kat75.html");
                    while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                        Application.DoEvents();
                    //W8();
                    string[] a = webBrowser2.DocumentText.Split(new string[] { "<div class=\"product-gallery\">" }, StringSplitOptions.None);
                    string[] a1 = a[1].Split(new string[] { "<div class=\"pagination\">" }, StringSplitOptions.None);
                    string[] a2 = a1[0].Split(new string[] { "<a href=\"" }, StringSplitOptions.None);
                    for (int j = 1; j < a2.Length; j++)
                    {
                        satir++; sutun = 1;
                        string[] a3 = a2[j].Split('"');
                        webBrowser2.Navigate("http://www.jasmin2020.com/" + a3[0]);
                        while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                            Application.DoEvents();
                        //W8();

                        StreamReader sr = new StreamReader(this.webBrowser2.DocumentStream, Encoding.GetEncoding("iso-8859-9"));
                        string source = sr.ReadToEnd();


                        string[] a4 = source.Split(new string[] { "<meta property=\"og:image\" content=\"" }, StringSplitOptions.None);
                        string[] a5 = a4[1].Split('"');

                        string[] a6 = source.Split(new string[] { "<h1>" }, StringSplitOptions.None);
                        string[] a7 = a6[1].Split(new string[] { "</h1>" }, StringSplitOptions.None);

                        string[] a13 = source.Split(new string[] { "<dt>Satış fiyatı :</dt>" }, StringSplitOptions.None);
                        string[] a14 = a13[1].Split(new string[] { "<dd>" }, StringSplitOptions.None);
                        string[] a15 = a14[1].Split(new string[] { "</dd>" }, StringSplitOptions.None);

                        string[] a16 = source.Split(new string[] { "<div id=\"dstoretab1\" class=\"dstoretab\">" }, StringSplitOptions.None);
                        string[] a17 = a16[1].Split(new string[] { "<div id=\"dstoretab2\" class=\"dstoretab\">" }, StringSplitOptions.None);

                        worksheet.Cells[satir, sutun].Value = a7[0];
                        sutun++;
                        worksheet.Cells[satir, sutun].Value = a15[0];
                        sutun++;
                        worksheet.Cells[satir, sutun].Value = a5[0];
                        sutun++;
                        worksheet.Cells[satir, sutun].Value = a17[0];
                        sutun++;
                    }
                    package.Save();
                }
                MessageBox.Show("Tamamlandı.");
            }
            else if (comboBox1.Text == "--Tuğla Desenli Duvar Paneli")
            {
                var fileName = "jasmin2020_tugla_desenli_duvar_paneli-" + DateTime.Now.ToString("HH-mm-ss__dd-MM-yyyy") + ".xlsx";
                var file = new FileInfo(Application.StartupPath + "\\" + fileName);
                int satir = 1, sutun = 1;
                using (var package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sayfa1");
                    worksheet.Cells[1, 1].Value = "Ürün Adı";
                    worksheet.Cells[1, 2].Value = "Fiyatı";
                    worksheet.Cells[1, 3].Value = "Resmi";
                    worksheet.Cells[1, 4].Value = "Açıklaması";
                    webBrowser2.Navigate("http://www.jasmin2020.com/tugla-desenli-duvar-paneli-kat102.html");
                    while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                        Application.DoEvents();
                    //W8();
                    string[] a = webBrowser2.DocumentText.Split(new string[] { "<div class=\"product-gallery\">" }, StringSplitOptions.None);
                    string[] a1 = a[1].Split(new string[] { "<div class=\"pagination\">" }, StringSplitOptions.None);
                    string[] a2 = a1[0].Split(new string[] { "<a href=\"" }, StringSplitOptions.None);
                    for (int j = 1; j < a2.Length; j++)
                    {
                        satir++; sutun = 1;
                        string[] a3 = a2[j].Split('"');
                        webBrowser2.Navigate("http://www.jasmin2020.com/" + a3[0]);
                        while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                            Application.DoEvents();
                        //W8();

                        StreamReader sr = new StreamReader(this.webBrowser2.DocumentStream, Encoding.GetEncoding("iso-8859-9"));
                        string source = sr.ReadToEnd();


                        string[] a4 = source.Split(new string[] { "<meta property=\"og:image\" content=\"" }, StringSplitOptions.None);
                        string[] a5 = a4[1].Split('"');

                        string[] a6 = source.Split(new string[] { "<h1>" }, StringSplitOptions.None);
                        string[] a7 = a6[1].Split(new string[] { "</h1>" }, StringSplitOptions.None);

                        string[] a13 = source.Split(new string[] { "<dt>Satış fiyatı :</dt>" }, StringSplitOptions.None);
                        string[] a14 = a13[1].Split(new string[] { "<dd>" }, StringSplitOptions.None);
                        string[] a15 = a14[1].Split(new string[] { "</dd>" }, StringSplitOptions.None);

                        string[] a16 = source.Split(new string[] { "<div id=\"dstoretab1\" class=\"dstoretab\">" }, StringSplitOptions.None);
                        string[] a17 = a16[1].Split(new string[] { "<div id=\"dstoretab2\" class=\"dstoretab\">" }, StringSplitOptions.None);

                        worksheet.Cells[satir, sutun].Value = a7[0];
                        sutun++;
                        worksheet.Cells[satir, sutun].Value = a15[0];
                        sutun++;
                        worksheet.Cells[satir, sutun].Value = a5[0];
                        sutun++;
                        worksheet.Cells[satir, sutun].Value = a17[0];
                        sutun++;
                    }
                    package.Save();
                }
                MessageBox.Show("Tamamlandı.");
            }
            else if (comboBox1.Text == "--Ahşap Desenli Duvar Paneli")
            {
                var fileName = "jasmin2020_ahsap_desenli_duvar_paneli-" + DateTime.Now.ToString("HH-mm-ss__dd-MM-yyyy") + ".xlsx";
                var file = new FileInfo(Application.StartupPath + "\\" + fileName);
                int satir = 1, sutun = 1;
                using (var package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sayfa1");
                    worksheet.Cells[1, 1].Value = "Ürün Adı";
                    worksheet.Cells[1, 2].Value = "Fiyatı";
                    worksheet.Cells[1, 3].Value = "Resmi";
                    worksheet.Cells[1, 4].Value = "Açıklaması";
                    webBrowser2.Navigate("http://www.jasmin2020.com/ahsap-desenli-duvar-paneli-kat103.html");
                    while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                        Application.DoEvents();
                    //W8();
                    string[] a = webBrowser2.DocumentText.Split(new string[] { "<div class=\"product-gallery\">" }, StringSplitOptions.None);
                    string[] a1 = a[1].Split(new string[] { "<div class=\"pagination\">" }, StringSplitOptions.None);
                    string[] a2 = a1[0].Split(new string[] { "<a href=\"" }, StringSplitOptions.None);
                    for (int j = 1; j < a2.Length; j++)
                    {
                        satir++; sutun = 1;
                        string[] a3 = a2[j].Split('"');
                        webBrowser2.Navigate("http://www.jasmin2020.com/" + a3[0]);
                        while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                            Application.DoEvents();
                        //W8();

                        StreamReader sr = new StreamReader(this.webBrowser2.DocumentStream, Encoding.GetEncoding("iso-8859-9"));
                        string source = sr.ReadToEnd();


                        string[] a4 = source.Split(new string[] { "<meta property=\"og:image\" content=\"" }, StringSplitOptions.None);
                        string[] a5 = a4[1].Split('"');

                        string[] a6 = source.Split(new string[] { "<h1>" }, StringSplitOptions.None);
                        string[] a7 = a6[1].Split(new string[] { "</h1>" }, StringSplitOptions.None);

                        string[] a13 = source.Split(new string[] { "<dt>Satış fiyatı :</dt>" }, StringSplitOptions.None);
                        string[] a14 = a13[1].Split(new string[] { "<dd>" }, StringSplitOptions.None);
                        string[] a15 = a14[1].Split(new string[] { "</dd>" }, StringSplitOptions.None);

                        string[] a16 = source.Split(new string[] { "<div id=\"dstoretab1\" class=\"dstoretab\">" }, StringSplitOptions.None);
                        string[] a17 = a16[1].Split(new string[] { "<div id=\"dstoretab2\" class=\"dstoretab\">" }, StringSplitOptions.None);

                        worksheet.Cells[satir, sutun].Value = a7[0];
                        sutun++;
                        worksheet.Cells[satir, sutun].Value = a15[0];
                        sutun++;
                        worksheet.Cells[satir, sutun].Value = a5[0];
                        sutun++;
                        worksheet.Cells[satir, sutun].Value = a17[0];
                        sutun++;
                    }
                    package.Save();
                }
                MessageBox.Show("Tamamlandı.");
            }
            else if (comboBox1.Text == "--Kilim")
            {
                var fileName = "jasmin2020_kilim-" + DateTime.Now.ToString("HH-mm-ss__dd-MM-yyyy") + ".xlsx";
                var file = new FileInfo(Application.StartupPath + "\\" + fileName);
                int satir = 1, sutun = 1;
                using (var package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sayfa1");
                    worksheet.Cells[1, 1].Value = "Ürün Adı";
                    worksheet.Cells[1, 2].Value = "Fiyatı";
                    worksheet.Cells[1, 3].Value = "Resmi";
                    worksheet.Cells[1, 4].Value = "Açıklaması";

                    for (int i = 1; i <= 8; i++)
                    {
                        webBrowser2.Navigate("http://www.jasmin2020.com/page.php?page=" + i.ToString() + "&act=kategoriGoster&catID=118&markaID=spkomut_HEPSI&name=kilim&orderBy=&op=");
                        while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                            Application.DoEvents();
                        //W8();
                        string[] a = webBrowser2.DocumentText.Split(new string[] { "<div class=\"product-gallery\">" }, StringSplitOptions.None);
                        string[] a1 = a[1].Split(new string[] { "<div class=\"pagination\">" }, StringSplitOptions.None);
                        string[] a2 = a1[0].Split(new string[] { "<a href=\"" }, StringSplitOptions.None);
                        for (int j = 1; j < a2.Length; j++)
                        {
                            satir++; sutun = 1;
                            string[] a3 = a2[j].Split('"');
                            webBrowser2.Navigate("http://www.jasmin2020.com/" + a3[0]);
                            while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                                Application.DoEvents();
                            //W8();

                            StreamReader sr = new StreamReader(this.webBrowser2.DocumentStream, Encoding.GetEncoding("iso-8859-9"));
                            string source = sr.ReadToEnd();


                            string[] a4 = source.Split(new string[] { "<meta property=\"og:image\" content=\"" }, StringSplitOptions.None);
                            string[] a5 = a4[1].Split('"');

                            string[] a6 = source.Split(new string[] { "<h1>" }, StringSplitOptions.None);
                            string[] a7 = a6[1].Split(new string[] { "</h1>" }, StringSplitOptions.None);

                            string[] a13 = source.Split(new string[] { "<dt>Satış fiyatı :</dt>" }, StringSplitOptions.None);
                            string[] a14 = a13[1].Split(new string[] { "<dd>" }, StringSplitOptions.None);
                            string[] a15 = a14[1].Split(new string[] { "</dd>" }, StringSplitOptions.None);

                            string[] a16 = source.Split(new string[] { "<div id=\"dstoretab1\" class=\"dstoretab\">" }, StringSplitOptions.None);
                            string[] a17 = a16[1].Split(new string[] { "<div id=\"dstoretab2\" class=\"dstoretab\">" }, StringSplitOptions.None);

                            worksheet.Cells[satir, sutun].Value = a7[0];
                            sutun++;
                            worksheet.Cells[satir, sutun].Value = a15[0];
                            sutun++;
                            worksheet.Cells[satir, sutun].Value = a5[0];
                            sutun++;
                            worksheet.Cells[satir, sutun].Value = a17[0];
                            sutun++;
                        }
                    }
                    package.Save();
                }
                MessageBox.Show("Tamamlandı.");
            }
            else if (comboBox1.Text == "--3 Boyutlu Saatler")
            {
                var fileName = "jasmin2020_3_boyutlu_saatler-" + DateTime.Now.ToString("HH-mm-ss__dd-MM-yyyy") + ".xlsx";
                var file = new FileInfo(Application.StartupPath + "\\" + fileName);
                int satir = 1, sutun = 1;
                using (var package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sayfa1");
                    worksheet.Cells[1, 1].Value = "Ürün Adı";
                    worksheet.Cells[1, 2].Value = "Fiyatı";
                    worksheet.Cells[1, 3].Value = "Resmi";
                    worksheet.Cells[1, 4].Value = "Açıklaması";
                    webBrowser2.Navigate("http://www.jasmin2020.com/3-boyutlu-saatler-kat101.html");
                    while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                        Application.DoEvents();
                    //W8();
                    string[] a = webBrowser2.DocumentText.Split(new string[] { "<div class=\"product-gallery\">" }, StringSplitOptions.None);
                    string[] a1 = a[1].Split(new string[] { "<div class=\"pagination\">" }, StringSplitOptions.None);
                    string[] a2 = a1[0].Split(new string[] { "<a href=\"" }, StringSplitOptions.None);
                    for (int j = 1; j < a2.Length; j++)
                    {
                        satir++; sutun = 1;
                        string[] a3 = a2[j].Split('"');
                        webBrowser2.Navigate("http://www.jasmin2020.com/" + a3[0]);
                        while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                            Application.DoEvents();
                        //W8();

                        StreamReader sr = new StreamReader(this.webBrowser2.DocumentStream, Encoding.GetEncoding("iso-8859-9"));
                        string source = sr.ReadToEnd();


                        string[] a4 = source.Split(new string[] { "<meta property=\"og:image\" content=\"" }, StringSplitOptions.None);
                        string[] a5 = a4[1].Split('"');

                        string[] a6 = source.Split(new string[] { "<h1>" }, StringSplitOptions.None);
                        string[] a7 = a6[1].Split(new string[] { "</h1>" }, StringSplitOptions.None);

                        string[] a13 = source.Split(new string[] { "<dt>Satış fiyatı :</dt>" }, StringSplitOptions.None);
                        string[] a14 = a13[1].Split(new string[] { "<dd>" }, StringSplitOptions.None);
                        string[] a15 = a14[1].Split(new string[] { "</dd>" }, StringSplitOptions.None);

                        string[] a16 = source.Split(new string[] { "<div id=\"dstoretab1\" class=\"dstoretab\">" }, StringSplitOptions.None);
                        string[] a17 = a16[1].Split(new string[] { "<div id=\"dstoretab2\" class=\"dstoretab\">" }, StringSplitOptions.None);

                        worksheet.Cells[satir, sutun].Value = a7[0];
                        sutun++;
                        worksheet.Cells[satir, sutun].Value = a15[0];
                        sutun++;
                        worksheet.Cells[satir, sutun].Value = a5[0];
                        sutun++;
                        worksheet.Cells[satir, sutun].Value = a17[0];
                        sutun++;
                    }
                    package.Save();
                }
                MessageBox.Show("Tamamlandı.");
            }
            else if (comboBox1.Text == "--Akrilik Boyama Saatler")
            {
                var fileName = "jasmin2020_akrilik_boyama_saatler-" + DateTime.Now.ToString("HH-mm-ss__dd-MM-yyyy") + ".xlsx";
                var file = new FileInfo(Application.StartupPath + "\\" + fileName);
                int satir = 1, sutun = 1;
                using (var package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sayfa1");
                    worksheet.Cells[1, 1].Value = "Ürün Adı";
                    worksheet.Cells[1, 2].Value = "Fiyatı";
                    worksheet.Cells[1, 3].Value = "Resmi";
                    worksheet.Cells[1, 4].Value = "Açıklaması";
                    webBrowser2.Navigate("http://www.jasmin2020.com/akrilik-boyama-saatler-kat107.html");
                    while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                        Application.DoEvents();
                    //W8();
                    string[] a = webBrowser2.DocumentText.Split(new string[] { "<div class=\"product-gallery\">" }, StringSplitOptions.None);
                    string[] a1 = a[1].Split(new string[] { "<div class=\"pagination\">" }, StringSplitOptions.None);
                    string[] a2 = a1[0].Split(new string[] { "<a href=\"" }, StringSplitOptions.None);
                    for (int j = 1; j < a2.Length; j++)
                    {
                        satir++; sutun = 1;
                        string[] a3 = a2[j].Split('"');
                        webBrowser2.Navigate("http://www.jasmin2020.com/" + a3[0]);
                        while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                            Application.DoEvents();
                        //W8();

                        StreamReader sr = new StreamReader(this.webBrowser2.DocumentStream, Encoding.GetEncoding("iso-8859-9"));
                        string source = sr.ReadToEnd();


                        string[] a4 = source.Split(new string[] { "<meta property=\"og:image\" content=\"" }, StringSplitOptions.None);
                        string[] a5 = a4[1].Split('"');

                        string[] a6 = source.Split(new string[] { "<h1>" }, StringSplitOptions.None);
                        string[] a7 = a6[1].Split(new string[] { "</h1>" }, StringSplitOptions.None);

                        string[] a13 = source.Split(new string[] { "<dt>Satış fiyatı :</dt>" }, StringSplitOptions.None);
                        string[] a14 = a13[1].Split(new string[] { "<dd>" }, StringSplitOptions.None);
                        string[] a15 = a14[1].Split(new string[] { "</dd>" }, StringSplitOptions.None);

                        string[] a16 = source.Split(new string[] { "<div id=\"dstoretab1\" class=\"dstoretab\">" }, StringSplitOptions.None);
                        string[] a17 = a16[1].Split(new string[] { "<div id=\"dstoretab2\" class=\"dstoretab\">" }, StringSplitOptions.None);

                        worksheet.Cells[satir, sutun].Value = a7[0];
                        sutun++;
                        worksheet.Cells[satir, sutun].Value = a15[0];
                        sutun++;
                        worksheet.Cells[satir, sutun].Value = a5[0];
                        sutun++;
                        worksheet.Cells[satir, sutun].Value = a17[0];
                        sutun++;
                    }
                    package.Save();
                }
                MessageBox.Show("Tamamlandı.");
            }
            else if (comboBox1.Text == "--Ahşap Tablo Saatler")
            {
                var fileName = "jasmin2020_ashap_tablo_saatler-" + DateTime.Now.ToString("HH-mm-ss__dd-MM-yyyy") + ".xlsx";
                var file = new FileInfo(Application.StartupPath + "\\" + fileName);
                int satir = 1, sutun = 1;
                using (var package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sayfa1");
                    worksheet.Cells[1, 1].Value = "Ürün Adı";
                    worksheet.Cells[1, 2].Value = "Fiyatı";
                    worksheet.Cells[1, 3].Value = "Resmi";
                    worksheet.Cells[1, 4].Value = "Açıklaması";

                    for (int i = 1; i <= 3; i++)
                    {
                        webBrowser2.Navigate("http://www.jasmin2020.com/page.php?page=" + i.ToString() + "&act=kategoriGoster&catID=105&markaID=spkomut_HEPSI&name=ahsap-tablo-saatler&orderBy=&op=");
                        while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                            Application.DoEvents();
                        //W8();
                        string[] a = webBrowser2.DocumentText.Split(new string[] { "<div class=\"product-gallery\">" }, StringSplitOptions.None);
                        string[] a1 = a[1].Split(new string[] { "<div class=\"pagination\">" }, StringSplitOptions.None);
                        string[] a2 = a1[0].Split(new string[] { "<a href=\"" }, StringSplitOptions.None);
                        for (int j = 1; j < a2.Length; j++)
                        {
                            satir++; sutun = 1;
                            string[] a3 = a2[j].Split('"');
                            webBrowser2.Navigate("http://www.jasmin2020.com/" + a3[0]);
                            while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                                Application.DoEvents();
                            //W8();

                            StreamReader sr = new StreamReader(this.webBrowser2.DocumentStream, Encoding.GetEncoding("iso-8859-9"));
                            string source = sr.ReadToEnd();


                            string[] a4 = source.Split(new string[] { "<meta property=\"og:image\" content=\"" }, StringSplitOptions.None);
                            string[] a5 = a4[1].Split('"');

                            string[] a6 = source.Split(new string[] { "<h1>" }, StringSplitOptions.None);
                            string[] a7 = a6[1].Split(new string[] { "</h1>" }, StringSplitOptions.None);

                            string[] a13 = source.Split(new string[] { "<dt>Satış fiyatı :</dt>" }, StringSplitOptions.None);
                            string[] a14 = a13[1].Split(new string[] { "<dd>" }, StringSplitOptions.None);
                            string[] a15 = a14[1].Split(new string[] { "</dd>" }, StringSplitOptions.None);

                            string[] a16 = source.Split(new string[] { "<div id=\"dstoretab1\" class=\"dstoretab\">" }, StringSplitOptions.None);
                            string[] a17 = a16[1].Split(new string[] { "<div id=\"dstoretab2\" class=\"dstoretab\">" }, StringSplitOptions.None);

                            worksheet.Cells[satir, sutun].Value = a7[0];
                            sutun++;
                            worksheet.Cells[satir, sutun].Value = a15[0];
                            sutun++;
                            worksheet.Cells[satir, sutun].Value = a5[0];
                            sutun++;
                            worksheet.Cells[satir, sutun].Value = a17[0];
                            sutun++;
                        }
                    }
                    package.Save();
                }
                MessageBox.Show("Tamamlandı.");
            }
            else if (comboBox1.Text == "--Duvar kağıdına Baskı Manzaralar")
            {
                var fileName = "jasmin2020_duvar_kagidina_baski_manzaralar-" + DateTime.Now.ToString("HH-mm-ss__dd-MM-yyyy") + ".xlsx";
                var file = new FileInfo(Application.StartupPath + "\\" + fileName);
                int satir = 1, sutun = 1;
                using (var package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sayfa1");
                    worksheet.Cells[1, 1].Value = "Ürün Adı";
                    worksheet.Cells[1, 2].Value = "Fiyatı";
                    worksheet.Cells[1, 3].Value = "Resmi";
                    worksheet.Cells[1, 4].Value = "Açıklaması";
                    worksheet.Cells[1, 5].Value = "Opsiyon1";
                    worksheet.Cells[1, 6].Value = "Opsiyon2";
                    worksheet.Cells[1, 7].Value = "Opsiyon3";

                    for (int i = 1; i <= 2; i++)
                    {
                        webBrowser2.Navigate("http://www.jasmin2020.com/page.php?page=" + i.ToString() + "&act=kategoriGoster&catID=111&markaID=spkomut_HEPSI&name=duvar-kagidina-baski-manzaralar&orderBy=&op=");
                        while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                            Application.DoEvents();
                        //W8();
                        string[] a = webBrowser2.DocumentText.Split(new string[] { "<div class=\"product-gallery\">" }, StringSplitOptions.None);
                        string[] a1 = a[1].Split(new string[] { "<div class=\"pagination\">" }, StringSplitOptions.None);
                        string[] a2 = a1[0].Split(new string[] { "<a href=\"" }, StringSplitOptions.None);
                        for (int j = 1; j < a2.Length; j++)
                        {
                            satir++; sutun = 1;
                            string[] a3 = a2[j].Split('"');
                            webBrowser2.Navigate("http://www.jasmin2020.com/" + a3[0]);
                            while (webBrowser2.ReadyState != WebBrowserReadyState.Complete)
                                Application.DoEvents();
                            //W8();

                            StreamReader sr = new StreamReader(this.webBrowser2.DocumentStream, Encoding.GetEncoding("iso-8859-9"));
                            string source = sr.ReadToEnd();


                            string[] a4 = source.Split(new string[] { "<meta property=\"og:image\" content=\"" }, StringSplitOptions.None);
                            string[] a5 = a4[1].Split('"');

                            string[] a6 = source.Split(new string[] { "<h1>" }, StringSplitOptions.None);
                            string[] a7 = a6[1].Split(new string[] { "</h1>" }, StringSplitOptions.None);

                            string[] a13 = source.Split(new string[] { "<dt>Satış fiyatı :</dt>" }, StringSplitOptions.None);
                            string[] a14 = a13[1].Split(new string[] { "<dd>" }, StringSplitOptions.None);
                            string[] a15 = a14[1].Split(new string[] { "</dd>" }, StringSplitOptions.None);

                            string[] a16 = source.Split(new string[] { "<div id=\"dstoretab1\" class=\"dstoretab\">" }, StringSplitOptions.None);
                            string[] a17 = a16[1].Split(new string[] { "<div id=\"dstoretab2\" class=\"dstoretab\">" }, StringSplitOptions.None);

                            worksheet.Cells[satir, sutun].Value = a7[0];
                            sutun++;
                            worksheet.Cells[satir, sutun].Value = a15[0];
                            sutun++;
                            worksheet.Cells[satir, sutun].Value = a5[0];
                            sutun++;
                            worksheet.Cells[satir, sutun].Value = a17[0];
                            sutun++;

                            string[] a8 = source.Split(new string[] { "<li value='" }, StringSplitOptions.None);
                            for (int k = 1; k < a8.Length; k++)
                            {
                                string[] a9 = a8[k].Split('\'');
                                string[] a11 = a8[k].Split(new string[] { "<b>" }, StringSplitOptions.None);
                                string[] a12 = a11[1].Split('<');
                                worksheet.Cells[satir, sutun].Value = a9[0] + " (" + a12[0] + ")";
                                sutun++;
                            }


                        }
                    }
                    package.Save();
                }
                MessageBox.Show("Tamamlandı.");
            }
        
        }

        private void webBrowser2_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            this.Text = e.Url.ToString();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            List<string> links = new List<string>();
            var fileName = "ozcan-" + DateTime.Now.ToString("HH-mm-ss__dd-MM-yyyy") + ".xlsx";
            var file = new FileInfo(Application.StartupPath + "\\" + fileName);
            int satir = 1, sutun = 1;
            using (var package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sayfa1");
                worksheet.Cells[1, 1].Value = "Ürün Adı";
                worksheet.Cells[1, 2].Value = "Ürün Kodu";
                worksheet.Cells[1, 3].Value = "Kategori Yolu";
                worksheet.Cells[1, 4].Value = "Açıklama";
                worksheet.Cells[1, 5].Value = "Resim1";
                worksheet.Cells[1, 6].Value = "Resim2";
                worksheet.Cells[1, 7].Value = "Resim3";
                for (int i = 1; i <= 16; i++)
                {
                    string src = "";
                    using (WebClient wb = new WebClient())
                    {
                        wb.Encoding = Encoding.UTF8;
                        src = wb.DownloadString("http://www.ozcanaydinlatma.com.tr/Default.aspx?pageID=34&CatID=213&page=" + i.ToString());
                    }
                    string[] a1 = src.Split(new string[] { "hypProduct\" href=\"" }, StringSplitOptions.None);
                    for (int k = 1; k < a1.Length; k++)
                    {
                        string[] a2 = a1[k].Split('"');
                        a2[0] = a2[0].Replace("amp;", "");
                        links.Add("http://www.ozcanaydinlatma.com.tr" + HttpUtility.UrlDecode(a2[0]));
                    }
                }
                for (int i = 1; i <= 23; i++)
                {
                    string src = "";
                    using (WebClient wb = new WebClient())
                    {
                        wb.Encoding = Encoding.UTF8;
                        src = wb.DownloadString("http://www.ozcanaydinlatma.com.tr/Default.aspx?pageID=34&CatID=219&page=" + i.ToString());
                    }
                    string[] a1 = src.Split(new string[] { "hypProduct\" href=\"" }, StringSplitOptions.None);
                    for (int k = 1; k < a1.Length; k++)
                    {
                        string[] a2 = a1[k].Split('"');
                        a2[0] = a2[0].Replace("amp;", "");
                        links.Add("http://www.ozcanaydinlatma.com.tr" + HttpUtility.UrlDecode(a2[0]));
                    }
                }
                for (int i = 1; i <= 9; i++)
                {
                    string src = "";
                    using (WebClient wb = new WebClient())
                    {
                        wb.Encoding = Encoding.UTF8;
                        src = wb.DownloadString("http://www.ozcanaydinlatma.com.tr/Default.aspx?pageID=34&CatID=214&page=" + i.ToString());
                    }
                    string[] a1 = src.Split(new string[] { "hypProduct\" href=\"" }, StringSplitOptions.None);
                    for (int k = 1; k < a1.Length; k++)
                    {
                        string[] a2 = a1[k].Split('"');
                        a2[0] = a2[0].Replace("amp;", "");
                        links.Add("http://www.ozcanaydinlatma.com.tr" + HttpUtility.UrlDecode(a2[0]));
                    }
                }
                for (int i = 1; i <= 2; i++)
                {
                    string src = "";
                    using (WebClient wb = new WebClient())
                    {
                        wb.Encoding = Encoding.UTF8;
                        src = wb.DownloadString("http://www.ozcanaydinlatma.com.tr/Default.aspx?pageID=34&CatID=215&page=" + i.ToString());
                    }
                    string[] a1 = src.Split(new string[] { "hypProduct\" href=\"" }, StringSplitOptions.None);
                    for (int k = 1; k < a1.Length; k++)
                    {
                        string[] a2 = a1[k].Split('"');
                        a2[0] = a2[0].Replace("amp;", "");
                        links.Add("http://www.ozcanaydinlatma.com.tr" + HttpUtility.UrlDecode(a2[0]));
                    }
                }
                for (int i = 1; i <= 1; i++)
                {
                    string src = "";
                    using (WebClient wb = new WebClient())
                    {
                        wb.Encoding = Encoding.UTF8;
                        src = wb.DownloadString("http://www.ozcanaydinlatma.com.tr/Default.aspx?pageID=34&CatID=216&page=" + i.ToString());
                    }
                    string[] a1 = src.Split(new string[] { "hypProduct\" href=\"" }, StringSplitOptions.None);
                    for (int k = 1; k < a1.Length; k++)
                    {
                        string[] a2 = a1[k].Split('"');
                        a2[0] = a2[0].Replace("amp;", "");
                        links.Add("http://www.ozcanaydinlatma.com.tr" + HttpUtility.UrlDecode(a2[0]));
                    }
                }
                for (int i = 1; i <= 1; i++)
                {
                    string src = "";
                    using (WebClient wb = new WebClient())
                    {
                        wb.Encoding = Encoding.UTF8;
                        src = wb.DownloadString("http://www.ozcanaydinlatma.com.tr/Default.aspx?pageID=34&CatID=218&page=" + i.ToString());
                    }
                    string[] a1 = src.Split(new string[] { "hypProduct\" href=\"" }, StringSplitOptions.None);
                    for (int k = 1; k < a1.Length; k++)
                    {
                        sutun++; satir = 1;
                        string[] a2 = a1[k].Split('"');
                        string src1 = "";
                        a2[0] = a2[0].Replace("amp;", "");
                        links.Add("http://www.ozcanaydinlatma.com.tr" + HttpUtility.UrlDecode(a2[0]));



                        string[] newlines = src1.Split(new string[] { "hypProduct\" href=\"" }, StringSplitOptions.None);
                        for (int j = 1; j < newlines.Length; j++)
                        {
                            string src2 = "";
                            string[] nl1 = newlines[j].Split('"');
                            nl1[0] = nl1[0].Replace("amp;", "");
                            using (WebClient wb = new WebClient())
                            {
                                wb.Encoding = Encoding.UTF8;
                                src2 = wb.DownloadString("http://www.ozcanaydinlatma.com.tr" + HttpUtility.UrlDecode(nl1[0]));
                            }
                        }
                    }
                }
                for (int i = 1; i <= 4; i++)
                {
                    string src = "";
                    using (WebClient wb = new WebClient())
                    {
                        wb.Encoding = Encoding.UTF8;
                        src = wb.DownloadString("http://www.ozcanaydinlatma.com.tr/Default.aspx?pageID=34&CatID=217&page=" + i.ToString());
                    }
                    string[] a1 = src.Split(new string[] { "hypProduct\" href=\"" }, StringSplitOptions.None);
                    for (int k = 1; k < a1.Length; k++)
                    {
                        string[] a2 = a1[k].Split('"');
                        a2[0] = a2[0].Replace("amp;", "");
                        links.Add("http://www.ozcanaydinlatma.com.tr" + HttpUtility.UrlDecode(a2[0]));
                    }
                }
                List<string> links2 = new List<string>();
                foreach (string s in links)
                {
                    string src = "";
                    using (WebClient wb = new WebClient())
                    {
                        wb.Encoding = Encoding.UTF8;
                        src = wb.DownloadString(s);
                    }
                    if (src.Contains("İlişkili Ürünler"))
                    {
                        string[] a1 = src.Split(new string[] { "hypProduct\" href=\"" }, StringSplitOptions.None);
                        for (int k = 1; k < a1.Length; k++)
                        {
                            string[] a2 = a1[k].Split('"');
                            a2[0] = a2[0].Replace("amp;", "");
                            if(!links.Contains("http://www.ozcanaydinlatma.com.tr" + HttpUtility.UrlDecode(a2[0])))
                                links2.Add("http://www.ozcanaydinlatma.com.tr" + HttpUtility.UrlDecode(a2[0]));
                        }
                    }
                }
                links2.AddRange(links);
                foreach (string s in links2)
                {
                    string src1 = "";
                    using (WebClient wb = new WebClient())
                    {
                        wb.Encoding = Encoding.UTF8;
                        src1 = wb.DownloadString(s);
                    }
                    sutun++; satir = 1;
                    string[] adi = src1.Split(new string[] { "<div class=\"urunAdi\">" }, StringSplitOptions.None);
                    string[] adi1 = adi[1].Split(new string[] { "</div>" }, StringSplitOptions.None);
                    adi1[0] = adi1[0].Trim();

                    string[] kodu = src1.Split(new string[] { "ProductCode\">" }, StringSplitOptions.None);
                    string[] kodu1 = kodu[1].Split('<');

                    string[] desc = src1.Split(new string[] { "ProductDescription\">" }, StringSplitOptions.None);
                    string[] desc1 = desc[1].Split(new string[] { "</table>" }, StringSplitOptions.None);
                    desc1[0] = desc1[0] + Environment.NewLine + "</table>";

                    string[] pic = src1.Split(new string[] { "<a href=\"/images/Products/" }, StringSplitOptions.None);
                    string[] pic1 = pic[1].Split('"');
                    pic1[0] = "http://www.ozcanaydinlatma.com.tr/images/Products/" + pic1[0];

                    string path = "";

                    string[] p1 = src1.Split(new string[] { "&raquo;" }, StringSplitOptions.None);
                    string[] p2 = p1[2].Split('<');
                    path += p2[0] + " >> ";
                    string[] p3 = src1.Split(new string[] { "&raquo; </span>" }, StringSplitOptions.None);
                    string[] p4 = p3[p3.Length - 1].Split('<');
                    path += p4[0];

                    worksheet.Cells[sutun, satir].Value = adi1[0];
                    satir++;
                    worksheet.Cells[sutun, satir].Value = kodu1[0];
                    satir++;
                    worksheet.Cells[sutun, satir].Value = path;
                    satir++;
                    worksheet.Cells[sutun, satir].Value = desc1[0];
                    satir++;
                    worksheet.Cells[sutun, satir].Value = pic1[0];
                    satir++;
                }
                package.Save();
            }
            MessageBox.Show("OK");
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            Thread a = new Thread(new ThreadStart(geteweb4));
            a.Start();
        }
        private void geteweb4()
        {
            var fileName = "eweb4-" + DateTime.Now.ToString("HH-mm-ss") + ".xlsx";
            var file = new FileInfo(Application.StartupPath + "\\" + fileName);
            int satir = 1, sutun = 1;
            using (var package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sayfa1");
                worksheet.Cells[1, 1].Value = "Ürün Adı";
                worksheet.Cells[1, 2].Value = "Ürün Resmi";
                string src = "";
                using (WebClient wb = new WebClient())
                {
                    wb.Encoding = Encoding.UTF8;
                    src = wb.DownloadString(textBox2.Text);
                }
                string[] pg = src.Split(new string[] { "<li><a class=\"btn med\" href=\"" }, StringSplitOptions.None);
                string[] pg1 = pg[pg.Length - 1].Split(new string[] { "onclick=\"return PG.R(" }, StringSplitOptions.None);
                string[] pg2 = pg1[1].Split(')');
                for (int i = 1; i <= Convert.ToInt32(pg2[0]); i++)
                {
                    label12.Text = "Toplam; " + i.ToString() + "/" + pg2[0] + " Sayfa";
                    using (WebClient wb = new WebClient())
                    {
                        wb.Encoding = Encoding.UTF8;
                        src = wb.DownloadString(textBox2.Text + i.ToString() + ".html");
                    }
                    string[] a = src.Split(new string[] { "<ul class=\"tnl1\">" }, StringSplitOptions.None);
                    string[] a1 = a[1].Split(new string[] { "<a href=\"" }, StringSplitOptions.None);
                    for (int j = 1; j < a1.Length; j++)
                    {
                        satir++; sutun = 1;
                        string[] a2 = a1[j].Split('"');
                        string src1 = "";
                        this.Text = "http://hdw.eweb4.com" + a2[0];
                        using (WebClient wb = new WebClient())
                        {
                            wb.Encoding = Encoding.UTF8;
                            src1 = wb.DownloadString("http://hdw.eweb4.com" + a2[0]);
                        }
                        string[] b1 = src1.Split(new string[] { "data-text=\"" }, StringSplitOptions.None);
                        string[] b2 = b1[1].Split('"');
                        worksheet.Cells[satir, sutun].Value = b2[0];
                        sutun++;
                        if (src1.Contains("window.open('"))
                        {
                            string[] b3 = src1.Split(new string[] { "window.open('" }, StringSplitOptions.None);
                            string[] b4 = b3[1].Split('\'');
                            worksheet.Cells[satir, sutun].Value = b4[0];
                            sutun++;
                        }
                        else if (src1.Contains("<div class=\"related2\">"))
                        {
                            string[] b5 = src1.Split(new string[] { "<div class=\"related2\">" }, StringSplitOptions.None);
                            string[] b6 = b5[1].Split(new string[] { "</div>" }, StringSplitOptions.None);
                            string[] b7 = b6[0].Split(new string[] { "<a href=\"" }, StringSplitOptions.None);
                            string[] b71 = b7[1].Split('"');
                            worksheet.Cells[satir, sutun].Value = b71[0];
                            sutun++;
                        }
                        else
                        {
                            string[] b8 = src1.Split(new string[] { "<meta property=\"og:image\" content=\"" }, StringSplitOptions.None);
                            string[] b9 = b8[1].Split('"');
                            worksheet.Cells[satir, sutun].Value = b9[0];
                            sutun++;
                        }
                    }
                }
                package.Save();
            } MessageBox.Show("Tamamlandı.");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Thread a = new Thread(new ThreadStart(notebookdepo));
            a.Start();
        }
        private void notebookdepo()
        {
            string[] kactanKaca = textBox3.Text.Split(',');
            int ss = 0;
            string[] lb = listBox3.Items[listBox3.SelectedIndex].ToString().Split('m');
            lb[1] = lb[1].Replace("/","");
            var fileName = "notebookDepo-"+lb[1]+"-" + kactanKaca[0]+"-"+kactanKaca[1] + ".xlsx";
            var file = new FileInfo(Application.StartupPath + "\\" + fileName);
            int satir = 1, sutun = 1;
            using (var package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sayfa1");
                worksheet.Cells[1, 1].Value = "Ürün Adı";
                worksheet.Cells[1, 2].Value = "Kategori";
                worksheet.Cells[1, 3].Value = "Marka";
                worksheet.Cells[1, 4].Value = "Model";
                worksheet.Cells[1, 5].Value = "SKU";
                worksheet.Cells[1, 6].Value = "Telefonla Sipariş Kodu";
                worksheet.Cells[1, 7].Value = "Fiyatı";
                worksheet.Cells[1, 8].Value = "Açıklaması";
                worksheet.Cells[1, 9].Value = "Resim1";
                worksheet.Cells[1, 10].Value = "Resim2";
                worksheet.Cells[1, 11].Value = "Resim3";
                worksheet.Cells[1, 12].Value = "Resim4";
                worksheet.Cells[1, 13].Value = "Varyant1";
                worksheet.Cells[1, 14].Value = "Varyant2";
                worksheet.Cells[1, 15].Value = "Varyant3";
                worksheet.Cells[1, 16].Value = "Varyant4";
                worksheet.Cells[1, 17].Value = "Varyant5";
                worksheet.Cells["A1:Q1"].Style.Font.Bold = true;
                    ss++;
                    string src = "";
                    for (int j = Convert.ToInt32(kactanKaca[0]); j <= Convert.ToInt32(kactanKaca[1]); j++)
                    {
                        this.Text = j + " to " + Convert.ToInt32(kactanKaca[1]) + " Adet Sayfa";
                        using (WebClient wb = new WebClient())
                        {
                            wb.Encoding = Encoding.UTF8;
                            src = wb.DownloadString(listBox3.SelectedItem.ToString() + "?rpg=" + j);
                        }
                        listBox2.Items.Add(listBox3.SelectedItem.ToString() + "?rpg=" + j);
                        listBox2.SelectedIndex = listBox2.Items.Count - 1;
                        listBox2.SelectedIndex = -1;
                        string[] gItem = src.Split(new string[] { "<div class=\"product item1\">" }, StringSplitOptions.None);
                        listBox2.Items.Add((gItem.Length - 1).ToString() + " Adet İtem");
                        listBox2.SelectedIndex = listBox2.Items.Count - 1;
                        listBox2.SelectedIndex = -1;
                        for (int k = 1; k < gItem.Length; k++)
                        {
                            satir++; sutun = 1;
                            string[] gL = gItem[k].Split(new string[] { "href=\"" }, StringSplitOptions.None);
                            string[] gL1 = gL[1].Split('"');
                            try
                            {
                                using (WebClient wb = new WebClient())
                                {
                                    wb.Encoding = Encoding.UTF8;
                                    src = wb.DownloadString("http://www.notebookdepo.com" + gL1[0]);
                                }
                            }
                            catch { continue; }
                            listBox2.Items.Add("http://www.notebookdepo.com" + gL1[0]);
                            listBox2.SelectedIndex = listBox2.Items.Count - 1;
                            listBox2.SelectedIndex = -1;
                            if (!src.Contains("og:title"))
                                continue;
                            string[] adi = src.Split(new string[] { "property=\"og:title\" content=\"" }, StringSplitOptions.None);
                            string[] adi1 = adi[1].Split(new string[] { "\" />" }, StringSplitOptions.None);
                            //Adı adi1[0]
                            worksheet.Cells[satir, sutun].Value = adi1[0];
                            sutun++;

                            string[] kat = src.Split(new string[] { "<ul class=\"cfix\">" }, StringSplitOptions.None);
                            string[] kat1 = kat[1].Split(new string[] { "</div>" }, StringSplitOptions.None);
                            string[] kat2 = kat1[0].Split(new string[] { "<a id=\"" }, StringSplitOptions.None);
                            string cat = "";
                            for (int t = 1; t < kat2.Length; t++)
                            {
                                string[] kat3 = kat2[t].Split(new string[] { "\">" }, StringSplitOptions.None);
                                string[] kat4 = kat3[1].Split('<');
                                if (t != kat2.Length-1)
                                    cat += kat4[0] + " > ";
                                else
                                    cat += kat4[0];
                            }
                            //Kategori cat
                            worksheet.Cells[satir, sutun].Value = cat;
                            sutun++;

                            string[] marka = src.Split(new string[] { "Marka:</span>" }, StringSplitOptions.None);
                            string[] marka1 = marka[1].Split(new string[] { "<span>" }, StringSplitOptions.None);
                            string[] marka2 = marka1[1].Split(new string[] { "</span" }, StringSplitOptions.None);
                            //Marka marka2[0]
                            worksheet.Cells[satir, sutun].Value = marka2[0];
                            sutun++;
                            string[] model = src.Split(new string[] { "Model Numarası :</span>" }, StringSplitOptions.None);
                            string[] model1 = model[1].Split(new string[] { "<span>" }, StringSplitOptions.None);
                            string[] model2 = model1[1].Split(new string[] { "</span" }, StringSplitOptions.None);
                            //model model2[0]
                            worksheet.Cells[satir, sutun].Value = model2[0];
                            sutun++;
                            string[] sku = src.Split(new string[] { "SKU : </span>" }, StringSplitOptions.None);
                            string[] sku1 = sku[1].Split(new string[] { "<span>" }, StringSplitOptions.None);
                            string[] sku2 = sku1[1].Split(new string[] { "</span" }, StringSplitOptions.None);
                            //sku sku2[0]
                            worksheet.Cells[satir, sutun].Value = sku2[0];
                            sutun++;
                            string[] tlfs = src.Split(new string[] { "Telefonda Sipariş Kodu:</span>" }, StringSplitOptions.None);
                            string[] tlfs1 = tlfs[1].Split(new string[] { "<span>" }, StringSplitOptions.None);
                            string[] tlfs2 = tlfs1[1].Split(new string[] { "</span" }, StringSplitOptions.None);
                            //Tlf Sipariş Kodu tlfs2[0]
                            worksheet.Cells[satir, sutun].Value = tlfs2[0];
                            sutun++;
                            string fi = "0";
                            if (src.Contains("<span class=\"price\">"))
                            {
                                string[] fiyat = src.Split(new string[] { "<span class=\"price\">" }, StringSplitOptions.None);
                                string[] fiyat1 = fiyat[1].Split('<');
                                fi = fiyat1[0];
                            }

                            //Fiyat fiyat1[0]
                            worksheet.Cells[satir, sutun].Value = fi;
                            sutun++;

                            string[] desc = src.Split(new string[] { "<div id=\"Details\" class=\"tab-content\">" }, StringSplitOptions.None);
                            string[] desc1 = desc[1].Split(new string[] { "</div>" }, StringSplitOptions.None);

                            //Açıklama desc1[0]
                            worksheet.Cells[satir, sutun].Value = desc1[0];
                            sutun++;
                           
                            string[] img = src.Split(new string[] { "<img src='" }, StringSplitOptions.None);
                            for (int y = 1; y < img.Length; y++)
                            {
                                string[] img1 = img[y].Split('\'');
                                img1[0] = img1[0].Replace("small", "big");
                                img1[0] = "http://www.notebookdepo.com" + img1[0];
                                worksheet.Cells[satir, sutun].Value = img1[0];
                                sutun++;
                            }

                            if (src.Contains("RbtnVariant"))
                            {
                                string[] vr = src.Split(new string[] { "value=\"RbtnVariant\"" }, StringSplitOptions.None);
                                for (int u = 1; u < vr.Length; u++)
                                {
                                    string[] vr1 = vr[u].Split(new string[] { "<span>" }, StringSplitOptions.None);
                                    string[] vr2 = vr1[1].Split(new string[] { "</span>" }, StringSplitOptions.None);
                                    vr2[0] = vr2[0].Trim();
                                    worksheet.Cells[satir, sutun].Value = vr2[0];
                                    sutun++;
                                }
                            }
                        }
                    }



                package.Save();
            }
            MessageBox.Show("bitti");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                string src = "";
                using (WebClient wb = new WebClient())
                {
                    wb.Encoding = Encoding.UTF8;
                    src = wb.DownloadString(listBox3.SelectedItem.ToString());
                }
                string[] pg = src.Split(new string[] { "class=\"last\"" }, StringSplitOptions.None);
                string[] pg1 = pg[1].Split(new string[] { "rpg=" }, StringSplitOptions.None);
                string[] pg2 = pg1[1].Split('"');
                label13.Text = pg2[0];
            }
            catch { label13.Text = "1"; }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Thread b = new Thread(new ThreadStart(getVitamin));
            b.Start();
        }
        void getSupplement()
        {
            List<string> pages = new List<string>(){"http://www.supplementler.com/c/protein-tozu-11","http://www.supplementler.com/c/amino-asit-2",
            "http://www.supplementler.com/c/kilo-aldiricilar-5","http://www.supplementler.com/c/diyet-fat-burner-4","http://www.supplementler.com/c/performans-arttirici-3","http://www.supplementler.com/c/kreatin-14",
            "http://www.supplementler.com/c/sporcu-vitaminleri-13","http://www.supplementler.com/c/ozel-urunler-ve-aksesuar-16"};
            int satir = 1, sutun = 1;
            var fileName = "supplement.xlsx";
            var file = new FileInfo(Application.StartupPath + "\\" + fileName);
            using (var package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sayfa1");
                worksheet.Cells[1, 1].Value = "Adı";
                worksheet.Cells[1, 2].Value = "Kategorisi";
                worksheet.Cells[1, 3].Value = "Resmi";
                worksheet.Cells[1, 4].Value = "Fiyatı";
                worksheet.Cells[1, 5].Value = "Varyantları";
                worksheet.Cells[1, 6].Value = "Özellikleri";
                worksheet.Cells[1, 7].Value = "Açıklaması";
                worksheet.Cells[1, 8].Value = "İçeriği";
                worksheet.Cells[1, 9].Value = "Yeni Başlayanlar";
                worksheet.Cells[1, 10].Value = "Orta Seviye";
                worksheet.Cells[1, 11].Value = "İleri Seviye";
                worksheet.Cells["A1:T1"].Style.Font.Bold = true;
                foreach (string pg in pages)
                {
                    int sayfa = 1;
                    while (true)
                    {
                        string src = "";
                        using (WebClient wb = new WebClient())
                        {
                            wb.Encoding = Encoding.UTF8;
                            src = wb.DownloadString(pg+ "?pagenumber=" + sayfa);
                        }
                        listBox4.Items.Add(pg + "?pagenumber=" + sayfa);
                        listBox4.SelectedIndex = listBox4.Items.Count - 1;
                        listBox4.SelectedIndex = -1;
                        sayfa++;
                        if (src.Contains("Son filtreleme kriterlerlerinizle eşleşen bir ürün bulunamadı."))
                            break;

                        string[] l = src.Split(new string[] { "<div class=\"product-image\">" }, StringSplitOptions.None);
                        for (int i = 1; i < l.Length; i++)
                        {
                            satir++; sutun = 1;
                            string[] l1 = l[i].Split(new string[] { "<a href=\"" }, StringSplitOptions.None);
                            string[] l2 = l1[1].Split('"');
                            string src1 = "";
                            using (WebClient wb = new WebClient())
                            {
                                wb.Encoding = Encoding.UTF8;
                                src1 = wb.DownloadString("http://www.supplementler.com" + l2[0]);
                            }
                            listBox4.Items.Add("http://www.supplementler.com" + l2[0]);
                            listBox4.SelectedIndex = listBox4.Items.Count - 1;
                            listBox4.SelectedIndex = -1;
                            string[] adi = src1.Split(new string[] { "\"><b>" }, StringSplitOptions.None);
                            string[] adi1 = adi[1].Split(new string[] { "</b></a>" }, StringSplitOptions.None);
                            adi1[0] = HttpUtility.HtmlDecode(adi1[0]);
                            worksheet.Cells[satir, sutun].Value = adi1[0];
                            sutun++;


                            string[] cat = src1.Split(new string[] { "<div class=\"catalog_path path\" itemprop=\"breadcrumb\">" }, StringSplitOptions.None);
                            string[] cat1 = cat[1].Split(new string[] { "</div>" }, StringSplitOptions.None);
                            string catPath = "";
                            string[] cat2 = cat1[0].Split(new string[] { "\">" }, StringSplitOptions.None);
                            for (int r = 1; r < cat2.Length; r++)
                            {
                                string[] cat3 = cat2[r].Split(new string[] { "</a>" }, StringSplitOptions.None);
                                cat3[0] = cat3[0].Replace("<b>", "").Replace("</b>", "");
                                cat3[0] = HttpUtility.HtmlDecode(cat3[0]);
                                if (r != cat2.Length - 1)
                                    catPath += cat3[0] + " > ";
                                else
                                    catPath += cat3[0];
                            }

                            worksheet.Cells[satir, sutun].Value = catPath;
                            sutun++;

                            string[] img = src1.Split(new string[] { "<meta property=\"og:image\" content=\"" }, StringSplitOptions.None);
                            string[] img1 = img[1].Split('"');

                            worksheet.Cells[satir, sutun].Value = img1[0];
                            sutun++;
                            if (src1.Contains("<span class=\"productPrice\">"))
                            {
                                string[] price = src1.Split(new string[] { "<span class=\"productPrice\">" }, StringSplitOptions.None);
                                string[] price1 = price[1].Split(new string[] { "</span>" }, StringSplitOptions.None);
                                price1[0] = price1[0].Trim();
                                worksheet.Cells[satir, sutun].Value = price1[0];
                            }
                            else
                                worksheet.Cells[satir, sutun].Value = "0";
                            sutun++;

                            if (src1.Contains("<div class=\"product-options product-options"))
                            {
                                string ozellikTut = "";
                                string[] uOz = src1.Split(new string[] { "<div class=\"product-options product-options" }, StringSplitOptions.None);
                                string[] uOz1 = uOz[1].Split(new string[] { "</div>" }, StringSplitOptions.None);
                                if (uOz1[0].Contains("<span>"))
                                {
                                    string[] uOz2 = uOz1[0].Split(new string[] { "<span>" }, StringSplitOptions.None);
                                    string[] uOz3 = uOz2[1].Split(new string[] { "</span>" }, StringSplitOptions.None);
                                    //özellik adı uOz3[0];

                                    string[] uTur = uOz1[0].Split(new string[] { "<option  value=\"" }, StringSplitOptions.None);
                                    for (int j = 1; j < uTur.Length; j++)
                                    {
                                        string[] uTur1 = uTur[j].Split(new string[] { "\">" }, StringSplitOptions.None);
                                        string[] uTur2 = uTur1[1].Split(' ');
                                        uTur2[0] = HttpUtility.HtmlDecode(uTur2[0]);
                                        ozellikTut += uOz3[0] + ": " + uTur2[0];
                                    }
                                    worksheet.Cells[satir, sutun].Value = ozellikTut;
                                }
                                else
                                    worksheet.Cells[satir, sutun].Value = "";
                            }
                            else
                                worksheet.Cells[satir, sutun].Value = "";
                            sutun++;

                            if (src1.Contains("<div class=\"product-specs-module\">"))
                            {
                                string[] specs = src1.Split(new string[] { "<div class=\"product-specs-module\">" }, StringSplitOptions.None);
                                string[] specs1 = specs[1].Split(new string[] { "</div>" }, StringSplitOptions.None);
                                specs1[0] = HttpUtility.HtmlDecode(specs1[0]);

                                worksheet.Cells[satir, sutun].Value = specs1[0];
                            }
                            else
                                worksheet.Cells[satir, sutun].Value = "";
                            sutun++;
                            if (src1.Contains("<div class=\"infotext\" itemprop=\"description\">"))
                            {
                                string[] desc = src1.Split(new string[] { "<div class=\"infotext\" itemprop=\"description\">" }, StringSplitOptions.None);
                                string[] desc1 = desc[1].Split(new string[] { "</div>" }, StringSplitOptions.None);
                                desc1[0] = HttpUtility.HtmlDecode(desc1[0]);

                                worksheet.Cells[satir, sutun].Value = desc1[0];
                            }
                            else
                                worksheet.Cells[satir, sutun].Value = "";
                            sutun++;

                            if (src1.Contains("<div class=\"nutritionLabel \" style=\'cursor:pointer\' title=\'Ürün içeriğini görmek için tıklayınız\' itemprop=\"nutrition\" itemscope itemtype=\"http://schema.org/NutritionInformation\">"))
                            {
                                string[] itemProp = src1.Split(new string[] { "<div class=\"nutritionLabel \" style=\'cursor:pointer\' title=\'Ürün içeriğini görmek için tıklayınız\' itemprop=\"nutrition\" itemscope itemtype=\"http://schema.org/NutritionInformation\">" }, StringSplitOptions.None);
                                string[] itemProp1 = itemProp[1].Split(new string[] { "<div class=\"clear\"></div>" }, StringSplitOptions.None);
                                worksheet.Cells[satir, sutun].Value = itemProp1[0];
                            }
                            else
                                worksheet.Cells[satir, sutun].Value = "";
                            sutun++;

                            if (src1.Contains("<div class=\"infotext\"><p>"))
                            {
                                string[] items = src1.Split(new string[] { "<div class=\"infotext\"><p>" }, StringSplitOptions.None);
                                if (src1.Contains("tabitem1"))
                                {
                                    string[] yba = src1.Split(new string[] { "<div id=\"tabitem1\" class=\"inv-tab-content-small\">" }, StringSplitOptions.None);
                                    string[] yba1 = yba[1].Split(new string[] { "<div class=\"infotext\"><p>" }, StringSplitOptions.None);
                                    string[] yba2 = yba1[1].Split(new string[] { "</p></div>" }, StringSplitOptions.None);
                                    yba2[0] = HttpUtility.HtmlDecode(yba2[0]);
                                    worksheet.Cells[satir, sutun].Value = yba2[0];
                                }
                                else
                                    worksheet.Cells[satir, sutun].Value = "";
                                sutun++;
                                if (src1.Contains("tabitem2"))
                                {
                                    string[] ose = src1.Split(new string[] { "<div id=\"tabitem2\" class=\"inv-tab-content-small\">" }, StringSplitOptions.None);
                                    string[] ose1 = ose[1].Split(new string[] { "<div class=\"infotext\"><p>" }, StringSplitOptions.None);
                                    string[] ose2 = ose1[1].Split(new string[] { "</p></div>" }, StringSplitOptions.None);
                                    ose2[0] = HttpUtility.HtmlDecode(ose2[0]);
                                    worksheet.Cells[satir, sutun].Value = ose2[0];
                                }
                                else
                                    worksheet.Cells[satir, sutun].Value = "";
                                sutun++;
                                if (src1.Contains("tabitem3"))
                                {
                                    string[] ise = src1.Split(new string[] { "<div id=\"tabitem3\" class=\"inv-tab-content-small\">" }, StringSplitOptions.None);
                                    string[] ise1 = ise[1].Split(new string[] { "<div class=\"infotext\"><p>" }, StringSplitOptions.None);
                                    string[] ise2 = ise1[1].Split(new string[] { "</p></div>" }, StringSplitOptions.None);
                                    ise2[0] = HttpUtility.HtmlDecode(ise2[0]);
                                    worksheet.Cells[satir, sutun].Value = ise2[0];
                                }
                                else
                                    worksheet.Cells[satir, sutun].Value = "";
                                sutun++;
                            }






                        }
                    }
                }
                package.Save();
            }
            MessageBox.Show("Bitti");
        }
        void getVitamin()
        {
            List<string> pages = new List<string>(){"http://www.vitaminler.com/c/vitaminler-63","http://www.vitaminler.com/c/glukozamin-ve-eklem-68","http://www.vitaminler.com/c/omega-3-ve-balik-yaglari-69",
            "http://www.vitaminler.com/c/bitkisel-urunler-70","http://www.vitaminler.com/c/antioksidan-71","http://www.vitaminler.com/c/mineraller-72","http://www.vitaminler.com/c/sindirim-probiyotik-73","http://www.vitaminler.com/c/diger-takviyeler-74"};
            int satir = 1, sutun = 1;
            var fileName = "vitamin.xlsx";
            var file = new FileInfo(Application.StartupPath + "\\" + fileName);
            using (var package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sayfa1");
                worksheet.Cells[1, 1].Value = "Adı";
                worksheet.Cells[1, 2].Value = "Kategorisi";
                worksheet.Cells[1, 3].Value = "Resmi";
                worksheet.Cells[1, 4].Value = "Fiyatı";
                worksheet.Cells[1, 5].Value = "Varyantları";
                worksheet.Cells[1, 6].Value = "Özellikleri";
                worksheet.Cells[1, 7].Value = "Açıklaması";
                worksheet.Cells[1, 8].Value = "İçeriği";
                worksheet.Cells[1, 9].Value = "Yeni Başlayanlar";
                worksheet.Cells[1, 10].Value = "Orta Seviye";
                worksheet.Cells[1, 11].Value = "İleri Seviye";
                worksheet.Cells["A1:T1"].Style.Font.Bold = true;
                foreach (string pg in pages)
                {
                    int sayfa = 1;
                    while (true)
                    {
                        string src = "";
                        using (WebClient wb = new WebClient())
                        {
                            wb.Encoding = Encoding.UTF8;
                            src = wb.DownloadString(pg + "?pagenumber=" + sayfa);
                        }
                        listBox4.Items.Add(pg + "?pagenumber=" + sayfa);
                        listBox4.SelectedIndex = listBox4.Items.Count - 1;
                        listBox4.SelectedIndex = -1;
                        sayfa++;
                        if (src.Contains("Son filtreleme kriterlerlerinizle eşleşen bir ürün bulunamadı."))
                            break;

                        string[] l = src.Split(new string[] { "<div class=\"product-image\">" }, StringSplitOptions.None);
                        for (int i = 1; i < l.Length; i++)
                        {
                            satir++; sutun = 1;
                            string[] l1 = l[i].Split(new string[] { "<a href=\"" }, StringSplitOptions.None);
                            string[] l2 = l1[1].Split('"');
                            string src1 = "";
                            using (WebClient wb = new WebClient())
                            {
                                wb.Encoding = Encoding.UTF8;
                                src1 = wb.DownloadString("http://www.supplementler.com" + l2[0]);
                            }
                            listBox4.Items.Add("http://www.supplementler.com" + l2[0]);
                            listBox4.SelectedIndex = listBox4.Items.Count - 1;
                            listBox4.SelectedIndex = -1;
                            string[] adi = src1.Split(new string[] { "\"><b>" }, StringSplitOptions.None);
                            string[] adi1 = adi[1].Split(new string[] { "</b></a>" }, StringSplitOptions.None);
                            adi1[0] = HttpUtility.HtmlDecode(adi1[0]);
                            worksheet.Cells[satir, sutun].Value = adi1[0];
                            sutun++;


                            string[] cat = src1.Split(new string[] { "<div class=\"catalog_path path\" itemprop=\"breadcrumb\">" }, StringSplitOptions.None);
                            string[] cat1 = cat[1].Split(new string[] { "</div>" }, StringSplitOptions.None);
                            string catPath = "";
                            string[] cat2 = cat1[0].Split(new string[] { "\">" }, StringSplitOptions.None);
                            for (int r = 1; r < cat2.Length; r++)
                            {
                                string[] cat3 = cat2[r].Split(new string[] { "</a>" }, StringSplitOptions.None);
                                cat3[0] = cat3[0].Replace("<b>", "").Replace("</b>", "");
                                cat3[0] = HttpUtility.HtmlDecode(cat3[0]);
                                if (r != cat2.Length - 1)
                                    catPath += cat3[0] + " > ";
                                else
                                    catPath += cat3[0];
                            }

                            worksheet.Cells[satir, sutun].Value = catPath;
                            sutun++;

                            string[] img = src1.Split(new string[] { "<meta property=\"og:image\" content=\"" }, StringSplitOptions.None);
                            string[] img1 = img[1].Split('"');

                            worksheet.Cells[satir, sutun].Value = img1[0];
                            sutun++;
                            if (src1.Contains("<span class=\"productPrice\">"))
                            {
                                string[] price = src1.Split(new string[] { "<span class=\"productPrice\">" }, StringSplitOptions.None);
                                string[] price1 = price[1].Split(new string[] { "</span>" }, StringSplitOptions.None);
                                price1[0] = price1[0].Trim();
                                worksheet.Cells[satir, sutun].Value = price1[0];
                            }
                            else
                                worksheet.Cells[satir, sutun].Value = "0";
                            sutun++;

                            if (src1.Contains("<div class=\"product-options product-options"))
                            {
                                string ozellikTut = "";
                                string[] uOz = src1.Split(new string[] { "<div class=\"product-options product-options" }, StringSplitOptions.None);
                                string[] uOz1 = uOz[1].Split(new string[] { "</div>" }, StringSplitOptions.None);
                                if (uOz1[0].Contains("<span>"))
                                {
                                    string[] uOz2 = uOz1[0].Split(new string[] { "<span>" }, StringSplitOptions.None);
                                    string[] uOz3 = uOz2[1].Split(new string[] { "</span>" }, StringSplitOptions.None);
                                    //özellik adı uOz3[0];

                                    string[] uTur = uOz1[0].Split(new string[] { "<option  value=\"" }, StringSplitOptions.None);
                                    for (int j = 1; j < uTur.Length; j++)
                                    {
                                        string[] uTur1 = uTur[j].Split(new string[] { "\">" }, StringSplitOptions.None);
                                        string[] uTur2 = uTur1[1].Split(' ');
                                        uTur2[0] = HttpUtility.HtmlDecode(uTur2[0]);
                                        ozellikTut += uOz3[0] + ": " + uTur2[0];
                                    }
                                    worksheet.Cells[satir, sutun].Value = ozellikTut;
                                }
                                else
                                    worksheet.Cells[satir, sutun].Value = "";
                            }
                            else
                                worksheet.Cells[satir, sutun].Value = "";
                            sutun++;

                            if (src1.Contains("<div class=\"product-specs-module-vitamin\">"))
                            {
                                string[] specs = src1.Split(new string[] { "<div class=\"product-specs-module-vitamin\">" }, StringSplitOptions.None);
                                string[] specs1 = specs[1].Split(new string[] { "<input class" }, StringSplitOptions.None);
                                specs1[0] = HttpUtility.HtmlDecode(specs1[0]);

                                worksheet.Cells[satir, sutun].Value = specs1[0];
                            }
                            else
                                worksheet.Cells[satir, sutun].Value = "";
                            sutun++;
                            if (src1.Contains("<div class=\"infotext\" itemprop=\"description\">"))
                            {
                                string[] desc = src1.Split(new string[] { "<div class=\"infotext\" itemprop=\"description\">" }, StringSplitOptions.None);
                                string[] desc1 = desc[1].Split(new string[] { "</div>" }, StringSplitOptions.None);
                                desc1[0] = HttpUtility.HtmlDecode(desc1[0]);

                                worksheet.Cells[satir, sutun].Value = desc1[0];
                            }
                            else
                                worksheet.Cells[satir, sutun].Value = "";
                            sutun++;

                            if (src1.Contains("<div class=\"nutritionLabel \" style=\'cursor:pointer\' title=\'Ürün içeriğini görmek için tıklayınız\' itemprop=\"nutrition\" itemscope itemtype=\"http://schema.org/NutritionInformation\">"))
                            {
                                string[] itemProp = src1.Split(new string[] { "<div class=\"nutritionLabel \" style=\'cursor:pointer\' title=\'Ürün içeriğini görmek için tıklayınız\' itemprop=\"nutrition\" itemscope itemtype=\"http://schema.org/NutritionInformation\">" }, StringSplitOptions.None);
                                string[] itemProp1 = itemProp[1].Split(new string[] { "<div class=\"clear\"></div>" }, StringSplitOptions.None);
                                worksheet.Cells[satir, sutun].Value = itemProp1[0];
                            }
                            else
                                worksheet.Cells[satir, sutun].Value = "";
                            sutun++;

                            if (src1.Contains("<div class=\"infotext\"><p>"))
                            {
                                string[] items = src1.Split(new string[] { "<div class=\"infotext\"><p>" }, StringSplitOptions.None);
                                if (src1.Contains("tabitem1"))
                                {
                                    string[] yba = src1.Split(new string[] { "<div id=\"tabitem1\" class=\"inv-tab-content-small\">" }, StringSplitOptions.None);
                                    string[] yba1 = yba[1].Split(new string[] { "<div class=\"infotext\"><p>" }, StringSplitOptions.None);
                                    string[] yba2 = yba1[1].Split(new string[] { "</p></div>" }, StringSplitOptions.None);
                                    yba2[0] = HttpUtility.HtmlDecode(yba2[0]);
                                    worksheet.Cells[satir, sutun].Value = yba2[0];
                                }
                                else
                                    worksheet.Cells[satir, sutun].Value = "";
                                sutun++;
                                if (src1.Contains("tabitem2"))
                                {
                                    string[] ose = src1.Split(new string[] { "<div id=\"tabitem2\" class=\"inv-tab-content-small\">" }, StringSplitOptions.None);
                                    string[] ose1 = ose[1].Split(new string[] { "<div class=\"infotext\"><p>" }, StringSplitOptions.None);
                                    string[] ose2 = ose1[1].Split(new string[] { "</p></div>" }, StringSplitOptions.None);
                                    ose2[0] = HttpUtility.HtmlDecode(ose2[0]);
                                    worksheet.Cells[satir, sutun].Value = ose2[0];
                                }
                                else
                                    worksheet.Cells[satir, sutun].Value = "";
                                sutun++;
                                if (src1.Contains("tabitem3"))
                                {
                                    string[] ise = src1.Split(new string[] { "<div id=\"tabitem3\" class=\"inv-tab-content-small\">" }, StringSplitOptions.None);
                                    string[] ise1 = ise[1].Split(new string[] { "<div class=\"infotext\"><p>" }, StringSplitOptions.None);
                                    string[] ise2 = ise1[1].Split(new string[] { "</p></div>" }, StringSplitOptions.None);
                                    ise2[0] = HttpUtility.HtmlDecode(ise2[0]);
                                    worksheet.Cells[satir, sutun].Value = ise2[0];
                                }
                                else
                                    worksheet.Cells[satir, sutun].Value = "";
                                sutun++;
                            }






                        }
                    }
                }
                package.Save();
            }
            MessageBox.Show("Bitti");
        }

        private void button12_Click(object sender, EventArgs e)
        {
            string all = "";
            string a = "<table class=\"List\"><tr><td><a href=\"/kategori/4b-ilanlari/?Expire=false\">4/B İlanları</a></td></tr><tr><td><a href=\"/kategori/akademik-ilanlar/?Expire=false\">Akademik İlanlar</a></td></tr><tr><td><a href=\"/kategori/askeri-ilanlar/?Expire=false\">Askeri İlanlar</a></td></tr><tr><td><a href=\"/kategori/daimi-isci-ilanlari/?Expire=false\">Daimi İşçi İlanları</a></td></tr><tr><td><a href=\"/kategori/engelli-ilanlari/?Expire=false\">Engelli İlanları</a></td></tr><tr><td><a href=\"/kategori/eski-hukumlu/?Expire=false\">Eski Hükümlü</a></td></tr><tr><td><a href=\"/kategori/gecici-isci-ilanlari/?Expire=false\">Geçici İşçi İlanları</a></td></tr><tr><td><a href=\"/kategori/genel-ilanlar/?Expire=false\">Genel İlanlar</a></td></tr><tr><td><a href=\"/kategori/lakinma-ajansi/?Expire=false\">Kalkınma Ajansı</a></td></tr><tr><td><a href=\"/kategori/kpss-a-ilanlari/?Expire=false\">KPSS-A İlanları</a></td></tr><tr><td><a href=\"/kategori/kpss-b-ilanlari/?Expire=false\">KPSS-B İlanları</a></td></tr><tr><td><a href=\"/kategori/mahalli-ilanlari/?Expire=false\">Mahalli İlanlar</a></td></tr><tr><td><a href=\"/kategori/ogrenci-alim-ilanlari/?Expire=false\">Öğrenci Alım İlanları</a></td></tr><tr><td><a href=\"/kategori/sinav-sonuclari/?Expire=false\">Sınav Sonuçları</a></td></tr><tr><td><a href=\"/kategori/sydv-ilanlari/?Expire=false\">SYDV İlanları</a></td></tr><tr><td><a href=\"/kategori/terorle-mucadelede-yaralanan-isci/?Expire=false\">Terörle Mücadelede Yaralanan İşçi</a></td></tr><tr><td><a href=\"/kategori/yuksek-lisans-doktora/?Expire=false\">Y. Lisans ve Doktora</a></td></tr><tr><td><a href=\"/kategori/yatay-gecis-ilanlari/?Expire=false\">Yatay Geçiş İlanları</a></td></tr><tr><td><a href=\"/kategori/yok-duyurulari/?Expire=false\">YÖK Duyuruları</a></td></tr><tr><td><a href=\"/kategori/yurt-disi-burs-prog-ilanlari/?Expire=false\">Yurt Dışı Burs/Prog. İlanları</a></td></tr></table>";
            string[] b = a.Split(new string[] { "?Expire=false\">" }, StringSplitOptions.None);
            for(int i = 1;i<b.Length;i++)
            {
                string[] c = b[i].Split('<');
                all+="insert into kategoriler values (null,'"+c[0]+"');"+Environment.NewLine;
            }
            Clipboard.SetText(all);
        }
    }
}
