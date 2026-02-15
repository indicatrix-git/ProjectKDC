using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ditec.RIS.RFO.Dol;

namespace Ditec.RIS.RFO.Dal.Inf
{
    public interface IVztahovaFyzOsoba
    {
        List<VztahovaFyzOsoba> VztahovaFyzOsobaFOGet(Guid FyzickaOsobaID);
        List<VztahovaFyzOsoba> VztahovaFyzOsobaFORodic(Guid? FyzickaOsobaID = null, DateTime? DatumSimulacie = null);
    }
}
