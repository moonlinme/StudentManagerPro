using System;
using System.Drawing;
using System.Windows.Forms;

namespace StudentManagerPro
{
    /// <summary>
    /// 定向通信委托：发数据的通道形状（接收一个 string）
    /// </summary>
    public delegate void DataHandler(string data);

    /// <summary>
    /// A/B/C 三个窗体共用的同一个类：靠实例区分角色，实例委托实现定向传递。
    /// </summary>
    public class FrmData : Form
    {
        private TextBox txtInput = new TextBox();
        private Button btnSend = new Button();
        private Label lblShow = new Label();

        /// <summary>发送通道：指向"下家"的接收方法（实例委托，非静态）</summary>
        public DataHandler SendTo;

        /// <summary>收到数据后是否自动转发给下家（B 设为 true，A/C 为 false）</summary>
        public bool AutoForward = false;

        public FrmData(string windowName)
        {
            this.Text = windowName;
            this.Size = new Size(420, 180);

            lblShow.Text = "（还未收到数据）";
            lblShow.Location = new Point(20, 20);
            lblShow.AutoSize = true;

            txtInput.Location = new Point(20, 60);
            txtInput.Size = new Size(260, 25);

            btnSend.Text = "发送";
            btnSend.Location = new Point(300, 60);
            btnSend.Size = new Size(90, 30);
            btnSend.Click += btnSend_Click;

            this.Controls.Add(lblShow);
            this.Controls.Add(txtInput);
            this.Controls.Add(btnSend);
        }

        /// <summary>
        /// 接收数据：先显示；若开启自动转发（B），立刻把数据继续传给下家（C）
        /// </summary>
        public void ReceiveData(string data)
        {
            lblShow.Text = "收到: " + data;

            if (AutoForward && SendTo != null)
            {
                SendTo(data);
            }
        }

        // 点发送：把自己的输入定向发给下家（判空，没接线时不报错）
        private void btnSend_Click(object sender, EventArgs e)
        {
            if (SendTo != null)
            {
                SendTo(txtInput.Text);
            }
        }
    }
}