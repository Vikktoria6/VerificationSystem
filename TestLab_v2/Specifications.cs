using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TestLab_v2
{
    internal class Specifications
    {
        static public int CountSpecific = 12;
        public List<int> Verify = new List<int>();

        private Dictionary<string, int> testInfo = new Dictionary<string, int>
        {
            { "Граф без ребер", -1},
            { "Полный граф", -1 },
            { "Граф с одной вершиной", -1 },
            { "Граф имеет больше 15 вершин", -1},
            { "Связный граф", -1},
            { "Несвязный граф", -1},
            { "Неориентированный граф", -1},
            { "Ориентированный граф", -1},
            { "Дерево", -1},
            { "Граф с циклами", -1},
            { "Взвешенный граф", -1},
            { "Граф с орицательными весами", -1}
        };


        public void FindError(List<int> spec, bool ok)
        {
            for (int i = 0; i < CountSpecific; i++)
            {
                if (testInfo.ElementAt(i).Value == -1 && spec[i] == 1 && ok) testInfo[testInfo.ElementAt(i).Key] = 0;
                if (testInfo.ElementAt(i).Value == -1 && spec[i] == 1 && !ok) testInfo[testInfo.ElementAt(i).Key] = 1;
                if (testInfo.ElementAt(i).Value == 1 && spec[i] == 1 && ok) testInfo[testInfo.ElementAt(i).Key] = 0;
            }
        }

        public string MsgError()        
        {
            string msg = "";

            foreach (var elem in testInfo)
            {
                if (elem.Value == 1)
                {
                    msg = msg + elem.Key + " (" + Verify[Array.IndexOf(testInfo.Keys.ToArray(), elem.Key)] + ")" + "\n";
                }
            }
            if (msg != "") msg = "Возможные ошибки на: \n" + "(В скобках указано количество тестов) \n"+ msg;
            return msg;
        }

        public void SumSpec(List<int> spec)        
        {
            for (int i = 0; i < CountSpecific; i++) {
                if (Verify.Count < CountSpecific) {
                    Verify.Add(spec[i]);
                }
                else Verify[i] += spec[i];
            }
        }

    }
}
