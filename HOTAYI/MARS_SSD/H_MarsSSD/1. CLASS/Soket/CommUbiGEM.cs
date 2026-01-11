using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UbiCom.Net.Structure;
using UbiGEM.Net.Structure;
using static H_MarsSSD.CommInsp;

namespace H_MarsSSD
{
	public class CommUbiGEM : UbiGEMBaseClass
	{
		public	delegate	void	UBIGEMDataLogEventHandler(string strData);
		public	event				UBIGEMDataLogEventHandler EventUBIGEMDataLog;

		#region variable
		PStopwatch		m_TimeoutMES	= new PStopwatch();
		bool			m_bSend			= false;
		ProcessState	m_BeforeState	= H_MarsSSD.ProcessState.UNKNOWN;
		#endregion

		public CommUbiGEM(string _ugcFileName = "")
		{
			try
			{
				m_ugcFilePath = Path.GetFullPath(Program.DataDirectory + _ugcFileName);
				base.ReceiveInfo += new ReceiveRemoteInfoHandler(OnReceiveRemoteInfo);
				base.ConnectComplete += new CommunicationCompletedHandler(OnCommunicationCompleted);
				base.TerminalTextReceive += new TerminalTextReceiveHandler(OnTerminalTextReceive);
				base.AckReceive += new AckReceiveHandler(OnAck6Receive);
				base.ControlStateChanged += new ControlStateChangedHandler(OnControlStateChanged);
				base.SecondaryMessageReceive += new SecondaryMessageReceiveHandler(OnReceiveSecondaryMessage); //CYJ 이번에 추가한거
				
				Initialize();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
			
		}
		
		#region [initialize, start, stop]
		public void Initialize()
		{
			try
			{
				Init();

			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public void InitGEM()
		{
			GEMInit();
			SM.SHM.SetBitData(BIT.UBIGEM_PP_SELECT, false);
			SM.SHM.SetBitData(BIT.UBIGEM_SENDWORKINFO, false);
			SM.SHM.SetBitData(BIT.UBIGEM_PROCESS_STATE_CHANGE, false);

			Start();
		}

		public void Start()
		{
			GEMStart();
		}

		public void Stop()
		{
			GEMStop();
		}

		public void Dispose()
		{
			GEMStop();
            UBIGEMDispose();
		}
		
		private void SetProcessState(ProcessState State)
		{
			m_BeforeState = State;
			m_TimeoutMES.Restart();
			m_bSend = true;
		}

		public void Update()
		{
			try
			{
				if (SM.SHM.GetUseSkipData(USESKIP.SGEM) == false)	return;
				if (SM.SHM.IsBitData(BIT.UbiGemConnect) == false)
				{
					SetProcessState(H_MarsSSD.ProcessState.UNKNOWN);
					return;
				}

				if (m_BeforeState == H_MarsSSD.ProcessState.UNKNOWN)
				{
					//프로그램 실행시
					G.commUbiGEM.SendProcessStateChanged((int)H_MarsSSD.ProcessState.INIT);
					SetProcessState(H_MarsSSD.ProcessState.INIT);
				}
				else if (SM.SHM.IsBitData(BIT.AutoRun) == true)
				{
					//런중에, 자재가 있는 경우
					if (SM.FMT.IsFlowRange(FlowTray.LDLift01, FlowTray.ULDLift31))
					{
						if (m_BeforeState != H_MarsSSD.ProcessState.EXECUTING)
						{
							G.commUbiGEM.SendProcessStateChanged((int)H_MarsSSD.ProcessState.EXECUTING);
							SetProcessState(H_MarsSSD.ProcessState.EXECUTING);
						}
					}
					//런중에, 자재가 없는 경우
					else 
					{
						if (m_BeforeState != H_MarsSSD.ProcessState.READY)
						{
							G.commUbiGEM.SendProcessStateChanged((int)H_MarsSSD.ProcessState.READY);
							SetProcessState(H_MarsSSD.ProcessState.READY);
						}
					}
				}
				else
				{
					if (SM.SHM.IsBitData(BIT.Pause) == true)
					{
						if (m_BeforeState != H_MarsSSD.ProcessState.PAUSE)
						{
							//일시정지를 누른 경우
							G.commUbiGEM.SendProcessStateChanged((int)H_MarsSSD.ProcessState.PAUSE);
							SetProcessState(H_MarsSSD.ProcessState.PAUSE);
						}
					}
					else if (m_BeforeState != H_MarsSSD.ProcessState.IDLE)
					{
						//키고 동작 전 또는 정지 버튼을 누른 경우
						G.commUbiGEM.SendProcessStateChanged((int)H_MarsSSD.ProcessState.IDLE);
						SetProcessState(H_MarsSSD.ProcessState.IDLE);
					}
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}

		}

		#endregion

		#region [Variable]
		
		//Check: 차후 각각의 RCMD로 받은 데이터를 저장하는 Dictionary 변수 추가
		public Dictionary<string, string>	PP_SELECT_RCVData		= new Dictionary<string, string>();
		public Dictionary<string, string>	START_RCVData			= new Dictionary<string, string>();
		public Dictionary<string, string>	STOP_RCVData			= new Dictionary<string, string>();
		public Dictionary<string, string>	REMOTE_RCVData			= new Dictionary<string, string>();
		public Dictionary<string, string>	LOCAL_RCVData			= new Dictionary<string, string>();
		public Dictionary<string, string>	SENDWORKINFO_RCVData	= new Dictionary<string, string>();
		public Dictionary<string, string>	INSP_RESULT_CONFIRMData	= new Dictionary<string, string>();
		public Dictionary<string, int>		ACK6_RCVData			= new Dictionary<string, int>();
		public Dictionary<string, int>		ACK5_RCVData			= new Dictionary<string, int>();
		private List<string> m_lstTerminalString = new List<string>();
		
		private string m_TrayID;
		private string m_LidID;
		private string m_WorkOrder;
		private string m_PPID;

		public byte ProcessState				{ get => m_bProcessState; set => m_bProcessState = value; }

		public List<string> LstTerminalString	{ get => m_lstTerminalString; }
		#endregion

		#region [Event]
		public void OnReceiveRemoteInfo(object sender, RemoteCommandInfo data)
		{
			try
			{
				string s_commandName = data.RemoteCommand;
				List<CommandParameterInfo>		infoList	= data.CommandParameter.Items;
				RemoteCommandResult				result		= new RemoteCommandResult();
				RemoteCommandParameterResult	paramResult;
				CPACK	Ack = CPACK.NoError;
                REP		cmd;
				string	logText;

				foreach (CommandParameterInfo paramInfo in data.CommandParameter.Items)
				{
					paramResult = new RemoteCommandParameterResult(paramInfo.Name, (int)CPACK.IllegalFormatSpecifiedForCPVAL);
					result.Items.Add(paramResult);
				}

                logText = $"{Environment.NewLine}    [RCV][RCMD : {data.RemoteCommand}]{Environment.NewLine}    ";
                foreach (CommandParameterInfo paramInfo in data.CommandParameter.Items)
                {
                    logText += $"{paramInfo.Value}, {paramInfo.Name}({paramInfo.Format}){Environment.NewLine}    ";
                }
                //GLog.AddUbi(logText);

				if (EventUBIGEMDataLog != null)			EventUBIGEMDataLog(logText);

				if (Enum.TryParse(s_commandName, out cmd) == false)
				{
					// 알람을 적어야 함
					return;
				}

				switch (cmd)
				{
					case REP.PP_SELECT:
						{
							CPNAME_PP_SELECT repDataEnum;
                            PP_SELECT_RCVData.Clear();
							for (int i = 0; i < infoList.Count; i++)
							{
								if (Enum.TryParse(infoList[i].Name, out repDataEnum) == false)
								{	// 알람을 적어야 함
									result.HostCommandAck = (int)CPACK.IllegalFormatSpecifiedForCPVAL; 
									m_gemDriver.ReplyRemoteCommandAck(data, result); 
									return;
								}
								else
								{
									PP_SELECT_RCVData.Add(infoList[i].Name, infoList[i].Value.ToString());
									//if (EventUBIGEMDataLog != null)			EventUBIGEMDataLog("[" + RepDataDescription + "]: " + FVIPGMREP_Data[RepDataDescription]);
								}
							}
                            GLog.Add("Receive SECS/GEM - PP_SELECT");


                            SM.SHM.SetBitData(BIT.UBIGEM_PP_SELECT, true); //Check : 변경이 필요 합니다.
						}
						break;
					case REP.START:
						{
							//Check : 협의 필요
						}
						break;
					case REP.STOP:
						{
							//Check : 협의 필요
						}
						break;
					case REP.REMOTE:
						{
							SendEquipmentOnlineRemoteReq();
							GLog.Add("Receive SECS/GEM - REMOTE");
						}
						break;
					case REP.LOCAL:
						{
							SendEquipmentOnlineLocalReq();
							GLog.Add("Receive SECS/GEM - LOCAL");
						}
						break;
					case REP.SENDWORKINFO:
						{
							if (SM.SHM.GetUseSkipData(USESKIP.SGEM) == false)
							{
								GLog.Add($"CHECK USK/SKIP STETING - SECS/GEM NOT USE ");
								G.AlmMng.ADD(ALM.ERROR_SESCGEM_DATA);
								result.HostCommandAck = (int)CPACK.OtherEquipmentspecificError;
								m_gemDriver.ReplyRemoteCommandAck(data, result);
								return;
							}

							CPNAME_SENDWORKINFO repDataEnum;
							SENDWORKINFO_RCVData.Clear();
							
							if (G.seqTrayLoader.IsReady == false  || SM.FMT.IsFlowRange(FlowTray.LDLift01, FlowTray.LDLift31) == true)
							{
								GLog.Add($"MES - SENDWORKINFO_RCVData : seqTrayLoader.IsReady = false or  SM.FMT.IsFlowRange = true ");
								G.AlmMng.ADD(ALM.ERROR_SESCGEM_DATA);
								result.HostCommandAck = (int)CPACK.OtherEquipmentspecificError;
								m_gemDriver.ReplyRemoteCommandAck(data, result);
								return;
							}

					
							for (int i = 0; i < infoList.Count; i++)
							{
								if (Enum.TryParse(infoList[i].Name, out repDataEnum) == false)
								{	
									GLog.Add($"MES - SENDWORKINFO_RCVData(Enum.TryParse) : {infoList[i].Name} ");
									G.AlmMng.ADD(ALM.ERROR_SESCGEM_DATA);
									result.HostCommandAck = (int)CPACK.IllegalFormatSpecifiedForCPVAL; 
									m_gemDriver.ReplyRemoteCommandAck(data, result); 
									return;
								}
								else
								{
									switch (repDataEnum)
									{
										case CPNAME_SENDWORKINFO.WO:			SDm.Write(eSDM.WO, infoList[i].Value);					break;
										case CPNAME_SENDWORKINFO.PPID_TOP:	SDm.Write(eSDM.PROGRAMID_TOP, infoList[i].Value);		break;
										case CPNAME_SENDWORKINFO.PPID_BOTTOM:	SDm.Write(eSDM.PROGRAMID_BOTTOM, infoList[i].Value);	break;
										case CPNAME_SENDWORKINFO.TrayID:		SDm.Write(eSDM.TRAYID, infoList[i].Value);				break;
										case CPNAME_SENDWORKINFO.LidID:		SDm.Write(eSDM.LIDID, infoList[i].Value);				break;
										case CPNAME_SENDWORKINFO.HasLid:		SDm.Write(eSDM.HASLID, infoList[i].Value);				break;
										case CPNAME_SENDWORKINFO.Column:
											{
												if (int.TryParse(infoList[i].Value, out int nResult ) ==  false || nResult != 25)
												{
													GLog.Add($"SENDWORKINFO ERROR - Column Value Is Invalid (Column : {infoList[i].Value})");
													SDm.Write(eSDM.WO, ""); //WO 만 공백처리하면 frmBarcode 에서 업데이트 하지 않음.
													Ack = CPACK.IllegalValueSpecifiedForCPVAL;
												}
												SDm.Write(eSDM.COLUMN, infoList[i].Value);
											}		
											break;
										case CPNAME_SENDWORKINFO.Row:
											{
												if (int.TryParse(infoList[i].Value, out int nResult ) ==  false || (nResult != 2 &&nResult != 3) ) 
												{
													GLog.Add($"SENDWORKINFO ERROR - Row Value Is Invalid (Row : {infoList[i].Value})");
													SDm.Write(eSDM.WO, ""); //WO 만 공백처리하면 frmBarcode 에서 업데이트 하지 않음.
													Ack = CPACK.IllegalValueSpecifiedForCPVAL;
												}
												SDm.Write(eSDM.ROW, infoList[i].Value);
											}
											break;
										case CPNAME_SENDWORKINFO.TotalQty:
											{
												if (int.TryParse(infoList[i].Value, out int nResult) == false ||
													( nResult > 700  && SDm.Read(eSDM.ROW) == "2")  ||
													( nResult > 1050  && SDm.Read(eSDM.ROW) == "3") )
												{
													GLog.Add($"SENDWORKINFO ERROR - The value of Total Qty is too high (TotalQty : {infoList[i].Value})");
													SDm.Write(eSDM.WO, ""); //WO 만 공백처리하면 frmBarcode 에서 업데이트 하지 않음.
													Ack = CPACK.IllegalValueSpecifiedForCPVAL;
												}

												SDm.Write(eSDM.TOTALQTY, infoList[i].Value);
											}
											break;
										case CPNAME_SENDWORKINFO.FormFactor:
											{
												if (infoList[i].Value != "RDIMM" && infoList[i].Value != "SODIMM" && infoList[i].Value != "UDIMM"  )
												{	
													GLog.Add($"SENDWORKINFO ERROR - FormFactor Value Is Invalid (FormFactor : {infoList[i].Value})");
													SDm.Write(eSDM.WO, ""); //WO 만 공백처리하면 frmBarcode 에서 업데이트 하지 않음.
													Ack = CPACK.IllegalValueSpecifiedForCPVAL;
												}
												SDm.Write(eSDM.FORMFACTOR, infoList[i].Value);
											}
											break;
										case CPNAME_SENDWORKINFO.DDRGeneration:
											{ 
												if (infoList[i].Value != "DDR4" && infoList[i].Value != "DDR5"  )
												{	
													GLog.Add($"SENDWORKINFO ERROR - DDRGeneration Value Is Invalid (DDRGeneration : {infoList[i].Value})");
													SDm.Write(eSDM.WO, ""); //WO 만 공백처리하면 frmBarcode 에서 업데이트 하지 않음.
													Ack = CPACK.IllegalValueSpecifiedForCPVAL;
												}
												SDm.Write(eSDM.DDRGENERATION, infoList[i].Value);
											}
											break;

									}

									if (SM.FMT.IsFlow(FlowTray.LDWaitBuf) == false) SM.FMT.Next(FlowTray.Empty, FlowTray.LDWaitBuf);

								
									SENDWORKINFO_RCVData.Add(infoList[i].Name, infoList[i].Value.ToString());
									if (EventUBIGEMDataLog != null)			EventUBIGEMDataLog("Name: " + infoList[i].Name + ", VALUE: " + infoList[i].Value.ToString());
								}
							}
							InfoTray.tsInfo info_t = SM.FMT[FlowTray.LDWaitBuf].GetInfo();
							info_t.WorkOrder = SDm.Read(eSDM.WO);
							info_t.ProgramID_Top = SDm.Read(eSDM.PROGRAMID_TOP);
							info_t.ProgramID_Btm = SDm.Read(eSDM.PROGRAMID_BOTTOM);
							info_t.TrayBarcode = SDm.Read(eSDM.TRAYID);
							info_t.ColCnt = int.Parse(SDm.Read(eSDM.COLUMN));
							info_t.RowCnt = int.Parse(SDm.Read(eSDM.ROW));
							info_t.MaxCntProdInLot = int.Parse(SDm.Read(eSDM.TOTALQTY));
							info_t.ModuleGen = SDm.Read(eSDM.DDRGENERATION);
							info_t.ModuleType = SDm.Read(eSDM.FORMFACTOR);
							SM.FMT[FlowTray.LDWaitBuf].SetInfo(info_t);

							SDm.Write(eSDM.WO, "");
							SDm.Write(eSDM.PROGRAMID_TOP, "");
							SDm.Write(eSDM.PROGRAMID_BOTTOM, "");
							SDm.Write(eSDM.TRAYID, "");
							SDm.Write(eSDM.LIDID, "");
							SDm.Write(eSDM.HASLID, "");
							SDm.Write(eSDM.COLUMN, "");
							SDm.Write(eSDM.ROW, "");
							SDm.Write(eSDM.TOTALQTY, "");
							SDm.Write(eSDM.FORMFACTOR, "");
							SDm.Write(eSDM.DDRGENERATION, "");

							SM.SHM.SetBitData(BIT.UBIGEM_SENDWORKINFO, true);
						}
						break;
					case REP.INSP_RESULT_CONFIRM:
						{
							CPNAME_INSP_RESULT_CONFIRM repDataEnum;
							INSP_RESULT_CONFIRMData.Clear();

							if (infoList.Count != 24)
							{
								GLog.Add($"MES - Insp Result Confirm Count Error  : Received Count : {infoList.Count} ");
								Ack= CPACK.IllegalFormatSpecifiedForCPVAL;
								G.AlmMng.ADD(ALM.ERROR_SESCGEM_DATA);
								result.HostCommandAck = (int)Ack;
								m_gemDriver.ReplyRemoteCommandAck(data, result); 
								return;
							}
							for (int i = 0; i < infoList.Count; i++)
							{
								if (Enum.TryParse(infoList[i].Name, out repDataEnum) == false)
								{	
									GLog.Add($"MES - INSP_RESULT_CONFIRMData(Enum.TryParse) : {infoList[i].Name} ");
									Ack= CPACK.IllegalFormatSpecifiedForCPVAL;
									G.AlmMng.ADD(ALM.ERROR_SESCGEM_DATA);
									result.HostCommandAck = (int)Ack;
									m_gemDriver.ReplyRemoteCommandAck(data, result); 
									return;
								}
								else
								{
									if (INSP_RESULT_CONFIRMData.ContainsKey(infoList[i].Name) == false)		INSP_RESULT_CONFIRMData.Add(infoList[i].Name, infoList[i].Value.ToString());
									else																	INSP_RESULT_CONFIRMData[infoList[i].Name] =  infoList[i].Value.ToString();
									
									if (EventUBIGEMDataLog != null)			EventUBIGEMDataLog("Name: " + infoList[i].Name + ", VALUE: " + infoList[i].Value.ToString());
								}	
							}
							

						for (int i = 0; i < SM.FMP[FlowPart.Empty].GetMaxProdCount(); i++)
							{
								if (INSP_RESULT_CONFIRMData.ContainsKey((CPNAME_INSP_RESULT_CONFIRM.SN1 + (i * 3)).ToString()) == true)
								{
									string strSerial = INSP_RESULT_CONFIRMData[(CPNAME_INSP_RESULT_CONFIRM.SN1 + (i * 3)).ToString()];
									if (string.IsNullOrEmpty(strSerial) == true) continue;
									string strMESResult = INSP_RESULT_CONFIRMData[(CPNAME_INSP_RESULT_CONFIRM.RESULT1 + (i * 3)).ToString()];
									string strRemark = INSP_RESULT_CONFIRMData[(CPNAME_INSP_RESULT_CONFIRM.REMARK1 + (i * 3)).ToString()];
									//CYJ Remark가 공백이나 Null로 왔을때 확인 필요

									if (SM.FMP.SetMesResult(strSerial, int.Parse(strMESResult)) == false)
									{
										GLog.Add($"MES - INSP_RESULT_CONFIRMData(SetMesResult) : The serial({strSerial}) could not be found. ");
										Ack = CPACK.OtherEquipmentspecificError;
										G.AlmMng.ADD(ALM.ERROR_SESCGEM_DATA);
									}

									if (int.Parse(strMESResult) != (int)MESStatus.OK)
									{
										G.AlmMng.ADD(ALM.WARNING_SESCGEM_INPSECTION_RESULT_CONFIRM_SECSGEM_NG);
										string strMsg = $"MES NG OCCURRED {Environment.NewLine}" + 
														$"REMARK : {strRemark} {Environment.NewLine} " +
														$"SERIAL NUM : {strSerial}"  ;
										UI.Logs.Write(LOGPART.WARNING, "MES NG OCCURRED ");
										SM.SHM.SetMsg(SeqMsg.MESNG, strMsg);
										SM.SHM.MsgShow(SeqMsg.MESNG);
									}
								}
							}
							SM.SHM.SetBitData(BIT.UBIGEM_INSP_RESULT_CONFIRM, true);
						}
						break;
				}
				GLog.Add($"Rcv SECS/GEM - {cmd.ToString()}");
				GLog.Add($"Secs/Gem TEST Ack : {(int)Ack}");
				result.HostCommandAck = (int)Ack; // 0 값으로 들어 옴
				// 만약 알람이라면 HostCommAck를 다른 값으로 변경 해야 함 

				m_gemDriver.ReplyRemoteCommandAck(data, result); 
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		//CYJ 2024/11/06
		//primaryMessage 우리쪽에서 보낸 정보 예로들어 S5F1을 보냈으면 그에 관한 S5F1의 정보가 전부 온다
		//secondaryMessage 우리쪽에서 받는 정보 예로들어 S5F1을 보냈으면 그에 관한 S5F2의 정보가 전부 온다
		public void OnReceiveSecondaryMessage(SECSMessage primaryMessage, SECSMessage secondaryMessage)
		{
			try
			{
				string sf = "S" + secondaryMessage.Stream.ToString() + "F" +  secondaryMessage.Function.ToString();

				switch(sf)
				{
					case "S5F2":
						{
							int ack = secondaryMessage.Body.Item[0].Value;
							//ACK_RCVData.Add(REQ.AlarmSendReport.ToString() ,ack); 
							string result = "";
							if (primaryMessage.Body.AsList[1].Value == 128)				result = "1011";
							else if (primaryMessage.Body.AsList[1].Value == 0)		result = "1012";
							else														result = REQ.UNKWON.ToString();

							if (ACK5_RCVData.ContainsKey(result.ToString()) == false)		ACK5_RCVData.Add(result.ToString(), ack);
							else															ACK5_RCVData[result.ToString()] = ack;
							// 원하시는 알람이나 다른 처리를 하시면 될것 같습니다.
						}
						break;
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public void OnCommunicationCompleted(bool IsCompplted)
		{
            SM.SHM.SetBitData(BIT.UbiGemConnect, IsCompplted);
			if (NDm.Read(eNDM.RecentlyAlarmCode) != 0)
			{
				G.commUbiGEM.SendAlarmClear((ALM)NDm.Read(eNDM.RecentlyAlarmCode));
				NDm.Write(eNDM.RecentlyAlarmCode, 0);
			}
		}

		public void OnTerminalTextReceive(string terminalText)
		{
			try
			{
				// event로 terminalText를 받을 때마다 UI에 누적해서 뿌려주면 된다.
				int keyCount = UI.TerminalMessageBox.GetCurTerminalKeyCount();
				UI.TerminalMessageBox.SetMessage(SeqMsg.Terminal1 + keyCount, terminalText);
				UI.TerminalMessageBox.Show(SeqMsg.Terminal1 + keyCount);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
		
		public void OnAck6Receive(string ceid, int ack)
		{
			if (ACK6_RCVData.ContainsKey(ceid) == false)		ACK6_RCVData.Add(ceid, ack);
			else												ACK6_RCVData[ceid] = ack;
		}

		public void OnControlStateChanged(ControlState controlState)
		{
			SendControlStateChanged(controlState);
		}
		#endregion

		#region [Offline, Online Local, Online Remote]
		
		// 나중에 필요하면 넣어주세요
		public void SendEquipmentOfflineReq()
		{
			SetControlState(ControlState.EquipmentOffline);
		}

		public void SendEquipmentOnlineLocalReq()
		{
			SetControlState(ControlState.OnlineLocal);
		}

		public void SendEquipmentOnlineRemoteReq()
		{
			SetControlState(ControlState.OnlineRemote);
		}

		#endregion

		#region [Send Function]
		public bool SendOpenLidRequest(string TrayID, string LidID)
		{
			try
			{
				if (m_bConnect == false)			return false;

				CollectionEventInfo ceInfo = new CollectionEventInfo();
				if (CE["1016"].Name != EnumHelper.GetDescription(REQ.OpenLidRequest)) return false;

				//// 예제에서 직접 호출해서 저장하는 것을 피해 달라고 함 
				//// EAPEIS_TAMSF_FVI_START_REQ의 ECID: "14", 해당되는 REPORT의 RPTID: "6";

				//Check : 데이터 입력 필요ㄴ
				ceInfo = CE["1016"];
				ceInfo.Reports.Items["1016"].Variables.Items[0].Value = TrayID;// TrayID
				ceInfo.Reports.Items["1016"].Variables.Items[1].Value = LidID; // LidID
				
				if (ACK6_RCVData.ContainsKey("1016") == false)	ACK6_RCVData.Add("1016", -1);
				else											ACK6_RCVData["1016"] = -1;
                //WriteSendLog(ceInfo, REQ.EAPEIS_TAMSF_FVI_END_REQ);
				GemDriverError driveResult = m_gemDriver.ReportCollectionEvent(ceInfo);
                
                GLog.Add("Send SECS/GEM - SendOpenReq");
				
				if(driveResult != GemDriverError.Ok)
				{
					G.AlmMng.ADD(ALM.ERROR_SESCGEM_DRIVER, driveResult.ToString());
					return false;
				}

                //check:ubiTEST

                return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public bool SendInspectionRequest(string wo, string sn)
		{
			try
			{
				if (m_bConnect == false)			return false;

				CollectionEventInfo ceInfo = new CollectionEventInfo();
				if (CE["1017"].Name != EnumHelper.GetDescription(REQ.InspectionRequest)) return false;

				//// 예제에서 직접 호출해서 저장하는 것을 피해 달라고 함 
				//// EAPEIS_TAMSF_FVI_START_REQ의 ECID: "14", 해당되는 REPORT의 RPTID: "6";

				//Check : 데이터 입력 필요 
				ceInfo = CE["1017"];
				ceInfo.Reports.Items["1017"].Variables.Items[0].Value = wo;// Work Order
				ceInfo.Reports.Items["1017"].Variables.Items[1].Value = sn; // Serial Number
				
				if (ACK6_RCVData.ContainsKey("1017") == false)	ACK6_RCVData.Add("1017", -1);
				else											ACK6_RCVData["1017"] = -1;
                //WriteSendLog(ceInfo, REQ.EAPEIS_TAMSF_FVI_END_REQ);
				GemDriverError driveResult = m_gemDriver.ReportCollectionEvent(ceInfo);
                
                GLog.Add("Send SECS/GEM - SendInspectionReques");
				
				if(driveResult != GemDriverError.Ok)
				{
					G.AlmMng.ADD(ALM.ERROR_SESCGEM_DRIVER, driveResult.ToString());
					return false;
				}

                //check:ubiTEST

                return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}
		
		public bool SendInspectionResult(FlowPart resultPart)
		{
			try
			{
				if (m_bConnect == false)						return false;
				if (SM.FMP.IsFlow(resultPart) == false)			return false;

					CollectionEventInfo ceInfo = new CollectionEventInfo();
				if (CE["1018"].Name != EnumHelper.GetDescription(REQ.InspectionResult)) return false;

				//// 예제에서 직접 호출해서 저장하는 것을 피해 달라고 함 
				//// EAPEIS_TAMSF_FVI_START_REQ의 ECID: "14", 해당되는 REPORT의 RPTID: "6";

				//Check : 데이터 입력 필요
				ceInfo = CE["1018"];
				ceInfo.Reports.Items["1018"].Variables.Items[0].Value = SM.FMP[resultPart].GetInfo().WorkOrder;// workorder
				ceInfo.Reports.Items["1018"].Variables.Items[1].Value = SM.FMP[resultPart].GetInfo().ProgramID_Top; // Process Program ID
				ceInfo.Reports.Items["1018"].Variables.Items[2].Value = SM.FMP[resultPart].GetInfo().ProgramID_Btm; // Process Program ID
				
				ceInfo.Reports.Items["1018"].Variables.Items[3].Value = ceInfo.Reports.Items["1018"].Variables.Items[3].Default;
				VariableInfo resultList = ceInfo.Reports.Items["1018"].Variables.Items[3];
			
				VariableInfo dataInfo;
				resultList.ChildVariables.Clear();
				for (int i = 0; i < SM.FMP[resultPart].GetInfo().ProdInfo.Length; i++)
				{
					//피듀셜, 바코드 NG발생 한 경우
					if ( SM.FMP[resultPart].GetInfo().ProdInfo[i].SerialNumber.ToLower().Contains("error")  == true )		continue;
					if (SM.FMP[resultPart].GetProdExist(i) == false)
					{
						dataInfo = new VariableInfo() { VID = "", Format = SECSItemFormat.L, Name = "INSP_RESULT_LIST" };
						dataInfo.ChildVariables.Add(new VariableInfo() { VID = "", Format = SECSItemFormat.A, Name = "TRAYID",	Value = "", Length = 0 });
						dataInfo.ChildVariables.Add(new VariableInfo() { VID = "", Format = SECSItemFormat.A, Name = "LIDID",	Value = "", Length = 0 });
						dataInfo.ChildVariables.Add(new VariableInfo() { VID = "", Format = SECSItemFormat.A, Name = "SN",		Value = "", Length = 0 });
						dataInfo.ChildVariables.Add(new VariableInfo() { VID = "", Format = SECSItemFormat.A, Name = "UNIQUEID",Value = "", Length = 0 });
						int nResult = 1;
						dataInfo.ChildVariables.Add(new VariableInfo() { VID = "", Format = SECSItemFormat.U1, Name = "RESULT", Value = nResult, Length = 1 });
						resultList.ChildVariables.Add(dataInfo);
					}
					else
					{
						dataInfo = new VariableInfo() { VID = "", Format = SECSItemFormat.L, Name = "INSP_RESULT_LIST" };
						dataInfo.ChildVariables.Add(new VariableInfo() { VID = "", Format = SECSItemFormat.A, Name = "TRAYID",	Value = SM.FMP[resultPart].GetInfo().ProdInfo[i].TrayBarcode, Length = SM.FMP[resultPart].GetInfo().ProdInfo[i].TrayID.Length });
						dataInfo.ChildVariables.Add(new VariableInfo() { VID = "", Format = SECSItemFormat.A, Name = "LIDID",	Value = "", Length = 0});
						dataInfo.ChildVariables.Add(new VariableInfo() { VID = "", Format = SECSItemFormat.A, Name = "SN",		Value = SM.FMP[resultPart].GetInfo().ProdInfo[i].SerialNumber, Length = SM.FMP[resultPart].GetInfo().ProdInfo[i].SerialNumber.Length });
						dataInfo.ChildVariables.Add(new VariableInfo() { VID = "", Format = SECSItemFormat.A, Name = "UNIQUEID",Value = SM.FMP[resultPart].GetInfo().ProdInfo[i].ProdID, Length = SM.FMP[resultPart].GetInfo().ProdInfo[i].ProdID.Length });
						int nResult = SM.FMP[resultPart].GetProdStatus(i) == ProdStatus.OK ? 1 : 0;
						dataInfo.ChildVariables.Add(new VariableInfo() { VID = "", Format = SECSItemFormat.U1, Name = "RESULT", Value = nResult, Length = 1 });
						resultList.ChildVariables.Add(dataInfo);
					}
				}

				//피듀셜, 바코드 NG발생 한 제품만 있는 경우
				if (resultList.ChildVariables.Count == 0)
				{
					if (ACK6_RCVData.ContainsKey("1018") == false)	ACK6_RCVData.Add("1018", 0);
					else											ACK6_RCVData["1018"] = 0;
					return true;
				}


				ceInfo.Reports.Items["1018"].Variables.Items[3] = resultList;
			
				
				if (ACK6_RCVData.ContainsKey("1018") == false)	ACK6_RCVData.Add("1018", -1);
				else											ACK6_RCVData["1018"] = -1;
                //WriteSendLog(ceInfo, REQ.EAPEIS_TAMSF_FVI_END_REQ);
				GemDriverError driveResult = m_gemDriver.ReportCollectionEvent(ceInfo);
                
                GLog.Add("Send SECS/GEM - SendInspectionResult");
				
				if(driveResult != GemDriverError.Ok)
				{
					G.AlmMng.ADD(ALM.ERROR_SESCGEM_DRIVER, driveResult.ToString());
					return false;
				}

                //check:ubiTEST

                return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}
		
		public bool SendTrayValidation(FlowTray resultTray)
		{
			try
			{
				if (m_bConnect == false)						return false;
				if (SM.FMT.IsFlow(resultTray) == false)			return false;

				CollectionEventInfo ceInfo = new CollectionEventInfo();
				if (CE["1019"].Name != EnumHelper.GetDescription(REQ.TRAY_VALIDATION)) return false;

				//// 예제에서 직접 호출해서 저장하는 것을 피해 달라고 함 
				//// EAPEIS_TAMSF_FVI_START_REQ의 ECID: "14", 해당되는 REPORT의 RPTID: "6";
				
				string strWorkOrder		= SM.FMT[resultTray].GetInfo().WorkOrder;
				string strProgramID_Top	= SM.FMT[resultTray].GetInfo().ProgramID_Top;
				string strProgramID_Btm	= SM.FMT[resultTray].GetInfo().ProgramID_Btm;
				string strTrayBarcode	= SM.FMT[resultTray].GetInfo().TrayBarcode;
				string strInTime		= TypeConversion.GetFormatTimeFromTicks(SM.FMT[resultTray].GetInfo().InTime);
				string strOutTime		= TypeConversion.GetFormatTimeFromTicks(SM.FMT[resultTray].GetInfo().OutTime);
				if (resultTray == FlowTray.Reject)
				{
					strWorkOrder		= SM.SHM.GetRunLotInfo(0).WorkOrder;
					strProgramID_Top	= SM.SHM.GetRunLotInfo(0).ProgramID_Top;
					strProgramID_Btm	= SM.SHM.GetRunLotInfo(0).ProgramID_Btm;
					strTrayBarcode		= "Reject";
					strOutTime			= TypeConversion.GetFormatTimeFromTicks(DateTime.Now.Ticks);
				} 
				ceInfo = CE["1019"];
				ceInfo.Reports.Items["1019"].Variables.Items[0].Value = strWorkOrder;
				ceInfo.Reports.Items["1019"].Variables.Items[1].Value = strProgramID_Top;
				ceInfo.Reports.Items["1019"].Variables.Items[2].Value = strProgramID_Btm;
				ceInfo.Reports.Items["1019"].Variables.Items[3].Value = strTrayBarcode;
				ceInfo.Reports.Items["1019"].Variables.Items[5].Value = strInTime;
				ceInfo.Reports.Items["1019"].Variables.Items[6].Value = strOutTime;
				
				VariableInfo resultList = ceInfo.Reports.Items["1019"].Variables.Items[7];
				VariableInfo dataInfo;
				resultList.ChildVariables.Clear();
				if (SM.FMT.IsFlow(resultTray) == true)
				{
					for (int i = 0; i < SM.FMT[resultTray].GetInfo().ProdInfo.Length; i++)
					{
						if (SM.FMT[resultTray].GetInfo().ProdInfo[i].Exist == true)
						{
							if (SM.FMT[resultTray].GetInfo().ProdInfo[i].SerialNumber.ToLower().Contains("error") ==  true)		continue;
							if (string.IsNullOrEmpty( SM.FMT[resultTray].GetInfo().ProdInfo[i].SerialNumber) ==  true)			continue;
							
							dataInfo = new VariableInfo() { VID = "", Format = SECSItemFormat.L, Name = "VALIDATED_RESULT_LIST" };
							
							dataInfo.ChildVariables.Add(new VariableInfo() { VID = "", Format = SECSItemFormat.A, Name = "SN", Value = SM.FMT[resultTray].GetInfo().ProdInfo[i].SerialNumber, Length = SM.FMT[resultTray].GetInfo().ProdInfo[i].SerialNumber.Length });
							int nResult =SM.FMT[resultTray].GetProdStatus(i) == ProdStatus.OK ? 1 : 0;
							dataInfo.ChildVariables.Add(new VariableInfo() { VID = "", Format = SECSItemFormat.U1, Name = "RESULT", Value = nResult, Length = 1 });
							resultList.ChildVariables.Add(dataInfo);
						}
					}

					ceInfo.Reports.Items["1019"].Variables.Items[7] = resultList;
				}

				ceInfo.Reports.Items["1019"].Variables.Items[8].Value = SM.FMT[resultTray].GetInfo().RowCnt;
				ceInfo.Reports.Items["1019"].Variables.Items[9].Value = SM.FMT[resultTray].GetInfo().ColCnt;

				//WriteSendLog(ceInfo, REQ.EAPEIS_TAMSF_FVI_END_REQ);
				if (ACK6_RCVData.ContainsKey("1019") == false)	ACK6_RCVData.Add("1019", -1);
				else											ACK6_RCVData["1019"] = -1;

				GemDriverError driveResult = m_gemDriver.ReportCollectionEvent(ceInfo);
                
                GLog.Add("Send SECS/GEM - SendTrayValidation");
				
				if(driveResult != GemDriverError.Ok)
				{
					G.AlmMng.ADD(ALM.ERROR_SESCGEM_DRIVER, driveResult.ToString());
					return false;
				}

                //check:ubiTEST

                return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public bool SendLotEnd()
		{
			try
			{
				if (m_bConnect == false)			return false;
				if (SM.FMT.IsFlow(FlowTray.ULDConv01) == false )			return false;

				CollectionEventInfo ceInfo = new CollectionEventInfo();
				if (CE["4000"].Name != EnumHelper.GetDescription(REQ.LOT_END)) return false;

				ceInfo = CE["4000"];
				ceInfo.Reports.Items["4000"].Variables.Items[0].Value = SM.FMT[FlowTray.ULDConv01].GetInfo().WorkOrder;// WO
				ceInfo.Reports.Items["4000"].Variables.Items[1].Value = SM.FMT[FlowTray.ULDConv01].GetInfo().ProgramID_Top; // ProgramProcessID
				ceInfo.Reports.Items["4000"].Variables.Items[2].Value = SM.FMT[FlowTray.ULDConv01].GetInfo().ProgramID_Btm; // ProgramProcessID

				GemDriverError driveResult = m_gemDriver.ReportCollectionEvent(ceInfo);
                
                GLog.Add("Send SECS/GEM - SendLOTEND");
				
				if(driveResult != GemDriverError.Ok)
				{
					G.AlmMng.ADD(ALM.ERROR_SESCGEM_DRIVER, driveResult.ToString());
					return false;
				}

				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public bool SendControlStateChanged(ControlState controlState)
		{
			try
			{
				if (m_bConnect == false)			return false;

				CollectionEventInfo ceInfo = new CollectionEventInfo();
				if (CE["1001"].Name != EnumHelper.GetDescription(REQ.ControlStateChanged)) return false;

				ceInfo = CE["1001"];
				
				ceInfo.Reports.Items["1001"].Variables.Items[0].Value = (int) controlState;
				ceInfo.Reports.Items["1001"].Variables.Items[1].Value = SM.SHM.GetRunLotInfo(0).WorkOrder;// WO
				ceInfo.Reports.Items["1001"].Variables.Items[2].Value = SM.SHM.GetRunLotInfo(0).ProgramID_Top; // ProgramProcessID
				ceInfo.Reports.Items["1001"].Variables.Items[3].Value = SM.SHM.GetRunLotInfo(0).ProgramID_Btm; // ProgramProcessID

				int orderType = 0;
				switch (Program.m_PasswordMode)
				{
					case PW_MODE.ADMIN:
					case PW_MODE.PRO:
						orderType = 5;
						break;
					case PW_MODE.OPER:
						orderType = 2;
						break;
					case PW_MODE.ENG:
						orderType = 4;
						break;
				}

				ceInfo.Reports.Items["1001"].Variables.Items[4].Value = orderType;

				GemDriverError driveResult = m_gemDriver.ReportCollectionEvent(ceInfo);
                
                GLog.Add($"Send SECS/GEM - SendControlStateChanged : {controlState}");
				
				if(driveResult != GemDriverError.Ok)
				{
					G.AlmMng.ADD(ALM.ERROR_SESCGEM_DRIVER, driveResult.ToString());
					return false;
				}

				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public bool SendProcessStateChanged(int nProcessState)
		{
			try
			{
				CollectionEventInfo ceInfo = new CollectionEventInfo();
				if (CE["1006"].Name != EnumHelper.GetDescription(REQ.ProcessStateChanged)) return false;

				ceInfo = CE["1006"];
				
				ceInfo.Reports.Items["1006"].Variables.Items[0].Value = nProcessState;
				ceInfo.Reports.Items["1006"].Variables.Items[1].Value = SM.SHM.GetRunLotInfo(0).WorkOrder;// WO
				ceInfo.Reports.Items["1006"].Variables.Items[2].Value = SM.SHM.GetRunLotInfo(0).ProgramID_Top; // ProgramProcessID
				ceInfo.Reports.Items["1006"].Variables.Items[3].Value = SM.SHM.GetRunLotInfo(0).ProgramID_Btm; // ProgramProcessID

				int orderType = 0;
				switch (Program.m_PasswordMode)
				{
					case PW_MODE.ADMIN:
					case PW_MODE.PRO:
						orderType = 5;
						break;
					case PW_MODE.OPER:
						orderType = 2;
						break;
					case PW_MODE.ENG:
						orderType = 4;
						break;
				}

				ceInfo.Reports.Items["1006"].Variables.Items[4].Value = orderType;

				GemDriverError driveResult = m_gemDriver.ReportCollectionEvent(ceInfo);
                
                GLog.Add($"Send SECS/GEM - SendProcessStateChanged : {nProcessState.ToString()}");
				
				if(driveResult != GemDriverError.Ok)
				{
					G.AlmMng.ADD(ALM.ERROR_SESCGEM_DRIVER, driveResult.ToString());
					return false;
				}

				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public bool SendAlarmSet(ALM code)
        {
            try
            {
				if (m_bConnect == false)			return false;

                //WriteSendLog(ceInfo, REQ.EAPEIS_TAMSF_GET_FVI_PGM_REQ);
                //GemDriverError driveResult = m_gemDriver.ReportAlarmSet((long)code, 0 , code.ToString());
				
				SECSMessage message = new SECSMessage();

				message = m_gemDriver.UserMessage["S5F1"];
				
				message.Body.AsList[1].Value = 128; // ALCD
				message.Body.AsList[2].Value = (long)code; //ALID
				message.Body.AsList[3].Value = code.ToString(); // ALTX
				GemDriverError driveResult = m_gemDriver.SendSECSMessage(message);

                GLog.Add(string.Format($"Send SECS/GEM - SendAlarmSet : {code.ToString()}"));

                if (driveResult != GemDriverError.Ok)
                {
                    G.AlmMng.ADD(ALM.ERROR_SESCGEM_DRIVER, driveResult.ToString());
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool SendAlarmClear(ALM code)
        {
            try
            {
				if (m_bConnect == false)			return false;
                //WriteSendLog(ceInfo, REQ.EAPEIS_TAMSF_GET_FVI_PGM_REQ);
               // GemDriverError driveResult = m_gemDriver.ReportAlarmClear((long)code);
                //GemDriverError driveResult = m_gemDriver.ReportAlarmClear((long)code, 128 , code.ToString());
				
				SECSMessage message = new SECSMessage();

				message = m_gemDriver.UserMessage["S5F1"];
				
				message.Body.AsList[1].Value = 0;// ALCD
				message.Body.AsList[2].Value = (long)code;  //ALID
				message.Body.AsList[3].Value = code.ToString(); //ALTX
				GemDriverError driveResult = m_gemDriver.SendSECSMessage(message);

                GLog.Add(string.Format($"Send SECS/GEM - SendAlarmClear : {code.ToString()}"));

                if (driveResult != GemDriverError.Ok)
                {
                    G.AlmMng.ADD(ALM.ERROR_SESCGEM_DRIVER, driveResult.ToString());
                    return false;
                }
#if _SIMUL_

#endif
                return true;
            }
            catch
            {
                return false;
            }
        }
		#endregion
		
		#region [Recv Function]
		public int RecvPP_SELECT(ref Dictionary<string, string> _PP_SELECTData)
		{
			try
			{
				// 0: 받지 않음, 1: 정상, -1: 받았지만 Data가 정상이 아님
				// -9999: 오류
				_PP_SELECTData = new Dictionary<string, string>();
				//if(SM.SHM.IsBitData(BIT.UBIGEM_PP_SELECT) == false)			return 0;
				if(PP_SELECT_RCVData.Count < (int)CPNAME_PP_SELECT.MAX)			return -1;
				
				_PP_SELECTData = PP_SELECT_RCVData;
				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}

		public int RecvSENDWORKINFO(ref Dictionary<string, string> _SENDWORKINFOData)
		{
			try
			{
				// 0: 받지 않음, 1: 정상, -1: 받았지만 Data가 정상이 아님
				// -9999: 오류
				_SENDWORKINFOData= new Dictionary<string, string>();

				if(SM.SHM.IsBitData(BIT.UBIGEM_SENDWORKINFO) == false)						return 0;
				else if(SENDWORKINFO_RCVData.Count < (int)CPNAME_SENDWORKINFO.MAX)		return -1;
				
				_SENDWORKINFOData = SENDWORKINFO_RCVData;
				
				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}
		
		public int RecvAck6(REQ ceid, ref int Ack)
		{
			try
			{
				if (Program._bSimul_ == true)
				{
					Ack = 0;
					return 1;
				}
				// 0: 받지 않음, 1: 정상, -1: 받았지만 Data가 정상이 아님
				// -9999: 오류
				string strCeid = ((int)ceid).ToString();

				if (ACK6_RCVData.ContainsKey(strCeid) == false)														return 0;
				if (ACK6_RCVData[strCeid] == -1)																		return 0;
				//if (ACK6_RCVData[strCeid] != (int)ACKC6.Accepted )	return -1;

				Ack = ACK6_RCVData[strCeid] ;
				ACK6_RCVData.Remove(strCeid);

				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}
		public int RecvAck5(REQ ceid, ref int Ack)
		{
			try
			{
				if (Program._bSimul_ == true)
				{
					Ack = 0;
					return 1;
				}
				// 0: 받지 않음, 1: 정상, -1: 받았지만 Data가 정상이 아님
				// -9999: 오류
				string strCeid = ((int)ceid).ToString();

				if (ACK5_RCVData.ContainsKey(strCeid) == false)														return 0;
				if (ACK5_RCVData[strCeid] == -1)																		return 0;
				//if (ACK5_RCVData[strCeid] != (int)ACKC6.Accepted && ACK5_RCVData[strCeid] != (int)ACKC6.Accepted )	return -1;

				GLog.Add("TEST ----------------   ack5 = 1");
				Ack = ACK5_RCVData[strCeid] ;
				ACK5_RCVData.Remove(strCeid);

				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}

		#endregion

		//         private void WriteSendLog(CollectionEventInfo ceInfo, REQ req)
		//         {
		//             try
		//             {
		//                 string logText = $"{Environment.NewLine}[SEND][CEID={ceInfo.Name}]";
		//                 string ItemsNo = ((int)req).ToString();
		//                 foreach (var ItemsInfo in ceInfo.Reports.Items)
		//                 {
		//                     logText += $"[RPTID : {ItemsInfo.Value.Description}] {Environment.NewLine}";
		//                     foreach (VariableInfo paramInfo in ceInfo.Reports.Items[ItemsNo].Variables.Items)
		//                     {
		//                         logText += $"{paramInfo.Value}, {paramInfo.Name}({paramInfo.Format}){Environment.NewLine}";
		//                     }
		//                 }
		//                 GLog.AddUbi(logText);
		//                 if (EventUBIGEMDataLog != null) EventUBIGEMDataLog(logText);
		//             }
		//             catch (Exception ex)
		//             {
		// 
		//                 throw;
		//             }

		// }


	}
}
