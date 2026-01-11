using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Cognex.DataMan.SDK;
using System.Threading;
using Cognex.DataMan.SDK.Discovery;
using Cognex.DataMan.SDK.Utils;
using System.IO;

namespace H_MarsSSD
{
	public partial class frmBarcodeViewer:Form
	{
		private enum Barcode
		{
			LOADER1,
			LOADER2,
			UNLOAD1,
			UNLOAD2,
			REJECT1,
			REJECT2,
		}
		#region [ Variables ]
		private bool m_bIsLive = false;
		public bool IsLive
		{
			get { return m_bIsLive;}
		}
		#endregion

		public frmBarcodeViewer()
		{
			InitializeComponent();
			G.LoadOESBarcode.ResultEventData += UpdateBarcodeResult;
			G.LoadLPCAMMBarcode.ResultEventData += UpdateBarcodeResult;
			G.UnloadOESBarcode.ResultEventData += UpdateBarcodeResult;
			G.UnloadLPCAMMBarcode.ResultEventData += UpdateBarcodeResult;
			G.RejectOESBarcode.ResultEventData += UpdateBarcodeResult;
			G.RejectLPCAMMBarcode.ResultEventData += UpdateBarcodeResult;
		}

		private Barcode SelectedBarcode = Barcode.LOADER1; // 0 : Tray , 1 : Cap

		#region [ Event Function ]

