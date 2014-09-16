using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace link_change
{
    public partial class Form1 : Form
    {
        string sourceName = "";
        string linkName = "";

        string linkSourceName = "";
        string linkLinkName = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            foreach (string s in System.IO.File.ReadAllLines(linkName))
                listBox1.Items.Add(s);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string[] a;
                List<string> last = new List<string>();
                List<string> lastFull = new List<string>();
                a = System.IO.File.ReadAllLines(onlyLink);
                string[] lnk = System.IO.File.ReadAllLines(onlyLinks);
                foreach (string s in a)
                {
                    if (s.Contains("http://fileparadox.in"))
                    {
                        string[] getN = s.Split(new string[] { textBox6.Text }, StringSplitOptions.None);
                        string[] getN2 = getN[getN.Length - 1].Split('.');

                        if (s.Contains("[b][url="))
                        {
                            string[] p = s.Split(new[] { "http://" }, StringSplitOptions.None);
                            string[] p1 = p[1].Split(']');

                            foreach (string link in lnk)
                            {
                                string[] getL = link.Split(new string[] { textBox5.Text }, StringSplitOptions.None);
                                string[] getL2 = getL[getL.Length - 1].Split('.');
                                if (getL2[0].Equals(getN2[0]))
                                {
                                    if (checkBox6.Checked)
                                    {
                                        lastFull.Add(s);
                                        last.Add(p[0] + link + "]" + p1[1] + "]");
                                        lastFull.Add(textBox7.Text);
                                        lastFull.Add(p[0] + link + "]" + p1[1] + "]");
                                    }
                                    else
                                        last.Add(p[0] + link + "]" + p1[1] + "]");
                                    break;
                                }
                            }
                        }
                        else
                        {
                            foreach (string link in lnk)
                            {
                                string[] getL = link.Split(new string[] { textBox5.Text }, StringSplitOptions.None);
                                string[] getL2 = getL[getL.Length - 1].Split('.');
                                if (getL2[0].Equals(getN2[0]))
                                {
                                    if (checkBox6.Checked)
                                    {
                                        lastFull.Add(s);
                                        last.Add(link);
                                        lastFull.Add(textBox7.Text);
                                        lastFull.Add(link);
                                    }
                                    else
                                        last.Add(link);
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        last.Add(s);
                        lastFull.Add(s);
                    }
                }
                System.IO.File.WriteAllLines("sonuc.txt", last.ToArray());
                if (checkBox6.Checked)
                    System.IO.File.WriteAllLines("sonuc-full.txt", lastFull.ToArray());
            }
            catch { MessageBox.Show("Err!"); }
            MessageBox.Show("Tamamlandı.");
            
        }
        string onlyLink = "";
        string onlyLinks = "";
        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            if (file.ShowDialog() == DialogResult.OK)
            {
                label7.Text = System.IO.Path.GetFileName(file.FileName);
                onlyLink = file.FileName;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //try
            //{
                string latest = "";
                richTextBox1.Clear();
                string l = System.IO.File.ReadAllText(sourceName);
                List<string> last = new List<string>();
                string[] l2 = l.Split(new string[] { "////////////////////" }, StringSplitOptions.None);
                for (int i = 1; i < l2.Length; i++)
                {
                    string[] kalan= new string[5];
                    bool b = false;
                    bool footer = false;
                    if (l2[i].Contains(".zip.html") || l2[i].Contains(".rar.html") || l2[i].Contains(".rar") || l2[i].Contains(".zip"))
                    {
                        string[] last_part = l2[i].Split(new string[] { "[url]" }, StringSplitOptions.None);
                        string[] bol = last_part[1].Split('-');
                        string[] bol2 = bol[1].Split('.');
                        if (l2[i].Contains("[url="))
                        {
                            footer = true;
                            kalan = l2[i].Split(new string[] { "[url=" }, StringSplitOptions.None);
                        }
                        
                        bool multi = false;
                        foreach (string lnk in listBox1.Items)
                        {
                            string[] lnk2 = lnk.Split('-');
                            string[] lnk3 = lnk2[1].Split('.');
                            if (lnk3[0].Equals(bol2[0]))
                            {
                                if (!multi)
                                {
                                    last_part[1] = ("[url]" + lnk + "[/url][/b]");
                                    b = true;
                                    //break;
                                }
                                else
                                {
                                    last_part[1] += (Environment.NewLine+"[url]" + lnk + "[/url][/b]");
                                    b = true;
                                    //break;
                                }
                                multi = true;
                            }
                        }
                        string harici = "";
                        for(int k = 1;k<kalan.Length;k++)
                        {
                            harici +="[url="+kalan[k];
                        }
                        if (b)
                        {
                            if(checkBox2.Checked)
                                if(footer)
                                    latest += "////////////////////" + last_part[0] + textBox2.Text + last_part[1] + Environment.NewLine + Environment.NewLine + harici;
                                else
                                    latest += "////////////////////" + last_part[0] + textBox2.Text + last_part[1];
                            else
                                if(footer)
                                    latest += "////////////////////" + last_part[0] + last_part[1] + Environment.NewLine+Environment.NewLine+ harici;
                                else
                                    latest += "////////////////////" + last_part[0] + last_part[1];
                        }
                    }
                }
                if (checkBox1.Checked)
                {
                    bool next = false;
                    if(System.IO.File.Exists("tmp.txt"))
                        System.IO.File.Delete("tmp.txt");
                    System.IO.File.WriteAllText("tmp.txt", latest);
                    string[] linepart = System.IO.File.ReadAllLines("tmp.txt");
                    List<string> soncikti = new List<string>();
                    foreach (string lp in linepart)
                    {
                        bool b = false;
                        if (next)
                        {
                            b = true;
                            soncikti.Add(textBox1.Text);
                            next = false;
                        }
                        if (lp.Contains("/////////////////"))
                        {
                            next = true;
                            b = true;
                            soncikti.Add(lp);
                        }
                        if (!b)
                            soncikti.Add(lp);
                    }
                    System.IO.File.Delete("tmp.txt");
                    System.IO.File.WriteAllLines("sonuc-"+String.Format("{0:dd-MM-yyyy-hh-mm-ss}", DateTime.Now)+".txt", soncikti.ToArray());
                }
                else
                    System.IO.File.WriteAllText("sonuc-" + String.Format("{0:dd-MM-yyyy-hh-mm-ss}", DateTime.Now) + ".txt", latest);
                MessageBox.Show("Sonuc Dosyası Oluşturuldu.");
            /*}
            catch (Exception ex)
            {
                MessageBox.Show("Hata Oluştu.");
            }*/
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            if (file.ShowDialog() == DialogResult.OK)
            {
                label2.Text = System.IO.Path.GetFileName(file.FileName);
                sourceName = file.FileName;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            if (file.ShowDialog() == DialogResult.OK)
            {
                label3.Text = System.IO.Path.GetFileName(file.FileName);
                linkName = file.FileName;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            if (file.ShowDialog() == DialogResult.OK)
            {
                label6.Text = System.IO.Path.GetFileName(file.FileName);
                linkSourceName = file.FileName;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> latest = new List<string>();
                string l = System.IO.File.ReadAllText(linkSourceName);
                string[] l2 = l.Split(new string[] { "////////////////////" }, StringSplitOptions.None);

                /*
                
                int st = 1;
                string[] satirr = l2[1].Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                foreach (string ss in satirr)
                {
                    richTextBox1.Text += st.ToString() + " -> " + ss + Environment.NewLine;
                    st++;
                }
                return;

                */



                for (int i = 1; i < l2.Length; i++)
                {
                    bool f = false;
                    string getLink = "";
                    //string[] satir = l2[i].Split(new[] { '\r', '\n' });
                    string[] satir = l2[i].Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                    foreach (string ss in satir)
                    {
                        if (ss.Contains("[/b]"))
                        {
                            break;
                        }
                        getLink = ss;
                    }
                    foreach (string ss in satir)
                    {
                        if (ss.Contains(".zip"))
                        {
                            if (checkBox4.Checked)
                                latest.Add(textBox4.Text);
                            latest.Add(getLink + ".zip");
                        }
                        else if (ss.Contains(".rar"))
                        {
                            if (checkBox4.Checked)
                                latest.Add(textBox4.Text);
                            latest.Add(getLink + ".rar");
                        }
                        else
                            if (!f)
                            {
                                latest.Add("////////////////////"+ss);
                                if (checkBox3.Checked)
                                    latest.Add(textBox3.Text);
                                f = true;
                            }
                            else
                                latest.Add(ss);
                    }
                }
                System.IO.File.WriteAllLines("sonuc-" + String.Format("{0:dd-MM-yyyy-hh-mm-ss}", DateTime.Now) + ".txt", latest.ToArray());
                MessageBox.Show("Sonuç Dosyanız Oluşturuldu.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata Oluştu.");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            if (file.ShowDialog() == DialogResult.OK)
            {
                label9.Text = System.IO.Path.GetFileName(file.FileName);
                onlyLinks = file.FileName;
            }
        }
    }
}
