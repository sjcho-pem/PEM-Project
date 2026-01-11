using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace H_MarsSSD
{
    public class LampBuzzer
    {
        private enum Cmd
        {
            START     = 10,
            CHECK,

            //LAMP
            RED,
            YELLOW,
            GREEN,
            LAMPOFF,

            //Buzzer
            ERR_CHECK,
            RUN_CHECK1,
            RUN_CHECK2,
			
            BUZZER_CHECK,

            END       = 199,
        }
		
        private SMM<Cmd> m_smmLamp			= new SMM<Cmd>(Cmd.END, "LAMP PROCESS");
        private SMM<Cmd> m_smmBuzzer		= new SMM<Cmd>(Cmd.END, "BUZZER PROCESS");
        private SMM<Cmd> m_smmRejectLamp	= new SMM<Cmd>(Cmd.END, "REJECT LAMP PROCESS");
        private SMM<Cmd> m_smmRejectBuzzer	= new SMM<Cmd>(Cmd.END, "REJECT BUZZER PROCESS");

        private CBlinkTimer tmBlink = new CBlinkTimer();
        private bool bErr, bWar, bRun, bPause, bBuzzer,bRejectWar,bRejectBuzzer;        

        public void Init()
        {
            StartLamp();
            StartBuzzer();
			StartRejectLamp();
			StartRejectBuzzer();
        }
		
        public void StartLamp() 		{ m_smmLamp.eInput = InState.run; }
        public void StartBuzzer() 		{ m_smmBuzzer.eInput = InState.run; }
        public void StartRejectLamp() 	{ m_smmRejectLamp.eInput = InState.run; }
        public void StartRejectBuzzer() { m_smmRejectBuzzer.eInput = InState.run; }
        public void StopBuzzer() 		{ m_smmBuzzer.Next(Cmd.END); }

        /// <summary>
        /// Run Process 
        /// </summary>
        public void Run()
        {
			try
			{
				bErr = SM.SHM.IsBitData(BIT.Error);
				bWar = SM.SHM.IsBitData(BIT.Warning);
				bRun = SM.SHM.IsBitData(BIT.AutoRun);
				bPause = SM.SHM.IsBitData(BIT.Pause);
				bBuzzer = SM.SHM.IsBitData(BIT.Buzzer) || SM.SHM.IsBitData(BIT.WarningBuzzer);
				bRejectWar = SM.SHM.IsBitData(BIT.RejectWarning);
				bRejectBuzzer = SM.SHM.IsBitData(BIT.RejectBuzzer);
				
				if (bErr == false  &&  bWar == false  &&  bBuzzer == true)			SM.SHM.SetBitData(BIT.Buzzer, false);
				if (bWar == false  &&  bBuzzer == true)								SM.SHM.SetBitData(BIT.WarningBuzzer, false);
				if (bRejectWar == false )											SM.SHM.SetBitData(BIT.RejectBuzzer, false);
				
				Buzzer();
				Lamp();
				RejectBuzzer();
				RejectLamp();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
        }

        /// <summary>
        /// Lamp Process
        /// </summary>
        private void Lamp()
        {
			try
			{
				switch (m_smmLamp.CurStep)
				{
				    case Cmd.START:
				        {
				            if (m_smmLamp.eInput == InState.run)
				            {
				                m_smmLamp.eInput = InState.none;
				                m_smmLamp.Next(Cmd.CHECK);
				            }
				        }
				        break;
				    case Cmd.CHECK:
				        {
				            if (bErr)
				            {
				                LampProc(Cmd.RED, true);
				                if (m_smmBuzzer.CurStep == Cmd.START)
				                    StartBuzzer();                            
				            }
				            else if (bWar || bPause)
				                LampProc(Cmd.YELLOW, true);
				            else if (bRun)
				            {
				                LampProc(Cmd.GREEN, false);
				                if (m_smmBuzzer.CurStep == Cmd.START)                            
				                    StartBuzzer();                            
				            }
				            else
				                LampProc(Cmd.LAMPOFF);
				        }
				        break;
				    case Cmd.END:
				        {
				            m_smmLamp.Next(Cmd.START);
				        }
				        break;
				}   
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
        }

        private void Buzzer()
        {
			try
			{
				switch (m_smmBuzzer.CurStep)
				{
				    case Cmd.START:
				        {
				            if (m_smmBuzzer.eInput == InState.run)
				            {
				                m_smmBuzzer.eInput = InState.none;
				                m_smmBuzzer.Next(Cmd.CHECK);
				            }                                           
				        }
				        break;
				    case Cmd.CHECK:
				        {                        
				            if (bBuzzer == true)
				            {
				                BuzzerProc();
				                m_smmBuzzer.Next(Cmd.BUZZER_CHECK);
				            }
				        }                    
				        break;
							
				    case Cmd.BUZZER_CHECK:
				        {                   
				            if (bBuzzer == true)
							{
								BuzzerBlink();
								break;
							}
								
				            BuzzerProc(false);
								
				            m_smmBuzzer.Next(Cmd.CHECK);
				        }
				        break;
				    case Cmd.END:
				        BuzzerProc(false);
				        m_smmBuzzer.Next(Cmd.START);
				        break;
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
        }

        private void LampProc(Cmd LampFlag, bool bBlinkFlag = false)
        {
			try
			{
				bool bBlink = tmBlink.Blink(500, 500);

				switch (LampFlag)
				{
				    case Cmd.RED:
				        if (bBlinkFlag)
				            G.cKernel.AOut(DO.LampRed, bBlink);
				        else
				            G.cKernel.AOut(DO.LampRed, true);
				        G.cKernel.AOut(DO.LampYellow, false);
				        G.cKernel.AOut(DO.LampGreen, false);
				        break;
				    case Cmd.YELLOW:
				        if (bBlinkFlag)
				            G.cKernel.AOut(DO.LampYellow, bBlink);
				        else
				            G.cKernel.AOut(DO.LampYellow, true);
				        G.cKernel.AOut(DO.LampRed, false);
				        G.cKernel.AOut(DO.LampGreen, false);
				        break;
				    case Cmd.GREEN:
				        if (bBlinkFlag)
				            G.cKernel.AOut(DO.LampGreen, bBlink);
				        else
				            G.cKernel.AOut(DO.LampGreen, true);
				        G.cKernel.AOut(DO.LampRed, false);
				        G.cKernel.AOut(DO.LampYellow, false);
				        break;
				    case Cmd.LAMPOFF:
				        G.cKernel.AOut(DO.LampRed, false);
				        G.cKernel.AOut(DO.LampYellow, false);
				        G.cKernel.AOut(DO.LampGreen, false);
				        break;
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
        }

        private void BuzzerProc(bool bFlag = true)
        {
			try
			{
				int nalarm_no	= (int)G.AlmMng.Get(0);
				if (nalarm_no == 0)
				{
					G.cKernel.AOut(DO.Buzzer, false);
					return;
				}

				int nBuzz = SM.SHM.GetBuzzerData()[nalarm_no-1];

				switch(nBuzz)
				{
				    case 0:                    
				        G.cKernel.AOut(DO.Buzzer, bFlag);
				        break;
				    case 1:                    
				        G.cKernel.AOut(DO.Buzzer, bFlag);
				        break;
				//    case 2:
				//        G.cKernel.AOut(DO.Buzzer2, bFlag);
				//        break;
				//    case 3:
				//        G.cKernel.AOut(DO.Buzzer3, bFlag);
				//        break;
				//    case 4:
				//        G.cKernel.AOut(DO.Buzzer4, bFlag);
				//        break;
				    default:
				        G.cKernel.AOut(DO.Buzzer, false);
				//        G.cKernel.AOut(DO.Buzzer2, false);
				//        G.cKernel.AOut(DO.Buzzer3, false);
				//        G.cKernel.AOut(DO.Buzzer4, false);
				        break;
				}

				if (bFlag == false)			psw.Stop();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
        }

		private	PStopwatch	psw	= new PStopwatch();

		private	void	BuzzerBlink()
		{
			try
			{
				int nalarm_no	= (int)G.AlmMng.Get(0);
				if (nalarm_no == 0)
				{
					G.cKernel.AOut(DO.Buzzer, false);
					return;
				}

				int nBuzz = SM.SHM.GetBuzzerData()[nalarm_no-1];

				switch(nBuzz)
				{
				    case 1:
						if		(psw.IsRunning == false)
						{
							psw.Restart();
							break;
						}
						else if (G.cKernel.IsAOut(DO.Buzzer) == true  &&
								psw.IsTimeOverMs(1000) == true)
						{
							psw.Restart();
							G.cKernel.AOut(DO.Buzzer, false);
						}
						else if (G.cKernel.IsAOut(DO.Buzzer) == false  &&
								psw.IsTimeOverMs(5000) == true)
						{
							psw.Restart();
							G.cKernel.AOut(DO.Buzzer, true);
						}
				        break;

				    default:
				        break;
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		
        private void RejectLamp()
        {
			try
			{
				switch (m_smmRejectLamp.CurStep)
				{
				    case Cmd.START:
				        {
				            if (m_smmRejectLamp.eInput == InState.run)
				            {
				                m_smmRejectLamp.eInput = InState.none;
				                m_smmRejectLamp.Next(Cmd.CHECK);
				            }
				        }
				        break;
				    case Cmd.CHECK:
				        {
				            if (bRejectWar)
				            {
								G.cKernel.AOut(DO.RejectLampRed, true);
				                if (m_smmBuzzer.CurStep == Cmd.START)
				                    StartRejectBuzzer();                            
				            }
				            else
								G.cKernel.AOut(DO.RejectLampRed, false);
				        }
				        break;
				    case Cmd.END:
				        {
				            m_smmRejectLamp.Next(Cmd.START);
				        }
				        break;
				}   
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
        }

        private void RejectBuzzer()
        {
			try
			{
				switch (m_smmRejectBuzzer.CurStep)
				{
				    case Cmd.START:
				        {
				            if (m_smmRejectBuzzer.eInput == InState.run)
				            {
				                m_smmRejectBuzzer.eInput = InState.none;
				                m_smmRejectBuzzer.Next(Cmd.CHECK);
				            }                                           
				        }
				        break;
				    case Cmd.CHECK:
				        {                        
				            if (bRejectBuzzer == true)
				            {
				                G.cKernel.AOut(DO.RejectBuzzer, true);
				                m_smmRejectBuzzer.Next(Cmd.BUZZER_CHECK);
				            }
				        }                    
				        break;
							
				    case Cmd.BUZZER_CHECK:
				        {                   
				            if (bRejectBuzzer == true)	break;
					
							G.cKernel.AOut(DO.RejectBuzzer, false);
								
				            m_smmRejectBuzzer.Next(Cmd.CHECK);
				        }
				        break;
				    case Cmd.END:
				        BuzzerProc(false);
				        m_smmRejectBuzzer.Next(Cmd.START);
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
