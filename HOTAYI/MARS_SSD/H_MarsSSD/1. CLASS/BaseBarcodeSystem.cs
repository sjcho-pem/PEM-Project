using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using Cognex.DataMan.SDK.Discovery;

namespace H_MarsSSD
{
	public class BaseBarcodeSystem
	{
		#region private Variables

		public SerSystemDiscoverer		m_SerSystemDiscoverer	= null;
		public int m_DeviceCount = 0;
		
		#endregion

		#region Events
		public delegate void	ResultEventHandler(string BarcodeData);
		public event			ResultEventHandler ResultEventData;
		#endregion

		public BaseBarcodeSystem()
		{

			m_SerSystemDiscoverer = new SerSystemDiscoverer();
			m_SerSystemDiscoverer.Discover();
		}

		#region public functions
		public void Dispose()
		{
			if (m_SerSystemDiscoverer != null)
			{
				m_SerSystemDiscoverer.Dispose();
			}
			m_SerSystemDiscoverer = null;
		}
		
		#endregion
	}
}

