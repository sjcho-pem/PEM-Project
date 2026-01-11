using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace H_MarsSSD
{
	public partial class frmTrayPnPRecipeEditor : Form
	{
		public			frmTrayPnPRecipeEditor(int nRecipeNo)
		{
			InitializeComponent();

			m_nRecipeNo	= nRecipeNo;
			m_strFile	= string.Format("{0}Recipe_{1:000}\\TrayPnPRecipe.Dat", Program.DataDirectory, nRecipeNo);
		}

		#region [ Variables ]
		private	tcTrayPnPInfo	m_PnPnfo	= new tcTrayPnPInfo();
		private	object			m_FileLock	= new object();
		private	int				m_nRecipeNo	= -1;
		private	string			m_strFile	= string.Empty;
        #endregion

        #region [ Event Function ]

		private void	frmTrayPnPRecipeEditor_Load(object sender, EventArgs e)
		{
			try
			{
				m_PnPnfo	= TrayPnPRecipe.FileOpen(m_strFile);

				if (m_PnPnfo != null)
				{
					tbTrayMaxRow.Text		= m_PnPnfo.Tray.RowsGap.Count.ToString();
					tbTrayMaxCol.Text		= m_PnPnfo.Tray.ColsGap.Count.ToString();

					tbLdPnPMaxRow.Text		= m_PnPnfo.LoadPicker.RowsGap.Count.ToString();
					tbLdPnPMaxCol.Text		= m_PnPnfo.LoadPicker.ColsGap.Count.ToString();

					tbUldPnPMaxRow.Text		= m_PnPnfo.UnloadPicker.RowsGap.Count.ToString();
					tbUldPnPMaxCol.Text		= m_PnPnfo.UnloadPicker.ColsGap.Count.ToString();
				}
				else
				{
					m_PnPnfo = new tcTrayPnPInfo();

					tbTrayMaxRow.Text		= "0";
					tbTrayMaxCol.Text		= "0";
					
					tbLdPnPMaxRow.Text		= "0";
					tbLdPnPMaxCol.Text		= "0";
					
					tbUldPnPMaxRow.Text		= "0";
					tbUldPnPMaxCol.Text		= "0";
				}
				
				TabGridUpdate("Tray");
				TabGridUpdate("LoadPnP");
				TabGridUpdate("UnloadPnP");

				ToolPropertyUpdate();
				SetLanguageText();
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
		
		private void	frmTrayPnPRecipeEditor_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				if (UI.SubpageRecipe != null)
				{
					UI.SubpageRecipe.WriteRecipeData((int)DEVICE.TrayColCnt, m_PnPnfo.Tray.ColsGap.Count * 1000);
					UI.SubpageRecipe.WriteRecipeData((int)DEVICE.TrayRowCnt, m_PnPnfo.Tray.RowsGap.Count * 1000);
				}
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	tbMaxVal_TextChanged(object sender, EventArgs e)
		{
			try
			{
				TabGridUpdate(tabSetGap.SelectedTab.Text);
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void	tabSetGap_Selected(object sender, TabControlEventArgs e)
		{
			try
			{
				TabGridUpdate(tabSetGap.SelectedTab.Text);
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
		
		private void	checkBox_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				if (sender == null)				return;
				CheckBox checkbox = (CheckBox)sender;

				if	(checkbox.Checked == true)			checkbox.BackColor = Color.Yellow;
				else									checkbox.BackColor = Color.LightGray;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
		
		private void	dgv_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				if (e.RowIndex == -1)						return;
				if (e.ColumnIndex == -1)					return;
				if (e.RowIndex == 0 && e.ColumnIndex == 0)	return;
				if (sender == null)							return;

				DataGridView	dgv		= (DataGridView)sender;
				Label			lb		= new Label();
				
				frmKeyPad pad_num = new frmKeyPad(lb, dgv[e.ColumnIndex, e.RowIndex].Value.ToString(), KeyPadType.unsigned);
				if (pad_num.ShowDialog() == DialogResult.Cancel)
				{
					dgv.Rows[e.RowIndex].Selected = false;
					return;
				}

				dgv[e.ColumnIndex, e.RowIndex].Value = lb.Text;
				dgv.Rows[e.RowIndex].Selected = false;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
        #endregion

        #region [ Timmer Function ]
        
        #endregion

        #region [ Button Event ]
		private	void	btnSave_Click(object sender, EventArgs e)
		{
			try
			{
				DataUpdate();

				TrayPnPRecipe.FileSave(m_strFile, m_PnPnfo);
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	btnOpen_Click(object sender, EventArgs e)
		{
			try
			{
				m_PnPnfo	= TrayPnPRecipe.FileOpen(m_strFile);

				TabGridUpdate("Tray");
				TabGridUpdate("LoadPnP");
				TabGridUpdate("UnloadPnP");
				TabGridUpdate("SortPnP");
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
        #endregion

        #region [ Function ]

        #region - Private Function
		private	void	TabGridUpdate(string strTabText)
		{
			try
			{
				int	nmax_row;
				int	nmax_col;

				switch (strTabText)
				{
					case "Tray" :
						if (int.TryParse(tbTrayMaxRow.Text, out nmax_row) == false)				return;
						if (int.TryParse(tbTrayMaxCol.Text, out nmax_col) == false)				return;
						GridUpdateGap(nmax_row, nmax_col, dgvTrayRow, dgvTrayCol, m_PnPnfo.Tray);
						break;

					case "LoadPnP" :
						if (int.TryParse(tbLdPnPMaxRow.Text, out nmax_row) == false)			return;
						if (int.TryParse(tbLdPnPMaxCol.Text, out nmax_col) == false)			return;
						GridUpdateGap(nmax_row, nmax_col, dgvLdPnPRow, dgvLdPnPCol, m_PnPnfo.LoadPicker);
						break;

					case "UnloadPnP" :
						if (int.TryParse(tbUldPnPMaxRow.Text, out nmax_row) == false)			return;
						if (int.TryParse(tbUldPnPMaxCol.Text, out nmax_col) == false)			return;
						GridUpdateGap(nmax_row, nmax_col, dgvUldPnPRow, dgvUldPnPCol, m_PnPnfo.UnloadPicker);
						break;
				}
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	GridUpdateGap(int nMaxRow, int nMaxCol, DataGridView dgvRow, DataGridView dgvCol, tsGapSize GapSize)
		{
			try
			{
				dgvCol.ColumnCount	= nMaxCol;
				dgvCol.RowCount		= 1;
				dgvRow.ColumnCount	= 1;
				dgvRow.RowCount		= nMaxRow;

				dgvCol.Rows[0].Height	= dgvTrayCol.Height;
				for (int i = 0; i < nMaxCol; i++)
				{
					dgvCol.Columns[i].AutoSizeMode	= DataGridViewAutoSizeColumnMode.Fill;
					dgvCol.Columns[i].FillWeight	= 1;

					if (i == 0)
					{
						dgvCol.Columns[i].DefaultCellStyle.BackColor	= Color.DarkGray;
						dgvCol.Columns[i].ReadOnly						= true;
					}

					if (GapSize.ColsGap.Count > i)			dgvCol[i, 0].Value	= GapSize.ColsGap[i];
					else									dgvCol[i, 0].Value	= 0;
				}

				for (int i = 0; i < nMaxRow; i++)
				{
					dgvRow.Rows[i].Height	= (dgvRow.Height - dgvRow.ColumnHeadersHeight - 1) / nMaxRow;

					if (i == 0)
					{
						dgvRow.Rows[i].DefaultCellStyle.BackColor	= Color.DarkGray;
						dgvRow.Rows[i].ReadOnly						= true;
					}

					if (GapSize.RowsGap.Count > i)			dgvRow[0, i].Value	= GapSize.RowsGap[i];
					else									dgvRow[0, i].Value	= 0;
				}

				dgvCol.DefaultCellStyle.SelectionBackColor	= Color.White;
				dgvRow.DefaultCellStyle.SelectionBackColor	= Color.White;
				dgvCol.DefaultCellStyle.SelectionForeColor	= Color.Black;
				dgvRow.DefaultCellStyle.SelectionForeColor	= Color.Black;
				dgvCol.DefaultCellStyle.Alignment			= DataGridViewContentAlignment.MiddleCenter;
				dgvRow.DefaultCellStyle.Alignment			= DataGridViewContentAlignment.MiddleCenter;
				dgvCol.ScrollBars							= ScrollBars.None;
				dgvRow.ScrollBars							= ScrollBars.None;
				dgvCol.SelectionMode						= DataGridViewSelectionMode.FullRowSelect;
				dgvRow.SelectionMode						= DataGridViewSelectionMode.FullRowSelect;
				dgvCol.ClearSelection();
				dgvRow.ClearSelection();
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}


		private	void	DataUpdate()
		{
			try
			{
				DataGridView[]	dgvs_col	= new DataGridView[] { dgvTrayCol, dgvLdPnPCol, dgvUldPnPCol };
				DataGridView[]	dgvs_row	= new DataGridView[] { dgvTrayRow, dgvLdPnPRow, dgvUldPnPRow };
				tsGapSize[]		gap_size	= new tsGapSize[] { m_PnPnfo.Tray, m_PnPnfo.LoadPicker, m_PnPnfo.UnloadPicker };

				double			dval		= 0;

				for (int ndgv = 0; ndgv < 3; ndgv++)
				{
					gap_size[ndgv].ColsGap.Clear();
					gap_size[ndgv].RowsGap.Clear();

					for (int i = 0; i < dgvs_col[ndgv].ColumnCount; i++)
					{
						if (dgvs_col[ndgv][i, 0].Value != null  &&
							double.TryParse(dgvs_col[ndgv][i, 0].Value.ToString(), out dval) == true)
						{
							gap_size[ndgv].ColsGap.Add(dval);
						}
						else
						{
							gap_size[ndgv].ColsGap.Add(0);
						}
					}

					for (int i = 0; i < dgvs_row[ndgv].RowCount; i++)
					{
						if (dgvs_row[ndgv][0, i].Value != null  &&
							double.TryParse(dgvs_row[ndgv][0, i].Value.ToString(), out dval) == true)
						{
							gap_size[ndgv].RowsGap.Add(dval);
						}
						else
						{
							gap_size[ndgv].RowsGap.Add(0);
						}
					}
				}

				m_PnPnfo.UsedRowLdPnP.Clear();
				m_PnPnfo.UsedRowUldPnP.Clear();
				m_PnPnfo.UsedRowSortPnP.Clear();

			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	ToolPropertyUpdate()
		{
			try
			{
				lbLDPnPCol.Location			= lbTrayCol.Location;
				lbLDPnPRow.Location			= lbTrayRow.Location;
				lbULDPnPCol.Location		= lbTrayCol.Location;
				lbULDPnPRow.Location		= lbTrayRow.Location;

				tbLdPnPMaxCol.Location		= tbTrayMaxCol.Location;
				tbLdPnPMaxRow.Location		= tbTrayMaxRow.Location;
				tbUldPnPMaxCol.Location		= tbTrayMaxCol.Location;
				tbUldPnPMaxRow.Location		= tbTrayMaxRow.Location;

				dgvLdPnPCol.Location		= dgvTrayCol.Location;
				dgvLdPnPRow.Location		= dgvTrayRow.Location;
				dgvUldPnPCol.Location		= dgvTrayCol.Location;
				dgvUldPnPRow.Location		= dgvTrayRow.Location;

				dgvLdPnPCol.Size			= dgvTrayCol.Size;
				dgvLdPnPRow.Size			= dgvTrayRow.Size;
				dgvUldPnPCol.Size			= dgvTrayCol.Size;
				dgvUldPnPRow.Size			= dgvTrayRow.Size;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
        #endregion

        #region - Public Function

        #endregion

        #region - Language Function 
		public void SetLanguageText()
        {
			try
			{
				btnSave.Text = Language.languageDefault.Save.ToUpper();
				btnImport.Text = "IMPORT";
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
        }
		#endregion

		#endregion

	}
}
