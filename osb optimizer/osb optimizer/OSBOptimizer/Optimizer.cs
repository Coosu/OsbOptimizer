using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using LibOSB;
using OsuStoryboard.OSBOptimizer;
using System.Text.RegularExpressions;

namespace LibOSB
{
    class Optimizer
    {
        public static int Decimal;
        public static bool ifPause = false;
        public static bool ifPause2 = true;
        public static bool ifPause3 = true;
        public static bool ifCheck = true;
        public static bool ifVar = true;

        private static long totalline = 0;
        private static double progress;

        public static bool Pause = false;
        public static bool Display = false;
        public static bool Finsh = false;
        public static StringBuilder sb;

        static SBObject obj;
        private static string before;
        private static string after;

        private static bool isT, isL;

        //static bool IsT;
        static private int errnum;
        static private int currentObjLine;

        static int currentLine = 0;


        public static string Before { get => before; }
        public static string After { get => after; }

        public static int CurrentObjLine { get => currentObjLine; }

        public static double Progress { get => progress; }

        private static void Exception(Exception ex = null, string exMessage = null, bool isobject = false)
        {
            if (ex != null)
            {
                Reporter.ExceptionMessages.Add(currentLine, ex.Message, DateTime.Now);
            }
            else
            {
                if (isobject == false)
                    Reporter.ExceptionMessages.Add(currentLine, exMessage, DateTime.Now);
                else
                    Reporter.ExceptionMessages.Add(CurrentObjLine, exMessage, DateTime.Now);
            }
            errnum++;
            Reporter.TotalUnrecognizedLines = errnum;
        }

        private static void Reset()
        {
            isT = false;
            isL = false;
            obj = null;
            //IsT = false;
            errnum = 0;
            totalline = 0;
            progress = 0;
            Pause = false;
            Display = false;
            Finsh = false;
            before = "";
            after = "";
            errnum = 0;
            currentObjLine = 0;
            currentLine = 0;

            Variables.lst.Clear();
        }

