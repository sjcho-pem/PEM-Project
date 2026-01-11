using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace H_MarsSSD
{
	public class SeqTrayClean : SeqBase<SeqTrayClean.Act, SeqTrayClean.Axis, SeqTrayClean.Cmd>
	{
		#region [ Enum ]
		public enum Act
		{
			TrayGrip,
			Rotate,
            TableUpDn,
			AirMove,
			Dust,
			Max,
		}

		public enum Axis
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

			TRAYCLEAN_10,
			TRAYCLEAN_20,
			TRAYCLEAN_30,
			TRAYCLEAN_40,
			TRAYCLEAN_50,
			TRAYCLEAN_60,
			TRAYCLEAN_70,
			TRAYCLEAN_80,
			TRAYCLEAN_90,
			TRAYCLEAN_100,
			TRAYCLEAN_110,
			TRAYCLEAN_120,
			TRAYCLEAN_130,
			TRAYCLEAN_140,
			TRAYCLEAN_150,

			GRIP_10,
			GRIP_20,
			RELEASE_10,
			RELEASE_20,
		}

		#endregion

		#region [Variables]
		private SMM<Cmd> m_smmHome		= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.TRAYCLEAN_HOME));
		private SMM<Cmd> m_smmTrayClean	= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.TRAYCLEAN_CLEAN));
		private SMM<Cmd> m_smmGrip		= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.TRAYCLEAN_GRIP));
		private SMM<Cmd> m_smmRelease		= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.TRAYCLEAN_RELEASE));

		public	Cmd	smmHome			{ get => m_smmHome.CurStep; }
		public	Cmd	smmTrayClean	{ get => m_smmTrayClean.CurStep; }
		public	Cmd	smmGrip			{ get => m_smmGrip.CurStep; }
		public	Cmd	smmRelease		{ get => m_smmRelease.CurStep; }

		private int		m_nCleanRunCnt		= 0;
		private int		m_nCleanOptCnt		= 0;
		private	bool	m_bUnloadResurve	= false;
		public	bool	UnloadResurve	{ get => m_bUnloadResurve; }
		#endregion


		public	void	Init()
		{
			try
			{
				lstSMM.Add(m_smmHome);
				lstSMM.Add(m_smmTrayClean);
				lstSMM.Add(m_smmGrip);
				lstSMM.Add(m_smmRelease);


				#region - Motor
				arrMt = new EnumArray<Axis, CMotor>(Axis.Max);
				
				#endregion

				#region - Actuator
				arrAct = new EnumArray<Act, Actuator>(Act.Max);

				arrAct[Act.TrayGrip] = new CDoubleSen();
				arrAct[Act.TrayGrip].SetSenser1(new List<DI>(new DI[] { }));
				arrAct[Act.TrayGrip].SetSenser2(new List<DI>(new DI[] { DI.TrayCleanGripOpenCyl }));
				arrAct[Act.TrayGrip].SetSol(DO.TrayCleanGripCloseCyl, DO.TrayCleanGripOpenCyl);
				arrAct[Act.TrayGrip].Setting(ALM.ERROR_TRAY_CLEAN_GRIP_CYL_GRIP, ALM.ERROR_TRAY_CLEAN_GRIP_CYL_RELEASE, ErrSenPos.Ori, SenPos.NONE, 300);

				//전진 - 180도
				arrAct[Act.Rotate] = new CDoubleSen();
				arrAct[Act.Rotate].SetSenser1(new List<DI>(new DI[] { DI.TrayCleanRotate180Cyl }));
				arrAct[Act.Rotate].SetSenser2(new List<DI>(new DI[] { DI.TrayCleanRotate0Cyl }));
				arrAct[Act.Rotate].SetSol(DO.TrayCleanRotate180Cyl, DO.TrayCleanRotate0Cyl);
				arrAct[Act.Rotate].Setting(ALM.ERROR_TRAY_CLEAN_ROTATE_CYL_FORWARD, ALM.ERROR_TRAY_CLEAN_ROTATE_CYL_BACKWARD, ErrSenPos.Ori, SenPos.NONE, 200, 10000);

				arrAct[Act.TableUpDn] = new CDoubleSen();
				arrAct[Act.TableUpDn].SetSenser1(new List<DI>(new DI[] { DI.TrayCleanUpCyl }));
				arrAct[Act.TableUpDn].SetSenser2(new List<DI>(new DI[] { DI.TrayCleanDnCyl }));
				arrAct[Act.TableUpDn].SetSol(DO.TrayCleanUpCyl, DO.TrayCleanDnCyl);
				arrAct[Act.TableUpDn].Setting(ALM.ERROR_TRAY_CLEAN_UP_CYL_UP, ALM.ERROR_TRAY_CLEAN_UP_CYL_DOWN, ErrSenPos.Ori, SenPos.NONE, 200, 10000);
				
				arrAct[Act.AirMove] = new CSingleSen();
				arrAct[Act.AirMove].SetSenser1(new List<DI>(new DI[] { DI.TrayCleanAirLeftCyl }));
				arrAct[Act.AirMove].SetSenser2(new List<DI>(new DI[] { DI.TrayCleanAirRightCyl }));
				arrAct[Act.AirMove].SetSol(DO.TrayCleanAirCyl);
				arrAct[Act.AirMove].Setting(ALM.ERROR_TRAY_CLEAN_AIR_CYL_FORWARD, ALM.ERROR_TRAY_CLEAN_AIR_CYL_BACKWARD, ErrSenPos.Ori, SenPos.NONE, 200, 5000);

				arrAct[Act.Dust] = new CSingleSen();
				arrAct[Act.Dust].SetSenser1(new List<DI>(new DI[] { DI.TrayCleanDustOpenCyl }));
				arrAct[Act.Dust].SetSenser2(new List<DI>(new DI[] { DI.TrayCleanDustCloseCyl }));
				arrAct[Act.Dust].SetSol(DO.TrayCleanDustCyl);
				arrAct[Act.Dust].Setting(ALM.ERROR_TRAY_CLEAN_DUST_CYL_FORWARD, ALM.ERROR_TRAY_CLEAN_DUST_CYL_BACKWARD, ErrSenPos.Ori);

				/*
				sensor List  
				TrayCleanExistSen			

				IO List
				TrayCleanIonizerBlow Blow  On/Off
				 */

				if (NDm.Read(eNDM.TrayCleanGripDelay) <= 0)			NDm.Write(eNDM.TrayCleanGripDelay, 500);

				#endregion

				base.Initialize();
			}
			catch (Exception ex)
			{

				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}


		#region [Function]

		public bool HomeStart() { return SetSeqRun(ref m_smmHome); }

		public bool TrayCleanStart() { return SetSeqRun(ref m_smmTrayClean); }

		public bool GripStart() { return SetSeqRun(ref m_smmGrip); }

		public bool ReleaseStart() { return SetSeqRun(ref m_smmRelease); }

		public bool IsGrip()
		{
			try
			{
				if (arrAct[Act.TrayGrip].IsPos(SenPos.FWD) == false) return SM.SHM.InterlockMsgAdd("arrAct[teAct.Grip].IsPos(SenPos.FWD) == false");

				return SM.SHM.InterlockMsgReset();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public bool IsRelease()
		{
			try
			{
				if (arrAct[Act.TrayGrip].IsPos(SenPos.BWD) == false) return SM.SHM.InterlockMsgAdd("arrAct[teAct.Grip].IsPos(SenPos.BWD) == false");

				return SM.SHM.InterlockMsgReset();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public bool IsHomeReady() { return m_smmHome.IsReady; }

		public bool IsTrayCleanReady() { return m_smmTrayClean.IsReady; }

		public bool IsGripReady() { return m_smmGrip.IsReady; }

		public bool IsReleaseReady() { return m_smmRelease.IsReady; }

		public bool IsLoadEnable()
		{
			try
			{
				if (this.IsReady == false)								return SM.SHM.InterlockMsgAdd("this.IsReady == false");
				if (G.DryRunIsBIn(DI.TrayCleanExistSen, false) == true)		return SM.SHM.InterlockMsgAdd("G.cKernel.IsAIn(DI.TrayCleanExistSen) == true");
				if (SM.FMT.IsFlow(FlowTray.TrayClean) == true)		return SM.SHM.InterlockMsgAdd("SM.FMT.IsFlow(teFlowTray.TrayClean) == true");
				if (arrAct[Act.TrayGrip].GetCmd() != SenPos.BWD)		return SM.SHM.InterlockMsgAdd("arrAct[teAct.Grip].GetCmd() != SenPos.BWD");
				if (arrAct[Act.Rotate].IsPos(SenPos.BWD) == false)	return SM.SHM.InterlockMsgAdd("arrAct[teAct.Rotate].IsPos(SenPos.BWD) == false");
				if (arrAct[Act.TableUpDn].IsPos(SenPos.UP) == false)	return SM.SHM.InterlockMsgAdd("arrAct[teAct.UpDn].IsPos(SenPos.UP) == false");

				return SM.SHM.InterlockMsgReset();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public bool IsUnloadEnable()
		{
			try
			{
				if (this.IsReady == false)									return SM.SHM.InterlockMsgAdd("this.IsReady == false");
				if (G.DryRunIsBIn(DI.TrayCleanExistSen, true) == false)		return SM.SHM.InterlockMsgAdd("G.DryRunIsBIn(DI.TrayCleanExistSen, true) == false");
				if (SM.FMT.IsFlow(FlowTray.TrayClean) == false)			return SM.SHM.InterlockMsgAdd("SM.FMT.IsFlow(teFlowTray.TrayClean) == false");
				if (arrAct[Act.TrayGrip].GetCmd() != SenPos.FWD)			return SM.SHM.InterlockMsgAdd("arrAct[teAct.Grip].GetCmd() != SenPos.FWD");
				if (arrAct[Act.Rotate].IsPos(SenPos.BWD) == false)		return SM.SHM.InterlockMsgAdd("arrAct[teAct.Rotate].IsPos(SenPos.BWD) == false");
				if (arrAct[Act.TableUpDn].IsPos(SenPos.UP) == false)		return SM.SHM.InterlockMsgAdd("arrAct[teAct.UpDn].IsPos(SenPos.UP) == false");

				return SM.SHM.InterlockMsgReset();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public bool IsSafety(int nTrayNum = -1)
		{
			try
			{
				//로드트레이1은 트레이클린상태체크 필요 x
				if (nTrayNum == 0) return SM.SHM.InterlockMsgReset();

				//Up 이고 클린 진행중일때
				if (IsCylStats(SenPos.DOWN) == false && m_smmTrayClean.IsReady == false) return SM.SHM.InterlockMsgAdd("IsCylStats(SenPos.DOWN) == false  &&  m_smmTrayClean.IsReady == false");

				return SM.SHM.InterlockMsgReset();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public bool IsCylStats(SenPos eSen)
		{
			try
			{
				if (arrAct[Act.TableUpDn].IsRdy() == false) return SM.SHM.InterlockMsgAdd("arrAct[teAct.UpDn].IsRdy() == false");

				SM.SHM.InterlockMsgReset();
				return arrAct[Act.TableUpDn].IsPos(eSen);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		private bool SetSeqRun(ref SMM<Cmd> smm)
		{
			try
			{
				if (m_smmHome.IsReady == false) return false;

				smm.eInput = InState.run;
				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		private void DustValueRun()
		{
			try
			{
				if (IsTrayCleanReady() == true && G.seqBtn.IsResetReady() == true)
				{
					if (G.cKernel.IsAOut(DO.LDBuf1DustCyl) == false &&
						G.cKernel.IsAOut(DO.LDBuf2DustCyl) == false)
					{
						if (arrAct[Act.Dust].GetPos() == SenPos.BWD) arrAct[Act.Dust].SetPos(SenPos.FWD);
					}
					else
					{
						if (arrAct[Act.Dust].GetPos() == SenPos.FWD) arrAct[Act.Dust].SetPos(SenPos.BWD);
					}
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
#endregion



		public void Run()
		{
			try
			{
				UpdateAct(Seq.TrayClean);
				DustValueRun();

				if (SM.SHM.IsBitData(BIT.Error) == true || SM.SHM.IsBitData(BIT.Pause) == true) return;

				Home(m_smmHome);
				TrayClean(m_smmTrayClean);
				Grip(m_smmGrip);
				Release(m_smmRelease);

				CheckUnloadReserve();

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
							if (SM.FMT.IsFlow(FlowTray.TrayClean) != G.DryRunIsBIn(DI.TrayCleanExistSen, SM.FMT.IsFlow(FlowTray.TrayClean)))
							{
								AddAlarm(ALM.ERROR_SEQ_TRAY_CLEAN_TRAY_MISMATCH);
								break;
							}

							//자재 존재시, 그립해둔 상태로 초기화 진행
							if (G.DryRunIsBIn(DI.TrayCleanExistSen, SM.FMT.IsFlow(FlowTray.TrayClean)) == true)
							{
								if (arrAct[Act.TrayGrip].IsPos(SenPos.FWD) == false)
								{
									arrAct[Act.TrayGrip].SetPos(SenPos.FWD);
									break;
								}
							}
							else
							{
								if (arrAct[Act.TrayGrip].IsPos(SenPos.BWD) == false)
								{
									arrAct[Act.TrayGrip].SetPos(SenPos.BWD);
									break;
								}
							}

							smm.Next(Cmd.HOME_20);
						}
						break;

					case Cmd.HOME_20:
						{
							if (G.cKernel.IsAOut(DO.TrayCleanAirBlow) == true)
							{
								G.cKernel.AOut(DO.TrayCleanAirBlow, false);
								break;
							}

							smm.Next(Cmd.HOME_30);
						}
						break;

					case Cmd.HOME_30:
						{
							// 0도 상태면 업 후, 종료
							if (arrAct[Act.Rotate].IsPos(SenPos.BWD) == true)
							{
								if (arrAct[Act.TableUpDn].IsPos(SenPos.UP) == false)
								{
									arrAct[Act.TableUpDn].SetPos(SenPos.UP);
									break;
								}
								smm.Next(Cmd.END);
							}
							else
							{
								smm.Next(Cmd.HOME_40);
							}
						}
						break;

					case Cmd.HOME_40:
						{
							if (arrAct[Act.TableUpDn].IsPos(SenPos.UP) == false)
							{
								arrAct[Act.TableUpDn].SetPos(SenPos.UP);
								break;
							}

							smm.Next(Cmd.HOME_50);
						}
						break;

					case Cmd.HOME_50:
						{
							if (arrAct[Act.Rotate].IsPos(SenPos.BWD) == false)
							{
								arrAct[Act.Rotate].SetPos(SenPos.BWD);
								break;
							}

							smm.Next(Cmd.HOME_60);
						}
						break;

					case Cmd.HOME_60:
						{
							if (arrAct[Act.TrayGrip].IsPos(SenPos.BWD) == false)
							{
								arrAct[Act.TrayGrip].SetPos(SenPos.BWD);
								break;
							}

							smm.Next(Cmd.HOME_70);
						}
						break;

					case Cmd.HOME_70:
						{
							if (arrAct[Act.AirMove].IsPos(SenPos.BWD) == false)
							{
								arrAct[Act.AirMove].SetPos(SenPos.BWD);
								break;
							}
							smm.Next(Cmd.HOME_80);
						}
						break;

					case Cmd.HOME_80:
						{
							if (SM.FMT.IsFlow(FlowTray.TrayClean) == true)
							{
								if (arrAct[Act.TrayGrip].IsPos(SenPos.FWD) == false)
								{
									arrAct[Act.TrayGrip].SetPos(SenPos.FWD);
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

		private void TrayClean(SMM<Cmd> smm)
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

								smm.Next(Cmd.TRAYCLEAN_10);
							}
						}
						break;

					case Cmd.TRAYCLEAN_10:
						{
							if (G.DryRunIsBIn(DI.TrayCleanExistSen, true) == false ||
								SM.FMT.IsFlow(FlowTray.TrayClean) == false)
							{
								AddAlarm(ALM.ERROR_SEQ_TRAY_CLEAN_TRAY_NONE);
								smm.Next(Cmd.END);
								break;
							}

							smm.Next(Cmd.TRAYCLEAN_20);
						}
						break;

					case Cmd.TRAYCLEAN_20:
						{
							if (G.seqLoadTransfer.TrayCleanUpSafety() == false) break;
							if (G.seqUnloadTransfer.TrayCleanUpSafety() == false) break;

							smm.Next(Cmd.TRAYCLEAN_30);
						}
						break;

					case Cmd.TRAYCLEAN_30:
						{
							if (arrAct[Act.TableUpDn].IsPos(SenPos.UP) == false)
							{
								arrAct[Act.TableUpDn].SetPos(SenPos.UP);
								break;
							}

							smm.Next(Cmd.TRAYCLEAN_40);
						}
						break;

					case Cmd.TRAYCLEAN_40:
						{
							//180도 회전
							if (arrAct[Act.Rotate].IsPos(SenPos.FWD) == false)
							{
								arrAct[Act.Rotate].SetPos(SenPos.FWD);
								break;
							}
							smm.Next(Cmd.TRAYCLEAN_50);
						}
						break;

					case Cmd.TRAYCLEAN_50:
						{
							if (smm.Delayms(500) == false) break;

							if (arrAct[Act.TableUpDn].IsPos(SenPos.DOWN) == false)
							{
								arrAct[Act.TableUpDn].SetPos(SenPos.DOWN);
								break;
							}
							m_nCleanOptCnt = NDm.Read(eNDM.TrayCleanCount);
							m_nCleanRunCnt = 0;

							smm.Next(Cmd.TRAYCLEAN_60);
						}
						break;

					case Cmd.TRAYCLEAN_60:
						{
							if (m_nCleanOptCnt <= 0)
							{
								smm.Next(Cmd.TRAYCLEAN_90);
								break;
							}
							if (arrAct[Act.Dust].IsPos(SenPos.FWD) == false)
							{
								arrAct[Act.Dust].SetPos(SenPos.FWD);
								break;
							}

							G.cKernel.AOut(DO.TrayCleanAirBlow, true);

							smm.Next(Cmd.TRAYCLEAN_70);
						}
						break;

					case Cmd.TRAYCLEAN_70:// 옵션값 반복 동작
						{
							if (arrAct[Act.AirMove].IsPos(SenPos.FWD) == false)
							{
								arrAct[Act.AirMove].SetPos(SenPos.FWD);
								break;
							}

							smm.Next(Cmd.TRAYCLEAN_80);
						}
						break;

					case Cmd.TRAYCLEAN_80:
						{
							if (arrAct[Act.AirMove].IsPos(SenPos.BWD) == false)
							{
								arrAct[Act.AirMove].SetPos(SenPos.BWD);
								break;
							}

							m_nCleanRunCnt++;

							if (m_nCleanOptCnt > m_nCleanRunCnt)
							{
								smm.Next(Cmd.TRAYCLEAN_70);
								break;
							}

							smm.Next(Cmd.TRAYCLEAN_90);
						}
						break;
					case Cmd.TRAYCLEAN_90:
						{
							G.cKernel.AOut(DO.TrayCleanAirBlow, false);

							if (arrAct[Act.Dust].IsPos(SenPos.BWD) == false)
							{
								arrAct[Act.Dust].SetPos(SenPos.BWD);
								break;
							}

							smm.Next(Cmd.TRAYCLEAN_100);
						}
						break;

					case Cmd.TRAYCLEAN_100:
						{
							if (arrAct[Act.TableUpDn].IsPos(SenPos.UP) == false)
							{
								arrAct[Act.TableUpDn].SetPos(SenPos.UP);
								break;
							}

							smm.Next(Cmd.TRAYCLEAN_110);
						}
						break;

					case Cmd.TRAYCLEAN_110:
						{

							if (G.seqLoadTransfer.TrayCleanUpSafety() == false) break;
							if (G.seqUnloadTransfer.TrayCleanUpSafety() == false) break;

							if (arrAct[Act.Rotate].IsPos(SenPos.BWD) == false)
							{
								arrAct[Act.Rotate].SetPos(SenPos.BWD);
								break;
							}

							smm.Next(Cmd.TRAYCLEAN_120);
						}
						break;

					case Cmd.TRAYCLEAN_120:
						{
							if (G.DryRunIsBIn(DI.TrayCleanExistSen, true) == false)
							{
								AddAlarm(ALM.ERROR_SEQ_TRAY_CLEAN_TRAY_NONE);
								break;
							}

							SM.FMT[FlowTray.TrayClean].ProcStatus = ProcStatus.CleanDone;

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

		private void Release(SMM<Cmd> smm)
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

								smm.Next(Cmd.RELEASE_10);
							}
						}
						break;

					case Cmd.RELEASE_10:
						{
							//플로우매니저 체크
							if (SM.FMT.IsFlow(FlowTray.TrayClean) == true)
							{
								AddAlarm(ALM.ERROR_SEQ_TRAY_CLEAN_TRAY_EXIST);
								break;
							}

							smm.Next(Cmd.RELEASE_20);
						}
						break;

					case Cmd.RELEASE_20:
						{
							if (arrAct[Act.TrayGrip].IsPos(SenPos.BWD) == false)
							{
								arrAct[Act.TrayGrip].SetPos(SenPos.BWD);
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

		private void Grip(SMM<Cmd> smm)
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

								smm.Next(Cmd.GRIP_10);
							}
						}
						break;

					case Cmd.GRIP_10:
						{
							if (G.DryRunIsBIn(DI.TrayCleanExistSen, true) == false)
							{
								AddAlarm(ALM.ERROR_SEQ_TRAY_CLEAN_TRAY_NONE);
								break;
							}

							smm.Next(Cmd.GRIP_20);
						}
						break;

					case Cmd.GRIP_20:
						{
							int nDelay = NDm.Read(eNDM.TrayCleanGripDelay);

							if (arrAct[Act.TrayGrip].IsPos(SenPos.FWD) == false)
							{
								arrAct[Act.TrayGrip].SetPos(SenPos.FWD);
								break;
							}

							if (smm.Delayms(nDelay) == false) break;

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

		private void CheckUnloadReserve()
		{
			try
			{
				if (SM.FMT.IsFlow(FlowTray.ULDTable1) == false || SM.FMT.IsFlow(FlowTray.ULDTable2) == false)
				{
					if (SM.FMT.IsFlow(FlowTray.TrayClean) == true)
					{
						if (SM.FMT[FlowTray.TrayClean].ProcStatus == ProcStatus.CleanDone) m_bUnloadResurve = true;
						if (m_smmTrayClean.CurStep == Cmd.TRAYCLEAN_90 && m_nCleanRunCnt >= (m_nCleanOptCnt - 1)) m_bUnloadResurve = true;
					}
					else
					{
						m_bUnloadResurve = false;
					}
				}
				else
				{
					m_bUnloadResurve = false;
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
	}
}
