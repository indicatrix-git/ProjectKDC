using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ditec.RIS.SysFra.DataAccess.Infrastructure;
using Ditec.SysFra.DataTypes.Infrastructure;
using Ditec.SysFra.Infrastructure.Impl;
using Ditec.SysFra.NhSql.Dal;
using LinFu.IoC.Configuration;
using NHibernate;
using Ditec.RIS.RFO.Dal.Inf;
using Ditec.RIS.RFO.Dol;

namespace Ditec.RIS.RFO.Dal.Repository
{
    [Implements(typeof(IFyzickaOsoba))]
    public partial class FyzickaOsobaRepository : IFyzickaOsoba
    {
        #region FyzickaOsobaFind
        private const string FyzickaOsobaFindQuery = "FyzickaOsobaFind";
        public List<FyzickaOsoba> FyzickaOsobaFind(string RodneCislo, DateTime? DatumNarodenia, List<Meno> menoList, List<Priezvisko> priezviskoList, List<RodnePriezvisko> rodnePriezviskoList, Guid? stupenZverejnenia = null)
        {
            var response = new List<FyzickaOsoba>();
            try
            {
                string nullValue = null;
                using (ISession session = SessionProvider.OpenSession())
                {
                    IQuery query = session.GetNamedQuery(FyzickaOsobaFindQuery);
                    System.Xml.Serialization.XmlSerializer serializer;
                    System.Xml.XmlWriterSettings settings;

                    settings = new System.Xml.XmlWriterSettings();
                    settings.Encoding = new UnicodeEncoding(false, false); // no BOM in a .NET string
                    settings.Indent = true;
                    settings.OmitXmlDeclaration = true;

                    if (menoList == null || menoList.Count == 0)
                        query.SetParameter("MENO", nullValue);
                    else
                    {
                        serializer = new System.Xml.Serialization.XmlSerializer(menoList.GetType());

                        using (System.IO.StringWriter textWriter = new System.IO.StringWriter())
                        {
                            using (System.Xml.XmlWriter xmlWriter = System.Xml.XmlWriter.Create(textWriter, settings))
                            {
                                serializer.Serialize(xmlWriter, menoList);
                            }
                            query.SetParameter("MENO", textWriter.ToString());
                        }
                    }

                    if (priezviskoList == null || priezviskoList.Count == 0)
                        query.SetParameter("PRIEZVISKO", nullValue);
                    else
                    {
                        serializer = new System.Xml.Serialization.XmlSerializer(priezviskoList.GetType());
                        using (System.IO.StringWriter textWriter = new System.IO.StringWriter())
                        {
                            using (System.Xml.XmlWriter xmlWriter = System.Xml.XmlWriter.Create(textWriter, settings))
                            {
                                serializer.Serialize(xmlWriter, priezviskoList);
                            }
                            query.SetParameter("PRIEZVISKO", textWriter.ToString());
                        }
                    }

                    if (rodnePriezviskoList == null || rodnePriezviskoList.Count == 0)
                        query.SetParameter("RODNE_PRIEZVISKO", nullValue);
                    else
                    {
                        serializer = new System.Xml.Serialization.XmlSerializer(rodnePriezviskoList.GetType());
                        using (System.IO.StringWriter textWriter = new System.IO.StringWriter())
                        {
                            using (System.Xml.XmlWriter xmlWriter = System.Xml.XmlWriter.Create(textWriter, settings))
                            {
                                serializer.Serialize(xmlWriter, rodnePriezviskoList);
                            }
                            query.SetParameter("RODNE_PRIEZVISKO", textWriter.ToString());
                        }
                    }

                    query.SetParameter("RODNE_CISLO", RodneCislo);
                    query.SetParameter("DATUM_NARODENIA", DatumNarodenia);
                    query.SetParameter("RFO_STUPEN_ZVEREJNENIA_IK", stupenZverejnenia);

                    response = query.List<FyzickaOsoba>().ToList();
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw ExceptionHandling.HandleTechnologicalException(this, ex, 108);
            }
        }

        #endregion FyzickaOsobaFind

        #region FyzickaOsobaFindVazby

        private const string FyzickaOsobaFindVazbyQuery = "FyzickaOsobaFindVazby";
        public List<TypOsobyVRis> FyzickaOsobaFindVazby(int EDUID)
        {
            var response = new List<TypOsobyVRis>();
            try
            {
                using (ISession session = SessionProvider.OpenSession())
                {
                    IQuery query = session.GetNamedQuery(FyzickaOsobaFindVazbyQuery);

                    query.SetParameter("EDUID", EDUID);

                    var retVal = query.List();
                    foreach (var item in retVal)
                    {
                        response.Add(((string)item).GetTypOsobyVRisByString());
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw ExceptionHandling.HandleTechnologicalException(this, ex, 109);
            }
        }

        #endregion FyzickaOsobaFindVazby

        #region FyzickaOsobaCreateBroker

        private const string FyzickaOsobaCreateBrokerQuery = "FyzickaOsobaCreateBroker";
        public Osoba FyzickaOsobaCreateBroker(Osoba osoba)
        {
            try
            {
                using (ISession session = SessionProvider.OpenSession())
                {
                    IQuery query = session.GetNamedQuery(FyzickaOsobaCreateBrokerQuery);

                    query.SetParameter("XML", ToolsRFO.GetXmlString(osoba));

                    var xml = query.UniqueResult<string>();

                    var reader = new System.IO.StringReader(xml);
                    var x = new System.Xml.Serialization.XmlSerializer(typeof(Osoba));
                    var retVal = (Osoba)x.Deserialize(reader);
                    return retVal;
                }
            }
            catch (Exception ex)
            {
                throw ExceptionHandling.HandleTechnologicalException(this, ex, 110);
            }
        }

        #endregion FyzickaOsobaCreateBroker

        #region FyzickaOsobaIfoList

        private const string FyzickaOsobaIfoListQuery = "FyzickaOsobaIfoList";

        public List<FyzickaOsoba> FyzickaOsobaIfoList(object criteria)
        {
            var response = new List<FyzickaOsoba>();
            try
            {
                var filterCriteria = criteria as FyzickaOsobaFilterCriteria;
                using (ISession session = SessionProvider.OpenSession())
                {
                    IQuery query = session.GetNamedQuery(FyzickaOsobaIfoListQuery);
                    query.SetProperties(filterCriteria);

                    if (query.NamedParameters.Contains("DATUM_SIMULACIE"))
                    {
                        if (filterCriteria.DatumPlatnosti.HasValue)
                            query.SetParameter("DATUM_SIMULACIE", filterCriteria.DatumPlatnosti.Value);
                        else
                            query.SetParameter("DATUM_SIMULACIE", DateTime.Now);
                    }

                    response = query.List<FyzickaOsoba>().ToList();
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw ExceptionHandling.HandleTechnologicalException(this, ex, 111);
            }
        }

        #endregion FyzickaOsobaIfoList

        #region FyzickaOsobaEduIdGet

        private const string FyzickaOsobaEDUIDGetQuery = "FyzickaOsobaEduIdGet";

        public FyzickaOsoba FyzickaOsobaEduIdGet(int eduid)
        {
            try
            {
                using (ISession session = SessionProvider.OpenSession())
                {
                    IQuery query = session.GetNamedQuery(FyzickaOsobaEDUIDGetQuery);
                    query.SetParameter("EDUID", eduid);
                    var response = query.UniqueResult<FyzickaOsoba>();
                    return response as FyzickaOsoba;
                }
            }
            catch (Exception ex)
            {
                throw ExceptionHandling.HandleTechnologicalException(this, ex, 112);
            }
        }

        #endregion FyzickaOsobaEduIdGet
    }

    [Implements(typeof(IStotoznenaFyzOsoba))]
    public partial class StotoznenaFyzOsobaRepository : IStotoznenaFyzOsoba
    {
        #region StotoznenaFyzOsobaIfoList

        private const string StotoznenaFyzOsobaIfoListQuery = "StotoznenaFyzOsobaIfoList";

        public List<StotoznenaFyzOsoba> StotoznenaFyzOsobaIfoList(object criteria)
        {
            var response = new List<StotoznenaFyzOsoba>();
            try
            {
                var filterCriteria = criteria as StotoznenaFyzOsobaFilterCriteria;
                using (ISession session = SessionProvider.OpenSession())
                {
                    IQuery query = session.GetNamedQuery(StotoznenaFyzOsobaIfoListQuery);
                    query.SetProperties(filterCriteria);

                    if (query.NamedParameters.Contains("DATUM_SIMULACIE"))
                    {
                        if (filterCriteria.DatumPlatnosti.HasValue)
                            query.SetParameter("DATUM_SIMULACIE", filterCriteria.DatumPlatnosti.Value);
                        else
                            query.SetParameter("DATUM_SIMULACIE", DateTime.Now);
                    }

                    response = query.List<StotoznenaFyzOsoba>().ToList();
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw ExceptionHandling.HandleTechnologicalException(this, ex, 105);
            }
        }
        #endregion StotoznenaFyzOsobaIfoList

        #region StotoznenaFyzOsobaFOGet

        private const string StotoznenaFyzOsobaFOGetQuery = "StotoznenaFyzOsobaFOGet";

        public List<StotoznenaFyzOsoba> StotoznenaFyzOsobaFOGet(Guid FyzickaOsobaID)
        {
            try
            {
                using (ISession session = SessionProvider.OpenSession())
                {
                    IQuery query = session.GetNamedQuery(StotoznenaFyzOsobaFOGetQuery);
                    query.SetParameter("FYZICKA_OSOBA_ID", FyzickaOsobaID.ToString());
                    return query.List<StotoznenaFyzOsoba>().ToList();
                }
            }
            catch (Exception ex)
            {
                throw ExceptionHandling.HandleTechnologicalException(this, ex, 104);
            }
        }

        #endregion StotoznenaFyzOsobaFOGet

        #region StotoznenaFyzickaOsobaFind

        private const string StotoznenaFyzickaOsobaFindQuery = "StotoznenaFyzickaOsobaFind";
        public List<StotoznenaFyzOsoba> StotoznenaFyzickaOsobaFind(string RodneCislo, DateTime? DatumNarodenia, List<Meno> menoList, List<Priezvisko> priezviskoList, List<RodnePriezvisko> rodnePriezviskoList, Guid? stupenZverejnenia = null)
        {
            var response = new List<StotoznenaFyzOsoba>();
            try
            {
                string nullValue = null;
                using (ISession session = SessionProvider.OpenSession())
                {
                    IQuery query = session.GetNamedQuery(StotoznenaFyzickaOsobaFindQuery);
                    System.Xml.Serialization.XmlSerializer serializer;
                    System.Xml.XmlWriterSettings settings;

                    settings = new System.Xml.XmlWriterSettings();
                    settings.Encoding = new UnicodeEncoding(false, false); // no BOM in a .NET string
                    settings.Indent = true;
                    settings.OmitXmlDeclaration = true;

                    if (menoList == null || menoList.Count == 0)
                        query.SetParameter("MENO", nullValue);
                    else
                    {
                        serializer = new System.Xml.Serialization.XmlSerializer(menoList.GetType());

                        using (System.IO.StringWriter textWriter = new System.IO.StringWriter())
                        {
                            using (System.Xml.XmlWriter xmlWriter = System.Xml.XmlWriter.Create(textWriter, settings))
                            {
                                serializer.Serialize(xmlWriter, menoList);
                            }
                            query.SetParameter("MENO", textWriter.ToString());
                        }
                    }

                    if (priezviskoList == null || priezviskoList.Count == 0)
                        query.SetParameter("PRIEZVISKO", nullValue);
                    else
                    {
                        serializer = new System.Xml.Serialization.XmlSerializer(priezviskoList.GetType());
                        using (System.IO.StringWriter textWriter = new System.IO.StringWriter())
                        {
                            using (System.Xml.XmlWriter xmlWriter = System.Xml.XmlWriter.Create(textWriter, settings))
                            {
                                serializer.Serialize(xmlWriter, priezviskoList);
                            }
                            query.SetParameter("PRIEZVISKO", textWriter.ToString());
                        }
                    }

                    if (rodnePriezviskoList == null || rodnePriezviskoList.Count == 0)
                        query.SetParameter("RODNE_PRIEZVISKO", nullValue);
                    else
                    {
                        serializer = new System.Xml.Serialization.XmlSerializer(rodnePriezviskoList.GetType());
                        using (System.IO.StringWriter textWriter = new System.IO.StringWriter())
                        {
                            using (System.Xml.XmlWriter xmlWriter = System.Xml.XmlWriter.Create(textWriter, settings))
                            {
                                serializer.Serialize(xmlWriter, rodnePriezviskoList);
                            }
                            query.SetParameter("RODNE_PRIEZVISKO", textWriter.ToString());
                        }
                    }

                    query.SetParameter("RODNE_CISLO", RodneCislo);
                    query.SetParameter("DATUM_NARODENIA", DatumNarodenia);
                    query.SetParameter("RFO_STUPEN_ZVEREJNENIA_IK", stupenZverejnenia);

                    response = query.List<StotoznenaFyzOsoba>().ToList();
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw ExceptionHandling.HandleTechnologicalException(this, ex, 128);
            }
        }

        #endregion StotoznenaFyzickaOsobaFind

        #region StotoznenaFyzickaOsobaEDUIDList

        private const string StotoznenaFyzickaOsobaEDUIDListQuery = "StotoznenaFyzickaOsobaEDUIDList";
        public List<StotoznenaFyzOsoba> StotoznenaFyzickaOsobaEDUIDList(int EDUID)
        {
            try
            {
                using (ISession session = SessionProvider.OpenSession())
                {
                    IQuery query = session.GetNamedQuery(StotoznenaFyzickaOsobaEDUIDListQuery);
                    query.SetParameter("EDUID", EDUID);
                    var response = query.List<StotoznenaFyzOsoba>().ToList();
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw ExceptionHandling.HandleTechnologicalException(this, ex, 129);
            }
        }

        #endregion StotoznenaFyzickaOsobaEDUIDList
    }

    [Implements(typeof(IFyzickaOsobaStatnaPrislusnost))]
    public partial class FyzickaOsobaStatnaPrislusnostRepository : IFyzickaOsobaStatnaPrislusnost
    {
        #region FyzickaOsobaStatnaPrislusnostFOGet

        private const string FyzickaOsobaStatnaPrislusnostFOGetQuery = "FyzickaOsobaStatnaPrislusnostFOGet";

        public List<FyzickaOsobaStatnaPrislusnost> FyzickaOsobaStatnaPrislusnostFOGet(Guid FyzickaOsobaID)
        {
            try
            {
                using (ISession session = SessionProvider.OpenSession())
                {
                    IQuery query = session.GetNamedQuery(FyzickaOsobaStatnaPrislusnostFOGetQuery);
                    query.SetParameter("FYZICKA_OSOBA_ID", FyzickaOsobaID.ToString());
                    return query.List<FyzickaOsobaStatnaPrislusnost>().ToList();
                }
            }
            catch (Exception ex)
            {
                throw ExceptionHandling.HandleTechnologicalException(this, ex, 112);
            }
        }

        #endregion FyzickaOsobaStatnaPrislusnostFOGet
    }

    [Implements(typeof(IPriezvisko))]
    public partial class PriezviskoRepository : IPriezvisko
    {
        #region PriezviskoFOGet

        private const string PriezviskoFOGetQuery = "PriezviskoFOGet";

        public List<Priezvisko> PriezviskoFOGet(Guid FyzickaOsobaID)
        {
            try
            {
                using (ISession session = SessionProvider.OpenSession())
                {
                    IQuery query = session.GetNamedQuery(PriezviskoFOGetQuery);
                    query.SetParameter("FYZICKA_OSOBA_ID", FyzickaOsobaID.ToString());
                    return query.List<Priezvisko>().ToList();
                }
            }
            catch (Exception ex)
            {
                throw ExceptionHandling.HandleTechnologicalException(this, ex, 104);
            }
        }

        #endregion PriezviskoFOGet
    }

    [Implements(typeof(IMeno))]
    public partial class MenoRepository : IMeno
    {
        #region MenoFOGet

        private const string MenoFOGetQuery = "MenoFOGet";

        public List<Meno> MenoFOGet(Guid FyzickaOsobaID)
        {
            try
            {
                using (ISession session = SessionProvider.OpenSession())
                {
                    IQuery query = session.GetNamedQuery(MenoFOGetQuery);
                    query.SetParameter("FYZICKA_OSOBA_ID", FyzickaOsobaID.ToString());
                    return query.List<Meno>().ToList();
                }
            }
            catch (Exception ex)
            {
                throw ExceptionHandling.HandleTechnologicalException(this, ex, 104);
            }
        }

        #endregion MenoFOGet
    }

    [Implements(typeof(IUdajePobytu))]
    public partial class UdajePobytuRepository : IUdajePobytu
    {
        #region UdajePobytuFOGet

        private const string UdajePobytuFOGetQuery = "UdajePobytuFyzOsGet";

        public List<UdajePobytu> UdajePobytuFOGet(Guid FyzickaOsobaID)
        {
            try
            {
                using (ISession session = SessionProvider.OpenSession())
                {
                    IQuery query = session.GetNamedQuery(UdajePobytuFOGetQuery);
                    query.SetParameter("FYZICKA_OSOBA_ID", FyzickaOsobaID.ToString());
                    return query.List<UdajePobytu>().ToList();
                }
            }
            catch (Exception ex)
            {
                throw ExceptionHandling.HandleTechnologicalException(this, ex, 104);
            }
        }

        #endregion UdajePobytuFOGet
    }

    [Implements(typeof(IRodnePriezvisko))]
    public partial class RodnePriezviskoRepository : IRodnePriezvisko
    {
        #region RodnePriezviskoFOGet

        private const string RodnePriezviskoFOGetQuery = "RodnePriezviskoFOGet";

        public List<RodnePriezvisko> RodnePriezviskoFOGet(Guid FyzickaOsobaID)
        {
            try
            {
                using (ISession session = SessionProvider.OpenSession())
                {
                    IQuery query = session.GetNamedQuery(RodnePriezviskoFOGetQuery);
                    query.SetParameter("FYZICKA_OSOBA_ID", FyzickaOsobaID.ToString());
                    return query.List<RodnePriezvisko>().ToList();
                }
            }
            catch (Exception ex)
            {
                throw ExceptionHandling.HandleTechnologicalException(this, ex, 104);
            }
        }

        #endregion RodnePriezviskoFOGet
    }

    [Implements(typeof(IFyzickaOsobaTitul))]
    public partial class FyzickaOsobaTitulRepository : IFyzickaOsobaTitul
    {
        #region FyzickaOsobaTitulFOGet

        private const string FyzickaOsobaTitulFOGetQuery = "FyzickaOsobaTitulFyzOsGet";

        public List<FyzickaOsobaTitul> FyzickaOsobaTitulFOGet(Guid FyzickaOsobaID)
        {
            try
            {
                using (ISession session = SessionProvider.OpenSession())
                {
                    IQuery query = session.GetNamedQuery(FyzickaOsobaTitulFOGetQuery);
                    query.SetParameter("FYZICKA_OSOBA_ID", FyzickaOsobaID.ToString());
                    return query.List<FyzickaOsobaTitul>().ToList();
                }
            }
            catch (Exception ex)
            {
                throw ExceptionHandling.HandleTechnologicalException(this, ex, 104);
            }
        }

        #endregion FyzickaOsobaTitulFOGet
    }
    
    [Implements(typeof(IVztahovaFyzOsoba))]
    public partial class VztahovaFyzOsobaRepository : IVztahovaFyzOsoba
    {
        #region VztahovaFyzOsobaFOGet

        private const string VztahovaFyzOsobaFOGetQuery = "VztahovaFyzOsobaFOGet";

        public List<VztahovaFyzOsoba> VztahovaFyzOsobaFOGet(Guid FyzickaOsobaID)
        {
            try
            {
                using (ISession session = SessionProvider.OpenSession())
                {
                    IQuery query = session.GetNamedQuery(VztahovaFyzOsobaFOGetQuery);
                    query.SetParameter("FYZICKA_OSOBA_ID", FyzickaOsobaID.ToString());
                    return query.List<VztahovaFyzOsoba>().ToList();
                }
            }
            catch (Exception ex)
            {
                throw ExceptionHandling.HandleTechnologicalException(this, ex, 104);
            }
        }

        #endregion VztahovaFyzOsobaFOGet

        #region VztahovaFyzOsobaFORodic

        private const string VztahovaFyzOsobaFORodicQuery = "VztahovaFyzOsobaFORodic";

        public List<VztahovaFyzOsoba> VztahovaFyzOsobaFORodic(Guid? FyzickaOsobaID = null, DateTime? DatumSimulacie = null)
        {
            try
            {
                using (ISession session = SessionProvider.OpenSession())
                {
                    IQuery query = session.GetNamedQuery(VztahovaFyzOsobaFORodicQuery);
                    query.SetParameter("FYZICKA_OSOBA_ID", FyzickaOsobaID);
                    query.SetParameter("DatumSimulacie", DatumSimulacie);
                    return query.List<VztahovaFyzOsoba>().ToList();
                }
            }
            catch (Exception ex)
            {
                throw ExceptionHandling.HandleTechnologicalException(this, ex, 108);
            }
        }

        #endregion VztahovaFyzOsobaFORodic
    }

    [Implements(typeof(IPravnaSposobilostObmedzenie))]
    public partial class PravnaSposobilostObmedzenieRepository : IPravnaSposobilostObmedzenie
    {
        #region PravnaSposobilostObmedzenieFOGet

        private const string PravnaSposobilostObmedzenieFOGetQuery = "PravnaSposobilostObmedzenieFOGet";

        public List<PravnaSposobilostObmedzenie> PravnaSposobilostObmedzenieFOGet(Guid FyzickaOsobaID)
        {
            try
            {
                using (ISession session = SessionProvider.OpenSession())
                {
                    IQuery query = session.GetNamedQuery(PravnaSposobilostObmedzenieFOGetQuery);
                    query.SetParameter("FYZICKA_OSOBA_ID", FyzickaOsobaID.ToString());
                    return query.List<PravnaSposobilostObmedzenie>().ToList();
                }
            }
            catch (Exception ex)
            {
                throw ExceptionHandling.HandleTechnologicalException(this, ex, 104);
            }
        }

        #endregion PravnaSposobilostObmedzenieFOGet
    }

    [Implements(typeof(IZakazPobytu))]
    public partial class ZakazPobytuRepository : IZakazPobytu
    {
        #region ZakazPobytuFOGet

        private const string ZakazPobytuFOGetQuery = "ZakazPobytuFOGet";

        public List<ZakazPobytu> ZakazPobytuFOGet(Guid FyzickaOsobaID)
        {
            try
            {
                using (ISession session = SessionProvider.OpenSession())
                {
                    IQuery query = session.GetNamedQuery(ZakazPobytuFOGetQuery);
                    query.SetParameter("FYZICKA_OSOBA_ID", FyzickaOsobaID.ToString());
                    return query.List<ZakazPobytu>().ToList();
                }
            }
            catch (Exception ex)
            {
                throw ExceptionHandling.HandleTechnologicalException(this, ex, 104);
            }
        }

        #endregion ZakazPobytuFOGet
    }
}
