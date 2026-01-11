using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_MarsSSD
{
	public	class CommInsp_Sim
	{
		public	delegate	void					TCPDataLogEventHandler(string strData);
		public	event		TCPDataLogEventHandler	EventTcpDataLog;

		public struct	tsIP
		{
			//public	const	string IpTopInsp	= "192.168.0.12";
			public	const	string IpTopInsp	= "127.0.0.1";
			public	const	string IpBtmInsp	= "192.168.0.13";
			public	const	string IpReview		= "192.168.0.14";
		}

		private	struct tsRcvData
		{
			public	CommInsp.RcvCmd	RcvCmd;
			public	CommInsp.CmdDir	CmdDir;
			public	string		strRcvData;
			public	string[]	strSplitData;
		}


		Dictionary<string, tsRcvData>	m_dicRcvData	= new Dictionary<string, tsRcvData>();
		private	bool				m_bIsConnect		= false;
		private	bool				m_bIsRecived		= false;
		private	int					m_nVerifyData		= 0;
		private	AsyncSocketServer	m_Server			= new AsyncSocketServer();
		private	string				m_strIpPort			= "";
		private	const	byte		STX					= 0x02;
		private	const	byte		ETX					= 0x03;
		private	object				m_lockRcv			= new object();
		private	List<byte>			m_ltSumRcvData		= new List<byte>();
		private PStopwatch			m_swConnectCheck;
		private	CommInsp.CmdDir	m_CmdDir			= CommInsp.CmdDir.Unknown;
		private	Random				m_result_random		= new Random();



		public			CommInsp_Sim()
		{
			
		}

		public	void	Init(string strIP, int nPort, CommInsp.CmdDir CmdDir)
		{
			try
			{
				m_CmdDir		= CmdDir;
				m_strIpPort		= string.Format("{0}:{1}", strIP, nPort);

				m_bIsConnect	= true;

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

			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public	void	Dispose()
		{
			if (m_Server == null)			return;

			m_Server.Dispose();
		}


		private	void	Server_ConnectReceive(object sender, string strIP, int nPort, bool IsConnect)
		{
			try
			{
                m_swConnectCheck.Restart();
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
				if (tempIP != m_strIpPort)
				{
					GLog.Add(string.Format("Data From Ohter IP({0})", tempIP));
					return;
				}

				// 받은 data 누적
				byte[]	by_tmp	= new byte[nDataLength];
				Array.Copy(byRcvData, by_tmp, nDataLength);

				m_ltSumRcvData.AddRange(by_tmp);

				while (true)
				{
					System.Threading.Thread.Sleep(1);

					by_rcv_data	= ReadRcvData(m_ltSumRcvData);

					if (by_rcv_data == null)
					{	
						string	str_sum_data	= Encoding.Default.GetString(m_ltSumRcvData.ToArray(), 0, m_ltSumRcvData.Count);
						string	str_rcv_data	= Encoding.Default.GetString(byRcvData, 0, nDataLength);
						GLog.Add(string.Format("ReadRcvData==Null <Sum:{0}><Rcv:{1}>", str_sum_data, str_rcv_data));
						break;
					}
					
					CommInsp.RcvCmd	Cmd			= 0;
					CommInsp.CmdDir	CmdDir		= 0;
					string		strkey		= "";
					tsRcvData	RcvData		= new tsRcvData();

					if (ReceiveData_Parsing(by_rcv_data, by_rcv_data.Length, ref Cmd, ref CmdDir, ref RcvData) == false)
					{
						if (EventTcpDataLog != null)			EventTcpDataLog(string.Format("RCV - ERROR  : {0}", RcvData.strRcvData));
						GLog.Add(string.Format("RCV - ERROR : {0}", RcvData.strRcvData));
						return;
					}

					if (EventTcpDataLog != null)				EventTcpDataLog(string.Format("RCV  : {0}", RcvData.strRcvData));
					
					AddCommLog(RcvData.strRcvData);

					switch (Cmd)
					{
						case CommInsp.RcvCmd.RunStateReport :
						case CommInsp.RcvCmd.InspStateReport :
						case CommInsp.RcvCmd.ProdDataReportConfirm :
						case CommInsp.RcvCmd.AutoRunStartConfirm :
						case CommInsp.RcvCmd.RejectDataReportConfrim :
						case CommInsp.RcvCmd.HomeStartConfirm :
						case CommInsp.RcvCmd.JobCheckReport :
						case CommInsp.RcvCmd.GroupNameReport :
							{
								strkey	= Cmd.ToString();
								lock (m_lockRcv)
								{
									if (m_dicRcvData.ContainsKey(strkey) == true)			m_dicRcvData[strkey]	= RcvData;
									else													m_dicRcvData.Add(strkey, RcvData);
								}
							}
							break;
						case CommInsp.RcvCmd.RunStateRequest :
							{
								m_bIsRecived	= true;
								
								SendRunStateReport();
							}
							break;
						case CommInsp.RcvCmd.ResultDataReport :
							{
								if		(CmdDir == CommInsp.CmdDir.FromTop  &&  SM.SHM.GetUseSkipData(USESKIP.TopInspection) == false)			break;
								else if (CmdDir == CommInsp.CmdDir.FromBtm  &&  SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == false)			break;

								int		nResult;
								if (RcvData.strSplitData.Length < 12)
								{
									G.AlmMng.ADD(ALM.ERROR_COMM_INSP_DATA, string.Format("Data Length Error. {0}", RcvData.strRcvData));
									break;
								}

								for(int i = 0; i < SM.FMP[FlowPart.Insp1].GetMaxProdCount(); i++)
								{
									if (int.TryParse(RcvData.strSplitData[i*3+2], out nResult) == false)
									{
										SendResultDataReportConfirm(CommInsp.Confirm.NG);

										G.AlmMng.ADD(ALM.ERROR_COMM_INSP_DATA, string.Format("Receive : {0}", Encoding.Default.GetString(by_rcv_data)));

										return;
									}

									if (SM.SHM.GetUseSkipData(USESKIP.InspResultAlwaysOK) == true)
									{
										if (CmdDir == CommInsp.CmdDir.FromTop)			SM.FMP.SetInspResult(FlowMngPart.teDir.Top, RcvData.strSplitData[i*3+0], ProdStatus.OK, RcvData.strSplitData[i*3+1]);
										else									SM.FMP.SetInspResult(FlowMngPart.teDir.Btm, RcvData.strSplitData[i*3+0], ProdStatus.OK, RcvData.strSplitData[i*3+1]);
									}
									else
									{
										if (CmdDir == CommInsp.CmdDir.FromTop)			SM.FMP.SetInspResult(FlowMngPart.teDir.Top, RcvData.strSplitData[i*3+0], (ProdStatus)(nResult+1), RcvData.strSplitData[i*3+1]);
										else									SM.FMP.SetInspResult(FlowMngPart.teDir.Btm, RcvData.strSplitData[i*3+0], (ProdStatus)(nResult+1), RcvData.strSplitData[i*3+1]);
									}
									//상부검사 진행 중인데, 바코드 정보가 넘어오지 않으면 알람 처리
									if (CmdDir == CommInsp.CmdDir.FromTop  &&
										SM.SHM.GetUseSkipData(USESKIP.SGEM) == true && 
										string.IsNullOrEmpty(RcvData.strSplitData[i * 3 + 0]) == false)
									{
										string strBarcodeData =  RcvData.strSplitData[i * 3 + 1];
										if (string.IsNullOrEmpty(strBarcodeData))
											G.AlmMng.ADD(ALM.ERROR_COMM_INSP_DATA, string.Format("BARCODE Data is Empty. Please Check the Job"));
										
									}
								}

								SendResultDataReportConfirm(CommInsp.Confirm.OK);
							}
							break;
						case CommInsp.RcvCmd.AlarmReport :
							{
								int nInspAlarmCode = int.Parse(RcvData.strSplitData[0]); 
								ALM eAlarm = ALM.WARNING_AOI_ALARM_THAT_IS_NOT_KNOWN_HAS_OCCURRED_PLEASE_RESTART_TO_RESET_THE_ALARM +  nInspAlarmCode;
								G.AlmMng.ADD(eAlarm);
							}
							break;
						case CommInsp.RcvCmd.VerifyDataReport :
							{
								int		nResult;
								if (int.TryParse(RcvData.strSplitData[1], out nResult) == false)
								{
									SendVerifyDataReportConfirm(CommInsp.Confirm.NG);

									G.AlmMng.ADD(ALM.ERROR_COMM_INSP_DATA, string.Format("Receive : {0}", Encoding.Default.GetString(by_rcv_data)));

									return;
								}

								if (SM.FMT[FlowTray.Reject].SetVerifyResult(RcvData.strSplitData[0], (ProdStatus)nResult+1, RcvData.strSplitData[2]=="T" ? true : false) == true)
								{
									SendVerifyDataReportConfirm(CommInsp.Confirm.OK);
								}
								else
								{
									SendVerifyDataReportConfirm(CommInsp.Confirm.NG);
								}
							}
							break;
						case CommInsp.RcvCmd.VerifyDataReportEnd :
							{
                                NDm.Write(eNDM.RejectRunMode, (int)RejectRunMode.VerifyComplete);

								SendVerifyDataReportEndConfirm(CommInsp.Confirm.OK);
							}
							break;
						case CommInsp.RcvCmd.ProdDataRequest:
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
								string[] strProdID = new string[4] {SM.FMP[curFlow].GetInfo().ProdInfo[0].ProdID, SM.FMP[curFlow].GetInfo().ProdInfo[1].ProdID,
																	SM.FMP[curFlow].GetInfo().ProdInfo[2].ProdID, SM.FMP[curFlow].GetInfo().ProdInfo[3].ProdID};

								string ProgramID = "";
								if (m_CmdDir == CommInsp.CmdDir.ToTop)	ProgramID = SM.FMP[curFlow].GetInfo().ProgramID_Top;
								else										ProgramID = SM.FMP[curFlow].GetInfo().ProgramID_Btm;

								SendProdDataReport(curRailNo, strProdID, ProgramID ,"0" ,"0");
							}
							break;
						case CommInsp.RcvCmd.PauseStart:
							{
								if (SM.SHM.IsBitData(BIT.AutoRun) == true)
								{
									SM.SHM.SetBitData(BIT.Pause, true);
									SM.SHM.MsgShow(SeqMsg.InspPause);
								}

								SendPauseStart(1);
							}
							break;
						case CommInsp.RcvCmd.ReferencePositionRequest:
							{
								if (int.TryParse(RcvData.strSplitData[0], out int nResult))
								{
									if		(CmdDir == CommInsp.CmdDir.FromTop && nResult == 0)			SM.SHM.SetBitData(BIT.ReferenceRequestTop1, true);
									else if (CmdDir == CommInsp.CmdDir.FromTop && nResult == 1)			SM.SHM.SetBitData(BIT.ReferenceRequestTop2, true);
									else if (CmdDir == CommInsp.CmdDir.FromBtm && nResult == 0)			SM.SHM.SetBitData(BIT.ReferenceRequestBtm1, true);
									else if (CmdDir == CommInsp.CmdDir.FromBtm && nResult == 1)			SM.SHM.SetBitData(BIT.ReferenceRequestBtm2, true);
								}
							}
							break;
						case CommInsp.RcvCmd.BoardSignalRequest :
							{ 
								if (int.TryParse(RcvData.strSplitData[0], out int nLane) && int.TryParse(RcvData.strSplitData[1], out int nState))
								{
									bool bState = nState == 1 ? true : false; 
									if		(CmdDir == CommInsp.CmdDir.FromTop && nLane == 0	&&	G.seqInspPlate1.IsGrabPosition() == true)			G.cKernel.AOut(DO.TopInspReady1, bState); //검사 위치에 보드있는지 체크
									else if (CmdDir == CommInsp.CmdDir.FromTop && nLane == 1	&&	G.seqInspPlate2.IsGrabPosition() == true)			G.cKernel.AOut(DO.TopInspReady2, bState);
									else if (CmdDir == CommInsp.CmdDir.FromBtm && nLane == 0	&&	G.seqInspPlate1.IsGrabPosition() == true)			G.cKernel.AOut(DO.BtmInspReady1, bState);
									else if (CmdDir == CommInsp.CmdDir.FromBtm && nLane == 1	&&	G.seqInspPlate2.IsGrabPosition() == true)			G.cKernel.AOut(DO.BtmInspReady2, bState);
								}
							}
							break;
						case CommInsp.RcvCmd.InspStartTimeReport :
							{
								if		(CmdDir == CommInsp.CmdDir.FromTop  &&  SM.SHM.GetUseSkipData(USESKIP.TopInspection) == false)			break;
								else if (CmdDir == CommInsp.CmdDir.FromBtm  &&  SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == false)			break;

								if (RcvData.strSplitData.Length < 8)
								{
									G.AlmMng.ADD(ALM.ERROR_COMM_INSP_DATA, string.Format("Data Length Error. {0}", RcvData.strRcvData));
									break;
								}

								for(int i = 0; i < SM.FMP[FlowPart.Empty].GetMaxProdCount(); i++)
								{
									if (CmdDir == CommInsp.CmdDir.FromTop)		SM.FMP.SetInspTime(FlowMngPart.teDir.Top, RcvData.strSplitData[i*2+0], RcvData.strSplitData[i*2+1]);
									else											SM.FMP.SetInspTime(FlowMngPart.teDir.Btm, RcvData.strSplitData[i*2+0], RcvData.strSplitData[i*2+1]);
								}

								SendInspEndTimeReportConfirm(CommInsp.Confirm.OK);
							}
							break;
					}

					if (m_ltSumRcvData.Count == 0)			break;
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}


		public	int		SendRunStateReport()
		{
			try
			{
				if (IsConnect() == false)			return -1;

				string	strdata	= "0";

				if		(SM.SHM.IsBitData(BIT.AllHoming) == true)		strdata	= ((int)CommInsp.RunState.Init).ToString();
				else if	(SM.SHM.IsBitData(BIT.AutoRun) == true)			strdata	= ((int)CommInsp.RunState.Run).ToString();
				else if (SM.SHM.IsBitData(BIT.Pause) == true)			strdata	= ((int)CommInsp.RunState.Pause).ToString();
				else if (SM.SHM.IsBitData(BIT.Error) == true)			strdata	= ((int)CommInsp.RunState.Alarm).ToString();
				else													strdata	= ((int)CommInsp.RunState.Idle).ToString();


				Send(CommInsp.SendCmd.RunStateReport, strdata);

				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}

		public	int		SendProdDataReport(int nRailNo, string[] ProdID , string Device , string Mark3, string WorkWeek)
		{
			try
			{
				if (IsConnect() == false)			return -1;
				if (ProdID.Length <  SM.FMP[FlowPart.Insp1].GetMaxProdCount())					return -1;

				string	strdata	= string.Format("{0},{1},{2},{3},{4},{5},{6},{7}", nRailNo, ProdID[0], ProdID[1], ProdID[2], ProdID[3], Device, Mark3, WorkWeek);

				ResetRcvData(CommInsp.RcvCmd.ProdDataReportConfirm);
				Send(CommInsp.SendCmd.ProdDataReport, strdata);

				ProdStatus		Result	 = ProdStatus.Unknown;
				for(int i = 0; i < SM.FMP[FlowPart.Insp1].GetMaxProdCount(); i++)
				{
					if (m_result_random.Next(20) == 1)			Result = ProdStatus.NG;
					else										Result = ProdStatus.OK;

					SM.FMP.SetInspResult(FlowMngPart.teDir.Top, ProdID[i], Result, ProdID[i]);

					if (m_result_random.Next(20) == 1)			Result = ProdStatus.NG;
					else										Result = ProdStatus.OK;
					
					SM.FMP.SetInspResult(FlowMngPart.teDir.Btm, ProdID[i], Result, ProdID[i]);
				}

				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}
		
		public	int		SendProdDataReport(int nRailNo, string[] ProdID , string Device)
		{
			try
			{
				if (IsConnect() == false)												return -1;
				if (ProdID.Length < SM.FMP[FlowPart.Empty].GetMaxProdCount())			return -1;

				string	strdata	= string.Format("{0},{1},{2},{3},{4},{5}", nRailNo, ProdID[0], ProdID[1], ProdID[2], ProdID[3], Device);

				Send(CommInsp.SendCmd.ProdDataReport, strdata);

				ProdStatus		Result	 = ProdStatus.Unknown;
				for(int i = 0; i < SM.FMP[FlowPart.Empty].GetMaxProdCount(); i++)
				{
					if (m_result_random.Next(50) == 1)			Result = ProdStatus.NG;
					else										Result = ProdStatus.OK;

					SM.FMP.SetInspResult(FlowMngPart.teDir.Top, ProdID[i], Result, ProdID[i]);

					if (m_result_random.Next(50) == 1)			Result = ProdStatus.NG;
					else										Result = ProdStatus.OK;
					
					SM.FMP.SetInspResult(FlowMngPart.teDir.Btm, ProdID[i], Result, ProdID[i]);
				}

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


		public	int		SendProdDataReport(int nLaneNo, InfoPart.tsInfo PartInfo)
		{
			try
			{
				if (IsConnect() == false)			return -1;

				string ProgramID = "";
				if (m_CmdDir == CommInsp.CmdDir.ToTop)	ProgramID = PartInfo.ProgramID_Top;
				else										ProgramID = PartInfo.ProgramID_Btm;

				string	strdata	= string.Format("{0},{1},{2}", nLaneNo, PartInfo.WorkOrder, ProgramID);
				for (int i = 0; i < PartInfo.ProdInfo.Length; i++)
				{
					strdata	+= string.Format(",{0},{1},{2},{3}", PartInfo.ProdInfo[i].ProdID, PartInfo.ProdInfo[i].TrayID, PartInfo.ProdInfo[i].TrayCol, PartInfo.ProdInfo[i].TrayRow);
				}
				
				ProdStatus		Result	 = ProdStatus.Unknown;
				for(int i = 0; i < SM.FMP[FlowPart.Insp1].GetMaxProdCount(); i++)
				{
					if (m_result_random.Next(50) == 1)			{	Result = ProdStatus.NG; SM.FMP.SetInspResult(FlowMngPart.teDir.Top, PartInfo.ProdInfo[i].ProdID, Result, "");}
					else if (m_result_random.Next(50) == 2)		{	Result = ProdStatus.NG; SM.FMP.SetInspResult(FlowMngPart.teDir.Top, PartInfo.ProdInfo[i].ProdID, Result, "Error");}
					else if (m_result_random.Next(50) == 3)		{	Result = ProdStatus.NG; SM.FMP.SetInspResult(FlowMngPart.teDir.Top, PartInfo.ProdInfo[i].ProdID, Result, "FiduError");}
					else										{	Result = ProdStatus.OK; SM.FMP.SetInspResult(FlowMngPart.teDir.Top, PartInfo.ProdInfo[i].ProdID, Result, "");}
			
					if (m_result_random.Next(50) == 4)			{	Result = ProdStatus.NG; SM.FMP.SetInspResult(FlowMngPart.teDir.Btm, PartInfo.ProdInfo[i].ProdID, Result, "");}
					else if (m_result_random.Next(50) == 5)		{	Result = ProdStatus.NG; SM.FMP.SetInspResult(FlowMngPart.teDir.Btm, PartInfo.ProdInfo[i].ProdID, Result, "Error");}
					else										{	Result = ProdStatus.OK; SM.FMP.SetInspResult(FlowMngPart.teDir.Btm, PartInfo.ProdInfo[i].ProdID, Result, "");}
				}

				ResetRcvData(CommInsp.RcvCmd.ProdDataReportConfirm);
				Send(CommInsp.SendCmd.ProdDataReport, strdata);

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

				Send(CommInsp.SendCmd.RunStateRequest, "");

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
				else if (SM.SHM.GetUseSkipData(USESKIP.InspDebugPlate1) == true)			nDebug	= 2;

				ResetRcvData(CommInsp.RcvCmd.AutoRunStartConfirm);
				Send(CommInsp.SendCmd.AutoRunStart, nDebug.ToString());

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

				Send(CommInsp.SendCmd.InspStateRequest, "");

				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}

		public	int		SendResultDataReportConfirm(CommInsp.Confirm Confirm)
		{
			try
			{
				if (IsConnect() == false)			return -1;

				Send(CommInsp.SendCmd.ResultDataReportConfirm, ((int)Confirm).ToString());

				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}
		
		public	int		SendRejectDataReport(string ProdID, int nRow, int nCol, int nTotalRow,int nTotalCol, MESStatus MesState = 0)
		{
			try
			{
				if (IsConnect() == false)			return -1;

				ResetRcvData(CommInsp.RcvCmd.RejectDataReportConfrim);
				
				int nMesResult =  MesState == MESStatus.OK ? 0 : 1;
				
				Send(CommInsp.SendCmd.RejectDataReport, string.Format("{0},{1},{2},{3},{4}", ProdID, nRow, nCol, nTotalRow, nTotalCol, nMesResult));

				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}

		public	int		SendVerifyDataReportConfirm(CommInsp.Confirm Confirm)
		{
			try
			{
				if (IsConnect() == false)			return -1;

				Send(CommInsp.SendCmd.VerifyDataReportConfirm, ((int)Confirm).ToString());

				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}

		public	int		SendVerifyDataReportEndConfirm(CommInsp.Confirm Confirm)
		{
			try
			{
				if (IsConnect() == false)			return -1;

				Send(CommInsp.SendCmd.VerifyDataEndConfirm, ((int)Confirm).ToString());

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

				Send(CommInsp.SendCmd.HomeStart, "");

				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}
        
		public	int		SendReferenceStart(int Lane)
		{
			try
			{
                
				if (IsConnect() == false)			return -1;
                

                string strData = string.Format("{0}", Lane.ToString());
				Send(CommInsp.SendCmd.ReferenceStart, strData);

				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}
        
		public	int		SendPauseStart(int nData)
		{
			try
			{
				if (IsConnect() == false)			return -1;

				Send(CommInsp.SendCmd.PauseStart, nData.ToString());

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
				if (IsConnect() == false)			return -1;

				Send(CommInsp.SendCmd.AlarmClear, "");

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
                
				Send(CommInsp.SendCmd.AlarmReport, Alarm.ToString());

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

				Send(CommInsp.SendCmd.YieldClear, "");

				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}

		public	int		SendLotStart()
		{
			try
			{
				if (IsConnect() == false)			return -1;

				Send(CommInsp.SendCmd.LotStartReport, "");

				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}

		public	int		SendLotEnd()
		{
			try
			{
				if (IsConnect() == false)			return -1;

				Send(CommInsp.SendCmd.LotEndReport, "");

				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}

		public	int		SendInspEndTimeReportConfirm(CommInsp.Confirm Confirm)
		{
			try
			{
				if (IsConnect() == false)			return -1;

				Send(CommInsp.SendCmd.InspStartTimeReportConfirm, ((int)Confirm).ToString());

				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}

        public  int     SendJobCheckRequest(string JobName)
        {
            try
			{
				if (IsConnect() == false)			return -1;

				Send(CommInsp.SendCmd.JobCheckRequest, JobName);

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

				ResetRcvData(CommInsp.RcvCmd.JobUpdateDataReport);
				Send(CommInsp.SendCmd.JobUpdateDataRequest, "");

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

				ResetRcvData(CommInsp.RcvCmd.JobUpdateReport);
				Send(CommInsp.SendCmd.JobUpdateRequest, "");

                return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}

		//head(0:1번헤드/ 1: 2번헤드) , JigType(0:조명/ 1:3D)
		public  int		SendMoveSetPosRequest(int nHead, int nJigType)
		{
			try
			{
				if (IsConnect() == false)			return -1;

				string strMsg = $"{nHead.ToString()}, {nJigType.ToString()}";

				ResetRcvData(CommInsp.RcvCmd.JobUpdateReport);
				Send(CommInsp.SendCmd.MoveSetPosRequest, strMsg);

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
				
                Send(CommInsp.SendCmd.PopupMsgReq, msg);

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
				
                Send(CommInsp.SendCmd.PopupMsgAllCloseReq, "");

				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}

		public  int		SendGroupNameRequest()
		{
			try
			{
				if (IsConnect() == false)			return -1;

				ResetRcvData(CommInsp.RcvCmd.GroupNameReport);
				Send(CommInsp.SendCmd.GroupNameRequest, "");

				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}

		
		public  int		SendUserChangeRequest(string strID, int nLevel)
		{
			try
			{
				if (IsConnect() == false)			return -1;
				if (nLevel > 2) nLevel = 2;
				
				string strMsg = $"{strID}, {nLevel.ToString()}";
				Send(CommInsp.SendCmd.UserChangeRequest, strMsg);

				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}

        
		public	int		RcvRunStateReport(ref CommInsp.RunState RunState)
		{
			try
			{
				RunState	= G.Simul.RunState;;
				
				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}

		public	int		RcvInspStateReport(ref CommInsp.InspState InspState)
		{
			try
			{
				InspState	= G.Simul.m_InspState;

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
				bConfirmOK	= true;

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
				bConfirmOK	= true;

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
				bConfirmOK	= true;
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
				bConfirmOK	= true;
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
				bState	= true;
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
				strGroupName	= "SimGroup";
				
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
				bResultOK	= true;
				
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
				bResultOK	= true;
				
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
				bResultOK	= true;
				
				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}

		public	void	ResetRcvData(CommInsp.RcvCmd Cmd)
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
		

		
		private bool	Send(CommInsp.SendCmd teCmd, string strData)
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

							str = ((CommInsp.RunState)int.Parse(str)).ToString();
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
							str = ((CommInsp.InspState)int.Parse(str)).ToString();
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
							str = ((CommInsp.Confirm)int.Parse(str)).ToString();
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
									strtemp[i] = ((CommInsp.Confirm)int.Parse(strtemp[i])).ToString();
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
									strtemp[i] = ((CommInsp.Confirm)int.Parse(strtemp[i])).ToString();
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

		private	void	GetIPPort(ref string strIP, ref int nPort)
		{
			try
			{
				string[] strSplit	= m_strIpPort.Split(':');

				strIP		= strSplit[0];
				nPort		= int.Parse(strSplit[1]);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private CommInsp.CmdDir	GetFromComDir(CommInsp.CmdDir ToComDir)
		{
			try
			{
				switch (ToComDir)
				{
					case CommInsp.CmdDir.ToTop :			return CommInsp.CmdDir.FromTop;
					case CommInsp.CmdDir.ToBtm :			return CommInsp.CmdDir.FromBtm;
					case CommInsp.CmdDir.ToReview :		return CommInsp.CmdDir.FromReview;
				}

				return CommInsp.CmdDir.Unknown;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return CommInsp.CmdDir.Unknown;
			}
		}

		private CommInsp.CmdDir	GetToComDir(CommInsp.CmdDir FromComDir)
		{
			try
			{
				switch (FromComDir)
				{
					case CommInsp.CmdDir.FromTop :			return CommInsp.CmdDir.ToTop;
					case CommInsp.CmdDir.FromBtm :			return CommInsp.CmdDir.ToBtm;
					case CommInsp.CmdDir.FromReview :			return CommInsp.CmdDir.ToReview;
				}

				return CommInsp.CmdDir.Unknown;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return CommInsp.CmdDir.Unknown;
			}
		}




		private bool	ReceiveData_Parsing(byte[] byRcvData, int nRcvDataLength, ref CommInsp.RcvCmd Cmd, ref CommInsp.CmdDir CmdDir, ref tsRcvData RcvData)
		{
			try
			{
				int		nLength		= 0;
				string	strDataTmp	= "";
				string	strRcvData	= Encoding.Default.GetString(byRcvData);

				Cmd			= (CommInsp.RcvCmd)byRcvData[1];
				CmdDir		= (CommInsp.CmdDir)byRcvData[2];
				nLength		= (int)byRcvData[3];
				strDataTmp	= Encoding.Default.GetString(byRcvData, 4, nRcvDataLength-5);

				if (nLength != strDataTmp.Length)
				{
					G.AlmMng.ADD(ALM.ERROR_COMM_INSP_DATA, string.Format("Data Length Error. {0}", strRcvData));

					return false;
				}
				
				RcvData.RcvCmd			= Cmd;
				RcvData.CmdDir			= CmdDir;
				RcvData.strRcvData		= string.Format("<STX><{0}><{1}><{2}><{3}><ETX>", Cmd, CmdDir, nLength, strDataTmp);
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
							ltRcvData.RemoveRange(0, nIdxStx);
							continue;
						}

						int	nidx_stx	= ltRcvData.IndexOf(STX, nIdxStx+1, nIdxEtx-(nIdxStx+1));
						if (nidx_stx >= 0)
						{
							ltRcvData.RemoveRange(0, nidx_stx);
							continue;
						}

						break;
					}
					else
					{
						return false;
					}
				}

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
			byte[] by_rcv_data	= null;
			try
			{
				int	nidx_stx	= 0;
				int	nidx_etx	= 0;
				
				
				if (IndexOfStxEtx(ltRcvData, ref nidx_stx, ref nidx_etx) == false)
				{
					//메모리 누수 체크, STX와 ETX가 없는 데이터가 특정 이상 쌓이는 경우
					if (ltRcvData.Count > 10000) // 예: 10,000 바이트 이상이면 경고
					{
						GLog.Add($"Warning: ltRcvData buffer size exceeded limit! Size: {ltRcvData.Count}");
					}
					return null;
				}

				int	ncnt	= nidx_etx - nidx_stx + 1;

				by_rcv_data	= ltRcvData.GetRange(nidx_stx, ncnt).ToArray();
				
				ltRcvData.RemoveRange(nidx_stx, ncnt);
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

	}
}
