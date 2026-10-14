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
    public partial class FrmAddStudent : Form
    {
        //
        private StudentClassService objClassService = new StudentClassService();
        private StudentService objStudentService = new StudentService();
        
        public FrmAddStudent()
        {
            InitializeComponent();
            this.cboClassName.DataSource = objClassService.GetAllClasses();
            this.cboClassName.DisplayMember = "ClassName";
            this.cboClassName.ValueMember = "ClassId";
            this.cboClassName.SelectedIndex = -1;
        }

        private void FrmAddStudent_Load(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmAddStudent_FormClosed(object sender, FormClosedEventArgs e)
        {
            // 关闭时，将对象设为null
            FrmMain.objFrmAddStudent = null;
            
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Student objStudent = new Student();
            #region 数据验证
            if (this.txtStudentName.Text.Trim() == "")
            {
                MessageBox.Show("请输入学员姓名");
                this.txtStudentName.Focus();
                return;
            }

            // if(objStudentService.IsIdNoExistd(this.txtStudentIdNo.Text.Trim()))
            // {
            //     MessageBox.Show("学员身份证号已存在");
            //     this.txtStudentIdNo.Focus();
            //     return;
            // }
            #endregion

            #region 封装学员对象
            objStudent.StudentName = this.txtStudentName.Text.Trim();
            objStudent.Gender = this.rdoMale.Checked ? "男" : "女";
            objStudent.Birthday = Convert.ToDateTime(this.dtpBirthday.Value);
            objStudent.StudentIdNo = this.txtStudentIdNo.Text.Trim();
            objStudent.Age = DateTime.Now.Year - Convert.ToInt32(this.dtpBirthday.Value.Year);
            objStudent.PhoneNumber = this.txtPhoneNumber.Text.Trim();
            objStudent.StudentAddress = this.txtStudentAddress.Text.Trim();
            // objStudent.CardNo = this.txtCardNo.Text.Trim();
            objStudent.ClassId = Convert.ToInt32(this.cboClassName.SelectedValue);



            #endregion

            #region 调用后台数据访问方法添加对象
            try
            {
                if (objStudentService.AddStudent(objStudent) > 0)
                {
                    // 通过委托通知学员管理窗体刷新列表
                    FrmStudentManage.NotifyStudentChanged(objStudent.StudentName);

                   DialogResult result = MessageBox.Show("添加学员成功，是否继续添加？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                   if(result == DialogResult.Yes)
                   {
                       this.cboClassName.SelectedIndex = -1;
                       this.rdoMale.Checked = false;
                       this.rdoFemale.Checked = false;

                       foreach (Control item in Controls)
                       {
                           if(item is TextBox)
                           {
                               item.Text = "";
                           }
                           this.txtStudentName.Focus();
                       }
                   }
                   else
                   {
                       this.Close();
                   }
                }
                else
                {
                    MessageBox.Show("添加学员失败");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("添加学员失败,错误信息：" + ex.Message);
            }

            #endregion

        }
    }
}
