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
	public partial class frmTact : Form
	{
		private	static string	E(SeqID seqID)
		{
			try
			{
				return EnumHelper.GetDescription(seqID);
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return seqID.ToString();
			}
		}

		private	static string	E(Tact tactID)
		{
			try
			{
				return EnumHelper.GetDescription(tactID);
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return tactID.ToString();
			}
		}

		private	string[]	m_strSeqTact	= new string[] {E(SeqID.LOADER_HOME), E(SeqID.LOADER_LOAD), E(SeqID.LOADER_DOWN), E(SeqID.LOADER_UP), "",
															E(SeqID.TRAYCLEAN_HOME), E(SeqID.TRAYCLEAN_CLEAN),E(SeqID.TRAYCLEAN_GRIP),E(SeqID.TRAYCLEAN_RELEASE), "",
															E(SeqID.LDTRANS_HOME), E(SeqID.LDTRANS_PICK), E(SeqID.LDTRANS_PICKCONV), E(SeqID.LDTRANS_PLACE), E(SeqID.LDTRANS_PNP), "",
															E(SeqID.LOADTABLE1_HOME), E(SeqID.LOADTABLE1_UP),E(SeqID.LOADTABLE1_DOWN),E(SeqID.LOADTABLE1_GRIP),E(SeqID.LOADTABLE1_RELEASE), "",
															E(SeqID.LOADTABLE2_HOME), E(SeqID.LOADTABLE2_UP),E(SeqID.LOADTABLE2_DOWN),E(SeqID.LOADTABLE2_GRIP),E(SeqID.LOADTABLE2_RELEASE), "",
															E(SeqID.LOADPNP_HOME), E(SeqID.LOADPNP_PICKTRAY), E(SeqID.LOADPNP_PLACEBUFFER), E(SeqID.LOADPNP_PNP), "",
															E(SeqID.LDBUFFER1_HOME), E(SeqID.LDBUFFER1_LOADWAIT), E(SeqID.LDBUFFER1_CLEAN), E(SeqID.LDBUFFER1_UNLOADWAIT), "",
															E(SeqID.LDBUFFER2_HOME), E(SeqID.LDBUFFER2_LOADWAIT), E(SeqID.LDBUFFER2_CLEAN), E(SeqID.LDBUFFER2_UNLOADWAIT), "",
															E(SeqID.LDINSPPNP_HOME), E(SeqID.LDINSPPNP_PICK), E(SeqID.LDINSPPNP_PLACE), E(SeqID.LDINSPPNP_PNP), "",
															E(SeqID.INSPPLATE1_HOME), E(SeqID.INSPPLATE1_LOADWAIT), E(SeqID.INSPPLATE1_UNLOADWAIT), E(SeqID.INSPPLATE1_INSPWAIT), "",
															E(SeqID.INSPPLATE2_HOME), E(SeqID.INSPPLATE2_LOADWAIT), E(SeqID.INSPPLATE2_UNLOADWAIT), E(SeqID.INSPPLATE2_INSPWAIT), "",
															E(SeqID.ULDINSPPNP_HOME), E(SeqID.ULDINSPPNP_PICK), E(SeqID.ULDINSPPNP_PLACE), E(SeqID.ULDINSPPNP_PNP), "",
															E(SeqID.ULDBUFFER_HOME), E(SeqID.ULDBUFFER_LOADWAIT), E(SeqID.ULDBUFFER_UNLOADWAIT), "",
															E(SeqID.REJECTBUFFER_HOME), E(SeqID.REJECTBUFFER_READY), E(SeqID.REJECTBUFFER_CHANGE), "",
															E(SeqID.UNLOADPNP_HOME), E(SeqID.UNLOADPNP_PICKBUFFER), E(SeqID.UNLOADPNP_PICKREJECT), E(SeqID.UNLOADPNP_PLACE), E(SeqID.UNLOADPNP_PNP), "",
															E(SeqID.UNLOADTABLE1_HOME), E(SeqID.UNLOADTABLE1_UP),E(SeqID.UNLOADTABLE1_DOWN),E(SeqID.UNLOADTABLE1_GRIP),E(SeqID.UNLOADTABLE1_RELEASE), "",
															E(SeqID.UNLOADTABLE2_HOME), E(SeqID.UNLOADTABLE2_UP),E(SeqID.UNLOADTABLE2_DOWN),E(SeqID.UNLOADTABLE2_GRIP),E(SeqID.UNLOADTABLE2_RELEASE), "",
															E(SeqID.ULDTRANS_HOME), E(SeqID.ULDTRANS_PLACE), E(SeqID.ULDTRANS_PLACECONV), E(SeqID.ULDTRANS_PICK), E(SeqID.ULDTRANS_PNP), "",
															E(SeqID.UNLOADER_UNLOAD), E(SeqID.UNLOADER_LIFTUP), E(SeqID.UNLOADER_LIFTDOWN), E(SeqID.UNLOADER_ALIGN)};

		private	string[]	m_strIntervalTact	= new string[] {E(Tact.LOAD_CONVEYOR_START), E(Tact.TRAY_LOAD_START), E(Tact.LOAD_PnP_START), E(Tact.UNLOAD_PnP_START), E(Tact.TRAY_UNLOAD_START), E(Tact.UNLOAD_CONVEYOR_START)};



		public			frmTact()
		{
			InitializeComponent();
		}



		private	void	frmTact_Load(object sender, EventArgs e)
		{
			try
			{
				SetLanguageText();

				Init();

				tmrUpdateInfo.Start();
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void	btnClose_Click(object sender, EventArgs e)
		{
			try
			{
				this.Close();
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}



		private	void	Init()
		{
			try
			{
				int	nno	= 1;

				dgvSeqTact.Rows.Clear();
				for (int i = 0; i < m_strSeqTact.Length; i++)
				{
					if (m_strSeqTact[i] == "")
					{
						dgvSeqTact.Rows.Add("", "", "", "");
						dgvSeqTact.Rows[i].Height	= 10;
						nno++;
					}
					else
					{
						dgvSeqTact.Rows.Add(nno, m_strSeqTact[i], SM.SHM.GetSeqLastTact(m_strSeqTact[i]), SM.SHM.GetSeqTact(m_strSeqTact[i]));
					}
				}

				dgvIntervalTact.Rows.Clear();
				for (int i = 0; i < m_strIntervalTact.Length; i++)
				{
					if (m_strIntervalTact[i] == "")
					{
						dgvIntervalTact.Rows.Add("", "", "");
					}
					else
					{
						dgvIntervalTact.Rows.Add(m_strIntervalTact[i], SM.SHM.GetSeqLastTact(m_strIntervalTact[i]), SM.SHM.GetSeqTact(m_strIntervalTact[i]));
					}
				}
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	UpdateSeqTact()
		{
			try
			{
				for (int i = 0; i < m_strSeqTact.Length; i++)
				{
					if (m_strSeqTact[i] != "")
					{
						dgvSeqTact[2, i].Value	= SM.SHM.GetSeqLastTact(m_strSeqTact[i]);
						dgvSeqTact[3, i].Value	= SM.SHM.GetSeqTact(m_strSeqTact[i]);
					}
				}
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	UpdateIntervalTact()
		{
			try
			{
				for (int i = 0; i < m_strIntervalTact.Length; i++)
				{
					if (m_strIntervalTact[i] != "")
					{
						dgvIntervalTact[1, i].Value	= SM.SHM.GetSeqLastTact(m_strIntervalTact[i]);
						dgvIntervalTact[2, i].Value	= SM.SHM.GetSeqTact(m_strIntervalTact[i]);
					}
				}
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}



		private void	tmrUpdateInfo_Tick(object sender, EventArgs e)
		{
			try
			{
				UpdateSeqTact();
				UpdateIntervalTact();
			}
			catch
			{

			}
		}

		

		private	void	SetLanguageText()
		{
			try
			{
				btnClose.Text	= Language.languageDefault.BTN_Close;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
	}
}
