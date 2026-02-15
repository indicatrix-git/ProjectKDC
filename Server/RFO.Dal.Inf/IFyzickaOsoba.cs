using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ditec.RIS.RFO.Dol;

namespace Ditec.RIS.RFO.Dal.Inf
{
    public interface IFyzickaOsoba
    {
        List<FyzickaOsoba> FyzickaOsobaFind(string RodneCislo, DateTime? DatumNarodenia, List<Meno> menoList, List<Priezvisko> priezviskoList, List<RodnePriezvisko> rodnePriezviskoList, Guid? stupenZverejnenia = null);
        List<TypOsobyVRis> FyzickaOsobaFindVazby(int EDUID);
		FyzickaOsoba FyzickaOsobaEduIdGet(string EduId);
        Osoba FyzickaOsobaCreateBroker(Osoba osoba);
		FyzickaOsoba Update(FyzickaOsoba dataObject);

        FyzickaOsoba FyzickaOsobaEduIdGet(int EduId);

        List<FyzickaOsoba> FyzickaOsobaIfoList(object criteria);

		FyzickaOsoba FyzickaOsobaEduIdStotoznenaGet(string EduId);
    }
}
