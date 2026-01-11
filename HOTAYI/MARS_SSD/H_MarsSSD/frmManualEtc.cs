using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Linq;
using System.ComponentModel;
using System.Windows.Threading;
using System.Collections.Generic;

namespace H_MarsSSD
{
	public partial class frmManualEtc : Form
	{
		//TableLayoutPanel이 한번에 갱신되게 하기 위해 추가
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cp = base.CreateParams;
				cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
				return cp;
			}
		}

		public			frmManualEtc()
		{
			InitializeComponent();
		}

		private string m_highlightButton = "";
		public string HighlightButton
		{
			set { m_highlightButton = value; }
		}

		private void	frmManualEtc_Load(object sender, EventArgs e)
		{
			try
			{
				tmrUpdate.Start();
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void	frmManualEtc_VisibleChanged(object sender, EventArgs e)
		{
			try
			{
				if (this.Visible == true)
				{
					tmrUpdate.Start();
					SM.SHM.StartManual();

					//특정 케이스 발생 시 버튼색 변경
					if (string.IsNullOrEmpty(m_highlightButton) == false)
					{
						Control[] foundControls = this.Controls.Find(m_highlightButton, true);

						if (foundControls.Length > 0 && foundControls[0] is Button button)
						{
							button.BackColor = Color.Orange;
						}
					}

				}
				else
				{
					tmrUpdate.Stop();
					SM.SHM.StopManual();

					//특정 케이스 발생 시 버튼색 원상 복구
					if (string.IsNullOrEmpty(m_highlightButton) == false)
					{
						Control[] foundControls = this.Controls.Find(m_highlightButton, true);

						if (foundControls.Length > 0 && foundControls[0] is Button button)
						{
							button.BackColor = Color.White;
						}
						m_highlightButton = "";
					}
				}
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	btn_Click(object sender, EventArgs e)
		{
		   try
			{
				Button			btn			= (Button)sender;
				string			str_manual	= btn.Tag.ToString().Trim();
				frmMessageBox	msg			= new frmMessageBox(frmMessageBox.teButtons.OKCancel, "QUESTION", "Do you want to run it?");

				if (msg.ShowDialog() == DialogResult.OK)
				{	
					UI.Logs.Write(LOGPART.TK, string.Format("{0} Click", btn.Name));

					SM.SHM.SetManualNum((MANUAL)Enum.Parse(typeof(MANUAL), str_manual));
				}
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void btnBarcodeViewer_Click(object sender, EventArgs e)
		{
			try
			{
				UI.ModalBCRViewer.Show();

				UI.Logs.Write(LOGPART.SYS, "Open QR Viewer");
			}
			catch(Exception ex)
			{
				 UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
		
		private	void	btnJobTeaching_Click(object sender, EventArgs e)
		{
		   try
			{
				if (SM.SHM.IsBitData(BIT.KeyLockAuto) == true)
				{
					SM.SHM.MsgShow(SeqMsg.KeyLockErr);
					return;
				}

				Button			btn			= (Button)sender;
				string			str_manual	= btn.Tag.ToString().Trim();
				frmMessageBox	msg			= new frmMessageBox(frmMessageBox.teButtons.OKCancel, "QUESTION", "Do you want to run it?");

				Form fc = Application.OpenForms["frmJobTeaching"];
				if (fc != null) return;

				frmJobTeaching pOtherJT = new frmJobTeaching(str_manual);
				pOtherJT.Show();
				
				SM.SHM.SetBitData(BIT.MainDisplayShow, true);
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	SetControlEnable(Control Ctrl, bool bEnable)
		{
			try
			{
				if (Ctrl.Enabled != bEnable)			Ctrl.Enabled = bEnable;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	SetControlColor(Control Ctrl, Color BackColor, Color ForeColor)
		{
			try
			{
				if (Ctrl.BackColor != BackColor)			Ctrl.BackColor = BackColor;
				if (Ctrl.ForeColor != ForeColor)			Ctrl.ForeColor = ForeColor;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	tmrUpdate_Tick(object sender, EventArgs e)
		{
			try
			{
				if (SM.SHM.IsBitData(BIT.AutoRun) == true)			SetControlEnable(this, false);
				else												SetControlEnable(this, true);
			
				lbLoadPnPRePick.Text	= SM.SHM.GetCurStep(SeqID.LOADPNP_RETRY_PICK);
				lbJobTeaching.Text		= SM.SHM.IsBitData(BIT.AutoRun) | SM.SHM.IsBitData(BIT.Pause) == true ? "NOT READY" : "READY";
				lbInspPlate1.Text		= G.seqInspPlate1.IsReady & G.cKernel.Motor( MT.InspPlate1Y).IsRdy() == true & G.cKernel.Motor(MT.InspPlate1Y).IsHomeDone() == true == true ? "READY" : "NOT READY";
				lbInspPlate2.Text		= G.seqInspPlate2.IsReady & G.cKernel.Motor( MT.InspPlate2Y).IsRdy() == true & G.cKernel.Motor(MT.InspPlate2Y).IsHomeDone() == true== true ? "READY" : "NOT READY";
				lbPlate1TOP.Text		= G.seqInspPlate1.IsReady & SM.SHM.IsBitData(BIT.TopInspConnect) == true ? "READY" : "NOT READY";
				lbPlate2BTM.Text		= G.seqInspPlate2.IsReady & SM.SHM.IsBitData(BIT.BtmInspConnect) == true ? "READY" : "NOT READY";
				lbTrayConvPick.Text		= SM.SHM.GetCurStep(SeqID.LDTRANS_PICKCONV);
				lbTrayConvPlace.Text	= SM.SHM.GetCurStep(SeqID.ULDTRANS_PLACECONV);

				lbTopCal.Text			= G.seqInspPlate1.smmCalibration.ToString() ;
				lbBtmCal.Text			= G.seqInspPlate2.smmCalibration.ToString() ;
				lbStage1Maint.Text		= G.seqInspPlate1.smmCalibration.ToString() ;
				lbStage2Maint.Text		= G.seqInspPlate2.smmCalibration.ToString() ;
				lbBotHeadMaint.Text		= G.seqInspPlate1.smmCalibration.ToString() ;
				
			}
			catch
			{

			}
		}
	}
}
