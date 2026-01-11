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
    public partial class frmIPSetting : Form
    {
        public enum Position
        {
            TOP,
            BOTTOM,
            REV,
			MES,
        }
        
		private	Position	m_CurPos		= Position.TOP;
		private	string		m_strShowItem	= string.Empty;
		private	Label		m_lbState		= new Label();


        public frmIPSetting()
        {
            InitializeComponent();
        }

        public void Show(Object sender)
        {
            int nHeight = 127;
            bool bIsSetupVisible = false;

            m_lbState = sender as Label;
            
			m_strShowItem	= m_lbState.Name;
            
            switch (m_lbState.Name)
            {
                case "lbTopState":
                    {
                        m_CurPos = Position.TOP;
                        gbIP.Text = "TOP IP Address";
                        string strIpPort =  Program.iniMachine.GetString("Socket", "TOP") == "" ? "192.168.0.12:8000" : Program.iniMachine.GetString("Socket", "TOP");
                        string[] str = strIpPort.Split(':');
                        if (str.Length != 2)
                        {
                            tbIP.Text = "";
                            tbPort.Text = "";
                        }
                        else
                        {
                            tbIP.Text = str[0];
                            tbPort.Text = str[1];
                        }
                    }
                    break;
                case "lbBtmState":
                    {
                        m_CurPos = Position.BOTTOM;
                        gbIP.Text = "BOTTOM IP Address";
                        string strIpPort =   Program.iniMachine.GetString("Socket", "BOTTOM") == "" ? "192.168.0.13:8000" : Program.iniMachine.GetString("Socket", "BOTTOM");
                        string[] str = strIpPort.Split(':');
                        if (str.Length != 2)
                        {
                            tbIP.Text = "";
                            tbPort.Text = "";
                        }
                        else
                        {
                            tbIP.Text = str[0];
                            tbPort.Text = str[1];
                        }
                    }
                    break;
                case "lbRevState":
                    {
                        m_CurPos = Position.REV;
                        gbIP.Text = "REV IP Address";
                        string strIpPort =  Program.iniMachine.GetString("Socket", "REV") == "" ? "192.168.0.13:8001" : Program.iniMachine.GetString("Socket", "REV");
                        string[] str = strIpPort.Split(':');
                        if (str.Length != 2)
                        {
                            tbIP.Text = "";
                            tbPort.Text = "";
                        }
                        else
                        {
                            tbIP.Text = str[0];
                            tbPort.Text = str[1];
                        }
                    }
                    break;
				case "lbMESState":
                    {
                        nHeight = 227;
                        bIsSetupVisible = true;

                        m_CurPos = Position.MES;
                        gbIP.Text = "SECS/GEM IP Address";
                        
                        if (Program.iniMachine.GetString("Socket", Position.MES.ToString()) == "")
                            Program.iniMachine.SetString("Socket", Position.MES.ToString(),"127.0.0.1:8000");
                        
                        string strIpPort = Program.iniMachine.GetString("Socket", "MES");

                        string[] str = strIpPort.Split(':');
                        if (str.Length != 2)
                        {
                            tbIP.Text = "";
                            tbPort.Text = "";
                        }
                        else
                        {
                            tbIP.Text = str[0];
                            tbPort.Text = str[1];
                        }
                        
                        tbEquipID.Text = Program.iniMachine.GetString("Machine", "EquipID");
                        tbEquipModel.Text = Program.iniMachine.GetString("Machine", "EquipModel");
                    }
                    break;
            }
            
            this.Height = nHeight;
            gbSetup.Visible = bIsSetupVisible;
            this.CenterToScreen();
            this.Location =new Point(Location.X + 500,Location.Y - 300);
            this.Show();
        }

		public void Show(string strName)
        {
			m_strShowItem	= strName;
            switch (strName)
            {
                case "lbMESState":
                    {
                        this.Height	= 227;
                        gbSetup.Visible = true;

                        m_CurPos = Position.MES;
                        gbIP.Text = "SECS/GEM IP Address";
                        
                        if (Program.iniMachine.GetString("Socket", frmIPSetting.Position.MES.ToString()) == "")
                            Program.iniMachine.SetString("Socket", frmIPSetting.Position.MES.ToString(),"127.0.0.1:8000");
                        
                        string strIpPort = Program.iniMachine.GetString("Socket", frmIPSetting.Position.MES.ToString());

                        string[] str = strIpPort.Split(':');
                        if (str.Length != 2)
                        {
                            tbIP.Text = "";
                            tbPort.Text = "";
                        }
                        else
                        {
                            tbIP.Text = str[0];
                            tbPort.Text = str[1];
                        }
                        
                        tbEquipID.Text = Program.iniMachine.GetString("Machine", "EquipID");
                        tbEquipModel.Text = Program.iniMachine.GetString("Machine", "EquipModel");
                    }
                    break;
            }
            
            this.CenterToScreen();
            this.Location = new Point(Location.X + 500,Location.Y - 300);
            this.Show();
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            try
            {
                string pattern = @"^(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])$";
                string strIPAddress = tbIP.Text;

                if (Regex.IsMatch(strIPAddress, pattern) == true)
                {
                    strIPAddress = strIPAddress + ":" + tbPort.Text;
                    Program.iniMachine.SetString("Socket", m_CurPos.ToString(), strIPAddress);
                    frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Confirmation, "Please Restart Program");
                    if (pMsg.ShowDialog() == DialogResult.Cancel) return;
                    this.Close();
                }
                else
                {
                    frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Confirmation, "Please Insert Valid IP ADDRESS");
                }
            }
           	catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
           
        }

        private void frmIPSetting_Deactivate(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnReconnect_Click(object sender, EventArgs e)
        {
			frmMessageBox pMsg;
			
			pMsg = new frmMessageBox(frmMessageBox.teButtons.OKCancel, Language.languageDefault.MB_Warnning, $"this connections are disconnected."+Environment.NewLine +"Would you like to retry this connection?");
			if (pMsg.ShowDialog() == DialogResult.Cancel) return;

			try
			{
				if (m_lbState.Name != null)
				{
					G.CommDispose(m_lbState.Name);
					G.CommInit(m_lbState.Name);
					GLog.Add(string.Format("{0} Reconnect", m_lbState.Name));
				}
			}
			catch(Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
        }

        private void brnSetupSet_Click(object sender, EventArgs e)
        {
            Program.iniMachine.SetString("Machine", "EquipID", tbEquipID.Text);
            Program.iniMachine.SetString("Machine", "EquipModel", tbEquipModel.Text);
        }
    }
}
