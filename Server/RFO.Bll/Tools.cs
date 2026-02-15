using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;
using Ditec.RIS.RFO.Dol;
using Ditec.SysFra.Infrastructure.Impl;

namespace Ditec.RIS.RFO.Bll
{
    public static class Tools
    {
        #region Properties

        #region PoskytnutieCiselnikovWS

        public static string PoskytnutieCiselnikov_URL
        {
            get
            {
                return ConfigurationManager.AppSettings["PoskytnutieCiselnikov_URL"] != null ? ConfigurationManager.AppSettings["PoskytnutieCiselnikov_URL"] : String.Empty;
            }
        }

        public static string ZDUser
        {
            get
            {
                return ConfigurationManager.AppSettings["ZDUser"] != null ? ConfigurationManager.AppSettings["ZDUser"] : String.Empty;
            }
        }

        public static string ZDPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["ZDPassword"] != null ? ConfigurationManager.AppSettings["ZDPassword"] : String.Empty;
            }
        }

        #endregion PoskytnutieCiselnikovWS
        #region PoskytnutieZoznamuIFOPodlaVyhladavacichKriteriiWS

        public static string RisUser
        {
            get
            {
                return ConfigurationManager.AppSettings["RisUser"] != null ? ConfigurationManager.AppSettings["RisUser"] : String.Empty;
            }
        }
        public static string PoskytnutieZoznamuIfoPodlaVyhladavacichKriterii_TypPodania
        {
            get
            {
                return ConfigurationManager.AppSettings["PoskytnutieZoznamuIfoPodlaVyhladavacichKriterii_TypPodania"] != null ? ConfigurationManager.AppSettings["PoskytnutieZoznamuIfoPodlaVyhladavacichKriterii_TypPodania"] : String.Empty;
            }
        }
        public static string PoskytnutieZoznamuIfoPodlaVyhladavacichKriterii_TypSluzby
        {
            get
            {
                return ConfigurationManager.AppSettings["PoskytnutieZoznamuIfoPodlaVyhladavacichKriterii_TypSluzby"] != null ? ConfigurationManager.AppSettings["PoskytnutieZoznamuIfoPodlaVyhladavacichKriterii_TypSluzby"] : String.Empty;
            }
        }
        public static string PoskytnutieZoznamuIfoPodlaVyhladavacichKriterii_RegistrationId
        {
            get
            {
                return ConfigurationManager.AppSettings["PoskytnutieZoznamuIfoPodlaVyhladavacichKriterii_RegistrationId"] != null ? ConfigurationManager.AppSettings["PoskytnutieZoznamuIfoPodlaVyhladavacichKriterii_RegistrationId"] : String.Empty;
            }
        }
        public static string PoskytnutieZoznamuIfoPodlaVyhladavacichKriterii_DocumentUnauthorizedId
        {
            get
            {
                return ConfigurationManager.AppSettings["PoskytnutieZoznamuIfoPodlaVyhladavacichKriterii_DocumentUnauthorizedId"] != null ? ConfigurationManager.AppSettings["PoskytnutieZoznamuIfoPodlaVyhladavacichKriterii_DocumentUnauthorizedId"] : String.Empty;
            }
        }
        public static string PoskytnutieZoznamuIfoPodlaVyhladavacichKriterii_ObjectIdentifier
        {
            get
            {
                return ConfigurationManager.AppSettings["PoskytnutieZoznamuIfoPodlaVyhladavacichKriterii_ObjectIdentifier"] != null ? ConfigurationManager.AppSettings["PoskytnutieZoznamuIfoPodlaVyhladavacichKriterii_ObjectIdentifier"] : String.Empty;
            }
        }
        public static string PoskytnutieZoznamuIfoPodlaVyhladavacichKriterii_URL
        {
            get
            {
                return ConfigurationManager.AppSettings["PoskytnutieZoznamuIfoPodlaVyhladavacichKriterii_URL"] != null ? ConfigurationManager.AppSettings["PoskytnutieZoznamuIfoPodlaVyhladavacichKriterii_URL"] : String.Empty;
            }
        }
        public static string IFO_User
        {
            get
            {
                return ConfigurationManager.AppSettings["IFO_User"] != null ? ConfigurationManager.AppSettings["IFO_User"] : String.Empty;
            }
        }
        public static string IFO_Password
        {
            get
            {
                return ConfigurationManager.AppSettings["IFO_Password"] != null ? ConfigurationManager.AppSettings["IFO_Password"] : String.Empty;
            }
        }

        #endregion PoskytnutieZoznamuIFOPodlaVyhladavacichKriteriiWS
        #region PoskytnutieUdajovIFOOnlineWS

        public static string PoskytnutieUdajovIFOOnline_TypPodania
        {
            get
            {
                return ConfigurationManager.AppSettings["PoskytnutieUdajovIFOOnline_TypPodania"] != null ? ConfigurationManager.AppSettings["PoskytnutieUdajovIFOOnline_TypPodania"] : String.Empty;
            }
        }
        public static string PoskytnutieUdajovIFOOnline_TypSluzby
        {
            get
            {
                return ConfigurationManager.AppSettings["PoskytnutieUdajovIFOOnline_TypSluzby"] != null ? ConfigurationManager.AppSettings["PoskytnutieUdajovIFOOnline_TypSluzby"] : String.Empty;
            }
        }
        public static string PoskytnutieUdajovIFOOnline_RegistrationId
        {
            get
            {
                return ConfigurationManager.AppSettings["PoskytnutieUdajovIFOOnline_RegistrationId"] != null ? ConfigurationManager.AppSettings["PoskytnutieUdajovIFOOnline_RegistrationId"] : String.Empty;
            }
        }
        public static string PoskytnutieUdajovIFOOnline_DocumentUnauthorizedId
        {
            get
            {
                return ConfigurationManager.AppSettings["PoskytnutieUdajovIFOOnline_DocumentUnauthorizedId"] != null ? ConfigurationManager.AppSettings["PoskytnutieUdajovIFOOnline_DocumentUnauthorizedId"] : String.Empty;
            }
        }
        public static string PoskytnutieUdajovIFOOnline_ObjectIdentifier
        {
            get
            {
                return ConfigurationManager.AppSettings["PoskytnutieUdajovIFOOnline_ObjectIdentifier"] != null ? ConfigurationManager.AppSettings["PoskytnutieUdajovIFOOnline_ObjectIdentifier"] : String.Empty;
            }
        }
        public static string PoskytnutieUdajovIFOOnline_URL
        {
            get
            {
                return ConfigurationManager.AppSettings["PoskytnutieUdajovIFOOnline_URL"] != null ? ConfigurationManager.AppSettings["PoskytnutieUdajovIFOOnline_URL"] : String.Empty;
            }
        }

        #endregion PoskytnutieUdajovIFOOnlineWS
        #region PoskytnutieZoznamuIFOSoZmenenymiReferencnymiUdajmiWS
        public static string PoskytnutieZoznamuIFOSoZmenenymiReferencnymiUdajmi_URL
        {
            get
            {
                return ConfigurationManager.AppSettings["PoskytnutieZoznamuIFOSoZmenenymiReferencnymiUdajmi_URL"] != null ? ConfigurationManager.AppSettings["PoskytnutieZoznamuIFOSoZmenenymiReferencnymiUdajmi_URL"] : String.Empty;
            }
        }

        #endregion PoskytnutieZoznamuIFOSoZmenenymiReferencnymiUdajmiWS
        #region OznacenieZaujmovejOsobyWS
        public static string OznacenieZaujmovejOsoby_URL
        {
            get
            {
                return ConfigurationManager.AppSettings["OznacenieZaujmovejOsoby_URL"] != null ? ConfigurationManager.AppSettings["OznacenieZaujmovejOsoby_URL"] : String.Empty;
            }
        }

        #endregion OznacenieZaujmovejOsobyWS
        #region ZrusenieOznaceniaZaujmovejOsobyWS

        public static string ZrusenieOznaceniaZaujmovejOsoby_URL
        {
            get
            {
                return ConfigurationManager.AppSettings["ZrusenieOznaceniaZaujmovejOsoby_URL"] != null ? ConfigurationManager.AppSettings["ZrusenieOznaceniaZaujmovejOsoby_URL"] : String.Empty;
            }
        }

        #endregion ZrusenieOznaceniaZaujmovejOsobyWS
        #region PotvrdzovaniePrijatiaZmienWS

        public static string PotvrdzovaniePrijatiaZmienWS_URL
        {
            get
            {
                return ConfigurationManager.AppSettings["PotvrdzovaniePrijatiaZmienWS_URL"] != null ? ConfigurationManager.AppSettings["PotvrdzovaniePrijatiaZmienWS_URL"] : String.Empty;
            }
        }

        #endregion PotvrdzovaniePrijatiaZmienWS
        #region ZapisNovychOsobWS

        public static string ZapisNovychOsob_TypPodania
        {
            get
            {
                return ConfigurationManager.AppSettings["ZapisNovychOsob_TypPodania"] != null ? ConfigurationManager.AppSettings["ZapisNovychOsob_TypPodania"] : String.Empty;
            }
        }
        public static string ZapisNovychOsob_TypSluzby
        {
            get
            {
                return ConfigurationManager.AppSettings["ZapisNovychOsob_TypSluzby"] != null ? ConfigurationManager.AppSettings["ZapisNovychOsob_TypSluzby"] : String.Empty;
            }
        }
        public static string ZapisNovychOsob_RegistrationId
        {
            get
            {
                return ConfigurationManager.AppSettings["ZapisNovychOsob_RegistrationId"] != null ? ConfigurationManager.AppSettings["ZapisNovychOsob_RegistrationId"] : String.Empty;
            }
        }
        public static string ZapisNovychOsob_DocumentUnauthorizedId
        {
            get
            {
                return ConfigurationManager.AppSettings["ZapisNovychOsob_DocumentUnauthorizedId"] != null ? ConfigurationManager.AppSettings["ZapisNovychOsob_DocumentUnauthorizedId"] : String.Empty;
            }
        }
        public static string ZapisNovychOsob_ObjectIdentifier
        {
            get
            {
                return ConfigurationManager.AppSettings["ZapisNovychOsob_ObjectIdentifier"] != null ? ConfigurationManager.AppSettings["ZapisNovychOsob_ObjectIdentifier"] : String.Empty;
            }
        }
        public static string ZapisNovychOsob_URL
        {
            get
            {
                return ConfigurationManager.AppSettings["ZapisNovychOsob_URL"] != null ? ConfigurationManager.AppSettings["ZapisNovychOsob_URL"] : String.Empty;
            }
        }

        #endregion ZapisNovychOsobWS
        #region AktualizaciaOsobWS

        public static string AktualizaciaOsob_TypPodania
        {
            get
            {
                return ConfigurationManager.AppSettings["AktualizaciaOsob_TypPodania"] != null ? ConfigurationManager.AppSettings["AktualizaciaOsob_TypPodania"] : String.Empty;
            }
        }
        public static string AktualizaciaOsob_TypSluzby
        {
            get
            {
                return ConfigurationManager.AppSettings["AktualizaciaOsob_TypSluzby"] != null ? ConfigurationManager.AppSettings["AktualizaciaOsob_TypSluzby"] : String.Empty;
            }
        }
        public static string AktualizaciaOsob_RegistrationId
        {
            get
            {
                return ConfigurationManager.AppSettings["AktualizaciaOsob_RegistrationId"] != null ? ConfigurationManager.AppSettings["AktualizaciaOsob_RegistrationId"] : String.Empty;
            }
        }
        public static string AktualizaciaOsob_DocumentUnauthorizedId
        {
            get
            {
                return ConfigurationManager.AppSettings["AktualizaciaOsob_DocumentUnauthorizedId"] != null ? ConfigurationManager.AppSettings["AktualizaciaOsob_DocumentUnauthorizedId"] : String.Empty;
            }
        }

        #region ZapisUpravyMenaWS

        public static string ZapisUpravyMena_ObjectIdentifier
        {
            get
            {
                return ConfigurationManager.AppSettings["ZapisUpravyMena_ObjectIdentifier"] != null ? ConfigurationManager.AppSettings["ZapisUpravyMena_ObjectIdentifier"] : String.Empty;
            }
        }
        public static string ZapisUpravyMena_URL
        {
            get
            {
                return ConfigurationManager.AppSettings["ZapisUpravyMena_URL"] != null ? ConfigurationManager.AppSettings["ZapisUpravyMena_URL"] : String.Empty;
            }
        }
        #endregion ZapisUpravyMenaWS
        #region ZapisUpravyPriezviskaWS

        public static string ZapisUpravyPriezviska_ObjectIdentifier
        {
            get
            {
                return ConfigurationManager.AppSettings["ZapisUpravyPriezviska_ObjectIdentifier"] != null ? ConfigurationManager.AppSettings["ZapisUpravyPriezviska_ObjectIdentifier"] : String.Empty;
            }
        }
        public static string ZapisUpravyPriezviska_URL
        {
            get
            {
                return ConfigurationManager.AppSettings["ZapisUpravyPriezviska_URL"] != null ? ConfigurationManager.AppSettings["ZapisUpravyPriezviska_URL"] : String.Empty;
            }
        }
        #endregion ZapisUpravyPriezviskaWS

        #endregion AktualizaciaOsobWS

        /// <summary>
        /// Budu sa logovat requesty a responsy s RFO, hodnota je v konfigu
        /// </summary>
        public static bool ShouldLogXmlMessages
        {
            get
            {
                return ConfigurationManager.AppSettings["ShouldLogXmlMessages"] != null ? Convert.ToBoolean(ConfigurationManager.AppSettings["ShouldLogXmlMessages"]) : false;
            }
        }

        /// <summary>
        /// Bude sa zapisovat osoba aj pre tie, ktore presli OK, hodnota je v konfigu
        /// </summary>
        public static bool ShouldLogXmlOsoba
        {
            get
            {
                return ConfigurationManager.AppSettings["ShouldLogXmlOsoba"] != null ? Convert.ToBoolean(ConfigurationManager.AppSettings["ShouldLogXmlOsoba"]) : false;
            }
        }

        /// <summary>
        /// True, ak ide o vyvojove prostredie. Hodnota je v konfigu
        /// </summary>
        public static bool PotvrdzovaniePrijatiaZmien
        {
            get
            {
                return ConfigurationManager.AppSettings["PotvrdzovaniePrijatiaZmien"] != null ? Convert.ToBoolean(ConfigurationManager.AppSettings["PotvrdzovaniePrijatiaZmien"]) : true;
            }
        }

        public static bool OznacenieZaujmovejOsoby
        {
            get
            {
                return ConfigurationManager.AppSettings["OznacenieZaujmovejOsoby"] != null ? Convert.ToBoolean(ConfigurationManager.AppSettings["OznacenieZaujmovejOsoby"]) : true;
            }
        }

        public static string DestinationFolder
        {
            get
            {
                return ConfigurationManager.AppSettings["DestinationFolder"] != null ? ConfigurationManager.AppSettings["DestinationFolder"] : "";
            }
        }

        public static string TargetFolder
        {
            get
            {
                return ConfigurationManager.AppSettings["TargetFolder"] != null ? ConfigurationManager.AppSettings["TargetFolder"] : "";
            }
        }

        #endregion Properties

        /// <summary>
        /// Zosrializuje vstupny objekt, zavola servis so zadanou linkou a vrati vystupny objekt zadaneh otypu
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="transEnvTypeIn"></param>
        /// <param name="URL"></param>
        /// <returns></returns>
        public static T CallService<T>(object transEnvTypeIn, string URL)
        {
            try
            {
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(transEnvTypeIn.GetType());
                var inputBytes = new byte[] { };
                using (var ms = new System.IO.MemoryStream())
                {
                    serializer.Serialize(ms, transEnvTypeIn);
                    ms.Position = 0;
                    inputBytes = ms.ToArray();
                }

                //RFO servis
                var response = Tools.CallRfoService(URL, inputBytes, Tools.ZDUser, Tools.ZDPassword);

                //deserializacia
                var reader = new System.IO.StringReader(response);
                var x = new System.Xml.Serialization.XmlSerializer(typeof(T));
                var output = (T)x.Deserialize(reader);


                return output;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Volanie servisov ORACLE Service Bus (problem pri vygenerovani Service referencie, podobne Web referencie) 
        /// </summary>
        /// <param name="url">url linka</param>
        /// <param name="input">vstup v bytoch</param>
        /// <param name="user">user</param>
        /// <param name="password">password</param>
        /// <returns>response</returns>
        public static string CallRfoService(string url, byte[] input, string user, string password)
        {
            try
            {
                var request = HttpWebRequest.Create(url);
                //POST method
                request.Method = WebRequestMethods.Http.Post;
                if (!String.IsNullOrEmpty(user) || !String.IsNullOrEmpty(password))
                {
                    //ak su potreben meno a heslo pri volani
                    request.Credentials = new NetworkCredential(user, password);
                }

                //musi ist v tvare soap:Envelope
                var envelope = getSoapEnvelopeMessage(input);
                
                request.ContentLength = envelope.Length;
                request.ContentType = "text/xml";
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(envelope, 0, envelope.Length);

                //using(StreamWriter sw = new StreamWriter(@"d:\Projects\RIS2\Trunk\Code\RIS2\Server\RIS.ProcessStarter\bin\Debug\Spravy\TTIMessage.xml"))
                //{
                //    sw.Write(Encoding.UTF8.GetString(envelope));
                //}
                var logWS = new SIS.Dol.DAVKA.LogWs() { DatumACas = DateTime.Now, NazovWs = "CallRfoService", XmlIn = Ditec.RIS.RFO.Dol.ToolsRFO.GetXmlString(envelope) };

                WebResponse response = request.GetResponse();

                var stream = response.GetResponseStream();
                var reader = new StreamReader(stream);

                var result = reader.ReadToEnd();

                //zaznacim si vystup
                if (Tools.ShouldLogXmlMessages)
                {
                    if (String.IsNullOrEmpty(result))
                        logWS.XmlOut = Ditec.RIS.RFO.Dol.ToolsRFO.GetXmlString("Odpoved je prazdny string");
                    else
                        logWS.XmlOut = Ditec.RIS.RFO.Dol.ToolsRFO.GetXmlString(result);
                    SIS.Dol.DAVKA.LogSpracovaniaDavok.GetInstance().LogWsList.Add(logWS);
                }


                stream.Dispose();
                reader.Dispose();

                //System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                //doc.Load(@"h:\RFO\PoskytnutieCiselnikovWS-v1 - XML out.xml");
                //var result = doc.InnerXml;

                return getBodyResponse(result);
            }
            catch (Exception ex)
            {
                //zaznacim si vystup
                if (Tools.ShouldLogXmlMessages)
                {
                    var logWSError = new SIS.Dol.DAVKA.LogWs() { DatumACas = DateTime.Now, NazovWs = "CallRfoService", XmlIn = "Chyba" };
                    logWSError.XmlOut = Ditec.RIS.RFO.Dol.ToolsRFO.GetXmlString(ex.Message);
                    SIS.Dol.DAVKA.LogSpracovaniaDavok.GetInstance().LogWsList.Add(logWSError);
                }

                throw ex;
            }
        }

        /// <summary>
        /// Rozsirena metoda s posielanim inych veci navyse
        /// </summary>
        /// <param name="url"></param>
        /// <param name="input"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string CallRfoServiceExtended(string url, byte[] input, string user, string password)
        {
            try
            {
                var request = HttpWebRequest.Create(url);
                //POST method
                request.Method = WebRequestMethods.Http.Post;
                if (!String.IsNullOrEmpty(user) || !String.IsNullOrEmpty(password))
                {
                    //ak su potreben meno a heslo pri volani
                    request.Credentials = new NetworkCredential(user, password);
                }

                //musi ist v tvare soap:Envelope
                var envelope = getSoapEnvelopeMessageExtended(input);

                request.ContentLength = envelope.Length;
                request.Method = "POST";
                request.ContentType = "text/xml";
				//string auth = Properties.Settings.Default.ZDUser + ":" + Properties.Settings.Default.ZDPassword;
				string auth = "";
                string authBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(auth));

                request.Headers.Add("Authorization: Basic " + authBase64);

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(envelope, 0, envelope.Length);

                WebResponse response = request.GetResponse();
                var stream = response.GetResponseStream();
                var reader = new StreamReader(stream);

                var result = reader.ReadToEnd();
                stream.Dispose();
                reader.Dispose();

                return getBodyResponse(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Vysklada Envelope obalku pre odoslanie
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static byte[] getSoapEnvelopeMessage(byte[] input)
        {
            try
            {
                var element = XElement.Parse(Encoding.UTF8.GetString(input));

                XNamespace soapenv = "http://schemas.xmlsoap.org/soap/envelope/";

                var mydoc = new XDocument(new XElement(soapenv + "Envelope",
                    new XAttribute(XNamespace.Xmlns + "soapenv", "http://schemas.xmlsoap.org/soap/envelope/"),
                    new XElement(soapenv + "Body", new XElement(element))
                    ));

                return Encoding.UTF8.GetBytes(mydoc.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Vysklada Envelope obalku pre odoslanie
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static byte[] getSoapEnvelopeMessageExtended(byte[] input)
        {
            try
            {
                var element = XElement.Parse(Encoding.UTF8.GetString(input));

                XNamespace soapenv = "http://schemas.xmlsoap.org/soap/envelope/";

                var mydoc = new XDocument(new XElement(soapenv + "Envelope",
                    new XAttribute(XNamespace.Xmlns + "s", "http://schemas.xmlsoap.org/soap/envelope/"),
                    new XElement(soapenv + "Body", new XElement(element))
                    ));

                //test - vyhodenie namspacov
                //xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" 
                var doc = mydoc.ToString();
                doc = mydoc.ToString().Replace("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" ", "");
                doc = doc.Replace("<s:Body", "<s:Body xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"");

                return Encoding.UTF8.GetBytes(doc);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private static string getBodyResponse(string response)
        {
            try
            {
                var doc = XDocument.Parse(response);
                XElement body = doc.Descendants().Where(p => p.Name.LocalName == "Body").First();
                return body.FirstNode.ToString();
            }
            catch (Exception ex)
            {
                ExceptionHandling.HandleTechnologicalException("Tools.parseResponse()" + response, new Exception(ex + ",\n Response: " + response), 104);
                throw ex;
            }
        }

        /// <summary>
        /// Podla koda ciselnika sa mi vytvori objekt vsetupnej spravy
        /// </summary>
        /// <param name="KodCiselnika"></param>
        /// <returns></returns>
        public static Dol.PoskytnutieCiselnikovWS.TransEnvTypeIn GetTransEnvTypeIn(string KodCiselnika)
        {
            Dol.PoskytnutieCiselnikovWS.TransEnvTypeIn returnClass = null;

            System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFile(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Ditec.RIS.RFO.Dol.dll"));
            
            //najdem si typ podla zadanej skratky
            var classes = assembly.GetTypes().ToList().FindAll(item => item.Namespace.Equals(typeof(Dol.PoskytnutieCiselnikovWS.TransEnvTypeIn).Namespace));
            Type selectedClass = null;
            //Type selectedParentClass = null;
            //prezriem si kazdu triedu z XSDcka
            foreach(var item in classes)
            {
                if (item.GetProperty("Item") == null)
                    continue;

                var attributes = item.GetProperty("Item").GetCustomAttributes(typeof(System.Xml.Serialization.XmlElementAttribute), false);
                foreach (var attribute in attributes)
                    if (((System.Xml.Serialization.XmlElementAttribute)attribute).ElementName.Equals(KodCiselnika))
                    {
                        returnClass = new Dol.PoskytnutieCiselnikovWS.TransEnvTypeIn();
                        selectedClass = ((System.Xml.Serialization.XmlElementAttribute)attribute).Type;
                        returnClass.Item = GetInstance(item);
                        if (item.UnderlyingSystemType == typeof(Dol.PoskytnutieCiselnikovWS.TPCI))
                        {
                            ((Dol.PoskytnutieCiselnikovWS.TPCI)returnClass.Item).Item = GetInstance(selectedClass);
                            return returnClass;
                        }
                        break;
                    }

                if (selectedClass != null)
                    break;
            }


            if (selectedClass != null)
            {
                returnClass.Item = GetInstance(selectedClass);

            }

            return returnClass;
        }

        /// <summary>
        /// Vytvori sa mi instancia daneho typu
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object GetInstance(Type type)
        {
            return Activator.CreateInstance(type);
        }

        /// <summary>
        /// vrati sa mi samotny zoznam ciselnikov, vyberie sa ten spravny list a len ten sa mi vrati
        /// </summary>
        /// <param name="output"></param>
        /// <param name="KodCiselnika"></param>
        /// <param name="codeList"></param>
        /// <returns></returns>
        public static bool FindCodeListByKodCiselnika(Dol.PoskytnutieCiselnikovWS.TransEnvTypeOut output, string KodCiselnika, ref List<object> codeList)
        {
            if (output == null)
                return false;

            System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFile(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Ditec.RIS.RFO.Dol.dll"));
            
            var properties = output.GetType().GetProperties();
            //prezriem si kazdu property
            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes(typeof(System.Xml.Serialization.XmlArrayItemAttribute), false);
                var attribute = attributes.ToList().Find(item => ((System.Xml.Serialization.XmlArrayItemAttribute)item).ElementName.Equals(MappingKodCiselnikaTransEnvTypeOut(KodCiselnika)));
                if (attribute != null)
                {
                    var list = (IList)property.GetValue(output);

                    //ak sa mi nic nevratilo, tak uz dalej nemusim nacitavat z RFO
                    if (list == null || list.Count == 0)
                        return false;

                    foreach (var item in list)
                    {
                        codeList.Add(item);
                    }

                    //ak sa mi este vratili nejake zaznamy, tak budem nacitavat dalej
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// RFO WS Poskytnutie číselníkov
        /// Premapovanie string kodov zo skratiek ciselnikov na kody pouzite vo vystupnej sprave
        /// </summary>
        /// <param name="KodCiselnika"></param>
        /// <returns></returns>
        private static string MappingKodCiselnikaTransEnvTypeOut(string KodCiselnika)
        {
            switch (KodCiselnika)
            {
                case "POH": return "PEX";
                default: return KodCiselnika;
            }
        }

        /// <summary>
        /// Vrati podla kodu triedu specifikacie pozadovanej informacie
        /// </summary>
        /// <param name="kod"></param>
        /// <returns></returns>
        public static Dol.PoskytnutieUdajovIFOOnlineWS.TSPI_SPN GetTSPI_SPN(int HO)
        {
            switch (HO)
            {
                case 1:
                    return new Dol.PoskytnutieUdajovIFOOnlineWS.TSPI_SPN() { HO = 1, TUDHONA = "Administratívne údaje" };
                case 2:
                    return new Dol.PoskytnutieUdajovIFOOnlineWS.TSPI_SPN() { HO = 2, TUDHONA = "Lokačné údaje" };
                case 3:
                    return new Dol.PoskytnutieUdajovIFOOnlineWS.TSPI_SPN() { HO = 3, TUDHONA = "Vzťahové údaje" };
                case 4:
                    return new Dol.PoskytnutieUdajovIFOOnlineWS.TSPI_SPN() { HO = 4, TUDHONA = "Identifikačné údaje" };
            }

            return null;
        }

        /// <summary>
        /// Naplni sa sprava zadanymi hodnotami
        /// </summary>
        /// <param name="transEnvTypeIn"></param>
        /// <param name="datumOd"></param>
        /// <param name="datumDo"></param>
        /// <param name="PS"></param>
        public static void FillTransEnvTypeIn(ref Dol.PoskytnutieCiselnikovWS.TransEnvTypeIn transEnvTypeIn, DateTime datumOd, DateTime datumDo, int PS)
        {
            if (transEnvTypeIn.Item.GetType() == typeof(Dol.PoskytnutieCiselnikovWS.TPCI))
            {
                ((Dol.PoskytnutieCiselnikovWS.TPCI)transEnvTypeIn.Item).DO = DateTime.Parse(datumOd.ToString("yyyy-MM-ddTHH:mm:sszzz"));
                ((Dol.PoskytnutieCiselnikovWS.TPCI)transEnvTypeIn.Item).DD = DateTime.Parse(datumDo.ToString("yyyy-MM-ddTHH:mm:sszzz"));
                ((Dol.PoskytnutieCiselnikovWS.TPCI)transEnvTypeIn.Item).PS = PS;
            }
        }

        #region PoskytnutieZoznamuIfoPodlaVyhladavacichKriterii

        /// <summary>
        /// Vrati Base64 string pre Registration
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="registrationId"></param>
        /// <param name="documentUnauthorizedId"></param>
        /// <param name="objectIdentifier"></param>
        /// <returns></returns>
        public static string GetRegistrationToBase64String(Dol.PoskytnutieZoznamuIFOPodlaVyhladavacichKriteriiWS.TransEnvTypeIn input, string registrationId, string documentUnauthorizedId, string objectIdentifier)
        {
            try
            {
                //return "PFJlZ2lzdHJhdGlvbiB4bWxucz0iaHR0cDovL3d3dy5kaXRlYy5zay9la3IvcmVnaXN0cmF0aW9uL3YxLjAiIElkPSJSRk9fUFNfWk9aTkFNX0lGT19QT0RMQV9WWUhMQURBVkFDSUNIX0tSSVRFUklJX0JFWl9aRVBfV1NfSU5fMV8wIj4NCsKgwqDCoMKgwqDCoMKgwqA8RG9jdW1lbnRVbmF1dGhvcml6ZWQgeG1sbnM9Imh0dHA6Ly93d3cuZGl0ZWMuc2svZWtyL3VuYXV0aG9yaXplZC92MS4wIiBJZD0iUkZPX1BTX1pPWk5BTV9JRk9fUE9ETEFfVllITEFEQVZBQ0lDSF9LUklURVJJSV9CRVpfWkVQX1dTX0lOXzFfMCI+DQoJCTxPYmplY3QgSWQ9Im5lamFreV9zdHJpbmdvdnlfaWRlbnRpZmlrYXRvcl9uYXByX3BvZGxhX2RhdHVtdV9hX2Nhc3VfMjAwOTEwMTAxMzIzNTYwMDAiIElkZW50aWZpZXI9Imh0dHA6Ly93d3cuZWdvdi5zay9tdnNyL1JGTy9kYXRhdHlwZXMvUG9kcC9FeHQvUG9za3l0bnV0aWVab3puYW11SUZPUG9kbGFWeWhsYWRhdmFjaWNoS3JpdGVyaWlXUy12MS4wLnhzZCI+DQoJCQk8VHJhbnNFbnZJbiB4bWxuczp4c2k9Imh0dHA6Ly93d3cudzMub3JnLzIwMDEvWE1MU2NoZW1hLWluc3RhbmNlIiB4bWxucz0iaHR0cDovL3d3dy5lZ292LnNrL212c3IvUkZPL2RhdGF0eXBlcy9Qb2RwL0V4dC9Qb3NreXRudXRpZVpvem5hbXVJRk9Qb2RsYVZ5aGxhZGF2YWNpY2hLcml0ZXJpaVdTLXYxLjAueHNkIj4NCgkJCQk8UE9EPg0KICA8IS0tIFJGTy5UX1BPU0tZVE5VVElFX09EUElTVV9WU1RVUCAoMSwgXykgLS0+DQogIDxPRVg+DQogICAgPCEtLSBSRk8uVF9PU09CQV9FWFQgKDEsIF8pIC0tPg0KICAgIDwhLS08RE4+MTk1NS0wMy0xMTwvRE4+IERUX0RBVFVNX05BUk9ERU5JQSAoMSwgKikgLS0+DQogICAgPFJDPjczMDcwNC82OTEyPC9SQz4NCiAgICA8IS0tU1ZfUk9ETkVfQ0lTTE8gKDEsICopLS0+DQogICAgPEROPjE5NzMtMDctMDQ8L0ROPg0KICAgIDwhLS0xOTU1LTAzLTExLS0+DQogICAgPFBJPjE8L1BJPg0KICAgIDwhLS1OTF9QT0hMQVZJRV9JRCAoMSwgXykgTVVaID0gMSwgWkVOQSA9IDIsIE5FVVJDRU5FID0gMywgLS0+DQogICAgPFJOPjE5NzM8L1JOPg0KICAgIDwhLS1ST0sgTkFST0RFTklBICgxLCAqKS0tPg0KICAgIDxNT1NMaXN0Pg0KICAgICAgPE1PUz4NCiAgICAgICAgPE1FPnNldmVyaW48L01FPg0KICAgICAgPC9NT1M+DQogICAgPC9NT1NMaXN0Pg0KICAgIDxQUklMaXN0Pg0KICAgICAgPFBSST4NCiAgICAgICAgPFBSPnBvbGlldmthPC9QUj4NCiAgICAgIDwvUFJJPg0KICAgIDwvUFJJTGlzdD4NCiAgICA8UlBSTGlzdD4NCiAgICAgIDxSUFI+DQogICAgICAgIDxSUD5ndWxhczwvUlA+DQogICAgICA8L1JQUj4NCiAgICA8L1JQUkxpc3Q+DQogIDwvT0VYPg0KICA8VUVTPg0KICAgIDwhLS0gUkZPLlRfVURBSkVfRVhUX1NZU1RFTVUgKDEsIF8pIC0tPg0KICAgIDxQTz54ZXFxcGR6czwvUE8+DQogICAgPCEtLSBEZXprbyBTVl9FWFRFUk5ZX1BPVVpJVkFURUwgKDEsIF8pIC0tPg0KICAgIDxUST4zMmJmZDUyZC1kN2FkLTQwMGEtOGE4Ni05NTg0MTY3ZjIyZWE8L1RJPg0KICAgIDwhLS0gMTIzNDU2IE5MX1RJRF9FWFRFUk5FSE9fU1lTVEVNVSAoMSwgXykgLS0+DQogIDwvVUVTPg0KPC9QT0Q+DQoJCQk8L1RyYW5zRW52SW4+DQoJCTwvT2JqZWN0Pg0KCTwvRG9jdW1lbnRVbmF1dGhvcml6ZWQ+DQo8L1JlZ2lzdHJhdGlvbj4NCg==";

                var registration = getRegistrationInput(input, registrationId, documentUnauthorizedId, objectIdentifier);
                var serializer = new XmlSerializer(registration.GetType());
                var regInBytes = new byte[] { };
                using (var ms = new MemoryStream())
                {
                    serializer.Serialize(ms, registration);
                    ms.Position = 0;
                    regInBytes = ms.ToArray();
                }

                return Convert.ToBase64String(regInBytes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Vrati obalku Registration z base64
        /// </summary>
        /// <returns></returns>
        public static Dol.PoskytnutieZoznamuIFOPodlaVyhladavacichKriteriiWS.TransEnvTypeOut GetTransEnvTypeOutFromBase64String(PoskytnutieZoznamuIFOPodlaVyhladavacichKriteriiWeb.ZasielkaType zasielkaType)
        {
            string registrationBase64 = null;
            try
            {
                registrationBase64 = zasielkaType.DataZasielkyBase64;
                var xml = Encoding.UTF8.GetString(Convert.FromBase64String(registrationBase64));

                //deserializacia
                var reader = new StringReader(xml);
                var x = new XmlSerializer(typeof(RegistrationType));
                var registration = (RegistrationType)x.Deserialize(reader);

                var documentUnauthorizedType = getDocumentUnauthorizedType(registration);
                var objectType = documentUnauthorizedType.Object[0];
                return getTransEnvOut(objectType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// getDocumentUnauthorizedType
        /// </summary>
        /// <param name="registrationType"></param>
        /// <returns></returns>
        private static DocumentUnauthorizedType getDocumentUnauthorizedType(RegistrationType registrationType)
        {
            try
            {
                //deserializacia
                var reader = new StringReader(registrationType.Any[0].OuterXml);
                var x = new XmlSerializer(typeof(DocumentUnauthorizedType));
                return (DocumentUnauthorizedType)x.Deserialize(reader);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// getObjectType
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        private static Dol.PoskytnutieZoznamuIFOPodlaVyhladavacichKriteriiWS.TransEnvTypeOut getTransEnvOut(ObjectType objectType)
        {
            try
            {
                //deserializacia
                var reader = new StringReader(objectType.Any[0].OuterXml);
                var x = new XmlSerializer(typeof(Dol.PoskytnutieZoznamuIFOPodlaVyhladavacichKriteriiWS.TransEnvTypeOut));
                return (Dol.PoskytnutieZoznamuIFOPodlaVyhladavacichKriteriiWS.TransEnvTypeOut)x.Deserialize(reader);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Vysklada Registration
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="registrationId"></param>
        /// <param name="documentUnauthorizedId"></param>
        /// <param name="objectIdentifier"></param>
        /// <returns></returns>
        private static RegistrationType getRegistrationInput(Dol.PoskytnutieZoznamuIFOPodlaVyhladavacichKriteriiWS.TransEnvTypeIn input, string registrationId, string documentUnauthorizedId, string objectIdentifier)
        {
            try
            {
                var registration = new RegistrationType();
                registration.Id = registrationId;
                registration.Any = new XmlElement[1];
                registration.Any[0] = getDocumentElement(input, documentUnauthorizedId, objectIdentifier);

                return registration;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Vysklada Document
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="documentUnauthorizedId"></param>
        /// <param name="objectIdentifier"></param>
        /// <returns></returns>
        private static XmlElement getDocumentElement(Dol.PoskytnutieZoznamuIFOPodlaVyhladavacichKriteriiWS.TransEnvTypeIn input, string documentUnauthorizedId, string objectIdentifier)
        {
            try
            {
                var document = new DocumentUnauthorizedType();
                document.Id = documentUnauthorizedId;
                document.Object = new ObjectType[1];
                document.Object[0] = getObjectElement(input, objectIdentifier);

                var serializer = new XmlSerializer(document.GetType());
                var documentInBytes = new byte[] { };
                using (var ms = new MemoryStream())
                {
                    serializer.Serialize(ms, document);
                    ms.Position = 0;
                    documentInBytes = ms.ToArray();
                }

                var xDoc = new XmlDocument();
                xDoc.LoadXml(Encoding.UTF8.GetString(documentInBytes));
                return xDoc.DocumentElement;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Vysklada Object
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="objectIdentifier"></param>
        /// <returns></returns>
        private static ObjectType getObjectElement(Dol.PoskytnutieZoznamuIFOPodlaVyhladavacichKriteriiWS.TransEnvTypeIn input, string objectIdentifier)
        {
            try
            {
                var objectType = new ObjectType();
                objectType.Id = DateTime.Now.ToString("yyyyMMddHHmmss");
                objectType.Identifier = objectIdentifier;
                objectType.Any = new XmlNode[1];
                objectType.Any[0] = getTransEnvTypeIn(input);
                return objectType;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// vrati XmlNode zo vstupnej spravy
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static XmlNode getTransEnvTypeIn(Dol.PoskytnutieZoznamuIFOPodlaVyhladavacichKriteriiWS.TransEnvTypeIn input)
        {
            try
            {
                var serializer = new XmlSerializer(input.GetType());
                var transEnvTypeInBytes = new byte[] { };
                using (var ms = new MemoryStream())
                {
                    serializer.Serialize(ms, input);
                    ms.Position = 0;
                    transEnvTypeInBytes = ms.ToArray();
                }

                var xDoc = new XmlDocument();
                xDoc.LoadXml(Encoding.UTF8.GetString(transEnvTypeInBytes));
                return xDoc.DocumentElement;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        ///// <summary>
        ///// Vrati naplneny zoznam osob pre detail, max 10, mnozinu danu podla strankovania
        ///// </summary>
        ///// <param name="registrationType"></param>
        ///// <param name="pagingInfo"></param>
        ///// <returns></returns>
        //private List<OsobaRfoBrowse> fillRfoList(RegistrationType registrationType, PagingInfo pagingInfo)
        //{
        //    try
        //    {
        //        var documentUnauthorizedType = getDocumentUnauthorizedType(registrationType);
        //        var objectType = documentUnauthorizedType.Object[0];
        //        var transEnvOut = this.getTransEnvOut(objectType);

        //        var osoby = new List<OsobaRfoBrowse>();
        //        foreach (var oex in transEnvOut.POV.OEXList)
        //        {
        //            osoby.Add(new OsobaRfoBrowse()
        //            {
        //                IFO = (!String.IsNullOrEmpty(oex.PO)) ? oex.PO : oex.ID,
        //            });
        //        }

        //        pagingInfo.TotalRecords = osoby.Count;
        //        if (osoby.Count > 0 && pagingInfo.CurrentPage == 0)
        //            pagingInfo.CurrentPage = 1;

        //        //ak je potrebna ina strana pageovania
        //        if (pagingInfo.CurrentPage > 1)
        //            osoby = osoby.Skip((pagingInfo.CurrentPage - 1) * pagingInfo.RecordsPerPage).ToList();

        //        return osoby.Take(10).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionHandling.HandleBusinessException(this, 103, ex + Environment.NewLine +
        //            "Serializovaná odpoveď z externého systému : " + serializeRegistrationType(registrationType));
        //        throw ex;
        //    }
        //}

        ///// <summary>
        ///// Hodnoty zo všetkých sekcií v správe sa spoja do jedného retazca v IAM, podla definovaného poradia
        ///// </summary>
        ///// <param name="tmosList"></param>
        ///// <returns></returns>
        //private string getOEXMeno(Dol.PoskytnutieZoznamuIFOPodlaVyhladavacichKriterii.TMOSO[] tmosList)
        //{
        //    try
        //    {
        //        var sb = new StringBuilder();
        //        if (tmosList != null && tmosList.Any())
        //        {
        //            sb.Append(tmosList.First().ME);
        //        }

        //        return sb.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        ///// <summary>
        ///// Hodnoty zo všetkých sekcií v správe sa spoja do jedného retazca v IAM, podla definovaného poradia
        ///// </summary>
        ///// <param name="priList"></param>
        ///// <returns></returns>
        //private string getOEXPriezvisko(Dol.PoskytnutieZoznamuIFOPodlaVyhladavacichKriterii.TPRIO[] priList)
        //{
        //    try
        //    {
        //        var sb = new StringBuilder();
        //        if (priList != null && priList.Any())
        //        {
        //            sb.Append(priList.First().PR);
        //        }

        //        return sb.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        ///// <summary>
        ///// Serializacia kvoli zalogovaniu v pripade chyby
        ///// </summary>
        ///// <param name="registrationType"></param>
        ///// <returns></returns>
        //private string serializeRegistrationType(RegistrationType registrationType)
        //{
        //    try
        //    {
        //        var serializer = new XmlSerializer(registrationType.GetType());
        //        var regInBytes = new byte[] { };
        //        using (var ms = new MemoryStream())
        //        {
        //            serializer.Serialize(ms, registrationType);
        //            ms.Position = 0;
        //            regInBytes = ms.ToArray();
        //        }

        //        return Encoding.UTF8.GetString(regInBytes);
        //    }
        //    catch
        //    {
        //        return "Nepodarilo sa serializovať odpoveď.";
        //    }
        //}



        #endregion PoskytnutieZoznamuIfoPodlaVyhladavacichKriterii

        #region PoskytnutieReferencnychUdajovZoznamuIfoOnline

        /// <summary>
        /// Vrati Base64 string pre Registration
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="registrationId"></param>
        /// <param name="documentUnauthorizedId"></param>
        /// <param name="objectIdentifier"></param>
        /// <returns></returns>
        public static string GetRegistrationToBase64String(object input, string registrationId, string documentUnauthorizedId, string objectIdentifier)
        {
            try
            {
                var registration = getRegistrationInput(input, registrationId, documentUnauthorizedId, objectIdentifier);
                var serializer = new XmlSerializer(registration.GetType());
                var regInBytes = new byte[] { };
                using (var ms = new MemoryStream())
                {
                    serializer.Serialize(ms, registration);
                    ms.Position = 0;
                    regInBytes = ms.ToArray();
                }

                return Convert.ToBase64String(regInBytes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Vysklada Registration
        /// </summary>
        /// <param name="ifo"></param>
        /// <param name="registrationId"></param>
        /// <param name="documentUnauthorizedId"></param>
        /// <param name="objectIdentifier"></param>
        /// <returns></returns>
        private static RegistrationType getRegistrationInput(object input, string registrationId, string documentUnauthorizedId, string objectIdentifier)
        {
            try
            {
                var registration = new RegistrationType();
                registration.Id = registrationId;
                registration.Any = new XmlElement[1];
                registration.Any[0] = getDocumentElement(input, documentUnauthorizedId, objectIdentifier);

                return registration;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Vysklada Document
        /// </summary>
        /// <param name="ifo"></param>
        /// <param name="documentUnauthorizedId"></param>
        /// <param name="objectIdentifier"></param>
        /// <returns></returns>
        private static XmlElement getDocumentElement(object input, string documentUnauthorizedId, string objectIdentifier)
        {
            try
            {
                var document = new DocumentUnauthorizedType();
                document.Id = documentUnauthorizedId;
                document.Object = new ObjectType[1];
                document.Object[0] = getObjectElement(input, objectIdentifier);

                var serializer = new XmlSerializer(document.GetType());
                var documentInBytes = new byte[] { };
                using (var ms = new MemoryStream())
                {
                    serializer.Serialize(ms, document);
                    ms.Position = 0;
                    documentInBytes = ms.ToArray();
                }

                var xDoc = new XmlDocument();
                xDoc.LoadXml(Encoding.UTF8.GetString(documentInBytes));
                return xDoc.DocumentElement;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Vysklada Object
        /// </summary>
        /// <param name="ifo"></param>
        /// <param name="objectIdentifier"></param>
        /// <returns></returns>
        private static ObjectType getObjectElement(object input, string objectIdentifier)
        {
            try
            {
                var objectType = new ObjectType();
                objectType.Id = DateTime.Now.ToString("yyyyMMddHHmmss");
                objectType.Identifier = objectIdentifier;
                objectType.Any = new XmlNode[1];
                objectType.Any[0] = getTransEnvTypeIn(input);
                return objectType;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// vysklada vstupnu spravu TransEnvTypeIn
        /// </summary>
        /// <param name="osoby"></param>
        /// <returns></returns>
        private static XmlNode getTransEnvTypeIn(object input)
        {
            try
            {
                var serializer = new XmlSerializer(input.GetType());
                var transEnvTypeInBytes = new byte[] { };
                using (var ms = new MemoryStream())
                {
                    serializer.Serialize(ms, input);
                    ms.Position = 0;
                    transEnvTypeInBytes = ms.ToArray();
                }

                var xDoc = new XmlDocument();
                xDoc.LoadXml(Encoding.UTF8.GetString(transEnvTypeInBytes));
                return xDoc.DocumentElement;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        /// <summary>
        /// Vrati obalku Registration z base64
        /// </summary>
        /// <returns></returns>
        public static Dol.PoskytnutieUdajovIFOOnlineWS.TransEnvTypeOut GetTransEnvTypeOutFromBase64String(PoskytnutieUdajovIFOOnlineWeb.ZasielkaType zasielkaType)
        {
            string registrationBase64 = null;
            try
            {
                registrationBase64 = zasielkaType.DataZasielkyBase64;
                var xml = Encoding.UTF8.GetString(Convert.FromBase64String(registrationBase64));

                //deserializacia
                var reader = new StringReader(xml);
                var x = new XmlSerializer(typeof(RegistrationType));
                var registration = (RegistrationType)x.Deserialize(reader);

                var documentUnauthorizedType = getDocumentUnauthorizedType(registration);
                var objectType = documentUnauthorizedType.Object[0];
                return getTransEnvOut2<Dol.PoskytnutieUdajovIFOOnlineWS.TransEnvTypeOut>(objectType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Vrati obalku Registration z base64
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="zasielkaType"></param>
        /// <returns></returns>
        public static T GetTransEnvTypeOutFromBase64String<T>(string DataZasielkyBase64)
        {
            try
            {
                var xml = Encoding.UTF8.GetString(Convert.FromBase64String(DataZasielkyBase64));

                //deserializacia
                var reader = new StringReader(xml);
                var x = new XmlSerializer(typeof(RegistrationType));
                var registration = (RegistrationType)x.Deserialize(reader);

                var documentUnauthorizedType = getDocumentUnauthorizedType(registration);
                var objectType = documentUnauthorizedType.Object[0];

                return getTransEnvOut2<T>(objectType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// getObjectType
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        private static T getTransEnvOut2<T>(ObjectType objectType)
        {
            try
            {
                //deserializacia
                var reader = new StringReader(objectType.Any[0].OuterXml);
                var x = new XmlSerializer(typeof(T));
                return (T)x.Deserialize(reader);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        ///// <summary>
        ///// Hodnoty zo všetkých sekcií v správe sa spoja do jedného retazca v IAM, podla definovaného poradia
        ///// </summary>
        ///// <param name="tmosList"></param>
        ///// <returns></returns>
        //private string getOEXMenoPreDetail(Dol.PoskytnutieUdajovIFOOnlineWS.TMOSO[] tmosList)
        //{
        //    try
        //    {
        //        var orderedList = tmosList.OrderBy(x => x.PO).ToList();
        //        return String.Join(" ", orderedList.Select(p => p.ME));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        ///// <summary>
        ///// Hodnoty zo všetkých sekcií v správe sa spoja do jedného retazca v IAM, podla definovaného poradia
        ///// </summary>
        ///// <param name="priList"></param>
        ///// <returns></returns>
        //private string getOEXPriezviskoPreDetail(Dol.PoskytnutieUdajovIFOOnlineWS.TPRIO[] priList)
        //{
        //    try
        //    {
        //        var orderedList = priList.OrderBy(x => x.PO).ToList();
        //        return String.Join(" ", orderedList.Select(p => p.PR));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        ///// <summary>
        ///// vrati zoznam titulov pre detail osoby
        ///// </summary>
        ///// <param name="toios"></param>
        ///// <returns></returns>
        //private List<OsobaRfoTitul> getTitulList(Dol.PoskytnutieUdajovIFOOnlineWS.TTOS_TOIO[] toios)
        //{
        //    try
        //    {
        //        var listTitul = new List<OsobaRfoTitul>();
        //        foreach (var toi in toios)
        //        {
        //            var osobaRfoTitul = new OsobaRfoTitul()
        //            {
        //                KodTypuTitulu = toi.TT,
        //                NazovTitulu = toi.TITTINA,
        //                TypTitulu = toi.TTITTNA
        //            };
        //            listTitul.Add(osobaRfoTitul);
        //        }

        //        return listTitul;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        ///// <summary>
        ///// varti zoznam ObmedzeniePozbaveniePravnejSposobilosti
        ///// </summary>
        ///// <returns></returns>
        //private List<OsobaRfoObmedzeniePozbaveniePravnejSposobilosti> getObmedzeniePozbaveniePravnejSposobilostiList(TSNR_SNPO[] snrList)
        //{
        //    try
        //    {
        //        var list = new List<OsobaRfoObmedzeniePozbaveniePravnejSposobilosti>();
        //        foreach (var snr in snrList)
        //        {
        //            var obmedzeniePozbaveniePravnejSposobilosti = new OsobaRfoObmedzeniePozbaveniePravnejSposobilosti()
        //            {
        //                TypObmedzeniaPozbaveniaPravnejSposobilostiKod = snr.SN,
        //                TypObmedzeniaPozbaveniaPravnejSposobilostiNazov = snr.SNPSNNA,
        //                DatumZaciatkuPlatnosti = snr.DZ,
        //                DatumUkonceniaPlatnosti = snr.DK,
        //                Poznamka = snr.PZ
        //            };
        //            list.Add(obmedzeniePozbaveniePravnejSposobilosti);
        //        }

        //        return list;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        ///// <summary>
        ///// varti zoznam adries v SR
        ///// </summary>
        ///// <returns></returns>
        //private List<OsobaRfoAdresaSr> getAdresaSrList(Ditec.Iam.Vseob.Dol.PoskytnutieUdajovIFOOnline.TPOB_PHRO[] pobList)
        //{
        //    try
        //    {
        //        var listAdresa = new List<OsobaRfoAdresaSr>();
        //        var pobListSr = pobList.Where(x => x.PM == false).ToList();
        //        foreach (var pob in pobListSr)
        //        {
        //            var adresa = new OsobaRfoAdresaSr()
        //            {
        //                TypPobytuKod = pob.TP,
        //                TypPobytu = pob.TB,
        //                DatumACasPrihlaseniaNaPobyt = pob.DP,
        //                DatumACasUkonceniaPobytu = pob.DU,
        //                Obec = pob.NO,
        //                //ByvanieU = UK,
        //                SupisneCislo = pob.SC,
        //                OrientacneCislo = pob.OL,
        //                CastObce = pob.NC,
        //                Okres = pob.NK,
        //                Ulica = pob.NU
        //            };
        //            listAdresa.Add(adresa);
        //        }

        //        return listAdresa;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        ///// <summary>
        ///// vrati zoznam adries mimo SR
        ///// </summary>
        ///// <returns></returns>
        //private List<OsobaRfoAdresaMimoSr> getAdresaMimoSrList(Ditec.Iam.Vseob.Dol.PoskytnutieUdajovIFOOnline.TPOB_PHRO[] pobList)
        //{
        //    try
        //    {
        //        var listAdresa = new List<OsobaRfoAdresaMimoSr>();
        //        var pobListMimoSr = pobList.Where(x => x.PM == true).ToList();
        //        foreach (var pob in pobListMimoSr)
        //        {
        //            var adresa = new OsobaRfoAdresaMimoSr()
        //            {
        //                TypPobytuKod = pob.TP,
        //                TypPobytu = pob.TB,
        //                DatumACasPrihlaseniaNaPobyt = pob.DP,
        //                DatumACasUkonceniaPobytu = pob.DU,
        //                Stat = pob.NS,
        //                Okres = pob.OP,
        //                Obec = pob.OO,
        //                CastObce = pob.CC,
        //                Ulica = pob.UM,
        //                OrientacneCislo = pob.OS,
        //                SupisneCislo = pob.SI,
        //                CastBudovy = pob.CU,
        //                PocitacoveCisloDomu = pob.PC,
        //                CisloBytu = pob.CB
        //            };
        //            listAdresa.Add(adresa);
        //        }

        //        return listAdresa;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        ///// <summary>
        ///// Naplní osobu z výstupu
        ///// </summary>
        ///// <param name="registrationType"></param>
        ///// <returns></returns>
        //private List<OsobaRfoBrowse> fillOsobaRfoBrowse(RegistrationType registrationType)
        //{
        //    try
        //    {
        //        var documentUnauthorizedType = this.getDocumentUnauthorizedType(registrationType);
        //        var objectType = documentUnauthorizedType.Object[0];
        //        var transEnvOut = this.getTransEnvOut2(objectType);

        //        var osoby = new List<OsobaRfoBrowse>();
        //        foreach (var oex in transEnvOut.POV.OEXList)
        //        {
        //            osoby.Add(new OsobaRfoBrowse()
        //            {
        //                IFO = (!String.IsNullOrEmpty(oex.PO)) ? oex.PO : oex.ID,
        //                Meno = getOEXMenoPreDetail(oex.MOSList),
        //                Priezvisko = getOEXPriezviskoPreDetail(oex.PRIList),
        //                DatumNarodenia = oex.DN
        //            });
        //        }

        //        return osoby;
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionHandling.HandleBusinessException(this, 112, ex + Environment.NewLine +
        //            "Serializovaná odpoveď z externého systému : " + serializeRegistrationType(registrationType));
        //        throw ex;
        //    }
        //}

        ///// <summary>
        ///// Naplní osobu z výstupu
        ///// </summary>
        ///// <param name="registrationType"></param>
        ///// <returns></returns>
        //private OsobaRfo fillOsobaRfo(RegistrationType registrationType)
        //{
        //    try
        //    {
        //        var documentUnauthorizedType = this.getDocumentUnauthorizedType(registrationType);
        //        var objectType = documentUnauthorizedType.Object[0];
        //        var transEnvOut = this.getTransEnvOut2(objectType);
        //        if (transEnvOut.POV.KO != 1)
        //        {
        //            throw new Exception("Odpoveď z RFO je prijatá s chybou - Kód: " + transEnvOut.POV.KO + ", Popis: " + transEnvOut.POV.NU);
        //        }

        //        var osobaRfo = new OsobaRfo()
        //        {
        //            Ifo = transEnvOut.POV.OEXList[0].ID,
        //            IfoPravejOsoby = transEnvOut.POV.OEXList[0].PO,
        //            DatumNarodenia = transEnvOut.POV.OEXList[0].DN,
        //            DatumUmrtia = transEnvOut.POV.OEXList[0].DU,
        //            Meno = this.getOEXMenoPreDetail(transEnvOut.POV.OEXList[0].MOSList),
        //            Priezvisko = this.getOEXPriezviskoPreDetail(transEnvOut.POV.OEXList[0].PRIList),
        //            RodneCislo = transEnvOut.POV.OEXList[0].RC,
        //            RokNarodenia = transEnvOut.POV.OEXList[0].RN,
        //            TypOsoby = transEnvOut.POV.OEXList[0].TVKTVNA,
        //            Titul = this.getTitulList(transEnvOut.POV.OEXList[0].TOSList),
        //            ObmedzeniePozbaveniePravnejSposobilosti = this.getObmedzeniePozbaveniePravnejSposobilostiList(transEnvOut.POV.OEXList[0].SNRList),
        //            AdresaSr = this.getAdresaSrList(transEnvOut.POV.OEXList[0].POBList),
        //            AdresaMimoSr = this.getAdresaMimoSrList(transEnvOut.POV.OEXList[0].POBList)
        //        };

        //        return osobaRfo;
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionHandling.HandleBusinessException(this, 204, ex + Environment.NewLine +
        //            "Serializovaná odpoveď z externého systému : " + serializeRegistrationType(registrationType));
        //        throw ex;
        //    }
        //}

        #endregion PoskytnutieReferencnychUdajovZoznamuIfoOnline

    }
}
