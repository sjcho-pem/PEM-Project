using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_MarsSSD
{
	public class SeqUbiGEMComm: SeqBase<SeqUbiGEMComm.teAct, SeqUbiGEMComm.teAxis, SeqUbiGEMComm.teCmd>
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
			END_OK	= 97,
			END_NG	= 98,
			END		= 99,
			
			TRAY_VALIDATION_10		= 100,
            TRAY_VALIDATION_20,
            TRAY_VALIDATION_30,
            TRAY_VALIDATION_40,

			ALARM_SET_10		= 200,
            ALARM_SET_20,
            ALARM_SET_30,
            ALARM_SET_40,
        }

        #endregion

		  #region [Variables]
		private SMM<teCmd> m_smmTrayValidation		= new SMM<teCmd>(teCmd.START, EnumHelper.GetDescription(SeqID.UBIGEM_TRAY_VALIDATION));
		private SMM<teCmd> m_smmAlarmSet			= new SMM<teCmd>(teCmd.START, EnumHelper.GetDescription(SeqID.UBIGEM_ALARM_SET));
		
		public teCmd smmTrayValidation		{ get => m_smmTrayValidation.CurStep; }
		public teCmd smmAlarmSet			{ get => m_smmAlarmSet.CurStep; }

		private bool	m_bResult_TrayValidation		= false;
		private bool	m_bResult_AlarmSet				= false;
        //호출 전 초기화 필요
		public bool		Result_TrayValidation		{ get => m_bResult_TrayValidation;	set => m_bResult_TrayValidation = value;  }
		public bool		Result_AlarmSet				{ get => m_bResult_AlarmSet;		set => m_bResult_AlarmSet = value;}
        public	bool	IsTrayValidationReady()		{ return m_smmTrayValidation.IsReady; }
		public	bool	IsAlarmSetReady()			{ return m_smmAlarmSet.IsReady; }
		
		public Dictionary<string, string>	FVITrayValidationData	= new Dictionary<string, string>();
		public Dictionary<string, string>	FVIAlarmSetData			= new Dictionary<string, string>();
		
		private	FlowTray	m_TrayFlow				= FlowTray.Empty;
		private int			m_SgemTrayRetryCount	= 0;
		private int			m_SgemAlarmRetryCount	= 0;
		private ALM			m_AlarmCode				=  ALM.UNKNOWN;
		#endregion


		public void Reset()
		{
			m_bResult_TrayValidation = false;
			m_bResult_AlarmSet = true;
		}

		//시퀀스 진행중 정지 시 초기화 하고 재시작할 수 있도록  수정
		public void Stop()
		{
			ResetStep();
			Reset();
		}

		private bool SetSeqRun(ref SMM<teCmd> smm)
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

		public	void	Init()
        {
            try
            {
                lstSMM.Add(m_smmTrayValidation);
                lstSMM.Add(m_smmAlarmSet);
                #region - Motor
                arrMt = new EnumArray<teAxis, CMotor>(teAxis.Max);
                #endregion

                #region - Actuator            
                arrAct = new EnumArray<teAct, Actuator>(teAct.Max);
                #endregion
                if (Program.iniMachine.GetString("Machine", "EquipID") == "" )              Program.iniMachine.SetString("Machine", "EquipID", "temp_EquipID");
                if (Program.iniMachine.GetString("Machine", "EquipModel") == "" )           Program.iniMachine.SetString("Machine", "EquipModel", "temp_EquipModel");
				
				Reset();

                base.Initialize();
            }
            catch (Exception ex)
            {
                G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
            }
        }

		public void Run()
		{
			try
			{
				SENDTRAYVALIDATION(m_smmTrayValidation);
				SENDALARMSET(m_smmAlarmSet);

				SM.SHM.SetSeqStatus(lstSMM);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public void		TrayValidationStart(FlowTray _TrayFlow)
		{
			if (SM.SHM.GetUseSkipData(USESKIP.SGEM) == false)
				return;

			if (SM.SHM.IsBitData(BIT.UbiGemConnect) == false)
				return;

			if (m_smmTrayValidation.IsReady == false)
				return;
			
			m_TrayFlow = _TrayFlow;
			m_SgemTrayRetryCount = 0;
			SetSeqRun(ref m_smmTrayValidation);
		}

		public void		AlarmSetStart(ALM _ALM)
		{
			if (SM.SHM.GetUseSkipData(USESKIP.SGEM) == false)
				return;

			if (SM.SHM.IsBitData(BIT.UbiGemConnect) == false)
				return;
			
			if (m_smmAlarmSet.IsReady == false)
				return;

			m_AlarmCode = _ALM;
			m_SgemTrayRetryCount = 0;
			SetSeqRun(ref m_smmAlarmSet);
		}

		private void SENDTRAYVALIDATION(SMM<teCmd> smm)
		{
			try
			{
				switch (smm.CurStep)
				{
					case teCmd.START:
						{
							if (smm.eInput == InState.run)
							{
								smm.eInput = InState.none;
								m_bResult_TrayValidation = false;
								m_SgemTrayRetryCount = 0;
								smm.Next(teCmd.TRAY_VALIDATION_10);
							}
						}
						break;
					case teCmd.TRAY_VALIDATION_10:
						{
							if (SM.SHM.GetUseSkipData(USESKIP.SGEM) == false)
							{
								smm.Next(teCmd.END_NG);
								break;
							}

							m_SgemTrayRetryCount ++;
							if (m_SgemTrayRetryCount >= 3)
							{
								//워닝
								AddAlarm(ALM.WARNING_SESCGEM_VALIDATION_ACK_ERROR);
								smm.Next(teCmd.TRAY_VALIDATION_30);
								break;
							}
							G.commUbiGEM.SendTrayValidation(m_TrayFlow);
							smm.Next(teCmd.TRAY_VALIDATION_20);
						}
						break;
					case teCmd.TRAY_VALIDATION_20:
						{
							if (SM.SHM.GetUseSkipData(USESKIP.SGEM) == false)
							{
								smm.Next(teCmd.END_NG);
								break;
							}
							
							int nAck = -1;
							int nret = G.commUbiGEM.RecvAck6(REQ.TRAY_VALIDATION, ref nAck);
							if (nret > 0)
							{
								//ack - Accepted
								if (nAck == 0)
								{
									smm.Next(teCmd.END_OK);
								}
								else//ack - ErrorNotAccepted
								{
									smm.Next(teCmd.TRAY_VALIDATION_10);
								}
							}
							else if (smm.Delayms(NDm.Read(eNDM.SecsGemCommTimeOut)) == true)
							{
								//AddAlarm(ALM.ERROR_SESCGEM_TIMEOUT);
								smm.Next(teCmd.TRAY_VALIDATION_10);
							}
						}
						break;
					case teCmd.TRAY_VALIDATION_30:
						{
							if (SM.SHM.GetUseSkipData(USESKIP.SGEM) == false )
							{
								smm.Next(teCmd.END_NG);
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
									m_SgemTrayRetryCount = 0;
									smm.Next(teCmd.TRAY_VALIDATION_10);
									break;
							}
						}
						break;
					case teCmd.END_NG:
						{
							m_bResult_TrayValidation = false;
							smm.Next(teCmd.START);
						}
						break;
					case teCmd.END_OK:
						{
							m_bResult_TrayValidation = true;
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
		
		private void SENDALARMSET(SMM<teCmd> smm)
		{
			try
			{
				switch (smm.CurStep)
				{
					case teCmd.START:
						{
							if (smm.eInput == InState.run)
							{
								smm.eInput = InState.none;
								m_SgemAlarmRetryCount = 0;
								m_bResult_AlarmSet = false;
								smm.Next(teCmd.ALARM_SET_10);
							}
						}
						break;
					case teCmd.ALARM_SET_10:
						{
								if (SM.SHM.GetUseSkipData(USESKIP.SGEM) == false)
							{
								smm.Next(teCmd.END_NG);
								break;
							}

							m_SgemTrayRetryCount ++;
							if (m_SgemTrayRetryCount > 3)
							{
								AddAlarm(ALM.ERROR_SESCGEM_ALARM_SET_ACK_ERROR);
								smm.Next(teCmd.ALARM_SET_30);
								break;
							}
							G.commUbiGEM.SendAlarmSet(m_AlarmCode);
							smm.Next(teCmd.ALARM_SET_20);
						}
						break;
					case teCmd.ALARM_SET_20:
						{
							if (SM.SHM.GetUseSkipData(USESKIP.SGEM) == false)
							{
								smm.Next(teCmd.END_NG);
								break;
							}
							
							int nAck = -1;
							int nret = G.commUbiGEM.RecvAck5(REQ.AlarmSendReport, ref nAck);
							if (nret > 0)
							{
								//ack - Accepted
								if (nAck == 0)
								{
									smm.Next(teCmd.END_OK);
								}
								else//ack - ErrorNotAccepted
								{
									smm.Next(teCmd.ALARM_SET_10);
								}
							}
							else if (smm.Delayms(NDm.Read(eNDM.SecsGemCommTimeOut)) == true)
							{
								AddAlarm(ALM.ERROR_SESCGEM_TIMEOUT);
								smm.Next(teCmd.ALARM_SET_10);
							}
						}
						break;
					case teCmd.ALARM_SET_30:
						{
							if (SM.SHM.GetUseSkipData(USESKIP.SGEM) == false )
							{
								smm.Next(teCmd.END_NG);
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
									m_SgemTrayRetryCount = 0;
									smm.Next(teCmd.ALARM_SET_10);
									break;
							}
						}
						break;
					case teCmd.END_NG:
						{
							m_bResult_AlarmSet = false;
							smm.Next(teCmd.START);
						}
						break;
					case teCmd.END_OK:
						{
							NDm.Write(eNDM.RecentlyAlarmCode , (int)m_AlarmCode);
							m_bResult_AlarmSet = true;
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

	}
}
