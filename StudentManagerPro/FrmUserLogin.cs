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

namespace StudentManagerPro
{
    public partial class FrmUserLogin : Form, IMessageReceiver
    {
        // 创建数据访问类对象
        private SysAdminService objAdminService = new SysAdminService();
        public FrmUserLogin()
        {
            InitializeComponent();
            // 订阅群聊（实现 IMessageReceiver 接口），并广播自己已打开
            MessageCenter.Subscribe(this);
            MessageCenter.Broadcast("登录窗体", "窗体已打开");
        }

        /// <summary>
        /// 接口实现：收到其他窗体的广播消息
        /// </summary>
        public void Receive(string senderName, string message)
        {
            // 忽略自己发的消息
            if (senderName == "登录窗体") return;
            MessageBox.Show("[" + senderName + "] " + message, "登录窗体收到消息");
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            // 广播关闭消息，并取消订阅
            MessageCenter.Broadcast("登录窗体", "窗体已关闭");
            MessageCenter.Unsubscribe(this);
            base.OnFormClosed(e);
        }

        private void FrmUserLogin_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            // 1.数据验证
           if(this.txtLoginId.Text.Trim().Length == 0)
           {
               MessageBox.Show("请输入登录账号");
               this.txtLoginId.Focus();
               return;
           }
           if(this.txtLoginPwd.Text.Trim().Length == 0)
           {
               MessageBox.Show("请输入登录密码");
               this.txtLoginPwd.Focus();
               return;
           }
           // 2.封装对象
           SysAdmin objAdmin = new SysAdmin();
           {
                objAdmin.LoginId = Convert.ToInt32(this.txtLoginId.Text.Trim());
                objAdmin.LoginPwd = this.txtLoginPwd.Text.Trim();
           }
           // 3.和后台交互，判断登录信息是否正确
           try
           {
               objAdmin = objAdminService.AdminLogin(objAdmin);
               if(objAdmin != null)
               {
                   // 保存登录信息
                   Program.objCurrentAdmin = objAdmin;
                   MessageBox.Show("登录成功");
                   this.DialogResult = DialogResult.OK;
                   this.Close();
               }
               else
               {
                   MessageBox.Show("登录失败-账号或密码错误");
               }
           }
           catch (Exception ex)
           {
               MessageBox.Show("登录失败——" + ex.Message);
               return;
           }


        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            // 关闭登录窗体
            // this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        #region 改善用户体验
        private void txtLoginId_KeyDown(object sender, KeyEventArgs e)
        {
            //
            if(e.KeyValue == 13)
            {
                if(this.txtLoginId.Text.Trim().Length != 0)
                {
                    this.txtLoginId.Focus();
                }
            }
        }
         private void txtLoginPwd_KeyDown(object sender, KeyEventArgs e)
        {
            //
            if(e.KeyValue == 13)
            {
               btnLogin_Click(null,null);
            }
        }
        #endregion

       
    }
}
