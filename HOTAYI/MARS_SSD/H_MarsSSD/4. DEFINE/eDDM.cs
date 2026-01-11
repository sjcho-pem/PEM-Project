using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace H_MarsSSD
{
    // DDM(Double형 DM)
    public enum eDDM
    {
		MainAirUpThresh	= 0,
		BlowAirUpThresh,
		LdPnPAirUpThresh,
		LdInspPnPAirUpThresh,
		ULdPnPAirUpThresh,
		ULdInspPnPAirUpThresh,
		
		MainAirDownThresh	,
		BlowAirDownThresh,
		LdPnPAirDownThresh,
		LdInspPnPAirDownThresh,
		ULdPnPAirDownThresh,
		ULdInspPnPAirDownThresh,

		DM255	= 255,
    }
}
