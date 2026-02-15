using Ditec.RIS.RFO.Dol;
using Ditec.RIS.RFO.Svc.Inf;
using Ditec.RIS.SysFra.Svc.Utils;
using Ditec.SysFra.DataTypes.Infrastructure;
using LinFu.IoC.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ditec.Log.SysFra.Iam.Impl;
using Ditec.Iam.Authorization;
using Ditec.SysFra.Infrastructure.Impl;
using System.ServiceModel;
using System.Reflection;
using Ditec.RIS.RFO.Bll.Inf;
using Ditec.RIS.CC.Bll.Inf;
using Ditec.RIS.CC.Dol;

namespace Ditec.RIS.RFO.Svc
{
	[Implements(typeof(IRfoService))]
	[Implements(typeof(RfoService))]
	public class RfoService : RISServiceBase, IRfoService
	{
		#region RfoForBrowse

		public RequestResult<List<FyzickaOsoba>> FyzickaOsobaListPaged(FyzickaOsobaFilterCriteria filterCriteria)
		{
			var result = new RequestResult<List<FyzickaOsoba>>();
			try
			{
				// overenie opravnenia
				this.CheckClaim(ClaimsWithErrorRfo.RightFO_Read);
				// zapis vstupnych parametrov do logovacieho modulu 
				this.AddStartEvent();
				// zavolanie BusinessRules metody 
				result = GetBussinessLogicLayer<IBFRFO>().FyzickaOsobaListPaged(filterCriteria);
			}
			catch (NotAuthorizedException ex)
			{
				ExceptionHandling.HandleBusinessException(this, ex, 200, MethodInfo.GetCurrentMethod().Name + ": " + ex.ToString());
				throw new FaultException<NotAuthorizedFault>(new NotAuthorizedFault(), ex.ErrorMessage);
			}
			catch (Exception ex)
			{
				// zalogovanie chyby do logovacieho modulu 
				this.AddError(ex, MethodBase.GetCurrentMethod().Name);
				// zalogovanie chyby do filesystemu a pridanie chybovej hlasky do RequestResult obalky 
                ExceptionHandling.HandleResponse(result, this, ex, 100, "Operácia neprebehla úspešne: RfoService.FyzickaOsobaListPaged()");
			}

			return result;
		}

		#endregion RfoForBrowse

		#region FyzickaOsoba

		public RequestResult<Osoba> GetFyzickaOsoba(FyzickaOsobaFilterCriteria filterCriteria)
		{
			var result = new RequestResult<Osoba>();
			try
			{
				// overenie opravnenia
				this.CheckClaim(ClaimsWithErrorRfo.RightFO_Read);
				// zapis vstupnych parametrov do logovacieho modulu 
				this.AddStartEvent();
				// zavolanie BusinessRules metody 
				result = GetBussinessLogicLayer<IBFRFO>().GetFyzickaOsoba(filterCriteria);
			}
			catch (NotAuthorizedException ex)
			{
				ExceptionHandling.HandleBusinessException(this, ex, 201, MethodInfo.GetCurrentMethod().Name + ": " + ex.ToString());
				throw new FaultException<NotAuthorizedFault>(new NotAuthorizedFault(), ex.ErrorMessage);
			}
			catch (Exception ex)
			{
				// zalogovanie chyby do logovacieho modulu 
				this.AddError(ex, MethodBase.GetCurrentMethod().Name);
				// zalogovanie chyby do filesystemu a pridanie chybovej hlasky do RequestResult obalky 
                ExceptionHandling.HandleResponse(result, this, ex, 101, "Operácia neprebehla úspešne: RfoService.GetFyzickaOsoba()");
			}

			return result;
		}

		#endregion FyzickaOsoba

