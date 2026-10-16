using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DAL;
using Models;
using DAL.Helper;

namespace StudentManagerPro
{
    public partial class FrmStudentManage : Form, IMessageReceiver
    {
        // 学员服务
        private StudentService objStudentService = new StudentService();
        private StudentClassService objClassService = new StudentClassService();

        public FrmStudentManage()
        {
            InitializeComponent();
            // 班级服务
            this.cboClass.DataSource = objClassService.GetAllClasses();
            this.cboClass.DisplayMember = "ClassName";
            this.cboClass.ValueMember = "ClassId";
            this.cboClass.SelectedIndex = -1;
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

        private void button4_Click(object sender, EventArgs e)
        {
        }

        private void testWeiTuo_Click(object sender, EventArgs e)
        {
            // 3. 触发：调用全局事件发数据，所有订阅者都能收到
            Program.SendData(testWeiTuo.Text);
            this .Close();

        }

        //按照班级查询
        private void btnQuery_Click(object sender, EventArgs e)
        {
            // 检查是否选择了班级
            if (this.cboClass.SelectedIndex == -1)
            {
                MessageBox.Show("请选择班级");
                return;
            }

            this.dgvStudent.AutoGenerateColumns = false;
            // 查询班级下的所有学员
            List<StudentExt> list = objStudentService.GetStudentByClassName(this.cboClass.Text);
            // 显示查询结果
            this.dgvStudent.DataSource = list;
        }

        private void dgvStudent_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
