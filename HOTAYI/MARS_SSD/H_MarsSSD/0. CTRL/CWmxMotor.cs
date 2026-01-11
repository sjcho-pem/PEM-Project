using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if _WMX3_
using WMX3ApiCLR;

namespace H_MarsSSD
{
    public class CWmxMotor : CMotor
    {
        #region -Variables, Events        
        private Motion.PosCommand m_PosCom = new Motion.PosCommand();
        private Motion.JogCommand m_JogCom = new Motion.JogCommand();
        private Motion.TriggerPosCommand m_TPosCom = new Motion.TriggerPosCommand();
        private Motion.LinearIntplCommand m_LCom = new Motion.LinearIntplCommand();
        #endregion

        public CWmxMotor(int _nAxis)
        {
            nAxis = _nAxis;

            MotionCommand_Init();
        }

        #region [ Motion Functions ]
        private void MotionCommand_Init()
        {
            m_PosCom.Profile.Type = WMX3ApiCLR.ProfileType.JerkRatio;
            m_PosCom.Axis = nAxis;
            m_PosCom.Target = 0;
            m_PosCom.Profile.Velocity = Vel;
            m_PosCom.Profile.StartingVelocity = 0;
            m_PosCom.Profile.EndVelocity = 0;
            m_PosCom.Profile.Acc = Acc;
            m_PosCom.Profile.Dec = Dec;
            m_PosCom.Profile.JerkAcc = 1000;
            m_PosCom.Profile.JerkAccRatio = 0.5;
            m_PosCom.Profile.JerkDec = 1000;
            m_PosCom.Profile.JerkDecRatio = 0.5;

            m_JogCom.Axis = nAxis;
            m_JogCom.Profile.Type = WMX3ApiCLR.ProfileType.JerkRatio;
            m_JogCom.Profile.Velocity = Vel;
            m_JogCom.Profile.Acc = Acc;
            m_JogCom.Profile.Dec = Dec;
            m_JogCom.Profile.JerkAcc = 1000;
            m_JogCom.Profile.JerkAccRatio = 0.5;
            m_JogCom.Profile.JerkDec = 1000;
            m_JogCom.Profile.JerkDecRatio = 0.5;

            m_TPosCom.Profile.Type = WMX3ApiCLR.ProfileType.Trapezoidal;
            m_TPosCom.Axis = nAxis;
            m_TPosCom.Target = 0;
            m_TPosCom.Profile.Velocity = Vel;
            //m_TPosCom.Profile.StartingVelocity = 0;
            //m_TPosCom.Profile.EndVelocity = 0;
            m_TPosCom.Profile.Acc = Acc;
            m_TPosCom.Profile.Dec = Dec;
            //m_TPosCom.Profile.JerkAcc = 1000;
            //m_TPosCom.Profile.JerkAccRatio = 0.5;
            //m_TPosCom.Profile.JerkDec = 1000;
            //m_TPosCom.Profile.JerkDecRatio = 0.5;

            //m_LCom.AxisCount = 2;
            //m_LCom.Axis[0] = nAxis;
            
            //m_LCom.Profile.Type = ProfileType.Trapezoidal;
            //m_LCom.Profile.Acc = 10000;
            //m_LCom.Profile.Dec = 10000;

            //m_LCom.Target[0] = 30000;
            //m_LCom.Target[1] = 10000;

            //m_LCom.MaxVelocity[0] = 6000;
        }

        private void SetMovePos(float fTarget)
        {
            m_PosCom.Target = fTarget;            

            SetTargetPos(fTarget);
        }

        private void SetMoveSpeed(int nVel, int nAcc, int nDec, bool bDir = true)
        {
            if (bDir == false)
                nVel *= -1;

            if (Math.Abs(nVel) > 100)
                m_PosCom.Profile.Velocity = nVel * VelPercent;
            else
                m_PosCom.Profile.Velocity = nVel;

            m_PosCom.Profile.Acc = nAcc;
            m_PosCom.Profile.Dec = nDec;
        }

        private void SetJogSpeed(int nVel, int nAcc, int nDec, bool bDir = true)
        {
            if (bDir == false)
                nVel *= -1;

            if (Math.Abs(nVel) > 100)
                m_JogCom.Profile.Velocity = nVel * VelPercent;
            else
                m_JogCom.Profile.Velocity = nVel;

            m_JogCom.Profile.Acc = nAcc;
            m_JogCom.Profile.Dec = nDec;
        }

