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

        private void btnQueryById_Click(object sender, EventArgs e)
        {
            //
            if(this.txtStudentId.Text.Trim() == "")
            {
                MessageBox.Show("请输入学员ID");
                this.txtStudentId.Focus();
                return;
            }
            this.dgvStudent.AutoGenerateColumns = false;
            StudentExt oneStudent = objStudentService.GetStudentById(Convert.ToInt32(this.txtStudentId.Text));
            // 显示查询结果
            List<StudentExt> list = new List<StudentExt>();
            list.Add(oneStudent);
            this.dgvStudent.DataSource = list;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void btnEidt_Click(object sender, EventArgs e)
        {
            // 检查是否选择了学员
            if (this.dgvStudent.RowCount == 0)
            {
                MessageBox.Show("请先查询学员");
                return;
            }
            if (this.dgvStudent.CurrentRow==null)
            {
                MessageBox.Show("请选择要修改的学员");
                return;
            }

            // 获取选中的学员Id
            string studentId = this.dgvStudent.CurrentRow.Cells["StudentId"].Value.ToString();

            // 查询学员详情
            StudentExt objStudent = objStudentService.GetStudentById(Convert.ToInt32(studentId));

            //显示要修改的学员信息窗口
            FrmEditStudent frmEditStudent = new FrmEditStudent(objStudent);
            DialogResult result = frmEditStudent.ShowDialog();

            //判断修改是否成功
            if(result == DialogResult.OK)
            {
                // 刷新列表
                btnQuery_Click(null, null);
            }




            
        }

        private void tsmiModifyStu_Click(object sender, EventArgs e)
        {
            //
            btnEidt_Click(null,null);
        }

        private void dgvStudent_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //
            if (this.dgvStudent.CurrentRow!=null)
            {
                // 防止双击表头时出错
            if (e.RowIndex < 0) return; 

            // 直接复用修改学员按钮的逻辑
            btnEidt_Click(null, null);
            }
        }
    }
}
