using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using MySql.Data.MySqlClient;
using System.Threading;
using System.Security.Cryptography;
using System.Diagnostics;
using YouTube_Downloader;
using Google.YouTube;
using System.Text.RegularExpressions;
using uploader;
using Google.GData.Client;
using Google.GData.Extensions.MediaRss;
using Google.GData.YouTube;


namespace webpage_freezone
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        List<YouTubeVideoQuality> downVideoUrls = new List<YouTubeVideoQuality>();
        string hata;
        bool youtubesonuc = true;
        bool giris = false;
        string[] videoUrls;
        string videoismi;
        private string VideoAdı;
        private string etiketler;
        private string Açıklama;

        private string hesapadi;
        private string hesapsifresi;
        private string developerkey;
        private bool ucretsiz = false;

        static readonly string PasswordHash = "oEH~W)D^";
        static readonly string SaltKey = "|g*^2iEM";
        static readonly string VIKey = "dEX(93{I^/%hUX3J";
        static readonly string auth = "oddiP9Jo8kcJR4sUoJZblg==";
        static readonly string Version = "v6";


        #region Türkçe Siteler

        private void izlesene(string site)//return video link|video title|description
        {
            try
            {

                string q = "";

                using (var client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                    q = client.DownloadString(site);
                }
                string[] cut = q.Split(new string[] { "streamurl:\"" }, StringSplitOptions.None);
                string[] cut2 = cut[1].Split('"');
                string[] title = q.Split(new string[] { "<title>" }, StringSplitOptions.None);
                string[] title2 = title[1].Split(new string[] { "</title>" }, StringSplitOptions.None);

                string[] desc = q.Split(new string[] { "og:description\" content=\"" }, StringSplitOptions.None);
                string[] desc2 = desc[1].Split('"');
                txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
                txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
                txvideo.Text = cut2[0];
            }
            catch { }

        }
        private void TvHaber(string site)//return video link|video title|description
        {
            try
            {

                string q = "";

                WebRequest req = HttpWebRequest.Create(site);
                req.Method = "GET";
                using (StreamReader reader = new StreamReader(req.GetResponse().GetResponseStream(), Encoding.UTF8))
                {
                    q = reader.ReadToEnd();
                }
                string[] cut = q.Split(new string[] { "&strSource=" }, StringSplitOptions.None);
                string[] cut2 = cut[1].Split(new string[] { "&doubleClick" }, StringSplitOptions.None);
                string[] title = q.Split(new string[] { "<title>" }, StringSplitOptions.None);
                string[] title2 = title[1].Split(new string[] { "</title>" }, StringSplitOptions.None);

                string[] desc = q.Split(new string[] { "\"og:description\" content=\"" }, StringSplitOptions.None);
                string[] desc2 = desc[1].Split(new string[] { "\" />" }, StringSplitOptions.None);
                desc2[0] = desc2[0].Replace("&nbsp;", "");
                desc2[0] = desc2[0].Replace("&ccedil;", "ç");
                txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
                txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
                txvideo.Text = cut2[0];
            }
            catch { }
        }
        private void videoizleco(string site)//return video link|video title|description
        {
            try
            {

                string q = "";

                WebRequest req = HttpWebRequest.Create(site);
                req.Method = "GET";
                using (StreamReader reader = new StreamReader(req.GetResponse().GetResponseStream(), Encoding.UTF8))
                {
                    q = reader.ReadToEnd();
                }
                string[] cut = q.Split(new string[] { "file		: '" }, StringSplitOptions.None);
                string[] cut2 = cut[1].Split(new string[] { "'," }, StringSplitOptions.None);
                string[] title = q.Split(new string[] { "<h1>" }, StringSplitOptions.None);
                string[] title2 = title[2].Split(new string[] { "</h1>" }, StringSplitOptions.None);
                string[] desc = q.Split(new string[] { "class=\"aciklama\">" }, StringSplitOptions.None);
                string[] desc2 = desc[1].Split(new string[] { "</p>" }, StringSplitOptions.None);

                txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
                txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
                txvideo.Text = cut2[0];
            }
            catch { }
        }
        private void Timsah(string site)//return video link|video title|description
        {
            try
            {

                string q = "";

                WebRequest req = HttpWebRequest.Create(site);
                req.Method = "GET";
                using (StreamReader reader = new StreamReader(req.GetResponse().GetResponseStream(), Encoding.UTF8))
                {
                    q = reader.ReadToEnd();
                }
                string[] cut = q.Split(new string[] { "<div id=\\\"bant\\\"" }, StringSplitOptions.None);
                string[] cut2 = cut[1].Split(new string[] { "(0)',file:'" }, StringSplitOptions.None);
                string[] cut3 = cut2[1].Split(new string[] { "'" }, StringSplitOptions.None);
                string[] title = q.Split(new string[] { "property=\"og:site_name\" name=\"og:site_name\" /><meta content=\"" }, StringSplitOptions.None);
                string[] title2 = title[1].Split(new string[] { "\" property=\"og:title\"" }, StringSplitOptions.None);
                string[] desc = q.Split(new string[] { "videoDescriptionShort\">" }, StringSplitOptions.None);
                string[] desc2 = desc[1].Split(new string[] { "</p>" }, StringSplitOptions.None);

                txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
                txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
                txvideo.Text = cut3[0];
            }
            catch { }
        }
        private void IHA(string site)//return video link|video title|description
        {
            try
            {

                string q = "";

                WebRequest req = HttpWebRequest.Create(site);
                req.Method = "GET";
                using (StreamReader reader = new StreamReader(req.GetResponse().GetResponseStream(), Encoding.UTF8))
                {
                    q = reader.ReadToEnd();
                }
                string[] cut = q.Split(new string[] { "videoInfo.videoUrl = '" }, StringSplitOptions.None);
                string[] cut2 = cut[1].Split(new string[] { "'" }, StringSplitOptions.None);
                string[] title = q.Split(new string[] { "<h1 class=\"videoUstBaslik\">" }, StringSplitOptions.None);
                string[] title2 = title[1].Split(new string[] { "</h1>" }, StringSplitOptions.None);

                string[] desc = q.Split(new string[] { "<h2 class=\"videoSpotFont\">" }, StringSplitOptions.None);
                string[] desc2 = desc[1].Split(new string[] { "</h2>" }, StringSplitOptions.None);
                desc2[0] = desc2[0].Replace("&nbsp;", "");
                desc2[0] = desc2[0].Replace("&ccedil;", "ç");
                txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
                txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
                txvideo.Text = cut2[0];
            }
            catch { }
        }
        private void Haber365(string site)//sorun var
        {
            try
            {
                string q = "";
                using (var client = new WebClient())
                {
                    client.Encoding = Encoding.GetEncoding("ISO-8859-9");
                    client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                    q = client.DownloadString(site);
                }
                string[] cut = q.Split(new string[] { "&strSource=" }, StringSplitOptions.None);
                string[] cut2 = cut[1].Split(new string[] { "&contentImg=" }, StringSplitOptions.None);
                string[] title = q.Split(new string[] { "<div id=\"dvVideoBilgiM\">" }, StringSplitOptions.None);
                string[] title2 = title[1].Split(new string[] { "</div>" }, StringSplitOptions.None);

                //string[] desc = q.Split(new string[] { "<div id=\"dvVideoBilgiO\">" }, StringSplitOptions.None);
                //string[] desc2 = desc[1].Split(new string[] { "</div>" }, StringSplitOptions.None);
                txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
                txaciklama.Text = WebUtility.HtmlDecode(title2[0]);
                txvideo.Text = cut2[0];
            }
            catch { }
        }
        private void EnSonHaber(string site)//return video link|video title|description
        {
            try
            {
                string q = "";

                WebRequest req = HttpWebRequest.Create(site);
                req.Method = "GET";
                using (StreamReader reader = new StreamReader(req.GetResponse().GetResponseStream(), Encoding.GetEncoding("iso-8859-9")))
                {
                    q = reader.ReadToEnd();
                }
                string[] cut = q.Split(new string[] { "'630','350','" }, StringSplitOptions.None);
                string[] cut2 = cut[1].Split(new string[] { "','',''," }, StringSplitOptions.None);
                string[] title = q.Split(new string[] { "<title>" }, StringSplitOptions.None);
                string[] title2 = title[1].Split(new string[] { "</title>" }, StringSplitOptions.None);

                string[] desc = q.Split(new string[] { "<div class=\"videoText\">" }, StringSplitOptions.None);
                string[] desc2 = desc[1].Split(new string[] { "</div>" }, StringSplitOptions.None);
                txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
                txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
                txvideo.Text = cut2[0];
            }
            catch { }
        }
        private void HaberTurk(string site)//same
        {
            try
            {
                string q = "";


                WebRequest req = HttpWebRequest.Create(site);
                req.Method = "GET";
                using (StreamReader reader = new StreamReader(req.GetResponse().GetResponseStream(), Encoding.UTF8))
                {
                    q = reader.ReadToEnd();
                }
                string[] cut = q.Split(new string[] { "<meta property=\"og:video\" content=" }, StringSplitOptions.None);
                string[] cut2 = cut[1].Split(new string[] { " />" }, StringSplitOptions.None);
                string[] cut3 = cut2[0].Split(new string[] { "&path=" }, StringSplitOptions.None);
                string[] cut4 = cut3[1].Split(new string[] { "&link=" }, StringSplitOptions.None);
                string[] title = q.Split(new string[] { "<title>" }, StringSplitOptions.None);
                string[] title2 = title[1].Split(new string[] { "</title>" }, StringSplitOptions.None);

                string[] desc = q.Split(new string[] { "<div class=\"text\"><p>" }, StringSplitOptions.None);
                string[] desc2 = desc[1].Split(new string[] { "</p></div>" }, StringSplitOptions.None);
                desc2[0] = desc2[0].Replace("&ouml;", "ö");
                desc2[0] = desc2[0].Replace("&uuml;", "ü");
                desc2[0] = desc2[0].Replace("&Ccedil;", "ç");
                desc2[0] = desc2[0].Replace("&ccedil;", "ç");
                desc2[0] = desc2[0].Replace("&quot;", "'");
                desc2[0] = desc2[0].Replace("&Uuml;", "Ü");
                desc2[0] = desc2[0].Replace("&amp;", "&");
                txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
                txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
                txvideo.Text = cut4[0];
            }
            catch { }
        }
        private void Bugün(string site)//same
        {
            try
            {
                string q = "";


                WebRequest req = HttpWebRequest.Create(site);
                req.Method = "GET";
                using (StreamReader reader = new StreamReader(req.GetResponse().GetResponseStream(), Encoding.UTF8))
                {
                    q = reader.ReadToEnd();
                }
                string[] cut = q.Split(new string[] { "bugunPlayer.swf?file=" }, StringSplitOptions.None);
                string[] cut2 = cut[1].Split(new string[] { "&amp;" }, StringSplitOptions.None);
                string[] title = q.Split(new string[] { "<title>" }, StringSplitOptions.None);
                string[] title2 = title[1].Split(new string[] { "</title>" }, StringSplitOptions.None);
                string[] desc = q.Split(new string[] { "<div style=\"margin-top: 10px; color:#fff; font-size:16px;\">" + Environment.NewLine }, StringSplitOptions.None);
                string[] desc2 = desc[1].Split(new string[] { "</div>" }, StringSplitOptions.None);
                txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
                txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
                txvideo.Text = cut2[0];
            }
            catch { }
        }
        private void EKolay(string site)//same
        {
            try
            {
                string q = "";


                WebRequest req = HttpWebRequest.Create(site);
                req.Method = "GET";
                using (StreamReader reader = new StreamReader(req.GetResponse().GetResponseStream(), Encoding.GetEncoding("windows-1254")))
                {
                    q = reader.ReadToEnd();
                }
                string[] cut = q.Split(new string[] { "\"VideoUrl\": \"" }, StringSplitOptions.None);
                string[] cut2 = cut[1].Split(new string[] { "\"" }, StringSplitOptions.None);
                string[] title = q.Split(new string[] { "<meta property=\"og:title\" content=\"" }, StringSplitOptions.None);
                string[] title2 = title[1].Split(new string[] { "\"" }, StringSplitOptions.None);
                string[] desc = q.Split(new string[] { "<h2 style=\"display: block;font: bold 10pt/1.4 arial;color:black;\">" }, StringSplitOptions.None);
                string[] desc2 = desc[1].Split(new string[] { "</h2>" }, StringSplitOptions.None);
                txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
                txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
                txvideo.Text = cut2[0];
            }
            catch { }
        }
        private void CNNTurk(string site)//same
        {
            try
            {
                string q = "";


                WebRequest req = HttpWebRequest.Create(site);
                req.Method = "GET";
                using (StreamReader reader = new StreamReader(req.GetResponse().GetResponseStream(), Encoding.UTF8))
                {
                    q = reader.ReadToEnd();
                }
                string[] cut = q.Split(new string[] { "\"VideoUrl\": \"" }, StringSplitOptions.None);
                string[] cut2 = cut[1].Split(new string[] { "\"" }, StringSplitOptions.None);
                string[] title = q.Split(new string[] { "<title>" }, StringSplitOptions.None);
                string[] title2 = title[1].Split(new string[] { "</title>" }, StringSplitOptions.None);
                string[] desc = q.Split(new string[] { "<meta name=\"description\" content=\"" }, StringSplitOptions.None);
                string[] desc2 = desc[1].Split(new string[] { "\" />  " }, StringSplitOptions.None);

                desc2[0] = desc2[0].Replace("&#39;", "'");
                desc2[0] = desc2[0].Replace("&#252;", "ü");
                desc2[0] = desc2[0].Replace("&#231;", "ç");
                desc2[0] = desc2[0].Replace("&#246;", "ö");
                desc2[0] = desc2[0].Replace("&quot;", "\"");

                txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
                txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
                txvideo.Text = cut2[0];
            }
            catch { }

        }
        private void NasilTV(string site)//no desc
        {
            try
            {
                string q = "";


                WebRequest req = HttpWebRequest.Create(site);
                req.Method = "GET";
                using (StreamReader reader = new StreamReader(req.GetResponse().GetResponseStream(), Encoding.UTF8))
                {
                    q = reader.ReadToEnd();
                }
                string[] cut = q.Split(new string[] { "video_vid = \"" }, StringSplitOptions.None);
                string[] cut2 = cut[1].Split(new string[] { "\"" }, StringSplitOptions.None);
                string[] title = q.Split(new string[] { "property=\"og:title\" content=\"" }, StringSplitOptions.None);
                string[] title2 = title[1].Split(new string[] { "\"" }, StringSplitOptions.None);
                txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
                txaciklama.Text = WebUtility.HtmlDecode(title2[0]);
                txvideo.Text = "http://www.nasil.tv/video/nasiltvplayer.swf=vi=" + cut2[0];
            }
            catch { }

        }
        private void FragmanTV(string site)//same
        {
            try
            {
                string q = "";


                WebRequest req = HttpWebRequest.Create(site);
                req.Method = "GET";
                using (StreamReader reader = new StreamReader(req.GetResponse().GetResponseStream(), Encoding.UTF8))
                {
                    q = reader.ReadToEnd();
                }
                string[] cut = q.Split(new string[] { "strSource=" }, StringSplitOptions.None);
                string[] cut2 = cut[1].Split(new string[] { "&relatedVideos=" }, StringSplitOptions.None);
                string[] title = q.Split(new string[] { "<title>" }, StringSplitOptions.None);
                string[] title2 = title[1].Split(new string[] { "</title>" }, StringSplitOptions.None);
                cut2[0] = cut2[0].Replace("%3A", ":");
                cut2[0] = cut2[0].Replace("%2F", "/");
                string[] desc = q.Split(new string[] { "</a>:</b>" }, StringSplitOptions.None);
                string[] desc2 = desc[1].Split(new string[] { "</p>" }, StringSplitOptions.None);
                txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
                txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
                txvideo.Text = cut2[0];
            }
            catch { }
        }
        private void FragmanlarOrg(string site)//same
        {
            try
            {
                string q = "";


                WebRequest req = HttpWebRequest.Create(site);
                req.Method = "GET";
                using (StreamReader reader = new StreamReader(req.GetResponse().GetResponseStream(), Encoding.UTF8))
                {
                    q = reader.ReadToEnd();
                }
                string[] cut = q.Split(new string[] { "strSource: \"" }, StringSplitOptions.None);
                string[] cut2 = cut[1].Split(new string[] { "\"" }, StringSplitOptions.None);
                string[] title = q.Split(new string[] { "<title>" }, StringSplitOptions.None);
                string[] title2 = title[1].Split(new string[] { "</title>" }, StringSplitOptions.None);
                string[] desc = q.Split(new string[] { "<div class=\"icerik\"><p>" }, StringSplitOptions.None);
                string[] desc2 = desc[1].Split(new string[] { "</div>" }, StringSplitOptions.None);
                desc2[0] = desc2[0].Replace("<p>", "");
                desc2[0] = desc2[0].Replace("</p>", "");
                desc2[0] = desc2[0].Replace("&#8242;", "'");
                txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
                txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
                txvideo.Text = cut2[0];
            }
            catch { }
        }
        private void Tamindir(string site)//same with <strong> vs. tags
        {
            try
            {
                string q = "";


                WebRequest req = HttpWebRequest.Create(site);
                req.Method = "GET";
                using (StreamReader reader = new StreamReader(req.GetResponse().GetResponseStream(), Encoding.UTF8))
                {
                    q = reader.ReadToEnd();
                }
                string[] cut = q.Split(new string[] { "levels:[{file:\"" }, StringSplitOptions.None);
                string[] cut2 = cut[1].Split(new string[] { "\"" }, StringSplitOptions.None);
                string[] title = q.Split(new string[] { "<title>" }, StringSplitOptions.None);
                string[] title2 = title[1].Split(new string[] { "</title>" }, StringSplitOptions.None);
                string[] desc = q.Split(new string[] { "<div id=\"aciklama\" itemprop=\"description\">" }, StringSplitOptions.None);
                string[] desc2 = desc[1].Split(new string[] { "</p></div>" }, StringSplitOptions.None);
                txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
                txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
                txvideo.Text = cut2[0];
            }
            catch { }

        }
        private void MilliyetTv(string site)
        {
            try
            {
                string q = "";


                WebRequest req = HttpWebRequest.Create(site);
                req.Method = "GET";
                using (StreamReader reader = new StreamReader(req.GetResponse().GetResponseStream(), Encoding.UTF8))
                {
                    q = reader.ReadToEnd();
                }
                string[] cut = q.Split(new string[] { "videoMp4Url = '" }, StringSplitOptions.None);
                string[] cut2 = cut[1].Split(new string[] { "'" }, StringSplitOptions.None);
                string[] title = q.Split(new string[] { "property=\"og:title\" content=\"" }, StringSplitOptions.None);
                string[] title2 = title[1].Split(new string[] { "\"" }, StringSplitOptions.None);
                string[] desc = q.Split(new string[] { "<div class=\"detTxt\">" }, StringSplitOptions.None);
                string[] desc2 = desc[1].Split(new string[] { "<" }, StringSplitOptions.None);
                txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
                txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
                txvideo.Text = cut2[0];
            }
            catch { }
        }
        private void TvArsivi(string site)//notitle - nodesc
        {
            try
            {
                string q = "";


                WebRequest req = HttpWebRequest.Create(site);
                req.Method = "GET";
                using (StreamReader reader = new StreamReader(req.GetResponse().GetResponseStream(), Encoding.GetEncoding("iso-8859-9")))
                {
                    q = reader.ReadToEnd();
                }
                string[] cut = q.Split(new string[] { "pc(\"" }, StringSplitOptions.None);
                string[] cut2 = cut[1].Split(new string[] { "\"," }, StringSplitOptions.None);
                txvideoadi.Text = "yok";
                txaciklama.Text = "yok";
                txvideo.Text = cut2[0];
            }
            catch { }
        }
        private void Hurriyet(string site)
        {
            try
            {
                string q = "";


                WebRequest req = HttpWebRequest.Create(site);
                req.Method = "GET";
                using (StreamReader reader = new StreamReader(req.GetResponse().GetResponseStream(), Encoding.GetEncoding("windows-1254")))
                {
                    q = reader.ReadToEnd();
                }
                string[] cut = q.Split(new string[] { "\"VideoUrl\": \"" }, StringSplitOptions.None);
                string[] cut2 = cut[1].Split(new string[] { "\"" }, StringSplitOptions.None);
                string[] title = q.Split(new string[] { "<title>" }, StringSplitOptions.None);
                string[] title2 = title[1].Split(new string[] { "</title>" }, StringSplitOptions.None);
                string[] desc = q.Split(new string[] { "og:description\" content=\"" }, StringSplitOptions.None);
                string[] desc2 = desc[1].Split(new string[] { "\"" }, StringSplitOptions.None);
                txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
                txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
                txvideo.Text = cut2[0];
            }
            catch { }
        }
        private void SporXTV(string site)
        {
            try
            {
                string q = "";


                WebRequest req = HttpWebRequest.Create(site);
                req.Method = "GET";
                using (StreamReader reader = new StreamReader(req.GetResponse().GetResponseStream(), Encoding.GetEncoding("windows-1254")))
                {
                    q = reader.ReadToEnd();
                }
                string[] cut = q.Split(new string[] { "fallbackUrls': [ '" }, StringSplitOptions.None);
                string[] cut2 = cut[1].Split(new string[] { "' ]" }, StringSplitOptions.None);
                string[] title = q.Split(new string[] { "<title>" }, StringSplitOptions.None);
                string[] title2 = title[1].Split(new string[] { "</title>" }, StringSplitOptions.None);
                string[] desc = q.Split(new string[] { "name=\"description\" content=\"" }, StringSplitOptions.None);
                string[] desc2 = desc[2].Split('"');
                txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
                txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
                txvideo.Text = cut2[0];
            }
            catch { }

        }
        private void Sabah(string site)//sorunlu
        {
            try
            {
                string q = "";

                using (var client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                    q = client.DownloadString(site);
                }
                string[] cut = q.Split(new string[] { "StreamUrl&quot;:&quot;" }, StringSplitOptions.None);
                string[] cut2 = cut[1].Split(new string[] { "video&quot;" }, StringSplitOptions.None);
                string[] cut3 = q.Split(new string[] { "VideoUrlWithMp4&quot;:&quot;" }, StringSplitOptions.None);
                string[] cut4 = cut3[1].Split(new string[] { "&quot;" }, StringSplitOptions.None);
                string[] title = q.Split(new string[] { "videoTitle = \"" }, StringSplitOptions.None);
                string[] title2 = title[1].Split(new string[] { "\";" }, StringSplitOptions.None);
                string[] desc = q.Split(new string[] { "\"txt\">" }, StringSplitOptions.None);
                string[] desc2 = desc[2].Split(new string[] { "</span>" }, StringSplitOptions.None);
                txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
                txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
                txvideo.Text = cut2[0] + cut4[0];
            }
            catch { }
        }
        private void Fanatik(string site)//same
        {
            try
            {
                string q = "";

                using (var client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                    q = client.DownloadString(site);
                }
                string[] cut = q.Split(new string[] { "\"VideoUrl\": \"" }, StringSplitOptions.None);
                string[] cut2 = cut[1].Split(new string[] { "\"" }, StringSplitOptions.None);
                string[] title = q.Split(new string[] { "<h2>" }, StringSplitOptions.None);
                string[] title2 = title[1].Split(new string[] { "</h2>" }, StringSplitOptions.None);
                string[] desc = q.Split(new string[] { "og:description\" content=\"" }, StringSplitOptions.None);
                string[] desc2 = desc[1].Split('"');
                txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
                txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
                txvideo.Text = cut2[0];
            }
            catch { }
        }
        #endregion

        #region Yabancı Siteler
        private void kuzutv(string site)//return video link|video title|description
        {
            try{string q = "";

            WebRequest req = HttpWebRequest.Create(site);
            req.Method = "GET";
            using (StreamReader reader = new StreamReader(req.GetResponse().GetResponseStream(), Encoding.UTF8))
            {
                q = reader.ReadToEnd();
            }
            string[] cut = q.Split(new string[] { "video/mp4\" src=\"" }, StringSplitOptions.None);
            string[] cut2 = cut[1].Split('"');
            string[] title = q.Split(new string[] { "<title>" }, StringSplitOptions.None);
            string[] title2 = title[1].Split(new string[] { "</title>" }, StringSplitOptions.None);
            string[] desc = q.Split(new string[] { "description\" content=\"" }, StringSplitOptions.None);
            string[] desc2 = desc[1].Split('"');

            txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
            txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
            txvideo.Text = cut2[0];
            }catch{}
        }
        private void ehow(string site)//return video link|video title|description
        {
            try{string q = "";

            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                q = client.DownloadString(site);
            }
            string[] cut = q.Split(new string[] { "contentURL\" content=\"" }, StringSplitOptions.None);
            string[] cut2 = cut[1].Split('"');
            cut2[0] = cut2[0].Replace(".jpg", ".mp4");
            string[] title = q.Split(new string[] { "<title>" }, StringSplitOptions.None);
            string[] title2 = title[1].Split(new string[] { "</title>" }, StringSplitOptions.None);

            string[] desc = q.Split(new string[] { "description\" content=\"" }, StringSplitOptions.None);
            string[] desc2 = desc[1].Split('"');
            txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
            txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
            txvideo.Text = cut2[0];}catch{}

        }
        private void veoh(string site)//return video link|video title|description
        {
            try{string q = "";

            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                q = client.DownloadString(site);
            }
            string[] cut = q.Split(new string[] { "og:image\" content=\"" }, StringSplitOptions.None);
            string[] cut2 = cut[1].Split('"');
            cut2[0] = cut2[0].Replace(".jpg", ".mp4");
            string[] title = q.Split(new string[] { "<title>" }, StringSplitOptions.None);
            string[] title2 = title[1].Split(new string[] { "</title>" }, StringSplitOptions.None);

            string[] desc = q.Split(new string[] { "og:description\" content=\"" }, StringSplitOptions.None);
            string[] desc2 = desc[1].Split('"');
            txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
            txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
            txvideo.Text = cut2[0];}catch{}

        }
        private void funnyjunk(string site)
        {
            try{string q = "";

            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                q = client.DownloadString(site);
            }
            string[] cut = q.Split(new string[] { "image_src\" href=\"" }, StringSplitOptions.None);
            string[] cut2 = cut[1].Split('"');
            string[] title = q.Split(new string[] { "<title>" }, StringSplitOptions.None);
            string[] title2 = title[1].Split(new string[] { "</title>" }, StringSplitOptions.None);
            string[] desc = q.Split(new string[] { "description\" content=\"" }, StringSplitOptions.None);
            string[] desc2 = desc[1].Split('"');
            txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
            txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
            txvideo.Text = cut2[0];}catch{}

        }
        private void ebaumsworld(string site)
        {
            try{string q = "";

            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                q = client.DownloadString(site);
            }
            string[] cut = q.Split(new string[] { "dropListTrigger-mediaAvatarMenu-" }, StringSplitOptions.None);
            string[] cut2 = cut[1].Split('"');
            string[] cut3 = q.Split(new string[] { "itemid','" }, StringSplitOptions.None);
            string[] cut4 = cut3[1].Split('\'');
            string[] title = q.Split(new string[] { "<title>" }, StringSplitOptions.None);
            string[] title2 = title[1].Split(new string[] { "</title>" }, StringSplitOptions.None);
            string[] desc = q.Split(new string[] { "og:description\" content=\"" }, StringSplitOptions.None);
            string[] desc2 = desc[1].Split('"');
            txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
            txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
            txvideo.Text = "http://level3.ebaumsworld.com/mediaFiles/video/" + cut2[0] + "/" + cut4[0]+".mp4";}catch{}

        }
        private void killsometime(string site)
        {
            try{string q = "";

            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                q = client.DownloadString(site);
            }
            string[] cut = q.Split(new string[] { "file: '" }, StringSplitOptions.None);
            string[] cut2 = cut[1].Split('\'');
            string[] title = q.Split(new string[] { "<title>" }, StringSplitOptions.None);
            string[] title2 = title[1].Split(new string[] { "</title>" }, StringSplitOptions.None);
            string[] desc = q.Split(new string[] { "og:description\" content='" }, StringSplitOptions.None);
            string[] desc2 = desc[1].Split('\'');
            txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
            txaciklama.Text = WebUtility.HtmlDecode(title2[0]);
            txvideo.Text = "http://www.killsometime.com" + cut2[0];}catch{}

        }
        private void dailymotion(string site)
        {
            try
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
                desc2[0] =desc2[0].Replace(" - Dailymotion video", "");
                txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
                txaciklama.Text = WebUtility.HtmlDecode(title2[0]);
            }catch{}

        }
        public static int CountWords2(string s)
        {
	    int c = 0;
	    for (int i = 1; i < s.Length; i++)
	    {
	        if (char.IsWhiteSpace(s[i - 1]) == true)
	        {
		    if (char.IsLetterOrDigit(s[i]) == true ||
		        char.IsPunctuation(s[i]))
		    {
		        c++;
		    }
	        }
	    }
	    if (s.Length > 2)
	    {
	        c++;
	    }
	    return c;
        }
        private void facebook(string site)
        {
            try{string q = "";

            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_6_8) AppleWebKit/537.1 (KHTML, like Gecko) Chrome/21.0.1180.89 Safari/537.1";
                q = client.DownloadString(site);
            }
                /*string[] cut = q.Split(new string[] { "hd_src" }, StringSplitOptions.None);
                string[] cut2 = cut[1].Split(new string[] { "default_hd" }, StringSplitOptions.None);
                string[] title = q.Split(new string[] { "<title id=\"pageTitle\">" }, StringSplitOptions.None);
                string[] title2 = title[1].Split(new string[] { "</title>" }, StringSplitOptions.None);
                cut2[0] = cut2[0].Replace("\\u00253D", "=").Replace("\\u00255C\\u00252F", "/").Replace("\\u00253F", "?").Replace("\\u002526", "&");
                string[] cut3 = cut2[0].Split(new string[] { "00253A//" }, StringSplitOptions.None);
                string[] cut4 = cut3[1].Split(new string[] { "\\u002522" }, StringSplitOptions.None);
                txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
                txaciklama.Text = WebUtility.HtmlDecode(title2[0]);
                txvideo.Text = "http://" + cut4[0];*/}catch{}
        }
        private void yapfiles(string site)
        {
            try{string q = "";
            
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                q = client.DownloadString(site);
            }
            string[] cut = q.Split(new string[] { "name=\"direct_link\" value=\"" }, StringSplitOptions.None);
            string[] cut2 = cut[1].Split('"');
            string[] title = q.Split(new string[] { "<title>" }, StringSplitOptions.None);
            string[] title2 = title[1].Split(new string[] { "</title>" }, StringSplitOptions.None);
            string[] desc = q.Split(new string[] { "description\" content=\"" }, StringSplitOptions.None);
            string[] desc2 = desc[1].Split('"');
            txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
                txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
                txvideo.Text = cut2[0];}catch{}

        }
        private void krasview(string site)
        {
            try{string q = "";
            
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.GetEncoding("windows-1251");
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                q = client.DownloadString(site);
            }
            string[] cut = q.Split(new string[] { "\"url\":\"" }, StringSplitOptions.None);
            string[] cut2 = cut[1].Split('"');
            cut2[0]=cut2[0].Replace("\\","");
            cut2[0] = cut2[0] + "&start=0";
            string[] title = q.Split(new string[] { "<title>" }, StringSplitOptions.None);
            string[] title2 = title[1].Split(new string[] { "</title>" }, StringSplitOptions.None);
            string[] desc = q.Split(new string[] { "description\" content=\"" }, StringSplitOptions.None);
            string[] desc2 = desc[1].Split('"');
            txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
                txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
                txvideo.Text = cut2[0];}catch{}

        }
        private void jokeroo(string site)
        {
            try{string q = "";
            
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                q = client.DownloadString(site);
            }
            string[] cut = q.Split(new string[] { "\\\"u\\\":\\\"" }, StringSplitOptions.None);
            string[] cut2 = cut[1].Split('\\');
            string[] title = q.Split(new string[] { "<title>" }, StringSplitOptions.None);
            string[] title2 = title[1].Split(new string[] { "</title>" }, StringSplitOptions.None);
            string[] desc = q.Split(new string[] { "description\" content=\"" }, StringSplitOptions.None);
            string[] desc2 = desc[1].Split('"');
            txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
                txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
                txvideo.Text = cut2[0];}catch{}

        }
        private void r7com(string site)
        {
            try{string q = "";
            
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                q = client.DownloadString(site);
            }
            string[] cut = q.Split(new string[] { "video=" }, StringSplitOptions.None);
            string[] cut2 = cut[1].Split('&');
            cut2[0] = cut2[0].Replace("%3A", ":");
            cut2[0] = cut2[0].Replace("%2F", "/");
            string[] title = q.Split(new string[] { "<title>" }, StringSplitOptions.None);
            string[] title2 = title[1].Split(new string[] { "</title>" }, StringSplitOptions.None);
            string[] desc = q.Split(new string[] { "description\" content=\"" }, StringSplitOptions.None);
            string[] desc2 = desc[1].Split(new string[] { "\"/>" }, StringSplitOptions.None);
            txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
                txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
                txvideo.Text = cut2[0];}catch{}

        }
        private void NewsLook(string site)
        {
            try{string q = "";
            
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                q = client.DownloadString(site);
            }
            string[] cut = q.Split(new string[] { "cdn_asset_url\":\"" }, StringSplitOptions.None);
            string[] cut2 = cut[1].Split(new string[] { "\",\"" }, StringSplitOptions.None);
            string[] title = q.Split(new string[] { "<h1>" }, StringSplitOptions.None);
            string[] title2 = title[1].Split(new string[] { "</h1>" }, StringSplitOptions.None);
            string[] desc = q.Split(new string[] { "class=\"desc\">" }, StringSplitOptions.None);
            string[] desc2 = desc[1].Split(new string[] { "</p>" }, StringSplitOptions.None);
            txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
                txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
                txvideo.Text = cut2[0];
                }catch{}
        }
        private void BreakCom(string site)
        {
            try{string q = "";
            
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                q = client.DownloadString(site);
            }
            string[] cut = q.Split(new string[] { "embed_video_url\" content=\"" }, StringSplitOptions.None);
            string[] cut2 = cut[1].Split('"');
            string[] title = q.Split(new string[] { "<title>" }, StringSplitOptions.None);
            string[] title2 = title[1].Split(new string[] { "</title>" }, StringSplitOptions.None);
            string[] desc = q.Split(new string[] { "description\" content=\"" }, StringSplitOptions.None);
            string[] desc2 = desc[1].Split('"');

            if (cut2[0].IndexOf("youtubeid") != -1)
            {
                string[] cut3=cut2[0].Split(new string[]{"youtubeid/"},StringSplitOptions.None);
                cut3[1] = "http://www.youtube.com/watch?v=" + cut3[1];
                txvideo.Text = cut3[1];
                do
                {
                    quality_ComboBox.Items.Clear();
                    videoUrls = new string[] { txlink.Text };
                    List<YouTubeVideoQuality> urls = YouTubeDownloader.GetYouTubeVideoUrls(videoUrls);
                    quality_ComboBox.Enabled = true;
                    foreach (var item in urls)
                    {
                        string videoExtention = item.Extention;
                        string videoDimension = formatSize(item.Dimension);
                        string videoSize = formatSizeBinary(item.VideoSize);

                        quality_ComboBox.Items.Add(String.Format("{0} ({1}) - {2}", videoExtention.ToUpper(), videoDimension, videoSize));
                        quality_ComboBox.Text = quality_ComboBox.Items[0].ToString();
                        quality_ComboBox.Enabled = true;
                        downVideoUrls.Add(item);
                        txvideoadi.Text = downVideoUrls[quality_ComboBox.SelectedIndex].VideoTitle;
                        if (radioButton6.Checked)
                            quality_ComboBox.SelectedIndex = quality_ComboBox.Items.Count - 1;
                        txvideo.Text = downVideoUrls[quality_ComboBox.SelectedIndex].DownloadUrl;
                    }
                }
                while (quality_ComboBox.Items.Count < 2);
            }
            else
            {
                WebBrowser a = new WebBrowser();
                a.Navigate(cut2[0]);
                a.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(a_DocumentCompleted);
                txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
                txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
                txvideo.Text = cut2[0];
            }
            }
            catch { }
        }
        private void a_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            string url = WebUtility.HtmlDecode(e.Url.ToString());
            string[] url2 = url.Split(new string[] { "icon=" }, StringSplitOptions.None);
            string url3 = url2[1];
            string[] url4 = url.Split(new string[] {"VidLoc=" }, StringSplitOptions.None);
            string[] url5 = url4[1].Split('&');

            txvideo.Text=url5[0]+"?"+url3+"&ec_rate=64&ec_prebuf=64";

        }

        private void FunnyPlace(string site)
        {
            try{string q = "";
            
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                q = client.DownloadString(site);
            }
            string[] cut = q.Split(new string[] { "file': '" }, StringSplitOptions.None);
            string[] cut2 = cut[1].Split(new string[] { "'," }, StringSplitOptions.None);
            string[] title = q.Split(new string[] { "<h1>" }, StringSplitOptions.None);
            string[] title2 = title[1].Split(new string[] { "</h1>" }, StringSplitOptions.None);
            string[] desc = q.Split(new string[] { "class=\"opis\">" }, StringSplitOptions.None);
            string[] desc2 = desc[1].Split(new string[] { "</p>" }, StringSplitOptions.None);
            desc2[0] = desc2[0].Replace("<br />", "");
            txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
                txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
                txvideo.Text = cut2[0];}catch{}

        }
        private void Bing(string site)
        {
            try{string q = "";
            
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                q = client.DownloadString(site);
            }
            string[] cut = q.Split(new string[] { "formatCode: 1002, url: '" }, StringSplitOptions.None);
            string[] cut2 = cut[1].Split(new string[] { "'," }, StringSplitOptions.None);
            string[] title = q.Split(new string[] { "<title>" }, StringSplitOptions.None);
            string[] title2 = title[1].Split(new string[] { "</title>" }, StringSplitOptions.None);
            string[] desc = q.Split(new string[] { "og:description\" content=\"" }, StringSplitOptions.None);
            string[] desc2 = desc[1].Split(new string[] { "\" />" }, StringSplitOptions.None);
            cut2[0] = cut2[0].Replace("\\x3a", ":");
            cut2[0] = cut2[0].Replace("\\x2f", "/");
            txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
                txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
                txvideo.Text = cut2[0];}catch{}

        }
        private void IGN(string site)
        {
            try{string q = "";
            
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                q = client.DownloadString(site);
            }
            string[] cut = q.Split(new string[] { "videoSource_0\" src=\"" }, StringSplitOptions.None);
            string[] cut2 = cut[1].Split(new string[] { "\">" }, StringSplitOptions.None);
            string[] title = q.Split(new string[] { "data-video-caption=\"" }, StringSplitOptions.None);
            string[] title2 = title[1].Split('"');
            string[] desc = q.Split(new string[] { "page-object-description\">" }, StringSplitOptions.None);
            string[] desc2 = desc[1].Split(new string[] { "</span>" }, StringSplitOptions.None);
            txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
                txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
                txvideo.Text = cut2[0];}catch{}
        }
        private void TED(string site)
        {
            try{string q = "";
            
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                q = client.DownloadString(site);
            }
            string[] cut = q.Split(new string[] { "no-flash-video-download\" href=\"" }, StringSplitOptions.None);
            string[] cut2 = cut[1].Split('"');
            string[] title = q.Split(new string[] { "<title>" }, StringSplitOptions.None);
            string[] title2 = title[1].Split(new string[] { "</title>" }, StringSplitOptions.None);
            string[] desc = q.Split(new string[] { "og:description\" content=\"" }, StringSplitOptions.None);
            string[] desc2 = desc[1].Split(new string[] { "\" />" }, StringSplitOptions.None);
            txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
                txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
                txvideo.Text = cut2[0];}catch{}

        }
        private void StupidVideos(string site)
        {
            try{string q = "";
            string l3 = "http://videos.stupidvideos.com/2/00/00/0";
            string l4 = "http://videos.stupidvideos.com/2/00/00/";
            string l5 = "http://videos.stupidvideos.com/2/00/0";
            string l6 = "http://videos.stupidvideos.com/2/00/";
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                q = client.DownloadString(site);
            }
            string[] cut = q.Split(new string[] { "var videoID = '" }, StringSplitOptions.None);
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
                    l5=l5+ cut2[0][i];
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
                    if(i==3)
                        l6 = l6 + "/";
                }
                l6 = l6 + "/";
                cut2[0] = l6+ cut2[0] + ".flv";
            }


            string[] title = q.Split(new string[] { "og:title\" content=\"" }, StringSplitOptions.None);
            string[] title2 = title[1].Split(new string[] { "\" />" }, StringSplitOptions.None);
            string[] desc = q.Split(new string[] { "description\" content=\"" }, StringSplitOptions.None);
            string[] desc2 = desc[1].Split(new string[] { "\"/>" }, StringSplitOptions.None);
            txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
                txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
                txvideo.Text = cut2[0];}catch{}
        }
        private void LiveLeak(string site)
        {
            try{string q = "";
            
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                q = client.DownloadString(site);
            }
            string[] cut = q.Split(new string[] { "file: \"" }, StringSplitOptions.None);
            string[] cut2 = cut[1].Split(new string[] { "\"," }, StringSplitOptions.None);
            string[] title = q.Split(new string[] { "padding-right:10px\">" }, StringSplitOptions.None);
            string[] title2 = title[1].Split('<');
            string[] desc = q.Split(new string[] { "og:description\" content=\"" }, StringSplitOptions.None);
            string[] desc2 = desc[1].Split('"');
            txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
                txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
                txvideo.Text = cut2[0] + "&ec_seek=0";}catch{}

        }
        private void MetaCafe(string site)//sorunlu
        {
            try{string q = "";
            
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                q = client.DownloadString(site);
            }
            string[] cut = q.Split(new string[] { "mediaURL%22%3A%22" }, StringSplitOptions.None);
            string[] cut2 = cut[1].Split(new string[] { "%22%2C%22access" }, StringSplitOptions.None);
            cut2[0] = cut2[0].Replace("%25", "%");
            cut2[0] = cut2[0].Replace("%3A", ":");
            cut2[0] = cut2[0].Replace("%5C%2F", "/");

            string[] title = q.Split(new string[] { "title\" content=\"" }, StringSplitOptions.None);
            string[] title2 = title[1].Split(new string[] { "\" />" }, StringSplitOptions.None);
            string[] desc = q.Split(new string[] { "description\" content=\"" }, StringSplitOptions.None);
            string[] desc2 = desc[1].Split(new string[] { "\" />" }, StringSplitOptions.None);
            txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
                txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
                txvideo.Text = cut2[0];}catch{}

        }
        private void VideoBash(string site)
        {
            try{string q = "";
            
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                q = client.DownloadString(site);
            }
            string[] cut = q.Split(new string[] { "'http://' + '" }, StringSplitOptions.None);
            string[] cut2 = cut[1].Split(new string[] { "';" }, StringSplitOptions.None);
            cut2[0] = cut2[0].Replace("%2F", "/");
            cut2[0] = cut2[0].Replace("%3F", "?");
            cut2[0] = cut2[0].Replace("%3D", "=");
            cut2[0] = cut2[0].Replace("%26", "&");
            cut2[0] = cut2[0].Replace("%25", "%");
            cut2[0] = cut2[0].Replace("%26", "=");
            cut2[0] = "http://" + cut2[0];
            string[] title = q.Split(new string[] { "og:title\" content=\"" }, StringSplitOptions.None);
            string[] title2 = title[1].Split(new string[] { "\" />" }, StringSplitOptions.None);
            string[] desc = q.Split(new string[] { "og:description\" content=\"" }, StringSplitOptions.None);
            string[] desc2 = desc[1].Split(new string[] { "\" />" }, StringSplitOptions.None);
            txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
                txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
                txvideo.Text = cut2[0];}catch{}

        }
        private void NewsSky(string site)
        {
            try{string q = "";
            
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                q = client.DownloadString(site);
            }
            string[] cut = q.Split(new string[] { "data-sn-mp4=\"" }, StringSplitOptions.None);
            string[] cut2 = cut[1].Split('"');
            string[] title = q.Split(new string[] { "<title>" }, StringSplitOptions.None);
            string[] title2 = title[1].Split(new string[] {"</title>" }, StringSplitOptions.None);
            string[] desc = q.Split(new string[] { "class=\"intro\">" }, StringSplitOptions.None);
            string[] desc2 = desc[1].Split(new string[] { "</p>" }, StringSplitOptions.None);
            txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
                txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
                txvideo.Text = cut2[0];}catch{}

        }
        private void VoaNews(string site)
        {
            try{string q = "";
            
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                q = client.DownloadString(site);
            }
            string[] cut = q.Split(new string[] { "<source src=\"" }, StringSplitOptions.None);
            string[] cut2 = cut[1].Split('"');
            string[] title = q.Split(new string[] { "j_title-text\">" }, StringSplitOptions.None);
            string[] title2 = title[1].Split(new string[] { "</h1>" }, StringSplitOptions.None);
            string[] desc = q.Split(new string[] { "class=\"descText\">" }, StringSplitOptions.None);
            string[] desc2 = desc[1].Split(new string[] { "</p>" }, StringSplitOptions.None);
            txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
                txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
                txvideo.Text = cut2[0];}catch{}

        }
        private void prochan(string site)
        {
            try{string q = "";
            
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                q = client.DownloadString(site);
            }
            string[] cut = q.Split(new string[] { "'file': '" }, StringSplitOptions.None);
            string[] cut2 = cut[1].Split('\'');
            string[] title = q.Split(new string[] { "st_title='" }, StringSplitOptions.None);
            string[] title2 = title[1].Split(new string[] { "'>" }, StringSplitOptions.None);
            string[] desc = q.Split(new string[] { "<br>" }, StringSplitOptions.None);
            string[] desc2 = desc[2].Split(new string[] { "</p>" }, StringSplitOptions.None);
            txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
                txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
                txvideo.Text = cut2[0];}catch{}

        }
        private void EuroNews(string site)
        {
            try{string q = "";
            
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                q = client.DownloadString(site);
            }
            if (q.IndexOf("vid1") != -1)
            {
                string[] cut = q.Split(new string[] { "vid1:\"" }, StringSplitOptions.None);
                string[] cut2 = cut[1].Split('"');
                cut2[0] = "http://video.euronews.com/" + cut2[0] + ".flv";
                string[] title = q.Split(new string[] { "<title>" }, StringSplitOptions.None);
                string[] title2 = title[1].Split(new string[] { "</title>" }, StringSplitOptions.None);
                string[] desc = q.Split(new string[] { "twitter:description\" content=\"" }, StringSplitOptions.None);
                string[] desc2 = desc[1].Split(new string[] { "\">" }, StringSplitOptions.None);
                txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
                txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
                txvideo.Text = cut2[0];
            }
            else
            {
                string[] cut = q.Split(new string[] { " videofile:\"" }, StringSplitOptions.None);
                string[] cut2 = cut[1].Split('"');
                cut2[0] = "http://video.euronews.com/" + cut2[0] + ".flv";
                string[] title = q.Split(new string[] { "<title>" }, StringSplitOptions.None);
                string[] title2 = title[1].Split(new string[] { "</title>" }, StringSplitOptions.None);
                string[] desc = q.Split(new string[] { "twitter:description\" content=\"" }, StringSplitOptions.None);
                string[] desc2 = desc[1].Split(new string[] { "\">" }, StringSplitOptions.None);
                txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
                txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
                txvideo.Text = cut2[0];
            }
                }catch{}
        }
        private void ochevidetsru(string site)
        {
            try{string q = "";
            
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                q = client.DownloadString(site);
            }
            string[] cut = q.Split(new string[] { "[{\"url\":\"" }, StringSplitOptions.None);
            string[] cut2 = cut[1].Split('"');
            string[] title = q.Split(new string[] { "<title>" }, StringSplitOptions.None);
            string[] title2 = title[1].Split(new string[] { "</title>" }, StringSplitOptions.None);
            string[] desc = q.Split(new string[] { "description\" content=\"" }, StringSplitOptions.None);
            string[] desc2 = desc[1].Split('"');
            txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
                txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
                txvideo.Text = cut2[0];}catch{}

        }
        private void nexttvcomtw(string site)
        {
            try{string q = "";
            
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                q = client.DownloadString(site);
            }
            string[] cut = q.Split(new string[] { "id=\"ntt-vod-src-detailview\" value=\"" }, StringSplitOptions.None);
            string[] cut2 = cut[1].Split('"');
            string[] title = q.Split(new string[] { "<h1>" }, StringSplitOptions.None);
            string[] title2 = title[2].Split(new string[] { "</h1>" }, StringSplitOptions.None);
            string[] desc = q.Split(new string[] { "description\">" }, StringSplitOptions.None);
            string[] desc2 = desc[1].Split(new string[] { "<" }, StringSplitOptions.None);
            txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
                txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
                txvideo.Text = cut2[0];}catch{}
        }
        private void funnyordie(string site)
        {
            try{string q = "";
            
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                q = client.DownloadString(site);
            }
            string[] cut = q.Split(new string[] { "<source src=\"" }, StringSplitOptions.None);
            string[] cut2 = cut[2].Split('"');
            string[] title = q.Split(new string[] { "<title>" }, StringSplitOptions.None);
            string[] title2 = title[1].Split(new string[] { "</title>" }, StringSplitOptions.None);
            string[] desc = q.Split(new string[] { "description\" content=\"" }, StringSplitOptions.None);
            string[] desc2 = desc[1].Split('"');
            txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
                txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
                txvideo.Text = cut2[0];
                }catch{}
        }
        private void dpccars(string site)
        {
            try{string q = "";
            
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                q = client.DownloadString(site);
            }
            string[] cut = q.Split(new string[] { "<iframe id=\"" }, StringSplitOptions.None);
            string[] cut2 = cut[1].Split(new string[] { "src=\"" }, StringSplitOptions.None);
            string[] cut3 = cut2[1].Split('"');
            string[] title = q.Split(new string[] { "<title>" }, StringSplitOptions.None);
            string[] title2 = title[1].Split(new string[] { "</title>" }, StringSplitOptions.None);
            string[] desc = q.Split(new string[] { "Description\" content=\"" }, StringSplitOptions.None);
            string[] desc2 = desc[1].Split('"');
            q = "";
            
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                q = client.DownloadString(cut3[0]);
            }
            string[] cut4 = q.Split(new string[] {"og:video\" content=\"" }, StringSplitOptions.None);
            string[] cut5 = cut4[1].Split('"');
            txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
                txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
                txvideo.Text = cut5[0];}catch{}

        }
        private void ioua(string site)
        {
            try{string q = "";
            
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.GetEncoding("windows-1251");
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                q = client.DownloadString(site);
            }
            string[] cut = q.Split(new string[] { "addVariable('file','" }, StringSplitOptions.None);
            string[] cut2 = cut[1].Split(new string[] { "')" }, StringSplitOptions.None);
            string[] title = q.Split(new string[] { "<TITLE>" }, StringSplitOptions.None);
            string[] title2 = title[1].Split(new string[] { "</TITLE>" }, StringSplitOptions.None);
            string[] desc = q.Split(new string[] { "description' content='" }, StringSplitOptions.None);
            string[] desc2 = desc[1].Split('\'');
            txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
                txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
                txvideo.Text = cut2[0];}catch{}

        }
        private void vidmax(string site)
        {
            try{string q = "";
            
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                q = client.DownloadString(site);
            }
            string[] cut = q.Split(new string[] { "&file=" }, StringSplitOptions.None);
            string[] cut2 = cut[1].Split('&');
            string[] title = q.Split(new string[] { "<h1>" }, StringSplitOptions.None);
            string[] title2 = title[1].Split(new string[] { "</h1>" }, StringSplitOptions.None);
            string[] desc = q.Split(new string[] { "<meta name=\"description\" content=\"" }, StringSplitOptions.None);
            string[] desc2 = desc[1].Split('"');
            txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
                txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
                txvideo.Text = cut2[0];}catch{}

        }
        private void extremevidazz(string site)
        {
            try{string q = "";
            
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.GetEncoding("windows-1251");
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                q = client.DownloadString(site);
            }
            string[] cut = q.Split(new string[] { "?vid=" }, StringSplitOptions.None);
            string[] cut2 = cut[2].Split('"');
            cut2[0] = "http://www.extremevidazz.com/files/videos/videos/"+cut2[0].Substring(0, 15)+".flv";

            string[] title = q.Split(new string[] { "<title>" }, StringSplitOptions.None);
            string[] title2 = title[1].Split(new string[] { "</title>" }, StringSplitOptions.None);
            string[] desc = q.Split(new string[] { "description\" content=\"" }, StringSplitOptions.None);
            string[] desc2 = desc[1].Split('"');
            txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
                txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
                txvideo.Text = cut2[0];}catch{}

        }
        private void rutvru(string site)
        {
            try{string q = "";
            
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                q = client.DownloadString(site);
            }
            string[] cut = q.Split(new string[] { "?vid=" }, StringSplitOptions.None);
            string[] cut2 = cut[1].Split('&');
            cut2[0] = "http://cdn1.vesti.ru/_cdn_auth/secure/v/vh/mp4/high/" + cut2[0].Substring(0, 3) + "/" + cut2[0].Substring(3, 3) + ".mp4?auth=vh&vid="+cut2[0]+"&sid=vh";

            string[] title = q.Split(new string[] { "<title>" }, StringSplitOptions.None);
            string[] title2 = title[1].Split(new string[] { "</title>" }, StringSplitOptions.None);
            string[] desc = q.Split(new string[] { "og:description\" content=\"" }, StringSplitOptions.None);
            string[] desc2 = desc[1].Split('"');
            txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
                txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
                txvideo.Text = cut2[0];}catch{}

        }
        private void kinostoktv(string site)
        {
            try{string q = "";
            
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                q = client.DownloadString(site);
            }
            string[] cut = q.Split(new string[] { "og:image\" content=\"" }, StringSplitOptions.None);
            string[] cut2 = cut[1].Split('"');

            cut2[0] = cut2[0].Replace("upload/img/photos/kinostok", "flv/728a2ae3034049e1bd662411ab031060/uploaded_video").Replace("b.jpg",".mp4");

            string[] title = q.Split(new string[] { "<title>" }, StringSplitOptions.None);
            string[] title2 = title[1].Split(new string[] { "</title>" }, StringSplitOptions.None);
            string[] desc = q.Split(new string[] { "description\" content=\"" }, StringSplitOptions.None);
            string[] desc2 = desc[1].Split('"');
            txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
                txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
                txvideo.Text = cut2[0];}catch{}

        }
        private void dayaz(string site)
        {
            try{string q = "";
            
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                q = client.DownloadString(site);
            }
            string[] cut = q.Split(new string[] { "og:image\" content=\"" }, StringSplitOptions.None);
            string[] cut2 = cut[1].Split('"');
            cut2[0] = cut2[0].Replace(".jpg", ".mp4");

            string[] title = q.Split(new string[] { "<title>" }, StringSplitOptions.None);
            string[] title2 = title[1].Split(new string[] { "</title>" }, StringSplitOptions.None);
            string[] desc = q.Split(new string[] { "og:description\" content=\"" }, StringSplitOptions.None);
            string[] desc2 = desc[1].Split('"');
            txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
            txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
            txvideo.Text = cut2[0];}catch{}

        }
        private void myvideoge(string site)
        {
            try{string q = "";

            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                q = client.DownloadString(site);
            }
            string[] cut = q.Split(new string[] { "'file': '" }, StringSplitOptions.None);
            string[] cut2 = cut[1].Split('\'');

            string[] title = q.Split(new string[] { "<title>" }, StringSplitOptions.None);
            string[] title2 = title[1].Split(new string[] { "</title>" }, StringSplitOptions.None);
            string[] desc = q.Split(new string[] { "description\" content=\"" }, StringSplitOptions.None);
            string[] desc2 = desc[1].Split('"');
            txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
            txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
            txvideo.Text = cut2[0];}catch{}

        }
        private void vidworru(string site)
        {
            try{string q = "";

            using (var client = new WebClient())
            {
                client.Encoding = Encoding.GetEncoding("windows-1251");
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                q = client.DownloadString(site);
            }
            string[] cut = q.Split(new string[] { "file: '" }, StringSplitOptions.None);
            string[] cut2 = cut[1].Split('\'');

            string[] title = q.Split(new string[] { "<title>" }, StringSplitOptions.None);
            string[] title2 = title[1].Split(new string[] { "</title>" }, StringSplitOptions.None);
            string[] desc = q.Split(new string[] { "og:description\" content=\"" }, StringSplitOptions.None);
            string[] desc2 = desc[1].Split('"');
            txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
            txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
            txvideo.Text = cut2[0];}catch{}

        }
        private void evocouk(string site)
        {
            try{string q = "";
            string[] cut = new string[15];
            string[] cut2 = new string[15];
            Array.Clear(cut, 0, cut.Length);
            Array.Clear(cut2, 0, cut2.Length);
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.GetEncoding("windows-1251");
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                q = client.DownloadString(site);
            }
            if (q.IndexOf("'file': '") != -1)
            {
                cut = q.Split(new string[] { "'file': '" }, StringSplitOptions.None);
                cut2 = cut[1].Split('\'');
                txvideo.Text = cut2[0];
            }
            else
            {
                cut = q.Split(new string[] { "embed/" }, StringSplitOptions.None);
                cut2 = cut[1].Split('&');
                txvideo.Text = "http://www.youtube.com/watch?v="+cut2[0];
                do
                {
                    quality_ComboBox.Items.Clear();
                    videoUrls = new string[] { txlink.Text };
                    List<YouTubeVideoQuality> urls = YouTubeDownloader.GetYouTubeVideoUrls(videoUrls);
                    quality_ComboBox.Enabled = true;
                    foreach (var item in urls)
                    {
                        string videoExtention = item.Extention;
                        string videoDimension = formatSize(item.Dimension);
                        string videoSize = formatSizeBinary(item.VideoSize);

                        quality_ComboBox.Items.Add(String.Format("{0} ({1}) - {2}", videoExtention.ToUpper(), videoDimension, videoSize));
                        quality_ComboBox.Text = quality_ComboBox.Items[0].ToString();
                        quality_ComboBox.Enabled = true;
                        downVideoUrls.Add(item);
                        txvideoadi.Text = downVideoUrls[quality_ComboBox.SelectedIndex].VideoTitle;
                        if (radioButton6.Checked)
                            quality_ComboBox.SelectedIndex = quality_ComboBox.Items.Count - 1;
                        txvideo.Text = downVideoUrls[quality_ComboBox.SelectedIndex].DownloadUrl;
                    }
                }
                while (quality_ComboBox.Items.Count < 5);
            }
            string[] title = q.Split(new string[] { "<TITLE>" }, StringSplitOptions.None);
            string[] title2 = title[1].Split(new string[] { "</TITLE>" }, StringSplitOptions.None);
            string[] desc = q.Split(new string[] { "description\" content=\"" }, StringSplitOptions.None);
            string[] desc2 = desc[1].Split('"');
            txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
            txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);}catch{}

        }
        private void gamespot(string site)
        {
            try{string q = "";

            using (var client = new WebClient())
            {
                client.Encoding = Encoding.GetEncoding("ISO-8859-1");
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                q = client.DownloadString(site);
            }
            string[] cut = q.Split(new string[] { "video src=\"" }, StringSplitOptions.None);
            string[] cut2 = cut[1].Split('?');

            string[] title = q.Split(new string[] { "<title>" }, StringSplitOptions.None);
            string[] title2 = title[1].Split(new string[] { "</title>" }, StringSplitOptions.None);
            string[] desc = q.Split(new string[] { "description\" content=\"" }, StringSplitOptions.None);
            string[] desc2 = desc[1].Split('"');
            txvideoadi.Text = WebUtility.HtmlDecode(title2[0]);
            txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
            txvideo.Text = cut2[0];}catch{}

        }
        private void kgwcom(string site)
        {
            try{string q = "";

            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                q = client.DownloadString(site);
            }
            string[] cut = q.Split(new string[] { "url\": \"" }, StringSplitOptions.None);
            string[] cut2 = cut[1].Split('"');

            string[] title = q.Split(new string[] { "<title>" }, StringSplitOptions.None);
            string[] title2 = title[1].Split(new string[] { "</title>" }, StringSplitOptions.None);
            string[] desc = q.Split(new string[] { "description\" content=\"" }, StringSplitOptions.None);
            string[] desc2 = desc[1].Split('"');
            txvideoadi.Text = WebUtility.HtmlDecode(title2[0].Trim());
            txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
            txvideo.Text = cut2[0];
                }catch{}
        }
        private void kmovcom(string site)
        {
            try{string q = "";

            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                q = client.DownloadString(site);
            }
            string[] cut = q.Split(new string[] { "url\": \"" }, StringSplitOptions.None);
            string[] cut2 = cut[1].Split('"');

            string[] title = q.Split(new string[] { "<title>" }, StringSplitOptions.None);
            string[] title2 = title[1].Split(new string[] { "</title>" }, StringSplitOptions.None);
            string[] desc = q.Split(new string[] { "description\" content=\"" }, StringSplitOptions.None);
            string[] desc2 = desc[1].Split('"');
            txvideoadi.Text = WebUtility.HtmlDecode(title2[0].Trim());
            txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
            txvideo.Text = cut2[0];
                }catch{}
        }
        private void guardiancouk(string site)
        {
            try{string q = "";

            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                q = client.DownloadString(site);
            }
            string[] cut = q.Split(new string[] { "file: \"" }, StringSplitOptions.None);
            string[] cut2 = cut[1].Split('"');

            string[] title = q.Split(new string[] { "<title>" }, StringSplitOptions.None);
            string[] title2 = title[1].Split(new string[] { "</title>" }, StringSplitOptions.None);
            string[] desc = q.Split(new string[] { "description\" content=\"" }, StringSplitOptions.None);
            string[] desc2 = desc[1].Split('"');
            txvideoadi.Text = WebUtility.HtmlDecode(title2[0].Trim());
            txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
            txvideo.Text = cut2[0];
                }catch{}
        }
        private void azfamily(string site)
        {
            try{string q = "";

            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                q = client.DownloadString(site);
            }
            string[] cut = q.Split(new string[] { "url\": \"" }, StringSplitOptions.None);
            string[] cut2 = cut[1].Split('"');

            string[] title = q.Split(new string[] { "<title>" }, StringSplitOptions.None);
            string[] title2 = title[1].Split(new string[] { "</title>" }, StringSplitOptions.None);
            string[] desc = q.Split(new string[] { "description\" content=\"" }, StringSplitOptions.None);
            string[] desc2 = desc[1].Split('"');
            txvideoadi.Text = WebUtility.HtmlDecode(title2[0].Trim());
            txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
            txvideo.Text = cut2[0];
                }catch{}
        }
        private void kvuecom(string site)
        {
            try{string q = "";

            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                q = client.DownloadString(site);
            }
            string[] cut = q.Split(new string[] { "url\": \"" }, StringSplitOptions.None);
            string[] cut2 = cut[1].Split('"');

            string[] title = q.Split(new string[] { "<title>" }, StringSplitOptions.None);
            string[] title2 = title[1].Split(new string[] { "</title>" }, StringSplitOptions.None);
            string[] desc = q.Split(new string[] { "description\" content=\"" }, StringSplitOptions.None);
            string[] desc2 = desc[1].Split('"');
            txvideoadi.Text = WebUtility.HtmlDecode(title2[0].Trim());
            txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
            txvideo.Text = cut2[0];
                }catch{}
        }
        private void khoucom(string site)
        {
            try{string q = "";

            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                q = client.DownloadString(site);
            }
            string[] cut = q.Split(new string[] { "url\": \"" }, StringSplitOptions.None);
            string[] cut2 = cut[1].Split('"');

            string[] title = q.Split(new string[] { "<title>" }, StringSplitOptions.None);
            string[] title2 = title[1].Split(new string[] { "</title>" }, StringSplitOptions.None);
            string[] desc = q.Split(new string[] { "description\" content=\"" }, StringSplitOptions.None);
            string[] desc2 = desc[1].Split('"');
            txvideoadi.Text = WebUtility.HtmlDecode(title2[0].Trim());
            txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
            txvideo.Text = cut2[0];
                }catch{}
        }
        private void wwltv(string site)
        {
            try{string q = "";

            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                q = client.DownloadString(site);
            }
            string[] cut = q.Split(new string[] { "url\": \"" }, StringSplitOptions.None);
            string[] cut2 = cut[1].Split('"');

            string[] title = q.Split(new string[] { "<title>" }, StringSplitOptions.None);
            string[] title2 = title[1].Split(new string[] { "</title>" }, StringSplitOptions.None);
            string[] desc = q.Split(new string[] { "description\" content=\"" }, StringSplitOptions.None);
            string[] desc2 = desc[1].Split('"');
            txvideoadi.Text = WebUtility.HtmlDecode(title2[0].Trim());
            txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
            txvideo.Text = cut2[0];
                }catch{}
        }
        private void whas11(string site)
        {
            try{string q = "";

            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                q = client.DownloadString(site);
            }
            string[] cut = q.Split(new string[] { "url\": \"" }, StringSplitOptions.None);
            string[] cut2 = cut[1].Split('"');

            string[] title = q.Split(new string[] { "<title>" }, StringSplitOptions.None);
            string[] title2 = title[1].Split(new string[] { "</title>" }, StringSplitOptions.None);
            string[] desc = q.Split(new string[] { "description\" content=\"" }, StringSplitOptions.None);
            string[] desc2 = desc[1].Split('"');
            txvideoadi.Text = WebUtility.HtmlDecode(title2[0].Trim());
            txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
            txvideo.Text = cut2[0];
                }catch{}
        }
        private void wfaa(string site)
        {
            try{string q = "";

            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                q = client.DownloadString(site);
            }
            string[] cut = q.Split(new string[] { "url\": \"" }, StringSplitOptions.None);
            string[] cut2 = cut[1].Split('"');

            string[] title = q.Split(new string[] { "<title>" }, StringSplitOptions.None);
            string[] title2 = title[1].Split(new string[] { "</title>" }, StringSplitOptions.None);
            string[] desc = q.Split(new string[] { "description\" content=\"" }, StringSplitOptions.None);
            string[] desc2 = desc[1].Split('"');
            txvideoadi.Text = WebUtility.HtmlDecode(title2[0].Trim());
            txaciklama.Text = WebUtility.HtmlDecode(desc2[0]);
            txvideo.Text = cut2[0];
            }
            catch { }
        }



        #endregion



        public static string GetText()//get clipboard
        {
            IDataObject dataObj = Clipboard.GetDataObject();

            if (!dataObj.GetDataPresent(DataFormats.Text))
                return "";

            return dataObj.GetData(DataFormats.Text).ToString();

        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)//key alma
        {
            MessageBox.Show("Tarayıcınızda açılan adrese gidin ve bir adet kullanıcı adı önemli olmayan developer key yaratın.");
            System.Diagnostics.Process.Start("https://code.google.com/apis/youtube/dashboard/gwt/index.html");
            Form a = new Form();
            a.Show();
            a.Width = 905;
            a.Height = 260;
            a.TopMost = true;
            a.BackgroundImage = uploader.Properties.Resources.youtube;
        }
        private void button2_Click_1(object sender, EventArgs e)
        {
                if (radioButton1.Checked)
                {
                    HesapBul(Convert.ToInt16(numericUpDown1.Value));
                    Thread a = new Thread(new ThreadStart(serveruploadsingle));
                    a.Start();
                }
                else if (radioButton2.Checked)
                {
                    HesapBul(Convert.ToInt16(numericUpDown1.Value));
                    Thread b = new Thread(new ThreadStart(localuploadsingle));
                    b.Start();
                }
        }
        private void LinkCheck(string a)
        {
            quality_ComboBox.Items.Clear();
            txaciklama.Clear();
            txvideoadi.Clear();
            txvideo.Clear();
            txetiket.Clear();
            VideoAdı = " ";
            Açıklama = " ";
            etiketler = " ";
            downVideoUrls.Clear();
            if (a != null & a != "")
            {
                if (a.IndexOf("kuzu.tv") != -1)
                {
                    kuzutv(txlink.Text);
                }
                if (a.IndexOf("jokeroo.com") != -1)
                {
                    jokeroo(txlink.Text);
                }
                else if (a.IndexOf("wfaa.com") != -1)
                {
                    whas11(txlink.Text);
                }
                else if (a.IndexOf("whas11.com") != -1)
                {
                    whas11(txlink.Text);
                }
                else if (a.IndexOf("wwltv.com") != -1)
                {
                    wwltv(txlink.Text);
                }
                else if (a.IndexOf("khou.com") != -1)
                {
                    kvuecom(txlink.Text);
                }
                else if (a.IndexOf("kvue.com") != -1)
                {
                    kvuecom(txlink.Text);
                }
                else if (a.IndexOf("azfamily.com") != -1)
                {
                    azfamily(txlink.Text);
                }
                else if (a.IndexOf("guardian.co.uk") != -1)
                {
                    guardiancouk(txlink.Text);
                }
                else if (a.IndexOf("kmov.com") != -1)
                {
                    kgwcom(txlink.Text);
                }
                else if (a.IndexOf("kgw.com") != -1)
                {
                    kgwcom(txlink.Text);
                }
                else if (a.IndexOf("evo.co.uk") != -1)
                {
                    evocouk(txlink.Text);
                }
                else if (a.IndexOf("gamespot.com") != -1)
                {
                    gamespot(txlink.Text);
                }
                else if (a.IndexOf("ehow.com") != -1)
                {
                    Haber365(txlink.Text);
                }
                else if (a.IndexOf("vidwor.ru") != -1)
                {
                    vidworru(txlink.Text);
                }
                else if (a.IndexOf("myvideo.ge") != -1)
                {
                    myvideoge(txlink.Text);
                }
                else if (a.IndexOf("funnyjunk.com") != -1)
                {
                    ehow(txlink.Text);
                }
                else if (a.IndexOf("dailymotion.com") != -1)
                {
                    dailymotion(txlink.Text);
                }
                else if (a.IndexOf("ebaumsworld.com") != -1)
                {
                    ebaumsworld(txlink.Text);
                }
                else if (a.IndexOf("killsometime.com") != -1)
                {
                    killsometime(txlink.Text);
                }
                else if (a.IndexOf("izlesene.com") != -1)
                {
                    izlesene(txlink.Text);
                }
                /*else if (a.IndexOf("facebook.com") != -1)
                {
                    facebook(txlink.Text);
                }*/
                else if (a.IndexOf("yapfiles.ru") != -1)
                {
                    yapfiles(txlink.Text);
                }
                else if (a.IndexOf("krasview.ru") != -1)
                {
                    krasview(txlink.Text);
                }
                else if (a.IndexOf("video.day.az") != -1)
                {
                    dayaz(txlink.Text);
                }
                else if (a.IndexOf("kinostok.tv") != -1)
                {
                    kinostoktv(txlink.Text);

                }
                else if (a.IndexOf("extremevidazz.com") != -1)
                {
                    extremevidazz(txlink.Text);

                }
                else if (a.IndexOf("rutv.ru") != -1)
                {
                    rutvru(txlink.Text);

                }
                else if (a.IndexOf("vidmax.com") != -1)
                {
                    vidmax(txlink.Text);

                }
                else if (a.IndexOf("tvhaber.com") != -1)
                {
                    TvHaber(txlink.Text);

                }
                else if (a.IndexOf("r7.com/videos") != -1)
                {
                    r7com(txlink.Text);

                }
                else if (a.IndexOf("io.ua") != -1)
                {
                    ioua(txlink.Text);

                }
                else if (a.IndexOf("break.com") != -1)
                {
                    BreakCom(txlink.Text);

                }
                else if (a.IndexOf("dpccars.com") != -1)
                {
                    dpccars(txlink.Text);

                }
                else if (a.IndexOf("metacafe.com") != -1)
                {
                    MetaCafe(txlink.Text);

                }
                else if (a.IndexOf("ochevidets.ru") != -1)
                {
                    ochevidetsru(txlink.Text);

                }
                else if (a.IndexOf("funnyordie.com") != -1)
                {
                    funnyordie(txlink.Text);

                }
                else if (a.IndexOf("nexttv.com.tw") != -1)
                {
                    nexttvcomtw(txlink.Text);


                }
                else if (a.IndexOf("videobash.com") != -1)
                {
                    VideoBash(txlink.Text);

                }
                else if (a.IndexOf("voanews.com/media/video") != -1)
                {
                    VoaNews(txlink.Text);
                }
                else if (a.IndexOf("euronews.com") != -1)
                {
                    EuroNews(txlink.Text);
                }
                else if (a.IndexOf("videoizle.co") != -1)
                {
                    videoizleco(txlink.Text);
                }
                else if (a.IndexOf("prochan.com") != -1)
                {
                    prochan(txlink.Text);
                }
                else if (a.IndexOf("news.sky.com") != -1)
                {
                    NewsSky(txlink.Text);
                }
                else if (a.IndexOf("liveleak.com") != -1)
                {
                    LiveLeak(txlink.Text);
                }
                else if (a.IndexOf("ign.com") != -1)
                {
                    IGN(txlink.Text);
                }
                else if (a.IndexOf("stupidvideos.com") != -1)
                {
                    StupidVideos(txlink.Text);
                }
                else if (a.IndexOf("bing") != -1)
                {
                    Bing(txlink.Text);
                }
                else if (a.IndexOf("funnyplace") != -1)
                {
                    FunnyPlace(txlink.Text);
                }
                else if (a.IndexOf("ted.com") != -1)
                {
                    TED(txlink.Text);
                }
                else if (a.IndexOf("newslook") != -1)
                {
                    NewsLook(txlink.Text);
                }
                else if (a.IndexOf("sabah.com") != -1)
                {
                    Sabah(txlink.Text);
                }
                else if (a.IndexOf("sporxtv.com") != -1)
                {
                    SporXTV(txlink.Text);
                }
                else if (a.IndexOf("fanatik") != -1)
                {
                    Fanatik(txlink.Text);
                }
                else if (a.IndexOf("timsah.com") != -1)
                {
                    Timsah(txlink.Text);
                }
                else if (a.IndexOf("videonuz.ensonhaber") != -1)
                {
                    EnSonHaber(txlink.Text);

                }
                else if (a.IndexOf("webtv.hurriyet") != -1)
                {
                    Hurriyet(txlink.Text);

                }
                else if (a.IndexOf("video.haberturk") != -1)
                {
                    HaberTurk(txlink.Text);

                }
                else if (a.IndexOf("videolar.bugun") != -1)
                {
                    Bugün(txlink.Text);

                }
                else if (a.IndexOf("video.ekolay") != -1)
                {
                    EKolay(txlink.Text);

                }
                else if (a.IndexOf("video.cnnturk") != -1)
                {
                    CNNTurk(txlink.Text);

                }
                else if (a.IndexOf("nasil.tv") != -1)
                {
                    NasilTV(txlink.Text);

                }
                else if (a.IndexOf("fragmantv.com") != -1)
                {
                    FragmanTV(txlink.Text);

                }
                else if (a.IndexOf("fragmanlar.org") != -1)
                {
                    FragmanlarOrg(txlink.Text);

                }
                else if (a.IndexOf("video.tamindir") != -1)
                {
                    Tamindir(txlink.Text);

                }
                else if (a.IndexOf("milliyet.tv") != -1)
                {
                    MilliyetTv(txlink.Text);

                }
                else if (a.IndexOf("tvarsivi") != -1)
                {
                    TvArsivi(txlink.Text);

                }
                else if (a.IndexOf("haber365") != -1)
                {
                    Haber365(txlink.Text);

                }
                else if (a.IndexOf("www.youtube.com/watch?") != -1)
                {
                    do
                    {
                        quality_ComboBox.Items.Clear();
                        videoUrls = new string[] { txlink.Text };
                        List<YouTubeVideoQuality> urls = YouTubeDownloader.GetYouTubeVideoUrls(videoUrls);
                        quality_ComboBox.Enabled = true;
                        foreach (var item in urls)
                        {
                            string videoExtention = item.Extention;
                            string videoDimension = formatSize(item.Dimension);
                            string videoSize = formatSizeBinary(item.VideoSize);

                            quality_ComboBox.Items.Add(String.Format("{0} ({1}) - {2}", videoExtention.ToUpper(), videoDimension, videoSize));
                            quality_ComboBox.Text = quality_ComboBox.Items[0].ToString();
                            quality_ComboBox.Enabled = true;
                            downVideoUrls.Add(item);
                            txvideoadi.Text = downVideoUrls[quality_ComboBox.SelectedIndex].VideoTitle;
                            if (radioButton6.Checked)
                                quality_ComboBox.SelectedIndex = quality_ComboBox.Items.Count - 1;
                            txvideo.Text = downVideoUrls[quality_ComboBox.SelectedIndex].DownloadUrl;
                        }
                    }
                    while (quality_ComboBox.Items.Count < 5);
                }
                else if (a.IndexOf("iha.com.tr") != -1)
                {
                    IHA(txlink.Text);

                }
                else
                {
                    tsdurum2.Text = "Bilinmeyen Site";
                }
                if (txaciklama.Text == "" || txaciklama.Text == " " || txaciklama.Text == null)
                    txaciklama.Text = txvideoadi.Text;
                string[] q = txvideoadi.Text.Split(' ');
                foreach (string b in q)
                {
                    if (b.Length > 2)
                        txetiket.Text += b + ",";
                }
                for (int ke = 0; ke < q.Length; ke++)
                {
                    for (int le = 0; le < q.Length; le++)
                    {
                        if (!q[ke].Equals(q[le]) && q[ke].Length > 2)
                        {
                            txetiket.Text += q[le] + " " + q[ke] + ",";
                        }
                    }
                }
                VideoAdı = txvideoadi.Text;
                Açıklama = txaciklama.Text;
                etiketler = txetiket.Text;

            }
        }
        private string VideoTur(string box)
        {
            string a;
            switch (box)
            {
                case "Otomobiller ve Araçlar": a = "Autos"; break;
                case "Müzik": a = "Music"; break;
                case "Ev Hayvanları ve Hayvanlar": a = "Animals"; break;
                case "Spor": a = "Sports"; break;
                case "Seyahat ve Etkinlikler": a = "Travel"; break;
                case "Oyun": a = "Games"; break;
                case "Kişiler ve Bloglar": a = "People"; break;
                case "Eğlence": a = "Entertainment"; break;
                case "Haberler ve Politika": a = "News"; break;
                case "Nasıl Yapılır ve Stil": a = "Howto"; break;
                case "Eğitim": a = "Education"; break;
                case "Bilim ve Teknoloji": a = "Tech"; break;
                case "Kâr Amacı Gütmeyen Kuruluşlar ve Aktivizm": a = "Nonprofit"; break;
                case "Film ve Animasyon": a = "Movies"; break;
                default: a = "Bilinmiyor"; break;
            }
            return a;
        }
        private void Ayrıntı()
        {
            try
            {

                if (GetText().Substring(0, 7).ToString() != "http://")
                {
                    txlink.Text = "http://" + GetText();
                }
                else
                {
                    txlink.Text = GetText();
                }
            }
            catch (Exception ex)
            {
                hata += DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + " " + ex.ToString().Substring(0, ex.ToString().IndexOf(Environment.NewLine)) + Environment.NewLine;
            }
            finally
            {
                    LinkCheck(txlink.Text);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Ayrıntı();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
                youtube.Default.tip = "server";
            if (radioButton2.Checked)
                youtube.Default.tip = "pc";
            if (radioButton3.Checked)
                youtube.Default.video = "kalsın";
            if (radioButton4.Checked)
                youtube.Default.video = "sil";
            if (radioButton5.Checked)
                youtube.Default.kalite="yuksek";
            if (radioButton6.Checked)
                youtube.Default.kalite = "dusuk";

            youtube.Default.kanal1 = txid.Text + "|" + txpw.Text + "|" + txytpw.Text;
            youtube.Default.kanal2 = textBox9.Text + "|" + textBox10.Text + "|" + textBox11.Text;
            youtube.Default.kanal3 = textBox12.Text + "|" + textBox13.Text + "|" + textBox14.Text;
            youtube.Default.kanal4 = textBox15.Text + "|" + textBox16.Text + "|" + textBox17.Text;

            youtube.Default.Save();
        }
        private void VersionCheck()
        {
            /*string q = "";
            
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                q = client.DownloadString("http://www.omrclk.com/version.txt");
            }
            string[] g = q.Split('|');
            if (q.IndexOf(Version) != -1)
            {
                richTextBox1.Text = "Versiyon: " + Version + " | Yeni Versiyon Yok.";
            }
            else
            {
                richTextBox1.Text = "Versiyon: " + Version + " | Programın Yeni Versiyonu Çıktı. " + g[1];
            }
                */
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                panel1.Enabled = true;
                panel2.Enabled = true;
                panel3.Enabled = true;
                panel4.Enabled = true;
                groupBox6.Enabled = false;
                groupBox2.Enabled = false;

                string[] pi = youtube.Default.kanal1.Split('|');
                txid.Text = pi[0];
                txpw.Text = pi[1];
                txytpw.Text = pi[2];
                string[] pi2 = youtube.Default.kanal2.Split('|');
                textBox9.Text = pi2[0];
                textBox10.Text = pi2[1];
                textBox11.Text = pi2[2];
                string[] pi3 = youtube.Default.kanal3.Split('|');
                textBox12.Text = pi3[0];
                textBox13.Text = pi3[1];
                textBox14.Text = pi3[2];
                string[] pi4 = youtube.Default.kanal4.Split('|');
                textBox15.Text = pi4[0];
                textBox16.Text = pi4[1];
                textBox17.Text = pi4[2];
            }
            catch
            { }
            this.Text = "Youtube Video Uploader | Saat: " + DateTime.Now.ToString("HH:MM");
            this.Icon = uploader.Properties.Resources.search;
            CheckForIllegalCrossThreadCalls = false;
            Thread a = new Thread(new ThreadStart(VersionCheck));
            a.Start();
            textBox1.Text = youtube.Default.uyeid;
            textBox2.Text = youtube.Default.uyepw;
            if (youtube.Default.tip == "pc")
                radioButton2.Checked = true;
            if (youtube.Default.tip == "server")
                radioButton1.Checked = true;
            if (youtube.Default.video=="sil")
                radioButton4.Checked = true;
            if (youtube.Default.video=="kalsın")
                radioButton3.Checked = true;
            if(youtube.Default.kalite=="yuksek")
                radioButton5.Checked=true;
            if(youtube.Default.kalite=="dusuk")
                radioButton6.Checked=true;
        }
        private void button8_Click(object sender, EventArgs e)
        {
            youtube.Default.uyeid = textBox1.Text;
            youtube.Default.uyepw = textBox2.Text;
            youtube.Default.Save();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            CheckMember();
            timer1.Enabled = true;
        }
        private string GirisVersion()
        {
            string q;
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.13) Gecko/20101203 Firefox/3.6.13";
                q = client.DownloadString("http://www.omrclk.com/version.txt");
            }
            string[] g = q.Split('|');
            return g[0];
        }
        private void CheckMember()
        {
            giris = false;
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                using (MySqlConnection cnn = new MySqlConnection("Server=5.2.81.121;Database=omrclkco_public;uid=omrclkco_admin;pwd=6528yayla"))
                {
                    MySqlCommand cmd = new MySqlCommand("select * from youtube where uyeadi='" + textBox1.Text + "' AND password='" + textBox2.Text + "'", cnn);
                    try
                    {
                        cnn.Open();
                        MySqlDataReader dr = cmd.ExecuteReader();
                        dr.Read();
                        if (dr.HasRows)
                        {
                            if (Version != "v4" && Version!="v5"&&Version!="v6")
                            {
                                MessageBox.Show("Programınız Güncel Değil. Lütfen Güncelleyin.");
                                cnn.Close();
                            }
                            if (dr["auth"].ToString() == "0")
                            {
                                if (ServerSaati() >= 17 && ServerSaati() < 18)
                                {
                                    MessageBox.Show("Ücretsiz Kullanım Saati İçerisindesiniz.");
                                    ucretsiz = true;
                                    giris = true;
                                    panel1.Enabled = true;
                                    panel2.Enabled = true;
                                    panel3.Enabled = true;
                                    panel4.Enabled = true;
                                    groupBox6.Enabled = false;
                                    groupBox2.Enabled = false;
                                    cnn.Close();
                                    cnn.Open();
                                    dr.Close();
                                    MySqlCommand guncelle = new MySqlCommand("update youtube set connect=1,version ='"+Version+"' where uyeadi='" + textBox1.Text + "' AND password='" + textBox2.Text + "' ", cnn);
                                    guncelle.ExecuteNonQuery();
                                }
                                else
                                {
                                    MessageBox.Show("Üyeliğiniz Kapatılmış veya Onaylanmamış.");
                                    dr.Close();
                                }
                            }
                            else if (dr["connect"].ToString() == "1")
                            {
                                MessageBox.Show("Üyeliğiniz Giriş Yapmış Durumda.");
                                dr.Close();
                            }
                            else if (dr["gun"].ToString() == "0")
                            {
                                if (ServerSaati() >= 17 && ServerSaati() < 18)
                                {
                                    MessageBox.Show("Ücretsiz Kullanım Saati İçerisindesiniz.");
                                    ucretsiz = true;
                                    giris = true;
                                    panel1.Enabled = true;
                                    panel2.Enabled = true;
                                    panel3.Enabled = true;
                                    panel4.Enabled = true;
                                    groupBox6.Enabled = false;
                                    groupBox2.Enabled = false;
                                    cnn.Close();
                                    cnn.Open();
                                    dr.Close();
                                    MySqlCommand guncelle = new MySqlCommand("update youtube set connect=1,version ='" + Version + "' where uyeadi='" + textBox1.Text + "' AND password='" + textBox2.Text + "' ", cnn);
                                    guncelle.ExecuteNonQuery();
                                }
                                else
                                {
                                    MessageBox.Show("Gününüz Kalmamış.");
                                    dr.Close();
                                }
                            }
                            else
                            {
                                tsdurum2.Text = "Giriş Yaptınız.";
                                textBox1.Enabled = false;
                                textBox2.Enabled = false;
                                tskalan2.Text = dr["gun"].ToString() +" Gün";
                                cnn.Close();
                                dr.Close();
                                cnn.Open();
                                giris = true;
                                panel1.Enabled = true;
                                panel2.Enabled = true;
                                panel3.Enabled = true;
                                panel4.Enabled = true;
                                groupBox6.Enabled = false;
                                groupBox2.Enabled = false;
                                MySqlCommand guncelle = new MySqlCommand("update youtube set connect=1,version ='" + Version + "' where uyeadi='" + textBox1.Text + "' AND password='" + textBox2.Text + "' ", cnn);
                                guncelle.ExecuteNonQuery();

                            }

                        }
                        else
                        {
                            MessageBox.Show("Üyelik Bulunamadı.");
                            dr.Close();
                            cnn.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        hata += DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + " " + ex.ToString().Substring(0, ex.ToString().IndexOf(Environment.NewLine)) + Environment.NewLine;
                            tsdurum2.Text = "Hata. Hata Penceresinden Görebilirsiniz.";
                    }
                }
            }
            else
                MessageBox.Show("Kullanıcı Adı ve Şifrenizi Giriniz.");
        }
        private void button10_Click(object sender, EventArgs e)
        {
            if (textBox3.TextLength >= 5 && textBox4.TextLength >= 5)
            {
                using (MySqlConnection cnn = new MySqlConnection("Server=5.2.81.121;Database=omrclkco_public;uid=omrclkco_admin;pwd=6528yayla"))
                {
                    try
                    {
                        cnn.Open();
                        MySqlCommand cmd = new MySqlCommand("select uyeadi from youtube where uyeadi='" + textBox3.Text + "'", cnn);
                        MySqlDataReader q = cmd.ExecuteReader();
                        if (q.HasRows)
                        {
                            MessageBox.Show("Kullanıcı Adı Alınmış.");
                            cnn.Close();
                            q.Close();

                        }
                        else
                        {
                            cnn.Close();
                            MessageBox.Show("Başarıyla Kayıt Oldunuz.");
                            cnn.Open();
                            MySqlCommand guncelle = new MySqlCommand("INSERT INTO youtube(uyeadi,password) VALUES ('" + textBox3.Text + "','" + textBox4.Text + "')", cnn);
                            guncelle.ExecuteNonQuery();
                            cnn.Close();
                        }

                    }
                    catch (Exception ex)
                    {
                        hata += DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + " " + ex.ToString().Substring(0, ex.ToString().IndexOf(Environment.NewLine)) + Environment.NewLine;
                            tsdurum2.Text = "Hata. Hata Penceresinden Görebilirsiniz.";
                    }

                }
            }
            else
                MessageBox.Show("Kullanıcı Adı ve Şifre 5 Karakterden Kısa Olamaz.");
        }
        private void button11_Click(object sender, EventArgs e)
        {
            if (txoldpw.Text != "" && txnewpw.Text != "" && txchangeid.Text !="")
            {
                using (MySqlConnection cnn = new MySqlConnection("Server=5.2.81.121;Database=omrclkco_public;uid=omrclkco_admin;pwd=6528yayla"))
                {
                    MySqlCommand cmd = new MySqlCommand("select * from youtube where uyeadi='" + txchangeid.Text + "' AND password='" + txoldpw.Text + "'", cnn);
                    try
                    {
                        cnn.Open();
                        MySqlDataReader dr = cmd.ExecuteReader();
                        dr.Read();
                        if (dr.HasRows)
                        {
                            dr.Close();
                            cnn.Close();
                            cnn.Open();
                            MySqlCommand guncelle = new MySqlCommand("update youtube set password='"+txnewpw.Text+"' where uyeadi='" + txchangeid.Text + "' AND password='" + txoldpw.Text + "'", cnn);
                            guncelle.ExecuteNonQuery();
                            MessageBox.Show("Şifreniz Değiştirildi.");
                            cnn.Close();

                        }
                        else
                        {
                            MessageBox.Show("Üyelik Bulunamadı.");
                            dr.Close();
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        hata += DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + " " + ex.ToString().Substring(0, ex.ToString().IndexOf(Environment.NewLine)) + Environment.NewLine;
                            tsdurum2.Text = "Hata. Hata Penceresinden Görebilirsiniz.";
                    }
                }
            }
            else
                MessageBox.Show("Kullanıcı Adı ve Şifrenizi Giriniz.");
        }
        private void button9_Click(object sender, EventArgs e)
        {
            Thread a = new Thread(new ThreadStart(TRY));
            a.Start();
        }
        private void TRY()
        {
            foreach (string g in listBox2.Items)
            {
                do
                {
                    quality_ComboBox.Items.Clear();
                    videoUrls = new string[] { g };
                    List<YouTubeVideoQuality> urls = YouTubeDownloader.GetYouTubeVideoUrls(videoUrls);
                    quality_ComboBox.Enabled = true;
                    foreach (var item in urls)
                    {
                        string videoExtention = item.Extention;
                        string videoDimension = formatSize(item.Dimension);
                        string videoSize = formatSizeBinary(item.VideoSize);

                        quality_ComboBox.Items.Add(String.Format("{0} ({1}) - {2}", videoExtention.ToUpper(), videoDimension, videoSize));
                        quality_ComboBox.Text = quality_ComboBox.Items[0].ToString();
                        quality_ComboBox.Enabled = true;
                        downVideoUrls.Add(item);
                        txvideoadi.Text = downVideoUrls[quality_ComboBox.SelectedIndex].VideoTitle;
                        txaciklama.Text = downVideoUrls[quality_ComboBox.SelectedIndex].Extention;
                        txvideo.Text = downVideoUrls[quality_ComboBox.SelectedIndex].DownloadUrl;
                    }
                }
                while (quality_ComboBox.Items.Count < 2);
            }
        }
        private void button12_Click(object sender, EventArgs e)
        {
            if (textBox5.Text != "" && textBox6.Text != "")
            {
                using (MySqlConnection cnn = new MySqlConnection("Server=5.2.81.121;Database=omrclkco_public;uid=omrclkco_admin;pwd=6528yayla"))
                {
                    try
                    {
                        cnn.Open();
                        MySqlCommand guncelle = new MySqlCommand("update youtube set connect=0 where uyeadi='" + textBox5.Text + "' AND password='" + textBox6.Text + "'", cnn);
                        guncelle.ExecuteNonQuery();
                        tsdurum2.Text = "Üyeliğiniz Askıdan Kurtarıldı.";
                    }
                    catch (Exception ex)
                    {
                        hata += DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + " " + ex.ToString().Substring(0, ex.ToString().IndexOf(Environment.NewLine)) + Environment.NewLine;
                            tsdurum2.Text = "Hata. Hata Penceresinden Görebilirsiniz.";
                    }

                }
            }
            else
                MessageBox.Show("Kullanıcı Adınızı ve Şifrenizi Girin.");
        }
        private int Gizlilik()
        {
            int a = 1;
            if (comboBox2.Text != "Herkese Açık")
                a = 0;
            return a;


        }
        public static string Encrypt(string plainText)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));

            byte[] cipherTextBytes;

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                    cryptoStream.Close();
                }
                memoryStream.Close();
            }
            return Convert.ToBase64String(cipherTextBytes);
        }
        private void button6_Click(object sender, EventArgs e)
        {
            lblcompleted.Text = "0";
            lblerror.Text = "0";
            listBox1.Items.Clear();
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
            lblmax.Text = listBox1.Items.Count.ToString();

        }
        private void button7_Click(object sender, EventArgs e)
        {
            Thread a = new Thread(new ThreadStart(localuploadmulti));
            Thread b = new Thread(new ThreadStart(serveruploadmulti));
            if (button7.Text == "Videolarımı Yüklemeye Başla")
            {
                lblmax.Text = listBox1.Items.Count.ToString();
                button7.Text = "Yüklemeyi Durdur";
                if (radioButton2.Checked)
                {
                    HesapBul(Convert.ToInt16(numericUpDown1.Value));
                    a.Start();
                }
                else if (radioButton1.Checked)
                {
                    HesapBul(Convert.ToInt16(numericUpDown1.Value));
                    b.Start();
                }
            }
            else
            {
                if (radioButton1.Checked)
                    b.Abort();
                if (radioButton2.Checked)
                    a.Abort();
                button7.Text = "Videolarımı Yüklemeye Başla";
                tsdurum2.Text = "Yükleme Durduruldu.";
            }
        }

        private void label25_Click(object sender, EventArgs e)
        {
            Process.Start("https://accounts.google.com/UnlockCaptcha?");
        }
        private void VideoDelete()
        {
            try
            {
                tsdurum2.Text = "Yüklenen Video Silindi";
                string FileToDelete;
                // Set full path to file 
                FileToDelete = videoismi;
                // Delete a file
                File.Delete(FileToDelete);
            }
            catch { }
        }
        private void VideoDownload(string url)
        {
            try
            {
                tsdurum2.Text = "Video İndiriliyor..";
                videoismi = "";
                Random a = new Random();
                string video = a.Next(10000000, 99999999).ToString();
                string name = video + ".flv";
                videoismi = video + ".flv";
                WebClient b = new WebClient();
                //b.Proxy = null;
                b.DownloadFile(url, name);
            }
            catch { }
        }
        private void YoutubeUpload()
        {
            youtubesonuc = true;
            tsdurum2.Text = "Video Yükleniyor..";
            try
            {
                VideoAdı = txvideoadi.Text;
                Açıklama = txaciklama.Text;
                etiketler = txetiket.Text;
                Random a = new Random();
                string id = textBox1.Text + a.Next(100000, 999999).ToString();
                YouTubeRequestSettings settings = new YouTubeRequestSettings(id, developerkey, hesapadi, hesapsifresi);
                YouTubeRequest request = new YouTubeRequest(settings);

                Video newVideo = new Video();
                ((GDataRequestFactory)request.Service.RequestFactory).Timeout = 9999999;
                newVideo.Title = VideoAdı;
                newVideo.Tags.Add(new MediaCategory(VideoTur(comboBox1.Text), YouTubeNameTable.CategorySchema));
                newVideo.Keywords = etiketler;
                newVideo.Description = Açıklama;
                if (Gizlilik() == 1)
                    newVideo.YouTubeEntry.Private = false;
                else
                    newVideo.YouTubeEntry.Private = true;
                newVideo.YouTubeEntry.MediaSource = new MediaFileSource(videoismi, "video/x-flv");

                request.Upload(newVideo);
            }
            catch (Exception ex)
            {
                hata += ex.ToString();
                tsdurum2.Text = "Hata.";
                youtubesonuc = false;
            }
            if (radioButton4.Checked)
                VideoDelete();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (textBox7.Text!="")
            {
                bool b = true;
                using (MySqlConnection cnn = new MySqlConnection("Server=5.2.81.121;Database=omrclkco_public;uid=omrclkco_admin;pwd=6528yayla"))
                {
                    try
                    {
                        cnn.Open();
                        MySqlCommand guncelle = new MySqlCommand("INSERT INTO tavsiye(metin,mail) VALUES ('" + textBox7.Text + "','"+textBox8.Text+"')", cnn);
                        guncelle.ExecuteNonQuery();
                        MessageBox.Show("Gönderildi. Teşekkürler.");

                    }
                    catch
                    {
                        b = false;
                    }
                    finally
                    {
                        if (b)
                            MessageBox.Show("Gönderildi.");
                        else
                            MessageBox.Show("Hata");
                    }
                }
        }
        }
        private int ServerSaati()
        {
            string saat = "0";
            using (MySqlConnection cnn = new MySqlConnection("Server=5.2.81.121;Database=omrclkco_public;uid=omrclkco_admin;pwd=6528yayla"))
            {
                cnn.Open();
                MySqlCommand time = new MySqlCommand("Select curtime() as saat", cnn);
                MySqlDataReader a = time.ExecuteReader();
                a.Read();
                saat = a["saat"].ToString();
            }
            string[] saat2 = saat.Split(':');
            return Convert.ToInt16(saat2[0]);

        }
        private void button3_Click(object sender, EventArgs e)
        {
            string devkey = "AI39si7mKq1rg-OK7-ZVgMfCB3Cd4GzWty9DyWg4JnHgFWzqrt3H3qy_-WH-exuCqlQgUQUjuOQtiuNoylM65XzkUp851jdnpA";
            string hesap = "by.sleepy@gmail.com";
            string sifre = "6528yayla";
            up go1 = new up(devkey, hesap, sifre, "deneme1", "aciklama deneme1", "dene,dene1,dene2", "Entertainment", "deneme1.mp4");
            up go2 = new up(devkey, hesap, sifre, "deneme2", "aciklama deneme2", "dene3,dene4,dene5", "Entertainment", "deneme2.mp4");
            up go3 = new up(devkey, hesap, sifre, "deneme3", "aciklama deneme3", "dene6,dene7,dene8", "Entertainment", "deneme3.mp4");
            Thread go11 = new Thread(new ThreadStart(go1.upload));
            txvideo.Text += "go1 start";
            Thread go21 = new Thread(new ThreadStart(go2.upload));
            txvideo.Text += "go2 start";
            Thread go31 = new Thread(new ThreadStart(go3.upload));
            txvideo.Text += "go3 start";

            
        }
        protected byte[] ExtractAudio(Stream stream)
        {
            var reader = new BinaryReader(stream);

            // Is stream a Flash Video stream
            if (reader.ReadChar() != 'F' || reader.ReadChar() != 'L' || reader.ReadChar() != 'V')
                throw new IOException("The file is not a FLV file.");

            // Is audio stream exists in the video stream
            var version = reader.ReadByte();
            var exists = reader.ReadByte();

            if ((exists != 5) && (exists != 4))
                throw new IOException("No Audio Stream");

            reader.ReadInt32(); // data offset of header. ignoring

            var output = new List<byte>();

            while (true)
            {
                try
                {
                    reader.ReadInt32(); // PreviousTagSize0 skipping

                    var tagType = reader.ReadByte();

                    while (tagType != 8)
                    {
                        var skip = ReadNext3Bytes(reader) + 11;
                        reader.BaseStream.Position += skip;

                        tagType = reader.ReadByte();
                    }

                    var DataSize = ReadNext3Bytes(reader);

                    reader.ReadInt32(); //skip timestamps 
                    ReadNext3Bytes(reader); // skip streamID
                    reader.ReadByte(); // skip audio header

                    for (int i = 0; i < DataSize - 1; i++)
                        output.Add(reader.ReadByte());
                }
                catch
                {
                    break;
                }
            }

            return output.ToArray();
        }

        private long ReadNext3Bytes(BinaryReader reader)
        {
            try
            {
                return Math.Abs((reader.ReadByte() & 0xFF) * 256 * 256 + (reader.ReadByte() & 0xFF)
                    * 256 + (reader.ReadByte() & 0xFF));
            }
            catch
            {
                return 0;
            }
        }
        private void label7_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Yüklenmesini İstediğiniz Tüm Videoların Linklerini Tek Bir .TXT Dosyasına" + Environment.NewLine + "Koyup, Programa Sırasıyla Yükletebilirsiniz.");

        }


        private string formatSize(object value)
        {
            string s = ((Size)value).Height >= 720 ? " HD" : "";
            if (value is Size) return ((Size)value).Width + " x " + ((Size)value).Height + s;
            return "";
        }
        private string formatSizeBinary(Int64 size, Int32 decimals = 2)
        {
            string[] sizes = { "Bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
            double formattedSize = size;
            Int32 sizeIndex = 0;
            while (formattedSize >= 1024 & sizeIndex < sizes.Length)
            {
                formattedSize /= 1024;
                sizeIndex += 1;
            }
            return string.Format("{0} {1}", Math.Round(formattedSize, decimals).ToString(), sizes[sizeIndex]);
        }

        public static string FormatTitle(string title)
        {
            return title.Replace(@"\", "").Replace("&#39;", "'").Replace("&quot;", "'").Replace("&lt;", "(").Replace("&gt;", ")").Replace("+", " ").Replace(":", "-");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (textBox1.Text != "" & textBox2.Text != "")
            {
                using (MySqlConnection cnn = new MySqlConnection("Server=5.2.81.121;Database=omrclkco_public;uid=omrclkco_admin;pwd=6528yayla"))
                {
                    cnn.Open();
                    MySqlCommand guncelle = new MySqlCommand("update youtube set connect=0 where uyeadi='" + textBox1.Text + "' AND password='" + textBox2.Text + "'", cnn);
                    guncelle.ExecuteNonQuery();
                }
            }
            Environment.Exit(0);
        }
        private void button14_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Text Dosyası Seçiniz ( Birden çok dosya seçebilirsiniz )";
            dialog.Filter = " (*.txt)|*.txt";
            dialog.FilterIndex = 1;
            dialog.Multiselect = true;
            DialogResult a = dialog.ShowDialog();
            if (a == DialogResult.OK)
                foreach (string str in dialog.FileNames)
                {
                    StreamReader stRead = new StreamReader(str, Encoding.Default);
                    while (!stRead.EndOfStream)
                    {
                        try
                        {
                            string site = stRead.ReadLine();
                            string[] site2 = site.Split('|');
                            listView1.Items.Add(new ListViewItem(new string[] { site2[0], site2[1], site2[2], site2[3] }));
                        }
                        catch
                        {
                            MessageBox.Show("Yüklediğiniz txt dosyasında sorun var. Düzgün yüklemezseniz düzgün çalışmaz.");
                            break;
                        }
                    }
                }
            int adet = 0;
            foreach (ListViewItem j in listView1.Items)
            {
                adet++;
            }
            label35.Text = adet.ToString();
        }
        private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }
        private void button15_Click(object sender, EventArgs e)
        {
            label35.Text = "0";
            label36.Text = "0";
            label37.Text = "0";
            Thread a = new Thread(new ThreadStart(localuploadmultimanual));
            a.Start();
        }

        private void hatayıGösterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(hata);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            /*if(ucretsiz)
                if (ServerSaati() > 18)
                {
                    AutoClosingMessageBox.Show("Ücretsiz Kullanım Saati Dolmuştur.", "Bitti", 3000);
                    Application.Exit();
                }
            this.Text = "Youtube Video Uploader | Saat: " + DateTime.Now.ToString("HH:MM") + " | 17-18 Arası Ücretsiz Kullanım Saatidir.";
            if (giris)
            {
                using (MySqlConnection cnn = new MySqlConnection("Server=5.2.81.121;Database=omrclkco_public;uid=omrclkco_admin;pwd=6528yayla"))
                {
                    MySqlCommand cmd = new MySqlCommand("select * from youtube where uyeadi='" + textBox1.Text + "' AND password='" + textBox2.Text + "'", cnn);
                    try
                    {
                        cnn.Open();
                        MySqlDataReader dr = cmd.ExecuteReader();
                        dr.Read();
                        if (dr["connect"].ToString()=="0" && giris)
                        {
                            AutoClosingMessageBox.Show("Hesabınız Düştü. Başka Yerden Giriş Yapılmış Olabilir.", "Dikkat!", 3000);
                            Application.Exit();
                        }
                    }
                    catch { }
                }
            }*/
        }
        public class AutoClosingMessageBox
        {
            System.Threading.Timer _timeoutTimer;
            string _caption;
            AutoClosingMessageBox(string text, string caption, int timeout)
            {
                _caption = caption;
                _timeoutTimer = new System.Threading.Timer(OnTimerElapsed,
                    null, timeout, System.Threading.Timeout.Infinite);
                MessageBox.Show(text, caption);
            }
            public static void Show(string text, string caption, int timeout)
            {
                new AutoClosingMessageBox(text, caption, timeout);
            }
            void OnTimerElapsed(object state)
            {
                IntPtr mbWnd = FindWindow(null, _caption);
                if (mbWnd != IntPtr.Zero)
                    SendMessage(mbWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                _timeoutTimer.Dispose();
            }
            const int WM_CLOSE = 0x0010;
            [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
            static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
            [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
            static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
        }
        private void serveruploadmulti()
        {
            foreach (string link in listBox1.Items)
            {
                txlink.Text = link;
                    string response = "";
                    tsdurum2.Text = "Yükleniyor...";
                    string a = "http://www.omrclk.com/yt/get.aspx?id=" + txid.Text + "&video=" + txvideo.Text + "&pw=" + txpw.Text + "&devkey=" + txytpw.Text + "&title=" + txvideoadi.Text + "&category=" + VideoTur(comboBox1.Text) + "&keywords=" + txvideo.Text + "&description=" + txaciklama.Text + "&visible=" + Gizlilik() + "&auth=" + auth;
                    a = a.Replace(" ", "%20");
                    WebRequest req = HttpWebRequest.Create(a);
                    req.Method = "GET";
                    using (StreamReader reader = new StreamReader(req.GetResponse().GetResponseStream()))
                    {
                        tsdurum2.Text = reader.ReadLine();
                        response = reader.ReadLine();
                    }
                    if (response == "Videonuz Yüklendi.")
                    {
                        tsdurum2.Text = "Yüklendi.";
                        lblcompleted.Text = Convert.ToString(Convert.ToInt16(lblcompleted.Text) + 1);
                    }
                    else
                        lblerror.Text = Convert.ToString(Convert.ToInt16(lblerror.Text) + 1);
                }
        }
        private void localuploadmultimanual()
        {
            //count 1 den başlar items 0 dan
            //subitemslar da 0 dan
            int sıra = 1;
            foreach (ListViewItem itemRow in listView1.Items)
            {
                tsdurum2.Text = sıra + " . Video Yükleniyor..";
                sıra++;
                string baslik = itemRow.SubItems[0].Text;
                string aciklama = itemRow.SubItems[1].Text;
                string etiket = itemRow.SubItems[2].Text;
                string link = itemRow.SubItems[3].Text;
                VideoAdı = baslik;
                Açıklama = aciklama;
                etiketler = etiket;
                videoismi = link;
                
                YoutubeUpload();
            }
            tsdurum2.Text = "Yüklemeler Tamamlandı.";
            button7.Text = "Videolarımı Yüklemeye Başla";
        }
        private void localuploadsingle()
        {
            VideoDownload(txvideo.Text);
            YoutubeUpload();
            tsdurum2.Text = "Yüklendi.";
        }
        private void serveruploadsingle()
        {
            VideoDownload(txvideo.Text);
            YoutubeUpload();
        }
        private void localuploadmulti()
        {
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                string[] pi = listBox1.Items[i].ToString().Split('|');
                listBox1.SetSelected(i, true);
                txlink.Text = pi[0];
                LinkCheck(pi[0]);
                VideoDownload(txvideo.Text);
                for (int j = 1; j < pi.Count(); j++)
                {
                    HesapBul(Convert.ToInt32(pi[j]));
                    tsdurum.Text = pi[j].ToString() + " .Hesaba Video Yükleniyor..";
                    YoutubeUpload();
                }
                if (youtubesonuc)
                    lblcompleted.Text = Convert.ToString(Convert.ToInt16(lblcompleted.Text) + 1);
                else
                    lblerror.Text = Convert.ToString(Convert.ToInt16(lblerror.Text) + 1);
            }
            tsdurum2.Text = "Yüklemeler Tamamlandı.";
        }

        private void richTextBox2_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }

        private void richTextBox3_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }

        private void richTextBox4_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }
        private void HesapBul(int s)
        {
            switch (s)
            {
                case 1:
                    hesapadi = txid.Text;
                    hesapsifresi = txpw.Text;
                    developerkey = txytpw.Text;
                    break;
                case 2:
                    hesapadi = textBox9.Text;
                    hesapsifresi = textBox10.Text;
                    developerkey = textBox11.Text;
                    break;
                case 3:
                    hesapadi = textBox12.Text;
                    hesapsifresi = textBox13.Text;
                    developerkey = textBox14.Text;
                    break;
                case 4:
                    hesapadi = textBox15.Text;
                    hesapsifresi = textBox16.Text;
                    developerkey = textBox17.Text;
                    break;
            }
        }

        private void richTextBox5_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            
            try{
                Remove(GetMyVideo("UCgr6mrIdTTg6jbOkQZpCgUA", "4RJ4H9S1epg"));
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public Video GetMyVideo(string uploader, string videoID)
        {
                YouTubeRequestSettings settings = new YouTubeRequestSettings("zohaan", "AI39si49oz87UsNqVvW32H5Ve3eJkrdPnctL0Q8kfbpGoqBpowKskuoYSC9iW_6_EVlxzXUpa0oPjZgHTYtXrEfTShULt3d1Dw", "cocumuyomu1@gmail.com", "6528yayla");
                YouTubeRequest request = new YouTubeRequest(settings);
                Uri uri = new Uri(String.Format("http://gdata.YouTube.com/feeds/api/users/{0}/uploads/{1}", uploader, videoID));
                return request.Retrieve<Video>(uri);

        }
        public void Remove(Video video)
        {
            YouTubeRequestSettings settings = new YouTubeRequestSettings("zohaan", "AI39si49oz87UsNqVvW32H5Ve3eJkrdPnctL0Q8kfbpGoqBpowKskuoYSC9iW_6_EVlxzXUpa0oPjZgHTYtXrEfTShULt3d1Dw", "cocumuyomu1@gmail.com", "6528yayla");
            YouTubeRequest request = new YouTubeRequest(settings);
            request.Delete(video);
        }

        private void txvideo_TextChanged(object sender, EventArgs e)
        {
            /*HttpWebRequest req = (HttpWebRequest)WebRequest.Create(txvideo.Text);
            req.Method = "HEAD";
            HttpWebResponse resp = (HttpWebResponse)(req.GetResponse());
            long len = resp.ContentLength;
            this.Text = (len/1024/1024).ToString()+" MB";*/
        }
    }
}
