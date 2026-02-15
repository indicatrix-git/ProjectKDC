using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ditec.RIS.RFO.Bll.PoskytnutieZoznamuIFOPodlaVyhladavacichKriteriiWeb;
using Ditec.SysFra.Infrastructure.Impl;

namespace Ditec.RIS.RFO.Bll
{
	/// <summary>
	/// Trieda obsahujuca volania transakcnych sluzieb, t.j. sluzieb, ktorych vysledkom je zmena udajov registra. 
	/// </summary>
    public class TransactionalWS : BusinessRulesBase
    {
        public Dol.OznacenieZaujmovejOsobyWS.TransEnvTypeOut OznacenieZaujmovejOsobyWSToRfoService(Dol.OznacenieZaujmovejOsobyWS.TransEnvTypeIn transEnvTypeIn)
        {
            try
            {
                //premenna na pripadne logovanie
                SIS.Dol.DAVKA.LogWs logWS = null;
                //ak mam zapnute logovanie sprav, tak si naplnim zoznam volanim
                if (Tools.ShouldLogXmlMessages)
                {
                    logWS = new SIS.Dol.DAVKA.LogWs() { DatumACas = DateTime.Now, NazovWs = "OznacenieZaujmovejOsobyWS", XmlIn = Ditec.RIS.RFO.Dol.ToolsRFO.GetXmlString(transEnvTypeIn) };
                    SIS.Dol.DAVKA.LogSpracovaniaDavok.GetInstance().LogWsList.Add(logWS);
                }

                var output = Tools.CallService<Dol.OznacenieZaujmovejOsobyWS.TransEnvTypeOut>(transEnvTypeIn, Tools.OznacenieZaujmovejOsoby_URL);

                //zaznacim si vystup
                if (Tools.ShouldLogXmlMessages)
                {
                    logWS.XmlOut = Ditec.RIS.RFO.Dol.ToolsRFO.GetXmlString(output);
                    if (output != null && output.ZVY != null)
                        logWS.NavratovyKod = output.ZVY.NK;

                    //zmazem zaznam, ktorym som si stringovy response zapisoval
                    SIS.Dol.DAVKA.LogSpracovaniaDavok.GetInstance().LogWsList.RemoveAll(item => item.NazovWs == "CallRfoService");
                }

                return output;
            }
            catch (Exception ex)
            {
                ExceptionHandling.HandleBusinessException(this, ex, 005, MethodInfo.GetCurrentMethod().Name);
                throw ex;
            }

            return null;
        }

        public Dol.ZrusenieOznaceniaZaujmovejOsoby.TransEnvTypeOut ZrusenieOznaceniaZaujmovejOsobyWSToRfoService(Dol.ZrusenieOznaceniaZaujmovejOsoby.TransEnvTypeIn transEnvTypeIn)
        {
            try
            {
                //premenna na pripadne logovanie
                SIS.Dol.DAVKA.LogWs logWS = null;
                //ak mam zapnute logovanie sprav, tak si naplnim zoznam volanim
                if (Tools.ShouldLogXmlMessages)
                {
                    logWS = new SIS.Dol.DAVKA.LogWs() { DatumACas = DateTime.Now, NazovWs = "ZrusenieOznaceniaZaujmovejOsobyWS", XmlIn = Ditec.RIS.RFO.Dol.ToolsRFO.GetXmlString(transEnvTypeIn) };
                    SIS.Dol.DAVKA.LogSpracovaniaDavok.GetInstance().LogWsList.Add(logWS);
                }

                var output = Tools.CallService<Dol.ZrusenieOznaceniaZaujmovejOsoby.TransEnvTypeOut>(transEnvTypeIn, Tools.ZrusenieOznaceniaZaujmovejOsoby_URL);

                //zaznacim si vystup
                if (Tools.ShouldLogXmlMessages && output != null)
                {
                    logWS.XmlOut = Ditec.RIS.RFO.Dol.ToolsRFO.GetXmlString(output);
                    if (output != null && output.VSP != null)
                        logWS.NavratovyKod = output.VSP.KI;

                    //zmazem zaznam, ktorym som si stringovy response zapisoval
                    SIS.Dol.DAVKA.LogSpracovaniaDavok.GetInstance().LogWsList.RemoveAll(item => item.NazovWs == "CallRfoService");
                }

                return output;
            }
            catch (Exception ex)
            {
                ExceptionHandling.HandleBusinessException(this, ex, 006, MethodInfo.GetCurrentMethod().Name);
                throw ex;
            }

            return null;
        }

