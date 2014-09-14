using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace htmlagilitypack
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void emlakjet()
        {
            int oksay = 0;
            int satir = 1, sutun = 1;
            var fileName = "emlakjet.xlsx";
            var file = new FileInfo(Application.StartupPath + "\\" + fileName);
            using (var package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sayfa1");
                worksheet.Cells[1, 1].Value = "Adı";
                worksheet.Cells[1, 2].Value = "İlçe / İl";
                worksheet.Cells[1, 3].Value = "GSM";
                worksheet.Cells[1, 4].Value = "Telefon";
                string src = getSite("http://www.emlakjet.com/firmalar/index.html");
                string[] a = src.Split(new string[] { "<td class=\"padding_02 font_11 lineheight_01\" style=\"width:132px\"><a href=\"" }, StringSplitOptions.None);
                for (int i = 1; i < a.Length; i++)
                {
                    string[] a1 = a[i].Split('"');
                    string src1 = getSite("http://www.emlakjet.com" + a1[0]);
                    string[] a2 = src1.Split(new string[] { "<td class=\"padding_02 font_11 lineheight_01\" style=\"width:132px\"><a href=\"" }, StringSplitOptions.None);
                    for (int j = 1; j < a2.Length; j++)
                    {
                        string[] a3 = a2[j].Split('"');
                        string def = a3[0];
                        for(int ss = 0;ss<50;ss++)
                        {
                            if (ss > 0)
                                a3[0] = def.Replace("index.html", "index"+ss+".html");
                            listBox1.Items.Add(a3[0]);
                            int visibleItems = listBox1.ClientSize.Height / listBox1.ItemHeight;
                            listBox1.TopIndex = Math.Max(listBox1.Items.Count - visibleItems + 1, 0);
                            string src2 = getSite("http://www.emlakjet.com" + a3[0]);
                            if (src2.Contains("<td class=\"padding_09\"><a href=\""))
                            {
                                string[] a4 = src2.Split(new string[] { "<td class=\"padding_09\"><a href=\"" }, StringSplitOptions.None);
                                for (int l = 1; l < a4.Length; l++)
                                {
                                    satir++; sutun = 1;
                                    string[] a5 = a4[l].Split('"');
                                    string src3 = getSite("http://www.emlakjet.com" + a5[0]);
                                    string[] ad = src3.Split(new string[] { "class=\"border_02\" alt=\"" }, StringSplitOptions.None);
                                    string[] ad1 = ad[1].Split('"');

                                    string[] ii = src3.Split(new string[] { "<td class=\"font_11 color_01\">" }, StringSplitOptions.None);
                                    string[] ii1 = ii[1].Split('<');

                                    string cept = "0", ist = "0";
                                    if (src3.Contains("+90 (5"))
                                    {
                                        string[] tel = src3.Split(new string[] { "+90 (5" }, StringSplitOptions.None);
                                        string[] tel1 = tel[1].Split('<');
                                        cept = "+90 (5"+tel1[0];
                                    }
                                    if (src3.Contains("+90 (2"))
                                    {
                                        string[] tel = src3.Split(new string[] { "+90 (2" }, StringSplitOptions.None);
                                        string[] tel1 = tel[1].Split('<');
                                        ist ="+90 (2"+ tel1[0];
                                    }
                                    worksheet.Cells[satir, sutun].Value = ad1[0];
                                    sutun++;
                                    worksheet.Cells[satir, sutun].Value = ii1[0];
                                    sutun++;
                                    worksheet.Cells[satir, sutun].Value = cept;
                                    sutun++;
                                    worksheet.Cells[satir, sutun].Value = ist;
                                    sutun++;
                                }
                            }
                            else
                                break;
                        }

                    }

                }
                package.Save();
            }

            MessageBox.Show("bitti");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Thread a = new Thread(new ThreadStart(emlakjet));
            a.Start();
        }
        private string getSite(string url)
        {
            string src = "";
            using (WebClient wb = new WebClient())
            {
                wb.Encoding = Encoding.UTF8;
                src = wb.DownloadString(url);
            }
            return src;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string[] a = System.IO.File.ReadAllLines("linkler.txt");
            int satir = 1, sutun = 1;
            var fileName = "emlaknet.xlsx";
            var file = new FileInfo(Application.StartupPath + "\\" + fileName);
            using (var package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sayfa1");
                worksheet.Cells[1, 1].Value = "Adı";
                worksheet.Cells[1, 2].Value = "İlçe / İl";
                worksheet.Cells[1, 3].Value = "GSM";
                worksheet.Cells[1, 4].Value = "Telefon";
                foreach (string aa in a)
                {
                    satir++; sutun = 1;
                    string src = "";
                    using (WebClient wb = new WebClient())
                    {
                        src = wb.DownloadString("http://emlak.net"+aa);
                    }

                    string[] ad = src.Split(new string[] { "<title>" }, StringSplitOptions.None);
                    string[] ad1 = ad[1].Split('-');
                    ad1[0] = ad1[0].TrimEnd();

                    string cept = "0", ist = "0";
                    string[] tel = src.Split(new string[] { "colspan=\"2\">" }, StringSplitOptions.None);
                    for (int t = 1; t < tel.Length; t++)
                    {
                        string[] tel1 = tel[t].Split('<');
                        if (tel1[0].Contains("0 5"))
                            cept = tel1[0];
                        else
                            ist = tel1[0];
                    }

                    string[] ii = src.Split(new string[] { "class=\"underline black\">" }, StringSplitOptions.None);
                    string[] ii1 = ii[1].Split(new string[]{"<div"},StringSplitOptions.None);
                    ii1[0] = StripHTML(ii1[0]);


                    worksheet.Cells[satir, sutun].Value = ad1[0];
                    sutun++;
                    worksheet.Cells[satir, sutun].Value = ii1[0];
                    sutun++;
                    worksheet.Cells[satir, sutun].Value = cept;
                    sutun++;
                    worksheet.Cells[satir, sutun].Value = ist;
                    sutun++;
                }
                package.Save();
            }
        }
        static string StripHTML(string inputString)
        {
            return Regex.Replace
              (inputString, "<.*?>", string.Empty);
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
        }
    }
}
