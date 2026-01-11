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
	public partial class CommFTV
	{
		public	delegate	void					TCPDataLogEventHandler(string strData);
		public	event		TCPDataLogEventHandler	EventTcpDataLog;

		#region Enum
		
		public  enum IV4Command
		{
			[DefaultValueAttribute("T1")]
			Trigger,
			[DefaultValueAttribute("RT")]
			GetResult,
			[DefaultValueAttribute("T2")]
			TriggerAndGetResult,
			[DefaultValueAttribute("PR")]
			GetProgramNumber,
			[DefaultValueAttribute("PW")]
			SetProgramNumber,
			[DefaultValueAttribute("PNR")]
			GetProgramName,
			[DefaultValueAttribute("PNW")]
			SetProgramName,
			[DefaultValueAttribute("DR")]
			ReadThreshold,
			[DefaultValueAttribute("DW")]
			WriteThreshold,
			[DefaultValueAttribute("CR")]
			ReadMasterString,
			[DefaultValueAttribute("CW")]
			WriteMasterString,
			[DefaultValueAttribute("CNR")]
			ReadMasterCharacterCount,
			[DefaultValueAttribute("CNW")]
			WriteMasterCharacterCount,
			[DefaultValueAttribute("FNR")]
			ReadFTPFileName,
			[DefaultValueAttribute("FNW")]
			WriteFTPFileName,
			[DefaultValueAttribute("MR")]
			RegisterMasterImage,
			[DefaultValueAttribute("RM")]
			ReadOperatingStatus,
			[DefaultValueAttribute("SR")]
			ReadSensorStatus,
			[DefaultValueAttribute("RER")]
			ReadErrorNumber,
			[DefaultValueAttribute("WR")]
			ReadWarningNumber,
			[DefaultValueAttribute("WC")]
			ClearWarning,
			[DefaultValueAttribute("SDR")]
			ReadSDCardCapacity,
			[DefaultValueAttribute("SDS")]
			StopSDCardStorage,
			[DefaultValueAttribute("OE")]
			EnableAutoResultTransmission,
			[DefaultValueAttribute("")]
			Unknown,
		}

		public enum FTVResult
		{
			[DefaultValueAttribute("ING")]
			ING,
			[DefaultValueAttribute("DATA NG")]
			DATA_NG,
			[DefaultValueAttribute("RESULT NG")]
			RESULT_NG,
			[DefaultValueAttribute("OK")]
			OK,
		}

		public	struct tsRcvData
		{
			public	IV4Command	RcvCmd;
			public	string		strRcvData;
			public	string[]	strSplitData;
		}
		#endregion

		#region Variable
		private const int		m_nBuffer		= 1024;
		private bool			m_bIsConnect	= false;
		private string			m_ETX			= "\r";
		private TcpClient		m_Client;
        private NetworkStream	m_Stream;
        private Thread			m_ReceiveThread;
		private bool			m_bReceiveShould = true;
		private int				m_FtvNo = -1;

		private Dictionary<string, tsRcvData> m_dicRcvData = new Dictionary<string, tsRcvData>();

		private bool m_TriggerRcvCheck = false;
		private FTVResult m_RstEmptyCheck = FTVResult.OK; 
		public bool TriggerRcvCheck
		{
			get { return m_TriggerRcvCheck; }
		}
		public FTVResult RstEmptyCheck
		{
			get { return m_RstEmptyCheck; }
		}
		#endregion

		#region private Function
		private void	AddCommLog(tsRcvData teData)
		{
			try
			{
				string		str = teData.strRcvData;
				IV4Command	cmd = teData.RcvCmd;

				switch (cmd)
				{
					case IV4Command.Trigger:
						break;
					case IV4Command.GetResult:
						break;
					case IV4Command.TriggerAndGetResult:
						break;
					case IV4Command.GetProgramNumber:
						break;
					case IV4Command.SetProgramNumber:
						break;
					case IV4Command.GetProgramName:
						break;
					case IV4Command.SetProgramName:
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
		public CommFTV()
		{
		}

		~CommFTV()
		{
			m_bReceiveShould = false;

			if (m_ReceiveThread != null)	m_ReceiveThread.Abort();
		}
		public void Dispose()
		{
			m_Stream?.Dispose();
			m_Client?.Close(); 
			m_bReceiveShould = false;

			if (m_ReceiveThread != null)	m_ReceiveThread.Abort();
		}


		public void		Init(int nNum)
		{
			try
			{
				m_FtvNo = nNum;
				string	ip;
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

				m_Client = new TcpClient(ip, nPort);
				m_Stream = m_Client.GetStream();

				m_ReceiveThread = new Thread(ReceiveData);
				m_ReceiveThread.IsBackground = true;
				m_ReceiveThread.Start();
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
				if	(m_Client != null)	m_bIsConnect = m_Client.Connected;
				else					m_bIsConnect = false;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public void		ConnectReTry()
		{
			try
			{
				string	ip;
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

				//m_ReceiveThread.Abort();

				if (m_Client != null)
				{
					if (IsConnect() == true) m_Client.Dispose();
					m_Client = null;
					m_Stream = null;
				}

				m_Client = new TcpClient(ip, nPort);
				m_Stream = m_Client.GetStream();

                if (m_ReceiveThread == null)
                {
                    m_ReceiveThread = new Thread(ReceiveData);
                    m_ReceiveThread.Start();
                }
                else if (m_ReceiveThread.IsAlive == false)
                {
                    m_ReceiveThread.Abort();
                    m_ReceiveThread = new Thread(ReceiveData);
                    m_ReceiveThread.Start();
                }

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
				return m_bIsConnect;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}
		
		public int		Send(IV4Command command, params string[] parameters)
		{
			try
			{
				if (IsConnect() == false)
				{
					GLog.Add($"FTV Send Fail(-1) CMD:{command} Connect:{IsConnect()}");
					return -1;
				}

				if (m_Stream == null)
				{
					GLog.Add($"FTV Send Fail(-1) CMD:{command} Stream:{m_Stream}");
					return -1;
				}

				string parameter		= parameters.FirstOrDefault() ?? "";
				string commandString	= "";

				switch (command)
				{
					case IV4Command.Trigger:
					case IV4Command.TriggerAndGetResult:
						m_TriggerRcvCheck = false;
						m_RstEmptyCheck = FTVResult.ING;
						commandString = EnumHelper.GetDescription(command);
						break;
					case IV4Command.GetResult:
					case IV4Command.GetProgramName:
					case IV4Command.GetProgramNumber:
					case IV4Command.ReadOperatingStatus:
					case IV4Command.ReadSensorStatus:
					case IV4Command.ReadErrorNumber:
					case IV4Command.ReadWarningNumber:
					case IV4Command.ClearWarning:
						commandString = EnumHelper.GetDescription(command);
						break;

					case IV4Command.SetProgramNumber:
						if (string.IsNullOrEmpty(parameter) == true)
						{
							GLog.Add($"FTV Send Fail(-2) CMD:{command} CMDStr:{commandString} Param:{parameter}");
							return -2;
						}
						commandString = $"{EnumHelper.GetDescription(command)},{parameter}";
						break;

					case IV4Command.SetProgramName:
						if (string.IsNullOrEmpty(parameter) == true)
						{
							GLog.Add($"FTV Send Fail(-2) CMD:{command} CMDStr:{commandString} Param:{parameter}");
							return -2;
						}
						commandString = $"{EnumHelper.GetDescription(command)},{parameter}";
						break;

					default:
						{
							GLog.Add($"FTV Send Fail(-3) CMD:{command} CMDStr:{commandString} Param:{parameter}");
							return -3;
						}
				}

				if (EventTcpDataLog != null)	EventTcpDataLog($"FTV Vision Send CMD:{command} CMDStr:{commandString} Param:{parameter}");
				
				commandString += m_ETX;
				byte[] sendBytes = Encoding.ASCII.GetBytes(commandString);
				m_Stream.Write(sendBytes, 0, sendBytes.Length);
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
				while (m_bReceiveShould == true)
				{
					Thread.Sleep(10);

					if (m_Client == null || m_Stream == null) continue;

					if (m_Client.Connected == false) continue;

					if (m_Stream.DataAvailable)
					{
						int			bytesRead	= m_Stream.Read(by_rcv_data, 0, by_rcv_data.Length);
						string		response	= Encoding.ASCII.GetString(by_rcv_data, 0, bytesRead);
						string		strkey		= "";
						tsRcvData	RcvData		= new tsRcvData();
						
						if (ReceiveData_Parsing(by_rcv_data, ref RcvData) == false)
						{
							if (EventTcpDataLog != null)	EventTcpDataLog($"RCV - ERROR : {RcvData.strRcvData}");
							string strLogData = RcvData.strRcvData.Replace(",", "/");
							GLog.Add(string.Format("RCV - ERROR : {0}", strLogData));
							break;
						}

						if (EventTcpDataLog != null)	EventTcpDataLog(RcvData.strRcvData);

						AddCommLog(RcvData);

						switch (RcvData.RcvCmd)
						{
							case IV4Command.Trigger:
							case IV4Command.GetProgramNumber:
							case IV4Command.SetProgramNumber:
							case IV4Command.GetProgramName:
							case IV4Command.SetProgramName:
							case IV4Command.ClearWarning:
							case IV4Command.ReadOperatingStatus:
								{
									strkey = RcvData.RcvCmd.ToString();
									if	(m_dicRcvData.ContainsKey(strkey) == true)	m_dicRcvData[strkey] = RcvData;
									else											m_dicRcvData.Add(strkey, RcvData);
								}
								break;
							case IV4Command.GetResult:
							case IV4Command.TriggerAndGetResult:
								{
									m_TriggerRcvCheck = true;
									

									// 결과 데이터 포맷
									// RT,00047,OK,01,OK,0000098,02,OK,0000100,03,OK,0000100,04,OK,0000093,05,OK,0000099,.....
									// CMD,촬영이미지,결과,  인덱스,결과,결과률,인덱스,결과,결과률,인덱스,결과,결과률,......
									int nProdExistRstIdx	= 4; // 제품 유무 검사 순번, 4번째 인덱스부터 제품별 결과

									// 결과 값이 제품 인덱스에 맞지 않게 들어올 경우 알람
									// ex) 4X6 제품 결과 값은 Length가 75(유무 검사),147(유무 검사 + 180도 돌아감 검사) 값이 들어와야 한다\
									
									int nRow = SM.SHM.GetRecipeDataToInt(DEVICE.TrayRowCnt) / 1000;
									int nCol = SM.SHM.GetRecipeDataToInt(DEVICE.TrayColCnt) / 1000;
                                    
									if (RcvData.strSplitData.Contains("NG"))
									{
										m_RstEmptyCheck = FTVResult.RESULT_NG;
										//G.AlmMng.ADD(ALM.ERROR_SEQ_FIRST_TRAY_NEED_EMPTY);
										break;
									}

									m_RstEmptyCheck = FTVResult.OK;
								}
								break;

							case IV4Command.ReadErrorNumber:
								{
									strkey = RcvData.RcvCmd.ToString();
									if	(m_dicRcvData.ContainsKey(strkey) == true)	m_dicRcvData[strkey] = RcvData;
									else											m_dicRcvData.Add(strkey, RcvData);

									G.AlmMng.ADD(ALM.ERROR_FTV_ERROR_CHECK, $"ERROR CODE {RcvData.strSplitData[0]}");
								}
								break;
							case IV4Command.ReadWarningNumber:
								{
									strkey = RcvData.RcvCmd.ToString();
									if	(m_dicRcvData.ContainsKey(strkey) == true)	m_dicRcvData[strkey] = RcvData;
									else											m_dicRcvData.Add(strkey, RcvData);

									G.AlmMng.ADD(ALM.ERROR_FTV_WARNING_CHECK, $"ERROR CODE {RcvData.strSplitData[0]}");
								}
								break;

							case IV4Command.ReadThreshold:
								break;
							case IV4Command.WriteThreshold:
								break;
							case IV4Command.ReadMasterString:
								break;
							case IV4Command.WriteMasterString:
								break;
							case IV4Command.ReadMasterCharacterCount:
								break;
							case IV4Command.WriteMasterCharacterCount:
								break;
							case IV4Command.ReadFTPFileName:
								break;
							case IV4Command.WriteFTPFileName:
								break;
							case IV4Command.RegisterMasterImage:
								break;
							case IV4Command.ReadSensorStatus:
								break;
							case IV4Command.ReadSDCardCapacity:
								break;
							case IV4Command.StopSDCardStorage:
								break;
							case IV4Command.EnableAutoResultTransmission:
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
			finally
			{
				GLog.Add($"CommFTV ReceiveData End.");
			}
		}

		private bool	ReceiveData_Parsing(byte[] byRcvData, ref tsRcvData RcvData)
		{
			try
			{
				IV4Command	Cmd						= IV4Command.Unknown;
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
					for (IV4Command i = 0; i < IV4Command.Unknown; i++)
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
		public int RcvRead(IV4Command command, ref string strData)
		{
			try
			{
				string	strkey	= command.ToString();

				if (m_dicRcvData.ContainsKey(strkey) == false)			return 0;
				if (m_dicRcvData[strkey].strSplitData.Length != 2  ||
					string.IsNullOrWhiteSpace(m_dicRcvData[strkey].strSplitData[0]) == true)
				{
					G.AlmMng.ADD(ALM.ERROR_COMM_FTV_DATA, m_dicRcvData[strkey].strRcvData);
					return -1;
				}

				strData	= m_dicRcvData[strkey].strSplitData[1];
				
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
				string	strkey	= IV4Command.GetProgramNumber.ToString();

				if (m_dicRcvData.ContainsKey(strkey) == false)			return 0;
				if (m_dicRcvData[strkey].strSplitData.Length != 2  ||
					string.IsNullOrWhiteSpace(m_dicRcvData[strkey].strSplitData[0]) == true)
				{
					G.AlmMng.ADD(ALM.ERROR_COMM_FTV_DATA, m_dicRcvData[strkey].strRcvData);
					return -1;
				}

				strNumber	= m_dicRcvData[strkey].strSplitData[1];
				
				m_dicRcvData.Remove(strkey);
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
				string	strkey	= IV4Command.GetProgramName.ToString();

				if (m_dicRcvData.ContainsKey(strkey) == false)			return 0;
				if (m_dicRcvData[strkey].strSplitData.Length != 1  ||
					string.IsNullOrWhiteSpace(m_dicRcvData[strkey].strSplitData[0]) == true)
				{
					G.AlmMng.ADD(ALM.ERROR_COMM_FTV_DATA, m_dicRcvData[strkey].strRcvData);
					return -1;
				}

				strName	= m_dicRcvData[strkey].strSplitData[0];
				
				m_dicRcvData.Remove(strkey);
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
				string	strkey	= IV4Command.ReadErrorNumber.ToString();

				if (m_dicRcvData.ContainsKey(strkey) == false)			return 0;
				if (m_dicRcvData[strkey].strSplitData.Length != 1  ||
					string.IsNullOrWhiteSpace(m_dicRcvData[strkey].strSplitData[0]) == true)
				{
					G.AlmMng.ADD(ALM.ERROR_COMM_FTV_DATA, m_dicRcvData[strkey].strRcvData);
					return -1;
				}

				strError	= m_dicRcvData[strkey].strSplitData[0];
				
				m_dicRcvData.Remove(strkey);
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
				string	strkey	= IV4Command.ReadWarningNumber.ToString();

				if (m_dicRcvData.ContainsKey(strkey) == false)			return 0;
				if (m_dicRcvData[strkey].strSplitData.Length != 1  ||
					string.IsNullOrWhiteSpace(m_dicRcvData[strkey].strSplitData[0]) == true)
				{
					G.AlmMng.ADD(ALM.ERROR_COMM_FTV_DATA, m_dicRcvData[strkey].strRcvData);
					return -1;
				}

				strWarning	= m_dicRcvData[strkey].strSplitData[0];
				
				m_dicRcvData.Remove(strkey);
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