        public Dol.ZapisNovychOsobWS.TransEnvTypeOut ZapisNovychOsobWSToRfoService(Dol.ZapisNovychOsobWS.TransEnvTypeIn transEnvTypeIn)
        {
            //premenna na pripadne logovanie
            SIS.Dol.DAVKA.LogWs logWS = null;

            try
            {
                //ak mam zapnute logovanie sprav, tak si naplnim zoznam volanim
                if (Tools.ShouldLogXmlMessages)
                {
                    logWS = new SIS.Dol.DAVKA.LogWs() { DatumACas = DateTime.Now, NazovWs = "ZapisNovychOsobWS", XmlIn = Ditec.RIS.RFO.Dol.ToolsRFO.GetXmlString(transEnvTypeIn) };
                    SIS.Dol.DAVKA.LogSpracovaniaDavok.GetInstance().LogWsList.Add(logWS);
                }

                //naplni PodanieType
                var podanieType = new ZapisNovychOsobWS.PodanieType()
                {
                    IdentifikatorSchranky = Tools.RisUser,
                    TypPodania = Tools.ZapisNovychOsob_TypPodania,
                    TypSluzby = Tools.ZapisNovychOsob_TypSluzby,
                    DataPodaniaBase64 = Tools.GetRegistrationToBase64String(
                        transEnvTypeIn,
                        Tools.ZapisNovychOsob_RegistrationId,
                        Tools.ZapisNovychOsob_DocumentUnauthorizedId,
                        Tools.ZapisNovychOsob_ObjectIdentifier)
                };


                ZapisNovychOsobWS.ZasielkaType zasielkaType = null;
                //volanie RFO rozhrania
                using (var client = new ZapisNovychOsobWS.ZapisNovychOsob_v1_0_ep())
                {
                    client.Url = Tools.ZapisNovychOsob_URL;
                    client.Credentials = new System.Net.NetworkCredential(Tools.IFO_User, Tools.IFO_Password);
                    client.PreAuthenticate = true;
                    zasielkaType = client.process(podanieType);

                    //zaznacim si vystup
                    if (Tools.ShouldLogXmlMessages)
                    {
                        if (zasielkaType != null)
                            logWS.XmlOut = Ditec.RIS.RFO.Dol.ToolsRFO.GetXmlString(zasielkaType);
                        else
                            logWS.XmlOut = "client.process vratil null.";
                    }
                }
                //vrati objekt z base64
                var output = Tools.GetTransEnvTypeOutFromBase64String<Dol.ZapisNovychOsobWS.TransEnvTypeOut>(zasielkaType.DataZasielkyBase64);

                //zaznacim si vystup
                if (Tools.ShouldLogXmlMessages && output != null)
                {
                    logWS.XmlOut = Ditec.RIS.RFO.Dol.ToolsRFO.GetXmlString(output);
                    if (output != null && output.VSP != null)
                        logWS.NavratovyKod = output.VSP.KI;
                }

                return output;
            }
            catch (Exception ex)
            {
                //zaznacim si vystup
                if (Tools.ShouldLogXmlMessages)
                {
                    if (logWS == null)
                    {
                        logWS = new SIS.Dol.DAVKA.LogWs() { DatumACas = DateTime.Now, NazovWs = "ZapisNovychOsob", XmlIn = "Chyba" };
                        SIS.Dol.DAVKA.LogSpracovaniaDavok.GetInstance().LogWsList.Add(logWS);
                    }
                    logWS.XmlOut = Ditec.RIS.RFO.Dol.ToolsRFO.GetXmlString(ex.Message);
                }
                ExceptionHandling.HandleBusinessException(this, ex, 004, MethodInfo.GetCurrentMethod().Name);

                throw ex;
            }
        }

