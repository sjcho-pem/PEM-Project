using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UbiCom.Net.Structure;
using UbiGEM.Net.Structure;
using System.Runtime.InteropServices;
using System.IO;
using System.Xml;

namespace H_MarsSSD
{
	public class UbiGEMBaseClass
	{
		public struct SystemTime
        {
            public ushort wYear;
            public ushort wMonth;
            public ushort wDayOfWeek;
            public ushort wDay;
            public ushort wHour;
            public ushort wMinute;
            public ushort wSecond;
            public ushort wMilliseconds;
        }

		[DllImport("kernel32.dll", SetLastError = true)]
        public static extern int SetSystemTime([In] SystemTime st); // System time을 변경하기 위한 함수: 컴퓨터 자체의 세팅도 같이 해줘야 함

		protected delegate void ReceiveRemoteInfoHandler(object sender, RemoteCommandInfo receivedMsg);
        protected event ReceiveRemoteInfoHandler ReceiveInfo;
        private void OnReceiveRemote(RemoteCommandInfo receivedRemoteInfo)
        {
            if (ReceiveInfo != null)
            {
                ReceiveInfo(this, receivedRemoteInfo);
            }
        }

		protected delegate void CommunicationCompletedHandler(bool ConnectState);
        protected event CommunicationCompletedHandler ConnectComplete;
        private void OnCommunicationComplete(bool ConnectState)
        {
            if (ConnectComplete != null)
            {
                ConnectComplete(ConnectState);
            }
        }

		protected delegate void TerminalTextReceiveHandler(string Text);
		protected event TerminalTextReceiveHandler TerminalTextReceive;
		private void OnTerminalTextReceive(string Text)
		{
			if (TerminalTextReceive != null)
			{
				TerminalTextReceive(Text);
			}
		}
		
		protected delegate void AckReceiveHandler(string ceid, int ack);
		protected event AckReceiveHandler AckReceive;
		private void OnAckReceive(string ceid, int ack)
		{
			if (AckReceive != null)
			{
				AckReceive(ceid, ack);
			}
		}

		
		protected delegate void ControlStateChangedHandler(ControlState controlState);
		protected event ControlStateChangedHandler ControlStateChanged;
		private void OnControlStateChanged(ControlState controlState)
		{
			if (ControlStateChanged != null)
			{
				ControlStateChanged(controlState);
			}
		}

		//CYJ 2024/11/06 user가 Define한 Secondary Message 적용
		protected delegate void SecondaryMessageReceiveHandler(SECSMessage primaryMessage, SECSMessage secondaryMessage);
		protected event SecondaryMessageReceiveHandler SecondaryMessageReceive;
		private void OnSecondaryMessageReceive(SECSMessage primaryMessage, SECSMessage secondaryMessage)
		{
			if (SecondaryMessageReceive != null)
			{
				SecondaryMessageReceive(primaryMessage, secondaryMessage);
			}
		}


		#region [Constant]
        private readonly string DATETIME_TEXT_FORMAT = "{0} [{1}] {2}" + Environment.NewLine;
        private const string PROGRAM_TITLE_FORMAT = "{0} - {1}";
        private const string PROGRAM_STATUS_FORMAT = "{0} - {1}:{2}";

        private const string UGC_FILE_FILTER = "UbiGEM Configuration File (*.ugc)|*.ugc|All files (*.*)|*.*";
        
        private const int LOG_LINE_MAX_COUNT = 100;
        #endregion
        #region [Variable]
        protected UbiGEM.Net.Driver.GemDriver			m_gemDriver;
        protected List<long>							m_setAlarmList; // 알람 리스트 차후 필요
        protected int									m_ack; // 데이터를 정확히 받았다는 것을 확인하는 변수
        protected string								m_ugcFilePath; // ugc 파일 이름
		

		protected List<VariableInfo>								ECID;
        protected List<VariableInfo>								VID;
        protected Dictionary<string, CollectionEventInfo>			CE;
        protected KeyValuePair<string, SECSMessage>[]				UM;//UserMessage Array
		protected RemoteCommandCollection							RM; // REMOTE COMMAND INFO
		
		#endregion

		#region [Communication Variable]
		protected CommunicationState		m_CommState;
		protected ControlState				m_CtrlState;
		protected bool						m_bConnect;
		protected bool						m_bSelected;
		protected bool						m_bCommCheckComplete;
		protected byte						m_bProcessState;
		protected byte						m_bPrevProcessState;
		#endregion
		

