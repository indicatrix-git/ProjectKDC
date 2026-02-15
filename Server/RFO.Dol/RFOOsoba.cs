using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ditec.RIS.RFO.Dol
{
    public class OsobaResponse
    {
        
        //private List<CC.Dol.RfoNarodnost> _RfoNarodnostList;
        //public virtual List<CC.Dol.RfoNarodnost> RfoNarodnostList
        //{
        //    get
        //    {
        //        return _RfoNarodnostList;
        //    }
        //    set
        //    {
        //        if (value != _RfoNarodnostList)
        //        {
        //            _RfoNarodnostList = value;
        //        }
        //    }
        //}

        /// <summary>
        /// Osoba 
        /// </summary>
        public List<Osoba> OsobaList { get; set; }
    }

    [DataContract]
    public partial class Osoba : BaseDataMember
    {
        public Osoba()
        {
            this.FyzickaOsoba = new FyzickaOsoba();

            this.MenoList = new List<Meno>();
            this.PravnaSposobilostObmedzenieList = new List<PravnaSposobilostObmedzenie>();
            this.PriezviskoList = new List<Priezvisko>();
            this.RodnePriezviskoList = new List<RodnePriezvisko>();
            this.StatnaPrislusnostList = new List<FyzickaOsobaStatnaPrislusnost>();
            this.StotoznenaFyzOsobaList = new List<StotoznenaFyzOsoba>();
            this.TitulList = new List<FyzickaOsobaTitul>();
            this.UdajePobytuList = new List<UdajePobytu>();
            this.VztahovaOsobaList = new List<VztahovaFyzOsoba>();
            this.ZakazPobytuList = new List<ZakazPobytu>();
        }

        private FyzickaOsoba _FyzickaOsoba;
		[DataMember]
        public virtual FyzickaOsoba FyzickaOsoba
        {
            get
            {
                if (_FyzickaOsoba == null)
                    _FyzickaOsoba = new FyzickaOsoba();

                return _FyzickaOsoba;
            }
            set
            {
                if (value != _FyzickaOsoba)
                {
                    _FyzickaOsoba = value;
                }
            }
        }

        /// <summary>
        /// Meno 
        /// </summary>
		[DataMember]
		public List<Meno> MenoList { get; set; }
        
        /// <summary>
        /// Priezvisko 
        /// </summary>
		[DataMember]
		public List<Priezvisko> PriezviskoList { get; set; }
        
        /// <summary>
        /// RodnePriezvisko 
        /// </summary>
		[DataMember]
		public List<RodnePriezvisko> RodnePriezviskoList { get; set; }

        /// <summary>
        /// Titul 
        /// </summary>
		[DataMember]
		public List<FyzickaOsobaTitul> TitulList { get; set; }
        
        /// <summary>
        /// StatnaPrislusnost 
        /// </summary>
		[DataMember]
		public List<FyzickaOsobaStatnaPrislusnost> StatnaPrislusnostList { get; set; }
        
        /// <summary>
        /// VztahovaOsoba 
        /// </summary>
		[DataMember]
		public List<VztahovaFyzOsoba> VztahovaOsobaList { get; set; }
        
        /// <summary>
        /// PravnaSposobilostObmedzenie 
        /// </summary>
		[DataMember]
		public List<PravnaSposobilostObmedzenie> PravnaSposobilostObmedzenieList { get; set; }

		/// <summary>
		/// StotoznenaFyzOsoba 
		/// </summary>
		[DataMember]
		public List<StotoznenaFyzOsoba> StotoznenaFyzOsobaList { get; set; }

        /// <summary>
        /// UdajeOZakazePobytu 
        /// </summary>
		[DataMember]
		public List<ZakazPobytu> ZakazPobytuList { get; set; }
        
        /// <summary>
        /// UdajePobytu 
        /// </summary>
		[DataMember]
		public List<UdajePobytu> UdajePobytuList { get; set; }

    }

    public class AktualizovanaOsoba
    {
        
        /// <summary>
        /// Novozapísaná osoba"  - príznak, či spracovávaná fyzická osoba je novozapísaná 
        /// </summary>
        public bool Novozapisana { get; set; }
        
        /// <summary>
        /// EDUID 
        /// </summary>
        public int? EDUID { get; set; }
        
        /// <summary>
        /// ErrorMessage 
        /// </summary>
        public string ErrorMessage { get; set; }
        
        /// <summary>
        /// Osoba 
        /// </summary>
        public Osoba Osoba { get; set; }

    }

    public class ZapisSparovanieOsobRisRfoRetVal
    {
        /// <summary>
        /// Message 
        /// </summary>
        public string Message { get; set; }
        
        /// <summary>
        /// SparovanaOsoba 
        /// </summary>
        public Osoba SparovanaOsoba { get; set; }

    }
}
