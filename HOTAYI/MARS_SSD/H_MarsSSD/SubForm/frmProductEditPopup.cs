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
	public partial class frmProductEditPopup : Form
	{
		public frmProductEditPopup()
		{
			InitializeComponent();
		}

		public enum teProductPos
		{
			Part,
			Tray,
			Reject,
		}

		public	stProdInfo		m_CopyProdInfo;
		public	bool			m_IsCopyDone		= false;

		private teProductPos	m_productPos;
		private int				m_ProductIndex;
		private FlowTray		m_flowTray;
		private FlowPart		m_flowPart;
		private FlowTray		m_flowReject;



		public	DialogResult	ShowDialog(teProductPos _productPos, stProdInfo _CopyProdInfo, bool _IsCopyDone, int _ProductIndex, FlowTray _flowTray = FlowTray.Max, FlowPart _flowPart = FlowPart.Max)
		{
			try
			{
				m_productPos	= _productPos;
				m_CopyProdInfo	= _CopyProdInfo;
				m_IsCopyDone	= _IsCopyDone;
				m_ProductIndex	= _ProductIndex;
				m_flowTray		= _flowTray;
				m_flowPart		= _flowPart;

				this.ShowDialog();

				return this.DialogResult;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return DialogResult.Cancel;
			}
		}

		public	DialogResult	ShowDialog(stProdInfo _CopyProdInfo, bool _IsCopyDone, int _ProductIndex, FlowTray _flowReject)
		{
			try
			{
				m_productPos	= teProductPos.Reject;
				m_CopyProdInfo	= _CopyProdInfo;
				m_IsCopyDone	= _IsCopyDone;
				m_ProductIndex	= _ProductIndex;
				m_flowTray		= _flowReject;
				m_flowTray		= FlowTray.Max;
				m_flowPart		= FlowPart.Max;

				this.ShowDialog();

				return this.DialogResult;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return DialogResult.Cancel;
			}
		}



		private			stProdInfo ConverSetValueToProdInfo()
		{
			stProdInfo info = new stProdInfo();
			
			try
			{
				string[] str_val	= new string[dgvProdInfo.RowCount];
				for (int i = 0; i < dgvProdInfo.RowCount; i++)
				{
					if (dgvProdInfo.Rows[i].Cells[3].Value == null)			str_val[i]	= String.Empty;
					else													str_val[i]	= dgvProdInfo.Rows[i].Cells[3].Value.ToString();
				}

				info.ProdID			= str_val[0]; 
				info.TrayID			= str_val[1];
				info.KeyValue		= str_val[2];
				info.Exist			= bool.Parse(str_val[3]);
				info.ResultTop		= (int)Enum.Parse(typeof(ProdStatus), str_val[4]);
				info.ResultBtm		= (int)Enum.Parse(typeof(ProdStatus), str_val[5]);
				info.VerifyTopDone	= bool.Parse(str_val[6]);
				info.VerifyBtmDone	= bool.Parse(str_val[7]);
				info.PickStatus		= (int)Enum.Parse(typeof(ProdPickStatus), str_val[8]);
				info.TrayCol		= int.Parse(str_val[9]);
				info.TrayRow		= int.Parse(str_val[10]);
				info.InspTimeTop	= str_val[11];
				info.InspTimeBtm	= str_val[12];
				info.SerialNumber	= str_val[13];

				return info;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return info;
			}
		}

		private	void	frmProductEditPopup_Load(object sender, EventArgs e)
		{
			try
			{
				SetLanguageText();

				stProdInfo temp = new stProdInfo();
				
				switch (m_productPos)
				{
					case teProductPos.Part:
						{
							if (m_flowPart == FlowPart.Max)				return;
							if (SM.FMP.IsFlow(m_flowPart) == false)			break;

							temp = SM.FMP[m_flowPart].GetInfo().ProdInfo[m_ProductIndex];
						}
						break;
					case teProductPos.Tray:
						{
							if (m_flowTray == FlowTray.Max)				return;
							if (SM.FMT.IsFlow(m_flowTray) == false)			break;

							temp = SM.FMT[m_flowTray].GetInfo().ProdInfo[m_ProductIndex];
						}
						break;
					case teProductPos.Reject:
						{
							if (m_flowReject == FlowTray.Max)			return;
							if (SM.FMT.IsFlow(m_flowReject) == false)		break;

							temp = SM.FMT[m_flowReject].GetInfo().ProdInfo[m_ProductIndex];
						}
						break;
					default:
						break;
				}

				dgvProdInfo.Rows.Clear();

				dgvProdInfo.Rows.Add(0, "ProdID", temp.ProdID, temp.ProdID);
				dgvProdInfo.Rows.Add(1, "TrayID", temp.TrayID, temp.TrayID);
				dgvProdInfo.Rows.Add(2, "KeyValue", temp.KeyValue, temp.KeyValue);
				dgvProdInfo.Rows.Add(3, "Exist", temp.Exist.ToString(), temp.Exist);
				dgvProdInfo.Rows.Add(4, "ResultTop", ((ProdStatus)temp.ResultTop).ToString(), ((ProdStatus)temp.ResultTop).ToString());
				dgvProdInfo.Rows.Add(5, "ResultBtm", ((ProdStatus)temp.ResultBtm).ToString(), ((ProdStatus)temp.ResultBtm).ToString());
				dgvProdInfo.Rows.Add(6, "VerifyTopDone", temp.VerifyTopDone, temp.VerifyTopDone);
				dgvProdInfo.Rows.Add(7, "VerifyBtmDone", temp.VerifyBtmDone, temp.VerifyBtmDone);
				dgvProdInfo.Rows.Add(8, "PickStatus", temp.PickStatus, temp.PickStatus);
				dgvProdInfo.Rows.Add(9, "TrayRow", temp.TrayRow, temp.TrayRow);
				dgvProdInfo.Rows.Add(10, "TrayCol", temp.TrayCol, temp.TrayCol);
				dgvProdInfo.Rows.Add(11, "InspTimeTop", temp.InspTimeTop, temp.InspTimeTop);
				dgvProdInfo.Rows.Add(12, "InspTimeBtm", temp.InspTimeBtm, temp.InspTimeBtm);
				dgvProdInfo.Rows.Add(13, "BtmBarcode", temp.SerialNumber, temp.SerialNumber);
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	btnSave_Click(object sender, EventArgs e)
		{
			try
			{
				Button btn = (Button)sender;
				frmMessageBox pMsg;

				switch (m_productPos)
				{
					case teProductPos.Part:
						{
							if (m_flowPart == FlowPart.Max)																	return;
							if (m_ProductIndex < 0  ||  m_ProductIndex >= SM.FMP[m_flowPart].GetInfo().ProdInfo.Length)			return;

							if (SM.FMP.IsFlow(m_flowPart) == false)			SM.FMP.Next(FlowPart.Empty, m_flowPart);

							if(btn.Name == "btnSaveAll")
							{
								pMsg = new frmMessageBox(frmMessageBox.teButtons.OKCancel, Language.languageDefault.MB_Confirmation, string.Format("{0}\nTarget Part: {1}", Language.languageDefault.MB_SaveAll_Part, m_flowPart.ToString()));
								if (pMsg.ShowDialog() == DialogResult.Cancel)			return;
							}
							else
							{
								pMsg = new frmMessageBox(frmMessageBox.teButtons.OKCancel, Language.languageDefault.MB_Confirmation, string.Format("{0}\nProd: {1}, Part: {2}", Language.languageDefault.MB_Save, m_ProductIndex, m_flowPart.ToString()));
								if (pMsg.ShowDialog() == DialogResult.Cancel)			return;
							}

							stProdInfo info = ConverSetValueToProdInfo();

							if(btn.Name == "btnSaveAll")
							{
								for(int i = 0; i < SM.FMP[m_flowPart].GetMaxProdCount(); i++)
									SM.FMP[m_flowPart].SetProd(i, info); // 모든 제품 저장

								if (SM.FMP[m_flowPart].GetExistCount() <= 0) //제품이 없으면 클리어 함
								{
									SM.FMP.Clear(m_flowPart);
								}
							}
							else
							{
								SM.FMP[m_flowPart].SetProd(m_ProductIndex, info);
								
								if (SM.FMP[m_flowPart].GetExistCount() <= 0) //제품이 없으면 클리어 함
								{
									SM.FMP.Clear(m_flowPart);
								}
							}
						}
						break;
					case teProductPos.Tray:
						{
							if (m_flowTray == FlowTray.Max) return;
							if (SM.FMT.IsFlow(m_flowTray) == false) SM.FMT.Next(FlowTray.Empty, m_flowTray);
							if (m_ProductIndex < 0 || m_ProductIndex >= SM.FMT[m_flowTray].GetInfo().ProdInfo.Length) return;

							if(btn.Name == "btnSaveAll")
							{
								pMsg = new frmMessageBox(frmMessageBox.teButtons.OKCancel, Language.languageDefault.MB_Confirmation, string.Format("{0}\nTarget Tray: {1}", Language.languageDefault.MB_SaveAll_Tray, m_flowTray.ToString()));
								if (pMsg.ShowDialog() == DialogResult.Cancel) return;
							}
							else
							{
								pMsg = new frmMessageBox(frmMessageBox.teButtons.OKCancel, Language.languageDefault.MB_Confirmation, string.Format("{0}\nProd: {1}, Tray: {2}", Language.languageDefault.MB_Save, m_ProductIndex, m_flowTray.ToString()));
								if (pMsg.ShowDialog() == DialogResult.Cancel) return;
							}

							stProdInfo info = ConverSetValueToProdInfo();
							
							if(btn.Name == "btnSaveAll")
							{
								int nRowCnt = SM.FMT[m_flowTray].GetInfo().RowCnt;
								int nColCnt = SM.FMT[m_flowTray].GetInfo().ColCnt;
								for(int row = 0; row < nRowCnt ; row++)
								{ 
									for(int col = 0; col < nColCnt; col++)
									{
										info.TrayRow = row; 
										info.TrayCol = col;
										SM.FMT[m_flowTray].SetProd(row * nColCnt + col, info); // 모든 제품 저장
									}
								}

								if (SM.FMT[m_flowTray].GetExistCount() <= 0) //제품이 없으면 클리어 함
								{
									SM.FMT.Clear(m_flowTray);
								}
							}
							else
							{
								SM.FMT[m_flowTray].SetProd(m_ProductIndex, info);
							}
						}
						break;
					case teProductPos.Reject:
						{
							if (m_flowReject == FlowTray.Max)			return;
							if (SM.FMT.IsFlow(m_flowReject) == false)			SM.FMT.Next(FlowTray.Empty, m_flowReject);
							if (m_ProductIndex < 0  ||  m_ProductIndex >= SM.FMT[m_flowReject].GetInfo().ProdInfo.Length)			return;

							if(btn.Name == "btnSaveAll")
							{
								pMsg = new frmMessageBox(frmMessageBox.teButtons.OKCancel, Language.languageDefault.MB_Confirmation, string.Format("{0}\nTarget Reject Buffer: {1}", Language.languageDefault.MB_SaveAll_Tray, m_flowReject.ToString()));
								if (pMsg.ShowDialog() == DialogResult.Cancel) return;
							}
							else
							{
								pMsg = new frmMessageBox(frmMessageBox.teButtons.OKCancel, Language.languageDefault.MB_Confirmation, string.Format("{0}\nProd: {1}, Reject Buffer: {2}", Language.languageDefault.MB_Save, m_ProductIndex, m_flowReject.ToString()));
								if (pMsg.ShowDialog() == DialogResult.Cancel) return;
							}

							stProdInfo info = ConverSetValueToProdInfo();
							
							if(btn.Name == "btnSaveAll")
							{
								int	nRowCnt	= SM.FMT[m_flowReject].GetInfo().RowCnt;
								int	nColCnt	= SM.FMT[m_flowReject].GetInfo().ColCnt;
								for (int row = 0; row < nRowCnt; row++)
								{
									for (int col = 0; col < nColCnt; col++)
									{
										info.TrayRow	= row;
										info.TrayCol	= col;
										SM.FMT[m_flowReject].SetProd(row*nColCnt+col, info); // 모든 제품 저장
									}
								}

								if (SM.FMT[m_flowReject].GetExistCount() <= 0) //제품이 없으면 클리어 함
								{
									SM.FMT.Clear(m_flowReject);
								}
							}
							else
							{
								SM.FMT[m_flowReject].SetProd(m_ProductIndex, info);
							}
						}
						break;
					default:
						break;
				}
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	btnClear_Click(object sender, EventArgs e)
		{
			try
			{
				Button btn = (Button)sender;
				frmMessageBox pMsg;

				switch (m_productPos)
				{
					case teProductPos.Part:
						{
							if (m_flowPart == FlowPart.Max) return;
							if (SM.FMP.IsFlow(m_flowPart) == false) return;
							if (m_ProductIndex < 0 || m_ProductIndex >= SM.FMP[m_flowPart].GetInfo().ProdInfo.Length) return;

							if(btn.Name == "btnClearAll")
							{
								pMsg = new frmMessageBox(frmMessageBox.teButtons.OKCancel, Language.languageDefault.MB_Confirmation, string.Format("{0}\nTarget Part: {1}", Language.languageDefault.MB_ClearAll_Part, m_flowPart.ToString()));
								if (pMsg.ShowDialog() == DialogResult.Cancel) return;
							}
							else
							{
								pMsg = new frmMessageBox(frmMessageBox.teButtons.OKCancel, Language.languageDefault.MB_Confirmation, string.Format("{0}\nProd: {1}, Part: {2}", Language.languageDefault.MB_Delete, m_ProductIndex, m_flowPart.ToString()));
								if (pMsg.ShowDialog() == DialogResult.Cancel) return;
							}

							stProdInfo info = new stProdInfo();
							
							if(btn.Name == "btnClearAll")
							{
								SM.FMP.Clear(m_flowPart); // 모든 제품 저장
								dgvProdInfo.Rows.Clear();
							}
							else
							{
								SM.FMP[m_flowPart].SetProd(m_ProductIndex, info);

								if (SM.FMP[m_flowPart].GetExistCount() <= 0) //하나 수정 후제품이 없으면 클리어 함
								{
									SM.FMP.Clear(m_flowPart);
								}
							}
						}
						break;
					case teProductPos.Tray:
						{
							if (m_flowTray == FlowTray.Max) return;
							if (SM.FMT.IsFlow(m_flowTray) == false) return;
							if (m_ProductIndex < 0 || m_ProductIndex >= SM.FMT[m_flowTray].GetInfo().ProdInfo.Length) return;

							if(btn.Name == "btnClearAll")
							{
								pMsg = new frmMessageBox(frmMessageBox.teButtons.OKCancel, Language.languageDefault.MB_Confirmation, string.Format("{0}\nTarget Tray: {1}", Language.languageDefault.MB_ClearAll_Tray, m_flowTray.ToString()));
								if (pMsg.ShowDialog() == DialogResult.Cancel) return;
							}
							else
							{
								pMsg = new frmMessageBox(frmMessageBox.teButtons.OKCancel, Language.languageDefault.MB_Confirmation, string.Format("{0}\nProd: {1}, Tray: {2}", Language.languageDefault.MB_Delete, m_ProductIndex, m_flowTray.ToString()));
								if (pMsg.ShowDialog() == DialogResult.Cancel) return;
							}

							stProdInfo info = new stProdInfo();
							
							if(btn.Name == "btnClearAll")
							{
								SM.FMT.Clear(m_flowTray); // 모든 제품 저장
							}
							else
							{
								SM.FMT[m_flowTray].SetProd(m_ProductIndex, info);

								if (SM.FMT[m_flowTray].GetExistCount() <= 0) //제품이 없으면 클리어 함
								{ 
									SM.FMT.Clear(m_flowTray);
								}
							}
						}
						break;
					default:
						break;
				}
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	btnCopy_Click(object sender, EventArgs e)
		{
			try
			{
				frmMessageBox pMsg;

				if(m_productPos == teProductPos.Part)
				{
					pMsg = new frmMessageBox(frmMessageBox.teButtons.OKCancel, Language.languageDefault.MB_Confirmation, string.Format("{0}\nProd: {1}, Part: {2}", Language.languageDefault.MB_Copy, m_ProductIndex, m_flowPart.ToString()));
				}
				else
				{
					pMsg = new frmMessageBox(frmMessageBox.teButtons.OKCancel, Language.languageDefault.MB_Confirmation, string.Format("{0}\nProd: {1}, Tray: {2}", Language.languageDefault.MB_Copy, m_ProductIndex, m_flowTray.ToString()));
				}
				if (pMsg.ShowDialog() == DialogResult.Cancel) return;

				stProdInfo info		= new stProdInfo();
				info.ProdID			= dgvProdInfo.Rows[0].Cells[2].Value == null ? "" : dgvProdInfo.Rows[0].Cells[2].Value.ToString();
				info.TrayID			= dgvProdInfo.Rows[1].Cells[2].Value == null ? "" : dgvProdInfo.Rows[1].Cells[2].Value.ToString();
				info.KeyValue		= dgvProdInfo.Rows[2].Cells[2].Value == null ? "" : dgvProdInfo.Rows[2].Cells[2].Value.ToString();
				info.Exist			= bool.Parse(dgvProdInfo.Rows[3].Cells[2].Value.ToString());
				info.ResultTop		= (int)Enum.Parse(typeof(ProdStatus), dgvProdInfo.Rows[4].Cells[2].Value.ToString());
				info.ResultBtm		= (int)Enum.Parse(typeof(ProdStatus), dgvProdInfo.Rows[5].Cells[2].Value.ToString());
				info.VerifyTopDone	= bool.Parse(dgvProdInfo.Rows[6].Cells[2].Value.ToString());
				info.VerifyBtmDone	= bool.Parse(dgvProdInfo.Rows[7].Cells[2].Value.ToString());
				info.PickStatus		= (int)Enum.Parse(typeof(ProdStatus), dgvProdInfo.Rows[8].Cells[2].Value.ToString());
				info.TrayCol		= int.Parse(dgvProdInfo.Rows[9].Cells[2].Value.ToString());
				info.TrayRow		= int.Parse(dgvProdInfo.Rows[10].Cells[2].Value.ToString());
				info.InspTimeTop	= dgvProdInfo.Rows[11].Cells[2].Value == null ? "" : dgvProdInfo.Rows[11].Cells[2].Value.ToString();
				info.InspTimeBtm	= dgvProdInfo.Rows[12].Cells[2].Value == null ? "" : dgvProdInfo.Rows[12].Cells[2].Value.ToString();
				info.SerialNumber	= dgvProdInfo.Rows[13].Cells[2].Value == null ? "" : dgvProdInfo.Rows[13].Cells[2].Value.ToString();

				m_CopyProdInfo = info;
				m_IsCopyDone = true;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	btnPaste_Click(object sender, EventArgs e)
		{
			try
			{
				frmMessageBox pMsg;

				if(m_productPos == teProductPos.Part)
				{
					pMsg = new frmMessageBox(frmMessageBox.teButtons.OKCancel, Language.languageDefault.MB_Confirmation, string.Format("{0}\nProd: {1}, Tray: {2}", Language.languageDefault.MB_Paste, m_ProductIndex, m_flowPart.ToString()));
				}
				else
				{
					pMsg = new frmMessageBox(frmMessageBox.teButtons.OKCancel, Language.languageDefault.MB_Confirmation, string.Format("{0}\nProd: {1}, Tray: {2}", Language.languageDefault.MB_Paste, m_ProductIndex, m_flowTray.ToString()));
				}
				if (pMsg.ShowDialog() == DialogResult.Cancel) return;

				dgvProdInfo.Rows[0].Cells[3].Value = m_CopyProdInfo.ProdID;
				dgvProdInfo.Rows[1].Cells[3].Value = m_CopyProdInfo.TrayID;
				dgvProdInfo.Rows[2].Cells[3].Value = m_CopyProdInfo.KeyValue;
				dgvProdInfo.Rows[3].Cells[3].Value = m_CopyProdInfo.Exist;
				dgvProdInfo.Rows[4].Cells[3].Value = ((ProdStatus)m_CopyProdInfo.ResultTop).ToString();
				dgvProdInfo.Rows[5].Cells[3].Value = ((ProdStatus)m_CopyProdInfo.ResultBtm).ToString();
				dgvProdInfo.Rows[6].Cells[3].Value = m_CopyProdInfo.VerifyTopDone;
				dgvProdInfo.Rows[7].Cells[3].Value = m_CopyProdInfo.VerifyBtmDone;
				dgvProdInfo.Rows[8].Cells[3].Value = m_CopyProdInfo.PickStatus;
				dgvProdInfo.Rows[9].Cells[3].Value = m_CopyProdInfo.TrayCol;
				dgvProdInfo.Rows[10].Cells[3].Value = m_CopyProdInfo.TrayRow;
				dgvProdInfo.Rows[11].Cells[3].Value = m_CopyProdInfo.InspTimeTop;
				dgvProdInfo.Rows[12].Cells[3].Value = m_CopyProdInfo.InspTimeBtm;
				dgvProdInfo.Rows[13].Cells[3].Value = m_CopyProdInfo.SerialNumber;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	dgvProdInfo_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				if (e.ColumnIndex < 3)			return;
				if (e.RowIndex < 0)				return;

				frmKeyboard			popup_key;
				frmKeyPad			popup_num;
				frmComboBoxSelect	popup_cbs;
				
				string				str_old	= "";
				bool				bool_old = false;
				int					nsel_no	= 0;

				switch (e.RowIndex)
				{
					case 0: //ProdID
					case 1: //TrayID
                    case 2: //KeyValue
                        if (dgvProdInfo.Rows[e.RowIndex].Cells[3].Value != null)
						{
							str_old = dgvProdInfo.Rows[e.RowIndex].Cells[3].Value.ToString();
						}
						popup_key = new frmKeyboard(str_old);
						if (popup_key.ShowDialog() == DialogResult.Cancel)			return;

						dgvProdInfo.Rows[e.RowIndex].Cells[3].Value = popup_key.Value;
						break;
                    case 9: //TrayRow
                    case 10: //TrayCol
                        Label lb = new Label();
						popup_num = new frmKeyPad(lb, dgvProdInfo.Rows[e.RowIndex].Cells[3].Value.ToString());
						popup_num.TopMost = true;
						popup_num.ShowInTaskbar = false;
						if (popup_num.ShowDialog() == DialogResult.Cancel)			return;
						
						dgvProdInfo.Rows[e.RowIndex].Cells[3].Value = lb.Text;
						break;
                    case 5: //ResultBtm
                    case 4: //ResultTop
						popup_cbs = new frmComboBoxSelect();
						nsel_no = (int)Enum.Parse(typeof(ProdStatus), dgvProdInfo.Rows[e.RowIndex].Cells[3].Value.ToString());
						if (popup_cbs.ShowDialog(Enum.GetNames(typeof(ProdStatus)), nsel_no) == DialogResult.Cancel)			return;

						dgvProdInfo.Rows[e.RowIndex].Cells[3].Value = ((ProdStatus)popup_cbs.SelNo).ToString();
						break;
                    case 8: //PickSatus
						popup_cbs = new frmComboBoxSelect();
						nsel_no = (int)Enum.Parse(typeof(ProdPickStatus), dgvProdInfo.Rows[e.RowIndex].Cells[3].Value.ToString());
						if (popup_cbs.ShowDialog(Enum.GetNames(typeof(ProdPickStatus)), nsel_no) == DialogResult.Cancel)			return;

						dgvProdInfo.Rows[e.RowIndex].Cells[3].Value = ((ProdPickStatus)popup_cbs.SelNo).ToString();
						break;
                    case 3: //Exist
                    case 6: //VerifyTopDone
                    case 7: //VerifyBtmDone
                        popup_cbs = new frmComboBoxSelect();
                        bool_old = bool.Parse(dgvProdInfo.Rows[e.RowIndex].Cells[3].Value.ToString());
                        if (popup_cbs.ShowDialog(frmComboBoxSelect.teType.True) == DialogResult.Cancel) return;

                        dgvProdInfo.Rows[e.RowIndex].Cells[3].Value = Convert.ToBoolean(popup_cbs.SelNo).ToString();
                        break;
                }
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	btnClose_Click(object sender, EventArgs e)
		{
			if(m_productPos == teProductPos.Part)
			{

			}
			else
			{

			}
		}



		private	void	SetLanguageText()
		{
			try
			{
				btnClose.Text		= Language.languageDefault.BTN_Close;
				btnClear.Text		= Language.languageDefault.BTN_Clear;
				btnClearAll.Text	= Language.languageDefault.BTN_ClearAll;
				btnSave.Text		= Language.languageDefault.BTN_SAVE;
				btnSaveAll.Text		= Language.languageDefault.BTN_SaveAll;
				btnCopy.Text		= Language.languageDefault.BTN_Copy;
				btnPaste.Text		= Language.languageDefault.BTN_Paste;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}



		private	void	tmrSSDEditorUpdate_Tick(object sender, EventArgs e)
		{
			stProdInfo temp = new stProdInfo();

			switch (m_productPos)
			{
				case teProductPos.Part:
					{
						if (m_flowPart == FlowPart.Max)				return;
						if (SM.FMP.IsFlow(m_flowPart) == false)			break;

						temp = SM.FMP[m_flowPart].GetInfo().ProdInfo[m_ProductIndex];
					}
					break;
				case teProductPos.Tray:
					{
						if (m_flowTray == FlowTray.Max)				return;
						if (SM.FMT.IsFlow(m_flowTray) == false)			break;

						temp = SM.FMT[m_flowTray].GetInfo().ProdInfo[m_ProductIndex];
					}
					break;
				case teProductPos.Reject:
					{
						if (m_flowReject == FlowTray.Max)			return;
						if (SM.FMT.IsFlow(m_flowReject) == false)		break;

						temp = SM.FMT[m_flowReject].GetInfo().ProdInfo[m_ProductIndex];
					}
					break;
				default:
					break;
			}
			
			if(dgvProdInfo.Rows.Count == 14)
			{
				dgvProdInfo.Rows[0].Cells[2].Value = temp.ProdID;
				dgvProdInfo.Rows[1].Cells[2].Value = temp.TrayID;
				dgvProdInfo.Rows[2].Cells[2].Value = temp.KeyValue;
				dgvProdInfo.Rows[3].Cells[2].Value = temp.Exist;
				dgvProdInfo.Rows[4].Cells[2].Value = ((ProdStatus)temp.ResultTop).ToString();
				dgvProdInfo.Rows[5].Cells[2].Value = ((ProdStatus)temp.ResultBtm).ToString();
				dgvProdInfo.Rows[6].Cells[2].Value = temp.VerifyTopDone;
				dgvProdInfo.Rows[7].Cells[2].Value = temp.VerifyBtmDone;
				dgvProdInfo.Rows[8].Cells[2].Value = temp.PickStatus;
				dgvProdInfo.Rows[9].Cells[2].Value = temp.TrayRow;
				dgvProdInfo.Rows[10].Cells[2].Value = temp.TrayCol;
				dgvProdInfo.Rows[11].Cells[2].Value = temp.InspTimeTop;
				dgvProdInfo.Rows[12].Cells[2].Value = temp.InspTimeBtm;
				dgvProdInfo.Rows[13].Cells[2].Value = temp.SerialNumber;
			}
			
			if (m_IsCopyDone == false || dgvProdInfo.Rows.Count < 1)			btnPaste.Enabled = false;
		}
	}
}