using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using System.Runtime.Serialization;


namespace Ditec.RIS.RFO.Dol
{
	[DataContract()]
	public partial class RegionMimoSrFilterCriteria  : BaseDataMemberFilterCriteria
	{
		
        private Guid? _UdajePobytuId; 
        [DataMember] 
        public virtual Guid? UdajePobytuId  
        {   
            get   
            {  
                return _UdajePobytuId; 
            } 
            set 
            {
                if (value != _UdajePobytuId) 
                {
                    _UdajePobytuId = value; 
                    OnPropertyChanged("UdajePobytuId"); 
                } 
            } 
        } 

        private Int32? _Poradie; 
        [DataMember] 
        public virtual Int32? Poradie  
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
                return "FO.REGION_MIMO_SR"; 
            } 
        } 

	}
}