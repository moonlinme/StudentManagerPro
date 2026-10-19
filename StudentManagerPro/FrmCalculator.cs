using StudentManagerPro.Helper;
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
    public partial class FrmCalculator : Form
    {
        private CalculatorEngine calculatorEngine = new CalculatorEngine();

        public FrmCalculator()
        {
            InitializeComponent();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void FrmCalculator_Load(object sender, EventArgs e)
        {
            UpdateDisplay();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        // 所有按钮点击事件
        private void Button_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null && btn.Tag != null)
            {
                string key = btn.Tag.ToString();

                calculatorEngine.PressButton(key);

                UpdateDisplay();
            }
        }

        private void UpdateDisplay()
        {
            lblExpression.Text = calculatorEngine.Expression;
            lblCurrentInput.Text = calculatorEngine.CurrentInput;
        }

        private void lblCurrentInput_Click(object sender, EventArgs e)
        {

        }

        private void lblExpression_Click(object sender, EventArgs e)
        {

        }
    }
}
