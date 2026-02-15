using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ditec.RIS.RFO.Dol;
using Ditec.RIS.SysFra.Svc.Utils;

namespace Ditec.RIS.RFO.Svc
{
	public class ClaimsWithErrorRfo
	{
		internal static readonly IamClaimWithError RightFO_Read = new IamClaimWithError(ClaimsRFO.RightFO_Read, "Pre čítanie chýba požadované oprávnenie");

		internal static readonly IamClaimWithError RightFO_Modify = new IamClaimWithError(ClaimsRFO.RightFO_Modify, "Pre zmenu údajov chýba požadované oprávnenie");
	}
}
