using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_MarsSSD
{
	public class SeqLoadBuffer : SeqBase<SeqLoadBuffer.Act, SeqLoadBuffer.Axis, SeqLoadBuffer.Cmd>
	{
		#region [ Enum ]
		public enum Act
		{
			TableUpDn,
			CleanUpDn,
			CleanAirMove,
			DuctUpDn,
			BlowIonizer,
			CleanAirBlow,
			DustValve,
			Grip,
			Max,
		}

		public enum Axis
		{
			Y,
			SIZE,
			Rotate,
			Max,
		}

		public enum Cmd
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

			LD_WAIT_10, // 레디 위치로 가서 기다림
            LD_WAIT_11,
            LD_WAIT_20,
            LD_WAIT_30,
			LD_WAIT_40,
			LD_WAIT_50,
			LD_WAIT_60,
			LD_WAIT_PLACE_WAIT,
			LD_WAIT_70,
			LD_WAIT_80,
			LD_WAIT_90,
			LD_WAIT_100,
			LD_WAIT_110,
			LD_WAIT_120,
			LD_WAIT_130,
			LD_WAIT_140,
			LD_WAIT_150,

			CLEAN_10, // 클린 위치로 가서 클린 진행
			CLEAN_20,
			CLEAN_30,
			CLEAN_40,
			CLEAN_50,
			CLEAN_60,
			CLEAN_70,
			CLEAN_80,
			CLEAN_90,
			CLEAN_100,
			CLEAN_110,
			CLEAN_120,
			CLEAN_130,
			
			ULD_WAIT_10, // Unload 위치로 가서 기다림
            ULD_WAIT_11,
            ULD_WAIT_20,
			ULD_WAIT_30,
			ULD_WAIT_40,
			ULD_WAIT_50,
            ULD_WAIT_60,
            ULD_WAIT_61,
            ULD_WAIT_PICK_WAIT,
			ULD_WAIT_70,
			ULD_WAIT_80,
			ULD_WAIT_90,
			ULD_WAIT_100,
			ULD_WAIT_110,
			ULD_WAIT_120,
			ULD_WAIT_130,
			ULD_WAIT_140,
			ULD_WAIT_150,
		}

		public enum PosY
		{
			LOAD,
			UNLOAD,
			CLEAN,
		}

		public enum PosSize
		{
			READY,
			LOAD,
            UNLOAD,
            GRIP,
        }
		public enum PosR
		{
			READY,
			PNP,
			CLEAN,
		}


		#endregion

		#region [Variables]
		private	SMM<Cmd>	m_smmHome;
		private	SMM<Cmd>	m_smmLoadWait;
		private	SMM<Cmd>	m_smmClean;
		private	SMM<Cmd>	m_smmUnloadWait;

		public	Cmd	smmHome			{ get => m_smmHome.CurStep; }
		public	Cmd	smmLoadWait		{ get => m_smmLoadWait.CurStep; }
		public	Cmd	smmClean		{ get => m_smmClean.CurStep; }
		public	Cmd	smmUnloadWait	{ get => m_smmUnloadWait.CurStep; }
		
		private	int		m_nPartNo			= 0;
		private	int		m_nOffsetActAlarm	= 0;
		private	int		m_nOffsetSeqAlarm	= 0;
		private	int		m_nCntCleanRepeat	= 0;

		private	bool	m_IsGrip			= false; // 현재 버퍼가 제품을 잡고 있는지를 저장, 오직 다른 곳에서 함수로만 확인 가능 
		private	bool	m_IsClean			= false;
		private	bool	m_IsGripReady		= false;
		private	bool	m_IsMtHome			= true;
		private	int		m_nRepeatCount		= 0;	// Insp repeat 쪽에서만 사용 가능 
		private	int		m_nCurPos			= 0;	
		#endregion

		#region 재정의
		private	DI	DI_ExistSen1		= DI.LDBuf1Grip1ExistSen;
		private	DI	DI_ExistSen2		= DI.LDBuf1Grip2ExistSen;
		private	DI	DI_ExistSen3		= DI.LDBuf1Grip3ExistSen;
		private	DI	DI_ExistSen4		= DI.LDBuf1Grip4ExistSen;
		
		private	DI	DI_TableUp1Cyl		= DI.LDBuf1TableUp1Cyl;
		private	DI	DI_TableDown1Cyl	= DI.LDBuf1TableDown1Cyl;
		private	DI	DI_TableUp2Cyl		= DI.LDBuf1TableUp2Cyl;
		private	DI	DI_TableDown2Cyl	= DI.LDBuf1TableDown2Cyl;
		private	DI	DI_GripOpen1Cyl		= DI.LDBuf1Grip1OpenCyl;
		private	DI	DI_GripOpen2Cyl		= DI.LDBuf1Grip2OpenCyl;
		private	DI	DI_GripOpen3Cyl		= DI.LDBuf1Grip3OpenCyl;
		private	DI	DI_GripOpen4Cyl		= DI.LDBuf1Grip4OpenCyl;

		private	DI	DI_CleanUpCyl		= DI.LDBuf1CleanUpCyl;
		private	DI	DI_CleanDnCyl		= DI.LDBuf1CleanDnCyl;
		private	DI	DI_CleanAirFwdCyl	= DI.LDBuf1CleanAirFwdCyl;
		private	DI	DI_CleanAirBwdCyl	= DI.LDBuf1CleanAirBwdCyl;
		private	DI	DI_DuctUpCyl		= DI.LDBuf1DuctUpCyl;
		private	DI	DI_DuctDnCyl		= DI.LDBuf1DuctDnCyl;
		private	DI	DI_DustOpenCyl		= DI.LDBuf1DustOpenCyl;
		private	DI	DI_DustCloseCyl		= DI.LDBuf1DustCloseCyl;
		
		private	DO	DO_TableUpCyl		= DO.LDBuf1TableUpCyl;
		private	DO	DO_TableDownCyl		= DO.LDBuf1TableDownCyl;
		private	DO	DO_GripCloseCyl		= DO.LDBuf1GripCloseCyl;
		private	DO	DO_GripOpenCyl		= DO.LDBuf1GripOpenCyl;

		private	DO	DO_CleanUpCyl		= DO.LDBuf1CleanUpCyl;
		private	DO	DO_CleanDnCyl		= DO.LDBuf1CleanDnCyl;
		private	DO	DO_CleanAirFwdCyl	= DO.LDBuf1CleanAirFwdCyl;
		private	DO	DO_CleanAirBwdCyl	= DO.LDBuf1CleanAirBwdCyl;
		private	DO	DO_DuctUpCyl		= DO.LDBuf1DuctUpCyl;
		private	DO	DO_DuctDnCyl		= DO.LDBuf1DuctDnCyl;
		private	DO	DO_BlowIonizer		= DO.LDBuf1BlowIonizer;
		private	DO	DO_CleanAirBlow		= DO.LDBuf1CleanAirBlow;
		private	DO	DO_DustCyl			= DO.LDBuf1DustCyl;

		private	MT	MT_Y				= MT.LDBuf1Y;
		private	MT	MT_Grip				= MT.LDBuf1Grip;
		private	MT	MT_R				= MT.LDBuf1R;

		private	FlowPart	FlowBuffer	= FlowPart.LDBuffer1;
		
		private	void	IODefine(int nNo)
		{
			if (nNo == 1)
			{
				DI_ExistSen1		= DI.LDBuf2Grip1ExistSen;
				DI_ExistSen2		= DI.LDBuf2Grip2ExistSen;
				DI_ExistSen3		= DI.LDBuf2Grip3ExistSen;
				DI_ExistSen4		= DI.LDBuf2Grip4ExistSen;
				
				DI_TableUp1Cyl		= DI.LDBuf2TableUp1Cyl;
				DI_TableDown1Cyl	= DI.LDBuf2TableDown1Cyl;
				DI_TableUp2Cyl		= DI.LDBuf2TableUp2Cyl;
				DI_TableDown2Cyl	= DI.LDBuf2TableDown2Cyl;
				
				DI_GripOpen1Cyl		= DI.LDBuf2Grip1OpenCyl;
				DI_GripOpen2Cyl		= DI.LDBuf2Grip2OpenCyl;
				DI_GripOpen3Cyl		= DI.LDBuf2Grip3OpenCyl;
				DI_GripOpen4Cyl		= DI.LDBuf2Grip4OpenCyl;

				DI_CleanUpCyl		= DI.LDBuf2CleanUpCyl;
				DI_CleanDnCyl		= DI.LDBuf2CleanDnCyl;
				DI_CleanAirFwdCyl	= DI.LDBuf2CleanAirFwdCyl;
				DI_CleanAirBwdCyl	= DI.LDBuf2CleanAirBwdCyl;
				DI_DuctUpCyl		= DI.LDBuf2DuctUpCyl;
				DI_DuctDnCyl		= DI.LDBuf2DuctDnCyl;
				DI_DustOpenCyl		= DI.LDBuf2DustOpenCyl;
				DI_DustCloseCyl		= DI.LDBuf2DustCloseCyl;
				
				DO_TableUpCyl		= DO.LDBuf2TableUpCyl;
				DO_TableDownCyl		= DO.LDBuf2TableDownCyl;
				
				DO_GripCloseCyl		= DO.LDBuf2GripCloseCyl;
				DO_GripOpenCyl		= DO.LDBuf2GripOpenCyl;

				DO_CleanUpCyl		= DO.LDBuf2CleanUpCyl;
				DO_CleanDnCyl		= DO.LDBuf2CleanDnCyl;
				DO_CleanAirFwdCyl	= DO.LDBuf2CleanAirFwdCyl;
				DO_CleanAirBwdCyl	= DO.LDBuf2CleanAirBwdCyl;
				DO_DuctUpCyl		= DO.LDBuf2DuctUpCyl;
				DO_DuctDnCyl		= DO.LDBuf2DuctDnCyl;
				DO_BlowIonizer		= DO.LDBuf2BlowIonizer;
				DO_CleanAirBlow		= DO.LDBuf2CleanAirBlow;
				DO_DustCyl			= DO.LDBuf2DustCyl;
				
				MT_Y				= MT.LDBuf2Y;
				MT_Grip				= MT.LDBuf2Grip;
				MT_R				= MT.LDBuf2R;

				FlowBuffer			= FlowPart.LDBuffer2;
			}
		}
		#endregion


		public void		Init(int BufferNo)
		{
			try
			{
				m_nPartNo		= BufferNo;

				m_smmHome		= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.LDBUFFER1_HOME+m_nPartNo));
				m_smmLoadWait	= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.LDBUFFER1_LOADWAIT+m_nPartNo));
				m_smmClean		= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.LDBUFFER1_CLEAN+m_nPartNo));
				m_smmUnloadWait	= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.LDBUFFER1_UNLOADWAIT+m_nPartNo));
			
				lstSMM.Add(m_smmHome);
				lstSMM.Add(m_smmLoadWait);
				lstSMM.Add(m_smmClean);
				lstSMM.Add(m_smmUnloadWait);

				IODefine(m_nPartNo);
				m_nOffsetActAlarm	= m_nPartNo * (ALM.ERROR_LOAD_BUFFER2_DUST_CYL_BACKWARD - ALM.ERROR_LOAD_BUFFER1_DUST_CYL_BACKWARD);
				m_nOffsetSeqAlarm	= m_nPartNo * (ALM.ERROR_SEQ_LOAD_BUFFER2_PROD_MISMATCH - ALM.ERROR_SEQ_LOAD_BUFFER1_PROD_MISMATCH);

				#region - Motor            
				arrMt = new EnumArray<Axis, CMotor>(Axis.Max);
				arrMt[Axis.Y]		= G.cKernel.Motor(MT_Y);
				arrMt[Axis.SIZE]	= G.cKernel.Motor(MT_Grip);
				arrMt[Axis.Rotate]	= G.cKernel.Motor(MT_R);
				#endregion

				#region - Actuator
				arrAct = new EnumArray<Act, Actuator>(Act.Max);
				
				arrAct[Act.TableUpDn] = new CDoubleSen();
				arrAct[Act.TableUpDn].SetSenser1(new List<DI>(new DI[] { DI_TableUp1Cyl ,  DI_TableUp2Cyl}));
				arrAct[Act.TableUpDn].SetSenser2(new List<DI>(new DI[] { DI_TableDown1Cyl, DI_TableDown2Cyl }));
				arrAct[Act.TableUpDn].SetSol(DO_TableUpCyl, DO_TableDownCyl);
				arrAct[Act.TableUpDn].Setting(ALM.ERROR_LOAD_BUFFER1_TABLE_UPDN_CYL_UP + m_nOffsetActAlarm, ALM.ERROR_LOAD_BUFFER1_TABLE_UPDN_CYL_DOWN + m_nOffsetActAlarm, ErrSenPos.Ori, SenPos.NONE, 200, 5000);
				
				arrAct[Act.Grip] = new CDoubleSen();
				arrAct[Act.Grip].SetSenser1(new List<DI>(new DI[] { }));
				arrAct[Act.Grip].SetSenser2(new List<DI>(new DI[] { DI_GripOpen1Cyl, DI_GripOpen2Cyl , DI_GripOpen3Cyl , DI_GripOpen4Cyl }));
				arrAct[Act.Grip].SetSol(DO_GripCloseCyl, DO_GripOpenCyl);
				arrAct[Act.Grip].Setting(ALM.ERROR_LOAD_BUFFER1_GRIP_CYL_CLOSE + m_nOffsetActAlarm, ALM.ERROR_LOAD_BUFFER1_GRIP_CYL_OPEN + m_nOffsetActAlarm, ErrSenPos.Ori, SenPos.NONE, 500, 5000);
				
				arrAct[Act.CleanUpDn] = new CDoubleSen();
				arrAct[Act.CleanUpDn].SetSenser1(new List<DI>(new DI[] { DI_CleanUpCyl }));
				arrAct[Act.CleanUpDn].SetSenser2(new List<DI>(new DI[] { DI_CleanDnCyl }));
				arrAct[Act.CleanUpDn].SetSol(DO_CleanUpCyl, DO_CleanDnCyl);
				arrAct[Act.CleanUpDn].Setting(ALM.ERROR_LOAD_BUFFER1_CLEAN_UPDN_CYL_UP + m_nOffsetActAlarm, ALM.ERROR_LOAD_BUFFER1_CLEAN_UPDN_CYL_DOWN + m_nOffsetActAlarm, ErrSenPos.Ori, SenPos.NONE, 200, 5000);
				
				arrAct[Act.CleanAirMove] = new CDoubleSen();
				arrAct[Act.CleanAirMove].SetSenser1(new List<DI>(new DI[] { DI_CleanAirFwdCyl }));
				arrAct[Act.CleanAirMove].SetSenser2(new List<DI>(new DI[] { DI_CleanAirBwdCyl }));
				arrAct[Act.CleanAirMove].SetSol(DO_CleanAirFwdCyl, DO_CleanAirBwdCyl);
				arrAct[Act.CleanAirMove].Setting(ALM.ERROR_LOAD_BUFFER1_CLEAN_AIR_CYL_FORWARD + m_nOffsetActAlarm, ALM.ERROR_LOAD_BUFFER1_CLEAN_AIR_CYL_BACKWARD + m_nOffsetActAlarm, ErrSenPos.Ori, SenPos.NONE, 200, 5000);

				arrAct[Act.DuctUpDn] = new CDoubleSen();
				arrAct[Act.DuctUpDn].SetSenser1(new List<DI>(new DI[] { DI_DuctUpCyl }));
				arrAct[Act.DuctUpDn].SetSenser2(new List<DI>(new DI[] { DI_DuctDnCyl }));
				arrAct[Act.DuctUpDn].SetSol(DO_DuctUpCyl, DO_DuctDnCyl);
				arrAct[Act.DuctUpDn].Setting(ALM.ERROR_LOAD_BUFFER1_DUCT_UPDN_CYL_UP + m_nOffsetActAlarm, ALM.ERROR_LOAD_BUFFER1_DUCT_UPDN_CYL_DOWN + m_nOffsetActAlarm, ErrSenPos.Ori, SenPos.NONE, 200, 3000);

				arrAct[Act.BlowIonizer] = new CSingleNoSen();
				arrAct[Act.BlowIonizer].SetSol(DO_BlowIonizer);
				arrAct[Act.BlowIonizer].Setting(ALM.NOT_ALARM, ALM.NOT_ALARM);

				arrAct[Act.CleanAirBlow] = new CSingleNoSen();
				arrAct[Act.CleanAirBlow].SetSol(DO_CleanAirBlow);
				arrAct[Act.CleanAirBlow].Setting(ALM.NOT_ALARM, ALM.NOT_ALARM);

				arrAct[Act.DustValve] = new CSingleSen();
				arrAct[Act.DustValve].SetSenser1(new List<DI>(new DI[] { DI_DustOpenCyl }));
				arrAct[Act.DustValve].SetSenser2(new List<DI>(new DI[] { DI_DustCloseCyl }));
				arrAct[Act.DustValve].SetSol(DO_DustCyl);
				arrAct[Act.DustValve].Setting(ALM.ERROR_LOAD_BUFFER1_DUST_CYL_FORWARD + m_nOffsetActAlarm, ALM.ERROR_LOAD_BUFFER1_DUST_CYL_BACKWARD + m_nOffsetActAlarm, ErrSenPos.Ori, SenPos.NONE, 500, 5000);
				#endregion

				base.Initialize();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}


		
		public	bool	HomeStart(bool IsMtHome = true)			{m_IsMtHome = IsMtHome;  return SetSeqRun(ref m_smmHome); }

		public	bool	LoadWaitStart()		{ return SetSeqRun(ref m_smmLoadWait); }

		public	bool	CleanStart()		{ return SetSeqRun(ref m_smmClean); }

		public	bool	UnloadWaitStart()	{ return SetSeqRun(ref m_smmUnloadWait); }



		public	bool	IsHomeReady()		{ return m_smmHome.IsReady; }

		public	bool	IsLoadWaitReady()	{ return m_smmLoadWait.IsReady; }

		public	bool	IsCleanReady()		{ return m_smmClean.IsReady; }

		public	bool	IsUnloadWaitReady()	{ return m_smmUnloadWait.IsReady; }

		public	bool	IsBufferGrip()		{ return m_IsGrip; }

		public	bool	IsBufferGripReady()	{ return m_IsGripReady; }

		public	bool	IsLDWait()			{ return m_smmLoadWait.CurStep == Cmd.LD_WAIT_PLACE_WAIT; }

		public	bool	IsULDWait()			{ return m_smmUnloadWait.CurStep == Cmd.ULD_WAIT_PICK_WAIT; }

		public	bool	IsClean()
		{
			if (arrAct[Act.CleanAirMove].IsPos(SenPos.BWD) == false  ||  m_IsClean == false)
			{
				return false;
			}

			return true;
		}

		private ProdExist CheckProdExist()
		{
			try
			{
				FlowPart flowPart = FlowBuffer;
				DI ExistSenStart = DI_ExistSen1;

				bool[] bexist = SM.FMP[flowPart].GetProdExist();
				bool[] sen = new bool[bexist.Length];

				for (int i = 0; i < bexist.Length; i++)
					sen[i] = G.DryRunIsBIn(ExistSenStart + i, bexist[i]);

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
		
		private	bool	SetSeqRun(ref SMM<Cmd> smm)
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



		public void Run()
		{
			try
			{
				UpdateAct(Seq.LoadBuffer1 + m_nPartNo);

				if (SM.SHM.IsBitData(BIT.Error) == true || SM.SHM.IsBitData(BIT.Pause) == true)
				{
					if (arrAct[Act.BlowIonizer].IsPos(SenPos.ON) == true)
					{
						arrAct[Act.BlowIonizer].SetPos(SenPos.OFF);
					}
					if (arrAct[Act.CleanAirBlow].IsPos(SenPos.ON) == true)
					{
						arrAct[Act.CleanAirBlow].SetPos(SenPos.OFF);
					}

					return;
				}

				Home(m_smmHome);
				LoadWait(m_smmLoadWait);
				Clean(m_smmClean);
				UnloadWait(m_smmUnloadWait);

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
							if (arrAct[Act.DustValve].IsPos(SenPos.OFF) == false  ||
								arrAct[Act.CleanAirBlow].IsPos(SenPos.OFF) == false  || 
								arrAct[Act.BlowIonizer].IsPos(SenPos.OFF) == false  ||
								arrAct[Act.CleanAirMove].IsPos(SenPos.BWD) == false)
							{
								arrAct[Act.DustValve].SetPos(SenPos.OFF);
								arrAct[Act.CleanAirBlow].SetPos(SenPos.OFF);
								arrAct[Act.BlowIonizer].SetPos(SenPos.OFF);
								arrAct[Act.CleanAirMove].SetPos(SenPos.BWD);
							}
							
							if (arrAct[Act.DuctUpDn].IsPos(SenPos.DOWN) == false ||
								arrAct[Act.CleanUpDn].IsPos(SenPos.UP) == false)
							{
								arrAct[Act.DuctUpDn].SetPos(SenPos.DOWN);
								arrAct[Act.CleanUpDn].SetPos(SenPos.UP);
								break;
							}

							smm.Next(Cmd.HOME_20);
						}
						break;

					case Cmd.HOME_20:
						{
							if (SM.FMP.IsFlow(FlowBuffer) == true)		smm.Next(Cmd.HOME_30);
							else										smm.Next(Cmd.HOME_40);

						}
						break;

					case Cmd.HOME_30:
						{	
							if (arrAct[Act.Grip].IsPos(SenPos.TABLE_GRIP) == false )
							{
								arrAct[Act.Grip].SetPos(SenPos.TABLE_GRIP);
								break;
							}
							
							switch (CheckProdExist() )
							{
								case ProdExist.Exist : 	smm.Next(Cmd.HOME_40);	break;
                                case ProdExist.Mismatch:
                                    AddAlarm(ALM.ERROR_SEQ_LOAD_BUFFER1_PROD_MISMATCH + m_nOffsetSeqAlarm);
                                    smm.Next(Cmd.END);
                                    break;
                                case ProdExist.None:
                                    AddAlarm(ALM.ERROR_SEQ_LOAD_BUFFER1_PROD_NONE + m_nOffsetSeqAlarm);
                                    smm.Next(Cmd.END);
                                    break;
                            }

						}
						break;

					case Cmd.HOME_40:
						{
							if (arrAct[Act.TableUpDn].IsPos(SenPos.DOWN) == false )
							{
								arrAct[Act.TableUpDn].SetPos(SenPos.DOWN);
								break;
							}
							
							if (m_IsMtHome == false)
							{
								smm.Next(Cmd.HOME_80);
								break;
							}

							smm.Next(Cmd.HOME_50);
						}
						break;

					case Cmd.HOME_50:
						{
							arrMt[Axis.SIZE].ClearHome();
							arrMt[Axis.SIZE].StartHoming();

							smm.Next(Cmd.HOME_60);
						}
						break;

					case Cmd.HOME_60:
						{
							if (arrMt[Axis.SIZE].IsHomeDone() == false || arrMt[Axis.SIZE].IsRdy() == false) break;
							
							arrMt[Axis.Y].ClearHome();
							arrMt[Axis.Y].StartHoming();
							
							arrMt[Axis.Rotate].ClearHome();
							arrMt[Axis.Rotate].StartHoming();

							smm.Next(Cmd.HOME_70);
						}
						break;

					case Cmd.HOME_70:
						{
							if (arrMt[Axis.Y].IsHomeDone() == false || arrMt[Axis.Y].IsRdy() == false) break;
							if (arrMt[Axis.Rotate].IsHomeDone() == false || arrMt[Axis.Rotate].IsRdy() == false) break;
							
							smm.Next(Cmd.HOME_80);
						}
						break;

						
					case Cmd.HOME_80:
						{
							
							if (SM.FMP.IsFlow(FlowBuffer) == true)
							{
								if (arrAct[Act.Grip].IsPos(SenPos.TABLE_GRIP) == false)
								{
									arrAct[Act.Grip].SetPos(SenPos.TABLE_GRIP);
									break;
								}
							}
							else
							{
								if (arrAct[Act.Grip].IsPos(SenPos.TABLE_RELEASE) == false)
								{
									arrAct[Act.Grip].SetPos(SenPos.TABLE_RELEASE);
									break;
								}
							}

							smm.Next(Cmd.HOME_90);
						}
						break;
					case Cmd.HOME_90:
						{
							if (arrMt[Axis.SIZE].IsRdy((int)PosSize.READY) == false)
							{
								arrMt[Axis.SIZE].MoveIndex((int)PosSize.READY);
								break;
							}

							if (arrMt[Axis.Y].IsInPosIndex((int)PosY.LOAD) == false ||
								arrMt[Axis.Rotate].IsRdy((int)PosR.PNP) == false)
							{
								arrMt[Axis.Y].MoveIndex((int)PosY.LOAD);
								arrMt[Axis.Rotate].MoveIndex((int)PosR.PNP);
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

		private void LoadWait(SMM<Cmd> smm)
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

					case Cmd.LD_WAIT_10:
						{
							if (arrAct[Act.Grip].IsPos(SenPos.TABLE_GRIP) == false)
							{
								arrAct[Act.Grip].SetPos(SenPos.TABLE_GRIP);
								break;
							}

                            smm.Next(Cmd.LD_WAIT_11);
                        }
						break;
                    case Cmd.LD_WAIT_11:
                        {
                            if (smm.Delayms(300) == false) break;

                            switch (CheckProdExist())
                            {
                                case ProdExist.Mismatch:
                                    AddAlarm(ALM.ERROR_SEQ_LOAD_BUFFER1_PROD_MISMATCH + m_nOffsetSeqAlarm);
                                    smm.Next(Cmd.END);
                                    break;
                                case ProdExist.Exist:
                                    AddAlarm(ALM.ERROR_SEQ_LOAD_BUFFER1_PROD_EXIST + m_nOffsetSeqAlarm);
                                    smm.Next(Cmd.END);
                                    break;
                                default:
                                    smm.Next(Cmd.LD_WAIT_20);
                                    break;
                            }
                        }
                        break;


                    case Cmd.LD_WAIT_20:
						{
							m_IsClean = false;
							m_IsGrip = false;

							smm.Next(Cmd.LD_WAIT_30);
						}
						break;

					case Cmd.LD_WAIT_30:
						{
							if (arrAct[Act.DuctUpDn].IsPos(SenPos.DOWN) == false ||
								arrAct[Act.CleanUpDn].IsPos(SenPos.UP) == false ||
								arrAct[Act.TableUpDn].IsPos(SenPos.DOWN) == false  ||
								arrAct[Act.Grip].IsPos(SenPos.TABLE_RELEASE) == false )
							{
								arrAct[Act.DuctUpDn].SetPos(SenPos.DOWN);
								arrAct[Act.CleanUpDn].SetPos(SenPos.UP);
								arrAct[Act.TableUpDn].SetPos(SenPos.DOWN);
								arrAct[Act.Grip].SetPos(SenPos.TABLE_RELEASE);
								break;
							}
							
							if (arrAct[Act.CleanAirBlow].IsPos(SenPos.OFF) == false)
							{
								arrAct[Act.CleanAirBlow].SetPos(SenPos.OFF);
								break;
							}

							smm.Next(Cmd.LD_WAIT_40);
						}
						break;

					case Cmd.LD_WAIT_40:
						{
							if (arrMt[Axis.SIZE].IsRdy((int)PosSize.READY) == false)
                            {
                                arrMt[Axis.Y].MoveIndex((int)PosY.LOAD);
                                arrMt[Axis.SIZE].MoveIndex((int)PosSize.READY);
								break;
							}

							smm.Next(Cmd.LD_WAIT_50);
						}
						break;

					case Cmd.LD_WAIT_50:
						{
							if (arrMt[Axis.Rotate].IsRdy((int)PosR.PNP) == false)
							{
								arrMt[Axis.Rotate].MoveIndex((int)PosR.PNP);
								break;
							}
							
							if (arrMt[Axis.SIZE].IsRdy((int)PosSize.LOAD) == false  ||
								arrMt[Axis.Y].IsRdy((int)PosY.LOAD) == false)
							{
								arrMt[Axis.SIZE].MoveIndex((int)PosSize.LOAD);
								arrMt[Axis.Y].MoveIndex((int)PosY.LOAD);
								break;
							}

							if (arrAct[Act.Grip].IsPos(SenPos.TABLE_RELEASE) == false ||
								arrAct[Act.TableUpDn].IsPos(SenPos.UP) == false)
							{
								arrAct[Act.Grip].SetPos(SenPos.TABLE_RELEASE);
								arrAct[Act.TableUpDn].SetPos(SenPos.UP);
								break;
							}
							
							smm.Next(Cmd.LD_WAIT_PLACE_WAIT);
						}
						break;

					case Cmd.LD_WAIT_PLACE_WAIT: // 놓는거 기다리는 중
						{
							if (G.seqLoadPnP.IsPlaceBuffer(m_nPartNo) == false) break;

							smm.Next(Cmd.LD_WAIT_60);
						}
						break;

					case Cmd.LD_WAIT_60:
						{
							if (smm.Delayms(100) == false) break;

                            if (arrMt[Axis.SIZE].IsRdy((int)PosSize.GRIP) == false)
                            {
                                arrMt[Axis.SIZE].MoveIndex((int)PosSize.GRIP);
                                break;
                            }


                            if (arrAct[Act.Grip].IsPos(SenPos.TABLE_GRIP) == false )
							{
								arrAct[Act.Grip].SetPos(SenPos.TABLE_GRIP);
								break;
							}

							m_IsGrip = true;

							smm.Next(Cmd.LD_WAIT_70);
						}
						break;

					case Cmd.LD_WAIT_70:
						{
							if (smm.Delayms(50) == false) break;

							if (G.seqLoadPnP.IsPlaceBufReady() == false) break;

							if (CheckProdExist() == ProdExist.Mismatch)
							{
								AddAlarm(ALM.ERROR_SEQ_LOAD_BUFFER1_PROD_MISMATCH + m_nOffsetSeqAlarm);
								break;
							}

                            if (arrMt[Axis.SIZE].IsRdy((int)PosSize.LOAD) == false)
                            {
                                arrMt[Axis.SIZE].MoveIndex((int)PosSize.LOAD);
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

		private void Clean(SMM<Cmd> smm)
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
								smm.Next(Cmd.CLEAN_10);
							}
						}
						break;

					case Cmd.CLEAN_10:
						{
							m_IsClean = false;

							if (arrAct[Act.BlowIonizer].IsPos(SenPos.OFF) == false ||
								arrAct[Act.DustValve].IsPos(SenPos.OFF) == false ||
								arrAct[Act.CleanAirBlow].IsPos(SenPos.OFF) == false)
							{
								arrAct[Act.BlowIonizer].SetPos(SenPos.OFF);
								arrAct[Act.DustValve].SetPos(SenPos.OFF);
								arrAct[Act.CleanAirBlow].SetPos(SenPos.OFF);

								break;
							}

                            if (arrAct[Act.Grip].IsPos(SenPos.TABLE_GRIP) == false)
                            {
                                arrAct[Act.Grip].SetPos(SenPos.TABLE_GRIP);
                                break;
                            }

                            smm.Next(Cmd.CLEAN_20);
						}
						break;

					case Cmd.CLEAN_20:
						{
                            if (smm.Delayms(300) == false) break;

							switch (CheckProdExist())
							{
								case ProdExist.Exist:
									m_IsGrip = true;
									smm.Next(Cmd.CLEAN_40);
									break;
								case ProdExist.Mismatch:
									AddAlarm(ALM.ERROR_SEQ_LOAD_BUFFER1_PROD_MISMATCH + m_nOffsetSeqAlarm);
									smm.Next(Cmd.END);
									break;
								case ProdExist.None:
									AddAlarm(ALM.ERROR_SEQ_LOAD_BUFFER1_PROD_NONE + m_nOffsetSeqAlarm);
									smm.Next(Cmd.END);
									break;
							}

							

							smm.Next(Cmd.CLEAN_30);
						}
						break;

					case Cmd.CLEAN_30:
                        {

                            if (arrAct[Act.DuctUpDn].IsPos(SenPos.DOWN) == false ||
								arrAct[Act.CleanUpDn].IsPos(SenPos.UP) == false ||
								arrAct[Act.CleanAirMove].IsPos(SenPos.BWD) == false||
								arrAct[Act.TableUpDn].IsPos(SenPos.DOWN) == false)
							{
								arrAct[Act.DuctUpDn].SetPos(SenPos.DOWN);
								arrAct[Act.CleanUpDn].SetPos(SenPos.UP);
								arrAct[Act.CleanAirMove].SetPos(SenPos.BWD);
								arrAct[Act.TableUpDn].SetPos(SenPos.DOWN);
								break;
							}


                            arrMt[Axis.Y].MoveIndex((int)PosY.CLEAN);
                            smm.Next(Cmd.CLEAN_40);
						}
						break;

					case Cmd.CLEAN_40:
						{
							if (arrMt[Axis.SIZE].IsRdy((int)PosSize.READY) == false)
                            {
                                arrMt[Axis.SIZE].MoveIndex((int)PosSize.READY);
								break;
							}

							smm.Next(Cmd.CLEAN_50);
						}
						break;

					case Cmd.CLEAN_50:
						{
							if (NDm.Read(eNDM.MemoryCleanCount) < 1)
							{
								smm.Next(Cmd.CLEAN_100);
								break;
							}

							if (arrMt[Axis.Rotate].IsRdy((int)PosR.CLEAN) == false  ||
								arrMt[Axis.Y].IsRdy((int)PosY.CLEAN) == false)
							{
								arrMt[Axis.Rotate].MoveIndex((int)PosR.CLEAN);
								arrMt[Axis.Y].MoveIndex((int)PosY.CLEAN);
								break;
							}

							smm.Next(Cmd.CLEAN_60);
						}
						break;

					case Cmd.CLEAN_60:
						{
                            if (arrAct[Act.DuctUpDn].IsPos(SenPos.UP) == false ||
                                arrAct[Act.CleanUpDn].IsPos(SenPos.DOWN) == false)
                            {
                                arrAct[Act.DuctUpDn].SetPos(SenPos.UP);
                                arrAct[Act.CleanUpDn].SetPos(SenPos.DOWN);
                                break;
                            }

                            //if (arrAct[Act.CleanUpDn].IsPos(SenPos.DOWN) == false)
                            //{
                            //    arrAct[Act.CleanUpDn].SetPos(SenPos.DOWN);
                            //    break;
                            //}
                            smm.Next(Cmd.CLEAN_70);
						}
						break;

					case Cmd.CLEAN_70:
						{
							if (arrAct[Act.BlowIonizer].IsPos(SenPos.ON) == false ||
								arrAct[Act.DustValve].IsPos(SenPos.ON) == false ||
                                arrAct[Act.CleanAirBlow].IsPos(SenPos.ON) == false)
							{
								arrAct[Act.BlowIonizer].SetPos(SenPos.ON);
								arrAct[Act.DustValve].SetPos(SenPos.ON);
                                arrAct[Act.CleanAirBlow].SetPos(SenPos.ON);
                                break;
							}
                            m_nCntCleanRepeat = NDm.Read(eNDM.MemoryCleanCount);
							m_nRepeatCount = 0;

							smm.Next(Cmd.CLEAN_80);
						}
						break;

					case Cmd.CLEAN_80:
						{
							if (m_nRepeatCount < m_nCntCleanRepeat)
							{
								if (arrAct[Act.CleanAirMove].IsPos(SenPos.FWD) == false)
								{
									arrAct[Act.CleanAirMove].SetPos(SenPos.FWD);
									break;
								}

								smm.Next(Cmd.CLEAN_90);
							}
							else
							{
								if (arrAct[Act.BlowIonizer].IsPos(SenPos.OFF) == false ||
									arrAct[Act.DustValve].IsPos(SenPos.OFF) == false)
								{
									arrAct[Act.BlowIonizer].SetPos(SenPos.OFF);
									arrAct[Act.DustValve].SetPos(SenPos.OFF);
									break;
								}

								if (arrAct[Act.CleanAirBlow].IsPos(SenPos.OFF) == false)
								{
									arrAct[Act.CleanAirBlow].SetPos(SenPos.OFF);
									break;
								}

								if (arrAct[Act.DuctUpDn].IsPos(SenPos.DOWN) == false ||
									arrAct[Act.CleanUpDn].IsPos(SenPos.UP) == false)
								{
									arrAct[Act.DuctUpDn].SetPos(SenPos.DOWN);
									arrAct[Act.CleanUpDn].SetPos(SenPos.UP);
									break;
								}

								smm.Next(Cmd.CLEAN_100);
							}
						}
						break;

					case Cmd.CLEAN_90:
						{
							if (arrAct[Act.CleanAirMove].IsPos(SenPos.BWD) == false)
							{
								arrAct[Act.CleanAirMove].SetPos(SenPos.BWD);
								break;
							}

							if (arrAct[Act.CleanAirBlow].IsPos(SenPos.ON) == false)
							{
								arrAct[Act.CleanAirBlow].SetPos(SenPos.ON);
								break;
							}

							m_nRepeatCount++;

							smm.Next(Cmd.CLEAN_80);
						}
						break;

					case Cmd.CLEAN_100:
						{
							m_IsClean = true;

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

		private void UnloadWait(SMM<Cmd> smm)
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

                    case Cmd.ULD_WAIT_10:
                        {
                            
                            if (arrAct[Act.Grip].IsPos(SenPos.TABLE_GRIP) == false)
                            {
                                arrAct[Act.Grip].SetPos(SenPos.TABLE_GRIP);
                                break;
                            }
                            m_IsClean = false;

                            smm.Next(Cmd.ULD_WAIT_11);
                        }
                        break;


                    case Cmd.ULD_WAIT_11:
                        {
                            if (smm.Delayms(300) == false) break;


                            switch (CheckProdExist())
                            {
                                case ProdExist.Exist:
                                    m_IsGrip = true;
                                    smm.Next(Cmd.ULD_WAIT_20);
                                    break;
                                case ProdExist.Mismatch:
                                    AddAlarm(ALM.ERROR_SEQ_LOAD_BUFFER1_PROD_MISMATCH + m_nOffsetSeqAlarm);
                                    smm.Next(Cmd.END);
                                    break;
                                case ProdExist.None:
                                    AddAlarm(ALM.ERROR_SEQ_LOAD_BUFFER1_PROD_NONE + m_nOffsetSeqAlarm);
                                    smm.Next(Cmd.END);
                                    break;
                            }
                            
                        }
                        break;

                    case Cmd.ULD_WAIT_20:
						{
							if (arrAct[Act.BlowIonizer].IsPos(SenPos.OFF) == false ||
								arrAct[Act.DustValve].IsPos(SenPos.OFF) == false ||
								arrAct[Act.CleanAirBlow].IsPos(SenPos.OFF) == false)
							{
								arrAct[Act.BlowIonizer].SetPos(SenPos.OFF);
								arrAct[Act.DustValve].SetPos(SenPos.OFF);
								arrAct[Act.CleanAirBlow].SetPos(SenPos.OFF);
								break;
							}

							if (arrAct[Act.DuctUpDn].IsPos(SenPos.DOWN) == false ||
								arrAct[Act.TableUpDn].IsPos(SenPos.DOWN) == false ||
								arrAct[Act.CleanUpDn].IsPos(SenPos.UP) == false ||
								arrAct[Act.CleanAirMove].IsPos(SenPos.BWD) == false)
                            {
                                arrMt[Axis.SIZE].MoveIndex((int)PosSize.READY);
                                arrAct[Act.DuctUpDn].SetPos(SenPos.DOWN);
								arrAct[Act.TableUpDn].SetPos(SenPos.DOWN);
								arrAct[Act.CleanUpDn].SetPos(SenPos.UP);
								arrAct[Act.CleanAirMove].SetPos(SenPos.BWD);
								break;
							}

                            arrMt[Axis.Y].MoveIndex((int)PosY.UNLOAD);
                            smm.Next(Cmd.ULD_WAIT_30);
						}
						break;

					case Cmd.ULD_WAIT_30:
						{
							if (arrMt[Axis.SIZE].IsRdy((int)PosSize.READY) == false)
							{
								arrMt[Axis.SIZE].MoveIndex((int)PosSize.READY);
								break;
							}

							smm.Next(Cmd.ULD_WAIT_40);
						}
						break;
						
					case Cmd.ULD_WAIT_40:
						{
							if (arrMt[Axis.Rotate].IsRdy((int)PosR.PNP) == false)
							{
								arrMt[Axis.Rotate].MoveIndex((int)PosR.PNP);
								break;
							}
							
							smm.Next(Cmd.ULD_WAIT_50);
						}
						break;


					case Cmd.ULD_WAIT_50:
						{
							if (arrMt[Axis.SIZE].IsRdy((int)PosSize.UNLOAD) == false)
							{
								arrMt[Axis.SIZE].MoveIndex((int)PosSize.UNLOAD);
								break;
							}
							
							if (arrMt[Axis.Y].IsRdy((int)PosY.UNLOAD) == false ||
                                arrAct[Act.TableUpDn].IsPos(SenPos.UP) == false)
							{
                                arrMt[Axis.Y].MoveIndex((int)PosY.UNLOAD);
                                arrAct[Act.TableUpDn].SetPos(SenPos.UP);
								break;
							}

							smm.Next(Cmd.ULD_WAIT_60);
						}
						break;

					case Cmd.ULD_WAIT_60:
						{
							switch (CheckProdExist())
							{
								case ProdExist.Exist:
									m_IsGrip = true;
									m_IsGripReady = false;
									m_nCurPos = arrMt[Axis.SIZE].GetLocalPos();
									smm.Next(Cmd.ULD_WAIT_61);
									break;
								case ProdExist.Mismatch:
									AddAlarm(ALM.ERROR_SEQ_LOAD_BUFFER1_PROD_MISMATCH + m_nOffsetSeqAlarm);
									smm.Next(Cmd.END);
									break;
								case ProdExist.None:
									AddAlarm(ALM.ERROR_SEQ_LOAD_BUFFER1_PROD_NONE + m_nOffsetSeqAlarm);
									smm.Next(Cmd.END);
									break;
							}
						}
						break;


                    case Cmd.ULD_WAIT_61:
                        {

                            smm.Next(Cmd.ULD_WAIT_PICK_WAIT);
                        }
                        break;
                    case Cmd.ULD_WAIT_PICK_WAIT:   // 잡는것 기다리는 중
						{

							if (SM.FMP.IsFlow(FlowBuffer) == true) break;


                            if (arrAct[Act.Grip].IsPos(SenPos.TABLE_RELEASE) == false)
                            {
                                arrAct[Act.Grip].SetPos(SenPos.TABLE_RELEASE);
                                break;
                            }

                            m_IsGrip = false;

                            smm.Next(Cmd.ULD_WAIT_80);
						}
						break;

					case Cmd.ULD_WAIT_80:
						{

							m_IsGripReady = true;

							if (G.seqLoadInspPnP.IsPickReady() == false) break;

                            if (arrAct[Act.Grip].IsPos(SenPos.TABLE_GRIP) == false)
                            {
                                arrAct[Act.Grip].SetPos(SenPos.TABLE_GRIP);
                                break;
                            }

                            switch (CheckProdExist())
							{
								case ProdExist.Exist:
									AddAlarm(ALM.ERROR_SEQ_LOAD_BUFFER1_PROD_EXIST + m_nOffsetSeqAlarm);
									smm.Next(Cmd.END);
									break;
								case ProdExist.Mismatch:
									AddAlarm(ALM.ERROR_SEQ_LOAD_BUFFER1_PROD_MISMATCH + m_nOffsetSeqAlarm);
									smm.Next(Cmd.END);
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
	}
}