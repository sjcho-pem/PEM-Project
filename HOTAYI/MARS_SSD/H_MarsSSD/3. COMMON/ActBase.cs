using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace H_MarsSSD
{
    #region Enum, State
    public enum SenPos
    {
        NONE = 0,
        UNKNOWN = 1,

        ON = 2,
        FWD = 2,
        PICKER_DOWN = 2,
		TABLE_GRIP =2,
        UP = 2,
        VAC = 2,

        OFF = 3,
        BWD = 3,
        PICKER_UP = 3,
		TABLE_RELEASE = 3,
        DOWN = 3,
        BLOW = 3,
    };

    public enum ErrSenPos
    {
        PRE,
        INIT,
        FWD,
        BWD,
        Ori,
        None,
    };

    public enum Contact
    {
        AOut,
        BOut,
    }
    #endregion Enum END

    public class Actuator
    {
        #region Variables
        protected SenPos m_ePos;        // 현재 위치
        protected SenPos m_eCmdPos;   // 명령 위치
        protected ErrSenPos m_eErrPos;   // 에러 위치
        protected SenPos m_eInitPos;    // 초기위치

        protected PStopwatch m_twDelay = new PStopwatch(); // 위치 업데이트 시간 타이머
        protected PStopwatch m_twAlarm = new PStopwatch();
        protected PStopwatch m_twInitCheck = new PStopwatch();

        protected long m_lDelayTime;
        protected long m_lAlarmTime;
        protected bool m_bErrOn;
        protected bool m_bInitCheck;

        protected bool m_bTackTimeCheck;
        #endregion Variables END

        public Actuator()
        {
            m_ePos = SenPos.UNKNOWN;
            m_eCmdPos = SenPos.NONE;
            m_eErrPos = ErrSenPos.PRE;
            m_eInitPos = SenPos.NONE;

            m_lDelayTime = 0;
            m_lAlarmTime = 0;
            m_bErrOn = false;
            m_bInitCheck = false;
        }

        #region Functions
        public virtual void SetSenser1(List<DI> lstDi) { }
        public virtual void SetSenser2(List<DI> lstDi) { }
        public virtual void SetSol(DO eDo, Contact econtact = Contact.AOut) { }
        public virtual void SetSol(DO eDo1, DO eDo2, ulong lOffTime = 300) { }
        public virtual void SetOff(bool bSol1OffUse = false, bool bSol2OffUse = false) { }
        public virtual void Setting(ALM eErrCode1, ALM eErrCode2,
            ErrSenPos eErrPos = ErrSenPos.None, SenPos eInit = SenPos.NONE,
            ulong lDelayTime = 200, ulong lAlarmTime = 2000){ }
        public virtual void Setting(SenPos eInit = SenPos.NONE, ulong lDelayTime = 200) { } // No Sensor Only Use..
        public virtual void Update() { }
        public void Reset() { m_twAlarm.Restart(); m_bErrOn = false; }
        public virtual void SetAlarm(bool bUse) { }
        public bool IsPos(SenPos ePos) { return (m_ePos == ePos); }
        public void SetPos(SenPos pos)
        {
            switch (pos)
            {
                case SenPos.FWD:
                    Forward();
                    break;
                case SenPos.BWD:
                    Backward();
                    break;
                case SenPos.NONE:
                    Stop();
                    break;
            }
        }
        public SenPos GetPos() { return m_ePos; }
        public SenPos GetCmd() { return m_eCmdPos; }

		public bool IsRdy() { return m_ePos == m_eCmdPos; }

        public virtual void Forward() { }
        public void Up() { Forward(); }
        public void Vacuum() { Forward(); }

        public virtual void Backward() { }
        public void Down() { Backward(); }
        public void Blow() { Backward(); }

        public virtual void Stop() { }

        public void Reverse()
        {
            switch (m_ePos)
            {
                case SenPos.FWD:
                    Backward();
                    break;
                case SenPos.BWD:
                    Forward();
                    break;
            }
        }
        #endregion Functions END

        #region Events

        /// <summary>
        /// ex) SenPos pos = new Actuator();
        /// </summary>
        /// <param name="act"></param>
        public static implicit operator SenPos(Actuator act)
        {
            if (act.m_bErrOn)
                return SenPos.UNKNOWN;

            return act.m_ePos;
        }

        public delegate bool InSenEventHandler(DI eDi);
        public event InSenEventHandler InSenEvent;
        protected virtual bool OnInSenEvent(DI eDi)
        {
            if (InSenEvent != null)
                return InSenEvent(eDi);

            return true;
        }

        public delegate void SendTackTimeEventHandler(DI eDi, int tackTime);
        public event SendTackTimeEventHandler SendTackTimeEvent;
        protected virtual void OnSendTackTimeEvent(DI eDi, int tackTime)
        {
            if (SendTackTimeEvent != null)
                SendTackTimeEvent(eDi, tackTime);
        }

        public delegate void OutEventHandler(DO eDo, bool bOn);
        public event OutEventHandler OutEvent;
        protected virtual bool OnOutEvent(DO eDo, bool bOn)
        {
            if (OutEvent != null)
                OutEvent(eDo, bOn);

            return true;
        }

        public delegate void AddAlarmEventHandler(ALM eValue);
        public event AddAlarmEventHandler AddAlarmEvent;
        protected virtual void OnAddAlarmEvent(ALM eValue)
        {
            m_bErrOn = true;
            if (AddAlarmEvent != null)
                AddAlarmEvent(eValue);
        }
        #endregion Events END

    }
}
