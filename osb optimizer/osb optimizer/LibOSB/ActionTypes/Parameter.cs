using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibOSB.ActionTypes
{
    class Parameter : Actions
    {
        public Parameter this[int index]
        {
            get { return P[index]; }
        }
        public Parameter(byte easing, int starttime, int endtime,
         string ptype, int? i, int? j)
        {
            type = "P";
            this.easing = easing;
            this.startTime = starttime;
            this.endTime = endtime;
            this.ptype = ptype;
            indexL = i;
            indexT = j;
            BuildParams();
        }

        private void BuildParams()
        {
            @params = ptype;
        }

        public Parameter() { }

        private List<Parameter> P = new List<Parameter>();
        private string ptype;
        public string Ptype { get { return ptype; } }
        public void Add(byte Easing, int StartTime, int EndTime,
         string PType)
        {
            if (Easing < 0 || Easing > 34) throw new Exception("Unknown Easing.");
            P.Add(new Parameter(Easing, StartTime, EndTime, PType, indexL, indexT));
            starttime_L.Add(StartTime);
            endtime_L.Add(EndTime);
        }
    }

}
