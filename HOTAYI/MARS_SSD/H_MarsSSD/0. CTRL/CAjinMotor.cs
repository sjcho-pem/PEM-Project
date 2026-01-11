using System;
using System.ComponentModel;

namespace H_MarsSSD
{
    /****************************************************************
    <SSCNET사용시 전자기어비 설정> 
    미쯔비시 MRJ4B Type(SSCNETIII/H) 서보 드라이브의 경우 전자기어비가 존재 하지 않기 때문에
    PCI-R1604-SIIIH Master Board에서 전자기어비를 설정 하고 비 휘발성 메모리에 저장
    전자기어비에 큰값(Dafault -> 2^22 :10000 이고 Unit/Pulse가 1:1 일때 570,000 pps) 이상이 들어가게 되면
    계산식에서 오버플로우가 발생하여 반대방향으로 이동 할 수 있으며 이때 전자기어비의 분자/분모를 약분하여 입력 
    예를 들어 2^22의 분해능을 가지는 서보 모터의 한바퀴 돌 때 필요한 펄스 수를 40000이라 한다면
    분자(Num.)에는 2^22인 4194304 가 입력 되고 분모(Deno.)에는 40000을 넣어야 하지만
    분자와 분모를 서로 같은 수로 약분하여 16로 약분한 분자(Num.)에 262144, 분모(Deno.)에 2500을 넣으시면 됩니다.
    약분하는 수에 따라 /2일 때 1.15Mbps, /4일 때 2.3Mbps, /8일 때 4.6Mbps, /16일 때 9.2Mbps 까지 사용이 가능 
    ****************************************************************/

    public class CAjinMotor : CMotor
    {
        #region Variables, Events        
        public enum Cmd
        {
            [DefaultValueAttribute("MOTOR ALM RESET")]
            START = 100,
            CHECK,

            END = 199,
        }

        private SMM<Cmd> m_smmAlmReset = new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(Cmd.START));
        private void AlmResetCycleStart() { m_smmAlmReset.eInput = InState.run; }

        private struct PosProfile
        {
            public double Target;
            public double Velocity;
            public double Acc;
            public double Dec;
        }
        private PosProfile stPosProfile;
        private struct JogProfile
        {
            public double Velocity;
            public double Acc;
            public double Dec;
        }        
        private JogProfile stJogProfile;        

        public CAjinMotor(int _nAxis)
        {
            nAxis = _nAxis;

            InitMotion();
        }
        #endregion

        #region private Funcations
        private void InitMotion()
        {

        }              

