using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace H_MarsSSD
{

    public partial class frmSearch : Form
    {
        /// <summary>
        /// ComboBox 혹은 List<string> 에서 특정 문자열을 포함한 값을 검색하여 해당 Index를 Return
        /// </summary>
        /// <param name="_obj"> ComboBox 혹은 List<string> </param>
        /// <param name="HeaderName">헤더 이름</param>
        /// <returns>검색 후 선택 된 Index</returns>
        public frmSearch(object _obj, string HeaderName = "NAME")
        {
            InitializeComponent();

            m_obj = _obj;
            m_sHeader = HeaderName;            
            
            InitGrid();
        }

        #region [ Variables ]
        private object m_obj;
        private string m_sHeader;
        private int _nSelectNo = 0;
        private int nSelectNo
        {
            get => _nSelectNo;
            set {
                _nSelectNo = value;
                lbNo.Text = _nSelectNo.ToString("00");                
            }
        }
        private List<string> m_ltData = new List<string>();

        #endregion

        #region [ Event Function ]

        private void txtSearchValue_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnFindNext.PerformClick();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        
        #endregion


        #region [ Button Event ]
        private void btnSet_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void dgList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                nSelectNo = e.RowIndex;            
        }

        private void btnFindNext_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_ltData.Count < 1 || dgList == null) return;
                if (string.IsNullOrEmpty(txtSearchValue.Text)) return;

                Regex reg = new Regex(txtSearchValue.Text, RegexOptions.IgnoreCase);
                Match m;
                int nRow = 0;                

                if (dgList.CurrentCell == null)
                {
                    nRow = 0;                    
                }
                else
                {
                    nRow = dgList.CurrentCell.RowIndex + 1;                                        
                }

                for (int i = nRow; i < m_ltData.Count; i++)
                {
                    m = reg.Match(m_ltData[i]);
                    if (m.Success)
                    {
                        dgList.CurrentCell = dgList[1, i];
                        nSelectNo = i;
                        return;
                    }
                    nSelectNo = 0;
                    dgList.CurrentCell = dgList[1, 0];
                }
            }
            catch (Exception ex)
            {
                UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
            }
        }

        private void btnPreFind_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_ltData.Count < 1 || dgList == null) return;
                if (string.IsNullOrEmpty(txtSearchValue.Text)) return;

                Regex reg = new Regex(txtSearchValue.Text, RegexOptions.IgnoreCase);
                Match m;
                int nRow = 0;                

                if (dgList.CurrentCell == null)
                {
                    nRow = m_ltData.Count - 1;
                }
                else
                {
                    nRow = dgList.CurrentCell.RowIndex - 1;                    
                }

                for (int i = nRow; i >= 0; i--)
                {
                    m = reg.Match(m_ltData[i]);
                    if (m.Success)
                    {
                        dgList.CurrentCell = dgList[1, i];
                        nSelectNo = i;
                        return;
                    }                    
                }
            }
            catch (Exception ex)
            {
                UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
            }
        }

        
        private void txtSearchValue_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (m_ltData.Count < 1 || dgList == null) return;
                if (string.IsNullOrEmpty(txtSearchValue.Text)) return;

                Regex reg = new Regex(txtSearchValue.Text, RegexOptions.IgnoreCase);
                Match m;
                int nRow = 0;                

                if (dgList.CurrentCell == null)
                {
                    nRow = 0;                    
                }
                else
                {
                    nRow = dgList.CurrentCell.RowIndex + 1;                                        
                }

                for (int i = 0; i < m_ltData.Count; i++)
                {
                    m = reg.Match(m_ltData[i]);
                    if (m.Success)
                    {
                        dgList.CurrentCell = dgList[1, i];
                        nSelectNo = i;
                        return;
                    }                    
                }
            }
            catch (Exception ex)
            {
                UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
            }
        }
        #endregion

        #region [ Function ]

        #region - Private Function

        private void InitGrid()
        {
            try
            {
                List<string> lstName = GetList();
                dgList.Columns["dgName"].HeaderText = m_sHeader;

                for (int i = 0; i < lstName.Count; i++)
                {
                    dgList.Rows.Add(i, lstName[i]);
                    m_ltData.Add(lstName[i]);
                }
                
                dgList.Rows[nSelectNo].Cells[1].Selected = true;
                dgList.Invalidate();
            }
            catch (Exception ex)
            {
                UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
            }
        }

        private List<string> GetList()
        {
            List<string> lstStr = new List<string>();
            if (m_obj is ComboBox)
            {
                ComboBox cbTemp =  m_obj as ComboBox;;
                lstStr = cbTemp.Items.Cast<Object>().Select(item => item.ToString()).ToList();
                nSelectNo = cbTemp.SelectedIndex;
            }
            else if(m_obj is List<string>)
            {
                lstStr = m_obj as List<string>;
                nSelectNo = 0;
            }
            return lstStr; 
        }
        #endregion

        #region - Public Function
        public int GetVal()
        {
            return nSelectNo;
        }
        #endregion

        #region - Language Function 

        public void SetLanguageText()
        {
            
        }


        #endregion

        #endregion

        
    }
}