		protected void Init()
		{
			try
			{
				if (m_gemDriver != null) return;

				m_gemDriver = new UbiGEM.Net.Driver.GemDriver();
				m_setAlarmList = new List<long>();

				if(m_ugcFilePath == "")
					m_ugcFilePath = Path.GetFullPath(@"..\..\secsgem_pemtron.ugc");
                
                
				m_CommState = m_gemDriver.CommunicationState;
				m_CtrlState = m_gemDriver.ControlState;
				m_bConnect = false;
				m_bSelected = false;
				m_bCommCheckComplete = false;
                
                m_bProcessState = 1;
				m_bPrevProcessState = 0;

				#region [Communication Event]
				m_gemDriver.OnCommunicationStateChanged += GemDriver_OnCommunicationStateChanged;
				m_gemDriver.OnControlStateChanged += GemDriver_OnControlStateChanged;
				m_gemDriver.OnEquipmentProcessState += GemDriver_OnEquipmentProcessState;
				m_gemDriver.OnGEMConnected += GemDriver_OnGEMConnected;
				m_gemDriver.OnGEMSelected += GemDriver_OnGEMSelected;
				m_gemDriver.OnGEMDeselected += GemDriver_OnGEMDeselected;
				m_gemDriver.OnGEMDisconnected += GemDriver_OnGEMDisconnected;
				m_gemDriver.OnControlStateOnlineChangeFailed += GemDriver_OnControlStateOnlineChangeFailed;
				#endregion


				#region [Received Message Event]
				m_gemDriver.OnReceivedRequestOffline += GemDriver_OnReceivedRequestOffline;
				m_gemDriver.OnReceivedRequestOnline += GemDriver_OnReceivedRequestOnline;
				m_gemDriver.OnReceivedDefineReport += GemDriver_OnReceivedDefineReport;
				m_gemDriver.OnReceivedLinkEventReport += GemDriver_OnReceivedLinkEventReport;
				m_gemDriver.OnReceivedEnableDisableEventReport += GemDriver_OnReceivedEnableDisableEventReport;
				m_gemDriver.OnReceivedRemoteCommand += GemDriver_OnReceivedRemoteCommand;
				m_gemDriver.OnReceivedEnhancedRemoteCommand += GemDriver_OnReceivedEnhancedRemoteCommand;
				m_gemDriver.OnReceivedNewECVSend += GemDriver_OnReceivedNewECVSend;
				m_gemDriver.OnReceivedEnableDisableAlarmSend += GemDriver_OnReceivedEnableDisableAlarmSend;
				m_gemDriver.OnReceivedTerminalMessage += GemDriver_OnReceivedTerminalMessage;
				m_gemDriver.OnReceivedTerminalMultiMessage += GemDriver_OnReceivedTerminalMultiMessage;
				m_gemDriver.OnReceivedPPRequest += GemDriver_OnReceivedPPRequest;
				m_gemDriver.OnReceivedPPSend += GemDriver_OnReceivedPPSend;
				m_gemDriver.OnReceivedPPLoadInquire += GemDriver_OnReceivedPPLoadInquire;
				m_gemDriver.OnReceivedDeletePPSend += GemDriver_OnReceivedDeletePPSend;
				m_gemDriver.OnReceivedFmtPPRequest += GemDriver_OnReceivedFmtPPRequest;
				m_gemDriver.OnReceivedFmtPPSend += GemDriver_OnReceivedFmtPPSend;
				m_gemDriver.OnReceivedCurrentEPPDRequest += GemDriver_OnReceivedCurrentEPPDRequest;
				m_gemDriver.OnReceivedDateTimeRequest += GemDriver_OnReceivedDateTimeRequest;
				m_gemDriver.OnReceivedDateTimeSetRequest += GemDriver_OnReceivedDateTimeSetRequest;
				m_gemDriver.OnReceivedLoopback += GemDriver_OnReceivedLoopback;
				m_gemDriver.OnReceivedEstablishCommunicationsRequest += GemDriver_OnReceivedEstablishCommunicationsRequest;
				m_gemDriver.OnUserPrimaryMessageReceived += GemDriver_OnUserPrimaryMessageReceived;
				m_gemDriver.OnUserSecondaryMessageReceived += GemDriver_OnUserSecondaryMessageReceived;
				m_gemDriver.OnReceivedUnknownMessage += GemDriver_OnReceivedUnknownMessage;
				m_gemDriver.OnInvalidMessageReceived += GemDriver_OnInvalidMessageReceived;
				m_gemDriver.OnReceivedInvalidRemoteCommand += GemDriver_OnReceivedInvalidRemoteCommand;
				m_gemDriver.OnReceivedInvalidEnhancedRemoteCommand += GemDriver_OnReceivedInvalidEnhancedRemoteCommand;
				#endregion

				#region [Resposnse Message Event]
				m_gemDriver.OnResponseTerminalRequest += GemDriver_OnResponseTerminalRequest;
				m_gemDriver.OnResponsePPRequest += GemDriver_OnResponsePPRequest;
				m_gemDriver.OnResponsePPSend += GemDriver_OnResponsePPSend;
				m_gemDriver.OnResponsePPLoadInquire += GemDriver_OnResponsePPLoadInquire;
				m_gemDriver.OnResponseFmtPPRequest += GemDriver_OnResponseFmtPPRequest;
				m_gemDriver.OnResponseFmtPPSend += GemDriver_OnResponseFmtPPSend;
				m_gemDriver.OnResponseFmtPPVerification += GemDriver_OnResponseFmtPPVerification;
				m_gemDriver.OnResponseDateTimeRequest += GemDriver_OnResponseDateTimeRequest;
				m_gemDriver.OnResponseLoopback += GemDriver_OnResponseLoopback;
				m_gemDriver.OnResponseEventReportAcknowledge += GemDriver_OnResponseEventReportAcknowledge;
				#endregion

				#region [Request Message Event]
				m_gemDriver.OnVariableUpdateRequest += GemDriver_OnVariableUpdateRequest;
				m_gemDriver.OnUserGEMMessageUpdateRequest += GemDriver_OnUserGEMMessageUpdateRequest;
				m_gemDriver.OnTraceDataUpdateRequest += GemDriver_OnTraceDataUpdateRequest;
				#endregion
			}
			catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
		}
        
        #region [Init, Start, Stop]
        protected void GEMInit()
		{
			try
			{
				if(m_gemDriver == null)			return;
                
				GEMIPInit();
				string errString = "";
				GemDriverError driverResult = m_gemDriver.Initialize(m_ugcFilePath, out errString);
				if(GemDriverError.Ok == driverResult)
				{
                    
					ECID			= m_gemDriver.Variables.ECV.Items.Where(t => t.Format != SECSItemFormat.L).ToList();
					VID				= m_gemDriver.Variables.Variables.Items.Where(t => t.Format != SECSItemFormat.L).ToList();
					CE				= m_gemDriver.CollectionEvents.Items;
					UM				= m_gemDriver.UserMessage.MessageInfo;
					RM				= m_gemDriver.RemoteCommand;
                    GEMVIDInit();
                }
				else
				{
					// 차후 로그 추가
				}
			}
			catch  (System.Exception ex)
            {
                GLog.Add("SetXML_UGCData Err : " + ex.ToString());
            }
		}

		protected void GEMStart()
		{
			try
			{
				if (m_gemDriver == null)			return;

				GemDriverError driverStart			= m_gemDriver.Start();

				if (driverStart != GemDriverError.Ok)
				{
					// 차후 로그 추가
				}
			}
			catch (System.Exception ex)
            {
                GLog.Add("GEMStart Err : " + ex.ToString());
            }
		}

		protected void GEMStop()
		{
			try
			{
				if (m_gemDriver == null)			return;

				GemDriverError driverStop			= m_gemDriver.Stop();

				if (driverStop != GemDriverError.Ok)
				{
					// 차후 로그 추가
				}
			}
			catch (System.Exception ex)
            {
                GLog.Add("GEMStart Err : " + ex.ToString());
            }
		}
        
