using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace H_MarsSSD
{
    public partial class frmLevelSetting : Form
    {
        #region [ Variables ]
        private bool bIsChanged = false;
        private int nSelectCombo;
        private XML xmlLevel = new XML();
        #endregion [ Variables ] (End)

        #region [ Initialize ]
        public frmLevelSetting()
        {
            InitializeComponent();
            InitialXML();
            InitDicChecked();
            cbLevelSetting.MouseWheel += cbLevelSetting_MouseWheel;   //콤보박스 마우스 휠 금지
        }

        private void InitialXML()
        {
            xmlLevel.Initializ("Level");
        }
        #endregion [ Initialize ] (End)

        #region [ Event ]

        private void frmLevelSetting_Load(object sender, EventArgs e)
        {
            cbLevelSetting.SelectedIndex = 0;
            nSelectCombo = cbLevelSetting.SelectedIndex;
            ReadLevelSettings();
        }

        private void cbLevelSetting_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bIsChanged == true)
            {
                //이벤트 반복 금지 조건문
                if (cbLevelSetting.SelectedIndex == nSelectCombo) 
                {
                    frmMessageBox msg = new frmMessageBox(true, Language.languageDefault.MB_Confirmation, "Save your changes?");
                    if (msg.ShowDialog() == DialogResult.OK)
                        WriteLevelSettings();
                    else
                        ReadLevelSettings();
                }
                else
                    cbLevelSetting.SelectedIndex = nSelectCombo;
            }
            else
            {
                nSelectCombo = cbLevelSetting.SelectedIndex;
                ReadLevelSettings();
            }
        }

        private void chkTotal_CheckedChanged(object sender, EventArgs e)
        {
            //저장된 체크박스와 변경된 체크박스를 비교
            foreach (var chk in dicChecked)
            {
                if (chk.Key.Checked != chk.Value)
                    bIsChanged = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            nSelectCombo = cbLevelSetting.SelectedIndex;
            frmMessageBox msg = new frmMessageBox(true, Language.languageDefault.MB_Confirmation, "Save your changes?");
            if (msg.ShowDialog() == DialogResult.OK)
            {
                if (WriteLevelSettings() == false)
                {
                    frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Error, "Save Failure!");
                    pMsg.ShowDialog();
                }
            }
        }

        private void frmLevelSetting_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            SolidBrush pBr = new SolidBrush(Color.Black);
            Pen pPen = new Pen(pBr, 1);

            Size _size = this.Size;
            g.DrawRectangle(pPen, 1, 1, _size.Width - 3, _size.Height - 3);

            pBr.Dispose();
            pPen.Dispose();
        }

        private void btnSave_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSave.PerformClick();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSetDefault_Click(object sender, EventArgs e)
        {
            frmMessageBox msg = new frmMessageBox(true, Language.languageDefault.MB_Confirmation, "Set Default?");
            if (msg.ShowDialog() == DialogResult.OK)
            {
                if (cbLevelSetting.SelectedIndex == 0)
                {
                    //OPERATOR
                    chkMain.Checked = true;
                    chkData.Checked = true;
                    chkManual.Checked = false;
                    chkMotor.Checked = false;
                    chkIO.Checked = true;
                    chkError.Checked = false;
                    chkLog.Checked = true;
                    chkRecipe.Checked = true;
                    chkDM.Checked = false;
                    chkSDM.Checked = false;
                    chkBit.Checked = false;
                    chkUseSkip.Checked = false;
                    chkTeaching.Checked = false;
                    chkPw.Checked = true;
                }
                else if (cbLevelSetting.SelectedIndex == 1)
                {
                    //ENGINEAR
                    chkMain.Checked = true;
                    chkData.Checked = true;
                    chkManual.Checked = true;
                    chkMotor.Checked = false;
                    chkIO.Checked = true;
                    chkError.Checked = false;
                    chkLog.Checked = true;
                    chkRecipe.Checked = true;
                    chkDM.Checked = false;
                    chkSDM.Checked = false;
                    chkBit.Checked = false;
                    chkUseSkip.Checked = false;
                    chkTeaching.Checked = false;
                    chkPw.Checked = true;
                }
                else
                {
                    //PROFESSIONAL
                    chkMain.Checked = true;
                    chkData.Checked = true;
                    chkManual.Checked = true;
                    chkMotor.Checked = true;
                    chkIO.Checked = true;
                    chkError.Checked = true;
                    chkLog.Checked = true;
                    chkRecipe.Checked = true;
                    chkDM.Checked = true;
                    chkSDM.Checked = true;
                    chkBit.Checked = true;
                    chkUseSkip.Checked = true;
                    chkTeaching.Checked = true;
                    chkPw.Checked = false;
                }
            }
        }

        private void cbLevelSetting_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        #endregion [ Event ] (End)

        #region [ Method ]

        private Dictionary<CheckBox, bool> dicChecked = new Dictionary<CheckBox, bool>();
        private void InitDicChecked()
        {
            dicChecked = new Dictionary<CheckBox, bool>
            {
                { chkMain,      chkMain.Checked    },
                { chkData,      chkData.Checked    },
                { chkManual,    chkManual.Checked  },
                { chkMotor,     chkMotor.Checked   },
                { chkIO,        chkIO.Checked      },
                { chkError,     chkError.Checked   },
                { chkLog,       chkLog.Checked     },
                { chkRecipe,    chkRecipe.Checked  },
                { chkDM,        chkDM.Checked      },
                { chkSDM,       chkSDM.Checked      },
                { chkBit,       chkBit.Checked     },
                { chkUseSkip,   chkUseSkip.Checked },
                { chkTeaching,   chkTeaching.Checked },
                { chkPw,        chkPw.Checked      }
            };
        }

        public bool ReadLevelXMLData(int No, string sColName)
        {
            return xmlLevel.iReadDataTable(No, sColName) == 1 ? true : false;
        }

        private void ReadLevelSettings()
        {
            try
            {
                //레벨 불러오기
                int nIdx = cbLevelSetting.SelectedIndex;

                chkMain.Checked     = ReadLevelXMLData(nIdx, "MAIN");
                chkData.Checked     = ReadLevelXMLData(nIdx, "DATA");
                chkManual.Checked   = ReadLevelXMLData(nIdx, "MANUAL");
                chkMotor.Checked    = ReadLevelXMLData(nIdx, "MOTOR");
                chkIO.Checked       = ReadLevelXMLData(nIdx, "IO");
                chkError.Checked    = ReadLevelXMLData(nIdx, "ERROR");
                chkLog.Checked      = ReadLevelXMLData(nIdx, "LOG");
                chkRecipe.Checked   = ReadLevelXMLData(nIdx, "RECIPE");
                chkDM.Checked       = ReadLevelXMLData(nIdx, "DM");
                chkSDM.Checked      = ReadLevelXMLData(nIdx, "DM");
                chkBit.Checked      = ReadLevelXMLData(nIdx, "BIT");
                chkUseSkip.Checked  = ReadLevelXMLData(nIdx, "USESKIP");
                chkTeaching.Checked = ReadLevelXMLData(nIdx, "TEACHING");
                chkPw.Checked       = ReadLevelXMLData(nIdx, "RECONFIRM_PW");
                bIsChanged = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool WriteLevelSettings()
        {
            try
            {
                //레벨 저장
                int nIdx = cbLevelSetting.SelectedIndex;

                xmlLevel.WriteDataTable(nIdx, "MAIN",           chkMain.Checked     ? 1 : 0);
                xmlLevel.WriteDataTable(nIdx, "DATA",           chkData.Checked     ? 1 : 0);
                xmlLevel.WriteDataTable(nIdx, "MANUAL",         chkManual.Checked   ? 1 : 0);
                xmlLevel.WriteDataTable(nIdx, "MOTOR",          chkMotor.Checked    ? 1 : 0);
                xmlLevel.WriteDataTable(nIdx, "IO",             chkIO.Checked       ? 1 : 0);
                xmlLevel.WriteDataTable(nIdx, "ERROR",          chkError.Checked    ? 1 : 0);
                xmlLevel.WriteDataTable(nIdx, "LOG",            chkLog.Checked      ? 1 : 0);
                xmlLevel.WriteDataTable(nIdx, "RECIPE",         chkRecipe.Checked   ? 1 : 0);
                xmlLevel.WriteDataTable(nIdx, "DM",             chkDM.Checked       ? 1 : 0);
                xmlLevel.WriteDataTable(nIdx, "SDM",            chkSDM.Checked      ? 1 : 0);
                xmlLevel.WriteDataTable(nIdx, "BIT",            chkBit.Checked      ? 1 : 0);
                xmlLevel.WriteDataTable(nIdx, "USESKIP",        chkUseSkip.Checked  ? 1 : 0);
                xmlLevel.WriteDataTable(nIdx, "TEACHING",       chkTeaching.Checked ? 1 : 0);
                xmlLevel.WriteDataTable(nIdx, "RECONFIRM_PW",   chkPw.Checked       ? 1 : 0);
                xmlLevel.UpdateData();
                bIsChanged = false;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        #endregion [ Method ] (End)

    }
}
