using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace H_MarsSSD
{
	#region [ public enum functions ]
	public enum LAN_MODE
	{
		ENG = 0,        
		KOR,
		CHA,        
	}  
	
	public enum PW_MODE
	{
		OPER = 0,
		ENG,
		PRO,
		ADMIN = 4,
	}
	#endregion

	static class Program
	{
		/// <summary>
		/// 해당 응용 프로그램의 주 진입점입니다.
		/// </summary>

		#region Public static variables
//         public static IniFile iniStart = new IniFile("C:/BIN/DATA/StartConfig.ini");
//         public static IniFile iniMaster = new IniFile("C:/BIN/DATA/MasterRecipe.ini");
//         public static CSVFile csvLanguage = new CSVFile("C:/BIN/DATA/Language.csv");
		public	const	string	DataDirectory	= "D:\\DATA(HOTAYI_MARS_SSD)\\";

		public	static	int		m_Languageidx	= (int)LAN_MODE.ENG;
		public	static	PW_MODE	m_PasswordMode	= PW_MODE.OPER;
		public	static	IniFile iniMachine		= new IniFile(DataDirectory + @"MachineConfig.ini");
		#endregion

		#region 전처리기 기호
		#if _SIMUL_
			/// <summary>
			/// 시뮬레이터 전처리기를 부울값으로 사용.
			/// </summary>
			public const bool _bSimul_ = true;
		#else     
			/// <summary>
			/// 시뮬레이터 전처리기를 부울값으로 사용.
			/// </summary>
			public const bool _bSimul_ = false;
		#endif
		#endregion


		[STAThread]
		static void Main()
		{   
			#if _WMX3_
			Process[] ProcessList1	= Process.GetProcessesByName("WMX3Manager");
			if (ProcessList1.Length == 0)
			{
				Process.Start(@"C:\Program Files\SoftServo\WMX3\WMX3Manager.exe");
			}
			#endif

		//	Process[] ProcessList2	= Process.GetProcessesByName("stationx");
		//	if (ProcessList2.Length == 0)
		//	{
		//		Process.Start(@"C:\Program Files (x86)\Miracom\Highway101V32\bin\stationx.exe");
		//	}

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			
			Application.Run(new Main());
		}
	}

	#region [ Form Page : static ]
	public class SM
	{
		private static SharedMemory m_sHM		= new SharedMemory();
		public static SharedMemory SHM
		{
			get
			{
				if (m_sHM == null)
				{
					m_sHM = new SharedMemory();
				}

				return m_sHM;
			}
		}

		private static FlowMngTray _flowMng1	= new FlowMngTray();
		public static FlowMngTray FMT
		{
			get
			{
				if (_flowMng1 == null)
				{
					_flowMng1 = new FlowMngTray();
				}

				return _flowMng1;
			}
		}

		private static FlowMngPart _flowMng2	= new FlowMngPart();
		public static FlowMngPart FMP
		{
			get
			{
				if (_flowMng2 == null)
				{
					_flowMng2 = new FlowMngPart();
				}

				return _flowMng2;
			}
		}

		
		private static FlowMngRejectTray _flowMng3	= new FlowMngRejectTray();
		public static FlowMngRejectTray FMR
		{
			get
			{
				if (_flowMng3 == null)
				{
					_flowMng3 = new FlowMngRejectTray();
				}

				return _flowMng3;
			}
		}

	}

	public class UI
	{
		#region -public static variables  
		/// <summary>
		/// MMI관련 외부 클래스 선언 ~ 필요한 클래스 여기서 추가하자.
		/// </summary> 
		//public static bool bConnectSeq = false;
		   
		public static UILog Logs = new UILog();
		
		private static string _strMemberID;
		public static string strMemberID
		{
			get { return _strMemberID; }
			set { _strMemberID = value; }
		}
		#endregion

		#region -public static Functions
		public static void LoadingFormOpen(string strText = "Loading . . .")
		{
			if (LoadingView != null) return;
			LoadingView = new frmLoading(strText);
			System.Threading.Thread thLoadingForm = new System.Threading.Thread(new System.Threading.ThreadStart(ShowformLoading));
			thLoadingForm.IsBackground = true;
			thLoadingForm.Start();
		}

		public static void LoadingFormClose()
		{
			try
			{
				System.Threading.Thread.Sleep(200);
				if (LoadingView != null)
				{
					LoadingView.bClose = true;
					LoadingView = null;
				}
			}
			catch (Exception ex)
			{
				string s = "LoadingFormClose_" + ex.Message;
				Logs.Write(LOGPART.EX, s);
			}
		}

		private static void ShowformLoading()
		{
            try
            {
                if (LoadingView.bClose == false)
                    return;
                LoadingView.bClose = false;
                LoadingView.ShowDialog();

            }
            catch (Exception)
            {

                throw;
            }
		}
		#endregion -public static Functions [END]

		#region -All Page
		public static frmMainDisp	PageMainDisp	= new frmMainDisp();
		public static frmSubItems	PageSubItems	= new frmSubItems();
		public static frmMotor		PageMotor		= new frmMotor();
		public static frmIO			PageIO			= new frmIO();
		public static frmManual		PageManual		= new frmManual();
		public static frmManualSeq	PageManualSeq	= new frmManualSeq();
		public static frmManualCyl	PageManualCyl	= new frmManualCyl();
		public static frmManualTCP	PageManualTCP	= new frmManualTCP();
		public static frmManualEtc	PageManualEtc	= new frmManualEtc();
		public static frmManualRun	PageManualRun	= new frmManualRun();
		public static frmTenKey		PageTenKey		= new frmTenKey();
		public static frmData		PageData		= new frmData();
		public static frmLog		PageLog			= new frmLog();
		public static frmError		PageError		= new frmError();

		// Sub Page
		public static frmData_Recipe	SubpageRecipe		= new frmData_Recipe();
		public static frmData_Dm		SubpageDm			= new frmData_Dm();
		public static frmData_SDm		SubpageSDm			= new frmData_SDm();
		public static frmData_Useskip	SubpageUseskip		= new frmData_Useskip();
		public static frmData_Bit		SubpageBit			= new frmData_Bit();
		public static frmErrRecipeDisp	ErrorRecipeDisp		= new frmErrRecipeDisp();
		
		public static frmErrDescView	ModalErrDescView	= new frmErrDescView();
		public static frmBarcode		Barcode				= new frmBarcode();
		public static frmBarcode		BarcodeInfo			= new frmBarcode(false);
		public static frmLoading		LoadingView;
		
		public static frmManage			ModalManage			= new frmManage();
		public static frmLogin			ModalLogin			= new frmLogin();
		public static frmLevelSetting	ModalLevelSetting	= new frmLevelSetting();
		public static frmKeyboard		ModalKeyBoard		= new frmKeyboard("");
		public static frmBarcodeViewer	ModalBCRViewer		= new frmBarcodeViewer();
		public static frmSeqMessageBox  SeqMessageBox		= new frmSeqMessageBox();
		public static frmSeqMessageBox	TerminalMessageBox	= new frmSeqMessageBox();
        
		#endregion
	}
	#endregion

	#region [ Delegates - Invoke ]
	public static class Delegates
	{
		public delegate void SetTextCallback(Control control, string text);

		public static void InvokeIfNeededC(this Control control, Action action)
		{
			try
			{
				if (control.IsDisposed == true)			return;

				if (control.InvokeRequired)
					control.Invoke(action);
				else
					action();
			}
			catch
			{

			}
		}

		public static void InvokeIfNeededC<T>(this Control control, Action<T> action, T arg)
		{
			try
			{
				if (control.IsDisposed == true)			return;

				if (control.InvokeRequired)
					control.Invoke(action, arg);
				else
					action(arg);
			}
			catch
			{

			}
		}


		public static void InvokeIfNeededF(this UserControl control, Action action)
		{
			try
			{
				if (control.IsDisposed == true)			return;

				if (control.InvokeRequired)
					control.Invoke(action);
				else
					action();
			}
			catch
			{

			}
		}

		public static void InvokeIfNeededF<T>(this UserControl control, Action<T> action, T arg)
		{
			try
			{
				if (control.IsDisposed == true)			return;

				if (control.InvokeRequired)
					control.Invoke(action, arg);
				else
					action(arg);
			}
			catch
			{

			}
		}
	}
	#endregion
}
