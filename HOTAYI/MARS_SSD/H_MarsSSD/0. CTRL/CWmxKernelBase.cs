using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

#if _WMX3_
namespace H_MarsSSD
{
    public class CWmxkernelBase
    {
        protected CWmxMotor[] Motors;
        protected CWmxIO IO;

        protected List<Actuator> arrCyl;
        protected List<Actuator> arrMt;
        protected List<Actuator> arrVac;

        public virtual void Init()
        {
            CWMX3.InitializeFunc();

            arrCyl = new List<Actuator>();
            arrMt = new List<Actuator>();
            arrVac = new List<Actuator>();
        }

        #region [ WMX Function ]    
        public void StateUpdate()
        {
            CWMX3.StateUpdate();
        }

        public bool IsDevice()
        {
            return CWMX3.IsWMXDevice();
        }

		public bool IsOnLineAllSlave()
		{
			return CWMX3.IsOnlineAllSlave();
		}

        public void ReStartDevice()
        {
            CWMX3.ReStartDevice();
        }
        public void ReStartEngine()
        {
            CWMX3.ReStartEngine();
        }

        public void StartHotConnect()
        {
            CWMX3.StartHotConnect();
        }

        public void ScanNetwork()
        {
            CWMX3.ScanNetwork();
        }

		public void StopCommunication()
		{
			CWMX3.StopCommunication();
		}

        public void SetSlaveOP()
        {
            CWMX3.SetSlaveOP();
        }

		public int GetOffLineSlaveCount()
		{
			return CWMX3.GetOffLineSlaveCount();
		}

        public void Dispose()
        {
            ;
        }
        #endregion

        #region Motor 관련

        protected void InitMotors(int nAxisCnt)
        {
            Motors = new CWmxMotor[nAxisCnt];
            for (int i = 0; i < Motors.Length; i++)
            {
                Motors[i] = new CWmxMotor(i);

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
            IO = new CWmxIO();
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
#endif