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
using System.Windows;

namespace H_MarsSSD
{
	[Serializable]
	public	struct	tsGapSize
	{
		public	List<double>	ColsGap;
		public	List<double>	RowsGap;

		public	void	Init()
		{
			ColsGap	= new List<double>();
			RowsGap	= new List<double>();
		}
	}

	[Serializable]
	public	class	tcTrayPnPInfo
	{
		public	tsGapSize	Tray;
		public	tsGapSize	LoadPicker;
		public	tsGapSize	UnloadPicker;
		public	tsGapSize	SortPicker;

		public	List<string>	UsedRowLdPnP;
		public	List<string>	UsedRowUldPnP;
		public	List<string>	UsedRowSortPnP;

		public	tcTrayPnPInfo()
		{
			Tray.Init();
			LoadPicker.Init();
			UnloadPicker.Init();
			SortPicker.Init();

			UsedRowLdPnP	= new List<string>();
			UsedRowUldPnP	= new List<string>();
			UsedRowSortPnP	= new List<string>();
		}
	}

    public class TrayPnPRecipe
    {
        private	static	tcTrayPnPInfo	m_PnPinfo	= new tcTrayPnPInfo();
		private	static	object			m_FileLock	= new object();

		private	static	Dictionary<Point, Point>	m_dicTrayPos	= new Dictionary<Point, Point>();

		private	static	Dictionary<int, Point>	m_dicLdPickerPos	= new Dictionary<int, Point>();
		private	static	Dictionary<int, Point>	m_dicUldPickerPos	= new Dictionary<int, Point>();
		private	static	Dictionary<int, Point>	m_dicSortPickerPos	= new Dictionary<int, Point>();

		private	static	Dictionary<int, List<int>>	m_dicUseRowLdPnp	= new Dictionary<int, List<int>>();
		private	static	Dictionary<int, List<int>>	m_dicUseRowUldPnp	= new Dictionary<int, List<int>>();
		private	static	Dictionary<int, List<int>>	m_dicUseRowSortPnp	= new Dictionary<int, List<int>>();

		public	static	tcTrayPnPInfo				PnPInfo			{	get => m_PnPinfo;	}
		public	static	Dictionary<Point, Point>	TrayPos			{	get => m_dicTrayPos;	}
		public	static	Dictionary<int, Point>		LdPickerPos		{	get => m_dicLdPickerPos;	}
		public	static	Dictionary<int, Point>		UldPickerPos	{	get => m_dicUldPickerPos;	}
		public	static	Dictionary<int, Point>		SortPickerPos	{	get => m_dicSortPickerPos;	}
		public	static	Dictionary<int, List<int>>	UseRowLdPnp		{	get => m_dicUseRowLdPnp;	}
		public	static	Dictionary<int, List<int>>	UseRowUldPnp	{	get => m_dicUseRowUldPnp;	}
		public	static	Dictionary<int, List<int>>	UseRowSortPnp	{	get => m_dicUseRowSortPnp;	}




