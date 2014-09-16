using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Threading;
using Google.YouTube;
using Google.GData.Client;
using Google.GData.Extensions.MediaRss;
using Google.GData.YouTube;

namespace solo_video_upload
{
    public partial class Form1 : Form
    {
        string videoismi;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread a = new Thread(new ThreadStart(basla));
            a.Start();
        }
        private void basla()
        {
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                string[] pi = listBox1.Items[i].ToString().Split('|');
                listBox1.SetSelected(i, true);
                DM(pi[0]);
                VideoDownload(txvideo.Text);
                YoutubeUpload();
                VideoDelete();
            }
        }
        private void DM(string site)
        {


            string q = "";
            string[] cut4 = new string[5];
            Array.Clear(cut4, 0, cut4.Length);
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                q = client.DownloadString(site);
            }

            if (q.Contains("video_url%22%3A%22"))
            {
                string[] a = q.Split(new string[] { "video_url%22%3A%22" }, StringSplitOptions.None);
                string[] b = a[1].Split(new string[] { "%22%2C%22daily_video_link" }, StringSplitOptions.None);
                b[0] = b[0].Replace("%253A", ":").Replace("%252F", "/").Replace("%253F", "?").Replace("%253D", "=");
                txvideo.Text = b[0];
            }
            else
            {
                string[] cut = q.Split(new string[] { "www.dailymotion.com%5C%2Fcdn%5C%2Fmanifest%5C%2Fvideo%5C%2F" }, StringSplitOptions.None);
                string[] cut2 = cut[1].Split(new string[] { "%22%2C%22" }, StringSplitOptions.None);
                cut2[0] = cut2[0].Replace("%5C%2F", "/").Replace("%3F", "?").Replace("%3D", "=").Replace("%3A", ":").Replace("mnft", "mp4");
                txvideo.Text = "http://www.dailymotion.com/cdn/H264-512x384/video/" + cut2[0];
            }


            string[] title = q.Split(new string[] { "<title>" }, StringSplitOptions.None);
            string[] title2 = title[1].Split(new string[] { "</title>" }, StringSplitOptions.None);
            title2[0] = title2[0].Replace("- Video Dailymotion", "");
            title2[0] = title2[0].Replace(" - Dailymotion video", "");
            string[] desc = q.Split(new string[] { "og:description\" content=\"" }, StringSplitOptions.None);
            string[] desc2 = desc[1].Split('"');
            desc2[0] = desc2[0].Replace(" - Dailymotion video", "");
            txvideoadi.Text = HttpUtility.HtmlDecode(title2[0]);
            txaciklama.Text = HttpUtility.HtmlDecode(title2[0]);
            


            string[] q2 = txvideoadi.Text.Split(' ');
            foreach (string b in q2)
            {
                if (b.Length > 2)
                    txetiket.Text += b + ",";
            }
            for (int ke = 0; ke < q2.Length; ke++)
            {
                for (int le = 0; le < q2.Length; le++)
                {
                    if (!q2[ke].Equals(q[le]) && q2[ke].Length > 2)
                    {
                        txetiket.Text += q2[le] + " " + q2[ke] + ",";
                    }
                }
            }


        }

        private void VideoDelete()
        {
                this.Text = "Yüklenen Video Silindi";
                string FileToDelete;
                // Set full path to file 
                FileToDelete = videoismi;
                // Delete a file
                File.Delete(FileToDelete);
        }
        private void VideoDownload(string url)
        {
                this.Text = "Video İndiriliyor..";
                videoismi = "";
                Random a = new Random();
                string video = a.Next(10000000, 99999999).ToString();
                string name = video + ".flv";
                videoismi = video + ".flv";
                WebClient b = new WebClient();
                b.DownloadFile(url, name);
        }
        private void YoutubeUpload()
        {
            bool aa = true;
            this.Text = "Video Yükleniyor..";


            try
            {
                YouTubeRequestSettings settings = new YouTubeRequestSettings("Contentor", txdevkey.Text, txid.Text, txpw.Text);
                YouTubeRequest request = new YouTubeRequest(settings);


                Video newVideo = new Video();
                ((GDataRequestFactory)request.Service.RequestFactory).Timeout = 9999999;
                newVideo.Title = txvideoadi.Text;
                newVideo.Tags.Add(new MediaCategory("Entertainment", YouTubeNameTable.CategorySchema));
                newVideo.Keywords = txetiket.Text;
                newVideo.Description = txaciklama.Text;
                newVideo.YouTubeEntry.Private = false;
                newVideo.YouTubeEntry.MediaSource = new MediaFileSource(videoismi, "video/x-flv");

                request.Upload(newVideo);
            }
            catch (Exception ex)
            {
                richTextBox1.Text += ex.ToString();
                this.Text = "Hata.";
                aa = false;
            }
            if (aa)
                label3.Text = (Convert.ToInt16(label3.Text) + 1).ToString();
            else
                label5.Text = (Convert.ToInt16(label5.Text) + 1).ToString();
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Text Dosyası Seçiniz ( Birden çok dosya seçebilirsiniz )";
            dialog.Filter = " (*.txt)|*.txt";
            dialog.FilterIndex = 1;
            dialog.Multiselect = true;
            DialogResult a = dialog.ShowDialog();
            if (a == DialogResult.OK)
                foreach (string str in dialog.FileNames)
                {
                    /*TextReader read = new StreamReader(str);
                    textBox1.Text += read.ReadToEnd();
                    string[] sıra = read.
                    read.Close();*/
                    FileInfo file = new FileInfo(str);
                    StreamReader stRead = file.OpenText();
                    while (!stRead.EndOfStream)
                    {
                        listBox1.Items.Add(stRead.ReadLine());
                    }
                }
            label2.Text = listBox1.Items.Count.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.IO.File.WriteAllLines(@"C:\a.txt", richTextBox1.Lines);
            Environment.Exit(0);
        }
    }
}