        private void ReadBitStatus()
        {
            uint uiErr = 0, uiStatus1 = 0, uiStatus2 = 0;
            ushort usStatus = 0;
            double dValue = 0.0f;
            
            uiErr = CAXM.AxmSignalReadServoAlarm(nAxis, ref uiStatus1);
            m_stMt.Alarm = Convert.ToBoolean(uiStatus1);
            uiErr = CAXM.AxmSignalReadServoAlarmCode(nAxis, ref usStatus);
            m_stMt.AlarmCode = Convert.ToInt16(usStatus);

            uiErr = CAXM.AxmStatusGetCmdPos(nAxis, ref dValue);
            m_stMt.CmdPos = (int)(Math.Truncate(dValue));
            uiErr = CAXM.AxmStatusGetActPos(nAxis, ref dValue);
            m_stMt.ActPos = (int)(Math.Truncate(dValue));
            uiErr = CAXM.AxmStatusReadVel(nAxis, ref dValue);
            m_stMt.ActVel = (int)(Math.Truncate(dValue));
                        
            m_stMt.CurOpState = OpState.Idle;

            uiErr = CAXM.AxmSignalIsServoOn(nAxis, ref uiStatus1);            
            m_stMt.ServoOn = Convert.ToBoolean(uiStatus1);            
            
            uiErr = CAXM.AxmHomeGetResult(nAxis, ref uiStatus1);
            switch ((AXT_MOTION_HOME_RESULT)uiStatus1)
            {
                case AXT_MOTION_HOME_RESULT.HOME_SEARCHING:
                    m_stMt.eHomeState = HomeState.HomeSearch;
                    break;
                case AXT_MOTION_HOME_RESULT.HOME_SUCCESS:
                    m_stMt.eHomeState = HomeState.HomeSuccess;
                    m_stMt.HomeDone = true;
                    break;
                case AXT_MOTION_HOME_RESULT.HOME_ERR_UNKNOWN:
                    m_stMt.HomeDone = false;
                    break;
                default:
                    m_stMt.eHomeState = HomeState.Idle;
                    break;
            }

            
            uiErr = CAXM.AxmStatusReadMotion(nAxis, ref uiStatus1);
            if (uiErr == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                int[] iDigitDrive = { 0, 1, 2, 3, 4, 5, 6, 7, 10, 11 };
                int iCheck = ((int)uiStatus1 >> iDigitDrive[0] & 0x1);
                m_stMt.PosSet = Convert.ToBoolean(iCheck);
                iCheck = ((int)uiStatus1 >> iDigitDrive[1] & 0x1);
                m_stMt.CmdDistributionEnd = Convert.ToBoolean(iCheck);                
            }

            uiErr = CAXM.AxmSignalReadInpos(nAxis, ref uiStatus1);
            m_stMt.Inpos = Convert.ToBoolean(uiStatus1);

            uiErr = CAXM.AxmSignalReadLimit(nAxis, ref uiStatus1, ref uiStatus2);
            m_stMt.NegLSen = Convert.ToBoolean(uiStatus1);
            m_stMt.PosLSen = Convert.ToBoolean(uiStatus2);
            uiErr = CAXM.AxmHomeReadSignal(nAxis, ref uiStatus1);
            m_stMt.HomeSen = Convert.ToBoolean(uiStatus1);

            if (uiErr != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                CAjin.CheckMotionErr(uiErr, nAxis);

            SetRdy(IsSVON() && !IsAlarm() && IsHomeDone() && IsInPos() && !IsCommand());

            if (IsCommand() && IsHomeDone())
            {
                if (CheckCommandTimeOut() && !IsInPos()) // !IsPSET() 대체가능한지 확인 필요
                    OnAddAlarmEvent((int)ALM.ERROR_MT_MOVE_TIME_OUT_START + nAxis + 1);

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

        private void ErrorProc()
        {
            if (IsAlarm())
                OnAddAlarmEvent((int)ALM.ERROR_MT_DRIVER_START + nAxis + 1);

            if (!IsSVON())
                OnAddAlarmEvent((int)ALM.ERROR_MT_POWER_START + nAxis + 1);

            if (IsPosLSen() || IsNegLSen())
                OnAddAlarmEvent((int)ALM.ERROR_MT_LIMIT_START + nAxis + 1);
        }

        private void AlarmResetCycle(SMM<Cmd> smm)
        {
            switch (smm.CurStep)
            {
                case Cmd.START:
                    {
                        if (smm.eInput == InState.run)
                        {
                            smm.eInput = InState.none;
                            CAjin.CheckMotionErr(CAXM.AxmSignalServoAlarmReset(nAxis, 1), nAxis);
                            smm.Next(Cmd.CHECK);
                        }                                                
                    }
                    break;
                case Cmd.CHECK:
                    {
                        if (!smm.Delayms(500))
                            break;
                        smm.Next(Cmd.END);
                    }
                    break;
                

                case Cmd.END:
                    {
                        CAjin.CheckMotionErr(CAXM.AxmSignalServoAlarmReset(nAxis, 0), nAxis);
                        smm.Next(Cmd.START);
                    }
                    break;
            }
        }

        private void SetMovePos(float fTarget)
        {
            stPosProfile.Target = fTarget;

            SetTargetPos(fTarget);
        }

        private void SetMoveSpeed(int nVel, int nAcc, int nDec, bool bDir = true)
        {
            if (bDir == false)
                nVel *= -1;

            if (nVel > 100)
                stPosProfile.Velocity = nVel * VelPercent;
            else
                stPosProfile.Velocity = nVel;

            stPosProfile.Acc = nAcc;
            stPosProfile.Dec = nDec;
        }

        private void SetJogSpeed(int nVel, int nAcc, int nDec, bool bDir = true)
        {
            if (bDir == false)
                nVel *= -1;

            if (nVel > 100)
                stJogProfile.Velocity = nVel * VelPercent;
            else
                stJogProfile.Velocity = nVel;

            stJogProfile.Acc = nAcc;
            stJogProfile.Dec = nDec;
        }
        #endregion private Funcations - End

        #region public Funcations
        public override void Run()
        {
            if (CAjin.bNoDevice == true)
                return;
            ReadBitStatus();
            ErrorProc();
            AlarmResetCycle(m_smmAlmReset);
        }

        public override bool SetServoOn(bool bOn)
        {
            if (CAjin.bNoDevice == true)
                return false;
            uint nOn = 1;

            if (!bOn)            
                nOn = 0;

            CAjin.CheckMotionErr(CAXM.AxmSignalServoOn(nAxis, nOn), nAxis);
            return true;
        }

        public override bool AlarmReset()
        {
            if (CAjin.bNoDevice == true)
                return false;

            //On/Off 사이클 필요함.               
            if (m_smmAlmReset.IsReady)
                AlmResetCycleStart();

            return true;
        }

        public override bool Stop()
        {
            if (CAjin.bNoDevice == true)
                return false;

            CAjin.CheckMotionErr(CAXM.AxmMoveSStop(nAxis), nAxis);

            OffCommand();
            SetCmdIndex(300);
            SetNowIndex(300);

            return true;
        }
        public override bool EStop()
        {
            if (CAjin.bNoDevice == true)
                return false;

            CAjin.CheckMotionErr(CAXM.AxmMoveStop(nAxis, GetVel((int)VELOCITY.DEC) * 10), nAxis);
            OffCommand();

            return true;
        }

        public override void ClearHome()
        {
            if (CAjin.bNoDevice == true)
                return;

            SetCmdIndex(300);
            SetNowIndex(300);
            
            CAjin.CheckMotionErr(CAXM.AxmHomeSetResult(nAxis, (uint)AXT_MOTION_HOME_RESULT.HOME_ERR_UNKNOWN), nAxis);
        }

        public override void StartHoming()
        {
            if (CAjin.bNoDevice == true)
                return;

            SetCmdIndex(300);
            SetNowIndex(300);
			SetMovePos(0);
            
            //double dVelFirst, dVelSecond, dVelThird, dVelLast, dAccFirst, dAccSecond;

            //// 각각의 홈 속도값 설정.
            //dVelFirst = Math.Abs(Convert.ToDouble(GetVel((int)VELOCITY.HomeFind) / 1000.0f));
            //dVelSecond = Math.Abs(Convert.ToDouble(GetVel((int)VELOCITY.HomeFind) / 1000.0f));
            //dVelThird = Math.Abs(Convert.ToDouble(GetVel((int)VELOCITY.HomeSearch) / 1000.0f));
            //dVelLast = Math.Abs(Convert.ToDouble(GetVel((int)VELOCITY.HomeSearch) / 1000.0f));
            //dAccFirst = Math.Abs(Convert.ToDouble(GetVel((int)VELOCITY.ACC) / 1000.0f));
            //dAccSecond = Math.Abs(Convert.ToDouble(GetVel((int)VELOCITY.DEC) /1000.0f));

            ////++ 원점검색에 사용되는 단계별 속도를 설정합니다.
            //CAjin.CheckMotionErr(CAXM.AxmHomeSetVel(nAxis, dVelFirst, dVelSecond, dVelThird, dVelLast, dAccFirst, dAccSecond), nAxis);

            //// Home 진행할때마다 설정값 넣어줘야하는지 테스트 필요.
            CAjin.CheckMotionErr(CAXM.AxmHomeSetStart(nAxis), nAxis);
        }

        public override void MoveJog(bool dir, int nVel = 0)
        {
            if (CAjin.bNoDevice == true)
                return;

            if (nVel == 0)
                nVel = GetVel((int)VELOCITY.JOG);

            SetJogSpeed(nVel, GetVel((int)VELOCITY.ACC), GetVel((int)VELOCITY.DEC), dir);
            SetCmdIndex(300);            
            
            CAjin.CheckMotionErr(CAXM.AxmMoveVel(nAxis, stJogProfile.Velocity, stJogProfile.Acc, stJogProfile.Dec), nAxis);
        }

        public override void MoveREL(int nDist, int nVel = 0)
        {
            if (CAjin.bNoDevice == true)
                return;

            if (nVel == 0)
                nVel = GetVel((int)VELOCITY.JOG);

            SetMovePos(GetLocalPos() + nDist);
            SetMoveSpeed(nVel, GetVel((int)VELOCITY.ACC), GetVel((int)VELOCITY.DEC));
            SetCmdIndex(300);

            OnCommand(stPosProfile.Velocity);

            CAjin.CheckMotionErr(CAXM.AxmMotSetAbsRelMode(nAxis, (uint)AXT_MOTION_ABSREL.POS_REL_MODE), nAxis);
            CAjin.CheckMotionErr(CAXM.AxmMoveStartPos(nAxis, nDist, stPosProfile.Velocity, 
                stPosProfile.Acc, stPosProfile.Dec), nAxis);
        }

        public override void MoveABS(int nPos)
        {
            if (CAjin.bNoDevice == true)
                return;

            if (!IsRdy() || IsCommand() || GetLocalPos() == nPos)
                return;

            SetMovePos(nPos);
            SetMoveSpeed(GetVel((int)VELOCITY.JOG), GetVel((int)VELOCITY.ACC), GetVel((int)VELOCITY.DEC));
            SetCmdIndex(300);

            OnCommand(stPosProfile.Velocity);

            CAjin.CheckMotionErr(CAXM.AxmMotSetAbsRelMode(nAxis, (uint)AXT_MOTION_ABSREL.POS_ABS_MODE), nAxis);
            CAjin.CheckMotionErr(CAXM.AxmMoveStartPos(nAxis, stPosProfile.Target, stPosProfile.Velocity,
                stPosProfile.Acc, stPosProfile.Dec), nAxis);
        }

        public override void MoveABS(int nPos, int nIdxVel)
        {
            if (CAjin.bNoDevice == true)
                return;

            if (!IsRdy() || IsCommand() || GetLocalPos() == nPos)
                return;

            SetMovePos(nPos);
            SetMoveSpeed(GetVel(nIdxVel), GetVel((int)VELOCITY.ACC), GetVel((int)VELOCITY.DEC));
            SetCmdIndex(300);

            OnCommand(stPosProfile.Velocity);

            CAjin.CheckMotionErr(CAXM.AxmMotSetAbsRelMode(nAxis, (uint)AXT_MOTION_ABSREL.POS_ABS_MODE), nAxis);
            CAjin.CheckMotionErr(CAXM.AxmMoveStartPos(nAxis, stPosProfile.Target, stPosProfile.Velocity,
                stPosProfile.Acc, stPosProfile.Dec), nAxis);
        }

        public override void MoveIndex(int eNum)
        {
            if (CAjin.bNoDevice == true)
                return;

            if (!IsRdy() || IsCommand() || GetNowIndex() == eNum)
                return;

            SetMovePos(GetIndexPos(eNum));
            SetMoveSpeed(GetVel(eNum), GetVel((int)VELOCITY.ACC), GetVel((int)VELOCITY.DEC));
            SetCmdIndex(eNum);

            OnCommand(stPosProfile.Velocity);
            
            CAjin.CheckMotionErr(CAXM.AxmMotSetAbsRelMode(nAxis, (uint)AXT_MOTION_ABSREL.POS_ABS_MODE), nAxis);
            CAjin.CheckMotionErr(CAXM.AxmMoveStartPos(nAxis, stPosProfile.Target, stPosProfile.Velocity,
                stPosProfile.Acc, stPosProfile.Dec), nAxis);
        }
        #endregion public Funcations - End


    }


}
