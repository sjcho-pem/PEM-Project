using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_MarsSSD
{
	public class SeqUnloadTrayTable : SeqBase<SeqUnloadTrayTable.Act, SeqUnloadTrayTable.Axis, SeqUnloadTrayTable.Cmd>
	{
		#region [ Enum ]
		public enum Act
		{
			TablePad,
			TrayGripX,
            AirVib,
			Max,
		}

		public enum Axis
		{
			GripY,
			UpDnZ,
			Max,
		}
		
		public enum PosGrip
		{
			RELEASE,
			GRIP,
		}

		public enum PosZ
		{
			UP,
			DOWN,
			SAFETY,
		}

		public enum Cmd
		{
			START	= 10,
			END		= 99,

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

			UP_UP,
			UP_10,
			UP_20,
			UP_30,
			UP_40,
			UP_50,
			UP_60,
			UP_70,
			UP_80,
			UP_90,
			UP_100,

            DOWN_VIBRATE1,
            DOWN_VIBRATE2,
            DOWN_DOWN,
			DOWN_10,
			DOWN_20,
			DOWN_30,
			DOWN_40,
			DOWN_50,
			DOWN_60,
			DOWN_70,
			DOWN_80,
			DOWN_90,
			DOWN_100,

			GRIP_10,
			GRIP_20,
			GRIP_30,
			GRIP_40,
			GRIP_50,
			GRIP_60,
			GRIP_70,
			GRIP_80,
			GRIP_90,
			GRIP_100,

			RELEASE_10,
			RELEASE_20,
			RELEASE_30,
			RELEASE_40,
			RELEASE_50,
			RELEASE_60,
			RELEASE_70,
			RELEASE_80,
			RELEASE_90,
			RELEASE_100,

			ALIGN_10,
			ALIGN_20,
		}

		public	enum UDMode
		{
			Load,
			Unload,
			Manual,
		}
		#endregion


		#region [Variables]
		private SMM<Cmd>	m_smmHome;
		private SMM<Cmd>	m_smmUp;
		private SMM<Cmd>	m_smmDown;
		private SMM<Cmd>	m_smmGrip;
		private SMM<Cmd>	m_smmRelease;
		private SMM<Cmd> m_smmAlign;

		public	Cmd	smmHome			{ get => m_smmHome.CurStep; }
		public	Cmd	smmUp			{ get => m_smmUp.CurStep; }
		public	Cmd	smmDown			{ get => m_smmDown.CurStep; }
		public	Cmd	smmGrip			{ get => m_smmGrip.CurStep; }
		public	Cmd	smmRelease		{ get => m_smmRelease.CurStep; }
		public Cmd smmAlign { get => m_smmAlign.CurStep; }

		private	UDMode	m_udMode;

		private int		m_nTableNo			= 0;
		private int		m_nOffsetActAlarm	= 0;
		private int		m_nOffsetSeqAlarm	= 0;
		private bool	m_IsMtHome		= false;
		private bool	m_bManual		= false;

		#region 재정의
		private	DI	DI_PadFwdCyl		= DI.ULDTrayTable1PadFwdCyl;
		private	DI	DI_PadBwdCyl		= DI.ULDTrayTable1PadBwdCyl;
		private	DI	DI_GripXOpenCyl		= DI.ULDTrayTable1GripOpenCyl;
		private	DI	DI_ExistSen			= DI.ULDTrayTable1ExistSen;

		private	DO	DO_PadCyl			= DO.ULDTrayTable1PadCyl;
		private	DO	DO_GripXCyl			= DO.ULDTrayTable1GripCyl;
        private DO  DO_AirVib           = DO.ULDTrayTable1AirVib;

        private MT  MT_GripY			= MT.ULDTable1GripY;
		private MT  MT_Z				= MT.ULDTable1Z;

		private	FlowTray	FlowTable	= FlowTray.ULDTable1;

		private	void	IODefine(int nNo)
		{
			if (nNo == 1)
			{
				DI_PadFwdCyl		= DI.ULDTrayTable2PadFwdCyl;
				DI_PadBwdCyl		= DI.ULDTrayTable2PadBwdCyl;
				DI_GripXOpenCyl		= DI.ULDTrayTable2GripOpenCyl;
				DI_ExistSen			= DI.ULDTrayTable2ExistSen;
				DO_PadCyl			= DO.ULDTrayTable2PadCyl;
				DO_GripXCyl			= DO.ULDTrayTable2GripCyl;
                DO_AirVib           = DO.ULDTrayTable2AirVib;
                MT_GripY			= MT.ULDTable2GripY;
				MT_Z				= MT.ULDTable2Z;
				FlowTable			= FlowTray.ULDTable2;
			}
		}
		#endregion

		private	FlowTray	m_OtherTray
		{
			get
			{
				if (FlowTable == FlowTray.ULDTable1)			return FlowTray.ULDTable2;
				else											return FlowTray.ULDTable1;
			}
		}
		#endregion

		public	void	Init(int nTableNo)
		{
			try
			{
				m_nTableNo = nTableNo;

				m_smmHome		= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.UNLOADTABLE1_HOME+m_nTableNo));
				m_smmUp			= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.UNLOADTABLE1_UP+m_nTableNo));
				m_smmDown		= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.UNLOADTABLE1_DOWN+m_nTableNo));
				m_smmGrip		= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.UNLOADTABLE1_GRIP+m_nTableNo));
				m_smmRelease	= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.UNLOADTABLE1_RELEASE+m_nTableNo));
				m_smmAlign		= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.UNLOADTABLE1_ALIGN + m_nTableNo));

				lstSMM.Add(m_smmHome);
				lstSMM.Add(m_smmUp);
				lstSMM.Add(m_smmDown);
				lstSMM.Add(m_smmGrip);
				lstSMM.Add(m_smmRelease);
				lstSMM.Add(m_smmAlign);

				IODefine(nTableNo);
				m_nOffsetActAlarm	= m_nTableNo * (ALM.ERROR_UNLOAD_TABLE2_GRIP_CYL_RELEASE - ALM.ERROR_UNLOAD_TABLE1_GRIP_CYL_RELEASE);
				m_nOffsetSeqAlarm	= m_nTableNo * (ALM.ERROR_SEQ_UNLOAD_TRAY_TABLE2_TRAY_EXIST - ALM.ERROR_SEQ_UNLOAD_TRAY_TABLE1_TRAY_EXIST);

				#region - Motor
				arrMt = new EnumArray<Axis, CMotor>(Axis.Max);
				arrMt[Axis.GripY] = G.cKernel.Motor(MT_GripY);
				arrMt[Axis.UpDnZ] = G.cKernel.Motor(MT_Z);
				#endregion

				#region - Actuator
				arrAct = new EnumArray<Act, Actuator>(Act.Max);

				arrAct[Act.TablePad] = new CSingleSen();
				arrAct[Act.TablePad].SetSenser1(new List<DI>(new DI[] { DI_PadFwdCyl }));
				arrAct[Act.TablePad].SetSenser2(new List<DI>(new DI[] { DI_PadBwdCyl }));
				arrAct[Act.TablePad].SetSol(DO_PadCyl);
				arrAct[Act.TablePad].Setting(ALM.ERROR_UNLOAD_TABLE1_PAD_CYL_FORWARD + m_nOffsetActAlarm, ALM.ERROR_UNLOAD_TABLE1_PAD_CYL_BACKWARD + m_nOffsetActAlarm, ErrSenPos.Ori);


				arrAct[Act.TrayGripX] = new CSingleSen();
				arrAct[Act.TrayGripX].SetSenser1(new List<DI>(new DI[] { }));
				arrAct[Act.TrayGripX].SetSenser2(new List<DI>(new DI[] { DI_GripXOpenCyl }));
				arrAct[Act.TrayGripX].SetSol(DO_GripXCyl);
				arrAct[Act.TrayGripX].Setting(ALM.ERROR_UNLOAD_TABLE1_GRIP_CYL_GRIP, ALM.ERROR_UNLOAD_TABLE1_GRIP_CYL_RELEASE + m_nOffsetActAlarm, ErrSenPos.Ori);

                arrAct[Act.AirVib] = new CSingleNoSen();
                arrAct[Act.AirVib].SetSol(DO_AirVib);
                arrAct[Act.AirVib].Setting(ALM.NOT_ALARM, ALM.NOT_ALARM);

                #endregion

                base.Initialize();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public	bool	HomeStart(bool IsMtHome = true)			{ m_IsMtHome = IsMtHome; return SetSeqRun(ref m_smmHome); }

		public	bool	UpStart(UDMode Mode)				{ m_udMode	= Mode;	return SetSeqRun(ref m_smmUp); }

		public	bool	DownStart(UDMode Mode)			{ m_udMode	= Mode;	return SetSeqRun(ref m_smmDown); }

		public	bool	GripStart()							{ return SetSeqRun(ref m_smmGrip); }
		
		public	bool	ReleaseStart(bool bManual = false)  {m_bManual = bManual; m_IsMtHome = false;  return SetSeqRun(ref m_smmRelease); }

		public bool AlignStart() { return SetSeqRun(ref m_smmAlign); }

		public	bool	IsHomeReady()						{ return m_smmHome.IsReady; }

		public	bool	IsUpReady()							{ return m_smmUp.IsReady; }

		public	bool	IsDownReady()						{ return m_smmDown.IsReady; }

		public	bool	IsGripReady()						{ return m_smmGrip.IsReady; }

		public	bool	IsReleaseReady()					{ return m_smmRelease.IsReady; }

		public bool IsAlignReady() { return m_smmAlign.IsReady; }

		public	bool	IsGrip()
		{
			try
			{
				if (arrAct[Act.TrayGripX].IsPos(SenPos.FWD) == false)														return false;
				if (arrMt[Axis.GripY].IsRdyPos((int)arrMt[Axis.GripY].GetIndexPos((int)PosGrip.GRIP)) == false)		return false;

				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public	bool	IsRelease()
		{
			try
			{
				if (arrMt[Axis.GripY].IsRdyPos((int)arrMt[Axis.GripY].GetIndexPos((int)PosGrip.RELEASE)) == false)		return false;
				if (arrAct[Act.TrayGripX].IsPos(SenPos.BWD) == false)															return false;

				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public	bool	IsLoadEnable()
		{
			try
			{
				if (this.IsReady == false)																						return SM.SHM.InterlockMsgAdd("this.IsReady == false");
				if (arrAct[Act.TablePad].IsPos(SenPos.BWD) == false)															return SM.SHM.InterlockMsgAdd("arrAct[teAct.TablePad].IsPos(SenPos.BWD) == false");
				if (arrAct[Act.TrayGripX].IsPos(SenPos.BWD) == false)															return SM.SHM.InterlockMsgAdd("arrAct[teAct.TrayGripX].IsPos(SenPos.BWD) == false");
				if (arrMt[Axis.GripY].IsRdyPos((int)arrMt[Axis.GripY].GetIndexPos((int)PosGrip.RELEASE)) == false)		return SM.SHM.InterlockMsgAdd("arrMt[teAxis.GripY].IsRdyPos(RELEASE) == false");;
				if (arrMt[Axis.UpDnZ].IsRdyPos((int)arrMt[Axis.UpDnZ].GetIndexPos((int)PosZ.DOWN)) == false)				return SM.SHM.InterlockMsgAdd("arrMt[teAxis.GripY].IsRdyPos(RELEASE) == false");;
				
				if (G.cKernel.IsBIn(DI_ExistSen) == true)
				{
					AddAlarm(ALM.ERROR_SEQ_UNLOAD_TRAY_TABLE1_TRAY_EXIST + m_nOffsetSeqAlarm);
					return SM.SHM.InterlockMsgAdd("G.cKernel.IsAIn(DI_ExistSen) == true");
				}
				if (SM.FMT.IsFlow(FlowTable) == true)			return SM.SHM.InterlockMsgAdd("SM.FMT.IsFlow(FlowTable) == true");

				return SM.SHM.InterlockMsgReset();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public	bool	IsUnloadEnable()
		{
			try
			{
				if (this.IsReady == false)																					return SM.SHM.InterlockMsgAdd("this.IsReady == false");
				if (arrAct[Act.TablePad].IsPos(SenPos.BWD) == false)														return SM.SHM.InterlockMsgAdd("arrAct[teAct.TablePad].IsPos(SenPos.BWD) == false");
				if (arrAct[Act.TrayGripX].IsPos(SenPos.FWD) == false)														return SM.SHM.InterlockMsgAdd("arrAct[teAct.TrayGrip2].IsPos(SenPos.FWD) == false");
				if (arrMt[Axis.GripY].IsRdyPos((int)arrMt[Axis.GripY].GetIndexPos((int)PosGrip.GRIP)) == false)		return SM.SHM.InterlockMsgAdd("arrMt[teAxis.GripY].IsRdyPos(GRIP) == false");;
				if (arrMt[Axis.UpDnZ].IsRdyPos((int)arrMt[Axis.UpDnZ].GetIndexPos((int)PosZ.DOWN)) == false)				return SM.SHM.InterlockMsgAdd("arrMt[teAxis.GripY].IsRdyPos(RELEASE) == false");;
				
				if (G.DryRunIsBIn(DI_ExistSen, true) == false)
				{
					AddAlarm(ALM.ERROR_SEQ_UNLOAD_TRAY_TABLE2_TRAY_NONE + m_nOffsetSeqAlarm);
					return SM.SHM.InterlockMsgAdd("G.DryRunIsAIn(DI_ExistSen, true) == false");
				}
				if (SM.FMT.IsFlow(FlowTable) == false)			return SM.SHM.InterlockMsgAdd("SM.FMT.IsFlow(FlowTable) == false");
				
				return SM.SHM.InterlockMsgReset();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public	bool	IsWaitEnable()
		{
			try
			{
				if (arrAct[Act.TablePad].IsPos(SenPos.FWD) == false)														return SM.SHM.InterlockMsgAdd("arrAct[teAct.TablePad].IsPos(SenPos.FWD) == false");
				if (arrMt[Axis.GripY].IsRdyPos((int)arrMt[Axis.GripY].GetIndexPos((int)PosGrip.GRIP)) == false)		return SM.SHM.InterlockMsgAdd("arrMt[teAxis.GripY].IsRdyPos(GRIP) == false");;
				if (arrMt[Axis.UpDnZ].IsRdyPos((int)arrMt[Axis.UpDnZ].GetIndexPos((int)PosZ.UP)) == false)				return SM.SHM.InterlockMsgAdd("arrMt[teAxis.GripY].IsRdyPos(RELEASE) == false");;
				
				if (arrAct[Act.TrayGripX].IsPos(SenPos.FWD) == false)														return SM.SHM.InterlockMsgAdd("arrAct[teAct.TrayGrip2].IsPos(SenPos.FWD) == false");
				
				if (G.DryRunIsBIn(DI_ExistSen, true) == false)
				{
					AddAlarm(ALM.ERROR_SEQ_UNLOAD_TRAY_TABLE2_TRAY_NONE + m_nOffsetSeqAlarm);
					return SM.SHM.InterlockMsgAdd("G.DryRunIsAIn(DI_ExistSen, true) == false");
				}
				if (SM.FMT.IsFlow(FlowTable) == false)									return SM.SHM.InterlockMsgAdd("SM.FMT.IsFlow(FlowTable) == false");
				if (SM.FMT[FlowTable].ProcStatus == ProcStatus.TrayFillDone)			return SM.SHM.InterlockMsgAdd("SM.FMT[FlowTable].ProcStatus == teProcStatus.TrayFillDone");

				return SM.SHM.InterlockMsgReset();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public bool		IsUpState()
		{
			try
			{
				if (arrAct[Act.TablePad].IsPos(SenPos.FWD) == false)			return SM.SHM.InterlockMsgAdd("arrAct[teAct.TablePad].IsPos(SenPos.FWD) == false");
				if (arrMt[Axis.UpDnZ].IsRdyPos((int)arrMt[Axis.UpDnZ].GetIndexPos((int)PosZ.UP)) == false)				return SM.SHM.InterlockMsgAdd("arrMt[teAxis.GripY].IsRdyPos(RELEASE) == false");;
				
				return SM.SHM.InterlockMsgReset();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public	bool	IsMotorStats(PosZ ePos)
		{
			try
			{
				if (arrMt[Axis.UpDnZ].IsRdy() == false)				return false;
				if (m_smmUp.IsReady == false)						return false;
				if (m_smmDown.IsReady ==false)						return false;

				if (arrMt[Axis.UpDnZ].IsRdyPos((int)arrMt[Axis.UpDnZ].GetIndexPos((int)ePos)) == false)			return false;

				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public	bool	CheckNeedSort(FlowTray SortFlow)
		{
			try
			{
				if (SM.FMT.IsFlow(SortFlow) == false)			return false;
				
				List<int>	lt_sort_place	= new List<int>();
				List<int>	lt_sort_pick	= new List<int>();
				int[]		nsort_map		= new int[SM.FMT[SortFlow].GetInfo().MaxCntProdInTray];
				int[]		nmask_end		= new int[] { 1, -1 };
				int			nsort_map_end	= -1;
				int			nstrart_place	= 0;

				// Maaping
				for (int i = 0; i < nsort_map.Length; i++)
				{
					if (SM.FMT[SortFlow].GetInfo().ProdInfo[SM.SHM.GetOriginToTransIdx(i, SM.FMT[SortFlow].GetInfo().ModuleType)].Exist == false)		nsort_map[i]	=  0;
					else																																	nsort_map[i]	=  1;
				}

				// 마지막 제품 위치 검색
				for (int i = nsort_map.Length-1; i >= 0; i--)
				{
					if (nsort_map[i] == 1)
					{
						nsort_map_end	= i;
						break;
					}
				}
				
				if (nsort_map_end < 0)			return false;

				//Place 할 위치와 Pick 할 위치 생성
				for (int i = nstrart_place; i <= nsort_map_end; i++)
				{
					if (nsort_map[i] == 0)
					{
						nstrart_place	= i+1;

						for (int j = nsort_map_end; j >= nstrart_place; j--)
						{
							if (nsort_map[j] == 1)
							{
								lt_sort_place.Add(i);
								lt_sort_pick.Add(j);
								nsort_map_end	= j-1;
								break;
							}
						}
					}
				}

				if (lt_sort_place.Count == 0  &&  lt_sort_pick.Count == 0)			return false;

				return true;
			}
			catch(Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return true;
			}
		}
		
		private	bool	CheckTrayPlaceEnd(FlowTray flow)
		{
			try
			{
				if (SM.FMT[flow].ProcStatus == ProcStatus.TrayFillDone  ||
					SM.FMT[flow].ProcStatus == ProcStatus.SortRun  ||
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
		
		public bool IsBiggerThanSafetyDown()
		{
			try
			{
				return arrMt[Axis.UpDnZ].IsBiggerThanOrEqual((int)PosZ.SAFETY) ;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}
		private	bool	SetSeqRun(ref SMM<Cmd> smm)
		{
			try
			{
				if (this.IsReady == false)			return false;

				smm.eInput = InState.run;
				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}



		private	void	SetTrayFillDone(FlowTray Flow)
		{
			try
			{
                //seqUnloadPnP상태를 안보면 자재 체크중에 filldone 처리 후 언로드 해버림
                if (SM.FMT.IsFlow(Flow) == true && G.seqUnloadPnP.CheckCylUpDown(SenPos.PICKER_UP) == true)
				{	
					if (SM.FMT[Flow].ProcStatus == ProcStatus.SortDone)
					{
						SM.FMT[Flow].ProcStatus = ProcStatus.TrayFillDone;

						NDm.Write(eNDM.FillDoneRemainCount, NDm.Read(eNDM.FillDoneRemainCount) - 1);
						if (NDm.Read(eNDM.FillDoneRemainCount) == 0 )
						{
							SM.FMT[Flow].SetLastOfLot(true);
							if (SM.FMT.IsFlow(FlowTray.Reject) == true)
							{
								var t_info =	SM.FMT[FlowTray.Reject].GetInfo();
								t_info.KeyValue = "";
								SM.FMT[FlowTray.Reject].SetInfo(t_info);
							}
							//NDm.Write(eNDM.AutoRunMode, (int)teAutoRunMode.Normal);
							GLog.Add(string.Format($"{Flow}:SetLastTray"));
						}

						GLog.Add(string.Format($"{Flow}:FillDone - Tray Remain Count : {NDm.Read(eNDM.FillDoneRemainCount)}"));
						return;
					}

					if (SM.FMT[Flow].ProcStatus == ProcStatus.TrayFillDone)	return;

					if (SM.FMT[Flow].IsFull() == true)
					{
						if (CheckNeedSort(Flow) == true)			SM.FMT[Flow].ProcStatus = ProcStatus.SortRun;
						else										SM.FMT[Flow].ProcStatus = ProcStatus.SortDone;

						return;
					}

					//Reject으로 너무 많은 트레이가 빠진 경우 예외처리
					if (NDm.Read(eNDM.AutoRunMode) == (int)AutoRunMode.Normal  &&
						SM.FMT[Flow].IsEmpty() == true  &&
						SM.FMT.IsFlow(FlowTray.TrayClean) == true &&
						(SM.FMT.IsFlow(FlowTray.ULDTable1) == true  && SM.FMT[FlowTray.ULDTable1].GetEmptyCount() > 4)  &&
						(SM.FMT.IsFlow(FlowTray.ULDTable2) == true && SM.FMT[FlowTray.ULDTable2].GetEmptyCount() > 4) &&
						 SM.SHM.GetLotYield(SDm.Read(eSDM.CurKeyValue))[1] > SM.FMT[Flow].GetInfo().MaxCntProdInTray  &&
						((SM.FMT[FlowTray.ULDTable1].IsEmpty() == false  &&  SM.FMT[FlowTray.ULDTable2].IsEmpty() == false) == false))
					{
						if (SM.FMT.IsFlow(FlowTray.LDTable1) == true  &&  SM.FMT[FlowTray.LDTable1].IsEmpty() == true &&
							SM.FMT.IsFlow(FlowTray.LDTable2) == true  &&  SM.FMT[FlowTray.LDTable2].GetExistCount() < 4)
						{
							//check 나중에 지워야함
							InfoTray.tsInfo tempInfo =  SM.FMT[Flow].GetInfo();
							int nNgCount = SM.SHM.GetLotYield(SDm.Read(eSDM.CurKeyValue))[1];
							int nOKCount = SM.SHM.GetLotYield(SDm.Read(eSDM.CurKeyValue))[0];
							string Total	 = SM.SHM.GetRunLotInfo(SDm.Read(eSDM.CurKeyValue)).ProdCount;
							UI.Logs.Write(LOGPART.WARNING,  $"MaxCntProdInLot : {Total},CurOk : {nOKCount}, CurNG : {nNgCount} , MaxCntProdInTray : {SM.FMT[Flow].GetInfo().MaxCntProdInTray}");
							G.AlmMng.Del(ALM.WARNING_SEQ_EMPTY_TRAY_UNLOAD);

							SM.FMT[Flow].ProcStatus = ProcStatus.SortDone;
							AddAlarm(ALM.WARNING_SEQ_EMPTY_TRAY_UNLOAD);
						}
						
						if (SM.FMT.IsFlow(FlowTray.LDTable2) == true  &&  SM.FMT[FlowTray.LDTable2].IsEmpty() == true &&
							SM.FMT.IsFlow(FlowTray.LDTable1) == true  &&  SM.FMT[FlowTray.LDTable1].GetExistCount() < 4)
						{
							InfoTray.tsInfo tempInfo =  SM.FMT[Flow].GetInfo();
							int nNgCount = SM.SHM.GetLotYield(SDm.Read(eSDM.CurKeyValue))[1];
							int nOKCount = SM.SHM.GetLotYield(SDm.Read(eSDM.CurKeyValue))[0];
							string Total	 = SM.SHM.GetRunLotInfo(SDm.Read(eSDM.CurKeyValue)).ProdCount;
							UI.Logs.Write(LOGPART.WARNING,  $"MaxCntProdInLot : {Total},CurOk : {nOKCount}, CurNG : {nNgCount} , MaxCntProdInTray : {SM.FMT[Flow].GetInfo().MaxCntProdInTray}");
							G.AlmMng.Del(ALM.WARNING_SEQ_EMPTY_TRAY_UNLOAD);

							SM.FMT[Flow].ProcStatus = ProcStatus.SortDone;
							AddAlarm(ALM.WARNING_SEQ_EMPTY_TRAY_UNLOAD);
						}
					}

					FlowTray OtherFlow = Flow == FlowTray.ULDTable1 ? FlowTray.ULDTable2 : FlowTray.ULDTable1;
					if (SM.FMT.IsFlow(OtherFlow) == true &&
						SM.FMT[OtherFlow].GetExistCount() > 0 &&
						SM.FMT[Flow].GetExistCount() == 0&&
						SM.FMT[Flow].GetInfo().KeyValue == SM.FMT[OtherFlow].GetInfo().KeyValue)
					{
						return;		//자재가 있는 쪽 먼저 Filldone처리
					}
					
					//더이상 같은 키밸류가 없으면 
					if (SM.FMT[FlowTray.Reject].GetVerifyNotCount() == 0  &&
						SM.FMT[FlowTray.Reject].GetVerifyCount(ProdStatus.OK) == 0 &&
						IsExistSameKeyValue() == false)
					{
						SeqUnloadTransfer.PosX eTargetTray = G.seqMain.CheckPriorityUnloadTray();
						if (eTargetTray == SeqUnloadTransfer.PosX.UNLOADTABLE1  &&  Flow == FlowTray.ULDTable2)
						{
							return;
						}
						if (eTargetTray == SeqUnloadTransfer.PosX.UNLOADTABLE2  &&  Flow == FlowTray.ULDTable1)
						{
							return;
						}

						if (CheckNeedSort(Flow) == true)			SM.FMT[Flow].ProcStatus = ProcStatus.SortRun;
						else										SM.FMT[Flow].ProcStatus = ProcStatus.SortDone;
						//GLog.Add($"{Flow} > ProcStatus: {SM.FMT[Flow].ProcStatus} , KeyValue : {SM.FMT[Flow].GetInfo().KeyValue} , Sdm : {SDm.Read(eSDM.CurKeyValue)}, AutorunMode : {(teAutoRunMode)NDm.Read(eNDM.AutoRunMode)}");

						return;
					}
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private bool IsExistSameKeyValue()
		{
			try
			{
				//if (NDm.Read(eNDM.AutoRunMode) == (int)teAutoRunMode.Bypass)		return false;

				//다음 랏 정보가 갱신되지 않은 경우 더 이상 안 내보냄
				if (SDm.Read(eSDM.CurTempKeyValue) != "NONE")						return true;
				
				//마지막트레이가 나갔으면 더이상 안내보냄
				if (NDm.Read(eNDM.FillDoneRemainCount) == 0 )						return true;
				
				string strKeyValue = "";
				if (NDm.Read(eNDM.LotEndMode) == (int)LotEndMode.LotEnd)		strKeyValue = SDm.Read(eSDM.CurKeyValue);
				else															strKeyValue = SM.FMT[FlowTable].GetInfo().KeyValue;

				for (int i = (int)FlowPart.LDPnP; i <  (int)FlowPart.Max; i++)
				{
					if (SM.FMP[(FlowPart)i].GetInfo().KeyValue == strKeyValue)		return true;
				}
				if (NDm.Read(eNDM.AutoRunMode) != (int)AutoRunMode.QuitLot)
				{
					if (SM.FMT[FlowTray.LDTable1].GetInfo().KeyValue == strKeyValue  &&
						SM.FMT[FlowTray.LDTable1].GetExistCount() > 0 )					return true;
					if (SM.FMT[FlowTray.LDTable2].GetInfo().KeyValue == strKeyValue  &&
						SM.FMT[FlowTray.LDTable2].GetExistCount() > 0)					return true;
				}
				if (SM.FMT[FlowTray.LDTrans].GetInfo().KeyValue == strKeyValue  &&
					SM.FMT[FlowTray.LDTrans].GetExistCount() > 0)					return true;

				return false;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}


		public	void	Run()
		{
			try
			{
				UpdateAct(Seq.UnloadTrayTable1+m_nTableNo);

				if (SM.SHM.IsBitData(BIT.Error) == true  ||  SM.SHM.IsBitData(BIT.Pause) == true)			return;

				SetTrayFillDone(FlowTable);

				Home(m_smmHome);
				Up(m_smmUp);
				Down(m_smmDown);
				Grip(m_smmGrip);
				Release(m_smmRelease);
				Align(m_smmAlign);

				SM.SHM.SetSeqStatus(lstSMM);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}



		private	void	Home(SMM<Cmd> smm)
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
							if (G.seqUnloadTransfer.UnloadTableUpDnSafety(m_nTableNo) == false  ||
								G.seqLoadTransfer.UnloadTableUpDnSafety() == false)
							{
								break;
							}

							if (SM.FMT.IsFlow(FlowTable) != G.DryRunIsBIn(DI_ExistSen, SM.FMT.IsFlow(FlowTable)))
							{
								AddAlarm(ALM.ERROR_SEQ_UNLOAD_TRAY_TABLE1_TRAY_EXIST + m_nOffsetSeqAlarm);
							}
							

							smm.Next(Cmd.HOME_20);
						}
						break;

					case Cmd.HOME_20:
						{
							if (arrAct[Act.TablePad].IsPos(SenPos.BWD) == false)
							{
								arrAct[Act.TablePad].SetPos(SenPos.BWD);
								break;
							}
							
							if (m_IsMtHome == false)
							{
								smm.Next(Cmd.HOME_40);
								break;
							}

							
							if (G.seqTrayClean.IsSafety(m_nTableNo) == false)	break;

							arrMt[Axis.GripY].ClearHome();
							arrMt[Axis.GripY].StartHoming();

							arrMt[Axis.UpDnZ].ClearHome();
							arrMt[Axis.UpDnZ].StartHoming();

							smm.Next(Cmd.HOME_30);
						}
						break;

					case Cmd.HOME_30:
						{
							if (arrMt[Axis.GripY].IsHomeDone() == false || arrMt[Axis.GripY].IsRdy() == false) break;
							if (arrMt[Axis.UpDnZ].IsHomeDone() == false || arrMt[Axis.UpDnZ].IsRdy() == false) break;

							smm.Next(Cmd.HOME_40);
						}
						break;

					case Cmd.HOME_40:
						{
							if (arrMt[Axis.UpDnZ].IsRdy((int)PosZ.UP) == false)
							{
								arrMt[Axis.UpDnZ].MoveIndex((int)PosZ.UP);
								break;
							}
							
							if (arrAct[Act.TablePad].IsPos(SenPos.FWD) == false)
							{
								arrAct[Act.TablePad].SetPos(SenPos.FWD);
								break;
							}
							
							smm.Next(Cmd.HOME_50);
						}
						break;

					case Cmd.HOME_50:
						{
							if (SM.FMT.IsFlow(FlowTable) == true)
							{
								if (arrMt[Axis.GripY].IsRdy((int)PosGrip.GRIP) == false)
								{
									arrMt[Axis.GripY].MoveIndex((int)PosGrip.GRIP);
									break;
								}
								if (arrAct[Act.TrayGripX].IsPos(SenPos.FWD) == false)
								{
									arrAct[Act.TrayGripX].SetPos(SenPos.FWD);
									break;
								}
							}
							else
							{
								if (arrMt[Axis.GripY].IsRdy((int)PosGrip.RELEASE) == false)
								{
									arrMt[Axis.GripY].MoveIndex((int)PosGrip.RELEASE);
									break;
								}
								if (arrAct[Act.TrayGripX].IsPos(SenPos.BWD) == false)
								{
									arrAct[Act.TrayGripX].SetPos(SenPos.BWD);
									break;
								}
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

		private	void	Up(SMM<Cmd> smm)
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
								smm.Next(Cmd.UP_10);
							}
						}
						break;

					case Cmd.UP_10:
						{
							
							//메뉴얼모드면 체크 안하고 동작함 
							if (m_udMode == UDMode.Manual )
							{
								smm.Next(Cmd.UP_20);
								break;
							}

							if (G.seqUnloadTransfer.UnloadTableUpDnSafety(m_nTableNo) == false  ||
								G.seqLoadTransfer.UnloadTableUpDnSafety() == false)
							{
								break;
							}

							if		(m_udMode == UDMode.Load  &&
									(G.DryRunIsBIn(DI_ExistSen, true) == false  ||
									 SM.FMT.IsFlow(FlowTable) == false))
							{
								AddAlarm(ALM.ERROR_SEQ_UNLOAD_TRAY_TABLE1_TRAY_NONE + m_nOffsetSeqAlarm);
								break;
							}
							else if (m_udMode == UDMode.Unload  &&
									(G.DryRunIsBIn(DI_ExistSen, false) == true  ||
									 SM.FMT.IsFlow(FlowTable) == true))
							{
								AddAlarm(ALM.ERROR_SEQ_UNLOAD_TRAY_TABLE1_TRAY_EXIST + m_nOffsetSeqAlarm);
								break;
							}

							smm.Next(Cmd.UP_20);
						}
						break;

					case Cmd.UP_20:
						{
							if (arrAct[Act.TablePad].IsPos(SenPos.BWD) == false)
							{
								arrAct[Act.TablePad].SetPos(SenPos.BWD);
								break;
							}

							smm.Next(Cmd.UP_UP);
						}
						break;

					case Cmd.UP_UP:
                        {
                            if (m_udMode != UDMode.Manual && 
                                (G.seqUnloadTransfer.UnloadTableUpDnSafety(m_nTableNo) == false ||
                                G.seqLoadTransfer.UnloadTableUpDnSafety() == false))
                            {
                                break;
                            }

                            if (arrMt[Axis.UpDnZ].IsRdyPos(arrMt[Axis.UpDnZ].GetIndexPos((int)PosZ.UP)) == false)
							{
								arrMt[Axis.UpDnZ].MoveIndex((int)PosZ.UP);
								break;
							}

							if (arrAct[Act.TablePad].IsPos(SenPos.FWD) == false)
							{
								arrAct[Act.TablePad].SetPos(SenPos.FWD);
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

		private	void	Down(SMM<Cmd> smm)
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
								smm.Next(Cmd.DOWN_10);
							}
						}
						break;

					case Cmd.DOWN_10:
						{
							if (IsMotorStats(PosZ.DOWN) == true)
							{
								smm.Next(Cmd.END);
								break;
							}
							
							if (m_nTableNo == 0 && G.seqTrayClean.IsTrayCleanReady() == false)	break;
							
							//메뉴얼모드면 체크 안하고 동작함 
							if (m_udMode == UDMode.Manual )
							{
								smm.Next(Cmd.DOWN_30);
								break;
							}

							if (G.seqUnloadTransfer.UnloadTableUpDnSafety(m_nTableNo) == false  ||
								G.seqLoadTransfer.UnloadTableUpDnSafety() == false)
							{
								break;
							}

							if		(m_udMode == UDMode.Load  &&
									(G.DryRunIsBIn(DI_ExistSen, false) == true  ||
									 SM.FMT.IsFlow(FlowTable) == true))
							{
								AddAlarm(ALM.ERROR_SEQ_UNLOAD_TRAY_TABLE1_TRAY_EXIST + m_nOffsetSeqAlarm);
								break;
							}
							else if (m_udMode == UDMode.Unload  &&
									(G.DryRunIsBIn(DI_ExistSen, true) == false  ||
									 SM.FMT.IsFlow(FlowTable) == false))
							{
								AddAlarm(ALM.ERROR_SEQ_UNLOAD_TRAY_TABLE1_TRAY_NONE + m_nOffsetSeqAlarm);
								break;
							}

							m_smmAlign.eInput = InState.run;
							smm.Next(Cmd.DOWN_20);
						}
						break;

					case Cmd.DOWN_20:
						{
							if		(m_udMode == UDMode.Load)
							{
								arrAct[Act.TrayGripX].SetPos(SenPos.BWD);
								arrMt[Axis.GripY].MoveIndex((int)PosGrip.RELEASE);
							}
							else if (m_udMode == UDMode.Unload)
							{
								arrMt[Axis.GripY].MoveIndex((int)PosGrip.GRIP);
								arrAct[Act.TrayGripX].SetPos(SenPos.FWD);
							}

							if (IsAlignReady() == false)
							{
								break;
							}

							smm.Next(Cmd.DOWN_30);
						}
						break;

					case Cmd.DOWN_30:
						{
							if (arrAct[Act.TablePad].IsPos(SenPos.BWD) == false)
							{
								arrAct[Act.TablePad].SetPos(SenPos.BWD);
								break;
							}

							if (arrMt[Axis.UpDnZ].IsHomeDone() == false)
							{
								arrMt[Axis.UpDnZ].ClearHome();
								arrMt[Axis.UpDnZ].StartHoming();
							}

							smm.Next(Cmd.DOWN_40);
						}
						break;

					case Cmd.DOWN_40:
						{
                            bool CheckEmptyTray = SM.FMT[(FlowTray)((int)FlowTray.ULDTable1 + m_nTableNo)].IsEmpty();


                            if (arrMt[Axis.UpDnZ].IsHomeDone() == false || arrMt[Axis.UpDnZ].IsRdy() == false)	break;

							if (G.seqTrayClean.IsSafety(m_nTableNo) == false)									break;

							//조성재
							//if (m_udMode == SeqUnloadTrayTable.UDMode.Unload && CheckEmptyTray == false) smm.Next(Cmd.DOWN_VIBRATE1);
							//else smm.Next(Cmd.DOWN_DOWN);

							smm.Next(Cmd.DOWN_DOWN);
						}
						break;

                    case Cmd.DOWN_VIBRATE1:
                        {
                            arrAct[Act.AirVib].SetPos(SenPos.ON);
                            if (smm.Delayms(NDm.Read(eNDM.AirVibrateTime)) == false) break;

                            smm.Next(Cmd.DOWN_VIBRATE2);
                        }
                        break;

                    case Cmd.DOWN_VIBRATE2:
                        {
                            arrAct[Act.AirVib].SetPos(SenPos.OFF);
                            if (smm.Delayms(500) == false) break;

                            smm.Next(Cmd.DOWN_DOWN);
                        }
                        break;

					case Cmd.DOWN_DOWN:
						{
                            if ((G.seqUnloadTransfer.UnloadTableUpDnSafety(m_nTableNo) == false ||
                                G.seqLoadTransfer.UnloadTableUpDnSafety() == false)  && 
								m_udMode != UDMode.Manual)
                            {
                                arrMt[Axis.UpDnZ].Stop();
                                break;
                            }

                            if (arrMt[Axis.UpDnZ].IsRdyPos(arrMt[Axis.UpDnZ].GetIndexPos((int)PosZ.DOWN)) == false)
							{
								arrMt[Axis.UpDnZ].MoveIndex((int)PosZ.DOWN);
								break;
							}
						
							if (smm.Delayms(300) == false)			break;

							smm.Next(Cmd.DOWN_50);
						}
						break;
					
					case Cmd.DOWN_50:
						{
							if		(m_udMode == UDMode.Load)
							{
								if (arrMt[Axis.GripY].IsRdy((int)PosGrip.RELEASE) == false)
								{
									arrMt[Axis.GripY].MoveIndex((int)PosGrip.RELEASE);
									break;
								}

								if (arrAct[Act.TrayGripX].IsPos(SenPos.BWD) == false)
								{
									arrAct[Act.TrayGripX].SetPos(SenPos.BWD);
									break;
								}
							}
							else if (m_udMode == UDMode.Unload)
							{
								if (arrMt[Axis.GripY].IsRdy((int)PosGrip.GRIP) == false)
								{
									arrMt[Axis.GripY].MoveIndex((int)PosGrip.GRIP);
									break;
								}

								if (arrAct[Act.TrayGripX].IsPos(SenPos.FWD) == false)
								{
									arrAct[Act.TrayGripX].SetPos(SenPos.FWD);
									break;
								}
							}
						
							if (smm.Delayms(200) == false)			break;

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

		private	void	Grip(SMM<Cmd> smm)
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
								smm.Next(Cmd.GRIP_10);
							}
						}
						break;

					case Cmd.GRIP_10:
						{
							if (G.DryRunIsBIn(DI_ExistSen, true) == false)
							{
								AddAlarm(ALM.ERROR_SEQ_UNLOAD_TRAY_TABLE1_TRAY_NONE + m_nOffsetSeqAlarm);
							}
							else
							{
								smm.Next(Cmd.GRIP_20);
							}
						}
						break;

					case Cmd.GRIP_20:
						{
							if (arrMt[Axis.GripY].IsRdy((int)PosGrip.GRIP) == false)
							{
								arrMt[Axis.GripY].MoveIndex((int)PosGrip.GRIP);
								break;
							}

							smm.Next(Cmd.GRIP_30);
						}
						break;

					case Cmd.GRIP_30:
						{
							if (arrAct[Act.TrayGripX].IsPos(SenPos.FWD) == false)
							{
								arrAct[Act.TrayGripX].SetPos(SenPos.FWD);
								break;
							}

							if (smm.Delayms(NDm.Read(eNDM.TrayTableGripDelay)) == false)		break;

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

		private	void	Release(SMM<Cmd> smm)
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
								smm.Next(Cmd.RELEASE_10);
							}
						}
						break;

					case Cmd.RELEASE_10:
						{
							//if (SM.FMT.IsFlow(FlowTable) == true  && m_bManual == false)
							//{
							//	AddAlarm(ALM.ERROR_SEQ_UNLOAD_TRAY_TABLE1_TRAY_NONE + m_nOffsetSeqAlarm);
							//}
							
							if (arrMt[Axis.GripY].IsHomeDone() == false || m_IsMtHome == true)
							{
								arrMt[Axis.GripY].ClearHome();
								arrMt[Axis.GripY].StartHoming();
							}

							smm.Next(Cmd.RELEASE_20);
						}
						break;

					case Cmd.RELEASE_20:
						{
							if (arrMt[Axis.GripY].IsHomeDone() == false)			break;

							if (arrMt[Axis.GripY].IsRdy((int)PosGrip.RELEASE) == false ||
								arrAct[Act.TrayGripX].IsPos(SenPos.BWD) == false)
							{
								arrMt[Axis.GripY].MoveIndex((int)PosGrip.RELEASE);
								arrAct[Act.TrayGripX].SetPos(SenPos.BWD);
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

		private void Align(SMM<Cmd> smm)
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
								smm.Next(Cmd.ALIGN_10);
							}
						}
						break;

					case Cmd.ALIGN_10:
						{
							if (SM.SHM.GetUseSkipData(USESKIP.UnloadTableAligner) == false ||
								SM.FMT[FlowTable].IsAlignDone() == true)
							{
								smm.Next(Cmd.END);
								break;
							}

							arrAct[Act.AirVib].SetPos(SenPos.ON);
							//G.cKernel.AOut(DO_Aligner, true);

							smm.Next(Cmd.ALIGN_20);
						}
						break;

					case Cmd.ALIGN_20:
						{
							if (smm.Delayms(NDm.Read(eNDM.AirVibrateTime)) == false)
							{
								break;
							}

							arrAct[Act.AirVib].SetPos(SenPos.OFF);
							//G.cKernel.AOut(DO_Aligner, false);

							smm.Next(Cmd.END);
						}
						break;


					case Cmd.END:
						{
							SM.FMT[FlowTable].SetTrayHandlingStatus(ProdHandlingStatus.Aligned);
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
