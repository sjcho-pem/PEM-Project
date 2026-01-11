using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace H_MarsSSD
{
	public partial class frmErrRecipeDisp : Form
	{
		public frmErrRecipeDisp()
		{
			InitializeComponent();
			this.Size = new Size(1742, 95);
		}

		#region [ Variables ]
		public string strNowRecipeName = "";
		public bool bErrorPoup = false;

		private	CBlinkTimer		m_blinkTmr		= new CBlinkTimer();
		private	List<double>	m_ltTact		= new List<double>();
		#endregion

		#region [ Event Function ]

		#endregion

		#region [ Timmer Function ]

		private void tmrErrRecipeDisp_Tick(object sender, EventArgs e)
		{
			// Error Pop function
			if (this.bErrorPoup)
			{
				//btnErrorPopup.PerformClick();
				this.bErrorPoup = false;
			}

			// recipe name display
			if (lbNowRecipe.Text != strNowRecipeName)
				lbNowRecipe.Text = strNowRecipeName;

			// Error Pop function
			if (this.bErrorPoup)
			{
				this.bErrorPoup = false;
			}

			SetNowRecipeDisp();
		}

		private void tmrStatusDisp_Tick(object sender, EventArgs e)
		{
			#region		[ Lot ID ]
			lbLotID1.Text		= SM.SHM.GetRunLotInfo(0).WorkOrder;
			lbDeviceTop1.Text	= SM.SHM.GetRunLotInfo(0).ProgramID_Top.Replace("@", Environment.NewLine);
			lbDeviceBtm1.Text	= SM.SHM.GetRunLotInfo(0).ProgramID_Btm.Replace("@", Environment.NewLine);
			lbCount1.Text		= SM.SHM.GetRunLotInfo(0).ProdCount;
			lbStartTime1.Text	= SM.SHM.GetRunLotInfo(0).StartTime;

			lbLotID2.Text		= SM.SHM.GetRunLotInfo(1).WorkOrder;
			lbDeviceTop2.Text	= SM.SHM.GetRunLotInfo(1).ProgramID_Top.Replace("@", Environment.NewLine);
			lbDeviceBtm2.Text	= SM.SHM.GetRunLotInfo(1).ProgramID_Btm.Replace("@", Environment.NewLine);
			lbCount2.Text		= SM.SHM.GetRunLotInfo(1).ProdCount;
			lbStartTime2.Text	= SM.SHM.GetRunLotInfo(1).StartTime;
			#endregion

			#region		[ Tact/UPH ]
			long	lTactLast	= SM.SHM.GetSeqLastTact(EnumHelper.GetDescription(Tact.UNLOAD_PnP_START));
			long	lTactCur	= SM.SHM.GetSeqTact(EnumHelper.GetDescription(Tact.UNLOAD_PnP_START));
			double	dTactAvg	= 0;
			double	dUphLast	= 0;
			double	dUphAvg		= 0;
			double	dSum		= 0;
			
			if (lTactLast.ToString() != lbTactLast.Text)
			{
				m_ltTact.Add((double)lTactLast);
				if (m_ltTact.Count > NDm.Read(eNDM.UphAverageCount))			m_ltTact.RemoveAt(0);
			}
			
			if (m_ltTact.Count > 0)
			{
				foreach (double val in m_ltTact)
				{
					dSum += val;
				}

				dTactAvg	= dSum / m_ltTact.Count;
			}
			
			if (lTactLast != 0)		dUphLast	= 3600.0 / (lTactLast/1000.0/4.0);
			if (dTactAvg != 0)		dUphAvg		= 3600.0 / (dTactAvg/1000.0/4.0);

			lbTactLast.Text	= lTactLast.ToString();
			lbTactAvg.Text	= string.Format("{0:0}", dTactAvg);
			//lbTactCur.Text	= lTactCur.ToString();

			lbUphLast.Text	= string.Format("{0:0.000}", dUphLast);
			lbUphAvg.Text	= string.Format("{0:0.000}", dUphAvg);
			#endregion
		}
		#endregion

		#region [ Label Event ]

		private void lbErrCode_Click(object sender, EventArgs e)
		{
            if (SM.SHM.GetErrorCode() == 0) return;
            
			PopupErrCode();
		}
		#endregion

		#region [ Button Event ]

		private void btnAllUPPopup_Click(object sender, EventArgs e)
		{
			try
			{
				Form fc = Application.OpenForms["frmUPHMeasurement"];
				if (fc != null)
				{
					fc.WindowState = FormWindowState.Normal;
					fc.BringToFront();
					return;
				}
				frmUPHMeasurement frm = new frmUPHMeasurement();
				frm.Show();
			}
			catch(Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void btnAvgClear_Click(object sender, EventArgs e)
		{
			m_ltTact.Clear();
		}

		#endregion

		#region [ Function ]

		#region - Private Function

		#endregion

		#region - Public Function

		public void DisplayErrorCode(uint uiErrNum, string strErrName)
		{
			string strDisp = "";

			if (uiErrNum == 0  ||  m_blinkTmr.Blink() == true)
			{// Normal
				strDisp = string.Format("[{0:D3}]" + strErrName, uiErrNum);
				lbErrCode.BackColor = Color.Transparent;
				lbErrCode.ForeColor = Color.Black;
			}
			else if (uiErrNum < (uint)ALM.WARNING_START)
			{// Error
				strDisp = string.Format("[{0:D3}]" + strErrName, uiErrNum);
				lbErrCode.BackColor = Color.Red;
				lbErrCode.ForeColor = Color.White;
			}
			else
			{// Warning
				strDisp = string.Format("[{0:D3}]" + strErrName, uiErrNum);
				lbErrCode.BackColor = Color.Yellow;
				lbErrCode.ForeColor = Color.Black;
			}

			lbErrCode.Text = strDisp;
		}

		public void SetNowRecipeDisp()
		{
			int strRecipeNum = UI.SubpageRecipe.iNowRecipeNum;
			string strRecipeName = UI.SubpageRecipe.GetRecipeName(strRecipeNum);

			strNowRecipeName = string.Format("[{0:D2}] {1}", Convert.ToInt32(strRecipeNum + 1), strRecipeName);
		}

		public void PopupErrCode()
		{
			if (UI.ModalErrDescView.Modal) return;
		   // if (UI.PageError.GetErrorCode() == 0) return;

			if (UI.ModalErrDescView.ShowDialog() == DialogResult.OK)
				return;
		}
		#endregion

		#region - Language Function 

		#endregion

		#endregion

#region [ Language ]
		public void SetLanguageText()
		{
			lbStartTime.Text	= Language.languageDefault.StartTime;
			lbLast.Text			= Language.languageDefault.Last;
			lbAverage.Text		= Language.languageDefault.Average;
			//lbCurrent.Text		= Language.languageDefault.Current;
			//btnAvgClear.Text	= Language.languageDefault.AvgClear;
		}
		#endregion

		//         private Font AutoFontSize(Label label, String text)
		//         {
		//             if (string.IsNullOrEmpty(text) == true)
		//                 return new Font(label.Font.Name, (float)10 );
		// 
		//             Font ft;
		//             Graphics gp;
		//             SizeF sz;
		//             Single Factor, FactorX, FactorY;
		// 
		//             gp = label.CreateGraphics();
		//             sz = gp.MeasureString(text, label.Font);
		//             gp.Dispose();
		// 
		//             FactorX = (label.Width) / sz.Width;
		//             FactorY = (label.Height) / sz.Height;
		// 
		//             if (FactorX > FactorY)
		//                 Factor = FactorY;
		//             else
		//                 Factor = FactorX;
		//             ft = label.Font;
		// 
		//             return new Font(ft.Name,( ft.SizeInPoints * (Factor) - (float)0.5) );
		//         }

	}
}
