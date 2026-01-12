using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace H_MarsSSD
{
	using	static	G;

	public	class	SeqInspPlate	: SeqBase<SeqInspPlate.Act, SeqInspPlate.Axis, SeqInspPlate.Cmd>
	{
		#region [ Enum ]
		public enum Act
		{
			Grip,
			Max,
		}

		public enum Axis
		{
			GRIP,
			W,
			Z,
			Y,
			Max,
		}

		public	enum	Cmd
		{
			START	= 10,
			END		= 99,

			HOME_10	= 100,
			HOME_11,
            HOME_12,
            HOME_13,
            HOME_20,
            HOME_30,
            HOME_40,
			HOME_50,
			HOME_60,
			HOME_70,
			HOME_80,
			HOME_90,
			HOME_100,

			LD_WAIT_10,
			LD_WAIT_20,
			LD_WAIT_30,
			LD_WAIT_40,
			LD_WAIT_50,
			LD_WAIT_60,
			LD_WAIT_70,
			LD_WAIT_80,
			LD_WAIT_90,
			
			ULD_WAIT_10,
			ULD_WAIT_20,
			ULD_WAIT_30,
			ULD_WAIT_40,
			ULD_WAIT_41,
			ULD_WAIT_42,
			ULD_WAIT_50,
			ULD_WAIT_60,
			ULD_WAIT_70,
			ULD_WAIT_80,
			ULD_WAIT_90,
			
			INSP_WAIT_01,
			INSP_WAIT_02,
			INSP_WAIT_10,
			INSP_WAIT_20,
			INSP_WAIT_30,
			INSP_WAIT_31,
			INSP_WAIT_32,
			INSP_WAIT_40,
			INSP_WAIT_50,
			INSP_WAIT_60,
			INSP_WAIT_70,
			INSP_WAIT_80,
			INSP_WAIT_90,
			INSP_WAIT_90_JobUpdateCheck1,
			INSP_WAIT_90_JobUpdateCheck2,
			INSP_WAIT_90_JobUpdateCheck3,
			INSP_WAIT_90_JobUpdateCheck4,
			INSP_WAIT_90_JobUpdateCheck5,
			INSP_WAIT_90_JobUpdateCheck6,
			INSP_WAIT_90_JobUpdateCheck7,
			INSP_WAIT_90_JobUpdateCheck8,
			INSP_WAIT_100,
			INSP_WAIT_110,
			INSP_WAIT_120,
			INSP_WAIT_130,
			INSP_WAIT_140,
			INSP_WAIT_150,
			INSP_WAIT_160,
			INSP_WAIT_170,
			INSP_WAIT_180,
			INSP_WAIT_190,
			INSP_WAIT_200,
			INSP_WAIT_210,
			
			RETRY_DELEAY_10,
			RETRY_DELEAY_20,
			RETRY_DELEAY_30,
			RETRY_DELEAY_40,

			INSP_AUTORUN_10,
			INSP_AUTORUN_20,
			INSP_AUTORUN_30,
			INSP_AUTORUN_40,
			INSP_AUTORUN_50,
			INSP_AUTORUN_60,
			
			TEACH_WAIT_01,
			TEACH_WAIT_02,
			TEACH_WAIT_10,
			TEACH_WAIT_20,
			TEACH_WAIT_30,

			REFERENCE_10,
			REFERENCE_20,
			REFERENCE_30,
			REFERENCE_40,
			REFERENCE_50,
			REFERENCE_60,
			REFERENCE_70,
			REFERENCE_80,
			REFERENCE_90,
			REFERENCE_100,
			REFERENCE_110,
			REFERENCE_INSPRUN_10,
			REFERENCE_INSPRUN_20,
			REFERENCE_INSPRUN_30,
			REFERENCE_INSPRUN_40,
			REFERENCE_INSPRUN_50,
			REFERENCE_INSPRUN_60,
			REFERENCE_INSPRUN_70,
			REFERENCE_INSPRUN_80,
			REFERENCE_INSPRUN_90,
			
			
			CALIBRATION_01,
			CALIBRATION_02,
			CALIBRATION_03,
			CALIBRATION_10,
			CALIBRATION_20,
			CALIBRATION_30,
			CALIBRATION_40,
			CALIBRATION_50,
			CALIBRATION_60,
			CALIBRATION_70,
			CALIBRATION_80,
			CALIBRATION_90,
			CALIBRATION_100,

			
			MAINTANCE_10,
			MAINTANCE_20,
			MAINTANCE_30,
			MAINTANCE_40,
			MAINTANCE_50,
			MAINTANCE_60,
			MAINTANCE_70,
			MAINTANCE_80,
		}

		public enum PosY
		{
			LOAD,
			UNLOAD,
			INSP,
			REFERENCE,
			CALIBRATION,
		}

		public enum PosGrip
		{
			READY,
            LOAD,
            UNLOAD,
			GRIP,
			CALIBRATION,
		}
		
		public enum PosW
		{
			READY,
			LOAD,
            UNLOAD,
            GRIP,
        }
		public enum PosZ
		{
			READY,
			LOAD,
			UNLOAD,
		}

		public	enum GripState
		{
			Unknown,
			Grip,
			Release,
		}

		#endregion

		#region [ Variables ]
		private	SMM<Cmd>		m_smmHome;
		private	SMM<Cmd>		m_smmLoadWait;
		private	SMM<Cmd>		m_smmUnloadWait;
		private	SMM<Cmd>		m_smmInspWait;
		private	SMM<Cmd>		m_smmTeachWait;
		private	SMM<Cmd>		m_smmReference;
		private	SMM<Cmd>		m_smmCalibration;

		public	Cmd			smmHome			{ get => m_smmHome.CurStep; }
		public	Cmd			smmLoadWait		{ get => m_smmLoadWait.CurStep; }
		public	Cmd			smmUnloadWait	{ get => m_smmUnloadWait.CurStep; }
		public	Cmd			smmInspWait		{ get => m_smmInspWait.CurStep; }
		public	Cmd			smmTeachWait	{ get => m_smmTeachWait.CurStep; }
		public	Cmd			smmReference	{ get => m_smmReference.CurStep; }
		public	Cmd			smmCalibration	{ get => m_smmCalibration.CurStep; }

		private	int				m_nOffsetActAlarm	= 2;
		private	int				m_nOffsetSeqAlarm	= 0;

		private	int				m_nPartNo			= 0; // 0 = 1번 스테이지, 1 = 2번 스테이지
		private	int				m_nLaneNo			= 1;
		private	int				m_nCamera			= 0; // 0=Top, 1=Bottom, 2= Both
		private int				m_nInspCommWait;
		private int				m_nRetryCount		= 0;

		private	bool			m_IsGrip			= false;
		private	bool			m_bInspDone			= false;
		//private	bool			m_bReferenceStart	= false;
		//private	bool			m_bReferenceManual	= false;
		private	int				m_nHead				= 0;
		private	int				m_nMoveType			= 0; // 0 = Light Jig, 1 = 3D Jig , 2 = 1StageMroPos, 3= 2StageMropPos, 4  = CamMroPos)
		private	bool[]			m_bDIInspEnd		= new bool[2] {false, false};
		private	bool[]			m_bDIInspStart		= new bool[2] {false, false};
		private	string[]		m_strGroupName		= new string[2] {string.Empty, string.Empty};

		private	PStopwatch		m_DelayStopwatch	= new PStopwatch();
		private Random			m_ResultRandom		= new Random();
		private bool			m_IsMtHome			= true;
        #endregion

        #region 재정의
        private DI	DI_ExistSen1		= DI.InspPlate1ExistSen1;
        private DI	DI_ExistSen2		= DI.InspPlate1ExistSen2;
        private DI	DI_ExistSen3		= DI.InspPlate1ExistSen3;
        private DI	DI_ExistSen4		= DI.InspPlate1ExistSen4;
		
        private	DI	DI_TopInspEnd		= DI.TopInspEnd1;
		private	DI	DI_BtmInspEnd		= DI.BtmInspEnd1;

		private	DO	DO_TopInspReady		= DO.TopInspReady1;
		private	DO	DO_BtmInspReady		= DO.BtmInspReady1;

		private	MT	MT_GRIP				= MT.InspPlate1Grip;
		private	MT	MT_Y				= MT.InspPlate1Y;
		private	MT	MT_W				= MT.InspPlate1W;
		private	MT	MT_Z				= MT.InspPlate1Z;
		
        private	DI	DI_GripClose		= DI.InspPlate1GripCloseCyl;
		private	DO	DO_GripOpen			= DO.InspPlate1GripOpenCyl;
		private	DO	DO_GripClose		= DO.InspPlate1GripCloseCyl;

		private	FlowPart	FlowPlate	= FlowPart.Insp1;

		private	void	IODefine(int nNo)
		{
			if (nNo == 1)
            {
				DI_ExistSen1		= DI.InspPlate2ExistSen1;
				DI_ExistSen2		= DI.InspPlate2ExistSen2;
				DI_ExistSen3		= DI.InspPlate2ExistSen3;
				DI_ExistSen4		= DI.InspPlate2ExistSen4;
				
				DI_TopInspEnd		= DI.TopInspEnd2;
				DI_BtmInspEnd		= DI.BtmInspEnd2;
				
				DO_TopInspReady		= DO.TopInspReady2;
				DO_BtmInspReady		= DO.BtmInspReady2;
				
				MT_GRIP				= MT.InspPlate2Grip;
				MT_Y				= MT.InspPlate2Y;
				MT_W				= MT.InspPlate2W;
				MT_Z				= MT.InspPlate2Z;
				
				DI_GripClose		= DI.InspPlate2GripCloseCyl;
				DO_GripOpen			= DO.InspPlate2GripOpenCyl;
				DO_GripClose		= DO.InspPlate2GripCloseCyl;

				FlowPlate		= FlowPart.Insp2;
				m_nLaneNo		= 2;
			}
		}
		
		#endregion
		
		public	void		Init(int nPartNo)
		{
			try
			{
				m_smmHome			= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.INSPPLATE1_HOME+nPartNo));
				m_smmLoadWait		= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.INSPPLATE1_LOADWAIT+nPartNo));
				m_smmUnloadWait		= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.INSPPLATE1_UNLOADWAIT+nPartNo));
				m_smmInspWait		= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.INSPPLATE1_INSPWAIT+nPartNo));
				m_smmTeachWait		= new SMM<Cmd>(Cmd.START, string.Format("INSPPLATE{0} TEACHWAIT", nPartNo+1));
				m_smmReference		= new SMM<Cmd>(Cmd.START, string.Format("INSPPLATE{0} REFRENCE", nPartNo+1));
				m_smmCalibration	= new SMM<Cmd>(Cmd.START, string.Format("INSPPLATE{0}_CALIBRATION", nPartNo+1));

				lstSMM.Add(m_smmHome);
				lstSMM.Add(m_smmLoadWait);
				lstSMM.Add(m_smmUnloadWait);
				lstSMM.Add(m_smmInspWait);
				lstSMM.Add(m_smmTeachWait);
				lstSMM.Add(m_smmReference);
				lstSMM.Add(m_smmCalibration);

				m_nPartNo	= nPartNo;

				IODefine(nPartNo);

				#region - Motor
				arrMt		= new EnumArray<Axis, CMotor>(Axis.Max);
				
				arrMt[Axis.GRIP]	= cKernel.Motor(MT_GRIP);
				arrMt[Axis.W]		= cKernel.Motor(MT_W);
				arrMt[Axis.Y]		= cKernel.Motor(MT_Y);
				arrMt[Axis.Z]		= cKernel.Motor(MT_Z);
				#endregion

				#region - Actuator
				arrAct = new EnumArray<Act, Actuator>(Act.Max);

				arrAct[Act.Grip] = new CDoubleSen();
				arrAct[Act.Grip].SetSenser1(new List<DI>(new DI[] { DI_GripClose }));
				//arrAct[Act.Grip].SetSenser2(new List<DI>(new DI[] { DI_GripClose })); //현재 센서가 B접이라서 임시로 Open 센서 역할로 사용, 추후 A섭 센서 교체 후 위부분 주석 풀고 사용
				arrAct[Act.Grip].SetSol(DO_GripClose, DO_GripOpen);
				arrAct[Act.Grip].Setting(ALM.ERROR_INSP_PLATE1_GRIP_CYL_GRIP + m_nOffsetActAlarm, ALM.ERROR_INSP_PLATE1_GRIP_CYL_RELEASE + m_nOffsetActAlarm, ErrSenPos.Ori, SenPos.NONE, 400, 5000);
				
				#endregion

				if (NDm.Read(eNDM.TimeoutInspection) <= 0)			NDm.Write(eNDM.TimeoutInspection, 10000);

				base.Initialize();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public bool HomeStart(bool IsMtHome = true) { m_IsMtHome = IsMtHome; return SetSeqRun(ref m_smmHome); }

		public bool LoadWaitStart() { return SetSeqRun(ref m_smmLoadWait); }

		public bool UnloadWaitStart() { return SetSeqRun(ref m_smmUnloadWait); }

		public bool InspWaitStart() { return SetSeqRun(ref m_smmInspWait); }

		public bool TeachWaitStart() { return SetSeqRun(ref m_smmTeachWait); }

		public bool ReferenceStart(int nHead)
		{
			m_nHead = nHead;
			return SetSeqRun(ref m_smmReference);
		}
		// nHead 0: 1번헤드, 1:2번 헤드  nJigType 0: 조명, 1: 3D
		public bool CalibrationStart(int nHead, int nJigType)
		{
			m_nHead = nHead;
			m_nMoveType = nJigType;

			return SetSeqRun(ref m_smmCalibration);
		}

		private int CheckInspResultExist()
		{
			try
			{
				bool[] bexist = SM.FMP[FlowPlate].GetProdExist();

				for (int i = 0; i < SM.FMP[FlowPlate].GetMaxProdCount(); i++)
				{
					if (bexist[i] == true)
					{
						if (SM.FMP[FlowPlate].GetInfo().ProdInfo[i].ResultTop == (int)ProdStatus.Unknown) return 1;
						if (SM.FMP[FlowPlate].GetInfo().ProdInfo[i].ResultBtm == (int)ProdStatus.Unknown) return 2;
					}
				}

				return 0;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -1;
			}
		}



		public bool IsHomeReady() { return m_smmHome.IsReady; }

		public bool IsLoadWaitReady() { return m_smmLoadWait.IsReady; }

		public bool IsUnloadWaitReady() { return m_smmUnloadWait.IsReady; }

		public bool IsInspWaitReady() { return m_smmInspWait.IsReady; }

		public bool IsTeachWaitReady() { return m_smmTeachWait.IsReady; }

		public bool IsReferenceReady() { return m_smmReference.IsReady; }

		public bool IsCalibrationReady() { return m_smmCalibration.IsReady; }
		
		public bool InspDone
		{
			get => m_bInspDone;
			set => m_bInspDone = value;
		}
		public bool IsLDWait()
		{
			try
			{
				if (SM.SHM.IsUsePart(UsePartName.Insp1 + m_nPartNo, UsePartMode.Load) == false) return false;
				
				//Check : 위치 확인
				return m_smmLoadWait.CurStep == Cmd.LD_WAIT_50;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public bool IsULDWait()
		{
			try
			{
				if (SM.SHM.IsUsePart(UsePartName.Insp1 + m_nPartNo, UsePartMode.Unload) == false) return false;
				
				//Check : 위치 확인
				return m_smmUnloadWait.CurStep == Cmd.ULD_WAIT_40;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public GripState IsGrip()
		{
			try
			{
				if ( m_IsGrip == true)				return GripState.Grip;
				else if ((arrMt[Axis.GRIP].IsRdy((int)PosGrip.LOAD) == true || arrMt[Axis.GRIP].IsRdy((int)PosGrip.UNLOAD) == true) && m_IsGrip == false)		return GripState.Release;
				else																					return GripState.Unknown;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return GripState.Unknown;
			}
		}
		public bool IsGrabPosition()
		{
			try
			{
				if (SM.FMP.IsFlow(FlowPlate) == false)															return false;
				if (arrMt[Axis.Y].IsRdyPos(arrMt[Axis.Y].GetIndexPos((int)PosY.INSP)) == false)			return false;
				if (arrMt[Axis.GRIP].IsRdyPos(arrMt[Axis.GRIP].GetIndexPos((int)PosGrip.GRIP)) == false)	return false;

				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}
		public bool IsUnloadPosition()
		{
			try
			{
				if (SM.FMP.IsFlow(FlowPlate) == false)														return false;
				if (arrMt[Axis.Y].IsRdyPos(arrMt[Axis.Y].GetIndexPos((int)PosY.UNLOAD)) == false)		return false;

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
				FlowPart flowPart = FlowPlate;
				DI ExistSenStart = DI_ExistSen1;

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

		private void InterlockCheckMove(PosY TargetPos)
		{
			try
			{
				//1 = + 방향 , -1 = - 방향
				//int nDir = -1;
				//if (arrMt[Axis.Y].GetIndexPos((int)xPos) > arrMt[Axis.X].GetLocalPos()) nDir = 1;


				if (arrMt[Axis.Z].IsRdyPos(arrMt[Axis.Z].GetIndexPos((int)PosZ.READY)) == false &&
					arrMt[Axis.Z].IsRdyPos(0) == false)
				{
					return;
				}

				if (arrMt[Axis.Y].IsRdy((int)TargetPos) == false)
				{
					arrMt[Axis.Y].MoveIndex((int)TargetPos);
				}

			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
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



		/// <summary>
		/// Run Process
		/// </summary>
		public void Run()
		{
			try
			{
				UpdateAct(Seq.InspPlate1 + m_nPartNo);
				
				if (SM.SHM.IsBitData(BIT.Error) == true || SM.SHM.IsBitData(BIT.Pause) == true)
				{
					if (IsInspWaitReady() == false) m_smmInspWait.ResetStep();
					if (SM.SHM.IsBitData(BIT.InspLock) == true) SM.SHM.SetBitData(BIT.InspLock, false);

					return;
				}
                m_nInspCommWait = NDm.Read(eNDM.InspCommTimeOut);
				Home(m_smmHome);
				LoadWait(m_smmLoadWait);
				UnloadWait(m_smmUnloadWait);
				InspWait(m_smmInspWait);
				TeachingWait(m_smmTeachWait);
				Reference(m_smmReference);
				Calibration(m_smmCalibration);

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
                            SM.SHM.SetBitData(BIT.InspLock, false);
                            G.cKernel.AOut(DO_TopInspReady, false);
                            G.cKernel.AOut(DO_BtmInspReady, false);
                            m_bInspDone = false;

                            smm.Next(Cmd.HOME_11);
                        }
                        break;

                    case Cmd.HOME_11:
                        {
                            if (SM.FMP.IsFlow(FlowPart.Insp1 + m_nPartNo) == true)
                            {
                                smm.Next(Cmd.HOME_12);
                                break;
                            }


                            smm.Next(Cmd.HOME_20);
                        }
                        break;

                    case Cmd.HOME_12:
                        {
                            if (arrMt[Axis.GRIP].IsHomeDone() == false)
                            {
                                if (m_nPartNo == 0)
                                {
                                    AddAlarm(ALM.ERROR_MT_HOME_INSP_PLATE1_GRIP);
                                }
                                else
                                {
                                    AddAlarm(ALM.ERROR_MT_HOME_INSP_PLATE2_GRIP);
                                }

                                smm.Next(Cmd.END);
                                break;
                            }

                            smm.Next(Cmd.HOME_13);
                        }
                        break;

                    case Cmd.HOME_13:
                        {
                            if (arrMt[Axis.GRIP].IsRdy((int)PosGrip.GRIP) == false)
                            {
                                arrMt[Axis.GRIP].MoveIndex((int)PosGrip.GRIP);
                                break;
                            }

                            if (arrAct[Act.Grip].IsPos(SenPos.TABLE_GRIP) == false)
                            {
                                arrAct[Act.Grip].SetPos(SenPos.TABLE_GRIP);
                                break;
                            }


                            smm.Next(Cmd.HOME_20);
                        }
                        break;

                    case Cmd.HOME_20:
						{
							if (m_IsMtHome == false)
							{
								smm.Next(Cmd.HOME_50);
								break;
							}
							
							arrMt[Axis.Z].ClearHome();
							arrMt[Axis.Z].StartHoming();

							smm.Next(Cmd.HOME_30);
						}
						break;

					case Cmd.HOME_30:
						{
							if (arrMt[Axis.Z].IsHomeDone() == false || arrMt[Axis.Z].IsRdy() == false) break;
							
							arrMt[Axis.W].ClearHome();
							arrMt[Axis.W].StartHoming();
							arrMt[Axis.Y].ClearHome();
							arrMt[Axis.Y].StartHoming();

							smm.Next(Cmd.HOME_40);
						}
						break;

					case Cmd.HOME_40:
						{
							if (arrMt[Axis.W].IsHomeDone() == false || arrMt[Axis.W].IsRdy() == false) break;
							if (arrMt[Axis.Y].IsHomeDone() == false || arrMt[Axis.Y].IsRdy() == false) break;
							
							smm.Next(Cmd.HOME_50);
						}
						break;
						
					case Cmd.HOME_50:
						{
							if (arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false ||
								arrMt[Axis.W].IsRdy((int)PosW.LOAD) == false)
							{
								arrMt[Axis.Z].MoveIndex((int)PosZ.READY);
								arrMt[Axis.W].MoveIndex((int)PosW.LOAD);
								break;
							}

							smm.Next(Cmd.HOME_60);
						}
						break;
						
					case Cmd.HOME_60:
						{
							if (arrMt[Axis.Y].IsRdy((int)PosY.LOAD) == false )
							{
								InterlockCheckMove(PosY.LOAD);
								break;
							}
							if (arrMt[Axis.Z].IsRdy((int)PosZ.LOAD) == false)
							{
								arrMt[Axis.Z].MoveIndex((int)PosZ.LOAD);
								break;
							}

							smm.Next(Cmd.HOME_70);
						}
						break;
						
					case Cmd.HOME_70:
						{
							switch (CheckProdExist())
							{
								case ProdExist.Mismatch:
									AddAlarm(ALM.ERROR_SEQ_INSP_PLATE1_RAM_MISMATCH + m_nPartNo);
									break;
								default :
									smm.Next(Cmd.HOME_80);
									break;
							}
						}
						break;
						
					case Cmd.HOME_80:
						{
							if (m_IsMtHome == false  || 
								SM.FMP.IsFlow(FlowPlate) == true)
							{
								smm.Next(Cmd.HOME_100);
								break;
							}
							
							arrMt[Axis.GRIP].ClearHome();
							arrMt[Axis.GRIP].StartHoming();
							arrAct[Act.Grip].SetPos(SenPos.TABLE_RELEASE);

							smm.Next(Cmd.HOME_90);
						}
						break;
						
					case Cmd.HOME_90:
						{
							if (arrMt[Axis.GRIP].IsHomeDone() == false || arrMt[Axis.GRIP].IsRdy() == false)	break;
							if (arrAct[Act.Grip].IsPos(SenPos.TABLE_RELEASE) == false)
							{
								arrAct[Act.Grip].SetPos(SenPos.TABLE_RELEASE);
							}

							smm.Next(Cmd.END);
						}
						break;
						
					case Cmd.HOME_100:
						{
							if (arrMt[Axis.GRIP].IsHomeDone() == false)
							{
								if (m_nPartNo == 0)		AddAlarm(ALM.ERROR_MT_HOME_INSP_PLATE1_GRIP);
								else					AddAlarm(ALM.ERROR_MT_HOME_INSP_PLATE2_GRIP);
								
								break;
							}

							if (SM.FMP.IsFlow(FlowPlate) == true)
							{
								if (arrMt[Axis.GRIP].IsRdy((int)PosGrip.GRIP) == false)
								{
									arrMt[Axis.GRIP].MoveIndex((int)PosGrip.GRIP);
									break;
								}

								if (arrAct[Act.Grip].IsPos(SenPos.TABLE_GRIP) == false)
								{
									arrAct[Act.Grip].SetPos(SenPos.TABLE_GRIP);
									break;
								}
							}
							else
							{
								if (arrMt[Axis.GRIP].IsRdy((int)PosGrip.READY) == false)
								{
									arrMt[Axis.GRIP].MoveIndex((int)PosGrip.READY);
									break;
								}

								if (arrAct[Act.Grip].IsPos(SenPos.TABLE_RELEASE) == false)
								{
									arrAct[Act.Grip].SetPos(SenPos.TABLE_RELEASE);
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

		public void LoadWait(SMM<Cmd> smm)
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

								smm.Next(Cmd.LD_WAIT_10);
							}
						}
						break;

					case Cmd.LD_WAIT_10:      // RAM 유무 확인
						{
							if (arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false ||
								arrMt[Axis.W].IsRdy((int)PosW.LOAD) == false)
							{
								arrMt[Axis.Z].MoveIndex((int)PosZ.READY);
								arrMt[Axis.W].MoveIndex((int)PosW.LOAD);
								break;
							}

							smm.Next(Cmd.LD_WAIT_20);
						}
						break;
						

					case Cmd.LD_WAIT_20:
						{
							if (arrMt[Axis.Y].IsRdy((int)PosY.LOAD) == false )
							{
								InterlockCheckMove(PosY.LOAD);
								break;
							}

							if (arrMt[Axis.Z].IsRdy((int)PosZ.LOAD) == false)
							{
								arrMt[Axis.Z].MoveIndex((int)PosZ.LOAD);
                                break;
							}

							smm.Next(Cmd.LD_WAIT_30);
						}
						break;


					case Cmd.LD_WAIT_30:      // Grip 해제, Load 위치 이동, Unlock
						{
							switch (CheckProdExist())
							{
								case ProdExist.Exist:
									if (m_nPartNo == 0)		AddAlarm(ALM.ERROR_SEQ_INSP_PLATE1_RAM_EXIST);
									else					AddAlarm(ALM.ERROR_SEQ_INSP_PLATE2_RAM_EXIST);
									break;
								case ProdExist.Mismatch:
									if (m_nPartNo == 0)		AddAlarm(ALM.ERROR_SEQ_INSP_PLATE1_RAM_MISMATCH);
									else					AddAlarm(ALM.ERROR_SEQ_INSP_PLATE2_RAM_MISMATCH);
									break;
								case ProdExist.None:
									m_IsGrip = false;
									m_bInspDone = false;
									smm.Next(Cmd.LD_WAIT_40);
									break;
							}
						}
						break;

					case Cmd.LD_WAIT_40:      // Grip 해제, Load 위치 이동, Unlock
						{
							if (arrMt[Axis.GRIP].IsRdy((int)PosGrip.LOAD) == false)
							{
								arrMt[Axis.GRIP].MoveIndex((int)PosGrip.LOAD);
								break;
							}
							
							if (arrAct[Act.Grip].IsPos(SenPos.TABLE_RELEASE) == false)
							{
								arrAct[Act.Grip].SetPos(SenPos.TABLE_RELEASE);
								break;
							}

							smm.Next(Cmd.LD_WAIT_50);
						}
						break;

					case Cmd.LD_WAIT_50:
						{
							if (G.seqLoadInspPnP.IsReadyPlacePos(m_nPartNo) == false) break;

							smm.Next(Cmd.LD_WAIT_60);
						}
						break;

					case Cmd.LD_WAIT_60:
						{
							m_IsGrip = true;

							smm.Next(Cmd.LD_WAIT_70);
						}
						break;

					case Cmd.LD_WAIT_70:
						{
							if (SM.FMP.IsFlow(FlowPlate) == false) break;

							smm.Next(Cmd.LD_WAIT_80);
						}
						break;

					case Cmd.LD_WAIT_80:        // LoadInspPnp cyl Up 완료 대기
                        {
							if (G.seqLoadInspPnP.CheckCylUpDown(SenPos.PICKER_UP) == false) break;

                            if (arrMt[Axis.W].IsRdy((int)PosW.GRIP) == false)
                            {
                                arrMt[Axis.W].MoveIndex((int)PosW.GRIP);
                                break;
                            }

                            if (arrMt[Axis.GRIP].IsRdy((int)PosGrip.GRIP) == false)
                            {
                                arrMt[Axis.GRIP].MoveIndex((int)PosGrip.GRIP);
                                break;
                            }
                            
                            if (arrAct[Act.Grip].IsPos(SenPos.TABLE_GRIP) == false)
                            {
                                arrAct[Act.Grip].SetPos(SenPos.TABLE_GRIP);
                                break;
                            }


                            smm.Next(Cmd.LD_WAIT_90);
						
						}
						break;

					case Cmd.LD_WAIT_90:    // RAM 유무 감지
                        {
							switch (CheckProdExist())
							{
								case ProdExist.Exist:
									smm.Next(Cmd.END);
									break;
								case ProdExist.Mismatch:
									if (m_nPartNo == 0)		AddAlarm(ALM.ERROR_SEQ_INSP_PLATE1_RAM_MISMATCH);
									else					AddAlarm(ALM.ERROR_SEQ_INSP_PLATE2_RAM_MISMATCH);
									break;
								case ProdExist.None:
									if (m_nPartNo == 0)		AddAlarm(ALM.ERROR_SEQ_INSP_PLATE1_RAM_NONE);
									else					AddAlarm(ALM.ERROR_SEQ_INSP_PLATE2_RAM_NONE);
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

		public void UnloadWait(SMM<Cmd> smm)
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

								smm.Next(Cmd.ULD_WAIT_10);
							}
						}
						break;

					case Cmd.ULD_WAIT_10:     // 제품 유무 확인
						{
							G.cKernel.AOut(DO_TopInspReady, false);
							G.cKernel.AOut(DO_BtmInspReady, false);
							
							if (arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false ||
								arrMt[Axis.W].IsRdy((int)PosW.UNLOAD) == false)
							{
								arrMt[Axis.Z].MoveIndex((int)PosZ.READY);
								arrMt[Axis.W].MoveIndex((int)PosW.UNLOAD);
								break;
							}
								
							smm.Next(Cmd.ULD_WAIT_20);
						}
						break;

					case Cmd.ULD_WAIT_20:     // 제품 유무 확인
						{
							if (arrMt[Axis.Y].IsRdy((int)PosY.UNLOAD) == false )
							{
								InterlockCheckMove(PosY.UNLOAD);
								break;
							}
							if (arrMt[Axis.Z].IsRdy((int)PosZ.UNLOAD) == false)
							{
								arrMt[Axis.Z].MoveIndex((int)PosZ.UNLOAD);
								break;
							}

							smm.Next(Cmd.ULD_WAIT_30);
						}
						break;
						

					case Cmd.ULD_WAIT_30:
						{
							switch (CheckProdExist())
							{
								case ProdExist.Exist:
									m_IsGrip = true;
									smm.Next(Cmd.ULD_WAIT_40);
									break;
								case ProdExist.Mismatch:
									if (m_nPartNo == 0)		AddAlarm(ALM.ERROR_SEQ_INSP_PLATE1_RAM_MISMATCH);
									else					AddAlarm(ALM.ERROR_SEQ_INSP_PLATE2_RAM_MISMATCH);
									break;
								case ProdExist.None:
									if (m_nPartNo == 0)		AddAlarm(ALM.ERROR_SEQ_INSP_PLATE1_RAM_NONE);
									else					AddAlarm(ALM.ERROR_SEQ_INSP_PLATE2_RAM_NONE);
									break;
							}
						}
						break;
						
					case Cmd.ULD_WAIT_40:
						{
							if (SM.FMP.IsFlow(FlowPlate) == true) break;

							smm.Next(Cmd.ULD_WAIT_50);
						}
						break;
					case Cmd.ULD_WAIT_50:
						{
							if (arrMt[Axis.GRIP].IsHomeDone() == false)
							{
								if (m_nPartNo == 0)		AddAlarm(ALM.ERROR_MT_HOME_INSP_PLATE1_GRIP);
								else					AddAlarm(ALM.ERROR_MT_HOME_INSP_PLATE2_GRIP);
								break;
							}

							
							if (arrMt[Axis.GRIP].IsRdy((int)PosGrip.UNLOAD) == false)
							{
								arrMt[Axis.GRIP].MoveIndex((int)PosGrip.UNLOAD);
								break;
							}
							
							if (arrAct[Act.Grip].IsPos(SenPos.TABLE_RELEASE) == false)
							{
								arrAct[Act.Grip].SetPos(SenPos.TABLE_RELEASE);
								break;
							}

							m_IsGrip = false;
							m_bInspDone = false;

							smm.Next(Cmd.ULD_WAIT_60);
						}
						break;

					case Cmd.ULD_WAIT_60:     // UnloadInspPnp Pick 완료 대기
						{
							if (G.seqUnloadInspPnP.CheckCylUpDown(SenPos.PICKER_UP) == false) break;

							smm.Next(Cmd.ULD_WAIT_70);
						}
						break;

					case Cmd.ULD_WAIT_70:     // UnloadInspPnp Pick 완료 대기
						{
							switch (CheckProdExist())
							{
								case ProdExist.Exist:
									if (m_nPartNo == 0) AddAlarm(ALM.ERROR_SEQ_INSP_PLATE1_RAM_EXIST);
									else AddAlarm(ALM.ERROR_SEQ_INSP_PLATE2_RAM_EXIST);
									break;
								case ProdExist.Mismatch:
									if (m_nPartNo == 0) AddAlarm(ALM.ERROR_SEQ_INSP_PLATE1_RAM_MISMATCH);
									else AddAlarm(ALM.ERROR_SEQ_INSP_PLATE2_RAM_MISMATCH);
									break;
								default:
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

		public void InspWait(SMM<Cmd> smm)
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

								smm.Next(Cmd.INSP_WAIT_01);
							}
						}
						break;

					case Cmd.INSP_WAIT_01:        // RAM 유무 확인
						{
                            if (arrMt[Axis.Y].IsRdy((int)PosY.LOAD) == false)
                            {
                                if (arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false)
                                {
                                    arrMt[Axis.Z].MoveIndex((int)PosZ.READY);
                                    break;
                                }
                            }
                            if (G.cKernel.IsAOut(DO_TopInspReady) || G.cKernel.IsAOut(DO_BtmInspReady))
							{
								G.cKernel.AOut(DO_TopInspReady, false);
								G.cKernel.AOut(DO_BtmInspReady, false);
							}
							
							m_bInspDone = false;

                            smm.Next(Cmd.INSP_WAIT_02);
						}
						break;

					case Cmd.INSP_WAIT_02:        // RAM 유무 확인
						{
							if (arrMt[Axis.Y].IsRdy((int)PosY.LOAD) == false )
							{
								InterlockCheckMove(PosY.LOAD);
								break;
							}

                            if (arrMt[Axis.W].IsRdy((int)PosW.UNLOAD) == false  ||
                                arrMt[Axis.Z].IsRdy((int)PosZ.LOAD) == false)
                            {
                                arrMt[Axis.W].MoveIndex((int)PosW.UNLOAD);
                                arrMt[Axis.Z].MoveIndex((int)PosZ.LOAD);
                                break;
                            }
                            smm.Next(Cmd.INSP_WAIT_10);
						}
						break;

					case Cmd.INSP_WAIT_10:        // RAM 유무 확인
						{
							switch (CheckProdExist())
							{
								case ProdExist.Exist:
									m_IsGrip = true;
									var tcinfo = SM.FMP[FlowPlate].GetInfo();
									for (int i = 0; i < SM.FMP[FlowPlate].GetMaxProdCount(); i++)
									{
										tcinfo.ProdInfo[i].ResultTop = (int)ProdStatus.Unknown;
										tcinfo.ProdInfo[i].ResultBtm = (int)ProdStatus.Unknown;
									}
									SM.FMP[FlowPlate].SetInfo(tcinfo);
									smm.Next(Cmd.INSP_WAIT_20);
									break;
								case ProdExist.Mismatch:
									AddAlarm(ALM.ERROR_SEQ_INSP_PLATE1_RAM_MISMATCH + m_nPartNo);
									break;
								case ProdExist.None:
									AddAlarm(ALM.ERROR_SEQ_INSP_PLATE1_RAM_NONE + m_nPartNo);
									break;
							}
						}
						break;

					case Cmd.INSP_WAIT_20:        // INSP 위치 이동, Sol on
						{
							if (NDm.Read(eNDM.AutoRunMode) == (int)AutoRunMode.QuitLot)
							{
								stProdInfo[] info = SM.FMP[FlowPlate].GetInfo().ProdInfo;
								string[] strProdID = new string[4] { info[0].ProdID, info[1].ProdID, info[2].ProdID, info[3].ProdID};

								for (int i = 0; i < SM.FMP[FlowPlate].GetMaxProdCount(); i++)
								{
									if (info[i].Exist == false) continue;

									SM.FMP.SetInspResult(FlowMngPart.teDir.Top, strProdID[i], ProdStatus.NG, "SkipError");
									SM.FMP.SetInspResult(FlowMngPart.teDir.Btm, strProdID[i], ProdStatus.NG, "SkipError");
								}

								m_bInspDone = true;
								smm.Next(Cmd.END);
								break;
							}


                            if (arrMt[Axis.W].IsRdy((int)PosW.GRIP) == false)
                            {
                                arrMt[Axis.W].MoveIndex((int)PosW.GRIP);
                                break;
                            }
                            if (arrMt[Axis.GRIP].IsRdy((int)PosGrip.GRIP) == false)
                            {
                                arrMt[Axis.GRIP].MoveIndex((int)PosGrip.GRIP);
                                break;
                            }
                            if (arrAct[Act.Grip].IsPos(SenPos.TABLE_GRIP) == false)
							{
								arrAct[Act.Grip].SetPos(SenPos.TABLE_GRIP);
								break;
							}

							smm.Next(Cmd.INSP_WAIT_31);
						}
						break;

                    case Cmd.INSP_WAIT_31:
                        {
                            if (smm.Delayms(200) == false) break;

                            if (arrMt[Axis.W].IsRdy((int)PosW.UNLOAD) == false ||
                                arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false)
                            {
                                arrMt[Axis.W].MoveIndex((int)PosW.UNLOAD);
                                arrMt[Axis.Z].MoveIndex((int)PosZ.READY);
                                break;
                            }

                            InterlockCheckMove(PosY.INSP);
                            smm.Next(Cmd.INSP_WAIT_40);
                        }
                        break;


                    case Cmd.INSP_WAIT_30:
						{
							if (arrMt[Axis.Y].IsRdy((int)PosY.INSP) == false)
							{
								InterlockCheckMove(PosY.INSP);
								break;
							}

							smm.Next(Cmd.INSP_WAIT_40);
						}
						break;


					case Cmd.INSP_WAIT_40:        // 검사 PC Run 상태 요청
						{
							if (SM.SHM.IsBitData(BIT.InspLock) == true) break;

							SM.SHM.SetBitData(BIT.InspLock, true);

							if (SM.SHM.GetUseSkipData(USESKIP.TopInspection) == true)
							{
								G.commInspTop.SendRunStateRequest();
							}
							if (SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == true)
							{
								G.commInspBtm.SendRunStateRequest();
							}
							smm.Next(Cmd.INSP_WAIT_50);
						}
						break;

					case Cmd.INSP_WAIT_50:        // 검사 PC Run 상태 확인 Top
						{
							if (SM.SHM.GetUseSkipData(USESKIP.TopInspection) == false && SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == false)
							{
								if (smm.Delayms(NDm.Read(eNDM.InspSkipInspDelay)) == false) break;
							}

							if (SM.SHM.GetUseSkipData(USESKIP.TopInspection) == false)
							{
								smm.Next(Cmd.INSP_WAIT_60);
								break;
							}

							CommInsp.RunState state = CommInsp.RunState.Idle;
							int nret = G.commInspTop.RcvRunStateReport(ref state);
							if (nret < 0)
							{
								smm.Next(Cmd.END);
							}
							else if (nret > 0)
							{
								if (state == CommInsp.RunState.Run)
								{
									smm.Next(Cmd.INSP_WAIT_60);
								}
								else
								{
									AddAlarm(ALM.ERROR_SEQ_INSP_TOP_PC_NOT_AUTORUN);
									smm.Next(Cmd.END);
								}
							}
							else if (smm.Delayms(m_nInspCommWait) == true)
							{
								AddAlarm(ALM.ERROR_SEQ_INSP_TOP_PC_AUTORUN_COMM_TIMEOUT);
								smm.Next(Cmd.END);
							}
						}
						break;
					case Cmd.INSP_WAIT_60:        // 검사 PC Run 상태 확인 Btm
						{
							if (SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == false)
							{
								smm.Next(Cmd.INSP_WAIT_70);
								break;
							}

							CommInsp.RunState state = CommInsp.RunState.Idle;
							int nret = G.commInspBtm.RcvRunStateReport(ref state);
							if (nret < 0)
							{
								smm.Next(Cmd.END);
							}
							else if (nret > 0)
							{
								if (state == CommInsp.RunState.Run)
								{
									smm.Next(Cmd.INSP_WAIT_70);
								}
								else
								{
									AddAlarm(ALM.ERROR_SEQ_INSP_BTM_PC_NOT_AUTORUN);
									smm.Next(Cmd.END);
								}
							}
							else if (smm.Delayms(m_nInspCommWait) == true)
							{
								AddAlarm(ALM.ERROR_SEQ_INSP_BTM_PC_AUTORUN_COMM_TIMEOUT);
								smm.Next(Cmd.END);
							}
						}
						break;
					case Cmd.INSP_WAIT_70:        // 검사 진행 상태 요청
						{
							if (SM.SHM.GetUseSkipData(USESKIP.TopInspection) == true)
							{
								G.commInspTop.SendInspStateRequest();
							}
							if (SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == true)
							{
								G.commInspBtm.SendInspStateRequest();
							}

							smm.Next(Cmd.INSP_WAIT_80);
						}
						break;

					case Cmd.INSP_WAIT_80:        // 검사 진행 상태 확인 Top
						{
							if (SM.SHM.GetUseSkipData(USESKIP.TopInspection) == false)
							{
								smm.Next(Cmd.INSP_WAIT_90);
								break;
							}
							if (smm.Delayms(100) == false)
							{
								break;
							}

							CommInsp.InspState state = CommInsp.InspState.Idle;
							int nret = G.commInspTop.RcvInspStateReport(ref state);
							if (nret < 0)
							{
								smm.Next(Cmd.END);
							}
							else if (nret > 0)
							{
								if (state == CommInsp.InspState.Idle)
								{
									smm.Next(Cmd.INSP_WAIT_90);
								}
								else
								{
									smm.Next(Cmd.RETRY_DELEAY_10);
								}
							}
							if (smm.Delayms(m_nInspCommWait) == true)
							{
								AddAlarm(ALM.ERROR_SEQ_INSP_TOP_PC_INSPSTATE_COMM_TIMEOUT);
								smm.Next(Cmd.END);
							}
						}
						break;

					case Cmd.INSP_WAIT_90:        // 검사 진행 상태 확인 Btm
						{
							if (SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == false)
							{
								smm.Next(Cmd.INSP_WAIT_90_JobUpdateCheck1);
								break;
							}

							CommInsp.InspState state = CommInsp.InspState.Idle;
							int nret = G.commInspBtm.RcvInspStateReport(ref state);
							if (nret < 0)
							{
								smm.Next(Cmd.END);
							}
							else if (nret > 0)
							{
								if (state == CommInsp.InspState.Idle)
								{
									smm.Next(Cmd.INSP_WAIT_90_JobUpdateCheck1);
								}
								else
								{
									smm.Next(Cmd.RETRY_DELEAY_10);
								}
							}
							else if (smm.Delayms(m_nInspCommWait) == true)
							{
								AddAlarm(ALM.ERROR_SEQ_INSP_BTM_PC_INSPSTATE_COMM_TIMEOUT);
								smm.Next(Cmd.END);
							}
						}
						break;

					case Cmd.RETRY_DELEAY_10:
						{
							if (smm.Delayms(500) == false) break;

							smm.Next(Cmd.INSP_WAIT_70);
						}
						break;


					case Cmd.INSP_WAIT_90_JobUpdateCheck1:    //상부 잡 업데이트 유/무 요청
						{
							if (SM.SHM.GetUseSkipData(USESKIP.InspOfflineJobCheck) == false ||
								SM.SHM.GetUseSkipData(USESKIP.TopInspection) == false)
							{
								//바텀 확인
								smm.Next(Cmd.INSP_WAIT_90_JobUpdateCheck3);
								break;
							}

							G.commInspTop.SendJobUpdateDataRequest();

							smm.Next(Cmd.INSP_WAIT_90_JobUpdateCheck2);
						}
						break;


					case Cmd.INSP_WAIT_90_JobUpdateCheck2:    //상부 잡 업데이트 유/무 확인
						{
							if (SM.SHM.GetUseSkipData(USESKIP.InspOfflineJobCheck) == false ||
								SM.SHM.GetUseSkipData(USESKIP.TopInspection) == false)
							{
								//바텀 확인
								smm.Next(Cmd.INSP_WAIT_90_JobUpdateCheck3);
								break;
							}

							bool bResult = false;
							int nret = G.commInspTop.RcvJobUpdateDataReport(ref bResult);
							if (nret < 0)
							{
								smm.Next(Cmd.END);
							}
							else if (nret > 0)
							{
								if (bResult == true) //잡 업데이트
								{
									smm.Next(Cmd.INSP_WAIT_90_JobUpdateCheck5);
								}
								else //바텀 확인
								{
									smm.Next(Cmd.INSP_WAIT_90_JobUpdateCheck3);
								}
							}
							else if (smm.Delayms(m_nInspCommWait) == true)
							{
								AddAlarm(ALM.ERROR_SEQ_INSP_TOP_PC_COMM_TIMEOUT);
								smm.Next(Cmd.END);
							}
						}
						break;


					case Cmd.INSP_WAIT_90_JobUpdateCheck3:    //하부 잡 업데이트 유/무 요청
						{
							if (SM.SHM.GetUseSkipData(USESKIP.InspOfflineJobCheck) == false ||
								SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == false)
							{
								smm.Next(Cmd.INSP_WAIT_100);
								break;
							}

							G.commInspBtm.SendJobUpdateDataRequest();

							smm.Next(Cmd.INSP_WAIT_90_JobUpdateCheck4);
						}
						break;


					case Cmd.INSP_WAIT_90_JobUpdateCheck4:    //하부 잡 업데이트 유/무 확인
						{
							if (SM.SHM.GetUseSkipData(USESKIP.InspOfflineJobCheck) == false ||
								SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == false)
							{
								smm.Next(Cmd.INSP_WAIT_100);
								break;
							}

							bool bResult = false;
							int nret = G.commInspBtm.RcvJobUpdateDataReport(ref bResult);
							if (nret < 0)
							{
								smm.Next(Cmd.END);
							}
							else if (nret > 0)
							{
								if (bResult == true) //잡 업데이트
								{
									smm.Next(Cmd.INSP_WAIT_90_JobUpdateCheck5);
								}
								else
								{
									smm.Next(Cmd.INSP_WAIT_100);
								}
							}
							else if (smm.Delayms(m_nInspCommWait) == true)
							{
								AddAlarm(ALM.ERROR_SEQ_INSP_BTM_PC_COMM_TIMEOUT);
								smm.Next(Cmd.END);
							}
						}
						break;


					case Cmd.INSP_WAIT_90_JobUpdateCheck5:    //상부 잡 업데이트 명령
						{
							if (SM.SHM.GetUseSkipData(USESKIP.InspOfflineJobCheck) == false ||
								SM.SHM.GetUseSkipData(USESKIP.TopInspection) == false)
							{
								smm.Next(Cmd.INSP_WAIT_90_JobUpdateCheck7);
								break;
							}

							G.commInspTop.SendJobUpdateRequest();

							smm.Next(Cmd.INSP_WAIT_90_JobUpdateCheck6);
						}
						break;


					case Cmd.INSP_WAIT_90_JobUpdateCheck6:    //상부 잡 업데이트 완료 확인
						{
							if (SM.SHM.GetUseSkipData(USESKIP.InspOfflineJobCheck) == false ||
								SM.SHM.GetUseSkipData(USESKIP.TopInspection) == false)
							{
								smm.Next(Cmd.INSP_WAIT_90_JobUpdateCheck7);
								break;
							}

							bool bResult = false;
							int nret = G.commInspTop.RcvJobUpdateReport(ref bResult);
							if (nret < 0)
							{
								smm.Next(Cmd.END);
							}
							else if (nret > 0)
							{
								if (bResult == true) //잡 업데이트 성공
								{
									smm.Next(Cmd.INSP_WAIT_90_JobUpdateCheck7);
								}
								else //실패
								{
									AddAlarm(ALM.ERROR_COMM_TOP_JOB_UPDATE_FAIL);
									smm.Next(Cmd.END);
								}
							}
							else if (smm.Delayms(m_nInspCommWait) == true)
							{
								AddAlarm(ALM.ERROR_SEQ_INSP_TOP_PC_COMM_TIMEOUT);
								smm.Next(Cmd.END);
							}
						}
						break;


					case Cmd.INSP_WAIT_90_JobUpdateCheck7:    //하부 잡 업데이트 명령
						{
							if (SM.SHM.GetUseSkipData(USESKIP.InspOfflineJobCheck) == false ||
								SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == false)
							{
								smm.Next(Cmd.INSP_WAIT_100);
								break;
							}

							G.commInspBtm.SendJobUpdateRequest();

							smm.Next(Cmd.INSP_WAIT_90_JobUpdateCheck8);
						}
						break;


					case Cmd.INSP_WAIT_90_JobUpdateCheck8:    //하부 잡 업데이트 완료 확인
						{
							if (SM.SHM.GetUseSkipData(USESKIP.InspOfflineJobCheck) == false ||
								SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == false)
							{
								smm.Next(Cmd.INSP_WAIT_100);
								break;
							}

							bool bResult = false;
							int nret = G.commInspBtm.RcvJobUpdateReport(ref bResult);
							if (nret < 0)
							{
								smm.Next(Cmd.END);
							}
							else if (nret > 0)
							{
								if (bResult == true) //잡 업데이트 성공
								{
									smm.Next(Cmd.INSP_WAIT_100);
								}
								else //실패
								{
									AddAlarm(ALM.ERROR_COMM_BOTTOM_JOB_UPDATE_FAIL);
									smm.Next(Cmd.END);
								}
							}
							else if (smm.Delayms(m_nInspCommWait) == true)
							{
								AddAlarm(ALM.ERROR_SEQ_INSP_BTM_PC_COMM_TIMEOUT);
								smm.Next(Cmd.END);
							}
						}
						break;


					case Cmd.INSP_WAIT_100:       // RAM 제품 ID 보고
						{

							stProdInfo[] info = SM.FMP[FlowPlate].GetInfo().ProdInfo;
							string[] strProdID = new string[info.Length];
							for (int i = 0; i < info.Length; i++)
							{
								strProdID[i] = info[i].ProdID;
							}

							if (SM.SHM.GetUseSkipData(USESKIP.TopInspection) == true)
							{
								for (int i = 0; i < info.Length; i++)
								{
									if (info[i].Exist == false) continue;

									G.commInspTop.SendProdDataReport(m_nLaneNo, SM.FMP[FlowPlate].GetInfo());
									break;
								}
							}
							else
							{
								for (int i = 0; i < SM.FMP[FlowPlate].GetMaxProdCount(); i++)
								{
									if (SM.SHM.GetUseSkipData(USESKIP.InspSkipResultNG) == true)
									{
										SM.FMP.SetInspResult(FlowMngPart.teDir.Top, strProdID[i], ProdStatus.NG, "error");
									}
									else if (SM.SHM.GetUseSkipData(USESKIP.InspSkipResultRandom) == true)
									{
										if (m_ResultRandom.Next(20) == 1) SM.FMP.SetInspResult(FlowMngPart.teDir.Top, strProdID[i], ProdStatus.NG, "Error");
										else SM.FMP.SetInspResult(FlowMngPart.teDir.Top, strProdID[i], ProdStatus.OK, "Error");
									}
									else
									{
										SM.FMP.SetInspResult(FlowMngPart.teDir.Top, strProdID[i], ProdStatus.OK, "error");
									}
								}
							}
							if (SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == true)
							{
								for (int i = 0; i < info.Length; i++)
								{
									if (info[i].Exist == false) continue;

									G.commInspBtm.SendProdDataReport(m_nLaneNo, SM.FMP[FlowPlate].GetInfo());
									break;
								}
							}
							else
							{
								for (int i = 0; i < SM.FMP[FlowPlate].GetMaxProdCount(); i++)
								{
									if (SM.SHM.GetUseSkipData(USESKIP.InspSkipResultNG) == true)
									{
										SM.FMP.SetInspResult(FlowMngPart.teDir.Btm, strProdID[i], ProdStatus.NG, "error");
									}
									else if (SM.SHM.GetUseSkipData(USESKIP.InspSkipResultRandom) == true)
									{
										if (m_ResultRandom.Next(10) == 1) SM.FMP.SetInspResult(FlowMngPart.teDir.Btm, strProdID[i], ProdStatus.NG, "error");
										else SM.FMP.SetInspResult(FlowMngPart.teDir.Btm, strProdID[i], ProdStatus.OK, "error");
									}
									else
									{
										SM.FMP.SetInspResult(FlowMngPart.teDir.Btm, strProdID[i], ProdStatus.OK, "error");
									}
								}
							}

							smm.Next(Cmd.INSP_WAIT_110);
						}
						break;

					case Cmd.INSP_WAIT_110:       // RAM 제품 ID 보고 확인 Top
						{
							if (SM.SHM.GetUseSkipData(USESKIP.TopInspection) == false)
							{
								smm.Next(Cmd.INSP_WAIT_120);
								break;
							}

							bool bconfirm = false;
							int nret = G.commInspTop.RcvProdDataReportConfirm(ref bconfirm);
							if (nret < 0)
							{
								smm.Next(Cmd.END);
							}
							else if (nret > 0)
							{
								if (bconfirm == true)
								{
									smm.Next(Cmd.INSP_WAIT_120);
								}
								else
								{
									AddAlarm(ALM.ERROR_SEQ_INSP_TOP_PC_CONFIRM_NG);
									smm.Next(Cmd.END);
								}
							}
							else if (smm.Delayms(m_nInspCommWait) == true)
							{
								AddAlarm(ALM.ERROR_SEQ_INSP_TOP_PC_CONFIRM_COMM_TIMEOUT);
								smm.Next(Cmd.END);
							}
						}
						break;
					case Cmd.INSP_WAIT_120:       // RAM 제품 ID 보고 확인 Btm
						{
							if (SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == false)
							{
								smm.Next(Cmd.INSP_WAIT_130);
								break;
							}

							bool bconfirm = false;
							int nret = G.commInspBtm.RcvProdDataReportConfirm(ref bconfirm);
							if (nret < 0)
							{
								smm.Next(Cmd.END);
							}
							else if (nret > 0)
							{
								if (bconfirm == true)
								{
									smm.Next(Cmd.INSP_WAIT_130);
								}
								else
								{
									AddAlarm(ALM.ERROR_SEQ_INSP_BTM_PC_CONFIRM_NG);
									smm.Next(Cmd.END);
								}
							}
							else if (smm.Delayms(m_nInspCommWait) == true)
							{
								AddAlarm(ALM.ERROR_SEQ_INSP_BTM_PC_CONFIRM_COMM_TIMEOUT);
								smm.Next(Cmd.END);
							}
						}
						break;

					case Cmd.INSP_WAIT_130:       // 검사 시작 Send
						{
							m_bDIInspStart[0] = false;
							m_bDIInspStart[1] = false;
							m_bDIInspEnd[0] = false;
							m_bDIInspEnd[1] = false;
							if (SM.SHM.GetUseSkipData(USESKIP.TopInspection) == true && G.cKernel.IsAIn(DI_TopInspEnd) == true) break;
							if (SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == true && G.cKernel.IsAIn(DI_BtmInspEnd) == true) break;

							if (SM.SHM.GetUseSkipData(USESKIP.TopInspection) == true)
							{
								G.commInspTop.ResetRcvData(CommInsp.RcvCmd.ResultDataReport);
								G.cKernel.AOut(DO_TopInspReady, true);
							}
							if (SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == true)
							{
								G.commInspBtm.ResetRcvData(CommInsp.RcvCmd.ResultDataReport);
								G.cKernel.AOut(DO_BtmInspReady, true);
							}

							smm.Next(Cmd.INSP_WAIT_140);
						}
						break;

					case Cmd.INSP_WAIT_140:       // 검사 진행 상태 요청
						{
							if (SM.SHM.GetUseSkipData(USESKIP.TopInspection) == true && m_bDIInspStart[0] == false)
							{
								G.commInspTop.SendInspStateRequest();
							}
							if (SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == true && m_bDIInspStart[1] == false)
							{
								G.commInspBtm.SendInspStateRequest();
							}

							smm.Next(Cmd.INSP_WAIT_150);
						}
						break;

					case Cmd.INSP_WAIT_150:       // 검사 진행 상태 확인 Top
						{
							if (SM.SHM.GetUseSkipData(USESKIP.TopInspection) == false || m_bDIInspStart[0] == true || m_bDIInspEnd[0] == true)
							{
								smm.Next(Cmd.INSP_WAIT_160);
								break;
							}

							CommInsp.InspState state = CommInsp.InspState.Idle;
							int nret = G.commInspTop.RcvInspStateReport(ref state);
							if (nret < 0)
							{
								smm.Next(Cmd.END);
							}
							else if (nret > 0)
							{
								if (state != CommInsp.InspState.Idle)
								{
									m_bDIInspStart[0] = true;
									G.cKernel.AOut(DO_TopInspReady, false);
									smm.Next(Cmd.INSP_WAIT_160);
								}
								else
								{
									smm.Next(Cmd.RETRY_DELEAY_20);
								}
							}
							else if (smm.Delayms(m_nInspCommWait) == true)
							{
								AddAlarm(ALM.ERROR_SEQ_INSP_TOP_PC_INSPSTATE_COMM_TIMEOUT);
								smm.Next(Cmd.END);
							}
						}
						break;

					case Cmd.INSP_WAIT_160:       // 검사 진행 상태 확인 Btm
						{
							if (SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == false || m_bDIInspStart[1] == true || m_bDIInspEnd[1] == true)
							{
								smm.Next(Cmd.INSP_WAIT_170);
								break;
							}

							CommInsp.InspState state = CommInsp.InspState.Idle;
							int nret = G.commInspBtm.RcvInspStateReport(ref state);
							if (nret < 0)
							{
								smm.Next(Cmd.END);
							}
							else if (nret > 0)
							{
								if (state != CommInsp.InspState.Idle)
								{
									m_bDIInspStart[1] = true;
									G.cKernel.AOut(DO_BtmInspReady, false);
									smm.Next(Cmd.INSP_WAIT_170);
								}
								else
								{
									smm.Next(Cmd.RETRY_DELEAY_20);
								}
							}
							else if (smm.Delayms(m_nInspCommWait) == true)
							{
								AddAlarm(ALM.ERROR_SEQ_INSP_BTM_PC_INSPSTATE_COMM_TIMEOUT);
								smm.Next(Cmd.END);
							}
						}
						break;

					case Cmd.RETRY_DELEAY_20:
						{
							if (G.cKernel.IsAIn(DI_TopInspEnd) == true)
							{
								G.cKernel.AOut(DO_TopInspReady, false);
								m_bDIInspEnd[0] = true;
							}
							if (G.cKernel.IsAIn(DI_BtmInspEnd) == true)
							{
								G.cKernel.AOut(DO_BtmInspReady, false);
								m_bDIInspEnd[1] = true;
							}

							if (SM.SHM.GetUseSkipData(USESKIP.TopInspection) == true && (m_bDIInspStart[0] == false && m_bDIInspEnd[0] == false))
							{
								G.cKernel.AOut(DO_TopInspReady, true);
							}
							if (SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == true && (m_bDIInspStart[1] == false && m_bDIInspEnd[1] == false))
							{
								G.cKernel.AOut(DO_BtmInspReady, true);
							}

							if (smm.Delayms(200) == false) break;

							smm.Next(Cmd.INSP_WAIT_140);
						}
						break;

					case Cmd.INSP_WAIT_170:       // 그랩 완료 대기, 디버깅시 검사 완료 대기
						{
							if (smm.Delayms(NDm.Read(eNDM.TimeoutInspection)) == true &&
								SM.SHM.GetUseSkipData(USESKIP.InspDebugPlate1) == false &&
								SM.SHM.GetUseSkipData(USESKIP.InspDebugPlate2) == false)
							{
								if (m_bDIInspEnd[0] == false) AddAlarm(ALM.ERROR_SEQ_INSP_TOP_PC_ISNPECTION_TIMEOUT);
								else if (m_bDIInspEnd[1] == false) AddAlarm(ALM.ERROR_SEQ_INSP_BTM_PC_ISNPECTION_TIMEOUT);
								else AddAlarm(ALM.ERROR_SEQ_INSP_INSPECTION_TIMEOUT);

								smm.Next(Cmd.END);
								break;
							}

							if (G.cKernel.IsAIn(DI_TopInspEnd) == true)
							{
								G.cKernel.AOut(DO_TopInspReady, false);
								m_bDIInspEnd[0] = true;
							}
							if (G.cKernel.IsAIn(DI_BtmInspEnd) == true)
							{
								G.cKernel.AOut(DO_BtmInspReady, false);
								m_bDIInspEnd[1] = true;
							}

							if (SM.SHM.GetUseSkipData(USESKIP.TopInspection) == true && m_bDIInspEnd[0] == false) break;
							if (SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == true && m_bDIInspEnd[1] == false) break;

							stProdInfo[] info = SM.FMP[FlowPlate].GetInfo().ProdInfo;

							//ㅇ
							if (SM.SHM.GetUseSkipData(USESKIP.TopInspection) == true &&
								(SM.SHM.GetUseSkipData(USESKIP.InspDebugPlate1) == true || SM.SHM.GetUseSkipData(USESKIP.InspDebugPlate2) == true) &&
								((info[0].ResultTop == (int)ProdStatus.Unknown && info[0].Exist == true) ||
								 (info[1].ResultTop == (int)ProdStatus.Unknown && info[1].Exist == true) ||
								 (info[2].ResultTop == (int)ProdStatus.Unknown && info[2].Exist == true) ||
								 (info[3].ResultTop == (int)ProdStatus.Unknown && info[3].Exist == true)))
							{
								break;
							}

							if (SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == true &&
								(SM.SHM.GetUseSkipData(USESKIP.InspDebugPlate1) == true || SM.SHM.GetUseSkipData(USESKIP.InspDebugPlate2) == true) &&
								((info[0].ResultBtm == (int)ProdStatus.Unknown && info[0].Exist == true) ||
								 (info[1].ResultBtm == (int)ProdStatus.Unknown && info[1].Exist == true) ||
								 (info[2].ResultBtm == (int)ProdStatus.Unknown && info[2].Exist == true) ||
								 (info[3].ResultBtm == (int)ProdStatus.Unknown && info[3].Exist == true)))
							{
								break;
							}

							if (SM.SHM.GetUseSkipData(USESKIP.InspDebugPlate1) == true)
							{
								if (NDm.Read(eNDM.BtmInspNextIdx) != 1 || NDm.Read(eNDM.TopInspNextIdx) != 1)
								{
									NDm.Write(eNDM.BtmInspNextIdx, 1);
									NDm.Write(eNDM.TopInspNextIdx, 1);
								}
							}
							else if (SM.SHM.GetUseSkipData(USESKIP.InspDebugPlate2) == true)
							{
								if (NDm.Read(eNDM.BtmInspNextIdx) != 2 || NDm.Read(eNDM.TopInspNextIdx) != 2)
								{
									NDm.Write(eNDM.BtmInspNextIdx, 2);
									NDm.Write(eNDM.TopInspNextIdx, 2);
								}
							}
							else
							{
								int nIdx = m_nPartNo == 0 ? 1 : 0;
								if (SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == true) NDm.Write(eNDM.BtmInspNextIdx, nIdx + 1);     // 애니메이션 용도
								if (SM.SHM.GetUseSkipData(USESKIP.TopInspection) == true) NDm.Write(eNDM.TopInspNextIdx, nIdx + 1);     // 애니메이션 용도
							}


							smm.Next(Cmd.INSP_WAIT_180);
						}
						break;
					case Cmd.INSP_WAIT_180:
						{
							if (smm.Delayms(NDm.Read(eNDM.InspWaitTimeOut)) == true)
							{
								if (CheckInspResultExist() == 1) AddAlarm(ALM.ERROR_SEQ_INSP_TOP_PC_ISNPECTION_TIMEOUT);
								else AddAlarm(ALM.ERROR_SEQ_INSP_BTM_PC_ISNPECTION_TIMEOUT);
								smm.ResetDelayms();

								break;
							}

							if (CheckInspResultExist() != 0) break;

							bool[] bexist = SM.FMP[FlowPlate].GetProdExist();

							for (int i = 0; i < SM.FMP[FlowPlate].GetMaxProdCount(); i++)
							{
								if (bexist[i] == true)
								{
									if (SM.FMP[FlowPlate].GetInfo().ProdInfo[i].SerialNumber.ToLower().Contains("fiduerror") == true)
									{
										SM.SHM.AddFiduErrorCount(SM.FMP[FlowPlate].GetInfo().KeyValue);
									}
								}
							}

							if (SM.SHM.GetFiduErrorCount(SM.FMP[FlowPlate].GetInfo().KeyValue) > NDm.Read(eNDM.FiduErrorThreshold))
							{
								SM.SHM.ClearFiduErrorCount(SM.FMP[FlowPlate].GetInfo().KeyValue);
								AddAlarm(ALM.ERROR_CAN_NOT_FIND_FIDUCIAL_REPEATLY);
								smm.Next(Cmd.END);
							}

							m_bInspDone = true;
							smm.Next(Cmd.END);
						}
						break;

					case Cmd.END:
						{
							G.cKernel.AOut(DO_TopInspReady, false);
							G.cKernel.AOut(DO_BtmInspReady, false);
							SM.SHM.SetBitData(BIT.InspLock, false);

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

		public void TeachingWait(SMM<Cmd> smm)
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

								smm.Next(Cmd.TEACH_WAIT_01);
							}
						}
						break;

					case Cmd.TEACH_WAIT_01:        // RAM 유무 확인
						{
							if (arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false ||
								arrMt[Axis.W].IsRdy((int)PosW.LOAD) == false)
							{
								arrMt[Axis.Z].MoveIndex((int)PosZ.READY);
								arrMt[Axis.W].MoveIndex((int)PosW.LOAD);
								break;
							}
							
							if (G.cKernel.IsAOut(DO_TopInspReady) || G.cKernel.IsAOut(DO_BtmInspReady))
							{
								G.cKernel.AOut(DO_TopInspReady, false);
								G.cKernel.AOut(DO_BtmInspReady, false);
							}
							
							smm.Next(Cmd.TEACH_WAIT_02);
						}
						break;

					case Cmd.TEACH_WAIT_02:        // RAM 유무 확인
						{
							if (arrMt[Axis.Y].IsRdy((int)PosY.LOAD) == false )
							{
								InterlockCheckMove(PosY.LOAD);
								break;
							}

							if (arrMt[Axis.Z].IsRdy((int)PosZ.LOAD) == false)
							{
								arrMt[Axis.Z].MoveIndex((int)PosZ.LOAD);
								break;
							}
							
							smm.Next(Cmd.TEACH_WAIT_10);
						}
						break;

					case Cmd.TEACH_WAIT_10:       // RAM 유무 확인
						{
							switch (CheckProdExist())
							{
								case ProdExist.Exist:
									m_IsGrip = true;
									smm.Next(Cmd.TEACH_WAIT_20);
									break;
								case ProdExist.Mismatch:
									AddAlarm(ALM.ERROR_SEQ_INSP_PLATE1_RAM_MISMATCH + m_nPartNo);
									break;
								case ProdExist.None:
									AddAlarm(ALM.ERROR_SEQ_INSP_PLATE1_RAM_NONE + m_nPartNo);
									break;
							}

							m_bInspDone = false;
						}
						break;

					case Cmd.TEACH_WAIT_20:       // 회전 90도, INSP 위치 이동
						{
							if (arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false)
							{
								arrMt[Axis.Z].MoveIndex((int)PosZ.READY);
								break;
							}

							if (arrMt[Axis.Y].IsRdy((int)PosY.INSP) == false )
							{
								InterlockCheckMove(PosY.INSP);
								break;
							}

							smm.Next(Cmd.TEACH_WAIT_30);
						}
						break;

					case Cmd.TEACH_WAIT_30:       // 검사 시작 bit On
						{
							G.cKernel.AOut(DO_TopInspReady, true);
							G.cKernel.AOut(DO_BtmInspReady, true);

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

		public void Reference(SMM<Cmd> smm)
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
								
								smm.Next(Cmd.REFERENCE_INSPRUN_10);
							}
						}
						break;

					case Cmd.REFERENCE_INSPRUN_10:
						{
							if (SM.SHM.GetUseSkipData(USESKIP.TopInspection) == true)
							G.commInspTop.SendRunStateRequest();

							smm.Next(Cmd.REFERENCE_INSPRUN_20);
						}
						break;

					case Cmd.REFERENCE_INSPRUN_20: //상부 검사 run 시작
						{
							if (SM.SHM.GetUseSkipData(USESKIP.TopInspection) == false)
							{
								smm.Next(Cmd.REFERENCE_INSPRUN_40);
								break;
							}

							CommInsp.RunState state = CommInsp.RunState.Idle;
							int nret = G.commInspTop.RcvRunStateReport(ref state);
							if (nret < 0)
							{
								smm.Next(Cmd.END);
							}
							else if (nret > 0)
							{
								if (state == CommInsp.RunState.Run)
								{
									smm.Next(Cmd.REFERENCE_INSPRUN_40);
								}
								else
								{
									G.commInspTop.SendAutoRunStart();
									smm.Next(Cmd.REFERENCE_INSPRUN_30);
								}
							}
							else if (smm.Delayms(m_nInspCommWait) == true)
							{
								AddAlarm(ALM.ERROR_SEQ_INSP_TOP_PC_RUNSTATE_COMM_TIMEOUT);
								smm.Next(Cmd.END);
							}
						}
						break;

					case Cmd.REFERENCE_INSPRUN_30: //상부 검사 run 체크
						{
							bool bOK = false;
							int nret = G.commInspTop.RcvAutoRunStartConfirm(ref bOK);
							if (nret < 0)
							{
								smm.Next(Cmd.END);
							}
							else if (nret > 0)
							{
								if (bOK == true)
								{
									smm.Next(Cmd.REFERENCE_INSPRUN_40);
								}
								else
								{
									AddAlarm(ALM.ERROR_SEQ_INSP_TOP_PC_NOT_AUTORUN);
									smm.Next(Cmd.END);
								}
							}
							else if (smm.Delayms(m_nInspCommWait) == true)
							{
								AddAlarm(ALM.ERROR_SEQ_INSP_BTM_PC_AUTORUN_COMM_TIMEOUT);
								smm.Next(Cmd.END);
							}
						}
						break;

					case Cmd.REFERENCE_INSPRUN_40://하부 검사 run 시작
						{
							if (SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == false)
							{
								smm.Next(Cmd.REFERENCE_10);
								break;
							}

							CommInsp.RunState state = CommInsp.RunState.Idle;
							int nret = G.commInspBtm.RcvRunStateReport(ref state);
							if (nret < 0)
							{
								smm.Next(Cmd.END);
							}
							else if (nret > 0)
							{
								if (state == CommInsp.RunState.Run)
								{
									smm.Next(Cmd.REFERENCE_10);
								}
								else
								{
									G.commInspBtm.SendAutoRunStart();
									smm.Next(Cmd.REFERENCE_INSPRUN_50);
								}
							}
							else if (smm.Delayms(m_nInspCommWait) == true)
							{
								AddAlarm(ALM.ERROR_SEQ_INSP_BTM_PC_RUNSTATE_COMM_TIMEOUT);
								smm.Next(Cmd.END);
							}
						}
						break;

					case Cmd.REFERENCE_INSPRUN_50:
						{
							bool bOK = false;
							int nret = G.commInspBtm.RcvAutoRunStartConfirm(ref bOK);
							if (nret < 0)
							{
								smm.Next(Cmd.END);
							}
							else if (nret > 0)
							{
								if (bOK == true)
								{
									smm.Next(Cmd.REFERENCE_10);
								}
								else
								{
									AddAlarm(ALM.ERROR_SEQ_INSP_BTM_PC_NOT_AUTORUN);
									smm.Next(Cmd.END);
								}
							}
							else if (smm.Delayms(m_nInspCommWait) == true)
							{
								AddAlarm(ALM.ERROR_SEQ_INSP_BTM_PC_AUTORUN_COMM_TIMEOUT);
								smm.Next(Cmd.END);
							}
						}
						break;

					case Cmd.REFERENCE_10:
						{
							//해당 하는 카메라 체크,
							//해당 bit false 처리
							if (m_nPartNo == 0 || m_nPartNo == 1)
							{
								m_nCamera = m_nPartNo;
								smm.Next(Cmd.REFERENCE_40);
							}
							else
							{
								smm.Next(Cmd.END);
								break;
							}
						}
						break;

					case Cmd.REFERENCE_40:
						{
							if (G.seqLoadInspPnP.IsPlaceReady() == false) break;
							if (G.seqUnloadInspPnP.IsPickReady() == false) break;
							
							if (arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false)
							{
								arrMt[Axis.Z].MoveIndex((int)PosZ.READY);
								break;
							}
							if (arrMt[Axis.Y].IsRdy((int)PosY.REFERENCE) == false)
							{
								InterlockCheckMove(PosY.REFERENCE);
								break;
							}

							switch (m_nCamera)
							{
								case 0: G.commInspTop.SendReferenceStart(m_nHead); break;
								case 1: G.commInspBtm.SendReferenceStart(m_nHead); break;
								default:
									break;
							}
							smm.Next(Cmd.REFERENCE_50);
						}
						break;

					case Cmd.REFERENCE_50:
						{
							switch (m_nCamera)
							{
								case 0: G.commInspTop.SendInspStateRequest(); break;
								case 1: G.commInspBtm.SendInspStateRequest(); break;
								default:
									break;
							}
							smm.Next(Cmd.REFERENCE_60);
						}
						break;

					case Cmd.REFERENCE_60:
						{
							if (m_nCamera == 1)//Bot만 측정하는 경우 현재 스탭 패스
							{
								smm.Next(Cmd.REFERENCE_70);
								break;
							}

							CommInsp.InspState state = CommInsp.InspState.Idle;
							int nret = G.commInspTop.RcvInspStateReport(ref state);

							if (nret < 0)
							{
								smm.Next(Cmd.END);
							}
							else if (nret > 0)
							{
								if (state == CommInsp.InspState.Ref)
								{
									m_nRetryCount = 0;
									smm.Next(Cmd.REFERENCE_70);
								}
								else
								{
									smm.Next(Cmd.RETRY_DELEAY_30);
								}
							}
							else if (smm.Delayms(10000) == true)
							{
								AddAlarm(ALM.ERROR_SEQ_INSP_TOP_PC_INSPSTATE_COMM_TIMEOUT);
								smm.Next(Cmd.END);
							}
						}
						break;

					case Cmd.REFERENCE_70:
						{
							if (m_nCamera == 0)//top만 측정하는 경우 현재 스탭 패스
							{
								smm.Next(Cmd.REFERENCE_80);
								break;
							}

							CommInsp.InspState state = CommInsp.InspState.Idle;
							int nret = G.commInspBtm.RcvInspStateReport(ref state);

							if (nret < 0)
							{
								smm.Next(Cmd.END);
							}
							else if (nret > 0)
							{
								if (state == CommInsp.InspState.Ref)
								{
									m_nRetryCount = 0;
									smm.Next(Cmd.REFERENCE_80);
								}
								else
								{
									smm.Next(Cmd.RETRY_DELEAY_30);
								}
							}
							else if (smm.Delayms(10000) == true)
							{
								AddAlarm(ALM.ERROR_SEQ_INSP_BTM_PC_INSPSTATE_COMM_TIMEOUT);
								smm.Next(Cmd.END);
							}
						}
						break;

					case Cmd.REFERENCE_80:
						{
							switch (m_nCamera)
							{
								case 0: G.commInspTop.SendInspStateRequest(); break;
								case 1: G.commInspBtm.SendInspStateRequest(); break;
								default:
									break;
							}
							smm.Next(Cmd.REFERENCE_90);
						}
						break;

					case Cmd.REFERENCE_90:
						{
							if (m_nCamera == 1)//Bot만 측정하는 경우 현재 스탭 패스
							{
								smm.Next(Cmd.REFERENCE_100);
							}

							CommInsp.InspState state = CommInsp.InspState.Idle;
							int nret = G.commInspTop.RcvInspStateReport(ref state);

							if (nret < 0)
							{
								smm.Next(Cmd.END);
								break;
							}
							else if (nret > 0)
							{
								if (state == CommInsp.InspState.Idle)
								{
									m_nRetryCount = 0;
									smm.Next(Cmd.REFERENCE_100);
								}
								else
								{
									smm.Next(Cmd.RETRY_DELEAY_40);
								}
							}
							else if (smm.Delayms(10000) == true)
							{
								AddAlarm(ALM.ERROR_SEQ_INSP_TOP_PC_INSPSTATE_COMM_TIMEOUT);
								smm.Next(Cmd.END);
							}
						}
						break;

					case Cmd.REFERENCE_100:
						{
							if (m_nCamera == 0)//top만 측정하는 경우 현재 스탭 패스
							{
								smm.Next(Cmd.REFERENCE_110);
								break;
							}

							CommInsp.InspState state = CommInsp.InspState.Idle;
							int nret = G.commInspBtm.RcvInspStateReport(ref state);

							if (nret < 0)
							{
								smm.Next(Cmd.END);
							}
							else if (nret > 0)
							{
								if (state == CommInsp.InspState.Idle)
								{
									m_nRetryCount = 0;
									smm.Next(Cmd.REFERENCE_110);
								}
								else
								{
									smm.Next(Cmd.RETRY_DELEAY_40);
								}
							}
							else if (smm.Delayms(10000) == true)
							{
								AddAlarm(ALM.ERROR_SEQ_INSP_BTM_PC_INSPSTATE_COMM_TIMEOUT);
								smm.Next(Cmd.END);
							}
						}
						break;

					case Cmd.REFERENCE_110:
						{
							smm.Next(Cmd.END);
						}
						break;

					case Cmd.RETRY_DELEAY_30:
						{
							if (smm.Delayms(300) == false) break;

							m_nRetryCount++;
							if (m_nRetryCount > 50)
							{
								m_nRetryCount = 0;
								if (m_nCamera == 0) AddAlarm(ALM.ERROR_SEQ_INSP_TOP_PC_COMM_TIMEOUT);
								else AddAlarm(ALM.ERROR_SEQ_INSP_BTM_PC_COMM_TIMEOUT);
							}

							smm.Next(Cmd.REFERENCE_50);
						}
						break;

					case Cmd.RETRY_DELEAY_40:
						{
							if (smm.Delayms(300) == false) break;

							m_nRetryCount++;
							if (m_nRetryCount > 50)
							{
								m_nRetryCount = 0;
								if (m_nCamera == 0) AddAlarm(ALM.ERROR_SEQ_INSP_TOP_PC_COMM_TIMEOUT);
								else AddAlarm(ALM.ERROR_SEQ_INSP_BTM_PC_COMM_TIMEOUT);
							}

							smm.Next(Cmd.REFERENCE_80);
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

		public void Calibration(SMM<Cmd> smm)
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
								smm.Next(Cmd.CALIBRATION_01);
							}
						}
						break;

						
					case Cmd.CALIBRATION_01:
						{					
							if (arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false ||
								arrMt[Axis.W].IsRdy((int)PosW.LOAD) == false)
							{
								arrMt[Axis.Z].MoveIndex((int)PosZ.READY);
								arrMt[Axis.W].MoveIndex((int)PosW.LOAD);
								break;
							}

							smm.Next(Cmd.CALIBRATION_02);
						}
						break;

						
					case Cmd.CALIBRATION_02:
						{		
							if (arrMt[Axis.Y].IsRdy((int)PosY.LOAD) == false )
							{
								InterlockCheckMove(PosY.LOAD);
								break;
							}
							if (arrMt[Axis.Z].IsRdy((int)PosZ.LOAD) == false)
							{
								arrMt[Axis.Z].MoveIndex((int)PosZ.LOAD);
								break;
							}
							
							smm.Next(Cmd.CALIBRATION_03);
						}
						break;

					case Cmd.CALIBRATION_03:
						{
							if (CheckProdExist() == ProdExist.Mismatch)
							{
								AddAlarm(ALM.ERROR_SEQ_INSP_PLATE1_RAM_MISMATCH + m_nPartNo);
								smm.Next(Cmd.END);
								break;
							}
							else if (CheckProdExist() == ProdExist.Exist)
							{
								AddAlarm(ALM.ERROR_SEQ_INSP_PLATE1_RAM_EXIST + m_nPartNo);
								smm.Next(Cmd.END);
								break;
							}

							//interlock check
							if (G.seqLoadInspPnP.CheckCylUpDown(SenPos.PICKER_UP) == false)
							{
								frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Please Check Load Insp PnP Up/Down Cylinder Position");
								pMsg.ShowDialog();
								smm.Next(Cmd.END);
								break;
							}

							if (G.seqUnloadInspPnP.CheckCylUpDown(SenPos.PICKER_UP) == false)
							{
								frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Please Check Unload Insp PnP Up/Down Cylinder Position");
								pMsg.ShowDialog();
								smm.Next(Cmd.END);
								break;
							}

							if (G.cKernel.IsAIn(DI.TopRearDoorSen) == false)
							{
								frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Please Check TOP Rear Door Sensor. The Top Rear door must be closed during this operation.");
								pMsg.ShowDialog();
								smm.Next(Cmd.END);
								break;
							}

							if (G.cKernel.IsAIn(DI.BtmDoorSen) == false)
							{
								frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Please Check Bottom Door Sensor. The Bottom door must be closed during this operation.");
								pMsg.ShowDialog();
								smm.Next(Cmd.END);
								break;
							}

							if (m_nMoveType >= 2)
								smm.Next(Cmd.MAINTANCE_10);
							else
								smm.Next(Cmd.CALIBRATION_10);
						}
						break;
					#region  캘레브레이션 위치 이동
					case Cmd.CALIBRATION_10:
						{
							//1번 스테이지 : 상부
							if (m_nPartNo == 0)
							{
								if (SM.SHM.IsBitData(BIT.TopInspConnect) == false)
								{
									frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Please Check Top Inspection Connect State");
									pMsg.ShowDialog();
									smm.Next(Cmd.END);
									break;
								}

								if (SM.SHM.GetUseSkipData(USESKIP.TopInspection) == false)
								{
									frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Please Check Use/Skip Option - Top Inspection");
									pMsg.ShowDialog();
									smm.Next(Cmd.END);
									break;
								}
							}


							//1번 스테이지 : 상부
							if (m_nPartNo == 1)
							{
								if (SM.SHM.IsBitData(BIT.BtmInspConnect) == false)
								{
									frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Please Check Bottom Inspection Connect State");
									pMsg.ShowDialog();
									smm.Next(Cmd.END);
									break;
								}

								if (SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == false)
								{
									frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Please Check Use/Skip Option - Bottom Inspection");
									pMsg.ShowDialog();
									smm.Next(Cmd.END);
									break;
								}
							}

							smm.Next(Cmd.CALIBRATION_20);
						}
						break;

					case Cmd.CALIBRATION_20:
						{
							if (arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false)
							{
								arrMt[Axis.Z].MoveIndex((int)PosZ.READY);
								break;
							}

							if (arrMt[Axis.Y].IsRdy((int)PosY.CALIBRATION) == false ||
								arrMt[Axis.GRIP].IsRdy((int)PosGrip.CALIBRATION) == false)
							{
								InterlockCheckMove(PosY.CALIBRATION);
								arrMt[Axis.GRIP].MoveIndex((int)PosGrip.CALIBRATION);
								break;
							}

							smm.Next(Cmd.CALIBRATION_30);
						}
						break;
						  
					case Cmd.CALIBRATION_30:
						{
							if (m_nPartNo == 0) G.commInspTop.SendMoveSetPosRequest(m_nHead, m_nMoveType);
							else G.commInspBtm.SendMoveSetPosRequest(m_nHead, m_nMoveType);

							smm.Next(Cmd.CALIBRATION_40);
						}
						break;

					case Cmd.CALIBRATION_40:
						{
							bool bState = false;
							int nret = -1;

							if (m_nPartNo == 0) nret = G.commInspTop.RcvMoveSetPosReport(ref bState);
							else nret = G.commInspBtm.RcvMoveSetPosReport(ref bState);
							if (nret < 0)
							{
								if (m_nPartNo == 0) AddAlarm(ALM.ERROR_SEQ_INSP_TOP_PC_CONFIRM_NG);
								else AddAlarm(ALM.ERROR_SEQ_INSP_BTM_PC_CONFIRM_NG);

								smm.Next(Cmd.END);
							}
							else if (nret > 0)
							{
								if (bState == true)
								{
									smm.Next(Cmd.CALIBRATION_50);
								}
								else
								{
									if (m_nPartNo == 0) AddAlarm(ALM.ERROR_SEQ_INSP_TOP_PC_MOVE_FAIL);
									else AddAlarm(ALM.ERROR_SEQ_INSP_BTM_PC_MOVE_FAIL);

									smm.Next(Cmd.END);
								}
							}
							else if (smm.Delayms(m_nInspCommWait) == true)
							{
								if (m_nPartNo == 0) AddAlarm(ALM.ERROR_SEQ_INSP_TOP_PC_COMM_TIMEOUT);
								else AddAlarm(ALM.ERROR_SEQ_INSP_BTM_PC_COMM_TIMEOUT);

								smm.Next(Cmd.END);
							}
						}
						break;

					case Cmd.CALIBRATION_50:
						{
							string strPos = m_nPartNo == 0 ? "TOP" : "BOTTOM";
							string strHead = m_nHead == 0 ? "1" : "2";
							string strJigType = m_nMoveType == 0 ? "LIGHT" : "3D";


							string strMsg = $" {strPos}-Head #{strHead} [{strJigType}] Calibration" + Environment.NewLine +
												$"Position Move Complete." + Environment.NewLine +
												$"Please Proceed calibration in {strPos} PC. ";

							SM.SHM.SetMsg(SeqMsg.SEQINFO, strMsg);
							SM.SHM.MsgShow(SeqMsg.SEQINFO);

							smm.Next(Cmd.END);
						}
						break;
					#endregion

					#region  유지보수 위치 이동
					case Cmd.MAINTANCE_10:
						{
							if (SM.SHM.IsBitData(BIT.TopInspConnect) == false)
							{
								frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Please Check Top Inspection Connect State");
								pMsg.ShowDialog();
								smm.Next(Cmd.END);
								break;
							}

							if (SM.SHM.GetUseSkipData(USESKIP.TopInspection) == false)
							{
								frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Please Check Use/Skip Option - Top Inspection");
								pMsg.ShowDialog();
								smm.Next(Cmd.END);
								break;
							}

							if (SM.SHM.IsBitData(BIT.BtmInspConnect) == false)
							{
								frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Please Check Bottom Inspection Connect State");
								pMsg.ShowDialog();
								smm.Next(Cmd.END);
								break;
							}

							if (SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == false)
							{
								frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Please Check Use/Skip Option - Bottom Inspection");
								pMsg.ShowDialog();
								smm.Next(Cmd.END);
								break;
							}


							smm.Next(Cmd.MAINTANCE_20);
						}
						break;
					case Cmd.MAINTANCE_20:
						{
							if (arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false)
							{
								arrMt[Axis.Z].MoveIndex((int)PosZ.READY);
								break;
							}
							// MoveType(0:Light Jig/ 1:3D Jig/ 2: 1StageMroPos/ 3: 2StageMropPos/ 4 CamMroPos)
							if (m_nPartNo == 0 && m_nMoveType == 2)
							{
								if (arrMt[Axis.Y].IsRdy((int)PosY.INSP) == false)
								{
									InterlockCheckMove(PosY.INSP);
									break;
								}
							}
							else if (m_nPartNo == 1 && m_nMoveType == 3)
							{
								if (arrMt[Axis.Y].IsRdy((int)PosY.INSP) == false)
								{
									InterlockCheckMove(PosY.INSP);
									break;
								}
							}
							else if (m_nPartNo == 0 && m_nMoveType == 4)
							{
								if (arrMt[Axis.Y].IsRdy((int)PosY.LOAD) == false)
								{
									InterlockCheckMove(PosY.LOAD);
									break;
								}
							}

							smm.Next(Cmd.MAINTANCE_30);
						}
						break;

					case Cmd.MAINTANCE_30:
						{
							G.commInspTop.SendMoveSetPosRequest(0, m_nMoveType);
							G.commInspBtm.SendMoveSetPosRequest(0, m_nMoveType);

							smm.Next(Cmd.MAINTANCE_40);
						}
						break;

					case Cmd.MAINTANCE_40: //상부 확인
						{
							bool bState = false;
							int nret = -1;

							nret = G.commInspTop.RcvMoveSetPosReport(ref bState);

							if (nret < 0)
							{
								AddAlarm(ALM.ERROR_SEQ_INSP_TOP_PC_CONFIRM_NG);
								smm.Next(Cmd.END);
							}
							else if (nret > 0)
							{
								if (bState == true)
								{
									smm.Next(Cmd.MAINTANCE_50);
								}
								else
								{
									AddAlarm(ALM.ERROR_SEQ_INSP_TOP_PC_MOVE_FAIL);
									smm.Next(Cmd.END);
								}
							}
							else if (smm.Delayms(m_nInspCommWait) == true)
							{
								AddAlarm(ALM.ERROR_SEQ_INSP_TOP_PC_COMM_TIMEOUT);
								smm.Next(Cmd.END);
							}
						}
						break;
					case Cmd.MAINTANCE_50:
						{
							bool bState = false;
							int nret = -1;

							nret = G.commInspBtm.RcvMoveSetPosReport(ref bState);
							if (nret < 0)
							{
								AddAlarm(ALM.ERROR_SEQ_INSP_BTM_PC_CONFIRM_NG);
								smm.Next(Cmd.END);
							}
							else if (nret > 0)
							{
								if (bState == true)
								{
									smm.Next(Cmd.MAINTANCE_60);
								}
								else
								{
									AddAlarm(ALM.ERROR_SEQ_INSP_BTM_PC_MOVE_FAIL);

									smm.Next(Cmd.END);
								}
							}
							else if (smm.Delayms(m_nInspCommWait) == true)
							{
								AddAlarm(ALM.ERROR_SEQ_INSP_BTM_PC_COMM_TIMEOUT);
								smm.Next(Cmd.END);
							}
						}
						break;
					case Cmd.MAINTANCE_60:
						{
							string strJigType = "";
							switch (m_nMoveType)
							{
								case 2: strJigType = "STAGE 1"; break;
								case 3: strJigType = "STAGE 2"; break;
								case 4: strJigType = "BOT HEAD"; break;
							}

							string strMsg = $" {strJigType} Maintenance" + Environment.NewLine +
												$"Position Move Complete." + Environment.NewLine;

							SM.SHM.SetMsg(SeqMsg.SEQINFO, strMsg);
							SM.SHM.MsgShow(SeqMsg.SEQINFO);

							smm.Next(Cmd.END);
						}
						break;
					#endregion

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