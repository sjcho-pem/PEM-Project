using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;
using System.ComponentModel;
using System.Windows.Forms;

namespace H_MarsSSD
{
	public class Simulation
	{
		private	PStopwatch[]	m_DelayInspPlate	= new PStopwatch[2] { new PStopwatch(), new PStopwatch() };

		public	CommInsp.InspState	m_InspState		= CommInsp.InspState.Idle;
		private	CommInsp.RunState		m_RunState		= CommInsp.RunState.Idle;
		public	CommInsp.RunState		RunState
		{
			get => m_RunState;
			set => m_RunState = value;
		}



		public	void	Init()
		{
			try
			{
				if (Program._bSimul_.Equals(false))			return;

				G.seqTrayClean.ActPos(SeqTrayClean.Act.TableUpDn, SenPos.DOWN);

				G.cKernel.AOut(DI.RESET,			true);
				G.cKernel.AOut(DI.EMO,				true);
				G.cKernel.AOut(DI.MainAir,			true);
				G.cKernel.AOut(DI.CleanerAir,		true);
				G.cKernel.AOut(DI.TopFrontDoorSen,	true);
				G.cKernel.AOut(DI.TopFrontDoorSen,	true);
				G.cKernel.AOut(DI.TopLeftDoorSen,	true);
				G.cKernel.AOut(DI.TopRearDoorSen,	true);
				G.cKernel.AOut(DI.TopRightDoorSen,	true);
				G.cKernel.AOut(DI.TopRightRearDoorSen,	true);
				G.cKernel.AOut(DI.BtmDoorSen,		true);

                G.cKernel.AOut(DI.LDTrayTransSafetySen, true);
                G.cKernel.AOut(DI.ULDTrayTransSafetySen, true);
                G.cKernel.AOut(DI.TrayTransCollideSen, true);
				
				G.cKernel.BOut(DI.TrayTransCollideSen, false);
				G.cKernel.BOut(DI.InspPnPCollideSen, false);
				G.cKernel.BOut(DI.InspPnPCollideSen, false);

				G.cKernel.BOut(DI.LDTrayLiftAreaSen1, false);
				G.cKernel.BOut(DI.LDTrayLiftAreaSen2, false);
				G.cKernel.BOut(DI.ULDTrayLiftAreaSen1, false);
				G.cKernel.BOut(DI.ULDTrayLiftAreaSen2, false);
				
				G.cKernel.AOut(DI.KeyLock, false);
				
				G.cKernel.AOut(DI.RejectTrayExistSen , true);
				G.cKernel.BOut(DI.LDTrayTableCollideSen , false);

				G.cKernel.BOut(DI.CleanerAir ,false);
				G.cKernel.BOut(DI.LDTrayLiftLimitSen,false);
//				G.cKernel.BOut(DI.LDBuf1CleanFilter,false);	
//				G.cKernel.BOut(DI.LDBuf2CleanFilter,false);	
//				G.cKernel.BOut(DI.LDBuf1Air,false);			
//				G.cKernel.BOut(DI.LDBuf2Air,false);	
// 				G.cKernel.BOut(DI.LDPnPAir,false);		
// 				G.cKernel.BOut(DI.ULDPnPAir,false);			
// 				G.cKernel.BOut(DI.LDInspPnPAir,false);			
// 				G.cKernel.BOut(DI.ULDInspPnPAir,false);		
// 				G.cKernel.BOut(DI.TrayCleanAir,false);		
//				G.cKernel.BOut(DI.TrayCleanFilter,false);		
				
				if (SM.FMP.IsFlow(FlowPart.LDPnP) == true)
				{
					bool[] bexist	= SM.FMP[FlowPart.LDPnP].GetProdExist();

					for (int i = 0; i < bexist.Length; i++)
					{
						if (bexist[i] == true)			G.seqLoadPnP.RunEjector(SenPos.VAC, i);
					}
				}

				if (SM.FMP.IsFlow(FlowPart.ULDPnP) == true)
				{
					bool[] bexist	= SM.FMP[FlowPart.ULDPnP].GetProdExist();

					for (int i = 0; i < bexist.Length; i++)
					{
						if (bexist[i] == true)			G.seqUnloadPnP.RunEjector(SenPos.VAC, i);
					}
				}

				if (SM.FMT.IsFlow(FlowTray.LDTrans) == true)
				{
					G.cKernel.AOut(DI.LDTrayTransExistSen, true);
				}

				if (SM.FMT.IsFlow(FlowTray.ULDTrans) == true)
				{
					G.cKernel.AOut(DI.ULDTrayTransExistSen, true);
				}

				//SM.SHM.SetBitData(BIT.MesConnect, true);
			}
			catch
			{

			}
		}
		public	void	Run()
		{
			try
			{   
				if (Program._bSimul_.Equals(false))			return;

				TrayLoader();
				TrayUnloader();
				TrayClean();
				LoadTrayTable();
				UnloadTrayTable();
				LoadTransfer();
				UnloadTransfer();
				LoadPnP();
				LoadBuffer();
				UnloadBuffer();
				InspPlate(0);
				InspPlate(1);
				LoadInspPnP();
				UnloadInspPnP();
				UnloadPnP();
				RejectBuffer();
				ButtonSeq();
			}
			catch
			{
				
			}
		}

