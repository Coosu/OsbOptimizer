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
        private static void ChkM(SBObject sb)
        {
            int?[] start, end;

            if (sb.Move.Count != 0)
            {

                List<int?> tmpstart = sb.Move.startTime_L.ToList();
                tmpstart.Sort();

                if (IsEqual(tmpstart, sb.Move.startTime_L)) tmpstart = null;
                else
                {
                    start = sb.Move.startTime_L.ToArray();
                    end = sb.Move.endTime_L.ToArray();
                    var line = sb.Move.M.ToArray();
                    Sort(start, end, line);

                    sb.Move.M = new List<Move>(line);
                }

            }
        }
        private static void ChkF(SBObject sb)
        {
            int?[] start, end;

            if (sb.Fade.Count != 0)
            {

                List<int?> tmpstart = sb.Fade.startTime_L.ToList();
                tmpstart.Sort();

                if (IsEqual(tmpstart, sb.Fade.startTime_L)) tmpstart = null;
                else
                {
                    start = sb.Fade.startTime_L.ToArray();
                    end = sb.Fade.endTime_L.ToArray();
                    var line = sb.Fade.F.ToArray();
                    Sort(start, end, line);

                    sb.Fade.F = new List<Fade>(line);
                }

            }
        }
        private static void ChkS(SBObject sb)
        {
            int?[] start, end;

            if (sb.Scale.Count != 0)
            {

                List<int?> tmpstart = sb.Scale.startTime_L.ToList();
                tmpstart.Sort();

                if (IsEqual(tmpstart, sb.Scale.startTime_L)) tmpstart = null;
                else
                {
                    start = sb.Scale.startTime_L.ToArray();
                    end = sb.Scale.endTime_L.ToArray();
                    var line = sb.Scale.S.ToArray();
                    Sort(start, end, line);

                    sb.Scale.S = new List<Scale>(line);
                }

            }
        }
        private static void ChkR(SBObject sb)
        {
            int?[] start, end;

            if (sb.Rotate.Count != 0)
            {

                List<int?> tmpstart = sb.Rotate.startTime_L.ToList();
                tmpstart.Sort();

                if (IsEqual(tmpstart, sb.Rotate.startTime_L)) tmpstart = null;
                else
                {
                    start = sb.Rotate.startTime_L.ToArray();
                    end = sb.Rotate.endTime_L.ToArray();
                    var line = sb.Rotate.R.ToArray();
                    Sort(start, end, line);

                    sb.Rotate.R = new List<Rotate>(line);
                }

            }
        }
        private static void ChkC(SBObject sb)
        {
            int?[] start, end;

            if (sb.Color.Count != 0)
            {

                List<int?> tmpstart = sb.Color.startTime_L.ToList();
                tmpstart.Sort();

                if (IsEqual(tmpstart, sb.Color.startTime_L)) tmpstart = null;
                else
                {
                    start = sb.Color.startTime_L.ToArray();
                    end = sb.Color.endTime_L.ToArray();
                    var line = sb.Color.C.ToArray();
                    Sort(start, end, line);

                    sb.Color.C = new List<Color>(line);
                }

            }
        }
        private static void ChkMX(SBObject sb)
        {
            int?[] start, end;

            if (sb.MoveX.Count != 0)
            {

                List<int?> tmpstart = sb.MoveX.startTime_L.ToList();
                tmpstart.Sort();

                if (IsEqual(tmpstart, sb.MoveX.startTime_L)) tmpstart = null;
                else
                {
                    start = sb.MoveX.startTime_L.ToArray();
                    end = sb.MoveX.endTime_L.ToArray();
                    var line = sb.MoveX.MX.ToArray();
                    Sort(start, end, line);

                    sb.MoveX.MX = new List<MoveX>(line);
                }

            }
        }
        private static void ChkMY(SBObject sb)
        {
            int?[] start, end;

            if (sb.MoveY.Count != 0)
            {

                List<int?> tmpstart = sb.MoveY.startTime_L.ToList();
                tmpstart.Sort();

                if (IsEqual(tmpstart, sb.MoveY.startTime_L)) tmpstart = null;
                else
                {
                    start = sb.MoveY.startTime_L.ToArray();
                    end = sb.MoveY.endTime_L.ToArray();
                    var line = sb.MoveY.MY.ToArray();
                    Sort(start, end, line);

                    sb.MoveY.MY = new List<MoveY>(line);
                }

            }
        }
        private static void ChkV(SBObject sb)
        {
            int?[] start, end;

            if (sb.Vector.Count != 0)
            {

                List<int?> tmpstart = sb.Vector.startTime_L.ToList();
                tmpstart.Sort();

                if (IsEqual(tmpstart, sb.Vector.startTime_L)) tmpstart = null;
                else
                {
                    start = sb.Vector.startTime_L.ToArray();
                    end = sb.Vector.endTime_L.ToArray();
                    var line = sb.Vector.V.ToArray();
                    Sort(start, end, line);

                    sb.Vector.V = new List<Vector>(line);
                }

            }
        }

        public static bool Check(SBObject sb)
        {
            ChkM(sb); ChkF(sb); ChkS(sb); ChkR(sb);
            ChkC(sb); ChkMX(sb); ChkMY(sb); ChkV(sb);

            for (int i = 1; i < sb.Move.Count; i++)
                if (sb.Move[i].StartTime < sb.Move[i - 1].EndTime ||
                    sb.Move[i].StartTime == sb.Move[i - 1].StartTime && sb.Move[i].EndTime == sb.Move[i - 1].EndTime &&
                    sb.Move[i].StartTime == sb.Move[i].EndTime) return false;
            for (int i = 1; i < sb.Fade.Count; i++)
                if (sb.Fade[i].StartTime < sb.Fade[i - 1].EndTime ||
                    sb.Fade[i].StartTime == sb.Fade[i - 1].StartTime && sb.Fade[i].EndTime == sb.Fade[i - 1].EndTime &&
                    sb.Fade[i].StartTime == sb.Fade[i].EndTime) return false;
            for (int i = 1; i < sb.Scale.Count; i++)
                if (sb.Scale[i].StartTime < sb.Scale[i - 1].EndTime ||
                    sb.Scale[i].StartTime == sb.Scale[i - 1].StartTime && sb.Scale[i].EndTime == sb.Scale[i - 1].EndTime &&
                    sb.Scale[i].StartTime == sb.Scale[i].EndTime) return false;
            for (int i = 1; i < sb.Rotate.Count; i++)
                if (sb.Rotate[i].StartTime < sb.Rotate[i - 1].EndTime ||
                    sb.Rotate[i].StartTime == sb.Rotate[i - 1].StartTime && sb.Rotate[i].EndTime == sb.Rotate[i - 1].EndTime &&
                    sb.Rotate[i].StartTime == sb.Rotate[i].EndTime) return false;
            for (int i = 1; i < sb.Color.Count; i++)
                if (sb.Color[i].StartTime < sb.Color[i - 1].EndTime ||
                    sb.Color[i].StartTime == sb.Color[i - 1].StartTime && sb.Color[i].EndTime == sb.Color[i - 1].EndTime &&
                    sb.Color[i].StartTime == sb.Color[i].EndTime) return false;
            for (int i = 1; i < sb.MoveX.Count; i++)
                if (sb.MoveX[i].StartTime < sb.MoveX[i - 1].EndTime ||
                    sb.MoveX[i].StartTime == sb.MoveX[i - 1].StartTime && sb.MoveX[i].EndTime == sb.MoveX[i - 1].EndTime &&
                    sb.MoveX[i].StartTime == sb.MoveX[i].EndTime) return false;
            for (int i = 1; i < sb.MoveY.Count; i++)
                if (sb.MoveY[i].StartTime < sb.MoveY[i - 1].EndTime ||
                    sb.MoveY[i].StartTime == sb.MoveY[i - 1].StartTime && sb.MoveY[i].EndTime == sb.MoveY[i - 1].EndTime &&
                    sb.MoveY[i].StartTime == sb.MoveY[i].EndTime) return false;
            for (int i = 1; i < sb.Vector.Count; i++)
                if (sb.Vector[i].StartTime < sb.Vector[i - 1].EndTime ||
                    sb.Vector[i].StartTime == sb.Vector[i - 1].StartTime && sb.Vector[i].EndTime == sb.Vector[i - 1].EndTime &&
                    sb.Vector[i].StartTime == sb.Vector[i].EndTime) return false;

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
