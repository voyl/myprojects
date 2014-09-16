using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace son_deneme
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        List<int> year = new List<int>() {2012,2013};
        List<int> byear = new List<int>() { 1980,1981,1982,1983,1984,1985,1986,1987,1988,1989,1990,1991,1992,1993,1994,1995,1996,1997,1998,1999,2000 };
        private string arindir(string s)
        {
            return s.Replace("ı", "i").Replace("ğ","g").Replace("ü","u").Replace("ş","s").Replace("ö","o").Replace("ç","c").Replace(" ","-").Replace("(","");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //webBrowser1.Document.GetElementById("TextBox1").InnerText = "burnu büyüklük";
            //webBrowser1.Document.GetElementById("Button1").InvokeMember("click");
            /*List<string> sn = new List<string>();
            OpenFileDialog a = new OpenFileDialog();
            if (a.ShowDialog() == DialogResult.OK)
            {
                string[] q = System.IO.File.ReadAllLines(a.FileName);
                foreach (string s in q)
                    if (!s.Equals(""))
                    {
                        string[] ss = s.Split(':');
                        sn.Add(ss[0]);
                    }
                System.IO.File.WriteAllLines(a.FileName, sn.ToArray());
            }*/
            webBrowser1.Document.GetElementById("locationLink").InnerText = "İzmir";
            webBrowser1.Document.GetElementById("locationLink").InvokeMember("onclick");
        }
    }
}
