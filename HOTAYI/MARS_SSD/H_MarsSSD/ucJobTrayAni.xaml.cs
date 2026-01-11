using System;
using PMN.Control.WPF.MachineAni;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;


namespace H_MarsSSD
{
	public partial class ucJobTrayAni : UserControl
	{
		public ucJobTrayAni()
		{
			InitializeComponent();
		}

		public	void	UpdateTray(Color color)
		{
			UpdateJobTrayPocket();
			UpdateTrayCountDisplay();
			UpdateobProdColor(color);
		}

		// 트레이 정보
		private void UpdateJobTrayPocket()
		{
			try
			{
				int ncnt_col	= SM.SHM.GetRecipeDataToInt(DEVICE.TrayColCnt) / 1000;
				int	ncnt_row	= SM.SHM.GetRecipeDataToInt(DEVICE.TrayRowCnt) / 1000;
				
				if (ucLdTray1._CountCol != ncnt_col) ucLdTray1._CountCol = ncnt_col;
				if (ucLdTray1._CountRow != ncnt_row) ucLdTray1._CountRow = ncnt_row;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void UpdateTrayCountDisplay()
        {
			int nColCount	= SM.SHM.GetRecipeDataToInt(DEVICE.TrayColCnt) / 1000;
			int nRowCount	= SM.SHM.GetRecipeDataToInt(DEVICE.TrayRowCnt) / 1000;
            Grid grdTray = new Grid();

			grdTray = GrdLdTray1;
            
            if (grdTray.RowDefinitions.Count == nRowCount && grdTray.ColumnDefinitions.Count == nColCount )
                return;

            grdTray.RowDefinitions.Clear();
            grdTray.ColumnDefinitions.Clear();
            grdTray.Children.Clear();

            for (int i = 0; i < nRowCount; i++)
            {
                grdTray.RowDefinitions.Add(new RowDefinition());
            }
            for (int i = 0; i < nColCount; i++)
            {
                grdTray.ColumnDefinitions.Add(new ColumnDefinition());
                if (i == 0  || (i == nColCount-1 && i % 5 != 0) || i % 5 == 4) // 처음, 끝, 5의 배수
                {
                    //상단
                    TextBlock tbTop = new TextBlock();
                    tbTop.TextAlignment = TextAlignment.Center;
                    tbTop.Text = (i+1).ToString();
                    Grid.SetColumn(tbTop,i);
                    Grid.SetRow(tbTop,0);
                    tbTop.VerticalAlignment = VerticalAlignment.Top;
                    tbTop.Margin = new Thickness(-4,2,-3,2);
                    grdTray.Children.Add(tbTop);

                    //하단
                    TextBlock tbBottom = new TextBlock();
                    tbBottom.TextAlignment = TextAlignment.Center;
                    tbBottom.Text = (i+1).ToString();
                    Grid.SetColumn(tbBottom,i);
                    Grid.SetRow(tbBottom,nRowCount);
                    tbBottom.VerticalAlignment = VerticalAlignment.Bottom;
                    tbBottom.Margin =new Thickness(-3,1,-3,1);

                    grdTray.Children.Add(tbBottom);

                }
            }
        }

		// 자재 색상
		private void UpdateobProdColor(Color color)
		{
			try
			{
				List<ucFigureArray.tsDrawInfo>	info	= new List<ucFigureArray.tsDrawInfo>();

				int nProdCnt = SM.FMP[FlowPart.LDPnP].GetMaxProdCount() * (SM.SHM.GetRecipeDataToInt(DEVICE.GripStepCnt) / 1000 + 1);

				for (int i = 0; i < nProdCnt; i++)
				{
					if (i % (SM.SHM.GetRecipeDataToInt(DEVICE.GripStepCnt) / 1000 + 1) == 0)
					{
						info.Add(new ucFigureArray.tsDrawInfo(1, Colors.Black, color));
					}
					else
					{
						info.Add(new ucFigureArray.tsDrawInfo(1, Colors.Black, Colors.White));
					}
				}

				ucLdTray1._SetInsideFigure(info);
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
	}
}
