using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Web;

namespace popularity_bot
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
        private void hepsiburadaCokSatanlar()
        {
            MessageBox.Show("hepsiburada started.");
            for (int i = 0; i <= 80; i += 20)
            {
                string html;
                using (WebClient wb = new WebClient())
                {
                    wb.Encoding = Encoding.UTF8;
                    html = HttpUtility.HtmlDecode(wb.DownloadString("http://www.hepsiburada.com/liste/coksatanlar/?q=fh_location%3d%2f%2fcatalog01%2ftr_TR%2finstock%3d1%2fitem_sale_count_pl%3E5%26fh_start_index%3d" + i + "%26fh_sort_by%3d-order_stock_attribute_pl%2c-item_sale_count_pl%26pqp%3dcoksatanlar%26sc%3d32%26pqpQueryAdded%3d1"));
                }
                string[] p = html.Split(new string[] { "<div class=\"HbImagesContainer140x140Yatay w140 h140 posR\">" }, StringSplitOptions.None);
                for (int j = 1; j < p.Length; j++)
                {
                    string[] p2 = p[j].Split(new string[] { "title=\"" }, StringSplitOptions.None);
                    string[] p3 = p2[1].Split('"');
                    listBox1.Items.Add(p3[0]);
                }
            }
        }
        private void n11CokSatanlar()
        {
            //main
            MessageBox.Show("n11 started.");
            string html;
            using (WebClient wb = new WebClient())
            {
                wb.Encoding = Encoding.UTF8;
                html = HttpUtility.HtmlDecode(wb.DownloadString("http://www.n11.com/cok-satanlar"));
            }
            string[] p = html.Split(new string[] { "<a class=\"img\" href=\"" }, StringSplitOptions.None);
            for (int j = 1; j < p.Length; j++)
            {
                string[] p2 = p[j].Split(new string[] { "title=\"" }, StringSplitOptions.None);
                string[] p3 = p2[1].Split('"');
                listBox1.Items.Add(p3[0]);
            }
        }
        private void zizigoCokSatanlar(string kategori, int sayfa_sayisi)
        {

        }
    }
}
