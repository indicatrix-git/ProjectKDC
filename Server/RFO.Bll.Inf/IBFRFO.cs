using Ditec.RIS.RFO.Dol;
using Ditec.SysFra.DataTypes.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ditec.RIS.RFO.Bll.Inf
{
    public interface IBFRFO
    {
        /// <summary>
        /// Nacita sa FO a vsetky jej naviazane tabulky
        /// </summary>
        /// <param name="IDFyziskaOsoba"></param>
        /// <param name="EDUID"></param>
        /// <returns></returns>
        Osoba GetOsoba(Guid? IDFyziskaOsoba = null, int? EDUID = null, bool NovaOsoba = false);

        void ZapisVystupXML(object input, string name = null);

        /// <summary>
        /// Zaktualizuju sa FO a vsetky naviazane tabulky podla novych hodnot
        /// </summary>
        /// <param name="osobaNew">nove zaznamy, ktorymi sa bude DB aktualizovat</param>
        /// <param name="osobaDB">povodne zaznamy z DB</param>
        /// <returns></returns>
        bool AktualizujOsobu(Osoba osobaNew, Osoba osobaDB);

        NajdenaOsoba NajdenieOsobyVRISPodlaIFOaIFOPravejOsoby(string IFO, string IFOPravejOsoby, bool throwExeption = false);

        bool StotoznenieOsobRIS(int EDUIDPravejOsoby, int? EDUIDPovodnejOsoby, string IFOPovodnejOsoby, FyzickaOsoba osobaPrava = null, FyzickaOsoba osobaPovodna = null);

        bool VratIFOPodlaEDUID(int? EDUID, out string ifo);

        ZapisSparovanieOsobRisRfoRetVal ZapisSparovanieOsobRisRfo(int? EDUID, string IFO, string IFOPravejOsoby, Spracovanie Spracovanie, FyzickaOsoba osobaKtorejSaNasloEDUID = null, OsobaResponse vyhladanaOsobaIFOOnline = null, bool throwExeption = false);

        /// <summary>
        /// RFO RIS Zápis novej osoby na základe osoby z RFO
        /// </summary>
        /// <param name="IFO"></param>
        /// <param name="IFOPravejOsoby"></param>
        /// <returns></returns>
        FyzickaOsoba ZapisNovejOsobyNaZakladeOsobyRFO(string IFO, string IFOPravejOsoby);

		/// <summary>
		/// Nacitanie zoznamu zaznamov
		/// </summary>
		/// <param name="filterCriteria">filtrovacie kriteria.</param>
		/// <returns></returns>
		RequestResult<List<FyzickaOsoba>> FyzickaOsobaListPaged(FyzickaOsobaFilterCriteria filterCriteria);

		/// <summary>
		/// Nacitanie detailu
		/// </summary>
		/// <param name="filterCriteria"></param>
		/// <returns></returns>
		RequestResult<Osoba> GetFyzickaOsoba(FyzickaOsobaFilterCriteria filterCriteria);

        /// <summary>
        /// Osoba v RFO sa oznaci za zaujmovu
        /// </summary>
        /// <param name="IFOList"></param>
        /// <returns></returns>
        Dol.OznacenieZaujmovejOsobyWS.TransEnvTypeOut OznacenieZaujmovejOsoby(List<string> IFOList, Osoba osoba = null, bool throwExeption = false);

        /// <summary>
        /// Potvrdi sa prevzatie konkretnej davky do RFO
        /// </summary>
        /// <param name="IdentifikatorPrijatejDavky"></param>
        /// <returns></returns>
        Dol.PotvrdzovaniePrijatiaZmienWS.TransEnvTypeOut PotvrdzovaniePrijatiaZmien(long IdentifikatorPrijatejDavky);

        /// <summary>
        /// Ulozi sa zaznam
        /// </summary>
        /// <param name="log"></param>
        void UlozZmenovyLog(Ditec.RIS.SIS.Dol.DAVKA.LogSpracovaniaDavok log);

		/// <summary>
		/// Zrusi oznacenie zaujmovej osoby
		/// </summary>
		/// <param name="IFOList"></param>
		/// <returns></returns>
        Dol.ZrusenieOznaceniaZaujmovejOsoby.TransEnvTypeOut ZrusenieOznaceniaZaujmovejOsoby(List<string> IFOList, bool throwExeption = false);

		/// <summary>
		/// Zmena udajov fyzickej osoby
		/// </summary>
		/// <param name="dataObject"></param>
		/// <returns></returns>
		RequestResult<FyzickaOsoba> FyzickaOsobaUpdate(FyzickaOsoba dataObject);

        /// <summary>
        /// Zapis cudzinca do RFO
        /// </summary>
        /// <param name="osoba"></param>
        /// <param name="throwException"></param>
        /// <returns></returns>
        Dol.ZapisNovychOsobWS.TransEnvTypeOut ZapisNovychOsobDoRFO(Osoba osoba, bool throwException = false);

        /// <summary>
        /// Zapis mena do RFO
        /// </summary>
        /// <param name="IFO"></param>
        /// <param name="values"></param>
        /// <param name="identifikatorPoslednejDavky"></param>
        /// <param name="throwException"></param>
        /// <returns></returns>
        Dol.ZapisUpravyMenaWS.TransEnvTypeOut ZapisUpravyMenaDoRFO(string IFO, List<UpdateValue> values, long? identifikatorPoslednejDavky = null, bool throwException = false);

        /// <summary>
        /// Zapis priezviska do RFO
        /// </summary>
        /// <param name="IFO"></param>
        /// <param name="values"></param>
        /// <param name="identifikatorPoslednejDavky"></param>
        /// <param name="throwException"></param>
        /// <returns></returns>
        Dol.ZapisUpravyPriezviskaWS.TransEnvTypeOut ZapisUpravyPriezviskaDoRFO(string IFO, List<UpdateValue> values, long? identifikatorPoslednejDavky = null, bool throwException = false);

		/// <summary>
		/// EDU ID pravej odsoby podla EDU ID fyzickej osoby
		/// </summary>
		/// <param name="fyzickaOsoba"></param>
		/// <returns></returns>
		RequestResult<Int32?> GetEduIdPravejOsoby(FyzickaOsoba fyzickaOsoba);
    }
}
