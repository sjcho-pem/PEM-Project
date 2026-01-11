using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_MarsSSD
{
	public class SeqLoadTrayTable1 : SeqBase<SeqLoadTrayTable1.Act, SeqLoadTrayTable1.Axis, SeqLoadTrayTable1.Cmd>
	{
		#region [ Enum ]
		public	enum Act
		{
			TablePad,
			TrayGripX,

			Max,
		}

		public	enum Axis
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

		public	enum Cmd
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

			UP_10,
			UP_20,
			UP_30,
			UP_UP,
			UP_40,
			UP_50,
			UP_60,
			UP_70,
			UP_80,
			UP_90,
			UP_100,

			DOWN_10,
			DOWN_20,
			DOWN_30,
			DOWN_40,
			DOWN_DOWN,
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
		}

		public	enum UDMode
		{
			Load,
			Unload,
			Manual,
		}

		public	enum AutoMode
		{
			Auto,
			Manual,
		}

		#endregion

		#region [Variables]
		private SMM<Cmd>  m_smmHome;
		private SMM<Cmd>  m_smmUp;
		private SMM<Cmd>  m_smmDown;
		private SMM<Cmd>  m_smmGrip;
		private SMM<Cmd>  m_smmRelease;

		public	Cmd	smmHome		{ get => m_smmHome.CurStep; }
		public	Cmd	smmUp		{ get => m_smmUp.CurStep; }
		public	Cmd	smmDown		{ get => m_smmDown.CurStep; }
		public	Cmd	smmGrip		{ get => m_smmGrip.CurStep; }
		public	Cmd	smmRelease	{ get => m_smmRelease.CurStep; }

		private	UDMode	m_udMode;
		private int		m_nTableNo			= 0;
		private bool	m_IsMtHome			= false;
		private bool	m_bManual			= false;
		#endregion
		

		public void		Init()
		{
			try
			{
				m_nTableNo = 0;

				m_smmHome		= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.LOADTABLE1_HOME));
				m_smmUp			= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.LOADTABLE1_UP));
				m_smmDown		= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.LOADTABLE1_DOWN));
				m_smmGrip		= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.LOADTABLE1_GRIP));
				m_smmRelease	= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.LOADTABLE1_RELEASE));

				lstSMM.Add(m_smmHome);
				lstSMM.Add(m_smmUp);
				lstSMM.Add(m_smmDown);
				lstSMM.Add(m_smmGrip);
				lstSMM.Add(m_smmRelease);
				
				#region - Motor
				arrMt = new EnumArray<Axis, CMotor>(Axis.Max);
				arrMt[Axis.GripY]	= G.cKernel.Motor(MT.LDTable1GripY);
				arrMt[Axis.UpDnZ]	= G.cKernel.Motor(MT.LDTable1Z);
				#endregion

				#region - Actuator
				arrAct = new EnumArray<Act, Actuator>(Act.Max);

				arrAct[Act.TablePad] = new CSingleSen();
				arrAct[Act.TablePad].SetSenser1(new List<DI>(new DI[] {  DI.LDTrayTable1PadFwdCyl }));
				arrAct[Act.TablePad].SetSenser2(new List<DI>(new DI[] {  DI.LDTrayTable1PadBwdCyl }));
				arrAct[Act.TablePad].SetSol(DO.LDTrayTable1PadCyl);
				arrAct[Act.TablePad].Setting(ALM.ERROR_LOAD_TABLE1_PAD1_CYL_FORWARD, ALM.ERROR_LOAD_TABLE1_PAD1_CYL_BACKWARD , ErrSenPos.Ori);

				arrAct[Act.TrayGripX] = new CSingleSen();
				arrAct[Act.TrayGripX].SetSenser1(new List<DI>(new DI[] { }));
				arrAct[Act.TrayGripX].SetSenser2(new List<DI>(new DI[] { DI.LDTrayTable1GripOpenCyl }));
				arrAct[Act.TrayGripX].SetSol(DO.LDTrayTable1GripCyl);
				arrAct[Act.TrayGripX].Setting(ALM.ERROR_LOAD_TABLE1_GRIP_CYL_GRIP , ALM.ERROR_LOAD_TABLE1_GRIP_CYL_RELEASE , ErrSenPos.Ori);

				#endregion

				base.Initialize();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public bool HomeStart(bool IsMtHome = true) { m_IsMtHome = IsMtHome; return SetSeqRun(ref m_smmHome); }

		public bool UpStart(UDMode Mode) { m_udMode = Mode; return SetSeqRun(ref m_smmUp); }

		public bool DownStart(UDMode Mode) { m_udMode = Mode; return SetSeqRun(ref m_smmDown); }

		public bool GripStart() { return SetSeqRun(ref m_smmGrip); }

		public bool ReleaseStart(bool bManual = false) { m_bManual = bManual; m_IsMtHome = false; return SetSeqRun(ref m_smmRelease); }

		public bool IsHomeReady() { return m_smmHome.IsReady; }

		public bool IsUpReady() { return m_smmUp.IsReady; }

		public bool IsDownReady() { return m_smmDown.IsReady; }

		public bool IsGripReady() { return m_smmGrip.IsReady; }

		public bool IsReleaseReady() { return m_smmRelease.IsReady; }

		public bool IsGrip()
		{
			try
			{
				if (arrAct[Act.TrayGripX].IsPos(SenPos.FWD) == false)													return SM.SHM.InterlockMsgAdd(m_nTableNo, "arrAct[teAct.TrayGrip2].IsPos(SenPos.FWD) == false");
				if (arrMt[Axis.GripY].IsRdyPos((int)arrMt[Axis.GripY].GetIndexPos((int)PosGrip.GRIP)) == false)			return SM.SHM.InterlockMsgAdd(m_nTableNo, "arrMt[teAxis.GripY].IsRdyPos(Grip) == false");

				return SM.SHM.InterlockMsgReset(m_nTableNo);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public bool IsRelease()
		{
			try
			{
				if (arrAct[Act.TrayGripX].IsPos(SenPos.BWD) == false)													return SM.SHM.InterlockMsgAdd(m_nTableNo, "arrAct[teAct.TrayGripY].IsPos(SenPos.BWD) == false");
				if (arrMt[Axis.GripY].IsRdyPos((int)arrMt[Axis.GripY].GetIndexPos((int)PosGrip.RELEASE)) == false)		return SM.SHM.InterlockMsgAdd(m_nTableNo, "arrMt[teAxis.GripY].IsRdyPos(Release) == false");

				return SM.SHM.InterlockMsgReset(m_nTableNo);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public bool IsLoadEnable()
		{
			try
			{
				if (this.IsReady == false)									return SM.SHM.InterlockMsgAdd(m_nTableNo, "this.IsReady == false");
				if (arrAct[Act.TablePad].IsPos(SenPos.BWD) == false)		return SM.SHM.InterlockMsgAdd(m_nTableNo, "arrAct[teAct.TablePad].IsPos(SenPos.BWD) == false");
				if (arrAct[Act.TrayGripX].IsPos(SenPos.BWD) == false)		return SM.SHM.InterlockMsgAdd(m_nTableNo, "arrAct[teAct.TrayGrip2].IsPos(SenPos.BWD) == false");

				if (arrMt[Axis.GripY].IsRdyPos((int)arrMt[Axis.GripY].GetIndexPos((int)PosGrip.RELEASE)) == false)	return SM.SHM.InterlockMsgAdd(m_nTableNo, "arrMt[teAxis.GripY].IsRdyPos(Release) == false");
				if (arrMt[Axis.UpDnZ].IsRdyPos((int)arrMt[Axis.UpDnZ].GetIndexPos((int)PosZ.DOWN)) == false)		return SM.SHM.InterlockMsgAdd(m_nTableNo, "arrMt[teAxis.GripY].IsRdyPos(Release) == false");
				
				if (G.cKernel.IsBIn(DI.LDTrayTable1ExistSen) == true)
				{
					AddAlarm(ALM.ERROR_SEQ_LOAD_TRAY_TABLE1_TRAY_EXIST );
					return SM.SHM.InterlockMsgAdd(m_nTableNo, "G.cKernel.IsAIn(DI_ExistSen) == true");
				}

				if (SM.FMT.IsFlow(FlowTray.LDTable1) == true) return SM.SHM.InterlockMsgAdd(m_nTableNo, "SM.FMT.IsFlow(FlowTable) == true");

				return SM.SHM.InterlockMsgReset(m_nTableNo);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public bool IsUnloadEnable()
		{
			try
			{
				if (this.IsReady == false) return SM.SHM.InterlockMsgAdd(m_nTableNo, "this.IsReady == false");
				if (arrAct[Act.TablePad].IsPos(SenPos.BWD) == false) return SM.SHM.InterlockMsgAdd(m_nTableNo, "arrAct[teAct.TablePad].IsPos(SenPos.BWD) == false");
				if (arrAct[Act.TrayGripX].IsPos(SenPos.FWD) == false) return SM.SHM.InterlockMsgAdd(m_nTableNo, "arrAct[teAct.TrayGrip2].IsPos(SenPos.FWD) == false");
			
				if (arrMt[Axis.GripY].IsRdyPos((int)arrMt[Axis.GripY].GetIndexPos((int)PosGrip.GRIP)) == false) return SM.SHM.InterlockMsgAdd(m_nTableNo, "arrMt[teAxis.GripY].IsRdyPos(Grip) == false");
				if (arrMt[Axis.UpDnZ].IsRdyPos((int)arrMt[Axis.UpDnZ].GetIndexPos((int)PosZ.DOWN)) == false)					return SM.SHM.InterlockMsgAdd(m_nTableNo, "arrMt[teAxis.GripY].IsRdyPos(Release) == false");
				
				if (G.DryRunIsBIn(DI.LDTrayTable1ExistSen, true) == false)
				{
					AddAlarm(ALM.ERROR_SEQ_LOAD_TRAY_TABLE1_TRAY_NONE);
					return SM.SHM.InterlockMsgAdd(m_nTableNo, "G.DryRunIsAIn(DI_ExistSen, true) == false");
				}
				if (SM.FMT.IsFlow(FlowTray.LDTable1) == false) return SM.SHM.InterlockMsgAdd(m_nTableNo, "SM.FMT.IsFlow(FlowTable) == false");

				return SM.SHM.InterlockMsgReset(m_nTableNo);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public bool IsPnPEnable()
		{
			try
			{
				if (IsReady == false)		return false;
					
				if (arrAct[Act.TablePad].IsPos(SenPos.FWD) == false)												return SM.SHM.InterlockMsgAdd(m_nTableNo, "arrAct[teAct.TablePad].IsPos(SenPos.FWD) == false");
				if (arrAct[Act.TrayGripX].IsPos(SenPos.FWD) == false)												return SM.SHM.InterlockMsgAdd(m_nTableNo, "arrAct[teAct.TrayGrip2].IsPos(SenPos.FWD) == false");
				
				if (arrMt[Axis.UpDnZ].IsRdyPos((int)arrMt[Axis.UpDnZ].GetIndexPos((int)PosZ.UP)) == false)			return SM.SHM.InterlockMsgAdd(m_nTableNo, "arrMt[teAxis.GripY].IsRdyPos(Release) == false");
				if (arrMt[Axis.GripY].IsRdyPos((int)arrMt[Axis.GripY].GetIndexPos((int)PosGrip.GRIP)) == false)		return SM.SHM.InterlockMsgAdd(m_nTableNo, "arrMt[teAxis.GripY].IsRdyPos(Grip) == false");
				
				if (G.DryRunIsBIn(DI.LDTrayTable1ExistSen, true) == false)											return SM.SHM.InterlockMsgAdd(m_nTableNo, "G.DryRunIsAIn(DI_ExistSen, true) == false");
				if (SM.FMT.IsFlow(FlowTray.LDTable1) == false)														return SM.SHM.InterlockMsgAdd(m_nTableNo, "SM.FMT.IsFlow(FlowTable) == false");
				if (SM.FMT[FlowTray.LDTable1].ProcStatus == ProcStatus.TrayEmptyDone)								return SM.SHM.InterlockMsgAdd(m_nTableNo, "SM.FMT[teFlowTray.LDTable1 + m_nTableNo].ProcStatus == teProcStatus.TrayEmptyDone");

				return SM.SHM.InterlockMsgReset(m_nTableNo);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public bool IsUpState()
		{
			try
			{
				if (arrMt[Axis.UpDnZ].IsRdyPos((int)arrMt[Axis.UpDnZ].GetIndexPos((int)PosZ.UP)) == false)		return SM.SHM.InterlockMsgAdd(m_nTableNo, "arrMt[teAxis.GripY].IsRdyPos(Release) == false");
				if (arrAct[Act.TablePad].IsPos(SenPos.FWD) == false)											return SM.SHM.InterlockMsgAdd("arrAct[teAct.TablePad].IsPos(SenPos.FWD) == false");

				SM.SHM.InterlockMsgReset();
				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public bool IsMotorStats(PosZ ePos)
		{
			try
			{
				if (arrMt[Axis.UpDnZ].IsRdy() == false)			return false;
                if (m_smmUp.IsReady == false)               return false;
                if (m_smmDown.IsReady == false)		            return false;

				if (arrMt[Axis.UpDnZ].IsRdyPos((int)arrMt[Axis.UpDnZ].GetIndexPos((int)ePos)) == false)		return false;
					
				return true;
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




		public void Run()
		{
			try
			{
				UpdateAct(Seq.LoadTrayTable1 + m_nTableNo);

				if (SM.SHM.IsBitData(BIT.Error) == true || SM.SHM.IsBitData(BIT.Pause) == true) return;

				Home(m_smmHome);
				Up(m_smmUp);
				Down(m_smmDown);
				Grip(m_smmGrip);
				Release(m_smmRelease);

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
							if (G.seqLoadTransfer.LoadTableUpDnSafety(m_nTableNo) == false)
							{
								break;
							}

							if (SM.FMT.IsFlow(FlowTray.LDTable1) != G.DryRunIsBIn(DI.LDTrayTable1ExistSen, SM.FMT.IsFlow(FlowTray.LDTable1)))
							{
								AddAlarm(ALM.ERROR_SEQ_LOAD_TRAY_TABLE1_TRAY_MISMATCH );
								break;
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

							smm.Next(Cmd.HOME_30);
						}
						break;

					case Cmd.HOME_30:
						{
							if (smm.Delayms(10000) == true)
							{
								AddAlarm(ALM.UNKNOWN);
							}
							
							if (m_IsMtHome == false)
							{
								smm.Next(Cmd.HOME_50);
								break;
							}
							
							arrMt[Axis.GripY].ClearHome();
							arrMt[Axis.GripY].StartHoming();

							arrMt[Axis.UpDnZ].ClearHome();
							arrMt[Axis.UpDnZ].StartHoming();

							smm.Next(Cmd.HOME_40);
						}
						break;

					case Cmd.HOME_40:
						{
							if (arrMt[Axis.GripY].IsHomeDone() == false || arrMt[Axis.GripY].IsRdy() == false) break;
							if (arrMt[Axis.UpDnZ].IsHomeDone() == false || arrMt[Axis.UpDnZ].IsRdy() == false) break;

							smm.Next(Cmd.HOME_50);
						}
						break;

					case Cmd.HOME_50:
						{
							if (arrMt[Axis.UpDnZ].IsRdy((int)PosZ.DOWN) == false)
							{
								arrMt[Axis.UpDnZ].MoveIndex((int)PosZ.DOWN);
								break;
							}

							smm.Next(Cmd.HOME_60);
						}
						break;

					case Cmd.HOME_60:
						{
							if (SM.FMT.IsFlow(FlowTray.LDTable1) == true)
							{
								if (arrMt[Axis.GripY].IsRdy((int)PosGrip.GRIP) == false)
								{
									arrMt[Axis.GripY].MoveIndex((int)PosGrip.GRIP);
									break;
								}
								if (arrAct[Act.TrayGripX].IsPos(SenPos.TABLE_GRIP) == false)
								{
									arrAct[Act.TrayGripX].SetPos(SenPos.TABLE_GRIP);
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
								if (arrAct[Act.TrayGripX].IsPos(SenPos.TABLE_RELEASE) == false)
								{
									arrAct[Act.TrayGripX].SetPos(SenPos.TABLE_RELEASE);
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

		private void Up(SMM<Cmd> smm)
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
							if (G.seqLoadTrayTable2.LoadTable1UpDnSafety() == false)	break;
							
							//메뉴얼모드면 다른건 체크 안하고 동작함 
							if (m_udMode == UDMode.Manual )
							{
								smm.Next(Cmd.UP_20);
								break;
							}

							if (G.seqLoadTransfer.LoadTableUpDnSafety(m_nTableNo) == false)
							{
								break;
							}

							if (G.seqLoadTrayTable2.LoadTable1UpDnSafety() == false)
							{
								break;
							}
							

							if (m_udMode == UDMode.Load &&
									(G.DryRunIsBIn(DI.LDTrayTable1ExistSen, true) == false ||
									 SM.FMT.IsFlow(FlowTray.LDTable1) == false))
							{
								AddAlarm(ALM.ERROR_SEQ_LOAD_TRAY_TABLE1_TRAY_NONE );
								break;
							}
							else if (m_udMode == UDMode.Unload &&
									(G.DryRunIsBIn(DI.LDTrayTable1ExistSen, false) == true ||
									SM.FMT.IsFlow(FlowTray.LDTable1) == true))
							{
								AddAlarm(ALM.ERROR_SEQ_LOAD_TRAY_TABLE1_TRAY_EXIST );
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

                            if (m_udMode != UDMode.Manual && G.seqLoadTransfer.LoadTableUpDnSafety(m_nTableNo) == false)
                            {
                                break;
                            }

                            smm.Next(Cmd.UP_UP);
						}
						break;

					case Cmd.UP_UP:
						{
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

		private void Down(SMM<Cmd> smm)
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

							//메뉴얼모드면 체크 안하고 동작함 
							if (m_udMode == UDMode.Manual )
							{
								smm.Next(Cmd.DOWN_30);
								break;
							}

							if (G.seqLoadTransfer.LoadTableUpDnSafety(m_nTableNo) == false)
							{
								break;
							}

							if (G.seqLoadTrayTable2.LoadTable1UpDnSafety() == false)
							{
								break;
							}

							if (m_udMode == UDMode.Load &&
									(G.DryRunIsBIn(DI.LDTrayTable1ExistSen, false) == true ||
									 SM.FMT.IsFlow(FlowTray.LDTable1) == true))
							{
								AddAlarm(ALM.ERROR_SEQ_LOAD_TRAY_TABLE1_TRAY_EXIST );
								break;
							}
							else if (m_udMode == UDMode.Unload &&
									(G.DryRunIsBIn(DI.LDTrayTable1ExistSen, true) == false ||
									 SM.FMT.IsFlow(FlowTray.LDTable1) == false))
							{
								AddAlarm(ALM.ERROR_SEQ_LOAD_TRAY_TABLE1_TRAY_NONE );
								break;
							}

							smm.Next(Cmd.DOWN_20);
						}
						break;

					case Cmd.DOWN_20:
						{
							if (m_udMode == UDMode.Load)
							{
								arrMt[Axis.GripY].MoveIndex((int)PosGrip.RELEASE);
								arrAct[Act.TrayGripX].SetPos(SenPos.BWD);
							}
							else if (m_udMode == UDMode.Unload)
							{
								arrMt[Axis.GripY].MoveIndex((int)PosGrip.GRIP);
								arrAct[Act.TrayGripX].SetPos(SenPos.FWD);
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
							
							smm.Next(Cmd.DOWN_DOWN);
						}
						break;

					case Cmd.DOWN_DOWN://위치 조정 시 반드시 인터락 구간 확인
						{
							if (arrMt[Axis.UpDnZ].IsHomeDone() == false || arrMt[Axis.UpDnZ].IsRdy() == false)	break;

							if (arrMt[Axis.UpDnZ].IsRdyPos(arrMt[Axis.UpDnZ].GetIndexPos((int)PosZ.DOWN)) == false)
							{
								arrMt[Axis.UpDnZ].MoveIndex((int)PosZ.DOWN);
								break;
							}

							smm.Next(Cmd.END);
						}
						break;

					case Cmd.DOWN_50:
						{
							if (m_udMode == UDMode.Load)
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

							if (smm.Delayms(200) == false) break;

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

		private void Grip(SMM<Cmd> smm)
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
							if (G.DryRunIsBIn(DI.LDTrayTable1ExistSen, true) == false)
							{
								AddAlarm(ALM.ERROR_SEQ_LOAD_TRAY_TABLE1_TRAY_NONE );
								break;
							}
							
							smm.Next(Cmd.GRIP_20);
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

							if (smm.Delayms(NDm.Read(eNDM.TrayTableGripDelay)) == false) break;

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

		private void Release(SMM<Cmd> smm)
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
							//if (SM.FMT.IsFlow(FlowTray.LDTable1) == false && m_bManual == false)
							//{
							//	AddAlarm(ALM.ERROR_SEQ_LOAD_TRAY_TABLE1_TRAY_NONE );
							//	break;
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
							if (arrMt[Axis.GripY].IsHomeDone() == false) break;

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
	}
}
