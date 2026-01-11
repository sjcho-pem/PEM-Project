using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace H_MarsSSD
{
	public class SeqJobTeaching : SeqBase<SeqJobTeaching.Act, SeqJobTeaching.Axis, SeqJobTeaching.Cmd>
	{
		#region [ Enum ]
		public enum Act
		{
			Max,
		}

		public enum Axis
		{
			Max,
		}

		public enum Cmd
		{
			START = 10,
			END = 99,
			
			TEACHINGLOAD1, 
			TEACHINGLOAD1_1,
			TEACHINGLOAD2, 
			TEACHINGLOAD3, 
			TEACHINGLOAD4, 
			TEACHINGLOAD5, 
			TEACHINGLOAD6,
			TEACHINGLOAD7,
			TEACHINGLOAD8,
			
			TEACHINGUNLOAD1, 
			TEACHINGUNLOAD2, 
			TEACHINGUNLOAD3, 
			TEACHINGUNLOAD4, 
			TEACHINGUNLOAD5, 
			TEACHINGUNLOAD6,
			TEACHINGUNLOAD7,
			TEACHINGUNLOAD8,
		}
		#endregion

		#region [Variables]
		private	SMM<Cmd>		m_smmLoad		= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.JOBTEACHING_LOAD));
		private	SMM<Cmd>		m_smmUnload		= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.JOBTEACHING_UNLOAD));
		
		private	int				m_nPlateNo		= 0;
		private	SeqInspPlate	m_seqInspPlate	= null;
		private	SeqLoadBuffer	m_seqLdBuf		= G.seqLoadBuffer1;
		#endregion

		#region [Function]
		public	bool	TeachingLoadStart(int nPlateNo)		{ m_nPlateNo = nPlateNo;	return SetSeqRun(ref m_smmLoad); }
		public	bool	TeachingUnloadStart(int nPlateNo)	{ m_nPlateNo = nPlateNo;	return SetSeqRun(ref m_smmUnload); }

		private	bool	SetSeqRun(ref SMM<Cmd> smm)
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
		#endregion

		public	void	Init()
		{
			try
			{
				lstSMM.Add(m_smmLoad);
				lstSMM.Add(m_smmUnload);

				#region - Motor
				arrMt = new EnumArray<Axis, CMotor>(Axis.Max);
				#endregion

				#region - Actuator
				arrAct = new EnumArray<Act, Actuator>(Act.Max);
				
				#endregion
				base.Initialize();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
		
		public	void	Run()
		{
			try
			{
				if (SM.SHM.IsBitData(BIT.Error) == true || SM.SHM.IsBitData(BIT.Pause) == true)
				{
					return;
				}

				LOAD(m_smmLoad);
				UNLOAD(m_smmUnload);
				SM.SHM.SetSeqStatus(lstSMM);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
		
		private	void	LOAD(SMM<Cmd> smm)
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

								smm.Next(Cmd.TEACHINGLOAD1);
							}
						}
						break;
                    case Cmd.TEACHINGLOAD1:
                        {
                            //메모리 생성 > load, unload 모두 생성
                            SM.SHM.SetBitData(BIT.AllHome, false);

                            if (SM.FMT.IsFlow(FlowTray.LDTable1) == false)
							{
								SM.FMT.Next(FlowTray.Empty, FlowTray.LDTable1);
							}
							
							int ncnt_col	= SM.SHM.GetRecipeDataToInt(DEVICE.TrayColCnt) / 1000;
							int ncnt_row	= SM.SHM.GetRecipeDataToInt(DEVICE.TrayRowCnt) / 1000;

							var	info_tray	= SM.FMT[FlowTray.LDTable1].GetInfo();


							for (int i = 0; i < SM.FMP[FlowPart.LDPnP].GetMaxProdCount()+1; i++)
							{
								int npocket_no	= i;

								if (i == SM.FMP[FlowPart.LDPnP].GetMaxProdCount())
								{
									npocket_no = (ncnt_col * ncnt_row) - 1;
								}

								info_tray.ProdInfo[npocket_no].Exist			= true;
								info_tray.ProdInfo[npocket_no].TrayRow			= npocket_no / ncnt_col;
								info_tray.ProdInfo[npocket_no].TrayCol			= npocket_no % ncnt_col;
								info_tray.ProdInfo[npocket_no].ResultTop		= (int)ProdStatus.Unknown;
								info_tray.ProdInfo[npocket_no].ResultBtm		= (int)ProdStatus.Unknown;
								info_tray.ProdInfo[npocket_no].TrayID			= "TeachingTray";
								info_tray.ProdInfo[npocket_no].ProdID			= "Teaching" + npocket_no.ToString();
								info_tray.ProdInfo[npocket_no].KeyValue			= "TeachingKeyValue";
								info_tray.ProdInfo[npocket_no].InspTimeTop	= "";
								info_tray.ProdInfo[npocket_no].InspTimeBtm	= "";
								info_tray.ProdInfo[npocket_no].SerialNumber		= "";
							}


							if (G.seqLoadTrayTable1.IsReady == false)			break;

							G.seqLoadTrayTable1.GripStart();
						
							if (IsReset(smm) == true)			smm.Next(Cmd.END);

							if		(SM.SHM.GetUsePart(UsePartName.LDBuf1) == UsePartMode.Enable)			m_seqLdBuf	= G.seqLoadBuffer1;
							else if (SM.SHM.GetUsePart(UsePartName.LDBuf1) != UsePartMode.Enable  &&
									 SM.SHM.GetUsePart(UsePartName.LDBuf2) == UsePartMode.Enable)			m_seqLdBuf	= G.seqLoadBuffer2;
							else if (SM.SHM.GetUsePart(UsePartName.LDBuf1) != UsePartMode.Enable  &&
									 SM.SHM.GetUsePart(UsePartName.LDBuf2) != UsePartMode.Enable)
							{
								smm.Next(Cmd.END);
								break;
							}

							smm.Next(Cmd.TEACHINGLOAD1_1);
						}
						break;
						

					case Cmd.TEACHINGLOAD1_1:
						{
							if (G.seqLoadTrayTable1.IsReady == false)			break;
							if (G.seqLoadTrayTable2.LoadTable1UpDnSafety() == false)
							{
								if (SM.FMT.IsFlow(FlowTray.LDTable1) == true)
								{
									SM.FMT.Clear(FlowTray.LDTable1);
								}

								AddAlarm(ALM.ERROR_SEQ_LOAD_TABLE1_UP_SAFETY);
								smm.Next(Cmd.END);
								break;
							}
							G.seqLoadTrayTable1.UpStart(SeqLoadTrayTable1.UDMode.Load);

							if (IsReset(smm) == true)			smm.Next(Cmd.END);
							
							smm.Next(Cmd.TEACHINGLOAD2);
						}
						break;

					case Cmd.TEACHINGLOAD2:
						{    
							if (G.seqLoadTrayTable1.IsReady == false)			break;
							if (G.seqLoadPnP.IsReady == false)					break;
							if (G.seqLoadTrayTable1.IsPnPEnable() == false)	break;
							if		(m_seqLdBuf.IsReady == false)				break;
							else if (m_seqLdBuf.IsReady == false)				break;

							G.seqLoadPnP.PnPStart();
							m_seqLdBuf.LoadWaitStart();

							if (IsReset(smm) == true)			smm.Next(Cmd.END);
							
							smm.Next(Cmd.TEACHINGLOAD3);
						}
						break;

					case Cmd.TEACHINGLOAD3:
						{
							if (G.seqLoadPnP.IsReady == false)				break;
							if (m_seqLdBuf.IsReady == false)			break;
							
							m_seqLdBuf.UnloadWaitStart();

							if (m_nPlateNo == 0)			m_seqInspPlate	= G.seqInspPlate1;
							else							m_seqInspPlate	= G.seqInspPlate2;
							
							if (IsReset(smm) == true)			smm.Next(Cmd.END);

							smm.Next(Cmd.TEACHINGLOAD4);
						}
						break;

					case Cmd.TEACHINGLOAD4:
						{
							if (G.seqLoadInspPnP.IsReady == false)			break;
							if (m_seqInspPlate.IsReady == false)			break;
							
							G.seqLoadInspPnP.PickStart();
							m_seqInspPlate.LoadWaitStart();
							
							if (IsReset(smm) == true)			smm.Next(Cmd.END);

							smm.Next(Cmd.TEACHINGLOAD5);
						}
						break;

					case Cmd.TEACHINGLOAD5:
						{
							if (G.seqLoadInspPnP.IsReady == false)			break;
							
							G.seqLoadInspPnP.PlaceStart();
							
							if (IsReset(smm) == true)			smm.Next(Cmd.END);

							smm.Next(Cmd.TEACHINGLOAD6);
						}
						break;
						
					case Cmd.TEACHINGLOAD6:
						{
							if (G.seqLoadInspPnP.IsReady == false)				break;
							if (m_seqInspPlate.IsReady == false)				break;
							if (G.seqLoadTrayTable1.IsReady == false)			break;

							m_seqInspPlate.TeachWaitStart();
							if (SM.FMT.IsFlow(FlowTray.LDTable1) == true)
							{
								SM.FMT.Clear(FlowTray.LDTable1);
							}
							G.seqLoadTrayTable1.ReleaseStart();
							
							if (IsReset(smm) == true)			smm.Next(Cmd.END);

							smm.Next(Cmd.TEACHINGLOAD7);
						}
						break;						

					case Cmd.TEACHINGLOAD7:
						{
							G.seqLoadInspPnP.ReadyPosStart();

							smm.Next(Cmd.TEACHINGLOAD8);
						}
						break;

					case Cmd.TEACHINGLOAD8:
						{
							if (G.seqLoadInspPnP.IsReady == false)			break;

							smm.Next(Cmd.END);
						}
						break;

					case Cmd.END:
						{
							if (m_seqInspPlate.IsReady == false)			break;
							
							SM.FMT.Clear(FlowTray.LDTable1);

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

		private	void	UNLOAD(SMM<Cmd> smm)
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

								smm.Next(Cmd.TEACHINGUNLOAD1);
							}
						}
						break;

					case Cmd.TEACHINGUNLOAD1:
						{
							if (G.DryRunIsBIn(DI.ULDTrayTable1ExistSen, true) == false)
							{
								AddAlarm(ALM.ERROR_SEQ_UNLOAD_TRAY_TABLE1_TRAY_NONE);
								smm.Next( Cmd.END);
							}

							SM.FMT.Next(FlowTray.Empty, FlowTray.ULDTable1);
							SM.FMT[FlowTray.ULDTable1].ProcStatus	= ProcStatus.Unknown;
							NDm.Write(eNDM.FillDoneRemainCount, 0);

							if (G.seqUnloadTrayTable1.IsReady == false) break;

							G.seqUnloadTrayTable1.GripStart();

							smm.Next(Cmd.TEACHINGUNLOAD2);
						}
						break;

					case Cmd.TEACHINGUNLOAD2 :
						{
							if (G.seqUnloadTrayTable1.IsReady == true)
							{
								G.seqUnloadTrayTable1.UpStart(SeqUnloadTrayTable.UDMode.Load);
							}

							smm.Next( Cmd.TEACHINGUNLOAD3);
						}
						break;

					case Cmd.TEACHINGUNLOAD3 :
						{						
							if (G.seqInspPlate1.IsReady ==false)			break;
							if (G.seqUnloadInspPnP.IsReady ==false)			break;

							FlowPart curFlow = FlowPart.Empty;
							if (m_nPlateNo == 0)	
							{
								curFlow = FlowPart.Insp1;
								m_seqInspPlate = G.seqInspPlate1;
							}
							else				
							{
								curFlow = FlowPart.Insp2;
								m_seqInspPlate = G.seqInspPlate2;
							}

							var info	= SM.FMP[curFlow].GetInfo();
							for (int i = 0; i < SM.FMP[curFlow].GetMaxProdCount(); i++)
							{
								info.ProdInfo[i].ResultTop	= (int)ProdStatus.OK;
								info.ProdInfo[i].ResultBtm	= (int)ProdStatus.OK;
							}
							SM.FMP[curFlow].SetInfo(info);

							m_seqInspPlate.UnloadWaitStart();
							G.seqUnloadInspPnP.PickStart();
						 
							if (IsReset(smm) == true)			smm.Next(Cmd.END);

							smm.Next(Cmd.TEACHINGUNLOAD4);
						}
						break;

					case Cmd.TEACHINGUNLOAD4:
						{
							if (m_seqInspPlate.IsReady ==false)					break;
							if (G.seqUnloadInspPnP.IsReady ==false)				break;
							if (G.seqUnloadBuffer.IsReady ==false)				break;
							if (G.seqUnloadTrayTable1.IsWaitEnable() == false)	break;

							G.seqUnloadInspPnP.PlaceStart();
							G.seqUnloadBuffer.LoadWaitStart();
							G.seqUnloadTrayTable1.GripStart();
							
							if (IsReset(smm) == true)			smm.Next(Cmd.END);

							smm.Next(Cmd.TEACHINGUNLOAD5);
						}
						break;

					case Cmd.TEACHINGUNLOAD5:
						{
							if (G.seqUnloadInspPnP.IsReady ==false)				break;
							if (G.seqUnloadBuffer.IsReady ==false)				break;
							if (G.seqUnloadPnP.IsReady ==false)					break;
							if (G.seqUnloadTrayTable1.IsReady ==false)			break;

							G.seqUnloadBuffer.UnloadWaitStart();
							      G.seqUnloadPnP.PnPStart();
							
							if (IsReset(smm) == true)			smm.Next(Cmd.END);

							smm.Next(Cmd.TEACHINGUNLOAD6);
						}
						break;

					case Cmd.TEACHINGUNLOAD6:
						{
							if (G.seqUnloadPnP.IsReady ==false)			break;

							G.seqUnloadTrayTable1.ReleaseStart();
							
							if (IsReset(smm) == true)			smm.Next(Cmd.END);

							smm.Next(Cmd.TEACHINGUNLOAD7);
						}
						break;

					case Cmd.TEACHINGUNLOAD7:
						{
							SM.FMT.Clear(FlowTray.ULDTable1);
							
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

		//잡 티칭의 경우 해당 시퀀스의 레디를 다음 스탭에서 체크해야하기 때문에
		//stop동작으로 모든 시퀀스가 reset이 되어도 하던 동작을 마무리하면서 next를 타버려서 
		//동작이 이어져 버리기 때문에, 모든 시퀀스 마지막에 Reset되었는지 체크여부를 추가
		private	bool	IsReset(SMM<Cmd> smm)
		{
			return smm.CurStep == Cmd.START;
		}
	}
}