		private	int GetMoveSpeed()
		{
			return (int)m_PosCom.Profile.Velocity;
		}
        #endregion

        #region 모터 행동 함수
        public override void Run()
        {
            TorqueParam();
            ReadBitStatus();
            ErrorProc();
        }

        private void TorqueParam()
        {
            //CWMX3.CM.Config.GetAxisParam(ref CWMX3.axParam);
        }

        private void ReadBitStatus()
        {
            m_stMt.Alarm = CWMX3.cmStatus.AxesStatus[nAxis].AmpAlarm;
            m_stMt.AlarmCode = CWMX3.cmStatus.AxesStatus[nAxis].AmpAlarmCode;

            m_stMt.ActTorque = (int)(Math.Truncate(CWMX3.cmStatus.AxesStatus[nAxis].ActualTorque * 1000));

            m_stMt.CmdPos = (int)(Math.Truncate(CWMX3.cmStatus.AxesStatus[nAxis].PosCmd));
            m_stMt.ActPos = (int)(Math.Truncate(CWMX3.cmStatus.AxesStatus[nAxis].ActualPos));
            m_stMt.ActVel = (int)(Math.Truncate(CWMX3.cmStatus.AxesStatus[nAxis].ActualVelocity));
            m_stMt.CurOpState = (OpState)CWMX3.cmStatus.AxesStatus[nAxis].OpState;
            m_stMt.ServoOn = CWMX3.cmStatus.AxesStatus[nAxis].ServoOn;
            m_stMt.HomeDone = CWMX3.cmStatus.AxesStatus[nAxis].HomeDone;
            

            switch (CWMX3.cmStatus.AxesStatus[nAxis].HomeState)
            {
                case WMX3ApiCLR.HomeState.Idle:
                    m_stMt.eHomeState = HomeState.Idle;
                    break;
                default:
                    m_stMt.eHomeState = HomeState.HomeSearch;
                    break;
            }

            m_stMt.PosSet = CWMX3.cmStatus.AxesStatus[nAxis].PosSet;
            m_stMt.CmdDistributionEnd = CWMX3.cmStatus.AxesStatus[nAxis].CmdDistributionEnd;
            m_stMt.Inpos = CWMX3.cmStatus.AxesStatus[nAxis].InPos;

            m_stMt.NegLSen = CWMX3.cmStatus.AxesStatus[nAxis].NegativeLS;
            m_stMt.PosLSen = CWMX3.cmStatus.AxesStatus[nAxis].PositiveLS;
            m_stMt.HomeSen = CWMX3.cmStatus.AxesStatus[nAxis].HomeSwitch;                      
			

			if (nAxis == (int)MT.LDTable1Z  ||
				nAxis == (int)MT.LDTable2Z ||
				nAxis == (int)MT.ULDTable1Z ||
				nAxis == (int)MT.ULDTable2Z )
			{
				SetRdy(IsSVON() && !IsAlarm() && IsInPos() && !IsCommand());
				if (IsCommand() )
				{
					if (CheckCommandTimeOut() && !IsInPos()) // !IsPSET() 대체가능한지 확인 필요
					{
						OnAddAlarmEvent((int)ALM.ERROR_MT_MOVE_TIME_OUT_START + nAxis + 1);
					}

					if (Math.Abs(GetLocalPos() - GetTargetPos()) > InposRange)
					{
						return;
					}
					else
					{
						OffCommand();
						SetNowIndex(GetCmdIndex());
					}
				}
			}
			else
			{
				SetRdy(IsSVON() && !IsAlarm() && IsHomeDone() && IsInPos() && !IsCommand());

				if (IsCommand() && IsHomeDone())
				{
					if (CheckCommandTimeOut() && !IsInPos()) // !IsPSET() 대체가능한지 확인 필요
					{
						OnAddAlarmEvent((int)ALM.ERROR_MT_MOVE_TIME_OUT_START + nAxis + 1);
					}

					if (Math.Abs(GetLocalPos() - GetTargetPos()) > InposRange)
					{
						return;
					}
					else
					{
						OffCommand();
						SetNowIndex(GetCmdIndex());
					}
				}
			}
        }

        void ErrorProc()
        {
            if (IsAlarm())
			{
				SetCmdIndex(300);
				SetNowIndex(300);
				OnAddAlarmEvent((int)ALM.ERROR_MT_DRIVER_START + nAxis + 1);
			}

			if (!IsSVON())
                OnAddAlarmEvent((int)ALM.ERROR_MT_POWER_START + nAxis + 1);

            if ((IsPosLSen() || IsNegLSen()) && IsHomeDone() && IsCommand())
                OnAddAlarmEvent((int)ALM.ERROR_MT_LIMIT_START + nAxis + 1);

            if (IsPosLSen() && IsNegLSen())
                OnAddAlarmEvent((int)ALM.ERROR_MT_LIMIT_START + nAxis + 1);
        }

