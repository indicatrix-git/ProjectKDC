using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Ditec.RIS.CC.Dol;
using Ditec.RIS.RFO.Dol;

namespace Ditec.RIS.RFO.Dol
{
    public static class ToolsRFO
    {
        public static String GetStringFromValue(this TypOsobyVRis value)
        {
            string output = null;
            switch (value)
            {
                case TypOsobyVRis.Nezadane: output = "nezadané";
                    break;
                case TypOsobyVRis.Ziak: output = "žiak";
                    break;
                case TypOsobyVRis.Rodic: output = "rodič";
                    break;
                case TypOsobyVRis.Zamestnanec: output = "zamestnanec";
                    break;
                case TypOsobyVRis.Pouzivatel: output = "používateľ";
                    break;
                default: output = "nezadané";
                    break;
            }
            return output;
        }
        public static TypOsobyVRis GetTypOsobyVRisByString(this string name)
        {
            TypOsobyVRis output = TypOsobyVRis.Nezadane;
            switch (name)
            {
                case "nezadané": output = TypOsobyVRis.Nezadane;
                    break;
                case "žiak": output = TypOsobyVRis.Ziak;
                    break;
                case "rodič": output = TypOsobyVRis.Rodic;
                    break;
                case "zamestnanec": output = TypOsobyVRis.Zamestnanec;
                    break;
                case "používateľ": output = TypOsobyVRis.Pouzivatel;
                    break;
                default: output = TypOsobyVRis.Nezadane;
                    break;
            }
            return output;
        }

        public static string GetXmlString(object input)
        {
            if (input == null)
                return null;

            string retval;
            try
            {
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(input.GetType());
                System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
                settings.Encoding = new UnicodeEncoding(false, false); // no BOM in a .NET string
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                using (System.IO.StringWriter textWriter = new System.IO.StringWriter())
                {
                    using (System.Xml.XmlWriter xmlWriter = System.Xml.XmlWriter.Create(textWriter, settings))
                    {
                        serializer.Serialize(xmlWriter, input);
                    }
                    retval = textWriter.ToString();
                }

                return retval;
            }
            catch
            {
                throw;
            }
        }
    }
    public enum TypOsobyVRis
    {
        Nezadane,
        Ziak,
        Rodic,
        Zamestnanec,
        Pouzivatel
    }

    public enum Spracovanie { Individualne = 0, Hromadne = 1}

    public enum PriznakAktualizacieOsoby { Oprava, Vymazanie };

    public class UpdateValue
    {
        /// <summary>
        /// Poradie 
        /// </summary>
        public int Poradie { get; set; }
        
        /// <summary>
        /// Priznak 
        /// </summary>
        public PriznakAktualizacieOsoby Priznak { get; set; }
        
        /// <summary>
        /// PovodnaHodnota 
        /// </summary>
        public string PovodnaHodnota { get; set; }
        
        /// <summary>
        /// NovaHodnota 
        /// </summary>
        public string NovaHodnota { get; set; }
        
        /// <summary>
        /// DatumZmeny 
        /// </summary>
        public DateTime? DatumZmeny { get; set; }
    }

    public static class FyzickaOsobaExtension
    {
        public static bool ExistFyzickaOsoba(this IList<FyzickaOsoba> list)
        {
            return (list != null && list.Count > 0);
        }
    }

    public static class StotoznenaFyzOsobaExtension
    {
        public static bool ExistStotoznenaFyzOsoba(this IList<StotoznenaFyzOsoba> list)
        {
            return (list != null && list.Count > 0);
        }
    }

    public partial class FyzickaOsoba
    {
        private string _IfoPravejOsoby;
        [DataMember]
        public virtual string IfoPravejOsoby
        {
            get
            {
                return _IfoPravejOsoby;
            }
            set
            {
                if (value != _IfoPravejOsoby)
                {
                    _IfoPravejOsoby = value;
                    OnPropertyChanged("IfoPravejOsoby");
                }
            }
        }

        private int? _UzemnyCelokNarodeniaKod;
        [DataMember]
        public virtual int? UzemnyCelokNarodeniaKod
        {
            get
            {
                return _UzemnyCelokNarodeniaKod;
            }
            set
            {
                if (value != _UzemnyCelokNarodeniaKod)
                {
                    _UzemnyCelokNarodeniaKod = value;
                    OnPropertyChanged("UzemnyCelokNarodeniaKod");
                }
            }
        }
        
        private int? _StatNarodeniaKod;
        [DataMember]
        public virtual int? StatNarodeniaKod
        {
            get
            {
                return _StatNarodeniaKod;
            }
            set
            {
                if (value != _StatNarodeniaKod)
                {
                    _StatNarodeniaKod = value;
                    OnPropertyChanged("StatNarodeniaKod");
                }
            }
        }

        private string _PohlavieKod;
        [DataMember]
        public virtual string PohlavieKod
        {
            get
            {
                return _PohlavieKod;
            }
            set
            {
                if (value != _PohlavieKod)
                {
                    _PohlavieKod = value;
                    OnPropertyChanged("PohlavieKod");
                }
            }
        }
       
        private int? _RodinnyStavKod;
        [DataMember]
        public virtual int? RodinnyStavKod
        {
            get
            {
                return _RodinnyStavKod;
            }
            set
            {
                if (value != _RodinnyStavKod)
                {
                    _RodinnyStavKod = value;
                    OnPropertyChanged("RodinnyStavKod");
                }
            }
        }

        private string _NarodnostKod;
        [DataMember]
        public virtual string NarodnostKod
        {
            get
            {
                return _NarodnostKod;
            }
            set
            {
                if (value != _NarodnostKod)
                {
                    _NarodnostKod = value;
                    OnPropertyChanged("NarodnostKod");
                }
            }
        }
        private int? _TypOsobyKod;
        [DataMember]
        public virtual int? TypOsobyKod
        {
            get
            {
                return _TypOsobyKod;
            }
            set
            {
                if (value != _TypOsobyKod)
                {
                    _TypOsobyKod = value;
                    OnPropertyChanged("TypOsobyKod");
                }
            }
        }

        private int? _StupenZverejneniaKod;
        [DataMember]
        public virtual int? StupenZverejneniaKod
        {
            get
            {
                return _StupenZverejneniaKod;
            }
            set
            {
                if (value != _StupenZverejneniaKod)
                {
                    _StupenZverejneniaKod = value;
                    OnPropertyChanged("StupenZverejneniaKod");
                }
            }
        }

        private int _Navratovykod;
        [DataMember]
        public virtual int Navratovykod
        {
            get
            {
                return _Navratovykod;
            }
            set
            {
                if (value != _Navratovykod)
                {
                    _Navratovykod = value;
                    OnPropertyChanged("Navratovykod");
                }
            }
        }

        private string _DovodNeposkytnutiaUdajov;
        [DataMember]
        public virtual string DovodNeposkytnutiaUdajov
        {
            get
            {
                return _DovodNeposkytnutiaUdajov;
            }
            set
            {
                if (value != _DovodNeposkytnutiaUdajov)
                {
                    _DovodNeposkytnutiaUdajov = value;
                    OnPropertyChanged("DovodNeposkytnutiaUdajov");
                }
            }
        }
        
        //RIS WS Získanie EDUID - InternalIdentifiers/InternalPersonID
        private string _InternalPersonID;
        [DataMember]
        public virtual string InternalPersonID
        {
            get
            {
                return _InternalPersonID;
            }
            set
            {
                if (value != _InternalPersonID)
                {
                    _InternalPersonID = value;
                    OnPropertyChanged("InternalPersonID");
                }
            }
        }
        
        /// <summary>
        /// Zmazat osobu 
        /// </summary>
        public virtual bool Vymazat { get; set; }
        
        private int? _OkresNarodeniaKod;
        [DataMember]
        public virtual int? OkresNarodeniaKod
        {
            get
            {
                return _OkresNarodeniaKod;
            }
            set
            {
                if (value != _OkresNarodeniaKod)
                {
                    _OkresNarodeniaKod = value;
                    OnPropertyChanged("OkresNarodenia");
                }
            }
        }
        
        private Guid? _RfoOkresNarodeniaIk;
        [DataMember]
        public virtual Guid? RfoOkresNarodeniaIk
        {
            get
            {
                return _RfoOkresNarodeniaIk;
            }
            set
            {
                if (value != _RfoOkresNarodeniaIk)
                {
                    _RfoOkresNarodeniaIk = value;
                    OnPropertyChanged("RfoOkresNarodenia");
                }
            }
        }
        
        private string _OkresNarodeniaMimoCiselnik;
        [DataMember]
        public virtual string OkresNarodeniaMimoCiselnik
        {
            get
            {
                return _OkresNarodeniaMimoCiselnik;
            }
            set
            {
                if (value != _OkresNarodeniaMimoCiselnik)
                {
                    _OkresNarodeniaMimoCiselnik = value;
                    OnPropertyChanged("OkresNarodeniaMimoCiselnik");
                }
            }
        }
        
        private int? _UzemnyCelokUmrtiaKod;
        [DataMember]
        public virtual int? UzemnyCelokUmrtiaKod
        {
            get
            {
                return _UzemnyCelokUmrtiaKod;
            }
            set
            {
                if (value != _UzemnyCelokUmrtiaKod)
                {
                    _UzemnyCelokUmrtiaKod = value;
                    OnPropertyChanged("UzemnyCelokUmrtia");
                }
            }
        }
        
        private Guid? _RfoUzemnyCelokUmrtiaIk;
        [DataMember]
        public virtual Guid? RfoUzemnyCelokUmrtiaIk
        {
            get
            {
                return _RfoUzemnyCelokUmrtiaIk;
            }
            set
            {
                if (value != _RfoUzemnyCelokUmrtiaIk)
                {
                    _RfoUzemnyCelokUmrtiaIk = value;
                    OnPropertyChanged("RfoUzemnyCelokUmrtia");
                }
            }
        }

        private string _MiestoUmrtiaMimoCiselnik;
        [DataMember]
        public virtual string MiestoUmrtiaMimoCiselnik
        {
            get
            {
                return _MiestoUmrtiaMimoCiselnik;
            }
            set
            {
                if (value != _MiestoUmrtiaMimoCiselnik)
                {
                    _MiestoUmrtiaMimoCiselnik = value;
                    OnPropertyChanged("MiestoUmrtiaMimoCiselnik");
                }
            }
        }

    }

	public partial class FyzickaOsobaFilterCriteria
	{
		private bool? _SparovanieSRFO;
		[DataMember]
		public virtual bool? SparovanieSRFO
		{
			get
			{
				return _SparovanieSRFO;
			}
			set
			{
				if (value != _SparovanieSRFO)
				{
					_SparovanieSRFO = value;
					OnPropertyChanged("SparovanieSRFO");
				}
			}
		}

        private Guid? _RfoOkresNarodeniaIk;
        [DataMember]
        public virtual Guid? RfoOkresNarodeniaIk
        {
            get
            {
                return _RfoOkresNarodeniaIk;
            }
            set
            {
                if (value != _RfoOkresNarodeniaIk)
                {
                    _RfoOkresNarodeniaIk = value;
                    OnPropertyChanged("RfoOkresNarodenia");
                }
            }
        }

        private string _OkresNarodeniaMimoCiselnik;
        [DataMember]
        public virtual string OkresNarodeniaMimoCiselnik
        {
            get
            {
                return _OkresNarodeniaMimoCiselnik;
            }
            set
            {
                if (value != _OkresNarodeniaMimoCiselnik)
                {
                    _OkresNarodeniaMimoCiselnik = value;
                    OnPropertyChanged("OkresNarodeniaMimoCiselnik");
                }
            }
        }

        private Guid? _RfoUzemnyCelokUmrtiaIk;
        [DataMember]
        public virtual Guid? RfoUzemnyCelokUmrtiaIk
        {
            get
            {
                return _RfoUzemnyCelokUmrtiaIk;
            }
            set
            {
                if (value != _RfoUzemnyCelokUmrtiaIk)
                {
                    _RfoUzemnyCelokUmrtiaIk = value;
                    OnPropertyChanged("RfoUzemnyCelokUmrtia");
                }
            }
        }

        private string _MiestoUmrtiaMimoCiselnik;
        [DataMember]
        public virtual string MiestoUmrtiaMimoCiselnik
        {
            get
            {
                return _MiestoUmrtiaMimoCiselnik;
            }
            set
            {
                if (value != _MiestoUmrtiaMimoCiselnik)
                {
                    _MiestoUmrtiaMimoCiselnik = value;
                    OnPropertyChanged("MiestoUmrtiaMimoCiselnik");
                }
            }
        }
	}

