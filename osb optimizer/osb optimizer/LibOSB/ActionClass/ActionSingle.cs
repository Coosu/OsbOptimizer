using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibOSB.ActionClass
{
    class ActionSingle : Action
    {
        public ActionSingle this[int index]
        {
            get => Single[index];
        }

        public List<ActionSingle> Single = new List<ActionSingle>();
        double paramPre, paramPost;

        public double ParamPre { get => paramPre; }
        public double ParamPost { get => paramPost; }

        public ActionSingle() { }
        public ActionSingle(byte easing, int startTime, int endTime, double param1, double param2, int? i, int? j)
        {
            type = "F";
            this.easing = easing;
            this.startTime = startTime;
            this.endTime = endTime;
            paramPre = param1;
            paramPost = param2;
            indexL = i;
            indexT = j;
            BuildParams();
        }

        private void BuildParams()
        {
            if (ParamPre == ParamPost) @params = ParamPre.ToString();
            else @params = ParamPre + "," + ParamPost;
        }
        public void Add(byte easing, int startTime, int endTime, double param1, double param2)
        {
            //if (easing < 0 || easing > 34) throw new Exception("Unknown Easing.");
            Single.Add(new ActionSingle(easing, startTime, endTime, param1, param2, indexL, indexT));
            startTime_L.Add(startTime);
            endTime_L.Add(endTime);
        }

        public void Remove(int index)
        {
            Single.Remove(Single[index]);
            startTime_L.RemoveAt(index);
            endTime_L.RemoveAt(index);
        }
    }
}