		private	void	TrayLoader()
		{
			try
			{

				if ((NDm.Read(eNDM.AutoRunMode) == (int)AutoRunMode.EndLot ||
					NDm.Read(eNDM.AutoRunMode) == (int)AutoRunMode.QuitLot)  &&
					G.seqTrayLoader.IsReady == true)
				{
					if (SM.FMT.IsFlowRange(FlowTray.LDLift01, FlowTray.LDLift31) == true && G.seqTrayLoader.IsLiftDownEnable() == true)
					{
						G.cKernel.AOut(DI.LDTrayLiftExistSen, false);
					}
				}

				if (G.seqTrayLoader.IsReady == true)
				{
					if (SM.FMT.IsFlowRange(FlowTray.LDLift01, FlowTray.LDLift31) == true ||  SM.FMT.IsFlow(FlowTray.LDWaitBuf))
					{
						if (NDm.Read(eNDM.AutoRunMode) == (int)AutoRunMode.Normal)			G.cKernel.AOut(DI.LDTrayLiftExistSen, true);
					}
					else
					{
						G.cKernel.AOut(DI.LDTrayLiftExistSen, false);
						G.cKernel.AOut(DI.LDTrayLiftDecSen, false);

					}

				}

				if		(G.seqTrayLoader.smmLoad == SeqTrayLoader.Cmd.LOAD_40)
				{
					G.cKernel.AOut(DI.LDTrayLiftExistSen, false);
					G.cKernel.AOut(DI.LDTrayLiftDecSen, false);
					G.cKernel.AOut(DI.LDTrayConvDecSen, true);
				}
				else if (G.seqTrayLoader.smmLoad == SeqTrayLoader.Cmd.LOAD_50)
				{
					G.cKernel.AOut(DI.LDTrayConvStopperSen, true);
					if (SM.SHM.GetRecipeDataToInt(DEVICE.TraySizeChkSen3Use) == 1000)			G.cKernel.AOut(DI.LDTrayConvDecSen, true);
					else																		G.cKernel.AOut(DI.LDTrayConvDecSen, false);
				}
				else
				{
					if (SM.FMT.IsFlowRange(FlowTray.LDConv01, FlowTray.LDConv31) == true )
					{
						G.cKernel.AOut(DI.LDTrayConvExistSen, true);
						G.cKernel.AOut(DI.LDTrayConvStopperSen, true);
						if (SM.SHM.GetRecipeDataToInt(DEVICE.TraySizeChkSen3Use) == 1000)		G.cKernel.AOut(DI.LDTrayConvDecSen, true);
						else																	G.cKernel.AOut(DI.LDTrayConvDecSen, false);
					}
					else
					{
						G.cKernel.AOut(DI.LDTrayConvExistSen, false);
						G.cKernel.AOut(DI.LDTrayConvDecSen, false);
						G.cKernel.AOut(DI.LDTrayConvStopperSen, false);
					}
				}

				if		(G.seqTrayLoader.smmUnload == SeqTrayLoader.Cmd.UNLOAD_30)
				{
					G.cKernel.AOut(DI.LDTrayLiftDecSen, true);
				}
				else if (G.seqTrayLoader.smmUnload == SeqTrayLoader.Cmd.UNLOAD_40)
				{
					G.cKernel.AOut(DI.LDTrayLiftExistSen, true);
				}
				else if (G.seqTrayLoader.smmHome == SeqTrayLoader.Cmd.HOME_92  && SM.FMT.IsFlowRange(FlowTray.LDLift01, FlowTray.LDLift31))
				{
					G.cKernel.AOut(DI.LDTrayLiftExistSen, true);
				}

				if (G.seqTrayLoader.smmLiftUp == SeqTrayLoader.Cmd.LIFTUP_10)
				{
					G.cKernel.AOut(DI.LDTrayLiftDecSen, false);
					G.cKernel.AOut(DI.LDTrayLiftExistSen, false);
				}
				if (G.seqTrayLoader.smmHome == SeqTrayLoader.Cmd.HOME_10)
				{
					G.cKernel.AOut(DI.LDTrayLiftDecSen, SM.FMT.IsFlowRange(FlowTray.LDLift01, FlowTray.LDLift31));
					G.cKernel.AOut(DI.LDTrayLiftExistSen, SM.FMT.IsFlowRange(FlowTray.LDLift01, FlowTray.LDLift31));
				}

			}
			catch
			{

			}
		}