    public partial class FyzickaOsobaTitul
    {
        
        private string _TitulKod;
        [DataMember]
        public virtual string TitulKod
        {
            get
            {
                return _TitulKod;
            }
            set
            {
                if (value != _TitulKod)
                {
                    _TitulKod = value;
                    OnPropertyChanged("TitulKod");
                }
            }
        }
        
        private int _TypTituluKod;
        [DataMember]
        public virtual int TypTituluKod
        {
            get
            {
                return _TypTituluKod;
            }
            set
            {
                if (value != _TypTituluKod)
                {
                    _TypTituluKod = value;
                    OnPropertyChanged("TypTituluKod");
                }
            }
        }
        
        private string _Pozicia;
        [DataMember]
        public virtual string Pozicia
        {
            get
            {
                return _Pozicia;
            }
            set
            {
                if (value != _Pozicia)
                {
                    _Pozicia = value;
                    OnPropertyChanged("Pozicia");
                }
            }
        }
        
        private string _TitulNazovSK;
        [DataMember]
        public virtual string TitulNazovSK
        {
            get
            {
                return _TitulNazovSK;
            }
            set
            {
                if (value != _TitulNazovSK)
                {
                    _TitulNazovSK = value;
                    OnPropertyChanged("TitulNazovSK");
                }
            }
        }

    }

    public partial class FyzickaOsobaStatnaPrislusnost
    {
        
        private string _StatnaPrislusnostKod;
        [DataMember]
        public virtual string StatnaPrislusnostKod
        {
            get
            {
                return _StatnaPrislusnostKod;
            }
            set
            {
                if (value != _StatnaPrislusnostKod)
                {
                    _StatnaPrislusnostKod = value;
                    OnPropertyChanged("StatnaPrislusnostKod");
                }
            }
        }

    }

    public partial class VztahovaFyzOsoba
    {
        
        private int _TypVztahuKod;
        [DataMember]
        public virtual int TypVztahuKod
        {
            get
            {
                return _TypVztahuKod;
            }
            set
            {
                if (value != _TypVztahuKod)
                {
                    _TypVztahuKod = value;
                    OnPropertyChanged("TypVztahuKod");
                }
            }
        }
        
        private DateTime? _DatumVznikuVztahu;
        [DataMember]
        public virtual DateTime? DatumVznikuVztahu
        {
            get
            {
                return _DatumVznikuVztahu;
            }
            set
            {
                if (value != _DatumVznikuVztahu)
                {
                    _DatumVznikuVztahu = value;
                    OnPropertyChanged("DatumVznikuVztahu");
                }
            }
        }
        
        private int? _RolaOsobyVRodinnomVztahuKod;
        [DataMember]
        public virtual int? RolaOsobyVRodinnomVztahuKod
        {
            get
            {
                return _RolaOsobyVRodinnomVztahuKod;
            }
            set
            {
                if (value != _RolaOsobyVRodinnomVztahuKod)
                {
                    _RolaOsobyVRodinnomVztahuKod = value;
                    OnPropertyChanged("RolaOsobyVRodinnomVztahuKod");
                }
            }
        }
        
        private int? _RolaVztahovejOsobyVRodinnomVztahuKod;
        [DataMember]
        public virtual int? RolaVztahovejOsobyVRodinnomVztahuKod
        {
            get
            {
                return _RolaVztahovejOsobyVRodinnomVztahuKod;
            }
            set
            {
                if (value != _RolaVztahovejOsobyVRodinnomVztahuKod)
                {
                    _RolaVztahovejOsobyVRodinnomVztahuKod = value;
                    OnPropertyChanged("RolaVztahovejOsobyVRodinnomVztahuKod");
                }
            }
        }

    }

    public partial class PravnaSposobilostObmedzenie
    {
        
        private int _TypObmedzeniaKod;
        [DataMember]
        public virtual int TypObmedzeniaKod
        {
            get
            {
                return _TypObmedzeniaKod;
            }
            set
            {
                if (value != _TypObmedzeniaKod)
                {
                    _TypObmedzeniaKod = value;
                    OnPropertyChanged("TypObmedzeniaKod");
                }
            }
        }

    }

    public partial class ZakazPobytu
    {
        
        private int _UzemnyCelokKod;
        [DataMember]
        public virtual int UzemnyCelokKod
        {
            get
            {
                return _UzemnyCelokKod;
            }
            set
            {
                if (value != _UzemnyCelokKod)
                {
                    _UzemnyCelokKod = value;
                    OnPropertyChanged("UzemnyCelokKod");
                }
            }
        }

    }

    public partial class UdajePobytu
    {
        private int _TypPobytuKod;
        [DataMember]
        public virtual int TypPobytuKod
        {
            get
            {
                return _TypPobytuKod;
            }
            set
            {
                if (value != _TypPobytuKod)
                {
                    _TypPobytuKod = value;
                    OnPropertyChanged("TypPobytuKod");
                }
            }
        }
        
        private string _StatKod;
        [DataMember]
        public virtual string StatKod
        {
            get
            {
                return _StatKod;
            }
            set
            {
                if (value != _StatKod)
                {
                    _StatKod = value;
                    OnPropertyChanged("StatKod");
                }
            }
        }
        
        private DateTime? _DatumPrihlaseniaNaPobyt;
        [DataMember]
        public virtual DateTime? DatumPrihlaseniaNaPobyt
        {
            get
            {
                return _DatumPrihlaseniaNaPobyt;
            }
            set
            {
                if (value != _DatumPrihlaseniaNaPobyt)
                {
                    _DatumPrihlaseniaNaPobyt = value;
                    OnPropertyChanged("DatumPrihlaseniaNaPobyt");
                }
            }
        }
        
        private DateTime? _DatumUkonceniaPobytu;
        [DataMember]
        public virtual DateTime? DatumUkonceniaPobytu
        {
            get
            {
                return _DatumUkonceniaPobytu;
            }
            set
            {
                if (value != _DatumUkonceniaPobytu)
                {
                    _DatumUkonceniaPobytu = value;
                    OnPropertyChanged("DatumUkonceniaPobytu");
                }
            }
        }
        
        private string _OkresKod;
        [DataMember]
        public virtual string OkresKod
        {
            get
            {
                return _OkresKod;
            }
            set
            {
                if (value != _OkresKod)
                {
                    _OkresKod = value;
                    OnPropertyChanged("OkresKod");
                }
            }
        }
        
        private string _ObecKod;
        [DataMember]
        public virtual string ObecKod
        {
            get
            {
                return _ObecKod;
            }
            set
            {
                if (value != _ObecKod)
                {
                    _ObecKod = value;
                    OnPropertyChanged("ObecKod");
                }
            }
        }
 
        private List<RegionMimoSr> _RegionMimoSrList;
        [DataMember]
        public virtual List<RegionMimoSr> RegionMimoSrList
        {
            get
            {
                if (_RegionMimoSrList == null)
                    _RegionMimoSrList = new List<RegionMimoSr>();

                return _RegionMimoSrList;
            }
            set
            {
                if (value != _RegionMimoSrList)
                {
                    _RegionMimoSrList = value;
                    OnPropertyChanged("RegionMimoSrList");
                }
            }
        }

		private string _RegionMimoSrListString;
		[DataMember]
		public virtual string RegionMimoSrListString
		{
			get
			{
				return _RegionMimoSrListString;
			}
			set
			{
				if (value != _RegionMimoSrListString)
				{
					_RegionMimoSrListString = value;
					OnPropertyChanged("RegionMimoSrListString");
				}
			}
		}

		private String _UlicaZobrazena;
		[DataMember]
		public virtual String UlicaZobrazena
		{
			get
			{
				return _UlicaZobrazena;
			}
			set
			{
				if (value != _UlicaZobrazena)
				{
					_UlicaZobrazena = value;
					OnPropertyChanged("UlicaZobrazena");
				}
			}
		} 
    }

    public partial class UdajePobytuFilterCriteria
    {
        private DateTime? _DatumPrihlaseniaNaPobyt;
        [DataMember]
        public virtual DateTime? DatumPrihlaseniaNaPobyt
        {
            get
            {
                return _DatumPrihlaseniaNaPobyt;
            }
            set
            {
                if (value != _DatumPrihlaseniaNaPobyt)
                {
                    _DatumPrihlaseniaNaPobyt = value;
                    OnPropertyChanged("DatumPrihlaseniaNaPobyt");
                }
            }
        }

        private DateTime? _DatumUkonceniaPobytu;
        [DataMember]
        public virtual DateTime? DatumUkonceniaPobytu
        {
            get
            {
                return _DatumUkonceniaPobytu;
            }
            set
            {
                if (value != _DatumUkonceniaPobytu)
                {
                    _DatumUkonceniaPobytu = value;
                    OnPropertyChanged("DatumUkonceniaPobytu");
                }
            }
        }
    }

	[DataContract()]
	public partial class SparovanieSRFO : BaseDataMember
	{
		private bool? _Hodnota;
		[DataMember]
		public virtual bool? Hodnota
		{
			get
			{
				return _Hodnota;
			}
			set
			{
				if (value != _Hodnota)
				{
					_Hodnota = value;
				}
			}
		}

		private string _Nazov;
		[DataMember]
		public virtual string Nazov
		{
			get
			{
				return _Nazov;
			}
			set
			{
				if (value != _Nazov)
				{
					_Nazov = value;
				}
			}
		}

		//#region INotifyPropertyChanged implements

		//[field: NonSerialized]
		//public virtual event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		///// <summary>
		///// Vola sa ked sa zmeni hodnota vlastnosti
		///// </summary>
		///// <param name="propertyName">Nazov vlastnosti.</param>
		//protected virtual void OnPropertyChanged(string propertyName)
		//{
		//	if (PropertyChanged != null)
		//	{
		//		PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
		//	}
		//}

		//#endregion INotifyPropertyChanged implements
	}

    public static class MenoExtension
    {
        public static List<Meno> ListToDelete(this List<Meno> originalList, List<Meno> newList)
        {
            //ak nemam s co zmazat
            if (originalList == null)
                return new List<Meno>();

            //ak po novom nema nic byt, tak zmazem vsetko
            if (newList == null || newList.Count == 0)
                return originalList;

            //vymazem zoznam tych, ktore su v DB, ale v novom zozname nie su
            return originalList.FindAll(item => !newList.Exists(newItem => newItem.Hodnota.Equals(item.Hodnota)));
        }

        public static List<Meno> ListToCreate(this List<Meno> originalList, List<Meno> newList)
        {
            //ak nemam co vytvorit
            if (newList == null)
                return new List<Meno>();

            //ak som nic nemal, musim vytvorit cely zoznam
            if (originalList == null || originalList.Count == 0)
                return newList;

            //vytvorim zoznam tych, ktore nie su v DB, ale v novom zozname su
            return newList.FindAll(item => !originalList.Exists(newItem => newItem.Hodnota.Equals(item.Hodnota)));
        }

        public static List<Meno> ListToUpdate(this List<Meno> originalList, List<Meno> newList)
        {
            //ak nemam s cim porovnat
            if (originalList == null || newList == null || originalList.Count == 0 || newList.Count == 0)
                return new List<Meno>();

            //vytvorim zoznam tych, ktorych hodnoty su v oboch (do noveho najskor zaznacim IDcka), ale je zmena v poradi
            newList.ForEach(item => item.ID = originalList.Find(newItem => newItem.Hodnota.Equals(item.Hodnota)) == null ? null : originalList.Find(newItem => newItem.Hodnota.Equals(item.Hodnota)).ID);
            return newList.FindAll(item => originalList.Exists(newItem => newItem.Hodnota.Equals(item.Hodnota) && !newItem.Poradie.Equals(item.Poradie)));
        }

        public static bool IsAnyDifference(this List<Meno> originalList, List<Meno> newList)
        {
            if (originalList.ListToCreate(newList).Count > 0)
                return true;
            else if (originalList.ListToDelete(newList).Count > 0)
                return true;
            else if (originalList.ListToUpdate(newList).Count > 0)
                return true;

            return false;
        }
    }

    public static class PriezviskoExtension
    {
        public static List<Priezvisko> ListToDelete(this List<Priezvisko> originalList, List<Priezvisko> newList)
        {
            //ak nemam s co zmazat
            if (originalList == null)
                return new List<Priezvisko>();

            //ak po novom nema nic byt, tak zmazem vsetko
            if (newList == null || newList.Count == 0)
                return originalList;

            //vymazem zoznam tych, ktore su v DB, ale v novom zozname nie su
            return originalList.FindAll(item => !newList.Exists(newItem => newItem.Hodnota.Equals(item.Hodnota)));
        }

