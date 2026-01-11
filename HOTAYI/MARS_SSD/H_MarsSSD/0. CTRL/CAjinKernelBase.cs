using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace H_MarsSSD
{
    public class CAjinkernelBase
    {
        protected CAjinMotor[] Motors;
        protected CAjinIO IO;

        public virtual void Init()
        {
            CAjin.InitializeFunc();
        }

        #region [ Functions ]    
        public void StateUpdate()
        {            
        }

        public bool IsDevice()
        {
            CAjin.IsAjinDevice();            
            return true;
        }

        public void ReStartDevice()
        {
            CAjin.ReDeviceOpen();            
        }

        public void StartHotConnect()
        {            
        }

        public void SetSlaveOP()
        {            
        }

        public void Dispose()
        {
            CAjin.Close();
        }
        #endregion

        #region Motor 관련        
        protected void InitMotors(int nAxisCnt)
        {
            Motors = new CAjinMotor[nAxisCnt];
            for (int i = 0; i < Motors.Length; i++)
            {
                Motors[i] = new CAjinMotor(i);

                Motors[i].AlarmReset();

                Thread.Sleep(10);

                Motors[i].SetServoOn(true);
            }
        }

        public CMotor Motor(MT eNum)
        {
            return Motors[(int)eNum];
        }

        public void MotorHomeCancle()
        {
            for (int i = 0; i < Motors.Length; i++)
            {
                Motors[i].ClearHome();
            }
        }

        public bool IsAllHome()
        {
            bool bHome = true;

            for (MT eNum = 0; eNum < MT.TOTAL; eNum++)
                bHome &= Motor(eNum).IsHomeDone();

            return bHome;
        }

        public void MotorVelocityPercentUpdate(VelocityRate eMcSpeed)
        {
            float fNum = 1;
            switch (eMcSpeed)
            {
                case VelocityRate.P100: break;
                case VelocityRate.P80: fNum = 0.8f; break;
                case VelocityRate.P50: fNum = 0.5f; break;
                case VelocityRate.P30: fNum = 0.3f; break;
                case VelocityRate.P10: fNum = 0.1f; break;
            }
            for (int i = 0; i < (int)MT.TOTAL; i++)
            {
                Motors[i].VelPercent = fNum;
            }
        }
        #endregion

        #region IO 관련  
        protected void InitIO()
        {
            IO = new CAjinIO();
        }

        public bool IsAIn(DI eNum)
        {
            return IO.IsAIn(eNum);
        }

        public bool IsBIn(DI eNum)
        {
            return IO.IsBIn(eNum);
        }

        public bool IsAOut(DO eNum)
        {
            return IO.IsAOut(eNum);
        }

        public bool IsBOut(DO eNum)
        {
            return IO.IsBOut(eNum);
        }

        public void AOut(DO eNum, bool bOn)
        {
            if (CAjin.bNoDevice == true)
                return;
            IO.AOut(eNum, bOn);
            CAXD.AxdoWriteOutport((int)eNum, Convert.ToUInt32(bOn));
        }

        public void BOut(DO eNum, bool bOn)
        {
            if (CAjin.bNoDevice == true)
                return;
            IO.BOut(eNum, bOn);
            CAXD.AxdoWriteOutport((int)eNum, Convert.ToUInt32(bOn));
        }
                
        #region Simulation Input Out
        public void AOut(DI eNum, bool bOn) { return; }
        public void BOut(DI eNum, bool bOn) { return; }
        #endregion

        public void SetTack(DI eNum, int Tack)
        {
            IO.SetTack(eNum, Tack);
        }

        public int GetTack(DI eNum)
        {
            return IO.GetTack(eNum);
        }
        #endregion
    }
}
