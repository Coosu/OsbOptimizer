using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LibOSB
{
    static class SControl
    {
        static string backuproot = "";

        public static void WriteFile(string str, string root)
        {
            string newfile = Path.GetDirectoryName(root) + @"\" + Path.GetFileNameWithoutExtension(root) + ".osb";
            var sw = new StreamWriter(newfile);
            sw.Write(str);
            sw.Flush();
            sw.Close();
        }
        public static long GetFileLine(string root)
        {

            long currentLine = 0;
            var sr = new StreamReader(root);

            string Line = sr.ReadLine();
            do
            {
                currentLine++;

                Line = sr.ReadLine();

            } while (Line != null);

            return currentLine + 1;
        }

        //static string tmp = "";
    
        public static string BackupRoot { get => backuproot; set => backuproot = value; }

        public static void Backup(string root)
        {
            string tmproot;
            if (BackupRoot == "") tmproot = Path.GetDirectoryName(root);
            else tmproot = BackupRoot;

            string BackupFile = tmproot + @"\" + Path.GetFileNameWithoutExtension(root) + ".bak";
            if (Path.GetExtension(root).ToLower() == ".osb")
            {
                if (!File.Exists(BackupFile))
                {
                    File.Copy(root, BackupFile);
                    Reporter.BackupRoot = BackupFile;
                }
            }
        }
      
    }
}
