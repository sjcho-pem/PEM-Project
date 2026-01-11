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
	public partial class frmSubItems : Form
	{
		#region [ Variables ]
		private	CBlinkTimer		m_blinkTmr			= new CBlinkTimer();
		private	PStopwatch		m_pswPauseBtnDown	= new PStopwatch();
		private	ColorConverter	m_ColorConverter	= new ColorConverter();
		#endregion

		public frmSubItems()
		{
			InitializeComponent();
			tmrSubItems.Start();
			this.Size = new Size(624, 881);
			richMainMonitor.BackColor = Color.White;


			//Log Class 초기화
			UI.Logs.LogMessageHandled += new UILog.LogMessageHandler(LogMessage);
			UI.Logs.ChangeLogFilePreName("MachineName");// Now Project Name ( ex:UI_BASE )
			UI.Logs.ChangeLogFolderName(@"LOG\MMI\"); // same default
			UI.Logs.ChangeLogExt("log");        // same default.
			UI.Logs.Write(LOGPART.SYS, "Program Start !!");
			//lbOkReset.BackColor = ColorTranslator.FromHtml("#7CB342");
			//lbNgReset.BackColor = ColorTranslator.FromHtml("#fa8072");
			//lbUserOKReset.BackColor = ColorTranslator.FromHtml("#add8e6");
			if (Program._bSimul_ == true)
				btnSimLotInsert.Visible = true;
		}

		#region [ Event Function ]
		private	void	LogMessage(string msg)
		{
			try
			{
				this.InvokeIfNeededC(() => RichTextBoxUpdate(msg));
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	RichTextBoxUpdate(string msg)
		{
			try
			{
				if (this.IsDisposed	== true)			return;

				// MMI 로그 출력 TextBox Clear, 메모리 문제 발생 예방
				if (richMainMonitor.Lines.Length > 1001)
					richMainMonitor.Clear();

				richMainMonitor.AppendText(msg + "\n");
				richMainMonitor.ScrollToCaret();
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
		#endregion

		#region [ timmer Function ]
		private	void	tmrSubItems_Tick(object sender, EventArgs e)
		{
			try
			{

                //	UpdateControlEnable();
                //시뮬레이션 랏 자동 투입용
                if (Program._bSimul_ == true)
                {

                    if (NDm.Read(eNDM.AutoRunMode) == (int)AutoRunMode.Normal)
                    {
                        //if (SM.FMT.IsFlow(FlowTray.LDLift01) == false && G.seqTrayLoader.IsReady == true)
                        //{
                        //    btnSimLotInsert_Click(null, null);
                        //}

                        //Random temp = new Random();
                        //if (temp.Next(5000) == 1)
                        //{
                        //    if (SM.FMT.IsFlow(FlowTray.LDTable1)) SM.FMT[FlowTray.LDTable1].SetAutoRunMode(AutoRunMode.QuitLot);
                        //    if (SM.FMT.IsFlow(FlowTray.LDTable2)) SM.FMT[FlowTray.LDTable2].SetAutoRunMode(AutoRunMode.QuitLot);
                        //    NDm.Write(eNDM.AutoRunMode, (int)AutoRunMode.EndLot);
                        //}
                        //else if (temp.Next(5000) == 2)
                        //{
                        //    if (SM.FMT.IsFlow(FlowTray.LDTable1)) SM.FMT[FlowTray.LDTable1].SetAutoRunMode(AutoRunMode.QuitLot);
                        //    if (SM.FMT.IsFlow(FlowTray.LDTable2)) SM.FMT[FlowTray.LDTable2].SetAutoRunMode(AutoRunMode.QuitLot);
                        //    NDm.Write(eNDM.AutoRunMode, (int)AutoRunMode.QuitLot);
                        //}
                    }
                }


				UpdateLamp();
				UpdateControlBlink();
				UpdateState();
				UpdateYield();
				UpdateBtnDown();
				UpdateColor();
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}


		#endregion

		#region [ Button Event ]
		private	void	btnRun_Click(object sender, EventArgs e)
		{
			try
			{
				frmMessageBox pMsgConfirm;

				//섹잼 사용시, 검사를 안하면 무조건 NG로 뺴야함
				if (SM.SHM.GetUseSkipData(USESKIP.SGEM) == true && 
					(SM.SHM.GetUseSkipData(USESKIP.TopInspection) == false  ||  SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == false)  &&
					SM.SHM.GetUseSkipData(USESKIP.InspSkipResultNG) == false  )
				{
					string strMsg =	"If you skip inspection while using Secs/gem communication"+ Environment.NewLine  + 
									"you must use the setting The Result of Skipping inspection is NG as True."+ Environment.NewLine  + 
									"Please check the Use/skip setting in Data.";
					SM.SHM.SetMsg(SeqMsg.SEQERR, strMsg);
					SM.SHM.MsgShow(SeqMsg.SEQERR);
					return;
				}

				if (SM.SHM.GetUseSkipData(USESKIP.Safety) == true && SM.SHM.IsBitData(BIT.KeyLockAuto) == false)
				{
					pMsgConfirm = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Info, Language.languageDefault.MB_KeyLockManual);
					if (pMsgConfirm.ShowDialog() == DialogResult.OK) return;
				}

				if (SM.SHM.GetUseSkipData(USESKIP.TopInspection) == false || SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == false)
				{
					pMsgConfirm = new frmMessageBox(frmMessageBox.teButtons.OKCancel, Language.languageDefault.MB_Confirmation, "Top or Bottom Inspection Option is Off" + Environment.NewLine + " Do you want to Run it? ");
					if (pMsgConfirm.ShowDialog() == DialogResult.Cancel) return;
				}

				if (SM.SHM.GetUseSkipData(USESKIP.SGEM) ==false)
				{
					pMsgConfirm = new frmMessageBox(frmMessageBox.teButtons.OKCancel, Language.languageDefault.MB_Confirmation, "MES Offline" + Environment.NewLine + "Do you want to Run it? ");
					if (pMsgConfirm.ShowDialog() == DialogResult.Cancel) return;
				}

				if (SM.SHM.GetUseSkipData(USESKIP.DRYRUN) == true)
				{
					pMsgConfirm = new frmMessageBox(frmMessageBox.teButtons.OKCancel, Language.languageDefault.MB_Confirmation, "Dry run option is on. Do you want to Run it?");
					if (pMsgConfirm.ShowDialog() == DialogResult.Cancel) return;
				}

				if (SM.SHM.GetUseSkipData(USESKIP.TopInspection) == true &&
					SM.SHM.IsBitData(BIT.TopInspConnect) == false)
				{
					pMsgConfirm = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Confirmation, "Please Check Top Inspection PC Connect");
					if (pMsgConfirm.ShowDialog() == DialogResult.OK) return;
				}

				if (SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == true &&
					SM.SHM.IsBitData(BIT.BtmInspConnect) == false)
				{
					pMsgConfirm = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Confirmation, "Please Check Bottom Inspection PC Connect");
					if (pMsgConfirm.ShowDialog() == DialogResult.OK) return;
				}

				if (SM.SHM.GetUseSkipData(USESKIP.InspDebugPlate1) == true &&
					SM.SHM.IsUsePart(UsePartName.Insp1, UsePartMode.Load) == false)
				{
					pMsgConfirm = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Confirmation, "Dubug Mode Use Plate1, Please Check Use/Skip Option");
					if (pMsgConfirm.ShowDialog() == DialogResult.OK) return;
				}

				if (SM.SHM.GetUseSkipData(USESKIP.InspDebugPlate2) == true &&
					SM.SHM.IsUsePart(UsePartName.Insp2, UsePartMode.Load) == false)
				{
					pMsgConfirm = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Confirmation, "Dubug Mode Use Plate2, Please Check Use/Skip Option");
					if (pMsgConfirm.ShowDialog() == DialogResult.OK) return;
				}

				if (SM.SHM.IsUsePart(UsePartName.Insp1, UsePartMode.Load) == false &&
					SM.SHM.IsUsePart(UsePartName.Insp2, UsePartMode.Load) == false)
				{
					pMsgConfirm = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Confirmation, "Plate1,2 Use option is off. Do you want to Run it?");
					if (pMsgConfirm.ShowDialog() == DialogResult.Cancel) return;
				}
				if (SM.SHM.GetUseSkipData(USESKIP.TopInspection) == false || SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == false)
				{
					pMsgConfirm = new frmMessageBox(frmMessageBox.teButtons.OKCancel, Language.languageDefault.MB_Confirmation, "Top or Bottom Inspection Option is Off. Do you want to Run it? ");
					if (pMsgConfirm.ShowDialog() == DialogResult.Cancel) return;
				}

				pMsgConfirm = new frmMessageBox(frmMessageBox.teButtons.OKCancel, Language.languageDefault.MB_Confirmation, Language.languageDefault.MB_AutoRunStart);
				if (pMsgConfirm.ShowDialog() == DialogResult.Cancel) return;

				if (SM.SHM.IsManualRunning() == true)		SM.SHM.StopManual();

				SM.SHM.SetButtonNum(BUTTON.START);

				UI.Logs.Write(LOGPART.SYS, "Auto Run Start Button Click");
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	btnStop_Click(object sender, EventArgs e)
		{
			try
			{
				frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OKCancel, Language.languageDefault.MB_Confirmation, Language.languageDefault.MB_AutoRunStop);
				if (pMsg.ShowDialog() == DialogResult.Cancel) return;

				SM.SHM.SetButtonNum(BUTTON.STOP);
				SM.SHM.SetBitData(BIT.AllHome, false);

				UI.Logs.Write(LOGPART.SYS, "Auto Run Stop Button Click");
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	btnPause_Click(object sender, EventArgs e)
		{
			try
			{
				SM.SHM.SetButtonNum(BUTTON.PAUSE);

				UI.Logs.Write(LOGPART.SYS, "Auto Run Pause Button Click");
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void	btnPause_MouseDown(object sender, MouseEventArgs e)
		{
			try
			{
				m_pswPauseBtnDown.Restart();

				UI.Logs.Write(LOGPART.SYS, "Auto Run Pause Button Click");
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void	btnPause_MouseUp(object sender, MouseEventArgs e)
		{
			try
			{
				if (m_pswPauseBtnDown.IsTimeOverMs(1000) == true  && 
					(Program.m_PasswordMode == PW_MODE.PRO ||
					 Program.m_PasswordMode == PW_MODE.ADMIN ))
				{
					NDm.Write(eNDM.OldAutoRunMode, NDm.Read(eNDM.AutoRunMode));
					NDm.Write(eNDM.AutoRunMode, (int)AutoRunMode.CycleStop);
					UI.Logs.Write(LOGPART.SYS, "Auto Run Cycle Stop Button Click");
				}
				else
				{
					SM.SHM.SetButtonNum(BUTTON.PAUSE);

					UI.Logs.Write(LOGPART.SYS, "Auto Run Pause Button Click");
				}

				m_pswPauseBtnDown.Stop();
				m_pswPauseBtnDown.Reset();
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	btnAllHome_Click(object sender, EventArgs e)
		{
			try
			{
				if (SM.SHM.IsBitData(BIT.AllHoming) == true)			return;

				if (SM.SHM.IsBitData(BIT.KeyLockAuto) == false && SM.SHM.GetUseSkipData(USESKIP.Safety) == true)
				{
					frmMessageBox pMsgConfirm = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Confirmation, "Key Lock is Manual.");
					if (pMsgConfirm.ShowDialog() == DialogResult.OK) return;
				}

				if (UI.SubpageRecipe.CheckAllEmpty() == false)			return;

				//모든 모터가 홈 상태일때만 레디를 선택할 수 있도록 함
				bool bAllMotorHome = true;
				for (int i = 0; i < (int)MT.TOTAL; i++)
				{
					STATUS_MOTOR stMtStatus = new STATUS_MOTOR();
					SM.SHM.GetMotorSatusData((uint)i, ref stMtStatus);
					if (stMtStatus.svStatus.HOME == false)
					{
						bAllMotorHome = false;
						break;
					}
				}
				List<string> lt_btn ;
				if (bAllMotorHome == true)		lt_btn = new List<string>() { "Cancel", "Ready", "AllHome" };
				else							lt_btn = new List<string>() { "Cancel", "AllHome" };

				frmSelectMessageBox pSelMsg = new frmSelectMessageBox(lt_btn, Language.languageDefault.MB_Confirmation, "Select Home Mode", 0);
				pSelMsg.ShowDialog();
				switch (pSelMsg.Result)
				{
					case "AllHome":
						{

							frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OKCancel, Language.languageDefault.MB_Confirmation, Language.languageDefault.MB_AllHome);
							if (pMsg.ShowDialog() == DialogResult.Cancel) return;

							SM.SHM.SetButtonParams(new object[] { true });
							SM.SHM.SetButtonNum(BUTTON.ALLHOME);

							UI.Logs.Write(LOGPART.SYS, "All Home Start Button Click");

							Form fc = Application.OpenForms["frmAllHomeMonitor"];
							if (fc != null)
							{
								if (fc.WindowState == FormWindowState.Minimized)
								{  // 폼을 최소화시켜 하단에 내려놓았는지 검사
									fc.WindowState = FormWindowState.Normal;
								}
								((frmAllHomeMonitor)fc).ResetStatus();
								fc.Activate();
								return;
							}

							frmAllHomeMonitor popup = new frmAllHomeMonitor();
							popup.Show();
						}
						break;
					case "Ready":
						{
							frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OKCancel, Language.languageDefault.MB_Confirmation, Language.languageDefault.MB_AllHome);
							if (pMsg.ShowDialog() == DialogResult.Cancel) return;

							SM.SHM.SetButtonParams(new object[] { false });
							SM.SHM.SetButtonNum(BUTTON.ALLHOME);

							UI.Logs.Write(LOGPART.SYS, "All Home Start Button Click");

						}
						break;
					case "Cancel":
						{
							return;
						}
				}

			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	btnReset_Click(object sender, EventArgs e)
		{
			try
			{
				SM.SHM.SetButtonNum(BUTTON.RESET);

				UI.Logs.Write(LOGPART.SYS, "Alarm Reset Button Click");
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	btnBuzzer_Click(object sender, EventArgs e)
		{
			try
			{
				SM.SHM.SetBitData(BIT.Buzzer, false);
				SM.SHM.SetBitData(BIT.WarningBuzzer, false);
				SM.SHM.SetBitData(BIT.RejectBuzzer, false);
				UI.Logs.Write(LOGPART.SYS, "Buzzer Off Button Click");
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void	btnFlowMapInfo_Click(object sender, EventArgs e)
		{
			try
			{
				Form	fc	= Application.OpenForms["frmFlowMapInfo"];
				if (fc != null)			return;

				frmFlowMapInfo	popup	= new frmFlowMapInfo();
				popup.Show();
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void	btnLotInfo_Click(object sender, EventArgs e)
		{
			try
			{
				if (UI.Barcode.ShowDialog() == DialogResult.OK)
				{
					if (SM.SHM.ContainsRunLotInfo(SM.FMT[FlowTray.LDWaitBuf].GetInfo().KeyValue) == true)
					{
						frmMessageBox msg_box	= new frmMessageBox(frmMessageBox.teButtons.OK, "Error", Language.languageDefault.MB_LotIDIsExist);
						msg_box.ShowDialog();
					}
				}
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void	btnTact_Click(object sender, EventArgs e)
		{
			try
			{
				Form	fc	= Application.OpenForms["frmTact"];
				if (fc != null)			return;
				
				frmTact	popup	= new frmTact();
				
				popup.Show();
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
		private int m_tempCount = 0;
		private string strModuleType = "";

		private void	btnSimLotInsert_Click(object sender, EventArgs e)
		{
			try
			{	
				
				if (G.cKernel.Motor(MT.LDTrayLiftZ).IsRdy((int)SeqTrayLoader.PosZ.UP) == false ) return;
				if (SM.FMT.IsFlow(FlowTray.LDWaitBuf) == false)  SM.FMT.Next(FlowTray.Empty, FlowTray.LDWaitBuf);
				 Random random = new Random();
				int nRow		= 0;
				int nCol		= 0;


				if (m_tempCount > 3  || strModuleType == "")
				{
					Array values = Enum.GetValues(typeof(ModuleType));
					ModuleType randomValue = (ModuleType)values.GetValue(random.Next(1, values.Length));
					strModuleType	= EnumHelper.GetDescription(randomValue);
					m_tempCount = 0 ;
				}
				else
				{
					m_tempCount++;
				}

				ModuleType module;
				if (!Enum.TryParse(strModuleType, out module))
				{
					module = ModuleType.UNKNOWN; // 변환 실패 시 기본값
				}
				
				nRow = SM.SHM.GetRecipeDataToInt(strModuleType, DEVICE.TrayRowCnt) / 1000;
				nCol = SM.SHM.GetRecipeDataToInt(strModuleType, DEVICE.TrayColCnt) / 1000;

				
				int nProdCount	= random.Next(20, 200);
				var	tcinfo					= SM.FMT[FlowTray.LDWaitBuf].GetInfo();
				tcinfo.WorkOrder			= DateTime.Now.ToString("HH:mm");
				tcinfo.ProgramID_Top		= DateTime.Now.ToString("HH:mm:ss")+"_TOP";
				tcinfo.ProgramID_Btm		= DateTime.Now.ToString("HH:mm:ss")+"_BTM";

				tcinfo.ModuleType = strModuleType;
				//tcinfo.ModuleGen = strModuleGen;
				tcinfo.ColCnt = 25;
				tcinfo.RowCnt = nRow;
				tcinfo.ColCnt = nCol;

				tcinfo.MaxCntProdInLot = nProdCount;

				int MaxProdCount = tcinfo.ColCnt * tcinfo.RowCnt * 14;

				SM.FMT[FlowTray.LDWaitBuf].SetInfo(tcinfo);

				SM.SHM.SetBitData(BIT.LotLoadStart, true);
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void	btnYieldClear_Click(object sender, EventArgs e)
		{
			try
			{
				
				frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OKCancel, Language.languageDefault.MB_Confirmation, Language.languageDefault.MB_ClearYield);
				if (pMsg.ShowDialog() == DialogResult.Cancel)			return;

				
				int nOKCount = NDm.Read(eNDM.CurOKProdCount);
				int nNGCount = NDm.Read(eNDM.CurNGProdCount);
				int nUserOKCount = NDm.Read(eNDM.CurUserOKProdCount);
				string str = string.Format("MANUAL DAILY YIELD CLEAR! TOTAL = {0}, OK = {1} , NG = {2} , USER OK = {3}     ", nOKCount + nNGCount + nUserOKCount, nOKCount, nNGCount, nUserOKCount);
				UI.Logs.Write(LOGPART.DATA, str);

				NDm.Write(eNDM.CurOKProdCount, 0);
				NDm.Write(eNDM.CurNGProdCount, 0);
				NDm.Write(eNDM.CurUserOKProdCount, 0);
				G.commInspTop.SendYieldClear();
				G.commInspBtm.SendYieldClear();
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
		
		private void    lbState_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (SM.SHM.IsBitData(BIT.AutoRun) == true)			return;

			Label lb	= (Label)sender;

			switch (lb.Name)
			{
				case "lbTopState" :
				case "lbBtmState" :
				case "lbRevState" :
				case "lbMESState" :
					frmIPSetting IpSetting = new frmIPSetting();
					IpSetting.Show(sender);
					break;
				
            }
		}

		private bool IsMachineEmpty()
		{
		//	if (SM.FMT.GetExistCountRange(teFlowTray.LDLift01, teFlowTray.ULDConv15)

			if (SM.FMT.IsFlowRange(FlowTray.LDLift01, FlowTray.ULDConv31) == true)			return false;
			if (SM.FMP.IsFlowRange(FlowPart.LDPnP, FlowPart.ULDPnP) == true)				return false;
			if (SM.FMT[FlowTray.Reject].GetExistCount() > 0)								return false;

			return true;
		}
		#endregion

		#region [ Function ]

		#region - Private Function
		private	int		GetRecipeNoFromPartNo(string strPartNo)
		{
			try
			{
				XML			xml		= new XML();
				xml.Initializ("RecipeManager");

				int		ncnt_row	= xml.GetDataTableCount();
				int		nchg_rcp	= 0;
				string	str_part_no	= "";

				for (int i = 0; i < ncnt_row; i++)
				{
					str_part_no	= xml.ReadDataTable(i, "PartNo");
				
					if (strPartNo.ToUpper() != str_part_no.ToUpper())			continue;

					string[]	str_no	= xml.ReadDataTable(i, "Recipe").Split('.');

					if (int.TryParse(str_no[0], out nchg_rcp) == false)
					{
						return -1;
					}
					break;
				}

				if (nchg_rcp < 1)
				{
					return -2;
				}

				return nchg_rcp-1;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -3;
			}
		}
		
		private void	SetButtonEnabled(Button Btn, bool bEnabled)
		{
			try
			{
				if (Btn.Enabled != bEnabled)		Btn.Enabled = bEnabled;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		#endregion

		#region - Public Function
	
        public void DisableSubButton()
        {
			SetButtonEnabled(btnRun,		false);
			SetButtonEnabled(btnAllHome,	false);
            //비활성화 할 버튼 추가
        }
		
        public void EnableSubButton()
        {
			if (SM.SHM.IsBitData(BIT.AutoRun) == false  &&
				SM.SHM.IsBitData(BIT.Error) == false  &&
				SM.SHM.IsBitData(BIT.Pause) == false)
			{
				SetButtonEnabled(btnAllHome, true);
			}
			else
			{
				SetButtonEnabled(btnAllHome, false);
			}

			if (SM.SHM.IsBitData(BIT.AllHome) == true  &&
				SM.SHM.IsBitData(BIT.AllHoming) == false  &&
				SM.SHM.IsBitData(BIT.Error) == false  &&
				SM.SHM.IsBitData(BIT.AutoRun) == false)
			{
				SetButtonEnabled(btnRun, true);
			}
			else
			{
				SetButtonEnabled(btnRun, false);
			}

			if (SM.SHM.IsBitData(BIT.AutoRun) == true)
			{
				SetButtonEnabled(btnPause, true);
			}
			else
			{
				SetButtonEnabled(btnPause, false);
			}

			//활성화 할 버튼 추가
        }

		#endregion

		#region - Language Function 

		public	void	SetLanguageText()
		{
			try
			{
				btnAllHome.Text = Language.languageDefault.AllHome;
				btnReset.Text = Language.languageDefault.AlarmReset;

				lbOkReset.Text = Language.languageDefault.GOOD;
				lbNgReset.Text = Language.languageDefault.NG;
				lbUserOKReset.Text = Language.languageDefault.UserOK;
				lbTotal.Text = Language.languageDefault.Total;
				lbYeild.Text = Language.languageDefault.Yield;
				btnYieldClear.Text = Language.languageDefault.Clear;

				gbConnectionState.Text = Language.languageDefault.ConnectState;

				lbTopState.Text = Language.languageDefault.TopState;
				lbBtmState.Text = Language.languageDefault.BtmState;
				lbControlState.Text = Language.languageDefault.WMXState;
				lbRevState.Text = Language.languageDefault.RevState;

				btnTact.Text = Language.languageDefault.Tact;
				btnLotInfo.Text = Language.languageDefault.LotInfo;
				btnFlowMapInfo.Text = Language.languageDefault.FlowMapInfo;
				btnBuzzer.Text = Language.languageDefault.Buzzer;


			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
		#endregion
		#endregion
		private	void	UpdateLamp()
		{
			try
			{
				if (SM.SHM.IsBitData(BIT.ControlDeviceState) == true)			SetControlColor(lbCtrlLed, Color.Lime,		Color.Black);
				else															SetControlColor(lbCtrlLed, Color.DarkGray,	Color.Black);

				if (SM.SHM.IsBitData(BIT.TopInspConnect) == true)				SetControlColor(lbTopLed, Color.Lime,		Color.Black);
				else															SetControlColor(lbTopLed, Color.DarkGray,	Color.Black);

				if (SM.SHM.IsBitData(BIT.BtmInspConnect) == true)				SetControlColor(lbBtmLed, Color.Lime,		Color.Black);
				else															SetControlColor(lbBtmLed, Color.DarkGray,	Color.Black);
				
				if (SM.SHM.IsBitData(BIT.ReviewConnect) == true)				SetControlColor(lbRevLed, Color.Lime,		Color.Black);
				else															SetControlColor(lbRevLed, Color.DarkGray,	Color.Black);
	
				if (SM.SHM.IsBitData(BIT.UbiGemConnect) == true)				SetControlColor(lbGEMLed, Color.Lime,		Color.Black);
				else															SetControlColor(lbGEMLed, Color.DarkGray,	Color.Black);
				
				if (SM.SHM.IsBitData(BIT.TotalBarcodeConnect) == true)			SetControlColor(lbTrayBCRLed, Color.Lime,		Color.Black);
				else															SetControlColor(lbTrayBCRLed, Color.DarkGray,	Color.Black);

				if (SM.SHM.IsBitData(BIT.SmartVisionLoaderConnect) == true)			SetControlColor(lbSmartVision1Led, Color.Lime,		Color.Black);
				else															SetControlColor(lbSmartVision1Led, Color.DarkGray,	Color.Black);

				if (SM.SHM.IsBitData(BIT.SmartVisionTableConnect) == true)			SetControlColor(lbSmartVision2Led, Color.Lime,		Color.Black);
				else															SetControlColor(lbSmartVision2Led, Color.DarkGray,	Color.Black);
				
				if (SM.SHM.GetUseSkipData(USESKIP.TopInspection) == false)		SetControlColor(lbTopState, Color.Red,   	Color.White);
				else															SetControlColor(lbTopState, Color.White,   	Color.Black);
				
				if (SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == false)		SetControlColor(lbBtmState,  Color.Red,   	Color.White);
				else															SetControlColor(lbBtmState, Color.White,   	Color.Black);
				
				if (SM.SHM.GetUseSkipData(USESKIP.SendRejectProdId) == false)	SetControlColor(lbRevState, Color.Red,	Color.White);
				else															SetControlColor(lbRevState, Color.White,   	Color.Black);
				
				if (SM.SHM.GetUseSkipData(USESKIP.BarcodeRead) == false)		SetControlColor(lbTrayBCRState,  Color.Red,   	Color.White);
				else															SetControlColor(lbTrayBCRState, Color.White,   	Color.Black);

				if (SM.SHM.GetUseSkipData(USESKIP.SmartVisionLoader) == false)	SetControlColor(lbSmartVision1State,  Color.Red,   	Color.White);
				else															SetControlColor(lbSmartVision1State, Color.White,   Color.Black);

				if (SM.SHM.GetUseSkipData(USESKIP.SmartVisionTable) == false)	SetControlColor(lbSmartVision2State,  Color.Red,   	Color.White);
				else															SetControlColor(lbSmartVision2State, Color.White,   Color.Black);
				
				if (SM.SHM.GetUseSkipData(USESKIP.SGEM) ==false)				SetControlColor(lbMESState,  Color.Red,   	Color.White);
				else															SetControlColor(lbMESState, Color.White,   	Color.Black);

				if (SM.SHM.IsBitData(BIT.Error) == true)						SetControlColor(btnReset, Color.OrangeRed,	Color.Black);
				else															SetControlColor(btnReset, Color.GhostWhite,	Color.Black);
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	UpdateControlBlink()
		{
			try
			{
				bool	bblink	= m_blinkTmr.Blink();

				if ((SM.SHM.IsBitData(BIT.Buzzer) == true || SM.SHM.IsBitData(BIT.WarningBuzzer) == true || SM.SHM.IsBitData(BIT.RejectBuzzer) == true)  &&  bblink == true)			SetButtonImage(btnBuzzer, Properties.Resources.BellOn);
				else																																							SetButtonImage(btnBuzzer, Properties.Resources.BellOff);

				if (SM.SHM.IsBitData(BIT.AllHoming) == true  &&  bblink == true)		SetButtonImage(btnAllHome, Properties.Resources.HomeOn);
				else																	SetButtonImage(btnAllHome, Properties.Resources.HomeOff);
				
				SetControlColor(btnReset, Color.GhostWhite, Color.Black);
				SetControlEnable(btnReset, true);
				 //임시 주석처리
				if (SM.SHM.IsBitData(BIT.Error) == true)
				{
					if (SM.SHM.GetUseSkipData(USESKIP.SGEM) == true)
					{
						if (G.seqUBIGEMComm.Result_AlarmSet == true)
						{
							SetControlColor(btnReset, Color.OrangeRed, Color.Black);
							SetControlEnable(btnReset, true);
						}
						else
						{
							SetControlColor(btnReset, Color.GhostWhite, Color.Black);
							SetControlEnable(btnReset, false);
						}
					}
					else
					{
						SetControlColor(btnReset, Color.OrangeRed, Color.Black);
						SetControlEnable(btnReset, true);
					}
				}
				else
				{
					SetControlColor(btnReset, Color.GhostWhite, Color.Black);
					SetControlEnable(btnReset, true);
				}
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	UpdateState()
		{
			try
			{
				if		(SM.SHM.IsBitData(BIT.AutoRun) == true)
				{
					if (SM.SHM.IsBitData(BIT.IDLE) == true)
					{
						lbMcState.BackColor = Color.White;
						lbMcState.ForeColor = Color.Black;
						lbMcState.Text	= "IDLE";
					}
					else
					{
						lbMcState.BackColor = Color.Lime;
						lbMcState.ForeColor = Color.Black;
						lbMcState.Text = "RUN";
					}

				}
				else if (SM.SHM.IsBitData(BIT.Pause) == true)
				{
					lbMcState.BackColor = Color.Blue;
					lbMcState.ForeColor = Color.White;
					lbMcState.Text = "PAUSE";
				}
				else
				{
					lbMcState.BackColor = Color.Red;
					lbMcState.ForeColor = Color.White;
					lbMcState.Text = "STOP";
				}

				if (SM.SHM.GetUseSkipData(USESKIP.SGEM) == false)
				{
					btnLotInfo.Visible = true;
				}
				else
				{
					btnLotInfo.Visible = false;
				}
				
				if (UI.ModalLogin.IsLogIn == false ||  Program.m_PasswordMode == PW_MODE.OPER)
				{
					btnFlowMapInfo.Visible = false;
				}
				else
				{
					btnFlowMapInfo.Visible = true;
				}
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void	UpdateYield()
		{
			try
			{
				//Daily Lot 업데이트
				int nOKCount = NDm.Read(eNDM.CurOKProdCount);
				int nNGCount = NDm.Read(eNDM.CurNGProdCount);
				int nUserOKCount = NDm.Read(eNDM.CurUserOKProdCount);

				lbOkCount.Text				= nOKCount.ToString();
				lbNgCount.Text				= nNGCount.ToString();
				lbUserOKCount.Text			= nUserOKCount.ToString();
				lbTotalCount.Text			= (nOKCount + nNGCount + nUserOKCount).ToString();
				if(nOKCount + nNGCount != 0)
				{
					lbYeildCount.Text = string.Format("{0:0.00}",((float)(nOKCount + nUserOKCount) * 100 / (nOKCount + nNGCount + nUserOKCount))) + "%";
				}
				else
				{
					lbYeildCount.Text = "0.00%";
				}

                if (NDm.Read(eNDM.DateTime) != DateTime.Now.Day)
                {
                    SM.SHM.SetBitData(BIT.NewDay, true);
                    NDm.Write(eNDM.DateTime, DateTime.Now.Day);
                }

                //Daily Lot 초기화 , 현재 자재가 없을땐 여기서 초기화 진행, 자재가 있는 경우 현재 언로드 중인 랏이 변경될때 초기화 진행
                if (SM.SHM.IsBitData(BIT.NewDay) == true  &&
					DateTime.Now.Hour >= NDm.Read(eNDM.ResetHourTotalYeild)  &&
					SM.SHM.GetRunLotInfo(0).WorkOrder == "")
				{
					nOKCount = NDm.Read(eNDM.CurOKProdCount);
					nNGCount = NDm.Read(eNDM.CurNGProdCount);
					nUserOKCount = NDm.Read(eNDM.CurUserOKProdCount);

					string str = string.Format("DAILY YIELD CLEAR! TOTAL = {0}, OK = {1} , NG = {2} , USER OK = {3}     ", nOKCount + nNGCount + nUserOKCount, nOKCount, nNGCount, nUserOKCount);
					NDm.Write(eNDM.CurOKProdCount, 0);
					NDm.Write(eNDM.CurNGProdCount, 0);
					NDm.Write(eNDM.CurUserOKProdCount, 0);
					SM.SHM.SetBitData(BIT.NewDay, false);
					UI.Logs.Write(LOGPART.DATA, str);
				}

				//Cur Lot 업데이트
				int[] tempYield = { 0, 0, 0 }; //ok , ng, user-ok
				string tempCurKeyValue = SDm.Read(eSDM.CurKeyValue);

				tempYield	= SM.SHM.GetLotYield(tempCurKeyValue);

				nOKCount		= tempYield[0];
				nNGCount		= tempYield[1];
				nUserOKCount	= tempYield[2];

				lbCurLotQty.Text	= SM.SHM.GetRunLotInfo(tempCurKeyValue).ProdCount;
				
				lbCurLotOK.Text		= nOKCount.ToString();
				lbCurLotNG.Text		= nNGCount.ToString();
				lbCurLotUserOK.Text	= nUserOKCount.ToString();
				lbCurLotTotal.Text	=  (nOKCount + nNGCount + nUserOKCount).ToString();
				if(nOKCount + nNGCount != 0)
				{
					lbCurLotYield.Text = string.Format("{0:0.00}",((float)(nOKCount + nUserOKCount) * 100 / (nOKCount + nNGCount + nUserOKCount))) + "%";
				}
				else
				{
					lbCurLotYield.Text = "0.00%";
				}
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
		
		private	void	UpdateControlEnable()
		{
			try
			{
				if (SM.SHM.IsBitData(BIT.AutoRun) == false  &&
					SM.SHM.IsBitData(BIT.Pause) == false  &&
					SM.SHM.IsBitData(BIT.Error) == false)							SetControlEnable(btnAllHome, true);
				else																SetControlEnable(btnAllHome, false);

				if (UI.PageTenKey.IsWorking() == true  ||
					UI.PageMotor.IsWorking() == true  ||
					UI.PageIO.IsWorking() == true  ||
					UI.SubpageRecipe.IsWorking() == true  ||
					UI.SubpageDm.IsWorking() == true  ||
					UI.SubpageSDm.IsWorking() == true  ||
					UI.SubpageUseskip.IsWorking() == true  ||
					UI.PageError.IsWorking() == true  ||
					UI.SubpageBit.IsWorking() == true)
				{
					SetControlEnable(btnRun, false);
				}
				else if (SM.SHM.IsBitData(BIT.AutoRun) == false  &&
						 SM.SHM.IsBitData(BIT.AllHoming) == false  &&
						 SM.SHM.IsBitData(BIT.AllHome) == true  &&
						 SM.SHM.IsBitData(BIT.Error) == false  &&
						 (SM.SHM.IsBitData(BIT.UBIGEMCheckingEquipment) == false  ||
						  SM.SHM.GetUseSkipData(USESKIP.SGEM) == true))
				{
					SetControlEnable(btnRun, true);
				}
				else
				{
					SetControlEnable(btnRun, false);
				}

				if (SM.SHM.IsBitData(BIT.AutoRun) == true)							SetControlEnable(btnPause, true);
				else																SetControlEnable(btnPause, false);
	
				
				if (SM.SHM.IsBitData(BIT.Error) == true )
				{
					if (SM.SHM.GetUseSkipData(USESKIP.SGEM) == true )
					{
						if (G.seqUBIGEMComm.Result_AlarmSet == true)
						{
							SetControlColor(btnReset, Color.OrangeRed, Color.Black);
							SetControlEnable(btnReset, true);
						}
						else
						{
							SetControlColor(btnReset, Color.GhostWhite, Color.Black);
							SetControlEnable(btnReset, false);
						}
					}
					else 
					{
						SetControlColor(btnReset, Color.OrangeRed, Color.Black);
						SetControlEnable(btnReset, true);
					}
				}
				else
				{
					SetControlColor(btnReset, Color.GhostWhite, Color.Black);
					SetControlEnable(btnReset, false);
				}

			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	UpdateBtnDown()
		{
			try
			{
				//Pause
				int	npause_val	= (int)m_pswPauseBtnDown.ElapsedMilliseconds;
				progPause.Value	= npause_val > 1000 ? 1000 : npause_val;
			
				//Run
				int	nrn_val		= NDm.Read(eNDM.AutoRunSWDown);
				progRun.Value	= nrn_val > 1000 ? 1000 : nrn_val;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	UpdateColor()
		{
			Color newColor = (Color)m_ColorConverter.ConvertFromString(SDm.Read(eSDM.UserNGColor));
			if (lbNgReset.BackColor != newColor)
			{
				lbNgReset.BackColor = newColor;
			}

			newColor = (Color)m_ColorConverter.ConvertFromString(SDm.Read(eSDM.OKColor));
			if (lbUserOKReset.BackColor != newColor)
			{
				lbUserOKReset.BackColor = newColor;
				lbOkReset.BackColor = newColor;
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

		private	void	SetControlColor(DataGridViewCellStyle CellStyle, Color BackColor, Color ForeColor)
		{
			try
			{
				if (CellStyle.BackColor != BackColor)			CellStyle.BackColor = BackColor;
				if (CellStyle.ForeColor != ForeColor)			CellStyle.ForeColor = ForeColor;
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

		private	void	SetButtonImage(Button Btn, Image image)
		{
			try
			{
				if (Btn.Image != image)			Btn.Image = image;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	SetButtonBackgroundImage(Button Btn, Image image)
		{
			try
			{
				if (Btn.BackgroundImage != image)			Btn.BackgroundImage = image;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
	}
}
