using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.ComponentModel;
using System.Threading;

namespace H_MarsSSD
{
	public class CommFTV_Sim
	{
		public	delegate	void					TCPDataLogEventHandler(string strData);
		public	event		TCPDataLogEventHandler	EventTcpDataLog;

		#region Enum

		#endregion

		#region Variable
		private int				m_nBuffer		= 1024;
		private bool			m_bIsConnect	= false;
		private string			m_ETX			= "\r";
		private TcpClient		m_Client;
        private NetworkStream	m_Stream;
        private Thread			m_ReceiveThread;
		private int				m_FtvNo = -1;

		private Dictionary<string, CommFTV.tsRcvData> m_dicRcvData = new Dictionary<string, CommFTV.tsRcvData>();

		private bool m_TriggerRcvCheck = false;
		private CommFTV.FTVResult m_RstEmptyCheck = CommFTV.FTVResult.OK; 
		public bool TriggerRcvCheck
		{
			get { return m_TriggerRcvCheck; }
		}
		public CommFTV.FTVResult RstEmptyCheck
		{
			get { return m_RstEmptyCheck; }
		}
		public void Dispose()
		{
			m_Stream?.Dispose();
			m_Client?.Close(); 

			if (m_ReceiveThread != null)	m_ReceiveThread.Abort();
		}



		#endregion

