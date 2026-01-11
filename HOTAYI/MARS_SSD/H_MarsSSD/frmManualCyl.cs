using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Linq;
using System.ComponentModel;
using System.Windows.Threading;
using System.Collections.Generic;

namespace H_MarsSSD
{
	public partial class frmManualCyl : Form
	{
        public delegate void btnMotorClickEventHandler(int nMtIdx);
        public event btnMotorClickEventHandler BtnMotorClick;
        protected virtual void btnMotorClickEvent(int nMtIdx)
        {
            if (BtnMotorClick != null)
                BtnMotorClick(nMtIdx);
        }

		//TableLayoutPanel이 한번에 갱신되게 하기 위해 추가
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cp = base.CreateParams;
				cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
				return cp;
			}
		}

		public			frmManualCyl()
		{
			InitializeComponent();
			InitTableLayout();
			InitLabel();
		}


		private void	InitTableLayout()
		{
			try
			{
				for (int i = 0; i < (int)Seq.Max; i++)
				{
					TableLayoutPanel rowLayoutPanel = tlpManualCyl.GetControlFromPosition(0, i) as TableLayoutPanel;
					Array enumValues = GetEnumArray((Seq)i);

					if (rowLayoutPanel != null)
					{
						foreach (object enumValue in enumValues)
						{
							if (enumValue.ToString() == "Max") break;

							int nRow = (int)enumValue / 8;
							if ((int)enumValue == 24) nRow = 2;

							int nCol = (int)enumValue - (nRow * 8) + 1;
							// 각 enum 값에 대한 버튼/라벨 생성
							Label label = new Label();
							label.Text = enumValue.ToString();
							label.Dock = DockStyle.Fill;
							label.Font = new Font("Segoe UI", 9, FontStyle.Bold);
							label.Margin = new Padding(1);
							label.TextAlign = ContentAlignment.MiddleCenter;

							Button button = new Button();
							button.Name = ((Seq)i).ToString() + "_" + enumValue.ToString();
							button.Dock = DockStyle.Fill;
							button.Font = new Font("Segoe UI", 9, FontStyle.Bold);
							button.Margin = new Padding(1);
							button.TextAlign = ContentAlignment.MiddleCenter;
							button.Click += btn_Click;

							// 특정 위치에 버튼 추가
							rowLayoutPanel.Controls.Add(label, nCol, nRow * 2);
							rowLayoutPanel.Controls.Add(button, nCol, nRow * 2 + 1);
						}

						// ALL 버튼
						int manual_start	= 0;
						int manual_end		= 0;
						int ncol			= 0;

						if ((Seq)i == Seq.LoadPnP)
						{
							manual_start	= (int)MANUAL.LoadPnP_AllUp;
							manual_end		= (int)MANUAL.LoadPnP_AllUpDn;
						}
						else if ((Seq)i == Seq.LoadInspPnP)
						{
							manual_start	= (int)MANUAL.LoadInspPnP_AllUp;
							manual_end		= (int)MANUAL.LoadInspPnP_AllUpDn;
						}
						else if ((Seq)i == Seq.UnloadPnP)
						{
							manual_start	= (int)MANUAL.UnloadPnP_AllUp;
							manual_end		= (int)MANUAL.UnloadPnP_AllUpDn;
						}
						else if ((Seq)i == Seq.UnloadInspPnP)
						{
							manual_start	= (int)MANUAL.UnloadInspPnP_AllUp;
							manual_end		= (int)MANUAL.UnloadInspPnP_AllUpDn;
						}

						for (int j = manual_start; j < manual_end + 1; j++)
						{
							if (manual_start == 0  && manual_end == 0)		break;

							string[] str = EnumHelper.GetDescription((MANUAL)j).Split('_');

							Button button		= new Button();
							string strvalue1	= str[1];
							string subvalue2	= strvalue1.Substring(0, 3) + "\n" + strvalue1.Substring(3);

							button.Name			= EnumHelper.GetDescription((MANUAL)j);
							button.Text			= subvalue2;
							button.Dock			= DockStyle.Fill;
							button.Font			= new Font("Segoe UI", 9, FontStyle.Bold);
							button.Margin		= new Padding(1);
							button.TextAlign	= ContentAlignment.MiddleCenter;
							button.Click		+= btnAll_Click;

							// 특정 위치에 버튼 추가
							rowLayoutPanel.Controls.Add(button, ncol, rowLayoutPanel.RowCount - 1);
							ncol++;
						}
					}
				}
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void InitLabel()
		{
			Label[] arrLabelImage = new Label[] {	lbLoader,
													lbTrayClean,
													lbLoadTransfer,
													lbLoadTrayTable1,
													lbLoadTrayTable2,
													lbLoadPnP,
													lbLoadBuffer1,
													lbLoadBuffer2,
													lbLoadInspPnP,
													lbInspPlate1,
													lbInspPlate2,
													lbUnloadInspPnP,
													lbUnloadBuffer,
													lbRejectBuffer,
													lbUnloadPnP,
													lbUnloadTrayTable1,
													lbUnloadTrayTable2,
													lbUnloadTransfer,
													lbUnloader	};

			Control container = this.pictureBox1;

			for (int i = 0; i < arrLabelImage.Length; i++)
			{
				arrLabelImage[i].Parent = container;
			}
		}

		private void	frmManualCyl_VisibleChanged(object sender, EventArgs e)
		{
			try
			{
				if (this.Visible == true)			{  tmrUpdate.Start();   SM.SHM.StartManual(); }
				else							    {  tmrUpdate.Stop();    SM.SHM.StopManual(); }
			}
            catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
		
		private Array	GetEnumArray(Seq Seq)
		{
			switch (Seq)
			{
				case Seq.TrayLoader:		return Enum.GetValues(typeof(SeqTrayLoader.Act));
				case Seq.TrayClean:			return Enum.GetValues(typeof(SeqTrayClean.Act));
				case Seq.LoadTransfer:		return Enum.GetValues(typeof(SeqLoadTransfer.Act));
				case Seq.LoadTrayTable1:	return Enum.GetValues(typeof(SeqLoadTrayTable1.Act));
				case Seq.LoadTrayTable2:	return Enum.GetValues(typeof(SeqLoadTrayTable1.Act));
				case Seq.LoadPnP:			return Enum.GetValues(typeof(SeqLoadPnP.Act));
				case Seq.LoadBuffer1:		return Enum.GetValues(typeof(SeqLoadBuffer.Act));
				case Seq.LoadBuffer2:		return Enum.GetValues(typeof(SeqLoadBuffer.Act));
				case Seq.LoadInspPnP:		return Enum.GetValues(typeof(SeqLoadInspPnP.Act));
				case Seq.InspPlate1:		return Enum.GetValues(typeof(SeqInspPlate.Act));
				case Seq.InspPlate2:		return Enum.GetValues(typeof(SeqInspPlate.Act));
				case Seq.UnloadInspPnP:		return Enum.GetValues(typeof(SeqUnloadInspPnP.Act));
				case Seq.UnloadBuffer:		return Enum.GetValues(typeof(SeqUnloadBuffer.Act));
				case Seq.RejectBuffer:		return Enum.GetValues(typeof(SeqRejectBuffer.Act));
				case Seq.UnloadPnP:			return Enum.GetValues(typeof(SeqUnloadPnP.Act));
				case Seq.UnloadTrayTable1:	return Enum.GetValues(typeof(SeqUnloadTrayTable.Act));
				case Seq.UnloadTrayTable2:	return Enum.GetValues(typeof(SeqUnloadTrayTable.Act));
				case Seq.UnloadTransfer:	return Enum.GetValues(typeof(SeqUnloadTransfer.Act));
				case Seq.Unloader:			return Enum.GetValues(typeof(SeqTrayUnloader.teAct));
				default: break;
			}
			return null;
		}

		private string	GetSenPos(string strFwd, string strBwd, SenPos senPos)
		{
			try
			{
				if		((int)senPos == 2)			return strFwd;
				else if ((int)senPos == 3)			return strBwd;
				else								return "-";
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return	"";
			}
		}

		#region button Event
		private void	btn_Click(object sender, EventArgs e)
		{
		   try
			{
				Button				btn			= (Button)sender;
				frmListBoxSelect	ltbox		= new frmListBoxSelect();
				SenPos				senPos		= SenPos.UNKNOWN;
				string btnName = btn.Name;

				List<string> ltlist; // 전진,후진 순으로 리스트
				if		(btnName.Contains("TrayGrip"))									ltlist = new List<string> { "Grip", "Release" };
				else if (btnName.Contains("Grip"))                                      ltlist = new List<string> { "Grip", "Release" };  //ltlist = new List<string> { "Release", "Grip" };
				else if (btnName.Contains("PnPUpDn"))								    ltlist = new List<string> { "Down", "Up" };
				else if (btnName.Contains("TableUpDn")  ||  btnName.Contains("UpDn"))	ltlist = new List<string> { "UP", "Down" };
				else																	ltlist = new List<string> { "Fwd", "Bwd" };

				if (ltbox.ShowDialog(ltlist, btn.Text) == DialogResult.OK)
				{
					if (string.IsNullOrWhiteSpace(ltbox.SelItem) == true)			return;

					if (ltbox.SelItem == ltlist[0])			senPos	= SenPos.FWD;
					else									senPos	= SenPos.BWD;
					
					UI.Logs.Write(LOGPART.TK, string.Format("{0} - {1} Click", btnName, ltbox.SelItem));

					if (CheckInterference(btn, senPos) == false)			return;

					string[] buttonNameParts = btnName.Split('_');
					Seq seq = (Seq)Enum.Parse(typeof(Seq), buttonNameParts[0]);
					Array enumValues = GetEnumArray(seq);
					int index = Array.IndexOf(enumValues, Enum.Parse(enumValues.GetType().GetElementType(), buttonNameParts[1]));

					SM.SHM.SetManualParams( new object[] {seq, index, senPos});
					SM.SHM.SetManualNum(MANUAL.ManualCyl);
				}
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void btnAll_Click(object sender, EventArgs e)
		{
			try
			{
				Button	btn		= (Button)sender;
				string	btnName	= btn.Name;
				MANUAL	manual	= (MANUAL)Enum.Parse(typeof(MANUAL), btnName);

				 frmMessageBox pMsg = new frmMessageBox(true, Language.languageDefault.MB_Confirmation, Language.languageDefault.MB_ManualAct);
				if (pMsg.ShowDialog() == DialogResult.OK)
				{
					SM.SHM.SetManualNum(manual);

					UI.Logs.Write(LOGPART.TK, string.Format("{0} Click", btnName));
				}
			}
			catch(Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void	LabelImage_Click(object sender, EventArgs e)
		{
			try
			{
				Label labelimg = sender as Label;
				int nLabelNum = int.Parse(labelimg.Tag.ToString()) - 1;
				
				Array enumValues = GetEnumArray((Seq)nLabelNum);

				tlpSeletedCyl.Controls.Clear();
				Label labelName			= new Label();
				labelName.Text			= $"[ {nLabelNum+1} ] {((Seq)nLabelNum).ToString() }";
				labelName.Name			= ((Seq)nLabelNum).ToString();
				labelName.Dock			= DockStyle.Fill;
				labelName.Font			= new Font("Segoe UI", 9, FontStyle.Bold);
				labelName.Margin		= new Padding(0);
				labelName.TextAlign		= ContentAlignment.MiddleCenter;
				labelName.BackColor		= Color.FromArgb(255, 224, 192);
				labelName.ForeColor		= Color.Black;
				labelName.BorderStyle	= BorderStyle.FixedSingle;
				tlpSeletedCyl.Controls.Add(labelName, 0, 0);
				tlpSeletedCyl.SetRowSpan(labelName, 7);

				labelName.Click		+= label_DoubleClick;
			
				foreach (object enumValue in enumValues)
				{
					if (enumValue.ToString() == "Max") break;

					int nRow = (int)enumValue / 8;
					if ((int)enumValue == 24) nRow = 2;

					int nCol = (int)enumValue - (nRow * 8) + 1;
					// 각 enum 값에 대한 버튼/라벨 생성
					Label label = new Label();
					label.Text = enumValue.ToString();
					label.Dock = DockStyle.Fill;
					label.Font = new Font("Segoe UI", 9, FontStyle.Bold);
					label.Margin = new Padding(1);
					label.TextAlign = ContentAlignment.MiddleCenter;

					Button button = new Button();
					button.Name = ((Seq)nLabelNum).ToString() + "_" + enumValue.ToString()+ "_Selected";
					button.Dock = DockStyle.Fill;
					button.Font = new Font("Segoe UI", 9, FontStyle.Bold);
					button.Margin = new Padding(1);
					button.TextAlign = ContentAlignment.MiddleCenter;
					button.Click += btn_Click;

					// 특정 위치에 버튼 추가
					tlpSeletedCyl.Controls.Add(label, nCol, nRow * 2);
					tlpSeletedCyl.Controls.Add(button, nCol, nRow * 2 + 1);
				}

				// ALL 버튼
				int manual_start	= 0;
				int manual_end		= 0;
				int ncol			= 0;

				if (labelName.Text.Contains("LoadPnP") == true)
				{
					manual_start	= (int)MANUAL.LoadPnP_AllUp;
					manual_end		= (int)MANUAL.LoadPnP_AllUpDn;
				}
				else if (labelName.Text.Contains("LoadInspPnP") == true)
				{
					manual_start	= (int)MANUAL.LoadInspPnP_AllUp;
					manual_end		= (int)MANUAL.LoadInspPnP_AllUpDn;
				}
				else if (labelName.Text.Contains("UnloadPnP") == true)
				{
					manual_start	= (int)MANUAL.UnloadPnP_AllUp;
					manual_end		= (int)MANUAL.UnloadPnP_AllUpDn;
				}
				else if (labelName.Text.Contains("UnloadInspPnP") == true)
				{
					manual_start	= (int)MANUAL.UnloadInspPnP_AllUp;
					manual_end		= (int)MANUAL.UnloadInspPnP_AllUpDn;
				}

				for (int i = manual_start; i < manual_end + 1; i++)
				{
					if (manual_start == 0  && manual_end == 0)		break;

					string[] str = EnumHelper.GetDescription((MANUAL)i).Split('_');

					Button button		= new Button();
					string strvalue1	= str[1];
					string subvalue2	= strvalue1.Substring(0, 3) + "\n" + strvalue1.Substring(3);

					button.Name			= EnumHelper.GetDescription((MANUAL)i);
					button.Text			= subvalue2;
					button.Dock			= DockStyle.Fill;
					button.Font			= new Font("Segoe UI", 9, FontStyle.Bold);
					button.Margin		= new Padding(1);
					button.TextAlign	= ContentAlignment.MiddleCenter;
					button.Click		+= btnAll_Click;

					// 특정 위치에 버튼 추가
					tlpSeletedCyl.Controls.Add(button, ncol, tlpSeletedCyl.RowCount - 1);
					ncol++;
				}

				tlpSeletedMotor.Controls.Clear();
				labelName				= new Label();
				labelName.Text			= $"[ {nLabelNum+1} ] {((Seq)nLabelNum).ToString() }";
				labelName.Dock			= DockStyle.Fill;
				labelName.Font			= new Font("Segoe UI", 9, FontStyle.Bold);
				labelName.Margin		= new Padding(0);
				labelName.TextAlign		= ContentAlignment.MiddleCenter;
				labelName.BackColor		= Color.FromArgb(255, 224, 192);
				labelName.ForeColor		= Color.Black;
				labelName.BorderStyle	= BorderStyle.FixedSingle;
				tlpSeletedMotor.Controls.Add(labelName, 0, 0);
				
				List<MT> ltMotor = new List<MT>();
				switch ((Seq)nLabelNum)
				{
					case Seq.TrayLoader: 
						ltMotor.Add(MT.LDConv1);
						ltMotor.Add(MT.LDConv2);
						ltMotor.Add(MT.LDTrayLiftZ);
						ltMotor.Add(MT.LDConvStopper);
						ltMotor.Add(MT.LDConvWidth);
						break;
					case Seq.TrayClean:
						break;
					case Seq.LoadTransfer:
						ltMotor.Add(MT.LDTrayTransX);
						ltMotor.Add(MT.LDTrayTransZ);
						ltMotor.Add(MT.LDTrayTransGrip);
                        ltMotor.Add(MT.LDTrayHolder1Z);
                        ltMotor.Add(MT.LDTrayHolder2Z);
                        break;
					case Seq.LoadTrayTable1:
                        ltMotor.Add(MT.LDTable1GripY);
                        ltMotor.Add(MT.LDTable1Z);
                        break;
					case Seq.LoadTrayTable2:
						ltMotor.Add(MT.LDTable2GripY);
                        ltMotor.Add(MT.LDTable2Z);
                        ltMotor.Add(MT.LDTable2X);
                        break;
					case Seq.LoadPnP:
						ltMotor.Add(MT.LDPnPX);
						ltMotor.Add(MT.LDPnPY);
                        ltMotor.Add(MT.LDPnPZ);
                        ltMotor.Add(MT.LDPnPW);
                        break;
                    case Seq.LoadBuffer1:
						ltMotor.Add(MT.LDBuf1Grip);
                        ltMotor.Add(MT.LDBuf1Y);
                        ltMotor.Add(MT.LDBuf1R);
                        break;
					case Seq.LoadBuffer2:
						ltMotor.Add(MT.LDBuf2Grip);
						ltMotor.Add(MT.LDBuf2Y);
                        ltMotor.Add(MT.LDBuf2R);
                        break;
					case Seq.LoadInspPnP:
						ltMotor.Add(MT.LDInspPnPX);
						ltMotor.Add(MT.LDInspPnPZ);
						break;
					case Seq.InspPlate1:
						ltMotor.Add(MT.InspPlate1W);
						ltMotor.Add(MT.InspPlate1Y);
						ltMotor.Add(MT.InspPlate1Z);
						ltMotor.Add(MT.InspPlate1Grip);
						break;
					case Seq.InspPlate2:
						ltMotor.Add(MT.InspPlate2W);
						ltMotor.Add(MT.InspPlate2Y);
						ltMotor.Add(MT.InspPlate2Z);
						ltMotor.Add(MT.InspPlate2Grip);
						break;
					case Seq.UnloadInspPnP:
						ltMotor.Add(MT.ULDInspPnPX);
						ltMotor.Add(MT.ULDInspPnPZ);
						break;
					case Seq.UnloadBuffer:
						ltMotor.Add(MT.ULDBufGripX);
						ltMotor.Add(MT.ULDBufGripY);
						ltMotor.Add(MT.ULDBufY);
						break;
					case Seq.RejectBuffer:
						ltMotor.Add(MT.RejectBufY);
						break;
					case Seq.UnloadPnP:
						ltMotor.Add(MT.ULDPnPX);
						ltMotor.Add(MT.ULDPnPY);
                        ltMotor.Add(MT.ULDPnPZ);
                        ltMotor.Add(MT.ULDPnPW);
                        break;
					case Seq.UnloadTrayTable1:
						ltMotor.Add(MT.ULDTable1GripY);
                        ltMotor.Add(MT.ULDTable1Z);
                        break;
					case Seq.UnloadTrayTable2:
						ltMotor.Add(MT.ULDTable2GripY);
                        ltMotor.Add(MT.ULDTable2Z);
                        break;
					case Seq.UnloadTransfer:
						ltMotor.Add(MT.ULDTrayTransX);
						ltMotor.Add(MT.ULDTrayTransZ);
						ltMotor.Add(MT.ULDTrayTransGrip);
						break;
					case Seq.Unloader:
						ltMotor.Add(MT.ULDConv1);
						ltMotor.Add(MT.ULDConv2);
						ltMotor.Add(MT.ULDTrayLiftZ);
						ltMotor.Add(MT.ULDConvWidth);
						break;
					case Seq.Max:
						break;
					default:
						break;
				}

				int nMotorCol = 1;
				foreach (MT item in ltMotor)
				{
					Button button = new Button();
					button.Name = ((int)item).ToString();
					button.Text = item.ToString();
					button.Dock = DockStyle.Fill;
					button.Font = new Font("Segoe UI", 9, FontStyle.Bold);
					button.Margin = new Padding(1);
					button.TextAlign = ContentAlignment.MiddleCenter;
					button.Click += btnMotor_Click;

					// 특정 위치에 버튼 추가
					tlpSeletedMotor.Controls.Add(button, nMotorCol++, 0);

				}
			}
			catch
			{

			}

		}

		private void	btnMotor_Click(object sender, EventArgs e)
		{
            Button button = sender as Button;
            int nMotorIdx =  int.Parse(button.Name);
            btnMotorClickEvent(nMotorIdx);

		}
		#endregion

		private	void	SetControlEnable(Control Ctrl, bool bEnable)
		{
			try
			{
				if (Ctrl.Enabled != bEnable)			Ctrl.Enabled = bEnable;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	SetControlColor(Control Ctrl, Color BackColor, Color ForeColor)
		{
			try
			{
				if (Ctrl.BackColor != BackColor)			Ctrl.BackColor = BackColor;
				if (Ctrl.ForeColor != ForeColor)			Ctrl.ForeColor = ForeColor;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	bool	CheckInterference(Button btn, SenPos senPos)
		{
			try
			{
				bool	brt		= true;
				string	strmsg	= "";
				string[] splitNames = btn.Name.Split('_');

				if (splitNames.Length < 2 )		return false;


				if (splitNames[0] == Seq.TrayClean.ToString()  && 
					splitNames[1] == SeqTrayClean.Act.Rotate.ToString() )
				{
					if (SM.SHM.GetActPos(Seq.TrayClean, (int)SeqTrayClean.Act.TableUpDn) != SenPos.UP)
					{
						brt = false;
						strmsg = string.Format("\n{0} : Tray Clean - Up", Language.languageDefault.MB_CheckConditions);
					}
				}
				else if (splitNames[0] == Seq.TrayClean.ToString()  && 
						 splitNames[1] == SeqTrayClean.Act.TableUpDn.ToString() )
				{
					if (senPos == SenPos.UP)
					{
						if (SM.SHM.IsBitData(BIT.TrayCleanUpSafety) == false)
						{
							brt		= false;
							strmsg	= string.Format("\n{0} : Transfer Position", Language.languageDefault.MB_CheckConditions);
						}
					}
				}

				if (brt == false)
				{
					frmMessageBox msg	= new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, Language.languageDefault.MB_Interference+strmsg);
					msg.ShowDialog();
				}

				return brt;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		private void	tmrUpdate_Tick(object sender, EventArgs e)
		{
			try
			{
				if (SM.SHM.IsBitData(BIT.AutoRun) == true) SetControlEnable(this, false);
				else SetControlEnable(this, true);

				for (int i = 0; i < (int)Seq.Max; i++)
				{
					Array enumValues = GetEnumArray((Seq)i);

					foreach (object enumValue in enumValues)
					{
						string buttonNameToUpdate =((Seq)i).ToString()+ "_" + enumValue.ToString();
						Control[] foundControls = tlpManualCyl.Controls.Find(buttonNameToUpdate, true); // 버튼 찾기
						if (foundControls.Length > 0 && foundControls[0] is Button)
						{
							Button foundButton = (Button)foundControls[0];

							if (enumValue.ToString().Contains("TrayGrip"))		
								foundButton.Text = GetSenPos("GRIP", "RELEASE", SM.SHM.GetActPos((Seq)i, (int)enumValue));
							else if (enumValue.ToString().Contains("Grip"))
                                foundButton.Text = GetSenPos("GRIP", "RELEASE", SM.SHM.GetActPos((Seq)i, (int)enumValue));
                            //foundButton.Text = GetSenPos("RELEASE", "GRIP", SM.SHM.GetActPos((Seq)i, (int)enumValue));
							else if (enumValue.ToString().Contains("PnPUpDn"))		
								foundButton.Text = GetSenPos("DOWN", "UP", SM.SHM.GetActPos((Seq)i, (int)enumValue));
							else if (enumValue.ToString().Contains("TableUpDn") || enumValue.ToString().Contains("UpDn"))		
								foundButton.Text = GetSenPos("UP", "DOWN", SM.SHM.GetActPos((Seq)i, (int)enumValue));
							else
								foundButton.Text = GetSenPos("FWD", "BWD", SM.SHM.GetActPos((Seq)i, (int)enumValue));
						}

						//선택해서 생긴 tlp업데이트
						string SelectedButtonNameToUpdate =((Seq)i).ToString()+ "_" + enumValue.ToString()+"_Selected";
						Control[] foundSelectedControls = tlpSeletedCyl.Controls.Find(SelectedButtonNameToUpdate, true); // 버튼 찾기
						if (foundSelectedControls.Length > 0 && foundSelectedControls[0] is Button)
						{
							Button foundButton = (Button)foundSelectedControls[0];

							if (enumValue.ToString().Contains("TrayGrip"))		
								foundButton.Text = GetSenPos("GRIP", "RELEASE", SM.SHM.GetActPos((Seq)i, (int)enumValue));
							else if (enumValue.ToString().Contains("Grip"))
                                foundButton.Text = GetSenPos("GRIP", "RELEASE", SM.SHM.GetActPos((Seq)i, (int)enumValue));
                      //      foundButton.Text = GetSenPos("RELEASE", "GRIP", SM.SHM.GetActPos((Seq)i, (int)enumValue));
							else if (enumValue.ToString().Contains("PnPUpDn"))		
								foundButton.Text = GetSenPos("DOWN", "UP", SM.SHM.GetActPos((Seq)i, (int)enumValue));
							else if (enumValue.ToString().Contains("TableUpDn") || enumValue.ToString().Contains("UpDn"))		
								foundButton.Text = GetSenPos("UP", "DOWN", SM.SHM.GetActPos((Seq)i, (int)enumValue));
							else
								foundButton.Text = GetSenPos("FWD", "BWD", SM.SHM.GetActPos((Seq)i, (int)enumValue));
						}
					}
				}
			}
			catch
			{

			}
		}

		private void	label_DoubleClick(object sender, EventArgs e)
		{
			try
			{
				if (sender == null)		return;

				Label label = (Label)sender;
				Seq seq	= (Seq)Enum.Parse(typeof(Seq), label.Name);

				switch (seq)
				{
					case Seq.LoadPnP:
					case Seq.UnloadPnP:
					case Seq.LoadInspPnP:
					case Seq.UnloadInspPnP:
						foreach (Form openForm in Application.OpenForms)
						{
							if (openForm.Text == string.Format("Sub Manual Cyl_{0}", seq.ToString())) // 열린 폼의 이름 검사
							{
								openForm.WindowState = FormWindowState.Normal;
								openForm.BringToFront();
								return;
							}
						}

						frmSubManualCyl frm = new frmSubManualCyl(seq);
						frm.Show();

						UI.Logs.Write(LOGPART.SYS, string.Format("Open Sub Manual Cyl - {0}", seq.ToString()));
						break;
					default:
						break;
				}
			}
			catch(Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
	}
}
