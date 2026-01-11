using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace H_MarsSSD
{
	public partial class ucProgressBar : Panel
	{
		private	int		m_Minimum	= 0;
		private	int		m_Maximum	= 100;
		private	int		m_nValue	= 0;
		private	Brush	m_brushProgressing	= Brushes.LawnGreen;
		private	Brush	m_brushProgressed	= Brushes.LimeGreen;

		public	int	Minimum { get => m_Minimum; set { m_Minimum = value; this.Invalidate(); } }
		public	int	Maximum { get => m_Maximum; set { m_Maximum = value; this.Invalidate(); } }
		public	int	Value
		{ 
			get => m_nValue;
			set
			{
				if (value > Maximum)		m_nValue = Maximum;
				else						m_nValue = value;
				this.Invalidate();
			}
		}
		public	Brush	BrushProgressing	{ get => m_brushProgressing; set { m_brushProgressing = value; this.Invalidate(); } }
		public	Brush	BrushProgressed		{ get => m_brushProgressed; set { m_brushProgressed = value; this.Invalidate(); } }

		public ucProgressBar()
		{
			InitializeComponent();

			this.DoubleBuffered	= true;
		}

		private void ucProgressBar_Paint(object sender, PaintEventArgs e)
		{
			float	fwidth	= (float)this.Width / ((float)Maximum - (float)Minimum) * Value;

			if (Maximum == Value)
			{
				fwidth	= this.Width;
				e.Graphics.FillRectangle(BrushProgressed, new RectangleF(0, 0, fwidth, this.Height));
			}
			else
			{
				e.Graphics.FillRectangle(BrushProgressing, new RectangleF(0, 0, fwidth, this.Height));
			}
		}
	}
}
