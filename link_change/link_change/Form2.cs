using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace link_change
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        string sourceName = "";
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            if (file.ShowDialog() == DialogResult.OK)
            {
                label2.Text = System.IO.Path.GetFileName(file.FileName);
                sourceName = file.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string a = System.IO.File.ReadAllText(sourceName);
            string[] b = a.Split(new[] { "////////////////////" }, StringSplitOptions.None);
            for(int i = 1;i<b.Length;i++)
            {
                for(int j = 1;j<b.Length;j++)
                {

                }
                string[] c = b[i].Split(new[] { "[b]Size:[/b] " }, StringSplitOptions.None);
                string[] c1 = c[1].Split(new[] {Environment.NewLine},StringSplitOptions.None);
                MessageBox.Show(c1[0].Equals("4,48 MB").ToString();
            }
        }
    }
}
