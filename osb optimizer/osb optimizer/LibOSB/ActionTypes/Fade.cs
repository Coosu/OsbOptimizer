using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibOSB.ActionTypes
{
    class Fade : Actions
    {
        public Fade this[int index]
        {
            get => F[index];
        }
        public Fade(byte easing, int starttime, int endtime,
         double F1, double F2, int? i, int? j)
        {
            type = "F";
            this.easing = easing;
            this.startTime = starttime;
            this.endTime = endtime;
            this.f1 = F1;
            this.f2 = F2;
            indexL = i;
            indexT = j;
            BuildParams();
        }

        private void BuildParams()
        {
            if (f1 == f2) @params = f1.ToString();
            else @params = f1 + "," + f2;
        }

        public Fade() { }
        public void Remove(int index)
        {
            F.Remove(F[index]);
            starttime_L.RemoveAt(index);
            endtime_L.RemoveAt(index);
        }

        public List<Fade> F = new List<Fade>();
        private double f1, f2;
        public double F1 { get => f1; }
        public double F2 { get => f2; }
        public void Add(byte Easing, int StartTime, int EndTime,
         double Fade_1, double Fade_2)
        {
            if (Easing < 0 || Easing > 34) throw new Exception("Unknown Easing.");
            F.Add(new Fade(Easing, StartTime, EndTime, Fade_1, Fade_2, indexL, indexT));
            starttime_L.Add(StartTime);
            endtime_L.Add(EndTime);
        }
    }

}