		private	void	TrayUnloader()
		{
			try
			{
				if (G.seqTrayUnloader.IsUnloadReady() == true)
				{
					if (SM.FMT.IsFlowRange(FlowTray.ULDConv01, FlowTray.ULDConv31) == true)			G.cKernel.AOut(DI.ULDTrayConvExistSen, true);
					else																				G.cKernel.AOut(DI.ULDTrayConvExistSen, false);

				//	if (SM.FMT.IsFlowRange(teFlowTray.ULDLift01, teFlowTray.ULDConv31) == true)			G.cKernel.AOut(DI.ULDTrayLiftExistSen, true);
				//	else																				G.cKernel.AOut(DI.ULDTrayLiftExistSen, false);
				}
				else
				{
					if (G.seqTrayUnloader.smmUnload == SeqTrayUnloader.teCmd.UNLOAD_40)
					{
						G.cKernel.AOut(DI.ULDTrayConvExistSen, false);
						G.cKernel.AOut(DI.ULDTrayLiftDecSen, true);
					}
					if (G.seqTrayUnloader.smmUnload == SeqTrayUnloader.teCmd.UNLOAD_50)
					{
						G.cKernel.AOut(DI.ULDTrayLiftDecSen, false);
						G.cKernel.AOut(DI.ULDTrayLiftExistSen, true);
					}
				}

				if (G.seqTrayUnloader.smmLiftUp == SeqTrayUnloader.teCmd.LIFTUP_30)
				{
					G.cKernel.AOut(DI.ULDTrayLiftExistSen, false);
				}

				if (G.seqTrayUnloader.smmHome == SeqTrayUnloader.teCmd.HOME_100)
				{
					if (SM.FMT.IsFlowRange(FlowTray.ULDLift01, FlowTray.ULDLift31) == true )
					{
						G.cKernel.AOut(DI.ULDTrayLiftExistSen, true);
					}
				}
			}
			catch
			{

			}
		}


