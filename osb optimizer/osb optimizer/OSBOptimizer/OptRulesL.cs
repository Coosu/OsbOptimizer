using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LibOSB
{
    partial class SBObject
    {
        private void optM_L(int gg)
        {
            int i = Loop[gg].Move.Count - 1;
            while (i >= 1)
            {
                int? mx2 = Loop[gg].MaxTime(); //这里有问题
                int? mi2 = Loop[gg].MinTime(); //这里有问题
                bool if2min2 = Loop[gg].TwoMin; 
                bool if2max2 = Loop[gg].TwoMax;

                /* 当 这个M的结束时间要小于obj的最大时间(或者是有两个以上的最大时间，其中包括M的)
                 * 且 就是一个静止的动作
                 * 且 这个动作与上一个末动作一样     
                 */
                if (
                   (Loop[gg].Move[i].EndTime < mx2 || Loop[gg].Move[i].EndTime == mx2 && if2max2 == true) &&
                   Loop[gg].Move[i].X1 == Loop[gg].Move[i].X2 && Loop[gg].Move[i].Y1 == Loop[gg].Move[i].Y2 &&
                   Loop[gg].Move[i].X1 == Loop[gg].Move[i - 1].X2 && Loop[gg].Move[i].Y1 == Loop[gg].Move[i - 1].Y2)
                {
                    Loop[gg].Move.Remove(i); Loop[gg].Move.ToNull(); Loop[gg].ToNull(); ToNull(); //删除这个m
                    i = Loop[gg].Move.Count - 1;
                }
                /* 当 这个M与前面的M一致，又是单关键帧的，而且又不满足上面的（这里就是指小于最大时间的）
                 */
                else if (Loop[gg].Move[i].X1 == Loop[gg].Move[i].X2 && Loop[gg].Move[i].Y1 == Loop[gg].Move[i].Y2 &&
                  Loop[gg].Move[i - 1].X1 == Loop[gg].Move[i - 1].X2 && Loop[gg].Move[i - 1].Y1 == Loop[gg].Move[i - 1].Y2 &&
                  Loop[gg].Move[i - 1].X1 == Loop[gg].Move[i].X1 && Loop[gg].Move[i - 1].Y1 == Loop[gg].Move[i].Y1)
                {
                    Loop[gg].Move[i - 1].EndTime = Loop[gg].Move[i].EndTime; //整合到前面的
                    Loop[gg].Move.MoveEndTime(i);
                    if (Loop[gg].Move[i - 1].StartTime > mi2 || Loop[gg].Move[i - 1].StartTime == mi2 && if2min2 == true)
                    {
                        Loop[gg].Move[i - 1].StartTime = Loop[gg].Move[i - 1].EndTime; //整合到前面的
                        Loop[gg].Move.MoveStartTime(i);
                    }
                    Loop[gg].Move.Remove(i); Loop[gg].Move.ToNull(); Loop[gg].ToNull(); ToNull();//删除这个m
                    i = Loop[gg].Move.Count - 1;
                }
                else i--;
            }

            /* 当 只有一个M的时候
             * 且 这个M的结束时间要小于obj的最大时间(或者是有两个以上的最大时间，其中包括M的)
             * 且 这个M的开始时间要大于obj的最小时间(或者是有两个以上的最小时间，其中包括M的)
             * 且 就是一个静止的物品             
             */
            int? mx = Loop[gg].MaxTime();
            int? mi = Loop[gg].MinTime();
            bool if2min = Loop[gg].TwoMin;
            bool if2max = Loop[gg].TwoMax;
            //Console.WriteLine(Loop[gg].Move[0].EndTime < mx);
            if (
                (Loop[gg].Move[0].EndTime < mx || Loop[gg].Move[0].EndTime == mx && if2max == true) &&
                (Loop[gg].Move[0].StartTime > mi || Loop[gg].Move[0].StartTime == mi && if2min == true) &&
                Loop[gg].Move[0].X1 == Loop[gg].Move[0].X2 && Loop[gg].Move[0].Y1 == Loop[gg].Move[0].Y2 &&
                Move.Count == 0)
            {
                if (X == (int)X)
                {
                    X = Loop[gg].Move[0].X1; Y = Loop[gg].Move[0].Y1;
                    Loop[gg].Move.Remove(0);
                    Loop[gg].Move.ToNull(); Loop[gg].ToNull(); ToNull(); //删除这个m
                }
                else if (Loop[gg].Move[0].X1 == X && Loop[gg].Move[0].Y1 == Y)
                {
                    Loop[gg].Move.Remove(0);
                    Loop[gg].Move.ToNull(); Loop[gg].ToNull(); ToNull(); //删除这个m
                }
            }
            //加个条件 对第一行再判断。
            else if (Loop[gg].Move.Count > 1 &&
                (Loop[gg].Move[0].EndTime < mx || Loop[gg].Move[0].EndTime == mx && if2max == true) &&
                (Loop[gg].Move[0].StartTime > mi || Loop[gg].Move[0].StartTime == mi && if2min == true) &&
                 Loop[gg].Move[0].X2 == Loop[gg].Move[0].X1 && Loop[gg].Move[0].Y2 == Loop[gg].Move[0].Y1 &&
                 Loop[gg].Move[0].X2 == Loop[gg].Move[1].X1 && Loop[gg].Move[0].Y2 == Loop[gg].Move[1].Y1 &&
                 Move.Count == 0)
            {
                if (X == (int)X)
                {
                    X = Loop[gg].Move[0].X1; Y = Loop[gg].Move[0].Y1;
                    Loop[gg].Move.Remove(0);
                    Loop[gg].Move.ToNull(); Loop[gg].ToNull(); ToNull(); //删除这个m
                }
                else if (Loop[gg].Move[0].X1 == X && Loop[gg].Move[0].Y1 == Y && X == (int)X)
                {
                    Loop[gg].Move.Remove(0);
                    Loop[gg].Move.ToNull(); Loop[gg].ToNull(); ToNull(); //删除这个m
                }
            }
        }
        private void optF_L(int gg)
        {
            int i = Loop[gg].Fade.Count - 1;
            while (i >= 1)
            {
                int? max2 = Loop[gg].MaxTime();
                int? min2 = Loop[gg].MinTime();
                bool if2min2 = Loop[gg].TwoMin;
                bool if2max2 = Loop[gg].TwoMax;

                /* 当 这个F的结束时间要小于obj的最大时间(或者是有两个以上的最大时间，其中包括F的)
                 * 且 就是一个静止的动作
                 * 且 这个动作与上一个末动作一样     
                 */
                if (
                    (Loop[gg].Fade[i].EndTime < max2 ||
                    Loop[gg].Fade[i].EndTime == max2 && if2max2 == true) &&
                   Loop[gg].Fade[i].F1 == Loop[gg].Fade[i].F2 &&
                   Loop[gg].Fade[i].F1 == Loop[gg].Fade[i - 1].F2)
                {
                    Loop[gg].Fade.Remove(i);
                    Loop[gg].Fade.ToNull(); Loop[gg].ToNull(); ToNull();  //删除这个F
                    i = Loop[gg].Fade.Count - 1;
                }
                /* 当 这个F与前面的F一致，又是单关键帧的，而且又不满足上面的（这里就是指本身就是最大时间的）
                 */
                else if (Loop[gg].Fade[i].F1 == Loop[gg].Fade[i].F2 &&
                  Loop[gg].Fade[i - 1].F1 == Loop[gg].Fade[i - 1].F2 &&
                  Loop[gg].Fade[i - 1].F1 == Loop[gg].Fade[i].F1)
                {
                    Loop[gg].Fade[i - 1].EndTime = Loop[gg].Fade[i].EndTime; //整合到前面的
                    Loop[gg].Fade.MoveEndTime(i);
                    if (Loop[gg].Fade[i - 1].StartTime > min2 || Loop[gg].Fade[i - 1].StartTime == min2 && if2min2 == true)
                    {
                        Loop[gg].Fade[i - 1].StartTime = Loop[gg].Fade[i - 1].EndTime;
                        Loop[gg].Fade.MoveStartTime(i);
                    }
                    Loop[gg].Fade.Remove(i);
                    Loop[gg].Fade.ToNull(); Loop[gg].ToNull(); ToNull(); //删除这个F
                    i = Loop[gg].Fade.Count - 1;
                }
                else i--;
            }

            /* 当 只有一个F的时候
             * 且 这个F的结束时间要小于obj的最大时间(或者是有两个以上的最大时间，其中包括F的)
             * 且 这个F的开始时间要大于obj的最小时间(或者是有两个以上的最小时间，其中包括F的)
             * 且 就是一个静止的物品
             * 且 F就为1             
             */
            int? max = Loop[gg].MaxTime();
            int? min = Loop[gg].MinTime();
            bool if2min = Loop[gg].TwoMin;
            bool if2max = Loop[gg].TwoMax;
            //Console.WriteLine(Loop[gg].Fade[0].EndTime < mx);
            if (Loop[gg].Fade.Count == 1 &&
                (Loop[gg].Fade[0].EndTime < max || Loop[gg].Fade[0].EndTime == max && if2max == true) &&
                (Loop[gg].Fade[0].StartTime > min || Loop[gg].Fade[0].StartTime == min && if2min == true) &&
                Loop[gg].Fade[0].F1 == Loop[gg].Fade[0].F2 &&
                Loop[gg].Fade[0].F1 == 1 &&
                Fade.Count == 0)
            {
                Loop[gg].Fade.Remove(0);
                Loop[gg].Fade.ToNull(); Loop[gg].ToNull(); ToNull();
            }
            //加个条件 对第一行再判断。
            else if (Loop[gg].Fade.Count > 1 &&
                (Loop[gg].Fade[0].EndTime < max || Loop[gg].Fade[0].EndTime == max && if2max == true) &&
                (Loop[gg].Fade[0].StartTime > min || Loop[gg].Fade[0].StartTime == min && if2min == true) &&
                Loop[gg].Fade[0].F1 == Loop[gg].Fade[0].F2 &&
                Loop[gg].Fade[0].F2 == Loop[gg].Fade[1].F1 &&
                Loop[gg].Fade[0].F1 == 1 &&
                Fade.Count == 0)
            {
                Loop[gg].Fade.Remove(0);
                Loop[gg].Fade.ToNull(); Loop[gg].ToNull(); ToNull();
            }
        }
        private void optS_L(int gg)
        {
            int i = Loop[gg].Scale.Count - 1;
            while (i >= 1)
            {
                int? max2 = Loop[gg].MaxTime();
                int? min2 = Loop[gg].MinTime();
                bool if2min2 = Loop[gg].TwoMin;
                bool if2max2 = Loop[gg].TwoMax;

                /* 当 这个S的结束时间要小于obj的最大时间(或者是有两个以上的最大时间，其中包括S的)
                 * 且 就是一个静止的动作
                 * 且 这个动作与上一个末动作一样     
                 */
                if ((Loop[gg].Scale[i].EndTime < max2 || Loop[gg].Scale[i].EndTime == max2 && if2max2 == true) &&
                   Loop[gg].Scale[i].S1 == Loop[gg].Scale[i].S2 &&
                   Loop[gg].Scale[i].S1 == Loop[gg].Scale[i - 1].S2)
                {
                    Loop[gg].Scale.Remove(i);
                    Loop[gg].Scale.ToNull(); Loop[gg].ToNull(); ToNull();  //删除这个S
                    i = Loop[gg].Scale.Count - 1;
                }
                /* 当 这个S与前面的S一致，又是单关键帧的，而且又不满足上面的（这里就是指小于最大时间的）
                 */
                else if (Loop[gg].Scale[i].S1 == Loop[gg].Scale[i].S2 &&
                  Loop[gg].Scale[i - 1].S1 == Loop[gg].Scale[i - 1].S2 &&
                  Loop[gg].Scale[i - 1].S1 == Loop[gg].Scale[i].S1)
                {

                    Loop[gg].Scale[i - 1].EndTime = Loop[gg].Scale[i].EndTime; //整合到前面的
                    Loop[gg].Scale.MoveEndTime(i);
                    if (Loop[gg].Scale[i - 1].StartTime > min2 || Loop[gg].Scale[i - 1].StartTime == min2 && if2min2 == true)
                    {
                        Loop[gg].Scale[i - 1].StartTime = Loop[gg].Scale[i - 1].EndTime;
                        Loop[gg].Scale.MoveStartTime(i);
                    }
                    //Loop[gg].Scale[i - 1].StartTime = Loop[gg].Scale[i - 1].EndTime;
                    Loop[gg].Scale.Remove(i);
                    Loop[gg].Scale.ToNull(); Loop[gg].ToNull(); ToNull(); //删除这个S
                    i = Loop[gg].Scale.Count - 1;
                }
                else i--;
            }

            /* 当 只有一个S的时候
             * 且 这个S的结束时间要小于obj的最大时间(或者是有两个以上的最大时间，其中包括S的)
             * 且 这个S的开始时间要大于obj的最小时间(或者是有两个以上的最小时间，其中包括S的)
             * 且 就是一个静止的物品
             * 且 S就为1             
             */
            int? max = Loop[gg].MaxTime();
            int? min = Loop[gg].MinTime();
            bool if2min = Loop[gg].TwoMin;
            bool if2max = Loop[gg].TwoMax;
            //Console.WriteLine(Loop[gg].Scale[0].EndTime < mx);
            if (
                (Loop[gg].Scale[0].EndTime < max || Loop[gg].Scale[0].EndTime == max && if2max == true) &&
                (Loop[gg].Scale[0].StartTime > min || Loop[gg].Scale[0].StartTime == min && if2min == true) &&
                Loop[gg].Scale[0].S1 == Loop[gg].Scale[0].S2 &&
                Loop[gg].Scale[0].S1 == 1 &&
                Scale.Count == 0)
            {
                Loop[gg].Scale.Remove(0);
                Loop[gg].Scale.ToNull(); Loop[gg].ToNull(); ToNull();
            }
            //加个条件 对第一行再判断。
            else if (Loop[gg].Scale.Count > 1 &&
                (Loop[gg].Scale[0].EndTime < max || Loop[gg].Scale[0].EndTime == max && if2max == true) &&
                (Loop[gg].Scale[0].StartTime > min || Loop[gg].Scale[0].StartTime == min && if2min == true) &&
                Loop[gg].Scale[0].S1 == Loop[gg].Scale[0].S2 &&
                Loop[gg].Scale[0].S2 == Loop[gg].Scale[1].S1 &&
                Loop[gg].Scale[0].S1 == 1 &&
                Scale.Count == 0)
            {
                Loop[gg].Scale.Remove(0);
                Loop[gg].Scale.ToNull(); Loop[gg].ToNull(); ToNull();
            }
        }
        private void optR_L(int gg)
        {
            int i = Loop[gg].Rotate.Count - 1;
            while (i >= 1)
            {
                int? max2 = Loop[gg].MaxTime();
                int? min2 = Loop[gg].MinTime();
                bool if2min2 = Loop[gg].TwoMin;
                bool if2max2 = Loop[gg].TwoMax;

                /* 当 这个R的结束时间要小于obj的最大时间(或者是有两个以上的最大时间，其中包括R的)
                 * 且 就是一个静止的动作
                 * 且 这个动作与上一个末动作一样     
                 */
                if (
                   (Loop[gg].Rotate[i].EndTime < max2 || Loop[gg].Rotate[i].EndTime == max2 && if2max2 == true) &&
                   Loop[gg].Rotate[i].R1 == Loop[gg].Rotate[i].R2 &&
                   Loop[gg].Rotate[i].R1 == Loop[gg].Rotate[i - 1].R2)
                {
                    Loop[gg].Rotate.Remove(i);
                    Loop[gg].Rotate.ToNull(); Loop[gg].ToNull(); ToNull();  //删除这个R
                    i = Loop[gg].Rotate.Count - 1;
                }
                /* 当 这个R与前面的R一致，又是单关键帧的，而且又不满足上面的（这里就是指小于最大时间的）
                 */
                else if (Loop[gg].Rotate[i].R1 == Loop[gg].Rotate[i].R2 &&
                  Loop[gg].Rotate[i - 1].R1 == Loop[gg].Rotate[i - 1].R2 &&
                  Loop[gg].Rotate[i - 1].R1 == Loop[gg].Rotate[i].R1)
                {

                    Loop[gg].Rotate[i - 1].EndTime = Loop[gg].Rotate[i].EndTime; //整合到前面的
                    Loop[gg].Rotate.MoveEndTime(i);
                    if (Loop[gg].Rotate[i - 1].StartTime > min2 || Loop[gg].Rotate[i - 1].StartTime == min2 && if2min2 == true)
                    {
                        Loop[gg].Rotate[i - 1].StartTime = Loop[gg].Rotate[i - 1].EndTime;
                        Loop[gg].Rotate.MoveStartTime(i);
                    }
                    //Loop[gg].Rotate[i - 1].StartTime = Loop[gg].Rotate[i - 1].EndTime;
                    Loop[gg].Rotate.Remove(i);
                    Loop[gg].Rotate.ToNull(); Loop[gg].ToNull(); ToNull(); //删除这个R
                    i = Loop[gg].Rotate.Count - 1;
                }
                else i--;
            }

            /* 当 只有一个R的时候
             * 且 这个R的结束时间要小于obj的最大时间(或者是有两个以上的最大时间，其中包括R的)
             * 且 这个R的开始时间要大于obj的最小时间(或者是有两个以上的最小时间，其中包括R的)
             * 且 就是一个静止的物品
             * 且 R就为0                 
             */
            int? max = Loop[gg].MaxTime();
            int? min = Loop[gg].MinTime();
            bool if2min = Loop[gg].TwoMin;
            bool if2max = Loop[gg].TwoMax;
            //Console.WriteLine(Loop[gg].Rotate[0].EndTime < mx);
            if (
                (Loop[gg].Rotate[0].EndTime < max || Loop[gg].Rotate[0].EndTime == max && if2max == true) &&
                (Loop[gg].Rotate[0].StartTime > min || Loop[gg].Rotate[0].StartTime == min && if2min == true) &&
                Loop[gg].Rotate[0].R1 == Loop[gg].Rotate[0].R2 &&
                Loop[gg].Rotate[0].R1 == 0 &&
                Rotate.Count == 0)
            {
                Loop[gg].Rotate.Remove(0);
                Loop[gg].Rotate.ToNull(); Loop[gg].ToNull(); ToNull();
            }
            //加个条件 对第一行再判断。
            else if (Loop[gg].Rotate.Count > 1 &&
                (Loop[gg].Rotate[0].EndTime < max || Loop[gg].Rotate[0].EndTime == max && if2max == true) &&
                (Loop[gg].Rotate[0].StartTime > min || Loop[gg].Rotate[0].StartTime == min && if2min == true) &&
                Loop[gg].Rotate[0].R1 == Loop[gg].Rotate[0].R2 &&
                Loop[gg].Rotate[0].R2 == Loop[gg].Rotate[1].R1 &&
                Loop[gg].Rotate[0].R1 == 0 &&
                Rotate.Count == 0)
            {
                Loop[gg].Rotate.Remove(0);
                Loop[gg].Rotate.ToNull(); Loop[gg].ToNull(); ToNull();
            }
        }
        private void optV_L(int gg)
        {
            int i = Loop[gg].Vector.Count - 1;
            while (i >= 1)
            {
                int? mx2 = Loop[gg].MaxTime();
                int? mi2 = Loop[gg].MinTime();
                bool if2min2 = Loop[gg].TwoMin;
                bool if2max2 = Loop[gg].TwoMax;

                /* 当 这个V的结束时间要小于obj的最大时间(或者是有两个以上的最大时间，其中包括V的)
                 * 且 就是一个静止的动作
                 * 且 这个动作与上一个末动作一样     
                 */
                if (
                   (Loop[gg].Vector[i].EndTime < mx2 || Loop[gg].Vector[i].EndTime == mx2 && if2max2 == true) &&
                   Loop[gg].Vector[i].VX1 == Loop[gg].Vector[i].VX2 && Loop[gg].Vector[i].VY1 == Loop[gg].Vector[i].VY2 &&
                   Loop[gg].Vector[i].VX1 == Loop[gg].Vector[i - 1].VX2 && Loop[gg].Vector[i].VY1 == Loop[gg].Vector[i - 1].VY2)
                {
                    Loop[gg].Vector.Remove(i);
                    Loop[gg].Vector.ToNull(); Loop[gg].ToNull(); ToNull(); //删除这个V
                    i = Loop[gg].Vector.Count - 1;
                }
                /* 当 这个V与前面的V一致，又是单关键帧的，而且又不满足上面的（这里就是指小于最大时间的）
                 */
                else if (Loop[gg].Vector[i].VX1 == Loop[gg].Vector[i].VX2 && Loop[gg].Vector[i].VY1 == Loop[gg].Vector[i].VY2 &&
                  Loop[gg].Vector[i - 1].VX1 == Loop[gg].Vector[i - 1].VX2 && Loop[gg].Vector[i - 1].VY1 == Loop[gg].Vector[i - 1].VY2 &&
                  Loop[gg].Vector[i - 1].VX1 == Loop[gg].Vector[i].VX1 && Loop[gg].Vector[i - 1].VY1 == Loop[gg].Vector[i].VY1)
                {
                    Loop[gg].Vector[i - 1].EndTime = Loop[gg].Vector[i].EndTime; //整合到前面的
                    Loop[gg].Vector.MoveEndTime(i);
                    if (Loop[gg].Vector[i - 1].StartTime > mi2 || Loop[gg].Vector[i - 1].StartTime == mi2 && if2min2 == true)
                    {
                        Loop[gg].Vector[i - 1].StartTime = Loop[gg].Vector[i - 1].EndTime; //整合到前面的
                        Loop[gg].Vector.MoveStartTime(i);
                    }
                    Loop[gg].Vector.Remove(i);
                    Loop[gg].Vector.ToNull(); Loop[gg].ToNull(); ToNull();//删除这个V
                    i = Loop[gg].Vector.Count - 1;
                }
                else i--;
            }

            /* 当 只有一个V的时候
             * 且 这个V的结束时间要小于obj的最大时间(或者是有两个以上的最大时间，其中包括V的)
             * 且 这个V的开始时间要大于obj的最小时间(或者是有两个以上的最小时间，其中包括V的)
             * 且 就是一个静止的物品
             * 且 VX,VY都是1              
             */
            int? mx = Loop[gg].MaxTime();
            int? mi = Loop[gg].MinTime();
            bool if2min = Loop[gg].TwoMin;
            bool if2max = Loop[gg].TwoMax;
            //Console.WriteLine(Loop[gg].Vector[0].EndTime < mx);
            if (
                (Loop[gg].Vector[0].EndTime < mx || Loop[gg].Vector[0].EndTime == mx && if2max == true) &&
                (Loop[gg].Vector[0].StartTime > mi || Loop[gg].Vector[0].StartTime == mi && if2min == true) &&
                Loop[gg].Vector[0].VX1 == Loop[gg].Vector[0].VX2 && Loop[gg].Vector[0].VY1 == Loop[gg].Vector[0].VY2 &&
                Loop[gg].Vector[0].VX1 == 1 && Loop[gg].Vector[0].VY1 == 1 &&
                Vector.Count == 0)
            {
                Loop[gg].Vector.Remove(0);
                Loop[gg].Vector.ToNull(); Loop[gg].ToNull(); ToNull();//删除这个V
            }
            //加个条件 对第一行再判断。
            else if (Loop[gg].Vector.Count > 1 &&
                (Loop[gg].Vector[0].EndTime < mx || Loop[gg].Vector[0].EndTime == mx && if2max == true) &&
                (Loop[gg].Vector[0].StartTime > mi || Loop[gg].Vector[0].StartTime == mi && if2min == true) &&
                Loop[gg].Vector[0].VX2 == Loop[gg].Vector[0].VX1 && Loop[gg].Vector[0].VY2 == Loop[gg].Vector[0].VY1 &&
                Loop[gg].Vector[0].VX2 == Loop[gg].Vector[1].VX1 && Loop[gg].Vector[0].VY2 == Loop[gg].Vector[1].VY1 &&
                Loop[gg].Vector[0].VX1 == 1 && Loop[gg].Vector[0].VY1 == 1 &&
                Vector.Count == 0)
            {
                Loop[gg].Vector.Remove(0);
                Loop[gg].Vector.ToNull(); Loop[gg].ToNull(); ToNull();//删除这个V
            }

        }
        private void optC_L(int gg)
        {
            int i = Loop[gg].Color.Count - 1;
            while (i >= 1)
            {
                int? max2 = Loop[gg].MaxTime();
                int? min2 = Loop[gg].MinTime();
                bool if2min2 = Loop[gg].TwoMin;
                bool if2max2 = Loop[gg].TwoMax;

                /* 当 这个C的结束时间要小于obj的最大时间(或者是有两个以上的最大时间，其中包括C的)
                 * 且 就是一个静止的动作
                 * 且 这个动作与上一个末动作一样     
                 */
                if (
                   (Loop[gg].Color[i].EndTime < max2 || Loop[gg].Color[i].EndTime == max2 && if2max2 == true) &&
                   Loop[gg].Color[i].R1 == Loop[gg].Color[i].R2 && Loop[gg].Color[i].G1 == Loop[gg].Color[i].G2 && Loop[gg].Color[i].B1 == Loop[gg].Color[i].B2 &&
                   Loop[gg].Color[i].R1 == Loop[gg].Color[i - 1].R2 && Loop[gg].Color[i].G1 == Loop[gg].Color[i - 1].G2 && Loop[gg].Color[i].B1 == Loop[gg].Color[i - 1].B2)
                {
                    Loop[gg].Color.Remove(i); Loop[gg].Color.ToNull(); ToNull(); //删除这个C
                    i = Loop[gg].Color.Count - 1;
                }
                /* 当 这个C与前面的C一致，又是单关键帧的，而且又不满足上面的（这里就是指小于最大时间的）
                 */
                else if (Loop[gg].Color[i].R1 == Loop[gg].Color[i].R2 && Loop[gg].Color[i].G1 == Loop[gg].Color[i].G2 && Loop[gg].Color[i].B1 == Loop[gg].Color[i].B2 &&
                  Loop[gg].Color[i - 1].R1 == Loop[gg].Color[i - 1].R2 && Loop[gg].Color[i - 1].G1 == Loop[gg].Color[i - 1].G2 && Loop[gg].Color[i - 1].B1 == Loop[gg].Color[i - 1].B2 &&
                  Loop[gg].Color[i - 1].R1 == Loop[gg].Color[i].R1 && Loop[gg].Color[i - 1].G1 == Loop[gg].Color[i].G1 && Loop[gg].Color[i - 1].B1 == Loop[gg].Color[i].B1)
                {
                    Loop[gg].Color[i - 1].EndTime = Loop[gg].Color[i].EndTime; //整合到前面的
                    Loop[gg].Color.MoveEndTime(i);
                    if (Loop[gg].Color[i - 1].StartTime > min2 || Loop[gg].Color[i - 1].StartTime == min2 && if2min2 == true)
                    {
                        Loop[gg].Color[i - 1].StartTime = Loop[gg].Color[i - 1].EndTime; //整合到前面的
                        Loop[gg].Color.MoveStartTime(i);
                    }
                    Loop[gg].Color.Remove(i); Loop[gg].Color.ToNull(); ToNull();//删除这个C
                    i = Loop[gg].Color.Count - 1;
                }
                else i--;
            }

            /* 当 只有一个C的时候
             * 且 这个C的结束时间要小于obj的最大时间(或者是有两个以上的最大时间，其中包括C的)
             * 且 这个C的开始时间要大于obj的最小时间(或者是有两个以上的最小时间，其中包括C的)
             * 且 就是一个静止的物品
             * 且 RGB=255,255,255             
             */
            int? max = Loop[gg].MaxTime();
            int? min = Loop[gg].MinTime();
            bool if2min = Loop[gg].TwoMin;
            bool if2max = Loop[gg].TwoMax;
            //Console.WriteLine(Loop[gg].Color[0].EndTime < mx);
            if (Loop[gg].Color.Count == 1 &&
                (Loop[gg].Color[0].EndTime < max || Loop[gg].Color[0].EndTime == max && if2max == true) &&
                (Loop[gg].Color[0].StartTime > min || Loop[gg].Color[0].StartTime == min && if2min == true) &&
                Loop[gg].Color[0].R1 == Loop[gg].Color[0].R2 && Loop[gg].Color[0].G1 == Loop[gg].Color[0].G2 && Loop[gg].Color[0].B1 == Loop[gg].Color[0].B2 &&
                Loop[gg].Color[0].R1 == 255 && Loop[gg].Color[0].G1 == 255 && Loop[gg].Color[0].B1 == 255 &&
                Color.Count == 0)
            {
                Loop[gg].Color.Remove(0); Loop[gg].Color.ToNull(); ToNull();//删除这个C
            }
            //加个条件 对第一行再判断。
            else if (Loop[gg].Color.Count > 1 &&
              (Loop[gg].Color[0].EndTime < max || Loop[gg].Color[0].EndTime == max && if2max == true) &&
              (Loop[gg].Color[0].StartTime > min || Loop[gg].Color[0].StartTime == min && if2min == true) &&
               Loop[gg].Color[0].R2 == Loop[gg].Color[0].R1 && Loop[gg].Color[0].G2 == Loop[gg].Color[0].G1 && Loop[gg].Color[0].B2 == Loop[gg].Color[0].B1 &&
               Loop[gg].Color[0].R2 == Loop[gg].Color[1].R1 && Loop[gg].Color[0].G2 == Loop[gg].Color[1].G1 && Loop[gg].Color[0].B2 == Loop[gg].Color[1].B1 &&
               Loop[gg].Color[0].R1 == 255 && Loop[gg].Color[0].G1 == 255 && Loop[gg].Color[0].B1 == 255 &&
               Color.Count == 0)
            {
                Loop[gg].Color.Remove(0); Loop[gg].Color.ToNull(); ToNull();//删除这个C
            }
        }
        private void optMX_L(int gg)
        {
            int i = Loop[gg].MoveX.Count - 1;
            while (i >= 1)
            {
                int? max2 = Loop[gg].MaxTime();
                int? min2 = Loop[gg].MinTime();
                bool if2min2 = Loop[gg].TwoMin;
                bool if2max2 = Loop[gg].TwoMax;

                /* 当 这个MX的结束时间要小于obj的最大时间(或者是有两个以上的最大时间，其中包括MX的)
                 * 且 就是一个静止的动作
                 * 且 这个动作与上一个末动作一样     
                 */
                if ((Loop[gg].MoveX[i].EndTime < this.MaxTime() || Loop[gg].MoveX[i].EndTime == max2 && if2max2 == true) &&
                   Loop[gg].MoveX[i].X1 == Loop[gg].MoveX[i].X2 &&
                   Loop[gg].MoveX[i].X1 == Loop[gg].MoveX[i - 1].X2)
                {
                    Loop[gg].MoveX.Remove(i); Loop[gg].MoveX.ToNull(); ToNull();  //删除这个MX
                    i = Loop[gg].MoveX.Count - 1;
                }
                /* 当 这个MX与前面的MX一致，又是单关键帧的，而且又不满足上面的（这里就是指小于最大时间的）
                 */
                else if (Loop[gg].MoveX[i].X1 == Loop[gg].MoveX[i].X2 &&
                  Loop[gg].MoveX[i - 1].X1 == Loop[gg].MoveX[i - 1].X2 &&
                  Loop[gg].MoveX[i - 1].X1 == Loop[gg].MoveX[i].X1)
                {

                    Loop[gg].MoveX[i - 1].EndTime = Loop[gg].MoveX[i].EndTime; //整合到前面的
                    Loop[gg].MoveX.MoveEndTime(i);
                    if (Loop[gg].MoveX[i - 1].StartTime > min2 || Loop[gg].MoveX[i - 1].StartTime == min2 && if2min2 == true)
                    {
                        Loop[gg].MoveX[i - 1].StartTime = Loop[gg].MoveX[i - 1].EndTime;
                        Loop[gg].MoveX.MoveStartTime(i);
                    }
                    //Loop[gg].MoveX[i - 1].StartTime = Loop[gg].MoveX[i - 1].EndTime;
                    Loop[gg].MoveX.Remove(i); Loop[gg].MoveX.ToNull(); ToNull(); //删除这个MX
                    i = Loop[gg].MoveX.Count - 1;
                }
                else i--;
            }

            /* 当 只有一个MX的时候
             * 且 这个MX的结束时间要小于obj的最大时间(或者是有两个以上的最大时间，其中包括MX的)
             * 且 这个MX的开始时间要大于obj的最小时间(或者是有两个以上的最小时间，其中包括MX的)
             * 且 就是一个静止的物品
             * 且 MX和X一样
             */
            int? max = Loop[gg].MaxTime();
            int? min = Loop[gg].MinTime();
            bool if2min = Loop[gg].TwoMin;
            bool if2max = Loop[gg].TwoMax;
            //Console.WriteLine(Loop[gg].MoveX[0].EndTime < mx);
            if (Loop[gg].MoveX.Count == 1 &&
                (Loop[gg].MoveX[0].EndTime < max || Loop[gg].MoveX[0].EndTime == max && if2max == true) &&
                (Loop[gg].MoveX[0].StartTime > min || Loop[gg].MoveX[0].StartTime == min && if2min == true) &&
                Loop[gg].MoveX[0].X1 == Loop[gg].MoveX[0].X2 &&
                Loop[gg].MoveX[0].X1 == X &&
                MoveX.Count == 0)
            {
                Loop[gg].MoveX.Remove(0); Loop[gg].MoveX.ToNull(); ToNull();
            }
            //加个条件 对第一行再判断。
            else if (Loop[gg].MoveX.Count > 1 &&
                (Loop[gg].MoveX[0].EndTime < max || Loop[gg].MoveX[0].EndTime == max && if2max == true) &&
                (Loop[gg].MoveX[0].StartTime > min || Loop[gg].MoveX[0].StartTime == min && if2min == true) &&
                Loop[gg].MoveX[0].X1 == Loop[gg].MoveX[0].X2 &&
                Loop[gg].MoveX[0].X2 == Loop[gg].MoveX[1].X1 &&
                Loop[gg].MoveX[0].X1 == X &&
                MoveX.Count == 0)
            {
                Loop[gg].MoveX.Remove(0); Loop[gg].MoveX.ToNull(); ToNull();
            }
        }
        private void optMY_L(int gg)
        {
            int i = Loop[gg].MoveY.Count - 1;
            while (i >= 1)
            {
                int? max2 = Loop[gg].MaxTime();
                int? min2 = Loop[gg].MinTime();
                bool if2min2 = Loop[gg].TwoMin;
                bool if2max2 = Loop[gg].TwoMax;

                /* 当 这个MY的结束时间要小于obj的最大时间(或者是有两个以上的最大时间，其中包括MY的)
                 * 且 就是一个静止的动作
                 * 且 这个动作与上一个末动作一样     
                 */
                if ((Loop[gg].MoveY[i].EndTime < this.MaxTime() || Loop[gg].MoveY[i].EndTime == max2 && if2max2 == true) &&
                   Loop[gg].MoveY[i].Y1 == Loop[gg].MoveY[i].Y2 &&
                   Loop[gg].MoveY[i].Y1 == Loop[gg].MoveY[i - 1].Y2)
                {
                    Loop[gg].MoveY.Remove(i); Loop[gg].MoveY.ToNull(); ToNull();  //删除这个MY
                    i = Loop[gg].MoveY.Count - 1;
                }
                /* 当 这个MY与前面的MY一致，又是单关键帧的，而且又不满足上面的（这里就是指小于最大时间的）
                 */
                else if (Loop[gg].MoveY[i].Y1 == Loop[gg].MoveY[i].Y2 &&
                  Loop[gg].MoveY[i - 1].Y1 == Loop[gg].MoveY[i - 1].Y2 &&
                  Loop[gg].MoveY[i - 1].Y1 == Loop[gg].MoveY[i].Y1)
                {

                    Loop[gg].MoveY[i - 1].EndTime = Loop[gg].MoveY[i].EndTime; //整合到前面的
                    Loop[gg].MoveY.MoveEndTime(i);
                    if (Loop[gg].MoveY[i - 1].StartTime > min2 || Loop[gg].MoveY[i - 1].StartTime == min2 && if2min2 == true)
                    {
                        Loop[gg].MoveY[i - 1].StartTime = Loop[gg].MoveY[i - 1].EndTime;
                        Loop[gg].MoveY.MoveStartTime(i);
                    }
                    //Loop[gg].MoveY[i - 1].StartTime = Loop[gg].MoveY[i - 1].EndTime;
                    Loop[gg].MoveY.Remove(i); Loop[gg].MoveY.ToNull(); ToNull(); //删除这个MY
                    i = Loop[gg].MoveY.Count - 1;
                }
                else i--;
            }

            /* 当 只有一个MY的时候
             * 且 这个MY的结束时间要小于obj的最大时间(或者是有两个以上的最大时间，其中包括MY的)
             * 且 这个MY的开始时间要大于obj的最小时间(或者是有两个以上的最小时间，其中包括MY的)
             * 且 就是一个静止的物品
             * 且 MY和X一样
             */
            int? max = Loop[gg].MaxTime();
            int? min = Loop[gg].MinTime();
            bool if2min = Loop[gg].TwoMin;
            bool if2max = Loop[gg].TwoMax;
            //Console.WriteLine(Loop[gg].MoveY[0].EndTime < MY);
            if (Loop[gg].MoveY.Count == 1 &&
                (Loop[gg].MoveY[0].EndTime < max || Loop[gg].MoveY[0].EndTime == max && if2max == true) &&
                (Loop[gg].MoveY[0].StartTime > min || Loop[gg].MoveY[0].StartTime == min && if2min == true) &&
                Loop[gg].MoveY[0].Y1 == Loop[gg].MoveY[0].Y2 &&
                Loop[gg].MoveY[0].Y1 == Y &&
                MoveY.Count == 0)
            {
                Loop[gg].MoveY.Remove(0); Loop[gg].MoveY.ToNull(); ToNull();
            }
            //加个条件 对第一行再判断。
            else if (Loop[gg].MoveY.Count > 1 &&
                (Loop[gg].MoveY[0].EndTime < max || Loop[gg].MoveY[0].EndTime == max && if2max == true) &&
                (Loop[gg].MoveY[0].StartTime > min || Loop[gg].MoveY[0].StartTime == min && if2min == true) &&
                Loop[gg].MoveY[0].Y1 == Loop[gg].MoveY[0].Y2 &&
                Loop[gg].MoveY[0].Y2 == Loop[gg].MoveY[1].Y1 &&
                Loop[gg].MoveY[0].Y1 == Y &&
                MoveY.Count == 0)
            {
                Loop[gg].MoveY.Remove(0); Loop[gg].MoveY.ToNull(); ToNull();
            }
        }
    }
}
