using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace net2._0
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int bos = 0;
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Title = "Bölünecek Txt Dosyasını Seçin";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string a = System.IO.File.ReadAllText(ofd.FileName);
                    string[] b = a.Split(new string[] { "__________" }, StringSplitOptions.None);
                    foreach (string s in b)
                    {
                        if (!s.Equals(""))
                        {
                            string adi = "";
                            if (s.Contains("[b]"))
                            {
                                string[] aa = s.Split(new string[] { "[b]" }, StringSplitOptions.None);
                                string[] aa1 = aa[1].Split(new string[] { "[/b]" }, StringSplitOptions.None);
                                aa1[0] = aa1[0].Trim();
                                aa1[0] = aa1[0].Replace(":", "").Replace("\\", "").Replace("/", "").Replace("*", "").Replace("?", "").Replace("\"", "").Replace("|", "").Replace("<", "").Replace(">", "");
                                if (aa1[0].Equals(""))
                                    aa1[0] = "bos" + bos++;
                                adi = aa1[0];
                            }
                            else
                                adi = "bos" + bos++;

                            string pth = Path.GetDirectoryName(ofd.FileName) + "\\" + adi + ".txt";
                            System.IO.File.WriteAllText(pth, s.TrimStart());
                        }
                    }
                    MessageBox.Show("Bitti");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Bitti");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog a = new FolderBrowserDialog();
            a.ShowDialog();
            MessageBox.Show(a.SelectedPath);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
        }
    }
}
