using Ditec.RIS.RFO.Dol;
using Ditec.SysFra.DataTypes.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Ditec.RIS.RFO.Svc.Inf
{
	[ServiceContract]
    public interface IRfoService
	{
		/// <summary>
		/// Nacitanie zoznamu zaznamov
		/// </summary>
		/// <param name="filterCriteria">filtrovacie kriteria.</param>
		/// <returns></returns>
		[OperationContract]
		RequestResult<List<FyzickaOsoba>> FyzickaOsobaListPaged(FyzickaOsobaFilterCriteria filterCriteria);

		/// <summary>
		/// Nacitanie detailu
		/// </summary>
		/// <param name="filterCriteria">filtrovacie kriteria.</param>
		/// <returns></returns>
		[OperationContract]
		RequestResult<Osoba> GetFyzickaOsoba(FyzickaOsobaFilterCriteria filterCriteria);

		/// <summary>
		/// Zmena udajov
		/// </summary>
		/// <param name="dataObject"></param>
		/// <returns></returns>
		[OperationContract]
		RequestResult<FyzickaOsoba> FyzickaOsobaModify(FyzickaOsoba dataObject);

        /// <summary>
        /// Nacitanie detailu
        /// </summary>
        /// <param name="filterCriteria">filtrovacie kriteria.</param>
        /// <returns></returns>
        [OperationContract]
        RequestResult<Dol.PotvrdzovaniePrijatiaZmienWS.TransEnvTypeOut> PotvrdzovaniePrijatiaZmien(long IdentifikatorPrijatejDavky);

		/// <summary>
		/// Stotoznenie osob RIS
		/// </summary>
		/// <param name="EDUIDPravejOsoby"></param>
		/// <param name="EDUIDPovodnejOsoby"></param>
		/// <param name="IFOPovodnejOsoby"></param>
		/// <param name="osobaPrava"></param>
		/// <param name="osobaPovodna"></param>
		/// <returns></returns>
		[OperationContract]
		RequestResult<Boolean> StotoznenieOsobRIS(int EDUIDPravejOsoby, int? EDUIDPovodnejOsoby, string IFOPovodnejOsoby, FyzickaOsoba osobaPrava = null, FyzickaOsoba osobaPovodna = null);

		/// <summary>
		/// Zrusi oznacenie zaujmovej osoby
		/// </summary>
		/// <param name="IFO"></param>
		/// <returns></returns>
		[OperationContract]
		RequestResult<Boolean> ZrusenieOznaceniaZaujmovejOsoby(String IFO);

		/// <summary>
		/// Vrati zoznam Osob z RFO
		/// </summary>
		/// <param name="RodneCislo"></param>
		/// <param name="Meno"></param>
		/// <param name="Priezvisko"></param>
		/// <param name="RodnePriezvisko"></param>
		/// <param name="DatumNarodenia"></param>
		/// <returns></returns>
		[OperationContract]
		RequestResult<List<Osoba>> VyhladanieZoznamuIFOVratFyzickeOsoby(string RodneCislo, string Meno, string Priezvisko, string RodnePriezvisko, DateTime? DatumNarodenia);

		/// <summary>
		/// Vrati osoby z RIS podla IFO a IFOPravejOsoby
		/// </summary>
		/// <param name="IFO"></param>
		/// <param name="IFOPravejOsoby"></param>
		/// <returns></returns>
		[OperationContract]
		RequestResult<NajdenaOsoba> NajdenieOsobyVRISPodlaIFOaIFOPravejOsoby(string IFO, string IFOPravejOsoby);

		/// <summary>
		/// Sparovanie osob RIS-RFO
		/// </summary>
		/// <param name="EDUID"></param>
		/// <param name="IFO"></param>
		/// <param name="IFOPravejOsoby"></param>
		/// <param name="Spracovanie"></param>
		/// <param name="osobaKtorejSaNasloEDUID"></param>
		/// <param name="vyhladanaOsobaIFOOnline"></param>
		/// <returns></returns>
		[OperationContract]
		RequestResult<ZapisSparovanieOsobRisRfoRetVal> ZapisSparovanieOsobRisRfo(int? EDUID, string IFO, string IFOPravejOsoby, Spracovanie Spracovanie = Spracovanie.Hromadne, FyzickaOsoba osobaKtorejSaNasloEDUID = null, OsobaResponse vyhladanaOsobaIFOOnline = null);

		/// <summary>
		/// EDU ID pravej odsoby podla EDU ID fyzickej osoby
		/// </summary>
		/// <param name="filterCriteria"></param>
		/// <returns></returns>
		[OperationContract]
		RequestResult<Int32?> GetEduIdPravejOsoby(FyzickaOsoba fyzickaOsoba);
	}
}
