using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace H_MarsSSD
{
	public partial class frmImageViewer : Form
	{
		public			frmImageViewer()
		{
			InitializeComponent();
		}

		public			frmImageViewer(string strFilePathName)
		{
			InitializeComponent();

			if (File.Exists(strFilePathName) == false)			return;

			picViewer.Image	= new Bitmap(strFilePathName);
		}

		private	void	frmImageViewer_Load(object sender, EventArgs e)
		{
			this.Location	= new Point(Screen.PrimaryScreen.Bounds.Width / 2, 0);
			this.Width		= Screen.PrimaryScreen.Bounds.Width / 2;
			this.Height		= Screen.PrimaryScreen.Bounds.Height;
		}


		public	void	SetImage(string strFilePathName)
		{
			if (File.Exists(strFilePathName) == false)			return;

			picViewer.Image	= new Bitmap(strFilePathName);
		}

		private	void	btnClose_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
