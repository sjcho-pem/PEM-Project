using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace H_MarsSSD
{
	public class SystemError
	{             
		public enum Cmd
		{
			[DefaultValueAttribute("SYSTEM ERROR")]
			START = 100,
			CHECK,
			
			END = 199,
		}

		private SMM<Cmd>	m_smmErr		= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(Cmd.START));
		private	PStopwatch	m_MESRepTimeOut	= new PStopwatch();
		private	PStopwatch	m_AirAlarmDelay	= new PStopwatch();
		private	bool		m_bSetAlarm		= false;

		public void Init()
		{
			m_smmErr.Next(Cmd.START);
			Start();
		}
		public void Start()
		{
			m_smmErr.eInput = InState.run;
		}

		private	void	SetUIDoorState()
		{
			bool[]	bIsAin	= new bool[] {	G.cKernel.IsAIn(DI.TopFrontDoorSen),
											G.cKernel.IsAIn(DI.TopLeftDoorSen),
											G.cKernel.IsAIn(DI.TopRearDoorSen),
											G.cKernel.IsAIn(DI.TopRightDoorSen),
											G.cKernel.IsAIn(DI.BtmDoorSen)};

			for (int i = 0; i < bIsAin.Length; i++)
			{
				SM.SHM.SetDoorOpen(i, bIsAin[i]);
			}
		}

		/// <summary>
		/// System Error Check Process
		/// </summary>
		public void Run()
		{
            // Tick
            SM.SHM.SetNDMData((uint)eNDM.Ticks, (int)m_smmErr.DelayTick); // Data 저장이 필요없을시...
            //NDm.Write(eNDM.Ticks, (int)m_smmErr.DelayTick); // Data 저장시....
            m_smmErr.Delayms(1);                
            m_smmErr.ResetDelayms();

            // Door 상태 UI 표시용
            SetUIDoorState();

			switch (m_smmErr.CurStep)
			{
				case Cmd.START:
					{
						if (m_smmErr.eInput == InState.run)
						{
							m_smmErr.eInput = InState.none;
							m_smmErr.Next(Cmd.CHECK);
						}
					}
					break;

				case Cmd.CHECK:
					{
						// Alarm Check Add
						Emergency();
						DoorOpen();
						MainAir();
						Reset();
						Comm();
						UBIGEMCheck();

						if (SM.SHM.IsBitData(BIT.Error))
							m_smmErr.Next(Cmd.END);
					}
					break;

				case Cmd.END:
					{
						m_smmErr.Next(Cmd.START);
					}
					break;
			}
		}

		private void Emergency()
		{
			bool bOn = G.cKernel.IsBIn(DI.EMO);

			if (bOn == true)
			{
				G.AlmMng.ADD(ALM.ERROR_EMO);
				SM.SHM.SetBitData(BIT.EMO, true);
				SM.SHM.SetBitData(BIT.AllHome, false);
			}
			else
				SM.SHM.SetBitData(BIT.EMO, false);
		}

		private void Reset()
		{
			bool bOn = G.cKernel.IsBIn(DI.RESET);

			if (bOn)
			{
				G.AlmMng.ADD(ALM.ERROR_RESET);

				if (SM.SHM.IsBitData(BIT.AutoRun) == true  ||
					SM.SHM.IsBitData(BIT.Pause) == true)			G.seqMain.AutoRunStop();
				if (SM.SHM.IsBitData(BIT.AllHoming) == true)		G.allHome.Stop();

				SM.SHM.SetBitData(BIT.AllHome, false);
			}
		}

		private void DoorOpen()
		{
			if (SM.SHM.GetUseSkipData(USESKIP.Safety) == false  ||
				SM.SHM.IsBitData(BIT.KeyLockAuto) == false)
			{
				if (SM.SHM.IsBitData(BIT.DoorOpen) == true)			SM.SHM.SetBitData(BIT.DoorOpen, false);
				return;
			}

			bool bTopRightRearDoorSen = G.seqRejectBuffer.IsTrayChangeReady() == false ? true : G.cKernel.IsAIn(DI.TopRightRearDoorSen);

			bool[]	bIsAin	= new bool[] {	G.cKernel.IsAIn(DI.TopFrontDoorSen),
											G.cKernel.IsAIn(DI.TopLeftDoorSen),
											G.cKernel.IsAIn(DI.TopRearDoorSen),
											G.cKernel.IsAIn(DI.TopRightDoorSen),
											G.cKernel.IsAIn(DI.BtmDoorSen),
											bTopRightRearDoorSen};
			bool	bOn		= false;

			for (int i = 0; i < bIsAin.Length; i++)
			{
				if (bIsAin[i] == false)
				{
					G.AlmMng.ADD(ALM.ERROR_TOP_FRONT_DOOR_OPEN + i);
					bOn	= true;
				}
			}

			if (bOn)
				SM.SHM.SetBitData(BIT.DoorOpen, true);
			else
				SM.SHM.SetBitData(BIT.DoorOpen, false);
		}

		private void MainAir()
		{
            return;
            if (Program._bSimul_ == true)		return;

			if (G.cKernel.IsBIn(DI.CleanerAir) == true)							G.AlmMng.ADD(ALM.ERROR_CLEANER_MAIN_AIR);
			if (G.cKernel.IsBIn(DI.MainAir) == true)							G.AlmMng.ADD(ALM.ERROR_MAIN_AIR);

            if (SM.SHM.IsBitData(BIT.AutoRun) == false)     return;
            if (SM.SHM.IsBitData(BIT.AllHoming) == true)    return;
			
			double	dMainAir		= TypeConversion.GetFlowMeterFromAInput(DI.MainAirFlow)		 ;
			double	dBlowAir		= TypeConversion.GetFlowMeterFromAInput(DI.BlowAirFlow)		 ;
			double	dLDPnPAir		= TypeConversion.GetFlowMeterFromAInput(DI.LDPnPAirFlow)	 ;
			double	dLDInspPnPAir	= TypeConversion.GetFlowMeterFromAInput(DI.LDInspPnPAirFlow) ;
			double	dULDPnPAir		= TypeConversion.GetFlowMeterFromAInput(DI.ULDPnPAirFlow)	 ;
			double	dULDInspPnPAir	= TypeConversion.GetFlowMeterFromAInput(DI.ULDInspPnPAirFlow);
			
			bool bMainAir		= dMainAir		 > DDm.Read(eDDM.MainAirUpThresh)		|| dMainAir			< DDm.Read(eDDM.MainAirDownThresh);
			bool bBlowAir		= dBlowAir		 > DDm.Read(eDDM.BlowAirUpThresh)		|| dBlowAir			< DDm.Read(eDDM.BlowAirDownThresh);
			bool bLDPnPAir		= dLDPnPAir		 > DDm.Read(eDDM.LdPnPAirUpThresh)		|| dLDPnPAir		< DDm.Read(eDDM.LdPnPAirDownThresh);
			bool bLDInspPnPAir	= dLDInspPnPAir	 > DDm.Read(eDDM.LdInspPnPAirUpThresh)	|| dLDInspPnPAir	< DDm.Read(eDDM.LdInspPnPAirDownThresh);
			bool bULDPnPAir		= dULDPnPAir	 > DDm.Read(eDDM.ULdPnPAirUpThresh)		|| dULDPnPAir		< DDm.Read(eDDM.ULdPnPAirDownThresh);
			bool bULDInspPnPAir = dULDInspPnPAir > DDm.Read(eDDM.ULdInspPnPAirUpThresh) || dULDInspPnPAir	< DDm.Read(eDDM.ULdInspPnPAirDownThresh);

            if ((G.cKernel.IsBIn(DI.TrayCleanFilter) == true  && G.cKernel.IsAOut(DO.TrayCleanAirBlow) == true) ||
				(G.cKernel.IsBIn(DI.LDBuf1CleanFilter) == true && G.cKernel.IsAOut(DO.LDBuf1CleanAirBlow)== true) ||
				(G.cKernel.IsBIn(DI.LDBuf2CleanFilter) == true && G.cKernel.IsAOut(DO.LDBuf2CleanAirBlow) == true)  ||
				(G.cKernel.IsAIn(DI.TrayCleanAir) == true)  ||
				(G.cKernel.IsAIn(DI.LDBuf1Air) == true)     ||
				(G.cKernel.IsAIn(DI.LDBuf2Air) == true)     ||
				( bMainAir	== true)		||
				( bBlowAir	== true)		||
				( bLDPnPAir	== true)		||
				( bLDInspPnPAir	 == true)		||
				( bULDPnPAir	 == true)		||
				( bULDInspPnPAir  == true)	)
			{	
				if (m_AirAlarmDelay.IsRunning == false)
				{
					m_AirAlarmDelay.Restart();
				}
				else if (m_AirAlarmDelay.IsTimeOverMs(3000) == true)
				{
					//구동중에 일정값이하로 떨어지면 input 들어오면서 알람(타이밍 안나오면 시퀀스로 이동)
					if (G.cKernel.IsBIn(DI.TrayCleanFilter) == true  && G.cKernel.IsAOut(DO.TrayCleanAirBlow) == true)      G.AlmMng.ADD(ALM.ERROR_SEQ_TRAY_CLEAN_CLEANER_FILTER);
					if (G.cKernel.IsBIn(DI.LDBuf1CleanFilter) == true && G.cKernel.IsAOut(DO.LDBuf1CleanAirBlow)== true)    G.AlmMng.ADD(ALM.ERROR_SEQ_LOAD_BUFFER1_CLEANER_FILTER);
					if (G.cKernel.IsBIn(DI.LDBuf2CleanFilter) == true && G.cKernel.IsAOut(DO.LDBuf2CleanAirBlow) == true)   G.AlmMng.ADD(ALM.ERROR_SEQ_LOAD_BUFFER2_CLEANER_FILTER);

					//제품이 없을때만 체크
                    if (SM.FMT.IsFlowRange(FlowTray.LDConv01, FlowTray.ULDConv31) == false &&
                        SM.FMP.IsFlowRange(FlowPart.LDPnP, FlowPart.ULDPnP) == false)
                    {
                        if (G.cKernel.IsAIn(DI.TrayCleanAir) == true)	G.AlmMng.ADD(ALM.ERROR_SEQ_TRAY_CLEAN_CLEANER_AIR);
                        if (G.cKernel.IsAIn(DI.LDBuf1Air) == true)		G.AlmMng.ADD(ALM.ERROR_SEQ_LOAD_BUFFER1_AIR);
                        if (G.cKernel.IsAIn(DI.LDBuf2Air) == true)		G.AlmMng.ADD(ALM.ERROR_SEQ_LOAD_BUFFER2_AIR);
						if (bMainAir == true) G.AlmMng.ADD(ALM.ERROR_MAIN_AIR);
						if (bBlowAir == true) G.AlmMng.ADD(ALM.ERROR_SEQ_TRAY_CLEAN_CLEANER_AIR);
						if (bLDPnPAir == true) G.AlmMng.ADD(ALM.ERROR_SEQ_LOAD_PnP_AIR);
						if (bLDInspPnPAir == true) G.AlmMng.ADD(ALM.ERROR_SEQ_LOAD_INSP_PnP_AIR);
						if (bULDPnPAir == true) G.AlmMng.ADD(ALM.ERROR_SEQ_UNLOAD_PnP_AIR);
						if (bULDInspPnPAir == true) G.AlmMng.ADD(ALM.ERROR_SEQ_UNLOAD_INSP_PnP_AIR);
					}
				}
			}
			else
			{
				m_AirAlarmDelay.Stop();
				m_AirAlarmDelay.Reset();
			}
		}
		private void UBIGEMCheck()
		{
// 			if (SM.SHM.IsBitData(BIT.UBIGEMCheckingEquipment) == true)
// 			{
// 				G.AlmMng.ADD(ALM.WARNING_CHECKING_EQUIPMENT);
// 			}
			
			if (SM.SHM.IsBitData(BIT.Error) == true && m_bSetAlarm == false)
			{
				for (FlowTray i = 0; i < FlowTray.Max; i++)
				{
					if (SM.FMT.IsFlow(i) == false)	continue;
				}
				m_bSetAlarm = true;
			}
			else if (SM.SHM.IsBitData(BIT.Error) == false)
			{
				m_bSetAlarm = false;
			}
		}

		private void Comm()
		{
			if (SM.SHM.IsBitData(BIT.AutoRun) == true)
			{
				//검사 사용시, Connect가 안되어 있는 경우 알람 처리
				if (SM.SHM.GetUseSkipData(USESKIP.TopInspection) == true  &&
					SM.SHM.IsBitData(BIT.TopInspConnect) == false)
				{
					G.AlmMng.ADD(ALM.ERROR_COMM_TOP_INSP_CONNECT);
				}
				
				if (SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == true  &&
					SM.SHM.IsBitData(BIT.BtmInspConnect) == false)
				{
					G.AlmMng.ADD(ALM.ERROR_COMM_BOTTOM_INSP_CONNECT);
				}
				
				if (SM.SHM.GetUseSkipData(USESKIP.SendRejectProdId) == true  &&
					SM.SHM.IsBitData(BIT.ReviewConnect) == false)
				{
					G.AlmMng.ADD(ALM.ERROR_COMM_REVIEW_CONNECT);
				}

				if (SM.SHM.GetUseSkipData(USESKIP.SGEM) == true  &&
					SM.SHM.IsBitData(BIT.UbiGemConnect) == false)
				{
					G.AlmMng.ADD(ALM.ERROR_SESCGEM_CONNECT);
				}
			}
		}
	}
}
