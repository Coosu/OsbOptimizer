using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace LibOSB
{
    class UpdateInfo
    {
        private DateTime datetime;
        private string infomation;
        private string link;
        private string version;

        public UpdateInfo(DateTime datetime, string infomation, string link, string version)
        {
            this.datetime = datetime;
            this.infomation = infomation;
            this.link = link;
            this.version = version;
        }

        public DateTime Datetime { get => datetime; }
        public string Infomation { get => infomation; }
        public string Link { get => link; }
        public string Version { get => version; }
    }

    static class Client
    {

        static string IPstring = "123.207.137.177";
        //static string IPstring = "10.128.143.236";
        //static string IPstring = "192.168.145.1";
        static int Port = 12345;

        static Thread ThreadClient = null; //负责监听客户端的线程
        static Socket SocketClient = null; //负责监听客户端的套接字

        private static UpdateInfo updateInfo;
        private static bool isFailed = false;

        public static UpdateInfo UpdateInfo { get => updateInfo; }
        public static bool IsFailed { get => isFailed; set => isFailed = value; }
        private static int firstcheck;
        public static void StartListen(int firstcheck)
        {
            Client.firstcheck = firstcheck;

            //定义一个套字节监听  包含3个参数(IP4寻址协议,流式连接,TCP协议)
            SocketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //需要获取文本框中的IP地址
            IPAddress IP = IPAddress.Parse(IPstring);
            //将IP地址和端口号绑定到网络节点endpoint上 
            IPEndPoint EndPoint = new IPEndPoint(IP, Port); //获取文本框上输入的端口号
            //这里客户端套接字连接到网络节点(服务端)用的方法是Connect 而不是Bind
            SocketClient.Connect(EndPoint);
            //创建一个线程 用于监听服务端发来的消息
            ThreadClient = new Thread(RecMsg)
            {
                //将窗体线程设置为与后台同步
                IsBackground = true
            };
            //启动线程
            ThreadClient.Start();
        }

        private static void RecMsg()
        {
            while (true) //持续监听服务端发来的消息
            {
                try
                {
                    //定义一个1M的内存缓冲区 用于临时性存储接收到的信息
                    byte[] arrRecMsg = new byte[1024 * 1024];
                    //将客户端套接字接收到的数据存入内存缓冲区, 并获取其长度
                    int length = SocketClient.Receive(arrRecMsg);
                    //将套接字获取到的字节数组转换为人可以看懂的字符串
                    //string strRecMsg = Convert.ToString(arrRecMsg);
                    string strRecMsg = Encoding.Unicode.GetString(arrRecMsg, 0, length);
                    if (strRecMsg != "How are you?")
                    {
                        updateInfo = ToObject(strRecMsg);
                        ClientSendMsg("closeconnection");
                        SocketClient.Close();
                        ThreadClient.Abort();
                    }
                    else
                    {
                        ClientSendMsg("updatereq{!}" + InfoCollector.GetMacAddress() + "{!}" + InfoCollector.GetIP() + "{!}" + firstcheck);
                    }
                }
                catch { }
            }
        }

        private static UpdateInfo ToObject(string msg)
        {
            string[] infos = System.Text.RegularExpressions.Regex.Split(msg, "{!}", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            if (infos.Length != 4)
            {
                isFailed = true;
                return null;

            }
            var tmp = new UpdateInfo(DateTime.Parse(infos[0]), infos[1], infos[2], infos[3]);

            return tmp;
        }

        /// <summary>
        /// 发送字符串信息到服务端的方法
        /// </summary>
        /// <param name="sendMsg">发送的字符串信息</param>
        private static void ClientSendMsg(string sendMsg)
        {
            //将输入的内容字符串转换为机器可以识别的字节数组
            byte[] arrClientSendMsg = Encoding.Unicode.GetBytes(sendMsg);
            //调用客户端套接字发送字节数组
            SocketClient.Send(arrClientSendMsg);
            //将发送的信息追加到聊天内容文本框中
            //txtMsg.AppendText("天之涯:" + GetCurrentTime() + "\r\n" + sendMsg + "\r\n");
        }

    }

    static class InfoCollector
    {
        public static string GetIP()
        {
            string tempip = "Unknown";
            string temploc = "Unknown";
            try
            {

                WebRequest wr = WebRequest.Create("https://www.ipip.net/ip.html");
                Stream s = wr.GetResponse().GetResponseStream();
                StreamReader sr = new StreamReader(s, Encoding.UTF8);
                string all = sr.ReadToEnd(); //读取网站的数据

                int start = all.IndexOf("您的当前IP:");
                start = all.IndexOf(">", start);
                int end = all.IndexOf("<", start);
                tempip = all.Substring(start + 1, end - start - 1);
                start = all.IndexOf("<span id=\"myself\">", start);
                start = all.IndexOf(">", start);
                end = all.IndexOf("<", start);
                temploc = all.Substring(start + 1, end - start - 1).Trim('\r').Trim('\n').Trim();
                sr.Close();
                s.Close();
            }
            catch
            {
            }
            return string.Format("{0} ({1})", tempip, temploc);
        }
        //    private static bool ifCollectEx;
        //    private static bool ifCollectInfo;

        //    //static Thread tt;
        //    private static string DatabaseName = "OSBLib";

        //    static MySqlConnection SqlConnection;

        //    public static bool IfCollectEx { get => ifCollectEx; set => ifCollectEx = value; }
        //    public static bool IfCollectInfo { get => ifCollectInfo; set => ifCollectInfo = value; }

        //    public static UpdateInfo Update()
        //    {
        //        UpdateInfo update = null;
        //        SqlConnection = new MySqlConnection();

        //        if (SqlConnection != null) SqlConnection.Close();
        //        SqlConnection.ConnectionString = string.Format("server={0}; user id={1}; password={2}; database={3}; pooling=false"
        //                  , server, userName, password, DatabaseName);
        //        SqlConnection.Open();

        //        string cmd = string.Format("select * from updateinfo order by datetime desc limit 0,1;");

        //        MySqlDataReader DataReader;
        //        MySqlCommand Command = new MySqlCommand(cmd, SqlConnection)
        //        {
        //            CommandType = System.Data.CommandType.Text
        //        };
        //        DataReader = Command.ExecuteReader();

        //        if (DataReader.HasRows)
        //        {
        //            int i = 0;
        //            while (DataReader.Read())
        //            {
        //                update = new UpdateInfo((DateTime)DataReader[0], DataReader[1].ToString(), DataReader[2].ToString(), DataReader[3].ToString());
        //                i++;
        //            }
        //        }

        //        Command = null;
        //        DataReader = null;
        //        SqlConnection.Close();

        //        return update;
        //    }

        //    public static void UploadEx(Exception ex)
        //    {
        //        SqlConnection = new MySqlConnection();

        //        if (SqlConnection != null) SqlConnection.Close();
        //        SqlConnection.ConnectionString = string.Format("server={0}; user id={1}; password={2}; database={3}; pooling=false"
        //                  , server, userName, password, DatabaseName);
        //        SqlConnection.Open();

        //        MySqlTransaction Transaction = SqlConnection.BeginTransaction();

        //        string cmd = string.Format("insert into `ExInfo` values ('{0}','{1}','{2}','{3}')", ex.GetType(), ex.ToString(), getMacAddress(), ex.GetType() + ex.StackTrace);

        //        MySqlCommand Command = new MySqlCommand(cmd, SqlConnection, Transaction) { CommandType = System.Data.CommandType.Text };
        //        Command.ExecuteNonQuery();

        //        Transaction.Commit();

        //        Command = null;
        //        Transaction = null;
        //        SqlConnection.Close();
        //    }

        //    public static void UploadInfo()
        //    {
        //        if (Reporter.TotalSpriteNumber == 0) return;
        //        SqlConnection = new MySqlConnection();

        //        if (SqlConnection != null) SqlConnection.Close();
        //        SqlConnection.ConnectionString = string.Format("server={0}; user id={1}; password={2}; database={3}; pooling=false"
        //            , server, userName, password, DatabaseName);
        //        SqlConnection.Open();

        //        MySqlTransaction Transaction = SqlConnection.BeginTransaction();
        //        //
        //        string cmd = string.Format("insert into `OptimizeInfo` values ('{0}',{1},'{2}','{3}','{4}','{5}',{6},{7},{8},{9},{10},'{11}','{12}')",
        //            Reporter.ExecTime, (int)Reporter.RealtotalTime.TotalMilliseconds, Reporter.EndTime, Reporter.SourceFileRoot, Reporter.ObjectFileRoot,
        //            Reporter.BackupRoot, Reporter.SizeBefore, Reporter.SizeAfter, Reporter.TotalUnrecognizedLines, Reporter.TotalSpriteNumber, Reporter.OptimizedSpriteNumber,
        //            Reporter.ExceptionMessages.ToString(), getMacAddress());

        //        MySqlCommand Command = new MySqlCommand(cmd, SqlConnection, Transaction) { CommandType = System.Data.CommandType.Text };

        //        Command.ExecuteNonQuery();
        //        //
        //        Transaction.Commit();

        //        Command = null;
        //        Transaction = null;
        //        SqlConnection.Close();

        //   }

        public static string GetMacAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            return nics[0].GetPhysicalAddress().ToString();
        }

    }
}
