using System;
using System.Windows.Forms;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

namespace H_MarsSSD
{
	public partial class frmLog : Form
	{
		#region [ Variables ]
		private	enum RadioButtonCode
		{
			/// <summary>
			///  라디오 버튼 구분용
			/// </summary>
			MmiButton,
			SeqButton,
			ErrButton,
			TimeButton,
			YieldButton,
			HistoryButton,
			AllErrorButton,
			Num
		};

		private	RadioButtonCode	m_rdCode			= new RadioButtonCode(); // 어떤 라디오 버튼이 눌렸는지 구분
		private	List<string[]>	m_ltData			= new List<string[]>();
		private	Thread			m_threadUpdate;
		private DateTime		m_dtPreDateTime		= new DateTime(); // 로그상 전에 확인한 시간
		private TimeSpan		m_tsRunTime			= new TimeSpan();
		private TimeSpan		m_tsStopTime		= new TimeSpan();
		private TimeSpan		m_tsErrTime			= new TimeSpan();
		private TimeSpan[]		m_tsEachErrTime		= new TimeSpan[Define.MAX_ERROR_OCCUR_NUM];
		private	bool			m_bPreDateTimeInit	= false;
		private int				m_nErrCount			= new int();
		private int				m_nErrorCode		= 0;
		private string			m_strPreStatus		= "STOP"; // 로그상 전에 확인한 장비 상태
		private string[]		m_strErrMsg			= new string[Define.MAX_ERROR_OCCUR_NUM];
		private bool    		m_bErrorSearch		= false;
		private bool    		m_bYieldSearch		= false;
		private bool    		m_bHisSearch		= false;
		private	const string	mmiGridMessageCol	= "LogMessage";
		#endregion

		public			frmLog()
		{
			InitializeComponent();

			m_rdCode = RadioButtonCode.MmiButton;
			this.tmrLogging.Enabled = true;
            dgLogView.MultiSelect = true;
		}

		#region [ Event Function ]
		private	void	frmLog_VisibleChanged(object sender, EventArgs e)
		{
			
		}
		#endregion

		#region [ Timmer Function ]

		#endregion

		#region [ Date Event ]

		private	void	dtpFrom_ValueChanged(object sender, EventArgs e)
		{
			if (dtpTo.Value.CompareTo(dtpFrom.Value) < 0)
				dtpTo.Value = dtpFrom.Value;
        }

		private	void	dtpTo_ValueChanged(object sender, EventArgs e)
		{
			if (dtpTo.Value.CompareTo(dtpFrom.Value) < 0)
				dtpTo.Value = dtpFrom.Value;
        }
		#endregion

