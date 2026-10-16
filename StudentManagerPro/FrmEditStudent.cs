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
    public partial class FrmEditStudent : Form
    {
        private StudentService objStudentService = new StudentService();
        private StudentClassService objClassService = new StudentClassService();
        public FrmEditStudent()
        {
            InitializeComponent();
            this.cboClassName.DataSource = objClassService.GetAllClasses();
            this.cboClassName.DisplayMember = "ClassName";
            this.cboClassName.ValueMember = "ClassId";
            // this.cboClassName.SelectedIndex = -1;
        }

        public FrmEditStudent(StudentExt objStudent):this()
        {
            //显示学员信息
            this.txtStudentId.Text = objStudent.StudentId.ToString();
            this.txtStudentName.Text = objStudent.StudentName;
            if (objStudent.Gender == "男")
            {
                this.rdoMale.Checked = true;
            }
            else
            {
                this.rdoFemale.Checked = true;
            }
            this.dtpBirthday.Value = objStudent.Birthday;
            this.txtStudentIdNo.Text = objStudent.StudentIdNo;
            this.txtPhoneNumber.Text = objStudent.PhoneNumber;
            this.txtStudentAddress.Text = objStudent.StudentAddress;
            this.txtCardNo.Text = objStudent.CardNo; 
            
            this.cboClassName.SelectedItem = objStudent.ClassName;
        }

        private void FrmEditStudent_Load(object sender, EventArgs e)
        {
            //
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
           
            //判断身份证号是否重复
            if(objStudentService.IsIdNoExistd(this.txtStudentIdNo.Text.Trim(),this.txtStudentId.Text.Trim()))
            {
                MessageBox.Show("身份证号已存在");
                this.txtStudentIdNo.Focus();
                this.txtStudentIdNo.SelectAll();
                return;
            }

            //封装学生对象
            Student objStudent = new Student()
            {
                StudentId = Convert.ToInt32(this.txtStudentId.Text),
                StudentName = this.txtStudentName.Text,
                Gender = this.rdoMale.Checked ? "男" : "女",
                Birthday = this.dtpBirthday.Value,
                StudentIdNo = this.txtStudentIdNo.Text,
                PhoneNumber = this.txtPhoneNumber.Text,
                StudentAddress = this.txtStudentAddress.Text,
                CardNo = this.txtCardNo.Text,
                ClassId = Convert.ToInt32(this.cboClassName.SelectedValue)
            };

             // 提交修改
             try
             {

                if(objStudentService.ModifyStudent(objStudent)==1)
                {
                    MessageBox.Show("修改成功");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("修改失败");
                }
             }
             catch (Exception ex)
             {
                 MessageBox.Show(ex.Message);
             }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            // 关闭窗口
            this.Close();
        }
    }
}