		public	static	void		FileSave(string strFileName, tcTrayPnPInfo PnPInfo)
		{
			try
			{
				lock (m_FileLock)
				{
					FileStream	fs	= new FileStream(strFileName, FileMode.Create);
					if (fs == null)			return;
					try
					{
						m_PnPinfo	= PnPInfo;

						BinaryFormatter	Formatter	= new BinaryFormatter();
						Formatter.Serialize(fs, m_PnPinfo);

						CalPos();

					}
					catch
					{
						fs.Close();
					}
					fs.Close();
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return ;
			}
		}

		public	static	tcTrayPnPInfo	FileOpen(string strFileName)
		{
			try
			{
				FileInfo	fi	= new FileInfo(strFileName);
			
				if (fi.Exists == false)			return null;

				lock (m_FileLock)
				{
					FileStream	fs	= new FileStream(strFileName, FileMode.Open);
					try
					{
						BinaryFormatter	Formatter	= new BinaryFormatter();
						m_PnPinfo					= (tcTrayPnPInfo)Formatter.Deserialize(fs);
					}
					catch
					{
						fs.Close();
						File.Delete(strFileName);	//파일이 망가져 읽지 못한 경우 삭제
					}
					fs.Close();
				}

				CalPos();

				return m_PnPinfo;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return null;
			}
		}

		private	static	void		CalPos()
		{
			try
			{
				CalTrayPos();
				CalPickerPos(m_PnPinfo.LoadPicker, ref m_dicLdPickerPos);
				CalPickerPos(m_PnPinfo.UnloadPicker, ref m_dicUldPickerPos);
				CalPickerPos(m_PnPinfo.SortPicker, ref m_dicSortPickerPos);

				CalUseRowPicker(2, m_PnPinfo.UsedRowLdPnP, ref m_dicUseRowLdPnp);
				CalUseRowPicker(2, m_PnPinfo.UsedRowUldPnP, ref m_dicUseRowUldPnp);
				CalUseRowPicker(4, m_PnPinfo.UsedRowSortPnP, ref m_dicUseRowSortPnp);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	static	void		CalTrayPos()
		{
			try
			{
				Point	ptpocket;
				Point	ptpos;

				int		nsum_x	= 0;
				int		nsum_y	= 0;


				m_dicTrayPos.Clear();

				for (int nrow = 0; nrow < m_PnPinfo.Tray.RowsGap.Count; nrow++)
				{
					nsum_y	+= (int)(m_PnPinfo.Tray.RowsGap[nrow]*1000);
					nsum_x	= 0;
					
					for (int ncol = 0; ncol < m_PnPinfo.Tray.ColsGap.Count; ncol++)
					{
						nsum_x	+= (int)(m_PnPinfo.Tray.ColsGap[ncol]*1000);

						ptpocket	= new Point(ncol, nrow);
						ptpos		= new Point(nsum_x, nsum_y);

						m_dicTrayPos.Add(ptpocket, ptpos);
					}
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));

			}
		}

		private	static	void		CalPickerPos(tsGapSize GapSize, ref Dictionary<int, Point>	dicPickerPos)
		{
			try
			{
				Point	ptpos;

				int		nsum_x		= 0;
				int		nsum_y		= 0;
				int		npicker_no	= 0;

				dicPickerPos.Clear();

				for (int nrow = 0; nrow < GapSize.RowsGap.Count; nrow++)
				{
					nsum_y	+= (int)(GapSize.RowsGap[nrow]*1000);
					nsum_x	= 0;
					
					for (int ncol = 0; ncol < GapSize.ColsGap.Count; ncol++)
					{
						nsum_x	+= (int)(GapSize.ColsGap[ncol]*1000);

						ptpos	= new Point(nsum_x, nsum_y);

						dicPickerPos.Add(npicker_no, ptpos);

						npicker_no++;
					}
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	static	void		CalUseRowPicker(int nMaxCntPickerRow, List<string> ltUsedSource, ref Dictionary<int, List<int>> dicUsedRow)
		{
			try
			{
				int			nval		= 0;
				string[]	str_split	= null;

				Dictionary<int, List<string>> dictemp	= new Dictionary<int, List<string>>();

				for (int nrow = 0; nrow < ltUsedSource.Count; nrow++)
				{
					str_split	= ltUsedSource[nrow].Split(',');

					dictemp.Add(nrow, str_split.ToList());
				}

				dicUsedRow.Clear();

				for (int npicker_row = 0; npicker_row < nMaxCntPickerRow; npicker_row++)
				{
					dicUsedRow.Add(npicker_row, new List<int>());

					for (int ntray_row = 0; ntray_row < dictemp.Count; ntray_row++)
					{
						if (int.TryParse(dictemp[ntray_row][0], out nval) == false  ||
							dictemp[ntray_row].IndexOf((npicker_row+1).ToString()) >= 0)
						{
							dicUsedRow[npicker_row].Add(ntray_row);
						}
					}
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
    }
}
