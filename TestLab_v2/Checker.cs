using System;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace TestLab_v2
{
    delegate void ShowMsg(string msg);
    class Checker
    {
        int v, n;
        int[,] m;
        int testsCount;
        int numberTest;
        string testingSolution;
        string testsFolder;
        string ansFolder;
        public event ShowMsg Show;

        void Write(string msg)
        {
            if (Show != null)
                Show(msg);
        }
        public Checker(int variant, int testsCount, int numberTest, string testingSolution, string testsFolder, string ansFolder)
        {
            v = variant;
            this.testsCount = testsCount;
            this.testingSolution = testingSolution;
            this.testsFolder = testsFolder;
            this.ansFolder = ansFolder;
            this.numberTest = numberTest;
        }
        public int[] Check(Specifications specific)
        {
            Process solver = new Process();
            var ans = new int[testsCount];
 
            solver.StartInfo.FileName = testingSolution;    //программа запуска
            solver.StartInfo.Arguments = "";            // аргументы 
            solver.StartInfo.UseShellExecute = false;
            solver.StartInfo.CreateNoWindow = true;
            string tmpPath = Directory.GetCurrentDirectory();

            for (int i = 0; i < testsCount; i++)
            {

                //Write("\nTest " + (i+1).ToString() + ": ");
                //FileInfo f = new FileInfo(testsFolder + "\\" + (i+1).ToString() + "_input.txt");
                FileInfo f, res, rightAns;
                if (numberTest == 0)
                {
                    f = new FileInfo(testsFolder + "\\Test" + (i + 1).ToString() + ".txt");
                    rightAns = new FileInfo(ansFolder + "\\" + (i + 1).ToString() + "_output.txt");
                }
                else
                {
                    f = new FileInfo(testsFolder + "\\Test" + numberTest + ".txt");
                    rightAns = new FileInfo(ansFolder + "\\" + numberTest + "_output.txt");
                }
                f.CopyTo(tmpPath + "\\input.txt", true);
                res = new FileInfo(tmpPath + "\\output.txt");
                solver.Start();
                int seconds = 0;
                StreamReader source = new StreamReader(f.FullName);
                List<int> spec = new List<int>(Specifications.CountSpecific);

                n = Int32.Parse(source.ReadLine());
                m = new int[n, n];
                for (int k = 0; k < n; k++)
                {
                    var tmp = source.ReadLine().Split(' ');
                    for (int j = 0; j < n; j++)
                    {
                        m[k, j] = Int32.Parse(tmp[j]);
                    }
                }
                List<string> str = source.ReadToEnd().Split('\n').ToList<string>();
                str.RemoveAll(x => x == "" || x == "\r" || x == "\n");
                spec = str.Select(e => Convert.ToInt32(e)).ToList();
                specific.SumSpec(spec);
                do
                {
                    if (!solver.HasExited)
                    {
                        solver.Refresh();
                        //Write(".");
                        seconds++;
                    }
                } while (!solver.WaitForExit(1000) && seconds < 10);

                if (!solver.HasExited)
                {
                    try
                    {
                        solver.Kill();
                    }
                    catch
                    {
                        ans[i] = 3;
                        specific.FindError(spec, ans[i] == 0);
                        
                        continue;
                    }
                    ans[i] = 2;
                    specific.FindError(spec, ans[i] == 0);
                    continue;
                }
                seconds = 0;
                while (!res.Exists && seconds < 10)
                {
                    //Console.Write(".");
                    Thread.Sleep(1000);
                    seconds++;
                }
                if (!res.Exists)
                {
                    ans[i] = 4;
                    specific.FindError(spec, ans[i] == 0);
                  
                    continue;
                }
                
                StreamReader solution = new StreamReader(res.FullName);
                StreamReader etalon = new StreamReader(rightAns.FullName);
                

                switch (v)
                {
                    
                    case 1:
                        ans[i] = Euler(solution, etalon);
                        specific.FindError(spec, ans[i] == 0);
                        
                        break;
                    case 2:
                        ans[i] = GraphSearch(source, solution, etalon);
                        break;
                    case 3:
                        ans[i] = CompCount(solution, etalon);
                        specific.FindError(spec, ans[i] == 0);
                        break;
                    case 4:
                        ans[i] = Bipart(source, solution, etalon);
                        break;
                    case 5:
                        ans[i] = TreeCRD(source, solution, etalon);
                        break;
                    case 6:
                        ans[i] = Salesman(source, solution, etalon);
                        break;
                    case 7:
                        ans[i] = MST(source, solution, etalon);
                        break;
                    case 8:
                        ans[i] = MST(source, solution, etalon);
                        break;
                    case 9:
                        ans[i] = MinDist(source, solution, etalon);
                        break;
                    case 10:
                        ans[i] = Floyd(source, solution, etalon);
                        break;
                    case 11:
                        ans[i] = Isomorph(source, solution, etalon);
                        break;
                    case 12:
                        ans[i] = FloydForCRD(source, solution, etalon);
                        break;
                    case 13:
                        ans[i] = MaxMatch(source, solution, etalon);
                        break;
                    case 14:
                        ans[i] = ChromoNum(source, solution, etalon);
                        break;
                    default:
                        ans[i] = 6;
                        break;
                }
                source.Close();
                solution.Close();
                etalon.Close();
                res.Delete();

                //вывод по тестам 
            }
            return ans;
        }
        bool BadVertexNum(int[] vertexes, int n)
        {
            foreach (int v in vertexes)
                if (v >= n || v < 0) return true;
            return false;
        }
        int Euler(StreamReader stud, StreamReader etalon) //Checker!!!!
        {
            string ans = etalon.ReadLine().Trim();
            int[] studChain;

            try
            {
                string studAns = stud.ReadLine().Trim();
                if (ans != studAns) return 1;
                if (ans == "No") return 0;
                var tmpChain = stud.ReadLine().Split(new char[] {' '},StringSplitOptions.RemoveEmptyEntries);
                studChain = new int[tmpChain.Length];
                for (int i = 0; i < tmpChain.Length; i++)
                    studChain[i] = Int32.Parse(tmpChain[i]) - 1;
            }
            catch
            {
                return 5;
            }

            int edgeCount = 0;
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    if (m[i, j] > 0) edgeCount++;

            if (BadVertexNum(studChain, n)) return 1;
            int v = studChain[0];
            for (int i = 1; i < studChain.Length; i++)
            {
                if (m[v, studChain[i]] == 0) return 1;
                m[v, studChain[i]] = m[studChain[i], v] = 0;
                edgeCount -= 2;
                v = studChain[i];
            }
            if (ans == "Cycle") edgeCount -= 2;
            if (edgeCount != 0) return 1;
            return 0;
        }

        int GraphSearch(StreamReader inp, StreamReader stud, StreamReader etalon)
        {
            string ans = etalon.ReadLine().Trim();
            int[] bfs;
            int[] dfs;
            int v;
            try
            {
                string studAns = stud.ReadLine().Trim();
                if (ans == "0")
                    if (studAns == "0")
                        return 0;
                    else
                        return 1;
                var tmpChain = studAns.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                dfs = new int[tmpChain.Length];
                for (int i = 0; i < tmpChain.Length; i++)
                    dfs[i] = Int32.Parse(tmpChain[i]) - 1;
                tmpChain = stud.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                bfs = new int[tmpChain.Length];
                for (int i = 0; i < tmpChain.Length; i++)
                    bfs[i] = Int32.Parse(tmpChain[i]) - 1;
            }
            catch
            {
                return 5;
            }
            if (dfs.Length != bfs.Length) return 1;
            int n = Int32.Parse(inp.ReadLine());
            var m = new int[n, n];
            for (int i = 0; i < n; i++)
            {
                var tmp = inp.ReadLine().Split(' ');
                for (int j = 0; j < n; j++)
                    m[i, j] = Int32.Parse(tmp[j]);
            }
            var d = new int[n];
            for (int i = 1; i < n; i++)
                d[i] = -1;
            d[0] = 0;
            var q = new Queue<int>();
            q.Enqueue(0);
            while (q.Count > 0)
            {
                v = q.Dequeue();
                for (int i = 0; i < n; i++)
                    if (m[v, i] == 1 && d[i] == -1)
                    {
                        d[i] = d[v] + 1;
                        q.Enqueue(i);
                    }
            }
            if (BadVertexNum(bfs, n)) return 1;
            if (BadVertexNum(dfs, n)) return 1;
            for (int i = 1; i < bfs.Length; i++)
            {
                var di = d[bfs[i]];
                var dprev = d[bfs[i - 1]];
                if (di != dprev && di != dprev + 1) return 1;
            }
            v = dfs[0];
            if (v != 0) return 1;
            var st = new Stack<int>();
            var visited = new bool[n];
            for (int i = 1; i < n; i++)
                visited[i] = false;
            visited[0] = true;
            for (int i = 1; i < dfs.Length;)
            {
                if (m[v, dfs[i]] == 1)
                {
                    m[v, dfs[i]] = m[dfs[i], v] = 0;
                    st.Push(v);
                    v = dfs[i];
                    visited[v] = true;
                    i++;
                }
                else
                {
                    while (m[v, dfs[i]] != 1 && st.Count > 0)
                    {
                        for (int j = 0; j < n; j++)
                            if (m[v, j] == 1 && !visited[j]) return 1;
                        v = st.Pop();
                    }
                    if (m[v, dfs[i]] != 1)
                        return 1;
                }
            }
            return 0;
        }

        int CompCount(StreamReader stud, StreamReader etalon)
        {
            int ans = Int32.Parse(etalon.ReadLine().Trim());
            try
            {
                int studAns = Int32.Parse(stud.ReadLine().Trim());
                if (ans != studAns) return 1;
                return 0;
            }
            catch
            {
                return 5;
            }
        }

        int Bipart(StreamReader inp, StreamReader stud, StreamReader etalon) //Checker!!!!
        {
            string ans = etalon.ReadLine().Trim();
            string[] tmpChain;
            int[] studChain;
            int[] studChain2 = null;
            int v;
            try
            {
                string studAns = stud.ReadLine().Trim();
                if (ans == "NOT BIPARTITE")
                {
                    if (ans != studAns) return 1;
                }
                else
                {
                    tmpChain = studAns.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    studChain2 = new int[tmpChain.Length];
                    for (int i = 0; i < tmpChain.Length; i++)
                        studChain2[i] = Int32.Parse(tmpChain[i]) - 1;
                }
                if (stud.EndOfStream)
                    studChain = new int[0];
                else
                {
                    tmpChain = stud.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    studChain = new int[tmpChain.Length];
                    for (int i = 0; i < tmpChain.Length; i++)
                        studChain[i] = Int32.Parse(tmpChain[i]) - 1;
                }
            }
            catch
            {
                return 5;
            }
            int n = Int32.Parse(inp.ReadLine());
            var m = new int[n, n];
            for (int i = 0; i < n; i++)
            {
                var tmp = inp.ReadLine().Split(' ');
                for (int j = 0; j < n; j++)
                    m[i, j] = Int32.Parse(tmp[j]);
            }
            if (BadVertexNum(studChain, n)) return 1;
            if (ans == "NOT BIPARTITE")
            {
                if (studChain.Length % 2 == 0) return 1;
                v = studChain[0];
                for (int i = 1; i < studChain.Length; i++)
                {
                    if (m[v, studChain[i]] == 0) return 1;
                    m[v, studChain[i]] = m[studChain[i], v] = 0;
                    v = studChain[i];
                }
                if (m[v, studChain[0]] == 0) return 1;
            }
            else // Bipartite
            {
                if (BadVertexNum(studChain2, n)) return 1;
                var marked = new bool[n];
                for (int i = 0; i < n; i++)
                    marked[i] = false;
                for (int i = 0; i < studChain.Length; i++)
                    marked[studChain[i]] = true;
                for (int i = 0; i < studChain2.Length; i++)
                    marked[studChain2[i]] = true;
                for (int i = 0; i < n; i++)
                    if (!marked[i]) return 1;
                for (int i = 0; i < studChain.Length; i++)
                    for (int j = 0; j < studChain.Length; j++)
                        if (m[studChain[i], studChain[j]] == 1) return 1;
                for (int i = 0; i < studChain2.Length; i++)
                    for (int j = 0; j < studChain2.Length; j++)
                        if (m[studChain2[i], studChain2[j]] == 1) return 1;
            }
            return 0;
        }

        int TreeCRD(StreamReader inp, StreamReader stud, StreamReader etalon)
        {
            var tmp = etalon.ReadLine().Split(' ');
            int c = Int32.Parse(tmp[0]);
            int r = Int32.Parse(tmp[1]);
            int d = Int32.Parse(tmp[2]);
            try
            {
                tmp = stud.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                int studc = Int32.Parse(tmp[0]);
                int studr = Int32.Parse(tmp[1]);
                int studd = Int32.Parse(tmp[2]);
                if (studc != c) return 1;
                if (studr != r) return 1;
                if (studd != d) return 1;
                return 0;
            }
            catch
            {
                return 5;
            }
        }

        int Salesman(StreamReader inp, StreamReader stud, StreamReader etalon)
        {
            int ans = Int32.Parse(etalon.ReadLine().Trim());
            int studAns = 0;
            int[] studChain;
            try
            {
                studAns = Int32.Parse(stud.ReadLine().Trim());
                if (studAns > ans) return 1;
                var tmpChain = stud.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                studChain = new int[tmpChain.Length];
                for (int i = 0; i < tmpChain.Length; i++)
                    studChain[i] = Int32.Parse(tmpChain[i]) - 1;
            }
            catch
            {
                return 5;
            }
            int n = Int32.Parse(inp.ReadLine());
            var m = new int[n, n];
            for (int i = 0; i < n; i++)
            {
                var tmp = inp.ReadLine().Split(' ');
                for (int j = 0; j < n; j++)
                    m[i, j] = Int32.Parse(tmp[j]);
            }
            if (BadVertexNum(studChain, n)) return 1;
            if (studChain.Length != n) return 1;
            int totalLen = 0;
            int v = studChain[0];
            for (int i = 1; i < studChain.Length; i++)
            {
                totalLen += m[v, studChain[i]];
                v = studChain[i];
            }
            totalLen += m[v, studChain[0]];
            if (totalLen != studAns) return 1;
            return 0;
        }

        int MST(StreamReader inp, StreamReader stud, StreamReader etalon)
        {
            int ans = Int32.Parse(etalon.ReadLine().Trim());
            try
            {
                int studAns = Int32.Parse(stud.ReadLine().Trim());
                if (studAns != ans) return 1;
            }
            catch
            {
                return 5;
            }
            int n = Int32.Parse(inp.ReadLine());
            var m = new int[n, n];
            for (int i = 0; i < n; i++)
            {
                var tmp = inp.ReadLine().Split(' ');
                for (int j = 0; j < n; j++)
                {
                    if (tmp[j] == "~")
                        m[i, j] = -1;
                    else
                        m[i, j] = Int32.Parse(tmp[j]);
                }
            }
            var studChain = new Tuple<int,int>[n-1];
            try
            {
                for (int i = 0; i < n - 1; i++)
                {
                    if (stud.EndOfStream) return 1;
                    var tmp = stud.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    var v1 = Int32.Parse(tmp[0]) - 1;
                    var v2 = Int32.Parse(tmp[1]) - 1;
                    if (v1 >= n || v1 < 0 || v2 >= n || v2 < 0) return 1;
                    studChain[i] = Tuple.Create(v1, v2);
                }
            }
            catch
            {
                return 5;
            }
            int totalLen = 0;
            foreach (var e in studChain)
            {
                if (m[e.Item1, e.Item2] < 0) return 1;
                totalLen += m[e.Item1, e.Item2];
            }
            if (totalLen != ans) return 1;
            return 0;
        }
        
        int MinDist(StreamReader inp, StreamReader stud, StreamReader etalon)
        {
            int ans = Int32.Parse(etalon.ReadLine().Trim());
            int[] studChain;
            try
            {
                int studAns = Int32.Parse(stud.ReadLine().Trim());
                if (studAns != ans) return 1;
                var tmpChain = stud.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                studChain = new int[tmpChain.Length];
                for (int i = 0; i < tmpChain.Length; i++)
                    studChain[i] = Int32.Parse(tmpChain[i]) - 1;
            }
            catch
            {
                return 5;
            }
            int n = Int32.Parse(inp.ReadLine());
            var m = new int[n, n];
            for (int i = 0; i < n; i++)
            {
                var tmp = inp.ReadLine().Split(' ');
                for (int j = 0; j < n; j++)
                {
                    if (tmp[j] == "~")
                        m[i, j] = -1;
                    else
                        m[i, j] = Int32.Parse(tmp[j]);
                }
            }
            var tmp2 = inp.ReadLine().Split(' ');
            int startV = Int32.Parse(tmp2[0]) - 1;
            int finV = Int32.Parse(tmp2[1]) - 1;
            if (BadVertexNum(studChain, n)) return 1;
            if (studChain[0] != startV) return 1;
            if (studChain[studChain.Length - 1] != finV) return 1;
            int totalLen = 0;
            int v = studChain[0];
            for (int i = 1; i < studChain.Length; i++)
            {
                if (m[v, studChain[i]] < 0) return 1;
                totalLen += m[v, studChain[i]];
                v = studChain[i];
            }
            if (totalLen != ans) return 1;
            return 0;
        }

        int Floyd(StreamReader inp, StreamReader stud, StreamReader etalon)
        {
            int n = Int32.Parse(inp.ReadLine());
            var m = new int[n, n];
            var ans = new int[n, n];
            string[] tmp;
            for (int i = 0; i < n; i++)
            {
                tmp = inp.ReadLine().Split(' ');
                for (int j = 0; j < n; j++)
                {
                    if (tmp[j] == "~")
                        m[i, j] = Int32.MaxValue;
                    else
                        m[i, j] = Int32.Parse(tmp[j]);
                }
                tmp = etalon.ReadLine().Split(' ');
                for (int j = 0; j < n; j++)
                    ans[i, j] = Int32.Parse(tmp[j]);
            }
            var tmp2 = inp.ReadLine().Split(' ');
            int startV = Int32.Parse(tmp2[0]) - 1;
            int finV = Int32.Parse(tmp2[1]) - 1;
            
            int[] studChain;
            try
            {
                for (int i = 0; i < n; i++)
                {
                    tmp = stud.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int j = 0; j < n; j++)
                        if (Int32.Parse(tmp[j]) != ans[i, j]) return 1;
                }
                tmp = stud.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                studChain = new int[tmp.Length];
                for (int i = 0; i < tmp.Length; i++)
                    studChain[i] = Int32.Parse(tmp[i]) - 1;
            }
            catch
            {
                return 5;
            }

            if (BadVertexNum(studChain, n)) return 1;
            if (studChain[0] != startV) return 1;
            if (studChain[studChain.Length - 1] != finV) return 1;
            int totalLen = 0;
            int v = studChain[0];
            for (int i = 1; i < studChain.Length; i++)
            {
                if (m[v, studChain[i]] == Int32.MaxValue) return 1;
                totalLen += m[v, studChain[i]];
                v = studChain[i];
            }
            if (totalLen != ans[startV, finV]) return 1;
            return 0;
        }

        int Isomorph(StreamReader inp, StreamReader stud, StreamReader etalon)
        {
            var ans = etalon.ReadLine().Trim();
            try
            {
                var studAns = stud.ReadLine().Trim();
                if (ans != studAns) return 1;
                return 0;
            }
            catch
            {
                return 5;
            }
        }

        int FloydForCRD(StreamReader inp, StreamReader stud, StreamReader etalon)
        {
            var rd = etalon.ReadLine().Split(' ').Select(s => int.Parse(s)).ToArray();
            var center = etalon.ReadLine().Trim(' ').Split(' ').Select(s => int.Parse(s)).ToList();
            int[] rdStud = new int[2];
            var centerStud = new List<int>();
            try
            {
                rdStud = stud.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).ToArray();
                centerStud = stud.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).ToList();
            }
            catch
            {
                return 5;
            }
            if (rdStud.Length != 2) return 5;
            if (rd[0] != rdStud[0] || rd[1] != rdStud[1] || center.Count != centerStud.Count) return 1;
            foreach (var v in center)
                if (!centerStud.Contains(v)) return 1;
            return 0;
        }

        int MaxMatch(StreamReader inp, StreamReader stud, StreamReader etalon)
        {
            var ansCnt = int.Parse(etalon.ReadLine());

            int ansCntStud = 0;
            var ansStud = new List<Tuple<int,int>>();
            try
            {
                ansCntStud = int.Parse(stud.ReadLine());
                for (int i = 0; i < ansCntStud; ++i)
                {
                    var tmp = stud.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)-1).ToArray();
                    ansStud.Add(new Tuple<int, int>(tmp[0], tmp[1]));
                }
            }
            catch
            {
                return 5;
            }

            if (ansCntStud != ansCnt) return 1;

            var n = int.Parse(inp.ReadLine());
            var m = new int[n][];
            for (int i = 0; i < n; ++i)
                m[i] = inp.ReadLine().Split(' ').Select(s => int.Parse(s)).ToArray();
            var used1 = new bool[n];
            var used2 = new bool[n];
            for (int i = 0; i < n; ++i) used1[i] = used2[i] = false;
            foreach (var e in ansStud)
            {
                if (used1[e.Item1] || used2[e.Item2]) return 1;
                if (m[e.Item1][e.Item2] != 1) return 1;
                used1[e.Item1] = used2[e.Item2] = true;
            }

            return 0;
        }

        int ChromoNum(StreamReader inp, StreamReader stud, StreamReader etalon)
        {
            var n = int.Parse(inp.ReadLine());
            var m = new int[n][];
            for (int i = 0; i < n; ++i)
                m[i] = inp.ReadLine().Split(' ').Select(s => int.Parse(s)).ToArray();

            var ansCnt = int.Parse(etalon.ReadLine());

            int ansCntStud = 0;
            var ansStud = new int[n];
            try
            {
                ansCntStud = int.Parse(stud.ReadLine());
                ansStud = stud.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).ToArray();
            }
            catch
            {
                return 5;
            }

            if (ansCntStud > ansCnt) return 1;

            if (ansStud.Length != n) return 5;
            foreach (var c in ansStud)
                if (c < 1 || c > ansCntStud) return 5;

            for (int i = 0; i < n; ++i)
                for (int j = 0; j < i; ++j)
                    if (m[i][j] == 1 && ansStud[i] == ansStud[j]) return 1;       

            return 0;
        }

        int Chromo(StreamReader inp, StreamReader stud, StreamReader etalon)
        {
            etalon.ReadLine();
            int ans = Int32.Parse(etalon.ReadLine().Trim());
            try
            {
                stud.ReadLine();
                int studAns = Int32.Parse(stud.ReadLine().Trim());
                if (ans != studAns) return 1;
                return 0;
            }
            catch
            {
                return 5;
            }
        }
    }
}
