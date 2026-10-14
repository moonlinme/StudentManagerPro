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

        #region 全局事件总线（跨窗体通信，任意窗体可订阅/发布）
        /// <summary>
        /// 委托：接收学员姓名
        /// </summary>
        public delegate void StudentChangedHandler(string studentName);

        /// <summary>
        /// 静态事件：学员变更通知，任意窗体都可订阅
        /// </summary>
        public static event StudentChangedHandler StudentChanged;

        /// <summary>
        /// 触发学员变更事件（由发布方窗体调用）
        /// </summary>
        public static void NotifyStudentChanged(string studentName)
        {
            if (StudentChanged != null)
            {
                StudentChanged(studentName);
            }
        }
        #endregion
    }

    /// <summary>
    /// 订阅者接口：参与“图对话”的窗体都实现它（面向接口编程，靠多态统一调用）
    /// </summary>
    public interface IMessageReceiver
    {
        void Receive(string senderName, string message);
    }

    /// <summary>
    /// 消息中心（被观察者/Subject）：用接口多态统一管理所有订阅者
    /// </summary>
    public class MessageCenter
    {
        private static List<IMessageReceiver> receivers = new List<IMessageReceiver>();

        public static void Subscribe(IMessageReceiver receiver)
        {
            receivers.Add(receiver);
        }

        public static void Unsubscribe(IMessageReceiver receiver)
        {
            receivers.Remove(receiver);
        }

        public static void Broadcast(string senderName, string message)
        {
            foreach (IMessageReceiver receiver in receivers)
            {
                receiver.Receive(senderName, message);
            }
        }
    }
}
