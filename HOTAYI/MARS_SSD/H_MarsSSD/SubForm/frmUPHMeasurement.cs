using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace H_MarsSSD
{
	public partial class frmUPHMeasurement : Form
	{
		public frmUPHMeasurement()
		{
			InitializeComponent();
		}

		#region [ Variables ]

		private	DateTime				m_LastInspTimeOld	= new DateTime();
		private	double					m_dProdCnt			= 0;
		private	double[]				m_dUPH				= new double[3];
		private	bool					m_bLogRead			= false;
		public	Dictionary<ALM, int>	m_dicAlarm			= new Dictionary<ALM, int>();
		public	Dictionary<ALM, int>	m_dicAlarmPrev		= new Dictionary<ALM, int>();

		private	Thread			m_threadUpdate;
		private	AutoResetEvent	m_EvtAlarm		= new AutoResetEvent(false);
		private	object			m_lockDicSave	= new object();

		#endregion

		#region [ Enums ]

		#endregion

		#region [ Timer Event ]

		private void tmrUPHUpdate_Tick(object sender, EventArgs e)
		{
			try
			{
				if (SM.SHM.IsBitData(BIT.AutoRun) == false ||
					SM.SHM.IsBitData(BIT.Pause) == true ||
					SM.SHM.IsBitData(BIT.Error) == true)
				{
					//check: 조건 확인 필요
					if (SM.SHM.GetUPHTime(UPHStep.FirstInsp) == DateTime.MinValue)
					{
						SM.SHM.StopDown();
					}
					else
					{
						SM.SHM.StartDown();
					}
				}
				else
				{
					SM.SHM.StopDown();
				}

				if (SM.SHM.GetUPHTime(UPHStep.LastInsp) != m_LastInspTimeOld)
				{
					m_LastInspTimeOld	= SM.SHM.GetUPHTime(UPHStep.LastInsp);
					m_dProdCnt			= SM.SHM.GetProdCnt();
				}

				lbProdCnt.Text		= m_dProdCnt.ToString();
				lbFirstInsp.Text	= SM.SHM.GetUPHTime(UPHStep.FirstInsp).ToString();
				lbLastInsp.Text		= SM.SHM.GetUPHTime(UPHStep.LastInsp).ToString();

				CalUPH();
				
				lbUPH.Text		= string.Format("{0:F3}", m_dUPH[1]);

				UpdateAlarmGrid();
			}
			catch(Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		#endregion

		#region [ Event Functions ]

		private void frmUPHMeasurement_Load(object sender, EventArgs e)
		{
			try
			{
				Init();

				if (SM.SHM.IsRunUPH() == true)
				{
					btnStart.PerformClick();
					
					btnStart.Enabled = false;
				}
			}
			catch(Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		#endregion

		#region [ Button Event Functions ]

		private void btnStart_Click(object sender, EventArgs e)
		{
			try
			{
				tmrUPHUpdate.Start();
				tmrAlarmUpdate.Start();

				if (SM.SHM.IsRunUPH () == false)
				{
					SM.SHM.ClearUPHTime();
				}
				SM.SHM.StartUPH();

				btnStart.Enabled	= false;
				btnStart.ForeColor	= Color.White;
				btnStart.BackColor	= Color.Lime;

				btnStop.ForeColor = Color.Red;
				btnStop.BackColor = Color.White;

				dgvAlarm.Rows.Clear();
				m_dicAlarm.Clear();
			}
			catch(Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void btnStop_Click(object sender, EventArgs e)
		{
			try
			{
				tmrUPHUpdate.Stop();
				tmrAlarmUpdate.Stop();

				SM.SHM.StopUPH();

				btnStart.Enabled	= true;
				btnStart.ForeColor	= Color.Lime;
				btnStart.BackColor	= Color.White;

				btnStop.ForeColor = Color.White;
				btnStop.BackColor = Color.Red;
			}
			catch(Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		#endregion

		#region [ Functions ]

		#region - Private Functions

		private void Init()
		{
			m_LastInspTimeOld	= SM.SHM.GetUPHTime(UPHStep.LastInsp);
			m_dProdCnt			= SM.SHM.GetProdCnt();

			lbFirstInsp.Text	= "0";
			lbLastInsp.Text		= "0";
			lbUPH.Text			= "0";
			
			if (m_threadUpdate == null ||
				m_threadUpdate.IsAlive == false)
			{
				m_threadUpdate				= new Thread(ThreadUpdate);
				m_threadUpdate.IsBackground	= true;
				m_threadUpdate.Start();
			}
		}

		private void ThreadUpdate()
		{
			try
			{
				FileStream		file;
				StreamReader	readFile;

				while (true)
				{
					string			strFullPath = string.Format("{0}{1}{2}_{3:D4}{4:D2}{5:D2}{6}", UI.Logs.GetLogBasePath(), UI.Logs.GetLogFolderName(), UI.Logs.GetLogFilePrename(), DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, @".log");
					Thread.Sleep(1);

					if (m_EvtAlarm.WaitOne() == false)			continue;

					// 파일이 존재하는지 여부 판단
					if (File.Exists(strFullPath) == false)			return;

					file		= new FileStream(strFullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
					readFile	= new StreamReader(file, Encoding.UTF8);

					try
					{	
						Dictionary<ALM, int> dicAlarm	= new Dictionary<ALM, int>();

						dicAlarm.Clear();
						string strLine			= "";
						string[] strSplite		= null;

						while ((strLine = readFile.ReadLine()) != null)
						{
							Thread.Sleep(1);

							DateTime dtlinetime;
							DateTime dtstarttime	= SM.SHM.GetUPHTime(UPHStep.FirstInsp);
							DateTime dtendtime		= SM.SHM.GetUPHTime(UPHStep.LastInsp);
					
							if (GetLogLevel(strLine) == "ERROR")
							{
								if (DateTime.TryParse(GetLogDateTime(strLine), out dtlinetime) == false)		continue;

								if (DateTime.Compare(dtlinetime, dtstarttime)	!= 1)							continue;
								if (DateTime.Compare(dtlinetime, dtendtime)		!= -1)							continue;

								strSplite = GetLogMessage(strLine).Split(',');
								ALM code = (ALM)Convert.ToInt16(strSplite[0]);


								if	(dicAlarm.ContainsKey(code) == false)			dicAlarm.Add(code, 1);
								else												dicAlarm[code] = dicAlarm[code] + 1;
							}
						}

						m_bLogRead	= true;
						m_dicAlarm	= new Dictionary<ALM, int>(dicAlarm);
						m_bLogRead	= false;
					}
					catch (Exception ex)
					{
						UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
					}
					finally
					{
						readFile.Close();
						file.Close();
					}
				}
			}
			catch(Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private string GetLogLevel(string strLogLine)
		{
			try
			{
				if (strLogLine[0] != '[')		return	"";

				string strResult;
				
				strResult = strLogLine.Substring(strLogLine.IndexOf('<') + 1);
				strResult = strResult.Remove(strResult.IndexOf('>'));

				return strResult;
			}
			catch(Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return "";
			}
		}

		private string GetLogMessage(string strLogLine)
		{
			try
			{
				string strResult = strLogLine.Substring(strLogLine.IndexOf('>') + 4);
				return strResult;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return "";
			}
		}

		private string GetLogDateTime(string strLogLine)
		{
			try
			{
				if (strLogLine[0] != '[')		return	"";

				string strResult = "9999-12-31 23 : 59 : 59.999";

				if (strLogLine.IndexOf('[') >= 0)
				{
					strResult = strLogLine.Substring(strLogLine.IndexOf('[') + 1);
					strResult = strResult.Remove(strResult.IndexOf(']'));
				}

				return strResult;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return "";
			}

		}

		private void UpdateAlarmGrid()
		{
			try
			{
				if (m_bLogRead == true)		return;

				if (m_dicAlarm.Count == m_dicAlarmPrev.Count  &&  m_dicAlarm.Except(m_dicAlarmPrev).Any() == false)		return;

				dgvAlarm.Rows.Clear();
				
				List<ALM> keys = new List<ALM>(m_dicAlarm.Keys);

				for (int i = 0; i < keys.Count; i++)
				{
					if (m_dicAlarm.ContainsKey(keys[i]) == false)		continue;

					dgvAlarm.Rows.Add(keys[i].ToString(), m_dicAlarm[keys[i]].ToString("00"));
				}

				m_dicAlarmPrev = new Dictionary<ALM, int>(m_dicAlarm);
			}
			catch(Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void CalUPH()
		{
			try
			{
				// 총시간
				//if (SM.SHM.GetUPHTime(teUPHStep.FirstPickProd) < SM.SHM.GetUPHTime(teUPHStep.LastPlaceProd))
				//{
				//	TimeSpan	ts	= SM.SHM.GetUPHTime(teUPHStep.LastPlaceProd) - SM.SHM.GetUPHTime(teUPHStep.FirstPickProd);
				//	m_dUPH[0]		= 3600 / (ts.TotalSeconds / m_dProdCnt);
				//}

				// 첫 검사 시작부터 마지막 검사 시작 까지
				if (SM.SHM.GetUPHTime(UPHStep.FirstInsp) < SM.SHM.GetUPHTime(UPHStep.LastInsp))
				{
					TimeSpan	ts	= SM.SHM.GetUPHTime(UPHStep.LastInsp) - SM.SHM.GetUPHTime(UPHStep.FirstInsp);
					m_dUPH[1]		= 3600 / (ts.TotalSeconds / m_dProdCnt);

					//dgvUPHData[2,2].Value = m_dUPH[1];
				}

				// 첫 검사 시작부터 마지막 검사 시작 까지. down 시간 제외
				if (SM.SHM.GetUPHTime(UPHStep.FirstInsp) < SM.SHM.GetUPHTime(UPHStep.LastInsp))
				{
					TimeSpan ts = SM.SHM.GetUPHTime(UPHStep.LastInsp) - SM.SHM.GetUPHTime(UPHStep.FirstInsp);
					ts			= ts - new TimeSpan(SM.SHM.GetDownTime());
					m_dUPH[2]	= 3600 / (ts.TotalSeconds / m_dProdCnt);

					//dgvUPHData[2,3].Value = m_dUPH[2];
				}
			}
			catch(Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		#endregion

		#region - Public Functions

		#endregion

		#endregion
	}
}
