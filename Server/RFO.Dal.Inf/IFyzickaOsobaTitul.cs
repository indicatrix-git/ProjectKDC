using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ditec.RIS.RFO.Dol;

namespace Ditec.RIS.RFO.Dal.Inf
{
    public interface IFyzickaOsobaTitul
    {
        List<FyzickaOsobaTitul> FyzickaOsobaTitulFOGet(Guid FyzickaOsobaID);
    }
}
