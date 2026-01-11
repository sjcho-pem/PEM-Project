using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace H_MarsSSD
{
    public class CSimulkernelBase
    {
        protected CSimulMotor[] Motors;
        protected CSimulIO IO;

        public virtual void Init()
        {

        }

        #region [ Functions ]    
        public void StateUpdate()
        {            
        }

        public bool IsDevice()
        {                     
            return true;
        }

		public bool IsOnLineAllSlave()
		{
			return true;
		}

        public void ReStartDevice()
        {
            
        }
		
        public void StartHotConnect()
        {            
        }

        public void ReStartEngine()
        {            
        }

		public void ScanNetwork()
        {
        }

		public void StopCommunication()
		{

		}

        public void SetSlaveOP()
        {                   
        }

        public void Dispose()
        {                       
            for (int i = 0; i < Motors.Length; i++)
            {
                Motors[i].Dispose();
            }
        }
        #endregion

        #region Motor 관련        
        protected void InitMotors(int nAxisCnt)
        {
            Motors = new CSimulMotor[nAxisCnt];
            for (int i = 0; i < Motors.Length; i++)
            {
                Motors[i] = new CSimulMotor(i);

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
            IO = new CSimulIO();
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
            IO.AOut(eNum, bOn);
        }
        public void BOut(DO eNum, bool bOn)
        {
            IO.BOut(eNum, bOn);
        }                
        public void AOut(DI eNum, bool bOn)
        {
            IO.AOut(eNum, bOn);
        }

        public void BOut(DI eNum, bool bOn)
        {
            IO.BOut(eNum, bOn);
        }

        public void SetTack(DI eNum, int Tack)
        {
            IO.SetTack(eNum, Tack);
        }

        public int GetTack(DI eNum)
        {
            return IO.GetTack(eNum);
        }

		public int GetOffLineSlaveCount()
		{
			return 0;
		}
        #endregion
    }
}
