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
    // int형 dm class(mmf)
    public class NDm
    {
        //---------------------------------------------------------------------
        // int Dm Define.MAX_DM_NUM
        private const Int32 SIZE = (Define.MAX_DM_NUM * 4);
        private static int[] m_nDm = new int[SHMDEF.DM_BUF_NUM];

        private static MemoryMappedFile m_mmf = null;
        private static MemoryMappedViewAccessor m_mmva = null;

        //---------------------------------------------------------------------
        // NDm 초기화...
        public static bool Init()
        {
            //string url = (System.Environment.CurrentDirectory) + "\\NDm.dat";
            string url = Program.DataDirectory + "NDm.dat";
            try
            {
                m_mmf = MemoryMappedFile.CreateFromFile(url, FileMode.OpenOrCreate, "NDm", SIZE, MemoryMappedFileAccess.ReadWrite);
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
            m_mmva.ReadArray(0, m_nDm, 0, m_nDm.Length);
            SM.SHM.SetNDMData(m_nDm);
        }

        public static void Write(eNDM eNo, Int32 nVal)
        {
			if (eNo == eNDM.AutoRunMode)
			{
				if ((NDm.Read(eNDM.AutoRunMode) == (int)AutoRunMode.EndLot &&  (AutoRunMode)nVal == AutoRunMode.QuitLot) ||
					(NDm.Read(eNDM.AutoRunMode) == (int)AutoRunMode.QuitLot &&  (AutoRunMode)nVal == AutoRunMode.EndLot)  )
				{
					string strMsg = $"You can't change the AutoRunmode from EndLot to QuitLot or from QuitLot to EndLot  {Environment.NewLine} " +
									$"You have to empty all the products in that mode";
					UI.Logs.Write(LOGPART.WARNING, strMsg);
					SM.SHM.SetMsg(SeqMsg.SEQERR, strMsg);
					SM.SHM.MsgShow(SeqMsg.SEQERR);
					return;
				}
				GLog.Add($"Auto Run Mode Change -> {(AutoRunMode)nVal}");
			}
			if (eNo == eNDM.FillDoneRemainCount)	GLog.Add($"FillDoneRemainCount -> {nVal}");
            if (eNo == eNDM.LotEndMode)             GLog.Add($"LotEndMode -> {(LotEndMode)nVal}");

            Int32 nOffset = (Int32)eNo * 4;
            m_mmva.Write(nOffset, nVal);
            m_nDm[(int)eNo] = nVal;
            SM.SHM.SetNDMData(m_nDm);
        }
		
        private static Int32 ReadToUpdate(eNDM eNo)
        {
            Int32 nOffset = (Int32)eNo * 4;
            Int32 nVal = m_mmva.ReadInt32(nOffset);
            m_nDm[(int)eNo] = nVal;
            SM.SHM.SetNDMData(m_nDm);
            return (nVal);
        }

        public static Int32 Read(eNDM eNo)
        {
            return (m_nDm[(int)eNo]);
        }

        internal static void Read(eNDM eList, int p)
        {
            throw new NotImplementedException();
        }

		public static void Copy(eNDM eNoSrc, eNDM eNoDest)
        {
			Write(eNoDest, Read(eNoSrc));
        }

		public static void Move(eNDM eNoSrc, eNDM eNoDest)
        {
			Write(eNoDest, Read(eNoSrc));
			Write(eNoSrc, 0);
        }

		
    }
}
