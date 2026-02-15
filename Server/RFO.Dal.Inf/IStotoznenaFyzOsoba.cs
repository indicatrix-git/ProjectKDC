using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ditec.RIS.RFO.Dol;
namespace Ditec.RIS.RFO.Dal.Inf
{
    public interface IStotoznenaFyzOsoba
    {
        List<StotoznenaFyzOsoba> StotoznenaFyzOsobaIfoList(object criteria);

        List<StotoznenaFyzOsoba> StotoznenaFyzOsobaFOGet(Guid FyzickaOsobaID);

        List<StotoznenaFyzOsoba> StotoznenaFyzickaOsobaFind(string RodneCislo, DateTime? DatumNarodenia, List<Meno> menoList, List<Priezvisko> priezviskoList, List<RodnePriezvisko> rodnePriezviskoList, Guid? stupenZverejnenia = null);

        List<StotoznenaFyzOsoba> StotoznenaFyzickaOsobaEDUIDList(int EDUID);
    }
}