		#region FyzickaOsobaModify
		public RequestResult<FyzickaOsoba> FyzickaOsobaModify(FyzickaOsoba dataObject)
		{
			var result = new RequestResult<FyzickaOsoba>();
			try
			{
				this.TransactionID = this.CreateTransaction();
				// overenie opravnenia
				this.CheckClaim(ClaimsWithErrorRfo.RightFO_Modify);
				// zapis vstupnych parametrov do logovacieho modulu 
				this.AddStartEvent();

				// zavolanie BusinessRules metody 
				result = GetBussinessLogicLayer<IBFRFO>().FyzickaOsobaUpdate(dataObject);
			}
			catch (NotAuthorizedException ex)
			{
				ExceptionHandling.HandleBusinessException(this, ex, 202, MethodInfo.GetCurrentMethod().Name + ": " + ex.ToString());
				throw new FaultException<NotAuthorizedFault>(new NotAuthorizedFault(), ex.ErrorMessage);
			}
			catch (Exception ex)
			{
				// zalogovanie chyby do logovacieho modulu 
				this.AddError(ex, MethodBase.GetCurrentMethod().Name);
				// zalogovanie chyby do filesystemu a pridanie chybovej hlasky do RequestResult obalky 
                ExceptionHandling.HandleResponse(result, this, ex, 102, "Operácia neprebehla úspešne: RfoService.FyzickaOsobaModify()");
			}

			return result;
		}

		#endregion

        #region Komunikacia s RFO

        public RequestResult<Dol.PotvrdzovaniePrijatiaZmienWS.TransEnvTypeOut> PotvrdzovaniePrijatiaZmien(long IdentifikatorPrijatejDavky)
        {
            var result = new RequestResult<Dol.PotvrdzovaniePrijatiaZmienWS.TransEnvTypeOut>();
            try
            {
                // overenie opravnenia
                this.CheckClaim(ClaimsWithErrorRfo.RightFO_Read);
                // zapis vstupnych parametrov do logovacieho modulu 
                this.AddStartEvent();
                // zavolanie BusinessRules metody 
                result.Response = GetBussinessLogicLayer<IBFRFO>().PotvrdzovaniePrijatiaZmien(IdentifikatorPrijatejDavky);
            }
            catch (NotAuthorizedException ex)
            {
                ExceptionHandling.HandleBusinessException(this, ex, 203, MethodInfo.GetCurrentMethod().Name);
                throw new FaultException<NotAuthorizedFault>(new NotAuthorizedFault(), ex.ErrorMessage);
            }
            catch (Exception ex)
            {
                // zalogovanie chyby do logovacieho modulu 
                this.AddError(ex, MethodBase.GetCurrentMethod().Name);
                // zalogovanie chyby do filesystemu a pridanie chybovej hlasky do RequestResult obalky 
                ExceptionHandling.HandleResponse(result, this, ex, 103, "Operácia neprebehla úspešne: RfoService.PotvrdzovaniePrijatiaZmien()");
            }

            return result;
        }

        #endregion Komunikacia s RFO

		#region StotoznenieOsobRIS

		public RequestResult<Boolean> StotoznenieOsobRIS(int EDUIDPravejOsoby, int? EDUIDPovodnejOsoby, string IFOPovodnejOsoby, FyzickaOsoba osobaPrava = null, FyzickaOsoba osobaPovodna = null)
		{
			var result = new RequestResult<Boolean>();
			try
			{
				this.TransactionID = this.CreateTransaction();
				// overenie opravnenia
				this.CheckClaim(ClaimsWithErrorRfo.RightFO_Modify);
				// zapis vstupnych parametrov do logovacieho modulu 
				this.AddStartEvent();
				// zavolanie BusinessRules metody 
				result.Response = GetBussinessLogicLayer<IBFRFO>().StotoznenieOsobRIS(EDUIDPravejOsoby, EDUIDPovodnejOsoby, IFOPovodnejOsoby, osobaPrava, osobaPovodna);
			}
			catch (NotAuthorizedException ex)
			{
				ExceptionHandling.HandleBusinessException(this, ex, 204, MethodInfo.GetCurrentMethod().Name + ": " + ex.ToString());
				throw new FaultException<NotAuthorizedFault>(new NotAuthorizedFault(), ex.ErrorMessage);
			}
			catch (Exception ex)
			{
				// zalogovanie chyby do logovacieho modulu 
				this.AddError(ex, MethodBase.GetCurrentMethod().Name);
				// zalogovanie chyby do filesystemu a pridanie chybovej hlasky do RequestResult obalky 
                ExceptionHandling.HandleResponse(result, this, ex, 104, "Operácia neprebehla úspešne: RfoService.StotoznenieOsobRIS()");
			}

			return result;
		}

