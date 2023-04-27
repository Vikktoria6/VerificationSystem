

//этот класс не используется 

using System;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace TestLab_v2
{
    class Launcher
    {
        int testsCount;
        string testingSolution;
        string testsFolder;
        public event ShowMsg Show;
        void Write(string msg)
        {
            if (Show != null)
                Show(msg);
        }
        public Launcher(int testsCount, string testingSolution, string testsFolder)
        {
            this.testsCount = testsCount;
            this.testingSolution = testingSolution;
            this.testsFolder = testsFolder;
        }
        public void Check()
        {
            Process solver = new Process();
            solver.StartInfo.FileName = testingSolution;
            solver.StartInfo.Arguments = "";
            string tmpPath = Directory.GetCurrentDirectory();
            for (int i = 0; i < testsCount; i++)
            {
                Write("\nTest " + (i+1).ToString() + ": ");
                FileInfo f = new FileInfo(testsFolder + "\\" + (i+1).ToString() + "_input.txt");
                f.CopyTo(tmpPath + "\\input.txt", true);
                solver.Start();
                FileInfo res = new FileInfo(tmpPath + "\\output.txt");
                int seconds = 0;
                do
                {
                    if (!solver.HasExited)
                    {
                        solver.Refresh();
                        Write(".");
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
                        continue;
                    }
                    continue;
                }
                seconds = 0;
                while (!res.Exists && seconds < 10)
                {
                    Console.Write(".");
                    Thread.Sleep(1000);
                    seconds++;
                }
                if (!res.Exists)
                {
                    continue;
                }
                res.CopyTo(tmpPath + "\\StudSolve\\" + (i + 1).ToString() + "_output.txt", true);
                res.Delete();
            }
        }
    }
}