		private	void	TrayClean()
		{	
			try
			{
				if (G.seqTrayClean.IsReady == true)
				{
					G.cKernel.AOut(DI.TrayCleanExistSen, SM.FMT.IsFlow(FlowTray.TrayClean));
				}
			
				if (G.seqTrayClean.smmGrip == SeqTrayClean.Cmd.GRIP_10)				G.cKernel.AOut(DI.TrayCleanExistSen, true);
				if (G.seqTrayClean.smmRelease == SeqTrayClean.Cmd.RELEASE_20)			G.cKernel.AOut(DI.TrayCleanExistSen, false);
				if (G.seqTrayClean.smmHome == SeqTrayClean.Cmd.HOME_10  &&
					SM.FMT.IsFlow( FlowTray.TrayClean) == false)						G.cKernel.AOut(DI.TrayCleanExistSen, false);
			}
			catch
			{

			}
		}
		
		private	void	LoadTrayTable()
		{	
			try
			{
                Form fc = Application.OpenForms["frmJobTeaching"];
                if (G.seqJobTeaching.IsReady == false || fc!= null )
                {
                    return;
                }
				if		(G.seqLoadTrayTable1.smmGrip == SeqLoadTrayTable1.Cmd.GRIP_10)				G.cKernel.BOut(DI.LDTrayTable1ExistSen, true);
				else if (G.seqLoadTrayTable1.smmRelease == SeqLoadTrayTable1.Cmd.RELEASE_20)		G.cKernel.BOut(DI.LDTrayTable1ExistSen, false);

				if		(G.seqLoadTrayTable2.smmGrip == SeqLoadTrayTable2.Cmd.GRIP_10)				G.cKernel.BOut(DI.LDTrayTable2ExistSen, true);
				else if (G.seqLoadTrayTable2.smmRelease == SeqLoadTrayTable2.Cmd.RELEASE_20)		G.cKernel.BOut(DI.LDTrayTable2ExistSen, false);

				if (G.seqLoadTrayTable1.IsHomeReady() == false)
				{
					if (SM.FMT.IsFlow(FlowTray.LDTable1) == true)			G.cKernel.BOut(DI.LDTrayTable1ExistSen, true);
					else													G.cKernel.BOut(DI.LDTrayTable1ExistSen, false);
				}

				if (G.seqLoadTrayTable2.IsHomeReady() == false)
				{
					if (SM.FMT.IsFlow(FlowTray.LDTable2) == true)			G.cKernel.BOut(DI.LDTrayTable2ExistSen, true);
					else													G.cKernel.BOut(DI.LDTrayTable2ExistSen, false);
				}
			}
			catch
			{

			}
		}

		private	void	UnloadTrayTable()
		{	
			try
			{
                
                Form fc = Application.OpenForms["frmJobTeaching"];
                if (G.seqJobTeaching.IsReady == false || fc!= null )
                {
                    return;
                }
				if		(G.seqUnloadTrayTable1.smmGrip == SeqUnloadTrayTable.Cmd.GRIP_10)				G.cKernel.BOut(DI.ULDTrayTable1ExistSen, true);
				else if (G.seqUnloadTrayTable1.smmRelease == SeqUnloadTrayTable.Cmd.RELEASE_20)		G.cKernel.BOut(DI.ULDTrayTable1ExistSen, false);
				else																					G.cKernel.BOut(DI.ULDTrayTable1ExistSen, SM.FMT.IsFlow( FlowTray.ULDTable1));

				if		(G.seqUnloadTrayTable2.smmGrip == SeqUnloadTrayTable.Cmd.GRIP_10)				G.cKernel.BOut(DI.ULDTrayTable2ExistSen, true);
				else if (G.seqUnloadTrayTable2.smmRelease == SeqUnloadTrayTable.Cmd.RELEASE_20)		G.cKernel.BOut(DI.ULDTrayTable2ExistSen, false);
				else																					G.cKernel.BOut(DI.ULDTrayTable2ExistSen, SM.FMT.IsFlow( FlowTray.ULDTable2));
			}
			catch
			{

			}
		}

