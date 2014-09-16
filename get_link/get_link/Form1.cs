using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Threading;
using System.IO;

namespace get_link
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<string> link = new List<string>();
            string q = "";
            for (int i = 50; i >= 1; i--)
            {
                this.Text = i + " Sayfa";
                using (var client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                    q = client.DownloadString("http://www.liveleak.com/browse?featured=1&page="+i);
                }

                string[] a = q.Split(new string[] { "<h2><a href=\"" }, StringSplitOptions.None);
                for (int j = 1; j < a.Length; j++)
                {
                    string[] b = a[j].Split('"');
                    link.Add(b[0] + "|4");
                }
            }



            System.IO.File.WriteAllLines(@"C:\liveleak.txt", link);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<string> link = new List<string>();
            string q = "";
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                q = client.DownloadString("http://rutv.ru/brand/brandlist");
            }

            string[] a = q.Split(new string[] { "<div class=\"video-list-item-img\"><a href=\"" }, StringSplitOptions.None);
            for (int j = 1; j < a.Length; j++)
            {
                string[] b = a[j].Split('"');
                if (b[0].Contains("brand/show/id/"))
                    link.Add("http://rutv.ru"+b[0]);
            }

            System.IO.File.WriteAllLines(@"C:\rutv.txt", link);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Thread a = new Thread(new ThreadStart(StupidVideosLastMonthViews5Page));
            a.Start();
        }
        private void StupidVideosLastMonthViews5Page()
        {
            this.Text = "başladı";
            List<string> link = new List<string>();
                this.Text = "forda";
                string q = "";
                using (var client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                    q = client.DownloadString("http://www.stupidvideos.com/videos/all/popular/all/6/");
                }

                string[] a = q.Split(new string[] { "<div class=\"block\"><a href=\"" }, StringSplitOptions.None);
                for (int j = 1; j < a.Length; j++)
                {
                    string[] b = a[j].Split('"');
                    link.Add("http://www.stupidvideos.com" + b[0] + "|3");
                }
                using (var client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                    q = client.DownloadString("http://www.stupidvideos.com/videos/all/popular/all/7/");
                }

                string[] aa = q.Split(new string[] { "<div class=\"block\"><a href=\"" }, StringSplitOptions.None);
                for (int j = 1; j < aa.Length; j++)
                {
                    string[] b = aa[j].Split('"');
                    link.Add("http://www.stupidvideos.com" + b[0] + "|3");
                }
                using (var client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                    q = client.DownloadString("http://www.stupidvideos.com/videos/all/popular/all/8/");
                }

                string[] aaa = q.Split(new string[] { "<div class=\"block\"><a href=\"" }, StringSplitOptions.None);
                for (int j = 1; j < aaa.Length; j++)
                {
                    string[] b = aaa[j].Split('"');
                    link.Add("http://www.stupidvideos.com" + b[0] + "|3");
                }
                using (var client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                    q = client.DownloadString("http://www.stupidvideos.com/videos/all/popular/all/9/");
                }

                string[] aaaa = q.Split(new string[] { "<div class=\"block\"><a href=\"" }, StringSplitOptions.None);
                for (int j = 1; j < aaaa.Length; j++)
                {
                    string[] b = aaaa[j].Split('"');
                    link.Add("http://www.stupidvideos.com" + b[0] + "|3");
                }
                using (var client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                    q = client.DownloadString("http://www.stupidvideos.com/videos/all/popular/all/10/");
                }
                string[] aaaaa = q.Split(new string[] { "<div class=\"block\"><a href=\"" }, StringSplitOptions.None);
                for (int j = 1; j < aaaaa.Length; j++)
                {
                    string[] b = aaaaa[j].Split('"');
                    link.Add("http://www.stupidvideos.com" + b[0] + "|3");
                }
            System.IO.File.WriteAllLines(@"C:\liveleak.txt", link);
            this.Text = "bitti";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            List<string> link = new List<string>();
            string q = "";
            for (int i = 50; i >= 1; i--)
            {
                this.Text = i + " Sayfa";
                using (var client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                    q = client.DownloadString("http://www.videobash.com/videos/all/mv/?page=" + i);
                }

                string[] a = q.Split(new string[] { "<h3><a href=\"" }, StringSplitOptions.None);
                for (int j = 1; j < a.Length; j++)
                {
                    string[] b = a[j].Split('"');
                    link.Add(b[0] + "|3");
                }
            }
            System.IO.File.WriteAllLines(@"C:\videobash.txt", link);
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            List<string> link = new List<string>();
            string q = "";
            /*for (int i = 50; i >= 1; i--)
            {
                this.Text = i + " Sayfa";
                using (var client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                    q = client.DownloadString("http://www.killsometime.com/videos/views/" + i);
                }

                string[] a = q.Split(new string[] { "<article class=\"MediaList-Article-WhatsNew\">" }, StringSplitOptions.None);
                for (int j = 1; j < a.Length; j++)
                {
                    string[] b = a[j].Split(new string[]{"<a class=\"Article\" href=\""},StringSplitOptions.None);
                    string[] c = b[1].Split('"');

                    link.Add("http://www.killsometime.com"+c[0] + "|3");
                }
            }
            System.IO.File.WriteAllLines(@"C:\killsometime.txt", link);*/

            link.Clear();
            for (int i = 50; i >= 1; i--)
            {
                try
                {
                    this.Text = i + " Sayfa";
                    using (var client = new WebClient())
                    {
                        client.Encoding = Encoding.UTF8;
                        client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                        q = client.DownloadString("http://www.vidwor.ru/ratings/?filt=month&sort=shows&PAGEN_1=" + i);
                    }
                    string[] a = q.Split(new string[] { "<div class=\"block-lite top-block\"" }, StringSplitOptions.None);
                    string[] b = a[1].Split(new string[] { "<div class=\"navigation\">" }, StringSplitOptions.None);
                    string[] c = b[0].Split(new string[] { "<div class=\"b-video-wrapper\" >" }, StringSplitOptions.None);
                    for (int j = 1; j < c.Length; j++)
                    {
                        string[] b1 = c[j].Split(new string[] { "<a href=\"" }, StringSplitOptions.None);
                        string[] c1 = b1[1].Split('"');
                        link.Add("http://www.vidwor.ru" + c1[0] + "|1");
                    }
                }
                catch { }
            }
            System.IO.File.WriteAllLines(@"C:\vidwor.txt", link);

            link.Clear();


            for (int i = 50; i >= 1; i--)
            {
                this.Text = i + " Sayfa";
                using (var client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                    q = client.DownloadString("http://video.day.az/video-popular-page-" + i);
                }
                string[] a = q.Split(new string[] { "<div class=\"video\" >" }, StringSplitOptions.None);
                for (int j = 1; j < a.Length; j++)
                {
                    string[] b1 = a[j].Split(new string[] { "<a href=\"" }, StringSplitOptions.None);
                    string[] c1 = b1[1].Split('"');
                    link.Add("http://www.vidwor.ru" + c1[0] + "|1");
                }
            }
            System.IO.File.WriteAllLines(@"C:\day-az.txt", link);


            link.Clear();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            List<string> link = new List<string>();
            string q = "";
            for (int i = 100; i >= 98; i--)
            {
                this.Text = i + " Sayfa";
                using (var client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                    q = client.DownloadString("http://www.metacafe.com/today/page-" + i+"/");
                }
                try
                {
                    string[] a = q.Split(new string[] { "<li id=\"Previous\" ><a title=\"Browse to previous page\"" }, StringSplitOptions.None);
                    string[] b = a[1].Split(new string[] { "<a href=\"/watch/" }, StringSplitOptions.None);
                    for (int j = 1; j < b.Length; j++)
                    {
                        string[] b2 = b[j].Split('"');
                        link.Add("http://www.metacafe.com/watch/" + b2[0] + "|1");
                    }
                }
                catch { }
            }
            System.IO.File.WriteAllLines(@"C:\metacafe.txt", link);
            /*link.Clear();

            for (int i = 100; i >= 1; i--)
            {
                this.Text = i + " Sayfa";
                using (var client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                    q = client.DownloadString("http://www.dailymotion.com/user/5minMusic/" + i);
                }
                string[] a = q.Split(new string[] { "preview_link white_border \"  href=\"" }, StringSplitOptions.None);
                for (int j = 1; j < a.Length; j++)
                {
                    string[] b = a[j].Split('"');
                    link.Add("http://www.dailymotion.com" + b[0] + "|1");
                }
            }
            System.IO.File.WriteAllLines(@"C:\5minMusic.txt", link);

            link.Clear();

            for (int i = 100; i >= 1; i--)
            {
                this.Text = i + " Sayfa";
                using (var client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                    q = client.DownloadString("http://www.dailymotion.com/user/5minFashion/" + i);
                }
                string[] a = q.Split(new string[] { "preview_link white_border \"  href=\"" }, StringSplitOptions.None);
                for (int j = 1; j < a.Length; j++)
                {
                    string[] b = a[j].Split('"');
                    link.Add("http://www.dailymotion.com" + b[0] + "|1");
                }
            }
            System.IO.File.WriteAllLines(@"C:\5minFashion.txt", link);

            link.Clear();

            for (int i = 3; i >= 1; i--)
            {
                this.Text = i + " Sayfa";
                using (var client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                    q = client.DownloadString("http://www.dailymotion.com/user/kuafor58/" + i);
                }
                string[] a = q.Split(new string[] { "preview_link white_border \"  href=\"" }, StringSplitOptions.None);
                for (int j = 1; j < a.Length; j++)
                {
                    string[] b = a[j].Split('"');
                    link.Add("http://www.dailymotion.com" + b[0] + "|1");
                }
            }
            System.IO.File.WriteAllLines(@"C:\kuafor58.txt", link);*/


        }

        private void button7_Click(object sender, EventArgs e)
        {

        }
        private void VideoDownload(string url)
        {
            try
            {
                Random a = new Random();
                string video = a.Next(10000000, 99999999).ToString();
                string name = video + ".flv";
                string videoismi = video + ".flv";
                WebClient b = new WebClient();
                //b.Proxy = null;
                b.DownloadFile(url, name);
            }
            catch { }
        }
    }
}
