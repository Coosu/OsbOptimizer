using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using LibOSB.Constants;

namespace LibOSB
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        //[MTAThread]
        static void Main()
        {
            //System.Threading.Mutex mutex = new System.Threading.Mutex(true, "OnlyRunOneInstance", out bool isRuned);
            //if (isRuned)
            {
                /* To do:
                    * 1.预查错
                    * * 其他的慢慢增加
                    * hide area：在 Fade=0 时间区间内其他的命令除每个最迟的那个都删除
                    */
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmMain());
            }
        }

    }
}