		private	void	LoadTransfer()
		{
			try
			{
				if		(G.seqLoadTransfer.smmPickConv == SeqLoadTransfer.Cmd.PICK_CONV_60)			G.cKernel.BOut(DI.LDTrayTransPosSen,	false);
				else if (G.seqLoadTransfer.smmPickConv == SeqLoadTransfer.Cmd.PICK_CONV_70)			G.cKernel.BOut(DI.LDTrayTransPosSen,	true);
				
				if		(G.seqLoadTransfer.smmPlaceConv == SeqLoadTransfer.Cmd.PLACE_CONV_70)			G.cKernel.BOut(DI.LDTrayTransPosSen,	false);
				if		(G.seqLoadTransfer.smmPlaceConv == SeqLoadTransfer.Cmd.PLACE_CONV_80)			G.cKernel.BOut(DI.LDTrayTransPosSen,	true);

				G.cKernel.AOut(DI.LDTrayTransExistSen,	SM.FMT.IsFlow(FlowTray.LDTrans));
			}
			catch
			{

			}
		}

		private	void	UnloadTransfer()
		{
			try
			{
				if		(G.seqUnloadTransfer.smmPlaceConv == SeqUnloadTransfer.Cmd.PLACE_CONV_50)
				{
					G.cKernel.BOut(DI.ULDTrayTransPosSen, true);
				}
				else if (G.seqUnloadTransfer.smmPlaceConv == SeqUnloadTransfer.Cmd.PLACE_CONV_70)
				{
					G.cKernel.BOut(DI.ULDTrayTransPosSen, false);
				}
				else if (G.seqUnloadTransfer.smmPlaceConv == SeqUnloadTransfer.Cmd.PLACE_CONV_80)
				{
					G.cKernel.BOut(DI.ULDTrayTransPosSen, true);
				
					if (SM.FMT.IsFlowRange(FlowTray.ULDConv01, FlowTray.ULDConv31) == true)
					{
						G.cKernel.AOut(DI.ULDTrayConvExistSen, true);
					}
				}

				if (G.seqUnloadTransfer.smmPlace == SeqUnloadTransfer.Cmd.PLACE_CHECKCAPCOVER_1 || 
					G.seqUnloadTransfer.smmPlace == SeqUnloadTransfer.Cmd.PLACE_CHECKCAPCOVER_2)
				{
					G.cKernel.AOut(DI.ULDTrayTransExistSen,	false);
				}
				else 
				{ 
					G.cKernel.AOut(DI.ULDTrayTransExistSen,	SM.FMT.IsFlow(FlowTray.ULDTrans));
				}

				
			}
			catch
			{

			}
		}

		private	void	LoadPnP()
		{
			try
			{
				bool[]	bexist	= SM.FMP[FlowPart.LDPnP].GetProdExist();

				for (int i = 0; i <  SM.FMP[FlowPart.LDPnP].GetMaxProdCount(); i++)
				{
						if (G.cKernel.IsAOut(DO.LDPnPVac1 + i) == true)			G.cKernel.AOut(DI.LDPnPVac1+i, true);
						else 													G.cKernel.AOut(DI.LDPnPVac1+i, false);
				}
			}
			catch
			{

			}
		}

		private	void	LoadBuffer()
		{
			try
			{
				bool[]	bexist1	= new bool[] { false, false, false, false, false, false, false, false };
				bool[]	bexist2	= new bool[] { false, false, false, false, false, false, false, false };
				
				if (SM.FMP.IsFlow(FlowPart.LDBuffer1) == true)			bexist1	= SM.FMP[FlowPart.LDBuffer1].GetProdExist();
				for(int i = 0; i < bexist1.Length; i++)
				{
					if (bexist1[i] == false)			G.cKernel.AOut(DI.LDBuf1Grip1ExistSen+i, true);
					else							G.cKernel.AOut(DI.LDBuf1Grip1ExistSen+i, false);

					//G.cKernel.AOut(DI.LDBuf1ExistSen7, false);
				}

				if (SM.FMP.IsFlow(FlowPart.LDBuffer2) == true)			bexist2	= SM.FMP[FlowPart.LDBuffer2].GetProdExist();
				for(int i = 0; i < bexist2.Length; i++)
				{
					if (bexist2[i] == false)			G.cKernel.AOut(DI.LDBuf2Grip1ExistSen+i, true);
					else							G.cKernel.AOut(DI.LDBuf2Grip1ExistSen+i, false);
				}
			}
			catch
			{

			}
		}

