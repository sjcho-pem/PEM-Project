using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace H_MarsSSD
{
	using	static	G;

	public	class	SeqLoadInspPnP	: SeqBase<SeqLoadInspPnP.Act, SeqLoadInspPnP.Axis, SeqLoadInspPnP.Cmd>
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
			PICK_130,
			PICK_140,
			PICK_150,
			PICK_160,
			PICK_170,
			PICK_180,

			PLACE_10,
			PLACE_20,
			PLACE_30,
			PLACE_40,
			PLACE_50,
			PLACE_60,
			PLACE_70,
			PLACE_80,
			PLACE_90,
			PLACE_100,
			PLACE_110,
			PLACE_120,
			PLACE_130,

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
			BUF1,
			BUF2,
			INSP1,
			INSP2,
			INSP_WAIT,
		}
		
		public enum PosZ
		{
			READY,
			BUFFER,
			INSP1,
			INSP2,
		}
		#endregion
		
		#region [ Variables ]
		private	SMM<Cmd>	m_smmHome		= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.LDINSPPNP_HOME));
		private	SMM<Cmd>	m_smmPick		= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.LDINSPPNP_PICK));
		private	SMM<Cmd>	m_smmPlace		= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.LDINSPPNP_PLACE));
		private	SMM<Cmd>	m_smmPnP		= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.LDINSPPNP_PNP));
		private	SMM<Cmd>	m_smmReadyPos	= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.LDINSPPNP_READYPOS));
	 
		public	Cmd	smmHome			{ get => m_smmHome.CurStep; }
		public	Cmd	smmPick			{ get => m_smmPick.CurStep; }
		public	Cmd	smmPlace		{ get => m_smmPlace.CurStep; }
		public	Cmd	smmPnP			{ get => m_smmPnP.CurStep; }
		public	Cmd	smmReadyPos		{ get => m_smmReadyPos.CurStep; }

		
		private	bool	m_IsMtHome			= true;
        private int     m_nCurLoadBuf = 0;
        private int     m_nBeforeLoadBuf = 0;
        private	int		m_nCurInspPlate		= 1;
		private	int		m_nManualLoadBuf	= -1;
		private	int		m_nManualInspPlate	= -1;

		private	PosZ		m_InspPosZ
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
		private	FlowPart	m_BufferFlow
		{ 
			get
			{
				switch (m_nCurLoadBuf)
				{
					case 0: return FlowPart.LDBuffer1;
					case 1: return FlowPart.LDBuffer2;
				}
				return FlowPart.LDBuffer1;
			}
		}
		private	FlowPart	m_InspFlow
		{
			get
			{
				switch (m_nCurInspPlate)
				{
					case 0: return FlowPart.Insp1;
					case 1: return FlowPart.Insp2;
				}
				return FlowPart.Insp1;
			}
		}
		#endregion
		
		#region [ Public Function ]

		public	void		Init()
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
				arrMt[Axis.X]		= cKernel.Motor(MT.LDInspPnPX);
				arrMt[Axis.Z]		= cKernel.Motor(MT.LDInspPnPZ);
				#endregion

				#region - Actuator
				arrAct = new EnumArray<Act, Actuator>(Act.Max);
				
				// Up/Down 1~4
				arrAct[Act.PnPUpDn] = new CDoubleSen();
				arrAct[Act.PnPUpDn].SetSenser1(new List<DI>(new DI[] { DI.LDInspPnPDnCyl }));
				arrAct[Act.PnPUpDn].SetSenser2(new List<DI>(new DI[] { DI.LDInspPnPUpCyl }));
				arrAct[Act.PnPUpDn].SetSol(DO.LDInspPnPDnCyl, DO.LDInspPnPUpCyl);
				arrAct[Act.PnPUpDn].Setting(ALM.ERROR_LOAD_INSP_PNP_UPDN_CYL_DOWN, ALM.ERROR_LOAD_INSP_PNP_UPDN_CYL_UP, ErrSenPos.BWD);
				
				
				arrAct[Act.Vac1] = new CSingleNoSen();
				arrAct[Act.Vac1].SetSol(DO.LDInspPnPVac1);
				arrAct[Act.Vac1].Setting(ALM.ERROR_LOAD_INSP_PNP_VAC1_VAC, ALM.ERROR_LOAD_INSP_PNP_VAC1_BLOW, ErrSenPos.Ori, SenPos.NONE, 200, 2000);
				
				arrAct[Act.Vac2] = new CSingleNoSen();
				arrAct[Act.Vac2].SetSol(DO.LDInspPnPVac2);
				arrAct[Act.Vac2].Setting(ALM.ERROR_LOAD_INSP_PNP_VAC2_VAC, ALM.ERROR_LOAD_INSP_PNP_VAC2_BLOW, ErrSenPos.Ori, SenPos.NONE, 200, 2000);
				
				arrAct[Act.Vac3] = new CSingleNoSen();
				arrAct[Act.Vac3].SetSol(DO.LDInspPnPVac3);
				arrAct[Act.Vac3].Setting(ALM.ERROR_LOAD_INSP_PNP_VAC3_VAC, ALM.ERROR_LOAD_INSP_PNP_VAC3_BLOW, ErrSenPos.Ori, SenPos.NONE, 200, 2000);
				
				arrAct[Act.Vac4] = new CSingleNoSen();
				arrAct[Act.Vac4].SetSol(DO.LDInspPnPVac4);
				arrAct[Act.Vac4].Setting(ALM.ERROR_LOAD_INSP_PNP_VAC4_VAC, ALM.ERROR_LOAD_INSP_PNP_VAC4_BLOW, ErrSenPos.Ori, SenPos.NONE, 200, 2000);
				

				#endregion

				base.Initialize();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}


		
		public	bool		HomeStart(bool IsMtHome = true)	{ m_IsMtHome = IsMtHome; return SetSeqRun(ref m_smmHome); }

		public	bool		PickStart()					{ return SetSeqRun(ref m_smmPick); }

		public	bool		PlaceStart()				{ return SetSeqRun(ref m_smmPlace); }

		public	bool		PnPStart()					{ return SetSeqRun(ref m_smmPnP); }
		
		public	bool		ReadyPosStart()				{ return SetSeqRun(ref m_smmReadyPos); }

		public	void		PlaceStop()					{ m_smmPlace.eInput = InState.none; m_smmPlace.Next(Cmd.START); }

		public	bool		IsHomeReady()				{ return m_smmHome.IsReady; }

		public	bool		IsPickReady()				{ return m_smmPick.IsReady; }

		public	bool		IsPlaceReady()				{ return m_smmPlace.IsReady; }

		public	bool		IsPnPReady()				{ return m_smmPnP.IsReady; }

		public	bool		IsReadyPosReady()			{ return m_smmReadyPos.IsReady; }
		
		public  void		ManualLoadBufSet(int nNum)		{ m_nManualLoadBuf		= nNum; }

		public  void		ManualInspPlateSet(int nNum)	{ m_nManualInspPlate	= nNum; }


		#endregion



		#region [ Private Function ]

		
		private	bool		SetSeqRun(ref SMM<Cmd> smm)
		{
			try
			{
				if (this.IsReady == false)			return false;

				smm.eInput	= InState.run;
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
		

		public	bool		CheckCylUpDown(SenPos eSenPos, int nProd = -1)
		{
			try
			{
				if (SM.SHM.GetUseSkipData(USESKIP.DRYRUN) == true  &&  eSenPos == SenPos.PICKER_DOWN)			return true;
				
				if (arrAct[Act.PnPUpDn].IsPos(eSenPos) == false)		return false;

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
					for (int i = 0; i < SM.FMP[FlowPart.LDInspPnP].GetMaxProdCount(); i++)
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
					for (int i = 0; i < SM.FMP[FlowPart.LDInspPnP].GetMaxProdCount(); i++)
                    {
                        if (arrAct[Act.Vac1 + i].GetPos() != eSenPos ||
                        G.DryRunIsAIn(DI.LDInspPnPVac1 + i, eSenPos == SenPos.VAC) != (eSenPos == SenPos.VAC))
                        {
                            if (bAlarm == true)
                            {
                                SM.FMP[FlowPart.LDInspPnP].SetPickupStatus(ProdPickStatus.Mismatch, i);
                                AddAlarm(ALM.ERROR_LOAD_INSP_PNP_VAC1_VAC + i);
                            }
                            else return false;
                        }
                        
					}
				}
				else
				{
					if (arrAct[Act.Vac1 + nProd].GetPos() != eSenPos  ||
						G.DryRunIsAIn(DI.LDInspPnPVac1 + nProd, eSenPos == SenPos.VAC) != (eSenPos == SenPos.VAC)   )
                    {
                        if (bAlarm == true)
                        {
                            SM.FMP[FlowPart.LDInspPnP].SetPickupStatus(ProdPickStatus.Mismatch, nProd);
                            AddAlarm(ALM.ERROR_LOAD_INSP_PNP_VAC1_VAC + nProd);
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

		// 해당 버퍼에 제품이 있는 피커만 그립을 진행
		public void RunEjectorVac(int nBufferNo)
		{
			try
			{
				FlowPart flow;
				if (nBufferNo == 0) flow = FlowPart.LDBuffer1;
				else flow = FlowPart.LDBuffer2;

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
		public bool CheckEjectorVac(int nBufferNo , bool bAlarm = false)
		{
			try
			{
				FlowPart flow;
				if (nBufferNo == 0) flow = FlowPart.LDBuffer1;
				else flow = FlowPart.LDBuffer2;

				bool[] bexist = SM.FMP[flow].GetProdExist();

				for (int i = 0; i < bexist.Length; i++)
				{
					if (bexist[i] == true && CheckEjector(SenPos.VAC, i, bAlarm) == false) return false;
				}

				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public void RunEjectorBlow(int nBufferNo)
		{
			try
			{
				if (SM.SHM.GetUseSkipData(USESKIP.DRYRUN) == true) return;

				FlowPart flow = FlowPart.LDBuffer1;
				if (nBufferNo == 1) flow = FlowPart.LDBuffer2;

				bool[] bexist = SM.FMP[flow].GetProdExist();

				for (int i = 0; i < bexist.Length; i++)
				{
					if (bexist[i] == false) RunEjector(SenPos.BLOW, i);
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public bool CheckEjectorBlow(int nBufferNo)
		{
			try
			{
				if (SM.SHM.GetUseSkipData(USESKIP.DRYRUN) == true) return true;

				FlowPart flow = FlowPart.LDBuffer1;
				if (nBufferNo == 1) flow = FlowPart.LDBuffer2;

				bool[] bexist = SM.FMP[flow].GetProdExist();

				for (int i = 0; i < bexist.Length; i++)
				{
					if (bexist[i] == false && CheckEjector(SenPos.BLOW, i) == false) return false;
				}

				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}


		// 레디위치에 있는지 확인
		public bool IsReadyPos()
		{
			try
			{
				if (CheckEjector(SenPos.BLOW) == false) return false;

				if (CheckCylUpDown(SenPos.PICKER_UP) == false) return false;

				if (arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false) return false;

				if (arrMt[Axis.X].IsRdy((int)PosX.BUF1+m_nBeforeLoadBuf) == false) return false;

				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		private ProdExist CheckPnpProdExist()
		{
			try
			{
				FlowPart flowPart = FlowPart.LDInspPnP;
				DI ExistSenStart = DI.LDInspPnPVac1;

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
				bool brt = true;

				if (TargetPosX == PosX.INSP1)
				{
					if (G.cKernel.Motor(MT.ULDInspPnPX).GetCmdIndex() == (int)SeqUnloadInspPnP.PosX.INSP1)		brt = false;
					else if (G.cKernel.Motor(MT.ULDInspPnPX).IsBiggerThan((int)SeqUnloadInspPnP.PosX.INSP2))	brt = false;
					else																						brt = true;
				}
				else if (TargetPosX == PosX.INSP2)
				{
					if (cKernel.Motor(MT.ULDInspPnPX).GetCmdIndex() == (int)SeqUnloadInspPnP.PosX.INSP1)				brt = false;
					else if (cKernel.Motor(MT.ULDInspPnPX).GetCmdIndex() == (int)SeqUnloadInspPnP.PosX.INSP2)			brt = false;
					else if (cKernel.Motor(MT.ULDInspPnPX).GetLocalPos() > (G.cKernel.Motor(MT.ULDInspPnPX).GetIndexPos((int)SeqUnloadInspPnP.PosX.INSP1) - 30000) ||
							 cKernel.Motor(MT.ULDInspPnPX).GetActPos() > (G.cKernel.Motor(MT.ULDInspPnPX).GetIndexPos((int)SeqUnloadInspPnP.PosX.INSP1) - 30000))		brt = false;
					else brt = true;
				}
				else if (TargetPosX == PosX.BUF1 || TargetPosX == PosX.BUF2)
				{
					if (arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false)          brt = false;
                    if (arrAct[Act.PnPUpDn].IsPos(SenPos.PICKER_UP) == false)    brt = false;
                  
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

		// Insp Plate에서 Grip을 해도 되는지 확인
		public bool IsReadyPlacePos(int nPartNo)
		{
			try
			{
				if (m_smmPlace.IsReady == true) return false;
				if (m_smmPlace.CurStep != Cmd.PLACE_60) return false;
				if (arrMt[Axis.X].IsRdy((int)PosX.INSP1 + nPartNo) == false) return false;
				if (arrMt[Axis.Z].IsRdy((int)PosZ.INSP1 + nPartNo) == false) return false;

				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public bool IsPlateMoveEnable(int nPartNo)
		{
			try
			{
				// Check : 인터락 보강 방법 고민
				//	if (m_smmPlace.IsReady == false)										return false;
				if (arrMt[Axis.X].IsHomeDone() == false) return false;
				if (arrMt[Axis.Z].IsHomeDone() == false) return false;
				if (arrMt[Axis.X].IsRdy((int)PosX.INSP1 + nPartNo) == true &&
					(arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false == false))
					return false;

				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}
		#endregion



		/// <summary>
		/// Run Process
		/// </summary>
		public void Run()
		{
			try
			{
				UpdateAct(Seq.LoadInspPnP);
				
				if (SM.SHM.IsBitData(BIT.AutoRun) == true)
				{
					if (m_nManualLoadBuf != -1)				m_nManualLoadBuf	= -1;
					if (m_nManualInspPlate != -1)			m_nManualInspPlate	= -1;
				}

				if (SM.SHM.IsBitData(BIT.Error) == true || SM.SHM.IsBitData(BIT.Pause) == true) return;
				if (SM.SHM.IsBitData(BIT.AutoRun) == true && IsReady == true && CheckPnpProdExist() == ProdExist.Mismatch) AddAlarm(ALM.ERROR_SEQ_LOADINSP_PnP_PROD_MISMATCH);

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
								if (SM.FMP.IsFlow(FlowPart.LDInspPnP) == false)
								{
									RunEjector(SenPos.BLOW);
								}
								else
								{
									for (int i = 0; i < SM.FMP[FlowPart.LDInspPnP].GetMaxProdCount(); i++)
									{
                                        if (SM.FMP[FlowPart.LDInspPnP].GetInfo().ProdInfo[i].Exist == false)
                                        {
                                            if (CheckEjector(SenPos.BLOW, i) == false)
                                            {
                                                RunEjector(SenPos.BLOW, i);
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            if (smm.Delayms(NDm.Read(eNDM.LoadInspPnPVacDelay)) == true)
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
                                smm.Next(Cmd.HOME_20);
                            }
							else
							{
								smm.Next(Cmd.HOME_20);
								break;
							}
						}
						break;

					case Cmd.HOME_20:
						{
							if (smm.Delayms(NDm.Read(eNDM.LoadInspPnPVacDelay)) == false) break;

							if (CheckCylUpDown(SenPos.PICKER_UP) == false)
							{
								RunCylUpDown(SenPos.PICKER_UP);
								break;
							}

                            switch (CheckPnpProdExist())
                            {
                                case ProdExist.Mismatch:
                                    AddAlarm(ALM.ERROR_SEQ_LOADINSP_PnP_PROD_MISMATCH);
                                    smm.Next(Cmd.END);
                                    break;
                                default:
                                    break;
                            }

                            if (m_IsMtHome == false)
							{
								smm.Next(Cmd.HOME_60);
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
							arrMt[Axis.X].StartHoming();

							smm.Next(Cmd.HOME_50);
						}
						break;

					case Cmd.HOME_50:
						{
							if (arrMt[Axis.X].IsHomeDone() == false || arrMt[Axis.X].IsRdy() == false) break;

							smm.Next(Cmd.HOME_60);
						}
						break;

					case Cmd.HOME_60:
						{
							if (arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false )
							{
								arrMt[Axis.Z].MoveIndex((int)PosZ.READY);
								break;
							}
							smm.Next(Cmd.HOME_70);
						}
						break;

					case Cmd.HOME_70:
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
							switch (CheckPnpProdExist())
							{
								case ProdExist.Exist:
									AddAlarm(ALM.ERROR_SEQ_LOADINSP_PnP_PROD_EXIST);
									break;
								case ProdExist.Mismatch:
									AddAlarm(ALM.ERROR_SEQ_LOADINSP_PnP_PROD_MISMATCH);
									break;
								case ProdExist.None:
									smm.Next(Cmd.PICK_20);
									break;
							}
						}
						break;

					case Cmd.PICK_20:     // Size 조절, Grip 해제, Pnp 상승, PnP 간격 조절 - 좁히기
						{
							if (CheckEjectorBlow(m_nCurLoadBuf) == false)
							{
								RunEjectorBlow(m_nCurLoadBuf);
								break;
							}

							smm.Next(Cmd.PICK_30);
						}
						break;

					case Cmd.PICK_30:     // Pnp 상승
						{
							if (CheckCylUpDown(SenPos.PICKER_UP) == false ||
								arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false )
							{
								RunCylUpDown(SenPos.PICKER_UP);
								arrMt[Axis.Z].MoveIndex((int)PosZ.READY);
								break;
							}

							smm.Next(Cmd.PICK_40);
						}
						break;

					case Cmd.PICK_40:     // 우선 순위 결정. 대기 중인 Load Buffer 확인
						{
							if (m_nManualLoadBuf == 0)
							{
								if (G.seqLoadBuffer1.IsULDWait() == false) break;
								m_nManualLoadBuf = -1;
								m_nCurLoadBuf = 0;
							}
							else if (m_nManualLoadBuf == 1)
							{
								if (G.seqLoadBuffer2.IsULDWait() == false) break;
								m_nManualLoadBuf = -1;
								m_nCurLoadBuf = 1;
							}
							else if (G.seqLoadBuffer1.IsULDWait() == true &&
									 G.seqLoadBuffer2.IsULDWait() == true)
							{
								if (SM.FMP[FlowPart.LDBuffer1].GetInfo().StartTime > SM.FMP[FlowPart.LDBuffer2].GetInfo().StartTime)
								{
									m_nCurLoadBuf = 1;
								}
								else
								{
									m_nCurLoadBuf = 0;
								}
							}
							else if (G.seqLoadBuffer1.IsULDWait() == true)
							{
								// 1번이 마지막인데 2번 보다 먼저 나갈 경우 마지막을 2번으로 변경
								if (SM.SHM.IsUsePart(UsePartName.LDBuf2, UsePartMode.Unload) == true &&
										 SM.FMP.IsFlow(FlowPart.LDBuffer2) == true &&
										 SM.FMP[FlowPart.LDBuffer1].GetInfo().LastPieceOfLot == true &&
										 SM.FMP[FlowPart.LDBuffer1].GetInfo().KeyValue == SM.FMP[FlowPart.LDBuffer2].GetInfo().KeyValue)
								{
									SM.FMP[FlowPart.LDBuffer1].SetLastOfLot(false);
									SM.FMP[FlowPart.LDBuffer2].SetLastOfLot(true);
									m_nCurLoadBuf = 0;
								}
								// 2번이 마지막인데 1번하고 LotID가 다르면 Lot 바뀐것이기 때문에 대기
								else if (SM.SHM.IsUsePart(UsePartName.LDBuf2, UsePartMode.Unload) == true &&
										 SM.FMP.IsFlow(FlowPart.LDBuffer2) == true &&
										 SM.FMP[FlowPart.LDBuffer2].GetInfo().LastPieceOfLot == true &&
										 SM.FMP[FlowPart.LDBuffer1].GetInfo().KeyValue != SM.FMP[FlowPart.LDBuffer2].GetInfo().KeyValue)
								{
									break;
								}
								else
								{
									m_nCurLoadBuf = 0;
								}
							}
							else if (G.seqLoadBuffer2.IsULDWait() == true)
							{
								// 2번이 마지막인데 1번 보다 먼저 나갈 경우 마지막을 1번으로 변경
								if (SM.SHM.IsUsePart(UsePartName.LDBuf1, UsePartMode.Unload) == true &&
										 SM.FMP.IsFlow(FlowPart.LDBuffer1) == true &&
										 SM.FMP[FlowPart.LDBuffer2].GetInfo().LastPieceOfLot == true &&
										 SM.FMP[FlowPart.LDBuffer2].GetInfo().KeyValue == SM.FMP[FlowPart.LDBuffer1].GetInfo().KeyValue)
								{
									SM.FMP[FlowPart.LDBuffer2].SetLastOfLot(false);
									SM.FMP[FlowPart.LDBuffer1].SetLastOfLot(true);
									m_nCurLoadBuf = 1;
								}
								// 1번이 마지막인데 2번하고 LotID가 다르면 Lot 바뀐것이기 때문에 대기
								else if (SM.SHM.IsUsePart(UsePartName.LDBuf1, UsePartMode.Unload) == true &&
										 SM.FMP.IsFlow(FlowPart.LDBuffer1) == true &&
										 SM.FMP[FlowPart.LDBuffer1].GetInfo().LastPieceOfLot == true &&
										 SM.FMP[FlowPart.LDBuffer2].GetInfo().KeyValue != SM.FMP[FlowPart.LDBuffer1].GetInfo().KeyValue)
								{
									break;
								}
								else
								{
									m_nCurLoadBuf = 1;
								}
							}
							else
							{
								break;
							}

							smm.Next(Cmd.PICK_50);
						}
						break;

					case Cmd.PICK_50:     // 우선 순위 위치로 X축 이동
						{
							if (m_nCurLoadBuf == 0)
							{
								if (CheckDeconflictionX(PosX.BUF1) == false) break;

								if (arrMt[Axis.X].IsRdy((int)PosX.BUF1) == false)
								{
									arrMt[Axis.X].MoveIndex((int)PosX.BUF1);
									break;
								}
							}
							else
							{
								if (CheckDeconflictionX(PosX.BUF2) == false) break;

								if (arrMt[Axis.X].IsRdy((int)PosX.BUF2) == false)
								{
									arrMt[Axis.X].MoveIndex((int)PosX.BUF2);
									break;
								}
							}

                            m_nBeforeLoadBuf = m_nCurLoadBuf == 0 ? 1 : 0;

                            smm.Next(Cmd.PICK_60);
						}
						break;

					case Cmd.PICK_60:     // Pnp 하강
						{
							if (CheckCylUpDown(SenPos.PICKER_DOWN) == false ||
								arrMt[Axis.Z].IsRdy((int)PosZ.BUFFER) == false )
							{
								RunCylUpDown(SenPos.PICKER_DOWN);
								arrMt[Axis.Z].MoveIndex((int)PosZ.BUFFER);
								break;
							}

                            RunEjectorVac(m_nCurLoadBuf);
                            smm.Next(Cmd.PICK_70);
						}
						break;

					case Cmd.PICK_70:     // Grip
						{
							if (smm.Delayms(100) == false) break;

                            if (smm.Delayms(NDm.Read(eNDM.LoadInspPnPVacDelay)) == true)
                            {
                                SM.FMP.Next(m_BufferFlow, FlowPart.LDInspPnP);
                                smm.Next(Cmd.PICK_80);
                                break;
                            }

                            if (CheckEjectorVac(m_nCurLoadBuf) == false)
							{
								RunEjectorVac(m_nCurLoadBuf);
								break;
							}

							SM.FMP.Next(m_BufferFlow, FlowPart.LDInspPnP);

							smm.Next(Cmd.PICK_80);
						}
						break;

					case Cmd.PICK_80:     // LoadBuffer Grip Release 대기
						{
							if (m_nCurLoadBuf == 0)
							{
								if (G.seqLoadBuffer1.IsBufferGripReady() == false) break;
							}
							else
							{
								if (G.seqLoadBuffer2.IsBufferGripReady() == false) break;
							}

							arrMt[Axis.Z].MoveIndex((int)PosZ.READY);

							smm.Next(Cmd.PICK_90);
						}
						break;

					case Cmd.PICK_90:     // Pnp 상승
						{
							if (CheckCylUpDown(SenPos.PICKER_UP) == false)
							{
								RunCylUpDown(SenPos.PICKER_UP);
								break;
							}

							if (arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false )
							{
								arrMt[Axis.Z].MoveIndex((int)PosZ.READY);
								break;
							}

							smm.Next(Cmd.PICK_100);
						}
						break;

					case Cmd.PICK_100:        // 제품 유무 감지
						{
							switch (CheckPnpProdExist())
							{
								case ProdExist.Exist:
									smm.Next(Cmd.END);
									break;
								case ProdExist.Mismatch:
									AddAlarm(ALM.ERROR_SEQ_LOADINSP_PnP_PROD_MISMATCH);
									break;
								case ProdExist.None:
									AddAlarm(ALM.ERROR_SEQ_LOADINSP_PnP_PROD_NONE);
									break;
							}
						}
						break;

					case Cmd.END:
						{
							// Error Clear Add

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
							switch (CheckPnpProdExist())
							{
								case ProdExist.Exist:
									smm.Next(Cmd.PLACE_20);
									break;
								case ProdExist.Mismatch:
									AddAlarm(ALM.ERROR_SEQ_LOADINSP_PnP_PROD_MISMATCH);
									break;
								case ProdExist.None:
									AddAlarm(ALM.ERROR_SEQ_LOADINSP_PnP_PROD_NONE);
									break;
							}
						}
						break;

					case Cmd.PLACE_20:        // Pnp 상승
						{
							if (CheckCylUpDown(SenPos.PICKER_UP) == false ||
								arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false)
							{
								RunCylUpDown(SenPos.PICKER_UP);
								arrMt[Axis.Z].MoveIndex((int)PosZ.READY);
								break;
							}

							smm.Next(Cmd.PLACE_30);
						}
						break;

					case Cmd.PLACE_30:        // 우선 순위 결정. 대기 중인 검사 테이블 확인
						{
							if (m_nManualInspPlate == 0)
							{
								if (G.seqInspPlate1.IsLDWait() == false ||
									SM.SHM.GetUseSkipData(USESKIP.InspDebugPlate2) == true) break;

								m_nManualInspPlate = -1;
								m_nCurInspPlate = 0;
							}
							else if (m_nManualInspPlate == 1)
							{
								if (G.seqInspPlate2.IsLDWait() == false ||
									SM.SHM.GetUseSkipData(USESKIP.InspDebugPlate1) == true) break;

								m_nManualInspPlate = -1;
								m_nCurInspPlate = 1;
							}
							else if (m_nCurInspPlate == 0)
							{
								if (G.seqInspPlate1.IsLDWait() == true &&
									SM.SHM.GetUseSkipData(USESKIP.InspDebugPlate2) == false)
								{
									m_nCurInspPlate = 0;
								}
								else if (G.seqInspPlate2.IsLDWait() == true &&
										 SM.SHM.GetUseSkipData(USESKIP.InspDebugPlate1) == false)
								{
									m_nCurInspPlate = 1;
								}
								else
								{
									if (arrMt[Axis.X].IsRdy((int)PosX.INSP_WAIT) == false)
									{
										arrMt[Axis.X].MoveIndex((int)PosX.INSP_WAIT);
									}
									break;
								}
							}
							else
							{
								if (G.seqInspPlate2.IsLDWait() == true &&
									 SM.SHM.GetUseSkipData(USESKIP.InspDebugPlate1) == false)
								{
									m_nCurInspPlate = 1;
								}
								else if (G.seqInspPlate1.IsLDWait() == true &&
										 SM.SHM.GetUseSkipData(USESKIP.InspDebugPlate2) == false)
								{
									m_nCurInspPlate = 0;
								}
								else
								{
									if (arrMt[Axis.X].IsRdy((int)PosX.INSP_WAIT) == false)
									{
										arrMt[Axis.X].MoveIndex((int)PosX.INSP_WAIT);
									}
									break;
								}
							}

							smm.Next(Cmd.PLACE_40);
						}
						break;

					case Cmd.PLACE_40:        // 우선 순위 위치로 X축 이동
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
								else
								{
									if (arrMt[Axis.X].IsRdy((int)PosX.INSP1) == false)
									{
										arrMt[Axis.X].MoveIndex((int)PosX.INSP1);
										break;
									}
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
								else
								{
									if (arrMt[Axis.X].IsRdy((int)PosX.INSP2) == false)
									{
										arrMt[Axis.X].MoveIndex((int)PosX.INSP2);
										break;
									}
								}
							}

							smm.Next(Cmd.PLACE_50);
						}
						break;

					case Cmd.PLACE_50:        // Pnp 하강
						{
							if (CheckCylUpDown(SenPos.PICKER_DOWN) == false ||
								arrMt[Axis.Z].IsRdy((int)m_InspPosZ) == false )
							{
								RunCylUpDown(SenPos.PICKER_DOWN);
								arrMt[Axis.Z].MoveIndex((int)m_InspPosZ);
								break;
							}

							if (smm.Delayms(300) == false) break;


							smm.Next(Cmd.PLACE_60);
						}
						break;

					case Cmd.PLACE_60:        // InspPlate Grip 대기
						{

                            if (smm.Delayms(100) == false) break;

                            RunEjector(SenPos.BLOW);
							smm.Next(Cmd.PLACE_70);
						}
						break;

					case Cmd.PLACE_70:        // Grip Release
                        {
                            if (m_nCurInspPlate == 0 && G.seqInspPlate1.IsGrip() != SeqInspPlate.GripState.Grip) break;
                            else if (m_nCurInspPlate == 1 && G.seqInspPlate2.IsGrip() != SeqInspPlate.GripState.Grip) break;

                            if (m_nCurInspPlate == 0) SM.FMP.Next(FlowPart.LDInspPnP, FlowPart.Insp1);
                            else SM.FMP.Next(FlowPart.LDInspPnP, FlowPart.Insp2);


                            smm.Next(Cmd.PLACE_80);
						}
						break;

					case Cmd.PLACE_80:        // PnP 상승
						{
							if (CheckCylUpDown(SenPos.PICKER_UP) == false ||
								arrMt[Axis.Z].IsRdy((int)PosX.READY) == false )
							{
								RunCylUpDown(SenPos.PICKER_UP);
								arrMt[Axis.Z].MoveIndex((int)PosX.READY);
								break;
							}

							smm.Next(Cmd.PLACE_90);
						}
						break;

					case Cmd.PLACE_90:        // RAM 유무 감지
						{
							switch (CheckPnpProdExist())
							{
								case ProdExist.Exist:
									AddAlarm(ALM.ERROR_SEQ_LOADINSP_PnP_PROD_EXIST);
									break;
								case ProdExist.Mismatch:
									AddAlarm(ALM.ERROR_SEQ_LOADINSP_PnP_PROD_MISMATCH);
									break;
								case ProdExist.None:
									if (m_nCurInspPlate == 0) m_nCurInspPlate = 1;
									else m_nCurInspPlate = 0;
									smm.Next(Cmd.END);
									break;
							}

						}
						break;

					case Cmd.END:
						{
							if (SM.SHM.IsRunUPH() == true)
							{
								SM.SHM.AddProd();
							}

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
							// Error Clear Add

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
							switch (CheckPnpProdExist())
							{
								case ProdExist.Exist:
									AddAlarm(ALM.ERROR_SEQ_LOADINSP_PnP_PROD_EXIST);
									break;
								case ProdExist.Mismatch:
									AddAlarm(ALM.ERROR_SEQ_LOADINSP_PnP_PROD_MISMATCH);
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
                            if (m_nBeforeLoadBuf == 0)
                            {
                                if (CheckDeconflictionX(PosX.BUF1) == false) break;

                                if (arrMt[Axis.X].IsRdy((int)PosX.BUF1) == false)
                                {
                                    arrMt[Axis.X].MoveIndex((int)PosX.BUF1);
                                    break;
                                }
                            }
                            else
                            {
                                if (CheckDeconflictionX(PosX.BUF2) == false) break;

                                if (arrMt[Axis.X].IsRdy((int)PosX.BUF2) == false)
                                {
                                    arrMt[Axis.X].MoveIndex((int)PosX.BUF2);
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
	}
}
