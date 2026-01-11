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
	public partial class frmManualTCP : Form
	{
		public			frmManualTCP()
		{
			InitializeComponent();
			InitTableLayout();
			
		}

		private string	GetBtnText(string btnName)
		{
			string[] temp = btnName.Split('_');
			if (temp.Length < 2)		return "-";

			string btnText = temp[1];
			string[] Words = {"Start", "Report","Request","Confirm","End"};

			foreach (var word in Words)
			{
				btnText = btnText.Replace(word, Environment.NewLine + word);
			}

			return btnText;
		}

		private void	InitTableLayout()
		{
			try
			{
				//Top Bottom
				int nBtnCount = 0;
				for (int i = 0; i < (int)CommInsp.SendCmd.Max; i++)
				{
					switch ((CommInsp.SendCmd)i)
					{
						case CommInsp.SendCmd.RunStateReport:
						case CommInsp.SendCmd.RunStateRequest:
						case CommInsp.SendCmd.InspStateRequest:
						case CommInsp.SendCmd.ProdDataReport:
						case CommInsp.SendCmd.AutoRunStart:
						case CommInsp.SendCmd.ResultDataReportConfirm:
						case CommInsp.SendCmd.HomeStart:
						case CommInsp.SendCmd.AlarmReport:
						case CommInsp.SendCmd.ReferenceStart:
						case CommInsp.SendCmd.PauseStart:
						case CommInsp.SendCmd.AlarmClear:
						case CommInsp.SendCmd.YieldClear:
						case CommInsp.SendCmd.JobCheckRequest:
							{
								Button buttonTop = new Button();
								buttonTop.Name = CommInsp.CmdDir.ToTop.ToString()+"_"+ ((CommInsp.SendCmd)i).ToString(); 
								buttonTop.Text = GetBtnText(buttonTop.Name); 
								buttonTop.Dock = DockStyle.Fill;
								buttonTop.Font = new Font("Segoe UI", 9, FontStyle.Bold);
								buttonTop.Margin = new Padding(2);
								buttonTop.TextAlign = ContentAlignment.MiddleCenter;
								buttonTop.Click += btn_Click;
								buttonTop.BackColor = Color.White;

								Button buttonBtm = new Button();
								buttonBtm.Name = CommInsp.CmdDir.ToBtm.ToString()+"_"+ ((CommInsp.SendCmd)i).ToString(); 
								buttonBtm.Text = GetBtnText(buttonBtm.Name); 
								buttonBtm.Dock = DockStyle.Fill;
								buttonBtm.Font = new Font("Segoe UI", 9, FontStyle.Bold);
								buttonBtm.Margin = new Padding(3);
								buttonBtm.TextAlign = ContentAlignment.MiddleCenter;
								buttonBtm.Click += btn_Click;
								buttonBtm.BackColor = Color.White;

								int nRow = nBtnCount / tlpTop.ColumnCount;
								int nCol = nBtnCount - (nRow * tlpTop.ColumnCount);
								tlpTop.Controls.Add(buttonTop, nCol, nRow);
								tlpBtm.Controls.Add(buttonBtm, nCol, nRow);
								nBtnCount++;
							}
						break;
						default:	continue;
					}
				}
				
				//Review
				 nBtnCount = 0;
				for (int i = 0; i < (int)CommInsp.SendCmd.Max; i++)
				{
					switch ((CommInsp.SendCmd)i)
					{
						case CommInsp.SendCmd.RunStateReport:
						case CommInsp.SendCmd.RunStateRequest:
						case CommInsp.SendCmd.RejectDataReport :
						case CommInsp.SendCmd.VerifyDataReportConfirm:
						case CommInsp.SendCmd.VerifyDataEndConfirm:
							{
								Button button = new Button();
								button.Name = CommInsp.CmdDir.ToReview.ToString()+"_"+ ((CommInsp.SendCmd)i).ToString(); 
								button.Text = GetBtnText(button.Name); 
								button.Dock = DockStyle.Fill;
								button.Font = new Font("Segoe UI", 9, FontStyle.Bold);
								button.Margin = new Padding(2);
								button.TextAlign = ContentAlignment.MiddleCenter;
								button.Click += btn_Click;
								button.BackColor = Color.White;

								int nRow = nBtnCount / tlpReview.ColumnCount;
								int nCol = nBtnCount - (nRow * tlpReview.ColumnCount);
								tlpReview.Controls.Add(button, nCol, nRow);
								nBtnCount++;
							}
						break;
						default:	continue;
					}
				}
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	frmManualTCP_Load(object sender, EventArgs e)
		{
			try
			{
				tmrUpdate.Start();
				
				G.commInspTop.EventTcpDataLog += CommInsp_EventTcpDataLog;
				G.commInspBtm.EventTcpDataLog += CommInsp_EventTcpDataLog;
				G.commInspRef.EventTcpDataLog += CommInsp_EventTcpDataLog;
				G.commUbiGEM.EventUBIGEMDataLog += CommUBIGEM_EventTcpDataLog;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
        }

		private void	CommInsp_EventTcpDataLog(string strData)
		{
			try
			{
				this.Invoke(new MethodInvoker(delegate()
				{
					if (lstBoxComm.Items.Count > 1000)			lstBoxComm.Items.RemoveAt(0);
					lstBoxComm.Items.Add(strData);
				}));
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void	CommUBIGEM_EventTcpDataLog(string strData)
		{
			try
			{
				this.Invoke(new MethodInvoker(delegate()
				{
					if (ltbUBIGEMLog.Items.Count > 1000)			ltbUBIGEMLog.Items.RemoveAt(0);
					ltbUBIGEMLog.Items.Add(strData);
				}));
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void	frmManualTCP_VisibleChanged(object sender, EventArgs e)
		{
			try
			{
				if (this.Visible == true)			{  tmrUpdate.Start();   SM.SHM.StartManual(); }
				else							    {  tmrUpdate.Stop();    SM.SHM.StopManual(); }
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
				Button				btn			= (Button)sender;
				object[]			objParam	= null;
				MANUAL				Manual		= MANUAL.NotManual;
				CommInsp.CmdDir	CmdDir		= CommInsp.CmdDir.ToTop;

				string[] temp = btn.Name.Split('_');
				if (temp.Length < 2) return;

				if (temp[0].Contains(CommInsp.CmdDir.ToTop.ToString()))			CmdDir	= CommInsp.CmdDir.ToTop;
				else if (temp[0].Contains(CommInsp.CmdDir.ToBtm.ToString()))		CmdDir	= CommInsp.CmdDir.ToBtm;
				else if (temp[0].Contains(CommInsp.CmdDir.ToReview.ToString()))	CmdDir	= CommInsp.CmdDir.ToReview;
				else return;

				UI.Logs.Write(LOGPART.TK, string.Format("{0} Click", btn.Name));

				if (Enum.TryParse(temp[1], out CommInsp.SendCmd cmd))
				{
					switch (cmd)
					{
						case CommInsp.SendCmd.RunStateReport:				Manual	= MANUAL.SEND_RUN_STATE_REPORT;					objParam	= new object[] {CmdDir};	break;	
						case CommInsp.SendCmd.RunStateRequest:			Manual	= MANUAL.SEND_RUN_STATE_REQUEST;				objParam	= new object[] {CmdDir};	break;	
						case CommInsp.SendCmd.InspStateRequest:			Manual	= MANUAL.SEND_INSP_STATE_REQUEST;				objParam	= new object[] {CmdDir};	break;	
						case CommInsp.SendCmd.AutoRunStart:				Manual	= MANUAL.SEND_AUTO_RUN_START;					objParam	= new object[] {CmdDir};	break;
						case CommInsp.SendCmd.ResultDataReportConfirm:	Manual	= MANUAL.SEND_RESULT_DATA_REPORT_CONFIRM;		objParam	= new object[] {CmdDir};	break;
						case CommInsp.SendCmd.VerifyDataReportConfirm:	Manual	= MANUAL.SEND_VERIFY_DATA_REPORT_CONFIRM;		objParam	= null;						break;
						case CommInsp.SendCmd.HomeStart:					Manual	= MANUAL.SEND_HOME_START;						objParam	= new object[] {CmdDir};	break;
						case CommInsp.SendCmd.AlarmReport:				Manual	= MANUAL.SEND_ALARM_REPORT;						objParam	= new object[] {CmdDir};	break;
						case CommInsp.SendCmd.VerifyDataEndConfirm:		Manual	= MANUAL.SEND_VERIFY_DATA_REPORT_END_CONFIRM;	objParam	= new object[] {CmdDir};	break;
						case CommInsp.SendCmd.PauseStart:					Manual	= MANUAL.SEND_PAUSE_START;						objParam	= new object[] {CmdDir};	break;
						case CommInsp.SendCmd.AlarmClear:					Manual	= MANUAL.SEND_ALARM_CLEAR;						objParam	= new object[] {CmdDir};	break;
						case CommInsp.SendCmd.YieldClear:					Manual	= MANUAL.SEND_YIELD_CLEAR;						objParam	= new object[] {CmdDir};	break;

						case CommInsp.SendCmd.ReferenceStart:	
							{
								frmComboBoxSelect sel = new frmComboBoxSelect();
								sel.ShowDialog(new string[] { "Lane1", "Lane2" });
								Manual = MANUAL.SEND_REFERENCE_START;
								objParam = new object[] {CmdDir, sel.SelNo };
							}
							break;
						case CommInsp.SendCmd.ProdDataReport:
							{
								frmComboBoxSelect sel = new frmComboBoxSelect();
								sel.ShowDialog(new string[] { "Inspection1 Data", "Inspection2 Data" });
								Manual = MANUAL.SEND_PROD_DATA_REPORT;
								objParam = new object[] { sel.SelNo, CmdDir };
							}
							break;
							
						case CommInsp.SendCmd.RejectDataReport:
							{
								frmKeyboard keyboard = new frmKeyboard("ID, Row Num,Col Num,Row max, Col Max, MesRetult");
								if (keyboard.ShowDialog() == DialogResult.Cancel) return;
								string[] strSplits = keyboard.Value.Split(',');
								if (strSplits.Length != 6) return;
								Manual = MANUAL.SEND_REJECT_DATA_REPORT; 
								objParam = new object[] { strSplits[0] ,strSplits[1],strSplits[2],strSplits[3],strSplits[4],strSplits[5]}; 
							}
							break;
							
						case CommInsp.SendCmd.JobCheckRequest:
							{
								frmKeyboard keyboardJob = new frmKeyboard("Module Name");
								if (keyboardJob.ShowDialog() == DialogResult.Cancel) return;
								string str = keyboardJob.Value;
								Manual = MANUAL.SEND_JOBCHECK; 
								objParam = new object[] { CmdDir, str };
							}
							break;
					}
				}
				 else
				{
					return;
				}

				frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OKCancel, Language.languageDefault.MB_Confirmation, "Do you want to Send?");
 				if (pMsg.ShowDialog() == DialogResult.Cancel)			return;

				SM.SHM.SetManualParams(objParam);
				SM.SHM.SetManualNum(Manual);
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void	btnLogClear_Click(object sender, EventArgs e)
		{
			try
			{
				lstBoxComm.Items.Clear();
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
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void btnUBIGEMLogClear_Click(object sender, EventArgs e)
		{
			try
			{
				ltbUBIGEMLog.Items.Clear();
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void btn_UBIGEM_Click(object sender, EventArgs e)
		{
			try
			{
				Button				btn			= (Button)sender;
				object[]			objParam	= null;
				MANUAL				Manual		= MANUAL.NotManual;
				string				strBtnName	= btn.Name;
				
				switch (strBtnName)
				{
					case "btnInspection_Result":		Manual	= MANUAL.SEND_UBIGEM_Inspection_Result;				break;
					case "btnTRAY_VALIDATION":			Manual	= MANUAL.SEND_UBIGEM_Tray_Validation;				break;
					case "btnLOT_END":					Manual	= MANUAL.SEND_UBIGEM_Lot_End;						break;
					case "btnControlStateChanged":		Manual	= MANUAL.SEND_UBIGEM_ControlStateChanged;			break;
					case "btnProcessStateChanged":		Manual	= MANUAL.SEND_UBIGEM_ProcessStateChanged;			break;
					case "btnAlarmSet":					Manual	= MANUAL.SEND_UBIGEM_Alarm_Set;						break;
					case "btnAlarmClear":				Manual	= MANUAL.SEND_UBIGEM_Alarm_Clear;					break;
				}

				frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OKCancel, Language.languageDefault.MB_Confirmation, "Do you want to Send?");
 				if (pMsg.ShowDialog() == DialogResult.Cancel)			return;

				SM.SHM.SetManualParams(objParam);
				SM.SHM.SetManualNum(Manual);
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}

			
		}

		private void ltbUBIGEMLog_KeyDown(object sender, KeyEventArgs e)
		{
			if(e.Control && e.KeyCode == Keys.C)
				{
					System.Text.StringBuilder copy_buffer = new System.Text.StringBuilder();
					foreach(object item in ltbUBIGEMLog.SelectedItems)
						copy_buffer.AppendLine(item.ToString());
					if (copy_buffer.Length > 0)
						Clipboard.SetText(copy_buffer.ToString());
				}
		}
	}
}
