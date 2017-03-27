using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.NetworkInformation;

namespace LibOSB
{
    static class InfoCollector
    {
        private static bool ifCollectEx;
        private static bool ifCollectInfo;

        //static Thread tt;
        private static string DatabaseName = "OSBLib";
        private static string server = "127.0.0.1";
        private static string userName = "";
        private static string password = "";

        static MySqlConnection SqlConnection;

        public static bool IfCollectEx { get => ifCollectEx; set => ifCollectEx = value; }
        public static bool IfCollectInfo { get => ifCollectInfo; set => ifCollectInfo = value; }

        public static void UploadEx(Exception ex)
        {
            SqlConnection = new MySqlConnection();

            if (SqlConnection != null) SqlConnection.Close();
            SqlConnection.ConnectionString = string.Format("server={0}; user id={1}; password={2}; database={3}; pooling=false"
                      , server, userName, password, DatabaseName);
            SqlConnection.Open();

            MySqlTransaction Transaction = SqlConnection.BeginTransaction();
            
            string cmd = string.Format("insert into `ExInfo` values ('{0}','{1}','{2}','{3}')", ex.GetType(), ex.ToString(), getMacAddress(), ex.GetType() + ex.StackTrace);

            MySqlCommand Command = new MySqlCommand(cmd, SqlConnection, Transaction);
            Command.CommandType = System.Data.CommandType.Text;
            Command.ExecuteNonQuery();

            Transaction.Commit();

            Command = null;
            Transaction = null;
            SqlConnection.Close();
        }

        public static void UploadInfo()
        {
            if (Reporter.TotalSpriteNumber == 0) return;
            SqlConnection = new MySqlConnection();

            if (SqlConnection != null) SqlConnection.Close();
            SqlConnection.ConnectionString = string.Format("server={0}; user id={1}; password={2}; database={3}; pooling=false"
                , server, userName, password, DatabaseName);
            SqlConnection.Open();

            MySqlTransaction Transaction = SqlConnection.BeginTransaction();
            //
            string cmd = string.Format("insert into `OptimizeInfo` values ('{0}',{1},'{2}','{3}','{4}','{5}',{6},{7},{8},{9},{10},'{11}','{12}')",
                Reporter.ExecTime, (int)Reporter.RealtotalTime.TotalMilliseconds, Reporter.EndTime, Reporter.SourceFileRoot, Reporter.ObjectFileRoot,
                Reporter.BackupRoot, Reporter.SizeBefore, Reporter.SizeAfter, Reporter.TotalUnrecognizedLines, Reporter.TotalSpriteNumber, Reporter.OptimizedSpriteNumber,
                Reporter.ExceptionMessages.ToString(), getMacAddress());

            MySqlCommand Command = new MySqlCommand(cmd, SqlConnection, Transaction);
            Command.CommandType = System.Data.CommandType.Text;
            Command.ExecuteNonQuery();
            //
            Transaction.Commit();

            Command = null;
            Transaction = null;
            SqlConnection.Close();

            //if (tt != null && tt.ThreadState == ThreadState.Running) tt.Abort();
            //tt = new Thread(t);
            //tt.Start();
        }
        private static string getMacAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            return nics[0].GetPhysicalAddress().ToString();
        }

        private static void t()
        {

        }
    }
}