        public static List<Priezvisko> ListToCreate(this List<Priezvisko> originalList, List<Priezvisko> newList)
        {
            //ak nemam co vytvorit
            if (newList == null)
                return new List<Priezvisko>();

            //ak som nic nemal, musim vytvorit cely zoznam
            if (originalList == null || originalList.Count == 0)
                return newList;

            //vytvorim zoznam tych, ktore nie su v DB, ale v novom zozname su
            return newList.FindAll(item => !originalList.Exists(newItem => newItem.Hodnota.Equals(item.Hodnota)));
        }

        public static List<Priezvisko> ListToUpdate(this List<Priezvisko> originalList, List<Priezvisko> newList)
        {
            //ak nemam s cim porovnat
            if (originalList == null || newList == null || originalList.Count == 0 || newList.Count == 0)
                return new List<Priezvisko>();

            //vytvorim zoznam tych, ktore su v oboch (do noveho najskor zaznacim IDcka)
            newList.ForEach(item => item.ID = originalList.Find(newItem => newItem.Hodnota.Equals(item.Hodnota)) == null ? null : originalList.Find(newItem => newItem.Hodnota.Equals(item.Hodnota)).ID);
            return newList.FindAll(item => originalList.Exists(newItem => newItem.Hodnota.Equals(item.Hodnota) && !newItem.Poradie.Equals(item.Poradie)));
        }

        public static bool IsAnyDifference(this List<Priezvisko> originalList, List<Priezvisko> newList)
        {
            if (originalList.ListToCreate(newList).Count > 0)
                return true;
            else if (originalList.ListToDelete(newList).Count > 0)
                return true;
            else if (originalList.ListToUpdate(newList).Count > 0)
                return true;

            return false;
        }

        public static List<RodnePriezvisko> ToRodnePriezviskoList(this List<Priezvisko> list)
        {
            var retVal = new List<RodnePriezvisko>();
            if(list != null)
                foreach(var item in list)
                {
                    retVal.Add(new RodnePriezvisko()
                    {
                        FyzickaOsobaId = item.FyzickaOsobaId,
                        Hodnota = item.Hodnota,
                        Changed = item.Changed,
                        ID = item.ID,
                        Identifikator = item.Identifikator,
                        Poradie = item.Poradie,
                        TransakciaId = item.TransakciaId,
                        TransakciaZruseneId = item.TransakciaZruseneId
                    });
                }

            return retVal;
        }
    }

    public static class RodnePriezviskoExtension
    {
        public static List<RodnePriezvisko> ListToDelete(this List<RodnePriezvisko> originalList, List<RodnePriezvisko> newList)
        {
            //ak nemam s co zmazat
            if (originalList == null)
                return new List<RodnePriezvisko>();

            //ak po novom nema nic byt, tak zmazem vsetko
            if (newList == null || newList.Count == 0)
                return originalList;

            //vymazem zoznam tych, ktore su v DB, ale v novom zozname nie su
            return originalList.FindAll(item => !newList.Exists(newItem => newItem.Hodnota.Equals(item.Hodnota)));
        }

        public static List<RodnePriezvisko> ListToCreate(this List<RodnePriezvisko> originalList, List<RodnePriezvisko> newList)
        {
            //ak nemam co vytvorit
            if (newList == null)
                return new List<RodnePriezvisko>();

            //ak som nic nemal, musim vytvorit cely zoznam
            if (originalList == null || originalList.Count == 0)
                return newList;

            //vytvorim zoznam tych, ktore nie su v DB, ale v novom zozname su
            return newList.FindAll(item => !originalList.Exists(newItem => newItem.Hodnota.Equals(item.Hodnota)));
        }

        public static List<RodnePriezvisko> ListToUpdate(this List<RodnePriezvisko> originalList, List<RodnePriezvisko> newList)
        {
            //ak nemam s cim porovnat
            if (originalList == null || newList == null || originalList.Count == 0 || newList.Count == 0)
                return new List<RodnePriezvisko>();

            //vytvorim zoznam tych, ktore su v oboch (do noveho najskor zaznacim IDcka)
            newList.ForEach(item => item.ID = originalList.Find(newItem => newItem.Hodnota.Equals(item.Hodnota)) == null ? null : originalList.Find(newItem => newItem.Hodnota.Equals(item.Hodnota)).ID);
            return newList.FindAll(item => originalList.Exists(newItem => newItem.Hodnota.Equals(item.Hodnota) && !newItem.Poradie.Equals(item.Poradie)));
        }
    }

    public static class FyzickaOsobaTitulExtension
    {
        public static List<FyzickaOsobaTitul> ListToDelete(this List<FyzickaOsobaTitul> originalList, List<FyzickaOsobaTitul> newList)
        {
            //ak nemam s co zmazat
            if (originalList == null)
                return new List<FyzickaOsobaTitul>();

            //ak po novom nema nic byt, tak zmazem vsetko
            if (newList == null || newList.Count == 0)
                return originalList;

            //vymazem zoznam tych, ktore su v DB, ale v novom zozname nie su
            return originalList.FindAll(item => !newList.Exists(newItem => newItem.RfoTitulIk.Equals(item.RfoTitulIk)));
        }

        public static List<FyzickaOsobaTitul> ListToCreate(this List<FyzickaOsobaTitul> originalList, List<FyzickaOsobaTitul> newList)
        {
            //ak nemam co vytvorit
            if (newList == null)
                return new List<FyzickaOsobaTitul>();

            //ak som nic nemal, musim vytvorit cely zoznam
            if (originalList == null || originalList.Count == 0)
                return newList;

            //vytvorim zoznam tych, ktore nie su v DB, ale v novom zozname su
            return newList.FindAll(item => !originalList.Exists(newItem => newItem.RfoTitulIk.Equals(item.RfoTitulIk)));
        }

        public static List<FyzickaOsobaTitul> ListToUpdate(this List<FyzickaOsobaTitul> originalList, List<FyzickaOsobaTitul> newList)
        {
            //ak nemam s cim porovnat
            if (originalList == null || newList == null || originalList.Count == 0 || newList.Count == 0)
                return new List<FyzickaOsobaTitul>();

            //vytvorim zoznam tych, ktore su v oboch (do noveho najskor zaznacim IDcka)
            newList.ForEach(item => item.ID = originalList.Find(newItem => newItem.RfoTitulIk.Equals(item.RfoTitulIk)) == null ? null : originalList.Find(newItem => newItem.RfoTitulIk.Equals(item.RfoTitulIk)).ID);
            return newList.FindAll(item => originalList.Exists(newItem => newItem.RfoTitulIk.Equals(item.RfoTitulIk)));
        }
    }

    public static class FyzickaOsobaStatnaPrislusnostExtension
    {
        public static List<FyzickaOsobaStatnaPrislusnost> ListToDelete(this List<FyzickaOsobaStatnaPrislusnost> originalList, List<FyzickaOsobaStatnaPrislusnost> newList)
        {
            //ak nemam s co zmazat
            if (originalList == null)
                return new List<FyzickaOsobaStatnaPrislusnost>();

            //ak po novom nema nic byt, tak zmazem vsetko
            if (newList == null || newList.Count == 0)
                return originalList;

            //vymazem zoznam tych, ktore su v DB, ale v novom zozname nie su
            return originalList.FindAll(item => !newList.Exists(newItem => newItem.RfoStatIk.Equals(item.RfoStatIk)));
        }

        public static List<FyzickaOsobaStatnaPrislusnost> ListToCreate(this List<FyzickaOsobaStatnaPrislusnost> originalList, List<FyzickaOsobaStatnaPrislusnost> newList)
        {
            //ak nemam co vytvorit
            if (newList == null)
                return new List<FyzickaOsobaStatnaPrislusnost>();

            //ak som nic nemal, musim vytvorit cely zoznam
            if (originalList == null || originalList.Count == 0)
                return newList;

            //vytvorim zoznam tych, ktore nie su v DB, ale v novom zozname su
            return newList.FindAll(item => !originalList.Exists(newItem => newItem.RfoStatIk.Equals(item.RfoStatIk)));
        }

        public static List<FyzickaOsobaStatnaPrislusnost> ListToUpdate(this List<FyzickaOsobaStatnaPrislusnost> originalList, List<FyzickaOsobaStatnaPrislusnost> newList)
        {
            //ak nemam s cim porovnat
            if (originalList == null || newList == null || originalList.Count == 0 || newList.Count == 0)
                return new List<FyzickaOsobaStatnaPrislusnost>();

            //vytvorim zoznam tych, ktore su v oboch (do noveho najskor zaznacim IDcka)
            newList.ForEach(item => item.ID = originalList.Find(newItem => newItem.RfoStatIk.Equals(item.RfoStatIk)) == null ? null : originalList.Find(newItem => newItem.RfoStatIk.Equals(item.RfoStatIk)).ID);
            return newList.FindAll(item => originalList.Exists(newItem => newItem.RfoStatIk.Equals(item.RfoStatIk)));
        }
    }

    public static class PravnaSposobilostObmedzenieExtension
    {
        public static List<PravnaSposobilostObmedzenie> ListToDelete(this List<PravnaSposobilostObmedzenie> originalList, List<PravnaSposobilostObmedzenie> newList)
        {
            //ak nemam s co zmazat
            if (originalList == null)
                return new List<PravnaSposobilostObmedzenie>();

            //ak po novom nema nic byt, tak zmazem vsetko
            if (newList == null || newList.Count == 0)
                return originalList;

            //vymazem zoznam tych, ktore su v DB, ale v novom zozname nie su
            return originalList.FindAll(item => !newList.Exists(newItem => newItem.RfoSposobilostPravUkonIk.Equals(item.RfoSposobilostPravUkonIk)));
        }

        public static List<PravnaSposobilostObmedzenie> ListToCreate(this List<PravnaSposobilostObmedzenie> originalList, List<PravnaSposobilostObmedzenie> newList)
        {
            //ak nemam co vytvorit
            if (newList == null)
                return new List<PravnaSposobilostObmedzenie>();

            //ak som nic nemal, musim vytvorit cely zoznam
            if (originalList == null || originalList.Count == 0)
                return newList;

            //vytvorim zoznam tych, ktore nie su v DB, ale v novom zozname su
            return newList.FindAll(item => !originalList.Exists(newItem => newItem.RfoSposobilostPravUkonIk.Equals(item.RfoSposobilostPravUkonIk)));
        }

        public static List<PravnaSposobilostObmedzenie> ListToUpdate(this List<PravnaSposobilostObmedzenie> originalList, List<PravnaSposobilostObmedzenie> newList)
        {
            //ak nemam s cim porovnat
            if (originalList == null || newList == null || originalList.Count == 0 || newList.Count == 0)
                return new List<PravnaSposobilostObmedzenie>();

            //vytvorim zoznam tych, ktore su v oboch (do noveho najskor zaznacim IDcka)
            newList.ForEach(item => item.ID = originalList.Find(newItem => newItem.RfoSposobilostPravUkonIk.Equals(item.RfoSposobilostPravUkonIk)) == null ? null : originalList.Find(newItem => newItem.RfoSposobilostPravUkonIk.Equals(item.RfoSposobilostPravUkonIk)).ID);
            return newList.FindAll(item => originalList.Exists(newItem => newItem.RfoSposobilostPravUkonIk.Equals(item.RfoSposobilostPravUkonIk)));
        }
    }

    public static class ZakazPobytuExtension
    {
        public static List<ZakazPobytu> ListToDelete(this List<ZakazPobytu> originalList, List<ZakazPobytu> newList)
        {
            //ak nemam s co zmazat
            if (originalList == null)
                return new List<ZakazPobytu>();

            //ak po novom nema nic byt, tak zmazem vsetko
            if (newList == null || newList.Count == 0)
                return originalList;

            //vymazem zoznam tych, ktore su v DB, ale v novom zozname nie su
            return originalList.FindAll(item => !newList.Exists(newItem => newItem.RfoObecId.Equals(item.RfoObecId)));
        }

        public static List<ZakazPobytu> ListToCreate(this List<ZakazPobytu> originalList, List<ZakazPobytu> newList)
        {
            //ak nemam co vytvorit
            if (newList == null)
                return new List<ZakazPobytu>();

            //ak som nic nemal, musim vytvorit cely zoznam
            if (originalList == null || originalList.Count == 0)
                return newList;

            //vytvorim zoznam tych, ktore nie su v DB, ale v novom zozname su
            return newList.FindAll(item => !originalList.Exists(newItem => newItem.RfoObecId.Equals(item.RfoObecId)));
        }