		private	void	UnloadBuffer()
		{
			try
			{
				bool[]	bexist	= new bool[] { false, false, false, false, false, false, false, false };

				if (SM.FMP.IsFlow(FlowPart.ULDBuffer) == true)			bexist	= SM.FMP[FlowPart.ULDBuffer].GetProdExist();
				for(int i = 0; i < bexist.Length; i++)
				{
					if (bexist[i] == true)			G.cKernel.AOut(DI.ULDBufExistSen1+i, true);
					else							G.cKernel.AOut(DI.ULDBufExistSen1+i, false);
				}
			}
			catch
			{

			}
		}

		private	void	InspPlate(int nPart)
		{
			try
			{
				FlowPart		flow			= FlowPart.Insp1;
				SeqInspPlate	seq				= G.seqInspPlate1;
				DI				di_exist1		= DI.InspPlate1ExistSen1;
				DI				dI_top_insp_end	= DI.TopInspEnd1;
				DI				dI_btm_insp_end	= DI.BtmInspEnd1;
				bool[]			bexist			= SM.FMP[flow].GetProdExist();

				if (nPart == 1)
				{
					flow			= FlowPart.Insp2;
					seq				= G.seqInspPlate2;
					di_exist1		= DI.InspPlate2ExistSen1;
					dI_top_insp_end	= DI.TopInspEnd2;
					dI_btm_insp_end	= DI.BtmInspEnd2;
					bexist			= SM.FMP[flow].GetProdExist();
				}

				for(int i = 0; i < SM.FMP[flow].GetMaxProdCount(); i++)
				{
					G.cKernel.AOut(di_exist1+i, SM.FMP[flow].GetProdExist()[i]);
				}

                if (seq.smmHome == SeqInspPlate.Cmd.HOME_30)
                {
                    m_InspState	= CommInsp.InspState.Idle;
                }
				switch (seq.smmInspWait)
				{
					case SeqInspPlate.Cmd.INSP_WAIT_120 :
						G.cKernel.AOut(dI_top_insp_end, false);
						G.cKernel.AOut(dI_btm_insp_end, false);
						break;
					case SeqInspPlate.Cmd.INSP_WAIT_160 :
						m_DelayInspPlate[nPart].Restart();
						break;
					case SeqInspPlate.Cmd.INSP_WAIT_170 :
						if (m_DelayInspPlate[nPart].IsTimeOverMs(2000) == false)			break;

						G.cKernel.AOut(dI_top_insp_end, true);
						G.cKernel.AOut(dI_btm_insp_end, true);
						break;
					case SeqInspPlate.Cmd.END :
						m_DelayInspPlate[nPart].Stop();

						G.cKernel.AOut(dI_top_insp_end, false);
						G.cKernel.AOut(dI_btm_insp_end, false);
						break;
				}

				switch (seq.smmInspWait)
				{
					case SeqInspPlate.Cmd.INSP_WAIT_80:
					case SeqInspPlate.Cmd.INSP_WAIT_90:
						m_InspState	= CommInsp.InspState.Idle;
                        break;
					case SeqInspPlate.Cmd.INSP_WAIT_40:
					case SeqInspPlate.Cmd.INSP_WAIT_50:
					case SeqInspPlate.Cmd.INSP_WAIT_60:
						m_RunState	= CommInsp.RunState.Run;
						break;
					case SeqInspPlate.Cmd.INSP_WAIT_150:
					case SeqInspPlate.Cmd.INSP_WAIT_160:
						m_InspState	= CommInsp.InspState.Insp;
						break;
				}
                
				switch (seq.smmReference)
				{
					case SeqInspPlate.Cmd.REFERENCE_100:
					case SeqInspPlate.Cmd.REFERENCE_90:
						m_InspState	= CommInsp.InspState.Idle;
						break;
					case SeqInspPlate.Cmd.REFERENCE_60:
					case SeqInspPlate.Cmd.REFERENCE_50:
						m_InspState	= CommInsp.InspState.Ref;
						break;
				}

				if (seq.IsHomeReady() == false)
				{
					if		(seq == G.seqInspPlate1)
					{
						for (int i = 0; i < bexist.Length; i++)
						{
							G.cKernel.AOut(DI.InspPlate1ExistSen1+i, bexist[i]);
						}
					}
					else if (seq == G.seqInspPlate2)
					{
						for (int i = 0; i < bexist.Length; i++)
						{
							G.cKernel.AOut(DI.InspPlate2ExistSen1+i, bexist[i]);
						}
					}
				}
			}
			catch
			{

			}
		}

