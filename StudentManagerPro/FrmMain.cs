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
    public partial class FrmMain : Form, IMessageReceiver
    {
        public FrmMain()
        {
            InitializeComponent();

            // string connString =DAL.Helper.SQLHelper.connString;
            // connString = Common.StringSecurity.EncryptDES(connString);

            //显示登录用户名
            this.lblCurrentUser.Text = Program.objCurrentAdmin.AdminName+"】";

            // 订阅群聊（实现 IMessageReceiver 接口），并广播自己已打开
            MessageCenter.Subscribe(this);
            MessageCenter.Broadcast("主窗体", "窗体已打开");
        }

        /// <summary>
        /// 接口实现：收到其他窗体的广播消息
        /// </summary>
        public void Receive(string senderName, string message)
        {
            // 忽略自己发的消息
            if (senderName == "主窗体") return;
            MessageBox.Show("[" + senderName + "] " + message, "主窗体收到消息");
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            // 广播关闭消息，并取消订阅
            MessageCenter.Broadcast("主窗体", "窗体已关闭");
            MessageCenter.Unsubscribe(this);
            base.OnFormClosed(e);
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {

        }

        private void 系统ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }


        #region 关闭其他窗体打开窗体
        private void ClosePreForm()
        {
            foreach (Control item in this.spContainer.Panel2.Controls)
            {
                if (item is Form)
                {
                    Form objControl = (Form)item;
                    objControl.Close();
                }
            }
        }

        private void OpenForm(Form objFrm)
        {
            objFrm.TopLevel = false;
            objFrm.WindowState = FormWindowState.Maximized;
            objFrm.Parent = this.spContainer.Panel2;
            objFrm.Show();
        }
        #endregion

        private void btnAddStudent_Click(object sender, EventArgs e)
        {
            ClosePreForm();
            OpenForm(new FrmAddStudent());
            //FrmAddStudent objFrm = new FrmAddStudent();
            
        }

        private void btnStudentManage_Click(object sender, EventArgs e)
        {
            ClosePreForm();
            OpenForm(new FrmStudentManage());
        }

        public static FrmAddStudent objFrmAddStudent = null;
        private void tsmiAddStudent_Click(object sender, EventArgs e)
        {
            if(objFrmAddStudent == null)
            {
                objFrmAddStudent = new FrmAddStudent();
                objFrmAddStudent.Show();
            }
            else 
            {
                objFrmAddStudent.Activate();
                objFrmAddStudent.WindowState = FormWindowState.Normal;
            }
            

        }

        private void tsbExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("确定退出吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if(result != DialogResult.OK)
            {
                e.Cancel = true;
            }
        }
    }
}
