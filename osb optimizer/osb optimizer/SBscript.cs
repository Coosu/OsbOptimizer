using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Vsa;
using Microsoft.JScript;

namespace LibOSB
{
    class SBscript
    {
        List<string> name;
        List<string> value;
        public SBscript()
        {
            name = new List<string>();
            value = new List<string>();
        }
        public string Check(string str, int line)
        {
            bool error;
            object result = sControls.Eval(str, out error);
            if (error) throw new Exception(str);
            if (result != null) return result.ToString();
            else return "";
            #region 暂时无用
            if (str.IndexOf("var ") == 0 && str.LastIndexOf("var ") == 0)
            {
                List<int> oper_equ = FindAllindex(str, "=");
                if (oper_equ.Count == 1)
                {
                    string compare = str.Substring(3, str.Length - 3).Trim();
                    string[] tmp = compare.Split('=');
                    if (tmp[0].Trim().IndexOf(" ") != -1) Error(line);
                    string CallEval = Eval(tmp[1]).ToString(); ;
                    if (CallEval.Trim().IndexOf(" ") != -1) Error(line);
                    if (!CanToVal(CallEval.Trim())) Error(line);
                    name.Add(tmp[0].Trim());
                    value.Add(CallEval.Trim());
                }
                else
                {
                    Error(line);
                }
                return "";
            }
            else if (str.IndexOf(" ") == -1)
            {
                int index;
                index = name.FindIndex(delegate (string s) { return s == str; });
                if (index != -1) return value[index];
            }
            //List<int> oper_plus = FindAllindex(str, "+");
            Error(line);
            return "";
            #endregion
        }
        private bool CanToVal(string str)
        {
            try { double a = double.Parse(str); return true; }
            catch { return false; }
        }
        private void Error(int line)
        {
            throw new Exception("syntax error on line: " + line);
        }
        private List<int> FindAllindex(string str, string c_str)
        {
            int index = -2;
            int i = 0;
            var list = new List<int>();
            while (index != -1)
            {
                index = str.IndexOf(c_str, i);
                if (index != -1) list.Add(index);
                i = index + 1;
            }
            if (list.Count == 0) list.Add(-1);
            return list;
        }
        private object Eval(string s)
        {
            Microsoft.JScript.Vsa.VsaEngine ve = Microsoft.JScript.Vsa.VsaEngine.CreateEngine();
            return Microsoft.JScript.Eval.JScriptEvaluate(s, ve);

        }
    }
    class ScriptObject
    {
        private string name;
        private string value;

        public string Name { get => name; set => name = value; }

        public string Value { get => value; set => this.value = value; }

        public ScriptObject(string name, string value)
        {
            this.name = name;
            this.value = value;
        }
    }
}
