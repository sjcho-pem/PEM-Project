using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Integration;
using System.Windows.Forms;
using System.Windows.Controls;
using PMN.Control.WPF.MachineAni;


namespace H_MarsSSD
{
    public partial class frmMainDisp : Form
    {
        public ucMainDisp wpfMainDisp = new ucMainDisp();
        public frmMainDisp()
        {
            InitializeComponent();
            elementHost1.Child = wpfMainDisp;

        }
      
        #region [ Variables ]

        #endregion

        #region [ Event Function ]

        #endregion

        #region [ Timmer Function ]

        #endregion

        #region [ Button Event ]

        #endregion

        #region [ Function ]

        #region - Private Function

        #endregion

        #region - Public Function

        #endregion

        #region - Language Function 
        public void SetLanguageText()
        {
            try
            {
                wpfMainDisp.SetLanguageText();

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