using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OsuStoryboard.OSBOptimizer
{
    class Variables
    {
        public static List<Variables_obj> lst = new List<Variables_obj>();
        static public void AddRoot(string name)
        {
            for (int i = 0; i < lst.Count; i++)
            {
                if (lst[i].Name == name)
                {
                    lst[i].Count++;
                    return;
                }
            }
            lst.Add(new Variables_obj( name , 1));
        }
    }
    class Variables_obj
    {
        string name;
        int count;

        public Variables_obj(string name, int count)
        {
            Count = count;
            Name = name;
        }
        new public string ToString()
        {
            return "(" + count + ") " + Name;
        }
        public string Name { get => name; set => name = value; }
        public int Count { get => count; set => count = value; }
    }
}