        private void GEMIPInit() // MaterialAPD_InspectionSummary
        {
            try
            {
                string sGEMIP = "";
                string sGEMPORT = "";

                if (Program.iniMachine.GetString("Socket", frmIPSetting.Position.MES.ToString()) == "")
					Program.iniMachine.SetString("Socket", frmIPSetting.Position.MES.ToString(), "127.0.0.1:8000");

                string strIpPort = Program.iniMachine.GetString("Socket", frmIPSetting.Position.MES.ToString());
                string[] str = strIpPort.Split(':');
                if (str.Length != 2)
                {
                    sGEMIP = "";
                    sGEMPORT = "";
                }
                else
                {
                    sGEMIP = str[0];
                    sGEMPORT = str[1];
                }


                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(m_ugcFilePath);
                XmlElement root = xmldoc.DocumentElement;
                XmlNodeList vNodeList = root.ChildNodes;
                foreach (XmlNode vNode in vNodeList)
                {
                    if (vNode.Name == "SECSDriver")
                    {
                        foreach (XmlNode vCNode in vNode.ChildNodes)
                        {
                            if (vCNode.Name == "HSMS")
                            {
                                foreach (XmlNode vCCNode in vCNode.ChildNodes)
                                {
                                    if (vCCNode.Name == "RemoteIPAddress")
                                    {
                                        vCCNode.InnerText = sGEMIP;                                        
                                    }
                                    else if (vCCNode.Name == "RemotePortNo")
                                    {
                                        vCCNode.InnerText = sGEMPORT;
                                    }
                                    else if (vCCNode.Name == "LocalIPAddress")
                                    {
                                        vCCNode.InnerText = sGEMIP;
                                    }
                                    else if (vCCNode.Name == "LocalPortNo")
                                    {
                                        vCCNode.InnerText = sGEMPORT;
                                    }
                                }
                                break;
                            }
                        }
                        break;
                    }
                }
                xmldoc.Save(m_ugcFilePath);
            }
            catch (System.Exception ex)
            {
                GLog.Add("SetXML_UGCData Err : " + ex.ToString());
            }
        }

        private void GEMVIDInit()
        {   
            //InputVid_Data((int)teVID.MDLN, "7010");
            //InputVid_Data((int)teVID.SOFTREV, "1.01");
            //InputVid_Data((int)teVID.user_name, "USER");
        }
		#endregion

		#region [Control State]
		protected void SetControlState(ControlState _controlState)
		{
			try
			{
				if(m_gemDriver == null)			return;

				GemDriverError driverResult;

				switch (_controlState)
				{
					case ControlState.EquipmentOffline:			driverResult = m_gemDriver.RequestOffline();
						break;
					case ControlState.OnlineLocal:				driverResult = m_gemDriver.RequestOnlineLocal();
						break;
					case ControlState.OnlineRemote:				driverResult = m_gemDriver.RequestOnlineRemote();
						break;
					default:
						break;
				}
			}
			catch (System.Exception ex)
            {
                GLog.Add("SetControlState Err : " + ex.ToString());
            }
			
		}
		#endregion

		private void CheckCommunication()
		{
			try
			{
				if (m_CommState != CommunicationState.Communicating)			{ m_bCommCheckComplete = false; OnCommunicationComplete(m_bCommCheckComplete); return; }
				//if (m_CtrlState != ControlState.OnlineRemote)					{ m_bCommCheckComplete = false; OnCommunicationComplete(m_bCommCheckComplete); return; }
				if (m_bConnect == false)										{ m_bCommCheckComplete = false; OnCommunicationComplete(m_bCommCheckComplete); return; }
				if (m_bSelected == false)										{ m_bCommCheckComplete = false; OnCommunicationComplete(m_bCommCheckComplete); return; }

				m_bCommCheckComplete = true;

				OnCommunicationComplete(m_bCommCheckComplete);
			}
			catch (System.Exception ex)
            {
                GLog.Add("CheckCommunication Err : " + ex.ToString());
            }
		}

		#region [Communication Event]
        /// <summary>
        /// Communication State가 변경될 경우 발생하는 이벤트입니다.
        /// </summary>
        /// <param name="communicationState"></param>
        private void GemDriver_OnCommunicationStateChanged(CommunicationState communicationState)
        {
			try
			{
				m_CommState = communicationState;
				CheckCommunication();
			}
			catch
			{

			}
        }

        /// <summary>
        /// Control State가 변경될 경우 발생하는 이벤트입니다.
        /// </summary>
        /// <param name="controlState"></param>
        private void GemDriver_OnControlStateChanged(ControlState controlState)
        {
            try
			{
				m_CtrlState = controlState;
				CheckCommunication();
				OnControlStateChanged(controlState);
			}
			catch
			{

			}
        }

        /// <summary>
        /// EquipmentProcess State가 변경될 경우 발생하는 이벤트입니다.
        /// </summary>
        /// <param name="equipmentProcessState"></param>
        private void GemDriver_OnEquipmentProcessState(byte equipmentProcessState)
        {
			m_bPrevProcessState = m_bProcessState;
			m_bProcessState = equipmentProcessState;
            //WriteLog(LogLevel.Information, $"OnEquipmentProcessState - {equipmentProcessState}");
			//차후 필요 시 추가
        }

        private void GemDriver_OnGEMConnected(string ipAddress, int portNo)
        {
            //UpdateTitle("Connected", ipAddress, portNo);
			m_bConnect = true;
			CheckCommunication();
        }

        private void GemDriver_OnGEMSelected(string ipAddress, int portNo)
        {
            m_bSelected = true;
			CheckCommunication();
        }

        private void GemDriver_OnGEMDeselected(string ipAddress, int portNo)
        {
            m_bSelected = false;
			CheckCommunication();
        }

        private void GemDriver_OnGEMDisconnected(string ipAddress, int portNo)
        {
            m_bConnect = false;
			CheckCommunication();
        }

        private void GemDriver_OnControlStateOnlineChangeFailed()
        {
            //WriteLog(LogLevel.Error, "OnControlStateOnlineChangeFailed");
			//차후 필요시 진행 예정
        }
        #endregion

		#region [Received Message Event]
        /// <summary>
        /// Host에서 S1F15(Offline Request)가 수신될 때 발생하는 이벤트입니다.
        /// </summary>
        /// <param name="systemBytes"></param>
        private void GemDriver_OnReceivedRequestOffline(uint systemBytes)
        {
			try
			{
				// 해당 이벤트가 발생 된 이후 CtrlState 상태가 HostOffline이 됩니다.

				m_gemDriver.ReplyRequestOfflineAck(systemBytes, m_ack); // 반드시 해야 함
			}
			catch
			{

			}
			
        }

