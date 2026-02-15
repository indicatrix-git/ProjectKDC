using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using System.Runtime.Serialization;


namespace Ditec.RIS.RFO.Dol
{
	[DataContract()]
	public partial class RodnePriezvisko  : BaseDataMember
	{
		
        private Guid _FyzickaOsobaId; 
        [DataMember] 
        public virtual Guid FyzickaOsobaId  
        {   
            get   
            {  
                return _FyzickaOsobaId; 
            } 
            set 
            {
                if (value != _FyzickaOsobaId) 
                {
                    _FyzickaOsobaId = value; 
                    OnPropertyChanged("FyzickaOsobaId"); 
                } 
            } 
        } 

        private String _Hodnota; 
        [DataMember] 
        [System.ComponentModel.DataAnnotations.StringLength(250)]
        public virtual String Hodnota  
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
                    OnPropertyChanged("Hodnota"); 
                } 
            } 
        } 

        private Int32 _Poradie; 
        [DataMember] 
        public virtual Int32 Poradie  
        {   
            get   
            {  
                return _Poradie; 
            } 
            set 
            {
                if (value != _Poradie) 
                {
                    _Poradie = value; 
                    OnPropertyChanged("Poradie"); 
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

        private int? _Identifikator;
        [DataMember]
        public virtual int? Identifikator
        {
            get
            {
                return _Identifikator;
            }
            set
            {
                if (value != _Identifikator)
                {
                    _Identifikator = value;
                    OnPropertyChanged("Identifikator");
                }
            }
        }

        public virtual string GetTableName  
        {   
            get   
            {  
                return "FO.RODNE_PRIEZVISKO"; 
            } 
        } 

	}
}
