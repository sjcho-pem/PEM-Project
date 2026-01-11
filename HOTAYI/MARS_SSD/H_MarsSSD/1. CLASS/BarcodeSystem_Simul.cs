using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Threading;
using Cognex.DataMan.SDK;
using Cognex.DataMan.SDK.Discovery;
using Cognex.DataMan.SDK.Utils;
using System.IO;

namespace H_MarsSSD
{
	public class BarcodeSystem_Simul
	{ 
		#region private Variables
		private ResultCollector			m_Results				= null;
		private ISystemConnector		m_Connector				= null;
		private SerSystemDiscoverer		m_SerSystemDiscoverer	= null;
		private DataManSystem			m_System				= null;
		private SerSystemDiscoverer.SystemInfo	m_SysInfo		= null;
		private List<string>			m_ltResultData			= new List<string>();
		
		private	string					m_strComPort			= "COM1";
	
		private bool					m_bConnect				= false; // 처음 연결 한번 성공 했을떄 true
		private bool					m_bAutoReconnected		= false; // 처음 연결 성공 후 끊겼을때 재연결 시도...
		private bool					m_bTriggerFlag			= false; // 중복 호출 차단 플래그,

		private const string			TYPE					= "DM152";
		public string DeviceName
		{
			get { return TYPE; }
		}

		private const bool				AUTO_RECONNECT_USESKIP	= true;
		private const bool				KEEP_ALIVE_OPTION		= true;

		private string					m_ResultData			= "";
		private string					m_CurPos				= "";
		
		public Image					m_Images				= null;
		public Image					m_LiveImages			= null;
		public List<string>				m_ltImagesGraphics		= new List<string>();
		#endregion

		#region Events
		public delegate void	ResultEventHandler(string BarcodeData);
		public event			ResultEventHandler ResultEventData;
		#endregion

		public BarcodeSystem_Simul(string strComPort, string Pos)
		{
			m_CurPos = Pos;
			m_strComPort	= strComPort.ToUpper();

			m_SerSystemDiscoverer = new SerSystemDiscoverer();
			m_SerSystemDiscoverer.SystemDiscovered += new SerSystemDiscoverer.SystemDiscoveredHandler(OnSerSystemDiscovered);
			//m_SerSystemDiscoverer.Discover();
		}

		private void OnSerSystemDiscovered(SerSystemDiscoverer.SystemInfo systemInfo)
		{
			try
			{
				if (systemInfo.Type == TYPE  &&  systemInfo.PortName == m_strComPort)
				{
					m_SysInfo	= systemInfo;  
					Connect();
				}
			}
			catch
			{

			}
		}
		
		public bool IsConnected()
		{
			//if (m_Connector == null)		return false;

			return true;
		}

		private void Connect()
		{
			if (m_SysInfo == null)			return;

			m_bConnect = false;

			try
			{

			}
			catch (Exception)
			{
				CleanupConnection();
			}

			m_bConnect = true;			
		}
				
		private void OnSystemConnected(object sender, EventArgs args)
		{
			// 연결 상태
			m_bAutoReconnected	= true;
			m_bTriggerFlag		= true;
		}

		private void OnSystemDisconnected(object sender, EventArgs args)
		{
			if (m_bConnect && AUTO_RECONNECT_USESKIP)
			{
				// 자동 재연결 시작....
				m_bAutoReconnected	= true;
			}
			else
			{
				RefreshSystem();
			}
		}
				
		private void OnKeepAliveResponseMissed(object sender, EventArgs args) // Keep Alive 사용시 호출됨. 
		{
			m_System.Disconnect();
		}
				
		private void Results_ComplexResultCompleted(object sender, ComplexResult e) // 바코드 리딩 성공시 호출됨.
		{
			string	str_parsing_data	= ParsingResultData(e);
			if (ResultEventData != null) 			ResultEventData(str_parsing_data);
			m_ResultData = str_parsing_data;
		}

			   
		private void Results_SimpleResultDropped(object sender, SimpleResult e)
		{
			// Alarm 추가 필요..                    
			//G.AlmMng.ADD(ALM , string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
		}

		private void CleanupConnection()
		{
			if (null != m_System)
			{
				m_System.SystemConnected			-= OnSystemConnected;
				m_System.SystemDisconnected			-= OnSystemDisconnected;
				m_System.KeepAliveResponseMissed	-= OnKeepAliveResponseMissed;
			}

			if (m_Results != null)
			{
				m_Results.ComplexResultCompleted	-= Results_ComplexResultCompleted;
				m_Results.SimpleResultDropped		-= Results_SimpleResultDropped;            
			}

			m_SysInfo	= null;
			m_Connector = null;
			m_System = null;
		}

		private string ParsingResultData(ComplexResult complexResult)
		{

			string read_result = null;
			int result_id = -1;
			ResultTypes collected_results = ResultTypes.None;

			foreach (var simple_result in complexResult.SimpleResults)
			{
				collected_results |= simple_result.Id.Type;

				switch (simple_result.Id.Type)
				{
					//case ResultTypes.ReadXml:
					//    read_result = GetReadStringFromResultXml(simple_result.GetDataAsString());
					//    result_id = simple_result.Id.Id;
					//    break;

					case ResultTypes.ReadString:
						read_result = simple_result.GetDataAsString();
						result_id = simple_result.Id.Id;
						break;
				}
			}
			
			return read_result;
		}

