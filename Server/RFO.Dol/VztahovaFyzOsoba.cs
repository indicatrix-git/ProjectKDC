using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using System.Runtime.Serialization;


namespace Ditec.RIS.RFO.Dol
{
	[DataContract()]
	public partial class VztahovaFyzOsoba  : BaseDataMember
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

        private Guid _FyzickaOsobaVztahId; 
        [DataMember] 
        public virtual Guid FyzickaOsobaVztahId  
        {   
            get   
            {  
                return _FyzickaOsobaVztahId; 
            } 
            set 
            {
                if (value != _FyzickaOsobaVztahId) 
                {
                    _FyzickaOsobaVztahId = value; 
                    OnPropertyChanged("FyzickaOsobaVztahId"); 
                } 
            } 
        } 

        private Guid _RfoTypRodinnehoVztahuIk; 
        [DataMember] 
        public virtual Guid RfoTypRodinnehoVztahuIk  
        {   
            get   
            {  
                return _RfoTypRodinnehoVztahuIk; 
            } 
            set 
            {
                if (value != _RfoTypRodinnehoVztahuIk) 
                {
                    _RfoTypRodinnehoVztahuIk = value; 
                    OnPropertyChanged("RfoTypRodinnehoVztahuIk"); 
                } 
            } 
        } 

        private Guid _RfoTypRoleVRodiVztahuIk; 
        [DataMember] 
        public virtual Guid RfoTypRoleVRodiVztahuIk  
        {   
            get   
            {  
                return _RfoTypRoleVRodiVztahuIk; 
            } 
            set 
            {
                if (value != _RfoTypRoleVRodiVztahuIk) 
                {
                    _RfoTypRoleVRodiVztahuIk = value; 
                    OnPropertyChanged("RfoTypRoleVRodiVztahuIk"); 
                } 
            } 
        } 

        private String _IfoVztahovejOsoby; 
        [DataMember] 
        [System.ComponentModel.DataAnnotations.StringLength(32)]
        public virtual String IfoVztahovejOsoby  
        {   
            get   
            {  
                return _IfoVztahovejOsoby; 
            } 
            set 
            {
                if (value != _IfoVztahovejOsoby) 
                {
                    _IfoVztahovejOsoby = value; 
                    OnPropertyChanged("IfoVztahovejOsoby"); 
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
                return "FO.VZTAHOVA_FYZ_OSOBA"; 
            } 
        } 

	}
}
