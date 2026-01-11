using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace H_MarsSSD
{
    public partial class frmLoading : Form
    {
        public frmLoading(string strText)
        {
            InitializeComponent();
            this.TopMost = true;

            plLoading.Location = new Point(this.ClientSize.Width / 2 - plLoading.Size.Width / 2, this.ClientSize.Height / 2 - plLoading.Size.Height / 2);
            plLoading.Anchor = AnchorStyles.None;

            // Label Text 지정
            lbText.Text = strText;
        }

        private void tmrLoading_Tick(object sender, EventArgs e)
        {
            if (m_bClose)
            {
                tmrLoading.Stop();
                FormClose();
            }
        }

        bool m_bClose = true;
        public bool bClose
        {
            get { return m_bClose; }
            set { m_bClose = value; }
        }

        private void FormClose()
        {
            if (this != null)
                this.Close();

            this.Close();
        }

        private void frmLoading_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible == true)
            {
                tmrLoading.Enabled = true;
            }
            else
            {
                tmrLoading.Enabled = false;
            }
        }
         protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Escape:
                    FormClose();
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

    }

}
