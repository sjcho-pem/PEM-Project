using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace H_MarsSSD
{
	/// <summary>
	/// Global Class : Seq Only 사용...
	/// </summary>
	public class G
	{
		#region [ Variables ]        
		private static Thread	thSeqMain = null;
        private static Thread thCtrlUpdate = null;
		private static CKernel m_CKernel = new CKernel();
		private static CAlarmMng m_AlmMng = new CAlarmMng();

		public static CKernel cKernel { get => m_CKernel; set => m_CKernel = value; }
		public static CAlarmMng AlmMng { get => m_AlmMng; set => m_AlmMng = value; }        

		//----------------- Seq Page Add
		public	static	SystemError		sysErr		= new SystemError();
		public	static	SeqButton		seqBtn		= new SeqButton();
		public	static	AllHome			allHome		= new AllHome();
		public	static	SeqManual		seqMnl		= new SeqManual();
		public	static	LampBuzzer		LampBuzz	= new LampBuzzer(); 
		public static 	Simulation		Simul		= new Simulation();
		public static 	SeqInterlock	Interlock	= new SeqInterlock();
		//----------------- Seq 추가
		public	static	SeqMain				seqMain				= new SeqMain();
		public	static	SeqLoadTrayTable1	seqLoadTrayTable1	= new SeqLoadTrayTable1();
		public	static	SeqLoadTrayTable2	seqLoadTrayTable2	= new SeqLoadTrayTable2();
		public  static  SeqUnloadTrayTable  seqUnloadTrayTable1 = new SeqUnloadTrayTable();
		public  static  SeqUnloadTrayTable  seqUnloadTrayTable2 = new SeqUnloadTrayTable();
		public	static	SeqLoadPnP			seqLoadPnP			= new SeqLoadPnP();
		public	static	SeqLoadBuffer		seqLoadBuffer1		= new SeqLoadBuffer();
		public	static	SeqLoadBuffer		seqLoadBuffer2		= new SeqLoadBuffer();
		public	static	SeqTrayLoader		seqTrayLoader		= new SeqTrayLoader();
		public	static	SeqLoadInspPnP		seqLoadInspPnP		= new SeqLoadInspPnP();
		public	static	SeqUnloadInspPnP	seqUnloadInspPnP	= new SeqUnloadInspPnP();
		public	static	SeqInspPlate		seqInspPlate1		= new SeqInspPlate();
		public	static	SeqInspPlate		seqInspPlate2		= new SeqInspPlate();
		public	static	SeqUnloadBuffer		seqUnloadBuffer		= new SeqUnloadBuffer();
		public  static  SeqRejectBuffer		seqRejectBuffer		= new SeqRejectBuffer();
		public	static	SeqUnloadPnP		seqUnloadPnP		= new SeqUnloadPnP();
		public  static  SeqLoadTransfer		seqLoadTransfer		= new SeqLoadTransfer();
		public  static  SeqTrayClean		seqTrayClean		= new SeqTrayClean();
		public  static  SeqUnloadTransfer	seqUnloadTransfer	= new SeqUnloadTransfer();
		public  static  SeqTrayUnloader		seqTrayUnloader		= new SeqTrayUnloader();
		public  static  SeqJobTeaching		seqJobTeaching		= new SeqJobTeaching();

		#if _SIMUL_
			public static	BaseBarcodeSystem	BaseBarcode			= new BaseBarcodeSystem();
			public	static	BarcodeSystem_Simul LoadOESBarcode		= new BarcodeSystem_Simul("COM1", "Loader1"); //로더 측면 OES용 -> 로드 트랜스퍼 OES용으로 변경
			public static	BarcodeSystem_Simul LoadLPCAMMBarcode	= new BarcodeSystem_Simul("COM2", "Loader2"); //로더 정면 LPCAMM용 -> 로드 트랜스퍼 LPCAMM용으로 변경
			public static	BarcodeSystem_Simul UnloadOESBarcode	= new BarcodeSystem_Simul("COM3", "Unload1"); //언로드 트랜스퍼 OES용
			public static	BarcodeSystem_Simul UnloadLPCAMMBarcode = new BarcodeSystem_Simul("COM4", "Unload2");//언로드 트랜스퍼 LPCAMM
			public static	BarcodeSystem_Simul RejectOESBarcode	= new BarcodeSystem_Simul("COM5", "Reject1"); //리젝 트레이1 //OES용
			public static	BarcodeSystem_Simul RejectLPCAMMBarcode = new BarcodeSystem_Simul("COM6", "Reject2"); //리젝 트레이2 //LPCAMM
			public static	CommFTV_Sim			commFTVLoader		= new CommFTV_Sim();
			public static	CommFTV_Sim			commFTVTable		= new CommFTV_Sim();
		#else
			public static	BaseBarcodeSystem	BaseBarcode				= new BaseBarcodeSystem();
			public	static	BarcodeSystem		LoadOESBarcode			= new BarcodeSystem("COM8","Loader1"); //로더 측면 OES용 -> 로드 트랜스퍼 OES용으로 변경
			public	static	BarcodeSystem		LoadLPCAMMBarcode		= new BarcodeSystem("COM7","Loader2"); //로더 정면 LPCAMM용-> 로드 트랜스퍼 LPCAMM용으로 변경
			public	static	BarcodeSystem		UnloadOESBarcode		= new BarcodeSystem("COM6","Unload1"); //언로드 트랜스퍼 OES용
			public	static	BarcodeSystem		UnloadLPCAMMBarcode		= new BarcodeSystem("COM5","Unload2");//언로드 트랜스퍼 LPCAMM
			public	static	BarcodeSystem		RejectOESBarcode		= new BarcodeSystem("COM2","Reject1"); //리젝 트레이1 //OES용
			public	static	BarcodeSystem		RejectLPCAMMBarcode		= new BarcodeSystem("COM1","Reject2"); //리젝 트레이2 //LPCAMM
			public static	CommFTV				commFTVLoader			= new CommFTV();
			public static	CommFTV				commFTVTable			= new CommFTV();
		#endif
		
		public	static	SeqUbiGEMComm		seqUBIGEMComm		= new SeqUbiGEMComm();

	
		#if _SIMUL_
		public	static	CommInsp_Sim		commInspTop			= new CommInsp_Sim();
		public	static	CommInsp_Sim		commInspBtm			= new CommInsp_Sim();
		public	static	CommInsp_Sim		commInspRef			= new CommInsp_Sim();
		#else
		public	static	CommInsp			commInspTop			= new CommInsp();
		public	static	CommInsp			commInspBtm			= new CommInsp();
		public	static	CommInsp			commInspRef			= new CommInsp();
		#endif
		public	static	CommUbiGEM			commUbiGEM			= new CommUbiGEM("secsgem_pemtron.ugc");

		private	static	bool	bSeqMainShould	= true;
		#endregion

		public static void	InitControl()
		{
			//N Data Memory 초기화
			NDm.Init();

			//D Data Memory 초기화
			DDm.Init();

			//S Data Memory 초기화
			SDm.Init();

			cKernel.Init();
			cKernel.Update();

			CommInit();

			//SEQ Page 초기화
			sysErr.Init();
			allHome.Init();
			seqBtn.Init();
			seqMnl.Init();
			LampBuzz.Init();
			
			seqMain.Init();

			seqLoadTrayTable1.Init();
			seqLoadTrayTable2.Init();
			seqUnloadTrayTable1.Init(0);
			seqUnloadTrayTable2.Init(1);

			seqLoadPnP.Init();
			seqLoadBuffer1.Init(0);
			seqLoadBuffer2.Init(1);
			seqTrayLoader.Init();
			seqLoadInspPnP.Init();
			seqUnloadInspPnP.Init();
			seqInspPlate1.Init(0);
			seqInspPlate2.Init(1);
			seqUnloadBuffer.Init();
			seqRejectBuffer.Init();
			seqUnloadPnP.Init();

			seqLoadTransfer.Init();
			seqTrayClean.Init();
			seqTrayUnloader.Init();
			seqUnloadTransfer.Init();
			seqJobTeaching.Init();
			seqUBIGEMComm.Init();

			//다른 시퀀스 초기화가 모두 끝난후에 시뮬레이션 초기화해야 함
			Simul.Init();
            
            //Ctrl Update Thread Start
            thCtrlUpdate = new Thread(MethodCtrlUpdate);
            thCtrlUpdate.Start();

			//메인쓰레드 시작
			thSeqMain = new Thread(MethodSeqMain);
			thSeqMain.SetApartmentState(ApartmentState.STA);
			thSeqMain.IsBackground = true;
			thSeqMain.Start();
		}

        private static void MethodCtrlUpdate()
		{
			// 상시 업데이트 전용!!
			while (bSeqMainShould)
			{
				Thread.Sleep(1);

				cKernel.Update();
				AlmMng.Update();

				CommUpdate();
			}
		}

		private static void MethodSeqMain()
		{
			try
			{
				while (bSeqMainShould)
				{
					Thread.Sleep(5);                    									
				
					Simul.Run();  

					//Seq All Run
					seqBtn.Run();
					sysErr.Run();
					seqMnl.Run();
					LampBuzz.Run();
					Interlock.Run();

					allHome.Run();
					seqMain.Run();

					seqLoadTrayTable1.Run();
					seqLoadTrayTable2.Run();
					seqUnloadTrayTable1.Run();
					seqUnloadTrayTable2.Run();
					seqLoadPnP.Run();
					seqLoadBuffer1.Run();
					seqLoadBuffer2.Run();
					seqTrayLoader.Run();
					seqLoadTransfer.Run();
					seqTrayClean.Run();
					seqTrayUnloader.Run();
					seqUnloadTransfer.Run();
					seqLoadInspPnP.Run();
					seqUnloadInspPnP.Run();
					seqInspPlate1.Run();
					seqInspPlate2.Run();
					seqRejectBuffer.Run();
					seqUnloadPnP.Run();
					seqUnloadBuffer.Run();
		
					seqJobTeaching.Run();
					seqUBIGEMComm.Run();
				}
			}
			catch (ThreadAbortException)
			{
			}
			catch (ThreadInterruptedException)
			{
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
			finally
			{
				bSeqMainShould = false;
			}
		}
		

		public static void CtrlDispose()
		{
			bSeqMainShould = false;

			Thread.Sleep(100);

			SM.SHM.MsgAllClose();

			if (thSeqMain.IsAlive == true)			thSeqMain.Abort();
			if (thCtrlUpdate.IsAlive == true)		thCtrlUpdate.Abort();

            if (thSeqMain != null)					thSeqMain = null;
            if (thCtrlUpdate != null)				thCtrlUpdate = null;


			G.cKernel.AOut(DO.LDBuf1DustCyl, false);
			G.cKernel.AOut(DO.LDBuf2DustCyl, false);
			GLog.Dispose();
			cKernel.Dispose();
			CommDispose();
			commUbiGEM.Dispose();
		}

		public static bool DryRunIsAIn(DI di, bool bReturnState)
		{
			try
			{
                if (SM.SHM.GetUseSkipData(USESKIP.DRYRUN) == false)
				{                    
					return G.cKernel.IsAIn(di);
				}
				else
				{
					return bReturnState;
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return G.cKernel.IsAIn(di);
			}
		}

		public static bool DryRunIsBIn(DI di, bool bReturnState)
		{
			try
			{
                if (SM.SHM.GetUseSkipData(USESKIP.DRYRUN) == false)
				{
					return G.cKernel.IsBIn(di);
				}
				else
				{
					return bReturnState;
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return G.cKernel.IsAIn(di);
			}
		}
		
		public	static	void	CommInit(string CommName)
		{
			string[] str_ip;

			switch (CommName)
			{
				case "lbTopState":
					
					if (Program.iniMachine.GetString("Socket", frmIPSetting.Position.TOP.ToString()) == "")
						Program.iniMachine.SetString("Socket", frmIPSetting.Position.TOP.ToString(), "192.168.0.12:7001");

					str_ip = Program.iniMachine.GetString("Socket", frmIPSetting.Position.TOP.ToString()).Split(':');
					commInspTop.Init(str_ip[0], int.Parse(str_ip[1]), CommInsp.CmdDir.ToTop);
					break;

				case "lbBtmState":
					if (Program.iniMachine.GetString("Socket", frmIPSetting.Position.BOTTOM.ToString()) == "")
						Program.iniMachine.SetString("Socket", frmIPSetting.Position.BOTTOM.ToString(), "192.168.0.13:7002");

					str_ip = Program.iniMachine.GetString("Socket", frmIPSetting.Position.BOTTOM.ToString()).Split(':');
					commInspBtm.Init(str_ip[0], int.Parse(str_ip[1]), CommInsp.CmdDir.ToBtm);
					break;

				case "lbRevState":
					if (Program.iniMachine.GetString("Socket", frmIPSetting.Position.REV.ToString()) == "")
						Program.iniMachine.SetString("Socket", frmIPSetting.Position.REV.ToString(), "192.168.0.14:7003");

					str_ip = Program.iniMachine.GetString("Socket", frmIPSetting.Position.REV.ToString()).Split(':');
					commInspRef.Init(str_ip[0], int.Parse(str_ip[1]), CommInsp.CmdDir.ToReview);
					break;


            }
		}

		public	static	void	CommInit()
		{
			if (Program.iniMachine.GetString("Socket", frmIPSetting.Position.TOP.ToString()) == "")
				Program.iniMachine.SetString("Socket", frmIPSetting.Position.TOP.ToString(), "192.168.0.12:7001");
			
			if (Program.iniMachine.GetString("Socket", frmIPSetting.Position.BOTTOM.ToString()) == "")
				Program.iniMachine.SetString("Socket", frmIPSetting.Position.BOTTOM.ToString(), "192.168.0.13:7002");
			
			if (Program.iniMachine.GetString("Socket", frmIPSetting.Position.REV.ToString()) == "")
				Program.iniMachine.SetString("Socket", frmIPSetting.Position.REV.ToString(), "192.168.0.14:7003");
			

			string[] str_ip;

			str_ip	= Program.iniMachine.GetString("Socket","TOP").Split(':');
			commInspTop.Init(str_ip[0], int.Parse(str_ip[1]), CommInsp.CmdDir.ToTop);

			str_ip	= Program.iniMachine.GetString("Socket","BOTTOM").Split(':');
			commInspBtm.Init(str_ip[0], int.Parse(str_ip[1]), CommInsp.CmdDir.ToBtm);

			str_ip	= Program.iniMachine.GetString("Socket","REV").Split(':');
			commInspRef.Init(str_ip[0], int.Parse(str_ip[1]), CommInsp.CmdDir.ToReview);
			
			commUbiGEM.InitGEM();

            commFTVLoader.Init(0);
            commFTVTable.Init(1);

        }

		public	static	void	CommUpdate()
		{
			commInspTop.Update();
			commInspBtm.Update();
			commInspRef.Update();
			commUbiGEM.Update();
            commFTVLoader.Update();
            commFTVTable.Update();


            SM.SHM.SetBitData(BIT.TopInspConnect,		commInspTop.IsConnect());
			SM.SHM.SetBitData(BIT.BtmInspConnect,		commInspBtm.IsConnect());
			SM.SHM.SetBitData(BIT.ReviewConnect,		commInspRef.IsConnect());
			SM.SHM.SetBitData(BIT.Loader1BarcodeConnect,LoadOESBarcode.IsConnect());
			SM.SHM.SetBitData(BIT.Loader2BarcodeConnect,LoadLPCAMMBarcode.IsConnect());
			SM.SHM.SetBitData(BIT.Unload1BarcodeConnect,UnloadOESBarcode.IsConnect());
			SM.SHM.SetBitData(BIT.Unload2BarcodeConnect,UnloadLPCAMMBarcode.IsConnect());
			SM.SHM.SetBitData(BIT.Reject1BarcodeConnect,RejectOESBarcode.IsConnect());
			SM.SHM.SetBitData(BIT.Reject2BarcodeConnect,RejectLPCAMMBarcode.IsConnect());
			SM.SHM.SetBitData(BIT.SmartVisionLoaderConnect, commFTVLoader.IsConnect());
			SM.SHM.SetBitData(BIT.SmartVisionTableConnect, commFTVTable.IsConnect());

			bool bTotalState =  LoadOESBarcode.IsConnect()  && 
								LoadLPCAMMBarcode.IsConnect() &&
								UnloadOESBarcode.IsConnect()  &&
								UnloadLPCAMMBarcode.IsConnect()  &&
								RejectOESBarcode.IsConnect()  &&
								RejectLPCAMMBarcode.IsConnect();

			SM.SHM.SetBitData(BIT.TotalBarcodeConnect, bTotalState);
			
		}

		
		public static void CommDispose(string CommName)
		{
			switch (CommName)
			{
				case "lbTopState":			commInspTop.Dispose();		break;
				case "lbBtmState":			commInspBtm.Dispose();		break;
				case "lbRevState":			commInspRef.Dispose();		break;
				case "lbSmartVision1State":	commFTVLoader.Dispose();	break;
				case "lbSmartVision2State":	commFTVTable.Dispose();;	break;
                case "lbControlState":									break;
			}
		}

		public	static	void	CommDispose()
		{
			commInspTop.Dispose();
			commInspBtm.Dispose();
			commInspRef.Dispose();
            BaseBarcode.Dispose();
            LoadOESBarcode.Dispose();
			LoadLPCAMMBarcode.Dispose();
			UnloadOESBarcode.Dispose();
			UnloadLPCAMMBarcode.Dispose();
			RejectOESBarcode.Dispose();
			RejectLPCAMMBarcode.Dispose();
			commFTVLoader.Dispose();
			commFTVTable.Dispose();
		}		
	}
}
