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
	public partial class frmPnPSelect : Form
	{
		public frmPnPSelect()
		{
			InitializeComponent();
		}

		private string m_selectedPick;
		public string SelectedPick { get => m_selectedPick; }

		private string m_selectedPlace;
		public string SelectedPlace { get => m_selectedPlace; }


		protected override void WndProc(ref Message m)
		{
			switch(m.Msg)
			{
				case 0x00A3: return; //더블 클릭 시 최대화 막기
				case 0x84:
					base.WndProc(ref m);
					if ((int)m.Result == 0x1)
						m.Result = (IntPtr)0x2;
					return;
			}

			base.WndProc(ref m);
		}
		
		//Enum 이름을 그대로 사용하기 싫다면 사용 > 디스크립션을 모두 정의해 줘야함
		private void SetComboBox(ComboBox cb, Enum en)
		{
			try
			{
				cb.Items.Clear();
				Array a = Enum.GetValues(en.GetType());
				for (int i = 0; i < a.Length; i++)
				{
					string str = EnumHelper.GetDescription((Enum)a.GetValue(i));
					cb.Items.Add(str);
				}
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public DialogResult ShowDialog(SeqID Seq)
		{
			try
			{
				switch (Seq)
				{
					case SeqID.LDTRANS_PICK:
						cbPlace.Enabled		= false;
						lbPlace.Enabled		= false;
						cbPick.DataSource	= Enum.GetValues(typeof(SeqLoadTransfer.PosX));
						break;
					case SeqID.LDTRANS_PLACE:
						cbPick.Enabled		= false;
						lbPick.Enabled		= false;
						cbPlace.DataSource	= Enum.GetValues(typeof(SeqLoadTransfer.PosX));
						break;
					case SeqID.LDTRANS_PNP:
						cbPick.DataSource	= Enum.GetValues(typeof(SeqLoadTransfer.PosX));
						cbPlace.DataSource	= Enum.GetValues(typeof(SeqLoadTransfer.PosX));
						break;
					case SeqID.LDINSPPNP_PICK:
						cbPlace.Enabled		= false;
						lbPlace.Enabled		= false;
						cbPick.DataSource	= new List<string> { "BUF1", "BUF2" };
						break;
					case SeqID.LDINSPPNP_PLACE:
						cbPick.Enabled		= false;
						lbPick.Enabled		= false;
						cbPlace.DataSource	= new List<string> { "INSP1", "INSP2" };//Enum.GetValues(typeof(SeqLoadInspPnP.tePosX));
						break;
					case SeqID.LDINSPPNP_PNP:
						cbPick.DataSource	= new List<string> { "BUF1", "BUF2" };
						cbPlace.DataSource	= new List<string> { "INSP1", "INSP2" };
						break;
					case SeqID.ULDINSPPNP_PICK:
						cbPlace.Enabled		= false;
						lbPlace.Enabled		= false;
						cbPick.DataSource	= new List<string> { "INSP1", "INSP2" };
						break;
					case SeqID.ULDINSPPNP_PLACE:
						cbPick.Enabled		= false;
						lbPick.Enabled		= false;
						cbPlace.DataSource	= Enum.GetValues(typeof(SeqUnloadInspPnP.PosX));
						break;
					case SeqID.ULDINSPPNP_PNP:
						cbPick.DataSource	= new List<string> { "INSP1", "INSP2" };
						cbPlace.DataSource	= Enum.GetValues(typeof(SeqUnloadInspPnP.PosX));
						break;
					case SeqID.ULDTRANS_PICK:
						cbPlace.Enabled		= false;
						lbPlace.Enabled		= false;
						cbPick.DataSource	= Enum.GetValues(typeof(SeqUnloadTransfer.PosX));
						break;
					case SeqID.ULDTRANS_PLACE:
						cbPick.Enabled		= false;
						lbPick.Enabled		= false;
						cbPlace.DataSource	= Enum.GetValues(typeof(SeqUnloadTransfer.PosX));
						break;

					case SeqID.ULDTRANS_PNP:
						cbPick.DataSource 	= Enum.GetValues(typeof(SeqUnloadTransfer.PosX));
						cbPlace.DataSource 	= Enum.GetValues(typeof(SeqUnloadTransfer.PosX));
						break;
				}
				cbPick.Items.Remove("UNKWNON");
				cbPlace.Items.Remove("UNKWNON");

				this.ShowDialog();

				return this.DialogResult;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return DialogResult.Cancel;
			}
		}


		private void frmPnPSelect_Load(object sender, EventArgs e)
		{
			m_selectedPick = "";
			m_selectedPlace = "";
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			try
			{
				object[] Params	= new object[2];

				if (cbPick.Enabled == false)
				{
					Params[0]		= -1;
				}
				else
				{
					m_selectedPick	= cbPick.SelectedValue.ToString();
					Params[0]		= cbPick.SelectedIndex;
				}

				if (cbPlace.Enabled == false)
				{
					Params[1]		= -1;
				}
				else
				{
					m_selectedPlace	= cbPlace.SelectedValue.ToString();
					Params[1]		= cbPlace.SelectedIndex;
				}

				SM.SHM.SetManualParams(Params);

				this.DialogResult = DialogResult.OK;
				Close();

			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		 
		}
		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			Close();
		}
	}
}
