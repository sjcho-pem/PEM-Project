using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace H_MarsSSD
{
	public class SeqLoadTransfer : SeqBase<SeqLoadTransfer.Act, SeqLoadTransfer.Axis, SeqLoadTransfer.Cmd>
	{
		#region [ Enum ]
		public enum Act
		{
			Holder,
			Max,
		}
		
		public enum Axis
		{
			X,
			Z,
			Grip,
			HolderLeftZ,
			HolderRightZ,
			Max,
		}

		public enum Cmd
		{
			START	= 10,
			END		= 99,
			
			HOME_10 = 100,
			HOME_11,
			HOME_20,
			HOME_30,
			HOME_40,
			HOME_50,
			HOME_60,
			HOME_70,
			HOME_80,
			HOME_90,
			HOME_100,
			HOME_110,

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
			PLACE_140,
			PLACE_150,

			PICK_10,
			PICK_20,
			PICK_22,
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
			PICK_190,

			PLACE_CONV_10,
			PLACE_CONV_20,
			PLACE_CONV_30,
			PLACE_CONV_40,
			PLACE_CONV_45,
			PLACE_CONV_50,
			PLACE_CONV_60,
			PLACE_CONV_70,
			PLACE_CONV_80,
			PLACE_CONV_90,
			PLACE_CONV_100,
			PLACE_CONV_110,
			PLACE_CONV_120,
			PLACE_CONV_130,
			PLACE_CONV_140,
			PLACE_CONV_150,

			PICK_CONV_10,
			PICK_CONV_20,
			PICK_CONV_30,
			PICK_CONV_35,
			PICK_CONV_40,
			PICK_CONV_50,
			PICK_CONV_60,
			PICK_CONV_70,
			PICK_CONV_80,
			PICK_CONV_90,
			PICK_CONV_100,
			PICK_CONV_110,
            PICK_CONV_120,
            PICK_CONV_121,
            PICK_CONV_130,
			PICK_CONV_131,
			PICK_CONV_132,
			PICK_CONV_140,
			PICK_CONV_BARCODE_SCAN_10,
			PICK_CONV_BARCODE_SCAN_20,
			PICK_CONV_BARCODE_SCAN_30,
			PICK_CONV_BARCODE_SCAN_40,
			PICK_CONV_NG,

			PnP_10,
			PnP_20,
			PnP_30,
			PnP_40,
			PnP_50,
			PnP_60,
			PnP_70,
			PnP_80,
			PnP_90,
			PnP_100,
			PnP_110,
			PnP_120,
			PnP_130,
			PnP_140,
			PnP_150,

			
			READY_10,
			READY_20,
			READY_30,
			READY_40,
			READY_50,
			READY_60,
		}

		public enum PosX
		{
			LOADCONV,
			TRAYCLEAN,
			LOADTABLE1,
			LOADTABLE2,
			UNKNOWN	= -1,
		}

		public enum PosGrip
		{
			RELEASE,
			GRIP,
			GRIPHARD,
		}

		public enum PosZ
		{
			READY,
			LOADCONV,
			LOADCONVLIMIT,
			TRAYCLEAN,
			LOADTABLE1,
			LOADTABLE2,
			TRAYFINDUPSPEED,
			PLACESLOWSPEED,
			DRYRUN,
			UNKNOWN	= -1,
		}

		public enum PosHolderZ
		{
			START, //홀더 맨 아래 집게로 맨 아래 트레이 집을 수 있는 위치
			END, //홀더 맨 위 집게로 맨 위 트레이 집을 수 있는 위치
		}

		#endregion

		#region [Variables]
		private	SMM<Cmd>	m_smmHome		= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.LDTRANS_HOME));
		private SMM<Cmd>	m_smmPlace		= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.LDTRANS_PLACE));
		private	SMM<Cmd>	m_smmPick		= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.LDTRANS_PICK));
		private	SMM<Cmd>	m_smmPickConv	= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.LDTRANS_PICKCONV));
		private	SMM<Cmd>	m_smmPlaceConv	= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.LDTRANS_PLACECONV));
		private SMM<Cmd>	m_smmPnP		= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.LDTRANS_PNP));
		private SMM<Cmd>	m_smmReady		= new SMM<Cmd>(Cmd.START, "LDTRANS_READY");

		public	Cmd	smmHome				{ get => m_smmHome.CurStep; }
		public	Cmd	smmPlace			{ get => m_smmPlace.CurStep; }
		public	Cmd	smmPick				{ get => m_smmPick.CurStep; }
		public	Cmd	smmPickConv			{ get => m_smmPickConv.CurStep; }
		public	Cmd	smmPlaceConv		{ get => m_smmPlaceConv.CurStep; }
		public	Cmd	smmPnP				{ get => m_smmPnP.CurStep; }
		public	Cmd	smmReady			{ get => m_smmReady.CurStep; }

		private PosX	m_tePickPosX		= PosX.UNKNOWN;
		private	PosX	m_tePlacePosX		= PosX.UNKNOWN;
		private PosZ	m_tePosZ			= PosZ.UNKNOWN;
		private	bool	m_bPnPConvTeaching	= false;
		private int		m_nConvPickPos		= 0;
		private int		m_nConvPlacePos		= 0;
		private	int		m_nConvDownFastPos	= 0;
		private	bool	m_IsMtHome		= true;
		private	ALM		m_CurAlarm		= ALM.NORMAL;
		private string	m_AlarmMsg		= "";
		private int		m_LeftHolderPos = 0;
		private int		m_RightHolderPos = 0;

		// Mode Select 관련
		private string	m_strResultMode = "";
		#endregion

		public void		Init()
		{
			try
			{
				lstSMM.Add(m_smmHome);
				lstSMM.Add(m_smmPick);
				lstSMM.Add(m_smmPickConv);
				lstSMM.Add(m_smmPnP);
				lstSMM.Add(m_smmPlace);
				lstSMM.Add(m_smmPlaceConv);
				lstSMM.Add(m_smmReady);
				

				#region - Motor
				arrMt	= new EnumArray<Axis, CMotor>(Axis.Max);
				arrMt[Axis.X]				= G.cKernel.Motor(MT.LDTrayTransX);
				arrMt[Axis.Z]				= G.cKernel.Motor(MT.LDTrayTransZ);
				arrMt[Axis.Grip]			= G.cKernel.Motor(MT.LDTrayTransGrip);
				arrMt[Axis.HolderLeftZ]	= G.cKernel.Motor(MT.LDTrayHolder1Z);
				arrMt[Axis.HolderRightZ]	= G.cKernel.Motor(MT.LDTrayHolder2Z);
				#endregion

				#region - Actuator
				arrAct	= new EnumArray<Act, Actuator>(Act.Max);
				
				arrAct[Act.Holder] = new CDoubleSen();
				arrAct[Act.Holder].SetSenser1(new List<DI>(new DI[] { DI.LDTrayHolderGrip1Close , DI.LDTrayHolderGrip2Close }));
				arrAct[Act.Holder].SetSenser2(new List<DI>(new DI[] { DI.LDTrayHolderGrip1Open, DI.LDTrayHolderGrip2Open, }));
				arrAct[Act.Holder].SetSol(DO.LDTrayHolderGripCloseCyl, DO.LDTrayHolderGripOpenCyl );
				arrAct[Act.Holder].Setting(ALM.ERROR_LOAD_CONV_TRAY_HOLDER1_CYL_GRIP, ALM.ERROR_LOAD_CONV_TRAY_HOLDER1_CYL_RELEASE, ErrSenPos.Ori, SenPos.NONE, 200, 5000);
				
				/*
				sensor List  
				LDTrayTransExistSen		 - 포토센서, 트레이 잡앗을때, 감지
				LDTrayTransPosSen	     - 대각선 센서, 모터의 홈센서 처럼 자재위치 잡을 때 사용
				LDTrayTransProdExistSen	 - Z축 리미트, 인터락 형식으로 사용		
				TrayTransCollideSen      - 충돌 방지
				 */
				#endregion

				base.Initialize();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		#region [Interlock]
		////인터락

		////로드트레이 체크용 
		////0 : 로드트레이1 , 1 : 로드트레이2 
		public bool LoadTableUpDnSafety(int nTableNo)
		{
			try
			{
				if (arrMt[Axis.X].IsHomeDone() == false) return SM.SHM.InterlockMsgAdd(nTableNo, "arrMt[Axis.X].IsHomeDone() == false");

				switch (nTableNo)
				{
					case 0: //로드트레이1
						{
							if (arrMt[Axis.X].IsRdy() == true)
							{
								if (arrMt[Axis.X].IsRdy((int)PosX.LOADCONV) == true ||
									arrMt[Axis.X].IsRdy((int)PosX.LOADTABLE2) == true ||
									arrMt[Axis.X].IsRdy((int)PosX.TRAYCLEAN) == true ||
									arrMt[Axis.X].IsRdyPos(0) == true)
								{
									SM.SHM.InterlockMsgReset(nTableNo);
									return true;
								}
							}
						}
						break;
					case 1: //로드트레이2
						{
							if (arrMt[Axis.X].IsRdy() == true)
							{
								if (arrMt[Axis.X].IsRdy((int)PosX.LOADCONV) == true ||
									arrMt[Axis.X].IsRdy((int)PosX.LOADTABLE1) == true ||
									arrMt[Axis.X].IsRdy((int)PosX.TRAYCLEAN) == true ||
									arrMt[Axis.X].IsSmallerThan((int)PosX.LOADTABLE1) == true ||
									arrMt[Axis.X].IsRdyPos(0) == true)
								{
									SM.SHM.InterlockMsgReset(nTableNo);
									return true;
								}
							}
							else if (arrMt[Axis.X].GetCmdIndex() != (int)PosX.LOADTABLE2 &&
									 arrMt[Axis.X].GetCmdIndex() != (int)PosX.TRAYCLEAN &&
									 arrMt[Axis.X].IsSmallerThan((int)PosX.LOADTABLE1) == true)
							{
								SM.SHM.InterlockMsgReset(nTableNo);
								return true;
							}
						}
						break;
				}

				SM.SHM.InterlockMsgAdd(nTableNo, "arrMt[Axis.X] 안전 위치 아님");
				//안전한 위치가 아니면 return false 
				return false;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public bool UnloadTableUpDnSafety()
		{
			try
			{
				if (arrMt[Axis.X].IsHomeDone() == false)						return SM.SHM.InterlockMsgAdd("arrMt[Axis.X].IsHomeDone() == false");
                if (arrMt[Axis.X].IsBiggerThan((int)PosX.LOADTABLE2) == true)   return SM.SHM.InterlockMsgAdd("arrMt[Axis.X].IsBiggerThan((int)PosX.TRAYCLEAN) == true");
                if (arrMt[Axis.X].GetCmdIndex() == (int)PosX.TRAYCLEAN)        return SM.SHM.InterlockMsgAdd("arrMt[Axis.X].GetCmdIndex() == (int)PosX.TRAYCLEAN");

                return SM.SHM.InterlockMsgReset();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		//언로드 트랜스퍼 체크용
		public bool TrayCleanUpSafety()
		{
			try
			{
				if (arrMt[Axis.X].IsHomeDone() == false) return SM.SHM.InterlockMsgAdd("arrMt[Axis.X].IsHomeDone() == false");
				if (arrMt[Axis.Z].IsHomeDone() == false) return SM.SHM.InterlockMsgAdd("arrMt[Axis.Z].IsHomeDone() == false");
				
				//현재위치
				if (arrMt[Axis.X].IsBiggerThan((int)PosX.LOADTABLE2) == true  &&
					arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false)
				{
					return SM.SHM.InterlockMsgAdd("arrMt[Axis.X].IsBiggerThan((int)PosX.LOADTABLE1) == true  &&  arrMt[Axis.Z].IsRdy((int)tePosZ.READY) == false");
				}
				
				return SM.SHM.InterlockMsgReset();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public bool LoadConvMoveSafety()
		{
			try
			{
				if (arrMt[Axis.X].IsHomeDone() == false) return SM.SHM.InterlockMsgAdd("arrMt[Axis.X].IsHomeDone() == false");
				if (arrMt[Axis.Z].IsHomeDone() == false) return SM.SHM.InterlockMsgAdd("arrMt[Axis.Z].IsHomeDone() == false");

				if (arrMt[Axis.X].IsSmallerThan((int)PosX.LOADTABLE1) == true &&
					arrMt[Axis.Z].IsBiggerThan((int)PosZ.READY) == true)
				{
					return SM.SHM.InterlockMsgAdd("arrMt[Axis.X].IsSmallerThan((int)PosX.LOADTABLE1) == true  &&\narrMt[Axis.Z].IsBiggerThan((int)tePosZ.READY) == true");
				}

				return SM.SHM.InterlockMsgReset();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public bool UnloadTransferMoveXSafety(PosX xPos)
		{
			try
			{
				if (arrMt[Axis.X].IsSVON() == false)		return SM.SHM.InterlockMsgAdd("arrMt[Axis.X].IsSVON() == false");
				if (arrMt[Axis.X].IsHomeDone() == false)	return SM.SHM.InterlockMsgAdd("arrMt[Axis.X].IsHomeDone() == false");

				switch (xPos)
				{
					case PosX.TRAYCLEAN:
						if (arrMt[Axis.X].GetCmdIndex() == (int)PosX.TRAYCLEAN)			return SM.SHM.InterlockMsgAdd("arrMt[Axis.X].GetCmdIndex() == (int)PosX.TRAYCLEAN");
						if (arrMt[Axis.X].IsBiggerThan((int)PosX.LOADTABLE2) == true)	return SM.SHM.InterlockMsgAdd("arrMt[Axis.X].IsBiggerThan((int)PosX.LOADTABLE1) == true");
						
						break;
				}

				return SM.SHM.InterlockMsgReset();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public bool InterLockCheckMoveIndex(PosX xPos)
		{
			try
			{
				if (G.cKernel.IsBIn(DI.TrayTransCollideSen) == true)
				{
					arrMt[Axis.X].Stop();
					return SM.SHM.InterlockMsgAdd("G.cKernel.IsBIn(DI.TrayTransCollideSen) == true");
				}

				//1 = + 방향 , -1 = - 방향
				int nDir = -1;
				if (arrMt[Axis.X].GetIndexPos((int)xPos) > arrMt[Axis.X].GetLocalPos()) nDir = 1;

				switch (xPos)
				{
					case PosX.LOADCONV:
						{
							if (nDir == -1)
							{
								if (G.seqLoadTrayTable1.IsMotorStats(SeqLoadTrayTable1.PosZ.UP) == false &&
									G.seqLoadTrayTable1.IsMotorStats(SeqLoadTrayTable1.PosZ.DOWN) == false)
								{
									if (arrMt[Axis.X].IsInPosIndex((int)PosX.LOADTABLE1) == false)
										return SM.SHM.InterlockMsgAdd("G.seqLoadTrayTable1.IsCylStats(SenPos.UP) == false  &&\nG.seqLoadTrayTable1.IsCylStats(SenPos.DOWN) == false");
								}

								if (arrMt[Axis.X].IsBiggerThan((int)PosX.LOADTABLE1) == true &&
									G.seqLoadTrayTable2.IsMotorStats(SeqLoadTrayTable2.PosZ.UP) == false &&
									G.seqLoadTrayTable2.IsMotorStats(SeqLoadTrayTable2.PosZ.DOWN) == false)
                                {
                                    if (arrMt[Axis.X].IsInPosIndex((int)PosX.LOADTABLE2) == false)
                                        return SM.SHM.InterlockMsgAdd("arrMt[Axis.X].IsBiggerThan((int)PosX.LOADTABLE1) == true  &&\nG.seqLoadTrayTable2.IsCylStats(SenPos.UP) == false  &&\nG.seqLoadTrayTable2.IsCylStats(SenPos.DOWN) == false");
								}
							}
						}
						break;

					case PosX.TRAYCLEAN:
						{
							if (nDir == 1)
							{
								if (G.seqUnloadTransfer.LoadTransferMoveXSafety(SeqUnloadTransfer.PosX.TRAYCLEAN) == false)		return SM.SHM.InterlockMsgAdd("G.seqUnloadTransfer.TrayCleanMoveSafety() == false");

								if (G.seqLoadTrayTable2.IsMotorStats(SeqLoadTrayTable2.PosZ.UP) == false &&
									G.seqLoadTrayTable2.IsMotorStats(SeqLoadTrayTable2.PosZ.DOWN) == false)
								{
									return SM.SHM.InterlockMsgAdd("G.seqLoadTrayTable2.IsCylStats(SenPos.UP) == false");
								}
								if (G.seqLoadTrayTable1.IsMotorStats(SeqLoadTrayTable1.PosZ.UP) == false &&
									G.seqLoadTrayTable1.IsMotorStats(SeqLoadTrayTable1.PosZ.DOWN) == false)
								{
									return SM.SHM.InterlockMsgAdd("G.seqLoadTrayTable1.IsCylStats(SenPos.UP) == false  &&\nG.seqLoadTrayTable1.IsCylStats(SenPos.DOWN) == false");
								}
								if (G.seqUnloadTrayTable1.IsMotorStats(SeqUnloadTrayTable.PosZ.UP) == false)
								{
									return SM.SHM.InterlockMsgAdd("G.seqUnloadTrayTable1.IsCylStats(SenPos.UP) == false  &&\nG.seqLoadTrayTable1.IsCylStats(SenPos.DOWN) == false");
								}
							}
							else
							{
							}
						}
						break;

					case PosX.LOADTABLE1:
						{
							if (nDir == 1)
							{
								if (G.seqLoadTrayTable1.IsMotorStats(SeqLoadTrayTable1.PosZ.DOWN) == false)	 return SM.SHM.InterlockMsgAdd("G.seqLoadTrayTable1.IsCylStats(SenPos.DOWN) == false");
							}
							else
							{
								if (G.seqLoadTrayTable2.IsMotorStats(SeqLoadTrayTable2.PosZ.DOWN) == false  &&
									G.seqLoadTrayTable2.IsMotorStats(SeqLoadTrayTable2.PosZ.UP) == false)
                                {
                                    if (arrMt[Axis.X].IsInPosIndex((int)PosX.LOADTABLE2) == false)
                                        return SM.SHM.InterlockMsgAdd("G.seqLoadTrayTable1.IsCylStats(SenPos.DOWN) == false");
                                }
							}
						}
						break;

					case PosX.LOADTABLE2:
						{
							if (nDir == 1)
							{
								if (G.seqUnloadTransfer.LoadTransferMoveXSafety(SeqUnloadTransfer.PosX.TRAYCLEAN) == false)							return SM.SHM.InterlockMsgAdd("G.seqUnloadTransfer.TrayCleanMoveSafety() == false");
								if (G.seqLoadTrayTable1.IsMotorStats(SeqLoadTrayTable1.PosZ.UP) == false  &&
									G.seqLoadTrayTable1.IsMotorStats(SeqLoadTrayTable1.PosZ.DOWN) == false)
								{
                                    if (arrMt[Axis.X].IsInPosIndex((int)PosX.LOADTABLE1) == false)
                                        return SM.SHM.InterlockMsgAdd("G.seqLoadTrayTable1.IsCylStats(SenPos.UP) == false");

                                }
								if (G.seqLoadTrayTable2.IsMotorStats(SeqLoadTrayTable2.PosZ.DOWN) == false)	
									return SM.SHM.InterlockMsgAdd("G.seqLoadTrayTable2.IsCylStats(SenPos.DOWN) == false");
							}
							else
							{
								if (G.seqLoadTrayTable1.IsMotorStats(SeqLoadTrayTable1.PosZ.DOWN) == false  &&
									G.seqLoadTrayTable1.IsMotorStats(SeqLoadTrayTable1.PosZ.UP) == false)
								{
                                    if (arrMt[Axis.X].IsInPosIndex((int)PosX.LOADTABLE1) == false)
                                        return SM.SHM.InterlockMsgAdd("G.seqLoadTrayTable1.IsCylStats(SenPos.DOWN) == false");
                                }
							}
						}
						break;
				}

				if (arrMt[Axis.X].IsRdy((int)xPos) == false)
				{
					arrMt[Axis.X].MoveIndex((int)xPos);
				}

				return SM.SHM.InterlockMsgReset();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}
#endregion

		#region [Function]

		public bool HomeStart(bool IsMtHome = true) { m_IsMtHome = IsMtHome; return SetSeqRun(ref m_smmHome); }

		public bool ReadyStart() { return SetSeqRun(ref m_smmReady); }

		public bool PickStart(PosX TargetPos)
		{
			try
			{
				if (PosX.LOADCONV == TargetPos || PosX.TRAYCLEAN == TargetPos || PosX.UNKNOWN == TargetPos)
					return false;

				m_tePickPosX = TargetPos;
				m_tePlacePosX = PosX.UNKNOWN;
				return SetSeqRun(ref m_smmPick);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public bool PickConvStart(bool bTeaching)
		{
			m_tePickPosX = PosX.UNKNOWN;
			m_tePlacePosX = PosX.UNKNOWN;
			m_bPnPConvTeaching = bTeaching;
			return SetSeqRun(ref m_smmPickConv);
		}

		public bool PlaceStart(PosX TargetPos)
		{
			try
			{
				if (PosX.UNKNOWN == TargetPos)
					return false;

				m_tePickPosX = PosX.UNKNOWN;
				m_tePlacePosX = TargetPos;
				return SetSeqRun(ref m_smmPlace);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public bool PlaceConvStart(bool bTeaching)
		{
			m_tePickPosX = PosX.UNKNOWN;
			m_tePlacePosX = PosX.UNKNOWN;
			m_bPnPConvTeaching = bTeaching;
			return SetSeqRun(ref m_smmPickConv);
		}

		public bool PnPStart(PosX PickPos, PosX PlacePos)
		{
			try
			{
				if (PosX.TRAYCLEAN == PickPos || PosX.UNKNOWN == PickPos)
					return false;

				if (PosX.UNKNOWN == PlacePos)
					return false;

				m_tePickPosX = PickPos;
				m_tePlacePosX = PlacePos;
				return SetSeqRun(ref m_smmPnP);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public bool IsHomeReady() { return m_smmHome.IsReady; }

		public bool IsPickReady() { return m_smmPick.IsReady; }

		public bool IsPickConvReady() { return m_smmPickConv.IsReady; }

		public bool IsPlaceReady() { return m_smmPlace.IsReady; }

		public bool IsPlaceConvReady() { return m_smmPlaceConv.IsReady; }

		public bool IsPnPReady() { return m_smmPnP.IsReady; }

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


		private void ExistSensorCheck()
		{
			try
			{
				//Ready 위치에서만 체크
				if (arrMt[Axis.Z].IsInPosIndex((int)PosZ.READY) == false) return;

				//플로우와 센서 감지가 다른 경우
				if (SM.FMT.IsFlow(FlowTray.LDTrans) == true && G.DryRunIsAIn(DI.LDTrayTransExistSen, true) == false &&
					NDm.Read(eNDM.AutoRunMode) != (int)AutoRunMode.QuitLot)
				{
					SM.SHM.MsgShow(SeqMsg.TrayMisMatch);
					AddAlarm(ALM.ERROR_SEQ_LOAD_TRANSFER_TRAY_NONE);
					return;
				}
				else if (SM.FMT.IsFlow(FlowTray.LDTrans) == false && G.DryRunIsAIn(DI.LDTrayTransExistSen, false) == true)
				{
					AddAlarm(ALM.ERROR_SEQ_LOAD_TRANSFER_TRAY_EXIST);
					return;
				}

				if (G.cKernel.IsBIn(DI.TrayTransCollideSen) == true)
				{
					arrMt[Axis.X].Stop();
					AddAlarm(ALM.ERROR_SEQ_INTERLOCK_TRANSFER_COLLISION_SENSOR);
					return;
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private bool CheckLoadCondition(PosX TargetPos)
		{
			try
			{
				switch (TargetPos)
				{
					case PosX.LOADCONV:		return G.seqTrayLoader.IsLoadEnable();
					case PosX.TRAYCLEAN:	return G.seqTrayClean.IsLoadEnable();
					case PosX.LOADTABLE1:	return G.seqLoadTrayTable1.IsLoadEnable();
					case PosX.LOADTABLE2:	return G.seqLoadTrayTable2.IsLoadEnable();
				}
				return false;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		private bool CheckUnloadCondition(PosX TargetPos)
		{
			try
			{
				bool bunload = !(NDm.Read(eNDM.AutoRunMode) == (int)AutoRunMode.EndLot || NDm.Read(eNDM.AutoRunMode) == (int)AutoRunMode.QuitLot);

				switch (TargetPos)
				{
					case PosX.LOADCONV: return G.seqTrayLoader.IsTrayTransferEnable();
					case PosX.TRAYCLEAN: return G.seqTrayClean.IsUnloadEnable();
					case PosX.LOADTABLE1: return G.seqLoadTrayTable1.IsUnloadEnable();
					case PosX.LOADTABLE2: return G.seqLoadTrayTable2.IsUnloadEnable();
				}
				return false;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		//Xpos에 따른 zPos 리턴
		private PosZ GetZPos(PosX TargetPos)
		{
			try
			{
				if (SM.SHM.GetUseSkipData(USESKIP.DRYRUN) == true)
				{
					return PosZ.DRYRUN;
				}

				switch (TargetPos)
				{
					case PosX.TRAYCLEAN: return PosZ.TRAYCLEAN;
					case PosX.LOADTABLE1: return PosZ.LOADTABLE1;
					case PosX.LOADTABLE2: return PosZ.LOADTABLE2;
				}
				return PosZ.READY;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return PosZ.UNKNOWN;
			}
		}

		//Xpos에 따른 플로우 리턴
		private FlowTray GetFlow(PosX TargetPos)
		{
			try
			{
				switch (TargetPos)
				{
					case PosX.LOADCONV: return SM.FMT.GetFirstExistFlow(FlowTray.LDConv01, FlowTray.LDConv31);
					case PosX.TRAYCLEAN: return FlowTray.TrayClean;
					case PosX.LOADTABLE1: return FlowTray.LDTable1;
					case PosX.LOADTABLE2: return FlowTray.LDTable2;
					default: return FlowTray.Empty;
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return FlowTray.Empty;
			}
		}

		private void GripStart(PosX TargetPos)
		{
			try
			{
				switch (TargetPos)
				{
					case PosX.LOADTABLE1:	G.seqLoadTrayTable1.GripStart();			break;
					case PosX.LOADTABLE2:	G.seqLoadTrayTable2.GripStart();			break;
					case PosX.TRAYCLEAN:	G.seqTrayClean.GripStart();					break;
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void ReleaseStart(PosX TargetPos)
		{
			try
			{
				switch (TargetPos)
				{
					case PosX.TRAYCLEAN:	G.seqTrayClean.ReleaseStart();				break;
					case PosX.LOADTABLE1:	G.seqLoadTrayTable1.ReleaseStart();			break;
					case PosX.LOADTABLE2:	G.seqLoadTrayTable2.ReleaseStart();			break;
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private bool GripCheck(PosX TargetPos)
		{
			try
			{
				switch (TargetPos)
				{
					case PosX.TRAYCLEAN: return G.seqTrayClean.IsGrip();
					case PosX.LOADTABLE1: return G.seqLoadTrayTable1.IsGrip();
					case PosX.LOADTABLE2: return G.seqLoadTrayTable2.IsGrip();
				}
				return false;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		private bool ReleaseCheck(PosX TargetPos)
		{
			try
			{
				switch (TargetPos)
				{
					case PosX.TRAYCLEAN: return G.seqTrayClean.IsRelease();
					case PosX.LOADTABLE1: return G.seqLoadTrayTable1.IsRelease();
					case PosX.LOADTABLE2: return G.seqLoadTrayTable2.IsRelease();

				}
				return false;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		//PNP 동작 예외 체크
		private bool PnPPosCheck()
		{
			try
			{
				switch (m_tePickPosX)
				{
					case PosX.LOADCONV:
						if (m_tePlacePosX == PosX.LOADTABLE1 ||
							m_tePlacePosX == PosX.LOADTABLE2 ||
							m_tePlacePosX == PosX.TRAYCLEAN) return true;
						break;
					case PosX.LOADTABLE1:
					case PosX.LOADTABLE2:
						if (m_tePlacePosX == PosX.TRAYCLEAN) return true;
						break;
				}

				GLog.AddErr(string.Format("LDTRANS PnP Position Check Error - PICK : {0}, PLACE : {1}", m_tePickPosX.ToString(), m_tePlacePosX.ToString()));
				return false;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		private bool CheckCollide(SMM<Cmd> smm)
		{
			try
			{
				if (G.cKernel.IsBIn(DI.LDTrayTransSafetySen) == true)
				{
					arrMt[Axis.Z].Stop();
					AddAlarm(ALM.ERROR_SEQ_LOAD_TRANSFER_TRAY_LIMIT_CHECK);
					smm.ResetStep();
					return true;
				}
				if (G.cKernel.IsBIn(DI.TrayTransCollideSen) == true)
				{
					arrMt[Axis.X].Stop();
					AddAlarm(ALM.ERROR_SEQ_INTERLOCK_TRANSFER_COLLISION_SENSOR);
					smm.ResetStep();
					return true;
				}

				return false;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return true;
			}
		}



		private void BarcodeScanStart()
		{
			try
			{
				if (SM.SHM.GetUseSkipData(USESKIP.BarcodeRead) == false) return;
				if (SM.SHM.GetUseSkipData(USESKIP.DRYRUN) == true) return;
				
				if (SM.SHM.GetRecipeData(DEVICE.ModuleType) == EnumHelper.GetDescription(ModuleType.LPCAMM))
				{
					if (G.LoadLPCAMMBarcode.IsConnect() == false)
					{
						AddAlarm(ALM.ERROR_SEQ_LOAD_TRANSFER_TRAY_BARCODE_READER_CONNECT_FAIL);
						return;
					}
					G.LoadLPCAMMBarcode.TriggerOn();
				}
				else
				{
					if (G.LoadOESBarcode.IsConnect() == false)
					{
						AddAlarm(ALM.ERROR_SEQ_LOAD_TRANSFER_TRAY_BARCODE_READER_CONNECT_FAIL);
						return;
					}
					G.LoadOESBarcode.TriggerOn();
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return;
			}
		}

		private void BarcodeScanStop()
		{
			try
			{
				if (SM.SHM.GetUseSkipData(USESKIP.BarcodeRead) == false) return;
				if (SM.SHM.GetUseSkipData(USESKIP.DRYRUN) == true) return;
				
				if (SM.SHM.GetRecipeData(DEVICE.ModuleType) == EnumHelper.GetDescription(ModuleType.LPCAMM))
				{
					G.LoadLPCAMMBarcode.TriggerOff();
				}
				else
				{
					G.LoadOESBarcode.TriggerOff();
				}

			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return;
			}
		}

		private string GetBarcodeResult()
		{
			try
			{
				if (SM.SHM.GetRecipeData(DEVICE.ModuleType) == EnumHelper.GetDescription(ModuleType.LPCAMM))
				{
					return G.LoadLPCAMMBarcode.GetResultData();
				}
				else
				{
					return G.LoadOESBarcode.GetResultData();
				}

			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return "";
			}
		}
		#endregion



		public void Run()
		{
			try
			{
				UpdateAct(Seq.LoadTransfer);

				if (SM.SHM.IsBitData(BIT.Error) == true || SM.SHM.IsBitData(BIT.Pause) == true) return;

				Home(m_smmHome);
				Pick(m_smmPick);
				PickConv(m_smmPickConv);
				Place(m_smmPlace);
				PlaceConv(m_smmPlaceConv);
				PnP(m_smmPnP);
				Ready(m_smmReady);

				//ExistSensorCheck();

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
							if (arrAct[Act.Holder].IsPos(SenPos.BWD) == false)
							{
								arrAct[Act.Holder].SetPos(SenPos.BWD);
								break;
							}

							if (SM.FMT.IsFlow(FlowTray.LDTrans) == true)
							{
								//Flow 존재, 자재 감지 되는 경우 > 하단 체크 후 안풀고 홈 동작
								if (G.DryRunIsAIn(DI.LDTrayTransExistSen, true) == true)
								{
									if (arrMt[Axis.X].IsInPosIndex((int)PosX.LOADTABLE1) &&
										arrMt[Axis.Z].IsInPosIndex((int)PosZ.LOADTABLE1))
									{
										if (G.seqLoadTrayTable1.IsRelease() == false)
										{
											G.seqLoadTrayTable1.ReleaseStart();
											break;
										}
										else
										{
											smm.Next(Cmd.HOME_11);
											break;
										}
									}
									else if (arrMt[Axis.X].IsInPosIndex((int)PosX.LOADTABLE2) &&
										arrMt[Axis.Z].IsInPosIndex((int)PosZ.LOADTABLE2))
									{
										if (G.seqLoadTrayTable2.IsRelease() == false)
										{
											G.seqLoadTrayTable2.ReleaseStart();
											break;
										}
										else
										{
											smm.Next(Cmd.HOME_11);
											break;
										}
									}
									else if (arrMt[Axis.X].IsInPosIndex((int)PosX.TRAYCLEAN) &&
										arrMt[Axis.Z].IsInPosIndex((int)PosZ.TRAYCLEAN))
									{
										if (G.seqTrayClean.IsRelease() == false)
										{
											G.seqTrayClean.ReleaseStart();
											break;
										}
										else
										{
											smm.Next(Cmd.HOME_11);
											break;
										}
									}
									else
									{
										smm.Next(Cmd.HOME_11);
										break;
									}
								}
								else //Flow 존재, 자재 감지 안되는 경우 > 알람 발생
								{
									AddAlarm(ALM.ERROR_SEQ_LOAD_TRANSFER_TRAY_NONE);
									break;
								}
							}
							else	//그외 Flow 없는데 , 자재 감지 되는 경우 > 그냥 홈처리 진행 > 그리퍼 풀고 올라가서 센서 체크 다시 진행 후 감지되면 알람 
							{
								smm.Next(Cmd.HOME_11);
							}
						}
						break;

					case Cmd.HOME_11:
						{
							//로드 테이블1번이 안전하게 움직이기 위해서 반드시 필요
							if (G.seqLoadTrayTable2.IsMotorStats(SeqLoadTrayTable2.PosX.READY) == false  && G.seqLoadTrayTable2.IsReady )
							{
								G.seqLoadTrayTable2.MoveXStart( SeqLoadTrayTable2.PosX.READY, SeqLoadTrayTable2.AutoMode.Manual);
								break;
							}
							
							smm.Next(Cmd.HOME_20);
						}
						break;

					case Cmd.HOME_20:
						{
							if (G.seqLoadTrayTable1.IsMotorStats(SeqLoadTrayTable1.PosZ.UP) == false && 
								G.seqLoadTrayTable1.IsMotorStats(SeqLoadTrayTable1.PosZ.DOWN) == false)
							{
								//세이프티 다운보다 큰 경우 올리고 그외엔 내리자
								if (G.seqLoadTrayTable1.IsBiggerThanSafetyDown())
                                {
                                    G.seqLoadTrayTable1.UpStart(SeqLoadTrayTable1.UDMode.Manual);
									break;
								}
								else
								{
                                    G.seqLoadTrayTable1.DownStart(SeqLoadTrayTable1.UDMode.Manual);
									break;
								}
							}
							if (G.seqLoadTrayTable2.IsMotorStats(SeqLoadTrayTable2.PosZ.UP) == false && 
								G.seqLoadTrayTable2.IsMotorStats(SeqLoadTrayTable2.PosZ.DOWN) == false)
							{
								//check
								if (G.seqLoadTrayTable2.IsBiggerThanSafetyDown())
                                {
                                    G.seqLoadTrayTable2.UpStart(SeqLoadTrayTable2.UDMode.Manual);
									break;
								}
								else
								{
                                    G.seqLoadTrayTable2.DownStart(SeqLoadTrayTable2.UDMode.Manual);
									break;
								}
							}

							if (m_IsMtHome == false)
							{
								smm.Next(Cmd.HOME_90);
								break;
							}

							arrMt[Axis.Grip].ClearHome();
							arrMt[Axis.Grip].StartHoming();

							smm.Next(Cmd.HOME_30);
						}
						break;
					case Cmd.HOME_30:
						{
							if (arrMt[Axis.Grip].IsHomeDone() == false || arrMt[Axis.Grip].IsRdy() == false)
								break;

							arrMt[Axis.Z].ClearHome();

							smm.Next(Cmd.HOME_40);
						}
						break;
					case Cmd.HOME_40:
						{
							if (m_IsMtHome == false)
							{
								smm.Next(Cmd.HOME_90);
								break;
							}

							arrMt[Axis.Z].StartHoming();

							smm.Next(Cmd.HOME_50);
						}
						break;
					case Cmd.HOME_50:
						{
							if (arrMt[Axis.Z].IsHomeDone() == false || arrMt[Axis.Z].IsRdy() == false)
								break;

							if (SM.FMT.IsFlow(FlowTray.LDTrans) != G.DryRunIsAIn(DI.LDTrayTransExistSen, SM.FMT.IsFlow(FlowTray.LDTrans)))
								AddAlarm(ALM.ERROR_SEQ_LOAD_TRANSFER_TRAY_EXIST);

							smm.Next(Cmd.HOME_60);
						}
						break;
					case Cmd.HOME_60:
						{
							if (arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false)
							{
								//SeqInterlock으로 인한 멈춤 문제 조치를 위해 추가
								NDm.Write(eNDM.ManualMotorMoveDir, -1);
								arrMt[Axis.Z].MoveIndex((int)PosZ.READY);
								break;
							}
							
							arrMt[Axis.X].ClearHome();
							arrMt[Axis.X].StartHoming();

							smm.Next(Cmd.HOME_70);
						}
						break;
					case Cmd.HOME_70:
						{
							if (arrMt[Axis.X].IsHomeDone() == false || arrMt[Axis.X].IsRdy() == false)
								break;

							if (arrAct[Act.Holder].IsPos(SenPos.BWD) == false)
							{
								arrAct[Act.Holder].SetPos(SenPos.BWD);
								break;
							}
							
							arrMt[Axis.HolderLeftZ].ClearHome();
							arrMt[Axis.HolderLeftZ].StartHoming();

							arrMt[Axis.HolderRightZ].ClearHome();
							arrMt[Axis.HolderRightZ].StartHoming();

							smm.Next(Cmd.HOME_80);
						}
						break;
					case Cmd.HOME_80:
						{
							if (arrMt[Axis.HolderLeftZ].IsHomeDone() == false || arrMt[Axis.HolderLeftZ].IsRdy() == false)
								break;

							if (arrMt[Axis.HolderRightZ].IsHomeDone() == false || arrMt[Axis.HolderRightZ].IsRdy() == false)
								break;

							smm.Next(Cmd.HOME_90);
						}
						break;

					case Cmd.HOME_90:
						{
							if (SM.FMT.IsFlow(FlowTray.LDTrans) == true)
							{
								if (arrMt[Axis.Grip].IsRdy((int)PosGrip.GRIPHARD) == false)
								{
									arrMt[Axis.Grip].MoveIndex((int)PosGrip.GRIPHARD);
									break;
								}
							}
							else
							{
								if (arrMt[Axis.Grip].IsRdy((int)PosGrip.RELEASE) == false)
								{
									arrMt[Axis.Grip].MoveIndex((int)PosGrip.RELEASE);
									break;
								}
							}
							

							smm.Next(Cmd.HOME_100);
						}
						break;

					case Cmd.HOME_100:
						{
							if (arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false)
							{
								//SeqInterlock으로 인한 멈춤 문제 조치를 위해 추가
								NDm.Write(eNDM.ManualMotorMoveDir, -1);
								arrMt[Axis.Z].MoveIndex((int)PosZ.READY);
								break;
							}
							
							if (arrAct[Act.Holder].IsPos(SenPos.BWD) == false)
							{
								arrAct[Act.Holder].SetPos(SenPos.BWD);
								break;
							}

							smm.Next(Cmd.HOME_110);
						}
						break;

					case Cmd.HOME_110:
						{
							if (arrMt[Axis.X].IsRdy((int)PosX.LOADCONV) == false  ||
								arrMt[Axis.HolderLeftZ].IsRdy((int)PosGrip.RELEASE) == false  ||
								arrMt[Axis.HolderRightZ].IsRdy((int)PosGrip.RELEASE) == false)
							{
								arrMt[Axis.X].MoveIndex((int)PosX.LOADCONV);
								arrMt[Axis.HolderLeftZ].MoveIndex((int)PosHolderZ.START);
								arrMt[Axis.HolderRightZ].MoveIndex((int)PosHolderZ.START);
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

		private void Pick(SMM<Cmd> smm)
		{
			try
			{
				if (smm.CurStep != Cmd.START && CheckCollide(smm) == true) return;

				switch (smm.CurStep)
				{
					case Cmd.START:
						{
							if (smm.eInput == InState.run)
							{
								smm.eInput = InState.none;

								smm.Next(Cmd.PICK_10);
								GLog.Add(string.Format("LDTRANS Pick - Step : {0}, Pick : {1}", Cmd.PICK_10, m_tePickPosX));
							}
						}
						break;

					case Cmd.PICK_10:
						{
							if (arrMt[Axis.Grip].IsRdy((int)PosGrip.RELEASE) == false)
							{
								arrMt[Axis.Grip].MoveIndex((int)PosGrip.RELEASE);
								break;
							}

							if (arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false)
							{
								arrMt[Axis.Z].MoveIndex((int)PosZ.READY);
								break;
							}


							//트레이 유무 예외체크
							if (G.cKernel.IsAIn(DI.LDTrayTransExistSen) == true)
							{
								AddAlarm(ALM.ERROR_SEQ_LOAD_TRANSFER_TRAY_EXIST);
								smm.Next(Cmd.END);
								break;
							}

							//플로우 매니저 체크
							if (SM.FMT.IsFlow(GetFlow(m_tePickPosX)) == false)
							{
								AddAlarm(ALM.ERROR_FLOW_MANAGER_CHECK);
								smm.Next(Cmd.END);
								break;
							}

							smm.Next(Cmd.PICK_20);
						}
						break;

					case Cmd.PICK_20:
						{
							//필요시 비켜주는 시퀀스
							if (arrMt[Axis.X].IsInPosIndex((int)m_tePickPosX) == true)
							{
								smm.Next(Cmd.PICK_22);
								break;
							}

							smm.Next(Cmd.PICK_30);
						}
						break;

					case Cmd.PICK_22:
						{
							switch (m_tePickPosX)
							{
								case PosX.LOADTABLE1:
									if (arrMt[Axis.X].IsRdy((int)PosX.LOADCONV) == false &&
										arrMt[Axis.X].IsRdy((int)PosX.LOADTABLE2) == false &&
										arrMt[Axis.X].IsRdy((int)PosX.TRAYCLEAN) == false)
									{
										InterLockCheckMoveIndex(PosX.LOADCONV);
										return;
									}
									break;
								case PosX.LOADTABLE2:
									if (arrMt[Axis.X].IsRdy((int)PosX.LOADCONV) == false &&
										arrMt[Axis.X].IsRdy((int)PosX.LOADTABLE1) == false)
									{
										InterLockCheckMoveIndex(PosX.LOADTABLE1);
										return;
									}
									break;
								default:
									break;
							}

							smm.Next(Cmd.PICK_30);
						}
						break;

					case Cmd.PICK_30:
						{

							smm.Next(Cmd.PICK_40);
						}
						break;

					case Cmd.PICK_40:
						{
							if (CheckUnloadCondition(m_tePickPosX) == false) break;

							if (arrMt[Axis.X].IsRdy((int)m_tePickPosX) == false)
							{
								InterLockCheckMoveIndex(m_tePickPosX);
								break;
							}

							m_tePosZ = GetZPos(m_tePickPosX);

							smm.Next(Cmd.PICK_50);
						}
						break;

					case Cmd.PICK_50:
						{
							if (arrMt[Axis.Z].IsRdy((int)m_tePosZ) == false)
							{
								arrMt[Axis.Z].MoveIndex((int)m_tePosZ);
								break;
							}

							smm.Next(Cmd.PICK_60);
						}
						break;

					case Cmd.PICK_60:
						{
							if (arrMt[Axis.Grip].IsRdy((int)PosGrip.GRIP) == false)
							{
								arrMt[Axis.Grip].MoveIndex((int)PosGrip.GRIP);
								break;
							}

							SM.FMT.Next(GetFlow(m_tePickPosX), FlowTray.LDTrans);

							if (m_tePickPosX == PosX.LOADTABLE1 || m_tePickPosX == PosX.LOADTABLE2)
							{
								if (SM.FMT[FlowTray.LDTrans].GetInfo().LastTrayOfLot == true)
								{
									SM.FMT[FlowTray.LDTrans].SetLastOfLot(false);
								}
							}

							smm.Next(Cmd.PICK_70);
						}
						break;

					case Cmd.PICK_70:
						{
							if (ReleaseCheck(m_tePickPosX) == false)
							{
								ReleaseStart(m_tePickPosX);
								break;
							}

							smm.Next(Cmd.PICK_80);
						}
						break;

					case Cmd.PICK_80:
						{

							smm.Next(Cmd.PICK_90);
						}
						break;

					case Cmd.PICK_90:
						{
							if (arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false)
							{
								arrMt[Axis.Z].MoveIndex((int)PosZ.READY);
								break;
							}

							if (arrMt[Axis.Grip].IsRdy((int)PosGrip.GRIPHARD) == false)
							{
								arrMt[Axis.Grip].MoveIndex((int)PosGrip.GRIPHARD);
								break;
							}

							smm.Next(Cmd.PICK_100);
						}
						break;

					case Cmd.PICK_100:
						{

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

		private void PickConv(SMM<Cmd> smm)
		{
			try
			{
				int ntray_height = SM.SHM.GetRecipeDataToInt(DEVICE.TrayHeight);

				if (smm.CurStep != Cmd.START && CheckCollide(smm) == true) return;

				if (smm.CurStep >= Cmd.PICK_CONV_10 &&
					smm.CurStep != Cmd.PICK_CONV_50 &&
					smm.CurStep < Cmd.PICK_CONV_90 &&
					arrMt[Axis.Z].GetLocalPos() > (arrMt[Axis.Z].GetIndexPos((int)PosZ.LOADCONVLIMIT) - ntray_height / 4))
				{
					arrMt[Axis.Z].Stop();
					smm.Next(Cmd.PICK_CONV_50);
				}

				switch (smm.CurStep)
				{
					case Cmd.START:
						{
							if (smm.eInput == InState.run)
							{
								smm.eInput = InState.none;

								smm.Next(Cmd.PICK_CONV_10);
							}
						}
						break;

					case Cmd.PICK_CONV_10:
						{

                            if (G.seqTrayLoader.CheckTraySize() < 0)
                            {
                                SM.SHM.MsgShow(SeqMsg.TraySize);
                                AddAlarm(ALM.ERROR_SEQ_LOADER_CONV_TRAY_SIZE_DIFFERENT_RECIPE);
                                return;
                            }

                            //Lot load 체크
                            if (CheckUnloadCondition(PosX.LOADCONV) == false)
							{
								AddAlarm(ALM.ERROR_SEQ_LOADER_LIFT_UNLOAD_ENABLE_CHECK);
								smm.Next(Cmd.END);
								break;
							}
							
							//이미 잡고 있는 트레이가 있는지 체크
							if (G.cKernel.IsAIn(DI.LDTrayTransExistSen) == true)
							{
								AddAlarm(ALM.ERROR_SEQ_LOAD_TRANSFER_TRAY_EXIST);
								smm.Next(Cmd.END);
								break;
							}
							
							if (arrAct[Act.Holder].IsPos(SenPos.BWD) == false)
							{
								arrAct[Act.Holder].SetPos(SenPos.BWD);
								break;
							}

							if (arrMt[Axis.Grip].IsRdy((int)PosGrip.RELEASE) == false  || 
								arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false)
							{
								arrMt[Axis.Z].MoveIndex((int)PosZ.READY);
								arrMt[Axis.Grip].MoveIndex((int)PosGrip.RELEASE);
								break;
							}

							smm.Next(Cmd.PICK_CONV_20);
						}
						break;

					case Cmd.PICK_CONV_20:
						{
							if (arrMt[Axis.X].IsRdy((int)PosX.LOADCONV) == false)
							{
								InterLockCheckMoveIndex(PosX.LOADCONV);
								break;
							}
							smm.Next(Cmd.PICK_CONV_30);
						}
						break;

					case Cmd.PICK_CONV_30:
						{
							int nTrayCount = SM.FMT.GetExistCountRange(FlowTray.LDConv01, FlowTray.LDConv31);
							int nTotalHight = ntray_height *  nTrayCount;

							m_nConvDownFastPos = arrMt[Axis.Z].GetIndexPos((int)PosZ.LOADCONVLIMIT) - (nTotalHight) - ntray_height / 4;

							if (m_nConvDownFastPos < arrMt[Axis.Z].GetIndexPos((int)PosZ.READY))
							{
								m_nConvDownFastPos = arrMt[Axis.Z].GetIndexPos((int)PosZ.READY);
							}

                            //홀더는 30mm 간격 6개, 최상단은 제일 아래있는 홀더 사용 불가
                            //20개 이하는 아래 집게 기준으로 아래에서 위만큼 더해서 계산
                            if (nTrayCount ==1 )
                            {
                                m_LeftHolderPos = arrMt[Axis.HolderLeftZ].GetIndexPos((int)PosHolderZ.START) ;
                                m_RightHolderPos = arrMt[Axis.HolderRightZ].GetIndexPos((int)PosHolderZ.START) ;
                            }
							else if (nTrayCount < 20)
							{
								int nPos = (SM.SHM.GetRecipeDataToInt(DEVICE.TrayStackedHeight)  ) * (nTrayCount - 2);
								m_LeftHolderPos = arrMt[Axis.HolderLeftZ].GetIndexPos((int)PosHolderZ.START) + nPos;
								m_RightHolderPos = arrMt[Axis.HolderRightZ].GetIndexPos((int)PosHolderZ.START) + nPos;
							}
							//20개 이상는 위쪽 집게 기준으로 위에서 아래만큼 빼서 계산
							else 
							{	
								int nPos = (SM.SHM.GetRecipeDataToInt(DEVICE.TrayStackedHeight) ) * (nTrayCount - 2);
								m_LeftHolderPos = arrMt[Axis.HolderLeftZ].GetIndexPos((int)PosHolderZ.START) + nPos - 150000;
								m_RightHolderPos = arrMt[Axis.HolderRightZ].GetIndexPos((int)PosHolderZ.START) + nPos - 150000;
							}
							
							arrMt[Axis.HolderLeftZ].MoveABS(m_LeftHolderPos, (int)PosHolderZ.START);
							arrMt[Axis.HolderRightZ].MoveABS(m_RightHolderPos, (int)PosHolderZ.START);

							smm.Next(Cmd.PICK_CONV_35);
						}
						break;

					case Cmd.PICK_CONV_35:
						{
                            if (arrMt[Axis.Z].IsRdyPos(m_nConvDownFastPos) == true)
							{
								smm.Next(Cmd.PICK_CONV_40);
								break;
							}
							else if (arrMt[Axis.Z].IsRdy() == true)
							{
								arrMt[Axis.Z].MoveABS(m_nConvDownFastPos, (int)PosZ.LOADCONV);
							}

							if (G.DryRunIsBIn(DI.LDTrayTransPosSen, true) == true)
							{
								arrMt[Axis.Z].Stop();
								smm.Next(Cmd.PICK_CONV_60);
								break;
							}
						}
						break;

					case Cmd.PICK_CONV_40:
						{
							if (arrMt[Axis.Z].IsRdy((int)PosZ.LOADCONVLIMIT) == true)
							{
								smm.Next(Cmd.PICK_CONV_90);
								break;
							}
							else if (arrMt[Axis.Z].IsRdy() == true)
							{
								arrMt[Axis.Z].MoveIndex((int)PosZ.LOADCONVLIMIT);
							}

							if (G.DryRunIsBIn(DI.LDTrayTransPosSen, true) == true)
							{
								arrMt[Axis.Z].Stop();
								smm.Next(Cmd.PICK_CONV_60);
								break;
							}
						}
						break;

					case Cmd.PICK_CONV_50:
						{
							if (smm.Delayms(300) == false) break;
							if (arrMt[Axis.Z].IsRdy() == false) break;

							if (arrMt[Axis.Z].IsRdy((int)PosZ.LOADCONVLIMIT) == false)
							{
								arrMt[Axis.Z].MoveIndex((int)PosZ.LOADCONVLIMIT);
								break;
							}
							smm.Next(Cmd.PICK_CONV_90);
						}
						break;

					case Cmd.PICK_CONV_60:
						{
							if (arrMt[Axis.Z].IsRdy() == true)
							{
								arrMt[Axis.Z].MoveJog(false, arrMt[Axis.Z].GetVel((int)PosZ.TRAYFINDUPSPEED));
							}

							//감지가 안되면 정지
							if (G.DryRunIsBIn(DI.LDTrayTransPosSen, false) == true) break;

							arrMt[Axis.Z].Stop();

							smm.Next(Cmd.PICK_CONV_70);
						}
						break;

					case Cmd.PICK_CONV_70:
						{
							if (arrMt[Axis.Z].IsRdy() == true)
							{
								arrMt[Axis.Z].MoveJog(true, 5000);
							}

							if (G.DryRunIsBIn(DI.LDTrayTransPosSen, true) == false) break;

							arrMt[Axis.Z].Stop();

							m_nConvPickPos = arrMt[Axis.Z].GetLocalPos() + SM.SHM.GetRecipeDataToInt(DEVICE.LDTransDnDist);

							smm.Next(Cmd.PICK_CONV_80);
						}
						break;

					case Cmd.PICK_CONV_80:
						{
							if (arrMt[Axis.Z].IsBiggerThan((int)PosZ.LOADCONVLIMIT) == true)
							{
								arrMt[Axis.Z].Stop();
								smm.Next(Cmd.PICK_CONV_90);
								break;
							}

							if (m_bPnPConvTeaching == true)
							{
								smm.Next(Cmd.END);
								break;
							}

							if (arrMt[Axis.Z].IsRdyPos(m_nConvPickPos) == false)
							{
								arrMt[Axis.Z].MoveABS(m_nConvPickPos, (int)PosZ.LOADCONVLIMIT);
								break;
							}

							smm.Next(Cmd.PICK_CONV_90);
						}
						break;

					case Cmd.PICK_CONV_90: // PICK 완료
						{
							if (arrMt[Axis.Z].IsBiggerThan((int)PosZ.LOADCONVLIMIT) == true)
							{
								arrMt[Axis.Z].Stop();
							}

							if (smm.Delayms(200) == false) break;
							if (arrMt[Axis.Z].IsRdy() == false) break;

							if (arrMt[Axis.Grip].IsRdy((int)PosGrip.GRIP) == false)
							{
								arrMt[Axis.Grip].MoveIndex((int)PosGrip.GRIP);
								break;
							}


							m_nConvPickPos = arrMt[Axis.Z].GetLocalPos() - 18000;
							smm.Next(Cmd.PICK_CONV_BARCODE_SCAN_10);
						}
						break;

					case Cmd.PICK_CONV_BARCODE_SCAN_10:
						{
							BarcodeScanStart();

							smm.Next(Cmd.PICK_CONV_BARCODE_SCAN_20);
						}
						break;


					case Cmd.PICK_CONV_BARCODE_SCAN_20:
						{
							if (smm.Delayms(300) == false) break;

							BarcodeScanStop();

							smm.Next(Cmd.PICK_CONV_BARCODE_SCAN_30);
						}
						break;


					case Cmd.PICK_CONV_BARCODE_SCAN_30:
						{
							string strTrayBarcode = GetBarcodeResult();

							if (SM.SHM.GetUseSkipData(USESKIP.BarcodeRead) == false)
							{
								strTrayBarcode = "Not Use";
							}

							if (string.IsNullOrEmpty(strTrayBarcode) == false || 
								smm.Delayms(5000) == true)
							{
								smm.Next(Cmd.PICK_CONV_BARCODE_SCAN_40);
								break;
							}
						}
						break;


					case Cmd.PICK_CONV_BARCODE_SCAN_40:
						{
							m_AlarmMsg = "";
							m_CurAlarm = ALM.NORMAL;
							string strTrayBarcode = GetBarcodeResult();
							string TrayRegex = SM.FMT[GetFlow(PosX.LOADCONV)].GetInfo().TrayBarcode;
							
							//트레이 바코드 읽기 실패
							if (SM.SHM.GetUseSkipData(USESKIP.BarcodeRead) == true && string.IsNullOrEmpty(strTrayBarcode))
							{
								m_CurAlarm = ALM.ERROR_SEQ_LOAD_TRANSFER_TRAY_BARCODE_READ_FAIL;
								smm.Next(Cmd.PICK_CONV_NG);
								break;
							}

							//트레이 정규식 매치 실패
							if (SM.SHM.GetUseSkipData(USESKIP.BarcodeRead) == true && Regex.IsMatch(strTrayBarcode, Regex.Escape(TrayRegex)) == false)
							{
								m_AlarmMsg = $"Tray Barcode Regex Value : {TrayRegex} , Read Tray Barcode : {strTrayBarcode}";
								m_CurAlarm = ALM.ERROR_SEQ_LOAD_TRANSFER_TRAY_BARCODE_REGEX_MATCH_FAIL;
								smm.Next(Cmd.PICK_CONV_NG);
								break;
							}

							FlowTray eFlow = GetFlow(PosX.LOADCONV);
							SM.FMT.Next(eFlow, FlowTray.LDTrans);

							GLog.Add($"Tray : {SM.FMT[FlowTray.LDTrans].GetInfo().TrayID} ,Set Tray Barcode : {strTrayBarcode}");
							SM.FMT[FlowTray.LDTrans].SetTrayBarcode(strTrayBarcode);

							GLog.Add(string.Format("Flow Next : ({0}, {1})", eFlow.ToString(), FlowTray.LDTrans.ToString()));

							smm.Next(Cmd.PICK_CONV_100);
						}
						break;
						
					case Cmd.PICK_CONV_100:
						{	
						
							smm.Next(Cmd.PICK_CONV_110);
						}
						break;

					case Cmd.PICK_CONV_110:
						{
							
							if (arrMt[Axis.HolderLeftZ].IsRdyPos(m_LeftHolderPos) == false  || 
								arrMt[Axis.HolderRightZ].IsRdyPos(m_RightHolderPos) == false)
							{
								arrMt[Axis.HolderLeftZ].MoveABS(m_LeftHolderPos, (int)PosHolderZ.START);
								arrMt[Axis.HolderRightZ].MoveABS(m_RightHolderPos, (int)PosHolderZ.START);
								break;
							}
							
                            if (SM.FMT.GetExistCountRange(FlowTray.LDConv01, FlowTray.LDConv31) != 0)
                            {
                                if (arrAct[Act.Holder].IsPos(SenPos.FWD) == false)
                                {
                                    arrAct[Act.Holder].SetPos(SenPos.FWD);
                                    break;
                                }
                            }
							
							if (arrMt[Axis.Z].IsRdyPos(m_nConvPickPos) == false)
							{
								arrMt[Axis.Z].MoveABS(m_nConvPickPos);
								break;
							}


							smm.Next(Cmd.PICK_CONV_120);
						}
						break;

					case Cmd.PICK_CONV_120:
						{
							if (arrAct[Act.Holder].IsPos(SenPos.BWD) == false)
							{
								arrAct[Act.Holder].SetPos(SenPos.BWD) ;
								break;
							}

							if (arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false)
							{
								arrMt[Axis.Z].MoveIndex((int)PosZ.READY);
								break;
							}

							if (arrMt[Axis.Grip].IsRdy((int)PosGrip.GRIPHARD) == false)
							{
								arrMt[Axis.Grip].MoveIndex((int)PosGrip.GRIPHARD);
								break;
							}

							smm.Next(Cmd.PICK_CONV_130);
						}
						break;

					case Cmd.PICK_CONV_130:
						{
							if (GetFlow(PosX.LOADCONV) == FlowTray.Empty &&
									 G.cKernel.IsAIn(DI.LDTrayConvExistSen) == true)
							{
								if (NDm.Read(eNDM.AutoRunMode) != (int)AutoRunMode.QuitLot)
								{
									SM.SHM.MsgShow(SeqMsg.TrayMisMatch);
									AddAlarm(ALM.ERROR_SEQ_LOADER_CONV_TRAY_MISMATCH);
								}
								smm.Next(Cmd.END);
								break;
							}
							else if (GetFlow(PosX.LOADCONV) != FlowTray.Empty &&
									 G.DryRunIsAIn(DI.LDTrayConvExistSen, true) == false)
							{
								if (NDm.Read(eNDM.AutoRunMode) != (int)AutoRunMode.QuitLot)
								{
									SM.SHM.MsgShow(SeqMsg.TrayMisMatch);
									AddAlarm(ALM.ERROR_SEQ_LOADER_CONV_TRAY_MISMATCH);
								}
								smm.Next(Cmd.END);
								break;
							}

							smm.Next(Cmd.PICK_CONV_140);
						}
						break;

					case Cmd.PICK_CONV_140:
						{
							FlowTray eFlow = GetFlow(PosX.LOADCONV);

							if (GetFlow(PosX.LOADCONV) == FlowTray.Empty &&
								G.cKernel.IsAIn(DI.LDTrayConvExistSen) == false)
							{
								SM.FMT[FlowTray.LDTrans].SetLastOfLot(true);
							}
							else
							{
								SM.FMT[FlowTray.LDTrans].SetLastOfLot(false);
							}

							smm.Next(Cmd.END);
						}
						break;

					case Cmd.PICK_CONV_NG:
						{
							switch (m_CurAlarm)
							{
								case ALM.ERROR_SEQ_LOAD_TRANSFER_TRAY_BARCODE_READ_FAIL:
									SM.SHM.MsgShow(SeqMsg.TrayBarcodeFail);
									break;
								case ALM.ERROR_SEQ_LOAD_TRANSFER_TRAY_BARCODE_REGEX_MATCH_FAIL:
									SM.SHM.SetMsg(SeqMsg.TrayBarcode_RegexMatchFail, m_AlarmMsg);
									SM.SHM.MsgShow(SeqMsg.TrayBarcode_RegexMatchFail);
									break;
								default:
									break;
							}
							AddAlarm(m_CurAlarm);
							m_smmPnP.ResetStep();

							smm.Next(Cmd.END);
						}
						break;

					case Cmd.END:
						{
							m_bPnPConvTeaching = false;
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

		private void Place(SMM<Cmd> smm)
		{
			try
			{
				if (smm.CurStep != Cmd.START && CheckCollide(smm) == true) return;

				switch (smm.CurStep)
				{
					case Cmd.START:
						{
							if (smm.eInput == InState.run)
							{
								smm.eInput = InState.none;

								smm.Next(Cmd.PLACE_10);
								GLog.Add(string.Format("LDTRANS Place - Step : {0}, Place : {1}", Cmd.PLACE_10, m_tePlacePosX));
							}
						}
						break;

					case Cmd.PLACE_10:
						{
							//트레이를 잡고 있는지 체크
							if (G.DryRunIsAIn(DI.LDTrayTransExistSen, true) == false ||
								SM.FMT.IsFlow(FlowTray.LDTrans) == false)
							{
								AddAlarm(ALM.ERROR_SEQ_LOAD_TRANSFER_TRAY_NONE);
								smm.Next(Cmd.END);
								break;
							}

							smm.Next(Cmd.PLACE_20);
						}
						break;

					case Cmd.PLACE_20:
						{
							if (arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false)
							{
								arrMt[Axis.Z].MoveIndex((int)PosZ.READY);
								break;
							}

							smm.Next(Cmd.PLACE_30);
						}
						break;

					case Cmd.PLACE_30:
						{
							if (arrMt[Axis.Grip].IsHomeDone() == true &&
								arrMt[Axis.Grip].IsRdy((int)PosGrip.GRIPHARD) == false)
							{
								arrMt[Axis.Grip].MoveIndex((int)PosGrip.GRIPHARD);
								break;
							}

							smm.Next(Cmd.PLACE_40);
						}
						break;

					case Cmd.PLACE_40:
						{
							switch (m_tePlacePosX)
							{
								case PosX.LOADTABLE1:
									if (arrMt[Axis.X].IsRdy((int)PosX.LOADCONV) == false &&
										arrMt[Axis.X].IsRdy((int)PosX.LOADTABLE2) == false &&
										arrMt[Axis.X].IsRdy((int)PosX.TRAYCLEAN) == false)
									{
										InterLockCheckMoveIndex(PosX.LOADCONV);
										return;
									}
									break;
								case PosX.LOADTABLE2:
									if (arrMt[Axis.X].IsRdy((int)PosX.LOADCONV) == false &&
										arrMt[Axis.X].IsRdy((int)PosX.LOADTABLE1) == false &&
										arrMt[Axis.X].IsRdy((int)PosX.TRAYCLEAN) == false)
									{
										InterLockCheckMoveIndex(PosX.LOADCONV);
										return;
									}
									break;
								default:
									break;
							}

							smm.Next(Cmd.PLACE_50);
						}
						break;

					case Cmd.PLACE_50:
						{


							smm.Next(Cmd.PLACE_60);
						}
						break;

					case Cmd.PLACE_60:
						{
							if (CheckLoadCondition(m_tePlacePosX) == false) break;

							if (arrMt[Axis.X].IsRdy((int)m_tePlacePosX) == false)
							{
								InterLockCheckMoveIndex(m_tePlacePosX);
								break;
							}

							if (arrMt[Axis.Grip].IsRdy((int)PosGrip.GRIP) == false)
							{
								arrMt[Axis.Grip].MoveIndex((int)PosGrip.GRIP);
								break;
							}

							m_tePosZ = GetZPos(m_tePlacePosX);

							smm.Next(Cmd.PLACE_70);
						}
						break;

					case Cmd.PLACE_70:
						{
							if (arrMt[Axis.Z].IsRdy() == true)
							{
								arrMt[Axis.Z].MoveIndex((int)m_tePosZ);
								arrMt[Axis.Z].TMoveIndex((int)m_tePosZ, (int)MT.LDTrayTransZ, 20000, arrMt[Axis.Z].GetVel((int)PosZ.PLACESLOWSPEED));
							}

							if (arrMt[Axis.Z].IsRdy((int)m_tePosZ) == false) break;

							if (smm.Delayms(100) == false) break;

							smm.Next(Cmd.PLACE_80);
						}
						break;

					case Cmd.PLACE_80:
						{
							if (GripCheck(m_tePlacePosX) == false)
							{
								GripStart(m_tePlacePosX);
								break;
							}

							SM.FMT.Next(FlowTray.LDTrans, GetFlow(m_tePlacePosX));

							if (arrMt[Axis.Grip].IsHomeDone() == false)
							{
								arrMt[Axis.Grip].StartHoming();
							}

							smm.Next(Cmd.PLACE_90);
						}
						break;

					case Cmd.PLACE_90:
						{
							if (arrMt[Axis.Grip].IsHomeDone() == false) break;

							if (arrMt[Axis.Grip].IsRdy((int)PosGrip.RELEASE) == false)
							{
								arrMt[Axis.Grip].MoveIndex((int)PosGrip.RELEASE);
								break;
							}

							smm.Next(Cmd.PLACE_100);
						}
						break;

					case Cmd.PLACE_100:
						{
							if (arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false)
							{
								arrMt[Axis.Z].MoveIndex((int)PosZ.READY);
								break;
							}

							smm.Next(Cmd.PLACE_110);
						}
						break;

					case Cmd.PLACE_110:
						{

							smm.Next(Cmd.PLACE_120);
						}
						break;

					case Cmd.PLACE_120:
						{
							if (arrMt[Axis.X].IsRdy((int)PosX.LOADCONV) == false)
							{
								InterLockCheckMoveIndex(PosX.LOADCONV);
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

		private void PlaceConv(SMM<Cmd> smm)
		{
			try
			{
				int ntray_height = SM.SHM.GetRecipeDataToInt(DEVICE.TrayHeight);

				if (smm.CurStep != Cmd.START && CheckCollide(smm) == true) return;

				if (smm.CurStep >= Cmd.PLACE_CONV_10 &&
					smm.CurStep != Cmd.PLACE_CONV_60 &&
					smm.CurStep < Cmd.PLACE_CONV_100 &&
					arrMt[Axis.Z].GetLocalPos() > (arrMt[Axis.Z].GetIndexPos((int)PosZ.LOADCONVLIMIT) - ntray_height / 4))
				{
					arrMt[Axis.Z].Stop();
					smm.Next(Cmd.PLACE_CONV_60);
				}

				switch (smm.CurStep)
				{
					case Cmd.START:
						{
							if (smm.eInput == InState.run)
							{
								smm.eInput = InState.none;

								smm.Next(Cmd.PLACE_CONV_10);
							}
						}
						break;

					case Cmd.PLACE_CONV_10:
						{
							//트레이를 잡고 있는지 체크
							if (SM.FMT.IsFlow(FlowTray.LDTrans) == false ||
								G.DryRunIsAIn(DI.LDTrayTransExistSen, true) == false)
							{
								AddAlarm(ALM.ERROR_SEQ_LOAD_TRANSFER_TRAY_NONE);
								smm.Next(Cmd.END);
								break;
							}

							if (arrAct[Act.Holder].IsPos(SenPos.BWD) == false)
							{
								arrAct[Act.Holder].SetPos(SenPos.BWD);
								break;
							}

							smm.Next(Cmd.PLACE_CONV_20);
						}
						break;

					case Cmd.PLACE_CONV_20:
						{
							if (arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false)
							{
								arrMt[Axis.Z].MoveIndex((int)PosZ.READY);
								break;
							}

							smm.Next(Cmd.PLACE_CONV_30);
						}
						break;

					case Cmd.PLACE_CONV_30:
						{
							if (arrMt[Axis.Grip].IsHomeDone() == true &&
								arrMt[Axis.Grip].IsRdy((int)PosGrip.GRIPHARD) == false)
							{
								arrMt[Axis.Grip].MoveIndex((int)PosGrip.GRIPHARD);
								break;
							}

							smm.Next(Cmd.PLACE_CONV_40);
						}
						break;

					case Cmd.PLACE_CONV_40:
						{
							if (arrMt[Axis.X].IsRdy((int)PosX.LOADCONV) == false)
							{
								InterLockCheckMoveIndex(PosX.LOADCONV);
								break;
							}
							if (G.cKernel.IsAIn(DI.LDTrayConvExistSen) == false)
							{
								//트레이가 없는 경우 제일 마지막위치로 바로 이동
								smm.Next(Cmd.PLACE_CONV_60);
								break;
							}

							m_nConvDownFastPos = arrMt[Axis.Z].GetIndexPos((int)PosZ.LOADCONVLIMIT) - (ntray_height *  SM.FMT.GetExistCountRange(FlowTray.LDConv01, FlowTray.LDConv31)) - ntray_height;

							if (m_nConvDownFastPos < arrMt[Axis.Z].GetIndexPos((int)PosZ.READY))
							{
								m_nConvDownFastPos = arrMt[Axis.Z].GetIndexPos((int)PosZ.READY);
							}

							smm.Next(Cmd.PLACE_CONV_45);
						}
						break;

					case Cmd.PLACE_CONV_45: //안전위치까지 빠르게 이동
						{
							if (arrMt[Axis.Z].IsRdyPos(m_nConvDownFastPos) == true)
							{
								smm.Next(Cmd.PLACE_CONV_50);
								break;
							}
							else if (arrMt[Axis.Z].IsRdy() == true)
							{
								arrMt[Axis.Z].MoveABS(m_nConvDownFastPos, (int)PosZ.LOADCONV);
							}

							if (G.DryRunIsBIn(DI.LDTrayTransPosSen, true) == true)
							{
								arrMt[Axis.Z].Stop();
								smm.Next(Cmd.PLACE_CONV_70);
								break;
							}
						}
						break;

					case Cmd.PLACE_CONV_50: //센서 감지 될때까지 다운
						{
							if (arrMt[Axis.Z].IsRdy((int)PosZ.LOADCONVLIMIT) == true)
							{
								smm.Next(Cmd.PLACE_CONV_100);
								break;
							}
							else if (arrMt[Axis.Z].IsRdy() == true)
							{
								arrMt[Axis.Z].MoveIndex((int)PosZ.LOADCONVLIMIT);
							}

							if (G.DryRunIsBIn(DI.LDTrayTransPosSen, true) == true)
							{
								arrMt[Axis.Z].Stop();
								smm.Next(Cmd.PLACE_CONV_70);
								break;
							}
						}
						break;

					case Cmd.PLACE_CONV_60:
						{

							if (arrMt[Axis.Z].IsRdy((int)PosZ.LOADCONVLIMIT) == false)
							{
								arrMt[Axis.Z].MoveIndex((int)PosZ.LOADCONVLIMIT);
								break;
							}

							smm.Next(Cmd.PLACE_CONV_100);
						}
						break;

					case Cmd.PLACE_CONV_70://감지 안될때까지 업
						{
							if (arrMt[Axis.Z].IsRdy() == true)
							{
								arrMt[Axis.Z].MoveJog(false, arrMt[Axis.Z].GetVel((int)PosZ.TRAYFINDUPSPEED));
							}

							//감지가 안되면 정지
							if (G.DryRunIsBIn(DI.LDTrayTransPosSen, false) == true) break;

							arrMt[Axis.Z].Stop();

							smm.Next(Cmd.PLACE_CONV_80);
						}
						break;

					case Cmd.PLACE_CONV_80:
						{
							if (arrMt[Axis.Z].IsRdy() == true)
							{
								arrMt[Axis.Z].MoveJog(true, 5000);
							}

							if (G.DryRunIsBIn(DI.LDTrayTransPosSen, true) == false) break;

							arrMt[Axis.Z].Stop();

							m_nConvPlacePos = arrMt[Axis.Z].GetLocalPos() + SM.SHM.GetRecipeDataToInt(DEVICE.ULDTransDnDist);

							smm.Next(Cmd.PLACE_CONV_90);
						}
						break;

					case Cmd.PLACE_CONV_90:
						{
							if (arrMt[Axis.Grip].IsHomeDone() == true &&
								arrMt[Axis.Grip].IsRdy((int)PosGrip.GRIP) == false)
							{
								arrMt[Axis.Grip].MoveIndex((int)PosGrip.GRIP);
							}

							if (arrMt[Axis.Z].GetLocalPos() > arrMt[Axis.Z].GetIndexPos((int)PosZ.LOADCONVLIMIT))
							{
								arrMt[Axis.Z].Stop();
								smm.Next(Cmd.PLACE_CONV_100);
								break;
							}

							if (m_bPnPConvTeaching == true)
							{
								smm.Next(Cmd.END);
								break;
							}

							if (arrMt[Axis.Z].IsRdyPos(m_nConvPlacePos) == false)
							{
								arrMt[Axis.Z].MoveABS(m_nConvPlacePos, (int)PosZ.LOADCONVLIMIT);
								break;
							}

							smm.Next(Cmd.PLACE_CONV_100);
						}
						break;

					case Cmd.PLACE_CONV_100:
						{
							if (arrMt[Axis.Z].GetLocalPos() > arrMt[Axis.Z].GetIndexPos((int)PosZ.LOADCONVLIMIT))
							{
								arrMt[Axis.Z].Stop();
							}

							if (smm.Delayms(200) == false) break;
							if (arrMt[Axis.Z].IsRdy() == false) break;

							FlowTray flow = SM.FMT.GetFirstExistFlow(FlowTray.LDConv01, FlowTray.LDConv31) - 1;
							if (flow < FlowTray.LDConv01 && flow == FlowTray.Empty)
							{
								AddAlarm(ALM.ERROR_FLOW_MANAGER_CHECK);
								smm.Next(Cmd.END);
								break;
							}


							if (SM.FMT.NextInsert(FlowTray.LDTrans, FlowTray.LDConv01) == false)
							{
								AddAlarm(ALM.ERROR_FLOW_MANAGER_CHECK);
								break;
							}


							if (arrMt[Axis.Grip].IsHomeDone() == false)
							{
								arrMt[Axis.Grip].StartHoming();
							}

							smm.Next(Cmd.PLACE_CONV_110);
						}
						break;

					case Cmd.PLACE_CONV_110:
						{
							if (arrMt[Axis.Grip].IsHomeDone() == false) break;

							if (arrMt[Axis.Grip].IsRdy((int)PosGrip.RELEASE) == false)
							{
								arrMt[Axis.Grip].MoveIndex((int)PosGrip.RELEASE);
								break;
							}

							smm.Next(Cmd.PLACE_CONV_120);
						}
						break;

					case Cmd.PLACE_CONV_120:
						{
							if (arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false)
							{
								arrMt[Axis.Z].MoveIndex((int)PosZ.READY);
								break;
							}

							smm.Next(Cmd.END);
						}
						break;

					case Cmd.END:
						{
							m_bPnPConvTeaching = false;
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

		private void PnP(SMM<Cmd> smm)
		{
			try
			{
				if (smm.CurStep != Cmd.START && CheckCollide(smm) == true) return;

				switch (smm.CurStep)
				{
					case Cmd.START:
						{
							if (smm.eInput == InState.run)
							{
								smm.eInput = InState.none;

								smm.Next(Cmd.PnP_10);
								GLog.Add(string.Format("LDTRANS PickAndPlace - Step : {0}, Pick : {1}, Place : {2}", Cmd.PnP_10, m_tePickPosX, m_tePlacePosX));
							}
						}
						break;

					case Cmd.PnP_10:
						{
							//PnP동작 예외체크
							if (PnPPosCheck() == false)
							{
								AddAlarm(ALM.ERROR_SEQ_LOAD_TRANSFER_PNP_POSITION);
								smm.Next(Cmd.END);
								break;
							}
							//PickPos Check - flow에 없으면 알람
							if (SM.FMT.IsFlow(GetFlow(m_tePickPosX)) == false)
							{
								GLog.Add(string.Format("ERROR : PickPos = {0} , IsFlow = {1}", m_tePickPosX.ToString(), SM.FMT.IsFlow(GetFlow(m_tePickPosX)).ToString()));
								AddAlarm(ALM.ERROR_FLOW_MANAGER_CHECK);
								smm.Next(Cmd.END);
								break;
							}
							//PlacePos Check - flow에 있으면 알람
							if (SM.FMT.IsFlow(GetFlow(m_tePlacePosX)) == true &&
								m_tePlacePosX != PosX.LOADCONV)
							{
								GLog.Add(string.Format("ERROR : PlacePos = {0} , IsFlow = {1}", m_tePlacePosX.ToString(), SM.FMT.IsFlow(GetFlow(m_tePlacePosX)).ToString()));
								AddAlarm(ALM.ERROR_FLOW_MANAGER_CHECK);
								smm.Next(Cmd.END);
								break;
							}

							smm.Next(Cmd.PnP_20);
						}
						break;

					case Cmd.PnP_20:
						{
							if (m_tePickPosX == PosX.LOADCONV)
							{
								m_smmPickConv.eInput = InState.run;
							}
							else
							{
								m_smmPick.eInput = InState.run;
							}

							smm.Next(Cmd.PnP_30);
						}
						break;

					case Cmd.PnP_30:
						{
							if (m_tePickPosX == PosX.LOADCONV)
							{
								if (IsPickConvReady() == false) break;
							}
							else
							{
								if (m_smmPick.CurStep >= Cmd.PICK_50 && m_smmPick.CurStep <= Cmd.PICK_80)
								{
									switch (m_tePlacePosX)
									{
										case PosX.LOADTABLE1:
											if (G.seqLoadTrayTable1.IsReady == false) return;
											if (G.seqLoadTrayTable1.IsRelease() == false)
											{
												AddAlarm(ALM.ERROR_SEQ_LOAD_TRAY_TABLE1_NOT_RELEASE);
												break;
											}
											
											break;
										case PosX.LOADTABLE2:
											if (G.seqLoadTrayTable2.IsReady == false) return;
											if (G.seqLoadTrayTable2.IsRelease() == false)
											{
												AddAlarm(ALM.ERROR_SEQ_LOAD_TRAY_TABLE1_NOT_RELEASE);
												break;
											}
											
											break;
									}
								}

								if (IsPickReady() == false) break;
							}

							smm.Next(Cmd.PnP_40);
						}
						break;

					case Cmd.PnP_40:
						{
							if (SM.FMT.IsFlow(FlowTray.LDTrans) == false)
							{
								smm.Next(Cmd.END);
								break;
							}
							if (m_tePlacePosX == PosX.LOADCONV)
							{
								m_smmPlaceConv.eInput = InState.run;
							}
							else
							{
								m_smmPlace.eInput = InState.run;
							}

							smm.Next(Cmd.PnP_50);
						}
						break;

					case Cmd.PnP_50:
						{
							if (m_tePlacePosX == PosX.LOADCONV)
							{
								if (IsPlaceConvReady() == false) break;
							}
							else
							{
								if (IsPlaceReady() == false) break;
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


		private void Ready(SMM<Cmd> smm)
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

								smm.Next(Cmd.READY_10);
							}
						}
						break;
					case Cmd.READY_10:
						{
							if ((G.DryRunIsAIn(DI.LDTrayTransExistSen, false) == true || 
								SM.FMT.IsFlow(FlowTray.LDTrans) == true)  && NDm.Read(eNDM.AutoRunMode) == (int)AutoRunMode.Normal )
							{
								AddAlarm(ALM.ERROR_FLOW_MANAGER_CHECK);
								//AddAlarm(ALM.ERROR_SEQ_SP_LOAD_TRANSFER_TRAY_EXIST);
								smm.Next(Cmd.END);
								break;
							}

							smm.Next(Cmd.READY_20);
						}
						break;
					case Cmd.READY_20:
						{
							if (arrMt[Axis.Grip].IsRdy((int)PosGrip.RELEASE) == false)
							{
								arrMt[Axis.Grip].MoveIndex((int)PosGrip.RELEASE);
								break;
							}

							smm.Next(Cmd.READY_30);
						}
						break;
					case Cmd.READY_30:
						{
							if (arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false)
							{
								NDm.Write(eNDM.ManualMotorMoveDir, -1);
								arrMt[Axis.Z].MoveIndex((int)PosZ.READY);
								break;
							}

							smm.Next(Cmd.END);
						}
						break;
					case Cmd.READY_40:
						{
							if (arrMt[Axis.X].IsRdy((int)PosX.LOADCONV) == false)
							{
								InterLockCheckMoveIndex(PosX.LOADCONV);
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
