using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestLab_v2
{
    public partial class Folders : Form
    {
        public Folders()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() != DialogResult.Cancel)
            {

                textBox1.Text = dlg.SelectedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() != DialogResult.Cancel)
            {
                textBox2.Text = dlg.SelectedPath;
            }
        }

        private void Folders_Load(object sender, EventArgs e)
        {
            if (File.Exists("paths.ini"))
            {
                using (StreamReader sr = new StreamReader("paths.ini"))
                {
                    sr.ReadLine();
                    textBox1.Text = sr.ReadLine();
                    sr.ReadLine();
                    textBox2.Text = sr.ReadLine();
                }
            }
        }

        private void Folders_FormClosing(object sender, FormClosingEventArgs e)
        {
            using (var sw = new StreamWriter("paths.ini"))
            {
                sw.WriteLine("Путь к папке с тестами:");
                sw.WriteLine(textBox1.Text);
                sw.WriteLine("Путь к папке с ответами:");
                sw.WriteLine(textBox2.Text);
            }
        }
    }
}
