using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace H_MarsSSD
{
    public class PStopwatch : Stopwatch
    {
        public bool IsTimeOverMs(long time)
        {
            if (IsRunning == false)
                return false;

            return (time < ElapsedMilliseconds);
        }
    }

    public class CBlinkTimer
    {
        private PStopwatch m_timer = new PStopwatch();
        private bool m_bBlink;

        public CBlinkTimer()
        {
            m_bBlink = false;
        }

        public bool Blink(bool bBlinkOn, int nOnTime = 500, int nOffTime = 500)
        {
            if (false == bBlinkOn)
            {
                m_timer.Reset();
                m_bBlink = true;
                return false;
            }

            if (true == m_bBlink)
            {
                if (false == m_timer.IsTimeOverMs(nOnTime))
                    return true;

                m_timer.Reset();
                m_bBlink = false;

            }
            else
            {
                if (false == m_timer.IsTimeOverMs(nOffTime))
                    return false;

                m_timer.Reset();
                m_bBlink = true;
            }

            return m_bBlink;
        }

		public bool Blink(int nOnTime = 500, int nOffTime = 500)
        {
            if (m_timer.IsRunning == false)			m_timer.Restart();

            if (m_bBlink == true)
            {
                if (false == m_timer.IsTimeOverMs(nOnTime))				return true;

                m_timer.Restart();
                m_bBlink = false;

            }
            else
            {
                if (false == m_timer.IsTimeOverMs(nOffTime))			return false;

                m_timer.Restart();
                m_bBlink = true;
            }

            return m_bBlink;
        }
    };//-----------------------------------------------------------------
}
