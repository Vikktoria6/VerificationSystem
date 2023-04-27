using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TestLab_v2
{
    public partial class Form1 : Form
    {
        private string folderTest = "";
        private string folderAns = "";
        static string[] msg = {
            "OK!",  //0
            "FAIL! Wrong answer", //1
            "FAIL! Time Limit",   //2
            "FAIL! Time Limit, process not killed!", //3
            "FAIL! File not generated", //4
            "FAIL! Presentation error", //5
            "No checker :("             //6
        };
        void ShowMsg(string msg, bool err)
        {
            var len = richTextBox1.TextLength;
            richTextBox1.AppendText(msg);
            richTextBox1.SelectionStart = len;
            richTextBox1.SelectionLength = msg.Length;          
            if (err)
                richTextBox1.SelectionColor = Color.Red;
            else
                richTextBox1.SelectionColor = Color.Green;
        }
        void ShowMsg(string msg)
        {
            richTextBox1.AppendText(msg);
        }
        public Form1()
        {
            InitializeComponent();
            comboBox2.SelectedIndex = 0;
            if (File.Exists("paths.ini"))
            {
                using (StreamReader sr = new StreamReader("paths.ini"))
                {
                    sr.ReadLine();
                    folderTest = sr.ReadLine();
                    sr.ReadLine();
                    folderAns = sr.ReadLine();
                }
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.AddExtension = true;
            dlg.DefaultExt = "exe";
            dlg.CheckFileExists = true;
            dlg.Filter = "Исполняемые файлы |*.exe";
            if (dlg.ShowDialog() != DialogResult.Cancel)
            {
                textBox3.Text = dlg.FileName;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            if (!File.Exists(textBox3.Text))
            {
                MessageBox.Show("Тестируемый файл не найден");
                return;
            }    
            if (comboBox1.SelectedIndex + 1 == 0)
            {
                MessageBox.Show("Выберите вариант");
                return;
            }
            int taskNumber = comboBox1.SelectedIndex + 1;
            string testDir = Path.Combine(folderTest, taskNumber.ToString());
            if (!Directory.Exists(testDir))
            {
                MessageBox.Show("Папка с тестами не найдена");
                return;
            }
            string ansDir = Path.Combine(folderAns, taskNumber.ToString());
            if (!Directory.Exists(ansDir))
            {
                MessageBox.Show("Папка с ответами не найдена");
                return;
            }

            int testCount = 0, numberTest = 0;
            if (comboBox2.SelectedIndex == 0) testCount = new DirectoryInfo(testDir).GetFiles().Length;
            else
            {
                numberTest = comboBox2.SelectedIndex;
                testCount = 1;
            }

            //    if (taskNumber <= 14)
            //{
            var chk = new Checker(taskNumber, testCount, numberTest, textBox3.Text, testDir, ansDir);
            chk.Show += new ShowMsg(ShowMsg);
            ShowMsg("Run " + testCount.ToString() + " tests \n");
            Specifications specific = new Specifications();
            var ans = chk.Check(specific);
            ShowMsg("\n");
            for (int testNum = 0; testNum < testCount; testNum++)
            {
                ShowMsg("Test " + (testNum + 1).ToString() + ": ");
                ShowMsg(msg[ans[testNum]], ans[testNum] > 0);
                ShowMsg("\n");
            }
            ShowMsg(specific.MsgError());

            //}
            //else
            //{
            //    var lnch = new Launcher(testCount, textBox3.Text, testDir);
            //    lnch.Show += new ShowMsg(ShowMsg);
            //    lnch.Check();
            //    ShowMsg("\n");
            //    ShowMsg("Files have been generated for manual cheking!");
            //}
        }

        private void создатьТестToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateTest CTest = new CreateTest();
            CTest.Show();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            comboBox2.Items.Add("Все тесты");
            comboBox2.SelectedIndex = 0;
            int taskNumber = comboBox1.SelectedIndex + 1;
            string dir = Path.Combine(folderTest, taskNumber.ToString());
            if (Directory.Exists(dir))
            {
                int countTests = new DirectoryInfo(dir).GetFiles().Length;
                for (int i = 1; i <= countTests; i++)
                {
                    comboBox2.Items.Add(i.ToString());
                }
            }
        }

        private void путьКПапкеСОтветамиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Folders folders = new Folders();
            if (folders.ShowDialog() == DialogResult.Cancel)
            {
                folderTest = folders.textBox1.Text;
                folderAns = folders.textBox2.Text;
            }
        }
    }
}
