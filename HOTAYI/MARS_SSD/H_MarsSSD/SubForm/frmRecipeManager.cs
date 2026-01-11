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
	public partial class frmRecipeManager : Form
	{
		private	XML		XMLRcpMng	= new XML();
		private	XML		XMLRecipe	= new XML();


		public			frmRecipeManager()
		{
			InitializeComponent();
		}

		private	void	frmRecipeManager_Load(object sender, EventArgs e)
		{
			try
			{
				XMLRcpMng.Initializ("RecipeManager");
				XMLRecipe.Initializ("Recipe");

				InitXml();
				InitGrid();
				InitComboRecipe();
				cbRecipe.SelectedIndex	= 0;
				SetLanguageText();
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	tbDeviceID_Click(object sender, EventArgs e)
		{
			try
			{
				frmKeyboard	pup	= new frmKeyboard(tbDeviceID.Text);

				if (pup.ShowDialog() == DialogResult.Cancel)			return;
				
				tbDeviceID.Text = pup.Value;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	btnAdd_Click(object sender, EventArgs e)
		{
			try
			{
				string	str_part_no	= tbDeviceID.Text;
				string	str_recipe	= "";

				if (cbRecipe.SelectedIndex >= 0)
				{
					str_recipe	= cbRecipe.SelectedItem.ToString();
				}

				if		(string.IsNullOrWhiteSpace(str_part_no) == true)
				{
					lbAddError.Text	= "* Please enter Part No.";
				}
				else if (ContainsDeviceID(str_part_no, new List<string>()) == true)
				{
					lbAddError.Text	= "* The same Part No already exists.";
				}
				else if (string.IsNullOrWhiteSpace(str_recipe) == true)
				{
					lbAddError.Text	= "* Please select Recipe.";
				}
				else
				{
					lbAddError.Text	= "";
					AddXml(str_part_no, str_recipe);
					UpdateGrid();
				}
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}

		}

		private	void	btnDel_Click(object sender, EventArgs e)
		{
			try
			{
				if (dgvInfo.SelectedRows.Count == 0)			return;

				frmMessageBox msg	= new frmMessageBox(frmMessageBox.teButtons.OKCancel, Language.languageDefault.MB_Question, Language.languageDefault.MB_Delete);
				if (msg.ShowDialog() == DialogResult.Cancel)			return;

				DataGridViewRow	row	= dgvInfo.SelectedRows[0];
				DelXml(row.Cells[1].Value.ToString());

				UpdateGrid();
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	btnClose_Click(object sender, EventArgs e)
		{
			try
			{
				Close();
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	dgvInfo_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				if (chkModify.Checked == false)			return;
				if (e.RowIndex < 0)						return;
				if (e.ColumnIndex < 1)					return;

				string	str_old_part_no	= dgvInfo[1, e.RowIndex].Value.ToString();

				if (e.ColumnIndex == 1)
				{
					frmKeyboard	pup	= new frmKeyboard(str_old_part_no);

					if (pup.ShowDialog() == DialogResult.Cancel)			return;
					
					if (ContainsDeviceID(pup.Value, new List<string>(){str_old_part_no}) == true)
					{
						frmMessageBox msg	= new frmMessageBox(frmMessageBox.teButtons.OK, "", "The same Part No already exists.");
						msg.ShowDialog();
						return;
					}

					dgvInfo[e.ColumnIndex, e.RowIndex].Value = pup.Value;
					ModifyXml(str_old_part_no, pup.Value, null);
				}
				else
				{
					string	str_recipe	= dgvInfo[e.ColumnIndex, e.RowIndex].Value.ToString();

					frmComboBoxSelect	pup	= new frmComboBoxSelect();

					if (pup.ShowDialog(cbRecipe, cbRecipe.Items.IndexOf(str_recipe)) == DialogResult.Cancel)			return;

					str_recipe	= cbRecipe.Items[pup.SelNo].ToString();
					dgvInfo[e.ColumnIndex, e.RowIndex].Value	= str_recipe;
					ModifyXml(str_old_part_no, null, str_recipe);
				}
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	chkModify_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				if (chkModify.Checked == true)
				{
					btnDel.Enabled	= true;
					btnAdd.Enabled	= true;
				}
				else
				{
					btnDel.Enabled	= false;
					btnAdd.Enabled	= false;
				}
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}



		private	void	InitXml()
		{
			try
			{
				int ncnt_row	= XMLRcpMng.GetDataTableCount();
				int	ncnt		= 0;

				for (int i = 0; i < ncnt_row; i++)
				{
					string	str_part_no	= XMLRcpMng.ReadDataTable(i, "DeviceID");
					string	str_recipe	= XMLRcpMng.ReadDataTable(i, "Recipe");

					if (string.IsNullOrWhiteSpace(str_part_no) == true  ||  string.IsNullOrWhiteSpace(str_recipe) == true)
					{
						XMLRcpMng.RemoveDataTable(i, "DeviceID");
						XMLRcpMng.RemoveDataTable(i, "Recipe");
						continue;
					}

					if (i != ncnt)
					{
						XMLRcpMng.AddDataTable(ncnt, "DeviceID", str_part_no);
						XMLRcpMng.AddDataTable(ncnt, "Recipe", str_recipe);

						XMLRcpMng.RemoveDataTable(i, "DeviceID");
						XMLRcpMng.RemoveDataTable(i, "Recipe");
					}

					ncnt++;
				}

				XMLRcpMng.UpdateData();
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	InitGrid()
		{
			try
			{
				int ncnt_row	= XMLRcpMng.GetDataTableCount();

				dgvInfo.Rows.Clear();

				for (int i = 0; i < ncnt_row; i++)
				{
					string	str_part_no	= XMLRcpMng.ReadDataTable(i, "DeviceID");
					string	str_recipe	= XMLRcpMng.ReadDataTable(i, "Recipe");

					if (string.IsNullOrWhiteSpace(str_part_no) == true  ||  string.IsNullOrWhiteSpace(str_recipe) == true)
					{
						continue;
					}

					dgvInfo.Rows.Add(i+1, str_part_no, str_recipe);
				}
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	InitComboRecipe()
		{
			try
			{
				string	str_name	= "";

				cbRecipe.Items.Clear();

				for (int i = 0; i < Define.MAX_RECIPE_NUM; i++)
				{
					str_name	= XMLRecipe.ReadDataTable(i, "RecipeName");
					if (string.IsNullOrWhiteSpace(str_name) == true)			continue;

					str_name	= string.Format("{0}.{1}", i+1, str_name);
					cbRecipe.Items.Add(str_name);
				}

			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	UpdateGrid()
		{
			try
			{
				int ncnt_row	= XMLRcpMng.GetDataTableCount();
				int	ncnt		= 0;

				for (int nrow = 0; nrow < ncnt_row; nrow++)
				{
					string	str_part_no	= XMLRcpMng.ReadDataTable(nrow, "DeviceID");
					string	str_recipe	= XMLRcpMng.ReadDataTable(nrow, "Recipe");

					if (string.IsNullOrWhiteSpace(str_part_no) == true  ||  string.IsNullOrWhiteSpace(str_recipe) == true)
					{
						continue;
					}

					if (dgvInfo.RowCount > nrow)
					{
						dgvInfo[1, nrow].Value	= str_part_no;
						dgvInfo[2, nrow].Value	= str_recipe;
					}
					else
					{
						dgvInfo.Rows.Add(nrow+1, str_part_no, str_recipe);
					}

					ncnt++;
				}

				dgvInfo.RowCount	= ncnt;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	bool	ContainsDeviceID(string strDeviceID, List<string> ltExceptionDeviceID)
		{
			try
			{
				for (int nrow = 0; nrow < dgvInfo.RowCount; nrow++)
				{
					if (dgvInfo[1, nrow].Value != null  &&
						ltExceptionDeviceID.Contains(strDeviceID, StringComparer.OrdinalIgnoreCase) == false  &&
						dgvInfo[1, nrow].Value.ToString().ToUpper() == strDeviceID.ToUpper())
					{
						return true;
					}
				}

				return false;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));

				return true;
			}
		}

		private	void	AddXml(string strDeviceID, string strRecipe)
		{
			try
			{
				int ncnt_row	= XMLRcpMng.GetDataTableCount();
				int i			= 0;

				for (i = 0; i < ncnt_row; i++)
				{
					string	str_part_no	= XMLRcpMng.ReadDataTable(i, "DeviceID");
					string	str_recipe	= XMLRcpMng.ReadDataTable(i, "Recipe");

					if (string.IsNullOrWhiteSpace(str_part_no) == true  ||  string.IsNullOrWhiteSpace(str_recipe) == true)
					{
						XMLRcpMng.WriteDataTable(i, "DeviceID", strDeviceID);
						XMLRcpMng.WriteDataTable(i, "Recipe", strRecipe);
						break;
					}
				}

				if (ncnt_row == i)
				{
					XMLRcpMng.AddDataTable(i, "DeviceID", strDeviceID);
					XMLRcpMng.AddDataTable(i, "Recipe", strRecipe);
				}

				XMLRcpMng.UpdateData();
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	ModifyXml(string strOldDeviceID, string strNewDeviceID, string strNewRecipe)
		{
			try
			{
				int ncnt_row	= XMLRcpMng.GetDataTableCount();
				int i			= 0;

				for (i = 0; i < ncnt_row; i++)
				{
					string	str_part_no	= XMLRcpMng.ReadDataTable(i, "DeviceID");

					if (str_part_no ==  strOldDeviceID)
					{
						if (string.IsNullOrWhiteSpace(strNewDeviceID) == false)
						{
							XMLRcpMng.WriteDataTable(i, "DeviceID", strNewDeviceID);
						}
						if (string.IsNullOrWhiteSpace(strNewRecipe) == false)
						{
							XMLRcpMng.WriteDataTable(i, "Recipe", strNewRecipe);
						}
						break;
					}
				}

				XMLRcpMng.UpdateData();
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	DelXml(string strDeviceID)
		{
			try
			{
				int ncnt_row	= XMLRcpMng.GetDataTableCount();
				int i			= 0;

				for (i = 0; i < ncnt_row; i++)
				{
					string	str_part_no	= XMLRcpMng.ReadDataTable(i, "DeviceID");

					if (strDeviceID == str_part_no)
					{
						XMLRcpMng.RemoveDataTable(i, "DeviceID");
						XMLRcpMng.RemoveDataTable(i, "Recipe");
					}
				}

				XMLRcpMng.UpdateData();

				InitXml();
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}



		private	void	SetLanguageText()
		{
			try
			{
				chkModify.Text	= Language.languageDefault.CHK_Modify;
				btnDel.Text		= Language.languageDefault.BTN_DELET;
				btnAdd.Text		= Language.languageDefault.BTN_ADD;
				btnClose.Text	= Language.languageDefault.BTN_Close;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
	}
}
