using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Ditec.SysFra.DataTypes.Infrastructure;

namespace Ditec.RIS.RFO.Dol
{
    [KnownType(typeof(PravnaSposobilostObmedzenieFilterCriteria))]
    [XmlInclude(typeof(PravnaSposobilostObmedzenieFilterCriteria))]
    [KnownType(typeof(FyzickaOsobaTitulFilterCriteria))]
    [XmlInclude(typeof(FyzickaOsobaTitulFilterCriteria))]
    [KnownType(typeof(StotoznenaFyzOsobaFilterCriteria))]
    [XmlInclude(typeof(StotoznenaFyzOsobaFilterCriteria))]
    [KnownType(typeof(MenoFilterCriteria))]
    [XmlInclude(typeof(MenoFilterCriteria))]
    [KnownType(typeof(VztahovaFyzOsobaFilterCriteria))]
    [XmlInclude(typeof(VztahovaFyzOsobaFilterCriteria))]
    [KnownType(typeof(UdajePobytuFilterCriteria))]
    [XmlInclude(typeof(UdajePobytuFilterCriteria))]
    [KnownType(typeof(ZakazPobytuFilterCriteria))]
    [XmlInclude(typeof(ZakazPobytuFilterCriteria))]
    [KnownType(typeof(PriezviskoFilterCriteria))]
    [XmlInclude(typeof(PriezviskoFilterCriteria))]
    [KnownType(typeof(FyzickaOsobaStatnaPrislusnostFilterCriteria))]
    [XmlInclude(typeof(FyzickaOsobaStatnaPrislusnostFilterCriteria))]
    [KnownType(typeof(RodnePriezviskoFilterCriteria))]
    [XmlInclude(typeof(RodnePriezviskoFilterCriteria))]
    [KnownType(typeof(RegionMimoSrFilterCriteria))]
    [XmlInclude(typeof(RegionMimoSrFilterCriteria))]
    [KnownType(typeof(FyzickaOsobaFilterCriteria))]
    [XmlInclude(typeof(FyzickaOsobaFilterCriteria))]
    [KnownType(typeof(PravnickaOsobaFilterCriteria))]
    [XmlInclude(typeof(PravnickaOsobaFilterCriteria))]
    [DataContract]
    public class BaseDataMemberFilterCriteria : INotifyPropertyChanged
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

        private DateTime? _DatumPlatnosti;
        [DataMember]
        public virtual DateTime? DatumPlatnosti
        {
            get
            {
                return _DatumPlatnosti;
            }
            set
            {
                if (value != _DatumPlatnosti)
                {
                    _DatumPlatnosti = value;
                    OnPropertyChanged("DatumPlatnosti");
                }
            }
        }

        [DataMember]
        public PagingInfo PagingInfo { get; set; }
    }
}
