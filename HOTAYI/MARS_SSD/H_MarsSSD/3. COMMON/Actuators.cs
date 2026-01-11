using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace H_MarsSSD
{
    public class CSingleSen : Actuator
    {    
        protected List<DI> m_lstSen1;
        protected List<DI> m_lstSen2;

        private Contact m_eOutcontact = Contact.AOut;
        protected DO m_eOut1;

        public ALM m_eErrCode1, m_eErrCode2;

        public CSingleSen()
        {
            m_lstSen1 = new List<DI>();
            m_lstSen2 = new List<DI>();

            m_eOut1 = DO.Total;

            m_eErrCode1 = m_eErrCode2 = 0;

            m_twInitCheck.Restart();
        }
        
        /// <summary>
        /// CSingleSen --> CSingleNoSen으로 캐스팅 사용.
        /// </summary>
        /// <param name="Sen"></param>
        public static explicit operator CSingleNoSen(CSingleSen Sen)
        {
            CSingleNoSen cSingleNoSen = new CSingleNoSen();
            cSingleNoSen.SetSol(Sen.GetSol1());
            cSingleNoSen.Setting(Sen.m_eInitPos, (ulong)Sen.m_lDelayTime);
            cSingleNoSen.SetPos(Sen.GetPos());
            return cSingleNoSen;
        }
        private bool GetContact(bool bIO)
        {
            switch (m_eOutcontact)
            {
                case Contact.BOut:
                    return !bIO;
                case Contact.AOut:
                default:
                    return bIO;
            }
        }

        public override void SetSenser1(List<DI> lstDi)
        {
            m_lstSen1 = lstDi;
        }

        public override void SetSenser2(List<DI> lstDi)
        {
            m_lstSen2 = lstDi;
        }

        public override void SetSol(DO eDo, Contact econtact = Contact.AOut)
        {
            m_eOut1 = eDo;
            m_eOutcontact = econtact;
        }

        public DO GetSol1()
        {
            return m_eOut1;
        }

        public override void Setting(ALM eErrCode1, ALM eErrCode2,
            ErrSenPos eErrPos = ErrSenPos.PRE, SenPos eInit = SenPos.NONE,
            ulong lDelayTime = 200, ulong lAlarmTime = 2000)
        {
            m_eErrCode1 = eErrCode1;
            m_eErrCode2 = eErrCode2;

            m_eErrPos = eErrPos;
            m_eInitPos = eInit;

			if (Program._bSimul_.Equals(true))		lDelayTime = lDelayTime/10;

			m_lDelayTime = (long)lDelayTime;
            m_lAlarmTime = (long)lAlarmTime;

            if (eInit == SenPos.FWD)
                Forward();
            else if (eInit == SenPos.BWD)
                Backward();
        }

        public override void Forward()
        {
            if (m_eCmdPos == SenPos.FWD)
                return;

            m_bErrOn = false;
            m_eCmdPos = SenPos.FWD;
            m_ePos = SenPos.UNKNOWN;
            m_twDelay.Restart();
            m_twAlarm.Restart();
            m_bTackTimeCheck = true;
        }

        public override void Backward()
        {
            if (m_eCmdPos == SenPos.BWD)
                return;

            m_bErrOn = false;
            m_eCmdPos = SenPos.BWD;
            m_ePos = SenPos.UNKNOWN;
            m_twDelay.Restart();
            m_twAlarm.Restart();
            m_bTackTimeCheck = true;
        }

        public override void Stop()
        {
            m_eCmdPos = m_ePos = SenPos.UNKNOWN;
            m_bErrOn = false;
            m_twDelay.Restart();
            m_twAlarm.Restart();

            OnOutEvent(m_eOut1, false);
        }

        public bool Sen1()
        {
            if (m_lstSen1.Count == 0)
                return true;

            bool bRet = true;
            foreach (DI eDi in m_lstSen1)
                bRet &= OnInSenEvent(eDi);

            return bRet;
        }

        public bool CheckSen1()
        {
            if (m_lstSen1.Count == 0)
                return false;

            bool bRet = false;
            foreach (DI eDi in m_lstSen1)
                bRet |= OnInSenEvent(eDi);

            return bRet;
        }

        public bool Sen2()
        {
            if (m_lstSen2.Count == 0)
                return true;

            bool bRet = true;
            foreach (DI eDi in m_lstSen2)
                bRet &= OnInSenEvent(eDi);

            return bRet;
        }

        public bool CheckSen2()
        {
            if (m_lstSen2.Count == 0)
                return false;

            bool bRet = false;
            foreach (DI eDi in m_lstSen2)
                bRet |= OnInSenEvent(eDi);

            return bRet;
        }

        public DI GetSen1DI()
        {
            DI res = DI.Total;

            for (int i = 0; i < m_lstSen1.Count(); i++)
            {
                res = m_lstSen1[i];
            }

            return res;
        }

        public DI GetSen2DI()
        {
            DI res = DI.Total;

            for (int i = 0; i < m_lstSen2.Count(); i++)
            {
                res = m_lstSen2[i];
            }

            return res;
        }

        public long GetDelayTime()
        {
            return m_lDelayTime;
        }
		
        public override void Update()
        {
			if (Program._bSimul_ == true)
			{
				UpdateSim();
				return;
			}

            if (m_bInitCheck == false)
            {
                if (m_twInitCheck.IsTimeOverMs(1500))
                    m_bInitCheck = true;

                if (Sen1() && !CheckSen2()) m_ePos = SenPos.FWD;
                else if (!CheckSen1() && Sen2()) m_ePos = SenPos.BWD;
                else m_ePos = SenPos.UNKNOWN;

                switch (m_eInitPos)
                {
                    case SenPos.NONE:
                        m_eCmdPos = m_ePos;
                        break;
                    case SenPos.FWD:
                        m_eCmdPos = SenPos.FWD;
                        break;
                    case SenPos.BWD:
                        m_eCmdPos = SenPos.BWD;
                        break;
                    default:
                        m_eCmdPos = m_ePos;
                        break;
                }
            }

            if (m_eCmdPos != m_ePos)
            {
                bool bRet = (m_eCmdPos == SenPos.FWD) ? OnOutEvent(m_eOut1, GetContact(true)) : OnOutEvent(m_eOut1, GetContact(false));

                bool bSenCheck = false;

                if ((Sen1() && !CheckSen2() && m_eCmdPos == SenPos.FWD)
                        || (!CheckSen1() && Sen2() && m_eCmdPos == SenPos.BWD))
                {
                    bSenCheck = true;
                    if (m_bTackTimeCheck == true)
                    {
                        m_bTackTimeCheck = false;
                        CheckTackTimeAndSendEvent(m_eCmdPos);
                    }
                }

                if (m_twDelay.IsTimeOverMs(m_lDelayTime))
                {
                    if (bSenCheck == true)
                        m_ePos = m_eCmdPos;
                }

                if (m_twAlarm.IsTimeOverMs(m_lAlarmTime)
                    && m_bErrOn == false)
                {
                    if (m_eCmdPos == SenPos.FWD)
                        OnAddAlarmEvent(m_eErrCode1);
                    else
                        OnAddAlarmEvent(m_eErrCode2);

                    switch (m_eErrPos)
                    {
                        case ErrSenPos.PRE:
                            if (m_eCmdPos == SenPos.FWD)
                                m_eCmdPos = SenPos.BWD;
                            else
                                m_eCmdPos = SenPos.FWD;
                            break;
                        case ErrSenPos.INIT:
                            if (m_eInitPos == SenPos.FWD)
                                Forward();
                            else
                                Backward();
                            break;
                        case ErrSenPos.FWD:
                            m_eCmdPos = SenPos.FWD;
                            break;
                        case ErrSenPos.BWD:
                            m_eCmdPos = SenPos.BWD;
                            break;
                        case ErrSenPos.Ori:
                            m_twAlarm.Restart();
                            break;
                        default:
                            m_eCmdPos = m_ePos;
                            break;
                    }
                }
            }
            else
            {
                if (m_eCmdPos == SenPos.FWD)
                {
                    if (!Sen1())
                    {
                        OnAddAlarmEvent(m_eErrCode1);

                        if (Sen2())
                            m_eCmdPos = m_ePos = SenPos.BWD;
                    }
                }
                else if (m_eCmdPos == SenPos.BWD)
                {
                    if (!Sen2())
                    {
                        OnAddAlarmEvent(m_eErrCode2);

                        if (Sen1())
                            m_eCmdPos = m_ePos = SenPos.FWD;
                    }
                }
            }
        }
		
		public	void	UpdateSim()
		{
			if (m_eCmdPos != m_ePos)
			{
				 bool bRet = (m_eCmdPos == SenPos.FWD) ? OnOutEvent(m_eOut1, GetContact(true)) : OnOutEvent(m_eOut1, GetContact(false));

				if (m_twDelay.IsTimeOverMs(m_lDelayTime))
				{
					CheckTackTimeAndSendEvent(m_eCmdPos);
					m_ePos = m_eCmdPos;

					if		(m_eCmdPos == SenPos.FWD)
					{
						if (m_lstSen1.Count > 0)
						{
							foreach (DI eDi in m_lstSen1)
							{
								G.cKernel.AOut(eDi, true);
							}
						}

						if (m_lstSen2.Count > 0)
						{
							foreach (DI eDi in m_lstSen2)
							{
								G.cKernel.AOut(eDi, false);
							}
						}
					}
					else if (m_eCmdPos == SenPos.BWD)
					{
						if (m_lstSen1.Count > 0)
						{
							foreach (DI eDi in m_lstSen1)
							{
								G.cKernel.AOut(eDi, false);
							}
						}

						if (m_lstSen2.Count > 0)
						{
							foreach (DI eDi in m_lstSen2)
							{
								G.cKernel.AOut(eDi, true);
							}
						}
					}
				}
			}
		}

        private void CheckTackTimeAndSendEvent(SenPos _m_eCmdPos)
        {
            if (_m_eCmdPos == SenPos.FWD)
            {
                if (m_lstSen1.Count > 0)
                {
                    foreach (DI eDI in m_lstSen1)
                        OnSendTackTimeEvent(eDI, (int)m_twDelay.ElapsedMilliseconds);
                }
            }
            else
            {
                if (m_lstSen2.Count > 0)
                {
                    foreach (DI eDI in m_lstSen2)
                        OnSendTackTimeEvent(eDI, (int)m_twDelay.ElapsedMilliseconds);
                }
            }
        }
    }

    public class CDoubleSen : CSingleSen
    {
        protected PStopwatch m_twOff;

        protected ulong m_lOffDelayFwd;
        protected ulong m_lOffDelayBwd;

        protected bool m_bSol1OffUse, m_bSol2OffUse;
        protected bool m_bOffComplete;
        protected DO m_eOut2;

        public CDoubleSen()
        {
            m_twOff = new PStopwatch();

            m_lOffDelayFwd = 300;
            m_lOffDelayFwd = 300;

            m_bSol1OffUse = m_bSol2OffUse = false;
        }

        /// <summary>
        /// CDoubleSen --> CDoubleNoSen 으로 캐스팅 사용.
        /// </summary>
        /// <param name="Sen"></param>
        public static explicit operator CDoubleNoSen(CDoubleSen Sen)
        {
            CDoubleNoSen cDoubleNoSen = new CDoubleNoSen();
            cDoubleNoSen.SetSol(Sen.GetSol1(), Sen.GetSol2());
            cDoubleNoSen.Setting(Sen.m_eInitPos, (ulong)Sen.m_lDelayTime);
            cDoubleNoSen.SetPos(Sen.GetPos());
            return cDoubleNoSen;
        }

        public override void SetSol(DO eDo1, DO eDo2, ulong lOffTime = 300)
        {
            m_eOut1 = eDo1;
            m_eOut2 = eDo2;

            m_lOffDelayFwd = m_lOffDelayBwd = lOffTime;
        }

        public void SetOffTimeFwd(ulong lOffTime)
        {
            m_lOffDelayFwd = lOffTime;
        }

        public void SetOffTimeBwd(ulong lOffTime)
        {
            m_lOffDelayBwd = lOffTime;
        }

        public override void SetOff(bool bSol1OffUse = false, bool bSol2OffUse = false)
        {
            m_bSol1OffUse = bSol1OffUse;
            m_bSol2OffUse = bSol2OffUse;
        }

        public void ForceOff()
        {
            m_bErrOn = false;
            m_eCmdPos = SenPos.BWD;
            m_ePos = SenPos.UNKNOWN;
            m_twDelay.Restart();
            m_twAlarm.Restart();
        }

        public override void Stop()
        {
            m_eCmdPos = m_ePos = SenPos.UNKNOWN;
            m_bErrOn = false;
            m_twDelay.Restart();
            m_twAlarm.Restart();
            m_bOffComplete = true;

            OnOutEvent(m_eOut1, false);
            OnOutEvent(m_eOut2, false);
        }

        public override void Backward()
        {
            if (!m_bSol1OffUse)
                OnOutEvent(m_eOut1, false);

            base.Backward();
        }

        public DO GetSol2()
        {
            return m_eOut2;
        }

        public override void Update()
        {
			if (Program._bSimul_ == true)
			{
				UpdateSim();
				return;
			}

            if (m_bInitCheck == false)
            {
                if (m_twInitCheck.IsTimeOverMs(1500))
                    m_bInitCheck = true;

                if (Sen1() && !CheckSen2())
                    m_ePos = SenPos.FWD;
                else if (!CheckSen1() && Sen2())
                    m_ePos = SenPos.BWD;
                else
                    m_ePos = SenPos.UNKNOWN;

                switch (m_eInitPos)
                {
                    case SenPos.NONE:
                        m_eCmdPos = m_ePos;
                        break;
                    case SenPos.FWD:
                        m_eCmdPos = SenPos.FWD;
                        break;
                    case SenPos.BWD:
                        m_eCmdPos = SenPos.BWD;
                        break;
                    default:
                        m_eCmdPos = m_ePos;
                        break;
                }
            }

            if (m_eCmdPos != m_ePos)
            {
                m_bOffComplete = false;

                if (!m_twAlarm.IsTimeOverMs(m_lAlarmTime))
                {
                    if (m_eCmdPos == SenPos.FWD)
                    {
                        OnOutEvent(m_eOut1, true);
                        OnOutEvent(m_eOut2, false);
                    }

                    if (m_eCmdPos == SenPos.BWD)
                    {
                        OnOutEvent(m_eOut1, false);
                        OnOutEvent(m_eOut2, true);
                    }
                }

                if (m_twDelay.IsTimeOverMs(m_lDelayTime))
                {
                    if ((Sen1() && !CheckSen2() && m_eCmdPos == SenPos.FWD)
                        || (!CheckSen1() && Sen2() && m_eCmdPos == SenPos.BWD))
                    {
                        m_ePos = m_eCmdPos;
                        m_twOff.Restart();
                    }
                }

                if (m_twAlarm.IsTimeOverMs(m_lAlarmTime)
                    && m_bErrOn == false)
                {
                    if (m_eCmdPos == SenPos.FWD)
                        OnAddAlarmEvent(m_eErrCode1);
                    else
                        OnAddAlarmEvent(m_eErrCode2);

                    switch (m_eErrPos)
                    {
                        case ErrSenPos.PRE:
                            if (m_eCmdPos == SenPos.FWD)
                                Backward();
                            else
                                Forward();
                            break;
                        case ErrSenPos.INIT:
                            if (m_eInitPos == SenPos.FWD)
                                Forward();
                            else
                                Backward();
                            break;
                        case ErrSenPos.FWD:
                            Forward();
                            break;
                        case ErrSenPos.BWD:
                            Backward();
                            break;
                        case ErrSenPos.Ori:
                            m_twAlarm.Restart();
                            break;
                        default:
                            m_eCmdPos = m_ePos;
                            if (m_bSol1OffUse)
                                OnOutEvent(m_eOut1, false);
                            if (m_bSol2OffUse)
                                OnOutEvent(m_eOut2, false);
                            break;
                    }
                }
            }
            else
            {
                if (m_eCmdPos == SenPos.FWD)
                {
                    if (m_twOff.IsTimeOverMs((long)m_lOffDelayFwd) && m_bOffComplete == false)
                    {
                        m_bOffComplete = true;
                        if (m_bSol1OffUse)
                            OnOutEvent(m_eOut1, false);
                    }

                    if (!Sen1())
                    {
                        OnAddAlarmEvent(m_eErrCode1);

                        if (Sen2())
                            m_eCmdPos = m_ePos = SenPos.BWD;
                    }
                }
                else if (m_eCmdPos == SenPos.BWD)
                {
                    if (m_twOff.IsTimeOverMs((long)m_lOffDelayBwd) && m_bOffComplete == false)
                    {
                        m_bOffComplete = true;
                        if (m_bSol2OffUse)
                            OnOutEvent(m_eOut2, false);
                    }

                    if (!Sen2())
                    {
                        OnAddAlarmEvent(m_eErrCode2);

                        if (Sen1())
                            m_eCmdPos = m_ePos = SenPos.FWD;
                    }
                }
            }
        }

		public	new void	UpdateSim()
		{
		//	if (m_bInitCheck == false)
		//	{
		//		if (m_twInitCheck.IsTimeOverMs(1500))
		//		{
		//			m_bInitCheck = true;
		//		}
		//
		//		if		(Sen1() == true  &&  CheckSen2() == false)			m_ePos = SenPos.FWD;
		//		else if (CheckSen1() == false  &&  Sen2() == true)			m_ePos = SenPos.BWD;
		//		else														m_ePos = SenPos.UNKNOWN;
		//
		//		switch (m_eInitPos)
		//		{
		//			case SenPos.NONE:	m_eCmdPos = m_ePos;			break;
		//			case SenPos.FWD:	m_eCmdPos = SenPos.FWD;		break;
		//			case SenPos.BWD:	m_eCmdPos = SenPos.BWD;		break;
		//			default:			m_eCmdPos = m_ePos;			break;
		//		}
		//	}

			if (m_eCmdPos != m_ePos)
			{
				if (m_eCmdPos == SenPos.FWD)
                {
                    OnOutEvent(m_eOut1, true);
                    OnOutEvent(m_eOut2, false);
                }

                if (m_eCmdPos == SenPos.BWD)
                {
                    OnOutEvent(m_eOut1, false);
                    OnOutEvent(m_eOut2, true);
                }

				m_bOffComplete = false;

				if (m_twDelay.IsTimeOverMs(m_lDelayTime+200))
				{
					m_ePos = m_eCmdPos;

					if		(m_eCmdPos == SenPos.FWD)
					{
						if (m_lstSen1.Count > 0)
						{
							foreach (DI eDi in m_lstSen1)
							{
								G.cKernel.AOut(eDi, true);
							}
						}

						if (m_lstSen2.Count > 0)
						{
							foreach (DI eDi in m_lstSen2)
							{
								G.cKernel.AOut(eDi, false);
							}
						}

						//if (m_twOff.IsTimeOverMs((long)m_lOffDelayFwd))
						{
							m_bOffComplete = true;
							if (m_bSol1OffUse)
							    OnOutEvent(m_eOut1, false);
						}
					}
					else if (m_eCmdPos == SenPos.BWD)
					{
						if (m_lstSen1.Count > 0)
						{
							foreach (DI eDi in m_lstSen1)
							{
								G.cKernel.AOut(eDi, false);
							}
						}

						if (m_lstSen2.Count > 0)
						{
							foreach (DI eDi in m_lstSen2)
							{
								G.cKernel.AOut(eDi, true);
							}
						}

						//if (m_twOff.IsTimeOverMs((long)m_lOffDelayBwd))
						{
							m_bOffComplete = true;
							if (m_bSol2OffUse)
							   OnOutEvent(m_eOut2, false);
						}
					}
				}
			}
		}
    }

    public class CIOMotor : CSingleSen
    {
        private ulong m_lDelayTimeOri;

        protected List<DI> m_lstPlus;
        protected List<DI> m_lstMinus;
        protected List<DI> m_lstHome;

        public delegate bool OutSenEventHandler(DO eDo);
        public event OutSenEventHandler OutSenEvent;
        protected virtual bool OnOutSenEvent(DO eDo)
        {
            if (OutSenEvent != null)
                OutSenEvent(eDo);

            return true;
        }

        protected DO m_eOutActive;
        protected DO m_eOutDirection;
        private Contact m_eOutcontact = Contact.AOut;

        public ALM m_eErrCode3;

        public CIOMotor()
        {
            m_lDelayTimeOri = 0;

            m_lstPlus = new List<DI>();
            m_lstMinus = new List<DI>();
            m_lstHome = new List<DI>();

            m_eOutActive = DO.Total;
            m_eOutDirection = DO.Total;

            m_eErrCode3 = 0;
        }

        public void SetSenserPlus(List<DI> lstDi)
        {
            m_lstPlus = lstDi;
        }

        public void SetSenserMinus(List<DI> lstDi)
        {
            m_lstMinus = lstDi;
        }

        public void SetSenserHome(List<DI> lstDi)
        {
            m_lstHome = lstDi;
        }

        public void SetActive(DO eDo)
        {
            m_eOutActive = eDo;
        }

        public void SetDirection(DO eDo, Contact econtact = Contact.AOut)
        {
            m_eOutDirection = eDo;
            m_eOutcontact = econtact;
        }

        private void MoveDir(bool bDir)
        {
            switch (m_eOutcontact)
            {
                case Contact.BOut:
                    OnOutEvent(m_eOutDirection, !bDir);
                    break;
                default:
                    OnOutEvent(m_eOutDirection, bDir);
                    break;
            }

            OnOutEvent(m_eOutActive, true);
        }

        public bool IsReadyIOMotor()
        {
            return OnOutSenEvent(m_eOutActive);
        }

        public override void Stop()
        {
            m_eCmdPos = SenPos.NONE;

            OnOutEvent(m_eOutDirection, false);
            OnOutEvent(m_eOutActive, false);
        }

        public static implicit operator SenPos(CIOMotor cIOM)
        {
            return cIOM.m_ePos;
        }

        public void Forward(ulong MoveTime)
        {
            Forward();

            m_lDelayTime = (long)MoveTime;
        }

        public void Backward(ulong MoveTime)
        {
            Backward();

            m_lDelayTime = (long)MoveTime;
        }

        public override void Forward()
        {
            m_lDelayTime = (long)m_lDelayTimeOri;

            base.Forward();
        }

        public override void Backward()
        {
            m_lDelayTime = (long)m_lDelayTimeOri;

            base.Backward();
        }

        public bool SenPlus()
        {
            if (m_lstPlus.Count == 0)
                return true;

            bool bRet = true;
            foreach (DI eDi in m_lstPlus)
                bRet &= OnInSenEvent(eDi);

            return bRet;
        }

        public bool CheckSenPlus()
        {
            if (m_lstPlus.Count == 0)
                return !SenPlus();

            bool bRet = false;
            foreach (DI eDi in m_lstPlus)
                bRet |= OnInSenEvent(eDi);

            return bRet;
        }

        public bool SenMinus()
        {
            if (m_lstMinus.Count == 0)
                return true;

            bool bRet = true;
            foreach (DI eDi in m_lstMinus)
                bRet &= OnInSenEvent(eDi);

            return bRet;
        }

        public bool CheckSenMinus()
        {
            if (m_lstMinus.Count == 0)
                return !SenMinus();

            bool bRet = false;
            foreach (DI eDi in m_lstMinus)
                bRet |= OnInSenEvent(eDi);

            return bRet;
        }

        public bool SenHome()
        {
            if (m_lstHome.Count == 0)
                return true;

            bool bRet = true;
            foreach (DI eDi in m_lstHome)
                bRet &= OnInSenEvent(eDi);

            return bRet;
        }

        public bool CheckSenHome()
        {
            if (m_lstHome.Count == 0)
                return !SenHome();

            bool bRet = false;
            foreach (DI eDi in m_lstHome)
                bRet |= OnInSenEvent(eDi);

            return bRet;
        }

        public override void Update()
        {
            if (m_eCmdPos != m_ePos)
            {
                switch (m_eCmdPos)
                {
                    case SenPos.NONE:
                        Stop();
                        break;
                    case SenPos.FWD:
                        if (SenPlus())
                        {
                            MoveDir(true);
                        }
                        else
                        {
                            m_ePos = m_eCmdPos = SenPos.FWD;

                            Stop();
                        }

                        break;
                    case SenPos.BWD:
                        if (SenMinus())
                        {
                            MoveDir(false);
                        }
                        else
                        {
                            m_ePos = m_eCmdPos = SenPos.BWD;

                            Stop();
                        }
                        break;
                }

                if (m_twDelay.IsTimeOverMs(m_lDelayTime))
                {
                    m_ePos = m_eCmdPos = SenPos.NONE;

                    Stop();
                }
            }
        }
    }

    public class CSingleNoSen : Actuator
    {        
        private Contact m_eOutcontact = Contact.AOut;
        protected DO m_eOut1;        

        public CSingleNoSen()
        {
            m_eOut1 = DO.Total;            

            m_twInitCheck.Restart();
        }

        /// <summary>
        /// CSingleNoSen --> CSingleSen으로 캐스팅 사용.        
        /// </summary>
        /// <param name="NoSen"></param>
        public static explicit operator CSingleSen(CSingleNoSen NoSen)
        {
            CSingleSen cSingleSen = new CSingleSen();
            cSingleSen.SetSol(NoSen.GetSol1());
            cSingleSen.Setting(ALM.NORMAL, ALM.NORMAL,
                NoSen.m_eErrPos, NoSen.m_eInitPos, (ulong)NoSen.m_lDelayTime, (ulong)NoSen.m_lAlarmTime);
            cSingleSen.SetPos(NoSen.GetPos());
            return cSingleSen;
        }

        private bool GetContact(bool bIO)
        {
            switch (m_eOutcontact)
            {
                case Contact.BOut:
                    return !bIO;
                case Contact.AOut:
                default:
                    return bIO;
            }
        }

        public override void Setting(SenPos eInit = SenPos.NONE, ulong lDelayTime = 200)
        {
            m_eInitPos = eInit;
            m_lDelayTime = (long)lDelayTime;

            if (eInit == SenPos.FWD)
                Forward();
            else if (eInit == SenPos.BWD)
                Backward();
        }

        public override void SetSol(DO eDo, Contact econtact = Contact.AOut)
        {
            m_eOut1 = eDo;
            m_eOutcontact = econtact;
        }

        public DO GetSol1()
        {
            return m_eOut1;
        }        

        public override void Forward()
        {
            if (m_eCmdPos == SenPos.FWD)
                return;
            
            m_eCmdPos = SenPos.FWD;
            m_ePos = SenPos.UNKNOWN;
            m_twDelay.Restart();                        
        }

        public override void Backward()
        {
            if (m_eCmdPos == SenPos.BWD)
                return;
            
            m_eCmdPos = SenPos.BWD;
            m_ePos = SenPos.UNKNOWN;
            m_twDelay.Restart();                        
        }

        public override void Stop()
        {
            m_eCmdPos = m_ePos = SenPos.UNKNOWN;
            m_bErrOn = false;
            m_twDelay.Restart();            

            OnOutEvent(m_eOut1, false);
        }

        public override void Update()
        {
            if (m_bInitCheck == false)
            {
                if (m_twInitCheck.IsTimeOverMs(1500))
                    m_bInitCheck = true;
                switch (m_eInitPos)
                {
                    case SenPos.NONE:
                        m_eCmdPos = m_ePos;
                        break;
                    case SenPos.FWD:
                        m_eCmdPos = SenPos.FWD;
                        break;
                    case SenPos.BWD:
                        m_eCmdPos = SenPos.BWD;
                        break;
                    default:
                        m_eCmdPos = m_ePos;
                        break;
                }
            }
			
            if (m_eCmdPos != m_ePos)
            {
                bool bRet = (m_eCmdPos == SenPos.FWD) ? OnOutEvent(m_eOut1, GetContact(true)) : OnOutEvent(m_eOut1, GetContact(false));
                if (m_twDelay.IsTimeOverMs(m_lDelayTime))
                {
                    m_ePos = m_eCmdPos;
                }
            }
                
        }
    }

    public class CDoubleNoSen : CSingleNoSen
    {
        protected PStopwatch m_twOff;

        protected ulong m_lOffDelayFwd;
        protected ulong m_lOffDelayBwd;

        protected bool m_bSol1OffUse, m_bSol2OffUse;
        protected bool m_bOffComplete;
        protected DO m_eOut2;

        public CDoubleNoSen()
        {
            m_twOff = new PStopwatch();

            m_lOffDelayFwd = 300;
            m_lOffDelayFwd = 300;

            m_bSol1OffUse = m_bSol2OffUse = false;
        }

        /// <summary>
        /// CDoubleNoSen --> CDoubleSen 으로 캐스팅 사용.
        /// </summary>
        /// <param name="NoSen"></param>
        public static explicit operator CDoubleSen(CDoubleNoSen NoSen)
        {
            CDoubleSen cDoubleSen = new CDoubleSen();
            cDoubleSen.SetSol(NoSen.GetSol1(), NoSen.GetSol2());
            cDoubleSen.Setting(ALM.NORMAL, ALM.NORMAL,
                NoSen.m_eErrPos, NoSen.m_eInitPos, (ulong)NoSen.m_lDelayTime, (ulong)NoSen.m_lAlarmTime);
            cDoubleSen.SetPos(NoSen.GetPos());
            return cDoubleSen;
        }

        public override void SetSol(DO eDo1, DO eDo2, ulong lOffTime = 300)
        {
            m_eOut1 = eDo1;
            m_eOut2 = eDo2;

            m_lOffDelayFwd = m_lOffDelayBwd = lOffTime;
        }

        public void SetOffTimeFwd(ulong lOffTime)
        {
            m_lOffDelayFwd = lOffTime;
        }

        public void SetOffTimeBwd(ulong lOffTime)
        {
            m_lOffDelayBwd = lOffTime;
        }

        public override void SetOff(bool bSol1OffUse = false, bool bSol2OffUse = false)
        {
            m_bSol1OffUse = bSol1OffUse;
            m_bSol2OffUse = bSol2OffUse;
        }

        public void ForceOff()
        {            
            m_eCmdPos = SenPos.BWD;
            m_ePos = SenPos.UNKNOWN;
            m_twDelay.Restart();            
        }

        public override void Stop()
        {
            m_eCmdPos = m_ePos = SenPos.UNKNOWN;            
            m_twDelay.Restart();            
            m_bOffComplete = true;

            OnOutEvent(m_eOut1, false);
            OnOutEvent(m_eOut2, false);
        }

        public override void Backward()
        {
            if (!m_bSol1OffUse)
                OnOutEvent(m_eOut1, false);

            base.Backward();
        }

        public DO GetSol2()
        {
            return m_eOut2;
        }

        public override void Update()
        {
            if (m_bInitCheck == false)
            {
                if (m_twInitCheck.IsTimeOverMs(1500))
                    m_bInitCheck = true;

                switch (m_eInitPos)
                {
                    case SenPos.NONE:
                        m_eCmdPos = m_ePos;
                        break;
                    case SenPos.FWD:
                        m_eCmdPos = SenPos.FWD;
                        break;
                    case SenPos.BWD:
                        m_eCmdPos = SenPos.BWD;
                        break;
                    default:
                        m_eCmdPos = m_ePos;
                        break;
                }
            }

            if (m_eCmdPos != m_ePos)
            {
                m_bOffComplete = false;

                if (m_eCmdPos == SenPos.FWD)
                {
                    OnOutEvent(m_eOut1, true);
                    OnOutEvent(m_eOut2, false);
                }

                if (m_eCmdPos == SenPos.BWD)
                {
                    OnOutEvent(m_eOut1, false);
                    OnOutEvent(m_eOut2, true);
                }

                if (m_twDelay.IsTimeOverMs(m_lDelayTime))
                {
                    m_ePos = m_eCmdPos;
                    m_twOff.Restart();
                }                
            }
            else
            {
                if (m_eCmdPos == SenPos.FWD)
                {
                    if (m_twOff.IsTimeOverMs((long)m_lOffDelayFwd) && m_bOffComplete == false)
                    {
                        m_bOffComplete = true;
                        if (m_bSol1OffUse)
                            OnOutEvent(m_eOut1, false);
                    }                    
                }
                else if (m_eCmdPos == SenPos.BWD)
                {
                    if (m_twOff.IsTimeOverMs((long)m_lOffDelayBwd) && m_bOffComplete == false)
                    {
                        m_bOffComplete = true;
                        if (m_bSol2OffUse)
                            OnOutEvent(m_eOut2, false);
                    }
                }
            }
        }
    }
}
