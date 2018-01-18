using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibOSB.ActionTypes
{
    class Parameter : Action
    {
        public Parameter this[int index] { get => P[index]; }

        public Parameter(byte easing, int starttime, int endtime,
         string ptype, int? i, int? j)
        {
            type = "P";
            this.easing = easing;
            this.startTime = starttime;
            this.endTime = endtime;
            this.pType = ptype;
            indexL = i;
            indexT = j;
            BuildParams();
        }

        private void BuildParams()
        {
            @params = pType;
        }

        public Parameter() { }

        private List<Parameter> P = new List<Parameter>();
        private string pType;
        public string Ptype { get => pType; }
        public void Add(byte Easing, int StartTime, int EndTime,
         string PType)
        {
            if (Easing < 0 || Easing > 34) throw new Exception("Unknown Easing.");
            P.Add(new Parameter(Easing, StartTime, EndTime, PType, indexL, indexT));
            startTime_L.Add(StartTime);
            endTime_L.Add(EndTime);
        }
    }

}
