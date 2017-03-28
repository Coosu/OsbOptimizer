using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibOSB;
using LibOSB.ActionTypes;

namespace OsuStoryboard.OSBOptimizer
{
    static class ChkRules
    {
        private static void chkM(sbObject sb)
        {
            int?[] start, end;

            if (sb.Move.Count != 0)
            {

                List<int?> tmpstart = sb.Move.starttime_L.ToList();
                tmpstart.Sort();

                if (IsEqual(tmpstart, sb.Move.starttime_L)) tmpstart = null;
                else
                {
                    start = sb.Move.starttime_L.ToArray();
                    end = sb.Move.endtime_L.ToArray();
                    var line = sb.Move.M.ToArray();
                    Sort(start, end, line);

                    sb.Move.M = new List<Move>(line);
                }

            }
        }
        private static void chkF(sbObject sb)
        {
            int?[] start, end;

            if (sb.Fade.Count != 0)
            {

                List<int?> tmpstart = sb.Fade.starttime_L.ToList();
                tmpstart.Sort();

                if (IsEqual(tmpstart, sb.Fade.starttime_L)) tmpstart = null;
                else
                {
                    start = sb.Fade.starttime_L.ToArray();
                    end = sb.Fade.endtime_L.ToArray();
                    var line = sb.Fade.F.ToArray();
                    Sort(start, end, line);

                    sb.Fade.F = new List<Fade>(line);
                }

            }
        }

        public static bool Check(sbObject sb)
        {
            chkM(sb); chkF(sb);


            for (int i = 1; i < sb.Move.Count; i++)
                if (sb.Move[i].StartTime < sb.Move[i - 1].EndTime) return false;
            for (int i = 1; i < sb.Fade.Count; i++) 
                if (sb.Fade[i].StartTime < sb.Fade[i - 1].EndTime) return false;

            return true;
        }
        private static bool IsEqual(List<int?> A, List<int?> B)
        {
            for (int i = 0; i < A.Count; i++)
            {
                if (A[i] != B[i]) return false;
            }
            return true;
        }
        private static int?[] Sort(int?[] start, int?[] end, object[] line)
        {
            int n = start.Length;
            int low = 0;
            int high = n - 1; //设置变量的初始值  
            int? tmp;
            object tmpobj;
            int j;
            while (low < high)
            {
                for (j = low; j < high; ++j) //正向冒泡，找到最大者  
                    if (start[j] > start[j + 1])
                    {
                        tmp = start[j]; start[j] = start[j + 1]; start[j + 1] = tmp;
                        tmp = end[j]; end[j] = end[j + 1]; end[j + 1] = tmp;
                        tmpobj = line[j]; line[j] = line[j + 1]; line[j + 1] = tmpobj;
                    }
                --high; //修改high值，前移一位  
                for (j = high; j > low; --j) //反向冒泡，找到最小者  
                    if (start[j] < start[j - 1])
                    {
                        tmp = start[j]; start[j] = start[j - 1]; start[j - 1] = tmp;
                        tmp = end[j]; end[j] = end[j - 1]; end[j - 1] = tmp;
                        tmpobj = line[j]; line[j] = line[j - 1]; line[j - 1] = tmpobj;
                    }
                ++low; //修改low值，后移一位  
            }
            return start;
        }
    }
}
