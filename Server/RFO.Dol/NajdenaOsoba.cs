using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Ditec.RIS.RFO.Dol
{
    [DataContract]
    public class NajdenaOsoba : BaseDataMember
    {
        private int? _EDUIDNajdene;
        [DataMember]
        public virtual int? EDUIDNajdene
        {
            get
            {
                return _EDUIDNajdene;
            }
            set
            {
                if (value != _EDUIDNajdene)
                {
                    _EDUIDNajdene = value;
                    OnPropertyChanged("EDUIDNajdene");
                }
            }
        }
        
        private bool _Najdena;
        [DataMember]
        public virtual bool Najdena
        {
            get
            {
                return _Najdena;
            }
            set
            {
                if (value != _Najdena)
                {
                    _Najdena = value;
                    OnPropertyChanged("Najdena");
                }
            }
        }
        
        private TypNajdenia _TypNajdenia;
        [DataMember]
        public virtual TypNajdenia TypNajdenia
        {
            get
            {
                return _TypNajdenia;
            }
            set
            {
                if (value != _TypNajdenia)
                {
                    _TypNajdenia = value;
                    OnPropertyChanged("TypNajdenia");
                }
            }
        }
        
        private bool _JeZadanyTypOsobyVRIS;
        [DataMember]
        public virtual bool JeZadanyTypOsobyVRIS
        {
            get
            {
                return _JeZadanyTypOsobyVRIS;
            }
            set
            {
                if (value != _JeZadanyTypOsobyVRIS)
                {
                    _JeZadanyTypOsobyVRIS = value;
                    OnPropertyChanged("JeZadanyTypOsobyVRIS");
                }
            }
        }
        
        private FyzickaOsoba _FyzickaOsobaNajdena;
        [DataMember]
        public virtual FyzickaOsoba FyzickaOsobaNajdena
        {
            get
            {
                return _FyzickaOsobaNajdena;
            }
            set
            {
                if (value != _FyzickaOsobaNajdena)
                {
                    _FyzickaOsobaNajdena = value;
                    OnPropertyChanged("FyzickaOsobaNajdena");
                }
            }
        }

        private int? _EDUIDPovodna;
        [DataMember]
        public virtual int? EDUIDPovodna
        {
            get
            {
                return _EDUIDPovodna;
            }
            set
            {
                if (value != _EDUIDPovodna)
                {
                    _EDUIDPovodna = value;
                    OnPropertyChanged("EDUIDPovodna");
                }
            }
        }

        private FyzickaOsoba _FyzickaOsobaPovodna;
        [DataMember]
        public virtual FyzickaOsoba FyzickaOsobaPovodna
        {
            get
            {
                return _FyzickaOsobaPovodna;
            }
            set
            {
                if (value != _FyzickaOsobaPovodna)
                {
                    _FyzickaOsobaPovodna = value;
                    OnPropertyChanged("FyzickaOsobaPovodna");
                }
            }
        }

        
        private List<Ditec.RIS.RFO.Dol.TypOsobyVRis> _TypOsobyVRISList;
        [DataMember]
        public virtual List<Ditec.RIS.RFO.Dol.TypOsobyVRis> TypOsobyVRISList
        {
            get
            {
                return _TypOsobyVRISList;
            }
            set
            {
                if (value != _TypOsobyVRISList)
                {
                    _TypOsobyVRISList = value;
                    
                    if (_TypOsobyVRISList != null && _TypOsobyVRISList.Count > 0 && (_TypOsobyVRISList.Count > 1 || _TypOsobyVRISList[0] != TypOsobyVRis.Nezadane))
                        this.JeZadanyTypOsobyVRIS = true;
                    else
                        this.JeZadanyTypOsobyVRIS = false;

                    OnPropertyChanged("TypOsobyVRISList");
                }
            }
        }

    }

    public enum TypNajdenia
    {
        Najdena = 0,
        TerajsiaNenajdenaExistujePovodna,
        TerajsiaNenajdena,
        PravaNajdenaExistujePovodna,
        PravaNajdenaNeexistujePovodna,
        PravaNenajdenaExistujePovodna,
        PravaNenajdenaNeexistujePovodna,
        ExistujuceStotoznenie,
        StotoznenaSInouOsobou
    }
}
