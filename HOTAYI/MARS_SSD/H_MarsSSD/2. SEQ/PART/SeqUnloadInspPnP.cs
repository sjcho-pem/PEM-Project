using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace H_MarsSSD
{
	using	static	G;

	public	class	SeqUnloadInspPnP	: SeqBase<SeqUnloadInspPnP.Act, SeqUnloadInspPnP.Axis, SeqUnloadInspPnP.Cmd>
	{
		#region [ Enum ]
		public enum Act
		{
			Vac1,
			Vac2,
			Vac3,
			Vac4,
			PnPUpDn,
			Max,
		}

		public enum Axis
		{
			X,
			Z,
			Max,
		}

		public	enum	Cmd
		{
			START	= 10,
			END		= 99,

			HOME_10	= 100,
			HOME_20,
			HOME_30,
			HOME_40,
			HOME_50,
			HOME_60,
			HOME_70,
			HOME_80,
            HOME_90,
            HOME_100,

            PICK_10,
			PICK_20,
			PICK_30,
			PICK_40,
			PICK_50,
			PICK_60,
			PICK_70,
			PICK_80,
			PICK_90,
			PICK_100,
			PICK_110,
			PICK_120,

			PLACE_10,
			PLACE_20,
			PLACE_30,
			PLACE_40,
			PLACE_50,
			PLACE_60,
			PLACE_70,
			PLACE_80,
			PLACE_90,

			PnP_10,
			PnP_20,
			PnP_30,
			PnP_40,
			PnP_50,
			PnP_60,

			READYPOS_10,
			READYPOS_20,
			READYPOS_30,
			READYPOS_40,
			READYPOS_50,
		}

		public enum PosX
		{
			READY,
			BUFFER,
			INSP1,
			INSP2,
			INSP_WAIT,
		}
		public enum PosZ
		{
			READY,
			INSP1,
			INSP2,
			BUFFER,
		}
		
		#endregion
		
		#region [ Variables ]
		private	SMM<Cmd>	m_smmHome		= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.ULDINSPPNP_HOME));
		private	SMM<Cmd>	m_smmPick		= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.ULDINSPPNP_PICK));
		private	SMM<Cmd>	m_smmPlace		= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.ULDINSPPNP_PLACE));
		private	SMM<Cmd>	m_smmPnP		= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.ULDINSPPNP_PNP));
		private	SMM<Cmd>	m_smmReadyPos	= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.ULDINSPPNP_READYPOS));

		public	Cmd	smmHome			{ get => m_smmHome.CurStep; }
		public	Cmd	smmPick			{ get => m_smmPick.CurStep; }
		public	Cmd	smmPlace		{ get => m_smmPlace.CurStep; }
		public	Cmd	smmPnP			{ get => m_smmPnP.CurStep; }
		public	Cmd	smmReadyPos		{ get => m_smmReadyPos.CurStep; }

		private	int		m_nCurInspPlate		= -1;
		private	int		m_nManualInspPlate	= -1;
		private	bool	m_IsMtHome			= true;

		private	PosZ	m_InspZPos
		{ 
			get
			{
				switch (m_nCurInspPlate)
				{
					case 0: return PosZ.INSP1;
					case 1: return PosZ.INSP2;
				}
				return PosZ.INSP1;
			}
		}

		private	bool	m_bIsGrip	= false;
		public  bool	bIsGrip
		{
			get => m_bIsGrip; 
		}
		#endregion

		public	void	Init()
		{
			try
			{
				lstSMM.Add(m_smmHome);
				lstSMM.Add(m_smmPick);
				lstSMM.Add(m_smmPlace);
				lstSMM.Add(m_smmPnP);
				lstSMM.Add(m_smmReadyPos);

				#region - Motor
				arrMt		= new EnumArray<Axis, CMotor>(Axis.Max);
				arrMt[Axis.X]		= cKernel.Motor(MT.ULDInspPnPX);
				arrMt[Axis.Z]		= cKernel.Motor(MT.ULDInspPnPZ);
				#endregion

				#region - Actuator
				arrAct	= new EnumArray<Act, Actuator>(Act.Max);
				
				// Up/Down 1~4
				arrAct[Act.PnPUpDn] = new CDoubleSen();
				arrAct[Act.PnPUpDn].SetSenser1(new List<DI>(new DI[] { DI.ULDInspPnPDnCyl }));
				arrAct[Act.PnPUpDn].SetSenser2(new List<DI>(new DI[] { DI.ULDInspPnPUpCyl }));
				arrAct[Act.PnPUpDn].SetSol(DO.ULDInspPnPDnCyl, DO.ULDInspPnPUpCyl);
				arrAct[Act.PnPUpDn].Setting(ALM.ERROR_UNLOAD_INSP_PNP_UPDN_CYL_DOWN, ALM.ERROR_UNLOAD_INSP_PNP_UPDN_CYL_UP, ErrSenPos.BWD);
				
				
				arrAct[Act.Vac1] = new CSingleNoSen();
				arrAct[Act.Vac1].SetSol(DO.ULDInspPnPVac1);
				arrAct[Act.Vac1].Setting(ALM.ERROR_UNLOAD_INSP_PNP_VAC1_VAC, ALM.ERROR_UNLOAD_INSP_PNP_VAC1_BLOW, ErrSenPos.Ori, SenPos.NONE, 200, 2000);
				
				arrAct[Act.Vac2] = new CSingleNoSen();
				arrAct[Act.Vac2].SetSol(DO.ULDInspPnPVac2);
				arrAct[Act.Vac2].Setting(ALM.ERROR_UNLOAD_INSP_PNP_VAC2_VAC, ALM.ERROR_UNLOAD_INSP_PNP_VAC2_BLOW, ErrSenPos.Ori, SenPos.NONE, 200, 2000);
				
				arrAct[Act.Vac3] = new CSingleNoSen();
				arrAct[Act.Vac3].SetSol(DO.ULDInspPnPVac3);
				arrAct[Act.Vac3].Setting(ALM.ERROR_UNLOAD_INSP_PNP_VAC3_VAC, ALM.ERROR_UNLOAD_INSP_PNP_VAC3_BLOW, ErrSenPos.Ori, SenPos.NONE, 200, 2000);
				
				arrAct[Act.Vac4] = new CSingleNoSen();
				arrAct[Act.Vac4].SetSol(DO.ULDInspPnPVac4);
				arrAct[Act.Vac4].Setting(ALM.ERROR_UNLOAD_INSP_PNP_VAC4_VAC, ALM.ERROR_UNLOAD_INSP_PNP_VAC4_BLOW, ErrSenPos.Ori, SenPos.NONE, 200, 2000);
				

				#endregion
				
				base.Initialize();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
		
		public	bool	HomeStart(bool IsMtHome = true)		{ m_IsMtHome = IsMtHome; return SetSeqRun(ref m_smmHome); }

		public	bool	PickStart()						{ return SetSeqRun(ref m_smmPick); }

		public	bool	PlaceStart()					{ return SetSeqRun(ref m_smmPlace); }

		public	bool	PnPStart()						{ return SetSeqRun(ref m_smmPnP); }

		public	bool	ReadyPosStart()					{ return SetSeqRun(ref m_smmReadyPos); }

		public	void	PlaceStop()						{ m_smmPlace.eInput = InState.none; m_smmPlace.Next(Cmd.START); }
		
		public	bool	IsHomeReady()					{ return m_smmHome.IsReady; }

		public	bool	IsPickReady()					{ return m_smmPick.IsReady; }

		public	bool	IsPlaceReady()					{ return m_smmPlace.IsReady; }

		public	bool	IsPnPReady()					{ return m_smmPnP.IsReady; }
		
		public  void	ManualInspPlateSet(int nNum)	{ m_nManualInspPlate = nNum; }


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
		

		public void RunCylUpDown(SenPos eSenPos, int nProd = -1)
		{
			try
			{
				if (SM.SHM.GetUseSkipData(USESKIP.DRYRUN) == true && eSenPos == SenPos.PICKER_DOWN) return;
				
				arrAct[Act.PnPUpDn].SetPos(eSenPos);

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

				if (arrAct[Act.PnPUpDn].IsPos(eSenPos) == false) return false;
				
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
					for (int i = 0; i < SM.FMP[FlowPart.ULDInspPnP].GetMaxProdCount(); i++)
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
					for (int i = 0; i < SM.FMP[FlowPart.ULDInspPnP].GetMaxProdCount(); i++)
					{
						if (arrAct[Act.Vac1 + i].GetPos() != eSenPos ||
						G.DryRunIsAIn(DI.ULDInspPnPVac1 + i, eSenPos == SenPos.VAC) != (eSenPos == SenPos.VAC))
                        {
                            if (bAlarm == true)
                            {
                                SM.FMP[FlowPart.ULDInspPnP].SetPickupStatus(ProdPickStatus.Mismatch, i);
                                AddAlarm(ALM.ERROR_UNLOAD_INSP_PNP_VAC1_VAC + i);
                            }
                            else                    return false;
                        }
					}
				}
				else
				{
					if (arrAct[Act.Vac1 + nProd].GetPos() != eSenPos  ||
						G.DryRunIsAIn(DI.ULDInspPnPVac1 + nProd, eSenPos == SenPos.VAC) != (eSenPos == SenPos.VAC)   )
                    {

                        if (bAlarm == true)
                        {
                            SM.FMP[FlowPart.ULDInspPnP].SetPickupStatus(ProdPickStatus.Mismatch, nProd);
                            AddAlarm(ALM.ERROR_UNLOAD_INSP_PNP_VAC1_VAC + nProd);
                        }
                        else                    return false;
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

		public void RunEjectorVacExist(int nPlateNo)
		{
			try
			{
				FlowPart flow;
				if (nPlateNo == 0)		flow = FlowPart.Insp1;
				else					flow = FlowPart.Insp2;

				bool[] bexist = SM.FMP[flow].GetProdExist();

				for (int i = 0; i < bexist.Length; i++)
				{
					if (bexist[i] == true) RunEjector(SenPos.VAC, i);
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		// 버퍼에 제품이 있는 피커만 그립이 되었는지 확인
		public bool CheckEjectorVacExist(int nPlateNo, bool bAlarm = false)
		{
			try
			{
				FlowPart flow;
				if (nPlateNo == 0)		flow = FlowPart.Insp1;
				else					flow = FlowPart.Insp2;

				bool[] bexist = SM.FMP[flow].GetProdExist();

				for (int i = 0; i < bexist.Length; i++)
				{
					if (bexist[i] == true && CheckEjector(SenPos.VAC, i , bAlarm) == false) return false;
				}

				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}


		private ProdExist CheckProdExist()
		{
			try
			{
				FlowPart flowPart = FlowPart.ULDInspPnP;
				DI ExistSenStart = DI.ULDInspPnPVac1;

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
		private bool CheckDeconflictionX(PosX TargetPosX)
		{
			try
			{
				bool brt = false;
				
				if (TargetPosX == PosX.INSP1)
				{
					if (cKernel.Motor(MT.LDInspPnPX).GetCmdIndex() == (int)SeqLoadInspPnP.PosX.INSP1)					brt = false;
					else if (cKernel.Motor(MT.LDInspPnPX).GetCmdIndex() == (int)SeqLoadInspPnP.PosX.INSP2)				brt = false;
					else if (G.cKernel.Motor(MT.LDInspPnPX).IsBiggerThan((int)SeqLoadInspPnP.PosX.INSP_WAIT) == true)	brt = false;
					else brt = true;
				}
				else if (TargetPosX == PosX.INSP2)
				{
					if (cKernel.Motor(MT.LDInspPnPX).GetCmdIndex() == (int)SeqLoadInspPnP.PosX.INSP2)				brt = false;
					else if (cKernel.Motor(MT.LDInspPnPX).GetLocalPos() > (G.cKernel.Motor(MT.LDInspPnPX).GetIndexPos((int)SeqLoadInspPnP.PosX.INSP1) - 30000) ||
							 cKernel.Motor(MT.LDInspPnPX).GetActPos() > (G.cKernel.Motor(MT.LDInspPnPX).GetIndexPos((int)SeqLoadInspPnP.PosX.INSP1) - 30000)) brt = false;
					else brt = true;
				}
				else if (TargetPosX == PosX.BUFFER)
				{
					if (arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false)	brt = false;
				}
				else
				{
					return true;
				}

				if (brt == false && arrMt[Axis.X].GetCmdIndex() == (int)TargetPosX)
				{
					arrMt[Axis.X].EStop();
				}

				return brt;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public bool IsReadyPos()
		{
			try
			{
				if (arrAct[Act.PnPUpDn].IsPos(SenPos.PICKER_UP) == false )
					return false;

				if (arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false)
					return false;
				
				if (arrMt[Axis.X].IsRdy((int)PosX.INSP_WAIT) == false)
					return false;

				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}




		/// <summary>
		/// Run Process
		/// </summary>
		public void Run()
		{
			try
			{
				UpdateAct(Seq.UnloadInspPnP);

				if (SM.SHM.IsBitData(BIT.AutoRun) == true && m_nManualInspPlate != -1) m_nManualInspPlate = -1;
				if (SM.SHM.IsBitData(BIT.Error) == true || SM.SHM.IsBitData(BIT.Pause) == true) return;
				if (SM.SHM.IsBitData(BIT.AutoRun) == true && IsReady == true && CheckProdExist() == ProdExist.Mismatch) AddAlarm(ALM.ERROR_SEQ_UNLOADINSP_PnP_PROD_MISMATCH);

				Home(m_smmHome);
				Pick(m_smmPick);
				Place(m_smmPlace);
				PnP(m_smmPnP);
				ReadyPos(m_smmReadyPos);

				SM.SHM.SetSeqStatus(lstSMM);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public void Home(SMM<Cmd> smm)
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
							if (SM.SHM.GetUseSkipData(USESKIP.DRYRUN) == false)
							{
								if (SM.FMP.IsFlow(FlowPart.ULDInspPnP) == false)
								{
									RunEjector(SenPos.BLOW);
								}
								else
								{
									for (int i = 0; i < SM.FMP[FlowPart.ULDInspPnP].GetMaxProdCount(); i++)
                                    {
                                        if (SM.FMP[FlowPart.ULDInspPnP].GetInfo().ProdInfo[i].Exist == false)
                                        {
                                            if (CheckEjector(SenPos.BLOW, i) == false)
                                            {
                                                RunEjector(SenPos.BLOW, i);
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            if (smm.Delayms(NDm.Read(eNDM.UnloadInspPnPVacDelay)) == true)
                                            {
                                                smm.Next(Cmd.HOME_20);
                                                break;
                                            }

                                            if (CheckEjector(SenPos.VAC, i) == false)
                                            {
                                                RunEjector(SenPos.VAC, i);
                                                break;
                                            }
                                        }
                                    }
								}
							}

							smm.Next(Cmd.HOME_20);
						}
						break;

					case Cmd.HOME_20:
                        {
                            if (smm.Delayms(NDm.Read(eNDM.UnloadInspPnPVacDelay)) == false) break;

                            m_bIsGrip = false;
							smm.Next(Cmd.HOME_30);
						}
						break;

					case Cmd.HOME_30:
						{
							if (arrAct[Act.PnPUpDn].IsPos(SenPos.PICKER_UP) == false)
							{
								arrAct[Act.PnPUpDn].SetPos(SenPos.PICKER_UP);
								break;
							}

                            if (CheckProdExist() == ProdExist.Mismatch)
                            {
                                AddAlarm(ALM.ERROR_SEQ_UNLOADINSP_PnP_PROD_MISMATCH);
                                smm.Next(Cmd.END);
                                break;
                            }

                            smm.Next(Cmd.HOME_40);
						}
						break;

					case Cmd.HOME_40:
						{
							if (m_IsMtHome == false)
							{
								smm.Next(Cmd.HOME_70);
								break;
							}

							arrMt[Axis.Z].ClearHome();
							arrMt[Axis.Z].StartHoming();

							smm.Next(Cmd.HOME_50);
						}
						break;

					case Cmd.HOME_50:
                        {
                            if (smm.Delayms(100) == false) break;

                            if (arrMt[Axis.Z].IsHomeDone() == false || arrMt[Axis.Z].IsRdy() == false) break;
							
							arrMt[Axis.X].ClearHome();
							arrMt[Axis.X].StartHoming();


							smm.Next(Cmd.HOME_60);
						}
						break;

					case Cmd.HOME_60:
						{
							if (arrMt[Axis.X].IsHomeDone() == false || arrMt[Axis.X].IsRdy() == false) break;

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
							if (arrMt[Axis.X].IsRdy((int)PosX.READY) == false)
							{
								arrMt[Axis.X].MoveIndex((int)PosX.READY);
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

		public void Pick(SMM<Cmd> smm)
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

								smm.Next(Cmd.PICK_10);
							}
						}
						break;

					case Cmd.PICK_10:     // 제품 유무 확인
						{
							switch (CheckProdExist())
							{
								case ProdExist.Mismatch:
									AddAlarm(ALM.ERROR_SEQ_UNLOADINSP_PnP_PROD_MISMATCH);
									break;
								case ProdExist.Exist:
									AddAlarm(ALM.ERROR_SEQ_UNLOADINSP_PnP_PROD_EXIST);
									break;
								case ProdExist.None:
									smm.Next(Cmd.PICK_20);
									break;
							}
						}
						break;

					case Cmd.PICK_20:     // Size 조절, Grip 해제
						{
							m_bIsGrip = false;
							smm.Next(Cmd.PICK_30);
						}
						break;

					case Cmd.PICK_30:     // Pnp 상승
						{
							if (CheckCylUpDown(SenPos.PICKER_UP) == false ||
								arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false)
							{
								RunCylUpDown(SenPos.PICKER_UP);
								arrMt[Axis.Z].MoveIndex((int)PosZ.READY);
								break;
							}

							smm.Next(Cmd.PICK_40);
						}
						break;

					case Cmd.PICK_40:     // 우선 순위 결정. 대기 중인 검사테이블 확인
						{
							if (m_nManualInspPlate == 0)
							{
								if (G.seqInspPlate1.IsULDWait() == false) break;

								m_nManualInspPlate = -1;
								m_nCurInspPlate = 0;
							}
							else if (m_nManualInspPlate == 1)
							{
								if (G.seqInspPlate2.IsULDWait() == false) break;

								m_nManualInspPlate = -1;
								m_nCurInspPlate = 1;
							}
							else if (G.seqInspPlate1.IsULDWait() == true && G.seqInspPlate2.IsULDWait() == true)
							{
								if (SM.FMP[FlowPart.Insp1].GetInfo().StartTime > SM.FMP[FlowPart.Insp2].GetInfo().StartTime)
								{
									m_nCurInspPlate = 1;
								}
								else
								{
									m_nCurInspPlate = 0;
								}
							}
							else if (G.seqInspPlate1.IsULDWait() == true)
							{
								// 1번이 마지막인데 2번 보다 먼저 나갈 경우 마지막을 2번으로 변경
								if (SM.SHM.IsUsePart(UsePartName.Insp2, UsePartMode.Unload) == true &&
										 SM.FMP.IsFlow(FlowPart.Insp2) == true &&
										 SM.FMP[FlowPart.Insp1].GetInfo().LastPieceOfLot == true &&
										 SM.FMP[FlowPart.Insp1].GetInfo().KeyValue == SM.FMP[FlowPart.Insp2].GetInfo().KeyValue)
								{
									SM.FMP[FlowPart.Insp1].SetLastOfLot(false);
									SM.FMP[FlowPart.Insp2].SetLastOfLot(true);
									m_nCurInspPlate = 0;
								}
								// 2번이 마지막인데 1번하고 LotID가 다르면 Lot 바뀐것이기 때문에 대기
								else if (SM.SHM.IsUsePart(UsePartName.Insp2, UsePartMode.Unload) == true &&
										 SM.FMP.IsFlow(FlowPart.Insp2) == true &&
										 SM.FMP[FlowPart.Insp2].GetInfo().LastPieceOfLot == true &&
										 SM.FMP[FlowPart.Insp1].GetInfo().KeyValue != SM.FMP[FlowPart.Insp2].GetInfo().KeyValue)
								{
									break;
								}
								else
								{
									m_nCurInspPlate = 0;
								}
							}
							else if (G.seqInspPlate2.IsULDWait() == true)
							{
								// 2번이 마지막인데 1번 보다 먼저 나갈 경우 마지막을 1번으로 변경
								if (SM.SHM.IsUsePart(UsePartName.Insp1, UsePartMode.Unload) == true &&
										 SM.FMP.IsFlow(FlowPart.Insp1) == true &&
										 SM.FMP[FlowPart.Insp2].GetInfo().LastPieceOfLot == true &&
										 SM.FMP[FlowPart.Insp2].GetInfo().KeyValue == SM.FMP[FlowPart.Insp1].GetInfo().KeyValue)
								{
									SM.FMP[FlowPart.Insp2].SetLastOfLot(false);
									SM.FMP[FlowPart.Insp1].SetLastOfLot(true);
									m_nCurInspPlate = 1;
								}
								// 1번이 마지막인데 2번하고 LotID가 다르면 Lot 바뀐것이기 때문에 대기
								else if (SM.SHM.IsUsePart(UsePartName.Insp1, UsePartMode.Unload) == true &&
										 SM.FMP.IsFlow(FlowPart.Insp1) == true &&
										 SM.FMP[FlowPart.Insp1].GetInfo().LastPieceOfLot == true &&
										 SM.FMP[FlowPart.Insp2].GetInfo().KeyValue != SM.FMP[FlowPart.Insp1].GetInfo().KeyValue)
								{
									break;
								}
								else
								{
									m_nCurInspPlate = 1;
								}
							}
							else
							{
								if (arrMt[Axis.X].IsRdy((int)PosX.INSP_WAIT) == false)
								{
									arrMt[Axis.X].MoveIndex((int)PosX.INSP_WAIT);
									break;
								}
								break;
							}

							smm.Next(Cmd.PICK_50);
						}
						break;

					case Cmd.PICK_50:     // 우선 순위 위치로 X축 이동
						{
							if (m_nCurInspPlate == 0)
							{
								if (CheckDeconflictionX(PosX.INSP1) == false)
								{
									if (arrMt[Axis.X].IsRdy((int)PosX.INSP_WAIT) == false)
									{
										arrMt[Axis.X].MoveIndex((int)PosX.INSP_WAIT);
										break;
									}
									break;
								}

								if (arrMt[Axis.X].IsRdy((int)PosX.INSP1) == false)
								{
									arrMt[Axis.X].MoveIndex((int)PosX.INSP1);
									break;
								}
							}
							else
							{
								if (CheckDeconflictionX(PosX.INSP2) == false)
								{
									if (arrMt[Axis.X].IsRdy((int)PosX.INSP_WAIT) == false)
									{
										arrMt[Axis.X].MoveIndex((int)PosX.INSP_WAIT);
										break;
									}
									break;
								}

								if (arrMt[Axis.X].IsRdy((int)PosX.INSP2) == false)
								{
									arrMt[Axis.X].MoveIndex((int)PosX.INSP2);
									break;
								}
							}


							smm.Next(Cmd.PICK_60);
						}
						break;

					case Cmd.PICK_60:     // Pnp 하강
						{
							if (m_nCurInspPlate == 0 && (G.seqInspPlate1.IsULDWait() == false || G.seqInspPlate1.IsUnloadPosition() == false)) break;
							else if (m_nCurInspPlate == 1 && (G.seqInspPlate2.IsULDWait() == false || G.seqInspPlate2.IsUnloadPosition() == false)) break;

							if (CheckCylUpDown(SenPos.PICKER_DOWN) == false ||
								arrMt[Axis.Z].IsRdy((int)m_InspZPos) == false)
							{
								RunCylUpDown(SenPos.PICKER_DOWN);
								arrMt[Axis.Z].MoveIndex((int)m_InspZPos);
								break;
							}

							if (smm.Delayms(100) == false) break;

                            RunEjectorVacExist(m_nCurInspPlate);
                            smm.Next(Cmd.PICK_70);
						}
						break;

					case Cmd.PICK_70:     // Grip
                        {
                            if (smm.Delayms(NDm.Read(eNDM.UnloadInspPnPVacDelay)) == true)
                            {
                                m_bIsGrip = true;

                                if (m_nCurInspPlate == 0) SM.FMP.Next(FlowPart.Insp1, FlowPart.ULDInspPnP);
                                else SM.FMP.Next(FlowPart.Insp2, FlowPart.ULDInspPnP);

                                smm.Next(Cmd.PICK_80);
                                break;
                            }

                            if (CheckEjectorVacExist(m_nCurInspPlate) == false)
							{
								RunEjectorVacExist(m_nCurInspPlate);
							}
							
							m_bIsGrip = true;

							if (m_nCurInspPlate == 0)	SM.FMP.Next(FlowPart.Insp1, FlowPart.ULDInspPnP);
							else						SM.FMP.Next(FlowPart.Insp2, FlowPart.ULDInspPnP);

							smm.Next(Cmd.PICK_80);
						}
						break;

					case Cmd.PICK_80:     // InspPlate Grip Release 대기
						{
							if (smm.Delayms(NDm.Read(eNDM.UnloadInspPnPVacDelay)) == false) break;

							if (m_nCurInspPlate == 0)
							{
								if (G.seqInspPlate1.IsGrip() != SeqInspPlate.GripState.Release) break;
							}
							else
							{
								if (G.seqInspPlate2.IsGrip() != SeqInspPlate.GripState.Release) break;
							}

							if (smm.Delayms(200) == false) break;

							smm.Next(Cmd.PICK_90);
						}
						break;

					case Cmd.PICK_90:     // Pnp 상승
						{
							if (CheckCylUpDown(SenPos.PICKER_UP) == false ||
								arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false )
							{
								RunCylUpDown(SenPos.PICKER_UP);
								arrMt[Axis.Z].MoveIndex((int)PosZ.READY);
								break;
							}

							smm.Next(Cmd.PICK_100);
						}
						break;

					case Cmd.PICK_100:        // 제품 유무 감지
						{
							switch (CheckProdExist())
							{
								case ProdExist.Exist:
									if (m_nCurInspPlate == 0) m_nCurInspPlate = 1;
									else m_nCurInspPlate = 0;
									smm.Next(Cmd.END);
									break;
								case ProdExist.Mismatch:
									AddAlarm(ALM.ERROR_SEQ_UNLOADINSP_PnP_PROD_MISMATCH);
									break;
								case ProdExist.None:
									AddAlarm(ALM.ERROR_SEQ_UNLOADINSP_PnP_PROD_NONE);
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

		public void Place(SMM<Cmd> smm)
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

								smm.Next(Cmd.PLACE_10);
							}
						}
						break;

					case Cmd.PLACE_10:        // RAM 유무 확인
						{
							switch (CheckProdExist())
							{
								case ProdExist.Exist:
									smm.Next(Cmd.PLACE_20);
									break;
								case ProdExist.Mismatch:
									AddAlarm(ALM.ERROR_SEQ_UNLOADINSP_PnP_PROD_MISMATCH);
									break;
								case ProdExist.None:
									AddAlarm(ALM.ERROR_SEQ_UNLOADINSP_PnP_PROD_NONE);
									break;
							}
						}
						break;

					case Cmd.PLACE_20:        // Pnp 상승
						{
							if (CheckCylUpDown(SenPos.PICKER_UP) == false ||
								arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false )
							{
								RunCylUpDown(SenPos.PICKER_UP);
								arrMt[Axis.Z].MoveIndex((int)PosZ.READY);
								break;
							}

							smm.Next(Cmd.PLACE_30);
						}
						break;

					case Cmd.PLACE_30:        // X축 이동
						{
							if (arrMt[Axis.X].IsRdy((int)PosX.BUFFER) == false )
							{
								arrMt[Axis.X].MoveIndex((int)PosX.BUFFER);
								break;
							}

							smm.Next(Cmd.PLACE_40);
						}
						break;

					case Cmd.PLACE_40:        // UnloadBuffer 준비 상태 대기
						{
							if (G.seqUnloadBuffer.IsLDWait() == false) break;

							smm.Next(Cmd.PLACE_50);
						}
						break;

					case Cmd.PLACE_50:        // Pnp 하강
						{
							if (CheckCylUpDown(SenPos.PICKER_DOWN) == false ||
								arrMt[Axis.Z].IsRdy((int)PosZ.BUFFER) == false )
							{
								RunCylUpDown(SenPos.PICKER_DOWN);
								arrMt[Axis.Z].MoveIndex((int)PosZ.BUFFER);
								break;
							}

							if (smm.Delayms(100) == false) break;

							smm.Next(Cmd.PLACE_60);
						}
						break;

					case Cmd.PLACE_60:        // Grip Release
                        {
                            if (CheckEjector(SenPos.BLOW) == false)
                            {
                                RunEjector(SenPos.BLOW);
                                break;
                            }

                            if (smm.Delayms(NDm.Read(eNDM.UnloadInspPnPVacDelay)) == false) break;

                               SM.FMP.Next(FlowPart.ULDInspPnP, FlowPart.ULDBuffer);

							m_bIsGrip = false;

							smm.Next(Cmd.PLACE_70);
						}
						break;

					case Cmd.PLACE_70:        // PnP 상승
						{
							if (CheckCylUpDown(SenPos.PICKER_UP) == false ||
								arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false )
							{
								RunCylUpDown(SenPos.PICKER_UP);
								arrMt[Axis.Z].MoveIndex((int)PosZ.READY);
								break;
							}

							smm.Next(Cmd.PLACE_80);
						}
						break;

					case Cmd.PLACE_80:        // RAM 유무 감지
						{
							switch (CheckProdExist())
							{
								case ProdExist.Exist:
									AddAlarm(ALM.ERROR_SEQ_UNLOADINSP_PnP_PROD_EXIST);
									break;
								case ProdExist.Mismatch:
									AddAlarm(ALM.ERROR_SEQ_UNLOADINSP_PnP_PROD_MISMATCH);
									break;
								case ProdExist.None:
									smm.Next(Cmd.END);
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

		public void PnP(SMM<Cmd> smm)
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

								GLog.Add("Pick & Place Start");
								smm.Next(Cmd.PnP_10);
							}
						}
						break;

					case Cmd.PnP_10:      // Pick Start
						{
							m_smmPick.eInput = InState.run;

							smm.Next(Cmd.PnP_20);
						}
						break;

					case Cmd.PnP_20:      // Pick End
						{
							if (IsPickReady() == false) break;

							smm.Next(Cmd.PnP_30);
						}
						break;

					case Cmd.PnP_30:      // Place Start
						{
							m_smmPlace.eInput = InState.run;

							smm.Next(Cmd.PnP_40);
						}
						break;

					case Cmd.PnP_40:      // Place End
						{
							if (IsPlaceReady() == false) break;

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

		public void ReadyPos(SMM<Cmd> smm)
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

								smm.Next(Cmd.READYPOS_10);
							}
						}
						break;

					case Cmd.READYPOS_10:
						{
							switch (CheckProdExist())
							{
								case ProdExist.Exist:
									AddAlarm(ALM.ERROR_SEQ_UNLOADINSP_PnP_PROD_EXIST);
									break;
								case ProdExist.Mismatch:
									AddAlarm(ALM.ERROR_SEQ_UNLOADINSP_PnP_PROD_MISMATCH);
									break;
								case ProdExist.None:
									smm.Next(Cmd.READYPOS_20);
									break;
							}
						}
						break;

					case Cmd.READYPOS_20:
						{
							if (CheckEjector(SenPos.BLOW) == false)
							{
								RunEjector(SenPos.BLOW);
								break;
							}

							smm.Next(Cmd.READYPOS_30);
						}
						break;

					case Cmd.READYPOS_30:
						{
							if (CheckCylUpDown(SenPos.PICKER_UP) == false ||
								arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false )
							{
								RunCylUpDown(SenPos.PICKER_UP);
								arrMt[Axis.Z].MoveIndex((int)PosZ.READY);
								break;
							}

							smm.Next(Cmd.READYPOS_40);
						}
						break;

					case Cmd.READYPOS_40:
						{
							if (arrMt[Axis.X].IsRdy((int)PosX.INSP_WAIT) == false)
							{
								arrMt[Axis.X].MoveIndex((int)PosX.INSP_WAIT);
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
