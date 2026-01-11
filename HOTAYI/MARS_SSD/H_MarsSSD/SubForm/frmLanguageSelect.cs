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
    public partial class frmLanguageSelect : Form
    {        
        public frmLanguageSelect()
        {
            InitializeComponent();            
        }
               
        #region [ Event Function ]
        private void frmLanguageSelect_Load(object sender, EventArgs e)
        {
            switch ((LAN_MODE) Program.m_Languageidx)
            {
                case LAN_MODE.ENG:
                    btnEng.BackColor = Color.LemonChiffon;
                    btnKor.BackColor = Color.White;
                //    btnCha.BackColor = Color.White;
                    break;
                case LAN_MODE.KOR:
                    btnEng.BackColor = Color.White;
                    btnKor.BackColor = Color.LemonChiffon;
                 //   btnCha.BackColor = Color.White;
                    break;
                case LAN_MODE.CHA:
                    btnEng.BackColor = Color.White;
                    btnKor.BackColor = Color.White;
                 //   btnCha.BackColor = Color.LemonChiffon;
                    break;
            }
        }

        private void frmLanguageSelect_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            SolidBrush pBr = new SolidBrush(Color.Black);
            Pen pPen = new Pen(pBr, 3);

            Size _size = this.Size;
            g.DrawRectangle(pPen, 3, 3, _size.Width - 6, _size.Height - 6);

            pBr.Dispose();
            pPen.Dispose();
        }
        #endregion [ Event Function ] : END

        #region [ Button Events ]
        private void btnEng_Click(object sender, EventArgs e)
        {            
            this.DialogResult = DialogResult.OK;
            //this.Close();
        }

        private void btnKor_Click(object sender, EventArgs e)
        {            
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnCha_Click(object sender, EventArgs e)
        {            
            this.DialogResult = DialogResult.Abort;
        }
        #endregion [ Button Events ] : END
        
    }
}