		#endregion StotoznenieOsobRIS

		#region ZrusenieOznaceniaZaujmovejOsoby

		public RequestResult<Boolean> ZrusenieOznaceniaZaujmovejOsoby(String IFO)
		{
			var result = new RequestResult<Boolean>();
			try
			{
				this.TransactionID = this.CreateTransaction();
				// overenie opravnenia
				this.CheckClaim(ClaimsWithErrorRfo.RightFO_Modify);
				// zapis vstupnych parametrov do logovacieho modulu 
				this.AddStartEvent();
				// zavolanie BusinessRules metody 
				List<String> listIFO = new List<String>() { IFO };
				if (GetBussinessLogicLayer<IBFRFO>().ZrusenieOznaceniaZaujmovejOsoby(listIFO, true) != null)
				{
					result.Response = true;
				}
				else
				{
					result.Response = false;
				}
			}
			catch (NotAuthorizedException ex)
			{
				ExceptionHandling.HandleBusinessException(this, ex, 205, MethodInfo.GetCurrentMethod().Name + ": " + ex.ToString());
				throw new FaultException<NotAuthorizedFault>(new NotAuthorizedFault(), ex.ErrorMessage);
			}
			catch (Exception ex)
			{
				// zalogovanie chyby do logovacieho modulu 
				this.AddError(ex, MethodBase.GetCurrentMethod().Name);
				// zalogovanie chyby do filesystemu a pridanie chybovej hlasky do RequestResult obalky 
                ExceptionHandling.HandleResponse(result, this, ex, 105, "Operácia neprebehla úspešne: RfoService.ZrusenieOznaceniaZaujmovejOsoby()");
			}

			return result;
		}

		#endregion

		#region VyhladanieZoznamuIFOVratFyzickeOsoby