        public override bool SetServoOn(bool bOn)
        {
            int nOn = 1;

            if (!bOn)
            {
                nOn = 0;
            }

            CWMX3.CheckMotionErr(CWMX3.CM.AxisControl.SetServoOn(nAxis, nOn), nAxis);

            return true;
        }

        public override bool AlarmReset()
        {
            CWMX3.CM.AxisControl.ClearAmpAlarm(nAxis);

            return true;
        }

        public override bool Stop()
        {
            CWMX3.CheckMotionErr(CWMX3.CM.Motion.Stop(nAxis), nAxis);

            OffCommand();
            SetCmdIndex(300);
            SetNowIndex(300);

            return true;
        }

        public override bool EStop()
        {
            //CWMX3.CheckMotionErr(CWMX3.CM.Motion.ExecQuickStop(nAxis), nAxis);
            CWMX3.CheckMotionErr(CWMX3.CM.Motion.Stop(nAxis, GetVel((int)VELOCITY.DEC) * 10), nAxis);
            OffCommand();

            return true;
        }

        public override bool TorqueStop()
        {
            CWMX3.CheckMotionErr(CWMX3.CM.Torque.StopTrq(nAxis), nAxis);
            return true;
        }

        public override void ClearHome()
        {
            SetCmdIndex(300);
            SetNowIndex(300);

            CWMX3.CheckMotionErr(CWMX3.CM.Home.SetHomeDone(nAxis, 0) , nAxis);
        }

        public override void StartHoming()
        {
            SetCmdIndex(300);
            SetNowIndex(300);
            SetMovePos(0);

            Config.HomeParam homeParam = new Config.HomeParam();
            CWMX3.CM.Config.GetHomeParam(nAxis, ref homeParam);
            int FastVel = GetVel((int)VELOCITY.HomeSearch);
            homeParam.HomingVelocityFast = FastVel;
            homeParam.HomingVelocityFastAcc = FastVel * 10;
            homeParam.HomingVelocityFastDec = FastVel * 10;

            int SlowVel = GetVel((int)VELOCITY.HomeFind);
            homeParam.HomingVelocitySlow = SlowVel;
            homeParam.HomingVelocitySlowAcc = SlowVel * 10;
            homeParam.HomingVelocitySlowDec = SlowVel * 10;

            CWMX3.CM.Config.SetHomeParam(nAxis, homeParam);

            // Home 진행할때마다 설정값 넣어줘야하는지 테스트 필요.
            CWMX3.CheckMotionErr(CWMX3.CM.Home.StartHome(nAxis), nAxis); 
        }

        public override void MoveJog(bool dir, int nVel = 0)
        {
            if (nVel == 0)
                nVel = GetVel((int)VELOCITY.JOG);

            SetJogSpeed(nVel, GetVel((int)VELOCITY.ACC), GetVel((int)VELOCITY.DEC), dir);
            SetCmdIndex(300);

            CWMX3.CheckMotionErr(CWMX3.CM.Motion.StartJog(m_JogCom), nAxis);            
        }

        public override void MoveREL(int nDist, int nVel = 0)
        {
            if (nVel == 0)
                nVel = GetVel((int)VELOCITY.JOG);

            SetMovePos(GetLocalPos() + nDist);
            SetMoveSpeed(nVel, GetVel((int)VELOCITY.ACC), GetVel((int)VELOCITY.DEC));
            SetCmdIndex(300);

            OnCommand(m_PosCom.Profile.Velocity);

            CWMX3.CheckMotionErr(CWMX3.CM.Motion.StartPos(m_PosCom), nAxis);
        }

        public override void MoveABS(int nPos)
        {
            if (!IsRdy() || IsCommand() || GetLocalPos() == nPos)
                return;

            SetMovePos(nPos);
            SetMoveSpeed(GetVel((int)VELOCITY.JOG), GetVel((int)VELOCITY.ACC), GetVel((int)VELOCITY.DEC));
            SetCmdIndex(300);

            OnCommand(m_PosCom.Profile.Velocity);

            CWMX3.CheckMotionErr(CWMX3.CM.Motion.StartPos(m_PosCom), nAxis);
        }