		private	void	LoadInspPnP()
		{
			try
			{
				bool[]	bexist	= SM.FMP[FlowPart.LDInspPnP].GetProdExist();

				for (int i = 0; i < SM.FMP[FlowPart.LDInspPnP].GetMaxProdCount(); i++)
				{
						if (G.cKernel.IsAOut(DO.LDInspPnPVac1 + i) == true )			G.cKernel.AOut(DI.LDInspPnPVac1+i, true);
						else 																				G.cKernel.AOut(DI.LDInspPnPVac1+i, false);
					//G.cKernel.AOut(DI.LDInspPnPVac1+i, bexist[i]);
				}
			}
			catch
			{

			}
		}

		private	void	UnloadInspPnP()
		{
			try
			{
				bool[]	bexist	= SM.FMP[FlowPart.ULDInspPnP].GetProdExist();

				for (int i = 0; i < SM.FMP[FlowPart.ULDInspPnP].GetMaxProdCount(); i++)
				{
						if (G.cKernel.IsAOut(DO.ULDInspPnPVac1 + i) == true)			G.cKernel.AOut(DI.ULDInspPnPVac1+i, true);
						else 																				G.cKernel.AOut(DI.ULDInspPnPVac1+i, false);
					//G.cKernel.AOut(DI.ULDInspPnPVac1+i, bexist[i]);
				}
				
			}
			catch
			{

			}
		}

		private	void	UnloadPnP()
		{
			try
			{
				bool[]	bexist	= SM.FMP[FlowPart.ULDPnP].GetProdExist();

				for (int i = 0; i < SM.FMP[FlowPart.ULDPnP].GetMaxProdCount(); i++)
				{
					if (G.cKernel.IsAOut(DO.ULDPnPVac1 + i) == true )			G.cKernel.AOut(DI.ULDPnPVac1+i, true);
					else 																			G.cKernel.AOut(DI.ULDPnPVac1+i, false);

					//if (G.seqUnloadPnP.IsPickBufReady() == false)
					//{
					//	if (G.cKernel.IsAOut(DO.ULDPnPVac1 + i) == true   ||  bexist[i] == true)			G.cKernel.AOut(DI.ULDPnPVac1+i, true);
					//	else 																							G.cKernel.AOut(DI.ULDPnPVac1+i, false);
					//}
					//else
					//{
					//	G.cKernel.AOut(DI.ULDPnPVac1+i, bexist[i]);
					//}
				}
			}
			catch
			{

			}
		}

