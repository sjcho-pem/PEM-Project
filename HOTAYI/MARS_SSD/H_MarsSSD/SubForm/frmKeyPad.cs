using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace H_MarsSSD
{
    public enum KeyPadType
    {
        signed = 0,
        unsigned,
    }

    public partial class frmKeyPad : Form
    {
        [DllImport("User32.dll")]
        public static extern void keybd_event(uint vk, uint scan, uint flags, uint extraInfo);

        public frmKeyPad(object Label, string strTitle, KeyPadType KeyType = KeyPadType.signed)
        {
            InitializeComponent();
            Init();

            objLabel = Label;
            tbTitle.Text = strTitle;
            switch (KeyType)
            {
                case KeyPadType.signed:
                    btnMinus.Enabled = true;
                    m_KeyType = KeyType;
                    break;
                case KeyPadType.unsigned:
                    btnMinus.Enabled = false;
                    m_KeyType = KeyType;
                    break;
            }
        }

        #region [ Variables ]
        private object objLabel;
        private KeyPadType m_KeyType = KeyPadType.signed;
        private int m_nMaxLength = 8;
        string BeforeValue = "";

        #endregion

        #region [ Event Function ]


        private void tbTitle_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbTitle.Text))
            {
                return;
            }
            double dTemp;
            if (double.TryParse(tbTitle.Text, out dTemp) ==false)
            {

                //Signed 값이고, 처음이 -일때만 예외
                if (btnMinus.Enabled == true && tbTitle.Text.LastIndexOf('-') == 0 )
                {
                    if (tbTitle.Text == "-")
                        return;

                    if (double.TryParse(tbTitle.Text.Substring(1), out dTemp) == true)
                        return;
                }
                //마지막이 .이여도 예외
                if (tbTitle.Text[tbTitle.TextLength -1]== '.')
                {
                    if (double.TryParse(tbTitle.Text.Substring(0 , tbTitle.Text.Length-1), out dTemp) == true)
                        return;
                }

                frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, "Confirm", "You have to Insert only Numbers.");
                pMsg.ShowDialog();
                tbTitle.Text = BeforeValue;
                tbTitle.SelectionStart = tbTitle.Text.Length;
            }
            BeforeValue = tbTitle.Text;
        }
        #endregion

        #region [ Timmer Function ]

        #endregion

        #region [ Button Event ]

        private void btn_Click(object sender, EventArgs e)
        {
            if (tbTitle.Text.Length > m_nMaxLength)
                return;

            Button btn = sender as Button;
            tbTitle.Focus();
            switch (btn.Text)
            {
                case "1":
                    keybd_event((byte)Keys.D1, 0x00, 0x00, 0);
                    keybd_event((byte)Keys.D1, 0x00, 0x02, 0);
                    break;
                case "2":
                    keybd_event((byte)Keys.D2, 0x00, 0x00, 0);
                    keybd_event((byte)Keys.D2, 0x00, 0x02, 0);
                    break;
                case "3":
                    keybd_event((byte)Keys.D3, 0x00, 0x00, 0);
                    keybd_event((byte)Keys.D3, 0x00, 0x02, 0);
                    break;
                case "4":
                    keybd_event((byte)Keys.D4, 0x00, 0x00, 0);
                    keybd_event((byte)Keys.D4, 0x00, 0x02, 0);
                    break;
                case "5":
                    keybd_event((byte)Keys.D5, 0x00, 0x00, 0);
                    keybd_event((byte)Keys.D5, 0x00, 0x02, 0);
                    break;
                case "6":
                    keybd_event((byte)Keys.D6, 0x00, 0x00, 0);
                    keybd_event((byte)Keys.D6, 0x00, 0x02, 0);
                    break;
                case "7":
                    keybd_event((byte)Keys.D7, 0x00, 0x00, 0);
                    keybd_event((byte)Keys.D7, 0x00, 0x02, 0);
                    break;
                case "8":
                    keybd_event((byte)Keys.D8, 0x00, 0x00, 0);
                    keybd_event((byte)Keys.D8, 0x00, 0x02, 0);
                    break;
                case "9":
                    keybd_event((byte)Keys.D9, 0x00, 0x00, 0);
                    keybd_event((byte)Keys.D9, 0x00, 0x02, 0);
                    break;
                case "0":
                    keybd_event((byte)Keys.D0, 0x00, 0x00, 0);
                    keybd_event((byte)Keys.D0, 0x00, 0x02, 0);
                    break;
                case "00":
                    if (string.IsNullOrEmpty(tbTitle.Text) || tbTitle.Text == "0")
                    {
                        keybd_event((byte)Keys.D0, 0x00, 0x00, 0);
                        keybd_event((byte)Keys.D0, 0x00, 0x02, 0);
                    }
                    else
                    {
                        keybd_event((byte)Keys.D0, 0x00, 0x00, 0);
                        keybd_event((byte)Keys.D0, 0x00, 0x02, 0);
                        keybd_event((byte)Keys.D0, 0x00, 0x00, 0);
                        keybd_event((byte)Keys.D0, 0x00, 0x02, 0);
                    }
                    break;
                case ".":
                    if (string.IsNullOrEmpty(tbTitle.Text))
                        return;
                    keybd_event((byte)Keys.OemPeriod, 0x00, 0x00, 0);
                    keybd_event((byte)Keys.OemPeriod, 0x00, 0x02, 0);
                    break;
                case "-":
                    if (string.IsNullOrEmpty(tbTitle.Text) || tbTitle.Text == "0")
                    {
                        tbTitle.Text = "-";
                        tbTitle.SelectionStart = tbTitle.Text.Length;

                    }
                    break;
                default:
                    break;
            }

        }

        private void btnBackClear_Click(object sender, EventArgs e)
        {
            if (tbTitle.Text == "")
                return;

            tbTitle.Text = tbTitle.Text.Remove(tbTitle.Text.Length - 1);

            if (tbTitle.Text == "")
                tbTitle.Text = "0";
        }

        private void btnAllClear_Click(object sender, EventArgs e)
        {
            tbTitle.Text = "0";
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            Label label = (Label)objLabel;

            if (tbTitle.Text == "")
                tbTitle.Text = "0";

            double dValue = 0.0;

            if (double.TryParse(tbTitle.Text, out dValue) == false)
            {
                this.DialogResult = DialogResult.Cancel;
                return;
            }

            label.Text = dValue.ToString();

            this.DialogResult = DialogResult.OK;
        }


        private void btnVMinus_Click(object sender, EventArgs e)
        {
            double nTitle = 0.0;
            double nValue = 0.0;

            if (tbTitle.Text == "")
                nTitle = 0.0;
            else if (double.TryParse(tbTitle.Text, out nTitle) == false)
                return;
            else
                nTitle = Convert.ToDouble(tbTitle.Text);

            nValue = Convert.ToDouble(cbValue.SelectedItem);

            switch (m_KeyType)
            {
                case KeyPadType.signed:
                    nTitle -= nValue;
                    break;
                case KeyPadType.unsigned:
                    nTitle -= nValue;

                    if (nTitle < 0)
                        nTitle = 0;
                    break;
            }

            tbTitle.Text = nTitle.ToString();
        }

        private void btnVPlus_Click(object sender, EventArgs e)
        {
            double nTitle = 0.0;
            double nValue = 0.0;

            if (tbTitle.Text == "")
                nTitle = 0.0;
            else if (double.TryParse(tbTitle.Text, out nTitle) == false)
                return;
            else
                nTitle = Convert.ToDouble(tbTitle.Text);

            nValue = Convert.ToDouble(cbValue.SelectedItem);

            nTitle += nValue;

            tbTitle.Text = nTitle.ToString();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
        #endregion

        #region [ Function ]

        #region - Private Function

        private void Init()
        {
            cbValue.SelectedIndex = 3;

            this.ActiveControl = tbTitle;
            tbTitle.Focus();
            tbTitle.SelectAll();
        }
        #endregion

        #region - Public Function

        #endregion

        #region - Language Function 

        public void SetLanguageText()
        {
            
        }

        #endregion

        #endregion

    }
}
