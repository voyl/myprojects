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
using Microsoft.Office.Core;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Text.RegularExpressions;

namespace hp
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }
        List<string> iller = new List<string>(){"ankara",
"izmir",
"adana",
"adiyaman",
"afyonkarahisar",
"agri",
"aksaray",
"amasya",
"antalya",
"ardahan",
"artvin",
"aydinbalikesi",
"bartin",
"batman",
"bayburt",
"bilecik",
"bingol",
"bitlis",
"bolu",
"burdur",
"bursa",
"canakkale",
"cankiri",
"corum",
"denizli",
"diyarbakir",
"duzce",
"edirne",
"elazig",
"erzincan",
"erzurum",
"eskisehir",
"gaziantep",
"giresun",
"gumushane",
"hakkari",
"hatay",
"igdir",
"isparta",
"kahramanmaras",
"karabuk",
"karaman",
"kars",
"kastamonu",
"kayseri",
"kirikkale",
"kirklareli",
"kirsehir",
"kilis",
"kocaeli",
"konya",
"kutahya",
"malatya",
"manisa",
"mardin",
"mersin",
"mugla",
"mus",
"nevsehir",
"nigde",
"ordu",
"osmaniye",
"rize",
"sakarya",
"samsun",
"siirt",
"sinop",
"sivas",
"sanliurfa",
"sirnak",
"tekirdag",
"tokat",
"trabzon",
"tunceli",
"usak",
"van",
"yalova",
"yozgat",
"zonguldak"};
        List<string> names = new List<string>();
        List<string> names2 = new List<string>();
        const string HTML_TAG_PATTERN = "<.*?>";
        public string resim_ismi = "";

        static string StripHTML(string inputString)
        {
            return Regex.Replace
              (inputString, HTML_TAG_PATTERN, string.Empty);
        }
        private void FileDownload(string v_name, string url)
        {
            try
            {
                string fname;
                Random a = new Random();
                fname = "resim/" + v_name + ".jpg";
                resim_ismi = v_name + ".jpg";
                WebClient b = new WebClient();
                b.DownloadFile(url, fname);
            }
            catch { richTextBox1.Text += "File Download Error" + Environment.NewLine; }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Thread a = new Thread(new ThreadStart(goDolumTurk));
            a.SetApartmentState(ApartmentState.STA);
            a.Start();
        }
        private void goDolumTurk()
        {
            List<String> siteler = new List<string>();
            string[] a = richTextBox2.Text.Split(new string[] { "<a href=\"" }, StringSplitOptions.None);
            for (int i = 1; i < a.Length; i++)
            {
                if (i % 2 == 1 && i != a.Length)
                {
                    string[] b = a[i].Split('"');
                    listBox1.Items.Add("http://www.dolumturk.com/" + b[0]);
                    siteler.Add("http://www.dolumturk.com/" + b[0]);
                }
            }
            int satir = 0;
            int sutun = 1;
            Excel.Application excelApp = new Excel.Application();
            string myPath = @"C:\Excel.xlsx";
            excelApp.Workbooks.Open(myPath);
            excelApp.Visible = true;
            int ii = 1;
            foreach (string si in siteler)
            {
                this.Text = ii + ". Sayfa";
                ii++;
                satir++;
                sutun = 1;
                string q = "";
                using (WebClient client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    client.Headers.Add("user-agent", "Mozilla/5.0 (Windows; Windows NT 5.1; rv:1.9.2.4) Gecko/20100611 Firefox/3.6.4");
                    q = client.DownloadString(si);
                }
                string[] sk = q.Split(new string[] { "<td class=\"col3\">" }, StringSplitOptions.None);
                string[] sk1 = sk[2].Split('<');
                sk1[0] = sk1[0].Replace('"', ' ').Trim();

                excelApp.Cells[satir, sutun] = sk1[0]; //stock code
                sutun++;

                string[] t = q.Split(new string[] { "<h1>" }, StringSplitOptions.None);
                string[] t1 = t[1].Split(new string[] { "</h1>" }, StringSplitOptions.None);

                excelApp.Cells[satir, sutun] = t1[0];
                sutun++;

                excelApp.Cells[satir, sutun] = "1";//status
                sutun++;

                string[] br = q.Split(new string[] { "<div id='cat2'>" }, StringSplitOptions.None);
                string[] br1 = br[1].Split('>');
                string[] br2 = br1[3].Split('<');
                br2[0] = br2[0].Replace('"', ' ').Trim();
                excelApp.Cells[satir, sutun] = br2[0]; //brand
                sutun++;

                excelApp.Cells[satir, sutun] = br2[0]; //main category
                sutun++;

                string[] ct = q.Split(new string[] { "<li id" }, StringSplitOptions.None);
                string[] ct1 = ct[3].Split('>');
                string[] ct2 = ct1[2].Split('<');
                ct2[0] = ct2[0].Replace('"', ' ').Trim();
                excelApp.Cells[satir, sutun] = ct2[0]; //category
                sutun++;

                string[] pr = q.Split(new string[] { "<span itemprop='price'>" }, StringSplitOptions.None);
                string[] pr1 = pr[1].Split(' ');

                excelApp.Cells[satir, sutun] = pr1[0]; //price
                sutun++;

                excelApp.Cells[satir, sutun] = "TL"; //currencyAbr
                sutun++;

                excelApp.Cells[satir, sutun] = "1"; //stock amount
                sutun++;

                excelApp.Cells[satir, sutun] = "Adet"; //stock type
                sutun++;

                excelApp.Cells[satir, sutun] = "0"; //warranty
                sutun++;

                string[] img = q.Split(new string[] { "id='PrimaryImage' src='" }, StringSplitOptions.None);
                string[] img1 = img[1].Split('\'');

                excelApp.Cells[satir, sutun] = "http://www.dolumturk.com/" + img1[0]; // image
                sutun++;

                excelApp.Cells[satir, sutun] = "1"; //dm3
                sutun++;

                if (q.Contains("Uyum"))
                {
                    if (q.Contains("Uyumlu \\u00dcr\\u00fcnler"))
                    {
                        Clipboard.SetText(si);
                        string list = "";
                        string[] fl = q.Split(new string[] { "font-family:Arial;color:black\\\">" }, StringSplitOptions.None);
                        for (int j = 1; j < fl.Length; j++)
                        {
                            string[] fl1 = fl[j].Split('<');
                            list += fl1[0] + Environment.NewLine;
                        }
                        list = list.Replace("\\r\\n\\t\\t\\t\\t\\t\\t", "");
                        //MessageBox.Show("Uyumlu \u00dcr\u00fcnler"+Environment.NewLine+list);
                        excelApp.Cells[satir, sutun] = list;
                    }
                    else if (q.Contains("Uyumlu Printer Listesi") || q.Contains("Uyumlu printer listesi"))
                    {
                        Clipboard.SetText(si);
                        string list = "";
                        string[] fl = q.Split(new string[] { "list-style-type: none;\\\">" }, StringSplitOptions.None);
                        for (int j = 1; j < fl.Length; j++)
                        {
                            string[] fl1 = fl[j].Split('<');
                            list += fl1[0] + Environment.NewLine;
                        }
                        list = list.Replace("\\r\\n\\t\\t\\t\\t\\t\\t", "");
                        //MessageBox.Show("Uyumlu Printer Listesi"+Environment.NewLine+list);
                        excelApp.Cells[satir, sutun] = list;
                    }
                    else
                    {
                        richTextBox1.Text += "new! -> "+si + Environment.NewLine;
                        //break;
                    }
                }
            }
        }
        private void Cek()
        {
            List<String> siteler = new List<string>();
            siteler.Add("http://findyoursupplies.com/canhpshopping/?_slpcid=67");//deskjet
            siteler.Add("http://findyoursupplies.com/canhpshopping/?_slpcid=103");//photosmart
            siteler.Add("http://findyoursupplies.com/canhpshopping/?_slpcid=143");//officejet
            siteler.Add("http://findyoursupplies.com/canhpshopping/?_slpcid=5");//laserjet
            siteler.Add("http://findyoursupplies.com/canhpshopping/?_slpcid=216");//designjet
            int satir = 1;
            int sutun = 1;
            Excel.Application excelApp = new Excel.Application();
            string myPath = @"C:\Excel.xlsx";
            excelApp.Workbooks.Open(myPath);
            excelApp.Visible = true;

            foreach (string s in siteler)
            {
                string q;
                using (WebClient client = new WebClient())
                {
                    client.Headers.Add("user-agent", "Mozilla/5.0 (Windows; Windows NT 5.1; rv:1.9.2.4) Gecko/20100611 Firefox/3.6.4");
                    q = client.DownloadString(s);
                }
                string[] panes = q.Split(new string[] { "<h2>" }, StringSplitOptions.None);
                for (int i = 1; i < panes.Length; i++)
                {
                    string[] adi = panes[i].Split(new string[] { "</h2>" }, StringSplitOptions.None);
                    string[] son = panes[i].Split(new string[] { "</div>" }, StringSplitOptions.None);
                    string[] href = son[0].Split(new string[] { "<a href=\"" }, StringSplitOptions.None);
                    for (int j = 1; j < href.Length; j++)
                    {
                        sutun = 1;
                        satir++;
                        string[] bitti = href[j].Split('"');
                        string q2;
                        using (WebClient client = new WebClient())
                        {
                            client.Headers.Add("user-agent", "Mozilla/5.0 (Windows; Windows NT 5.1; rv:1.9.2.4) Gecko/20100611 Firefox/3.6.4");
                            q2 = client.DownloadString("http://findyoursupplies.com/" + bitti[0]);
                        }
                        string[] ad = q2.Split(new string[] { "<h2 class=\"headerAlpha\" style=\"margin-bottom:0\">" }, StringSplitOptions.None);
                        string[] son2 = ad[1].Split('<');
                        string[] link = q2.Split(new string[] { "<td width=\"1%\"><img src='" }, StringSplitOptions.None);
                        string[] link2 = link[1].Split('\'');

                        link2[0] = "http://findyoursupplies.com" + link2[0];
                        switch (s)
                        {
                            case "http://findyoursupplies.com/canhpshopping/?_slpcid=67":
                                excelApp.Cells[satir, sutun] = "Deskjet";
                                break;
                            case "http://findyoursupplies.com/canhpshopping/?_slpcid=103":
                                excelApp.Cells[satir, sutun] = "PhotoSmart";
                                break;
                            case "http://findyoursupplies.com/canhpshopping/?_slpcid=143":
                                excelApp.Cells[satir, sutun] = "OfficeJet";
                                break;
                            case "http://findyoursupplies.com/canhpshopping/?_slpcid=5":
                                excelApp.Cells[satir, sutun] = "LaserJet";
                                break;
                            case "http://findyoursupplies.com/canhpshopping/?_slpcid=216":
                                excelApp.Cells[satir, sutun] = "DesignJet";
                                break;
                        }
                        sutun++;
                        excelApp.Cells[satir, sutun] = adi[0];
                        sutun++;
                        excelApp.Cells[satir, sutun] = son2[0];
                        sutun++;
                        excelApp.Cells[satir, sutun] = link2[0];
                        sutun++;
                        string[] adet = q2.Split(new string[] { "<span class=\"product-title\">" }, StringSplitOptions.None);
                        int ilk = 0;
                        for(int g = 1;g<adet.Length;g++)
                        {
                            string a = adet[g];
                            if (a.Contains("Cartridge"))
                            {
                                try
                                {
                                    ilk++;
                                    string[] toner = a.Split('<');
                                    toner[0] = toner[0].Trim();
                                    
                                    string[] pic = a.Split(new string[] { "<div id=\"value_messaging_" }, StringSplitOptions.None);
                                    string[] pic2 = pic[1].Split('"');

                                    excelApp.Cells[satir, sutun] = toner[0]+" ("+pic2[0]+")";
                                    sutun++;

                                    excelApp.Cells[satir, sutun] = "http://findyoursupplies.com/media/productimages_150px/" + pic2[0] + ".jpg";
                                    sutun++;

                                    string[] col = a.Split(new string[] { "<li>" }, StringSplitOptions.None);

                                    string[] col2 = col[1].Split('<');
                                    col2[0] = col2[0].Trim();

                                    excelApp.Cells[satir, sutun] = col2[0];
                                    sutun++;

                                    string[] max3 = col[2].Split('<');
                                    max3[0] = max3[0].Trim();
                                    excelApp.Cells[satir, sutun] = max3[0];
                                    sutun++;


                                    string[] max4 = col[3].Split('<');
                                    max4[0] = max4[0].Trim();
                                    excelApp.Cells[satir, sutun] = max4[0];
                                    sutun++;
                                }
                                catch { }
                            }
                            else
                                if(ilk==0)
                                excelApp.Cells[satir, sutun] = "Toneri Yok";
                        }
                        ilk = 0;
                    }

                }
            }
            /*foreach (string s in listBox1.Items)
            {
                if (s.Contains("/?_"))
                {
                    string q2;
                    using (WebClient client = new WebClient())
                    {
                        client.Headers.Add("user-agent", "Mozilla/5.0 (Windows; Windows NT 5.1; rv:1.9.2.4) Gecko/20100611 Firefox/3.6.4");
                        q2 = client.DownloadString("http://findyoursupplies.com/" + s);
                    }
                    string[] ad = q2.Split(new string[] { "<h2 class=\"headerAlpha\" style=\"margin-bottom:0\">" }, StringSplitOptions.None);
                    string[] son = ad[1].Split('<');
                    string[] link = q2.Split(new string[] { "<td width=\"1%\"><img src='" }, StringSplitOptions.None);
                    string[] link2 = link[1].Split('\'');
                    richTextBox1.Text += "Deskjet" + son[0] + " Link: " + link2[0] + Environment.NewLine;
                }
            }*/
            this.Text = "Bitti";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string filePath = "C:/Excel-Ciktisi (son).xls";
            Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook wb = ExcelApp.Workbooks.Open(filePath, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);


            Microsoft.Office.Interop.Excel.Worksheet sh = (Microsoft.Office.Interop.Excel.Worksheet)wb.Sheets["WorkSheet1"];
            _Worksheet asd = null;
            asd = (_Worksheet)ExcelApp.ActiveWorkbook.ActiveSheet;
            for (int i = 2; i <= 1418; i++)
            {
                    string q = "AB" + i.ToString();
                    string value = asd.get_Range(q).get_Value().ToString();
                    if (value.Contains("Uyum"))
                    {
                        string top = "";
                        string[] a = value.Split(new string[] { "Uyum" }, StringSplitOptions.None);
                        for (int j = 1; j < a.Length; j++)
                        {
                            if (a[j].Contains(" serisi") || a[j].Contains(" Serisi"))
                            {
                                a[j] = a[j].Replace(" serisi", "").Replace(" Serisi","");
                                string[] b = a[j].Split(new string[] { "<td>" }, StringSplitOptions.None);
                                string[] c = b[1].Split('<');
                                string st = c[0];
                                if (st[st.Length - 1].ToString() == " ")
                                {
                                    st = st.Remove(st.Length - 1);
                                }
                                if (st[0].ToString() == " ")
                                {
                                    st = st.Remove(0);
                                }
                                int ix = st.LastIndexOf(' ');
                                int len = st.Length;
                                top += "," + st.Substring(ix, len - ix) + " Serisi";
                            }
                            else if (a[j].Contains(" mfp"))
                            {
                                a[j] = a[j].Replace(" mfp", "");
                                string[] b = a[j].Split(new string[] { "<td>" }, StringSplitOptions.None);
                                string[] c = b[1].Split('<');
                                string st = c[0];
                                if (st[st.Length - 1].ToString() == " ")
                                {
                                    st = st.Remove(st.Length - 1);
                                }
                                if (st[0].ToString() == " ")
                                {
                                    st = st.Remove(0);
                                }
                                int ix = st.LastIndexOf(' ');
                                int len = st.Length;
                                top += "," + st.Substring(ix, len - ix) + " mfp";
                            }
                            else
                            {
                                try
                                {
                                    string[] b = a[j].Split(new string[] { "<td>" }, StringSplitOptions.None);
                                    string[] c = b[1].Split('<');
                                    if (!c[0].Contains(' '))
                                    {
                                        top += "," + c[0];
                                    }
                                    else
                                    {
                                        string st = c[0];
                                        st = st.Trim();
                                        if (st[st.Length - 1].ToString() == " ")
                                        {
                                            st = st.Remove(st.Length - 1);
                                        }
                                        if (st[0].ToString() == " ")
                                        {
                                            st = st.Remove(0);
                                        }
                                        int ix = st.LastIndexOf(' ');
                                        int len = st.Length;
                                        string s = st.Substring(ix, len - ix);

                                        top += "," + st.Substring(ix, len - ix);
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }
                        names.Add(top);
                    }
                    else
                        names.Add(" ");


            }
            for (int i = 2; i <= 1418; i++)
            {
                string q = "B" + i.ToString();
                string value = asd.get_Range(q).get_Value().ToString();
                value += names[i - 2];
                names2.Add(value);
                listBox1.Items.Add(value);
            }
            wb.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string filePath = "C:/a.xls";
            Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook wb = ExcelApp.Workbooks.Open(filePath, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);


            Microsoft.Office.Interop.Excel.Worksheet sh = (Microsoft.Office.Interop.Excel.Worksheet)wb.Sheets["WorkSheet1"];
            _Worksheet asd = null;
            asd = (_Worksheet)ExcelApp.ActiveWorkbook.ActiveSheet;
            string value = asd.get_Range("AB16").get_Value().ToString();
            string[] b = value.Split(new string[] { "<td>" }, StringSplitOptions.None);
            string[] c = b[1].Split('<');
            string st = c[0];
            if (st[st.Length - 1].ToString() == " ")
            {
                st = st.Remove(st.Length - 1);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string filePath = "C:/Excel-Ciktisi (son).xls";
            Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook wb = ExcelApp.Workbooks.Open(filePath, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
            ExcelApp.Visible = true;


            int rowIndex = 2; int columnIndex = 2;
            for (int i = 2; i <= 1418; i++)
            {
                rowIndex = i;
                wb.ActiveSheet.Cells[rowIndex, columnIndex] = names2[i - 2].ToString();
            }
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            List<char> harf = new List<char>() { 'A', 'B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z' };
            int j = 0;
            foreach (char h in harf)
            {
                listBox1.Items.Add(harf[j]);
                j++;
                string q;
                using (WebClient client = new WebClient())
                {
                    client.Headers.Add("user-agent", "Mozilla/5.0 (Windows; Windows NT 5.1; rv:1.9.2.4) Gecko/20100611 Firefox/3.6.4");
                    q = client.DownloadString("https://www.patroneer.com/stores?list=a&alpha="+h+"&view=n");
                }
                string[] a = q.Split(new string[] { "<a href=\"https://www.patroneer.com/stores/" }, StringSplitOptions.None);
                for (int i = 1; i < a.Length; i++)
                {
                    string[] b = a[i].Split('"');
                    listBox1.Items.Add("https://www.patroneer.com/stores/" + b[0]);
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            for (int j = 1; j <= 1; j++)
            {
                string q;
                using (WebClient client = new WebClient())
                {
                    string link = "http://www.newhomeinturkey.com/property-in-turkey.html?page="+j.ToString();
                    this.Text = link;
                    client.Headers.Add("user-agent", "Mozilla/5.0 (Windows; Windows NT 5.1; rv:1.9.2.4) Gecko/20100611 Firefox/3.6.4");
                    q = client.DownloadString(link);
                }
                string[] a = q.Split(new string[] { "<div class=\"mainpage corner10\">" }, StringSplitOptions.None);
                string[] b = a[1].Split(new string[] { "<p><a href=\"" }, StringSplitOptions.None);
                for (int i = 1; i < b.Length; i++)
                {
                    string[] c = b[i].Split('"');
                    using (WebClient client = new WebClient())
                    {
                        string link = "http://www.newhomeinturkey.com/" + c[0];
                        this.Text = link;
                        client.Headers.Add("user-agent", "Mozilla/5.0 (Windows; Windows NT 5.1; rv:1.9.2.4) Gecko/20100611 Firefox/3.6.4");
                        q = client.DownloadString(link);
                    }
                    string[] z = q.Split(new string[] { "<h2>Development Details</h2>" }, StringSplitOptions.None);
                    string[] z1 = z[1].Split(new string[] { "</div>" }, StringSplitOptions.None);
                    string[] z2 = z1[0].Split(new string[] { "<li" }, StringSplitOptions.None);

                    string[] fi = q.Split(new string[] { "<div class=\"fyt\">" }, StringSplitOptions.None);
                    string[] fi3 = fi[1].Split('>');
                    string[] fi2 = fi3[1].Split('<');
                    fi2[0] = StripHTML(fi2[0]);
                    listBox1.Items.Add(fi2[0]);
                    


                    string[] l = q.Split(new string[] { "<div id=\"imglist\">"}, StringSplitOptions.None);
                    string[] l2 = l[1].Split(new string[]{"<script type=\"text/javascript\">"},StringSplitOptions.None);
                    string[] l3 = l2[0].Split(new string[] { "<a href=\""}, StringSplitOptions.None);
                    for (int y = 1; y < l3.Length; y++)
                    {
                        string[] ho = l3[y].Split('"');
                        listBox1.Items.Add(ho[0]);
                    }


                    for (int k = 1; k < z2.Length; k++)
                    {
                        z2[k] = z2[k].Replace(" class=\"dark\"><b>","").Replace("</b> ","").Replace("</li>","").Replace("<b>","").Replace(">","").Replace("</ul","");
                        

                        if (z2[k].Contains("images/01.png"))
                        {
                            z2[k] += "Yes";
                            z2[k] = z2[k].Replace("<img src=\"images/01.png\" alt=\"Swimming Pool\" /","");
                            z2[k] = z2[k].Replace("<img src=\"images/01.png\" alt=\"Mountain View\" /", "");
                            z2[k] = z2[k].Replace("<img src=\"images/01.png\" alt=\"Sea View\" /", "");
                            z2[k] = z2[k].Replace("<img src=\"images/01.png\" alt=\"Fitness\" /", "");
                            z2[k] = z2[k].Replace("<img src=\"images/01.png\" alt=\"Sauna\" /", "");
                            z2[k]=z2[k].Replace("\\","");
                        }
                        if (z2[k].Contains("images/00.png"))
                        {
                            z2[k] += "No";
                            z2[k] = z2[k].Replace("<img src=\"images/00.png\" alt=\"Swimming Pool\" /", "");
                            z2[k] = z2[k].Replace("<img src=\"images/00.png\" alt=\"Mountain View\" /", "");
                            z2[k] = z2[k].Replace("<img src=\"images/00.png\" alt=\"Sea View\" /", "");
                            z2[k] = z2[k].Replace("<img src=\"images/00.png\" alt=\"Fitness\" /", "");
                            z2[k] = z2[k].Replace("<img src=\"images/00.png\" alt=\"Sauna\" /", "");
                            z2[k] = z2[k].Replace("\\", "");
                        }
                        string[] son = z2[k].Split(':');
                        son[1] = son[1].Trim();
                        listBox1.Items.Add(son[0] + " " +son[1]);
                    }
                    listBox1.Items.Add("");
                    break;
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            string q,ad;
            string k1, k2, k3, k4, metrekare, odasayisi, isitma="", kullanimdurumu, binayasi, binakatsayisi, bulundugukat, aciklama,kimden,ceptel="",istel="";
            int l=1;

            Excel.Application excelApp = new Excel.Application();
            string myPath = @"C:\sahibinden.xlsx";
            excelApp.Workbooks.Open(myPath);
            excelApp.Visible = true;

            Excel.Application excelApp2 = new Excel.Application();
            string myPath2 = @"C:\resimler.xlsx";
            excelApp2.Workbooks.Open(myPath2);
            excelApp2.Visible = true;

            int satir = 1, sutun = 1, sayac = 1, rsayac = 1, rsatir=1,rsutun=1 ;
            for (int m = 0; m < listBox1.Items.Count; m++)
            {
                k1 = "";
                k2 = "";
                k3 = "";
                k4 = "";
                metrekare = "";
                odasayisi = "";
                isitma = "";
                kullanimdurumu = "";
                binayasi = "";
                binakatsayisi = "";
                bulundugukat = "";
                aciklama = "";
                kimden = "";
                ceptel = "";
                istel = "";
                sutun = 1;
                satir++;
                rsutun = 1;

                excelApp.Cells[satir, sutun] = sayac++;
                sutun++;

                

                listBox1.SetSelected(m, true);
                string link = listBox1.Items[m].ToString();
                using (WebClient client = new WebClient())
                {
                    client.Headers.Add("user-agent", "Mozilla/5.0 (Windows; Windows NT 5.1; rv:1.9.2.4) Gecko/20100611 Firefox/3.6.4");
                    client.Encoding = Encoding.UTF8;
                    q = client.DownloadString(link);
                }
                if (q.Contains("Pasif İlan") || q.Contains("classifiedExpired"))
                    continue;

                string[] ino = q.Split(new string[]{"<span class=\"adNumber\"> "},StringSplitOptions.None);
                string[] ino1 = ino[1].Split('<');

                excelApp.Cells[satir, sutun] = ino1[0];
                sutun++;

                string[] fullname = q.Split(new string[] { "<h1 id=\"classifiedTitle\">" }, StringSplitOptions.None);
                string[] fn2 = fullname[1].Split(new string[] { "</h1>" }, StringSplitOptions.None);

                excelApp.Cells[satir, sutun] = WebUtility.HtmlDecode(fn2[0]);
                sutun++;

                string[] fiyat = q.Split(new string[] { "<h3 class=\"briefPriceContent\">" }, StringSplitOptions.None);
                string[] fiyat1 = fiyat[1].Split('<');

                if (fiyat1[0].Contains("€"))
                    fiyat1[0] = fiyat1[0].Replace("€", "Euro");
                if (fiyat1[0].Contains("$"))
                    fiyat1[0] = fiyat1[0].Replace("$", "Dolar");

                excelApp.Cells[satir, sutun] = fiyat1[0];
                sutun++;




                string[] a = q.Split(new string[] { "href=\"#\" rel=\"" }, StringSplitOptions.None);
                for (int i = 1; i < a.Length; i++)
                {
                    string[] b = a[i].Split('"');
                    FileDownload(ino1[0]+"_"+l++, b[0]);
                    rsutun = 1;
                    rsatir++;
                    excelApp2.Cells[rsatir, rsutun] = rsayac++;
                    rsutun++;
                    excelApp2.Cells[rsatir, rsutun] = resim_ismi;
                }
                l = 1;


                string[] kat = q.Split(new string[] { "trackClick trackId_breadcrumb\">" }, StringSplitOptions.None);
                for (int i = 1; i < kat.Length; i++)
                {
                    string[] kat2 = kat[i].Split('<');
                    switch (i)
                    {
                        case 1:
                            excelApp.Cells[satir, sutun] = kat2[0];
                            sutun++;
                            break;
                        case 2:
                            excelApp.Cells[satir, sutun] = kat2[0];
                            sutun++;
                            break;
                        case 3:
                            excelApp.Cells[satir, sutun] = kat2[0];
                            sutun++;
                            break;
                        case 4:
                            excelApp.Cells[satir, sutun] = kat2[0];
                            sutun++;
                            break;
                    }
                }


                string[] aa = q.Split(new string[] { "<h2 class=\"briefLocation\">" }, StringSplitOptions.None);
                string[] aaa = aa[1].Split(new string[] { "\">" }, StringSplitOptions.None);
                string[] il1 = aaa[1].Split('<');
                string[] ilce1 = aaa[2].Split('<');
                string[] ilmah1 = aaa[3].Split('<');

                excelApp.Cells[satir, sutun] = il1[0];
                sutun++;
                excelApp.Cells[satir, sutun] = ilce1[0];
                sutun++;
                excelApp.Cells[satir, sutun] = ilmah1[0];
                sutun++;



                string[] it = q.Split(new string[] { "Tarihi</div><div>" }, StringSplitOptions.None);
                string[] it1 = it[1].Split('<');

                excelApp.Cells[satir, sutun] = it1[0];
                sutun++;


                string[] ki = q.Split(new string[] { "class=\"registrationFullName mhmdef\">" }, StringSplitOptions.None);
                string[] ki1 = ki[1].Split('<');

                excelApp.Cells[satir, sutun] = ki1[0];
                sutun++;


                string[] num = q.Split(new string[] { "phoneLength17\">" }, StringSplitOptions.None);
                for (int i = 1; i < num.Length; i++)
                {
                    string[] num1 = num[i].Split('<');

                    if (num1[0].Substring(0, 4) == "0 (5")
                        ceptel = num1[0];
                    else
                        istel = num1[0];
                }


                excelApp.Cells[satir, sutun] = ceptel;
                sutun++;
                excelApp.Cells[satir, sutun] = istel;
                sutun++;




                if (q.Contains("<div class=\"title\">m"))
                {
                    string[] mkare = q.Split(new string[] { "<div class=\"title\">m" }, StringSplitOptions.None);
                    string[] mkare1 = mkare[1].Split(new string[]{"</div><div> "},StringSplitOptions.None);
                    string[] mkare2 = mkare1[1].Split('<');
                    listBox1.Items.Add("MetreKare: " + mkare2[0]);
                    metrekare = mkare2[0];
                }
                else metrekare = "0";

                if (q.Contains("<div class=\"title\">Oda Sayısı</div><div> "))
                {
                    string[] os = q.Split(new string[] { "<div class=\"title\">Oda Sayısı</div><div> " }, StringSplitOptions.None);
                    string[] os1 = os[1].Split('<');
                    listBox1.Items.Add("Oda Sayısı: " + os1[0]);
                    odasayisi = os1[0];
                }
                else odasayisi = "0";

                if (q.Contains("<div class=\"title\">Isıtma</div><div> "))
                {
                    string[] isi = q.Split(new string[] { "<div class=\"title\">Isıtma</div><div> " }, StringSplitOptions.None);
                    string[] isi1 = isi[1].Split('<');
                    listBox1.Items.Add("Isıtma: " + isi1[0]);
                    isitma = isi1[0];
                }

                if (q.Contains("<div class=\"title\">Isınma Tipi</div><div> "))
                {
                    string[] isi = q.Split(new string[] { "<div class=\"title\">Isınma Tipi</div><div> " }, StringSplitOptions.None);
                    string[] isi1 = isi[1].Split('<');
                    listBox1.Items.Add("Isıtma: " + isi1[0]);
                    isitma = isi1[0];
                }

                if (q.Contains("<div class=\"title\">Kullanım Durumu</div><div> "))
                {
                    string[] kul = q.Split(new string[] { "<div class=\"title\">Kullanım Durumu</div><div> " }, StringSplitOptions.None);
                    string[] kul1 = kul[1].Split('<');
                    listBox1.Items.Add("Kullanım: " + kul1[0]);
                    kullanimdurumu = kul1[0];
                }
                else kullanimdurumu = "Bilgi Yok";

                if (q.Contains("<div class=\"title\">Kimden</div><div> "))
                {
                    string[] kul = q.Split(new string[] { "<div class=\"title\">Kimden</div><div> " }, StringSplitOptions.None);
                    string[] kul1 = kul[1].Split('<');
                    listBox1.Items.Add("Kimden: " + kul1[0]);
                    kimden = kul1[0];
                }
                else kimden = "Bilgi Yok";

                if (q.Contains("<div class=\"title\">Bina Yaşı</div><div> "))
                {
                    string[] ks = q.Split(new string[] { "<div class=\"title\">Bina Yaşı</div><div> " }, StringSplitOptions.None);
                    string[] ks1 = ks[1].Split('<');
                    listBox1.Items.Add("Bina Yaşı: " + ks1[0]);
                    binayasi = ks1[0];
                }
                else binayasi = "0";

                if (q.Contains("<div class=\"title\">Bulunduğu Kat</div><div> "))
                {
                    string[] ks = q.Split(new string[] { "<div class=\"title\">Bulunduğu Kat</div><div> " }, StringSplitOptions.None);
                    string[] ks1 = ks[1].Split('<');
                    listBox1.Items.Add("Bulunduğu Kat: " + ks1[0]);
                    bulundugukat = ks1[0];
                }
                else bulundugukat = "0";

                if (q.Contains("<div class=\"title\">Binadaki Kat Sayısı</div><div> "))
                {
                    string[] ks = q.Split(new string[] { "<div class=\"title\">Binadaki Kat Sayısı</div><div> " }, StringSplitOptions.None);
                    string[] ks1 = ks[1].Split('<');
                    listBox1.Items.Add("Binadaki Kat Sayısı: " + ks1[0]);
                    binakatsayisi = ks1[0];
                }
                else binakatsayisi = "0";

                string[] desc = q.Split(new string[] { "<div class=\"uiBoxContainer\" id=\"classifiedDescription\"><div class=\"fullDescription\">" }, StringSplitOptions.None);
                string[] desc1 = desc[1].Split(new string[] { "</div></div></div><div class=\"uiBox" }, StringSplitOptions.None);

                aciklama = WebUtility.HtmlDecode(desc1[0]);

                excelApp.Cells[satir, sutun] = kimden;
                sutun++;
                excelApp.Cells[satir, sutun] = metrekare;
                sutun++;
                excelApp.Cells[satir, sutun] = odasayisi;
                sutun++;
                excelApp.Cells[satir, sutun] = isitma;
                sutun++;
                excelApp.Cells[satir, sutun] = kullanimdurumu;
                sutun++;
                excelApp.Cells[satir, sutun] = binayasi;
                sutun++;
                excelApp.Cells[satir, sutun] = binakatsayisi;
                sutun++;
                excelApp.Cells[satir, sutun] = bulundugukat;
                sutun++;
                excelApp.Cells[satir, sutun] = aciklama;
                sutun++;

                

                

                
            }
        }
        private void al()
        {
            List<string> links = new List<string>();
            List<string> tur = new List<string>() { "gunluk-kiralik-daire", "gunluk-kiralik-residence", "gunluk-kiralik-villa", "gunluk-kiralik-kosk-konak", "gunluk-kiralik-yazlik", "gunluk-kiralik-yali-dairesi", "gunluk-kiralik-yali", "gunluk-kiralik-ciftlik-evi", "gunluk-kiralik-mustakil-ev" };
            List<string> ilce = new List<string>(){"adalar",
"arnavutkoy",
"atasehir",
"avcilar",
"bagcilar",
"bahcelievler",
"bakirkoy",
"basaksehir",
"bayrampasa",
"besiktas",
"beykoz",
"beylikduzu",
"beyoglu",
"buyukcekmece",
"catalca",
"cekmekoy",
"esenler",
"esenyurt",
"eyup",
"fatih",
"gaziosmanpasa",
"gungoren",
"kadikoy",
"kagithane",
"kartal",
"kucukcekmece",
"maltepe",
"pendik",
"sancaktepe",
"sariyer",
"sile",
"silivri",
"sisli",
"sultanbeyli",
"sultangazi",
"tuzla",
"umraniye",
"uskudar",
"zeytinburnu"};
            foreach (string t in tur)
            {
                //listBox1.Items.Add(t);
                foreach (string s in ilce)
                {
                    //listBox1.Items.Add(t+"/"+s);
                    for (int j = 0; j <= 5000; j += 50)
                    {
                        string q;
                        using (WebClient client = new WebClient())
                        {
                            string link = "http://www.sahibinden.com/" + t + "/istanbul-" + s + "?pagingSize=50&locationRequestType=StepByStep&pagingOffset=" + j;
                            client.Headers.Add("user-agent", "Mozilla/5.0 (Windows; Windows NT 5.1; rv:1.9.2.4) Gecko/20100611 Firefox/3.6.4");
                            q = client.DownloadString(link);
                        }
                        if (!q.Contains("<td class=\"searchResultsSmallThumbnail\">  <a href=\""))
                        {
                            this.Text = s + "bitti";
                            break;
                        }
                        string[] a = q.Split(new string[] { "<td class=\"searchResultsSmallThumbnail\">  <a href=\"" }, StringSplitOptions.None);
                        for (int i = 1; i < a.Length; i++)
                        {
                            string[] b = a[i].Split('"');
                            listBox1.Items.Add(b[0]);
                            links.Add(b[0]);
                        }
                    }
                }
            }
            foreach (string s in iller)
            {
                //listBox1.Items.Add(s);
                string q;
                using (WebClient client = new WebClient())
                {
                    string link = "http://www.sahibinden.com/gunluk-kiralik/" + s;
                    //listBox1.Items.Add(link);
                    client.Headers.Add("user-agent", "Mozilla/5.0 (Windows; Windows NT 5.1; rv:1.9.2.4) Gecko/20100611 Firefox/3.6.4");
                    client.Encoding = Encoding.UTF8;
                    q = client.DownloadString(link);
                }
                if (q.Contains("Arama kriterlerinize uyan ilan bulunamadı."))
                {
                    continue;
                }
                for (int j = 0; j <= 5000; j += 50)
                {
                    if (!q.Contains("<td class=\"searchResultsSmallThumbnail\">  <a href=\""))
                    {
                        this.Text = s + "bitti";
                        break;
                    }
                    using (WebClient client = new WebClient())
                    {
                        string link = "http://www.sahibinden.com/gunluk-kiralik/" + s + "?pagingSize=50&pagingOffset=" + j;
                        //listBox1.Items.Add(link);
                        client.Headers.Add("user-agent", "Mozilla/5.0 (Windows; Windows NT 5.1; rv:1.9.2.4) Gecko/20100611 Firefox/3.6.4");
                        client.Encoding = Encoding.UTF8;
                        q = client.DownloadString(link);
                    }
                    string[] a = q.Split(new string[] { "<td class=\"searchResultsSmallThumbnail\">  <a href=\"" }, StringSplitOptions.None);
                    for (int i = 1; i < a.Length; i++)
                    {
                        string[] b = a[i].Split('"');
                        listBox1.Items.Add(b[0]);
                        links.Add(b[0]);
                    }
                }
            }



            this.Text = "bitti toplam: " + listBox1.Items.Count.ToString();
            
            
            System.IO.File.WriteAllLines("sahibinden.txt", links);
            MessageBox.Show(links.Count.ToString());
        }

        private void button9_Click(object sender, EventArgs e)
        {
            /*List<string> a = new List<string>();
            for (int i = 0; i <= 8999; i++)
            {
                a.Add("a" + i + ".Start();");
            }
            System.IO.File.WriteAllLines("start.txt", a);
            string last="";
            string threads = "";
            string counters = "";
            string func;
            int tut=0;
            int fsayac = 0;
            bool b=false;
            for (int i = 100000000; i < 1000000000; i++)
            {
                if (!b)
                {
                    tut = i;
                    b = true;
                }
                if (i % 100000 == 0)
                {
                    func = "TS" + tut + "to" + i;
                    threads += "Thread a"+ fsayac++ +" = new Thread(new ThreadStart(" + func + "));\n";
                    counters += "a" + fsayac + ",";
                    string a = richTextBox2.Text;
                    a = a.Replace("basligigetir", func).Replace("hangi_sayidan",tut.ToString()).Replace("hangi_sayiya",i.ToString());
                    last += a;
                    b = false;
                }
            }
            System.IO.File.WriteAllText("son.txt", last);
            System.IO.File.WriteAllText("threads.txt", threads);
            System.IO.File.WriteAllText("counters.txt", counters);
            MessageBox.Show("bitti");*/
            /*Thread a1 = new Thread(new ThreadStart(TumSahibindenIDDene1to2));
            Thread a2 = new Thread(new ThreadStart(TumSahibindenIDDene2to3));
            Thread a3 = new Thread(new ThreadStart(TumSahibindenIDDene3to4));
            Thread a4 = new Thread(new ThreadStart(TumSahibindenIDDene4to5));
            Thread a5 = new Thread(new ThreadStart(TumSahibindenIDDene5to6));
            Thread a6 = new Thread(new ThreadStart(TumSahibindenIDDene6to7));
            Thread a7 = new Thread(new ThreadStart(TumSahibindenIDDene7to8));
            Thread a8 = new Thread(new ThreadStart(TumSahibindenIDDene8to9));
            Thread a9 = new Thread(new ThreadStart(TumSahibindenIDDene9to9));
            a1.Start();
            a2.Start();
            a3.Start();
            a4.Start();
            a5.Start();
            a6.Start();
            a7.Start();
            a8.Start();
            a9.Start();*/
            Thread te = new Thread(new ThreadStart(Tek));
            te.Start();
        }
        int ts1, ts2, ts3, ts4, ts5, ts6, ts7, ts8, ts9;
        private void Tek()
        {
            int say = 0;
            for (int i = 100000000; i <= 999999999; i++)
            {
                string q;
                using (WebClient client = new WebClient())
                {
                    string link = "http://www.sahibinden.com/search.php?b%5Bsearch_text%5D=" + i;
                    //listBox1.Items.Add(link);
                    client.Headers.Add("user-agent", "Mozilla/5.0 (Windows; Windows NT 5.1; rv:1.9.2.4) Gecko/20100611 Firefox/3.6.4");
                    client.Encoding = Encoding.UTF8;
                    q = client.DownloadString(link);
                }
                this.Text = say++.ToString();
                if (!q.Contains("<meta name=\"og:url\" content=\"") || !q.Contains("<h2 class=\"briefLocation\">") || q.Contains("Arama kriterlerinize uyan ilan bulunamadı.") || q.Contains("Pasif İlan") || q.Contains("classifiedExpired") || q.Contains("İlan giriş tarihiniz 30 günden eski görünüyor!") || !q.Contains("<a href=\"#\" class=\"blankLayoutLink\" title=\""))
                    continue;
                string[] ne = q.Split(new string[] { "<a href=\"#\" class=\"blankLayoutLink\" title=\"" }, StringSplitOptions.None);
                string[] ne2 = ne[1].Split('"');

                string[] nerde = q.Split(new string[] { "<meta name=\"og:url\" content=\"" }, StringSplitOptions.None);
                string[] nerde2 = nerde[1].Split('"');

                string[] aa = q.Split(new string[] { "<h2 class=\"briefLocation\">" }, StringSplitOptions.None);
                string[] aaa = aa[1].Split(new string[] { "\">" }, StringSplitOptions.None);
                string[] il1 = aaa[1].Split('<');
                string[] ilce1 = aaa[2].Split('<');
                string[] ilmah1 = aaa[3].Split('<');

                string son = WebUtility.HtmlDecode(ne2[0] + " --- " + nerde2[0] + " --- " + il1[0] + " / " + ilce1[0] + " / " + ilmah1[0]);
                listBox1.Items.Add(son);

            }
        }
        private void TumSahibindenIDDene1to2()
        {
            for (int i = 100000000; i <= 200000000; i++)
            {

                string q;
                using (WebClient client = new WebClient())
                {
                    string link = "http://www.sahibinden.com/search.php?b%5Bsearch_text%5D=" + i;
                    //listBox1.Items.Add(link);
                    client.Headers.Add("user-agent", "Mozilla/5.0 (Windows; Windows NT 5.1; rv:1.9.2.4) Gecko/20100611 Firefox/3.6.4");
                    client.Encoding = Encoding.UTF8;
                    q = client.DownloadString(link);
                }
                ts1++;
                if (q.Contains("Arama kriterlerinize uyan ilan bulunamadı.") || q.Contains("Pasif İlan") || q.Contains("classifiedExpired") || q.Contains("İlan giriş tarihiniz 30 günden eski görünüyor!"))
                    continue;
                string[] ne = q.Split(new string[] { "<a href=\"#\" class=\"blankLayoutLink\" title=\"" }, StringSplitOptions.None);
                string[] ne2 = ne[1].Split('"');

                string[] nerde = q.Split(new string[] { "<meta name=\"og:url\" content=\"" }, StringSplitOptions.None);
                string[] nerde2 = nerde[1].Split('"');

                string[] aa = q.Split(new string[] { "<h2 class=\"briefLocation\">" }, StringSplitOptions.None);
                string[] aaa = aa[1].Split(new string[] { "\">" }, StringSplitOptions.None);
                string[] il1 = aaa[1].Split('<');
                string[] ilce1 = aaa[2].Split('<');
                string[] ilmah1 = aaa[3].Split('<');

                string son = WebUtility.HtmlDecode(ne2[0] + " --- " + nerde2[0] + " --- " + il1[0] + " / " + ilce1[0] + " / " + ilmah1[0]);
                listBox1.Items.Add(son);

            }
        }
        private void TumSahibindenIDDene2to3()
        {
            for (int i = 200000001; i <= 300000000; i++)
            {

                
                string q;
                using (WebClient client = new WebClient())
                {
                    string link = "http://www.sahibinden.com/search.php?b%5Bsearch_text%5D=" + i;
                    //listBox1.Items.Add(link);
                    client.Headers.Add("user-agent", "Mozilla/5.0 (Windows; Windows NT 5.1; rv:1.9.2.4) Gecko/20100611 Firefox/3.6.4");
                    client.Encoding = Encoding.UTF8;
                    q = client.DownloadString(link);
                }
                ts2++;
                if (q.Contains("Arama kriterlerinize uyan ilan bulunamadı.") || q.Contains("Pasif İlan") || q.Contains("classifiedExpired") || q.Contains("İlan giriş tarihiniz 30 günden eski görünüyor!"))
                    continue;
                string[] ne = q.Split(new string[] { "<a href=\"#\" class=\"blankLayoutLink\" title=\"" }, StringSplitOptions.None);
                string[] ne2 = ne[1].Split('"');

                string[] nerde = q.Split(new string[] { "<meta name=\"og:url\" content=\"" }, StringSplitOptions.None);
                string[] nerde2 = nerde[1].Split('"');

                string[] aa = q.Split(new string[] { "<h2 class=\"briefLocation\">" }, StringSplitOptions.None);
                string[] aaa = aa[1].Split(new string[] { "\">" }, StringSplitOptions.None);
                string[] il1 = aaa[1].Split('<');
                string[] ilce1 = aaa[2].Split('<');
                string[] ilmah1 = aaa[3].Split('<');

                string son = WebUtility.HtmlDecode(ne2[0] + " --- " + nerde2[0] + " --- " + il1[0] + " / " + ilce1[0] + " / " + ilmah1[0]);
                listBox1.Items.Add(son);

            }
        }
        private void TumSahibindenIDDene3to4()
        {
            for (int i = 300000001; i <= 400000000; i++)
            {

                
                string q;
                using (WebClient client = new WebClient())
                {
                    string link = "http://www.sahibinden.com/search.php?b%5Bsearch_text%5D=" + i;
                    //listBox1.Items.Add(link);
                    client.Headers.Add("user-agent", "Mozilla/5.0 (Windows; Windows NT 5.1; rv:1.9.2.4) Gecko/20100611 Firefox/3.6.4");
                    client.Encoding = Encoding.UTF8;
                    q = client.DownloadString(link);
                }
                ts3++;
                if (q.Contains("Arama kriterlerinize uyan ilan bulunamadı.") || q.Contains("Pasif İlan") || q.Contains("classifiedExpired") || q.Contains("İlan giriş tarihiniz 30 günden eski görünüyor!"))
                    continue;
                string[] ne = q.Split(new string[] { "<a href=\"#\" class=\"blankLayoutLink\" title=\"" }, StringSplitOptions.None);
                string[] ne2 = ne[1].Split('"');

                string[] nerde = q.Split(new string[] { "<meta name=\"og:url\" content=\"" }, StringSplitOptions.None);
                string[] nerde2 = nerde[1].Split('"');

                string[] aa = q.Split(new string[] { "<h2 class=\"briefLocation\">" }, StringSplitOptions.None);
                string[] aaa = aa[1].Split(new string[] { "\">" }, StringSplitOptions.None);
                string[] il1 = aaa[1].Split('<');
                string[] ilce1 = aaa[2].Split('<');
                string[] ilmah1 = aaa[3].Split('<');

                string son = WebUtility.HtmlDecode(ne2[0] + " --- " + nerde2[0] + " --- " + il1[0] + " / " + ilce1[0] + " / " + ilmah1[0]);
                listBox1.Items.Add(son);

            }
        }
        private void TumSahibindenIDDene4to5()
        {
            for (int i = 400000001; i <= 500000000; i++)
            {

                
                string q;
                using (WebClient client = new WebClient())
                {
                    string link = "http://www.sahibinden.com/search.php?b%5Bsearch_text%5D=" + i;
                    //listBox1.Items.Add(link);
                    client.Headers.Add("user-agent", "Mozilla/5.0 (Windows; Windows NT 5.1; rv:1.9.2.4) Gecko/20100611 Firefox/3.6.4");
                    client.Encoding = Encoding.UTF8;
                    q = client.DownloadString(link);
                }
                ts4++;
                if (q.Contains("Arama kriterlerinize uyan ilan bulunamadı.") || q.Contains("Pasif İlan") || q.Contains("classifiedExpired") || q.Contains("İlan giriş tarihiniz 30 günden eski görünüyor!"))
                    continue;
                string[] ne = q.Split(new string[] { "<a href=\"#\" class=\"blankLayoutLink\" title=\"" }, StringSplitOptions.None);
                string[] ne2 = ne[1].Split('"');

                string[] nerde = q.Split(new string[] { "<meta name=\"og:url\" content=\"" }, StringSplitOptions.None);
                string[] nerde2 = nerde[1].Split('"');

                string[] aa = q.Split(new string[] { "<h2 class=\"briefLocation\">" }, StringSplitOptions.None);
                string[] aaa = aa[1].Split(new string[] { "\">" }, StringSplitOptions.None);
                string[] il1 = aaa[1].Split('<');
                string[] ilce1 = aaa[2].Split('<');
                string[] ilmah1 = aaa[3].Split('<');

                string son = WebUtility.HtmlDecode(ne2[0] + " --- " + nerde2[0] + " --- " + il1[0] + " / " + ilce1[0] + " / " + ilmah1[0]);
                listBox1.Items.Add(son);

            }
        }
        private void TumSahibindenIDDene5to6()
        {
            for (int i = 500000001; i <= 600000000; i++)
            {

                
                string q;
                using (WebClient client = new WebClient())
                {
                    string link = "http://www.sahibinden.com/search.php?b%5Bsearch_text%5D=" + i;
                    //listBox1.Items.Add(link);
                    client.Headers.Add("user-agent", "Mozilla/5.0 (Windows; Windows NT 5.1; rv:1.9.2.4) Gecko/20100611 Firefox/3.6.4");
                    client.Encoding = Encoding.UTF8;
                    q = client.DownloadString(link);
                }
                ts5++;
                if (q.Contains("Arama kriterlerinize uyan ilan bulunamadı.") || q.Contains("Pasif İlan") || q.Contains("classifiedExpired") || q.Contains("İlan giriş tarihiniz 30 günden eski görünüyor!"))
                    continue;
                string[] ne = q.Split(new string[] { "<a href=\"#\" class=\"blankLayoutLink\" title=\"" }, StringSplitOptions.None);
                string[] ne2 = ne[1].Split('"');

                string[] nerde = q.Split(new string[] { "<meta name=\"og:url\" content=\"" }, StringSplitOptions.None);
                string[] nerde2 = nerde[1].Split('"');

                string[] aa = q.Split(new string[] { "<h2 class=\"briefLocation\">" }, StringSplitOptions.None);
                string[] aaa = aa[1].Split(new string[] { "\">" }, StringSplitOptions.None);
                string[] il1 = aaa[1].Split('<');
                string[] ilce1 = aaa[2].Split('<');
                string[] ilmah1 = aaa[3].Split('<');

                string son = WebUtility.HtmlDecode(ne2[0] + " --- " + nerde2[0] + " --- " + il1[0] + " / " + ilce1[0] + " / " + ilmah1[0]);
                listBox1.Items.Add(son);

            }
        }
        private void TumSahibindenIDDene6to7()
        {
            for (int i = 600000001; i <= 700000000; i++)
            {

                
                string q;
                using (WebClient client = new WebClient())
                {
                    string link = "http://www.sahibinden.com/search.php?b%5Bsearch_text%5D=" + i;
                    //listBox1.Items.Add(link);
                    client.Headers.Add("user-agent", "Mozilla/5.0 (Windows; Windows NT 5.1; rv:1.9.2.4) Gecko/20100611 Firefox/3.6.4");
                    client.Encoding = Encoding.UTF8;
                    q = client.DownloadString(link);
                }
                ts6++;
                if (q.Contains("Arama kriterlerinize uyan ilan bulunamadı.") || q.Contains("Pasif İlan") || q.Contains("classifiedExpired") || q.Contains("İlan giriş tarihiniz 30 günden eski görünüyor!"))
                    continue;
                string[] ne = q.Split(new string[] { "<a href=\"#\" class=\"blankLayoutLink\" title=\"" }, StringSplitOptions.None);
                string[] ne2 = ne[1].Split('"');

                string[] nerde = q.Split(new string[] { "<meta name=\"og:url\" content=\"" }, StringSplitOptions.None);
                string[] nerde2 = nerde[1].Split('"');

                string[] aa = q.Split(new string[] { "<h2 class=\"briefLocation\">" }, StringSplitOptions.None);
                string[] aaa = aa[1].Split(new string[] { "\">" }, StringSplitOptions.None);
                string[] il1 = aaa[1].Split('<');
                string[] ilce1 = aaa[2].Split('<');
                string[] ilmah1 = aaa[3].Split('<');

                string son = WebUtility.HtmlDecode(ne2[0] + " --- " + nerde2[0] + " --- " + il1[0] + " / " + ilce1[0] + " / " + ilmah1[0]);
                listBox1.Items.Add(son);

            }
        }
        private void TumSahibindenIDDene7to8()
        {
            for (int i = 700000001; i <= 800000000; i++)
            {

                
                string q;
                using (WebClient client = new WebClient())
                {
                    string link = "http://www.sahibinden.com/search.php?b%5Bsearch_text%5D=" + i;
                    //listBox1.Items.Add(link);
                    client.Headers.Add("user-agent", "Mozilla/5.0 (Windows; Windows NT 5.1; rv:1.9.2.4) Gecko/20100611 Firefox/3.6.4");
                    client.Encoding = Encoding.UTF8;
                    q = client.DownloadString(link);
                }
                ts7++;
                if (q.Contains("Arama kriterlerinize uyan ilan bulunamadı.") || q.Contains("Pasif İlan") || q.Contains("classifiedExpired") || q.Contains("İlan giriş tarihiniz 30 günden eski görünüyor!"))
                    continue;
                string[] ne = q.Split(new string[] { "<a href=\"#\" class=\"blankLayoutLink\" title=\"" }, StringSplitOptions.None);
                string[] ne2 = ne[1].Split('"');

                string[] nerde = q.Split(new string[] { "<meta name=\"og:url\" content=\"" }, StringSplitOptions.None);
                string[] nerde2 = nerde[1].Split('"');

                string[] aa = q.Split(new string[] { "<h2 class=\"briefLocation\">" }, StringSplitOptions.None);
                string[] aaa = aa[1].Split(new string[] { "\">" }, StringSplitOptions.None);
                string[] il1 = aaa[1].Split('<');
                string[] ilce1 = aaa[2].Split('<');
                string[] ilmah1 = aaa[3].Split('<');

                string son = WebUtility.HtmlDecode(ne2[0] + " --- " + nerde2[0] + " --- " + il1[0] + " / " + ilce1[0] + " / " + ilmah1[0]);
                listBox1.Items.Add(son);

            }
        }
        private void TumSahibindenIDDene8to9()
        {
            for (int i = 800000001; i <= 900000000; i++)
            {

                
                string q;
                using (WebClient client = new WebClient())
                {
                    string link = "http://www.sahibinden.com/search.php?b%5Bsearch_text%5D=" + i;
                    //listBox1.Items.Add(link);
                    client.Headers.Add("user-agent", "Mozilla/5.0 (Windows; Windows NT 5.1; rv:1.9.2.4) Gecko/20100611 Firefox/3.6.4");
                    client.Encoding = Encoding.UTF8;
                    q = client.DownloadString(link);
                }
                ts8++;
                if (q.Contains("Arama kriterlerinize uyan ilan bulunamadı.") || q.Contains("Pasif İlan") || q.Contains("classifiedExpired") || q.Contains("İlan giriş tarihiniz 30 günden eski görünüyor!"))
                    continue;
                string[] ne = q.Split(new string[] { "<a href=\"#\" class=\"blankLayoutLink\" title=\"" }, StringSplitOptions.None);
                string[] ne2 = ne[1].Split('"');

                string[] nerde = q.Split(new string[] { "<meta name=\"og:url\" content=\"" }, StringSplitOptions.None);
                string[] nerde2 = nerde[1].Split('"');

                string[] aa = q.Split(new string[] { "<h2 class=\"briefLocation\">" }, StringSplitOptions.None);
                string[] aaa = aa[1].Split(new string[] { "\">" }, StringSplitOptions.None);
                string[] il1 = aaa[1].Split('<');
                string[] ilce1 = aaa[2].Split('<');
                string[] ilmah1 = aaa[3].Split('<');

                string son = WebUtility.HtmlDecode(ne2[0] + " --- " + nerde2[0] + " --- " + il1[0] + " / " + ilce1[0] + " / " + ilmah1[0]);
                listBox1.Items.Add(son);

            }
        }
        private void TumSahibindenIDDene9to9()
        {
            for (int i = 900000001; i <= 999999999; i++)
            {

                
                string q;
                using (WebClient client = new WebClient())
                {
                    string link = "http://www.sahibinden.com/search.php?b%5Bsearch_text%5D=" + i;
                    //listBox1.Items.Add(link);
                    client.Headers.Add("user-agent", "Mozilla/5.0 (Windows; Windows NT 5.1; rv:1.9.2.4) Gecko/20100611 Firefox/3.6.4");
                    client.Encoding = Encoding.UTF8;
                    q = client.DownloadString(link);
                }
                ts9++;
                if (q.Contains("Arama kriterlerinize uyan ilan bulunamadı.") || q.Contains("Pasif İlan") || q.Contains("classifiedExpired") || q.Contains("İlan giriş tarihiniz 30 günden eski görünüyor!"))
                    continue;
                string[] ne = q.Split(new string[] { "<a href=\"#\" class=\"blankLayoutLink\" title=\"" }, StringSplitOptions.None);
                string[] ne2 = ne[1].Split('"');

                string[] nerde = q.Split(new string[] { "<meta name=\"og:url\" content=\"" }, StringSplitOptions.None);
                string[] nerde2 = nerde[1].Split('"');

                string[] aa = q.Split(new string[] { "<h2 class=\"briefLocation\">" }, StringSplitOptions.None);
                string[] aaa = aa[1].Split(new string[] { "\">" }, StringSplitOptions.None);
                string[] il1 = aaa[1].Split('<');
                string[] ilce1 = aaa[2].Split('<');
                string[] ilmah1 = aaa[3].Split('<');

                string son = WebUtility.HtmlDecode(ne2[0] + " --- " + nerde2[0] + " --- " + il1[0] + " / " + ilce1[0] + " / " + ilmah1[0]);
                listBox1.Items.Add(son);

            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            richTextBox1.Text = "1to2: " + ts1 + "\n2to3: " + ts2 + "\n3to4: " + ts3 + "\n4to5: " + ts4 + "\n5to6: " + ts5 + "\n6to7: " + ts6 + "\n7to8: " + ts7 + "\n8to9: " + ts8 + "\n9to9: " + ts9;
        }

        private void ilİlçeAdetBulToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread a1 = new Thread(new ThreadStart(TumSahibindenIDDene1to2));
            Thread a2 = new Thread(new ThreadStart(TumSahibindenIDDene2to3));
            Thread a3 = new Thread(new ThreadStart(TumSahibindenIDDene3to4));
            Thread a4 = new Thread(new ThreadStart(TumSahibindenIDDene4to5));
            Thread a5 = new Thread(new ThreadStart(TumSahibindenIDDene5to6));
            Thread a6 = new Thread(new ThreadStart(TumSahibindenIDDene6to7));
            Thread a7 = new Thread(new ThreadStart(TumSahibindenIDDene7to8));
            Thread a8 = new Thread(new ThreadStart(TumSahibindenIDDene8to9));
            Thread a9 = new Thread(new ThreadStart(TumSahibindenIDDene9to9));
            a1.Start();
            a2.Start();
            a3.Start();
            a4.Start();
            a5.Start();
            a6.Start();
            a7.Start();
            a8.Start();
            a9.Start();
            /*string q;
            using (WebClient client = new WebClient())
            {
                string link = "http://www.sahibinden.com/satilik-daire/istanbul-avcilar-ambarli?address_quarter=22816&locationRequestType=StepByStep";
                client.Headers.Add("user-agent", "Mozilla/5.0 (Windows; Windows NT 5.1; rv:1.9.2.4) Gecko/20100611 Firefox/3.6.4");
                client.Encoding = Encoding.UTF8;
                q = client.DownloadString(link);
            }
            string[] a = q.Split(new string[] { "id=\"address_district\" multiple=\"multiple\" name=\"address_quarter\">" }, StringSplitOptions.None);
            string[] b = a[1].Split(new string[]{"</optgroup> </select>"},StringSplitOptions.None);
            string[] c = b[0].Split(new string[] {"\">" }, StringSplitOptions.None);
            for (int i = 1; i < c.Length; i++)
            {
                string[] d = c[i].Split(' ');
                if(d[0]!="")
                    listBox1.Items.Add(d[0]);
            }*/
        }

        private void deneİlçeYazToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void emlakjetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread a = new Thread(new ThreadStart(emlakjet));
            a.Start();
        }
        private void emlakjet()
        {
            List<string> l = new List<string>();
            string q = "";
            using (WebClient client = new WebClient())
            {

                client.Headers.Add("user-agent", "Mozilla/5.0 (Windows; Windows NT 5.1; rv:1.9.2.4) Gecko/20100611 Firefox/3.6.4");
                client.Encoding = Encoding.UTF8;
                q = client.DownloadString("http://www.emlakjet.com/kategori/satilik/konut/daire/istanbul/index.html");
            }
            string[] link = q.Split(new string[] { "width=\"2\" height=\"5\" class=\"padding_12\" /> <a href=\"" }, StringSplitOptions.None);
            for (int i = 1; i < link.Length; i++)
            {
                string[] a = link[i].Split('"');
                a[0] = "http://www.emlakjet.com" + a[0];
                l.Add(a[0]);
            }
            foreach (string s in l)
            {
                using (WebClient client = new WebClient())
                {

                    client.Headers.Add("user-agent", "Mozilla/5.0 (Windows; Windows NT 5.1; rv:1.9.2.4) Gecko/20100611 Firefox/3.6.4");
                    client.Encoding = Encoding.UTF8;
                    q = client.DownloadString(s);
                }
                string[] cc = q.Split(new string[] { "<td style=\"width:136px;\" class=\"top padding_02 lineheight_01 font_11 padding_03\" nowrap=\"nowrap\">" }, StringSplitOptions.None);
                string[] cc2 = cc[1].Split(new string[] { "</tr>" }, StringSplitOptions.None);
                string[] cc1 = cc2[0].Split(new string[] { "\">" }, StringSplitOptions.None);
                for (int j = 1; j < cc1.Length; j++)
                {
                    string[] a = cc1[j].Split('<');
                    listBox1.Items.Add(a[0]);
                }
                
            }
        }
    }

}
