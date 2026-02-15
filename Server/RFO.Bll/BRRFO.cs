using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ditec.RIS.CC.Dal.Inf;
using Ditec.RIS.CC.Dol;
using Ditec.RIS.RFO.Bll.Inf;
using Ditec.RIS.RFO.Dol;
using Ditec.RIS.SysFra.DataAccess.Infrastructure;
using Ditec.SysFra.Infrastructure.Impl;
using LinFu.IoC.Configuration;
using Ditec.SysFra.DataTypes.Infrastructure;
using Ditec.RIS.DZP.Dol;
using Ditec.RIS.ZAM.Dol;
using Ditec.RIS.RFO.Dal.Inf;
using Ditec.RIS.CC.Bll.Inf;
using NHibernate;
using Ditec.SysFra.NhSql.Dal;
using System.Xml;
using System.IO;

namespace Ditec.RIS.RFO.Bll
{
    [Implements(typeof(IBFRFO))]
    [Implements(typeof(IBFPoskytnutieCiselnikov))]
    [Implements(typeof(IBFZoznamIFOPodlaKriterii))]
    [Implements(typeof(BRRFO))]
    public class BRRFO : BusinessRulesBase, IBFPoskytnutieCiselnikov, IBFZoznamIFOPodlaKriterii, IBFRFO
    {
        /// <summary>
        /// RFOCis UC 001 Aktualizácia èíselníkov 
        /// Pozn. Aktualizácia číselníka Typ územného celku TUC musí byť pred aktualizáciou číselníka Územný celok UC a  aktualizácia číselníka Typ titulu TTI musí byť pred aktualizáciou číselníka Titul TIT
        /// </summary>
        public int AktualizaciaCiselnikov()
        {
            var LogZmeny = SIS.Dol.DAVKA.LogSpracovaniaDavok.GetInstance();
            try
            {
                var informationalWS = new InformationalWS();
                //System.IO.File.AppendAllText(@"d:\Projects\RIS2\Trunk\Code\RIS2\Server\RIS.ProcessStarter\bin\Debug\Spravy\Annotations.txt", "Zaciatok: " + DateTime.Now + Environment.NewLine);

                //Systém vyhľadá zoznam číselníkov, kde Externý primárny zdroj - Popis = RFO  
                var dataOperationZoznamCiselnikovBrowse = GetDataAccessLayer<IBrowse<BaseClass>>(ToolsDol.GetServiceNameByCriteria(new ZoznamCiselnikovFilterCriteria()));
                var dataOperationZoznamCiselnikovUpdate = GetDataAccessLayer<ICRUD<BaseClass>>(ToolsDol.GetServiceNameByCriteria(new ZoznamCiselnikovFilterCriteria()));
                var zoznamCiselnikovList = dataOperationZoznamCiselnikovBrowse.Browse(new ZoznamCiselnikovFilterCriteria() { ExtPrimarnyZdrojPopis = "RFO" }).ToList().ToChildList<ZoznamCiselnikov>();
                zoznamCiselnikovList.Sort((item1, item2) => item1.ZoznamCiselnikovId.CompareTo(item2.ZoznamCiselnikovId));

                //Pre každý číselník zo zoznamu číselníkov
                foreach (var ciselnik in zoznamCiselnikovList)
                {
                    Console.WriteLine("Aktualizujem číselník " + ciselnik.Skratka + " " + DateTime.Now.ToString("HH:mm:ss"));
                    //if (ciselnik.Skratka != "UCE")
                    //    continue;

                    var PS = 1;
                    //najdem si triedu, ktoru idem pouzit na vstupnu spravu
                    var transEnvTypeIn = Tools.GetTransEnvTypeIn(ciselnik.Skratka);
                    if (transEnvTypeIn != null)
                    {
                        //Systém načíta "Kód číselníka" = "Skratka" a Dátum od" =  "Dátum poslednej aktualizácie" z príslušného záznamu zoznamu číselníkov
                        var datumOd = ciselnik.DatumPoslednejAktualizacie;

                        //Ak "Dátum od" je prázdny (číselník ešte nebol naplnený) "Dátum od" = "0001-01-01T00:00:00+02:00"
                        if (!datumOd.HasValue)
                            datumOd = DateTime.Parse("0001-01-01T00:00:00+02:00");

                        Tools.FillTransEnvTypeIn(ref transEnvTypeIn, datumOd.Value, DateTime.Now, PS);

                        //zavolanie servisu
                        var resultResponse = informationalWS.PoskytnutieCiselnikovWSRequestToRfoService(transEnvTypeIn);
                        var codeList = new List<object>();

                        //ak sa mi nieco vratilo, tak idem nacitavat dalej az kym sa mi nevrati prazdny zoznam
                        while (Tools.FindCodeListByKodCiselnika(resultResponse, ciselnik.Skratka, ref codeList))
                        {
                            //vytvorim si vstupnu spravu s posunutou strankou
                            PS++;
                            Tools.FillTransEnvTypeIn(ref transEnvTypeIn, datumOd.Value, DateTime.Now, PS);
                            //nacitam si udaje z RFO
                            resultResponse = informationalWS.PoskytnutieCiselnikovWSRequestToRfoService(transEnvTypeIn);
                        }

                        if (resultResponse != null && resultResponse.VSP.KI == 1)
                        {
                            LogEntryFactory.LogBusinessRulesInformation(this, 2012, "Aktualizoval sa " + ciselnik.Skratka + ", naslo sa " + codeList.Count + " zaznamov, datum bol " + datumOd.Value);

                            int i = 0;
                            int reminder;
                            var partList = new List<object>();
                            foreach (var item in codeList)
                            {
                                partList.Add(item);
                                i++;
                                //zistim, ci je zvysok po deleni budem ukladat po 1000, kazdych 10 poloziek budem volat v samostatnej davke
                                Math.DivRem(i, 100, out reminder);
                                if (reminder == 0 || i == codeList.Count)
                                {
                                    // vytvori sa nova databazova transakcia 
                                    //using (var transactionScope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, new TimeSpan(0, 20, 0)))
                                    //{
                                    //mam nacitany zoznam ciselnikov z RFO, idem ho ulozit do DB
                                    var dataOperation = GetDataAccessLayer<IRFOAktualizacia>(ciselnik.Skratka);
                                    dataOperation.Aktualizacia(partList);

                                    // potvrdenie dokoncenia transakcie 
                                    //    transactionScope.Complete();
                                    //}

                                    Console.WriteLine("Ulozilo sa " + partList.Count + " zaznamov číselníka " + ciselnik.Skratka + ", ostava " + (codeList.Count - i) + " " + DateTime.Now.ToString("HH:mm:ss"));

                                    partList.Clear();
                                }
                            }


                            //Zapíše sa dátum poslednej aktualizácie èíselníka
                            ciselnik.DatumPoslednejAktualizacie = DateTime.Now;
                            dataOperationZoznamCiselnikovUpdate.Update(ciselnik);
                        }
                        else
                        {
                            LogEntryFactory.LogBusinessRulesInformation(this, 2013, "Aktualizacia " + ciselnik.Skratka + " nepresla." + (resultResponse != null ? Environment.NewLine + "Navratovy popis chyby: " + resultResponse.VSP.PO : string.Empty));
                        }
                    }
                }

                LogEntryFactory.LogBusinessRulesInformation(this, 2011, "Aktualizácia bola úspešná.");
                return 1;
            }
            catch (Exception ex)
            {
                ExceptionHandling.HandleBusinessException(this, ex, 201, MethodInfo.GetCurrentMethod().Name);
                return -1;
            }
            finally
            {
                //Zapise sa logWS z volania RFO
                if (Tools.ShouldLogXmlMessages)
                    this.UlozLogWS();
                SIS.Dol.DAVKA.LogSpracovaniaDavok.FinishInstance();
            }
        }

        public void SpracovanieZmenovychDavok(OsobaResponse input = null, bool zapisNovejOsoby = false)
        {
            var start = DateTime.Now;
            var pocetCelkovy = 0;
            var pocetUspesnych = 0;
            var LogZmeny = SIS.Dol.DAVKA.LogSpracovaniaDavok.GetInstance();
            var uspesneSpracovanie = false;
            try
            {
                LogEntryFactory.LogBusinessRulesInformation(this, 2033, "Zacina spracovanie zmenovych davok");

                Dol.ZoznamIFOSoZmenenymiReferencnymiUdajmiWS.TransEnvTypeOut resultResponse = null;

                if (input == null)
                {
                    //3.1   Systém zavolá  získanie zmenovej dávky
                    resultResponse = this.ZoznamIFOSoZmenenymiReferencnymiUdajmiWS();

                    //Ak  v návratových údajoch výstupnej správy služby  Návratový kód" <> 1
                    if (resultResponse == null || resultResponse.POV != null && resultResponse.POV.KO != 1)
                    {
                        //3.1.1.1 Ak Návratový kód = 1010,  t.j. neexistuje žiadna dávka pre RIS
                        if (resultResponse != null && resultResponse.POV != null && resultResponse.POV.KO == 1010)
                        {
                            //3.1.2.1   Systém aktualizuje log zmenoveho suboru : 
                            //Súbor úspešne spracovaný = TRUE, Poèet dávok v súbore = 0,
                            //Návratový kód = 1010
                            this.ZapisZmenovyLog(ref LogZmeny.LogZmenovychSuborov, DateTime.Now, this.TransactionID == null ? "prazdne" : this.TransactionID.ToString(), resultResponse.POV.KO, 0, null, null, null, true);
                            uspesneSpracovanie = true;
                            //Systém ukončí činnosť 
                            return;
                        }

                        //chybu z RFO si zapisem do logSuboru
                        this.ZapisZmenovyLog(ref LogZmeny.LogZmenovychSuborov, DateTime.Now, this.TransactionID == null ? "prazdne" : this.TransactionID.ToString(), resultResponse == null ? null : resultResponse.POV.KO, null, null, null, null, false);

                        //Systém zaloguje danú udalosť, že "Spracovanie zmenových dávok z RFO padlo s chybou " a proces sa ukončí.
                        throw new Exception("Spracovanie zmenových dávok z RFO padlo s chybou: " + (resultResponse != null ? resultResponse.POV.NU : "resultResponse == null"));
                    }

                    input = resultResponse;
                }

                //Cyklus sa bude vykonávať dovtedy pokiaľ sa v zmenových dávkach vrátia zmenové údaje aspoň jednej osoby:
                while (input.OsobaList.Count > 0)
                {
                    pocetCelkovy += input.OsobaList.Count;
                    this.ZapisZmenovyLog(ref LogZmeny.LogZmenovychSuborov, DateTime.Now, this.TransactionID == null ? "prazdne" : this.TransactionID.ToString(), resultResponse == null ? 1 : resultResponse.POV.KO, input.OsobaList.Count, input.OsobaList.Count > 0 ? input.OsobaList[0].FyzickaOsoba.IdentifikatorZmenovejDavky : new long?(), input.OsobaList.Count > 0 ? input.OsobaList[input.OsobaList.Count - 1].FyzickaOsoba.IdentifikatorZmenovejDavky : new long?(), null, false);

                    foreach (var osoba in input.OsobaList)
                    {
                        try
                        {
                            SIS.Dol.DAVKA.ZmenovaDavka zmenovaDavka = null;

                            //3.1.3.2   Ak je "Testovacie logovanie" = TRUE
                            if (Tools.ShouldLogXmlOsoba)
                            {
                                zmenovaDavka = new SIS.Dol.DAVKA.ZmenovaDavka() { Ifo = osoba.FyzickaOsoba.Ifo, IfoPravejOsoby = osoba.FyzickaOsoba.IfoPravejOsoby, Xml = ToolsRFO.GetXmlString(osoba), IdentifikatorZmenovejDavky = osoba.FyzickaOsoba.IdentifikatorZmenovejDavky, Vyriesena = false };
                                //Systém vytvorí záznam o spracovávanej dávke
                                LogZmeny.ZmenovaDavkaList.Add(zmenovaDavka);
                            }

                            //ak osoba nema IFO, tak sa nebude spracovavat
                            if (String.IsNullOrEmpty(osoba.FyzickaOsoba.Ifo))
                            {
                                this.ZapisZmenovyLog(ref LogZmeny.LogZmenovychSuborov, null, null, null, null, null, null, osoba.FyzickaOsoba.IdentifikatorZmenovejDavky, true);
                                pocetUspesnych++;
                                continue;
                            }

                            //ak prisla prazdna osoba len s ifom, mame ju u nas vymazat
                            if (osoba.FyzickaOsoba.Vymazat)
                            {
                                var foList = GetDataAccessLayer<IFyzickaOsoba>().FyzickaOsobaIfoList(new FyzickaOsobaFilterCriteria() { Ifo = osoba.FyzickaOsoba.Ifo });
                                if (foList.Count > 0)
                                    GetDataAccessLayer<ICRUD<Ditec.RIS.RFO.Dol.FyzickaOsoba>>().Delete(foList[0]);
                            }
                            else
                            {
                                //3.1.2 Pre každý záznam  výstupnej správy
                                //Systém zavolá metódu na spracovanie zmien jednej fyzickej osoby
                                osoba.FyzickaOsoba.ZaujmovaOsoba = true;
                                this.AktualizaciaUdajovFyzickejOsobyUdajmiRFO(osoba, true, zapisNovejOsoby, null);
                            }

                            //Ak je "Testovacie logovanie" = TRUE
                            if (Tools.ShouldLogXmlOsoba)
                                //Systém aktualizuje záznam o spracovávanej dávke:  Vyriešená = TRUE
                                zmenovaDavka.Vyriesena = true;

                            //3.1.2.2  Ak spracovanie zmien osoby prebehlo úspešne
                            //ak som na vyvoji nepotvrdzujem prijatie, nech neberiem davky testerom
                            if (Tools.PotvrdzovaniePrijatiaZmien)
                            {
                                //3.1.3.4.3 Systém zavolá Potvrdenie prijatia zmien z RFO s parametrom identifikátora zmenovej dávky - "Identifikátor zmeny pre externý systém" zo zmenovej dávky
                                var retVal = this.PotvrdzovaniePrijatiaZmien(osoba.FyzickaOsoba.IdentifikatorZmenovejDavky.Value);
                                //Ak sa vo výstupnej správe služby potvrdenia prijatia zmien nachádza Návratový kód"  <> 1
                                if (retVal == null || retVal.VSP.KI != 1)
                                {
                                    //Systém zaloguje danú udalosť a proces sa ukončí.
                                    throw new Exception("Potvrdenie prijatia zmien z RFO padlo s chybou: " + (retVal != null ? retVal.VSP.PO : "retVal == null"));
                                }
                            }
                            pocetUspesnych++;

                            //3.1.3.4.2 Systém aktualizuje log zmenoveho suboru : 
                            //ID poslednej úspešne spracovanej zmenovej  dávky = identifikátor práve spracovanej zmenovej dávky
                            this.ZapisZmenovyLog(ref LogZmeny.LogZmenovychSuborov, null, null, null, null, null, null, osoba.FyzickaOsoba.IdentifikatorZmenovejDavky, true);
                        }
                        catch(Exception ex)
                        {
                            //Ináč (ak sa vyskytla chyba)
                            //Ak je "Testovacie logovanie" = FALSE
                            if (!Tools.ShouldLogXmlOsoba)
                                //Systém vytvorí záznam o spracovávanej dávke
                                LogZmeny.ZmenovaDavkaList.Add(new SIS.Dol.DAVKA.ZmenovaDavka() { Ifo = osoba.FyzickaOsoba.Ifo, IfoPravejOsoby = osoba.FyzickaOsoba.IfoPravejOsoby, Xml = ToolsRFO.GetXmlString(osoba), IdentifikatorZmenovejDavky = osoba.FyzickaOsoba.IdentifikatorZmenovejDavky, Vyriesena = false });
                            
                            ExceptionHandling.HandleBusinessException(this, ex, 226, MethodInfo.GetCurrentMethod().Name);
                            throw ex;
                        }
                    }

                    //ak som na vyvoji koncim
                    if (!Tools.PotvrdzovaniePrijatiaZmien)
                    {
                        uspesneSpracovanie = true;
                        return;
                    }

                    //idem na dalsie kolecko
                    resultResponse = this.ZoznamIFOSoZmenenymiReferencnymiUdajmiWS();

                    //Ak  v návratových údajoch výstupnej správy služby  Návratový kód" <> 1
                    if (resultResponse == null || (resultResponse.POV != null && resultResponse.POV.KO != 1))
                    {
                        //3.1.1.1 Ak Návratový kód = 1010,  t.j. neexistuje žiadna dávka pre RIS
                        if (resultResponse != null && resultResponse.POV != null && resultResponse.POV.KO == 1010)
                        {
                            //Systém ukončí činnosť 
                            this.ZapisZmenovyLog(ref LogZmeny.LogZmenovychSuborov, DateTime.Now, this.TransactionID == null ? "prazdne" : this.TransactionID.ToString(), resultResponse.POV.KO, null, null, null, null, true);
                            uspesneSpracovanie = true;
                            return;
                        }

                        //chybu z RFO si zapisem do logSuboru
                        this.ZapisZmenovyLog(ref LogZmeny.LogZmenovychSuborov, DateTime.Now, this.TransactionID == null ? "prazdne" : this.TransactionID.ToString(), resultResponse == null ? null : resultResponse.POV.KO, null, null, null, null, false);

                        //Systém zaloguje danú udalosť, že "Spracovanie zmenových dávok z RFO padlo s chybou " a proces sa ukončí.
                        throw new Exception("Spracovanie zmenových dávok z RFO padlo s chybou: " + (resultResponse != null ? resultResponse.POV.NU : "resultResponse == null"));
                    }

                    input = resultResponse;
                }
                uspesneSpracovanie = true;
            }
            catch (Exception ex)
            {
                //ak sa uz prve volanie nepodarilo
                this.ZapisZmenovyLog(ref LogZmeny.LogZmenovychSuborov, null, null, null, null, null, null, null, false);
                ExceptionHandling.HandleBusinessException(this, ex, 221, MethodInfo.GetCurrentMethod().Name);
            }
            finally
            {
                this.UlozZmenovyLog(LogZmeny);
                SIS.Dol.DAVKA.LogSpracovaniaDavok.FinishInstance();

                LogEntryFactory.LogBusinessRulesInformation(this, 2034, "Spracovanie zmenovych davok: trvanie " + (DateTime.Now - start) + ", celkovy pocet osob " + pocetCelkovy + ", pocet uspesnych " + pocetUspesnych + ", uspesne spracovanie vsetkych davok " + uspesneSpracovanie);
            }
        }

