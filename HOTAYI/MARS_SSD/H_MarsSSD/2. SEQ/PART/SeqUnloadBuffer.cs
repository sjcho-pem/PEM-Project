using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_MarsSSD
{
	public class SeqUnloadBuffer : SeqBase<SeqUnloadBuffer.Act, SeqUnloadBuffer.Axis, SeqUnloadBuffer.Cmd>
	{
		#region [ Enum ]
		public enum Act
		{
			Max,
		}

		public enum Axis
		{
			Y,
			GripX,
			GripY,
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

			LD_WAIT_WAIT,
			LD_WAIT_10, // Load 위치로 가서 기다림
			LD_WAIT_20,
			LD_WAIT_30,
			LD_WAIT_40,
			LD_WAIT_50,
			LD_WAIT_60,
			LD_WAIT_70,
			LD_WAIT_80,
			LD_WAIT_90,
			LD_WAIT_100,
			LD_WAIT_110,
			LD_WAIT_120,
			LD_WAIT_130,
			LD_WAIT_140,
			LD_WAIT_150,

			ULD_WAIT_WAIT,
			ULD_WAIT_10, // Unload 위치로 가서 기다림
			ULD_WAIT_20,
			ULD_WAIT_20_MESCHECK1,
			ULD_WAIT_20_MESCHECK2,
			ULD_WAIT_20_MESCHECK3,
			ULD_WAIT_20_MESCHECK4,
			ULD_WAIT_30,
			ULD_WAIT_31,
			ULD_WAIT_40,
			ULD_WAIT_50,
			ULD_WAIT_60,
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
			UNLOAD,
			LOAD,
		}
		
		public enum PosGripX
		{
			LOADRELEASE,
			UNLOADRELEASE,
			GRIP,
            RELEASE,
        }
		public enum PosGripY
		{
			LOADRELEASE,
			UNLOADRELEASE,
			GRIP,
            RELEASE,
		}
		#endregion

		#region [Variables]
		private SMM<Cmd>	m_smmHome		= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.ULDBUFFER_HOME));
		private SMM<Cmd>	m_smmLoadWait	= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.ULDBUFFER_LOADWAIT));
		private SMM<Cmd>	m_smmUnloadWait	= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.ULDBUFFER_UNLOADWAIT));

		public	Cmd	smmHome			{ get => m_smmHome.CurStep; }
		public	Cmd	smmLoadWait		{ get => m_smmLoadWait.CurStep; }
		public	Cmd	smmUnloadWait	{ get => m_smmUnloadWait.CurStep; }

		private bool	m_IsGrip		= false; // 현재 버퍼가 제품을 잡고 있는지를 저장, 오직 다른 곳에서 함수로만 확인 가능 
		private bool	m_IsServerSend	= false;
		private	int		m_nCurPos		= 0;
		private	bool	m_IsMtHome		= true;
		private	int		m_MesRetryCount = 0;
		#endregion



		public	void	Init()
		{
			try
			{
				lstSMM.Add(m_smmHome);
				lstSMM.Add(m_smmLoadWait);
				lstSMM.Add(m_smmUnloadWait);

				#region - Motor
				arrMt	= new EnumArray<Axis, CMotor>(Axis.Max);
				arrMt[Axis.Y]		= G.cKernel.Motor(MT.ULDBufY);
				arrMt[Axis.GripX]	= G.cKernel.Motor(MT.ULDBufGripX);
				arrMt[Axis.GripY]	= G.cKernel.Motor(MT.ULDBufGripY);
				#endregion

				#region - Actuator
				arrAct = new EnumArray<Act, Actuator>(Act.Max);
				#endregion

				//Sensor
				/*
				ExistSen1
				ExistSen2
				ExistSen3
				ExistSen4
				*/

				base.Initialize();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
		
		public	bool	HomeStart(bool IsMtHome = true)	{m_IsMtHome = IsMtHome;  return SetSeqRun(ref m_smmHome); }

		public	bool	LoadWaitStart()				{ return SetSeqRun(ref m_smmLoadWait); }

		public	bool	UnloadWaitStart()			{ return SetSeqRun(ref m_smmUnloadWait); }



		public	bool	IsHomeReady()				{ return m_smmHome.IsReady; }

		public	bool	IsLoadWaitReady()			{ return m_smmLoadWait.IsReady; }

		public	bool	IsUnloadWaitReady()			{ return m_smmUnloadWait.IsReady; }

		public	bool	IsLDWait()					{ return m_smmLoadWait.CurStep == Cmd.LD_WAIT_WAIT; }

		public	bool	IsULDWait()					{ return m_smmUnloadWait.CurStep == Cmd.ULD_WAIT_WAIT; }

		public	bool	IsBufferGrip()				{ return m_IsGrip; }
		
		private ProdExist CheckProdExist()
		{
			try
			{
				FlowPart flowPart = FlowPart.ULDBuffer;
				DI ExistSenStart = DI.ULDBufExistSen1;

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

		private	int		CheckInspResultExist()
		{
			try
			{
				bool[]	bexist	= SM.FMP[FlowPart.ULDBuffer].GetProdExist();

				for (int i = 0; i < SM.FMP[FlowPart.ULDBuffer].GetMaxProdCount(); i++)
				{
					if (bexist[i] == true)
					{
						if (SM.FMP[FlowPart.ULDBuffer].GetInfo().ProdInfo[i].ResultTop == (int)ProdStatus.Unknown)			return 1;
						if (SM.FMP[FlowPart.ULDBuffer].GetInfo().ProdInfo[i].ResultBtm == (int)ProdStatus.Unknown)			return 2;
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

		private bool	SetSeqRun(ref SMM<Cmd> smm)
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
				UpdateAct(Seq.UnloadBuffer);

				if (SM.SHM.IsBitData(BIT.Error) == true || SM.SHM.IsBitData(BIT.Pause) == true) return;

				Home(m_smmHome);
				LoadWait(m_smmLoadWait);
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
							if (m_IsMtHome == false  || CheckProdExist() == ProdExist.Exist)
							{
								smm.Next(Cmd.HOME_50);
								break;
							}
							
							arrMt[Axis.GripX].ClearHome();
							arrMt[Axis.GripX].StartHoming();

							arrMt[Axis.GripY].ClearHome();
							arrMt[Axis.GripY].StartHoming();

							
							smm.Next(Cmd.HOME_20);
						}
						break;

					case Cmd.HOME_20:
						{
							if (arrMt[Axis.GripX].IsHomeDone() == false || arrMt[Axis.GripX].IsRdy() == false) break;
							if (arrMt[Axis.GripY].IsHomeDone() == false || arrMt[Axis.GripY].IsRdy() == false) break;

							smm.Next(Cmd.HOME_30);
						}
						break;

					case Cmd.HOME_30:
						{
							arrMt[Axis.Y].ClearHome();
							arrMt[Axis.Y].StartHoming();

							smm.Next(Cmd.HOME_40);
						}
						break;
					case Cmd.HOME_40:
						{
							if (arrMt[Axis.Y].IsHomeDone() == false || arrMt[Axis.Y].IsRdy() == false) break;

							smm.Next(Cmd.HOME_50);
						}
						break;

					case Cmd.HOME_50:
						{
							if (arrMt[Axis.GripX].IsHomeDone() == false)
							{
								AddAlarm(ALM.ERROR_MT_HOME_ULD_BUF_GRIP_X);
								smm.Next(Cmd.END);
								break;
							}
							if (arrMt[Axis.GripY].IsHomeDone() == false)
							{
								AddAlarm(ALM.ERROR_MT_HOME_ULD_BUF_GRIP_Y);
								smm.Next(Cmd.END);
								break;
							}
							
							if (arrMt[Axis.Y].IsHomeDone() == false)
							{
								AddAlarm(ALM.ERROR_MT_HOME_ULD_BUF_Y);
								smm.Next(Cmd.END);
								break;
							}

							if (SM.FMP.IsFlow(FlowPart.ULDBuffer) )
							{
								if (arrMt[Axis.GripX].IsRdy((int)PosGripX.GRIP) == false)
								{
									arrMt[Axis.GripX].MoveIndex((int)PosGripX.GRIP);
									break;
								}
								if (arrMt[Axis.GripY].IsRdy((int)PosGripY.GRIP) == false)
								{
									arrMt[Axis.GripY].MoveIndex((int)PosGripY.GRIP);
									break;
								}
							}
							else
							{
								if (arrMt[Axis.GripX].IsRdy((int)PosGripX.LOADRELEASE) == false)
								{
									arrMt[Axis.GripX].MoveIndex((int)PosGripX.LOADRELEASE);
									break;
								}
								if (arrMt[Axis.GripY].IsRdy((int)PosGripY.LOADRELEASE) == false)
								{
									arrMt[Axis.GripY].MoveIndex((int)PosGripY.LOADRELEASE);
									break;
								}
							}

							if (arrMt[Axis.Y].IsRdy((int)PosY.LOAD) == false)
							{
								arrMt[Axis.Y].MoveIndex((int)PosY.LOAD);
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

					case Cmd.LD_WAIT_10:      // RAM 유무 확인
						{
							switch (CheckProdExist())
							{
								case ProdExist.Exist:
									AddAlarm(ALM.ERROR_SEQ_UNLOAD_BUFFER_PROD_EXIST);
									smm.Next(Cmd.END);
									break;
								case ProdExist.Mismatch:
									AddAlarm(ALM.ERROR_SEQ_UNLOAD_BUFFER_PROD_MISMATCH);
									smm.Next(Cmd.END);
									break;
								case ProdExist.None:
									m_IsGrip = false;
									smm.Next(Cmd.LD_WAIT_20);
									break;
							}
						}
						break;

					case Cmd.LD_WAIT_20:      // Load 위치 이동, Grip Release
						{
							if (arrMt[Axis.Y].IsRdy((int)PosY.LOAD) == false ||
								arrMt[Axis.GripX].IsRdy((int)PosGripX.LOADRELEASE) == false||
								arrMt[Axis.GripY].IsRdy((int)PosGripY.LOADRELEASE) == false)
							{
								arrMt[Axis.Y].MoveIndex((int)PosY.LOAD);
								arrMt[Axis.GripX].MoveIndex((int)PosGripX.LOADRELEASE);
								arrMt[Axis.GripY].MoveIndex((int)PosGripY.LOADRELEASE);
								break;
							}

							smm.Next(Cmd.LD_WAIT_WAIT);
						}
						break;

					case Cmd.LD_WAIT_WAIT:        // Data 이전 대기
						{
							if (SM.FMP.IsFlow(FlowPart.ULDBuffer) == false)		break;
							if (SM.FMP[FlowPart.ULDBuffer].GetExistCount() == 0)	break;

							if (G.seqUnloadInspPnP.bIsGrip == true)				break;
							if (G.seqUnloadInspPnP.IsPlaceReady() == false)		break;

							smm.Next(Cmd.LD_WAIT_30);
						}
						break;

					case Cmd.LD_WAIT_30:      // Grip
						{
							if (arrMt[Axis.GripX].IsRdy((int)PosGripX.GRIP) == false  ||
								arrMt[Axis.GripY].IsRdy((int)PosGripY.GRIP) == false)
							{
								arrMt[Axis.GripX].MoveIndex((int)PosGripX.GRIP);
								arrMt[Axis.GripY].MoveIndex((int)PosGripY.GRIP);
								break;
							}

							m_IsGrip = true;
							m_IsServerSend = false;

							smm.Next(Cmd.LD_WAIT_40);
						}
						break;

					case Cmd.LD_WAIT_40:      // RAM 유무확인
						{
							if (smm.Delayms(200) == false) break;

							switch (CheckProdExist())
							{
								case ProdExist.Exist:
									smm.Next(Cmd.LD_WAIT_50);
									break;
								case ProdExist.Mismatch:
									AddAlarm(ALM.ERROR_SEQ_UNLOAD_BUFFER_PROD_MISMATCH);
									smm.Next(Cmd.END);
									break;
								case ProdExist.None:
									AddAlarm(ALM.ERROR_SEQ_UNLOAD_BUFFER_PROD_NONE);
									smm.Next(Cmd.END);
									break;
							}
						}
						break;

					case Cmd.LD_WAIT_50:      // UnloadInspPnP Place 완료 대기
						{
							if (G.seqUnloadInspPnP.IsPlaceReady() == false) break;

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

					case Cmd.ULD_WAIT_10:     // RAM 유무 확인
						{
							switch (CheckProdExist())
							{
								case ProdExist.Exist:
									m_IsGrip = true;
									smm.Next(Cmd.ULD_WAIT_20);
									break;
								case ProdExist.Mismatch:
									AddAlarm(ALM.ERROR_SEQ_UNLOAD_BUFFER_PROD_MISMATCH);
									smm.Next(Cmd.END);
									break;
								case ProdExist.None:
									AddAlarm(ALM.ERROR_SEQ_UNLOAD_BUFFER_PROD_NONE);
									smm.Next(Cmd.END);
									break;
							}
						}
						break;

					case Cmd.ULD_WAIT_20:         // Unload 위치 이동
						{
							if (arrMt[Axis.GripX].IsRdy((int)PosGripX.GRIP) == false  ||
								arrMt[Axis.GripY].IsRdy((int)PosGripY.GRIP) == false)
							{
								arrMt[Axis.GripX].MoveIndex((int)PosGripX.GRIP);
								arrMt[Axis.GripY].MoveIndex((int)PosGripY.GRIP);
								break;
							}

							if (arrMt[Axis.Y].IsRdy((int)PosY.UNLOAD) == false)
							{
								arrMt[Axis.Y].MoveIndex((int)PosY.UNLOAD);
								break;
							}

							if (smm.Delayms(NDm.Read(eNDM.InspWaitTimeOut)) == true)
							{
								if (CheckInspResultExist() == 1) AddAlarm(ALM.ERROR_SEQ_INSP_TOP_PC_ISNPECTION_TIMEOUT);
								else AddAlarm(ALM.ERROR_SEQ_INSP_BTM_PC_ISNPECTION_TIMEOUT);
								smm.ResetDelayms();

								break;
							}

							if (CheckInspResultExist() != 0) break;
							m_MesRetryCount = 0;
							smm.Next(Cmd.ULD_WAIT_20_MESCHECK1);
						}
						break;


					case Cmd.ULD_WAIT_20_MESCHECK1:
						{
							if (G.seqJobTeaching.IsReady == false ||
								SM.SHM.GetUseSkipData(USESKIP.TopInspection) == false ||
								SM.SHM.GetUseSkipData(USESKIP.SGEM) == false)
							{
								smm.Next(Cmd.ULD_WAIT_30);
								break;
							}

							int nCount = 0;
							for (int i = 0; i < SM.FMP[FlowPart.ULDBuffer].GetInfo().ProdInfo.Length; i++)
							{
								if (SM.FMP[FlowPart.ULDBuffer].GetProdExist(i) == true)
								{
									if (SM.FMP[FlowPart.ULDBuffer].GetInfo().ProdInfo[i].SerialNumber.ToLower().Contains("error") == false &&
										SM.FMP[FlowPart.ULDBuffer].GetInfo().ProdInfo[i].ResultMES == (int)MESStatus.Unknown)
									{
										nCount++;
									}
								}
							}
							//모든 제품이 Error/ 이미 결과보고를 받은 경우 MES통신을 하지 않고 넘어감
							if (nCount == 0)
							{
								smm.Next(Cmd.ULD_WAIT_30);
								break;
							}

							m_MesRetryCount++;
							if (m_MesRetryCount > 3)
							{
								//워닝
								AddAlarm(ALM.WARNING_SESCGEM_INPSECTION_RESULT_CONFIRM_SECSGEM_NG);
								smm.Next(Cmd.ULD_WAIT_20_MESCHECK4);
								break;
							}
							G.commUbiGEM.SendInspectionResult(FlowPart.ULDBuffer);

							smm.Next(Cmd.ULD_WAIT_20_MESCHECK2);
						}
						break;

					case Cmd.ULD_WAIT_20_MESCHECK2:
						{
							if (SM.SHM.GetUseSkipData(USESKIP.SGEM) == false)
							{
								smm.Next(Cmd.ULD_WAIT_30);
								break;
							}

							int nAck = -1;
							int nret = G.commUbiGEM.RecvAck6(REQ.InspectionResult, ref nAck);
							if (nret > 0)
							{
								//ack - Accepted
								if (nAck == 0)
								{
									smm.Next(Cmd.ULD_WAIT_20_MESCHECK3);
								}
								else//ack - ErrorNotAccepted
								{
									//AddAlarm(ALM.ERROR_SESCGEM_INSPECTION_RESULT_ACK_ERROR);
									smm.Next(Cmd.ULD_WAIT_20_MESCHECK1);
								}
							}
							else if (smm.Delayms(NDm.Read(eNDM.SecsGemCommTimeOut)) == true)
							{
								//AddAlarm(ALM.ERROR_SESCGEM_TIMEOUT);
								smm.Next(Cmd.ULD_WAIT_20_MESCHECK1);
							}
						}
						break;

					case Cmd.ULD_WAIT_20_MESCHECK3:
						{
							if (SM.SHM.GetUseSkipData(USESKIP.SGEM) == false)
							{
								smm.Next(Cmd.ULD_WAIT_30);
								break;
							}

							if (smm.Delayms(5000) == true)
							{
								smm.Next(Cmd.ULD_WAIT_20_MESCHECK1);
								break;
							}

							bool bResultComplete = true;
							for (int i = 0; i < SM.FMP[FlowPart.ULDBuffer].GetMaxProdCount(); i++)
							{
								if ((SM.FMP[FlowPart.ULDBuffer].GetProdExist(i) == true && SM.FMP[FlowPart.ULDBuffer].GetInfo().ProdInfo[i].SerialNumber.ToLower() != "error") &&
									SM.FMP[FlowPart.ULDBuffer].GetInfo().ProdInfo[i].ResultMES == (int)MESStatus.Unknown)
								{
									bResultComplete = false;
									break;
								}
							}
							if (bResultComplete == false)
								break;


							smm.Next(Cmd.ULD_WAIT_30);
						}
						break;


					case Cmd.ULD_WAIT_20_MESCHECK4: //3회 이상 ack이 없거나, result Confirm이 제대로 오지 않은 경우
						{
							if (SM.SHM.GetUseSkipData(USESKIP.SGEM) == false)
							{
								smm.Next(Cmd.ULD_WAIT_30);
								break;
							}

							if (SM.SHM.IsMsgPopUp(SeqMsg.SGEMConnect) == false)
							{
								SM.SHM.MsgShow(SeqMsg.SGEMConnect);
							}

							string strResult = SM.SHM.PopupMsgResult(SeqMsg.SGEMConnect);
							switch (strResult)
							{
								case "RETRY":
									m_MesRetryCount = 0;
									smm.Next(Cmd.ULD_WAIT_20_MESCHECK1);
									break;
							}

						}
						break;

					case Cmd.ULD_WAIT_30:
						{

                            if (arrMt[Axis.GripX].IsRdy((int)PosGripX.UNLOADRELEASE) == false ||
                                arrMt[Axis.GripY].IsRdy((int)PosGripY.UNLOADRELEASE) == false)
                            {
                                arrMt[Axis.GripX].MoveIndex((int)PosGripX.UNLOADRELEASE);
                                arrMt[Axis.GripY].MoveIndex((int)PosGripY.UNLOADRELEASE);
                                break;
                            }

                            smm.Next(Cmd.ULD_WAIT_WAIT);
						}
						break;

					case Cmd.ULD_WAIT_WAIT:       // Data 이전 대기
						{
							if (SM.FMP.IsFlow(FlowPart.ULDBuffer) == true)	break;
							
							smm.Next(Cmd.ULD_WAIT_40);
						}
						break;

                    case Cmd.ULD_WAIT_40:
                        {
                            if (smm.Delayms(300) == false) break;

                            if (arrMt[Axis.GripX].IsRdy((int)PosGripX.RELEASE) == false ||
                                arrMt[Axis.GripY].IsRdy((int)PosGripY.RELEASE) == false)
                            {
                                arrMt[Axis.GripX].MoveIndex((int)PosGripX.RELEASE);
                                arrMt[Axis.GripY].MoveIndex((int)PosGripY.RELEASE);
                                break;
                            }

                            m_IsGrip = false;

                            smm.Next(Cmd.ULD_WAIT_50);
                        }
                        break;


                    case Cmd.ULD_WAIT_50:     // UnloadInspPnP Pick 완료 대기
						{
                            if (G.seqUnloadPnP.IsPickBufReady() == false) break;

							switch (CheckProdExist())
							{
								case ProdExist.Exist:
									AddAlarm(ALM.ERROR_SEQ_UNLOAD_BUFFER_PROD_EXIST);
									smm.Next(Cmd.END);
									break;
								case ProdExist.Mismatch:
									AddAlarm(ALM.ERROR_SEQ_UNLOAD_BUFFER_PROD_MISMATCH);
									smm.Next(Cmd.END);
									break;
								case ProdExist.None:
									m_IsServerSend = false;
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