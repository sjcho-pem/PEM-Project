using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace H_MarsSSD
{
	// SeqButton enum
	public enum BUTTON
	{
		// 0 ~ 9
		START           = 0,
		STOP,
		PAUSE,
		RESET,
		ALLHOME,
		BUZZEROFF,

		// 10 ~ 19
		MT_STEP         = 10,
		MT_JOG,
		MT_JOGSTOP,
		MT_INDEX,
		MT_SERVO,
		MT_HOME,
		MT_STOP,
		MT_REPEAT,
		MT_VELRATE,

		REJECTSEND		= 20,

		RECIPE_CHANGE,

		NotButton = 99,
	}

	public class SeqButton
	{
		public enum Cmd
		{
			START     = 100,
			CHECK,
            TIMER,
            TIMER1,
            TIMER2,
            ALARM,
			ALARM_DELAY,
			
			MOVE1      = 110,
			MOVE2,

			START1      = 120,
			START2,
			START3,
			START4,
			START5,
			START6,
			START6_RUNCHECK,
			START7,
			START7_RUNCHECK,
			START8,
			START9,
			START10,
			START11,
			RETRY_DELAY_TOP,
			RETRY_DELAY_BTM,
			START_SEND_AUTORUN,
			START_BTM_RUNSTARTCHECK,
			START_TOP_RUNSTARTCHECK,
			
			FTV_START_10,
			FTV_START_20,
			FTV_START_30,
			FTV_START_40,
			FTV_START_50,
			FTV_START_60,
			FTV_START_70,
			FTV_START_80,
			FTV_START_90,
			FTV_START_100,


			END       = 199,
			END_NG,
		}

		private SMM<Cmd> m_smmCmd	= new SMM<Cmd>(Cmd.END,		"BTN CMD PROCESS");
		private SMM<Cmd> m_smmStart	= new SMM<Cmd>(Cmd.START,	"BTN START PROCESS");
		private SMM<Cmd> m_smmStop	= new SMM<Cmd>(Cmd.START,	"BTN STOP PROCESS");
		private SMM<Cmd> m_smmPause	= new SMM<Cmd>(Cmd.START,	"BTN PAUSE PROCESS");
		private SMM<Cmd> m_smmReset	= new SMM<Cmd>(Cmd.START,	"BTN RESET PROCESS");
		private SMM<Cmd> m_smmRepeat = new SMM<Cmd>(Cmd.START,	"REPEAT MOVE PROCESS");
		
		public	Cmd smmStart   { get => m_smmStart.CurStep; }
		
		private PStopwatch m_twDelay;
		private PStopwatch m_twReset;
		private bool bRunning = false;
		private int  m_CurRetryCount = 0;
		private int  m_RetryCount =30;
		CommInsp.RunState	m_topRunState = CommInsp.RunState.Unknown;
		CommInsp.RunState	m_btmRunState = CommInsp.RunState.Unknown;

		/// <summary>
		/// Repeat Move 사용
		/// </summary>
		private MtInfo curMtinfo = new MtInfo();
		private int nPreIndex = 99;
        private bool m_IsConnectLoading = false;

		public void Init()
		{
			try
			{
				btnCmd();
				curMtinfo.Init();

				m_twDelay = new PStopwatch();
				m_twDelay.Stop();        
				m_twReset = new PStopwatch();
				m_twReset.Stop();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}         
		}

		public	void btnStart()		{ m_smmStart.eInput = InState.run; }
		public	void btnStop()		{ m_smmStop.eInput = InState.run; }
		public	void btnPause()		{ m_smmPause.eInput = InState.run; }
		public	void btnReset()		{ m_smmReset.eInput = InState.run; }
		public	void btnCmd()		{ m_smmCmd.eInput = InState.run; }

        public bool IsStartReady() { return m_smmStart.IsReady; }
        public bool IsResetReady() { return m_smmReset.IsReady; }

        public	bool IsStopReady()	{ return m_smmStop.IsReady; }

		private void RepeatMove()	{ m_smmRepeat.eInput = InState.run; }
		private void RepeatStop()	{ m_smmRepeat.Next(Cmd.END); }

		/// <summary>
		/// Button Run Process 
		/// </summary>
		public void Run()
		{
			try
			{
				Start(m_smmStart);
				Stop(m_smmStop);
				Pause(m_smmPause);
				Reset(m_smmReset);
				Command(m_smmCmd);

				Repeat(m_smmRepeat);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		/// <summary>
		/// Button Command Process
		/// </summary>
		private void Command(SMM<Cmd> smm)
		{
			try
			{
				BUTTON eNo = SM.SHM.GetButtonNum();

				switch (smm.CurStep)
				{
					case Cmd.START:
						{
							if (smm.eInput == InState.run)
							{
								smm.eInput = InState.none;
								smm.Next(Cmd.TIMER);
							}
						}
						break;
					case Cmd.TIMER:
						{
							bool bEmo			= SM.SHM.IsBitData(BIT.EMO);
							if (bEmo == true)
							{
								SM.SHM.ClearButtonNum();

								if (eNo == BUTTON.RESET)
								{
									bRunning = true;
									GLog.Add(string.Format("Button No : {0}({1})", eNo.ToString(), (int)eNo));
								}
								break;
                            }
                            
                            if (eNo == BUTTON.STOP) btnStop();

                            if (bRunning == true)
								break;

							if (eNo != BUTTON.NotButton)
							{
								bRunning = true;
								GLog.Add(string.Format("Button No : {0}({1})", eNo.ToString(), (int)eNo));

								SM.SHM.ClearButtonNum();
							}                        
						}
						break;
					case Cmd.END:
						{
							smm.Next(Cmd.START);
						}
						break;
				}

				if (bRunning == false)			return;

				switch (eNo)
				{
					case BUTTON.START:
						btnStart();
						break;
					case BUTTON.STOP:
						G.commInspBtm.SendPauseStart(2);
						G.commInspTop.SendPauseStart(2);
						btnStop();
						break;
					case BUTTON.PAUSE:
						//반복적으로 보내지 않도록 수정
						if (SM.SHM.IsBitData(BIT.Pause) == false)
						{
							G.commInspBtm.SendPauseStart(2);
							G.commInspTop.SendPauseStart(2);
							btnPause();
						}
						break;
					case BUTTON.RESET:
						GLog.Add("btnReset");
						btnReset();
						break;
					case BUTTON.ALLHOME:
						G.allHome.Start((bool)SM.SHM.GetButtonParams()[0]);
						break;
					case BUTTON.MT_VELRATE:
						{
							MtInfo curMt = SM.SHM.GetCurMT();
							G.cKernel.MotorVelocityPercentUpdate((VelocityRate)curMt.nVel);                        
						}
						break;
					case BUTTON.REJECTSEND:
							G.commInspRef.SendRejectDataReport((string)SM.SHM.GetManualParam(2) ,(int)SM.SHM.GetManualParam(0),(int)SM.SHM.GetManualParam(1),SM.SHM.GetRecipeDataToInt(DEVICE.TrayRowCnt),SM.SHM.GetRecipeDataToInt(DEVICE.TrayColCnt) );
						break;
					case BUTTON.NotButton:
						break;
					default:
						break;	
				}

				if (SM.SHM.GetUseSkipData(USESKIP.Safety) == true  &&  SM.SHM.IsBitData(BIT.KeyLockAuto) == true)
				{
					if (eNo >= BUTTON.MT_STEP  && eNo <= BUTTON.MT_REPEAT)
					{
						SM.SHM.MsgShow(SeqMsg.KeyLockErr);
					}

					bRunning = false;
					return;
				}

				switch (eNo)
				{
					case BUTTON.MT_STEP:
						{
							MtInfo curMt = SM.SHM.GetCurMT();
							G.cKernel.Motor(curMt.eMt).MoveREL(curMt.nDist, curMt.nVel);
						}
						break;
					case BUTTON.MT_JOG:
						{
							MtInfo curMt = SM.SHM.GetCurMT();
							if (curMt.eMt == MT.LDConv1 ||curMt.eMt == MT.LDConv2 )
							{
								G.cKernel.Motor(MT.LDConv1).MoveJog(curMt.bFlag, curMt.nVel);
								G.cKernel.Motor(MT.LDConv2).MoveJog(curMt.bFlag, curMt.nVel);
							}
							else if (curMt.eMt == MT.ULDConv1 ||curMt.eMt == MT.ULDConv2 )
							{
								G.cKernel.Motor(MT.ULDConv1).MoveJog(curMt.bFlag, curMt.nVel);
								G.cKernel.Motor(MT.ULDConv2).MoveJog(curMt.bFlag, curMt.nVel);
							}
							else
								G.cKernel.Motor(curMt.eMt).MoveJog(curMt.bFlag, curMt.nVel);

						}
						break;
					case BUTTON.MT_JOGSTOP:
						{
							MtInfo curMt = SM.SHM.GetCurMT();
							if (curMt.eMt == MT.LDConv1 ||curMt.eMt == MT.LDConv2 )
							{
								G.cKernel.Motor(MT.LDConv1).Stop();
								G.cKernel.Motor(MT.LDConv2).Stop();
							}
							else if (curMt.eMt == MT.ULDConv1 ||curMt.eMt == MT.ULDConv2 )
							{
								G.cKernel.Motor(MT.ULDConv1).Stop();
								G.cKernel.Motor(MT.ULDConv2).Stop();
							}
							else
								G.cKernel.Motor(curMt.eMt).Stop();
						}
						break;
					case BUTTON.MT_INDEX:
						{
							MtInfo curMt = SM.SHM.GetCurMT();
							G.cKernel.Motor(curMt.eMt).MoveIndex(curMt.nIdx);
						}
						break;
					case BUTTON.MT_SERVO:
						{
							MtInfo curMt = SM.SHM.GetCurMT();
							G.cKernel.Motor(curMt.eMt).SetServoOn(curMt.bFlag);
						}
						break;
					case BUTTON.MT_HOME:
						{
							MtInfo curMt = SM.SHM.GetCurMT();
							G.cKernel.Motor(curMt.eMt).StartHoming();
						}
						break;
					case BUTTON.MT_STOP:
						{
							MtInfo curMt = SM.SHM.GetCurMT();
							G.cKernel.Motor(curMt.eMt).Stop();

							RepeatStop();
						}
						break;
					case BUTTON.MT_REPEAT:
						{
							curMtinfo = SM.SHM.GetCurMT();
							RepeatMove();
						}
						break;
					case BUTTON.NotButton:
						break;
					default:
						break;
				}

				bRunning = false;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		/// <summary>
		/// Start Button Process
		/// </summary>
		private void Start(SMM<Cmd> smm)
		{
			try
			{
				switch (smm.CurStep)
				{
					case Cmd.START:
						{
							if (smm.eInput == InState.run)
							{
								smm.eInput = InState.none;
								smm.Next(Cmd.START1);
							}
						}
						break;

					case Cmd.START1:
						{
							m_CurRetryCount = 0;
							UI.LoadingFormOpen("STARTING");

							if (G.cKernel.IsAOut(DO.BtmInspReady1) || G.cKernel.IsAOut(DO.TopInspReady1) ||
								G.cKernel.IsAOut(DO.BtmInspReady2) || G.cKernel.IsAOut(DO.TopInspReady2))
							{
								if (SM.SHM.GetUsePart(UsePartName.Insp1) != UsePartMode.Disable)			G.cKernel.AOut(DO.TopInspReady1, false);
								if (SM.SHM.GetUsePart(UsePartName.Insp2) != UsePartMode.Disable)			G.cKernel.AOut(DO.TopInspReady2, false);
								if (SM.SHM.GetUsePart(UsePartName.Insp1) != UsePartMode.Disable)			G.cKernel.AOut(DO.BtmInspReady1, false);
								if (SM.SHM.GetUsePart(UsePartName.Insp2) != UsePartMode.Disable)			G.cKernel.AOut(DO.BtmInspReady2, false);
							}

							SM.SHM.XmlModelUpdate();

							smm.Next(Cmd.FTV_START_10);
						}
						break;


						
				case Cmd.FTV_START_10:
					{
						if (SM.SHM.GetUseSkipData(USESKIP.SmartVisionLoader) == false)
						{
							smm.Next(Cmd.FTV_START_60);
							break;
						}

						G.commFTVLoader.Send(CommFTV.IV4Command.ReadOperatingStatus);

						smm.Next(Cmd.FTV_START_20);
					}
					break;

				case Cmd.FTV_START_20:
					{
						string readRcvData = "";
						int ret = G.commFTVLoader.RcvRead(CommFTV.IV4Command.ReadOperatingStatus, ref readRcvData);

						if (ret != 1)
						{
							if (smm.Delayms(5000) == false) break;

							G.AlmMng.ADD(ALM.ERROR_MAIN_SEQ_FTV_INSP_FTV_STATE_CHECK_TIME_OUT);// 운전 준비상태 타임 아웃
							smm.Next(Cmd.END_NG);
							break;
						}

						if (readRcvData == "0") // 0 - 설정중 대기한다.
						{
							G.AlmMng.ADD(ALM.ERROR_MAIN_SEQ_FTV_INSP_FTV_STATE_CHECK);// 운전 준비상태 확인.
							smm.Next(Cmd.END_NG);
							break;
						}

						smm.Next(Cmd.FTV_START_30);
					}
					break;

				case Cmd.FTV_START_30:
					{
						G.commFTVLoader.Send(CommFTV.IV4Command.GetProgramNumber);

						smm.Next(Cmd.FTV_START_40);
					}
					break;

				case Cmd.FTV_START_40:
					{
						string readRcvNumber = "";
						int ret = G.commFTVLoader.RcvGetProgramNumber(ref readRcvNumber);
						string oriRcvNumber = (SM.SHM.GetRecipeDataToInt(DEVICE.FTVJob) / 1000).ToString("000");

						if (ret != 1)
						{
							if (smm.Delayms(3000) == false) break;
							
							G.AlmMng.ADD(ALM.ERROR_MAIN_SEQ_FTV_INSP_CONNECT_CHECK);
							smm.Next(Cmd.END_NG);
							break;
						}

						if (readRcvNumber != oriRcvNumber) // FTV 레시피 체크 확인 ##
						{
							G.commFTVLoader.Send(CommFTV.IV4Command.SetProgramNumber, oriRcvNumber);
							smm.Next(Cmd.FTV_START_50);
							break;
						}
						
						m_CurRetryCount = 0;
						smm.Next(Cmd.FTV_START_60);
					}
					break;

				case Cmd.FTV_START_50:
					{
						if (smm.Delayms(5000) == false) break;

						m_CurRetryCount ++ ;
						if (m_CurRetryCount >= 3 )
						{
							G.AlmMng.ADD(ALM.ERROR_MAIN_SEQ_FTV_INSP_RECIPE_CHECK);// FTV Insp 레시피 체크 알람
							
							smm.Next(Cmd.END_NG);
							break;
						}

						smm.Next(Cmd.FTV_START_30);
					}
					break;
						
				case Cmd.FTV_START_60:
					{
						if (SM.SHM.GetUseSkipData(USESKIP.SmartVisionTable) == false)
						{
							smm.Next(Cmd.START2);
							break;
						}

						G.commFTVTable.Send(CommFTV.IV4Command.ReadOperatingStatus);

						smm.Next(Cmd.FTV_START_70);
					}
					break;

				case Cmd.FTV_START_70:
					{
						string readRcvData = "";
						int ret = G.commFTVTable.RcvRead(CommFTV.IV4Command.ReadOperatingStatus, ref readRcvData);

						if (ret != 1)
						{
							if (smm.Delayms(5000) == false) break;

							G.AlmMng.ADD(ALM.ERROR_MAIN_SEQ_FTV_INSP_FTV_STATE_CHECK_TIME_OUT);// 운전 준비상태 타임 아웃
							smm.Next(Cmd.END_NG);
							break;
						}

						if (readRcvData == "0") // 0 - 설정중 대기한다.
						{
							G.AlmMng.ADD(ALM.ERROR_MAIN_SEQ_FTV_INSP_FTV_STATE_CHECK);// 운전 준비상태 확인.
							smm.Next(Cmd.END_NG);
							break;
						}

						smm.Next(Cmd.FTV_START_80);
					}
					break;

				case Cmd.FTV_START_80:
					{
						G.commFTVTable.Send(CommFTV.IV4Command.GetProgramNumber);

						smm.Next(Cmd.FTV_START_90);
					}
					break;

				case Cmd.FTV_START_90:
					{
						string readRcvNumber = "";
						int ret = G.commFTVTable.RcvGetProgramNumber(ref readRcvNumber);
						string oriRcvNumber = (SM.SHM.GetRecipeDataToInt(DEVICE.FTVJob) / 1000).ToString("000");

						if (ret != 1)
						{
							if (smm.Delayms(3000) == false) break;
							
							G.AlmMng.ADD(ALM.ERROR_MAIN_SEQ_FTV_INSP_CONNECT_CHECK);
							smm.Next(Cmd.END_NG);
							break;
						}

						if (readRcvNumber != oriRcvNumber) // FTV 레시피 체크 확인 ##
						{
							G.commFTVTable.Send(CommFTV.IV4Command.SetProgramNumber, oriRcvNumber);
							smm.Next(Cmd.FTV_START_80);
							break;
						}
						
						m_CurRetryCount = 0;
						smm.Next(Cmd.START2);
					}
					break;

				case Cmd.FTV_START_100:
					{
						if (smm.Delayms(5000) == false) break;

						m_CurRetryCount ++ ;
						if (m_CurRetryCount >= 3 )
						{
							G.AlmMng.ADD(ALM.ERROR_MAIN_SEQ_FTV_INSP_RECIPE_CHECK);// FTV Insp 레시피 체크 알람
							
							smm.Next(Cmd.END_NG);
							break;
						}

						smm.Next(Cmd.FTV_START_80);
					}
					break;

						
					#region  검사상태 확인 , Cycle Stop 진행중일 수 있기 때문에
					case Cmd.START2:
						{
							if (SM.SHM.GetUseSkipData(USESKIP.TopInspection) == true)
								G.commInspTop.SendInspStateRequest();
							if (SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == true)
								G.commInspBtm.SendInspStateRequest();

							smm.Next(Cmd.START3);
						}
						break;
					case Cmd.START3:
						{
							if (SM.SHM.GetUseSkipData(USESKIP.TopInspection) == false)
							{
								smm.Next(Cmd.START4);
								break;
							}

							CommInsp.InspState state = CommInsp.InspState.Idle;
							int nret = G.commInspTop.RcvInspStateReport( ref state);
							if (nret < 0)
							{
								smm.Next(Cmd.END_NG);
							}
							else if (nret > 0)
							{
								if (state == CommInsp.InspState.Idle)
								{
									smm.Next(Cmd.START4);
								}
								else
								{
									string strMsg = "The top inspection is still in progress." + Environment.NewLine +
													"Please restart after the inspection has been completed.";
									SM.SHM.SetMsg(SeqMsg.SEQINFO, strMsg);
									SM.SHM.MsgShow(SeqMsg.SEQINFO);

									smm.Next(Cmd.END_NG);
								}
							}
							else if (smm.Delayms(NDm.Read(eNDM.InspCommTimeOut)) == true)
							{
								G.AlmMng.ADD(ALM.ERROR_SEQ_INSP_TOP_PC_COMM_TIMEOUT);
								smm.Next(Cmd.END_NG);
							}
						}
						break;
					case Cmd.RETRY_DELAY_TOP:
						{
							if (smm.Delayms(500) == false)			break;

							smm.Next(Cmd.START2);
						}
						break;
					case Cmd.START4:
						{
							if (SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == false)
							{
								smm.Next(Cmd.START5);
								break;
							}
							CommInsp.InspState state = CommInsp.InspState.Idle;
							int nret = G.commInspBtm.RcvInspStateReport(ref state);
							if (nret < 0)
							{
								smm.Next(Cmd.END_NG);
							}
							else if (nret > 0)
							{
								if (state == CommInsp.InspState.Idle)
								{
									smm.Next(Cmd.START5);
								}
								else
								{
									string strMsg = "The Bottom inspection is still in progress." + Environment.NewLine +
													"Please restart after the inspection has been completed.";
									SM.SHM.SetMsg(SeqMsg.SEQINFO, strMsg);
									SM.SHM.MsgShow(SeqMsg.SEQINFO);
									smm.Next(Cmd.END_NG);
								}
							}
							else if (smm.Delayms(NDm.Read(eNDM.InspCommTimeOut)) == true)
							{
								G.AlmMng.ADD(ALM.ERROR_SEQ_INSP_BTM_PC_COMM_TIMEOUT);
								smm.Next(Cmd.END_NG);
							}
						}
						break;
					case Cmd.RETRY_DELAY_BTM:
						{
							if (smm.Delayms(500) == false) break;

							smm.Next(Cmd.START2);
						}
						break;
					#endregion

					#region 검사 Run 상태 체크, Run이 아닐때는 AutoRunStart 통신 후, 다시 Run 상태 체크하여 Run 확인
					case Cmd.START5:
						{
							if (smm.Delayms(500) == false)
								break;

							if (SM.SHM.GetUseSkipData(USESKIP.TopInspection) == true)
								G.commInspTop.SendRunStateRequest();
							if (SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == true)
								G.commInspBtm.SendRunStateRequest();
                            
							smm.Next(Cmd.START6);
						}
						break;
					case Cmd.START6:
						{
							if (SM.SHM.GetUseSkipData(USESKIP.TopInspection) == false)
							{
								smm.Next(Cmd.START7);
								break;
							}

							CommInsp.RunState state = CommInsp.RunState.Idle;
							int nret = G.commInspTop.RcvRunStateReport( ref state);
							if (nret < 0)
							{
								smm.Next(Cmd.END_NG);
							}
							else if (nret > 0)
							{
								if (state == CommInsp.RunState.Run)
								{
									m_topRunState = CommInsp.RunState.Run; 
									smm.Next(Cmd.START7);
								}
								else
								{
									m_topRunState = CommInsp.RunState.Unknown; 
									smm.Next(Cmd.START7);
								}
							}
							else if (smm.Delayms(NDm.Read(eNDM.InspCommTimeOut)) == true)
							{
								G.AlmMng.ADD(ALM.ERROR_SEQ_INSP_TOP_PC_COMM_TIMEOUT);
								smm.Next(Cmd.END_NG);
							}
						}
						break;
					
					case Cmd.START7:
						{
							if (SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == false)
							{
								smm.Next(Cmd.START_SEND_AUTORUN);
								break;
							}

							CommInsp.RunState state = CommInsp.RunState.Idle;
							int nret = G.commInspBtm.RcvRunStateReport(ref state);
							if (nret < 0)
							{
								smm.Next(Cmd.END_NG);
							}
							else if (nret > 0)
							{
								if (state == CommInsp.RunState.Run)
								{
									m_btmRunState = CommInsp.RunState.Run; 
									smm.Next(Cmd.START_SEND_AUTORUN);
								}
								else
								{
									m_btmRunState = CommInsp.RunState.Unknown; 
									smm.Next(Cmd.START_SEND_AUTORUN);
								}
							}
							else if (smm.Delayms(NDm.Read(eNDM.InspCommTimeOut)) == true)
							{
								G.AlmMng.ADD(ALM.ERROR_SEQ_INSP_BTM_PC_COMM_TIMEOUT);
								smm.Next(Cmd.END_NG);
							}
						}
						break;
						
					case Cmd.START_SEND_AUTORUN:
						{
							if (SM.SHM.GetUseSkipData(USESKIP.TopInspection) == true &&
								m_topRunState != CommInsp.RunState.Run)
							{
								G.commInspTop.SendAutoRunStart();
							}
							if (SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == true &&
								m_btmRunState != CommInsp.RunState.Run)
							{
								G.commInspBtm.SendAutoRunStart();
							}
							
							smm.Next(Cmd.START_TOP_RUNSTARTCHECK);
						}
						break;
					case Cmd.START_TOP_RUNSTARTCHECK:
						{	
							if (SM.SHM.GetUseSkipData(USESKIP.TopInspection) == false ||
								m_topRunState == CommInsp.RunState.Run)
							{
								smm.Next(Cmd.START_BTM_RUNSTARTCHECK);
								break;
							}

							bool bOK = false;
							int nret = G.commInspTop.RcvAutoRunStartConfirm(ref bOK);

							if (nret < 0)
							{
								smm.Next(Cmd.END_NG);
							}
							else if (nret > 0)
							{
								if (bOK == true)
								{
									smm.Next(Cmd.START_BTM_RUNSTARTCHECK);
								}
								else
								{
									G.AlmMng.ADD(ALM.ERROR_SEQ_INSP_TOP_PC_COMM_TIMEOUT);
									smm.Next(Cmd.END_NG);
								}
							}
							else if (smm.Delayms(NDm.Read(eNDM.InspCommTimeOut)) == true)
							{
								G.AlmMng.ADD(ALM.ERROR_SEQ_INSP_BTM_PC_COMM_TIMEOUT);
								smm.Next(Cmd.END_NG);
							}
						}
						break;
					case Cmd.START_BTM_RUNSTARTCHECK:
						{
							if (SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == false ||
								m_btmRunState == CommInsp.RunState.Run)
							{
								smm.Next(Cmd.START8);
								break;
							}

							bool bOK = false;
							int nret = G.commInspBtm.RcvAutoRunStartConfirm(ref bOK);

							if (nret < 0)
							{
								smm.Next(Cmd.END_NG);
							}
							else if (nret > 0)
							{
								if (bOK == true)
								{
									smm.Next(Cmd.START8);
								}
								else
								{
									G.AlmMng.ADD(ALM.ERROR_SEQ_INSP_BTM_PC_COMM_TIMEOUT);
									smm.Next(Cmd.END_NG);
								}
							}
							else if (smm.Delayms(NDm.Read(eNDM.InspCommTimeOut)) == true)
							{
								G.AlmMng.ADD(ALM.ERROR_SEQ_INSP_BTM_PC_COMM_TIMEOUT);
								smm.Next(Cmd.END_NG);
							}
						}
						break;
					case Cmd.START8:
						{
							if (smm.Delayms(1000) == false)
								break;

							m_CurRetryCount++;

							if (SM.SHM.GetUseSkipData(USESKIP.TopInspection) == true)
							{
								G.commInspTop.SendRunStateRequest();
							}
							if (SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == true)
							{
								G.commInspBtm.SendRunStateRequest();
							}
							smm.Next(Cmd.START9);
						}
						break;
					case Cmd.START9:
						{
							if (SM.SHM.GetUseSkipData(USESKIP.TopInspection) == false)
							{
								smm.Next(Cmd.START10);
								break;
							}

							CommInsp.RunState state = CommInsp.RunState.Idle;
							int nret = G.commInspTop.RcvRunStateReport(ref state);
							if (nret < 0)
							{
								smm.Next(Cmd.END_NG);
							}
							else if (nret > 0)
							{
								if (state == CommInsp.RunState.Run)
								{
									smm.Next(Cmd.START10);
								}
								else
								{
									if (m_CurRetryCount > m_RetryCount)
									{
										G.AlmMng.ADD(ALM.ERROR_SEQ_INSP_TOP_PC_COMM_TIMEOUT);
										smm.Next(Cmd.END_NG);
										break;
									}
									smm.Next(Cmd.START8); //재시도
								}
							}
							else if (smm.Delayms(NDm.Read(eNDM.InspCommTimeOut)) == true)
							{
								G.AlmMng.ADD(ALM.ERROR_SEQ_INSP_TOP_PC_COMM_TIMEOUT);
								smm.Next(Cmd.END_NG);
							}
						}
						break;
					case Cmd.START10:
						{
							if (SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == false)
							{
								smm.Next(Cmd.START11);
								break;
							}

							CommInsp.RunState state = CommInsp.RunState.Idle;
							int nret = G.commInspBtm.RcvRunStateReport(ref state);
							if (nret < 0)
							{
								smm.Next(Cmd.END_NG);
							}
							else if (nret > 0)
							{
								if (state == CommInsp.RunState.Run)
								{
									smm.Next(Cmd.START11);
								}
								else
								{
									if (m_CurRetryCount > m_RetryCount)
									{
										G.AlmMng.ADD(ALM.ERROR_SEQ_INSP_BTM_PC_COMM_TIMEOUT);
										smm.Next(Cmd.END_NG);
										break;
									}
									smm.Next(Cmd.START8);
								}
							}
							else if (smm.Delayms(NDm.Read(eNDM.InspCommTimeOut)) == true)
							{
								G.AlmMng.ADD(ALM.ERROR_SEQ_INSP_BTM_PC_COMM_TIMEOUT);
								smm.Next(Cmd.END_NG);
							}
						}
						break;
					#endregion


					case Cmd.START11:
						{
							G.seqMain.AutoRunStart();
						}
						smm.Next(Cmd.END);
						break;

					case Cmd.END:
						UI.LoadingFormClose();
						SM.SHM.SetBitData(BIT.AutoRun, true);

						smm.Next(Cmd.START);
						break;

					case Cmd.END_NG:
						UI.LoadingFormClose();

						smm.Next(Cmd.START);
						break;

					default:
						smm.Next(Cmd.START);
						break;
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		/// <summary>
		/// Stop Button Process
		/// </summary>
		private void Stop(SMM<Cmd> smm)
		{
			try
			{
				switch (smm.CurStep)
				{
					case Cmd.START:
						{
							if (smm.eInput == InState.run)
							{
								smm.eInput = InState.none;
								smm.Next(Cmd.TIMER);
							}
						}
						break;

					case Cmd.TIMER:
						{
							if (smm.Delayms(100) == false)			break;

							G.allHome.Stop();
							G.seqMain.AutoRunStop();
							m_smmStart.ResetStep();
							
							SM.SHM.SetBitData(BIT.AllHome, false);
							SM.SHM.SetBitData(BIT.UIAllHomeStopping, true);
							SM.SHM.SetBitData(BIT.AutoRun, false);
							SM.SHM.SetBitData(BIT.Pause, false);

							smm.Next(Cmd.END);
						}
						break;

					case Cmd.END:

						smm.Next(Cmd.START);
						break;

					default:
						smm.Next(Cmd.START);
						break;
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		/// <summary>
		/// Pause Button Process
		/// </summary>
		/// <param name="smm"></param>
		private void Pause(SMM<Cmd> smm)
		{
			try
			{
				switch (smm.CurStep)
				{
					case Cmd.START:
						{
							if (smm.eInput == InState.run)
							{
								smm.eInput = InState.none;
								smm.Next(Cmd.TIMER);
							}
						}
						break;

					case Cmd.TIMER:
						{
							if (!smm.Delayms(100))
								break;

							SM.SHM.SetBitData(BIT.Pause, true);

							smm.Next(Cmd.END);
						}
						break;

					case Cmd.END:

						smm.Next(Cmd.START);
						break;
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		/// <summary>
		/// Reset Button Process
		/// </summary>
		private void Reset(SMM<Cmd> smm)
		{
			try
			{
				switch (smm.CurStep)
				{
					case Cmd.START:
						{

							if (smm.eInput == InState.run)
							{
								smm.eInput = InState.none;
								smm.Next(Cmd.CHECK);
							}
						}
						break;
					case Cmd.CHECK:
						{
							UI.LoadingFormOpen("ALARM RESET");
							if (G.AlmMng.Find(ALM.ERROR_CONTROL_DEVICE_DISCONECTED) == true ||
								G.cKernel.IsOnLineAllSlave() == false)
							{
								smm.Next(Cmd.TIMER1);
							}
							else
							{
								smm.Next(Cmd.ALARM);
							}

							m_twReset.Restart();
						}
						break;
                    case Cmd.TIMER1:
                        {
                            if (G.cKernel.IsOnLineAllSlave() == false)
                            {
                                if (m_IsConnectLoading == false)
                                {
                                    G.cKernel.ReStartEngine();
                                    m_IsConnectLoading = true;
                                }

                                if (smm.Delayms(15000) == false)
                                    break;
                            }
                            m_IsConnectLoading = false;
                            smm.Next(Cmd.TIMER2);
                        }
                        break;
                    case Cmd.TIMER2:
                        {
                            if (!smm.Delayms(100))
                                break;

                            if (G.AlmMng.Find(ALM.ERROR_CONTROL_DEVICE_DISCONECTED))
                            {
                                G.cKernel.ReStartDevice();
                            }


                            smm.Next(Cmd.ALARM);
                        }
                        break;
                    case Cmd.ALARM:
						{
							if (!smm.Delayms(2000))
								break;

							for (MT eAxis = (MT)0; eAxis < MT.TOTAL; eAxis++)
							{
								ALM err = (ALM)((int)ALM.ERROR_MT_DRIVER_START + (int)eAxis + 1);
								if (G.AlmMng.Find(err))
								{
									G.cKernel.Motor(eAxis).AlarmReset();
									continue;
								}
								err = (ALM)((int)ALM.ERROR_MT_POWER_START + (int)eAxis + 1);
								if (G.AlmMng.Find(err))
								{
									G.cKernel.Motor(eAxis).SetServoOn(true);
								}
							}
							
							G.seqInspPlate1.AlarmReset();
							G.seqInspPlate2.AlarmReset();
							G.seqLoadBuffer1.AlarmReset();
							G.seqLoadBuffer2.AlarmReset();
							G.seqLoadInspPnP.AlarmReset();
							G.seqTrayLoader.AlarmReset();
							G.seqLoadPnP.AlarmReset();
							G.seqLoadTransfer.AlarmReset();
							G.seqLoadTrayTable1.AlarmReset();
							G.seqLoadTrayTable2.AlarmReset();
							G.seqRejectBuffer.AlarmReset();
							G.seqTrayClean.AlarmReset();
							G.seqUnloadBuffer.AlarmReset();
							G.seqUnloadInspPnP.AlarmReset();
							G.seqTrayUnloader.AlarmReset();
							G.seqUnloadPnP.AlarmReset();
							G.seqUnloadTransfer.AlarmReset();
							G.seqUnloadTrayTable1.AlarmReset();
							G.seqUnloadTrayTable2.AlarmReset();

							if (m_twReset.IsTimeOverMs(20000) == true)
							{
								G.AlmMng.ADD(ALM.ERROR_ALARMCLEAR_TIMEOUT);
								m_twReset.Stop();
								smm.Next(Cmd.END);
								break;
							}
						
							if (m_twReset.IsTimeOverMs(10000) == true && G.cKernel.IsBIn(DI.RESET) == true)
							{
								G.AlmMng.ADD(ALM.ERROR_RESET);
								m_twReset.Stop();
								smm.Next(Cmd.END);
								break;
							}

							G.AlmMng.Clear();
							G.sysErr.Start();
							SM.SHM.SetBitData(BIT.Buzzer, false);
							SM.SHM.SetBitData(BIT.WarningBuzzer, false);
							SM.SHM.SetBitData(BIT.RejectBuzzer, false);

							smm.Next(Cmd.ALARM_DELAY);
						}
						break;
					case Cmd.ALARM_DELAY:
						{
							if (smm.Delayms(500) == false)
								break;

							if  (IsAlarm() == true)	smm.Next(Cmd.ALARM);
							else					smm.Next(Cmd.END);
						}
						break;
					case Cmd.END:
						{
							m_IsConnectLoading = false;
							if (NDm.Read(eNDM.RecentlyAlarmCode) != 0)
							{
								G.commUbiGEM.SendAlarmClear((ALM)NDm.Read(eNDM.RecentlyAlarmCode));
								NDm.Write(eNDM.RecentlyAlarmCode, 0);
							}
							UI.LoadingFormClose();
							smm.Next(Cmd.START);
						}
						break;

					default:
						smm.Next(Cmd.START);
						break;
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		/// <summary>
		/// Repeat Move Process
		/// </summary>
		/// <param name="smm"></param>
		private void    Repeat(SMM<Cmd> smm)
		{
			try
			{
				switch (smm.CurStep)
				{
					case Cmd.START:
						{
							if (smm.eInput == InState.run)
							{
								smm.eInput = InState.none;

								nPreIndex = G.cKernel.Motor(curMtinfo.eMt).GetNowIndex();
								
								if (nPreIndex.Equals(curMtinfo.nIdx) || nPreIndex == 300 || 
									G.cKernel.Motor(curMtinfo.eMt).IsHomeDone() == false)
									smm.Next(Cmd.END);
								else
									smm.Next(Cmd.MOVE1);
							}
						}
						break;
					case Cmd.MOVE1:
						{
							if (G.cKernel.Motor(curMtinfo.eMt).IsHomeDone() == false || nPreIndex == 300)
							{
								frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Check Motor Home State");

								smm.Next(Cmd.END);
								break;
							}

							if (G.cKernel.Motor(curMtinfo.eMt).IsRdy(nPreIndex) == true)
							{
								if (smm.Delayms(50)) return;

								G.cKernel.Motor(curMtinfo.eMt).MoveIndex(curMtinfo.nIdx);
								smm.Next(Cmd.MOVE2);
							}
						}
						break;
					case Cmd.MOVE2:
						{
							if (G.cKernel.Motor(curMtinfo.eMt).IsRdy(curMtinfo.nIdx) == true)
							{
								if (smm.Delayms(50)) return;

								G.cKernel.Motor(curMtinfo.eMt).MoveIndex(nPreIndex);
								smm.Next(Cmd.MOVE1);
							}
						}
						break;

					case Cmd.END:
						smm.Next(Cmd.START);
						break;
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}


		private bool    IsAlarm()
		{
			try
			{
				for (MT eAxis = (MT)0; eAxis < MT.TOTAL; eAxis++)
				{
					ALM err = (ALM)((int)ALM.ERROR_MT_DRIVER_START + (int)eAxis + 1);
					if (G.AlmMng.Find(err)) 
						return true;

					err = (ALM)((int)ALM.ERROR_MT_POWER_START + (int)eAxis + 1);
					if (G.AlmMng.Find(err)) 
						return true;
				}

				return false;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}
	}
	
}
