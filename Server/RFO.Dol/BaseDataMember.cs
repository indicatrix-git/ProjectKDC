using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Ditec.RIS.RFO.Dol
{
    [KnownType(typeof(PravnaSposobilostObmedzenie))]
    [XmlInclude(typeof(PravnaSposobilostObmedzenie))]
    [KnownType(typeof(FyzickaOsobaTitul))]
    [XmlInclude(typeof(FyzickaOsobaTitul))]
    [KnownType(typeof(StotoznenaFyzOsoba))]
    [XmlInclude(typeof(StotoznenaFyzOsoba))]
    [KnownType(typeof(Meno))]
    [XmlInclude(typeof(Meno))]
    [KnownType(typeof(VztahovaFyzOsoba))]
    [XmlInclude(typeof(VztahovaFyzOsoba))]
    [KnownType(typeof(UdajePobytu))]
    [XmlInclude(typeof(UdajePobytu))]
    [KnownType(typeof(ZakazPobytu))]
    [XmlInclude(typeof(ZakazPobytu))]
    [KnownType(typeof(Priezvisko))]
    [XmlInclude(typeof(Priezvisko))]
    [KnownType(typeof(FyzickaOsobaStatnaPrislusnost))]
    [XmlInclude(typeof(FyzickaOsobaStatnaPrislusnost))]
    [KnownType(typeof(RodnePriezvisko))]
    [XmlInclude(typeof(RodnePriezvisko))]
    [KnownType(typeof(RegionMimoSr))]
    [XmlInclude(typeof(RegionMimoSr))]
    [KnownType(typeof(FyzickaOsoba))]
    [XmlInclude(typeof(FyzickaOsoba))]
    [KnownType(typeof(PravnickaOsoba))]
    [XmlInclude(typeof(PravnickaOsoba))]
	[KnownType(typeof(SparovanieSRFO))]
	[XmlInclude(typeof(SparovanieSRFO))]
    [DataContract()]
    public class BaseDataMember : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged implements

        [field: NonSerialized]
        public virtual event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Vola sa ked sa zmeni hodnota vlastnosti
        /// </summary>
        /// <param name="propertyName">Nazov vlastnosti.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion INotifyPropertyChanged implements

        private Guid? _ID;
        [DataMember]
        public virtual Guid? ID
        {
            get
            {
                return _ID;
            }
            set
            {
                if (value != _ID)
                {
                    _ID = value;
                    OnPropertyChanged("ID");
                }
            }
        }

        private Guid? _TransakciaId;

        /// <summary>
        /// Identifikačné číslo transakcie
        /// </summary>
        [DataMember]
        public virtual Guid? TransakciaId
        {
            get { return _TransakciaId; }
            set
            {
                if (_TransakciaId != value)
                {
                    _TransakciaId = value;
                    OnPropertyChanged("TransakciaId");
                }
            }
        }

        private DateTime? _Changed;

        ///<summary>
        ///Datum a čas zmeny hodnôt  
        ///</summary>
        [DataMember]
        public virtual DateTime? Changed
        {
            get { return _Changed; }
            set
            {
                if (_Changed != value)
                {
                    _Changed = value;
                    OnPropertyChanged("Changed");
                }
            }
        }
    }

    public static class BaseDataMemberExtension
    {
        public static List<T> ToChildList<T>(this IList<BaseDataMember> list)
        {
            var newList = new List<T>();
            foreach (var item in list)
            {
                newList.Add((T)Convert.ChangeType(item, typeof(T)));
            }
            return newList;
        }
    }
}
