using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibOSB
{
    partial class sbObject
    {
        private void optM_T(int gg)
        {
            int i = Trigger[gg].Move.Count - 1;
            while (i >= 1)
            {
                int? mx2 = Trigger[gg].MaxTime(); //这里有问题
                int? mi2 = Trigger[gg].MinTime(); //这里有问题
                bool if2min2 = Trigger[gg].TwoMin;
                bool if2max2 = Trigger[gg].TwoMax;

                /* 当 这个M的结束时间要小于obj的最大时间(或者是有两个以上的最大时间，其中包括M的)
                 * 且 就是一个静止的动作
                 * 且 这个动作与上一个末动作一样     
                 */
                if (
                   (Trigger[gg].Move[i].EndTime < mx2 || Trigger[gg].Move[i].EndTime == mx2 && if2max2 == true) &&
                   Trigger[gg].Move[i].X1 == Trigger[gg].Move[i].X2 && Trigger[gg].Move[i].Y1 == Trigger[gg].Move[i].Y2 &&
                   Trigger[gg].Move[i].X1 == Trigger[gg].Move[i - 1].X2 && Trigger[gg].Move[i].Y1 == Trigger[gg].Move[i - 1].Y2)
                {
                    Trigger[gg].Move.Remove(i); Trigger[gg].Move.ToNull(); Trigger[gg].ToNull(); ToNull(); //删除这个m
                    i = Trigger[gg].Move.Count - 1;
                }
                /* 当 这个M与前面的M一致，又是单关键帧的，而且又不满足上面的（这里就是指小于最大时间的）
                 */
                else if (Trigger[gg].Move[i].X1 == Trigger[gg].Move[i].X2 && Trigger[gg].Move[i].Y1 == Trigger[gg].Move[i].Y2 &&
                  Trigger[gg].Move[i - 1].X1 == Trigger[gg].Move[i - 1].X2 && Trigger[gg].Move[i - 1].Y1 == Trigger[gg].Move[i - 1].Y2 &&
                  Trigger[gg].Move[i - 1].X1 == Trigger[gg].Move[i].X1 && Trigger[gg].Move[i - 1].Y1 == Trigger[gg].Move[i].Y1)
                {
                    Trigger[gg].Move[i - 1].EndTime = Trigger[gg].Move[i].EndTime; //整合到前面的
                    Trigger[gg].Move.MoveEndTime(i);
                    if (Trigger[gg].Move[i - 1].StartTime > mi2 || Trigger[gg].Move[i - 1].StartTime == mi2 && if2min2 == true)
                    {
                        Trigger[gg].Move[i - 1].StartTime = Trigger[gg].Move[i - 1].EndTime; //整合到前面的
                        Trigger[gg].Move.MoveStartTime(i);
                    }
                    Trigger[gg].Move.Remove(i); Trigger[gg].Move.ToNull(); Trigger[gg].ToNull(); ToNull();//删除这个m
                    i = Trigger[gg].Move.Count - 1;
                }
                else i--;
            }

            /* 当 只有一个M的时候
             * 且 这个M的结束时间要小于obj的最大时间(或者是有两个以上的最大时间，其中包括M的)
             * 且 这个M的开始时间要大于obj的最小时间(或者是有两个以上的最小时间，其中包括M的)
             * 且 就是一个静止的物品             
             */
            int? mx = Trigger[gg].MaxTime();
            int? mi = Trigger[gg].MinTime();
            bool if2min = Trigger[gg].TwoMin;
            bool if2max = Trigger[gg].TwoMax;
            //Console.WriteLine(Trigger[gg].Move[0].EndTime < mx);
            if (
                (Trigger[gg].Move[0].EndTime < mx || Trigger[gg].Move[0].EndTime == mx && if2max == true) &&
                (Trigger[gg].Move[0].StartTime > mi || Trigger[gg].Move[0].StartTime == mi && if2min == true) &&
                Trigger[gg].Move[0].X1 == Trigger[gg].Move[0].X2 && Trigger[gg].Move[0].Y1 == Trigger[gg].Move[0].Y2 &&
                Move.Count == 0)
            {
                if (X == (int)X)
                {
                    X = Trigger[gg].Move[0].X1; Y = Trigger[gg].Move[0].Y1;
                    Trigger[gg].Move.Remove(0);
                    Trigger[gg].Move.ToNull(); Trigger[gg].ToNull(); ToNull(); //删除这个m
                }
                else if (Trigger[gg].Move[0].X1 == X && Trigger[gg].Move[0].Y1 == Y)
                {
                    Trigger[gg].Move.Remove(0);
                    Trigger[gg].Move.ToNull(); Trigger[gg].ToNull(); ToNull(); //删除这个m
                }
            }
            //加个条件 对第一行再判断。
            else if (Trigger[gg].Move.Count > 1 &&
                (Trigger[gg].Move[0].EndTime < mx || Trigger[gg].Move[0].EndTime == mx && if2max == true) &&
                (Trigger[gg].Move[0].StartTime > mi || Trigger[gg].Move[0].StartTime == mi && if2min == true) &&
                 Trigger[gg].Move[0].X2 == Trigger[gg].Move[0].X1 && Trigger[gg].Move[0].Y2 == Trigger[gg].Move[0].Y1 &&
                 Trigger[gg].Move[0].X2 == Trigger[gg].Move[1].X1 && Trigger[gg].Move[0].Y2 == Trigger[gg].Move[1].Y1 &&
                 Move.Count == 0)
            {
                if (X == (int)X)
                {
                    X = Trigger[gg].Move[0].X1; Y = Trigger[gg].Move[0].Y1;
                    Trigger[gg].Move.Remove(0);
                    Trigger[gg].Move.ToNull(); Trigger[gg].ToNull(); ToNull(); //删除这个m
                }
                else if (Trigger[gg].Move[0].X1 == X && Trigger[gg].Move[0].Y1 == Y && X == (int)X)
                {
                    Trigger[gg].Move.Remove(0);
                    Trigger[gg].Move.ToNull(); Trigger[gg].ToNull(); ToNull(); //删除这个m
                }
            }
        }
        private void optF_T(int gg)
        {
            int i = Trigger[gg].Fade.Count - 1;
            while (i >= 1)
            {
                int? max2 = Trigger[gg].MaxTime();
                int? min2 = Trigger[gg].MinTime();
                bool if2min2 = Trigger[gg].TwoMin;
                bool if2max2 = Trigger[gg].TwoMax;

                /* 当 这个F的结束时间要小于obj的最大时间(或者是有两个以上的最大时间，其中包括F的)
                 * 且 就是一个静止的动作
                 * 且 这个动作与上一个末动作一样     
                 */
                if (
                    (Trigger[gg].Fade[i].EndTime < max2 ||
                    Trigger[gg].Fade[i].EndTime == max2 && if2max2 == true) &&
                   Trigger[gg].Fade[i].F1 == Trigger[gg].Fade[i].F2 &&
                   Trigger[gg].Fade[i].F1 == Trigger[gg].Fade[i - 1].F2)
                {
                    Trigger[gg].Fade.Remove(i);
                    Trigger[gg].Fade.ToNull(); Trigger[gg].ToNull(); ToNull();  //删除这个F
                    i = Trigger[gg].Fade.Count - 1;
                }
                /* 当 这个F与前面的F一致，又是单关键帧的，而且又不满足上面的（这里就是指本身就是最大时间的）
                 */
                else if (Trigger[gg].Fade[i].F1 == Trigger[gg].Fade[i].F2 &&
                  Trigger[gg].Fade[i - 1].F1 == Trigger[gg].Fade[i - 1].F2 &&
                  Trigger[gg].Fade[i - 1].F1 == Trigger[gg].Fade[i].F1)
                {
                    Trigger[gg].Fade[i - 1].EndTime = Trigger[gg].Fade[i].EndTime; //整合到前面的
                    Trigger[gg].Fade.MoveEndTime(i);
                    if (Trigger[gg].Fade[i - 1].StartTime > min2 || Trigger[gg].Fade[i - 1].StartTime == min2 && if2min2 == true)
                    {
                        Trigger[gg].Fade[i - 1].StartTime = Trigger[gg].Fade[i - 1].EndTime;
                        Trigger[gg].Fade.MoveStartTime(i);
                    }
                    Trigger[gg].Fade.Remove(i);
                    Trigger[gg].Fade.ToNull(); Trigger[gg].ToNull(); ToNull(); //删除这个F
                    i = Trigger[gg].Fade.Count - 1;
                }
                else i--;
            }

            /* 当 只有一个F的时候
             * 且 这个F的结束时间要小于obj的最大时间(或者是有两个以上的最大时间，其中包括F的)
             * 且 这个F的开始时间要大于obj的最小时间(或者是有两个以上的最小时间，其中包括F的)
             * 且 就是一个静止的物品
             * 且 F就为1             
             */
            int? max = Trigger[gg].MaxTime();
            int? min = Trigger[gg].MinTime();
            bool if2min = Trigger[gg].TwoMin;
            bool if2max = Trigger[gg].TwoMax;
            //Console.WriteLine(Trigger[gg].Fade[0].EndTime < mx);
            if (Trigger[gg].Fade.Count == 1 &&
                (Trigger[gg].Fade[0].EndTime < max || Trigger[gg].Fade[0].EndTime == max && if2max == true) &&
                (Trigger[gg].Fade[0].StartTime > min || Trigger[gg].Fade[0].StartTime == min && if2min == true) &&
                Trigger[gg].Fade[0].F1 == Trigger[gg].Fade[0].F2 &&
                Trigger[gg].Fade[0].F1 == 1 &&
                Fade.Count == 0)
            {
                Trigger[gg].Fade.Remove(0);
                Trigger[gg].Fade.ToNull(); Trigger[gg].ToNull(); ToNull();
            }
            //加个条件 对第一行再判断。
            else if (Trigger[gg].Fade.Count > 1 &&
                (Trigger[gg].Fade[0].EndTime < max || Trigger[gg].Fade[0].EndTime == max && if2max == true) &&
                (Trigger[gg].Fade[0].StartTime > min || Trigger[gg].Fade[0].StartTime == min && if2min == true) &&
                Trigger[gg].Fade[0].F1 == Trigger[gg].Fade[0].F2 &&
                Trigger[gg].Fade[0].F2 == Trigger[gg].Fade[1].F1 &&
                Trigger[gg].Fade[0].F1 == 1 &&
                Fade.Count == 0)
            {
                Trigger[gg].Fade.Remove(0);
                Trigger[gg].Fade.ToNull(); Trigger[gg].ToNull(); ToNull();
            }
        }
        private void optS_T(int gg)
        {
            int i = Trigger[gg].Scale.Count - 1;
            while (i >= 1)
            {
                int? max2 = Trigger[gg].MaxTime();
                int? min2 = Trigger[gg].MinTime();
                bool if2min2 = Trigger[gg].TwoMin;
                bool if2max2 = Trigger[gg].TwoMax;

                /* 当 这个S的结束时间要小于obj的最大时间(或者是有两个以上的最大时间，其中包括S的)
                 * 且 就是一个静止的动作
                 * 且 这个动作与上一个末动作一样     
                 */
                if ((Trigger[gg].Scale[i].EndTime < max2 || Trigger[gg].Scale[i].EndTime == max2 && if2max2 == true) &&
                   Trigger[gg].Scale[i].S1 == Trigger[gg].Scale[i].S2 &&
                   Trigger[gg].Scale[i].S1 == Trigger[gg].Scale[i - 1].S2)
                {
                    Trigger[gg].Scale.Remove(i);
                    Trigger[gg].Scale.ToNull(); Trigger[gg].ToNull(); ToNull();  //删除这个S
                    i = Trigger[gg].Scale.Count - 1;
                }
                /* 当 这个S与前面的S一致，又是单关键帧的，而且又不满足上面的（这里就是指小于最大时间的）
                 */
                else if (Trigger[gg].Scale[i].S1 == Trigger[gg].Scale[i].S2 &&
                  Trigger[gg].Scale[i - 1].S1 == Trigger[gg].Scale[i - 1].S2 &&
                  Trigger[gg].Scale[i - 1].S1 == Trigger[gg].Scale[i].S1)
                {

                    Trigger[gg].Scale[i - 1].EndTime = Trigger[gg].Scale[i].EndTime; //整合到前面的
                    Trigger[gg].Scale.MoveEndTime(i);
                    if (Trigger[gg].Scale[i - 1].StartTime > min2 || Trigger[gg].Scale[i - 1].StartTime == min2 && if2min2 == true)
                    {
                        Trigger[gg].Scale[i - 1].StartTime = Trigger[gg].Scale[i - 1].EndTime;
                        Trigger[gg].Scale.MoveStartTime(i);
                    }
                    //Trigger[gg].Scale[i - 1].StartTime = Trigger[gg].Scale[i - 1].EndTime;
                    Trigger[gg].Scale.Remove(i);
                    Trigger[gg].Scale.ToNull(); Trigger[gg].ToNull(); ToNull(); //删除这个S
                    i = Trigger[gg].Scale.Count - 1;
                }
                else i--;
            }

            /* 当 只有一个S的时候
             * 且 这个S的结束时间要小于obj的最大时间(或者是有两个以上的最大时间，其中包括S的)
             * 且 这个S的开始时间要大于obj的最小时间(或者是有两个以上的最小时间，其中包括S的)
             * 且 就是一个静止的物品
             * 且 S就为1             
             */
            int? max = Trigger[gg].MaxTime();
            int? min = Trigger[gg].MinTime();
            bool if2min = Trigger[gg].TwoMin;
            bool if2max = Trigger[gg].TwoMax;
            //Console.WriteLine(Trigger[gg].Scale[0].EndTime < mx);
            if (
                (Trigger[gg].Scale[0].EndTime < max || Trigger[gg].Scale[0].EndTime == max && if2max == true) &&
                (Trigger[gg].Scale[0].StartTime > min || Trigger[gg].Scale[0].StartTime == min && if2min == true) &&
                Trigger[gg].Scale[0].S1 == Trigger[gg].Scale[0].S2 &&
                Trigger[gg].Scale[0].S1 == 1 &&
                Scale.Count == 0)
            {
                Trigger[gg].Scale.Remove(0);
                Trigger[gg].Scale.ToNull(); Trigger[gg].ToNull(); ToNull();
            }
            //加个条件 对第一行再判断。
            else if (Trigger[gg].Scale.Count > 1 &&
                (Trigger[gg].Scale[0].EndTime < max || Trigger[gg].Scale[0].EndTime == max && if2max == true) &&
                (Trigger[gg].Scale[0].StartTime > min || Trigger[gg].Scale[0].StartTime == min && if2min == true) &&
                Trigger[gg].Scale[0].S1 == Trigger[gg].Scale[0].S2 &&
                Trigger[gg].Scale[0].S2 == Trigger[gg].Scale[1].S1 &&
                Trigger[gg].Scale[0].S1 == 1 &&
                Scale.Count == 0)
            {
                Trigger[gg].Scale.Remove(0);
                Trigger[gg].Scale.ToNull(); Trigger[gg].ToNull(); ToNull();
            }
        }
        private void optR_T(int gg)
        {
            int i = Trigger[gg].Rotate.Count - 1;
            while (i >= 1)
            {
                int? max2 = Trigger[gg].MaxTime();
                int? min2 = Trigger[gg].MinTime();
                bool if2min2 = Trigger[gg].TwoMin;
                bool if2max2 = Trigger[gg].TwoMax;

                /* 当 这个R的结束时间要小于obj的最大时间(或者是有两个以上的最大时间，其中包括R的)
                 * 且 就是一个静止的动作
                 * 且 这个动作与上一个末动作一样     
                 */
                if (
                   (Trigger[gg].Rotate[i].EndTime < max2 || Trigger[gg].Rotate[i].EndTime == max2 && if2max2 == true) &&
                   Trigger[gg].Rotate[i].R1 == Trigger[gg].Rotate[i].R2 &&
                   Trigger[gg].Rotate[i].R1 == Trigger[gg].Rotate[i - 1].R2)
                {
                    Trigger[gg].Rotate.Remove(i);
                    Trigger[gg].Rotate.ToNull(); Trigger[gg].ToNull(); ToNull();  //删除这个R
                    i = Trigger[gg].Rotate.Count - 1;
                }
                /* 当 这个R与前面的R一致，又是单关键帧的，而且又不满足上面的（这里就是指小于最大时间的）
                 */
                else if (Trigger[gg].Rotate[i].R1 == Trigger[gg].Rotate[i].R2 &&
                  Trigger[gg].Rotate[i - 1].R1 == Trigger[gg].Rotate[i - 1].R2 &&
                  Trigger[gg].Rotate[i - 1].R1 == Trigger[gg].Rotate[i].R1)
                {

                    Trigger[gg].Rotate[i - 1].EndTime = Trigger[gg].Rotate[i].EndTime; //整合到前面的
                    Trigger[gg].Rotate.MoveEndTime(i);
                    if (Trigger[gg].Rotate[i - 1].StartTime > min2 || Trigger[gg].Rotate[i - 1].StartTime == min2 && if2min2 == true)
                    {
                        Trigger[gg].Rotate[i - 1].StartTime = Trigger[gg].Rotate[i - 1].EndTime;
                        Trigger[gg].Rotate.MoveStartTime(i);
                    }
                    //Trigger[gg].Rotate[i - 1].StartTime = Trigger[gg].Rotate[i - 1].EndTime;
                    Trigger[gg].Rotate.Remove(i);
                    Trigger[gg].Rotate.ToNull(); Trigger[gg].ToNull(); ToNull(); //删除这个R
                    i = Trigger[gg].Rotate.Count - 1;
                }
                else i--;
            }

            /* 当 只有一个R的时候
             * 且 这个R的结束时间要小于obj的最大时间(或者是有两个以上的最大时间，其中包括R的)
             * 且 这个R的开始时间要大于obj的最小时间(或者是有两个以上的最小时间，其中包括R的)
             * 且 就是一个静止的物品
             * 且 R就为0                 
             */
            int? max = Trigger[gg].MaxTime();
            int? min = Trigger[gg].MinTime();
            bool if2min = Trigger[gg].TwoMin;
            bool if2max = Trigger[gg].TwoMax;
            //Console.WriteLine(Trigger[gg].Rotate[0].EndTime < mx);
            if (
                (Trigger[gg].Rotate[0].EndTime < max || Trigger[gg].Rotate[0].EndTime == max && if2max == true) &&
                (Trigger[gg].Rotate[0].StartTime > min || Trigger[gg].Rotate[0].StartTime == min && if2min == true) &&
                Trigger[gg].Rotate[0].R1 == Trigger[gg].Rotate[0].R2 &&
                Trigger[gg].Rotate[0].R1 == 0 &&
                Rotate.Count == 0)
            {
                Trigger[gg].Rotate.Remove(0);
                Trigger[gg].Rotate.ToNull(); Trigger[gg].ToNull(); ToNull();
            }
            //加个条件 对第一行再判断。
            else if (Trigger[gg].Rotate.Count > 1 &&
                (Trigger[gg].Rotate[0].EndTime < max || Trigger[gg].Rotate[0].EndTime == max && if2max == true) &&
                (Trigger[gg].Rotate[0].StartTime > min || Trigger[gg].Rotate[0].StartTime == min && if2min == true) &&
                Trigger[gg].Rotate[0].R1 == Trigger[gg].Rotate[0].R2 &&
                Trigger[gg].Rotate[0].R2 == Trigger[gg].Rotate[1].R1 &&
                Trigger[gg].Rotate[0].R1 == 0 &&
                Rotate.Count == 0)
            {
                Trigger[gg].Rotate.Remove(0);
                Trigger[gg].Rotate.ToNull(); Trigger[gg].ToNull(); ToNull();
            }
        }
        private void optV_T(int gg)
        {
            int i = Trigger[gg].Vector.Count - 1;
            while (i >= 1)
            {
                int? mx2 = Trigger[gg].MaxTime();
                int? mi2 = Trigger[gg].MinTime();
                bool if2min2 = Trigger[gg].TwoMin;
                bool if2max2 = Trigger[gg].TwoMax;

                /* 当 这个V的结束时间要小于obj的最大时间(或者是有两个以上的最大时间，其中包括V的)
                 * 且 就是一个静止的动作
                 * 且 这个动作与上一个末动作一样     
                 */
                if (
                   (Trigger[gg].Vector[i].EndTime < mx2 || Trigger[gg].Vector[i].EndTime == mx2 && if2max2 == true) &&
                   Trigger[gg].Vector[i].VX1 == Trigger[gg].Vector[i].VX2 && Trigger[gg].Vector[i].VY1 == Trigger[gg].Vector[i].VY2 &&
                   Trigger[gg].Vector[i].VX1 == Trigger[gg].Vector[i - 1].VX2 && Trigger[gg].Vector[i].VY1 == Trigger[gg].Vector[i - 1].VY2)
                {
                    Trigger[gg].Vector.Remove(i);
                    Trigger[gg].Vector.ToNull(); Trigger[gg].ToNull(); ToNull(); //删除这个V
                    i = Trigger[gg].Vector.Count - 1;
                }
                /* 当 这个V与前面的V一致，又是单关键帧的，而且又不满足上面的（这里就是指小于最大时间的）
                 */
                else if (Trigger[gg].Vector[i].VX1 == Trigger[gg].Vector[i].VX2 && Trigger[gg].Vector[i].VY1 == Trigger[gg].Vector[i].VY2 &&
                  Trigger[gg].Vector[i - 1].VX1 == Trigger[gg].Vector[i - 1].VX2 && Trigger[gg].Vector[i - 1].VY1 == Trigger[gg].Vector[i - 1].VY2 &&
                  Trigger[gg].Vector[i - 1].VX1 == Trigger[gg].Vector[i].VX1 && Trigger[gg].Vector[i - 1].VY1 == Trigger[gg].Vector[i].VY1)
                {
                    Trigger[gg].Vector[i - 1].EndTime = Trigger[gg].Vector[i].EndTime; //整合到前面的
                    Trigger[gg].Vector.MoveEndTime(i);
                    if (Trigger[gg].Vector[i - 1].StartTime > mi2 || Trigger[gg].Vector[i - 1].StartTime == mi2 && if2min2 == true)
                    {
                        Trigger[gg].Vector[i - 1].StartTime = Trigger[gg].Vector[i - 1].EndTime; //整合到前面的
                        Trigger[gg].Vector.MoveStartTime(i);
                    }
                    Trigger[gg].Vector.Remove(i);
                    Trigger[gg].Vector.ToNull(); Trigger[gg].ToNull(); ToNull();//删除这个V
                    i = Trigger[gg].Vector.Count - 1;
                }
                else i--;
            }

            /* 当 只有一个V的时候
             * 且 这个V的结束时间要小于obj的最大时间(或者是有两个以上的最大时间，其中包括V的)
             * 且 这个V的开始时间要大于obj的最小时间(或者是有两个以上的最小时间，其中包括V的)
             * 且 就是一个静止的物品
             * 且 VX,VY都是1              
             */
            int? mx = Trigger[gg].MaxTime();
            int? mi = Trigger[gg].MinTime();
            bool if2min = Trigger[gg].TwoMin;
            bool if2max = Trigger[gg].TwoMax;
            //Console.WriteLine(Trigger[gg].Vector[0].EndTime < mx);
            if (
                (Trigger[gg].Vector[0].EndTime < mx || Trigger[gg].Vector[0].EndTime == mx && if2max == true) &&
                (Trigger[gg].Vector[0].StartTime > mi || Trigger[gg].Vector[0].StartTime == mi && if2min == true) &&
                Trigger[gg].Vector[0].VX1 == Trigger[gg].Vector[0].VX2 && Trigger[gg].Vector[0].VY1 == Trigger[gg].Vector[0].VY2 &&
                Trigger[gg].Vector[0].VX1 == 1 && Trigger[gg].Vector[0].VY1 == 1 &&
                Vector.Count == 0)
            {
                Trigger[gg].Vector.Remove(0);
                Trigger[gg].Vector.ToNull(); Trigger[gg].ToNull(); ToNull();//删除这个V
            }
            //加个条件 对第一行再判断。
            else if (Trigger[gg].Vector.Count > 1 &&
                (Trigger[gg].Vector[0].EndTime < mx || Trigger[gg].Vector[0].EndTime == mx && if2max == true) &&
                (Trigger[gg].Vector[0].StartTime > mi || Trigger[gg].Vector[0].StartTime == mi && if2min == true) &&
                Trigger[gg].Vector[0].VX2 == Trigger[gg].Vector[0].VX1 && Trigger[gg].Vector[0].VY2 == Trigger[gg].Vector[0].VY1 &&
                Trigger[gg].Vector[0].VX2 == Trigger[gg].Vector[1].VX1 && Trigger[gg].Vector[0].VY2 == Trigger[gg].Vector[1].VY1 &&
                Trigger[gg].Vector[0].VX1 == 1 && Trigger[gg].Vector[0].VY1 == 1 &&
                Vector.Count == 0)
            {
                Trigger[gg].Vector.Remove(0);
                Trigger[gg].Vector.ToNull(); Trigger[gg].ToNull(); ToNull();//删除这个V
            }

        }
        private void optC_T(int gg)
        {
            int i = Trigger[gg].Color.Count - 1;
            while (i >= 1)
            {
                int? max2 = Trigger[gg].MaxTime();
                int? min2 = Trigger[gg].MinTime();
                bool if2min2 = Trigger[gg].TwoMin;
                bool if2max2 = Trigger[gg].TwoMax;

                /* 当 这个C的结束时间要小于obj的最大时间(或者是有两个以上的最大时间，其中包括C的)
                 * 且 就是一个静止的动作
                 * 且 这个动作与上一个末动作一样     
                 */
                if (
                   (Trigger[gg].Color[i].EndTime < max2 || Trigger[gg].Color[i].EndTime == max2 && if2max2 == true) &&
                   Trigger[gg].Color[i].R1 == Trigger[gg].Color[i].R2 && Trigger[gg].Color[i].G1 == Trigger[gg].Color[i].G2 && Trigger[gg].Color[i].B1 == Trigger[gg].Color[i].B2 &&
                   Trigger[gg].Color[i].R1 == Trigger[gg].Color[i - 1].R2 && Trigger[gg].Color[i].G1 == Trigger[gg].Color[i - 1].G2 && Trigger[gg].Color[i].B1 == Trigger[gg].Color[i - 1].B2)
                {
                    Trigger[gg].Color.Remove(i); Trigger[gg].Color.ToNull(); ToNull(); //删除这个C
                    i = Trigger[gg].Color.Count - 1;
                }
                /* 当 这个C与前面的C一致，又是单关键帧的，而且又不满足上面的（这里就是指小于最大时间的）
                 */
                else if (Trigger[gg].Color[i].R1 == Trigger[gg].Color[i].R2 && Trigger[gg].Color[i].G1 == Trigger[gg].Color[i].G2 && Trigger[gg].Color[i].B1 == Trigger[gg].Color[i].B2 &&
                  Trigger[gg].Color[i - 1].R1 == Trigger[gg].Color[i - 1].R2 && Trigger[gg].Color[i - 1].G1 == Trigger[gg].Color[i - 1].G2 && Trigger[gg].Color[i - 1].B1 == Trigger[gg].Color[i - 1].B2 &&
                  Trigger[gg].Color[i - 1].R1 == Trigger[gg].Color[i].R1 && Trigger[gg].Color[i - 1].G1 == Trigger[gg].Color[i].G1 && Trigger[gg].Color[i - 1].B1 == Trigger[gg].Color[i].B1)
                {
                    Trigger[gg].Color[i - 1].EndTime = Trigger[gg].Color[i].EndTime; //整合到前面的
                    Trigger[gg].Color.MoveEndTime(i);
                    if (Trigger[gg].Color[i - 1].StartTime > min2 || Trigger[gg].Color[i - 1].StartTime == min2 && if2min2 == true)
                    {
                        Trigger[gg].Color[i - 1].StartTime = Trigger[gg].Color[i - 1].EndTime; //整合到前面的
                        Trigger[gg].Color.MoveStartTime(i);
                    }
                    Trigger[gg].Color.Remove(i); Trigger[gg].Color.ToNull(); ToNull();//删除这个C
                    i = Trigger[gg].Color.Count - 1;
                }
                else i--;
            }

            /* 当 只有一个C的时候
             * 且 这个C的结束时间要小于obj的最大时间(或者是有两个以上的最大时间，其中包括C的)
             * 且 这个C的开始时间要大于obj的最小时间(或者是有两个以上的最小时间，其中包括C的)
             * 且 就是一个静止的物品
             * 且 RGB=255,255,255             
             */
            int? max = Trigger[gg].MaxTime();
            int? min = Trigger[gg].MinTime();
            bool if2min = Trigger[gg].TwoMin;
            bool if2max = Trigger[gg].TwoMax;
            //Console.WriteLine(Trigger[gg].Color[0].EndTime < mx);
            if (Trigger[gg].Color.Count == 1 &&
                (Trigger[gg].Color[0].EndTime < max || Trigger[gg].Color[0].EndTime == max && if2max == true) &&
                (Trigger[gg].Color[0].StartTime > min || Trigger[gg].Color[0].StartTime == min && if2min == true) &&
                Trigger[gg].Color[0].R1 == Trigger[gg].Color[0].R2 && Trigger[gg].Color[0].G1 == Trigger[gg].Color[0].G2 && Trigger[gg].Color[0].B1 == Trigger[gg].Color[0].B2 &&
                Trigger[gg].Color[0].R1 == 255 && Trigger[gg].Color[0].G1 == 255 && Trigger[gg].Color[0].B1 == 255 &&
                Color.Count == 0)
            {
                Trigger[gg].Color.Remove(0); Trigger[gg].Color.ToNull(); ToNull();//删除这个C
            }
            //加个条件 对第一行再判断。
            else if (Trigger[gg].Color.Count > 1 &&
              (Trigger[gg].Color[0].EndTime < max || Trigger[gg].Color[0].EndTime == max && if2max == true) &&
              (Trigger[gg].Color[0].StartTime > min || Trigger[gg].Color[0].StartTime == min && if2min == true) &&
               Trigger[gg].Color[0].R2 == Trigger[gg].Color[0].R1 && Trigger[gg].Color[0].G2 == Trigger[gg].Color[0].G1 && Trigger[gg].Color[0].B2 == Trigger[gg].Color[0].B1 &&
               Trigger[gg].Color[0].R2 == Trigger[gg].Color[1].R1 && Trigger[gg].Color[0].G2 == Trigger[gg].Color[1].G1 && Trigger[gg].Color[0].B2 == Trigger[gg].Color[1].B1 &&
               Trigger[gg].Color[0].R1 == 255 && Trigger[gg].Color[0].G1 == 255 && Trigger[gg].Color[0].B1 == 255 &&
               Color.Count == 0)
            {
                Trigger[gg].Color.Remove(0); Trigger[gg].Color.ToNull(); ToNull();//删除这个C
            }
        }
        private void optMX_T(int gg)
        {
            int i = Trigger[gg].MoveX.Count - 1;
            while (i >= 1)
            {
                int? max2 = Trigger[gg].MaxTime();
                int? min2 = Trigger[gg].MinTime();
                bool if2min2 = Trigger[gg].TwoMin;
                bool if2max2 = Trigger[gg].TwoMax;

                /* 当 这个MX的结束时间要小于obj的最大时间(或者是有两个以上的最大时间，其中包括MX的)
                 * 且 就是一个静止的动作
                 * 且 这个动作与上一个末动作一样     
                 */
                if ((Trigger[gg].MoveX[i].EndTime < this.MaxTime() || Trigger[gg].MoveX[i].EndTime == max2 && if2max2 == true) &&
                   Trigger[gg].MoveX[i].X1 == Trigger[gg].MoveX[i].X2 &&
                   Trigger[gg].MoveX[i].X1 == Trigger[gg].MoveX[i - 1].X2)
                {
                    Trigger[gg].MoveX.Remove(i); Trigger[gg].MoveX.ToNull(); ToNull();  //删除这个MX
                    i = Trigger[gg].MoveX.Count - 1;
                }
                /* 当 这个MX与前面的MX一致，又是单关键帧的，而且又不满足上面的（这里就是指小于最大时间的）
                 */
                else if (Trigger[gg].MoveX[i].X1 == Trigger[gg].MoveX[i].X2 &&
                  Trigger[gg].MoveX[i - 1].X1 == Trigger[gg].MoveX[i - 1].X2 &&
                  Trigger[gg].MoveX[i - 1].X1 == Trigger[gg].MoveX[i].X1)
                {

                    Trigger[gg].MoveX[i - 1].EndTime = Trigger[gg].MoveX[i].EndTime; //整合到前面的
                    Trigger[gg].MoveX.MoveEndTime(i);
                    if (Trigger[gg].MoveX[i - 1].StartTime > min2 || Trigger[gg].MoveX[i - 1].StartTime == min2 && if2min2 == true)
                    {
                        Trigger[gg].MoveX[i - 1].StartTime = Trigger[gg].MoveX[i - 1].EndTime;
                        Trigger[gg].MoveX.MoveStartTime(i);
                    }
                    //Trigger[gg].MoveX[i - 1].StartTime = Trigger[gg].MoveX[i - 1].EndTime;
                    Trigger[gg].MoveX.Remove(i); Trigger[gg].MoveX.ToNull(); ToNull(); //删除这个MX
                    i = Trigger[gg].MoveX.Count - 1;
                }
                else i--;
            }

            /* 当 只有一个MX的时候
             * 且 这个MX的结束时间要小于obj的最大时间(或者是有两个以上的最大时间，其中包括MX的)
             * 且 这个MX的开始时间要大于obj的最小时间(或者是有两个以上的最小时间，其中包括MX的)
             * 且 就是一个静止的物品
             * 且 MX和X一样
             */
            int? max = Trigger[gg].MaxTime();
            int? min = Trigger[gg].MinTime();
            bool if2min = Trigger[gg].TwoMin;
            bool if2max = Trigger[gg].TwoMax;
            //Console.WriteLine(Trigger[gg].MoveX[0].EndTime < mx);
            if (Trigger[gg].MoveX.Count == 1 &&
                (Trigger[gg].MoveX[0].EndTime < max || Trigger[gg].MoveX[0].EndTime == max && if2max == true) &&
                (Trigger[gg].MoveX[0].StartTime > min || Trigger[gg].MoveX[0].StartTime == min && if2min == true) &&
                Trigger[gg].MoveX[0].X1 == Trigger[gg].MoveX[0].X2 &&
                Trigger[gg].MoveX[0].X1 == X &&
                MoveX.Count == 0)
            {
                Trigger[gg].MoveX.Remove(0); Trigger[gg].MoveX.ToNull(); ToNull();
            }
            //加个条件 对第一行再判断。
            else if (Trigger[gg].MoveX.Count > 1 &&
                (Trigger[gg].MoveX[0].EndTime < max || Trigger[gg].MoveX[0].EndTime == max && if2max == true) &&
                (Trigger[gg].MoveX[0].StartTime > min || Trigger[gg].MoveX[0].StartTime == min && if2min == true) &&
                Trigger[gg].MoveX[0].X1 == Trigger[gg].MoveX[0].X2 &&
                Trigger[gg].MoveX[0].X2 == Trigger[gg].MoveX[1].X1 &&
                Trigger[gg].MoveX[0].X1 == X &&
                MoveX.Count == 0)
            {
                Trigger[gg].MoveX.Remove(0); Trigger[gg].MoveX.ToNull(); ToNull();
            }
        }
        private void optMY_T(int gg)
        {
            int i = Trigger[gg].MoveY.Count - 1;
            while (i >= 1)
            {
                int? max2 = Trigger[gg].MaxTime();
                int? min2 = Trigger[gg].MinTime();
                bool if2min2 = Trigger[gg].TwoMin;
                bool if2max2 = Trigger[gg].TwoMax;

                /* 当 这个MY的结束时间要小于obj的最大时间(或者是有两个以上的最大时间，其中包括MY的)
                 * 且 就是一个静止的动作
                 * 且 这个动作与上一个末动作一样     
                 */
                if ((Trigger[gg].MoveY[i].EndTime < this.MaxTime() || Trigger[gg].MoveY[i].EndTime == max2 && if2max2 == true) &&
                   Trigger[gg].MoveY[i].Y1 == Trigger[gg].MoveY[i].Y2 &&
                   Trigger[gg].MoveY[i].Y1 == Trigger[gg].MoveY[i - 1].Y2)
                {
                    Trigger[gg].MoveY.Remove(i); Trigger[gg].MoveY.ToNull(); ToNull();  //删除这个MY
                    i = Trigger[gg].MoveY.Count - 1;
                }
                /* 当 这个MY与前面的MY一致，又是单关键帧的，而且又不满足上面的（这里就是指小于最大时间的）
                 */
                else if (Trigger[gg].MoveY[i].Y1 == Trigger[gg].MoveY[i].Y2 &&
                  Trigger[gg].MoveY[i - 1].Y1 == Trigger[gg].MoveY[i - 1].Y2 &&
                  Trigger[gg].MoveY[i - 1].Y1 == Trigger[gg].MoveY[i].Y1)
                {

                    Trigger[gg].MoveY[i - 1].EndTime = Trigger[gg].MoveY[i].EndTime; //整合到前面的
                    Trigger[gg].MoveY.MoveEndTime(i);
                    if (Trigger[gg].MoveY[i - 1].StartTime > min2 || Trigger[gg].MoveY[i - 1].StartTime == min2 && if2min2 == true)
                    {
                        Trigger[gg].MoveY[i - 1].StartTime = Trigger[gg].MoveY[i - 1].EndTime;
                        Trigger[gg].MoveY.MoveStartTime(i);
                    }
                    //Trigger[gg].MoveY[i - 1].StartTime = Trigger[gg].MoveY[i - 1].EndTime;
                    Trigger[gg].MoveY.Remove(i); Trigger[gg].MoveY.ToNull(); ToNull(); //删除这个MY
                    i = Trigger[gg].MoveY.Count - 1;
                }
                else i--;
            }

            /* 当 只有一个MY的时候
             * 且 这个MY的结束时间要小于obj的最大时间(或者是有两个以上的最大时间，其中包括MY的)
             * 且 这个MY的开始时间要大于obj的最小时间(或者是有两个以上的最小时间，其中包括MY的)
             * 且 就是一个静止的物品
             * 且 MY和X一样
             */
            int? max = Trigger[gg].MaxTime();
            int? min = Trigger[gg].MinTime();
            bool if2min = Trigger[gg].TwoMin;
            bool if2max = Trigger[gg].TwoMax;
            //Console.WriteLine(Trigger[gg].MoveY[0].EndTime < MY);
            if (Trigger[gg].MoveY.Count == 1 &&
                (Trigger[gg].MoveY[0].EndTime < max || Trigger[gg].MoveY[0].EndTime == max && if2max == true) &&
                (Trigger[gg].MoveY[0].StartTime > min || Trigger[gg].MoveY[0].StartTime == min && if2min == true) &&
                Trigger[gg].MoveY[0].Y1 == Trigger[gg].MoveY[0].Y2 &&
                Trigger[gg].MoveY[0].Y1 == Y &&
                MoveY.Count == 0)
            {
                Trigger[gg].MoveY.Remove(0); Trigger[gg].MoveY.ToNull(); ToNull();
            }
            //加个条件 对第一行再判断。
            else if (Trigger[gg].MoveY.Count > 1 &&
                (Trigger[gg].MoveY[0].EndTime < max || Trigger[gg].MoveY[0].EndTime == max && if2max == true) &&
                (Trigger[gg].MoveY[0].StartTime > min || Trigger[gg].MoveY[0].StartTime == min && if2min == true) &&
                Trigger[gg].MoveY[0].Y1 == Trigger[gg].MoveY[0].Y2 &&
                Trigger[gg].MoveY[0].Y2 == Trigger[gg].MoveY[1].Y1 &&
                Trigger[gg].MoveY[0].Y1 == Y &&
                MoveY.Count == 0)
            {
                Trigger[gg].MoveY.Remove(0); Trigger[gg].MoveY.ToNull(); ToNull();
            }
        }
    }
}
