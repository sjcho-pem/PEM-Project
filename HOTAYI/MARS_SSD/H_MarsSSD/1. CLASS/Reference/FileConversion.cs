using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Text;

namespace H_MarsSSD
{
    class FileConversion
    {
        public static List<string> ReadLog(string path)
        {
            List<string> lst = new List<string>();
            FileInfo srcfi = new FileInfo(path);
            string dscPath = (@"C:\Temp\LOG\TempLog.log");
            try
            {
//                 if (srcfi.Length > 1000000)
//                 {
//                     System.Windows.Forms.MessageBox.Show("Files are large in size.", "FILE INFO", 
//                         System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
//                     return new List<string>();
//                 }
                srcfi.CopyTo(dscPath, true);

                string[] val = File.ReadAllLines(dscPath);                

                for (int i = 0; i < val.Length; i++)
                    lst.Add(val[i]);

                return lst;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                System.Windows.Forms.MessageBox.Show(ex.Message, "EXCEPTION",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return lst;
            }
        }

        public static List<string> streamReadLog(string path)
        {
            StreamReader sr = new StreamReader(@path, Encoding.Default, true);
            List<string> lst = new List<string>();

            try
            {
                while (sr.Peek() >= 0)
                {
                    string str = sr.ReadLine().Trim();
                    lst.Add(str);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                System.Windows.Forms.MessageBox.Show(ex.Message, "EXCEPTION",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return lst;
            }

            return lst;
        }        
    }


    public class MemMappedFile
    {        
        private long lSize = 0;

        protected const int OFFSET = 1;
        protected int SIZE_ENUM = 0;
        protected int SIZE_STRUCT = 0;
        protected long SIZE_USE_MAP = 0;        
        protected long SIZE_MAP = 0;
        protected long SIZE_IDX = 0;        

        private MemoryMappedFile m_mmf = null;
        private MemoryMappedViewAccessor m_mmva = null;

        protected bool Create(string sName)
        {
            SIZE_USE_MAP = (sizeof(bool) * (SIZE_ENUM + OFFSET));
            SIZE_MAP = (SIZE_STRUCT * (SIZE_ENUM + OFFSET));
            SIZE_IDX = (sizeof(int) * SIZE_ENUM);

            string sUrl = string.Format(Program.DataDirectory + "{0}.dat", sName);

            if (SIZE_MAP <= 0) return false;
            if (string.IsNullOrEmpty(sUrl) == true) return false;
            if (string.IsNullOrEmpty(sName) == true) return false;
            try
            {
                lSize = SIZE_USE_MAP + SIZE_MAP + SIZE_IDX;
                m_mmf = MemoryMappedFile.CreateFromFile(sUrl, FileMode.OpenOrCreate, sName, lSize, MemoryMappedFileAccess.ReadWrite);
                m_mmva = m_mmf.CreateViewAccessor(0, lSize, MemoryMappedFileAccess.ReadWrite);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        protected void Write(long lOffset, byte[] byBuff)
        {
            try
            {
                m_mmva.WriteArray(lOffset, byBuff, 0, byBuff.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        protected void Write(long lOffset, int nVal)
        {
            try
            {
                m_mmva.Write(lOffset, nVal);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        protected void Write(long lOffset, bool bVal)
        {
            try
            {
                m_mmva.Write(lOffset, bVal);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// Read : return=byte array
        /// </summary>
        /// <param name="lOffset"></param>
        /// <param name="nBufSize"></param>
        /// <returns>byte[]</returns>
        protected byte[] Read(long lOffset, int nBufSize)
        {
            try
            {                
                byte[] byBuff = new byte[nBufSize];                
                int nRet = m_mmva.ReadArray(lOffset, byBuff, 0, nBufSize);                

                return byBuff;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// Read : return=bool
        /// </summary>
        /// <param name="lOffset"></param>
        /// <returns>bool</returns>
        protected bool Read(long lOffset)
        {
			if (m_mmva == null)
			{
				GLog.AddErr("Flow File is Size Fail.");
			}
            return m_mmva.ReadBoolean(lOffset);
        }
        /// <summary>
        /// Read : retrun=int
        /// </summary>
        /// <param name="lOffset"></param>
        /// <returns>int</returns>
        protected int IRead(long lOffset)
        {
            return m_mmva.ReadInt32(lOffset);
        }

        protected void ClearAll()
        {
            try
            {
                byte[] byBuff = new byte[m_mmva.Capacity];
                m_mmva.WriteArray(0, byBuff, 0, byBuff.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //public void Clear(long lOffset)
        //{
        //    try
        //    {
        //        //fInfo[nNo].Init();
        //        Write(nNo, fInfo[nNo]);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //}

        //public FlowInfo Pop(int nNo = 0)
        //{
        //    FlowInfo wi = Read(nNo);
        //    Clear(nNo);
        //    return wi;
        //}
    }
}
