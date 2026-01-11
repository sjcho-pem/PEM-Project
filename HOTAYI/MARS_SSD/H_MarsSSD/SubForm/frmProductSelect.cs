using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PMN.Control.WPF.MachineAni;

namespace H_MarsSSD
{
	public partial class frmProductSelect : Form
	{

		#region [ Varibles ]

		public	ucTraynPartFigure	m_TraynPartFigure;
		private	FlowTray			m_flowtray			= FlowTray.Empty;
		private	FlowPart			m_flowpart			= FlowPart.Empty;
		private bool				m_bUpdateTray		= false;
		private bool				m_bUpdatePart		= false;

		#endregion

		public frmProductSelect()
		{
			InitializeComponent();
		}

		#region [ Timmer Function ]
		private void tmrDelay_Tick(object sender, EventArgs e)
		{
			tmrDelay.Stop();

			if (m_bUpdateTray == true)
			{
				m_TraynPartFigure.UpdateTrayFigure();
				m_bUpdateTray = false;
			}
			if (m_bUpdatePart == true)
			{
				m_TraynPartFigure.UpdatePartFigure();
				m_bUpdatePart = false;
			}
		}

		#endregion

		#region [ Event Functions ]

		private void frmProductSelect_Load(object sender, EventArgs e)
		{
			try
			{
				tmrDelay.Start();
			}
			catch(Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		#endregion

		#region [ Button Functions ]

		#endregion

		#region [ Functions ]


		#region - Public Functions

		public DialogResult ShowDialog(FlowTray tray, List<int> deleteTargetPart, FlowPart flowpartOnlyDeleteTarget, string description = "") 
		{
			try
			{
				m_flowtray = tray;

				

				m_TraynPartFigure = new ucTraynPartFigure(tray, deleteTargetPart, flowpartOnlyDeleteTarget);

				elementHost1.Child	= m_TraynPartFigure;
				m_TraynPartFigure.WinFormInstance = this;

				if (string.IsNullOrWhiteSpace(description) == false)
					lbFigureName.Text	= tray.ToString();
				else
					lbFigureName.Text	= description;

				m_bUpdateTray		= true;

				this.ShowDialog();

				return this.DialogResult;
			}
			catch(Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return DialogResult.Cancel;
			}
		}

		public DialogResult ShowDialog(FlowPart part, string description = "")
		{
			try
			{
				m_flowpart = part;	

				m_TraynPartFigure = new ucTraynPartFigure(part);

				elementHost1.Child	= m_TraynPartFigure;
				lbFigureName.Text	= part.ToString();
				m_bUpdatePart		= true;

				this.ShowDialog();

				return this.DialogResult;
			}
			catch(Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return DialogResult.Cancel;
			}
		}

		#endregion

		#region - Privates Functions

		#endregion

		#endregion

	}
}
