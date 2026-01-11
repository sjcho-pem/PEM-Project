using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if _WMX3_
using WMX3ApiCLR;
using WMX3ApiCLR.EcApiCLR;

namespace H_MarsSSD
{
    public class CWMX3
    {
        private static WMX3Api wmxApi;

        private static EngineStatus EngStatus;
        //private DevicesInfo allDevStatus;

        //Motor Library
        public static CoreMotionStatus cmStatus;
        public static CoreMotion CM;
        public static Config.AxisParam axParam;

        //IO Library
        public static Io wmxIO;

        //EtherCAT Library
        private static Ecat wmxEcat;
        private static EcMasterInfo EcMasterInfo;


        public static void InitializeFunc()
        {
            wmxApi = new WMX3Api();
            cmStatus = new CoreMotionStatus();
            CM = new CoreMotion(wmxApi);
            wmxIO = new Io(wmxApi);
            wmxEcat = new Ecat(wmxApi);
            EcMasterInfo = new EcMasterInfo();
            axParam = new Config.AxisParam();

            int ret;
            string err;

            System.Threading.Thread.Sleep(1000);

            // Create device.
            ret = wmxApi.CreateDevice(Environment.GetEnvironmentVariable("WMX3"), DeviceType.DeviceTypeNormal,
                0xFFFFFFFF);
            err = WMX3Api.ErrorToString(ret);
            // Set Device Name.
            wmxApi.SetDeviceName("SyncMotion");

			System.Threading.Thread.Sleep(1000);
			wmxIO.SetInitialOutByte(74, 0x01, 1); //25번 모터의 setbrake 설정 주소/ configuator의 입출력 정보쪽을 통해서 해당 주소 확인하였음
			wmxIO.SetInitialOutByte(34, 0x01, 1); //0번 모터의 setbrake 설정 주소/ configuator의 입출력 정보쪽을 통해서 해당 주소 확인하였음
            // Start Communication.
            ret = wmxApi.StartCommunication(0xFFFFFFFF);
            err = WMX3Api.ErrorToString(ret);


            int r = CM.Config.ImportAndSetAll(@"C:\Program Files\SoftServo\WMX3\wmx_parameters.xml");
        }

        public static bool IsWMXDevice()
        {
            wmxApi.GetEngineStatus(ref EngStatus);


            return (EngStatus.State == EngineState.Communicating) && (cmStatus.EngineState == EngineState.Communicating);
        }

        public static void ReStartDevice()
        {
            int ret = wmxApi.StartCommunication(0xFFFFFFFF);
            string err = WMX3Api.ErrorToString(ret);
            CM.Config.ImportAndSetAll(@"C:\Program Files\SoftServo\WMX3\wmx_parameters.xml");
        }

        public static void StartHotConnect()
        {
            wmxEcat.StartHotconnect();
        }
        
        public static void ScanNetwork()
        {
            wmxEcat.ScanNetwork();
        }

        public static void SetSlaveOP()
        {
            wmxEcat.GetMasterInfo(EcMasterInfo);

            for(int i = 0; i < EcMasterInfo.GetOnlineSlaveCount(); i++)
            {
                if(EcMasterInfo.Slaves[i].State != EcStateMachine.Op)
                {
                    EcMasterInfo.Slaves[i].State = EcStateMachine.Op;
                }
            }
        }

        public static int GetOffLineSlaveCount()
        {
            return EcMasterInfo.GetOfflineSlaveCount();
        }

        public static void ReStartEngine()
        {
            int ret = wmxApi.RestartEngine("C:\\Program Files\\SoftServo\\WMX3", 15000);
            
        }

        public static void StateUpdate()
        {
            CM.GetStatus(ref cmStatus);
        }

        public static void CheckMotionErr(int ret, int nAxis)
        {
            if (ret != 0)
            {
                // 로그 추가
                string strErr = nAxis + " " + ret + "- " + CoreMotion.ErrorToString(ret);
                GLog.Add(strErr);
            }
        }

		public static bool IsOnlineAllSlave()
		{
			for(int i = 0; i < (int)MT.TOTAL; i++)
			{
				if (cmStatus.AxesStatus[i].ServoOffline == true)
				{
					return false;
				}
			 }

			 return true;
		}

		public static int StopCommunication()
		{
			return wmxApi.StopCommunication(2000);
		}
    }
}
#endif