        /// <summary>
        /// RFO UC 021 Vyhľadanie zoznamu IFO
        /// </summary>
        /// <param name="RodneCislo"></param>
        /// <param name="Meno"></param>
        /// <param name="Priezvisko"></param>
        /// <param name="RodnePriezvisko"></param>
        /// <param name="DatumNarodenia"></param>
        /// <returns></returns>
        public Dol.PoskytnutieUdajovIFOOnlineWS.TransEnvTypeOut VyhľadanieZoznamuIFO(string RodneCislo, List<Meno> MenoList, List<Priezvisko> PriezviskoList, List<RodnePriezvisko> RodnePriezviskoList, DateTime? DatumNarodenia, bool callIfoOnline = true, bool throwExeption = false)
        {
            Dol.PoskytnutieUdajovIFOOnlineWS.TransEnvTypeOut resultResponse = null;
            try
            {
                /*
                 Minimálna množina vyh¾adávacích kritérií, ktorá je potrebná pre vyhåadávanie:
                Niektorá z nasledujúcich minimálnych množín požadovaných údajov (minimálna množina môže by zadaná aj v kombinácii s inými vyh¾adávacími parametrami):
                        "Rodné èíslo"
                        "Priezvisko", "Rodné priezvisko" a "Dátum narodenia"  môžu by zadané minimálne v kombinácií 
                    a) "Priezvisko" - "Dátum narodenia" 
                    b) "Rodné priezvisko" - "Dátum narodenia" 
                */
                if (!String.IsNullOrEmpty(RodneCislo) || (PriezviskoList != null && PriezviskoList.Count > 0 && DatumNarodenia.HasValue) || (RodnePriezviskoList != null && RodnePriezviskoList.Count > 0 && DatumNarodenia.HasValue))
                {
                    //Systém zavolá WS na vyh¾adanie osôb v RFO, ktoré spåòajú vyh¾adávacie kritériá
                    var resultCriteria = this.PoskytnutieZoznamuIFOPodlaVyhladavacichKriterii(RodneCislo, MenoList, PriezviskoList, RodnePriezviskoList, DatumNarodenia);
                    //ak  Návratový kód z WS <> 1, UC sa ukonèí  a vráti návratovú hodnotu "Chyba pri spracovaní"
                    if (resultCriteria == null || resultCriteria.POV == null || resultCriteria.POV.KO != 1)
                    {
                        //ak sa nenaslo IFO pre osobu, nebudem to pokladat za chybu
                        if (resultCriteria != null && resultCriteria.POV != null && resultCriteria.POV.KO == 1020)
                        {
                            resultResponse = new Dol.PoskytnutieUdajovIFOOnlineWS.TransEnvTypeOut();
                            resultResponse.POV = new Dol.PoskytnutieUdajovIFOOnlineWS.TPOVO() { KO = 1, OEXList = new Dol.PoskytnutieUdajovIFOOnlineWS.TOEX_OEIO[0] };
                            return resultResponse;
                        }

                        throw new Exception(resultCriteria != null && resultCriteria.POV != null ? "Popis chyby z RFO pri volani PoskytnutieZoznamuIFOPodlaVyhladavacichKriterii: " + resultCriteria.POV.NU.Trim() : string.Empty);
                    }

                    if (!callIfoOnline)
                    {
                        resultResponse = new Dol.PoskytnutieUdajovIFOOnlineWS.TransEnvTypeOut();
                        resultResponse.POV = new Dol.PoskytnutieUdajovIFOOnlineWS.TPOVO() { KO = 1, OEXList = new Dol.PoskytnutieUdajovIFOOnlineWS.TOEX_OEIO[resultCriteria.POV.OEXList.Length] };
                        for (int i = 0; i < resultCriteria.POV.OEXList.Length; i++)
                        {
                            resultResponse.POV.OEXList[i] = new Dol.PoskytnutieUdajovIFOOnlineWS.TOEX_OEIO();
                            resultResponse.POV.OEXList[i].ID = resultCriteria.POV.OEXList[i].ID;
                            resultResponse.POV.OEXList[i].PO = resultCriteria.POV.OEXList[i].PO;
                        }
                    }
                    else
                    {
                        //Systém z obdržanej výstupnej správy vytvorí zoznam osôb - zoznam IFO, pre ktoré sa následne bude volať WS na poskytnutie referenčných údajov osoby.
                        //Vytvorenie zoznamu IFO:
                        //Zoznam IFO sa berie z výstupnej správy z minulého kroku. V prípade, že je v zázname vyplnený atribút "IFO pravej osoby" berie sa jeho hodnota, ak nie je vyplnený berie sa hodnota atribútu "IFO".
                        //zrusene Xeniou, bude sa vzdy vyhladavat IFO
                        var ifoList = new List<string>();
                        foreach (var item in resultCriteria.POV.OEXList)
                        {
                            ifoList.Add(item.ID);
                        }

                        //zavolam RFO pre info
                        // Vo vstupnej správe pre poskytnutie údajov osoby je možné zadefinovať sekcie, ktoré majú byť vrátené. V tomto procese sú vyžadované Identifikačné údaje (2) a Lokačné údaje (4).
                        //Do zoznamu "Požadované sekcie", ktorý bude parametrom volania pre Poskytnutie referenčńých údajov sa vloží 2 a 4.
                        //Systém zavolá proces pre získanie identifikačných údajov   zonamu IFO.  Vstupom do daného procesu bude zoznam parametrov:
                        //"Zoznam IFO"
                        //"Požadované sekcie"
                        //Ak  Návratový kód z WS <> 1,  UC sa ukonč a vráti návratovú hodnotu "Chyba pri spracovaní"
                        resultResponse = this.PoskytnutieReferencnychUdajovZoznamuIFOOnline(ifoList, new List<int>() { 1, 2, 3, 4 }, throwExeption);
                        if (resultResponse == null || resultResponse.POV == null || resultResponse.POV.KO != 1)
                            throw new Exception(resultResponse != null && resultResponse.POV != null ? Environment.NewLine + "Popis chyby z RFO pri volani PoskytnutieReferencnychUdajovZoznamuIFOOnline: " + resultResponse.POV.NU : string.Empty);
                    }
                }
                else
                {
					//Nie je zadaná minimálna množina vyhľadávacích kritérií!
					throw new Exception(GetBussinessLogicLayer<IBFCodeList>().GetBufferedCodeList<ChybovaSprava>().GetText(51606));
                }
            }
            catch (Exception ex)
            {
                //resultResponse.POV = new Dol.PoskytnutieUdajovIFOOnlineWS.TPOVO();
                //resultResponse.POV.KO = -1;
                //resultResponse.POV.NU = "Chyba pri spracovaní" + Environment.NewLine + ex.Message;
                //TODO - upravit tento exception odtial az na klientovi
				if (ex.Message.Equals(GetBussinessLogicLayer<IBFCodeList>().GetBufferedCodeList<ChybovaSprava>().GetText(51606)))
                {
					//Nie je zadaná minimálna množina vyhľadávacích kritérií!
					throw new Exception(GetBussinessLogicLayer<IBFCodeList>().GetBufferedCodeList<ChybovaSprava>().GetText(51606));
                }
                else
                {
                    ExceptionHandling.HandleBusinessException(this, ex, 202, MethodInfo.GetCurrentMethod().Name);
                    throw new Exception(GetBussinessLogicLayer<IBFCodeList>().GetBufferedCodeList<ChybovaSprava>().GetText(51501), ex);
                }
            }

            return resultResponse;
        }

        /// <summary>
        /// RFOWS UC 010 Poskytnutie zoznamu IFO pod¾a vyh¾adávacích kritérií
        /// </summary>
        /// <returns></returns>
        public Dol.PoskytnutieZoznamuIFOPodlaVyhladavacichKriteriiWS.TransEnvTypeOut PoskytnutieZoznamuIFOPodlaVyhladavacichKriterii(string RodneCislo, List<Meno> MenoList, List<Priezvisko> PriezviskoList, List<RodnePriezvisko> RodnePriezviskoList, DateTime? DatumNarodenia, bool throwExeption = false)
        {
            var informationalWS = new InformationalWS();
            Dol.PoskytnutieZoznamuIFOPodlaVyhladavacichKriteriiWS.TransEnvTypeOut resultResponse = null;
            try
            {
                Dol.PoskytnutieZoznamuIFOPodlaVyhladavacichKriteriiWS.TransEnvTypeIn transEnvTypeIn = new Dol.PoskytnutieZoznamuIFOPodlaVyhladavacichKriteriiWS.TransEnvTypeIn();
                transEnvTypeIn.POD = new Dol.PoskytnutieZoznamuIFOPodlaVyhladavacichKriteriiWS.TPOD();
                transEnvTypeIn.POD.OEX = new Dol.PoskytnutieZoznamuIFOPodlaVyhladavacichKriteriiWS.TOEX_OEI();
                transEnvTypeIn.POD.OEX.MOSList = MenoList != null && MenoList.Count > 0 ? new Dol.PoskytnutieZoznamuIFOPodlaVyhladavacichKriteriiWS.TMOS[MenoList.Count] : new Dol.PoskytnutieZoznamuIFOPodlaVyhladavacichKriteriiWS.TMOS[1];
                transEnvTypeIn.POD.OEX.PRIList = PriezviskoList != null && PriezviskoList.Count > 0 ? new Dol.PoskytnutieZoznamuIFOPodlaVyhladavacichKriteriiWS.TPRI[PriezviskoList.Count] : new Dol.PoskytnutieZoznamuIFOPodlaVyhladavacichKriteriiWS.TPRI[1];
                transEnvTypeIn.POD.OEX.RPRList = RodnePriezviskoList != null && RodnePriezviskoList.Count > 0 ? new Dol.PoskytnutieZoznamuIFOPodlaVyhladavacichKriteriiWS.TRPR[RodnePriezviskoList.Count] : new Dol.PoskytnutieZoznamuIFOPodlaVyhladavacichKriteriiWS.TRPR[1];

                if (!String.IsNullOrEmpty(RodneCislo))
                    transEnvTypeIn.POD.OEX.RC = RodneCislo;

                if (DatumNarodenia.HasValue)
                {
                    transEnvTypeIn.POD.OEX.DNSpecified = true;
                    transEnvTypeIn.POD.OEX.DN = DatumNarodenia.Value;
                }

                transEnvTypeIn.POD.UES = new Dol.PoskytnutieZoznamuIFOPodlaVyhladavacichKriteriiWS.TUES();
                transEnvTypeIn.POD.UES.PO = Tools.RisUser;
                transEnvTypeIn.POD.UES.TI = this.TransactionID == null ? "prazdne" : this.TransactionID.ToString();

                if (MenoList != null)
                    for (int i = 0; i < MenoList.Count; i++)
                        transEnvTypeIn.POD.OEX.MOSList[i] = new Dol.PoskytnutieZoznamuIFOPodlaVyhladavacichKriteriiWS.TMOS() { ME = MenoList[i].Hodnota };

                if (PriezviskoList != null)
                    for (int i = 0; i < PriezviskoList.Count; i++)
                        transEnvTypeIn.POD.OEX.PRIList[i] = new Dol.PoskytnutieZoznamuIFOPodlaVyhladavacichKriteriiWS.TPRI() { PR = PriezviskoList[i].Hodnota };

                if (RodnePriezviskoList != null)
                    for (int i = 0; i < RodnePriezviskoList.Count; i++)
                        transEnvTypeIn.POD.OEX.RPRList[i] = new Dol.PoskytnutieZoznamuIFOPodlaVyhladavacichKriteriiWS.TRPR() { RP = RodnePriezviskoList[i].Hodnota };

                //Systém zavolá WS na vyh¾adanie osôb v RFO, ktoré spåòajú vyh¾adávacie kritériá

                resultResponse = informationalWS.PoskytnutieZoznamuIFOPodlaVyhladavacichKriteriiWSRequestToRfoService(transEnvTypeIn);

                //this.ZapisVystupXML(resultResponse);
                //ak  Návratový kód z WS <> 1, UC sa ukonèí  a vráti návratovú hodnotu "Chyba pri spracovaní"
                if (resultResponse == null || resultResponse.POV == null || resultResponse.POV.KO != 1)
                {
                    //Ak bol kod 1020 - Nebola nájdena fyzická osoba na základe vstupných parametrov
                    //vytvorim si prazdny zoznam
                    if (resultResponse != null && resultResponse.POV != null && resultResponse.POV.KO == 1020)
                        resultResponse.POV.OEXList = new Dol.PoskytnutieZoznamuIFOPodlaVyhladavacichKriteriiWS.TOEX_OEIO[0];
                    else
                        throw new Exception(resultResponse != null && resultResponse.POV != null ? "Popis chyby z RFO pri volani PoskytnutieZoznamuIFOPodlaVyhladavacichKriteriiWSRequestToRfoService: " + resultResponse.POV.NU.Trim() : string.Empty);
                }
            }
            catch (Exception ex)
            {
                if (resultResponse == null)
                    resultResponse = new Dol.PoskytnutieZoznamuIFOPodlaVyhladavacichKriteriiWS.TransEnvTypeOut();
                resultResponse.POV = new Dol.PoskytnutieZoznamuIFOPodlaVyhladavacichKriteriiWS.TPOVO();
                resultResponse.POV.KO = -1;
                resultResponse.POV.NU = "Chyba pri spracovaní: " + ex.Message;

                ExceptionHandling.HandleBusinessException(this, ex, 202, MethodInfo.GetCurrentMethod().Name);

                if (throwExeption)
                    throw ex;
            }
            finally
            {
                //Zapise sa logWS z volania RFO
                if (Tools.ShouldLogXmlMessages)
                    this.UlozLogWS();
            }
            return resultResponse;
        }

        /// <summary>
        /// RFOWS UC 011 Poskytnutie referenčných údajov zoznamu IFO online
        /// </summary>
        /// <param name="ZoznamIFO"></param>
        /// <param name="PozadovaneSekcie"></param>
        /// <returns></returns>
        public Dol.PoskytnutieUdajovIFOOnlineWS.TransEnvTypeOut PoskytnutieReferencnychUdajovZoznamuIFOOnline(List<string> ZoznamIFO, List<int> PozadovaneSekcie, bool throwExeption = false)
        {
            Dol.PoskytnutieUdajovIFOOnlineWS.TransEnvTypeOut output = null;
            var informationalWS = new InformationalWS();
            try
            {
                var inputList = new List<Dol.PoskytnutieUdajovIFOOnlineWS.TransEnvTypeIn>();
                Dol.PoskytnutieUdajovIFOOnlineWS.TransEnvTypeIn input = null;
                var iosList = new List<Dol.PoskytnutieUdajovIFOOnlineWS.TIOS>();
                for (int i = 0; i < ZoznamIFO.Count; i++)
                {
                    var item = ZoznamIFO[i];
                    int reminder;
                    //zistim, ci je zvysok po deleni 10, kazdych 10 poloziek budem volat v samostatnej davke
                    Math.DivRem(i, 10, out reminder);
                    //V XSD je väzba 1..*, avšak rozhranie definuje podmienku, že môže byť zadaných max. 10 IFO
                    if (reminder == 0)
                    {
                        //vyprazdnim si zoznam, ak tam nieco bolo
                        iosList.Clear();

                        input = new Dol.PoskytnutieUdajovIFOOnlineWS.TransEnvTypeIn();
                        input.POD = new Dol.PoskytnutieUdajovIFOOnlineWS.TPOD();
                        input.POD.UES = new Dol.PoskytnutieUdajovIFOOnlineWS.TUES();
                        input.POD.UES.PO = Tools.RisUser; //(((System.Security.Principal.GenericPrincipal)(Principal)).Identity).Name;
                        input.POD.UES.TI = this.TransactionID == null ? "prazdne" : this.TransactionID.ToString();

                        /*
                         * Systém vytvorí dátovú štruktúru pre Poskytnutie referenčných údajov zoznamu IFO online

                        Sekcia "RFO WS Poskytnutie odpisu - vstup"
                        \IFO = Vstupný parameter

                        Vytvoria sa 4 sekcie pre zadanie požadovaných informácií:
                        Ak v "Požadované sekcie" nie sú zadané čísla požadovaných sekcií, tak sa vytvorie všetky z nasledujúcich sekcií, ináć tie, ktoré sú uvedené v "Požadované sekcie":

                        Sekcie:
                        - v prvej sekcii
                        \Kód = 1
                        \Názov = "Administratívne údaje"
                        - v druhej sekcii
                        \Kód = 2
                        \Názov = "Lokačné údaje"
                        - v tretej sekcii
                        \Kód = 3
                        \Názov =  "Vzťahové údaje"
                        - vo štvtrej sekcii
                        \Kód = 4
                        \Názov = "Identifikačné údaje"

                         */

                        if (PozadovaneSekcie == null || PozadovaneSekcie.Count == 0)
                        {
                            input.POD.SPIList = new Dol.PoskytnutieUdajovIFOOnlineWS.TSPI_SPN[]
                        {
                            Tools.GetTSPI_SPN(1),
                            Tools.GetTSPI_SPN(2),
                            Tools.GetTSPI_SPN(3),
                            Tools.GetTSPI_SPN(4)
                        };
                        }
                        else
                        {
                            input.POD.SPIList = new Dol.PoskytnutieUdajovIFOOnlineWS.TSPI_SPN[PozadovaneSekcie.Count];
                            for (int a = 0; a < PozadovaneSekcie.Count; a++)
                                input.POD.SPIList[a] = Tools.GetTSPI_SPN(PozadovaneSekcie[a]);
                        }

                        //pridam si do zoznamu, ktory potom budem volat na RFO
                        inputList.Add(input);
                    }

                    var ios = new Dol.PoskytnutieUdajovIFOOnlineWS.TIOS();
                    //V prípade, že je naplnený atribút "RFO WS Údaje o osobe.IFO pravej osoby" zobrazí sa tento. Ak tento atribút nie je naplnený, potom sa zobrazí hodnota z "RFO WS Údaje o osobe.IFO"
                    ios.IF = item;
                    iosList.Add(ios);

                    //bud mam zoznam desiatich, alebo koncim v zozname osob
                    if (reminder + 1 == 10 || (i + 1) == ZoznamIFO.Count)
                    {
                        input.POD.IOSList = iosList.ToArray();
                    }
                }

                output = new Dol.PoskytnutieUdajovIFOOnlineWS.TransEnvTypeOut();
                output.POV = new Dol.PoskytnutieUdajovIFOOnlineWS.TPOVO() { AC = DateTime.Now, KO = 1, NU = String.Empty };
                var OEXList = new List<Dol.PoskytnutieUdajovIFOOnlineWS.TOEX_OEIO>();
                //Teraz pre kazdu sadu max 10 IFO zavolam WS
                foreach (var item in inputList)
                {
                    try
                    {
                        //zavolam si server po davkach po 10
                        var resultResponse = informationalWS.PoskytnutieReferencnychUdajovZoznamuIFOOnlineWSRequestToRfoService(item);

                        //this.ZapisVystupXML(resultResponse);
                        if (resultResponse == null || resultResponse.POV == null || resultResponse.POV.KO != 1)
                        {
                            output.POV = (resultResponse == null || resultResponse.POV == null ? new Dol.PoskytnutieUdajovIFOOnlineWS.TPOVO() { AC = DateTime.Now, KO = -1, NU = "Chyba pri volani WS PoskytnutieReferencnychUdajovZoznamuIFOOnlineWSRequestToRfoService" } : resultResponse.POV);
                            ExceptionHandling.HandleBusinessException(this, 2031, Environment.NewLine + "Popis chyby z RFO pri volani PoskytnutieReferencnychUdajovZoznamuIFOOnlineWSRequestToRfoService: " + output.POV.NU);

                            //zalogovanie kazdej samostatnej chyby
                            if (output.POV.OEXList != null)
                                foreach (var oex in output.POV.OEXList)
                                {
                                    //zaznacim chyby pre jednotlive IFO
                                    if (oex.NK != 1)
                                        ExceptionHandling.HandleBusinessException(this, 2032, Environment.NewLine + "Vstupny parameter: " + oex.ID + Environment.NewLine + "Chybová správa: " + oex.DP);
                                }
                        }

                        OEXList.AddRange(resultResponse.POV.OEXList);
                    }
                    catch (Exception ex)
                    {
                        if (throwExeption)
                            throw ex;
                    }
                }

                //zo zoznamu sprav musim vytvorit jednu velku spravu
                output.POV.OEXList = OEXList.ToArray();

                //System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(output.GetType());
                //System.IO.TextWriter WriteFileStream = new System.IO.StreamWriter(@"d:\Projects\RIS2\Trunk\Code\RIS2\Test\RIS.Svc.Wcf.Test\output" + DateTime.Now.ToString("HH-mm-ss") + ".xml");
                //serializer.Serialize(WriteFileStream, output);
                //WriteFileStream.Close();

                return output;
            }
            catch (Exception ex)
            {
                ExceptionHandling.HandleBusinessException(this, ex, 203, MethodInfo.GetCurrentMethod().Name);

                if (throwExeption)
                    throw ex;

                return null;
            }
            finally
            {
                //Zapise sa logWS z volania RFO
                if (Tools.ShouldLogXmlMessages)
                    this.UlozLogWS();
            }
        }


        public Dol.ZoznamIFOSoZmenenymiReferencnymiUdajmiWS.TransEnvTypeOut ZoznamIFOSoZmenenymiReferencnymiUdajmiWS()
        {
            try
            {
                var informationalWS = new InformationalWS();
                var transEnvTypeIn = new Ditec.RIS.RFO.Dol.ZoznamIFOSoZmenenymiReferencnymiUdajmiWS.TransEnvTypeIn();
                transEnvTypeIn.POD = new Dol.ZoznamIFOSoZmenenymiReferencnymiUdajmiWS.TPOD();
                transEnvTypeIn.POD.UD = true;
                transEnvTypeIn.POD.UES = new Dol.ZoznamIFOSoZmenenymiReferencnymiUdajmiWS.TUES();
                transEnvTypeIn.POD.UES.PO = Tools.RisUser;
                transEnvTypeIn.POD.UES.TI = this.TransactionID == null ? "prazdne" : this.TransactionID.ToString();

                return informationalWS.PoskytnutieZoznamuIFOSoZmenenymiReferencnymiUdajmiWSRequestToRfoService(transEnvTypeIn);
            }
            catch (Exception ex)
            {
                ExceptionHandling.HandleBusinessException(this, ex, 213, MethodInfo.GetCurrentMethod().Name);
                throw;
            }
            finally
            {
                //Zapise sa logWS z volania RFO
                if (Tools.ShouldLogXmlMessages)
                    this.UlozLogWS();
            }
        }

        /// <summary>
        /// RFOWS UC 013 Označenie záujmovej osoby
        /// Modul prijme požiadavku pre označenie konkrétnej osoby v RFO ako záujmovej na základe jej identifikátora v RFO (IFO)
        ///Vstupným parametrom je:
        ///"IFO"
        /// </summary>
        /// <param name="IFO">zaujmova osoba</param>
        public Dol.OznacenieZaujmovejOsobyWS.TransEnvTypeOut OznacenieZaujmovejOsoby(List<string> IFOList, Osoba osoba = null, bool throwExeption = false)
        {
            Dol.OznacenieZaujmovejOsobyWS.TransEnvTypeOut output = null;
            var transactionalWS = new TransactionalWS();
            try
            {
                //ak som na vyvoji koncim
                if (!Tools.OznacenieZaujmovejOsoby)
                    return output;

                var input = new Dol.OznacenieZaujmovejOsobyWS.TransEnvTypeIn();
                input.OSOList = new Dol.OznacenieZaujmovejOsobyWS.TOSO[IFOList.Count];
                for (int i = 0; i < IFOList.Count; i++)
                {
                    input.OSOList[i] = new Dol.OznacenieZaujmovejOsobyWS.TOSO() { ID = IFOList[i] };
                }
                input.UES = new Dol.OznacenieZaujmovejOsobyWS.TUES();
                input.UES.PO = Tools.RisUser;
                input.UES.TI = this.TransactionID.ToString();
                output = transactionalWS.OznacenieZaujmovejOsobyWSToRfoService(input);

                if (output == null || output.ZVY == null || output.ZVY.NK != 1)
                {
                    output.ZVY = (output.ZVY == null ? new Dol.OznacenieZaujmovejOsobyWS.TZVYO() { NK = -1, DN = "Chyba pri volani WS OznacenieZaujmovejOsobyWSToRfoService" } : output.ZVY);

                    //zalogovanie kazdej samostatnej chyby
                    if (output.ZVY.OEXList != null)
                        foreach (var item in output.ZVY.OEXList)
                        {
                            //zaznacim chyby pre jednotlive IFO
                            if (item.NK != 1)
                                ExceptionHandling.HandleBusinessException(this, 2042, Environment.NewLine + "Vstupny parameter: " + item.ID + Environment.NewLine + "Chybová správa: " + item.DE);
                        }

                    throw new Exception(Environment.NewLine + "Popis chyby z RFO pri volani OznacenieZaujmovejOsobyWSToRfoService: " + output.ZVY.DN);
                }

                if (osoba != null)
                {
                    osoba.FyzickaOsoba.ZaujmovaOsoba = true;
                    //update FO
                    GetDataAccessLayer<ICRUD<Ditec.RIS.RFO.Dol.FyzickaOsoba>>().Update(osoba.FyzickaOsoba);
                }

                return output;
            }
            catch (Exception ex)
            {
                ExceptionHandling.HandleBusinessException(this, ex, 204, MethodInfo.GetCurrentMethod().Name);

                if (throwExeption)
                    throw ex;

                return null;
            }
            finally
            {
                //Zapise sa logWS z volania RFO
                if (Tools.ShouldLogXmlMessages)
                    this.UlozLogWS();
            }
        }

