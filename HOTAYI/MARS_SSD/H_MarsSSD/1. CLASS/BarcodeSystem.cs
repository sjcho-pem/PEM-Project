using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;

using Cognex.DataMan.SDK;
using Cognex.DataMan.SDK.Discovery;
using Cognex.DataMan.SDK.Utils;



namespace H_MarsSSD
{
    public class BarcodeSystem
    {
        #region private Variables
        private ResultCollector m_Results = null;
        private ISystemConnector m_Connector = null;
        //private SerSystemDiscoverer		m_SerSystemDiscoverer	= null;
        private DataManSystem m_System = null;
        private SerSystemDiscoverer.SystemInfo m_SysInfo = null;

        private string m_strComPort = "COM1";
        private string m_strDeviceName = "NONE";

        public string DeviceName { get { return m_strDeviceName; } }


        private bool m_bConnect = false; // 처음 연결 한번 성공 했을떄 true
        private bool m_bAutoReconnected = false; // 처음 연결 성공 후 끊겼을때 재연결 시도...
        private bool m_bTriggerFlag = false; // 중복 호출 차단 플래그,

        private const string TYPE = "DM152";
        private const bool AUTO_RECONNECT_USESKIP = true;
        private const bool KEEP_ALIVE_OPTION = true;

        private List<string> m_ltResultData = new List<string>();

        public bool m_bDevice = false;
        public Image m_Images = null;
        public Image m_LiveImages = null;
        public List<string> m_ltImagesGraphics = new List<string>();
        #endregion

        #region Events
        public delegate void ResultEventHandler(string BarcodeData);
        public event ResultEventHandler ResultEventData;
        #endregion

        public BarcodeSystem(string strComPort, string strDeviceName)
        {
            m_strComPort = strComPort.ToUpper();
            m_strDeviceName = strDeviceName;
            G.BaseBarcode.m_SerSystemDiscoverer.SystemDiscovered += new SerSystemDiscoverer.SystemDiscoveredHandler(OnSerSystemDiscovered);
        }

        private void OnSerSystemDiscovered(SerSystemDiscoverer.SystemInfo systemInfo)
        {
            try
            {

                if (systemInfo.Type == TYPE && systemInfo.PortName == m_strComPort)
                {
                    if (m_bDevice == false)
                    {
                        m_bDevice = true;
                        G.BaseBarcode.m_DeviceCount++;
                    }

                    m_SysInfo = systemInfo;
                }
            }
            catch
            {

            }
        }


        private void Connect()
        {
            if (m_SysInfo == null) return;

            m_bConnect = false;

            try
            {
                m_Connector = new SerSystemConnector(m_SysInfo.PortName, m_SysInfo.Baudrate);

                m_System = new DataManSystem(m_Connector);
                m_System.DefaultTimeout = 5000;
                m_System.SystemConnected += new SystemConnectedHandler(OnSystemConnected);
                m_System.SystemDisconnected += new SystemDisconnectedHandler(OnSystemDisconnected);
                m_System.KeepAliveResponseMissed += new KeepAliveResponseMissedHandler(OnKeepAliveResponseMissed);
                ResultTypes requested_result_types = ResultTypes.ReadString;
                m_Results = new ResultCollector(m_System, requested_result_types);
                m_Results.ComplexResultCompleted += Results_ComplexResultCompleted;
                m_Results.SimpleResultDropped += Results_SimpleResultDropped;

                m_System.Connect();

                //생존 옵션 처리
                m_System.SetKeepAliveOptions(KEEP_ALIVE_OPTION, 3000, 1000);

                //결과 타입 등록
                m_System.SetResultTypes(requested_result_types);
            }
            catch (Exception ex)
            {
                CleanupConnection();
                G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
            }

            m_bConnect = true;
        }

        private void OnSystemConnected(object sender, EventArgs args)
        {
            // 연결 상태
            m_bAutoReconnected = false;
            m_bTriggerFlag = false;
        }