		#region private Function
		private void	AddCommLog(CommFTV.tsRcvData teData)
		{
			try
			{
				string		str = teData.strRcvData;
				CommFTV.IV4Command	cmd = teData.RcvCmd;

				switch (cmd)
				{
					case CommFTV.IV4Command.Trigger:
						break;
					case CommFTV.IV4Command.GetResult:
						break;
					case CommFTV.IV4Command.TriggerAndGetResult:
						break;
					case CommFTV.IV4Command.GetProgramNumber:
						break;
					case CommFTV.IV4Command.SetProgramNumber:
						break;
					case CommFTV.IV4Command.GetProgramName:
						break;
					case CommFTV.IV4Command.SetProgramName:
						break;
					default:
						break;
				}

				string strLogData = str.Replace(",", "/");
				GLog.Add($"FTV Rcv:{cmd} Data:{strLogData}");
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
		#endregion

		#region Public Function
		public CommFTV_Sim()
		{
		}

		public void		Init(int nNum)
		{
			try
			{
				m_FtvNo = nNum;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public void		Update()
		{
			try
			{
				
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public void		ConnectReTry()
		{
			try
			{	string	ip;
				int		nPort;
				if (m_FtvNo == 0)
				{
					ip = SDm.Read(eSDM.SmartVision1IP);
					nPort = int.Parse(SDm.Read(eSDM.SmartVision1Port));
				}
				else
				{
					ip = SDm.Read(eSDM.SmartVision2IP);
					nPort = int.Parse(SDm.Read(eSDM.SmartVision2Port));
				}
				GLog.Add($"FTV Vision ConnectReTry() IP:{ip} Port:{nPort} Connect:{IsConnect()} m_Client:{m_Client} m_Stream:{m_Stream}");
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public	bool	IsConnect()
		{
			try
			{
				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}
		
		public int		Send(CommFTV.IV4Command command, params string[] parameters)
		{
			try
			{
				if (IsConnect() == false)
				{
					GLog.Add($"FTV Send Fail(-1) CMD:{command} Connect:{IsConnect()}");
					return -1;
				}

				string parameter		= parameters.FirstOrDefault() ?? "";
				string commandString	= "";

				switch (command)
				{
					case CommFTV.IV4Command.Trigger:
					case CommFTV.IV4Command.TriggerAndGetResult:
						{
							m_TriggerRcvCheck = true;
							m_RstEmptyCheck = CommFTV.FTVResult.OK;

							commandString = EnumHelper.GetDescription(command);
						}
						break;
					case CommFTV.IV4Command.GetResult:
					case CommFTV.IV4Command.GetProgramName:
					case CommFTV.IV4Command.GetProgramNumber:
					case CommFTV.IV4Command.ReadOperatingStatus:
					case CommFTV.IV4Command.ReadSensorStatus:
					case CommFTV.IV4Command.ReadErrorNumber:
					case CommFTV.IV4Command.ReadWarningNumber:
					case CommFTV.IV4Command.ClearWarning:
						commandString = EnumHelper.GetDescription(command);
						break;

					case CommFTV.IV4Command.SetProgramNumber:
						if (string.IsNullOrEmpty(parameter) == true)
						{
							GLog.Add($"FTV Send Fail(-2) CMD:{command} CMDStr:{commandString} Param:{parameter}");
							return -2;
						}
						commandString = $"{EnumHelper.GetDescription(command)}{parameter}";
						break;

					case CommFTV.IV4Command.SetProgramName:
						if (string.IsNullOrEmpty(parameter) == true)
						{
							GLog.Add($"FTV Send Fail(-2) CMD:{command} CMDStr:{commandString} Param:{parameter}");
							return -2;
						}
						commandString = $"{EnumHelper.GetDescription(command)}{parameter}";
						break;

					default:
						{
							GLog.Add($"FTV Send Fail(-3) CMD:{command} CMDStr:{commandString} Param:{parameter}");
							return -3;
						}
				}

				if (EventTcpDataLog != null)	EventTcpDataLog($"FTV Send CMD:{command} CMDStr:{commandString} Param:{parameter}");

				commandString += m_ETX;
				byte[] sendBytes = Encoding.ASCII.GetBytes(commandString);

                string str = $"FTV Send CMD:{command} CMDStr:{commandString.Replace(",", "/")} Param:{parameter}";
                GLog.Add(str.Replace("\r", " "));
				
				return 1;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}

		private void	ReceiveData()
		{
			try
			{
				byte[] by_rcv_data = new byte[m_nBuffer];
				while (m_Client.Connected == true)
				{
					Thread.Sleep(10);

					if (m_Client == null || m_Stream == null) continue;

					if (m_Stream.DataAvailable)
					{
						int			bytesRead	= m_Stream.Read(by_rcv_data, 0, by_rcv_data.Length);
						string		response	= Encoding.ASCII.GetString(by_rcv_data, 0, bytesRead);
						string		strkey		= "";
						CommFTV.tsRcvData	RcvData		= new CommFTV.tsRcvData();
						
						if (ReceiveData_Parsing(by_rcv_data, ref RcvData) == false)
						{
							if (EventTcpDataLog != null)	EventTcpDataLog($"RCV - ERROR : {RcvData.strRcvData}");
							string strLogData = RcvData.strRcvData.Replace(",", "/");
							GLog.Add(string.Format("RCV - ERROR : {0}", strLogData));
							return;
						}

						if (EventTcpDataLog != null)	EventTcpDataLog(RcvData.strRcvData);

						AddCommLog(RcvData);

						switch (RcvData.RcvCmd)
						{
							case CommFTV.IV4Command.Trigger:
							case CommFTV.IV4Command.GetProgramNumber:
							case CommFTV.IV4Command.SetProgramNumber:
							case CommFTV.IV4Command.GetProgramName:
							case CommFTV.IV4Command.SetProgramName:
							case CommFTV.IV4Command.ClearWarning:
								{
									strkey = RcvData.RcvCmd.ToString();
									if	(m_dicRcvData.ContainsKey(strkey) == true)	m_dicRcvData[strkey] = RcvData;
									else											m_dicRcvData.Add(strkey, RcvData);
								}
								break;
							case CommFTV.IV4Command.GetResult:
							case CommFTV.IV4Command.TriggerAndGetResult:
								{
									m_TriggerRcvCheck = true;
									m_RstEmptyCheck = CommFTV.FTVResult.OK;
								}
								break;

							case CommFTV.IV4Command.ReadErrorNumber:
								{
									strkey = RcvData.RcvCmd.ToString();
									if	(m_dicRcvData.ContainsKey(strkey) == true)	m_dicRcvData[strkey] = RcvData;
									else											m_dicRcvData.Add(strkey, RcvData);

									G.AlmMng.ADD(ALM.ERROR_FTV_ERROR_CHECK, $"ERROR CODE {RcvData.strSplitData[0]}");
								}
								break;
							case CommFTV.IV4Command.ReadWarningNumber:
								{
									strkey = RcvData.RcvCmd.ToString();
									if	(m_dicRcvData.ContainsKey(strkey) == true)	m_dicRcvData[strkey] = RcvData;
									else											m_dicRcvData.Add(strkey, RcvData);

									G.AlmMng.ADD(ALM.ERROR_FTV_WARNING_CHECK, $"ERROR CODE {RcvData.strSplitData[0]}");
								}
								break;

							case CommFTV.IV4Command.ReadThreshold:
								break;
							case CommFTV.IV4Command.WriteThreshold:
								break;
							case CommFTV.IV4Command.ReadMasterString:
								break;
							case CommFTV.IV4Command.WriteMasterString:
								break;
							case CommFTV.IV4Command.ReadMasterCharacterCount:
								break;
							case CommFTV.IV4Command.WriteMasterCharacterCount:
								break;
							case CommFTV.IV4Command.ReadFTPFileName:
								break;
							case CommFTV.IV4Command.WriteFTPFileName:
								break;
							case CommFTV.IV4Command.RegisterMasterImage:
								break;
							case CommFTV.IV4Command.ReadOperatingStatus:
								break;
							case CommFTV.IV4Command.ReadSensorStatus:
								break;
							case CommFTV.IV4Command.ReadSDCardCapacity:
								break;
							case CommFTV.IV4Command.StopSDCardStorage:
								break;
							case CommFTV.IV4Command.EnableAutoResultTransmission:
								break;
							default:
								break;
						}
					}
				}
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private bool	ReceiveData_Parsing(byte[] byRcvData, ref CommFTV.tsRcvData RcvData)
		{
			try
			{
				CommFTV.IV4Command	Cmd				= CommFTV.IV4Command.Unknown;
				string		strrcv_data				= Encoding.Default.GetString(byRcvData);
				string[]	strrcv_data_split_list;
				int nEtxIndex = strrcv_data.IndexOf(m_ETX);
				
				strrcv_data				= strrcv_data.Remove(nEtxIndex); // ETX 뒤로 삭제
				strrcv_data_split_list	= strrcv_data.Split(','); // , 기준으로 split

				if (strrcv_data_split_list.Length < 1)
				{
					G.AlmMng.ADD(ALM.ERROR_COMM_FTV_DATA, string.Format("Data Length Error. {0}", Encoding.Default.GetString(byRcvData)));
					return false;
				}
				
				if (String.IsNullOrWhiteSpace(strrcv_data_split_list[0]) == false)
				{
					for (CommFTV.IV4Command i = 0; i < CommFTV.IV4Command.Unknown; i++)
					{
						if (strrcv_data_split_list[0] == EnumHelper.GetDescription(i))
						{
							Cmd = i;
							break;
						}
					}
				}
				else
				{
					G.AlmMng.ADD(ALM.ERROR_COMM_FTV_DATA, string.Format("Data CMD Error. {0}", strrcv_data_split_list[0]));
					return false;
				}
				
				RcvData.RcvCmd			= Cmd;
				RcvData.strRcvData		= string.Format($"<{strrcv_data}><ETX>");
				RcvData.strSplitData	= new string[strrcv_data_split_list.Length];
				for (int i = 0; i < strrcv_data_split_list.Length; i++)
				{
					// i == 0은 Cmd
					RcvData.strSplitData[i] = strrcv_data_split_list[i];
				}
				
				return true;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}
		#endregion

		#region Receive Function
		public int RcvRead(CommFTV.IV4Command command, ref string strData)
		{
			try
			{
				string	strkey	= command.ToString();

				switch (command)
				{
					case CommFTV.IV4Command.Trigger:				break;
					case CommFTV.IV4Command.GetResult:				break;
					case CommFTV.IV4Command.TriggerAndGetResult:	break;
					case CommFTV.IV4Command.GetProgramNumber:		break;
					case CommFTV.IV4Command.SetProgramNumber:		break;
					case CommFTV.IV4Command.GetProgramName:			break;
					case CommFTV.IV4Command.SetProgramName:			break;
					case CommFTV.IV4Command.ReadOperatingStatus:	strData = "1";	 break;
					case CommFTV.IV4Command.Unknown:
						break;
					default:
						break;
				}

				m_dicRcvData.Remove(strkey);
				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}

		public int RcvGetProgramNumber(ref string strNumber)
		{
			try
			{
				strNumber = (SM.SHM.GetRecipeDataToInt(DEVICE.FTVJob)/1000).ToString("000");
				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}

		public int RcvGetProgramName(ref string strName)
		{
			try
			{
				strName = SM.SHM.GetRecipeData(DEVICE.ModuleType);
				
				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}

		public int RcvReadErrorNumber(ref string strError)
		{
			try
			{
				strError = 0.ToString("000");

				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}

		public int RcvReadWarningNumber(ref string strWarning)
		{
			try
			{
				strWarning	= 0.ToString("000");
				
				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -9999;
			}
		}
		#endregion
	}
}