        /// <summary>
        /// Host에서 S1F17(Online Request)가 수신될 때 발생하는 이벤트입니다.
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        private void GemDriver_OnReceivedRequestOnline(uint systemBytes)
        {
			try
			{
				// 해당 이벤트가 발생 된 이후 CtrlState 상태가 Online Remote 또는 Online local이 됩니다.

				 m_gemDriver.ReplyRequestOnlineAck(systemBytes, m_ack);
			}
			catch
			{

			}

           
        }

		/// <summary>
        /// S2F33(Define Report)가 수신될 경우 발생하는 이벤트입니다.
        /// </summary>
        private void GemDriver_OnReceivedDefineReport()
        {
            //WriteLog(LogLevel.Information, "Received Define Report");
        }

		/// <summary>
        /// S2F35(Link Event Report)가 수신될 경우 발생하는 이벤트입니다.
        /// </summary>
        private void GemDriver_OnReceivedLinkEventReport()
        {
            //WriteLog(LogLevel.Information, "Received LinkEvent Report");
        }

		 /// <summary>
        /// S2F37(Event Report Enable/Disable)이 수신될 경우 발생하는 이벤트입니다.
        /// </summary>
        private void GemDriver_OnReceivedEnableDisableEventReport()
        {
            //WriteLog(LogLevel.Information, "Received Enable Disable Event Send");
        }
		
        /// <summary>
        /// Host에서 S2F41(Remote Command)가 수신될 때 발생하는 이벤트입니다.
        /// RemoteCommandInfo 의 아이템을 순회하는 코드입니다.
        /// RemoteCommandParameterResult 에 IRemoteCommandParameterResult 를 추가하여 parameter 별 ack를 구성할 수 있습니다.
        /// </summary>
        /// <param name="remoteCommandInfo"></param>
        private void GemDriver_OnReceivedRemoteCommand(RemoteCommandInfo remoteCommandInfo)
        {
            RemoteCommandResult result = new RemoteCommandResult();
            RemoteCommandParameterResult paramResult;

			int isIndex = RM.RemoteCommandItems.FindIndex(x => x.RemoteCommand == remoteCommandInfo.RemoteCommand);

			if(isIndex == -1)
			{		
				result.HostCommandAck = (int)CPACK.OtherEquipmentspecificError;
                GLog.Add($"GemDriver_OnReceivedRemoteCommand Error : {CPACK.OtherEquipmentspecificError.ToString()} ");
				m_gemDriver.ReplyRemoteCommandAck(remoteCommandInfo, result); 
				return;
			}

            foreach (CommandParameterInfo paramInfo in remoteCommandInfo.CommandParameter.Items)
            {
                paramResult = new RemoteCommandParameterResult(paramInfo.Name, (int)CPACK.IllegalFormatSpecifiedForCPVAL);
                result.Items.Add(paramResult);
                
				if(paramInfo.Name != RM.RemoteCommandItems[isIndex].CommandParameter.Items[result.Items.Count - 1].Name ||
					paramInfo.Format != RM.RemoteCommandItems[isIndex].CommandParameter.Items[result.Items.Count - 1].Format)
				{ 
					GLog.Add($"GemDriver_OnReceivedRemoteCommand Error : {CPACK.IllegalFormatSpecifiedForCPVAL.ToString()} ");

					if(paramInfo.Name != RM.RemoteCommandItems[isIndex].CommandParameter.Items[result.Items.Count - 1].Name)
						result.HostCommandAck = (int)CPACK.ParameterNameDoesNotExist;

					if(paramInfo.Format != RM.RemoteCommandItems[isIndex].CommandParameter.Items[result.Items.Count - 1].Format)
						result.HostCommandAck = (int)CPACK.IllegalFormatSpecifiedForCPVAL;

					m_gemDriver.ReplyRemoteCommandAck(remoteCommandInfo, result);
					return; // Name or Format 값이 틀리면 
				}
            }
			
            //result.HostCommandAck = m_ack; // 0은 CPACK의 NoError를 뜻함
            //S2F42 Reply이전 호스트에서 받은 S2F41(EnhancedRemoteCommand)에 대한 Validation Check가 필요합니다.

            //m_gemDriver.ReplyRemoteCommandAck(remoteCommandInfo, result);

            //S2F42 Reply이후 관련된 Logic을 넣으면 됩니다.
			OnReceiveRemote(remoteCommandInfo);
        }

		/// <summary>
        /// Host에서 S2F49(Enhanced Remote Command)가 수신될 때 발생하는 이벤트입니다.
        /// EnhancedRemoteCommandInfo 의 아이템을 순회하는 코드입니다.
        /// RemoteCommandResult 에 RemoteCommandParameterResult 를 추가하여 parameter 별 ack를 구성할 수 있습니다.
        /// </summary>
        /// <param name="remoteCommandInfo"></param>
        private void GemDriver_OnReceivedEnhancedRemoteCommand(EnhancedRemoteCommandInfo remoteCommandInfo)
        {
            RemoteCommandResult result = new RemoteCommandResult();
            RemoteCommandParameterResult paramResult;
            string logText;

            result.HostCommandAck = m_ack;

            logText = $"[RemoteCommand={remoteCommandInfo.RemoteCommand}]{Environment.NewLine}";

            foreach (EnhancedCommandParameterInfo paramInfo in remoteCommandInfo.EnhancedCommandParameter.Items)
            {
                if (paramInfo.Format == SECSItemFormat.L)
                {
                    logText += $": [CPNAME={paramInfo.Name},Format={paramInfo.Format},Count={paramInfo.Items.Count}]{Environment.NewLine}";
                    paramResult = new RemoteCommandParameterResult(paramInfo.Name);

                    foreach (EnhancedCommandParameterItem item in paramInfo.Items)
                    {
                        logText += CheckValidationParameterItem(1, item, paramResult);
                    }
                }
                else
                {
                    logText += $": [CPNAME={paramInfo.Name},Format={paramInfo.Format},CPVAL={paramInfo.Value}]{Environment.NewLine}";
                    paramResult = new RemoteCommandParameterResult(paramInfo.Name, (int)CPACK.IllegalFormatSpecifiedForCPVAL);
                }

                result.Items.Add(paramResult);
            }

            if (logText.Length > 0)
            {
                logText = logText.Substring(0, logText.Length - Environment.NewLine.Length);
            }

            //WriteLog(LogLevel.Information, $"OnReceivedEnhancedRemoteCommand : {logText}");

            //S2F50 Reply이전 호스트에서 받은 S2F49(EnhancedRemoteCommand)에 대한 Validation Check가 필요합니다.

            m_gemDriver.ReplyEnhancedRemoteCommandAck(remoteCommandInfo, result);

            //S2F50 Reply이후 관련된 Logic을 넣으면 됩니다.
        }

