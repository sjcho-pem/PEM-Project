using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.MemoryMappedFiles;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections;

namespace H_MarsSSD
{
    //-------------------------------------------------------------------------
    // Double형 dm class(mmf)
    public class DDm
    {
        //---------------------------------------------------------------------
        // Double Dm Define.MAX_DM_NUM
        private const Int32 SIZE = (Define.MAX_DM_NUM * 8);
        private static double[] m_dDm = new double[Define.MAX_DM_NUM];

        private static MemoryMappedFile m_mmf = null;
        private static MemoryMappedViewAccessor m_mmva = null;

        //---------------------------------------------------------------------
        // DDm 초기화...
        public static bool Init()
        {
            //string url = (System.Environment.CurrentDirectory) + "\\NDm.dat";
            string url = Program.DataDirectory + "DDm.dat";
            try
            {
                m_mmf = MemoryMappedFile.CreateFromFile(url, FileMode.OpenOrCreate, "DDm", SIZE, MemoryMappedFileAccess.ReadWrite);
                m_mmva = m_mmf.CreateViewAccessor(0, SIZE, MemoryMappedFileAccess.ReadWrite);
                Update();
            }
            catch (System.Exception e)
            {
                MessageBox.Show(e.ToString() + "Failed to init NDm!!!");
                return (false);
            }
            return (true);
        }

        //---------------------------------------------------------------------
        public static void Update()
        {
            m_mmva.ReadArray(0, m_dDm, 0, m_dDm.Length);
            SM.SHM.SetDDMData(m_dDm);
        }

        public static void Write(eDDM eNo, double dVal)
        {
            Int32 nOffset = (Int32)eNo * 8;
            m_mmva.Write(nOffset, dVal);
            m_dDm[(int)eNo] = dVal;
            SM.SHM.SetDDMData(m_dDm);
        }

        private static double ReadToUpdate(eDDM eNo)
        {
            Int32 nOffset = (Int32)eNo * 8;
            double nVal = m_mmva.ReadDouble(nOffset);
            m_dDm[(int)eNo] = nVal;
            SM.SHM.SetDDMData(m_dDm);
            return (nVal);
        }

        public static double Read(eDDM eNo)
        {
            return (m_dDm[(int)eNo]);
        }

        internal static void Read(eDDM eList, int p)
        {
            throw new NotImplementedException();
        }
    }
}