		private string GetReadStringFromResultXml(string resultXml)
		{
			try
			{
				//XmlDocument doc = new XmlDocument();

				//doc.LoadXml(resultXml);

				//XmlNode full_string_node = doc.SelectSingleNode("result/general/full_string");

				//if (full_string_node != null && _system != null && _system.State == Cognex.DataMan.SDK.ConnectionState.Connected)
				//{
				//    XmlAttribute encoding = full_string_node.Attributes["encoding"];
				//    if (encoding != null && encoding.InnerText == "base64")
				//    {
				//        if (!string.IsNullOrEmpty(full_string_node.InnerText))
				//        {
				//            byte[] code = Convert.FromBase64String(full_string_node.InnerText);
				//            return _system.Encoding.GetString(code, 0, code.Length);
				//        }
				//        else
				//        {
				//            return "";
				//        }
				//    }

				//    return full_string_node.InnerText;
				//}
			}
			catch
			{
			}

			return "";
		}

		private void RefreshSystem()
		{
			// 연결 가능한 디바이스를 못 찾았을때 사용 함.
			try
			{
				if (m_SerSystemDiscoverer.IsDiscoveryInProgress)			return;

				m_SysInfo	= null;
				m_ResultData = "";
			}
			finally
			{
				m_SerSystemDiscoverer.Discover();
			}
		}

		#region public functions
		public bool IsConnect()
		{
			//if (m_Connector == null) return false;
			return true;
		}

		public int IsState()
		{
			/*Disconnected = 0,
			  Connecting = 1,
			  Connected = 2,
			  Disconnecting = 3*/
			  
			if (m_Connector == null)			return 0;
			return 2;
		}

		public void Dispose()
		{
			m_bConnect	= false;

			if (null != m_System  &&  m_System.State == ConnectionState.Connected)
			{
				m_System.Disconnect();
				CleanupConnection();
			}
			if (m_SerSystemDiscoverer != null)	m_SerSystemDiscoverer.Dispose();
			
			m_SerSystemDiscoverer = null;
			//_system.Dispose();
		}

		public bool TriggerOn()
		{ 
			try
			{
				if (m_bTriggerFlag == true  ||  m_System == null)			return false;
				m_bTriggerFlag	= true;

				m_ResultData = "";
				m_System.SendCommand("TRIGGER ON");//셀프(내부)
				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public bool TriggerOff()
		{
			try
			{ 
				if (m_bTriggerFlag == false  ||  m_System == null)			return false;
				m_bTriggerFlag = false;

				m_System.SendCommand("TRIGGER OFF");//단일(외부)
				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}
				
		public void TriggerOnTriggerOnContinuous()
		{
			try
			{
				if (m_bTriggerFlag == true  ||  m_System == null)			return;
				m_bTriggerFlag	= true;

				m_ltResultData.Clear();
				m_System.SendCommand("SET TRIGGER.TYPE 4");//셀프(내부)
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		
		public void TriggerOffContinuous()
		{
			try
			{ 
				if (m_bTriggerFlag == false  ||  m_System == null)			return;
				m_bTriggerFlag = false;

				m_System.SendCommand("SET TRIGGER.TYPE 0");//단일(외부)
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public void SetTriggerInterval(int nTimeUs)
		{
			try
			{ 
				if (m_System == null) return;

				m_System.SendCommand("SET CAMERA.INTERVAL-US " + nTimeUs.ToString());//단위:us
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
		
		public List<string> GetResultDataList()
		{
			List<string>  ltResultData = new List<string>();
			int nMaxCount = 31;
			for (int i = 0; i < nMaxCount; i++)
			{
				ltResultData.Add($"Sim_Barcode_{i}");
			}
			return ltResultData;
		}
		
		public string GetResultData()
		{
			string temp = "Barcode" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
 
			return temp;
		}
		

		public void LiveOn()
		{
			try
			{
				m_System.SendCommand("SET LIVEIMG.MODE 2");

				m_System.BeginGetLiveImage(
					ImageFormat.jpeg,
					ImageSize.Sixteenth,
					ImageQuality.Medium,
					OnLiveImageArrived,
					null);
			}
			catch(Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public void LiveOff()
		{
			try
			{
				m_System.SendCommand("SET LIVEIMG.MODE 0");
			}
			catch(Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
		

		private void OnLiveImageArrived(IAsyncResult result)
		{
			try
			{
				m_LiveImages = m_System.EndGetLiveImage(result);
				UI.ModalBCRViewer.UpdateLiveImage(m_LiveImages);

				if (UI.ModalBCRViewer.IsLive == true)
				{
					m_System.BeginGetLiveImage(
						ImageFormat.jpeg,
						ImageSize.Sixteenth,
						ImageQuality.Medium,
						OnLiveImageArrived,
					null);
				}
			}
			catch(Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		/// <summary>
		/// 시퀀스 내에서 실시간 호출 해야됨.
		/// RefreshSystem() : 해당 디바이스 없을때 연결 시도함.
		/// Connect() : 연결된 상태일때 끊겼을때 재연결 시도함. ex)케이블분리 or 디바이스 Disconnect 발생시..
		/// </summary>
		public void ConnectChecking()
		{
			try
			{
			}
			catch (Exception)
            {
            }
		} 
		
		public void SystemConnect() // 매뉴얼로 사용시....
		{
			if (m_System != null)			m_System.Connect();
			else							m_SerSystemDiscoverer.Discover();
		}
		#endregion

	}
}
