using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace H_MarsSSD
{
	public class AllHome
	{
		public enum Cmd
		{
			START = 100,
			INIT,

			Home_10,
			Home_20,
			Home_30,
			Home_40,
			Home_50,
			Home_60,
			Home_70,
			Home_80,
			Home_90,
			Home_100,
			Home_110,
			Home_120,
			
			FTV_INIT_10,
			FTV_INIT_20,
			FTV_INIT_30,
			FTV_INIT_40,
			FTV_INIT_50,
			FTV_INIT_60,
			FTV_INIT_70,
			FTV_INIT_80,
			FTV_INIT_90,
			FTV_INIT_100,

			INSP_INIT_10,
			INSP_INIT_20,
			INSP_INIT_30,
			INSP_INIT_40,
			INSP_INIT_50,
			INSP_INIT_60,
			INSP_INIT_70,
			INSP_INIT_75,
			INSP_INIT_80,
			INSP_INIT_90,
			INSP_INIT_100,
			
			END_NG = 199,
			END_OK,
		}

		private	SMM<Cmd>	m_smmHome			= new SMM<Cmd>(Cmd.START, "ALL HOME PROCESS");
		private	SMM<Cmd>	m_smmTopInsp		= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.INSP_TOP_HOME));
		private	SMM<Cmd>	m_smmBtmInsp		= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.INSP_BTM_HOME));
		private	SMM<Cmd>	m_smmFtvInit		= new SMM<Cmd>(Cmd.START, "FTV_INIT");
		
		public Cmd smmHome { get => m_smmHome.CurStep; }
		public Cmd smmTopInsp { get => m_smmTopInsp.CurStep; }
		public Cmd smmBtmInsp { get => m_smmBtmInsp.CurStep; }

		private	int			m_nTopInspInitReuslt	= 0;
		private	int			m_nBtmInspInitReuslt	= 0;
		private	int			m_nFTVInitReuslt	= 0;
		private	int			m_nFTVTryCount		= 0;
		private	bool		m_IsMtHome = true;

		public	void	Init()
		{
			try
			{
				if (NDm.Read(eNDM.TimeoutAllHome) <= 0)			NDm.Write(eNDM.TimeoutAllHome, 60000);

				m_smmHome.Next(Cmd.START);
				m_smmTopInsp.Next(Cmd.START);
				m_smmBtmInsp.Next(Cmd.START);
				m_smmFtvInit.Next(Cmd.START);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		//false 시 레디 동작만 진행
		public	void	Start(bool bHomeMove = true)
		{
			try
			{
				m_IsMtHome		= bHomeMove;
				m_smmHome.eInput	= InState.run;
				m_smmTopInsp.eInput	= InState.run;
				m_smmBtmInsp.eInput	= InState.run;
				m_smmFtvInit.eInput	= InState.run;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
		
		public	void	Stop()
		{
			try
			{
				if (m_smmHome.IsReady == false)
				{
					m_smmHome.Next(Cmd.END_NG);
				}
				
				if (m_smmTopInsp.IsReady == false)
				{
					m_smmTopInsp.Next(Cmd.END_NG);
				}
				
				if (m_smmBtmInsp.IsReady == false)
				{
					m_smmBtmInsp.Next(Cmd.END_NG);
				}
				
				if (m_smmFtvInit.IsReady == false)
				{
					m_smmFtvInit.Next(Cmd.END_NG);
				}


			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public	bool	IsHomeReady { get => m_smmHome.IsReady; }
		
		private bool	IsMachineEmpty()
		{
			//Check : 범위 확인
			try 
			{
				if (SM.FMT.IsFlowRange(FlowTray.LDLift01, FlowTray.ULDTable2) == true)								return false;
				if (SM.FMT.IsFlow(FlowTray.Reject) == true  &&  SM.FMT[FlowTray.Reject].GetVerifyNotCount() > 0)	return false;
				if (SM.FMP.IsFlowRange(FlowPart.LDPnP, FlowPart.ULDPnP) == true)									return false;

				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		private void	SetUDAct(Seq seq, SenPos senPos)
        {
            switch (seq)
            {  
                case Seq.LoadPnP:			G.seqLoadPnP.RunCylUpDown(senPos);			break;
                case Seq.UnloadPnP:		G.seqUnloadPnP.RunCylUpDown(senPos);		break;
                case Seq.UnloadInspPnP:	G.seqUnloadInspPnP.RunCylUpDown(senPos);	break;
                case Seq.LoadInspPnP:		G.seqLoadInspPnP.RunCylUpDown(senPos);		break;
                default: return;
            }
        }

		private bool	IsUDAct(Seq seq, SenPos senPos)
        {
            switch (seq)
            {

                case Seq.LoadPnP:			return G.seqLoadPnP.CheckCylUpDown(senPos);
                case Seq.UnloadPnP:		return G.seqUnloadPnP.CheckCylUpDown(senPos);
                case Seq.UnloadInspPnP:	return G.seqUnloadInspPnP.CheckCylUpDown(senPos);
                case Seq.LoadInspPnP:		return G.seqLoadInspPnP.CheckCylUpDown(senPos);
                default: return false;
            }
        }

		private void	SetGripAct(Seq seq, SenPos senPos)
		{
			switch (seq)
			{
				case Seq.LoadPnP:				G.seqLoadPnP.RunEjector(senPos);					break;
				case Seq.UnloadPnP:			G.seqUnloadPnP.RunEjector(senPos);					break;
				case Seq.UnloadInspPnP:		G.seqUnloadInspPnP.RunEjector(senPos);				break;
				case Seq.LoadInspPnP:			G.seqLoadInspPnP.RunEjector(senPos);				break;
				default: return;
			}
		}

		private bool	IsGripAct(Seq seq, SenPos senPos)
		{
			switch (seq)
			{

				case Seq.LoadPnP:			return G.seqLoadPnP.CheckEjector(senPos);
				case Seq.UnloadPnP:		return G.seqUnloadPnP.CheckEjector(senPos);
				case Seq.UnloadInspPnP:	return G.seqUnloadInspPnP.CheckEjector(senPos);
				case Seq.LoadInspPnP:		return G.seqLoadInspPnP.CheckEjector(senPos);
				default: return false;
			}
		}

		//그립이 풀려있거나, 안풀려도 되는지 여부
		private bool	IsRelease(Seq seq)
		{
			switch (seq)
			{
				case Seq.LoadTrayTable1:		return SM.FMT.IsFlow(FlowTray.LDTable1) == true  ||  G.seqLoadTrayTable1.IsRelease() == true;
				case Seq.LoadTrayTable2:		return SM.FMT.IsFlow(FlowTray.LDTable2) == true  ||  G.seqLoadTrayTable2.IsRelease() == true;
				case Seq.TrayClean:			return SM.FMT.IsFlow(FlowTray.TrayClean) == true  ||  G.seqTrayClean.IsRelease() == true;
				case Seq.UnloadTrayTable1:	return SM.FMT.IsFlow(FlowTray.ULDTable1) == true  ||  G.seqUnloadTrayTable1.IsRelease() == true;
				case Seq.UnloadTrayTable2:	return SM.FMT.IsFlow(FlowTray.ULDTable2) == true  ||  G.seqUnloadTrayTable2.IsRelease() == true;
				case Seq.LoadBuffer1 :
					if (SM.FMP.IsFlow(FlowPart.LDBuffer1) == true)								return true;
					if (G.seqLoadBuffer1.IsActPos(SeqLoadBuffer.Act.Grip, SenPos.TABLE_RELEASE))	return true;
				
					return false;

				case Seq.LoadBuffer2:
					if (SM.FMP.IsFlow(FlowPart.LDBuffer2) == true)								return true;
					if (G.seqLoadBuffer2.IsActPos(SeqLoadBuffer.Act.Grip, SenPos.TABLE_RELEASE))	return true;
					
					return false;

				case Seq.UnloadBuffer:
					if (SM.FMP.IsFlow(FlowPart.ULDBuffer) == true)		return true;
					
					if ((G.cKernel.Motor(MT.ULDBufGripX).IsRdyPos(0) == true  ||  
						 G.cKernel.Motor(MT.ULDBufGripX).IsRdyPos(G.cKernel.Motor(MT.ULDBufGripX).GetIndexPos((int)SeqUnloadBuffer.PosGripX.UNLOADRELEASE)) == true)  &&
						(G.cKernel.Motor(MT.ULDBufGripY).IsRdyPos(0) == true  ||  
						 G.cKernel.Motor(MT.ULDBufGripY).IsRdyPos(G.cKernel.Motor(MT.ULDBufGripY).GetIndexPos((int)SeqUnloadBuffer.PosGripY.UNLOADRELEASE)) == true)) return true;

					return false;

				case Seq.InspPlate1:
					if (SM.SHM.GetUsePart(UsePartName.Insp1) == UsePartMode.Disable)	return true;
					if (SM.FMP.IsFlow(FlowPart.Insp1) == true)							return true;
					
					if ((G.cKernel.Motor(MT.InspPlate1Grip).IsRdyPos(0) == true  ||  
						 G.cKernel.Motor(MT.InspPlate1Grip).IsRdyPos(G.cKernel.Motor(MT.InspPlate1Grip).GetIndexPos((int)SeqInspPlate.PosGrip.READY)) == true)  &&
						G.seqInspPlate1.IsActPos(SeqInspPlate.Act.Grip, SenPos.TABLE_RELEASE) == true) return true;

					return false;

				case Seq.InspPlate2:
					if (SM.SHM.GetUsePart(UsePartName.Insp2) == UsePartMode.Disable)	return true;
					if (SM.FMP.IsFlow(FlowPart.Insp2) == true)							return true;
					
					if ((G.cKernel.Motor(MT.InspPlate2Grip).IsRdyPos(0) == true  ||  
						 G.cKernel.Motor(MT.InspPlate2Grip).IsRdyPos(G.cKernel.Motor(MT.InspPlate2Grip).GetIndexPos((int)SeqInspPlate.PosGrip.READY)) == true)  &&
						G.seqInspPlate2.IsActPos(SeqInspPlate.Act.Grip, SenPos.TABLE_RELEASE) == true) return true;

					return false;
				default:
					return false;
			}
		}
		
		private void	SetRelease(Seq seq)
		{
			switch (seq)
			{
				case Seq.LoadTrayTable1:		G.seqLoadTrayTable1.ReleaseStart(true);									break;
				case Seq.LoadTrayTable2:		G.seqLoadTrayTable2.ReleaseStart();									break;
				case Seq.TrayClean:				G.seqTrayClean.ReleaseStart();											break;
				case Seq.UnloadTrayTable1:		G.seqUnloadTrayTable1.ReleaseStart(true);								break;
				case Seq.UnloadTrayTable2:		G.seqUnloadTrayTable2.ReleaseStart(true);								break;
				case Seq.LoadBuffer1:			G.seqLoadBuffer1.ActPos(SeqLoadBuffer.Act.Grip, SenPos.TABLE_RELEASE);	break;
				case Seq.LoadBuffer2:			G.seqLoadBuffer2.ActPos(SeqLoadBuffer.Act.Grip, SenPos.TABLE_RELEASE);	break;
				case Seq.UnloadBuffer:
					{
						if (m_IsMtHome)
						{
                            if (G.cKernel.Motor(MT.ULDBufGripX).GetHomeState() == HomeState.Idle  && 
                                G.cKernel.Motor(MT.ULDBufGripY).GetHomeState() == HomeState.Idle)
                            {
                                G.cKernel.Motor(MT.ULDBufGripX).StartHoming();
                                G.cKernel.Motor(MT.ULDBufGripY).StartHoming();
                            }
						}
						else
						{
							G.cKernel.Motor(MT.ULDBufGripY).MoveIndex((int)SeqUnloadBuffer.PosGripY.UNLOADRELEASE);
							G.cKernel.Motor(MT.ULDBufGripX).MoveIndex((int)SeqUnloadBuffer.PosGripX.UNLOADRELEASE);
						}
					}
					break;
				case Seq.InspPlate1:
					{
						if (m_IsMtHome)
						{								
							G.cKernel.Motor(MT.InspPlate1Grip).StartHoming(); 
							G.seqInspPlate1.ActPos(SeqInspPlate.Act.Grip, SenPos.TABLE_RELEASE); 
						}
						else
						{
							G.seqInspPlate1.ActPos(SeqInspPlate.Act.Grip, SenPos.TABLE_RELEASE); 
							G.cKernel.Motor(MT.InspPlate1Grip).MoveIndex((int)SeqInspPlate.PosGrip.READY);
						}
					}
					break;

				case Seq.InspPlate2:
					{
						if (m_IsMtHome)
						{								
							G.cKernel.Motor(MT.InspPlate2Grip).StartHoming(); 
							G.seqInspPlate2.ActPos(SeqInspPlate.Act.Grip, SenPos.TABLE_RELEASE); 
						}
						else
						{
							G.seqInspPlate2.ActPos(SeqInspPlate.Act.Grip, SenPos.TABLE_RELEASE); 
							G.cKernel.Motor(MT.InspPlate2Grip).MoveIndex((int)SeqInspPlate.PosGrip.READY);
						}
					}
					break;
			}
		}


		/// <summary>
		/// All Home Process
		/// </summary>
		public	void	Run()
		{
			try
			{
				if (SM.SHM.IsBitData(BIT.Error) == true)
				{
					if (m_smmHome.CurStep != Cmd.START)			m_smmHome.Next(Cmd.END_NG);
					if (m_smmTopInsp.CurStep != Cmd.START)		m_smmTopInsp.Next(Cmd.END_NG);
					if (m_smmBtmInsp.CurStep != Cmd.START)		m_smmBtmInsp.Next(Cmd.END_NG);
					if (m_smmFtvInit.CurStep != Cmd.START)		m_smmFtvInit.Next(Cmd.END_NG);
				}

				Home(m_smmHome);
				TopInspInit(m_smmTopInsp);
				BtmInspInit(m_smmBtmInsp);
				FTVInit(m_smmFtvInit);

			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public	void	Home(SMM<Cmd> smm)
		{
			switch (smm.CurStep)
			{
				case Cmd.START:
					{
						if (smm.eInput == InState.run)
						{
							smm.eInput = InState.none;
							smm.Next(Cmd.INIT);
						}
					}
					break;

				case Cmd.INIT:
					{
                        if (m_IsMtHome == false) SeqInit();

                        UI.PageMainDisp.Invoke(new System.Windows.Forms.MethodInvoker(delegate ()
								{
									Form fc = Application.OpenForms["frmAllHomeMonitor"];
									if (fc != null)
									{
										if (fc.WindowState == FormWindowState.Minimized)
										{  // 폼을 최소화시켜 하단에 내려놓았는지 검사
											fc.WindowState = FormWindowState.Normal;
										}
										((frmAllHomeMonitor)fc).ResetStatus();
										fc.Activate();
										return;
									}
									frmAllHomeMonitor popup = new frmAllHomeMonitor();
									popup.Show();
								}));

						int nBeforeRecipeIdx	= NDm.Read(eNDM.OldRecipeIdx);
						int nNowRecipeIdx		= UI.SubpageRecipe.iNowRecipeNum;
						if (nBeforeRecipeIdx != nNowRecipeIdx)
						{
							if (SM.FMT.IsFlowRange(FlowTray.LDConv01, FlowTray.ULDTable2) == true  ||
								SM.FMP.IsFlowRange(FlowPart.LDPnP, FlowPart.ULDPnP) == true )
							{
								frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Memory Size Change ! Please Check Tray");
								pMsg.ShowDialog();
								smm.Next(Cmd.END_NG);
								break;
							}
							SeqInit();
						}
						else if (SM.SHM.IsBitData(BIT.Pause) == true  ||
							SM.SHM.IsBitData(BIT.Error) == true)
						{
							G.AlmMng.ADD(ALM.ERROR_SEQ_ALLHOME_CANT_ALL_HOME);
							smm.Next(Cmd.END_NG);
							break;
						}
						
					
						if (G.seqLoadTrayTable1.IsReady == false )		{ G.AlmMng.ADD(ALM.ERROR_SEQ_ALLHOME_CANT_ALL_HOME_LOAD_TRAY_TABLE1);		smm.Next(Cmd.END_NG); break; }
						if (G.seqLoadTrayTable2.IsReady == false )		{ G.AlmMng.ADD(ALM.ERROR_SEQ_ALLHOME_CANT_ALL_HOME_LOAD_TRAY_TABLE2);		smm.Next(Cmd.END_NG); break; }
						if (G.seqUnloadTrayTable1.IsReady == false )	{ G.AlmMng.ADD(ALM.ERROR_SEQ_ALLHOME_CANT_ALL_HOME_UNLOAD_TRAY_TABLE1);		smm.Next(Cmd.END_NG); break; }
						if (G.seqUnloadTrayTable2.IsReady == false )	{ G.AlmMng.ADD(ALM.ERROR_SEQ_ALLHOME_CANT_ALL_HOME_UNLOAD_TRAY_TABLE2);		smm.Next(Cmd.END_NG); break; }
						if (G.seqUnloadPnP.IsReady == false )			{ G.AlmMng.ADD(ALM.ERROR_SEQ_ALLHOME_CANT_ALL_HOME_UNLOAD_PNP);				smm.Next(Cmd.END_NG); break; }
						if (G.seqLoadPnP.IsReady == false )				{ G.AlmMng.ADD(ALM.ERROR_SEQ_ALLHOME_CANT_ALL_HOME_LOAD_PNP);				smm.Next(Cmd.END_NG); break; }
						if (G.seqLoadBuffer1.IsReady == false )			{ G.AlmMng.ADD(ALM.ERROR_SEQ_ALLHOME_CANT_ALL_HOME_LOAD_BUFFER1);			smm.Next(Cmd.END_NG); break; }
						if (G.seqLoadBuffer2.IsReady == false )			{ G.AlmMng.ADD(ALM.ERROR_SEQ_ALLHOME_CANT_ALL_HOME_LOAD_BUFFER2);			smm.Next(Cmd.END_NG); break; }
						if (G.seqTrayLoader.IsReady == false )			{ G.AlmMng.ADD(ALM.ERROR_SEQ_ALLHOME_CANT_ALL_HOME_TRAY_LOADER);			smm.Next(Cmd.END_NG); break; }
						if (G.seqLoadTransfer.IsReady == false )		{ G.AlmMng.ADD(ALM.ERROR_SEQ_ALLHOME_CANT_ALL_HOME_LOAD_TRANSFER);			smm.Next(Cmd.END_NG); break; }
						if (G.seqTrayClean.IsReady == false )			{ G.AlmMng.ADD(ALM.ERROR_SEQ_ALLHOME_CANT_ALL_HOME_TRAY_CLEAN);				smm.Next(Cmd.END_NG); break; }
						if (G.seqTrayUnloader.IsReady == false )		{ G.AlmMng.ADD(ALM.ERROR_SEQ_ALLHOME_CANT_ALL_HOME_TRAY_UNLOADER);    		smm.Next(Cmd.END_NG); break; }
						if (G.seqUnloadTransfer.IsReady == false )		{ G.AlmMng.ADD(ALM.ERROR_SEQ_ALLHOME_CANT_ALL_HOME_UNLOAD_TRANSFER);		smm.Next(Cmd.END_NG); break; }
						if (G.seqLoadInspPnP.IsReady == false )			{ G.AlmMng.ADD(ALM.ERROR_SEQ_ALLHOME_CANT_ALL_HOME_LOAD_INSP_PNP);			smm.Next(Cmd.END_NG); break; }
						if (G.seqUnloadInspPnP.IsReady == false )		{ G.AlmMng.ADD(ALM.ERROR_SEQ_ALLHOME_CANT_ALL_HOME_UNLOAD_INSP_PNP);		smm.Next(Cmd.END_NG); break; }
						if (G.seqInspPlate1.IsReady == false )			{ G.AlmMng.ADD(ALM.ERROR_SEQ_ALLHOME_CANT_ALL_HOME_INSP_PLATE1);			smm.Next(Cmd.END_NG); break; }
						if (G.seqInspPlate2.IsReady == false )			{ G.AlmMng.ADD(ALM.ERROR_SEQ_ALLHOME_CANT_ALL_HOME_INSP_PLATE2);			smm.Next(Cmd.END_NG); break; }
						if (G.seqUnloadBuffer.IsReady == false )		{ G.AlmMng.ADD(ALM.ERROR_SEQ_ALLHOME_CANT_ALL_HOME_UNLOAD_BUFFER);			smm.Next(Cmd.END_NG); break; }
						if (G.seqRejectBuffer.IsReady == false )		{ G.AlmMng.ADD(ALM.ERROR_SEQ_ALLHOME_CANT_ALL_HOME_REJECT_BUFFER);			smm.Next(Cmd.END_NG); break; }

						SM.SHM.SetBitData(BIT.AllHome, false);
						SM.SHM.SetBitData(BIT.AllHoming, true);
						
						smm.Next(Cmd.Home_10);
					}
					break;

				case Cmd.Home_10:
					{
						if (smm.Delayms(NDm.Read(eNDM.TimeoutAllHome)) == true)
						{
							if (IsRelease(Seq.TrayClean) == false)			G.AlmMng.ADD(ALM.ERROR_SEQ_ALLHOME_CANT_ALL_HOME_TRAY_CLEAN);    
							if (IsRelease(Seq.LoadTrayTable1) == false)		G.AlmMng.ADD(ALM.ERROR_SEQ_ALLHOME_CANT_ALL_HOME_LOAD_TRAY_TABLE1);
							if (IsRelease(Seq.LoadTrayTable2) == false)		G.AlmMng.ADD(ALM.ERROR_SEQ_ALLHOME_CANT_ALL_HOME_LOAD_TRAY_TABLE2);
							if (IsRelease(Seq.UnloadTrayTable1) == false)	G.AlmMng.ADD(ALM.ERROR_SEQ_ALLHOME_CANT_ALL_HOME_UNLOAD_TRAY_TABLE1);
							if (IsRelease(Seq.UnloadTrayTable2) == false)	G.AlmMng.ADD(ALM.ERROR_SEQ_ALLHOME_CANT_ALL_HOME_UNLOAD_TRAY_TABLE2);
							if (IsRelease(Seq.LoadBuffer1) == false)		G.AlmMng.ADD(ALM.ERROR_SEQ_ALLHOME_CANT_ALL_HOME_LOAD_BUFFER1);
							if (IsRelease(Seq.LoadBuffer2) == false)		G.AlmMng.ADD(ALM.ERROR_SEQ_ALLHOME_CANT_ALL_HOME_LOAD_BUFFER2);
							if (IsRelease(Seq.UnloadBuffer) == false)		G.AlmMng.ADD(ALM.ERROR_SEQ_ALLHOME_CANT_ALL_HOME_UNLOAD_BUFFER);
							if (IsRelease(Seq.InspPlate1) == false)			G.AlmMng.ADD(ALM.ERROR_SEQ_ALLHOME_CANT_ALL_HOME_INSP_PLATE1);
							if (IsRelease(Seq.InspPlate2) == false)			G.AlmMng.ADD(ALM.ERROR_SEQ_ALLHOME_CANT_ALL_HOME_INSP_PLATE2);
							
							smm.Next(Cmd.END_NG);
							break;
						}

						if (IsRelease(Seq.TrayClean) == false)				SetRelease(Seq.TrayClean);
						if (IsRelease(Seq.LoadTrayTable1) == false)			SetRelease(Seq.LoadTrayTable1);
						if (IsRelease(Seq.LoadTrayTable2) == false)			SetRelease(Seq.LoadTrayTable2);
						if (IsRelease(Seq.UnloadTrayTable1) == false)		SetRelease(Seq.UnloadTrayTable1);
						if (IsRelease(Seq.UnloadTrayTable2) == false)		SetRelease(Seq.UnloadTrayTable2);
						if (IsRelease(Seq.LoadBuffer1) == false)			SetRelease(Seq.LoadBuffer1);
						if (IsRelease(Seq.LoadBuffer2) == false)			SetRelease(Seq.LoadBuffer2);
						if (IsRelease(Seq.UnloadBuffer) == false)			SetRelease(Seq.UnloadBuffer);
						if (IsRelease(Seq.InspPlate1) == false)				SetRelease(Seq.InspPlate1);
						if (IsRelease(Seq.InspPlate2) == false)				SetRelease(Seq.InspPlate2);
						

						if (IsRelease(Seq.TrayClean) == false)				break;
						if (IsRelease(Seq.LoadTrayTable1) == false)			break;
						if (IsRelease(Seq.LoadTrayTable2) == false)			break;
						if (IsRelease(Seq.UnloadTrayTable1) == false)		break;
						if (IsRelease(Seq.UnloadTrayTable2) == false)		break;
						if (IsRelease(Seq.LoadBuffer1) == false)			break;
						if (IsRelease(Seq.LoadBuffer2) == false)			break;
						if (IsRelease(Seq.UnloadBuffer) == false)			break;
						if (IsRelease(Seq.InspPlate1) == false)				break;
						if (IsRelease(Seq.InspPlate2) == false)				break;

						smm.Next(Cmd.Home_20);
					}
					break;

				case Cmd.Home_20:
					{
						G.seqLoadTransfer.HomeStart(m_IsMtHome);
						G.seqUnloadTransfer.HomeStart(m_IsMtHome);
						G.seqUnloadInspPnP.HomeStart(m_IsMtHome);
						G.seqUnloadPnP.HomeStart(m_IsMtHome);
						G.seqLoadInspPnP.HomeStart(m_IsMtHome);
						G.seqLoadPnP.HomeStart(m_IsMtHome);
					   
						smm.Next(Cmd.Home_30);
					}
					break;

				case Cmd.Home_30:
					{
						if (smm.Delayms(NDm.Read(eNDM.TimeoutAllHome)) == true)
						{
							if (G.seqLoadTransfer.IsHomeReady() == false)		G.AlmMng.ADD(ALM.ERROR_SEQ_ALLHOME_CANT_ALL_HOME_LOAD_TRANSFER);
							if (G.seqUnloadTransfer.IsHomeReady() == false)		G.AlmMng.ADD(ALM.ERROR_SEQ_ALLHOME_CANT_ALL_HOME_UNLOAD_TRANSFER);
							if (G.seqLoadInspPnP.IsHomeReady() == false)		G.AlmMng.ADD(ALM.ERROR_SEQ_ALLHOME_CANT_ALL_HOME_LOAD_INSP_PNP);
							if (G.seqUnloadInspPnP.IsHomeReady() == false)		G.AlmMng.ADD(ALM.ERROR_SEQ_ALLHOME_CANT_ALL_HOME_UNLOAD_INSP_PNP);
							if (G.seqLoadPnP.IsHomeReady() == false)			G.AlmMng.ADD(ALM.ERROR_SEQ_ALLHOME_CANT_ALL_HOME_LOAD_PNP);
							if (G.seqUnloadPnP.IsHomeReady() == false)			G.AlmMng.ADD(ALM.ERROR_SEQ_ALLHOME_CANT_ALL_HOME_UNLOAD_PNP);

							smm.Next(Cmd.END_NG);
							break;
						}
						
						if (G.seqLoadTransfer.IsHomeReady() == false)		break;
						if (G.seqUnloadTransfer.IsHomeReady() == false)		break;
						if (G.seqLoadInspPnP.IsHomeReady() == false)		break;
						if (G.seqUnloadInspPnP.IsHomeReady() == false)		break;
						if (G.seqLoadPnP.IsHomeReady() == false)			break;
						if (G.seqUnloadPnP.IsHomeReady() == false)			break;
						

						if (SM.SHM.GetUsePart(UsePartName.LDTable1) != UsePartMode.Disable)			G.seqLoadTrayTable1.HomeStart(m_IsMtHome);
						if (SM.SHM.GetUsePart(UsePartName.LDTable2) != UsePartMode.Disable)			G.seqLoadTrayTable2.HomeStart(m_IsMtHome);
						if (SM.SHM.GetUsePart(UsePartName.ULDTable1) != UsePartMode.Disable)		G.seqUnloadTrayTable1.HomeStart(m_IsMtHome);
						if (SM.SHM.GetUsePart(UsePartName.ULDTable2) != UsePartMode.Disable)		G.seqUnloadTrayTable2.HomeStart(m_IsMtHome);
						if (SM.SHM.GetUsePart(UsePartName.LDBuf1) != UsePartMode.Disable)			G.seqLoadBuffer1.HomeStart(m_IsMtHome);
						if (SM.SHM.GetUsePart(UsePartName.LDBuf2) != UsePartMode.Disable)			G.seqLoadBuffer2.HomeStart(m_IsMtHome);
						if (SM.SHM.GetUsePart(UsePartName.Insp1) != UsePartMode.Disable)			G.seqInspPlate1.HomeStart(m_IsMtHome);
						if (SM.SHM.GetUsePart(UsePartName.Insp2) != UsePartMode.Disable)			G.seqInspPlate2.HomeStart(m_IsMtHome);

						G.seqUnloadBuffer.HomeStart(m_IsMtHome);
						G.seqTrayLoader.HomeStart(m_IsMtHome);
						G.seqTrayClean.HomeStart();
						G.seqTrayUnloader.HomeStart(m_IsMtHome);
						G.seqRejectBuffer.HomeStart(m_IsMtHome);
						
						smm.Next(Cmd.Home_40);
					}
					break;

				case Cmd.Home_40:
					{
						if (smm.Delayms(NDm.Read(eNDM.TimeoutAllHome)) == true)
						{
							if (SM.SHM.GetUsePart(UsePartName.LDTable1) != UsePartMode.Disable && G.seqLoadTrayTable1.IsHomeReady() == false)
								G.AlmMng.ADD(ALM.ERROR_SEQ_ALLHOME_CANT_ALL_HOME_LOAD_TRAY_TABLE1);
							if (SM.SHM.GetUsePart(UsePartName.LDTable2) != UsePartMode.Disable && G.seqLoadTrayTable2.IsHomeReady() == false)
								G.AlmMng.ADD(ALM.ERROR_SEQ_ALLHOME_CANT_ALL_HOME_LOAD_TRAY_TABLE2);
							if (SM.SHM.GetUsePart(UsePartName.ULDTable1) != UsePartMode.Disable && G.seqUnloadTrayTable1.IsHomeReady() == false)
								G.AlmMng.ADD(ALM.ERROR_SEQ_ALLHOME_CANT_ALL_HOME_UNLOAD_TRAY_TABLE1);
							if (SM.SHM.GetUsePart(UsePartName.ULDTable2) != UsePartMode.Disable && G.seqUnloadTrayTable2.IsHomeReady() == false)
								G.AlmMng.ADD(ALM.ERROR_SEQ_ALLHOME_CANT_ALL_HOME_UNLOAD_TRAY_TABLE2);
							if (SM.SHM.GetUsePart(UsePartName.LDBuf1) != UsePartMode.Disable && G.seqLoadBuffer1.IsHomeReady() == false)
								G.AlmMng.ADD(ALM.ERROR_SEQ_ALLHOME_CANT_ALL_HOME_LOAD_BUFFER1);
							if (SM.SHM.GetUsePart(UsePartName.LDBuf2) != UsePartMode.Disable && G.seqLoadBuffer2.IsHomeReady() == false)
								G.AlmMng.ADD(ALM.ERROR_SEQ_ALLHOME_CANT_ALL_HOME_LOAD_BUFFER2);
							if (SM.SHM.GetUsePart(UsePartName.Insp1) != UsePartMode.Disable && G.seqInspPlate1.IsHomeReady() == false)
								G.AlmMng.ADD(ALM.ERROR_SEQ_ALLHOME_CANT_ALL_HOME_INSP_PLATE1);
							if (SM.SHM.GetUsePart(UsePartName.Insp2) != UsePartMode.Disable && G.seqInspPlate2.IsHomeReady() == false)
								G.AlmMng.ADD(ALM.ERROR_SEQ_ALLHOME_CANT_ALL_HOME_INSP_PLATE2);

							if (G.seqUnloadBuffer.IsHomeReady() == false)				G.AlmMng.ADD(ALM.ERROR_SEQ_ALLHOME_CANT_ALL_HOME_UNLOAD_BUFFER);
							if (G.seqTrayClean.IsHomeReady() == false)					G.AlmMng.ADD(ALM.ERROR_SEQ_ALLHOME_CANT_ALL_HOME_TRAY_CLEAN);    
							if (G.seqTrayLoader.IsHomeReady() == false)					G.AlmMng.ADD(ALM.ERROR_SEQ_ALLHOME_CANT_ALL_HOME_TRAY_LOADER);    
							if (G.seqTrayUnloader.IsHomeReady() == false)				G.AlmMng.ADD(ALM.ERROR_SEQ_ALLHOME_CANT_ALL_HOME_TRAY_UNLOADER);    
							if (G.seqRejectBuffer.IsHomeReady() == false)				G.AlmMng.ADD(ALM.ERROR_SEQ_ALLHOME_CANT_ALL_HOME_REJECT_BUFFER);    
							
							smm.Next(Cmd.END_NG);
							break;
						}

						if (SM.SHM.GetUsePart(UsePartName.LDTable1) != UsePartMode.Disable  &&  G.seqLoadTrayTable1.IsHomeReady() == false)			break;
						if (SM.SHM.GetUsePart(UsePartName.LDTable2) != UsePartMode.Disable  &&  G.seqLoadTrayTable2.IsHomeReady() == false)			break;
						if (SM.SHM.GetUsePart(UsePartName.ULDTable1) != UsePartMode.Disable  &&  G.seqUnloadTrayTable1.IsHomeReady() == false)		break;
						if (SM.SHM.GetUsePart(UsePartName.ULDTable2) != UsePartMode.Disable  &&  G.seqUnloadTrayTable2.IsHomeReady() == false)		break;
						if (SM.SHM.GetUsePart(UsePartName.LDBuf1) != UsePartMode.Disable  &&  G.seqLoadBuffer1.IsHomeReady() == false)				break;
						if (SM.SHM.GetUsePart(UsePartName.LDBuf2) != UsePartMode.Disable  &&  G.seqLoadBuffer2.IsHomeReady() == false)				break;
						if (SM.SHM.GetUsePart(UsePartName.Insp1) != UsePartMode.Disable  &&  G.seqInspPlate1.IsHomeReady() == false)				break;
						if (SM.SHM.GetUsePart(UsePartName.Insp2) != UsePartMode.Disable  &&  G.seqInspPlate2.IsHomeReady() == false)				break;

						if (G.seqUnloadBuffer.IsHomeReady() == false)			break;
						if (G.seqTrayLoader.IsHomeReady() == false)				break;
						if (G.seqTrayClean.IsHomeReady() == false)				break;
						if (G.seqTrayUnloader.IsHomeReady() == false)			break;
						if (G.seqRejectBuffer.IsHomeReady() == false)			break;


						if (IsMachineEmpty() == false)
						{
							smm.Next(Cmd.END_OK);
							break;
						}

						smm.Next(Cmd.Home_50);
					}
					break;

				case Cmd.Home_50:
					{
						//실린더 구동 오류 조치를 위해 추가
						//동시에 구동시 소리가 커서 개별로 구동
					
						if (IsUDAct(Seq.LoadPnP, SenPos.PICKER_DOWN) == false)
						{
							SetUDAct(Seq.LoadPnP, SenPos.PICKER_DOWN);
							break;
						}

						if (IsUDAct(Seq.UnloadPnP, SenPos.PICKER_DOWN) == false)
						{
							SetUDAct(Seq.UnloadPnP, SenPos.PICKER_DOWN);
							break;
						}

						if (IsUDAct(Seq.LoadInspPnP, SenPos.PICKER_DOWN) == false)
						{
							SetUDAct(Seq.LoadInspPnP, SenPos.PICKER_DOWN);
							break;
						}

						if (IsUDAct(Seq.UnloadInspPnP, SenPos.PICKER_DOWN) == false)
						{
							SetUDAct(Seq.UnloadInspPnP, SenPos.PICKER_DOWN);
							break;
						}
						
						smm.Next(Cmd.Home_60);
					}
					break;

				case Cmd.Home_60:
					{
						SetUDAct(Seq.LoadPnP,		SenPos.PICKER_UP);
						SetUDAct(Seq.UnloadPnP,		SenPos.PICKER_UP);
						SetUDAct(Seq.LoadInspPnP,	SenPos.PICKER_UP);
						SetUDAct(Seq.UnloadInspPnP,	SenPos.PICKER_UP);
						
						if (IsUDAct(Seq.LoadPnP,		SenPos.PICKER_UP) == false)			break;
						if (IsUDAct(Seq.UnloadPnP,		SenPos.PICKER_UP) == false)			break;
						if (IsUDAct(Seq.LoadInspPnP,	SenPos.PICKER_UP) == false)			break;
						if (IsUDAct(Seq.UnloadInspPnP,	SenPos.PICKER_UP) == false)			break;
						
						smm.Next(Cmd.END_OK);
					}
					break;
				case Cmd.END_OK:
					{
						if (m_nTopInspInitReuslt != 1)			break;
						if (m_nBtmInspInitReuslt != 1)			break;
						if (m_nFTVInitReuslt != 1)			break;

						SM.SHM.SetBitData(BIT.AllHome,		true);
						SM.SHM.SetBitData(BIT.AllHoming,	false);
						//Check : MES
					 //   G.commUbiGEM.InitGEM();
					 //   G.seqUBIGEMComm.Reset();

						if (IsMachineEmpty() == true)
						{
							NDm.Write(eNDM.AutoRunMode, (int)AutoRunMode.Normal);
							NDm.Write(eNDM.LotEndMode, (int)LotEndMode.Normal);
							NDm.Write(eNDM.RejectRunMode, (int)RejectRunMode.Normal);

							SM.SHM.SetBitData(BIT.LotLoadStart, false);
							
							SM.FMR.FlowClearAll();
							NDm.Write(eNDM.FillDoneRemainCount, 0);
							SDm.Write(eSDM.CurTempKeyValue , "NONE");
							SM.SHM.m_dicInspYield.Clear();
                            SM.SHM.ClearLotYield();
							SM.SHM.ClearRunLotInfo();
							SM.SHM.AllClearFiduErrorCount();
							G.seqUBIGEMComm.Stop();

							if (SM.FMT.IsFlow(FlowTray.Reject) == true  && SM.FMT[FlowTray.Reject].GetExistCount() == 0 )
							{
								var t_info = SM.FMT[FlowTray.Reject].GetInfo();
								t_info.KeyValue = "";
								SM.FMT[FlowTray.Reject].SetInfo(t_info);
							}
						}

						int nBeforeRecipeIdx	= NDm.Read(eNDM.OldRecipeIdx);
						int nNowRecipeIdx		= UI.SubpageRecipe.iNowRecipeNum;
						if (nBeforeRecipeIdx != nNowRecipeIdx)
						{
							NDm.Write(eNDM.OldRecipeIdx, nNowRecipeIdx);
						}
						NDm.Write(eNDM.RecipeChange, -1);

						smm.Next(Cmd.START);
					}
					break;

				case Cmd.END_NG:
					{
						SeqInit();
						//Chekc : MES
					//    G.seqUBIGEMComm.ResetStep();

						SM.SHM.SetBitData(BIT.AllHome,		false);
						SM.SHM.SetBitData(BIT.AllHoming,	false);

						smm.Next(Cmd.START);
					}
					break;
			}
		}
		
		public void TopInspInit(SMM<Cmd> smm)
		{
			switch (smm.CurStep)
			{
				case Cmd.START:
					{
						if (smm.eInput == InState.run)
						{
							smm.eInput = InState.none;
							smm.Next(Cmd.INSP_INIT_10);

							m_nTopInspInitReuslt = 0;
						}
					}
					break;

				case Cmd.INSP_INIT_10:
					{
						if (SM.SHM.GetUseSkipData(USESKIP.TopInspection) == false)
						{
							smm.Next(Cmd.END_OK);
							break;
						}

						G.commInspTop.SendRunStateRequest();

						smm.Next(Cmd.INSP_INIT_20);
					}
					break;
				case Cmd.INSP_INIT_20:      // 검사 PC Run 상태 확인 Top
					{
						if (SM.SHM.GetUseSkipData(USESKIP.TopInspection) == false)
						{
							smm.Next(Cmd.INSP_INIT_40);
							break;
						}

						CommInsp.RunState state = CommInsp.RunState.Idle;
						int nret = G.commInspTop.RcvRunStateReport(ref state);
						if (nret < 0)           //Data에 문제가 있는 경우
						{
							smm.Next(Cmd.END_NG);
						}
						else if (nret > 0)
						{
							if (state == CommInsp.RunState.Init || state == CommInsp.RunState.Run) //이미 런중이거나, 홈 진행 중인 경우
							{
								smm.Next(Cmd.INSP_INIT_60);
							}
							else
							{
								G.commInspTop.SendHomeStart();
								smm.Next(Cmd.INSP_INIT_30);
							}
						}
						else if (smm.Delayms(NDm.Read(eNDM.InspCommTimeOut)) == true)
						{
							G.AlmMng.ADD(ALM.ERROR_SEQ_INSP_TOP_PC_COMM_TIMEOUT);
							smm.Next(Cmd.END_NG);
						}
					}
					break;

				case Cmd.INSP_INIT_30:
					{
						bool bOK = false;
						int nret = G.commInspTop.RcvHomeStartConfirm(ref bOK);
						if (nret < 0)
						{
							smm.Next(Cmd.END_NG);
						}
						else if (nret > 0)
						{
							if (bOK == true)
							{
								smm.Next(Cmd.INSP_INIT_40);
							}
							else
							{
								G.AlmMng.ADD(ALM.ERROR_SEQ_INSP_TOP_PC_CONFIRM_NG);
								smm.Next(Cmd.END_NG);
							}
						}
						else if (smm.Delayms(NDm.Read(eNDM.InspCommTimeOut)) == true)
						{
							G.AlmMng.ADD(ALM.ERROR_SEQ_INSP_TOP_PC_COMM_TIMEOUT);
							smm.Next(Cmd.END_NG);
						}
					}
					break;
				case Cmd.INSP_INIT_40:
					{
						if (smm.Delayms(500) == false) break;

						if (SM.SHM.GetUseSkipData(USESKIP.TopInspection) == true)
						{
							G.commInspTop.SendRunStateRequest();
						}

						smm.Next(Cmd.INSP_INIT_50);
					}
					break;
				case Cmd.INSP_INIT_50:      // 검사 PC Run 상태 확인 Top
					{
						if (SM.SHM.GetUseSkipData(USESKIP.TopInspection) == false)
						{
							smm.Next(Cmd.END_OK);
							break;
						}

						CommInsp.RunState state = CommInsp.RunState.Idle;
						int nret = G.commInspTop.RcvRunStateReport(ref state);
						if (nret < 0)
						{
							smm.Next(Cmd.END_NG);
						}
						else if (nret > 0)
						{
							if (state != CommInsp.RunState.Init)
							{
								smm.Next(Cmd.END_OK);
							}
							else if (smm.Delayms(NDm.Read(eNDM.InspHomeTimeOut)) == true)
							{
								G.AlmMng.ADD(ALM.ERROR_SEQ_INSP_TOP_PC_COMM_TIMEOUT);
								smm.Next(Cmd.END_NG);
							}
							else
							{
								smm.Next(Cmd.INSP_INIT_40);
							}
						}
						else if (smm.Delayms(NDm.Read(eNDM.InspCommTimeOut)) == true)
						{
							G.AlmMng.ADD(ALM.ERROR_SEQ_INSP_TOP_PC_RUNSTATE_COMM_TIMEOUT);
							smm.Next(Cmd.END_NG);
						}
					}
					break;
				case Cmd.END_OK:
					{
						m_nTopInspInitReuslt = 1;
						smm.Next(Cmd.START);
					}
					break;

				case Cmd.END_NG:
					{
						m_nTopInspInitReuslt = -1;
						smm.Next(Cmd.START);
					}
					break;
			}
		}

		
		public void BtmInspInit(SMM<Cmd> smm)
		{
			switch (smm.CurStep)
			{
				case Cmd.START:
					{
						if (smm.eInput == InState.run)
						{
							smm.eInput = InState.none;
							smm.Next(Cmd.INSP_INIT_10);

							m_nBtmInspInitReuslt = 0;
						}
					}
					break;

				case Cmd.INSP_INIT_10:
					{
						if (SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == false)
						{
							smm.Next(Cmd.END_OK);
							break;
						}

						G.commInspBtm.SendRunStateRequest();

						smm.Next(Cmd.INSP_INIT_20);
					}
					break;
				case Cmd.INSP_INIT_20:      // 검사 PC Run 상태 확인 Top
					{
						if (SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == false)
						{
							smm.Next(Cmd.INSP_INIT_40);
							break;
						}

						CommInsp.RunState state = CommInsp.RunState.Idle;
						int nret = G.commInspBtm.RcvRunStateReport(ref state);
						if (nret < 0)           //Data에 문제가 있는 경우
						{
							smm.Next(Cmd.END_NG);
						}
						else if (nret > 0)
						{
							if (state == CommInsp.RunState.Init || state == CommInsp.RunState.Run) //이미 런중이거나, 홈 진행 중인 경우
							{
								smm.Next(Cmd.INSP_INIT_40);
							}
							else
							{
								G.commInspBtm.SendHomeStart();
								smm.Next(Cmd.INSP_INIT_30);
							}
						}
						else if (smm.Delayms(NDm.Read(eNDM.InspCommTimeOut)) == true)
						{
							G.AlmMng.ADD(ALM.ERROR_SEQ_INSP_BTM_PC_COMM_TIMEOUT);
							smm.Next(Cmd.END_NG);
						}
					}
					break;

				case Cmd.INSP_INIT_30:
					{
						bool bOK = false;
						int nret = G.commInspBtm.RcvHomeStartConfirm(ref bOK);
						if (nret < 0)
						{
							smm.Next(Cmd.END_NG);
						}
						else if (nret > 0)
						{
							if (bOK == true)
							{
								smm.Next(Cmd.INSP_INIT_40);
							}
							else
							{
								G.AlmMng.ADD(ALM.ERROR_SEQ_INSP_BTM_PC_CONFIRM_NG);
								smm.Next(Cmd.END_NG);
							}
						}
						else if (smm.Delayms(NDm.Read(eNDM.InspCommTimeOut)) == true)
						{
							G.AlmMng.ADD(ALM.ERROR_SEQ_INSP_BTM_PC_COMM_TIMEOUT);
							smm.Next(Cmd.END_NG);
						}
					}
					break;
				case Cmd.INSP_INIT_40:
					{
						if (smm.Delayms(500) == false) break;

						if (SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == true)
						{
							G.commInspBtm.SendRunStateRequest();
						}

						smm.Next(Cmd.INSP_INIT_50);
					}
					break;
				case Cmd.INSP_INIT_50:      // 검사 PC Run 상태 확인 Top
					{
						if (SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == false)
						{
							smm.Next(Cmd.END_OK);
							break;
						}

						CommInsp.RunState state = CommInsp.RunState.Idle;
						int nret = G.commInspBtm.RcvRunStateReport(ref state);
						if (nret < 0)
						{
							smm.Next(Cmd.END_NG);
						}
						else if (nret > 0)
						{
							if (state != CommInsp.RunState.Init)
							{
								smm.Next(Cmd.END_OK);
							}
							else if (smm.Delayms(NDm.Read(eNDM.InspHomeTimeOut)) == true)
							{
								G.AlmMng.ADD(ALM.ERROR_SEQ_INSP_BTM_PC_COMM_TIMEOUT);
								smm.Next(Cmd.END_NG);
							}
							else
							{
								smm.Next(Cmd.INSP_INIT_40);
							}
						}
						else if (smm.Delayms(NDm.Read(eNDM.InspCommTimeOut)) == true)
						{
							G.AlmMng.ADD(ALM.ERROR_SEQ_INSP_BTM_PC_RUNSTATE_COMM_TIMEOUT);
							smm.Next(Cmd.END_NG);
						}
					}
					break;
				case Cmd.END_OK:
					{
						m_nBtmInspInitReuslt = 1;
						smm.Next(Cmd.START);
					}
					break;

				case Cmd.END_NG:
					{
						m_nBtmInspInitReuslt = -1;
						smm.Next(Cmd.START);
					}
					break;
			}
		}

		private void FTVInit(SMM<Cmd> smm)
		{
			switch (smm.CurStep)
			{
				case Cmd.START:
					{
						if (smm.eInput == InState.run)
						{
							smm.eInput = InState.none;
							smm.Next(Cmd.FTV_INIT_10);

							m_nFTVInitReuslt = 0;
							m_nFTVTryCount = 0;
						}
					}
					break;

				case Cmd.FTV_INIT_10:
					{
						if (SM.SHM.GetUseSkipData(USESKIP.SmartVisionLoader) == false)
						{
							smm.Next(Cmd.FTV_INIT_60);
							break;
						}

						G.commFTVLoader.Send(CommFTV.IV4Command.ReadOperatingStatus);

						smm.Next(Cmd.FTV_INIT_20);
					}
					break;

				case Cmd.FTV_INIT_20:
					{
						string readRcvData = "";
						int ret = G.commFTVLoader.RcvRead(CommFTV.IV4Command.ReadOperatingStatus, ref readRcvData);

						if (ret != 1)
						{
							if (smm.Delayms(5000) == false) break;

							G.AlmMng.ADD(ALM.ERROR_MAIN_SEQ_FTV_INSP_FTV_STATE_CHECK_TIME_OUT);// 운전 준비상태 타임 아웃
							smm.Next(Cmd.END_NG);
							break;
						}

						if (readRcvData == "0") // 0 - 설정중 대기한다.
						{
							G.AlmMng.ADD(ALM.ERROR_MAIN_SEQ_FTV_INSP_FTV_STATE_CHECK);// 운전 준비상태 확인.
							smm.Next(Cmd.END_NG);
							break;
						}

						smm.Next(Cmd.FTV_INIT_30);
					}
					break;

				case Cmd.FTV_INIT_30:
					{
						G.commFTVLoader.Send(CommFTV.IV4Command.GetProgramNumber);

						smm.Next(Cmd.FTV_INIT_40);
					}
					break;

				case Cmd.FTV_INIT_40:
					{
						string readRcvNumber = "";
						int ret = G.commFTVLoader.RcvGetProgramNumber(ref readRcvNumber);
						string oriRcvNumber = (SM.SHM.GetRecipeDataToInt(DEVICE.FTVJob) / 1000).ToString("000");

						if (ret != 1)
						{
							if (smm.Delayms(3000) == false) break;
							
							G.AlmMng.ADD(ALM.ERROR_MAIN_SEQ_FTV_INSP_CONNECT_CHECK);
							smm.Next(Cmd.END_NG);
							break;
						}

						if (readRcvNumber != oriRcvNumber) // FTV 레시피 체크 확인 ##
						{
							G.commFTVLoader.Send(CommFTV.IV4Command.SetProgramNumber, oriRcvNumber);
							smm.Next(Cmd.FTV_INIT_50);
							break;
						}
						
						smm.Next(Cmd.FTV_INIT_60);
					}
					break;

				case Cmd.FTV_INIT_50:
					{
						if (smm.Delayms(5000) == false) break;

						m_nFTVTryCount ++ ;
						if (m_nFTVTryCount >= 3 )
						{
							G.AlmMng.ADD(ALM.ERROR_MAIN_SEQ_FTV_INSP_RECIPE_CHECK);// FTV Insp 레시피 체크 알람
							
							smm.Next(Cmd.END_NG);
							break;
						}

						smm.Next(Cmd.FTV_INIT_30);
					}
					break;
					
				case Cmd.FTV_INIT_60:
					{
						if (SM.SHM.GetUseSkipData(USESKIP.SmartVisionTable) == false)
						{
							smm.Next(Cmd.END_OK);
							break;
						}

						G.commFTVTable.Send(CommFTV.IV4Command.ReadOperatingStatus);

						smm.Next(Cmd.FTV_INIT_70);
					}
					break;

				case Cmd.FTV_INIT_70:
					{
						string readRcvData = "";
						int ret = G.commFTVTable.RcvRead(CommFTV.IV4Command.ReadOperatingStatus, ref readRcvData);

						if (ret != 1)
						{
							if (smm.Delayms(5000) == false) break;

							G.AlmMng.ADD(ALM.ERROR_MAIN_SEQ_FTV_INSP_FTV_STATE_CHECK_TIME_OUT);// 운전 준비상태 타임 아웃
							smm.Next(Cmd.END_NG);
							break;
						}

						if (readRcvData == "0") // 0 - 설정중 대기한다.
						{
							G.AlmMng.ADD(ALM.ERROR_MAIN_SEQ_FTV_INSP_FTV_STATE_CHECK);// 운전 준비상태 확인.
							smm.Next(Cmd.END_NG);
							break;
						}

						smm.Next(Cmd.FTV_INIT_80);
					}
					break;

				case Cmd.FTV_INIT_80:
					{
						G.commFTVTable.Send(CommFTV.IV4Command.GetProgramNumber);

						smm.Next(Cmd.FTV_INIT_90);
					}
					break;

				case Cmd.FTV_INIT_90:
					{
						string readRcvNumber = "";
						int ret = G.commFTVTable.RcvGetProgramNumber(ref readRcvNumber);
						string oriRcvNumber = (SM.SHM.GetRecipeDataToInt(DEVICE.FTVJob) / 1000).ToString("000");

						if (ret != 1)
						{
							if (smm.Delayms(3000) == false) break;
							
							G.AlmMng.ADD(ALM.ERROR_MAIN_SEQ_FTV_INSP_CONNECT_CHECK);
							smm.Next(Cmd.END_NG);
							break;
						}

						if (readRcvNumber != oriRcvNumber) // FTV 레시피 체크 확인 ##
						{
							G.commFTVTable.Send(CommFTV.IV4Command.SetProgramNumber, oriRcvNumber);
							smm.Next(Cmd.FTV_INIT_100);
							break;
						}
						
						smm.Next(Cmd.END_OK);
					}
					break;

				case Cmd.FTV_INIT_100:
					{
						if (smm.Delayms(5000) == false) break;

						m_nFTVTryCount ++ ;
						if (m_nFTVTryCount >= 3 )
						{
							G.AlmMng.ADD(ALM.ERROR_MAIN_SEQ_FTV_INSP_RECIPE_CHECK);// FTV Insp 레시피 체크 알람
							
							smm.Next(Cmd.END_NG);
							break;
						}

						smm.Next(Cmd.FTV_INIT_80);
					}
					break;
				case Cmd.END_NG:
					{
						m_nFTVInitReuslt = 0;

						smm.Next(Cmd.START);
					}
					break;

				case Cmd.END_OK:
					{
						m_nFTVInitReuslt = 1;

						smm.Next(Cmd.START);
					}
					break;
			}
		}

		private void	SeqInit()
		{
			for (int i = 0; i < (int)MT.TOTAL; i++)
			{
				G.cKernel.Motor((MT)i).Stop();
			}

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
		}
	}
}