        public static List<ZakazPobytu> ListToUpdate(this List<ZakazPobytu> originalList, List<ZakazPobytu> newList)
        {
            //ak nemam s cim porovnat
            if (originalList == null || newList == null || originalList.Count == 0 || newList.Count == 0)
                return new List<ZakazPobytu>();

            //vytvorim zoznam tych, ktore su v oboch (do noveho najskor zaznacim IDcka)
            newList.ForEach(item => item.ID = originalList.Find(newItem => newItem.RfoObecId.Equals(item.RfoObecId)) == null ? null : originalList.Find(newItem => newItem.RfoObecId.Equals(item.RfoObecId)).ID);
            return newList.FindAll(item => originalList.Exists(newItem => newItem.RfoObecId.Equals(item.RfoObecId)));
        }
    }

    public static class UdajePobytuExtension
    {
        public static List<UdajePobytu> ListToDelete(this List<UdajePobytu> originalList, List<UdajePobytu> newList)
        {
            //ak nemam s cim porovnat
            if (originalList == null)
                return new List<UdajePobytu>();

            //return originalList;

            //vymazem zoznam tych, ktore su v DB, ale v novom zozname nie su
            return originalList.FindAll(item => !newList.Exists(newItem => newItem.Identifikator == item.Identifikator));
        }

        public static List<UdajePobytu> ListToCreate(this List<UdajePobytu> originalList, List<UdajePobytu> newList)
        {
            //ak nemam s cim porovnat
            if (newList == null)
                return new List<UdajePobytu>();

            //return newList;

            //vratim zoznam tych, ktore nie su v povodnom zozname, teda su nove
            return newList.FindAll(item => !originalList.Exists(newItem => newItem.Identifikator == item.Identifikator));
        }

        /// <summary>
        /// nie je  to zahraničná osoba bez trvalého pobytu na územi SR (nie je to už zapísaný cudzinec bez pobytu na území SR),
        /// Country/Codelist/CodelistItem/ItemCode = 703 (Slovenská republika)
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool HasTrvalyPobytSR(this List<UdajePobytu> list)
        {
            return list == null || list.Exists(pobyt => pobyt.StatKod.Equals("703"));
        }
    }

    public static class OsobaExtension
    {
        /// <summary>
        /// nie je  to zahraničná osoba bez trvalého pobytu na územi SR (nie je to už zapísaný cudzinec bez pobytu na území SR),
        /// Country/Codelist/CodelistItem/ItemCode = 703 (Slovenská republika)
        /// priznak nastaveny docasne na true, pokym sme v rezime, ze nezapisujeme do RFO
        /// </summary>
        /// <param name="osoba"></param>
        /// <returns></returns>
        public static bool HasTrvalyPobytSR(this Osoba osoba, bool zapisDoRFO = false, bool zapnuteRFO = false)
        {
            return zapisDoRFO && zapnuteRFO ? osoba.UdajePobytuList == null || osoba.UdajePobytuList.Count == 0 || osoba.UdajePobytuList.Exists(pobyt => pobyt.StatKod.Equals("703")) : true;
        }
    }

    public static class VztahovaFyzOsobaExtension
    {
        public static List<VztahovaFyzOsoba> ListToDelete(this List<VztahovaFyzOsoba> originalList, List<VztahovaFyzOsoba> newList)
        {
            //ak nemam s co zmazat
            if (originalList == null)
                return new List<VztahovaFyzOsoba>();

            //ak po novom nema nic byt, tak zmazem vsetko
            if (newList == null || newList.Count == 0)
                return originalList;

            //vymazem zoznam tych, ktore su v DB, ale v novom zozname nie su
            return originalList.FindAll(item => !newList.Exists(newItem => newItem.IfoVztahovejOsoby.Equals(item.IfoVztahovejOsoby)
                                                                        && newItem.RfoTypRodinnehoVztahuIk.Equals(item.RfoTypRodinnehoVztahuIk)
                                                                        && newItem.RfoTypRoleVRodiVztahuIk.Equals(item.RfoTypRoleVRodiVztahuIk)
                                                                        && newItem.RolaOsobyVRodinnomVztahuKod.Equals(item.RolaOsobyVRodinnomVztahuKod)
                                                                        && newItem.RolaVztahovejOsobyVRodinnomVztahuKod.Equals(item.RolaVztahovejOsobyVRodinnomVztahuKod)));
        }

        public static List<VztahovaFyzOsoba> ListToCreate(this List<VztahovaFyzOsoba> originalList, List<VztahovaFyzOsoba> newList)
        {
            //ak nemam co vytvorit
            if (newList == null)
                return new List<VztahovaFyzOsoba>();

            //ak som nic nemal, musim vytvorit cely zoznam
            if (originalList == null || originalList.Count == 0)
                return newList;

            //vytvorim zoznam tych, ktore nie su v DB, ale v novom zozname su
            return newList.FindAll(item => !originalList.Exists(newItem => newItem.IfoVztahovejOsoby.Equals(item.IfoVztahovejOsoby)
                                                                        && newItem.RfoTypRodinnehoVztahuIk.Equals(item.RfoTypRodinnehoVztahuIk)
                                                                        && newItem.RfoTypRoleVRodiVztahuIk.Equals(item.RfoTypRoleVRodiVztahuIk)
                                                                        && newItem.RolaOsobyVRodinnomVztahuKod.Equals(item.RolaOsobyVRodinnomVztahuKod)
                                                                        && newItem.RolaVztahovejOsobyVRodinnomVztahuKod.Equals(item.RolaVztahovejOsobyVRodinnomVztahuKod)));
        }

        public static List<VztahovaFyzOsoba> ListToUpdate(this List<VztahovaFyzOsoba> originalList, List<VztahovaFyzOsoba> newList)
        {
            //ak nemam s cim porovnat
            if (originalList == null || newList == null || originalList.Count == 0 || newList.Count == 0)
                return new List<VztahovaFyzOsoba>();

            //vytvorim zoznam tych, ktore su v oboch (do noveho najskor zaznacim IDcka)
            newList.ForEach(item => item.ID = originalList.Find(newItem => newItem.IfoVztahovejOsoby.Equals(item.IfoVztahovejOsoby)
                                                                        && newItem.RfoTypRodinnehoVztahuIk.Equals(item.RfoTypRodinnehoVztahuIk)
                                                                        && newItem.RfoTypRoleVRodiVztahuIk.Equals(item.RfoTypRoleVRodiVztahuIk)
                                                                        && newItem.RolaOsobyVRodinnomVztahuKod.Equals(item.RolaOsobyVRodinnomVztahuKod)
                                                                        && newItem.RolaVztahovejOsobyVRodinnomVztahuKod.Equals(item.RolaVztahovejOsobyVRodinnomVztahuKod)) == null ? null : originalList.Find(newItem => newItem.IfoVztahovejOsoby.Equals(item.IfoVztahovejOsoby)
                                                                                                                                                                                                                    && newItem.RfoTypRodinnehoVztahuIk.Equals(item.RfoTypRodinnehoVztahuIk)
                                                                                                                                                                                                                    && newItem.RfoTypRoleVRodiVztahuIk.Equals(item.RfoTypRoleVRodiVztahuIk)
                                                                                                                                                                                                                    && newItem.RolaOsobyVRodinnomVztahuKod.Equals(item.RolaOsobyVRodinnomVztahuKod)
                                                                                                                                                                                                                    && newItem.RolaVztahovejOsobyVRodinnomVztahuKod.Equals(item.RolaVztahovejOsobyVRodinnomVztahuKod)).ID);
            return newList.FindAll(item => originalList.Exists(newItem => newItem.IfoVztahovejOsoby.Equals(item.IfoVztahovejOsoby)
                                                                        && newItem.RfoTypRodinnehoVztahuIk.Equals(item.RfoTypRodinnehoVztahuIk)
                                                                        && newItem.RfoTypRoleVRodiVztahuIk.Equals(item.RfoTypRoleVRodiVztahuIk)
                                                                        && newItem.RolaOsobyVRodinnomVztahuKod.Equals(item.RolaOsobyVRodinnomVztahuKod)
                                                                        && newItem.RolaVztahovejOsobyVRodinnomVztahuKod.Equals(item.RolaVztahovejOsobyVRodinnomVztahuKod)));
        }

        public static List<VztahovaFyzOsoba> ListToDeleteByIdentifikator(this List<VztahovaFyzOsoba> originalList, List<VztahovaFyzOsoba> newList)
        {
            //ak nemam s co zmazat
            if (originalList == null)
                return new List<VztahovaFyzOsoba>();

            //ak po novom nema nic byt, tak zmazem vsetko
            if (newList == null || newList.Count == 0)
                return originalList;

            //vymazem zoznam tych, ktore su v DB, ale v novom zozname nie su
            return originalList.FindAll(item => !newList.Exists(newItem => newItem.Identifikator.Equals(item.Identifikator)));
        }

        public static List<VztahovaFyzOsoba> ListToCreateByIdentifikator(this List<VztahovaFyzOsoba> originalList, List<VztahovaFyzOsoba> newList)
        {
            //ak nemam co vytvorit
            if (newList == null)
                return new List<VztahovaFyzOsoba>();

            //ak som nic nemal, musim vytvorit cely zoznam
            if (originalList == null || originalList.Count == 0)
                return newList;

            //vymazem zoznam tych, ktore su v DB, ale v novom zozname nie su
            return newList.FindAll(item => !originalList.Exists(newItem => newItem.Identifikator.Equals(item.Identifikator)));
        }
    }
}