		private void frmBarcodeViewer_Load(object sender, EventArgs e)
		{
			try
			{	
				rbLoader1.Checked = true;
				RadioButton_CheckedChanged(null,null);
			}
			catch(Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void chkLive_CheckStateChanged(object sender, EventArgs e)
		{
			try
			{
				m_bIsLive = chkLive.Checked;
				if	(chkLive.Checked == true)
				{
					btnTrigger.Enabled	= false;
					rbLoader2.Enabled		= false;
					rbLoader1.Enabled		= false;
					switch (SelectedBarcode)
					{
						case Barcode.LOADER1:	G.LoadOESBarcode.LiveOn();	break;
                        case Barcode.LOADER2:	G.LoadLPCAMMBarcode.LiveOn();	break;
                        case Barcode.UNLOAD1:	G.UnloadOESBarcode.LiveOn();	break;
                        case Barcode.UNLOAD2:	G.UnloadLPCAMMBarcode.LiveOn();	break;
                        case Barcode.REJECT1:	G.RejectOESBarcode.LiveOn();	break;
                        case Barcode.REJECT2:	G.RejectLPCAMMBarcode.LiveOn();	break;
                    }
				}
				else
				{
					btnTrigger.Enabled	= true;
					rbLoader2.Enabled		= true;
					rbLoader1.Enabled		= true;
					switch (SelectedBarcode)
					{
						case Barcode.LOADER1:	G.LoadOESBarcode.LiveOff();		break;
						case Barcode.LOADER2:	G.LoadLPCAMMBarcode.LiveOff();		break;
                        case Barcode.UNLOAD1:	G.UnloadOESBarcode.LiveOff();		break;
                        case Barcode.UNLOAD2:	G.UnloadLPCAMMBarcode.LiveOff();		break;
                        case Barcode.REJECT1:	G.RejectOESBarcode.LiveOff();		break;
                        case Barcode.REJECT2:	G.RejectLPCAMMBarcode.LiveOff();		break;
                    }
				}
			}
			catch(Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		#endregion

		#region [ Button Event ]

		private void btnTrigger_MouseDown(object sender, MouseEventArgs e)
		{
			try
			{
				if (SelectedBarcode == Barcode.LOADER1  &&  G.LoadOESBarcode.IsConnect() == true)
				{
					G.LoadOESBarcode.TriggerOn();
					lbBarcodeInfo.Text = "READING...";
				}
				else if (SelectedBarcode == Barcode.LOADER2  &&  G.LoadLPCAMMBarcode.IsConnect() == true)
				{
					G.LoadLPCAMMBarcode.TriggerOn();
					lbBarcodeInfo.Text = "READING...";
				}
				else if (SelectedBarcode == Barcode.UNLOAD1  &&  G.UnloadOESBarcode.IsConnect() == true)
				{
					G.UnloadOESBarcode.TriggerOn();
					lbBarcodeInfo.Text = "READING...";
				}
				else if (SelectedBarcode == Barcode.UNLOAD2  &&  G.UnloadLPCAMMBarcode.IsConnect() == true)
				{
					G.UnloadLPCAMMBarcode.TriggerOn();
					lbBarcodeInfo.Text = "READING...";
				}
				else if (SelectedBarcode == Barcode.REJECT1  &&  G.RejectOESBarcode.IsConnect() == true)
				{
					G.RejectOESBarcode.TriggerOn();
					lbBarcodeInfo.Text = "READING...";
				}
				else if (SelectedBarcode == Barcode.REJECT2  &&  G.RejectLPCAMMBarcode.IsConnect() == true)
				{
					G.RejectLPCAMMBarcode.TriggerOn();
					lbBarcodeInfo.Text = "READING...";
				}
			}
			catch(Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void btnTrigger_MouseUp(object sender, MouseEventArgs e)
		{
			try
			{
				if (SelectedBarcode == Barcode.LOADER1  &&  G.LoadOESBarcode.IsConnect() == true)
				{
					G.LoadOESBarcode.TriggerOff();
				}
				else if (SelectedBarcode == Barcode.LOADER2  &&  G.LoadLPCAMMBarcode.IsConnect() == true)
				{
					G.LoadLPCAMMBarcode.TriggerOff();
				}
				else if (SelectedBarcode == Barcode.UNLOAD1  &&  G.UnloadOESBarcode.IsConnect() == true)
				{
					G.UnloadOESBarcode.TriggerOff();
				}
				else if (SelectedBarcode == Barcode.UNLOAD2  &&  G.UnloadLPCAMMBarcode.IsConnect() == true)
				{
					G.UnloadLPCAMMBarcode.TriggerOff();
				}
				else if (SelectedBarcode == Barcode.REJECT1  &&  G.RejectOESBarcode.IsConnect() == true)
				{
					G.RejectOESBarcode.TriggerOff();
				}
				else if (SelectedBarcode == Barcode.REJECT2  &&  G.RejectLPCAMMBarcode.IsConnect() == true)
				{
					G.RejectLPCAMMBarcode.TriggerOff();
				}

				if (lbBarcodeInfo.Text  ==  "READING...")	lbBarcodeInfo.Text = "FAIL";

			}
			catch(Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
		
		private void RadioButton_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				chkLive.Checked = false;

				if		(rbLoader1.Checked == true)	SelectedBarcode = Barcode.LOADER1;
                else if (rbLoader2.Checked == true)	SelectedBarcode = Barcode.LOADER2;
                else if (rbUnload1.Checked == true)	SelectedBarcode = Barcode.UNLOAD1;
                else if (rbUnload2.Checked == true)	SelectedBarcode = Barcode.UNLOAD2;
                else if (rbReject1.Checked == true)	SelectedBarcode = Barcode.REJECT1;
                else if (rbReject2.Checked == true)	SelectedBarcode = Barcode.REJECT2;

                bool bIsConnect = false;

				switch (SelectedBarcode)
				{
					case Barcode.LOADER1:	bIsConnect = G.LoadOESBarcode.IsConnect();		lbDeviceName.Text	= G.LoadOESBarcode.DeviceName;		break;
					case Barcode.LOADER2:	bIsConnect = G.LoadLPCAMMBarcode.IsConnect();	lbDeviceName.Text	= G.LoadLPCAMMBarcode.DeviceName;		break;
					case Barcode.UNLOAD1:	bIsConnect = G.UnloadOESBarcode.IsConnect();	lbDeviceName.Text	= G.UnloadOESBarcode.DeviceName;		break;
					case Barcode.UNLOAD2:	bIsConnect = G.UnloadLPCAMMBarcode.IsConnect();	lbDeviceName.Text	= G.UnloadLPCAMMBarcode.DeviceName;		break;
					case Barcode.REJECT1:	bIsConnect = G.RejectOESBarcode.IsConnect();	lbDeviceName.Text	= G.RejectOESBarcode.DeviceName;		break;
					case Barcode.REJECT2:	bIsConnect = G.RejectLPCAMMBarcode.IsConnect();	lbDeviceName.Text	= G.RejectLPCAMMBarcode.DeviceName;		break;
				}

				if (bIsConnect== false)
				{
					chkLive.Enabled		= false;
					btnTrigger.Enabled	= false;
					lbDeviceName.Text	= "DEVICE DISCONNECT";
					UpdateBarcodeResult("-");
				}
				else
				{
					chkLive.Enabled		= true;
					btnTrigger.Enabled	= true;
				}
			
			}
			catch(Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void btmClose_Click(object sender, EventArgs e)
		{
			chkLive.Checked = false;

			this.Hide();
		}

		#endregion

		#region [ Functions ]

		#region - Public Functions

		public void UpdateLiveImage(Image image)
		{
			try
			{
				if (this.Visible == false)	return;

				switch (SelectedBarcode)
				{
					case Barcode.LOADER1:	picResultImage.Image = Gui.ResizeImageToBitmap(G.LoadOESBarcode.m_LiveImages, G.LoadOESBarcode.m_LiveImages.Size);	    break;
                    case Barcode.LOADER2:	picResultImage.Image = Gui.ResizeImageToBitmap(G.LoadLPCAMMBarcode.m_LiveImages, G.LoadLPCAMMBarcode.m_LiveImages.Size);      break;
                    case Barcode.UNLOAD1:	picResultImage.Image = Gui.ResizeImageToBitmap(G.UnloadOESBarcode.m_LiveImages, G.UnloadOESBarcode.m_LiveImages.Size);    break;
                    case Barcode.UNLOAD2:	picResultImage.Image = Gui.ResizeImageToBitmap(G.UnloadLPCAMMBarcode.m_LiveImages, G.UnloadLPCAMMBarcode.m_LiveImages.Size);    break;
                    case Barcode.REJECT1:	picResultImage.Image = Gui.ResizeImageToBitmap(G.RejectOESBarcode.m_LiveImages, G.RejectOESBarcode.m_LiveImages.Size);    break;
                    case Barcode.REJECT2:	picResultImage.Image = Gui.ResizeImageToBitmap(G.RejectLPCAMMBarcode.m_LiveImages, G.RejectLPCAMMBarcode.m_LiveImages.Size);    break;
                }
				picResultImage.Invalidate();
			}
			catch(Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		#endregion

		#region - Priavte Functions

		private void UpdateBarcodeResult(string strResult)
		{
			try
			{
				if (this.Visible == false)	return;
				
				this.Invoke(new MethodInvoker(delegate() 
				{ 
					//바코드 정보 표시
					if (string.IsNullOrWhiteSpace(strResult) == true)
						lbBarcodeInfo.Text = "NO READ BARCODE DATA";
					else
						lbBarcodeInfo.Text = strResult;
				
				}));

				//바코드 이미지 표시
				if (SelectedBarcode == Barcode.LOADER1 && G.LoadOESBarcode.m_Images != null)
				{
					Image fitted_image = Gui.ResizeImageToBitmap(G.LoadOESBarcode.m_Images, G.LoadOESBarcode.m_Images.Size);

					//바코드 이미지 가이드라인 표시
					if (G.LoadOESBarcode.m_ltImagesGraphics.Count > 0)
					{
						using (Graphics g = Graphics.FromImage(fitted_image))
						{
							foreach (var graphics in G.LoadOESBarcode.m_ltImagesGraphics)
							{
								ResultGraphics rg = GraphicsResultParser.Parse(graphics, new Rectangle(0, 0, fitted_image.Width, fitted_image.Height));
								ResultGraphicsRenderer.PaintResults(g, rg);
							}
						}
					}

					if (picResultImage.Image != null)
					{
						var image = picResultImage.Image;
						picResultImage.Image = null;
						image.Dispose();
					}

					picResultImage.Image = fitted_image;
					picResultImage.Invalidate();
					G.LoadOESBarcode.m_ltImagesGraphics.Clear();
				}
				else if (SelectedBarcode == Barcode.LOADER2 && G.LoadLPCAMMBarcode.m_Images != null)
				{
					Image fitted_image = Gui.ResizeImageToBitmap(G.LoadLPCAMMBarcode.m_Images, G.LoadLPCAMMBarcode.m_Images.Size);

					//바코드 이미지 가이드라인 표시
					if (G.LoadLPCAMMBarcode.m_ltImagesGraphics.Count > 0)
					{
						using (Graphics g = Graphics.FromImage(fitted_image))
						{
							foreach (var graphics in G.LoadLPCAMMBarcode.m_ltImagesGraphics)
							{
								ResultGraphics rg = GraphicsResultParser.Parse(graphics, new Rectangle(0, 0, fitted_image.Width, fitted_image.Height));
								ResultGraphicsRenderer.PaintResults(g, rg);
							}
						}
					}

					if (picResultImage.Image != null)
					{
						var image = picResultImage.Image;
						picResultImage.Image = null;
						image.Dispose();
					}

					picResultImage.Image = fitted_image;
					picResultImage.Invalidate();
					G.LoadLPCAMMBarcode.m_ltImagesGraphics.Clear();
				}
				else if (SelectedBarcode == Barcode.UNLOAD1 && G.UnloadOESBarcode.m_Images != null)
				{
					Image fitted_image = Gui.ResizeImageToBitmap(G.UnloadOESBarcode.m_Images, G.UnloadOESBarcode.m_Images.Size);

					//바코드 이미지 가이드라인 표시
					if (G.UnloadOESBarcode.m_ltImagesGraphics.Count > 0)
					{
						using (Graphics g = Graphics.FromImage(fitted_image))
						{
							foreach (var graphics in G.UnloadOESBarcode.m_ltImagesGraphics)
							{
								ResultGraphics rg = GraphicsResultParser.Parse(graphics, new Rectangle(0, 0, fitted_image.Width, fitted_image.Height));
								ResultGraphicsRenderer.PaintResults(g, rg);
							}
						}
					}

					if (picResultImage.Image != null)
					{
						var image = picResultImage.Image;
						picResultImage.Image = null;
						image.Dispose();
					}

					picResultImage.Image = fitted_image;
					picResultImage.Invalidate();
					G.UnloadOESBarcode.m_ltImagesGraphics.Clear();
				}
				else if (SelectedBarcode == Barcode.UNLOAD2 && G.UnloadLPCAMMBarcode.m_Images != null)
				{
					Image fitted_image = Gui.ResizeImageToBitmap(G.UnloadLPCAMMBarcode.m_Images, G.UnloadLPCAMMBarcode.m_Images.Size);

					//바코드 이미지 가이드라인 표시
					if (G.UnloadLPCAMMBarcode.m_ltImagesGraphics.Count > 0)
					{
						using (Graphics g = Graphics.FromImage(fitted_image))
						{
							foreach (var graphics in G.UnloadLPCAMMBarcode.m_ltImagesGraphics)
							{
								ResultGraphics rg = GraphicsResultParser.Parse(graphics, new Rectangle(0, 0, fitted_image.Width, fitted_image.Height));
								ResultGraphicsRenderer.PaintResults(g, rg);
							}
						}
					}

					if (picResultImage.Image != null)
					{
						var image = picResultImage.Image;
						picResultImage.Image = null;
						image.Dispose();
					}

					picResultImage.Image = fitted_image;
					picResultImage.Invalidate();
					G.UnloadLPCAMMBarcode.m_ltImagesGraphics.Clear();
				}
				else if (SelectedBarcode == Barcode.REJECT1 && G.RejectOESBarcode.m_Images != null)
				{
					Image fitted_image = Gui.ResizeImageToBitmap(G.RejectOESBarcode.m_Images, G.RejectOESBarcode.m_Images.Size);

					//바코드 이미지 가이드라인 표시
					if (G.RejectOESBarcode.m_ltImagesGraphics.Count > 0)
					{
						using (Graphics g = Graphics.FromImage(fitted_image))
						{
							foreach (var graphics in G.RejectOESBarcode.m_ltImagesGraphics)
							{
								ResultGraphics rg = GraphicsResultParser.Parse(graphics, new Rectangle(0, 0, fitted_image.Width, fitted_image.Height));
								ResultGraphicsRenderer.PaintResults(g, rg);
							}
						}
					}

					if (picResultImage.Image != null)
					{
						var image = picResultImage.Image;
						picResultImage.Image = null;
						image.Dispose();
					}

					picResultImage.Image = fitted_image;
					picResultImage.Invalidate();
					G.RejectOESBarcode.m_ltImagesGraphics.Clear();
				}
				
				else if (SelectedBarcode == Barcode.REJECT2 && G.RejectLPCAMMBarcode.m_Images != null)
				{
					Image fitted_image = Gui.ResizeImageToBitmap(G.RejectLPCAMMBarcode.m_Images, G.RejectLPCAMMBarcode.m_Images.Size);

					//바코드 이미지 가이드라인 표시
					if (G.RejectLPCAMMBarcode.m_ltImagesGraphics.Count > 0)
					{
						using (Graphics g = Graphics.FromImage(fitted_image))
						{
							foreach (var graphics in G.RejectLPCAMMBarcode.m_ltImagesGraphics)
							{
								ResultGraphics rg = GraphicsResultParser.Parse(graphics, new Rectangle(0, 0, fitted_image.Width, fitted_image.Height));
								ResultGraphicsRenderer.PaintResults(g, rg);
							}
						}
					}

					if (picResultImage.Image != null)
					{
						var image = picResultImage.Image;
						picResultImage.Image = null;
						image.Dispose();
					}

					picResultImage.Image = fitted_image;
					picResultImage.Invalidate();
					G.RejectLPCAMMBarcode.m_ltImagesGraphics.Clear();
				}
			}
			catch(Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		#endregion

		#region - Language Function 

		public void SetLanguageText()
        {
			try
			{
				btnTrigger.Text		= Language.languageDefault.Trigger.ToUpper();
				btmClose.Text		= Language.languageDefault.BTN_Close.ToUpper();
				chkLive.Text		= Language.languageDefault.LiveImage.ToUpper();

				lbDeviceNameText.Text	= Language.languageDefault.DeviceName.ToUpper() + " :";
				lbQRInfoText.Text		= Language.languageDefault.BarcodeInfo.ToUpper() + " :";
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