        /// <summary>
        /// Host에서 S2F15(New ECV Send)가 수신될 때 발생하는 이벤트입니다.
        /// </summary>
        /// <param name="systemBytes"></param>
        /// <param name="newEcInfo"></param>
        private void GemDriver_OnReceivedNewECVSend(uint systemBytes, VariableCollection newEcInfo)
        {
            m_gemDriver.ReplyNewEquipmentConstantSend(systemBytes, newEcInfo, m_ack); 
			// 차후 Host에서 새로운 ECV 추가를 자유롭게 하기 위해 남김
        }

        /// <summary>
        /// S5F3(Alarm Enable/Disable Send)가 수신될 경우 발생하는 이벤트입니다.
        /// </summary>
        private void GemDriver_OnReceivedEnableDisableAlarmSend()
        {
            // 알람에 대한 컨셉이 정해지면 작성 시작
        }

        /// <summary>
        /// Host에서 S10F3(Terminal Message Single)이 수신될 때 발생하는 이벤트입니다
        /// </summary>
        /// <param name="systemBytes"></param>
        /// <param name="tid"></param>
        /// <param name="terminalMessage"></param>
        private void GemDriver_OnReceivedTerminalMessage(uint systemBytes, int tid, string terminalMessage)
        {
			try
			{
				OnTerminalTextReceive(terminalMessage);
				// 터미널 관련 메세지를 받을 일이 생기면 수정 및 함수 추가 필요

				// Check : 지금은 무조건 정상으로 보냄
				// 차후 시나리오에 따라 변경이 필요함
				m_gemDriver.ReplyTerminalMessageAck(systemBytes, m_ack);

				//Check : 차후 만들어야 함
			}
			catch (Exception ex)
            {
                UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
            }
			
        }

        /// <summary>
        /// Host에서 S10F5(Terminal Message Multi)가 수신될 때 발생하는 이벤트입니다
        /// </summary>
        /// <param name="systemBytes"></param>
        /// <param name="tid"></param>
        /// <param name="terminalMessages"></param>
        private void GemDriver_OnReceivedTerminalMultiMessage(uint systemBytes, int tid, List<string> terminalMessages)
        {
            string logText;

            logText = string.Empty;

            foreach (string terminalMessage in terminalMessages)
            {
                logText += terminalMessage + Environment.NewLine;
            }

            if (logText.Length > 0)
            {
                logText = logText.Substring(0, logText.Length - Environment.NewLine.Length);
            }

            // 터미널 관련 메세지를 받을 일이 생기면 수정 및 함수 추가 필요
            m_gemDriver.ReplyTerminalMultiMessageAck(systemBytes, m_ack);
        }

        /// <summary>
        /// S7F5(PP Reqeust)가 수신될 경우 발생하는 이벤트입니다
        /// </summary>
        /// <param name="systemBytes"></param>
        /// <param name="ppid"></param>
        private void GemDriver_OnReceivedPPRequest(uint systemBytes, string ppid)
        {
            bool result = false;
            List<byte> ppbody = new List<byte>();
            //result = MakePPBody(out ppbody); // 차후 Programe Process에 관련된 컨셉이 정해 질 떄 까지 주석 처리

            //WriteLog(LogLevel.Information, "Received PP Request");

            m_gemDriver.ReplyPPRequestAck(systemBytes, ppid, ppbody, result);
        }

        /// <summary>
        /// Host에서 S7F3(PP Send)가 수신될 때 발생하는 이벤트입니다
        /// </summary>
        /// <param name="systemBytes"></param>
        /// <param name="ppid"></param>
        /// <param name="ppbody"></param>
        private void GemDriver_OnReceivedPPSend(uint systemBytes, string ppid, List<byte> ppbody)
        {
            // 차후 Programe Process에 관련된 컨셉이 정해 질 떄 까지 주석 처리
            m_gemDriver.ReplyPPSendAck(systemBytes, (int)ACKC6.ErrorNotAccepted); // 0이 아니면 무조건 잘못 됨
        }

        /// <summary>
        /// Host에서 S7F1(PP Load Inquire)가 수신될 때 발생하는 이벤트입니다.
        /// </summary>
        /// <param name="systemBytes"></param>
        /// <param name="ppid"></param>
        /// <param name="length"></param>
        private void GemDriver_OnReceivedPPLoadInquire(uint systemBytes, string ppid, int length)
        {
           // 차후 Programe Process에 관련된 컨셉이 정해 질 떄 까지 주석 처리
		   m_gemDriver.ReplyPPLoadInquireAck(systemBytes, (int)ACKC6.ErrorNotAccepted);
        }

        /// <summary>
        /// S7F17(Delete PP Send)가 수신될 경우 발생하는 이벤트입니다.
        /// </summary>
        /// <param name="systemBytes"></param>
        /// <param name="ppids"></param>
        private void GemDriver_OnReceivedDeletePPSend(uint systemBytes, List<string> ppids)
        {
            string logText;

            logText = string.Empty;

            if (ppids.Count == 0)
            {
                //Delete All PPIDs
            }
            else
            {
                foreach (string ppid in ppids)
                {
                    //Delete Existing PPID.

                    logText += ppid + ",";
                }

                if (logText.Length > 0)
                {
                    logText = logText.Substring(0, logText.Length - 1);
                }
            }

			
			// 차후 Programe Process에 관련된 컨셉이 정해 질 떄 까지 주석 처리
            m_gemDriver.ReplyPPDeleteAck(systemBytes, (int)ACKC6.ErrorNotAccepted);
        }

        /// <summary>
        /// S7F25(Formatted PP Reqeust)가 수신될 경우 발생하는 이벤트입니다
        /// </summary>
        /// <param name="systemBytes"></param>
        /// <param name="ppid"></param>
        private void GemDriver_OnReceivedFmtPPRequest(uint systemBytes, string ppid)
        {
            FmtPPCollection fmtPPCollection = new FmtPPCollection(ppid);
            bool result = false; //= ProcessProgramParsing(ppid, false, out fmtPPCollection);
            //WriteLog(LogLevel.Information, "Received FMT PP Request");

            m_gemDriver.ReplyFmtPPRequestAck(systemBytes, ppid, fmtPPCollection, result);
        }

