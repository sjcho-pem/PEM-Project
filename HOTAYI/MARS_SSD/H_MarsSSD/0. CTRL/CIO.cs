using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace H_MarsSSD
{
    public class CIO
    {
        protected const int MAX_INPUT_BUFF_SIZE = 1450;
        protected const int MAX_OUTPUT_BUFF_SIZE = 1450;

        protected byte[] m_InData = new byte[MAX_INPUT_BUFF_SIZE];
        protected byte[] m_OutData = new byte[MAX_OUTPUT_BUFF_SIZE];

        protected int[] m_Tack = Enumerable.Repeat<int>(-1, MAX_INPUT_BUFF_SIZE).ToArray<int>();

        // 체크만 하는 함수
        private bool CheckBit(byte addr, int bit)
        {
            return ((addr >> bit) & 0x01) == 1;
        }

        // output의 경우 해당 함수를 거치면 Output이 변경 되고 이를 update 함수에서 해당 함수로 
        // 변경된 m_OutData를 기반
        protected void OutBit(bool bOn, ref byte addr, int bit)
        {
            addr = bOn ? (addr |= (byte)(0x01 << bit)) : addr &= (byte)~(0x01 << bit);
        }

        protected void OutNBit(bool bOn, ref byte addr, int bit)
        {
            addr = bOn ? addr &= (byte)~(0x01 << bit) : addr |= (byte)(0x01 << bit);
        }

        public virtual void Update() { }

        public bool IsAIn(DI eNum)
        {
            return CheckBit(m_InData[(int)eNum / 8], (int)eNum % 8);
        }

        public bool IsBIn(DI eNum)
        {
            return !CheckBit(m_InData[(int)eNum / 8], (int)eNum % 8);
        }

        public bool IsAOut(DO eNum)
        {
            return CheckBit(m_OutData[(int)eNum / 8], (int)eNum % 8);
        }

        public bool IsBOut(DO eNum)
        {
            return CheckBit(m_OutData[(int)eNum / 8], (int)eNum % 8);
        }

        public void AOut(DO eNum, bool bOn)
        {
            OutBit(bOn, ref m_OutData[(int)eNum / 8], (int)eNum % 8);
        }

        public void BOut(DO eNum, bool bOn)
        {
            OutNBit(bOn, ref m_OutData[(int)eNum / 8], (int)eNum % 8);
        }

        public byte[] GetInputByte()
        {
            return m_InData;
        }

        public byte[] GetOutputByte()
        {
            return m_OutData;
        }

        public void SetTack(DI eNum, int tackTime)
        {
            m_Tack[(int)eNum] = tackTime;
        }

        public int GetTack(DI eNum)
        {
            return m_Tack[(int)eNum];
        }
    }
}
