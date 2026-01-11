using H_MarsSSD.Properties;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace H_MarsSSD
{
	public partial class Main : Form
	{
		public Main()
		{
			if (MultiCheck())
				MultiKill();

			if (!SM.SHM.Initialization())
			{
				MessageBox.Show("Not Connect Shared Memory!!");
				this.Close();
			}

			if (Screen.AllScreens.Count() > 1)
			{
				//this.showOnMonitor(1);
			}

			InitializeComponent();
			InitFromPage();
			initTimerWatch();

			SM.SHM.SeqInitializ();
		}

		#region [ Variables ]

		// 외부 프로그램 시작 사용 설정
		private bool ExtProgramUse = false;

		// 메인 타이머 플래그.
		private const string	sColIDLang = "Language";

		private bool	bFlag			= false;
		private bool	bFlagOld		= false;
		private XML		xmlMain			= new XML();
		private Random	m_result_random	= new Random();

		#endregion

		#region [ Event Function ]
		
		private void Main_Load(object sender, EventArgs e)
		{
			//Form Color Change
			ChangColorStyle(Color.GhostWhite);

			// Check Database Connection. 
			if (InitialXML() == false) return;

			// 타이머 초기화
			initTimer();

			//-- 외부프로그램 실행
			//ExternalProgramStart();

			// recipe data
			SM.SHM.SetRecipeData(UI.SubpageRecipe.strRecipeData);

			// Use/Skip Data
			SM.SHM.SetUseSkipData(UI.SubpageUseskip.ushUseSkip);

			//// Motor Table
			for (int i = 0; i < UI.PageMotor.iMotorNumNow; i++)
			{
				SM.SHM.SetMotorTableData((uint)i, UI.PageMotor.arrMotorTable[i]);
			}
			
			UI.PageError.tmrError.Start();

			//-- 언어 변경
			initializeLanguage();
			
			lbxVersion.Text = "Ver." + Settings.Default._mmiVersion;
			
			if (Program._bSimul_ == true)			lbSimMode.Visible	= true;
			else									lbSimMode.Visible	= false;

			lbSimMode.Text	= string.Format("SIMULATION MODE - Motor Velocity X{0}", NDm.Read(eNDM.SimMotorSpeedRate));
			
            UI.PageManualCyl.BtnMotorClick	+= PageManualCyl_btnMotorClick;
            UI.PageMotor.btnCylClick		+= PageManual_CylBtnClick;
			UI.SeqMessageBox.btnEtcClick    += PageManual_EtcBtnClick;
		}

		private void Main_FormClosed(object sender, FormClosedEventArgs e)
		{
			UI.Logs.Write(LOGPART.SYS, "Program Close !!");

			//ExternalProgramStop();

			G.CtrlDispose();

			Application.Exit();
			return;
		}

		protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
		{
			Keys key = keyData & ~(Keys.Shift | Keys.Control);
			switch (key)
			{
				case Keys.I:
					if ((keyData & Keys.Control) != 0)
					{
						Form fc = Application.OpenForms["frmOtherIO"];
						if (fc != null) break;

						frmOtherIO pOtherIO = new frmOtherIO();
						pOtherIO.SetLanguageText();
						pOtherIO.Show();
					}
					break;
				case Keys.X:
					{
						if ((keyData & Keys.Control) != 0)
						{
							frmMessageBox pMsg;
							pMsg = new frmMessageBox(frmMessageBox.teButtons.OKCancel, Language.languageDefault.MB_Confirmation, "Do You Want to All Verify?");
							if (pMsg.ShowDialog() == DialogResult.Cancel) break;

							var	tsInfo	= SM.FMT[FlowTray.Reject].GetInfo();

							for (int i = 0; i < tsInfo.MaxCntProdInTray; i++)
							{
								if (tsInfo.ProdInfo[i].Exist == true)
								{
									if (tsInfo.ProdInfo[i].VerifyBtmDone == true &&
										tsInfo.ProdInfo[i].VerifyTopDone == true)
									{
										continue;
									}

									ProdStatus Result = ProdStatus.OK;
									
									tsInfo.ProdInfo[i].ResultTop = (int)Result;
									tsInfo.ProdInfo[i].ResultBtm = (int)Result;
									
									SM.FMT[FlowTray.Reject].SetInfo(tsInfo);
									SM.FMT[FlowTray.Reject].SetVerifyDone(i, true, true);
								}
							}
						}
					}
					break;

				case Keys.Z:
					{
						if ((keyData & Keys.Control) != 0)
						{
							frmMessageBox pMsg;
							pMsg = new frmMessageBox(frmMessageBox.teButtons.OKCancel, Language.languageDefault.MB_Confirmation, "Do You Want to All Delete?");
							if (pMsg.ShowDialog() == DialogResult.Cancel) break;

							SM.FMP.FlowClearAll();
							SM.FMT[FlowTray.Reject].ClearProdAll();
							SM.FMT.FlowClearAll();
							SM.FMR.FlowClearAll();
							NDm.Write(eNDM.FillDoneRemainCount, 0);
							SM.SHM.ClearLotYield();
							SM.SHM.ClearRunLotInfo();
							

						}
					}
					break;
			}
			return base.ProcessCmdKey(ref msg, keyData);

		}
#endregion

#region [ Button Event ]
		
        private void PageMotor_btnCylClick()
        {
            btnManual.PerformClick();
            UI.PageManual.ManualCylButtonClick();
        }

        private void PageManualCyl_btnMotorClick(int nMtIdx)
        {
            UI.PageMotor.SetSelectedMotorIndex (nMtIdx);
            btnMotor.PerformClick();
        }

		private void btnX_Quit_Click(object sender, EventArgs e)
		{
			string msg = "Do you want to end it?";
			frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OKCancel, Language.languageDefault.MB_Confirmation, msg);
			if (pMsg.ShowDialog() == DialogResult.Cancel) return;
			
			Close();
		}
		
		private void btnMain_Click(object sender, EventArgs e)
		{
			if (btnMain.BackColor == System.Drawing.Color.SteelBlue) return;

			Hide_AllPage();
			UI.PageMainDisp.Show();
			btnMain.BackColor = System.Drawing.Color.SteelBlue;
		}

		private void btnMotor_Click(object sender, EventArgs e)
		{
			if (btnMotor.BackColor == System.Drawing.Color.SteelBlue) return;
			
			Hide_AllPage();
			UI.PageMotor.Show();
			btnMotor.BackColor = System.Drawing.Color.SteelBlue;
		}

		private void btnIO_Click(object sender, EventArgs e)
		{
			if (btnIO.BackColor == System.Drawing.Color.SteelBlue) return;
			
			Hide_AllPage();
			UI.PageIO.Show();
			btnIO.BackColor = System.Drawing.Color.SteelBlue;
		}

		private void btnManual_Click(object sender, EventArgs e)
		{
			if (btnManual.BackColor == System.Drawing.Color.SteelBlue) return;
		   
			Hide_AllPage();
			UI.PageManual.Show();
			btnManual.BackColor = System.Drawing.Color.SteelBlue;
		}

		private void btnData_Click(object sender, EventArgs e)
		{
			if (btnData.BackColor == System.Drawing.Color.SteelBlue) return;
			
			Hide_AllPage();
			UI.SubpageRecipe.UpdateRecipeData();
			UI.PageData.Show();
			btnData.BackColor = System.Drawing.Color.SteelBlue;
		}

		private void btnLog_Click(object sender, EventArgs e)
		{
			if (btnLog.BackColor == System.Drawing.Color.SteelBlue) return;
			
			try
			{
				Hide_AllPage();
				UI.PageLog.Show();
				btnLog.BackColor = System.Drawing.Color.SteelBlue;
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

		}

		private void btnError_Click(object sender, EventArgs e)
		{
			Hide_AllPage();
			UI.PageError.Show();
			btnError.BackColor = System.Drawing.Color.SteelBlue;
		}

		private void btnMinimization_Click(object sender, EventArgs e)
		{
			WindowState = FormWindowState.Minimized;
		}
		
		private void PageManual_CylBtnClick()
		{
			btnManual.PerformClick();
			UI.PageManual.ManualCylButtonClick();
		}
		
		private void PageManual_EtcBtnClick()
		{
			btnManual_Click(null,null);
			UI.PageManual.ManualEtcButtonClick();
		}
		
		private void btnLanguage_Click(object sender, EventArgs e)
		{            
			frmLanguageSelect frmSelect = new frmLanguageSelect();
			Program.m_Languageidx = (int)frmSelect.ShowDialog() - 1;  //DialogResult 리턴값으로 사용.          

			xmlMain.WriteDataTable(0, sColIDLang, Program.m_Languageidx.ToString());
			xmlMain.UpdateData();

			initializeLanguage();
		}
		
#endregion

#region [ 외부 프로그램 실행 및 종료 ]
		//////////////////////////////////////////////////////////////////////////
		// 외부 프로그램 시작 시키는 함수. (시퀀스 프로그램 같은...)
		// fullpath 경로만 수정해 주면 됨.

		Process ExtProgram;

		private void ExternalProgramStart()
		{
			if (!ExtProgramUse) return;

			ProcessStartInfo startinfo = new ProcessStartInfo();

			startinfo.FileName = "C:/BIN/LTK_Seq.exe";
			startinfo.CreateNoWindow = false;
			//startinfo.WindowStyle = ProcessWindowStyle.Hidden;
			try
			{
				ExtProgram = Process.Start(startinfo);
			}
			catch (Exception e)
			{
				string str = string.Format("SEQ : {0}", e.Message);
				UI.Logs.Write(LOGPART.EX, str);
			}
		}

		private void ExternalProgramStop()
		{
			if (!ExtProgramUse) return;

			try
			{
				ExtProgram.Kill();
			}
			catch (Exception e)
			{
				string str = string.Format("SEQ : {0}", e.Message);
				UI.Logs.Write(LOGPART.EX, str);
				//return;
			}
		}
#endregion

#region [ 프로그램 중복 체크 ]
		/// <summary>
		/// 동일 프로그램 채크.
		/// </summary>
		/// <returns>동일 프로그램이 실행되는지 여부.</returns>
		private static bool MultiCheck()
		{
			bool check = false;
			System.Diagnostics.Process thisproc = System.Diagnostics.Process.GetCurrentProcess();
			System.Diagnostics.Process[] procs = System.Diagnostics.Process.GetProcessesByName(thisproc.ProcessName);
			check = procs.Count() > 1;
			if (check)
			{
				MessageBox.Show("The program is already running.", thisproc.ProcessName);
			}
			return check;
		}

		/// <summary>
		/// 동일 프로그램은 모두 종료.
		/// </summary>
		private static void MultiKill()
		{
			System.Diagnostics.Process thisproc = System.Diagnostics.Process.GetCurrentProcess();
			thisproc.Kill();
		}
#endregion

#region [ 폼 초기화 ]
		private void InitFromPage()
		{
			UI.PageSubItems.TopLevel = false;
			UI.PageMainDisp.TopLevel = false;
			UI.PageMotor.TopLevel = false;
			UI.PageIO.TopLevel = false;
			UI.PageManual.TopLevel = false;
			UI.PageData.TopLevel = false;
			UI.PageLog.TopLevel = false;
			UI.PageError.TopLevel = false;
			UI.ErrorRecipeDisp.TopLevel = false;

			Hide_AllPage();
			UI.PageMainDisp.Show(); // 최초 보이는 페이지
			UI.PageSubItems.Show();
			btnMain.BackColor = System.Drawing.Color.SteelBlue;

			UI.ErrorRecipeDisp.Show();

			//-- 패널박스에 페이지 추가.....          
			this.plRight.Controls.Add(UI.PageSubItems);

			this.plMainCenter.Controls.Add(UI.PageMainDisp);
			this.plMainCenter.Controls.Add(UI.PageMotor);
			this.plMainCenter.Controls.Add(UI.PageIO);
			this.plMainCenter.Controls.Add(UI.PageManual);
			this.plMainCenter.Controls.Add(UI.PageData);
			this.plMainCenter.Controls.Add(UI.PageLog);
			this.plMainCenter.Controls.Add(UI.PageError);

			this.panelErr.Controls.Add(UI.ErrorRecipeDisp);
		}

		private void ChangColorStyle(Color _color)
		{
			//             this.BackColor = _color;
			UI.PageMainDisp.BackColor	= _color;
			UI.PageMotor.BackColor		= _color;
			UI.PageIO.BackColor			= _color;
			UI.PageManual.BackColor		= _color;
			UI.PageManualSeq.BackColor	= _color;
			UI.PageManualCyl.BackColor	= _color;
			UI.PageManualTCP.BackColor	= _color;
			UI.PageManualEtc.BackColor	= _color;

			UI.PageData.BackColor		= _color;
			UI.PageLog.BackColor		= _color;
			UI.PageError.BackColor		= _color;
			UI.PageTenKey.BackColor		= _color;

			UI.SubpageRecipe.BackColor	= _color;
			UI.SubpageDm.BackColor		= _color;
			UI.SubpageSDm.BackColor		= _color;
			UI.SubpageUseskip.BackColor	= _color;
			UI.SubpageBit.BackColor		= _color;

			UI.Barcode.BackColor		= _color;
		}

		private void Hide_AllPage()
		{
			// 페이지 추가시 추가해주세요~!
			UI.PageMainDisp.Hide();
			UI.PageMotor.Hide();
			UI.PageIO.Hide();
			UI.PageManual.Hide();
			UI.PageData.Hide();
			UI.PageLog.Hide();
			UI.PageError.Hide();
			UI.Barcode.Hide();

			btnMain.BackColor = System.Drawing.Color.White;
			btnMotor.BackColor = System.Drawing.Color.White;
			btnIO.BackColor = System.Drawing.Color.White;
			btnManual.BackColor = System.Drawing.Color.White;
			btnData.BackColor = System.Drawing.Color.White;
			btnLog.BackColor = System.Drawing.Color.White;
			btnError.BackColor = System.Drawing.Color.White;
		}
#endregion

#region [ 이벤트 / 타이머 ]

#region - private variables
		private Timer tmrDispView = new Timer();
		private	frmMessageBox	m_VerifyMsg;
#endregion

#region - Function
		private void initQueueRecv()
		{
		}        

		private void initTimer()
		{
			tmrDispView.Enabled = true;
			tmrDispView.Interval = 100;
			tmrDispView.Tick += TmrDispView_Tick;
		}

		private void StartStateTimeUpdate()
		{
			Task.Run(async () =>
			{
				while (true)
				{
					try
					{
						if (SM.SHM.IsBitData(BIT.AutoRun) == true )
						{
							if (SM.SHM.GetCurrentState() == RunStateType.NormalRun &&
								SM.FMP.IsFlow(FlowPart.Insp1) == false &&
								SM.FMP.IsFlow(FlowPart.Insp1) == false)
							{
								SM.SHM.ChangeState(RunStateType.CameraIdle);
							}
							else if (SM.SHM.GetCurrentState()  == RunStateType.CameraIdle  &&
									 (SM.FMP.IsFlow(FlowPart.Insp1) == true ||
									  SM.FMP.IsFlow(FlowPart.Insp1) == true) )
							{
								SM.SHM.ChangeState(RunStateType.NormalRun);
							}
						}
						SM.SHM.UpdateDailyStateTime();      // 60초 주기 포함되어 있음

						await Task.Delay(TimeSpan.FromSeconds(1));
					}
					catch (Exception ex)
					{
						G.AlmMng.ADD(ALM.ERROR_EXCEPTION, $"[BackgroundUpdateLoop] {ex.Message}\n{ex.StackTrace}");
					}
				}
			});
		}

		#endregion

		#region - Event
		//private void TmrCheckSeqRun_Tick(object sender, EventArgs e)
		//{



		//}

		//private void TmrMain_Tick(object sender, EventArgs e)
		//{

		//}

		private void TmrDispView_Tick(object sender, EventArgs e)
        {            
            // 사이드 페이지로 이동
            bFlag = UI.SubpageBit.IsBitData(BIT.AutoRun);

            if (bFlag && bFlagOld != bFlag) // Autorun Start 시
            {
                UI.PageTenKey.WorkEnd();
                UI.PageMotor.WorkEnd();
                UI.PageIO.WorkEnd();
                UI.SubpageRecipe.WorkEnd();
                UI.SubpageDm.WorkEnd();
                UI.SubpageSDm.WorkEnd();
                UI.SubpageUseskip.WorkEnd();
                UI.PageError.WorkEnd();
                UI.SubpageBit.WorkEnd();

                MainButtonEnable();
                UI.PageSubItems.EnableSubButton();
                UI.PageData.DataPageButtonEnable();

                btnMain.PerformClick();

                UI.PageTenKey.DisableWork();
                UI.PageMotor.DisableWork();
                UI.PageIO.DisableWork();
                UI.SubpageRecipe.DisableWork();
                UI.SubpageDm.DisableWork();
                UI.SubpageSDm.DisableWork();
                UI.SubpageUseskip.DisableWork();
                UI.PageError.DisableWork();
                UI.SubpageBit.DisableWork();
            }
            else if (!bFlag && bFlagOld != bFlag) // Autorun Stop 시
            {
                UI.PageTenKey.EnableWork();
                UI.PageMotor.EnableWork();
                UI.PageIO.EnableWork();
                UI.SubpageRecipe.EnableWork();
                UI.SubpageDm.EnableWork();
                UI.SubpageSDm.EnableWork();
                UI.SubpageUseskip.EnableWork();
                UI.PageError.EnableWork();
                UI.SubpageBit.EnableWork();
            }
			else if (SM.SHM.IsBitData(BIT.MainDisplayShow) == true)
			{
				btnMain.PerformClick();
				SM.SHM.SetBitData(BIT.MainDisplayShow, false);
			}

            bFlagOld = bFlag;

            ////////////////////////////////////////////////////////////////////////////

            if (!bFlag)
            {
                if (UI.PageManual.IsWorking() || UI.PageMotor.IsWorking() || UI.PageIO.IsWorking() || UI.PageData.IsWorking() || UI.PageError.IsWorking())
                {                    
                    UI.PageData.DataPageButtonDisable();
                    UI.PageManual.ManualPageButtonDisable();

                    MainButtonDisable();
                }
                else
                {
                    if (UI.ModalLogin.IsLogIn == false)
                        DisableAllButton();
                    else
                    {                        
                        UI.PageData.DataPageButtonEnable();
                        UI.PageManual.ManualPageButtonEnable();

                        MainButtonEnable();
                        UI.PageSubItems.EnableSubButton();  //서브 버튼 활성화
                    }
                }
            }
        }
		private void lbSimMode_DoubleClick(object sender, EventArgs e)
		{
			if		(NDm.Read(eNDM.SimMotorSpeedRate) == 100)		NDm.Write(eNDM.SimMotorSpeedRate, 1);
			else if (NDm.Read(eNDM.SimMotorSpeedRate) < 10)			NDm.Write(eNDM.SimMotorSpeedRate, NDm.Read(eNDM.SimMotorSpeedRate)+1);
			else													NDm.Write(eNDM.SimMotorSpeedRate, NDm.Read(eNDM.SimMotorSpeedRate)+10);

			lbSimMode.Text	= string.Format("SIMULATION MODE - Motor Velocity X{0}", NDm.Read(eNDM.SimMotorSpeedRate));
		}
#endregion

#endregion

#region [ 메인 함수 ]      
		private bool InitialXML()
		{
			if (xmlMain.Initializ("Main") == false)                
			{
				return false;
			}
			Program.m_Languageidx = Convert.ToInt32(xmlMain.ReadDataTable(0, sColIDLang));
			return true;
		}
		
        private bool ReconfirmPassword()
        {
            //비밀번호 재확인
            if (Program.m_PasswordMode == PW_MODE.ADMIN)    
                return false;

            if (UI.ModalLevelSetting.ReadLevelXMLData((int)Program.m_PasswordMode, "RECONFIRM_PW"))
                return true;

            return false;
        }

        private void DisableAllButton()
        {
            //로그아웃일 때
            //메인으로 돌아가기
            btnMain.PerformClick();
            //모든 버튼 비활성화
            btnData.Enabled = false;
            btnManual.Enabled = false;
            btnMotor.Enabled = false;
            btnIO.Enabled = false;
            btnError.Enabled = false;
            btnLog.Enabled = false;
            ChangePWModeText();

            UI.PageSubItems.DisableSubButton();  //서브 버튼 비활성화
        }

        private void MainButtonEnable()
        {
            
            if (Program.m_PasswordMode == PW_MODE.ADMIN)
            {                                
                btnMng.Visible = true;
                btnMain.Enabled = true;
                btnData.Enabled = true;
                btnManual.Enabled = true;
                btnMotor.Enabled = true;
                btnIO.Enabled = true;
                btnError.Enabled = true;
                btnLog.Enabled = true;                
            }
            else
            {
                int nIdx = (int)Program.m_PasswordMode;
                btnMain.Enabled = UI.ModalLevelSetting.ReadLevelXMLData(nIdx, "MAIN");
                btnData.Enabled = UI.ModalLevelSetting.ReadLevelXMLData(nIdx, "DATA");
                btnManual.Enabled = UI.ModalLevelSetting.ReadLevelXMLData(nIdx, "MANUAL");
                btnMotor.Enabled = UI.ModalLevelSetting.ReadLevelXMLData(nIdx, "MOTOR");
                btnIO.Enabled = UI.ModalLevelSetting.ReadLevelXMLData(nIdx, "IO");
                btnError.Enabled = UI.ModalLevelSetting.ReadLevelXMLData(nIdx, "ERROR");
                btnLog.Enabled = UI.ModalLevelSetting.ReadLevelXMLData(nIdx, "LOG");
            }

            btnLogIn.Enabled = true;
            btnMng.Enabled = true;
        }

        private void MainButtonDisable()
        {
            /* //기존 코드
            if (!(btnMain.Enabled && btnMotor.Enabled && btnIO.Enabled && btnManual.Enabled
               && btnData.Enabled && btnLog.Enabled)) return;*/

            /* //테스트중 코드.....
            if (Program.m_PasswordMode == PW_MODE.ADMIN)
            {
                if (!(btnMain.Enabled && btnMotor.Enabled && btnIO.Enabled && btnManual.Enabled
                    && btnData.Enabled && btnLog.Enabled)) return;
            }
            else
            {
                int nIdx = (int)Program.m_PasswordMode;
                bool bMain = UI.ModalLevelSetting.ReadLevelXMLData(nIdx, "MAIN");
                bool bData = UI.ModalLevelSetting.ReadLevelXMLData(nIdx, "DATA");
                bool bManual = UI.ModalLevelSetting.ReadLevelXMLData(nIdx, "MANUAL");
                bool bMotor = UI.ModalLevelSetting.ReadLevelXMLData(nIdx, "MOTOR");
                bool bIO = UI.ModalLevelSetting.ReadLevelXMLData(nIdx, "IO");
                bool bError = UI.ModalLevelSetting.ReadLevelXMLData(nIdx, "ERROR");
                bool bLog = UI.ModalLevelSetting.ReadLevelXMLData(nIdx, "LOG");

                if (!(bMain && btnMain.Enabled) || !(bData && btnData.Enabled))
                    return;
            } */

            bool bAutoRun = UI.SubpageBit.IsBitData(BIT.AutoRun);
            if (!UI.PageMainDisp.Visible && !bAutoRun)
                btnMain.Enabled = false;
            if (!UI.PageManual.Visible && !bAutoRun)
                btnManual.Enabled = false;
            if (!UI.PageMotor.Visible)
                btnMotor.Enabled = false;
            if (!UI.PageIO.Visible && !bAutoRun)
                btnIO.Enabled = false;
            if (!UI.PageData.Visible && !bAutoRun)
                btnData.Enabled = false;
            if (!UI.PageLog.Visible && !bAutoRun)
                btnLog.Enabled = false;
            if (!UI.PageError.Visible && !bAutoRun)
                btnError.Enabled = false;
                       
            btnLogIn.Enabled = false;
            btnMng.Enabled = false;
        }

		private void ChangePWModeText()
		{
            if (UI.ModalLogin.IsLogIn == false)
			{
				lbID.Text = Language.languageDefault.ID;
				UI.strMemberID = string.Empty;
				btnLogIn.Image = Properties.Resources.LogOutState;    
				this.lbPwMode.Text = Language.languageDefault.NotUser;
				return;
			}
			else if (Program.m_PasswordMode == PW_MODE.ADMIN)		this.lbPwMode.Text = Language.languageDefault.Admin;
			else if (Program.m_PasswordMode == PW_MODE.OPER)		this.lbPwMode.Text = Language.languageDefault.Operator;
			else if (Program.m_PasswordMode == PW_MODE.ENG)			this.lbPwMode.Text = Language.languageDefault.Engineer;
			else if (Program.m_PasswordMode == PW_MODE.PRO)			this.lbPwMode.Text = Language.languageDefault.Professional;

			btnLogIn.Image = Properties.Resources.LogInState;
			lbID.Text = UI.strMemberID;

		}
        #endregion

        #region [ 메인 시계 ]

        #region - Private variables
        private Timer tmBCR      = new Timer();
        private Timer tmWatch    = new Timer();
#endregion

#region - Function
		
        private Timer tmTempResourceWatch    = new Timer();	
		private Process currentProcess;
		private TimeSpan prevTotalProcessorTime;
		private DateTime prevTime;

		private void initTimerWatch()
        {
            tmWatch.Enabled = true;
            tmWatch.Interval = 100;
            tmWatch.Tick += TmWatch_Tick;

            tmBCR.Enabled = true;
            tmBCR.Interval = 300;
            tmBCR.Tick += TmBCR_Tick;


			// Initialize the current process
			currentProcess = Process.GetCurrentProcess();// 초기 시간 설정
			prevTotalProcessorTime = currentProcess.TotalProcessorTime;
			prevTime = DateTime.Now;

			tmTempResourceWatch.Enabled = true;
            tmTempResourceWatch.Interval = 60000;
            tmTempResourceWatch.Tick += TmResource_Tick;

			StartStateTimeUpdate();
		}
		 private void TmResource_Tick(object sender, EventArgs e)
		{
			Process[] all = Process.GetProcessesByName("H_MarsSSD");
			foreach (Process thisProc in all.OrderBy(x => x.ProcessName))
			{
				GLog.Add("TEST ------------------------------------------------------------------");
				GLog.Add($"TEST - {"Nonpaged",-10} : {thisProc.NonpagedSystemMemorySize64 / (1024 * 1024),6} MB");//NonpagedSystemMemorySize64
				GLog.Add($"TEST - {"Paged",-10} : {thisProc.PagedMemorySize64 / (1024 * 1024),6} MB");		//PagedMemorySize64
				GLog.Add($"TEST - {"Virtual",-10} : {thisProc.VirtualMemorySize64 / (1024 * 1024),6} MB");	//VirtualMemorySize64
				GLog.Add($"TEST - {"WorkingSet",-10} : {thisProc.WorkingSet64 / (1024 * 1024),6} MB");	//WorkingSet64
				GLog.Add($"TEST - {"Memory",-10} : {thisProc.PrivateMemorySize64 / (1024 * 1024),6} MB");	//PrivateMemorySize64
			
				//PrivateMemorySize64: 프로세스가 사용하는 가상 메모리 중에서 다른 프로세스와 공유되지 않는 부분. 물리적 메모리뿐만 아니라 페이지 파일에 있을 수도 있음.
				//WorkingSet64: 물리적 메모리(RAM) 중에서 현재 프로세스가 직접 사용하고 있는 부분. 페이지 파일로 스왑되지 않은 메모리만 포함됨.

			}

			 //       string Name = thisProc.ProcessName;
             //   Console.WriteLine("Name : " + Name);
             //   Console.WriteLine("NonpagedSystemMemorySize64 : " + thisProc.NonpagedSystemMemorySize64);
             //   Console.WriteLine("PagedMemorySize64 : " + thisProc.PagedMemorySize64);
             //   Console.WriteLine("PagedSystemMemorySize64 : " + thisProc.PagedSystemMemorySize64);
             //   Console.WriteLine("PeakPagedMemorySize64 : " + thisProc.PeakPagedMemorySize64);
             //   Console.WriteLine("PeakVirtualMemorySize64 : " + thisProc.PeakVirtualMemorySize64);
             //   Console.WriteLine("PeakWorkingSet64 : " + thisProc.PeakWorkingSet64);
             //   Console.WriteLine("PrivateMemorySize64 : " + thisProc.PrivateMemorySize64);
             //   Console.WriteLine("PrivilegedProcessorTime : " + thisProc.PrivilegedProcessorTime);
             //   Console.WriteLine("ProcessorAffinity : " + thisProc.ProcessorAffinity);
             //   Console.WriteLine("TotalProcessorTime : " + thisProc.TotalProcessorTime);
             //   Console.WriteLine("UserProcessorTime : " + thisProc.UserProcessorTime);
             //   Console.WriteLine("VirtualMemorySize64 : " + thisProc.VirtualMemorySize64);
             //   Console.WriteLine("WorkingSet64 : " + thisProc.WorkingSet64); // 작업관리자에서 보이는 메모리 사용량
		}

		private void StartRunTime()
        {
            DateTime dtLoginTime = DateTime.Parse(UI.ModalLogin.LoginTime);
            TimeSpan ts = DateTime.Now - dtLoginTime;
            lbRunTime.Text = string.Format("{0}  {1:D2}:{2:D2}:{3:D2}", 
                Language.languageDefault.RunTime,
                (int)ts.TotalHours,
                ts.Minutes,
                ts.Seconds);

			int nAutoLogoutDelay = NDm.Read(eNDM.AutoLogoutDelay);
			if (nAutoLogoutDelay > 0 && 
				ts.TotalMilliseconds > nAutoLogoutDelay && 
				Program.m_PasswordMode != PW_MODE.ADMIN)
			{
				UI.ModalLogin.LogOut();
				btnMng.Visible = false;  
				ChangePWModeText();
			}
        }

        private void StopRunTime()
        {
            btnMng.Visible = false;            
            lbRunTime.Text = string.Format("{0}  00:00:00", Language.languageDefault.RunTime);            
        }

        #endregion

        #region - Event timer.
        private void TmWatch_Tick(object sender, EventArgs e)
        {
            lbx_CurrentTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (UI.ModalLogin.IsLogIn)
                StartRunTime();
        }
        private void TmBCR_Tick(object sender, EventArgs e)
        {
            if (SM.SHM.GetUseSkipData(USESKIP.BarcodeRead) == true)
			{
                if (G.BaseBarcode.m_DeviceCount < 6 && G.BaseBarcode.m_SerSystemDiscoverer != null)
                {
                    if (G.BaseBarcode.m_SerSystemDiscoverer.IsDiscoveryInProgress == false)
                        G.BaseBarcode.m_SerSystemDiscoverer.Discover();
                }

                if (G.BaseBarcode.m_DeviceCount == 6)
                {
                    if (SM.SHM.IsBitData(BIT.Loader1BarcodeConnect) == false)       G.LoadOESBarcode.ConnectChecking();
                    else if (SM.SHM.IsBitData(BIT.Loader2BarcodeConnect) == false)  G.LoadLPCAMMBarcode.ConnectChecking();
                    else if (SM.SHM.IsBitData(BIT.Unload1BarcodeConnect) == false)  G.UnloadOESBarcode.ConnectChecking();
                    else if (SM.SHM.IsBitData(BIT.Unload2BarcodeConnect) == false)  G.UnloadLPCAMMBarcode.ConnectChecking();
                    else if (SM.SHM.IsBitData(BIT.Reject1BarcodeConnect) == false)  G.RejectOESBarcode.ConnectChecking();
                    else if (SM.SHM.IsBitData(BIT.Reject2BarcodeConnect) == false)  G.RejectLPCAMMBarcode.ConnectChecking();
                }
            }
            if (SM.SHM.GetUseSkipData(USESKIP.SmartVisionLoader) == true)
			{
				if (SM.SHM.IsBitData(BIT.SmartVisionLoaderConnect) == false)			G.commFTVLoader.ConnectReTry();
			}
			if (SM.SHM.GetUseSkipData(USESKIP.SmartVisionTable) == true)
			{
				if (SM.SHM.IsBitData(BIT.SmartVisionTableConnect) == false) G.commFTVTable.ConnectReTry();
			}
		}

		
#endregion

#endregion

#region [ Language ]
		private void initializeLanguage()
		{   
			SetLanguageText();
		}

		private void SetLanguageText()
		{
			Language.languageDefault.Culture = LanguageManager.GetLanguageCulture(Program.m_Languageidx);
			LanguageManager.LanguageUpdate();

			btnMain.Text		= Language.languageDefault.Main;
			btnManual.Text		= Language.languageDefault.Manual;
			btnData.Text		= Language.languageDefault.Data;
			btnIO.Text			= Language.languageDefault.IO;
			btnError.Text		= Language.languageDefault.Error;
			btnLog.Text			= Language.languageDefault.Log;
			btnMotor.Text		= Language.languageDefault.Motor;
			btnLanguage.Text	= Language.languageDefault.MB_Language;
			
			ChangePWModeText();
		}
		#endregion
		
		
        private void btnMng_Click(object sender, EventArgs e)
        {
            UI.ModalManage.ShowDialog();
        }

		private void btnLogIn_Click(object sender, EventArgs e)
		{
			if (UI.ModalLogin.IsLogIn == true)
			{
				frmMessageBox msgDig = new frmMessageBox(true, "LOG OUT", "Do you want to LOG OUT?");
				if (msgDig.ShowDialog() == DialogResult.OK)
				{
					StopRunTime();
					UI.ModalLogin.LogOut();
				}
			}
			else
			{
				UI.ModalLogin.ShowDialog();
			}
			ChangePWModeText();
        }

	}
}
