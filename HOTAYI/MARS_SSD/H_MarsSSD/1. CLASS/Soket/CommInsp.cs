using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_MarsSSD
{
	public	class CommInsp
	{
		public	delegate	void					TCPDataLogEventHandler(string strData);
		public	event		TCPDataLogEventHandler	EventTcpDataLog;
		#region Enum , Struct 
		public enum		SendCmd
		{
			RunStateReport	= 1,
			RunStateRequest,
			InspStateRequest,
			ProdDataReport,
			AutoRunStart,
			ResultDataReportConfirm,
			HomeStart,
			AlarmReport	,
			ReferenceStart,
			RejectDataReport = 11,
			VerifyDataReportConfirm,
			VerifyDataEndConfirm,
			PauseStart = 15,
			AlarmClear,
			YieldClear,
			LotStartReport,
			LotEndReport,
			JobCheckRequest,
			InspStartTimeReportConfirm	= 21,
			GroupNameRequest = 22,
			PopupMsgReq = 23,
			PopupMsgAllCloseReq = 24,
			BoardSignalRequestConfirm = 77,
			JobUpdateDataRequest, 
			JobUpdateRequest, 
			MoveSetPosRequest, 
			UserChangeRequest = 90,
			Max	= 100,
		}

		public	enum	RcvCmd
		{
			Unknown	= 0,
			RunStateReport	= 1,
			RunStateRequest,
			InspStateReport,
			ProdDataReportConfirm,
			AutoRunStartConfirm,
			ResultDataReport,
			AlarmReport,
			HomeStartConfirm,
			ProdDataRequest,
			PauseStart,
			RejectDataReportConfrim = 11,
			VerifyDataReport,
			VerifyDataReportEnd,
			//ReferencePositionRequest =15,
			TeachingStateReport =16,
			BoardSignalRequest =17,
			JobCheckReport,
			InspStartTimeReport	= 21,
			GroupNameReport = 22,
			JobUpdateDataReport = 78, 
			JobUpdateReport, 
			MoveSetPosReport, 
			Max	= 100,
		}
		public	enum	RunState
		{
			Idle	= 0,
			Init,
			Run,
			Alarm,
			ManualRun,
			Pause,
			SafetyAlarm,
			Unknown,
		}

		public	enum	InspState
		{
			Idle	= 0,
			Grab,
			Insp,
			Ref,
			Unknown,
		}

		public	enum	Bypass
		{
			None	= 0,
			Top,
			Bottom,
			All,
		}

		public	enum	Confirm
		{
			OK	= 0,
			NG,
			Unknown,
		}

		public	enum	CmdDir	//통신 방향
		{
			Unknown,
			ToTop	= 10,
			ToBtm,
			ToReview,
			FromTop,
			FromBtm,
			FromReview,
		}

		public	struct	stIP
		{
			public	string	IpTopInsp;
			public	string	IpBtmInsp;
			public	string	IpRepair;
		}
		
		private	struct stRcvData
		{
			public	RcvCmd	RcvCmd;
			public	CmdDir	CmdDir;
			public	string		strRcvData;
			public	string[]	strSplitData;
		}
		#endregion

		#region Variable
		private	Dictionary<string, stRcvData>	m_dicRcvData		= new Dictionary<string, stRcvData>();
		private	Dictionary<string, List<byte>>	m_dicSumRcvData		= new Dictionary<string, List<byte>>();

		private	bool					m_bIsConnect		= false;
		private	bool					m_bIsRecived		= false;
		private	int						m_nVerifyData		= 0;
		private	AsyncSocketServer		m_Server			= new AsyncSocketServer();
		private	string					m_strIpPort			= "";
		private	const	byte			STX					= 0x02;
		private	const	byte			ETX					= 0x03;
		private	object					m_lockRcv			= new object();
		private PStopwatch				m_swConnectCheck;
		private	CmdDir				m_CmdDir			= CmdDir.Unknown;

		#endregion

		#region Public Function
		public CommInsp()
		{
			
		}

		public	void	Init(string strIP, int nPort, CmdDir CmdDir)
		{
			try
			{
				m_CmdDir		= CmdDir;
				m_strIpPort		= string.Format("{0}:{1}", strIP, nPort);
				 
				m_bIsConnect	= false;

				m_Server		= new AsyncSocketServer();
				m_Server.Bind(nPort);
				m_Server.Listen(5);
				m_Server.ServerDataByReceived	+= Server_ServerDataByReceived;
				m_Server.ConnectReceive			+= Server_ConnectReceive;

				m_swConnectCheck = new PStopwatch();
				m_swConnectCheck.Start();    
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public	void	Update()
		{
			try
			{
				if (m_swConnectCheck.IsTimeOverMs(60000) == true)
				{
					m_swConnectCheck.Restart();
					
					if		(m_bIsRecived == true)			m_bIsRecived	= false;
					else if (m_bIsConnect == true)			m_bIsConnect	= false;
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
		
		public  void    Dispose()
		{
			if (m_Server == null)			return;

			m_Server.ServerDataByReceived	-= Server_ServerDataByReceived;
			m_Server.ConnectReceive			-= Server_ConnectReceive;

			m_Server.Dispose();
		}

		public	bool	IsConnect()
		{
			try
			{
				return m_bIsConnect;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}
		#endregion

		#region Event
		private void	Server_ConnectReceive(object sender, string strIP, int nPort, bool IsConnect)
		{
			try
			{
                m_swConnectCheck.Restart();
				if (IsConnect == true)
				{
					if (string.IsNullOrEmpty(UI.strMemberID) == false)
					{
						if (m_CmdDir == CmdDir.ToTop)	G.commInspTop.SendUserChangeRequest(UI.strMemberID, (int)Program.m_PasswordMode);
						if (m_CmdDir == CmdDir.ToBtm)	G.commInspBtm.SendUserChangeRequest(UI.strMemberID, (int)Program.m_PasswordMode);
					}
				}
				m_bIsConnect	= IsConnect;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	Server_ServerDataByReceived(object sender, string strIP, int nPort, byte[] byRcvData, int nDataLength)
		{
			try
			{
				byte[]	by_rcv_data;
				string tempIP	= string.Format("{0}:{1}", strIP, nPort);
				// 받은 data 누적
				byte[]	by_tmp	= new byte[nDataLength];
				Array.Copy(byRcvData, by_tmp, nDataLength);
				string str_key	= string.Format("{0}_{1}", strIP, nPort);

				if (m_dicSumRcvData.ContainsKey(str_key) == false)			m_dicSumRcvData.Add(str_key, new List<byte>());
				
				m_dicSumRcvData[str_key].AddRange(by_tmp);

				while (true)
				{
					System.Threading.Thread.Sleep(1);
                    //string strTemp = Encoding.UTF8.GetString(m_dicSumRcvData[str_key].ToArray());
                    //GLog.Add(strTemp);
                    by_rcv_data	= ReadRcvData(m_dicSumRcvData[str_key]);
					if (by_rcv_data == null)	break;
					
					string		strkey		= "";
					stRcvData	RcvData		= new stRcvData();

					if (ReceiveData_Parsing(by_rcv_data, ref RcvData) == false)
					{
						if (EventTcpDataLog != null)			EventTcpDataLog(string.Format("RCV - ERROR  : {0}", RcvData.strRcvData));
						GLog.Add(string.Format("RCV - ERROR : {0}", RcvData.strRcvData));
						return;
					}

					if (EventTcpDataLog != null)				EventTcpDataLog(string.Format("RCV  : {0}", RcvData.strRcvData));
					
					AddCommLog(RcvData.strRcvData);

					switch (RcvData.RcvCmd)
					{
						case RcvCmd.RunStateReport :
						case RcvCmd.InspStateReport :
						case RcvCmd.ProdDataReportConfirm :
						case RcvCmd.AutoRunStartConfirm :
						case RcvCmd.RejectDataReportConfrim :
						case RcvCmd.HomeStartConfirm :
						case RcvCmd.JobCheckReport :
						case RcvCmd.GroupNameReport :
						case RcvCmd.JobUpdateDataReport :
						case RcvCmd.JobUpdateReport :
						case RcvCmd.MoveSetPosReport :
							{
								strkey	= RcvData.RcvCmd.ToString();
								lock (m_lockRcv)
								{
									if (m_dicRcvData.ContainsKey(strkey) == true)			m_dicRcvData[strkey]	= RcvData;
									else													m_dicRcvData.Add(strkey, RcvData);
								}
							}
							break;
							
						case RcvCmd.RunStateRequest :
							{
								m_bIsRecived	= true;
								
								SendRunStateReport();
							}
							break;
						case RcvCmd.ResultDataReport :
							{
								if		(RcvData.CmdDir == CmdDir.FromTop  &&  SM.SHM.GetUseSkipData(USESKIP.TopInspection) == false)			break;
								else if (RcvData.CmdDir == CmdDir.FromBtm  &&  SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == false)			break;

								int		nResult;
								if (SM.FMP[FlowPart.Insp1].GetMaxProdCount() * 3 != RcvData.strSplitData.Length)
								{
									G.AlmMng.ADD(ALM.ERROR_COMM_INSP_DATA, string.Format("Data Length Error. {0}", RcvData.strRcvData));
									break;
								}

								for(int i = 0; i < SM.FMP[FlowPart.Insp1].GetMaxProdCount(); i++)
								{
									if (int.TryParse(RcvData.strSplitData[i*3+2], out nResult) == false)
									{
										SendResultDataReportConfirm(Confirm.NG);

										G.AlmMng.ADD(ALM.ERROR_COMM_INSP_DATA, string.Format("Receive : {0}", Encoding.Default.GetString(by_rcv_data)));

										return;
									}

									//상부검사 진행 중인데, 바코드 정보가 넘어오지 않으면 알람 처리
									if (RcvData.CmdDir == CmdDir.FromTop  &&
										SM.SHM.GetUseSkipData(USESKIP.SGEM) == true  && 
										string.IsNullOrEmpty(RcvData.strSplitData[i * 3 + 0]) == false  &&
										RcvData.strSplitData[i * 3 + 1].ToLower().Contains("error") == false ) //바코드, 피듀셜 NG인 경우 해당 값에 ERROR로 받음
									{
										string strBarcodeData =  RcvData.strSplitData[i * 3 + 1];
										if (string.IsNullOrEmpty(strBarcodeData))
										{
											G.AlmMng.ADD(ALM.ERROR_COMM_INSP_DATA, string.Format("BARCODE Data is Empty. Please Check the Job"));
											SendResultDataReportConfirm(Confirm.NG);
											return;
										}
									}

									if (SM.SHM.GetUseSkipData(USESKIP.InspResultAlwaysOK) == true)
									{
										if (RcvData.CmdDir == CmdDir.FromTop)			SM.FMP.SetInspResult(FlowMngPart.teDir.Top, RcvData.strSplitData[i*3+0], ProdStatus.OK, RcvData.strSplitData[i*3+1]);
										else											SM.FMP.SetInspResult(FlowMngPart.teDir.Btm, RcvData.strSplitData[i*3+0], ProdStatus.OK, RcvData.strSplitData[i*3+1]);
									}
									else
									{
										if (RcvData.CmdDir == CmdDir.FromTop)			SM.FMP.SetInspResult(FlowMngPart.teDir.Top, RcvData.strSplitData[i*3+0], (ProdStatus)(nResult+1), RcvData.strSplitData[i*3+1]);
										else											SM.FMP.SetInspResult(FlowMngPart.teDir.Btm, RcvData.strSplitData[i*3+0], (ProdStatus)(nResult+1), RcvData.strSplitData[i*3+1]);
									}
								}

								SendResultDataReportConfirm(Confirm.OK);
							}
							break;
						case RcvCmd.AlarmReport :
							{
								int nInspAlarmCode = int.Parse(RcvData.strSplitData[0]); 
								ALM eAlarm = ALM.WARNING_AOI_ALARM_THAT_IS_NOT_KNOWN_HAS_OCCURRED_PLEASE_RESTART_TO_RESET_THE_ALARM +  nInspAlarmCode;
								G.AlmMng.ADD(eAlarm);
							}
							break;
						case RcvCmd.VerifyDataReport :
							{
								int		nResult;
								if (int.TryParse(RcvData.strSplitData[1], out nResult) == false)
								{
									SendVerifyDataReportConfirm(Confirm.NG);
									G.AlmMng.ADD(ALM.ERROR_COMM_INSP_DATA, string.Format("Receive : {0}", Encoding.Default.GetString(by_rcv_data)));

									return;
								}

								if (SM.FMT[FlowTray.Reject].SetVerifyResult(RcvData.strSplitData[0], (ProdStatus)nResult+1, RcvData.strSplitData[2]=="T" ? true : false) == true)
								{
									SendVerifyDataReportConfirm(Confirm.OK);
								}
								//MES NG인 경우 컨펌 없이 트레이를 교체할 수 있기때문에, 리젝 버퍼 정보도 체크
								else if (SM.FMR.GetMESNGExistID(RcvData.strSplitData[0]) == true)
								{
									SendVerifyDataReportConfirm(Confirm.OK);
								}
								else
								{
									SendVerifyDataReportConfirm(Confirm.NG);
								}
							}
							break;
						case RcvCmd.VerifyDataReportEnd :
                            {
                                NDm.Write(eNDM.RejectRunMode, (int)RejectRunMode.VerifyComplete);

                                SendVerifyDataReportEndConfirm(Confirm.OK);
							}
							break;
						case RcvCmd.ProdDataRequest:
							{
								FlowPart curFlow = FlowPart.Empty;
								int curRailNo = 0;

								if (RcvData.strSplitData[0] == "0")
								{
									curRailNo = 1;
									curFlow = FlowPart.Insp1;
								}
								else 
								{
									curRailNo = 2;
									curFlow = FlowPart.Insp2;
								}

								InfoPart.tsInfo info = SM.FMP[curFlow].GetInfo();
								string[] strProdID = new string[info.ProdInfo.Length];
								for (int i = 0; i < info.ProdInfo.Length; i++)
								{
									strProdID[i] = info.ProdInfo[i].ProdID;
								}

								SendProdDataReport(curRailNo, SM.FMP[curFlow].GetInfo());
							}
							break;
						case RcvCmd.PauseStart:
							{
								if (SM.SHM.IsBitData(BIT.AutoRun) == true)
								{
									SM.SHM.SetBitData(BIT.Pause, true);
									SM.SHM.MsgShow(SeqMsg.InspPause);
								}
								G.commInspBtm.SendPauseStart(1);
								G.commInspTop.SendPauseStart(1);

							}
							break;
						case RcvCmd.BoardSignalRequest :
							{ 
								if (int.TryParse(RcvData.strSplitData[0], out int nLane) && int.TryParse(RcvData.strSplitData[1], out int nState))
								{
									bool bState = nState == 1 ? true : false; 
									if		(RcvData.CmdDir == CmdDir.FromTop && nLane == 0	&&	G.seqInspPlate1.IsGrabPosition() == true)			G.cKernel.AOut(DO.TopInspReady1, bState); //검사 위치에 보드있는지 체크
									else if (RcvData.CmdDir == CmdDir.FromTop && nLane == 1	&&	G.seqInspPlate2.IsGrabPosition() == true)			G.cKernel.AOut(DO.TopInspReady2, bState);
									else if (RcvData.CmdDir == CmdDir.FromBtm && nLane == 0	&&	G.seqInspPlate1.IsGrabPosition() == true)			G.cKernel.AOut(DO.BtmInspReady1, bState);
									else if (RcvData.CmdDir == CmdDir.FromBtm && nLane == 1	&&	G.seqInspPlate2.IsGrabPosition() == true)			G.cKernel.AOut(DO.BtmInspReady2, bState);
									
									SendBoardSignalReqConfirm(nLane, nState);
								}
							}
							break;
						case RcvCmd.InspStartTimeReport :
							{
								if		(RcvData.CmdDir == CmdDir.FromTop  &&  SM.SHM.GetUseSkipData(USESKIP.TopInspection) == false)			break;
								else if (RcvData.CmdDir == CmdDir.FromBtm  &&  SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == false)			break;

								if (RcvData.strSplitData.Length != SM.FMP[FlowPart.Insp1].GetMaxProdCount()*2)
								{
									G.AlmMng.ADD(ALM.ERROR_COMM_INSP_DATA, string.Format("Data Length Error. {0}", RcvData.strRcvData));
									break;
								}

								for(int i = 0; i < SM.FMP[FlowPart.Empty].GetMaxProdCount(); i++)
								{
									if (RcvData.CmdDir == CmdDir.FromTop)			SM.FMP.SetInspTime(FlowMngPart.teDir.Top, RcvData.strSplitData[i*2+0], RcvData.strSplitData[i*2+1]);
									else											SM.FMP.SetInspTime(FlowMngPart.teDir.Btm, RcvData.strSplitData[i*2+0], RcvData.strSplitData[i*2+1]);
								}

								SendInspStartTimeReportConfirm(Confirm.OK);
							}
							break;
					}
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
		#endregion

		#region Send
		public int		SendRunStateReport()
		{
			try
			{
				if (IsConnect() == false)			return -1;

				string	strdata	= "0";

				if		(SM.SHM.IsBitData(BIT.AllHoming) == true)			strdata	= ((int)RunState.Init).ToString();
				else if	(SM.SHM.IsBitData(BIT.AutoRun) == true)				strdata	= ((int)RunState.Run).ToString();
				else if (SM.SHM.IsBitData(BIT.Pause) == true)				strdata	= ((int)RunState.Pause).ToString();
				else if (SM.SHM.IsBitData(BIT.Error) == true)				strdata	= ((int)RunState.Alarm).ToString();
				else if (SM.SHM.IsBitData(BIT.DoorOpen) == true)			strdata	= ((int)RunState.SafetyAlarm).ToString();
				else														strdata	= ((int)RunState.Idle).ToString();

				Send(SendCmd.RunStateReport, strdata);

				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}

		public	int		SendProdDataReport(int nLaneNo, InfoPart.tsInfo PartInfo)
		{
			try
			{
				if (IsConnect() == false)			return -1;

				string strdata	= $"{nLaneNo},{PartInfo.WorkOrder},{PartInfo.ProgramID_Top}@{PartInfo.ProgramID_Btm}";

				for (int i = 0; i < PartInfo.ProdInfo.Length; i++)
				{
                    strdata += ',';
                    strdata	+= PartInfo.ProdInfo[i].ProdID;
                    strdata += ',';
                    strdata	+= PartInfo.ProdInfo[i].TrayID;
                    strdata += ',';
                    strdata	+= PartInfo.ProdInfo[i].TrayCol;
                    strdata += ',';
                    strdata	+= PartInfo.ProdInfo[i].TrayRow;
				}

				ResetRcvData(RcvCmd.ProdDataReportConfirm);
				Send(SendCmd.ProdDataReport, strdata);

				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}

		public	int		SendRunStateRequest()
		{
			try
			{
				if (IsConnect() == false)			return -1;

				ResetRcvData(RcvCmd.RunStateReport);
				Send(SendCmd.RunStateRequest, "");

				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}

		public	int		SendAutoRunStart()
		{
			try
			{
				if (IsConnect() == false)			return -1;
				int nDebug = 0;
				if		(SM.SHM.GetUseSkipData(USESKIP.InspDebugPlate1) == true)			nDebug	= 1;
				else if (SM.SHM.GetUseSkipData(USESKIP.InspDebugPlate2) == true)			nDebug	= 2;

				ResetRcvData(RcvCmd.AutoRunStartConfirm);
				Send(SendCmd.AutoRunStart, nDebug.ToString());

				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}

		public	int		SendInspStateRequest()
		{
			try
			{
				if (IsConnect() == false)			return -1;

				ResetRcvData(RcvCmd.InspStateReport);
				Send(SendCmd.InspStateRequest, "");

				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}

		public	int		SendResultDataReportConfirm(Confirm Confirm)
		{
			try
			{
				if (IsConnect() == false)			return -1;

				Send(SendCmd.ResultDataReportConfirm, ((int)Confirm).ToString());

				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}

		public	int		SendRejectDataReport(string ProdID, int nRow, int nCol, int nTotalRow,int nTotalCol, MESStatus MesState = MESStatus.Unknown)
		{
			try
			{
				if (IsConnect() == false)			return -1;

				ResetRcvData(RcvCmd.RejectDataReportConfrim);
				
				int nMesResult =  MesState == MESStatus.NG ? 1 : 0;
				
				Send(SendCmd.RejectDataReport, string.Format("{0},{1},{2},{3},{4}, {5}", ProdID, nRow, nCol, nTotalRow, nTotalCol, nMesResult));
				

				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}

		public	int		SendVerifyDataReportConfirm(Confirm Confirm)
		{
			try
			{
				if (IsConnect() == false)			return -1;

				Send(SendCmd.VerifyDataReportConfirm, ((int)Confirm).ToString());

				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}

		public	int		SendVerifyDataReportEndConfirm(Confirm Confirm)
		{
			try
			{
				if (IsConnect() == false)			return -1;

				Send(SendCmd.VerifyDataEndConfirm, ((int)Confirm).ToString());

				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}

		public	int		SendHomeStart()
		{
			try
			{
				if (IsConnect() == false)			return -1;

				ResetRcvData(RcvCmd.HomeStartConfirm);
				Send(SendCmd.HomeStart, "");

				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}
		
		public	int		SendReferenceStart(int nhead) // 0 : TOP1(BTM1), 1 : TOP2(BTM2)
		{
			try
			{
				if (IsConnect() == false)			return -1;

				string strData = string.Format("{0}", nhead.ToString() /*,nAutoStart.ToString()*/);
				Send(SendCmd.ReferenceStart, strData);

				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}

		//nData  1:stop, 2:Cycle stop
		public	int		SendPauseStart(int nData)
		{
			try
			{
				if (IsConnect() == false)			return -1;

				Send(SendCmd.PauseStart, nData.ToString());

				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}

		public	int		SendAlarmClear()
		{
			try
			{
				if (IsConnect() == false)						return -1;
				//검사쪽 꼬일 위험이 있어서, 자동 런 중에는 알람 클리어 보내지 않음
				if (SM.SHM.IsBitData(BIT.AutoRun) == true)		return 1;

				Send(SendCmd.AlarmClear, "");

				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}

		public int		SendAlarmReport(int Alarm)
		{
			try
			{
				if (IsConnect() == false)			return -1;
				
				Send(SendCmd.AlarmReport, Alarm.ToString());

				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}

		}

		public	int		SendYieldClear()
		{
			try
			{
				if (IsConnect() == false)			return -1;

				Send(SendCmd.YieldClear, "");

				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}

		
		public  int		SendGroupNameRequest(string DeviceID)
		{
			try
			{
				if (IsConnect() == false)			return -1;
                
				ResetRcvData(CommInsp.RcvCmd.GroupNameReport);
				Send(CommInsp.SendCmd.GroupNameRequest, DeviceID);

				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}


		public	int		SendBoardSignalReqConfirm (int nLane, int nState)
		{
			try
			{
				if (IsConnect() == false)			return -1;
				
				string strData = nLane.ToString() + "," + nState.ToString();

				Send(CommInsp.SendCmd.BoardSignalRequestConfirm, strData);

				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}

		public	int		SendInspStartTimeReportConfirm(Confirm Confirm)
		{
			try
			{
				if (IsConnect() == false)			return -1;

				Send(SendCmd.InspStartTimeReportConfirm, ((int)Confirm).ToString());

				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}
		
		public  int		SendJobCheckRequest(string JobName)
		{
			try
			{
				if (IsConnect() == false)			return -1;

				//0: Job Change / 1: Don’t Job Change 
				string strJobChange = "1";
				//Load 부분에 제품이 없을 땐 job change 진행
				if (SM.FMP.IsFlowRange(FlowPart.LDPnP, FlowPart.Insp2) == false  &&
					SM.FMT.IsFlowRange(FlowTray.LDConv01, FlowTray.LDTable2) == false)
				{
					strJobChange = "0";
				}

				ResetRcvData(RcvCmd.JobCheckReport);
				Send(SendCmd.JobCheckRequest, JobName + "," + strJobChange);

                //Send(teSendCmd.JobCheckRequest, JobName);
                return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}
		
		public  int		SendJobUpdateDataRequest()
		{
			try
			{
				if (IsConnect() == false)			return -1;

				ResetRcvData(RcvCmd.JobUpdateDataReport);
				Send(SendCmd.JobUpdateDataRequest,"");

                return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}
		
		public  int		SendJobUpdateRequest()
		{
			try
			{
				if (IsConnect() == false)			return -1;

				ResetRcvData(RcvCmd.JobUpdateReport);
				Send(SendCmd.JobUpdateRequest,"");

                return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}

		//head(0:1번헤드/ 1: 2번헤드) , MoveType(0:Light Jig/ 1:3D Jig/ 2: 1StageMroPos/ 3: 2StageMropPos/ 4 CamMroPos)
		public  int		SendMoveSetPosRequest(int nHead, int nJigType)
		{
			try
			{
				if (IsConnect() == false)			return -1;

				string strMsg = $"{nHead.ToString()},{nJigType.ToString()}";

				ResetRcvData(RcvCmd.MoveSetPosReport);
				Send(SendCmd.MoveSetPosRequest, strMsg);

                return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}

		public	int		SendPopupMsgReq(string msg)
		{
			try
			{
				if (IsConnect() == false)			return -1;
				
                Send(SendCmd.PopupMsgReq, msg);

				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}

		public	int		SendPopupMsgAllCloseReq()
		{
			try
			{
				if (IsConnect() == false)			return -1;
				
                Send(SendCmd.PopupMsgAllCloseReq, "");

				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}
		
		// 모든 Send 함수들이 쓰는 기본 Send 함수
		private bool	Send(SendCmd teCmd, string strData)
		{
			try
			{
				byte[]		bydata;
				byte[]		bypacket;
				
				string		strcmd				= ((int)teCmd).ToString();
				string		strdir				= ((int)m_CmdDir).ToString();
				string		strdata_for_packet	= strcmd + "`" + strdir + "`" + strData;
				string		strcheck_sum		= GetHexChecksum(strdata_for_packet);

				strdata_for_packet				+= strcheck_sum; // CheckSum을 뒤에 그냥 붙임
				bydata							= Encoding.Default.GetBytes(strdata_for_packet); // byte화
				bypacket						= new byte[bydata.Length + 2]; // CheckSum포함한 데이터 + STX, ETX

                bypacket[0]						= STX;
				bypacket[bypacket.Length - 1]	= ETX; // STX, ETX 입력
            
				Array.Copy(bydata, 0, bypacket, 1, bydata.Length);

				string strSendData			= string.Format("<STX><{0}><{1}><{2}><{3}><ETX>", teCmd, m_CmdDir, strData, strcheck_sum);
				
				if (EventTcpDataLog != null)			EventTcpDataLog(strSendData);
                
                AddCommLog(strSendData);

				if (m_Server.Send(m_strIpPort, bypacket) == false)			return false;

				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
      }

		
		public  int		SendUserChangeRequest(string strID, int nLevel)
		{
			try
			{
				if (IsConnect() == false)			return -1;
				if (nLevel > 2) nLevel = 2;
				
				string strMsg = $"{strID}, {nLevel.ToString()}";
				Send(SendCmd.UserChangeRequest, strMsg);

				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}


		#endregion

		#region Receive
		public int		RcvRunStateReport(ref RunState RunState)
		{
			try
			{
				string	strkey	= RcvCmd.RunStateReport.ToString();
				int		nval	= 0;

				if (m_dicRcvData.ContainsKey(strkey) == false)			return 0;
				if (m_dicRcvData[strkey].strSplitData.Length != 1  ||
					int.TryParse(m_dicRcvData[strkey].strSplitData[0], out nval) == false)
				{
					G.AlmMng.ADD(ALM.ERROR_COMM_INSP_DATA, m_dicRcvData[strkey].strRcvData);
					return -1;
				}

				RunState	= (RunState)nval;
				
				m_dicRcvData.Remove(strkey);
				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}

		public	int		RcvInspStateReport(ref InspState InspState)
		{
			try
			{
				string	strkey	= RcvCmd.InspStateReport.ToString();
				int		nval	= 0;

				if (m_dicRcvData.ContainsKey(strkey) == false)			return 0;
				if (m_dicRcvData[strkey].strSplitData.Length != 1  ||
					int.TryParse(m_dicRcvData[strkey].strSplitData[0], out nval) == false)
				{
					G.AlmMng.ADD(ALM.ERROR_COMM_INSP_DATA, m_dicRcvData[strkey].strRcvData);
					return -1;
				}

				InspState	= (InspState)nval;
				
				m_dicRcvData.Remove(strkey);
				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}

		public	int		RcvProdDataReportConfirm(ref bool bConfirmOK)
		{
			try
			{
				string	strkey	= RcvCmd.ProdDataReportConfirm.ToString();
				int		nval	= 0;

				if (m_dicRcvData.ContainsKey(strkey) == false)			return 0;
				if (m_dicRcvData[strkey].strSplitData.Length != 1  ||
					int.TryParse(m_dicRcvData[strkey].strSplitData[0], out nval) == false)
				{
					G.AlmMng.ADD(ALM.ERROR_COMM_INSP_DATA, m_dicRcvData[strkey].strRcvData);
					return -1;
				}

				bConfirmOK	= nval == 0 ? true : false;
				
				m_dicRcvData.Remove(strkey);
				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}

		public	int		RcvAutoRunStartConfirm(ref bool bConfirmOK)
		{
			try
			{
				string	strkey	= RcvCmd.AutoRunStartConfirm.ToString();
				int		nval	= 0;

				if (m_dicRcvData.ContainsKey(strkey) == false)			return 0;
				if (m_dicRcvData[strkey].strSplitData.Length != 1  ||
					int.TryParse(m_dicRcvData[strkey].strSplitData[0], out nval) == false)
				{
					G.AlmMng.ADD(ALM.ERROR_COMM_INSP_DATA, m_dicRcvData[strkey].strRcvData);
					return -1;
				}

				bConfirmOK	= nval == 0 ? true : false;
				
				m_dicRcvData.Remove(strkey);
				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}

		public	int		RcvRejectDataReportConfrim(ref bool bConfirmOK)
		{
			try
			{
				string		strkey	= RcvCmd.RejectDataReportConfrim.ToString();
				int			nval	= 0;

				if (m_dicRcvData.ContainsKey(strkey) == false)			return 0;
				if (m_dicRcvData[strkey].strSplitData.Length != 1  ||
					int.TryParse(m_dicRcvData[strkey].strSplitData[0], out nval) == false)
				{
					G.AlmMng.ADD(ALM.ERROR_COMM_INSP_DATA, m_dicRcvData[strkey].strRcvData);
					return -1;
				}

				bConfirmOK	= nval == 0 ? true : false;
				
				m_dicRcvData.Remove(strkey);
				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}

		public	int		RcvHomeStartConfirm(ref bool bConfirmOK)
		{
			try
			{
				string	strkey	= RcvCmd.HomeStartConfirm.ToString();
				int		nval	= 0;

				if (m_dicRcvData.ContainsKey(strkey) == false)			return 0;
				if (m_dicRcvData[strkey].strSplitData.Length != 1  ||
					int.TryParse(m_dicRcvData[strkey].strSplitData[0], out nval) == false)
				{
					G.AlmMng.ADD(ALM.ERROR_COMM_INSP_DATA, m_dicRcvData[strkey].strRcvData);
					return -1;
				}

				bConfirmOK	= nval == 0 ? true : false;
				
				m_dicRcvData.Remove(strkey);
				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}
		
		public	int		RcvJobCheckReport(ref bool bState)
		{
			try
			{
				string	strkey	= RcvCmd.JobCheckReport.ToString();
				int		nval	= 0;

				if (m_dicRcvData.ContainsKey(strkey) == false)			return 0;
				if (m_dicRcvData[strkey].strSplitData.Length != 1  ||
					int.TryParse(m_dicRcvData[strkey].strSplitData[0], out nval) == false)
				{
					G.AlmMng.ADD(ALM.ERROR_COMM_INSP_DATA, m_dicRcvData[strkey].strRcvData);
					return -1;
				}

				bState	= nval == 0 ? true : false;
				
				m_dicRcvData.Remove(strkey);
				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}
		
		public	int		RcvRunStateRequest(ref bool bState)
		{
			try
			{
				string	strkey	= RcvCmd.RunStateRequest.ToString();
				int		nval	= 0;

				if (m_dicRcvData.ContainsKey(strkey) == false)			return 0;
				if (m_dicRcvData[strkey].strSplitData.Length != 1  ||
					int.TryParse(m_dicRcvData[strkey].strSplitData[0], out nval) == false)
				{
					G.AlmMng.ADD(ALM.ERROR_COMM_INSP_DATA, m_dicRcvData[strkey].strRcvData);
					return -1;
				}

				bState	= nval == 0 ? true : false;
				
				m_dicRcvData.Remove(strkey);
				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}

		public	int		RcvGroupNameReport(ref string strGroupName)
		{
			try
			{
				string	strkey	= RcvCmd.GroupNameReport.ToString();

				if (m_dicRcvData.ContainsKey(strkey) == false)			return 0;
				if (m_dicRcvData[strkey].strSplitData.Length != 1  ||
					string.IsNullOrWhiteSpace(m_dicRcvData[strkey].strSplitData[0]) == true)
				{
					G.AlmMng.ADD(ALM.ERROR_COMM_INSP_DATA, m_dicRcvData[strkey].strRcvData);
					return -1;
				}

				strGroupName	= m_dicRcvData[strkey].strSplitData[0];
				
				m_dicRcvData.Remove(strkey);
				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}
		

		public	int		RcvJobUpdateDataReport(ref bool bResultOK)
		{
			try
			{
				string	strkey	= RcvCmd.JobUpdateDataReport.ToString();
				int		nval	= 0;

				if (m_dicRcvData.ContainsKey(strkey) == false)			return 0;
				if (m_dicRcvData[strkey].strSplitData.Length != 1  ||
					int.TryParse(m_dicRcvData[strkey].strSplitData[0], out nval) == false)
				{
					G.AlmMng.ADD(ALM.ERROR_COMM_INSP_DATA, m_dicRcvData[strkey].strRcvData);
					return -1;
				}

				bResultOK	= nval == 1 ? true : false;
				
				m_dicRcvData.Remove(strkey);
				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}
		

		public	int		RcvJobUpdateReport(ref bool bResultOK)
		{
			try
			{
				string	strkey	= RcvCmd.JobUpdateReport.ToString();
				int		nval	= 0;

				if (m_dicRcvData.ContainsKey(strkey) == false)			return 0;
				if (m_dicRcvData[strkey].strSplitData.Length != 1  ||
					int.TryParse(m_dicRcvData[strkey].strSplitData[0], out nval) == false)
				{
					G.AlmMng.ADD(ALM.ERROR_COMM_INSP_DATA, m_dicRcvData[strkey].strRcvData);
					return -1;
				}

				bResultOK	= nval == 0 ? true : false;
				
				m_dicRcvData.Remove(strkey);
				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}
		

		public	int		RcvMoveSetPosReport(ref bool bResultOK)
		{
			try
			{
				string	strkey	= RcvCmd.MoveSetPosReport.ToString();
				int		nval	= 0;

				if (m_dicRcvData.ContainsKey(strkey) == false)			return 0;
				if (m_dicRcvData[strkey].strSplitData.Length != 1  ||
					int.TryParse(m_dicRcvData[strkey].strSplitData[0], out nval) == false)
				{
					G.AlmMng.ADD(ALM.ERROR_COMM_INSP_DATA, m_dicRcvData[strkey].strRcvData);
					return -1;
				}

				bResultOK	= nval == 0 ? true : false;
				
				m_dicRcvData.Remove(strkey);
				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}

		public	void	ResetRcvData(RcvCmd Cmd)
		{
			try
			{
				string	strkey	= Cmd.ToString();

				if (m_dicRcvData.ContainsKey(strkey) == false)			return;
				
				m_dicRcvData.Remove(strkey);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public	void	ResetRcvDataAll()
		{
			try
			{
				m_dicRcvData.Clear();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
		#endregion


		#region private Function 
		
		private void	AddCommLog(string strData)
		{
			try
			{
				string[] strTemp = strData.Split('>');
				for (int i = 0; i < strTemp.Length; i++)
				{
					strTemp[i] = strTemp[i].Trim('<');
				}
				string cmd = strTemp[1];
				string cmdDir = strTemp[2];
				string str = strTemp[3];

				switch (cmd)
				{
					case "RunStateReport":
						{
							if (SM.SHM.GetUseSkipData(USESKIP.RunStateLog) == false)
								return;
                            
                            int nResult = 0;
                            if (int.TryParse(str , out nResult) == true)
                                str = ((RunState)nResult).ToString();
                            
                        }
						break;
					case "RunStateRequest":
						{
							if (SM.SHM.GetUseSkipData(USESKIP.RunStateLog) == false)
								return;

							str = "";
						}
						break;
					case "InspStateReport":
                        {
                            int nResult = 0;
                            if (int.TryParse(str, out nResult) == true)
                                str = ((InspState)nResult).ToString();
						}
						break;
					case "ProdDataReportConfirm":
					case "AutoRunStartConfirm":
					case "HomeStartConfirm":
					case "RejectDataReportConfrim":
					case "ResultDataReportConfirm":
					case "VerifyDataReportConfirm":
					case "VerifyDataEndConfirm":
                        {
                            int nResult = 0;
                            if (int.TryParse(str, out nResult) == true)
                                str = ((Confirm)nResult).ToString();
						}
						break;
					case "ResultDataReport":
						{
							string[] strtemp = str.Split(',');
							str = "";

							for (int i = 0; i < strtemp.Length; i++)
							{
								if (i % 3 == 2)
								{
                                    int nResult = 0;
                                    if (int.TryParse(strtemp[i], out nResult) == true)
                                        strtemp[i] = ((ProdStatus)nResult+1).ToString();
                                    
								}

                                str +=  strtemp[i] + "," ;
							}
						}
						break;
					case "VerifyDataReport":
						{
							string[] strtemp = str.Split(',');
							str = "";
							for (int i = 0; i < strtemp.Length; i++)
							{
								if (i % 2 == 1)
                                {
                                    int nResult = 0;
                                    if (int.TryParse(strtemp[i], out nResult) == true)
                                        strtemp[i] = ((ProdStatus)nResult+1).ToString();
								}
								str +=  strtemp[i] + "," ;
							}
						}
						break;
				}

				GLog.Add(string.Format("TCP:{0},{1},{2}", cmd, cmdDir, str));
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private bool	ReceiveData_Parsing(byte[] byRcvData, ref stRcvData RcvData)
		{
		try
			{
				string		strDataTmp	= "";
				string		strrcv_data	= Encoding.Default.GetString(byRcvData);
				string		strcheck_sum = "";
				string[]	strrcv_data_split_list;
				RcvCmd	Cmd			= RcvCmd.Unknown;
				CmdDir	CmdDir		= CmdDir.Unknown;

				strrcv_data				= strrcv_data.Remove(0, 1); // STX 삭제
				strrcv_data				= strrcv_data.Remove(strrcv_data.Length - 1, 1); // ETX 삭제
				strcheck_sum			= strrcv_data.Substring(strrcv_data.Length - 2, 2); // CheckSum 얻음
				strrcv_data				= strrcv_data.Remove(strrcv_data.Length - 2, 2); // CheckSum 삭제
				strrcv_data_split_list	= strrcv_data.Split('`'); // ` 기준으로 split

				if (strrcv_data_split_list.Length < 3  ||  strrcv_data_split_list.Length > 3)
				{
					G.AlmMng.ADD(ALM.ERROR_COMM_INSP_DATA, string.Format("Data Length Error. {0}", Encoding.Default.GetString(byRcvData)));
					return false;
				}

				int	tempint;
				if (int.TryParse(strrcv_data_split_list[0],out tempint))
				{
					Cmd		= (RcvCmd)tempint;
				}
				else
				{
					G.AlmMng.ADD(ALM.ERROR_COMM_INSP_DATA, string.Format("Data CMD Error. {0}", strrcv_data_split_list[0]));
					return false;
				}
				
				if (int.TryParse(strrcv_data_split_list[1],out tempint))
				{
					CmdDir	= (CmdDir)tempint;
				}
				else
				{
					G.AlmMng.ADD(ALM.ERROR_COMM_INSP_DATA, string.Format("Data Direction Error. {0}", strrcv_data_split_list[1]));
					return false;
				}

				strDataTmp		= strrcv_data_split_list[2];

				if (strcheck_sum != GetHexChecksum(strrcv_data))
				{
					RcvData.strRcvData		= string.Format("<STX><{0}><{1}><{2}><{3}><ETX>", Cmd, CmdDir, strDataTmp, strcheck_sum);
					G.AlmMng.ADD(ALM.ERROR_COMM_INSP_DATA, string.Format("Data CheckSum Error. {0}", RcvData.strRcvData));

					return false;
				}
				
				RcvData.RcvCmd			= Cmd;
				RcvData.CmdDir			= CmdDir;
				RcvData.strRcvData		= string.Format("<STX><{0}><{1}><{2}><{3}><ETX>", Cmd, CmdDir, strDataTmp, strcheck_sum);
				RcvData.strSplitData	= strDataTmp.Split(',');

				return true;
			}
			catch (System.Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		private string	ByteToHex(byte[] bytes)
		{
			try
			{
				string hex = BitConverter.ToString(bytes);
				return hex.Replace("-", "");
			}
			catch (System.Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return "ByteToHex_Catch";
			}
		}

		private	bool	IndexOfStxEtx(List<byte> ltRcvData, ref int nIdxStx, ref int nIdxEtx)
		{
			try
			{
				while (true)
				{
					nIdxStx	= ltRcvData.IndexOf(STX);
					nIdxEtx	= ltRcvData.IndexOf(ETX);

					if (nIdxStx >= 0  &&  nIdxEtx >= 0)
					{
						// ETX가 앞에 있을 경우 STX의 내용 삭제
						if		(nIdxStx > nIdxEtx)
						{
							//GLog.Add("IndexOfStxEtx(nIdxStx > nIdxEtx)");
							ltRcvData.RemoveRange(0, nIdxStx);
							continue;
						}
						// STX 앞에 ETX가 없지만 0이 아닐 경우 앞의 내용 삭제
						else if (nIdxStx > 0)
						{
							//GLog.Add("IndexOfStxEtx(nIdxStx > 0)");
							ltRcvData.RemoveRange(0, nIdxStx);
							continue;
						}

						int	nidx_stx	= ltRcvData.IndexOf(STX, nIdxStx+1, nIdxEtx-(nIdxStx+1));
						if (nidx_stx >= 0)
						{
						   //	GLog.Add("IndexOfStxEtx(nidx_stx >= 0)");
							ltRcvData.RemoveRange(0, nidx_stx);
							continue;
						}

						break;
					}
					else
					{
					   // GLog.Add(string.Format("IndexOfStxEtx(False) nIdxStx : {0}, nIdxEtx : {1}", nIdxStx,nIdxEtx));
						return false;
					}
				}
				//GLog.Add(string.Format("3. IndexOfStxEtx(True) nIdxStx : {0},nIdxEtx : {0}", nIdxStx,nIdxEtx));
				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		private	byte[]	ReadRcvData(List<byte> ltRcvData)
		{
            if (ltRcvData.Count == 0)  return null;

			byte[] by_rcv_data	= null;
			try
			{
				int	nidx_stx	= 0;
				int	nidx_etx	= 0;

				if (IndexOfStxEtx(ltRcvData, ref nidx_stx, ref nidx_etx) == false)
				{
					//메모리 누수 체크, STX와 ETX가 없는 데이터가 특정 이상 쌓이는 경우
					if (ltRcvData.Count > 10000) 
					{
						GLog.Add($"Warning: ltRcvData buffer size exceeded limit! Size: {ltRcvData.Count}");
					}
					return null;
				}

				int	ncnt	= nidx_etx - nidx_stx + 1;

				by_rcv_data	= ltRcvData.GetRange(nidx_stx, ncnt).ToArray();
				
				ltRcvData.RemoveRange(nidx_stx, ncnt);
				//메모리 누수 체크, 조건에 맞는 데이터 삭제후 담은 데이터 사이즈 확인
				if (ltRcvData.Count > 10000) 
					GLog.Add($"Unprocessed data remains in ltRcvData, size: {ltRcvData.Count}");
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
			return by_rcv_data;
		}
		
		private string	GetHexChecksum(string strInput)
        {
            if (string.IsNullOrEmpty(strInput))
                return "";

            uint unsum = 0;
            foreach (char c in strInput)
            {
                unsum += (uint)c;
            }
            string strhex_value = unsum.ToString("X");
            return strhex_value.Substring(strhex_value.Length - 2, 2);
        }

		#endregion
	}
}
