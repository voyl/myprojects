using Google.GData.Client;
using Google.GData.Extensions.MediaRss;
using Google.GData.YouTube;
using Google.YouTube;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Splicer;
using Splicer.Renderer;
using Splicer.Timeline;
using Splicer.WindowsMedia;


namespace new_upload
{
    public partial class Form2 : Form
    {
        string etiket, videoAdi, aciklama, videoismi, developerkey, hesapadi, hesapsifresi,defVideoAdi,defAciklama;
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
        }
        static string StripHTML(string inputString)
        {
            return Regex.Replace
              (inputString, "<.*?>", string.Empty);
        }
        private string GetSource(string url)
        {
            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/34.0.1847.131 Safari/537.36";
                return client.DownloadString(url);
            }
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
        private void LiveLeakLatest()
        {
            developerkey = "AI39si6jhuuv9pZwmPcOs75g02rwOG17jk2QYIpqnKMFUY1D_H-2xK-_q0f_kCWAh-pJmGiQi8SClMLm3_CrFQfYfMQ3vMzNxA";
            hesapadi = "spencerw172@gmail.com";
            hesapsifresi = "6528yayla";
            try
            {
                AppendText(richTextBox1, Color.Red, DateTime.Now.ToString() + "Liveleak Latest Başladı");
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
                    if (Kontrol(s))
                    {
                        string[] cut = s1.Split(new string[] { "file: \"" }, StringSplitOptions.None);
                        string[] cut2 = cut[1].Split(new string[] { "\"," }, StringSplitOptions.None);
                        string[] title = s1.Split(new string[] { "style=\"vertical-align:top; padding-right:10px\">" }, StringSplitOptions.None);
                        string[] title2 = title[1].Split('<');
                        string[] desc = s1.Split(new string[] { "<div id=\"body_text\"><p>" }, StringSplitOptions.None);
                        string[] desc2 = desc[1].Split(new[] { "<style>" }, StringSplitOptions.None);
                        cut2[0] = cut2[0] + "&ec_seek=0";
                        videoAdi = HttpUtility.HtmlDecode(title2[0]);
                        defVideoAdi = videoAdi;
                        aciklama = StripHTML(HttpUtility.HtmlDecode(desc2[0])).Trim();
                        defAciklama = aciklama;
                        Etiket(videoAdi);
                        richTextBox3.Text += videoAdi + Environment.NewLine + aciklama + Environment.NewLine + etiket + Environment.NewLine + cut2[0] + Environment.NewLine;
                        VideoDownload(cut2[0], "liveleak");
                        DialogResult dialogResult = MessageBox.Show("Sure?", "Approvement", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.No)
                        {
                            break;
                        }

                        AppendText(richTextBox1, Color.DarkOrange, DateTime.Now.ToString() + "Liveleak İngilizce Video Yükleniyor..");
                        YoutubeUpload();

                        AppendText(richTextBox1, Color.DarkOrange, DateTime.Now.ToString() + "Liveleak Arapça Video Yükleniyor..");
                        videoAdi = toArapca(defVideoAdi);
                        aciklama = toArapca(defAciklama);
                        Etiket(videoAdi);
                        YoutubeUpload();

                        AppendText(richTextBox1, Color.DarkOrange, DateTime.Now.ToString() + "Liveleak Azerice Video Yükleniyor..");
                        videoAdi = toAzerice(defVideoAdi);
                        aciklama = toAzerice(defAciklama);
                        Etiket(videoAdi);
                        YoutubeUpload();

                        AppendText(richTextBox1, Color.DarkOrange, DateTime.Now.ToString() + "Liveleak Belarusça Video Yükleniyor..");
                        videoAdi = toBelarusca(defVideoAdi);
                        aciklama = toBelarusca(defAciklama);
                        Etiket(videoAdi);
                        YoutubeUpload();

                        AppendText(richTextBox1, Color.DarkOrange, DateTime.Now.ToString() + "Liveleak Bulgarca Video Yükleniyor..");
                        videoAdi = toBulgarca(defVideoAdi);
                        aciklama = toBulgarca(defAciklama);
                        Etiket(videoAdi);
                        YoutubeUpload();

                        AppendText(richTextBox1, Color.DarkOrange, DateTime.Now.ToString() + "Liveleak Katalanca Video Yükleniyor..");
                        videoAdi = toKatalanca(defVideoAdi);
                        aciklama = toKatalanca(defAciklama);
                        Etiket(videoAdi);
                        YoutubeUpload();

                        AppendText(richTextBox1, Color.DarkOrange, DateTime.Now.ToString() + "Liveleak Çekce Video Yükleniyor..");
                        videoAdi = toCekce(defVideoAdi);
                        aciklama = toCekce(defAciklama);
                        Etiket(videoAdi);
                        YoutubeUpload();

                        AppendText(richTextBox1, Color.DarkOrange, DateTime.Now.ToString() + "Liveleak Danca Video Yükleniyor..");
                        videoAdi = toDanca(defVideoAdi);
                        aciklama = toDanca(defAciklama);
                        Etiket(videoAdi);
                        YoutubeUpload();

                        AppendText(richTextBox1, Color.DarkOrange, DateTime.Now.ToString() + "Liveleak Almanca Video Yükleniyor..");
                        videoAdi = toAlmanca(defVideoAdi);
                        aciklama = toAlmanca(defAciklama);
                        Etiket(videoAdi);
                        YoutubeUpload();

                        AppendText(richTextBox1, Color.DarkOrange, DateTime.Now.ToString() + "Liveleak Yunanca Video Yükleniyor..");
                        videoAdi = toYunanca(defVideoAdi);
                        aciklama = toYunanca(defAciklama);
                        Etiket(videoAdi);
                        YoutubeUpload();

                        AppendText(richTextBox1, Color.DarkOrange, DateTime.Now.ToString() + "Liveleak İspanyolca Video Yükleniyor..");
                        videoAdi = toIspanyolca(defVideoAdi);
                        aciklama = toIspanyolca(defAciklama);
                        Etiket(videoAdi);
                        YoutubeUpload();

                        AppendText(richTextBox1, Color.DarkOrange, DateTime.Now.ToString() + "Liveleak Estonca Video Yükleniyor..");
                        videoAdi = toEstonca(defVideoAdi);
                        aciklama = toEstonca(defAciklama);
                        Etiket(videoAdi);
                        YoutubeUpload();

                        AppendText(richTextBox1, Color.DarkOrange, DateTime.Now.ToString() + "Liveleak Fince Video Yükleniyor..");
                        videoAdi = toFince(defVideoAdi);
                        aciklama = toFince(defAciklama);
                        Etiket(videoAdi);
                        YoutubeUpload();

                        AppendText(richTextBox1, Color.DarkOrange, DateTime.Now.ToString() + "Liveleak Fransızca Video Yükleniyor..");
                        videoAdi = toFransizca(defVideoAdi);
                        aciklama = toFransizca(defAciklama);
                        Etiket(videoAdi);
                        YoutubeUpload();

                        AppendText(richTextBox1, Color.DarkOrange, DateTime.Now.ToString() + "Liveleak İbranice Video Yükleniyor..");
                        videoAdi = toIbranice(defVideoAdi);
                        aciklama = toIbranice(defAciklama);
                        Etiket(videoAdi);
                        YoutubeUpload();

                        AppendText(richTextBox1, Color.DarkOrange, DateTime.Now.ToString() + "Liveleak Hırvatça Video Yükleniyor..");
                        videoAdi = toHirvatca(defVideoAdi);
                        aciklama = toHirvatca(defAciklama);
                        Etiket(videoAdi);
                        YoutubeUpload();

                        AppendText(richTextBox1, Color.DarkOrange, DateTime.Now.ToString() + "Liveleak Macarca Video Yükleniyor..");
                        videoAdi = toMacarca(defVideoAdi);
                        aciklama = toMacarca(defAciklama);
                        Etiket(videoAdi);
                        YoutubeUpload();

                        AppendText(richTextBox1, Color.DarkOrange, DateTime.Now.ToString() + "Liveleak Ermenice Video Yükleniyor..");
                        videoAdi = toErmenice(defVideoAdi);
                        aciklama = toErmenice(defAciklama);
                        Etiket(videoAdi);
                        YoutubeUpload();

                        AppendText(richTextBox1, Color.DarkOrange, DateTime.Now.ToString() + "Liveleak İtalyanca Video Yükleniyor..");
                        videoAdi = toItalyanca(defVideoAdi);
                        aciklama = toItalyanca(defAciklama);
                        Etiket(videoAdi);
                        YoutubeUpload();

                        AppendText(richTextBox1, Color.DarkOrange, DateTime.Now.ToString() + "Liveleak Gürcüce Video Yükleniyor..");
                        videoAdi = toGurcuce(defVideoAdi);
                        aciklama = toGurcuce(defAciklama);
                        Etiket(videoAdi);
                        YoutubeUpload();

                        AppendText(richTextBox1, Color.DarkOrange, DateTime.Now.ToString() + "Liveleak Litvanca Video Yükleniyor..");
                        videoAdi = toLitvanca(defVideoAdi);
                        aciklama = toLitvanca(defAciklama);
                        Etiket(videoAdi);
                        YoutubeUpload();

                        AppendText(richTextBox1, Color.DarkOrange, DateTime.Now.ToString() + "Liveleak Letonca Video Yükleniyor..");
                        videoAdi = toLetonca(defVideoAdi);
                        aciklama = toLetonca(defAciklama);
                        Etiket(videoAdi);
                        YoutubeUpload();

                        AppendText(richTextBox1, Color.DarkOrange, DateTime.Now.ToString() + "Liveleak Makedonca Video Yükleniyor..");
                        videoAdi = toMakedonca(defVideoAdi);
                        aciklama = toMakedonca(defAciklama);
                        Etiket(videoAdi);

                        AppendText(richTextBox1, Color.DarkOrange, DateTime.Now.ToString() + "Liveleak Hollandaca Video Yükleniyor..");
                        videoAdi = toHollandaca(defVideoAdi);
                        aciklama = toHollandaca(defAciklama);
                        Etiket(videoAdi);
                        YoutubeUpload();

                        AppendText(richTextBox1, Color.DarkOrange, DateTime.Now.ToString() + "Liveleak Norveççe Video Yükleniyor..");
                        videoAdi = toNorvecce(defVideoAdi);
                        aciklama = toNorvecce(defAciklama);
                        Etiket(videoAdi);
                        YoutubeUpload();

                        AppendText(richTextBox1, Color.DarkOrange, DateTime.Now.ToString() + "Liveleak Polonca Video Yükleniyor..");
                        videoAdi = toPolonca(defVideoAdi);
                        aciklama = toPolonca(defAciklama);
                        Etiket(videoAdi);
                        YoutubeUpload();

                        AppendText(richTextBox1, Color.DarkOrange, DateTime.Now.ToString() + "Liveleak Portekizce Video Yükleniyor..");
                        videoAdi = toPortekizce(defVideoAdi);
                        aciklama = toPortekizce(defAciklama);
                        Etiket(videoAdi);
                        YoutubeUpload();

                        AppendText(richTextBox1, Color.DarkOrange, DateTime.Now.ToString() + "Liveleak Romanca Video Yükleniyor..");
                        videoAdi = toRomanca(defVideoAdi);
                        aciklama = toRomanca(defAciklama);
                        Etiket(videoAdi);
                        YoutubeUpload();

                        AppendText(richTextBox1, Color.DarkOrange, DateTime.Now.ToString() + "Liveleak Rusça Video Yükleniyor..");
                        videoAdi = toRusca(defVideoAdi);
                        aciklama = toRusca(defAciklama);
                        Etiket(videoAdi);
                        YoutubeUpload();

                        AppendText(richTextBox1, Color.DarkOrange, DateTime.Now.ToString() + "Liveleak Slovakça Video Yükleniyor..");
                        videoAdi = toSlovakca(defVideoAdi);
                        aciklama = toSlovakca(defAciklama);
                        Etiket(videoAdi);
                        YoutubeUpload();

                        AppendText(richTextBox1, Color.DarkOrange, DateTime.Now.ToString() + "Liveleak Slovence Video Yükleniyor..");
                        videoAdi = toSlovence(defVideoAdi);
                        aciklama = toSlovence(defAciklama);
                        Etiket(videoAdi);
                        YoutubeUpload();

                        AppendText(richTextBox1, Color.DarkOrange, DateTime.Now.ToString() + "Liveleak Arnavutça Video Yükleniyor..");
                        videoAdi = toArnavutca(defVideoAdi);
                        aciklama = toArnavutca(defAciklama);
                        Etiket(videoAdi);
                        YoutubeUpload();

                        AppendText(richTextBox1, Color.DarkOrange, DateTime.Now.ToString() + "Liveleak Sırpça Video Yükleniyor..");
                        videoAdi = toSirpca(defVideoAdi);
                        aciklama = toSirpca(defAciklama);
                        Etiket(videoAdi);
                        YoutubeUpload();

                        AppendText(richTextBox1, Color.DarkOrange, DateTime.Now.ToString() + "Liveleak İsveççe Video Yükleniyor..");
                        videoAdi = toIsvecce(defVideoAdi);
                        aciklama = toIsvecce(defAciklama);
                        Etiket(videoAdi);
                        YoutubeUpload();

                        AppendText(richTextBox1, Color.DarkOrange, DateTime.Now.ToString() + "Liveleak Ukraynaca Video Yükleniyor..");
                        videoAdi = toUkraynaca(defVideoAdi);
                        aciklama = toUkraynaca(defAciklama);
                        Etiket(videoAdi);
                        YoutubeUpload();



                        VideoDelete();
                        Ekle(s);
                    }
                }
            }
            catch (Exception ex) { richTextBox2.Text += ex.ToString(); AppendText(richTextBox1, Color.Red, "LiveLeak Hatası."); }
            AppendText(richTextBox1, Color.Red, DateTime.Now.ToString()+" LiveLeak Bitti.");
        }
        private void YoutubeUpload()
        {
            try
            {
                Random a = new Random();
                string id = a.Next(100000, 999999).ToString();
                YouTubeRequestSettings settings = new YouTubeRequestSettings(id, developerkey, hesapadi, hesapsifresi);
                YouTubeRequest request = new YouTubeRequest(settings);

                Video newVideo = new Video();
                ((GDataRequestFactory)request.Service.RequestFactory).Timeout = 9999999;
                newVideo.Title = videoAdi;
                newVideo.Tags.Add(new MediaCategory("News", YouTubeNameTable.CategorySchema));
                newVideo.Keywords = etiket;
                newVideo.Description = aciklama;
                newVideo.YouTubeEntry.Private = false;
                newVideo.YouTubeEntry.MediaSource = new MediaFileSource(videoismi, "video/x-flv");

                request.Upload(newVideo);
                AppendText(richTextBox1,Color.Azure,DateTime.Now.ToString() + " -> " + videoismi);
            }
            catch (Exception ex)
            {
                richTextBox2.Text += ex.ToString();
                AppendText(richTextBox1, Color.Red,"Yükleme Hatası." + Environment.NewLine);
            }
        }
        private void VideoDelete()
        {
            File.Delete(videoismi);
        }
        private void VideoDownload(string url, string site)
        {
            videoismi = "";
            Random a = new Random();
            videoismi = site + "_" + a.Next(10000000, 99999999).ToString() + ".flv";
            WebClient b = new WebClient();
            b.DownloadFile(url, videoismi);
        }
        private void channelchecker()
        {
            /*try
            {
                foreach (DataGridViewRow a in dataGridView1.Rows)
                {
                    bool b = true;
                    try
                    {
                        using (var client = new WebClient())
                        {
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
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }*/
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Thread a = new Thread(new ThreadStart(LiveLeakLatest));
            a.Start();
        }
        //Her Dile Kanal Eklenecek.
        #region Translations
        private string toArapca(string m)
        {
            using (WebClient wb = new WebClient())
            {
                wb.Encoding = Encoding.UTF8;
                string a = wb.DownloadString("http://translate.yandex.net/tr.json/translate?lang=en-ar&text=" + m.Replace(" ", "+") + "&srv=tr-text");
                a = a.Remove(0, 1);
                a = a.Remove(a.Length - 1, 1);
                return a;
            }
        }
        private string toAzerice(string m)
        {
            using (WebClient wb = new WebClient())
            {
                wb.Encoding = Encoding.UTF8;
                string a = wb.DownloadString("http://translate.yandex.net/tr.json/translate?lang=en-az&text=" + m.Replace(" ", "+") + "&srv=tr-text");
                a = a.Remove(0, 1);
                a = a.Remove(a.Length - 1, 1);
                return a;
            }
        }
        private string toBelarusca(string m)
        {
            using (WebClient wb = new WebClient())
            {
                wb.Encoding = Encoding.UTF8;
                string a = wb.DownloadString("http://translate.yandex.net/tr.json/translate?lang=en-be&text=" + m.Replace(" ", "+") + "&srv=tr-text");
                a = a.Remove(0, 1);
                a = a.Remove(a.Length - 1, 1);
                return a;
            }
        }
        private string toBulgarca(string m)
        {
            using (WebClient wb = new WebClient())
            {
                wb.Encoding = Encoding.UTF8;
                string a = wb.DownloadString("http://translate.yandex.net/tr.json/translate?lang=en-bg&text=" + m.Replace(" ", "+") + "&srv=tr-text");
                a = a.Remove(0, 1);
                a = a.Remove(a.Length - 1, 1);
                return a;
            }
        }
        private string toKatalanca(string m)
        {
            using (WebClient wb = new WebClient())
            {
                wb.Encoding = Encoding.UTF8;
                string a = wb.DownloadString("http://translate.yandex.net/tr.json/translate?lang=en-ca&text=" + m.Replace(" ", "+") + "&srv=tr-text");
                a = a.Remove(0, 1);
                a = a.Remove(a.Length - 1, 1);
                return a;
            }
        }
        private string toCekce(string m)
        {
            developerkey = "AI39si52s-L4H5f22vzFyXBWcbin9sbfIzZGUXuHV0vKFcfih4q44pBvuIKbeaq12FOO_45F9ZUOzipNGYyooS7qXimWUkryjQ";
            hesapadi = "osean956@gmail.com";
            hesapsifresi = "6528yayla";
            using (WebClient wb = new WebClient())
            {
                wb.Encoding = Encoding.UTF8;
                string a = wb.DownloadString("http://translate.yandex.net/tr.json/translate?lang=en-cs&text=" + m.Replace(" ", "+") + "&srv=tr-text");
                a = a.Remove(0, 1);
                a = a.Remove(a.Length - 1, 1);
                return a;
            }
        }
        private string toDanca(string m)
        {
            using (WebClient wb = new WebClient())
            {
                wb.Encoding = Encoding.UTF8;
                string a = wb.DownloadString("http://translate.yandex.net/tr.json/translate?lang=en-da&text=" + m.Replace(" ", "+") + "&srv=tr-text");
                a = a.Remove(0, 1);
                a = a.Remove(a.Length - 1, 1);
                return a;
            }
        }
        private string toAlmanca(string m)
        {
            using (WebClient wb = new WebClient())
            {
                wb.Encoding = Encoding.UTF8;
                string a = wb.DownloadString("http://translate.yandex.net/tr.json/translate?lang=en-de&text=" + m.Replace(" ", "+") + "&srv=tr-text");
                a = a.Remove(0, 1);
                a = a.Remove(a.Length - 1, 1);
                return a;
            }
        }
        private string toYunanca(string m)
        {
            using (WebClient wb = new WebClient())
            {
                wb.Encoding = Encoding.UTF8;
                string a = wb.DownloadString("http://translate.yandex.net/tr.json/translate?lang=en-el&text=" + m.Replace(" ", "+") + "&srv=tr-text");
                a = a.Remove(0, 1);
                a = a.Remove(a.Length - 1, 1);
                return a;
            }
        }
        private string toIspanyolca(string m)
        {
            using (WebClient wb = new WebClient())
            {
                wb.Encoding = Encoding.UTF8;
                string a = wb.DownloadString("http://translate.yandex.net/tr.json/translate?lang=en-es&text=" + m.Replace(" ", "+") + "&srv=tr-text");
                a = a.Remove(0, 1);
                a = a.Remove(a.Length - 1, 1);
                return a;
            }
        }
        private string toEstonca(string m)
        {
            using (WebClient wb = new WebClient())
            {
                wb.Encoding = Encoding.UTF8;
                string a = wb.DownloadString("http://translate.yandex.net/tr.json/translate?lang=en-et&text=" + m.Replace(" ", "+") + "&srv=tr-text");
                a = a.Remove(0, 1);
                a = a.Remove(a.Length - 1, 1);
                return a;
            }
        }
        private string toFince(string m)
        {
            developerkey = "AI39si7vofyxiGO_yK2rZH39CSBmsyVNwUzPzF3Hy0EbFVI1I6TnoKd-yzS1syFTDpUOkq-AZ_CR-SR8TdONZXKea4ACbAHw3Q";
            hesapadi = "buckleyjohn401@gmail.com";
            hesapsifresi = "6528yayla";
            using (WebClient wb = new WebClient())
            {
                wb.Encoding = Encoding.UTF8;
                string a = wb.DownloadString("http://translate.yandex.net/tr.json/translate?lang=en-fi&text=" + m.Replace(" ", "+") + "&srv=tr-text");
                a = a.Remove(0, 1);
                a = a.Remove(a.Length - 1, 1);
                return a;
            }
        }
        private string toFransizca(string m)
        {
            using (WebClient wb = new WebClient())
            {
                wb.Encoding = Encoding.UTF8;
                string a = wb.DownloadString("http://translate.yandex.net/tr.json/translate?lang=en-fr&text=" + m.Replace(" ", "+") + "&srv=tr-text");
                a = a.Remove(0, 1);
                a = a.Remove(a.Length - 1, 1);
                return a;
            }
        }
        private string toIbranice(string m)
        {
            using (WebClient wb = new WebClient())
            {
                wb.Encoding = Encoding.UTF8;
                string a = wb.DownloadString("http://translate.yandex.net/tr.json/translate?lang=en-he&text=" + m.Replace(" ", "+") + "&srv=tr-text");
                a = a.Remove(0, 1);
                a = a.Remove(a.Length - 1, 1);
                return a;
            }
        }
        private string toHirvatca(string m)
        {
            using (WebClient wb = new WebClient())
            {
                wb.Encoding = Encoding.UTF8;
                string a = wb.DownloadString("http://translate.yandex.net/tr.json/translate?lang=en-hr&text=" + m.Replace(" ", "+") + "&srv=tr-text");
                a = a.Remove(0, 1);
                a = a.Remove(a.Length - 1, 1);
                return a;
            }
        }
        private string toMacarca(string m)
        {
            using (WebClient wb = new WebClient())
            {
                wb.Encoding = Encoding.UTF8;
                string a = wb.DownloadString("http://translate.yandex.net/tr.json/translate?lang=en-hu&text=" + m.Replace(" ", "+") + "&srv=tr-text");
                a = a.Remove(0, 1);
                a = a.Remove(a.Length - 1, 1);
                return a;
            }
        }
        private string toErmenice(string m)
        {
            using (WebClient wb = new WebClient())
            {
                wb.Encoding = Encoding.UTF8;
                string a = wb.DownloadString("http://translate.yandex.net/tr.json/translate?lang=en-hy&text=" + m.Replace(" ", "+") + "&srv=tr-text");
                a = a.Remove(0, 1);
                a = a.Remove(a.Length - 1, 1);
                return a;
            }
        }
        private string toItalyanca(string m)
        {
            developerkey = "AI39si5fKy1z4ipV-LqVE6XFXi0F04otsTdBPfuHxZXYJDRXkKxgSRRKyYiiJnaUdksNiUJWn_KyXLBQOig7H8P4XoegqT59Gw";
            hesapadi = "hartleylucas91@gmail.com";
            hesapsifresi = "6528yayla";
            using (WebClient wb = new WebClient())
            {
                wb.Encoding = Encoding.UTF8;
                string a = wb.DownloadString("http://translate.yandex.net/tr.json/translate?lang=en-it&text=" + m.Replace(" ", "+") + "&srv=tr-text");
                a = a.Remove(0, 1);
                a = a.Remove(a.Length - 1, 1);
                return a;
            }
        }
        private string toGurcuce(string m)
        {
            using (WebClient wb = new WebClient())
            {
                wb.Encoding = Encoding.UTF8;
                string a = wb.DownloadString("http://translate.yandex.net/tr.json/translate?lang=en-ka&text=" + m.Replace(" ", "+") + "&srv=tr-text");
                a = a.Remove(0, 1);
                a = a.Remove(a.Length - 1, 1);
                return a;
            }
        }
        private string toLitvanca(string m)
        {
            using (WebClient wb = new WebClient())
            {
                wb.Encoding = Encoding.UTF8;
                string a = wb.DownloadString("http://translate.yandex.net/tr.json/translate?lang=en-lt&text=" + m.Replace(" ", "+") + "&srv=tr-text");
                a = a.Remove(0, 1);
                a = a.Remove(a.Length - 1, 1);
                return a;
            }
        }
        private string toLetonca(string m)
        {
            using (WebClient wb = new WebClient())
            {
                wb.Encoding = Encoding.UTF8;
                string a = wb.DownloadString("http://translate.yandex.net/tr.json/translate?lang=en-lv&text=" + m.Replace(" ", "+") + "&srv=tr-text");
                a = a.Remove(0, 1);
                a = a.Remove(a.Length - 1, 1);
                return a;
            }
        }
        private string toMakedonca(string m)
        {
            using (WebClient wb = new WebClient())
            {
                wb.Encoding = Encoding.UTF8;
                string a = wb.DownloadString("http://translate.yandex.net/tr.json/translate?lang=en-mk&text=" + m.Replace(" ", "+") + "&srv=tr-text");
                a = a.Remove(0, 1);
                a = a.Remove(a.Length - 1, 1);
                return a;
            }
        }
        private string toHollandaca(string m)
        {
            using (WebClient wb = new WebClient())
            {
                wb.Encoding = Encoding.UTF8;
                string a = wb.DownloadString("http://translate.yandex.net/tr.json/translate?lang=en-nl&text=" + m.Replace(" ", "+") + "&srv=tr-text");
                a = a.Remove(0, 1);
                a = a.Remove(a.Length - 1, 1);
                return a;
            }
        }
        private string toNorvecce(string m)
        {
            developerkey = "AI39si6Jpal9JXuK-7Pt-Dgqi5hWY0WrBN7aMlyBG2cNIh8hR4N5QCnUSEeqFmnFjgknZnAU0X2NcoNLxK4hRbJjBowuZPF4UQ";
            hesapadi = "archien429@gmail.com";
            hesapsifresi = "6528yayla";
            using (WebClient wb = new WebClient())
            {
                wb.Encoding = Encoding.UTF8;
                string a = wb.DownloadString("http://translate.yandex.net/tr.json/translate?lang=en-no&text=" + m.Replace(" ", "+") + "&srv=tr-text");
                a = a.Remove(0, 1);
                a = a.Remove(a.Length - 1, 1);
                return a;
            }
        }
        private string toPolonca(string m)
        {
            using (WebClient wb = new WebClient())
            {
                wb.Encoding = Encoding.UTF8;
                string a = wb.DownloadString("http://translate.yandex.net/tr.json/translate?lang=en-pl&text=" + m.Replace(" ", "+") + "&srv=tr-text");
                a = a.Remove(0, 1);
                a = a.Remove(a.Length - 1, 1);
                return a;
            }
        }
        private string toPortekizce(string m)
        {
            using (WebClient wb = new WebClient())
            {
                wb.Encoding = Encoding.UTF8;
                string a = wb.DownloadString("http://translate.yandex.net/tr.json/translate?lang=en-pt&text=" + m.Replace(" ", "+") + "&srv=tr-text");
                a = a.Remove(0, 1);
                a = a.Remove(a.Length - 1, 1);
                return a;
            }
        }
        private string toRomanca(string m)
        {
            using (WebClient wb = new WebClient())
            {
                wb.Encoding = Encoding.UTF8;
                string a = wb.DownloadString("http://translate.yandex.net/tr.json/translate?lang=en-ro&text=" + m.Replace(" ", "+") + "&srv=tr-text");
                a = a.Remove(0, 1);
                a = a.Remove(a.Length - 1, 1);
                return a;
            }
        }
        private string toRusca(string m)
        {
            using (WebClient wb = new WebClient())
            {
                wb.Encoding = Encoding.UTF8;
                string a = wb.DownloadString("http://translate.yandex.net/tr.json/translate?lang=en-ru&text=" + m.Replace(" ", "+") + "&srv=tr-text");
                a = a.Remove(0, 1);
                a = a.Remove(a.Length - 1, 1);
                return a;
            }
        }
        private string toSlovakca(string m)
        {
            using (WebClient wb = new WebClient())
            {
                wb.Encoding = Encoding.UTF8;
                string a = wb.DownloadString("http://translate.yandex.net/tr.json/translate?lang=en-sk&text=" + m.Replace(" ", "+") + "&srv=tr-text");
                a = a.Remove(0, 1);
                a = a.Remove(a.Length - 1, 1);
                return a;
            }
        }
        private string toSlovence(string m)
        {
            developerkey = "AI39si5f5ycePTeH8Eev0gvDMTcabkaCjG5heu8RUsN9bdx4e1902ccMcA2zUfgOdBshyyQVMLZPMcTI5Y7S5TomdCrkfhDx8g";
            hesapadi = "chancock349@gmail.com";
            hesapsifresi = "6528yayla";
            using (WebClient wb = new WebClient())
            {
                wb.Encoding = Encoding.UTF8;
                string a = wb.DownloadString("http://translate.yandex.net/tr.json/translate?lang=en-sl&text=" + m.Replace(" ", "+") + "&srv=tr-text");
                a = a.Remove(0, 1);
                a = a.Remove(a.Length - 1, 1);
                return a;
            }
        }
        private string toArnavutca(string m)
        {
            using (WebClient wb = new WebClient())
            {
                wb.Encoding = Encoding.UTF8;
                string a = wb.DownloadString("http://translate.yandex.net/tr.json/translate?lang=en-sq&text=" + m.Replace(" ", "+") + "&srv=tr-text");
                a = a.Remove(0, 1);
                a = a.Remove(a.Length - 1, 1);
                return a;
            }
        }
        private string toSirpca(string m)
        {
            using (WebClient wb = new WebClient())
            {
                wb.Encoding = Encoding.UTF8;
                string a = wb.DownloadString("http://translate.yandex.net/tr.json/translate?lang=en-sr&text=" + m.Replace(" ", "+") + "&srv=tr-text");
                a = a.Remove(0, 1);
                a = a.Remove(a.Length - 1, 1);
                return a;
            }
        }
        private string toIsvecce(string m)
        {
            using (WebClient wb = new WebClient())
            {
                wb.Encoding = Encoding.UTF8;
                string a = wb.DownloadString("http://translate.yandex.net/tr.json/translate?lang=en-sv&text=" + m.Replace(" ", "+") + "&srv=tr-text");
                a = a.Remove(0, 1);
                a = a.Remove(a.Length - 1, 1);
                return a;
            }
        }
        private string toUkraynaca(string m)
        {
            using (WebClient wb = new WebClient())
            {
                wb.Encoding = Encoding.UTF8;
                string a = wb.DownloadString("http://translate.yandex.net/tr.json/translate?lang=en-uk&text=" + m.Replace(" ", "+") + "&srv=tr-text");
                a = a.Remove(0, 1);
                a = a.Remove(a.Length - 1, 1);
                return a;
            }
        }
        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            string firstVideoFilePath = @"C:\a.avi";
            string secondVideoFilePath = @"C:\b.avi";
            string outputVideoPath = @"C:\output.avi";

            using (ITimeline timeline = new DefaultTimeline())
            {
                timeline.AddAudioGroup();
                IGroup group = timeline.AddVideoGroup(32, 600, 400);

                var firstVideoClip = group.AddTrack().AddVideo(firstVideoFilePath);
                var secondVideoClip = group.AddTrack().AddVideo(secondVideoFilePath, firstVideoClip.Duration);

                using(WindowsMediaRenderer renderer = new WindowsMediaRenderer(timeline, outputVideoPath,WindowsMediaProfiles.HighQualityVideo))
                {
                    renderer.Render();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            while (!asd())
                Application.DoEvents();
            MessageBox.Show("bitti");
        }
        private bool asd()
        {
            int i = 0;
            while (i < 100000)
            {
                richTextBox1.Text = i.ToString();
                i++;
            }
            return true;
        }
    }
}
