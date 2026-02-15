using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using System.Runtime.Serialization;


namespace Ditec.RIS.RFO.Dol
{
	[DataContract()]
	public partial class ZakazPobytuFilterCriteria  : BaseDataMemberFilterCriteria
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

        private Guid? _RfoObecId; 
        [DataMember] 
        public virtual Guid? RfoObecId  
        {   
            get   
            {  
                return _RfoObecId; 
            } 
            set 
            {
                if (value != _RfoObecId) 
                {
                    _RfoObecId = value; 
                    OnPropertyChanged("RfoObecId"); 
                } 
            } 
        } 

        private DateTime? _PlatnostOd; 
        [DataMember] 
        public virtual DateTime? PlatnostOd  
        {   
            get   
            {  
                return _PlatnostOd; 
            } 
            set 
            {
                if (value != _PlatnostOd) 
                {
                    _PlatnostOd = value; 
                    OnPropertyChanged("PlatnostOd"); 
                } 
            } 
        } 

        private DateTime? _PlatnostDo; 
        [DataMember] 
        public virtual DateTime? PlatnostDo  
        {   
            get   
            {  
                return _PlatnostDo; 
            } 
            set 
            {
                if (value != _PlatnostDo) 
                {
                    _PlatnostDo = value; 
                    OnPropertyChanged("PlatnostDo"); 
                } 
            } 
        } 

        private String _Poznamka; 
        [DataMember] 
        [System.ComponentModel.DataAnnotations.StringLength(2560)]
        public virtual String Poznamka  
        {   
            get   
            {  
                return _Poznamka; 
            } 
            set 
            {
                if (value != _Poznamka) 
                {
                    _Poznamka = value; 
                    OnPropertyChanged("Poznamka"); 
                } 
            } 
        } 

        private Int32? _Identifikator; 
        [DataMember] 
        public virtual Int32? Identifikator  
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
                return "FO.ZAKAZ_POBYTU"; 
            } 
        } 

	}
}