		public override void MoveABS(int nPos, int nIdxVel)
        {
            if (!IsRdy() || IsCommand() || GetLocalPos() == nPos)
                return;

            SetMovePos(nPos);
            SetMoveSpeed(GetVel(nIdxVel), GetVel((int)VELOCITY.ACC), GetVel((int)VELOCITY.DEC));
            SetCmdIndex(300);

            OnCommand(m_PosCom.Profile.Velocity);

            CWMX3.CheckMotionErr(CWMX3.CM.Motion.StartPos(m_PosCom), nAxis);
        }

        public override void MoveIndex(int eNum)
        {
            if (!IsRdy() || IsCommand() || GetNowIndex() == eNum)
                return;

            SetMovePos(GetIndexPos(eNum));
            SetMoveSpeed(GetVel(eNum), GetVel((int)VELOCITY.ACC), GetVel((int)VELOCITY.DEC));
            SetCmdIndex(eNum);

            OnCommand(m_PosCom.Profile.Velocity);

            CWMX3.CheckMotionErr(CWMX3.CM.Motion.StartPos(m_PosCom), nAxis);
        }

		public override void MoveIndex(int eNum, int nIdxVel)
		{
            if (!IsRdy() || IsCommand() || GetNowIndex() == eNum)
                return;

            SetMovePos(GetIndexPos(eNum));
            SetMoveSpeed(GetVel(nIdxVel), GetVel((int)VELOCITY.ACC), GetVel((int)VELOCITY.DEC));
            SetCmdIndex(eNum);

            OnCommand(m_PosCom.Profile.Velocity);

            CWMX3.CheckMotionErr(CWMX3.CM.Motion.StartPos(m_PosCom), nAxis);
        }

        public override void TMoveIndex(int eNum, int nTMt, int nRemainDist, int nSecondVel)
        {
            //SetMovePos(GetIndexPos(nPos));
            //SetMoveSpeed(GetVel(nPos), GetVel((int)VELOCITY.ACC), GetVel((int)VELOCITY.DEC));
            m_TPosCom.Target = GetIndexPos(eNum);

            m_TPosCom.Trigger.TriggerType = TriggerType.RemainingDistance;
            m_TPosCom.Trigger.TriggerAxis = nTMt;
            m_TPosCom.Trigger.TriggerValue = nRemainDist;

            if (Math.Abs(nSecondVel) > 100)
                m_TPosCom.Profile.Velocity = nSecondVel * VelPercent;
            else
                m_TPosCom.Profile.Velocity = nSecondVel;

            m_TPosCom.Profile.Acc		= GetVel((int)VELOCITY.ACC);
			m_TPosCom.Profile.Dec		= GetVel((int)VELOCITY.DEC);
            SetCmdIndex(eNum);

            OnCommand(m_TPosCom.Profile.Velocity);

            CWMX3.CheckMotionErr(CWMX3.CM.Motion.StartPos(m_TPosCom), nAxis);
        }

        public override void MoveTorque(int eNum)
        {
            Torque.TrqCommand m_TrqCmd = new Torque.TrqCommand();
            m_TrqCmd.Axis = nAxis;
            m_TrqCmd.Torque = GetVel(eNum);

            CWMX3.CheckMotionErr(CWMX3.CM.AxisControl.SetAxisCommandMode(nAxis, AxisCommandMode.Torque), nAxis);
            CWMX3.CheckMotionErr(CWMX3.CM.Torque.StartTrq(m_TrqCmd), nAxis);
        }

		public override void OverrideVel(int nVel)
		{
			if (GetMoveSpeed() == nVel)			return;

			SetMoveSpeed(nVel, GetVel((int)VELOCITY.ACC), GetVel((int)VELOCITY.DEC));

			CWMX3.CheckMotionErr(CWMX3.CM.Motion.OverrideVel(m_PosCom), nAxis);
		}

		public void MoveIndexREL(int eNum, bool dir = true)
        {
            int direction = 1;
            if (dir == true)
                direction = 1;
            else
                direction = -1;

            SetMovePos(GetLocalPos() + direction * GetIndexPos(eNum));
            SetMoveSpeed(GetVel(eNum), GetVel((int)VELOCITY.ACC), GetVel((int)VELOCITY.DEC));

            SetCmdIndex(300);
            
            OnCommand(m_PosCom.Profile.Velocity);

            CWMX3.CheckMotionErr(CWMX3.CM.Motion.StartMov(m_PosCom), nAxis);
        }
        #endregion
    }
}
#endif