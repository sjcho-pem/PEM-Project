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
    public partial class frmOtherIO : Form
    {
        public frmOtherIO()
        {
            InitializeComponent();
            InitialXML();
            InitialIOGrid();
            InitialInputCombobox();
            InitialOutputCombobox();
            InitializeTimer();
        }

        #region [ variables ]
        private XML XMLIOConfig = new XML();
        private string strIOConfigColName0 = "InputNum";
        private string strIOConfigColName1 = "OutputNum";

        private XML XMLIO = new XML();
        private string strIOColName0 = "InputName";
        private string strIOColName1 = "OutputName";


        public ushort[] ushInput = new ushort[Define.MAX_IOCH_NUM];
        public ushort[] ushOutput = new ushort[Define.MAX_IOCH_NUM];

        private uint uiNowInputCnt = new uint();
        private uint uiNowOutputCnt = new uint();

        private bool bTimerRunning = false;

        private Timer tmrIO = new Timer();
        #endregion

        #region [ Event Function ]

        private void frmOtherIO_FormClosing(object sender, FormClosingEventArgs e)
        {
            tmrIO.Stop();
        }
        #endregion

        #region [ Timmer Function ]

        private void tmrIO_Tick(object sender, EventArgs e)
        {
            if (chkIOModify.Checked) return;

            bTimerRunning = true;

            // IO 상태 체크 -LWJ
            //Global.COM.SetCommand(CMD.MS_WRITE_IO);

            //Global.SHM.ReadSHM_Input(this.uiNowInputCnt);
            SM.SHM.GetInputData(ref ushInput);
            SetInputValue(ushInput[cbSelInput.SelectedIndex]);

            //Global.SHM.ReadSHM_Output(this.uiNowOutputCnt);
            SM.SHM.GetOutputData(ref ushOutput);
            SetOutputValue(ushOutput[cbSelOutput.SelectedIndex]);

            bTimerRunning = false;

        }
        #endregion

        #region [ CheckBox Event ]

        private void chkIOModify_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIOModify.Checked)
            {
                SetGridColumReadOnly(false);
                chkIOModify.ForeColor = Color.Red;
                cbSelInput.Enabled = false;
                cbSelOutput.Enabled = false;
                chkOutputEnable.Enabled = false;
            }
            else
            {
                XMLIO.UpdateData();

                SetGridColumReadOnly(true);
                chkIOModify.ForeColor = Color.Black;
                cbSelInput.Enabled = true;
                cbSelOutput.Enabled = true;
                chkOutputEnable.Enabled = true;
            }
        }

        private void chkOutputEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOutputEnable.Checked)
            {
                chkOutputEnable.ForeColor = Color.Red;
                chkIOModify.Enabled = false;

            }
            else
            {
                chkOutputEnable.ForeColor = Color.Black;
                chkIOModify.Enabled = true;
            }
        }
        #endregion

        #region [ ComboBox Event ]

        private void cbSelInput_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateInputChannel(cbSelInput.SelectedIndex);
        }

        private void cbSelOutput_SelectedValueChanged(object sender, EventArgs e)
        {
            UpdateOutputChannel(cbSelOutput.SelectedIndex);
        }
        #endregion

        #region [ Grid Setting ]

        #region - Function

        private void InitialIOGrid()
        {
            ////////////////////////////////////////////////
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;

            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;

            // 
            // dgridInput
            // 
            this.dgridInput.AllowUserToAddRows = false;
            this.dgridInput.AllowUserToDeleteRows = false;
            this.dgridInput.AllowUserToResizeColumns = false;
            this.dgridInput.AllowUserToResizeRows = false;
            this.dgridInput.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgridInput.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgridInput.ColumnHeadersHeight = 40;
            this.dgridInput.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgridInput.EnableHeadersVisualStyles = false;
            this.dgridInput.RowHeadersVisible = false;
            this.dgridInput.RowTemplate.Height = 20;
            this.dgridInput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;

            // 
            // tbColumnInPutNum
            // 
            this.tbColumnInPutNum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.tbColumnInPutNum.DefaultCellStyle = dataGridViewCellStyle2;
            this.tbColumnInPutNum.HeaderText = "NO";
            this.tbColumnInPutNum.Name = "tbColumnInPutNum";
            this.tbColumnInPutNum.ReadOnly = true;
            this.tbColumnInPutNum.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.tbColumnInPutNum.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.tbColumnInPutNum.Width = 60;
            // 
            // tbColumnInPutName
            // 
            this.tbColumnInPutName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.tbColumnInPutName.HeaderText = "INPUT NAME";
            this.tbColumnInPutName.Name = "tbColumnInPutName";
            this.tbColumnInPutName.ReadOnly = true;
            this.tbColumnInPutName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.tbColumnInPutName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.tbColumnInPutName.Width = 250;
            // 
            // tbColumnInPutOn
            // 
            this.tbColumnInPutOn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.tbColumnInPutOn.DefaultCellStyle = dataGridViewCellStyle2;
            this.tbColumnInPutOn.HeaderText = "ON";
            this.tbColumnInPutOn.Name = "tbColumnInPutOn";
            this.tbColumnInPutOn.ReadOnly = true;
            this.tbColumnInPutOn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.tbColumnInPutOn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.tbColumnInPutOn.Width = 103;

            // 
            // dgridOutput
            // 
            this.dgridOutput.AllowUserToAddRows = false;
            this.dgridOutput.AllowUserToDeleteRows = false;
            this.dgridOutput.AllowUserToResizeColumns = false;
            this.dgridOutput.AllowUserToResizeRows = false;
            this.dgridOutput.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgridOutput.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgridOutput.ColumnHeadersHeight = 40;
            this.dgridOutput.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgridOutput.EnableHeadersVisualStyles = false;
            this.dgridOutput.RowHeadersVisible = false;
            this.dgridOutput.RowTemplate.Height = 20;
            this.dgridOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;

            // 
            // tbColumnOutPutNum
            // 
            this.tbColumnOutPutNum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.tbColumnOutPutNum.DefaultCellStyle = dataGridViewCellStyle2;
            this.tbColumnOutPutNum.HeaderText = "NO";
            this.tbColumnOutPutNum.Name = "tbColumnInPutNum";
            this.tbColumnOutPutNum.ReadOnly = true;
            this.tbColumnOutPutNum.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.tbColumnOutPutNum.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.tbColumnOutPutNum.Width = 60;
            // 
            // tbColumnOutPutName
            // 
            this.tbColumnOutPutName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.tbColumnOutPutName.HeaderText = "INPUT NAME";
            this.tbColumnOutPutName.Name = "tbColumnInPutName";
            this.tbColumnOutPutName.ReadOnly = true;
            this.tbColumnOutPutName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.tbColumnOutPutName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.tbColumnOutPutName.Width = 250;
            // 
            // tbColumnOutPutOn
            // 
            this.tbColumnOutPutOn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.tbColumnOutPutOn.DefaultCellStyle = dataGridViewCellStyle2;
            this.tbColumnOutPutOn.HeaderText = "ON";
            this.tbColumnOutPutOn.Name = "tbColumnInPutOn";
            this.tbColumnOutPutOn.ReadOnly = true;
            this.tbColumnOutPutOn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.tbColumnOutPutOn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.tbColumnOutPutOn.Width = 103;

            string strTemp;

            this.dgridInput.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.tbColumnInPutNum,
            this.tbColumnInPutName,
            this.tbColumnInPutOn});

            for (int i = 0; i < Define.MAX_IOBIT_NUM; i++)
            {
                strTemp = XMLIO.ReadDataTable(i, strIOColName0);

                this.dgridInput.Rows.Add(i, strTemp, "OFF");
            }

            this.dgridOutput.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.tbColumnOutPutNum,
            this.tbColumnOutPutName,
            this.tbColumnOutPutOn});

            for (int i = 0; i < Define.MAX_IOBIT_NUM; i++)
            {
                strTemp = XMLIO.ReadDataTable(i, strIOColName1);

                this.dgridOutput.Rows.Add(i, strTemp, "OFF");
            }

            SetGridColumReadOnly(true);

            // for test
            //UInt16 temp = 0x3333;
            //SetInputValue(temp);
            //SetOutputValue(temp);
        }

        private void SetInputValue(int num, bool bon)
        {
            if (bon)
            {
                this.dgridInput.Rows[num].Cells[2].Value = "ON";
            }
            else
            {
                this.dgridInput.Rows[num].Cells[2].Value = "OFF";
            }
        }

        private void SetInputValue(UInt16 data)
        {
            for (int i = 0; i < Define.MAX_IOBIT_NUM; i++)
            {
                if (((data >> i) & 0x0001) == 0x0001)
                {
                    this.dgridInput.Rows[i].Cells[2].Value = "ON";
                    this.dgridInput.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                    this.dgridInput.Rows[i].DefaultCellStyle.SelectionBackColor = Color.Yellow;
                }
                else
                {
                    this.dgridInput.Rows[i].Cells[2].Value = "OFF";
                    this.dgridInput.Rows[i].DefaultCellStyle.BackColor = Color.White;
                    this.dgridInput.Rows[i].DefaultCellStyle.SelectionBackColor = Color.White;
                }
            }
        }

        private void SetOutputValue(int num, bool bon)
        {
            if (bon)
            {
                this.dgridOutput.Rows[num].Cells[2].Value = "ON";
            }
            else
            {
                this.dgridOutput.Rows[num].Cells[2].Value = "OFF";
            }
        }

        private void SetOutputValue(UInt16 data)
        {
            for (int i = 0; i < Define.MAX_IOBIT_NUM; i++)
            {

                if (((data >> i) & 0x0001) == 0x0001)
                {
                    this.dgridOutput.Rows[i].Cells[2].Value = "ON";
                    this.dgridOutput.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                    this.dgridOutput.Rows[i].DefaultCellStyle.SelectionBackColor = Color.Yellow;
                }
                else
                {
                    this.dgridOutput.Rows[i].Cells[2].Value = "OFF";
                    this.dgridOutput.Rows[i].DefaultCellStyle.BackColor = Color.White;
                    this.dgridOutput.Rows[i].DefaultCellStyle.SelectionBackColor = Color.White;
                }
            }
        }

        private void SetGridColumReadOnly(bool bon)
        {
            if (bon)
            {
                this.dgridInput.Columns[1].ReadOnly = true;
                this.dgridOutput.Columns[1].ReadOnly = true;
            }
            else
            {
                this.dgridInput.Columns[1].ReadOnly = false;
                this.dgridOutput.Columns[1].ReadOnly = false;
            }
        }

        private void UpdateInputChannel(int ch)
        {
            int iStart = ch * Define.MAX_IOBIT_NUM;

            for (int i = 0; i < Define.MAX_IOBIT_NUM; i++)
            {
                this.dgridInput.Rows[i].Cells[1].Value = XMLIO.ReadDataTable(iStart + i, strIOColName0);
            }

            // add input value update
            // here
            SetInputValue(ushInput[ch]);
        }

        private void UpdateOutputChannel(int ch)
        {
            int iStart = ch * Define.MAX_IOBIT_NUM;

            for (int i = 0; i < Define.MAX_IOBIT_NUM; i++)
            {
                this.dgridOutput.Rows[i].Cells[1].Value = XMLIO.ReadDataTable(iStart + i, strIOColName1);
            }

            // add output value update
            // here
            SetOutputValue(ushOutput[ch]);
        }
        #endregion

        #region -Event
        private void dgridInput_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (cbSelInput.SelectedIndex < 0) return;
            if (e.ColumnIndex != 1) return;
            if (e.RowIndex == -1) return;
            if (this.dgridInput.ReadOnly == true) return;

            string strNewValue = "";
            if (this.dgridInput[e.ColumnIndex, e.RowIndex].FormattedValue.ToString() == null)
                strNewValue = "";
            else
                strNewValue = this.dgridInput[e.ColumnIndex, e.RowIndex].FormattedValue.ToString();

            int idx = e.RowIndex + (cbSelInput.SelectedIndex * 16);
            this.XMLIO.WriteDataTable(idx, strIOColName0, strNewValue);
        }

        private void dgridOutput_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (cbSelOutput.SelectedIndex < 0) return;
            if (e.ColumnIndex != 1) return;
            if (e.RowIndex == -1) return;
            if (this.dgridInput.ReadOnly == true) return;

            string strNewValue = "";
            if (this.dgridOutput[e.ColumnIndex, e.RowIndex].FormattedValue.ToString() == null)
                strNewValue = "";
            else
                strNewValue = this.dgridOutput[e.ColumnIndex, e.RowIndex].FormattedValue.ToString();

            int idx = e.RowIndex + (cbSelOutput.SelectedIndex * 16);
            this.XMLIO.WriteDataTable(idx, strIOColName1, strNewValue);
        }

        private void dgridOutput_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!chkOutputEnable.Checked) return;
            while (bTimerRunning) ;

            tmrIO.Stop();

            int iCol = e.ColumnIndex;
            int iRow = e.RowIndex;
            int ch = cbSelOutput.SelectedIndex;

            if (iCol != 2) return;

            if (((ushOutput[ch] >> iRow) & 0x0001) == 0x0001)
            {
                ushOutput[ch] &= Convert.ToUInt16((~(0x0001 << iRow)) & 0xFFFF);
            }
            else
            {
                ushOutput[ch] |= Convert.ToUInt16(0x0001 << iRow);
            }

            SM.SHM.SetOutputData((DO)((ch * 16) + iRow));

            tmrIO.Start();
        }

        private void dgridInput_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{			
            if (Program._bSimul_.Equals(false))		return;
			if (!chkOutputEnable.Checked)			return;
            if (e.RowIndex == -1)					return;
           
			while (bTimerRunning) ;

            tmrIO.Stop();

            int iCol	= e.ColumnIndex;
            int iRow	= e.RowIndex;
            int ch		= cbSelInput.SelectedIndex;

            if (iCol != 2)							return;

            if (((ushInput[ch] >> iRow) & 0x0001) == 0x0001)
            {
                ushInput[ch] &= Convert.ToUInt16((~(0x0001 << iRow)) & 0xFFFF);
            }
            else
            {
                ushInput[ch] |= Convert.ToUInt16(0x0001 << iRow);
            }

            this.dgridInput.Rows[iRow].Selected = false;

            SM.SHM.SetInputData((DI)((ch * 16) + iRow));

            tmrIO.Start();
		}
        #endregion
        #endregion

        #region [ Function ]

        #region - Private Function

        private void InitialXML()
        {
            this.XMLIOConfig.Initializ("IOConfig");
            this.XMLIO.Initializ("IO");
        }

        private void InitializeTimer()
        {
            tmrIO.Enabled = true;
            tmrIO.Interval = 100;
            tmrIO.Tick += tmrIO_Tick;
        }
        private void InitialIOData()
        {
            for (int i = 0; i < Define.MAX_IOCH_NUM; i++)
            {
                ushInput[i] = new ushort();
                ushOutput[i] = new ushort();
                ushInput[i] = 0x0000;
                ushOutput[i] = 0x0000;
            }
        }

        private void InitialInputCombobox()
        {
            cbSelInput.Items.Clear();

            int chnum = Convert.ToInt32(XMLIOConfig.ReadDataTable(0, strIOConfigColName0));

            this.uiNowInputCnt = (uint)chnum;

            if (chnum > Define.MAX_IOCH_NUM)
                chnum = Define.MAX_IOCH_NUM;
            if (chnum < 1)
                chnum = 1;

            for (int i = 0; i < chnum; i++)
            {
                if (i < 10)
                    cbSelInput.Items.Add("INPUT CH : 0" + Convert.ToInt32(i));
                else
                    cbSelInput.Items.Add("INPUT CH : " + Convert.ToInt32(i));
            }

            cbSelInput.SelectedIndex = 0;
        }

        private void InitialOutputCombobox()
        {
            cbSelOutput.Items.Clear();

            int chnum = Convert.ToInt32(XMLIOConfig.ReadDataTable(0, strIOConfigColName1));

            this.uiNowOutputCnt = (uint)chnum;

            if (chnum > Define.MAX_IOCH_NUM)
                chnum = Define.MAX_IOCH_NUM;
            if (chnum < 1)
                chnum = 1;

            for (int i = 0; i < chnum; i++)
            {
                if (i < 10)
                    cbSelOutput.Items.Add("OUTPUT CH : 0" + Convert.ToInt32(i));
                else
                    cbSelOutput.Items.Add("OUTPUT CH : " + Convert.ToInt32(i));
            }

            cbSelOutput.SelectedIndex = 0;
        }
        #endregion

        #region - Public Function

        public bool IsWorking()
        {
            if (chkOutputEnable.Checked || chkIOModify.Checked)
                return true;
            return false;
        }

        public void WorkEnd()
        {
            if (chkOutputEnable.Checked)
                chkOutputEnable.Checked = false;

            if (chkIOModify.Checked)
                chkIOModify.Checked = false;
        }

        public void DisableWork()
        {
            if (chkIOModify.Enabled)
                chkIOModify.Enabled = false;
            if (chkOutputEnable.Enabled)
                chkOutputEnable.Enabled = false;
        }

        public void EnableWork()
        {
            if (!chkIOModify.Enabled)
                chkIOModify.Enabled = true;
            if (!chkOutputEnable.Enabled)
                chkOutputEnable.Enabled = true;
        }

        // ch : 0~15 , bit : 0~15
        public bool IsInput(int ch, int bit)
        {
            if (ch > 15) ch = 15;
            if (ch < 0) ch = 0;
            if (bit > 15) bit = 15;
            if (bit < 0) bit = 0;

            if (((ushInput[ch] >> bit) & 0x0001) == 0x0001)
                return true;
            return false;
        }

        // ch : 0~15 , bit : 0~15
        public bool IsOutput(int ch, int bit)
        {
            if (ch > 15) ch = 15;
            if (ch < 0) ch = 0;
            if (bit > 15) bit = 15;
            if (bit < 0) bit = 0;


            if (((ushOutput[ch] >> bit) & 0x0001) == 0x0001)
                return true;
            return false;
        }
        #endregion

        #region - Language Function 

        public void SetLanguageText()
        {
            chkIOModify.Text = Language.languageDefault.NameModify;
            chkOutputEnable.Text = Language.languageDefault.OutPutEnable;
        }
        #endregion

        #endregion

    }
}
