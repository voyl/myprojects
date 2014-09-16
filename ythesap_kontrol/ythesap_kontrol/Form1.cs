using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Google.YouTube;
using Google.GData.Client;
using System.Data.OleDb;
using System.Threading;
using System.Net;

namespace ythesap_kontrol
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
        private void Guncelle()
        {
            using (OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=hesap.mdb"))
            {
                conn.Open();
                OleDbCommand komut = new OleDbCommand();
                komut.Connection = conn;
                komut.CommandText = "Select * from hesap"; // sorgu / komut cumlemi yazıyorum.
                komut.ExecuteNonQuery(); // insert , updateiçin gerekli satir sayisi donduruyoruz.
                OleDbDataReader dr = komut.ExecuteReader(); // datareader olusturup komut sorgulayıp veritabaninda okuma işlemini tanıtıyoruz
                while (dr.Read()) // datareader ile okuyoruz.
                {
                    string hadi = dr["hadi"].ToString();
                    string kadi = dr["kadi"].ToString();
                    string q;
                    using (WebClient asd = new WebClient())
                    {
                        asd.Encoding = Encoding.UTF8;
                        q = asd.DownloadString("http://gdata.youtube.com/feeds/api/users/" + kadi + "/uploads?v=2&alt=jsonc&max-results=0");
                    }
                    string[] adet1 = q.Split(new string[] { "totalItems\":" }, StringSplitOptions.None);
                    string[] adet2 = adet1[1].Split(',');
                    listView1.Items.Add(new ListViewItem(new string[] { hadi, "Adet: "+adet2[0] }));
                }
                dr.Close();
                komut.ExecuteNonQuery(); // insert , updateiçin gerekli satir sayisi donduruyoruz.
                dr = komut.ExecuteReader(); // datareader olusturup komut sorgulayıp veritabaninda okuma işlemini tanıtıyoruz
                while (dr.Read()) // datareader ile okuyoruz.
                {
                    string kadi = dr["hadi"].ToString(); // veritabanimdaki "kadi" alanımdaki veriyi alip kadi değişkenine atıyorum(yukarıda string olusturmustum)
                    string sifre = dr["hsifresi"].ToString(); // aynı durum söz konusu
                    string devkey = dr["devkey"].ToString();
                    Random a = new Random();
                    string id = a.Next(100000, 999999).ToString();
                    YouTubeRequestSettings settings = new YouTubeRequestSettings(id, devkey, kadi,sifre);
                    YouTubeRequest request = new YouTubeRequest(settings);

                    string feedUrl = "https://gdata.youtube.com/feeds/api/users/default/uploads";

                    Feed<Video> videoFeed = request.Get<Video>(new Uri(feedUrl));
                    foreach (Video entry in videoFeed.Entries)
                    {
                        string vid_thumb ="http://img.youtube.com/vi/"+entry.VideoId+"/0.jpg";
                        int izlenme = entry.ViewCount;
                        if(izlenme == -1)
                            izlenme = 0;
                        listView1.Items.Add(new ListViewItem(new string[] { kadi,entry.YouTubeEntry.Title.Text,izlenme.ToString()  }));
                    }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            Thread a = new Thread(new ThreadStart(Guncelle));
            a.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
