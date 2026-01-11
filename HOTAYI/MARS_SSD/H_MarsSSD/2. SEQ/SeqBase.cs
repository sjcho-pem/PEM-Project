using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;
using System.ComponentModel;

namespace H_MarsSSD
{
    public class SeqBase<TAct, TAxis, TCmd>
    {
        protected List<SMM<TCmd>> lstSMM = new List<SMM<TCmd>>();

        protected EnumArray<TAct, Actuator> arrAct;
        protected EnumArray<TAxis, CMotor> arrMt;

        private bool m_bErrOn = false;
        protected bool IsError { get => m_bErrOn; }

        public bool IsReady
        {
            get
            {
                foreach (var item in lstSMM)
                {
                    if (item.IsReady == false)
                        return false;
                }

                return true;
            }
        }

        protected void AddAlarm(ALM code)
        {
            if (1 > (int)code)
                return;

            if (code == ALM.NOT_ALARM)
                return;

            if (code < ALM.WARNING_START)
                m_bErrOn = true;

            G.AlmMng.ADD(code);
        }

        protected void Initialize()
        {
            for (int i = 0; i < arrAct.Count; i++)
            {
                arrAct[i].InSenEvent += G.cKernel.IsAIn;
                arrAct[i].OutEvent += G.cKernel.AOut;
                arrAct[i].SendTackTimeEvent += G.cKernel.SetTack;                

                arrAct[i].AddAlarmEvent += new Actuator.AddAlarmEventHandler(AddAlarm);
            }

            for (int i = 0; i < arrMt.Count; i++)
            {
                if (arrMt[i] != null)
                {
                    arrMt[i].AddAlarmEvent += new CMotor.AddAlarmEventHandler(AddAlarm);
                    arrMt[i].SetNowIndex(GetMtNowIndex(i));
                }
            }
        }

        private int GetMtNowIndex(int nAxis)
        {
            return 300;
        }

        protected void UpdateAct(Seq	Seq)
        {
            for (int i = 0; i < arrAct.Count; i++)
            {
				arrAct[i].Update();

				SM.SHM.SetActPos(Seq, i, arrAct[i].GetPos());
			}
        }

        public virtual void AlarmReset()
        {
            for (int i = 0; i < arrAct.Count; i++)
                arrAct[i].Reset();

            m_bErrOn = false;
        }

		public virtual void ResetStep(bool bMotorStop = true)
        {
            foreach (var item in lstSMM)
            {
                item.ResetStep();
            }

			if (arrMt == null)				return;
			if (bMotorStop == false)		return;

            for (int i = 0; i < arrMt.Count; i++)
            {
                arrMt[i].Stop();
            }
        }

        public void Reverse(TAct eAct)
        {
            arrAct[eAct].Reverse();
        }

        public void ActPos(TAct eAct, SenPos ePos)
        {
            switch (ePos)
            {
                case SenPos.FWD:
                    arrAct[eAct].Forward();
                    break;
                case SenPos.BWD:
                    arrAct[eAct].Backward();
                    break;
            }
        }
		
        public void ActPos(int nActIndex, SenPos ePos)
        {
            switch (ePos)
            {
                case SenPos.FWD:
                    arrAct[nActIndex].Forward();
                    break;
                case SenPos.BWD:
                    arrAct[nActIndex].Backward();
                    break;
            }
        }
        public bool IsActPos(TAct eAct, SenPos ePos)
        {
            if (arrAct[eAct].GetPos() == ePos)
                return true;
            return false;
        }

		public SenPos IsActPos(TAct eAct)
        {
            return arrAct[eAct].GetPos();
        }
    }
}
