using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using System.Runtime.Serialization;


namespace Ditec.RIS.RFO.Dol
{
	[DataContract()]
	public partial class PravnickaOsobaFilterCriteria  : BaseDataMemberFilterCriteria
	{
		
        private Guid? _PravnaFormaOrgIk; 
        [DataMember] 
        public virtual Guid? PravnaFormaOrgIk  
        {   
            get   
            {  
                return _PravnaFormaOrgIk; 
            } 
            set 
            {
                if (value != _PravnaFormaOrgIk) 
                {
                    _PravnaFormaOrgIk = value; 
                    OnPropertyChanged("PravnaFormaOrgIk"); 
                } 
            } 
        } 

        private Guid? _KodCinnostiIk; 
        [DataMember] 
        public virtual Guid? KodCinnostiIk  
        {   
            get   
            {  
                return _KodCinnostiIk; 
            } 
            set 
            {
                if (value != _KodCinnostiIk) 
                {
                    _KodCinnostiIk = value; 
                    OnPropertyChanged("KodCinnostiIk"); 
                } 
            } 
        } 

        private String _Ico; 
        [DataMember] 
        [System.ComponentModel.DataAnnotations.StringLength(30)]
        public virtual String Ico  
        {   
            get   
            {  
                return _Ico; 
            } 
            set 
            {
                if (value != _Ico) 
                {
                    _Ico = value; 
                    OnPropertyChanged("Ico"); 
                } 
            } 
        } 

        private String _Dic; 
        [DataMember] 
        [System.ComponentModel.DataAnnotations.StringLength(30)]
        public virtual String Dic  
        {   
            get   
            {  
                return _Dic; 
            } 
            set 
            {
                if (value != _Dic) 
                {
                    _Dic = value; 
                    OnPropertyChanged("Dic"); 
                } 
            } 
        } 

        private String _IcDph; 
        [DataMember] 
        [System.ComponentModel.DataAnnotations.StringLength(30)]
        public virtual String IcDph  
        {   
            get   
            {  
                return _IcDph; 
            } 
            set 
            {
                if (value != _IcDph) 
                {
                    _IcDph = value; 
                    OnPropertyChanged("IcDph"); 
                } 
            } 
        } 

        private String _Nazov; 
        [DataMember] 
        [System.ComponentModel.DataAnnotations.StringLength(30)]
        public virtual String Nazov  
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
                    OnPropertyChanged("Nazov"); 
                } 
            } 
        } 

        private DateTime? _DatumVzniku; 
        [DataMember] 
        public virtual DateTime? DatumVzniku  
        {   
            get   
            {  
                return _DatumVzniku; 
            } 
            set 
            {
                if (value != _DatumVzniku) 
                {
                    _DatumVzniku = value; 
                    OnPropertyChanged("DatumVzniku"); 
                } 
            } 
        } 

        private DateTime? _DatumZaniku; 
        [DataMember] 
        public virtual DateTime? DatumZaniku  
        {   
            get   
            {  
                return _DatumZaniku; 
            } 
            set 
            {
                if (value != _DatumZaniku) 
                {
                    _DatumZaniku = value; 
                    OnPropertyChanged("DatumZaniku"); 
                } 
            } 
        } 

        private String _InyIdentifikator; 
        [DataMember] 
        [System.ComponentModel.DataAnnotations.StringLength(30)]
        public virtual String InyIdentifikator  
        {   
            get   
            {  
                return _InyIdentifikator; 
            } 
            set 
            {
                if (value != _InyIdentifikator) 
                {
                    _InyIdentifikator = value; 
                    OnPropertyChanged("InyIdentifikator"); 
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


        public virtual string GetTableName  
        {   
            get   
            {  
                return "PO.PRAVNICKA_OSOBA"; 
            } 
        } 

	}
}