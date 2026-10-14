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
    public partial class FrmStudentManage : Form
    {
        /// <summary>
        /// 定义委托：接收学员姓名，用于跨窗体通知
        /// </summary>
        public delegate void StudentChangedHandler(string studentName);

        /// <summary>
        /// 静态事件：供其他窗体（如 FrmAddStudent）触发，通知本窗体刷新
        /// </summary>
        public static event StudentChangedHandler StudentChanged;

        /// <summary>
        /// 供其他窗体调用，触发学员变更事件
        /// </summary>
        public static void NotifyStudentChanged(string studentName)
        {
            if (StudentChanged != null)
            {
                StudentChanged(studentName);
            }
        }

        public FrmStudentManage()
        {
            InitializeComponent();
            // 订阅静态事件
            StudentChanged += OnStudentChanged;
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
            // 取消订阅，避免内存泄漏
            StudentChanged -= OnStudentChanged;
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