        private void OnSystemDisconnected(object sender, EventArgs args)
        {
            if (m_bConnect && AUTO_RECONNECT_USESKIP)
            {
                // 자동 재연결 시작....
                m_bAutoReconnected = true;
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
            string str_parsing_data = ParsingResultData(e);
            if (ResultEventData != null) ResultEventData(str_parsing_data);

            if (m_ltResultData.Contains(str_parsing_data) == false)
            {
                m_ltResultData.Insert(0, str_parsing_data);
            }
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
                m_System.SystemConnected -= OnSystemConnected;
                m_System.SystemDisconnected -= OnSystemDisconnected;
                //_system.SystemWentOnline -= OnSystemWentOnline;
                //_system.SystemWentOffline -= OnSystemWentOffline;
                m_System.KeepAliveResponseMissed -= OnKeepAliveResponseMissed;
                //_system.BinaryDataTransferProgress -= OnBinaryDataTransferProgress;
                //_system.OffProtocolByteReceived -= OffProtocolByteReceived;
                //_system.AutomaticResponseArrived -= AutomaticResponseArrived;
            }

            m_SysInfo = null;
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
            if (G.BaseBarcode.m_SerSystemDiscoverer == null) return;
            // 연결 가능한 디바이스를 못 찾았을때 사용 함.
            try
            {
                if (G.BaseBarcode.m_SerSystemDiscoverer.IsDiscoveryInProgress) return;

                m_SysInfo = null;
                m_ltResultData.Clear();
            }
            finally
            {
                G.BaseBarcode.m_SerSystemDiscoverer.Discover();
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
            catch (Exception ex)
            {
                UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
            }
        }

        #region public functions
        public bool IsConnect()
        {
            if (m_Connector == null) return false;
            return ConnectionState.Connected == m_Connector.State ? true : false;
        }

        public int IsState()
        {
            /*Disconnected = 0,
			  Connecting = 1,
			  Connected = 2,
			  Disconnecting = 3*/

            if (m_Connector == null) return 0;
            return (int)m_Connector.State;
        }

        public void Dispose()
        {
            m_bConnect = false;

            if (null != m_System && m_System.State == ConnectionState.Connected)
            {
                m_System.Disconnect();
                CleanupConnection();
            }
        }

        public void TriggerOn()
        {
            try
            {
                if (m_bTriggerFlag == true || m_System == null) return;
                m_bTriggerFlag = true;

                m_ltResultData.Clear();
                m_System.SendCommand("TRIGGER ON");//셀프(내부)
            }
            catch (Exception ex)
            {
                G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
            }
        }

        public void TriggerOff()
        {
            try
            {
                if (m_bTriggerFlag == false || m_System == null) return;
                m_bTriggerFlag = false;

                m_System.SendCommand("TRIGGER OFF");//단일(외부)
            }
            catch (Exception ex)
            {
                G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
            }
        }

        public void TriggerOnTriggerOnContinuous()
        {
            try
            {
                if (m_bTriggerFlag == true || m_System == null) return;
                m_bTriggerFlag = true;

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
                if (m_bTriggerFlag == false || m_System == null) return;
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
                //모터 움직이면서 인터벌을 통해 바코드를 찍는 경우 외부 프로그램에서 타입 설정 -> 셀프(내부) 필요
                m_System.SendCommand("SET CAMERA.INTERVAL-US " + nTimeUs.ToString());//단위:us
            }
            catch (Exception ex)
            {
                G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
            }
        }

        //트리거 On/Off 이후 여러 바코드 값 한번에 가져오기.
        public List<string> GetResultDataList()
        {
            return m_ltResultData;
        }

        public string GetResultData()
        {
            if (m_ltResultData.Count == 0) return "";

            return m_ltResultData[0];
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
                if (m_System == null)
                {
                    if (G.BaseBarcode.m_SerSystemDiscoverer.IsDiscoveryInProgress) return;
                    Connect();
                }
                else
                {
                    m_System = null;
                }
            }
            catch (Exception ex)
            {
                G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
            }
        }

        public void SystemConnect() // 매뉴얼로 사용시....
        {
            if (m_System != null) m_System.Connect();
            else if (G.BaseBarcode.m_SerSystemDiscoverer != null)
            {
                if (G.BaseBarcode.m_SerSystemDiscoverer.IsDiscoveryInProgress) return;

                G.BaseBarcode.m_SerSystemDiscoverer.Discover();
            }
            else
            {
                G.BaseBarcode.m_SerSystemDiscoverer = new SerSystemDiscoverer();
                G.BaseBarcode.m_SerSystemDiscoverer.SystemDiscovered += new SerSystemDiscoverer.SystemDiscoveredHandler(OnSerSystemDiscovered);
                G.BaseBarcode.m_SerSystemDiscoverer.Discover();
            }
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
            catch (Exception ex)
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
            catch (Exception ex)
            {
                UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
            }
        }

        #endregion
    }
}

