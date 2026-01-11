using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.MemoryMappedFiles;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace H_MarsSSD
{
    //-------------------------------------------------------------------------
    // Sr형 dm class(mmf)
    public class SDm
    {
        //---------------------------------------------------------------------
        // String Dm Define.MAX_DM_NUM
        private static	string[]	m_sDm		= new string[SHMDEF.DM_BUF_NUM];
		private	static	string		m_url		= Program.DataDirectory +"SDm.dat";
		private	static	object		m_FileLock	= new object();
		private	static	bool		m_bInit		= false;

        //---------------------------------------------------------------------
        // DDm 초기화...
        public static bool Init()
        {   
            try
            {
                Update(m_url);
				m_bInit	= true;
            }
            catch (System.Exception e)
            {
                MessageBox.Show(e.ToString() + "Failed to init SDm!!!");
                return (false);
            }
            return (true);
        }

        //---------------------------------------------------------------------
        public static void Update(string strFilePath)
        {
            SDmDataOpen(strFilePath);
        }

        public static void Write(eSDM eNo, string strVal)
        {
			if (m_bInit == false)			Init();

            m_sDm[(int)eNo] = strVal;

			SDmDataSave(m_url);
        }

        public static string Read(eSDM eNo)
        {
			if (m_bInit == false)			Init();

			if (m_sDm[(int)eNo] == null)			m_sDm[(int)eNo]	= string.Empty;

            return m_sDm[(int)eNo];
        }



		private	static	void	SDmDataSave(string strFileName)
		{
			lock (m_FileLock)
			{
				FileStream	fs	= new FileStream(strFileName, FileMode.Create);
				if (fs == null)			return;
				try
				{
					BinaryFormatter	Formatter	= new BinaryFormatter();
					Formatter.Serialize(fs, m_sDm);
				}
				catch
				{
					fs.Close();
				}
				fs.Close();
			}
		}

		private	static	void	SDmDataOpen(string strFileName)
		{
			FileInfo	fi	= new FileInfo(strFileName);
			if (fi.Exists == false)
			{
				for (int i = 0; i < m_sDm.Length; i++)
				{
					m_sDm[i]	= string.Empty;
				}
				return;
			}

			lock (m_FileLock)
			{
				FileStream	fs	= new FileStream(strFileName, FileMode.Open);
				try
				{
					BinaryFormatter	Formatter	= new BinaryFormatter();
					m_sDm						= (string[])Formatter.Deserialize(fs);
				}
				catch
				{
					fs.Close();
					File.Delete(strFileName);	//파일이 망가져 읽지 못한 경우 삭제
				}
				fs.Close();
			}
		}
    }
}
