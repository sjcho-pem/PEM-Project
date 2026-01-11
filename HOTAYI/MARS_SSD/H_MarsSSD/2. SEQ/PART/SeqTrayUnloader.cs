using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace H_MarsSSD
{
	public class SeqTrayUnloader : SeqBase<SeqTrayUnloader.teAct, SeqTrayUnloader.teAxis, SeqTrayUnloader.teCmd>
	{
		#region [ Enum ]
		public enum teAct
		{
			FrontStopper,
			Max,
		}

		public enum teAxis
		{
			TrayLift,
			ConvLeft,
			ConvRight,
			ConvWidth,
			Max,
		}

		public enum teCmd
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
			HOME_91,
			HOME_92,
			HOME_100,
			HOME_110,
			HOME_120,
			HOME_130,
			HOME_140,
			HOME_150,

			LIFTUP_10,
			LIFTUP_MESCHECK1,
			LIFTUP_MESCHECK2,
			LIFTUP_20,
			LIFTUP_30,
			LIFTUP_32,
			LIFTUP_40,
			LIFTUP_50,

			LIFTDOWN_10,
			LIFTDOWN_20,
			LIFTDOWN_30,
			LIFTDOWN_40,
			LIFTDOWN_50,

			UNLOAD_10,
			UNLOAD_20,
			UNLOAD_30,
			UNLOAD_31,
			UNLOAD_32,
			UNLOAD_33,
			UNLOAD_GROUPNAMECHECK1,
			UNLOAD_GROUPNAMECHECK2,
			UNLOAD_GROUPNAMECHECK3,
			UNLOAD_GROUPNAMECHECK4,
			UNLOAD_34,
			UNLOAD_40,
			UNLOAD_50,
            UNLOAD_60,
            UNLOAD_61,
            UNLOAD_70,
			UNLOAD_80,

			ALIGN_10,
			ALIGN_20,
			ALIGN_30,
			ALIGN_40,
			ALIGN_50,
			ALIGN3_MESCHECK,
			
			SWLIFTUP_10,
			SWLIFTUP_20,
			SWLIFTUP_30,
			SWLIFTUP_40,

			SWLIFTDOWN_10,
			SWLIFTDOWN_20,
			SWLIFTDOWN_30,
			SWLIFTDOWN_40,
		}

		public enum tePosAlign
		{
			READY,
			ALIGN,
		}

		public enum tePosConv
		{
			UNLOAD_FAST,
			UNLOAD_SLOW,
		}

		public enum tePosLift
		{
			DOWN,
			UP,
            SWITCHUP,
            SWITCHDOWN,
        }

        #endregion

        #region [Variables]
        private SMM<teCmd> m_smmHome		= new SMM<teCmd>(teCmd.START, EnumHelper.GetDescription(SeqID.UNLOADER_HOME));
		private SMM<teCmd> m_smmUnload		= new SMM<teCmd>(teCmd.START, EnumHelper.GetDescription(SeqID.UNLOADER_UNLOAD));
		private SMM<teCmd> m_smmLiftUp		= new SMM<teCmd>(teCmd.START, EnumHelper.GetDescription(SeqID.UNLOADER_LIFTUP));
		private SMM<teCmd> m_smmLiftDown	= new SMM<teCmd>(teCmd.START, EnumHelper.GetDescription(SeqID.UNLOADER_LIFTDOWN));
		private SMM<teCmd> m_smmAlign		= new SMM<teCmd>(teCmd.START, EnumHelper.GetDescription(SeqID.UNLOADER_ALIGN));
		private SMM<teCmd> m_smmSWLiftDown	= new SMM<teCmd>(teCmd.START, "SWITCH DOWN");
		private SMM<teCmd> m_smmSWLiftUp	= new SMM<teCmd>(teCmd.START, "SWITCH UP");
		

		public	teCmd	smmHome			{ get => m_smmHome.CurStep; }
		public	teCmd	smmLiftDown		{ get => m_smmLiftDown.CurStep; }
		public	teCmd	smmUnload		{ get => m_smmUnload.CurStep; }
		public	teCmd	smmLiftUp		{ get => m_smmLiftUp.CurStep; }
		public	teCmd	smmAlign		{ get => m_smmAlign.CurStep; }
		public	teCmd	smmSWLiftDown	{ get => m_smmSWLiftDown.CurStep; }
		public	teCmd	smmSWLiftUp		{ get => m_smmSWLiftUp.CurStep; }

		private	PVI			m_PVI					= new PVI();
		private	int			nStopDelay				= 0;
		private	int			m_nSecsGemAlarmDelay;
		private	bool		m_IsMtHome				= true;
		private	int			m_nMesCount				= 0;
		private int			m_nLiftTartgetPos		= 0;
		private	string[]	m_strGroupName			= new string[2] {string.Empty, string.Empty};
		#endregion

		#region [Function]
		public	void	Init()
		{
			try
			{
				lstSMM.Add(m_smmHome);
				lstSMM.Add(m_smmUnload);
				lstSMM.Add(m_smmLiftUp);
				lstSMM.Add(m_smmLiftDown);
				lstSMM.Add(m_smmAlign);
				lstSMM.Add(m_smmSWLiftDown);
				lstSMM.Add(m_smmSWLiftUp);

				#region - Motor
				arrMt = new EnumArray<teAxis, CMotor>(teAxis.Max);

				arrMt[teAxis.ConvLeft]	= G.cKernel.Motor(MT.ULDConv1);
				arrMt[teAxis.ConvRight]	= G.cKernel.Motor(MT.ULDConv2);
				arrMt[teAxis.ConvWidth]	= G.cKernel.Motor(MT.ULDConvWidth);
				arrMt[teAxis.TrayLift]	= G.cKernel.Motor(MT.ULDTrayLiftZ);
				#endregion

				#region - Actuator            
				arrAct = new EnumArray<teAct, Actuator>(teAct.Max);

				arrAct[teAct.FrontStopper] = new CSingleSen();
				arrAct[teAct.FrontStopper].SetSenser1(new List<DI>(new DI[] { DI.ULDConvStopperFwdCyl }));
				arrAct[teAct.FrontStopper].SetSenser2(new List<DI>(new DI[] { DI.ULDConvStopperBwdCyl }));
				arrAct[teAct.FrontStopper].SetSol(DO.ULDConvStopperCyl);
				arrAct[teAct.FrontStopper].Setting(ALM.ERROR_UNLOAD_LIFT_STOPPER_CYL_FORWARD, ALM.ERROR_UNLOAD_LIFT_STOPPER_CYL_BACKWARD, ErrSenPos.Ori, SenPos.NONE, 500);

				/*
				sensor List  
				ULDTrayLiftAreaSen1	- Lot 투입부, 안전 센서 
				ULDTrayLiftAreaSen2	- Lot 투입부, 안전 센서
				ULDTrayLiftLimitSen	- Lot 투입 후, Lot 갯수 초과 방지 센서
				ULDTrayLiftExistSen	- Lift z 축 하강 후, 트레이 감지 센서
				ULDTrayLiftDecSen	- Unload 감속 센서
				ULDTrayConvExistSen	- 컨베이어 트레이 감지 센서
				 */
				#endregion

				if (NDm.Read(eNDM.ULDLiftAlignDelay) <= 0)			NDm.Write(eNDM.ULDLiftAlignDelay, 500);

				base.Initialize();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
		
		public	bool	HomeStart(bool IsMtHome = true)	{m_IsMtHome = IsMtHome;  return SetSeqRun(ref m_smmHome); }

		public	bool	UnloadStart()				{ return SetSeqRun(ref m_smmUnload); }

		public	bool	LiftDownStart()				{ return SetSeqRun(ref m_smmLiftDown); }

		public	bool	LiftUpStart()				{ return SetSeqRun(ref m_smmLiftUp); }

		public	bool	AlignStart()				{ return SetSeqRun(ref m_smmAlign); }
		
		public	bool	SwitchLiftUpStart()					{ return SetSeqRun(ref m_smmSWLiftUp); }

		public	bool	SwitchLiftDownStart()				{ return SetSeqRun(ref m_smmSWLiftDown); }


		public	bool	IsHomeReady()				{ return m_smmHome.IsReady; }
		
		public	bool	IsUnloadReady()				{ return m_smmUnload.IsReady; }

		public	bool	IsAlignReady()				{ return m_smmAlign.IsReady; }
		
		public	bool	IsConvReady()				{ return m_smmUnload.IsReady && m_smmHome.IsReady && m_smmAlign.IsReady; }

		public	bool	IsLiftReady()				{ return m_smmLiftDown.IsReady && m_smmLiftUp.IsReady && m_smmHome.IsReady; }



		public	void	ResetLiftStep()
		{
			arrMt[teAxis.TrayLift].Stop();
			
			m_smmLiftUp.ResetStep();
			m_smmLiftDown.ResetStep();
			m_smmSWLiftUp.ResetStep();
			m_smmSWLiftDown.ResetStep();
		}



		public	bool	IsTransLoadEnable()
		{
			try
			{
				if (arrMt[teAxis.ConvWidth].IsRdy((int)tePosAlign.READY) == false)			return SM.SHM.InterlockMsgAdd("arrMt[teAxis.ConvWidth].IsRdy((int)tePosAlign.READY) == false");

				if (arrMt[teAxis.ConvRight].IsRdy() == false)			return SM.SHM.InterlockMsgAdd("arrMt[teAxis.ConvRight].IsRdy() == false");
				if (arrMt[teAxis.ConvLeft].IsRdy() == false)			return SM.SHM.InterlockMsgAdd("arrMt[teAxis.ConvLeft].IsRdy() == false");

				if (m_smmUnload.IsReady == false)			return SM.SHM.InterlockMsgAdd("m_smmUnload.IsReady == false");
				if (m_smmAlign.IsReady == false)			return SM.SHM.InterlockMsgAdd("m_smmAlign.IsReady == false");

				if (SM.FMT.IsFlow(FlowTray.ULDConv01+stLotCnt.Max-1) == true)
				{
					AddAlarm(ALM.ERROR_SEQ_UNLOADER_CONV_TRAY_FULL);
					return false;
				}

				SM.SHM.InterlockMsgReset();
				return true;
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
				if (G.DryRunIsAIn(DI.ULDTrayConvExistSen, true) == false)		return SM.SHM.InterlockMsgAdd("G.DryRunIsAIn(DI.ULDTrayConvExistSen, true) == false");
				if (G.DryRunIsAIn(DI.ULDTrayLiftExistSen, false) == true)		return SM.SHM.InterlockMsgAdd("G.DryRunIsAIn(DI.ULDTrayLiftExistSen, false) == true");
				if (arrMt[teAxis.ConvRight].IsRdy() == false)					return SM.SHM.InterlockMsgAdd("arrMt[teAxis.ConvRight].IsRdy() == false");
				if (arrMt[teAxis.ConvLeft].IsRdy() == false)					return SM.SHM.InterlockMsgAdd("arrMt[teAxis.ConvLeft].IsRdy() == false");

				if (arrMt[teAxis.ConvWidth].IsRdy((int)tePosAlign.READY) == false)			return SM.SHM.InterlockMsgAdd("arrMt[teAxis.ConvWidth].IsRdy((int)tePosAlign.READY) == false");
				if (arrMt[teAxis.TrayLift].IsRdyPos(arrMt[teAxis.TrayLift].GetIndexPos((int)tePosLift.DOWN))  == false)			return SM.SHM.InterlockMsgAdd("arrMt[teAxis.TrayLift].IsRdyPos(arrMt[teAxis.TrayLift].GetIndexPos((int)tePosLift.DOWN))  == false");
				
				if (m_smmUnload.IsReady == false)			return SM.SHM.InterlockMsgAdd("m_smmUnload.IsReady == false");
				if (m_smmAlign.IsReady == false)			return SM.SHM.InterlockMsgAdd("m_smmAlign.IsReady == false");

				return SM.SHM.InterlockMsgReset();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public	bool	IsLiftUpEnable()
		{
			try
			{
				if (G.DryRunIsAIn(DI.ULDTrayLiftExistSen, true) == false)							return SM.SHM.InterlockMsgAdd("G.DryRunIsAIn(DI.ULDTrayLiftExistSen, true) == false");
				if (SM.FMT.IsFlowRange(FlowTray.ULDLift01, FlowTray.ULDLift31) == false)		return SM.SHM.InterlockMsgAdd("SM.FMT.IsFlowRange(teFlowTray.ULDLift01, teFlowTray.ULDLift15) == false");
				if (arrMt[teAxis.ConvRight].IsRdy() == false)										return SM.SHM.InterlockMsgAdd("arrMt[teAxis.ConvRight].IsRdy() == false");
				if (arrMt[teAxis.ConvLeft].IsRdy() == false)										return SM.SHM.InterlockMsgAdd("arrMt[teAxis.ConvLeft].IsRdy() == false");
				if (arrMt[teAxis.TrayLift].IsRdy((int)tePosLift.DOWN) == false)						return SM.SHM.InterlockMsgAdd("arrMt[teAxis.TrayLift].IsRdy((int)tePosLift.UP) == true");
				
				if (m_smmLiftUp.IsReady == false)			return SM.SHM.InterlockMsgAdd("m_smmLiftUp.IsReady == false");
				if (m_smmLiftDown.IsReady == false)			return SM.SHM.InterlockMsgAdd("m_smmLiftDown.IsReady == false");

				return SM.SHM.InterlockMsgReset();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public	bool	IsLiftDownEnable()
		{
			try
			{
				if (G.cKernel.IsAIn(DI.ULDTrayLiftExistSen) == true)								return SM.SHM.InterlockMsgAdd("G.cKernel.IsAIn(DI.ULDTrayLiftExistSen) == true");
				if (SM.FMT.IsFlowRange(FlowTray.ULDLift01, FlowTray.ULDLift31) == true)			return SM.SHM.InterlockMsgAdd("SM.FMT.IsFlowRange(teFlowTray.ULDLift01, teFlowTray.ULDLift15) == true");
				if (arrMt[teAxis.ConvRight].IsRdy() == false)										return SM.SHM.InterlockMsgAdd("arrMt[teAxis.ConvRight].IsRdy() == false");
				if (arrMt[teAxis.ConvLeft].IsRdy() == false)										return SM.SHM.InterlockMsgAdd("arrMt[teAxis.ConvLeft].IsRdy() == false");

				if (arrMt[teAxis.TrayLift].IsRdy((int)tePosLift.DOWN) == true)						return SM.SHM.InterlockMsgAdd("arrMt[teAxis.TrayLift].IsRdy((int)tePosLift.DOWN) == true");
				
				if (m_smmUnload.IsReady == false)			return SM.SHM.InterlockMsgAdd("m_smmUnload.IsReady == false");
				if (m_smmLiftUp.IsReady == false)			return SM.SHM.InterlockMsgAdd("m_smmLiftUp.IsReady == false");
				if (m_smmLiftDown.IsReady == false)			return SM.SHM.InterlockMsgAdd("m_smmLiftDown.IsReady == false");

				return SM.SHM.InterlockMsgReset();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public	bool	IsLiftUp()
		{
			try
			{
				if (arrMt[teAxis.TrayLift].IsRdy((int)tePosLift.UP) == true)						return true;
				
				return false;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public	bool	IsLastTrayExist()
		{
			try
			{
				//if (NDm.Read(eNDM.AutoRunMode) == (int)teAutoRunMode.Bypass)			return false;

				for (int i = 0; i < stLotCnt.Max; i++)
				{
					if (SM.FMT.IsFlow(FlowTray.ULDConv01+i) == true  &&  SM.FMT[FlowTray.ULDConv01+i].GetInfo().LastTrayOfLot == true)
					{
						return true;
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
		

		private	bool	SetSeqRun(ref SMM<teCmd> smm)
		{
			try
			{
				if (this.IsReady == false)	return false;

				smm.eInput = InState.run;
				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}
		#endregion

		
		public  void Run()
		{
			try
			{
				UpdateAct(Seq.Unloader);

                SWLiftUp(m_smmSWLiftUp);
                SWLiftDown(m_smmSWLiftDown);

                if (SM.SHM.IsBitData(BIT.Error) == true  ||  SM.SHM.IsBitData(BIT.Pause) == true)
				{
					if (arrMt[teAxis.ConvLeft].IsCommand() == true ||
						arrMt[teAxis.ConvRight].IsCommand() == true)
					{
						arrMt[teAxis.ConvLeft].Stop();
						arrMt[teAxis.ConvRight].Stop();
					}
					
					return;
				}
				
				Home(m_smmHome);
				Unload(m_smmUnload);
				LiftUp(m_smmLiftUp);
				LiftDown(m_smmLiftDown);
				Align(m_smmAlign);
				SM.SHM.SetSeqStatus(lstSMM);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void Home(SMM<teCmd> smm)
		{
			try
			{
				switch (smm.CurStep)
				{
					case teCmd.START:
						{
							if (smm.eInput == InState.run)
							{
								smm.eInput = InState.none;

								smm.Next(teCmd.HOME_10);
							}
						}
						break;

					case teCmd.HOME_10:
						{
							//자재 존재시, 홈 동작하지않음
							if (G.DryRunIsAIn(DI.ULDTrayLiftExistSen, false) == true)
							{
								smm.Next(teCmd.HOME_40);
								break;
							}
                            
                            if (G.DryRunIsBIn(DI.ULDTrayLiftAreaSen1, false) == true)
                            {
                                AddAlarm(ALM.ERROR_SEQ_UNLOADER_LIFT_AREA_SENSOR1_CHECK);
                                smm.Next(teCmd.END);
                                break;
                            }
                            if (G.DryRunIsBIn(DI.ULDTrayLiftAreaSen2, false) == true)
                            {
                                AddAlarm(ALM.ERROR_SEQ_UNLOADER_LIFT_AREA_SENSOR2_CHECK);
                                smm.Next(teCmd.END);
                                break;
                            }

                            if (m_IsMtHome == false)
							{
								smm.Next(teCmd.HOME_90);
								break;
							}

							smm.Next(teCmd.HOME_20);
						}
						break;

					case teCmd.HOME_20:
						{
							arrMt[teAxis.TrayLift].ClearHome();
							arrMt[teAxis.TrayLift].StartHoming();

							smm.Next(teCmd.HOME_30);
						}
						break;

					case teCmd.HOME_30:
						{
							if (arrMt[teAxis.TrayLift].IsRdy() == false  ||
								arrMt[teAxis.TrayLift].IsHomeDone() == false)			break;

							smm.Next(teCmd.HOME_40);
						}
						break;

					case teCmd.HOME_40:
						{
                            if (m_IsMtHome == false)
                            {
                                smm.Next(teCmd.HOME_90);
                                break;
                            }

                            if (G.seqUnloadTransfer.UnloadConvMoveSafety() == false)			break;

							smm.Next(teCmd.HOME_50);
						}
						break;

					case teCmd.HOME_50:
						{
							arrMt[teAxis.ConvWidth].ClearHome();
							arrMt[teAxis.ConvWidth].StartHoming();

							smm.Next(teCmd.HOME_60);
						}
						break;

					case teCmd.HOME_60:
						{
							if (arrMt[teAxis.ConvWidth].IsRdy() == false  ||
								arrMt[teAxis.ConvWidth].IsHomeDone() == false)			break;

							smm.Next(teCmd.HOME_70);
						}
						break;

					case teCmd.HOME_70:
						{
							//현재 위치 홈동작
							arrMt[teAxis.ConvLeft].ClearHome();
							arrMt[teAxis.ConvLeft].StartHoming();
							arrMt[teAxis.ConvRight].ClearHome();
							arrMt[teAxis.ConvRight].StartHoming();

							smm.Next(teCmd.HOME_80);
						}
						break;

					case teCmd.HOME_80:
						{

							if (arrMt[teAxis.ConvLeft].IsRdy() == false ||
								arrMt[teAxis.ConvLeft].IsHomeDone() == false)			break;
							if (arrMt[teAxis.ConvRight].IsRdy() == false ||
								arrMt[teAxis.ConvRight].IsHomeDone() == false)			break;

							smm.Next(teCmd.HOME_90);
						}
						break;

					case teCmd.HOME_90:
						{
							if (arrMt[teAxis.ConvWidth].IsRdy((int)tePosAlign.READY) == false)
							{
								arrMt[teAxis.ConvWidth].MoveIndex((int)tePosAlign.READY);
								break;
							}
							//언로드 동작 중 정지 시
							if (SM.FMT.IsFlowRange(FlowTray.ULDConv01, FlowTray.ULDConv31)  == true  &&
								G.cKernel.IsAIn(DI.ULDTrayConvExistSen) == false)
							{
								smm.Next(teCmd.HOME_91);
								break;
							}

							smm.Next(teCmd.HOME_100);
						}
						break;
						

					case teCmd.HOME_91:
						{
							if (G.seqUnloadTransfer.UnloadConvMoveSafety() == false)			break;

								//속도가 동일해야 함으로, Left속도를 동일하게 적용
							if (arrMt[teAxis.ConvLeft].IsRdy() == true  ||  arrMt[teAxis.ConvLeft].IsRdy() == true)
							{
								arrMt[teAxis.ConvLeft].MoveJog(false, arrMt[teAxis.ConvLeft].GetVel((int)tePosConv.UNLOAD_SLOW));
								arrMt[teAxis.ConvRight].MoveJog(false, arrMt[teAxis.ConvLeft].GetVel((int)tePosConv.UNLOAD_SLOW));
							}

							if (G.DryRunIsAIn(DI.ULDTrayConvExistSen, true) == false)					break;
						

							smm.Next(teCmd.HOME_92);
						}
						break;
							

					case teCmd.HOME_92:
						{
							if (smm.Delayms(100) == false)			break;

							arrMt[teAxis.ConvLeft].Stop();
							arrMt[teAxis.ConvRight].Stop();
							
							smm.Next(teCmd.HOME_100);
						}
						break;
							

					case teCmd.HOME_100:
						{
							//메모리엔 리프트에 자재가 있고, 센서에 감지되지 않는다면, 언로더는 그냥 플로우 날려도 무방한듯
							if (SM.FMT.IsFlowRange(FlowTray.ULDLift01, FlowTray.ULDLift31) == true  &&
								G.DryRunIsAIn(DI.ULDTrayLiftExistSen, true) == false)
							{
								SM.SHM.RemoveRunLotInfo(0);
								SM.SHM.RemoveLotYield(SM.FMT[FlowTray.ULDLift01].GetInfo().KeyValue);
								SM.SHM.ClearFiduErrorCount(SM.FMT[FlowTray.ULDLift01].GetInfo().KeyValue);
								SM.FMT.FlowClearRange(FlowTray.ULDLift01, FlowTray.ULDLift31);

								if (G.AlmMng.Find(ALM.WARNING_TRAY_UNLOAD_END_QUIT_LOT) == true ||
									G.AlmMng.Find(ALM.WARNING_TRAY_UNLOAD_END) == true)
								{
									G.AlmMng.Del(ALM.WARNING_TRAY_UNLOAD_END);
									G.AlmMng.Del(ALM.WARNING_TRAY_UNLOAD_END_QUIT_LOT);
									G.commInspTop.SendPopupMsgAllCloseReq();
									G.commInspBtm.SendPopupMsgAllCloseReq();
								}
								break;
							}

							smm.Next(teCmd.HOME_120);
						}
						break;

					case teCmd.HOME_120:
						{
							if (arrAct[teAct.FrontStopper].IsPos(SenPos.FWD) == false)
							{
								arrAct[teAct.FrontStopper].SetPos(SenPos.FWD);
								break;
							}
							if (SM.FMT.IsFlowRange(FlowTray.ULDLift01, FlowTray.ULDLift31) == true )
							{
								if (arrMt[teAxis.TrayLift].IsRdy((int)tePosLift.DOWN) == false)
								{
									arrMt[teAxis.TrayLift].MoveIndex((int)tePosLift.DOWN);
									break;
								}
							}
							else
							{
								if (arrMt[teAxis.TrayLift].IsRdy((int)tePosLift.UP) == false)
								{
									arrMt[teAxis.TrayLift].MoveIndex((int)tePosLift.UP);
									break;
								}
							}

							smm.Next(teCmd.END);
						}
						break;

					case teCmd.END:
						{
							smm.Next(teCmd.START);
						}
						break;
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void Unload(SMM<teCmd> smm)
		{
			try
			{
				switch (smm.CurStep)
				{
					case teCmd.START:
						{
							if (smm.eInput == InState.run)
							{
								smm.eInput = InState.none;

								smm.Next(teCmd.UNLOAD_10);
							}
						}
						break;

					case teCmd.UNLOAD_10:
						{
							if (G.DryRunIsAIn(DI.ULDTrayLiftExistSen,false) == true)
							{
								AddAlarm(ALM.ERROR_SEQ_UNLOADER_LIFT_TRAY_EXIST);
								smm.Next(teCmd.END);
								break;
							}

                            if (G.DryRunIsBIn(DI.ULDTrayLiftAreaSen1, false) == true)
                            {
                                AddAlarm(ALM.ERROR_SEQ_UNLOADER_LIFT_AREA_SENSOR1_CHECK);
                                smm.Next(teCmd.END);
                                break;
                            }
                            if (G.DryRunIsBIn(DI.ULDTrayLiftAreaSen2, false) == true)
                            {
                                AddAlarm(ALM.ERROR_SEQ_UNLOADER_LIFT_AREA_SENSOR2_CHECK);
                                smm.Next(teCmd.END);
                                break;
                            }
                            if (G.cKernel.IsAIn(DI.ULDTrayLiftExistSen) == true)
							{
								AddAlarm(ALM.ERROR_SEQ_UNLOADER_LIFT_EXIST_SENSOR_CHECK);
								smm.Next(teCmd.END);
								break;
							}

							smm.Next(teCmd.UNLOAD_20);
						}
						break;

					case teCmd.UNLOAD_20:
						{
							if (G.DryRunIsAIn(DI.ULDTrayConvExistSen, true) == false)
							{
								AddAlarm(ALM.ERROR_SEQ_UNLOADER_CONV_TRAY_NONE);
								smm.Next(teCmd.END);
								break;
							}

							if (arrMt[teAxis.TrayLift].IsBiggerThan((int)tePosLift.DOWN) == true)
							{
								AddAlarm(ALM.ERROR_SEQ_UNLOADER_LIFT_MOTOR_POSITION_CHECK);
							}
							m_smmAlign.eInput =  InState.run;
							smm.Next(teCmd.UNLOAD_30);
						}
						break;

					case teCmd.UNLOAD_30:
						{
							if (IsAlignReady() == false)			break;

							if (arrAct[teAct.FrontStopper].IsPos(SenPos.BWD) == false)
							{
								arrAct[teAct.FrontStopper].SetPos(SenPos.BWD);
								break;
							}

							smm.Next(teCmd.UNLOAD_32);
						}
						break;

					case teCmd.UNLOAD_32:
						{
							//제일 마지막에 들어간 트레이가 ULDConv01
							FlowTray FirstTray = FlowTray.ULDConv01;
							for (int i = (int)FlowTray.ULDConv31; i >= (int)FlowTray.ULDConv01; i--)
							{
								if ( SM.FMT.IsFlow((FlowTray)i) == true)
								{
									if (SM.FMT[(FlowTray)i].GetExistCount() > 0)//자재가 들어가야 해당 정보도 들어감
									{
										FirstTray = (FlowTray)i;
										break;
									}
								}
							}


							//TEST 로그용
							int nCount = 0;
							for (int i = (int)FlowTray.ULDConv31; i >= (int)FlowTray.ULDConv01; i--)
							{
								if ( SM.FMT.IsFlow((FlowTray)i) == true)		nCount++;
							}
							UI.Logs.Write(LOGPART.DATA, $"TEST LOG - TRAY Count = {nCount}, Prod Count = {SM.FMT[FirstTray].GetInfo().MaxCntProdInLot}");

							
							string	strcur_lot_id		= SM.FMT[FirstTray].GetInfo().WorkOrder;
							string	strcur_Key_Value	= SM.FMT[FirstTray].GetInfo().KeyValue;
							int[]	nyield			= new int[3] { 0, 0, 0 };			//ok , ng, user-ok

							if (SM.SHM.GetLotYield(strcur_Key_Value, ref nyield) == true)
							{
								string	str		= string.Format("[YIELD INFO] LOT ID = {0}, KEY VALUE = {6}, QTY = {5} ,TOTAL = {4}, OK = {1} , NG = {2} , USER OK = {3}", strcur_lot_id, nyield[0], nyield[1], nyield[2], nyield[0] + nyield[1] + nyield[2], SM.FMT[FirstTray].GetInfo().MaxCntProdInLot, strcur_Key_Value);
								str += $", Run Mode = {(AutoRunMode)NDm.Read(eNDM.AutoRunMode)} ";
								GLog.Add(str);
								UI.Logs.Write(LOGPART.DATA, str);
							}
							else
							{
								if (NDm.Read(eNDM.AutoRunMode) != (int)AutoRunMode.EndLot  &&  NDm.Read(eNDM.AutoRunMode) != (int)AutoRunMode.QuitLot)
									AddAlarm(ALM.ERROR_SESCGEM_DATA);

								string str = string.Format("[YIELD INFO EMPTY!] LOT ID = {0}, KEY VALUE = {1}", strcur_lot_id, strcur_Key_Value);
								GLog.Add(str);
								UI.Logs.Write(LOGPART.DATA, str);
							}

							smm.Next(teCmd.UNLOAD_33);
						}
						break;

					case teCmd.UNLOAD_33:
						{
							m_nMesCount = 0;
							if (NDm.Read(eNDM.WritePvi) == 0)
							{
								smm.Next(teCmd.UNLOAD_40);
								break;
							}

							bool bOKExist = false;
							//if (NDm.Read(eNDM.AutoRunMode) != (int)teAutoRunMode.Bypass)
							for (FlowTray i = FlowTray.ULDConv01; i < FlowTray.ULDLift01; i++)
							{
								if (SM.FMT.IsFlow(i) == false)	continue;
								
								if (SM.FMT[i].IsResultExist(ProdStatus.OK) == true)
								{
									bOKExist = true;
									break;
								}
							}
							//QuitLot 진행으로 양품없는 경우 pvi 쓰지 않음
							if (bOKExist == false  && NDm.Read(eNDM.AutoRunMode) == (int)AutoRunMode.QuitLot )
							{
								smm.Next(teCmd.UNLOAD_40);
							}
							else
							{
								smm.Next(teCmd.UNLOAD_GROUPNAMECHECK1);
							}

						}
						break;

					case teCmd.UNLOAD_GROUPNAMECHECK1 :
						{
							string strDeviceID_Top = SM.FMT[FlowTray.ULDConv01].GetInfo().ProgramID_Top;
							string strDeviceID_Btm = SM.FMT[FlowTray.ULDConv01].GetInfo().ProgramID_Btm;
							string strMsg = $"{strDeviceID_Top}@{strDeviceID_Btm}";
							if (SM.SHM.GetUseSkipData(USESKIP.TopInspection) == true)
							{
								G.commInspTop.SendGroupNameRequest(strMsg);
							}
							if (SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == true)
							{
								G.commInspBtm.SendGroupNameRequest(strMsg);
							}

							smm.Next(teCmd.UNLOAD_GROUPNAMECHECK2);
						}
						break;
						
					case teCmd.UNLOAD_GROUPNAMECHECK2 : //상부 체크
						{
							if (SM.SHM.GetUseSkipData(USESKIP.TopInspection) == false)
							{
								m_strGroupName[0] = string.Empty;
								smm.Next(teCmd.UNLOAD_GROUPNAMECHECK3);
								break;
							}
						
							int		nret	= G.commInspTop.RcvGroupNameReport(ref m_strGroupName[0]);
							
							if (nret > 0)
							{
								smm.Next(teCmd.UNLOAD_GROUPNAMECHECK3);
							}
							else if (smm.Delayms(5000) == true)
							{
								G.AlmMng.ADD(ALM.ERROR_SEQ_INSP_TOP_PC_COMM_TIMEOUT);
								smm.Next(teCmd.UNLOAD_GROUPNAMECHECK1);
							}
						}
						break;
						
					case teCmd.UNLOAD_GROUPNAMECHECK3 : //하부 체크
						{
							if (SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == false)
							{
								m_strGroupName[1] = string.Empty;
								smm.Next(teCmd.UNLOAD_GROUPNAMECHECK4);
								break;
							}
						
							int		nret	= G.commInspBtm.RcvGroupNameReport(ref m_strGroupName[1]);
							
							if (nret > 0)
							{
								smm.Next(teCmd.UNLOAD_GROUPNAMECHECK4);
							}
							else if (smm.Delayms(5000) == true)
							{
								G.AlmMng.ADD(ALM.ERROR_SEQ_INSP_BTM_PC_COMM_TIMEOUT);
								smm.Next(teCmd.UNLOAD_GROUPNAMECHECK1);
							}
						}
						break;
						
					case teCmd.UNLOAD_GROUPNAMECHECK4 :
						{
							if (SM.SHM.GetUseSkipData(USESKIP.TopInspection) == true &&
								SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == true)
							{
								if (m_strGroupName[0] != m_strGroupName[1])
								{
									AddAlarm(ALM.ERROR_SEQ_INSP_TOP_BTM_PC_GROUP_NAME_DIFFERENT);
									smm.Next(teCmd.UNLOAD_GROUPNAMECHECK1);
									break;
								}
								else
								{
									SDm.Write(eSDM.PVI_GroupName, m_strGroupName[0]);
								}
							}
							else if (SM.SHM.GetUseSkipData(USESKIP.TopInspection) == true)
							{
								SDm.Write(eSDM.PVI_GroupName, m_strGroupName[0]);
							}
							else if (SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == true)
							{
								SDm.Write(eSDM.PVI_GroupName, m_strGroupName[1]);
							}

							m_PVI.WritePVIStart();
							smm.Next(teCmd.UNLOAD_34);
						}
						break;


					case teCmd.UNLOAD_34 :
						{
							if (m_PVI.WritePVIResult() == PVI.WriteResult.Doing)			break;

							if (m_PVI.WritePVIResult() == PVI.WriteResult.EndNG)
							{
								smm.Next(teCmd.UNLOAD_33);
								break;
							}
							
								smm.Next(teCmd.UNLOAD_40);
						}
						break;

					case teCmd.UNLOAD_40:
						{
							if (arrMt[teAxis.ConvLeft].IsRdy() == true  ||
								arrMt[teAxis.ConvRight].IsRdy() == true )
							{
								arrMt[teAxis.ConvLeft].MoveJog(false, arrMt[teAxis.ConvLeft].GetVel((int)tePosConv.UNLOAD_FAST));
								arrMt[teAxis.ConvRight].MoveJog(false, arrMt[teAxis.ConvLeft].GetVel((int)tePosConv.UNLOAD_FAST));
							}

							if (G.DryRunIsAIn(DI.ULDTrayLiftDecSen, true) == false)			break;
							
							arrMt[teAxis.ConvLeft].MoveJog(false, arrMt[teAxis.ConvLeft].GetVel((int)tePosConv.UNLOAD_SLOW));
							arrMt[teAxis.ConvRight].MoveJog(false, arrMt[teAxis.ConvLeft].GetVel((int)tePosConv.UNLOAD_SLOW));

							smm.Next(teCmd.UNLOAD_50);
						}
						break;

					case teCmd.UNLOAD_50:
						{
							if (arrMt[teAxis.ConvLeft].IsRdy() == true  ||  arrMt[teAxis.ConvRight].IsRdy() == true )
							{
								arrMt[teAxis.ConvLeft].MoveJog(false, arrMt[teAxis.ConvLeft].GetVel((int)tePosConv.UNLOAD_SLOW));
								arrMt[teAxis.ConvRight].MoveJog(false, arrMt[teAxis.ConvLeft].GetVel((int)tePosConv.UNLOAD_SLOW));
							}

							if (G.DryRunIsAIn(DI.ULDTrayLiftExistSen,true) == false)			break;

							nStopDelay = SM.SHM.GetRecipeDataToInt(DEVICE.TrayUnloaderConvUnloadDelay)/1000;

							smm.Next(teCmd.UNLOAD_60);
						}
						break;

					case teCmd.UNLOAD_60:
                        {
                            if (arrMt[teAxis.ConvLeft].IsRdy() == true || arrMt[teAxis.ConvRight].IsRdy() == true)
                            {
                                arrMt[teAxis.ConvLeft].MoveJog(false, arrMt[teAxis.ConvLeft].GetVel((int)tePosConv.UNLOAD_SLOW));
                                arrMt[teAxis.ConvRight].MoveJog(false, arrMt[teAxis.ConvLeft].GetVel((int)tePosConv.UNLOAD_SLOW));
                            }

                            if (smm.Delayms(nStopDelay) == false)			break;
                            
							smm.Next(teCmd.UNLOAD_61);
						}
						break;


                    case teCmd.UNLOAD_61:
                        {

                            arrMt[teAxis.ConvLeft].Stop();
                            arrMt[teAxis.ConvRight].Stop();

                            SM.FMT.NextLot(FlowTray.ULDConv01, FlowTray.ULDLift01);

                            smm.Next(teCmd.UNLOAD_70);
                        }
                        break;
                    case teCmd.UNLOAD_70:
						{
							if (IsLiftUpEnable() == false)			break;

							m_smmLiftUp.eInput = InState.run;

							smm.Next(teCmd.END);
						}
						break;

					case teCmd.END:
						{
							smm.Next(teCmd.START);
						}
						break;
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void LiftUp(SMM<teCmd> smm)
		{
			try
			{
				switch (smm.CurStep)
				{
					case teCmd.START:
						{
							if (smm.eInput == InState.run)
							{
								smm.eInput = InState.none;

								smm.Next(teCmd.LIFTUP_10);
							}
                        }
						break;

					case teCmd.LIFTUP_10:
                        {
                            if (G.DryRunIsBIn(DI.ULDTrayLiftAreaSen1, false) == true)
                            {
                                AddAlarm(ALM.ERROR_SEQ_UNLOADER_LIFT_AREA_SENSOR1_CHECK);
                                smm.Next(teCmd.END);
                                break;
                            }

                            if (G.DryRunIsBIn(DI.ULDTrayLiftAreaSen2, false) == true)
                            {
                                AddAlarm(ALM.ERROR_SEQ_UNLOADER_LIFT_AREA_SENSOR2_CHECK);
                                smm.Next(teCmd.END);
                                break;
                            }

                            if (arrMt[teAxis.TrayLift].IsRdy() == false)
								break;

							smm.Next(teCmd.LIFTUP_MESCHECK1);
						}
						break;
						
					case teCmd.LIFTUP_MESCHECK1:
						{
							if (SM.SHM.GetUseSkipData(USESKIP.SGEM) == false  || 
								SM.FMT.IsFlow(FlowTray.ULDLift01) == false || 
								SM.FMT[FlowTray.Reject].GetInfo().WorkOrder == SM.SHM.GetRunLotInfo(1).WorkOrder)//0은 현재 랏, 1은 다음 랏
							{
								smm.Next(teCmd.LIFTUP_20);
								break;
							}

							G.commUbiGEM.SendLotEnd();

							smm.Next(teCmd.LIFTUP_MESCHECK2);
						}
						break;
						
					case teCmd.LIFTUP_MESCHECK2:
						{
							if (SM.SHM.GetUseSkipData(USESKIP.SGEM) == false)
							{
								smm.Next(teCmd.LIFTUP_20);
								break;
							}
							
							int nAck = -1;
							int nret = G.commUbiGEM.RecvAck6(REQ.LOT_END, ref nAck);
							if (nret > 0)
							{
								//ack - Accepted
								if (nAck == 0)
								{
									smm.Next(teCmd.LIFTUP_20);
								}
								else//ack - ErrorNotAccepted
								{
									AddAlarm(ALM.ERROR_SESCGEM_VALIDATION_ACK_ERROR);
									smm.Next(teCmd.LIFTUP_MESCHECK1);
								}
							}
							else if (smm.Delayms(NDm.Read(eNDM.SecsGemCommTimeOut)) == true)
							{
								AddAlarm(ALM.ERROR_SESCGEM_TIMEOUT);
								smm.Next(teCmd.LIFTUP_MESCHECK1);
							}
							smm.Next(teCmd.LIFTUP_20);
						}
						break;
					
					case teCmd.LIFTUP_20:
						{
							if (arrAct[teAct.FrontStopper].IsPos(SenPos.FWD) == false)
							{
								arrAct[teAct.FrontStopper].SetPos(SenPos.FWD);
								break;
							}

							smm.Next(teCmd.LIFTUP_30);
						}
						break;

					case teCmd.LIFTUP_30:       //감지되거나 up까지
						{
							if (G.DryRunIsBIn(DI.ULDTrayLiftAreaSen2, false) == true)
							{
								arrMt[teAxis.TrayLift].Stop();
								//감지한 경우 그 위치에서 5개 높이 만큼 이동 
								int nTrayHeight = SM.SHM.GetRecipeDataToInt(DEVICE.TrayHeight);
								m_nLiftTartgetPos = arrMt[teAxis.TrayLift].GetLocalPos() + nTrayHeight * 5 - 90000;

								if (m_nLiftTartgetPos >= arrMt[teAxis.TrayLift].GetIndexPos((int)tePosLift.UP))
									m_nLiftTartgetPos = arrMt[teAxis.TrayLift].GetIndexPos((int)tePosLift.UP);

								smm.Next(teCmd.LIFTUP_40);
								break;
							}

							if (G.DryRunIsBIn(DI.ULDTrayLiftAreaSen2, false) == false && arrMt[teAxis.TrayLift].IsRdy((int)tePosLift.UP) == false)
							{
								arrMt[teAxis.TrayLift].MoveIndex((int)tePosLift.UP, (int)tePosLift.UP);
								break;
							}

							//UP 위치 까지 간 경우 끝
							if (arrMt[teAxis.TrayLift].IsRdy((int)tePosLift.UP) == true)
							{
								smm.Next(teCmd.END);
								break;
							}
						}
						break;

					case teCmd.LIFTUP_40:
						{
							if (arrMt[teAxis.TrayLift].IsRdyPos(m_nLiftTartgetPos) == false)
							{
								arrMt[teAxis.TrayLift].MoveABS(m_nLiftTartgetPos, (int)tePosLift.UP);
								break;
							}
							smm.Next(teCmd.END);
						}
						break;

					case teCmd.END:
						{
							smm.Next(teCmd.START);
						}
						break;
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void LiftDown(SMM<teCmd> smm)
		{
			try
			{
				switch (smm.CurStep)
				{
					case teCmd.START:
						{
							if (smm.eInput == InState.run)
							{
								smm.eInput = InState.none;

								smm.Next(teCmd.LIFTDOWN_10);
							}
						}
						break;

					case teCmd.LIFTDOWN_10:
						{
							if (arrMt[teAxis.TrayLift].IsRdy() == false)			break;

							smm.Next(teCmd.LIFTDOWN_20);
						}
						break;

					case teCmd.LIFTDOWN_20:
						{
							if (arrAct[teAct.FrontStopper].IsPos(SenPos.FWD) == false)
							{
								arrAct[teAct.FrontStopper].SetPos(SenPos.FWD);
								break;
							}

							smm.Next(teCmd.LIFTDOWN_30);
						}
						break;

					case teCmd.LIFTDOWN_30:
						{
							if (arrMt[teAxis.TrayLift].IsHomeDone() == false)
							{
								arrMt[teAxis.TrayLift].StartHoming();
							}

							smm.Next(teCmd.LIFTDOWN_40);
						}
						break;

					case teCmd.LIFTDOWN_40:
						{
							if (arrMt[teAxis.TrayLift].IsHomeDone() == false)			break;

							if (arrMt[teAxis.TrayLift].IsRdy((int)tePosLift.DOWN) == false)
							{
								arrMt[teAxis.TrayLift].MoveIndex((int)tePosLift.DOWN);
								break;
							}

							smm.Next(teCmd.END);
						}
						break;

					case teCmd.END:
						{
							smm.Next(teCmd.START);
						}
						break;
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void Align(SMM<teCmd> smm)
		{
			try
			{
				switch (smm.CurStep)
				{
					case teCmd.START:
						{
							if (smm.eInput == InState.run)
							{
								smm.eInput = InState.none;

								smm.Next(teCmd.ALIGN_10);
							}
						}
						break;

					case teCmd.ALIGN_10:
						{
							if (G.DryRunIsAIn(DI.ULDTrayConvExistSen, true) == false)
							{
								AddAlarm(ALM.ERROR_SEQ_UNLOADER_CONV_TRAY_NONE);
								smm.Next(teCmd.END);
								break;
							}

							if (G.seqUnloadTransfer.UnloadLiftMoveSafety() == false)			break;

							smm.Next(teCmd.ALIGN_20);
						}
						break;

					case teCmd.ALIGN_20:
						{
							if (arrMt[teAxis.ConvWidth].IsRdy((int)tePosAlign.ALIGN) == false)
							{
								arrMt[teAxis.ConvWidth].MoveIndex((int)tePosAlign.ALIGN);
								break;
							}
							
							smm.Next(teCmd.ALIGN_30);
						}
						break;

					case teCmd.ALIGN_30:
						{
							int nDelay = NDm.Read(eNDM.ULDLiftAlignDelay);
							if (smm.Delayms(nDelay) == false)			break;

							if (arrMt[teAxis.ConvWidth].IsRdy((int)tePosAlign.READY) == false)
							{
								arrMt[teAxis.ConvWidth].MoveIndex((int)tePosAlign.READY);
								break;
							}

							smm.Next(teCmd.END);
						}
						break;
					
					case teCmd.END:
						{
							smm.Next(teCmd.START);
						}
						break;
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
		
		private	void	SWLiftUp(SMM<teCmd> smm)
		{
			try
			{
				switch (smm.CurStep)
				{
					case teCmd.START:
						{
							if (smm.eInput == InState.run)
							{
								smm.eInput = InState.none;

								smm.Next(teCmd.SWLIFTUP_10);
							}
						}
						break;


					case teCmd.SWLIFTUP_10:		
						{
							
							if (G.DryRunIsAIn(DI.ULDTrayLiftExistSen, true) == false)
							{					
								smm.Next(teCmd.END);
								break;
							}

							smm.Next(teCmd.SWLIFTUP_20);
						}
						break;


					case teCmd.SWLIFTUP_20:		//감지되거나, UP 포지션까지 올라감
						{
                            int nTrayHeight = SM.SHM.GetRecipeDataToInt(DEVICE.TrayHeight);
                            m_nLiftTartgetPos = arrMt[teAxis.TrayLift].GetLocalPos() + nTrayHeight * 5;

                            if (m_nLiftTartgetPos >= arrMt[teAxis.TrayLift].GetIndexPos((int)tePosLift.UP))
                            	m_nLiftTartgetPos = arrMt[teAxis.TrayLift].GetIndexPos((int)tePosLift.UP);
                            
                            smm.Next(teCmd.SWLIFTUP_30);
                            break;
                        }
						break;

					case teCmd.SWLIFTUP_30:
						{
							if (arrMt[teAxis.TrayLift].IsRdyPos(m_nLiftTartgetPos) == false)
							{
								arrMt[teAxis.TrayLift].MoveABS(m_nLiftTartgetPos, (int)tePosLift.SWITCHUP);
								break;
							}
							smm.Next(teCmd.END);
						}
						break;

					case teCmd.END:
						{
							smm.Next(teCmd.START);
						}
						break;
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	SWLiftDown(SMM<teCmd> smm)
		{
			try
			{
				switch (smm.CurStep)
				{
					case teCmd.START:
						{
							if (smm.eInput == InState.run)
							{
								smm.eInput = InState.none;

								smm.Next(teCmd.SWLIFTDOWN_10);
							}
						}
						break;


					case teCmd.SWLIFTDOWN_10:		//애초에 감지 안되어 있으면 안 내려감
						{
							
							if (G.DryRunIsBIn(DI.ULDTrayLiftAreaSen2, false) == false  || G.DryRunIsAIn(DI.ULDTrayLiftExistSen, true) == false)
							{					
								smm.Next(teCmd.END);
								break;
							}

							smm.Next(teCmd.SWLIFTDOWN_20);
						}
						break;


					case teCmd.SWLIFTDOWN_20:		//감지안되거나, Down 포지션까지 내려감
						{
							if (G.DryRunIsBIn(DI.ULDTrayLiftAreaSen2, false) == false )
                            {
                                arrMt[teAxis.TrayLift].Stop();
                                //                        //감지한 경우 그 위치에서 조금 더 아래로 이동
                                m_nLiftTartgetPos = arrMt[teAxis.TrayLift].GetLocalPos() - 90000;

                                if (m_nLiftTartgetPos <= arrMt[teAxis.TrayLift].GetIndexPos((int)tePosLift.DOWN))
                                    m_nLiftTartgetPos = arrMt[teAxis.TrayLift].GetIndexPos((int)tePosLift.DOWN);

                                smm.Next(teCmd.SWLIFTDOWN_30);
                                break;
                            }

							if (G.DryRunIsBIn(DI.ULDTrayLiftAreaSen2, false) == true && arrMt[teAxis.TrayLift].IsRdy((int)tePosLift.DOWN) == false)
							{
								arrMt[teAxis.TrayLift].MoveIndex((int)tePosLift.DOWN, (int)tePosLift.SWITCHDOWN);
								break;
							}

                            //down 위치 까지 간 경우 끝
                            if (arrMt[teAxis.TrayLift].IsRdy((int)tePosLift.DOWN) == true)
                            {
                                smm.Next(teCmd.END);
                                break;
                            }
						}
						break;

					case teCmd.SWLIFTDOWN_30:
						{
							if (arrMt[teAxis.TrayLift].IsRdyPos(m_nLiftTartgetPos) == false)
							{
								arrMt[teAxis.TrayLift].MoveABS(m_nLiftTartgetPos, (int)tePosLift.SWITCHDOWN);
								break;
							}
							smm.Next(teCmd.END);
						}
						break;

					case teCmd.END:
						{
							smm.Next(teCmd.START);
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
