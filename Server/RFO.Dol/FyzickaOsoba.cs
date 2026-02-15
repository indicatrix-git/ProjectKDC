using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using System.Runtime.Serialization;


namespace Ditec.RIS.RFO.Dol
{
	[DataContract()]
	public partial class FyzickaOsoba  : BaseDataMember
	{
		
        private Guid _RfoPohlavieIk; 
        [DataMember] 
        public virtual Guid RfoPohlavieIk  
        {   
            get   
            {  
                return _RfoPohlavieIk; 
            } 
            set 
            {
                if (value != _RfoPohlavieIk) 
                {
                    _RfoPohlavieIk = value; 
                    OnPropertyChanged("RfoPohlavieIk"); 
                } 
            } 
        } 

        private Guid? _RfoRodinnyStavIk; 
        [DataMember] 
        public virtual Guid? RfoRodinnyStavIk  
        {   
            get   
            {  
                return _RfoRodinnyStavIk; 
            } 
            set 
            {
                if (value != _RfoRodinnyStavIk) 
                {
                    _RfoRodinnyStavIk = value; 
                    OnPropertyChanged("RfoRodinnyStavIk"); 
                } 
            } 
        } 

        private Guid? _RfoTypOsobyIk; 
        [DataMember] 
        public virtual Guid? RfoTypOsobyIk  
        {   
            get   
            {  
                return _RfoTypOsobyIk; 
            } 
            set 
            {
                if (value != _RfoTypOsobyIk) 
                {
                    _RfoTypOsobyIk = value; 
                    OnPropertyChanged("RfoTypOsobyIk"); 
                } 
            } 
        } 

        private Guid? _RfoNarodnostIk; 
        [DataMember] 
        public virtual Guid? RfoNarodnostIk  
        {   
            get   
            {  
                return _RfoNarodnostIk; 
            } 
            set 
            {
                if (value != _RfoNarodnostIk) 
                {
                    _RfoNarodnostIk = value; 
                    OnPropertyChanged("RfoNarodnostIk"); 
                } 
            } 
        } 

        private Guid? _RfoStupenZverejneniaIk; 
        [DataMember] 
        public virtual Guid? RfoStupenZverejneniaIk  
        {   
            get   
            {  
                return _RfoStupenZverejneniaIk; 
            } 
            set 
            {
                if (value != _RfoStupenZverejneniaIk) 
                {
                    _RfoStupenZverejneniaIk = value; 
                    OnPropertyChanged("RfoStupenZverejneniaIk"); 
                } 
            } 
        } 

        private Guid? _StatNarodeniaIk; 
        [DataMember] 
        public virtual Guid? StatNarodeniaIk  
        {   
            get   
            {  
                return _StatNarodeniaIk; 
            } 
            set 
            {
                if (value != _StatNarodeniaIk) 
                {
                    _StatNarodeniaIk = value; 
                    OnPropertyChanged("StatNarodeniaIk"); 
                } 
            } 
        } 

        private Guid? _ObecNarodeniaIk; 
        [DataMember] 
        public virtual Guid? ObecNarodeniaIk  
        {   
            get   
            {  
                return _ObecNarodeniaIk; 
            } 
            set 
            {
                if (value != _ObecNarodeniaIk) 
                {
                    _ObecNarodeniaIk = value; 
                    OnPropertyChanged("ObecNarodeniaIk"); 
                } 
            } 
        } 

        private Int32 _Eduid; 
        [DataMember] 
        public virtual Int32 Eduid  
        {   
            get   
            {  
                return _Eduid; 
            } 
            set 
            {
                if (value != _Eduid) 
                {
                    _Eduid = value; 
                    OnPropertyChanged("Eduid"); 
                } 
            } 
        } 

        private String _Ifo; 
        [DataMember] 
        [System.ComponentModel.DataAnnotations.StringLength(30)]
        public virtual String Ifo  
        {   
            get   
            {  
                return _Ifo; 
            } 
            set 
            {
                if (value != _Ifo) 
                {
                    _Ifo = value; 
                    OnPropertyChanged("Ifo"); 
                } 
            } 
        } 

        private String _RodneCislo; 
        [DataMember] 
        [System.ComponentModel.DataAnnotations.StringLength(30)]
        public virtual String RodneCislo  
        {   
            get   
            {  
                return _RodneCislo; 
            } 
            set 
            {
                if (value != _RodneCislo) 
                {
                    _RodneCislo = value; 
                    OnPropertyChanged("RodneCislo"); 
                } 
            } 
        } 

        private DateTime? _DatumNarodenia; 
        [DataMember] 
        public virtual DateTime? DatumNarodenia  
        {   
            get   
            {  
                return _DatumNarodenia; 
            } 
            set 
            {
                if (value != _DatumNarodenia) 
                {
                    _DatumNarodenia = value; 
                    OnPropertyChanged("DatumNarodenia"); 
                } 
            } 
        } 

        private String _MiestoNarodeniaIne; 
        [DataMember] 
        [System.ComponentModel.DataAnnotations.StringLength(30)]
        public virtual String MiestoNarodeniaIne  
        {   
            get   
            {  
                return _MiestoNarodeniaIne; 
            } 
            set 
            {
                if (value != _MiestoNarodeniaIne) 
                {
                    _MiestoNarodeniaIne = value; 
                    OnPropertyChanged("MiestoNarodeniaIne"); 
                } 
            } 
        } 

