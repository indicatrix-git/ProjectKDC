using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ditec.Iam.Authorization;

namespace Ditec.RIS.RFO.Dol
{
	public class ClaimsRFO
	{
		public static Claim RightFO_Read { get { return new Claim("Permission", null, "uri://RIS/RegFO/FO_Read"); } }

		public static Claim RightFO_Modify { get { return new Claim("Permission", null, "uri://RIS/RegFO/FO_Modify_Rel"); } }
	}
}
