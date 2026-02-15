using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using System.Runtime.Serialization;


namespace Ditec.RIS.RFO.Dol
{
	[DataContract()]
	public partial class FyzickaOsobaStatnaPrislusnost  : BaseDataMember
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

        private Guid _RfoStatIk; 
        [DataMember] 
        public virtual Guid RfoStatIk  
        {   
            get   
            {  
                return _RfoStatIk; 
            } 
            set 
            {
                if (value != _RfoStatIk) 
                {
                    _RfoStatIk = value; 
                    OnPropertyChanged("RfoStatIk"); 
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
                return "FO.FYZICKA_OSOBA_STATNA_PRISLUSNOST"; 
            } 
        } 

	}
}
