using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ditec.RIS.RFO.Dol;

namespace Ditec.RIS.RFO.Bll.Inf
{
    public interface IBFZoznamIFOPodlaKriterii
    {
        Dol.PoskytnutieUdajovIFOOnlineWS.TransEnvTypeOut VyhľadanieZoznamuIFO(string RodneCislo, List<Meno> MenoList, List<Priezvisko> PriezviskoList, List<RodnePriezvisko> RodnePriezviskoList, DateTime? DatumNarodenia, bool callIfoOnline = true, bool throwExeption = false);

        Dol.PoskytnutieZoznamuIFOPodlaVyhladavacichKriteriiWS.TransEnvTypeOut PoskytnutieZoznamuIFOPodlaVyhladavacichKriterii(string RodneCislo, List<Meno> MenoList, List<Priezvisko> PriezviskoList, List<RodnePriezvisko> RodnePriezviskoList, DateTime? DatumNarodenia, bool throwExeption = false);
    }
}
