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
	public partial class frmTraySizeSensorSel : Form
	{
		public frmTraySizeSensorSel()
		{
			InitializeComponent();
		}

		#region [ Variables ]

		bool[] m_bSenState = new bool[] { false, false, false, false };
		#endregion

		#region [ Event Function ]
		private void frmTraySizeSensorSel_Load(object sender, EventArgs e)
		{
			m_bSenState[0]	= SM.SHM.GetRecipeDataToInt(DEVICE.TraySizeChkSen1Use) / 1000 == 0 ? false : true;
			m_bSenState[2]	= SM.SHM.GetRecipeDataToInt(DEVICE.TraySizeChkSen3Use) / 1000 == 0 ? false : true;
			m_bSenState[3]	= SM.SHM.GetRecipeDataToInt(DEVICE.TraySizeChkSen4Use) / 1000 == 0 ? false : true;
			
			if (m_bSenState[0] == true)
			{
				btnSensor1.BackColor	= Color.Lime;
				btnSensor1.Text			= "ON";
			}
			if (m_bSenState[2] == true)
			{
				btnSensor3.BackColor = Color.Lime;
				btnSensor3.Text			= "ON";
			}
			if (m_bSenState[3] == true)
			{
				btnSensor4.BackColor = Color.Lime;
				btnSensor4.Text			= "ON";
			}
			
			SetSensorState(1, m_bSenState[0]);
			SetSensorState(2, true);
			SetSensorState(3, m_bSenState[2]);
			SetSensorState(4, m_bSenState[3]);
		}
		#endregion

		#region [ Timmer Function ]

		#endregion


		#region [ Button Event ]

		private void btnSensor_Click(object sender, EventArgs e)
		{
			Button btn = sender as Button;

			if (btn.BackColor == Color.White)
			{
				btn.BackColor = Color.Lime;
				btn.Text = "ON";
				SetSensorState(Convert.ToInt32((string)btn.Tag), true);
			}
			else
			{
				btn.BackColor = Color.White;
				btn.Text = "OFF";
				SetSensorState(Convert.ToInt32((string)btn.Tag), false);
			}

			btnSave.Enabled = true;
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			frmMessageBox pMsg = new frmMessageBox(true, Language.languageDefault.MB_Warnning, Language.languageDefault.MB_Save);
			if (pMsg.ShowDialog() == DialogResult.Cancel)			return;
			
			btnSave.Enabled = false;

			if (m_bSenState[0] == true)			UI.SubpageRecipe.SetRecipeData((int)DEVICE.TraySizeChkSen1Use, 1000);
			else								UI.SubpageRecipe.SetRecipeData((int)DEVICE.TraySizeChkSen1Use, 0);
			
			if (m_bSenState[2] == true)			UI.SubpageRecipe.SetRecipeData((int)DEVICE.TraySizeChkSen3Use, 1000);
			else								UI.SubpageRecipe.SetRecipeData((int)DEVICE.TraySizeChkSen3Use, 0);

			if (m_bSenState[3] == true)			UI.SubpageRecipe.SetRecipeData((int)DEVICE.TraySizeChkSen4Use, 1000);
			else								UI.SubpageRecipe.SetRecipeData((int)DEVICE.TraySizeChkSen4Use, 0);

			UI.SubpageRecipe.UpdateRecipeDataGrid(UI.SubpageRecipe.iNowRecipeNum); // Recipe Data Grid Refresh

			string msg_ = string.Format("[{0:D2}] Tray Size Sensor State Save !!", UI.SubpageRecipe.iNowRecipeNum + 1);
			UI.Logs.Write(LOGPART.DATA, msg_);
		}
		#endregion

		#region [ Function ]

		#region - Private Function

		

		private void SetSensorState(int nSensorNum, bool bSensorOnOff)
		{
			Color color = Color.GhostWhite;
			if (bSensorOnOff)
			{
				color = Color.Lime;
			}

			switch (nSensorNum)
			{
				case 1:
					lbSen1UseState.BackColor = color;
					m_bSenState[0] = bSensorOnOff;
					break;
				case 2:
					lbSen2UseState.BackColor = color;
					m_bSenState[1] = bSensorOnOff;
					break;
				case 3:
					lbSen3UseState.BackColor = color;
					m_bSenState[2] = bSensorOnOff;
					break;
				case 4:
					lbSen4UseState.BackColor = color;
					m_bSenState[3] = bSensorOnOff;
					break;
			}
		}
		#endregion

		#region - Public Function

		#endregion

		#region - Language Function 

		public void SetLanguageText()
		{
			lbSensor1.Text	= Language.languageDefault.Sensor1;
			lbSensor3.Text	= Language.languageDefault.Sensor3;
			lbSensor1.Text	= Language.languageDefault.Sensor1;
			lbSensor3.Text	= Language.languageDefault.Sensor3;
			btnSave.Text	= Language.languageDefault.Save;
			btnClose.Text	= Language.languageDefault.BTN_Close;
		}




		#endregion

		#endregion

		private void btnClose_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
