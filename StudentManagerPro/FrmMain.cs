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
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
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
    }
}
