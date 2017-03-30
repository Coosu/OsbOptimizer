using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibOSB
{
    partial class SBObject
    {
        private static bool ifDeep = true;

        public static bool IfDeep { get => ifDeep; set => ifDeep = value; }

        public void Optimize()
        {
            if (Fade.Count > 0) optF(); if (unusefulObj) return;
            if (Move.Count > 0) optM();
            if (Scale.Count > 0) optS();
            if (Rotate.Count > 0) optR();
            if (Vector.Count > 0) optV();
            if (Color.Count > 0) optC();
            if (MoveX.Count > 0) optMX();
            if (MoveY.Count > 0) optMY();
            for (int gg = 0; gg < Loop.Count; gg++)
            {
                if (Loop[gg].Move.Count > 0) optM_L(gg);
                if (Loop[gg].Fade.Count > 0) optF_L(gg);
                if (Loop[gg].Scale.Count > 0) optS_L(gg);
                if (Loop[gg].Rotate.Count > 0) optR_L(gg);
                if (Loop[gg].Vector.Count > 0) optV_L(gg);
                if (Loop[gg].Color.Count > 0) optC_L(gg);
                if (Loop[gg].MoveX.Count > 0) optMX();
                if (Loop[gg].MoveY.Count > 0) optMY();
            }
            for (int gg = 0; gg < Trigger.Count; gg++)
            {
                if (Trigger[gg].Move.Count > 0) optM_T(gg);
                if (Trigger[gg].Fade.Count > 0) optF_T(gg);
                if (Trigger[gg].Scale.Count > 0) optS_T(gg);
                if (Trigger[gg].Rotate.Count > 0) optR_T(gg);
                if (Trigger[gg].Vector.Count > 0) optV_T(gg);
                if (Trigger[gg].Color.Count > 0) optC_T(gg);
                if (Trigger[gg].MoveX.Count > 0) optMX();
                if (Trigger[gg].MoveY.Count > 0) optMY();
            }
        }
        private void optM()
        {
            if (IfDeep)
            {
                //根据是否显示判定是否有效
                int tmpindex = 0;
                for (int k = 0; k < Fade.FadeOutList.Count; k++)
                {
                    for (int j = tmpindex; j < Move.Count; j++)
                    {
                        if (Move[j].StartTime > Fade.FadeOutList[k].EndTime)
                        {
                            tmpindex = j;
                            break;
                        }
                        if (j < Move.Count - 1 &&
                          Move[j].StartTime >= Fade.FadeOutList[k].StartTime && Move[j].EndTime <= Fade.FadeOutList[k].EndTime &&
                          Move[j + 1].StartTime <= Fade.FadeOutList[k].EndTime)
                        {
                            Move.Remove(j); Move.ToNull(); ToNull();
                            j--;
                        }
                        else if (j == Move.Count - 1 &&
                           Move[j].StartTime >= Fade.FadeOutList[k].StartTime && Move[j].EndTime < Fade.FadeOutList[k].EndTime &&
                          Fade.FadeOutList[k].EndTime >= MaxTime())
                        {
                            Move.Remove(j); Move.ToNull(); ToNull();
                            j--;
                        }
                    }
                }
                //
            }
            int i = Move.Count - 1;
            while (i >= 1)
            {
                int? max2 = MaxTime();
                int? min2 = MinTime();
                bool if2min2 = TwoMin;
                bool if2max2 = TwoMax;

                /* 当 这个M的结束时间要小于obj的最大时间(或者是有两个以上的最大时间，其中包括M的)
                 * 且 就是一个静止的动作
                 * 且 这个动作与上一个末动作一样     
                 */
                if (
                   (Move[i].EndTime < max2 || Move[i].EndTime == max2 && if2max2 == true) &&
                   Move[i].X1 == Move[i].X2 && Move[i].Y1 == Move[i].Y2 &&
                   Move[i].X1 == Move[i - 1].X2 && Move[i].Y1 == Move[i - 1].Y2)
                {
                    Move.Remove(i); Move.ToNull(); ToNull(); //删除这个m
                    i = Move.Count - 1;
                }
                /* 当 这个M与前面的M一致，又是单关键帧的，而且又不满足上面的（这里就是指小于最大时间的）
                 */
                else if (Move[i].X1 == Move[i].X2 && Move[i].Y1 == Move[i].Y2 &&
                  Move[i - 1].X1 == Move[i - 1].X2 && Move[i - 1].Y1 == Move[i - 1].Y2 &&
                  Move[i - 1].X1 == Move[i].X1 && Move[i - 1].Y1 == Move[i].Y1)
                {
                    Move[i - 1].EndTime = Move[i].EndTime; //整合到前面的
                    Move.MoveEndTime(i);
                    if (Move[i - 1].StartTime == min2 && if2min2 == true)
                    {
                        Move[i - 1].StartTime = Move[i - 1].EndTime; //整合到前面的
                        Move.MoveStartTime(i);
                    }
                    Move.Remove(i); Move.ToNull(); ToNull();//删除这个m
                    i = Move.Count - 1;
                }
                else i--;
            }

            /* 当 只有一个M的时候
             * 且 这个M的结束时间要小于obj的最大时间(或者是有两个以上的最大时间，其中包括M的)
             * 且 这个M的开始时间要大于obj的最小时间(或者是有两个以上的最小时间，其中包括M的)
             * 且 就是一个静止的物品             
             */
            int? max = MaxTime();
            int? min = MinTime();
            bool if2min = TwoMin;
            bool if2max = TwoMax;
            //Console.WriteLine(Move[0].EndTime < mx);
            if (Move.Count == 1 &&
                (Move[0].EndTime < max || Move[0].EndTime == max && if2max == true) &&
                (Move[0].StartTime > min || Move[0].StartTime == min && if2min == true) &&
                Move[0].X1 == Move[0].X2 && Move[0].Y1 == Move[0].Y2)
            {
                if (Move[0].X1 == (int)Move[0].X1 && Move[0].Y1 == (int)Move[0].Y1)
                {
                    X = Move[0].X1; Y = Move[0].Y1;
                    Move.Remove(0); Move.ToNull(); ToNull();//删除这个M 
                }
                else if (Move[0].X1 == X && Move[0].Y1 == Y)
                {
                    Move.Remove(0); Move.ToNull(); ToNull();//删除这个M 
                }
            }
            //加个条件 对第一行再判断。
            else if (Move.Count > 1 &&
                (Move[0].EndTime < max || Move[0].EndTime == max && if2max == true) &&
                (Move[0].StartTime > min || Move[0].StartTime == min && if2min == true) &&
                 Move[0].X2 == Move[0].X1 && Move[0].Y2 == Move[0].Y1 &&
                 Move[0].X2 == Move[1].X1 && Move[0].Y2 == Move[1].Y1)
            {
                if (Move[0].X1 == (int)Move[0].X1 && Move[0].Y1 == (int)Move[0].Y1)
                {
                    X = Move[0].X1; Y = Move[0].Y1;
                    Move.Remove(0); Move.ToNull(); ToNull();//删除这个M 
                }
                else if (Move[0].X1 == X && Move[0].Y1 == Y && X == (int)X)
                {
                    Move.Remove(0); Move.ToNull(); ToNull();//删除这个M 
                }
            }
        }
        private void optF()
        {
            if (IfDeep)
            {
                int? tmpstart = null;

                bool IsInFadeOut = false;
                if (Fade[0].F1 == 0 && Fade[0].StartTime > MinTime())
                {
                    tmpstart = MinTime();
                    IsInFadeOut = true;
                }
                for (int j = 0; j < Fade.Count; j++)
                {


                    if ((Fade[j].F1 != 0 || Fade[j].F2 != 0 ||
                        (Fade[j].EndTime == MaxTime() && Fade[j].F2 == 0)
                        ) && IsInFadeOut == true)
                    {
                        Fade.FadeOutList.Add(new ActionTypes.FadeOutTime(tmpstart, Fade[j].StartTime));
                        IsInFadeOut = false;
                    }
                    if (Fade[j].F2 == 0 && IsInFadeOut == false)
                    {
                        tmpstart = Fade[j].EndTime;
                        IsInFadeOut = true;
                    }
                }
                if (IsInFadeOut)
                {
                    Fade.FadeOutList.Add(new ActionTypes.FadeOutTime(tmpstart, MaxTime()));
                    //IsInFadeOut = true;
                }
            }
            ///
            int i = Fade.Count - 1;
            while (i >= 1)
            {
                int? max2 = MaxTime();
                int? min2 = MinTime();
                bool if2min2 = TwoMin;
                bool if2max2 = TwoMax;

                /* 当 这个F的结束时间要小于obj的最大时间(或者是有两个以上的最大时间，其中包括F的)
                 * 且 就是一个静止的动作
                 * 且 这个动作与上一个末动作一样     
                 */
                if (
                        (
                            (Fade[i].EndTime < this.MaxTime() || Fade[i].EndTime == max2 && if2max2 == true
                        )
                        || Fade[i].F1 == 0 // ==============F特有。==============
                   ) &&
                   Fade[i].F1 == Fade[i].F2 &&
                   Fade[i].F1 == Fade[i - 1].F2)
                {
                    Fade.Remove(i); Fade.ToNull(); ToNull();  //删除这个F
                    i = Fade.Count - 1;
                }
                /* 当 这个F与前面的F一致，又是单关键帧的，而且又不满足上面的（这里就是指小于最大时间的）
                 */
                else if (Fade[i].F1 == Fade[i].F2 &&
                  Fade[i - 1].F1 == Fade[i - 1].F2 &&
                  Fade[i - 1].F1 == Fade[i].F1)
                {
                    Fade[i - 1].EndTime = Fade[i].EndTime; //整合到前面的
                    Fade.MoveEndTime(i);
                    if (Fade[i - 1].StartTime == min2 && if2min2 == true)
                    {
                        Fade[i - 1].StartTime = Fade[i - 1].EndTime;
                        Fade.MoveStartTime(i);
                    }    //Fade[i - 1].StartTime = Fade[i - 1].EndTime;
                    Fade.Remove(i); Fade.ToNull(); ToNull(); //删除这个F
                    i = Fade.Count - 1;
                }
                else i--;
            }

            // ==============F特有。==============
            if (Fade.Count == 1 &&
                Fade[0].F1 == 0 && Fade[0].F2 == 0
                && Loop.Count == 0 && Trigger.Count == 0)
            {
                unusefulObj = true;
                return;
            }

            /* 当 只有一个F的时候
             * 且 这个F的结束时间要小于obj的最大时间(或者是有两个以上的最大时间，其中包括F的)
             * 且 这个F的开始时间要大于obj的最小时间(或者是有两个以上的最小时间，其中包括F的)
             * 且 就是一个静止的物品
             * 且 F就为1             
             */
            int? max = MaxTime();
            int? min = MinTime();
            bool if2min = TwoMin;
            bool if2max = TwoMax;
            //Console.WriteLine(Fade[0].EndTime < mx);
            if (Fade.Count == 1 &&
                (Fade[0].EndTime < max || Fade[0].EndTime == max && if2max == true) &&
                (Fade[0].StartTime > min || Fade[0].StartTime == min && if2min == true) &&
                Fade[0].F1 == Fade[0].F2 &&
                Fade[0].F1 == 1)
            {
                Fade.Remove(0); Fade.ToNull(); ToNull();
            }
            //加个条件 对第一行再判断。
            else if (Fade.Count > 1 &&
                (Fade[0].EndTime < max || Fade[0].EndTime == max && if2max == true) &&
                (Fade[0].StartTime > min || Fade[0].StartTime == min && if2min == true) &&
                Fade[0].F1 == Fade[0].F2 &&
                Fade[0].F2 == Fade[1].F1 &&
                Fade[0].F1 == 1)
            {
                Fade.Remove(0); Fade.ToNull(); ToNull();
            }
        }
        private void optS()
        {
            if (IfDeep)
            {     //根据是否显示判定是否有效
                int tmpindex = 0;
                for (int k = 0; k < Fade.FadeOutList.Count; k++)
                {
                    for (int j = tmpindex; j < Scale.Count; j++)
                    {
                        if (Scale[j].StartTime > Fade.FadeOutList[k].EndTime)
                        {
                            tmpindex = j;
                            break;
                        }
                        if (j < Scale.Count - 1 &&
                          Scale[j].StartTime >= Fade.FadeOutList[k].StartTime && Scale[j].EndTime <= Fade.FadeOutList[k].EndTime &&
                          Scale[j + 1].StartTime <= Fade.FadeOutList[k].EndTime)
                        {
                            Scale.Remove(j); Scale.ToNull(); ToNull();
                            j--;
                        }
                        else if (j == Scale.Count - 1 &&
                           Scale[j].StartTime >= Fade.FadeOutList[k].StartTime && Scale[j].EndTime < Fade.FadeOutList[k].EndTime &&
                          Fade.FadeOutList[k].EndTime >= MaxTime())
                        {
                            Scale.Remove(j); Scale.ToNull(); ToNull();
                            j--;
                        }
                    }
                }
                //
            }
            int i = Scale.Count - 1;
            while (i >= 1)
            {
                int? max2 = MaxTime();
                int? min2 = MinTime();
                bool if2min2 = TwoMin;
                bool if2max2 = TwoMax;

                /* 当 这个S的结束时间要小于obj的最大时间(或者是有两个以上的最大时间，其中包括S的)
                 * 且 就是一个静止的动作
                 * 且 这个动作与上一个末动作一样     
                 */
                if ((Scale[i].EndTime < this.MaxTime() || Scale[i].EndTime == max2 && if2max2 == true) &&
                   Scale[i].S1 == Scale[i].S2 &&
                   Scale[i].S1 == Scale[i - 1].S2)
                {
                    Scale.Remove(i); Scale.ToNull(); ToNull();  //删除这个S
                    i = Scale.Count - 1;
                }
                /* 当 这个S与前面的S一致，又是单关键帧的，而且又不满足上面的（这里就是指小于最大时间的）
                 */
                else if (Scale[i].S1 == Scale[i].S2 &&
                  Scale[i - 1].S1 == Scale[i - 1].S2 &&
                  Scale[i - 1].S1 == Scale[i].S1)
                {

                    Scale[i - 1].EndTime = Scale[i].EndTime; //整合到前面的
                    Scale.MoveEndTime(i);
                    if (Scale[i - 1].StartTime == min2 && if2min2 == true)
                    {
                        Scale[i - 1].StartTime = Scale[i - 1].EndTime;
                        Scale.MoveStartTime(i);
                    }
                    //Scale[i - 1].StartTime = Scale[i - 1].EndTime;
                    Scale.Remove(i); Scale.ToNull(); ToNull(); //删除这个S
                    i = Scale.Count - 1;
                }
                else i--;
            }

            /* 当 只有一个S的时候
             * 且 这个S的结束时间要小于obj的最大时间(或者是有两个以上的最大时间，其中包括S的)
             * 且 这个S的开始时间要大于obj的最小时间(或者是有两个以上的最小时间，其中包括S的)
             * 且 就是一个静止的物品
             * 且 S就为1             
             */
            int? max = MaxTime();
            int? min = MinTime();
            bool if2min = TwoMin;
            bool if2max = TwoMax;
            //Console.WriteLine(Scale[0].EndTime < mx);
            if (Scale.Count == 1 &&
                (Scale[0].EndTime < max || Scale[0].EndTime == max && if2max == true) &&
                (Scale[0].StartTime > min || Scale[0].StartTime == min && if2min == true) &&
                Scale[0].S1 == Scale[0].S2 &&
                Scale[0].S1 == 1)
            {
                Scale.Remove(0); Scale.ToNull(); ToNull();
            }
            //加个条件 对第一行再判断。
            else if (Scale.Count > 1 &&
                (Scale[0].EndTime < max || Scale[0].EndTime == max && if2max == true) &&
                (Scale[0].StartTime > min || Scale[0].StartTime == min && if2min == true) &&
                Scale[0].S1 == Scale[0].S2 &&
                Scale[0].S2 == Scale[1].S1 &&
                Scale[0].S1 == 1)
            {
                Scale.Remove(0); Scale.ToNull(); ToNull();
            }
        }
        private void optR()
        {
            if (IfDeep)
            {
                //根据是否显示判定是否有效
                int tmpindex = 0;
                for (int k = 0; k < Fade.FadeOutList.Count; k++)
                {
                    for (int j = tmpindex; j < Rotate.Count; j++)
                    {
                        if (Rotate[j].StartTime > Fade.FadeOutList[k].EndTime)
                        {
                            tmpindex = j;
                            break;
                        }
                        if (j < Rotate.Count - 1 &&
                          Rotate[j].StartTime >= Fade.FadeOutList[k].StartTime && Rotate[j].EndTime <= Fade.FadeOutList[k].EndTime &&
                          Rotate[j + 1].StartTime <= Fade.FadeOutList[k].EndTime)
                        {
                            Rotate.Remove(j); Rotate.ToNull(); ToNull();
                            j--;
                        }
                        else if (j == Rotate.Count - 1 &&
                           Rotate[j].StartTime >= Fade.FadeOutList[k].StartTime && Rotate[j].EndTime < Fade.FadeOutList[k].EndTime &&
                          Fade.FadeOutList[k].EndTime >= MaxTime())
                        {
                            Rotate.Remove(j); Rotate.ToNull(); ToNull();
                            j--;
                        }
                    }
                }
                //
            }
            int i = Rotate.Count - 1;
            while (i >= 1)
            {
                int? max2 = MaxTime();
                int? min2 = MinTime();
                bool if2min2 = TwoMin;
                bool if2max2 = TwoMax;

                /* 当 这个R的结束时间要小于obj的最大时间(或者是有两个以上的最大时间，其中包括R的)
                 * 且 就是一个静止的动作
                 * 且 这个动作与上一个末动作一样     
                 */
                if (
                   (Rotate[i].EndTime < this.MaxTime() || Rotate[i].EndTime == max2 && if2max2 == true) &&
                   Rotate[i].R1 == Rotate[i].R2 &&
                   Rotate[i].R1 == Rotate[i - 1].R2)
                {
                    Rotate.Remove(i); Rotate.ToNull(); ToNull();  //删除这个R
                    i = Rotate.Count - 1;
                }
                /* 当 这个R与前面的R一致，又是单关键帧的，而且又不满足上面的（这里就是指小于最大时间的）
                 */
                else if (Rotate[i].R1 == Rotate[i].R2 &&
                  Rotate[i - 1].R1 == Rotate[i - 1].R2 &&
                  Rotate[i - 1].R1 == Rotate[i].R1)
                {

                    Rotate[i - 1].EndTime = Rotate[i].EndTime; //整合到前面的
                    Rotate.MoveEndTime(i);
                    if (Rotate[i - 1].StartTime == min2 && if2min2 == true)
                    {
                        Rotate[i - 1].StartTime = Rotate[i - 1].EndTime;
                        Rotate.MoveStartTime(i);
                    }
                    //Rotate[i - 1].StartTime = Rotate[i - 1].EndTime;
                    Rotate.Remove(i); Rotate.ToNull(); ToNull(); //删除这个R
                    i = Rotate.Count - 1;
                }
                else i--;
            }

            /* 当 只有一个R的时候
             * 且 这个R的结束时间要小于obj的最大时间(或者是有两个以上的最大时间，其中包括R的)
             * 且 这个R的开始时间要大于obj的最小时间(或者是有两个以上的最小时间，其中包括R的)
             * 且 就是一个静止的物品
             * 且 R就为0                 
             */
            int? max = MaxTime();
            int? min = MinTime();
            bool if2min = TwoMin;
            bool if2max = TwoMax;
            //Console.WriteLine(Rotate[0].EndTime < mx);
            if (Rotate.Count == 1 &&
                (Rotate[0].EndTime < max || Rotate[0].EndTime == max && if2max == true) &&
                (Rotate[0].StartTime > min || Rotate[0].StartTime == min && if2min == true) &&
                Rotate[0].R1 == Rotate[0].R2 &&
                Rotate[0].R1 == 0)
            {
                Rotate.Remove(0); Rotate.ToNull(); ToNull();
            }
            //加个条件 对第一行再判断。
            else if (Rotate.Count > 1 &&
                (Rotate[0].EndTime < max || Rotate[0].EndTime == max && if2max == true) &&
                (Rotate[0].StartTime > min || Rotate[0].StartTime == min && if2min == true) &&
                Rotate[0].R1 == Rotate[0].R2 &&
                Rotate[0].R2 == Rotate[1].R1 &&
                Rotate[0].R1 == 0)
            {
                Rotate.Remove(0); Rotate.ToNull(); ToNull();
            }
        }
        private void optV()
        {
            if (IfDeep)
            {   //根据是否显示判定是否有效
                int tmpindex = 0;
                for (int k = 0; k < Fade.FadeOutList.Count; k++)
                {
                    for (int j = tmpindex; j < Vector.Count; j++)
                    {
                        if (Vector[j].StartTime > Fade.FadeOutList[k].EndTime)
                        {
                            tmpindex = j;
                            break;
                        }
                        if (j < Vector.Count - 1 &&
                          Vector[j].StartTime >= Fade.FadeOutList[k].StartTime && Vector[j].EndTime <= Fade.FadeOutList[k].EndTime &&
                          Vector[j + 1].StartTime <= Fade.FadeOutList[k].EndTime)
                        {
                            Vector.Remove(j); Vector.ToNull(); ToNull();
                            j--;
                        }
                        else if (j == Vector.Count - 1 &&
                           Vector[j].StartTime >= Fade.FadeOutList[k].StartTime && Vector[j].EndTime < Fade.FadeOutList[k].EndTime &&
                          Fade.FadeOutList[k].EndTime >= MaxTime())
                        {
                            Vector.Remove(j); Vector.ToNull(); ToNull();
                            j--;
                        }
                    }
                }
                //
            }
            int i = Vector.Count - 1;
            while (i >= 1)
            {
                int? max2 = MaxTime();
                int? min2 = MinTime();
                bool if2min2 = TwoMin;
                bool if2max2 = TwoMax;

                /* 当 这个V的结束时间要小于obj的最大时间(或者是有两个以上的最大时间，其中包括V的)
                 * 且 就是一个静止的动作
                 * 且 这个动作与上一个末动作一样     
                 */
                if (
                   (Vector[i].EndTime < max2 || Vector[i].EndTime == max2 && if2max2 == true) &&
                   Vector[i].VX1 == Vector[i].VX2 && Vector[i].VY1 == Vector[i].VY2 &&
                   Vector[i].VX1 == Vector[i - 1].VX2 && Vector[i].VY1 == Vector[i - 1].VY2)
                {
                    Vector.Remove(i); Vector.ToNull(); ToNull(); //删除这个V
                    i = Vector.Count - 1;
                }
                /* 当 这个V与前面的V一致，又是单关键帧的，而且又不满足上面的（这里就是指小于最大时间的）
                 */
                else if (Vector[i].VX1 == Vector[i].VX2 && Vector[i].VY1 == Vector[i].VY2 &&
                  Vector[i - 1].VX1 == Vector[i - 1].VX2 && Vector[i - 1].VY1 == Vector[i - 1].VY2 &&
                  Vector[i - 1].VX1 == Vector[i].VX1 && Vector[i - 1].VY1 == Vector[i].VY1)
                {
                    Vector[i - 1].EndTime = Vector[i].EndTime; //整合到前面的
                    Vector.MoveEndTime(i);
                    if (Vector[i - 1].StartTime == min2 && if2min2 == true)
                    {
                        Vector[i - 1].StartTime = Vector[i - 1].EndTime; //整合到前面的
                        Vector.MoveStartTime(i);
                    }
                    Vector.Remove(i); Vector.ToNull(); ToNull();//删除这个V
                    i = Vector.Count - 1;
                }
                else i--;
            }

            /* 当 只有一个V的时候
             * 且 这个V的结束时间要小于obj的最大时间(或者是有两个以上的最大时间，其中包括V的)
             * 且 这个V的开始时间要大于obj的最小时间(或者是有两个以上的最小时间，其中包括V的)
             * 且 就是一个静止的物品
             * 且 VX,VY都是1              
             */
            int? max = MaxTime();
            int? min = MinTime();
            bool if2min = TwoMin;
            bool if2max = TwoMax;
            //Console.WriteLine(Vector[0].EndTime < mx);
            if (Vector.Count == 1 &&
                (Vector[0].EndTime < max || Vector[0].EndTime == max && if2max == true) &&
                (Vector[0].StartTime > min || Vector[0].StartTime == min && if2min == true) &&
                Vector[0].VX1 == Vector[0].VX2 && Vector[0].VY1 == Vector[0].VY2 &&
                Vector[0].VX1 == 1 && Vector[0].VY1 == 1)
            {
                Vector.Remove(0); Vector.ToNull(); ToNull();//删除这个V
            }
            //加个条件 对第一行再判断。
            else if (Vector.Count > 1 &&
              (Vector[0].EndTime < max || Vector[0].EndTime == max && if2max == true) &&
              (Vector[0].StartTime > min || Vector[0].StartTime == min && if2min == true) &&
               Vector[0].VX2 == Vector[0].VX1 && Vector[0].VY2 == Vector[0].VY1 &&
               Vector[0].VX2 == Vector[1].VX1 && Vector[0].VY2 == Vector[1].VY1 &&
               Vector[0].VX1 == 1 && Vector[0].VY1 == 1)
            {
                Vector.Remove(0); Vector.ToNull(); ToNull();//删除这个V
            }
        }
        private void optC()
        {
            if (IfDeep)
            {  //根据是否显示判定是否有效
                int tmpindex = 0;
                for (int k = 0; k < Fade.FadeOutList.Count; k++)
                {
                    for (int j = tmpindex; j < Color.Count; j++)
                    {
                        if (Color[j].StartTime > Fade.FadeOutList[k].EndTime)
                        {
                            tmpindex = j;
                            break;
                        }
                        if (j < Color.Count - 1 &&
                          Color[j].StartTime >= Fade.FadeOutList[k].StartTime && Color[j].EndTime <= Fade.FadeOutList[k].EndTime &&
                          Color[j + 1].StartTime <= Fade.FadeOutList[k].EndTime)
                        {
                            Color.Remove(j); Color.ToNull(); ToNull();
                            j--;
                        }
                        else if (j == Color.Count - 1 &&
                           Color[j].StartTime >= Fade.FadeOutList[k].StartTime && Color[j].EndTime < Fade.FadeOutList[k].EndTime &&
                          Fade.FadeOutList[k].EndTime >= MaxTime())
                        {
                            Color.Remove(j); Color.ToNull(); ToNull();
                            j--;
                        }
                    }
                }
                //
            }
            int i = Color.Count - 1;
            while (i >= 1)
            {
                int? max2 = MaxTime();
                int? min2 = MinTime();
                bool if2min2 = TwoMin;
                bool if2max2 = TwoMax;

                /* 当 这个C的结束时间要小于obj的最大时间(或者是有两个以上的最大时间，其中包括C的)
                 * 且 就是一个静止的动作
                 * 且 这个动作与上一个末动作一样     
                 */
                if (
                   (Color[i].EndTime < max2 || Color[i].EndTime == max2 && if2max2 == true) &&
                   Color[i].R1 == Color[i].R2 && Color[i].G1 == Color[i].G2 && Color[i].B1 == Color[i].B2 &&
                   Color[i].R1 == Color[i - 1].R2 && Color[i].G1 == Color[i - 1].G2 && Color[i].B1 == Color[i - 1].B2)
                {
                    Color.Remove(i); Color.ToNull(); ToNull(); //删除这个C
                    i = Color.Count - 1;
                }
                /* 当 这个C与前面的C一致，又是单关键帧的，而且又不满足上面的（这里就是指小于最大时间的）
                 */
                else if (Color[i].R1 == Color[i].R2 && Color[i].G1 == Color[i].G2 && Color[i].B1 == Color[i].B2 &&
                  Color[i - 1].R1 == Color[i - 1].R2 && Color[i - 1].G1 == Color[i - 1].G2 && Color[i - 1].B1 == Color[i - 1].B2 &&
                  Color[i - 1].R1 == Color[i].R1 && Color[i - 1].G1 == Color[i].G1 && Color[i - 1].B1 == Color[i].B1)
                {
                    Color[i - 1].EndTime = Color[i].EndTime; //整合到前面的
                    Color.MoveEndTime(i);
                    if (Color[i - 1].StartTime == min2 && if2min2 == true)
                    {
                        Color[i - 1].StartTime = Color[i - 1].EndTime; //整合到前面的
                        Color.MoveStartTime(i);
                    }
                    Color.Remove(i); Color.ToNull(); ToNull();//删除这个C
                    i = Color.Count - 1;
                }
                else i--;
            }

            /* 当 只有一个C的时候
             * 且 这个C的结束时间要小于obj的最大时间(或者是有两个以上的最大时间，其中包括C的)
             * 且 这个C的开始时间要大于obj的最小时间(或者是有两个以上的最小时间，其中包括C的)
             * 且 就是一个静止的物品
             * 且 RGB=255,255,255             
             */
            int? max = MaxTime();
            int? min = MinTime();
            bool if2min = TwoMin;
            bool if2max = TwoMax;
            //Console.WriteLine(Color[0].EndTime < mx);
            if (Color.Count == 1 &&
                (Color[0].EndTime < max || Color[0].EndTime == max && if2max == true) &&
                (Color[0].StartTime > min || Color[0].StartTime == min && if2min == true) &&
                Color[0].R1 == Color[0].R2 && Color[0].G1 == Color[0].G2 && Color[0].B1 == Color[0].B2 &&
                Color[0].R1 == 255 && Color[0].G1 == 255 && Color[0].B1 == 255)
            {
                Color.Remove(0); Color.ToNull(); ToNull();//删除这个C
            }
            //加个条件 对第一行再判断。
            else if (Color.Count > 1 &&
              (Color[0].EndTime < max || Color[0].EndTime == max && if2max == true) &&
              (Color[0].StartTime > min || Color[0].StartTime == min && if2min == true) &&
               Color[0].R2 == Color[0].R1 && Color[0].G2 == Color[0].G1 && Color[0].B2 == Color[0].B1 &&
               Color[0].R2 == Color[1].R1 && Color[0].G2 == Color[1].G1 && Color[0].B2 == Color[1].B1 &&
               Color[0].R1 == 255 && Color[0].G1 == 255 && Color[0].B1 == 255)
            {
                Color.Remove(0); Color.ToNull(); ToNull();//删除这个C
            }
        }
        private void optMX()
        {
            if (IfDeep)
            {    //根据是否显示判定是否有效
                int tmpindex = 0;
                for (int k = 0; k < Fade.FadeOutList.Count; k++)
                {
                    for (int j = tmpindex; j < MoveX.Count; j++)
                    {
                        if (MoveX[j].StartTime > Fade.FadeOutList[k].EndTime)
                        {
                            tmpindex = j;
                            break;
                        }
                        if (j < MoveX.Count - 1 &&
                          MoveX[j].StartTime >= Fade.FadeOutList[k].StartTime && MoveX[j].EndTime <= Fade.FadeOutList[k].EndTime &&
                          MoveX[j + 1].StartTime <= Fade.FadeOutList[k].EndTime)
                        {
                            MoveX.Remove(j); MoveX.ToNull(); ToNull();
                            j--;
                        }
                        else if (j == MoveX.Count - 1 &&
                           MoveX[j].StartTime >= Fade.FadeOutList[k].StartTime && MoveX[j].EndTime < Fade.FadeOutList[k].EndTime &&
                          Fade.FadeOutList[k].EndTime >= MaxTime())
                        {
                            MoveX.Remove(j); MoveX.ToNull(); ToNull();
                            j--;
                        }
                    }
                }
                //
            }
            int i = MoveX.Count - 1;
            while (i >= 1)
            {
                int? max2 = MaxTime();
                int? min2 = MinTime();
                bool if2min2 = TwoMin;
                bool if2max2 = TwoMax;

                /* 当 这个MX的结束时间要小于obj的最大时间(或者是有两个以上的最大时间，其中包括MX的)
                 * 且 就是一个静止的动作
                 * 且 这个动作与上一个末动作一样     
                 */
                if ((MoveX[i].EndTime < this.MaxTime() || MoveX[i].EndTime == max2 && if2max2 == true) &&
                   MoveX[i].X1 == MoveX[i].X2 &&
                   MoveX[i].X1 == MoveX[i - 1].X2)
                {
                    MoveX.Remove(i); MoveX.ToNull(); ToNull();  //删除这个MX
                    i = MoveX.Count - 1;
                }
                /* 当 这个MX与前面的MX一致，又是单关键帧的，而且又不满足上面的（这里就是指小于最大时间的）
                 */
                else if (MoveX[i].X1 == MoveX[i].X2 &&
                  MoveX[i - 1].X1 == MoveX[i - 1].X2 &&
                  MoveX[i - 1].X1 == MoveX[i].X1)
                {

                    MoveX[i - 1].EndTime = MoveX[i].EndTime; //整合到前面的
                    MoveX.MoveEndTime(i);
                    if (MoveX[i - 1].StartTime == min2 && if2min2 == true)
                    {
                        MoveX[i - 1].StartTime = MoveX[i - 1].EndTime;
                        MoveX.MoveStartTime(i);
                    }
                    //MoveX[i - 1].StartTime = MoveX[i - 1].EndTime;
                    MoveX.Remove(i); MoveX.ToNull(); ToNull(); //删除这个MX
                    i = MoveX.Count - 1;
                }
                else i--;
            }

            /* 当 只有一个MX的时候
             * 且 这个MX的结束时间要小于obj的最大时间(或者是有两个以上的最大时间，其中包括MX的)
             * 且 这个MX的开始时间要大于obj的最小时间(或者是有两个以上的最小时间，其中包括MX的)
             * 且 就是一个静止的物品
             * 且 MX和X一样
             */
            int? max = MaxTime();
            int? min = MinTime();
            bool if2min = TwoMin;
            bool if2max = TwoMax;
            //Console.WriteLine(MoveX[0].EndTime < mx);
            if (MoveX.Count == 1 &&
                (MoveX[0].EndTime < max || MoveX[0].EndTime == max && if2max == true) &&
                (MoveX[0].StartTime > min || MoveX[0].StartTime == min && if2min == true) &&
                MoveX[0].X1 == MoveX[0].X2 &&
                MoveX[0].X1 == X)
            {
                MoveX.Remove(0); MoveX.ToNull(); ToNull();
            }
            //加个条件 对第一行再判断。
            else if (MoveX.Count > 1 &&
                (MoveX[0].EndTime < max || MoveX[0].EndTime == max && if2max == true) &&
                (MoveX[0].StartTime > min || MoveX[0].StartTime == min && if2min == true) &&
                MoveX[0].X1 == MoveX[0].X2 &&
                MoveX[0].X2 == MoveX[1].X1 &&
                MoveX[0].X1 == X)
            {
                MoveX.Remove(0); MoveX.ToNull(); ToNull();
            }
        }
        private void optMY()
        {
            if (IfDeep)
            {   //根据是否显示判定是否有效
                int tmpindex = 0;
                for (int k = 0; k < Fade.FadeOutList.Count; k++)
                {
                    for (int j = tmpindex; j < MoveY.Count; j++)
                    {
                        if (MoveY[j].StartTime > Fade.FadeOutList[k].EndTime)
                        {
                            tmpindex = j;
                            break;
                        }
                        if (j < MoveY.Count - 1 &&
                          MoveY[j].StartTime >= Fade.FadeOutList[k].StartTime && MoveY[j].EndTime <= Fade.FadeOutList[k].EndTime &&
                          MoveY[j + 1].StartTime <= Fade.FadeOutList[k].EndTime)
                        {
                            MoveY.Remove(j); MoveY.ToNull(); ToNull();
                            j--;
                        }
                        else if (j == MoveY.Count - 1 &&
                           MoveY[j].StartTime >= Fade.FadeOutList[k].StartTime && MoveY[j].EndTime < Fade.FadeOutList[k].EndTime &&
                          Fade.FadeOutList[k].EndTime >= MaxTime())
                        {
                            MoveY.Remove(j); MoveY.ToNull(); ToNull();
                            j--;
                        }
                    }
                }
                //
            }
            int i = MoveY.Count - 1;
            while (i >= 1)
            {
                int? max2 = MaxTime();
                int? min2 = MinTime();
                bool if2min2 = TwoMin;
                bool if2max2 = TwoMax;

                /* 当 这个MY的结束时间要小于obj的最大时间(或者是有两个以上的最大时间，其中包括MY的)
                 * 且 就是一个静止的动作
                 * 且 这个动作与上一个末动作一样     
                 */
                if ((MoveY[i].EndTime < this.MaxTime() || MoveY[i].EndTime == max2 && if2max2 == true) &&
                   MoveY[i].Y1 == MoveY[i].Y2 &&
                   MoveY[i].Y1 == MoveY[i - 1].Y2)
                {
                    MoveY.Remove(i); MoveY.ToNull(); ToNull();  //删除这个MY
                    i = MoveY.Count - 1;
                }
                /* 当 这个MY与前面的MY一致，又是单关键帧的，而且又不满足上面的（这里就是指小于最大时间的）
                 */
                else if (MoveY[i].Y1 == MoveY[i].Y2 &&
                  MoveY[i - 1].Y1 == MoveY[i - 1].Y2 &&
                  MoveY[i - 1].Y1 == MoveY[i].Y1)
                {

                    MoveY[i - 1].EndTime = MoveY[i].EndTime; //整合到前面的
                    MoveY.MoveEndTime(i);
                    if (MoveY[i - 1].StartTime == min2 && if2min2 == true)
                    {
                        MoveY[i - 1].StartTime = MoveY[i - 1].EndTime;
                        MoveY.MoveStartTime(i);
                    }
                    //MoveY[i - 1].StartTime = MoveY[i - 1].EndTime;
                    MoveY.Remove(i); MoveY.ToNull(); ToNull(); //删除这个MY
                    i = MoveY.Count - 1;
                }
                else i--;
            }

            /* 当 只有一个MY的时候
             * 且 这个MY的结束时间要小于obj的最大时间(或者是有两个以上的最大时间，其中包括MY的)
             * 且 这个MY的开始时间要大于obj的最小时间(或者是有两个以上的最小时间，其中包括MY的)
             * 且 就是一个静止的物品
             * 且 MY和X一样
             */
            int? max = MaxTime();
            int? min = MinTime();
            bool if2min = TwoMin;
            bool if2max = TwoMax;
            //Console.WriteLine(MoveY[0].EndTime < MY);
            if (MoveY.Count == 1 &&
                (MoveY[0].EndTime < max || MoveY[0].EndTime == max && if2max == true) &&
                (MoveY[0].StartTime > min || MoveY[0].StartTime == min && if2min == true) &&
                MoveY[0].Y1 == MoveY[0].Y2 &&
                MoveY[0].Y1 == Y)
            {
                MoveY.Remove(0); MoveY.ToNull(); ToNull();
            }
            //加个条件 对第一行再判断。
            else if (MoveY.Count > 1 &&
                (MoveY[0].EndTime < max || MoveY[0].EndTime == max && if2max == true) &&
                (MoveY[0].StartTime > min || MoveY[0].StartTime == min && if2min == true) &&
                MoveY[0].Y1 == MoveY[0].Y2 &&
                MoveY[0].Y2 == MoveY[1].Y1 &&
                MoveY[0].Y1 == Y)
            {
                MoveY.Remove(0); MoveY.ToNull(); ToNull();
            }
        }
    }
}
