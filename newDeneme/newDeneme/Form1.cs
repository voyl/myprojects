using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.IO;
using mshtml;

namespace newDeneme
{
    public partial class Form1 : Form
    {
        List<string> keys = new List<string>();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //htagsafhasru
            //yjsbaxkwr
            //gvaasacassqe
            //gsgqtzxghmuryryri
            //o
            //oo
            webBrowser1.Document.GetElementById("email").InnerText = listBox2.GetItemText(listBox2.SelectedItem) + Convert.ToInt32(numericUpDown1.Value) + "@bspamfree.org";
            webBrowser1.Document.GetElementById("password").InnerText = "12345678";
            webBrowser1.Document.GetElementById("submit_archeage").InvokeMember("click");

        }
        int sec = 5;
        private void Wait()
        {
            timer1.Enabled = true;
            while (sec > 0)
                Application.DoEvents();
            sec = 5;
            timer1.Enabled = false;
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            //http://www.razerzone.com/gamebooster-archeage-beta/
            while(true)
            {
                button1.PerformClick();
                Wait();
                try
                {
                    string a = webBrowser1.Document.GetElementById("sso_return").InnerText;
                    string[] b = a.Split(new string[] { "Code:" }, StringSplitOptions.None);
                    string c = b[1].Trim();
                    listBox1.Items.Add(numericUpDown1.Value + " -> " + c);
                    File.AppendAllText("keys.txt", c + Environment.NewLine);
                }
                catch { break; }
                webBrowser1.Refresh();
                numericUpDown1.Value += 1;
                Wait();
            }
        }
        private bool isConfirmed()
        {
            string src = "";
            using (WebClient wb = new WebClient())
            {
                src = wb.DownloadString("http://bspamfree.org/emails.php?EmailAccount=yjsbaxkwr" + Convert.ToInt32(numericUpDown1.Value) + "&submit=Go%21");
            }
            if (src.Contains("Thank you for signing up for Razer ID."))
            {
                string[] a = src.Split(new string[] { "https://" }, StringSplitOptions.None);
                string[] b = a[1].Split(' ');
                using (WebClient wb = new WebClient())
                {
                    src = wb.DownloadString("https://"+b[0]);
                }
                return true;
            }
            else
                return false;
        }
        private bool isConfirmed2()
        {
            string src = "";
            using (WebClient wb = new WebClient())
            {
                src = wb.DownloadString("https://api.mailinator.com/api/inbox?to=oo" + Convert.ToInt32(numericUpDown1.Value) + "&token=8410b1e50c4948eb9bd8ac34ee91fb1e");
            }
            if (src.Contains("\"subject\":\"EMAIL VERIFICATION\",\"fromfull\":\"donotreply@razerzone.com\""))
            {
                string[] a = src.Split(new string[] { "\"id\":\"" }, StringSplitOptions.None);
                string[] b = a[1].Split('"');
                using (WebClient wb = new WebClient())
                {
                    src = wb.DownloadString("https://api.mailinator.com/api/email?msgid="+b[0]+"&token=8410b1e50c4948eb9bd8ac34ee91fb1e");
                }
                string[] c = src.Split(new string[] { "verify?u=" }, StringSplitOptions.None);
                string[] d = c[1].Split('\\');
                using (WebClient wb = new WebClient())
                {
                    src = wb.DownloadString("https://ec.razerzone.com/verify?u=" + d[0]);
                }
                return true;
            }
            else
                return false;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            sec--;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://www.hidemyass.com/proxy");
            MessageBox.Show("go");
            do
            {
                Application.DoEvents();
            }
            while (webBrowser1.ReadyState != WebBrowserReadyState.Complete);
            webBrowser1.Document.GetElementById("hmaurl").InnerText = "https://eu.alienwarearena.com/account/register";
            foreach (HtmlElement el in webBrowser1.Document.GetElementsByTagName("input"))
            {
                if (el.GetAttribute("value").Equals("Hide My Ass"))
                {
                    el.InvokeMember("click");
                    break;
                }
            }
            Wait();
            do
            {
                Application.DoEvents();
            }
            while (webBrowser1.ReadyState != WebBrowserReadyState.Complete);


            webBrowser1.Document.GetElementById("fos_user_registration_form_username").InnerText = "voop"+numericUpDown1.Value;
            webBrowser1.Document.GetElementById("fos_user_registration_form_email").InnerText = "gsgqtzxghmuryryri" + numericUpDown1.Value + "@bspamfree.org";
            webBrowser1.Document.GetElementById("fos_user_registration_form_plainPassword").InnerText = "12345678Deneme";
            webBrowser1.Document.GetElementById("fos_user_registration_form_firstname").InnerText = "hamdi";
            webBrowser1.Document.GetElementById("fos_user_registration_form_lastname").InnerText = "camis";
            webBrowser1.Document.GetElementById("fos_user_registration_form_birthdate_day").SetAttribute("value", "1");
            webBrowser1.Document.GetElementById("fos_user_registration_form_birthdate_month").SetAttribute("value", "1");
            webBrowser1.Document.GetElementById("fos_user_registration_form_birthdate_year").SetAttribute("value", "1991");
            webBrowser1.Document.GetElementById("recaptcha_response_field").InnerText = textBox1.Text;
            webBrowser1.Document.GetElementById("fos_user_registration_form_termsAccepted").InvokeMember("click");
            foreach (HtmlElement el in webBrowser1.Document.GetElementsByTagName("button"))
            {
                if (el.InnerHtml.Equals("Register"))
                {
                    el.InvokeMember("click");
                    break;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (isConfirmed())
            {
                do
                {
                    Application.DoEvents();
                }
                while (webBrowser1.ReadyState != WebBrowserReadyState.Complete);
                webBrowser1.Navigate("https://eu.alienwarearena.com/login");
                Wait();
                webBrowser1.Document.GetElementById("_username").InnerText = "voop" + numericUpDown1.Value;
                webBrowser1.Document.GetElementById("_password").InnerText = "12345678Deneme";
                webBrowser1.Document.GetElementById("_login").InvokeMember("click");
                Wait();
                webBrowser1.Navigate("http://eu.alienwarearena.com/giveaways/226/archeage-closed-beta-event-2-setting-sail-key-giveaway/key");
                Wait();
                webBrowser1.Navigate("http://eu.alienwarearena.com/logout");
                Wait();
                webBrowser1.Navigate("https://eu.alienwarearena.com/account/register");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            for (int i = 1; i < 5; i++)
            {
                string src = "";
                using (WebClient wb = new WebClient())
                {
                    src = wb.DownloadString("http://bspamfree.org/emails.php?EmailAccount=gvaasacassqe" + i.ToString() + "&submit=Go%21");
                }
                if (src.Contains("tigen: <a href=\""))
                {
                    string[] a = src.Split(new string[]{"tigen: <a href=\""},StringSplitOptions.None);
                    string[] b = a[1].Split('"');
                    string src1 = "";
                    MessageBox.Show(b[0]);
                    using (WebClient wb = new WebClient())
                    {
                        src1 = wb.DownloadString(b[0]);
                    }

                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //http://signup.leagueoflegends.com/?ref=54024733d199f548369810
            
            /*string[] a = webBrowser1.DocumentText.Split(new string[] { "<img src=\"" }, StringSplitOptions.None);
            string[] b = a[2].Split('"');
            pictureBox1.ImageLocation = "https://signup.leagueoflegends.com" + b[0];

            HtmlElementCollection imgCollection = webBrowser1.Document.GetElementsByTagName("img");
            foreach (HtmlElement h in imgCollection)
            {
                listBox1.Items.Add(h);
            }*/
            Random a = new Random();
            int i = 0;
            int li = 0;
            string ln = "";
            string lp = "";
            while (true)
            {
                i = 0;
                ln = "";
                lp = "";
                while (true)
                {
                    if (i < 8)
                    {
                        int c = a.Next(97, 122);
                        ln += ((char)c).ToString();
                    }
                    else
                    {
                        int b = a.Next(0, 9);
                        ln += b.ToString();
                    }
                    if (i == 10)
                        break;
                    i++;
                }
                i = 0;
                while (true)
                {
                    if (i < 8)
                    {
                        int c = a.Next(97, 122);
                        lp += ((char)c).ToString();
                    }
                    else
                    {
                        int b = a.Next(0, 9);
                        lp += b.ToString();
                    }
                    if (i == 10)
                        break;
                    i++;
                }
                if (li == 100)
                    break;
                li++;
                listBox1.Items.Add(ln + "|" + lp);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            int i = 0;
            foreach (HtmlElement el in webBrowser1.Document.GetElementsByTagName("input"))
            {
                if (el.GetAttribute("name").Equals("data[PvpnetAccount][realm]"))
                {
                    if (i == 1)
                    {
                        el.InvokeMember("click");
                        break;
                    }
                    else
                        i++;
                }
            }

            string a = listBox1.Items[Convert.ToInt32(numericUpDown1.Value)].ToString();
            string[] b = a.Split('|');
            webBrowser1.Document.GetElementById("PvpnetAccountName").InnerText = b[0];
            webBrowser1.Document.GetElementById("PvpnetAccountPassword").InnerText = b[1];
            webBrowser1.Document.GetElementById("PvpnetAccountConfirmPassword").InnerText = b[1];
            webBrowser1.Document.GetElementById("PvpnetAccountEmailAddress").InnerText = b[0]+"@mailinator.com";
            webBrowser1.Document.GetElementById("PvpnetAccountDateOfBirthDay").InnerText = "10";
            webBrowser1.Document.GetElementById("PvpnetAccountDateOfBirthMonth").InnerText = "10";
            webBrowser1.Document.GetElementById("PvpnetAccountDateOfBirthYear").InnerText = "1991";
            webBrowser1.Document.GetElementById("PvpnetAccountTouAgree").InvokeMember("click");
            webBrowser1.Document.GetElementById("PvpnetAccountNewsletter").InvokeMember("click");
            webBrowser1.Document.GetElementById("PvpnetAccountCaptcha").InnerText = textBox2.Text;
            webBrowser1.Document.GetElementById("AccountSubmit").InvokeMember("click");
            File.AppendAllText("accs.txt", a+ Environment.NewLine);
            Wait();
            webBrowser1.Navigate("http://signup.leagueoflegends.com/?ref=54024733d199f548369810");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            webBrowser1.Document.GetElementById("raffle_email").InnerText = "gvaasacassqe" + numericUpDown1.Value + "@bspamfree.org";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (isConfirmed())
            {
                button1.PerformClick();
                Wait();
                string a = webBrowser1.Document.GetElementById("sso_return").InnerText;
                string[] b = a.Split(new string[] { "Code:" }, StringSplitOptions.None);
                string c = b[1].Trim();
                listBox1.Items.Add(numericUpDown1.Value + " -> " + c);
                File.AppendAllText("keys.txt", c + Environment.NewLine);
                webBrowser1.Refresh();
                Wait();
                numericUpDown1.Value += 1;
                button2.PerformClick();
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            IHTMLDocument2 doc = (IHTMLDocument2)webBrowser1.Document.DomDocument;
            IHTMLControlRange imgRange = (IHTMLControlRange)((HTMLBody)doc.body).createControlRange();
            int i = 0;
            foreach (IHTMLImgElement img in doc.images)
            {
                if (i == 1)
                {
                    imgRange.add((IHTMLControlElement)img);

                    imgRange.execCommand("Copy", false, null);
                    Image img2 = Clipboard.GetImage();
                    if (File.Exists(Application.StartupPath + "\\image.jpg"))
                    {
                        File.Delete(Application.StartupPath + "\\image.jpg");
                    }
                    img2.Save(Application.StartupPath + "\\image.jpg");
                    pictureBox2.Load(Application.StartupPath + "\\image.jpg");
                    break;
                }
                    i++;

            }
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                IHTMLDocument2 doc = (IHTMLDocument2)webBrowser1.Document.DomDocument;
                IHTMLControlRange imgRange = (IHTMLControlRange)((HTMLBody)doc.body).createControlRange();
                int i = 0;
                foreach (IHTMLImgElement img in doc.images)
                {
                    if (i == 1)
                    {
                        imgRange.add((IHTMLControlElement)img);

                        imgRange.execCommand("Copy", false, null);
                        Image img2 = Clipboard.GetImage();
                        if (File.Exists(Application.StartupPath + "\\image.jpg"))
                        {
                            File.Delete(Application.StartupPath + "\\image.jpg");
                        }
                        img2.Save(Application.StartupPath + "\\image.jpg");
                        pictureBox2.Load(Application.StartupPath + "\\image.jpg");
                        break;
                    }
                    i++;

                }
            }
            catch { }
        }
    }
}
