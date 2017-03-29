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
        private static void chkS(sbObject sb)
        {
            int?[] start, end;

            if (sb.Scale.Count != 0)
            {

                List<int?> tmpstart = sb.Scale.starttime_L.ToList();
                tmpstart.Sort();

                if (IsEqual(tmpstart, sb.Scale.starttime_L)) tmpstart = null;
                else
                {
                    start = sb.Scale.starttime_L.ToArray();
                    end = sb.Scale.endtime_L.ToArray();
                    var line = sb.Scale.S.ToArray();
                    Sort(start, end, line);

                    sb.Scale.S = new List<Scale>(line);
                }

            }
        }
        private static void chkR(sbObject sb)
        {
            int?[] start, end;

            if (sb.Rotate.Count != 0)
            {

                List<int?> tmpstart = sb.Rotate.starttime_L.ToList();
                tmpstart.Sort();

                if (IsEqual(tmpstart, sb.Rotate.starttime_L)) tmpstart = null;
                else
                {
                    start = sb.Rotate.starttime_L.ToArray();
                    end = sb.Rotate.endtime_L.ToArray();
                    var line = sb.Rotate.R.ToArray();
                    Sort(start, end, line);

                    sb.Rotate.R = new List<Rotate>(line);
                }

            }
        }
        private static void chkC(sbObject sb)
        {
            int?[] start, end;

            if (sb.Color.Count != 0)
            {

                List<int?> tmpstart = sb.Color.starttime_L.ToList();
                tmpstart.Sort();

                if (IsEqual(tmpstart, sb.Color.starttime_L)) tmpstart = null;
                else
                {
                    start = sb.Color.starttime_L.ToArray();
                    end = sb.Color.endtime_L.ToArray();
                    var line = sb.Color.C.ToArray();
                    Sort(start, end, line);

                    sb.Color.C = new List<Color>(line);
                }

            }
        }
        private static void chkMX(sbObject sb)
        {
            int?[] start, end;

            if (sb.MoveX.Count != 0)
            {

                List<int?> tmpstart = sb.MoveX.starttime_L.ToList();
                tmpstart.Sort();

                if (IsEqual(tmpstart, sb.MoveX.starttime_L)) tmpstart = null;
                else
                {
                    start = sb.MoveX.starttime_L.ToArray();
                    end = sb.MoveX.endtime_L.ToArray();
                    var line = sb.MoveX.MX.ToArray();
                    Sort(start, end, line);

                    sb.MoveX.MX = new List<MoveX>(line);
                }

            }
        }
        private static void chkMY(sbObject sb)
        {
            int?[] start, end;

            if (sb.MoveY.Count != 0)
            {

                List<int?> tmpstart = sb.MoveY.starttime_L.ToList();
                tmpstart.Sort();

                if (IsEqual(tmpstart, sb.MoveY.starttime_L)) tmpstart = null;
                else
                {
                    start = sb.MoveY.starttime_L.ToArray();
                    end = sb.MoveY.endtime_L.ToArray();
                    var line = sb.MoveY.MY.ToArray();
                    Sort(start, end, line);

                    sb.MoveY.MY = new List<MoveY>(line);
                }

            }
        }
        private static void chkV(sbObject sb)
        {
            int?[] start, end;

            if (sb.Vector.Count != 0)
            {

                List<int?> tmpstart = sb.Vector.starttime_L.ToList();
                tmpstart.Sort();

                if (IsEqual(tmpstart, sb.Vector.starttime_L)) tmpstart = null;
                else
                {
                    start = sb.Vector.starttime_L.ToArray();
                    end = sb.Vector.endtime_L.ToArray();
                    var line = sb.Vector.V.ToArray();
                    Sort(start, end, line);

                    sb.Vector.V = new List<Vector>(line);
                }

            }
        }

        public static bool Check(sbObject sb)
        {
            chkM(sb); chkF(sb); chkS(sb); chkR(sb);
            chkC(sb); chkMX(sb); chkMY(sb); chkV(sb);

            for (int i = 1; i < sb.Move.Count; i++)
                if (sb.Move[i].StartTime < sb.Move[i - 1].EndTime) return false;
            for (int i = 1; i < sb.Fade.Count; i++)
                if (sb.Fade[i].StartTime < sb.Fade[i - 1].EndTime) return false;
            for (int i = 1; i < sb.Scale.Count; i++)
                if (sb.Scale[i].StartTime < sb.Scale[i - 1].EndTime) return false;
            for (int i = 1; i < sb.Rotate.Count; i++)
                if (sb.Rotate[i].StartTime < sb.Rotate[i - 1].EndTime) return false;
            for (int i = 1; i < sb.Color.Count; i++)
                if (sb.Color[i].StartTime < sb.Color[i - 1].EndTime) return false;
            for (int i = 1; i < sb.MoveX.Count; i++)
                if (sb.MoveX[i].StartTime < sb.MoveX[i - 1].EndTime) return false;
            for (int i = 1; i < sb.MoveY.Count; i++)
                if (sb.MoveY[i].StartTime < sb.MoveY[i - 1].EndTime) return false;
            for (int i = 1; i < sb.Vector.Count; i++)
                if (sb.Vector[i].StartTime < sb.Vector[i - 1].EndTime) return false;

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