        public Dol.ZapisUpravyMenaWS.TransEnvTypeOut ZapisUpravyMenaWSToRfoService(Dol.ZapisUpravyMenaWS.TransEnvTypeIn transEnvTypeIn)
        {
            //premenna na pripadne logovanie
            SIS.Dol.DAVKA.LogWs logWS = null;

            try
            {
                //ak mam zapnute logovanie sprav, tak si naplnim zoznam volanim
                if (Tools.ShouldLogXmlMessages)
                {
                    logWS = new SIS.Dol.DAVKA.LogWs() { DatumACas = DateTime.Now, NazovWs = "ZapisUpravyMenaWS", XmlIn = Ditec.RIS.RFO.Dol.ToolsRFO.GetXmlString(transEnvTypeIn) };
                    SIS.Dol.DAVKA.LogSpracovaniaDavok.GetInstance().LogWsList.Add(logWS);
                }

                //naplni PodanieType
                var podanieType = new ZapisUpravyMenaWeb.PodanieType()
                {
                    IdentifikatorSchranky = Tools.RisUser,
                    TypPodania = Tools.AktualizaciaOsob_TypPodania,
                    TypSluzby = Tools.AktualizaciaOsob_TypSluzby,
                    DataPodaniaBase64 = Tools.GetRegistrationToBase64String(
                        transEnvTypeIn,
                        Tools.AktualizaciaOsob_RegistrationId,
                        Tools.AktualizaciaOsob_DocumentUnauthorizedId,
                        Tools.ZapisUpravyMena_ObjectIdentifier)
                };


                ZapisUpravyMenaWeb.ZasielkaType zasielkaType = null;
                //volanie RFO rozhrania
                using (var client = new ZapisUpravyMenaWeb.ZapisUpravyMena_v1_0_ep())
                {
                    client.Url = Tools.ZapisUpravyMena_URL;
                    client.Credentials = new System.Net.NetworkCredential(Tools.IFO_User, Tools.IFO_Password);
                    client.PreAuthenticate = true;
                    zasielkaType = client.process(podanieType);

                    //zaznacim si vystup
                    if (Tools.ShouldLogXmlMessages)
                    {
                        if (zasielkaType != null)
                            logWS.XmlOut = Ditec.RIS.RFO.Dol.ToolsRFO.GetXmlString(zasielkaType);
                        else
                            logWS.XmlOut = "client.process vratil null.";
                    }
                }
                //vrati objekt z base64
                var output = Tools.GetTransEnvTypeOutFromBase64String<Dol.ZapisUpravyMenaWS.TransEnvTypeOut>(zasielkaType.DataZasielkyBase64);

                //zaznacim si vystup
                if (Tools.ShouldLogXmlMessages && output != null)
                {
                    logWS.XmlOut = Ditec.RIS.RFO.Dol.ToolsRFO.GetXmlString(output);
                    if (output != null && output.VSP != null)
                        logWS.NavratovyKod = output.VSP.KI;
                }

                return output;
            }
            catch (Exception ex)
            {
                //zaznacim si vystup
                if (Tools.ShouldLogXmlMessages)
                {
                    if (logWS == null)
                    {
                        logWS = new SIS.Dol.DAVKA.LogWs() { DatumACas = DateTime.Now, NazovWs = "ZapisUpravyMenaWS", XmlIn = "Chyba" };
                        SIS.Dol.DAVKA.LogSpracovaniaDavok.GetInstance().LogWsList.Add(logWS);
                    }
                    logWS.XmlOut = Ditec.RIS.RFO.Dol.ToolsRFO.GetXmlString(ex.Message);
                }
                ExceptionHandling.HandleBusinessException(this, ex, 004, MethodInfo.GetCurrentMethod().Name);

                throw ex;
            }
        }

