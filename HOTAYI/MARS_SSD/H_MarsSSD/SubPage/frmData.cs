using System;
using System.Windows.Forms;
using System.Drawing;

namespace H_MarsSSD
{
	public partial class frmData : Form
	{
		public frmData()
		{
			InitializeComponent();
		}

		#region [ Variables ]

		private void frmData_VisibleChanged(object sender, EventArgs e)
		{
			if (this.Visible)
			{
				Hide_AllPage();
				UI.SubpageRecipe.Show();
				btnRecipe.BackColor = Color.SteelBlue;
				btnRecipe.Select();
			}
		}
		#endregion

		#region [ Event Function ]

		private void frmData_Load(object sender, EventArgs e)
		{
			UI.SubpageRecipe.TopLevel = false;
			UI.SubpageDm.TopLevel = false;
			UI.SubpageSDm.TopLevel = false;
			UI.SubpageUseskip.TopLevel = false;
			UI.SubpageBit.TopLevel = false;

			Hide_AllPage();
			UI.SubpageRecipe.Show();
			btnRecipe.BackColor = Color.SteelBlue;

			this.panelData.Controls.Add(UI.SubpageRecipe);
			this.panelData.Controls.Add(UI.SubpageDm);
			this.panelData.Controls.Add(UI.SubpageSDm);
			this.panelData.Controls.Add(UI.SubpageUseskip);
			this.panelData.Controls.Add(UI.SubpageBit);
		}
		#endregion

		#region [ Timmer Function ]

		#endregion

		#region [ Button Event ]
		private void btnRecipe_Click(object sender, EventArgs e)
		{
			if (btnRecipe.BackColor == Color.SteelBlue) return;

			Hide_AllPage();
			UI.SubpageRecipe.Show();
			btnRecipe.BackColor = Color.SteelBlue;
		}

		private void btnDm_Click(object sender, EventArgs e)
		{
			if (btnDm.BackColor == Color.SteelBlue) return;

			Hide_AllPage();
			UI.SubpageDm.Show();
			btnDm.BackColor = Color.SteelBlue;
		}

		private void btnSDm_Click(object sender, EventArgs e)
		{
			if (btnSDm.BackColor == Color.SteelBlue) return;

			Hide_AllPage();
			UI.SubpageSDm.Show();
			btnSDm.BackColor = Color.SteelBlue;
		}

		private void btnUseskip_Click(object sender, EventArgs e)
		{
			if (btnUseskip.BackColor == Color.SteelBlue) return;

			Hide_AllPage();
			UI.SubpageUseskip.Show();
			btnUseskip.BackColor = Color.SteelBlue;
		}

		private void btnBit_Click(object sender, EventArgs e)
		{
			if (btnBit.BackColor == Color.SteelBlue) return;

			Hide_AllPage();
			UI.SubpageBit.Show();
			btnBit.BackColor = Color.SteelBlue;
		}
		#endregion

		#region [ Function ]

		#region - Private Function

		#endregion

		#region - Public Function
		private void Hide_AllPage()
		{
			UI.SubpageRecipe.Hide();
			UI.SubpageDm.Hide();
			UI.SubpageSDm.Hide();
			UI.SubpageUseskip.Hide();
			UI.SubpageBit.Hide();

			btnRecipe.BackColor = System.Drawing.Color.White;
			btnDm.BackColor = System.Drawing.Color.White;
			btnSDm.BackColor = System.Drawing.Color.White;
			btnUseskip.BackColor = System.Drawing.Color.White;
			btnBit.BackColor = System.Drawing.Color.White;
		}

		public void DataPageButtonControl(bool on)
		{
			if (btnRecipe.Enabled == on) return;

			if (!UI.SubpageRecipe.Visible)
				btnRecipe.Enabled = on;
			if (!UI.SubpageDm.Visible)
				btnDm.Enabled = on;
			if (!UI.SubpageSDm.Visible)
				btnSDm.Enabled = on;
			if (!UI.SubpageUseskip.Visible)
				btnUseskip.Enabled = on;
			if (!UI.SubpageBit.Visible)
				btnBit.Enabled = on;
		}
		
        public void DataPageButtonEnable()
        {
            int nIdx = 0;
            switch (Program.m_PasswordMode)
            {
                case PW_MODE.OPER:  nIdx = 0;   break;
                case PW_MODE.ENG:   nIdx = 1;   break;
                case PW_MODE.PRO:   nIdx = 2;   break;
                case PW_MODE.ADMIN:
                    btnRecipe.Enabled = true;
                    btnDm.Enabled = true;
                    btnSDm.Enabled = true;
                    btnUseskip.Enabled = true;
                    btnBit.Enabled = true;
                    return;
            }
            btnRecipe.Enabled = UI.ModalLevelSetting.ReadLevelXMLData(nIdx, "RECIPE");
            btnDm.Enabled = UI.ModalLevelSetting.ReadLevelXMLData(nIdx, "DM");
            btnSDm.Enabled = UI.ModalLevelSetting.ReadLevelXMLData(nIdx, "SDM");
            btnUseskip.Enabled = UI.ModalLevelSetting.ReadLevelXMLData(nIdx, "USESKIP");
            btnBit.Enabled = UI.ModalLevelSetting.ReadLevelXMLData(nIdx, "BIT");

        }
		
        public void DataPageButtonDisable()
        {
            if (!(btnRecipe.Enabled && btnDm.Enabled && btnSDm.Enabled && btnUseskip.Enabled && btnBit.Enabled)) return;

            if (!UI.SubpageRecipe.Visible)
                btnRecipe.Enabled = false;
            if (!UI.SubpageDm.Visible)
                btnDm.Enabled = false;
            if (!UI.SubpageSDm.Visible)
                btnSDm.Enabled = false;
            if (!UI.SubpageUseskip.Visible)
                btnUseskip.Enabled = false;
            if (!UI.SubpageBit.Visible)
                btnBit.Enabled = false;
        }

		public void	UpdatePageVisible(PW_MODE nowPw)
		{
			if (nowPw == PW_MODE.ADMIN)
			{
				btnSDm.Visible = true;
				btnBit.Visible = true;
			}
			else
			{
				btnSDm.Visible = false;
				btnBit.Visible = false;
			}
		}

		public bool IsWorking()
		{
			if (UI.SubpageRecipe.IsWorking() || UI.SubpageDm.IsWorking() || UI.SubpageSDm.IsWorking() || UI.SubpageUseskip.IsWorking() || UI.SubpageBit.IsWorking())
				return true;
			return false;
		}
		#endregion

		#region - Language Function 

		public void SetLanguageText()
		{
			btnRecipe.Text = Language.languageDefault.Recipe;
		}
		#endregion

		#endregion
	}
}
