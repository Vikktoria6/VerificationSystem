using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestLab_v2
{
    public partial class CreateTest : Form
    {
        private int n;
        private int[][] matrix;
        private string[] tempMatrix;
        public CreateTest()
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

        private void CreateTest_Load(object sender, EventArgs e)
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

        private bool validate(int lev)
        {

            if (SizeMatrix.Text.Length == 0) 
            {
                MessageBox.Show("Введите размер матрицы");
                return false;
            }
            if (comboBox1.SelectedIndex + 1 == 0 && lev > 0)
            {
                MessageBox.Show("Введите вариант");
                return false;
            } 
            if (checkedListBox1.CheckedItems.Count == 0 || checkedListBox1.CheckedItems.Count == 2)
            {
                MessageBox.Show("Укажите разделитель");
                return false;
            }
            if (richTextBox1.Text.Length == 0)
            {
                MessageBox.Show("Введите матрицу");
                return false;
            }
            if (richTextBox2.Text.Length == 0 && lev > 0)
            {
                MessageBox.Show("Введите ответ");
                return false;
            }

            if ((textBox1.Text.Length == 0 || textBox2.Text.Length == 0) && lev > 0)
            {
                MessageBox.Show("Введите путь до папки с тестами или ответами");
                return false;
            }
            if (lev > 0)
            {
                if (checkedListBox2.CheckedIndices.Contains(0) && checkedListBox2.CheckedIndices.Contains(1)
                    || checkedListBox2.CheckedIndices.Contains(2) && checkedListBox2.CheckedIndices.Contains(3)
                    || checkedListBox2.CheckedIndices.Contains(4) && checkedListBox2.CheckedIndices.Contains(5)
                    || checkedListBox2.CheckedIndices.Contains(6) && checkedListBox2.CheckedIndices.Contains(7)
                    || checkedListBox2.CheckedIndices.Contains(8) && checkedListBox2.CheckedIndices.Contains(9))
                {
                    MessageBox.Show("Характеристики противоречат друг другу");
                    return false;
                }
            }
            return true;
        }

        private void fill_matrix()
        {
            n = Convert.ToInt32(SizeMatrix.Text);
            matrix = new int[n][]; 
            for (int i = 0; i < n; i ++)
                matrix[i] = new int[n];
            char[] s;
            if (checkedListBox1.CheckedIndices.Contains(0)) s = new char[] { ' ' , '\n' };
            else s = new char[] { ',', '\n'};
            tempMatrix = richTextBox1.Text.Split(s, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < tempMatrix.Length; i++)
            {
                matrix[i / n][i % n] = Convert.ToInt32(tempMatrix[i]);
            }
        }

        private bool permission(int v)
        {
            switch (v)
            {
                case 1:
                    if (checkedListBox2.CheckedIndices.Contains(10) 
                        || checkedListBox2.CheckedIndices.Contains(11))
                        return false;
                    break;
                case 2:
                    if (checkedListBox2.CheckedIndices.Contains(7)
                        || checkedListBox2.CheckedIndices.Contains(10)
                        || checkedListBox2.CheckedIndices.Contains(11))
                        return false;
                    break;
                case 3:
                    if (checkedListBox2.CheckedIndices.Contains(7)
                        || checkedListBox2.CheckedIndices.Contains(10)
                        || checkedListBox2.CheckedIndices.Contains(11))
                        return false;
                    break;
                case 4:
                    if (checkedListBox2.CheckedIndices.Contains(7)
                        || checkedListBox2.CheckedIndices.Contains(10)
                        || checkedListBox2.CheckedIndices.Contains(11))
                        return false;
                    break;
                case 5:
                    if (checkedListBox2.CheckedIndices.Contains(5)
                        || checkedListBox2.CheckedIndices.Contains(7)
                        || checkedListBox2.CheckedIndices.Contains(9)
                        || checkedListBox2.CheckedIndices.Contains(10)
                        || checkedListBox2.CheckedIndices.Contains(11))
                        return false;
                    break;
                case 6:
                    if (checkedListBox2.CheckedIndices.Contains(0)
                        || !checkedListBox2.CheckedIndices.Contains(1)
                        || checkedListBox2.CheckedIndices.Contains(2)
                        || checkedListBox2.CheckedIndices.Contains(5)
                        || checkedListBox2.CheckedIndices.Contains(11))
                        return false;
                    break;
                case 7:
                    if (checkedListBox2.CheckedIndices.Contains(5)
                        || checkedListBox2.CheckedIndices.Contains(7)
                        || checkedListBox2.CheckedIndices.Contains(11))
                        return false;
                    break;
                case 8:
                    if (checkedListBox2.CheckedIndices.Contains(5)
                        || checkedListBox2.CheckedIndices.Contains(7)
                        || checkedListBox2.CheckedIndices.Contains(11))
                        return false;
                    break;
                case 9:
                    if (checkedListBox2.CheckedIndices.Contains(5)
                        || checkedListBox2.CheckedIndices.Contains(11))
                        return false;
                    break;
                case 10:
                    if (checkedListBox2.CheckedIndices.Contains(5))
                        return false;
                    break;
                case 11:
                    if (checkedListBox2.CheckedIndices.Contains(7)
                        || checkedListBox2.CheckedIndices.Contains(10)
                        || checkedListBox2.CheckedIndices.Contains(11))
                        return false;
                    break;
                case 12:
                    if (checkedListBox2.CheckedIndices.Contains(5))
                        return false;
                    break;
                case 13:
                    if (checkedListBox2.CheckedIndices.Contains(7)
                        || checkedListBox2.CheckedIndices.Contains(10)
                        || checkedListBox2.CheckedIndices.Contains(11))
                        return false;
                    break;
            }
            return true;
        }

        void countTests()
        {
            int taskNumber = comboBox1.SelectedIndex + 1;
            string dir = Path.Combine(textBox1.Text, taskNumber.ToString());
            if (Directory.Exists(dir))
            {
                int countTests = new DirectoryInfo(dir).GetFiles().Length;
                label9.Text = "Кол-во созданных тестов: " + countTests;
            }
            else label9.Text = "Кол-во созданных тестов: 0";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            countTests();
        }

        private void AddNewTest_Click(object sender, EventArgs e)
        {
            if (validate(1) && permission(comboBox1.SelectedIndex + 1))
            {
                fill_matrix();
                string testFolder = textBox1.Text + "\\" + (comboBox1.SelectedIndex + 1).ToString();
                string ansFolder = textBox2.Text + "\\" + (comboBox1.SelectedIndex + 1).ToString();
                if (!Directory.Exists(testFolder)) Directory.CreateDirectory(testFolder);
                if (!Directory.Exists(ansFolder)) Directory.CreateDirectory(ansFolder);
                int ind = new DirectoryInfo(testFolder).GetFiles().Length + 1;
                
                FileStream f_test = File.Create(testFolder + "\\test" + ind.ToString() + ".txt");
                FileStream f_ans = File.Create(ansFolder + "\\" + ind.ToString()+ "_output"  + ".txt");

                using (StreamWriter fwrite = new StreamWriter(f_test))
                {
                    fwrite.WriteLine(n);
                    
                    for (int i = 0; i < tempMatrix.Length; i++)
                    {
                        matrix[i / n][i % n] = Convert.ToInt32(tempMatrix[i]);
                        if (i != 0 && i % n == 0) fwrite.WriteLine();
                        if (i % n == n - 1) fwrite.Write(tempMatrix[i]);
                        else fwrite.Write("{0} ", tempMatrix[i]);
                    }
                    fwrite.WriteLine();
                    for (int i = 0; i < Specifications.CountSpecific; i++)
                    {
                        if (checkedListBox2.CheckedIndices.Contains(i)) fwrite.WriteLine(1);
                        else fwrite.WriteLine(0);
                    }
                  
                }
                using (StreamWriter fwr = new StreamWriter(f_ans)) {
                    char[] s;
                    if (checkedListBox1.CheckedItems.Contains(0)) s = new char[] { ' ' };
                    else s = new char[] { ',' };
                    var ans_string = richTextBox2.Text.Split(s, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < ans_string.Length; i++)
                    {
                        fwr.Write(ans_string[i]);
                    }
                }
                MessageBox.Show("Тест создан");
                countTests();
            }
            else if (!permission(comboBox1.SelectedIndex + 1))
            {
                MessageBox.Show("Тест не соответствует условиям задания");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SizeMatrix.Text = "";
            foreach (int elem in checkedListBox1.CheckedIndices)
                checkedListBox1.SetItemChecked(elem, false);
            foreach (int elem in checkedListBox2.CheckedIndices)
                checkedListBox2.SetItemChecked(elem, false);
            comboBox1.SelectedIndex = -1;
            richTextBox1.Text = "";
            richTextBox2.Text = "";
        }

        private bool NoEdge()
        {
            if (n == 1) return false;
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    if (matrix[i][j] != 0) return false;
            return true;
        }

        private bool Complete()
        {
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    if (i != j && matrix[i][j] == 0) return false;
            return true;
        }

        private void dfs(int u, bool[] visited, int[][] mtr)
        {
            visited[u] = true;
            for (int i = 0; i < n; i++)
            {
                if (mtr[u][i] != 0)
                    if (!visited[i]) dfs(i, visited, mtr);
            }
        }

        private bool Connected()
        {
            bool[] visited = new bool[n];
            int[][] mtr = new int[n][];
            for (int i = 0; i < n; i++)
                mtr[i] = new int[n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    mtr[i][j] = matrix[i][j];
                    if (mtr[i][j] != 0) mtr[j][i] = mtr[i][j];
                }
            dfs(0, visited, mtr);
            foreach (bool v in visited)
                if (!v) return false;
            return true;
        }

        private bool Oriented()
        {
            // if (n == 1) ??????
            for (int i = 0; i < n; i++)
                for (int j = i; j < n; j++)
                    if (i != j && matrix[i][j] != matrix[j][i] && matrix[i][j] != 0) return true;
            return false;
        }

        private bool HasCycle(int v, bool[] visited, int parent)
        {
            visited[v] = true;
            for (int i = 0; i < n; i++)
            {
                if (matrix[v][i] != 0 && v != i)
                    if (!visited[i])
                    {
                        if (HasCycle(i, visited, v))
                        {
                            return true;
                        }
                    }
                    else if (i != parent)
                    {
                        return true;
                    }
            }
            return false;
        }

        private bool Cycles()
        {
                                                              // if (n == 1) ??????
            bool []visited = new bool[n];
            if (!HasCycle(0, visited, -1))
            {
                return false;
            }
            else return true;
        }

        private bool Weighted()
        {
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    if (matrix[i][j] != 1 && matrix[i][j] != 0) return true;
            return false;
        }

        private bool Negative()
        {
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    if (matrix[i][j] < 0) return true;
            return false;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (validate(0))
            {
                fill_matrix();
                checkedListBox2.SetItemChecked(0, NoEdge());
                checkedListBox2.SetItemChecked(1, Complete());
                checkedListBox2.SetItemChecked(2, n == 1);
                checkedListBox2.SetItemChecked(3, n > 15);
                bool connect = Connected();
                checkedListBox2.SetItemChecked(4, connect);
                checkedListBox2.SetItemChecked(5, !connect);
                bool orientation = Oriented();
                checkedListBox2.SetItemChecked(6, !orientation);
                checkedListBox2.SetItemChecked(7, orientation);
                bool cycle = Cycles();
                checkedListBox2.SetItemChecked(8, connect && !cycle);
                checkedListBox2.SetItemChecked(9, cycle);
                bool weight = Weighted();
                checkedListBox2.SetItemChecked(10, weight);
                checkedListBox2.SetItemChecked(11, Negative());
            }
        }

        private void generate_Click(object sender, EventArgs e)
        {
            int end_rand, start_rand;
            Random rnd = new Random();
            if (SizeMatrix.Text != "")
            {
                n = Convert.ToInt32(SizeMatrix.Text);
                matrix = new int[n][];
                for (int i = 0; i < n; i++)
                    matrix[i] = new int[n];
            }
            else
            {
                MessageBox.Show("Введите размер матрицы");
                return;
            }
            if ((checkedListBox3.CheckedIndices.Contains(0) && checkedListBox3.CheckedIndices.Contains(1)) ||
                    (checkedListBox3.CheckedIndices.Contains(2) && checkedListBox3.CheckedIndices.Contains(3)) ||
                    (checkedListBox3.CheckedIndices.Contains(1) && checkedListBox3.CheckedIndices.Contains(3)) ||
                    (!checkedListBox3.CheckedIndices.Contains(4) && checkedListBox3.CheckedIndices.Contains(5)))
                    {
                        MessageBox.Show("Характеристики противоречат друг другу");
                        return;
                    }
            if (checkedListBox3.CheckedIndices.Contains(0))  //без ребер
            {
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                        matrix[i][j] = 0;
            }
            else
            {
                if (checkedListBox3.CheckedIndices.Contains(4)) end_rand = 10;  //взвешенный
                else end_rand = 2;
                if (checkedListBox3.CheckedIndices.Contains(1)) start_rand = 1; //полный 
                else start_rand = 0;

                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                    {
                        matrix[i][j] = rnd.Next(start_rand, end_rand);
                        if (!checkedListBox3.CheckedIndices.Contains(1)) matrix[i][j] = Convert.ToInt32(matrix[i][j] * Math.Pow((0), rnd.Next(0, 2)));
            }

                if (checkedListBox3.CheckedIndices.Contains(5))  //отриц. веса
                {
                    for (int i = 0; i < n; i++)
                        for (int j = 0; j < n; j++)
                            matrix[j][i] = Convert.ToInt32(matrix[j][i] * Math.Pow((-1), rnd.Next(0, 10)));
                }

                if (checkedListBox3.CheckedIndices.Contains(2) || checkedListBox3.CheckedIndices.Contains(1))     //неориентированный
                {
                    for (int i = 0; i < n - 1; i++)
                        for (int j = i + 1; j < n; j++) matrix[j][i] = matrix[i][j];
                }
                else if (checkedListBox3.CheckedIndices.Contains(3))   //ориентированный 
                {
                    for (int i = 0; i < n - 1; i++)
                        for (int j = i + 1; j < n; j++)
                            if (matrix[i][j] != 0)
                                matrix[j][i] = 0;
                }
                
                for (int i = 0; i < n; i++) matrix[i][i] = 0;   //главная диагональ 0

            }
            richTextBox1.Text = "";
            richTextBox2.Text = "";
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    richTextBox1.Text = richTextBox1.Text + matrix[i][j].ToString() + " ";
                richTextBox1.Text = richTextBox1.Text + "\n";
            }
        }

    }
}