        /// <summary>
        /// RFOWS UC 014 Zrušenie označenia záujmovej osoby
        /// Modul prijme požiadavku pre zrušenie označenia konkrétnej osoby v RFO ako záujmovej na základe jej identifikátora v RFO (IFO)
        ///Vstupným parametrom je:
        ///"IFO"
        /// </summary>
        /// <param name="IFOList"></param>
        /// <returns></returns>
        public Dol.ZrusenieOznaceniaZaujmovejOsoby.TransEnvTypeOut ZrusenieOznaceniaZaujmovejOsoby(List<string> IFOList, bool throwExeption = false)
        {
            Dol.ZrusenieOznaceniaZaujmovejOsoby.TransEnvTypeOut output = null;
            var transactionalWS = new TransactionalWS();
            try
            {
                var input = new Dol.ZrusenieOznaceniaZaujmovejOsoby.TransEnvTypeIn();
                input.OSOList = new Dol.ZrusenieOznaceniaZaujmovejOsoby.TOSO[IFOList.Count];
                for (int i = 0; i < IFOList.Count; i++)
                {
                    input.OSOList[i] = new Dol.ZrusenieOznaceniaZaujmovejOsoby.TOSO() { ID = IFOList[i] };
                }
                input.UES = new Dol.ZrusenieOznaceniaZaujmovejOsoby.TUES();
                input.UES.PO = Tools.RisUser;
                input.UES.TI = this.TransactionID.ToString();

                output = transactionalWS.ZrusenieOznaceniaZaujmovejOsobyWSToRfoService(input);
                if (output == null || output.VSP == null || output.VSP.KI != 1)
                {
                    output.VSP = (output.VSP == null ? new Dol.ZrusenieOznaceniaZaujmovejOsoby.TVSPO() { KI = -1, PO = "Chyba pri volani WS ZrusenieOznaceniaZaujmovejOsoby" } : output.VSP);

                    //zalogovanie kazdej samostatnej chyby
                    if (output.VSP.OEXList != null)
                        foreach (var item in output.VSP.OEXList)
                        {
                            //zaznacim chyby pre jednotlive IFO
                            if (item.NK != 1)
                                ExceptionHandling.HandleBusinessException(this, 2052, Environment.NewLine + "Vstupny parameter: " + item.ID + Environment.NewLine + "Chybová správa: " + item.DR);
                        }

                    throw new Exception(Environment.NewLine + "Popis chyby z RFO pri volani ZrusenieOznaceniaZaujmovejOsobyWSToRfoService: " + output.VSP.PO);
                }
                return output;
            }
            catch (Exception ex)
            {
                ExceptionHandling.HandleBusinessException(this, ex, 205, MethodInfo.GetCurrentMethod().Name);
                if (throwExeption)
                    throw ex;

                return null;
            }
            finally
            {
                //Zapise sa logWS z volania RFO
                if (Tools.ShouldLogXmlMessages)
                    this.UlozLogWS();
            }
        }

        /// <summary>
        /// RFOWS UC 015 Potvrdzovanie prijatia zmien
        /// Modul prijme požiadavku pre potvrdenie prijatia a spracovania zmien údajov osoby z RFO v zmenovej dávke
        /// Vstupným parametrom je:
        ///"Identifikátor zmeny pre externý systém"
        /// </summary>
        /// <param name="IdentifikatorPrijatejDavky"></param>
        /// <returns></returns>
        public Dol.PotvrdzovaniePrijatiaZmienWS.TransEnvTypeOut PotvrdzovaniePrijatiaZmien(long IdentifikatorPrijatejDavky)
        {
            var informationalWS = new InformationalWS();
            Dol.PotvrdzovaniePrijatiaZmienWS.TransEnvTypeOut output = null;
            try
            {
                var input = new Dol.PotvrdzovaniePrijatiaZmienWS.TransEnvTypeIn();
                input.PPZ = new Dol.PotvrdzovaniePrijatiaZmienWS.TPPZ_ZZV();
                input.PPZ.IP = IdentifikatorPrijatejDavky;

                input.UES = new Dol.PotvrdzovaniePrijatiaZmienWS.TUES();
                input.UES.PO = Tools.RisUser;
                input.UES.TI = this.TransactionID.ToString();

                output = informationalWS.PotvrdzovaniePrijatiaZmienWSToRfoService(input);

                if (output == null || output.VSP == null || output.VSP.KI != 1)
                {
                    output.VSP = (output.VSP == null ? new Dol.PotvrdzovaniePrijatiaZmienWS.TVSPO() { KI = -1, PO = "Chyba pri volani WS PotvrdzovaniePrijatiaZmien" } : output.VSP);
                    throw new Exception("Vstupny parameter: " + IdentifikatorPrijatejDavky + Environment.NewLine + "Popis chyby z RFO pri volani PotvrdzovaniePrijatiaZmienWSToRfoService: " + output.VSP.PO);
                }
                return output;
            }
            catch (Exception ex)
            {
                ExceptionHandling.HandleBusinessException(this, ex, 206, MethodInfo.GetCurrentMethod().Name);
                return null;
            }
            finally
            {
                //Zapise sa logWS z volania RFO
                if (Tools.ShouldLogXmlMessages)
                    this.UlozLogWS();
            }
        }

