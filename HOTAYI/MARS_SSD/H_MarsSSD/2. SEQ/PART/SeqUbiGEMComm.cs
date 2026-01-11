using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_MarsSSD
{
	public class SeqUbiGEMComm : SeqBase<SeqUbiGEMComm.teAct, SeqUbiGEMComm.teAxis, SeqUbiGEMComm.teCmd>
	{
		#region [ Enum ]
		public enum teAct
		{
			Max,
		}

		public enum teAxis
		{
			Max,
		}

		public enum teCmd
		{
			START	= 10,
			END		= 99,
			END_OK	= 97,
			END_NG	= 98,

			FVI_PGM_REQ_10		= 100,
			FVI_PGM_REQ_20,
			FVI_PGM_REQ_30,
			FVI_PGM_REQ_40,
			FVI_PGM_REQ_50,
			FVI_PGM_REQ_60,
			FVI_PGM_REQ_70,
			FVI_PGM_REQ_80,
			FVI_PGM_REQ_90,
			FVI_PGM_REQ_100,
			FVI_PGM_REQ_110,
			FVI_PGM_REQ_120,

			FVI_START_REQ_10	= 200,
			FVI_START_REQ_20,
			FVI_START_REQ_30,
			FVI_START_REQ_40,
			FVI_START_REQ_50,
			FVI_START_REQ_60,
			FVI_START_REQ_70,
			FVI_START_REQ_80,
			FVI_START_REQ_90,
			FVI_START_REQ_100,
			FVI_START_REQ_110,
			FVI_START_REQ_120,
			
			UP_SERIAL_NO_REQ_10	= 300,
			UP_SERIAL_NO_REQ_20,
			UP_SERIAL_NO_REQ_30,
			UP_SERIAL_NO_REQ_40,
			UP_SERIAL_NO_REQ_50,
			UP_SERIAL_NO_REQ_60,
			UP_SERIAL_NO_REQ_70,
			UP_SERIAL_NO_REQ_80,
			UP_SERIAL_NO_REQ_90,
			UP_SERIAL_NO_REQ_100,
			UP_SERIAL_NO_REQ_110,
			UP_SERIAL_NO_REQ_120,

			ERROR_REQ_10	= 400,
			ERROR_REQ_20,
			ERROR_REQ_30,
			ERROR_REQ_40,
			ERROR_REQ_50,
			ERROR_REQ_60,
			ERROR_REQ_70,
			ERROR_REQ_80,
			ERROR_REQ_90,
			ERROR_REQ_100,
			ERROR_REQ_110,
			ERROR_REQ_120,

			EQ_MAINT_REQ_10	= 500,
			EQ_MAINT_REQ_20,
			EQ_MAINT_REQ_30,
			EQ_MAINT_REQ_40,
			EQ_MAINT_REQ_50,
			EQ_MAINT_REQ_60,
			EQ_MAINT_REQ_70,
			EQ_MAINT_REQ_80,
			EQ_MAINT_REQ_90,
			EQ_MAINT_REQ_100,
			EQ_MAINT_REQ_110,
			EQ_MAINT_REQ_120,
		}


		#endregion

		#region [Variables]
		private SMM<teCmd> m_smmFVIPGMReq			= new SMM<teCmd>(teCmd.START, EnumHelper.GetDescription(teSeqID.UBIGEM_FVI_PGM_REQ));
		private SMM<teCmd> m_smmFVIStartReq			= new SMM<teCmd>(teCmd.START, EnumHelper.GetDescription(teSeqID.UBIGEM_FVI_START_REQ));
		private SMM<teCmd> m_smmUPSerialNoReq		= new SMM<teCmd>(teCmd.START, EnumHelper.GetDescription(teSeqID.UBIGEM_UP_SERIAL_NO_REQ));
		private SMM<teCmd> m_smmErrorReq			= new SMM<teCmd>(teCmd.START, EnumHelper.GetDescription(teSeqID.UBIGEM_ERROR_REQ));
		private SMM<teCmd> m_smmEQMAINTReq			= new SMM<teCmd>(teCmd.START, EnumHelper.GetDescription(teSeqID.UBIGEM_EQ_MAINT_REQ));
		
		public teCmd smmFVIPGMReq			{ get => m_smmFVIPGMReq.CurStep; }
		public teCmd smmFVIStartReq			{ get => m_smmFVIStartReq.CurStep; }
		public teCmd smmUPSerialReq			{ get => m_smmUPSerialNoReq.CurStep; }
		public teCmd smmErrorReq			{ get => m_smmErrorReq.CurStep; }
		public teCmd smmEQMAINTReq			{ get => m_smmEQMAINTReq.CurStep; }

		private bool	m_bResult_FVIPGMReq				= false;
		private bool	m_bResult_FVIStartReq			= false;
		private bool	m_bResult_UPSerialReq			= false;
		private bool	m_bResult_ErrorReq				= false;
		private bool	m_bResult_EQMAINTReq			= false;
        private int     m_nRetryDelay ;
		private string  m_ModelName;		
		private string  m_LotID;			
		
        //호출 전 초기화 필요
		public bool		Result_FVIPGM		{ get => m_bResult_FVIPGMReq; set => m_bResult_FVIPGMReq = value;  }
		public bool		Result_FVIStart		{ get => m_bResult_FVIStartReq; set => m_bResult_FVIStartReq = value;}
		public bool		Result_UPSerial		{ get => m_bResult_UPSerialReq; set => m_bResult_UPSerialReq = value;}
		public bool		Result_Error		{ get => m_bResult_ErrorReq;    set => m_bResult_ErrorReq = value;}
		public bool		Result_EQMAINT		{ get => m_bResult_EQMAINTReq;  set => m_bResult_EQMAINTReq = value;} 

		public Dictionary<string, string>	FVIPGMRCVData				= new Dictionary<string, string>();
		public Dictionary<string, string>	FVISTARTRCVData			= new Dictionary<string, string>();
		public Dictionary<string, string>	UPSERIALNORCVData			= new Dictionary<string, string>();
		public Dictionary<string, string>	ERRORRCVData				= new Dictionary<string, string>();
		public Dictionary<string, string>	EQMAINTRCVData				= new Dictionary<string, string>();

		// event3 ~ equip_data 까지 총 12개
		public List<string>					EQMAINTREQ_SendData			= new List<string>();
		#endregion

		public	void	Init()
		{
			try
			{
				for(int i = 0; i < 12; i++)
				{
					EQMAINTREQ_SendData.Add(""); // 빈걸로 초기화
				}

				lstSMM.Add(m_smmFVIPGMReq);
				lstSMM.Add(m_smmFVIStartReq);
				lstSMM.Add(m_smmUPSerialNoReq);
				lstSMM.Add(m_smmErrorReq);
				lstSMM.Add(m_smmEQMAINTReq);

				#region - Motor
				arrMt = new EnumArray<teAxis, CMotor>(teAxis.Max);
				#endregion

				#region - Actuator            
				arrAct = new EnumArray<teAct, Actuator>(teAct.Max);
                #endregion
                
                if (NDm.Read(eNDM.SecsGemRetryDelay) < 5000 ) NDm.Write(eNDM.SecsGemRetryDelay, 5000);
                if (NDm.Read(eNDM.SecsGemAlarmDelay) < 30000 ) NDm.Write(eNDM.SecsGemAlarmDelay, 30000);
                
                if (Program.iniMachine.GetString("Machine", "EquipID") == "" )              Program.iniMachine.SetString("Machine", "EquipID", "temp_EquipID");
                if (Program.iniMachine.GetString("Machine", "EquipModel") == "" )           Program.iniMachine.SetString("Machine", "EquipModel", "temp_EquipModel");
                if (SDm.Read((eSDM.Operator1 + NDm.Read(eNDM.SelectedOperatorIdx))) == "" )               SDm.Write((eSDM.Operator1 + NDm.Read(eNDM.SelectedOperatorIdx)), "temp_user");
                if (NDm.Read(eNDM.SimulationLotCount) == 0 )    NDm.Write(eNDM.SimulationLotCount,50);
                if (SDm.Read(eSDM.OPER) == "" )                 SDm.Write(eSDM.OPER,"5800");

                

                m_bResult_UPSerialReq   = false;
                m_bResult_FVIPGMReq     = false;
                m_bResult_FVIStartReq   = false;
                m_bResult_ErrorReq      = false;
                m_bResult_EQMAINTReq    = false;

				base.Initialize();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public	void	FVIPGMReqStart()
        {
            if (SM.SHM.GetUseSkipData(USESKIP.UseMES) == false)
                return;
            SetSeqRun(ref m_smmFVIPGMReq);
        }
        public void FVIStartReqStart()
        {
            if (SM.SHM.GetUseSkipData(USESKIP.UseMES) == false)
                return;
            SetSeqRun(ref m_smmFVIStartReq);
        }
        public void UPSerialReqStart()
        {
            if (SM.SHM.GetUseSkipData(USESKIP.UseMES) == false)
                return;
            SetSeqRun(ref m_smmUPSerialNoReq);
        }
        public void ErrorReqStart()
        {
            if (SM.SHM.GetUseSkipData(USESKIP.UseMES) == false)
                return;
            SetSeqRun(ref m_smmErrorReq);
        }
        public void EQMAINTReqStart(List<string> _EQMAINTREQ_Data, string _ModelName, string _LotID)
        {
            if (SM.SHM.GetUseSkipData(USESKIP.UseMES) == false)
                return;
            m_ModelName = _ModelName;
            m_LotID = _LotID;
            EQMAINTREQ_SendData = _EQMAINTREQ_Data;
            SetSeqRun(ref m_smmEQMAINTReq);
        }

        
        public void EQMAINTReqStart(teEVENT2 _teEvent2, int nAlarmCode = 0)
        {
            if (SM.SHM.GetUseSkipData(USESKIP.UseMES) == false)
                return;
            m_ModelName = SDm.Read(eSDM.LotDevice1);
            m_LotID = SDm.Read(eSDM.LotID1);
            List<string> temp_SendData = new List<string>();
            temp_SendData.Add(teEVENT1.PRODUCT.ToString());
            temp_SendData.Add(_teEvent2.ToString());

            //Jam 일경우 최상단 에러 추가
            if (_teEvent2 == teEVENT2.JAM)
                temp_SendData.Add($"{nAlarmCode.ToString()}");

            EQMAINTREQ_SendData = temp_SendData;
            SetSeqRun(ref m_smmEQMAINTReq);
        }

        public	bool	IsFVIPGMReqReady()				{ return m_smmFVIPGMReq.IsReady; }
		public	bool	IsFVIStartReqReady()			{ return m_smmFVIStartReq.IsReady; }
		public	bool	IsUPSerialReqReady()			{ return m_smmUPSerialNoReq.IsReady; }
		public	bool	IsErrorReqReady()				{ return m_smmErrorReq.IsReady; }
		public	bool	IsEQMAINTReqReady()				{ return m_smmEQMAINTReq.IsReady; }
        public  void    Reset()
        {
                m_bResult_UPSerialReq   = false;
                m_bResult_FVIPGMReq     = false;
                m_bResult_FVIStartReq   = false;
                m_bResult_ErrorReq      = false;
                m_bResult_EQMAINTReq    = false;
        }
		private bool    SetSeqRun(ref SMM<teCmd> smm)
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

		public	void	Run()
		{
			try
			{
				FVIPGMReq(m_smmFVIPGMReq);
				FVIStartReq(m_smmFVIStartReq);
				UPSerialReq(m_smmUPSerialNoReq);
				ErrorReq(m_smmErrorReq);
				EQMAINTReq(m_smmEQMAINTReq);

				SM.SHM.SetSeqStatus(lstSMM);
			}
			catch (Exception ex)
            {
                G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
            }
		}

		private void	FVIPGMReq(SMM<teCmd> smm)
		{
			try
			{
				switch(smm.CurStep)
				{
					case teCmd.START:
						{
							if (smm.eInput == InState.run)
							{
								smm.eInput = InState.none;
                                m_bResult_FVIPGMReq = false;
								smm.Next(teCmd.FVI_PGM_REQ_10);
							}
						}
						break;
					case teCmd.FVI_PGM_REQ_10:
						{
                            m_nRetryDelay = NDm.Read(eNDM.SecsGemRetryDelay);
							G.commUbiGEM.SendFVIProgramInfoReq(SM.FMT[teFlowTray.LDLiftMax]);
							smm.Next(teCmd.FVI_PGM_REQ_20);
						}
						break;
					case teCmd.FVI_PGM_REQ_20:
						{
                            int nResult = G.commUbiGEM.RecvFVIPGM(ref FVIPGMRCVData);
							if(nResult <= -1)
							{ 
                                AddAlarm(ALM.ERROR_SESCGEM_DATA_LENGTH);
								smm.Next(teCmd.END_NG);
								break;
							}
							else if(nResult == 0)
							{
								if(smm.Delayms(m_nRetryDelay))
									smm.Next(teCmd.END_NG);

								break;
							}

							smm.Next(teCmd.END_OK);
						}
						break;
					case teCmd.END_OK:
                        {
                            //check
                            for (int i = 0; i < (int)teFVIPGMReply.MAX; i++)
                            {
                                if (FVIPGMRCVData.TryGetValue(((teFVIPGMReply)i).ToString(), out string str) == false)
                                {
                                    GLog.Add($"FVIPGMData - TryGetValue {((teFVIPGMReply)i).ToString()}");
                                    AddAlarm(ALM.ERROR_SESCGEM_DATA);
                                    smm.Next(teCmd.END_NG);
                                    break;
                                }
                            }

                            //Status ng이면 error msg로 알람을 띄워준다.
                            if (FVIPGMRCVData[teFVIPGMReply.status.ToString()] != "0")
                            {
                                G.AlmMng.ADD(ALM.ERROR_SESCGEM_STATUS, $"{FVIPGMRCVData[teFVIPGMReply.error_msg.ToString()]}");
                                smm.Next(teCmd.END_NG);
                                break;
                            }

                            string strEquipID = Program.iniMachine.GetString("Machine", "EquipID") ;
                            //Equip ID 체크
                            if (strEquipID  != FVIPGMRCVData[teFVIPGMReply.equip_id.ToString()])
                            {
                                GLog.Add("FVIPGMData - EquipID Error");
                                G.AlmMng.ADD(ALM.ERROR_SESCGEM_DATA, $"Cur EquipID ={strEquipID} , Data Lot ID = {FVIPGMRCVData[teFVIPGMReply.equip_id.ToString()]}") ;
                                smm.Next(teCmd.END_NG);
                                break;
                            }

                            //LotID 체크
                            if (SM.FMT[teFlowTray.LDLiftMax].LotID != FVIPGMRCVData[teFVIPGMReply.lot_id.ToString()])
                            {
                                GLog.Add("FVIPGMData - LOT ID Error");
                                G.AlmMng.ADD(ALM.ERROR_SESCGEM_DATA, $"Cur Lot ID ={SM.FMT[teFlowTray.LDLiftMax].LotID} , Data Lot ID = {FVIPGMRCVData[teFVIPGMReply.lot_id.ToString()]}") ;
                                smm.Next(teCmd.END_NG);
                                break;
                            }

                            //User Name 체크
                            if (SDm.Read((eSDM.Operator1 + NDm.Read(eNDM.SelectedOperatorIdx))) != FVIPGMRCVData[teFVIPGMReply.user_name.ToString()])
                            {
                                GLog.Add("FVIPGMData - UserID Error");
                                G.AlmMng.ADD(ALM.ERROR_SESCGEM_DATA, $"Cur User ID ={SDm.Read((eSDM.Operator1 + NDm.Read(eNDM.SelectedOperatorIdx)))} , Data User ID = {FVIPGMRCVData[teFVIPGMReply.user_name.ToString()]}") ;
                                smm.Next(teCmd.END_NG);
                                break;
                            }
                            
                            //랏 카운트 집어넣기
                            if (int.TryParse(FVIPGMRCVData[teFVIPGMReply.in_qty.ToString()], out int nLotCount) == true)
                            {
                                SM.FMT[teFlowTray.LDLiftMax].ProdMaxCount = nLotCount;
                            }
                            //Device 집어넣기
                            if ( FVIPGMRCVData[teFVIPGMReply.device.ToString()] != null)
                            {
                                SM.FMT[teFlowTray.LDLiftMax].Device = FVIPGMRCVData[teFVIPGMReply.device.ToString()];
                            }
                            //Mark3 집어넣기
                            if ( FVIPGMRCVData[teFVIPGMReply.mark_3.ToString()] != null)
                            {
                                SM.FMT[teFlowTray.LDLiftMax].Mark3 = FVIPGMRCVData[teFVIPGMReply.mark_3.ToString()];
                            }
                            //WorkWeek 집어넣기
                            if ( FVIPGMRCVData[teFVIPGMReply.work_week.ToString()] != null)
                            {
                                SM.FMT[teFlowTray.LDLiftMax].WorkWeek = FVIPGMRCVData[teFVIPGMReply.work_week.ToString()];
                            }
                            //oper 집어넣기
                            if ( FVIPGMRCVData[teFVIPGMReply.oper.ToString()] != null)
                            {
                                SDm.Write(eSDM.OPER,FVIPGMRCVData[teFVIPGMReply.oper.ToString()]);
                            }

                            m_bResult_FVIPGMReq = true;
							smm.Next(teCmd.START);
						}
						break;
					case teCmd.END_NG:
						{
							m_bResult_FVIPGMReq = false;
							smm.Next(teCmd.START);
						}
						break;
				}
			}
			catch (Exception ex)
            {
                G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
            }
		}

		private void	FVIStartReq(SMM<teCmd> smm)
		{
			try
			{
				switch(smm.CurStep)
				{
					case teCmd.START:
						{
							if (smm.eInput == InState.run)
							{
								smm.eInput = InState.none;
                                m_bResult_FVIStartReq = false; 
								smm.Next(teCmd.FVI_START_REQ_10);
							}
						}
						break;
					case teCmd.FVI_START_REQ_10:
						{						
                            m_nRetryDelay = NDm.Read(eNDM.SecsGemRetryDelay);
							G.commUbiGEM.SendFVIStartReq(SM.FMT[teFlowTray.LDLiftMax]);
							smm.Next(teCmd.FVI_START_REQ_20);
						}
						break;
					case teCmd.FVI_START_REQ_20:
						{
							if(G.commUbiGEM.RecvFVIStart(ref FVISTARTRCVData) <= -1)
							{ 
                                AddAlarm(ALM.ERROR_SESCGEM_DATA_LENGTH);
								smm.Next(teCmd.END_NG);
								break;
							}
							else if(G.commUbiGEM.RecvFVIStart(ref FVISTARTRCVData) == 0)
							{
								if(smm.Delayms(m_nRetryDelay))
								{
									smm.Next(teCmd.END_NG);
								}

								break;
							}

							smm.Next(teCmd.END_OK);
						}
						break;
					case teCmd.END_OK:
                        {
                            //check
                            for (int i = 0; i < (int)teFVISTARTReply.MAX; i++)
                            {
                                if (FVISTARTRCVData.TryGetValue(((teFVISTARTReply)i).ToString(), out string str) == false)
                                {
                                    GLog.Add($"FVISTARTData - TryGetValue {((teFVISTARTReply)i).ToString()}");
                                    AddAlarm(ALM.ERROR_SESCGEM_DATA);
                                    smm.Next(teCmd.END_NG);
                                    break;
                                }
                            }

                            //status_value 체크
                            if (FVISTARTRCVData[teFVISTARTReply.status_value.ToString()] != "0")
                            {
                                GLog.Add("teFVISTARTReply - Result Flag");
                                string strErr = FVISTARTRCVData[teFVISTARTReply.error_msg.ToString()];
                                if (strErr.Contains("This User is not exist"))
                                    G.AlmMng.ADD(ALM.ERROR_SESCGEM_USER_DATA);
                                else if(strErr.Contains("This LOT ID is not exist"))
                                    G.AlmMng.ADD(ALM.ERROR_SESCGEM_LOT_DATA);
                                else
                                    G.AlmMng.ADD(ALM.ERROR_SESCGEM_DATA, $"error_msg = {FVISTARTRCVData[teFVISTARTReply.error_msg.ToString()]}");
                                smm.Next(teCmd.END_NG);
                                break;
                            }
                            string strEquipID =  Program.iniMachine.GetString("Machine", "EquipID");
                            //EquipID 체크
                            if (strEquipID.Equals(FVISTARTRCVData[teFVISTARTReply.equip_id.ToString()]) == false)
                            {
                                GLog.Add("teFVISTARTReply - EquipID");
                                G.AlmMng.ADD(ALM.ERROR_SESCGEM_DATA, $"Cur EquipID = {strEquipID} Data EquipID = {FVISTARTRCVData[teFVISTARTReply.equip_id.ToString()]} ");
                                smm.Next(teCmd.END_NG);
                                break;
                            }
                            
                            //Lot ID 체크
                            if (SM.FMT[teFlowTray.LDLiftMax].LotID.Equals(FVISTARTRCVData[teFVISTARTReply.lot_id.ToString()]) == false)
                            {
                                GLog.Add("teFVISTARTReply - EquipID");
                                G.AlmMng.ADD(ALM.ERROR_SESCGEM_DATA, $"Cur Lot ID = {SM.FMT[teFlowTray.LDLiftMax].LotID} Data Lot ID = {FVISTARTRCVData[teFVISTARTReply.lot_id.ToString()]} ");
                                smm.Next(teCmd.END_NG);
                                break;
                            }

                            //user Name 체크
                            if (SDm.Read((eSDM.Operator1 + NDm.Read(eNDM.SelectedOperatorIdx))).Equals(FVISTARTRCVData[teFVISTARTReply.user_name.ToString()], StringComparison.CurrentCultureIgnoreCase) == false)
                            {
                                GLog.Add("teFVISTARTReply - UserID");
                                G.AlmMng.ADD(ALM.ERROR_SESCGEM_DATA, $"Cur UserID = {SDm.Read((eSDM.Operator1 + NDm.Read(eNDM.SelectedOperatorIdx)))} Data UserID = {FVISTARTRCVData[teFVISTARTReply.user_name.ToString()]} ");
                                smm.Next(teCmd.END_NG);
                                break;
                            }

                            m_bResult_FVIStartReq = true;
							smm.Next(teCmd.START);
						}
						break;
					case teCmd.END_NG:
						{
							m_bResult_FVIStartReq = false;
							smm.Next(teCmd.START);
						}
						break;
				}
			}
			catch (Exception ex)
            {
                G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
            }
		}

		private void	UPSerialReq(SMM<teCmd> smm)
		{
			switch (smm.CurStep)
			{
				case teCmd.START:
					{
						if (smm.eInput == InState.run)
						{
							smm.eInput = InState.none;
                            m_bResult_UPSerialReq = false; 
							smm.Next(teCmd.UP_SERIAL_NO_REQ_10);
						}
					}
					break;
				case teCmd.UP_SERIAL_NO_REQ_10:
					{
                        m_nRetryDelay = NDm.Read(eNDM.SecsGemRetryDelay);
						G.commUbiGEM.SendUPSerialNoReq(SM.FMP[teFlowPart.ULDBuffer]);
						smm.Next(teCmd.UP_SERIAL_NO_REQ_20);
					}
					break;
				case teCmd.UP_SERIAL_NO_REQ_20:
					{
						if (G.commUbiGEM.RecvUPSERIALNO(ref UPSERIALNORCVData) <= -1)
						{
                            AddAlarm(ALM.ERROR_SESCGEM_DATA_LENGTH);
							smm.Next(teCmd.END_NG);
							break;
						}
						else if (G.commUbiGEM.RecvUPSERIALNO(ref UPSERIALNORCVData) == 0)
						{
							if (smm.Delayms(m_nRetryDelay))
							{
								//Check: 알람 필요
								smm.Next(teCmd.END_NG);
							}

							break;
						}

						smm.Next(teCmd.END_OK);
					}
					break;
				case teCmd.END_OK:
					{
						bool bFlag = false;
                        //check
                        for (int i = 0; i < (int)teUPSERIALNOReply.MAX; i++)
                        {
                            if (UPSERIALNORCVData.TryGetValue(((teUPSERIALNOReply)i).ToString(), out string str) == false)
                            {
                                GLog.Add($"UPSERIALNOData - TryGetValue {((teUPSERIALNOReply)i).ToString()}");
                                AddAlarm(ALM.ERROR_SESCGEM_DATA);
                                smm.Next(teCmd.END_NG);
								bFlag= true;
                                break;
                            }
                        }

                        //status_value 체크
                        if (UPSERIALNORCVData[teUPSERIALNOReply.status_value.ToString()] != "0")
                        {
                            GLog.Add("UPSERIALNOData - status_value");
                            string strErr = UPSERIALNORCVData[teUPSERIALNOReply.error_msg.ToString()];
                            if (strErr.Contains("The same barcode exists in the barcode history"))
                                G.AlmMng.ADD(ALM.ERROR_SESCGEM_BARCODE_DATA);
                            else
                                G.AlmMng.ADD(ALM.ERROR_SESCGEM_DATA, $"error_msg= {strErr}");
                            smm.Next(teCmd.END_NG);
                            break;
                        }

                        //LotID 체크
                        if (SM.FMP[teFlowPart.ULDBuffer].LotID.Equals(UPSERIALNORCVData[teUPSERIALNOReply.lot_id.ToString()]) == false)
                        {
                            GLog.Add("UPSERIALNOData - status_value");
                            G.AlmMng.ADD(ALM.ERROR_SESCGEM_DATA, $"Cur Lot ID = {SM.FMP[teFlowPart.ULDBuffer].LotID} , Data Lot ID = {UPSERIALNORCVData[teUPSERIALNOReply.lot_id.ToString()]} ");
                            smm.Next(teCmd.END_NG);
                            break;
                        }

                        //User ID 체크
                        if (SDm.Read((eSDM.Operator1 + NDm.Read(eNDM.SelectedOperatorIdx))).Equals(UPSERIALNORCVData[teUPSERIALNOReply.user_name.ToString()]) == false)
                        {
                            GLog.Add("UPSERIALNOData - UserID");
                            G.AlmMng.ADD(ALM.ERROR_SESCGEM_DATA, $"Cur User ID = {SDm.Read((eSDM.Operator1 + NDm.Read(eNDM.SelectedOperatorIdx)))} , Data User ID = {UPSERIALNORCVData[teUPSERIALNOReply.user_name.ToString()]} ");
                            smm.Next(teCmd.END_NG);
                            break;
                        }
                        
                            string strEquipID =  Program.iniMachine.GetString("Machine", "EquipID");
                        //EquipID 체크
                        if (strEquipID.Equals(UPSERIALNORCVData[teUPSERIALNOReply.equip_id.ToString()]) == false)
                        {
                            GLog.Add("UPSERIALNOData - EquipID");
                            G.AlmMng.ADD(ALM.ERROR_SESCGEM_DATA, $"Cur equip ID = {strEquipID} , Data equip ID = {UPSERIALNORCVData[teUPSERIALNOReply.equip_id.ToString()]} ");
                            smm.Next(teCmd.END_NG);
                            break;
                        }

                        if (int.TryParse(UPSERIALNORCVData[teUPSERIALNOReply.result_sn_cnt.ToString()], out int nDataCnt) == false)
                        {
                            G.AlmMng.ADD(ALM.ERROR_SESCGEM_DATA, $"Cur result_sn_cnt Is not Integer");
                            smm.Next(teCmd.END_NG);
                            break;
                        }
                        //Prod Count 체크
                        else if (nDataCnt != SM.FMP[teFlowPart.ULDBuffer].GetCurCount())
                        {
                            GLog.Add("UPSERIALNOData - UserID");
                            G.AlmMng.ADD(ALM.ERROR_SESCGEM_DATA, $"Cur Prod Count = {SM.FMP[teFlowPart.ULDBuffer].GetCurCount()} , Data Prod Count= {nDataCnt} ");
                            smm.Next(teCmd.END_NG);
                            break;
                        }

                        for (int i = 0; i < SM.FMP[teFlowPart.ULDBuffer].GetCurCount(); i++)
                        {
                            if ( SM.FMP[teFlowPart.ULDBuffer].GetExist(i) == true)
                            {
							 	string strResult = "00";
								if (UPSERIALNORCVData[(teUPSERIALNOReply.result_sn1+i).ToString()].Count() > 2 )
							 		strResult = UPSERIALNORCVData[(teUPSERIALNOReply.result_sn1+i).ToString()].Substring(0,2);
								else
									 continue;
								if (strResult == "01") //server에서 검사 바코드 검증결과가 비정상이면 01 정상이면 00
								{ 
                                    AddAlarm(ALM.ERROR_SESCGEM_BARCODE_DATA);
									smm.Next(teCmd.END_NG);
									bFlag= true;
                                    break;
								}

                                if (UPSERIALNORCVData[(teUPSERIALNOReply.result_sn1+i).ToString()] != $"{strResult}{SM.FMP[teFlowPart.ULDBuffer].GetBarcode(i)}")
                                {
                                    GLog.Add("UPSERIALNOData - barcode");
                                    G.AlmMng.ADD(ALM.ERROR_SESCGEM_DATA, $"Cur Prod Barcode({i}) = {SM.FMP[teFlowPart.ULDBuffer].GetBarcode(i)} , Data  Prod Barcode({i}) = {UPSERIALNORCVData[(teUPSERIALNOReply.result_sn1+i).ToString()]} ");
                                    smm.Next(teCmd.END_NG);
									bFlag= true;
                                    break;
                                }
                            }
                        }

						if (bFlag == true)
							break;

                        m_bResult_UPSerialReq = true;
						smm.Next(teCmd.START);
					}
					break;
				case teCmd.END_NG:
					{
						m_bResult_UPSerialReq = false;
						smm.Next(teCmd.START);
					}
					break;
			}
		}

		private void	ErrorReq(SMM<teCmd> smm)
		{
			switch (smm.CurStep)
			{
				case teCmd.START:
					{
						if (smm.eInput == InState.run)
						{
							smm.eInput = InState.none;
                            m_bResult_ErrorReq = false; 
							smm.Next(teCmd.ERROR_REQ_10);
						}
					}
					break;
				case teCmd.ERROR_REQ_10:
					{
						if (SDm.Read(eSDM.LotID1) == "")
						{
							// NG로 이동
							smm.Next(teCmd.END_NG);
							break;
						}
                        
                        m_nRetryDelay = NDm.Read(eNDM.SecsGemRetryDelay);
						G.commUbiGEM.SendErrorReq();
						smm.Next(teCmd.UP_SERIAL_NO_REQ_20);
					}
					break;
				case teCmd.UP_SERIAL_NO_REQ_20:
					{
						if (G.commUbiGEM.RecvERROR(ref ERRORRCVData) <= -1)
						{
                            AddAlarm(ALM.ERROR_SESCGEM_DATA_LENGTH);
							smm.Next(teCmd.END_NG);
							break;
						}
						else if (G.commUbiGEM.RecvERROR(ref ERRORRCVData) == 0)
						{
							if (smm.Delayms(m_nRetryDelay))
							{
								//Check: 알람 필요
								smm.Next(teCmd.END_NG);
							}

							break;
						}

						smm.Next(teCmd.END_OK);
					}
					break;
				case teCmd.END_OK:
					{//Status ng이면 error msg로 알람을 띄워준다.
						m_bResult_ErrorReq= true;
						smm.Next(teCmd.START);
					}
					break;
				case teCmd.END_NG:
					{
						m_bResult_ErrorReq = false;
						smm.Next(teCmd.START);
					}
					break;
			}
		}

		private void	EQMAINTReq(SMM<teCmd> smm)
		{
			switch (smm.CurStep)
			{
				case teCmd.START:
					{
						if (smm.eInput == InState.run)
						{
							smm.eInput = InState.none;
                            m_bResult_EQMAINTReq = false; 
							smm.Next(teCmd.EQ_MAINT_REQ_10);
						}
					}
					break;
				case teCmd.EQ_MAINT_REQ_10:
					{
                        m_nRetryDelay = NDm.Read(eNDM.SecsGemRetryDelay);
						//Check: 예시임 차후 해당 부분에 맞게 입력 받아야 함
						G.commUbiGEM.SendEQMaintReq(EQMAINTREQ_SendData, m_ModelName, m_LotID);
						smm.Next(teCmd.EQ_MAINT_REQ_20);
					}
					break;
				case teCmd.EQ_MAINT_REQ_20:
					{
						if (G.commUbiGEM.RecvEQMAINT(ref EQMAINTRCVData) <= -1)
						{
                            AddAlarm(ALM.ERROR_SESCGEM_DATA_LENGTH);
							smm.Next(teCmd.END_NG);
							break;
						}
						else if (G.commUbiGEM.RecvEQMAINT(ref EQMAINTRCVData) == 0)
						{
							if (smm.Delayms(m_nRetryDelay))
							{
								//Check: 알람 필요
								smm.Next(teCmd.END_NG);
							}

							break;
						}

						smm.Next(teCmd.END_OK);
					}
					break;
				case teCmd.END_OK:
					{
                        //check
                        bool bResult = true;
                        for (int i = 0; i < (int)teMAINTReply.MAX; i++)
                        {
                            if (EQMAINTRCVData.TryGetValue( EnumHelper.GetDescription((teMAINTReply)i), out string str) == false)
                            {
                                GLog.Add($"EQMAINTRCVData - TryGetValue {((teMAINTReply)i).ToString()}");
                                AddAlarm(ALM.ERROR_SESCGEM_DATA);
                                smm.Next(teCmd.END_NG);
                                bResult = false;
                                break;
                            }
                        }
                        if (bResult == false)   break;

                        //Result Flag 체크, 신고나 접수여부는 해당 데이터를 실제로 사용하기 때문에 아래에서 확인함.
                        if (EQMAINTREQ_SendData[(int)teMainternanceValiable.event2] != teEVENT2.REPORTSTATUS.ToString() &&
                            EQMAINTREQ_SendData[(int)teMainternanceValiable.event2] != teEVENT2.REGISTERSTATUS.ToString()&&
                            EQMAINTRCVData[teMAINTReply.result_flag.ToString()] != "OK")
                        {
                            GLog.Add("teMAINTReply - Result Flag");
                            G.AlmMng.ADD(ALM.ERROR_SESCGEM_DATA, $"Result Flag = {EQMAINTRCVData[teMAINTReply.result_flag.ToString()]}");
                            smm.Next(teCmd.END_NG);
                            break;
                        }
                        string strEquipID =  Program.iniMachine.GetString("Machine", "EquipID");
                        //EquipID 체크 j
                        if (strEquipID.Equals(EQMAINTRCVData[teMAINTReply.equip_id.ToString()]) == false)
                        {
                            GLog.Add("teMAINTReply - EquipID");
                            G.AlmMng.ADD(ALM.ERROR_SESCGEM_DATA, $"Cur EquipID = {strEquipID} , Data ID = {EQMAINTRCVData[teMAINTReply.equip_id.ToString()]}");
                            smm.Next(teCmd.END_NG);
                            break;
                        }

                        //리포트 창에서 데이터를 보낸경우
                        if (UI.ModalReport.IsOpen())
                        {
                            UI.ModalReport.bSendComplete = true;
                            //신고여부를 보낸 경우
                            if (EQMAINTREQ_SendData[(int)teMainternanceValiable.event2] == teEVENT2.REPORTSTATUS.ToString())
                            {
                                if (EQMAINTRCVData[teMAINTReply.result_flag.ToString()] == "OK")
                                {
                                    UI.ModalReport.bRepairCheck = true;
                                }
                                else
                                {	
                                    UI.ModalReport.showMsg("Already RepairCheck");
                                    UI.ModalReport.bRepairCheck = false;
                                }
                            }
                            //접수여부를 보낸 경우
                            else if (EQMAINTREQ_SendData[(int)teMainternanceValiable.event2] == teEVENT2.REGISTERSTATUS.ToString())
                            {
                                if (EQMAINTRCVData[teMAINTReply.result_flag.ToString()] == "OK")
                                {
                                    UI.ModalReport.bReceiptCheck =true ;
                                }
                                else
								{ 
                                    UI.ModalReport.showMsg("Already ReceiptCheck");
                                    UI.ModalReport.bReceiptCheck = false;
								}
                            }
                            else
                                UI.ModalReport.showMsg("Report Success");
                        }

                        if (EQMAINTREQ_SendData[(int)teMainternanceValiable.event2] == teEVENT2.PM.ToString() ||
                            EQMAINTREQ_SendData[(int)teMainternanceValiable.event2] == teEVENT2.USEENGR.ToString() ||
                            EQMAINTREQ_SendData[(int)teMainternanceValiable.event2] == teEVENT2.NODEVICE.ToString() ||
                            EQMAINTREQ_SendData[(int)teMainternanceValiable.event2] == teEVENT2.EQUIPMENTCHECK.ToString() ||
                            EQMAINTREQ_SendData[(int)teMainternanceValiable.event2] == teEVENT2.CONVERSION.ToString() ||
                            EQMAINTREQ_SendData[(int)teMainternanceValiable.event2] == teEVENT2.REPAIRECHECK.ToString() ||
                            EQMAINTREQ_SendData[(int)teMainternanceValiable.event2] == teEVENT2.REGISTERREPORT.ToString())
                        {
                            SM.SHM.SetBitData(BIT.UBIGEMCheckingEquipment, true);
                            if (string.IsNullOrEmpty(EQMAINTREQ_SendData[(int)teMainternanceValiable.event4]) == true)
                                SDm.Write(eSDM.ReportCause , EQMAINTREQ_SendData[(int)teMainternanceValiable.event2]);
                            else
                                SDm.Write(eSDM.ReportCause , EQMAINTREQ_SendData[(int)teMainternanceValiable.event4]);
                        }
                        else if (EQMAINTREQ_SendData[(int)teMainternanceValiable.event2] == teEVENT2.CHECKCOMPLETED.ToString())
                        {
                            SM.SHM.SetBitData(BIT.UBIGEMCheckingEquipment, false);
                            SDm.Write(eSDM.ReportCause,"");
                            SM.SHM.SetBitData(BIT.Buzzer, false);
                            G.AlmMng.Clear();
						    G.sysErr.Start();
                        }


                        m_bResult_EQMAINTReq = true;
						smm.Next(teCmd.START);
					}
					break;
				case teCmd.END_NG:
					{
                        if (UI.ModalReport.IsOpen())
                        {
                            UI.ModalReport.bSendComplete = true;
                            if (EQMAINTREQ_SendData[(int)teMainternanceValiable.event2] == teEVENT2.REPORTSTATUS.ToString())
                                UI.ModalReport.bRepairCheck = false;
                            else if (EQMAINTREQ_SendData[(int)teMainternanceValiable.event2] == teEVENT2.REGISTERSTATUS.ToString())
                                UI.ModalReport.bReceiptCheck = false;
                            else
                                UI.ModalReport.showMsg("Report Fail...Please Retry");
                        }
                        m_bResult_EQMAINTReq = false;
						smm.Next(teCmd.START);
					}
					break;
			}
		}




	}	
}						
						
						