		public RequestResult<List<Osoba>> VyhladanieZoznamuIFOVratFyzickeOsoby(string RodneCislo, string Meno, string Priezvisko, string RodnePriezvisko, DateTime? DatumNarodenia)
		{

			//List<Meno> MenoList, List<Priezvisko> PriezviskoList, List<RodnePriezvisko> RodnePriezviskoList
			var result = new RequestResult<List<Osoba>>();
			try
			{
				// overenie opravnenia
				this.CheckClaim(ClaimsWithErrorRfo.RightFO_Read);
				// zapis vstupnych parametrov do logovacieho modulu 
				this.AddStartEvent();
				// zavolanie BusinessRules metody
				List<Meno> listMeno = null;
				List<Priezvisko> listPriezvisko = null;
				List<RodnePriezvisko> listRodnePriezvisko = null;

				if (!String.IsNullOrEmpty(Meno))
				{
					listMeno = new List<Meno> { new Meno() { Hodnota = Meno } };
				}
				if (!String.IsNullOrEmpty(Priezvisko))
				{
					listPriezvisko = new List<Priezvisko> { new Priezvisko() { Hodnota = Priezvisko } };
				}
				if (!String.IsNullOrEmpty(RodnePriezvisko))
				{
					listRodnePriezvisko = new List<RodnePriezvisko> { new RodnePriezvisko() { Hodnota = RodnePriezvisko } };
				}

                //naplnim si zasobnik ciselnikov, aby som ho mohol pouzit pri konverzii na osobu v dol vrstve, kde si uz nemozem nacitavat ciselniky z DB
                GetBussinessLogicLayer<IBFCodeList>().GetBufferedCodeList<StavDavky>();
                GetBussinessLogicLayer<IBFCodeList>().GetBufferedCodeList<RfoTitul>();
                GetBussinessLogicLayer<IBFCodeList>().GetBufferedCodeList<RfoStat>();
                GetBussinessLogicLayer<IBFCodeList>().GetBufferedCodeList<RfoNarodnost>();
                GetBussinessLogicLayer<IBFCodeList>().GetBufferedCodeList<RfoPohlavie>();
                GetBussinessLogicLayer<IBFCodeList>().GetBufferedCodeList<RfoTypPobytu>();
                GetBussinessLogicLayer<IBFCodeList>().GetBufferedCodeList<RfoOkres>();
                GetBussinessLogicLayer<IBFCodeList>().GetBufferedCodeList<RfoObec>();
                GetBussinessLogicLayer<IBFCodeList>().GetBufferedCodeList<RfoUlica>();
                GetBussinessLogicLayer<IBFCodeList>().GetBufferedCodeList<RfoUzemnyCelok>();

                //kvoli konverzii z RFO datoveho typu a naslednom ulozeni do DB aj s vazbami na RFOCiselniky (tie, ktore nie je potrebne pri Ris osobach)
                GetBussinessLogicLayer<IBFCodeList>().GetBufferedCodeList<RfoRodinnyStav>();
                GetBussinessLogicLayer<IBFCodeList>().GetBufferedCodeList<RfoTypOsoby>();
                GetBussinessLogicLayer<IBFCodeList>().GetBufferedCodeList<RfoStupenZverejnenia>();
                GetBussinessLogicLayer<IBFCodeList>().GetBufferedCodeList<RfoTypRodinnehoVztahu>();
                GetBussinessLogicLayer<IBFCodeList>().GetBufferedCodeList<RfoTypRoleVRodiVztahu>();
                GetBussinessLogicLayer<IBFCodeList>().GetBufferedCodeList<RfoSposobilostPravUkon>();
                GetBussinessLogicLayer<IBFCodeList>().GetBufferedCodeList<ChybovaSprava>();

				OsobaResponse osobaResponse = GetBussinessLogicLayer<IBFZoznamIFOPodlaKriterii>().VyhľadanieZoznamuIFO(RodneCislo, listMeno, listPriezvisko, listRodnePriezvisko, DatumNarodenia, true, true);
				if (osobaResponse != null && osobaResponse.OsobaList != null && osobaResponse.OsobaList.Count > 0)
				{
					result.Response = osobaResponse.OsobaList;
				}
			}
			catch (NotAuthorizedException ex)
			{
				ExceptionHandling.HandleBusinessException(this, ex, 206, MethodInfo.GetCurrentMethod().Name + ": " + ex.ToString());
				throw new FaultException<NotAuthorizedFault>(new NotAuthorizedFault(), ex.ErrorMessage);
			}
			catch (Exception ex)
			{
				// zalogovanie chyby do logovacieho modulu 
				this.AddError(ex, MethodBase.GetCurrentMethod().Name);

				//Nie je zadaná minimálna množina vyhľadávacích kritérií!
				if (ex.Message.Equals(GetBussinessLogicLayer<IBFCodeList>().GetBufferedCodeList<ChybovaSprava>().GetText(51606)))
				{
					//Nie je zadaná minimálna množina vyhľadávacích kritérií!
					ExceptionHandling.AddResponseMessageWarning(result, this, GetBussinessLogicLayer<IBFCodeList>().GetBufferedCodeList<ChybovaSprava>().GetText(51606));
				}
				else 
				{
					// zalogovanie chyby do filesystemu a pridanie chybovej hlasky do RequestResult obalky 
					ExceptionHandling.HandleResponse(result, this, ex, 106, "Operácia neprebehla úspešne: RfoService.VyhladanieZoznamuIFOVratFyzickeOsoby()");
				}	
			}

			return result;
		}

		#endregion

		#region NajdenieOsobyVRISPodlaIFOaIFOPravejOsoby

		public RequestResult<NajdenaOsoba> NajdenieOsobyVRISPodlaIFOaIFOPravejOsoby(string IFO, string IFOPravejOsoby)
		{
			var result = new RequestResult<NajdenaOsoba>();
			try
			{
				// overenie opravnenia
				this.CheckClaim(ClaimsWithErrorRfo.RightFO_Read);
				// zapis vstupnych parametrov do logovacieho modulu 
				this.AddStartEvent();
				// zavolanie BusinessRules metody 
				result.Response = GetBussinessLogicLayer<IBFRFO>().NajdenieOsobyVRISPodlaIFOaIFOPravejOsoby(IFO, IFOPravejOsoby, true);
				
			}
			catch (NotAuthorizedException ex)
			{
				ExceptionHandling.HandleBusinessException(this, ex, 207, MethodInfo.GetCurrentMethod().Name + ": " + ex.ToString());
				throw new FaultException<NotAuthorizedFault>(new NotAuthorizedFault(), ex.ErrorMessage);
			}
			catch (Exception ex)
			{
				// zalogovanie chyby do logovacieho modulu 
				this.AddError(ex, MethodBase.GetCurrentMethod().Name);
				// zalogovanie chyby do filesystemu a pridanie chybovej hlasky do RequestResult obalky 
                ExceptionHandling.HandleResponse(result, this, ex, 107, "Operácia neprebehla úspešne: RfoService.NajdenieOsobyVRISPodlaIFOaIFOPravejOsoby()");
			}

			return result;
		}

