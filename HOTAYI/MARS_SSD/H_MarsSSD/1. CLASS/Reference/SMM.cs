using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace H_MarsSSD
{
    public enum InState
    {
        none,
        run,
        stop,
    };

    /// <summary>
    /// State Machine Manager
    /// </summary>
    public class SMM<TStep>
    {        
        public InState eInput;

        private TStep m_ResetStep;
        private TStep m_CurStep;
        public TStep CurStep { get => m_CurStep; }             
        private bool m_bChange = true;
        
        private PStopwatch twDelay;
        private PStopwatch twTact = new PStopwatch();

        private long lLastTact = 0;
        private string m_sAppendLog = "";

        public bool IsReady { get => m_ResetStep.Equals(m_CurStep) & (eInput != InState.run); }
        public long DelayTime { get => twDelay.ElapsedMilliseconds; }
        public long DelayTick { get => twDelay.ElapsedTicks; }
        public long Tact { get => twTact.ElapsedMilliseconds; }
        public long LastTact { get => lLastTact; }
        public string AppendLog { get => m_sAppendLog; }

        public SMM(TStep eStep, string sMsg = "")
        {
            eInput = InState.none;

            m_ResetStep = m_CurStep = eStep;

            m_sAppendLog = sMsg;
            
            twDelay = new PStopwatch();
            twDelay.Stop();            
        }

        public void Next(TStep eStep)
        {
            if (CurStep.Equals(eStep) == true)
                return;

            m_CurStep = eStep;
            m_bChange = true;                       
            GLog.AddSeq(m_sAppendLog + " - Step : " + eStep.ToString());

            if (m_ResetStep.Equals(eStep) == true)
            {
                twTact.Stop();
                GLog.AddSeq(string.Format("{0} Tact : {1} msec", m_sAppendLog, twTact.ElapsedMilliseconds));
            }
            else if (twTact.IsRunning == false)
            {
                lLastTact = twTact.ElapsedMilliseconds;
                twTact.Restart();
            }
        }

        public void ResetStep()
        {
            m_CurStep = m_ResetStep;
            twTact.Stop();
        }

        public bool Delayms(int nmsec)
        {
            if (m_bChange)
            {
                m_bChange = false;
                twDelay.Restart();
            }

            return twDelay.IsTimeOverMs(nmsec);
        }

        public void ResetDelayms()
        {
            m_bChange = true;
        }
    }
}
