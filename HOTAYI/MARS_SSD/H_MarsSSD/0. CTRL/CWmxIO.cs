using System;

#if _WMX3_
namespace H_MarsSSD
{
    public class CWmxIO : CIO
    {
        public CWmxIO()
        {
            CWMX3.wmxIO.GetOutBytes(0x00, MAX_OUTPUT_BUFF_SIZE, ref m_OutData);

            SM.SHM.SetOutputData(TypeConversion.GetUShortArrayFromByteArray(m_OutData));

            Update();
        }

        public override void Update()
        {
            CmdOutputCheck();

            CWMX3.wmxIO.GetInBytes(0x00, MAX_INPUT_BUFF_SIZE, ref m_InData);
            CWMX3.wmxIO.SetOutBytes(0x00, MAX_OUTPUT_BUFF_SIZE, m_OutData);

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
    }
}
#endif