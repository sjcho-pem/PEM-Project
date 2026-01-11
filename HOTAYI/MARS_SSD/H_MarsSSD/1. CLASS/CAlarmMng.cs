using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace H_MarsSSD
{
    #region [ Enum ]    
    
    #endregion

    public class CAlarmMng
    {
        #region [ Variables ]
        private List<ALM> lErr = new List<ALM>();

        #endregion

        public CAlarmMng()
        {
            
        }

        public void ADD(ALM code)
        {
            if (1 > (int)code)
                return;

            if (!lErr.Contains(code))
            {
                lErr.Add(code);

                if (code == ALM.WARNING_REJECT_TRAY_REPLACE)        SM.SHM.SetBitData(BIT.RejectBuzzer, true);
                else if (code > ALM.WARNING_START)			        SM.SHM.SetBitData(BIT.WarningBuzzer, true);
				else									            SM.SHM.SetBitData(BIT.Buzzer, true);
				
				LOGPART ePart = LOGPART.ERR;
				if (code > ALM.WARNING_START) ePart =  LOGPART.WARNING;

                UI.Logs.Write(ePart, (int)code + "," + EnumHelper.GetDescription(code));
                GLog.AddErr(code);
            }
        }

		public void ADD(ALM code, string strErrMsg)
        {
            if (1 > (int)code)
                return;

            if (!lErr.Contains(code))
            {
                lErr.Add(code);

                if (code == ALM.WARNING_REJECT_TRAY_REPLACE)            SM.SHM.SetBitData(BIT.RejectBuzzer, true);
                else if (code > ALM.WARNING_START)                      SM.SHM.SetBitData(BIT.WarningBuzzer, true);
                else                                                    SM.SHM.SetBitData(BIT.Buzzer, true);

                LOGPART ePart = LOGPART.ERR;
				if (code > ALM.WARNING_START) ePart =  LOGPART.WARNING;

                UI.Logs.Write(ePart, (int)code + "," + EnumHelper.GetDescription(code) + "\n" + strErrMsg);
                GLog.AddErr((int)code + "," + EnumHelper.GetDescription(code));
            }
        }

		public void Update()
		{
			if (0 < Cnt())
			{
				lErr.Sort();

				if (lErr[0] < ALM.WARNING_START)
				{
					if (SM.SHM.IsBitData(BIT.Error) == false)
					{
						G.commInspTop.SendAlarmReport((int)lErr[0]);
						G.commInspBtm.SendAlarmReport((int)lErr[0]);

						if (NDm.Read(eNDM.RecentlyAlarmCode) == 0)
						{
							//NDm.Write(eNDM.RecentlyAlarmCode, (int)lErr[0]);
							G.seqUBIGEMComm.AlarmSetStart(lErr[0]);
						}
						SM.SHM.ChangeState(RunStateType.ErrorStop);
					}
					SM.SHM.SetBitData(BIT.Error, true);
				}
				else
				{
					if (lErr.Contains(ALM.WARNING_REJECT_TRAY_REPLACE))		SM.SHM.SetBitData(BIT.RejectWarning, true);
					else													SM.SHM.SetBitData(BIT.Warning, true);
				}

				for (int i = 0; i < lErr.Count; i++)
				{
					if (i >= SHMDEF.ERRCODE_BUF_NUM)
						break;
					SM.SHM.SetErrCodeData(i, (int)lErr[i]);
				}
			}
			else
			{
				SM.SHM.SetBitData(BIT.Error, false);
				SM.SHM.SetBitData(BIT.Warning, false);
				SM.SHM.SetBitData(BIT.RejectWarning, false);

				if (SM.SHM.IsBitData(BIT.Error) == true)
					SM.SHM.ChangeState(RunStateType.NormalStop);
			}
        }

        private int Cnt()
        {
            return (lErr.Count);
        }

        public void Del(ALM code)
        {
            lErr.Remove(code);
            SM.SHM.ErrorInit();
        }

        public void Clear()
        {
            lErr.Clear();
            SM.SHM.ErrorInit();

            //NDm.Write(eNDM.Equipment, (int)MachineState.idle);
            SM.SHM.SetBitData(BIT.Error, false);
            SM.SHM.SetBitData(BIT.Warning, false);
			SM.SHM.SetBitData(BIT.RejectWarning, false);

			G.commInspTop.SendAlarmClear();
			G.commInspBtm.SendAlarmClear();
        }

        public bool Check()
        {
            return 0 < Cnt() ? true : false;
        }

        public ALM Get(int index)
        {
            if (lErr.Count != 0)
                return lErr[index];
            return ALM.NORMAL;
        }

        public bool Find(ALM code)
        {
            if (lErr.Contains(code))
                return (true);
            else
                return (false);
        }



    }
}
