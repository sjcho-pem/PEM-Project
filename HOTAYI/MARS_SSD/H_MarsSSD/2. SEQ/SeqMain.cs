using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace H_MarsSSD
{
	public class SeqMain : SeqBase<SeqMain.teAct, SeqMain.teAxis, SeqMain.teCmd>
	{
		#region [ Enum ]
		public enum teAct
		{
			Max,
		}

		public enum teAxis
		{
			Max,
		}

		public enum teCmd
		{
			START = 10,
			END = 99,

			AUTO_RUN1,
		}
		#endregion

		#region [Variables]
		private	SMM<teCmd>	m_smmAutoRun		= new SMM<teCmd>(teCmd.START, "Auto Run");
		private	PStopwatch	m_timeoutLD			= new PStopwatch();
		private	PStopwatch	m_timeoutULD		= new PStopwatch();
		private PStopwatch	m_timeoutULDWarning = new PStopwatch();
		private	PStopwatch	m_BtnDelay			= new PStopwatch();
		private	bool		m_bSwLoadUpOld		= false;
		private	bool		m_bSwLoadDnOld		= false;
		private	bool		m_bSwUnloadUpOld	= false;
		private	bool		m_bSwUnloadDnOld	= false;
		private	bool		m_bMesChecked		= false;
		

		private	Dictionary<string, int>			m_dicBypassNG	= new Dictionary<string, int>();
		private	Dictionary<string, PStopwatch>	m_dicTact		= new Dictionary<string, PStopwatch>();
		private	List<string>					m_ltTactPaused	= new List<string>();

		private	Dictionary<FlowTray, UsePartName>	m_dicTrayUse	= new Dictionary<FlowTray, UsePartName>();
		private	Dictionary<FlowPart, UsePartName>	m_dicPartUse	= new Dictionary<FlowPart, UsePartName>();
		private	List<FlowPart>						m_ltPnpPart		= new List<FlowPart>();
		#endregion



		public	void	Init()
		{
			try
			{
				lstSMM.Add(m_smmAutoRun);

				arrAct = new EnumArray<teAct, Actuator>(teAct.Max);
				arrMt = new EnumArray<teAxis, CMotor>(teAxis.Max);
				
				G.cKernel.AOut(DO.DustControl, true);
				base.Initialize();

				//InitPartUse();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public	bool	AutoRunStart()
		{
			try
			{
				SM.SHM.SetBitData(BIT.AutoRun,	true);
				SM.SHM.SetBitData(BIT.Pause,	false);
				if (G.cKernel.IsAOut(DO.DustControl) == false)			G.cKernel.AOut(DO.DustControl,true);
				
				GLog.Add("Machine AutoRun Start !!");	// LogViewer에서 AutoRun 시작 확인용으로 사용. 때문에 내용 변경하면 안됨.
				SM.SHM.ChangeState(RunStateType.NormalRun);

				TactResumAll();

				return SetSeqRun(ref m_smmAutoRun);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public	void	AutoRunStop()
		{ 
			try
			{
				SM.SHM.SetBitData(BIT.AutoRun,	false);

				this.ResetStep();

				G.seqLoadTrayTable1.ResetStep();
				G.seqLoadTrayTable2.ResetStep();
				G.seqUnloadTrayTable1.ResetStep();
				G.seqUnloadTrayTable2.ResetStep();
				G.seqLoadPnP.ResetStep();
				G.seqUnloadPnP.ResetStep();
				G.seqLoadBuffer1.ResetStep();
				G.seqLoadBuffer2.ResetStep();
				G.seqTrayLoader.ResetStep();
				G.seqLoadTransfer.ResetStep();
				G.seqTrayClean.ResetStep();
				G.seqTrayUnloader.ResetStep();
				G.seqUnloadTransfer.ResetStep();
				G.seqLoadInspPnP.ResetStep();
				G.seqUnloadInspPnP.ResetStep();
				G.seqInspPlate1.ResetStep();
				G.seqInspPlate2.ResetStep();
				G.seqUnloadBuffer.ResetStep();
				G.seqRejectBuffer.ResetStep();
				G.seqJobTeaching.ResetStep();

				SM.SHM.SetBitData(BIT.Pause,			false);
				SM.SHM.SetBitData(BIT.LotLoadStart,		false);
				SM.SHM.SetBitData(BIT.AllHome, false);
				SM.SHM.SetBitData(BIT.InspLock,			false);
				
				G.cKernel.AOut(DO.LDBuf1BlowIonizer,	false);
				G.cKernel.AOut(DO.LDBuf2BlowIonizer,	false);
				G.cKernel.AOut(DO.TrayCleanAirBlow,		false);

				GLog.Add("Machine Stop !!");			// LogViewer에서 AutoRun 중지 확인용으로 사용. 때문에 내용 변경하면 안됨.
				SM.SHM.ChangeState(RunStateType.NormalStop);
				TactStopAll();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public	void	ManualRunStop()
		{
			try
			{
				this.ResetStep();

				if (G.seqMain.IsReady == false)					G.seqMain.ResetStep();
				if (G.seqTrayLoader.IsReady == false)			G.seqTrayLoader.ResetStep();
				if (G.seqTrayUnloader.IsReady == false)			G.seqTrayUnloader.ResetStep();
				if (G.seqTrayClean.IsReady == false)			G.seqTrayClean.ResetStep();
				if (G.seqLoadTrayTable1.IsReady == false)		G.seqLoadTrayTable1.ResetStep();
				if (G.seqLoadTrayTable2.IsReady == false)		G.seqLoadTrayTable2.ResetStep();
				if (G.seqUnloadTrayTable1.IsReady == false)		G.seqUnloadTrayTable1.ResetStep();
				if (G.seqUnloadTrayTable2.IsReady == false)		G.seqUnloadTrayTable2.ResetStep();
				if (G.seqLoadTransfer.IsReady == false)			G.seqLoadTransfer.ResetStep();
				if (G.seqUnloadTransfer.IsReady == false)		G.seqUnloadTransfer.ResetStep();
				if (G.seqLoadPnP.IsReady == false)				G.seqLoadPnP.ResetStep();
				if (G.seqLoadBuffer1.IsReady == false)			G.seqLoadBuffer1.ResetStep();
				if (G.seqLoadBuffer2.IsReady == false)			G.seqLoadBuffer2.ResetStep();
				if (G.seqInspPlate1.IsReady == false)			G.seqInspPlate1.ResetStep();
				if (G.seqInspPlate2.IsReady == false)			G.seqInspPlate2.ResetStep();
				if (G.seqLoadInspPnP.IsReady == false)			G.seqLoadInspPnP.ResetStep();
				if (G.seqUnloadInspPnP.IsReady == false)		G.seqUnloadInspPnP.ResetStep();
				if (G.seqUnloadBuffer.IsReady == false)			G.seqUnloadBuffer.ResetStep();
				if (G.seqUnloadPnP.IsReady == false)			G.seqUnloadPnP.ResetStep();
				if (G.seqRejectBuffer.IsReady == false)			G.seqRejectBuffer.ResetStep();
				if (G.seqJobTeaching.IsReady == false)			G.seqJobTeaching.ResetStep();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public	void	AutoRunPause()
		{ 
			try
			{
				SM.SHM.SetBitData(BIT.AutoRun,	false);
				SM.SHM.SetBitData(BIT.Pause,	true);

				m_smmAutoRun.ResetStep();
				SM.SHM.ChangeState(RunStateType.NormalStop);

				TactPauseAll();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	bool	SetSeqRun(ref SMM<teCmd> smm)
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

		public	void	Run()
		{
			try
			{
				UpdateTact();

				Monitor();

				LiftSwitch();
				if (SM.SHM.IsBitData(BIT.AutoRun) == false && SM.SHM.IsBitData(BIT.Pause) == false)
				{
					if (SM.SHM.IsBitData(BIT.LotLoadStart) == true) SM.SHM.SetBitData(BIT.LotLoadStart, false);
				}


				BuzzerSwitch();

				if (SM.SHM.IsBitData(BIT.Error) == true || SM.SHM.IsBitData(BIT.Pause) == true)
				{
					if (m_smmAutoRun.IsReady == false) AutoRunPause();
					return;
				}
				if (NDm.Read(eNDM.LotEndMode) != (int)LotEndMode.LotEnd &&
					SDm.Read(eSDM.CurTempKeyValue) != "NONE")
				{
					SDm.Write(eSDM.CurKeyValue, SDm.Read(eSDM.CurTempKeyValue));
					SDm.Write(eSDM.CurTempKeyValue, "NONE");
				}

				AutoRun(m_smmAutoRun);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void AutoRun(SMM<teCmd> smm)
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

								smm.Next(teCmd.AUTO_RUN1);
							}
						}
						break;

					case teCmd.AUTO_RUN1:
						{
							SchRecipeChange();
							if (G.allHome.IsHomeReady == false) break; //레시피 자동 체인지 중 동작 하지 않도록

							SchEndLot();
							SchQuitLot();
							SchTrayLoader();
							SchLoadTransfer();
							SchTrayClean();
							SchLoadPnP();
							SchLoadBuffer();
							SchLoadInspPnp();
							SchInspPlate();
							SchUnloadInspPnp();
							SchUnloadBuffer();
							SchUnloadPnP();
							SchUnloadTransfer();
							SchTrayUnloader();
							SchCycleStop();
							SchRejectBuffer();
							SchLoadtable1();
							SchLoadtable2();
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


		private void SchTrayLoader()
		{
			try
			{
				if (G.seqTrayLoader.IsReady == true)
				{
					if (NDm.Read(eNDM.AutoRunMode) == (int)AutoRunMode.EndLot ||
						NDm.Read(eNDM.AutoRunMode) == (int)AutoRunMode.QuitLot)
					{
						SM.SHM.SetBitData(BIT.LotLoadStart, false);

						if (G.seqTrayLoader.IsUnloadLiftUpEnable() == true &&
							SM.FMT.IsFlowRange(FlowTray.LDLift01, FlowTray.LDLift31) == true)
						{
							G.seqTrayLoader.LiftUpStart(false);
							return;
						}
						if (SM.FMT.IsFlowRange(FlowTray.LDLift01, FlowTray.LDLift31) == false &&
							SM.FMT.IsFlow(FlowTray.LDTrans) == false)
						{
							if (G.seqTrayLoader.IsUnloadConvEnable() == true &&
								G.seqLoadTransfer.LoadConvMoveSafety() == true)
							{
								G.seqTrayLoader.UnloadStart();
								return;
							}
						}
						if (G.seqTrayLoader.IsUnloadLiftDownEnable() == true &&
							 SM.FMT.IsFlowRange(FlowTray.LDConv01, FlowTray.LDConv31) == true)
						{
							if (m_timeoutLD.IsRunning == false) m_timeoutLD.Restart();
							if (m_timeoutLD.IsTimeOverMs(3000) == false) return;

							G.seqTrayLoader.LiftDownStart(false);

							m_timeoutLD.Stop();
							return;
						}

						if (G.DryRunIsAIn(DI.LDTrayLiftExistSen, true) == false &&
							SM.FMT.IsFlowRange(FlowTray.LDLift01, FlowTray.LDLift31) == true)
						{
							if (m_timeoutLD.IsRunning == false) m_timeoutLD.Restart();
							if (m_timeoutLD.IsTimeOverMs(3000) == false) return;

							SM.FMT.FlowClearRange(FlowTray.LDLift01, FlowTray.LDLift31);

							m_timeoutLD.Stop();
							return;
						}

						return;
					}

					if ((SM.FMT.IsFlowRange(FlowTray.LDLift01, FlowTray.LDLift31) == true ||  SM.FMT.IsFlow(FlowTray.LDWaitBuf) == true)  )
					{
						if (G.seqTrayLoader.IsLiftDownEnable() == true &&
							(G.cKernel.IsAIn(DI.LDTrayLiftStartBtn) == true || SM.SHM.IsBitData(BIT.LotLoadStart) == true) &&
							string.IsNullOrEmpty(SM.FMT[FlowTray.LDWaitBuf].GetInfo().WorkOrder) == false)
						{
							SM.SHM.SetBitData(BIT.LotLoadStart, false);
							
							if (SM.FMT.IsFlow(FlowTray.LDWaitBuf))
							{
								if (SM.FMT[FlowTray.LDWaitBuf].GetInfo().WorkOrder == "")			return;
								if (SM.FMT[FlowTray.LDWaitBuf].GetInfo().ProgramID_Top == "")		return;
								if (SM.FMT[FlowTray.LDWaitBuf].GetInfo().ProgramID_Btm == "")		return;
								
								if (SM.FMT.IsFlowRange(FlowTray.LDLift01, FlowTray.LDLift31) == false)
								{
									SM.FMT.NextLot(FlowTray.Empty, FlowTray.LDLift01);
								}

								if (CheckUseTrayEmpty(FlowTray.LDTrans, FlowTray.ULDTable2) == true) 
									SDm.Write(eSDM.CurKeyValue, SM.FMT[FlowTray.LDLift01].GetInfo().KeyValue);
							}
							if (RecipeCheck() == true)
							{
								//Recipe 변경해야하면 더 이상 투입 안함.
								if (NDm.Read(eNDM.RecipeChange) > 0) return;

								G.seqTrayLoader.LiftDownStart();
							}

							return;
						}
						//down중 알람등으로 멈추는 경우 다시 다운 시작
						else if (G.seqTrayLoader.IsLiftDownEnable() == true &&
								SM.FMT.IsFlowRange(FlowTray.LDLift01, FlowTray.LDLift31) == true &&
								G.cKernel.Motor(MT.LDTrayLiftZ).IsRdy((int)SeqTrayLoader.PosZ.DOWN) == false)
						{
							if (RecipeCheck() == true)
							{
								//Recipe 변경해야하면 더 이상 투입 안함.
								if (NDm.Read(eNDM.RecipeChange) > 0) return;

								G.seqTrayLoader.LiftDownStart();
							}
							return;
						}
						if (G.seqTrayLoader.IsLoadEnable() == true &&
							G.seqLoadTransfer.LoadConvMoveSafety() == true)
						{
							if (SM.FMT.GetExistCountRange(FlowTray.LDConv01, FlowTray.LDConv31) > 0) return;
							
							G.seqTrayLoader.LoadStart();
							TactStart(EnumHelper.GetDescription(Tact.LOAD_CONVEYOR_START));
							
							return;
						}
					}

					if (G.cKernel.Motor(MT.LDTrayLiftZ).IsRdy((int)SeqTrayLoader.PosZ.UP) == false &&
						(SM.FMT.IsFlowRange(FlowTray.LDLift01, FlowTray.LDLift31) == false && SM.FMT.IsFlow(FlowTray.LDWaitBuf) == false))
					{
						G.seqTrayLoader.LiftUpStart(true);
						return;
					}
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void SchLoadTransfer()
		{
			try
			{
				if (G.seqLoadTransfer.IsReady == true)
				{
					if (SM.FMT.IsFlow(FlowTray.LDTrans) == true)
					{
						switch (SM.FMT[FlowTray.LDTrans].ProcStatus)
						{
							case ProcStatus.Unknown:
								if (G.seqLoadTrayTable1.IsReady == true &&
									SM.FMT.IsFlow(FlowTray.LDTable1) == false &&
									SM.SHM.IsUsePart(UsePartName.LDTable1, UsePartMode.Load) == true)
								{
									G.seqLoadTransfer.PlaceStart(SeqLoadTransfer.PosX.LOADTABLE1);
									return;
								}
								if (G.seqLoadTrayTable2.IsReady == true &&
								   SM.FMT.IsFlow(FlowTray.LDTable2) == false &&
								   SM.SHM.IsUsePart(UsePartName.LDTable2, UsePartMode.Load) == true)
								{
									G.seqLoadTransfer.PlaceStart(SeqLoadTransfer.PosX.LOADTABLE2);
									return;
								}
								break;

							case ProcStatus.TrayEmptyDone:
								if (G.seqTrayClean.IsReady == true &&
									G.seqTrayClean.IsLoadEnable() == true)
								{
									G.seqLoadTransfer.PlaceStart(SeqLoadTransfer.PosX.TRAYCLEAN);
								}
								break;
						}

						return;
					}

					if ((NDm.Read(eNDM.AutoRunMode) == (int)AutoRunMode.EndLot ||
						 NDm.Read(eNDM.AutoRunMode) == (int)AutoRunMode.QuitLot) &&
						 G.seqLoadTransfer.LoadConvMoveSafety() == false)
					{
						G.seqLoadTransfer.ReadyStart();
						return;
					}
					//check 추후 로드 테이블 -> 컨베이어?


					//quit lot 진행시 언로드부에 랏이 있으면 트랜스퍼 진행 안함
					if (NDm.Read(eNDM.AutoRunMode) == (int)AutoRunMode.QuitLot &&
						CheckUseTrayEmpty(FlowTray.ULDTrans, FlowTray.ULDTable2) == false) return;

					if (G.seqLoadTrayTable1.IsReady == true &&
						SM.FMT.IsFlow(FlowTray.LDTable1) == true &&
						SM.FMT[FlowTray.LDTable1].ProcStatus == ProcStatus.TrayEmptyDone &&
						G.seqTrayClean.IsLoadEnable() == true &&
						SM.SHM.IsUsePart(UsePartName.LDTable1, UsePartMode.Unload) == true)
					{
						G.seqLoadTransfer.PnPStart(SeqLoadTransfer.PosX.LOADTABLE1, SeqLoadTransfer.PosX.TRAYCLEAN);
						return;
					}

					if (G.seqLoadTrayTable2.IsReady == true &&
						SM.FMT.IsFlow(FlowTray.LDTable2) == true &&
						SM.FMT[FlowTray.LDTable2].ProcStatus == ProcStatus.TrayEmptyDone &&
						G.seqTrayClean.IsLoadEnable() == true &&
						SM.SHM.IsUsePart(UsePartName.LDTable2, UsePartMode.Unload) == true)
					{
						G.seqLoadTransfer.PnPStart(SeqLoadTransfer.PosX.LOADTABLE2, SeqLoadTransfer.PosX.TRAYCLEAN);
						return;
					}

					if (G.seqTrayLoader.IsConvReady() == true &&
						G.seqTrayLoader.IsTrayTransferEnable() == true &&
						NDm.Read(eNDM.AutoRunMode) != (int)AutoRunMode.EndLot &&
						NDm.Read(eNDM.AutoRunMode) != (int)AutoRunMode.QuitLot )
					{
						
						if (SM.FMT.IsFlow(FlowTray.LDConv01) && SM.FMT[FlowTray.LDConv01].ProcStatus == ProcStatus.TrayEmptyDone)
						{
							if (G.seqTrayClean.IsLoadEnable() == true)
							{
								G.seqLoadTransfer.PnPStart(SeqLoadTransfer.PosX.LOADCONV, SeqLoadTransfer.PosX.TRAYCLEAN);
								TactStart(EnumHelper.GetDescription(Tact.TRAY_LOAD_START));
								return;
							}

							//연속 랏중 트레이 클린에 트레이가 있어서 다음 트레이가 못들어오는 경우, 빈 트레이라도 테이블로 먼저 이동
							if (G.seqLoadTrayTable2.IsReady == true &&
							SM.FMT.IsFlow(FlowTray.LDTable2) == false &&
							SM.SHM.IsUsePart(UsePartName.LDTable2, UsePartMode.Load) == true)
							{
								G.seqLoadTransfer.PnPStart(SeqLoadTransfer.PosX.LOADCONV, SeqLoadTransfer.PosX.LOADTABLE2);
								TactStart(EnumHelper.GetDescription(Tact.TRAY_LOAD_START));
								return;
							}

							if (G.seqLoadTrayTable1.IsReady == true &&
							SM.FMT.IsFlow(FlowTray.LDTable1) == false &&
							SM.SHM.IsUsePart(UsePartName.LDTable1, UsePartMode.Load) == true)
							{
								G.seqLoadTransfer.PnPStart(SeqLoadTransfer.PosX.LOADCONV, SeqLoadTransfer.PosX.LOADTABLE1);
								TactStart(EnumHelper.GetDescription(Tact.TRAY_LOAD_START));
								return;
							}

							return;
						}
						


						if (G.seqLoadTrayTable2.IsReady == true &&
							SM.FMT.IsFlow(FlowTray.LDTable2) == false &&
							SM.SHM.IsUsePart(UsePartName.LDTable2, UsePartMode.Load) == true)
						{
							G.seqLoadTransfer.PnPStart(SeqLoadTransfer.PosX.LOADCONV, SeqLoadTransfer.PosX.LOADTABLE2);
							TactStart(EnumHelper.GetDescription(Tact.TRAY_LOAD_START));
							return;
						}
						else if (G.seqLoadTrayTable1.IsReady == true &&
								 SM.FMT.IsFlow(FlowTray.LDTable1) == false &&
								 SM.SHM.IsUsePart(UsePartName.LDTable1, UsePartMode.Load) == true)
						{
							G.seqLoadTransfer.PnPStart(SeqLoadTransfer.PosX.LOADCONV, SeqLoadTransfer.PosX.LOADTABLE1);
							TactStart(EnumHelper.GetDescription(Tact.TRAY_LOAD_START));
							return;
						}
					}
					
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void SchTrayClean()
		{
			try
			{
				if (G.seqTrayClean.IsReady == true &&
					 NDm.Read(eNDM.AutoRunMode) != (int)AutoRunMode.QuitLot)
				{
					if (SM.FMT.IsFlow(FlowTray.TrayClean) == true &&
						SM.FMT[FlowTray.TrayClean].ProcStatus != ProcStatus.CleanDone)
					{
						if (SM.FMT[FlowTray.TrayClean].GetExistCount() == 0) G.seqTrayClean.TrayCleanStart();
						else SM.FMT[FlowTray.TrayClean].ProcStatus = ProcStatus.CleanDone;
					}
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void SchLoadPnP()
		{
			try
			{
				if (G.seqLoadPnP.IsReady == true)
				{

					if (SM.FMP[FlowPart.LDPnP].GetExistCount() > 0 &&
						 (SM.FMP[FlowPart.LDPnP].GetInfo().LastPieceOfLot == true ||
						  NDm.Read(eNDM.AutoRunMode) == (int)AutoRunMode.EndLot ||
						  NDm.Read(eNDM.AutoRunMode) == (int)AutoRunMode.QuitLot))
					{
						G.seqLoadPnP.PlaceBufStart();
						return;
					}

					if (G.seqLoadPnP.CheckLoadPnPFull() == true)
					{
						G.seqLoadPnP.PlaceBufStart();
						return;
					}

					if (NDm.Read(eNDM.AutoRunMode) == (int)AutoRunMode.QuitLot)
						return;

					//TrayEmptyDone이 필요하면 먼저 할 수 있도록 수정
					if (SM.FMT.IsFlow(FlowTray.LDTable1) &&
						SM.FMT[FlowTray.LDTable1].GetExistCount() == 0 &&
						SM.FMT[FlowTray.LDTable1].ProcStatus != ProcStatus.TrayEmptyDone)
					{
						if (G.seqLoadTrayTable1.IsReady == true &&
							G.seqLoadTrayTable1.IsPnPEnable() == true )
						{
							G.seqLoadPnP.EmptyCheckStart(FlowTray.LDTable1);
						}

						if (SM.FMT[FlowTray.LDTable1].GetInfo().KeyValue == SM.FMT[FlowTray.LDTable2].GetInfo().KeyValue )
						{
							return;
						}
					}

					if (SM.FMT.IsFlow(FlowTray.LDTable2) &&
						SM.FMT[FlowTray.LDTable2].GetExistCount() == 0 &&
						SM.FMT[FlowTray.LDTable2].ProcStatus != ProcStatus.TrayEmptyDone)
					{
						if (G.seqLoadTrayTable2.IsReady == true &&
							G.seqLoadTrayTable2.IsEmptyCheckEnable()  )
						{
							G.seqLoadPnP.EmptyCheckStart(FlowTray.LDTable2);
						}
						if (SM.FMT[FlowTray.LDTable1].GetInfo().KeyValue == SM.FMT[FlowTray.LDTable2].GetInfo().KeyValue )
						{
							return;
						}
					}


					if (NDm.Read(eNDM.AutoRunMode) == (int)AutoRunMode.CycleStop)		return;
					if (SM.SHM.GetUseSkipData(USESKIP.LoadPnPStop) == true)			return;
					
					
					if (((SM.FMT.IsFlow(FlowTray.LDTable1) == true && SM.FMT[FlowTray.LDTable1].GetExistCount() > 0 && G.seqLoadTrayTable1.IsPnPEnable() == true) ||
						(SM.FMT.IsFlow(FlowTray.LDTable2) == true && SM.FMT[FlowTray.LDTable2].GetExistCount() > 0 && G.seqLoadTrayTable2.IsPnPEnable() == true)))
					{
						G.seqLoadPnP.PnPStart();
						TactStart(EnumHelper.GetDescription(Tact.LOAD_PnP_START));
						return;
					}

				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void SchLoadBuffer()
		{
			try
			{
				int nPriorityNo = 0;

				if (SM.FMP.IsFlow(FlowPart.LDBuffer1) == true && G.seqLoadBuffer1.IsReady == true &&
					SM.FMP.IsFlow(FlowPart.LDBuffer2) == true && G.seqLoadBuffer2.IsReady == true &&
					SM.SHM.IsUsePart(UsePartName.LDBuf1, UsePartMode.Enable) == true &&
					SM.SHM.IsUsePart(UsePartName.LDBuf2, UsePartMode.Enable) == true)
				{
					nPriorityNo = CheckPriorityProd(FlowPart.LDBuffer1, FlowPart.LDBuffer2);
				}

				if (nPriorityNo == 0 || nPriorityNo == 1)
				{
					if (G.seqLoadBuffer1.IsReady == true)
					{
						if (SM.FMP[FlowPart.LDBuffer1].GetExistCount() > 0 &&
							SM.SHM.IsUsePart(UsePartName.LDBuf1, UsePartMode.Unload) == true)
						{
							if (G.seqLoadBuffer1.IsClean() == true) G.seqLoadBuffer1.UnloadWaitStart();
							else G.seqLoadBuffer1.CleanStart();
						}
						else if (SM.SHM.IsUsePart(UsePartName.LDBuf1, UsePartMode.Load) == true)
						{
							G.seqLoadBuffer1.LoadWaitStart();
						}
					}
				}

				if (nPriorityNo == 0 || nPriorityNo == 2)
				{
					if (G.seqLoadBuffer2.IsReady == true)
					{
						if (SM.FMP[FlowPart.LDBuffer2].GetExistCount() > 0 &&
							SM.SHM.IsUsePart(UsePartName.LDBuf2, UsePartMode.Unload) == true)
						{
							if (G.seqLoadBuffer2.IsClean() == true) G.seqLoadBuffer2.UnloadWaitStart();
							else G.seqLoadBuffer2.CleanStart();
						}
						else if (SM.SHM.IsUsePart(UsePartName.LDBuf2, UsePartMode.Load) == true)
						{
							G.seqLoadBuffer2.LoadWaitStart();
						}
					}
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void SchLoadInspPnp()
		{
			try
			{
				if (G.seqLoadInspPnP.IsReady == true)
				{
					if (SM.FMP[FlowPart.LDInspPnP].GetExistCount() > 0)
					{
						G.seqLoadInspPnP.PlaceStart();
						return;
					}

					//CycleStop일 때는 LDPnP가 제품을 Place 하지 못할 경우 PnP Start
					if (NDm.Read(eNDM.AutoRunMode) == (int)AutoRunMode.CycleStop)
					{
						if (SM.FMP.IsFlow(FlowPart.LDPnP) == true &&
							SM.FMP.IsFlow(FlowPart.LDBuffer1) == true &&
							SM.FMP.IsFlow(FlowPart.LDBuffer2) == true &&
							(G.seqLoadBuffer1.IsULDWait() == true || G.seqLoadBuffer2.IsULDWait() == true))
						{
							G.seqLoadInspPnP.PnPStart();
						}
						return;
					}

					if (SM.SHM.GetUseSkipData(USESKIP.InspDebugPlate1) == true)
					{
						if (G.seqInspPlate1.IsLDWait() == true)				G.seqLoadInspPnP.PnPStart();
						else if (G.seqLoadInspPnP.IsReadyPos() == false)	G.seqLoadInspPnP.ReadyPosStart();
						return;
					}

					if (SM.SHM.GetUseSkipData(USESKIP.InspDebugPlate2) == true)
					{
						if (G.seqInspPlate2.IsLDWait() == true)				G.seqLoadInspPnP.PnPStart();
						else if (G.seqLoadInspPnP.IsReadyPos() == false)	G.seqLoadInspPnP.ReadyPosStart();
						return;
					}

					if (G.seqLoadBuffer1.IsULDWait() == true || G.seqLoadBuffer2.IsULDWait() == true)	G.seqLoadInspPnP.PnPStart();
					else if (G.seqLoadInspPnP.IsReadyPos() == false)									G.seqLoadInspPnP.ReadyPosStart();

				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void SchInspPlate()
		{
			try
			{
				int nPriorityNo = 0;


				if (SM.FMP.IsFlow(FlowPart.Insp1) == true &&
					SM.FMP.IsFlow(FlowPart.Insp2) == true &&
					G.seqInspPlate1.IsReady == true &&
					G.seqInspPlate2.IsReady == true &&
					SM.SHM.IsUsePart(UsePartName.Insp1, UsePartMode.Unload) == true &&
					SM.SHM.IsUsePart(UsePartName.Insp2, UsePartMode.Unload) == true)
				{
					//제품이 양쪽 다 들어있는 경우 시간으로 우선순위 설정
					nPriorityNo = CheckPriorityProd(FlowPart.Insp1, FlowPart.Insp2);

					//만약 마지막 제품 언로드 시 디버그 모드 사용시
					if (SM.FMP[FlowPart.Insp1].GetInfo().LastPieceOfLot == true || SM.FMP[FlowPart.Insp2].GetInfo().LastPieceOfLot == true)
					{
						if (SM.SHM.GetUseSkipData(USESKIP.InspDebugPlate1) == true || SM.SHM.GetUseSkipData(USESKIP.InspDebugPlate2) == true)
						{
							AddAlarm(ALM.ERROR_DEBUG_MODE_IN_LAST_PRODUCT);
							return;
						}
					}
					else if (SM.SHM.GetUseSkipData(USESKIP.InspDebugPlate1) == true)
					{
						nPriorityNo = 0;// 그외엔 해당 디버그 사용중인 플레이트 우선순위
					}
					else if (SM.SHM.GetUseSkipData(USESKIP.InspDebugPlate2) == true)
					{
						nPriorityNo = 1;
					}
				}

				if (nPriorityNo == 0 || nPriorityNo == 1)
				{
					if (G.seqInspPlate1.IsReady == true &&
						SM.SHM.IsUsePart(UsePartName.Insp1, UsePartMode.Unload) == true &&
						SM.SHM.GetUseSkipData(USESKIP.InspDebugPlate2) == false)
					{
						if (SM.FMP[FlowPart.Insp1].GetExistCount() > 0)
						{
							if (G.seqInspPlate1.InspDone == true)
							{
								G.seqInspPlate1.UnloadWaitStart();
							}
							else if (G.seqLoadInspPnP.IsPlateMoveEnable(0) == true)
							{
								G.seqInspPlate1.InspWaitStart();
							}
						}
						else if (SM.SHM.IsBitData(BIT.ReferenceRequestTop1) == true || SM.SHM.IsBitData(BIT.ReferenceRequestBtm1) == true)
						{
							G.seqInspPlate1.ReferenceStart(true, false, 0);
						}
						else if (SM.FMP.IsFlow(FlowPart.LDInspPnP) == true || SM.FMP.IsFlow(FlowPart.LDBuffer1) == true || SM.FMP.IsFlow(FlowPart.LDBuffer2) == true)
						{
							G.seqInspPlate1.LoadWaitStart();
						}
					}
				}

				if (nPriorityNo == 0 || nPriorityNo == 2)
				{
					if (G.seqInspPlate2.IsReady == true &&
						SM.SHM.IsUsePart(UsePartName.Insp2, UsePartMode.Unload) == true &&
						SM.SHM.GetUseSkipData(USESKIP.InspDebugPlate1) == false)
					{
						if (SM.FMP[FlowPart.Insp2].GetExistCount() > 0)
						{
							if (G.seqInspPlate2.InspDone == true)
							{
								G.seqInspPlate2.UnloadWaitStart();
							}
							else if (G.seqLoadInspPnP.IsPlateMoveEnable(1) == true)
							{
								G.seqInspPlate2.InspWaitStart();
							}
						}
						else if (SM.SHM.IsBitData(BIT.ReferenceRequestTop2) == true || SM.SHM.IsBitData(BIT.ReferenceRequestBtm2) == true)
						{
							G.seqInspPlate2.ReferenceStart(true, false, 0);
						}
						else if (SM.FMP.IsFlow(FlowPart.LDInspPnP) == true || SM.FMP.IsFlow(FlowPart.LDBuffer1) == true || SM.FMP.IsFlow(FlowPart.LDBuffer2) == true)
						{
							G.seqInspPlate2.LoadWaitStart();
						}
					}
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void SchUnloadInspPnp()
		{
			try
			{
				if (G.seqUnloadInspPnP.IsReady == true)
				{
					if (SM.FMP[FlowPart.ULDInspPnP].GetExistCount() > 0)
					{
						G.seqUnloadInspPnP.PlaceStart();
						return;
					}
					//CycleStop일 때는 LDinspPnP가 제품을 Place 하지 못할 경우 PnP Start
					if (NDm.Read(eNDM.AutoRunMode) == (int)AutoRunMode.CycleStop)
					{
						if (SM.FMP.IsFlow(FlowPart.LDInspPnP) == true &&
							SM.FMP.IsFlow(FlowPart.Insp1) == true &&
							SM.FMP.IsFlow(FlowPart.Insp2) == true &&
							(G.seqInspPlate1.IsULDWait() == true || G.seqInspPlate2.IsULDWait() == true))
						{
							G.seqUnloadInspPnP.PnPStart();
						}
						return;
					}

					if (G.seqInspPlate1.IsULDWait() == true || G.seqInspPlate2.IsULDWait() == true) G.seqUnloadInspPnP.PnPStart();
					else if (G.seqUnloadInspPnP.IsReadyPos() == false) G.seqUnloadInspPnP.ReadyPosStart();
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void SchUnloadBuffer()
		{
			try
			{
				if (G.seqUnloadBuffer.IsReady == true)
				{
					if (SM.FMP[FlowPart.ULDBuffer].GetExistCount() > 0)
					{
						G.seqUnloadBuffer.UnloadWaitStart();
					}
					else
					{
						G.seqUnloadBuffer.LoadWaitStart();
					}
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void SchUnloadTransfer()
		{
			try
			{
				if (G.seqUnloadTransfer.IsReady == true)
				{
					if (SM.FMT.IsFlow(FlowTray.ULDTrans) == true)
					{
						switch (SM.FMT[FlowTray.ULDTrans].ProcStatus)
						{
							case ProcStatus.CleanDone:
								if (G.seqUnloadTrayTable1.IsReady == true &&
									 SM.FMT.IsFlow(FlowTray.ULDTable1) == false &&
									 SM.SHM.IsUsePart(UsePartName.ULDTable1, UsePartMode.Load) == true)
								{
									G.seqUnloadTransfer.PlaceStart(SeqUnloadTransfer.PosX.UNLOADTABLE1);
								}
								else if (G.seqUnloadTrayTable2.IsReady == true &&
										 SM.FMT.IsFlow(FlowTray.ULDTable2) == false &&
										 SM.SHM.IsUsePart(UsePartName.ULDTable2, UsePartMode.Load) == true)
								{
									G.seqUnloadTransfer.PlaceStart(SeqUnloadTransfer.PosX.UNLOADTABLE2);
								}
								break;
							case ProcStatus.TrayFillDone:
								if (IsUnloadConvLastTrayExist() == false &&
									G.seqTrayUnloader.IsTransLoadEnable() == true &&
									G.seqTrayUnloader.IsUnloadReady() == true)
								{
									G.seqUnloadTransfer.PlaceConvStart();
								}
								break;
						}

						return;
					}

					#region		[ Tray move : Tray Clean -> Unload Tray Table1,2]
					if (G.seqTrayClean.IsReady == true &&
						G.seqTrayClean.IsUnloadEnable() == true &&
						SM.FMT.IsFlow(FlowTray.TrayClean) == true &&
						SM.FMT[FlowTray.TrayClean].ProcStatus == ProcStatus.CleanDone)
					{
						if (G.seqUnloadTrayTable1.IsReady == true &&
							SM.FMT.IsFlow(FlowTray.ULDTable1) == false &&
							SM.SHM.IsUsePart(UsePartName.ULDTable1, UsePartMode.Load) == true)
						{
							G.seqUnloadTransfer.PnPStart(SeqUnloadTransfer.PosX.TRAYCLEAN, SeqUnloadTransfer.PosX.UNLOADTABLE1);
							return;
						}

						if (G.seqUnloadTrayTable2.IsReady == true &&
							SM.FMT.IsFlow(FlowTray.ULDTable2) == false &&
							SM.SHM.IsUsePart(UsePartName.ULDTable2, UsePartMode.Load) == true)
						{
							G.seqUnloadTransfer.PnPStart(SeqUnloadTransfer.PosX.TRAYCLEAN, SeqUnloadTransfer.PosX.UNLOADTABLE2);
							return;
						}
					}
					#endregion

					// Tray 클린에서 Unload 예약되어 있으면 다른 조건은 확인하지 않음
					if (G.seqTrayClean.UnloadResurve == true) return;
					
					#region		[ Tray move : Unload Tray Table1,2 -> Conveyor ]

					SeqUnloadTransfer.PosX eTargetTray = SeqUnloadTransfer.PosX.UNKNOWN;

					bool bTable1Ready = G.seqUnloadTrayTable1.IsReady == true &&
										SM.FMT.IsFlow(FlowTray.ULDTable1) == true &&
										SM.FMT[FlowTray.ULDTable1].ProcStatus == ProcStatus.TrayFillDone &&
										SM.SHM.IsUsePart(UsePartName.ULDTable1, UsePartMode.Unload) == true;

					bool bTable2Ready = G.seqUnloadTrayTable2.IsReady == true &&
										SM.FMT.IsFlow(FlowTray.ULDTable2) == true &&
										SM.FMT[FlowTray.ULDTable2].ProcStatus == ProcStatus.TrayFillDone &&
										SM.SHM.IsUsePart(UsePartName.ULDTable2, UsePartMode.Unload) == true;

					if (bTable1Ready && bTable2Ready)	eTargetTray = CheckPriorityUnloadTray();
					else if (bTable1Ready == true)		eTargetTray = SeqUnloadTransfer.PosX.UNLOADTABLE1;
					else if (bTable2Ready == true)		eTargetTray = SeqUnloadTransfer.PosX.UNLOADTABLE2;

					// 이부분에서 대기 시간이 길어지면 GetRunLotInfo 의 0,1 의 Lid use 가 다른 경우만 대기하도록 수정
					// 아니면 GetCurLotLidUse에서 conv에 자재가 있고 언로드 중이면 1번껄 사용하도록 수정? 이건 좀 위험 할 수도?
					if (eTargetTray != SeqUnloadTransfer.PosX.UNKNOWN &&
						G.seqTrayUnloader.IsUnloadReady() == true)
					{
						if (G.seqTrayUnloader.IsLastTrayExist() == false &&
							G.seqTrayUnloader.IsUnloadReady() == true &&
							SM.FMT.GetExistCountRange(FlowTray.ULDConv01, FlowTray.ULDConv31) < stLotCnt.Max)
						{
							G.seqUnloadTransfer.PnPStart(eTargetTray, SeqUnloadTransfer.PosX.UNLOADCONV);
							return;
						}
					}

					#endregion

				
					G.seqUnloadTransfer.InterLockCheckMoveIndex(SeqUnloadTransfer.PosX.UNLOADCONV);
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void SchUnloadPnP()
		{
			try
			{
				if (NDm.Read(eNDM.RejectRunMode) != (int)RejectRunMode.VerifyReq)
				{
					if (G.AlmMng.Find(ALM.WARNING_VERIFY_REQUEST) == true)
					{
						G.AlmMng.Del(ALM.WARNING_VERIFY_REQUEST);
						SM.SHM.SetBitData(BIT.WarningBuzzer, false);
					}

					if ((Program._bSimul_ == true ||
						SM.SHM.GetUseSkipData(USESKIP.SendRejectProdId) == false) &&
						SM.SHM.IsMsgPopUp(SeqMsg.VerifyReqSel) == true)
					{
						G.commInspTop.SendPopupMsgAllCloseReq();
						G.commInspBtm.SendPopupMsgAllCloseReq();
						SM.SHM.MsgClose(SeqMsg.VerifyReqSel);
					}
					else if (SM.SHM.IsMsgPopUp(SeqMsg.VerifyReq) == true)
					{
						G.commInspTop.SendPopupMsgAllCloseReq();
						G.commInspBtm.SendPopupMsgAllCloseReq();
						SM.SHM.MsgClose(SeqMsg.VerifyReq);
					}
				}

				if (G.seqUnloadPnP.IsReady == true)
				{
					if (NDm.Read(eNDM.RejectRunMode) == (int)RejectRunMode.VerifyReq)
					{
						G.AlmMng.ADD(ALM.WARNING_VERIFY_REQUEST);

						if (Program._bSimul_ == true)
						{
							//시뮬레이션 랏 자동 투입용
							G.Simul.SimVerify("Random");
							//if (SM.SHM.IsMsgPopUp(SeqMsg.VerifyReqSel) == false)
							//{
							//	SM.SHM.MsgShow(SeqMsg.VerifyReqSel);
							//	string strResult = SM.SHM.PopupMsgResult(SeqMsg.VerifyReqSel);
							//	switch (strResult)
							//	{
							//		case "Random":
							//		case "All OK":
							//		case "All NG": G.Simul.SimVerify(strResult); break;
							//	}
							//
							//	G.commInspTop.SendPopupMsgReq(Language.languageDefault.MB_VerifyRequest);
							//	G.commInspBtm.SendPopupMsgReq(Language.languageDefault.MB_VerifyRequest);
							//}
						}
						else if (SM.SHM.GetUseSkipData(USESKIP.SendRejectProdId) == false)
						{
							if (SM.SHM.IsMsgPopUp(SeqMsg.VerifyReqSel) == false)
							{
								SM.SHM.MsgShow(SeqMsg.VerifyReqSel);
								string strResult = SM.SHM.PopupMsgResult(SeqMsg.VerifyReqSel);
								switch (strResult)
								{
									case "Random":
									case "All OK":
									case "All NG": G.Simul.SimVerify(strResult); break;
								}

								G.commInspTop.SendPopupMsgReq(Language.languageDefault.MB_VerifyRequest);
								G.commInspBtm.SendPopupMsgReq(Language.languageDefault.MB_VerifyRequest);

							}
						}
						else
						{
							if (SM.SHM.GetUseSkipData(USESKIP.TopInspection) == false)
							{
								if (SM.FMT.IsFlow(FlowTray.Reject) == false) return;

								var tsInfo = SM.FMT[FlowTray.Reject].GetInfo();
								for (int i = 0; i < tsInfo.MaxCntProdInTray; i++)
								{
									if (tsInfo.ProdInfo[i].Exist == false)
										continue;

									tsInfo.ProdInfo[i].VerifyTopDone = true;
								}
								SM.FMT[FlowTray.Reject].SetInfo(tsInfo);
							}

							if (SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == false)
							{
								if (SM.FMT.IsFlow(FlowTray.Reject) == false) return;

								var tsInfo = SM.FMT[FlowTray.Reject].GetInfo();
								for (int i = 0; i < tsInfo.MaxCntProdInTray; i++)
								{
									if (tsInfo.ProdInfo[i].Exist == false)
										continue;

									tsInfo.ProdInfo[i].VerifyBtmDone = true;
								}
								SM.FMT[FlowTray.Reject].SetInfo(tsInfo);
							}

							if (SM.SHM.IsMsgPopUp(SeqMsg.VerifyReq) == false)
							{
								SM.SHM.MsgShow(SeqMsg.VerifyReq);
								G.commInspTop.SendPopupMsgReq(Language.languageDefault.MB_VerifyRequest);
								G.commInspBtm.SendPopupMsgReq(Language.languageDefault.MB_VerifyRequest);
							}
						}

						if (SM.FMT[FlowTray.Reject].CheckVerifyComplete() == true)
						{
							NDm.Write(eNDM.RejectRunMode, (int)RejectRunMode.VerifyComplete);
						}
						if (SM.FMT[FlowTray.Reject].GetExistCount() == 0 && NDm.Read(eNDM.RejectRunMode) != (int)RejectRunMode.Normal)
						{
							NDm.Write(eNDM.RejectRunMode, (int)RejectRunMode.Normal);
						}
					}

					if (SM.FMP[FlowPart.ULDPnP].GetExistCount() > 0)
					{
						if (SM.FMT[FlowTray.ULDTable1].ProcStatus == ProcStatus.SortRun &&
							SM.FMP[FlowPart.ULDPnP].IsResultExist(ProdStatus.OK) != -1 &&
							G.seqUnloadTrayTable1.IsUpState() == true)
						{
							G.seqUnloadPnP.SortPlaceStart(FlowTray.ULDTable1);
						}
						else if (SM.FMT[FlowTray.ULDTable2].ProcStatus == ProcStatus.SortRun &&
								 SM.FMP[FlowPart.ULDPnP].IsResultExist(ProdStatus.OK) != -1 &&
								G.seqUnloadTrayTable2.IsUpState() == true)
						{
							G.seqUnloadPnP.SortPlaceStart(FlowTray.ULDTable2);
						}
						//Verify된 NG를 들고 있는 경우 무조건 sort Place
						else if (SM.FMP[FlowPart.ULDPnP].IsResultExist(ProdStatus.NG) != -1 &&
								 SM.FMP[FlowPart.ULDPnP].IsNoVerifyExist() == false)
						{
							G.seqUnloadPnP.SortPlaceStart(FlowTray.Reject);
						}
						else if (G.seqRejectBuffer.IsPnPReady() == true)
						{
							G.seqUnloadPnP.PlaceStart();
						}

						return;
					}

					if (G.seqRejectBuffer.IsPnPReady() == false) return;

					if (SM.FMT[FlowTray.Reject].GetExistCount(ProdStatus.OK) >= SM.FMP[FlowPart.ULDPnP].GetMaxProdCount() ||
						(SM.FMT[FlowTray.Reject].GetVerifyDoneCount() == SM.FMT[FlowTray.Reject].GetExistCount() && SM.FMT[FlowTray.Reject].GetExistCount(ProdStatus.OK) > 0))
					{
						G.seqUnloadPnP.PickRejectStart(true);
						return;
					}

					if (SM.FMT.IsFlow(FlowTray.ULDTable1) == true &&
						SM.FMT[FlowTray.ULDTable1].ProcStatus == ProcStatus.SortRun &&
						G.seqUnloadTrayTable1.IsUpState() == true)
					{
						G.seqUnloadPnP.SortStart(FlowTray.ULDTable1);
						return;
					}
					if (SM.FMT.IsFlow(FlowTray.ULDTable2) == true &&
						SM.FMT[FlowTray.ULDTable2].ProcStatus == ProcStatus.SortRun &&
						G.seqUnloadTrayTable2.IsUpState() == true)
					{
						G.seqUnloadPnP.SortStart(FlowTray.ULDTable2);
						return;
					}
					if (SM.FMT.IsFlow(FlowTray.Reject) == true &&
						SM.FMT[FlowTray.Reject].ProcStatus == ProcStatus.SortRun &&
						G.seqRejectBuffer.CheckNeedSort() == true)
					{
						G.seqUnloadPnP.SortStart(FlowTray.Reject);
						return;
					}

					if (G.seqRejectBuffer.IsPnPReady() == false)								return;
					if (NDm.Read(eNDM.LotEndMode) == (int)LotEndMode.LotEnd)					return;
					if (SM.FMT[FlowTray.Reject].GetInfo().KeyValue != "" && SM.FMP[FlowPart.ULDBuffer].GetInfo().KeyValue != SM.FMT[FlowTray.Reject].GetInfo().KeyValue) return;

					if (SM.FMP[FlowPart.ULDBuffer].GetExistCount() > 0 && G.seqUnloadBuffer.IsULDWait() == true &&  //결과 나온 후 NG의 갯수를 확인해야함
						SM.FMP[FlowPart.ULDBuffer].GetExistCount(ProdStatus.NG) <= SM.FMT[FlowTray.Reject].GetEmptyCount())
					{
						G.seqUnloadPnP.PnPStart();
						TactStart(EnumHelper.GetDescription(Tact.UNLOAD_PnP_START));
						return;
					}

					if (G.seqUnloadPnP.IsReadyPos() == false)
					{
						G.seqUnloadPnP.ReadyStart();
						return;
					}

				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void SchTrayUnloader()
		{
			try
			{
				if (G.seqTrayUnloader.IsReady == true)
				{
					if (G.seqTrayUnloader.IsLiftDownEnable() == true)
					{
						G.seqTrayUnloader.LiftDownStart();
						return;
					}

					if (G.seqTrayUnloader.IsLiftUpEnable() == true)
					{
						G.seqTrayUnloader.LiftUpStart();
						return;
					}

					if (NDm.Read(eNDM.AutoRunMode) == (int)AutoRunMode.EndLot)
					{
						//if (SM.FMT.GetFirstEmptyFlow(FlowTray.ULDConv01,FlowTray.ULDConv31 ) !=  FlowTray.Empty &&
						//	G.seqTrayUnloader.IsUnloadEnable() == true)
						//{
						//	G.seqTrayUnloader.UnloadStart();
						//	TactStart(EnumHelper.GetDescription(Tact.UNLOAD_CONVEYOR_START));
						//	return;
						//}

						if (SM.FMT.IsFlow(FlowTray.TrayClean) == false &&
							SM.FMT.IsFlow(FlowTray.LDTable1) == false &&
							SM.FMT.IsFlow(FlowTray.ULDTable1) == false &&
							SM.FMT.IsFlow(FlowTray.LDTable2) == false &&
							SM.FMT.IsFlow(FlowTray.ULDTable2) == false &&
							SM.FMT.IsFlow(FlowTray.LDTrans) == false &&
							SM.FMT.IsFlow(FlowTray.ULDTrans) == false &&
							SM.FMT.IsFlowRange(FlowTray.ULDConv01, FlowTray.ULDConv31) == true &&
							G.seqTrayUnloader.IsUnloadEnable() == true)
						{
							G.seqTrayUnloader.UnloadStart();
							TactStart(EnumHelper.GetDescription(Tact.UNLOAD_CONVEYOR_START));
							return;
						}
						if (G.seqTrayUnloader.IsLastTrayExist() == true &&
							 G.seqTrayUnloader.IsUnloadEnable() == true)
						{
							G.seqTrayUnloader.UnloadStart();

							//로그용
							for (int i = 0; i < stLotCnt.Max; i++)
							{
								if (SM.FMT.IsFlow(FlowTray.ULDConv01 + i) == true && SM.FMT[FlowTray.ULDConv01 + i].GetInfo().LastTrayOfLot == true)
								{
									GLog.Add($"{(FlowTray.ULDConv01 + i).ToString()} - Last Tray");
								}
							}
							TactStart(EnumHelper.GetDescription(Tact.UNLOAD_CONVEYOR_START));
						}
					}
					else if (G.seqTrayUnloader.IsLastTrayExist() == true &&
							 G.seqTrayUnloader.IsUnloadEnable() == true)
					{
						G.seqTrayUnloader.UnloadStart();

						//check 로그용

						for (int i = 0; i < stLotCnt.Max; i++)
						{
							if (SM.FMT.IsFlow(FlowTray.ULDConv01 + i) == true && SM.FMT[FlowTray.ULDConv01 + i].GetInfo().LastTrayOfLot == true)
							{
								GLog.Add($"{(FlowTray.ULDConv01 + i).ToString()} - Last Tray");
							}
						}
						TactStart(EnumHelper.GetDescription(Tact.UNLOAD_CONVEYOR_START));
					}
				}
				else
				{
					if (m_timeoutULD.IsRunning == true) m_timeoutULD.Stop();
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void SchRejectBuffer()
		{
			try
			{
				//Cur Lot 업데이트
				int[] tempYield = { 0, 0, 0 }; //ok , ng, user-ok
				string KeyValue = SDm.Read(eSDM.CurKeyValue);
				tempYield = SM.SHM.GetLotYield(KeyValue);
				if (m_dicBypassNG.ContainsKey(KeyValue) == false)
				{
					m_dicBypassNG.Clear();
					m_dicBypassNG.Add(KeyValue, 0);
				}

				int nVerifyNGCount = tempYield[1] - SM.FMT[FlowTray.Reject].GetVerifyNotCount() - SM.FMT[FlowTray.Reject].GetVerifyCount(ProdStatus.OK);

				//설정해둔 전체 NG수량보다 많은 경우
				if (nVerifyNGCount - m_dicBypassNG[KeyValue] > NDm.Read(eNDM.TotalNGCount) && SM.SHM.GetManyNGBypass(KeyValue) == true)
				{
					switch (SM.SHM.PopupMsgResult(SeqMsg.BypassByNG))
					{
						case "QUIT LOT":
							SM.SHM.SetManyNGBypass(KeyValue, false);
							break;
						case "CLOSE":
							//일반 Run 선택 시 해당 nVerifyNGCount를 초기화 해주기 위해 사용
							m_dicBypassNG[KeyValue] = nVerifyNGCount;
							SM.SHM.SetManyNGBypass(KeyValue, false);
							break;
						default:
							AddAlarm(ALM.ERROR_TOO_MANY_NG);
							SM.SHM.MsgShow(SeqMsg.BypassByNG);
							break;
					}
				}

				//설정해둔 연속 NG수량보다 많은 경우
				if (NDm.Read(eNDM.ContinuousNGCount) < NDm.Read(eNDM.CurContinuousNGCount))
				{
					switch (SM.SHM.PopupMsgResult(SeqMsg.BypassByContinueNG))
					{
						case "QUIT LOT":
							NDm.Write(eNDM.CurContinuousNGCount, 0);
							break;
						case "CLOSE":
							NDm.Write(eNDM.CurContinuousNGCount, 0);
							break;
						default:
							AddAlarm(ALM.ERROR_CONTINUOUS_NG);
							SM.SHM.MsgShow(SeqMsg.BypassByContinueNG);
							break;
					}
				}

				if (SM.FMT[FlowTray.Reject].GetExistCount() >= SM.SHM.GetRecipeDataToInt(DEVICE.RejectTrayVerifyCount) / 1000&&
					NDm.Read(eNDM.RejectRunMode) != (int)RejectRunMode.VerifyComplete &&
					SM.FMT[FlowTray.Reject].GetVerifyNotCount() != 0)
				{
					NDm.Write(eNDM.RejectRunMode, (int)RejectRunMode.VerifyReq);
				}

				if (NDm.Read(eNDM.RejectRunMode) == (int)RejectRunMode.VerifyComplete &&
					SM.FMT[FlowTray.Reject].GetVerifyNotCount() > 0)
				{
					NDm.Write(eNDM.RejectRunMode, (int)RejectRunMode.Normal);
				}

				if (G.seqRejectBuffer.IsReady == true &&
					G.seqUnloadPnP.IsSafetyPos() == true)
				{
					//센서가 안맞는 경우
					if (G.seqRejectBuffer.IsReadyPos() == true &&
						G.seqRejectBuffer.CheckSen() == false)
					{
						//컨펌안함 제품이 있으면 알람만 처리
						if (SM.FMT[FlowTray.Reject].GetVerifyNotCount() > 0)
						{
							G.seqRejectBuffer.CheckSen(true);
						}
						else
						{
							G.seqRejectBuffer.TrayChangeStart();
						}
						return;
					}

					if (SM.FMT.IsFlow(FlowTray.Reject) == false)
					{
						G.seqRejectBuffer.TrayChangeStart();
						return;
					}
					
					//레시피 변경 시
					if (G.seqUnloadPnP.IsSafetyPos() == true  &&  
						SM.SHM.GetRecipeData(DEVICE.ModuleType) != SM.FMT[FlowTray.Reject].GetInfo().ModuleType)
					{
						
						G.seqRejectBuffer.TrayChangeStart();
						return;
					}

					if (SM.FMP.IsFlow(FlowPart.ULDBuffer))
					{
						//다음 work order가 다른 경우
						if (SM.FMT[FlowTray.Reject].GetInfo().WorkOrder != "" &&
							SM.FMP[FlowPart.ULDBuffer].GetInfo().WorkOrder != SM.FMT[FlowTray.Reject].GetInfo().WorkOrder)
						{
							if (G.seqRejectBuffer.IsTrayChangePossible() == true)
							{
								G.seqRejectBuffer.TrayChangeStart();
								return;
							}
							else if (SM.FMT[FlowTray.Reject].GetVerifyNotCount() > 0)
							{
								NDm.Write(eNDM.RejectRunMode, (int)RejectRunMode.VerifyReq);
								return;
							}
							return;
						}
					}
					//로드 부분에 동일한 wo가 없을떄도 체인지 시작
					else if (SM.FMT[FlowTray.Reject].GetInfo().WorkOrder != "" &&
							CheckWOExist(FlowPart.LDPnP, FlowPart.ULDPnP, SM.FMT[FlowTray.Reject].GetInfo().WorkOrder) == false &&
							CheckWOExist(FlowTray.LDLift01, FlowTray.LDTable2, SM.FMT[FlowTray.Reject].GetInfo().WorkOrder) == false)
					{
						if (G.seqRejectBuffer.IsTrayChangePossible() == true)
						{
							G.seqRejectBuffer.TrayChangeStart();
							return;
						}
					}

					//NG가 가득차면 체인지 시작
					if ((SM.FMT[FlowTray.Reject].GetVerifyyNGCount() >= SM.SHM.GetRecipeDataToInt(DEVICE.RejectTrayChangeCount) / 1000 ||
						 SM.FMT[FlowTray.Reject].GetVerifyyNGCount() >= SM.FMT[FlowTray.Reject].GetInfo().MaxCntProdInTray) &&
						G.seqRejectBuffer.IsTrayChangePossible() == true)
					{
						G.seqRejectBuffer.TrayChangeStart();
						return;
					}

					if (G.seqRejectBuffer.IsReadyPos() == false)
					{
						G.seqRejectBuffer.ReadyStart();
						return;
					}
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
		private void SchLoadtable1()
		{
			try
			{
				if (G.seqLoadTrayTable1.IsReady == true)
				{
					if (SM.FMT.IsFlow(FlowTray.LDTable1) == true)
					{
						if (G.seqLoadTrayTable1.IsGrip() == false)
						{
							G.seqLoadTrayTable1.GripStart();
							return;
						}
						else if (SM.FMT[FlowTray.LDTable1].ProcStatus == ProcStatus.TrayEmptyDone &&
							G.seqLoadTrayTable1.IsMotorStats(SeqLoadTrayTable1.PosZ.DOWN) == false &&
							G.seqLoadTransfer.LoadTableUpDnSafety(0) == true)
						{
							G.seqLoadTrayTable1.DownStart(SeqLoadTrayTable1.UDMode.Unload);
							return;
						}

						if (SM.FMT[FlowTray.LDTable1].ProcStatus != ProcStatus.TrayEmptyDone &&
							G.seqLoadTrayTable1.IsMotorStats(SeqLoadTrayTable1.PosZ.UP) == false  &&
							G.seqLoadTransfer.LoadTableUpDnSafety(0) == true  && 
							G.seqLoadTrayTable2.LoadTable1UpDnSafety() == true &&
							CheckPriorityLoadTray() == 0)
						{
							G.seqLoadTrayTable1.UpStart(SeqLoadTrayTable1.UDMode.Load);
							return;
						}

					}
					else
					{
						if (G.seqLoadTrayTable1.IsRelease() == false)
						{
							G.seqLoadTrayTable1.ReleaseStart();
							return;
						}
						else if (G.seqLoadTrayTable1.IsMotorStats(SeqLoadTrayTable1.PosZ.DOWN) == false &&
								 G.seqLoadTransfer.LoadTableUpDnSafety(0) == true)
						{
							G.seqLoadTrayTable1.DownStart(SeqLoadTrayTable1.UDMode.Load);
							return;
						}
					}
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void SchLoadtable2()
		{
			try
			{	
				if (G.seqLoadTrayTable2.IsReady == true)
				{
					//둘중 하나라도 업일때만 동작
					if (G.cKernel.Motor(MT.LDPnPZ).IsRdy((int)SeqLoadPnP.PosZ.READY) == false &&  
						G.seqLoadPnP.CheckCylUpDown(SenPos.PICKER_UP) == false)		return;
					
					if (SM.FMT.IsFlow(FlowTray.LDTable2) == true)
					{
						if (G.seqLoadTrayTable2.IsGrip() ==false)
						{
							G.seqLoadTrayTable2.GripStart();
							return;
						}
						else if (SM.FMT[FlowTray.LDTable2].ProcStatus == ProcStatus.TrayEmptyDone )
						{
							if (G.seqLoadTrayTable2.IsMotorStats(SeqLoadTrayTable2.PosX.READY) == false &&
								G.seqLoadTrayTable2.IsMotorStats(SeqLoadTrayTable2.PosZ.UP) == true)
							{
								G.seqLoadTrayTable2.MoveXStart(SeqLoadTrayTable2.PosX.READY);
								return;
							}
							if (G.seqLoadTrayTable2.IsMotorStats(SeqLoadTrayTable2.PosX.READY) == true  &&
								G.seqLoadTrayTable2.IsMotorStats(SeqLoadTrayTable2.PosZ.DOWN) == false  &&
								G.seqLoadTransfer.LoadTableUpDnSafety(1) == true)
							{
								G.seqLoadTrayTable2.DownStart(SeqLoadTrayTable2.UDMode.Unload);
								return;
							}
						}
						else 
						{
							//empty 체크를 위한 위치 이동
							if (SM.FMT[FlowTray.LDTable2].GetExistCount() == 0 )
							{
								if (SM.FMT[FlowTray.LDTable2].GetExistCount() == 0 &&
									G.seqLoadTrayTable2.IsMotorStats(SeqLoadTrayTable2.PosX.READY) == false &&
									G.seqLoadTrayTable2.IsMotorStats(SeqLoadTrayTable2.PosZ.UP) == true)
								{
									G.seqLoadTrayTable2.MoveXStart(SeqLoadTrayTable2.PosX.READY);
									return;
								}

								if (G.seqLoadTrayTable2.IsMotorStats(SeqLoadTrayTable2.PosZ.UP) == false &&
									G.seqLoadTransfer.LoadTableUpDnSafety(1) == true &&
									G.seqLoadTrayTable2.IsMotorStats(SeqLoadTrayTable2.PosX.READY) == true)
								{
									G.seqLoadTrayTable2.UpStart(SeqLoadTrayTable2.UDMode.Load);
									return;
								}
							}
							else
							{
								if (G.seqLoadTrayTable2.IsMotorStats(SeqLoadTrayTable2.PosZ.UP) == false &&
									G.seqLoadTransfer.LoadTableUpDnSafety(1) == true &&
									G.seqLoadTrayTable2.IsMotorStats(SeqLoadTrayTable2.PosX.READY) == true)
								{
									G.seqLoadTrayTable2.UpStart(SeqLoadTrayTable2.UDMode.Load);
									return;
								}
								if (G.seqLoadTrayTable2.IsMotorStats(SeqLoadTrayTable2.PosX.PnP) == false &&
									G.seqLoadTrayTable1.IsMotorStats(SeqLoadTrayTable1.PosZ.DOWN) == true &&
									G.seqLoadTrayTable2.IsMotorStats(SeqLoadTrayTable2.PosZ.UP) == true  &&
                                    CheckPriorityLoadTray() == 1)
								{
									G.seqLoadTrayTable2.MoveXStart(SeqLoadTrayTable2.PosX.PnP);
									return;
								}
							}
						}
					}
					else
					{
						if (G.seqLoadTrayTable2.IsRelease() ==false  &&
							G.seqLoadTrayTable2.IsMotorStats(SeqLoadTrayTable2.PosZ.DOWN) == true)
						{
							G.seqLoadTrayTable2.ReleaseStart();
							return;
						}
						else if (G.seqLoadTrayTable2.IsMotorStats(SeqLoadTrayTable2.PosZ.UP) == true  &&
							G.seqLoadTrayTable2.IsMotorStats(SeqLoadTrayTable2.PosX.READY) == false )
						{
							G.seqLoadTrayTable2.MoveXStart(SeqLoadTrayTable2.PosX.READY);
							return;
						}
						else if (G.seqLoadTrayTable2.IsMotorStats(SeqLoadTrayTable2.PosZ.DOWN) == false &&
								 G.seqLoadTrayTable2.IsMotorStats(SeqLoadTrayTable2.PosX.READY) == true  &&
								 G.seqLoadTransfer.LoadTableUpDnSafety(0) == true)
						{
							G.seqLoadTrayTable2.DownStart(SeqLoadTrayTable2.UDMode.Load);
							return;
						}
					}
				}

			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
	

		private void SchEndLot()
		{
			try
			{
				if (NDm.Read(eNDM.AutoRunMode) == (int)AutoRunMode.EndLot)
				{
					if (CheckUsePartEmpty() == true &&
						SM.FMT[FlowTray.Reject].GetExistCount(ProdStatus.OK) == 0 &&
						SM.FMT[FlowTray.Reject].GetVerifyNotCount() == 0)
					{
						if (SM.FMT.IsFlow(FlowTray.ULDTable1) == true &&
							SM.FMT[FlowTray.ULDTable1].ProcStatus != ProcStatus.SortRun &&
							SM.FMT[FlowTray.ULDTable1].ProcStatus != ProcStatus.TrayFillDone &&
							(SM.FMT.IsFlow(FlowTray.ULDTable2) == true &&
							 SM.FMT[FlowTray.ULDTable2].GetExistCount() > 0 &&
							 SM.FMT[FlowTray.ULDTable1].GetExistCount() == 0) == false)
						{
							if (G.seqUnloadTrayTable1.CheckNeedSort(FlowTray.ULDTable1) == true) SM.FMT[FlowTray.ULDTable1].ProcStatus = ProcStatus.SortRun;
							else ProcessTrayFillDone(FlowTray.ULDTable1);
						}

						if (SM.FMT.IsFlow(FlowTray.ULDTable2) == true &&
							SM.FMT[FlowTray.ULDTable2].ProcStatus != ProcStatus.SortRun &&
							SM.FMT[FlowTray.ULDTable2].ProcStatus != ProcStatus.TrayFillDone &&
							(SM.FMT.IsFlow(FlowTray.ULDTable1) == true &&
							 SM.FMT[FlowTray.ULDTable1].GetExistCount() > 0 &&
							 SM.FMT[FlowTray.ULDTable2].GetExistCount() == 0) == false)
						{
							if (G.seqUnloadTrayTable2.CheckNeedSort(FlowTray.ULDTable2) == true) SM.FMT[FlowTray.ULDTable2].ProcStatus = ProcStatus.SortRun;
							else ProcessTrayFillDone(FlowTray.ULDTable2);
						}

						if (CheckUseTrayEmpty(FlowTray.LDTrans, FlowTray.ULDTable2) == true &&
							SM.FMT.IsFlow(FlowTray.ULDConv01) == true &&
							SM.FMT[FlowTray.ULDConv01].GetInfo().LastTrayOfLot == false)
						{
							NDm.Write(eNDM.FillDoneRemainCount, 0);
							SM.FMT[FlowTray.ULDConv01].SetLastOfLot(true);
						}
					}

					if (CheckUseTrayEmpty() == true)
					{
						EndLotProcessing();
					}

					if (SM.FMP.IsFlowRange(FlowPart.LDPnP, FlowPart.ULDPnP) == false &&
						SM.FMT[FlowTray.Reject].GetVerifyNotCount() > 0 &&
						NDm.Read(eNDM.RejectRunMode) != (int)RejectRunMode.VerifyComplete)
					{
						NDm.Write(eNDM.RejectRunMode, (int)RejectRunMode.VerifyReq);
					}

					//있어야 되는데 없는 경우 - 트레이 갯수를 실제보다 많이 입력
					if (SM.FMT.IsFlowRange(FlowTray.LDConv01, FlowTray.LDConv31) == true &&
						G.cKernel.IsAIn(DI.LDTrayConvExistSen) == false &&
						G.seqTrayLoader.IsReady == true &&
						G.seqLoadTransfer.IsReady == true)
					{
						SM.FMT.FlowClearRange(FlowTray.LDConv01, FlowTray.LDConv31);
					}

					//없어야 되는데 있는 경우 - 트레이 갯수를 실제보다 적게 입력
					if (SM.FMT.IsFlowRange(FlowTray.LDConv01, FlowTray.LDConv31) == false &&
						G.cKernel.IsAIn(DI.LDTrayConvExistSen) == true &&
						G.seqTrayLoader.IsReady == true &&
						G.seqLoadTransfer.IsReady == true)
					{
						SM.FMT.Next(FlowTray.Empty, FlowTray.LDConv01);
					}
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void SchQuitLot()
		{
			try
			{
				if (NDm.Read(eNDM.AutoRunMode) == (int)AutoRunMode.QuitLot)
				{
					if (SM.FMT.IsFlow(FlowTray.TrayClean) == true) SM.FMT[FlowTray.TrayClean].ProcStatus = ProcStatus.CleanDone;

					if (CheckUseTrayEmpty() == true)
					{
						EndLotProcessing();
					}
					//파트에 제품 있는 경우 모두 뺄때까지 정상 동작 진행
					if (CheckUsePartEmpty() == false) return;

					//언로드 테이블 TrayFillDone 처리
					if (SM.FMT[FlowTray.Reject].GetExistCount(ProdStatus.OK) == 0 &&
						SM.FMT[FlowTray.Reject].GetVerifyNotCount() == 0 &&
						SM.FMP[FlowPart.ULDPnP].GetExistCount() == 0 &&
						G.seqUnloadPnP.IsReady == true)
					{
						if (SM.FMT.IsFlow(FlowTray.ULDTable1) == true &&
							SM.FMT[FlowTray.ULDTable1].ProcStatus != ProcStatus.TrayFillDone &&
							(SM.FMT.IsFlow(FlowTray.ULDTable2) == true && //반대쪽에만 자재가 있는 경우 그쪽 먼저 처리
							SM.FMT[FlowTray.ULDTable2].GetExistCount() > 0 &&
							SM.FMT[FlowTray.ULDTable1].GetExistCount() == 0) == false)
						{
							if (G.seqUnloadTrayTable1.CheckNeedSort(FlowTray.ULDTable1) == true) SM.FMT[FlowTray.ULDTable1].ProcStatus = ProcStatus.SortRun;
							else ProcessTrayFillDone(FlowTray.ULDTable1);
						}

						if (SM.FMT.IsFlow(FlowTray.ULDTable2) == true &&
							SM.FMT[FlowTray.ULDTable2].ProcStatus != ProcStatus.TrayFillDone &&
							(SM.FMT.IsFlow(FlowTray.ULDTable1) == true && //반대쪽에만 자재가 있는 경우 그쪽 먼저 처리
							SM.FMT[FlowTray.ULDTable1].GetExistCount() > 0 &&
							SM.FMT[FlowTray.ULDTable2].GetExistCount() == 0) == false)
						{
							if (G.seqUnloadTrayTable2.CheckNeedSort(FlowTray.ULDTable2) == true) SM.FMT[FlowTray.ULDTable2].ProcStatus = ProcStatus.SortRun;
							else ProcessTrayFillDone(FlowTray.ULDTable2);
						}
					}

					if (SM.FMT[FlowTray.Reject].GetVerifyNotCount() > 0 &&
						NDm.Read(eNDM.RejectRunMode) != (int)RejectRunMode.VerifyComplete)
					{
						NDm.Write(eNDM.RejectRunMode, (int)RejectRunMode.VerifyReq);
					}

					//후반부 트레이 없으면 로드 테이블 TrayEmptyDone 처리
					if ((SM.FMT.IsFlow(FlowTray.TrayClean) == false ||
						(SM.FMT.IsFlow(FlowTray.TrayClean) == true && SM.FMT[FlowTray.TrayClean].GetInfo().AutoRunMode == (int)AutoRunMode.QuitLot)) &&
						 (SM.FMT.IsFlow(FlowTray.ULDTrans) == false ||
						(SM.FMT.IsFlow(FlowTray.ULDTrans) == true && SM.FMT[FlowTray.ULDTrans].GetInfo().AutoRunMode == (int)AutoRunMode.QuitLot)) &&
						 (SM.FMT.IsFlow(FlowTray.ULDTable1) == false ||
						(SM.FMT.IsFlow(FlowTray.ULDTable1) == true && SM.FMT[FlowTray.ULDTable1].GetInfo().AutoRunMode == (int)AutoRunMode.QuitLot)) &&
						 (SM.FMT.IsFlow(FlowTray.ULDTable2) == false ||
						(SM.FMT.IsFlow(FlowTray.ULDTable2) == true && SM.FMT[FlowTray.ULDTable2].GetInfo().AutoRunMode == (int)AutoRunMode.QuitLot)))
					{
						//검사진행한 후반부 트레이 먼저 배출
						if (SM.FMT.IsFlow(FlowTray.ULDConv01) == true &&
							SM.FMT[FlowTray.ULDConv01].GetInfo().AutoRunMode != (int)AutoRunMode.QuitLot &&
							SM.FMT[FlowTray.ULDConv01].GetInfo().LastTrayOfLot == false)
						{
							NDm.Write(eNDM.FillDoneRemainCount, 0);
							SM.FMT[FlowTray.ULDConv01].SetLastOfLot(true);
						}
						//이후 마지막 트레이 배출
						else if (SM.FMT.IsFlow(FlowTray.ULDConv01) == true &&
								 SM.FMT.IsFlow(FlowTray.LDTable1) == false &&
								 SM.FMT.IsFlow(FlowTray.LDTable2) == false &&
								 SM.FMT.IsFlow(FlowTray.TrayClean) == false &&
								 SM.FMT.IsFlow(FlowTray.ULDTrans) == false &&
								 CheckUseTrayEmpty(FlowTray.LDTrans, FlowTray.ULDTable2) == true &&
								 NDm.Read(eNDM.FillDoneRemainCount) != 0 &&
								 SM.FMT[FlowTray.ULDConv01].GetInfo().LastTrayOfLot == false)
						{
							NDm.Write(eNDM.FillDoneRemainCount, 0);
							SM.FMT[FlowTray.ULDConv01].SetLastOfLot(true);
						}

						if (SM.FMT.IsFlow(FlowTray.LDTable1) == true && SM.FMT[FlowTray.LDTable1].ProcStatus != ProcStatus.TrayEmptyDone) SM.FMT[FlowTray.LDTable1].ProcStatus = ProcStatus.TrayEmptyDone;
						if (SM.FMT.IsFlow(FlowTray.LDTable2) == true && SM.FMT[FlowTray.LDTable2].ProcStatus != ProcStatus.TrayEmptyDone) SM.FMT[FlowTray.LDTable2].ProcStatus = ProcStatus.TrayEmptyDone;
					}


                    //있어야 되는데 없는 경우 - 트레이 갯수를 실제보다 많이 입력
                    if (SM.FMT.IsFlowRange(FlowTray.LDConv01, FlowTray.LDConv31) == true &&
                        G.cKernel.IsAIn(DI.LDTrayConvExistSen) == false &&
                        G.seqTrayLoader.IsReady == true &&
                        G.seqLoadTransfer.IsReady == true)
                    {
                        SM.FMT.FlowClearRange(FlowTray.LDConv01, FlowTray.LDConv31);
                    }
                    //없어야 되는데 있는 경우 - 트레이 갯수를 실제보다 적게 입력
                    if (SM.FMT.IsFlowRange(FlowTray.LDConv01, FlowTray.LDConv31) == false &&
                        G.cKernel.IsAIn(DI.LDTrayConvExistSen) == true &&
                        G.seqTrayLoader.IsReady == true &&
                        G.seqLoadTransfer.IsReady == true)
                    {
                        SM.FMT.Next(FlowTray.Empty, FlowTray.LDConv01);
                    }
                }
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void SchRecipeChange()
		{
			try
			{
				if (NDm.Read(eNDM.RecipeChange) >= 0)
				{
					if (G.allHome.IsHomeReady == false)																							return;
					if (G.seqTrayUnloader.IsReady == false)																						return; //언로더 리프트는 자재 유/무를 체크 하지 않기 떄문에 필요
					if (SM.FMT.IsFlowRange(FlowTray.LDConv01, FlowTray.ULDConv31) == true)														return;
					if (SM.FMP.IsFlowRange(FlowPart.LDPnP, FlowPart.ULDPnP) == true)															return;
					if (SM.FMT.IsFlow(FlowTray.Reject) == true && SM.FMT[FlowTray.Reject].GetInfo().ProcStatus == (int)ProcStatus.SortRun)		return;

					bool bResult = false;
					UI.PageMainDisp.Invoke(new System.Windows.Forms.MethodInvoker(delegate ()
					{
						bResult = UI.SubpageRecipe.RecipeChange(NDm.Read(eNDM.RecipeChange));
					}));
					if (bResult == true)	G.allHome.Start(false);
					else					AddAlarm(ALM.ERROR_CHANGE_RECIPE);
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void SchCycleStop()
		{
			try
			{
				if (NDm.Read(eNDM.AutoRunMode) == (int)AutoRunMode.CycleStop)
				{
					// Unload 버퍼가 비어있지 않으면 Place 정지
					if (G.seqLoadInspPnP.IsPlaceReady() == false &&
						(G.seqInspPlate1.IsLoadWaitReady() == true && G.seqInspPlate2.IsLoadWaitReady() == true))
					{
						G.seqLoadInspPnP.PlaceStop();
					}

					// Unload 버퍼가 비어있지 않으면 Place 정지
					if (G.seqUnloadInspPnP.IsPlaceReady() == false &&
						G.seqUnloadBuffer.IsLoadWaitReady() == true)
					{
						G.seqUnloadInspPnP.PlaceStop();
					}

					// Place 할 트레이가 없을 경우 Place 정지
					if (G.seqUnloadPnP.IsPlaceReady() == false &&
						G.seqUnloadPnP.PlaceTrayExist() == false)
					{
						G.seqUnloadPnP.PlaceStop();
					}

					if (G.seqLoadTransfer.IsReady == true &&
						G.seqUnloadTransfer.IsReady == true &&
						G.seqLoadPnP.IsReady == true &&
						G.seqLoadInspPnP.IsReady == true &&
						G.seqUnloadPnP.IsReady == true &&
						G.seqUnloadInspPnP.IsReady == true)
					{
						AutoRunPause();
						NDm.Write(eNDM.AutoRunMode, NDm.Read(eNDM.OldAutoRunMode));
					}
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}


		public void Monitor()
		{
			try
			{
				MonIdle();
				MonTrayUnloader();
				MonDustCollectorOnOff();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public void MonIdle()
		{
			try
			{
				if (G.seqInspPlate1.IsReady == true &&
					G.seqInspPlate2.IsReady == true &&
					G.seqLoadBuffer1.IsReady == true &&
					G.seqLoadBuffer2.IsReady == true &&
					G.seqLoadInspPnP.IsReady == true &&
					G.seqTrayLoader.IsReady == true &&
					G.seqLoadPnP.IsReady == true &&
					G.seqLoadTransfer.IsReady == true &&
					G.seqLoadTrayTable1.IsReady == true &&
					G.seqLoadTrayTable2.IsReady == true &&
					G.seqRejectBuffer.IsReady == true &&
					G.seqTrayClean.IsReady == true &&
					G.seqUnloadBuffer.IsReady == true &&
					G.seqUnloadInspPnP.IsReady == true &&
					G.seqTrayUnloader.IsReady == true &&
					G.seqUnloadPnP.IsReady == true &&
					G.seqUnloadTransfer.IsReady == true &&
					G.seqUnloadTrayTable1.IsReady == true &&
					G.seqUnloadTrayTable2.IsReady == true)
				{
					SM.SHM.SetBitData(BIT.IDLE, true);
				}
				else
				{
					SM.SHM.SetBitData(BIT.IDLE, false);
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void MonTrayUnloader()
		{
			try
			{
				if (SM.SHM.IsBitData(BIT.AutoRun) == false) return;
				if (G.seqTrayUnloader.IsReady == false) return;//기존에 리프트 업상태 체크, 사용자가 움질일 수 있어서 수정
				if (SM.FMT.IsFlowRange(FlowTray.ULDLift01, FlowTray.ULDLift31) == false) return;

				if (G.cKernel.IsAIn(DI.ULDTrayLiftExistSen) == true)
				{
					if (IsResultExistRange(ProdStatus.OK, FlowTray.ULDLift01, FlowTray.ULDLift31) == true)
						G.AlmMng.ADD(ALM.WARNING_TRAY_UNLOAD_END);
					else if (G.AlmMng.Find(ALM.WARNING_TRAY_UNLOAD_END_QUIT_LOT) == false)
					{
						G.commInspTop.SendPopupMsgReq("QUIT LOT UNLOAD END");
						G.commInspBtm.SendPopupMsgReq("QUIT LOT UNLOAD END");
						G.AlmMng.ADD(ALM.WARNING_TRAY_UNLOAD_END_QUIT_LOT);
					}

					if (NDm.Read(eNDM.ULDWarningDelay) > 0 &&
						SM.SHM.IsBitData(BIT.Buzzer) == false)      // 언로드 부저 관련 조건이 맞으면 다시 울리도록 추가됨
					{
						if (m_timeoutULDWarning.IsRunning == false) m_timeoutULDWarning.Restart();
						else if (m_timeoutULDWarning.IsTimeOverMs(NDm.Read(eNDM.ULDWarningDelay)) == true)
						{
							SM.SHM.SetBitData(BIT.WarningBuzzer, true);
							m_timeoutULDWarning.Stop();
						}
					}
				}
				else
				{
					if (m_timeoutULD.IsRunning == false) m_timeoutULD.Restart();
					if (m_timeoutULD.IsTimeOverMs(3000) == false) return;

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
					SM.SHM.SetBitData(BIT.WarningBuzzer, false);

					if (m_timeoutULDWarning.IsRunning == true) m_timeoutULDWarning.Stop();
					m_timeoutULD.Stop();
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void MonDustCollectorOnOff()
		{
			try
			{
				bool bempty = CheckUseTrayEmpty() & CheckUsePartEmpty();

				if (G.cKernel.IsAOut(DO.DustControl) == bempty) G.cKernel.BOut(DO.DustControl, bempty);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}



		private void LiftSwitch()
		{
			try
			{
				//Load Lift
				//1초? 0,5초 누르면 동작
				if (SM.FMT.IsFlowRange(FlowTray.LDLift01, FlowTray.LDLift31) == true ||
					SM.FMT.IsFlowRange(FlowTray.ULDLift01, FlowTray.ULDLift31) == true ||
					SM.FMT.IsFlow(FlowTray.LDWaitBuf) == true &&
					SM.SHM.IsBitData(BIT.Error) == false)
				{

					if (G.cKernel.IsAIn(DI.LDTrayLiftUpBtn) == true ||
						G.cKernel.IsAIn(DI.LDTrayLiftDnBtn) == true ||
						G.cKernel.IsAIn(DI.ULDTrayLiftUpBtn) == true ||
						G.cKernel.IsAIn(DI.ULDTrayLiftDnBtn) == true)
					{
						if (m_BtnDelay.IsRunning == false)
						{
							m_BtnDelay.Restart();
						}
						else if (m_BtnDelay.IsTimeOverMs(500) == true)
						{
							if (G.cKernel.IsAIn(DI.LDTrayLiftUpBtn) && G.seqTrayLoader.IsReady == true)
							{
								G.seqTrayLoader.SwitchLiftUpStart();
								return;
							}

							if (G.cKernel.IsAIn(DI.LDTrayLiftDnBtn) && G.seqTrayLoader.IsReady == true)
							{
								G.seqTrayLoader.SwitchLiftDownStart();
								return;
							}

							if (G.cKernel.IsAIn(DI.ULDTrayLiftUpBtn) && G.seqTrayUnloader.IsReady == true)
							{
								G.seqTrayUnloader.SwitchLiftUpStart();
								return;
							}

							if (G.cKernel.IsAIn(DI.ULDTrayLiftDnBtn) && G.seqTrayUnloader.IsReady == true)
							{
								G.seqTrayUnloader.SwitchLiftDownStart();
								return;
							}
						}
					}
					else
					{
						if (m_BtnDelay.IsRunning == true)
						{
							m_BtnDelay.Stop();
							m_BtnDelay.Reset();
						}
					}
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void BuzzerSwitch()
		{
			try
			{
				//Buzzer Off Click
				if (G.cKernel.IsAIn(DI.BuzzerOffBtn) == true && (SM.SHM.IsBitData(BIT.Buzzer) == true || SM.SHM.IsBitData(BIT.WarningBuzzer) == true || SM.SHM.IsBitData(BIT.RejectBuzzer) == true))
				{
					SM.SHM.SetBitData(BIT.Buzzer, false);
					SM.SHM.SetBitData(BIT.WarningBuzzer, false);
					SM.SHM.SetBitData(BIT.RejectBuzzer, false);
					UI.Logs.Write(LOGPART.SYS, "Buzzer Off Button Click(Switch)");
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private bool	RecipeCheck()
		{
			try
			{
				FlowTray flow = SM.FMT.GetFirstExistFlow(FlowTray.LDLift01, FlowTray.LDLift31);
				if (flow== FlowTray.Empty )						return false;
				
				
				InfoTray.tsInfo info = SM.FMT[flow].GetInfo();
				stRunLotInfo Lotinfo = new stRunLotInfo();
				Lotinfo = SM.SHM.GetRunLotInfo(info.KeyValue);

				if (Lotinfo.RecipeCheck == true)							return true;
				
				//메모리 타입별로 레시피 변경 진행
				string strModuleType = info.ModuleType;
				NDm.Write(eNDM.RecipeChange, -1);
				if (strModuleType != SM.SHM.GetRecipeData(DEVICE.ModuleType))
				{
					for (int i = 0; i < Define.MAX_RECIPE_NUM; i++)
					{
						if (SM.SHM.GetRecipeData(i, DEVICE.ModuleType) != strModuleType) continue;

						NDm.Write(eNDM.RecipeChange, i);
						GLog.Add($"Ndm - RecipeChange : {i}");
						Lotinfo.RecipeCheck = true;
						break;
					}
					if (NDm.Read(eNDM.RecipeChange) == -1)
					{
						AddAlarm(ALM.ERROR_FIND_RECIPE);
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

		private void InitPartUse()
		{
			try
			{
				m_dicPartUse.Add(FlowPart.Insp1, UsePartName.Insp1);
				m_dicPartUse.Add(FlowPart.Insp2, UsePartName.Insp2);
				m_dicPartUse.Add(FlowPart.LDBuffer1, UsePartName.LDBuf1);
				m_dicPartUse.Add(FlowPart.LDBuffer2, UsePartName.LDBuf2);

				m_ltPnpPart.Add(FlowPart.LDPnP);
				m_ltPnpPart.Add(FlowPart.LDInspPnP);
				m_ltPnpPart.Add(FlowPart.ULDInspPnP);
				m_ltPnpPart.Add(FlowPart.ULDPnP);

				m_dicTrayUse.Add(FlowTray.LDTable2, UsePartName.LDTable2);
				m_dicTrayUse.Add(FlowTray.LDTable1, UsePartName.LDTable1);
				m_dicTrayUse.Add(FlowTray.ULDTable1, UsePartName.ULDTable1);
				m_dicTrayUse.Add(FlowTray.ULDTable2, UsePartName.ULDTable2);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private bool CheckUsePartEmpty()
		{
			try
			{
				for (FlowPart flow = FlowPart.Empty + 1; flow < FlowPart.Max; flow++)
				{
					if (m_dicPartUse.ContainsKey(flow) == true && SM.SHM.IsUsePart(m_dicPartUse[flow], UsePartMode.Unload) == false) continue;
					if (m_ltPnpPart.Contains(flow) == true && CheckPnPEmpty(flow) == true) continue;

					if (SM.FMP.IsFlow(flow) == true)
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

		private bool CheckUseTrayEmpty()
		{
			try
			{
				for (FlowTray flow = FlowTray.LDLift01; flow <= FlowTray.ULDConv31; flow++)
				{
					if (m_dicTrayUse.ContainsKey(flow) == true && SM.SHM.IsUsePart(m_dicTrayUse[flow], UsePartMode.Unload) == false) continue;

					if (SM.FMT.IsFlow(flow) == true)
					{
						return false;
					}
				}

				if (SM.FMT.IsFlow(FlowTray.Reject) == true && SM.FMT[FlowTray.Reject].GetVerifyNotCount() > 0)
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

		private bool CheckUseTrayEmpty(FlowTray FlowStart, FlowTray FlowEnd)
		{
			try
			{
				for (FlowTray flow = FlowStart; flow <= FlowEnd; flow++)
				{
					if (m_dicTrayUse.ContainsKey(flow) == true && SM.SHM.IsUsePart(m_dicTrayUse[flow], UsePartMode.Unload) == false) continue;

					if (SM.FMT.IsFlow(flow) == true)
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

		//Quit Lot 진행 시, uldConv에 양품제품이 있는지 여부
		private bool IsResultExistRange(ProdStatus result, FlowTray startFlow, FlowTray Endflow)
		{
			for (FlowTray i = startFlow; i <= Endflow; i++)
			{
				if (SM.FMT.IsFlow(i) == false) continue;

				if (SM.FMT[i].IsResultExist(result)) return true;
			}

			return false;
		}

		public bool CheckPnPEmpty(FlowPart FlowPnP)
		{
			try
			{
				if (SM.FMP.IsFlow(FlowPnP) == false) return true;
				if (SM.FMP[FlowPnP].GetExistCount() == 0) return true;

				bool[] bexist = SM.FMP[FlowPnP].GetProdExist();

				for (int i = 0; i < bexist.Length; i++)
				{
					if (bexist[i] == true && SM.SHM.IsUsePart(UsePartName.Picker1 + i, UsePartMode.Disable) == false)
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
		//동일 work order가 있는지 체크
		private bool CheckWOExist(FlowPart startPart, FlowPart EndPart, string strWorkOrder)
		{
			try
			{

				for (FlowPart i = startPart; i <= EndPart; i++)
				{
					if (SM.FMP.IsFlow(i) == false) continue;

					if (SM.FMP[i].GetInfo().WorkOrder == strWorkOrder)
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

		private bool CheckWOExist(FlowTray startTray, FlowTray EndTray, string strWorkOrder)
		{
			try
			{

				for (FlowTray i = startTray; i <= EndTray; i++)
				{
					if (SM.FMT.IsFlow(i) == false) continue;

					if (SM.FMT[i].GetInfo().WorkOrder == strWorkOrder)
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

		public void TactStart(string strKey)
		{
			try
			{
				if (m_dicTact.ContainsKey(strKey) == false) m_dicTact.Add(strKey, new PStopwatch());

				SM.SHM.SetSeqLastTact(strKey, m_dicTact[strKey].ElapsedMilliseconds);

				m_dicTact[strKey].Restart();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public void TactStop(string strKey)
		{
			try
			{
				if (m_dicTact.ContainsKey(strKey) == false) return;

				m_dicTact[strKey].Stop();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public void TactStopAll()
		{
			try
			{
				m_ltTactPaused.Clear();

				foreach (var dic in m_dicTact)
				{
					dic.Value.Stop();
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public void TactPauseAll()
		{
			try
			{
				foreach (var dic in m_dicTact)
				{
					if (dic.Value.IsRunning == true)
					{
						dic.Value.Stop();
						m_ltTactPaused.Add(dic.Key);
					}
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public void TactResumAll()
		{
			try
			{
				foreach (var lt in m_ltTactPaused)
				{
					m_dicTact[lt].Start();
				}

				m_ltTactPaused.Clear();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public void UpdateTact()
		{
			try
			{
				foreach (var dic in m_dicTact)
				{
					SM.SHM.SetSeqTact(dic.Key, dic.Value.ElapsedMilliseconds);
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public int CheckPriorityProd(FlowPart Flow1, FlowPart Flow2)
		{
			try
			{
				if (SM.FMP.IsFlow(Flow1) == false && SM.FMP.IsFlow(Flow2) == false) return 0;
				if (SM.FMP.IsFlow(Flow1) == true && SM.FMP.IsFlow(Flow2) == false) return 1;
				if (SM.FMP.IsFlow(Flow1) == false && SM.FMP.IsFlow(Flow2) == true) return 2;

				if (SM.FMP[Flow1].GetInfo().StartTime < SM.FMP[Flow2].GetInfo().StartTime) return 1;
				else return 2;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return 0;
			}
		}

		private bool IsUnloadConvLastTrayExist()
		{
			try
			{
				for (FlowTray t = FlowTray.ULDConv01; t <= FlowTray.ULDConv31; t++)
				{
					if (SM.FMT[t].GetInfo().LastTrayOfLot == true) return true;
				}

				return false;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		//둘다 FillDone인 경우 해당 함수 호출 => 양품있는 트레이, NG있는 트레이, 비어있는 트레이 순으로 언로드
		public SeqUnloadTransfer.PosX CheckPriorityUnloadTray()
		{
			if (SM.FMT.IsFlow(FlowTray.ULDTable1) == false) return SeqUnloadTransfer.PosX.UNLOADTABLE2;
			if (SM.FMT.IsFlow(FlowTray.ULDTable2) == false) return SeqUnloadTransfer.PosX.UNLOADTABLE1;

			bool bTable1OkExist = SM.FMT[FlowTray.ULDTable1].IsResultExist(ProdStatus.OK);
			bool bTable2OkExist = SM.FMT[FlowTray.ULDTable2].IsResultExist(ProdStatus.OK);
			bool bTable1NgExist = SM.FMT[FlowTray.ULDTable1].IsResultExist(ProdStatus.NG);
			bool bTable2NgExist = SM.FMT[FlowTray.ULDTable2].IsResultExist(ProdStatus.NG);

			
			if (bTable1OkExist == true && bTable2OkExist == true)  //둘다 양품이 있는 경우, 가득 차있는 트레이부터 언로드
			{
				if (SM.FMT[FlowTray.ULDTable1].IsFull() == true)		return SeqUnloadTransfer.PosX.UNLOADTABLE1;
				else													return SeqUnloadTransfer.PosX.UNLOADTABLE2;
			}
			else if (bTable1OkExist == true)							return SeqUnloadTransfer.PosX.UNLOADTABLE1;
			else if (bTable2OkExist == true)							return SeqUnloadTransfer.PosX.UNLOADTABLE2;	
			else if (bTable1NgExist == true && bTable2NgExist == true) //둘다 NG가 있는 경우도 가득 차 있는 트레이부터
			{
				if (SM.FMT[FlowTray.ULDTable1].IsFull() == true)		return SeqUnloadTransfer.PosX.UNLOADTABLE1;
				else													return SeqUnloadTransfer.PosX.UNLOADTABLE2;
			}
			else if (bTable1NgExist == true)							return SeqUnloadTransfer.PosX.UNLOADTABLE1;
			else if (bTable2NgExist == true)							return SeqUnloadTransfer.PosX.UNLOADTABLE2;
			else	//둘다 비어있는 경우 - filldone이 먼저된거 먼저 빼기
			{
				if (SM.FMT[FlowTray.ULDTable1].GetInfo().TransTime > SM.FMT[FlowTray.ULDTable2].GetInfo().TransTime)	return SeqUnloadTransfer.PosX.UNLOADTABLE2;
				else																										return SeqUnloadTransfer.PosX.UNLOADTABLE1;
			}
		}
		
		// 0: loadtable1 , 1: loadtable2 우선 
		public int CheckPriorityLoadTray()
		{
			if (SM.FMT.IsFlow(FlowTray.LDTable1) == false  && 
				SM.FMT.IsFlow(FlowTray.LDTable2) == false )
			{	
				return -1;
			}
			else if (SM.FMT.IsFlow(FlowTray.LDTable1) == true &&
					 SM.FMT.IsFlow(FlowTray.LDTable2) == false)
			{
				return 0;
			}
			else if (SM.FMT.IsFlow(FlowTray.LDTable1) == false &&
					 SM.FMT.IsFlow(FlowTray.LDTable2) == true)
			{
				return 1;
			}

			int nExistCount1 = SM.FMT[FlowTray.LDTable1].GetExistCount();
			int nExistCount2 = SM.FMT[FlowTray.LDTable2].GetExistCount();
			
			bool bTable1NeedEmptCheck = nExistCount1 == 0 && SM.FMT[FlowTray.LDTable1].ProcStatus != ProcStatus.TrayEmptyDone;
			bool bTable2NeedEmptCheck = nExistCount2 == 0 && SM.FMT[FlowTray.LDTable2].ProcStatus != ProcStatus.TrayEmptyDone;

			if (bTable1NeedEmptCheck && bTable2NeedEmptCheck)
			{
				if (SM.FMT[FlowTray.LDTable1].GetInfo().TransTime > SM.FMT[FlowTray.LDTable2].GetInfo().TransTime ) return 0;
				else																								return 1;
			}
			else if (bTable1NeedEmptCheck)
			{
				return 0;
			}
			else if (bTable2NeedEmptCheck)
			{
				return 1;
			}

			if (nExistCount1 > 0  &&  nExistCount2 > 0)
			{
				if (SM.FMT[FlowTray.LDTable1].GetInfo().TransTime > SM.FMT[FlowTray.LDTable2].GetInfo().TransTime ) return 1;
				else																								return 0;
			}
			else if (nExistCount1 == 0 && nExistCount2 > 0)		return 1;
			else if (nExistCount1 > 0  && nExistCount2 == 0)	return 0;

			return -1;
		}

		//Fill done 카운트 처리, 0이 될때 Last처리
		private void ProcessTrayFillDone(FlowTray tray)
		{
			NDm.Write(eNDM.FillDoneRemainCount, NDm.Read(eNDM.FillDoneRemainCount) - 1);
			SM.FMT[tray].ProcStatus = ProcStatus.TrayFillDone;
			GLog.Add($"{tray}:FillDone - Tray Remain Count : {NDm.Read(eNDM.FillDoneRemainCount)}");
		}
		//Lot 완료 시 데이터 초기화 작업
		private void EndLotProcessing()
		{
			NDm.Write(eNDM.FillDoneRemainCount, 0);
			NDm.Write(eNDM.RecipeChange, -1);
			SDm.Write(eSDM.CurTempKeyValue, "NONE");
			NDm.Write(eNDM.AutoRunMode, (int)AutoRunMode.Normal);
			NDm.Write(eNDM.LotEndMode, (int)LotEndMode.Normal);
			if (SM.FMT.IsFlow(FlowTray.Reject) == true)
			{
				var t_info = SM.FMT[FlowTray.Reject].GetInfo();
				t_info.KeyValue = "";
				SM.FMT[FlowTray.Reject].SetInfo(t_info);
			}
			SM.SHM.ClearRunLotInfo();
		}


	}
}
