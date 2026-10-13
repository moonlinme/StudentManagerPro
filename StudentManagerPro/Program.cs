using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace StudentManagerPro
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //创建登录窗体
            FrmUserLogin objFrmLogin = new FrmUserLogin();
            DialogResult dialogResult = objFrmLogin.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                Application.Run(new FrmMain());
            }
            else
            {
                Application.Exit();
            }
        }
        // 定义一个全局变量保存用户对象
        public static Models.SysAdmin objCurrentAdmin = null;
    }
}
