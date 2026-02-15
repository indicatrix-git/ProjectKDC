using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ditec.RIS.RFO.Bll.Inf
{
	/// <summary>
	/// Poskytuje zoznam procesov na automaticke spustanie 
	/// </summary>
	public enum ProcessType
	{
        [Description("Aktualizacia ciselnikov z RFO")]
        [EnumMember]
        AktualizaciaCiselnikov = 0,
        [Description("Aktualizacia ciselnikov z RFO a SpracovanieZmenovychDavok")]
		[EnumMember]
        AktualizaciaCiselnikovSpracovanieZmenovychDavok = 1,
        [Description("Spracovanie inicialnej davky")]
		[EnumMember]
        SpracovanieXML = 2,
        [Description("Kontrola pripadne nezapisanych osob")]
		[EnumMember]
        KontrolaXML = 3,
        [Description("Vymaz vztahovych osob")]
		[EnumMember]
        VymazVztahovychOsob = 4
	}
}
