using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test_subd
{
    public partial class FormCaptcha : Form
    {
        public FormCaptcha()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 6)
            {
                if (textBox1.Text != "azyayz")
                {
                    frmAuthorization fm = Application.OpenForms["frmAuthorization"] as frmAuthorization;
                    fm.StartBtnConnectTimer();
                }
                this.Close();
            }
        }
    }
}