namespace Ditec.RIS.RFO.Dol.PoskytnutieUdajovIFOOnlineWS
{
    public partial class TransEnvTypeOut
    {
        public static implicit operator OsobaResponse(TransEnvTypeOut response)
        {
            //System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(response.GetType());
            //System.IO.TextWriter WriteFileStream = new System.IO.StreamWriter(@"d:\Projects\RIS2\Trunk\Code\RIS2\Test\RIS.Svc.Wcf.Test\response" + DateTime.Now.ToString("HH-mm-ss") + ".xml");
            //serializer.Serialize(WriteFileStream, response);
            //WriteFileStream.Close();

            var retVal = new OsobaResponse();
            retVal.OsobaList = new List<Osoba>();

            try
            {
                foreach (var itemOsoba in response.POV.OEXList)
                {
                    var osoba = new Osoba();
                    osoba.FyzickaOsoba = new FyzickaOsoba();
                    retVal.OsobaList.Add(osoba);

                    osoba.FyzickaOsoba.Ifo = itemOsoba.ID;
                    osoba.FyzickaOsoba.IfoPravejOsoby = itemOsoba.PO;
                    osoba.FyzickaOsoba.RodneCislo = itemOsoba.RC;
                    osoba.FyzickaOsoba.DatumNarodenia = itemOsoba.DN;
                    osoba.FyzickaOsoba.MiestoNarodeniaIne = itemOsoba.MN;
                    osoba.FyzickaOsoba.StatNarodeniaKod = itemOsoba.SN;
                    if(osoba.FyzickaOsoba.StatNarodeniaKod.HasValue)
                        osoba.FyzickaOsoba.StatNarodeniaIk = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoStat>().Exists(item => item.Kod == osoba.FyzickaOsoba.StatNarodeniaKod.ToString()) ? Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoStat>().Find(item => item.Kod == osoba.FyzickaOsoba.StatNarodeniaKod.ToString()).InternyKod.Value : Guid.Empty;

                    osoba.FyzickaOsoba.UzemnyCelokNarodeniaKod = itemOsoba.UC;
                    if(osoba.FyzickaOsoba.UzemnyCelokNarodeniaKod.HasValue)
                        osoba.FyzickaOsoba.ObecNarodeniaIk = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoObec>().Exists(item => item.Kod == osoba.FyzickaOsoba.UzemnyCelokNarodeniaKod.ToString()) ? Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoObec>().Find(item => item.Kod == osoba.FyzickaOsoba.UzemnyCelokNarodeniaKod.ToString()).InternyKod.Value : Guid.Empty;

                    osoba.FyzickaOsoba.PohlavieKod = itemOsoba.PI.HasValue ? itemOsoba.PI.Value.ToString() : "";
                    //mapovanie nasich ciselnikov
                    osoba.FyzickaOsoba.RfoPohlavieIk = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoPohlavie>().Exists(item => item.Kod == osoba.FyzickaOsoba.PohlavieKod.ToString()) ? Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoPohlavie>().Find(item => item.Kod == osoba.FyzickaOsoba.PohlavieKod.ToString()).InternyKod.Value : Guid.Empty;

                    osoba.FyzickaOsoba.RodinnyStavKod = itemOsoba.RS;
                    if (osoba.FyzickaOsoba.RodinnyStavKod.HasValue)
                        //mapovanie nasich ciselnikov
                        osoba.FyzickaOsoba.RfoRodinnyStavIk = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoRodinnyStav>().Exists(item => item.Kod == osoba.FyzickaOsoba.RodinnyStavKod.ToString()) ? Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoRodinnyStav>().Find(item => item.Kod == osoba.FyzickaOsoba.RodinnyStavKod.ToString()).InternyKod.Value : Guid.Empty;

                    osoba.FyzickaOsoba.NarodnostKod = itemOsoba.NI.HasValue ? itemOsoba.NI.Value.ToString() : "";
                    //mapovanie ciselnika
                    if(itemOsoba.NI.HasValue)
                    osoba.FyzickaOsoba.RfoNarodnostIk = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoNarodnost>().Exists(item => item.Kod == osoba.FyzickaOsoba.NarodnostKod) ? Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoNarodnost>().Find(item => item.Kod == osoba.FyzickaOsoba.NarodnostKod).InternyKod : Guid.Empty;

                    osoba.FyzickaOsoba.DatumUmrtia = itemOsoba.DU;
                    osoba.FyzickaOsoba.DatumPravoplatnostiRozh = (itemOsoba.PZMList != null && itemOsoba.PZMList.Length > 0 ? itemOsoba.PZMList[0].DP : null);
                    osoba.FyzickaOsoba.TypOsobyKod = itemOsoba.TV;
                    if (osoba.FyzickaOsoba.TypOsobyKod.HasValue)
                        //mapovanie ciselnika
                        osoba.FyzickaOsoba.RfoTypOsobyIk = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoTypOsoby>().Exists(item => item.Kod == osoba.FyzickaOsoba.TypOsobyKod.ToString()) ? Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoTypOsoby>().Find(item => item.Kod == osoba.FyzickaOsoba.TypOsobyKod.ToString()).InternyKod : Guid.Empty;

                    osoba.FyzickaOsoba.StupenZverejneniaKod = itemOsoba.SZ;
                    if (osoba.FyzickaOsoba.StupenZverejneniaKod.HasValue)
                        //mapovanie ciselnika
                        osoba.FyzickaOsoba.RfoStupenZverejneniaIk = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoStupenZverejnenia>().Exists(item => item.Kod == osoba.FyzickaOsoba.StupenZverejneniaKod.ToString()) ? Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoStupenZverejnenia>().Find(item => item.Kod == osoba.FyzickaOsoba.StupenZverejneniaKod.ToString()).InternyKod : Guid.Empty;

                    osoba.FyzickaOsoba.Navratovykod = itemOsoba.NK;
                    osoba.FyzickaOsoba.DovodNeposkytnutiaUdajov = itemOsoba.DP;
                    //osoba.FyzickaOsoba.IdentifikatorDavky = itemOsoba.ZZ;
                    //Nove zadanie - CR_IRIS_150828_2_FO okres narodenia a miesto umrtia - WS
                    osoba.FyzickaOsoba.OkresNarodeniaKod = itemOsoba.UL;
                    if (osoba.FyzickaOsoba.OkresNarodeniaKod.HasValue)
                        osoba.FyzickaOsoba.RfoOkresNarodeniaIk = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoUzemnyCelok>().Exists(item => item.Kod == osoba.FyzickaOsoba.OkresNarodeniaKod.Value.ToString()) ? Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoUzemnyCelok>().Find(item => item.Kod == osoba.FyzickaOsoba.OkresNarodeniaKod.Value.ToString()).InternyKod.Value : Guid.Empty;

                    osoba.FyzickaOsoba.UzemnyCelokUmrtiaKod = itemOsoba.UE;
                    if (osoba.FyzickaOsoba.UzemnyCelokUmrtiaKod.HasValue)
                        osoba.FyzickaOsoba.RfoUzemnyCelokUmrtiaIk = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoUzemnyCelok>().Exists(item => item.Kod == osoba.FyzickaOsoba.UzemnyCelokUmrtiaKod.Value.ToString()) ? Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoUzemnyCelok>().Find(item => item.Kod == osoba.FyzickaOsoba.UzemnyCelokUmrtiaKod.Value.ToString()).InternyKod.Value : Guid.Empty;

                    osoba.FyzickaOsoba.MiestoUmrtiaMimoCiselnik = itemOsoba.MU;

                    osoba.MenoList = new List<Meno>();
                    if (itemOsoba.MOSList != null && itemOsoba.MOSList.Length > 0)
                    foreach (var itemMeno in itemOsoba.MOSList.OrderBy(x => x.PO))
                    {
                        var meno = new Meno();
                        osoba.MenoList.Add(meno);
                        meno.Hodnota = itemMeno.ME.Trim();
                        meno.Poradie = itemMeno.PO;
                        meno.Identifikator = itemMeno.ID;

                        osoba.FyzickaOsoba.MenoZobrazovane += meno.Hodnota + " ";
                    }
                    osoba.FyzickaOsoba.MenoZobrazovane = String.IsNullOrEmpty(osoba.FyzickaOsoba.MenoZobrazovane) ? null : osoba.FyzickaOsoba.MenoZobrazovane.Trim();

                    osoba.PriezviskoList = new List<Priezvisko>();
                    if (itemOsoba.PRIList != null && itemOsoba.PRIList.Length > 0)
                    foreach (var itemPriezvisko in itemOsoba.PRIList.OrderBy(x => x.PO))
                    {
                        var priezvisko = new Priezvisko();
                        osoba.PriezviskoList.Add(priezvisko);
                        priezvisko.Hodnota = itemPriezvisko.PR.Trim();
                        priezvisko.Poradie = itemPriezvisko.PO;
                        priezvisko.Identifikator = itemPriezvisko.ID;

                        osoba.FyzickaOsoba.PriezviskoZobrazovane += priezvisko.Hodnota + " ";
                    }
                    osoba.FyzickaOsoba.PriezviskoZobrazovane = String.IsNullOrEmpty(osoba.FyzickaOsoba.PriezviskoZobrazovane) ? null : osoba.FyzickaOsoba.PriezviskoZobrazovane.Trim();

                    osoba.RodnePriezviskoList = new List<RodnePriezvisko>();
                    if (itemOsoba.RPRList != null && itemOsoba.RPRList.Length > 0)
                    foreach (var itemRodnePriezvisko in itemOsoba.RPRList)
                    {
                        var rodnePriezvisko = new RodnePriezvisko();
                        osoba.RodnePriezviskoList.Add(rodnePriezvisko);
                        rodnePriezvisko.Hodnota = itemRodnePriezvisko.RP.Trim();
                        rodnePriezvisko.Poradie = itemRodnePriezvisko.PO;
                        rodnePriezvisko.Identifikator = itemRodnePriezvisko.ID;

                        osoba.FyzickaOsoba.RodneMenoZobrazovane += rodnePriezvisko.Hodnota + " ";
                    }
                    osoba.FyzickaOsoba.RodneMenoZobrazovane = String.IsNullOrEmpty(osoba.FyzickaOsoba.RodneMenoZobrazovane) ? null : osoba.FyzickaOsoba.RodneMenoZobrazovane.Trim();

                    osoba.TitulList = new List<FyzickaOsobaTitul>();
                    if (itemOsoba.TOSList != null && itemOsoba.TOSList.Length > 0)
                    foreach (var itemTitul in itemOsoba.TOSList)
                    {
                        var titul = new FyzickaOsobaTitul();
                        osoba.TitulList.Add(titul);
                        titul.TitulKod = itemTitul.TI.ToString();
                        //mapovanie ciselnika
                        titul.RfoTitulIk = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoTitul>().Exists(item => item.Kod == titul.TitulKod.ToString()) ? Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoTitul>().Find(item => item.Kod == titul.TitulKod.ToString()).InternyKod.Value : Guid.Empty;
                        titul.Identifikator = itemTitul.ID;

                        titul.TypTituluKod = itemTitul.TT;
                    }

                    osoba.StatnaPrislusnostList = new List<FyzickaOsobaStatnaPrislusnost>();
                    if (itemOsoba.SPRList != null && itemOsoba.SPRList.Length > 0)
                    foreach (var itemStatnaPrislusnost in itemOsoba.SPRList)
                    {
                        var statnaPrislusnost = new FyzickaOsobaStatnaPrislusnost();
                        osoba.StatnaPrislusnostList.Add(statnaPrislusnost);
                        statnaPrislusnost.StatnaPrislusnostKod = itemStatnaPrislusnost.ST.ToString();
                        //mapovanie ciselnika
                        statnaPrislusnost.RfoStatIk = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoStat>().Exists(item => item.Kod == statnaPrislusnost.StatnaPrislusnostKod.ToString()) ? Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoStat>().Find(item => item.Kod == statnaPrislusnost.StatnaPrislusnostKod.ToString()).InternyKod.Value : Guid.Empty;
                    }

                    osoba.VztahovaOsobaList = new List<VztahovaFyzOsoba>();
                    if (itemOsoba.RVEList != null && itemOsoba.RVEList.Length > 0)
                    foreach (var itemVztahovaOsoba in itemOsoba.RVEList)
                    {
                        var vztahovaOsoba = new VztahovaFyzOsoba();
                        osoba.VztahovaOsobaList.Add(vztahovaOsoba);
                        vztahovaOsoba.IfoVztahovejOsoby = itemVztahovaOsoba.IF;
                        vztahovaOsoba.TypVztahuKod = itemVztahovaOsoba.TR;
                        //mapovanie ciselnika
                        vztahovaOsoba.RfoTypRodinnehoVztahuIk = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoTypRodinnehoVztahu>().Exists(item => item.Kod == vztahovaOsoba.TypVztahuKod.ToString()) ? Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoTypRodinnehoVztahu>().Find(item => item.Kod == vztahovaOsoba.TypVztahuKod.ToString()).InternyKod.Value : Guid.Empty;

                        vztahovaOsoba.DatumVznikuVztahu = itemVztahovaOsoba.DV;
                        vztahovaOsoba.RolaOsobyVRodinnomVztahuKod = itemVztahovaOsoba.TE;
                        //mapovanie ciselnika
                        vztahovaOsoba.RfoTypRoleVRodiVztahuIk = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoTypRoleVRodiVztahu>().Exists(item => item.Kod == vztahovaOsoba.RolaOsobyVRodinnomVztahuKod.ToString()) ? Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoTypRoleVRodiVztahu>().Find(item => item.Kod == vztahovaOsoba.RolaOsobyVRodinnomVztahuKod.ToString()).InternyKod.Value : Guid.Empty;

                        vztahovaOsoba.RolaVztahovejOsobyVRodinnomVztahuKod = itemVztahovaOsoba.TL;
                        vztahovaOsoba.Identifikator = itemVztahovaOsoba.ID;
                    }

                    osoba.PravnaSposobilostObmedzenieList = new List<PravnaSposobilostObmedzenie>();
                    if (itemOsoba.SNRList != null && itemOsoba.SNRList.Length > 0)
                    foreach (var itemPravnaSposobilost in itemOsoba.SNRList)
                    {
                        var pravnaSposobilost = new PravnaSposobilostObmedzenie();
                        osoba.PravnaSposobilostObmedzenieList.Add(pravnaSposobilost);
                        pravnaSposobilost.TypObmedzeniaKod = itemPravnaSposobilost.SN;
                        //mapovanie ciselnika
                        pravnaSposobilost.RfoSposobilostPravUkonIk = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoSposobilostPravUkon>().Exists(item => item.Kod == pravnaSposobilost.TypObmedzeniaKod.ToString()) ? Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoSposobilostPravUkon>().Find(item => item.Kod == pravnaSposobilost.TypObmedzeniaKod.ToString()).InternyKod.Value : Guid.Empty;

                        pravnaSposobilost.PlatnostOd = itemPravnaSposobilost.DZ;
                        pravnaSposobilost.PlatnostDo = itemPravnaSposobilost.DK;
                        pravnaSposobilost.Poznamka = itemPravnaSposobilost.PZ;
                        pravnaSposobilost.Identifikator = itemPravnaSposobilost.ID;
                    }

                    osoba.ZakazPobytuList = new List<ZakazPobytu>();
                    if (itemOsoba.ZPOList != null && itemOsoba.ZPOList.Length > 0)
                    foreach (var itemZakazPobytu in itemOsoba.ZPOList)
                    {
                        var zakazPobytu = new ZakazPobytu();
                        osoba.ZakazPobytuList.Add(zakazPobytu);
                        zakazPobytu.PlatnostOd = itemZakazPobytu.DZ.Value;
                        zakazPobytu.PlatnostDo = itemZakazPobytu.DK;
                        zakazPobytu.Poznamka = itemZakazPobytu.PO;
                        zakazPobytu.Identifikator = itemZakazPobytu.ID;
                        zakazPobytu.UzemnyCelokKod = itemZakazPobytu.ZPZList.Length > 0 ? itemZakazPobytu.ZPZList[0].UC : itemOsoba.UC.Value;
                        //mapovanie ciselnika
                        zakazPobytu.RfoObecId = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoObec>().Exists(item => item.Kod == zakazPobytu.UzemnyCelokKod.ToString()) ? Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoObec>().Find(item => item.Kod == zakazPobytu.UzemnyCelokKod.ToString()).InternyKod.Value : Guid.Empty;
                    }

                    osoba.UdajePobytuList = new List<UdajePobytu>();
                    if (itemOsoba.POBList != null && itemOsoba.POBList.Length > 0)
                    foreach (var itemUdajePobytu in itemOsoba.POBList)
                    {
                        var udajePobytu = new UdajePobytu();
                        osoba.UdajePobytuList.Add(udajePobytu);
                        udajePobytu.Identifikator = itemUdajePobytu.ID;
                        udajePobytu.TypPobytuKod = itemUdajePobytu.TP;
                        //mapovanie nasich ciselnikov
                        udajePobytu.TypPobytuIk = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoTypPobytu>().Exists(item => item.Kod == udajePobytu.TypPobytuKod.ToString()) ? Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoTypPobytu>().Find(item => item.Kod == udajePobytu.TypPobytuKod.ToString()).InternyKod.Value : Guid.Empty;

                        udajePobytu.MimoSr = itemUdajePobytu.PM;
                        udajePobytu.StatKod = itemUdajePobytu.ST.HasValue ? itemUdajePobytu.ST.Value.ToString() : null;
                        //mapovanie nasich ciselnikov
                        if (!String.IsNullOrEmpty(udajePobytu.StatKod))
                            udajePobytu.RfoStatIk = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoStat>().Exists(item => item.Kod == udajePobytu.StatKod) ? Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoStat>().Find(item => item.Kod == udajePobytu.StatKod).InternyKod.Value : Guid.Empty;

                        udajePobytu.DatumPrihlaseniaNaPobyt = itemUdajePobytu.DP;
                        udajePobytu.DatumUkonceniaPobytu = itemUdajePobytu.DU;
                        udajePobytu.OkresKod = itemUdajePobytu.UE.HasValue ? itemUdajePobytu.UE.ToString() : null;
                        if (!String.IsNullOrEmpty(udajePobytu.OkresKod))
                            //mapovanie nasich ciselnikov
                            udajePobytu.RfoOkresIk = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoOkres>().Exists(item => item.Kod == udajePobytu.OkresKod.ToString()) ? Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoOkres>().Find(item => item.Kod == udajePobytu.OkresKod.ToString()).InternyKod.Value : Guid.Empty;

                        udajePobytu.ObecKod = itemUdajePobytu.OA.HasValue ? itemUdajePobytu.OA.ToString() : null;
                        if (!String.IsNullOrEmpty(udajePobytu.ObecKod))
                            //mapovanie nasich ciselnikov
                            udajePobytu.RfoObecIk = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoObec>().Exists(item => item.Kod == udajePobytu.ObecKod.ToString()) ? Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoObec>().Find(item => item.Kod == udajePobytu.ObecKod.ToString()).InternyKod.Value : Guid.Empty;

                        //mapovanie nasich ciselnikov
                        if (itemUdajePobytu.CE.HasValue)
                            udajePobytu.RfoCastObceIk = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoUzemnyCelok>().Exists(item => item.Kod == itemUdajePobytu.CE.Value.ToString()) ? Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoUzemnyCelok>().Find(item => item.Kod == itemUdajePobytu.CE.Value.ToString()).InternyKod.Value : Guid.Empty;

                        //mapovanie nasich ciselnikov
                        if (itemUdajePobytu.UI.HasValue)
                        {
                            udajePobytu.RfoUlicaIk = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoUlica>().Exists(item => item.Kod == itemUdajePobytu.UI.Value.ToString()) ? Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoUlica>().Find(item => item.Kod == itemUdajePobytu.UI.Value.ToString()).InternyKod.Value : Guid.Empty;
                            udajePobytu.UlicaZobrazena = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoUlica>().GetNazov(udajePobytu.RfoUlicaIk.Value);
                        }

                        if (udajePobytu.RfoUlicaIk == Guid.Empty)
                        {
                            udajePobytu.Ulica = itemUdajePobytu.NU;
                            udajePobytu.UlicaZobrazena = itemUdajePobytu.NU;
                        }


                        //udajePobytu.Ulica = itemUdajePobytu.NU;
                        udajePobytu.OrientacneCislo = itemUdajePobytu.OL;
                        udajePobytu.SupisneCislo = itemUdajePobytu.SC;
                        udajePobytu.MiestoMimoSr = itemUdajePobytu.MP;
                        udajePobytu.OkresMimoSr = itemUdajePobytu.OP;
                        udajePobytu.ObecMimoSr = itemUdajePobytu.OO;
                        udajePobytu.CastObceMimoSr = itemUdajePobytu.CC;
                        udajePobytu.UlicaMimoSr = itemUdajePobytu.UM;
                        udajePobytu.OrientacneCisloMimoSr = itemUdajePobytu.OS;
                        udajePobytu.SupisneCisloMimoSr = itemUdajePobytu.SI;

                        udajePobytu.RegionMimoSrList = new List<RegionMimoSr>();
                        if (itemUdajePobytu.REGList != null)
                        foreach (var itemRegionMimoSr in itemUdajePobytu.REGList)
                        {
                            var regionMimoSr = new RegionMimoSr();
                            udajePobytu.RegionMimoSrList.Add(regionMimoSr);
                            regionMimoSr.Poradie = itemRegionMimoSr.PO;
                            regionMimoSr.Hodnota = itemRegionMimoSr.RE;
                            regionMimoSr.Identifikator = itemRegionMimoSr.ID;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }

            return retVal;
        }
    }
}

namespace Ditec.RIS.RFO.Dol.ZoznamIFOSoZmenenymiReferencnymiUdajmiWS
{
    public partial class TransEnvTypeOut
    {
        public static implicit operator OsobaResponse(TransEnvTypeOut response)
        {
            var retVal = new OsobaResponse();
            retVal.OsobaList = new List<Osoba>();

            try
            {
                foreach (var itemOsoba in response.POV.ZZVList)
                {
                    retVal.OsobaList.Add(itemOsoba);
                }
            }
            catch
            {
                throw;
            }

            return retVal;
        }
    }

    public partial class TZZV_ZZDO
    {
        public static implicit operator Osoba(TZZV_ZZDO itemOsoba)
        {
            var osoba = new Osoba();
            osoba.FyzickaOsoba = new FyzickaOsoba();

            try
            {
                osoba.FyzickaOsoba.Ifo = itemOsoba.ID;
                osoba.FyzickaOsoba.IfoPravejOsoby = itemOsoba.NI;
                osoba.FyzickaOsoba.RodneCislo = itemOsoba.RC;

                //ak prisla osoba a treba ju vymazat, nema nic vyplnene okrem ifo
                //ak ma vyplnene len ifo a nema zakladne udaje - datum narodenia, meno a priezvisko
                if (!String.IsNullOrEmpty(osoba.FyzickaOsoba.Ifo) && !itemOsoba.DN.HasValue && (itemOsoba.ZMEList == null || itemOsoba.ZMEList.Length == 0) && (itemOsoba.ZPRList == null || itemOsoba.ZPRList.Length == 0))
                {
                    osoba.FyzickaOsoba.Vymazat = true;
                    osoba.FyzickaOsoba.IdentifikatorZmenovejDavky = itemOsoba.ZZ;
                    return osoba;
                }

                osoba.FyzickaOsoba.DatumNarodenia = itemOsoba.DN;
                osoba.FyzickaOsoba.UzemnyCelokNarodeniaKod = itemOsoba.UC;
                osoba.FyzickaOsoba.MiestoNarodeniaIne = itemOsoba.MN;
                osoba.FyzickaOsoba.StatNarodeniaKod = itemOsoba.SN;
                if (osoba.FyzickaOsoba.StatNarodeniaKod.HasValue)
                    osoba.FyzickaOsoba.StatNarodeniaIk = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoStat>().Exists(item => item.Kod == osoba.FyzickaOsoba.StatNarodeniaKod.ToString()) ? Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoStat>().Find(item => item.Kod == osoba.FyzickaOsoba.StatNarodeniaKod.ToString()).InternyKod.Value : Guid.Empty;

                osoba.FyzickaOsoba.UzemnyCelokNarodeniaKod = itemOsoba.UC;
                if (osoba.FyzickaOsoba.UzemnyCelokNarodeniaKod.HasValue)
                    osoba.FyzickaOsoba.ObecNarodeniaIk = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoObec>().Exists(item => item.Kod == osoba.FyzickaOsoba.UzemnyCelokNarodeniaKod.ToString()) ? Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoObec>().Find(item => item.Kod == osoba.FyzickaOsoba.UzemnyCelokNarodeniaKod.ToString()).InternyKod.Value : Guid.Empty;

                osoba.FyzickaOsoba.PohlavieKod = itemOsoba.PO.HasValue ? itemOsoba.PO.Value.ToString() : "";
                //mapovanie nasich ciselnikov
                osoba.FyzickaOsoba.RfoPohlavieIk = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoPohlavie>().Exists(item => item.Kod == osoba.FyzickaOsoba.PohlavieKod.ToString()) ? Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoPohlavie>().Find(item => item.Kod == osoba.FyzickaOsoba.PohlavieKod.ToString()).InternyKod.Value : Guid.Empty;

                osoba.FyzickaOsoba.RodinnyStavKod = itemOsoba.RS;
                if (osoba.FyzickaOsoba.RodinnyStavKod.HasValue)
                    //mapovanie nasich ciselnikov
                    osoba.FyzickaOsoba.RfoRodinnyStavIk = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoRodinnyStav>().Exists(item => item.Kod == osoba.FyzickaOsoba.RodinnyStavKod.ToString()) ? Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoRodinnyStav>().Find(item => item.Kod == osoba.FyzickaOsoba.RodinnyStavKod.ToString()).InternyKod.Value : Guid.Empty;

                osoba.FyzickaOsoba.NarodnostKod = itemOsoba.ND.HasValue ? itemOsoba.ND.Value.ToString() : "";
                //mapovanie ciselnika
                if (!String.IsNullOrEmpty(osoba.FyzickaOsoba.NarodnostKod))
                    osoba.FyzickaOsoba.RfoNarodnostIk = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoNarodnost>().Exists(item => item.Kod == osoba.FyzickaOsoba.NarodnostKod) ? Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoNarodnost>().Find(item => item.Kod == osoba.FyzickaOsoba.NarodnostKod).InternyKod : Guid.Empty;

                osoba.FyzickaOsoba.DatumUmrtia = itemOsoba.DU;
                osoba.FyzickaOsoba.DatumPravoplatnostiRozh = itemOsoba.DP;
                osoba.FyzickaOsoba.TypOsobyKod = itemOsoba.TV;
                if (osoba.FyzickaOsoba.TypOsobyKod.HasValue)
                    //mapovanie ciselnika
                    osoba.FyzickaOsoba.RfoTypOsobyIk = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoTypOsoby>().Exists(item => item.Kod == osoba.FyzickaOsoba.TypOsobyKod.ToString()) ? Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoTypOsoby>().Find(item => item.Kod == osoba.FyzickaOsoba.TypOsobyKod.ToString()).InternyKod : Guid.Empty;

                osoba.FyzickaOsoba.StupenZverejneniaKod = itemOsoba.SZ;
                if (osoba.FyzickaOsoba.StupenZverejneniaKod.HasValue)
                    //mapovanie ciselnika
                    osoba.FyzickaOsoba.RfoStupenZverejneniaIk = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoStupenZverejnenia>().Exists(item => item.Kod == osoba.FyzickaOsoba.StupenZverejneniaKod.ToString()) ? Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoStupenZverejnenia>().Find(item => item.Kod == osoba.FyzickaOsoba.StupenZverejneniaKod.ToString()).InternyKod : Guid.Empty;

                osoba.FyzickaOsoba.Navratovykod = itemOsoba.KO.HasValue ? itemOsoba.KO.Value : 1;
                osoba.FyzickaOsoba.DovodNeposkytnutiaUdajov = itemOsoba.NU;
                osoba.FyzickaOsoba.IdentifikatorZmenovejDavky = itemOsoba.ZZ;
                //Nove zadanie - CR_IRIS_150828_2_FO okres narodenia a miesto umrtia - WS
                osoba.FyzickaOsoba.OkresNarodeniaKod = itemOsoba.UE;
                if (osoba.FyzickaOsoba.OkresNarodeniaKod.HasValue)
                    osoba.FyzickaOsoba.RfoOkresNarodeniaIk = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoUzemnyCelok>().Exists(item => item.Kod == osoba.FyzickaOsoba.OkresNarodeniaKod.Value.ToString()) ? Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoUzemnyCelok>().Find(item => item.Kod == osoba.FyzickaOsoba.OkresNarodeniaKod.Value.ToString()).InternyKod.Value : Guid.Empty;
                osoba.FyzickaOsoba.OkresNarodeniaMimoCiselnik = itemOsoba.ON;
                osoba.FyzickaOsoba.UzemnyCelokUmrtiaKod = itemOsoba.UL;
                if (osoba.FyzickaOsoba.UzemnyCelokUmrtiaKod.HasValue)
                    osoba.FyzickaOsoba.RfoUzemnyCelokUmrtiaIk = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoUzemnyCelok>().Exists(item => item.Kod == osoba.FyzickaOsoba.UzemnyCelokUmrtiaKod.Value.ToString()) ? Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoUzemnyCelok>().Find(item => item.Kod == osoba.FyzickaOsoba.UzemnyCelokUmrtiaKod.Value.ToString()).InternyKod.Value : Guid.Empty;
                osoba.FyzickaOsoba.MiestoUmrtiaMimoCiselnik = itemOsoba.MU;

                osoba.MenoList = new List<Meno>();
                if (itemOsoba.ZMEList != null)
                    foreach (var itemMeno in itemOsoba.ZMEList.OrderBy(x => x.PA))
                    {
                        var meno = new Meno();
                        osoba.MenoList.Add(meno);
                        meno.Hodnota = itemMeno.ME.Trim();
                        meno.Poradie = itemMeno.PA;
                        meno.Identifikator = itemMeno.ID;

                        osoba.FyzickaOsoba.MenoZobrazovane += meno.Hodnota + " ";
                    }
                osoba.FyzickaOsoba.MenoZobrazovane = String.IsNullOrEmpty(osoba.FyzickaOsoba.MenoZobrazovane) ? null : osoba.FyzickaOsoba.MenoZobrazovane.Trim();

                osoba.PriezviskoList = new List<Priezvisko>();
                foreach (var itemPriezvisko in itemOsoba.ZPRList.OrderBy(x => x.PO))
                {
                    var priezvisko = new Priezvisko();
                    osoba.PriezviskoList.Add(priezvisko);
                    priezvisko.Hodnota = itemPriezvisko.PR.Trim();
                    priezvisko.Poradie = itemPriezvisko.PO;
                    priezvisko.Identifikator = itemPriezvisko.ID;

                    osoba.FyzickaOsoba.PriezviskoZobrazovane += priezvisko.Hodnota + " ";
                }
                osoba.FyzickaOsoba.PriezviskoZobrazovane = String.IsNullOrEmpty(osoba.FyzickaOsoba.PriezviskoZobrazovane) ? null : osoba.FyzickaOsoba.PriezviskoZobrazovane.Trim();

                osoba.RodnePriezviskoList = new List<RodnePriezvisko>();
                if (itemOsoba.ZRPList != null)
                    foreach (var itemRodnePriezvisko in itemOsoba.ZRPList)
                    {
                        var rodnePriezvisko = new RodnePriezvisko();
                        osoba.RodnePriezviskoList.Add(rodnePriezvisko);
                        rodnePriezvisko.Hodnota = itemRodnePriezvisko.RP.Trim();
                        rodnePriezvisko.Poradie = itemRodnePriezvisko.PO;
                        rodnePriezvisko.Identifikator = itemRodnePriezvisko.ID;

                        osoba.FyzickaOsoba.RodneMenoZobrazovane += rodnePriezvisko.Hodnota + " ";
                    }
                osoba.FyzickaOsoba.RodneMenoZobrazovane = String.IsNullOrEmpty(osoba.FyzickaOsoba.RodneMenoZobrazovane) ? null : osoba.FyzickaOsoba.RodneMenoZobrazovane.Trim();

                osoba.TitulList = new List<FyzickaOsobaTitul>();
                if (itemOsoba.ZTIList != null)
                    foreach (var itemTitul in itemOsoba.ZTIList)
                    {
                        var titul = new FyzickaOsobaTitul();
                        osoba.TitulList.Add(titul);
                        titul.TitulKod = itemTitul.TI.ToString();
                        //mapovanie ciselnika
                        titul.RfoTitulIk = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoTitul>().Exists(item => item.Kod == titul.TitulKod.ToString()) ? Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoTitul>().Find(item => item.Kod == titul.TitulKod.ToString()).InternyKod.Value : Guid.Empty;

                        titul.TypTituluKod = itemTitul.TT;
                        titul.Identifikator = itemTitul.ID;
                    }

                osoba.StatnaPrislusnostList = new List<FyzickaOsobaStatnaPrislusnost>();
                if (itemOsoba.ZSPList != null)
                    foreach (var itemStatnaPrislusnost in itemOsoba.ZSPList)
                    {
                        var statnaPrislusnost = new FyzickaOsobaStatnaPrislusnost();
                        osoba.StatnaPrislusnostList.Add(statnaPrislusnost);
                        statnaPrislusnost.StatnaPrislusnostKod = itemStatnaPrislusnost.SI.ToString();
                        //mapovanie ciselnika
                        statnaPrislusnost.RfoStatIk = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoStat>().Exists(item => item.Kod == statnaPrislusnost.StatnaPrislusnostKod.ToString()) ? Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoStat>().Find(item => item.Kod == statnaPrislusnost.StatnaPrislusnostKod.ToString()).InternyKod.Value : Guid.Empty;
                    }

                osoba.VztahovaOsobaList = new List<VztahovaFyzOsoba>();
                if (itemOsoba.ZRVList != null)
                    foreach (var itemVztahovaOsoba in itemOsoba.ZRVList)
                    {
                        var vztahovaOsoba = new VztahovaFyzOsoba();
                        osoba.VztahovaOsobaList.Add(vztahovaOsoba);
                        vztahovaOsoba.IfoVztahovejOsoby = itemVztahovaOsoba.ID;
                        vztahovaOsoba.TypVztahuKod = itemVztahovaOsoba.TR;
                        //mapovanie ciselnika
                        vztahovaOsoba.RfoTypRodinnehoVztahuIk = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoTypRodinnehoVztahu>().Exists(item => item.Kod == vztahovaOsoba.TypVztahuKod.ToString()) ? Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoTypRodinnehoVztahu>().Find(item => item.Kod == vztahovaOsoba.TypVztahuKod.ToString()).InternyKod.Value : Guid.Empty;

                        vztahovaOsoba.DatumVznikuVztahu = itemVztahovaOsoba.DV;
                        vztahovaOsoba.RolaOsobyVRodinnomVztahuKod = itemVztahovaOsoba.TL;
                        //mapovanie ciselnika
                        vztahovaOsoba.RfoTypRoleVRodiVztahuIk = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoTypRoleVRodiVztahu>().Exists(item => item.Kod == vztahovaOsoba.RolaOsobyVRodinnomVztahuKod.ToString()) ? Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoTypRoleVRodiVztahu>().Find(item => item.Kod == vztahovaOsoba.RolaOsobyVRodinnomVztahuKod.ToString()).InternyKod.Value : Guid.Empty;

                        vztahovaOsoba.RolaVztahovejOsobyVRodinnomVztahuKod = itemVztahovaOsoba.TO;
                        vztahovaOsoba.Identifikator = itemVztahovaOsoba.ID1;
                    }

                osoba.PravnaSposobilostObmedzenieList = new List<PravnaSposobilostObmedzenie>();
                if (itemOsoba.ZHSList != null)
                    foreach (var itemPravnaSposobilost in itemOsoba.ZHSList)
                    {
                        var pravnaSposobilost = new PravnaSposobilostObmedzenie();
                        osoba.PravnaSposobilostObmedzenieList.Add(pravnaSposobilost);
                        pravnaSposobilost.TypObmedzeniaKod = itemPravnaSposobilost.SA;
                        //mapovanie ciselnika
                        pravnaSposobilost.RfoSposobilostPravUkonIk = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoSposobilostPravUkon>().Exists(item => item.Kod == pravnaSposobilost.TypObmedzeniaKod.ToString()) ? Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoSposobilostPravUkon>().Find(item => item.Kod == pravnaSposobilost.TypObmedzeniaKod.ToString()).InternyKod.Value : Guid.Empty;

                        pravnaSposobilost.PlatnostOd = itemPravnaSposobilost.DZ;
                        pravnaSposobilost.PlatnostDo = itemPravnaSposobilost.DK;
                        pravnaSposobilost.Poznamka = itemPravnaSposobilost.PO;
                        pravnaSposobilost.Identifikator = itemPravnaSposobilost.ID;
                    }

                osoba.ZakazPobytuList = new List<ZakazPobytu>();
                if (itemOsoba.ZZPList != null)
                    foreach (var itemZakazPobytu in itemOsoba.ZZPList)
                    {
                        var zakazPobytu = new ZakazPobytu();
                        osoba.ZakazPobytuList.Add(zakazPobytu);
                        zakazPobytu.PlatnostOd = itemZakazPobytu.DZ.Value;
                        zakazPobytu.PlatnostDo = itemZakazPobytu.DK;
                        zakazPobytu.Poznamka = itemZakazPobytu.PO;
                        zakazPobytu.Identifikator = itemZakazPobytu.ID;
                        zakazPobytu.UzemnyCelokKod = itemZakazPobytu.UC.HasValue ? itemZakazPobytu.UC.Value : 0;
                        //mapovanie ciselnika
                        zakazPobytu.RfoObecId = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoObec>().Exists(item => item.Kod == zakazPobytu.UzemnyCelokKod.ToString()) ? Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoObec>().Find(item => item.Kod == zakazPobytu.UzemnyCelokKod.ToString()).InternyKod.Value : Guid.Empty;
                    }

                osoba.UdajePobytuList = new List<UdajePobytu>();
                if (itemOsoba.ZPBList != null)
                    foreach (var itemUdajePobytu in itemOsoba.ZPBList)
                    {
                        var udajePobytu = new UdajePobytu();
                        udajePobytu.RegionMimoSrList = new List<RegionMimoSr>();
                        osoba.UdajePobytuList.Add(udajePobytu);
                        udajePobytu.Identifikator = itemUdajePobytu.ID;
                        udajePobytu.TypPobytuKod = itemUdajePobytu.TP;
                        //mapovanie nasich ciselnikov
                        udajePobytu.TypPobytuIk = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoTypPobytu>().Exists(item => item.Kod == udajePobytu.TypPobytuKod.ToString()) ? Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoTypPobytu>().Find(item => item.Kod == udajePobytu.TypPobytuKod.ToString()).InternyKod.Value : Guid.Empty;

                        udajePobytu.MimoSr = itemUdajePobytu.PM.HasValue ? itemUdajePobytu.PM.Value : false;
                        udajePobytu.StatKod = itemUdajePobytu.SI.HasValue ? itemUdajePobytu.SI.Value.ToString() : null;
                        //mapovanie nasich ciselnikov
                        if (!String.IsNullOrEmpty(udajePobytu.StatKod))
                            udajePobytu.RfoStatIk = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoStat>().Exists(item => item.Kod == udajePobytu.StatKod) ? Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoStat>().Find(item => item.Kod == udajePobytu.StatKod).InternyKod.Value : Guid.Empty;

                        udajePobytu.DatumPrihlaseniaNaPobyt = itemUdajePobytu.DP;
                        udajePobytu.DatumUkonceniaPobytu = itemUdajePobytu.DK;
                        udajePobytu.OkresKod = itemUdajePobytu.UL.HasValue ? itemUdajePobytu.UL.ToString() : null;
                        if (!String.IsNullOrEmpty(udajePobytu.OkresKod))
                            //mapovanie nasich ciselnikov
                            udajePobytu.RfoOkresIk = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoOkres>().Exists(item => item.Kod == udajePobytu.OkresKod.ToString()) ? Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoOkres>().Find(item => item.Kod == udajePobytu.OkresKod.ToString()).InternyKod.Value : Guid.Empty;

                        udajePobytu.ObecKod = itemUdajePobytu.UC.HasValue ? itemUdajePobytu.UC.ToString() : null;
                        if (!String.IsNullOrEmpty(udajePobytu.ObecKod))
                            //mapovanie nasich ciselnikov
                            udajePobytu.RfoObecIk = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoObec>().Exists(item => item.Kod == udajePobytu.ObecKod.ToString()) ? Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoObec>().Find(item => item.Kod == udajePobytu.ObecKod.ToString()).InternyKod.Value : Guid.Empty;

                        //mapovanie nasich ciselnikov
                        if (itemUdajePobytu.UE.HasValue)
                            udajePobytu.RfoCastObceIk = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoUzemnyCelok>().Exists(item => item.Kod == itemUdajePobytu.UE.Value.ToString()) ? Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoUzemnyCelok>().Find(item => item.Kod == itemUdajePobytu.UE.Value.ToString()).InternyKod.Value : Guid.Empty;

                        //mapovanie nasich ciselnikov
                        if (itemUdajePobytu.UI.HasValue)
                        {
                            udajePobytu.RfoUlicaIk = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoUlica>().Exists(item => item.Kod == itemUdajePobytu.UI.Value.ToString()) ? Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoUlica>().Find(item => item.Kod == itemUdajePobytu.UI.Value.ToString()).InternyKod.Value : Guid.Empty;
                            udajePobytu.UlicaZobrazena = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoUlica>().GetNazov(udajePobytu.RfoUlicaIk.Value);
                        }

                        if (udajePobytu.RfoUlicaIk == Guid.Empty)
                        {
                            udajePobytu.Ulica = itemUdajePobytu.ULIUINA;
                            udajePobytu.UlicaZobrazena = itemUdajePobytu.ULIUINA;
                        }

                        //udajePobytu.Ulica = itemUdajePobytu.ULIUINA;
                        //IRIS_150805_1-Zmena mapovania orientacneho cisla
                        //Hodnotu orientacneho cisla nebrat z XML tagu OC , ale OL.
                        //(Tag OL bude pre vsetky WS).
                        udajePobytu.OrientacneCislo = itemUdajePobytu.OL;
                        udajePobytu.SupisneCislo = itemUdajePobytu.SC;
                        udajePobytu.MiestoMimoSr = itemUdajePobytu.CB;
                        udajePobytu.OkresMimoSr = itemUdajePobytu.OP;
                        udajePobytu.ObecMimoSr = itemUdajePobytu.OO;
                        udajePobytu.CastObceMimoSr = itemUdajePobytu.CO;
                        udajePobytu.UlicaMimoSr = itemUdajePobytu.UM;
                        udajePobytu.OrientacneCisloMimoSr = itemUdajePobytu.OI;
                        udajePobytu.SupisneCisloMimoSr = itemUdajePobytu.SS;

                        udajePobytu.RegionMimoSrList = new List<RegionMimoSr>();
                        if (itemUdajePobytu.ZREList != null)
                            foreach (var itemRegionMimoSr in itemUdajePobytu.ZREList)
                            {
                                var regionMimoSr = new RegionMimoSr();
                                udajePobytu.RegionMimoSrList.Add(regionMimoSr);
                                regionMimoSr.Poradie = itemRegionMimoSr.PO;
                                regionMimoSr.Hodnota = itemRegionMimoSr.RE;
                                regionMimoSr.Identifikator = itemRegionMimoSr.ID;
                            }
                    }
            }
            catch
            {
                throw;
            }
            return osoba;
        }
    }
}

namespace Ditec.RIS.RFO.Dol.ZapisNovychOsobWS
{
    public partial class TransEnvTypeIn
    {
        public static implicit operator TransEnvTypeIn(OsobaResponse osobaResponse)
        {
            var retVal = new TransEnvTypeIn();
            retVal.OEXList = new TOEX_OEI[osobaResponse.OsobaList.Count];

            try
            {
                for (int i = 0; i < osobaResponse.OsobaList.Count; i++ )
                {
                    retVal.OEXList[i] = osobaResponse.OsobaList[i];
                    retVal.OEXList[i].PA = (i + 1);
                }
            }
            catch
            {
                throw;
            }

            return retVal;
        }
    }

    public partial class TOEX_OEI
    {
        public static implicit operator TOEX_OEI(Osoba itemOsoba)
        {
            var osoba = new TOEX_OEI();
            try
            {
                {
                    #region FyzickaOsoba

                    if (itemOsoba.FyzickaOsoba.RfoTypOsobyIk.HasValue)
                    {
                        osoba.TV = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoTypOsoby>().GetKodByInternyKod(itemOsoba.FyzickaOsoba.RfoTypOsobyIk);
                        osoba.TVKTVNA = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoTypOsoby>().GetNazov(itemOsoba.FyzickaOsoba.RfoTypOsobyIk);
                    }
                    osoba.RC = itemOsoba.FyzickaOsoba.RodneCislo;
                    osoba.DN = itemOsoba.FyzickaOsoba.DatumNarodenia.Value;
                    osoba.MR = itemOsoba.FyzickaOsoba.MiestoNarodeniaIne;
                    if (itemOsoba.FyzickaOsoba.ObecNarodeniaIk.HasValue)
                    {
                        osoba.UC = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoUzemnyCelok>().GetKodByInternyKod(itemOsoba.FyzickaOsoba.ObecNarodeniaIk);
                        osoba.UCEUCNA = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoUzemnyCelok>().GetNazov(itemOsoba.FyzickaOsoba.ObecNarodeniaIk);
                    }

                    osoba.PO = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoPohlavie>().GetKodByInternyKod(itemOsoba.FyzickaOsoba.RfoPohlavieIk);
                    osoba.POHPONA = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoPohlavie>().GetNazov(itemOsoba.FyzickaOsoba.RfoPohlavieIk);

                    if (itemOsoba.FyzickaOsoba.StatNarodeniaIk.HasValue)
                    {
                        osoba.SN = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoStat>().GetKodByInternyKod(itemOsoba.FyzickaOsoba.StatNarodeniaIk);
                        osoba.STASNNA = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoStat>().GetNazov(itemOsoba.FyzickaOsoba.StatNarodeniaIk);
                    }

                    if (itemOsoba.FyzickaOsoba.RfoRodinnyStavIk.HasValue)
                    {
                        osoba.RS = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoRodinnyStav>().GetKodByInternyKod(itemOsoba.FyzickaOsoba.RfoRodinnyStavIk);
                        osoba.RSTRSNA = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoRodinnyStav>().GetNazov(itemOsoba.FyzickaOsoba.RfoRodinnyStavIk);
                    }

                    if (itemOsoba.FyzickaOsoba.RfoNarodnostIk.HasValue)
                    {
                        osoba.NI = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoNarodnost>().GetKodByInternyKod(itemOsoba.FyzickaOsoba.RfoNarodnostIk);
                        osoba.NARNINA = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoNarodnost>().GetNazov(itemOsoba.FyzickaOsoba.RfoNarodnostIk);
                    }
                    //Identifikátor cudzinca používaný v cudzine. - nema sa mapovat
                    //XML tag: IC
                    osoba.ON = itemOsoba.FyzickaOsoba.OkresNarodeniaMimoCiselnik;
                    if (itemOsoba.FyzickaOsoba.RfoStupenZverejneniaIk.HasValue)
                    {
                        osoba.SZ = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoStupenZverejnenia>().GetKodByInternyKod(itemOsoba.FyzickaOsoba.RfoStupenZverejneniaIk);
                        osoba.SZVSZNA = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoStupenZverejnenia>().GetNazov(itemOsoba.FyzickaOsoba.RfoStupenZverejneniaIk);
                    }
                    osoba.ZR = false;

                    #endregion FyzickaOsoba

                    #region StatnaPrislusnost

                    osoba.SPRList = new TSPR_SPI[itemOsoba.StatnaPrislusnostList.Count];
                    for (int i = 0; i < itemOsoba.StatnaPrislusnostList.Count; i++)
                    {
                        var prislusnot = new TSPR_SPI();
                        osoba.SPRList[i] = prislusnot;

                        prislusnot.ST = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoStat>().GetKodByInternyKod(itemOsoba.StatnaPrislusnostList[i].RfoStatIk);
                        prislusnot.STASTNA = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoStat>().GetNazov(itemOsoba.StatnaPrislusnostList[i].RfoStatIk);
                    }

                    #endregion StatnaPrislusnost

                    #region Meno

                    osoba.MOSList = new TMOS[itemOsoba.MenoList.Count];
                    for (int i = 0; i < itemOsoba.MenoList.Count; i++)
                    {
                        var meno = new TMOS();
                        osoba.MOSList[i] = meno;

                        meno.ME = itemOsoba.MenoList[i].Hodnota;
                        meno.PO = itemOsoba.MenoList[i].Poradie;
                    }

                    #endregion Meno

                    #region Priezvisko

                    osoba.PRIList = new TPRI[itemOsoba.PriezviskoList.Count];
                    for (int i = 0; i < itemOsoba.PriezviskoList.Count; i++)
                    {
                        var priezvisko = new TPRI();
                        osoba.PRIList[i] = priezvisko;

                        priezvisko.PR = itemOsoba.PriezviskoList[i].Hodnota;
                        priezvisko.PO = itemOsoba.PriezviskoList[i].Poradie;
                    }

                    #endregion Priezvisko

                    #region RodnePriezvisko

                    osoba.RPRList = new TRPR[itemOsoba.RodnePriezviskoList.Count];
                    for (int i = 0; i < itemOsoba.RodnePriezviskoList.Count; i++)
                    {
                        var rodnePriezvisko = new TRPR();
                        osoba.RPRList[i] = rodnePriezvisko;

                        rodnePriezvisko.RP = itemOsoba.RodnePriezviskoList[i].Hodnota;
                        rodnePriezvisko.PO = itemOsoba.RodnePriezviskoList[i].Poradie;
                    }

                    #endregion RodnePriezvisko

                    #region Titul

                    osoba.TOSList = new TTOS_TOI[itemOsoba.TitulList.Count];
                    for (int i = 0; i < itemOsoba.TitulList.Count; i++)
                    {
                        var titul = new TTOS_TOI();
                        osoba.TOSList[i] = titul;

                        titul.TI = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoTitul>().GetKodByInternyKod(itemOsoba.TitulList[i].RfoTitulIk);
                        titul.TITTINA = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoTitul>().GetNazov(itemOsoba.TitulList[i].RfoTitulIk);

                        var typTituluIk = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoTitul>().GetTypTituluIk(itemOsoba.TitulList[i].RfoTitulIk);
                        titul.TT = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoTypTitulu>().GetKodByInternyKod(typTituluIk);
                        titul.TTITTNA = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoTypTitulu>().GetNazov(typTituluIk);
                    }

                    #endregion Titul

                    #region UdajePobytu

                    osoba.POBList = new TPOB_PHR[itemOsoba.UdajePobytuList.Count];
                    for (int i = 0; i < itemOsoba.UdajePobytuList.Count; i++)
                    {
                        var pobyt = new TPOB_PHR();
                        osoba.POBList[i] = pobyt;

                        pobyt.TP = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoTypPobytu>().GetKodByInternyKod(itemOsoba.UdajePobytuList[i].TypPobytuIk);
                        pobyt.TB = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoTypPobytu>().GetNazov(itemOsoba.UdajePobytuList[i].TypPobytuIk);

                        pobyt.DP = DateTime.Now;
                        pobyt.CC = itemOsoba.UdajePobytuList[i].CastObceMimoSr;
                        pobyt.OO = itemOsoba.UdajePobytuList[i].ObecMimoSr;
                        pobyt.OP = itemOsoba.UdajePobytuList[i].OkresMimoSr;
                        pobyt.OS = itemOsoba.UdajePobytuList[i].OrientacneCisloMimoSr;
                        pobyt.UM = itemOsoba.UdajePobytuList[i].UlicaMimoSr;
                        pobyt.SI = itemOsoba.UdajePobytuList[i].SupisneCisloMimoSr;
                        pobyt.ST = Ditec.RIS.CC.Tools.Tools.GetInstance().GetCodeList<RfoStat>().GetKodByInternyKod(itemOsoba.UdajePobytuList[i].RfoStatIk);
                        pobyt.MP = itemOsoba.UdajePobytuList[i].MiestoMimoSr;

                        #region RegionAdresyMimoSR

                        pobyt.REGList = new TREG[itemOsoba.UdajePobytuList[i].RegionMimoSrList.Count];
                        for (int a = 0; a < itemOsoba.UdajePobytuList[i].RegionMimoSrList.Count; a++)
                        {
                            var region = new TREG();
                            pobyt.REGList[a] = region;

                            region.PO = itemOsoba.UdajePobytuList[i].RegionMimoSrList[a].Poradie;
                            region.RE = itemOsoba.UdajePobytuList[i].RegionMimoSrList[a].Hodnota;
                        }

                        #endregion RegionAdresyMimoSR
                    }

                    #endregion UdajePobytu
                }
            }
            catch
            {
                throw;
            }

            return osoba;
        }
    }
}
