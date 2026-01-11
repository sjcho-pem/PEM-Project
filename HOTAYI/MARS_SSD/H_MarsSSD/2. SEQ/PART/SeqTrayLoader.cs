using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace H_MarsSSD
{
	public class SeqTrayLoader : SeqBase<SeqTrayLoader.Act, SeqTrayLoader.Axis, SeqTrayLoader.Cmd>
	{
		#region [ Enum ]
		public enum Act
		{
			FrontStopper,
			Max,
		}

		public enum Axis
		{
			LiftZ,
			ConvLeft,
			ConvRight,
			ConvWidth,
			ConvStopper,
			Max,
		}

		public enum Cmd
		{
			START	= 10,
			END		= 99,
			END_NG	= 98,

			HOME_10	= 100,
			HOME_20,
			HOME_30,
			HOME_40,
			HOME_50,
			HOME_60,
			HOME_70,
			HOME_80,
			HOME_90,
			HOME_91,
			HOME_92,
			HOME_93,
			HOME_100,
			HOME_110,
			HOME_120,
			HOME_130,

			LIFTDOWN_10,
			LIFTDOWN_11,
			LIFTDOWN_12,
			LIFTDOWN_13,
			LIFTDOWN_14,
			LIFTDOWN_15,
			LIFTDOWN_20,
			LIFTDOWN_22,
			LIFTDOWN_30,
			LIFTDOWN_40,
			LIFTDOWN_50,
			LIFTDOWN_60,
			LIFTDOWN_70,
			LIFTDOWN_80,
			LIFTDOWN_EMPTYCHECK_10,
			LIFTDOWN_EMPTYCHECK_20,
			LIFTDOWN_EMPTYCHECK_30,
			LIFTDOWN_EMPTYCHECK_40,
			LIFTDOWN_EMPTYCHECK_50,
			LIFTDOWN_JOBCHECK_10,
			LIFTDOWN_JOBCHECK_20,
			LIFTDOWN_JOBCHECK_30,

			END_NG_RECIPE,

			LIFTUP_10,
			LIFTUP_20,
			LIFTUP_30,
			LIFTUP_40,
			LIFTUP_50,
			LIFTUP_60,
			LIFTUP_70,
			LIFTUP_80,
			LIFTUP_90,

			LOAD_10,
			LOAD_12,
			LOAD_20,
			LOAD_30,
			LOAD_40,
			LOAD_50,
			LOAD_60,
			LOAD_70,
			LOAD_80,
			LOAD_90,
			LOAD_100,

			UNLOAD_10,
			UNLOAD_20,
			UNLOAD_30,
			UNLOAD_40,
			UNLOAD_50,
			UNLOAD_60,
			UNLOAD_70,
			UNLOAD_80,
			UNLOAD_90,
			UNLOAD_100,
			UNLOAD_110,
			UNLOAD_120,
			UNLOAD_130,

			
			SWLIFTUP_10,
			SWLIFTUP_20,
			SWLIFTUP_30,
			SWLIFTUP_40,

			SWLIFTDOWN_10,
			SWLIFTDOWN_20,
			SWLIFTDOWN_30,
			SWLIFTDOWN_40,
		}

		public enum PosAlign
		{
			READY,
			ALIGN,
			PNP,
		}

		public enum PosStopper
		{
			READY,
			STOPPER,
		}

		public enum PosConv
		{
			LOAD_FAST,
			LOAD_SLOW,
			UNLOAD_FAST,
			UNLOAD_SLOW,
		}

		public enum PosZ
		{
			DOWN,
            UP,
			SCAN_START,
			EMPTY_CHECK,
            SWITCHDOWN,
            SWITCHUP,
            COUNT_TRAY_SPEED,
        }
		#endregion

		#region [Variables]
		private SMM<Cmd>	m_smmHome		= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.LOADER_HOME));
		private SMM<Cmd>	m_smmLoad		= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.LOADER_LOAD));
		private SMM<Cmd>	m_smmUnload		= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.LOADER_UNLOAD));
		private SMM<Cmd>	m_smmLiftDown	= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.LOADER_DOWN));
		private SMM<Cmd>	m_smmLiftUp		= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.LOADER_UP));
		private SMM<Cmd>	m_smmSWLiftDown	= new SMM<Cmd>(Cmd.START, "SWITCH DOWN");
		private SMM<Cmd>	m_smmSWLiftUp	= new SMM<Cmd>(Cmd.START, "SWITCH UP");
		
		public	Cmd	smmHome			{ get => m_smmHome.CurStep; }
		public	Cmd	smmLoad			{ get => m_smmLoad.CurStep; }
		public	Cmd	smmUnload		{ get => m_smmUnload.CurStep; }
		public	Cmd	smmLiftDown		{ get => m_smmLiftDown.CurStep; }
		public	Cmd	smmLiftUp		{ get => m_smmLiftUp.CurStep; }
		public	Cmd	smmSWLiftDown	{ get => m_smmSWLiftDown.CurStep; }
		public	Cmd	smmSWLiftUp		{ get => m_smmSWLiftUp.CurStep; }
		private bool	m_bAutoRun		= false;
		public	bool			AutoRun			{ get => m_bAutoRun; }
		private	bool			m_IsMtHome			= true;
		private	bool			m_IsDownComplete	= false;
		private int				m_nLiftTartgetPos	= 0;
		private string			m_strErrMsg			="";
		private SeqMsg			m_ErrSeqMsg			= SeqMsg.Unknown;
		private int				m_smmFTVInspRetry	= 0;
		private ALM				m_Alarm				= ALM.UNKNOWN;

		private int				m_nTrayCountStartPos = 0;
		private int				m_nTrayCountEndPos = 0;
        #endregion


        #region Public Function
        public void	Init()
		{
			try
			{
				lstSMM.Add(m_smmHome);
				lstSMM.Add(m_smmLoad);
				lstSMM.Add(m_smmLiftDown);
				lstSMM.Add(m_smmLiftUp);
				lstSMM.Add(m_smmUnload);
				lstSMM.Add(m_smmSWLiftDown);
				lstSMM.Add(m_smmSWLiftUp);

				#region - Motor
				arrMt = new EnumArray<Axis, CMotor>(Axis.Max);

				arrMt[Axis.ConvStopper]	= G.cKernel.Motor(MT.LDConvStopper); 
				arrMt[Axis.ConvLeft]		= G.cKernel.Motor(MT.LDConv1);
				arrMt[Axis.ConvRight]		= G.cKernel.Motor(MT.LDConv2);
				arrMt[Axis.ConvWidth]		= G.cKernel.Motor(MT.LDConvWidth);
				arrMt[Axis.LiftZ]		= G.cKernel.Motor(MT.LDTrayLiftZ);
				#endregion

				#region - Actuator            
				arrAct = new EnumArray<Act, Actuator>(Act.Max);
				
				arrAct[Act.FrontStopper] = new CSingleSen();
				arrAct[Act.FrontStopper].SetSenser1(new List<DI>(new DI[] { DI.LDTrayConvStopperFwd}));
				arrAct[Act.FrontStopper].SetSenser2(new List<DI>(new DI[] { DI.LDTrayConvStopperBwd }));
				arrAct[Act.FrontStopper].SetSol(DO.LDConvStopperCyl);
				arrAct[Act.FrontStopper].Setting(ALM.ERROR_LOAD_LIFT_STOPPER_CYL_FORWARD, ALM.ERROR_LOAD_LIFT_STOPPER_CYL_BACKWARD, ErrSenPos.Ori, SenPos.NONE, 500);
				
				/*
				[ sensor List ]
				LDTrayLiftAreaSen1		- Lot 투입부, 안전 센서
				LDTrayLiftAreaSen2		- Lot 투입부, 안전 센서

				LDTrayLiftLimitSen		- Lot 투입 후, Lot 갯수 초과 방지 센서
				LDTrayLiftExistSen		- Lift z 축 하강 후, 트레이 감지 센서
				LDTrayLiftDecSen		- 역물류 감속 센서

				LDTrayConvExistSen		- 컨베이어 유/무 감지 센서
				LDTrayConvDecSen		- 정물류 감속 센서 , 트레이 크기 확인 센서
				LDTrayConvStopperSen	- 트레이가 스토퍼에 도달했음 확인
				*/
				#endregion

				if (NDm.Read(eNDM.LDLiftAlignDelay) <= 0)			NDm.Write(eNDM.LDLiftAlignDelay, 500);
				base.Initialize();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public bool HomeStart(bool IsMtHome = true) { m_IsMtHome = IsMtHome; return SetSeqRun(ref m_smmHome); }

		public bool LoadStart() { return SetSeqRun(ref m_smmLoad); }

		public bool LiftDownStart(bool bAutoRun = true)
		{
			try
			{
				m_bAutoRun = bAutoRun;
				return SetSeqRun(ref m_smmLiftDown);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public bool LiftUpStart(bool bAutoRun)
		{
			try
			{
				m_bAutoRun = bAutoRun;
				return SetSeqRun(ref m_smmLiftUp);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public bool UnloadStart() { return SetSeqRun(ref m_smmUnload); }

		public bool SwitchLiftUpStart() { return SetSeqRun(ref m_smmSWLiftUp); }

		public bool SwitchLiftDownStart() { return SetSeqRun(ref m_smmSWLiftDown); }

		public bool IsHomeReady() { return m_smmHome.IsReady; }

		public bool IsConvReady() { return m_smmLoad.IsReady && m_smmUnload.IsReady && m_smmHome.IsReady; }

		public bool IsLiftReady() { return m_smmLiftDown.IsReady && m_smmLiftUp.IsReady && m_smmHome.IsReady; }

		public void ResetLiftStep()
		{
			arrMt[Axis.LiftZ].Stop();

			m_smmLiftUp.ResetStep();
			m_smmLiftDown.ResetStep();
			m_smmSWLiftUp.ResetStep();
			m_smmSWLiftDown.ResetStep();
		}
		// 리프트 -> 컨베이어 까지 투입 가능 여부
		public bool IsLoadEnable()
		{
			try
			{
				if (arrMt[Axis.LiftZ].IsRdyPos(arrMt[Axis.LiftZ].GetIndexPos((int)PosZ.DOWN)) == false) return SM.SHM.InterlockMsgAdd("arrMt[teAxis.TrayLift].IsRdy((int)tePosZ.DOWN) == false");

				//안전 체크 
				if (G.cKernel.IsAIn(DI.LDTrayConvExistSen) == true)			return SM.SHM.InterlockMsgAdd("G.cKernel.IsAIn(DI.LDTrayConvExistSen) == true");
				if (G.cKernel.IsAIn(DI.LDTrayConvStopperSen) == true)		return SM.SHM.InterlockMsgAdd("G.cKernel.IsAIn(DI.LDTrayConvStopperSen) == true");
				if (G.cKernel.IsAIn(DI.LDTrayConvDecSen) == true)			return SM.SHM.InterlockMsgAdd("G.cKernel.IsAIn(DI.LDTrayConvDecSen) == true");
				if (G.cKernel.IsAIn(DI.LDTrayHolderGrip1Close) == true)		return SM.SHM.InterlockMsgAdd("G.cKernel.IsAIn(DI.LDTrayHolderGrip1Close) == true");
				if (G.cKernel.IsAIn(DI.LDTrayHolderGrip2Close) == true)		return SM.SHM.InterlockMsgAdd("G.cKernel.IsAIn(DI.LDTrayHolderGrip2Close) == true");
				
				if (G.DryRunIsBIn(DI.LDTrayLiftAreaSen1, false) == true)	return SM.SHM.InterlockMsgAdd("G.DryRunIsBIn(DI.LDTrayLiftAreaSen1, false) == true");
				if (G.DryRunIsBIn(DI.LDTrayLiftAreaSen2, false) == true)	return SM.SHM.InterlockMsgAdd("G.DryRunIsBIn(DI.LDTrayLiftAreaSen2, false) == true");

				if (arrMt[Axis.ConvWidth].IsBiggerThan((int)PosAlign.PNP) == true) return SM.SHM.InterlockMsgAdd("arrMt[teAxis.ConvWidth].IsBiggerThan((int)tePosAlign.READY) == true");

				SM.SHM.InterlockMsgReset();
				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
			//리프트 모터 위치 체크
		}
		// Load Transfer로 이동 가능 여부
		public bool IsTrayTransferEnable()
		{
			try
			{
				if (SM.FMT.IsFlowRange(FlowTray.LDConv01, FlowTray.LDConv31) == false)		return SM.SHM.InterlockMsgAdd("SM.FMT.IsFlowRange(teFlowTray.LDConv01, teFlowTray.LDConv15) == false");

				if (G.DryRunIsAIn(DI.LDTrayConvExistSen, true) == false)		return SM.SHM.InterlockMsgAdd("G.DryRunIsAIn(DI.LDTrayConvExistSen, true) == false");
				if (arrMt[Axis.ConvRight].IsRdy() == false)					    return SM.SHM.InterlockMsgAdd("arrMt[teAxis.ConvRight].IsRdy() == false");
				if (arrMt[Axis.ConvLeft].IsRdy() == false)					    return SM.SHM.InterlockMsgAdd("arrMt[teAxis.ConvLeft].IsRdy() == false");
				//if (G.seqTrayLoader.CheckTraySize() < 0)						return SM.SHM.InterlockMsgAdd("seqTrayLoader.CheckTraySize() < 0"); ;

				if (arrMt[Axis.ConvWidth].IsInPosIndex((int)PosAlign.PNP) == false)		return SM.SHM.InterlockMsgAdd("arrMt[teAxis.ConvWidth].IsInPosIndex((int)tePosAlign.READY) == false");
				if (arrMt[Axis.ConvStopper].IsInPosIndex((int)PosStopper.READY) == false)	return SM.SHM.InterlockMsgAdd("arrMt[teAxis.ConvStopper].IsInPosIndex((int)tePosStopper.READY) == false");

				SM.SHM.InterlockMsgReset();
				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}
		//바이패스 동작 시, 역방향으로 트레이를 빼는 경우 사용
		public bool IsUnloadConvEnable()
		{
			try
			{
				if (SM.FMT.IsFlowRange(FlowTray.LDConv01, FlowTray.LDConv31) == false)	return SM.SHM.InterlockMsgAdd("SM.FMT.IsFlowRange(teFlowTray.LDConv01, teFlowTray.LDConv15) == false");
				if (SM.FMT.IsFlowRange(FlowTray.LDLift01, FlowTray.LDLift31) == true)	return SM.SHM.InterlockMsgAdd("SM.FMT.IsFlowRange(teFlowTray.LDLift01, teFlowTray.LDLift15) == true");

				//안전 체크 
				if (G.DryRunIsBIn(DI.LDTrayLiftAreaSen1, false) == true)	return SM.SHM.InterlockMsgAdd("G.DryRunIsBIn(DI.LDTrayLiftAreaSen1,false) == true");
				if (G.DryRunIsBIn(DI.LDTrayLiftAreaSen2, false) == true)	return SM.SHM.InterlockMsgAdd("G.DryRunIsBIn(DI.LDTrayLiftAreaSen2,false) == true");
				if (G.cKernel.IsAIn(DI.LDTrayConvExistSen) == false)		return SM.SHM.InterlockMsgAdd("G.cKernel.IsAIn(DI.LDTrayConvExistSen) == false");
				if (G.cKernel.IsAIn(DI.LDTrayLiftDecSen) == true)			return SM.SHM.InterlockMsgAdd("G.cKernel.IsAIn(DI.ULDTrayLiftDecSen) == false");
				if (G.cKernel.IsAIn(DI.LDTrayLiftExistSen) == true)			return SM.SHM.InterlockMsgAdd("G.cKernel.IsAIn(DI.ULDTrayLiftExistSen) == false");
				if (G.cKernel.IsAIn(DI.LDTrayHolderGrip1Close) == true)		return SM.SHM.InterlockMsgAdd("G.cKernel.IsAIn(DI.LDTrayHolderGrip1Close) == true");
				if (G.cKernel.IsAIn(DI.LDTrayHolderGrip2Close) == true)		return SM.SHM.InterlockMsgAdd("G.cKernel.IsAIn(DI.LDTrayHolderGrip2Close) == true");

				if (arrMt[Axis.ConvWidth].IsBiggerThan((int)PosAlign.ALIGN) == true)	return SM.SHM.InterlockMsgAdd("arrMt[teAxis.ConvWidth].IsBiggerThan((int)tePosAlign.READY) == true");
				if (arrMt[Axis.LiftZ].IsRdy((int)PosZ.DOWN) == false)				return SM.SHM.InterlockMsgAdd("arrMt[teAxis.ConvWidth].IsBiggerThan((int)tePosLift.READY) == true");

				if (G.DryRunIsAIn(DI.LDTrayLiftExistSen, SM.FMT.IsFlowRange(FlowTray.LDLift01, FlowTray.LDLift31)) == true) return SM.SHM.InterlockMsgAdd("G.DryRunIsAIn(DI.LDTrayLiftExistSen, SM.FMT.IsFlowRange(teFlowTray.LDLift01, teFlowTray.LDLift15)) == true");

				SM.SHM.InterlockMsgReset();
				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public bool IsUnloadLiftUpEnable()
		{
			try
			{
				if (SM.FMT.IsFlowRange(FlowTray.LDLift01, FlowTray.LDLift31) == false)		return SM.SHM.InterlockMsgAdd("SM.FMT.IsFlowRange(teFlowTray.LDLift01, teFlowTray.LDLift15) == false");

				if (arrMt[Axis.LiftZ].IsRdyPos(arrMt[Axis.LiftZ].GetIndexPos((int)PosZ.DOWN)) == false) return SM.SHM.InterlockMsgAdd("arrMt[teAxis.TrayLift].IsRdy((int)tePosLift.UP) == true");
				if (G.DryRunIsBIn(DI.LDTrayLiftAreaSen1, false) == true)	return SM.SHM.InterlockMsgAdd("G.DryRunIsAIn(DI.LDTrayLiftAreaSen1, true) == false");
				if (G.DryRunIsBIn(DI.LDTrayLiftAreaSen2, false) == true)	return SM.SHM.InterlockMsgAdd("G.DryRunIsAIn(DI.LDTrayLiftAreaSen2, true) == false");

				//센서 감지여부 체크
				if (G.DryRunIsAIn(DI.LDTrayLiftExistSen, true) == false)	return SM.SHM.InterlockMsgAdd("G.DryRunIsAIn(DI.LDTrayLiftExistSen, true) == false");
				//if (G.DryRunIsAIn(DI.LDTrayLiftDecSen, true) == false)		return SM.SHM.InterlockMsgAdd("G.DryRunIsAIn(DI.LDTrayLiftLimitSen, true) == false");
				
				SM.SHM.InterlockMsgReset();
				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public bool IsUnloadLiftDownEnable()
		{
			try
			{
				//리프트 모터 위치 체크
				if (arrMt[Axis.LiftZ].IsRdy((int)PosZ.DOWN) == true) return SM.SHM.InterlockMsgAdd("arrMt[teAxis.TrayLift].IsRdy((int)tePosZ.DOWN) == true");
				
				if (G.DryRunIsBIn(DI.LDTrayLiftAreaSen1, false) == true)	return SM.SHM.InterlockMsgAdd("G.DryRunIsAIn(DI.LDTrayLiftAreaSen1, true) == false");
				if (G.DryRunIsBIn(DI.LDTrayLiftAreaSen2, false) == true)	return SM.SHM.InterlockMsgAdd("G.DryRunIsAIn(DI.LDTrayLiftAreaSen2, true) == false");

				//센서 감지여부 체크
				if (G.DryRunIsAIn(DI.LDTrayLiftDecSen, false) == true)		return SM.SHM.InterlockMsgAdd("G.DryRunIsAIn(DI.LDTrayLiftDecSen, true) == true");
				if (G.DryRunIsAIn(DI.LDTrayLiftExistSen, false) == true)	return SM.SHM.InterlockMsgAdd("G.DryRunIsAIn(DI.LDTrayLiftExistSen, true) == true");

				SM.SHM.InterlockMsgReset();
				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public bool IsLiftDownEnable()
		{
			try
			{
				//전면부만 체크
				if (G.DryRunIsBIn(DI.LDTrayLiftAreaSen1, false) == true)	return SM.SHM.InterlockMsgAdd("G.DryRunIsAIn(DI.LDTrayLiftAreaSen1, true) == false");
				//센서 감지여부 체크
				if (G.DryRunIsAIn(DI.LDTrayLiftExistSen, true) == false) return SM.SHM.InterlockMsgAdd("G.DryRunIsAIn(DI.LDTrayLiftDecSen, true) == false");

				SM.SHM.InterlockMsgReset();
				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public bool IsLiftUpEnable()
		{
			try
			{
				if (SM.FMT.IsFlowRange(FlowTray.LDLift01, FlowTray.LDLift31) == true)	return SM.SHM.InterlockMsgAdd("SM.FMT.IsFlowRange(teFlowTray.LDLift01, teFlowTray.LDLift15) == true");

				//센서 감지여부 체크
				if (G.DryRunIsAIn(DI.LDTrayLiftExistSen, SM.FMT.IsFlowRange(FlowTray.LDLift01, FlowTray.LDLift31)) == true)		return SM.SHM.InterlockMsgAdd("G.DryRunIsAIn(DI.LDTrayLiftExistSen, SM.FMT.IsFlowRange(teFlowTray.LDLift01, teFlowTray.LDLift15)) == true");
				if (G.DryRunIsAIn(DI.LDTrayLiftDecSen, SM.FMT.IsFlowRange(FlowTray.LDLift01, FlowTray.LDLift31)) == true)		return SM.SHM.InterlockMsgAdd("G.DryRunIsAIn(DI.LDTrayLiftDecSen, SM.FMT.IsFlowRange(teFlowTray.LDLift01, teFlowTray.LDLift15)) == true");

				SM.SHM.InterlockMsgReset();
				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public int CheckTraySize()
		{
			try
			{
				if (SM.FMT.IsFlowRange(FlowTray.LDConv01, FlowTray.LDConv31) == false) return 0;
				if (G.cKernel.IsAIn(DI.LDTrayConvExistSen) == false) return 0;
				if (G.cKernel.IsAIn(DI.LDTrayConvStopperSen) == false) return 0;

				if (SM.SHM.GetRecipeDataToInt(DEVICE.TraySizeChkSen3Use) == 0)
				{
					if (G.DryRunIsAIn(DI.LDTrayConvDecSen, false) == true)
					{
						return -1;
					}
				}
				else
				{
					if (G.DryRunIsAIn(DI.LDTrayConvDecSen, true) == false)
					{
						return -1;
					}
				}

				return 1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return 0;
			}
		}

        #endregion Public Function

        #region private Function



        #endregion
        
		
		private bool SetSeqRun(ref SMM<Cmd> smm)
		{
			try
			{
				if (this.IsReady == false) return false;

				smm.eInput = InState.run;
				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		private void SetRunLotInfoForUI()
		{
			try
			{
				if (SM.FMT.IsFlow(FlowTray.LDLift01) == false) return;

				// UI에 보여주기 용
				stRunLotInfo tsinfo = new stRunLotInfo();

				tsinfo.WorkOrder = SM.FMT[FlowTray.LDLift01].GetInfo().WorkOrder;
				tsinfo.KeyValue = SM.FMT[FlowTray.LDLift01].GetInfo().KeyValue;
				tsinfo.ProgramID_Top = SM.FMT[FlowTray.LDLift01].GetInfo().ProgramID_Top;
				tsinfo.ProgramID_Btm = SM.FMT[FlowTray.LDLift01].GetInfo().ProgramID_Btm;
				tsinfo.ProdCount = SM.FMT[FlowTray.LDLift01].GetInfo().MaxCntProdInLot.ToString();
				tsinfo.StartTime = DateTime.Now.ToString("MM.dd HH:mm:ss");

				SM.SHM.SetRunLotInfo(tsinfo);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public void Run()
		{
			try
			{
				UpdateAct(Seq.TrayLoader);

				if (SM.SHM.IsBitData(BIT.Error) == true || SM.SHM.IsBitData(BIT.Pause) == true)
				{
					if (arrMt[Axis.ConvLeft].IsRdy() == false ||
						arrMt[Axis.ConvRight].IsRdy() == false)
					{
						arrMt[Axis.ConvLeft].Stop();
						arrMt[Axis.ConvRight].Stop();
					}
					return;
				}

				Home(m_smmHome);
				Load(m_smmLoad);
				Unload(m_smmUnload);
				LiftDown(m_smmLiftDown);
				LiftUp(m_smmLiftUp);
				SWLiftUp(m_smmSWLiftUp);
				SWLiftDown(m_smmSWLiftDown);
				SM.SHM.SetSeqStatus(lstSMM);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void Home(SMM<Cmd> smm)
		{
			try
			{
				switch (smm.CurStep)
				{
					case Cmd.START:
						{
							if (smm.eInput == InState.run)
							{
								smm.eInput = InState.none;

								smm.Next(Cmd.HOME_10);
							}
						}
						break;

					case Cmd.HOME_10:
						{
							//안전 체크
							if (G.cKernel.IsAIn(DI.LDTrayLiftDecSen) == false && G.cKernel.IsAIn(DI.LDTrayLiftExistSen) == false)
							{
								if (G.DryRunIsBIn(DI.LDTrayLiftAreaSen1, false) == true)
								{
									AddAlarm(ALM.ERROR_SEQ_LOADER_LIFT_AREA_SENSOR1_CHECK);
									smm.Next(Cmd.END);
									break;
								}
								if (G.DryRunIsBIn(DI.LDTrayLiftAreaSen2, false) == true)
								{
									AddAlarm(ALM.ERROR_SEQ_LOADER_LIFT_AREA_SENSOR2_CHECK);
									smm.Next(Cmd.END);
									break;
								}
							}

							//if (SM.FMT.IsFlowRange(FlowTray.LDLift01, FlowTray.LDLift31) == false &&
							//	SM.FMT.IsFlow(FlowTray.LDWaitBuf) == false &&
							//	(G.cKernel.IsAIn(DI.LDTrayLiftDecSen) || G.cKernel.IsAIn(DI.LDTrayLiftExistSen)))
							//{
							//	AddAlarm(ALM.ERROR_SEQ_LOADER_LIFT_TRAY_MISMATCH);
							//	break;
							//}

							//if (SM.FMT.IsFlowRange(FlowTray.LDLift01, FlowTray.LDLift31) == true &&
							//	G.cKernel.IsAIn(DI.LDTrayLiftDecSen) == false && G.cKernel.IsAIn(DI.LDTrayLiftExistSen) == false)
							//{
							//	AddAlarm(ALM.ERROR_SEQ_LOADER_LIFT_TRAY_MISMATCH);
							//	break;
							//}

							if (m_IsMtHome == false)
							{
								smm.Next(Cmd.HOME_90);
								break;

							}
							smm.Next(Cmd.HOME_20);
						}
						break;

					case Cmd.HOME_20:
						{
							arrMt[Axis.LiftZ].ClearHome();
							arrMt[Axis.LiftZ].StartHoming();

							smm.Next(Cmd.HOME_30);
						}
						break;

					case Cmd.HOME_30:
						{
							if (arrMt[Axis.LiftZ].IsRdy() == false ||
								arrMt[Axis.LiftZ].IsHomeDone() == false) break;

							smm.Next(Cmd.HOME_40);
						}
						break;

					case Cmd.HOME_40:
						{
							smm.Next(Cmd.HOME_50);
						}
						break;

					case Cmd.HOME_50:
						{
							arrMt[Axis.ConvWidth].ClearHome();
							arrMt[Axis.ConvStopper].ClearHome();

							arrMt[Axis.ConvWidth].StartHoming();
							arrMt[Axis.ConvStopper].StartHoming();

							smm.Next(Cmd.HOME_60);
						}
						break;

					case Cmd.HOME_60:
						{
							if (arrMt[Axis.ConvWidth].IsRdy() == false || arrMt[Axis.ConvWidth].IsHomeDone() == false) break;
							if (arrMt[Axis.ConvStopper].IsRdy() == false || arrMt[Axis.ConvStopper].IsHomeDone() == false) break;

							smm.Next(Cmd.HOME_70);
						}
						break;

					case Cmd.HOME_70:
						{
							//현재 위치 홈동작
							arrMt[Axis.ConvLeft].ClearHome();
							arrMt[Axis.ConvLeft].StartHoming();
							arrMt[Axis.ConvRight].ClearHome();
							arrMt[Axis.ConvRight].StartHoming();

							smm.Next(Cmd.HOME_80);
						}
						break;

					case Cmd.HOME_80:
						{
							if (arrMt[Axis.ConvLeft].IsRdy() == false || arrMt[Axis.ConvLeft].IsHomeDone() == false) break;
							if (arrMt[Axis.ConvRight].IsRdy() == false || arrMt[Axis.ConvRight].IsHomeDone() == false) break;

							smm.Next(Cmd.HOME_90);
						}
						break;

					case Cmd.HOME_90:
						{
							if (arrMt[Axis.ConvWidth].IsRdy((int)PosAlign.PNP) == false)
							{
								arrMt[Axis.ConvWidth].MoveIndex((int)PosAlign.PNP);
								break;
							}

							//트레이 로드 중 정지 된 경우
							if (SM.FMT.IsFlowRange(FlowTray.LDLift01, FlowTray.LDLift31) == true &&
								G.cKernel.IsAIn(DI.LDTrayLiftExistSen) == false)
							{
								smm.Next(Cmd.HOME_91);
								break;
							}

							smm.Next(Cmd.HOME_100);
						}
						break;


					case Cmd.HOME_91:
						{
							if (arrMt[Axis.LiftZ].IsRdy((int)PosZ.DOWN) == false)
							{
								arrMt[Axis.LiftZ].MoveIndex((int)PosZ.DOWN);
								break;
							}

							smm.Next(Cmd.HOME_92);
						}
						break;

					case Cmd.HOME_92:
						{
							//속도가 동일해야 함으로, Left속도를 동일하게 적용
							if (arrMt[Axis.ConvLeft].IsRdy() == true || arrMt[Axis.ConvLeft].IsRdy() == true)
							{
								arrMt[Axis.ConvLeft].MoveJog(false, arrMt[Axis.ConvLeft].GetVel((int)PosConv.UNLOAD_SLOW));
								arrMt[Axis.ConvRight].MoveJog(false, arrMt[Axis.ConvLeft].GetVel((int)PosConv.UNLOAD_SLOW));
							}

							if (G.DryRunIsAIn(DI.LDTrayLiftExistSen, true) == false) break;

							smm.Next(Cmd.HOME_93);
						}
						break;

					case Cmd.HOME_93:
						{
							if (smm.Delayms(SM.SHM.GetRecipeDataToInt(DEVICE.TrayLoaderConvUnloadDelay)/1000) == false) break; 

							arrMt[Axis.ConvLeft].Stop();
							arrMt[Axis.ConvRight].Stop();

							smm.Next(Cmd.HOME_100);
						}
						break;

					case Cmd.HOME_100:
						{
							if (arrMt[Axis.ConvStopper].IsRdy((int)PosStopper.READY) == false)
							{
								arrMt[Axis.ConvStopper].MoveIndex((int)PosStopper.READY);
								break;
							}

							if (arrAct[Act.FrontStopper].IsPos(SenPos.FWD) == false)
							{
								arrAct[Act.FrontStopper].SetPos(SenPos.FWD);
							}
							//Flow존재 시 다운상태로 시작
							if (SM.FMT.IsFlowRange(FlowTray.LDLift01, FlowTray.LDLift31) == true)
							{
								if (G.DryRunIsAIn(DI.LDTrayLiftExistSen, true) == false && G.DryRunIsAIn(DI.LDTrayLiftDecSen, true) == false)
								{
									AddAlarm(ALM.ERROR_FLOW_MANAGER_CHECK);
									break;
								}
								if (m_IsDownComplete == true)
								{
									//이미 이전에 다운 완료 했으면 다운 위치로 이동

									smm.Next(Cmd.HOME_130);
									break;
								}
								else
								{
									smm.Next(Cmd.HOME_110);
									break;
								}
								//m_bAutoRun = true;
								//m_smmLiftDown.eInput = InState.run;
							}
							else
							{
								m_IsDownComplete = false; //제품 없을땐 초기화
								smm.Next(Cmd.HOME_110);
								break;
							}
						}

					case Cmd.HOME_110:
						{
							if (arrMt[Axis.LiftZ].IsRdy((int)PosZ.UP) == false)
							{
								arrMt[Axis.LiftZ].MoveIndex((int)PosZ.UP);
								break;
							}

							smm.Next(Cmd.END);
						}
						break;

					case Cmd.HOME_120:
						{

							if (m_smmLiftDown.IsReady == false) break;

							smm.Next(Cmd.END);
						}
						break;

					case Cmd.HOME_130:
						{
							if (arrAct[Act.FrontStopper].IsPos(SenPos.FWD) == false)
							{
								arrAct[Act.FrontStopper].SetPos(SenPos.FWD);
								break;
							}
							if (arrMt[Axis.LiftZ].IsInPosIndex((int)PosZ.DOWN) == false)
							{
								arrMt[Axis.LiftZ].MoveIndex((int)PosZ.DOWN);
								break;
							}

							smm.Next(Cmd.END);
						}
						break;

					case Cmd.END:
						{
							smm.Next(Cmd.START);
						}
						break;
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		//Z축 하강운동, 버튼 클릭시 동작
		private void LiftDown(SMM<Cmd> smm)
		{
			try
			{
				switch (smm.CurStep)
				{
					case Cmd.START:
						{
							if (smm.eInput == InState.run)
							{
								smm.eInput = InState.none;

								smm.Next(Cmd.LIFTDOWN_10);
							}
						}
						break;

					case Cmd.LIFTDOWN_10:
						{
							// LOT 유무 확인, 없으면 알람
							if (m_bAutoRun == true)
							{
								if (G.DryRunIsAIn(DI.LDTrayLiftDecSen, true) == false && 
									G.DryRunIsAIn(DI.LDTrayLiftExistSen, true) == false)
								{
									AddAlarm(ALM.ERROR_SEQ_LOADER_LIFT_TRAY_NONE);
									smm.Next(Cmd.END);
									break;
								}
							}
                            if (NDm.Read(eNDM.AutoRunMode) != (int)AutoRunMode.Normal)
                            {
                                smm.Next(Cmd.LIFTDOWN_30);
                                break;
                                
                            }
							smm.Next(Cmd.LIFTDOWN_11);
						}
						break;

					case Cmd.LIFTDOWN_11:
						{
							if (arrMt[Axis.LiftZ].IsRdy((int)PosZ.UP) == false)
							{
								arrMt[Axis.LiftZ].MoveIndex((int)PosZ.UP);
								break;
							}
							m_nTrayCountStartPos = 0;
							m_nTrayCountEndPos = 0;

                            if (Program._bSimul_ == true)
                            {
                                smm.Next(Cmd.LIFTDOWN_20);
                                break;
                            }

                            //smm.Next(Cmd.LIFTDOWN_20);
                            smm.Next(Cmd.LIFTDOWN_12);
						}
						break;

					case Cmd.LIFTDOWN_12:
						{            
                            //UP 포지션에서 LDTrayLiftCountSen 감지 안되면 알람
                            if (G.DryRunIsAIn(DI.LDTrayLiftCountSen, true) == false)
                            {
                                m_Alarm = ALM.ERROR_SEQ_LOADER_LIFT_TRAY_MISMATCH;
                                smm.Next(Cmd.END_NG);
                                break;
                            }
                            else
                            {
                                m_nTrayCountStartPos = arrMt[Axis.LiftZ].GetIndexPos((int)PosZ.UP) - SM.SHM.GetRecipeDataToInt(DEVICE.CountSensorDistance);
                                smm.Next(Cmd.LIFTDOWN_13);
                            }
						}
						break;

					case Cmd.LIFTDOWN_13:
						{
                            if (arrMt[Axis.LiftZ].IsRdy() == true)
                            {
                                arrMt[Axis.LiftZ].MoveJog(false, arrMt[Axis.LiftZ].GetVel((int)PosZ.COUNT_TRAY_SPEED));
                            }

                            //계속 감지 되면 알람 처리
                            if (arrMt[Axis.LiftZ].GetLocalPos() < (arrMt[Axis.LiftZ].GetIndexPos((int)PosZ.DOWN) + 10000))
                            {
                                arrMt[Axis.LiftZ].Stop();
                                m_Alarm = ALM.ERROR_SEQ_LOADER_LIFT_TRAY_MISMATCH;
                                smm.Next(Cmd.END_NG);
                                break;
                            }

                            //감지가 안되면 정지
                            if (G.DryRunIsAIn(DI.LDTrayLiftCountSen, false) == false)
                            {
                                arrMt[Axis.LiftZ].Stop();
                                m_nTrayCountEndPos = arrMt[Axis.LiftZ].GetLocalPos();
                                smm.Next(Cmd.LIFTDOWN_14);
                            }

                            
						}
						break;

					case Cmd.LIFTDOWN_14:
						{
							int nDiff = m_nTrayCountStartPos - m_nTrayCountEndPos;
							int nTrayHeight = SM.SHM.GetRecipeDataToInt(DEVICE.TrayHeight);
							int nCount = 0;
							if (nDiff > nTrayHeight)
							{
								//쌓여있는 트레이 중 1개는 쌓여있는 높이가 아니라, 일반 높이이므로, 아래와 같이 계산 진행
								nDiff -= nTrayHeight;
								nCount = (int)Math.Round((double)nDiff / SM.SHM.GetRecipeDataToInt(DEVICE.TrayStackedHeight));
								nCount++;
							}
							else
							{
								//1개 투입 시 센서 오차 반영
								if (nDiff > nTrayHeight * 0.8)
								{
									nCount = 1;
								}
							}
							
							if (nCount != SM.FMT.GetExistCountRange(FlowTray.LDLift01, FlowTray.LDLift31))
							{
								arrMt[Axis.LiftZ].Stop();
								m_Alarm = ALM.ERROR_SEQ_LOADER_LIFT_TRAY_MISMATCH;
								GLog.Add($"Number of trays detected by the sensor: {nCount}   Number of trays in data:{SM.FMT.GetExistCountRange(FlowTray.LDLift01, FlowTray.LDLift31)}");
								smm.Next(Cmd.END_NG);
								break;
							}


							smm.Next(Cmd.LIFTDOWN_20);
						}
						break;
						

					case Cmd.LIFTDOWN_20:
						{
							FlowTray flowUPT = SM.FMT.GetFirstExistFlow(FlowTray.LDLift01, FlowTray.LDLift31);
							if (flowUPT == FlowTray.Empty)
								AddAlarm(ALM.ERROR_FLOW_MANAGER_CHECK);

							InfoTray.tsInfo info_upt = SM.FMT[flowUPT].GetInfo();
							
							if (SM.SHM.ContainsRunLotInfo(info_upt.KeyValue) == true)
							{
								smm.Next(Cmd.LIFTDOWN_EMPTYCHECK_10);
								break;
							}

							stRunLotInfo Lotinfo = new stRunLotInfo();
							Lotinfo = SM.SHM.GetRunLotInfo(info_upt.KeyValue);

							Lotinfo.KeyValue		= info_upt.KeyValue;
							Lotinfo.ProdCount		= info_upt.MaxCntProdInLot.ToString();
							Lotinfo.WorkOrder		= info_upt.WorkOrder;
							Lotinfo.ProgramID_Top	= info_upt.ProgramID_Top;
							Lotinfo.ProgramID_Btm	= info_upt.ProgramID_Btm;
							Lotinfo.UserID			= UI.strMemberID;
							Lotinfo.StartTime = new DateTime(info_upt.StartTime).ToString("MM dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

							SM.SHM.SetRunLotInfo(Lotinfo);

							string str = "";
							if (Program._bSimul_ == true)
							{
								str = "SIM - ";
							}
							str = str + string.Format($"[LOT INFO WRITE] WorkOrder = {Lotinfo.WorkOrder} Key Value = {Lotinfo.KeyValue} ProgramID Top = {Lotinfo.ProgramID_Top} ProgramID Btm =  {Lotinfo.ProgramID_Btm} Prod Count =     {Lotinfo.ProdCount}" );

							UI.Logs.Write(LOGPART.DATA, str);
							UI.Logs.Write(LOGPART.SYS, $"LOT START (ID : {Lotinfo.WorkOrder})");
							
							smm.Next(Cmd.LIFTDOWN_EMPTYCHECK_10);
						}
						break;
                        

					case Cmd.LIFTDOWN_EMPTYCHECK_10:
                        {
                            if (SM.SHM.GetUseSkipData(USESKIP.SmartVisionLoader) == false)
                            {
                                SM.FMT[FlowTray.LDLift01].ProcStatus = ProcStatus.TrayEmptyDone;
                                smm.Next(Cmd.LIFTDOWN_JOBCHECK_10);
                                break;
                            }

                            int nPosCheck = arrMt[Axis.LiftZ].GetIndexPos((int)PosZ.EMPTY_CHECK);
							int nTrayCount = SM.FMT.GetExistCountRange(FlowTray.LDLift01 , FlowTray.LDLift31);
							int nTotalHeight = (nTrayCount - 1) * SM.SHM.GetRecipeDataToInt(DEVICE.TrayStackedHeight);//트레이가 1개 있을때 기준으로 티칭했기때문에 맨위 트레이를 뺀 높이로 생각

							m_nLiftTartgetPos = nPosCheck - nTotalHeight;

							smm.Next(Cmd.LIFTDOWN_EMPTYCHECK_20);
						}
						break;
						
						
					case Cmd.LIFTDOWN_EMPTYCHECK_20:
						{

							if (arrMt[Axis.LiftZ].IsRdyPos(m_nLiftTartgetPos) == false)
							{
								arrMt[Axis.LiftZ].MoveABS(m_nLiftTartgetPos, (int)PosZ.EMPTY_CHECK);
								break;
							}
							m_smmFTVInspRetry = 0;
							smm.Next(Cmd.LIFTDOWN_EMPTYCHECK_30);
						}
						break;

					case Cmd.LIFTDOWN_EMPTYCHECK_30:
                        {
                            if (smm.Delayms(500) == false) break;
                            G.commFTVLoader.Send(CommFTV.IV4Command.TriggerAndGetResult);

							smm.Next(Cmd.LIFTDOWN_EMPTYCHECK_40);
						}
						break;

					case Cmd.LIFTDOWN_EMPTYCHECK_40:
						{
							//스캔 확인
							if (G.commFTVLoader.TriggerRcvCheck == false)
							{
								if (smm.Delayms(1000) == false) break;

								smm.Next(Cmd.LIFTDOWN_EMPTYCHECK_50);

								break;
							}

							switch (G.commFTVLoader.RstEmptyCheck)
							{
								case CommFTV.FTVResult.DATA_NG:
									{
										m_Alarm = ALM.ERROR_COMM_FTV_DATA;
										smm.Next(Cmd.END_NG);
										return;
									}
								case CommFTV.FTVResult.RESULT_NG:
									{
										m_Alarm = ALM.ERROR_SEQ_FIRST_TRAY_NEED_EMPTY;
										smm.Next(Cmd.END_NG);
										return;
									}
								case CommFTV.FTVResult.OK:
									{
										SM.FMT[FlowTray.LDLift01].ProcStatus = ProcStatus.TrayEmptyDone;
										smm.Next(Cmd.LIFTDOWN_JOBCHECK_10);
									}
									break;
							}

						}
						break;
						
					case Cmd.LIFTDOWN_EMPTYCHECK_50:
						{
							m_smmFTVInspRetry++;
							if (m_smmFTVInspRetry >= 3) // 검사결과 체크 3회 실패시 알람후 종료.
							{
								m_Alarm = ALM.ERROR_MAIN_SEQ_FTV_INSP_RESULT_CHECK;// FTV Insp 검사결과 체크 알람
								smm.Next(Cmd.END_NG);
								break;
							}

							smm.Next(Cmd.LIFTDOWN_EMPTYCHECK_30);
						}
						break;

                    case Cmd.LIFTDOWN_JOBCHECK_10:
						{

							string strMsg = $"{SM.FMT[FlowTray.LDLift01].GetInfo().ProgramID_Top}@{SM.FMT[FlowTray.LDLift01].GetInfo().ProgramID_Btm}";
							if (SM.SHM.GetUseSkipData(USESKIP.TopInspection) == true)
								G.commInspTop.SendJobCheckRequest(strMsg);

							if (SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == true)
								G.commInspBtm.SendJobCheckRequest(strMsg);

							smm.Next(Cmd.LIFTDOWN_JOBCHECK_20);
						}
						break;

					case Cmd.LIFTDOWN_JOBCHECK_20:
						{
							if (SM.SHM.GetUseSkipData(USESKIP.TopInspection) == false)
							{
								smm.Next(Cmd.LIFTDOWN_JOBCHECK_30);
								break;
							}

							bool bOK = false;
							int nret = G.commInspTop.RcvJobCheckReport(ref bOK);

							if (nret > 0)
							{
								if (bOK == true)
								{
									smm.Next(Cmd.LIFTDOWN_JOBCHECK_30);
								}
								else
								{
									if (NDm.Read(eNDM.AutoRunMode) == (int)AutoRunMode.EndLot ||
										NDm.Read(eNDM.AutoRunMode) == (int)AutoRunMode.QuitLot)
									{
										smm.Next(Cmd.LIFTDOWN_30);
										break;
									}

									SM.SHM.MsgShow(SeqMsg.JOBCHECKNG);
									G.AlmMng.ADD(ALM.ERROR_COMM_TOP_JOB_EXIST);
									string strResult = SM.SHM.PopupMsgResult(SeqMsg.JOBCHECKNG);
									switch (strResult)
									{
										case "CLOSE":
											smm.Next(Cmd.END_NG_RECIPE);
											break;
									}
								}
							}
							else if (smm.Delayms(30000) == true)
							{
								G.AlmMng.ADD(ALM.ERROR_SEQ_INSP_TOP_PC_COMM_TIMEOUT);
								smm.Next(Cmd.LIFTDOWN_JOBCHECK_10);
							}
						}
						break;

					case Cmd.LIFTDOWN_JOBCHECK_30:
						{
							if (SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == false)
							{
								smm.Next(Cmd.LIFTDOWN_30);
								break;
							}

							bool bOK = false;
							int nret = G.commInspBtm.RcvJobCheckReport(ref bOK);

							if (nret > 0)
							{
								if (bOK == true)
								{
									smm.Next(Cmd.LIFTDOWN_30);
								}
								else
								{
									SM.SHM.MsgShow(SeqMsg.JOBCHECKNG);
									G.AlmMng.ADD(ALM.ERROR_COMM_TOP_JOB_EXIST);
									string strResult = SM.SHM.PopupMsgResult(SeqMsg.JOBCHECKNG);
									switch (strResult)
									{
										case "CLOSE":
											smm.Next(Cmd.END_NG_RECIPE);
											break;
									}
								}
							}
							else if (smm.Delayms(5000) == true)
							{
								G.AlmMng.ADD(ALM.ERROR_SEQ_INSP_BTM_PC_COMM_TIMEOUT);
								smm.Next(Cmd.LIFTDOWN_JOBCHECK_10);
							}

						}
						break;

						case Cmd.LIFTDOWN_30:
						{
							if (arrMt[Axis.LiftZ].IsRdy((int)PosZ.DOWN) == false)
							{
								arrMt[Axis.LiftZ].MoveIndex((int)PosZ.DOWN);
								break;
							}
							
							smm.Next(Cmd.END);
						}
						break;
						
					case Cmd.END_NG_RECIPE:
						{
							if (arrMt[Axis.LiftZ].IsRdy((int)PosZ.UP) == false)
							{
								arrMt[Axis.LiftZ].MoveIndex((int)PosZ.UP);
								break;
							}

							G.AlmMng.ADD(ALM.ERROR_FIND_RECIPE);
							smm.Next(Cmd.START);
						}
						break;
						
					case Cmd.END_NG:
						{
							if (arrMt[Axis.LiftZ].IsRdy((int)PosZ.UP) == false)
							{
								arrMt[Axis.LiftZ].MoveIndex((int)PosZ.UP);
								break;
							}
							
							AddAlarm(m_Alarm);
							smm.Next(Cmd.START);
						}
						break;
						

					case Cmd.END:
						{
							m_IsDownComplete = true;
							smm.Next(Cmd.START);
						}
						break;
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void LiftUp(SMM<Cmd> smm)
		{
			try
			{
				switch (smm.CurStep)
				{
					case Cmd.START:
						{
							if (smm.eInput == InState.run)
							{
								smm.eInput = InState.none;

								smm.Next(Cmd.LIFTUP_10);
							}
						}
						break;

					case Cmd.LIFTUP_10:
						{
							if (arrMt[Axis.LiftZ].IsRdy() == false)
							{
								break;
							}

							if (G.DryRunIsBIn(DI.LDTrayLiftAreaSen1, false) == true)
							{
								AddAlarm(ALM.ERROR_SEQ_LOADER_LIFT_AREA_SENSOR1_CHECK);
								smm.Next(Cmd.END);
								break;
							}
							if (G.DryRunIsBIn(DI.LDTrayLiftAreaSen2, false) == true)
							{
								AddAlarm(ALM.ERROR_SEQ_LOADER_LIFT_AREA_SENSOR2_CHECK);
								smm.Next(Cmd.END);
								break;
							}
							if (G.cKernel.IsAIn(DI.LDTrayLiftExistSen) == true ||
								G.cKernel.IsAIn(DI.LDTrayLiftDecSen) == true)
							{
								if (m_bAutoRun == true)
								{
									AddAlarm(ALM.ERROR_SEQ_LOADER_LIFT_TRAY_EXIST);
									smm.Next(Cmd.END);
									break;
								}
							}

							smm.Next(Cmd.LIFTUP_20);
						}
						break;

					case Cmd.LIFTUP_20:
						{
							if (arrAct[Act.FrontStopper].IsPos(SenPos.FWD) == false)
							{
								arrAct[Act.FrontStopper].SetPos(SenPos.FWD);
								break;
							}

							smm.Next(Cmd.LIFTUP_30);
						}
						break;


					case Cmd.LIFTUP_30:       //감지되거나 up까지
						{
							if (G.DryRunIsBIn(DI.LDTrayLiftAreaSen2, false) == true)
							{
								arrMt[Axis.LiftZ].Stop();
								//감지한 경우 그 위치에서 5개 높이 만큼 이동 
								int nTrayHeight = SM.SHM.GetRecipeDataToInt(DEVICE.TrayHeight);
								m_nLiftTartgetPos = arrMt[Axis.LiftZ].GetLocalPos() + nTrayHeight * 5 - 90000;

								if (m_nLiftTartgetPos >= arrMt[Axis.LiftZ].GetIndexPos((int)PosZ.UP))
									m_nLiftTartgetPos = arrMt[Axis.LiftZ].GetIndexPos((int)PosZ.UP);

								smm.Next(Cmd.LIFTUP_40);
								break;
							}

							if (G.DryRunIsBIn(DI.LDTrayLiftAreaSen2, false) == false && arrMt[Axis.LiftZ].IsRdy((int)PosZ.UP) == false)
							{
								arrMt[Axis.LiftZ].MoveIndex((int)PosZ.UP, (int)PosZ.UP);
								break;
							}

							//UP 위치 까지 간 경우 끝
							if (arrMt[Axis.LiftZ].IsRdy((int)PosZ.UP) == true)
							{
								smm.Next(Cmd.END);
								break;
							}
						}
						break;

					case Cmd.LIFTUP_40:
						{
							if (arrMt[Axis.LiftZ].IsRdyPos(m_nLiftTartgetPos) == false)
							{
								arrMt[Axis.LiftZ].MoveABS(m_nLiftTartgetPos, (int)PosZ.UP);
								break;
							}
							smm.Next(Cmd.END);
						}
						break;

					case Cmd.END:
						{
							smm.Next(Cmd.START);
						}
						break;
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
		//IN / OUT
		private void Load(SMM<Cmd> smm)
		{
			try
			{
				switch (smm.CurStep)
				{
					case Cmd.START:
						{
							if (smm.eInput == InState.run)
							{
								smm.eInput = InState.none;

								smm.Next(Cmd.LOAD_10);
							}
						}
						break;

					case Cmd.LOAD_10:
						{
							SM.SHM.RemoveLotYield(SM.FMT[FlowTray.LDLift01].GetInfo().KeyValue);

							SetRunLotInfoForUI();

							smm.Next(Cmd.LOAD_20);
						}
						break;

					case Cmd.LOAD_20:
						{
							//자재 갯수 확인, 초과되어 감지시 정지
							if (G.cKernel.IsBIn(DI.LDTrayLiftLimitSen) == true)
							{
								AddAlarm(ALM.ERROR_SEQ_LOADER_LIFT_LIMIT_SENSOR_CHECK);
								smm.Next(Cmd.END);
								break;
							}

							//센서 체크
							if (G.DryRunIsAIn(DI.LDTrayConvExistSen, false) == true) break;

							if (arrMt[Axis.ConvStopper].IsRdy((int)PosStopper.STOPPER) == false)
							{
								arrMt[Axis.ConvStopper].MoveIndex((int)PosStopper.STOPPER);
								break;
							}

							smm.Next(Cmd.LOAD_30);
						}
						break;

					case Cmd.LOAD_30:
						{
							//안전 체크 
							if (G.DryRunIsBIn(DI.LDTrayLiftAreaSen1, false) == true)
							{
								AddAlarm(ALM.ERROR_SEQ_LOADER_LIFT_AREA_SENSOR1_CHECK);
								smm.Next(Cmd.END);
								break;
							}
							if (G.DryRunIsBIn(DI.LDTrayLiftAreaSen2, false) == true)
							{
								AddAlarm(ALM.ERROR_SEQ_LOADER_LIFT_AREA_SENSOR2_CHECK);
								smm.Next(Cmd.END);
								break;
							}
							if (G.DryRunIsAIn(DI.LDTrayLiftDecSen, true) == false && G.DryRunIsAIn(DI.LDTrayLiftExistSen, true) == false)
							{
								AddAlarm(ALM.ERROR_SEQ_LOADER_LIFT_TRAY_NONE);
								smm.Next(Cmd.END);
								break;
							}
							if (arrMt[Axis.LiftZ].IsRdy((int)PosZ.DOWN) == false)
							{
								arrMt[Axis.LiftZ].MoveIndex((int)PosZ.DOWN);
								break;
							}

							if (arrMt[Axis.ConvWidth].IsRdy((int)PosAlign.READY) == false)
							{
								arrMt[Axis.ConvWidth].MoveIndex((int)PosAlign.READY);
								break;
							}

							smm.Next(Cmd.LOAD_40);
						}
						break;

					case Cmd.LOAD_40:
						{
							//속도가 동일해야 함으로, Left속도를 동일하게 적용
							if (arrMt[Axis.ConvLeft].IsRdy() == true || arrMt[Axis.ConvLeft].IsRdy() == true)
							{
								arrMt[Axis.ConvLeft].MoveJog(true, arrMt[Axis.ConvLeft].GetVel((int)PosConv.LOAD_FAST));
								arrMt[Axis.ConvRight].MoveJog(true, arrMt[Axis.ConvLeft].GetVel((int)PosConv.LOAD_FAST));
							}

							if (smm.Delayms(15000) == true)
							{
								AddAlarm(ALM.ERROR_SEQ_LOADER_CONV_TRAY_NONE);
								smm.ResetDelayms();
								break;
							}
							if (G.DryRunIsAIn(DI.LDTrayConvDecSen, true) == false) break;

							arrMt[Axis.ConvLeft].MoveJog(true, arrMt[Axis.ConvLeft].GetVel((int)PosConv.LOAD_SLOW));
							arrMt[Axis.ConvRight].MoveJog(true, arrMt[Axis.ConvLeft].GetVel((int)PosConv.LOAD_SLOW));

							smm.Next(Cmd.LOAD_50);
						}
						break;

					case Cmd.LOAD_50:
						{
							if (arrMt[Axis.ConvLeft].IsRdy() == true || arrMt[Axis.ConvLeft].IsRdy() == true)
							{
								arrMt[Axis.ConvLeft].MoveJog(true, arrMt[Axis.ConvLeft].GetVel((int)PosConv.LOAD_SLOW));
								arrMt[Axis.ConvRight].MoveJog(true, arrMt[Axis.ConvLeft].GetVel((int)PosConv.LOAD_SLOW));
							}

							if (G.DryRunIsAIn(DI.LDTrayConvStopperSen, true) == false) break;

							smm.Next(Cmd.LOAD_60);
						}
						break;

					case Cmd.LOAD_60:
						{
							if (arrMt[Axis.ConvLeft].IsRdy() == true || arrMt[Axis.ConvLeft].IsRdy() == true)
							{
								arrMt[Axis.ConvLeft].MoveJog(true, arrMt[Axis.ConvLeft].GetVel((int)PosConv.LOAD_SLOW));
								arrMt[Axis.ConvRight].MoveJog(true, arrMt[Axis.ConvLeft].GetVel((int)PosConv.LOAD_SLOW));
							}

							if (smm.Delayms(SM.SHM.GetRecipeDataToInt(DEVICE.TrayLoaderConvLoadDelay)/1000) == false) break; 

							smm.Next(Cmd.LOAD_70);
						}
						break;

					case Cmd.LOAD_70:
						{
							arrMt[Axis.ConvLeft].Stop();
							arrMt[Axis.ConvRight].Stop();

							SM.FMT.NextLot(FlowTray.LDLift01, FlowTray.LDConv01);

							smm.Next(Cmd.LOAD_80);
						}
						break;

					case Cmd.LOAD_80:
						{
							if (arrMt[Axis.ConvWidth].IsRdy((int)PosAlign.ALIGN) == false)
							{
								arrMt[Axis.ConvWidth].MoveIndex((int)PosAlign.ALIGN);
								break;
							}

							smm.Next(Cmd.LOAD_90);
						}
						break;

					case Cmd.LOAD_90:
						{
							if (smm.Delayms(NDm.Read(eNDM.LDLiftAlignDelay)) == false) break;

							if (arrMt[Axis.ConvWidth].IsRdy((int)PosAlign.PNP) == false)
							{
								arrMt[Axis.ConvWidth].MoveIndex((int)PosAlign.PNP);
								break;
							}

							smm.Next(Cmd.LOAD_100);
						}
						break;

					case Cmd.LOAD_100:
						{
							if (arrMt[Axis.ConvStopper].IsRdy((int)PosStopper.READY) == false)
							{
								arrMt[Axis.ConvStopper].MoveIndex((int)PosStopper.READY);
								break;
							}
							m_IsDownComplete = false;
							smm.Next(Cmd.END);
						}
						break;

					case Cmd.END:
						{
							smm.Next(Cmd.START);
						}
						break;
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void Unload(SMM<Cmd> smm)
		{
			try
			{
				switch (smm.CurStep)
				{
					case Cmd.START:
						{
							if (smm.eInput == InState.run)
							{
								smm.eInput = InState.none;

								smm.Next(Cmd.UNLOAD_10);
							}
						}
						break;

					case Cmd.UNLOAD_10:
						{
							//센서 체크 
							if (G.cKernel.IsAIn(DI.LDTrayLiftExistSen) == true)
							{
								AddAlarm(ALM.ERROR_SEQ_LOADER_LIFT_TRAY_EXIST);
								smm.Next(Cmd.END);
								break;
							}

							if (arrMt[Axis.LiftZ].IsRdy((int)PosZ.DOWN) == false)
							{
								arrMt[Axis.LiftZ].MoveIndex((int)PosZ.DOWN);
								break;
							}

							smm.Next(Cmd.UNLOAD_20);
						}
						break;

					case Cmd.UNLOAD_20:
						{
							//안전 체크 
							if (G.DryRunIsBIn(DI.LDTrayLiftAreaSen1, false) == true)
							{
								AddAlarm(ALM.ERROR_SEQ_LOADER_LIFT_AREA_SENSOR1_CHECK);
								smm.Next(Cmd.END);
								break;
							}
							if (G.DryRunIsBIn(DI.LDTrayLiftAreaSen2, false) == true)
							{
								AddAlarm(ALM.ERROR_SEQ_LOADER_LIFT_AREA_SENSOR2_CHECK);
								smm.Next(Cmd.END);
								break;
							}

							if (arrMt[Axis.ConvWidth].IsRdy((int)PosAlign.READY) == false)
							{
								arrMt[Axis.ConvWidth].MoveIndex((int)PosAlign.READY);
								break;
							}

							if (arrAct[Act.FrontStopper].IsPos(SenPos.BWD) == false)
							{
								arrAct[Act.FrontStopper].SetPos(SenPos.BWD);
								break;
							}

							smm.Next(Cmd.UNLOAD_30);
						}
						break;

					case Cmd.UNLOAD_30:
						{
							//속도가 동일해야 함으로, Left속도를 동일하게 적용
							if (arrMt[Axis.ConvLeft].IsRdy() == true || arrMt[Axis.ConvLeft].IsRdy() == true)
							{
								arrMt[Axis.ConvLeft].MoveJog(false, arrMt[Axis.ConvLeft].GetVel((int)PosConv.UNLOAD_FAST));
								arrMt[Axis.ConvRight].MoveJog(false, arrMt[Axis.ConvLeft].GetVel((int)PosConv.UNLOAD_FAST));
							}

							if (G.DryRunIsAIn(DI.LDTrayLiftDecSen, true) == false) break;

							arrMt[Axis.ConvLeft].MoveJog(false, arrMt[Axis.ConvLeft].GetVel((int)PosConv.UNLOAD_SLOW));
							arrMt[Axis.ConvRight].MoveJog(false, arrMt[Axis.ConvLeft].GetVel((int)PosConv.UNLOAD_SLOW));

							smm.Next(Cmd.UNLOAD_40);
						}
						break;

					case Cmd.UNLOAD_40:
						{
							//속도가 동일해야 함으로, Left속도를 동일하게 적용
							if (arrMt[Axis.ConvLeft].IsRdy() == true || arrMt[Axis.ConvLeft].IsRdy() == true)
							{
								arrMt[Axis.ConvLeft].MoveJog(false, arrMt[Axis.ConvLeft].GetVel((int)PosConv.UNLOAD_SLOW));
								arrMt[Axis.ConvRight].MoveJog(false, arrMt[Axis.ConvLeft].GetVel((int)PosConv.UNLOAD_SLOW));
							}

							if (G.DryRunIsAIn(DI.LDTrayLiftExistSen, true) == false) break;

							smm.Next(Cmd.UNLOAD_50);
						}
						break;

					case Cmd.UNLOAD_50:
						{
							if (smm.Delayms(SM.SHM.GetRecipeDataToInt(DEVICE.TrayLoaderConvUnloadDelay)/1000) == false) break; 

							arrMt[Axis.ConvLeft].Stop();
							arrMt[Axis.ConvRight].Stop();

							smm.Next(Cmd.UNLOAD_60);
						}
						break;

					case Cmd.UNLOAD_60:
						{
							//메모리 이동
							SM.FMT.NextLot(FlowTray.LDConv01, FlowTray.LDLift01);

							smm.Next(Cmd.END);
						}
						break;

					case Cmd.END:
						{
							smm.Next(Cmd.START);
						}
						break;
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void SWLiftUp(SMM<Cmd> smm)
		{
			try
			{
				switch (smm.CurStep)
				{
					case Cmd.START:
						{
							if (smm.eInput == InState.run)
							{
								smm.eInput = InState.none;

								smm.Next(Cmd.SWLIFTUP_10);
							}
						}
						break;


					case Cmd.SWLIFTUP_10:     //애초에 감지되어 있으면 안 올라감
						{

							if (G.DryRunIsAIn(DI.LDTrayLiftExistSen, true) == false)
							{
								smm.Next(Cmd.END);
								break;
							}

							smm.Next(Cmd.SWLIFTUP_20);
						}
						break;


					case Cmd.SWLIFTUP_20:     //감지되거나, UP 포지션까지 올라감
						{

							int nTrayHeight = SM.SHM.GetRecipeDataToInt(DEVICE.TrayHeight);
							m_nLiftTartgetPos = arrMt[Axis.LiftZ].GetLocalPos() + nTrayHeight * 5;

							if (m_nLiftTartgetPos >= arrMt[Axis.LiftZ].GetIndexPos((int)PosZ.UP))
								m_nLiftTartgetPos = arrMt[Axis.LiftZ].GetIndexPos((int)PosZ.UP);

							smm.Next(Cmd.SWLIFTUP_30);
							break;
							
						}
						break;

					case Cmd.SWLIFTUP_30:
						{
							if (arrMt[Axis.LiftZ].IsRdyPos(m_nLiftTartgetPos) == false)
							{
								arrMt[Axis.LiftZ].MoveABS(m_nLiftTartgetPos, (int)PosZ.SWITCHUP);
								break;
							}
							smm.Next(Cmd.END);
						}
						break;

					case Cmd.END:
						{
							smm.Next(Cmd.START);
						}
						break;
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void SWLiftDown(SMM<Cmd> smm)
		{
			try
			{
				switch (smm.CurStep)
				{
					case Cmd.START:
						{
							if (smm.eInput == InState.run)
							{
								smm.eInput = InState.none;

								smm.Next(Cmd.SWLIFTDOWN_10);
							}
						}
						break;


					case Cmd.SWLIFTDOWN_10:       //애초에 감지 안되어 있으면 안 내려감
						{

							if (G.DryRunIsBIn(DI.LDTrayLiftAreaSen2, false) == false || G.DryRunIsAIn(DI.LDTrayLiftExistSen, true) == false)
							{
								smm.Next(Cmd.END);
								break;
							}

							smm.Next(Cmd.SWLIFTDOWN_20);
						}
						break;


					case Cmd.SWLIFTDOWN_20:       //감지안되거나, Down 포지션까지 내려감
						{
							if (G.DryRunIsBIn(DI.LDTrayLiftAreaSen2, false) == false)
							{
								arrMt[Axis.LiftZ].Stop();

								// 감지한 경우 그 위치에서 조금 더 아래로 이동
								m_nLiftTartgetPos = arrMt[Axis.LiftZ].GetLocalPos() - 90000;

								if (m_nLiftTartgetPos <= arrMt[Axis.LiftZ].GetIndexPos((int)PosZ.DOWN))
									m_nLiftTartgetPos = arrMt[Axis.LiftZ].GetIndexPos((int)PosZ.DOWN);

								smm.Next(Cmd.SWLIFTDOWN_30);
								break;
							}

							if (G.DryRunIsBIn(DI.LDTrayLiftAreaSen2, false) == true && arrMt[Axis.LiftZ].IsRdy((int)PosZ.DOWN) == false)
							{
								arrMt[Axis.LiftZ].MoveIndex((int)PosZ.DOWN, (int)PosZ.SWITCHDOWN);
								break;
							}

							//down 위치 까지 간 경우 끝
							if (arrMt[Axis.LiftZ].IsRdy((int)PosZ.DOWN) == true)
							{
								smm.Next(Cmd.END);
								break;
							}
						}
						break;

					case Cmd.SWLIFTDOWN_30:
						{
							if (arrMt[Axis.LiftZ].IsRdyPos(m_nLiftTartgetPos) == false)
							{
								arrMt[Axis.LiftZ].MoveABS(m_nLiftTartgetPos, (int)PosZ.SWITCHDOWN);
								break;
							}
							smm.Next(Cmd.END);
						}
						break;

					case Cmd.END:
						{
							smm.Next(Cmd.START);
						}
						break;
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
	}
}
