using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibOSB
{
    static class Reporter
    {
        private static DateTime execTime;
        private static TimeSpan realtotalTime;
        private static DateTime endTime;

        private static string sourcefileroot;
        private static string objectfileroot;
        private static string backuproot = null;

        private static long sizeBefore, sizeAfter;

        private static int totalUnrecognizedLines;
        private static int totalSpriteNumber;
        private static int optimizedSpriteNumber;

        public static ExceptionMessages ExceptionMessages = new ExceptionMessages();

        public static long SizeBefore { get => sizeBefore; set => sizeBefore = value; }
        public static long SizeAfter { get => sizeAfter; set => sizeAfter = value; }
        public static int TotalUnrecognizedLines { get => totalUnrecognizedLines; set => totalUnrecognizedLines = value; }
        public static int TotalSpriteNumber { get => totalSpriteNumber; set => totalSpriteNumber = value; }
        public static int OptimizedSpriteNumber { get => optimizedSpriteNumber; set => optimizedSpriteNumber = value; }
        public static DateTime ExecTime { get => execTime; set => execTime = value; }
        public static TimeSpan RealtotalTime { get => realtotalTime; set => realtotalTime = value; }
        public static DateTime EndTime { get => endTime; set => endTime = value; }
        public static string SourceFileRoot { get => sourcefileroot; set => sourcefileroot = value; }
        public static string ObjectFileRoot { get => objectfileroot; set => objectfileroot = value; }
        public static string BackupRoot { get => backuproot; set => backuproot = value; }

        public static void Clear()
        {
            execTime = new DateTime();
            realtotalTime = new TimeSpan();
            endTime = new DateTime();

            sourcefileroot = null;
            objectfileroot = null;
            backuproot = null;

            sizeBefore = 0; sizeAfter = 0;

            TotalUnrecognizedLines = 0;
            totalSpriteNumber = 0;
            optimizedSpriteNumber = 0;
            ExceptionMessages = new ExceptionMessages();
        }

        public static string ToString(bool detail)
        {
            var sb = new StringBuilder();

            if (!detail)
            {
                sb.AppendLine("Optimization report:");
                sb.AppendLine();
                sb.AppendLine("[Plan Info]");
                sb.AppendLine("  Start Time: " + execTime.ToString() + "." + execTime.Millisecond);
                sb.AppendLine("  End Time: " + endTime.ToString() + "." + endTime.Millisecond);
                sb.AppendLine();
                sb.AppendLine("  Source File: " + sourcefileroot);
                sb.AppendLine("  Object File: " + objectfileroot);
                if (backuproot != null) sb.AppendLine("  Backup File: " + backuproot);
                sb.AppendLine();
                sb.AppendLine("[Optimization Info]");
                sb.AppendLine("  Size Befoe: " + sizeBefore);
                sb.AppendLine("  Size After: " + sizeAfter);
                sb.AppendLine("  Compression Ratio: " + Math.Round((double)sizeAfter / sizeBefore * 100, 2) + "%");
                sb.AppendLine();
                sb.AppendLine("  Used Time: " + Math.Round(realtotalTime.TotalSeconds, 2) + "s");
                sb.AppendLine();
                sb.AppendLine("  Total " + totalSpriteNumber + " objects, optimized " + optimizedSpriteNumber + " objects.");
                sb.AppendLine("  Error Lines: " + totalUnrecognizedLines);
            }
            else
            {
                return ExceptionMessages.ToString();
            }
            return sb.ToString();
        }
    }
    class ExceptionMessages
    {
        private int exceptionLine;
        private string exception;
        private DateTime exceptionTime;

        public ExceptionMessages this[int index] { get => ex[index]; }

        public ExceptionMessages() { }
        public ExceptionMessages(int ExceptionLine, string Exception, DateTime ExceptionTime)
        {
            exceptionLine = ExceptionLine;
            exception = Exception;
            exceptionTime = ExceptionTime;
        }

        private List<ExceptionMessages> ex = new List<ExceptionMessages>();

        public int ExceptionLine { get => exceptionLine; }
        public string Exception { get => exception; }
        public DateTime ExceptionTime { get => exceptionTime; }

        public void Add(int ExceptionLine, string Exception, DateTime ExceptionTime)
        {
            ex.Add(new ExceptionMessages(ExceptionLine, Exception, ExceptionTime));
        }

        public new string ToString()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < ex.Count; i++)
            {
                sb.AppendLine("{");
                sb.AppendLine("    Time: " + ex[i].exceptionTime + "." + ex[i].exceptionTime.Millisecond);
                sb.AppendLine("    Error Line: " + ex[i].exceptionLine);
                sb.AppendLine("    Error Info: " + ex[i].exception);
                sb.AppendLine("}");
            }
            return sb.ToString();
        }
    }
}