		#endregion

	    #region ZapisSparovanieOsobRisRfo

		public RequestResult<ZapisSparovanieOsobRisRfoRetVal> ZapisSparovanieOsobRisRfo(int? EDUID, string IFO, string IFOPravejOsoby, Spracovanie Spracovanie = Spracovanie.Hromadne, FyzickaOsoba osobaKtorejSaNasloEDUID = null, OsobaResponse vyhladanaOsobaIFOOnline = null)
		{
			var result = new RequestResult<ZapisSparovanieOsobRisRfoRetVal>();
			try
			{
				// overenie opravnenia
				this.CheckClaim(ClaimsWithErrorRfo.RightFO_Modify);
				// zapis vstupnych parametrov do logovacieho modulu 
				this.AddStartEvent();
				// zavolanie BusinessRules metody 
				result.Response = GetBussinessLogicLayer<IBFRFO>().ZapisSparovanieOsobRisRfo(EDUID, IFO, IFOPravejOsoby, Spracovanie, osobaKtorejSaNasloEDUID, vyhladanaOsobaIFOOnline, true);
				
			}
			catch (NotAuthorizedException ex)
			{
				ExceptionHandling.HandleBusinessException(this, ex, 208, MethodInfo.GetCurrentMethod().Name + ": " + ex.ToString());
				throw new FaultException<NotAuthorizedFault>(new NotAuthorizedFault(), ex.ErrorMessage);
			}
			catch (Exception ex)
			{
				// zalogovanie chyby do logovacieho modulu 
				this.AddError(ex, MethodBase.GetCurrentMethod().Name);
				// zalogovanie chyby do filesystemu a pridanie chybovej hlasky do RequestResult obalky 
                ExceptionHandling.HandleResponse(result, this, ex, 108, "Operácia neprebehla úspešne: RfoService.ZapisSparovanieOsobRisRfo()");
			}

			return result;
		}

		#endregion

		#region EduId Pravej Osoby

		/// <summary>
		/// EDU ID pravej odsoby podla EDU ID fyzickej osoby
		/// </summary>
		/// <param name="filterCriteria"></param>
		/// <returns></returns>
		public RequestResult<Int32?> GetEduIdPravejOsoby(FyzickaOsoba fyzickaOsoba)
		{
			var result = new RequestResult<Int32?>();
			try
			{
				// overenie opravnenia
				this.CheckClaim(ClaimsWithErrorRfo.RightFO_Read);
				// zapis vstupnych parametrov do logovacieho modulu 
				this.AddStartEvent();
				// zavolanie BusinessRules metody 
				result = GetBussinessLogicLayer<IBFRFO>().GetEduIdPravejOsoby(fyzickaOsoba);
			}
			catch (NotAuthorizedException ex)
			{
				ExceptionHandling.HandleBusinessException(this, ex, 209, MethodInfo.GetCurrentMethod().Name + ": " + ex.ToString());
				throw new FaultException<NotAuthorizedFault>(new NotAuthorizedFault(), ex.ErrorMessage);
			}
			catch (Exception ex)
			{
				// zalogovanie chyby do logovacieho modulu 
				this.AddError(ex, MethodBase.GetCurrentMethod().Name);
				// zalogovanie chyby do filesystemu a pridanie chybovej hlasky do RequestResult obalky 
				ExceptionHandling.HandleResponse(result, this, ex, 109, "Operácia neprebehla úspešne: RfoService.GetEduIdPravejOsoby()");
			}

			return result;
		}

		#endregion
	}
}