        /// <summary>
        /// 开始读取一个受支持的文件。
        /// </summary>
        /// <param name="root"></param>
        /// <param name="lines"></param>
        public static void ReadFile(string root, long lines)
        {
            sb = new StringBuilder();
            sb.AppendLine("[Events]");

            SBscript sbscript = new SBscript();
            Reset();
            Finsh = false;
            using (StreamReader sr = new StreamReader(root))
            {
                totalline = lines;
                //int ind, ind_l;
                string Line = sr.ReadLine();

                //var scriptSB = new StringBuilder();
                //string part1 = "", part2, part3;
                //bool ifinpart = false;

                do
                {
                    currentLine++;
                    progress = (double)currentLine / (double)totalline;

                    #region osb Script(施工中)
                    /*
                    ind = Line.IndexOf("{");
                    ind_l = Line.LastIndexOf("}");

                    if (ind > -1 && ifinpart == false)
                    {
                        if (ind > 0) part1 = Line.Substring(0, ind);
                        else part1 = "";

                        if (ind_l == -1)
                        {
                            ifinpart = true;
                            scriptSB.Append(Line.Substring(ind + 1, Line.Length - ind - 1).Trim());
                            Line = sr.ReadLine();
                            continue;
                        }
                        else
                        {
                            scriptSB.Append(Line.Substring(ind + 1, ind_l - ind - 1).Trim());
                            part2 = sbscript.Check(scriptSB.ToString().Trim(), currentLine);
                            if (ind_l < Line.Length - 1) part3 = Line.Substring(ind_l + 1, Line.Length - (ind_l + 1));
                            else part3 = "";

                            Line = part1 + part2 + part3;

                            ifinpart = false;
                            part1 = ""; part2 = ""; part3 = "";
                            scriptSB.Clear();
                        }
                    }
                    else if (ifinpart)
                    {

                        if (ind_l == -1)
                        {
                            scriptSB.Append(Line);
                            Line = sr.ReadLine();
                            continue;
                        }
                        else
                        {
                            scriptSB.Append(Line.Substring(ind + 1, ind_l - ind - 1).Trim());
                            part2 = sbscript.Check(scriptSB.ToString().Trim(), currentLine);
                            if (ind_l < Line.Length - 1) part3 = Line.Substring(ind_l + 1, Line.Length - (ind_l + 1));
                            else part3 = "";

                            Line = part1 + part2 + part3;

                            ifinpart = false;
                            part1 = ""; part2 = ""; part3 = "";
                            scriptSB.Clear();
                        }
                    }
                    */
                    #endregion

                    if (Line.Trim() != "")
                    {
                        string[] @params = Line.Split(',');

                        装箱(@params, currentLine);

                    }
                    //if (errnum == 5)
                    Line = sr.ReadLine();

                } while (Line != null);

                WaitToCompare();
                sr.Close();
            }

            sb.AppendLine(@"//Storyboard Sound Samples");
            if (ifVar)
            {
                var sb2 = new StringBuilder();
                sb2.AppendLine("[Variables]");
                sb2.AppendLine("$1=BottomLeft");
                sb2.AppendLine("$2=BottomCentre");
                sb2.AppendLine("$3=BottomRight");
                sb2.AppendLine("$4=CentreLeft");
                sb2.AppendLine("$5=Centre");
                sb2.AppendLine("$6=CentreRight");
                sb2.AppendLine("$7=TopLeft");
                sb2.AppendLine("$8=TopCentre");
                sb2.AppendLine("$9=TopRight");
                sb2.AppendLine("$a=Sprite");
                sb2.AppendLine("$b=Animation");
                sb2.AppendLine("$c=Background");
                sb2.AppendLine("$d=Foreground");
                sb2.AppendLine("$e=Fail");
                sb2.AppendLine("$f=Pass");
                sb2.AppendLine("$z=Sprite,Foreground,Centre,");
                sb2.AppendLine("$y=Sprite,Foreground,TopLeft,");
                sb2.AppendLine("$x=Sprite,Background,Centre,");
                sb2.AppendLine("$w=Sprite,Background,TopLeft,");
                sb2.AppendLine("$v=,320,240");
                sb2.AppendLine("$Ga= F,0,"); sb2.AppendLine("$Gb= F,1,"); sb2.AppendLine("$Gc= F,2,");
                sb2.AppendLine("$Gd= M,0,"); sb2.AppendLine("$Ge= M,1,"); sb2.AppendLine("$Gf= M,2,");
                sb2.AppendLine("$Gg= MX,0,"); sb2.AppendLine("$Gh= MX,1,"); sb2.AppendLine("$Gi= MX,2,");
                sb2.AppendLine("$Gj= MY,0,"); sb2.AppendLine("$Gk= MY,1,"); sb2.AppendLine("$Gl= MY,2,");
                sb2.AppendLine("$Gm= P,0,"); sb2.AppendLine("$Gn= P,1,"); sb2.AppendLine("$Go= P,2,");
                sb2.AppendLine("$Gp= R,0,"); sb2.AppendLine("$Gq= R,1,"); sb2.AppendLine("$Gr= R,2,");
                sb2.AppendLine("$Gs= S,0,"); sb2.AppendLine("$Gt= S,1,"); sb2.AppendLine("$Gu= S,2,");
                sb2.AppendLine("$Gv= V,0,"); sb2.AppendLine("$Gw= V,1,"); sb2.AppendLine("$Gx= V,2,");
                sb2.AppendLine("$Ha= C,0,"); sb2.AppendLine("$Hb= C,1,"); sb2.AppendLine("$Hc= C,2,");
                sb2.AppendLine("$i=0,1"); sb2.AppendLine("$j=1,0");
                //sb2.AppendLine("$k=,LoopForever"); sb2.AppendLine("$l=,LoopOnce");

                sb.Replace("Sprite,Foreground,Centre,", "$z");
                sb.Replace("Sprite,Foreground,TopLeft,", "$y");
                sb.Replace("Sprite,Background,Centre,", "$x");
                sb.Replace("Sprite,Background,TopLeft,", "$w");

                sb.Replace("BottomLeft", "$1");
                sb.Replace("BottomCentre", "$2");
                sb.Replace("BottomRight", "$3");
                sb.Replace("CentreLeft", "$4");
                sb.Replace("Centre", "$5");
                sb.Replace("CentreRight", "$6");
                sb.Replace("TopLeft", "$7");
                sb.Replace("TopCentre", "$8");
                sb.Replace("TopRight", "$9");

                sb.Replace("Background", "$c");
                sb.Replace("Fail", "$e");
                sb.Replace("Pass", "$f");
                sb.Replace("Foreground", "$d");

                sb.Replace("Sprite", "$a");
                sb.Replace("Animation", "$b");

                //sb.Replace("LoopForever", "$k");
                //sb.Replace("LoopOnce", "$l");

                //string[] tmpstr = Regex.Split(sb.ToString(), "\r\n", RegexOptions.IgnoreCase);
                //foreach (string str in tmpstr)
                //{
                //    bool ifreplaced = false;
                //    int tmp2 = 0;
                //    for (int test = 0; test < Variables.lst.Count; test++)
                //    {
                //        if (Variables.lst[test].Count > 1)
                //        {
                //            string strbefore = str;
                //            str.Replace(Variables.lst[test].Name + ",", "$R" + string.Format("{0:D3}", tmp2) + ",");
                //            sb2.AppendLine("$R" + string.Format("{0:D3}", tmp2) + "=" + Variables.lst[test].Name);
                //            tmp2++;
                //        }
                //    }
                //}
                int tmp = 0;
                for (int test = 0; test < Variables.lst.Count; test++)
                {
                    if (Variables.lst[test].Count > 1)
                    {
                        sb.Replace(Variables.lst[test].Name + ",", "$R" + string.Format("{0:D3}", tmp) + ",");
                        sb2.AppendLine("$R" + string.Format("{0:D3}", tmp) + "=" + Variables.lst[test].Name);
                        tmp++;
                    }
                }

                sb.Replace(" F,0,", "$Ga"); sb.Replace(" F,1,", "$Gb"); sb.Replace(" F,2,", "$Gc");
                sb.Replace(" M,0,", "$Gd"); sb.Replace(" M,1,", "$Ge"); sb.Replace(" M,2,", "$Gf");
                sb.Replace(" MX,0,", "$Gg"); sb.Replace(" MX,1,", "$Gh"); sb.Replace(" MX,2,", "$Gi");
                sb.Replace(" MY,0,", "$Gj"); sb.Replace(" MY,1,", "$Gk"); sb.Replace(" MY,2,", "$Gl");
                sb.Replace(" P,0,", "$Gm"); sb.Replace(" P,1,", "$Gn"); sb.Replace(" P,2,", "$Go");
                sb.Replace(" R,0,", "$Gp"); sb.Replace(" R,1,", "$Gq"); sb.Replace(" R,2,", "$Gr");
                sb.Replace(" S,0,", "$Gs"); sb.Replace(" S,1,", "$Gt"); sb.Replace(" S,2,", "$Gu");
                sb.Replace(" V,0,", "$Gv"); sb.Replace(" V,1,", "$Gw"); sb.Replace(" V,2,", "$Gx");
                sb.Replace(" C,0,", "$Ha"); sb.Replace(" C,1,", "$Hb"); sb.Replace(" C,2,", "$Hc");
                sb.Replace(",320,240", "$v");
                sb.Replace("0,1", "$i");
                sb.Replace("1,0", "$j");
                sb.Insert(0, sb2.ToString());
            }
            GC.Collect();
            Finsh = true;
        }
        public static void 装箱(string[] @params, int currentLine)
        {
            if (@params[0].Length >= 2 && @params[0].Substring(0, 2) == "//") return;

            if (@params[0].Length >= 1)
            {
                if (@params[0].Substring(0, 1) == "[") return;
                if (@params[0].Substring(0, 1) == " ")
                {
                    string type = @params[0].Trim();
                    if (type.Length <= 2 && type != "")
                    {
                        int? tmpindex = null;
                        byte easing = 0;
                        int starttime = 0, endtime = 0;
                        try
                        {
                            if (type == "L")
                            {
                                isL = true; isT = false;
                                tmpindex = null;
                                starttime = int.Parse(@params[1]);
                            }
                            else if (type != "L" && type != "T" && @params.Length > 4)
                            {
                                if (@params[0].Length - @params[0].Trim().Length == 1) { tmpindex = null; isT = false; isL = false; }
                                else if (@params[0].Length - @params[0].Trim().Length == 2)
                                {
                                    if (isL) tmpindex = obj.Loop.Count - 1;
                                    else if (isT) tmpindex = obj.Trigger.Count - 1;
                                    else throw new Exception("垃圾逻辑毁我青春");
                                }
                                easing = byte.Parse(@params[1]);
                                starttime = int.Parse(@params[2]);
                                if (@params[3] == "") @params[3] = @params[2];
                                endtime = int.Parse(@params[3]);
                            }
                            else if (type == "T")
                            {
                                isT = true; isL = false;
                                tmpindex = null;

                                starttime = int.Parse(@params[2]);
                                endtime = int.Parse(@params[3]);
                            }
                            if (type == "M") AddRules.AddToM(easing, starttime, endtime, @params, tmpindex);
                            else if (type == "S") AddRules.AddToS(easing, starttime, endtime, @params, tmpindex);
                            else if (type == "F") AddRules.AddToF(easing, starttime, endtime, @params, tmpindex);
                            else if (type == "R") AddRules.AddToR(easing, starttime, endtime, @params, tmpindex);
                            else if (type == "V") AddRules.AddToV(easing, starttime, endtime, @params, tmpindex);
                            else if (type == "C") AddRules.AddToC(easing, starttime, endtime, @params, tmpindex);
                            else if (type == "MX") AddRules.AddToMX(easing, starttime, endtime, @params, tmpindex);
                            else if (type == "MY") AddRules.AddToMY(easing, starttime, endtime, @params, tmpindex);
                            else if (type == "P") AddRules.AddToP(easing, starttime, endtime, @params, tmpindex);
                            else if (type == "L") AddRules.AddToL(starttime, @params);
                            else if (type == "T") AddRules.AddToT(@params, starttime, endtime);
                            else
                            {
                                Exception(exMessage: "Unknown action command in Sprite[" + CurrentObjLine + "]: " + type);
                                //errnum++;
                            }
                        }
                        catch (Exception ex)
                        {
                            Exception(ex: ex);

                            //errnum++;
                            //Console.WriteLine("{0} - Line {1}", ex.Message, currentLine);
                        }
                    }
                    else
                    {
                        Exception(exMessage: "Unknown action command in Sprite[" + CurrentObjLine + "]: " + type);
                    }
                }
                else
                {
                    if (@params[0] == "Sprite")
                    {
                        isT = false; isL = false;
                        WaitToCompare();

                        string type = @params[0];
                        string layer = @params[1];
                        string origin = @params[2];
                        string root = @params[3];
                        double x = double.Parse(@params[4]);
                        double y = double.Parse(@params[5]);
                        obj = new SBObject(type, layer, origin, root, x, y, currentLine);
                    }
                    else if (@params[0] == "Animation")
                    {
                        isT = false; isL = false;
                        WaitToCompare();

                        string type = @params[0];
                        string layer = @params[1];
                        string origin = @params[2];
                        string root = @params[3];
                        double x = double.Parse(@params[4]);
                        double y = double.Parse(@params[5]);
                        double framecount = double.Parse(@params[6]);
                        double framerate = double.Parse(@params[7]);
                        string looptype = @params[8];
                        obj = new SBObject(type, layer, origin, root, x, y, framecount, framerate, looptype, currentLine);
                    }
                    else
                    {
                        Exception(exMessage: "Unknown sprite command: " + @params[0].Trim());
                    }
                    //Err(); //Console.WriteLine("可能是写了假sb. (Line {0})", currentLine);
                }
            }

        }
        static void WaitToCompare()
        {
            if (obj != null) //此为读完一个object
            {
                currentObjLine = obj.Line;
                Reporter.TotalSpriteNumber++;
                Variables.AddRoot(obj.FilePath);
                before = obj.ToString();

                bool IsOK = ChkRules.Check(obj);
                if (ifCheck && !IsOK)
                {
                    Exception(exMessage: "Exist illogical, conflicting or obsolete commands.", isobject: true);
                    after = "IsError";
                    if (ifPause3)
                    {
                        Pause = true; Display = true;
                        while (Pause)
                        {
                            System.Threading.Thread.Sleep(10);
                        }
                        Display = false;
                    }
                    else sb.Append(before);
                }
                else
                {
                    obj.Optimize();
                    after = obj.ToString();
                    //ifPause2 = obj.UnusefulObj;
                    if (ifPause) //如果开了确认
                    {
                        //ifPause = true;
                        if (before != after)
                        {
                            Pause = true; Display = true;
                        }
                        else sb.Append(after);

                        while (Pause)
                        {
                            System.Threading.Thread.Sleep(10);
                        }
                        Display = false;
                    }

                    else if (ifPause2)
                    {
                        if (before != after && after != "")
                        {
                            sb.Append(after);
                            Reporter.OptimizedSpriteNumber++;
                        }
                        else if (after == "")
                        {
                            Pause = true; Display = true;
                        }
                        else sb.Append(after);

                        while (Pause)
                        {
                            System.Threading.Thread.Sleep(10);
                        }
                        Display = false;
                    }
                    else
                    {
                        sb.Append(after);
                        if (before != after) Reporter.OptimizedSpriteNumber++;
                    }
                }
                before = ""; after = "";
                //Console.WriteLine(obj.ToString());
                obj = null;

            }
        }
        static class AddRules
        {
            public static double Round(double x)
            {
                double result = Math.Round(x, Decimal);
                return result;
            }
            public static void AddToM(byte easing, int starttime, int endtime, string[] @params, int? loopindex = null)
            {
                double X1 = Round(double.Parse(@params[4]));
                double Y1 = Round(double.Parse(@params[5]));
                double X2, Y2;
                if (@params.Length > 6)
                {
                    X2 = Round(double.Parse(@params[6]));
                    Y2 = Round(double.Parse(@params[7]));
                }
                else { X2 = X1; Y2 = Y1; }

                if (loopindex == null) obj.Move.Add(easing, starttime, endtime, X1, Y1, X2, Y2);
                else
                {
                    if (isL) obj.Loop[(int)loopindex].Move.Add(easing, starttime, endtime, X1, Y1, X2, Y2);
                    else if (isT) obj.Trigger[(int)loopindex].Move.Add(easing, starttime, endtime, X1, Y1, X2, Y2);
                }
            }
            public static void AddToV(byte easing, int starttime, int endtime, string[] @params, int? loopindex = null)
            {
                double VX1 = Round(double.Parse(@params[4]));
                double VY1 = Round(double.Parse(@params[5]));
                double VX2, VY2;
                if (@params.Length > 6)
                {
                    VX2 = Round(double.Parse(@params[6]));
                    VY2 = Round(double.Parse(@params[7]));
                }
                else { VX2 = VX1; VY2 = VY1; }
                if (loopindex == null) obj.Vector.Add(easing, starttime, endtime, VX1, VY1, VX2, VY2);
                else
                {
                    if (isL) obj.Loop[(int)loopindex].Vector.Add(easing, starttime, endtime, VX1, VY1, VX2, VY2);
                    else if (isT) obj.Trigger[(int)loopindex].Vector.Add(easing, starttime, endtime, VX1, VY1, VX2, VY2);
                }
            }
            public static void AddToS(byte easing, int starttime, int endtime, string[] @params, int? loopindex = null)
            {
                double S1 = Round(double.Parse(@params[4]));
                double S2;
                if (@params.Length > 5)
                {
                    S2 = Round(double.Parse(@params[5]));
                }
                else { S2 = S1; }
                if (loopindex == null) obj.Scale.Add(easing, starttime, endtime, S1, S2);
                else
                {
                    if (isL) obj.Loop[(int)loopindex].Scale.Add(easing, starttime, endtime, S1, S2);
                    else if (isT) obj.Trigger[(int)loopindex].Scale.Add(easing, starttime, endtime, S1, S2);
                }
            }
            public static void AddToF(byte easing, int starttime, int endtime, string[] @params, int? loopindex = null)
            {
                double F1 = Round(double.Parse(@params[4]));
                double F2;
                if (@params.Length > 5)
                {
                    F2 = Round(double.Parse(@params[5]));
                }
                else { F2 = F1; }
                if (loopindex == null) obj.Fade.Add(easing, starttime, endtime, F1, F2);
                else
                {
                    if (isL) obj.Loop[(int)loopindex].Fade.Add(easing, starttime, endtime, F1, F2);
                    else if (isT) obj.Trigger[(int)loopindex].Fade.Add(easing, starttime, endtime, F1, F2);
                }
            }
            public static void AddToR(byte easing, int starttime, int endtime, string[] @params, int? loopindex = null)
            {
                double R1 = Round(double.Parse(@params[4]));
                double R2;
                if (@params.Length > 5)
                {
                    R2 = Round(double.Parse(@params[5]));
                }
                else { R2 = R1; }
                if (loopindex == null) obj.Rotate.Add(easing, starttime, endtime, R1, R2);
                else
                {
                    if (isL) obj.Loop[(int)loopindex].Rotate.Add(easing, starttime, endtime, R1, R2);
                    else if (isT) obj.Trigger[(int)loopindex].Rotate.Add(easing, starttime, endtime, R1, R2);
                }
            }
            public static void AddToC(byte easing, int starttime, int endtime, string[] @params, int? loopindex = null)
            {
                byte R1 = byte.Parse(@params[4]);
                byte G1 = byte.Parse(@params[5]);
                byte B1 = byte.Parse(@params[6]);
                byte R2, G2, B2;
                if (@params.Length > 7)
                {
                    R2 = byte.Parse(@params[7]);
                    G2 = byte.Parse(@params[8]);
                    B2 = byte.Parse(@params[9]);
                }
                else { R2 = R1; G2 = G1; B2 = B1; }
                if (loopindex == null) obj.Color.Add(easing, starttime, endtime, R1, G1, B1, R2, G2, B2);
                else
                {
                    if (isL) obj.Loop[(int)loopindex].Color.Add(easing, starttime, endtime, R1, G1, B1, R2, G2, B2);
                    else if (isT) obj.Trigger[(int)loopindex].Color.Add(easing, starttime, endtime, R1, G1, B1, R2, G2, B2);
                }
            }
            public static void AddToMX(byte easing, int starttime, int endtime, string[] @params, int? loopindex = null)
            {
                double MX1 = Round(double.Parse(@params[4]));
                double MX2;
                if (@params.Length > 5)
                {
                    MX2 = Round(double.Parse(@params[5]));
                }
                else { MX2 = MX1; }
                if (loopindex == null) obj.MoveX.Add(easing, starttime, endtime, MX1, MX2);
                else
                {
                    if (isL) obj.Loop[(int)loopindex].MoveX.Add(easing, starttime, endtime, MX1, MX2);
                    else if (isT) obj.Trigger[(int)loopindex].MoveX.Add(easing, starttime, endtime, MX1, MX2);
                }
            }
            public static void AddToMY(byte easing, int starttime, int endtime, string[] @params, int? loopindex = null)
            {
                double MY1 = Round(double.Parse(@params[4]));
                double MY2;
                if (@params.Length > 5)
                {
                    MY2 = Round(double.Parse(@params[5]));
                }
                else { MY2 = MY1; }
                if (loopindex == null) obj.MoveY.Add(easing, starttime, endtime, MY1, MY2);
                else
                {
                    if (isL) obj.Loop[(int)loopindex].MoveY.Add(easing, starttime, endtime, MY1, MY2);
                    else if (isT) obj.Trigger[(int)loopindex].MoveY.Add(easing, starttime, endtime, MY1, MY2);
                }
            }
            public static void AddToP(byte easing, int starttime, int endtime, string[] @params, int? loopindex = null)
            {
                string P = @params[4];

                if (loopindex == null) obj.Parameter.Add(easing, starttime, endtime, P);
                else
                {
                    if (isL) obj.Loop[(int)loopindex].Parameter.Add(easing, starttime, endtime, P);
                    else if (isT) obj.Trigger[(int)loopindex].Parameter.Add(easing, starttime, endtime, P);
                }
            }
            public static void AddToL(int starttime, string[] @params)
            {
                int times = int.Parse(@params[2]);

                obj.Loop.Add(starttime, times);
            }
            public static void AddToT(string[] @params, int starttime, int endtime)
            {
                string triggertype = @params[1];

                obj.Trigger.Add(triggertype, starttime, endtime);
                //Debug.WriteLine(currentLine);
            }

        }

    }
}