        public Dol.ZapisUpravyPriezviskaWS.TransEnvTypeOut ZapisUpravyPriezviskaWSToRfoService(Dol.ZapisUpravyPriezviskaWS.TransEnvTypeIn transEnvTypeIn)
        {
            //premenna na pripadne logovanie
            SIS.Dol.DAVKA.LogWs logWS = null;

            try
            {
                //ak mam zapnute logovanie sprav, tak si naplnim zoznam volanim
                if (Tools.ShouldLogXmlMessages)
                {
                    logWS = new SIS.Dol.DAVKA.LogWs() { DatumACas = DateTime.Now, NazovWs = "ZapisUpravyPriezviskaWS", XmlIn = Ditec.RIS.RFO.Dol.ToolsRFO.GetXmlString(transEnvTypeIn) };
                    SIS.Dol.DAVKA.LogSpracovaniaDavok.GetInstance().LogWsList.Add(logWS);
                }

                //naplni PodanieType
                var podanieType = new ZapisUpravyPriezviskaWeb.PodanieType()
                {
                    IdentifikatorSchranky = Tools.RisUser,
                    TypPodania = Tools.AktualizaciaOsob_TypPodania,
                    TypSluzby = Tools.AktualizaciaOsob_TypSluzby,
                    DataPodaniaBase64 = Tools.GetRegistrationToBase64String(
                        transEnvTypeIn,
                        Tools.AktualizaciaOsob_RegistrationId,
                        Tools.AktualizaciaOsob_DocumentUnauthorizedId,
                        Tools.ZapisUpravyPriezviska_ObjectIdentifier)
                };


                ZapisUpravyPriezviskaWeb.ZasielkaType zasielkaType = null;
                //volanie RFO rozhrania
                using (var client = new ZapisUpravyPriezviskaWeb.ZapisUpravyPriezviska_v1_0_ep())
                {
                    client.Url = Tools.ZapisUpravyPriezviska_URL;
                    client.Credentials = new System.Net.NetworkCredential(Tools.IFO_User, Tools.IFO_Password);
                    client.PreAuthenticate = true;
                    zasielkaType = client.process(podanieType);

                    //zaznacim si vystup
                    if (Tools.ShouldLogXmlMessages)
                    {
                        if (zasielkaType != null)
                            logWS.XmlOut = Ditec.RIS.RFO.Dol.ToolsRFO.GetXmlString(zasielkaType);
                        else
                            logWS.XmlOut = "client.process vratil null.";
                    }
                }
                //vrati objekt z base64
                var output = Tools.GetTransEnvTypeOutFromBase64String<Dol.ZapisUpravyPriezviskaWS.TransEnvTypeOut>(zasielkaType.DataZasielkyBase64);

                //zaznacim si vystup
                if (Tools.ShouldLogXmlMessages && output != null)
                {
                    logWS.XmlOut = Ditec.RIS.RFO.Dol.ToolsRFO.GetXmlString(output);
                    if (output != null && output.VSP != null)
                        logWS.NavratovyKod = output.VSP.KI;
                }

                return output;
            }
            catch (Exception ex)
            {
                //zaznacim si vystup
                if (Tools.ShouldLogXmlMessages)
                {
                    if (logWS == null)
                    {
                        logWS = new SIS.Dol.DAVKA.LogWs() { DatumACas = DateTime.Now, NazovWs = "ZapisUpravyPriezviskaWS", XmlIn = "Chyba" };
                        SIS.Dol.DAVKA.LogSpracovaniaDavok.GetInstance().LogWsList.Add(logWS);
                    }
                    logWS.XmlOut = Ditec.RIS.RFO.Dol.ToolsRFO.GetXmlString(ex.Message);
                }
                ExceptionHandling.HandleBusinessException(this, ex, 005, MethodInfo.GetCurrentMethod().Name);

                throw ex;
            }
        }
	}
}