		private	void	RejectBuffer()
		{
			try
			{
				if (G.seqRejectBuffer.smmTrayChange == SeqRejectBuffer.Cmd.TRAYCHANGE_20)
				{
					G.cKernel.AOut(DI.RejectTrayExistSen , false);
					G.cKernel.AOut(DI.TopRightRearDoorSen, false);
//					G.cKernel.AOut(DI.RejectTrayDirSen , false);
				}
				else if (G.seqRejectBuffer.smmTrayChange == SeqRejectBuffer.Cmd.TRAYCHANGE_30)
                {
					G.cKernel.AOut(DI.RejectTrayExistSen , true);
                    G.cKernel.AOut(DI.TopRightRearDoorSen, true);
                }
				else if (G.seqRejectBuffer.IsTrayChangeReady() == false)
				{

					if (SM.SHM.GetRecipeData(DEVICE.ModuleType) == EnumHelper.GetDescription(ModuleType.LPCAMM))
					{
						G.cKernel.AOut(DI.RejectTrayExistSen, true);
						G.cKernel.AOut(DI.RejectTraySizeSen, true);
					}
					else
					{
						G.cKernel.AOut(DI.RejectTrayExistSen, true);
						G.cKernel.AOut(DI.RejectTraySizeSen, false);
					}
				}
            }
			catch
			{

			}
		}

		private	void	ButtonSeq()
        {
              
			try
			{
                
				switch (G.seqBtn.smmStart)
				{
                    case SeqButton.Cmd.START2:
                    case SeqButton.Cmd.START3:
                        m_RunState = CommInsp.RunState.Idle;
                        m_InspState = CommInsp.InspState.Idle;
                        break;
                    case SeqButton.Cmd.START8:
                        m_RunState = CommInsp.RunState.Run;
                        break;
                }

            }
            catch
			{

			}
		}      



		public	Random	rd	= new Random();
		public	void	SimVerify(string strMode)
		{
			var	tcinfo	= SM.FMT[FlowTray.Reject].GetInfo();

			for (int i = 0; i < tcinfo.MaxCntProdInTray; i++)
			{
				if (tcinfo.ProdInfo[i].VerifyTopDone == true  &&
					tcinfo.ProdInfo[i].VerifyBtmDone == true)
				{
					continue;
				}

				if (tcinfo.ProdInfo[i].Exist == true)
				{
					switch (strMode)
					{
						case "All OK":
							tcinfo.ProdInfo[i].ResultTop		= (int)ProdStatus.OK;
							tcinfo.ProdInfo[i].ResultBtm		= (int)ProdStatus.OK;
							break;

						case "All NG":
							tcinfo.ProdInfo[i].ResultTop		= (int)ProdStatus.NG;
							tcinfo.ProdInfo[i].ResultBtm		= (int)ProdStatus.NG;
							break;

						default:

							if (tcinfo.TrayBarcode.Contains("error"))
							{
								tcinfo.ProdInfo[i].ResultTop = (int)ProdStatus.NG;
								tcinfo.ProdInfo[i].ResultBtm = (int)ProdStatus.NG;
							}
							else
							{
								if (rd.Next(2) == 1)
								{
									tcinfo.ProdInfo[i].ResultTop = (int)ProdStatus.NG;
									tcinfo.ProdInfo[i].ResultBtm = (int)ProdStatus.NG;
								}
								else
								{
									tcinfo.ProdInfo[i].ResultTop = (int)ProdStatus.OK;
									tcinfo.ProdInfo[i].ResultBtm = (int)ProdStatus.OK;
								}
							}
							break;
					}

					tcinfo.ProdInfo[i].VerifyTopDone	= true;
					tcinfo.ProdInfo[i].VerifyBtmDone	= true;
				}
			}

			SM.FMT[FlowTray.Reject].SetInfo(tcinfo);
		}

		
		private	int				m_cnt_scan = 0;
		private	List<string>	m_lt_scan_data = new List<string>();
		public void TrayScanStart(bool _bIsUPT)
		{
			try
			{
				bool	bIsUPT = _bIsUPT;
				m_cnt_scan = 0;
				m_lt_scan_data.Clear();

				for (int i = 30; i >= 0; i--)
				{
					if (SM.FMT.IsFlow(FlowTray.LDLift01 + i) == true)
					{
						m_lt_scan_data.Insert(0, SM.FMT[FlowTray.LDLift01  + i].GetInfo().TrayBarcode);

						m_cnt_scan++;
					}
				}
				m_cnt_scan++;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
		
		public	List<string>	GetTrayScanData()
		{
			return m_lt_scan_data;
		}
	}
}
