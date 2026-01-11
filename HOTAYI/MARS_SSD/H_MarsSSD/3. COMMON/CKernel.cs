using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace H_MarsSSD
{
#if _AJIN_
    public class CKernel : CAjinkernelBase
#elif _WMX3_
    public class CKernel : CWmxkernelBase
#elif _SIMUL_        
    public class CKernel : CSimulkernelBase
#endif
    {
        public override void Init()
        {
            base.Init();

            InitMotors((int)MT.TOTAL);
            InitIO();

            for (int i = 0; i < (int)MT.TOTAL; i++)
            {                
                Motors[i].GetPosDataEvent += new CMotor.GetPosDataEventHandler(SM.SHM.GetMotorPos);
                Motors[i].GetVelDataEvent += new CMotor.GetVelDataEventHandler(SM.SHM.GetMotorVel);
            }
        }

        public void Update()
        {
            StateUpdate();

            if (!IsDevice())
            {
                SM.SHM.SetBitData(BIT.ControlDeviceState, false);
                G.AlmMng.ADD(ALM.ERROR_CONTROL_DEVICE_DISCONECTED);
            }
            else
            {
                SM.SHM.SetBitData(BIT.ControlDeviceState, true);
            }

            IO.Update();

            for (int i = 0; i < Motors.Length; i++)
            {                
                Motors[i].Run();
                SM.SHM.SetMotorState((uint)i, Motors[i].stMt, Motors[i].IsRdy());
            }
        }
    }
}
