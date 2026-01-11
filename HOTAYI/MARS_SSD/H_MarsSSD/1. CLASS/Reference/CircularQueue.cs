using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_MarsSSD
{
    public class CircularQueue<T>
    {
        protected int m_nStIdx;
        protected int m_nEdIdx;
        public T[] m_array;
        protected int m_nSize;
        protected int m_Offset;

        public CircularQueue()
        {
            m_array = null;
            m_nSize = 0;
            m_nStIdx = 0;
            m_nEdIdx = 0;
            m_Offset = 0;
        }

        public bool Alloc(int nSize, int Ofs = 0)
        {
            Free();

            m_Offset = Ofs;
            m_nStIdx = m_nEdIdx = m_Offset;
            m_nSize = nSize;
            m_array = new T[nSize];

            return m_array != null;
        }

        public void Free()
        {
            if(m_array != null)
            {
                m_nStIdx = m_nEdIdx = m_Offset;
                Array.Clear(m_array, 0, m_array.Length);
                m_array = null;
            }
        }

        public int Push(ref T val)
        {
            m_nEdIdx = m_nEdIdx < (m_nSize - 1) ? m_nEdIdx + 1 : m_Offset;

            if (m_nStIdx == m_nEdIdx)
                Pop();

            m_array[m_nEdIdx] = val;
            return m_nEdIdx;
        }

        public ref T Front()
        {
            return ref m_array[m_nStIdx];
        }

        public T[] Ptr()
        {
            return m_array;
        }

        public bool Pop()
        {
            if (m_nStIdx == m_nEdIdx)
                return false;

            m_nStIdx = m_nStIdx < (m_nSize - 1) ? m_nStIdx + 1 : m_Offset;

            return true;
        }

        public T this[int i]
        {
            set
            {
                m_array[i] = value;
            }
            get => m_array[i];
        }

        public void Set(int idx, ref T val)
        {
            m_array[idx] = val;
        }
    }
}