		#region [ Radio Button Event ]
		private	void	rbtn_CheckedChanged(object sender, EventArgs e)
		{
			RadioButton rbtn = sender as RadioButton;

			if (rbtn != null)
			{
				if (rbtn.Checked)
				{
					switch (rbtn.Name)
					{
						case "rbtnMMI":
							{
								this.tbTimeAnalysis.Visible = false;
								this.dgLogView.Visible = true;
								this.btnClear.Visible = false;
								this.tbTimeAnalysis.Clear();
								m_rdCode = RadioButtonCode.MmiButton;
								this.dgLogView.Columns.Clear(); // 기존 헤더 삭제 후에 새로운 헤더 추가
								this.dgLogView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
									this.LogDate,
									this.LogLevel,
									this.LogMessage});
                                dgLogView.ContextMenuStrip = MenuStrip;
                                SetMenuItemUnChecked();
                                mitemAll.Checked = true;
                                DatagridUpdate();
                            }
                            break;
						case "rbtnSeq":
							{
								this.tbTimeAnalysis.Visible = false;
								this.dgLogView.Visible = true;
								this.btnClear.Visible = false;
								this.tbTimeAnalysis.Clear();
								m_rdCode = RadioButtonCode.SeqButton;
								this.dgLogView.Columns.Clear(); // 기존 헤더 삭제 후에 새로운 헤더 추가
								this.dgLogView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
									this.LogDate,
									this.LogLevel,
									this.LogMessage,
									this.LogLocation,
                                    this.LogLine});
                                dgLogView.ContextMenuStrip = null;
                                DatagridUpdate();
                            }
                            break;
						case "rbtnErr":
							{
								this.tbTimeAnalysis.Visible = false;
								this.dgLogView.Visible = true;
								this.btnClear.Visible = false;
								this.tbTimeAnalysis.Clear();
								m_rdCode = RadioButtonCode.ErrButton;
								this.dgLogView.Columns.Clear(); // 기존 헤더 삭제 후에 새로운 헤더 추가
								this.dgLogView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
									this.ErrCode,
									this.ErrorName,
									this.ErrCount/*, this.ErrTime*/});
                                dgLogView.ContextMenuStrip = null;
                            }
							break;
						case "rbtnTime":
							{
								m_rdCode = RadioButtonCode.TimeButton;
								this.tbTimeAnalysis.Visible = true;
								this.dgLogView.Visible = false;
								this.btnClear.Visible = true;
                                dgLogView.ContextMenuStrip = null;
							}
							break;
						case "rbtnAllError":
							{
								this.dgLogView.Visible = true;
								this.tbTimeAnalysis.Clear();
								m_rdCode = RadioButtonCode.AllErrorButton;
								this.dgLogView.Columns.Clear(); // 기존 헤더 삭제 후에 새로운 헤더 추가
								this.dgLogView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
									this.LogDate,
									this.LogLevel,
									this.LogMessage});
                                m_bErrorSearch = true;
                                DatagridUpdate();
							}
							break;
						case "rbtnAllYield":
							{
								this.dgLogView.Visible = true;
								this.tbTimeAnalysis.Clear();
								m_rdCode = RadioButtonCode.YieldButton;
								this.dgLogView.Columns.Clear(); // 기존 헤더 삭제 후에 새로운 헤더 추가
								this.dgLogView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
									this.LogDate,
									this.LogLevel,
									this.LogMessage});
                                m_bYieldSearch = true;
                                DatagridUpdate();
							}
							break;
						case "rbtnAllHistory":
							{
								this.dgLogView.Visible = true;
								this.tbTimeAnalysis.Clear();
								m_rdCode = RadioButtonCode.HistoryButton;
								this.dgLogView.Columns.Clear(); // 기존 헤더 삭제 후에 새로운 헤더 추가
								this.dgLogView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
									this.LogDate,
									this.LogLevel,
									this.LogMessage});
                                m_bHisSearch = true;
                                DatagridUpdate();
							}
							break;
						default:
							break;
					}
				}
			}

		}
		#endregion

		#region [ Button Event ]
		private	void	btnUpdate_Click(object sender, EventArgs e)
		{
			try
			{
				this.tbLoading.Visible = true;
				tbLoading.Invoke(new Action(() => tbLoading.AppendText(Language.languageDefault.PleaseWait)));

				if (m_threadUpdate != null  &&  m_threadUpdate.IsAlive == true)			return;
			
				dgLogView.RowCount	= 0;
		
				m_threadUpdate	= new Thread(ThreadUpdate);
				m_threadUpdate.IsBackground	= true;
				m_threadUpdate.Start();
			}
			catch
			{
			}
		}

		private	void	btnClear_Click(object sender, EventArgs e)
		{
			this.tbTimeAnalysis.Clear();
		}

		private	void	btnSearch_Click(object sender, EventArgs e)
		{
			Button btn = sender as Button;
			try
			{
				if (m_ltData.Count < 1 || dgLogView == null)			return;
				if (string.IsNullOrEmpty(txtSearchValue.Text))			return;

				btn.Enabled = false;

				this.tbLoading.Visible = true;
				tbLoading.BeginInvoke(new Action(() => tbLoading.AppendText(Language.languageDefault.PleaseWait)));        

				dgLogView.RowCount	= 0;

				Regex reg = new Regex(txtSearchValue.Text, RegexOptions.IgnoreCase);
				Match m;            
				List<string[]> ltTemp = new List<string[]>();
			
				foreach (string[] dataes in m_ltData)
				{
					foreach (string data in dataes)
					{ 
						m = reg.Match(data);
						if (m.Success)
						{
							ltTemp.Add(dataes);
							break;
						}
					}
				}
			
				dgLogView.RowCount		= ltTemp.Count;
				m_ltData				= ltTemp;
				dgLogView.CurrentCell	= null;

				this.tbLoading.Visible = false;
				this.tbLoading.Clear();

				btn.Enabled = true;
			}
			catch (Exception ex)
			{
				GLog.Add(string.Format("Exception Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				btn.Enabled = true;
			}
		}

		private void	butFindNext_Click(object sender, EventArgs e)
		{
			try
			{
				if (m_ltData.Count < 1 || dgLogView == null)			return;
				if (string.IsNullOrEmpty(txtSearchValue.Text))			return;

				Regex	reg		= new Regex(txtSearchValue.Text, RegexOptions.IgnoreCase);
				Match	m;
				int		nRow	= 0;
				int		nCol	= 0;

				if (dgLogView.CurrentCell == null)
				{
					nRow	= 0;
					nCol	= 0;
				}
				else
				{
					nRow	= dgLogView.CurrentCell.RowIndex;
					nCol	= dgLogView.CurrentCell.ColumnIndex;
					if (nCol == (m_ltData[0].Length-1))
					{
						nRow++;
						nCol	= 0;
					}
					else
					{
						nCol++;
					}
				}

				for (int i = nRow; i < m_ltData.Count; i++)
				{
					if (nCol >= m_ltData[i].Length)			nCol	= 0;
					for (int j = nCol; j < m_ltData[i].Length; j++)
					{ 
						m = reg.Match(m_ltData[i][j]);
						if (m.Success)
						{
							dgLogView.CurrentCell	= dgLogView[j, i];
							return;
						}
						nCol	= j+1;
					}
				}
			}
			catch
			{
			}
		}

		private void	butFindPre_Click(object sender, EventArgs e)
		{
			try
			{
				if (m_ltData.Count < 1 || dgLogView == null)			return;
				if (string.IsNullOrEmpty(txtSearchValue.Text))			return;

				Regex	reg		= new Regex(txtSearchValue.Text, RegexOptions.IgnoreCase);
				Match	m;
				int		nRow	= 0;
				int		nCol	= 0;
			
				if (dgLogView.CurrentCell == null)
				{
					nRow = m_ltData.Count-1;
					nCol = m_ltData[0].Length-1;
				}
				else
				{
					nRow	= dgLogView.CurrentCell.RowIndex;
					nCol	= dgLogView.CurrentCell.ColumnIndex;
					if (nCol == 0)
					{
						nRow--;
						nCol	= m_ltData[0].Length-1;
					}
					else
					{
						nCol--;
					}
				}

				for (int i = nRow; i >= 0; i--)
				{
					if (nCol < 0)			nCol	= m_ltData[i].Length-1;
					for (int j = nCol; j >= 0; j--)
					{ 
						m = reg.Match(m_ltData[i][j]);
						if (m.Success)
						{
							dgLogView.CurrentCell	= dgLogView[j, i];
							return;
						}
						nCol	= j-1;
					}
				}
			}
			catch
			{
			}
		}
        
        private void    btnFolderOpen_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(GLog.GetLogFilePath());
        }
        
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            
            RadioButton rbtn = rbtnMMI;
            switch (m_rdCode)
            {
                case RadioButtonCode.MmiButton: rbtn = rbtnMMI; break;
                case RadioButtonCode.SeqButton: rbtn = rbtnSeq; break;
                case RadioButtonCode.ErrButton: rbtn = rbtnErr; break;
                case RadioButtonCode.AllErrorButton: rbtn = rbtnAllError; break;
                case RadioButtonCode.TimeButton: rbtn = rbtnTime; break;
                case RadioButtonCode.YieldButton: rbtn = rbtnAllYield; break;
                case RadioButtonCode.HistoryButton: rbtn = rbtnAllHistory; break;
                default:
                    break;
            }
            rbtn_CheckedChanged(rbtn, null);
        }
		#endregion

		#region [ Function ]

		#region - Private Function

		#region Path Function
		// Log 파일에 전체 경로 및 파일 이름을 얻는 함수.
		private	string	GetLogFullPath(DateTime dtDate)
		{
			string FullPath = "";

			switch (m_rdCode)
			{
                case RadioButtonCode.MmiButton:
                case RadioButtonCode.HistoryButton:
                case RadioButtonCode.YieldButton:
                case RadioButtonCode.AllErrorButton:
                    FullPath = string.Format("{0}{1}{2}_{3:D4}{4:D2}{5:D2}{6}", UI.Logs.GetLogBasePath(), UI.Logs.GetLogFolderName(),
											UI.Logs.GetLogFilePrename(), dtDate.Year, dtDate.Month, dtDate.Day, @".log");
					break;
				case RadioButtonCode.ErrButton:
				case RadioButtonCode.TimeButton:
				case RadioButtonCode.SeqButton:
					{
						FullPath = string.Format("{0}{1}{2}{3}_{4:D4}{5:D2}{6:D2}{7}", UI.Logs.GetLogBasePath(), @"LOG\",
											@"Sequence\", "SeqLog", dtDate.Year, dtDate.Month, dtDate.Day, @".csv");
					}
					break;
				case RadioButtonCode.Num:
					break;
				default:
					break;
			}

			return FullPath;
		}

		#endregion

		#region Date Function
		// MMILog 중 |XXXX| 에서 XXXX를 추출
		private	string	GetLogLevel(string strLogLine)
		{
			if (strLogLine[0] != '[')			return	"";

			string strResult;
			
			strResult = strLogLine.Substring(strLogLine.IndexOf('<') + 1);
			strResult = strResult.Remove(strResult.IndexOf('>'));

			return strResult;
		}

		// MMILog 중 날짜를 추출
		private	string	GetLogDateTime(string strLogLine)
		{
			if (strLogLine[0] != '[')			return	"";

			string strResult = "9999-12-31 23 : 59 : 59.999";

			if (strLogLine.IndexOf('[') >= 0)
			{
				strResult = strLogLine.Substring(strLogLine.IndexOf('[') + 1);
				strResult = strResult.Remove(strResult.IndexOf(']'));
			}

			return strResult;
		}

		// MMILog 중 메세지를 추출
		private	string	GetLogMessage(string strLogLine)
		{
			string strResult;
			strResult = strLogLine.Substring(strLogLine.IndexOf('>') + 4);
			return strResult;
		}

		// Error 로그인 경우에서 메세지 내용만 추출
		private	string	GetErrLogMessage(string strLogLine)
		{
			string strResult;
			if (strLogLine.IndexOf('>') > 0)
				strResult = strLogLine.Substring(strLogLine.IndexOf('>') + 4);
			else
				strResult = strLogLine;

			return strResult;
		}

		// Error 시퀀스 로그에서 LineNumber 항목 추출
		private	int		GetErrLogLineNumber(string strLogLine)
		{
			string strResult;

			strResult = strLogLine.Substring(strLogLine.IndexOf('[') + 1);
			strResult = strResult.Remove(strResult.IndexOf(']'));

			return Convert.ToInt32(strResult);
		}

		// 날짜 형식 변형 dd-MM-yyyy HH:mm:ss.fff => yyyy-MM-dd HH:mm:ss.fff
		private	string	GetDateFormatChange(string strDateTime)
		{
			string strPreDate = "";     // 변경 전 날짜 형식
			string strNextDate = "";    // 변경 후 날짜 형식
			string strTime = "";        // 시간
			string strResult = "";

			strPreDate = strDateTime.Remove(strDateTime.IndexOf(' ')); // dd-MM-yyyy 형식 저장
			strTime = strDateTime.Substring(strDateTime.IndexOf(' ')); // 시간 저장

			string[] strDateTemp = strPreDate.Split('-'); // 년, 월, 일 분리

			strNextDate = strDateTemp[2] + "-" + strDateTemp[1] + "-" + strDateTemp[0]; // 년,월,일 재조합(yyyy-MM-dd 형식으로 변경)

			strResult = strNextDate + strTime; // 변경할 날짜 형식으로 저장

			return strResult;
		}
		#endregion

		#region Time Analysis Function
		/// <summary>
		/// 평균 고장 간격(Mean Time Between Failure)
		/// 고장간의 평균 간격 => 가동시간 / 고장건수
		/// </summary>
		private	string	CalMTBF()
		{
			int temp, errCnt;
			errCnt = m_nErrCount;
			if (errCnt == 0)
				errCnt = 1;
			temp = Convert.ToInt32(m_tsRunTime.TotalSeconds) / errCnt;
			TimeSpan time = TimeSpan.FromSeconds(temp);
			return time.ToString(@"dd\-hh\:mm\:ss");
		}

		/// <summary>
		/// 평균 수리 시간(Mean Time To Repair)
		/// 고장이 났을 때 수리하는데까지 걸린 시간의 평균 => 전체 고장 수리시간 / 고장 건수
		/// </summary>
		private	string	CalMTTR()
		{
			int temp, errCnt;
			errCnt = m_nErrCount;
			if (errCnt == 0)
				errCnt = 1;
			temp = Convert.ToInt32(m_tsErrTime.TotalSeconds) / errCnt;
			TimeSpan time = TimeSpan.FromSeconds(temp);
			return time.ToString(@"dd\-hh\:mm\:ss");
		}
		#endregion

		#region Update Function
		private	void	LogUpdate(DateTime dtDday)
		{
			string			strFullPath		= GetLogFullPath(dtDday);
			FileStream		file;
			StreamReader	readFile;

			// 파일이 존재하는지 여부 판단
			if (!System.IO.File.Exists(strFullPath))		return;

			file		= new FileStream(strFullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			readFile	= new StreamReader(file, Encoding.UTF8);

			try
			{
				DateTime		dtCurrDateTime	= new DateTime(); // 로그상 현재 읽고있는 시간
				string			strStatus		= "";
				string			strLine			= "";
				string[]		strSplite		= null;

				m_nErrorCode = 0;

				while ((strLine = readFile.ReadLine()) != null)
				{
					strSplite	= strLine.Split(',');

					// 로그 내용중 로그헤더가 있어서 로그헤더는 작성하지 않기 위해서 예외처리
					if (strSplite[0] == "Level")			continue;

					if (m_bPreDateTimeInit == false)
					{
						// Log 상 가장 처음 시간 입력 => new DateTime()으로 생성시에 0001년 01월 01일로 생성 되어 계산이 안 맞음
						if (m_rdCode == RadioButtonCode.ErrButton  ||  m_rdCode == RadioButtonCode.TimeButton)
						{
							m_dtPreDateTime = Convert.ToDateTime(GetDateFormatChange(strSplite[1]));
							m_bPreDateTimeInit	= true;
						}
					}

					switch (m_rdCode)
					{
						case RadioButtonCode.MmiButton:
						case RadioButtonCode.HistoryButton:
						case RadioButtonCode.YieldButton:
						case RadioButtonCode.AllErrorButton:
							{
								if (GetLogDateTime(strLine) == "9999-12-31 23 : 59 : 59.999")			continue;
								m_ltData.Add(new string[] {GetLogDateTime(strLine), GetLogLevel(strLine), GetLogMessage(strLine)});
							}
							break;
						case RadioButtonCode.SeqButton:
							{
								string str_msg	= "";

								for (int i = 4; i < strSplite.Length; i++)
								{
									str_msg += strSplite[i] +" ";
								}

								// 날짜[1], 레벨[0], 메세지[4], 위치[2], 라인[3] 순으로 datagrid에 작성
								m_ltData.Add(new string[] {GetDateFormatChange(strSplite[1]), strSplite[0], str_msg, strSplite[2], GetErrLogLineNumber(strSplite[3]).ToString()});
							}
							break;
						case RadioButtonCode.ErrButton:
						case RadioButtonCode.TimeButton:
							{
								switch (strSplite[0])
								{
									case "INFO":
										{
											switch (strSplite[4])
											{
												case "Machine AutoRun Start !!":
													strStatus = "AUTORUN";
													break;
												case "Machine Stop !!":
													strStatus = "STOP";
													break;
											}
										}
										break;
									case "ERR":
										strStatus = "ERROR";
										break;
									case "SEQ":
										strStatus = "SEQ";
										break;
									default:
										strStatus = "WARN";
										break;
								}

								dtCurrDateTime = Convert.ToDateTime(GetDateFormatChange(strSplite[1]));

								// Level이 ERR일때
								if (strSplite[0] == "ERR")
								{
									string strErrName = strSplite[4];
									string strErrCode = "";

									if (strSplite.Length < 6) // 시퀀스 로그에 에러코드가 작성이 안되는 경우 예외처리
									{
										// 에러코드가 작성되어 있지 않아서 에러코드를 찾아야 될 경우
										for (int j = 0; j <= (int)ALM.TOTAL; j++)
										{
											if (strErrName == ((ALM)j).ToString())
											{
												strErrCode = j.ToString();
												break;
											}
										}
									}
									else
									{
										if (strSplite[5] != "" && Convert.ToInt32(strSplite[5]) >= 0) // 시퀀스 로그에 에러코드가 작성 되어 있는 경우
										{
											strErrCode = strSplite[5].ToString();
										}
										else// 에러코드가 작성되어 있지 않아서 에러코드를 찾아야 될 경우
										{
											for (int j = 0; j <= (int)ALM.TOTAL; j++)
											{
												if (strErrName == ((ALM)j).ToString())
												{
													strErrCode = j.ToString();
													break;
												}
											}
										}
									}

									// 에러 코드가 등록되지 않은 에러는 999로 표기
									if (strErrCode == "")
										strErrCode = "999";

									SetErrSummaryValueUpdate(strStatus, dtCurrDateTime, strErrCode + "," + strErrName);
								}
								else
								{
									SetErrSummaryValueUpdate(strStatus, dtCurrDateTime);
								}

								SetErrSummaryGridUpdate();
							}
							break;
						default:
							break;
					}
				}
			}
			catch (Exception)
			{

			}
			finally
			{
				readFile.Close();
				file.Close();
			}
		}
        
        private void    DatagridUpdate()
        {
            this.tbLoading.Visible = true;
            tbLoading.Invoke(new Action(() => tbLoading.AppendText(Language.languageDefault.PleaseWait)));

            if (m_threadUpdate != null && m_threadUpdate.IsAlive == true) return;

            dgLogView.RowCount = 0;

            m_threadUpdate = new Thread(ThreadUpdate);
            m_threadUpdate.IsBackground = true;
            m_threadUpdate.Start();
        }

		private	void	SetErrSummaryValueUpdate(string strStatus, DateTime dtCurrDateTime, string strLogLine = "")
		{
			try
			{
				if (strStatus == "AUTORUN")
				{
					if (m_strPreStatus == "STOP")
					{
						m_tsStopTime += dtCurrDateTime - m_dtPreDateTime;
						m_dtPreDateTime = dtCurrDateTime;
						m_strPreStatus = strStatus;
					}
					else if (m_strPreStatus == "ERROR")
					{
						m_tsStopTime += dtCurrDateTime - m_dtPreDateTime;
						m_tsErrTime += dtCurrDateTime - m_dtPreDateTime;

						if (m_nErrCount > 0)
							m_tsEachErrTime[m_nErrCount - 1] = (dtCurrDateTime - m_dtPreDateTime);
						m_dtPreDateTime = dtCurrDateTime;
						m_strPreStatus = strStatus;
					}
				}
				else if (strStatus == "STOP")
				{
					if (m_strPreStatus == "AUTORUN")
					{
						m_tsRunTime += dtCurrDateTime - m_dtPreDateTime;
						m_dtPreDateTime = dtCurrDateTime;
						m_strPreStatus = strStatus;
					}
				}
				else if (strStatus == "ERROR")
				{
					if (m_strPreStatus == "AUTORUN")
					{
						string strMessage = GetErrLogMessage(strLogLine);
						string[] strTemp = strMessage.Split(",".ToCharArray());
						m_nErrorCode = Convert.ToInt32(strTemp[0]);

						m_tsRunTime += dtCurrDateTime - m_dtPreDateTime;
						m_dtPreDateTime = dtCurrDateTime;

						m_strErrMsg[m_nErrCount] = GetErrLogMessage(strLogLine);
						m_nErrCount++;

						m_strPreStatus = strStatus;
					}
				}
			}
			catch (Exception)
			{

			}
		}

		private	string[]	m_strCode	= new string[Define.MAX_ERROR_NUM];        // Err Code
		private	string[]	m_strName	= new string[Define.MAX_ERROR_NUM];        // Err Name
		private	int[]		m_nCount	= new int[Define.MAX_ERROR_NUM];           // Err 갯수
		private	TimeSpan[]	m_tsTime	= new TimeSpan[Define.MAX_ERROR_NUM];      // Err 누적 시간
		private	int			m_nBufCnt;

		private	void	SetErrSummaryGridUpdate()
		{
			string strNum, strMsg;
			TimeSpan tsErrorTime;

			m_nBufCnt = 0;

			for (int i = 0; i < m_nErrCount; i++)
			{
				string[] strTemp = m_strErrMsg[i].Split(',');
				strNum = strTemp[0]; // Err code
				strMsg = strTemp[1]; // Err Name(Message)
				tsErrorTime = m_tsEachErrTime[i];

				// 가장 처음 Err 등록
				if (i == 0)
				{
					m_strCode[m_nBufCnt]	= strNum;
					m_strName[m_nBufCnt]	= strMsg;
					m_nCount[m_nBufCnt]		= 1;
					m_tsTime[m_nBufCnt]		= tsErrorTime;
					m_nBufCnt++;
					continue;
				}

				for (int j = 0; j < m_nBufCnt; j++)
				{
					// 새로 들어온 Err와 저장된 배열 안에 있는 Err가 같다면 시간 누적
					if (strMsg == m_strName[j])
					{
						m_nCount[j]++;                // Err 카운팅
						m_tsTime[j] += tsErrorTime;   // Err 시간 누적
						break;
					}

					// 새로운 Err 등록
					if (j == (m_nBufCnt - 1))
					{
						m_strCode[m_nBufCnt]	= strNum;
						m_strName[m_nBufCnt]	= strMsg;
						m_nCount[m_nBufCnt]		= 1;
						m_tsTime[m_nBufCnt]		= tsErrorTime;
						m_nBufCnt++;
						break;
					}
				}
			}

			m_ltData.Clear();

			if (RadioButtonCode.ErrButton == m_rdCode)
			{
				// Err datagrid에 추가
				for (int k = 0; k < m_nBufCnt; k++)
					m_ltData.Add(new string[] {m_strCode[k], m_strName[k], m_nCount[k].ToString()});
			}
		}

		// 시간 분석 결과를 TextBox에서 출력
		private	void	TimeAnalysisUpdate()
		{
			String[] Lines = new String[100];

			//Lines[0] = string.Format("=========================================================================");
			Lines[0] = string.Format("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
			Lines[1] = string.Format("  LOG DATA OUTPUT ({0})", DateTime.Now);
			Lines[2] = string.Format("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
			//Lines[2] = string.Format("=========================================================================");
			Lines[3] = string.Format(" 1. LOGGING DATE : {0} ~ {1}", dtpFrom.Value.ToShortDateString(), dtpTo.Value.ToShortDateString());
			Lines[4] = string.Format(" 2. RUN TIME     : {0}(dd-hh:mm:ss)", m_tsRunTime.ToString(@"dd\-hh\:mm\:ss"));
			Lines[5] = string.Format(" 3. STOP TIME    : {0}(dd-hh:mm:ss)", m_tsStopTime.ToString(@"dd\-hh\:mm\:ss"));
			Lines[6] = string.Format(" 4. MTBF            : {0}(dd-hh:mm:ss)", CalMTBF());
			Lines[7] = string.Format(" 5. MTTR            : {0}(dd-hh:mm:ss)", CalMTTR());
			Lines[8] = string.Format("=========================================================================");

			for (int i = 0; i < m_nBufCnt; i++)
			{
				Lines[i + 9] = string.Format("CODE : {0:D3}  |  COUNT : {1:D3}  |  TIME : {2}  |  NAME : {3}",
							Convert.ToInt32(m_strCode[i]), m_nCount[i], m_tsTime[i].ToString(@"hh\:mm\:ss"), m_strName[i]);
			}

			Lines[m_nBufCnt + 9] = string.Format("=========================================================================");

			for (int i = 0; i < m_nBufCnt + 10; i++)
			{
				this.tbTimeAnalysis.AppendText(Lines[i]);
				this.tbTimeAnalysis.AppendText("\r\n"); // 줄바꿈
			}
			this.tbTimeAnalysis.AppendText("\r\n\r\n");
		}

		private	void	dgLogView_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
		{
			try
			{
				e.Value	= m_ltData[e.RowIndex][e.ColumnIndex];
			}
			catch
			{

			}
		}

		private	void	ThreadUpdate()
		{
			try
			{
				DateTime dtStartDateTime = dtpFrom.Value;
				DateTime dtEndDateTime = dtpTo.Value;
				m_tsStopTime = new TimeSpan();
				m_tsErrTime = new TimeSpan();
				m_tsRunTime = new TimeSpan();
				int nDayNum = 0;

				nDayNum = (dtEndDateTime.Date - dtStartDateTime.Date).Days + 1;

				m_strPreStatus = "STOP";
				m_nErrCount = 0;

				m_ltData.Clear();

				this.Invoke(new MethodInvoker(delegate() 
				{ 
					dgLogView.RowCount	= m_ltData.Count;
				}));

				m_bPreDateTimeInit	= false;

				for (int i = 0; i < nDayNum; i++)
				{
					LogUpdate(dtStartDateTime);
					dtStartDateTime = dtStartDateTime.AddDays(1.0);
				}

				this.Invoke(new MethodInvoker(delegate() 
				{ 
					dgLogView.RowCount	= m_ltData.Count;
					dgLogView.CurrentCell	= null;

                    // 시간 분석을 업데이트
                    if (m_rdCode == RadioButtonCode.TimeButton)
                        TimeAnalysisUpdate();
                }));

                if (m_bErrorSearch == true)
                {
                    m_bErrorSearch = false;
                    this.Invoke(new MethodInvoker(delegate ()
                    {
                        txtSearchValue.Text = "Error";
                        btnSearch.PerformClick();
                        txtSearchValue.Text = "";
                    }));
                }
                if (m_bYieldSearch == true)
                {
                    m_bYieldSearch = false;
                    this.Invoke(new MethodInvoker(delegate ()
                    {
                        txtSearchValue.Text = "Yield";
                        btnSearch.PerformClick();
                        txtSearchValue.Text = "";
                    }));
                }
                if (m_bHisSearch == true)
                {
                    m_bHisSearch = false;
                    this.Invoke(new MethodInvoker(delegate ()
                    {
                        txtSearchValue.Text = "HISTORY";
                        btnSearch.PerformClick();
                        txtSearchValue.Text = "";
                    }));
                }
            }
			catch (ThreadAbortException)
			{
			}
			catch (ThreadInterruptedException)
			{
			}
			catch (Exception)
			{
			}
			finally
			{
				this.Invoke(new MethodInvoker(delegate() 
				{ 
					Thread.Sleep(300);  // please wait을 잠깐 보여주기 위해 추가
					this.tbLoading.Visible = false;
					this.tbLoading.Clear();
				}));
			}
		}
		#endregion
		#endregion

		#region - Public Function

		#endregion

		#region - Language Function 

		public	void	SetLanguageText()
		{
			gbDataRange.Text	= Language.languageDefault.DateRange;
			rbtnMMI.Text		= Language.languageDefault.MMILog;
			rbtnSeq.Text		= Language.languageDefault.SEQLog;
			rbtnErr.Text		= Language.languageDefault.ErrorSummary;
			rbtnTime.Text		= Language.languageDefault.TimeAnalysis;
			btnClear.Text		= Language.languageDefault.LogClear;
			btnUpdate.Text		= Language.languageDefault.LogUpdate;
			rbtnAllError.Text	= Language.languageDefault.BtnErrSummary;
			rbtnAllHistory.Text	= Language.languageDefault.BtnDataHistory;
			btnFileOpen.Text	= Language.languageDefault.BtnFolderOpen;
			btnSearch.Text		= Language.languageDefault.BtnSearch;
		}

        #endregion

        #endregion
        
        #region [ Menu Strip ]        
        private void mitemSys_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem mi = (ToolStripMenuItem)sender;            

            SetMenuItemUnChecked();
            if (mi.Tag.ToString() == "ALL")
            {
                btnUpdate.PerformClick();                                
            }
            else
            {
                txtSearchValue.Text = mi.Tag.ToString();
                btnSearch.PerformClick();
            }
                                        
            txtSearchValue.Text = "";
            mi.Checked = true;
        }
        private void SetMenuItemUnChecked()
        {
            mitemAll.Checked = false;
            mitemData.Checked = false;
            mitemErr.Checked = false;
            mitemEx.Checked = false;
            mitemHis.Checked = false;
            mitemMt.Checked = false;
            mitemPw.Checked = false;
            mitemSys.Checked = false;
            mitemTk.Checked = false;
            mitemWarn.Checked = false;
			
        }

        #endregion [ Menu Strip ] (END)

    }
}
