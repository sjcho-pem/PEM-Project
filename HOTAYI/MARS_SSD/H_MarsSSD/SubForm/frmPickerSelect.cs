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
    public partial class frmPickerSelect : Form
    {
        public frmPickerSelect()
        {
            InitializeComponent();
        }

        #region [ Variables ]

            int[] m_nPickerCheck = new int[4] { 0, 0, 0, 0 }; // 각 피커 Vac 상태 변경 체크, ON: 1, OFF: 0 
        #endregion

        #region [ Event Function ]

        #endregion

        #region [ Timmer Function ]

        #endregion

        #region [ Button Event ]

        private void btnPicker_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            if (btn.BackColor == Color.White)
            {
                btn.BackColor = Color.Lime;
                btn.Text = "ON";
                SetPickerVac(Convert.ToInt32((string)btn.Tag), true, Color.Lime);
            }
            else
            {
                btn.BackColor = Color.White;
                btn.Text = "OFF";
                SetPickerVac(Convert.ToInt32((string)btn.Tag), false, Color.GhostWhite);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            frmMessageBox pMsg = new frmMessageBox(true, Language.languageDefault.MB_Warnning, Language.languageDefault.MB_VacSave);
            if (pMsg.ShowDialog() == DialogResult.Cancel) return;
            
            btnSave.Enabled = false;

            UI.SubpageRecipe.UpdateRecipeDataGrid(UI.SubpageRecipe.iNowRecipeNum); // Recipe Data Grid Refresh

            string msg_ = string.Format("[{0:D2}] Picker Vacuum Save !!", UI.SubpageRecipe.iNowRecipeNum + 1);
            UI.Logs.Write(LOGPART.DATA, msg_);
        }
        #endregion

        #region [ Function ]

        #region - Private Function

        

        private void SetPickerVac(int nPickerNum, bool bPickerOnOff, Color color)
        {
            int nPickerOnOff = 0; // ON: 1, OFF: 0 
            if (bPickerOnOff)
            {
                nPickerOnOff = 1;
            }

            switch (nPickerNum)
            {
                case 1:
                    lbVacNum1.BackColor = color;
                    m_nPickerCheck[0] = nPickerOnOff;
                    break;
                case 2:
                    lbVacNum2.BackColor = color;
                    m_nPickerCheck[1] = nPickerOnOff;
                    break;
                case 3:
                    lbVacNum3.BackColor = color;
                    m_nPickerCheck[2] = nPickerOnOff;
                    break;
                case 4:
                    lbVacNum4.BackColor = color;
                    m_nPickerCheck[3] = nPickerOnOff;
                    break;
            }

        }
        #endregion

        #region - Public Function

        #endregion

        #region - Language Function 

        public void SetLanguageText()
        {
            lbVacNum1.Text = Language.languageDefault.Vacuum1;
            lbVacNum2.Text = Language.languageDefault.Vacuum2;
            lbVacNum3.Text = Language.languageDefault.Vacuum3;
            lbVacNum4.Text = Language.languageDefault.Vacuum4;
            lbVac1.Text = Language.languageDefault.Vacuum1;
            lbVac2.Text = Language.languageDefault.Vacuum2;
            lbVac3.Text = Language.languageDefault.Vacuum3;
            lbVac4.Text = Language.languageDefault.Vacuum4;
            btnSave.Text = Language.languageDefault.Save;
        }
        #endregion
        #endregion






    }
}