        private DateTime? _DatumUmrtia; 
        [DataMember] 
        public virtual DateTime? DatumUmrtia  
        {   
            get   
            {  
                return _DatumUmrtia; 
            } 
            set 
            {
                if (value != _DatumUmrtia) 
                {
                    _DatumUmrtia = value; 
                    OnPropertyChanged("DatumUmrtia"); 
                } 
            } 
        } 

        private DateTime? _DatumPravoplatnostiRozh; 
        [DataMember] 
        public virtual DateTime? DatumPravoplatnostiRozh  
        {   
            get   
            {  
                return _DatumPravoplatnostiRozh; 
            } 
            set 
            {
                if (value != _DatumPravoplatnostiRozh) 
                {
                    _DatumPravoplatnostiRozh = value; 
                    OnPropertyChanged("DatumPravoplatnostiRozh"); 
                } 
            } 
        } 

        private Boolean _ZaujmovaOsoba; 
        [DataMember] 
        public virtual Boolean ZaujmovaOsoba  
        {   
            get   
            {  
                return _ZaujmovaOsoba; 
            } 
            set 
            {
                if (value != _ZaujmovaOsoba) 
                {
                    _ZaujmovaOsoba = value; 
                    OnPropertyChanged("ZaujmovaOsoba"); 
                } 
            } 
        } 

        private String _MenoZobrazovane; 
        [DataMember] 
        [System.ComponentModel.DataAnnotations.StringLength(255)]
        public virtual String MenoZobrazovane  
        {   
            get   
            {  
                return _MenoZobrazovane; 
            } 
            set 
            {
                if (value != _MenoZobrazovane) 
                {
                    _MenoZobrazovane = value; 
                    OnPropertyChanged("MenoZobrazovane"); 
                } 
            } 
        } 

        private String _PriezviskoZobrazovane; 
        [DataMember] 
        [System.ComponentModel.DataAnnotations.StringLength(255)]
        public virtual String PriezviskoZobrazovane  
        {   
            get   
            {  
                return _PriezviskoZobrazovane; 
            } 
            set 
            {
                if (value != _PriezviskoZobrazovane) 
                {
                    _PriezviskoZobrazovane = value; 
                    OnPropertyChanged("PriezviskoZobrazovane"); 
                } 
            } 
        } 

        private String _RodneMenoZobrazovane; 
        [DataMember] 
        [System.ComponentModel.DataAnnotations.StringLength(255)]
        public virtual String RodneMenoZobrazovane  
        {   
            get   
            {  
                return _RodneMenoZobrazovane; 
            } 
            set 
            {
                if (value != _RodneMenoZobrazovane) 
                {
                    _RodneMenoZobrazovane = value; 
                    OnPropertyChanged("RodneMenoZobrazovane"); 
                } 
            } 
        } 

        private Boolean _Zneplatnena; 
        [DataMember] 
        public virtual Boolean Zneplatnena  
        {   
            get   
            {  
                return _Zneplatnena; 
            } 
            set 
            {
                if (value != _Zneplatnena) 
                {
                    _Zneplatnena = value; 
                    OnPropertyChanged("Zneplatnena"); 
                } 
            } 
        } 

        private Int64? _IdentifikatorZmenovejDavky; 
        [DataMember] 
        public virtual Int64? IdentifikatorZmenovejDavky  
        {   
            get   
            {  
                return _IdentifikatorZmenovejDavky; 
            } 
            set 
            {
                if (value != _IdentifikatorZmenovejDavky) 
                {
                    _IdentifikatorZmenovejDavky = value; 
                    OnPropertyChanged("IdentifikatorZmenovejDavky"); 
                } 
            } 
        } 

        private String _Bifo; 
        [DataMember] 
        [System.ComponentModel.DataAnnotations.StringLength(20)]
        public virtual String Bifo  
        {   
            get   
            {  
                return _Bifo; 
            } 
            set 
            {
                if (value != _Bifo) 
                {
                    _Bifo = value; 
                    OnPropertyChanged("Bifo"); 
                } 
            } 
        } 

        private Guid? _TransakciaZruseneId; 
        [DataMember] 
        public virtual Guid? TransakciaZruseneId  
        {   
            get   
            {  
                return _TransakciaZruseneId; 
            } 
            set 
            {
                if (value != _TransakciaZruseneId) 
                {
                    _TransakciaZruseneId = value; 
                    OnPropertyChanged("TransakciaZruseneId"); 
                } 
            } 
        } 

        private String _RodneCisloClear; 
        [DataMember] 
        [System.ComponentModel.DataAnnotations.StringLength(4000)]
        public virtual String RodneCisloClear  
        {   
            get   
            {  
                return _RodneCisloClear; 
            } 
            set 
            {
                if (value != _RodneCisloClear) 
                {
                    _RodneCisloClear = value; 
                    OnPropertyChanged("RodneCisloClear"); 
                } 
            } 
        } 


        public virtual string GetTableName  
        {   
            get   
            {  
                return "FO.FYZICKA_OSOBA"; 
            } 
        } 

	}
}
