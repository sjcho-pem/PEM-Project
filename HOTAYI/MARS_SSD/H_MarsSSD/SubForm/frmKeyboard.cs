using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace H_MarsSSD
{
	public partial class frmKeyboard : Form
	{
		[DllImport("imm32.dll")]
		private	static extern IntPtr	ImmGetContext(IntPtr hWnd);
		[DllImport("imm32.dll")]
		private	static extern bool		ImmGetConversionStatus(IntPtr himc, ref int lpdw, ref int lpdw2);
		[DllImport("imm32.dll")]
		private	static extern bool		ImmSetConversionStatus(IntPtr hIMC, Int32 fdwConversion, Int32 fdwSentence);
		[DllImport("User32.dll")]
		private	static extern void		keybd_event(uint vk, uint scan, uint flags, uint extraInfo);
		[DllImport("kernel32.dll")]
		private static extern int		GetLocaleInfo(int Locale, int LCType, StringBuilder lpLCDate, int cchData);

		private	const int	LOCAL_SYSTEM_DEFAULT	= 0x800;
		private	const int	LOCAL_USER_DEFAULT		= 0x400;
		private	const int	LOCALE_SNATIVELANGNAME	= 0x4;
		private	const int	LOCALE_SENGLANGUAGE		= 0x1001;

		private	enum	teKeyMode
		{
			Normal,
			HanLow,
			HanUp,
			EngLow,
			EngUp,
			EngCapsLow,
			EngCapsUp,
		}

		private string[] m_strHanLowerKey			= new string[] {	"`", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "-", "=", "←",
																		"", "ㅂ", "ㅈ", "ㄷ", "ㄱ", "ㅅ", "ㅛ", "ㅕ", "ㅑ", "ㅐ", "ㅔ", "[", "]", "\\",
																		"CapsLock", "ㅁ", "ㄴ", "ㅇ", "ㄹ", "ㅎ", "ㅗ", "ㅓ", "ㅏ", "ㅣ", ";", "'", "Enter",
																		"Shift", "ㅋ", "ㅌ", "ㅊ", "ㅍ", "ㅠ", "ㅜ", "ㅡ", ",", ".", "/", "Shift",
																		"", "", "Kor/En", ""};
		private	string[] m_strHanUpperKey			= new string[] {	"~", "!", "@", "#", "$", "%", "^", "&&", "*", "(", ")", "_", "+", "←",
																		"", "ㅃ", "ㅉ", "ㄸ", "ㄲ", "ㅆ", "ㅛ", "ㅕ", "ㅑ", "ㅒ", "ㅖ", "{", "}", "|",
																		"CapsLock", "ㅁ", "ㄴ", "ㅇ", "ㄹ", "ㅎ", "ㅗ", "ㅓ", "ㅏ", "ㅣ", ":", "\"", "Enter",
																		"Shift", "ㅋ", "ㅌ", "ㅊ", "ㅍ", "ㅠ", "ㅜ", "ㅡ", "<", ">", "?", "Shift",
																		"", "", "Kor/En", ""};
		private	string[] m_strEngLowerKey			= new string[] {	"`", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "-", "=", "←",
																		"", "q", "w", "e", "r", "t", "y", "u", "i", "o", "p", "[", "]", "\\",
																		"CapsLock", "a", "s", "d", "f", "g", "h", "j", "k", "l", ";", "'", "Enter",
																		"Shift", "z", "x", "c", "v", "b", "n", "m", ",", ".", "/", "Shift",
																		"", "", "Kor/En", ""};
		private	string[] m_strEngUpperKey			= new string[] {	"~", "!", "@", "#", "$", "%", "^", "&&", "*", "(", ")", "_", "+", "←",
																		"", "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P", "{", "}", "|",
																		"CapsLock", "A", "S", "D", "F", "G", "H", "J", "K", "L", ":", "\"", "Enter",
																		"Shift", "Z", "X", "C", "V", "B", "N", "M", "<", ">", "?", "Shift",
																		"", "", "Kor/En", ""};
		private	string[] m_strEngCapsLockLowerKey	= new string[] {	"`", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "-", "=", "←",
																		"", "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P", "[", "]", "\\",
																		"CapsLock", "A", "S", "D", "F", "G", "H", "J", "K", "L", ";", "'", "Enter",
																		"Shift", "Z", "X", "C", "V", "B", "N", "M", ",", ".", "/", "Shift",
																		"", "", "Kor/En", ""};
		private	string[] m_strEngCapsLockUpperKey	= new string[]	{	"~", "!", "@", "#", "$", "%", "^", "&&", "*", "(", ")", "_", "+", "←",
																		"", "q", "w", "e", "r", "t", "y", "u", "i", "o", "p", "{", "}", "|",
																		"CapsLock", "a", "s", "d", "f", "g", "h", "j", "k", "l", ":", "\"", "Enter",
																		"Shift", "z", "x", "c", "v", "b", "n", "m", "<", ">", "?", "Shift",
																		"", "", "Kor/En", ""};
		private	byte[]	m_KeyCode					= new byte[]	{	0xC0, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x30, 0xBD, 0xBB, 0x08,
																		0, 0x51, 0x57, 0x45, 0x52, 0x54, 0x59, 0x55, 0x49, 0x4F, 0x50, 0xDB, 0xDD, 0xDC,
																		0x14, 0x41, 0x53, 0x44, 0x46, 0x47, 0x48, 0x4A, 0x4B, 0x4C, 0xBA, 0xDE, 0,
																		0x10, 0x5A, 0x58, 0x43, 0x56, 0x42, 0x4E, 0x4D, 0xBC, 0xBE, 0xBF, 0x10,
																		0, 0x20, 0x15, 0};

		private	bool		m_bShift				= false;
		private	bool		m_bCapsLock				= false;
		private	bool		m_bHan					= false;
		private	teKeyMode	m_CurKeyMode			= teKeyMode.Normal;
		private	string		m_strValue				= "";

		private	const int	IME_CMODE_ALPHANUMERIC	= 0x0;
		private	const int	IME_CMODE_NATIVE		= 0x1;
		private	const int	IME_SMODE_NONE			= 0x0;

		private	System.Windows.Controls.TextBox	m_wpftb	= new System.Windows.Controls.TextBox();

		public	string	Value
		{
			get => m_strValue;
		}


		protected override void WndProc(ref Message m)
		{
			switch(m.Msg)
			{
				case 0x00A3: return; //더블 클릭 시 최대화 막기
				case 0x84:
					base.WndProc(ref m);
					if ((int)m.Result == 0x1)
						m.Result = (IntPtr)0x2;
					return;
			}

			base.WndProc(ref m);
		}

		public			frmKeyboard(string strDefaultValue)
		{
			InitializeComponent();

			m_wpftb.Text	= strDefaultValue;
			m_strValue		= strDefaultValue;
		}

		private void	frmKeyboard_Load(object sender, EventArgs e)
		{
			try
			{
				Init();
			}
			catch
			{

			}
		}

		private void	frmKeyboard_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				keybd_event(0x10, 0, 2, 0);
			}
			catch
			{

			}
		}

		private	void	lbKey_MouseDown(object sender, MouseEventArgs e)
		{
			try
			{
				Label lb	= (Label)sender;

				int			nIndex		= 0;
				if (lb.Tag == null  ||  int.TryParse(lb.Tag.ToString(), out nIndex) == false)			return;

				lb.BackColor	= Color.LightBlue;

				if (m_KeyCode[nIndex] == 0x00)
				{
					m_wpftb.Text	+= lb.Text;
				}
				else
				{
					keybd_event(m_KeyCode[nIndex], 0x00, 0x00, 0);
				}
			}
			catch
			{

			}
		}

		private	void	lbKey_MouseUp(object sender, MouseEventArgs e)
		{
			try
			{
				Label lb	= (Label)sender;

				int			nIndex		= 0;
				if (lb.Tag == null  ||  int.TryParse(lb.Tag.ToString(), out nIndex) == false)			return;

				lb.BackColor	= SystemColors.Window;

				if (m_KeyCode[nIndex] != 0x00)
				{
					keybd_event(m_KeyCode[nIndex], 0x00, 0x02, 0);
				}
			}
			catch
			{

			}
		}

		private	void	lbSpecialKey_Click(object sender, EventArgs e)
		{
			try
			{
				Label lb	= (Label)sender;

				switch (lb.Name)
				{
					case "lbCapsLock" :
						keybd_event(0x14, 0, 0, 0);
						keybd_event(0x14, 0, 2, 0);

						ChangeKeyStatus();
						break;

					case "lbShiftL" :
					case "lbShiftR" :
						m_bShift	= (Control.ModifierKeys & Keys.Shift) == Keys.Shift ? false : true;
					
						if (m_bShift == true)			keybd_event(0x10, 0, 0, 0);
						else							keybd_event(0x10, 0, 2, 0);
						
						m_bShift	= !m_bShift;
						ChangeKeyStatus();
						break;

					case "lbLang" :
					{
						keybd_event(0x15, 0, 0, 0);
						keybd_event(0x15, 0, 2, 0);
						
						ChangeKeyStatus();
						break;
					}
				}
			}
			catch
			{

			}
		}

		private void	lbEnter_Click(object sender, EventArgs e)
		{
			try
			{
				OKClose();
			}
			catch
			{

			}
		}

		private	void	btnCancel_Click(object sender, EventArgs e)
		{
			try
			{
				this.DialogResult	= DialogResult.Cancel;
				Close();
			}
			catch
			{

			}
		}

		private void	wpftb_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			try
			{
				switch (e.Key)
				{
					case System.Windows.Input.Key.LeftShift :
					case System.Windows.Input.Key.RightShift :
						m_bShift	= true;
						ChangeKeyStatus();
						break;
				}
			}
			catch
			{

			}
		}

		private void	wpftb_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
		{
			try
			{
				switch (e.Key)
				{
					case System.Windows.Input.Key.LeftShift :
					case System.Windows.Input.Key.RightShift :
						m_bShift	= false;
						ChangeKeyStatus();
						break;
					case System.Windows.Input.Key.CapsLock :
						ChangeKeyStatus();
						break;
					case System.Windows.Input.Key.HangulMode :
						ChangeKeyStatus();
						break;
					case System.Windows.Input.Key.Enter :
						OKClose();
						break;
				}
			}
			catch
			{

			}
		}



		private	void	Init()
		{
			try
			{
				#region Wpf TextBox init
				elementHostText.Child		= m_wpftb;
				m_wpftb.FontSize			= 30;
				m_wpftb.FontFamily			= new System.Windows.Media.FontFamily("Segoe UI");
				m_wpftb.FontWeight			= System.Windows.FontWeight.FromOpenTypeWeight(700);
				m_wpftb.TextAlignment		= System.Windows.TextAlignment.Center;
				m_wpftb.VerticalAlignment	= System.Windows.VerticalAlignment.Center;
				m_wpftb.Padding				= new System.Windows.Thickness(5);
				#endregion

				this.ActiveControl = elementHostText;
				elementHostText.Focus();
				m_wpftb.Focus();
				m_wpftb.SelectAll();

				ChangeKeyStatus();

				m_wpftb.KeyDown += wpftb_KeyDown;
				m_wpftb.KeyUp += wpftb_KeyUp;
			}
			catch
			{

			}
		}

		private	void	OKClose()
		{
			try
			{
				this.DialogResult	= DialogResult.OK;
				m_strValue			= m_wpftb.Text.Trim();
				Close();
			}
			catch
			{

			}
		}
		
		

		private	void	ChangeKeyStatus()
		{
			try
			{
				IntPtr	hIMC;
				int		dwConversion	= 0;
				int		dwSentence		= 0;
				bool	bCheck			= false;

				StringBuilder sbuff = new StringBuilder(10);
				GetLocaleInfo(LOCAL_USER_DEFAULT, LOCALE_SENGLANGUAGE, sbuff, 10);

				if (sbuff.ToString() == "Korean")
				{
					hIMC		= ImmGetContext(elementHostText.Handle);
					bCheck		= ImmGetConversionStatus(hIMC, ref dwConversion, ref dwSentence);
					m_bHan		= dwConversion == 0 ? false : true;
				}
				m_bCapsLock	= Control.IsKeyLocked(Keys.CapsLock);

				#region 버튼 색상
				if (m_bCapsLock == true)			lbCapsLock.BackColor	= Color.LightBlue;
				else								lbCapsLock.BackColor	= SystemColors.Window;

				if (m_bShift == true)				lbShiftL.BackColor		= lbShiftR.BackColor = Color.LightBlue;
				else								lbShiftL.BackColor		= lbShiftR.BackColor = SystemColors.Window;

				if (m_bHan == true)					lbLang.BackColor		= Color.LightBlue;
				else								lbLang.BackColor		= SystemColors.Window;
				#endregion

				if (m_bHan == false  &&  m_bCapsLock == false  &&  m_bShift == false)
				{
					SetKeyChar(teKeyMode.EngLow);
				}
				else if	(m_bHan == false  &&  m_bCapsLock == false  &&  m_bShift == true)
				{
					SetKeyChar(teKeyMode.EngUp);
				}
				else if (m_bHan == false  &&  m_bCapsLock == true  &&  m_bShift == false)
				{
					SetKeyChar(teKeyMode.EngCapsLow);
				}
				else if (m_bHan == false  &&  m_bCapsLock == true  &&  m_bShift == true)
				{
					SetKeyChar(teKeyMode.EngCapsUp);
				}
				else if	(m_bHan == true  &&  m_bShift == false)
				{
					SetKeyChar(teKeyMode.HanLow);
				}
				else if	(m_bHan == true  &&  m_bShift == true)
				{
					SetKeyChar(teKeyMode.HanUp);
				}
			}
			catch
			{

			}
		}

		private	void	SetKeyChar(teKeyMode KeyMode)
		{
			try
			{
				if (m_CurKeyMode == KeyMode)			return;

				int			nIndex		= 0;
				string[]	strArray	= m_strEngLowerKey;

				switch (KeyMode)
				{
					case teKeyMode.Normal		:
					case teKeyMode.EngLow		:	strArray	= m_strEngLowerKey;					break;
					case teKeyMode.HanLow		:	strArray	= m_strHanLowerKey;					break;
					case teKeyMode.HanUp		:	strArray	= m_strHanUpperKey;					break;
					case teKeyMode.EngUp		:	strArray	= m_strEngUpperKey;					break;
					case teKeyMode.EngCapsLow	:	strArray	= m_strEngCapsLockLowerKey;			break;
					case teKeyMode.EngCapsUp	:	strArray	= m_strEngCapsLockUpperKey;			break;
				}

				foreach (Control ctrl in fllpKey.Controls)
				{
					if (ctrl.Tag == null  ||  int.TryParse(ctrl.Tag.ToString(), out nIndex) == false)			continue;

					ctrl.Text	= strArray[nIndex];
				}

				m_CurKeyMode	= KeyMode;
			}
			catch
			{

			}
		}
	}
}