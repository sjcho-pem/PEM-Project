using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace H_MarsSSD
{
	public class SeqUnloadPnP : SeqBase<SeqUnloadPnP.Act, SeqUnloadPnP.Axis, SeqUnloadPnP.Cmd>
	{
		#region [ Enum ]
		public enum Act
		{
			PnPUpDn1,
			PnPUpDn2,
			PnPUpDn3,
			PnPUpDn4,
			Vac1,
			Vac2,
			Vac3,
			Vac4,
			Max,
		}

		public enum Axis
		{
			X,
			Y,
			Z,
			W,
			Max,
		}

		public enum Cmd
		{
			START	= 10,
			END		= 99,
			END_OK	= 97,
			END_NG	= 98,
			
			HOME_10 = 100,
			HOME_20,
			HOME_30,
			HOME_40,
			HOME_50,
			HOME_60,
			HOME_70,
			HOME_80,
			HOME_90,
			HOME_100,
			HOME_110,

			PICK_BUF_10,
			PICK_BUF_20,
			PICK_BUF_30,
			PICK_BUF_40,
			PICK_BUF_50,
			PICK_BUF_60,
			PICK_BUF_70,
			PICK_BUF_80,
			PICK_BUF_90,
			PICK_BUF_100,
			PICK_BUF_110,
			PICK_BUF_120,
			PICK_BUF_130,

            PICK_REJECT_VIBRATE1,
            PICK_REJECT_VIBRATE2,
            PICK_REJECT_10,
			PICK_REJECT_20,
			PICK_REJECT_30,
			PICK_REJECT_40,
			PICK_REJECT_50,
			PICK_REJECT_60,
			PICK_REJECT_70,
			PICK_REJECT_80,
			PICK_REJECT_90,
			PICK_REJECT_100,
			PICK_REJECT_110,
			PICK_REJECT_120,
			PICK_REJECT_130,

			PICK_REJECT_RETRY_10,
			PICK_REJECT_RETRY_20,
			PICK_REJECT_RETRY_30,
			PICK_REJECT_RETRY_40,
			PICK_REJECT_RETRY_50,
			PICK_REJECT_RETRY_60,
			PICK_REJECT_RETRY_70,
			PICK_REJECT_RETRY_80,
			PICK_REJECT_RETRY_90,

			PLACE_10,
			PLACE_20,
			PLACE_30,
			PLACE_40,
			PLACE_50,
			PLACE_60,
			PLACE_70,
			PLACE_UNLOAD_OK_10,
			PLACE_UNLOAD_OK_20,
			PLACE_UNLOAD_NG_10,
			PLACE_UNLOAD_NG_20,
			PLACE_UNLOAD_10,
			PLACE_UNLOAD_20,
			PLACE_UNLOAD_30,
			PLACE_UNLOAD_40,
			PLACE_UNLOAD_50,
			PLACE_UNLOAD_50_CHECKPROD_01,
			PLACE_UNLOAD_50_CHECKPROD_02,
			PLACE_UNLOAD_60,
			PLACE_UNLOAD_70,
			PLACE_UNLOAD_80,
			PLACE_UNLOAD_90,
			PLACE_UNLOAD_100,
			PLACE_UNLOAD_110,
			PLACE_UNLOAD_120,
			PLACE_UNLOAD_130,
			PLACE_REJECT_10,
			PLACE_REJECT_20,
			PLACE_REJECT_30,
			PLACE_REJECT_40,
			PLACE_REJECT_50,
			PLACE_REJECT_51,
			PLACE_REJECT_52,
			PLACE_REJECT_60,
			PLACE_REJECT_60_CHECKPROD_01,
			PLACE_REJECT_60_CHECKPROD_02,
			PLACE_REJECT_70,
			PLACE_REJECT_80,
			PLACE_REJECT_90,
			PLACE_REJECT_100,
			PLACE_REJECT_110,
			PLACE_REJECT_120,

			PnP_10,
			PnP_20,
			PnP_30,
			PnP_40,
			PnP_50,
			PnP_60,
			PnP_70,
			PnP_80,
			PnP_90,
			
			READY_10,
			READY_20,
			READY_30,

			SORT_10,
			SORT_20,
			SORT_30,
			SORT_40,
			SORT_50,
			SORT_60,
			SORT_70,
			SORT_80,
			SORT_90,
			SORT_PICK_10,
			SORT_PICK_20,
			SORT_PICK_30,
			SORT_PICK_40,
			SORT_PICK_50,
			SORT_PICK_60,
			SORT_PICK_70,
			SORT_PICK_80,
			SORT_PICK_90,
			SORT_PLACE_10,
			SORT_PLACE_20,
			SORT_PLACE_30,
			SORT_PLACE_40,
			SORT_PLACE_40_CHECKPROD_01,
			SORT_PLACE_40_CHECKPROD_02,
			SORT_PLACE_50,
			SORT_PLACE_60,
			SORT_PLACE_70,
			SORT_PLACE_80,
			SORT_PLACE_90,
			
			//소팅중 정지 후 재 시작 시
			SORT_RE_PLACE_10,
			SORT_RE_PLACE_20,
			SORT_RE_PLACE_30,
			SORT_RE_PLACE_40,
			SORT_RE_PLACE_50,
			SORT_RE_PLACE_60,
			SORT_RE_PLACE_70,
			SORT_RE_PLACE_70_CHECKPROD_01,
			SORT_RE_PLACE_70_CHECKPROD_02,
			SORT_RE_PLACE_80,
			SORT_RE_PLACE_90,
		}

		public enum PosX
		{
			READY,
			BUFFER,
			TABLE1_PICK_FIRST,
			TABLE2_PICK_FIRST,
			TABLE2_PICK_LAST,

			REJECT_PICK_FIRST,
			REJECT_PICK_LAST,

			TABLE1_PLACE_FIRST,
			TABLE2_PLACE_FIRST,
			TABLE2_PLACE_LAST,

			REJECT_PLACE_FIRST,
			REJECT_PLACE_LAST
		}

		public enum PosY
		{
			READY,
			BUFFER,
			TABLE1_PICK_FIRST,
			TABLE1_PICK_LAST,
			TABLE2_PICK_FIRST,

			REJECT_PICK_FIRST,
			REJECT_PICK_LAST,

			TABLE1_PLACE_FIRST,
			TABLE1_PLACE_LAST,
			TABLE2_PLACE_FIRST,

			REJECT_PLACE_FIRST,
			REJECT_PLACE_LAST
		}

		public enum PosZ
		{
			READY,
			BUFFER_PICK,
            TABLE1_PLACE,
            TABLE2_PLACE,
            REJECT_PLACE,
			TABLE1_PICK,
			TABLE2_PICK,
			REJECT_PICK,
		}

		public enum PosW
		{
			READY,
			BUFFER,
			TABLE,
		}

		private enum	Dir
		{
			Right,
			Left,
			Top,
			Bottom
		}

		private	enum	PlaceMode
		{
			None,
			OK,
			NG,
			Reject,
		}

		#endregion

		#region [Variables]

		private SMM<Cmd> m_smmHome				= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.UNLOADPNP_HOME));
		private SMM<Cmd> m_smmPickBuf			= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.UNLOADPNP_PICKBUFFER));
		private SMM<Cmd> m_smmPickReject		= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.UNLOADPNP_PICKREJECT));
		private SMM<Cmd> m_smmPlace				= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.UNLOADPNP_PLACE));
		private SMM<Cmd> m_smmPnP				= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.UNLOADPNP_PNP));
		private SMM<Cmd> m_smmReady				= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.UNLOADPNP_READY));
		private SMM<Cmd> m_smmSort				= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.UNLOADPNP_SORT));
		private SMM<Cmd> m_smmPickRejectRetry	= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.UNLOADPNP_PICKREJECTRETRY));
		private SMM<Cmd> m_smmSortPlace			= new SMM<Cmd>(Cmd.START, "SORT REPLACE");

		public	Cmd	smmHome			{ get => m_smmHome.CurStep; }
		public	Cmd	smmPickBuf		{ get => m_smmPickBuf.CurStep; }
		public	Cmd	smmPickReject	{ get => m_smmPickReject.CurStep; }
		public	Cmd	smmPlace		{ get => m_smmPlace.CurStep; }
		public	Cmd	smmPnP			{ get => m_smmPnP.CurStep; }
		public	Cmd	smmReady		{ get => m_smmReady.CurStep; }
		public	Cmd	smmSort			{ get => m_smmSort.CurStep; }
		public	Cmd	smmSortPlace	{ get => m_smmSortPlace.CurStep; }

		private int		m_nCntTrayCol		= 0;
		private int		m_nCntTrayRow		= 0;

		private	int		m_nProdRow			= 0;
		private	int		m_nProdCol			= 0;
		private Dir		m_TrayDirection		= Dir.Left;
		
		private	int		m_PickerCnt		= 4;
		private	int		m_PickerMaxCol	= 4;

		//Unload Tray
		private	int		m_nCurTable			= -1; // Tray 시퀀스들이 정해줌, 가능한 트레이가 존재하지 않을 경우 -1
		private	int		m_nCurNGTable		= -1; // 마지막에 NG트레이를넣을곳을 저장하는 변수
        private int     m_nSeqUsingTable    = -1; // 현재 시퀀스에서 사용하는 SeqUsingTray
        private int     m_nSeqUseTable      = -1; // 현재 시퀀스에서 사용했던 트레이
        private	PlaceMode	m_PlaceMode		= PlaceMode.None;
		
		//Picker 관련
		private	int			m_nStartPicker	= 0;
		private	stActPicker	m_ActPicker		= new stActPicker(8);

		//Reject Buffer 관련
		private	int		m_nTargetIdx		= SM.FMT[FlowTray.Reject].GetInfo().MaxCntProdInTray;
		private	bool	m_bRejectPickOnlyOK	= false;
		private	int		m_nCntRejctSend		= 0;
		private	ProdStatus	m_RejectPickMode	= ProdStatus.None;
        private bool m_bRejectAirVibDone = false;

		private	int			m_nSortMapEnd		= 0;
		private	List<int>	m_ltSortPick		= new List<int>();
		private	List<int>	m_ltSortPlace		= new List<int>();
		private	int[]		m_nSortMap			= null;
		private	PosX		m_SortCurX			= PosX.TABLE1_PICK_FIRST;
		private	FlowTray	m_SortFlow			= FlowTray.ULDTable1;
		private	int			m_nAbsPosX			= 0;
		private	int			m_nAbsPosY			= 0;
		private	int			m_SeqSortTray		= 0;
		private	int			m_nPickRetryNo		= -1;
		private	bool		m_bTray1CantPlace	= false;
		private	bool		m_IsMtHome		= true;
		
		private Dir			m_TrayFirstDirection = Dir.Left;
		private Dir			m_TraySecondDirection = Dir.Top;
		#endregion

		#region Function
		public void		Init()
		{
			try
			{
				lstSMM.Add(m_smmHome);
				lstSMM.Add(m_smmPickBuf);
				lstSMM.Add(m_smmPickReject);
				lstSMM.Add(m_smmPlace);
				lstSMM.Add(m_smmPnP);
				lstSMM.Add(m_smmReady);
				lstSMM.Add(m_smmSort);
				lstSMM.Add(m_smmSortPlace);

				
				#region - Motor
				arrMt = new EnumArray<Axis, CMotor>(Axis.Max);
				arrMt[Axis.X] = G.cKernel.Motor(MT.ULDPnPX); 
				arrMt[Axis.Y] = G.cKernel.Motor(MT.ULDPnPY);
				arrMt[Axis.Z] = G.cKernel.Motor(MT.ULDPnPZ);
				arrMt[Axis.W] = G.cKernel.Motor(MT.ULDPnPW);
				#endregion

				#region - Actuator
				arrAct = new EnumArray<Act, Actuator>(Act.Max);
				
				arrAct[Act.Vac1] = new CSingleNoSen();
				arrAct[Act.Vac1].SetSol(DO.ULDPnPVac1);
				arrAct[Act.Vac1].Setting(ALM.ERROR_UNLOAD_PNP_VAC1_VAC, ALM.ERROR_UNLOAD_PNP_VAC1_BLOW, ErrSenPos.Ori, SenPos.NONE, 200, 2000);
				
				arrAct[Act.Vac2] = new CSingleNoSen();
				arrAct[Act.Vac2].SetSol(DO.ULDPnPVac2);
				arrAct[Act.Vac2].Setting(ALM.ERROR_UNLOAD_PNP_VAC2_VAC, ALM.ERROR_UNLOAD_PNP_VAC2_BLOW, ErrSenPos.Ori, SenPos.NONE, 200, 2000);
				
				arrAct[Act.Vac3] = new CSingleNoSen();
				arrAct[Act.Vac3].SetSol(DO.ULDPnPVac3);
				arrAct[Act.Vac3].Setting(ALM.ERROR_UNLOAD_PNP_VAC3_VAC, ALM.ERROR_UNLOAD_PNP_VAC3_BLOW, ErrSenPos.Ori, SenPos.NONE, 200, 2000);
				
				arrAct[Act.Vac4] = new CSingleNoSen();
				arrAct[Act.Vac4].SetSol(DO.ULDPnPVac4);
				arrAct[Act.Vac4].Setting(ALM.ERROR_UNLOAD_PNP_VAC4_VAC, ALM.ERROR_UNLOAD_PNP_VAC4_BLOW, ErrSenPos.Ori, SenPos.NONE, 200, 2000);

				arrAct[Act.PnPUpDn1] = new CDoubleSen();
				arrAct[Act.PnPUpDn1].SetSenser1(new List<DI>(new DI[] { DI.ULDPnPDn1Cyl }));
				arrAct[Act.PnPUpDn1].SetSenser2(new List<DI>(new DI[] { DI.ULDPnPUp1Cyl }));
				arrAct[Act.PnPUpDn1].SetSol(DO.ULDPnPDn1Cyl, DO.ULDPnPUp1Cyl);
				arrAct[Act.PnPUpDn1].Setting(ALM.ERROR_UNLOAD_PNP_UPDN1_CYL_DOWN, ALM.ERROR_UNLOAD_PNP_UPDN1_CYL_UP, ErrSenPos.BWD, SenPos.NONE, 600);

				arrAct[Act.PnPUpDn2] = new CDoubleSen();
				arrAct[Act.PnPUpDn2].SetSenser1(new List<DI>(new DI[] { DI.ULDPnPDn2Cyl }));
				arrAct[Act.PnPUpDn2].SetSenser2(new List<DI>(new DI[] { DI.ULDPnPUp2Cyl }));
				arrAct[Act.PnPUpDn2].SetSol(DO.ULDPnPDn2Cyl, DO.ULDPnPUp2Cyl);
				arrAct[Act.PnPUpDn2].Setting(ALM.ERROR_UNLOAD_PNP_UPDN2_CYL_DOWN, ALM.ERROR_UNLOAD_PNP_UPDN2_CYL_UP, ErrSenPos.BWD, SenPos.NONE, 600);
				
				arrAct[Act.PnPUpDn3] = new CDoubleSen();
				arrAct[Act.PnPUpDn3].SetSenser1(new List<DI>(new DI[] { DI.ULDPnPDn3Cyl }));
				arrAct[Act.PnPUpDn3].SetSenser2(new List<DI>(new DI[] { DI.ULDPnPUp3Cyl }));
				arrAct[Act.PnPUpDn3].SetSol(DO.ULDPnPDn3Cyl, DO.ULDPnPUp3Cyl);
				arrAct[Act.PnPUpDn3].Setting(ALM.ERROR_UNLOAD_PNP_UPDN3_CYL_DOWN, ALM.ERROR_UNLOAD_PNP_UPDN3_CYL_UP, ErrSenPos.BWD, SenPos.NONE, 600);

				arrAct[Act.PnPUpDn4] = new CDoubleSen();
				arrAct[Act.PnPUpDn4].SetSenser1(new List<DI>(new DI[] { DI.ULDPnPDn4Cyl }));
				arrAct[Act.PnPUpDn4].SetSenser2(new List<DI>(new DI[] { DI.ULDPnPUp4Cyl }));
				arrAct[Act.PnPUpDn4].SetSol(DO.ULDPnPDn4Cyl, DO.ULDPnPUp4Cyl);
				arrAct[Act.PnPUpDn4].Setting(ALM.ERROR_UNLOAD_PNP_UPDN4_CYL_DOWN, ALM.ERROR_UNLOAD_PNP_UPDN4_CYL_UP, ErrSenPos.BWD, SenPos.NONE, 600);
				
				#endregion

				m_nCntTrayCol = SM.SHM.GetRecipeDataToInt(DEVICE.TrayColCnt) / 1000;
				m_nCntTrayRow = SM.SHM.GetRecipeDataToInt(DEVICE.TrayRowCnt) / 1000;
				
				m_PickerCnt		= SM.FMP[FlowPart.Empty].GetMaxProdCount();
				m_PickerMaxCol	= SM.FMP[FlowPart.Empty].GetMaxProdCount();

				base.Initialize();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}



		public bool HomeStart(bool IsMtHome = true) { m_IsMtHome = IsMtHome; return SetSeqRun(ref m_smmHome); }

		public bool PickBufStart() { return SetSeqRun(ref m_smmPickBuf); }

		public bool PickRejectStart(bool bRejectPickOnlyOK = false) { m_bRejectPickOnlyOK = bRejectPickOnlyOK; return SetSeqRun(ref m_smmPickReject); }

		public bool PlaceStart() { return SetSeqRun(ref m_smmPlace); }

		public bool PnPStart() { return SetSeqRun(ref m_smmPnP); }

		public bool ReadyStart() { return SetSeqRun(ref m_smmReady); }

		public bool SortStart(FlowTray sortFlow)//CSJ
		{
			if (sortFlow == FlowTray.ULDTable1)
			{
				m_SortCurX = PosX.TABLE1_PICK_FIRST;
				m_SortFlow = sortFlow;
				m_SeqSortTray = 0;
			}
			else if (sortFlow == FlowTray.ULDTable2)
			{
				m_SortCurX = PosX.TABLE2_PICK_FIRST;
				m_SortFlow = sortFlow;
				m_SeqSortTray = 1;
			}
			else if (sortFlow == FlowTray.Reject)
			{
				m_SortCurX = PosX.REJECT_PICK_FIRST;
				m_SortFlow = sortFlow;
				m_SeqSortTray = 2;
			}
			else return false;

			return SetSeqRun(ref m_smmSort);
		}

		public bool SortPlaceStart(FlowTray sortFlow)//CSJ
		{
			if (sortFlow == FlowTray.ULDTable1)
			{
				m_SortCurX = PosX.TABLE1_PLACE_FIRST;
				m_SortFlow = sortFlow;
				m_SeqSortTray = 0;
			}
			else if (sortFlow == FlowTray.ULDTable2)
			{
				m_SortCurX = PosX.TABLE2_PLACE_FIRST;
				m_SortFlow = sortFlow;
				m_SeqSortTray = 1;
			}
			else if (sortFlow == FlowTray.Reject)
			{
				m_SortCurX = PosX.REJECT_PLACE_FIRST;
				m_SortFlow = sortFlow;
				m_SeqSortTray = 2;
			}
			else return false;

			return SetSeqRun(ref m_smmSortPlace);
		}

		public bool PickRejectRetryStart(int nPickNo)
		{
			m_nPickRetryNo = nPickNo;

			m_nCntTrayCol = SM.SHM.GetRecipeDataToInt(DEVICE.TrayColCnt) / 1000;
			m_nCntTrayRow = SM.SHM.GetRecipeDataToInt(DEVICE.TrayRowCnt) / 1000;

			m_smmPickRejectRetry.eInput = InState.run;

			return true;
		}

		public void PlaceStop() { m_smmPlace.eInput = InState.none; m_smmPlace.Next(Cmd.START); }

		public bool IsHomeReady() { return m_smmHome.IsReady; }

		public bool IsPickBufReady() { return m_smmPickBuf.IsReady; }

		public bool IsPickRejectReady() { return m_smmPickReject.IsReady; }

		public bool IsPlaceReady() { return m_smmPlace.IsReady; }

		public bool IsPnPReady() { return m_smmPnP.IsReady; }

		public bool IsPickRejectRetryReady() { return m_smmPickRejectRetry.IsReady && SM.SHM.IsBitData(BIT.Error) == false; }

		public bool IsReadyPos()
		{
			if (CheckCylUpDown(SenPos.PICKER_UP) == false) return false;
			if (arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false) return false;
			if (arrMt[Axis.X].IsRdy((int)PosX.BUFFER) == false) return false;
			if (arrMt[Axis.Y].IsRdy((int)PosY.BUFFER) == false) return false;

			return true;
		}

		public bool IsSafetyPos()
		{
			if (CheckCylUpDown(SenPos.PICKER_UP) == false) return false;
			if (arrMt[Axis.Z].IsRdyPos(arrMt[Axis.Z].GetIndexPos((int)PosZ.READY)) == false) return false;

			return true;
		}



		private void UpdateCurTable()
		{
			try
			{
				int i = 0;

				if (m_nCurTable == -1)
				{
					if (SM.FMT.IsFlow(FlowTray.ULDTable1) == true &&
							 SM.FMT.IsFlow(FlowTray.ULDTable2) == true &&
							 G.seqUnloadTrayTable1.IsWaitEnable() == true &&
							 G.seqUnloadTrayTable2.IsWaitEnable() == true &&
							 CheckTrayPlaceEnd(FlowTray.ULDTable1) == false &&
							 CheckTrayPlaceEnd(FlowTray.ULDTable2) == false &&
							 SM.FMT[FlowTray.ULDTable1].IsResultExist(ProdStatus.NG) == false &&
							 SM.FMT[FlowTray.ULDTable2].IsResultExist(ProdStatus.NG) == false &&
							 SM.SHM.IsUsePart(UsePartName.ULDTable1, UsePartMode.Unload) == true &&
							 SM.SHM.IsUsePart(UsePartName.ULDTable2, UsePartMode.Unload) == true)
					{
						if (SM.FMT[FlowTray.ULDTable1].GetInfo().TransTime <= SM.FMT[FlowTray.ULDTable2].GetInfo().TransTime &&
								 SM.FMT[FlowTray.ULDTable1].IsFull() == false &&
								 m_bTray1CantPlace == false &&
								 IsCanPlaceTray(0, PlaceMode.OK) == true)
						{
							m_nCurTable = 0;
						}
						else if (SM.FMT[FlowTray.ULDTable2].IsFull() == false)
						{
							m_nCurTable = 1;
						}
					}
					else if (SM.FMT.IsFlow(FlowTray.ULDTable1) == true &&
							 CheckTrayPlaceEnd(FlowTray.ULDTable1) == false &&
							 SM.FMT[FlowTray.ULDTable1].IsResultExist(ProdStatus.NG) == false &&
							 G.seqUnloadTrayTable1.IsWaitEnable() == true &&
							 SM.SHM.IsUsePart(UsePartName.ULDTable1, UsePartMode.Unload) == true)
					{
						if (SM.FMT[FlowTray.ULDTable1].IsFull() == false &&
							IsCanPlaceTray(0, PlaceMode.OK) == true)
						{
							m_nCurTable = 0;
						}
					}
					else if (SM.FMT.IsFlow(FlowTray.ULDTable2) == true &&
							 CheckTrayPlaceEnd(FlowTray.ULDTable2) == false &&
							 SM.FMT[FlowTray.ULDTable2].IsResultExist(ProdStatus.NG) == false &&
							 G.seqUnloadTrayTable2.IsWaitEnable() == true &&
							 SM.SHM.IsUsePart(UsePartName.ULDTable2, UsePartMode.Unload) == true)
					{
						if (SM.FMT[FlowTray.ULDTable2].IsFull() == false)
						{
							m_nCurTable = 1;
						}
					}
				}
				else if (m_nCurTable == 0)
				{
					if (SM.FMT.IsFlow(FlowTray.ULDTable1) == false ||
						CheckTrayPlaceEnd(FlowTray.ULDTable1) == true ||
						SM.FMT[FlowTray.ULDTable1].IsFull() == true ||
						SM.FMT[FlowTray.ULDTable1].IsResultExist(ProdStatus.NG) == true ||
						SM.SHM.IsUsePart(UsePartName.ULDTable1, UsePartMode.Unload) == false ||
						IsCanPlaceTray(0, PlaceMode.OK) == false)
					{
						m_nCurTable = -1;
					}
				}
				else if (m_nCurTable == 1)
				{
					if (SM.FMT[FlowTray.ULDTable1].GetInfo().TransTime <= SM.FMT[FlowTray.ULDTable2].GetInfo().TransTime &&
							 CheckTrayPlaceEnd(FlowTray.ULDTable1) == false &&
							 SM.FMT[FlowTray.ULDTable1].IsFull() == false &&
							 m_bTray1CantPlace == false &&
							 IsCanPlaceTray(0, PlaceMode.OK) == true)
					{
						m_nCurTable = 0;
					}
					else if (SM.FMT.IsFlow(FlowTray.ULDTable2) == false ||
							 CheckTrayPlaceEnd(FlowTray.ULDTable2) == true ||
							 SM.FMT[FlowTray.ULDTable2].IsFull() == true ||
							 SM.FMT[FlowTray.ULDTable2].IsEmpty() == true ||
							 SM.FMT[FlowTray.ULDTable2].IsResultExist(ProdStatus.NG) == true ||
							 SM.SHM.IsUsePart(UsePartName.ULDTable2, UsePartMode.Unload) == false)
					{
						m_nCurTable = -1;
					}
				}

				if (m_nCurNGTable == -1)
				{
					if (SM.FMT.IsFlow(FlowTray.ULDTable1) == true &&
							 SM.FMT.IsFlow(FlowTray.ULDTable2) == true &&
							 G.seqUnloadTrayTable1.IsWaitEnable() == true &&
							 G.seqUnloadTrayTable2.IsWaitEnable() == true &&
							 CheckTrayPlaceEnd(FlowTray.ULDTable1) == false &&
							 CheckTrayPlaceEnd(FlowTray.ULDTable2) == false &&
							 SM.FMT[FlowTray.ULDTable1].IsResultExist(ProdStatus.OK) == false &&
							 SM.FMT[FlowTray.ULDTable2].IsResultExist(ProdStatus.OK) == false &&
							 SM.SHM.IsUsePart(UsePartName.ULDTable1, UsePartMode.Unload) == true &&
							 SM.SHM.IsUsePart(UsePartName.ULDTable2, UsePartMode.Unload) == true)
					{
						if (SM.FMT[FlowTray.ULDTable1].GetInfo().TransTime <= SM.FMT[FlowTray.ULDTable2].GetInfo().TransTime &&
								 SM.FMT[FlowTray.ULDTable1].IsFull() == false &&
								 IsCanPlaceTray(0, PlaceMode.NG) == true)
						{
							m_nCurNGTable = 0;
						}
						else if (SM.FMT[FlowTray.ULDTable2].IsFull() == false)
						{
							m_nCurNGTable = 1;
						}
					}
					else if (SM.FMT.IsFlow(FlowTray.ULDTable1) == true &&
							 CheckTrayPlaceEnd(FlowTray.ULDTable1) == false &&
							 SM.FMT[FlowTray.ULDTable1].IsResultExist(ProdStatus.OK) == false &&
							 G.seqUnloadTrayTable1.IsWaitEnable() == true &&
							 SM.SHM.IsUsePart(UsePartName.ULDTable1, UsePartMode.Unload) == true)
					{
						if (SM.FMT[FlowTray.ULDTable1].IsFull() == false &&
							IsCanPlaceTray(0, PlaceMode.NG) == true)
						{
							m_nCurNGTable = 0;
						}
					}
					else if (SM.FMT.IsFlow(FlowTray.ULDTable2) == true &&
							 CheckTrayPlaceEnd(FlowTray.ULDTable2) == false &&
							 SM.FMT[FlowTray.ULDTable2].IsResultExist(ProdStatus.OK) == false &&
							 G.seqUnloadTrayTable2.IsWaitEnable() == true &&
							 SM.SHM.IsUsePart(UsePartName.ULDTable2, UsePartMode.Unload) == true)
					{
						if (SM.FMT[FlowTray.ULDTable2].IsFull() == false)
						{
							m_nCurNGTable = 1;
						}
					}
				}
				else if (m_nCurNGTable == 0)
				{
					if (SM.FMT.IsFlow(FlowTray.ULDTable1) == false ||
							 SM.FMT[FlowTray.ULDTable1].IsResultExist(ProdStatus.OK) == true ||
							 CheckTrayPlaceEnd(FlowTray.ULDTable1) == true ||
							 IsCanPlaceTray(0, PlaceMode.NG) == false)
					{
						m_nCurNGTable = -1;
					}
				}
				else if (m_nCurNGTable == 1)
				{
					if (SM.FMT[FlowTray.ULDTable1].GetInfo().TransTime <= SM.FMT[FlowTray.ULDTable2].GetInfo().TransTime &&
							 CheckTrayPlaceEnd(FlowTray.ULDTable1) == false &&
							 SM.FMT[FlowTray.ULDTable1].IsFull() == false &&
							 IsCanPlaceTray(0, PlaceMode.NG) == true)
					{
						m_nCurNGTable = 0;
					}
					else if (SM.FMT.IsFlow(FlowTray.ULDTable2) == false ||
							 SM.FMT[FlowTray.ULDTable2].IsResultExist(ProdStatus.OK) == true ||
							 CheckTrayPlaceEnd(FlowTray.ULDTable2) == true)
					{
						m_nCurNGTable = -1;
					}
				}


				if (SM.FMP[FlowPart.ULDPnP].IsResultExist(ProdStatus.NG) >= 0 &&
						 SM.FMP[FlowPart.ULDPnP].IsNoVerifyExist() == true)
				{
					m_PlaceMode = PlaceMode.Reject;
				}
				else if (SM.FMP[FlowPart.ULDPnP].IsResultExist(ProdStatus.OK) >= 0)
				{
					m_PlaceMode = PlaceMode.OK;
					if (m_nCurTable >= 0) m_nSeqUsingTable = m_nCurTable;
					// 					else if (NDm.Read(eNDM.AutoRunMode) == (int)teAutoRunMode.Bypass)
					// 					{
					// 						if (m_nCurNGTable >= 0)			m_nSeqUsingTable	= m_nCurNGTable;
					// 					}
				}
				else if (SM.FMP[FlowPart.ULDPnP].IsResultExist(ProdStatus.NG) >= 0)
				{
					m_PlaceMode = PlaceMode.NG;
					if (m_nCurNGTable >= 0) m_nSeqUsingTable = m_nCurNGTable;
					// 					else if (NDm.Read(eNDM.AutoRunMode) == (int)teAutoRunMode.Bypass)
					// 					{
					// 						if (m_nCurTable >= 0)			m_nSeqUsingTable	= m_nCurTable;
					// 					}
				}
				else
				{
					m_PlaceMode = PlaceMode.None;
					m_nSeqUsingTable = -1;
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private bool GetPnPPosTable(int nTableNo, int nPickerNo, int nCol, int nRow, ref int nXPos, ref int nYPos, bool bMode)
		{
			try
			{
				int ndist_x = 0;
				int ndist_y = 0;
				int npicker_dif_x = 0;
				int ncnt_row = SM.SHM.GetRecipeDataToInt(DEVICE.TrayRowCnt) / 1000;
				int ncnt_col = SM.SHM.GetRecipeDataToInt(DEVICE.TrayColCnt) / 1000;

				//Check : 모터 구동 방향에 따라 수정 필요
				//int nTemp  =	arrMt[teAxis.X].GetIndexPos((int)tePosX.TABLE2_1) - arrMt[teAxis.X].GetIndexPos((int)tePosX.TABLE2_1+nPickerNo);
				npicker_dif_x = GetPickerGap(nPickerNo);
				if (bMode == true) // Pick Mode
				{
					if (nTableNo == 0 || nTableNo == 1)
					{
						ndist_x = (arrMt[Axis.X].GetIndexPos((int)PosX.TABLE2_PICK_LAST) - arrMt[Axis.X].GetIndexPos((int)PosX.TABLE2_PICK_FIRST)) / (ncnt_col - 1);
						ndist_y = (arrMt[Axis.Y].GetIndexPos((int)PosY.TABLE1_PICK_LAST) - arrMt[Axis.Y].GetIndexPos((int)PosY.TABLE1_PICK_FIRST)) / (ncnt_row - 1); ;
					}
					else
					{
						ndist_x = (arrMt[Axis.X].GetIndexPos((int)PosX.REJECT_PICK_LAST) - arrMt[Axis.X].GetIndexPos((int)PosX.REJECT_PICK_FIRST)) / (ncnt_col - 1);
						ndist_y = (arrMt[Axis.Y].GetIndexPos((int)PosY.REJECT_PICK_LAST) - arrMt[Axis.Y].GetIndexPos((int)PosY.REJECT_PICK_FIRST)) / (ncnt_row - 1);
					}

					if (nTableNo == 0)
					{
						nXPos = arrMt[Axis.X].GetIndexPos((int)PosX.TABLE1_PICK_FIRST);
						nYPos = arrMt[Axis.Y].GetIndexPos((int)PosY.TABLE1_PICK_FIRST);
					}
					else if (nTableNo == 1)
					{
						nXPos = arrMt[Axis.X].GetIndexPos((int)PosX.TABLE2_PICK_FIRST);
						nYPos = arrMt[Axis.Y].GetIndexPos((int)PosY.TABLE2_PICK_FIRST);
					}
					else if (nTableNo == 2)
					{
						nXPos = arrMt[Axis.X].GetIndexPos((int)PosX.REJECT_PICK_FIRST);
						nYPos = arrMt[Axis.Y].GetIndexPos((int)PosY.REJECT_PICK_FIRST);
					}
					else
					{
						return false;
					}
				}
				else //Place Mode
				{
					if (nTableNo == 0 || nTableNo == 1)
					{
						ndist_x = (arrMt[Axis.X].GetIndexPos((int)PosX.TABLE2_PLACE_LAST) - arrMt[Axis.X].GetIndexPos((int)PosX.TABLE2_PLACE_FIRST)) / (ncnt_col - 1);
						ndist_y = (arrMt[Axis.Y].GetIndexPos((int)PosY.TABLE1_PLACE_LAST) - arrMt[Axis.Y].GetIndexPos((int)PosY.TABLE1_PLACE_FIRST)) / (ncnt_row - 1); ;
					}
					else
					{
						ndist_x = (arrMt[Axis.X].GetIndexPos((int)PosX.REJECT_PLACE_LAST) - arrMt[Axis.X].GetIndexPos((int)PosX.REJECT_PLACE_FIRST)) / (ncnt_col - 1);
						ndist_y = (arrMt[Axis.Y].GetIndexPos((int)PosY.REJECT_PLACE_LAST) - arrMt[Axis.Y].GetIndexPos((int)PosY.REJECT_PLACE_FIRST)) / (ncnt_row - 1);
					}

					if (nTableNo == 0)
					{
						nXPos = arrMt[Axis.X].GetIndexPos((int)PosX.TABLE1_PLACE_FIRST);
						nYPos = arrMt[Axis.Y].GetIndexPos((int)PosY.TABLE1_PLACE_FIRST);
					}
					else if (nTableNo == 1)
					{
						nXPos = arrMt[Axis.X].GetIndexPos((int)PosX.TABLE2_PLACE_FIRST);
						nYPos = arrMt[Axis.Y].GetIndexPos((int)PosY.TABLE2_PLACE_FIRST);
					}
					else if (nTableNo == 2)
					{
						nXPos = arrMt[Axis.X].GetIndexPos((int)PosX.REJECT_PLACE_FIRST);
						nYPos = arrMt[Axis.Y].GetIndexPos((int)PosY.REJECT_PLACE_FIRST);
					}
					else
					{
						return false;
					}
				}

				nXPos += (nCol * ndist_x) + npicker_dif_x;
				nYPos += (nRow * ndist_y);

				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		private bool GetPnPPosReject(int nPickerNo, int nCol, int nRow, ref int nXPos, ref int nYPos, bool bMode)//CSJ
		{
			try
			{
				int ndist_x = 0;
				int npicker_dif_x = 0;
				int ncnt_col = SM.FMT[FlowTray.Reject].GetInfo().ColCnt;

				//Check : 모터 구동 방향에 따라 수정 필요
				npicker_dif_x = GetPickerGap(nPickerNo);
				if(bMode)//Pick Mode
				{
					ndist_x = (arrMt[Axis.X].GetIndexPos((int)PosX.REJECT_PICK_LAST) - arrMt[Axis.X].GetIndexPos((int)PosX.REJECT_PICK_FIRST)) / (ncnt_col - 1);

					nXPos = arrMt[Axis.X].GetIndexPos((int)PosX.REJECT_PICK_FIRST);
					nYPos = arrMt[Axis.Y].GetIndexPos((int)PosY.REJECT_PICK_FIRST + nRow);
				}
				else //Place Mode
				{
					ndist_x = (arrMt[Axis.X].GetIndexPos((int)PosX.REJECT_PLACE_LAST) - arrMt[Axis.X].GetIndexPos((int)PosX.REJECT_PLACE_FIRST)) / (ncnt_col - 1);

					nXPos = arrMt[Axis.X].GetIndexPos((int)PosX.REJECT_PLACE_FIRST);
					nYPos = arrMt[Axis.Y].GetIndexPos((int)PosY.REJECT_PLACE_FIRST + nRow);
				}

				nXPos += (nCol * ndist_x);
				nXPos += npicker_dif_x;
				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		private FlowTray GetFlow(int nTableNo)
		{
			try
			{
				if (nTableNo == 0)			return FlowTray.ULDTable1;
				else if (nTableNo == 1)		return FlowTray.ULDTable2;
				else						return FlowTray.Empty;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return FlowTray.Empty;
			}
		}

		private bool SearchProdTray(int nTableNo, Dir FromSearchDir, ref int nCol, ref int nRow, int nStartCol = 0, int nStartRow = 0)
		{
			try
			{
				FlowTray flow = FlowTray.ULDTable1;
				int nindex = 0;

				if (nTableNo == 0) flow = FlowTray.ULDTable1;
				else if (nTableNo == 1) flow = FlowTray.ULDTable2;
				else return false;

				if (SM.FMT.IsFlow(flow) == false) return false;

				if (FromSearchDir == Dir.Left)
				{
					for (int nrow = nStartRow; nrow < m_nCntTrayRow; nrow++)
					{
						for (int ncol = nStartCol; ncol < m_nCntTrayCol; ncol++)
						{
							nindex = nrow * m_nCntTrayCol + ncol;

							if (SM.FMT[flow].GetInfo().ProdInfo[nindex].Exist == true)
							{
								nCol = ncol;
								nRow = nrow;
								return true;
							}
						}
					}
				}
				else
				{
					for (int nrow = nStartRow; nrow < m_nCntTrayRow; nrow++)
					{
						for (int ncol = m_nCntTrayCol - 1; ncol >= nStartCol; ncol--)
						{
							nindex = nrow * m_nCntTrayCol + ncol;

							if (SM.FMT[flow].GetInfo().ProdInfo[nindex].Exist == true)
							{
								nCol = ncol;
								nRow = nrow;
								return true;
							}
						}
					}
				}

				return false;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		private bool SearchProdTray(int nTableNo, Dir FromSearchDir, int nStartCol = 0, int nStartRow = 0)
		{
			try
			{
				int ncol = 0;
				int nrow = 0;
				return SearchProdTray(nTableNo, FromSearchDir, ref ncol, ref nrow, nStartCol, nStartRow);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		private bool SearchProdReject(Dir FromSearchDir, ProdStatus ProdStatus, ref int nCol, ref int nRow, int nStartCol = 0, int nStartRow = 0)
		{
			try
			{
				FlowTray flow = FlowTray.Reject;
				int nindex = 0;
				int ncnt_col = SM.FMT[flow].GetInfo().ColCnt;

				if (SM.FMT.IsFlow(flow) == false) return false;

				if (FromSearchDir == Dir.Left)
				{
					for (int nrow = nStartRow; nrow < SM.FMT[FlowTray.Reject].GetInfo().RowCnt; nrow++)
					{
						for (int ncol = nStartCol; ncol < SM.FMT[FlowTray.Reject].GetInfo().ColCnt; ncol++)
						{
							nindex = nrow * ncnt_col + ncol;

							if (SM.FMT[flow].GetInfo().ProdInfo[nindex].Exist == true &&
								SM.FMT[flow].GetProdStatus(nindex) == ProdStatus &&
								SM.FMT[flow].CheckVerifyComplete(nindex) == true)
							{
								nCol = ncol;
								nRow = nrow;
								return true;
							}
						}
					}
				}
				else
				{
					for (int nrow = nStartRow; nrow < SM.FMT[FlowTray.Reject].GetInfo().RowCnt; nrow++)
					{
						for (int ncol = SM.FMT[FlowTray.Reject].GetInfo().ColCnt - 1; ncol >= nStartCol; ncol--)
						{
							nindex = nrow * ncnt_col + ncol;

							if (SM.FMT[flow].GetInfo().ProdInfo[nindex].Exist == true &&
								SM.FMT[flow].GetProdStatus(nindex) == ProdStatus &&
								SM.FMT[flow].CheckVerifyComplete(nindex) == true)
							{
								nCol = ncol;
								nRow = nrow;
								return true;
							}
						}
					}
				}

				return false;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		private bool SearchProdReject(Dir FromSearchDir, ProdStatus ProdStatus, int nStartCol = 0, int nStartRow = 0)
		{
			try
			{
				int ncol = 0;
				int nrow = 0;
				return SearchProdReject(FromSearchDir, ProdStatus, ref ncol, ref nrow, nStartCol, nStartRow);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		private bool SearchEmptyTray(int nTableNo, Dir FromSearchDir, ref int nCol, ref int nRow, int nStartCol = 0, int nStartRow = 0)
		{
			try
			{
				FlowTray flow = FlowTray.ULDTable1;
			
				if (nTableNo == 0) flow = FlowTray.ULDTable1;
				else if (nTableNo == 1) flow = FlowTray.ULDTable2;
				else return false;

				if (SM.FMT.IsFlow(flow) == false)
				{
					if (nTableNo == 0 || nTableNo == 1) SM.SHM.MsgShow(SeqMsg.UnloadNone);
					return false;
				}

				SM.SHM.MsgClose(SeqMsg.UnloadNone);
				for (int nrow = nStartRow; nrow < m_nCntTrayRow; nrow++)
				{
					for (int ncol = nStartCol; ncol < m_nCntTrayCol; ncol++)
					{
						if (SM.FMT[flow].GetInfo().ProdInfo[nrow * m_nCntTrayCol + ncol].Exist == false)
						{
							nCol = ncol;
							nRow = nrow;
							return true;
						}
					}
				}

				return false;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		private bool SearchEmptyReject(Dir FromSearchDir, ref int nCol, ref int nRow, int nStartCol = 0, int nStartRow = 0)
		{
			try
			{
				int nindex = 0;
				int ncnt_col = SM.FMT[FlowTray.Reject].GetInfo().ColCnt;

				if (SM.FMT.IsFlow(FlowTray.Reject) == false)
				{
					SM.SHM.MsgShow(SeqMsg.RejectNone);
					return false;
				}
				else if (SM.SHM.IsMsgPopUp(SeqMsg.RejectNone) == true)
				{
					SM.SHM.MsgClose(SeqMsg.RejectNone);
				}

				if (FromSearchDir == Dir.Left)
				{
					for (int nrow = nStartRow; nrow < SM.FMT[FlowTray.Reject].GetInfo().RowCnt; nrow++)
					{
						for (int ncol = nStartCol; ncol < SM.FMT[FlowTray.Reject].GetInfo().ColCnt; ncol++)
						{
							nindex = nrow * ncnt_col + ncol;

							if (SM.FMT[FlowTray.Reject].GetInfo().ProdInfo[nindex].Exist == false)
							{
								nCol = ncol;
								nRow = nrow;
								return true;
							}
						}
					}
				}
				else
				{
					for (int nrow = nStartRow; nrow < SM.FMT[FlowTray.Reject].GetInfo().RowCnt; nrow++)
					{
						for (int ncol = SM.FMT[FlowTray.Reject].GetInfo().ColCnt - 1; ncol >= nStartCol; ncol--)
						{
							nindex = nrow * ncnt_col + ncol;

							if (SM.FMT[FlowTray.Reject].GetInfo().ProdInfo[nindex].Exist == false)
							{
								nCol = ncol;
								nRow = nrow;
								return true;
							}
						}
					}
				}

				return false;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		private bool CheckGapValid(int nCol, int nPickerNo)
		{
			try
			{
				if (nPickerNo == 0) return true;

				double dsum_gap = 0;

				
				//정상 동작 확인 필요
				double nDistPicker = TrayPnPRecipe.PnPInfo.UnloadPicker.ColsGap[nPickerNo];
				double nDistTray = TrayPnPRecipe.PnPInfo.Tray.ColsGap[nCol];
				double nstep = nDistPicker / nDistTray;


				for (int i = 0; i < nstep; i++)
				{
					if ((nCol - i) < 0) return false;
					dsum_gap += TrayPnPRecipe.PnPInfo.Tray.ColsGap[nCol - i];
				}

				if (dsum_gap != TrayPnPRecipe.PnPInfo.UnloadPicker.ColsGap[nPickerNo]) return false;

				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		private int GetPickerGap(int nPickerNo)
		{
			try
			{
				double dsum_gap = 0;
				for (int i = 0; i <= nPickerNo; i++)
				{
					if (i > SM.FMP[FlowPart.ULDPnP].GetMaxProdCount()) return -1;
					dsum_gap += (TrayPnPRecipe.PnPInfo.UnloadPicker.ColsGap[i] * 1000);
				}
				return (int)dsum_gap;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -1;
			}
		}

		private bool CheckTrayPlaceEnd(FlowTray flow)
		{
			try
			{
				if (SM.FMT[flow].ProcStatus == ProcStatus.TrayFillDone ||
					SM.FMT[flow].ProcStatus == ProcStatus.SortDone)
				{
					return true;
				}

				return false;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

        public void RunCylUpDown(SenPos eSenPos, int nProd = -1)
        {
            try
            {
                if (SM.SHM.GetUseSkipData(USESKIP.DRYRUN) == true && eSenPos == SenPos.PICKER_DOWN) return;

                if (nProd == -1)
                {
                    for (int i = 0; i < SM.FMP[FlowPart.ULDPnP].GetMaxProdCount(); i++)
                    {
                        if (eSenPos == SenPos.PICKER_DOWN &&
                            SM.SHM.IsUsePart(UsePartName.Picker1 + i, UsePartMode.Enable) == false)
                        {
                            continue;
                        }

                        arrAct[Act.PnPUpDn1 + i].SetPos(eSenPos);
                    }
                }
                else
                {
                    arrAct[Act.PnPUpDn1 + nProd].SetPos(eSenPos);
                }
            }
            catch (Exception ex)
            {
                G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
            }
        }


        private void RunCylUpDown(SenPos eSenPos, stActPicker actPicker)
		{
			try
			{
				if (SM.SHM.GetUseSkipData(USESKIP.DRYRUN) == true && eSenPos == SenPos.PICKER_DOWN) return;

				int[] nno = actPicker.GetPocketNoAll();

				for (int i = 0; i < nno.Length; i++)
				{
					if (nno[i] < 0) continue;

					RunCylUpDown(eSenPos, i);
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

        public bool CheckCylUpDown(SenPos eSenPos, int nProd = -1)
        {
            try
            {
                if (SM.SHM.GetUseSkipData(USESKIP.DRYRUN) == true && eSenPos == SenPos.PICKER_DOWN) return true;

                bool res = true;

                if (nProd == -1)
                {
                    for (int i = 0; i < SM.FMP[FlowPart.ULDPnP].GetMaxProdCount(); i++)
                    {
                        if (eSenPos == SenPos.PICKER_DOWN &&  //Up은 체크할거임
                            SM.SHM.IsUsePart(UsePartName.Picker1 + i, UsePartMode.Enable) == false)
                        {
                            continue;
                        }

                        if (arrAct[Act.PnPUpDn1 + i].IsPos(eSenPos) == false)
                        {
                            res = false;
                        }
                    }
                }
                else
                {
                    if (eSenPos == SenPos.PICKER_DOWN &&
                        SM.SHM.IsUsePart(UsePartName.Picker1 + nProd, UsePartMode.Enable) == false)
                    {
                        return true;
                    }
                    else if (arrAct[Act.PnPUpDn1 + nProd].IsPos(eSenPos) == false)
                    {
                        res = false;
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
                return false;
            }
        }


        private bool CheckCylUpDown(SenPos eSenPos, stActPicker actPicker)
		{
			try
			{
				if (SM.SHM.GetUseSkipData(USESKIP.DRYRUN) == true && eSenPos == SenPos.PICKER_DOWN) return true;

				int[] nno = actPicker.GetPocketNoAll();

				for (int i = 0; i < nno.Length; i++)
				{
					if (nno[i] < 0) continue;

					if (CheckCylUpDown(eSenPos, i) == false) return false;
				}

				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		private bool IsCanPlaceTray(int nTray, PlaceMode PlaceMode)
		{
			try
			{
				int nstart_picker = -1;

				if (PlaceMode == PlaceMode.OK)
				{
					for (int i = 0; i < SM.FMP[FlowPart.ULDPnP].GetMaxProdCount(); i++)
					{
						if (SM.FMP[FlowPart.ULDPnP].GetProdStatus(i) == ProdStatus.OK)
						{
							nstart_picker = i;
							break;
						}
					}
				}
				else if (PlaceMode == PlaceMode.NG)
				{
					for (int i = 0; i < SM.FMP[FlowPart.ULDPnP].GetMaxProdCount(); i++)
					{
						if (SM.FMP[FlowPart.ULDPnP].GetProdStatus(i) == ProdStatus.NG &&
							SM.FMP[FlowPart.ULDPnP].IsNoVerifyExist(i) == false)
						{
							nstart_picker = i;
							break;
						}
					}
				}
				else
				{
					return false;
				}

				if (nstart_picker == -1)
				{
					return false;
				}

				// 내려 둘 트레이가 있는지 체크
				if (nTray == -1) return false;

				int nstart_col = 0;
				int nprod_col = 0;
				int nprod_row = 0;
				
				if (SearchEmptyTray(nTray, m_TrayDirection, ref nprod_col, ref nprod_row, nstart_col) == false)
				{
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

		public void RunEjector(SenPos eSenPos, int nProd = -1)
		{
			try
			{
				if (nProd == -1)
				{
					for (int i = 0; i < SM.FMP[FlowPart.ULDPnP].GetMaxProdCount(); i++)
					{
						arrAct[Act.Vac1 + i].SetPos(eSenPos);
					}
				}
				else
				{
					arrAct[Act.Vac1 + nProd].SetPos(eSenPos);
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public bool CheckEjector(SenPos eSenPos, int nProd = -1, bool bAlarm = false)
		{
			try
			{
				
				if (nProd == -1)
				{
					for (int i = 0; i < SM.FMP[FlowPart.ULDPnP].GetMaxProdCount(); i++)
					{
						if (arrAct[Act.Vac1 + i].GetPos() != eSenPos ||
						G.DryRunIsAIn(DI.ULDPnPVac1 + i, eSenPos == SenPos.VAC) != (eSenPos == SenPos.VAC))
                        {
                            if (bAlarm == true)
                            {
                                SM.FMP[FlowPart.ULDPnP].SetPickupStatus(ProdPickStatus.Mismatch, i);
                                AddAlarm(ALM.ERROR_UNLOAD_PNP_VAC1_VAC + i);
                            }
                            else return false;
                        }
                    }
				}
				else
				{
					if (arrAct[Act.Vac1 + nProd].GetPos() != eSenPos  ||
						G.DryRunIsAIn(DI.ULDPnPVac1 + nProd, eSenPos == SenPos.VAC) != (eSenPos == SenPos.VAC))
                    {
                        if (bAlarm == true)
                        {
                            SM.FMP[FlowPart.ULDPnP].SetPickupStatus(ProdPickStatus.Mismatch, nProd);
                            AddAlarm(ALM.ERROR_UNLOAD_PNP_VAC1_VAC + nProd);
                        }
                        else return false;
                    }
                }


				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		private void RunPickerEjector(SenPos eSenPos, stActPicker actPicker)
		{
			try
			{
				int[] nno = actPicker.GetPocketNoAll();

				for (int i = 0; i < nno.Length; i++)
				{
					if (nno[i] < 0) continue;

					RunEjector(eSenPos, i);
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private bool CheckPickerEjector(SenPos eSenPos, stActPicker actPicker, bool bAlarm = false)
		{
			try
			{
				int[] nno = actPicker.GetPocketNoAll();

				for (int i = 0; i < nno.Length; i++)
				{
					if (nno[i] < 0) continue;

					if (CheckEjector(eSenPos, i, bAlarm) == false) return false;
				}

				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}
		


		public bool CheckUnloadPnPFull()
		{
			try
			{
				if (SM.FMP[FlowPart.ULDPnP].GetExistCount() == SM.FMP[FlowPart.ULDPnP].GetMaxProdCount()) return true;

				bool[] bexist = SM.FMP[FlowPart.ULDPnP].GetProdExist();

				for (int i = 0; i < bexist.Length; i++)
				{
					if (bexist[i] == false && SM.SHM.IsUsePart(UsePartName.Picker1 + i, UsePartMode.Disable) == false)
					{
						return false;
					}
				}

				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public bool PlaceTrayExist()
		{
			try
			{
				bool brt = true;

				if (SM.FMP[FlowPart.ULDPnP].IsResultExist(ProdStatus.NG) >= 0)
				{
					if (SM.FMT[FlowTray.Reject].IsFull() == false) brt = true;
					else brt = false;
				}

				if (SM.FMP[FlowPart.ULDPnP].IsResultExist(ProdStatus.OK) >= 0)
				{
					if (m_nCurTable >= 0) brt = true;
					else brt = false;
				}

				return brt;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}


		private void FlowMngMoveToPicker(int nTableNo, int nTrayPocketNo, int nPickerNo)
		{
			try
			{
				if (SM.FMP.IsFlow(FlowPart.ULDPnP) == false)
				{
					SM.FMP.Next(FlowPart.Empty, FlowPart.ULDPnP);
				}

				InfoPart.tsInfo info = SM.FMP[FlowPart.ULDPnP].GetInfo();

				if (nTableNo == 0 || nTableNo == 1)
				{
					FlowTray flow = FlowTray.ULDTable1;

					if (nTableNo == 0) flow = FlowTray.ULDTable1;
					else if (nTableNo == 1) flow = FlowTray.ULDTable2;
					else return;

					info.ProdInfo[nPickerNo] = SM.FMT[flow].GetInfo().ProdInfo[nTrayPocketNo];
					info.WorkOrder = SM.FMT[flow].GetInfo().WorkOrder;
					info.KeyValue = SM.FMT[flow].GetInfo().KeyValue;
					info.ProgramID_Top = SM.FMT[flow].GetInfo().ProgramID_Top;
					info.ProgramID_Btm = SM.FMT[flow].GetInfo().ProgramID_Btm;
					info.MaxCntTray = SM.FMT[flow].GetInfo().MaxCntTray;

					SM.FMP[FlowPart.ULDPnP].SetInfo(info);
					GLog.Add(string.Format($"Prod Flow {FlowTray.ULDTable1 + nTableNo} -> {FlowPart.ULDPnP} - {nPickerNo} . ID = {info.ProdInfo[nPickerNo].ProdID}"));

					SM.FMT[flow].ClearProd(nTrayPocketNo);
				}
				else if (nTableNo == 2)
				{
					InfoTray.tsInfo info_t = SM.FMT[FlowTray.Reject].GetInfo();

					info.ProdInfo[nPickerNo] = info_t.ProdInfo[nTrayPocketNo];
					info.WorkOrder = info_t.WorkOrder;
					info.KeyValue = info_t.KeyValue;
					info.ProgramID_Top = info_t.ProgramID_Top;
					info.ProgramID_Btm = info_t.ProgramID_Btm;
					info.MaxCntTray = info_t.MaxCntTray;

					SM.FMP[FlowPart.ULDPnP].SetInfo(info);

					GLog.Add(string.Format($"Prod Flow {FlowTray.Reject } -> {FlowPart.ULDPnP} - {nPickerNo} . ID = {info.ProdInfo[nPickerNo].ProdID}"));

					SM.FMT[FlowTray.Reject].ClearProd(nTrayPocketNo);
					if (SM.FMT[FlowTray.Reject].GetExistCount() == 0)
					{
						info_t.WorkOrder = "";
						SM.FMT[FlowTray.Reject].SetInfo(info_t);
					}
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void FlowMngMoveTableToPicker(int nTableNo, stActPicker ActPicker)
		{
			try
			{
				for (int i = 0; i < ActPicker.Picker.Length; i++)
				{
					if (ActPicker.GetPocketNo(i) == -1) continue;

					FlowMngMoveToPicker(nTableNo, ActPicker.GetPocketNo(i), i);
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void FlowMngMoveRejectToPicker(stActPicker ActPicker)
		{
			try
			{
				for (int i = 0; i < ActPicker.Picker.Length; i++)
				{
					if (ActPicker.GetPocketNo(i) == -1) continue;

					FlowMngMoveToPicker(2, ActPicker.GetPocketNo(i), i);
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void FlowMngMoveToTable(int nTableNo, int nTrayPocketNo, int nPickerNo)
		{
			try
			{
				if (nTableNo == 0 || nTableNo == 1)
				{
					InfoTray.tsInfo info_t;
					FlowTray flow = FlowTray.ULDTable1;

					if (nTableNo == 0) flow = FlowTray.ULDTable1;
					else if (nTableNo == 1) flow = FlowTray.ULDTable2;
					else return;

					info_t = SM.FMT[flow].GetInfo();

					if (SM.FMT[flow].GetExistCount() == 0)
					{
						info_t.WorkOrder = SM.FMP[FlowPart.ULDPnP].GetInfo().WorkOrder;
						info_t.KeyValue = SM.FMP[FlowPart.ULDPnP].GetInfo().KeyValue;
						info_t.ProgramID_Top = SM.FMP[FlowPart.ULDPnP].GetInfo().ProgramID_Top;
						info_t.ProgramID_Btm = SM.FMP[FlowPart.ULDPnP].GetInfo().ProgramID_Btm;
						if (SM.FMP[FlowPart.ULDPnP].GetInfo().MaxCntTray > 0)
						{
							info_t.MaxCntTray = SM.FMP[FlowPart.ULDPnP].GetInfo().MaxCntTray;
						}
						if (info_t.MaxCntProdInLot != SM.FMP[FlowPart.ULDPnP].GetInfo().MaxCntProdInLot)
						{
							info_t.MaxCntProdInLot = SM.FMP[FlowPart.ULDPnP].GetInfo().MaxCntProdInLot;
						}
					}

					info_t.ProdInfo[nTrayPocketNo] = SM.FMP[FlowPart.ULDPnP].GetInfo().ProdInfo[nPickerNo];
					info_t.ProdInfo[nTrayPocketNo].TrayCol = nTrayPocketNo % info_t.ColCnt;
					info_t.ProdInfo[nTrayPocketNo].TrayRow = nTrayPocketNo / info_t.ColCnt;
					info_t.ProdInfo[nTrayPocketNo].TrayID = info_t.TrayID;
					info_t.ProdInfo[nTrayPocketNo].KeyValue = info_t.KeyValue;
					info_t.ProdInfo[nTrayPocketNo].HandlingStatus = (int)ProdHandlingStatus.Normal;

					SM.FMT[flow].SetInfo(info_t);
					GLog.Add(string.Format($"Prod Flow {FlowPart.ULDPnP} -> {FlowTray.ULDTable1 + nTableNo} - {nPickerNo} . ID = {info_t.ProdInfo[nTrayPocketNo].ProdID}"));

					SM.FMP[FlowPart.ULDPnP].ClearProd(nPickerNo);
				}
				else if (nTableNo == 2)
				{
					InfoTray.tsInfo info_t;
					FlowTray flow = FlowTray.Reject;

					if (nTableNo == 2) flow = FlowTray.Reject;
					else return;

					info_t = SM.FMT[flow].GetInfo();

					if (info_t.WorkOrder == "")
					{
						info_t.WorkOrder = SM.FMP[FlowPart.ULDPnP].GetInfo().WorkOrder;
					}

					info_t.ProgramID_Top = SM.FMP[FlowPart.ULDPnP].GetInfo().ProgramID_Top;
					info_t.ProgramID_Btm = SM.FMP[FlowPart.ULDPnP].GetInfo().ProgramID_Btm;
					info_t.KeyValue = SM.FMP[FlowPart.ULDPnP].GetInfo().KeyValue;
					if (SM.FMP[FlowPart.ULDPnP].GetInfo().MaxCntTray > 0)
					{
						info_t.MaxCntTray = SM.FMP[FlowPart.ULDPnP].GetInfo().MaxCntTray;
					}

					info_t.ProdInfo[nTrayPocketNo] = SM.FMP[FlowPart.ULDPnP].GetInfo().ProdInfo[nPickerNo];
					info_t.ProdInfo[nTrayPocketNo].TrayCol = nTrayPocketNo % info_t.ColCnt;
					info_t.ProdInfo[nTrayPocketNo].TrayRow = nTrayPocketNo / info_t.ColCnt;

					SM.FMT[flow].SetInfo(info_t);
					GLog.Add(string.Format($"Prod Flow {FlowPart.ULDPnP} -> {FlowTray.Reject} - {nPickerNo} . ID = {info_t.ProdInfo[nTrayPocketNo].ProdID}"));

					SM.FMP[FlowPart.ULDPnP].ClearProd(nPickerNo);
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return;
			}
		}

		private void FlowMngMoveToTable(int nTableNo, stActPicker ActPicker)
		{
			try
			{
				for (int i = 0; i < ActPicker.Picker.Length; i++)
				{
					if (ActPicker.GetPocketNo(i) == -1) continue;

					FlowMngMoveToTable(nTableNo, ActPicker.GetPocketNo(i), i);
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return;
			}
		}

		private void FlowMngMoveToReject(stActPicker ActPicker)
		{
			try
			{
				int nrow = 0;
				int ncol = 0;
				string str_id = string.Empty;

				for (int i = 0; i < ActPicker.Picker.Length; i++)
				{
					if (ActPicker.GetPocketNo(i) == -1) continue;

					nrow = m_ActPicker.Picker[i].nRow;
					ncol = m_ActPicker.Picker[i].nCol;
					str_id = SM.FMT[FlowTray.Reject].GetInfo().ProdInfo[m_ActPicker.Picker[i].PocketNo].ProdID;

					FlowMngMoveToTable(2, ActPicker.GetPocketNo(i), i);

				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return;
			}
		}

		private bool FlowMngMoveToReject(int nPickerNo)
		{
			try
			{
				int nrow = 0;
				int ncol = 0;
				string str_id = string.Empty;

				if (m_ActPicker.GetPocketNo(nPickerNo) == -1) return false;

				nrow = m_ActPicker.Picker[nPickerNo].nRow;
				ncol = m_ActPicker.Picker[nPickerNo].nCol;
				str_id = SM.FMT[FlowTray.Reject].GetInfo().ProdInfo[m_ActPicker.Picker[nPickerNo].PocketNo].ProdID;

				FlowMngMoveToTable(2, m_ActPicker.GetPocketNo(nPickerNo), nPickerNo);

				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		private ProdExist CheckProdExist(bool bAlarm = false)
		{
			try
			{
				FlowPart flowPart = FlowPart.ULDPnP;
				DI ExistSenStart = DI.ULDPnPVac1;

				bool[] bexist = SM.FMP[flowPart].GetProdExist();
				bool[] sen = new bool[bexist.Length];

				for (int i = 0; i < bexist.Length; i++)
					sen[i] = G.DryRunIsAIn(ExistSenStart + i, bexist[i]);

				// 완전 없음
				if (sen.All(s => s == false) && SM.FMP[flowPart].GetExistCount() == 0)
					return ProdExist.None;

				bool isMismatch = false;
				for (int i = 0; i < bexist.Length; i++)
				{
					if (bexist[i] != sen[i])
					{
						SM.FMP[flowPart].SetPickupStatus(ProdPickStatus.Mismatch, i);
						if (bAlarm == true) AddAlarm(ALM.ERROR_SEQ_UNLOAD_PnP_PICKER1_PROD_PLACE_FAIL + i);
						isMismatch = true;
					}
				}

				if (isMismatch)
				{
					string dataStr = string.Join(", ", bexist.Select(b => b.ToString()));
					string sensorStr = string.Join(", ", sen.Select(s => s.ToString()));

					GLog.AddErr($"{flowPart.ToString()} Mismatching. Data: {dataStr}, Sensor: {sensorStr}");


					return ProdExist.Mismatch;
				}

				// 모두 정상
				SM.FMP[flowPart].SetPickupStatus(ProdPickStatus.Normal);
				return ProdExist.Exist;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, $"Message : {ex.Message}\nStack trace : {ex.StackTrace}");
				return ProdExist.Mismatch;
			}
		}
		
		//status  Unkonwn : Sorting 시
		private void SearchPlaceTarget(ProdStatus PnPstatus = ProdStatus.Unknown)
		{
			try
			{
				int ncnt		= 0;
				int ncol		= 0;
				int npocket		= 0;

				int nstep		= SM.SHM.GetRecipeDataToInt(DEVICE.GripStepCnt) / 1000;
				int maxCount	= SM.FMP[FlowPart.ULDPnP].GetMaxProdCount();

				FlowTray flow = FlowTray.Empty;
				if (PnPstatus == ProdStatus.NG)			flow = FlowTray.Reject;
				else if (PnPstatus == ProdStatus.OK)	flow = GetFlow(m_nSeqUsingTable);
				else									flow = (FlowTray)m_SortFlow;
				
				m_ActPicker.ResetAll();
				
				if (m_TrayFirstDirection == Dir.Right)
				{
					for (int i = m_nStartPicker; i >= 0; i--)
					{
						ncol = m_nProdCol + ncnt + nstep * ncnt;
						npocket = m_nProdRow * m_nCntTrayCol + ncol;
						
						if (ncol >= m_nCntTrayCol)										continue;
						if (ncol < 0)													continue;
						if (i != m_nStartPicker  &&  
							CheckGapValid(ncol, i % m_PickerMaxCol) == false)
						{
							continue;
						}
						
						if (SM.FMT[flow].GetInfo().ProdInfo[npocket].Exist == false  ||	
							m_ActPicker.ContainsPocket(npocket) == true)
						{
							ncnt++; 
							continue;
						}

						if (SM.FMP[FlowPart.ULDPnP].GetProdStatus(i) == PnPstatus)
						{
							if (PnPstatus == ProdStatus.OK &&
								(SM.FMP[FlowPart.ULDPnP].GetInfo().ProdInfo[i].VerifyTopDone == true ||
								 SM.FMP[FlowPart.ULDPnP].GetInfo().ProdInfo[i].VerifyBtmDone == true))
							{
								m_ActPicker.Set(i, ncol, m_nProdRow, m_nCntTrayCol, true);
							}
							else
							{
								m_ActPicker.Set(i, ncol, m_nProdRow, m_nCntTrayCol, false);
							}
						}

						ncnt++;
					}
				}
				else
				{
					for (int i = m_nStartPicker; i < maxCount; i ++)
					{
						ncol = m_nProdCol + ncnt + nstep * ncnt;
						npocket = m_nProdRow * m_nCntTrayCol + ncol;
						
						if (ncol >= m_nCntTrayCol)										continue;
						if (ncol < 0)													continue;
						if (i != m_nStartPicker  &&  
							CheckGapValid(ncol, i % m_PickerMaxCol) == false)
						{
							continue;
						}
						
						if (SM.FMT[flow].GetInfo().ProdInfo[npocket].Exist == true  ||	
							m_ActPicker.ContainsPocket(npocket) == true  ||
							SM.FMP[FlowPart.ULDPnP].GetProdExist(i) == false )
						{
							ncnt++; 
							continue;
						}

						if (PnPstatus == ProdStatus.Unknown  ||  SM.FMP[FlowPart.ULDPnP].GetProdStatus(i) == PnPstatus)
						{
							if (PnPstatus == ProdStatus.OK &&
								(SM.FMP[FlowPart.ULDPnP].GetInfo().ProdInfo[i].VerifyTopDone == true ||
								 SM.FMP[FlowPart.ULDPnP].GetInfo().ProdInfo[i].VerifyBtmDone == true))
							{
								m_ActPicker.Set(i, ncol, m_nProdRow, m_nCntTrayCol, true);
							}
							else
							{
								m_ActPicker.Set(i, ncol, m_nProdRow, m_nCntTrayCol, false);
							}
						}

						ncnt++;
					}
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return;
			}
		}

		
		//CYJ 모든 PLACE 함수를 시작하기 전에 진행
		private void UpdateTrayDir(bool bIsSorting)
		{
			try
			{
				if (bIsSorting == false)
				{
					m_TrayFirstDirection = Dir.Left;
					m_TraySecondDirection = Dir.Top;
					return;
				}

				switch (SM.SHM.GetRecipeData(DEVICE.TrayFirstDir))
				{
					case "T":
						m_TrayFirstDirection = Dir.Top;
						break;
					case "B":
						m_TrayFirstDirection = Dir.Bottom;
						break;
					case "L":
						m_TrayFirstDirection = Dir.Left;
						break;
					case "R":
						m_TrayFirstDirection = Dir.Right;
						break;
					default:
						m_TrayFirstDirection = Dir.Left; // 설정이 하나라도 안되어 있으면 기본으로 변경
						m_TraySecondDirection = Dir.Top;
						return;
				}

				switch (SM.SHM.GetRecipeData(DEVICE.TraySecondDir))
				{
					case "T":
						m_TraySecondDirection = Dir.Top;
						break;
					case "B":
						m_TraySecondDirection = Dir.Bottom;
						break;
					case "L":
						m_TraySecondDirection = Dir.Left;
						break;
					case "R":
						m_TraySecondDirection = Dir.Right;
						break;
					default:
						m_TrayFirstDirection = Dir.Left; // 설정이 하나라도 안되어 있으면 기본으로 변경
						m_TraySecondDirection = Dir.Top;
						break;
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}


		private bool SetSeqRun(ref SMM<Cmd> smm)
		{
			try
			{
				if (this.IsReady == false) return false;

				m_nCntTrayCol = SM.SHM.GetRecipeDataToInt(DEVICE.TrayColCnt) / 1000;
				m_nCntTrayRow = SM.SHM.GetRecipeDataToInt(DEVICE.TrayRowCnt) / 1000;

				smm.eInput = InState.run;
				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}
		//		#endregion



		public void Run()
		{
			try
			{
				UpdateCurTable();
				UpdateAct(Seq.UnloadPnP);

				if (SM.SHM.IsBitData(BIT.AutoRun) == true)
				{
					if (m_smmPickRejectRetry.IsReady == false) m_smmPickRejectRetry.ResetStep();
				}
				else
				{
					PickRejectRetry(m_smmPickRejectRetry);
				}

				SM.SHM.SetSeqStatus(lstSMM);

				if (SM.SHM.IsBitData(BIT.Error) == true || SM.SHM.IsBitData(BIT.Pause) == true) return;
				//if (SM.SHM.IsBitData(BIT.AutoRun) == true && IsReady == true && CheckProdExist() == teProdExist.Mismatch) AddAlarm(ALM.ERROR_SEQ_UNLOAD_PnP_PROD_MISMATCH);

				Home(m_smmHome);
				PickBuf(m_smmPickBuf);
				PickReject(m_smmPickReject);
				Place(m_smmPlace);
				PnP(m_smmPnP);
				Ready(m_smmReady);
				Sort(m_smmSort);
				SortPlace(m_smmSortPlace);
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
							if (SM.FMP.IsFlow(FlowPart.ULDPnP) == false)
							{
								if (CheckEjector(SenPos.BLOW) == false)
								{
									RunEjector(SenPos.BLOW);
									break;
								}
							}
							else
							{
                                bool bCheckComplete = false;
								for (int i = 0; i < SM.FMP[FlowPart.ULDPnP].GetMaxProdCount(); i++)
								{
									if (SM.FMP[FlowPart.ULDPnP].GetInfo().ProdInfo[i].Exist == false)
									{
										if (CheckEjector(SenPos.BLOW, i) == false)
										{
											RunEjector(SenPos.BLOW, i);
											break;
										}

                                        bCheckComplete = true;
                                    }
									else
									{

                                        if (smm.Delayms(NDm.Read(eNDM.UnloadPnPVacDelay)) == true)
                                        {
                                            smm.ResetDelayms();
                                            CheckEjector(SenPos.VAC, i, true);
                                            break;
                                        }

                                        if (CheckEjector(SenPos.VAC, i) == false)
										{
											RunEjector(SenPos.VAC, i);
                                            bCheckComplete = false;

                                            continue;
										}

                                        bCheckComplete = true; 
                                    }
                                }
                                if (bCheckComplete == false)
                                {
                                    break;
                                }
                            }
							smm.Next(Cmd.HOME_20);
						}
						break;

					case Cmd.HOME_20:
						{
							if (CheckCylUpDown(SenPos.PICKER_UP) == false)
							{
								RunCylUpDown(SenPos.PICKER_UP);
								break;
							}
							
							if (m_IsMtHome == false)
							{
								smm.Next(Cmd.HOME_70);
								break;
							}

							smm.Next(Cmd.HOME_30);
						}
						break;

					case Cmd.HOME_30:
						{
							arrMt[Axis.Z].ClearHome();
							arrMt[Axis.Z].StartHoming();
						
							smm.Next(Cmd.HOME_40);
						}
						break;

					case Cmd.HOME_40:
                        {
                            if (smm.Delayms(100) == false) break;

                            if (arrMt[Axis.Z].IsHomeDone() == false || arrMt[Axis.Z].IsRdy() == false) break;
							
							arrMt[Axis.X].ClearHome();
							arrMt[Axis.Y].ClearHome();
							arrMt[Axis.W].ClearHome();
							
							arrMt[Axis.X].StartHoming();
							arrMt[Axis.Y].StartHoming();
							arrMt[Axis.W].StartHoming();

							smm.Next(Cmd.HOME_60);
						}
						break;

					case Cmd.HOME_60:
						{
							if (arrMt[Axis.X].IsHomeDone() == false || arrMt[Axis.X].IsRdy() == false) break;
							if (arrMt[Axis.Y].IsHomeDone() == false || arrMt[Axis.Y].IsRdy() == false) break;
							if (arrMt[Axis.W].IsHomeDone() == false || arrMt[Axis.W].IsRdy() == false) break;

							smm.Next(Cmd.HOME_70);
						}
						break;

					case Cmd.HOME_70:
						{
							if (arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false)
							{
								arrMt[Axis.Z].MoveIndex((int)PosZ.READY);
								break;
							}

							smm.Next(Cmd.HOME_80);
						}
						break;

					case Cmd.HOME_80:
						{
							if (arrMt[Axis.X].IsRdy((int)PosX.READY) == false ||
								arrMt[Axis.Y].IsRdy((int)PosY.READY) == false||
								arrMt[Axis.W].IsRdy((int)PosW.READY) == false)
							{
								arrMt[Axis.X].MoveIndex((int)PosX.READY);
								arrMt[Axis.Y].MoveIndex((int)PosY.READY);
								arrMt[Axis.W].MoveIndex((int)PosW.READY);
								break;
							}

							smm.Next(Cmd.END);
						}
						break;

					case Cmd.END:
						{
							m_bRejectPickOnlyOK = false;
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

		private void PickBuf(SMM<Cmd> smm)
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

								smm.Next(Cmd.PICK_BUF_10);
							}
						}
						break;

					case Cmd.PICK_BUF_10:
						{
							switch (CheckProdExist())
							{
								case ProdExist.Exist:
									AddAlarm(ALM.ERROR_SEQ_UNLOAD_PnP_PROD_EXIST);
									break;
								case ProdExist.Mismatch:
									AddAlarm(ALM.ERROR_SEQ_UNLOAD_PnP_PROD_MISMATCH);
									break;
								case ProdExist.None:
									if (SM.FMP.IsFlow(FlowPart.ULDPnP) == true) SM.FMP.Clear(FlowPart.ULDPnP);
									smm.Next(Cmd.PICK_BUF_20);
									break;
							}
						}
						break;

					case Cmd.PICK_BUF_20:
						{
							if (arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false ||
								CheckCylUpDown(SenPos.PICKER_UP) == false)
							{
								arrMt[Axis.Z].MoveIndex((int)PosZ.READY);
								RunCylUpDown(SenPos.PICKER_UP);
								break;
							}
							smm.Next(Cmd.PICK_BUF_30);
						}
						break;

					case Cmd.PICK_BUF_30:
						{
							if (arrMt[Axis.X].IsRdy((int)PosX.BUFFER) == false ||
								arrMt[Axis.Y].IsRdy((int)PosY.BUFFER) == false ||
								arrMt[Axis.W].IsRdy((int)PosW.BUFFER) == false)
							{
								arrMt[Axis.X].MoveIndex((int)PosX.BUFFER);
								arrMt[Axis.Y].MoveIndex((int)PosY.BUFFER);
								arrMt[Axis.W].MoveIndex((int)PosW.BUFFER);
								break;
							}

							smm.Next(Cmd.PICK_BUF_40);
						}
						break;

					case Cmd.PICK_BUF_40:
						{
							if (G.seqUnloadBuffer.IsULDWait() == false) break;

							bool[] bexist = SM.FMP[FlowPart.ULDBuffer].GetProdExist();

							m_ActPicker.ResetAll();
							for (int i = 0; i < bexist.Length; i++)
							{
								if (bexist[i] == true)
								{
									m_ActPicker.Set(i, i, 0, 4);
								}
							}

							smm.Next(Cmd.PICK_BUF_50);
						}
						break;

					case Cmd.PICK_BUF_50:
						{
							if (CheckCylUpDown(SenPos.PICKER_DOWN, m_ActPicker) == false ||
								arrMt[Axis.Z].IsRdy((int)PosZ.BUFFER_PICK) == false)
							{
								RunCylUpDown(SenPos.PICKER_DOWN, m_ActPicker);
								arrMt[Axis.Z].MoveIndex((int)PosZ.BUFFER_PICK);
								break;
							}

							smm.Next(Cmd.PICK_BUF_60);
						}
						break;

					case Cmd.PICK_BUF_60:
						{
							if (smm.Delayms(100) == false) break;

							//그냥 올린뒤 뒤에서 미스매치 알람 발생
                            if (smm.Delayms(NDm.Read(eNDM.UnloadPnPVacDelay)) == true)
                            {
                                smm.Next(Cmd.PICK_BUF_70);
                                break;
                            }

                            if (CheckPickerEjector(SenPos.VAC, m_ActPicker) == false)
							{
								RunPickerEjector(SenPos.VAC, m_ActPicker);
								break;
							}
							
							smm.Next(Cmd.PICK_BUF_70);
						}
						break;

					case Cmd.PICK_BUF_70:
						{	
							for (int i = 0; i < m_ActPicker.Count; i++)
							{
								if (m_ActPicker.GetPocketNo(i) < 0) continue;
								SM.FMP.Next(FlowPart.ULDBuffer, FlowPart.ULDPnP);
							}

							for (int i = 0; i < SM.FMP[FlowPart.ULDPnP].GetMaxProdCount(); i++)
							{
								if (SM.FMP[FlowPart.ULDPnP].GetProdExist(i))
								{
									if (SM.FMP[FlowPart.ULDPnP].GetProdStatus(i) == ProdStatus.NG)	NDm.Write(eNDM.CurContinuousNGCount, NDm.Read(eNDM.CurContinuousNGCount) + 1);
									else															NDm.Write(eNDM.CurContinuousNGCount, 0);
								}
							}

							smm.Next(Cmd.PICK_BUF_80);
						}
						break;

					case Cmd.PICK_BUF_80:
						{	
							if (G.seqUnloadBuffer.IsBufferGrip() == true) break;

							smm.Next(Cmd.PICK_BUF_90);
						}
						break;

					case Cmd.PICK_BUF_90:
						{
							if (smm.Delayms(100) == false) break;

							if (CheckCylUpDown(SenPos.PICKER_UP) == false ||
								arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false)
							{
								RunCylUpDown(SenPos.PICKER_UP);
								arrMt[Axis.Z].MoveIndex((int)PosZ.READY);
								break;
							}

							smm.Next(Cmd.PICK_BUF_100);
						}
						break;

					case Cmd.PICK_BUF_100:
						{
							switch (CheckProdExist())
							{
								case ProdExist.Exist:
									smm.Next(Cmd.END);
									break;
								case ProdExist.Mismatch:
									AddAlarm(ALM.ERROR_SEQ_UNLOAD_PnP_PROD_MISMATCH);
									break;
								case ProdExist.None:
									AddAlarm(ALM.ERROR_SEQ_UNLOAD_PnP_PROD_NONE);
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

		private void PickReject(SMM<Cmd> smm)
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

								smm.Next(Cmd.PICK_REJECT_10);
							}
						}
						break;

					case Cmd.PICK_REJECT_10:
						{
							if (arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false ||
								CheckCylUpDown(SenPos.PICKER_UP) == false)
							{
								arrMt[Axis.Z].MoveIndex((int)PosZ.READY);
								RunCylUpDown(SenPos.PICKER_UP);
								break;
							}

							smm.Next(Cmd.PICK_REJECT_20);
						}
						break;

					case Cmd.PICK_REJECT_20:
						{	
							if (arrMt[Axis.W].IsRdy((int)PosW.TABLE) == false)
							{
								arrMt[Axis.W].MoveIndex((int)PosW.TABLE);
								break;
							}

                            smm.Next(Cmd.PICK_REJECT_30);

                        }
						break;

                    case Cmd.PICK_REJECT_30:
						{
							m_nStartPicker = -1;
							for (int i = 0; i < SM.FMP[FlowPart.ULDPnP].GetMaxProdCount(); i++)
							{
								if (SM.SHM.IsUsePart(UsePartName.Picker1 + i, UsePartMode.Disable) == true) continue;

								if (SM.FMP[FlowPart.ULDPnP].GetProdExist(i) == false)
								{
									m_nStartPicker = i;
									break;
								}
							}
							if (m_nStartPicker == -1)
							{
								smm.Next(Cmd.PICK_REJECT_110);
								break;
							}

							smm.Next(Cmd.PICK_REJECT_40);
						}
						break;

					case Cmd.PICK_REJECT_40:
						{
							if (SearchProdReject(Dir.Left, ProdStatus.OK, ref m_nProdCol, ref m_nProdRow) == true)
							{
								m_RejectPickMode = ProdStatus.OK;
								GLog.Add("Pick Reject - OK");
								smm.Next(Cmd.PICK_REJECT_50);
								break;
							}
							else
							{
								smm.Next(Cmd.PICK_REJECT_110);
							}
						}
						break;

					case Cmd.PICK_REJECT_50:
						{
							int ncol = 0;
							int ncnt = 0;
							int npocket = 0;
							int nstep = SM.SHM.GetRecipeDataToInt(DEVICE.GripStepCnt) / 1000;

							m_ActPicker.ResetAll();

							for (int i = m_nStartPicker; i < SM.FMP[FlowPart.LDPnP].GetMaxProdCount(); i++)
							{
								if (SM.SHM.IsUsePart(UsePartName.Picker1 + i, UsePartMode.Disable) == true)
								{
									ncnt++;
									continue;
								}

								ncol = m_nProdCol + ncnt + nstep * ncnt;
								npocket = m_nProdRow * SM.FMT[FlowTray.Reject].GetInfo().ColCnt + ncol;

								if (ncol < m_nCntTrayCol &&
									i > m_nStartPicker &&
									ncol > 0)
								{
									if (CheckGapValid(ncol, i % m_PickerMaxCol) == false) break;
								}

								if (ncol < SM.FMT[FlowTray.Reject].GetInfo().ColCnt &&
									SM.FMP[FlowPart.ULDPnP].GetProdExist(i) == false &&
									m_ActPicker.ContainsPocket(npocket) == false &&
									SM.FMT[FlowTray.Reject].GetInfo().ProdInfo[npocket].Exist == true &&
									SM.FMT[FlowTray.Reject].GetProdStatus(npocket) == m_RejectPickMode &&
									SM.FMT[FlowTray.Reject].CheckVerifyComplete(npocket) == true)
								{
									m_ActPicker.Set(i, ncol, m_nProdRow, SM.FMT[FlowTray.Reject].GetInfo().ColCnt);
								}

								ncnt++;
							}

							GetPnPPosTable(2, m_nStartPicker, m_nProdCol, m_nProdRow, ref m_nAbsPosX, ref m_nAbsPosY, true);

                            smm.Next(Cmd.PICK_REJECT_60);
                        }
						break;

					case Cmd.PICK_REJECT_60:
						{
                            if (G.seqRejectBuffer.IsPnPReady() == false) break;

							if (arrMt[Axis.X].IsRdyPos(m_nAbsPosX) == false ||
								arrMt[Axis.Y].IsRdyPos(m_nAbsPosY) == false ||
								arrMt[Axis.W].IsRdy((int)PosW.TABLE) == false)
							{
								arrMt[Axis.X].MoveABS(m_nAbsPosX, (int)PosX.REJECT_PICK_FIRST);
								arrMt[Axis.Y].MoveABS(m_nAbsPosY, (int)PosY.REJECT_PICK_FIRST);
								arrMt[Axis.W].MoveIndex((int)PosW.TABLE);
								break;
							}

							if (SM.FMT[FlowTray.Reject].IsAlignDone(m_nProdCol + m_nProdRow * SM.FMT[FlowTray.Reject].GetInfo().ColCnt) == false)
							{
								if (G.seqRejectBuffer.IsReady == true)
									G.seqRejectBuffer.AlignStart();
							}

							smm.Next(Cmd.PICK_REJECT_70);
						}
						break;


                    case Cmd.PICK_REJECT_70:
						{
							if (arrMt[Axis.Z].IsRdy((int)PosZ.REJECT_PICK) == false ||
							CheckCylUpDown(SenPos.PICKER_DOWN, m_ActPicker) == false)
							{
								arrMt[Axis.Z].MoveIndex((int)PosZ.REJECT_PICK);
								RunCylUpDown(SenPos.PICKER_DOWN, m_ActPicker);
								break;
							}

							smm.Next(Cmd.PICK_REJECT_80);
						}
						break;

					case Cmd.PICK_REJECT_80:
						{
							if (smm.Delayms(100) == false) break;

                            //그냥 올린뒤 뒤에서 미스매치 알람 발생
                            if (smm.Delayms(NDm.Read(eNDM.UnloadPnPVacDelay)) == true)
                            {
                                smm.Next(Cmd.PICK_REJECT_90);
                                break;
                            }

                            if (CheckPickerEjector(SenPos.VAC, m_ActPicker) == false)
							{
								RunPickerEjector(SenPos.VAC, m_ActPicker);
								break;
							}

							smm.Next(Cmd.PICK_REJECT_90);
						}
						break;

					case Cmd.PICK_REJECT_90:
						{
							RunCylUpDown(SenPos.PICKER_UP);

							FlowMngMoveRejectToPicker(m_ActPicker);

							string CurKeyValue = SM.FMP[FlowPart.ULDPnP].GetInfo().KeyValue;
							int[] Yield = new int[3] { 0, 0, 0 }; // ok , ng , user ok

							Yield = SM.SHM.GetLotYield(CurKeyValue);
							Yield[1] -= m_ActPicker.CountExist();
							SM.SHM.SetLotYield(CurKeyValue, Yield);

							int tempCount = NDm.Read(eNDM.CurNGProdCount);
							NDm.Write(eNDM.CurNGProdCount, tempCount - m_ActPicker.CountExist());

							smm.Next(Cmd.PICK_REJECT_100);
						}
						break;

					case Cmd.PICK_REJECT_100:
						{
							if (CheckCylUpDown(SenPos.PICKER_UP) == false)
							{
								RunCylUpDown(SenPos.PICKER_UP);
								break;
							}

							switch (CheckProdExist())
							{
								case ProdExist.Exist:
									if (CheckUnloadPnPFull() == true)	smm.Next(Cmd.PICK_REJECT_110);
									else								smm.Next(Cmd.PICK_REJECT_30);

									break;
								case ProdExist.Mismatch:
									AddAlarm(ALM.ERROR_SEQ_UNLOAD_PnP_PROD_MISMATCH);
									break;
								case ProdExist.None:
									AddAlarm(ALM.ERROR_SEQ_UNLOAD_PnP_PROD_NONE);
									break;
							}

						}
						break;

					case Cmd.PICK_REJECT_110:
						{

                            if (arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false ||
								CheckCylUpDown(SenPos.PICKER_UP) == false)
							{
								arrMt[Axis.Z].MoveIndex((int)PosZ.READY);
								RunCylUpDown(SenPos.PICKER_UP);
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

		private void PickRejectRetry(SMM<Cmd> smm)
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

								smm.Next(Cmd.PICK_REJECT_RETRY_10);
							}
						}
						break;

					case Cmd.PICK_REJECT_RETRY_10:
						{
							if (SM.FMT[FlowTray.Reject].GetProdStatus(m_nPickRetryNo) == ProdStatus.None &&
								 G.cKernel.IsAIn(DI.ULDPnPVac1 + m_nPickRetryNo) == false)
							{
								smm.Next(Cmd.PICK_REJECT_RETRY_20);
							}
							else if (SM.FMT[FlowTray.Reject].GetProdStatus(m_nPickRetryNo) != ProdStatus.None &&
									 G.cKernel.IsAIn(DI.ULDPnPVac1 + m_nPickRetryNo) == false)
							{
								frmMessageBox msg = new frmMessageBox(frmMessageBox.teButtons.OKCancel, "CHECK", "Sensors are not detected, only data is present.\nDo you want to move the data to the Reject Buffer?");

								if (msg.ShowDialog() == DialogResult.Cancel) smm.Next(Cmd.END);
								else smm.Next(Cmd.PICK_REJECT_RETRY_20);
							}
							else
							{
								frmMessageBox msg = new frmMessageBox(frmMessageBox.teButtons.OK, "ERROR", "This is not a condition to pick.");
								msg.ShowDialog();
								smm.Next(Cmd.END);
							}
						}
						break;

					case Cmd.PICK_REJECT_RETRY_20:
						{
							// 중요 : PickReject 시퀀스에서 m_ActPicker.Get(m_nPickRetryNo).PocketNo 값이 남아 있어야 재시도 가능
							if (m_ActPicker.Get(m_nPickRetryNo).PocketNo < 0)
							{
								frmMessageBox msg = new frmMessageBox(frmMessageBox.teButtons.OK, "ERROR", "The Reject Buffer Pocket number is missing.");
								msg.ShowDialog();
								smm.Next(Cmd.END);
							}
							else
							{

								if (FlowMngMoveToReject(m_nPickRetryNo) == false) smm.Next(Cmd.END);
								else smm.Next(Cmd.PICK_REJECT_RETRY_30);
							}
						}
						break;

					case Cmd.PICK_REJECT_RETRY_30:
						{
							if (CheckEjector(SenPos.BLOW, m_nPickRetryNo) == false)
							{
								RunEjector(SenPos.BLOW, m_nPickRetryNo);
								break;
							}

							smm.Next(Cmd.PICK_REJECT_RETRY_40);
						}
						break;

					case Cmd.PICK_REJECT_RETRY_40:
						{
							if (arrMt[Axis.Z].IsRdy((int)PosZ.REJECT_PICK) == false ||
								CheckCylUpDown(SenPos.PICKER_DOWN, m_nPickRetryNo) == false)
							{
								arrMt[Axis.Z].MoveIndex((int)PosZ.REJECT_PICK);
								RunCylUpDown(SenPos.PICKER_DOWN, m_nPickRetryNo);
								break;
							}

							smm.Next(Cmd.PICK_REJECT_RETRY_50);
						}
						break;

					case Cmd.PICK_REJECT_RETRY_50:
						{
							if (smm.Delayms(100) == false) break;

                            if (smm.Delayms(NDm.Read(eNDM.UnloadPnPVacDelay)) == true)
                            {
                                smm.ResetDelayms();
                                CheckEjector(SenPos.VAC, m_nPickRetryNo, true);
                                break;
                            }

                            if (CheckEjector(SenPos.VAC, m_nPickRetryNo) == false)
							{
								RunEjector(SenPos.VAC, m_nPickRetryNo);
								break;
							}

							smm.Next(Cmd.PICK_REJECT_RETRY_60);
						}
						break;

					case Cmd.PICK_REJECT_RETRY_60:
						{
							RunCylUpDown(SenPos.PICKER_UP);

							FlowMngMoveToPicker(2, m_ActPicker.GetPocketNo(m_nPickRetryNo), m_nPickRetryNo);

							smm.Next(Cmd.PICK_REJECT_RETRY_70);
						}
						break;

					case Cmd.PICK_REJECT_RETRY_70:
						{
							if (CheckCylUpDown(SenPos.PICKER_UP) == false)
							{
								RunCylUpDown(SenPos.PICKER_UP);
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

		private void Place(SMM<Cmd> smm)
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
								UpdateTrayDir(false);
								smm.Next(Cmd.PLACE_10);
							}
						}
						break;

					case Cmd.PLACE_10:
						{
							m_nSeqUsingTable = -1;

							if (SM.FMP.IsFlow(FlowPart.ULDPnP) == false)
							{
								smm.Next(Cmd.END); //종료시퀀스
								break;
							}

							if (arrMt[Axis.Z].IsRdy() == false) break;

							smm.Next(Cmd.PLACE_20);
						}
						break;

					case Cmd.PLACE_20:
						{
							switch (CheckProdExist())
							{
								case ProdExist.Exist:
									smm.Next(Cmd.PLACE_30);
									break;
								case ProdExist.Mismatch:
									AddAlarm(ALM.ERROR_SEQ_UNLOAD_PnP_PROD_MISMATCH);
									smm.Next(Cmd.END);
									break;
								case ProdExist.None:
									AddAlarm(ALM.ERROR_SEQ_UNLOAD_PnP_PROD_NONE);
									smm.Next(Cmd.END);
									break;
							}
						}
						break;

					case Cmd.PLACE_30:
						{
							if (SM.FMP.IsFlow(FlowPart.ULDPnP) == false)
							{
								smm.Next(Cmd.END);
								break;
							}

							if (SM.FMP[FlowPart.ULDPnP].GetExistCount() == 0)
							{
								if (SM.FMP[FlowPart.ULDPnP].GetInfo().LastPieceOfLot == true)
								{
									if (SM.FMT[FlowTray.Reject].GetExistCount() > 0 &&
										SM.FMT[FlowTray.Reject].GetVerifyNotCount() != 0)
									{
										NDm.Write(eNDM.LotEndMode, (int)LotEndMode.LotEnd);
										NDm.Write(eNDM.RejectRunMode, (int)RejectRunMode.VerifyReq);
									}
									else
									{
										NDm.Write(eNDM.LotEndMode, (int)LotEndMode.LotEnd);
									}

									string strKeyValue = SM.FMP[FlowPart.ULDPnP].GetInfo().KeyValue;
									if (SM.SHM.m_dicLotPickTime.ContainsKey(strKeyValue) == true)
									{
										int ProdCount = 0;
										int[] tempYield = { 0, 0, 0 }; //ok , ng, user-ok
										tempYield = SM.SHM.GetLotYield(strKeyValue);
										ProdCount = tempYield[0] + tempYield[1] + tempYield[2];

										DateTime formattedTime = DateTime.ParseExact(SM.SHM.m_dicLotPickTime[strKeyValue], "yyyy-MM-dd HH:mm:ss.fff", null);
										DateTime currentTime = DateTime.Now;
										TimeSpan timeDifference = currentTime - formattedTime;
										GLog.Add($"UPH - LOT ID : {strKeyValue}  Count : {ProdCount}  Time = {timeDifference.TotalSeconds:F3}");
										SM.SHM.m_dicLotPickTime.Remove(strKeyValue);
									}
								}

								if (NDm.Read(eNDM.FillDoneRemainCount) <= 0)
								{
									NDm.Write(eNDM.FillDoneRemainCount, SM.FMP[FlowPart.ULDPnP].GetInfo().MaxCntTray);
									GLog.Add(string.Format("Tray Remain Count : {0}", NDm.Read(eNDM.FillDoneRemainCount)));
								}

								SM.FMP.Clear(FlowPart.ULDPnP);

								smm.Next(Cmd.END);
								break;
							}
							else if (m_nSeqUsingTable != -1)
							{
								if (SM.FMP[FlowPart.ULDPnP].IsResultExist(ProdStatus.NG) != -1)
								{
									smm.Next(Cmd.PLACE_REJECT_10);
									break;
								}
								else if (CheckTrayPlaceEnd(GetFlow(m_nSeqUsingTable)) == true ||
										 SM.FMT[GetFlow(m_nSeqUsingTable)].IsFull() == true)
								{
									smm.Next(Cmd.END);
									break;
								}
							}

							smm.Next(Cmd.PLACE_40);
						}
						break;

					case Cmd.PLACE_40:
						{
							if (m_PlaceMode == PlaceMode.Reject)
							{
								if (SM.FMT[FlowTray.Reject].IsFull() == true &&
									SM.FMT[FlowTray.Reject].GetVerifyNotCount() != 0)
								{
									NDm.Write(eNDM.RejectRunMode, (int)RejectRunMode.VerifyReq);  // 만약 Reject쪽에 여유공간이 없으면 verifyRequest On
									smm.Next(Cmd.PLACE_UNLOAD_10);
								}
								else
								{
									smm.Next(Cmd.PLACE_REJECT_10);
								}
							}
							else if (m_PlaceMode == PlaceMode.OK ||
									 m_PlaceMode == PlaceMode.NG)
							{
								smm.Next(Cmd.PLACE_UNLOAD_10);
							}
							else
							{
								smm.Next(Cmd.END);
							}
						}
						break;

					case Cmd.PLACE_REJECT_10:
						{
							if (SearchEmptyReject(Dir.Left, ref m_nProdCol, ref m_nProdRow) == false)
							{
								smm.Next(Cmd.END);
								break;
							}

							if (G.seqRejectBuffer.IsReadyPos() == false || G.seqRejectBuffer.IsPnPReady() == false)
							{
								smm.Next(Cmd.END);
								break;
							}

							smm.Next(Cmd.PLACE_REJECT_20);
						}
						break;

					case Cmd.PLACE_REJECT_20:
						{
							m_nStartPicker = SM.FMP[FlowPart.ULDPnP].IsResultExist(ProdStatus.NG);

							if (m_nStartPicker == -1)
							{
								smm.Next(Cmd.PLACE_30);
								break;
							}

							smm.Next(Cmd.PLACE_REJECT_30);
						}
						break;

					case Cmd.PLACE_REJECT_30:
						{	
							SearchPlaceTarget(ProdStatus.NG);
							GetPnPPosTable(2, m_nStartPicker, m_nProdCol, m_nProdRow, ref m_nAbsPosX, ref m_nAbsPosY, false);

							smm.Next(Cmd.PLACE_REJECT_40);
						}
						break;

					case Cmd.PLACE_REJECT_40:
						{
							if (G.seqRejectBuffer.IsPnPReady() == false) break;
							
							if (arrMt[Axis.X].IsRdyPos(m_nAbsPosX) == false ||
								arrMt[Axis.Y].IsRdyPos(m_nAbsPosY) == false ||
								arrMt[Axis.W].IsRdy((int)PosW.TABLE) == false)
							{
								arrMt[Axis.X].MoveABS(m_nAbsPosX, (int)PosX.REJECT_PLACE_FIRST);
								arrMt[Axis.Y].MoveABS(m_nAbsPosY, (int)PosY.REJECT_PLACE_FIRST);
								arrMt[Axis.W].MoveIndex((int)PosW.TABLE);
								break;
							}
							

							smm.Next(Cmd.PLACE_REJECT_50);
						}
						break;

					case Cmd.PLACE_REJECT_50:
						{
							if (CheckCylUpDown(SenPos.PICKER_DOWN, m_ActPicker) == false ||
								arrMt[Axis.Z].IsRdy((int)PosZ.REJECT_PLACE) == false)
							{
								RunCylUpDown(SenPos.PICKER_DOWN, m_ActPicker);
								arrMt[Axis.Z].MoveIndex((int)PosZ.REJECT_PLACE);
								break;
							}

							m_nCntRejctSend = 0;
							smm.Next(Cmd.PLACE_REJECT_51);
						}
						break;
					case Cmd.PLACE_REJECT_51:
						{
							if (SM.SHM.GetUseSkipData(USESKIP.SendRejectProdId) == true &&
								m_nCntRejctSend < 8)
							{
								int nrow = 0;
								int ncol = 0;
								int nTotalRow = 0;
								int nTotalCol = 0;
								string str_id = string.Empty;

								if (m_ActPicker.Picker[m_nCntRejctSend].PocketNo < 0)
								{
									m_nCntRejctSend++;
									break;
								}

								InfoPart.tsInfo info_p = SM.FMP[FlowPart.ULDPnP].GetInfo();
								InfoTray.tsInfo info_t = SM.FMT[FlowTray.Reject].GetInfo();

								nrow = m_ActPicker.Picker[m_nCntRejctSend].nRow;
								ncol = m_ActPicker.Picker[m_nCntRejctSend].nCol;
								nTotalRow = 2;
								nTotalCol = 25;

								if (info_p.ProdInfo[m_nCntRejctSend].Exist == false)
								{
									m_nCntRejctSend++;
									break;
								}

								str_id = info_p.ProdInfo[m_nCntRejctSend].ProdID;
								string strBarcode = info_p.ProdInfo[m_nCntRejctSend].SerialNumber;
								MESStatus mesResult = (MESStatus)info_p.ProdInfo[m_nCntRejctSend].ResultMES;

								//바코드, 피두셜, MES NG인 경우 컨펌 없이 무조건 NG처리
								if (mesResult == MESStatus.NG || strBarcode.ToLower().Contains("error") == true)
								{
									info_p.ProdInfo[m_nCntRejctSend].VerifyTopDone = true;
									info_p.ProdInfo[m_nCntRejctSend].VerifyBtmDone = true;
									SM.FMP[FlowPart.ULDPnP].SetInfo(info_p);
									mesResult = MESStatus.NG;
								}
								//검사 스킵한 제품은 통신 안함
								if (strBarcode.ToLower().Contains("skiperror") == true)
								{
									m_nCntRejctSend++;
									break;
								}

								G.commInspRef.SendRejectDataReport(str_id, nrow, ncol, nTotalRow, nTotalCol, mesResult);
								smm.Next(Cmd.PLACE_REJECT_52);
								break;
							}

							smm.Next(Cmd.PLACE_REJECT_60);
						}
						break;
					case Cmd.PLACE_REJECT_52:
						{
							bool bConfirm = false;
							int nRt = G.commInspRef.RcvRejectDataReportConfrim(ref bConfirm);

							if (smm.Delayms(NDm.Read(eNDM.RpsCommTimeOut)) == true)
							{
								AddAlarm(ALM.ERROR_SEQ_UNLOAD_PnP_COMM_TIMEOUT);
								smm.ResetDelayms();
								smm.Next(Cmd.PLACE_REJECT_51);
								break;
							}

							if (nRt == 0)
							{
								break;
							}
							else if (nRt < 0)
							{
								smm.Next(Cmd.PLACE_REJECT_51);
								break;
							}
							else if (nRt > 0)
							{
								if (bConfirm == false)
								{
									AddAlarm(ALM.ERROR_SEQ_UNLOAD_PnP_COMM_DATA_WRONG);
									smm.Next(Cmd.PLACE_REJECT_51);
									break;
								}

								m_nCntRejctSend++;

								if (m_nCntRejctSend < SM.FMP[FlowPart.ULDPnP].GetMaxProdCount())
								{
									smm.Next(Cmd.PLACE_REJECT_51);
									break;
								}
							}

							smm.Next(Cmd.PLACE_REJECT_60);
						}
						break;
					case Cmd.PLACE_REJECT_60:
						{
                            //if (CheckPickerEjector(SenPos.BLOW, m_ActPicker) == false)
                            //{
                            //RunPickerEjector(SenPos.BLOW, m_ActPicker);
                            //	break;
                            //}

                            RunPickerEjector(SenPos.BLOW, m_ActPicker);
                            FlowMngMoveToReject(m_ActPicker);

							// UI 생산량 표시 용
							string CurKeyValue = SM.FMP[FlowPart.ULDPnP].GetInfo().KeyValue;
							int[] Yield = new int[3] { 0, 0, 0 }; // ok , ng , user ok

							Yield = SM.SHM.GetLotYield(CurKeyValue);
							Yield[1] += m_ActPicker.CountExist();
							SM.SHM.SetLotYield(CurKeyValue, Yield);

							NDm.Write(eNDM.CurNGProdCount, NDm.Read(eNDM.CurNGProdCount) + m_ActPicker.CountExist());

							m_nCntRejctSend = 0;
							
							smm.Next(Cmd.PLACE_REJECT_100);
						}
						break;
						
					case Cmd.PLACE_REJECT_100:
						{
                            if (smm.Delayms(NDm.Read(eNDM.UnloadPnPVacDelay)) == false) break;

                            if (SM.FMP[FlowPart.ULDPnP].IsResultExist(ProdStatus.NG) < 0)
							{
								if (CheckCylUpDown(SenPos.PICKER_UP) == false ||
									arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false)
								{
									RunCylUpDown(SenPos.PICKER_UP);
									arrMt[Axis.Z].MoveIndex((int)PosZ.READY);
									break;
								}
							}
							else
							{
								if (CheckCylUpDown(SenPos.PICKER_UP) == false)
								{
									RunCylUpDown(SenPos.PICKER_UP);
									break;
								}
							}

							smm.Next(Cmd.PLACE_50);
						}
						break;

					case Cmd.PLACE_UNLOAD_10:
						{
							m_nStartPicker = -1;
							if (m_PlaceMode == PlaceMode.OK)
							{
								for (int i = 0; i < SM.FMP[FlowPart.ULDPnP].GetMaxProdCount(); i++)
								{
									if (SM.FMP[FlowPart.ULDPnP].GetProdStatus(i) == ProdStatus.OK)
									{
										m_nStartPicker = i;
										break;
									}
								}
								smm.Next(Cmd.PLACE_UNLOAD_20);
							}
							else if (m_PlaceMode == PlaceMode.NG)
							{
								for (int i = 0; i < SM.FMP[FlowPart.ULDPnP].GetMaxProdCount(); i++)
								{
									if (m_PlaceMode == PlaceMode.NG &&
										SM.FMP[FlowPart.ULDPnP].GetProdStatus(i) == ProdStatus.NG &&
										SM.FMP[FlowPart.ULDPnP].IsNoVerifyExist(i) == false)
									{
										m_nStartPicker = i;
										break;
									}
								}
								smm.Next(Cmd.PLACE_UNLOAD_20);
							}
							else
							{
								smm.Next(Cmd.END);
							}

							if (m_nStartPicker == -1)
							{
								smm.Next(Cmd.PLACE_30);
								break;
							}
						}
						break;

					case Cmd.PLACE_UNLOAD_20:
						{
							//내려 둘 트레이가 있는지 체크
							if (m_nSeqUsingTable == -1) break;

							int nstart_col = 0;

							if (SearchEmptyTray(m_nSeqUsingTable, m_TrayDirection, ref m_nProdCol, ref m_nProdRow, nstart_col) == false)
							{
								smm.Next(Cmd.END);
								break;
							}

							if (m_PlaceMode == PlaceMode.OK)		smm.Next(Cmd.PLACE_UNLOAD_OK_10);
							else if (m_PlaceMode == PlaceMode.NG)	smm.Next(Cmd.PLACE_UNLOAD_NG_10);
							else									smm.Next(Cmd.END);
						}
						break;

					case Cmd.PLACE_UNLOAD_OK_10:
						{
							
							SearchPlaceTarget(ProdStatus.OK);
							GetPnPPosTable(m_nSeqUsingTable, m_nStartPicker, m_nProdCol, m_nProdRow, ref m_nAbsPosX, ref m_nAbsPosY,false);

							smm.Next(Cmd.PLACE_UNLOAD_30);
						}
						break;

					case Cmd.PLACE_UNLOAD_NG_10:
						{
							int ncnt = 0;
							int ncol = 0;
							int npocket = 0;
							int nstep = SM.SHM.GetRecipeDataToInt(DEVICE.GripStepCnt) / 1000;

							m_ActPicker.ResetAll();

							for (int i = m_nStartPicker; i < SM.FMP[FlowPart.ULDPnP].GetMaxProdCount(); i++)
							{
								if (m_TrayDirection == Dir.Left) ncol = m_nProdCol + ncnt + nstep * ncnt;
								else ncol = m_nProdCol - ncnt - nstep * ncnt;

								npocket = m_nProdRow * m_nCntTrayCol + ncol;

								if (ncol < m_nCntTrayCol &&
									ncol >= 0 &&
									i > m_nStartPicker &&
									ncol > 0)
								{
									if (CheckGapValid(ncol, i % m_PickerMaxCol) == false) break;
								}

								if (ncol < m_nCntTrayCol &&
									ncol >= 0 &&
									SM.FMP[FlowPart.ULDPnP].GetProdExist(i) == true &&
									m_ActPicker.ContainsPocket(npocket) == false &&
									SM.FMP[FlowPart.ULDPnP].IsNoVerifyExist(i) == false &&
									SM.FMT[GetFlow(m_nSeqUsingTable)].GetInfo().ProdInfo[npocket].Exist == false)
								{
									m_ActPicker.Set(i, ncol, m_nProdRow, m_nCntTrayCol);
								}

								ncnt++;
							}

							GetPnPPosTable(m_nSeqUsingTable, m_nStartPicker, m_nProdCol, m_nProdRow, ref m_nAbsPosX, ref m_nAbsPosY,false);

							smm.Next(Cmd.PLACE_UNLOAD_30);
						}
						break;

					case Cmd.PLACE_UNLOAD_30:
						{
							if (arrMt[Axis.X].IsRdyPos(m_nAbsPosX) == false ||
								arrMt[Axis.Y].IsRdyPos(m_nAbsPosY) == false ||
                                arrMt[Axis.W].IsRdy((int)PosW.TABLE) == false)
							{
								arrMt[Axis.X].MoveABS(m_nAbsPosX, (int)PosX.TABLE1_PLACE_FIRST);
                                arrMt[Axis.Y].MoveABS(m_nAbsPosY, (int)PosY.TABLE1_PLACE_FIRST);
                                arrMt[Axis.W].MoveIndex((int)PosW.TABLE);
                                break;
							}

							smm.Next(Cmd.PLACE_UNLOAD_40);
						}
						break;

					case Cmd.PLACE_UNLOAD_40:
						{
							if (CheckCylUpDown(SenPos.PICKER_DOWN, m_ActPicker) == false ||
								arrMt[Axis.Z].IsRdy((int)PosZ.TABLE1_PLACE + m_nSeqUsingTable) == false)
							{
								RunCylUpDown(SenPos.PICKER_DOWN, m_ActPicker);
								arrMt[Axis.Z].MoveIndex((int)PosZ.TABLE1_PLACE + m_nSeqUsingTable);
								break;
							}

							smm.Next(Cmd.PLACE_UNLOAD_50);
						}
						break;

					case Cmd.PLACE_UNLOAD_50:
						{
							//if (CheckPickerEjector(SenPos.BLOW, m_ActPicker) == false)
							//{
							//	RunPickerEjector(SenPos.BLOW, m_ActPicker);
							//	break;
							//}

                            RunPickerEjector(SenPos.BLOW, m_ActPicker);
                            FlowMngMoveToTable(m_nSeqUsingTable, m_ActPicker);
							m_nSeqUseTable = m_nSeqUsingTable;
							// UI 생산량 표시 용
							string CurKeyValue = SM.FMP[FlowPart.ULDPnP].GetInfo().KeyValue;
							int[] Yield = new int[3] { 0, 0, 0 }; // ok , ng , user ok

							Yield = SM.SHM.GetLotYield(CurKeyValue);

							if (m_PlaceMode == PlaceMode.OK)
							{
								int ncnt_ok = 0;
								int ncnt_user_ok = 0;
								for (int i = 0; i < m_ActPicker.Count; i++)
								{
									if (m_ActPicker.GetPocketNo(i) >= 0 && m_ActPicker.Picker[i].bFromReject == true)
									{
										ncnt_user_ok += 1;
									}
									else if (m_ActPicker.GetPocketNo(i) >= 0 && m_ActPicker.Picker[i].bFromReject == false)
									{
										ncnt_ok += 1;
									}
								}

								Yield[0] += ncnt_ok;
								NDm.Write(eNDM.CurOKProdCount, NDm.Read(eNDM.CurOKProdCount) + ncnt_ok);

								Yield[2] += ncnt_user_ok;
								NDm.Write(eNDM.CurUserOKProdCount, NDm.Read(eNDM.CurUserOKProdCount) + ncnt_user_ok);
							}
							else if (m_PlaceMode == PlaceMode.NG)
							{
								Yield[1] += m_ActPicker.CountExist();
								NDm.Write(eNDM.CurNGProdCount, NDm.Read(eNDM.CurNGProdCount) + m_ActPicker.CountExist());
							}

							SM.SHM.SetLotYield(CurKeyValue, Yield);

							
							smm.Next(Cmd.PLACE_UNLOAD_60);
						}
						break;
					case Cmd.PLACE_UNLOAD_60:
						{
                            if (smm.Delayms(NDm.Read(eNDM.UnloadPnPVacDelay)) == false) break;


                            RunCylUpDown(SenPos.PICKER_UP);

							smm.Next(Cmd.PLACE_UNLOAD_70);
						}
						break;

					case Cmd.PLACE_UNLOAD_70:
						{
							if (SM.FMP[FlowPart.ULDPnP].GetProdExist().Contains(true) == false)
							{
								if (CheckCylUpDown(SenPos.PICKER_UP) == false ||
									arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false)
								{
									RunCylUpDown(SenPos.PICKER_UP);
									arrMt[Axis.Z].MoveIndex((int)PosZ.READY);
									break;
								}
							}
							else
							{
								if (CheckCylUpDown(SenPos.PICKER_UP) == false)
								{
									RunCylUpDown(SenPos.PICKER_UP);
									break;
								}
							}

							smm.Next(Cmd.PLACE_50);
						}
						break;

					case Cmd.PLACE_50:
						{
							switch (CheckProdExist())
							{
								case ProdExist.Mismatch:
									AddAlarm(ALM.ERROR_SEQ_UNLOAD_PnP_PROD_MISMATCH);
									break;
								default:
									smm.Next(Cmd.PLACE_30);
									break;
							}
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

		private void PnP(SMM<Cmd> smm)
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

								smm.Next(Cmd.PnP_10);
							}
						}
						break;

					case Cmd.PnP_10:
						{
							int ncnt_ng = 0;

							if (SM.FMP.IsFlow(FlowPart.ULDPnP) == false &&
								SM.FMP.IsFlow(FlowPart.ULDBuffer) == true)
							{
								for (int i = 0; i < SM.FMP[FlowPart.ULDBuffer].GetMaxProdCount(); i++)
								{
									if (SM.FMP[FlowPart.ULDBuffer].GetProdStatus(i) == ProdStatus.NG)
									{
										ncnt_ng++;
									}
								}

								if (ncnt_ng > SM.FMT[FlowTray.Reject].GetEmptyCount() &&
									NDm.Read(eNDM.RejectRunMode) != (int)RejectRunMode.VerifyReq &&
									SM.FMT[FlowTray.Reject].GetVerifyNotCount() != 0)
								{
									NDm.Write(eNDM.RejectRunMode, (int)RejectRunMode.VerifyReq);
									smm.Next(Cmd.END);
									break;
								}
							}

							bool nOkExist = SM.FMT[FlowTray.Reject].IsResultExist(ProdStatus.OK);
							bool nNGExist = SM.FMT[FlowTray.Reject].IsResultExist(ProdStatus.NG);
							bool bVerifyComplete = SM.FMT[FlowTray.Reject].CheckVerifyComplete();

							if (ncnt_ng > SM.FMT[FlowTray.Reject].GetEmptyCount())
							{
								smm.Next(Cmd.END);        // Pick
								break;
								//AddAlarm(ALM.ERROR_SEQ_REJECT_BUF_NONE_EMPTY);
							}
							else if (nOkExist == true && bVerifyComplete == true)
							{
								if (NDm.Read(eNDM.LotEndMode) == (int)LotEndMode.LotEnd) m_bRejectPickOnlyOK = false;
								else m_bRejectPickOnlyOK = true;
								smm.Next(Cmd.PnP_30);     // PickReject
							}
							else if (SM.FMP[FlowPart.ULDPnP].GetInfo().LastPieceOfLot == true)
							{
								smm.Next(Cmd.PnP_40);     // Place
							}
							else if (G.seqUnloadBuffer.IsULDWait() == true)
							{
								if ((SM.FMT[FlowTray.Reject].GetInfo().KeyValue == "" ||
									SM.FMP[FlowPart.ULDBuffer].GetInfo().KeyValue == SM.FMT[FlowTray.Reject].GetInfo().KeyValue) &&
									G.seqRejectBuffer.IsReady == true ||
									G.seqJobTeaching.IsReady == true)
								{
									smm.Next(Cmd.PnP_20);     // Pick
								}
								else
									smm.Next(Cmd.END);
							}
						}
						break;

					case Cmd.PnP_20:
						{
							if (IsPickBufReady() == false) break;

							m_smmPickBuf.eInput = InState.run;

							smm.Next(Cmd.PnP_40);
						}
						break;

					case Cmd.PnP_30:
						{
							if (IsPickRejectReady() == false) break;

							m_smmPickReject.eInput = InState.run;

							smm.Next(Cmd.PnP_40);
						}
						break;

					case Cmd.PnP_40:
						{
							if (IsPickBufReady() == false || IsPickRejectReady() == false) break;

							m_smmPlace.eInput = InState.run;

							smm.Next(Cmd.PnP_50);
						}
						break;

					case Cmd.PnP_50:
						{
							if (IsPlaceReady() == false) break;

							if (SM.SHM.IsRunUPH() == true)
							{
								DateTime dt = DateTime.Now;
								SM.SHM.SetUPHTime(UPHStep.LastInsp, dt);
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

		private void Ready(SMM<Cmd> smm)
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

								smm.Next(Cmd.READY_10);
							}
						}
						break;

					case Cmd.READY_10:
						{
							if (CheckCylUpDown(SenPos.PICKER_UP) == false ||
								arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false)
							{
								RunCylUpDown(SenPos.PICKER_UP);
								arrMt[Axis.Z].MoveIndex((int)PosZ.READY);
								break;
							}

							smm.Next(Cmd.READY_20);
						}
						break;

					case Cmd.READY_20:
						{
							if (arrMt[Axis.X].IsRdy((int)PosX.BUFFER) == false ||
								arrMt[Axis.Y].IsRdy((int)PosY.BUFFER) == false ||
								arrMt[Axis.W].IsRdy((int)PosW.BUFFER) == false)
							{
								arrMt[Axis.X].MoveIndex((int)PosX.BUFFER);
								arrMt[Axis.Y].MoveIndex((int)PosY.BUFFER);
								arrMt[Axis.W].MoveIndex((int)PosW.BUFFER);

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

		private void Sort(SMM<Cmd> smm)
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
								UpdateTrayDir(true);
								smm.Next(Cmd.SORT_10);
							}
						}
						break;

					case Cmd.SORT_10:
						{
							if (SM.FMT.IsFlow(m_SortFlow) == false)
							{
								smm.Next(Cmd.END_NG);
							}
							else if (m_SortFlow == FlowTray.ULDTable1 &&
								G.seqUnloadTrayTable1.IsUpState() == false)
							{
								smm.Next(Cmd.END);
								break;
							}
							else if (m_SortFlow == FlowTray.ULDTable2 &&
								G.seqUnloadTrayTable2.IsUpState() == false)
							{
								smm.Next(Cmd.END);
								break;
							}

							if (SM.FMT[m_SortFlow].IsAlignDone() == false)
							{
								switch (m_SortFlow)
								{
									case FlowTray.ULDTable1:

										if (G.seqUnloadTrayTable1.IsReady == true)
										{
											G.seqUnloadTrayTable1.AlignStart();
										}
										break;
									case FlowTray.ULDTable2:

										if (G.seqUnloadTrayTable2.IsReady == true)
										{
											G.seqUnloadTrayTable2.AlignStart();
										}
										break;
									case FlowTray.Reject:

										if (G.seqRejectBuffer.IsReady == true)
										{
											G.seqRejectBuffer.AlignStart();
										}
										break;
								}
								break;
							}

							switch (CheckProdExist())
							{
								case ProdExist.Exist:
									AddAlarm(ALM.ERROR_SEQ_UNLOAD_PnP_PROD_EXIST);
									smm.Next(Cmd.END_NG);
									break;
								case ProdExist.Mismatch:
									AddAlarm(ALM.ERROR_SEQ_UNLOAD_PnP_PROD_MISMATCH);
									smm.Next(Cmd.END_NG);
									break;
								case ProdExist.None:
									smm.Next(Cmd.SORT_20);
									break;
							}
						}
						break;

					case Cmd.SORT_20:
						{
							// Maaping
							m_nSortMap = new int[SM.FMT[m_SortFlow].GetInfo().MaxCntProdInTray];

							for (int i = 0; i < m_nSortMap.Length; i++)
							{								
								int nindex = SM.SHM.GetOriginToTransIdx(i , SM.FMT[m_SortFlow].GetInfo().ModuleType);
								if (SM.FMT[m_SortFlow].GetInfo().ProdInfo[nindex].Exist == false)		m_nSortMap[i] = 0;
								else																	m_nSortMap[i] = 1;
							}

							smm.Next(Cmd.SORT_30);
						}
						break;

					case Cmd.SORT_30:
						{
							// 마지막 제품 위치 검색
							int[] nmask_end = new int[] { 1, -1 };
							m_nSortMapEnd = -1;
							
							for (int i = m_nSortMap.Length - 1; i >= 0; i--)
							{
								if (m_nSortMap[i] == 1)
								{
									m_nSortMapEnd = i;
								}
							}
							m_nSortMapEnd = m_nSortMap.Length -1;
							if (m_nSortMapEnd < 0)
							{
								smm.Next(Cmd.END_OK);
							}
							else
							{
								smm.Next(Cmd.SORT_40);
							}
						}
						break;

					case Cmd.SORT_40:
						{
							//Place 할 위치와 Pick 할 위치 생성

							int nstrart_place = 0;

							m_ltSortPlace.Clear();
							m_ltSortPick.Clear();
							
							m_nSortMapEnd = m_nSortMap.Length -1;
							for (int i = nstrart_place; i <= m_nSortMapEnd; i++)
							{
								if (m_nSortMap[i] == 0)
								{
									nstrart_place = i + 1;

									for (int j = m_nSortMapEnd; j >= nstrart_place; j--)
									{
										if (m_nSortMap[j] == 1)
										{
											int place = SM.SHM.GetOriginToTransIdx(i, SM.FMT[m_SortFlow].GetInfo().ModuleType);
											int pick =  SM.SHM.GetOriginToTransIdx(j, SM.FMT[m_SortFlow].GetInfo().ModuleType);;

											m_ltSortPlace.Add(place);
											m_ltSortPick.Add(pick);
											m_nSortMapEnd = j - 1;
											break;
										}
									}
								}
							}

							if (m_ltSortPlace.Count == 0 && m_ltSortPlace.Count == 0)
							{
								smm.Next(Cmd.END_OK);
								break;
							}

							m_ltSortPlace.Sort();
							m_ltSortPick.Sort();

							smm.Next(Cmd.SORT_50);
						}
						break;

					case Cmd.SORT_50:
						{

							bool[] bexist = SM.FMP[FlowPart.ULDPnP].GetProdExist();

							for (int i = 0; i < bexist.Length; i++)
							{
								if (bexist[i] == false)
								{
									if (CheckEjector(SenPos.BLOW, i) == false)
									{
										RunEjector(SenPos.BLOW, i);
										break;
									}
								}
							}
							
							if (CheckCylUpDown(SenPos.PICKER_UP) == false  ||
								arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false)
							{
								RunCylUpDown(SenPos.PICKER_UP);
								arrMt[Axis.Z].MoveIndex((int)PosZ.READY);
								break;
							}

							smm.Next(Cmd.SORT_PICK_10);
						}
						break;

					case Cmd.SORT_PICK_10:
						{
							int ncur_x = 0;
							int nindex = 0;
							int ncol_cnt = SM.SHM.GetRecipeDataToInt(DEVICE.TrayColCnt) / 1000;
							bool[] bexist = SM.FMP[FlowPart.ULDPnP].GetProdExist();

							if (m_ltSortPick.Count == 0)
							{
								smm.Next(Cmd.SORT_PLACE_10);
								break;
							}

							m_nStartPicker = -1;
							for (int i = 0; i < SM.FMP[FlowPart.ULDPnP].GetMaxProdCount(); i++)
							{
								if (SM.SHM.IsUsePart(UsePartName.Picker1 + i, UsePartMode.Disable) == true) continue;

								if (bexist[i] == false)
								{
									m_nStartPicker = i;
									break;
								}
							}
							if (m_nStartPicker == -1)
							{
								smm.Next(Cmd.SORT_PLACE_10);
								break;
							}

							nindex = m_ltSortPick.First();
							ncur_x = nindex % ncol_cnt;
							m_nProdRow = nindex / ncol_cnt;

							bool bpick_act = false;
							int ncnt = 0;
							int nmin_col = 0;

							m_ActPicker.ResetAll();
							
							for (int i = m_nStartPicker; i < SM.FMP[FlowPart.ULDPnP].GetMaxProdCount(); i++)
							{
								int nstep = SM.SHM.GetRecipeDataToInt(DEVICE.GripStepCnt) / 1000;
								int nPickCol = ncur_x + ncnt + nstep * ncnt;
								int nPlaceCol = nindex + ncnt + nstep * ncnt;
								if (SM.SHM.IsUsePart(UsePartName.Picker1 + i, UsePartMode.Disable) == true)
								{
									ncnt++;
									continue;
								}
						
								if ((nPickCol) >= ncol_cnt) continue;
								// Pick 할 수 있는지 확인
								if ((nPickCol) < nmin_col) continue;

								// Place 할 수 있는지 확인
								bool brst = false;
								foreach (int n in m_ltSortPlace)
								{
									int ncol = n % ncol_cnt;
									if (ncol >= nmin_col) brst = true;
								}
								if (brst == false) continue;
								//

								if (nPlaceCol < m_nCntTrayCol &&
									i > m_nStartPicker &&
									nPlaceCol > 0)
								{
									if (CheckGapValid(nPlaceCol, i % m_PickerMaxCol) == false) break;
								}

								if (SM.FMP[FlowPart.ULDPnP].GetProdExist(i) == false &&
									m_ltSortPick.Contains(nPlaceCol) == true &&
									m_ActPicker.ContainsPocket(nPlaceCol) == false &&
									SM.FMT[m_SortFlow].GetInfo().ProdInfo[nPlaceCol].Exist == true)
								{
									m_ActPicker.Set(i, nPickCol, m_nProdRow, m_nCntTrayCol);
									bpick_act = true;
								}
								ncnt++;
							}


							if (CheckProdExist() == ProdExist.Exist && bpick_act == false)
							{
								smm.Next(Cmd.SORT_PLACE_10);
								break;
							}

							GetPnPPosTable(m_SeqSortTray, m_nStartPicker, ncur_x, m_nProdRow, ref m_nAbsPosX, ref m_nAbsPosY,true);

							if (SM.FMP.IsFlow(FlowPart.ULDPnP) == false) SM.FMP.Next(FlowPart.Empty, FlowPart.ULDPnP);

							smm.Next(Cmd.SORT_PICK_20);
						}
						break;

					case Cmd.SORT_PICK_20:
						{
							if (arrMt[Axis.X].IsRdyPos(m_nAbsPosX) == false ||
								arrMt[Axis.Y].IsRdyPos(m_nAbsPosY) == false||
								arrMt[Axis.W].IsRdy((int)PosW.TABLE) == false)
							{
								arrMt[Axis.X].MoveABS(m_nAbsPosX, (int)PosX.TABLE1_PICK_FIRST);
								arrMt[Axis.Y].MoveABS(m_nAbsPosY, (int)PosY.TABLE1_PICK_FIRST);
								arrMt[Axis.W].MoveIndex((int)PosW.TABLE);
								break;
							}

							smm.Next(Cmd.SORT_PICK_30);
						}
						break;

					case Cmd.SORT_PICK_30:
						{
							if (arrMt[Axis.Z].IsRdy((int)PosZ.TABLE1_PICK + m_SeqSortTray) == false ||
								CheckCylUpDown(SenPos.PICKER_DOWN, m_ActPicker) == false)
							{
								arrMt[Axis.Z].MoveIndex((int)PosZ.TABLE1_PICK + m_SeqSortTray);
								RunCylUpDown(SenPos.PICKER_DOWN, m_ActPicker);
								break;
							}

							smm.Next(Cmd.SORT_PICK_40);
						}
						break;

					case Cmd.SORT_PICK_40:
                        {
                            //그냥 올린뒤 뒤에서 미스매치 알람 발생
                            if (smm.Delayms(NDm.Read(eNDM.UnloadPnPVacDelay)) == true)
                            {
                                smm.Next(Cmd.SORT_PICK_50);
                                break;
                            }

                            if (CheckPickerEjector(SenPos.VAC, m_ActPicker) == false)
							{
								RunPickerEjector(SenPos.VAC, m_ActPicker);
								break;
							}

							smm.Next(Cmd.SORT_PICK_50);
						}
						break;

					case Cmd.SORT_PICK_50:
						{
							if (CheckCylUpDown(SenPos.PICKER_UP, m_ActPicker) == false)
							{
								RunCylUpDown(SenPos.PICKER_UP, m_ActPicker);
								break;
							}

							smm.Next(Cmd.SORT_PICK_60);
						}
						break;

					case Cmd.SORT_PICK_60:
						{
							if (m_SortFlow == FlowTray.Reject) FlowMngMoveRejectToPicker(m_ActPicker);
							else FlowMngMoveTableToPicker(m_SeqSortTray, m_ActPicker);

							for (int i = 0; i < SM.FMP[FlowPart.ULDPnP].GetMaxProdCount(); i++)
							{
								if (m_ActPicker.GetPocketNo(i) == -1) continue;

								m_ltSortPick.Remove(m_ActPicker.GetPocketNo(i));
							}

							smm.Next(Cmd.SORT_PICK_70);
						}
						break;

					case Cmd.SORT_PICK_70:
						{
							switch (CheckProdExist())
							{
								case ProdExist.Mismatch:
									AddAlarm(ALM.ERROR_SEQ_UNLOAD_PnP_PROD_MISMATCH);
									smm.Next(Cmd.END_NG);
									break;
								default:
									if (m_ltSortPick.Count == 0)		smm.Next(Cmd.SORT_PLACE_10);
									else								smm.Next(Cmd.SORT_PICK_10);

									break;
							}
						}
						break;

					case Cmd.SORT_PLACE_10:
						{
							int ncur_x = 0;
							int nindex = 0;
							int ncol_cnt = SM.SHM.GetRecipeDataToInt(DEVICE.TrayColCnt) / 1000;
							bool[] bexist = SM.FMP[FlowPart.ULDPnP].GetProdExist();

							if (m_ltSortPlace.Count == 0)
							{
								smm.Next(Cmd.SORT_20);
								break;
							}

							m_nStartPicker = -1;
							for (int i = 0; i < bexist.Length; i++)
							{
								if (bexist[i] == true)
								{
									m_nStartPicker = i;
									break;
								}
							}
							if (m_nStartPicker == -1)
							{
								if (m_ltSortPick.Count > 0)
								{
									if (CheckProdExist() == ProdExist.None) SM.FMP.Clear(FlowPart.ULDPnP);
									smm.Next(Cmd.SORT_PICK_10);
								}
								else
								{
									smm.Next(Cmd.SORT_20);
								}
								break;
							}

							nindex = m_ltSortPlace.First();
							ncur_x = nindex % ncol_cnt;
							m_nProdRow = nindex / ncol_cnt;

							//	bool	nplace_act	= false;
							int ncnt = 0;
							int nmin_col = 0;

							m_ActPicker.ResetAll();
							
							for (int i = m_nStartPicker; i < SM.FMP[FlowPart.ULDPnP].GetMaxProdCount(); i++)
							{
								int nstep = SM.SHM.GetRecipeDataToInt(DEVICE.GripStepCnt) / 1000;
								int nPickCol = ncur_x + ncnt + nstep * ncnt;
								int nPlaceCol = nindex + ncnt + nstep * ncnt;

								if ((nPickCol) >= ncol_cnt) continue;
								if ((nPickCol) < nmin_col) continue;

								if (nPlaceCol < m_nCntTrayCol &&
									i > m_nStartPicker &&
									nPlaceCol > 0)
								{
									if (CheckGapValid(nPlaceCol, i % m_PickerMaxCol) == false) break;
								}

								if (bexist[i] == true &&
									m_ltSortPlace.Contains(nPlaceCol) == true &&
									m_ActPicker.ContainsPocket(nPlaceCol) == false &&
									SM.FMT[m_SortFlow].GetInfo().ProdInfo[nPlaceCol].Exist == false)
								{
									m_ActPicker.Set(i, nPickCol, m_nProdRow, m_nCntTrayCol);
								}

								ncnt++;
							}

							GetPnPPosTable(m_SeqSortTray, m_nStartPicker, ncur_x, m_nProdRow, ref m_nAbsPosX, ref m_nAbsPosY,false);

							smm.Next(Cmd.SORT_PLACE_20);
						}
						break;

					case Cmd.SORT_PLACE_20:
						{
							if (arrMt[Axis.X].IsRdyPos(m_nAbsPosX) == false ||
								arrMt[Axis.Y].IsRdyPos(m_nAbsPosY) == false||
								arrMt[Axis.W].IsRdy((int)PosW.TABLE) == false)
							{
								arrMt[Axis.X].MoveABS(m_nAbsPosX, (int)PosX.TABLE1_PLACE_FIRST);
								arrMt[Axis.Y].MoveABS(m_nAbsPosY, (int)PosY.TABLE1_PLACE_FIRST);
								arrMt[Axis.W].MoveIndex((int)PosW.TABLE);
								break;
							}

							smm.Next(Cmd.SORT_PLACE_30);
						}
						break;

					case Cmd.SORT_PLACE_30:
						{
							if (arrMt[Axis.Z].IsRdy((int)PosZ.TABLE1_PLACE + m_SeqSortTray) == false ||
								CheckCylUpDown(SenPos.PICKER_DOWN, m_ActPicker) == false)
							{
								arrMt[Axis.Z].MoveIndex((int)PosZ.TABLE1_PLACE + m_SeqSortTray);
								RunCylUpDown(SenPos.PICKER_DOWN, m_ActPicker);
								break;
							}

							smm.Next(Cmd.SORT_PLACE_40);
						}
						break;

					case Cmd.SORT_PLACE_40:
						{
							//if (CheckPickerEjector(SenPos.BLOW, m_ActPicker) == false)
							//{
							//	RunPickerEjector(SenPos.BLOW, m_ActPicker);
							//	break;
							//}

                            RunPickerEjector(SenPos.BLOW, m_ActPicker);
                            if (m_SortFlow == FlowTray.Reject) FlowMngMoveToReject(m_ActPicker);
							else FlowMngMoveToTable(m_SeqSortTray, m_ActPicker);
							
							smm.Next(Cmd.SORT_PLACE_50);
						}
						break;
						
					case Cmd.SORT_PLACE_50:
						{
                            if (smm.Delayms(NDm.Read(eNDM.UnloadPnPVacDelay)) == false) break;

                            if (CheckCylUpDown(SenPos.PICKER_UP, m_ActPicker) == false)
							{
								RunCylUpDown(SenPos.PICKER_UP, m_ActPicker);
								break;
							}

							smm.Next(Cmd.SORT_PLACE_60);
						}
						break;

					case Cmd.SORT_PLACE_60:
						{
							for (int i = 0; i < SM.FMP[FlowPart.ULDPnP].GetMaxProdCount(); i++)
							{
								if (m_ActPicker.GetPocketNo(i) == -1) continue;

								m_ltSortPlace.Remove(m_ActPicker.GetPocketNo(i));
							}

							smm.Next(Cmd.SORT_PLACE_70);
						}
						break;

					case Cmd.SORT_PLACE_70:
						{
							switch (CheckProdExist())
							{
								case ProdExist.Mismatch:
									AddAlarm(ALM.ERROR_SEQ_UNLOAD_PnP_PROD_MISMATCH);
									smm.Next(Cmd.END_NG);
									break;
								default:
									if (m_ltSortPlace.Count == 0)
									{
										if (CheckProdExist() == ProdExist.None) SM.FMP.Clear(FlowPart.ULDPnP);
										smm.Next(Cmd.SORT_20);
									}
									else
									{
										smm.Next(Cmd.SORT_PLACE_10);
									}
									break;
							}
						}
						break;

					case Cmd.END_OK:
						{
							if (arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false)
							{
								arrMt[Axis.Z].MoveIndex((int)PosZ.READY);
								break;
							}

							if (m_SortFlow != FlowTray.Reject &&
								SM.FMT[m_SortFlow].ProcStatus == ProcStatus.SortRun)
							{
								SM.FMT[m_SortFlow].ProcStatus = ProcStatus.SortDone;
							}

							smm.Next(Cmd.START);
						}
						break;

					case Cmd.END_NG:
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

		private void SortPlace(SMM<Cmd> smm)
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
								UpdateTrayDir(true);
								smm.Next(Cmd.SORT_RE_PLACE_10);
							}
						}
						break;

					case Cmd.SORT_RE_PLACE_10:
						{
							switch (CheckProdExist())
							{
								case ProdExist.Exist:
									smm.Next(Cmd.SORT_RE_PLACE_20);
									break;
								case ProdExist.Mismatch:
									AddAlarm(ALM.ERROR_SEQ_UNLOAD_PnP_PROD_MISMATCH);
									smm.Next(Cmd.END);
									break;
								case ProdExist.None:
									AddAlarm(ALM.ERROR_SEQ_UNLOAD_PnP_PROD_NONE);
									smm.Next(Cmd.END);
									break;
							}
						}
						break;

					case Cmd.SORT_RE_PLACE_20:
						{
							if (SM.FMP.IsFlow(FlowPart.ULDPnP) == false)
							{
								smm.Next(Cmd.END);
								break;
							}

							if (SM.FMT.IsFlow(m_SortFlow) == false)
							{
								smm.Next(Cmd.END);
								break;
							}

							m_nStartPicker = -1;
							for (int i = 0; i < SM.FMP[FlowPart.ULDPnP].GetMaxProdCount(); i++)
							{
								if (SM.FMP[FlowPart.ULDPnP].GetProdExist(i) == true)
								{
									m_nStartPicker = i;
									break;
								}
							}
							if (m_nStartPicker == -1)
							{
								smm.Next(Cmd.END);
								break;
							}

							if (m_SortFlow == FlowTray.ULDTable1 || m_SortFlow == FlowTray.ULDTable2)
							{
								if (SearchEmptyTray(m_SeqSortTray, m_TrayDirection, ref m_nProdCol, ref m_nProdRow) == false)
								{
									smm.Next(Cmd.END);
									break;
								}

							}
							else
							{
								if (SearchEmptyReject(Dir.Left, ref m_nProdCol, ref m_nProdRow) == false)
								{
									smm.Next(Cmd.END);
									break;
								}
							}


							smm.Next(Cmd.SORT_RE_PLACE_30);
						}
						break;

					case Cmd.SORT_RE_PLACE_30:
						{
							SearchPlaceTarget();
							GetPnPPosTable(m_SeqSortTray, m_nStartPicker, m_nProdCol, m_nProdRow, ref m_nAbsPosX, ref m_nAbsPosY,false);

							smm.Next(Cmd.SORT_RE_PLACE_40);
						}
						break;

					case Cmd.SORT_RE_PLACE_40:
						{
							if (arrMt[Axis.X].IsRdyPos(m_nAbsPosX) == false ||
								arrMt[Axis.Y].IsRdyPos(m_nAbsPosY) == false||
								arrMt[Axis.W].IsRdy((int)PosW.TABLE) == false)
							{
								arrMt[Axis.X].MoveABS(m_nAbsPosX, (int)PosX.REJECT_PLACE_FIRST);
								arrMt[Axis.Y].MoveABS(m_nAbsPosY, (int)PosY.REJECT_PLACE_FIRST);
								arrMt[Axis.W].MoveIndex((int)PosW.TABLE);
								break;
							}
							

							smm.Next(Cmd.SORT_RE_PLACE_50);
						}
						break;

					case Cmd.SORT_RE_PLACE_50:
						{
							if (CheckCylUpDown(SenPos.PICKER_DOWN, m_ActPicker) == false ||
								arrMt[Axis.Z].IsRdy((int)PosZ.REJECT_PLACE) == false)
							{
								RunCylUpDown(SenPos.PICKER_DOWN, m_ActPicker);
								arrMt[Axis.Z].MoveIndex((int)PosZ.REJECT_PLACE);
								break;
							}

							smm.Next(Cmd.SORT_RE_PLACE_60);
						}
						break;

					case Cmd.SORT_RE_PLACE_60:
						{
							//if (CheckPickerEjector(SenPos.BLOW, m_ActPicker) == false)
							//{
							//	RunPickerEjector(SenPos.BLOW, m_ActPicker);
							//	break;
							//}

                            RunPickerEjector(SenPos.BLOW, m_ActPicker);
                            if (m_SortFlow == FlowTray.Reject) FlowMngMoveToReject(m_ActPicker);
							else FlowMngMoveToTable(m_SeqSortTray, m_ActPicker);
							
							smm.Next(Cmd.SORT_RE_PLACE_70);
						}
						break;

					case Cmd.SORT_RE_PLACE_70:
						{
                            if (smm.Delayms(NDm.Read(eNDM.UnloadPnPVacDelay)) == false) break;


							if (CheckCylUpDown(SenPos.PICKER_UP) == false ||
								arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false)
							{
								RunCylUpDown(SenPos.PICKER_UP);
								arrMt[Axis.Z].MoveIndex((int)PosZ.READY);
								break;
							}
							smm.Next(Cmd.SORT_RE_PLACE_80);
						}
						break;
					case Cmd.SORT_RE_PLACE_80:
						{
							switch (CheckProdExist())
							{
								case ProdExist.Mismatch:
									AddAlarm(ALM.ERROR_SEQ_UNLOAD_PnP_PROD_MISMATCH);
									break;
								default:
									smm.Next(Cmd.SORT_RE_PLACE_20);
									break;
							}

							if (CheckProdExist() == ProdExist.None) SM.FMP.Clear(FlowPart.ULDPnP);
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
	#endregion
}