        /// <summary>
        /// RFO RIS Nájdenie osoby v RIS pod¾a IFO a IFO pravej osoby
        /// </summary>
        /// <param name="IdentifikatorPrijatejDavky"></param>
        /// <returns></returns>
        public NajdenaOsoba NajdenieOsobyVRISPodlaIFOaIFOPravejOsoby(string IFO, string IFOPravejOsoby, bool throwExeption = false)
        {
            var result = new NajdenaOsoba();
            try
            {
                // zo ServiceLocatora sa ziska trieda implementujuca rozhranie 
                var dataOperationFOBrowse = GetDataAccessLayer<IBrowse<FyzickaOsoba>>();
                var dataOperationStotoznenaFO = GetDataAccessLayer<IBrowse<StotoznenaFyzOsoba>>();
                var dataOperationStotoznenaFindIfo = GetDataAccessLayer<IStotoznenaFyzOsoba>();
                var dataoperationFindFO = GetDataAccessLayer<IFyzickaOsoba>();
                var dataOperationFO = GetDataAccessLayer<ICRUD<FyzickaOsoba>>();

                IList<FyzickaOsoba> osobaList;
                IList<StotoznenaFyzOsoba> stotoznenaFOList;
                //Pomocné premenné:
                //"Nájdená" = FALSE
                //"Je zadaný Typ osoby v RIS" = FALSE
                //Ak "IFO pravej osoby" = NULL ( fyzická osoba je v RFO nie je stotožnená s inou  fyzickou osobou)
                if (String.IsNullOrEmpty(IFOPravejOsoby) && !String.IsNullOrEmpty(IFO))
                {
                    //Systém v RIS vyh¾adá identifikátor EDUID nájdenej  fyzickej osoby pod¾a IFO 
                    //osobaList = GetDataAccessLayer<IFyzickaOsoba>().FyzickaOsobaIfoList(new FyzickaOsobaFilterCriteria() { Ifo = IFO, Zneplatnena = false });
                    osobaList = dataoperationFindFO.FyzickaOsobaIfoList(new FyzickaOsobaFilterCriteria() { Ifo = IFO, Zneplatnena = false });
                    //Ak  "EDUID" <> NULL  (v RIS príslušná  fyzická osoba bola nájdená )
                    if (osobaList.ExistFyzickaOsoba())
                    {
                        //"Nájdená" = TRUE
                        //"Typ nájdenia"  = "Nájdená"
                        //"EDUIDNájdené" = "EDUID"
                        result = new NajdenaOsoba() { Najdena = true, TypNajdenia = TypNajdenia.Najdena, EDUIDNajdene = osobaList[0].Eduid, FyzickaOsobaNajdena = osobaList[0] };

                        //5.    Ak "EDUIDNájdené" <> NULL 
                        //Systém zistí sa, či nájdená  fyzická osoba má zadaný Typ osoby v RIS: "
                        result.TypOsobyVRISList = this.MaZadanyTypOsobyRIS(osobaList[0].Eduid);
                    }
                    //Ináč  Ak  "EDUID" = NULL (v RIS príslušná  fyzická osoba nebola nájdená)
                    else
                    {
                        //Systém v RIS skúsi vyhľadať či  fyzická osoba s daným "IFO " nie je pôvodná  fyzická osoba, 
                        //ktorá bola stotožnená s inou  fyzickou osobou. t.j. či k danému "IFO" existuje  "Pravé IFO"
                        //Vyhľadá sa Osoby.Fyzická osoba.IFO, pre ktorú platí:
                        //Osoby.Fyzická osoba.EDUID = Osoby.Stotožnené osoby.EDUID a 
                        //Oosoby.Stotožnené osoby.IFO pôvodnej osoby = "IFOPôvodné"
                        stotoznenaFOList = dataOperationStotoznenaFindIfo.StotoznenaFyzOsobaIfoList(new StotoznenaFyzOsobaFilterCriteria() { IfoPovodne = IFO });
                        //Ak "Pravé IFO" <> NULL ( podarilo sa  v RIS nájsť  fyzickú osobu, ktorej predchádzajúce IFO je zhodné so zadaným "IFO", t.j. došlo na strane RFO k zrušeniu stotožnenia  osôb) 
                        if (stotoznenaFOList.ExistStotoznenaFyzOsoba())
                        {
                            ////vratim si udaju povodnej osoby, ktora je aktualne zneplatnena (je stotoznena), budem rusit stotoznenie
                            //osobaList = dataOperationFO.Browse(new FyzickaOsobaFilterCriteria() { ID = stotoznenaFOList[0].FyzickaOsobaPovodnaId });
                            //Načíta sa EDUID osoby , ktorá ma IFO = "Pravé IFO"
                            var stotoznenaFO = dataOperationFO.Read(new FyzickaOsobaFilterCriteria() { ID = stotoznenaFOList[0].FyzickaOsobaPovodnaId }); //pride mi to, ze sa ma vratit EDUID pravej osoby, ktora je aktualne platna medzi FO
                            var pravaFO = dataOperationFO.Read(new FyzickaOsobaFilterCriteria() { ID = stotoznenaFOList[0].FyzickaOsobaId });
                            //"Nájdená" = TRUE
                            //"Typ nájdenia"  = "Terajšia nenájdená, existuje pôvodná"
                            //"EDUIDNájdené" = "EDUID"
                            result = new NajdenaOsoba() { Najdena = true, TypNajdenia = TypNajdenia.TerajsiaNenajdenaExistujePovodna, EDUIDNajdene = pravaFO.Eduid, FyzickaOsobaNajdena = pravaFO, EDUIDPovodna = stotoznenaFO.Eduid, FyzickaOsobaPovodna = stotoznenaFO };

                            //5.    Ak "EDUIDNájdené" <> NULL 
                            //Systém zistí sa, či nájdená  fyzická osoba má zadaný Typ osoby v RIS: "
                            result.TypOsobyVRISList = this.MaZadanyTypOsobyRIS(pravaFO.Eduid);
                        }
                        //Ináč Ak "Pravé IFO" = NULL  (Daná osoba sa v systéme nenachádza. ) - nenasiel sa zaznam ani medzi povodnymi
                        else
                        {
                            //"Nájdená" = FALSE
                            //"Typ nájdenia"  = "Terajšia nenájdená
                            //"EDUIDNájdené" = NULL
                            result = new NajdenaOsoba() { Najdena = false, TypNajdenia = TypNajdenia.TerajsiaNenajdena };
                        }
                    }
                }
                //Ináč Ak "IFO pravej osoby" <>NULL (fyzická osoba je v RFO stotožnená s inou osobou)
                else if (!String.IsNullOrEmpty(IFOPravejOsoby) && !String.IsNullOrEmpty(IFO))
                {
                    //Systém vyhľadá fyzickú osobu, ktrej IFO = "IFO pravej osoby ((zistenie, či v RIS existuje fyzická osoba, 
                    //s ktorou sa má osoba stotožniť - existuje pravá osoba)
                    osobaList = dataoperationFindFO.FyzickaOsobaIfoList(new FyzickaOsobaFilterCriteria() { Ifo = IFOPravejOsoby, Zneplatnena = false });
                    //Ak  "EDUID" <> NULL (v RIS existuje  fyzická osoba, s ktorou je osoba z RFO stotožnená)
                    if (osobaList.ExistFyzickaOsoba())
                    {
                        var pravaRFOOsoba = osobaList[0];
                        //Systém vyhľadá  fyzickú osobu, ktrej IFO = "IFO" ((zistí sa, či v RIS existuje  fyzická osoba, s ktorá sa má stotožniť - pôvodná osoba )
                        osobaList = dataoperationFindFO.FyzickaOsobaIfoList(new FyzickaOsobaFilterCriteria() { Ifo = IFO, Zneplatnena = false });
                        //Ak EDUIDPovodné <> NULL (existuje pôvodná  fyzická osoba)
                        if (osobaList.ExistFyzickaOsoba())
                        {
                            var naStotoznenieOsoba = osobaList[0];
                            //4.1.1.1.1
                            //"Nájdená" = TRUE
                            //"Typ nájdenia"  = "Stotožnená nájdená, existuje pôvodná"
                            //"EDUIDNájdené" = "EDUID"
                            result = new NajdenaOsoba() { Najdena = true, TypNajdenia = TypNajdenia.PravaNajdenaExistujePovodna, EDUIDNajdene = pravaRFOOsoba.Eduid, FyzickaOsobaNajdena = pravaRFOOsoba, EDUIDPovodna = naStotoznenieOsoba.Eduid, FyzickaOsobaPovodna = naStotoznenieOsoba };

                            //5.    Ak "EDUIDNájdené" <> NULL 
                            //Systém zistí sa, či nájdená  fyzická osoba má zadaný Typ osoby v RIS: "
                            result.TypOsobyVRISList = this.MaZadanyTypOsobyRIS(result.FyzickaOsobaNajdena.Eduid);
                        }
                        //Ináč ak EDUIDPovodné = NULL (neexistuje pôvodná  fyzická osoba)
                        else
                        {
                            //Systém  skúsi vyhľadať či  fyzická osoba s daným "IFO " nie je medzi stotožnenými osobami
                            //Vyhľadá sa Osoby.Fyzická osoba.IFO, pre ktorú platí:
                            //Osoby.Fyzická osoba.EDUID = Osoby.Stotožnené osoby.EDUID a 
                            //Oosoby.Stotožnené osoby.IFO pôvodnej osoby = "IFOPôvodné"
                            stotoznenaFOList = dataOperationStotoznenaFindIfo.StotoznenaFyzOsobaIfoList(new StotoznenaFyzOsobaFilterCriteria() { IfoPovodne = IFO });
                            //4.1.1.2.1.1 Ak "Pravé IFO" <> NULL, t.j. IFO sa naślo medzi stotožnenými IFO
                            if (stotoznenaFOList.ExistStotoznenaFyzOsoba())
                            {
                                var osoba = GetDataAccessLayer<ICRUD<FyzickaOsoba>>().Read(new FyzickaOsobaFilterCriteria() { ID = stotoznenaFOList[0].FyzickaOsobaId }); //naviazana osoba ma prave IFO
                                //Ak "Pravé IFO" ="IFO pravej osoby"
                                if (osoba.Ifo == IFOPravejOsoby) // == pravaRFOOsoba
                                {
                                    //"Nájdená" = TRUE
                                    //"Typ nájdenia"  = "Existujúce stotožnenie"
                                    //"EDUIDNájdené" = "EDUID"
                                    result = new NajdenaOsoba() { Najdena = true, TypNajdenia = TypNajdenia.ExistujuceStotoznenie, EDUIDNajdene = pravaRFOOsoba.Eduid, FyzickaOsobaNajdena = pravaRFOOsoba, EDUIDPovodna = osoba.Eduid, FyzickaOsobaPovodna = osoba };

                                    //5.    Ak "EDUIDNájdené" <> NULL 
                                    //Systém zistí sa, či nájdená  fyzická osoba má zadaný Typ osoby v RIS: "
                                    result.TypOsobyVRISList = this.MaZadanyTypOsobyRIS(pravaRFOOsoba.Eduid);
                                }
                                //Ináè Ak "Pravé IFO" <> "IFO pravej osoby"
                                else
                                {
                                    //"Nájdená" = TRUE
                                    //"Typ nájdenia"  = "Stotožnená ns inou osobou"
                                    //"EDUIDNájdené" = "EDUID"
                                    result = new NajdenaOsoba() { Najdena = true, TypNajdenia = TypNajdenia.StotoznenaSInouOsobou, EDUIDNajdene = pravaRFOOsoba.Eduid, FyzickaOsobaNajdena = pravaRFOOsoba, EDUIDPovodna = osoba.Eduid, FyzickaOsobaPovodna = osoba };

                                    //5.    Ak "EDUIDNájdené" <> NULL 
                                    //Systém zistí sa, či nájdená  fyzická osoba má zadaný Typ osoby v RIS: "
                                    result.TypOsobyVRISList = this.MaZadanyTypOsobyRIS(pravaRFOOsoba.Eduid);
                                }
                            }
                            //Ináè ak "Pravé IFO" = NULL (neexistuje  fyzická osoba, ktorej IFO je zaznamenané v RISe)
                            else
                            {
                                //"Nájdená" = TRUE
                                //"Typ nájdenia"  = "Stotožnená nájdená, neexistuje pôvodná"
                                //"EDUIDNájdené" = "EDUID"
                                result = new NajdenaOsoba() { Najdena = true, TypNajdenia = TypNajdenia.PravaNajdenaNeexistujePovodna, EDUIDNajdene = pravaRFOOsoba.Eduid, FyzickaOsobaNajdena = pravaRFOOsoba };

                                //5.    Ak "EDUIDNájdené" <> NULL 
                                //Systém zistí sa, či nájdená  fyzická osoba má zadaný Typ osoby v RIS: "
                                result.TypOsobyVRISList = this.MaZadanyTypOsobyRIS(pravaRFOOsoba.Eduid);
                            }
                        }
                    }
                    //Ináč ak v RIS neexistuje  fyzická osoba, ktorej "Osoba.IFO" = "IFO pravej osoby" (V RIS neexistuje  fyzická osoba, s ktorou je osoba z RFO stotožnená.)
                    else
                    {
                        //Systém vyhľadá  fyzickú osobu, ktrej IFO = "IFO" (vyhľadá  fyzickú osobu s pôvodným IFO)
                        osobaList = dataoperationFindFO.FyzickaOsobaIfoList(new FyzickaOsobaFilterCriteria() { Ifo = IFO, Zneplatnena = false });
                        //Ak  "EDUID" <> NULL (existuje taká  fyzická osoba)
                        if (osobaList.ExistFyzickaOsoba())
                        {
                            //"Nájdená" = TRUE
                            //"Typ nájdenia"  = "Stotožnená nenájdená, existuje pôvodná"
                            //"EDUIDNájdené" = "EDUID"
                            result = new NajdenaOsoba() { Najdena = true, TypNajdenia = TypNajdenia.PravaNenajdenaExistujePovodna, EDUIDNajdene = osobaList[0].Eduid, FyzickaOsobaNajdena = osobaList[0] };

                            //5.    Ak "EDUIDNájdené" <> NULL 
                            //Systém zistí sa, či nájdená  fyzická osoba má zadaný Typ osoby v RIS: "
                            result.TypOsobyVRISList = this.MaZadanyTypOsobyRIS(osobaList[0].Eduid);
                        }
                        //Ináč Ak  "EDUID" = NULL, t.j. neexistuje  fyzická osoba v RIS, ktorej IFO = "IFO"
                        else
                        {
                            //"Nájdená" = FALSE
                            //"Typ nájdenia"  = "Stotožnená nenájdená, neexistuje pôvodná"
                            //"EDUIDNájdené" = NULL
                            result = new NajdenaOsoba() { Najdena = false, TypNajdenia = TypNajdenia.PravaNenajdenaNeexistujePovodna, EDUIDNajdene = null };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandling.HandleBusinessException(this, ex, 207, MethodInfo.GetCurrentMethod().Name);

                if (throwExeption)
                    throw ex;
            }
            return result;
        }

        /// <summary>
        /// Modul prijme požiadavku na zápis aktualizácie údajov fyzickej osoby v RIS na základe údajov osoby z RFO. 
        /// Vstupné parametre:
        ///- štruktúra údajov , ktorá sa získala na základe predchádzajúcich volaní RFO - RFO WS Údaje o osobe
        ///- "Vzťahové" - príznak, či sa majú načítavať vzťahové osoby 
        ///- "Zápis novej osoby" - príznak, či sa má zapísať nová osoba 
        ///- "Typ osoby v RIS" -Typ osoby v RIS, ktorý bude priradený novozapísanej osobe. Berie sa do úvahy len vtedy, ak je príznak "Zápis novej osoby" = TRUE
        ///-Výstupné paramatre:
        ///- "Novozapísaná osoba"  - príznak, či spracovávaná fyzická osoba je novozapísaná
        ///- EDUID
        /// </summary>
        private List<AktualizovanaOsoba> AktualizaciaUdajovFyzickejOsobyUdajmiRFO(OsobaResponse OsobaResponse, bool Vztahove, bool ZapisNovejOsoby, TypOsobyVRis? TypOsobyVRis, bool ZapisNovejOsobyNaZakladeOsobyRFO = false)
        {
            var retVal = new List<AktualizovanaOsoba>();
            try
            {

                foreach (var osoba in OsobaResponse.OsobaList)
                {
                    retVal.Add(this.AktualizaciaUdajovFyzickejOsobyUdajmiRFO(osoba, Vztahove, ZapisNovejOsoby, TypOsobyVRis, ZapisNovejOsobyNaZakladeOsobyRFO));
                }
            }
            catch (Exception ex)
            {
                ExceptionHandling.HandleBusinessException(this, ex, 208, MethodInfo.GetCurrentMethod().Name);
                throw ex;
            }

            return retVal;
        }

        private AktualizovanaOsoba AktualizaciaUdajovFyzickejOsobyUdajmiRFO(Osoba Osoba, bool Vztahove, bool ZapisNovejOsoby, TypOsobyVRis? TypOsobyVRis, bool ZapisNovejOsobyNaZakladeOsobyRFO = false)
        {
            AktualizovanaOsoba retVal = new AktualizovanaOsoba();
            try
            {
                string ifoPovodne = null;
                var dataOperationFO = GetDataAccessLayer<ICRUD<FyzickaOsoba>>();

                var rfoTitulList = GetBussinessLogicLayer<IBFCodeList>().GetBufferedCodeList<RfoTitul>();

                //Pomocné premenné:
                //"IFO" = "RFO WS Údaje o osobe.IFO"
                //"IFO pravej osoby" = "RFO WS Údaje o osobe.IFOpravej osoby"
                //"IFO pôvodné" = NULL
                //"Novozapísaná osoba" = FALSE
                ifoPovodne = null;
                retVal.Novozapisana = false;

                //3.    Systém  zistí, či v RIS už je zadaná fyzická osoba  zapísaná (jednotlivé možné varianty zápisu a stotožnenia sú popísané v UC  RFO 050 Nájdenie osoby v RIS podľa IFO a IFO pravej osoby) 
                var najdenaOsoba = this.NajdenieOsobyVRISPodlaIFOaIFOPravejOsoby(Osoba.FyzickaOsoba.Ifo, Osoba.FyzickaOsoba.IfoPravejOsoby);

                //3.1   Ak "Nájdená" = TRUE (daná fyzická osoba je zapísaná)
                if (najdenaOsoba.Najdena)
                {
                    //Ak "Typ nájdenia" = "Nájdená"
                    if (najdenaOsoba.TypNajdenia == TypNajdenia.Najdena)
                    {
                        //"EDUID" = "EDUIDNájdené"
                        retVal.EDUID = najdenaOsoba.EDUIDNajdene;
                    }
                    //Ak Typ nájdenia" = "Existujúce stotožnenie"
                    else if (najdenaOsoba.TypNajdenia == TypNajdenia.ExistujuceStotoznenie)
                    {
                        retVal.EDUID = najdenaOsoba.EDUIDNajdene;
                        //"IFO" = "IFO pravej osoby"
                        Osoba.FyzickaOsoba.Ifo = Osoba.FyzickaOsoba.IfoPravejOsoby;
                    }
                    //3.1.3 Ak "Typ nájdenia" = "Stotožnená s inou osobou" ,t.j. Nemalo by nastať
                    else if (najdenaOsoba.TypNajdenia == TypNajdenia.StotoznenaSInouOsobou)
                    {
                        //Systém ukončí UC, vráti návratový kód "Neplatné stotožnenie"
                        retVal.ErrorMessage = "Neplatné stotožnenie";
                        //zapise sa aj do logu
                        ExceptionHandling.HandleBusinessException(this, new Exception(retVal.ErrorMessage), 2108, MethodInfo.GetCurrentMethod().Name + ": " + retVal.ErrorMessage);
                        //throw new Exception("Neplatné stotožnenie");
                    }
                    //3.1.4  Ak "Typ nájdenia" = "Stotožnená nájdená, existuje pôvodná" (osoba je stotožnená, existuje aj pôvodná osoba)
                    else if (najdenaOsoba.TypNajdenia == TypNajdenia.PravaNajdenaExistujePovodna)
                    {
                        //"FO pôvodné"  = "IFO"
                        ifoPovodne = Osoba.FyzickaOsoba.Ifo;

                        //Systém stotožní fyzické osoby v RISE: Stotožnenie osoby  kde "Osoba.EDUID" = "EDUIDPovodné"s osobou  "Osoba.EDUID" = "EDUID" 
                        this.StotoznenieOsobRIS(najdenaOsoba.EDUIDNajdene.Value, najdenaOsoba.EDUIDPovodna, ifoPovodne, najdenaOsoba.FyzickaOsobaNajdena, najdenaOsoba.FyzickaOsobaPovodna);

                        //"Nájdená osoba" = fyzická osoba v RIS, ktorej IFO = "IFO pravej osoby"
                        //asi sa toto myslelo
                        retVal.EDUID = najdenaOsoba.FyzickaOsobaNajdena.Eduid;

                        //"IFO" = "IFO pravej osoby
                        Osoba.FyzickaOsoba.Ifo = Osoba.FyzickaOsoba.IfoPravejOsoby;
                    }
                    //3.1.5  Ak "Typ nájdenia" = "Stotožnená nájdená, neexistuje pôvodná"
                    else if (najdenaOsoba.TypNajdenia == TypNajdenia.PravaNajdenaNeexistujePovodna)
                    {
                        using (var transactionScope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
                        {
                            //"IFO pôvodné" = "IFO"
                            ifoPovodne = Osoba.FyzickaOsoba.Ifo;

                            //K fyzickej osobe, ktorej IFO = "IFO pravej osoby" sa zaznamená pôvodné (stotožnené) IFO
                            //(EDUID sa nezapisuje, lebo neexistovala pôvodná  fyzická osoba v RIS (fyzická osoba na stotožnenie)
                            //Vytvorim prazdnu FO s povodnym IFO
                            //Vytvorim vazbu cez tabulku StotoznenaFO s pravou (najdenou) FO
                            var povodnaFO = new FyzickaOsoba();
                            povodnaFO.Ifo = ifoPovodne;
                            povodnaFO.Zneplatnena = true;
                            povodnaFO.DatumNarodenia = new DateTime(1990, 1, 1);
                            povodnaFO.MenoZobrazovane = "x";
                            povodnaFO.PriezviskoZobrazovane = "x";
                            povodnaFO.ZaujmovaOsoba = true;
                            povodnaFO = dataOperationFO.Create(povodnaFO);
                            najdenaOsoba.FyzickaOsobaPovodna = povodnaFO;

                            this.StotoznenieOsobRIS(najdenaOsoba.EDUIDNajdene.Value, povodnaFO.Eduid, povodnaFO.Ifo, najdenaOsoba.FyzickaOsobaNajdena, povodnaFO);
                            //Zavolá sa označenie pôvodnej osoby ako záujmovej v RFO
                            this.OznacenieZaujmovejOsoby(new List<string> { ifoPovodne });
                            Osoba.FyzickaOsoba.ZaujmovaOsoba = true;

                            //"Nájdená osoba" =fyzická osoba v RIS, ktorej IFO = "IFO pravej osoby"
                            //asi sa toto myslelo
                            retVal.EDUID = najdenaOsoba.FyzickaOsobaNajdena.Eduid;

                            //"IFO" = "IFO pravej osoby"
                            Osoba.FyzickaOsoba.Ifo = Osoba.FyzickaOsoba.IfoPravejOsoby;

                            transactionScope.Complete();
                        }
                    }
                    //3.1.6 Ak "Typ nájdenia" = "Stotožnená nenájdená, existuje pôvodná"
                    else if (najdenaOsoba.TypNajdenia == TypNajdenia.PravaNenajdenaExistujePovodna)
                    {
                        //"IFOPôvodné"  = "IFO"
                        ifoPovodne = Osoba.FyzickaOsoba.Ifo;
                        //K nájdenej fyzickej osobe  sa zaznamená pôvodné (stotožnené) IFO
                        //(Fyzickej osobe sa prepíše IFO na IFO pravej osoby a pôvodné IFO sa zapíše do stotožnených osôb)
                        //vytvorim stotoznenie, na to budem musiet opat vytvorit prazdnu FO kvoli vazbe
                        var pravaFO = new FyzickaOsoba();
                        najdenaOsoba.FyzickaOsobaPovodna = najdenaOsoba.FyzickaOsobaNajdena;

                        using (var transactionScope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
                        {
                            //najdenaOsoba.FyzickaOsobaNajdena.Ifo = Osoba.FyzickaOsoba.IfoPravejOsoby;
                            //najdenaOsoba.FyzickaOsobaNajdena = dataOperationFO.Update(najdenaOsoba.FyzickaOsobaNajdena);

                            pravaFO.Ifo = Osoba.FyzickaOsoba.IfoPravejOsoby;
                            pravaFO.DatumNarodenia = new DateTime(1990, 1, 1);
                            pravaFO.MenoZobrazovane = "x";
                            pravaFO.PriezviskoZobrazovane = "x";
                            pravaFO = dataOperationFO.Create(pravaFO);
                            najdenaOsoba.FyzickaOsobaNajdena = pravaFO;

                            //vytvorim stotoznenie vazbu
                            this.StotoznenieOsobRIS(najdenaOsoba.FyzickaOsobaNajdena.Eduid, najdenaOsoba.FyzickaOsobaPovodna.Eduid, ifoPovodne, najdenaOsoba.FyzickaOsobaNajdena, najdenaOsoba.FyzickaOsobaPovodna);

                            //Systém označí  osobu s IFOm pravej osoby v RFO ako záujmovú
                            this.OznacenieZaujmovejOsoby(new List<string> { Osoba.FyzickaOsoba.IfoPravejOsoby });
                            Osoba.FyzickaOsoba.ZaujmovaOsoba = true;

                            //"Nájdená osoba" = osoba v RIS, ktorej IFO = "IFO pravej osoby"
                            //asi sa toto myslelo
                            retVal.EDUID = najdenaOsoba.FyzickaOsobaNajdena.Eduid;
                            //"IFO" = "IFO pravej osoby"

                            Osoba.FyzickaOsoba.Ifo = Osoba.FyzickaOsoba.IfoPravejOsoby;

                            transactionScope.Complete();
                        }
                    }
                    //3.1.7  Ak "Typ nájdenia" = "Nenájdená, existuje pôvodná"
                    //( podarilo sa  v RIS nájsť osobu, ktorej predchádzajúce IFO je zhodné so zadaným "IFO", t.j. došlo na strane RFO k zrušeniu stotožnenia  osôb)
                    else if (najdenaOsoba.TypNajdenia == TypNajdenia.TerajsiaNenajdenaExistujePovodna)
                    {
                        //V sytéme RIS dôjde k zrušeniu stotožnenia fyzických osôb 
                        retVal.EDUID = this.ZrusenieStotozneniaRIS(najdenaOsoba.EDUIDNajdene, Osoba.FyzickaOsoba.Ifo, najdenaOsoba.EDUIDPovodna, najdenaOsoba.FyzickaOsobaNajdena, najdenaOsoba.FyzickaOsobaPovodna);

                        //Idem updatovat povodnu osobu, lebo sa mi zrusilo stotoznenie, takze si musim nastavit upravovanu osobu do najdenej
                        najdenaOsoba.FyzickaOsobaNajdena = najdenaOsoba.FyzickaOsobaPovodna;

                        //"EDUID" = "EDUID pôvodnej osoby"
                        retVal.EDUID = najdenaOsoba.EDUIDPovodna;
                    }
                }
                else if (!najdenaOsoba.Najdena)
                {
                    //3.2.1 Ak "Typ nájdenia" = "Terajšia nenájdená"
                    if(najdenaOsoba.TypNajdenia == TypNajdenia.TerajsiaNenajdena)
                    {
                        //Systém vyhľadá, či v RIS neexistuje osoba, ktorá má údaje zhodné s údajmi spracovávanej osoby
                        var foList = GetDataAccessLayer<IFyzickaOsoba>().FyzickaOsobaFind(Osoba.FyzickaOsoba.RodneCislo, Osoba.FyzickaOsoba.DatumNarodenia, Osoba.MenoList, Osoba.PriezviskoList, null);
                        //Ak výstupný "Zoznam nájdených osôb" má práve jeden záznam a Zoznam nájdených osôb.IFO = NULL (IFO nie je vyplnené, nájdená osoba nie je spárovaná s osobou z RFO)
                        if (foList.Count == 1 && String.IsNullOrEmpty(foList[0].Ifo))
                        {
                            //Nájdenej osobe z RIS sa zapíše IFO  osoby z RFO
                            //"EDUID" = "EDUIDNájdené"
                            //Systém pokračuje bodom "Osoba nájdená"
                            najdenaOsoba.Najdena = true;
                            najdenaOsoba.EDUIDNajdene = foList[0].Eduid;
                            najdenaOsoba.TypNajdenia = TypNajdenia.Najdena;
                            najdenaOsoba.FyzickaOsobaNajdena = foList[0];
                            retVal.EDUID = foList[0].Eduid;
                        }
                    }

                    //3.2.2 Ak "Typ nájdenia" = "Pravá  nenájdená, neexistuje pôvodná"
                    else if (najdenaOsoba.TypNajdenia == TypNajdenia.PravaNenajdenaNeexistujePovodna)
                    {
                        //Systém vyhľadá, či v RIS neexistuje osoba, ktorá má údaje zhodné s údajmi spracovávanej osoby
                        var foList = GetDataAccessLayer<IFyzickaOsoba>().FyzickaOsobaFind(Osoba.FyzickaOsoba.RodneCislo, Osoba.FyzickaOsoba.DatumNarodenia, Osoba.MenoList, Osoba.PriezviskoList, null);
                        //Ak výstupný "Zoznam nájdených osôb" má práve jeden záznam a Zoznam nájdených osôb.IFO = NULL (IFO nie je vyplnené, nájdená osoba nie je spárovaná s osobou z RFO)
                        if (foList.Count == 1 && String.IsNullOrEmpty(foList[0].Ifo))
                        {
                            //Nájdenej osobe z RIS sa zapíše IFO pravej  osoby z RFO - toto sa urobi na konci pri aktualizacii najdenej osoby
                            //"IFO pôvodné" = "IFO"
                            ifoPovodne = Osoba.FyzickaOsoba.Ifo;
                            //"IFO" = "IFO pravej osoby"
                            Osoba.FyzickaOsoba.Ifo = Osoba.FyzickaOsoba.IfoPravejOsoby;

                            //K fyzickej osobe, ktorej IFO = "IFO pravej osoby" sa zaznamená pôvodné (stotožnené) IFO
                            //(EDUID sa nezapisuje, lebo neexistovala pôvodná  fyzická osoba v RIS (fyzická osoba na stotožnenie)
                            this.ZapisStotozneneIdentifikatory(foList[0].Eduid, null, ifoPovodne, foList[0]);

                            //Zavolá sa označenie pôvodnej osoby ako záujmovej v RFO
                            this.OznacenieZaujmovejOsoby(new List<string> { ifoPovodne });

                            //3.2.2.2.5 "Nájdená osoba" =fyzická osoba v RIS, ktorej IFO = "IFO pravej osoby"
                            najdenaOsoba.FyzickaOsobaNajdena = foList[0];

                            //Systém pokračuje bodom "Osoba nájdená"
                            najdenaOsoba.Najdena = true;
                            najdenaOsoba.EDUIDNajdene = foList[0].Eduid;
                            najdenaOsoba.TypNajdenia = TypNajdenia.Najdena;
                            retVal.EDUID = foList[0].Eduid;
                        }
                    }
                }
                //4.    "Zápis novej fyzickej osoby" (Spracovávaná  osoba sa v systéme nenachádza. )
                if (!najdenaOsoba.Najdena)
                {
                    //Ak zapisovaná osoba má menej ako 18 r. (t.j. musí byť zadaný dátum narodenia osoby )- je to záujmový žiak z RFO alebo "Zápis novej osoby" = TRUE (má sa zapísať nová osoba), systém fyzickú osobu zapíše
                    if ((ZapisNovejOsoby || (Osoba.FyzickaOsoba.DatumNarodenia.HasValue && (new DateTime(DateTime.Now.Subtract(Osoba.FyzickaOsoba.DatumNarodenia.Value).Ticks)).Year - 1 < 18)))
                    {
                        //Systém vytvorí novú fyzickú osobu v RIS  bez údajov (údaje sa následne zaktualizujú), vráti sa EDUID novozapísanej fyzickej osoby
                        // Systém zapíše fyzickú osobu, ktorej údaje nie sú známe (napr.pri zápise vzťahových údajov treba zapísať rodiča, ale zatiaľ je známe iba jeho IFO, 
                        // nie sú známe údaje alebo keď potrebujeme do RISu zapísať osobu, ktorej údaje sa následne budú aktualizovať). 
                        // Osoba bude mať príznak zneplatnenia nastavený na "Zneplatnená".
                        //Ak je zadané "EDUID pôvodnej osoby", tak vytvárame osobu so známym EDUID, ináč sa EDUID vygeneruje.
                        //Ak "EDUID pôvodnej osoby" = NULL, tak
                        //  Osobe sa vygeneruje EDUID.
                        //Inak
                        //  "EDUID" = "EDUID pôvodnej osoby"
                        //Do osoby.Fyzická osoba(EDUID, IFO, Dátum narodenia, meno, priezvisko, zneplatnená) sa zapíše (EDUID, "IFO", 1.1.1990, x,x,"Zneplatnená")
                        using (var transactionScope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
                        {
                            najdenaOsoba.FyzickaOsobaNajdena = new FyzickaOsoba();
                            najdenaOsoba.FyzickaOsobaNajdena.Ifo = Osoba.FyzickaOsoba.Ifo;
                            if (retVal.EDUID.HasValue)
                                najdenaOsoba.FyzickaOsobaNajdena.Eduid = retVal.EDUID.Value;
                            najdenaOsoba.FyzickaOsobaNajdena.ZaujmovaOsoba = true;
                            najdenaOsoba.FyzickaOsobaNajdena.DatumNarodenia = new DateTime(1990, 1, 1);
                            najdenaOsoba.FyzickaOsobaNajdena.MenoZobrazovane = "x";
                            najdenaOsoba.FyzickaOsobaNajdena.PriezviskoZobrazovane = "x";
                            najdenaOsoba.FyzickaOsobaNajdena = dataOperationFO.Create(najdenaOsoba.FyzickaOsobaNajdena);

                            //4.1.2 Ak "IFO pôvodné" <> NULL (má sa zapísať stotožnenie v RFO)
                            if (ifoPovodne != null)
                            {
                                //K fyzickej osobe, ktorej IFO = "IFO " sa zaznamená pôvodné (stotožnené) IFO
                                //(EDUID sa nezapisuje, lebo neexistovala pôvodná  fyzická osoba v RIS (fyzická osoba na stotožnenie)

                                //ak nemam povodnu FO na vytvorenie vazby, jednu prazdnu urobim kvoli vazbe
                                if (najdenaOsoba.FyzickaOsobaPovodna == null)
                                {
                                    najdenaOsoba.FyzickaOsobaPovodna = new FyzickaOsoba();
                                    najdenaOsoba.FyzickaOsobaPovodna.Ifo = ifoPovodne;
                                    najdenaOsoba.FyzickaOsobaPovodna.Zneplatnena = true;
                                    najdenaOsoba.FyzickaOsobaPovodna.DatumNarodenia = new DateTime(1990, 1, 1);
                                    najdenaOsoba.FyzickaOsobaPovodna.MenoZobrazovane = "x";
                                    najdenaOsoba.FyzickaOsobaPovodna.PriezviskoZobrazovane = "x";
                                    najdenaOsoba.FyzickaOsobaPovodna.ZaujmovaOsoba = true;

                                    najdenaOsoba.FyzickaOsobaPovodna = dataOperationFO.Create(najdenaOsoba.FyzickaOsobaPovodna);
                                }

                                //4.1.2.1
                                this.ZapisStotozneneIdentifikatory(najdenaOsoba.FyzickaOsobaNajdena.Eduid, najdenaOsoba.FyzickaOsobaPovodna.Eduid, ifoPovodne, najdenaOsoba.FyzickaOsobaNajdena, najdenaOsoba.FyzickaOsobaPovodna);

                                //4.1.2.2.  Systém označí  osobu s IFOm pôvodnej osoby v RFO ako záujmovú
                                if (!ZapisNovejOsobyNaZakladeOsobyRFO)
                                    this.OznacenieZaujmovejOsoby(new List<string>() { ifoPovodne });
                            }

                            //4.1.3 Systém označí  osobu s v RFO ako záujmovú
                            if (!ZapisNovejOsobyNaZakladeOsobyRFO)
                                this.OznacenieZaujmovejOsoby(new List<string>() { Osoba.FyzickaOsoba.Ifo });
                            Osoba.FyzickaOsoba.ZaujmovaOsoba = true;

                            //4.1.4 Ak je zadaný "Typ osoby v RIS" 
                            if (najdenaOsoba.JeZadanyTypOsobyVRIS)
                            {
                                //4.1.4.1   Danej fyzickej osobe sa zapíše  Typ osoby v RIS 
                                //Fyzickej osobe  identifikovanej  Osoby.Fyzická osoba.EDUID= "EDUID"  sa vytvorí zadaný typ osoby v RIS identifikovaný  paramentrom Typ osoby v RIS
                                //TODO
                            }

                            retVal.Novozapisana = true;
                            retVal.EDUID = najdenaOsoba.FyzickaOsobaNajdena.Eduid;

                            transactionScope.Complete();
                        }
                    }
                    //4.2   Inak (osoba sa nemá zapísať)
                    else
                    {
                        //UC sa ukončí
                        return retVal;
                    }
                }

                //5.    "Osoba nájdená":
                //Ak EDUID <>NULL (príslušná fyzická osoba v RIS nájdená):
                if (retVal.EDUID.HasValue)
                {
                    //Systém aktualizuje údaje fyzickej osoby.  Mapovanie na RIS je popísané v jednotlivých   interface class. 
                    //ak ju este nemam nacitanu
                    if (najdenaOsoba.FyzickaOsobaNajdena == null)
                    {
                        najdenaOsoba.FyzickaOsobaNajdena = GetDataAccessLayer<IFyzickaOsoba>().FyzickaOsobaEduIdGet(retVal.EDUID.Value);
                    }

                    //nacitam si celu osobu z DB
                    var dbOsoba = this.GetOsoba(najdenaOsoba.FyzickaOsobaNajdena.ID, null, ZapisNovejOsoby);

                    this.AktualizujOsobu(Osoba, dbOsoba);

                    retVal.Osoba = Osoba;

                }
            }
            catch (Exception ex)
            {
                ExceptionHandling.HandleBusinessException(this, ex, 218, MethodInfo.GetCurrentMethod().Name);
                throw ex;
            }

            return retVal;
        }

        /// <summary>
        ///Systém pre všetky dcérske objekty (DŽP, zamestnanec, rodič, používateľ), ktoré majú väzbu na fyzickú osobu identifikovanú Osoby.Fyzicka osoba.EDUID = "EDUID pôvodnej osoby" :
        ///Zruší sa väzba dcérskeho objektu (DŽP, zamestnanec, rodič, používateľ) na Osoby.Fyzicka osoba.EDUID = "EDUID pôvodnej osoby".
        ///Vytvorí sa väzba objektu na Osoby.Fyzicka osoba.EDUID = "EDUID pravej osoby"
        /// </summary>
        /// <param name="EDUIDPravejOsoby"></param>
        /// <param name="EDUIDPovodnejOsoby"></param>
        /// <param name="IFOPovodnejOsoby"></param>
        /// <returns></returns>
        public bool StotoznenieOsobRIS(int EDUIDPravejOsoby, int? EDUIDPovodnejOsoby, string IFOPovodnejOsoby, FyzickaOsoba osobaPrava = null, FyzickaOsoba osobaPovodna = null)
        {
            var retVal = false;
            try
            {
                // zo ServiceLocatora sa ziska trieda implementujuca rozhranie 
                var dataOperationFO = GetDataAccessLayer<IBrowse<FyzickaOsoba>>();
                var dataOperationFOEdit = GetDataAccessLayer<ICRUD<FyzickaOsoba>>();
                var dataOperationStotoznenaFO = GetDataAccessLayer<IBrowse<StotoznenaFyzOsoba>>();
                var dataOperationStotoznenaFOEdit = GetDataAccessLayer<ICRUD<StotoznenaFyzOsoba>>();
                var dataOperationDZPBrowse = GetDataAccessLayer<IBrowse<Dzp>>();
                var dataOperationDZPEdit = GetDataAccessLayer<ICRUD<Dzp>>();
                var dataOperationDZPStotoznenaEdit = GetDataAccessLayer<ICRUD<StotozneneDzp>>();
                var dataOperationZamestnanecBrowse = GetDataAccessLayer<IBrowse<Zamestnanec>>();
                var dataOperationZamestnanecEdit = GetDataAccessLayer<ICRUD<Zamestnanec>>();
                var dataOperationZamestnanecStotoznenyEdit = GetDataAccessLayer<ICRUD<StotoznenyZam>>();

                //najdem si fyzicku osobu na vytvorenie vazby
                osobaPrava = osobaPrava != null ? osobaPrava : GetDataAccessLayer<IFyzickaOsoba>().FyzickaOsobaEduIdGet(EDUIDPravejOsoby);
                osobaPovodna = osobaPovodna != null ? osobaPovodna : GetDataAccessLayer<IFyzickaOsoba>().FyzickaOsobaEduIdGet(EDUIDPovodnejOsoby.Value);

                //najdem si dcerske zaznamy na povodnu FO
                var dzpList = dataOperationDZPBrowse.Browse(new DzpFilterCriteria() { FyzickaOsobaId = osobaPovodna.ID });
                var zamestnanecList = dataOperationZamestnanecBrowse.Browse(new ZamestnanecFilterCriteria() { FyzickaOsobaId = osobaPovodna.ID });
                var rodicList = GetDataAccessLayer<IBrowse<Rodic>>().Browse(new RodicFilterCriteria() { FyzickaOsobaId = osobaPovodna.ID });

                //najdem si zaznamy na stotoznenie, musim si ich nacitat pred transakciou
                #region Nacitanie stotoznovanych udajov
                //FO
                var stotoznenaFOList = dataOperationStotoznenaFO.Browse(new StotoznenaFyzOsobaFilterCriteria() { FyzickaOsobaPovodnaId = osobaPovodna.ID, FyzickaOsobaId = osobaPrava.ID });

                //DZP
                var dzpStotozneneList = new Dictionary<Dzp, List<StotozneneDzp>>();
                if (dzpList != null && dzpList.Count > 0)
                {
                    foreach (var dzp in dzpList)
                    {
                        var list = GetDataAccessLayer<IBrowse<StotozneneDzp>>().Browse(new StotozneneDzpFilterCriteria() { DzpId = dzp.ID });
                        dzpStotozneneList.Add(dzp, new List<StotozneneDzp>());
                        foreach (var stotoznenie in list)
                            dzpStotozneneList[dzp].Add(stotoznenie);
                    }
                }

                //ZAM
                var zamStotozneneList = new Dictionary<Zamestnanec, List<StotoznenyZam>>();
                if (zamestnanecList != null && zamestnanecList.Count > 0)
                {
                    foreach (var zamestnanec in zamestnanecList)
                    {
                        var list = GetDataAccessLayer<IBrowse<StotoznenyZam>>().Browse(new StotoznenyZamFilterCriteria() { ZamestnanecId = zamestnanec.ID });
                        zamStotozneneList.Add(zamestnanec, new List<StotoznenyZam>());
                        foreach (var stotoznenie in list)
                            zamStotozneneList[zamestnanec].Add(stotoznenie);
                    }
                }
                #endregion Nacitanie stotoznovanych udajov

                //stotoznovanie
                using (var transactionScope = new System.Transactions.TransactionScope())
                {
                    using (ISession session = SessionProvider.OpenSession())
                    {
                        //K fyzickej osobe, ktorej Osoba.Fyzická osoba.EDUID = "EDUID pravej osoby" sa zaznamená pôvodné (stotožnené IFO)  a pôvodné (stotožnené)  EDUID
                        #region StotoznenaFO

                        //this.ZapisStotozneneIdentifikatory(EDUIDPravejOsoby, EDUIDPovodnejOsoby, IFOPovodnejOsoby, PravaOsobaID, PovodnaOsobaID, osobaPrava, osobaPovodna);

                        /*Osobe, ktorej EDUID  = "EDUID"  sa do Stotožnené osoby  zapíše IFOPôvodné    a EDUIDPovodné
                            Pozn.: 
                            kombinácia parametrov môže byť:
                            1) zadané aj IFOPôvodné aj EDUIDPôvodné : v systéme RIS existovala pôvodná osoba spárovaná s RFO, ktorá sa stotožňuje
                            2) zadané len IFOPôvodné - v systéme RIS neexistovala pôvodná osoba, ktorá sa stotožňuje
                            3) zadané len EDUIDPôvodné - stotožňuje sa osoba v RIS, ktorá nie je spárovaná s osobou v RFO alebo má  rovnaké IFO ako osoba, s ktorou sa stotožňuje*/

                        //mojimi slovami - najdem si oba zaznamy fyzickej osoby, vytvorim im vazbu cez vazobnu tabulku stotoznenie. Ak uz medzi nimi vazba existuju, prepisem len atribut IFOPovodne novo zadanym
                        // zo ServiceLocatora sa ziska trieda implementujuca rozhranie 
                        //najdem si fyzicku osobu na vytvorenie vazby
                        //najdem si dcerske zaznamy na povodnu FO

                        if (stotoznenaFOList != null && stotoznenaFOList.Count > 0)
                        {
                            stotoznenaFOList[0].IfoPovodne = IFOPovodnejOsoby;
                            stotoznenaFOList[0].EduidPovodne = EDUIDPovodnejOsoby;
                            //updatnem
                            GetDataAccessLayer<ICRUDTransaction<Ditec.RIS.RFO.Dol.StotoznenaFyzOsoba>>().Update(stotoznenaFOList[0], session);

                            //nastavim stotoznenie osobe
                            osobaPovodna.Zneplatnena = true;
                            GetDataAccessLayer<ICRUDTransaction<Ditec.RIS.RFO.Dol.FyzickaOsoba>>().Update(osobaPovodna, session);
                        }
                        else
                        {
                            //vytvorim vazbu
                            var stotoznenaOsoba = new StotoznenaFyzOsoba();
                            stotoznenaOsoba.EduidPovodne = EDUIDPovodnejOsoby;
                            stotoznenaOsoba.FyzickaOsobaId = osobaPrava.ID.Value;
                            stotoznenaOsoba.FyzickaOsobaPovodnaId = osobaPovodna.ID.Value;
                            stotoznenaOsoba.IfoPovodne = IFOPovodnejOsoby;

                            GetDataAccessLayer<ICRUDTransaction<Ditec.RIS.RFO.Dol.StotoznenaFyzOsoba>>().Create(stotoznenaOsoba, session);
                        }

                        ////nakoniec nastavim povodnej FO Zneplatnenie - urobi vznik vazby stotoznenaFO
                        //osobaPovodna.Zneplatnena = true;
                        //dataOperationFOEdit.Update(osobaPovodna);



                        #endregion StotoznenaFO

                        //Všetkým DŽP, ktoré majú väzbu na  Osoba.Fyzická osoba.EDUID = "EDUID povodnej osoby" sa zaznamená pôvodné (stotožnené IFO)  a pôvodné (stotožnené)  EDUID
                        #region DZP

                        if (dzpList != null && dzpList.Count > 0)
                        {
                            foreach (var dzp in dzpList)
                            {
                                //odstranim povodne stotoznenie na 
                                foreach (var stotoznenie in dzpStotozneneList[dzp])
                                    GetDataAccessLayer<ICRUDTransaction<Ditec.RIS.DZP.Dol.StotozneneDzp>>().Delete(stotoznenie, session);

                                //zapisem stotoznenie na povodnu
                                var stotozneneDzp = new StotozneneDzp();
                                stotozneneDzp.DzpId = dzp.ID.Value;
                                stotozneneDzp.EduidPovodne = EDUIDPovodnejOsoby;
                                stotozneneDzp.IfoPovodne = IFOPovodnejOsoby;
                                GetDataAccessLayer<ICRUDTransaction<Ditec.RIS.DZP.Dol.StotozneneDzp>>().Create(stotozneneDzp, session);


                                //premapujem na pravu osobu
                                dzp.FyzickaOsobaId = osobaPrava.ID.Value;
                                GetDataAccessLayer<ICRUDTransaction<Ditec.RIS.DZP.Dol.Dzp>>().Update(dzp, session);
                            }
                        }

                        #endregion DZP

                        //Všetkým Zamestnancom, ktorí majú väzbu na  Osoba.Fyzická osoba.EDUID = "EDUID pravej osoby" sa zaznamená pôvodné (stotožnené IFO)  a pôvodné (stotožnené)  EDUID
                        #region Zamestnanec

                        if (zamestnanecList != null && zamestnanecList.Count > 0)
                        {
                            foreach (var zamestnanec in zamestnanecList)
                            {
                                //odstranim povodne stotoznenie na 
                                foreach (var stotoznenie in zamStotozneneList[zamestnanec])
                                    GetDataAccessLayer<ICRUDTransaction<StotoznenyZam>>().Delete(stotoznenie, session);

                                //zapisem stotoznenie na povodnu
                                var stotoznenyZam = new StotoznenyZam();
                                stotoznenyZam.ZamestnanecId = zamestnanec.ID.Value;
                                stotoznenyZam.EduidPovodne = EDUIDPovodnejOsoby;
                                stotoznenyZam.IfoPovodne = IFOPovodnejOsoby;
                                GetDataAccessLayer<ICRUDTransaction<StotoznenyZam>>().Update(stotoznenyZam, session);

                                //premapujem na pravu osobu
                                zamestnanec.FyzickaOsobaId = osobaPrava.ID.Value;
                                GetDataAccessLayer<ICRUDTransaction<Zamestnanec>>().Update(zamestnanec, session);
                            }
                        }

                        #endregion Zamestnanec

                        //Všetkým Rodičom, ktorí majú väzbu na  Osoba.Fyzická osoba.EDUID = "EDUID pravej osoby" sa zaznamená pôvodné (stotožnené IFO)  a pôvodné (stotožnené)  EDUID
                        #region Rodic

                        if (rodicList != null && rodicList.Count > 0)
                        {
                            foreach (var rodic in rodicList)
                            {
                                //odstranim povodne stotoznenie na 
                                //var zamStotozneneList = GetDataAccessLayer<IBrowse<stoto>>().Browse(new StotoznenyZamFilterCriteria() { ZamestnanecId = rodic.ID });
                                //foreach (var stotoznenie in zamStotozneneList)
                                //    GetDataAccessLayer<ICRUD<StotoznenyZam>>().Delete(stotoznenie);

                                ////zapisem stotoznenie na povodnu
                                //var stotoznenyZam = new StotoznenyZam();
                                //stotoznenyZam.ZamestnanecId = rodic.ID.Value;
                                //stotoznenyZam.EduidPovodne = EDUIDPovodnejOsoby;
                                //stotoznenyZam.IfoPovodne = IFOPovodnejOsoby;
                                //dataOperationZamestnanecStotoznenyEdit.Update(stotoznenyZam);

                                //premapujem na pravu osobu
                                rodic.FyzickaOsobaId = osobaPrava.ID.Value;
                                GetDataAccessLayer<ICRUDTransaction<Rodic>>().Update(rodic, session);
                            }
                        }

                        #endregion Rodic

                        //Všetkým Používateľom,  ktorí majú väzbu na  Osoba.Fyzická osoba.EDUID = "EDUID pravej osoby" sa zaznamená pôvodné (stotožnené IFO)  a pôvodné (stotožnené)  EDUID
                    }
                    // potvrdenie dokoncenia transakcie 
                    transactionScope.Complete();
                }
                retVal = true;
            }
            catch (Exception ex)
            {
                ExceptionHandling.HandleBusinessException(this, ex, 209, MethodInfo.GetCurrentMethod().Name);
                throw ex;
            }
            return retVal;
        }

        /// <summary>
        /// RFO RIS Zrušenie stotožnenia v RIS
        /// </summary>
        /// <param name="EDUIDPrave"></param>
        /// <param name="IFOPovodnej"></param>
        /// <param name="EDUIDPovodne"></param>
        /// <param name="PravaOsobaID"></param>
        /// <param name="PovodnaOsobaID"></param>
        private int ZrusenieStotozneniaRIS(int? EDUIDPrave, string IFOPovodnej, int? EDUIDPovodne, FyzickaOsoba osobaPrava = null, FyzickaOsoba osobaPovodna = null)
        {
            try
            {
                var dataOperationFOBrowse = GetDataAccessLayer<IBrowse<FyzickaOsoba>>();
                var dataOperationFOEdit = GetDataAccessLayer<ICRUD<FyzickaOsoba>>();

                //2.    Ak nie je zadané "EDUID" osoby, s ktorou je osoba stotožnená, systém ho načíta
                if (!EDUIDPrave.HasValue && osobaPrava == null)
                {
                    var najdenaOsoba = this.NajdenieOsobyVRISPodlaIFOaIFOPravejOsoby(IFOPovodnej, null);

                    osobaPrava = najdenaOsoba.Najdena ? najdenaOsoba.FyzickaOsobaNajdena : null;
                    if (osobaPrava == null)
                        throw new Exception("Nenasla sa prava osobaPrava v DB");
                }
                using (var transactionScope = new System.Transactions.TransactionScope())
                {

                    //3.    Ak je zadané "EDUID pôvodnej osoby" 
                    if (EDUIDPovodne.HasValue)
                    {
                        //Systém zistí, či v systéme je zapísaná osoba, ktorej EDUID = "EDUID pôvodnej osoby"
                        if (osobaPovodna == null)
                            osobaPovodna = GetDataAccessLayer<IFyzickaOsoba>().FyzickaOsobaEduIdGet(EDUIDPovodne.Value);

                        //3.1.1 Ak existuje
                        if (osobaPovodna != null)
                        {
                            //Príznak zneplatnenia pôvodnej osoby sa nastaví na FALSE 
                            if (osobaPovodna.Zneplatnena)
                            {
                                osobaPovodna.Zneplatnena = false;
                                dataOperationFOEdit.Update(osobaPovodna);
                            }
                        }
                        else
                        {
                            //Vytvorí sa nová osoba so zadaným EDUID v RISe 
                            osobaPovodna = new FyzickaOsoba();
                            osobaPovodna.Ifo = IFOPovodnej;
                            osobaPovodna.Eduid = EDUIDPovodne.Value;
                            osobaPovodna.Zneplatnena = false;
                            osobaPovodna.DatumNarodenia = new DateTime(1990, 1, 1);
                            osobaPovodna.MenoZobrazovane = "x";
                            osobaPovodna.PriezviskoZobrazovane = "x";
                            osobaPovodna = dataOperationFOEdit.Create(osobaPovodna);
                        }
                    }
                    //4.    Ináč ak "EDUID pôvodnej osoby" = NULL
                    else
                    {
                        //Vytvorí sa nová osoba so zadaným EDUID v RISe 
                        osobaPovodna = new FyzickaOsoba();
                        osobaPovodna.Ifo = IFOPovodnej;
                        osobaPovodna.Zneplatnena = false;
                        osobaPovodna.DatumNarodenia = new DateTime(1990, 1, 1);
                        osobaPovodna.MenoZobrazovane = "x";
                        osobaPovodna.PriezviskoZobrazovane = "x";
                        osobaPovodna = dataOperationFOEdit.Create(osobaPovodna);
                    }

                    //5.    Zrušia sa väzby dcérskych objektov na pôvodnú FO , zruší sa záznam o stotoźnení a vytvoria sa nové väzby na FO
                    //5.1
                    #region DZP
                    //Zrušia sa väzby DŽP  na existujúcu fyzickú osobu ,vtrvoria sa väzby na pôvodnú osobu, zruší sa záznam o stotožnení
                    //DŽP, ktoré má vzťah s osobou s Osoby.Fyzická osoba.EDUID  = "EDUID"  sa z Stotožnené DŽP vymaže záznam, , pričom platí:
                    //Ak parameter  "EDUID pôvodnej osoby" <> NULL , tak DŽP.Stotožnené DŽP.EDUID pôvodnej osoby = "EDUID pôvodnej osoby"
                    //a súčasne
                    //Ak parameter  "IFO pôvodnej osoby" <> NULL , tak DŽP.Stotožnené DŽP.IFO pôvodnej osoby = "IFO pôvodnej osoby"

                    //mojimi slovami: zaznamy, ktore boli povodne zaznacene v tabulke Stotoznene DZP a mali v sebe udaje povodnej FO (takze boli kedysi naviazane na povodnu FO, potom sa
                    //FO stotoznila s pravou FO a preto sa DZP naviazali na pravu a byvala vazba ostala len v stotoznenej tabulke, tak tieto stotoznene DZP sa nazad naviazu na povodnu FO podla stotoznenych zaznamov)
                    var dataOperationDZPEdit = GetDataAccessLayer<ICRUD<Dzp>>();
                    var dataOperationDzpStotoznenaEdit = GetDataAccessLayer<ICRUD<StotozneneDzp>>();
                    var dataOperationDzptotoznenaBrowse = GetDataAccessLayer<IBrowse<StotozneneDzp>>();
                    //najdem vsetky stotoznene na povodnu FO
                    var dzpStotozneneList = dataOperationDzptotoznenaBrowse.Browse(new StotozneneDzpFilterCriteria() { IfoPovodne = osobaPovodna.Ifo, EduidPovodne = osobaPovodna.Eduid });
                    Dzp dzp;
                    foreach (var dzpStotoznene in dzpStotozneneList)
                    {
                        //premapujem naviazane dzp, ak ma vazbu na pravu FO, nazad na povodnu FO
                        dzp = dataOperationDZPEdit.Read(new DzpFilterCriteria() { ID = dzpStotoznene.DzpId });
                        if (dzp.FyzickaOsobaId == osobaPrava.ID)
                        {
                            //premapujem
                            dzp.FyzickaOsobaId = osobaPovodna.ID.Value;
                            dataOperationDZPEdit.Update(dzp);

                            //zrusim stotoznenie
                            dataOperationDzpStotoznenaEdit.Delete(dzpStotoznene);
                        }
                    }

                    //Následne sa zruší väzba na Osoby.Fyzická osoba.EDUID = "EDUID" a vytvorí sa väzba na  Osoby.Fyzická osoba.EDUID = "EDUID pôvodnej osoby"


                    #endregion DZP
                    //5.2
                    #region Zamestnanci
                    //Zrušia sa väzby zamestnancov  na existujúcu fyzickú osobu ,vtrvoria sa väzby na pôvodnú osobu, zruší sa záznam o stotožnení
                    var dataOperationZamestnanecEdit = GetDataAccessLayer<ICRUD<Zamestnanec>>();
                    var dataOperationZamestnanecStotoznenaEdit = GetDataAccessLayer<ICRUD<StotoznenyZam>>();
                    var dataOperationZamestnanecStotoznenaBrowse = GetDataAccessLayer<IBrowse<StotoznenyZam>>();
                    //najdem vsetky stotoznene na povodnu FO
                    var zamestnanecStotozneneList = dataOperationZamestnanecStotoznenaBrowse.Browse(new StotoznenyZamFilterCriteria() { IfoPovodne = osobaPovodna.Ifo, EduidPovodne = osobaPovodna.Eduid });
                    Zamestnanec zamestnanec;
                    foreach (var zamestnanecStotoznene in zamestnanecStotozneneList)
                    {
                        //premapujem naviazane dzp, ak ma vazbu na pravu FO, nazad na povodnu FO
                        zamestnanec = dataOperationZamestnanecEdit.Read(new ZamestnanecFilterCriteria() { ID = zamestnanecStotoznene.ZamestnanecId });
                        if (zamestnanec.FyzickaOsobaId == osobaPrava.ID)
                        {
                            //premapujem
                            zamestnanec.FyzickaOsobaId = osobaPovodna.ID.Value;
                            dataOperationZamestnanecEdit.Update(zamestnanec);

                            //zrusim stotoznenie
                            dataOperationZamestnanecStotoznenaEdit.Delete(zamestnanecStotoznene);
                        }
                    }

                    //Následne sa zruší väzba na Osoby.Fyzická osoba.EDUID = "EDUID" a vytvorí sa väzba na  Osoby.Fyzická osoba.EDUID = "EDUID pôvodnej osoby"


                    #endregion Zamestnanci
                    //5.3 Rodicia
                    #region Rodicia
                    //Zrušia sa väzby rodičov  na existujúcu fyzickú osobu ,vtrvoria sa väzby na pôvodnú osobu, zruší sa záznam o stotožnení
                    //zoznam vztahovych osob, ktore su namapovane na pravu osobu
                    var vztahList = GetDataAccessLayer<IBrowse<VztahovaFyzOsoba>>().Browse(new VztahovaFyzOsobaFilterCriteria() { FyzickaOsobaVztahId = osobaPrava.ID, RfoTypRodinnehoVztahuIk = GetBussinessLogicLayer<IBFCodeList>().GetBufferedCodeList<RfoTypRodinnehoVztahu>().GetInternyKodByKod("1") });
                    foreach (var vztah in vztahList)
                    {
                        //premapujem naviazanu pravu na povodnu osobu
                        vztah.FyzickaOsobaVztahId = osobaPovodna.ID.Value;
                        vztah.IfoVztahovejOsoby = osobaPovodna.Ifo;
                        GetDataAccessLayer<ICRUD<VztahovaFyzOsoba>>().Update(vztah);
                    }

                    #endregion Rodicia

                    //6.    Zrušia záznamy o stotožnení fyzickej osoby
                    var dataOperationFOStotoznenaEdit = GetDataAccessLayer<ICRUD<StotoznenaFyzOsoba>>();
                    var dataOperationFOStotoznenaBrowse = GetDataAccessLayer<IBrowse<StotoznenaFyzOsoba>>();
                    //najdem si stotoznenia
                    var stotozneneFOList = dataOperationFOStotoznenaBrowse.Browse(new StotoznenaFyzOsobaFilterCriteria() { EduidPovodne = osobaPovodna.Eduid, IfoPovodne = osobaPovodna.Ifo });
                    foreach (var stotoznenieFO in stotozneneFOList)
                    {
                        //zrusim kazde jedno
                        dataOperationFOStotoznenaEdit.Delete(stotoznenieFO);
                    }

                    transactionScope.Complete();
                }
                return osobaPovodna.Eduid;
            }
            catch (Exception ex)
            {
                ExceptionHandling.HandleBusinessException(this, ex, 210, MethodInfo.GetCurrentMethod().Name);
                throw ex;
            }
        }

        /// <summary>
        /// RFO RIS Zápis novej osoby na základe osoby z RFO
        /// </summary>
        /// <param name="IFO"></param>
        /// <param name="IFOPravejOsoby"></param>
        /// <returns></returns>
        public FyzickaOsoba ZapisNovejOsobyNaZakladeOsobyRFO(string IFO, string IFOPravejOsoby)
        {
            //Systém prijme požiadavku pre zápis novej osoby v RIS na základe osoby z RFO.
            //Vstupnými parametrami sú:
            //"IFO"
            //"IFO pravej osoby"

            //Výstupný parameter:
            //"EDUID"
            try
            {
                //Systém  zistí, či v RIS už daná osoba nie je zapísaná
                var najdenaOsoba = this.NajdenieOsobyVRISPodlaIFOaIFOPravejOsoby(IFO, IFOPravejOsoby);
                //3.1   Ak "Nájdená" = FALSE - systém nenašiel osobu v RIS RIS 
                if (najdenaOsoba == null || !najdenaOsoba.Najdena)
                {
                    //Systém  zavolá označenie osoby s  IFO v RFO za záujmovú
                    this.OznacenieZaujmovejOsoby(new List<string> { IFO });

                    //3.1.3 Systém  zavolá aktualizáciu údajov fyzickej  osoby z RFO pre osobu so zvoleným IFO 
                    var refUdaje = (OsobaResponse)this.PoskytnutieReferencnychUdajovZoznamuIFOOnline(new List<string> { IFO }, null);

                    //3.1.2 Ak IFO pravej osoby != NULL - az z detailu osoby si zistim, ci ma pravu
                    if (!String.IsNullOrEmpty(refUdaje.OsobaList[0].FyzickaOsoba.IfoPravejOsoby))
                    {
                        //Systém  zavolá označenie osoby s  IFO pravej osoby v RFO za záujmovú
                        this.OznacenieZaujmovejOsoby(new List<string> { IFOPravejOsoby });
                    }

                    //3.1.4 Systém  zavolá zápis  údajov fyzickej  osoby v RIS na základe údajov osoby z RFO
                    var aktualizovanaOsoba = this.AktualizaciaUdajovFyzickejOsobyUdajmiRFO(refUdaje, true, true, null, true);
                    return aktualizovanaOsoba[0].Osoba.FyzickaOsoba;
                }
                else
                    return najdenaOsoba.FyzickaOsobaNajdena;

            }
            catch (Exception ex)
            {
                ExceptionHandling.HandleBusinessException(this, ex, 211, MethodInfo.GetCurrentMethod().Name);
                throw ex;
            }

            return null;
        }

        private void ZapisStotozneneIdentifikatory(int EDUIDPravejOsoby, int? EDUIDPovodnejOsoby, string IFOPovodnejOsoby, FyzickaOsoba osobaPrava = null, FyzickaOsoba osobaPovodna = null)
        {
            #region StotoznenaFO

            /*Osobe, ktorej EDUID  = "EDUID"  sa do Stotožnené osoby  zapíše IFOPôvodné    a EDUIDPovodné
                    Pozn.: 
                    kombinácia parametrov môže byť:
                    1) zadané aj IFOPôvodné aj EDUIDPôvodné : v systéme RIS existovala pôvodná osoba spárovaná s RFO, ktorá sa stotožňuje
                    2) zadané len IFOPôvodné - v systéme RIS neexistovala pôvodná osoba, ktorá sa stotožňuje
                    3) zadané len EDUIDPôvodné - stotožňuje sa osoba v RIS, ktorá nie je spárovaná s osobou v RFO alebo má  rovnaké IFO ako osoba, s ktorou sa stotožňuje*/

            //mojimi slovami - najdem si oba zaznamy fyzickej osoby, vytvorim im vazbu cez vazobnu tabulku stotoznenie. Ak uz medzi nimi vazba existuju, prepisem len atribut IFOPovodne novo zadanym
            // zo ServiceLocatora sa ziska trieda implementujuca rozhranie 
            var dataOperationFO = GetDataAccessLayer<IBrowse<FyzickaOsoba>>();
            var dataOperationFOEdit = GetDataAccessLayer<ICRUD<FyzickaOsoba>>();
            var dataOperationStotoznenaFO = GetDataAccessLayer<IBrowse<StotoznenaFyzOsoba>>();
            var dataOperationStotoznenaFOEdit = GetDataAccessLayer<ICRUD<StotoznenaFyzOsoba>>();

            //najdem si fyzicku osobu na vytvorenie vazby
            osobaPrava = osobaPrava != null ? osobaPrava : GetDataAccessLayer<IFyzickaOsoba>().FyzickaOsobaEduIdGet(EDUIDPravejOsoby);
            osobaPovodna = osobaPovodna != null || !EDUIDPovodnejOsoby.HasValue ? osobaPovodna : GetDataAccessLayer<IFyzickaOsoba>().FyzickaOsobaEduIdGet(EDUIDPovodnejOsoby.Value);

            //najdem si dcerske zaznamy na povodnu FO
            IList<StotoznenaFyzOsoba> stotoznenaFOList = null;
            if(osobaPrava != null && osobaPovodna != null)
             stotoznenaFOList = dataOperationStotoznenaFO.Browse(new StotoznenaFyzOsobaFilterCriteria() { FyzickaOsobaPovodnaId = osobaPovodna.ID, FyzickaOsobaId = osobaPrava.ID });

            using (var transactionScope = new System.Transactions.TransactionScope())
            {
                if (stotoznenaFOList != null && stotoznenaFOList.Count > 0)
                {
                    stotoznenaFOList[0].IfoPovodne = IFOPovodnejOsoby;
                    stotoznenaFOList[0].EduidPovodne = EDUIDPovodnejOsoby;
                    //updatnem
                    dataOperationStotoznenaFOEdit.Update(stotoznenaFOList[0]);
                }
                else
                {
                    if (osobaPovodna == null && !String.IsNullOrEmpty(IFOPovodnejOsoby))
                    {
                        //vytvorim si prazdnu osobu, ktoru naviazem
                        osobaPovodna = new FyzickaOsoba();
                        osobaPovodna.Ifo = IFOPovodnejOsoby;
                        osobaPovodna.Zneplatnena = true;
                        osobaPovodna.DatumNarodenia = new DateTime(1990, 1, 1);
                        osobaPovodna.MenoZobrazovane = "x";
                        osobaPovodna.PriezviskoZobrazovane = "x";
                        osobaPovodna.ZaujmovaOsoba = true;
                        osobaPovodna = dataOperationFOEdit.Create(osobaPovodna);
                    }
                    //vytvorim vazbu
                    var stotoznenaOsoba = new StotoznenaFyzOsoba();
                    stotoznenaOsoba.EduidPovodne = EDUIDPovodnejOsoby;
                    stotoznenaOsoba.FyzickaOsobaId = osobaPrava.ID.Value;
                    stotoznenaOsoba.FyzickaOsobaPovodnaId = osobaPovodna.ID.Value;
                    stotoznenaOsoba.IfoPovodne = IFOPovodnejOsoby;

                    dataOperationStotoznenaFOEdit.Create(stotoznenaOsoba);
                }

                ////nakoniec nastavim povodnej FO Zneplatnenie
                //osobaPovodna.Zneplatnena = true;
                //dataOperationFOEdit.Update(osobaPovodna);

                transactionScope.Complete();
            }

            #endregion StotoznenaFO
        }

        /// <summary>
        /// Metoda pre ziskanie doplnkovych udajov fyzickej osoby
        /// </summary>
        /// <param name="IDFyziskaOsoba"></param>
        /// <returns></returns>
        public Osoba GetOsoba(Guid? IDFyziskaOsoba = null, int? EDUID = null, bool NovaOsoba = false)
        {
            if (!IDFyziskaOsoba.HasValue && !EDUID.HasValue)
                return null;

            var osoba = new Osoba();
            try
            {
                //Nacitam si zaznam RFO.FyzickaOsoba
                var dataOperationFyzikaOsoba = GetDataAccessLayer<ICRUD<FyzickaOsoba>>();
                var fyzickaOsoba = IDFyziskaOsoba.HasValue ? dataOperationFyzikaOsoba.Read(new FyzickaOsobaFilterCriteria() { ID = IDFyziskaOsoba }) : GetDataAccessLayer<IFyzickaOsoba>().FyzickaOsobaEduIdGet(EDUID.Value);
                if (fyzickaOsoba == null)
                    return null;

                osoba.FyzickaOsoba = fyzickaOsoba;
                IDFyziskaOsoba = osoba.FyzickaOsoba.ID;

                if (!NovaOsoba)
                {
                    osoba.MenoList = GetDataAccessLayer<IMeno>().MenoFOGet(IDFyziskaOsoba.Value);

                    osoba.PriezviskoList = GetDataAccessLayer<IPriezvisko>().PriezviskoFOGet(IDFyziskaOsoba.Value);

                    osoba.RodnePriezviskoList = GetDataAccessLayer<IRodnePriezvisko>().RodnePriezviskoFOGet(IDFyziskaOsoba.Value);

                    osoba.TitulList = GetDataAccessLayer<IFyzickaOsobaTitul>().FyzickaOsobaTitulFOGet(IDFyziskaOsoba.Value);

                    //Nacitam si vsetky objekty FO.FYZICKA_OSOBA_STATNA_PRISLUSNOST
                    osoba.StatnaPrislusnostList = GetDataAccessLayer<IFyzickaOsobaStatnaPrislusnost>().FyzickaOsobaStatnaPrislusnostFOGet(IDFyziskaOsoba.Value);

                    //Nacitam si vsetky objekty FO.STOTOZNENA_FYZ_OSOBA
                    osoba.StotoznenaFyzOsobaList = GetDataAccessLayer<IStotoznenaFyzOsoba>().StotoznenaFyzOsobaFOGet(IDFyziskaOsoba.Value);

                    //Nacitam si vsetky objekty FO.VZTAHOVA_FYZ_OSOBA
                    osoba.VztahovaOsobaList = GetDataAccessLayer<IVztahovaFyzOsoba>().VztahovaFyzOsobaFOGet(IDFyziskaOsoba.Value);

                    //Nacitam si vsetky objekty FO.PRAVNA_SPOSOBILOST_OBMEDZENIE
                    osoba.PravnaSposobilostObmedzenieList = GetDataAccessLayer<IPravnaSposobilostObmedzenie>().PravnaSposobilostObmedzenieFOGet(IDFyziskaOsoba.Value);

                    //Nacitam si vsetky objekty FO.ZAKAZ_POBYTU
                    osoba.ZakazPobytuList = GetDataAccessLayer<IZakazPobytu>().ZakazPobytuFOGet(IDFyziskaOsoba.Value);

                    //Nacitam si vsetky objekty FO.UDAJE_POBYTU
                    osoba.UdajePobytuList = GetDataAccessLayer<IUdajePobytu>().UdajePobytuFOGet(IDFyziskaOsoba.Value);
                    foreach (var udaj in osoba.UdajePobytuList)
                    {
                        udaj.RegionMimoSrList = GetDataAccessLayer<IBrowse<RegionMimoSr>>().Browse(new RegionMimoSrFilterCriteria() { UdajePobytuId = udaj.ID }).ToList();

                        if (udaj.MimoSr == false && udaj.RfoUlicaIk.HasValue)
                        {
                            var listUlica = GetBussinessLogicLayer<IBFCodeList>().GetBufferedCodeList<RfoUlica>();
                            if (listUlica != null && listUlica.Count > 0)
                            {
                                var ulica = listUlica.FirstOrDefault(u => u.InternyKod == udaj.RfoUlicaIk);
                                if (ulica != null)
                                {
                                    udaj.UlicaZobrazena = ulica.NazovSk;
                                }
                                else
                                {
                                    udaj.UlicaZobrazena = udaj.Ulica;
                                }
                            }
                            else
                            {
                                udaj.UlicaZobrazena = udaj.Ulica;
                            }
                        }
                        else
                        {
                            udaj.UlicaZobrazena = udaj.Ulica;
                        }
                    }
                }

                return osoba;
            }
            catch (Exception ex)
            {
                ExceptionHandling.HandleBusinessException(this, ex, 212, MethodInfo.GetCurrentMethod().Name);
                throw ex;
            }
        }

        /// <summary>
        /// Systém zistí, či osoba má zadaný Typ osoby v RIS. Ak áno, vráti TRUE, ináč FALSE.
        ///Parameter Typ osoby v RIS môže nadobúdať hodnoty:
        ///"" - (nezadané) - osoba je v RIS použitá, má zadaný ľubovoľný Typ osoby v RIS
        ///"žiak"  - osoba je źiakom
        ///"rodič" - osoba je rodičom
        ///"zamestnanec" - osoba je zamestnanec
        ///"používateľ" - osoba je pouźívateľ
        /// </summary>
        /// <returns></returns>
        private List<TypOsobyVRis> MaZadanyTypOsobyRIS(int EDUID)
        {
            try
            {
                return GetDataAccessLayer<IFyzickaOsoba>().FyzickaOsobaFindVazby(EDUID);
            }
            catch (Exception ex)
            {
                ExceptionHandling.HandleBusinessException(this, ex, 213, MethodInfo.GetCurrentMethod().Name);
            }
            return null;
        }


        /// <summary>
        /// Aktualizuje (updatne) sa zaznam Fyzickej osoby aj s naviazanymi zaznamami v transakcii
        /// </summary>
        /// <param name="osobaNew">osoba, ktorej udaje sa prenesu do DB. Nastavia sa jej ID a EDUID</param>
        /// <param name="osobaDB">povodna osoba nacitana z DB, voci ktorej sa nove udaje porovnavaju</param>
        /// <returns></returns>
        public bool AktualizujOsobu(Osoba osobaNew, Osoba osobaDB)
        {
            try
            {
                //identifikatory z DB si necham., budem robit update
                osobaNew.FyzickaOsoba.Eduid = osobaDB.FyzickaOsoba.Eduid;
                osobaNew.FyzickaOsoba.ID = osobaDB.FyzickaOsoba.ID;
                var internalPersonID = osobaNew.FyzickaOsoba.InternalPersonID;

                using (var transactionScope = new System.Transactions.TransactionScope()) //new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
                {
                    using (ISession session = SessionProvider.OpenSession())
                    {
                        //update FO
                        GetDataAccessLayer<ICRUDTransaction<Ditec.RIS.RFO.Dol.FyzickaOsoba>>().Update(osobaNew.FyzickaOsoba, session);
                        osobaNew.FyzickaOsoba.InternalPersonID = internalPersonID;

                        //Pre každý záznam z osoby.Fyzická osoba.Meno
                        #region Meno
                        foreach (var meno in osobaDB.MenoList.ListToDelete(osobaNew.MenoList))
                        {
                            GetDataAccessLayer<ICRUDTransaction<Ditec.RIS.RFO.Dol.Meno>>().Delete(meno, session);
                        }
                        foreach (var meno in osobaDB.MenoList.ListToUpdate(osobaNew.MenoList))
                        {
                            meno.FyzickaOsobaId = osobaNew.FyzickaOsoba.ID.Value;
                            GetDataAccessLayer<ICRUDTransaction<Ditec.RIS.RFO.Dol.Meno>>().Update(meno, session);
                        }
                        foreach (var meno in osobaDB.MenoList.ListToCreate(osobaNew.MenoList))
                        {
                            meno.FyzickaOsobaId = osobaNew.FyzickaOsoba.ID.Value;
                            GetDataAccessLayer<ICRUDTransaction<Ditec.RIS.RFO.Dol.Meno>>().Create(meno, session);
                        }
                        #endregion Meno

                        //Pre každý záznam z osoby.Fyzická osoba.Priezvisko
                        #region Priezvisko
                        foreach (var priezvisko in osobaDB.PriezviskoList.ListToCreate(osobaNew.PriezviskoList))
                        {
                            priezvisko.FyzickaOsobaId = osobaNew.FyzickaOsoba.ID.Value;
                            GetDataAccessLayer<ICRUDTransaction<Ditec.RIS.RFO.Dol.Priezvisko>>().Create(priezvisko, session);
                        }
                        foreach (var priezvisko in osobaDB.PriezviskoList.ListToUpdate(osobaNew.PriezviskoList))
                        {
                            priezvisko.FyzickaOsobaId = osobaNew.FyzickaOsoba.ID.Value;
                            GetDataAccessLayer<ICRUDTransaction<Ditec.RIS.RFO.Dol.Priezvisko>>().Update(priezvisko, session);
                        }
                        foreach (var priezvisko in osobaDB.PriezviskoList.ListToDelete(osobaNew.PriezviskoList))
                        {
                            GetDataAccessLayer<ICRUDTransaction<Ditec.RIS.RFO.Dol.Priezvisko>>().Delete(priezvisko, session);
                        }
                        #endregion Priezvisko

                        //Systém aktualizuje Rodné priezviská pre danú fyzickú osobu:
                        #region RodnePriezvisko
                        foreach (var rodnePriezvisko in osobaDB.RodnePriezviskoList.ListToCreate(osobaNew.RodnePriezviskoList))
                        {
                            rodnePriezvisko.FyzickaOsobaId = osobaNew.FyzickaOsoba.ID.Value;
                            GetDataAccessLayer<ICRUDTransaction<Ditec.RIS.RFO.Dol.RodnePriezvisko>>().Create(rodnePriezvisko, session);
                        }
                        foreach (var rodnePriezvisko in osobaDB.RodnePriezviskoList.ListToUpdate(osobaNew.RodnePriezviskoList))
                        {
                            rodnePriezvisko.FyzickaOsobaId = osobaNew.FyzickaOsoba.ID.Value;
                            GetDataAccessLayer<ICRUDTransaction<Ditec.RIS.RFO.Dol.RodnePriezvisko>>().Update(rodnePriezvisko, session);
                        }
                        foreach (var rodnePriezvisko in osobaDB.RodnePriezviskoList.ListToDelete(osobaNew.RodnePriezviskoList))
                        {
                            GetDataAccessLayer<ICRUDTransaction<Ditec.RIS.RFO.Dol.RodnePriezvisko>>().Delete(rodnePriezvisko, session);
                        }
                        #endregion RodnePriezvisko

                        //Pre každý záznam z osoby.Fyzická osoba.Titul 
                        #region Titul
                        foreach (var titul in osobaDB.TitulList.ListToCreate(osobaNew.TitulList))
                        {
                            titul.FyzickaOsobaId = osobaNew.FyzickaOsoba.ID.Value;
                            GetDataAccessLayer<ICRUDTransaction<Ditec.RIS.RFO.Dol.FyzickaOsobaTitul>>().Create(titul, session);
                        }
                        foreach (var titul in osobaDB.TitulList.ListToUpdate(osobaNew.TitulList))
                        {
                            titul.FyzickaOsobaId = osobaNew.FyzickaOsoba.ID.Value;
                            GetDataAccessLayer<ICRUDTransaction<Ditec.RIS.RFO.Dol.FyzickaOsobaTitul>>().Update(titul, session);
                        }
                        foreach (var titul in osobaDB.TitulList.ListToDelete(osobaNew.TitulList))
                        {
                            GetDataAccessLayer<ICRUDTransaction<Ditec.RIS.RFO.Dol.FyzickaOsobaTitul>>().Delete(titul, session);
                        }
                        #endregion Titul

                        //Pre každý záznam z osoby.Fyzická osoba.štátna príslušnosť 
                        #region StatnaPrislusnost
                        foreach (var statnaPrislusnost in osobaDB.StatnaPrislusnostList.ListToCreate(osobaNew.StatnaPrislusnostList))
                        {
                            statnaPrislusnost.FyzickaOsobaId = osobaNew.FyzickaOsoba.ID.Value;
                            GetDataAccessLayer<ICRUDTransaction<Ditec.RIS.RFO.Dol.FyzickaOsobaStatnaPrislusnost>>().Create(statnaPrislusnost, session);
                        }
                        foreach (var statnaPrislusnost in osobaDB.StatnaPrislusnostList.ListToUpdate(osobaNew.StatnaPrislusnostList))
                        {
                            statnaPrislusnost.FyzickaOsobaId = osobaNew.FyzickaOsoba.ID.Value;
                            GetDataAccessLayer<ICRUDTransaction<Ditec.RIS.RFO.Dol.FyzickaOsobaStatnaPrislusnost>>().Update(statnaPrislusnost, session);
                        }
                        foreach (var statnaPrislusnost in osobaDB.StatnaPrislusnostList.ListToDelete(osobaNew.StatnaPrislusnostList))
                        {
                            GetDataAccessLayer<ICRUDTransaction<Ditec.RIS.RFO.Dol.FyzickaOsobaStatnaPrislusnost>>().Delete(statnaPrislusnost, session);
                        }
                        #endregion StatnaPrislusnost

                        #region UdajePobytu
                        foreach (var pobyt in osobaDB.UdajePobytuList.ListToCreate(osobaNew.UdajePobytuList))
                        {
                            pobyt.FyzickaOsobaId = osobaNew.FyzickaOsoba.ID.Value;
                            var pob = GetDataAccessLayer<ICRUDTransaction<Ditec.RIS.RFO.Dol.UdajePobytu>>().Create(pobyt, session);
                            foreach (var regMimoSr in pobyt.RegionMimoSrList)
                            {
                                regMimoSr.UdajePobytuId = pob.ID.Value;
                                GetDataAccessLayer<ICRUDTransaction<Ditec.RIS.RFO.Dol.RegionMimoSr>>().Create(regMimoSr, session);
                            }
                        }
                        foreach (var pobyt in osobaDB.UdajePobytuList.ListToDelete(osobaNew.UdajePobytuList))
                        {
                            foreach (var regMimoSr in pobyt.RegionMimoSrList)
                            {
                                GetDataAccessLayer<ICRUDTransaction<Ditec.RIS.RFO.Dol.RegionMimoSr>>().Delete(regMimoSr, session);
                            }
                            GetDataAccessLayer<ICRUDTransaction<Ditec.RIS.RFO.Dol.UdajePobytu>>().Delete(pobyt, session);
                        }
                        #endregion UdajePobytu

                        //Pre každý záznam z osoby.Fyzická osoba.Obmedzenie/pozbavenie právnej spôsobilosti na právne úkony 
                        #region PravnaSposobilostObmedzenie
                        foreach (var sposobilost in osobaDB.PravnaSposobilostObmedzenieList.ListToCreate(osobaNew.PravnaSposobilostObmedzenieList))
                        {
                            sposobilost.FyzickaOsobaId = osobaNew.FyzickaOsoba.ID.Value;
                            GetDataAccessLayer<ICRUDTransaction<Ditec.RIS.RFO.Dol.PravnaSposobilostObmedzenie>>().Create(sposobilost, session);
                        }
                        foreach (var sposobilost in osobaDB.PravnaSposobilostObmedzenieList.ListToUpdate(osobaNew.PravnaSposobilostObmedzenieList))
                        {
                            sposobilost.FyzickaOsobaId = osobaNew.FyzickaOsoba.ID.Value;
                            GetDataAccessLayer<ICRUDTransaction<Ditec.RIS.RFO.Dol.PravnaSposobilostObmedzenie>>().Update(sposobilost, session);
                        }
                        foreach (var sposobilost in osobaDB.PravnaSposobilostObmedzenieList.ListToDelete(osobaNew.PravnaSposobilostObmedzenieList))
                        {
                            GetDataAccessLayer<ICRUDTransaction<Ditec.RIS.RFO.Dol.PravnaSposobilostObmedzenie>>().Delete(sposobilost, session);
                        }
                        #endregion PravnaSposobilostObmedzenie

                        //Pre každý záznam z osoby.Fyzická osoba.Údaje o zákaze pobytu 
                        #region ZakazPobytu
                        foreach (var item in osobaDB.ZakazPobytuList.ListToCreate(osobaNew.ZakazPobytuList))
                        {
                            item.FyzickaOsobaId = osobaNew.FyzickaOsoba.ID.Value;
                            GetDataAccessLayer<ICRUDTransaction<Ditec.RIS.RFO.Dol.ZakazPobytu>>().Create(item, session);
                        }
                        foreach (var item in osobaDB.ZakazPobytuList.ListToUpdate(osobaNew.ZakazPobytuList))
                        {
                            item.FyzickaOsobaId = osobaNew.FyzickaOsoba.ID.Value;
                            GetDataAccessLayer<ICRUDTransaction<Ditec.RIS.RFO.Dol.ZakazPobytu>>().Update(item, session);
                        }
                        foreach (var item in osobaDB.ZakazPobytuList.ListToDelete(osobaNew.ZakazPobytuList))
                        {
                            GetDataAccessLayer<ICRUDTransaction<Ditec.RIS.RFO.Dol.ZakazPobytu>>().Delete(item, session);
                        }
                        #endregion ZakazPobytu

                        //Systém zistí, či spracovávaná osoba má menej ako 18 rokov
                        if (osobaNew.FyzickaOsoba.DatumNarodenia.HasValue && (new DateTime(DateTime.Now.Subtract(osobaNew.FyzickaOsoba.DatumNarodenia.Value).Ticks)).Year - 1 < 18)
                        {

                            //TODO - toto si budem musiet objasnit

                            #region VztahovaOsoba

                            //Ak áno, systém aktualizuje Vzťahové údaje pre danú fyzickú osobu:
                            //Zrušia sa rodičovské vzťahy daného žiaka, ktoré nemajú zadané IFO vzťahovej osoby (také, ktoré nie sú z RFO)
                            //5.1.1.7.1.2   Pre všetky existujúce záznamy z Osoby.Fyzicka osoba.Vzťahová osoba spracovávaného žiaka
                            //Systém zistí, či  vzťahová osoba s IFO vzťahovej osoby sa nachádza medzi IFOVzťahová z RFO.  WS Vzťahová osoba ,pričom 
                            //Typ vzťahu = "Rodič"  v RFO WS Vzťahová osoba.
                            //Porovnáva sa identifikátor.
                            //Ak sa nenachádza (bol zrušený daný rodičovský vzťah)
                            //Systém načíta EDUID vzťahovej osoby
                            //Vymaže sa  záznam pre daný vzťah
                            foreach (var item in osobaDB.VztahovaOsobaList.ListToDeleteByIdentifikator(osobaNew.VztahovaOsobaList))
                            {
                                GetDataAccessLayer<ICRUDTransaction<Ditec.RIS.RFO.Dol.VztahovaFyzOsoba>>().Delete(item, session);
                            }

                            //Pre všetky existujúce záznamy  z RFO WS Vzťahová osoba pre spracovávanú fyzickú osobu ,kdeTyp vzťahu = "Rodič" v RFO WS Vzťahová osoba
                            //Systém zistí, či RFO WS Vzťahová osoba.IFOVzťahová  sa nachádza medzi vzťahovými osobami pre "EDUID" osoby. Porovnáva sa identifikátor. 
                            foreach (var item in osobaDB.VztahovaOsobaList.ListToCreateByIdentifikator(osobaNew.VztahovaOsobaList))
                            {
                                //ak nie Systém načíta EDUID vzťahovej osoby
                                //Ak EDIUD vzťahovej osoby = NULL -  v RIS neexistuje osoba s IFO vzťahovej osoby , treba ju zapísat
                                var risOsoba = this.NajdenieOsobyVRISPodlaIFOaIFOPravejOsoby(item.IfoVztahovejOsoby, null);
                                if (risOsoba.Najdena)
                                    item.FyzickaOsobaVztahId = risOsoba.FyzickaOsobaNajdena.ID.Value;

                                item.FyzickaOsobaId = osobaNew.FyzickaOsoba.ID.Value;
                                GetDataAccessLayer<ICRUDTransaction<Ditec.RIS.RFO.Dol.VztahovaFyzOsoba>>().Create(item, session);
                            }

                            #endregion VztahovaOsoba
                        }
                    }

                    transactionScope.Complete();
                }
                return true;
            }
            catch (Exception ex)
            {
                ExceptionHandling.HandleBusinessException(this, ex, 201, MethodInfo.GetCurrentMethod().Name);
                throw new Exception(GetBussinessLogicLayer<IBFCodeList>().GetBufferedCodeList<ChybovaSprava>().GetText(51501));
            }
        }

        /// <summary>
        /// Vyhlada IFO z DB podla EDUID
        /// </summary>
        /// <param name="EDUID"></param>
        /// <returns></returns>
        public bool VratIFOPodlaEDUID(int? EDUID, out string ifo)
        {
            ifo = null;
            if (!EDUID.HasValue)
                return false;

            try
            {
                var FO = GetDataAccessLayer<IFyzickaOsoba>().FyzickaOsobaEduIdGet(EDUID.Value);
                if (FO != null)
                {
                    ifo = FO.Ifo;
                    return true;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandling.HandleBusinessException(this, ex, 214, MethodInfo.GetCurrentMethod().Name);
            }
            return false;
        }

        /// <summary>
        /// Modul prijme požiadavku pre zápis spárovania osoby v RIS s osobou v RFO (priradenie osoby z RFO k osobe z RIS)
        /// </summary>
        /// <param name="EDUID"></param>
        /// <param name="IFO"></param>
        /// <param name="IFOPravejOsoby"></param>
        /// <param name="Spracovanie">Individuálne/Hromadné"</param>
        /// <param name="osobaKtorejSaNasloEDUID">ak uz mam nacitany detail osoby s EDUID, nebudem zase nacitavat z DB</param>
        /// <returns></returns>
        public ZapisSparovanieOsobRisRfoRetVal ZapisSparovanieOsobRisRfo(int? EDUID, string IFO, string IFOPravejOsoby, Spracovanie Spracovanie = Spracovanie.Hromadne, FyzickaOsoba osobaKtorejSaNasloEDUID = null, OsobaResponse vyhladanaOsobaIFOOnline = null, bool throwExeption = false)
        {
            var retVal = new ZapisSparovanieOsobRisRfoRetVal();
            try
            {
                //SQD   RFO RIS Zápis individuálneho spárovania osôb RIS-RFO
                if (Spracovanie == Dol.Spracovanie.Individualne)
                {
                    retVal = this.ZapisindIvidualnehoSparovaniaOsobRisRfo(EDUID, IFO, IFOPravejOsoby, osobaKtorejSaNasloEDUID, vyhladanaOsobaIFOOnline, throwExeption);
                }
                //SQD   RFO RIS Zápis spárovania osôb RIS-RFO - jednoduchsie prerobenie ako zmenit cele mapovanie az po SVC vrstvu..
                else
                {
                    //2.    Systém  zistí, èi v RISe už nemá nejaká osoba priradené požadované IFO - èi už nie je spárovaná s nejakou FO
                    var najdenaOsoba = this.NajdenieOsobyVRISPodlaIFOaIFOPravejOsoby(IFO, IFOPravejOsoby, throwExeption);
                    //2.1   Ak "Nájdená" = FALSE - neexistuje taká  FO v RIS , ktorá je spárovaná s osobou so zadaným IFO
                    if (!najdenaOsoba.Najdena) //RFO RIS Zápis individuálneho spárovania osôb RIS-RFO
                    {
                        if (osobaKtorejSaNasloEDUID == null)
                        {
                            var fo = GetDataAccessLayer<IFyzickaOsoba>().FyzickaOsobaEduIdGet(EDUID.Value);
                            if (fo == null)
                                throw new Exception("Nenašla sa osoba podľa EDUID: - " + EDUID);

                            osobaKtorejSaNasloEDUID = fo;
                        }

                        using (var transactionScope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
                        {
                            //2.1.1 Systém   fyzickej osobe s daným EDUID zapíše IFO osoby vyhľadanej v RFO (Pre osobu s EDUID = "EDUID" sa zapíše IFO = "IFO")
                            osobaKtorejSaNasloEDUID.Ifo = IFO;
                            osobaKtorejSaNasloEDUID.ZaujmovaOsoba = true;
                            GetDataAccessLayer<ICRUD<FyzickaOsoba>>().Update(osobaKtorejSaNasloEDUID);

                            //2.1.2 Systém  zavolá označenie osoby s  IFO v RFO za záujmovú
                            this.OznacenieZaujmovejOsoby(new List<string>() { IFO }, null, throwExeption);

                            //2.1.4 Systém načíta aktuálne údaje j osoby z RFO 
                            //ak som to uz raz volal, nebudem to volat znova
                            if (vyhladanaOsobaIFOOnline == null)
                                vyhladanaOsobaIFOOnline = this.PoskytnutieReferencnychUdajovZoznamuIFOOnline(new List<string>() { IFO }, null, throwExeption);


                            //2.1.3 Ak IFO pravej osoby <> NULL
                            if (!String.IsNullOrEmpty(vyhladanaOsobaIFOOnline.OsobaList[0].FyzickaOsoba.IfoPravejOsoby))
                                //2.1.3.1   Systém  zavolá označenie osoby s  IFO pravej osoby v RFO za záujmovú
                                this.OznacenieZaujmovejOsoby(new List<string>() { IFOPravejOsoby }, null, throwExeption);

                            //2.1.5 Systém  zavolá aktualizáciu údajov  fyzickej  osoby v RIS na základe údajov osoby z RFO
                            var aktualizovanaOsoba = this.AktualizaciaUdajovFyzickejOsobyUdajmiRFO(vyhladanaOsobaIFOOnline, true, false, null);
                            retVal.SparovanaOsoba = aktualizovanaOsoba[0].Osoba;

                            transactionScope.Complete();
                        }

                        //2.1.6.2   Systém vráti návratovú hodotu 1500 - Spracovanie úspešné
                        retVal.Message = GetBussinessLogicLayer<IBFCodeList>().GetBufferedCodeList<ChybovaSprava>().GetText(51500);
                    }
                    else
                    {
                        //2.1.7.2   pokračuje sa stotožnením osôb v RISe osôb s EDUID a EDUIDNajdene
                        this.StotoznenieOsobRIS(najdenaOsoba.EDUIDNajdene.Value, EDUID, IFO, najdenaOsoba.FyzickaOsobaNajdena, najdenaOsoba.FyzickaOsobaPovodna);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandling.HandleBusinessException(this, ex, 215, MethodInfo.GetCurrentMethod().Name);
                throw new Exception(GetBussinessLogicLayer<IBFCodeList>().GetBufferedCodeList<ChybovaSprava>().GetText(51501), ex);
            }
            return retVal;
        }

        /// <summary>
        /// RFO RIS Zápis individuálneho spárovania osôb RIS-RFO
        /// </summary>
        /// <param name="EDUID"></param>
        /// <param name="IFO"></param>
        /// <param name="IFOPravejOsoby"></param>
        /// <param name="osobaKtorejSaNasloEDUID"></param>
        /// <param name="vyhladanaOsobaIFOOnline"></param>
        /// <param name="throwExeption"></param>
        /// <returns></returns>
        public ZapisSparovanieOsobRisRfoRetVal ZapisindIvidualnehoSparovaniaOsobRisRfo(int? EDUID, string IFO, string IFOPravejOsoby, FyzickaOsoba osobaKtorejSaNasloEDUID = null, OsobaResponse vyhladanaOsobaIFOOnline = null, bool throwExeption = false)
        {
            var retVal = new ZapisSparovanieOsobRisRfoRetVal();
            try
            {
                //ak nemam nacitanu osobu, tak si ju nacitam
                if (osobaKtorejSaNasloEDUID == null)
                {
                    var fo = GetDataAccessLayer<IFyzickaOsoba>().FyzickaOsobaEduIdGet(EDUID.Value);
                    if (fo == null)
                        throw new Exception("Nenašla sa osoba podľa EDUID: - " + EDUID);

                    osobaKtorejSaNasloEDUID = fo;
                }

                using (var transactionScope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
                {
                    //2 Systém   fyzickej osobe s daným EDUID zapíše IFO osoby vyh¾adanej v RFO 
                    osobaKtorejSaNasloEDUID.Ifo = IFO;
                    osobaKtorejSaNasloEDUID.ZaujmovaOsoba = true;
                    GetDataAccessLayer<ICRUD<FyzickaOsoba>>().Update(osobaKtorejSaNasloEDUID);

                    //3 Systém  zavolá oznaèenie osoby s  IFO v RFO za záujmovú
                    this.OznacenieZaujmovejOsoby(new List<string>() { IFO }, null, throwExeption);

                    //5 Systém načíta aktuálne údaje j osoby z RFO 
                    //ak som to uz raz volal, nebudem to volat znova
                    if (vyhladanaOsobaIFOOnline == null)
                        vyhladanaOsobaIFOOnline = this.PoskytnutieReferencnychUdajovZoznamuIFOOnline(new List<string>() { IFO }, null, throwExeption);


                    //4 Ak IFO pravej osoby <> NULL
                    if (!String.IsNullOrEmpty(vyhladanaOsobaIFOOnline.OsobaList[0].FyzickaOsoba.IfoPravejOsoby))
                        //4.1   Systém  zavolá oznaèenie osoby s  IFO pravej osoby v RFO za záujmovú
                        this.OznacenieZaujmovejOsoby(new List<string>() { IFOPravejOsoby }, null, throwExeption);

                    //6 Systém  zavolá aktualizáciu údajov  fyzickej  osoby v RIS na základe údajov osoby z RFO
                    var aktualizovanaOsoba = this.AktualizaciaUdajovFyzickejOsobyUdajmiRFO(vyhladanaOsobaIFOOnline, true, false, null);
                    retVal.SparovanaOsoba = aktualizovanaOsoba[0].Osoba;

                    transactionScope.Complete();
                }

                //2.1.6.2   Systém vráti návratovú hodotu 1500 - Spracovanie úspešné
                retVal.Message = GetBussinessLogicLayer<IBFCodeList>().GetBufferedCodeList<ChybovaSprava>().GetText(51500);
            }
            catch (Exception ex)
            {
                ExceptionHandling.HandleBusinessException(this, ex, 2151, MethodInfo.GetCurrentMethod().Name);
                throw new Exception(GetBussinessLogicLayer<IBFCodeList>().GetBufferedCodeList<ChybovaSprava>().GetText(51501), ex);
            }
            return retVal;
        }


        public void ZapisVystupXML(object input, string name = null)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(input.GetType());
            System.IO.TextWriter WriteFileStream = new System.IO.StreamWriter(Path.Combine(Tools.DestinationFolder, (name == null ? input.GetType().Name : name) + DateTime.Now.ToString("HH-mm-ss") + DateTime.Now.Millisecond + ".xml"));
            serializer.Serialize(WriteFileStream, input);
            WriteFileStream.Close();
        }

        /// <summary>
        /// Ulozi sa do DB zoznam sprav, ktore sa zatial naplnili do zoznamu, zoznam sa vyprazdni
        /// </summary>
        public void UlozLogWS()
        {
            try
            {
                foreach (var item in SIS.Dol.DAVKA.LogSpracovaniaDavok.GetInstance().LogWsList)
                {
                    GetDataAccessLayer<ICRUD<Ditec.RIS.SIS.Dol.DAVKA.LogWs>>().Create(item);
                }
                SIS.Dol.DAVKA.LogSpracovaniaDavok.GetInstance().LogWsList.Clear();
            }
            catch (Exception ex)
            {
                ExceptionHandling.HandleBusinessException(this, ex, 230, MethodInfo.GetCurrentMethod().Name);
            }
        }

        /// <summary>
        /// Zaznacia sa vyplnene udaje do log zaznamu
        /// </summary>
        /// <param name="logZmeny"></param>
        /// <param name="DatumACasVolania"></param>
        /// <param name="TID"></param>
        /// <param name="NavratovyKod"></param>
        /// <param name="PocetDavokVSysteme"></param>
        /// <param name="IDPrvejDavky"></param>
        /// <param name="IDPoslednejDavky"></param>
        /// <param name="IDPoslednejUspesnejDavky"></param>
        /// <param name="SpracovanySubor"></param>
        public void ZapisZmenovyLog(ref Ditec.RIS.SIS.Dol.DAVKA.LogZmenovychSuborov logZmeny, DateTime? DatumACasVolania, string TID, int? NavratovyKod, int? PocetDavokVSysteme, long? IDPrvejDavky, long? IDPoslednejDavky, long? IDPoslednejUspesnejDavky, bool SpracovanySubor)
        {
            if (logZmeny == null)
                logZmeny = new SIS.Dol.DAVKA.LogZmenovychSuborov();
            if (!logZmeny.PocetDavok.HasValue)
                logZmeny.PocetDavok = 0;

            if (DatumACasVolania.HasValue)
                logZmeny.DatumACasVolania = DatumACasVolania.Value;

            if (!String.IsNullOrEmpty(TID))
                logZmeny.TidExternehoSystemu = TID;
            if (NavratovyKod.HasValue)
                logZmeny.NavratovyKod = NavratovyKod;
            if (PocetDavokVSysteme.HasValue)
                logZmeny.PocetDavok += PocetDavokVSysteme;
            if (IDPrvejDavky.HasValue)
                logZmeny.IdentifikatorZmenovejDavkyPrvej = IDPrvejDavky;
            if (IDPoslednejDavky.HasValue)
                logZmeny.IdentifikatorZmenovejDavkyPoslednej = IDPoslednejDavky;
            if (IDPoslednejUspesnejDavky.HasValue)
                logZmeny.IdentifikatorZmenovejDavkyUspesnej = IDPoslednejUspesnejDavky;
            logZmeny.UspesneSpracovany = SpracovanySubor;
        }

        /// <summary>
        /// Vytvoria sa zaznamy, ktore su naplnene
        /// </summary>
        /// <param name="log"></param>
        public void UlozZmenovyLog(Ditec.RIS.SIS.Dol.DAVKA.LogSpracovaniaDavok log)
        {
            try
            {
                if (log.LogZmenovychSuborov.DatumACasVolania != DateTime.MinValue)
                {
                    log.LogZmenovychSuborov = GetDataAccessLayer<ICRUD<Ditec.RIS.SIS.Dol.DAVKA.LogZmenovychSuborov>>().Create(log.LogZmenovychSuborov);
                    foreach (var item in log.ZmenovaDavkaList)
                    {
                        item.LogZmenovychSuborovId = log.LogZmenovychSuborov.ID;
                        GetDataAccessLayer<ICRUD<Ditec.RIS.SIS.Dol.DAVKA.ZmenovaDavka>>().Create(item);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandling.HandleBusinessException(this, ex, 216, MethodInfo.GetCurrentMethod().Name);
            }
        }

        #region Fyzicka Osoba pre browse

        public RequestResult<List<FyzickaOsoba>> FyzickaOsobaListPaged(FyzickaOsobaFilterCriteria filterCriteria)
        {
            var result = new RequestResult<List<FyzickaOsoba>>() { Response = new List<FyzickaOsoba>() };
            try
            {
                var dataOperation = GetDataAccessLayer<IPagedBrowse<FyzickaOsoba>>();
                result.Response = dataOperation.PagedBrowse(ref result, filterCriteria).ToList();
            }
            catch (Exception ex)
            {
                ExceptionHandling.HandleResponse<List<FyzickaOsoba>>(result, this, ex, 214, "Chyba pri načítavaní stránkovaného zoznamu fyzických osôb.");
            }
            return result;
        }

        #endregion

        #region Fyzicka Osoba pre detail

        public RequestResult<Osoba> GetFyzickaOsoba(FyzickaOsobaFilterCriteria filterCriteria)
        {
            var result = new RequestResult<Osoba>();
            try
            {
                result.Response = this.GetOsoba(filterCriteria.ID);
            }
            catch (Exception ex)
            {
                ExceptionHandling.HandleResponse<Osoba>(result, this, ex, 215, "Chyba pri načítavaní detailu fyzickej osoby a .");
            }
            return result;
        }

        #endregion

        #region FyzickaOsobaUpdate

        public RequestResult<FyzickaOsoba> FyzickaOsobaUpdate(FyzickaOsoba dataObject)
        {
            var result = new RequestResult<FyzickaOsoba>();
            try
            {
                // vytvori sa nova databazova transakcia 
                using (var transactionScope = new System.Transactions.TransactionScope())
                {
                    // zo ServiceLocatora sa ziska trieda implementujuca rozhranie IDopravna - DataAccess vrstva 
                    var dataOperation = GetDataAccessLayer<ICRUD<FyzickaOsoba>>();

                    // ulozit objekt Dopravna 
                    result.Response = dataOperation.Update(dataObject);
                    // potvrdenie dokoncenia transakcie 
                    transactionScope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw ExceptionHandling.HandleBusinessException(this, ex, 118, "Chyba pri update záznamu Fyzická osoba s ID='" + dataObject.ID + "'.");
            }

            return result;
        }

        #endregion

        public void SpracovanieXML(string fileName)
        {
            try
            {
                var content = File.ReadAllText(fileName);
                //deserializacia
                var reader = new System.IO.StringReader(content);
                var x = new System.Xml.Serialization.XmlSerializer(typeof(Ditec.RIS.RFO.Dol.PoskytnutieUdajovIFOOnlineWS.TransEnvTypeOut));
                var osobaList = (Ditec.RIS.RFO.Dol.PoskytnutieUdajovIFOOnlineWS.TransEnvTypeOut)x.Deserialize(reader);
                this.SpracovanieZmenovychDavok(osobaList, true);
            }
            catch (Exception ex)
            {
                ExceptionHandling.HandleBusinessException(this, ex, 213, MethodInfo.GetCurrentMethod().Name);
            }
        }

        public void PrekontrolovanieXML(string fileName)
        {
            int zapisane = 0;
            try
            {
                var content = File.ReadAllText(fileName);
                //deserializacia
                var reader = new System.IO.StringReader(content);
                var x = new System.Xml.Serialization.XmlSerializer(typeof(Ditec.RIS.RFO.Dol.PoskytnutieUdajovIFOOnlineWS.TransEnvTypeOut));
                var osobaResponse = (Ditec.RIS.RFO.Dol.PoskytnutieUdajovIFOOnlineWS.TransEnvTypeOut)x.Deserialize(reader);

                var ifoList = File.ReadAllText(Path.Combine(Tools.DestinationFolder, "IFO.CSV")).Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                var dataoperationFindFO = GetDataAccessLayer<IFyzickaOsoba>();
                //pozriem sa, ci mam v DB taky zaznam uz ulozeny
                foreach (var itemOsoba in osobaResponse.POV.OEXList)
                {
                    //ak ten zaznam nemam ulozeny v DB
                    if (!String.IsNullOrEmpty(itemOsoba.ID) && Array.IndexOf(ifoList, itemOsoba.ID) < 0)
                    {
                        var input = new Ditec.RIS.RFO.Dol.PoskytnutieUdajovIFOOnlineWS.TransEnvTypeOut();
                        input.POV = new Dol.PoskytnutieUdajovIFOOnlineWS.TPOVO();
                        input.POV.OEXList = new Dol.PoskytnutieUdajovIFOOnlineWS.TOEX_OEIO[] { itemOsoba };

                        this.SpracovanieZmenovychDavok(input, true);
                        zapisane++;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandling.HandleBusinessException(this, ex, 214, MethodInfo.GetCurrentMethod().Name);
            }
            finally
            {
                if (zapisane != 0)
                {
                    Console.WriteLine("Pre subor " + fileName + " sa vytvorilo " + zapisane + " osob");
                    LogEntryFactory.LogBusinessRulesInformation(this, 100, "Pre subor " + fileName + " sa vytvorilo " + zapisane + " osob");
                }
            }
        }

        public void ProcessDirectory(ProcessType procesType)
        {
            try
            {
                // Process the list of files found in the directory. 
                while (Directory.GetFiles(Tools.TargetFolder) != null && Directory.GetFiles(Tools.TargetFolder).Length > 0)
                {
                    try
                    {
                        var fileName = Directory.GetFiles(Tools.TargetFolder)[0];

                        Console.WriteLine("Spracuvam file " + fileName + " " + DateTime.Now.ToString("HH:mm:ss"));
                        LogEntryFactory.LogBusinessRulesInformation(this, 100, "Spracuvam file " + fileName + " " + DateTime.Now.ToString("HH:mm:ss"));

                        if (procesType == ProcessType.SpracovanieXML)
                            this.SpracovanieXML(fileName);
                        else if (procesType == ProcessType.KontrolaXML)
                            this.PrekontrolovanieXML(fileName);

                        System.IO.File.Move(fileName, Path.Combine(Tools.DestinationFolder, fileName.Remove(0, fileName.LastIndexOf("\\") + 1)));
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandling.HandleBusinessException(this, ex, 313, MethodInfo.GetCurrentMethod().Name);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandling.HandleBusinessException(this, ex, 215, MethodInfo.GetCurrentMethod().Name);
            }
        }

        public void VymazVztahovychUdajovPreZiakovNad18Rokov()
        {
            try
            {
                //1 Systém načíta zoznam fyzických osôb  z RIS, ktoré majú viac ako 18 rokov a majú záznam vo vzťahových údajoch
                var vztahovaOsobaList = GetDataAccessLayer<IVztahovaFyzOsoba>().VztahovaFyzOsobaFORodic();

                //2 Systém pre všetky fyzické osoby  z tohto zoznamu
                foreach (var item in vztahovaOsobaList)
                {
                    //2.1   Systém zruší všetky vzťahy typu rodič zo vzťahových osôb
                    GetDataAccessLayer<ICRUD<VztahovaFyzOsoba>>().Delete(item);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandling.HandleBusinessException(this, ex, 2115, MethodInfo.GetCurrentMethod().Name);
            }
        }

        /// <summary>
        /// RFO WS Zápis nových osôb (cudzinca bez pobytu na území SR)
        /// </summary>
        /// <param name="osoba"></param>
        /// <returns></returns>
        public Dol.ZapisNovychOsobWS.TransEnvTypeOut ZapisNovychOsobDoRFO(Osoba osoba, bool throwException = false)
        {
            var transactionalWS = new TransactionalWS();
            Dol.ZapisNovychOsobWS.TransEnvTypeOut resultResponse = null;

            //Systém prijme požiadavku na zápis cudzinca bez pobytu na území SR
            //Vstupné parametre sú:
            //Údaje o osobe podľa RFO WS Zápis nových osôb - vstup
            //Minimálna množina vstupných parametrov:
            //"Typ osoby" - parameter môže nadobúdať hodnotu z číselníka Typ osoby
            //"Pohlavie" - parameter môže nadobúdať hodnotu z číselníka Pohlavie
            //"Stupeň zverejnenia" - parameter nadobúda hodnotu z číselníka Stupeň zverejnenia 
            //Údaje o priezvisku osoby a poradí priezviska (parameter môže nadobúdať viacero hodnôt priezviska):
            //"Hodnota" - hodnota priezviska. Povinné je zadanie aspoň jednej hodnoty
            //"Poradie priezviska" - poradie priezviska. Povinné je zadanie aspoň jednej hodnoty. Primárne je prvé priezvisko v poradí
            if (!osoba.FyzickaOsoba.RfoTypOsobyIk.HasValue ||
                osoba.FyzickaOsoba.RfoPohlavieIk.Equals(Guid.Empty) ||
                !osoba.FyzickaOsoba.RfoStupenZverejneniaIk.HasValue ||
                osoba.PriezviskoList.Count == 0
                )
                throw new Exception(GetBussinessLogicLayer<IBFCodeList>().GetBufferedCodeList<ChybovaSprava>().GetText(51117));

            try
            {
                var transEnvTypeIn = new Ditec.RIS.RFO.Dol.ZapisNovychOsobWS.TransEnvTypeIn();
                transEnvTypeIn.OEXList = new Dol.ZapisNovychOsobWS.TOEX_OEI[1];
                transEnvTypeIn.OEXList[0] = osoba;

                transEnvTypeIn.UES = new Dol.ZapisNovychOsobWS.TUES();
                transEnvTypeIn.UES.PO = Tools.RisUser;
                transEnvTypeIn.UES.TI = this.TransactionID == null ? "prazdne" : this.TransactionID.ToString();


                resultResponse = transactionalWS.ZapisNovychOsobWSToRfoService(transEnvTypeIn);
                return resultResponse;
            }
            catch (Exception ex)
            {
                ExceptionHandling.HandleBusinessException(this, ex, 223, MethodInfo.GetCurrentMethod().Name);

                if (throwException)
                    throw ex;

                return null;
            }
            finally
            {
                //Zapise sa logWS z volania RFO
                if (Tools.ShouldLogXmlMessages)
                    this.UlozLogWS();
            }
        }

        /// <summary>
        /// RFO WS Zápis úpravy mena (cudzinca bez pobytu na území SR)
        /// </summary>
        /// <param name="osoba"></param>
        /// <param name="throwException"></param>
        /// <returns></returns>
        public Dol.ZapisUpravyMenaWS.TransEnvTypeOut ZapisUpravyMenaDoRFO(string IFO, List<UpdateValue> values, long? identifikatorPoslednejDavky = null, bool throwException = false)
        {
            var transactionalWS = new TransactionalWS();
            Dol.ZapisUpravyMenaWS.TransEnvTypeOut resultResponse = null;

            
            try
            {
                var transEnvTypeIn = new Ditec.RIS.RFO.Dol.ZapisUpravyMenaWS.TransEnvTypeIn();
                transEnvTypeIn.OSO = new Dol.ZapisUpravyMenaWS.TOSO();
                transEnvTypeIn.OSO.ID = IFO;
                if (identifikatorPoslednejDavky.HasValue)
                    transEnvTypeIn.OSO.ZZ = identifikatorPoslednejDavky.Value;

                transEnvTypeIn.OSO.MOSList = new Dol.ZapisUpravyMenaWS.TMOS_ZIE[values.Count];
                for (int i = 0; i < values.Count; i++)
                {
                    transEnvTypeIn.OSO.MOSList[i] = new Dol.ZapisUpravyMenaWS.TMOS_ZIE();
                    switch (values[i].Priznak)
                    {
                        case PriznakAktualizacieOsoby.Oprava: transEnvTypeIn.OSO.MOSList[i].OP = true;
                            break;
                        case PriznakAktualizacieOsoby.Vymazanie: transEnvTypeIn.OSO.MOSList[i].VY = true;
                            break;
                    }

                    if (values[i].DatumZmeny.HasValue)
                        transEnvTypeIn.OSO.MOSList[i].DZ = values[i].DatumZmeny.Value;
                    transEnvTypeIn.OSO.MOSList[i].PO = values[i].Poradie;
                    transEnvTypeIn.OSO.MOSList[i].PH = values[i].PovodnaHodnota;
                    transEnvTypeIn.OSO.MOSList[i].NH = values[i].NovaHodnota;
                }

                transEnvTypeIn.UES = new Dol.ZapisUpravyMenaWS.TUES();
                transEnvTypeIn.UES.PO = Tools.RisUser;
                transEnvTypeIn.UES.TI = this.TransactionID == null ? "prazdne" : this.TransactionID.ToString();


                resultResponse = transactionalWS.ZapisUpravyMenaWSToRfoService(transEnvTypeIn);
                return resultResponse;
            }
            catch (Exception ex)
            {
                ExceptionHandling.HandleBusinessException(this, ex, 224, MethodInfo.GetCurrentMethod().Name);

                if (throwException)
                    throw ex;

                return null;
            }
            finally
            {
                //Zapise sa logWS z volania RFO
                if (Tools.ShouldLogXmlMessages)
                    this.UlozLogWS();
            }
        }

        /// <summary>
        /// RFO WS Zápis úpravy priezviska (cudzinca bez pobytu na území SR)
        /// </summary>
        /// <param name="IFO"></param>
        /// <param name="values"></param>
        /// <param name="identifikatorPoslednejDavky"></param>
        /// <param name="throwException"></param>
        /// <returns></returns>
        public Dol.ZapisUpravyPriezviskaWS.TransEnvTypeOut ZapisUpravyPriezviskaDoRFO(string IFO, List<UpdateValue> values, long? identifikatorPoslednejDavky = null, bool throwException = false)
        {
            var transactionalWS = new TransactionalWS();
            Dol.ZapisUpravyPriezviskaWS.TransEnvTypeOut resultResponse = null;


            try
            {
                var transEnvTypeIn = new Ditec.RIS.RFO.Dol.ZapisUpravyPriezviskaWS.TransEnvTypeIn();
                transEnvTypeIn.OSO = new Dol.ZapisUpravyPriezviskaWS.TOSO();
                transEnvTypeIn.OSO.ID = IFO;
                if (identifikatorPoslednejDavky.HasValue)
                    transEnvTypeIn.OSO.ZZ = identifikatorPoslednejDavky.Value;

                transEnvTypeIn.OSO.PRIList = new Dol.ZapisUpravyPriezviskaWS.TPRI_ZIE[values.Count];
                for (int i = 0; i < values.Count; i++ )
                {
                    transEnvTypeIn.OSO.PRIList[i] = new Dol.ZapisUpravyPriezviskaWS.TPRI_ZIE();
                    switch (values[i].Priznak)
                    {
                        case PriznakAktualizacieOsoby.Oprava: transEnvTypeIn.OSO.PRIList[i].OP = true;
                            break;
                        case PriznakAktualizacieOsoby.Vymazanie: transEnvTypeIn.OSO.PRIList[i].VY = true;
                            break;
                    }

                    if (values[i].DatumZmeny.HasValue)
                        transEnvTypeIn.OSO.PRIList[i].DZ = values[i].DatumZmeny.Value;
                    transEnvTypeIn.OSO.PRIList[i].PO = values[i].Poradie;
                    transEnvTypeIn.OSO.PRIList[i].PH = values[i].PovodnaHodnota;
                    transEnvTypeIn.OSO.PRIList[i].NH = values[i].NovaHodnota;
                }

                transEnvTypeIn.UES = new Dol.ZapisUpravyPriezviskaWS.TUES();
                transEnvTypeIn.UES.PO = Tools.RisUser;
                transEnvTypeIn.UES.TI = this.TransactionID == null ? "prazdne" : this.TransactionID.ToString();


                resultResponse = transactionalWS.ZapisUpravyPriezviskaWSToRfoService(transEnvTypeIn);
                return resultResponse;
            }
            catch (Exception ex)
            {
                ExceptionHandling.HandleBusinessException(this, ex, 225, MethodInfo.GetCurrentMethod().Name);

                if (throwException)
                    throw ex;

                return null;
            }
            finally
            {
                //Zapise sa logWS z volania RFO
                if (Tools.ShouldLogXmlMessages)
                    this.UlozLogWS();
            }
        }

		#region EduIdPravejOsoby
		/// <summary>
		/// EDU ID pravej odsoby podla EDU ID fyzickej osoby
		/// </summary>
		/// <param name="filterCriteria"></param>
		/// <returns></returns>
		public RequestResult<Int32?> GetEduIdPravejOsoby(FyzickaOsoba fyzickaOsoba)
		{
			var result = new RequestResult<Int32?>() { Response = null };
			try
			{
				//Fyzická osoba.EDUID, pre ktorú platí:
				//Fyzická osoba.EDUID.Stotožnená osoba.EDUID pôvodnej osoby = EDUID práve zobrazovanej osoby
				var dataOperation = GetDataAccessLayer<IBrowse<StotoznenaFyzOsoba>>();
				var stotoznanaOsobaList = dataOperation.Browse(new StotoznenaFyzOsobaFilterCriteria() { EduidPovodne = fyzickaOsoba.Eduid });
				{
					if (stotoznanaOsobaList != null && stotoznanaOsobaList.Count > 0)
					{
						var dataOperationFyzOsoba = GetDataAccessLayer<ICRUD<FyzickaOsoba>>();
						var fyzOsoba = dataOperationFyzOsoba.Read(new FyzickaOsobaFilterCriteria() { ID = stotoznanaOsobaList[0].FyzickaOsobaId });
						if (fyzOsoba != null)
						{
							result.Response = fyzOsoba.Eduid;
						}
					}
				}
			}
			catch (Exception ex)
			{
				ExceptionHandling.HandleResponse<Int32?>(result, this, ex, 126, "Chyba pri načítavaní EDU ID pravej osoby.");
			}
			return result;
		}

		#endregion
    }
}
