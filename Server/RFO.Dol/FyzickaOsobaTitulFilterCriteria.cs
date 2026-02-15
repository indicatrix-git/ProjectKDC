using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using System.Runtime.Serialization;


namespace Ditec.RIS.RFO.Dol
{
	[DataContract()]
	public partial class FyzickaOsobaTitulFilterCriteria  : BaseDataMemberFilterCriteria
	{
		
        private Guid? _FyzickaOsobaId; 
        [DataMember] 
        public virtual Guid? FyzickaOsobaId  
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

        private Guid? _RfoTitulIk; 
        [DataMember] 
        public virtual Guid? RfoTitulIk  
        {   
            get   
            {  
                return _RfoTitulIk; 
            } 
            set 
            {
                if (value != _RfoTitulIk) 
                {
                    _RfoTitulIk = value; 
                    OnPropertyChanged("RfoTitulIk"); 
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
                return "FO.FYZICKA_OSOBA_TITUL"; 
            } 
        } 

	}
}