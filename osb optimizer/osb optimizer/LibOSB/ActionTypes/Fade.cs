using LibOSB.ActionClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibOSB.ActionTypes
{
    class Fade : ActionSingle
    {
        public List<FadeOutTime> FadeOutList { get; set; } = new List<FadeOutTime>();
    }

}
