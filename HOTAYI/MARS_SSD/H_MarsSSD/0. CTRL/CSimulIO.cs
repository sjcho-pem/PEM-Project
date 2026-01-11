using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace H_MarsSSD
{
    public class CSimulIO : CIO
    {
        public CSimulIO()
        {                
            SM.SHM.SetOutputData(TypeConversion.GetUShortArrayFromByteArray(m_OutData));

            Update();
        }

        public override void Update()
        {
            CmdOutputCheck();
            CmdInputCheck();

            SM.SHM.SetInputData(TypeConversion.GetUShortArrayFromByteArray(m_InData));
            SM.SHM.SetOutputData(TypeConversion.GetUShortArrayFromByteArray(m_OutData));
        }      

        private void CmdOutputCheck()
        {
            DO dOut = DO.Total;
            if (SM.SHM.IsUiOutputChange(ref dOut) == true)
            {
                AOut(dOut, !IsAOut(dOut));
            }
        }

        private void CmdInputCheck()
        {
            DI dOut = DI.Total;
            if (SM.SHM.IsUiInputChange(ref dOut) == true)
            {
                AOut(dOut, !IsAIn(dOut));
            }                
        }

        public void AOut(DI eNum, bool bOn)
        {
            OutBit(bOn, ref m_InData[(int)eNum / 8], (int)eNum % 8);
        }

        public void BOut(DI eNum, bool bOn)
        {
            OutNBit(bOn, ref m_InData[(int)eNum / 8], (int)eNum % 8);
        }
    }
   



}
