using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace H_MarsSSD
{
	public class SeqRejectBuffer : SeqBase<SeqRejectBuffer.Act, SeqRejectBuffer.Axis, SeqRejectBuffer.Cmd>
	{
		#region [ Enum ]
		public enum Act
		{
			TrayGripX,
			TrayGripY,
            AirVib,
			Max,
		}

		public enum Axis
		{
			Y,
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

			READY_10,
			READY_20,
			READY_30,
			READY_40,
			READY_50,
			READY_ALARM_DIR,
			READY_ALARM_SIZE,

			TRAYCHANGE_10,
			TRAYCHANGE_MESCHECK1,
			TRAYCHANGE_MESCHECK2,
			TRAYCHANGE_MESCHECK3,
			TRAYCHANGE_MESCHECK4,
			TRAYCHANGE_20,
			TRAYCHANGE_30,
			TRAYCHANGE_40,
			TRAYCHANGE_50,
			TRAYCHANGE_BARCODECHECK1,
			TRAYCHANGE_BARCODECHECK2,
			TRAYCHANGE_BARCODECHECK3,
			TRAYCHANGE_BARCODECHECK4,
			TRAYCHANGE_60,
			TRAYCHANGE_70,
			TRAYCHANGE_80,
			TRAYCHANGE_NG,

			ALIGN_10,
			ALIGN_20
		}

		public enum PosY
		{
			READY,		//PnP위치
			TRAYCHANGE, //트레이 교체 위치
			BARCODEREAD,//바코드 읽는 위치
		}
		#endregion

		#region [Variables]
		private	SMM<Cmd>		m_smmHome			= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.REJECTBUFFER_HOME));
		private	SMM<Cmd>		m_smmReady			= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.REJECTBUFFER_READY));
		private	SMM<Cmd>		m_smmTrayChange		= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.REJECTBUFFER_CHANGE));
		private SMM<Cmd>		m_smmAlign			= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.REJECTBUFFER_VIBRATE));

		public	Cmd		smmHome				{ get => m_smmHome.CurStep; }
		public	Cmd		smmReady			{ get => m_smmReady.CurStep; }
		public	Cmd		smmTrayChange		{ get => m_smmTrayChange.CurStep; }
		public Cmd		smmAlign			{ get => m_smmAlign.CurStep; }
		private	bool	m_IsMtHome			= true;
		private	int		m_SgemRetryCount	= 0;
        private bool m_bAirVibRequest = false;
		private FlowTray FlowTable = FlowTray.Reject;
		RunStateType	BeforeState = RunStateType.NormalRun;
		#endregion



		public void Init()
		{
			try
			{
				lstSMM.Add(m_smmHome);
				lstSMM.Add(m_smmReady);
				lstSMM.Add(m_smmTrayChange);
				lstSMM.Add(m_smmAlign);

				#region - Motor
				arrMt = new EnumArray<Axis, CMotor>(Axis.Max);
				arrMt[Axis.Y] = G.cKernel.Motor(MT.RejectBufY);
				#endregion

				#region - Actuator
				//센서를 하나만 사용하는 경우, 장비 전원이 내려간 경우 현재 상태를 잘못 알고 있을 수 있음.
				//장비 전원을 내렸다 올리면 무조건 동작해서 초기화 한번 시켜줘야함.
				arrAct = new EnumArray<Act, Actuator>(Act.Max);
				arrAct[Act.TrayGripX] = new CSingleSen();
				arrAct[Act.TrayGripX].SetSenser1(new List<DI>(new DI[] { }));
				arrAct[Act.TrayGripX].SetSenser2(new List<DI>(new DI[] { DI.RejectTrayGripXOpenCyl }));
				arrAct[Act.TrayGripX].SetSol(DO.RejectTrayGripXCyl);
				arrAct[Act.TrayGripX].Setting(ALM.NOT_ALARM, ALM.ERROR_REJECT_TRAY_TABLE_GRIP_X_CYL_OPEN, ErrSenPos.Ori, SenPos.NONE, 1000, 5000);
				

				arrAct[Act.TrayGripY] = new CSingleSen();
				arrAct[Act.TrayGripY].SetSenser1(new List<DI>(new DI[] { }));
				arrAct[Act.TrayGripY].SetSenser2(new List<DI>(new DI[] { DI.RejectTrayGripYOpenCyl }));
				arrAct[Act.TrayGripY].SetSol(DO.RejectTrayGripYCyl);
				arrAct[Act.TrayGripY].Setting(ALM.NOT_ALARM, ALM.ERROR_REJECT_TRAY_TABLE_GRIP_Y_CYL_OPEN, ErrSenPos.Ori, SenPos.NONE, 1000, 5000);

                arrAct[Act.AirVib] = new CSingleNoSen();
                arrAct[Act.AirVib].SetSol(DO.RejectTrayAirVib);
                arrAct[Act.AirVib].Setting(ALM.NOT_ALARM, ALM.NOT_ALARM);


                #endregion

                base.Initialize();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public bool HomeStart(bool IsMtHome = true) { m_IsMtHome = IsMtHome; return SetSeqRun(ref m_smmHome); }
		public bool ReadyStart() { return SetSeqRun(ref m_smmReady); }
		public bool TrayChangeStart() { return SetSeqRun(ref m_smmTrayChange); }
		public bool AlignStart() { return SetSeqRun(ref m_smmAlign); }

		public bool IsHomeReady() { return m_smmHome.IsReady; }
		public bool IsReadyReady() { return m_smmReady.IsReady; }
		public bool IsTrayChangeReady() { return m_smmTrayChange.IsReady; }
		public bool IsAlignReady() { return m_smmAlign.IsReady; }
		public bool IsPnPReady()
		{
			if (IsReady == false)																			return false;
			if (CheckSen() == false)																		return false;
			if (IsReadyPos() == false)																		return false;
			if (SM.FMT.IsFlow(FlowTray.Reject) == false)													return false;
			if (SM.FMT[FlowTray.Reject].ProcStatus == ProcStatus.SortDone)									return false;
			if (SM.FMT[FlowTray.Reject].GetInfo().ModuleType != SM.SHM.GetRecipeData(DEVICE.ModuleType))	return false;

			return true;
		}


		public bool IsTrayChangePossible()
		{
			if (SM.FMT[FlowTray.Reject].GetVerifyCount(ProdStatus.OK) > 0) return false;
			if (SM.FMT[FlowTray.Reject].CheckVerifyComplete() == false) return false;
			if (CheckNeedSort() == true) return false;

			return true;
		}

		public bool IsReadyPos()
		{
			if (arrAct[Act.TrayGripX].IsPos(SenPos.FWD) == false) return false;
			if (arrAct[Act.TrayGripY].IsPos(SenPos.FWD) == false) return false;
			if (arrMt[Axis.Y].IsRdyPos(arrMt[Axis.Y].GetIndexPos((int)PosY.READY)) == false) return false;

			return true;
		}

		public bool CheckSen(bool bAlarm = false)
		{
			try
			{
				if (SM.SHM.GetRecipeData(DEVICE.ModuleType) == EnumHelper.GetDescription(ModuleType.LPCAMM))
				{
					if (G.cKernel.IsAIn(DI.RejectTrayExistSen) == false)
					{
						if (bAlarm == true) AddAlarm(ALM.ERROR_SEQ_REJECT_BUF_TABLE_NOT_LARGE_SIZE);

						return false;
					}
					if (G.cKernel.IsAIn(DI.RejectTraySizeSen) == false)
					{
						if (bAlarm == true) AddAlarm(ALM.ERROR_SEQ_REJECT_BUF_TABLE_NOT_LARGE_SIZE);

						return false;
					}
				}
				else 
				{
					if (G.cKernel.IsAIn(DI.RejectTrayExistSen) == false)
					{
						if (bAlarm == true) AddAlarm(ALM.ERROR_SEQ_REJECT_BUF_TABLE_NOT_SMALL_SIZE);

						return false;
					}
					if (G.cKernel.IsAIn(DI.RejectTraySizeSen) == true)
					{
						if (bAlarm == true) AddAlarm(ALM.ERROR_SEQ_REJECT_BUF_TABLE_NOT_SMALL_SIZE);

						return false;
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


		private void CheckNGCount()
		{
			try
			{
				
				int ncnt_row = SM.SHM.GetRecipeDataToInt(SM.FMT[FlowTray.Reject].GetInfo().ModuleType, DEVICE.TrayRowCnt) / 1000;
				int ncnt_col = SM.SHM.GetRecipeDataToInt(SM.FMT[FlowTray.Reject].GetInfo().ModuleType, DEVICE.TrayColCnt) / 1000;
				int ncnt_max = ncnt_row * ncnt_col;

				if (SM.FMT[FlowTray.Reject].GetVerifyCount(ProdStatus.NG) > ncnt_max)
				{
					if (SM.SHM.IsBitData(BIT.AutoRun) == true)
					{
						AddAlarm(ALM.ERROR_SEQ_REJECT_BUF_NG_COUNT_OVER);
					}
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void CheckTrayState()
		{
			if (SM.FMT.IsFlow(FlowTray.Reject) == false) return;
			if (G.seqUnloadPnP.IsReady == false) return;
			
			//if (SM.FMT[FlowTray.Reject].GetInfo().ModuleType != SM.SHM.GetRecipeData(DEVICE.ModuleType))
			//	return;
			

			//해당 구문 없을 시 리젝부터 소팅하여 택에 영향
			if (SM.FMT[FlowTray.Reject].ProcStatus != ProcStatus.Unknown
				&& CheckNeedSort() == false && SM.FMP.IsFlow(FlowPart.ULDPnP) == false)
			{
				SM.FMT[FlowTray.Reject].ProcStatus = ProcStatus.Unknown;
			}

			

			if ((SM.FMT[FlowTray.Reject].GetVerifyyNGCount() >= SM.SHM.GetRecipeDataToInt(DEVICE.RejectTrayChangeCount) / 1000 ||
				 SM.FMT[FlowTray.Reject].GetVerifyyNGCount() >= SM.FMT[FlowTray.Reject].GetInfo().MaxCntProdInTray ||
				 (SM.FMT[FlowTray.Reject].GetInfo().WorkOrder != "" &&
				  CheckWOExist(FlowPart.LDPnP, FlowPart.ULDPnP, SM.FMT[FlowTray.Reject].GetInfo().WorkOrder) == false) &&
				  CheckWOExist(FlowTray.LDLift01, FlowTray.LDTable2, SM.FMT[FlowTray.Reject].GetInfo().WorkOrder) == false) &&
				 SM.FMT[FlowTray.Reject].GetVerifyNotCount() == 0 &&
				 SM.FMT[FlowTray.Reject].GetVerifyCount(ProdStatus.OK) == 0)
			{
				if (SM.FMT[FlowTray.Reject].GetExistCount() == 0)
				{
					var tcinfo = SM.FMT[FlowTray.Reject].GetInfo();
					tcinfo.WorkOrder = "";
					SM.FMT[FlowTray.Reject].SetInfo(tcinfo);
				}

				if (CheckNeedSort() == true) SM.FMT[FlowTray.Reject].ProcStatus = ProcStatus.SortRun;
				else SM.FMT[FlowTray.Reject].ProcStatus = ProcStatus.SortDone;
			}

			if (SM.FMT.IsFlow(FlowTray.ULDTable1) == true || SM.FMT.IsFlow(FlowTray.ULDTable2)) return;

			if (SM.FMT[FlowTray.Reject].GetVerifyNotCount() == 0 &&
				SM.FMT[FlowTray.Reject].GetVerifyCount(ProdStatus.OK) == 0)
			{
				if (CheckNeedSort() == true)
				{
					SM.FMT[FlowTray.Reject].ProcStatus = ProcStatus.SortRun;
				}
				else if (SM.FMT[FlowTray.Reject].GetVerifyyNGCount() >= SM.SHM.GetRecipeDataToInt(DEVICE.RejectTrayChangeCount)/ 1000 ||
						 SM.FMT[FlowTray.Reject].GetVerifyyNGCount() >= SM.FMT[FlowTray.Reject].GetInfo().MaxCntProdInTray)
				{
					SM.FMT[FlowTray.Reject].ProcStatus = ProcStatus.SortDone;
				}
				else if (CheckNeedSort() == false && SM.FMP.IsFlow(FlowPart.ULDPnP) == false)
				{
					SM.FMT[FlowTray.Reject].ProcStatus = ProcStatus.Unknown;
				}
			}
		}

		//동일 work order가 있는지 체크
		private bool CheckWOExist(FlowPart startPart, FlowPart EndPart, string strWorkOrder)
		{
			try
			{

				for (FlowPart i = startPart; i <= EndPart; i++)
				{
					if (SM.FMP.IsFlow(i) == false) continue;

					if (SM.FMP[i].GetInfo().WorkOrder == strWorkOrder)
					{
						return true;
					}
				}

				return false;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		private bool CheckWOExist(FlowTray startTray, FlowTray EndTray, string strWorkOrder)
		{
			try
			{

				for (FlowTray i = startTray; i <= EndTray; i++)
				{
					if (SM.FMT.IsFlow(i) == false) continue;

					if (SM.FMT[i].GetInfo().WorkOrder == strWorkOrder)
					{
						return true;
					}
				}

				return false;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public bool CheckNeedSort()
		{
			try
			{
				if (SM.FMT.IsFlow(FlowTray.Reject) == false) return false;
				if (SM.FMT[FlowTray.Reject].GetInfo().ModuleType != SM.SHM.GetRecipeData(DEVICE.ModuleType)) return false;
				

				List<int> lt_sort_place = new List<int>();
				List<int> lt_sort_pick = new List<int>();
				int[] nsort_map = new int[SM.FMT[FlowTray.Reject].GetInfo().MaxCntProdInTray];
				int[] nmask_end = new int[] { 1, -1 };
				int nsort_map_end = -1;
				int nstrart_place = 0;

				// Maaping
				for (int i = 0; i < nsort_map.Length; i++)
				{ 
					int nindex = SM.SHM.GetOriginToTransIdx(i, SM.FMT[FlowTray.Reject].GetInfo().ModuleType);
					if (SM.FMT[FlowTray.Reject].GetInfo().ProdInfo[nindex].Exist == false)	nsort_map[i] = 0;
					else																	nsort_map[i] = 1;
				}

				// 마지막 제품 위치 검색
				for (int i = nsort_map.Length - 1; i >= 0; i--)
				{
					if (nsort_map[i] == 1)
					{
						nsort_map_end = i;
						break;
					}
				}

				if (nsort_map_end < 0) return false;

				//Place 할 위치와 Pick 할 위치 생성
				for (int i = nstrart_place; i <= nsort_map_end; i++)
				{
					if (nsort_map[i] == 0)
					{
						nstrart_place = i + 1;

						for (int j = nsort_map_end; j >= nstrart_place; j--)
						{
							if (nsort_map[j] == 1)
							{
								int place = SM.SHM.GetOriginToTransIdx(i ,SM.FMT[FlowTray.Reject].GetInfo().ModuleType );
								int pick =  SM.SHM.GetOriginToTransIdx(j, SM.FMT[FlowTray.Reject].GetInfo().ModuleType);;
								lt_sort_place.Add(place);
								lt_sort_pick.Add(pick);
								nsort_map_end = j - 1;
								break;
							}
						}
					}
				}

				if (lt_sort_place.Count == 0 && lt_sort_pick.Count == 0) return false;

				return true;
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
					if (G.RejectLPCAMMBarcode.IsConnect() == false)
					{
						AddAlarm(ALM.ERROR_SEQ_REJECT_BUFFER2_BARCODE_READ_FAIL);
						return;
					}
					G.RejectLPCAMMBarcode.TriggerOn();
				}
				else
				{
					if (G.RejectOESBarcode.IsConnect() == false)
					{
						AddAlarm(ALM.ERROR_SEQ_REJECT_BUFFER1_BARCODE_READ_FAIL);
						return;
					}
					G.RejectOESBarcode.TriggerOn();
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
					G.RejectLPCAMMBarcode.TriggerOff();
				}
				else
				{
					G.RejectOESBarcode.TriggerOff();
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
				if (SM.SHM.GetUseSkipData(USESKIP.BarcodeRead) == false)	return "";
				if (SM.SHM.GetUseSkipData(USESKIP.DRYRUN) == true)			return "";

				if (SM.SHM.GetRecipeData(DEVICE.ModuleType) == EnumHelper.GetDescription(ModuleType.LPCAMM))
				{
					return G.RejectLPCAMMBarcode.GetResultData();
				}
				else
				{
					return G.RejectOESBarcode.GetResultData();
				}
				
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return "";
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



		public void Run()
		{
			try
			{
				UpdateAct(Seq.RejectBuffer);
				TrayChange(m_smmTrayChange);

				if (SM.SHM.IsBitData(BIT.Error) == true || SM.SHM.IsBitData(BIT.Pause) == true) return;

				CheckNGCount();
				CheckTrayState();

				Home(m_smmHome);
				Ready(m_smmReady);
				Align(m_smmAlign);

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
							if (m_IsMtHome == false)
							{
								smm.Next(Cmd.HOME_30);
								break;
							}
							arrMt[Axis.Y].ClearHome();
							arrMt[Axis.Y].StartHoming();

							smm.Next(Cmd.HOME_20);
						}
						break;

					case Cmd.HOME_20:
						{
							if (arrMt[Axis.Y].IsHomeDone() == false || arrMt[Axis.Y].IsRdy() == false) break;

							smm.Next(Cmd.HOME_30);
						}

						break;
					case Cmd.HOME_30:
						{
							if (arrAct[Act.TrayGripX].IsPos(SenPos.BWD) == false ||
								arrAct[Act.TrayGripY].IsPos(SenPos.BWD) == false)
							{
								arrAct[Act.TrayGripX].SetPos(SenPos.BWD);
								arrAct[Act.TrayGripY].SetPos(SenPos.BWD);
								break;
							}

							smm.Next(Cmd.HOME_40);
						}
						break;
					case Cmd.HOME_40:
						{
							if (SM.FMT.IsFlow(FlowTray.Reject) == false)
							{
								if (arrMt[Axis.Y].IsRdy((int)PosY.TRAYCHANGE) == false)
								{
									arrMt[Axis.Y].MoveIndex((int)PosY.TRAYCHANGE);
									break;
								}
							}
							else
							{
								if (arrAct[Act.TrayGripY].IsPos(SenPos.FWD) == false)
								{
									arrAct[Act.TrayGripY].SetPos(SenPos.FWD);
									break;
								}

								if (arrAct[Act.TrayGripX].IsPos(SenPos.FWD) == false)
								{
									arrAct[Act.TrayGripX].SetPos(SenPos.FWD);
									break;
								}
								if (CheckSen() == false)
								{
									m_smmTrayChange.eInput = InState.run;
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
							if (SM.FMT.IsFlow(FlowTray.Reject) == false)
							{
								AddAlarm(ALM.ERROR_REJECT_TRAY_TABLE_FLOWMNG);
								smm.Next(Cmd.END);
								break;
							}

							if (arrAct[Act.TrayGripY].IsPos(SenPos.FWD) == false)
							{
								arrAct[Act.TrayGripY].SetPos(SenPos.FWD);
								break;
							}

							if (arrAct[Act.TrayGripX].IsPos(SenPos.FWD) == false)
							{
								arrAct[Act.TrayGripX].SetPos(SenPos.FWD);
								break;
							}

							G.cKernel.AOut(DO.TopRightRearDoor, true);

							smm.Next(Cmd.READY_20);
						}
						break;
							
					case Cmd.READY_20:
						{
							if (smm.Delayms(3000) == false) break;

							if (CheckSen() == false)
							{
								smm.Next(Cmd.READY_ALARM_SIZE);
								break;
							}

							smm.Next(Cmd.READY_30);
						}
						break;

					case Cmd.READY_30:
						{
							if (arrMt[Axis.Y].IsRdy((int)PosY.READY) == false)
							{
								arrMt[Axis.Y].MoveIndex((int)PosY.READY);
								break;
							}

							smm.Next(Cmd.END);
						}
						break;

					case Cmd.READY_ALARM_DIR:
						{

							if (arrMt[Axis.Y].IsRdy((int)PosY.TRAYCHANGE) == false)
							{
								arrMt[Axis.Y].MoveIndex((int)PosY.TRAYCHANGE);
								break;
							}

							G.cKernel.AOut(DO.TopRightRearDoor, false);


							if (arrAct[Act.TrayGripX].IsPos(SenPos.BWD) == false ||
								arrAct[Act.TrayGripY].IsPos(SenPos.BWD) == false)
							{
								arrAct[Act.TrayGripX].SetPos(SenPos.BWD);
								arrAct[Act.TrayGripY].SetPos(SenPos.BWD);
								break;
							}

							AddAlarm(ALM.ERROR_REJECT_TRAY_TABLE_TRAY_DIRECTION);

							smm.Next(Cmd.END);
						}
						break;

					case Cmd.READY_ALARM_SIZE:
						{
							if (arrMt[Axis.Y].IsRdy((int)PosY.TRAYCHANGE) == false)
							{
								arrMt[Axis.Y].MoveIndex((int)PosY.TRAYCHANGE);
								break;
							}

							G.cKernel.AOut(DO.TopRightRearDoor, false);
							//check 센서 확인 어떻게 하는게 좋을지
							//if (SM.SHM.GetRecipeDataToInt(DEVICE.ModuleType) / 1000 == (int)ModuleType.SO_DIMM &&
							//		 G.cKernel.IsAIn(DI.RejectTraySizeSen) == true)
							//{
							//	AddAlarm(ALM.ERROR_SEQ_REJECT_BUF_TABLE_NOT_SMALL_SIZE);
							//}
							//else if (SM.SHM.GetRecipeDataToInt(DEVICE.ModuleType) / 1000 == (int)ModuleType.R_DIMM &&
							//G.cKernel.IsAIn(DI.RejectTraySizeSen) == false)
							//{
							//	AddAlarm(ALM.ERROR_SEQ_REJECT_BUF_TABLE_NOT_LARGE_SIZE);
							//}

							if (arrAct[Act.TrayGripX].IsPos(SenPos.BWD) == false ||
								arrAct[Act.TrayGripY].IsPos(SenPos.BWD) == false)
							{
								arrAct[Act.TrayGripX].SetPos(SenPos.BWD);
								arrAct[Act.TrayGripY].SetPos(SenPos.BWD);
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

		private void TrayChange(SMM<Cmd> smm)
		{
			try
			{
				if (smm.CurStep == Cmd.TRAYCHANGE_20 ||
					smm.CurStep == Cmd.TRAYCHANGE_30)
				{
					AddAlarm(ALM.WARNING_REJECT_TRAY_REPLACE);
					G.cKernel.AOut(DO.RejectLampRed, true);
					SM.SHM.MsgShow(SeqMsg.RejectTrayChange);
				}

				switch (smm.CurStep)
				{
					case Cmd.START:
						{
							if (smm.eInput == InState.run)
							{
								smm.eInput = InState.none;
								m_SgemRetryCount = 0;

								BeforeState = SM.SHM.GetCurrentState();
								SM.SHM.ChangeState(RunStateType.RejectTrayAlarm);
								smm.Next(Cmd.TRAYCHANGE_MESCHECK1);
							}
						}
						break;

					case Cmd.TRAYCHANGE_MESCHECK1:
						{
							if (SM.SHM.GetUseSkipData(USESKIP.SGEM) == false || SM.FMT.IsFlow(FlowTray.Reject) == false)
							{
								smm.Next(Cmd.TRAYCHANGE_10);
								break;
							}
							//Pick중에 SendTrayValidation 진행중일 수 있기 때문에 대기, 시퀀스 진행해보고 문제 발생시 디테일하게 체크
							if (G.seqUnloadTransfer.IsPickReady() == false) break;

							if (G.seqUBIGEMComm.IsTrayValidationReady() == false) break;
							
							var info_t = SM.FMT[FlowTray.Reject].GetInfo();
							info_t.OutTime = DateTime.Now.Ticks;
							SM.FMT[FlowTray.Reject].SetInfo(info_t);

							smm.Next(Cmd.TRAYCHANGE_MESCHECK2);
						}
						break;

					case Cmd.TRAYCHANGE_MESCHECK2:
						{
							if (SM.SHM.GetUseSkipData(USESKIP.SGEM) == false)
							{
								smm.Next(Cmd.TRAYCHANGE_10);
								break;
							}

							if (G.seqUBIGEMComm.Result_TrayValidation == false)
							{
								G.seqUBIGEMComm.TrayValidationStart(FlowTray.Reject);
								break;
							}

							smm.Next(Cmd.TRAYCHANGE_10);
						}
						break;


                    case Cmd.TRAYCHANGE_10:
						{
                            arrAct[Act.AirVib].SetPos(SenPos.OFF);
                            if (smm.Delayms(500) == false) break;

                            if (G.seqUnloadPnP.IsReady == false) break;
						
							if (arrMt[Axis.Y].IsRdy((int)PosY.TRAYCHANGE) == false)
							{
								arrMt[Axis.Y].MoveIndex((int)PosY.TRAYCHANGE);
								break;
							}

							smm.Next(Cmd.TRAYCHANGE_20);
						}
						break;

					case Cmd.TRAYCHANGE_20:
						{
							if (arrAct[Act.TrayGripX].IsPos(SenPos.BWD) == false ||
								arrAct[Act.TrayGripY].IsPos(SenPos.BWD) == false)
							{
								arrAct[Act.TrayGripX].SetPos(SenPos.BWD);
								arrAct[Act.TrayGripY].SetPos(SenPos.BWD);
								break;
							}

							G.cKernel.AOut(DO.TopRightRearDoor, false);

							//문 열리면 리젝 부저는 끔
							if (G.cKernel.IsAIn(DI.TopRightRearDoorSen) == true)		break;
							else														SM.SHM.SetBitData(BIT.RejectBuzzer, false);
							
							if (G.cKernel.IsAIn(DI.RejectTrayExistSen) == true) break;

							InfoTray.tsInfo info_t = SM.FMT[FlowTray.Reject].GetInfo();
							FlowRejectTray flow = SM.FMR.GetFirstEmptyFlow(FlowRejectTray.RejectBuf1, FlowRejectTray.RejectBuf15);
							SM.FMR.Next(FlowRejectTray.Empty, flow);
							SM.FMR[flow].SetInfo(info_t);
							SM.FMT.Clear(FlowTray.Reject);

							smm.Next(Cmd.TRAYCHANGE_30);
						}
						break;


					case Cmd.TRAYCHANGE_30:
						{
							if (G.cKernel.IsAIn(DI.RejectTrayExistSen) == false)		break;
							if (G.cKernel.IsAIn(DI.TopRightRearDoorSen) == false)		break;

							G.cKernel.AOut(DO.TopRightRearDoor, true);

							smm.Next(Cmd.TRAYCHANGE_40);
						}
						break;


					case Cmd.TRAYCHANGE_40:
						{
							if (smm.Delayms(1000) == false) break;

							if (G.cKernel.IsAIn(DI.RejectTrayExistSen) == false)
							{
								smm.Next(Cmd.TRAYCHANGE_20);
								break;
							}

							SM.FMT.Next(FlowTray.Empty, FlowTray.Reject);

							var tcinfo = SM.FMT[FlowTray.Reject].GetInfo();
							tcinfo.WorkOrder = "";
							tcinfo.RowCnt = SM.SHM.GetRecipeDataToInt(DEVICE.TrayRowCnt) / 1000;
							tcinfo.ColCnt = SM.SHM.GetRecipeDataToInt(DEVICE.TrayColCnt) / 1000;
							tcinfo.InTime = DateTime.Now.Ticks;
							tcinfo.ModuleType = SM.SHM.GetRecipeData(DEVICE.ModuleType);
							tcinfo.TrayBarcode = "Reject";
							SM.FMT[FlowTray.Reject].SetInfo(tcinfo);

							G.AlmMng.Del(ALM.WARNING_REJECT_TRAY_REPLACE);
							SM.SHM.SetBitData(BIT.RejectBuzzer, false);
							G.cKernel.AOut(DO.RejectLampRed, false);
							SM.SHM.MsgClose(SeqMsg.RejectTrayChange);
							SM.SHM.ChangeState(BeforeState);

							smm.Next(Cmd.TRAYCHANGE_50);
						}
						break;


					case Cmd.TRAYCHANGE_50:
						{
							if (arrAct[Act.TrayGripY].IsPos(SenPos.FWD) == false)
							{
								arrAct[Act.TrayGripY].SetPos(SenPos.FWD);
								break;
							}

							if (arrAct[Act.TrayGripX].IsPos(SenPos.FWD) == false)
							{
								arrAct[Act.TrayGripX].SetPos(SenPos.FWD);
								break;
							}
							smm.Next(Cmd.TRAYCHANGE_BARCODECHECK1);
						}
						break;
						
					case Cmd.TRAYCHANGE_BARCODECHECK1:
						{
							if (SM.SHM.GetUseSkipData(USESKIP.BarcodeRead) == false)
							{
								smm.Next(Cmd.END);
								break;
							}

							if (arrMt[Axis.Y].IsRdy((int)PosY.BARCODEREAD) == false)
							{
								arrMt[Axis.Y].MoveIndex((int)PosY.BARCODEREAD);
								break;
							}
							

							smm.Next(Cmd.TRAYCHANGE_BARCODECHECK2);
						}
						break;
						
					case Cmd.TRAYCHANGE_BARCODECHECK2:
						{
							BarcodeScanStart();

							smm.Next(Cmd.TRAYCHANGE_BARCODECHECK3);
						}
						break;

					case Cmd.TRAYCHANGE_BARCODECHECK3:
						{
							if (smm.Delayms(100) == false) break;

							BarcodeScanStop() ;
							
							smm.Next(Cmd.TRAYCHANGE_BARCODECHECK4);
						}
						break;

					case Cmd.TRAYCHANGE_BARCODECHECK4:
						{
							string strRejectBarcode = GetBarcodeResult();
							if (Program._bSimul_ == true)
							{
								string tempModel = SM.SHM.FindRegexByModel(SM.SHM.GetRecipeData(DEVICE.ModuleType));
								strRejectBarcode = tempModel + "-Reject";
							}


							if (smm.Delayms(5000) == false  && string.IsNullOrEmpty(strRejectBarcode) == true)			break;

							if (string.IsNullOrEmpty(strRejectBarcode) == true)
							{
								smm.ResetDelayms();
								AddAlarm(ALM.ERROR_SEQ_REJECT_BUFFER1_BARCODE_READ_FAIL);
                                SM.FMT.Clear(FlowTray.Reject);
                                smm.Next(Cmd.END);
								break;
							}
                            string[] parts = strRejectBarcode.Split('|');
                            string model1 = SM.SHM.FindModelByRegex(parts[0]);
                            string model2 = SM.SHM.FindModelByRegex(parts[1]);
                            if (SM.SHM.GetRecipeData(DEVICE.ModuleType) != model1 &&
                                SM.SHM.GetRecipeData(DEVICE.ModuleType) != model2 )
                            {
								string strErrMsg =  $"Barcode Value : {strRejectBarcode}" + Environment.NewLine +
													$"Barcode Model : {model1}. {model2}" + Environment.NewLine + 
													$"Current Model : {SM.SHM.GetRecipeData(DEVICE.ModuleType)}" ;

								SM.SHM.SetMsg(SeqMsg.TrayBarcode_RegexMatchFail, strErrMsg);
								SM.SHM.MsgShow(SeqMsg.TrayBarcode_RegexMatchFail);
                                AddAlarm(ALM.ERROR_SEQ_REJECT_BUFFER1_BARCODE_REGEX_MATCH_FAIL);
                                SM.FMT.Clear(FlowTray.Reject);
                                smm.Next(Cmd.END);
								break;
							}
							
							var tcinfo = SM.FMT[FlowTray.Reject].GetInfo();
							tcinfo.TrayBarcode = strRejectBarcode;
							SM.FMT[FlowTray.Reject].SetInfo(tcinfo);
							
							if (CheckSen() == false) //센서가 안맞는경우 다시 flow삭제
							{
								smm.Next(Cmd.TRAYCHANGE_NG);
								break;
							}

							smm.Next(Cmd.END);
						}
						break;

					case Cmd.TRAYCHANGE_NG:
						{
							if (arrAct[Act.TrayGripY].IsPos(SenPos.BWD) == false ||
								arrAct[Act.TrayGripX].IsPos(SenPos.BWD) == false)
							{
								arrAct[Act.TrayGripY].SetPos(SenPos.BWD);
								arrAct[Act.TrayGripX].SetPos(SenPos.BWD);
								break;
							}

							G.cKernel.AOut(DO.TopRightRearDoor, true);
							//alarm
							CheckSen(true);
							smm.Next(Cmd.TRAYCHANGE_20);
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

		private void Align(SMM<Cmd> smm)
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
								smm.Next(Cmd.ALIGN_10);
							}
						}
						break;

					case Cmd.ALIGN_10:
						{
							if (SM.SHM.GetUseSkipData(USESKIP.RejectBufferAligner) == false ||
								SM.FMT[FlowTray.Reject].IsAlignDone() == true)
							{
								smm.Next(Cmd.END);
								break;
							}

							arrAct[Act.AirVib].SetPos(SenPos.ON);

							//G.cKernel.AOut(DO_Aligner, true);

							smm.Next(Cmd.ALIGN_20);
						}
						break;

					case Cmd.ALIGN_20:
						{
							if (smm.Delayms(NDm.Read(eNDM.AirVibrateTime)) == false)
							{
								break;
							}

							arrAct[Act.AirVib].SetPos(SenPos.OFF);
							//G.cKernel.AOut(DO_Aligner, false);

							smm.Next(Cmd.END);
						}
						break;


					case Cmd.END:
						{
							SM.FMT[FlowTray.Reject].SetTrayHandlingStatus(ProdHandlingStatus.Aligned);
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