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
	/// Trieda obsahujuca volania informacnych sluzieb, t.j. sluzieb, ktorych vysledkom je poskytnutie udajov registra. 
	/// </summary>
    public class InformationalWS : BusinessRulesBase
	{
        public Dol.PoskytnutieCiselnikovWS.TransEnvTypeOut PoskytnutieCiselnikovWSRequestToRfoService(Dol.PoskytnutieCiselnikovWS.TransEnvTypeIn transEnvTypeIn)
        {
            try
            {
                //premenna na pripadne logovanie
                SIS.Dol.DAVKA.LogWs logWS = null;
                //ak mam zapnute logovanie sprav, tak si naplnim zoznam volanim
                if (Tools.ShouldLogXmlMessages)
                {
                    logWS = new SIS.Dol.DAVKA.LogWs() { DatumACas = DateTime.Now, NazovWs = "PoskytnutieCiselnikovWS", XmlIn = Ditec.RIS.RFO.Dol.ToolsRFO.GetXmlString(transEnvTypeIn) };
                    SIS.Dol.DAVKA.LogSpracovaniaDavok.GetInstance().LogWsList.Add(logWS);
                }

                var output = Tools.CallService<Dol.PoskytnutieCiselnikovWS.TransEnvTypeOut>(transEnvTypeIn, Tools.PoskytnutieCiselnikov_URL);

                //zaznacim si vystup
                if (Tools.ShouldLogXmlMessages)
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
                ExceptionHandling.HandleBusinessException(this, ex, 001, MethodInfo.GetCurrentMethod().Name + ": " + ex.ToString());
            }

            return null;
        }

        public Dol.PoskytnutieZoznamuIFOPodlaVyhladavacichKriteriiWS.TransEnvTypeOut PoskytnutieZoznamuIFOPodlaVyhladavacichKriteriiWSRequestToRfoService(Dol.PoskytnutieZoznamuIFOPodlaVyhladavacichKriteriiWS.TransEnvTypeIn transEnvTypeIn)
        {
            //premenna na pripadne logovanie
            SIS.Dol.DAVKA.LogWs logWS = null;
            try
            {

                //ak mam zapnute logovanie sprav, tak si naplnim zoznam volanim
                if (Tools.ShouldLogXmlMessages)
                {
                    logWS = new SIS.Dol.DAVKA.LogWs() { DatumACas = DateTime.Now, NazovWs = "PoskytnutieZoznamuIFOPodlaVyhladavacichKriterii", XmlIn = Ditec.RIS.RFO.Dol.ToolsRFO.GetXmlString(transEnvTypeIn) };
                    SIS.Dol.DAVKA.LogSpracovaniaDavok.GetInstance().LogWsList.Add(logWS);
                }


                //naplni PodanieType
                var podanieTypePreList = new PodanieType()
                {
                    IdentifikatorSchranky = Tools.RisUser,
                    TypPodania = Tools.PoskytnutieZoznamuIfoPodlaVyhladavacichKriterii_TypPodania,
                    TypSluzby = Tools.PoskytnutieZoznamuIfoPodlaVyhladavacichKriterii_TypSluzby,
                    DataPodaniaBase64 = Tools.GetRegistrationToBase64String(
                        transEnvTypeIn,
                        Tools.PoskytnutieZoznamuIfoPodlaVyhladavacichKriterii_RegistrationId,
                        Tools.PoskytnutieZoznamuIfoPodlaVyhladavacichKriterii_DocumentUnauthorizedId,
                        Tools.PoskytnutieZoznamuIfoPodlaVyhladavacichKriterii_ObjectIdentifier)
                };

                //var ar = Convert.FromBase64String(podanieTypePreList.DataPodaniaBase64);
                //var xDoc = new System.Xml.XmlDocument();
                //xDoc.LoadXml(Encoding.UTF8.GetString(ar));
                //xDoc.Save(@"d:\Projects\RIS2\Trunk\Code\RIS2\Server\RIS.ProcessStarter\bin\Debug\Spravy\Kriteria.xml");


                //Volanie RFO rozhrania
                ZasielkaType zasielkaTypePreList = null;
                using (var client = new PoskytnutieZoznamuIFOPodlaVyhladavacichKriteriiWeb.PoskytnutieZoznamuIFOPodlaVyhladavacichKriterii_v1_0_ep())
                {
                    client.Url = Tools.PoskytnutieZoznamuIfoPodlaVyhladavacichKriterii_URL;
                    client.Credentials = new System.Net.NetworkCredential(Tools.IFO_User,
                        Tools.IFO_Password);
                    client.PreAuthenticate = true;
                    zasielkaTypePreList = client.process(podanieTypePreList);

                    if (Tools.ShouldLogXmlMessages)
                    {
                        if (zasielkaTypePreList != null)
                            logWS.XmlOut = Ditec.RIS.RFO.Dol.ToolsRFO.GetXmlString(zasielkaTypePreList);
                        else
                            logWS.XmlOut = "client.process vratil null.";
                    }
                }

                //vrati objekt z base64
                var output = Tools.GetTransEnvTypeOutFromBase64String<Dol.PoskytnutieZoznamuIFOPodlaVyhladavacichKriteriiWS.TransEnvTypeOut>(zasielkaTypePreList.DataZasielkyBase64);

                //zaznacim si vystup
                if (Tools.ShouldLogXmlMessages && output != null)
                {
                    logWS.XmlOut = Ditec.RIS.RFO.Dol.ToolsRFO.GetXmlString(output);
                    if (output != null && output.POV != null)
                        logWS.NavratovyKod = output.POV.KO;
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
                        logWS = new SIS.Dol.DAVKA.LogWs() { DatumACas = DateTime.Now, NazovWs = "PoskytnutieZoznamuIFOPodlaVyhladavacichKriterii", XmlIn = "Chyba" };
                        SIS.Dol.DAVKA.LogSpracovaniaDavok.GetInstance().LogWsList.Add(logWS);
                    }
                    logWS.XmlOut = Ditec.RIS.RFO.Dol.ToolsRFO.GetXmlString(ex.Message);
                }

                throw ex;
            }
        }

        public Dol.PoskytnutieUdajovIFOOnlineWS.TransEnvTypeOut PoskytnutieReferencnychUdajovZoznamuIFOOnlineWSRequestToRfoService(Dol.PoskytnutieUdajovIFOOnlineWS.TransEnvTypeIn transEnvTypeIn)
        {
            //premenna na pripadne logovanie
            SIS.Dol.DAVKA.LogWs logWS = null;

            try
            {
                //ak mam zapnute logovanie sprav, tak si naplnim zoznam volanim
                if (Tools.ShouldLogXmlMessages)
                {
                    logWS = new SIS.Dol.DAVKA.LogWs() { DatumACas = DateTime.Now, NazovWs = "PoskytnutieUdajovIFOOnline", XmlIn = Ditec.RIS.RFO.Dol.ToolsRFO.GetXmlString(transEnvTypeIn) };
                    SIS.Dol.DAVKA.LogSpracovaniaDavok.GetInstance().LogWsList.Add(logWS);
                }

                //naplni PodanieType
                var podanieType = new PoskytnutieUdajovIFOOnlineWeb.PodanieType()
                {
                    IdentifikatorSchranky = Tools.RisUser,
                    TypPodania = Tools.PoskytnutieUdajovIFOOnline_TypPodania,
                    TypSluzby = Tools.PoskytnutieUdajovIFOOnline_TypSluzby,
                    DataPodaniaBase64 = Tools.GetRegistrationToBase64String(
                        transEnvTypeIn,
                        Tools.PoskytnutieUdajovIFOOnline_RegistrationId,
                        Tools.PoskytnutieUdajovIFOOnline_DocumentUnauthorizedId,
                        Tools.PoskytnutieUdajovIFOOnline_ObjectIdentifier)
                };


                PoskytnutieUdajovIFOOnlineWeb.ZasielkaType zasielkaType = null;
                //volanie RFO rozhrania
                using (var client = new PoskytnutieUdajovIFOOnlineWeb.PoskytnutieUdajovIFOOnline_v1_0_ep())
                {
                    client.Url = Tools.PoskytnutieUdajovIFOOnline_URL;
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
                var output = Tools.GetTransEnvTypeOutFromBase64String<Dol.PoskytnutieUdajovIFOOnlineWS.TransEnvTypeOut>(zasielkaType.DataZasielkyBase64);

                //zaznacim si vystup
                if (Tools.ShouldLogXmlMessages && output != null)
                {
                    logWS.XmlOut = Ditec.RIS.RFO.Dol.ToolsRFO.GetXmlString(output);
                    if (output != null && output.POV != null)
                        logWS.NavratovyKod = output.POV.KO;
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
                        logWS = new SIS.Dol.DAVKA.LogWs() { DatumACas = DateTime.Now, NazovWs = "PoskytnutieUdajovIFOOnline", XmlIn = "Chyba" };
                        SIS.Dol.DAVKA.LogSpracovaniaDavok.GetInstance().LogWsList.Add(logWS);
                    }
                    logWS.XmlOut = Ditec.RIS.RFO.Dol.ToolsRFO.GetXmlString(ex.Message);
                }
                ExceptionHandling.HandleBusinessException(this, ex, 003, MethodInfo.GetCurrentMethod().Name + ": " + ex.ToString());

                throw ex;
            }

            return null;
        }

        public Dol.ZoznamIFOSoZmenenymiReferencnymiUdajmiWS.TransEnvTypeOut PoskytnutieZoznamuIFOSoZmenenymiReferencnymiUdajmiWSRequestToRfoService(Dol.ZoznamIFOSoZmenenymiReferencnymiUdajmiWS.TransEnvTypeIn transEnvTypeIn)
        {
            try
            {
                //premenna na pripadne logovanie
                SIS.Dol.DAVKA.LogWs logWS = null;
                //ak mam zapnute logovanie sprav, tak si naplnim zoznam volanim
                if (Tools.ShouldLogXmlMessages)
                {
                    logWS = new SIS.Dol.DAVKA.LogWs() { DatumACas = DateTime.Now, NazovWs = "ZoznamIFOSoZmenenymiReferencnymiUdajmiWS", XmlIn = Ditec.RIS.RFO.Dol.ToolsRFO.GetXmlString(transEnvTypeIn) };
                    SIS.Dol.DAVKA.LogSpracovaniaDavok.GetInstance().LogWsList.Add(logWS);
                }

                var output = Tools.CallService<Dol.ZoznamIFOSoZmenenymiReferencnymiUdajmiWS.TransEnvTypeOut>(transEnvTypeIn, Tools.PoskytnutieZoznamuIFOSoZmenenymiReferencnymiUdajmi_URL);

                //zaznacim si vystup
                if (Tools.ShouldLogXmlMessages)
                {
                    logWS.XmlOut = Ditec.RIS.RFO.Dol.ToolsRFO.GetXmlString(output);
                    if (output != null && output.POV != null)
                        logWS.NavratovyKod = output.POV.KO;

                    //zmazem zaznam, ktorym som si stringovy response zapisoval
                    SIS.Dol.DAVKA.LogSpracovaniaDavok.GetInstance().LogWsList.RemoveAll(item => item.NazovWs == "CallRfoService");
                }

                return output;
            }
            catch (Exception ex)
            {
                ExceptionHandling.HandleBusinessException(this, ex, 004, MethodInfo.GetCurrentMethod().Name + ": " + ex.ToString());
            }

            return null;
        }

        public Dol.PotvrdzovaniePrijatiaZmienWS.TransEnvTypeOut PotvrdzovaniePrijatiaZmienWSToRfoService(Dol.PotvrdzovaniePrijatiaZmienWS.TransEnvTypeIn transEnvTypeIn)
        {
            try
            {
                //premenna na pripadne logovanie
                SIS.Dol.DAVKA.LogWs logWS = null;

                //ak mam zapnute logovanie sprav, tak si naplnim zoznam volanim
                if (Tools.ShouldLogXmlMessages)
                {
                    logWS = new SIS.Dol.DAVKA.LogWs() { DatumACas = DateTime.Now, NazovWs = "PotvrdzovaniePrijatiaZmienWS", XmlIn = Ditec.RIS.RFO.Dol.ToolsRFO.GetXmlString(transEnvTypeIn) };
                    SIS.Dol.DAVKA.LogSpracovaniaDavok.GetInstance().LogWsList.Add(logWS);
                }

                var output = Tools.CallService<Dol.PotvrdzovaniePrijatiaZmienWS.TransEnvTypeOut>(transEnvTypeIn, Tools.PotvrdzovaniePrijatiaZmienWS_URL);

                //zaznacim si vystup
                if (Tools.ShouldLogXmlMessages)
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
                ExceptionHandling.HandleBusinessException(this, ex, 007, MethodInfo.GetCurrentMethod().Name + ": " + ex.ToString());
            }

            return null;
        }

	}
}
