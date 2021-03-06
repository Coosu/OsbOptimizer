﻿using LibOSB.ActionTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibOSB
{
    /// <summary>
    /// Base class of storyboard objects.
    /// </summary>
    partial class SBObject
    {
        StringBuilder sb = new StringBuilder();

        private bool unusefulObj = false;
        private bool issueObj = false;

        private static bool autooptimize = false; 
        public static bool AutoOptimize { get => autooptimize; set => autooptimize = value; }

        private string type;
        private string layer;
        private string origin;
        private string filepath;
        private double x, y;
        private double? framecount, framerate;
        private string looptype;
        private int line;

        #region 一堆属性
        /// <summary>
        /// 获取物件在源文本的行数。
        /// </summary>
        public int Line { get => line; }
        /// <summary>
        /// 获取物件的种类。
        /// </summary>
        public string Type { get => type; }
        /// <summary>
        /// 获取物件的图层。
        /// </summary>
        public string Layer { get => layer; }
        /// <summary>
        /// 获取物件的原点位置。
        /// </summary>
        public string Origin { get => origin; }
        /// <summary>
        /// 获取物件的相对路径。
        /// </summary>
        public string FilePath { get => filepath; }
        /// <summary>
        /// 获取物件的默认y坐标。
        /// </summary>
        public double Y { get => y; set => y = value; }
        /// <summary>
        /// 获取物件的默认x坐标。
        /// </summary>
        public double X { get => x; set => x = value; }
        public string Looptype { get => looptype; }
        public double? Framecount { get => framecount; }
        public double? Framerate { get => framerate; }
        #endregion

        /// <summary>
        /// 获取当前所有动作的最大时间。
        /// </summary>

        private int? tmpMaxTime;
        private int? tmpMinTime;
        public int? TmpMaxTime { get => tmpMaxTime; set => tmpMaxTime = value; }
        public int? TmpMinTime { get => tmpMinTime; set => tmpMinTime = value; }

        protected bool TwoMin
        {
            get
            {
                //List<int?> min1 = min.FindAll(
                //    delegate (int? x)
                //    {
                //        return x == min.Min();
                //    }
                //    );
                //if (min1.Count > 1) return true;
                //else return false;
                return min.FindAll((int? x) => x == min.Min()).Count > 1;
            }
        }
        protected bool TwoMax
        {
            get
            {
                //List<int?> max1 = max.FindAll(
                //    delegate (int? x)
                //    {
                //        return x == max.Max();
                //    }
                //    );
                //if (max1.Count > 1) return true;
                //else return false;
                return max.FindAll((int? x) => x == max.Max()).Count > 1;
            }
        }

        private List<int?> max = new List<int?>();
        private List<int?> min = new List<int?>();

        public int? MaxTime()
        {

            {
                if (tmpMaxTime != null) return tmpMaxTime; //缓存

                max.Clear();
                if (Scale.TmpMaxTime != null) max.Add(Scale.TmpMaxTime);
                else if (Scale.MaxTime() != null) max.Add(Scale.MaxTime());

                if (Move.TmpMaxTime != null) max.Add(Move.TmpMaxTime);
                else if (Move.MaxTime() != null) max.Add(Move.MaxTime());

                if (Fade.TmpMaxTime != null) max.Add(Fade.TmpMaxTime);
                else if (Fade.MaxTime() != null) max.Add(Fade.MaxTime());

                if (Rotate.TmpMaxTime != null) max.Add(Rotate.TmpMaxTime);
                else if (Rotate.MaxTime() != null) max.Add(Rotate.MaxTime());

                if (Vector.TmpMaxTime != null) max.Add(Vector.TmpMaxTime);
                else if (Vector.MaxTime() != null) max.Add(Vector.MaxTime());

                if (Color.TmpMaxTime != null) max.Add(Color.TmpMaxTime);
                else if (Color.MaxTime() != null) max.Add(Color.MaxTime());

                if (MoveX.TmpMaxTime != null) max.Add(MoveX.TmpMaxTime);
                else if (MoveX.MaxTime() != null) max.Add(MoveX.MaxTime());

                if (MoveY.TmpMaxTime != null) max.Add(MoveY.TmpMaxTime);
                else if (MoveY.MaxTime() != null) max.Add(MoveY.MaxTime());

                if (Parameter.TmpMaxTime != null) max.Add(Parameter.TmpMaxTime);
                else if (Parameter.MaxTime() != null) max.Add(Parameter.MaxTime());

                for (int gg = 0; gg < Loop.Count; gg++)
                {
                    if (Loop[gg].TmpMaxTime != null) max.Add(Loop[gg].StartTime + (Loop[gg].TmpMaxTime * Loop[gg].Times));
                    else if (Loop[gg].MaxTime() != null) max.Add(Loop[gg].StartTime + (Loop[gg].MaxTime() * Loop[gg].Times));
                }

                for (int gg = 0; gg < Trigger.Count; gg++)
                {
                    if (Trigger[gg].TmpMaxTime != null) max.Add(Trigger[gg].StartTime + Trigger[gg].TmpMaxTime);
                    else if (Trigger[gg].MaxTime() != null) max.Add(Trigger[gg].StartTime + Trigger[gg].MaxTime());
                }

                if (max.Count < 1) return null;

                TmpMaxTime = max.Max();
                return max.Max();
            }
        }
        public void ToNull()
        {
            TmpMaxTime = null; TmpMinTime = null;
        }
        /// <summary>
        /// 获取当前所有动作的最小时间。
        /// </summary>
        public int? MinTime()
        {

            {
                if (TmpMinTime != null) return TmpMinTime; //缓存

                min.Clear();
                if (Scale.TmpMinTime != null) min.Add(Scale.TmpMinTime);
                else if (Scale.MinTime() != null) min.Add(Scale.MinTime());

                if (Move.TmpMinTime != null) min.Add(Move.TmpMinTime);
                else if (Move.MinTime() != null) min.Add(Move.MinTime());

                if (Fade.TmpMinTime != null) min.Add(Fade.TmpMinTime);
                else if (Fade.MinTime() != null) min.Add(Fade.MinTime());

                if (Rotate.TmpMinTime != null) min.Add(Rotate.TmpMinTime);
                else if (Rotate.MinTime() != null) min.Add(Rotate.MinTime());

                if (Vector.TmpMinTime != null) min.Add(Vector.TmpMinTime);
                else if (Vector.MinTime() != null) min.Add(Vector.MinTime());

                if (Color.TmpMinTime != null) min.Add(Color.TmpMinTime);
                else if (Color.MinTime() != null) min.Add(Color.MinTime());

                if (MoveX.TmpMinTime != null) min.Add(MoveX.TmpMinTime);
                else if (MoveX.MinTime() != null) min.Add(MoveX.MinTime());

                if (MoveY.TmpMinTime != null) min.Add(MoveY.TmpMinTime);
                else if (MoveY.MinTime() != null) min.Add(MoveY.MinTime());

                if (Parameter.TmpMinTime != null) min.Add(Parameter.TmpMinTime);
                else if (Parameter.MinTime() != null) min.Add(Parameter.MinTime());

                for (int gg = 0; gg < Loop.Count; gg++)
                {
                    if (Loop[gg].TmpMinTime != null) max.Add(Loop[gg].StartTime + Loop[gg].TmpMinTime);
                    else if (Loop[gg].MinTime() != null) max.Add(Loop[gg].StartTime + Loop[gg].MinTime());
                }
                for (int gg = 0; gg < Trigger.Count; gg++)
                {
                    if (Trigger[gg].TmpMinTime != null) max.Add(Trigger[gg].StartTime);
                    else if (Trigger[gg].MinTime() != null) max.Add(Trigger[gg].StartTime);
                }
                if (min.Count < 1) return null;

                TmpMinTime = min.Min();
                return min.Min();
            }
        }

        new public string ToString()
        {
            if (unusefulObj == true) return "";

            //if (AutoOptimize == false) Optimize();
            sb.Clear();
            sb.Append(Type); sb.Append(",");
            sb.Append(Layer); sb.Append(",");
            sb.Append(Origin); sb.Append(",");
            sb.Append(FilePath); sb.Append(",");
            sb.Append(X); sb.Append(",");
            sb.Append(Y.ToString());
            if (Framecount != null)
            {
                sb.Append(",");
                sb.Append(Framecount); sb.Append(",");
                sb.Append(Framerate); sb.Append(",");
                sb.Append(Looptype);
            }
            sb.AppendLine();
            for (int i = 1; i <= Move.Count; i++) sb.AppendLine(Move[i - 1].ToString());
            for (int i = 1; i <= Scale.Count; i++) sb.AppendLine(Scale[i - 1].ToString());
            for (int i = 1; i <= Fade.Count; i++) sb.AppendLine(Fade[i - 1].ToString());
            for (int i = 1; i <= Rotate.Count; i++) sb.AppendLine(Rotate[i - 1].ToString());
            for (int i = 1; i <= Vector.Count; i++) sb.AppendLine(Vector[i - 1].ToString());
            for (int i = 1; i <= Color.Count; i++) sb.AppendLine(Color[i - 1].ToString());
            for (int i = 1; i <= MoveX.Count; i++) sb.AppendLine(MoveX[i - 1].ToString());
            for (int i = 1; i <= MoveY.Count; i++) sb.AppendLine(MoveY[i - 1].ToString());
            for (int i = 1; i <= Parameter.Count; i++) sb.AppendLine(Parameter[i - 1].ToString());
            for (int i = 1; i <= Loop.Count; i++)
            {
                sb.AppendLine(Loop[i - 1].ToString());
                for (int j = 1; j <= Loop[i - 1].Move.Count; j++) sb.AppendLine(Loop[i - 1].Move[j - 1].ToString());
                for (int j = 1; j <= Loop[i - 1].Scale.Count; j++) sb.AppendLine(Loop[i - 1].Scale[j - 1].ToString());
                for (int j = 1; j <= Loop[i - 1].Fade.Count; j++) sb.AppendLine(Loop[i - 1].Fade[j - 1].ToString());
                for (int j = 1; j <= Loop[i - 1].Rotate.Count; j++) sb.AppendLine(Loop[i - 1].Rotate[j - 1].ToString());
                for (int j = 1; j <= Loop[i - 1].Vector.Count; j++) sb.AppendLine(Loop[i - 1].Vector[j - 1].ToString());
                for (int j = 1; j <= Loop[i - 1].Color.Count; j++) sb.AppendLine(Loop[i - 1].Color[j - 1].ToString());
                for (int j = 1; j <= Loop[i - 1].MoveX.Count; j++) sb.AppendLine(Loop[i - 1].MoveX[j - 1].ToString());
                for (int j = 1; j <= Loop[i - 1].MoveY.Count; j++) sb.AppendLine(Loop[i - 1].MoveY[j - 1].ToString());
                for (int j = 1; j <= Loop[i - 1].Parameter.Count; j++) sb.AppendLine(Loop[i - 1].Parameter[j - 1].ToString());
            }
            for (int i = 1; i <= Trigger.Count; i++)
            {
                sb.AppendLine(Trigger[i - 1].ToString());
                for (int j = 1; j <= Trigger[i - 1].Move.Count; j++) sb.AppendLine(Trigger[i - 1].Move[j - 1].ToString());
                for (int j = 1; j <= Trigger[i - 1].Scale.Count; j++) sb.AppendLine(Trigger[i - 1].Scale[j - 1].ToString());
                for (int j = 1; j <= Trigger[i - 1].Fade.Count; j++) sb.AppendLine(Trigger[i - 1].Fade[j - 1].ToString());
                for (int j = 1; j <= Trigger[i - 1].Rotate.Count; j++) sb.AppendLine(Trigger[i - 1].Rotate[j - 1].ToString());
                for (int j = 1; j <= Trigger[i - 1].Vector.Count; j++) sb.AppendLine(Trigger[i - 1].Vector[j - 1].ToString());
                for (int j = 1; j <= Trigger[i - 1].Color.Count; j++) sb.AppendLine(Trigger[i - 1].Color[j - 1].ToString());
                for (int j = 1; j <= Trigger[i - 1].MoveX.Count; j++) sb.AppendLine(Trigger[i - 1].MoveX[j - 1].ToString());
                for (int j = 1; j <= Trigger[i - 1].MoveY.Count; j++) sb.AppendLine(Trigger[i - 1].MoveY[j - 1].ToString());
                for (int j = 1; j <= Trigger[i - 1].Parameter.Count; j++) sb.AppendLine(Trigger[i - 1].Parameter[j - 1].ToString());

            }
            return sb.ToString();
        }
        /// <summary>
        /// 创建一个Storyboard物件的实例。
        /// </summary>
        public SBObject(string Type, string Layer, string Origin, string FilePath, double X, double Y, int line)
        {
            if (Type != "Sprite" && Type != "Animation")
                throw new Exception("Unknown object type.");
            else
            {
                //if (Type == "Sprite") type = "$a";
                //else if (Type == "Animation") type = "$b";
                type = Type;
            }
            if (Layer != "Background" && Layer != "Fail"
                     && Layer != "Pass" && Layer != "Foreground")
                throw new Exception("Unknown Layer.");
            else
            {
                //if (Layer == "Background") layer = "$c";
                //else if (Layer == "Fail") layer = "$e";
                //else if (Layer == "Pass") layer = "$f";
                //else if (Layer == "Foreground") layer = "$d";
                layer = Layer;
            }
            if (Origin != "TopLeft" && Origin != "TopCentre" && Origin != "TopRight"
             && Origin != "CentreLeft" && Origin != "Centre" && Origin != "CentreRight"
             && Origin != "BottomLeft" && Origin != "BottomCentre" && Origin != "BottomRight")
                throw new Exception("Unknown Origin.");
            else
            {
                //if (Origin == "TopLeft") origin = "$7";
                //else if (Origin == "TopCentre") origin = "$8";
                //else if (Origin == "TopRight") origin = "$9";
                //else if (Origin == "CentreLeft") origin = "$4";
                //else if (Origin == "Centre") origin = "$5";
                //else if (Origin == "CentreRight") origin = "$6";
                //else if (Origin == "BottomLeft") origin = "$1";
                //else if (Origin == "BottomCentre") origin = "$2";
                //else if (Origin == "BottomRight") origin = "$3";

                origin = Origin;
            }
            filepath = FilePath;
            x = X;
            y = Y;
            this.line = line;

            Move = new Move();

            MoveX = new MoveX();
            MoveY = new MoveY();
            Scale = new Scale();
            Fade = new Fade();
            Rotate = new Rotate();
            Vector = new Vector();
            Color = new Color();
            Parameter = new Parameter();
            Loop = new Loop();
            Trigger = new Trigger();
        }
        public SBObject(string Type, string Layer, string Origin, string FilePath, double X, double Y, double FrameCount, double FrameRate, string LoopType, int line)
        {
            if (Type != "Sprite" && Type != "Animation")
                throw new Exception("Unknown object type.");
            else type = Type;

            if (Layer != "Background" && Layer != "Fail"
                     && Layer != "Pass" && Layer != "Foreground")
                throw new Exception("Unknown Layer.");
            else layer = Layer;

            if (Origin != "TopLeft" && Origin != "TopCentre" && Origin != "TopRight"
             && Origin != "CentreLeft" && Origin != "Centre" && Origin != "CentreRight"
             && Origin != "BottomLeft" && Origin != "BottomCentre" && Origin != "BottomRight")
                throw new Exception("Unknown Origin.");
            else origin = Origin;

            filepath = FilePath;
            x = X;
            y = Y;

            framecount = FrameCount;
            framerate = FrameRate;
            looptype = LoopType;

            this.line = line;

            Move = new Move();

            MoveX = new MoveX();
            MoveY = new MoveY();
            Scale = new Scale();
            Fade = new Fade();
            Rotate = new Rotate();
            Vector = new Vector();
            Color = new Color();
            Parameter = new Parameter();
            Loop = new Loop();
            Trigger = new Trigger();
        }
        /// <summary>
        /// An action that controls the object to move. 
        /// </summary>
        public Move Move { set; get; }
        /// <summary>
        /// An action that controls the object to zoom. 
        /// </summary>
        public Scale Scale { set; get; }
        /// <summary>
        /// An action that controls the object to change the transparency. 
        /// </summary>
        public Fade Fade { set; get; }
        /// <summary>
        /// An action that controls the object to change the degree. 
        /// </summary>
        public Rotate Rotate { set; get; }
        /// <summary>
        /// An action that controls the object to zoom the width and height dividually. 
        /// </summary>
        public Vector Vector { set; get; }
        /// <summary>
        /// An action that controls the object to have addtional color. 
        /// </summary>
        public Color Color { set; get; }
        public MoveX MoveX { set; get; }
        public MoveY MoveY { set; get; }
        public Parameter Parameter { set; get; }
        public Loop Loop { set; get; }
        public Trigger Trigger { set; get; }

        public bool UnusefulObj { get => unusefulObj; }

        public bool IssueObj { get => issueObj; set => issueObj = value; }
    }

}
