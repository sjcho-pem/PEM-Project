using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace H_MarsSSD
{
    public partial class frmErrDescView : Form
    {
        public frmErrDescView()
        {
            InitializeComponent();
            //this.Size = new System.Drawing.Size(735, 650);
        }

        #region [ Variables ]

        #endregion

        #region [ Event Function ]

        private void frmErrDescView_Load(object sender, EventArgs e)
        {
            lbErrCode.Text = UI.PageError.GetErrorName(UI.PageError.GetErrorCode());
            richtxtErrDesc.Text = UI.PageError.GetErrorDescription(UI.PageError.GetErrorCode());

            DisplayGrid();

            #region -Get Error Image path DataBase
            SetDisplayFromGetErrorImgPathDB(UI.PageError.GetErrorCode(0));
            #endregion
        }

        private void frmErrDescView_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void gridErrList_SelectionChanged(object sender, EventArgs e)
        {
            #region --Gets the associated Row index for the DataGridView.
            DataGridView dgv = sender as DataGridView;
            int nNowRowIdx = dgv.CurrentRow.Index;
            if (nNowRowIdx == -1)
                return;
            #endregion

            lbErrCode.Text = UI.PageError.GetErrorName(UI.PageError.GetErrorCode(nNowRowIdx));
            richtxtErrDesc.Text = UI.PageError.GetErrorDescription(UI.PageError.GetErrorCode(nNowRowIdx));

            #region --Set Display to Image and path.
            SetDisplayFromGetErrorImgPathDB(UI.PageError.GetErrorCode(nNowRowIdx));
            #endregion
        }

		private void picErrorImage_DoubleClick(object sender, EventArgs e)
		{
			int		nerror_no	= UI.PageError.GetErrorCode(gridErrList.CurrentRow.Index);
			string	str_file	= UI.PageError.GetErrorImagePath(nerror_no);

			if (File.Exists(str_file) == false)
			{
				frmMessageBox msg	= new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "No File");
				msg.Show();
				return;
			}

			Form fc = Application.OpenForms["frmImageViewer"];
			if (fc != null)
			{
				((frmImageViewer)fc).SetImage(str_file);
				return;
			}

			frmImageViewer	viewer	= new frmImageViewer(str_file);
			viewer.Show();
		}
        #endregion

        #region [ Timmer Function ]

        #endregion

        #region [ Button Event ]

        #endregion

        #region [ Function ]

        #region - Private Function

        private void DisplayGrid()
        {
            gridErrList.Rows.Clear();
            for (int i = 0; i < 10; i++)
			{
				string[] arrStr = GetRow(i);
                if (arrStr.Length <= 2)     continue;
                if (arrStr[1] == "0")		continue;
				
				gridErrList.Rows.Add(GetRow(i));
			}

		}

        private string[] GetRow(int index)
        {
            string[] row = new string[]
            {
                index.ToString(),
                UI.PageError.GetErrorCode(index).ToString(),
                UI.PageError.GetErrorName(UI.PageError.GetErrorCode(index))
            };

            return row;
        }

        private void SetErrorImage(string path)
        {
            try
            {
                if (!string.IsNullOrEmpty(path))
                {
                    if (File.Exists(path))
                    {
                        Image img = Image.FromFile(path);
                        picErrorImage.BackgroundImage = img;
                    }
                    else
                    {
                        picErrorImage.BackgroundImage = null;
                    }
                }
                else
                {
                    picErrorImage.BackgroundImage = null;
                }
            }
            catch (Exception ex)
            {
                UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
            }
        }

        private void SetDisplayFromGetErrorImgPathDB(int index)
        {
            #region -Get Error Image path DataBase
            string imgPath = UI.PageError.GetErrorImagePath(index);
            SetErrorImage(imgPath);
            #endregion
        }

		#endregion

		#region - Public Function

		#endregion

		#region - Language Function 

		#endregion

		#endregion

    }
}
