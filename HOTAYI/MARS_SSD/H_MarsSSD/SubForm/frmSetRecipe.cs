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
	public partial class frmSetRecipe : Form
	{
		private	XML		XMLRcpMng	= new XML();
		private	XML		XMLRecipe	= new XML();

		public			frmSetRecipe(string	strPartNo)
		{
			InitializeComponent();

			tbPartNo.Text	= strPartNo;
		}

		private	void	frmSetRecipe_Load(object sender, EventArgs e)
		{
			try
			{
				XMLRcpMng.Initializ("RecipeManager");
				XMLRecipe.Initializ("Recipe");

				InitXml();
				InitComboRecipe();
				cbRecipe.SelectedIndex	= 0;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	btnOK_Click(object sender, EventArgs e)
		{
			try
			{
				string	str_part_no	= tbPartNo.Text;
				string	str_recipe	= "";

				if (cbRecipe.SelectedIndex >= 0)
				{
					str_recipe	= cbRecipe.SelectedItem.ToString();
				}
				
				AddXml(tbPartNo.Text, str_recipe);

				this.DialogResult	= DialogResult.OK;
				Close();
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}

		}

		private	void	btnCancel_Click(object sender, EventArgs e)
		{
			try
			{
				this.DialogResult	= DialogResult.Cancel;
				Close();
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
					string	str_part_no	= XMLRcpMng.ReadDataTable(i, "PartNo");
					string	str_recipe	= XMLRcpMng.ReadDataTable(i, "Recipe");

					if (string.IsNullOrWhiteSpace(str_part_no) == true  ||  string.IsNullOrWhiteSpace(str_recipe) == true)
					{
						XMLRcpMng.RemoveDataTable(i, "PartNo");
						XMLRcpMng.RemoveDataTable(i, "Recipe");
						continue;
					}

					if (i != ncnt)
					{
						XMLRcpMng.AddDataTable(ncnt, "PartNo", str_part_no);
						XMLRcpMng.AddDataTable(ncnt, "Recipe", str_recipe);

						XMLRcpMng.RemoveDataTable(i, "PartNo");
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

		private	void	AddXml(string strPartNo, string strRecipe)
		{
			try
			{
				int ncnt_row	= XMLRcpMng.GetDataTableCount();
				int i			= 0;

				for (i = 0; i < ncnt_row; i++)
				{
					string	str_part_no	= XMLRcpMng.ReadDataTable(i, "PartNo");
					string	str_recipe	= XMLRcpMng.ReadDataTable(i, "Recipe");

					if (string.IsNullOrWhiteSpace(str_part_no) == true  ||  string.IsNullOrWhiteSpace(str_recipe) == true)
					{
						XMLRcpMng.WriteDataTable(i, "PartNo", strPartNo);
						XMLRcpMng.WriteDataTable(i, "Recipe", strRecipe);
						break;
					}
				}

				if (ncnt_row == i)
				{
					XMLRcpMng.AddDataTable(i, "PartNo", strPartNo);
					XMLRcpMng.AddDataTable(i, "Recipe", strRecipe);
				}

				XMLRcpMng.UpdateData();
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
	}
}
