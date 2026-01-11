using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace H_MarsSSD
{
	public partial class frmAllHomeMonitor : Form
	{
		private	enum	teStatus
		{
			Waiting,
			Homing,
			Complete,
			Stop,
			Disable,
		}

		private	UsePartName[]	m_UsePartNames	= new UsePartName[]
		{
			UsePartName.LDTable1,
			UsePartName.LDTable2,
			UsePartName.LDBuf1,
			UsePartName.LDBuf2,
			UsePartName.Insp1,
			UsePartName.Insp2,
			UsePartName.ULDTable1,
			UsePartName.ULDTable2,
		};
		private	SeqID[]		m_UsePartSeqIDs	= new SeqID[]
		{
			SeqID.LOADTABLE1_HOME,
			SeqID.LOADTABLE2_HOME,
			SeqID.LDBUFFER1_HOME,
			SeqID.LDBUFFER2_HOME,
			SeqID.INSPPLATE1_HOME,
			SeqID.INSPPLATE2_HOME,
			SeqID.UNLOADTABLE1_HOME,
			SeqID.UNLOADTABLE2_HOME,
		};


		private	Dictionary<SeqID, teStatus>	m_dicHomeStatus		= new Dictionary<SeqID, teStatus>();
		private	PStopwatch						m_AutoCloseWatch	= new PStopwatch();		

		public			frmAllHomeMonitor()
		{
			InitializeComponent();
		}

		private	void	frmAllHomeMonitor_Load(object sender, EventArgs e)
		{
			try
			{
				SM.SHM.SetBitData(BIT.UIAllHomeStopping, false);

				GridInit();

				tmrUpdateStatus.Start();
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public	void	ResetStatus()
		{
			try
			{
				for (int i = 0; i < m_dicHomeStatus.Count; i++)
				{
					var	dic	= m_dicHomeStatus.ElementAt(i);

					m_dicHomeStatus[dic.Key]			= teStatus.Waiting;
					dgvHomeStatus[1, i].Value			= teStatus.Waiting.ToString();
					dgvHomeStatus[1, i].Style.BackColor	= Color.Yellow;
					dgvHomeStatus[1, i].Style.ForeColor	= Color.Black;
				}

				int nrow	= 0;
				for (int i = 0; i < m_UsePartNames.Length; i++)
				{
					if (SM.SHM.IsUsePart(m_UsePartNames[i], UsePartMode.Disable) == true)
					{
						m_dicHomeStatus[m_UsePartSeqIDs[i]]	= teStatus.Disable;

						nrow	= m_dicHomeStatus.Keys.ToList().IndexOf(m_UsePartSeqIDs[i]);
						dgvHomeStatus[1, nrow].Value			= teStatus.Disable.ToString();;
						dgvHomeStatus[1, nrow].Style.BackColor	= Color.DarkGray;
						dgvHomeStatus[1, nrow].Style.ForeColor	= Color.White;
					}
				}

				if (SM.SHM.GetUseSkipData(USESKIP.TopInspection) == false)
					{
						m_dicHomeStatus[SeqID.INSP_TOP_HOME]	= teStatus.Disable;

						nrow	= m_dicHomeStatus.Keys.ToList().IndexOf(SeqID.INSP_TOP_HOME);
						dgvHomeStatus[1, nrow].Value			= teStatus.Disable.ToString();;
						dgvHomeStatus[1, nrow].Style.BackColor	= Color.DarkGray;
						dgvHomeStatus[1, nrow].Style.ForeColor	= Color.White;
					}
					
					if (SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == false)
					{
						m_dicHomeStatus[SeqID.INSP_BTM_HOME]	= teStatus.Disable;

						nrow	= m_dicHomeStatus.Keys.ToList().IndexOf(SeqID.INSP_BTM_HOME);
						dgvHomeStatus[1, nrow].Value			= teStatus.Disable.ToString();;
						dgvHomeStatus[1, nrow].Style.BackColor	= Color.DarkGray;
						dgvHomeStatus[1, nrow].Style.ForeColor	= Color.White;
					}

				SM.SHM.SetBitData(BIT.UIAllHomeStopping, false);
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	GridInit()
		{
			try
			{
				
			


				m_dicHomeStatus.Clear();
				foreach (SeqID id in Enum.GetValues(typeof(SeqID)))
				{
				
					string	str		= id.ToString();
					int		nidx	= str.IndexOf("_HOME", StringComparison.OrdinalIgnoreCase);

					if (nidx > 0)
					{
						str	= str.Remove(nidx, str.Length-nidx);
						
						int count = 21;//dgvHomeStatus.RowCount;
						dgvHomeStatus.RowTemplate.Height = dgvHomeStatus.Height / (count + 1);
						dgvHomeStatus.ColumnHeadersHeight = dgvHomeStatus.Height - dgvHomeStatus.RowTemplate.Height * count;
						m_dicHomeStatus.Add(id, teStatus.Waiting);
						dgvHomeStatus.Rows.Add(str, teStatus.Waiting.ToString());
						dgvHomeStatus[0, dgvHomeStatus.RowCount-1].Style.BackColor	= Color.DimGray;
						dgvHomeStatus[0, dgvHomeStatus.RowCount-1].Style.ForeColor	= Color.White;
						dgvHomeStatus[1, dgvHomeStatus.RowCount-1].Style.BackColor	= Color.Yellow;
					}
				}

				int nrow	= 0;
				for (int i = 0; i < m_UsePartNames.Length; i++)
				{
					if (SM.SHM.IsUsePart(m_UsePartNames[i], UsePartMode.Disable) == true)
					{
						m_dicHomeStatus[m_UsePartSeqIDs[i]]	= teStatus.Disable;

						nrow	= m_dicHomeStatus.Keys.ToList().IndexOf(m_UsePartSeqIDs[i]);
						dgvHomeStatus[1, nrow].Value			= teStatus.Disable.ToString();;
						dgvHomeStatus[1, nrow].Style.BackColor	= Color.DarkGray;
						dgvHomeStatus[1, nrow].Style.ForeColor	= Color.White;
					}
					
					if (SM.SHM.GetUseSkipData(USESKIP.TopInspection) == false)
					{
						m_dicHomeStatus[SeqID.INSP_TOP_HOME]	= teStatus.Disable;

						nrow	= m_dicHomeStatus.Keys.ToList().IndexOf(SeqID.INSP_TOP_HOME);
						dgvHomeStatus[1, nrow].Value			= teStatus.Disable.ToString();;
						dgvHomeStatus[1, nrow].Style.BackColor	= Color.DarkGray;
						dgvHomeStatus[1, nrow].Style.ForeColor	= Color.White;
					}
					
					if (SM.SHM.GetUseSkipData(USESKIP.BtmInspection) == false)
					{
						m_dicHomeStatus[SeqID.INSP_BTM_HOME]	= teStatus.Disable;

						nrow	= m_dicHomeStatus.Keys.ToList().IndexOf(SeqID.INSP_BTM_HOME);
						dgvHomeStatus[1, nrow].Value			= teStatus.Disable.ToString();;
						dgvHomeStatus[1, nrow].Style.BackColor	= Color.DarkGray;
						dgvHomeStatus[1, nrow].Style.ForeColor	= Color.White;
					}
					
				}

				dgvHomeStatus.ClearSelection();
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	dgvHomeStatus_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			dgvHomeStatus.ClearSelection();
		}

		private void	btnClose_Click(object sender, EventArgs e)
		{
			Close();
		}



		private	void	tmrUpdateStatus_Tick(object sender, EventArgs e)
		{
			try
			{
				if		(SM.SHM.IsBitData(BIT.AllHoming) == true  &&  btnClose.Enabled == true)
				{
					btnClose.Enabled	= false;
				}
				else if (SM.SHM.IsBitData(BIT.AllHoming) == false  &&  btnClose.Enabled == false)
				{
					btnClose.Enabled	= true;
				}

				if		(SM.SHM.IsBitData(BIT.AllHoming) == false  &&  SM.SHM.IsBitData(BIT.AllHome) == false)
				{
					return;
				}
				else if	(SM.SHM.IsBitData(BIT.AllHoming) == false)
				{
					if (m_AutoCloseWatch.IsRunning == false)				m_AutoCloseWatch.Restart();
					if (m_AutoCloseWatch.IsTimeOverMs(500) == true)			this.Close();
				}
				else if (m_AutoCloseWatch.IsRunning == true)
				{
					m_AutoCloseWatch.Stop();
				}

				for (int i = 0; i < m_dicHomeStatus.Count; i++)
				{
					var	dic	= m_dicHomeStatus.ElementAt(i);

					if		(dic.Value == teStatus.Waiting)
					{
						if (SM.SHM.GetCurStep(dic.Key) != "START")
						{
							m_dicHomeStatus[dic.Key]			= teStatus.Homing;
							dgvHomeStatus[1, i].Value			= teStatus.Homing.ToString();
							dgvHomeStatus[1, i].Style.BackColor	= Color.Lime;
						}
					}
					else if (dic.Value == teStatus.Homing)
					{
						if (SM.SHM.GetCurStep(dic.Key) == "START")
						{
							if (SM.SHM.IsBitData(BIT.UIAllHomeStopping) == false)
							{
								m_dicHomeStatus[dic.Key]			= teStatus.Complete;
								dgvHomeStatus[1, i].Value			= teStatus.Complete.ToString();
								dgvHomeStatus[1, i].Style.BackColor	= Color.Blue;
								dgvHomeStatus[1, i].Style.ForeColor	= Color.White;
							}
							else
							{
								m_dicHomeStatus[dic.Key]			= teStatus.Stop;
								dgvHomeStatus[1, i].Value			= teStatus.Stop.ToString();
								dgvHomeStatus[1, i].Style.BackColor	= Color.Red;
								dgvHomeStatus[1, i].Style.ForeColor	= Color.White;
							}
						}
					}
				}
			}
			catch
			{

			}
		}
	}
}
