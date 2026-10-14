using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StudentManagerPro
{
    public partial class FrmStudentManage : Form, IMessageReceiver
    {
        public FrmStudentManage()
        {
            InitializeComponent();
            // 订阅学员变更事件
            Program.StudentChanged += OnStudentChanged;
            // 订阅群聊（实现 IMessageReceiver 接口），并广播自己已打开
            MessageCenter.Subscribe(this);
            MessageCenter.Broadcast("学员管理", "窗体已打开");
        }

        /// <summary>
        /// 接口实现：收到其他窗体的广播消息
        /// </summary>
        public void Receive(string senderName, string message)
        {
            // 忽略自己发的消息
            if (senderName == "学员管理") return;
            MessageBox.Show("[" + senderName + "] " + message, "学员管理收到消息");
        }

        /// <summary>
        /// 收到其他窗体添加学员通知后刷新列表
        /// </summary>
        private void OnStudentChanged(string studentName)
        {
            MessageBox.Show("学员【" + studentName + "】已添加，正在刷新列表...");
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            // 广播关闭消息，并取消订阅，避免内存泄漏
            MessageCenter.Broadcast("学员管理", "窗体已关闭");
            Program.StudentChanged -= OnStudentChanged;
            MessageCenter.Unsubscribe(this);
            base.OnFormClosed(e);
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void FrmStudentManage_Load(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }
    }
}
