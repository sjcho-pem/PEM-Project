using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace H_MarsSSD
{
    public class CAjinIO : CIO
    {
        // -- AXT MODULE -- //
        // AXT_MODULE 종류에 따라 접점 개수 변경해서 사용!!
        // Module Slave 개수에 따라 Module Cnt 변경(링크 순서에 따라 적용!!)
        // AXT_SIO_DB32P or AXT_SIO_DB32T : Input 16, Output 16
        // AXT_SIO_DI32, AXT_SIO_DO32P or AXT_SIO_DO32T : Input 32, Output 32
        protected const int MAX_INPUT_COUNT = 16;
        protected const int MAX_OUTPUT_COUNT = 16;

        public CAjinIO()
        {
            if (CAjin.bNoDevice == true)
                return;        
            ReadOutput();

            SM.SHM.SetOutputData(TypeConversion.GetUShortArrayFromByteArray(m_OutData));

            Update();
        }

        public override void Update()
        {
            CmdOutputCheck();

            if (CAjin.bNoDevice == true)
                return;

            ReadInput();
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
        private void SetInputData(bool bOn, int nNo)
        {
            if (bOn == true)
                m_InData[nNo / 8] = m_InData[nNo / 8] |= (byte)(0x01 << (nNo % 8));
            else
                m_InData[nNo / 8] = m_InData[nNo / 8] &= (byte)~(0x01 << (nNo % 8));                        
        }

        private void SetOutputData(bool bOn, int nNo)
        {
            if (bOn == true)
                m_OutData[nNo / 8] = m_OutData[nNo / 8] |= (byte)(0x01 << (nNo % 8));
            else
                m_OutData[nNo / 8] = m_OutData[nNo / 8] &= (byte)~(0x01 << (nNo % 8));           
        }


        private void ReadOutput()
        {
            int nModuleCnt = 0;
            CAjin.CheckAjinErr(CAXD.AxdInfoGetModuleCount(ref nModuleCnt));                
            if (nModuleCnt == 0) return;

            for (int i = 0; i < MAX_OUTPUT_COUNT; i++)
            {
                uint nVal = 0;
                CAXD.AxdiReadInport(i, ref nVal);
                SetOutputData(Convert.ToBoolean(nVal), i);
            }
        }

        private void ReadInput()
        {
            int nModuleCnt = 0;
            CAjin.CheckAjinErr(CAXD.AxdInfoGetModuleCount(ref nModuleCnt));
            if (nModuleCnt == 0) return;

            for (int i = 0; i < MAX_INPUT_COUNT; i++)
            {
                uint nVal = 0;
                CAXD.AxdiReadInport(i, ref nVal);                                
                SetInputData(Convert.ToBoolean(nVal), i);
            }
        }
    }
   



}
