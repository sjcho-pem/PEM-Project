using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_MarsSSD
{
    #region [ ENUM ]    
    public enum OpState
    {
        Idle = 0,
        Pos = 1,
        Jog = 2,
        Home = 3,
        Sync = 4,
        GantryHome = 5,
        Stop = 6,
        Intpl = 7,
        Velocity = 8,
        ConstLinearVelocity = 9,
        Trq = 10,
        DirectControl = 11,
        PVT = 12,
        ECAM = 13,
        SyncCatchUp = 14,
        DancerControl = 15
    }

    public enum HomeState
    {
        Idle,
        HomeSearch,
        HomeSuccess,
    }
    #endregion

    #region [ STRUCT ]
    public struct MotorStatus
    {
        public bool Alarm;
        public int AlarmCode;

        public int CmdPos;
        public int ActPos;
        public int ActVel;
        public OpState CurOpState;
        public int ActTorque;

        public bool ServoOn;
        public bool HomeDone;
        public HomeState eHomeState;

        public bool PosSet;             // Positioning 
        public bool CmdDistributionEnd; // Command Distribution Completed Flag 
        public bool Inpos;

        public bool NegLSen;             // NegartiveLimitSenser
        public bool PosLSen;             // PositiveLimitSenser
        public bool HomeSen;             // ORG Limit Sensor

        public int CurIndex;
        public int NextIndex;
    }
    #endregion

    public class CMotor
    {
        #region - event callback
        public delegate void AddAlarmEventHandler(ALM eValue);
        public event AddAlarmEventHandler AddAlarmEvent;
        protected virtual void OnAddAlarmEvent(int nValue)
        {
            if (AddAlarmEvent != null)
                AddAlarmEvent((ALM)nValue);
        }

        public delegate int GetPosDataEventHandler(MT nMtNum, int eNum);
        public event GetPosDataEventHandler GetPosDataEvent;
        protected virtual int OnGetPosDataEvent(MT nMtNum, int eNum)
        {
            int nPos = 0;
            if (GetPosDataEvent != null)
                nPos = GetPosDataEvent(nMtNum, eNum);

            return nPos;
        }

        public delegate int GetVelDataEventHandler(MT nMtNum, int eNum);
        public event GetVelDataEventHandler GetVelDataEvent;
        protected virtual int OnGetVelDataEvent(MT nMtNum, int eNum)
        {
            int nVel = 0;
            if (GetVelDataEvent != null)
                nVel = GetVelDataEvent(nMtNum, eNum);

            return nVel;
        }
        #endregion

        #region - Variables
        private int m_nAxis;

        private int m_Vel;
        private int m_Acc;
        private int m_Dec;
        private float m_VelPercent;

        private int m_nTimeOut;

        private double m_dInposRange;
        #endregion

        private bool m_bCommand;
        private bool m_bRdy;
        private float m_fTargetPos;

        private PStopwatch m_twTimeOut;        
        protected MotorStatus m_stMt;

        public MotorStatus stMt
        {
            get
            {
                return m_stMt;
            }
        }

        public CMotor()
        {
            m_nAxis = 0;

            m_Vel = 100;
            m_Acc = 1000;
            m_Dec = 1000;
            m_stMt.CurOpState = OpState.Idle;
            m_VelPercent = 1.0f;

            m_bCommand = false;

            m_fTargetPos = 0;

            m_twTimeOut = new PStopwatch();
            m_twTimeOut.Reset();
            m_nTimeOut = 5000;

            m_dInposRange = 50;
        }

        #region 실질적으로 쓰이는 값
        public int nAxis { get => m_nAxis; set => m_nAxis = value; }
        public int Vel { get => m_Vel; set => m_Vel = value; }
        public OpState CurOpState { get => m_stMt.CurOpState; }
        public int Acc { get => m_Acc; set => m_Acc = value; }
        public int Dec { get => m_Dec; set => m_Dec = value; }
        public float VelPercent { get => m_VelPercent; set => m_VelPercent = value; }
        public double InposRange { get => m_dInposRange; }

        public int MoveTimeOut { get => m_nTimeOut; }

        public void SetRdy(bool bOn) { m_bRdy = bOn; }
        public bool IsCommand() { return (m_bCommand); }
        #endregion

        #region 모터 관련 세팅 함수
        public void OnCommand(double dSpd)
        {
            double speed = (dSpd / 1000);
            //if (speed < 1) speed = 1;
            double temp = Math.Abs(m_stMt.CmdPos - m_fTargetPos);
            m_bCommand = true;
            m_nTimeOut = (int)(2 * (temp / speed));
            if (m_nTimeOut < 5000) m_nTimeOut = 5000;
            m_twTimeOut.Restart();
        }
        public void OffCommand() { m_bCommand = false; }
        public bool CheckCommandTimeOut()
        {
            if (m_nTimeOut == 0) return true;
            return (m_twTimeOut.IsTimeOverMs(m_nTimeOut));
        }

        public int GetNowIndex() { return (m_stMt.CurIndex); }
        public void SetNowIndex(int eNum) { m_stMt.CurIndex = eNum; }
        public int GetCmdIndex() { return m_stMt.NextIndex; }
        public void SetCmdIndex(int eNum) { m_stMt.NextIndex = eNum; }

        public float GetTargetPos() { return (m_fTargetPos); }
        public void SetTargetPos(float fPos)
        {
            m_fTargetPos = fPos;
        }

        public int GetLocalPos() { return (m_stMt.CmdPos); }
		public int GetActPos() { return (m_stMt.ActPos); }

        public int GetIndexPos(int iIndex)
        {
            //if (iIndex == 0) return (-1);
            return OnGetPosDataEvent((MT)m_nAxis, iIndex);
        }

        public int GetVel(int iIndex)
        {
            return OnGetVelDataEvent((MT)m_nAxis, iIndex);
        }
        #endregion

        #region 모터 상태를 알려주는함수
        public virtual void Run() { }
        public virtual bool SetServoOn(bool bOn) => false;
        public virtual bool AlarmReset() => true;
        public virtual bool Stop() => false;
        public virtual bool EStop() => false;
        public virtual bool TorqueStop() => false;
        public virtual void StartHoming() { }
        public virtual void ClearHome() { }

        public virtual void MoveJog(bool dir, int nVel = 0) { }
        public virtual void MoveREL(int nDist, int nVel = 0) { }
        public virtual void MoveABS(int nPos) { }
		public virtual void MoveABS(int nPos, int nIdxVel) { }
        public virtual void MoveIndex(int eNum) { }
		public virtual void MoveIndex(int eNum, int nIdxVel) { }
        public virtual void TMoveIndex(int eNum, int nTMt, int nRemainDist, int nSecondVel) { }
        public virtual void MoveTorque(int eNum) { }
        public virtual void MultiMoveIndex(int[] nIndex, int[] nSpeed = null) { }
		public virtual void OverrideVel(int nVel) { }

        public virtual bool IsAlarm() { return m_stMt.Alarm; }
        public virtual bool IsSVON() { return m_stMt.ServoOn; }
        public virtual bool IsPSET() { return m_stMt.PosSet; }
        public virtual bool IsDEN() { return m_stMt.CmdDistributionEnd; }
        public virtual bool IsInPos() { return m_stMt.Inpos; }

        public virtual bool IsNegLSen() { return m_stMt.NegLSen; }
        public virtual bool IsPosLSen() { return m_stMt.PosLSen; }
        public virtual bool IsHomeSen() { return m_stMt.HomeSen; }
        public virtual bool IsHomeDone() { return m_stMt.HomeDone; }

        public virtual HomeState GetHomeState() { return m_stMt.eHomeState; }

        public bool IsRdy() { return (m_bRdy); }
        public bool IsRdy(int eNum)
        {
            if (!m_bRdy) return false;
            if (GetNowIndex() != eNum) return false;
            if (!IsInPosIndex(eNum)) return false;

            return true;
        }
        public bool IsRdyPos(int nPos)
        {
            if (!IsRdy()) return false;
			
			return (GetLocalPos() == nPos  &&
					GetActPos() > (nPos - InposRange)  &&
					GetActPos() < (nPos + InposRange));
        }

        public bool IsInPosIndex(int eNum)
        {
            return (GetLocalPos() == GetIndexPos(eNum)  &&
					GetActPos() > (GetIndexPos(eNum) - InposRange)  &&
					GetActPos() < (GetIndexPos(eNum) + InposRange));
        }

        public bool IsBiggerThan(int eNum)
        {
            return (GetLocalPos() > GetIndexPos(eNum)  &&
					GetActPos() > (GetIndexPos(eNum) + InposRange));
        }

		public bool IsBiggerThanOrEqual(int eNum)
        {
            return (GetLocalPos() >= GetIndexPos(eNum)  &&
					GetActPos() >= (GetIndexPos(eNum) - InposRange));
        }

        public bool IsSmallerThan(int eNum)
        {
            return (GetLocalPos() < GetIndexPos(eNum)  &&
					GetActPos() < (GetIndexPos(eNum) - InposRange));
        }

		public bool IsSmallerThanOrEqual(int eNum)
        {
            return (GetLocalPos() <= GetIndexPos(eNum)  &&
					GetActPos() <= (GetIndexPos(eNum) + InposRange));
        }

        #endregion
    }
}
