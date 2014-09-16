using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Data.OleDb;
using Google.YouTube;
using Google.GData.Client;
using Google.GData.YouTube;
using Google.GData.Extensions.MediaRss;
using System.Xml;
using System.Diagnostics;
using System.Threading;
using YoutubeExtractor;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace new_upload
{
    public partial class Form1 : Form
    {
        const string HTML_TAG_PATTERN = "<.*?>";

        public int sira = 0;
        public int err = 0;

        string kimde = "";
        string videoadi, aciklama, etiket, developerkey, hesapadi, hesapsifresi;
        int funnyplacelimit = 25, killsometimelimit = 25, videobashlimit = 25, stupidvideoslimit = 25, dpccarslimit = 25;
        int saclimit = 25, australialimit = 25, tvuolimit = 25, alkislimit = 25, russiaprochanlimit = 25, ochevidetslimit = 25, yapfileslimit = 25, videogelimit = 25;
        bool oto = false;
        int discloselimit = 25, liveleaklimit = 25, vidmaxlimit = 25, euronewslimit = 25;

        List<string> country = new List<string>() { "AR","AU","AT","BE","BR","CA","CL","CO","CZ","EG","FR","DE","GB","HK","HU","IN","IE","IL","IT","JP","JO","MY","MX","MA","NL","NZ","PE","PH","PL","RU","SA","SG","ZA","KR","ES","SE","CH","TW","AE","US" };
        List<string> kind = new List<string>() {"Music","Animals","Sports","Travel","Games","People","Entertainment","News","Howto","Education","Tech","Nonprofit","Movies" };
        
        string knt_hadi, knt_hsifresi, knt_devkey, knt_kadi;

        List<string> videos = new List<string>();
        List<string> lst4 = new List<string>();
        int sec = 5;
        int deleted;

        string videoismi;

        public Form1()
        {
            InitializeComponent();
        }
        static string StripHTML(string inputString)
        {
            return Regex.Replace
              (inputString, HTML_TAG_PATTERN, string.Empty);
        }
        private void Ekle(string l)
        {
            using (OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=link.mdb"))
            {
                conn.Open();
                OleDbCommand sorgu = new OleDbCommand("insert into link(video) values('" + l + "')", conn);
                sorgu.ExecuteNonQuery();
            }
        }
        private bool Kontrol(string l)
        {
            using (OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=link.mdb"))
            {
                conn.Open();
                OleDbCommand sorgu = new OleDbCommand("select Count(video) from link where video ='" + l + "'", conn);
                if (Convert.ToInt16(sorgu.ExecuteScalar()) > 0)
                    return false;
                else
                    return true;
            }
        }
        private void Etiket(string ad)//Etiket uzunlukları 500 byte olucak sanırım. Kısalttığımda düzeldi.
        {
            etiket = "";
            char[] add = ad.ToCharArray();
            List<char> addd = new List<char>();
            foreach (char a in add)
                if ((a >= 33 && a <= 47) || (a >= 58 && a <= 64) || (a >= 91 && a <= 96) || (a >= 123 && a <= 126)) { }
                else
                    addd.Add(a);
            ad = "";
            foreach (char a in addd)
                ad += a;

            string[] q = ad.Split(' ');
            foreach (string b in q)
            {
                if (b.Length > 2)
                    etiket += b + ",";
            }
            /*for (int ke = 0; ke < q.Length; ke++)
            {
                for (int le = 0; le < q.Length; le++)
                {
                    if (!q[ke].Equals(q[le]) && q[ke].Length > 2)
                    {
                        etiket += q[le] + " " + q[ke] + ",";
                    }
                }
            }*/
            if (etiket[0] == ',')
                etiket.Remove(0, 1);
            if (etiket[etiket.Length - 1] == ',')
                etiket.Remove(etiket.Length - 1, 1);
        }
        private string EtiketUret(string ad)
        {
            string etiket = "";
            string[] q = ad.Split(' ');
            foreach (string b in q)
            {
                if (b.Length > 2)
                    etiket += b + ",";
            }
            for (int ke = 0; ke < q.Length; ke++)
            {
                for (int le = 0; le < q.Length; le++)
                {
                    if (!q[ke].Equals(q[le]) && q[ke].Length > 2)
                    {
                        etiket += q[le] + " " + q[ke] + ",";
                    }
                }
            }
            return etiket;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult sonuc;
            Thread a = new Thread(new ThreadStart(Sina));
            sonuc = MessageBox.Show("Başlasın mı?", "Uyarı", MessageBoxButtons.OKCancel);
            if (sonuc == DialogResult.OK)
            {
                a.Start();
            }
        }
        private void Sina()
        {
            List<string> links = new List<string>();
            int i = 1;
            while (i <= 5)
            {
                string a = GetSource("http://video.sina.com/v/video.php?page="+i.ToString()+"&class=5&channel=69&category=mr");
                if (a == "")
                    continue;
                i++;
                string[] a1 = a.Split(new string[]{"<li style=\"width: 150px;\">"},StringSplitOptions.None);
                for (int j = 1; j < a1.Length; j++)
                {
                    string[] a2 = a1[j].Split(new string[] { "<a href=\"" }, StringSplitOptions.None);
                    string[] a3 = a2[1].Split('"');
                    links.Add(a3[0]);  
                }
            }
            for(int k = 1;k<links.Count;k++)
            {
                this.Text = k + "/" + links.Count;
                string a4 = GetSource(links[k]);
                if (a4.Contains("vkey:\""))
                {
                    string[] lnk = a4.Split(new string[] { "vkey:\"" }, StringSplitOptions.None);
                    string[] lnk1 = lnk[1].Split('"');
                    string fSize = getSize("http://video.sina.com/v/flvideo/" + lnk1[0] + ".flv", 1);
                    if (fSize == "")
                    {
                        lnk1[0] = lnk1[0] + "_0";
                        fSize = getSize("http://video.sina.com/v/flvideo/" + lnk1[0] + ".flv", 1);
                    }

                    string tags = "";
                    string[] tg = a4.Split(new string[] { "<div class=\"vInfos_tag\">" }, StringSplitOptions.None);
                    string[] tg1 = tg[1].Split(new string[] { "</div>" }, StringSplitOptions.None);
                    string[] tg2 = tg1[0].Split(new string[] { "\">" }, StringSplitOptions.None);
                    for (int m = 1; m < tg2.Length; m++)
                    {
                        string[] tg3 = tg2[m].Split('<');
                        tags += tg3[0] + ",";
                    }

                    string[] dc = a4.Split(new string[] { "<div class=\"vInfos_text\">" }, StringSplitOptions.None);
                    string[] dc1 = dc[1].Split(new string[] { "</div>" }, StringSplitOptions.None);

                    string[] tt = a4.Split(new string[] { "title:\"" }, StringSplitOptions.None);
                    string[] tt1 = tt[1].Split('"');

                    richTextBox5.Text += tt1[0] + Environment.NewLine + dc1[0] + Environment.NewLine + tags + Environment.NewLine + "Link: "+ lnk1[0] + ".flv Size: " + fSize + Environment.NewLine + Environment.NewLine;
                }
            }
            MessageBox.Show("Bitti.");

        }
        private string GetSourceLongWay(string url)
        {
            using (var client = new WebClient())
            {
                client.Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/34.0.1847.131 Safari/537.36";
                client.Headers["Accept"] = "*/*";
                client.Headers["Accept-Language"] = "tr-TR,tr;q=0.8,en-US;q=0.6,en;q=0.4";
                client.Headers["Accept-Encoding"] = "gzip,deflate,sdch";
                //client.Headers["Accept-Charset"] = "ISO-8859-1,utf-8;q=0.7,*;q=0.7";
                client.Encoding = Encoding.UTF8;
                return client.DownloadString(url);
            }
        }
        private string GetSourceWebClient(string url)
        {
            try
            {
                using (var client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/34.0.1847.131 Safari/537.36";
                    return client.DownloadString(url);
                }
            }
            catch { return ""; }
        }
        private bool VarMi(string link)
        {
            bool b = true;
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(link);
                req.Method = "HEAD";
                HttpWebResponse resp = (HttpWebResponse)(req.GetResponse());
            }
            catch { b = false; }
            return b;
            
        }
        private long boyutBul(string link)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(link);
            req.Method = "HEAD";
            HttpWebResponse resp = (HttpWebResponse)(req.GetResponse());
            long len = resp.ContentLength;
            return (len / 1024 / 1024);
        }
        private string getSize(string link,int Show)
        {
            try
            {
                if (Show == 1)
                    richTextBox1.Text += link + Environment.NewLine;
                System.Net.WebRequest req = System.Net.HttpWebRequest.Create(link);
                req.Method = "HEAD";
                System.Net.WebResponse resp = req.GetResponse();
                long ContentLength = 0;
                long result;
                string File_Size = "";
                string ft = "";
                if (long.TryParse(resp.Headers.Get("Content-Length"), out ContentLength))
                {


                    if (ContentLength >= 1073741824)
                    {
                        result = ContentLength / 1073741824;
                        ft = "GB";
                    }
                    else if (ContentLength >= 1048576)
                    {
                        result = ContentLength / 1048576;
                        ft = "MB";
                    }
                    else
                    {
                        result = ContentLength / 1024;
                        ft = "KB";
                    }
                    File_Size += result.ToString("0.00");
                    File_Size += " " + ft;
                }
                return File_Size;
            }
            catch { return ""; }
        }
        #region Siteler
        private void FunnyPlaceIndexWeekly()
        {
            kimde = "funnyplace";
            developerkey = "AI39si5qVzWdGpKXbC24f1foGTaJyO9_1rmGyCW-e31ZaeNoOtUwz1RlWwqP2QXQs3FhIKgdx7DDKMLrrMnalzaUl_qhKUrhIA";
            hesapadi = "Ringe1965@gmail.com";
            hesapsifresi = "6dJzthRX";
            try
            {
                listBox1.Items.Add(DateTime.Now.ToString() + " FunnyPlace");
                List<string> f = new List<string>();
                string s1 = GetSource("http://www.funnyplace.org/");
                string[] a = s1.Split(new string[] { "<li><a href=\"" }, StringSplitOptions.None);
                for (int i = 1; i < a.Length; i++)
                {
                    string[] b = a[i].Split('"');
                    b[0] = "http://www.funnyplace.org" + b[0];
                    f.Add(b[0]);
                }
                foreach (string s in f)
                {
                    s1 = GetSource(s);
                    if (funnyplacelimit > 0 && Kontrol(s)==true)
                    {
                        string[] title = s1.Split(new string[] { "<h1>" }, StringSplitOptions.None);
                        string[] title1 = title[1].Split(new string[] { "</h1>" }, StringSplitOptions.None);
                        videoadi = title1[0];
                        string[] desc = s1.Split(new string[] { "<!-- AddThis Button END -->" }, StringSplitOptions.None);
                        string[] desc1 = desc[1].Split(new string[] { "</div>" }, StringSplitOptions.None);
                        desc1[0] = StripHTML(desc1[0]);
                        desc1[0] = HttpUtility.HtmlDecode(desc1[0].Trim());
                        aciklama = desc1[0];
                        string[] link = s1.Split(new string[] { "'file': '" }, StringSplitOptions.None);
                        string[] link1 = link[1].Split('\'');
                        Etiket(title1[0]);
                        richTextBox1.Text += videoadi + Environment.NewLine + aciklama + Environment.NewLine + etiket + Environment.NewLine+link1[0]+Environment.NewLine;
                        /*VideoDownload(link1[0],"funnyplace");
                        YoutubeUpload();
                        VideoDelete();
                        Ekle(s);
                        funnyplacelimit--;*/
                    }
                }
            }
            catch (Exception ex) { richTextBox2.Text += ex.ToString(); AppendText(richTextBox1, Color.Red, "FunnyPlace Hatası."); }
            AppendText(richTextBox1, Color.Red, "FunnyPlace Bitti.");
        }//OK / AYNI VİDEOLAR. BUNU DEĞİŞTİRİCEZ.
        private void AlkislarlaYasiyorumLatest()
        {
            kimde = "alkışlar";
            developerkey = "AI39si7t4DuFEIL2z0lIvrAsvonxIXzPuMiVxRziWQa9x5LGiApdCzJey0nPm3blUonhNHmNMc7YepyBTExsMV4nLOY3HnbGig";
            hesapadi = "rhysowens48@gmail.com";
            hesapsifresi = "6528yayla";

            //try
            //{
            listBox1.Items.Add(DateTime.Now.ToString() + " AlkışlarlaYaşıyorum");
            List<string> f = new List<string>();
            string s1 = GetSourceLongWay("http://www.alkislarlayasiyorum.com/");
            richTextBox1.Text = s1;
            string[] a = s1.Split(new string[] { "<a class=\"cont-img-a fl\" href=\"" }, StringSplitOptions.None);
            for (int i = 1; i < a.Length; i++)
            {
                string[] d = a[i].Split('"');
                f.Add(d[0]);
                listBox1.Items.Add(d[0]);
            }

            foreach (string s in f)//açıklamadan sonrasını al
            {
                if (alkislimit > 0 && Kontrol(s)==true)
                {
                    s1 = GetSourceLongWay(s);
                    string[] title = s1.Split(new string[] { "<h1 class=\"title fl\" itemprop=\"name\">" }, StringSplitOptions.None);
                    string[] title1 = title[1].Split('<');
                    videoadi = title1[0];
                    string[] desc = s1.Split(new string[] { "<p class=\"info\" id=\"content_desc_linkify\" >" }, StringSplitOptions.None);
                    string[] desc1 = desc[1].Split(new string[] { "</div>" }, StringSplitOptions.None);
                    desc1[0] = StripHTML(desc1[0]);
                    desc1[0] = HttpUtility.HtmlDecode(desc1[0].Trim());
                    aciklama = desc1[0];
                    string[] link = s1.Split(new string[] { "streamurl:\"" }, StringSplitOptions.None);
                    string[] link1 = link[1].Split('"');
                    Etiket(videoadi);

                    richTextBox1.Text += videoadi + Environment.NewLine + aciklama + Environment.NewLine + etiket + Environment.NewLine + link1[0] + Environment.NewLine;
                    /*VideoDownload(link1[0], "alkis");
                    YoutubeUpload();
                    VideoDelete();
                    Ekle(s);
                    alkislimit--;*/
                }
            }
            //}
            //catch (Exception ex) { richTextBox2.Text += ex.ToString(); AppendText(richTextBox1, Color.Red, "AlkislarlaYasiyorum Hatası."); }
            AppendText(richTextBox1, Color.Red, "Alkış Bitti.");
        }//OK - OLMUYO SİLECEZ.


        private void KillSomeTimeLatestFunnyVideos()
        {
            kimde = "killsometime";
            developerkey = "AI39si6VigB0NVukSTQnzfFnvrZurXNIKRIQUmorzJ3Qlwhhwb4XE67YV74tqqL6YEvsmJEnDvEEntY4cAznvfhJqQ6jfocucw";
            hesapadi = "markourner4@gmail.com";
            hesapsifresi = "LbwPMvMR";
            try
            {
                listBox1.Items.Add(DateTime.Now.ToString() + " KillSomeTime");
                List<string> f = new List<string>();
                string s1 = GetSource("http://www.killsometime.com/home/1");
                string[] a = s1.Split(new string[] { "<article class=\"Homepage-Article-WhatsNew\">" }, StringSplitOptions.None);
                for (int i = 1; i < a.Length; i++)
                {
                    string[] b = a[i].Split(new string[] { "<a class=\"Article\" href=\"" }, StringSplitOptions.None);
                    string[] l = b[1].Split('"');
                    l[0] = "http://www.killsometime.com" + l[0];
                    f.Add(l[0]);
                }
                s1 = GetSource("http://www.killsometime.com/home/2");
                string[] aa11 = s1.Split(new string[] { "<article class=\"Homepage-Article-WhatsNew\">" }, StringSplitOptions.None);
                for (int i = 1; i < aa11.Length; i++)
                {
                    string[] b = aa11[i].Split(new string[] { "<a class=\"Article\" href=\"" }, StringSplitOptions.None);
                    string[] l = b[1].Split('"');
                    l[0] = "http://www.killsometime.com" + l[0];
                    f.Add(l[0]);
                }
                foreach (string s in f)
                {
                    s1 = GetSource(s);
                    //if (killsometimelimit > 0 && Kontrol(s)==true)
                    //{
                        if (s.Contains("http://www.killsometime.com/videos/"))
                        {
                            string[] title = s1.Split(new string[] { "<meta property=\"og:title\" content=\"" }, StringSplitOptions.None);
                            string[] title1 = title[1].Split('"');
                            videoadi = HttpUtility.HtmlDecode(title1[0]);
                            string[] desc = s1.Split(new string[] { "<meta property=\"og:description\" content='" }, StringSplitOptions.None);
                            string[] desc1 = desc[1].Split('\'');
                            desc1[0] = StripHTML(desc1[0]);
                            desc1[0] = HttpUtility.HtmlDecode(desc1[0].Trim());
                            aciklama = desc1[0];
                            string[] link = s1.Split(new string[] { "file: '" }, StringSplitOptions.None);
                            string[] link1 = link[1].Split('\'');
                            link1[0] = "http://www.killsometime.com" + link1[0];
                            Etiket(videoadi);
                            /*VideoDownload(link1[0], "killsometime");
                            YoutubeUpload();
                            VideoDelete();
                            Ekle(s);
                            killsometimelimit--;*/
                        }
                    //}
                }
            }
            catch (Exception ex) { richTextBox2.Text += ex.ToString(); AppendText(richTextBox1, Color.Red, "KillSomeTime Hatası."); }
            AppendText(richTextBox1, Color.Red, "KillSomeTime Bitti.");
        }//OK
        private void VideoBashDailyMostViewedVideos()//OK
        {
            kimde = "videobash";
            developerkey = "AI39si6MEPAf3yCxBbapRXjBYSUoVYbaI6o-ebhcBh2kA4xLfTrUQfXPSRKbySWMV9krm8sbBt9tH0xQ906BGBX3YTtwzTWYDQ";
            hesapadi = "joshhogson@gmail.com";
            hesapsifresi = "6528yayla";
            try
            {
                listBox1.Items.Add(DateTime.Now.ToString() + " VideoBash");
                List<string> f = new List<string>();
                string s1 = GetSource("http://www.videobash.com/videos");
                string[] a = s1.Split(new string[] { "<div class=\"info-video-wrapper\">" }, StringSplitOptions.None);
                for (int i = 1; i < a.Length; i++)
                {
                    string[] link = a[i].Split(new string[] { "<h3><a href=\"" }, StringSplitOptions.None);
                    string[] link1 = link[1].Split('"');
                    f.Add(link1[0]);
                }
                foreach (string s in f)
                {
                    if (videobashlimit > 0 && Kontrol(s)==true)
                    {
                        s1 = GetSource(s);
                        if (s1.Contains("'http://'"))
                        {
                            string[] cut = s1.Split(new string[] { "'http://' + '" }, StringSplitOptions.None);
                            string[] cut2 = cut[1].Split(new string[] { "';" }, StringSplitOptions.None);
                            cut2[0] = cut2[0].Replace("%2F", "/").Replace("%3F", "?").Replace("%3D", "=").Replace("%26", "&").Replace("%25", "%").Replace("%26", "=");
                            cut2[0] = "http://" + cut2[0]+"&start=0";
                            string[] title = s1.Split(new string[] { "<meta property=\"og:title\" content=\"" }, StringSplitOptions.None);
                            string[] title1 = title[1].Split('"');
                            videoadi = title1[0];
                            string[] desc = s1.Split(new string[] { "<meta property=\"og:description\" content=\"" }, StringSplitOptions.None);
                            string[] desc1 = desc[1].Split('"');
                            desc1[0] = StripHTML(desc1[0]);
                            desc1[0] = HttpUtility.HtmlDecode(desc1[0].Trim());
                            aciklama = desc1[0];
                            Etiket(videoadi);
                            VideoDownload(cut2[0], "videobash");
                            YoutubeUpload();
                            VideoDelete();
                            Ekle(s);
                            videobashlimit--;
                        }
                    }
                }
            }
            catch (Exception ex) { richTextBox2.Text += ex.ToString(); AppendText(richTextBox1, Color.Red, "VideoBash Hatası."); };
            AppendText(richTextBox1, Color.Red, "VideoBash Bitti.");
        }
        private void LiveLeakLatest()
        {
            kimde = "liveleak";
            hesapadi = dataGridView1.Rows[0].Cells[1].Value.ToString();
            hesapsifresi = dataGridView1.Rows[0].Cells[2].Value.ToString();
            developerkey = dataGridView1.Rows[0].Cells[3].Value.ToString();
            //try
            //{
                listBox1.Items.Add(DateTime.Now.ToString() + " Liveleak Latest");
                List<string> f = new List<string>();
                string s1 = GetSource("http://www.liveleak.com/");
                string[] a = s1.Split(new string[] { "id=\"favorite-" }, StringSplitOptions.None);
                for (int i = 1; i < a.Length; i++)
                {
                    string[] b = a[i].Split('"');
                    f.Add("http://www.liveleak.com/view?i=" + b[0]);
                }
                foreach (string s in f)
                {
                    s1 = GetSource(s);
                    if (Kontrol(s)==true && s1.Contains("file: \""))
                    {
                        string[] cut = s1.Split(new string[] { "file: \"" }, StringSplitOptions.None);
                        string[] cut2 = cut[1].Split(new string[] { "\"," }, StringSplitOptions.None);
                        string[] title = s1.Split(new string[] { "style=\"vertical-align:top; padding-right:10px\">" }, StringSplitOptions.None);
                        string[] title2 = title[1].Split('<');
                        string[] desc = s1.Split(new string[] { "<div id=\"body_text\"><p>" }, StringSplitOptions.None);
                        string[] desc2 = desc[1].Split(new[] { "<style>" }, StringSplitOptions.None);
                        cut2[0] = cut2[0] + "&ec_seek=0";
                        videoadi = HttpUtility.HtmlDecode(title2[0]);
                        aciklama = StripHTML(HttpUtility.HtmlDecode(desc2[0])).Trim();
                        if (cut[0].Contains("rmtp"))
                            continue;
                        Etiket(videoadi);
                        richTextBox2.Text += videoadi + Environment.NewLine + aciklama + Environment.NewLine + etiket + Environment.NewLine + cut2[0] + Environment.NewLine;
                        VideoDownload(cut2[0], "liveleak");
                        YoutubeUpload();
                        VideoDelete();
                        Ekle(s);
                        liveleaklimit--;
                    }
                }
            //}
            //catch (Exception ex) { richTextBox2.Text += ex.ToString(); AppendText(richTextBox1, Color.Red, "LiveLeak Hatası."); }
            AppendText(richTextBox1, Color.Red, "LiveLeak Bitti.");
        }//HATA VERIYO BU ELEMAN DA SÜREKLİ.
        private void VidMaxLatest()
        {
            kimde = "vidmax";
            developerkey = "AI39si6sAkzxHuTte-nu9N2NpBMnP0HtQ2tcrIlFg2n-HFDNKqyBzy8DWGY-5EWy2biKIYC2YhSatJ0mKkJLhHnN0gjF8XVswA";
            hesapadi = "lucagodfrey@gmail.com";
            hesapsifresi = "6528yayla";
            try
            {
                listBox1.Items.Add(DateTime.Now.ToString() + " VidMax Latest");
                List<string> f = new List<string>();
                string s1 = GetSource("http://vidmax.com/videos");
                string[] a = s1.Split(new string[] { "<h2><a href=\"" }, StringSplitOptions.None);
                for (int i = 1; i < a.Length; i++)
                {
                    string[] b = a[i].Split('"');
                    f.Add("http://www.vidmax.com" + b[0]);
                }
                foreach (string s in f)
                {
                    if (vidmaxlimit > 0 && Kontrol(s)==true)
                    {
                        s1 = GetSource(s);
                        if (s.Contains("nsfw/lockedBig.png"))
                            continue;
                        string[] cut = s1.Split(new string[] { "&file=" }, StringSplitOptions.None);
                        string[] cut2 = cut[1].Split('&');
                        string[] title = s1.Split(new string[] { "<h1>" }, StringSplitOptions.None);
                        string[] title2 = title[1].Split(new string[] { "</h1>" }, StringSplitOptions.None);
                        string[] desc = s1.Split(new string[] { "<meta name=\"description\" content=\"" }, StringSplitOptions.None);
                        string[] desc2 = desc[1].Split('"');
                        cut2[0] = cut2[0] + "?start=0&id=player&client=FLASH%20WIN%2011,9,900,117&version=4.2.95&width=475";
                        videoadi = HttpUtility.HtmlDecode(title2[0]);
                        aciklama = HttpUtility.HtmlDecode(desc2[0]);
                        Etiket(videoadi);
                        richTextBox2.Text += videoadi + Environment.NewLine + aciklama + Environment.NewLine + etiket + Environment.NewLine + cut2[0] + Environment.NewLine;
                        /*VideoDownload(cut2[0], "vidmax");
                        YoutubeUpload();
                        VideoDelete();
                        Ekle(s);
                        vidmaxlimit--;*/
                    }
                }
            }
            catch (Exception ex) { richTextBox2.Text += ex.ToString(); AppendText(richTextBox1, Color.Red, "VidMax Hatası."); }
            AppendText(richTextBox1, Color.Red, "VidMax Bitti.");
        }//bunu napsam bilemedim. logolu tüm videolar.
        private void EuroNewsLatest()
        {
            string vlink;
            kimde = "euronews";
            hesapadi = dataGridView1.Rows[0].Cells[1].Value.ToString();
            hesapsifresi = dataGridView1.Rows[0].Cells[2].Value.ToString();
            developerkey = dataGridView1.Rows[0].Cells[3].Value.ToString();
            try
            {
                listBox1.Items.Add(DateTime.Now.ToString() + " EuroNews Latest");
                List<string> f = new List<string>();
                string s1 = GetSource("http://feeds.feedburner.com/euronews/en/home/");
                string[] a = s1.Split(new string[] { "<link>" }, StringSplitOptions.None);
                for (int i = 1; i < a.Length; i++)
                {
                    if (a[i].Contains("feedproxy.google.com"))
                    {
                        string[] b = a[i].Split(new string[] { "</link>" }, StringSplitOptions.None);
                        f.Add(b[0]);
                    }
                }
                foreach (string s in f)
                {
                    if (euronewslimit > 0 && Kontrol(s)==true)
                    {
                        s1 = GetSource(s);
                        if (s1.Contains("file: \""))
                        {
                            string[] cut = s1.Split(new string[] { "file: \"" }, StringSplitOptions.None);
                            string[] cut2 = cut[1].Split('"');
                            string[] title = s1.Split(new string[] { "<meta property=\"og:title\" content=\"" }, StringSplitOptions.None);
                            string[] title2 = title[1].Split('"');
                            string[] desc = s1.Split(new string[] { "<div id='articleTranscript'>" }, StringSplitOptions.None);
                            string[] desc2 = desc[1].Split(new string[] { "</div>" }, StringSplitOptions.None);
                            videoadi = StripHTML(HttpUtility.HtmlDecode(title2[0])).Trim();
                            aciklama = StripHTML(HttpUtility.HtmlDecode(desc2[0])).Trim();
                            vlink = cut2[0];
                            Etiket(videoadi);
                            richTextBox1.Text += videoadi + Environment.NewLine + aciklama + Environment.NewLine + etiket + Environment.NewLine + vlink + Environment.NewLine;
                            VideoDownload(vlink, "euronews");
                            YoutubeUpload();
                            VideoDelete();
                            Ekle(s);
                            euronewslimit--;
                        }
                    }
                }
            }
            catch (Exception ex) { richTextBox2.Text += ex.ToString(); AppendText(richTextBox1, Color.Red, "EuroNews Hatası."); }
            AppendText(richTextBox1, Color.Red, "EuroNews Bitti.");
        }//OK 22.05.2014
        private void AustraliaSmhLatest()
        {
            kimde = "smh";
            hesapadi = dataGridView1.Rows[1].Cells[1].Value.ToString();
            hesapsifresi = dataGridView1.Rows[1].Cells[2].Value.ToString();
            developerkey = dataGridView1.Rows[1].Cells[3].Value.ToString();
            knt_kadi = dataGridView1.Rows[1].Cells[4].Value.ToString();

            try
            {
                listBox1.Items.Add(DateTime.Now.ToString() + " Smh.Com.Au");
                List<string> f = new List<string>();
                string s1 = GetSource("http://media.smh.com.au/");
                string[] a = s1.Split(new string[] { "<h3>Most Watched Videos</h3>" }, StringSplitOptions.None);
                string[] b = a[1].Split(new string[] { "</div>" }, StringSplitOptions.None);
                string[] c = b[0].Split(new string[] { "<p><a href=\"" }, StringSplitOptions.None);
                for (int i = 1; i < c.Length; i++)
                {
                    string[] d = c[i].Split('"');
                    f.Add(d[0]);
                }
                string[] aa = s1.Split(new string[] { "<h3>Featured Videos</h3>" }, StringSplitOptions.None);
                string[] bb = aa[1].Split(new string[] { "</div>" }, StringSplitOptions.None);
                string[] cc = bb[0].Split(new string[] { "<p><a href=\"" }, StringSplitOptions.None);
                for (int i = 1; i < cc.Length; i++)
                {
                    string[] d = cc[i].Split('"');
                    f.Add(d[0]);
                }
                string[] aaa = s1.Split(new string[] { "<h3>More News</h3>" }, StringSplitOptions.None);
                string[] bbb = aaa[1].Split(new string[] { "</div>" }, StringSplitOptions.None);
                string[] ccc = bbb[0].Split(new string[] { "<p><a href=\"" }, StringSplitOptions.None);
                for (int i = 1; i < ccc.Length; i++)
                {
                    string[] d = ccc[i].Split('"');
                    f.Add(d[0]);
                }
                foreach (string s in f)
                {
                    if (australialimit > 0 && Kontrol(s)==true) 
                    {
                        s1 = GetSource(s);
                        if (!s1.Contains("\"bitrate\": \""))
                            continue;
                        string[] title = s1.Split(new string[] { "<meta property=\"og:title\" content=\"" }, StringSplitOptions.None);
                        string[] title1 = title[1].Split('"');
                        videoadi = title1[0];
                        string[] desc = s1.Split(new string[] { "<meta property=\"og:description\" content=\"" }, StringSplitOptions.None);
                        string[] desc1 = desc[1].Split('"');
                        desc1[0] = StripHTML(desc1[0]);
                        desc1[0] = HttpUtility.HtmlDecode(desc1[0].Trim());
                        aciklama = desc1[0];
                        string[] link = s1.Split(new string[] { "\"bitrate\": \"" }, StringSplitOptions.None);
                        int max = 0;
                        string l = "";
                        for (int j = 1; j < link.Length; j++)
                        {
                            string[] t = link[j].Split('"');
                            if (Convert.ToInt16(t[0]) > max)
                            {
                                max = Convert.ToInt16(t[0]);
                                string[] l2 = link[j].Split(new string[] { "\"file\": \"" }, StringSplitOptions.None);
                                string[] l1 = l2[1].Split('"');
                                l = l1[0];
                            }
                        }
                        Etiket(title1[0]);
                        richTextBox2.Text += videoadi + Environment.NewLine + aciklama + Environment.NewLine + etiket + Environment.NewLine + l + Environment.NewLine;
                        VideoDownload(l, "smh");
                        YoutubeUpload();
                        VideoDelete();
                        Ekle(s);
                        australialimit--;
                    }
                }
            }
            catch (Exception ex) { richTextBox2.Text += ex.ToString(); AppendText(richTextBox1, Color.Red, "Smh.Com.Au Hatası."); }
            AppendText(richTextBox1, Color.Red, "Australia Smh Bitti.");
        }//OK 22.05.2014
        private void RussiaProChanLatest()
        {
            kimde = "prochan russia";
            hesapadi = dataGridView1.Rows[2].Cells[1].Value.ToString();
            hesapsifresi = dataGridView1.Rows[2].Cells[2].Value.ToString();
            developerkey = dataGridView1.Rows[2].Cells[3].Value.ToString();
            knt_kadi = dataGridView1.Rows[2].Cells[4].Value.ToString();
            try
            {
                listBox1.Items.Add(DateTime.Now.ToString() + " Russia ProChan");
                List<string> f = new List<string>();
                string s1 = GetSourceWebClient("http://russia.prochan.com/");
                string[] a = s1.Split(new string[] { "<h3><a href=\"" }, StringSplitOptions.None);
                for (int i = 1; i < a.Length; i++)
                {
                    string[] b = a[i].Split('"');
                    f.Add(b[0]);
                }
                foreach (string s in f)
                {
                    if (russiaprochanlimit > 0 && Kontrol(s) == true)
                    {
                        s1 = GetSource(s);
                        if (s1.Contains("'file': '"))
                        {
                            string[] cut = s1.Split(new string[] { "'file': '" }, StringSplitOptions.None);
                            string[] cut2 = cut[1].Split('\'');
                            string[] title = s1.Split(new string[] { "title=\"display thread " }, StringSplitOptions.None);
                            string[] title2 = title[1].Split('/');
                            string ru = "";
                            using(WebClient wb = new WebClient())
                            {
                                wb.Encoding = Encoding.UTF8;
                                ru = wb.DownloadString("http://translate.yandex.net/tr.json/translate?lang=en-ru&text="+title2[0].Replace(" ","+")+"&srv=tr-text");
                            }

                            title2[0] += "/ " +ru.Replace("\"","");

                            string[] desc = s1.Split(new string[] { "<meta property=\"og:description\" content=\"" }, StringSplitOptions.None);
                            string[] desc2 = desc[1].Split('"');
                            videoadi = HttpUtility.HtmlDecode(title2[0]);
                            aciklama = HttpUtility.HtmlDecode(desc2[0]);
                            cut2[0] = cut2[0].Replace("rate=400", "seek=0");
                            Etiket(videoadi);
                            richTextBox2.Text += videoadi + Environment.NewLine + aciklama + Environment.NewLine + etiket + Environment.NewLine + cut2[0] + Environment.NewLine;
                            VideoDownload(cut2[0], "prochan");
                            YoutubeUpload();
                            VideoDelete();
                            Ekle(s);
                            russiaprochanlimit--;
                        }
                    }
                }
            }
            catch (Exception ex) { richTextBox2.Text += ex.ToString(); AppendText(richTextBox1, Color.Red, "RussiaProChan Hatası."); }
            AppendText(richTextBox1, Color.Red, "ProChan Russia Bitti.");
        }//OK 22.05.2014
        private void OchevidetsLatest()
        {
            kimde = "ochevidets";
            hesapadi = dataGridView1.Rows[3].Cells[1].Value.ToString();
            hesapsifresi = dataGridView1.Rows[3].Cells[2].Value.ToString();
            developerkey = dataGridView1.Rows[3].Cells[3].Value.ToString();
            knt_kadi = dataGridView1.Rows[3].Cells[4].Value.ToString();
            //try
            //{
                listBox1.Items.Add(DateTime.Now.ToString() + " Ochevidets");
                List<string> f = new List<string>();
                string s1 = GetSource("http://www.ochevidets.ru/videos/");
                string[] a = s1.Split(new string[] { "<div class=\"item_pic\">" }, StringSplitOptions.None);
                for (int i = 1; i < a.Length; i++)
                {
                    string[] b = a[i].Split(new string[] { "<a href=\"" }, StringSplitOptions.None);
                    string[] c = b[1].Split('"');
                    f.Add("http://www.ochevidets.ru" + c[0]);
                }
                foreach (string s in f)
                {
                    if (ochevidetslimit > 0 && Kontrol(s)==true)
                    {
                        s1 = GetSource(s);
                        if (s1.Contains("[{\"url\":\""))
                        {
                            string[] cut = s1.Split(new string[] { "[{\"url\":\"" }, StringSplitOptions.None);
                            string[] cut2 = cut[1].Split('"');
                            string[] title = s1.Split(new string[] { "<h1>" }, StringSplitOptions.None);
                            string[] title2 = title[1].Split(new string[] { "</h1>" }, StringSplitOptions.None);
                            string[] desc = s1.Split(new string[] { "description\" content=\"" }, StringSplitOptions.None);
                            string[] desc2 = desc[1].Split('"');
                            videoadi = HttpUtility.HtmlDecode(title2[0]);
                            aciklama = HttpUtility.HtmlDecode(desc2[0]);
                            Etiket(videoadi);
                            richTextBox2.Text += videoadi + Environment.NewLine + aciklama + Environment.NewLine + etiket + Environment.NewLine + cut2[0] + Environment.NewLine;
                            VideoDownload(cut2[0], "ochevidets");
                            YoutubeUpload();
                            VideoDelete();
                            Ekle(s);
                            ochevidetslimit--;
                        }
                    }
                }
            //}
            //catch (Exception ex) { richTextBox2.Text += ex.ToString(); AppendText(richTextBox1, Color.Red, "Ochevidets Hatası."); }
            AppendText(richTextBox1, Color.Red, "Ochevidets Bitti.");
        }//OK 22.05.2014
        private void YapFilesLatest()
        {
            kimde = "yapfiles";
            hesapadi = dataGridView1.Rows[4].Cells[1].Value.ToString();
            hesapsifresi = dataGridView1.Rows[4].Cells[2].Value.ToString();
            developerkey = dataGridView1.Rows[4].Cells[3].Value.ToString();
            knt_kadi = dataGridView1.Rows[4].Cells[4].Value.ToString();
            try
            {
                listBox1.Items.Add(DateTime.Now.ToString() + " YapFiles");
                List<string> f = new List<string>();
                string s1 = GetSource("http://www.yapfiles.ru/cat/1/sort_by_new/page/1/");
                string[] a = s1.Split(new string[] { "<div style=\"width: 136px; height: 30px; line-height: 15px; overflow: hidden;\">" }, StringSplitOptions.None);
                for (int i = 1; i < a.Length; i++)
                {
                    string[] b = a[i].Split(new string[] { "<a href=\"" }, StringSplitOptions.None);
                    string[] c = b[1].Split('"');
                    f.Add(c[0]);
                }
                foreach (string s in f)
                {
                    if (yapfileslimit > 0 && Kontrol(s) == true)
                    {
                        s1 = GetSource(s);
                        if (s1.Contains("name=\"direct_link\" value=\""))
                        {
                            string[] cut = s1.Split(new string[] { "name=\"direct_link\" value=\"" }, StringSplitOptions.None);
                            string[] cut2 = cut[1].Split('"');
                            string[] title = s1.Split(new string[] { "<title>" }, StringSplitOptions.None);
                            string[] title2 = title[1].Split(new string[] { "</title>" }, StringSplitOptions.None);
                            string[] desc = s1.Split(new string[] { "description\" content=\"" }, StringSplitOptions.None);
                            string[] desc2 = desc[1].Split('"');
                            videoadi = HttpUtility.HtmlDecode(title2[0]);
                            aciklama = HttpUtility.HtmlDecode(desc2[0]);
                            cut2[0] = cut2[0] + "&ref=" + s;

                            Etiket(videoadi);
                            richTextBox2.Text += videoadi + Environment.NewLine + aciklama + Environment.NewLine + etiket + Environment.NewLine + cut2[0] + Environment.NewLine;
                            VideoDownload(cut2[0], "yapfiles");
                            YoutubeUpload();
                            VideoDelete();
                            Ekle(s);
                            yapfileslimit--;
                        }
                    }
                }
            }
            catch (Exception ex) { richTextBox2.Text += ex.ToString(); AppendText(richTextBox1, Color.Red, "YapFiles Hatası."); }
            AppendText(richTextBox1, Color.Red, "YapFiles Bitti.");
        }//OK 22.05.2014
        private void disclosetvLatest()
        {
            kimde = "disclose";
            hesapadi = dataGridView1.Rows[5].Cells[1].Value.ToString();
            hesapsifresi = dataGridView1.Rows[5].Cells[2].Value.ToString();
            developerkey = dataGridView1.Rows[5].Cells[3].Value.ToString();
            knt_kadi = dataGridView1.Rows[5].Cells[4].Value.ToString();
            try
            {
                listBox1.Items.Add(DateTime.Now.ToString() + " DisCLoseTV");
                List<string> f = new List<string>();
                string s1 = GetSource("http://www.disclose.tv/action/videolist/page/1/all/filter/");
                string[] a = s1.Split(new string[] { "class=\"thumbnail clearfix\" href=\"" }, StringSplitOptions.None);
                for (int i = 5; i < a.Length; i++)
                {
                    string[] link = a[i].Split('"');
                    f.Add("http://www.disclose.tv" + link[0]);
                }
                foreach (string s in f)
                {
                    if (discloselimit > 0 && Kontrol(s) == true)
                    {
                        s1 = GetSource(s);
                        if (s1.Contains("<source src='"))
                        {
                            string[] title = s1.Split(new string[] { "<title>" }, StringSplitOptions.None);
                            string[] title1 = title[1].Split(new string[] { "</title>" }, StringSplitOptions.None);
                            videoadi = HttpUtility.HtmlDecode(title1[0]);
                            string[] desc = s1.Split(new string[] { "<meta name=\"description\" content=\"" }, StringSplitOptions.None);
                            string[] desc1 = desc[1].Split(new string[] { "\">" }, StringSplitOptions.None);
                            desc1[0] = StripHTML(desc1[0]);
                            desc1[0] = HttpUtility.HtmlDecode(desc1[0].Trim());
                            aciklama = desc1[0];
                            string[] link = s1.Split(new string[] { "<source src='" }, StringSplitOptions.None);
                            string[] link1 = link[1].Split('\'');
                            Etiket(title1[0]);
                            if (boyutBul(link1[0])>40)
                                continue;
                            richTextBox2.Text += videoadi + Environment.NewLine + aciklama + Environment.NewLine + etiket + Environment.NewLine + link1[0] + Environment.NewLine;
                            VideoDownload(link1[0], "disclose");
                            YoutubeUpload();
                            VideoDelete();
                            Ekle(s);
                            discloselimit--;
                        }
                    }
                }
            }
            catch (Exception ex) { richTextBox2.Text += ex.ToString(); AppendText(richTextBox1, Color.Red, "DisCloseTv Hatası."); };
            AppendText(richTextBox1, Color.Red, "DisClose Bitti.");
            if (gun.Enabled == false)
                gun.Enabled = true;
        }//OK 22.05.2014
        #endregion

        private void YoutubeUpload()
        {
            try
            {
                listBox1.Items.Add(DateTime.Now.ToString()+" "+videoismi + " Yükleniyor..");
                Random a = new Random();
                string id = a.Next(100000, 999999).ToString();
                YouTubeRequestSettings settings = new YouTubeRequestSettings(id, developerkey, hesapadi, hesapsifresi);
                YouTubeRequest request = new YouTubeRequest(settings);

                Video newVideo = new Video();
                ((GDataRequestFactory)request.Service.RequestFactory).Timeout = 9999999;
                newVideo.Title = videoadi;
                string cat = "Entertainment";
                if (videoismi.Contains("liveleak"))
                    cat = "News";
                newVideo.Tags.Add(new MediaCategory(cat, YouTubeNameTable.CategorySchema));
                newVideo.Keywords = etiket;
                newVideo.Description = aciklama;
                newVideo.YouTubeEntry.Private = false;
                newVideo.YouTubeEntry.MediaSource = new MediaFileSource(videoismi, "video/x-flv");

                request.Upload(newVideo);
                listBox1.Items.Add(DateTime.Now.ToString() + "   " + videoismi);
            }
            catch (Exception ex)
            {
                richTextBox4.Text += ex.ToString();
                AppendText(richTextBox1, Color.Red, kimde + " Yükleme Hatası." + Environment.NewLine);
            }
        }
        private void VideoDelete()
        {
            File.Delete(videoismi);
        }
        private void VideoDownload(string url,string site)
        {
            videoismi = "";
            Random a = new Random();
            listBox1.Items.Add(DateTime.Now.ToString()+" "+site + " Videosu İndiriliyor..");
            videoismi = site+"_"+a.Next(10000000, 99999999).ToString() + ".flv";
            using (WebClient wb = new WebClient())
            {
                /*wb.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                wb.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                wb.DownloadFileAsync(URL, videoismi);*/
                wb.DownloadFile(url, videoismi);
            }
        }
        private void gun_Tick(object sender, EventArgs e)
        {
            int h, m;
            h = DateTime.Now.Hour;
            m = DateTime.Now.Minute;

            if ((h == 00 && m == 00 || h == 12 && m == 00) && oto)
            {
                Thread a = new Thread(new ThreadStart(Kontrol));
                a.Start();
                gun.Enabled = false;
            }

        }
        private void Guncelle()
        {
            using (OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=hesap.mdb;Jet OLEDB:Database Password=6528yayla"))
            {
                OleDbDataAdapter adaptor = new OleDbDataAdapter("Select * from hesap", conn);
                DataSet ds = new DataSet();
                ds.Clear();
                adaptor.Fill(ds, "Kimlik");
                dataGridView1.DataSource = ds.Tables["Kimlik"];
                adaptor.Dispose();
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            Guncelle();
        }
        private void channelchecker()
        {
            try
            {
                foreach (DataGridViewRow a in dataGridView1.Rows)
                {
                    bool b = true;
                    try
                    {
                        using (var client = new WebClient())
                        {
                            client.Headers["User-Agent"] = "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US; rv:1.9.2.6) Gecko/20100625 Firefox/3.6.6 (.NET CLR 3.5.30729)";
                            client.Headers["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                            client.Headers["Accept-Language"] = "en-us,en;q=0.5";
                            client.Headers["Accept-Encoding"] = "    gzip,deflate";
                            client.Headers["Accept-Charset"] = "ISO-8859-1,utf-8;q=0.7,*;q=0.7";
                            client.Encoding = Encoding.UTF8;
                            client.DownloadString("https://gdata.youtube.com/feeds/api/users/" + a.Cells[4].Value.ToString());
                        }
                    }
                    catch
                    { b = false; }
                    if (!b)
                        AppendText(richTextBox5, Color.Red, a.Cells[1].Value + " Kapanmış");
                    else
                        AppendText(richTextBox5, Color.Green, a.Cells[1].Value + " Açık");
                }
            }
            catch(Exception ex) { MessageBox.Show(ex.ToString()); }
            
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            sec--;
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox5.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            textBox4.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox3.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            textBox1.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            using (OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=hesap.mdb;Jet OLEDB:Database Password=6528yayla"))
            {
                conn.Open();
                OleDbCommand sorgu = new OleDbCommand("insert into hesap(hadi,hsifresi,devkey,kadi) values('" + txadi.Text + "','" + txsifresi.Text + "','" + txdevkey.Text + "','" + txkadi.Text + "')", conn);
                sorgu.ExecuteNonQuery();
            }
            Guncelle();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            using (OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=hesap.mdb;Jet OLEDB:Database Password=6528yayla"))
            {
                conn.Open();
                OleDbCommand sorgu = new OleDbCommand("update hesap set hadi = '" + textBox4.Text + "', hsifresi = '" + textBox3.Text + "', devkey = '" + textBox2.Text + "', kadi = '" + textBox1.Text + "' where id = " + textBox5.Text, conn);
                sorgu.ExecuteNonQuery();
            }
            Guncelle();
        }
        private void dataGridView1_CellContentDoubleClick_1(object sender, DataGridViewCellEventArgs e)
        {
            DialogResult sonuc;
            sonuc = MessageBox.Show("Silmek İstediğinizden Emin Misiniz?", "Uyarı", MessageBoxButtons.OKCancel);
            if (sonuc == DialogResult.OK)
            {
                using (OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=hesap.mdb;Jet OLEDB:Database Password=6528yayla"))
                {
                    conn.Open();
                    OleDbCommand sorgu = new OleDbCommand("delete from hesap where id=" + dataGridView1.CurrentRow.Cells[0].Value.ToString(), conn);
                    sorgu.ExecuteNonQuery();
                }
                Guncelle();
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.IO.File.WriteAllText(@"log.txt", richTextBox1.Text);
            Environment.Exit(0);
        }
        private void W8()
        {
            timer1.Enabled = true;
            do
            {
                this.Text = sec.ToString();
                Application.DoEvents();
            }
            while (sec > 1);
            sec = 5;
            timer1.Enabled = false;
        }
        private void Do()
        {

            deleted = 0;
            AppendText(richTextBox1, Color.Black, "Denetim Başladı. Hesap: " + knt_hadi + " " + DateTime.Now.ToString());
            try
            {
                webBrowser1.Navigate("http://www.youtube.com/account_monetization");
                W8();
                videos.Clear();
                /*if (webBrowser1.DocumentText.Contains("Telif"))
                {
                    AppendText(richTextBox1,Color.Red,"Telifli Kanal!!!" + knt_hadi);
                    return;
                }*/
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                if (!webBrowser1.DocumentText.Contains("Tek hesap. Tüm Google.") || !webBrowser1.DocumentText.Contains("Şimdi oturum açın ve hesabınızı kullanarak şunları yapın:"))
                {
                    webBrowser1.Navigate("https://accounts.google.com/Logout?continue=https%3A%2F%2Faccounts.google.com%2FServiceLogin%3Fsacu%3D1%26continue%3Dhttp%253A%252F%252Fwww.youtube.com%252Fsignin%253Faction_handle_signin%253Dtrue%2526app%253Ddesktop%2526feature%253Dsign_in_button%2526hl%253Dtr%2526next%253D%25252F%26service%3Dyoutube%26hl%3Dtr&il=true");
                    W8();
                }
                if (webBrowser1.DocumentText.Contains("Tek hesap. Tüm Google.") || webBrowser1.DocumentText.Contains("Şimdi oturum açın ve hesabınızı kullanarak şunları yapın:"))//while a girmesi lazım 50 den çok varsa.
                {
                    AppendText(richTextBox1, Color.Black, "Hesabınıza Giriliyor.. " + DateTime.Now.ToString());
                    webBrowser1.Document.GetElementById("Email").SetAttribute("value", knt_hadi);
                    webBrowser1.Document.GetElementById("Passwd").SetAttribute("value", knt_hsifresi);
                    webBrowser1.Document.GetElementById("signIn").InvokeMember("click");
                    W8();
                    if (webBrowser1.DocumentText.Contains("Yayınladığınız materyalle ilgili aşağıdaki telif hakkı şikayetlerini aldık:"))
                    { AppendText(richTextBox1, Color.Red, "TELİFLİ KANAL!!!"); return; }
                    if (webBrowser1.DocumentText.Contains("Hesabınızda normal olmayan bir etkinlik tespit ettik. Hesabınıza erişimi hemen geri kazanmak için, hesabınızı nasıl doğrulayacağınızı seçin."))
                    { AppendText(richTextBox1, Color.Red, "Kapanmış -_-"); return; }
                    webBrowser1.Navigate("http://www.youtube.com/account_monetization");
                    W8();
                    if (webBrowser1.DocumentText.Contains("monetize-all-button"))
                    {
                        string[] a = webBrowser1.DocumentText.Split(new string[] { "<div class=\"no-adsense-text\">" }, StringSplitOptions.None);
                        string[] b = a[1].Split(new string[] { "<b>" }, StringSplitOptions.None);
                        string[] c = b[1].Split('<');
                        
                        AppendText(richTextBox1, Color.DarkOliveGreen, "Monetize Edilecek Video Sayınız: " + c[0] + " " + DateTime.Now.ToString());
                        while (webBrowser1.DocumentText.Contains("monetize-all-button"))
                        {
                            if (Convert.ToInt32(c[0]) == 1)
                            {
                                AppendText(richTextBox1, Color.DarkOliveGreen, "Monetize Edilecek Videonuz 1, Geçildi. " + DateTime.Now.ToString());
                                break;
                            }
                            Application.DoEvents();
                            webBrowser1.Document.GetElementById("monetize-all-button").InvokeMember("click");
                            W8();
                            HtmlElementCollection html = webBrowser1.Document.GetElementsByTagName("input");
                            foreach (HtmlElement ee in html)
                            {
                                if (ee.GetAttribute("name").Equals("product_placement"))
                                {
                                    ee.InvokeMember("click");
                                    break;
                                }
                            }
                            sec = 2;
                            W8();
                            webBrowser1.Document.GetElementById("overlay-monetize-button").InvokeMember("click");
                            W8();
                            webBrowser1.Navigate("http://www.youtube.com/features");
                            W8();
                        }
                    }
                    else
                        AppendText(richTextBox1, Color.Blue, "Monetize edilecek video yok.");
                    webBrowser1.Navigate("https://www.youtube.com/my_videos?o=U");
                    W8();
                    TelifliBul();
                    webBrowser1.Navigate("https://accounts.google.com/Logout?continue=https%3A%2F%2Faccounts.google.com%2FServiceLogin%3Fsacu%3D1%26continue%3Dhttp%253A%252F%252Fwww.youtube.com%252Fsignin%253Faction_handle_signin%253Dtrue%2526app%253Ddesktop%2526feature%253Dsign_in_button%2526hl%253Dtr%2526next%253D%25252F%26service%3Dyoutube%26hl%3Dtr&il=true");
                }
                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;

                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds,
                    ts.Milliseconds / 10);
                AppendText(richTextBox1, Color.Coral, "İşlemlerinizin Süresi: " + elapsedTime);
            }
            catch (Exception ex)
            {
                richTextBox4.Text += ex.ToString();
                AppendText(richTextBox1, Color.Red, "HATA oluştu. Tekrar Deneyin.");
                webBrowser1.Navigate("https://accounts.google.com/Logout?continue=https%3A%2F%2Faccounts.google.com%2FServiceLogin%3Fsacu%3D1%26continue%3Dhttp%253A%252F%252Fwww.youtube.com%252Fsignin%253Faction_handle_signin%253Dtrue%2526app%253Ddesktop%2526feature%253Dsign_in_button%2526hl%253Dtr%2526next%253D%25252F%26service%3Dyoutube%26hl%3Dtr&il=true");

            }

        }
        private string GetSource(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ReadWriteTimeout = 10000;
                request.Timeout = 10000;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream receiveStream = response.GetResponseStream();
                    StreamReader readStream = null;

                    if (response.CharacterSet == null)
                        readStream = new StreamReader(receiveStream, Encoding.UTF8);
                    else
                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                    string data = readStream.ReadToEnd();
                    response.Close();
                    readStream.Close();
                    return data;
                }
                else
                    return "";
            }
            catch { return ""; }
        }
        private void TelifliBul()
        {
            if (webBrowser1.DocumentText.Contains("my_videos?o=U&amp;pi="))
            {
                string[] sayfa = webBrowser1.DocumentText.Split(new string[] { "<a href=\"/my_videos?o=U&amp;pi=" }, StringSplitOptions.None);
                string[] son = sayfa[sayfa.Length - 2].Split('"');
                W8();
                AppendText(richTextBox1, Color.DarkOrange, "Toplam Video Sayfanız: " + (Convert.ToInt16(son[0]) + 1) + " " + DateTime.Now.ToString());
                for (int j = 0; j <= Convert.ToInt16(son[0]); j++)
                {
                    webBrowser1.Navigate("https://www.youtube.com/my_videos?o=U&pi=" + j);
                    W8();
                    string[] a = webBrowser1.DocumentText.Split(new string[] { "<li id=\"vm-video-" }, StringSplitOptions.None);
                    for (int i = 1; i < a.Length; i++)
                    {
                        if (a[i].Contains("<span class=\"primary-notification\">"))
                        {
                            string[] b = a[i].Split('"');
                            videos.Add(b[0]);
                        }
                    }
                }
            }
            else
            {
                webBrowser1.Navigate("https://www.youtube.com/my_videos?o=U");
                W8();
                string[] a = webBrowser1.DocumentText.Split(new string[] { "<li id=\"vm-video-" }, StringSplitOptions.None);
                for (int i = 1; i < a.Length; i++)
                {
                    if (a[i].Contains("<span class=\"primary-notification\">"))
                    {
                        string[] b = a[i].Split('"');
                        videos.Add(b[0]);
                    }
                }
            }
            if (videos.Count > 0)
                foreach (string s in videos)
                {
                    Remove(knt_kadi, s);
                    deleted++;
                }
            else
                AppendText(richTextBox1, Color.DarkBlue, "Telifli Videonuz Yok!!");

            if (deleted > 0)
                AppendText(richTextBox1, Color.DarkOliveGreen, deleted + " adet videonuz telif hakkı yüzünden silindi.");
        }
        public void Remove(string uploader, string videoID)
        {
            YouTubeRequestSettings settings = new YouTubeRequestSettings("ControlTower", knt_devkey, knt_hadi, knt_hsifresi);
            bool s = true;
            string vidid = "";
            try
            {
                YouTubeRequest request = new YouTubeRequest(settings);
                Uri uri = new Uri(String.Format("http://gdata.YouTube.com/feeds/api/users/{0}/uploads/{1}", uploader, videoID));
                Video a = request.Retrieve<Video>(uri);
                request.Delete(a);
                vidid = a.VideoId;
            }
            catch
            {
                s = false;
            }
            if (s)
                AppendText(richTextBox1, Color.Chocolate, " Silindi: " + videoID + " " + DateTime.Now.ToString());


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
        private void Kontrol()
        {
            listBox1.Items.Add(DateTime.Now.ToString() + " Kontrol Başladı.");
            /*funnyplacelimit = 25; killsometimelimit = 25; videobashlimit = 25; stupidvideoslimit = 25; dpccarslimit = 25;
            saclimit = 25; australialimit = 25; tvuolimit = 25; alkislimit = 25; yapfileslimit = 25; videogelimit = 25;
            russiaprochanlimit = 25; ochevidetslimit = 25; discloselimit = 25;
            FunnyPlaceIndexWeekly();
            KillSomeTimeLatestFunnyVideos();
            AlkislarlaYasiyorumLatest();
            VideoBashDailyMostViewedVideos();
            VidMaxLatest();
            EuroNewsLatest();*/
            AustraliaSmhLatest();
            RussiaProChanLatest();
            OchevidetsLatest();
            YapFilesLatest();
            LiveLeakLatest();
            disclosetvLatest();
            panel1.Visible = true;
        }
        private void SadeceKontrol()
        {
            for (int i = 0; i < dataGridView1.RowCount - 1; i++)
            {
                knt_hadi = dataGridView1.Rows[i].Cells[1].Value.ToString();
                knt_hsifresi = dataGridView1.Rows[i].Cells[2].Value.ToString();
                knt_devkey = dataGridView1.Rows[i].Cells[3].Value.ToString();
                knt_kadi = dataGridView1.Rows[i].Cells[4].Value.ToString();
                Do();
            }
            System.Diagnostics.Process.Start("rundll32.exe", "InetCpl.cpl,ClearMyTracksByProcess 255");
            AppendText(richTextBox1, Color.AliceBlue, "IE Temizlendi.");
            webBrowser1.Navigate("about:blank");
        }
        private void button2_Click(object sender, EventArgs e)
        {
            SadeceKontrol();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Thread a = new Thread(new ThreadStart(FunnyPlaceIndexWeekly));
            a.Start();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            Thread a = new Thread(new ThreadStart(KillSomeTimeLatestFunnyVideos));
            a.Start();
        }
        private void button7_Click(object sender, EventArgs e)
        {
            Thread a = new Thread(new ThreadStart(AustraliaSmhLatest));
            a.Start();
        }
        private void button10_Click(object sender, EventArgs e)
        {
        }
        private void button9_Click(object sender, EventArgs e)
        {
            Thread a = new Thread(new ThreadStart(LiveLeakLatest));
            a.Start();
        }
        private void button12_Click(object sender, EventArgs e)
        {
            Thread a = new Thread(new ThreadStart(RussiaProChanLatest));
            a.Start();
        }
        private void button11_Click(object sender, EventArgs e)
        {
            Thread a = new Thread(new ThreadStart(OchevidetsLatest));
            a.Start();
        }
        private void button20_Click(object sender, EventArgs e)
        {
            Thread a = new Thread(new ThreadStart(YapFilesLatest));
            a.Start();
        }
        private void button19_Click(object sender, EventArgs e)
        {

        }
        private void button18_Click(object sender, EventArgs e)
        {
            Thread a = new Thread(new ThreadStart(disclosetvLatest));
            a.Start();
        }
        private void gogogo()
        {
            List<string> links = new List<string>();
            List<string> links1 = new List<string>();
            string a = GetSourceWebClient("http://www.onlinehile.org");
            string[] b = a.Split(new string[] { "http://" }, StringSplitOptions.None);
            for (int i = 1; i < b.Length; i++)
            {
                string[] c = b[i].Split('"');
                if (c[0].Contains("onlinehile.org") && !links.Contains(c[0]))
                { links.Add("http://" + c[0]); richTextBox2.Text += c[0] + Environment.NewLine; }
            }
            foreach (string s in links)
            {
                string ss = GetSourceLongWay(s);
                string[] bb = a.Split(new string[] { "http://" }, StringSplitOptions.None);
                for (int j = 1; j < bb.Length; j++)
                {
                    string[] c = bb[j].Split('"');
                    if (c[0].Contains("onlinehile.org") && !links.Contains(c[0]) && !links1.Contains(c[0]))
                    { links1.Add(c[0]); richTextBox2.Text += c[0] + Environment.NewLine; }
                }
            }
            foreach (string s in links1)
            {
                string ss = GetSourceLongWay(s);
                string[] bb = a.Split(new string[] { "http://" }, StringSplitOptions.None);
                for (int j = 1; j < bb.Length; j++)
                {
                    string[] c = bb[j].Split('"');
                    if (c[0].Contains("onlinehile.org") && !links.Contains(c[0]) && !links1.Contains(c[0]))
                    { /*links1.Add(c[0]);*/ richTextBox2.Text += c[0] + Environment.NewLine; }
                }
            }
        }
        private void button13_Click(object sender, EventArgs e)
        {
            Thread a = new Thread(new ThreadStart(gogogo));
            a.Start();
        }
        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {
            richTextBox3.SelectionStart = richTextBox3.Text.Length; //Set the current caret position at the end
            richTextBox3.ScrollToCaret(); //Now scroll it automatically
        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = richTextBox1.Text.Length; //Set the current caret position at the end
            richTextBox1.ScrollToCaret(); //Now scroll it automatically
        }
        private void button16_Click(object sender, EventArgs e)
        {
            MessageBox.Show(GetSourceWebClient("http://video.sina.com/v/video.php?page=2&class=5&channel=67&category=mr"));
        }
        private void button14_Click(object sender, EventArgs e)
        {
            List<string> ls = new List<string>();
            string s = GetSourceWebClient("https://web.archive.org/web/20101124221036/http://news.google.com.tr/");
            string[] a = s.Split(new string[] { "<div class=\"main\">" }, StringSplitOptions.None);
            string[] b = a[1].Split(new string[] { "href=\"/web/" }, StringSplitOptions.None);
            for (int i = 1; i < b.Length; i++)
            {
                string[] c = b[i].Split('"');
                if (!c[0].Contains(".google."))
                { listBox1.Items.Add(c[0]);
                ls.Add(c[0]);
                }
            }
            
            MessageBox.Show(ls.Distinct().Count().ToString());
        }
        private void button17_Click(object sender, EventArgs e)
        {

        }

        private void button25_Click(object sender, EventArgs e)
        {
            Ekle("omer");
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            int visibleItems = listBox1.ClientSize.Height / listBox1.ItemHeight;
            listBox1.TopIndex = Math.Max(listBox1.Items.Count - visibleItems + 1, 0);
        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void button10_Click_1(object sender, EventArgs e)
        {
            try
            {
                IEnumerable<VideoInfo> vi = DownloadUrlResolver.GetDownloadUrls(txV.Text);
                string s = GetSource(txV.Text);
                foreach (VideoInfo i in vi)
                {
                    txVlink.Text = i.DownloadUrl;
                    txVadi.Text = i.Title;
                    txVetiket.Text = EtiketUret(txVadi.Text);
                    string[] a = s.Split(new string[] { "<p id=\"eow-description\" >" }, StringSplitOptions.None);
                    string[] b = a[1].Split(new string[] { "</p>" }, StringSplitOptions.None);
                    b[0] = HttpUtility.HtmlDecode(b[0]);
                    txVaciklama.Text = b[0];
                    break;
                }
            }
            catch { MessageBox.Show("Err!"); }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            label2.Text = "Hesap Adı: " + dataGridView1.Rows[Convert.ToInt16(numericUpDown1.Value)].Cells[1].Value.ToString() + Environment.NewLine + "Kanal Adı: " + dataGridView1.Rows[Convert.ToInt16(numericUpDown1.Value)].Cells[4].Value.ToString();
            knt_hadi = dataGridView1.Rows[Convert.ToInt16(numericUpDown1.Value)].Cells[1].Value.ToString();
            knt_hsifresi = dataGridView1.Rows[Convert.ToInt16(numericUpDown1.Value)].Cells[2].Value.ToString();
            knt_devkey = dataGridView1.Rows[Convert.ToInt16(numericUpDown1.Value)].Cells[3].Value.ToString();
            knt_kadi = dataGridView1.Rows[Convert.ToInt16(numericUpDown1.Value)].Cells[4].Value.ToString();
        }

        private void button17_Click_1(object sender, EventArgs e)
        {
            if (Kontrol(txV.Text))
            {
                label13.Text = (Convert.ToInt16(label13.Text) + 1).ToString();
                videoadi = txVadi.Text;
                etiket = txVetiket.Text;
                aciklama = txVaciklama.Text;
                VideoDownload(txVlink.Text,"deneme");
                YoutubeUpload();
                VideoDelete();
                Ekle(txV.Text);
                txV.Clear();
            }
            else
                MessageBox.Show("Eklemişin");
        }

        private void txVadi_TextChanged(object sender, EventArgs e)
        {
            txVetiket.Text = EtiketUret(txVadi.Text);
        }

        private void button19_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Text Dosyası Seçiniz ( Birden çok dosya seçebilirsiniz )";
            dialog.Filter = " (*.txt)|*.txt";
            dialog.FilterIndex = 1;
            dialog.Multiselect = true;
            DialogResult a = dialog.ShowDialog();
            if (a == DialogResult.OK)
            {
                foreach (string str in dialog.FileNames)
                {
                    FileInfo file = new FileInfo(str);
                    StreamReader stRead = file.OpenText();
                    while (!stRead.EndOfStream)
                    {
                        listBox3.Items.Add(stRead.ReadLine());
                    }
                }
                Thread q = new Thread(new ThreadStart(manual));
                q.Start();
            }
        }
        private void manual()
        {
            foreach (string s in listBox3.Items)
            {
                string[] a1 = s.Split('|');
                if (Kontrol(a1[0]))
                {
                    developerkey = dataGridView1.Rows[Convert.ToInt16(a1[1])].Cells[3].Value.ToString();
                    hesapadi = dataGridView1.Rows[Convert.ToInt16(a1[1])].Cells[1].Value.ToString();
                    hesapsifresi = dataGridView1.Rows[Convert.ToInt16(a1[1])].Cells[2].Value.ToString();
                    IEnumerable<VideoInfo> vi = DownloadUrlResolver.GetDownloadUrls(a1[0]);
                    string ss = GetSource(a1[0]);
                    foreach (VideoInfo i in vi)
                    {
                        txVlink.Text = i.DownloadUrl;
                        txVadi.Text = i.Title;
                        txVetiket.Text = EtiketUret(txVadi.Text);
                        string[] aa = ss.Split(new string[] { "<p id=\"eow-description\" >" }, StringSplitOptions.None);
                        string[] b = aa[1].Split(new string[] { "</p>" }, StringSplitOptions.None);
                        b[0] = HttpUtility.HtmlDecode(b[0]);
                        txVaciklama.Text = b[0];
                        break;
                    }
                    label13.Text = (Convert.ToInt16(label13.Text) + 1).ToString();
                    videoadi = txVadi.Text;
                    etiket = txVetiket.Text;
                    aciklama = txVaciklama.Text;
                    VideoDownload(txVlink.Text,"yt");
                    YoutubeUpload();
                    VideoDelete();
                    Ekle(txV.Text);
                    txV.Clear();
                }
            }
        }
        private void button21_Click(object sender, EventArgs e)
        {
            List<string> kst = new List<string>();
            for(int i = 200;i>=1;i--)
            {
                this.Text = i.ToString();
                string s1 = GetSource("http://www.killsometime.com/home/" + i);
                string[] a = s1.Split(new string[] { "<article class=\"Homepage-Article-WhatsNew\">" }, StringSplitOptions.None);
                for (int k = 1; k < a.Length; k++)
                {
                    string[] b = a[k].Split(new string[] { "<a class=\"Article\" href=\"" }, StringSplitOptions.None);
                    string[] l = b[1].Split('"');
                    l[0] = "http://www.killsometime.com" + l[0];
                    if (l[0].Contains("/videos/"))
                        kst.Add(l[0]);
                }
            }
            File.WriteAllLines("killsometime.txt", kst);
            kst.Clear();
            for(int i = 200;i>=1;i--)
            {
                this.Text = i.ToString();
                string s1 = GetSource("http://www.stupidvideos.com/videos/all/popular/all/"+i+"/");
                string[] a = s1.Split(new string[] { "<div class=\"block\"><a href=\"" }, StringSplitOptions.None);
                for (int k = 1; k < a.Length; k++)
                {
                    string[] b = a[k].Split('"');
                    kst.Add("http://www.stupidvideos.com" + b[0]);
                }
            }
            File.WriteAllLines("stupidvideos.txt", kst);
            
        }
        private void button22_Click(object sender, EventArgs e)
        {

            /*int k = 0;
            for (int i = 5; i >= 1; i--)
            {
                string s = GetSource("http://www.stupidvideos.com/search/videos/views/month/week/" + i + "/");
                string[] a = s.Split(new string[] { "<div class=\"video_box\"><a href=\"" }, StringSplitOptions.None);
                for (int j = 1; j < a.Length; j++)
                {
                    string[] b = a[j].Split('"');
                    listBox4.Items.Add("http://www.stupidvideos.com"+b[0]+"|"+k++);
                    if (k > 47)
                        k = 0;
                }
            }
            for (int i = 3; i >= 1; i--)
            {
                string s = GetSource("http://www.evo.co.uk/videos/viralvideos/archive/" + i + "/");
                string[] a = s.Split(new string[] { "<div class='archivetitle lf'><a href='" }, StringSplitOptions.None);
                for(int j = 1;j<a.Length;j++)
                {
                    string[] b = a[j].Split('\'');
                    listBox4.Items.Add("http://www.evo.co.uk" + b[0] + "|" + k++);
                    if (k > 47)
                        k = 0;
                }
            }
            for (int i = 2; i >= 1; i--)
            {
                string s = GetSource("http://www.evo.co.uk/videos/trackdayvideos/archive/" + i + "/");
                string[] a = s.Split(new string[] { "<div class='archivetitle lf'><a href='" }, StringSplitOptions.None);
                for (int j = 1; j < a.Length; j++)
                {
                    string[] b = a[j].Split('\'');
                    listBox4.Items.Add("http://www.evo.co.uk" + b[0] + "|" + k++);
                    if (k > 47)
                        k = 0;
                }
            }
            for (int i = 2; i >= 1; i--)
            {
                string s = GetSource("http://www.evo.co.uk/videos/supercarvideos/archive/" + i + "/");
                string[] a = s.Split(new string[] { "<div class='archivetitle lf'><a href='" }, StringSplitOptions.None);
                for (int j = 1; j < a.Length; j++)
                {
                    string[] b = a[j].Split('\'');
                    listBox4.Items.Add("http://www.evo.co.uk" + b[0] + "|" + k++);
                    if (k > 47)
                        k = 0;
                }
            }
            for (int i = 6; i >= 1; i--)
            {
                string s = GetSource("http://www.evo.co.uk/videos/planetevovideos/archive/" + i + "/");
                string[] a = s.Split(new string[] { "<div class='archivetitle lf'><a href='" }, StringSplitOptions.None);
                for (int j = 1; j < a.Length; j++)
                {
                    string[] b = a[j].Split('\'');
                    listBox4.Items.Add("http://www.evo.co.uk" + b[0] + "|" + k++);
                    if (k > 47)
                        k = 0;
                }
            }
            for (int i = 1; i >= 1; i--)
            {
                string s = GetSource("http://www.evo.co.uk/videos/motorsportvideos/archive/" + i + "/");
                string[] a = s.Split(new string[] { "<div class='archivetitle lf'><a href='" }, StringSplitOptions.None);
                for (int j = 1; j < a.Length; j++)
                {
                    string[] b = a[j].Split('\'');
                    listBox4.Items.Add("http://www.evo.co.uk" + b[0] + "|" + k++);
                    if (k > 47)
                        k = 0;
                }
            }
            for (int i = 34; i >= 1; i--)
            {
                string s = GetSourceLongWay("http://www.lepak.tv/video.php?category=mv&ordertype=&videoold=all&catid=3&page=" + i);
                string[] a = s.Split(new string[] { "<a href=\"http://www.lepak.tv/watch/" }, StringSplitOptions.None);
                for (int j = 1; j < a.Length; j++)
                {
                    string[] b = a[j].Split('"');
                    listBox4.Items.Add("http://www.lepak.tv/watch/" + b[0] + "|" + k++);
                    if (k > 47)
                        k = 0;
                }
            }
            for (int i = 30; i >= 1; i--)
            {
                string s = GetSourceLongWay("http://www.lepak.tv/video.php?category=mv&ordertype=&videoold=all&catid=1&page=" + i);
                string[] a = s.Split(new string[] { "<a href=\"http://www.lepak.tv/watch/" }, StringSplitOptions.None);
                for (int j = 1; j < a.Length; j++)
                {
                    string[] b = a[j].Split('"');
                    listBox4.Items.Add("http://www.lepak.tv/watch/" + b[0] + "|" + k++);
                    if (k > 47)
                        k = 0;
                }
            }
            for (int i = 47; i >= 1; i--)
            {
                string s = GetSourceLongWay("http://www.lepak.tv/video.php?category=mv&ordertype=&videoold=all&catid=9&page=" + i);
                string[] a = s.Split(new string[] { "<a href=\"http://www.lepak.tv/watch/" }, StringSplitOptions.None);
                for (int j = 1; j < a.Length; j++)
                {
                    string[] b = a[j].Split('"');
                    listBox4.Items.Add("http://www.lepak.tv/watch/" + b[0] + "|" + k++);
                    if (k > 47)
                        k = 0;
                }
            }*/

        }
        private void button23_Click(object sender, EventArgs e)
        {
            string[] lines = File.ReadAllLines("toplam.txt");
            List<string> son = new List<string>();
            int i = 1;
            foreach (string line in lines)
            {
                lst4.Add(line);
                i++;
                if (i > 740)
                    break;
            }
            for (int j = 740; j <= lines.Length-1; j++)
            {
                son.Add(lines[j]);
            }
            File.WriteAllLines("toplam.txt", son);
            Thread a = new Thread(new ThreadStart(Yapistir));
            a.SetApartmentState(ApartmentState.STA);
            a.Start();
            
            
        }
        private void Yapistir()
        {
            string last = "";
            try
            {
                int k = 0;
                foreach (string s in lst4)
                    listBox4.Items.Add(s);
                foreach (string s in lst4)
                {
                    listBox4.SetSelected(k, true);
                    k++;
                    string[] ab = last.Split('|');
                    string s1 = GetSource(ab[0]);
                    if(s1.Contains("youtube.com"))
                    {

                    }
                    else if (s1.Contains("http://www.killsometime.com/videos/"))
                    {
                        if (s1.Contains("file: '"))
                        {
                            developerkey = dataGridView1.Rows[Convert.ToInt16(ab[1])].Cells[3].Value.ToString();
                            hesapadi = dataGridView1.Rows[Convert.ToInt16(ab[1])].Cells[1].Value.ToString();
                            hesapsifresi = dataGridView1.Rows[Convert.ToInt16(ab[1])].Cells[2].Value.ToString();
                            string[] title = s1.Split(new string[] { "<meta property=\"og:title\" content=\"" }, StringSplitOptions.None);
                            string[] title1 = title[1].Split('"');
                            videoadi = HttpUtility.HtmlDecode(title1[0]);
                            string[] desc = s1.Split(new string[] { "<meta property=\"og:description\" content='" }, StringSplitOptions.None);
                            string[] desc1 = desc[1].Split('\'');
                            desc1[0] = StripHTML(desc1[0]);
                            desc1[0] = HttpUtility.HtmlDecode(desc1[0].Trim());
                            aciklama = desc1[0];
                            string[] link = s1.Split(new string[] { "file: '" }, StringSplitOptions.None);
                            string[] link1 = link[1].Split('\'');
                            link1[0] = "http://www.killsometime.com" + link1[0];
                            Etiket(videoadi);
                            if (!VarMi(link1[0]))
                                continue;
                            VideoDownload(link1[0], "killsometime_remote");
                            YoutubeUpload();
                            VideoDelete();
                            Ekle(listBox4.Items[sira].ToString());
                        }
                    }
                    else
                    {
                        string l3 = "http://videos.stupidvideos.com/2/00/00/0";
                        string l4 = "http://videos.stupidvideos.com/2/00/00/";
                        string l5 = "http://videos.stupidvideos.com/2/00/0";
                        string l6 = "http://videos.stupidvideos.com/2/00/";

                        developerkey = dataGridView1.Rows[Convert.ToInt16(ab[1])].Cells[3].Value.ToString();
                        hesapadi = dataGridView1.Rows[Convert.ToInt16(ab[1])].Cells[1].Value.ToString();
                        hesapsifresi = dataGridView1.Rows[Convert.ToInt16(ab[1])].Cells[2].Value.ToString();

                        string[] cut = s1.Split(new string[] { "var videoID = '" }, StringSplitOptions.None);
                        string[] cut2 = cut[1].Split(new string[] { "';" }, StringSplitOptions.None);
                        if (cut2[0].Length == 3)
                        {

                            for (int i = 0; i < 3; i++)
                            {
                                l3 = l3 + cut2[0][i];
                                if (i == 0)
                                    l3 = l3 + "/";
                                if (i == 2)
                                    l3 = l3 + "/";
                            }
                            cut2[0] = l3 + cut2[0] + ".flv";
                        }
                        else if (cut2[0].Length == 4)
                        {

                            for (int i = 0; i < 4; i++)
                            {
                                l4 = l4 + cut2[0][i];
                                if (i == 1)
                                    l4 = l4 + "/";
                                if (i == 3)
                                    l4 = l4 + "/";
                            }
                            cut2[0] = l4 + cut2[0] + ".flv";
                        }
                        else if (cut2[0].Length == 5)
                        {

                            for (int i = 0; i < 5; i++)
                            {
                                l5 = l5 + cut2[0][i];
                                if (i == 0)
                                    l5 = l5 + "/";
                                if (i == 2)
                                    l5 = l5 + "/";
                            }
                            l5 = l5 + "/";
                            cut2[0] = l5 + cut2[0] + ".flv";
                        }
                        else if (cut2[0].Length == 6)
                        {
                            for (int i = 0; i <= 5; i++)
                            {
                                l6 = l6 + cut2[0][i];
                                if (i == 1)
                                    l6 = l6 + "/";
                                if (i == 3)
                                    l6 = l6 + "/";
                            }
                            l6 = l6 + "/";
                            cut2[0] = l6 + cut2[0] + ".flv";
                        }


                        string[] title = s1.Split(new string[] { "og:title\" content=\"" }, StringSplitOptions.None);
                        string[] title2 = title[1].Split(new string[] { "\" />" }, StringSplitOptions.None);
                        string[] desc = s1.Split(new string[] { "description\" content=\"" }, StringSplitOptions.None);
                        string[] desc2 = desc[1].Split(new string[] { "\"/>" }, StringSplitOptions.None);
                        videoadi = HttpUtility.HtmlDecode(title2[0]);
                        aciklama = HttpUtility.HtmlDecode(desc2[0]);
                        Etiket(videoadi);
                        VideoDownload(cut2[0], "stupid_remote");
                        YoutubeUpload();
                        VideoDelete();
                        Ekle(listBox4.Items[sira].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                AppendText(richTextBox4, Color.Red, ex.ToString());
                AppendText(richTextBox4, Color.Red, last);
            }
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }
        private void button24_Click(object sender, EventArgs e)
        {
            if (!oto)
            {
                oto = true;
                button24.ForeColor = Color.Green;
                button24.Text = "Açık";
            }
            else
            {
                oto = false;
                button24.ForeColor = Color.Maroon;
                button24.Text = "Kapalı";
            }
        }
        private void button25_Click_1(object sender, EventArgs e)
        {
            Thread a = new Thread(new ThreadStart(channelchecker));
            a.Start();
        }
        private void button26_Click(object sender, EventArgs e)
        {
            List<string> kanallar = new List<string>() { "HC7Dr1BKwqctY", "TurkFenomenler", "UCcFhxjEiLdumoBM6NUerIlg", "UCA-GAuice3JZsvIuPBD4jdw", "UCR7-FsKo_QHiBl8GzRptmzg", "ModifiyeTV", "CHPAykutErdogdu", "UC7D6Xp7-B9Rlg05mAyWJfJw", "UC9cC8SO_9qeTvDXrNca60vg", "NetKolayCa", "TheOttomanvideo", "TheCosmosNews", "TheTurkdizileri", "KingTubeNews", "UC2LdDsh1Gy_b34Px-U7teXQ", "UC9NGyswxkya27e6a8ZPeWUw", "UCk0nxnc2IykuguONxE16CWg" };
            foreach (string s in kanallar)
                VideolariGetir(s);
            kanal_sayisi = 0;
        }
        int kanal_sayisi = 0;
        private void VideolariGetir(string kanalAdi)
        {
            try
            {
                int toplam = 0;
                lstmanual.Items.Clear();
                string q;
                using (WebClient asd = new WebClient())
                {
                    asd.Encoding = Encoding.UTF8;
                    q = asd.DownloadString("http://gdata.youtube.com/feeds/api/users/" + kanalAdi + "/uploads?v=2&alt=jsonc&max-results=0");
                }
                string[] adet1 = q.Split(new string[] { "totalItems\":" }, StringSplitOptions.None);
                string[] adet2 = adet1[1].Split(',');
                string adet3 = adet2[0];
                lstmanual.Items.Add(adet3);

                YouTubeRequestSettings settings = new YouTubeRequestSettings("Contentor", dataGridView1.Rows[0].Cells[3].Value.ToString(), dataGridView1.Rows[0].Cells[1].Value.ToString(), dataGridView1.Rows[0].Cells[2].Value.ToString());
                YouTubeRequest request = new YouTubeRequest(settings);
                int index = 1;
                for (int i = 1; i <= Convert.ToInt16(adet3) / 50; i++)
                {
                    Uri uri = new Uri("http://gdata.youtube.com/feeds/api/users/" + kanalAdi + "/uploads?&max-results=50&start-index=" + index);
                    Feed<Video> videoFeed = request.Get<Video>(uri);
                    foreach (Video entry in videoFeed.Entries)
                    {
                        lstmanual.Items.Add("http://www.youtube.com/watch?v=" + entry.VideoId + "|" + kanal_sayisi++);
                        if (kanal_sayisi > 47)
                            kanal_sayisi = 0;
                    }
                    index += 50;
                }
                toplam = ((Convert.ToInt16(adet3) / 50) * 50) + 1;
                Uri uri2 = new Uri("http://gdata.youtube.com/feeds/api/users/" +kanalAdi+ "/uploads?&max-results=50&start-index=" + toplam);
                Feed<Video> videoFeed2 = request.Get<Video>(uri2);
                foreach (Video entry in videoFeed2.Entries)
                {
                    lstmanual.Items.Add("http://www.youtube.com/watch?v=" + entry.VideoId + "|" + kanal_sayisi++);
                    if (kanal_sayisi > 47)
                        kanal_sayisi = 0;
                }

            }
            catch
            {

            }
        }
        private void button27_Click(object sender, EventArgs e)
        {
            List<string> a = new List<string>();
            a.AddRange(System.IO.File.ReadAllLines("evilchili.txt"));
            foreach (string s in a)
                listBox4.Items.Add(s);
        }
        private void button28_Click(object sender, EventArgs e)
        {
            MessageBox.Show(listBox4.Items.Count.ToString());
        }
        private void button29_Click(object sender, EventArgs e)
        {
            Thread a = new Thread(new ThreadStart(evilchili));
            a.Start();
        }
        public string chiliLink = "";
        private void getChili()
        {
            for (int i = 0; i < listBox4.Items.Count; i++)
            {
                listBox4.SetSelected(i, true);
                if (listBox4.Items[i].ToString().Length < 20)
                    continue;
                string[] ab = listBox4.Items[i].ToString().Split('|');
                string s = GetSource(ab[0]);
                if (s.Contains("window.clipURL = \""))
                {
                    developerkey = dataGridView1.Rows[Convert.ToInt16(ab[1])].Cells[3].Value.ToString();
                    hesapadi = dataGridView1.Rows[Convert.ToInt16(ab[1])].Cells[1].Value.ToString();
                    hesapsifresi = dataGridView1.Rows[Convert.ToInt16(ab[1])].Cells[2].Value.ToString();

                    string[] title = s.Split(new string[] { "og:title\" content=\"" }, StringSplitOptions.None);
                    string[] title2 = title[1].Split('"');
                    string[] desc = s.Split(new string[] { "description\" content=\"" }, StringSplitOptions.None);
                    string[] desc2 = desc[1].Split(new string[] { "\" />" }, StringSplitOptions.None);

                    string[] cut = s.Split(new string[] { "window.clipURL = \"" }, StringSplitOptions.None);
                    string[] cut2 = cut[1].Split('"');

                    videoadi = HttpUtility.HtmlDecode(title2[0]);
                    aciklama = HttpUtility.HtmlDecode(desc2[0]);
                    Etiket(videoadi);
                    VideoDownload(cut2[0], "chili remote");
                    YoutubeUpload();
                    VideoDelete();
                    Ekle(chiliLink);
                }
            }

        }
        private void evilchili()
        {
            int k = 0;
            List<string> q = new List<string>();
            for (int i = 642; i >= 1; i--)
            {
                this.Text = i.ToString();
                string s = GetSource("http://www.evilchili.com/index.php?page=" + i + "&ipp=20&p=videos");
                string[] a = s.Split(new string[] { "<h2><a href=\"" }, StringSplitOptions.None);

                for (int j = 1; j < a.Length; j++)
                {
                    string[] b = a[j].Split(new string[] {"\">" }, StringSplitOptions.None);
                    if (!b[0].Contains("http://www.evilchili.com/videos"))
                        continue;
                    string son = b[0] + "|" + k++;
                    listBox4.Items.Add(son);
                    q.Add(son);
                    if (k > 47)
                        k = 0;
                }

            }
            for (int i = 0; i < listBox4.Items.Count; i++)
            {
                if (listBox4.Items[i].ToString().Length < 40)
                    listBox4.Items.RemoveAt(i);
            }
        }
        private void button30_Click(object sender, EventArgs e)
        {
            Thread a = new Thread(new ThreadStart(getChili));
            a.Start();
        }
        private void ToplaKaristirCorbala()
        {
            string s1="";
            for (int u = 0; u < listBox4.Items.Count; u++)
            {
                listBox4.SetSelected(u, true);
                string[] ab = listBox4.Items[u].ToString().Split('|');
                if (ab[0].Contains("stupidvideos.com"))
                {
                    string l3 = "http://videos.stupidvideos.com/2/00/00/0";
                    string l4 = "http://videos.stupidvideos.com/2/00/00/";
                    string l5 = "http://videos.stupidvideos.com/2/00/0";
                    string l6 = "http://videos.stupidvideos.com/2/00/";

                    developerkey = dataGridView1.Rows[Convert.ToInt16(ab[1])].Cells[3].Value.ToString();
                    hesapadi = dataGridView1.Rows[Convert.ToInt16(ab[1])].Cells[1].Value.ToString();
                    hesapsifresi = dataGridView1.Rows[Convert.ToInt16(ab[1])].Cells[2].Value.ToString();

                    string[] cut = s1.Split(new string[] { "var videoID = '" }, StringSplitOptions.None);
                    string[] cut2 = cut[1].Split(new string[] { "';" }, StringSplitOptions.None);
                    if (cut2[0].Length == 3)
                    {

                        for (int i = 0; i < 3; i++)
                        {
                            l3 = l3 + cut2[0][i];
                            if (i == 0)
                                l3 = l3 + "/";
                            if (i == 2)
                                l3 = l3 + "/";
                        }
                        cut2[0] = l3 + cut2[0] + ".flv";
                    }
                    else if (cut2[0].Length == 4)
                    {

                        for (int i = 0; i < 4; i++)
                        {
                            l4 = l4 + cut2[0][i];
                            if (i == 1)
                                l4 = l4 + "/";
                            if (i == 3)
                                l4 = l4 + "/";
                        }
                        cut2[0] = l4 + cut2[0] + ".flv";
                    }
                    else if (cut2[0].Length == 5)
                    {

                        for (int i = 0; i < 5; i++)
                        {
                            l5 = l5 + cut2[0][i];
                            if (i == 0)
                                l5 = l5 + "/";
                            if (i == 2)
                                l5 = l5 + "/";
                        }
                        l5 = l5 + "/";
                        cut2[0] = l5 + cut2[0] + ".flv";
                    }
                    else if (cut2[0].Length == 6)
                    {
                        for (int i = 0; i <= 5; i++)
                        {
                            l6 = l6 + cut2[0][i];
                            if (i == 1)
                                l6 = l6 + "/";
                            if (i == 3)
                                l6 = l6 + "/";
                        }
                        l6 = l6 + "/";
                        cut2[0] = l6 + cut2[0] + ".flv";
                    }


                    string[] title = s1.Split(new string[] { "og:title\" content=\"" }, StringSplitOptions.None);
                    string[] title2 = title[1].Split(new string[] { "\" />" }, StringSplitOptions.None);
                    string[] desc = s1.Split(new string[] { "description\" content=\"" }, StringSplitOptions.None);
                    string[] desc2 = desc[1].Split(new string[] { "\"/>" }, StringSplitOptions.None);
                    videoadi = HttpUtility.HtmlDecode(title2[0]);
                    aciklama = HttpUtility.HtmlDecode(desc2[0]);
                    Etiket(videoadi);
                    VideoDownload(cut2[0], "stupid_remote");
                    YoutubeUpload();
                    VideoDelete();
                    Ekle(listBox4.Items[u].ToString());
                }
                else if (ab[0].Contains("http://www.evo.co.uk"))
                {
                    s1 = GetSource(ab[0]);
                    if (!s1.Contains("'file': 'http://www.youtube.com/"))
                    {
                        string[] title = s1.Split(new string[] { "s.prop5=\"" }, StringSplitOptions.None);
                        string[] title2 = title[1].Split('"');
                        string[] desc = s1.Split(new string[] { "description\" content=\"" }, StringSplitOptions.None);
                        string[] desc2 = desc[1].Split(new string[] { "\"/>" }, StringSplitOptions.None);

                        string[] cut = s1.Split(new string[] { "'file': '" }, StringSplitOptions.None);
                        string[] cut2 = cut[1].Split('\'');

                        videoadi = HttpUtility.HtmlDecode(title2[0]);
                        aciklama = HttpUtility.HtmlDecode(desc2[0]);
                        Etiket(videoadi);
                        VideoDownload(cut2[0], "evo.co.uk remote");
                        YoutubeUpload();
                        VideoDelete();
                        Ekle(listBox4.Items[u].ToString());
                    }
                }
                else
                {
                    s1 = GetSourceLongWay(ab[0]);
                    if (s1.Contains("&VKEY="))
                    {
                        string[] a = s1.Split(new string[] { "&VKEY=" }, StringSplitOptions.None);
                        string[] b = a[1].Split('\'');
                        string link = b[0].Substring(0, 6);
                        string tamlink = "http://www.lepak.tv/uploads/flvideo";
                        for (int i = 0; i < link.Length; i++)
                        {
                            if (i % 2 == 0)
                                tamlink += "/";
                            tamlink += link[i];
                        }
                        tamlink += "/" + b[0] + ".flv";


                        string[] title = s1.Split(new string[] { "og:title\" content=\"" }, StringSplitOptions.None);
                        string[] title2 = title[1].Split('<');
                        string[] desc = s1.Split(new string[] { "og:description\" content=\"" }, StringSplitOptions.None);
                        string[] desc2 = desc[1].Split(new string[] { "\"/>" }, StringSplitOptions.None);

                        videoadi = HttpUtility.HtmlDecode(title2[0]);
                        aciklama = HttpUtility.HtmlDecode(desc2[0]);
                        Etiket(videoadi);
                        VideoDownload(tamlink, "lepak remote");
                        YoutubeUpload();
                        VideoDelete();
                        Ekle(listBox4.Items[u].ToString());
                    }
                }

            }
            this.Text = "bitti";
        }
        private void button31_Click(object sender, EventArgs e)
        {
            List<string> a = new List<string>();
            foreach (string s in listBox4.Items)
                a.Add(s);
            File.WriteAllLines("evilchili.txt", a);
        }
        private void button32_Click(object sender, EventArgs e)
        {
            string data = "username=yourusername&password=password"; //replace <value>
            byte[] dataStream = Encoding.UTF8.GetBytes(data);
            string urlPath = "https://api.vineapp.com/users/authenticate";
            string request = urlPath;
            WebRequest webRequest = WebRequest.Create(request);
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ContentLength = dataStream.Length;
            Stream newStream = webRequest.GetRequestStream();
            // Send the data.
            newStream.Write(dataStream, 0, dataStream.Length);
            newStream.Close();
            WebResponse webResponse = webRequest.GetResponse();
            Stream responseStream = webResponse.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            string responseFromServer = reader.ReadToEnd();
            JObject js1 = JObject.Parse(responseFromServer);
            string key = js1["data"]["key"].ToString();

            //GetVineUsers();
            string URL = "https://api.vineapp.com/users/search/" + txtName.Text;

            var webClient = new WebClient();
            webClient.Headers.Add("user-agent", "com.vine.iphone/1.0.3 (unknown, iPhone OS 6.0.1, iPhone, Scale/2.000000)");
            webClient.Headers.Add("vine-session-id", key);
            webClient.Headers.Add("accept-language", "en, sv, fr, de, ja, nl, it, es, pt, pt-PT, da, fi, nb, ko, zh-Hans, zh-Hant, ru, pl, tr, uk, ar, hr, cs, el, he, ro, sk, th, id, ms, en-GB, ca, hu, vi, en-us;q=0.8");
            var json = webClient.DownloadString(URL);
            JObject js = JObject.Parse(json);
            MessageBox.Show(js.Count.ToString());
            /*for (int i = 0; i < 19; i++)
            {
                FbUser cls = new FbUser();
                cls.Id = js["data"]["records"][i]["userId"].ToString();
                cls.Name = js["data"]["records"][i]["username"].ToString();
                cls.MediaName = "Vine";
                listFbUsers.Add(cls);
            }*/
        }
        private void button33_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
        }
        private void button34_Click(object sender, EventArgs e)
        {
            if (panel1.Visible == false)
            {
                panel1.Visible = true;
                return;
            }
            if (panel1.Visible == true)
            {
                panel1.Visible = false;
                return;
            }
        }
        private void button5_Click_1(object sender, EventArgs e)
        {
            Thread a = new Thread(new ThreadStart(yukle));
            a.Start();
            //MessageBox.Show((5 % 5).ToString());
        }
        private void yukle()
        {
            string[] a = System.IO.File.ReadAllLines("links.txt");
            var b = a.Take(300).ToArray();
            var c = a.Skip(300).ToArray();
            System.IO.File.WriteAllLines("links2.txt", c);
            int kim = 0;
            for (int i = Convert.ToInt32(numericUpDown2.Value); i < b.Length; i++)
            {
                this.Text = i.ToString();


                string s1 = GetSourceWebClient(b[i]);
                if (!s1.Contains("\"bitrate\": \""))
                    continue;

                /*if (i % 6 == 1)
                    kim = 0;
                else if (i % 6 == 2)
                    kim = 1;
                else if (i % 6 == 3)
                    kim = 2;
                else if (i % 6 == 4)
                    kim = 3;
                else if (i % 6 == 5)
                    kim = 4;
                else
                    kim = 5;*/
                if (i < 50)
                    kim = 0;
                else if (i < 100)
                    kim = 1;
                else if (i< 150)
                    kim = 2;
                else if (i <200)
                    kim = 3;
                else if (i<250)
                    kim = 4;
                else
                    kim = 5;

                kimde = "smh";
                hesapadi = dataGridView1.Rows[kim].Cells[1].Value.ToString();
                hesapsifresi = dataGridView1.Rows[kim].Cells[2].Value.ToString();
                developerkey = dataGridView1.Rows[kim].Cells[3].Value.ToString();
                knt_kadi = dataGridView1.Rows[kim].Cells[4].Value.ToString();

                toolStripStatusLabel1.Text = hesapadi + " | " + hesapsifresi + " | " + developerkey + " | " + knt_kadi;

                string[] title = s1.Split(new string[] { "<meta property=\"og:title\" content=\"" }, StringSplitOptions.None);
                string[] title1 = title[1].Split('"');
                videoadi = title1[0];
                string[] desc = s1.Split(new string[] { "<meta property=\"og:description\" content=\"" }, StringSplitOptions.None);
                string[] desc1 = desc[1].Split('"');
                desc1[0] = StripHTML(desc1[0]);
                desc1[0] = HttpUtility.HtmlDecode(desc1[0].Trim());
                aciklama = desc1[0];
                string[] link = s1.Split(new string[] { "\"bitrate\": \"" }, StringSplitOptions.None);
                int max = 0;
                string l = "";
                for (int j = 1; j < link.Length; j++)
                {
                    string[] t = link[j].Split('"');
                    if (Convert.ToInt16(t[0]) > max)
                    {
                        max = Convert.ToInt16(t[0]);
                        string[] l2 = link[j].Split(new string[] { "\"file\": \"" }, StringSplitOptions.None);
                        string[] l1 = l2[1].Split('"');
                        l = l1[0];
                    }
                }
                Etiket(title1[0]);
                richTextBox2.Text += videoadi + Environment.NewLine + aciklama + Environment.NewLine + etiket + Environment.NewLine + l + Environment.NewLine;
                VideoDownload(l, "smh");
                /*using (OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=link.mdb"))
                {
                    conn.Open();
                    OleDbCommand sorgu = new OleDbCommand("insert into smh(hesapAdi,hesapSifresi,devKey,videoAdi,aciklama,etiket,videoYolu) values('" + hesapadi + "','"+hesapsifresi+"','"+developerkey+"','"+videoadi+"','"+aciklama+"','"+etiket+"','"+Application.StartupPath+"\\"+videoismi+"')", conn);
                    sorgu.ExecuteNonQuery();
                }*/
                YoutubeUpload();
                VideoDelete();
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            string[] a = getChannelVideos(1,dataGridView1.Rows[1].Cells[4].Value.ToString(), dataGridView1.Rows[1].Cells[3].Value.ToString(), dataGridView1.Rows[1].Cells[1].Value.ToString(), dataGridView1.Rows[1].Cells[2].Value.ToString());
            foreach (string s in a)
                richTextBox2.Text += s + Environment.NewLine;
            /*YouTubeRequestSettings settings = new YouTubeRequestSettings("Ex.", "AI39si5f5ycePTeH8Eev0gvDMTcabkaCjG5heu8RUsN9bdx4e1902ccMcA2zUfgOdBshyyQVMLZPMcTI5Y7S5TomdCrkfhDx8g", "chancock349@gmail.com", "6528yayla");
            YouTubeRequest request = new YouTubeRequest(settings);
            Uri videoEntryUrl = new Uri("http://gdata.youtube.com/feeds/api/videos/Z674UslQ0Mg");
            Video video = request.Retrieve<Video>(videoEntryUrl);
            if (video.Media != null && video.Media.Rating != null)
            {
                MessageBox.Show("Restricted in: " + video.Media.Rating.Country);
            }

            if (video.IsDraft)
            {
                MessageBox.Show("Video is not live.");
                string stateName = video.Status.Name;
                if (stateName == "processing")
                {
                    MessageBox.Show("Video is still being processed.");
                }
                else if (stateName == "rejected")
                {
                    MessageBox.Show("Video has been rejected because: " + Environment.NewLine + video.Status.Value);
                }
                else if (stateName == "failed")
                {
                    Console.Write("Video failed uploading because:" + Environment.NewLine + video.Status.Value);
                }

            }*/
        }

        private string[] getChannelVideos(int type,string channelUserName,string devKey,string userName, string pass)
        {
            List<string> videos = new List<string>();
            YouTubeRequestSettings settings = new YouTubeRequestSettings("Contentor", devKey, userName, pass);
            YouTubeRequest request = new YouTubeRequest(settings);
            try
            {
                int toplam = 0;
                string q;
                using (WebClient asd = new WebClient())
                {
                    asd.Encoding = Encoding.UTF8;
                    q = asd.DownloadString("http://gdata.youtube.com/feeds/api/users/" + channelUserName + "/uploads?v=2&alt=jsonc&max-results=0");
                }
                string[] adet1 = q.Split(new string[] { "totalItems\":" }, StringSplitOptions.None);
                string[] adet2 = adet1[1].Split(',');
                string adet3 = adet2[0];
                int index = 1;
                for (int i = 1; i <= Convert.ToInt16(adet3) / 50; i++)
                {
                    Uri uri = new Uri("http://gdata.youtube.com/feeds/api/users/" + channelUserName + "/uploads?&max-results=50&start-index=" + index);
                    Feed<Video> videoFeed = request.Get<Video>(uri);
                    foreach (Video entry in videoFeed.Entries)
                    {
                        videos.Add(entry.VideoId);
                    }
                    index += 50;
                }
                toplam = ((Convert.ToInt16(adet3) / 50) * 50) + 1;
                Uri uri2 = new Uri("http://gdata.youtube.com/feeds/api/users/" + channelUserName + "/uploads?&max-results=50&start-index=" + toplam);
                Feed<Video> videoFeed2 = request.Get<Video>(uri2);
                foreach (Video entry in videoFeed2.Entries)
                {
                    videos.Add(entry.VideoId);
                }
            }
            catch
            {
                MessageBox.Show("Sayfa Adında Sorun Var!");
            }
            if (type == 1)
            {
                List<string> checkedVideos = new List<string>();
                foreach (string s in videos)
                {
                    Uri videoEntryUrl = new Uri("http://gdata.youtube.com/feeds/api/videos/"+s);
                    Video video = request.Retrieve<Video>(videoEntryUrl);
                    if (video.Media != null && video.Media.Rating != null)
                    {
                        checkedVideos.Add(s+" restricted: "+video.Media.Rating.Country);
                    }

                    if (video.IsDraft)
                    {
                        string stateName = video.Status.Name;
                        checkedVideos.Add(s + " not live: "+stateName);
                        /*if (stateName == "processing")
                        {
                            checkedVideos.Add(s + " processing: " + video.Media.Rating.Country);
                        }
                        else if (stateName == "rejected")
                        {
                            checkedVideos.Add(s+" rejected: "+video.Status.Value);
                        }
                        else if (stateName == "failed")
                        {
                            checkedVideos.Add(s+" failed uploading: "+ video.Status.Value);
                        }*/
                    }
                }
                return checkedVideos.ToArray();
            }
                else
            return videos.ToArray();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://accounts.google.com/UnlockCaptcha?");
        }
    }
}
