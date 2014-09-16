using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using Google.YouTube;
using Google.GData.Client;
using Google.GData.Extensions.MediaRss;
using Google.GData.YouTube;
using System.Threading;

namespace BotLiveLeak
{
    public partial class Form1 : Form
    {
        string videoismi, videoadi, aciklama, etiket, developerkey, hesapadi, hesapsifresi;
        int sec = 120;
        Thread liveleak;
        bool bekle300 = false;
        public Form1()
        {
            InitializeComponent();
        }
        const string HTML_TAG_PATTERN = "<.*?>";
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
                    etiket += b.Trim() + ",";
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

            if (checkBox2.Checked)
                etiket += textBox4.Text;
            if (etiket[0] == ',')
                etiket.Remove(0, 1);
            if (etiket[etiket.Length - 1] == ',')
                etiket.Remove(etiket.Length - 1, 1);
        }
        static string StripHTML(string inputString)
        {
            return Regex.Replace
              (inputString, HTML_TAG_PATTERN, string.Empty);
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
        private void YoutubeUpload()
        {
            hesapadi = textBox1.Text;
            hesapsifresi = textBox2.Text;
            developerkey = textBox3.Text;
            try
            {
                listBox1.Items.Add(DateTime.Now.ToString() + " Video Yükleniyor..");
                Random a = new Random();
                string id = a.Next(100000, 999999).ToString();
                YouTubeRequestSettings settings = new YouTubeRequestSettings(id, developerkey, hesapadi, hesapsifresi);
                YouTubeRequest request = new YouTubeRequest(settings);

                Video newVideo = new Video();
                ((GDataRequestFactory)request.Service.RequestFactory).Timeout = 9999999;
                newVideo.Title = videoadi;
                newVideo.Tags.Add(new MediaCategory("News", YouTubeNameTable.CategorySchema));
                newVideo.Keywords = etiket;
                if (checkBox1.Checked)
                    aciklama += richTextBox2.Text;
                newVideo.Description = aciklama;
                newVideo.YouTubeEntry.Private = false;
                newVideo.YouTubeEntry.MediaSource = new MediaFileSource(videoismi, "video/x-flv");

                request.Upload(newVideo);
                listBox1.Items.Add(DateTime.Now.ToString() + "   " + videoismi);
            }
            catch (Exception ex)
            {
                richTextBox1.Text += ex.ToString();
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
            listBox1.Items.Add(DateTime.Now.ToString() + " Video İndiriliyor..");
            videoismi = site + "_" + a.Next(10000000, 99999999).ToString() + ".flv";
            using (WebClient wb = new WebClient())
            {
                wb.DownloadFile(url, videoismi);
            }
        }
        private bool Kontrol(string s)
        {
            if (File.Exists("links.txt"))
            {
                string[] a = File.ReadAllLines("links.txt");
                if (a.Contains(s))
                    return true;
                else
                    return false;
            }
            return false;

        }
        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Kontrol("asd").ToString());
        }
        private void LiveLeak()
        {
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
                s1 = GetSource(s + "&safe_mode=off");
                if (!Kontrol(s) && s1.Contains("file: \""))
                {
                    if(bekle300)
                        Wait300();
                    if (!bekle300)
                        bekle300 = true;
                    s1 = GetSource(s + "&safe_mode=off");
                    string[] cut = s1.Split(new string[] { "file: \"" }, StringSplitOptions.None);
                    string[] cut2 = cut[1].Split(new string[] { "\"," }, StringSplitOptions.None);
                    string[] title = s1.Split(new string[] { "style=\"vertical-align:top; padding-right:10px\">" }, StringSplitOptions.None);
                    string[] title2 = title[1].Split('<');
                    string[] desc = s1.Split(new string[] { "<div id=\"body_text\"><p>" }, StringSplitOptions.None);
                    string[] desc2 = desc[1].Split(new[] { "<style>" }, StringSplitOptions.None);
                    cut2[0] = cut2[0] + "&ec_seek=0";

                    videoadi = WebUtility.HtmlDecode(title2[0]);
                    aciklama = videoadi +" "+StripHTML(WebUtility.HtmlDecode(desc2[0])).Trim();
                    if (cut2[0].Contains("rtmp"))
                        continue;
                    Etiket(videoadi);
                    richTextBox1.Text += videoadi + Environment.NewLine + aciklama + Environment.NewLine + etiket + Environment.NewLine + cut2[0] + Environment.NewLine;
                    VideoDownload(cut2[0], "liveleak");
                    YoutubeUpload();
                    VideoDelete();
                    File.AppendAllText("links.txt", s + Environment.NewLine);
                }
            }
            bekle300 = false;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Text = "Liveleak Bot - Çalışıyor.";
            liveleak = new Thread(new ThreadStart(LiveLeak));
            liveleak.Start();
            timer1.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (File.Exists("acc.txt"))
                File.Delete("acc.txt");
            File.AppendAllText("acc.txt", textBox1.Text + Environment.NewLine + textBox2.Text + Environment.NewLine + textBox3.Text);
            MessageBox.Show("Kaydedildi.");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /*if (GetSource("http://voyl.webuda.com/check.html").Contains("block"))
            {
                MessageBox.Show("Kullanma İzniniz Yok.");
                Environment.Exit(0);
            }*/
            bek = Convert.ToInt32(numericUpDown1.Value);
            CheckForIllegalCrossThreadCalls = false;
            if (File.Exists("acc.txt"))
            {
                string[] a = File.ReadAllLines("acc.txt");
                hesapadi = a[0];
                hesapsifresi = a[1];
                developerkey = a[2];
                textBox1.Text = a[0];
                textBox2.Text = a[1];
                textBox3.Text = a[2];
            }
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            /*if (sec == 0)
            {
                if (!bg.IsBusy)
                {
                    bg.RunWorkerAsync();
                    toolStripStatusLabel2.Text = "Program Çalıştırıldı. " + DateTime.Now.ToString();
                }
                else
                    toolStripStatusLabel2.Text = "Program Çalışıyor. " + DateTime.Now.ToString();

                sec = 120;
            }*/
            if (sec == 0)
            {
                if (liveleak.ThreadState.ToString() != "Running")
                {
                    liveleak = new Thread(new ThreadStart(LiveLeak));
                    liveleak.Start();
                    this.Text = "Liveleak Bot - Çalışıyor.";
                }
                else
                    this.Text = "Liveleak Bot - Bot Hâlâ Çalışıyor.. Lütfen Bitmesini Bekleyin..";
                sec = 120;
            }
            sec--;
            toolStripStatusLabel1.Text = sec.ToString();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled)
            {
                timer1.Enabled = false;
                if (liveleak.CurrentCulture.ToString() != "Running")
                {
                    liveleak = new Thread(new ThreadStart(LiveLeak));
                    liveleak.Start();
                }
            }
            else
                MessageBox.Show("Bot Zaten Çalışmıyor..");
        }
        int bek = 1;
        private void Wait300()
        {
            listBox1.Items.Add(bek+" Saniye Bekleniyor..");
            timer2.Enabled = true;
            while (bek > 0)
                Application.DoEvents();
            bek = Convert.ToInt32(numericUpDown1.Value);
            timer2.Enabled = false;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            this.Text = "LiveLeak Bot - "+bek + " Saniye Bekleniyor..";
            bek--;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            bek = Convert.ToInt32(numericUpDown1.Value);
            label8.Text = bek.ToString();
        }
        private void bg_DoWork(object sender, DoWorkEventArgs e)
        {
            List<string> f = new List<string>();
            string s1 = GetSource("http://www.liveleak.com/browse");
            string[] a = s1.Split(new string[] { "id=\"favorite-" }, StringSplitOptions.None);
            for (int i = 1; i < a.Length; i++)
            {
                string[] b = a[i].Split('"');
                f.Add("http://www.liveleak.com/view?i=" + b[0]);
            }
            foreach (string s in f)
            {
                s1 = GetSource(s + "&safe_mode=off");
                if (!Kontrol(s) && s1.Contains("file: \""))
                {
                    if (bekle300)
                        Wait300();
                    if (!bekle300)
                        bekle300 = true;
                    s1 = GetSource(s + "&safe_mode=off");
                    string[] cut = s1.Split(new string[] { "file: \"" }, StringSplitOptions.None);
                    string[] cut2 = cut[1].Split(new string[] { "\"," }, StringSplitOptions.None);
                    string[] title = s1.Split(new string[] { "style=\"vertical-align:top; padding-right:10px\">" }, StringSplitOptions.None);
                    string[] title2 = title[1].Split('<');
                    string[] desc = s1.Split(new string[] { "<div id=\"body_text\"><p>" }, StringSplitOptions.None);
                    string[] desc2 = desc[1].Split(new[] { "<style>" }, StringSplitOptions.None);
                    cut2[0] = cut2[0] + "&ec_seek=0";

                    videoadi = WebUtility.HtmlDecode(title2[0]);
                    aciklama = videoadi + " " + StripHTML(WebUtility.HtmlDecode(desc2[0])).Trim();
                    if (cut2[0].Contains("rtmp"))
                        continue;
                    Etiket(videoadi);
                    richTextBox1.Text += videoadi + Environment.NewLine + aciklama + Environment.NewLine + etiket + Environment.NewLine + cut2[0] + Environment.NewLine;
                    VideoDownload(cut2[0], "liveleak");
                    YoutubeUpload();
                    VideoDelete();
                    File.AppendAllText("links.txt", s + Environment.NewLine);
                }
            }
            bekle300 = false;
        }
    }
}