        /// <summary>
        /// Host에서 S7F23(Formatted PP Send)가 수신될 때 발생하는 이벤트입니다.
        /// </summary>
        /// <param name="systemBytes"></param>
        /// <param name="fmtPPCollection"></param>
        private void GemDriver_OnReceivedFmtPPSend(uint systemBytes, FmtPPCollection fmtPPCollection)
        {
            string lotText = string.Empty;

            lotText += $"[PPID={fmtPPCollection.PPID}]{Environment.NewLine}";

            foreach (FmtPPCCodeInfo ppcodeInfo in fmtPPCollection.Items)
            {
                lotText += $": [CCODE={ppcodeInfo.CommandCode}]{Environment.NewLine}";

                foreach (FmtPPItem ppitem in ppcodeInfo.Items)
                {
                    lotText += $":    [PPNAME={ppitem.PPName},FORMAT={ppitem.Format}]{Environment.NewLine},PPVALUE={ppitem.PPValue}";
                }
            }

            if (lotText.Length > 0)
            {
                lotText = lotText.Substring(0, lotText.Length - Environment.NewLine.Length);
            }

           // WriteLog(LogLevel.Information, $"OnReceivedFmtPPSend : {lotText}");
           
            //GLog.Add($"OnReceivedFmtPPSend : {lotText} ");

            m_gemDriver.ReplyFmtPPSendAck(systemBytes, (int)ACKC6.ErrorNotAccepted);
        }

        private void GemDriver_OnReceivedCurrentEPPDRequest(uint systemBytes)
        {
            List<string> ppids = new List<string>();

            //Add PP List to ppids

            //WriteLog(LogLevel.Information, "Received Current EPPD Request");

            m_gemDriver.ReplyCurrentEPPDRequestAck(systemBytes, ppids, false);
        }

        /// <summary>
        /// S2F17(Date Time Reqeust)가 수신될 경우 발생하는 이벤트입니다.
        /// </summary>
        /// <param name="systemBytes"></param>
        private void GemDriver_OnReceivedDateTimeRequest(uint systemBytes)
        {
            DateTime timeData = DateTime.Now; //시간 요청
            //WriteLog(LogLevel.Information, "Received Date Time Request");

            m_gemDriver.ReplyDateTimeRequest(systemBytes, timeData);
        }

        /// <summary>
        /// S2F31(Date Time Set Request)가 수신될 경우 발생하는 이벤트입니다.
        /// </summary>
        /// <param name="systemBytes"></param>
        /// <param name="timeData"></param>
        private void GemDriver_OnReceivedDateTimeSetRequest(uint systemBytes, DateTime timeData)
        {
			//WriteLog(LogLevel.Information, $"Received Date Time Set Request DateTime={timeData:yyyy-MM-dd HH:mm:ss.fff}");
			//Check 차후 기능 추가 필요

			try
			{
				SystemTime ServerTime = new SystemTime();

				ServerTime.wYear = (ushort)timeData.Year;
				ServerTime.wMonth = (ushort)timeData.Month;
                ServerTime.wDay = (ushort)timeData.Day;
                ServerTime.wHour = (ushort)timeData.Hour;
                ServerTime.wMinute = (ushort)timeData.Minute;
                ServerTime.wSecond = (ushort)timeData.Second;

				if(SetSystemTime(ServerTime) != 0)	m_gemDriver.ReplyDateTimeSetRequest(systemBytes, m_ack, timeData);
				else								m_gemDriver.ReplyDateTimeSetRequest(systemBytes, (int)GemDriverError.Disabled, timeData);
			}
			catch
			{
				m_gemDriver.ReplyDateTimeSetRequest(systemBytes, (int)ACKC6.ErrorNotAccepted, timeData);
			}

        }

        /// <summary>
        /// Host에서 S2F25(Loopback)이 수신될 때 발생하는 이벤트입니다.
        /// </summary>
        /// <param name="receiveData"></param>
        private void GemDriver_OnReceivedLoopback(List<byte> receiveData)
        {
            string strReceiveData = string.Empty;

            foreach (byte data in receiveData)
            {
                strReceiveData += data + " ";
            }

            if(strReceiveData.Length > 0)
            {
                strReceiveData = strReceiveData.Substring(0, strReceiveData.Length - 1);
            }

			//차후 필요하면 추가 및 작성
            //WriteLog(LogLevel.Information, $"Received Loopback Data={strReceiveData}");
        }

        /// <summary>
        /// Host에서 S1F13(Establish Communication)이 수신될 때 발생하는 이벤트입니다.
        /// </summary>
        /// <param name="mdln"></param>
        /// <param name="sofRev"></param>
        /// <returns></returns>
        private int GemDriver_OnReceivedEstablishCommunicationsRequest(string mdln, string sofRev)
        {
            //WriteLog(LogLevel.Information, "Received Establish Communication Request");

			// 차후 컨셉이 정해지면 리턴값은 m_ack로 변경하고 작성
            return (int)GemDriverError.Ok;
        }

        /// <summary>
        /// 사용자 정의 Message로 등록한 Stream, Function 중 Primary 메시지가 수신될 경우 발생하는 이벤트입니다.
        /// </summary>
        /// <param name="message"></param>
        private void GemDriver_OnUserPrimaryMessageReceived(SECSMessage message)
        {
            // 차후 컨셉이 정해지면 작성
        }

        /// <summary>
        /// 사용자 정의 Message로 등록한 Stream, Function 중 Secondary 메시지가 수신될 경우 발생하는 이벤트입니다.
        /// </summary>
        /// <param name="primaryMessage"></param>
        /// <param name="secondaryMessage"></param>
        private void GemDriver_OnUserSecondaryMessageReceived(SECSMessage primaryMessage, SECSMessage secondaryMessage)
        {
            //CYJ 2024/11/06
			OnSecondaryMessageReceive(primaryMessage, secondaryMessage);
        }

        private void GemDriver_OnReceivedUnknownMessage(SECSMessage message)
        {
            // 차후 컨셉이 정해지면 작성
        }

        private void GemDriver_OnInvalidMessageReceived(MessageValidationError error, SECSMessage message)
        {
           // 차후 컨셉이 정해지면 작성
        }

