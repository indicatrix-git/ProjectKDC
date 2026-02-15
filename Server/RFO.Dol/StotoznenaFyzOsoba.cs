using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using System.Runtime.Serialization;


namespace Ditec.RIS.RFO.Dol
{
	[DataContract()]
	public partial class StotoznenaFyzOsoba  : BaseDataMember
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

        private Guid _FyzickaOsobaPovodnaId; 
        [DataMember] 
        public virtual Guid FyzickaOsobaPovodnaId  
        {   
            get   
            {  
                return _FyzickaOsobaPovodnaId; 
            } 
            set 
            {
                if (value != _FyzickaOsobaPovodnaId) 
                {
                    _FyzickaOsobaPovodnaId = value; 
                    OnPropertyChanged("FyzickaOsobaPovodnaId"); 
                } 
            } 
        } 

        private Int32? _EduidPovodne; 
        [DataMember] 
        public virtual Int32? EduidPovodne  
        {   
            get   
            {  
                return _EduidPovodne; 
            } 
            set 
            {
                if (value != _EduidPovodne) 
                {
                    _EduidPovodne = value; 
                    OnPropertyChanged("EduidPovodne"); 
                } 
            } 
        } 

        private String _IfoPovodne; 
        [DataMember] 
        [System.ComponentModel.DataAnnotations.StringLength(30)]
        public virtual String IfoPovodne  
        {   
            get   
            {  
                return _IfoPovodne; 
            } 
            set 
            {
                if (value != _IfoPovodne) 
                {
                    _IfoPovodne = value; 
                    OnPropertyChanged("IfoPovodne"); 
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
                return "FO.STOTOZNENA_FYZ_OSOBA"; 
            } 
        } 

	}
}
