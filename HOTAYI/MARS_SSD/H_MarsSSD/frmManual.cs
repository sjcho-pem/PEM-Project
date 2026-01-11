using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Text;

namespace H_MarsSSD
{
    public partial class frmManual : Form
    {           
        public frmManual()
        {
            InitializeComponent();
        }

        #region [ Variables ]

        #endregion

        #region [ Event Function ]

        private void frmManual_Load(object sender, EventArgs e)
        {
            UI.PageTenKey.TopLevel		= false;
            UI.PageManualSeq.TopLevel	= false;
			UI.PageManualCyl.TopLevel	= false;
			UI.PageManualTCP.TopLevel	= false;
			UI.PageManualEtc.TopLevel	= false;

            this.plManual.Controls.Add(UI.PageTenKey);
            this.plManual.Controls.Add(UI.PageManualSeq);
			this.plManual.Controls.Add(UI.PageManualCyl);
			this.plManual.Controls.Add(UI.PageManualTCP);
			this.plManual.Controls.Add(UI.PageManualEtc);
        }

        private void frmManual_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                Hide_AllPage();
                UI.PageManualCyl.Show();
                btnManualCyl.BackColor = Color.SteelBlue;
                btnManualCyl.Select();
            }
			else
			{
				SM.SHM.StopManual();
			}
        }
        #endregion

        #region [ Timmer Function ]

        #endregion

        #region [ CheckBox Function ]

        #endregion

        #region [ Button Event ]
        private void btnTenkey_Click(object sender, EventArgs e)
        {
            if (btnTenkey.BackColor == Color.SteelBlue) return;

            Hide_AllPage();
            UI.PageTenKey.Show();
            btnTenkey.BackColor = Color.SteelBlue;
        }
		
		private void btnManualSeq_Click(object sender, EventArgs e)
		{
			if (btnManualSeq.BackColor == Color.SteelBlue)		return;

			Hide_AllPage();
			UI.PageManualSeq.Show();
			btnManualSeq.BackColor = Color.SteelBlue;
		}

		private void btnManualCyl_Click(object sender, EventArgs e)
		{
			if (btnManualCyl.BackColor == Color.SteelBlue)		return;

			Hide_AllPage();
			UI.PageManualCyl.Show();
			btnManualCyl.BackColor = Color.SteelBlue;
		}

		private void btnManualTCP_Click(object sender, EventArgs e)
		{
			if (btnManualTCP.BackColor == Color.SteelBlue)		return;

			Hide_AllPage();
			UI.PageManualTCP.Show();
			btnManualTCP.BackColor = Color.SteelBlue;
		}

		private void btnManualEtc_Click(object sender, EventArgs e)
		{
			if (btnManualEtc.BackColor == Color.SteelBlue)		return;

			Hide_AllPage();
			UI.PageManualEtc.Show();
			btnManualEtc.BackColor = Color.SteelBlue;
		}
        #endregion

        #region [ Grid Setting ]

        #region - Function
        
        #endregion

        #region - Event
        
        #endregion

        #endregion

        #region [ Function ]

        #region - Private Function

        #endregion

        #region - Public Function

        private void Hide_AllPage()
        {
            
            UI.PageTenKey.Hide();
			UI.PageManualSeq.Hide();
			UI.PageManualCyl.Hide();
			UI.PageManualTCP.Hide();
			UI.PageManualEtc.Hide();
            
            btnTenkey.BackColor		= Color.White;
            btnManualSeq.BackColor	= Color.White;
			btnManualCyl.BackColor	= Color.White;
			btnManualTCP.BackColor	= Color.White;
			btnManualEtc.BackColor	= Color.White;
        }
		
		public void ManualPageButtonEnable()
		{
			switch (Program.m_PasswordMode)
			{
				case PW_MODE.OPER:
					{
						if (btnTenkey.Enabled == true)				btnTenkey.Enabled		= false;
						if (btnManualSeq.Enabled == true)			btnManualSeq.Enabled	= false;
						if (btnManualCyl.Enabled == true)			btnManualCyl.Enabled	= false;
						if (btnManualTCP.Enabled == true)			btnManualTCP.Enabled	= false;
						if (btnManualEtc.Enabled == true)			btnManualEtc.Enabled	= false;
					}
					break;
				case PW_MODE.ENG:
				case PW_MODE.PRO:
				case PW_MODE.ADMIN:
					{
						if (btnTenkey.Enabled == false)				btnTenkey.Enabled		= true;
						if (btnManualSeq.Enabled == false)			btnManualSeq.Enabled	= true;
						if (btnManualCyl.Enabled == false)			btnManualCyl.Enabled	= true;
						if (btnManualTCP.Enabled == false)			btnManualTCP.Enabled	= true;
						if (btnManualEtc.Enabled == false)			btnManualEtc.Enabled	= true;
					}
					break;
			}
		}

        public void ManualPageButtonDisable()
        {
            if (btnTenkey.Enabled == true)				btnTenkey.Enabled		= false;
			if (btnManualSeq.Enabled == true)			btnManualSeq.Enabled	= false;
			if (btnManualCyl.Enabled == true)			btnManualCyl.Enabled	= false;
			if (btnManualTCP.Enabled == true)			btnManualTCP.Enabled	= false;
			if (btnManualEtc.Enabled == true)			btnManualEtc.Enabled	= false;
        }
		
		public void	UpdatePageVisible(PW_MODE nowPw)
		{
			if (nowPw == PW_MODE.ADMIN)
			{
				btnManualSeq.Visible	= true;
				btnManualTCP.Visible	= true;
				btnTenkey.Visible		= true;
			}
			else
			{
				btnManualSeq.Visible	= false;
				btnManualTCP.Visible	= false;
				btnTenkey.Visible		= false;
			}
		}

        public bool IsWorking()
        {
            if (UI.PageTenKey.IsWorking())
                return true;
            return false;
        }

        public void ManualCylButtonClick()
        {
			if (btnManualCyl.BackColor == Color.SteelBlue)		return;

            Hide_AllPage();
            UI.PageManualCyl.Show();
            btnManualCyl.BackColor = Color.SteelBlue;
        }

        public void ManualEtcButtonClick()
        {
			if (btnManualEtc.BackColor == Color.SteelBlue)		return;

            Hide_AllPage();
			UI.PageManualEtc.HighlightButton = "btnTrayBcrRead";
            UI.PageManualEtc.Show();
            btnManualEtc.BackColor = Color.SteelBlue;
        }
        #endregion

        #region - Language Function 

        public void SetLanguageText()
        {
            btnTenkey.Text      = Language.languageDefault.TenKey;
            btnManualCyl.Text   = Language.languageDefault.ManualCyl;
            btnManualSeq.Text   = Language.languageDefault.ManualSeq;
			btnManualTCP.Text	= Language.languageDefault.ManualTcp;
			btnManualEtc.Text	= Language.languageDefault.ManualEtc;
        }
		#endregion

		#endregion
	}
}