        /// <summary>
        /// UbiGEM Configuration 파일에 정의되지 않은 Remote Command 가 수신될 경우 발생합니다.
        /// </summary>
        /// <param name="remoteCommandInfo"></param>
        private void GemDriver_OnReceivedInvalidRemoteCommand(RemoteCommandInfo remoteCommandInfo)
        {
            // 차후 컨셉이 정해지면 작성
        }

        /// <summary>
        /// UbiGEM Configuration 파일에 정의되지 않은 Enhanced Remote Command 가 수신될 경우 발생합니다.
        /// </summary>
        /// <param name="remoteCommandInfo"></param>
        private void GemDriver_OnReceivedInvalidEnhancedRemoteCommand(EnhancedRemoteCommandInfo remoteCommandInfo)
        {
           // 차후 컨셉이 정해지면 작성
        }
        #endregion

		#region [Response Message Event]
        private void GemDriver_OnResponseTerminalRequest(int ack)
        {
            //WriteLog(LogLevel.Information, "Response Terminal Request");
        }

        /// <summary>
        /// S7F5(PP Request)를 발송 후 Host에서 S7F6이 수신될 때 발생하는 이벤트입니다.
        /// </summary>
        /// <param name="ppid"></param>
        /// <param name="ppbody"></param>
        private void GemDriver_OnResponsePPRequest(string ppid, List<byte> ppbody)
        {
           // WriteLog(LogLevel.Information, "Response PP Request");
        }

        /// <summary>
        /// S7F3(PP Send)를 발송 후 Host에서 S7F4가 수신될 때 발생하는 이벤트입니다.
        /// </summary>
        /// <param name="ack"></param>
        /// <param name="ppid"></param>
        private void GemDriver_OnResponsePPSend(int ack, string ppid)
        {
           // WriteLog(LogLevel.Information, "Response PP Send");
        }

        /// <summary>
        /// S7F1(PP Load Inquire)를 발송 후 Host에서 S7F2가 수신될 때 발생하는 이벤트입니다.
        /// </summary>
        /// <param name="ppgnt"></param>
        /// <param name="ppid"></param>
        private void GemDriver_OnResponsePPLoadInquire(int ppgnt, string ppid)
        {
            //WriteLog(LogLevel.Information, "Response PP Load Inquire");
        }

        /// <summary>
        /// S2F23(Formatted PP Request) 발송 후 Host에서 S2F24가 수신될 때 발생하는 이벤트입니다.
        /// </summary>
        /// <param name="fmtPPCollection"></param>
        private void GemDriver_OnResponseFmtPPRequest(FmtPPCollection fmtPPCollection)
        {
            //WriteLog(LogLevel.Information, "Response FMT PP Request");
        }

        /// <summary>
        /// S7F25(Formatted PP Send)를 발송 후 Host에서 S7F26이 수신될 때 발생하는 이벤트입니다.
        /// </summary>
        /// <param name="ack"></param>
        /// <param name="fmtPPCollection"></param>
        private void GemDriver_OnResponseFmtPPSend(int ack, FmtPPCollection fmtPPCollection)
        {
            //WriteLog(LogLevel.Information, "Response FMT PP Send");
        }

        /// <summary>
        /// S2F27(Formatted PP Verification Send)를 발송 후 Host에서 S2F28이 수신될 때 발생하는 이벤트입니다.
        /// </summary>
        /// <param name="fmtPPVerificationCollection"></param>
        private void GemDriver_OnResponseFmtPPVerification(FmtPPVerificationCollection fmtPPVerificationCollection)
        {
            //WriteLog(LogLevel.Information, "Response FMT Verification Ack");
        }

        /// <summary>
        /// S7F17(Date Time Reqeust)를 발송 후 Host에서 S7F18이 수신될 때 발생하는 이벤트입니다.
        /// </summary>
        /// <param name="timeData"></param>
        /// <returns></returns>
        private bool GemDriver_OnResponseDateTimeRequest(DateTime timeData)
        {
            bool result = true;

            //WriteLog(LogLevel.Information, $"Response Date Time Request DateTime={timeData:yyyy-MM-dd HH:mm:ss.fff}");

            return result;
        }

        /// <summary>
        /// S2F25(Loopback)을 발송 후 Host에서 S2F26이 수신될 때 발생하는 이벤트입니다.
        /// </summary>
        /// <param name="receiveData"></param>
        /// <param name="sendData"></param>
        private void GemDriver_OnResponseLoopback(List<byte> receiveData, List<byte> sendData)
        {
            bool result = false;

            if (receiveData.Count == sendData.Count)
            {
                int count = receiveData.Count;

                for (int i = 0; i < count; i++)
                {
                    if (receiveData[i] != sendData[i])
                    {
                        result = false;
                        break;
                    }
                }
            }

            //WriteLog(LogLevel.Information, $"Response Loopback : Receive Data={string.Join(",", receiveData)} : Send Data={string.Join(",", sendData)} : Result={result}");
        }

        /// <summary>
        /// S6F11(Event Report)의 Secondary Message(S6F12)가 수신될 경우 발생하는 이벤트입니다.
        /// </summary>
        /// <param name="ceid"></param>
        /// <param name="ack"></param>
        private void GemDriver_OnResponseEventReportAcknowledge(string ceid, int ack)
        {
			OnAckReceive(ceid,ack);
        }
        #endregion

