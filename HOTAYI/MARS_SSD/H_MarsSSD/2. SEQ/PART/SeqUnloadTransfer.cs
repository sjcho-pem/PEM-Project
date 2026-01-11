using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace H_MarsSSD
{
	public class SeqUnloadTransfer : SeqBase<SeqUnloadTransfer.Act, SeqUnloadTransfer.Axis, SeqUnloadTransfer.Cmd>
	{
		#region [ Enum ]

		public enum Axis
		{
			X,
			Z,
			Grip,

			Max,
		}

		public enum Act
		{
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
			HOME_110,

			TrayUnload1,
			TrayUnload2,
			TrayUnload3,
			TrayUnload4,
			TrayUnload5,
			TrayUnload6,
			TrayUnload7,
			TrayUnload8,
			TrayUnload9,
			TrayUnload10,
			TrayUnload11,
			TrayUnload12,
			TrayUnload13,
			TrayUnload14,
			TrayUnload15,
			TrayUnload16,
			TrayUnload17,

			PLACE_10,
			PLACE_20,
			PLACE_30,
			PLACE_40,
			PLACE_50,
			PLACE_60,
			PLACE_70,
			PLACE_80,
			PLACE_90,
			PLACE_91,
			PLACE_100,
			PLACE_110,
			PLACE_120,
			PLACE_CHECKCAPCOVER_1,
			PLACE_CHECKCAPCOVER_2,
			PLACE_CHECKCAPCOVER_3,
			PLACE_130,

			PLACE_140,
			PLACE_150,
			PLACE_160,
			PLACE_170,
			PLACE_180,
			PLACE_190,
			PLACE_200,
			PLACE_210,
			PLACE_220,

			PLACE_CAP_10,
			PLACE_CAP_20,
			PLACE_CAP_30,
			PLACE_CAP_40,
			PLACE_CAP_50,
			PLACE_CAP_60,
			PLACE_CAP_70,
			PLACE_CAP_80,
			PLACE_CAP_90,

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
			
			PICK_10,
			PICK_10_MESCHECK1,
			PICK_10_MESCHECK2,
			PICK_10_MESCHECK3,
			PICK_20,
			PICK_22,
			PICK_30,
			PICK_40,
			PICK_41,
			PICK_50,
			PICK_60,
            PICK_70,
            PICK_71,
            PICK_72,
            PICK_73,
            PICK_74,
            PICK_75,
            PICK_80,
			PICK_81,
			PICK_82,
			PICK_83,
			PICK_90,
			PICK_100,
			PICK_110,
			PICK_120,
			PICK_130,
			PICK_140,
			PICK_150,
			PICK_CHECKBARCODE_1,
			PICK_CHECKBARCODE_2,
			PICK_CHECKBARCODE_3,

			PnP_10,
			PnP_20,
			PnP_30,
			PnP_40,
			PnP_50,
			PnP_60,
			PnP_70,
			PnP_80,
			PnP_90,
		}

		public enum PosX
		{
			TRAYCLEAN,
			UNLOADTABLE1,
			UNLOADTABLE2,
			UNLOADCONV,
			UNKNOWN	= -1,
		}

		public enum PosGrip
		{
			RELEASE,
			GRIP,
			GRIPHARD,
			GRIPUNLOAD,
		}

		public enum PosZ
		{
			READY,
			UNLOADCONV,
			UNLOADCONVLIMIT,
			TRAYCLEAN,
			UNLOADTABLE1,
			UNLOADTABLE2,
			TRAYFINDUPSPEED,
			PLACESLOWSPEED,
			DRYRUN,
			UNKNOWN = -1,
		}
		#endregion

		#region [Variables]
		private SMM<Cmd>	m_smmHome		= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.ULDTRANS_HOME));
		private SMM<Cmd>	m_smmPlace		= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.ULDTRANS_PLACE));
		private SMM<Cmd>	m_smmPlaceConv	= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.ULDTRANS_PLACECONV));
		private SMM<Cmd>	m_smmPick		= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.ULDTRANS_PICK));
		private SMM<Cmd>	m_smmPnP		= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.ULDTRANS_PNP));

		public	Cmd	smmHome				{ get => m_smmHome.CurStep; }
		public	Cmd	smmPlace			{ get => m_smmPlace.CurStep; }
		public	Cmd	smmPlaceConv		{ get => m_smmPlaceConv.CurStep; }
		public	Cmd	smmPick				{ get => m_smmPick.CurStep; }
		public	Cmd	smmPnP				{ get => m_smmPnP.CurStep; }

		private PosX		    m_tePickPosX	= PosX.UNKNOWN;
		private PosX		    m_tePlacePosX	= PosX.UNKNOWN;
		private PosZ		    m_tePosZ		= PosZ.UNKNOWN;

		private PosZ		    m_CurloadPos;
		private PosZ		    m_CurUnloadPos;

		private	bool		    m_bPlaceConvTeaching	= false;
		private int			    m_nConvPlacePos			= 0;
		private int			    m_nConvDownFastPos		= 0;
		private	bool		    m_IsMtHome				= true;
		private	int			    m_SgemRetryCount		= 0;
		#endregion

		#region [Interlock]
		
		//언로드트레이 체크용 
		//0 : 로드트레이1 , 1 : 로드트레이2 
		public	bool	UnloadTableUpDnSafety(int nTableNo)
		{
			if (arrMt[Axis.X].IsHomeDone() == false)			return SM.SHM.InterlockMsgAdd("arrMt[teAxis.X].IsHomeDone() == false");

			switch (nTableNo)
			{
				case 0: //언로드트레이1
					{
						//안전한 위치에서 레디일때만 true
						if (arrMt[Axis.X].IsRdy() == true)
						{
							if (arrMt[Axis.X].IsSmallerThanOrEqual((int)PosX.UNLOADTABLE2) == true  )
							{ 
								SM.SHM.InterlockMsgReset();
								return true;
							}
						}
					}
					break;
				case 1: //언로드트레이2
					{
						if (arrMt[Axis.X].IsRdy() == true)
						{
							if (arrMt[Axis.X].IsSmallerThanOrEqual((int)PosX.UNLOADCONV) == true  ||
								arrMt[Axis.X].IsBiggerThanOrEqual((int)PosX.TRAYCLEAN) == true )
							{
								SM.SHM.InterlockMsgReset();
								return true;
							}
						}
					}
					break;
			}

			return SM.SHM.InterlockMsgAdd("arrMt[teAxis.X] 위치 확인");
		}
		
		public	bool	TrayCleanUpSafety()
		{
			int nIdx = arrMt[Axis.X].GetCmdIndex();

			//이동중인 위치
			if ((PosX)nIdx == PosX.TRAYCLEAN)
				return false;

			if ((PosX)nIdx == PosX.UNLOADTABLE1)
				return false;

			//현재위치
			if (arrMt[Axis.X].GetLocalPos() > arrMt[Axis.X].GetIndexPos((int)PosX.UNLOADTABLE2) &&
				arrMt[Axis.X].GetLocalPos() >= arrMt[Axis.X].GetIndexPos((int)PosX.TRAYCLEAN))
				return false;
			return true;
		}

		public bool LoadTransferMoveXSafety(PosX xPos)
		{
			try
			{
				if (arrMt[Axis.X].IsSVON() == false)		return SM.SHM.InterlockMsgAdd("arrMt[Axis.X].IsSVON() == false");
				if (arrMt[Axis.X].IsHomeDone() == false)	return SM.SHM.InterlockMsgAdd("arrMt[Axis.X].IsHomeDone() == false");

				switch (xPos)
				{
					case PosX.TRAYCLEAN:
						int nIdx = arrMt[Axis.X].GetCmdIndex();

						//이동중인 위치
						if ((PosX)nIdx == PosX.TRAYCLEAN)
							return false;

						if ((PosX)nIdx == PosX.UNLOADTABLE1)
							return false;

						//현재위치
						if (arrMt[Axis.X].GetLocalPos() > arrMt[Axis.X].GetIndexPos((int)PosX.UNLOADTABLE2) &&
							arrMt[Axis.X].GetLocalPos() >= arrMt[Axis.X].GetIndexPos((int)PosX.TRAYCLEAN))
							return false;

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

		public	bool	UnloadConvMoveSafety()
		{
			if (arrMt[Axis.X].IsHomeDone() == false)			return SM.SHM.InterlockMsgAdd("arrMt[teAxis.X].IsHomeDone() == false");
			if (arrMt[Axis.Z].IsHomeDone() == false)			return SM.SHM.InterlockMsgAdd("arrMt[teAxis.Z].IsHomeDone() == false");

			if (arrMt[Axis.X].IsSmallerThan((int)PosX.UNLOADTABLE2) == true  &&
				arrMt[Axis.Z].IsBiggerThan((int)PosZ.READY) == true)
			{
				return SM.SHM.InterlockMsgAdd("arrMt[teAxis.X].IsSmallerThan((int)tePosX.UNLOADTABLE2) == true  &&\narrMt[teAxis.Z].IsBiggerThan((int)tePosZ.READY) == true");
			}

			SM.SHM.InterlockMsgReset();
			return true;
		}

		public	bool	UnloadLiftMoveSafety()
		{
			if (arrMt[Axis.Z].IsRdyPos(arrMt[Axis.Z].GetIndexPos((int)PosZ.READY))  == false)
			{
				return false;
			}

			return true;
		}

		////X축 이동시 인터락체크용
		public bool InterLockCheckMoveIndex(PosX xPos)
		{
			//check 트레이 클린쪽도 확인해서 움직여야 할까?
			if (G.cKernel.IsBIn(DI.TrayTransCollideSen) == true)
			{
				arrMt[Axis.X].Stop();
				return false;
			}

			//1 = + 방향 , -1 = - 방향
			int nDir = -1;
			if (arrMt[Axis.X].GetIndexPos((int)xPos) > arrMt[Axis.X].GetLocalPos())
			{
				nDir = 1;
			}

			switch (xPos)
			{
				case PosX.UNLOADCONV:
					{
						if (nDir == -1)
						{
							if (arrMt[Axis.X].IsBiggerThan((int)PosX.UNLOADTABLE1) == true &&
								G.seqUnloadTrayTable1.IsMotorStats(SeqUnloadTrayTable.PosZ.UP) == false &&
								G.seqUnloadTrayTable1.IsMotorStats(SeqUnloadTrayTable.PosZ.DOWN) == false)
							{
								if (arrMt[Axis.X].IsInPosIndex((int)PosX.UNLOADTABLE1) == false)
									return false;
							}

							if (arrMt[Axis.X].IsBiggerThan((int)PosX.UNLOADTABLE2) == true &&
								G.seqUnloadTrayTable2.IsMotorStats(SeqUnloadTrayTable.PosZ.UP) == false &&
								G.seqUnloadTrayTable2.IsMotorStats(SeqUnloadTrayTable.PosZ.DOWN) == false)
                            {
                                if (arrMt[Axis.X].IsInPosIndex((int)PosX.UNLOADTABLE2) == false)
                                    return false;
							}
						}
					}
					break;
				case PosX.TRAYCLEAN:
					{
						if (nDir == 1)
						{
							if ((G.seqLoadTransfer.UnloadTransferMoveXSafety(SeqLoadTransfer.PosX.TRAYCLEAN) == false) ||
								(G.seqTrayClean.IsSafety() == false) ||
								(G.seqUnloadTrayTable1.IsMotorStats(SeqUnloadTrayTable.PosZ.UP) == false) ||
								(G.seqUnloadTrayTable2.IsMotorStats(SeqUnloadTrayTable.PosZ.UP) == false && G.seqUnloadTrayTable1.IsMotorStats(SeqUnloadTrayTable.PosZ.DOWN) == false))
							{
								return false;
							}
						}
						else
						{
							if (G.seqUnloadTrayTable1.IsMotorStats(SeqUnloadTrayTable.PosZ.UP) == false ||
								(G.seqUnloadTrayTable2.IsMotorStats(SeqUnloadTrayTable.PosZ.UP) == false && G.seqUnloadTrayTable2.IsMotorStats(SeqUnloadTrayTable.PosZ.DOWN) == false))
							{
								return false;
							}
						}
					}
					break;

				case PosX.UNLOADTABLE1:
					{
						if (nDir == 1)
						{
							if (G.seqUnloadTrayTable1.IsMotorStats(SeqUnloadTrayTable.PosZ.DOWN) == false)
                            {
                                return false;
                            }



							if (G.seqUnloadTrayTable2.IsMotorStats(SeqUnloadTrayTable.PosZ.UP) == false && 
								G.seqUnloadTrayTable2.IsMotorStats(SeqUnloadTrayTable.PosZ.DOWN) == false)
                            {
                                if (arrMt[Axis.X].IsInPosIndex((int)PosX.UNLOADTABLE2) == false)
                                    return false;
							}
						}
						else
						{
							if (G.seqUnloadTrayTable1.IsMotorStats(SeqUnloadTrayTable.PosZ.DOWN) == false)
							{
								return false;
							}
						}
					}
					break;

				case PosX.UNLOADTABLE2:
					{
						if (nDir == 1)
						{
							if (G.seqUnloadTrayTable2.IsMotorStats(SeqUnloadTrayTable.PosZ.DOWN) == false)
							{
								return false;
							}
						}
						else
                        {
                            if (G.seqUnloadTrayTable1.IsMotorStats(SeqUnloadTrayTable.PosZ.UP) == false && G.seqUnloadTrayTable1.IsMotorStats(SeqUnloadTrayTable.PosZ.DOWN) == false)
                            {
                                if (arrMt[Axis.X].IsInPosIndex((int)PosX.UNLOADTABLE1) == false &&
									arrMt[Axis.X].IsInPosIndex((int)PosX.TRAYCLEAN) == false)
                                    return false;
                            }

                            if (G.seqUnloadTrayTable2.IsMotorStats(SeqUnloadTrayTable.PosZ.DOWN) == false && G.seqUnloadTrayTable2.IsMotorStats(SeqUnloadTrayTable.PosZ.UP) == false)
                            {
                                return false;
                            }
                        }
					}
					break;
					
			}

			arrMt[Axis.X].MoveIndex((int)xPos);

			return true;
		}
		#endregion

		#region [Function]
		public	void	Init()
		{
			try
			{
				lstSMM.Add(m_smmHome);
				lstSMM.Add(m_smmPlace);
				lstSMM.Add(m_smmPlaceConv);
				lstSMM.Add(m_smmPick);
				lstSMM.Add(m_smmPnP);

				#region - Motor
				arrMt = new EnumArray<Axis, CMotor>(Axis.Max);
				arrMt[Axis.X]		= G.cKernel.Motor(MT.ULDTrayTransX);
				arrMt[Axis.Z]		= G.cKernel.Motor(MT.ULDTrayTransZ);
				arrMt[Axis.Grip]	= G.cKernel.Motor(MT.ULDTrayTransGrip);
				#endregion

				#region - Actuator            
				arrAct = new EnumArray<Act, Actuator>(Act.Max);

				/*
				sensor List
				ULDTrayTransExistSen		- 포토센서, 트레이 잡앗을때, 감지
				ULDTrayTransPosSen			- 대각선 센서, 모터의 홈센서 처럼 자재위치 잡을 때 사용
				ULDTrayTransProdExistSen	- Z축 리미트, 인터락 형식으로 사용
				TrayTransCollideSen			- 충돌 방지
				*/

				#endregion
				

				base.Initialize();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public	bool	HomeStart(bool IsMtHome = true)		{ m_IsMtHome = IsMtHome; return SetSeqRun(ref m_smmHome); }

		public bool PickStart(PosX TargetPos)
		{
			try
			{
				m_tePickPosX = TargetPos;
				return SetSeqRun(ref m_smmPick);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public bool PlaceStart(PosX TargetPos)
		{
			try
			{
				m_tePlacePosX = TargetPos;
				return SetSeqRun(ref m_smmPlace);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public bool PlaceConvStart(bool bTeaching = false)
		{
			try
			{
				m_bPlaceConvTeaching = bTeaching;
				return SetSeqRun(ref m_smmPlaceConv);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public bool PnPStart(PosX PickPos, PosX PlacePos)
		{
			try
			{
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
				if (SM.FMT.IsFlow(FlowTray.ULDTrans) == true && G.DryRunIsAIn(DI.ULDTrayTransExistSen, true) == false)
				{
					AddAlarm(ALM.ERROR_SEQ_UNLOAD_TRANSFER_TRAY_NONE);
				}
				else if (SM.FMT.IsFlow(FlowTray.ULDTrans) == false && G.DryRunIsAIn(DI.ULDTrayTransExistSen, false) == true)
				{
					AddAlarm(ALM.ERROR_SEQ_UNLOAD_TRANSFER_TRAY_EXIST);
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
					case PosX.UNLOADCONV:		return G.seqTrayUnloader.IsUnloadEnable();
					case PosX.TRAYCLEAN:		return G.seqTrayClean.IsLoadEnable();
					case PosX.UNLOADTABLE1:		return G.seqUnloadTrayTable1.IsLoadEnable();
					case PosX.UNLOADTABLE2:		return G.seqUnloadTrayTable2.IsLoadEnable();
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
				switch (TargetPos)
				{
					case PosX.TRAYCLEAN:		return G.seqTrayClean.IsUnloadEnable();
					case PosX.UNLOADTABLE1:		return G.seqUnloadTrayTable1.IsUnloadEnable();
					case PosX.UNLOADTABLE2:		return G.seqUnloadTrayTable2.IsUnloadEnable();
				}
				return false;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}
		
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
					case PosX.TRAYCLEAN:	return PosZ.TRAYCLEAN;
					case PosX.UNLOADTABLE1: return PosZ.UNLOADTABLE1;
					case PosX.UNLOADTABLE2: return PosZ.UNLOADTABLE2;
				}

				return PosZ.UNKNOWN;
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
			switch (TargetPos)
			{
				case PosX.UNLOADCONV:		return SM.FMT.GetFirstEmptyFlow(FlowTray.ULDConv01, FlowTray.ULDConv31);
				case PosX.TRAYCLEAN:		return FlowTray.TrayClean;
				case PosX.UNLOADTABLE1:		return FlowTray.ULDTable1;
				case PosX.UNLOADTABLE2:		return FlowTray.ULDTable2;
				default:					return FlowTray.Empty;
			}
		}

		private void GripStart(PosX TargetPos)
		{
			try
			{
				switch (TargetPos)
				{
					case PosX.TRAYCLEAN:
						G.seqTrayClean.GripStart();
						break;
					case PosX.UNLOADTABLE1:
						G.seqUnloadTrayTable1.GripStart();
						break;
					case PosX.UNLOADTABLE2:
						G.seqUnloadTrayTable2.GripStart();
						break;
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
					case PosX.TRAYCLEAN:
						G.seqTrayClean.ReleaseStart();
						break;
					case PosX.UNLOADTABLE1:
						G.seqUnloadTrayTable1.ReleaseStart();
						break;
					case PosX.UNLOADTABLE2:
						G.seqUnloadTrayTable2.ReleaseStart();
						break;
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
					case PosX.TRAYCLEAN:
						return G.seqTrayClean.IsGrip();
					case PosX.UNLOADTABLE1:
						return G.seqUnloadTrayTable1.IsGrip();
					case PosX.UNLOADTABLE2:
						return G.seqUnloadTrayTable2.IsGrip();
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
					case PosX.TRAYCLEAN:
						return G.seqTrayClean.IsRelease();
					case PosX.UNLOADTABLE1:
						return G.seqUnloadTrayTable1.IsRelease();
					case PosX.UNLOADTABLE2:
						return G.seqUnloadTrayTable2.IsRelease();
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
			//	string str = string.Format("ULDTRANS PnP Position Check - PICK : {0}, PLACE : {1}", m_tePickPosX.ToString(), m_tePlacePosX.ToString());
			//	GLog.Add(str);
			switch (m_tePickPosX)
			{
			
				case PosX.TRAYCLEAN:
					if (m_tePlacePosX == PosX.UNLOADTABLE1 ||
						m_tePlacePosX == PosX.UNLOADTABLE2)
					{
						return true;
					}
					break;
				case PosX.UNLOADTABLE1:
				case PosX.UNLOADTABLE2: 
					if (m_tePlacePosX == PosX.UNLOADCONV) 
						return true; 
					break;
				default: return false;
			}
			GLog.AddErr(string.Format("ULDTRANS PnP Position Check Error - PICK : {0}, PLACE : {1}", m_tePickPosX.ToString(), m_tePlacePosX.ToString()));
			return false;
		}

		private bool CheckCollide(SMM<Cmd> smm)
		{
			try
			{
				if (G.cKernel.IsBIn(DI.ULDTrayTransSafetySen) == true)
				{
					arrMt[Axis.Z].Stop();
					AddAlarm(ALM.ERROR_SEQ_UNLOAD_TRANSFER_TRAY_LIMIT_CHECK);
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
				if (SM.SHM.GetUseSkipData(USESKIP.BarcodeRead) == false)	return;
				if (SM.SHM.GetUseSkipData(USESKIP.DRYRUN) == true)			return;

				if (SM.SHM.GetRecipeData(DEVICE.ModuleType) == EnumHelper.GetDescription(ModuleType.LPCAMM))
				{
					if (G.UnloadLPCAMMBarcode.IsConnect() == false)
					{
						AddAlarm(ALM.ERROR_SEQ_UNLOAD_TRANSFER_BARCODE_READ_FAIL);
						return;
					}
					G.UnloadLPCAMMBarcode.TriggerOn();
				}
				else
				{
					if (G.UnloadOESBarcode.IsConnect() == false)
					{
						AddAlarm(ALM.ERROR_SEQ_UNLOAD_TRANSFER_BARCODE_READ_FAIL);
						return;
					}
					G.UnloadOESBarcode.TriggerOn();
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
				if (SM.SHM.GetUseSkipData(USESKIP.BarcodeRead) == false)	return;
				if (SM.SHM.GetUseSkipData(USESKIP.DRYRUN) == true)			return;

				
				if (SM.SHM.GetRecipeData(DEVICE.ModuleType) == EnumHelper.GetDescription(ModuleType.LPCAMM))
				{
					G.UnloadLPCAMMBarcode.TriggerOff();
				}
				else
				{
					G.UnloadOESBarcode.TriggerOff();
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
					return G.UnloadLPCAMMBarcode.GetResultData();
				}
				else
				{
					return G.UnloadOESBarcode.GetResultData();
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
				UpdateAct(Seq.UnloadTransfer);

				if (SM.SHM.IsBitData(BIT.Error) == true || SM.SHM.IsBitData(BIT.Pause) == true) return;

				Home(m_smmHome);
				Pick(m_smmPick);
				Place(m_smmPlace);
				PlaceConv(m_smmPlaceConv);
				PnP(m_smmPnP);

				ExistSensorCheck();

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
							if (SM.FMT.IsFlow(FlowTray.ULDTrans) == true)
							{
								//Flow 존재, 자재 감지 되는 경우 > 하단 체크 후 안풀고 홈 동작
								if (G.DryRunIsAIn(DI.ULDTrayTransExistSen, true) == true)
								{
									if (arrMt[Axis.Grip].IsInPosIndex((int)PosGrip.GRIP) == false)
									{
										arrMt[Axis.Grip].MoveIndex((int)PosGrip.GRIP);
										break;
									}
									if (arrMt[Axis.X].IsInPosIndex((int)PosX.UNLOADTABLE1) &&
										arrMt[Axis.Z].IsInPosIndex((int)PosZ.UNLOADTABLE1))
									{
										if (G.seqUnloadTrayTable1.IsRelease() == false)
										{
											G.seqUnloadTrayTable1.ReleaseStart();
											break;
										}
										else
										{
											smm.Next(Cmd.HOME_40);
											break;
										}
									}
									else if (arrMt[Axis.X].IsInPosIndex((int)PosX.UNLOADTABLE2) &&
										arrMt[Axis.Z].IsInPosIndex((int)PosZ.UNLOADTABLE2))
									{
										if (G.seqUnloadTrayTable2.IsRelease() == false)
										{
											G.seqUnloadTrayTable2.ReleaseStart();
											break;
										}
										else
										{
											smm.Next(Cmd.HOME_40);
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
											smm.Next(Cmd.HOME_40);
											break;
										}
									}
									else
									{
										smm.Next(Cmd.HOME_40);
										break;
									}
								}
								else //Flow 존재, 자재 감지 안되는 경우 > 알람 발생
								{
									AddAlarm(ALM.ERROR_SEQ_UNLOAD_TRANSFER_TRAY_NONE);
									break;
								}
							}
							//그외 Flow 없는데 , 자재 감지 되는 경우 > 그냥 홈처리 진행 > 그리퍼 풀고 올라가서 센서 체크 다시 진행 후 감지되면 알람 

							smm.Next(Cmd.HOME_20);
						}
						break;

					case Cmd.HOME_20:
						{
							//로드 트레이 테이블 실린더가 업/다운 센서가 모두 안들어온 경우 알람 처리
							if (G.seqUnloadTrayTable1.IsMotorStats(SeqUnloadTrayTable.PosZ.UP) == false && 
								G.seqUnloadTrayTable1.IsMotorStats(SeqUnloadTrayTable.PosZ.DOWN) == false)
							{
								//세이프티 다운보다 큰 경우 올리고 그외엔 내리자
								if (G.seqUnloadTrayTable1.IsBiggerThanSafetyDown())
                                {
                                    G.seqUnloadTrayTable1.UpStart(SeqUnloadTrayTable.UDMode.Manual);
									break;
								}
								else
								{
                                    G.seqUnloadTrayTable1.DownStart(SeqUnloadTrayTable.UDMode.Manual);
									break;
								}
							}
							if (G.seqUnloadTrayTable2.IsMotorStats(SeqUnloadTrayTable.PosZ.UP) == false && 
								G.seqUnloadTrayTable2.IsMotorStats(SeqUnloadTrayTable.PosZ.DOWN) == false)
							{
								if (G.seqUnloadTrayTable2.IsBiggerThanSafetyDown())
                                {
                                    G.seqUnloadTrayTable2.UpStart(SeqUnloadTrayTable.UDMode.Manual);
									break;
								}
								else
								{
                                    G.seqUnloadTrayTable2.DownStart(SeqUnloadTrayTable.UDMode.Manual);
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
							if (arrMt[Axis.Grip].IsHomeDone() == false || arrMt[Axis.Grip].IsRdy() == false) break;

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

							arrMt[Axis.Z].ClearHome();
							arrMt[Axis.Z].StartHoming();

							smm.Next(Cmd.HOME_50);
						}
						break;

					case Cmd.HOME_50:
						{
							if (arrMt[Axis.Z].IsHomeDone() == false || arrMt[Axis.Z].IsRdy() == false) break;

							if (SM.FMT.IsFlow(FlowTray.ULDTrans) != G.DryRunIsAIn(DI.ULDTrayTransExistSen, SM.FMT.IsFlow(FlowTray.ULDTrans)))
							{
								AddAlarm(ALM.ERROR_SEQ_UNLOAD_TRANSFER_TRAY_EXIST);
							}

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

							smm.Next(Cmd.HOME_70);
						}
						break;

					case Cmd.HOME_70:
						{
							if (smm.Delayms(500) == false) break;
							arrMt[Axis.X].StartHoming();

							smm.Next(Cmd.HOME_80);
						}
						break;

					case Cmd.HOME_80:
						{
							if (arrMt[Axis.X].IsHomeDone() == false || arrMt[Axis.X].IsRdy() == false) break;

							smm.Next(Cmd.HOME_90);
						}
						break;
					case Cmd.HOME_90:
						{
							if (SM.FMT.IsFlow(FlowTray.ULDTrans) == true)
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
								arrMt[Axis.Z].MoveIndex((int)PosZ.READY);
								break;
							}

							smm.Next(Cmd.HOME_110);
						}
						break;

					case Cmd.HOME_110:
						{
							if (arrMt[Axis.X].IsRdy((int)PosX.UNLOADCONV) == false)
							{
								arrMt[Axis.X].MoveIndex((int)PosX.UNLOADCONV);
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
								GLog.Add(string.Format("ULDTRANS Pick - Step : {0}, Pick : {1}", Cmd.PICK_10, m_tePickPosX));
							}
						}
						break;

					case Cmd.PICK_10:
						{
							// 예외체크
							if (G.cKernel.IsAIn(DI.ULDTrayTransExistSen) == true)
							{
								AddAlarm(ALM.ERROR_SEQ_UNLOAD_TRANSFER_TRAY_EXIST);
								smm.Next(Cmd.END);
								break;
							}

							if (SM.FMT.IsFlow(GetFlow(m_tePickPosX)) == false)
							{
								m_smmPnP.ResetStep();
								GLog.Add(string.Format("ERROR : PICKPOS = {0} , IsFlow = false  ", m_tePickPosX.ToString() ));
								AddAlarm(ALM.ERROR_FLOW_MANAGER_CHECK);
								smm.Next(Cmd.END);
								break;
							}

							smm.Next(Cmd.PICK_20);
						}
						break;
					case Cmd.PICK_10_MESCHECK1:
						{
							if (SM.SHM.GetUseSkipData(USESKIP.SGEM) == false  ||
								GetFlow(m_tePickPosX) == FlowTray.TrayClean)
							{
								smm.Next(Cmd.PICK_20);
								break;
							}

							if (G.seqUBIGEMComm.IsTrayValidationReady() == false) break;

							if (SM.FMT[GetFlow(m_tePickPosX)].GetInfo().OutTime == 0)
							{
								var info_t = SM.FMT[GetFlow(m_tePickPosX)].GetInfo();
								info_t.OutTime = DateTime.Now.Ticks;
								SM.FMT[GetFlow(m_tePickPosX)].SetInfo(info_t);
							}

							G.seqUBIGEMComm.Result_TrayValidation = false;
							smm.Next(Cmd.PICK_10_MESCHECK2);
						}
						break;
					case Cmd.PICK_10_MESCHECK2:
						{
							if (SM.SHM.GetUseSkipData(USESKIP.SGEM) == false)
							{
								smm.Next(Cmd.PICK_20);
								break;
							}

							if (G.seqUBIGEMComm.Result_TrayValidation == false)
							{
								G.seqUBIGEMComm.TrayValidationStart(GetFlow(m_tePickPosX));
								break;
							}

							smm.Next(Cmd.PICK_20);
						}
						break;

					case Cmd.PICK_20:
						{
							if (arrMt[Axis.X].IsRdy((int)m_tePickPosX) == false)
							{
								if (arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false)
								{
									arrMt[Axis.Z].MoveIndex((int)PosZ.READY);
									break;
								}
							}

							if (m_tePickPosX == PosX.UNLOADTABLE1 &&
									 arrMt[Axis.X].IsBiggerThan((int)PosX.UNLOADTABLE2) == true &&
									 G.seqUnloadTrayTable1.IsMotorStats(SeqUnloadTrayTable.PosZ.DOWN)  == false)
							{
								smm.Next(Cmd.PICK_30);//필요시 안전위치 이동 추가
								break;
							}
							else if (m_tePickPosX == PosX.UNLOADTABLE2 &&
									 arrMt[Axis.X].IsBiggerThan((int)PosX.UNLOADCONV) == true &&
									 arrMt[Axis.X].IsSmallerThan((int)PosX.UNLOADTABLE1) == true &&
									 G.seqUnloadTrayTable2.IsMotorStats(SeqUnloadTrayTable.PosZ.DOWN)  == false)
							{
								smm.Next(Cmd.PICK_30);//필요시 안전위치 이동 추가
								break;
							}
							else if (m_tePickPosX == PosX.TRAYCLEAN &&
									 arrMt[Axis.X].IsBiggerThan((int)PosX.UNLOADTABLE2) == true &&
									 G.seqUnloadTrayTable1.IsMotorStats(SeqUnloadTrayTable.PosZ.UP)  == false)
							{
								smm.Next(Cmd.PICK_30);//필요시 안전위치 이동 추가
								break;
							}

							smm.Next(Cmd.PICK_40);
						}
						break;

					case Cmd.PICK_30:
						{
							if (m_tePickPosX == PosX.UNLOADTABLE1  ||
								m_tePickPosX == PosX.TRAYCLEAN )
							{
								if (arrMt[Axis.X].IsRdy((int)PosX.UNLOADCONV) == false)
								{
									InterLockCheckMoveIndex(PosX.UNLOADCONV);
									break;
								}
							}
							else if (m_tePickPosX == PosX.UNLOADTABLE2)
							{
								if (arrMt[Axis.X].IsRdy((int)PosX.UNLOADCONV) == false)
								{
									InterLockCheckMoveIndex(PosX.UNLOADCONV);
									break;
								}
							}

							smm.Next(Cmd.PICK_40);
						}
						break;

					case Cmd.PICK_40:
						{
							switch (m_tePickPosX)
							{
								case PosX.UNLOADTABLE1:
									if (G.seqUnloadTrayTable1.IsReady == true)
									{
										G.seqUnloadTrayTable1.DownStart(SeqUnloadTrayTable.UDMode.Unload);
										smm.Next(Cmd.PICK_41);
									}
									break;
								case PosX.UNLOADTABLE2:
									if (G.seqUnloadTrayTable2.IsReady == true)
									{
										G.seqUnloadTrayTable2.DownStart(SeqUnloadTrayTable.UDMode.Unload);
										smm.Next(Cmd.PICK_41);
									}
									break;
								default:
									smm.Next(Cmd.PICK_50);
									break;
							}
						}
						break;

					case Cmd.PICK_41:
						{
							switch (m_tePickPosX)
							{
								case PosX.UNLOADTABLE1:
									if (G.seqUnloadTrayTable1.IsDownReady() == false) return;
									break;
								case PosX.UNLOADTABLE2:
									if (G.seqUnloadTrayTable2.IsDownReady() == false) return;
									break;
							}

							smm.Next(Cmd.PICK_50);
						}
						break;

					case Cmd.PICK_50:
						{
							if (CheckUnloadCondition(m_tePickPosX) == false ) break;

							if (arrMt[Axis.X].IsRdy((int)m_tePickPosX) == false)
							{
								InterLockCheckMoveIndex(m_tePickPosX);
								break;
							}

							m_tePosZ = GetZPos(m_tePickPosX);

							if (m_tePosZ == PosZ.UNKNOWN)
							{
								m_smmPnP.ResetStep();
								AddAlarm(ALM.ERROR_SEQ_UNLOAD_TRANSFER_PICK_Z_UNKNOWN);
								smm.Next(Cmd.END);
								break;
							}

							smm.Next(Cmd.PICK_60);
						}
						break;

					case Cmd.PICK_60:
						{
							if (arrMt[Axis.Z].IsRdy((int)m_tePosZ) == false)
							{
								arrMt[Axis.Z].MoveIndex((int)m_tePosZ);
								break;
							}

							m_CurUnloadPos = m_tePosZ;
							smm.Next(Cmd.PICK_70);
						}
						break;

					case Cmd.PICK_70:
						{
							if (arrMt[Axis.Grip].IsRdy((int)PosGrip.GRIP) == false)
							{
								arrMt[Axis.Grip].MoveIndex((int)PosGrip.GRIP);
								break;
							}
							if (m_tePickPosX == PosX.UNLOADTABLE1 ||
								m_tePickPosX == PosX.UNLOADTABLE2 )
							{
								smm.Next(Cmd.PICK_CHECKBARCODE_1);
								break;
							}

							smm.Next(Cmd.PICK_75);
						}
						break;

						
					#region [Check Barcode]
					case Cmd.PICK_CHECKBARCODE_1:
						{
							if (Program._bSimul_ == true)
							{
								smm.Next(Cmd.PICK_75);
								break;
							}


							if (SM.SHM.GetUseSkipData(USESKIP.BarcodeRead) == false)
							{
								smm.Next(Cmd.PICK_75);
								break;
							}

							BarcodeScanStart();

							smm.Next(Cmd.PICK_CHECKBARCODE_2);
						}
						break;
					case Cmd.PICK_CHECKBARCODE_2:
						{
							if (smm.Delayms(100) == false) break;

							BarcodeScanStop() ;

							smm.Next(Cmd.PICK_CHECKBARCODE_3);
						}
						break;
					case Cmd.PICK_CHECKBARCODE_3:
						{
							string strUnloadBarcode = GetBarcodeResult();
							if (smm.Delayms(5000) == false  && string.IsNullOrEmpty(strUnloadBarcode) == true)			break;

							if (string.IsNullOrEmpty(strUnloadBarcode) == true)
							{
								smm.ResetDelayms();
								AddAlarm(ALM.ERROR_SEQ_UNLOAD_TRANSFER_BARCODE_READ_FAIL);
								smm.Next(Cmd.PICK_CHECKBARCODE_1);
								break;
							}
							else if (strUnloadBarcode.Contains(SM.FMT[GetFlow(m_tePickPosX)].GetInfo().TrayBarcode) == false)
							{
								string msg = "Tray Scanned Not Match." + Environment.NewLine +
											 $"Before ID : {SM.FMT[GetFlow(m_tePickPosX)].GetInfo().TrayBarcode} " + Environment.NewLine +
											 $"After ID : {strUnloadBarcode} ";

								SM.SHM.SetMsg(SeqMsg.TrayBarcodeMisMatch, msg);
								SM.SHM.MsgShow(SeqMsg.TrayBarcodeMisMatch);
								AddAlarm(ALM.ERROR_SEQ_UNLOAD_TRANSFER_BARCODE_MISMATCH);
								smm.Next(Cmd.PICK_CHECKBARCODE_1);
								break;
							}

							smm.Next(Cmd.PICK_75);
						}
						break;
					#endregion



					case Cmd.PICK_75:
						{
							if (SM.FMT[GetFlow(m_tePickPosX)].GetInfo().LastTrayOfLot == true &&
								NDm.Read(eNDM.LotEndMode) == (int)LotEndMode.LotEnd &&
								(m_tePickPosX == PosX.UNLOADTABLE1 || m_tePickPosX == PosX.UNLOADTABLE2)) 
							{
								NDm.Write(eNDM.LotEndMode, (int)LotEndMode.Normal);
							}
							SM.FMT.Next(GetFlow(m_tePickPosX), FlowTray.ULDTrans);


							smm.Next(Cmd.PICK_80);
						}
						break;

					case Cmd.PICK_80:
						{
							if (smm.Delayms(100) == false) break;

							if (ReleaseCheck(m_tePickPosX) == false)
							{
								ReleaseStart(m_tePickPosX);
								break;
							}

							smm.Next(Cmd.PICK_90);
						}
						break;

					case Cmd.PICK_90:
						{
							if (smm.Delayms(100) == false) break;

							switch (m_tePickPosX)
							{
								case PosX.UNLOADTABLE1:
									if (G.seqUnloadTrayTable1.IsReady == true)
									{
										G.seqUnloadTrayTable1.UpStart(SeqUnloadTrayTable.UDMode.Unload);
										return;
									}
									break;
								case PosX.UNLOADTABLE2:
									if (G.seqUnloadTrayTable2.IsReady == true)
									{
										G.seqUnloadTrayTable2.UpStart(SeqUnloadTrayTable.UDMode.Unload);
										return;
									}
									break;
							}

							smm.Next(Cmd.PICK_100);
						}
						break;

					case Cmd.PICK_100:
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
								GLog.Add(string.Format("ULDTRANS Place - Step : {0}, Place : {1}", Cmd.PLACE_10, m_tePlacePosX));
							}
						}
						break;

					case Cmd.PLACE_10:
						{
							// 예외체크
							if (SM.FMT.IsFlow(FlowTray.ULDTrans) == false)
							{
								AddAlarm(ALM.ERROR_FLOW_MANAGER_CHECK);
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
							
							if (arrMt[Axis.Grip].IsRdy((int)PosGrip.GRIPHARD) == false)
							{
								arrMt[Axis.Grip].MoveIndex((int)PosGrip.GRIPHARD);
								break;
							}

							smm.Next(Cmd.PLACE_30);
						}
						break;

					case Cmd.PLACE_30:
						{
							//필요시 안전위치 이동 추가
							if (arrMt[Axis.X].IsInPosIndex((int)PosX.TRAYCLEAN) == true && m_tePlacePosX == PosX.UNLOADTABLE1)
							{
								if (arrMt[Axis.X].IsRdy((int)PosX.UNLOADTABLE2) == false)
								{
									InterLockCheckMoveIndex(PosX.UNLOADTABLE2);
									break;
								}
							}
							smm.Next(Cmd.PLACE_40);
						}
						break;

					case Cmd.PLACE_40:
						{
							if (m_tePlacePosX == PosX.UNLOADCONV)	smm.Next(Cmd.END);
							else									smm.Next(Cmd.PLACE_50);
						}
						break;

					case Cmd.PLACE_50:
						{
							switch (m_tePlacePosX)
							{
								case PosX.UNLOADTABLE1:
									if (G.seqUnloadTrayTable1.IsReady == false) return;

									G.seqUnloadTrayTable1.DownStart(SeqUnloadTrayTable.UDMode.Load);
									break;
								case PosX.UNLOADTABLE2:
									if (G.seqUnloadTrayTable2.IsReady == false) return;

									G.seqUnloadTrayTable2.DownStart(SeqUnloadTrayTable.UDMode.Load);
									break;
								default:
									break;
							}

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

							if (arrMt[Axis.Grip].IsHomeDone() == true &&
								arrMt[Axis.Grip].IsRdy((int)PosGrip.GRIP) == false)
							{
								arrMt[Axis.Grip].MoveIndex((int)PosGrip.GRIP);
								break;
							}

							m_tePosZ = GetZPos(m_tePlacePosX);
							if (m_tePosZ == PosZ.UNKNOWN)
							{
								AddAlarm(ALM.ERROR_SEQ_UNLOAD_TRANSFER_PLACE_Z_UNKNOWN);
								smm.Next(Cmd.END);
								break;
							}

							smm.Next(Cmd.PLACE_70);
						}
						break;

					case Cmd.PLACE_70:
						{
							if (arrMt[Axis.Z].IsRdy() == true)
							{
								arrMt[Axis.Z].MoveIndex((int)m_tePosZ);
								arrMt[Axis.Z].TMoveIndex((int)m_tePosZ, (int)MT.ULDTrayTransZ, 20000, arrMt[Axis.Z].GetVel((int)PosZ.PLACESLOWSPEED));
							}

							if (arrMt[Axis.Z].IsRdy((int)m_tePosZ) == false) break;

							m_CurloadPos = m_tePosZ; // 시뮬레이션 전용

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
						
							smm.Next(Cmd.PLACE_90);
						}
						break;


					case Cmd.PLACE_90:
						{
							 if (arrMt[Axis.Grip].IsRdy((int)PosGrip.RELEASE) == false)
							{
								arrMt[Axis.Grip].MoveIndex((int)PosGrip.RELEASE);
								break;
							}
							 
							smm.Next(Cmd.PLACE_130);
						}
						break;

					case Cmd.PLACE_130:
						{
							
							SM.FMT.Next(FlowTray.ULDTrans, GetFlow(m_tePlacePosX));
							smm.Next(Cmd.PLACE_140);

							smm.Next(Cmd.PLACE_140);
						}
						break;


					case Cmd.PLACE_140:
						{
							if (arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false)
							{
								arrMt[Axis.Z].MoveIndex((int)PosZ.READY);
								break;
							}
							
							smm.Next(Cmd.PLACE_150);
						}
						break;

					case Cmd.PLACE_150:
						{
							switch (m_tePlacePosX)
							{
								case PosX.UNLOADTABLE1:
									if (G.seqUnloadTrayTable1.IsReady == false) return;

									G.seqUnloadTrayTable1.UpStart(SeqUnloadTrayTable.UDMode.Load);
									break;
								case PosX.UNLOADTABLE2:
									if (G.seqUnloadTrayTable2.IsReady == false) return;

									G.seqUnloadTrayTable2.UpStart(SeqUnloadTrayTable.UDMode.Load);
									break;
								default:
									break;
							}

							smm.Next(Cmd.PLACE_160);
						}
						break;

					case Cmd.PLACE_160:
						{
							if (m_tePlacePosX == PosX.UNLOADTABLE1)
							{
								if (arrMt[Axis.X].IsRdy((int)PosX.UNLOADCONV) == false)
								{
									InterLockCheckMoveIndex(PosX.UNLOADCONV);
									break;
								}
							}
							if (m_tePlacePosX == PosX.UNLOADTABLE2)
							{
								if (arrMt[Axis.X].IsRdy((int)PosX.UNLOADCONV) == false)
								{
									InterLockCheckMoveIndex(PosX.UNLOADCONV);
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

		private void PlaceConv(SMM<Cmd> smm)
		{
			try
			{
				int ntray_height = SM.SHM.GetRecipeDataToInt(DEVICE.TrayHeight);

				if (smm.CurStep != Cmd.START && CheckCollide(smm) == true) return;

				if (smm.CurStep >= Cmd.PLACE_CONV_10 &&
					smm.CurStep != Cmd.PLACE_CONV_60 &&
					smm.CurStep < Cmd.PLACE_CONV_100 &&
					arrMt[Axis.Z].GetLocalPos() > (arrMt[Axis.Z].GetIndexPos((int)PosZ.UNLOADCONVLIMIT) - ntray_height / 2))
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
							if (SM.FMT.IsFlow(FlowTray.ULDTrans) == false ||
								G.DryRunIsAIn(DI.ULDTrayTransExistSen, true) == false)
							{
								AddAlarm(ALM.ERROR_SEQ_UNLOAD_TRANSFER_TRAY_NONE);
								smm.Next(Cmd.END);
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
							if (arrMt[Axis.X].IsRdy((int)PosX.UNLOADCONV) == false)
							{
								InterLockCheckMoveIndex(PosX.UNLOADCONV);
								break;
							}

							m_nConvDownFastPos = arrMt[Axis.Z].GetIndexPos((int)PosZ.UNLOADCONVLIMIT) - (ntray_height * SM.FMT.GetExistCountRange(FlowTray.ULDConv01, FlowTray.ULDConv31)) - (ntray_height * 2);

							if (m_nConvDownFastPos < arrMt[Axis.Z].GetIndexPos((int)PosZ.READY))
							{
								m_nConvDownFastPos = arrMt[Axis.Z].GetIndexPos((int)PosZ.READY);
							}

							smm.Next(Cmd.PLACE_CONV_45);
						}
						break;

					case Cmd.PLACE_CONV_45:
						{
							if (arrMt[Axis.Z].IsRdyPos(m_nConvDownFastPos) == true)
							{
								smm.Next(Cmd.PLACE_CONV_50);
								break;
							}
							else if (arrMt[Axis.Z].IsRdy() == true)
							{
								arrMt[Axis.Z].MoveABS(m_nConvDownFastPos, (int)PosZ.UNLOADCONV);
							}

							if (G.DryRunIsBIn(DI.ULDTrayTransPosSen, true) == true)
							{
								arrMt[Axis.Z].Stop();
								smm.Next(Cmd.PLACE_CONV_70);
								break;
							}
						}
						break;

					case Cmd.PLACE_CONV_50:
						{
							if (arrMt[Axis.Z].IsRdy((int)PosZ.UNLOADCONVLIMIT) == true)
							{
								smm.Next(Cmd.PLACE_CONV_100);
								break;
							}
							else if (arrMt[Axis.Z].IsRdy() == true)
							{
								arrMt[Axis.Z].MoveIndex((int)PosZ.UNLOADCONVLIMIT);
							}

							if (G.DryRunIsBIn(DI.ULDTrayTransPosSen, true) == true)
							{
								arrMt[Axis.Z].Stop();
								smm.Next(Cmd.PLACE_CONV_70);
								break;
							}
						}
						break;

					case Cmd.PLACE_CONV_60:
						{
							if (smm.Delayms(300) == false) break;
							if (arrMt[Axis.Z].IsRdy() == false) break;

							if (arrMt[Axis.Z].IsRdy((int)PosZ.UNLOADCONVLIMIT) == false)
							{
								arrMt[Axis.Z].MoveIndex((int)PosZ.UNLOADCONVLIMIT);
								break;
							}

							smm.Next(Cmd.PLACE_CONV_100);
						}
						break;

					case Cmd.PLACE_CONV_70:
						{
							if (arrMt[Axis.Z].IsRdy() == true)
							{
								arrMt[Axis.Z].MoveJog(false, arrMt[Axis.Z].GetVel((int)PosZ.TRAYFINDUPSPEED));
							}

							//감지가 안되면 정지
							if (G.DryRunIsBIn(DI.ULDTrayTransPosSen, false) == true) break;

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

							if (G.DryRunIsBIn(DI.ULDTrayTransPosSen, true) == false) break;

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

							if (arrMt[Axis.Z].GetLocalPos() > arrMt[Axis.Z].GetIndexPos((int)PosZ.UNLOADCONVLIMIT))
							{
								arrMt[Axis.Z].Stop();
								smm.Next(Cmd.PLACE_CONV_100);
								break;
							}

							if (m_bPlaceConvTeaching == true)
							{
								smm.Next(Cmd.END);
								break;
							}

							if (arrMt[Axis.Z].IsRdyPos(m_nConvPlacePos) == false)
							{
								arrMt[Axis.Z].MoveABS(m_nConvPlacePos, (int)PosZ.UNLOADCONVLIMIT);
								break;
							}

							smm.Next(Cmd.PLACE_CONV_100);
						}
						break;

					case Cmd.PLACE_CONV_100:
						{
							if (arrMt[Axis.Z].GetLocalPos() > arrMt[Axis.Z].GetIndexPos((int)PosZ.UNLOADCONVLIMIT))
							{
								arrMt[Axis.Z].Stop();
							}

							if (smm.Delayms(200) == false) break;
							if (arrMt[Axis.Z].IsRdy() == false) break;

							SM.FMT.NextInsert(FlowTray.ULDTrans, FlowTray.ULDConv01);

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
							m_bPlaceConvTeaching = false;
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
								GLog.Add(string.Format("ULDTRANS PickAndPlace - Step : {0}, Pick : {1}, Place : {2}", Cmd.PnP_10, m_tePickPosX, m_tePlacePosX));
							}
						}
						break;

					case Cmd.PnP_10:
						{
							// 예외체크
							if (G.cKernel.IsAIn(DI.ULDTrayTransExistSen) == true)
							{
								AddAlarm(ALM.ERROR_SEQ_UNLOAD_TRANSFER_TRAY_EXIST);
								smm.Next(Cmd.END);
								break;
							}
							//PnP동작 예외체크
							if (PnPPosCheck() == false)
							{
								AddAlarm(ALM.ERROR_SEQ_UNLOAD_TRANSFER_PNP_POSITION);
								smm.Next(Cmd.END);
								break;
							}

							smm.Next(Cmd.PnP_20);
						}
						break;

					case Cmd.PnP_20:
						{
							m_smmPick.eInput = InState.run;

							smm.Next(Cmd.PnP_30);
						}
						break;

					case Cmd.PnP_30:
						{
							if (m_smmPick.CurStep >= Cmd.PICK_60 && m_smmPick.CurStep <= Cmd.PICK_90)
							{
								switch (m_tePlacePosX)
								{
									case PosX.UNLOADTABLE1:
										if (G.seqUnloadTrayTable1.IsReady == true)
										{
											G.seqUnloadTrayTable1.DownStart(SeqUnloadTrayTable.UDMode.Load);
										}
										break;
									case PosX.UNLOADTABLE2:
										if (G.seqUnloadTrayTable2.IsReady == true)
										{
											G.seqUnloadTrayTable2.DownStart(SeqUnloadTrayTable.UDMode.Load);
										}
										break;
									default:
										break;
								}
							}

							if (IsPickReady() == false) break;

							smm.Next(Cmd.PnP_40);
						}
						break;

					case Cmd.PnP_40:
						{
							if (m_tePlacePosX == PosX.UNLOADCONV)
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
							if (m_tePlacePosX == PosX.UNLOADCONV)
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
	}
}