		#region [Request Message Event]
        /// <summary>
        /// Variable 정보의 Update가 필요할 때 발생하는 이벤트입니다.
        /// </summary>
        /// <param name="updateType"></param>
        /// <param name="variables"></param>
        private void GemDriver_OnVariableUpdateRequest(VariableUpdateType updateType, List<VariableInfo> variables)
        {
            /* <OnVariableUpdateRequest Event의 경우>
             * 1. 호스트가 S1F3 Message를 Send 하였을 때
             * 2. 호스트가 S6F19 Message를 Send 하였을 때
             * 3. ReportCollectionEvent(string) API를 사용할 경우.
             */

            // List Type Variable의 데이터 설정 방법
            // VID=2000 이고 구조가 아래와 같고, n = 5, m = 4 인 경우
            // Ln DataList
            //    L3 DataInfo
            //       A DataID
            //       U1 SubDataCount
            //       Lm SubDataList
            //          L2 SubDataInfo
            //              A SubDataID
            //              U1 SubDataNo

            /*
            VariableInfo dataList = new VariableInfo() { VID = "2000", Format = SECSItemFormat.L, Name = "DataList" };

            for (int i = 0; i < 5; i++)
            {
                int subCount = 4;

                VariableInfo dataInfo = new VariableInfo() { VID = "", Format = SECSItemFormat.L, Name = "DataInfo" };

                dataList.ChildVariables.Add(dataInfo);

        		dataInfo.ChildVariables.Add(new VariableInfo() { VID = "", Format = SECSItemFormat.A, Name = "DataID", Value = "DataID" });
                dataInfo.ChildVariables.Add(new VariableInfo() { VID = "", Format = SECSItemFormat.U1, Name = "SubDataCount", Value = subCount });
                
                VariableInfo subDataList = new VariableInfo() { VID = "", Format = SECSItemFormat.L, Name = "SubDataList" };
		        dataInfo.ChildVariables.Add(subDataList);

		        for(int j = 0; j < subCount; j++)
		        {
			        VariableInfo subDataInfo = new VariableInfo() { VID = "", Format = SECSItemFormat.L, Name = "SubDataInfo" };
			        subDataList.ChildVariables.Add(subDataInfo);

                    subDataInfo.ChildVariables.Add(new VariableInfo() { VID = "", Format = SECSItemFormat.A, Name = "SubDataID", Value = "SubDataID" });

                    subDataInfo.ChildVariables.Add(new VariableInfo() { VID = "", Format = SECSItemFormat.U1, Name = "SubDataNo", Value = j });
		        }
	        }

            _gemDriver.SetVariable(dataList);
            */

            //foreach (VariableInfo variableInfo in variables)
            //{
            //    if(variableInfo.VID == DefinedV.Alarmset)
            //    {
            //        // Format 'L'의 ChildVariable 'n'개 값 설정 방법
            //        VariableInfo alarmSet = _gemDriver.Variables[DefinedV.Alarmset];
            //        VariableInfo alarmID;

            //        // 상위 Variable의 ChildVariables을 Clear
            //        alarmSet.ChildVariables.Clear();

            //        // 하위 Variable의 개수 만큼 상위 Variable ChildVariables에 추가
            //        foreach (long alid in _setAlarmList)
            //        {
            //            // 하위 Variable 생성 방법
            //            // 1. ugc file에서 정의한 Variable의 정보를 Copy해서 생성
            //            alarmID = _gemDriver.Variables[DefinedV.ALID].CopyTo();
            //            alarmID.Value = alid;

            //            // 2. 직접 Variable 객체를 생성
            //            alarmID = new VariableInfo()
            //            {
            //                VID = DefinedV.ALID,
            //                Name = "ALID",
            //                Format = SECSItemFormat.A,
            //                Length = 1,
            //                Value = alid
            //            };

            //            alarmSet.ChildVariables.Add(alarmID);
            //        }
            //    }
            //}

            //WriteLog(LogLevel.Information, "Variable Update Request");
        }

        /// <summary>
        /// 사용자 정의 GEM Message의 업데이트가 필요할 경우 발생합니다.
        /// </summary>
        /// <param name="message"></param>
        private void GemDriver_OnUserGEMMessageUpdateRequest(SECSMessage message)
        {
            //WriteLog(LogLevel.Information, "GEM Message Update Request");
        }

        /// <summary>
        /// Trace Data를 발송하기 위해 Variable의 Update가 필요한 경우 발생합니다.
        /// </summary>
        /// <param name="variables"></param>
        private void GemDriver_OnTraceDataUpdateRequest(List<VariableInfo> variables)
        {
            //WriteLog(LogLevel.Information, "Trace Data Update Request");
        }
        #endregion

		protected void UBIGEMDispose()
		{
            if (m_gemDriver != null)
            {
                m_gemDriver.Dispose();
                m_gemDriver = null;
            }
			GC.SuppressFinalize(this);
		}
        
        public void InputVid_Data(string strVid, string strValue)
        {
            foreach (VariableInfo vinfo in this.m_gemDriver.Variables.SV.Items)
            {
                if (vinfo.Name == strVid)
                {
                    if (vinfo.Format != UbiCom.Net.Structure.SECSItemFormat.A)
                        vinfo.Value = int.Parse(strValue);
                    else
                        vinfo.Value = strValue;

                }
            }
        }

        public void InputVid_Data(int nVid, string strValue)
        {
            foreach (VariableInfo vinfo in this.m_gemDriver.Variables.SV.Items)
            {
                if (vinfo.VID == nVid.ToString())
                {
                    if (vinfo.Format != UbiCom.Net.Structure.SECSItemFormat.A)
                        vinfo.Value = int.Parse(strValue);
                    else
                        vinfo.Value = strValue;

                }
            }
        }
		
		private string CheckValidationParameterItem(int level, EnhancedCommandParameterItem enhancedCommandParameterItem, RemoteCommandParameterResult paramResult)
        {
            string logText = ":";
            RemoteCommandParameterResult itemResult;

            for (int i = 0; i < level; i++)
            {
                logText += " ";
            }

            level++;

            if (enhancedCommandParameterItem.Format == SECSItemFormat.L)
            {
                logText += $"[CPNAME={enhancedCommandParameterItem.Name},Format={enhancedCommandParameterItem.Format},CEPVAL={enhancedCommandParameterItem.Value}]{Environment.NewLine}";

                if (string.IsNullOrEmpty(enhancedCommandParameterItem.Name) == true)
                {
                    itemResult = new RemoteCommandParameterResult((int)CPACK.IllegalFormatSpecifiedForCPVAL);
                }
                else
                {
                    itemResult = new RemoteCommandParameterResult(enhancedCommandParameterItem.Name, (int)CPACK.IllegalFormatSpecifiedForCPVAL);
                }

                foreach (EnhancedCommandParameterItem item in enhancedCommandParameterItem.ChildParameterItem.Items)
                {
                    logText += CheckValidationParameterItem(level, item, itemResult);
                }
            }
            else
            {
                logText += $"[CPNAME={enhancedCommandParameterItem.Name},Format={enhancedCommandParameterItem.Format},CEPVAL={enhancedCommandParameterItem.Value}]{Environment.NewLine}";
                itemResult = new RemoteCommandParameterResult(enhancedCommandParameterItem.Name, (int)CPACK.IllegalFormatSpecifiedForCPVAL);
            }

            paramResult.ParameterListAck.Add(itemResult);

            return logText;
        }
	}
}
